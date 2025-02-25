using MagicScannerLib.Models.ResponseModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xaminals.Data;

namespace Xaminals.Services
{
	public class CollectionService
	{
		private readonly HttpClient _httpClient;

		public CollectionService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IList<Collection>> GetCollectionItemsAsync()
		{
			return CollectionData.Collections;


			var response = await _httpClient.GetAsync("https://api.example.com/collections");
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<Collection>>(content);
		}


		//public async Task<Guid> PostCollectionItemsAsync()
		//{
		//	var response = await _httpClient.PostAsync("https://api.example.com/collections");
		//	response.EnsureSuccessStatusCode();

		//	var content = await response.Content.ReadAsStringAsync();
		//	return Guid.NewGuid();
		//}
	}
}

