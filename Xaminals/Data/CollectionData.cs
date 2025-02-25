using MagicScannerLib.Models.ResponseModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Xaminals.Data
{
	public static class CollectionData
	{
		public static IList<Collection> Collections { get; private set; }

		static CollectionData()
		{
			Collections = new List<Collection>
			{
				new Collection
				{
					Id =  Guid.Parse("34C1947F-B4C9-4855-AFE3-597A600AC8A4"),
					Name = "Magic: The Gathering",
					Cards = new List<Card>
					{
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Black Lotus",
							ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Gustav_chocolate.jpg/168px-Gustav_chocolate.jpg"
						},
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Mox Pearl",
							ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Gustav_chocolate.jpg/168px-Gustav_chocolate.jpg"
						},
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Black Lotus",
							ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Gustav_chocolate.jpg/168px-Gustav_chocolate.jpg"
						},
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Mox Pearl",
							ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Gustav_chocolate.jpg/168px-Gustav_chocolate.jpg"
						}
						,
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Black Lotus",
							ImageUrl = "https://example.com/black_lotus.jpg"
						},
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Mox Pearl",
							ImageUrl = "https://example.com/mox_pearl.jpg"
						},
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Black Lotus",
							ImageUrl = "https://example.com/black_lotus.jpg"
						},
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Mox Pearl",
							ImageUrl = "https://example.com/mox_pearl.jpg"
						}
						,
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Black Lotus",
							ImageUrl = "https://example.com/black_lotus.jpg"
						},
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Mox Pearl",
							ImageUrl = "https://example.com/mox_pearl.jpg"
						}
					}
				},
				new Collection
				{
					Id = Guid.Parse("62B9B6F4-CCAA-4DE0-AE7B-E1AF4E6ADD63"),
					Name = "Pokemon",
					Cards = new List<Card>
					{
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Pikachu",
							ImageUrl = "https://example.com/pikachu.jpg"
						},
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Charizard",
							ImageUrl = "https://example.com/charizard.jpg"
						}
					}
				},
				new Collection
				{
					Id = Guid.NewGuid(),
					Name = "Yu-Gi-Oh!",
					Cards = new List<Card>
					{
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Blue-Eyes White Dragon",
							ImageUrl = "https://example.com/blue_eyes_white_dragon.jpg"
						},
						new Card
						{
							Id = Guid.NewGuid(),
							Name = "Dark Magician",
							ImageUrl = "https://example.com/dark_magician.jpg"
						}
					}
				}
			};
		}
	}
}