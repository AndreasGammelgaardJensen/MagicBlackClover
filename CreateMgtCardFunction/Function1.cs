using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CreateMgtCardFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Function1))]
        public void Run([QueueTrigger("createQueue", Connection = "Endpoint=sb:::sb-magiccard.servicebus.windows.net:;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=QGIj+EE5Joow8J0hC3zrI:GnFKUIrsef6+ASbLndeEw=")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
