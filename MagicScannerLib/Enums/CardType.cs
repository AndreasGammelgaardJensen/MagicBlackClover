using MagicScannerLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicScannerLib.Enums
{
	public enum CardType
	{
		Artifact = 0,
		ArtifactEquipment = 1,
		ArtifactCreatureConstruct = 2,
		ArtifactCreatureGolem = 3,
		ArtifactCreatureMyr = 4,
		CreatureCephalidWizard = 5,
		CreatureFoxCleric = 6,
		CreatureHumanDruid = 7,
		CreatureMerfolkWizard = 8,
		CreatureNagaCleric = 9,
		CreatureRat = 10,
		CreatureSpirit = 11,
		CreatureZombieWizard = 12,
		Enchantment = 13,
		EnchantmentAura = 14,
		EnchantmentAuraCartouche = 15,
		Instant = 16,
		LegendaryArtifact = 17,
		LegendaryCreatureHumanKnight = 18,
		Sorcery = 19,
		TribalArtifactWizardEquipment = 20,
		Unknown = 999
	}
}
public static class CardTypePapper
{
	public static CardType MapStringToCardType(string type)
	{
		return type switch
		{
			"Artifact" => CardType.Artifact,
			"Artifact — Equipment" => CardType.ArtifactEquipment,
			"Artifact Creature — Construct" => CardType.ArtifactCreatureConstruct,
			"Artifact Creature — Golem" => CardType.ArtifactCreatureGolem,
			"Artifact Creature — Myr" => CardType.ArtifactCreatureMyr,
			"Creature — Cephalid Wizard" => CardType.CreatureCephalidWizard,
			"Creature — Fox Cleric" => CardType.CreatureFoxCleric,
			"Creature — Human Druid" => CardType.CreatureHumanDruid,
			"Creature — Merfolk Wizard" => CardType.CreatureMerfolkWizard,
			"Creature — Naga Cleric" => CardType.CreatureNagaCleric,
			"Creature — Rat" => CardType.CreatureRat,
			"Creature — Spirit" => CardType.CreatureSpirit,
			"Creature — Zombie Wizard" => CardType.CreatureZombieWizard,
			"Enchantment" => CardType.Enchantment,
			"Enchantment — Aura" => CardType.EnchantmentAura,
			"Enchantment — Aura Cartouche" => CardType.EnchantmentAuraCartouche,
			"Instant" => CardType.Instant,
			"Legendary Artifact" => CardType.LegendaryArtifact,
			"Legendary Creature — Human Knight" => CardType.LegendaryCreatureHumanKnight,
			"Sorcery" => CardType.Sorcery,
			"Tribal Artifact — Wizard Equipment" => CardType.TribalArtifactWizardEquipment,
			_ => CardType.Unknown
		};
	}
} 
