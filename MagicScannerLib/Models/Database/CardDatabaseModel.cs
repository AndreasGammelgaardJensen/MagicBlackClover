using System;
using System.Collections.Generic;

namespace MagicScannerLib.Models.Database
{
	public class CardDatabaseModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string ManaCost { get; set; }
		public double Cmc { get; set; }
		public int Type { get; set; }
		public string TypeDescription { get; set; }
		public string Rarity { get; set; }
		public string Set { get; set; }
		public string SetName { get; set; }
		public string Text { get; set; }
		public string Artist { get; set; }
		public string Number { get; set; }
		public string? Power { get; set; }
		public string? Toughness { get; set; }
		public string Layout { get; set; }
		public string? MultiverseId { get; set; }
		public string? ImageUrl { get; set; }
		public ICollection<ForeignNameDatabaseModel> ForeignNames { get; set; }
		public ICollection<LegalityDatabaseModel> Legalities { get; set; }
		public ICollection<PrintingDatabaseModel> Printings { get; set; }
	}

	public class ForeignNameDatabaseModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Language { get; set; }
		public int? MultiverseId { get; set; }
		public Guid CardId { get; set; }
		public CardDatabaseModel Card { get; set; }
	}

	public class LegalityDatabaseModel
	{
		public Guid Id { get; set; }
		public string Format { get; set; }
		public string LegalityStatus { get; set; }
		public Guid CardId { get; set; }
		public CardDatabaseModel Card { get; set; }
	}

	public class PrintingDatabaseModel
	{
		public Guid Id { get; set; }
		public string Set { get; set; }
		public Guid CardId { get; set; }
		public CardDatabaseModel Card { get; set; }
	}
}
