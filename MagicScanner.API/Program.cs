using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web.Resource;
using MagicScannerLib.ComputerVision;
using MagicScanner.API.ControllerExtensions;
using Microsoft.Extensions.Options;
using MagicScannerLib.Messaging.AzureServiceBus;
using MagicScannerLib.Settings;
using MagicScanner.API.Handlers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

var config = builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

builder.Services.AddSingleton<ServiceBusHandler>(sp =>
{
	var serviceBusSettings = sp.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
	return new ServiceBusHandler("Endpoint = sb://sb-magiccard.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=QGIj+EE5Joow8J0hC3zrI/GnFKUIrsef6+ASbLndeEw=");
});

builder.Services.AddDbContextFactory<DataContext>(options =>
			options.UseSqlServer(@"Server=localhost;Database=MagicScannerDB;Trusted_Connection=True;TrustServerCertificate=True;"));


builder.Services.AddScoped<IAnalyseImage,ImageTextRecognitionAnalyser>();
builder.Services.AddSingleton<HandleImageExtraction>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.AddVisionEndpoints();

app.MapGet("/weatherforecast", (HttpContext httpContext) =>
{
    httpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi()
.RequireAuthorization();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
