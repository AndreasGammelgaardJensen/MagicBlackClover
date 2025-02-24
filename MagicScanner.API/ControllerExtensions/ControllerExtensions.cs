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

			app.MapGet("/scan/{id}", async (HttpContext httpContext, Guid id) =>
			{
				//httpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
				var extractio = httpContext.RequestServices.GetRequiredService<HandleImageExtraction>();
				// Use the 'id' parameter in your logic
				var scan = await extractio.GetScanById(id);
				return Results.Ok(scan);
			})
			.WithName("GetScanById")
			.WithOpenApi(operation => new OpenApiOperation
			{
				Summary = "Scan with ID",
				Description = "Scans with the provided ID",
				Parameters = new List<OpenApiParameter>
				{
					new OpenApiParameter
					{
						Name = "id",
						In = ParameterLocation.Path,
						Required = true,
						Schema = new OpenApiSchema
						{
							Type = "string",
							Format = "uuid"
						}
					}
				}
			});


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
								Required = new HashSet<string> { "id", "image", "userid" }
							}
						}
					}
				}
			});

			return app;
		}

		public static WebApplication AddCollectionEndpoints(this WebApplication app)
		{
			var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";


			app.MapGet("/collection/{id}", async (HttpContext httpContext, Guid id) =>
			{
				//httpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

				var collectionHandler = httpContext.RequestServices.GetRequiredService<CollectionHandler>();
				var collection = await collectionHandler.GetCollection(id);

				return Results.Ok(collection);
			})
			.WithName("GetcollectionById")
			.WithOpenApi(operation => new OpenApiOperation
			{
				Summary = "Collection with ID",
				Description = "Collection with the provided ID",
				Parameters = new List<OpenApiParameter>
				{
					new OpenApiParameter
					{
						Name = "id",
						In = ParameterLocation.Path,
						Required = true,
						Schema = new OpenApiSchema
						{
							Type = "string",
							Format = "uuid"
						}
					}
				}
			});

			//Create a POST endpoint that takes a collectionID and a photo and returns the result of the analysis
			app.MapPost("/collection", async (HttpContext httpContext) =>
			{
				var analyseModel = await httpContext.MapToCollectionPostModel();


				if (analyseModel.userId == Guid.Empty)
				{
					return Results.BadRequest("Invalid request. Please provide an ID and an image file.");
				}

				var collectionHandler = httpContext.RequestServices.GetRequiredService<CollectionHandler>();
				var collectionGuid = await collectionHandler.CreateCollection(analyseModel.userId, analyseModel.collectionName);

				return Results.Ok(collectionGuid);
			})
			.WithName("Collection")
			.WithOpenApi(operation => new OpenApiOperation
			{
				Summary = "Create a collection",
				Description = "Create a collection",
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
									["name"] = new OpenApiSchema
									{
										Type = "string",
										Description = "The collection name"
									},
									["userid"] = new OpenApiSchema
									{
										Type = "string",
										Description = "The user ID"
									}
								},
								Required = new HashSet<string> { "name", "userid" }
							}
						}
					}
				}
			});

			app.MapGet("/collections/{userid}", async (HttpContext httpContext, Guid userid) =>
			{

				var collectionHandler = httpContext.RequestServices.GetRequiredService<CollectionHandler>();
				var collectionGuid = await collectionHandler.GetCollections(userid);

				return Results.Ok(collectionGuid);
			})
			.WithName("Get Collections")
			.WithOpenApi(operation => new OpenApiOperation
			{
				Summary = "Get list of collections",
				Description = "Get list of collections",
				Parameters = new List<OpenApiParameter>
				{
					new OpenApiParameter
					{
						Name = "userid",
						In = ParameterLocation.Path,
						Required = true,
						Schema = new OpenApiSchema
						{
							Type = "string",
							Format = "uuid"
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

		private static async Task<CollectionPostModel> MapToCollectionPostModel(this HttpContext context)
		{
			var form = await context.Request.ReadFormAsync();
			var collectionName = form["name"].ToString();
			var userId = Guid.Parse(form["userid"].ToString());

			// Return the model
			return new CollectionPostModel
			{
				collectionName = collectionName,
				userId = userId			};
		}
	}

	internal class AnalysePostModel
	{
		public Guid userId { get; set; }
		public Guid collectionId { get; set; }
		public BinaryData? binaryData { get; set; }
	}

	internal class CollectionPostModel
	{
		public Guid userId { get; set; }
		public string collectionName { get; set; }
	}
}
