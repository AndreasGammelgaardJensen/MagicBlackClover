using Azure.Messaging.ServiceBus;

namespace MagicScannerLib.Messaging.AzureServiceBus
{
	public class ServiceBusHandler : IDisposable
	{
		private readonly ServiceBusClient _client;
		private ServiceBusSender? _sender;
		private readonly Dictionary<string, ServiceBusSender> _receivers = new Dictionary<string, ServiceBusSender>();

		public ServiceBusHandler(string connectionString)
		{
			_client = new ServiceBusClient(connectionString);
		}


		public async Task SendMessageAsync(string message, string queueName)
		{
			_receivers.TryGetValue(queueName, out _sender);
			if (_sender == null)
			{
				_sender = _client.CreateSender(queueName);
				_receivers.Add(queueName, _sender);
			}

			await _sender.SendMessageAsync(new ServiceBusMessage(message));
		}

		public async Task SendRangeMessageAsync(List<string> message, string queueName)
		{
			_receivers.TryGetValue(queueName, out _sender);
			if (_sender == null)
			{
				_sender = _client.CreateSender(queueName);
				_receivers.Add(queueName, _sender);
			}

			foreach (var item in message)
				await _sender.SendMessageAsync(new ServiceBusMessage(item));
		}

		public async Task DeadLetterMessageAsync(ServiceBusReceiver receiver, ServiceBusReceivedMessage message, string reason, string description)
		{
			await receiver.DeadLetterMessageAsync(message, reason, description);
		}

		public void Dispose()
		{
			if (_sender != null)
				_sender.DisposeAsync();
			_client.DisposeAsync();
		}
	}
}

