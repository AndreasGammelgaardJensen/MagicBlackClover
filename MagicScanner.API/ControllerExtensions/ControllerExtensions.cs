using Microsoft.Identity.Web.Resource;
using System.Runtime.CompilerServices;
using System.IO; // Add this using directive
using System.Threading.Tasks;
using MagicScannerLib.ComputerVision;
using Microsoft.OpenApi.Models;
using MagicScanner.API.Handlers; // Add this using directive

namespace MagicScanner.API.ControllerExtensions
{
	public static class ControllerExtensions
	{
		public static WebApplication AddVisionEndpoints(this WebApplication app)
		{
			var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";
			app.MapGet("/weatherforecastExt", (HttpContext httpContext) =>
			{
				httpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

				return Results.Ok();
			})
			.WithName("GetWeatherForecastExten")
			.WithOpenApi()
			.RequireAuthorization();

			//Create a POST endpoint that takes a collectionID and a photo and returns the result of the analysis
			app.MapPost("/analyse", async (HttpContext httpContext) =>
			{
				var analyseModel = await httpContext.MapToAnalysePostModel();


				if (analyseModel.userId == Guid.Empty)
				{
					return Results.BadRequest("Invalid request. Please provide an ID and an image file.");
				}

				var analyser = httpContext.RequestServices.GetRequiredService<IAnalyseImage>();
				var extractio = httpContext.RequestServices.GetRequiredService<HandleImageExtraction>(); 
				await extractio.HandleImageExtractionHandler(analyseModel.userId, analyseModel.collectionId, analyseModel.binaryData, analyser);

				return Results.Ok();
			})
			.WithName("ImageAnalyser")
			.WithOpenApi(operation => new OpenApiOperation
			{
				Summary = "Analyse an image",
				Description = "Takes a collection ID and an image file, and returns the result of the analysis.",
				RequestBody = new OpenApiRequestBody
				{
					Content = new Dictionary<string, OpenApiMediaType>
					{
						["multipart/form-data"] = new OpenApiMediaType
						{
							Schema = new OpenApiSchema
							{
								Type = "object",
								Properties = new Dictionary<string, OpenApiSchema>
								{
									["id"] = new OpenApiSchema
									{
										Type = "string",
										Description = "The collection ID"
									},
									["userid"] = new OpenApiSchema
									{
										Type = "string",
										Description = "The user ID"
									},
									["image"] = new OpenApiSchema
									{
										Type = "string",
										Format = "binary",
										Description = "The image file"
									}
								},
								Required = new HashSet<string> { "id", "image" }
							}
						}
					}
				}
			});

			return app;
		}

		private static async Task<AnalysePostModel> MapToAnalysePostModel(this HttpContext context)
		{
			var form = await context.Request.ReadFormAsync();
			var id = Guid.Parse(form["id"].ToString());
			var file = form.Files["image"];
			var userId = Guid.Parse(form["userid"].ToString());

			using var stream = file.OpenReadStream();
			using var memoryStream = new MemoryStream();
			await stream.CopyToAsync(memoryStream);
			var binaryData = new BinaryData(memoryStream.ToArray());

			// Return the model
			return new AnalysePostModel
			{
				collectionId = id,
				userId = userId,
				binaryData = binaryData
			};
		}
	}

	internal class AnalysePostModel
	{
		public Guid userId { get; set; }
		public Guid collectionId { get; set; }
		public BinaryData? binaryData { get; set; }
	}
}
