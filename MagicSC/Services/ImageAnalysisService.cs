using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MagicSC.Services
{
	public class ImageAnalysisService
	{
		private readonly HttpClient _httpClient;

		public ImageAnalysisService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<HttpResponseMessage> UploadImageAsync(string url, Guid id, Stream imageStream, string fileName)
		{
			using var content = new MultipartFormDataContent();
			content.Add(new StringContent(id.ToString()), "id");

			var imageContent = new StreamContent(imageStream);
			imageContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
			content.Add(imageContent, "image", fileName);

			var response = await _httpClient.PostAsync(url, content);
			return response;
		}
	}
}
