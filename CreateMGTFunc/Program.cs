using DataAccess;
using MagicScannerLib.Messaging.AzureServiceBus;
using MagicScannerLib.Settings;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
		//Add DataContext
		services.AddDbContextFactory<DataContext>(options =>
		{
			options.EnableSensitiveDataLogging(false);
			options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings:SQLConnectionString"));
		});

		services.AddSingleton<ServiceBusHandler>(sp =>
		{
			var serviceBusSettings = sp.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
			return new ServiceBusHandler("");
		});

		//Add httpclient that add this https://api.magicthegathering.io/v1/ as base address
		services.AddHttpClient("MTGAPI", client =>
		{
			client.BaseAddress = new Uri("https://api.magicthegathering.io/v1/");
		});
	})
    .Build();

host.Run();