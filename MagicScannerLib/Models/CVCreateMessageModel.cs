namespace MagicScannerLib.Models
{
	public record CVCreateMessageModel
	{
		public Guid ScanItemId { get; set; }
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public Guid CollectionId { get; set; }
		public int TimesProcessed { get; set; }
	}
}