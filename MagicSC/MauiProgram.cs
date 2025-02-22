using MagicSC.Services;
using MagicSC.ViewModels;
using Microsoft.Extensions.Logging;

namespace MagicSC;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		//Add ImageAnalysisViewModel
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<ImageAnalysisViewModel>();
		builder.Services.AddHttpClient<ImageAnalysisService>(client =>
		{
			client.BaseAddress = new Uri("http://10.0.2.2:5212");
		});      //Add httpclient
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
