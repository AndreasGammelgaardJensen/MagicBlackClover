using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MagicScannerLib.Models
{
	public class MTGApiResponse
	{
		[JsonPropertyName("cards")]
		public List<Card> Cards { get; set; }
	}

	public class Card
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("manaCost")]
		public string ManaCost { get; set; }

		[JsonPropertyName("cmc")]
		public double Cmc { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("rarity")]
		public string Rarity { get; set; }

		[JsonPropertyName("set")]
		public string Set { get; set; }

		[JsonPropertyName("toughness")]
		public string Toughness { get; set; }

		[JsonPropertyName("power")]
		public string Power { get; set; }

		[JsonPropertyName("setName")]
		public string SetName { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("artist")]
		public string Artist { get; set; }

		[JsonPropertyName("number")]
		public string Number { get; set; }

		[JsonPropertyName("layout")]
		public string Layout { get; set; }

		[JsonPropertyName("multiverseid")]
		public string? MultiverseId { get; set; }

		[JsonPropertyName("imageUrl")]
		public string ImageUrl { get; set; }

		[JsonPropertyName("rulings")]
		public List<Ruling> Rulings { get; set; }

		[JsonPropertyName("foreignNames")]
		public List<ForeignName> ForeignNames { get; set; }

		[JsonPropertyName("printings")]
		public List<string> Printings { get; set; }

		[JsonPropertyName("originalText")]
		public string OriginalText { get; set; }

		[JsonPropertyName("originalType")]
		public string OriginalType { get; set; }

		[JsonPropertyName("legalities")]
		public List<Legality> Legalities { get; set; }
	}

	public class Ruling
	{
		[JsonPropertyName("date")]
		public string Date { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class ForeignName
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("flavor")]
		public string Flavor { get; set; }

		[JsonPropertyName("language")]
		public string Language { get; set; }

		[JsonPropertyName("identifiers")]
		public Identifiers Identifiers { get; set; }

		[JsonPropertyName("multiverseid")]
		public int? MultiverseId { get; set; }
	}

	public class Identifiers
	{
		[JsonPropertyName("scryfallId")]
		public Guid ScryfallId { get; set; }
	}

	public class Legality
	{
		[JsonPropertyName("format")]
		public string Format { get; set; }

		[JsonPropertyName("legality")]
		public string LegalityStatus { get; set; }
	}
}
