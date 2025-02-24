using MagicScannerLib.Models;
using System.Text.Json;
using MagicScannerLib.Helper;

namespace MagicScanner.Test
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			try
			{
				var json = LoadTestTwoCardVerwticies();

				var result = JsonSerializer.Deserialize<List<ACVOCRResultModel>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				Console.WriteLine("JSON is valid.");

				// Sort the lines based on the centroid and y-coordinate with tolerance
				var Lines = result.SelectMany(x => x.Lines);

				// Print sorted lines
				foreach (var line in Lines)
				{
					Console.WriteLine(line.Text);
				}

				//Create a list og centroids for all lines
				var centroids = Lines.Select(line => line.GetCentroid()).ToList();

				// Remove centroids that are too close to each other
				var filteredCentroids = GeometryHelper.RemoveCloseCentroids(centroids,100);

				//Find the lineopject for each remaning centriod and print the text
				foreach (var centroid in filteredCentroids)
				{
					var line = Lines.FirstOrDefault(l => l.GetCentroid().DistanceTo(centroid) < 5);
					Console.WriteLine(line.Text);
				}

				//filter the results.lines to only contain the ones that are in the filteredCentroids
				Lines = Lines.Where(line => filteredCentroids.Any(fc => line.GetCentroid().DistanceTo(fc) == 0.0)).ToList();

				//Wride alle text to a file
				File.WriteAllText("output.txt", string.Join("\n", Lines.Select(line => line.Text)));


			}
			catch (JsonException ex)
			{
				Console.WriteLine($"JSON is invalid: {ex.Message}");
			}
		}

        [Fact]
        public void Test_Deserialize_of_MGTDeveloperApi()
        {
            var responseString = @"{""cards"":[{""name"":""Ancestor's Chosen"",""manaCost"":""{5}{W}{W}"",""cmc"":7.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Cleric""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""First strike (This creature deals combat damage before creatures without first strike.)\nWhen Ancestor's Chosen enters the battlefield, you gain 1 life for each card in your graveyard."",""artist"":""Pete Venters"",""number"":""1"",""power"":""4"",""toughness"":""4"",""layout"":""normal"",""multiverseid"":""130550"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130550&type=card"",""variations"":[""b7c19924-b4bf-56fc-aa73-f586e940bd42""],""foreignNames"":[{""name"":""Ausgewählter der Ahnfrau"",""text"":""Erstschlag (Diese Kreatur fügt Kampfschaden vor Kreaturen ohne Erstschlag zu.)\nWenn der Ausgewählte der Ahnfrau ins Spiel kommt, erhältst du 1 Lebenspunkt für jede Karte in deinem Friedhof dazu."",""type"":""Kreatur — Mensch, Kleriker"",""flavor"":""„Es ist der Wille aller, und meine Hand, die ihn ausführt.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148411&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""c8d214a2-6bce-4a31-a346-7d9831ff068c"",""multiverseId"":148411},""multiverseid"":148411},{""name"":""Elegido de la Antepasada"",""text"":""Daña primero. (Esta criatura hace daño de combate antes que las criaturas sin la habilidad de dañar primero.)\nCuando el Elegido de la Antepasada entre en juego, ganas 1 vida por cada carta en tu cementerio."",""type"":""Criatura — Clérigo humano"",""flavor"":""\""La voluntad de todos, realizada por mi mano.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150317&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""3f227aa5-cacf-47d0-917a-c6de07ec4fb3"",""multiverseId"":150317},""multiverseid"":150317},{""name"":""Élu de l'Ancêtre"",""text"":""Initiative (Cette créature inflige des blessures de combat avant les créatures sans l'initiative.)\nQuand l'Élu de l'Ancêtre arrive en jeu, vous gagnez 1 point de vie pour chaque carte dans votre cimetière."",""type"":""Créature : humain et clerc"",""flavor"":""« La volonté de tous passe par ma main. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149934&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""03da122a-4487-4dcb-94e1-7029206d583c"",""multiverseId"":149934},""multiverseid"":149934},{""name"":""Prescelto dell'Antenata"",""text"":""Attacco improvviso (Questa creatura infligge danno da combattimento prima delle creature senza attacco improvviso.)\nQuando il Prescelto dell'Antenata entra in gioco, guadagni 1 punto vita per ogni carta nel tuo cimitero."",""type"":""Creatura — Chierico Umano"",""flavor"":""\""La volontà di tutti, eseguita per mano mia.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148794&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""3d078ccc-7d64-42ab-9631-f3270ec1bec1"",""multiverseId"":148794},""multiverseid"":148794},{""name"":""祖神に選ばれし者"",""text"":""先制攻撃 （このクリーチャーは先制攻撃を持たないクリーチャーよりも先に戦闘ダメージを与える。）\n祖神に選ばれし者が場に出たとき、あなたはあなたの墓地にあるカード１枚につき１点のライフを得る。"",""type"":""クリーチャー — 人間・クレリック"",""flavor"":""すべての意思を、この手で成そう。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148028&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""6eac08c0-9201-4931-9b41-f3814c8449d6"",""multiverseId"":148028},""multiverseid"":148028},{""name"":""Eleito da Ancestral"",""text"":""Iniciativa (Esta criatura causa dano de combate antes de criaturas sem a habilidade de iniciativa.)\nQuando Eleito da Ancestral entra em jogo, você ganha 1 ponto de vida para cada card em seu cemitério."",""type"":""Criatura — Humano Clérigo"",""flavor"":""\""A vontade de todos pelas minhas mãos realizada.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149551&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""fd447049-4cd9-4850-a7c0-6051f9672710"",""multiverseId"":149551},""multiverseid"":149551},{""name"":""Избранник Прародителя"",""text"":""Первый удар (Это существо наносит боевые повреждения раньше существ без Первого удара.)\nКогда Избранник Прародителя входит в игру, вы получаете 1 жизнь за каждую карту на вашем кладбище."",""type"":""Существо — Человек Священник"",""flavor"":""\""Общая воля моей рукой вершится\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149168&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""a6b3f7cd-5a8a-441c-a84a-3a5d7ad860f9"",""multiverseId"":149168},""multiverseid"":149168},{""name"":""祖灵的爱民"",""text"":""先攻（此生物会比不具先攻异能的生物提前造成战斗伤害。）\n当祖灵的爱民进场时，你坟墓场中每有一张牌，你便获得1点生命。"",""type"":""生物～人类／僧侣"",""flavor"":""「众生所愿，吾手所圆。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147645&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""1e59f85a-ca42-46a1-a6aa-01bc9afd6f6c"",""multiverseId"":147645},""multiverseid"":147645}],""printings"":[""10E"",""JUD"",""UMA""],""originalText"":""First strike\nWhen Ancestor's Chosen comes into play, you gain 1 life for each card in your graveyard."",""originalType"":""Creature - Human Cleric"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""5f8287b1-5bb6-5f4c-ad17-316a40d5bb0c""},{""name"":""Ancestor's Chosen"",""manaCost"":""{5}{W}{W}"",""cmc"":7.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Cleric""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""First strike (This creature deals combat damage before creatures without first strike.)\nWhen Ancestor's Chosen enters the battlefield, you gain 1 life for each card in your graveyard."",""flavor"":""\""The will of all, by my hand done.\"""",""artist"":""Pete Venters"",""number"":""1★"",""power"":""4"",""toughness"":""4"",""layout"":""normal"",""variations"":[""5f8287b1-5bb6-5f4c-ad17-316a40d5bb0c""],""printings"":[""10E"",""JUD"",""UMA""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""b7c19924-b4bf-56fc-aa73-f586e940bd42""},{""name"":""Angel of Mercy"",""manaCost"":""{4}{W}"",""cmc"":5.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Angel"",""types"":[""Creature""],""subtypes"":[""Angel""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying\nWhen Angel of Mercy enters the battlefield, you gain 3 life."",""flavor"":""Every tear shed is a drop of immortality."",""artist"":""Volkan Baǵa"",""number"":""2"",""power"":""3"",""toughness"":""3"",""layout"":""normal"",""multiverseid"":""129465"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129465&type=card"",""variations"":[""8fd4e2eb-3eb4-50ea-856b-ef638fa47f8a""],""foreignNames"":[{""name"":""Engel der Gnade"",""text"":""Fliegend (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)\nWenn der Engel der Gnade ins Spiel kommt, erhältst du 3 Lebenspunkte dazu."",""type"":""Kreatur — Engel"",""flavor"":""Jede ihrer Tränen ist ein Tropfen Unsterblichkeit."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148412&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""528ac57a-a8e9-448a-8cfd-65de45b46942"",""multiverseId"":148412},""multiverseid"":148412},{""name"":""Ángel de piedad"",""text"":""Vuela. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)\nCuando el Ángel de piedad entre en juego, gana 3 vidas."",""type"":""Criatura — Ángel"",""flavor"":""Cada lágrima derramada es una gota de inmortalidad."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150318&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""912bbca9-d3ef-40e7-8228-1191c6f3228f"",""multiverseId"":150318},""multiverseid"":150318},{""name"":""Ange de miséricorde"",""text"":""Vol (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée.)\nQuand l'Ange de miséricorde arrive en jeu, vous gagnez 3 points de vie."",""type"":""Créature : ange"",""flavor"":""Chaque larme versée est une goutte d'immortalité."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149935&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""64835af8-f6f3-4a7c-a944-74b4252b20c6"",""multiverseId"":149935},""multiverseid"":149935},{""name"":""Angelo della Misericordia"",""text"":""Volare (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere.)\nQuando l'Angelo della Misericordia entra in gioco, guadagni 3 punti vita."",""type"":""Creatura — Angelo"",""flavor"":""Ogni lacrima versata è una goccia d'immortalità."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148795&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""12fefa2c-88e9-4261-8924-a64156780ea6"",""multiverseId"":148795},""multiverseid"":148795},{""name"":""慈悲の天使"",""text"":""飛行 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされない。）\n慈悲の天使が場に出たとき、あなたは３点のライフを得る。"",""type"":""クリーチャー — 天使"",""flavor"":""流す涙は不死の滴。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148029&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""80d6d63b-58be-49e5-8aba-7ec6b4bd5722"",""multiverseId"":148029},""multiverseid"":148029},{""name"":""Anjo de Misericórdia"",""text"":""Voar (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)\nQuando Anjo de Misericórdia entra em jogo, você ganha 3 pontos de vida."",""type"":""Criatura — Anjo"",""flavor"":""Cada lágrima derramada é uma gota de imortalidade."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149552&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""4536c1c2-00f0-46fb-9898-d1237a9c7a2f"",""multiverseId"":149552},""multiverseid"":149552},{""name"":""Ангел Милосердия"",""text"":""Полет (Это существо может быть заблокировано только существом с Полетом или Захватом.)\nКогда Ангел Милосердия входит в игру, вы получаете 3 жизни."",""type"":""Существо — Ангел"",""flavor"":""Каждая пролитая слеза это капля бессмертия."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149169&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""58429920-9b38-454d-9cb2-912a509d7795"",""multiverseId"":149169},""multiverseid"":149169},{""name"":""慈悲天使"",""text"":""飞行（只有具飞行或延势异能的生物才能阻挡它。）\n当慈悲天使进场时，你获得3点生命。"",""type"":""生物～天使"",""flavor"":""每颗泪珠都是一滴不朽。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147646&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""18bd3568-e398-44d7-9ae2-002acfb17324"",""multiverseId"":147646},""multiverseid"":147646}],""printings"":[""10E"",""8ED"",""9ED"",""DDC"",""DVD"",""IMA"",""INV"",""JMP"",""P02"",""PLST"",""PS11"",""PSAL"",""S99""],""originalText"":""Flying\nWhen Angel of Mercy comes into play, you gain 3 life."",""originalType"":""Creature - Angel"",""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""57aaebc1-850c-503d-9f6e-bb8d00d8bf7c""},{""name"":""Angel of Mercy"",""manaCost"":""{4}{W}"",""cmc"":5.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Angel"",""types"":[""Creature""],""subtypes"":[""Angel""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying\nWhen Angel of Mercy enters the battlefield, you gain 3 life."",""flavor"":""Every tear shed is a drop of immortality."",""artist"":""Volkan Baǵa"",""number"":""2★"",""power"":""3"",""toughness"":""3"",""layout"":""normal"",""variations"":[""57aaebc1-850c-503d-9f6e-bb8d00d8bf7c""],""printings"":[""10E"",""8ED"",""9ED"",""DDC"",""DVD"",""IMA"",""INV"",""JMP"",""P02"",""PLST"",""PS11"",""PSAL"",""S99""],""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""8fd4e2eb-3eb4-50ea-856b-ef638fa47f8a""},{""name"":""Angelic Blessing"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Sorcery"",""types"":[""Sorcery""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Target creature gets +3/+3 and gains flying until end of turn. (It can't be blocked except by creatures with flying or reach.)"",""flavor"":""Only the warrior who can admit mortal weakness will be bolstered by immortal strength."",""artist"":""Mark Zug"",""number"":""3"",""layout"":""normal"",""multiverseid"":""129711"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129711&type=card"",""variations"":[""c5655330-5131-5f40-9d3e-0549d88c6e9e""],""foreignNames"":[{""name"":""Himmlischer Segen"",""text"":""Eine Kreatur deiner Wahl erhält +3/+3 und Flugfähigkeit bis zum Ende des Zuges. (Sie kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)"",""type"":""Hexerei"",""flavor"":""Nur demjenigen Krieger, der sterbliche Schwäche zugeben kann, wird unsterbliche Stärke gewährt."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148414&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""9e19c048-f825-48ba-9957-dbf5e133e280"",""multiverseId"":148414},""multiverseid"":148414},{""name"":""Bendición angélica"",""text"":""La criatura objetivo obtiene +3/+3 y gana la habilidad de volar hasta el final del turno. (No puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)"",""type"":""Conjuro"",""flavor"":""Sólo el guerrero que pueda admitir debilidad mortal será auxiliado con fuerza inmortal."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150319&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""5f4da2bd-db8d-4eb6-9ff6-8cbcd49a9463"",""multiverseId"":150319},""multiverseid"":150319},{""name"":""Bénédiction angélique"",""text"":""La créature ciblée gagne +3/+3 et acquiert le vol jusqu'à la fin du tour. (Elle ne peut être bloquée que par des créatures avec le vol ou la portée.)"",""type"":""Rituel"",""flavor"":""Seul le guerrier qui admet une faiblesse mortelle peut recevoir la force immortelle."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149936&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""42cdc454-8326-4386-9e79-381e6ed6136c"",""multiverseId"":149936},""multiverseid"":149936},{""name"":""Benedizione Angelica"",""text"":""Una creatura bersaglio prende +3/+3 e ha volare fino alla fine del turno. (Non può essere bloccata tranne che da creature con volare o raggiungere.)"",""type"":""Stregoneria"",""flavor"":""Solo il guerriero capace di ammettere la propria fragilità mortale sarà sostenuto dalla forza immortale."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148797&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""03ca8824-158c-4fee-89c8-e0dd427f2c9d"",""multiverseId"":148797},""multiverseid"":148797},{""name"":""天使の祝福"",""text"":""クリーチャー１体を対象とする。それはターン終了時まで＋３/＋３の修整を受けるとともに飛行を得る。 （それは飛行や到達を持たないクリーチャーによってブロックされない。）"",""type"":""ソーサリー"",""flavor"":""必滅の身であることを知っている戦士だけが、不滅の力に支えられる。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148031&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""cc76004c-89b4-485d-9f53-c68e1818f67e"",""multiverseId"":148031},""multiverseid"":148031},{""name"":""Bênção Angelical"",""text"":""A criatura alvo recebe +3/+3 e ganha a habilidade de voar até o final do turno. (Ela só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)"",""type"":""Feitiço"",""flavor"":""Somente o guerreiro que for capaz de admitir sua própria fragilidade mortal será favorecido com força imortal."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149553&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""93e90c33-4634-4742-9741-9832d620c3ee"",""multiverseId"":149553},""multiverseid"":149553},{""name"":""Благословение Ангела"",""text"":""Целевое существо получает +3/+3 и Полет до конца хода. (Оно может быть заблокировано только существом с Полетом или Захватом.)"",""type"":""Волшебство"",""flavor"":""Только тот воин, что признает в себе слабости смертных, будет наделен силой бессмертных."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149170&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""401bb417-858e-4ad4-b14b-4b96119d1c59"",""multiverseId"":149170},""multiverseid"":149170},{""name"":""天使的祝福"",""text"":""目标生物得+3/+3并获得飞行异能直到回合结束。 （只有具飞行或延势异能的生物才能阻挡它。）"",""type"":""法术"",""flavor"":""只有勇于正视自身致命弱点的战士，才能受到超凡力量的庇护。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147648&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""0aea70ec-2317-4856-858b-62f44b29c30f"",""multiverseId"":147648},""multiverseid"":147648}],""printings"":[""10E"",""9ED"",""EXO"",""P02"",""POR"",""PS11"",""S00"",""S99"",""TPR""],""originalText"":""Target creature gets +3/+3 and gains flying until end of turn."",""originalType"":""Sorcery"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""55bd38ca-dc73-5c06-8f80-a6ddd2f44382""},{""name"":""Angelic Blessing"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Sorcery"",""types"":[""Sorcery""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Target creature gets +3/+3 and gains flying until end of turn. (It can't be blocked except by creatures with flying or reach.)"",""flavor"":""Only the warrior who can admit mortal weakness will be bolstered by immortal strength."",""artist"":""Mark Zug"",""number"":""3★"",""layout"":""normal"",""variations"":[""55bd38ca-dc73-5c06-8f80-a6ddd2f44382""],""printings"":[""10E"",""9ED"",""EXO"",""P02"",""POR"",""PS11"",""S00"",""S99"",""TPR""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""c5655330-5131-5f40-9d3e-0549d88c6e9e""},{""name"":""Angelic Chorus"",""manaCost"":""{3}{W}{W}"",""cmc"":5.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment"",""types"":[""Enchantment""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Whenever a creature enters the battlefield under your control, you gain life equal to its toughness."",""flavor"":""The harmony of the glorious is a dirge to the wicked."",""artist"":""Jim Murray"",""number"":""4"",""layout"":""normal"",""multiverseid"":""129710"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129710&type=card"",""rulings"":[{""date"":""2004-10-04"",""text"":""Angelic Chorus does not trigger when creatures phase in or change controllers.""},{""date"":""2004-10-04"",""text"":""This does not trigger on a permanent being turned into a creature. That is just a permanent changing type, not something entering the battlefield.""}],""foreignNames"":[{""name"":""Choral der Engel"",""text"":""Immer wenn ein Kreatur unter deiner Kontrolle ins Spiel kommt, erhältst du soviele Lebenspunkte dazu, wie ihre Widerstandskraft beträgt."",""type"":""Verzauberung"",""flavor"":""Dieser Einklang in Herrlichkeit ist ein Trauergesang für das Böse."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148415&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""b864a3f3-a863-4bcf-8451-c4583e152caf"",""multiverseId"":148415},""multiverseid"":148415},{""name"":""Coro angélico"",""text"":""Siempre que una criatura entre en juego bajo tu control, ganas una cantidad de vida igual a su resistencia."",""type"":""Encantamiento"",""flavor"":""La armonía de lo glorioso es un canto fúnebre para los malvados."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150320&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""65950426-5610-4e50-8ab4-b637b6868d36"",""multiverseId"":150320},""multiverseid"":150320},{""name"":""Chœur angélique"",""text"":""À chaque fois qu'une créature arrive en jeu sous votre contrôle, vous gagnez autant de points de vie que son endurance."",""type"":""Enchantement"",""flavor"":""L'harmonie de la gloire est le chant funèbre du mal."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149937&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""a9f9e4ca-d127-4140-9541-a538e2902020"",""multiverseId"":149937},""multiverseid"":149937},{""name"":""Coro Angelico"",""text"":""Ogniqualvolta una creatura entra in gioco sotto il tuo controllo, guadagni un ammontare di punti vita pari alla sua costituzione."",""type"":""Incantesimo"",""flavor"":""L'armonia dei gloriosi è un canto funebre per i malvagi."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148798&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""f3fa73dc-73c0-40e1-9f54-5db1ee886912"",""multiverseId"":148798},""multiverseid"":148798},{""name"":""天使の合唱"",""text"":""いずれかのクリーチャーがあなたのコントロール下で場に出るたび、あなたはそれのタフネスに等しい点数のライフを得る。"",""type"":""エンチャント"",""flavor"":""栄光ある者達の合唱は、邪悪なる者達の葬送歌。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148032&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""f9ac6756-fe63-4ebc-ac78-716c98d95741"",""multiverseId"":148032},""multiverseid"":148032},{""name"":""Coro Angelical"",""text"":""Toda vez que uma criatura entra em jogo sob seu controle, você ganha uma quantidade de pontos de vida igual à resistência dela."",""type"":""Encantamento"",""flavor"":""A harmonia do glorioso é um lamento para o perverso."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149554&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""5455dd8c-3144-4c99-97be-0678fa2360e8"",""multiverseId"":149554},""multiverseid"":149554},{""name"":""Хор Ангелов"",""text"":""Каждый раз, когда в игру входит существо под вашим контролем, вы получаете количество жизни, равное его выносливости."",""type"":""Чары"",""flavor"":""Песнь блаженных это погребальный плач для ушей нечестивых."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149171&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""922e3cb8-231f-4069-90c4-50b03bbb47ae"",""multiverseId"":149171},""multiverseid"":149171},{""name"":""天使的合唱"",""text"":""每当一个生物在你的操控下进场时，你获得等同于其防御力的生命。"",""type"":""结界"",""flavor"":""荣光声韵乃为恶者挽歌。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147649&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""f72ef1ec-e1b6-4315-8982-d4cf13c20c4c"",""multiverseId"":147649},""multiverseid"":147649}],""printings"":[""10E"",""BBD"",""USG""],""originalText"":""Whenever a creature comes into play under your control, you gain life equal to its toughness."",""originalType"":""Enchantment"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""3b77bb52-4181-57f5-b3cd-f3a15b95aa29""},{""name"":""Angelic Wall"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Wall"",""types"":[""Creature""],""subtypes"":[""Wall""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Defender (This creature can't attack.)\nFlying"",""flavor"":""\""The Ancestor protects us in ways we can't begin to comprehend.\""\n—Mystic elder"",""artist"":""John Avon"",""number"":""5"",""power"":""0"",""toughness"":""4"",""layout"":""normal"",""multiverseid"":""129671"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129671&type=card"",""variations"":[""60b93108-8790-591b-844e-c3d311698767""],""foreignNames"":[{""name"":""Mauer der Engel"",""text"":""Verteidiger, Fliegend (Diese Kreatur kann nicht angreifen und kann fliegende Kreaturen blocken.)"",""type"":""Kreatur — Mauer"",""flavor"":""„Die Ahnfrau beschützt uns in einer Weise, die wir kaum erfassen können. —Älterer Mystiker"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148416&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""05e5beb5-b587-4ab7-a8f0-efd508b789dc"",""multiverseId"":148416},""multiverseid"":148416},{""name"":""Muro angelical"",""text"":""Defensor, vuela. (Esta criatura no puede atacar y puede bloquear criaturas que vuelan.)"",""type"":""Criatura — Muro"",""flavor"":""\""La Antepasada nos protege en maneras que distan mucho de ser comprendidas.\"" —Anciano místico"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150321&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""ba9361fa-883b-42f0-b464-b6789dad8c87"",""multiverseId"":150321},""multiverseid"":150321},{""name"":""Mur angélique"",""text"":""Défenseur, vol (Cette créature ne peut pas attaquer et elle peut bloquer les créatures avec le vol.)"",""type"":""Créature : mur"",""flavor"":""« L'Ancêtre nous protège par des moyens que nous commençons seulement à comprendre. » —Un doyen des mystiques"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149938&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""7deb4772-834a-4112-a2ca-9f0a8a3647f1"",""multiverseId"":149938},""multiverseid"":149938},{""name"":""Muro Angelico"",""text"":""Difensore, volare (Questa creatura non può attaccare, e può bloccare le creature con volare.)"",""type"":""Creatura — Muro"",""flavor"":""\""Gli Antenati ci proteggono in un modo che non possiamo neppure comprendere.\"" —Anziano mistico"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148799&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""a4fea532-0ab3-4398-b04c-8c9973610c96"",""multiverseId"":148799},""multiverseid"":148799},{""name"":""天使の壁"",""text"":""防衛、飛行 （このクリーチャーは攻撃できず、飛行を持つクリーチャーをブロックできる。）"",""type"":""クリーチャー — 壁"",""flavor"":""祖神様は我らには理解できないやりかたで我らを守ってくださっている。 ――秘教の古老"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148033&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""5c842d9e-ad66-44ae-8e09-606e32afafb7"",""multiverseId"":148033},""multiverseid"":148033},{""name"":""Barreira Angelical"",""text"":""Defensor, voar (Esta criatura não pode atacar, e pode bloquear criaturas com a habilidade de voar.)"",""type"":""Criatura — Barreira"",""flavor"":""\""A Ancestral nos protege de maneiras que nos é difícil começar a compreender.\"" — Ancião místico"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149555&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""25f1dab6-da59-444b-bdab-746612748389"",""multiverseId"":149555},""multiverseid"":149555},{""name"":""Стена Ангела"",""text"":""Защитник, Полет (Это существо не может атаковать и может блокировать существа с Полетом.)"",""type"":""Существо — Стена"",""flavor"":""\""Непостижимы пути, избираемые Прародителем, чтобы защитить нас\"". — Старший жрец"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149172&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""902e61b3-074f-4362-9c76-717103f1466a"",""multiverseId"":149172},""multiverseid"":149172},{""name"":""天使圣墙"",""text"":""守军，飞行（此生物不能攻击，且能阻挡具飞行异能的生物。）"",""type"":""生物～墙"",""flavor"":""「圣祖灵以无从领略的形式保护着我们。」 ～秘教长老"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147650&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""bffe99fc-4088-445c-a7ed-0d2cecfe84c5"",""multiverseId"":147650},""multiverseid"":147650}],""printings"":[""10E"",""AVR"",""M14"",""ODY"",""P02""],""originalText"":""Defender, flying"",""originalType"":""Creature - Wall"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""fadda48c-6226-5ac5-a2b9-e9170d2017cd""},{""name"":""Angelic Wall"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Wall"",""types"":[""Creature""],""subtypes"":[""Wall""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Defender (This creature can't attack.)\nFlying"",""flavor"":""\""The Ancestor protects us in ways we can't begin to comprehend.\""\n—Mystic elder"",""artist"":""John Avon"",""number"":""5★"",""power"":""0"",""toughness"":""4"",""layout"":""normal"",""variations"":[""fadda48c-6226-5ac5-a2b9-e9170d2017cd""],""printings"":[""10E"",""AVR"",""M14"",""ODY"",""P02""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""60b93108-8790-591b-844e-c3d311698767""},{""name"":""Aura of Silence"",""manaCost"":""{1}{W}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment"",""types"":[""Enchantment""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Artifact and enchantment spells your opponents cast cost {2} more to cast.\nSacrifice Aura of Silence: Destroy target artifact or enchantment."",""flavor"":""Not all silences are easily broken."",""artist"":""D. Alexander Gregory"",""number"":""6"",""layout"":""normal"",""multiverseid"":""132127"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132127&type=card"",""rulings"":[{""date"":""2004-10-04"",""text"":""Aura of Silence affects all opponents in a multiplayer game.""}],""foreignNames"":[{""name"":""Aura des Schweigens"",""text"":""Artefakt- und Verzauberungszaubersprüche, die deine Gegner spielen, kosten beim Ausspielen {2} mehr.\nOpfere die Aura des Schweigens: Zerstöre ein Artefakt oder eine Verzauberung deiner Wahl."",""type"":""Verzauberung"",""flavor"":""Nicht jede Stille kann man leicht unterbrechen."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148422&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""5db2d3f5-95a3-46ee-baa8-e125a9053fc0"",""multiverseId"":148422},""multiverseid"":148422},{""name"":""Aura de silencio"",""text"":""A tus oponentes les cuesta {2} más jugar hechizos de artefacto y de encantamiento.\nSacrificar el Aura de silencio: Destruye el artefacto o encantamiento objetivo."",""type"":""Encantamiento"",""flavor"":""No todos los silencios se rompen fácilmente."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150322&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""5dcc05a8-cb42-44cf-82bf-2d21899d8482"",""multiverseId"":150322},""multiverseid"":150322},{""name"":""Aura de silence"",""text"":""Les sorts d'artefact et d'enchantement que vos adversaires jouent coûtent {2} de plus à jouer.\nSacrifiez l'Aura de silence : Détruisez l'artefact ou l'enchantement ciblé."",""type"":""Enchantement"",""flavor"":""« Le silence fait plus peur que les cris. »Jean Cocteau, Antigone"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149939&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""b3843064-4357-4ebc-b26b-f40b451bfcc2"",""multiverseId"":149939},""multiverseid"":149939},{""name"":""Aura di Silenzio"",""text"":""Le magie artefatto e incantesimo che giocano i tuoi avversari costano {2} in più per essere giocate.\nSacrifica l'Aura di Silenzio: Distruggi un artefatto o un incantesimo bersaglio."",""type"":""Incantesimo"",""flavor"":""Non tutti i silenzi vengono interrotti facilmente."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148805&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""1f9b0c70-e0f3-4a9c-ae43-5995d44b1966"",""multiverseId"":148805},""multiverseid"":148805},{""name"":""沈黙のオーラ"",""text"":""あなたの対戦相手がプレイするアーティファクト呪文やエンチャント呪文は、それをプレイするためのコストが{2}多くなる。\n沈黙のオーラを生け贄に捧げる：アーティファクト１つかエンチャント１つを対象とし、それを破壊する。"",""type"":""エンチャント"",""flavor"":""すべての沈黙が容易く破られるわけではない。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148039&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""fedfea1c-9cc8-48eb-889b-31a8e8de3408"",""multiverseId"":148039},""multiverseid"":148039},{""name"":""Aura de Silêncio"",""text"":""As mágicas de artefato e de encantamento que seus oponentes jogam custam {2} a mais para serem jogadas.\nSacrifique Aura de Silêncio: Destrua o artefato ou encantamento alvo."",""type"":""Encantamento"",""flavor"":""Nem todos os silêncios são facilmente rompidos."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149556&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""3db33ab5-a057-4efb-ab80-075f557b6765"",""multiverseId"":149556},""multiverseid"":149556},{""name"":""Аура Молчания"",""text"":""Заклинания артефактов и чар, разыгрываемые вашими оппонентами, стоят на {2} больше.\nПожертвуйте Ауру Молчания: Уничтожьте целевой артефакт или чары."",""type"":""Чары"",""flavor"":""Не всякое молчание легко нарушить."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149173&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""15dfdc04-b0f7-486a-ac7f-1c59c5a69620"",""multiverseId"":149173},""multiverseid"":149173},{""name"":""静寂灵气"",""text"":""对手使用的神器和结界咒语费用增加{2}来使用。\n牺牲静寂灵气：消灭目标神器或结界。"",""type"":""结界"",""flavor"":""寂静不总能轻易打破。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147656&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""777be5c4-2eee-45f0-9235-235ce1b7a8bb"",""multiverseId"":147656},""multiverseid"":147656}],""printings"":[""10E"",""C15"",""F02"",""PLST"",""PRM"",""TD0"",""WC98"",""WTH""],""originalText"":""Artifact and enchantment spells your opponents play cost {2} more to play.\nSacrifice Aura of Silence: Destroy target artifact or enchantment."",""originalType"":""Enchantment"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""fac6ad26-f8c2-51bd-9f6a-a1b0940b4cef""},{""name"":""Aven Cloudchaser"",""manaCost"":""{3}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Bird Soldier"",""types"":[""Creature""],""subtypes"":[""Bird"",""Soldier""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying (This creature can't be blocked except by creatures with flying or reach.)\nWhen Aven Cloudchaser enters the battlefield, destroy target enchantment."",""artist"":""Justin Sweet"",""number"":""7"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""129470"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129470&type=card"",""variations"":[""6adaf14d-43e3-521a-adf1-960c808e5b1a""],""foreignNames"":[{""name"":""Avior-Wolkenjäger"",""text"":""Fliegend (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)\nWenn der Avior-Wolkenjäger ins Spiel kommt, zerstöre eine Verzauberung deiner Wahl."",""type"":""Kreatur — Vogel, Soldat"",""flavor"":""„Als es zur Neuverteilung kam, bat der Adler darum, Mensch werden zu dürfen. Die Ahnfrau gewährte das Gebet zur Hälfte.\"" —Nomadenlegende"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148424&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""4d373600-f386-470a-805b-3ed5fc2d5cc0"",""multiverseId"":148424},""multiverseid"":148424},{""name"":""Cazanubes aven"",""text"":""Vuela. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)\nCuando el Cazanubes aven entre en juego, destruye el encantamiento objetivo."",""type"":""Criatura — Soldado ave"",""flavor"":""\""En la Nueva Repartición, Águila suplicó ser humano. La Antepasada le concedió la mitad de esa súplica.\"" —Mito nómada"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150323&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""9a051f4b-1198-4724-9fcc-98ff393336e1"",""multiverseId"":150323},""multiverseid"":150323},{""name"":""Avemain chasse-nuage"",""text"":""Vol (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée.)\nQuand l'Avemain chasse-nuage arrive en jeu, détruisez l'enchantement ciblé."",""type"":""Créature : oiseau et soldat"",""flavor"":""« Lors de la Grande Réattribution, les aigles demandèrent à devenir humains. L'Ancêtre exauça cette prière à moitié. » —Mythe des nomades"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149940&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""6d8fb627-197b-47f0-9655-bf16606fc5a5"",""multiverseId"":149940},""multiverseid"":149940},{""name"":""Caccianubi Aviano"",""text"":""Volare (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere.)\nQuando il Caccianubi Aviano entra in gioco, distruggi un incantesimo bersaglio."",""type"":""Creatura — Uccello Soldato"",""flavor"":""\""Durante la Ridistribuzione, le Aquile chiesero di diventare umane. Gli Antenati esaudirono solo in parte la loro preghiera.\"" —Mito nomade"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148807&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""51f4eec5-245e-46b3-b4db-f6364738847a"",""multiverseId"":148807},""multiverseid"":148807},{""name"":""雲を追うエイヴン"",""text"":""飛行 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされない。）\n雲を追うエイヴンが場に出たとき、エンチャント１つを対象とし、それを破壊する。"",""type"":""クリーチャー — 鳥・兵士"",""flavor"":""祖神が改めて生き物の数を決めたとき、鷲は人間になりたいと希望した。 祖神はその祈りを半分だけかなえてやった。 ――遊牧の民の神話"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148041&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""51c86b1e-40cf-467b-9d39-f9e7434ea111"",""multiverseId"":148041},""multiverseid"":148041},{""name"":""Caça-Nuvens Aviano"",""text"":""Voar (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)\nQuando Caça-Nuvens Aviano entrar em jogo, destrua o encantamento alvo."",""type"":""Criatura — Ave Soldado"",""flavor"":""\""Na nova Repartição, as Águias imploraram para ser humanas. A Ancestral concedeu metade daquela súplica.\"" — Mito dos nômades"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149557&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""1d3726ee-d237-4e4e-b246-2f3e24b5f11e"",""multiverseId"":149557},""multiverseid"":149557},{""name"":""Загонщик Облаков"",""text"":""Полет (Это существо может быть заблокировано только существом с Полетом или Захватом.)\nКогда Загонщик Облаков входит в игру, уничтожьте целевые чары."",""type"":""Существо — Птица Солдат"",""flavor"":""\""На Великом распределении Орел умолял о том, чтобы стать человеком. Но Прародитель пожаловал лишь половину испрошенного\"". — Миф кочевников"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149174&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""c3183808-8809-455b-8b72-27c447984059"",""multiverseId"":149174},""multiverseid"":149174},{""name"":""艾文逐云战士"",""text"":""飞行（只有具飞行或延势异能的生物才能阻挡它。）\n当艾文逐云战士进场时，消灭目标结界。"",""type"":""生物～鸟／士兵"",""flavor"":""「在分判日时，老鹰乞求成为人类。 圣祖灵仅允诺了半份祈愿。」 ～游牧人神话"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147658&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""3e1b42aa-81a4-4b5e-9565-2faf89ebe537"",""multiverseId"":147658},""multiverseid"":147658}],""printings"":[""10E"",""8ED"",""9ED"",""ODY""],""originalText"":""Flying\nWhen Aven Cloudchaser comes into play, destroy target enchantment."",""originalType"":""Creature - Bird Soldier"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""8ac972b5-9f6e-5cc8-91c3-b9a40a98232e""},{""name"":""Aven Cloudchaser"",""manaCost"":""{3}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Bird Soldier"",""types"":[""Creature""],""subtypes"":[""Bird"",""Soldier""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying (This creature can't be blocked except by creatures with flying or reach.)\nWhen Aven Cloudchaser enters the battlefield, destroy target enchantment."",""flavor"":""\""At the Reapportionment, Eagle begged to be human. The Ancestor granted half that prayer.\""\n—Nomad myth"",""artist"":""Justin Sweet"",""number"":""7★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""8ac972b5-9f6e-5cc8-91c3-b9a40a98232e""],""printings"":[""10E"",""8ED"",""9ED"",""ODY""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""6adaf14d-43e3-521a-adf1-960c808e5b1a""},{""name"":""Ballista Squad"",""manaCost"":""{3}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Rebel"",""types"":[""Creature""],""subtypes"":[""Human"",""Rebel""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{X}{W}, {T}: Ballista Squad deals X damage to target attacking or blocking creature."",""flavor"":""The perfect antidote for a tightly packed formation."",""artist"":""Matthew D. Wilson"",""number"":""8"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""129477"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129477&type=card"",""foreignNames"":[{""name"":""Ballistaeinheit"",""text"":""{X}{W}, {T}: Die Ballistaeinheit fügt einer angreifenden oder blockenden Kreatur deiner Wahl X Schadenspunkte zu."",""type"":""Kreatur — Mensch, Rebell"",""flavor"":""Das perfekte Gegenmittel zu einer dichten Formation."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148427&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""0e2843f1-43b1-4cc8-8d49-2ca3ef745b15"",""multiverseId"":148427},""multiverseid"":148427},{""name"":""Escuadra de ballesta"",""text"":""{X}{W}, {T}: La Escuadra de ballesta hace X puntos de daño a la criatura atacante o bloqueadora objetivo."",""type"":""Criatura — Rebelde humano"",""flavor"":""Es el antídoto perfecto para una formación apretada."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150324&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""2e3a5e60-9e4d-45d3-8dd7-420066dd2c78"",""multiverseId"":150324},""multiverseid"":150324},{""name"":""Escouade de balistes"",""text"":""{X}{W}, {T} : L'Escouade de balistes inflige X blessures à la créature attaquante ou bloqueuse ciblée."",""type"":""Créature : humain et rebelle"",""flavor"":""L'antidote parfait à la formation serrée."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149941&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""c16f09db-f2f0-46c2-82a2-6e22149a9c24"",""multiverseId"":149941},""multiverseid"":149941},{""name"":""Unità Balista"",""text"":""{X}{W}, {T}: L'Unità Balista infligge X danni a una creatura attaccante o bloccante bersaglio."",""type"":""Creatura — Ribelle Umano"",""flavor"":""L'antidoto perfetto per una formazione estremamente compatta."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148810&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""bdb7250b-75f3-418c-8b7f-6db1d9c45083"",""multiverseId"":148810},""multiverseid"":148810},{""name"":""バリスタ班"",""text"":""{X}{W}, {T}：攻撃かブロックしているクリーチャー１体を対象とする。バリスタ班はそれにＸ点のダメージを与える。"",""type"":""クリーチャー — 人間・レベル"",""flavor"":""密集した隊形のための完璧な対策だ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148044&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""00a527b8-b80e-41e8-b007-b97b008d063b"",""multiverseId"":148044},""multiverseid"":148044},{""name"":""Esquadrão de Balista"",""text"":""{X}{W}, {T}: Esquadrão de Balista causa X pontos de dano à criatura alvo atacante ou bloqueadora."",""type"":""Criatura — Humano Rebelde"",""flavor"":""O antídoto perfeito contra as formações muito cerradas."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149558&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""b6d7b966-fd99-4131-92e0-a3de4ddd1baf"",""multiverseId"":149558},""multiverseid"":149558},{""name"":""Баллиста"",""text"":""{X}{W}, {T}: Баллиста наносит X повреждений целевому атакующему или блокирующему существу."",""type"":""Существо — Человек Повстанец"",""flavor"":""Прекрасное средство против сплоченных отрядов."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149175&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""a52a70ca-08fa-44a5-8444-be2ac3dfb4fd"",""multiverseId"":149175},""multiverseid"":149175},{""name"":""巨弩小队"",""text"":""{X}{W}，{T}：巨弩小队对目标进行攻击或阻挡的生物造成X点伤害。"",""type"":""生物～人类／反抗军"",""flavor"":""对付坚实紧密的阵型之良方。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147661&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""daac7934-91ce-40b8-bec3-3cc0297cd56d"",""multiverseId"":147661},""multiverseid"":147661}],""printings"":[""10E"",""9ED"",""MMQ""],""originalText"":""{X}{W}, {T}: Ballista Squad deals X damage to target attacking or blocking creature."",""originalType"":""Creature - Human Rebel"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""a69b404f-144a-5317-b10e-7d9dce135b24""},{""name"":""Bandage"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Prevent the next 1 damage that would be dealt to any target this turn.\nDraw a card."",""flavor"":""Life is measured in inches. To a healer, every one of those inches is precious."",""artist"":""Rebecca Guay"",""number"":""9"",""layout"":""normal"",""multiverseid"":""132106"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132106&type=card"",""rulings"":[{""date"":""2007-07-15"",""text"":""You draw the card when Bandage resolves, not when the damage is actually prevented.""}],""foreignNames"":[{""name"":""Verband"",""text"":""Verhindere den nächsten 1 Schadenspunkt, der in diesem Zug einer Kreatur oder einem Spieler deiner Wahl zugefügt würde.\nZiehe eine Karte."",""type"":""Spontanzauber"",""flavor"":""Das Leben wird in Spannen gemessen. Für einen Heiler ist jede dieser Spannen wertvoll."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148428&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""5cd2f12c-7ffc-42d9-b467-75cb3d929541"",""multiverseId"":148428},""multiverseid"":148428},{""name"":""Vendaje"",""text"":""Prevén el siguiente 1 punto de daño que se le fuera a hacer a la criatura o jugador objetivo este turno.\nRoba una carta."",""type"":""Instantáneo"",""flavor"":""La vida se mide en centímetros. Para un sanador, cada uno de esos centímetros es precioso."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150325&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""afb47421-63aa-436c-b5b1-1b7821c8649e"",""multiverseId"":150325},""multiverseid"":150325},{""name"":""Bandage"",""text"":""Prévenez, ce tour-ci, la prochaine 1 blessure qui devrait être infligée à une cible, créature ou joueur.\nPiochez une carte."",""type"":""Éphémère"",""flavor"":""La vie se mesure en centimètres. Pour le guérisseur, chacun d'eux est précieux."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149942&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""33790270-ddb6-449a-8e4d-1da44e1fb988"",""multiverseId"":149942},""multiverseid"":149942},{""name"":""Fasciatura"",""text"":""Previeni il prossimo punto danno che verrebbe inflitto a una creatura o a un giocatore bersaglio in questo turno.\nPesca una carta."",""type"":""Istantaneo"",""flavor"":""La vita è una questione di centimetri. Per un guaritore, ogni centimetro è prezioso."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148811&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""2f75c312-5b80-4808-9d50-2b8636a736df"",""multiverseId"":148811},""multiverseid"":148811},{""name"":""目かくし"",""text"":""クリーチャー１体かプレイヤー１人を対象とする。このターン、次にそれに与えられるダメージを１点軽減する。\nカードを１枚引く。"",""type"":""インスタント"",""flavor"":""命にとっては、一寸先は闇だ。 癒し手にとっては、その一寸が貴重なのだ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148045&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""000d42b4-1b62-4c1b-b3b0-514a100cd136"",""multiverseId"":148045},""multiverseid"":148045},{""name"":""Atadura"",""text"":""Previna o próximo 1 ponto de dano que seria causado à criatura ou ao jogador alvo neste turno.\nCompre um card."",""type"":""Mágica Instantânea"",""flavor"":""A vida é medida em centímetros. Para um curandeiro, cada um desses centímetros é precioso."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149559&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""6cd4689f-f693-4cb7-a744-f9129d0e7dcb"",""multiverseId"":149559},""multiverseid"":149559},{""name"":""Повязка"",""text"":""Предотвратите следующее 1 повреждение, которое будет нанесено целевому существу или игроку в этом ходу.\nВозьмите карту."",""type"":""Мгновенное заклинание"",""flavor"":""Жизнь измеряется пядями. Настоящий целитель дорожит каждой из них."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149176&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""94ab5428-92a5-4cd5-87da-8320ad1a8f1e"",""multiverseId"":149176},""multiverseid"":149176},{""name"":""包扎"",""text"":""于本回合中，防止接下来将对目标生物或牌手造成的1点伤害。\n抓一张牌。"",""type"":""瞬间"",""flavor"":""生命由许多小部分组成。 对医者而言，这每个小部分都珍贵无比。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147662&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""02ef19b8-fff9-4763-9cc7-cdadfe50797d"",""multiverseId"":147662},""multiverseid"":147662}],""printings"":[""10E"",""STH"",""TPR""],""originalText"":""Prevent the next 1 damage that would be dealt to target creature or player this turn.\nDraw a card."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""6d268c95-c176-5766-9a46-c14f739aba1c""},{""name"":""Beacon of Immortality"",""manaCost"":""{5}{W}"",""cmc"":6.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Double target player's life total. Shuffle Beacon of Immortality into its owner's library."",""flavor"":""The cave floods with light. A thousand rays shine forth and meld into one."",""artist"":""Rob Alexander"",""number"":""10"",""layout"":""normal"",""multiverseid"":""130553"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130553&type=card"",""rulings"":[{""date"":""2007-02-01"",""text"":""If you double a negative life total, you do the real math. A life total of -10 becomes -20.""},{""date"":""2007-07-15"",""text"":""Beacon of Immortality’s effect counts as life gain (or life loss, if the life total was negative) for effects that trigger on or replace life gain (or life loss).""},{""date"":""2007-07-15"",""text"":""If a Beacon is countered or doesn’t resolve, it’s put into its owner’s graveyard, not shuffled into the library.""}],""foreignNames"":[{""name"":""Leitstern der Unsterblichkeit"",""text"":""Verdopple die Lebenspunkte eines Spielers deiner Wahl. Mische den Leitstern der Unsterblichkeit in die Bibliothek seines Besitzers."",""type"":""Spontanzauber"",""flavor"":""Die Höhle füllt sich mit Licht. Tausend Strahlen leuchten hinein und verschmelzen miteinander."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148431&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""5f2c31a7-26dc-42f0-9d32-2fdd767f0c8c"",""multiverseId"":148431},""multiverseid"":148431},{""name"":""Faro de inmortalidad"",""text"":""Duplica el total de vidas del jugador objetivo. Baraja el Faro de inmortalidad en la biblioteca de su propietario."",""type"":""Instantáneo"",""flavor"":""La cueva está inundada de luz. Miles de rayos brillan de ella y se funden en uno."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150326&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""c82ebf31-b1e2-4411-bab5-9e32ef03a5bb"",""multiverseId"":150326},""multiverseid"":150326},{""name"":""Flambeau de l'Immortalité"",""text"":""Doublez le total de points de vie du joueur ciblé. Mélangez le Flambeau de l'Immortalité dans la bibliothèque de son propriétaire."",""type"":""Éphémère"",""flavor"":""La caverne est baignée de lumière. Mille rayons convergent pour n'en plus former qu'un."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149943&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""c34918a0-baf1-468e-9850-7aaca9a998a0"",""multiverseId"":149943},""multiverseid"":149943},{""name"":""Faro dell'Immortalità"",""text"":""Raddoppia i punti vita di un giocatore bersaglio. Rimescola il Faro dell'Immortalità nel grimorio del suo proprietario."",""type"":""Istantaneo"",""flavor"":""La caverna è inondata di luce. Migliaia di raggi balenano nel buio e si fondono in un unico sprazzo."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148814&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""5cb0428e-77ff-45be-85e8-eb37a54f4314"",""multiverseId"":148814},""multiverseid"":148814},{""name"":""不死の標"",""text"":""プレイヤー１人を対象とし、そのプレイヤーのライフの総量を２倍にする。 不死の標をオーナーのライブラリーに加えて切り直す。"",""type"":""インスタント"",""flavor"":""洞穴は光に満ちている。 幾千もの光が注ぎだし、そして一つにと溶け合うのだ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148048&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""8c323cad-79e7-43df-b24e-434658558f3c"",""multiverseId"":148048},""multiverseid"":148048},{""name"":""Guia da Imortalidade"",""text"":""Duplique o total de pontos de vida do jogador alvo. Embaralhe Guia da Imortalidade no grimório de seu dono."",""type"":""Mágica Instantânea"",""flavor"":""A caverna se inunda de luz. Milhares de raios reluzem e se combinam em um."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149560&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""c9a91a7f-d6ef-48d3-b5bd-8e11a7a45978"",""multiverseId"":149560},""multiverseid"":149560},{""name"":""Маяк Бессмертия"",""text"":""Удвойте количество жизни целевого игрока. Втасуйте Маяк Бессмертия в библиотеку его владельца."",""type"":""Мгновенное заклинание"",""flavor"":""Свет наполняет пещеру. Тысяча лучей, вспыхивая, сливается в один."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149177&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""047af470-c894-4ad7-a7b6-45a9278df8cc"",""multiverseId"":149177},""multiverseid"":149177},{""name"":""永生信标"",""text"":""将目标牌手的总生命加倍。 将永生信标洗入其拥有者的牌库。"",""type"":""瞬间"",""flavor"":""洞窟内光华满溢；万道白芒激射而出，终尔融聚为一。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147665&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""12412a36-ab59-4f86-8f15-4919184e66b6"",""multiverseId"":147665},""multiverseid"":147665}],""printings"":[""10E"",""5DN"",""E02"",""PLST""],""originalText"":""Double target player's life total. Shuffle Beacon of Immortality into its owner's library."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""56f4935b-f6c5-59b9-88bf-9bcce20247ce""},{""name"":""Benalish Knight"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Knight"",""types"":[""Creature""],""subtypes"":[""Human"",""Knight""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flash (You may cast this spell any time you could cast an instant.)\nFirst strike (This creature deals combat damage before creatures without first strike.)"",""flavor"":""\""We called them 'armored lightning.'\""\n—Gerrard of the *Weatherlight*"",""artist"":""Zoltan Boros & Gabor Szikszai"",""number"":""11"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""136279"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=136279&type=card"",""variations"":[""0d2a4e1d-cc12-5675-8044-9a7bc7d60050""],""foreignNames"":[{""name"":""Benalischer Ritter"",""text"":""Aufblitzen (Du kannst diesen Zauberspruch zu jedem Zeitpunkt spielen, zu dem du einen Spontanzauber spielen könntest.)\nErstschlag (Diese Kreatur fügt Kampfschaden vor Kreaturen ohne Erstschlag zu.)"",""type"":""Kreatur — Mensch, Ritter"",""flavor"":""„Wir nannten sie ‚Gepanzerte Blitze'\"" —Gerrard von der Wetterlicht"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148433&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""2b240450-7116-4958-8106-b28c8c49d568"",""multiverseId"":148433},""multiverseid"":148433},{""name"":""Caballero benalita"",""text"":""Destello. (Puedes jugar este hechizo en cualquier momento en que pudieras jugar un instantáneo.)\nDaña primero. (Esta criatura hace daño de combate antes que las criaturas sin la habilidad de dañar primero.)"",""type"":""Criatura — Caballero humano"",""flavor"":""\""Los llamábamos 'relámpagos blindados'.\"" —Gerrard del Vientoligero"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150327&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""d9ba462c-0c20-4a76-9b0c-5e9bed149c3c"",""multiverseId"":150327},""multiverseid"":150327},{""name"":""Chevalier bénalian"",""text"":""Flash (Vous pouvez jouer ce sort à tout moment où vous pourriez jouer un éphémère.)\nInitiative (Cette créature inflige des blessures de combat avant les créatures sans l'initiative.)"",""type"":""Créature : humain et chevalier"",""flavor"":""« Nous les appelions 'les foudres de guerre'. » —Gerrard de l'Aquilon"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149944&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""3c0557ae-d52e-4944-9c8f-2202fc3157e0"",""multiverseId"":149944},""multiverseid"":149944},{""name"":""Cavaliere di Benalia"",""text"":""Lampo (Puoi giocare questa magia in ogni momento in cui potresti giocare un istantaneo.)\nAttacco improvviso (Questa creatura infligge danno da combattimento prima delle creature senza attacco improvviso.)"",""type"":""Creatura — Cavaliere Umano"",""flavor"":""\""Li chiamavamo 'fulmini corazzati'.\"" —Gerrard della Cavalcavento"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148816&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""49e09fe7-bef4-4ff8-bed7-da68231bbdeb"",""multiverseId"":148816},""multiverseid"":148816},{""name"":""ベナリアの騎士"",""text"":""瞬速 （あなたはこの呪文を、あなたがインスタントをプレイできるときならいつでもプレイしてよい。）\n先制攻撃 （このクリーチャーは先制攻撃を持たないクリーチャーよりも先に戦闘ダメージを与える。）"",""type"":""クリーチャー — 人間・騎士"",""flavor"":""「鎧を着た稲妻」って呼ばれてるよ。 ――ウェザーライト号のジェラード"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148050&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""4b2c6cbb-f1cd-4abf-bc2f-94204aec890e"",""multiverseId"":148050},""multiverseid"":148050},{""name"":""Cavaleiro de Benália"",""text"":""Lampejo (Você poderá jogar esta mágica a qualquer momento em que puder jogar uma mágica instantânea.)\nIniciativa (Esta criatura causa dano de combate antes de criaturas sem a habilidade de iniciativa.)"",""type"":""Criatura — Humano Cavaleiro"",""flavor"":""\""Nós os chamávamos de 'relâmpago de armadura'.\"" — Gerrard do Bons Ventos"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149561&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""e35a35c8-53bf-4ccc-a48d-a2c24afdcbfa"",""multiverseId"":149561},""multiverseid"":149561},{""name"":""Беналийский Рыцарь"",""text"":""Миг (Вы можете разыграть это заклинание при любой возможности разыгрывать мгновенные заклинания.)\nПервый удар (Это существо наносит боевые повреждения раньше существ без Первого удара.)"",""type"":""Существо — Человек Рыцарь"",""flavor"":""\""Мы звали их \""молнии в доспехах\""\"". — Джерард с Везерлайта"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149178&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""d183d2e3-418e-43bb-b586-37139e9311d0"",""multiverseId"":149178},""multiverseid"":149178},{""name"":""宾纳里亚骑士"",""text"":""闪现（你可以于你能够使用瞬间的时机下使用此咒语。）\n先攻（此生物会比不具先攻异能的生物提前造成战斗伤害。）"",""type"":""生物～人类／骑士"",""flavor"":""「我们称之为『武装的闪电』。」 ～晴空号的杰拉尔德"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147667&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""d55f1cb3-85d4-4643-908e-a6428cd0df6d"",""multiverseId"":147667},""multiverseid"":147667}],""printings"":[""10E"",""ATH"",""WTH""],""originalText"":""Flash\nFirst strike"",""originalType"":""Creature - Human Knight"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""62365fbb-af07-5c2a-976d-e3092bdfc317""},{""name"":""Benalish Knight"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Knight"",""types"":[""Creature""],""subtypes"":[""Human"",""Knight""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flash (You may cast this spell any time you could cast an instant.)\nFirst strike (This creature deals combat damage before creatures without first strike.)"",""flavor"":""\""We called them 'armored lightning.'\""\n—Gerrard of the *Weatherlight*"",""artist"":""Zoltan Boros & Gabor Szikszai"",""number"":""11★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""62365fbb-af07-5c2a-976d-e3092bdfc317""],""printings"":[""10E"",""ATH"",""WTH""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""0d2a4e1d-cc12-5675-8044-9a7bc7d60050""},{""name"":""Cho-Manno, Revolutionary"",""manaCost"":""{2}{W}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Legendary Creature — Human Rebel"",""supertypes"":[""Legendary""],""types"":[""Creature""],""subtypes"":[""Human"",""Rebel""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Prevent all damage that would be dealt to Cho-Manno, Revolutionary."",""flavor"":""\""Mercadia's masks can no longer hide the truth. Our day has come at last.\"""",""artist"":""Steven Belledin"",""number"":""12"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""130554"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130554&type=card"",""foreignNames"":[{""name"":""Cho-Manno"",""text"":""Verhindere allen Schaden, der Cho-Manno zugefügt würde."",""type"":""Legendäre Kreatur — Mensch, Rebell"",""flavor"":""„Merkadias Masken können die Wahrheit nicht länger verbergen. Unsere Stunde ist endlich gekommen.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148449&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""c8ba336d-ccc2-40a8-b1dd-e22dcc0f30e4"",""multiverseId"":148449},""multiverseid"":148449},{""name"":""Cho-Manno, revolucionario"",""text"":""Prevén todo el daño que fuera a ser hecho a Cho-Manno, revolucionario."",""type"":""Criatura legendaria — Rebelde humano"",""flavor"":""\""Las máscaras de Mercadia no pueden seguir ocultando la verdad. Al fin ha llegado nuestro día.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150328&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""b41a7a20-72f0-4b39-8e06-8a1be410204f"",""multiverseId"":150328},""multiverseid"":150328},{""name"":""Cho-Manno, révolutionnaire"",""text"":""Prévenez toutes les blessures qui devraient être infligées à Cho-Manno, révolutionnaire."",""type"":""Créature légendaire : humain et rebelle"",""flavor"":""« Les masques de Mercadia sont tombés. Notre jour est enfin arrivé. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149945&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""f4560031-e999-4d50-9634-81c4a6051c25"",""multiverseId"":149945},""multiverseid"":149945},{""name"":""Cho-Manno, Rivoluzionario"",""text"":""Previeni tutto il danno che verrebbe inflitto a Cho-Manno, Rivoluzionario."",""type"":""Creatura Leggendaria — Ribelle Umano"",""flavor"":""\""Le maschere di Mercadia non possono più nascondere la verità. Il nostro giorno è infine arrivato.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148832&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""6a077524-0981-45ba-990b-61f3ac760bf4"",""multiverseId"":148832},""multiverseid"":148832},{""name"":""革命家チョー＝マノ"",""text"":""革命家チョー＝マノに与えられるすべてのダメージを軽減し、０にする。"",""type"":""伝説のクリーチャー — 人間・レベル"",""flavor"":""もはやメルカディアの仮面が真実を覆い隠すことはできぬ。 ついに我らの時代がやってきたのだ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148066&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""fa9b5a59-39c6-454e-a649-28d417fd8737"",""multiverseId"":148066},""multiverseid"":148066},{""name"":""Cho-Manno, Revolucionário"",""text"":""Previna todo o dano que seria causado a Cho-Manno, Revolucionário."",""type"":""Criatura Lendária — Humano Rebelde"",""flavor"":""\""As máscaras de Mercádia não podem mais esconder a verdade. Nosso dia finalmente chegou.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149562&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""5fc3615a-30c1-4e46-bf32-a3dc022920d9"",""multiverseId"":149562},""multiverseid"":149562},{""name"":""Чо-Манно, Революционер"",""text"":""Предотвратите все повреждения, которые будут нанесены Чо-Манно, Революционеру."",""type"":""Легендарное Существо — Человек Повстанец"",""flavor"":""\""Маски Меркадии больше не могут скрывать правду. Наконец наш день пришел\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149179&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""08a42111-a3a9-454e-8120-21817ff09f9a"",""multiverseId"":149179},""multiverseid"":149179},{""name"":""革命家柯·曼诺"",""text"":""防止将对革命家柯·曼诺造成的所有伤害。"",""type"":""传奇生物～人类／反抗军"",""flavor"":""「玛凯迪亚之假面将再也掩藏不住事实。 我们的时代终于来临了。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147683&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""cd62f763-62a5-41dc-9089-07ba4b155040"",""multiverseId"":147683},""multiverseid"":147683}],""printings"":[""10E"",""MMQ"",""PS11""],""originalText"":""Prevent all damage that would be dealt to Cho-Manno, Revolutionary."",""originalType"":""Legendary Creature - Human Rebel"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""81daea6a-2735-5a46-a2da-b65a2ad5738f""},{""name"":""Condemn"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Put target attacking creature on the bottom of its owner's library. Its controller gains life equal to its toughness."",""flavor"":""\""No doubt the arbiters would put you away, after all the documents are signed. But I will have justice now!\""\n—Alovnek, Boros guildmage"",""artist"":""Daren Bader"",""number"":""13"",""layout"":""normal"",""multiverseid"":""130528"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130528&type=card"",""rulings"":[{""date"":""2010-08-15"",""text"":""The affected creature's last known existence on the battlefield is checked to determine its toughness.""}],""foreignNames"":[{""name"":""Verdammen"",""text"":""Lege eine angreifende Kreatur deiner Wahl unter die Bibliothek ihres Besitzers. Ihr Beherrscher erhält Lebenspunkte in Höhe ihrer Widerstandskraft dazu."",""type"":""Spontanzauber"",""flavor"":""„Es besteht kein Zweifel, dass die Schiedsmänner dich einsperren würden, nachdem alle Dokumente unterzeichnet sind. Aber mir geht es um Gerechtigkeit, und zwar jetzt\"" —Alovnek, Boros-Gildenmagier"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148460&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""685a22ac-11a9-4345-9996-3ddab9b5c06d"",""multiverseId"":148460},""multiverseid"":148460},{""name"":""Condenar"",""text"":""Pon la criatura atacante objetivo en el fondo de la biblioteca de su propietario. Su controlador gana vida igual a su resistencia."",""type"":""Instantáneo"",""flavor"":""\""Sin duda los árbitros te encerrarían, después de que se firmasen todos los documentos. Pero yo tendré justicia ¡ahora\"" —Alóvnek, mago del Gremio Boros"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150329&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""88390067-fba0-43c7-b109-ea236273db6f"",""multiverseId"":150329},""multiverseid"":150329},{""name"":""Condamnation"",""text"":""Mettez la créature attaquante ciblée au-dessous de la bibliothèque de son propriétaire. Son contrôleur gagne un nombre de points de vie égal à son endurance."",""type"":""Éphémère"",""flavor"":""« Les Arbitres vous mettront sûrement en prison une fois que tous les documents seront signés. Mais je veux que justice soit faite sur le champ. » —Alovnek, ghildmage de Boros"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149946&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""5b0251e6-97c4-4a67-abb3-dd8d1ca320e8"",""multiverseId"":149946},""multiverseid"":149946},{""name"":""Condannare"",""text"":""Metti una creatura attaccante bersaglio in fondo al grimorio del suo proprietario. Il suo controllore guadagna un ammontare di punti vita pari alla sua costituzione."",""type"":""Istantaneo"",""flavor"":""\""Non ho dubbi che i giudici ti rinchiuderebbero, una volta firmati tutti i documenti. Ma io otterrò giustizia adesso\"" —Alovnek, mago della gilda Boros"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148843&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""7f765c02-de10-41cd-a2a5-f64f03606534"",""multiverseId"":148843},""multiverseid"":148843},{""name"":""糾弾"",""text"":""攻撃しているクリーチャー１体を対象とし、それをオーナーのライブラリーの一番下に置く。 それのコントローラーは、それのタフネスに等しい点数のライフを得る。"",""type"":""インスタント"",""flavor"":""判事どもなら、全部の書類に署名して、それから判決といくんだろう。 だが、俺が裁きを下すのは今だ！ ――ボロスのギルド魔道士、アラヴネク"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148077&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""0d4473bb-4c6f-418f-b2a7-438b4fe23a98"",""multiverseId"":148077},""multiverseid"":148077},{""name"":""Condenar"",""text"":""Coloque a criatura alvo atacante no fundo do grimório de seu dono. Seu controlador ganha uma quantidade de pontos de vida igual à resistência dela."",""type"":""Mágica Instantânea"",""flavor"":""\""Sem dúvida os árbitros darão um fim a você, depois que todos os documentos forem assinados. Mas eu terei justiça agora\"" — Alovnek, mago da guilda Boros"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149563&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""af82f2ad-c859-45f8-aa53-b6d46b70ca83"",""multiverseId"":149563},""multiverseid"":149563},{""name"":""Осуждение"",""text"":""Положите целевое атакующее существо в низ библиотеки его владельца. Контролирующий его игрок получает количество жизни, равное его выносливости."",""type"":""Мгновенное заклинание"",""flavor"":""\""Разумеется, верховные судьи засадили бы тебя после подписания всех документов. Но я добьюсь правосудия сейчас\"" — Аловнек, маг гильдии Боросов"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149180&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""2e7574ee-648d-4c2b-b45e-ad7d5fe30862"",""multiverseId"":149180},""multiverseid"":149180},{""name"":""判罪"",""text"":""将目标进行攻击的生物置于其拥有者的牌库底。 其操控者获得等同于其防御力的生命。"",""type"":""瞬间"",""flavor"":""「既然需签署的文件一无遗漏，仲裁者当然会放过你。 但我现在就要主持正义！」 ～波洛斯公会法师阿劳涅"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147694&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""831aa434-8e0c-4e58-b9d3-469c2451e1ab"",""multiverseId"":147694},""multiverseid"":147694}],""printings"":[""10E"",""C14"",""C17"",""CMR"",""DDL"",""DIS"",""M11"",""P07"",""PRM"",""PS11"",""RVR"",""SCD"",""TD0"",""TD2"",""ZNC""],""originalText"":""Put target attacking creature on the bottom of its owner's library. Its controller gains life equal to its toughness."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""7fef665c-36a1-5f7a-9299-cf8938708710""},{""name"":""Demystify"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Destroy target enchantment."",""flavor"":""\""Illusion is a crutch for those with no grounding in reality.\""\n—Cho-Manno"",""artist"":""Christopher Rush"",""number"":""14"",""layout"":""normal"",""multiverseid"":""129524"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129524&type=card"",""foreignNames"":[{""name"":""Entmystifizieren"",""text"":""Zerstöre eine Verzauberung deiner Wahl."",""type"":""Spontanzauber"",""flavor"":""„Illusion ist ein Krückstock für all jene ohne Wurzeln in der Realität.\"" —Cho-Manno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148476&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""e5088ee4-b1f1-462b-ad1f-169235f9c763"",""multiverseId"":148476},""multiverseid"":148476},{""name"":""Desmitificar"",""text"":""Destruye el encantamiento objetivo."",""type"":""Instantáneo"",""flavor"":""\""La ilusión es una muleta para aquellos sin asidero en la realidad.\"" —Cho-Manno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150330&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""f95c5fd1-60bf-4ac1-96a9-20b4099fde78"",""multiverseId"":150330},""multiverseid"":150330},{""name"":""Démystification"",""text"":""Détruisez l'enchantement ciblé."",""type"":""Éphémère"",""flavor"":""« L'illusion est un appui pour quiconque n'a pas pied dans la réalité. » —Cho-Manno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149947&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""3c4a53c9-0c27-4065-86fc-8581d3243ed6"",""multiverseId"":149947},""multiverseid"":149947},{""name"":""Demistificare"",""text"":""Distruggi un incantesimo bersaglio."",""type"":""Istantaneo"",""flavor"":""\""L'illusione è un sostegno per quanti non hanno appoggio nella realtà.\"" —Cho-Manno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148859&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""bca022e9-4f2f-4dab-a8e7-65eb170ba667"",""multiverseId"":148859},""multiverseid"":148859},{""name"":""啓蒙"",""text"":""エンチャント１つを対象とし、それを破壊する。"",""type"":""インスタント"",""flavor"":""幻影とは、現実に根ざしていないものの支えに過ぎない。 ――チョー＝マノ"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148093&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""4384341d-802f-4e2e-9344-a3992d7d2255"",""multiverseId"":148093},""multiverseid"":148093},{""name"":""Desmistificar"",""text"":""Destrua o encantamento alvo."",""type"":""Mágica Instantânea"",""flavor"":""\""A ilusão é o arrimo dos que desconhecem a realidade.\"" — Cho-Manno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149564&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""495a6214-4766-4fca-9536-17e7b33431ae"",""multiverseId"":149564},""multiverseid"":149564},{""name"":""Прояснение"",""text"":""Уничтожьте целевые чары."",""type"":""Мгновенное заклинание"",""flavor"":""\""Иллюзия это опора для тех, кто не базируется на реальности\"". — Чо-Манно"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149181&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""a5b05f19-963d-4aaa-bd10-a647479c4107"",""multiverseId"":149181},""multiverseid"":149181},{""name"":""揭秘"",""text"":""消灭目标结界。"",""type"":""瞬间"",""flavor"":""「在现实无法立足者，才会寻求幻影支撑。」 ～柯·曼诺"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147710&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""a928f102-dd17-4144-a7ee-db7734c1ede5"",""multiverseId"":147710},""multiverseid"":147710}],""printings"":[""10E"",""8ED"",""9ED"",""M12"",""ONS"",""ROE"",""XLN""],""originalText"":""Destroy target enchantment."",""originalType"":""Instant"",""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""2f9c211e-1869-5b3f-94ea-f73b7910a5af""},{""name"":""Field Marshal"",""manaCost"":""{1}{W}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Soldier"",""types"":[""Creature""],""subtypes"":[""Human"",""Soldier""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Other Soldier creatures get +1/+1 and have first strike. (They deal combat damage before creatures without first strike.)"",""flavor"":""He is the only one who sees the patterns in the overlapping maps and conflicting reports."",""artist"":""Stephen Tappin"",""number"":""15"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""135258"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=135258&type=card"",""variations"":[""6d6a31d5-a80b-5f31-9b46-2c2083a95581""],""foreignNames"":[{""name"":""Feldmarschall"",""text"":""Andere Soldatenkreaturen erhalten +1/+1 und Erstschlag. (Sie fügen Kampfschaden vor Kreaturen ohne Erstschlag zu.)"",""type"":""Kreatur — Mensch, Soldat"",""flavor"":""Er ist der einzige, der die Muster in den sich überschneidenden Karten und widersprechenden Berichten erkennt."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148502&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""b8046035-7670-4983-84be-1f46696d5d79"",""multiverseId"":148502},""multiverseid"":148502},{""name"":""Mariscal de campo"",""text"":""Las otras criaturas Soldado obtienen +1/+1 y tienen la habilidad de dañar primero. (Hacen daño de combate antes que las criaturas sin la habilidad de dañar primero.)"",""type"":""Criatura — Soldado humano"",""flavor"":""Él es el único que ve los patrones en los distintos mapas y en los informes contradictorios."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150331&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""e3781d52-2465-4222-88e7-3d6323e10713"",""multiverseId"":150331},""multiverseid"":150331},{""name"":""Général de campagne"",""text"":""Les autres créatures Soldat gagnent +1/+1 et ont l'initiative. (Elles infligent des blessures de combat avant les créatures sans l'initiative.)"",""type"":""Créature : humain et soldat"",""flavor"":""Il est le seul à comprendre le chaos des cartes et des rapports conflictuels."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149948&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""dcd8b40f-2127-4fb1-a1b6-023e1a267df3"",""multiverseId"":149948},""multiverseid"":149948},{""name"":""Maresciallo di Campo"",""text"":""Le altre creature Soldato prendono +1/+1 e hanno attacco improvviso. (Infliggono danno da combattimento prima delle creature senza attacco improvviso.)"",""type"":""Creatura — Soldato Umano"",""flavor"":""E' l'unico che riesce a vedere gli schemi nella sovrapposizione delle mappe e nei rapporti contrastanti."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148885&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""688ae87a-9700-487b-ae5e-a81243fbd199"",""multiverseId"":148885},""multiverseid"":148885},{""name"":""陸軍元帥"",""text"":""他の兵士クリーチャーは＋１/＋１の修整を受けるとともに先制攻撃を持つ。 （それらは先制攻撃を持たないクリーチャーよりも先に戦闘ダメージを与える。）"",""type"":""クリーチャー — 人間・兵士"",""flavor"":""折り重なった地図と矛盾する報告の中に傾向を見つけられるのは彼だけだ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148119&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""e6625a0d-df99-4224-ad71-1a9282894c30"",""multiverseId"":148119},""multiverseid"":148119},{""name"":""Marechal-de-Campo"",""text"":""As outras criaturas do tipo Soldado recebem +1/+1 e têm iniciativa. (Elas causam dano de combate antes de criaturas sem a habilidade de iniciativa.)"",""type"":""Criatura — Humano Soldado"",""flavor"":""Ele é o único que consegue enxergar alguma coisa nos mapas sobrepostos e relatórios conflituosos."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149565&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""b3ad064c-2a21-419e-84f6-f1b4c0026e2f"",""multiverseId"":149565},""multiverseid"":149565},{""name"":""Фельдмаршал"",""text"":""Остальные существа Солдаты получают +1/+1 и имеют Первый удар. (Они наносят боевые повреждения раньше существ без Первого удара.)"",""type"":""Существо — Человек Солдат"",""flavor"":""Он один видит закономерности в перекроенных картах и противоречивых донесениях."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149182&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""cb9e1a2e-69c6-4ed4-b66f-0e5a1dd1d89c"",""multiverseId"":149182},""multiverseid"":149182},{""name"":""元帅"",""text"":""其它的士兵生物得+1/+1并具有先攻异能。 （它们会比不具先攻异能的生物提前造成战斗伤害。）"",""type"":""生物～人类／士兵"",""flavor"":""在堆积如山的地图与互相矛盾的回报中，只有他才看得出埋藏其间的动向。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147736&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""04298afb-01e3-4f55-8278-f4efccc56590"",""multiverseId"":147736},""multiverseid"":147736}],""printings"":[""10E"",""CSP"",""SLD""],""originalText"":""Other Soldier creatures get +1/+1 and have first strike."",""originalType"":""Creature - Human Soldier"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""668c64a2-cecb-5f48-af16-d7a64814d3e7""},{""name"":""Field Marshal"",""manaCost"":""{1}{W}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Soldier"",""types"":[""Creature""],""subtypes"":[""Human"",""Soldier""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Other Soldier creatures get +1/+1 and have first strike. (They deal combat damage before creatures without first strike.)"",""flavor"":""He is the only one who sees the patterns in the overlapping maps and conflicting reports."",""artist"":""Stephen Tappin"",""number"":""15★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""668c64a2-cecb-5f48-af16-d7a64814d3e7""],""printings"":[""10E"",""CSP"",""SLD""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""6d6a31d5-a80b-5f31-9b46-2c2083a95581""},{""name"":""Ghost Warden"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Spirit"",""types"":[""Creature""],""subtypes"":[""Spirit""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{T}: Target creature gets +1/+1 until end of turn."",""flavor"":""\""I thought of fate as an iron lattice, intricate but rigidly unchangeable. That was until some force bent fate's bars to spare my life.\""\n—Ilromov, traveling storyteller"",""artist"":""Ittoku"",""number"":""16"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""132105"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132105&type=card"",""foreignNames"":[{""name"":""Geisterhafter Wächter"",""text"":""{T}: Eine Kreatur deiner Wahl erhält +1/+1 bis zum Ende des Zuges."",""type"":""Kreatur — Geist"",""flavor"":""„Lange habe ich geglaubt, dass das Schicksal wie ein eisernes Gitter ist: verschlungen und eigentlich unveränderbar. Das war, bevor eine unbekannte Kraft die Stangen des Schicksals verbogen hat, um mein Leben zu retten.\"" —Ilromov, herumwandernder Geschichtenerzähler"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148520&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""8fcb1952-0afe-4a0c-a664-8d96e9bb9601"",""multiverseId"":148520},""multiverseid"":148520},{""name"":""Protectora fantasma"",""text"":""{T}: La criatura objetivo obtiene +1/+1 hasta el final del turno."",""type"":""Criatura — Espíritu"",""flavor"":""\""Pensaba que el destino era como una red de acero, intrincada pero rígidamente inmutable. Eso fue hasta que una fuerza dobló los barrotes del destino para salvar mi vida.\"" —Ílromov, narrador viajero"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150332&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""fbfd034e-be9b-4285-acce-b463ae6128cf"",""multiverseId"":150332},""multiverseid"":150332},{""name"":""Garde fantôme"",""text"":""{T} : La créature ciblée gagne +1/+1 jusqu'à la fin du tour."",""type"":""Créature : esprit"",""flavor"":""« J'avais toujours visualisé le destin comme une grille de fer, ornée et rigide. Mais c'était avant qu'une force ne torde les barreaux du destin pour épargner ma vie. » —Ilromov, conteur itinérant"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149949&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""a38a7c37-9380-4ff9-8c03-134fa692d4e1"",""multiverseId"":149949},""multiverseid"":149949},{""name"":""Guardiana Fantasma"",""text"":""{T}: Una creatura bersaglio prende +1/+1 fino alla fine del turno."",""type"":""Creatura — Spirito"",""flavor"":""\""Pensavo al fato come a un reticolo di ferro, intricato ma rigido e immutabile. Questo fino a quando una qualche forza ha piegato le sbarre del fato per risparmiarmi la vita.\"" —Ilromov, cantastorie girovago"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148903&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""91d0655b-26ac-4fb1-8dc7-f9c1befcfb21"",""multiverseId"":148903},""multiverseid"":148903},{""name"":""幽霊の管理人"",""text"":""{T}：クリーチャー１体を対象とする。それはターン終了時まで＋１/＋１の修整を受ける。"",""type"":""クリーチャー — スピリット"",""flavor"":""運命とは鉄格子のように、絡み合いながらも頑丈で変わらないものだと思っていました。 しかしそれは、何かの力が運命の格子をねじ曲げて私の命を救ってくれるまでのことでした。 ――旅の語り部、イルロモフ"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148137&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""da805147-1025-4503-afab-d046115facb2"",""multiverseId"":148137},""multiverseid"":148137},{""name"":""Carcereiro Fantasma"",""text"":""{T}: A criatura alvo recebe +1/+1 até o final do turno."",""type"":""Criatura — Espírito"",""flavor"":""\""Sempre pensei no destino como uma treliça de aço: intricada mas rigorosamente imutável. Até que uma força entortou as barras do destino para salvar a minha vida.\"" — Ilromov, contador de histórias viajante"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149566&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""0554a925-0a85-4cb7-a71c-a13872f24e24"",""multiverseId"":149566},""multiverseid"":149566},{""name"":""Призрачный Хранитель"",""text"":""{T}: Целевое существо получает +1/+1 до конца хода."",""type"":""Существо — Дух"",""flavor"":""\""Я всегда думал, что судьба похожа на кованую железную решетку. Ее узор прихотлив, но изменить его нельзя. Так было до тех пор, пока некая сила не изогнула прутья судьбы и не спасла мне жизнь\"". — Илромов, бродячий рассказчик"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149183&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""95565023-bd30-45cb-9e8e-e675dd0e5a24"",""multiverseId"":149183},""multiverseid"":149183},{""name"":""护持鬼影"",""text"":""{T}：目标生物得+1/+1直到回合结束。"",""type"":""生物～精怪"",""flavor"":""「我总以为命运就像铁栅栏，错综复杂而又无从改变。 自从某种力量弯曲命运的铁杆来救我一命之后，我再也不作如是想。」 ～旅行说书人伊洛莫"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147754&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""8a3c4e7f-6d1b-4d1d-90ce-c52824a3842f"",""multiverseId"":147754},""multiverseid"":147754}],""printings"":[""10E"",""GPT""],""originalText"":""{T}: Target creature gets +1/+1 until end of turn."",""originalType"":""Creature - Spirit"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""d68306e2-9877-5987-84b3-12b8234c8eec""},{""name"":""Glorious Anthem"",""manaCost"":""{1}{W}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment"",""types"":[""Enchantment""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Creatures you control get +1/+1."",""flavor"":""Once heard, the battle song of an angel becomes part of the listener forever."",""artist"":""Kev Walker"",""number"":""17"",""layout"":""normal"",""multiverseid"":""129572"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129572&type=card"",""foreignNames"":[{""name"":""Glorreiche Hymne"",""text"":""Kreaturen, die du kontrollierst, erhalten +1/+1."",""type"":""Verzauberung"",""flavor"":""Wer auch nur einmal die Kampfeslieder der Engel gehört hat, wird sie sein Leben lang nicht mehr vergessen."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148523&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""41aaefb2-1196-4c0f-8400-4a94ae54078e"",""multiverseId"":148523},""multiverseid"":148523},{""name"":""Himno glorioso"",""text"":""Las criaturas que controlas obtienen +1/+1."",""type"":""Encantamiento"",""flavor"":""Una vez escuchada, la canción de batalla de un ángel es parte del oyente para siempre."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150333&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""85456f3d-809e-4173-bd1a-3861814cf5ce"",""multiverseId"":150333},""multiverseid"":150333},{""name"":""Antienne glorieuse"",""text"":""Les créatures que vous contrôlez gagnent +1/+1."",""type"":""Enchantement"",""flavor"":""Quiconque écoute le chant de guerre d'un ange en est imprégné pour toujours."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149950&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""fa15d3f0-0852-4b1e-9988-35856547e474"",""multiverseId"":149950},""multiverseid"":149950},{""name"":""Inno Glorioso"",""text"":""Le creature che controlli prendono +1/+1."",""type"":""Incantesimo"",""flavor"":""Una volta udito, il canto di battaglia di un angelo diventa parte di chi lo ascolta per sempre."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148906&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""9e2f8c93-6d11-4386-9012-5f14e6361bf6"",""multiverseId"":148906},""multiverseid"":148906},{""name"":""栄光の頌歌"",""text"":""あなたがコントロールするクリーチャーは＋１/＋１の修整を受ける。"",""type"":""エンチャント"",""flavor"":""一度耳にすれば、天使の戦いの歌は永遠に聞き手の一部となる。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148140&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""baf45fdd-7ae8-42e9-8e39-f7aeb4cdf86a"",""multiverseId"":148140},""multiverseid"":148140},{""name"":""Antífona Gloriosa"",""text"":""As criaturas que você controla recebem +1/+1."",""type"":""Encantamento"",""flavor"":""Uma vez ouvido, o canto de batalha de um anjo torna-se parte do ouvinte para sempre."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149567&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""64938ee8-4778-432c-ab01-18da0efa3873"",""multiverseId"":149567},""multiverseid"":149567},{""name"":""Славный Гимн"",""text"":""Существа под вашим контролем получают +1/+1."",""type"":""Чары"",""flavor"":""Раз услышав боевую песнь ангела, ты будешь петь в душе ее всегда."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149184&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""5a56ebd3-f00b-467d-a597-074c0eac7ac8"",""multiverseId"":149184},""multiverseid"":149184},{""name"":""辉煌的赞美诗"",""text"":""由你操控的生物得+1/+1。"",""type"":""结界"",""flavor"":""只要听过一次，天使战歌将永远成为聆听者的一部份。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147757&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""1ddc32a2-8a2e-4176-a479-e51262d7ae68"",""multiverseId"":147757},""multiverseid"":147757}],""printings"":[""10E"",""7ED"",""8ED"",""9ED"",""M21"",""PJAS"",""PJJT"",""PJSE"",""PM21"",""PRM"",""PS11"",""PSUS"",""USG""],""originalText"":""Creatures you control get +1/+1."",""originalType"":""Enchantment"",""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""546eac7c-1424-597d-ac13-bf8558e88fe3""},{""name"":""Hail of Arrows"",""manaCost"":""{X}{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Hail of Arrows deals X damage divided as you choose among any number of target attacking creatures."",""flavor"":""\""Do not let a single shaft loose until my word. And when I give that word, do not leave a single shaft in Eiganjo.\""\n—General Takeno"",""artist"":""Anthony S. Waters"",""number"":""18"",""layout"":""normal"",""multiverseid"":""132107"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132107&type=card"",""rulings"":[{""date"":""2005-06-01"",""text"":""You choose how the damage will be divided among the target creatures at the time you cast Hail of Arrows. Each target must be dealt at least 1 damage. If any of those creatures becomes an illegal target before Hail of Arrows resolves, the division of damage among the remaining creatures doesn’t change.""}],""foreignNames"":[{""name"":""Pfeilhagel"",""text"":""Pfeilhagel fügt X Schadenspunkte zu, deren Verteilung auf eine beliebige Anzahl an angreifenden Kreaturen deiner Wahl du bestimmst."",""type"":""Spontanzauber"",""flavor"":""„Kein einziger Pfeil verlässt Eiganjo, bevor ich nicht das Kommando gebe. Und wenn ich das Kommando gebe, bleibt kein einziger Pfeil in Eiganjo.\"" —General Takeno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148534&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""544cf2fb-5c24-47d3-9075-d22ab785abc6"",""multiverseId"":148534},""multiverseid"":148534},{""name"":""Lluvia de flechas"",""text"":""La Lluvia de flechas hace X puntos de daño divididos como elijas entre cualquier número de criaturas atacantes objetivo."",""type"":""Instantáneo"",""flavor"":""\""No se dispare ni una sola flecha antes de que lo ordene. Y cuando dé la orden, que no quede ni una sola flecha en Eiganjo.\"" —General Takeno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150334&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""369600ea-9d9a-4080-a14a-20c868afc74f"",""multiverseId"":150334},""multiverseid"":150334},{""name"":""Pluie de flèches"",""text"":""La Pluie de flèches inflige X blessures réparties comme vous le désirez entre n'importe quel nombre de créatures attaquantes ciblées."",""type"":""Éphémère"",""flavor"":""« Qu'aucune flèche ne soit décochée avant mon ordre. Et quand je le donnerai, je ne veux plus voir la moindre flèche dans Eiganjo. » —Général Takeno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149951&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""abd1c13b-f0f0-4ea1-bc8b-71b72e41cff2"",""multiverseId"":149951},""multiverseid"":149951},{""name"":""Salva di Frecce"",""text"":""La Salva di Frecce infligge X danni suddivisi a tua scelta tra un qualsiasi numero di creature attaccanti bersaglio."",""type"":""Istantaneo"",""flavor"":""\""Non scoccate neppure una freccia fino al mio ordine. E quando darò l'ordine, non voglio che resti neppure una freccia in tutta Eiganjo.\"" —Generale Takeno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148917&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""07bf18d1-432e-47c7-8627-4667cac54697"",""multiverseId"":148917},""multiverseid"":148917},{""name"":""矢ぶすま"",""text"":""望む数の攻撃しているクリーチャーを対象とする。矢ぶすまはそれらに、Ｘ点のダメージを好きなように割り振って与える。"",""type"":""インスタント"",""flavor"":""我が指示あるまで、矢一本も放ってはならぬ。 我が指示あらば、永岩城に矢一本も残してはならぬ。 ――武野御大将"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148151&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""6cd693d8-f04d-493b-85e4-6441aaed6785"",""multiverseId"":148151},""multiverseid"":148151},{""name"":""Saraivada de Flechas"",""text"":""Saraivada de Flechas causa X pontos de dano divididos como você escolher entre qualquer número de criaturas atacantes alvo."",""type"":""Mágica Instantânea"",""flavor"":""\""Não disparem nenhuma flecha até que eu diga. E quando eu disser, não deixem nenhuma flecha em Eiganjo.\"" — General Takeno"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149568&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""21fe28b5-66a2-4179-b239-f8ea898c99e9"",""multiverseId"":149568},""multiverseid"":149568},{""name"":""Град Стрел"",""text"":""Град Стрел наносит X повреждений, распределенных по вашему выбору между любым количеством целевых атакующих существ."",""type"":""Мгновенное заклинание"",""flavor"":""\""И чтобы ни одна стрела не взмыла в небо до моей команды. Но когда я отдам эту команду, ни одна стрела не должна оставаться в Эйгандзё\"". — Генерал Такено"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149185&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""0eb41caf-a1ec-4f8c-aa4c-0db0e2b74e14"",""multiverseId"":149185},""multiverseid"":149185},{""name"":""箭如雨下"",""text"":""箭如雨下造成X点伤害，你可任意分配于任何数量之目标进行攻击的生物上。"",""type"":""瞬间"",""flavor"":""「在我下令前，一支箭都不准射。 在我下令后，永岩城一支箭都不准留。」 ～武野将军"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147768&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""29f5bf61-6782-4fa4-b40d-0fbe401000cc"",""multiverseId"":147768},""multiverseid"":147768}],""printings"":[""10E"",""CN2"",""SOK""],""originalText"":""Hail of Arrows deals X damage divided as you choose among any number of target attacking creatures."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""c19bcfc2-fc10-5040-8239-1c193098df47""},{""name"":""Heart of Light"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature (Target a creature as you cast this. This card enters the battlefield attached to that creature.)\nPrevent all damage that would be dealt to and dealt by enchanted creature."",""artist"":""Luca Zontini"",""number"":""19"",""layout"":""normal"",""multiverseid"":""132090"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132090&type=card"",""variations"":[""08548840-8e62-5fed-b432-f2a5a0cb5f4e""],""foreignNames"":[{""name"":""Herz aus Licht"",""text"":""Kreaturenverzauberung (Bestimme eine Kreatur als Ziel, sowie du diese Karte spielst. Diese Karte kommt an die Kreatur angelegt ins Spiel.)\nVerhindere allen Schaden, der der verzauberten Kreatur zugefügt und von ihr zugefügt würde."",""type"":""Verzauberung — Aura"",""flavor"":""Für jene, die nach Erleuchtung streben, ist Gewalt eine unnötige Ablenkung."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148537&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""32edd6bf-df93-4b06-8cf9-2deb43785825"",""multiverseId"":148537},""multiverseid"":148537},{""name"":""Corazón de luz"",""text"":""Encantar criatura. (Haz objetivo a una criatura al jugarlo. Esta carta entra en juego anexada a esa criatura.)\nPrevén todo el daño que se le fuera a hacer y que fuera a hacer la criatura encantada."",""type"":""Encantamiento — Aura"",""flavor"":""Para aquellos que alcanzan la iluminación, la violencia es una distracción innecesaria."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150335&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""5e6f8d0a-90e1-4c32-aae1-2f5de950f82b"",""multiverseId"":150335},""multiverseid"":150335},{""name"":""Cœur de lumière"",""text"":""Enchanter : créature (Ciblez une créature au moment où vous jouez cette carte. Cette carte arrive en jeu attachée à cette créature.)\nPrévenez toutes les blessures qui devraient être infligées à et par la créature enchantée."",""type"":""Enchantement : aura"",""flavor"":""Pour ceux qui atteignent l'illumination, la violence est une distraction inutile."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149952&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""8aec5f56-713e-4d83-ad86-33c12d75ddb5"",""multiverseId"":149952},""multiverseid"":149952},{""name"":""Cuore di Luce"",""text"":""Incanta creatura (Bersaglia una creatura mentre giochi questa carta. Questa carta entra in gioco assegnata a quella creatura.)\nPrevieni tutto il danno che verrebbe inflitto alla e dalla creatura incantata."",""type"":""Incantesimo — Aura"",""flavor"":""Per coloro che raggiungono l'illuminazione, la violenza è una distrazione inutile."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148920&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""a879e411-a9cd-47ca-a34b-2076dae5537b"",""multiverseId"":148920},""multiverseid"":148920},{""name"":""光の心"",""text"":""エンチャント（クリーチャー） （これをプレイする際に、クリーチャー１体を対象とする。 このカードはそのクリーチャーにつけられている状態で場に出る。）\nエンチャントされているクリーチャーが与えるダメージと与えられるダメージをすべて軽減し、０にする。"",""type"":""エンチャント — オーラ"",""flavor"":""悟りに達した者に、暴力など無用の妨げ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148154&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""51374c7d-2821-4427-8967-e8e382967966"",""multiverseId"":148154},""multiverseid"":148154},{""name"":""Coração de Luz"",""text"":""Encantar criatura (Ao jogar este card, escolha uma criatura alvo. Este card entra em jogo anexado àquela criatura.)\nPrevina todo o dano que seria causado a e causado pela criatura encantada."",""type"":""Encantamento — Aura"",""flavor"":""Para os que atingem a iluminação, a violência é uma distração desnecessária."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149569&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""a8b23e5a-49a4-4a79-a19e-eb9faf0276d9"",""multiverseId"":149569},""multiverseid"":149569},{""name"":""Сердце Света"",""text"":""Зачаровать существо (При разыгрывании этой карты выберите целью существо. Эта карта входит в игру прикрепленной к тому существу.)\nПредотвратите все повреждения, которые будут нанесены зачарованному существу и зачарованным существом."",""type"":""Чары — Аура"",""flavor"":""Для просвещенных насилие это ненужное отвлечение."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149186&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""421431ad-f349-47f0-a281-58d20ad73b11"",""multiverseId"":149186},""multiverseid"":149186},{""name"":""净光之心"",""text"":""生物结界（于使用时指定一个生物为目标。 此牌进场时结附在该生物上。）\n防止受此结界的生物将受到或造成的所有伤害。"",""type"":""结界～灵气"",""flavor"":""对已受教化的人来说，暴力只能分散点不必要的注意力。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147771&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""f24c13b3-cb08-46f4-b51a-c8bca6c9cd29"",""multiverseId"":147771},""multiverseid"":147771}],""printings"":[""10E"",""BOK""],""originalText"":""Enchant creature\nPrevent all damage that would be dealt to and dealt by enchanted creature."",""originalType"":""Enchantment - Aura"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""36fcaa1e-64e3-56e3-950c-907db167e41f""},{""name"":""Heart of Light"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature (Target a creature as you cast this. This card enters the battlefield attached to that creature.)\nPrevent all damage that would be dealt to and dealt by enchanted creature."",""flavor"":""For those who reach enlightenment, violence is an unnecessary distraction."",""artist"":""Luca Zontini"",""number"":""19★"",""layout"":""normal"",""variations"":[""36fcaa1e-64e3-56e3-950c-907db167e41f""],""printings"":[""10E"",""BOK""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""08548840-8e62-5fed-b432-f2a5a0cb5f4e""},{""name"":""High Ground"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment"",""types"":[""Enchantment""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Each creature you control can block an additional creature each combat."",""flavor"":""In war, as in society, position is everything."",""artist"":""rk post"",""number"":""20"",""layout"":""normal"",""multiverseid"":""132145"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132145&type=card"",""rulings"":[{""date"":""2007-07-15"",""text"":""High Ground allows you to make some complicated blocks. For example, if you’re being attacked by three creatures (call them A, B, and C) and you control three creatures (X, Y, and Z), you can have X block A and B, Y block B and C, and Z block just C, among many other possible options. The defending player chooses how each blocking creature’s combat damage will be divided among the creatures it’s blocking.""},{""date"":""2007-07-15"",""text"":""High Ground’s effect is cumulative. If you have a creature that can already block an additional creature, now it can block three creatures.""}],""foreignNames"":[{""name"":""Erhöhte Stellung"",""text"":""Jede Kreatur, die du kontrollierst, kann eine zusätzliche Kreatur blocken."",""type"":""Verzauberung"",""flavor"":""Im Krieg — wie im Leben — ist die Stellung entscheidend."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148539&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""31b31863-dc7c-4854-875b-db764e00be51"",""multiverseId"":148539},""multiverseid"":148539},{""name"":""Terreno elevado"",""text"":""Cada criatura que controlas puede bloquear una criatura adicional."",""type"":""Encantamiento"",""flavor"":""En la guerra, al igual que en la sociedad, la posición lo es todo."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150336&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""25a57350-a93b-4a0d-80ac-7d4aeb5f3301"",""multiverseId"":150336},""multiverseid"":150336},{""name"":""Terrain élevé"",""text"":""Chaque créature que vous contrôlez peut bloquer une créature supplémentaire."",""type"":""Enchantement"",""flavor"":""En guerre, comme en société, tout est une question de position."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149953&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""b1966c1f-d423-4e80-a990-90543f910779"",""multiverseId"":149953},""multiverseid"":149953},{""name"":""Terreno Favorevole"",""text"":""Ogni creatura che controlli può bloccare una creatura addizionale."",""type"":""Incantesimo"",""flavor"":""In guerra, come in società, la posizione è fondamentale."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148922&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""e9cb07d9-1811-4dd3-a5d5-e70a214c3ef8"",""multiverseId"":148922},""multiverseid"":148922},{""name"":""高所"",""text"":""あなたがコントロールする各クリーチャーは、本来に加えて追加で１体のクリーチャーをブロックできる。"",""type"":""エンチャント"",""flavor"":""戦場では、一般社会と同じように、立っている場所こそが物を言うのだ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148156&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""b6a93cea-e9e0-46ab-83a1-2813af457ade"",""multiverseId"":148156},""multiverseid"":148156},{""name"":""Terreno Elevado"",""text"":""Cada criatura que você controla pode bloquear uma criatura adicional."",""type"":""Encantamento"",""flavor"":""Na guerra, assim como na sociedade, a posição é fundamental."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149570&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""7fa00f71-d38b-46e1-ace6-2830d3ff230e"",""multiverseId"":149570},""multiverseid"":149570},{""name"":""Занятая Высота"",""text"":""Каждое существо под вашим контролем может блокировать дополнительное существо."",""type"":""Чары"",""flavor"":""На войне, как и в обществе, положение это все."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149187&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""6b37d5d7-43bf-4902-92a0-5cb121e59848"",""multiverseId"":149187},""multiverseid"":149187},{""name"":""制高点"",""text"":""由你操控的每个生物可以额外多阻挡一个生物。"",""type"":""结界"",""flavor"":""在战场上，就如同在社会中，位置高低是重要的关键。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147773&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""9cb91a6d-03f8-4bd1-824d-bc350fac9055"",""multiverseId"":147773},""multiverseid"":147773}],""printings"":[""10E"",""EXO"",""PLST""],""originalText"":""Each creature you control can block an additional creature."",""originalType"":""Enchantment"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""60f49caf-3583-5f85-b4b3-08dca73a8628""},{""name"":""Holy Day"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Prevent all combat damage that would be dealt this turn."",""flavor"":""\""Today there is feasting and peace across our land, but the war has not ended. Tuck away your bloodlust. You'll need it tomorrow.\""\n—Karrim, Samite healer"",""artist"":""Volkan Baǵa"",""number"":""21"",""layout"":""normal"",""multiverseid"":""129593"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129593&type=card"",""foreignNames"":[{""name"":""Feiertag"",""text"":""Verhindere allen Kampfschaden, der in diesem Zug zugefügt würde."",""type"":""Spontanzauber"",""flavor"":""„Heute ist ein Festtag, Frieden herrscht im ganzen Land, doch der Krieg ist noch nicht beendet. Hebe dir deinen Blutrausch auf. Du wirst ihn morgen wieder brauchen.\"" —Karrim, Samitischer Heiler"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148542&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""4d58444a-c8da-4122-b085-fc41bb95e1ff"",""multiverseId"":148542},""multiverseid"":148542},{""name"":""Día santo"",""text"":""Prevén todo el daño de combate que se fuera a hacer este turno."",""type"":""Instantáneo"",""flavor"":""\""Hoy hay fiesta y paz en nuestra tierra, pero la guerra no terminó. Guarda tu sed de sangre. La necesitarás mañana.\"" —Karrim, sanador samita"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150337&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""e6707d2d-2eec-4af9-98e3-536a8ad10444"",""multiverseId"":150337},""multiverseid"":150337},{""name"":""Jour saint"",""text"":""Prévenez toutes les blessures de combat qui devraient être infligées ce tour-ci."",""type"":""Éphémère"",""flavor"":""« Aujourd'hui, on festoie sur nos terres en toute paix, mais la guerre n'est pas finie. Mettez de côté votre soif de sang. Vous en aurez besoin demain. » —Karrim, guérisseur sanctif"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149954&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""e684549b-1d25-4425-b938-621dd2411a41"",""multiverseId"":149954},""multiverseid"":149954},{""name"":""Giorno Sacro"",""text"":""Previeni tutto il danno da combattimento che verrebbe inflitto in questo turno."",""type"":""Istantaneo"",""flavor"":""\""Oggi nella nostra terra ci sono pace e festeggiamenti, ma la guerra non è ancora finita. Mettete da parte la vostra sete di sangue. Ne avrete bisogno domani.\"" —Karrim, guaritore bianco"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148925&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""6d4b9f4f-e43a-4905-8b64-5132f6e3a7da"",""multiverseId"":148925},""multiverseid"":148925},{""name"":""聖なる日"",""text"":""このターンに与えられるすべての戦闘ダメージを軽減し、０にする。"",""type"":""インスタント"",""flavor"":""今日は祝宴でこの地も平和だが、戦争が終わったわけではない。 血を求める心はしまっておくがいい。明日にはそれが必要となろう。 ――サマイトの癒し手、カリム"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148159&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""146deb43-a416-4629-8a9d-d0b93d9e4b87"",""multiverseId"":148159},""multiverseid"":148159},{""name"":""Dia Sagrado"",""text"":""Previna todo o dano de combate que seria causado neste turno."",""type"":""Mágica Instantânea"",""flavor"":""\""Hoje há fartura e paz em nossas terras, mas a guerra não acabou. Reserve sua sede de sangue. Você vai precisar dela amanhã.\"" — Karrim, curandeiro Samita"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149571&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""39fd04ee-7efe-4b9d-8c67-ca3208cdb780"",""multiverseId"":149571},""multiverseid"":149571},{""name"":""Святой День"",""text"":""Предотвратите все боевые повреждения, которые будут нанесены в этом ходу."",""type"":""Мгновенное заклинание"",""flavor"":""\""Сегодня в нашей земле мир и радость, однако война не окончена. Приберегите свою жажду крови. Она пригодится вам завтра\"". — Каррим, самитский лекарь"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149188&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""ec9dbcf5-12eb-41c2-af34-a8ddb4dcafd6"",""multiverseId"":149188},""multiverseid"":149188},{""name"":""圣日"",""text"":""于本回合中，防止将造成的所有战斗伤害。"",""type"":""瞬间"",""flavor"":""「今天境内一片欢庆平和，但战事还没结束。 把嗜血欲望先收好；你明天就会用到。」 ～撒姆尼人的医疗员卡林"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147776&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""facf8f07-e044-46c4-bf32-472297edbdbf"",""multiverseId"":147776},""multiverseid"":147776}],""printings"":[""10E"",""8ED"",""9ED"",""INV"",""LEG"",""PS11""],""originalText"":""Prevent all combat damage that would be dealt this turn."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""02e23fa1-b2f0-528c-b331-12a0c27bc5eb""},{""name"":""Holy Strength"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature\nEnchanted creature gets +1/+2."",""artist"":""Terese Nielsen"",""number"":""22"",""layout"":""normal"",""multiverseid"":""129594"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129594&type=card"",""variations"":[""79996e77-112f-51e8-980c-0ec82f2d7754""],""foreignNames"":[{""name"":""Heilige Stärke"",""text"":""Kreaturenverzauberung (Bestimme eine Kreatur als Ziel, sowie du diese Karte spielst. Diese Karte kommt an die Kreatur angelegt ins Spiel.)Die verzauberte Kreatur erhält +1/+2."",""type"":""Verzauberung — Aura"",""flavor"":""„Mögen die Engel hinter dir fliegen. Möge deine Waffe die Dunkelheit zertrennen.\"" —Serras Kriegssegen"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148543&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""0183bcd6-1a4d-4ca3-b68f-5212da198272"",""multiverseId"":148543},""multiverseid"":148543},{""name"":""Fuerza sagrada"",""text"":""Encantar criatura. (Haz objetivo a una criatura al jugarlo. Esta carta entra en juego anexada a esa criatura.)\nLa criatura encantada obtiene +1/+2."",""type"":""Encantamiento — Aura"",""flavor"":""\""Que los ángeles vuelen a tu espalda. Que tu espada atraviese la oscuridad.\"" —Bendición de guerra de Serra"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150338&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""d47cf7c4-f023-4d42-938c-633e1cfbbcd8"",""multiverseId"":150338},""multiverseid"":150338},{""name"":""Force sacrée"",""text"":""Enchanter : créature (Ciblez une créature au moment où vous jouez cette carte. Cette carte arrive en jeu attachée à cette créature.)\nLa créature enchantée gagne +1/+2."",""type"":""Enchantement : aura"",""flavor"":""« Que les anges couvrent tes arrières. Que ta lame tranche les ténèbres. » —Bénédiction de guerre de Serra"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149955&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""5a37a14e-b67c-433a-ac4e-6f3ea1ac0b2a"",""multiverseId"":149955},""multiverseid"":149955},{""name"":""Forza Sacra"",""text"":""Incanta creatura (Bersaglia una creatura mentre giochi questa carta. Questa carta entra in gioco assegnata a quella creatura.)\nLa creatura incantata prende +1/+2."",""type"":""Incantesimo — Aura"",""flavor"":""\""Possano gli angeli seguirti in volo. Possa la tua spada fendere l'oscurità.\"" —Benedizione bellica di Serra"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148926&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""69c77d80-9d86-482a-bee5-9c843c4d55ff"",""multiverseId"":148926},""multiverseid"":148926},{""name"":""聖なる力"",""text"":""エンチャント（クリーチャー） （これをプレイする際に、クリーチャー１体を対象とする。 このカードはそのクリーチャーにつけられている状態で場に出る。）\nエンチャントされているクリーチャーは＋１/＋２の修整を受ける。"",""type"":""エンチャント — オーラ"",""flavor"":""天使がその背に羽ばたかんことを。 その剣が闇を裂かんことを。 ――セラの戦いの祝福"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148160&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""3ef760c6-555f-4189-9b9b-90ff55c5a7a2"",""multiverseId"":148160},""multiverseid"":148160},{""name"":""Força Divina"",""text"":""Encantar criatura (Ao jogar este card, escolha uma criatura alvo. Este card entra em jogo anexado àquela criatura.)\nA criatura encantada recebe +1/+2."",""type"":""Encantamento — Aura"",""flavor"":""\""Que os anjos voem atrás de você. Que sua espada possa romper a escuridão.\"" —Benção de guerra de Serra"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149572&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""11d2f465-2833-474d-a2b2-0b36be0ceaa4"",""multiverseId"":149572},""multiverseid"":149572},{""name"":""Святая Сила"",""text"":""Зачаровать существо (При разыгрывании этой карты выберите целью существо. Эта карта входит в игру прикрепленной к тому существу.)\nЗачарованное существо получает +1/+2."",""type"":""Чары — Аура"",""flavor"":""\""Да летят ангелы у тебя за спиной. Да разит темноту меч твой\"". — Благословление Серры на войну"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149189&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""beb56911-4dad-4c1c-84d0-a8a4789ed41b"",""multiverseId"":149189},""multiverseid"":149189},{""name"":""神圣之力"",""text"":""生物结界（于使用时指定一个生物为目标。 此牌进场时结附在该生物上。）\n受此结界的生物得+1/+2。"",""type"":""结界～灵气"",""flavor"":""「愿天使随你背后翱翔。 愿你刀刃划破黑暗。」 ～撒拉战场祈愿"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147777&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""e786ad59-1173-44b3-b545-71049e96468c"",""multiverseId"":147777},""multiverseid"":147777}],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""5ED"",""7ED"",""8ED"",""9ED"",""CED"",""CEI"",""FBB"",""LEA"",""LEB"",""M10"",""M11"",""PS11"",""SUM""],""originalText"":""Enchant creature\nEnchanted creature gets +1/+2."",""originalType"":""Enchantment - Aura"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""4e4cbdec-e2d4-5f31-98a3-f2ef052c849d""},{""name"":""Holy Strength"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature\nEnchanted creature gets +1/+2."",""flavor"":""\""May angels fly at your back. May your blade cleave the darkness.\""\n—War blessing of Serra"",""artist"":""Terese Nielsen"",""number"":""22★"",""layout"":""normal"",""variations"":[""4e4cbdec-e2d4-5f31-98a3-f2ef052c849d""],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""5ED"",""7ED"",""8ED"",""9ED"",""CED"",""CEI"",""FBB"",""LEA"",""LEB"",""M10"",""M11"",""PS11"",""SUM""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""79996e77-112f-51e8-980c-0ec82f2d7754""},{""name"":""Honor Guard"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Soldier"",""types"":[""Creature""],""subtypes"":[""Human"",""Soldier""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{W}: Honor Guard gets +0/+1 until end of turn."",""flavor"":""The strength of one. The courage of ten."",""artist"":""Dan Dos Santos"",""number"":""23"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""129595"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129595&type=card"",""foreignNames"":[{""name"":""Ehrengarde"",""text"":""{W}: Die Ehrengarde erhält +0/+1 bis zum Ende des Zuges."",""type"":""Kreatur — Mensch, Soldat"",""flavor"":""Die Stärke von einem, den Mut von zehn."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148544&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""5a81c265-665b-404d-952a-742e035495fa"",""multiverseId"":148544},""multiverseid"":148544},{""name"":""Guardia de honor"",""text"":""{W}: El Guardia de honor obtiene +0/+1 hasta el final del turno."",""type"":""Criatura — Soldado humano"",""flavor"":""La fuerza de uno. El valor de diez."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150339&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""ab8e4bf4-e517-418e-aa2a-1e1162d9fceb"",""multiverseId"":150339},""multiverseid"":150339},{""name"":""Garde d'honneur"",""text"":""{W} : La Garde d'honneur gagne +0/+1 jusqu'à la fin du tour."",""type"":""Créature : humain et soldat"",""flavor"":""La force d'un. Le courage de dix."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149956&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""4c81d32a-6b46-45ad-8c9e-cf640cfcab94"",""multiverseId"":149956},""multiverseid"":149956},{""name"":""Guardia d'Onore"",""text"":""{W}: La Guardia d'Onore prende +0/+1 fino alla fine del turno."",""type"":""Creatura — Soldato Umano"",""flavor"":""La forza di uno. Il coraggio di dieci."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148927&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""5e2e4f5b-8979-4e5b-b153-6c15bd38a8cc"",""multiverseId"":148927},""multiverseid"":148927},{""name"":""儀仗兵"",""text"":""{W}：儀仗兵はターン終了時まで＋０/＋１の修整を受ける。"",""type"":""クリーチャー — 人間・兵士"",""flavor"":""一人の力は 十人の勇気。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148161&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""a36dc597-755c-4bce-ad64-b2741353896b"",""multiverseId"":148161},""multiverseid"":148161},{""name"":""Guarda de Honra"",""text"":""{W}: Guarda de Honra recebe +0/+1 até o final do turno."",""type"":""Criatura — Humano Soldado"",""flavor"":""A força de um. A coragem de dez."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149573&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""1b138078-a1c8-42a1-a168-0d9dc65e8de5"",""multiverseId"":149573},""multiverseid"":149573},{""name"":""Почетный Караул"",""text"":""{W}: Почетный Караул получает +0/+1 до конца хода."",""type"":""Существо — Человек Солдат"",""flavor"":""Сила одного. Храбрость десяти."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149190&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""2f91ab28-6c32-4469-bdfc-8a4058c0c488"",""multiverseId"":149190},""multiverseid"":149190},{""name"":""仪队兵"",""text"":""{W}：仪队兵得+0/+1直到回合结束。"",""type"":""生物～人类／士兵"",""flavor"":""一分力量， 十分勇气。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147778&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""5f12fbc0-6b30-46fa-91ff-e57a56e3e1de"",""multiverseId"":147778},""multiverseid"":147778}],""printings"":[""10E"",""7ED"",""8ED"",""9ED"",""STH""],""originalText"":""{W}: Honor Guard gets +0/+1 until end of turn."",""originalType"":""Creature - Human Soldier"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""9bee1901-1125-5635-9e22-3b4f37989c37""},{""name"":""Icatian Priest"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Cleric""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{1}{W}{W}: Target creature gets +1/+1 until end of turn."",""flavor"":""Grelden knelt and felt the cool, dry hand of the priest on his brow. Hours later, when his wits returned, he was covered in his enemies' blood on the field of victory."",""artist"":""Stephen Tappin"",""number"":""24"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""132123"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132123&type=card"",""foreignNames"":[{""name"":""Icatianischer Priester"",""text"":""{1}{W}{W}: Eine Kreatur deiner Wahl erhält +1/+1 bis zum Ende des Zuges."",""type"":""Kreatur — Mensch, Kleriker"",""flavor"":""Grelden kniete nieder und spürte die kalte, trockene Hand des Priesters auf seiner Stirn. Als er Stunden später wieder zu sich kam, war er vom Blut seiner Feinde befleckt, aber siegreich und am Leben."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148551&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""20762a16-b1a1-4f03-a165-dd6a8444e5f7"",""multiverseId"":148551},""multiverseid"":148551},{""name"":""Sacerdote icatiano"",""text"":""{1}{W}{W}: La criatura objetivo obtiene +1/+1 hasta el final del turno."",""type"":""Criatura — Clérigo humano"",""flavor"":""Grelden se arrodilló y sintió la mano fría y seca del sacerdote en su frente. Horas más tarde, cuando volvió en sí, estaba cubierto de la sangre de sus enemigos en el campo de la victoria."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150340&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""ab2bf5b8-bda2-43cb-845d-edb4960a6b29"",""multiverseId"":150340},""multiverseid"":150340},{""name"":""Prêtre d'Icatia"",""text"":""{1}{W}{W} : La créature ciblée gagne +1/+1 jusqu'à la fin du tour."",""type"":""Créature : humain et clerc"",""flavor"":""Grelden s'agenouilla et sentit la main froide et sèche du prêtre sur son front. Plusieurs heures plus tard, quand il recouvra ses esprits, il était trempé du sang de ses ennemis sur le champ de la victoire."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149957&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""c096dd5b-c544-440a-ae13-fb245fdb593d"",""multiverseId"":149957},""multiverseid"":149957},{""name"":""Sacerdote di Icatia"",""text"":""{1}{W}{W}: Una creatura bersaglio prende +1/+1 fino alla fine del turno."",""type"":""Creatura — Chierico Umano"",""flavor"":""Grelden si inginocchiò e sentì sulla fronte la fredda mano asciutta del sacerdote. Parecchie ore dopo, recuperato il senno, si ritrovò coperto di sangue nemico sul campo di vittoria."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148934&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""fa1d6ddb-1fd2-4e83-abc7-61c695ab4c58"",""multiverseId"":148934},""multiverseid"":148934},{""name"":""アイケイシアの僧侶"",""text"":""{1}{W}{W}：クリーチャー１体を対象とする。それはターン終了時まで＋１/＋１の修整を受ける。"",""type"":""クリーチャー — 人間・クレリック"",""flavor"":""ひざまずいたグレンデルは、僧侶の冷たく乾いた手が傷を覆うのを感じた。 数時間後に気づいたとき、彼は戦場で敵の血にまみれながら勝利を手にしていた。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148168&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""af048ae6-c951-4c0d-8b6b-9853d65ce889"",""multiverseId"":148168},""multiverseid"":148168},{""name"":""Sacerdote Icatiano"",""text"":""{1}{W}{W}: A criatura alvo recebe +1/+1 até o final do turno."",""type"":""Criatura — Humano Clérigo"",""flavor"":""Grelden ajoelhou-se e sentiu a mão fria e seca do sacerdote em sua fronte. Horas mais tarde, quando voltou a ter consciência, estava coberto com o sangue de seus inimigos no campo da vitória."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149574&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""bc947e87-5c56-4a43-ad5e-d454de73b4c4"",""multiverseId"":149574},""multiverseid"":149574},{""name"":""Айкейшунский Священник"",""text"":""{1}{W}{W}: Целевое существо получает +1/+1 до конца хода."",""type"":""Существо — Человек Священник"",""flavor"":""Грельден преклонил колена и почувствовал, как на лоб легла прохладная и сухая рука священника. Много часов спустя, когда к нему вернулось сознание, он, обагренный кровью своих врагов, был победителем на поле боя."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149191&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""fd092ddb-bee3-4fd6-8cc2-d673bcb5995c"",""multiverseId"":149191},""multiverseid"":149191},{""name"":""艾凯逊祝祷士"",""text"":""{1}{W}{W}：目标生物得+1/+1直到回合结束。"",""type"":""生物～人类／僧侣"",""flavor"":""盖登跪下，感到祝祷士冰凉干燥的手贴上自己额头。 数小时后他回复神智，发现自己立于胜利战场，敌人鲜血溅了满身。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147785&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""4c6eba01-53e2-4381-a154-8d9b83e84eec"",""multiverseId"":147785},""multiverseid"":147785}],""printings"":[""10E"",""DDC"",""DVD"",""FEM""],""originalText"":""{1}{W}{W}: Target creature gets +1/+1 until end of turn."",""originalType"":""Creature - Human Cleric"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""cb865391-5b14-58ce-aa99-b36b83e6377f""},{""name"":""Kjeldoran Royal Guard"",""manaCost"":""{3}{W}{W}"",""cmc"":5.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Soldier"",""types"":[""Creature""],""subtypes"":[""Human"",""Soldier""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{T}: All combat damage that would be dealt to you by unblocked creatures this turn is dealt to Kjeldoran Royal Guard instead."",""flavor"":""Upon the frozen tundra stand the Kjeldoran Royal Guard, pikes raised, with the king's oath upon their lips."",""artist"":""Carl Critchlow"",""number"":""25"",""power"":""2"",""toughness"":""5"",""layout"":""normal"",""multiverseid"":""130551"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130551&type=card"",""rulings"":[{""date"":""2004-10-04"",""text"":""If you activate the ability but Kjeldoran Royal Guard leaves the battlefield before combat damage is dealt, the combat damage from unblocked creatures won’t be redirected. It will be dealt to you as normal.""}],""foreignNames"":[{""name"":""Kjeldoranische Hofgarde"",""text"":""{T}: Aller Kampfschaden, der dir in diesem Zug von ungeblockten Kreaturen zugefügt würde, wird stattdessen der Kjeldoranischen Hofgarde zugefügt."",""type"":""Kreatur — Mensch, Soldat"",""flavor"":""Mit erhobenen Piken und dem Eid auf den Lippen, den sie ihrem König geleistet haben, bewacht die Hofgarde die gefrorene Tundra."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148565&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""b66a17ac-dae4-461e-b6b3-da1ab595e3ea"",""multiverseId"":148565},""multiverseid"":148565},{""name"":""Guardia Real kjeldorana"",""text"":""{T}: Todo el daño de combate que te fueran a hacer criaturas no bloqueadas este turno, en vez de eso, se le hace a la Guardia Real kjeldorana."",""type"":""Criatura — Soldado humano"",""flavor"":""Sobre la tundra congelada se alza la Guardia Real kjeldorana, con las picas en alto y el juramento del rey en sus labios."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150341&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""ab6383f8-207a-4659-bbd8-7e71c0dd1a47"",""multiverseId"":150341},""multiverseid"":150341},{""name"":""Garde royale du Kjeldor"",""text"":""{T} : Toutes les blessures de combat qui devraient vous être infligées ce tour-ci par des créatures non-bloquées sont, à la place, infligées à la Garde royale du Kjeldor."",""type"":""Créature : humain et soldat"",""flavor"":""La garde royale du Kjeldor se tenait sur la toundra gelée, armes au clair, le serment du roi sur les lèvres."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149958&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""f87a2201-960a-4a09-9739-4b62fa1869b0"",""multiverseId"":149958},""multiverseid"":149958},{""name"":""Guardia Reale di Kjeldor"",""text"":""{T}: Tutto il danno da combattimento che ti verrebbe inflitto da creature non bloccate in questo turno viene invece inflitto alla Guardia Reale di Kjeldor."",""type"":""Creatura — Soldato Umano"",""flavor"":""Nella tundra gelata si erge la Guardia Reale di Kjeldor, con le picche alzate e il giuramento al re sulle labbra."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148948&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""ccd1e201-35ff-4e1c-809f-013e640a85ec"",""multiverseId"":148948},""multiverseid"":148948},{""name"":""キイェルドーの近衛隊"",""text"":""{T}：このターン、ブロックされていないクリーチャーがあなたに与えるすべての戦闘ダメージは、代わりにキイェルドーの近衛隊に与えられる。"",""type"":""クリーチャー — 人間・兵士"",""flavor"":""キイェルドーの近衛隊は凍りついたツンドラの上に立ち、王への誓いを口にしつつ矛を掲げている。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148182&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""16882c4b-21a3-45b2-880f-6468fccd0f73"",""multiverseId"":148182},""multiverseid"":148182},{""name"":""Guarda Real Kjeldorana"",""text"":""{T}: Todo o dano de combate que seria causado a você por criaturas não bloqueadas neste turno, em vez disso, será causado à Guarda Real Kjeldorana."",""type"":""Criatura — Humano Soldado"",""flavor"":""Na tundra congelada encontra-se a Guarda Real Kjeldorana, lanças ao alto, com o juramento ao rei em seus lábios."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149575&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""41dec82b-4fa8-4d9a-bd5d-6d08b411ebbf"",""multiverseId"":149575},""multiverseid"":149575},{""name"":""Королевская Стража Кьельдора"",""text"":""{T}: Все боевые повреждения, которые будут нанесены вам в этом ходу незаблокированными существами, наносятся Королевской Страже Кьельдора вместо этого."",""type"":""Существо — Человек Солдат"",""flavor"":""В промерзлой тундре несет караул королевская стража Кьельдора: их копья высоко подняты, а на устах застыла клятва их короля."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149192&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""ca3bd9c6-963c-44e7-bb90-f856367be9b5"",""multiverseId"":149192},""multiverseid"":149192},{""name"":""奇亚多朗皇家护卫"",""text"":""{T}：本回合中，未受阻挡的生物将对你造成的所有战斗伤害都改为对奇亚多朗皇家护卫造成之。"",""type"":""生物～人类／士兵"",""flavor"":""奇亚多朗皇家护卫挺立冰雪苔原，长枪耸立，永遵王誓。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147799&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""ba16844a-1f42-463e-993c-235982d397b1"",""multiverseId"":147799},""multiverseid"":147799}],""printings"":[""10E"",""5ED"",""6ED"",""7ED"",""ICE""],""originalText"":""{T}: All combat damage that would be dealt to you by unblocked creatures this turn is dealt to Kjeldoran Royal Guard instead."",""originalType"":""Creature - Human Soldier"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""bb6e511d-3e7f-5dd3-b40c-dbeb09c734f7""},{""name"":""Loxodon Mystic"",""manaCost"":""{3}{W}{W}"",""cmc"":5.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Elephant Cleric"",""types"":[""Creature""],""subtypes"":[""Elephant"",""Cleric""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{W}, {T}: Tap target creature."",""flavor"":""Elder mystics take their vow of silence so seriously that they impose it on any who enter their presence."",""artist"":""Randy Gallegos"",""number"":""26"",""power"":""3"",""toughness"":""3"",""layout"":""normal"",""multiverseid"":""129638"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129638&type=card"",""foreignNames"":[{""name"":""Loxodon-Mystiker"",""text"":""{W}, {T}: Tappe eine Kreatur deiner Wahl."",""type"":""Kreatur — Elefant, Kleriker"",""flavor"":""Die älteren Mystiker nehmen ihr Schweigegelübde so ernst, dass sie es allen auferlegen, von denen sie besucht werden."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148579&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""0d8871ec-c888-4530-9e8a-832aebd94ec6"",""multiverseId"":148579},""multiverseid"":148579},{""name"":""Místico loxodón"",""text"":""{W}, {T}: Gira la criatura objetivo."",""type"":""Criatura — Clérigo elefante"",""flavor"":""Los ancianos místicos toman tan en serio su voto de silencio que lo imponen sobre cualquiera que entre en su presencia."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150342&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""b6717e44-27df-47f3-9a48-1bc2955acd9e"",""multiverseId"":150342},""multiverseid"":150342},{""name"":""Mystique loxodon"",""text"":""{W}, {T} : Engagez la créature ciblée."",""type"":""Créature : éléphant et clerc"",""flavor"":""Les anciens mystiques prennent leur vœu de silence tellement au sérieux qu'ils l'imposent à ceux qui leur demandent audience."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149959&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""dd335c92-528a-4beb-b03d-a67ae99926ab"",""multiverseId"":149959},""multiverseid"":149959},{""name"":""Mistico Lossodonte"",""text"":""{W}, {T}: TAPpa una creatura bersaglio."",""type"":""Creatura — Chierico Elefante"",""flavor"":""I mistici anziani prendono così seriamente il voto di silenzio da imporlo a chiunque si presenti al loro cospetto."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148962&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""2d2ba6f8-9b49-40bb-b963-809f1c943b51"",""multiverseId"":148962},""multiverseid"":148962},{""name"":""ロクソドンの神秘家"",""text"":""{W}, {T}：クリーチャー１体を対象とし、それをタップする。"",""type"":""クリーチャー — 象・クレリック"",""flavor"":""神秘家の長老は静寂の誓約を非常に真摯に守るため、その存在に近づいた者にすらそれを強要する。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148196&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""93111623-c04f-4b90-b77c-4219f74f0b73"",""multiverseId"":148196},""multiverseid"":148196},{""name"":""Loxodonte Místico"",""text"":""{W}, {T}: Vire a criatura alvo."",""type"":""Criatura — Elefante Clérigo"",""flavor"":""Os místicos mais antigos levam seu voto de silêncio tão a sério que o impõem a qualquer um que esteja em sua presença."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149576&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""d47220be-49c7-446f-89d4-14b0b76800f1"",""multiverseId"":149576},""multiverseid"":149576},{""name"":""Локсодонский Мистик"",""text"":""{W}, {T}: Поверните целевое существо."",""type"":""Существо — Слон Священник"",""flavor"":""Старшие жрецы придают такое серьезное значение своему обету молчания, что налагают его на всякого, кто находится в их присутствии."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149193&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""866223b1-bf20-44c1-8f69-0c103fd262cf"",""multiverseId"":149193},""multiverseid"":149193},{""name"":""象族秘教徒"",""text"":""{W}，{T}：横置目标生物。"",""type"":""生物～象／僧侣"",""flavor"":""秘教徒长者严循禁语誓言，就连在身边的人也必须奉行。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147813&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""8be00241-dc62-4b73-be7a-9ab197cb3378"",""multiverseId"":147813},""multiverseid"":147813}],""printings"":[""10E"",""DST""],""originalText"":""{W}, {T}: Tap target creature."",""originalType"":""Creature - Elephant Cleric"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""73e8c198-4ba4-5f42-9781-167150eae4be""},{""name"":""Loyal Sentry"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Soldier"",""types"":[""Creature""],""subtypes"":[""Human"",""Soldier""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""When Loyal Sentry blocks a creature, destroy that creature and Loyal Sentry."",""flavor"":""\""My cause is simple: To stop you, at any cost, from ever seeing the inside of this keep.\"""",""artist"":""Michael Sutfin"",""number"":""27"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""129798"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129798&type=card"",""rulings"":[{""date"":""2018-03-16"",""text"":""Loyal Sentry and the creature it blocks are destroyed before combat damage is dealt. The blocked creature is destroyed even if Loyal Sentry leaves the battlefield before its triggered ability resolves.""}],""foreignNames"":[{""name"":""Loyaler Wachposten"",""text"":""Wenn der Loyale Wachposten eine Kreatur blockt, zerstöre diese Kreatur und den Loyalen Wachposten."",""type"":""Kreatur — Mensch, Soldat"",""flavor"":""„Meine Aufgabe ist ganz einfach: dich daran zu hindern, jemals das Innere dieser Festung zu sehen, koste es, was es wolle.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148581&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""497d0db9-da3e-43cd-9867-a10de47dc867"",""multiverseId"":148581},""multiverseid"":148581},{""name"":""Centinela leal"",""text"":""Cuando el Centinela leal bloquee a una criatura, destruye esa criatura y al Centinela leal."",""type"":""Criatura — Soldado humano"",""flavor"":""\""Mi causa es simple: detenerte, a cualquier costo, evitar que incluso veas el interior de esta fortaleza.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150343&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""88f8cb02-c3ed-4c0f-9915-1341fa645f13"",""multiverseId"":150343},""multiverseid"":150343},{""name"":""Sentinelle loyale"",""text"":""Quand la Sentinelle loyale bloque une créature, détruisez cette créature et la Sentinelle loyale."",""type"":""Créature : humain et soldat"",""flavor"":""« Mon rôle est simple : vous empêcher à tout prix de mettre les pieds dans cette forteresse. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149960&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""d1cfb912-4381-45ef-88c4-3fcb888630ad"",""multiverseId"":149960},""multiverseid"":149960},{""name"":""Sentinella Fidata"",""text"":""Quando la Sentinella Fidata blocca una creatura, distruggi quella creatura e la Sentinella Fidata."",""type"":""Creatura — Soldato Umano"",""flavor"":""\""La mia missione è semplice: impedirti di entrare in questa fortezza, a qualsiasi costo.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148964&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""528668f0-ea8c-4418-85c5-ce62bc3d9fbd"",""multiverseId"":148964},""multiverseid"":148964},{""name"":""忠誠な歩哨"",""text"":""忠誠な歩哨がいずれかのクリーチャーをブロックしたとき、そのクリーチャーと忠誠な歩哨を破壊する。"",""type"":""クリーチャー — 人間・兵士"",""flavor"":""我が目的は単純だ。あらゆる手段を以ってお前を止め、砦の中を決して見せぬことだ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148198&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""4e584156-6744-457a-ad42-974234fae742"",""multiverseId"":148198},""multiverseid"":148198},{""name"":""Sentinela Leal"",""text"":""Quando Sentinela Leal bloquear uma criatura, destrua aquela criatura e Sentinela Leal."",""type"":""Criatura — Humano Soldado"",""flavor"":""\""Minha causa é simples: Impedi-lo, a qualquer custo, de ver o interior desta fortaleza.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149577&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""8f82ff4d-a18c-4ce3-a6dc-9911d1878049"",""multiverseId"":149577},""multiverseid"":149577},{""name"":""Верный Страж"",""text"":""Когда Верный Страж блокирует существо, уничтожьте то существо и Верного Стража."",""type"":""Существо — Человек Солдат"",""flavor"":""\""У меня простая цель: любой ценой не дать вам увидеть, никогда, то, что находится внутри этой крепости\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149194&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""17c9a343-5fc9-4b1f-8b32-f3ace3efc8f8"",""multiverseId"":149194},""multiverseid"":149194},{""name"":""忠诚哨兵"",""text"":""当忠诚哨兵阻挡一个生物时，消灭该生物与忠诚哨兵。"",""type"":""生物～人类／士兵"",""flavor"":""「我的使命很简单：让你永远看不到要塞内部，无论任何代价。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147815&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""71f91fd8-0987-4a2f-b4ee-4deb92e69a29"",""multiverseId"":147815},""multiverseid"":147815}],""printings"":[""10E"",""A25"",""DDF"",""PLST"",""S99""],""originalText"":""When Loyal Sentry blocks a creature, destroy that creature and Loyal Sentry."",""originalType"":""Creature - Human Soldier"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""3f809288-0eaa-5f55-8144-483ed8ea810c""},{""name"":""Luminesce"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Prevent all damage that black sources and red sources would deal this turn."",""flavor"":""\""The White Shield is not the burnished metal you lash to your forearm but the conviction that burns in your chest.\""\n—Lucilde Fiksdotter, leader of the Order of the White Shield"",""artist"":""Daren Bader"",""number"":""28"",""layout"":""normal"",""multiverseid"":""129912"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129912&type=card"",""foreignNames"":[{""name"":""Nachleuchten"",""text"":""Verhindere allen Schaden, den schwarze und rote Quellen in diesem Zug zufügen würden."",""type"":""Spontanzauber"",""flavor"":""„Der Weiße Schild ist nicht das polierte Stück Metall, das du an deinem Arm befestigst, sondern die Überzeugung, die in deinem Herzen brennt.\"" —Lucilde Fiksdotter, Anführerin des Ordens des Weißen Schildes"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148583&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""f13bfd46-84b9-4bfd-8dc8-ee7d4bdb479d"",""multiverseId"":148583},""multiverseid"":148583},{""name"":""Luminescer"",""text"":""Prevén todo el daño que fueran a hacer este turno fuentes negras y fuentes rojas."",""type"":""Instantáneo"",""flavor"":""\""El Escudo Blanco no es el metal pulido que cuelgas de tu brazo, sino la convicción que quema en tu pecho.\"" —Lucilde Fiksdotter, líder de la Orden del Escudo Blanco"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150344&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""d76343e8-060f-4f67-9487-c278b106bd36"",""multiverseId"":150344},""multiverseid"":150344},{""name"":""Luminescence"",""text"":""Prévenez toutes les blessures que des sources noires et des sources rouges devraient infliger ce tour-ci."",""type"":""Éphémère"",""flavor"":""« Le Bouclier blanc n'est pas le métal que vous portez à l'avant-bras, mais la conviction qui brûle dans votre poitrine. » —Lucilde Fiksdotter, capitaine de l'Ordre du Bouclier blanc"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149961&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""68074cb2-41fb-40e3-aba5-f4dd7b1d2795"",""multiverseId"":149961},""multiverseid"":149961},{""name"":""Luminescenza"",""text"":""Previeni tutto il danno che le fonti nere e le fonti rosse infliggerebbero in questo turno."",""type"":""Istantaneo"",""flavor"":""\""Lo Scudo Bianco non è il pezzo di metallo che fissi all'avambraccio ma il fermo convincimento che brucia nel tuo petto.\"" —Lucilde Fiksdotter, capo dell'Ordine dello Scudo Bianco"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148966&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""ddf54e2f-c5e6-4023-903d-bfe09ea6f044"",""multiverseId"":148966},""multiverseid"":148966},{""name"":""発光"",""text"":""このターン、赤か黒の発生源が与えるダメージをすべて軽減し、０にする。"",""type"":""インスタント"",""flavor"":""白き盾は腕に結びつける磨かれた鋼などではありません。胸の中に燃える信念なのです。 ――白き盾の騎士団の団長、 ルシルド・フィクスドッター"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148200&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""cb457f87-bd9d-461a-8982-e2fde4997df6"",""multiverseId"":148200},""multiverseid"":148200},{""name"":""Luminescência"",""text"":""Previna todo o dano que as fontes pretas e vermelhas causariam neste turno."",""type"":""Mágica Instantânea"",""flavor"":""\""O Escudo Branco não é o metal polido que você amarra ao antebraço, mas a convicção que arde em seu peito.\"" — Lucilde Fiksdotter, líder da Ordem do Escudo Branco"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149578&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""72344e8e-135f-4baa-b2ce-1d601ae23d20"",""multiverseId"":149578},""multiverseid"":149578},{""name"":""Свечение"",""text"":""Предотвратите все повреждения, которые черные источники и красные источники нанесут в этом ходу."",""type"":""Мгновенное заклинание"",""flavor"":""\""Белый Щит — не просто блестящий кусок металла, пристегнутый к твоей руке. Это убеждение, горящее в твоей груди\"". — Люсильда Фиксдоттер, глава Ордена Белого Щита"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149195&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""9c04afc0-c31a-4f74-969f-f4fd3614991d"",""multiverseId"":149195},""multiverseid"":149195},{""name"":""辉光"",""text"":""防止黑色来源与红色来源于本回合中将造成的所有伤害。"",""type"":""瞬间"",""flavor"":""「白盾并不是扎在你前臂的那片光亮金属，而是燃烧在你胸膛的信念。」 ～白盾骑士团长 露西妲·斐斯朵"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147817&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""f8cf9f7c-26cc-4921-84ba-00e0d15c1f5d"",""multiverseId"":147817},""multiverseid"":147817}],""printings"":[""10E"",""CSP""],""originalText"":""Prevent all damage that black sources and red sources would deal this turn."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""efe6103c-6a3e-5ed0-a689-81a2efe4273b""},{""name"":""Mobilization"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment"",""types"":[""Enchantment""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Soldier creatures have vigilance.\n{2}{W}: Create a 1/1 white Soldier creature token."",""flavor"":""Wars are won with strength, valor, and numbers—especially numbers."",""artist"":""Carl Critchlow"",""number"":""29"",""layout"":""normal"",""multiverseid"":""129716"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129716&type=card"",""variations"":[""1afcbee1-1310-5144-86ea-e7bf0fb34d2b""],""foreignNames"":[{""name"":""Mobilisierung"",""text"":""Soldatenkreaturen haben Wachsamkeit. (Sie werden beim Angreifen nicht getappt.)\n{2}{W}: Bringe einen 1/1 weißen Soldatenkreaturspielstein ins Spiel."",""type"":""Verzauberung"",""flavor"":""Kriege werden durch Stärke, Tapferkeit und Anzahl der Kämpfer gewonnen — vor allem durch letzteres."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148600&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""af802059-2ec2-4085-a69c-6644397d287a"",""multiverseId"":148600},""multiverseid"":148600},{""name"":""Movilización"",""text"":""Las criaturas Soldado tienen la habilidad de vigilancia. (No se giran al atacar.)\n{2}{W}: Pon en juego una ficha de criatura Soldado blanca 1/1."",""type"":""Encantamiento"",""flavor"":""Las guerras se ganan con fuerza, valor y números... especialmente números."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150345&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""efbac946-1f18-481a-8e06-a5680b24614f"",""multiverseId"":150345},""multiverseid"":150345},{""name"":""Incorporation"",""text"":""Les créatures Soldat ont la vigilance. (Attaquer avec ces créatures ne les fait pas s'engager.)\n{2}{W} : Mettez en jeu un jeton de créature 1/1 blanche Soldat."",""type"":""Enchantement"",""flavor"":""Les guerres se gagnent avec force, courage et nombre — particulièrement le nombre."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149962&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""dc7079f4-6652-4a41-9502-30dbff48eed6"",""multiverseId"":149962},""multiverseid"":149962},{""name"":""Mobilitazione"",""text"":""Le creature Soldato hanno cautela. (Attaccano senza TAPpare.)\n{2}{W}: Metti in gioco una pedina creatura Soldato 1/1 bianca."",""type"":""Incantesimo"",""flavor"":""Le guerre sono vinte con la forza, il valore e i numeri. . . soprattutto i numeri."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148983&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""93722dc9-6047-4d39-8188-6ccf440e995c"",""multiverseId"":148983},""multiverseid"":148983},{""name"":""動員令"",""text"":""兵士クリーチャーは警戒を持つ。 （それらは攻撃してもタップしない。）\n{2}{W}：白の１/１の兵士クリーチャー・トークンを１体場に出す。"",""type"":""エンチャント"",""flavor"":""戦争は、力と勇気と数で勝利する――とりわけ、数が重要だ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148217&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""d24f0383-602b-426b-b54b-24c3c2161f67"",""multiverseId"":148217},""multiverseid"":148217},{""name"":""Mobilização"",""text"":""As criaturas do tipo Soldado têm vigilância. (Elas não são viradas para atacar.)\n{2}{W}: Coloque em jogo uma ficha de criatura branca 1/1 do tipo Soldado."",""type"":""Encantamento"",""flavor"":""As guerras são vencidas com força, coragem e números especialmente números."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149579&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""5b5516ce-1bc9-42a3-8afd-db7b5ac103d4"",""multiverseId"":149579},""multiverseid"":149579},{""name"":""Мобилизация"",""text"":""Существа Солдаты имеют Бдительность. (При нападении они не поворачиваются.)\n{2}{W}: Положите в игру одну фишку существа 1/1 белый Солдат."",""type"":""Чары"",""flavor"":""Победу в войне приносят сила, храбрость и численность особенно численность."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149196&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""3f7a1434-94d3-45af-819b-d6c3d1091fe2"",""multiverseId"":149196},""multiverseid"":149196},{""name"":""动员时期"",""text"":""士兵生物具有警戒异能。 （它们攻击时不需横置。）\n{2}{W}：将一个1/1白色士兵衍生物放置进场。"",""type"":""结界"",""flavor"":""胜仗得靠力量，英勇，以及数量～尤其是数量。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147834&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""a05238b0-05e3-4d23-83de-fd63e7deb822"",""multiverseId"":147834},""multiverseid"":147834}],""printings"":[""10E"",""C14"",""ONS""],""originalText"":""Soldier creatures have vigilance.\n{2}{W}: Put a 1/1 white Soldier creature token into play."",""originalType"":""Enchantment"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""7f1bea50-20c0-51b3-8b2a-ac60f5d31133""},{""name"":""Mobilization"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment"",""types"":[""Enchantment""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Soldier creatures have vigilance.\n{2}{W}: Create a 1/1 white Soldier creature token."",""flavor"":""Wars are won with strength, valor, and numbers—especially numbers."",""artist"":""Carl Critchlow"",""number"":""29★"",""layout"":""normal"",""variations"":[""7f1bea50-20c0-51b3-8b2a-ac60f5d31133""],""printings"":[""10E"",""C14"",""ONS""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""1afcbee1-1310-5144-86ea-e7bf0fb34d2b""},{""name"":""Nomad Mythmaker"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Nomad Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Nomad"",""Cleric""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{W}, {T}: Put target Aura card from a graveyard onto the battlefield under your control attached to a creature you control."",""flavor"":""On the wild steppes, history vanishes in the dust. Only the mythmakers remain to say what was, and is, and will be."",""artist"":""Darrell Riche"",""number"":""30"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""130547"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130547&type=card"",""variations"":[""ac19e0e0-6d8e-55ad-a7a9-68fd6032eaab""],""rulings"":[{""date"":""2004-10-04"",""text"":""You don’t choose which creature the Aura will enter the battlefield attached to until the ability resolves. You must choose a creature the Aura can legally enchant. (For example, you can’t choose a creature with protection from black if the targeted Aura card is black.) If you don’t control any creatures that the Aura can enchant, it remains in the graveyard.""},{""date"":""2007-07-15"",""text"":""Nomad Mythmaker’s ability can target an Aura card in any graveyard, not just yours.""}],""foreignNames"":[{""name"":""Sagenerzähler der Nomaden"",""text"":""{W}, {T}: Bringe eine Aurakarte deiner Wahl aus einem Friedhof an eine Kreatur, die du kontrollierst, angelegt ins Spiel. (Du kontrollierst diese Aura.)"",""type"":""Kreatur — Mensch, Nomade, Kleriker"",""flavor"":""In den wilden Steppen verweht die Geschichte im staubigen Wind. Nur die Sagenerzähler verbleiben, um zu berichten, was war, was ist und was sein wird."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148615&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""8e0de2b2-8ba0-45ee-86c6-3f11e9d19be0"",""multiverseId"":148615},""multiverseid"":148615},{""name"":""Creamitos nómada"",""text"":""{W}, {T}: Pon en juego la carta de aura objetivo de un cementerio anexada a una criatura que controlas. (Tú controlas ese aura.)"",""type"":""Criatura — Clérigo nómada humano"",""flavor"":""En las salvajes estepas, la historia se desvanece en el polvo. Sólo quedan los creamitos para contar lo que fue, lo que es, y lo que será."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150346&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""d8214d35-c4b8-47d5-8f58-7fdc2fa684d3"",""multiverseId"":150346},""multiverseid"":150346},{""name"":""Mythifieur nomade"",""text"":""{W}, {T} : Mettez en jeu, attachée à une créature que vous contrôlez, une carte d'aura ciblée d'un cimetière. (Vous contrôlez cette aura.)"",""type"":""Créature : humain et nomade et clerc"",""flavor"":""Dans les steppes sauvages, l'histoire disparaît dans la poussière. Seuls les mythifieurs gardent le souvenir de ce qui a été, de ce qui est et de ce qui sera."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149963&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""ae5a52d8-5a5b-45f1-b7c2-40bd1befd680"",""multiverseId"":149963},""multiverseid"":149963},{""name"":""Cantastorie Nomade"",""text"":""{W}, {T}: Metti in gioco una carta Aura bersaglio da un cimitero assegnata a una creatura che controlli. (Controlli quell'Aura.)"",""type"":""Creatura — Chierico Nomade Umano"",""flavor"":""Nelle steppe selvagge, le storie svaniscono nella polvere. Solo i cantastorie rimangono per raccontare cos'era, cos'è e cosa sarà."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148998&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""d5055a86-6a1e-4d26-874f-bf2424c86bcd"",""multiverseId"":148998},""multiverseid"":148998},{""name"":""遊牧の民の神話作家"",""text"":""{W}, {T}：いずれかの墓地にあるオーラ・カード1枚を対象とし、それをあなたがコントロールするいずれかのクリーチャーにつけた状態で場に出す。 （あなたはそのオーラをコントロールする。）"",""type"":""クリーチャー — 人間・ノーマッド・クレリック"",""flavor"":""荒野の草原では、歴史は塵の中に消えてしまう。 神話作家のみがそれをありしまま、あるまま、あろうままに残すことができる。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148232&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""bbce707f-ade3-4643-87e5-846cb002d70c"",""multiverseId"":148232},""multiverseid"":148232},{""name"":""Nômade Criador de Mitos"",""text"":""{W}, {T}: Coloque em jogo o card alvo de Aura de um cemitério anexado a uma criatura que você controla. (Você controla aquela Aura.)"",""type"":""Criatura — Humano Nômade Clérigo"",""flavor"":""Nas estepes selvagens, a história desaparece na poeira. Apenas os criadores de mitos permanecem para contar o que foi, é e será."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149580&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""c28e0858-426b-48cf-9ca1-6efc9f571501"",""multiverseId"":149580},""multiverseid"":149580},{""name"":""Кочующий Мифотворец"",""text"":""{W}, {T}: Положите находящуюся на кладбище целевую карту Ауры в игру прикрепленной к существу под вашим контролем. (Вы контролируете ту Ауру.)"",""type"":""Существо — Человек Кочевник Священник"",""flavor"":""В пустынных степях История превращается в пыль. Лишь мифотворцы могут поведать о том, что было, что есть и что будет."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149197&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""ed634d55-4af4-4b4d-9aa7-c6c0a8a252eb"",""multiverseId"":149197},""multiverseid"":149197},{""name"":""牧民神话诗人"",""text"":""{W}，{T}：将目标灵气牌从坟墓场放置进场，并结附于由你操控的生物上。 （该灵气由你操控。）"",""type"":""生物～人类／游牧人／僧侣"",""flavor"":""在荒野草原上，历史化作微尘。 只剩神话诗人诉说一切的曾是，正是，与将是。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147849&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""7971277e-6157-40de-892c-f5bff95aaa32"",""multiverseId"":147849},""multiverseid"":147849}],""printings"":[""10E"",""JUD"",""PLST""],""originalText"":""{W}, {T}: Put target Aura card in a graveyard into play attached to a creature you control."",""originalType"":""Creature - Human Nomad Cleric"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""5d53a06b-68a0-5d1a-a1a5-a676f16c23dd""},{""name"":""Nomad Mythmaker"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Nomad Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Nomad"",""Cleric""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{W}, {T}: Put target Aura card from a graveyard onto the battlefield under your control attached to a creature you control."",""flavor"":""On the wild steppes, history vanishes in the dust. Only the mythmakers remain to say what was, and is, and will be."",""artist"":""Darrell Riche"",""number"":""30★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""5d53a06b-68a0-5d1a-a1a5-a676f16c23dd""],""rulings"":[{""date"":""2004-10-04"",""text"":""You don’t choose which creature the Aura will enter the battlefield attached to until the ability resolves. You must choose a creature the Aura can legally enchant. (For example, you can’t choose a creature with protection from black if the targeted Aura card is black.) If you don’t control any creatures that the Aura can enchant, it remains in the graveyard.""},{""date"":""2007-07-15"",""text"":""Nomad Mythmaker’s ability can target an Aura card in any graveyard, not just yours.""}],""printings"":[""10E"",""JUD"",""PLST""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""ac19e0e0-6d8e-55ad-a7a9-68fd6032eaab""},{""name"":""Pacifism"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature\nEnchanted creature can't attack or block."",""flavor"":""For the first time in his life, Grakk felt a little warm and fuzzy inside."",""artist"":""Robert Bliss"",""number"":""31"",""layout"":""normal"",""multiverseid"":""129667"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129667&type=card"",""variations"":[""b1e43df0-d501-5297-9cb4-c84d2a76945d""],""foreignNames"":[{""name"":""Pazifismus"",""text"":""Kreaturenverzauberung (Bestimme eine Kreatur als Ziel, sowie du diese Karte spielst. Diese Karte kommt an die Kreatur angelegt ins Spiel.)\nDie verzauberte Kreatur kann nicht angreifen oder blocken."",""type"":""Verzauberung — Aura"",""flavor"":""Ein Friedlicher ist einer, der sich totschießen lässt, um zu beweisen, dass der andere der Aggressor gewesen ist."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148620&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""b7c51c0b-e141-44c0-b51a-fbb983890d93"",""multiverseId"":148620},""multiverseid"":148620},{""name"":""Pacifismo"",""text"":""Encantar criatura. (Haz objetivo a una criatura al jugarlo. Esta carta entra en juego anexada a esa criatura.)\nLa criatura encantada no puede atacar ni bloquear."",""type"":""Encantamiento — Aura"",""flavor"":""Por primera vez en su vida, Grakk sintió ternura en su interior."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150347&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""e391ed0a-8e20-415c-9913-b03b839b7539"",""multiverseId"":150347},""multiverseid"":150347},{""name"":""Pacifisme"",""text"":""Enchanter : créature (Ciblez une créature au moment où vous jouez cette carte. Cette carte arrive en jeu attachée à cette créature.)\nLa créature enchantée ne peut ni attaquer ni bloquer."",""type"":""Enchantement : aura"",""flavor"":""Pour la première fois dans sa vie, Grakk sentit une douce chaleur se répandre en lui."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149964&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""dd8c9acf-8482-4e0c-b992-7e1ce7e60556"",""multiverseId"":149964},""multiverseid"":149964},{""name"":""Pacifismo"",""text"":""Incanta creatura (Bersaglia una creatura mentre giochi questa carta. Questa carta entra in gioco assegnata a quella creatura.)\nLa creatura incantata non può attaccare o bloccare."",""type"":""Incantesimo — Aura"",""flavor"":""Per la prima volta nella propria vita, Grakk sentì una sensazione di calore e confusione dentro di sé."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149003&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""b8acadb7-6f3c-47af-a5ed-052547b0755d"",""multiverseId"":149003},""multiverseid"":149003},{""name"":""平和な心"",""text"":""エンチャント（クリーチャー） （これをプレイする際に、クリーチャー１体を対象とする。 このカードはそのクリーチャーにつけられている状態で場に出る。）\nエンチャントされているクリーチャーは攻撃したりブロックしたりできない。"",""type"":""エンチャント — オーラ"",""flavor"":""グラックは生まれて初めて、ほんわかふわふわした気持ちになった。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148237&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""43bec5c8-efb3-4ac7-b1a0-8a70c8f5ca6c"",""multiverseId"":148237},""multiverseid"":148237},{""name"":""Pacifismo"",""text"":""Encantar criatura (Ao jogar este card, escolha uma criatura alvo. Este card entra em jogo anexado àquela criatura.)\nA criatura encantada não pode atacar nem bloquear."",""type"":""Encantamento — Aura"",""flavor"":""Pela primeira vez na vida, Grakk sentiu-se agitado e confuso por dentro."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149581&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""517fd089-f6aa-495c-a6a0-0a51a1fbb231"",""multiverseId"":149581},""multiverseid"":149581},{""name"":""Пацифизм"",""text"":""Зачаровать существо (При разыгрывании этой карты выберите целью существо. Эта карта входит в игру прикрепленной к тому существу.)\nЗачарованное существо не может атаковать или блокировать."",""type"":""Чары — Аура"",""flavor"":""Впервые в жизни Гракк почувствовал некое умиротворение."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149198&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""0eed5f31-88fb-44ad-9cec-7d40106ae9ca"",""multiverseId"":149198},""multiverseid"":149198},{""name"":""和平主义"",""text"":""生物结界（于使用时指定一个生物为目标。 此牌进场时结附在该生物上。）\n受此结界的生物不能进行攻击或阻挡。"",""type"":""结界～灵气"",""flavor"":""在葛瑞克的一生中，这是他首次感到内心的温情与暖意。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147854&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""e6662f7c-356b-47c5-b3aa-001358557151"",""multiverseId"":147854},""multiverseid"":147854}],""printings"":[""10E"",""6ED"",""7ED"",""8ED"",""9ED"",""A25"",""ANB"",""ATH"",""BBD"",""BRB"",""DDC"",""DMR"",""DTK"",""DVD"",""EMA"",""IKO"",""JMP"",""M10"",""M11"",""M12"",""M13"",""M14"",""M20"",""MIR"",""ONS"",""PLST"",""PS11"",""PSAL"",""TMP"",""TPR"",""USG"",""WC04""],""originalText"":""Enchant creature\nEnchanted creature can't attack or block."",""originalType"":""Enchantment - Aura"",""legalities"":[{""format"":""Alchemy"",""legality"":""Legal""},{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""26fc6830-a092-5e24-9b58-97f4c38f3e7c""},{""name"":""Pacifism"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature\nEnchanted creature can't attack or block."",""flavor"":""For the first time in his life, Grakk felt a little warm and fuzzy inside."",""artist"":""Robert Bliss"",""number"":""31★"",""layout"":""normal"",""variations"":[""26fc6830-a092-5e24-9b58-97f4c38f3e7c""],""printings"":[""10E"",""6ED"",""7ED"",""8ED"",""9ED"",""A25"",""ANB"",""ATH"",""BBD"",""BRB"",""DDC"",""DMR"",""DTK"",""DVD"",""EMA"",""IKO"",""JMP"",""M10"",""M11"",""M12"",""M13"",""M14"",""M20"",""MIR"",""ONS"",""PLST"",""PS11"",""PSAL"",""TMP"",""TPR"",""USG"",""WC04""],""legalities"":[{""format"":""Alchemy"",""legality"":""Legal""},{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""b1e43df0-d501-5297-9cb4-c84d2a76945d""},{""name"":""Paladin en-Vec"",""manaCost"":""{1}{W}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Knight"",""types"":[""Creature""],""subtypes"":[""Human"",""Knight""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""First strike, protection from black and from red (This creature deals combat damage before creatures without first strike. It can't be blocked, targeted, dealt damage, or enchanted by anything black or red.)"",""artist"":""Dave Kendall"",""number"":""32"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""129668"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129668&type=card"",""variations"":[""08a671f0-ea90-531e-bbeb-c00793a57672""],""foreignNames"":[{""name"":""Paladin en-Vec"",""text"":""Erstschlag, Schutz vor Schwarz, Schutz vor Rot (Diese Kreatur fügt Kampfschaden vor Kreaturen ohne Erstschlag zu. Sie kann von nichts Schwarzem oder Rotem geblockt oder als Ziel bestimmt werden, von ihm Schaden zugefügt bekommen oder von ihm verzaubert werden.)"",""type"":""Kreatur — Mensch, Ritter"",""flavor"":""„Ich halte mich nicht für einen Helden. Ich weiß nur, was die Vec mich gelehrt haben: Der Gerechtigkeit muss immer Genüge getan werden, die Korruption immer bekämpft.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148621&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""3f184a22-cb00-4f03-b089-36bcc6455774"",""multiverseId"":148621},""multiverseid"":148621},{""name"":""Paladín en-Vec"",""text"":""Daña primero, protección contra negro, protección contra rojo. (Esta criatura hace daño de combate antes que las criaturas sin la habilidad de dañar primero. Esta criatura no puede ser bloqueada, hecha objetivo, recibir daño de o estar encantada por nada negro o rojo.)"",""type"":""Criatura — Caballero humano"",""flavor"":""\""No me considero un héroe. Sólo sé lo que enseñan los Vec: la justicia debe defenderse y la corrupción debe ser enfrentada.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150348&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""e33fe02d-c250-4e76-b316-9895f3fee7b1"",""multiverseId"":150348},""multiverseid"":150348},{""name"":""Paladin en-Vec"",""text"":""Initiative, protection contre le noir, protection contre le rouge (Cette créature inflige des blessures de combat avant les créatures sans l'initiative. Elle ne peut pas être bloquée, ciblée, blessée ou enchantée par une source noire ou rouge.)"",""type"":""Créature : humain et chevalier"",""flavor"":""« Je ne me considère pas comme un héros. Je ne fais qu'appliquer les enseignements des Vecs : la justice doit toujours être servie et la corruption doit toujours être confrontée. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149965&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""1cf3deb7-53ba-478d-902a-2c2941f9c09a"",""multiverseId"":149965},""multiverseid"":149965},{""name"":""Paladino en-Vec"",""text"":""Attacco improvviso, protezione dal nero, protezione dal rosso (Questa creatura infligge danno da combattimento prima delle creature senza attacco improvviso. Non può essere bloccata, bersagliata, non le può essere inflitto danno, né può essere incantata da nulla di nero o rosso.)"",""type"":""Creatura — Cavaliere Umano"",""flavor"":""\""Non mi considero un eroe. So solo quello che insegnano i Vec: servi sempre la giustizia e combatti sempre la corruzione.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149004&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""ed22928e-9cf1-460c-ab81-439d0724e4b4"",""multiverseId"":149004},""multiverseid"":149004},{""name"":""ヴェクの聖騎士"",""text"":""先制攻撃、プロテクション（黒）、プロテクション（赤） （このクリーチャーは先制攻撃を持たないクリーチャーよりも先に戦闘ダメージを与える。 それは黒や赤の何かによって、ブロックされず、対象にならず、ダメージを与えられず、エンチャントされない。）"",""type"":""クリーチャー — 人間・騎士"",""flavor"":""自分が英雄とは思いません。 ただヴェクの教えを知るのみです。正義には仕えよ、堕落には対せよ、と。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148238&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""f053c797-5c0d-4915-a851-62433d8b9033"",""multiverseId"":148238},""multiverseid"":148238},{""name"":""Paladino en-Vec"",""text"":""Iniciativa, proteção contra o preto, proteção contra o vermelho (Esta criatura causa dano de combate antes de criaturas sem a habilidade de iniciativa. Esta criatura não pode ser bloqueada, ser alvo, sofrer dano, ou ser encantada por algo que seja preto ou vermelho.)"",""type"":""Criatura — Humano Cavaleiro"",""flavor"":""\""Não me considero um herói. Sei apenas o que os Vec ensinam: a justiça deve ser sempre feita e a corrupção deve ser sempre combatida.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149582&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""7c9281de-335a-4bb2-bb7f-201d08d003d7"",""multiverseId"":149582},""multiverseid"":149582},{""name"":""Паладин эн-Век"",""text"":""Первый удар, Защита от черного, Защита от красного (Это существо наносит боевые повреждения раньше существ без Первого удара. Ни один черный или красный объект не может блокировать это существо, делать его своей целью, наносить ему повреждения и зачаровывать его.)"",""type"":""Существо — Человек Рыцарь"",""flavor"":""\""Я не считаю себя героем. Я знаю только то, чему учат у Веков: всегда стоять на страже правосудия и всегда противостоять коррупции\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149199&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""97d8a8ae-22de-4f71-84c2-59973a8960c3"",""multiverseId"":149199},""multiverseid"":149199},{""name"":""维克族神圣武士"",""text"":""先攻，反黑保护，反红保护（此生物会比不具先攻异能的生物提前造成战斗伤害。 它不能被任何黑色或红色的东西所阻挡，指定为目标，造成伤害，或是被结附。）"",""type"":""生物～人类／骑士"",""flavor"":""「我不把自己当英雄。 我只奉行维克族的教诲：必得效命正义，铲除腐败。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147855&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""61d0ccfc-901b-4f23-a17a-37abbe227a18"",""multiverseId"":147855},""multiverseid"":147855}],""printings"":[""10E"",""9ED"",""EXO"",""PLST"",""TPR"",""WC98""],""originalText"":""First strike, protection from black, protection from red"",""originalType"":""Creature - Human Knight"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""214dbfe9-0f16-5b6b-be30-8771192a89ec""},{""name"":""Paladin en-Vec"",""manaCost"":""{1}{W}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Knight"",""types"":[""Creature""],""subtypes"":[""Human"",""Knight""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""First strike, protection from black and from red (This creature deals combat damage before creatures without first strike. It can't be blocked, targeted, dealt damage, or enchanted by anything black or red.)"",""flavor"":""\""I do not consider myself a hero. I know only what the Vec teach: justice must always be served and corruption must always be opposed.\"""",""artist"":""Dave Kendall"",""number"":""32★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""214dbfe9-0f16-5b6b-be30-8771192a89ec""],""printings"":[""10E"",""9ED"",""EXO"",""PLST"",""TPR"",""WC98""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""08a671f0-ea90-531e-bbeb-c00793a57672""},{""name"":""Pariah"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature\nAll damage that would be dealt to you is dealt to enchanted creature instead."",""artist"":""Jon J Muth"",""number"":""33"",""layout"":""normal"",""multiverseid"":""135248"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=135248&type=card"",""variations"":[""9f315280-5d52-5dea-a679-a2d97386907e""],""rulings"":[{""date"":""2004-10-04"",""text"":""All damage being dealt to you at one time gets redirected to one Pariah. If you take 3 damage at once, all 3 damage goes to one Pariah. If you take damage from multiple creatures in combat, all the combat damage goes to one Pariah.""},{""date"":""2004-10-04"",""text"":""You can attach this to an opponent’s creature, and all damage done to you is instead done to their creature.""}],""foreignNames"":[{""name"":""Paria"",""text"":""Kreaturenverzauberung (Bestimme eine Kreatur als Ziel, sowie du diese Karte spielst. Diese Karte kommt an die Kreatur angelegt ins Spiel.)\nAller Schaden, der dir zugefügt würde, wird stattdessen der verzauberten Kreatur zugefügt."",""type"":""Verzauberung — Aura"",""flavor"":""„Warum sollte ich Buße für meine Taten tun, wenn ich so viele Untertanen habe, die es freiwillig für mich machen?\"" —Fürst Konda"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148622&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""75603615-a8d7-4806-b312-0a45c6db76cb"",""multiverseId"":148622},""multiverseid"":148622},{""name"":""Paria"",""text"":""Encantar criatura. (Haz objetivo a una criatura al jugarlo. Esta carta entra en juego anexada a esa criatura.)\nTodo el daño que se te fuera a hacer, en vez de eso, se le hace a la criatura encantada."",""type"":""Encantamiento — Aura"",""flavor"":""\""¿Por qué habría de considerar una penitencia por mis acciones cuando tengo tantos súbditos dispuestos a hacerlo por mi?\"" —Señor Konda"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150349&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""963da484-1461-4e13-876b-f5615d1bcc61"",""multiverseId"":150349},""multiverseid"":150349},{""name"":""Paria"",""text"":""Enchanter : créature (Ciblez une créature au moment où vous jouez cette carte. Cette carte arrive en jeu attachée à cette créature.)\nToutes les blessures qui devraient vous être infligées sont infligées à la créature enchantée à la place."",""type"":""Enchantement : aura"",""flavor"":""« Pourquoi serais-je puni pour mes actes alors que tant de mes sujets sont prêts souffrir à ma place ? » —Seigneur Konda"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149966&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""d114882c-5683-4f12-aa0d-52c30e2f078e"",""multiverseId"":149966},""multiverseid"":149966},{""name"":""Pariah"",""text"":""Incanta creatura (Bersaglia una creatura mentre giochi questa carta. Questa carta entra in gioco assegnata a quella creatura.)\nTutto il danno che ti verrebbe inflitto viene invece inflitto alla creatura incantata."",""type"":""Incantesimo — Aura"",""flavor"":""\""Perché dovrei prendere in considerazione una punizione per le mie azioni quando ho così tanti sudditi desiderosi di farlo al posto mio?\"" —Lord Konda"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149005&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""e0a1d7fd-f81e-4104-b2a9-5a88b5c4cf1d"",""multiverseId"":149005},""multiverseid"":149005},{""name"":""最下層民"",""text"":""エンチャント（クリーチャー） （これをプレイする際に、クリーチャー１体を対象とする。 このカードはそのクリーチャーにつけられている状態で場に出る。）\nあなたに与えられるすべてのダメージは、代わりにエンチャントされているクリーチャーに与えられる。"",""type"":""エンチャント — オーラ"",""flavor"":""我が行いを我が悔いることなどあろうか。これほど多くの者が、我の代わりにならんと望んでおるのに。 ――君主今田"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148239&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""4da6a955-cc7d-44a9-a2fc-8e91bf5aed90"",""multiverseId"":148239},""multiverseid"":148239},{""name"":""Pária"",""text"":""Encantar criatura (Ao jogar este card, escolha uma criatura alvo. Este card entra em jogo anexado àquela criatura.)\nTodo o dano que seria causado a você, em vez disso, é causado à criatura encantada."",""type"":""Encantamento — Aura"",""flavor"":""\""Por que devo considerar uma punição para as minhas ações quando tenho tantos indivíduos desejosos de fazê-lo por mim?\"" — Senhor Konda"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149583&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""c72ceee5-db9c-4eeb-ae6e-525e8a7442df"",""multiverseId"":149583},""multiverseid"":149583},{""name"":""Пария"",""text"":""Зачаровать существо (При разыгрывании этой карты выберите целью существо. Эта карта входит в игру прикрепленной к тому существу.)\nВсе повреждения, которые будут нанесены вам, наносятся зачарованному существу вместо этого."",""type"":""Чары — Аура"",""flavor"":""\""Почему я должен нести наказание за мои действия, когда у меня так много подданных, с радостью сделающих это за меня?\"" — Лорд Конда"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149200&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""ee3bbf39-a0a8-4dbb-a07d-4b91fdee69e9"",""multiverseId"":149200},""multiverseid"":149200},{""name"":""贱民"",""text"":""生物结界（于使用时指定一个生物为目标。 此牌进场时结附在该生物上。）\n所有将对你造成的伤害改为对受此结界的生物造成之。"",""type"":""结界～灵气"",""flavor"":""「我虽造了业，但既然有那么多人愿替我承担，又何必一肩揽下？」 ～今田城主"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147856&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""c6bb1bd0-6c2e-4c91-9452-381b6fcdf295"",""multiverseId"":147856},""multiverseid"":147856}],""printings"":[""10E"",""7ED"",""CN2"",""OTP"",""PLST"",""PS11"",""USG""],""originalText"":""Enchant creature\nAll damage that would be dealt to you is dealt to enchanted creature instead."",""originalType"":""Enchantment - Aura"",""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""7dff9ddf-213e-593d-a007-a72cf6ff925e""},{""name"":""Pariah"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature\nAll damage that would be dealt to you is dealt to enchanted creature instead."",""flavor"":""\""Why would I consider penance for my actions when I have so many subjects willing to do it for me?\""\n—Lord Konda"",""artist"":""Jon J Muth"",""number"":""33★"",""layout"":""normal"",""variations"":[""7dff9ddf-213e-593d-a007-a72cf6ff925e""],""rulings"":[{""date"":""2004-10-04"",""text"":""All damage being dealt to you at one time gets redirected to one Pariah. If you take 3 damage at once, all 3 damage goes to one Pariah. If you take damage from multiple creatures in combat, all the combat damage goes to one Pariah.""},{""date"":""2004-10-04"",""text"":""You can attach this to an opponent’s creature, and all damage done to you is instead done to their creature.""}],""printings"":[""10E"",""7ED"",""CN2"",""OTP"",""PLST"",""PS11"",""USG""],""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""9f315280-5d52-5dea-a679-a2d97386907e""},{""name"":""Reviving Dose"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""You gain 3 life.\nDraw a card."",""flavor"":""Samite healers never mix their pungent elixir with sweetener or tea. The threat of a second dose is enough to get most warriors back on their feet."",""artist"":""D. Alexander Gregory"",""number"":""34"",""layout"":""normal"",""multiverseid"":""132089"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132089&type=card"",""foreignNames"":[{""name"":""Belebende Dosis"",""text"":""Du erhältst 3 Lebenspunkte dazu.Ziehe eine Karte."",""type"":""Spontanzauber"",""flavor"":""Samitische Heiler mischen ihre Tinkturen nie mit Süßstoff oder Tee. Die Androhung einer zweiten Dosis reicht meistens aus, damit sich die Krieger wieder gesund melden."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148658&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""1f7880b4-aea9-4cb2-9c64-ecdac11fbad9"",""multiverseId"":148658},""multiverseid"":148658},{""name"":""Dosis reanimadora"",""text"":""Gana 3 vidas.\nRoba una carta."",""type"":""Instantáneo"",""flavor"":""Los sanadores samitas nunca endulzan su hediento elixir o lo mezclan con té. La amenaza de una segunda dosis es suficiente para poner en pie a la mayoría de los guerreros."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150350&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""1dae4864-fdca-4669-87eb-32f740dc26f4"",""multiverseId"":150350},""multiverseid"":150350},{""name"":""Dose revivifiante"",""text"":""Vous gagnez 3 points de vie.\nPiochez une carte."",""type"":""Éphémère"",""flavor"":""Les guérisseurs sanctifs ne mélangent jamais leurs répugnants élixirs avec du sucre ou du thé. La simple menace d'une deuxième dose est suffisante pour revigorer la plupart des guerriers."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149967&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""c8a9b6b4-b2a0-4390-bc3f-b6be6265910e"",""multiverseId"":149967},""multiverseid"":149967},{""name"":""Porzione Rivitalizzante"",""text"":""Guadagni 3 punti vita.\nPesca una carta."",""type"":""Istantaneo"",""flavor"":""I guaritori bianchi non mescolano mai il loro acre elisir con bevande dolci o tè. La minaccia di una seconda dose è sufficiente a rimettere in piedi la maggior parte dei guerrieri."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149041&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""3d1cd433-05e0-4cdc-ad40-c7666261dc8c"",""multiverseId"":149041},""multiverseid"":149041},{""name"":""蘇生の妙薬"",""text"":""あなたは3点のライフを得る。\nカードを１枚引く。"",""type"":""インスタント"",""flavor"":""サマイトの癒し手たちは、ひどい味の霊薬を甘味料や茶と混ぜることはしない。 二服目を与えられる恐怖は、戦士を立ち直らせるには十分だ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148275&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""53cc3a82-ea71-495f-81e6-a3846b9a5bba"",""multiverseId"":148275},""multiverseid"":148275},{""name"":""Dose Restauradora"",""text"":""Você ganha 3 pontos de vida.\nCompre um card."",""type"":""Mágica Instantânea"",""flavor"":""Os curandeiros samitas nunca misturam seu elixir pungente com adoçante ou chá. A ameaça de uma segunda dose é suficiente para colocar a maioria dos guerreiros em pé novamente."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149584&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""56c0d85c-0499-4d9c-a82e-f05c388487c8"",""multiverseId"":149584},""multiverseid"":149584},{""name"":""Воскрешающая Доза"",""text"":""Вы получаете 3 жизни.\nВозьмите карту."",""type"":""Мгновенное заклинание"",""flavor"":""Самитские целители никогда не подслащивают и не разбавляют чаем свой едкий эликсир. Большинству воинов достаточно пригрозить второй дозой, чтобы они встали на ноги."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149201&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""705ab945-3663-4a26-a3bc-34b1fb852506"",""multiverseId"":149201},""multiverseid"":149201},{""name"":""提神剂"",""text"":""你获得3点生命。\n抓一张牌。"",""type"":""瞬间"",""flavor"":""撒姆尼人的医疗员从不在辛辣刺鼻的灵药里加入甘味或茶香。 只消施药两回，便能让大多数战士如常步行。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147892&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""0db3bdd3-40a7-4e4b-8b1b-8c6993421cd2"",""multiverseId"":147892},""multiverseid"":147892}],""printings"":[""10E"",""CN2"",""INV"",""PLST""],""originalText"":""You gain 3 life.\nDraw a card."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""39b50224-8305-56dd-9849-09deecb07d20""},{""name"":""Reya Dawnbringer"",""manaCost"":""{6}{W}{W}{W}"",""cmc"":9.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Legendary Creature — Angel"",""supertypes"":[""Legendary""],""types"":[""Creature""],""subtypes"":[""Angel""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying\nAt the beginning of your upkeep, you may return target creature card from your graveyard to the battlefield."",""flavor"":""\""You have not died until I consent.\"""",""artist"":""Matthew D. Wilson"",""number"":""35"",""power"":""4"",""toughness"":""6"",""layout"":""normal"",""multiverseid"":""106384"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=106384&type=card"",""variations"":[""341f9c8d-a838-58f9-933b-094c8d889a88""],""foreignNames"":[{""name"":""Reya Morgenbringer"",""text"":""Fliegend (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)\nDu kannst zu Beginn deines Versorgungssegments eine Kreaturenkarte deiner Wahl aus deinem Friedhof ins Spiel zurückbringen."",""type"":""Legendäre Kreatur — Engel"",""flavor"":""„Du bist nicht tot, solange ich dem nicht zugestimmt habe.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148659&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""014472cd-4859-49d5-ad0f-4007fce62637"",""multiverseId"":148659},""multiverseid"":148659},{""name"":""Reya Portaalba"",""text"":""Vuela. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)\nAl comienzo de tu mantenimiento, puedes regresar al juego una carta objetivo de criatura de tu cementerio."",""type"":""Criatura legendaria — Ángel"",""flavor"":""\""No mueres hasta que yo lo permito.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150351&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""3e58b497-3058-4520-ac93-53b023d7805b"",""multiverseId"":150351},""multiverseid"":150351},{""name"":""Reya Aubevenant"",""text"":""Vol (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée.)\nAu début de votre entretien, vous pouvez renvoyer en jeu une carte de créature ciblée de votre cimetière."",""type"":""Créature légendaire : ange"",""flavor"":""« Vous ne mourrez pas sans mon consentement. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149968&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""190eb9ab-0200-4580-873a-53996b9953d6"",""multiverseId"":149968},""multiverseid"":149968},{""name"":""Reya Dawnbringer"",""text"":""Volare (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere.)\nAll'inizio del tuo mantenimento, puoi rimettere in gioco una carta creatura bersaglio dal tuo cimitero."",""type"":""Creatura Leggendaria — Angelo"",""flavor"":""\""Tu non sei morto fino a che io non acconsento.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149042&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""28e4e333-1d90-42da-8b85-606c0a734c47"",""multiverseId"":149042},""multiverseid"":149042},{""name"":""黎明をもたらす者レイヤ"",""text"":""飛行 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされない。）\nあなたのアップキープの開始時に、あなたの墓地にあるクリーチャー・カード１枚を対象とする。あなたはそれを場に戻してもよい。"",""type"":""伝説のクリーチャー — 天使"",""flavor"":""私の許し無く死すことはありません。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148276&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""5fe1ba62-a21f-42df-aa0b-039a6c64ff72"",""multiverseId"":148276},""multiverseid"":148276},{""name"":""Reya, Portadora da Alvorada"",""text"":""Voar (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)\nNo início de sua manutenção, você pode devolver o card de criatura alvo de seu cemitério para o jogo."",""type"":""Criatura Lendária — Anjo"",""flavor"":""\""Você não morreu até que eu consinta.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149585&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""352a0da9-d85a-4198-8aac-06f2e4909218"",""multiverseId"":149585},""multiverseid"":149585},{""name"":""Рея, Приносящая Рассвет"",""text"":""Полет (Это существо может быть заблокировано только существом с Полетом или Захватом.)\nВ начале вашего шага поддержки вы можете вернуть целевую карту существа из вашего кладбища в игру."",""type"":""Легендарное Существо — Ангел"",""flavor"":""\""Ты не умрешь, пока я не дам на это согласия\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149202&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""78bb3a30-c5fc-404f-9ada-4ca82b421102"",""multiverseId"":149202},""multiverseid"":149202},{""name"":""黎明使者蕾亚"",""text"":""飞行（只有具飞行或延势异能的生物才能阻挡它。）\n在你的维持开始时，你可以将目标生物牌从你的坟墓场移回场上。"",""type"":""传奇生物～天使"",""flavor"":""「在我答应之前，你还没死。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147893&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""655b5dbc-19f1-4c1b-b1b9-2674754dc422"",""multiverseId"":147893},""multiverseid"":147893}],""printings"":[""10E"",""CNS"",""DDC"",""DVD"",""INV"",""P10E"",""PRM"",""PZ1"",""UMA""],""originalText"":""Flying\nAt the beginning of your upkeep, you may return target creature card from your graveyard to play."",""originalType"":""Legendary Creature - Angel"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""6028cab2-0478-584b-a880-8174a88eb188""},{""name"":""Reya Dawnbringer"",""manaCost"":""{6}{W}{W}{W}"",""cmc"":9.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Legendary Creature — Angel"",""supertypes"":[""Legendary""],""types"":[""Creature""],""subtypes"":[""Angel""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying\nAt the beginning of your upkeep, you may return target creature card from your graveyard to the battlefield."",""flavor"":""\""You have not died until I consent.\"""",""artist"":""Matthew D. Wilson"",""number"":""35★"",""power"":""4"",""toughness"":""6"",""layout"":""normal"",""variations"":[""6028cab2-0478-584b-a880-8174a88eb188""],""printings"":[""10E"",""CNS"",""DDC"",""DVD"",""INV"",""P10E"",""PRM"",""PZ1"",""UMA""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""341f9c8d-a838-58f9-933b-094c8d889a88""},{""name"":""Righteousness"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Target blocking creature gets +7/+7 until end of turn."",""flavor"":""Sometimes the greatest strength is the strength of conviction."",""artist"":""Wayne England"",""number"":""36"",""layout"":""normal"",""multiverseid"":""130552"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130552&type=card"",""rulings"":[{""date"":""2009-10-01"",""text"":""A “blocking creature” is one that has been declared as a blocker this combat, or one that was put onto the battlefield blocking this combat. Unless that creature leaves combat, it continues to be a blocking creature through the end of combat step, even if the creature or creatures it was blocking are no longer on the battlefield or have otherwise left combat.""}],""foreignNames"":[{""name"":""Rechtschaffenheit"",""text"":""Eine blockende Kreatur deiner Wahl erhält +7/+7 bis zum Ende des Zuges."",""type"":""Spontanzauber"",""flavor"":""Manchmal ist die Stärke der Überzeugung stärker als alles andere."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148661&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""21dbee24-1d42-4021-98dc-fa565260e701"",""multiverseId"":148661},""multiverseid"":148661},{""name"":""Rectitud"",""text"":""La criatura bloqueadora objetivo obtiene +7/+7 hasta el final del turno."",""type"":""Instantáneo"",""flavor"":""A veces la mayor fuerza es la fuerza de la convicción."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150352&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""cb1d1ab8-d56f-4b7a-9058-4c72a80ae5f0"",""multiverseId"":150352},""multiverseid"":150352},{""name"":""Droiture"",""text"":""La créature bloqueuse ciblée gagne +7/+7 jusqu'à la fin du tour."",""type"":""Éphémère"",""flavor"":""« Dieu donne à la franchise, à la fidélité, à la droiture un accent qui ne peut être ni contrefait, ni méconnu. »Joseph, comte de Maistre"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149969&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""9c146238-71f9-4d74-959e-fd8f800d7a96"",""multiverseId"":149969},""multiverseid"":149969},{""name"":""Rettitudine"",""text"":""Una creatura bloccante bersaglio prende +7/+7 fino alla fine del turno."",""type"":""Istantaneo"",""flavor"":""A volte la forza più grande è la forza del convincimento."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149044&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""a95f9d50-e3d0-4855-be7e-0cd4780608b6"",""multiverseId"":149044},""multiverseid"":149044},{""name"":""高潔のあかし"",""text"":""ブロックしているクリーチャー１体を対象とする。それはターン終了時まで＋７/＋７の修整を受ける。"",""type"":""インスタント"",""flavor"":""信じることの力が最も強い力となることもある。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148278&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""17789c37-a841-43e1-b863-f620052fbcf5"",""multiverseId"":148278},""multiverseid"":148278},{""name"":""Integridade"",""text"":""A criatura alvo bloqueadora recebe +7/+7 até o final do turno."",""type"":""Mágica Instantânea"",""flavor"":""Às vezes a maior força é a força da convicção."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149586&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""f2a36094-7c81-4dfa-808c-54968b322a0f"",""multiverseId"":149586},""multiverseid"":149586},{""name"":""Праведность"",""text"":""Целевое блокирующее существо получает +7/+7 до конца хода."",""type"":""Мгновенное заклинание"",""flavor"":""Иногда главная сила это сила убеждения."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149203&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""358ecbd5-be1a-46c6-9d6c-c9117ee69991"",""multiverseId"":149203},""multiverseid"":149203},{""name"":""正气"",""text"":""目标进行阻挡的生物得+7/+7直到回合结束。"",""type"":""瞬间"",""flavor"":""有时候，最强大的力量便是信念之力。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147895&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""5b7911a0-a7e2-4a80-93d3-444069601a1a"",""multiverseId"":147895},""multiverseid"":147895}],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""5ED"",""9ED"",""CED"",""CEI"",""DDL"",""ELD"",""FBB"",""J22"",""LEA"",""LEB"",""M10"",""SUM""],""originalText"":""Target blocking creature gets +7/+7 until end of turn."",""originalType"":""Instant"",""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""9a14261a-b567-5429-a5ca-a913e15d8bf7""},{""name"":""Rule of Law"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment"",""types"":[""Enchantment""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Each player can't cast more than one spell each turn."",""flavor"":""Appointed by the kha himself, members of the tribunal ensure all disputes are settled with the utmost fairness."",""artist"":""Scott M. Fischer"",""number"":""37"",""layout"":""normal"",""multiverseid"":""136291"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=136291&type=card"",""rulings"":[{""date"":""2019-07-12"",""text"":""If you cast a spell that was countered, you can’t cast another spell during the same turn.""},{""date"":""2019-07-12"",""text"":""Rule of Law looks at the entire turn to see if a player has cast a spell, even if Rule of Law wasn’t on the battlefield when that spell was cast. Notably, you can’t cast Rule of Law and then cast another spell during the same turn.""}],""foreignNames"":[{""name"":""Rechtsstaatlichkeit"",""text"":""Jeder Spieler kann nicht mehr als einen Zauberspruch pro Zug spielen."",""type"":""Verzauberung"",""flavor"":""Die vom Kha persönlich bestimmten Mitglieder des Tribunals sorgen dafür, dass alle Konflikte mit allergrößter Fairness ausgetragen werden."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148670&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""5a6a3738-efdf-4ec8-92b7-4498948ea470"",""multiverseId"":148670},""multiverseid"":148670},{""name"":""Imperio de la ley"",""text"":""Ningún jugador puede jugar más de un hechizo cada turno."",""type"":""Encantamiento"",""flavor"":""Los miembros del tribunal, nombrados por el mismo kha, aseguran que todas las disputas se resuelvan con suma imparcialidad."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150353&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""0397db67-4ae6-4b15-928b-ad587bae5d0d"",""multiverseId"":150353},""multiverseid"":150353},{""name"":""Autorité de la loi"",""text"":""Aucun joueur ne peut jouer plus d'un sort par tour."",""type"":""Enchantement"",""flavor"":""Nommés par le kha, les membres du tribunal tranchent les litiges avec équité — et sans appel."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149970&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""3d3e6697-3468-4575-ae9e-3da0d41c865e"",""multiverseId"":149970},""multiverseid"":149970},{""name"":""Regola della Legge"",""text"":""Ogni giocatore non può giocare più di una magia per turno."",""type"":""Incantesimo"",""flavor"":""Nominati dal kha in persona, i membri del tribunale si assicurano che tutti i processi si concludano nella più assoluta imparzialità."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149053&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""7c7b0c32-ebbd-4b41-81f9-c7e5db1c5c25"",""multiverseId"":149053},""multiverseid"":149053},{""name"":""法の定め"",""text"":""各プレイヤーは、毎ターン１つしか呪文をプレイできない。"",""type"":""エンチャント"",""flavor"":""王自身に指名された裁きの面々は、すべての論議が究極の公平さの下に行われていることを保障している。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148287&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""292e49af-2db6-43c2-8b62-8168bc169be6"",""multiverseId"":148287},""multiverseid"":148287},{""name"":""Regra de Lei"",""text"":""Nenhum jogador pode jogar mais do que uma mágica a cada turno."",""type"":""Encantamento"",""flavor"":""Indicados pelo próprio cã, os membros do tribunal garantem que todas as disputas sejam resolvidas com a máxima justiça."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149587&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""2cdfd2fc-52d2-400d-b840-b2fd240c80e1"",""multiverseId"":149587},""multiverseid"":149587},{""name"":""Власть Закона"",""text"":""Каждый игрок не может разыгрывать более одного заклинания за ход."",""type"":""Чары"",""flavor"":""Назначенные самим Ка, члены трибунала следят за тем, чтобы все тяжбы разрешались предельно справедливо."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149204&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""7fc6cb19-3b5d-418a-8d38-f5b0abd84d62"",""multiverseId"":149204},""multiverseid"":149204},{""name"":""依法治理"",""text"":""每位牌手每回合不能使用一个以上的咒语。"",""type"":""结界"",""flavor"":""裁决所的成员由狮王亲自指派，以确保所有争端都得到最公正的解决。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147904&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""5decfd5a-e817-41e2-9a73-aa6d0533f004"",""multiverseId"":147904},""multiverseid"":147904}],""printings"":[""10E"",""M20"",""MRD""],""originalText"":""Each player can't play more than one spell each turn."",""originalType"":""Enchantment"",""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""eee00aa2-c730-5ff1-84a9-51d95a9fe180""},{""name"":""Samite Healer"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Cleric""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{T}: Prevent the next 1 damage that would be dealt to any target this turn."",""flavor"":""Healers ultimately acquire the divine gifts of spiritual and physical wholeness. The most devout are also granted the ability to pass physical wholeness on to others."",""artist"":""Anson Maddocks"",""number"":""38"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""132101"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132101&type=card"",""foreignNames"":[{""name"":""Feldscher"",""text"":""{T}: Verhindere den nächsten 1 Schadenspunkt, der in diesem Zug einer Kreatur oder einem Spieler deiner Wahl zugefügt würde."",""type"":""Kreatur — Mensch, Kleriker"",""flavor"":""Heiler erlangen schließlich die göttliche Gabe geistiger und körperlicher Einheit. Den Ergebensten von ihnen wird zusätzlich die Fähigkeit zuteil, körperliche Gesundheit an andere weiterzugeben."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148673&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""7783ef0d-cfc7-4734-aa38-0ebe67f090ab"",""multiverseId"":148673},""multiverseid"":148673},{""name"":""Sanador samita"",""text"":""{T}: Prevén el siguiente 1 punto de daño que se le fuera a hacer a la criatura o jugador objetivo este turno."",""type"":""Criatura — Clérigo humano"",""flavor"":""Los curanderos finalmente adquieren los dones divinos de la integridad espiritual y física. A los más devotos también les conceden la habilidad de pasar la integridad física a otros."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150354&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""bf939419-0153-4b54-bb1f-f72d7677e12d"",""multiverseId"":150354},""multiverseid"":150354},{""name"":""Guérisseur sanctif"",""text"":""{T} : Prévenez, ce tour-ci, la prochaine 1 blessure qui devrait être infligée à une cible, créature ou joueur."",""type"":""Créature : humain et clerc"",""flavor"":""Les guérisseurs finissent par acquérir les dons divins de plénitude physique et spirituelle. Les plus fervents acquièrent la capacité de passer cette plénitude à autrui."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149971&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""ad2e30dc-a78e-4551-a03d-3385052f7b4b"",""multiverseId"":149971},""multiverseid"":149971},{""name"":""Guaritore Bianco"",""text"":""{T}: Previeni il prossimo punto danno che verrebbe inflitto a una creatura o a un giocatore bersaglio in questo turno."",""type"":""Creatura — Chierico Umano"",""flavor"":""I guaritori acquisiscono essenzialmente i doni divini dell'integrità fisica e spirituale. Solo i più devoti tra loro ricevono anche l'abilità di passare ad altri l'integrità fisica."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149056&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""e0bec1b2-54dc-4fe3-aa98-d9b64f05bd62"",""multiverseId"":149056},""multiverseid"":149056},{""name"":""サマイトの癒し手"",""text"":""{T}：クリーチャー１体かプレイヤー１人を対象とする。このターン、次にそれに与えられるダメージを１点軽減する。"",""type"":""クリーチャー — 人間・クレリック"",""flavor"":""癒し手は、最終的には、霊的かつ肉体的な完全さという天の賜物を獲得する。 最も献身的な者はまた、その肉体的な完全さを他者に分け与える力を授かる。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148290&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""9f5901b2-9a32-4864-b654-d1b721fc3308"",""multiverseId"":148290},""multiverseid"":148290},{""name"":""Curandeiro Samita"",""text"":""{T}: Previna o próximo 1 ponto de dano que seria causado à criatura ou ao jogador alvo neste turno."",""type"":""Criatura — Humano Clérigo"",""flavor"":""Os curandeiros acabam por adquirir os dons divinos da integridade física e espiritual. Aos mais devotos, também é concedida a habilidade de passar a integridade física para outros."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149588&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""10774c10-bc9f-4d47-a2e4-b6436e3ad28e"",""multiverseId"":149588},""multiverseid"":149588},{""name"":""Самитский Лекарь"",""text"":""{T}: Предотвратите следующее 1 повреждение, которое будет нанесено целевому существу или игроку в этом ходу."",""type"":""Существо — Человек Священник"",""flavor"":""Лекари получают божественный дар духовной и физической цельности. А самым лучшим из них даруется способность передавать эту цельность окружающим."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149205&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""71d07db7-b94e-49d0-9b6f-c367e8d15af9"",""multiverseId"":149205},""multiverseid"":149205},{""name"":""撒姆尼人的医疗员"",""text"":""{T}：于本回合中，防止接下来将对目标生物或牌手造成的1点伤害。"",""type"":""生物～人类／僧侣"",""flavor"":""医疗员们终究会得到神赐的健全身心。 其中最虔诚者还将获赐使他人身体健全的能力。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147907&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""663ccec9-a579-4df1-ad48-3f6119ba8f8d"",""multiverseId"":147907},""multiverseid"":147907}],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""5ED"",""6ED"",""7ED"",""8ED"",""9ED"",""ATH"",""CED"",""CEI"",""FBB"",""LEA"",""LEB"",""SUM""],""originalText"":""{T}: Prevent the next 1 damage that would be dealt to target creature or player this turn."",""originalType"":""Creature - Human Cleric"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""a2be2e9b-5211-53e6-ac6a-abd65dbcac1d""},{""name"":""Serra Angel"",""manaCost"":""{3}{W}{W}"",""cmc"":5.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Angel"",""types"":[""Creature""],""subtypes"":[""Angel""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying, vigilance"",""flavor"":""Her sword sings more beautifully than any choir."",""artist"":""Greg Staples"",""number"":""39"",""power"":""4"",""toughness"":""4"",""layout"":""normal"",""multiverseid"":""129726"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129726&type=card"",""variations"":[""b2f56602-e85a-588f-a4be-40b6e56f44f7""],""foreignNames"":[{""name"":""Serra-Engel"",""text"":""Fliegend, Wachsamkeit (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden und wird beim Angreifen nicht getappt.)"",""type"":""Kreatur — Engel"",""flavor"":""Ihr Schwert singt schöner als jeder Chor."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148682&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""2f394e40-9a5c-4d98-9615-1795b80f0bd5"",""multiverseId"":148682},""multiverseid"":148682},{""name"":""Ángel de Serra"",""text"":""Vuela, vigilancia. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance, y esta criatura no se gira al atacar.)"",""type"":""Criatura — Ángel"",""flavor"":""Su espada canta más bellamente que cualquier coro."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150355&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""a68a2e43-2fba-482c-aa79-0922e060d98b"",""multiverseId"":150355},""multiverseid"":150355},{""name"":""Ange de Serra"",""text"":""Vol, vigilance (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée, et attaquer avec cette créature ne la fait pas s'engager.)"",""type"":""Créature : ange"",""flavor"":""Son épée chante plus merveilleusement que le plus beau des chœurs."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149972&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""72909de0-5ca6-4f56-b3af-c591c3337a33"",""multiverseId"":149972},""multiverseid"":149972},{""name"":""Angelo di Serra"",""text"":""Volare, cautela (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere, e attacca senza TAPpare.)"",""type"":""Creatura — Angelo"",""flavor"":""La sua spada canta meglio di qualsiasi coro."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149065&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""5c311a32-24d9-4168-a3fe-7481e3f6cf6d"",""multiverseId"":149065},""multiverseid"":149065},{""name"":""セラの天使"",""text"":""飛行、警戒 （このクリーチャーは、飛行も到達も持たないクリーチャーによってブロックされず、攻撃してもタップしない。）"",""type"":""クリーチャー — 天使"",""flavor"":""彼女の剣はどんな聖歌隊よりも美しく歌う。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148299&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""50d571be-cabe-41fd-8232-e9c299b2f6e6"",""multiverseId"":148299},""multiverseid"":148299},{""name"":""Anjo Serra"",""text"":""Voar, vigilância (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance, e esta criatura não é virada para atacar.)"",""type"":""Criatura — Anjo"",""flavor"":""O som de sua espada é mais belo que o ecoar das vozes de um coral."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149589&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""b0eaf7cd-2ed2-4746-a533-e49875cfa6cd"",""multiverseId"":149589},""multiverseid"":149589},{""name"":""Ангел Серры"",""text"":""Полет, Бдительность (Это существо может быть заблокировано только существом с Полетом или Захватом, и при нападении это существо не поворачивается.)"",""type"":""Существо — Ангел"",""flavor"":""Песнь ее меча прекраснее любого хора."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149206&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""54734e48-3b82-4fde-abd2-a6230fd2e81f"",""multiverseId"":149206},""multiverseid"":149206},{""name"":""撒拉天使"",""text"":""飞行，警戒（只有具飞行或延势异能的生物才能阻挡它，且此生物攻击时不需横置。）"",""type"":""生物～天使"",""flavor"":""她的剑吟唱着比任何歌曲更优美的旋律。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147916&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""58e445ec-1958-4556-8bb2-e86b82160ea2"",""multiverseId"":147916},""multiverseid"":147916}],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""7ED"",""8ED"",""9ED"",""ANB"",""ATH"",""CED"",""CEI"",""CMA"",""CMD"",""DDC"",""DMR"",""DOM"",""DVD"",""EMA"",""FBB"",""GN3"",""GNT"",""IMA"",""JMP"",""LEA"",""LEB"",""M10"",""M11"",""M12"",""M13"",""M14"",""M15"",""ME4"",""O90P"",""OANA"",""ORI"",""P30A"",""P30H"",""PDOM"",""PMDA"",""PRM"",""PS11"",""PTC"",""PW24"",""PWOS"",""SLD"",""SUM"",""V15"",""W16"",""W17""],""originalText"":""Flying, vigilance"",""originalType"":""Creature - Angel"",""legalities"":[{""format"":""Alchemy"",""legality"":""Legal""},{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""be665b02-1cf2-50c6-8861-85da921bc853""},{""name"":""Serra Angel"",""manaCost"":""{3}{W}{W}"",""cmc"":5.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Angel"",""types"":[""Creature""],""subtypes"":[""Angel""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying, vigilance"",""flavor"":""Her sword sings more beautifully than any choir."",""artist"":""Greg Staples"",""number"":""39★"",""power"":""4"",""toughness"":""4"",""layout"":""normal"",""variations"":[""be665b02-1cf2-50c6-8861-85da921bc853""],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""7ED"",""8ED"",""9ED"",""ANB"",""ATH"",""CED"",""CEI"",""CMA"",""CMD"",""DDC"",""DMR"",""DOM"",""DVD"",""EMA"",""FBB"",""GN3"",""GNT"",""IMA"",""JMP"",""LEA"",""LEB"",""M10"",""M11"",""M12"",""M13"",""M14"",""M15"",""ME4"",""O90P"",""OANA"",""ORI"",""P30A"",""P30H"",""PDOM"",""PMDA"",""PRM"",""PS11"",""PTC"",""PW24"",""PWOS"",""SLD"",""SUM"",""V15"",""W16"",""W17""],""legalities"":[{""format"":""Alchemy"",""legality"":""Legal""},{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""b2f56602-e85a-588f-a4be-40b6e56f44f7""},{""name"":""Serra's Embrace"",""manaCost"":""{2}{W}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature\nEnchanted creature gets +2/+2 and has flying and vigilance."",""artist"":""Zoltan Boros & Gabor Szikszai"",""number"":""40"",""layout"":""normal"",""multiverseid"":""135214"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=135214&type=card"",""variations"":[""dc219ada-64b4-5846-9840-e49986109aa9""],""foreignNames"":[{""name"":""Serras Umarmung"",""text"":""Kreaturenverzauberung (Bestimme eine Kreatur als Ziel, sowie du diese Karte spielst. Diese Karte kommt an die Kreatur angelegt ins Spiel.)\nDie verzauberte Kreatur erhält +2/+2, Flugfähigkeit und Wachsamkeit. (Sie kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden und wird beim Angreifen nicht getappt.)"",""type"":""Verzauberung — Aura"",""flavor"":""Eine Berührung von Serras Engeln erweckt Hoffnungen wieder und verstärkt gute Taten."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148683&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""e9dc3581-d356-4dee-a4fb-e8a9d1e0168b"",""multiverseId"":148683},""multiverseid"":148683},{""name"":""Abrazo de Serra"",""text"":""Encantar criatura. (Haz objetivo a una criatura al jugarlo. Esta carta entra en juego anexada a esa criatura.)\nLa criatura encantada obtiene +2/+2 y tiene la habilidad de volar y vigilancia. (No puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance, y no se gira al atacar.)"",""type"":""Encantamiento — Aura"",""flavor"":""El toque de los ángeles de Serra lleva esperanza y da fuerza a las causas nobles."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150356&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""800b8a85-0a58-4b33-96c5-17d1ac962213"",""multiverseId"":150356},""multiverseid"":150356},{""name"":""Étreinte de Serra"",""text"":""Enchanter : créature (Ciblez une créature au moment où vous jouez cette carte. Cette carte arrive en jeu attachée à cette créature.)\nLa créature enchantée gagne +2/+2 et a le vol et la vigilance. (Elle ne peut être bloquée que par des créatures avec le vol ou la portée, et attaquer avec elle ne la fait pas s'engager.)"",""type"":""Enchantement : aura"",""flavor"":""Le contact des anges de Serra ravive l'espoir et donne puissance aux causes les plus nobles."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149973&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""1bedaf94-502e-4f88-ba9e-7b2094b776ea"",""multiverseId"":149973},""multiverseid"":149973},{""name"":""Abbraccio di Serra"",""text"":""Incanta creatura (Bersaglia una creatura mentre giochi questa carta. Questa carta entra in gioco assegnata a quella creatura.)\nLa creatura incantata prende +2/+2 e ha volare e cautela. (Non può essere bloccata tranne che da creature con volare o raggiungere, e attacca senza TAPpare.)"",""type"":""Incantesimo — Aura"",""flavor"":""Il tocco degli angeli di Serra eleva le speranze e potenzia le nobili cause."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149066&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""6149358e-7be7-43fa-bc40-2b3c5c195c98"",""multiverseId"":149066},""multiverseid"":149066},{""name"":""セラの抱擁"",""text"":""エンチャント（クリーチャー） （これをプレイする際に、クリーチャー１体を対象とする。 このカードはそのクリーチャーにつけられている状態で場に出る。）\nエンチャントされているクリーチャーは、＋２/＋２の修整を受けるとともに飛行と警戒を持つ。 （それは飛行も到達も持たないクリーチャーによってブロックされず、攻撃してもタップしない。）"",""type"":""エンチャント — オーラ"",""flavor"":""セラの天使の手には立ち上る希望があり、高潔な意志に力を与える。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148300&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""da7718aa-b4c0-47fe-8f71-c84d472b9d4a"",""multiverseId"":148300},""multiverseid"":148300},{""name"":""Abraço de Serra"",""text"":""Encantar criatura (Ao jogar este card, escolha uma criatura alvo. Este card entra em jogo anexado àquela criatura.)\nA criatura encantada recebe +2/+2 e tem a habilidade de voar e vigilância. (Ela só pode ser bloqueada por criaturas com a habilidade de voar ou alcance e não é virada para atacar.)"",""type"":""Encantamento — Aura"",""flavor"":""O toque dos anjos de Serra eleva as esperanças e fortifica causas nobres."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149590&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""5bf0c505-503b-4446-88e3-312f987c9729"",""multiverseId"":149590},""multiverseid"":149590},{""name"":""Объятия Серры"",""text"":""Зачаровать существо (При разыгрывании этой карты выберите целью существо. Эта карта входит в игру прикрепленной к тому существу.)\nЗачарованное существо получает +2/+2 и имеет Полет и Бдительность. (Оно может быть заблокировано только существом с Полетом или Захватом, и при нападении это существо не поворачивается.)"",""type"":""Чары — Аура"",""flavor"":""Прикосновение ангелов Серры не дает надежде угаснуть и побуждает к благородным деяниям."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149207&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""dc8367a6-d65b-41ad-b5cf-d704d6ee46c8"",""multiverseId"":149207},""multiverseid"":149207},{""name"":""撒拉之拥"",""text"":""生物结界（于使用时指定一个生物为目标。 此牌进场时结附在该生物上。）\n受此结界的生物得+2/+2并具有飞行与警戒异能。 （只有具飞行或延势异能的生物才能阻挡它，且它攻击时不需横置。）"",""type"":""结界～灵气"",""flavor"":""撒拉麾下天使之轻触能让希望攀扬，并授与高尚使命。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147917&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""8b63567b-3729-4f30-ad06-551629d16fd0"",""multiverseId"":147917},""multiverseid"":147917}],""printings"":[""10E"",""7ED"",""DDC"",""DVD"",""PLST"",""PS11"",""USG""],""originalText"":""Enchant creature\nEnchanted creature gets +2/+2 and has flying and vigilance."",""originalType"":""Enchantment - Aura"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""fe9187fa-cd14-5618-9a62-bcf18b523d84""},{""name"":""Serra's Embrace"",""manaCost"":""{2}{W}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature\nEnchanted creature gets +2/+2 and has flying and vigilance."",""flavor"":""The touch of Serra's angels bears hopes aloft and empowers noble causes."",""artist"":""Zoltan Boros & Gabor Szikszai"",""number"":""40★"",""layout"":""normal"",""variations"":[""fe9187fa-cd14-5618-9a62-bcf18b523d84""],""printings"":[""10E"",""7ED"",""DDC"",""DVD"",""PLST"",""PS11"",""USG""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""dc219ada-64b4-5846-9840-e49986109aa9""},{""name"":""Skyhunter Patrol"",""manaCost"":""{2}{W}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Cat Knight"",""types"":[""Creature""],""subtypes"":[""Cat"",""Knight""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying, first strike (This creature can't be blocked except by creatures with flying or reach, and it deals combat damage before creatures without first strike.)"",""flavor"":""\""We leonin have come to rule the plains by taking to the skies.\""\n—Raksha Golden Cub"",""artist"":""Matt Cavotta"",""number"":""41"",""power"":""2"",""toughness"":""3"",""layout"":""normal"",""multiverseid"":""129735"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129735&type=card"",""variations"":[""db62961f-51de-569a-bfd8-b3f82abf1b18""],""foreignNames"":[{""name"":""Himmeljäger-Patrouille"",""text"":""Fliegend, Erstschlag (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden und fügt Kampfschaden vor Kreaturen ohne Erstschlag zu.)"",""type"":""Kreatur — Katze, Ritter"",""flavor"":""„Wir Leoniden haben die Herrschaft über die Ebenen errungen, indem wir die Lüfte erobert haben.\"" —Raksha Goldjunges"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148696&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""25f27e8b-241f-447c-94a8-953439c205b6"",""multiverseId"":148696},""multiverseid"":148696},{""name"":""Patrulla cazacielo"",""text"":""Vuela, daña primero. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance, y hace daño de combate antes que las criaturas sin la habilidad de dañar primero.)"",""type"":""Criatura — Caballero felino"",""flavor"":""\""Los leoninos hemos logrado dominar las llanuras llegando a los cielos.\"" —Raksha Cachorro Dorado"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150357&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""79420988-8d98-448b-88c8-54df7f1be6a4"",""multiverseId"":150357},""multiverseid"":150357},{""name"":""Patrouille chasseciel"",""text"":""Vol, initiative (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée, et elle inflige des blessures de combat avant les créatures sans l'initiative.)"",""type"":""Créature : chat et chevalier"",""flavor"":""« Les léonins sont parvenus à régner sur les plaines en dominant les cieux. » —Raksha Lionceaudor"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149974&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""19116718-7a8f-4798-b493-7e56f6967464"",""multiverseId"":149974},""multiverseid"":149974},{""name"":""Pattuglia di Solcacielo"",""text"":""Volare, attacco improvviso (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere, e infligge danno da combattimento prima delle creature senza attacco improvviso.)"",""type"":""Creatura — Cavaliere Felino"",""flavor"":""\""Noi leonid siamo riusciti a governare le pianure ritirandoci in cielo.\"" —Raksha, Cucciolo d'Oro"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149079&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""0c490ee4-13e2-4d3c-b21c-8053903cce5d"",""multiverseId"":149079},""multiverseid"":149079},{""name"":""空狩人の巡回兵"",""text"":""飛行、先制攻撃 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされず、先制攻撃を持たないクリーチャーよりも先に戦闘ダメージを与える。）"",""type"":""クリーチャー — 猫・騎士"",""flavor"":""我々レオニンは、空を飛ぶことで平原を制圧してきた。 ――黄金の若人ラクシャ"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148313&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""f319aef1-f729-4faf-b1c4-f4332c0f7338"",""multiverseId"":148313},""multiverseid"":148313},{""name"":""Patrulha Caçadora Celeste"",""text"":""Voar, iniciativa (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance, e causa dano de combate antes de criaturas sem a habilidade de iniciativa.)"",""type"":""Criatura — Felino Cavaleiro"",""flavor"":""\""Nós leoninos passamos a governar as planícies alçando vôo nos céus.\"" — Raksha, Filhote Dourado"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149591&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""98bb1e07-cde4-4e21-a212-431022b06e5b"",""multiverseId"":149591},""multiverseid"":149591},{""name"":""Патруль Небесных Охотников"",""text"":""Полет, Первый удар (Это существо может быть заблокировано только существом с Полетом или Захватом и наносит боевые повреждения раньше существ без Первого удара.)"",""type"":""Существо — Кошка Рыцарь"",""flavor"":""\""Мы, леонинцы, царим на равнине, паря высоко в небе\"". — Ракша, Золотая львица"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149208&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""e2994fc8-f9c8-48c7-9799-d6b5e6b68794"",""multiverseId"":149208},""multiverseid"":149208},{""name"":""巡防空猎者"",""text"":""飞行，先攻（只有具飞行或延势异能的生物才能阻挡它，且它会比不具先攻异能的生物提前造成战斗伤害。）"",""type"":""生物～猫／骑士"",""flavor"":""「我们狮族飞入长空，藉以统治无尽平原。」 ～金狮王洛夏"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147930&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""0681f671-c3ec-4c3a-8d11-2692fa5469d3"",""multiverseId"":147930},""multiverseid"":147930}],""printings"":[""10E"",""DDG"",""J22"",""MRD"",""PSAL""],""originalText"":""Flying, first strike"",""originalType"":""Creature - Cat Knight"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""db398a18-0d81-50ba-bc86-a7de52a1be7f""},{""name"":""Skyhunter Patrol"",""manaCost"":""{2}{W}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Cat Knight"",""types"":[""Creature""],""subtypes"":[""Cat"",""Knight""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying, first strike (This creature can't be blocked except by creatures with flying or reach, and it deals combat damage before creatures without first strike.)"",""flavor"":""\""We leonin have come to rule the plains by taking to the skies.\""\n—Raksha Golden Cub"",""artist"":""Matt Cavotta"",""number"":""41★"",""power"":""2"",""toughness"":""3"",""layout"":""normal"",""variations"":[""db398a18-0d81-50ba-bc86-a7de52a1be7f""],""printings"":[""10E"",""DDG"",""J22"",""MRD"",""PSAL""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""db62961f-51de-569a-bfd8-b3f82abf1b18""},{""name"":""Skyhunter Prowler"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Cat Knight"",""types"":[""Creature""],""subtypes"":[""Cat"",""Knight""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying, vigilance (This creature can't be blocked except by creatures with flying or reach, and attacking doesn't cause this creature to tap.)"",""flavor"":""As tireless as her mount, a skyhunter's vigil is measured in days."",""artist"":""Vance Kovacs"",""number"":""42"",""power"":""1"",""toughness"":""3"",""layout"":""normal"",""multiverseid"":""132102"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132102&type=card"",""variations"":[""5e670818-f7b1-52a6-bc6f-c9eccb8ce3ef""],""foreignNames"":[{""name"":""Himmeljäger-Streife"",""text"":""Fliegend, Wachsamkeit (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden und wird beim Angreifen nicht getappt.)"",""type"":""Kreatur — Katze, Ritter"",""flavor"":""Da sie so wenig ermüden wie ihre Reittiere, dauert die Schicht eines Himmeljägers oft mehrere Tage."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148697&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""77f55f1e-2fe2-4d42-b2e9-6db8c2c66539"",""multiverseId"":148697},""multiverseid"":148697},{""name"":""Rondadora cazacielos"",""text"":""Vuela, vigilancia. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance, y esta criatura no se gira al atacar.)"",""type"":""Criatura — Caballero felino"",""flavor"":""Incansable como su montura, la vigilia de una cazacielos se mide en días."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150358&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""74e13fb5-7914-4eb8-b3a0-c435ae2ce873"",""multiverseId"":150358},""multiverseid"":150358},{""name"":""Rôdeuse chasseciel"",""text"":""Vol, vigilance (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée, et attaquer avec cette créature ne la fait pas s'engager.)"",""type"":""Créature : chat et chevalier"",""flavor"":""Le chasseciel est aussi infatigable que sa monture. Sa garde peut durer pendant des jours."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149975&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""ddc32d2c-f4eb-41e7-a810-337c0f086fb6"",""multiverseId"":149975},""multiverseid"":149975},{""name"":""Predatore Solcacielo"",""text"":""Volare, cautela (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere, e attacca senza TAPpare.)"",""type"":""Creatura — Cavaliere Felino"",""flavor"":""Essendo instancabile come la sua cavalcatura, la veglia di un solcacielo viene misurata in giorni."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149080&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""11389775-0fef-41e0-8486-3a82746fd5c0"",""multiverseId"":149080},""multiverseid"":149080},{""name"":""うろつく空狩人"",""text"":""飛行、警戒 （このクリーチャーは、飛行も到達も持たないクリーチャーによってブロックされず、攻撃してもタップしない。）"",""type"":""クリーチャー — 猫・騎士"",""flavor"":""空狩人の不寝番は、疲れを知らぬ翼竜同様に長い間評価されている。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148314&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""55e40370-9750-46b8-942e-798954a7c43b"",""multiverseId"":148314},""multiverseid"":148314},{""name"":""Caçadora Celeste Espreitadora"",""text"":""Voar, vigilância (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance, e esta criatura não é virada para atacar.)"",""type"":""Criatura — Felino Cavaleiro"",""flavor"":""Tão incansável quanto sua cavalgada, a vigília de um caçador celeste é medida em dias."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149592&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""29413c29-67de-4fb1-b523-dd56725f2afc"",""multiverseId"":149592},""multiverseid"":149592},{""name"":""Небесный Охотник"",""text"":""Полет, Бдительность (Это существо может быть заблокировано только существом с Полетом или Захватом, и при нападении это существо не поворачивается.)"",""type"":""Существо — Кошка Рыцарь"",""flavor"":""Неутомимая наездница, небесная охотница дежурит целые дни напролет."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149209&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""a2a9a2b4-9f20-49fe-bb6e-30c7c4d6a35f"",""multiverseId"":149209},""multiverseid"":149209},{""name"":""游掠空猎者"",""text"":""飞行，警戒（只有具飞行或延势异能的生物才能阻挡它，且此生物攻击时不需横置。）"",""type"":""生物～猫／骑士"",""flavor"":""空猎者与其座骑都不知疲累为何，一趟警戒总以日为单位。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147931&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""e67ea825-47a7-4b74-a01b-fe9b2de98c47"",""multiverseId"":147931},""multiverseid"":147931}],""printings"":[""10E"",""5DN"",""9ED"",""J22""],""originalText"":""Flying, vigilance"",""originalType"":""Creature - Cat Knight"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""66993685-84dd-5143-9454-8a4028d10a6a""},{""name"":""Skyhunter Prowler"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Cat Knight"",""types"":[""Creature""],""subtypes"":[""Cat"",""Knight""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying, vigilance (This creature can't be blocked except by creatures with flying or reach, and attacking doesn't cause this creature to tap.)"",""flavor"":""As tireless as her mount, a skyhunter's vigil is measured in days."",""artist"":""Vance Kovacs"",""number"":""42★"",""power"":""1"",""toughness"":""3"",""layout"":""normal"",""variations"":[""66993685-84dd-5143-9454-8a4028d10a6a""],""printings"":[""10E"",""5DN"",""9ED"",""J22""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""5e670818-f7b1-52a6-bc6f-c9eccb8ce3ef""},{""name"":""Skyhunter Skirmisher"",""manaCost"":""{1}{W}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Cat Knight"",""types"":[""Creature""],""subtypes"":[""Cat"",""Knight""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying, double strike"",""artist"":""Greg Staples"",""number"":""43"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""129513"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129513&type=card"",""variations"":[""0bd58de8-5807-55d0-a5ad-ebe42f2f1ebc""],""foreignNames"":[{""name"":""Himmeljäger-Plänkler"",""text"":""Fliegend, Doppelschlag (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden und fügt sowohl Erstschlags- als auch regulären Kampfschaden zu.)"",""type"":""Kreatur — Katze, Ritter"",""flavor"":""„Nimm die tödliche Anmut und Stärke der Leoniden und kombiniere sie mit einem Reitpteron, der einen Feind in Stücke zerreißen kann und eine Rüstung aus schartigem Stahl hat. Dann hast du einen Himmeljäger — und den Grund dafür, warum Taj-Nar nie erobert wurde.\"" —Raksha Goldjunges"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148698&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""8f437a4a-7144-4e84-a16b-59743494098c"",""multiverseId"":148698},""multiverseid"":148698},{""name"":""Escaramuzadora cazacielos"",""text"":""Vuela, daña dos veces. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance, y hace tanto daño de combate de dañar primero como normal.)"",""type"":""Criatura — Caballero felino"",""flavor"":""\""Toma la fuerza y la gracia mortal de un leonino, combínalo con una montura de pterón capaz de partir a un enemigo en dos y ármalo con acero mellado. Eso es una cazacielos, la razón por la cual Taj-Nar no ha caído.\"" —Raksha Cachorro Dorado"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150359&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""762cf25f-b70b-49db-83d2-532ddab7b731"",""multiverseId"":150359},""multiverseid"":150359},{""name"":""Assaillante chasseciel"",""text"":""Vol, double initiative (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée, et elle inflige des blessures de combat à deux reprises : avant les créatures sans l'initiative et ensuite en même temps qu'elles.)"",""type"":""Créature : chat et chevalier"",""flavor"":""« Combinez la grâce et la force mortelles du léonin à un ptéron capable de déchiqueter un adversaire, ajoutez-y des armes d'acier dentelé. Et vous obtenez un chasseciel — la raison pour laquelle Taj-Nar n'a jamais été vaincu. » —Raksha Lionceaudor"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149976&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""c3aafd41-b56f-4c42-8885-15c6af25c685"",""multiverseId"":149976},""multiverseid"":149976},{""name"":""Esploratrice Solcacielo"",""text"":""Volare, doppio attacco (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere, e infligge sia danno da combattimento da attacco improvviso che danno da combattimento regolare.)"",""type"":""Creatura — Cavaliere Felino"",""flavor"":""\""Prendi la grazia letale e la forza del leonid, unita a una cavalcatura pteron in grado di squartare in due un nemico e armata di acciaio dentellato. Questo è un solcacielo, e anche il motivo per cui Taj-Nar non ha mai fallito.\"" —Raksha, Cucciolo d'Oro"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149081&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""f69cbeed-566d-4834-a599-c3abfb406a9e"",""multiverseId"":149081},""multiverseid"":149081},{""name"":""空狩人の散兵"",""text"":""飛行、二段攻撃 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされず、先制攻撃と通常の両方で戦闘ダメージを与える。）"",""type"":""クリーチャー — 猫・騎士"",""flavor"":""レオニンから恐ろしいまでの優雅さと力を受け、それを敵を引き裂く翼竜の鋼の爪を持つ腕と組み合わせよ。 それこそが空狩人が――そしてタージ＝ナールが落ちぬ理由だ。 ――黄金の若人ラクシャ"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148315&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""3093fc09-9818-4725-b1a9-051ef3eac645"",""multiverseId"":148315},""multiverseid"":148315},{""name"":""Caçadora Celeste Escaramuçadora"",""text"":""Voar, golpe duplo (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance, e causa tanto o dano da iniciativa quanto o dano de combate normal.)"",""type"":""Criatura — Felino Cavaleiro"",""flavor"":""\""Pegue a graça e a força mortais de um leonino, misture com a cavalgada de um ptero, capaz de partir um inimigo ao meio, e arme-o com aço entalhado. Isso é um caçador celeste e a razão de Taj-Nar nunca ter sido subjugada.\"" — Raksha, Filhote Dourado"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149593&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""f44444a2-deb7-4259-9412-06acb2033a2b"",""multiverseId"":149593},""multiverseid"":149593},{""name"":""Застрельщик из Небесных Охотников"",""text"":""Полет, Двойной удар (Это существо может быть заблокировано только существом с Полетом или Захватом и наносит боевые повреждения как на этапе Первого удара, так и на этапе обычных повреждений.)"",""type"":""Существо — Кошка Рыцарь"",""flavor"":""\""Представьте себе смертоносную ловкость и силу леонинца, восседающего верхом на птероне, покрытом зазубренной броней и способном разорвать врага на куски. Таков небесный охотник, и поэтому Тадж-Нар не знает поражений\"". — Ракша, Золотая львица"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149210&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""a431f805-927c-4e79-878d-df48f84183a4"",""multiverseId"":149210},""multiverseid"":149210},{""name"":""侦卫空猎者"",""text"":""飞行，连击（只有具飞行或延势异能的生物才能阻挡它，且它能造成先攻与普通战斗伤害。）"",""type"":""生物～猫／骑士"",""flavor"":""「用狮族强猛的优雅与力量，加上能将敌手撕成两半的翼兽，配以带锯齿的武器。 这就是空猎者～他们正是塔吉纳从未失陷的功臣。」 ～金狮王洛夏"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147932&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""306d27c2-20cf-40dd-998a-1275bc052bf0"",""multiverseId"":147932},""multiverseid"":147932}],""printings"":[""10E"",""5DN"",""C14"",""MM2"",""PLST"",""PS11"",""PSAL""],""originalText"":""Flying, double strike"",""originalType"":""Creature - Cat Knight"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""261c0ce9-13d9-58e2-922b-d4068ff4d743""},{""name"":""Skyhunter Skirmisher"",""manaCost"":""{1}{W}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Cat Knight"",""types"":[""Creature""],""subtypes"":[""Cat"",""Knight""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying, double strike"",""flavor"":""\""Take the deadly grace and strength of the leonin, combined with a pteron mount capable of rending a foe in two and armed with notched steel. That is a skyhunter—and why Taj-Nar has never fallen.\""\n—Raksha Golden Cub"",""artist"":""Greg Staples"",""number"":""43★"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""variations"":[""261c0ce9-13d9-58e2-922b-d4068ff4d743""],""printings"":[""10E"",""5DN"",""C14"",""MM2"",""PLST"",""PS11"",""PSAL""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""0bd58de8-5807-55d0-a5ad-ebe42f2f1ebc""},{""name"":""Soul Warden"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Cleric""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Whenever another creature enters the battlefield, you gain 1 life."",""flavor"":""Count carefully the souls and see that none are lost.\n—Vec teaching"",""artist"":""Randy Gallegos"",""number"":""44"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""129740"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129740&type=card"",""rulings"":[{""date"":""2004-10-04"",""text"":""Does not trigger on a card on the battlefield being changed into a creature.""},{""date"":""2004-10-04"",""text"":""The ability will not trigger on itself entering the battlefield, but it will trigger on any other creature that is put onto the battlefield at the same time.""},{""date"":""2005-08-01"",""text"":""The life gain is mandatory.""},{""date"":""2005-08-01"",""text"":""Two Soul Wardens entering the battlefield at the same time will each cause the other’s ability to trigger.""}],""foreignNames"":[{""name"":""Seelenwächter"",""text"":""Immer wenn eine andere Kreatur ins Spiel kommt, erhältst du 1 Lebenspunkt dazu."",""type"":""Kreatur — Mensch, Kleriker"",""flavor"":""Zähle die Seelen sorgfältig, damit keine verlorengeht. —Lehre der Vec"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148704&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""9b6d999f-7719-45e5-bd61-6a981efde3a1"",""multiverseId"":148704},""multiverseid"":148704},{""name"":""Protectora de almas"",""text"":""Siempre que otra criatura entre en juego, ganas 1 vida."",""type"":""Criatura — Clérigo humano"",""flavor"":""Cuenta cuidadosamente las almas y comprueba que no se haya perdido ninguna. —Enseñanza vec"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150360&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""2be53cf0-32d3-4b5a-b257-0b92d2510302"",""multiverseId"":150360},""multiverseid"":150360},{""name"":""Garde des âmes"",""text"":""À chaque fois qu'une autre créature arrive en jeu, vous gagnez 1 point de vie."",""type"":""Créature : humain et clerc"",""flavor"":""Comptez les âmes et assurez-vous qu'elles soient toutes présentes. —Enseignement Vec"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149977&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""f21ea28c-3073-4e6c-9436-2c743e87cf61"",""multiverseId"":149977},""multiverseid"":149977},{""name"":""Guardiano dell'Anima"",""text"":""Ogniqualvolta un'altra creatura entra in gioco, guadagni 1 punto vita."",""type"":""Creatura — Chierico Umano"",""flavor"":""Conta attentamente le anime e assicurati che nessuna si sia persa. —Insegnamento Vec"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149087&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""ad35f073-e764-4d34-adf8-82d9d5579cff"",""multiverseId"":149087},""multiverseid"":149087},{""name"":""魂の管理人"",""text"":""他のいずれかのクリーチャーが場に出るたび、あなたは１点のライフを得る。"",""type"":""クリーチャー — 人間・クレリック"",""flavor"":""魂はよく注意して数えよ。失われた魂がないかどうか確認せよ。 ――ヴェクの教え"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148321&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""6d7d6c50-9bba-472f-886c-6196f22ba686"",""multiverseId"":148321},""multiverseid"":148321},{""name"":""Encarregado das Almas"",""text"":""Toda vez que outra criatura entra em jogo, você ganha 1 ponto de vida."",""type"":""Criatura — Humano Clérigo"",""flavor"":""Conta as almas com cuidado e certifica-te que nenhuma se perdeu. — Ensinamento Vec"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149594&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""cb425ff8-ae2c-489b-9bc6-b07cbabfb8a7"",""multiverseId"":149594},""multiverseid"":149594},{""name"":""Страж Души"",""text"":""Каждый раз, когда другое существо входит в игру, вы получаете 1 жизнь."",""type"":""Существо — Человек Священник"",""flavor"":""Хорошенько пересчитай души, чтобы ни одна не пропала. — Уроки Веков"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149211&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""5b054694-f2f2-43e7-bc94-e0a8c1a1ef53"",""multiverseId"":149211},""multiverseid"":149211},{""name"":""护灵师"",""text"":""每当另一个生物进场时，你获得1点生命。"",""type"":""生物～人类／僧侣"",""flavor"":""仔细照护灵魂，勿使之迷途。 ～维克族教谕"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147938&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""f98fa7bf-db50-4807-b84b-cb47899c615b"",""multiverseId"":147938},""multiverseid"":147938}],""printings"":[""10E"",""9ED"",""BRB"",""EXO"",""HA1"",""HOP"",""M10"",""MD1"",""MM3"",""PLST"",""PSAL"",""WC98""],""originalText"":""Whenever another creature comes into play, you gain 1 life."",""originalType"":""Creature - Human Cleric"",""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""bcc70712-566e-5a6f-8d73-f1ab87c57454""},{""name"":""Spirit Link"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature (Target a creature as you cast this. This card enters the battlefield attached to that creature.)\nWhenever enchanted creature deals damage, you gain that much life."",""artist"":""Kev Walker"",""number"":""45"",""layout"":""normal"",""multiverseid"":""129744"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129744&type=card"",""variations"":[""0da4a2a0-4d13-5c8a-bdff-12b67070798e""],""rulings"":[{""date"":""2022-12-08"",""text"":""The triggered ability triggers when the enchanted creature deals any damage, not only combat damage.""},{""date"":""2022-12-08"",""text"":""Unlike the lifelink ability, the ability of Spirit Link is a triggered ability that goes on the stack and may be responded to. Notably, if the enchanted creature deals damage at the same time you’re dealt enough damage to reduce your life total to 0 or less, you’ll lose the game before you can gain any life.""}],""foreignNames"":[{""name"":""Geisteskontakt"",""text"":""Kreaturenverzauberung (Bestimme eine Kreatur als Ziel, sowie du diese Karte spielst. Diese Karte kommt an die Kreatur angelegt ins Spiel.)\nImmer wenn die verzauberte Kreatur Schaden zufügt, erhältst du ebenso viele Lebenspunkte dazu."",""type"":""Verzauberung — Aura"",""flavor"":""„Wir sind alle untrennbar miteinander verbunden, zu einem Teppich gewebte Seelen.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148712&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""5215c9e8-5dff-4737-b2e1-7a0e566bf39c"",""multiverseId"":148712},""multiverseid"":148712},{""name"":""Vínculo espiritual"",""text"":""Encantar criatura. (Haz objetivo a una criatura al jugarlo. Esta carta entra en juego anexada a esa criatura.)\nSiempre que la criatura encantada haga daño, ganas esa cantidad de vida."",""type"":""Encantamiento — Aura"",""flavor"":""\""Estamos inexorablemente unidos, almas entretejidas en un tapiz.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150361&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""cf9b52fa-9a2b-4529-aafd-5e5e1c85a5e1"",""multiverseId"":150361},""multiverseid"":150361},{""name"":""Liaison psychique"",""text"":""Enchanter : créature (Ciblez une créature au moment où vous jouez cette carte. Cette carte arrive en jeu attachée à cette créature.)\nÀ chaque fois que la créature enchantée inflige des blessures, vous gagnez autant de points de vie."",""type"":""Enchantement : aura"",""flavor"":""« Nous sommes étroitement liés, âmes entrelacées comme une tapisserie. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149978&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""351a19df-ef15-4d29-89c8-b0e48b0daf0b"",""multiverseId"":149978},""multiverseid"":149978},{""name"":""Legame Spirituale"",""text"":""Incanta creatura (Bersaglia una creatura mentre giochi questa carta. Questa carta entra in gioco assegnata a quella creatura.)\nOgniqualvolta la creatura incantata infligge danno, guadagni altrettanti punti vita."",""type"":""Incantesimo — Aura"",""flavor"":""\""Siamo tutti legati inestricabilmente, le anime intrecciate come un arazzo.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149095&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""de4bffe1-fb7d-4143-bc51-c2efa799a1cd"",""multiverseId"":149095},""multiverseid"":149095},{""name"":""魂の絆"",""text"":""エンチャント（クリーチャー） （これをプレイする際に、クリーチャー１体を対象とする。 このカードはそのクリーチャーにつけられている状態で場に出る。）\nエンチャントされているクリーチャーがダメージを与えるたび、あなたはそれに等しい点数のライフを得る。"",""type"":""エンチャント — オーラ"",""flavor"":""我らの繋がりは分かつことができず、魂は絡み合ってタペストリーを成す。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148329&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""4c9766ea-ff69-4e5a-bb85-ff29585a082e"",""multiverseId"":148329},""multiverseid"":148329},{""name"":""Vínculo Espiritual"",""text"":""Encantar criatura (Ao jogar este card, escolha uma criatura alvo. Este card entra em jogo anexado àquela criatura.)\nToda vez que a criatura encantada causa dano, você ganha aquela quantidade em pontos de vida."",""type"":""Encantamento — Aura"",""flavor"":""\""Estamos todos inextrincavelmente ligados, nossas almas entrelaçadas como tapeçaria.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149595&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""04572fa4-3a51-4ade-998e-842fb0cf0bee"",""multiverseId"":149595},""multiverseid"":149595},{""name"":""Духовная Связь"",""text"":""Зачаровать существо (При разыгрывании этой карты выберите целью существо. Эта карта входит в игру прикрепленной к тому существу.)\nКаждый раз, когда зачарованное существо наносит повреждения, вы получаете столько же жизни."",""type"":""Чары — Аура"",""flavor"":""\""Все мы неразрывно связаны, наши души сплетены в одно целое\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149212&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""f3cfbd22-7219-4492-9575-cc04f603eb65"",""multiverseId"":149212},""multiverseid"":149212},{""name"":""心灵羁绊"",""text"":""生物结界（于使用时指定一个生物为目标。 此牌进场时结附在该生物上。）\n每当受此结界的生物造成伤害时，你获得等量的生命。"",""type"":""结界～灵气"",""flavor"":""「我们的命运注定彼此通连，灵魂交缠有如挂毯。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147946&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""e0e02747-50fa-4915-af36-8ac3f7a46b7c"",""multiverseId"":147946},""multiverseid"":147946}],""printings"":[""10E"",""4BB"",""4ED"",""5ED"",""6ED"",""7ED"",""8ED"",""9ED"",""DMR"",""LEG"",""PSAL"",""REN"",""WC98""],""originalText"":""Enchant creature\nWhenever enchanted creature deals damage, you gain that much life."",""originalType"":""Enchantment - Aura"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""a563f008-ed9f-5fc4-b6aa-1e68d63cb814""},{""name"":""Spirit Link"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment — Aura"",""types"":[""Enchantment""],""subtypes"":[""Aura""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Enchant creature (Target a creature as you cast this. This card enters the battlefield attached to that creature.)\nWhenever enchanted creature deals damage, you gain that much life."",""flavor"":""\""We are all inextricably linked, souls woven in tapestry.\"""",""artist"":""Kev Walker"",""number"":""45★"",""layout"":""normal"",""variations"":[""a563f008-ed9f-5fc4-b6aa-1e68d63cb814""],""rulings"":[{""date"":""2022-12-08"",""text"":""The triggered ability triggers when the enchanted creature deals any damage, not only combat damage.""},{""date"":""2022-12-08"",""text"":""Unlike the lifelink ability, the ability of Spirit Link is a triggered ability that goes on the stack and may be responded to. Notably, if the enchanted creature deals damage at the same time you’re dealt enough damage to reduce your life total to 0 or less, you’ll lose the game before you can gain any life.""}],""printings"":[""10E"",""4BB"",""4ED"",""5ED"",""6ED"",""7ED"",""8ED"",""9ED"",""DMR"",""LEG"",""PSAL"",""REN"",""WC98""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""0da4a2a0-4d13-5c8a-bdff-12b67070798e""},{""name"":""Spirit Weaver"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Wizard"",""types"":[""Creature""],""subtypes"":[""Human"",""Wizard""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{2}: Target green or blue creature gets +0/+1 until end of turn."",""flavor"":""\""Let my hope be your shield.\"""",""artist"":""Matthew D. Wilson"",""number"":""46"",""power"":""2"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""130999"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130999&type=card"",""foreignNames"":[{""name"":""Weber des Geistes"",""text"":""{2}: Eine grüne oder blaue Kreatur deiner Wahl erhält +0/+1 bis zum Ende des Zuges."",""type"":""Kreatur — Mensch, Zauberer"",""flavor"":""„Lass meine Hoffnung dein Schild sein.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148713&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""8a97ad81-5079-426e-9439-7ffc2eda38ea"",""multiverseId"":148713},""multiverseid"":148713},{""name"":""Tejedora de espíritu"",""text"":""{2}: La criatura objetivo verde o azul obtiene +0/+1 hasta el final del turno."",""type"":""Criatura — Hechicero humano"",""flavor"":""\""Que mi esperanza sea tu escudo.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150362&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""a4c08cc8-1dcb-4577-9def-74363b2c2903"",""multiverseId"":150362},""multiverseid"":150362},{""name"":""Tisseuse d'esprit"",""text"":""{2} : La créature verte ou bleue ciblée gagne +0/+1 jusqu'à la fin du tour."",""type"":""Créature : humain et sorcier"",""flavor"":""« Que mon espoir soit votre bouclier. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149979&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""dcf876cc-7852-47c5-b215-6c85be36e411"",""multiverseId"":149979},""multiverseid"":149979},{""name"":""Tessitore di Spiriti"",""text"":""{2}: Una creatura verde o blu bersaglio prende +0/+1 fino alla fine del turno."",""type"":""Creatura — Mago Umano"",""flavor"":""\""Lasciate che la mia speranza sia il vostro scudo.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149096&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""9b5cff30-2389-47af-9546-06408dfbd82f"",""multiverseId"":149096},""multiverseid"":149096},{""name"":""魂の織り手"",""text"":""{2}：緑か青のクリーチャー１体を対象とする。それはターン終了時まで＋０/＋１の修整を受ける。"",""type"":""クリーチャー — 人間・ウィザード"",""flavor"":""私の希望をそなたの盾としよう。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148330&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""4b06b008-4008-466f-bc3e-5f8938d7ed80"",""multiverseId"":148330},""multiverseid"":148330},{""name"":""Tecelão de Espíritos"",""text"":""{2}: A criatura alvo verde ou azul recebe +0/+1 até o final do turno."",""type"":""Criatura — Humano Mago"",""flavor"":""\""Que a minha esperança seja teu escudo.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149596&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""6d3d4020-45ca-4d85-b8b3-c1e57be7c42f"",""multiverseId"":149596},""multiverseid"":149596},{""name"":""Плетельщица Духа"",""text"":""{2}: Целевое зеленое или синее существо получает +0/+1 до конца хода."",""type"":""Существо — Человек Чародей"",""flavor"":""\""Пусть моя надежда станет тебе щитом\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149213&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""cf9fc58b-08b0-450f-8775-3bd633b75d91"",""multiverseId"":149213},""multiverseid"":149213},{""name"":""织灵巧匠"",""text"":""{2}：目标绿色或蓝色生物得+0/+1直到回合结束。"",""type"":""生物～人类／法术师"",""flavor"":""「让我的希望成为你的护盾。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147947&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""d036bc40-7828-4d2b-8df1-791d2d53ba6d"",""multiverseId"":147947},""multiverseid"":147947}],""printings"":[""10E"",""INV""],""originalText"":""{2}: Target green or blue creature gets +0/+1 until end of turn."",""originalType"":""Creature - Human Wizard"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""ae68e914-6896-5ee8-af5f-8ea88f5e0200""},{""name"":""Starlight Invoker"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Cleric Mutant"",""types"":[""Creature""],""subtypes"":[""Human"",""Cleric"",""Mutant""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{7}{W}: You gain 5 life."",""flavor"":""\""The constellations form a tapestry of light that traces my people's broken history. Day and night, I feel their glittering presence calling me to weave the pattern whole.\"""",""artist"":""Glen Angus"",""number"":""47"",""power"":""1"",""toughness"":""3"",""layout"":""normal"",""multiverseid"":""130385"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130385&type=card"",""foreignNames"":[{""name"":""Sternenlicht-Beschwörer"",""text"":""{7}{W}: Du erhältst 5 Lebenspunkte dazu."",""type"":""Kreatur — Mensch, Kleriker, Mutant"",""flavor"":""„Die Sternbilder erzeugen einen Teppich aus Licht, auf dem man Teile der Geschichte meines Volkes lesen kann. Tag und Nacht spüre ich, wie ihr Leuchten nach mir ruft, das Muster fertig zu weben.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148718&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""831f90ed-b1c7-419c-ac68-5db73f54205c"",""multiverseId"":148718},""multiverseid"":148718},{""name"":""Invocadora de luz estelar"",""text"":""{7}{W}: Ganas 5 vidas."",""type"":""Criatura — Clérigo mutante humano"",""flavor"":""\""Las constelaciones forman un tapiz de luz que rastrea la historia fracturada de mi gente. Noche y día, siento su brillante presencia pidiéndome que complete el tejido.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150363&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""7264849d-e96c-4e4b-bb3f-8dab222f0a6c"",""multiverseId"":150363},""multiverseid"":150363},{""name"":""Invocatrice de lumière d'étoile"",""text"":""{7}{W} : Vous gagnez 5 point de vie."",""type"":""Créature : humain et clerc et mutant"",""flavor"":""« Les constellations forment une tapisserie de lumière qui retrace l'histoire brisée de mon peuple. Nuit et jour, je sens cette présence scintillante m'appeler pour finir de la tisser. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149980&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""530918ef-3eaf-4b2b-ae30-60680e6b3418"",""multiverseId"":149980},""multiverseid"":149980},{""name"":""Evocatore di Luce Stellare"",""text"":""{7}{W}: Guadagni 5 punti vita."",""type"":""Creatura — Mutante Chierico Umano"",""flavor"":""\""Le costellazioni formano un tappeto di luce che ripercorre la storia frammentata del mio popolo. Giorno e notte, sento la loro presenza scintillante che mi chiama per tessere l'intera trama.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149101&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""a5a3f047-f8dd-4445-8d55-03c4f1c4d018"",""multiverseId"":149101},""multiverseid"":149101},{""name"":""星明かりの発動者"",""text"":""{7}{W}：あなたは5点のライフを得る。"",""type"":""クリーチャー — 人間・クレリック・ミュータント"",""flavor"":""星座は我が一族の壊れた歴史を追う光のタペストリーを成します。 日毎夜毎、私にはその模様を編み上げるよう求める輝く存在を感じるのです。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148335&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""09d1c9c3-1b34-44df-ab31-a7704373aea5"",""multiverseId"":148335},""multiverseid"":148335},{""name"":""Invocadora da Luz das Estrelas"",""text"":""{7}{W}: Você ganha 5 ponto de vida."",""type"":""Criatura — Humano Clérigo Mutante"",""flavor"":""\""As constelações formam um tapete de luzes que traçam a história despedaçada do meu povo. Dia e noite eu sinto a sua presença cintilante me chamando para completar a trama.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149597&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""4d25513f-aa99-47df-9d72-5b5175d28a54"",""multiverseId"":149597},""multiverseid"":149597},{""name"":""Призывательница Звездного Света"",""text"":""{7}{W}: Вы получаете 5 жизней."",""type"":""Существо — Человек Священник Мутант"",""flavor"":""\""Созвездия образуют ткань из света, в которой прослеживаются отрывки из истории моего народа. И днем и ночью я чувствую их притягивающее мерцание, напоминающее, чтобы я сплела эту ткань воедино\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149214&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""1367d1eb-20b9-4744-8c43-77663d467e41"",""multiverseId"":149214},""multiverseid"":149214},{""name"":""星光召现师"",""text"":""{7}{W}：你获得5点生命。"",""type"":""生物～人类／僧侣／突变体"",""flavor"":""「星光交织而成的星座图，让我能追寻族人的破碎历史。 那闪烁光芒不分昼夜地召唤着，要我快将图样补齐。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147952&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""0acbeb00-1756-4852-b39f-342e85b9d20e"",""multiverseId"":147952},""multiverseid"":147952}],""printings"":[""10E"",""LGN""],""originalText"":""{7}{W}: You gain 5 life."",""originalType"":""Creature - Human Cleric Mutant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""ecf2b08e-7eca-5f13-a9f7-7a915ee259f5""},{""name"":""Steadfast Guard"",""manaCost"":""{W}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Rebel"",""types"":[""Creature""],""subtypes"":[""Human"",""Rebel""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Vigilance (Attacking doesn't cause this creature to tap.)"",""flavor"":""\""Best leave your tongue in its yap-hole, Mercadian scum, for your silvered words and golden bribes do not sparkle so brightly outside your city.\"""",""artist"":""Michael Komarck"",""number"":""48"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""132111"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132111&type=card"",""variations"":[""c3b8083a-bbe4-524f-8e90-8b8a493152db""],""foreignNames"":[{""name"":""Standhafte Wache"",""text"":""Wachsamkeit (Diese Kreatur wird beim Angreifen nicht getappt.)"",""type"":""Kreatur — Mensch, Rebell"",""flavor"":""„Halt einfach dein Maul, merkadischer Abschaum. Deine silbernen Worte und goldenes Schmiergeld glitzern außerhalb deiner Stadt nicht so hell.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148719&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""1fca278a-824a-41b3-939c-160aafbe1435"",""multiverseId"":148719},""multiverseid"":148719},{""name"":""Guardia resuelto"",""text"":""Vigilancia. (Esta criatura no se gira al atacar.)"",""type"":""Criatura — Rebelde humano"",""flavor"":""\""Mejor deja tu lengua en ese agujero, basura de Mercadia, porque tus palabras de plata y tus sobornos de oro no brillan tanto fuera de tu ciudad.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150364&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""de02ceb6-3239-4a1b-a0bf-f2995ab05d44"",""multiverseId"":150364},""multiverseid"":150364},{""name"":""Garde inébranlable"",""text"":""Vigilance (Attaquer avec cette créature ne la fait pas s'engager.)"",""type"":""Créature : humain et rebelle"",""flavor"":""« Mieux vaut fermer ton clapet, fumier de mercadien. Tes belles paroles et ton or perdent de leur éclat en dehors de ta cité. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149981&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""4ccff7fd-823a-4b79-9850-fb779a3a25d5"",""multiverseId"":149981},""multiverseid"":149981},{""name"":""Guardia Risoluta"",""text"":""Cautela (Attacca senza TAPpare.)"",""type"":""Creatura — Ribelle Umano"",""flavor"":""\""Meglio che chiudi la tua ciabatta, feccia di Mercadia, perché le tue parole argentee e le bustarelle dorate non luccicano poi tanto fuori dalla tua città.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149102&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""a7cd9889-29a0-4673-b3ee-63754d2becd0"",""multiverseId"":149102},""multiverseid"":149102},{""name"":""不動の守備兵"",""text"":""警戒 （このクリーチャーは攻撃してもタップしない。）"",""type"":""クリーチャー — 人間・レベル"",""flavor"":""お前の舌はそのお喋り穴にしまっとけ、メルカディア野郎。銀色の甘言だの金色の賄賂だのは、都市の外に出たらそんなに光りゃしないんだぞ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148336&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""71ecacc0-04c9-462f-887f-f2dae5a580dd"",""multiverseId"":148336},""multiverseid"":148336},{""name"":""Guarda Ferrenha"",""text"":""Vigilância (Esta criatura não é virada para atacar.)"",""type"":""Criatura — Humano Rebelde"",""flavor"":""\""Melhor deixar sua língua na toca, gentalha de Mercádia, pois suas palavras prateadas e seus subornos dourados não têm tanto brilho fora de sua cidade.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149598&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""ffddf3be-88dd-4df0-8061-1ba351cdb9c1"",""multiverseId"":149598},""multiverseid"":149598},{""name"":""Непоколебимый Страж"",""text"":""Бдительность (При нападении это существо не поворачивается.)"",""type"":""Существо — Человек Повстанец"",""flavor"":""\""Лучше заткните глотки, меркадианская мразь Ваши золотые обещания и дорогие посулы тускнеют в чужом краю\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149215&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""438ff6f9-5820-4854-9ea6-96aed9f53587"",""multiverseId"":149215},""multiverseid"":149215},{""name"":""坚定的护卫"",""text"":""警戒（此生物攻击时不需横置。）"",""type"":""生物～人类／反抗军"",""flavor"":""「玛凯迪亚败类，最好把舌头塞回狗嘴里。你的白银巧言与黄金贿赂一出了城，就没你以为的那么闪亮。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147953&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""7bd40d65-f697-4e36-8dc2-488c97cd2dfd"",""multiverseId"":147953},""multiverseid"":147953}],""printings"":[""10E"",""MMQ""],""originalText"":""Vigilance"",""originalType"":""Creature - Human Rebel"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""c81dca90-b0cf-5ce8-965c-8dbe5a55cc1b""},{""name"":""Steadfast Guard"",""manaCost"":""{W}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Rebel"",""types"":[""Creature""],""subtypes"":[""Human"",""Rebel""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Vigilance (Attacking doesn't cause this creature to tap.)"",""flavor"":""\""Best leave your tongue in its yap-hole, Mercadian scum, for your silvered words and golden bribes do not sparkle so brightly outside your city.\"""",""artist"":""Michael Komarck"",""number"":""48★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""c81dca90-b0cf-5ce8-965c-8dbe5a55cc1b""],""printings"":[""10E"",""MMQ""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""c3b8083a-bbe4-524f-8e90-8b8a493152db""},{""name"":""Story Circle"",""manaCost"":""{1}{W}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Enchantment"",""types"":[""Enchantment""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""As Story Circle enters the battlefield, choose a color.\n{W}: The next time a source of your choice of the chosen color would deal damage to you this turn, prevent that damage."",""artist"":""Aleksi Briclot"",""number"":""49"",""layout"":""normal"",""multiverseid"":""129748"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129748&type=card"",""foreignNames"":[{""name"":""Sagenkreis"",""text"":""Bestimme eine Farbe, sowie der Sagenkreis ins Spiel kommt.\n{W}: Sobald eine Quelle der bestimmten Farbe, die du bestimmst, dir das nächste Mal in diesem Zug Schaden zufügen würde, verhindere diesen Schaden."",""type"":""Verzauberung"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148721&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""e1fde162-f872-4f8e-9ca0-2801476cb7b5"",""multiverseId"":148721},""multiverseid"":148721},{""name"":""Círculo de historias"",""text"":""En cuanto el Círculo de historias entre en juego, elige un color.\n{W}: La próxima vez que una fuente de tu elección del color elegido fuera a hacerte daño este turno, prevén ese daño."",""type"":""Encantamiento"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150365&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""09f373da-22e7-4899-a9eb-b5db779b3979"",""multiverseId"":150365},""multiverseid"":150365},{""name"":""Théâtre en cercle"",""text"":""Au moment où le Théâtre en cercle arrive en jeu, choisissez une couleur.\n{W} : La prochaine fois qu'une source de votre choix de la couleur choisie devrait vous blesser ce tour-ci, prévenez ces blessures."",""type"":""Enchantement"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149982&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""4a733d9a-a7d8-4196-aca7-b1f4a098f373"",""multiverseId"":149982},""multiverseid"":149982},{""name"":""Circolo della Storia"",""text"":""Mentre il Circolo della Storia entra in gioco, scegli un colore.\n{W}: La prossima volta che una fonte a tua scelta del colore scelto ti infliggerebbe danno in questo turno, previeni quel danno."",""type"":""Incantesimo"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149104&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""2cb43d29-d824-4024-a8a1-a37c3c640115"",""multiverseId"":149104},""multiverseid"":149104},{""name"":""物語の円"",""text"":""物語の円が場に出るに際し、色を１色選ぶ。\n{W}：このターン、あなたが選んだ、選ばれた色の発生源１つが次にあなたに与えるダメージをすべて軽減し、０にする。"",""type"":""エンチャント"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148338&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""5b7397b3-f9cf-414c-863d-84bde0d77bb4"",""multiverseId"":148338},""multiverseid"":148338},{""name"":""Círculo de Histórias"",""text"":""Conforme Círculo de Histórias entra em jogo, escolha uma cor.\n{W}: Na próxima vez que uma fonte de sua escolha da cor escolhida fosse causar dano a você neste turno, previna aquele dano."",""type"":""Encantamento"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149599&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""230f9fe8-afee-4e64-9a2d-17588aaef8e1"",""multiverseId"":149599},""multiverseid"":149599},{""name"":""Круг Истории"",""text"":""При входе Круга Истории в игру выберите цвет.\n{W}: В следующий раз при нанесении вам повреждений в этом ходу от выбираемого вами источника выбранного цвета предотвратите эти повреждения."",""type"":""Чары"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149216&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""3ac1bb48-4ead-4c38-ba59-27983c7b7589"",""multiverseId"":149216},""multiverseid"":149216},{""name"":""诵传仪典"",""text"":""于诵传仪典进场时，选择一种颜色。\n{W}：选择一个该色的伤害来源，于本回合中，防止该来源下一次将对你造成的伤害。"",""type"":""结界"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147955&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""fe481e2f-b660-46e0-854d-40a5e010234c"",""multiverseId"":147955},""multiverseid"":147955}],""printings"":[""10E"",""8ED"",""9ED"",""MMQ""],""originalText"":""As Story Circle comes into play, choose a color.\n{W}: The next time a source of your choice of the chosen color would deal damage to you this turn, prevent that damage."",""originalType"":""Enchantment"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""b5bdfa8c-00ab-5b6a-b97a-ee77ffcbc4e9""},{""name"":""Suntail Hawk"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Bird"",""types"":[""Creature""],""subtypes"":[""Bird""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying"",""flavor"":""Its eye the glaring sun, its cry the keening wind."",""artist"":""Heather Hudson"",""number"":""50"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""129753"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129753&type=card"",""variations"":[""f286fe97-e2ad-5a5a-8e7f-0caa9dc898b4""],""foreignNames"":[{""name"":""Sonnenschwanz-Falke"",""text"":""Fliegend (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)"",""type"":""Kreatur — Vogel"",""flavor"":""Sein Auge ist wie die brennende Sonne, sein Ruf wie der scharfe Wind."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148727&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""695ea6de-e1ae-476e-b165-538d95e8bdae"",""multiverseId"":148727},""multiverseid"":148727},{""name"":""Halcón colasol"",""text"":""Vuela. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)"",""type"":""Criatura — Ave"",""flavor"":""Su ojo es el sol deslumbrador, su grito es el viento cortante."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150366&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""a78fb31b-90bd-42c0-9b8e-46adc635e01d"",""multiverseId"":150366},""multiverseid"":150366},{""name"":""Faucon mordoré"",""text"":""Vol (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée.)"",""type"":""Créature : oiseau"",""flavor"":""Son regard a la radiance du soleil ; son cri, l'intensité du vent."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149983&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""53edebef-10a4-4f58-9ecf-45663d61d304"",""multiverseId"":149983},""multiverseid"":149983},{""name"":""Falco Astrocoda"",""text"":""Volare (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere.)"",""type"":""Creatura — Uccello"",""flavor"":""Il suo occhio è il sole abbagliante, il suo grido è il vento tagliente."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149110&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""f1e78fc8-a359-43c8-9bb3-f99c9941ce66"",""multiverseId"":149110},""multiverseid"":149110},{""name"":""陽光尾の鷹"",""text"":""飛行 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされない。）"",""type"":""クリーチャー — 鳥"",""flavor"":""その眼は輝く太陽、その叫びは身を切る風。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148344&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""97d89d30-923e-4b57-9758-dedf83a2d767"",""multiverseId"":148344},""multiverseid"":148344},{""name"":""Falcão da Cauda Solar"",""text"":""Voar (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)"",""type"":""Criatura — Ave"",""flavor"":""Seu olho, o sol fulgurante; seu grito, o vento penetrante."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149600&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""6b3f2174-c46b-413b-8676-8a9b4aabcf94"",""multiverseId"":149600},""multiverseid"":149600},{""name"":""Солнечный Ястреб"",""text"":""Полет (Это существо может быть заблокировано только существом с Полетом или Захватом.)"",""type"":""Существо — Птица"",""flavor"":""Взор его яркое солнце, крик его воющий ветер."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149217&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""e239f6b1-29a7-40d2-ae9c-84772a02c643"",""multiverseId"":149217},""multiverseid"":149217},{""name"":""旭羽翔鹰"",""text"":""飞行（只有具飞行或延势异能的生物才能阻挡它。）"",""type"":""生物～鸟"",""flavor"":""目如炫日，鸣似寒风。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147961&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""50bd3993-a13f-4017-b4b1-4e97413de9e4"",""multiverseId"":147961},""multiverseid"":147961}],""printings"":[""10E"",""8ED"",""9ED"",""JUD"",""M14"",""PS11""],""originalText"":""Flying"",""originalType"":""Creature - Bird"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""6b754dda-9a6c-5762-ae67-8069101eb4d4""},{""name"":""Suntail Hawk"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Bird"",""types"":[""Creature""],""subtypes"":[""Bird""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying"",""flavor"":""Its eye the glaring sun, its cry the keening wind."",""artist"":""Heather Hudson"",""number"":""50★"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""variations"":[""6b754dda-9a6c-5762-ae67-8069101eb4d4""],""printings"":[""10E"",""8ED"",""9ED"",""JUD"",""M14"",""PS11""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""f286fe97-e2ad-5a5a-8e7f-0caa9dc898b4""},{""name"":""Tempest of Light"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Destroy all enchantments."",""flavor"":""\""Let everything return to its true nature, so that destiny may takes its course.\"""",""artist"":""Wayne England"",""number"":""51"",""layout"":""normal"",""multiverseid"":""132131"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132131&type=card"",""foreignNames"":[{""name"":""Sturmwind des Lichts"",""text"":""Zerstöre alle Verzauberungen."",""type"":""Spontanzauber"",""flavor"":""„Alles möge zu seiner wahren Natur zurückkehren, so dass das Schicksal seinen Lauf nehmen kann.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148737&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""378ea5dd-9bc0-4dd8-b26b-7a26a758632d"",""multiverseId"":148737},""multiverseid"":148737},{""name"":""Tempestad de luz"",""text"":""Destruye todos los encantamientos."",""type"":""Instantáneo"",""flavor"":""\""Deja que todo regrese a su verdadera naturaleza, para que el destino siga su curso.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150367&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""36f65d3f-c342-4aba-98ee-5483f232e765"",""multiverseId"":150367},""multiverseid"":150367},{""name"":""Tourmente de lumière"",""text"":""Détruisez tous les enchantements."",""type"":""Éphémère"",""flavor"":""« Que tout retrouve sa véritable nature, pour que la destinée suive son cours. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149984&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""99a8c069-f702-465c-a08d-1ec0eade52ad"",""multiverseId"":149984},""multiverseid"":149984},{""name"":""Tempesta di Luce"",""text"":""Distruggi tutti gli incantesimi."",""type"":""Istantaneo"",""flavor"":""\""Che tutto faccia ritorno alla propria natura, affinché il destino possa seguire il proprio corso.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149120&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""f803768c-2cd8-44a8-8806-efcfcbd30fcb"",""multiverseId"":149120},""multiverseid"":149120},{""name"":""光の大嵐"",""text"":""すべてのエンチャントを破壊する。"",""type"":""インスタント"",""flavor"":""運命の導きに従わすべく、すべてを真の姿に戻そう。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148354&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""62f49599-5e7d-41e6-a028-56b5478efcde"",""multiverseId"":148354},""multiverseid"":148354},{""name"":""Tempestade de Luz"",""text"":""Destrua todos os encantamentos."",""type"":""Mágica Instantânea"",""flavor"":""\""Deixe tudo voltar a sua verdadeira natureza, para que o destino possa seguir seu curso.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149601&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""6d925946-5ce9-424a-af90-6dc7b8626682"",""multiverseId"":149601},""multiverseid"":149601},{""name"":""Буря Света"",""text"":""Уничтожьте все чары."",""type"":""Мгновенное заклинание"",""flavor"":""\""Пусть все станет таким, каким его задумала природа, и тогда судьба вернется на круги своя\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149218&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""3d58ac16-06f3-4521-a135-ee914d1534cb"",""multiverseId"":149218},""multiverseid"":149218},{""name"":""明光风暴"",""text"":""消灭所有结界。"",""type"":""瞬间"",""flavor"":""「让万物回归本我，使天命运行无碍。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147971&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""0527aaae-e082-4942-a1f5-ee9b6e129e0a"",""multiverseId"":147971},""multiverseid"":147971}],""printings"":[""10E"",""9ED"",""M10"",""MRD""],""originalText"":""Destroy all enchantments."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""f44e0c48-1740-5e18-859f-4e4615f3a3b1""},{""name"":""Treasure Hunter"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human"",""types"":[""Creature""],""subtypes"":[""Human""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""When Treasure Hunter enters the battlefield, you may return target artifact card from your graveyard to your hand."",""flavor"":""\""The treasures of the ancients belong in museums, not in the grubby hands of grave robbers.\"""",""artist"":""Adam Rex"",""number"":""52"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""135232"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=135232&type=card"",""foreignNames"":[{""name"":""Schatzsucher"",""text"":""Wenn der Schatzsucher ins Spiel kommt, kannst du eine Artefaktkarte deiner Wahl aus deinem Friedhof auf deine Hand zurücknehmen."",""type"":""Kreatur — Mensch"",""flavor"":""„Die Schätze der Ahnen gehören in Museen und nicht in die schwieligen Hände von Grabräubern.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148749&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""7089ba57-991b-4e8b-97e4-235cce08aba6"",""multiverseId"":148749},""multiverseid"":148749},{""name"":""Buscador de tesoros"",""text"":""Cuando el Buscador de tesoros entre en juego, puedes regresar la carta de artefacto objetivo de tu cementerio a tu mano."",""type"":""Criatura — Humano"",""flavor"":""\""Los tesoros antiguos pertenecen a los museos, no a las sucias manos de los saqueadores de tumbas.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150368&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""06a508f5-64b4-4fa7-98e1-644e9ef70fb0"",""multiverseId"":150368},""multiverseid"":150368},{""name"":""Chasseur de trésor"",""text"":""Quand le Chasseur de trésor arrive en jeu, vous pouvez renvoyer une carte d'artefact ciblée depuis votre cimetière dans votre main."",""type"":""Créature : humain"",""flavor"":""« Les trésors des anciens appartiennent aux musées, et pas aux mains avides des pilleurs de tombes. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149985&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""d4e29316-2cbd-4fe5-b1e7-89a0e23391d6"",""multiverseId"":149985},""multiverseid"":149985},{""name"":""Cacciatore di Tesori"",""text"":""Quando il Cacciatore di Tesori entra in gioco, puoi riprendere in mano una carta artefatto bersaglio dal tuo cimitero."",""type"":""Creatura — Umano"",""flavor"":""\""I tesori dell'antichità appartengono ai musei, non alle mani sudicie dei tombaroli.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149132&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""cf723126-5a53-441d-9290-320a760f8902"",""multiverseId"":149132},""multiverseid"":149132},{""name"":""宝捜し"",""text"":""宝捜しが場に出たとき、あなたの墓地にあるアーティファクト・カード１枚を対象とする。あなたはそれをあなたの手札に戻してもよい。"",""type"":""クリーチャー — 人間"",""flavor"":""古代の宝物は博物館にあるものであって、墓荒らしの汚い手にあるべきものじゃない。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148366&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""9fa364d0-c84f-4ee7-9ec1-48645c2187b0"",""multiverseId"":148366},""multiverseid"":148366},{""name"":""Caçador de Tesouros"",""text"":""Quando Caçador de Tesouros entra em jogo, você pode devolver o card de artefato alvo de seu cemitério para a sua mão."",""type"":""Criatura — Humano"",""flavor"":""\""Os tesouros dos anciãos pertencem aos museus, não às mãos sujas dos ladrões de tumbas.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149602&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""00fdc71c-1c60-4633-a15c-d9fb299ffcb6"",""multiverseId"":149602},""multiverseid"":149602},{""name"":""Кладоискатель"",""text"":""Когда Кладоискатель входит в игру, вы можете вернуть целевую карту артефакта из вашего кладбища в вашу руку."",""type"":""Существо — Человек"",""flavor"":""\""Сокровища древних должны попадать в музеи, а не в грязные лапы разорителей могил\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149219&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""bae041f4-f467-42e3-b38c-41ff333cf4e0"",""multiverseId"":149219},""multiverseid"":149219},{""name"":""猎宝者"",""text"":""当猎宝者进场时，你可以将目标神器牌从你的坟墓场移回你手上。"",""type"":""生物～人类"",""flavor"":""「先人的宝藏属于博物馆，而不是盗墓人的脏手。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147983&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""74475c08-2cde-4cc9-82e5-c8265b55d1f0"",""multiverseId"":147983},""multiverseid"":147983}],""printings"":[""10E"",""EXO"",""PLST""],""originalText"":""When Treasure Hunter comes into play, you may return target artifact card from your graveyard to your hand."",""originalType"":""Creature - Human"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""cfa41bcc-edea-53e6-8aec-e7f9d5d23089""},{""name"":""True Believer"",""manaCost"":""{W}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Cleric""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""You have shroud. (You can't be the target of spells or abilities.)"",""flavor"":""So great is his certainty that mere facts cannot shake it."",""artist"":""Alex Horley-Orlandelli"",""number"":""53"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""129610"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129610&type=card"",""variations"":[""c84cfe08-d681-58e9-a003-1447f3b6d0f7""],""foreignNames"":[{""name"":""Rechtgläubiger"",""text"":""Du bist verhüllt. (Du kannst nicht das Ziel von Zaubersprüchen oder Fähigkeiten sein.)"",""type"":""Kreatur — Mensch, Kleriker"",""flavor"":""Es ist sich seiner Sache so sicher, dass noch nicht mal Tatsachen ihn umwerfen können."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148753&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""c52d6b06-ccae-4d09-8ac2-169504205bc6"",""multiverseId"":148753},""multiverseid"":148753},{""name"":""Creyente verdadero"",""text"":""Tienes la habilidad de velo. (No puedes ser objetivo de hechizos o habilidades.)"",""type"":""Criatura — Clérigo humano"",""flavor"":""Tan grande es su certeza que los meros hechos no la afectan."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150369&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""24f762f1-dfa4-4aba-8f02-e012b33b621b"",""multiverseId"":150369},""multiverseid"":150369},{""name"":""Adepte convaincu"",""text"":""Vous avez le linceul. (Vous ne pouvez pas être la cible de sorts ou de capacités.)"",""type"":""Créature : humain et clerc"",""flavor"":""Sa certitude est telle que les faits ne l'ébranlent même plus."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149986&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""53ecd1f4-24a0-4fe0-93d9-70a8be638fe2"",""multiverseId"":149986},""multiverseid"":149986},{""name"":""Vero Credente"",""text"":""Hai velo. (Non puoi essere bersaglio di magie o abilità.)"",""type"":""Creatura — Chierico Umano"",""flavor"":""La sua certezza è così grande che i semplici fatti non riescono a scuoterla."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149136&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""46dd1a01-83d5-4fd7-8a79-5bd21fc81a95"",""multiverseId"":149136},""multiverseid"":149136},{""name"":""真実の信仰者"",""text"":""あなたは被覆を持つ。 （あなたは呪文や能力の対象にならない。）"",""type"":""クリーチャー — 人間・クレリック"",""flavor"":""彼の確信は偉大なもので、単なる事実などには心揺さぶられない。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148370&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""bef84359-fbf8-4a39-9ad8-3b4982901303"",""multiverseId"":148370},""multiverseid"":148370},{""name"":""Adepto da Verdade"",""text"":""Você tem manto. (Você não pode ser alvo de mágicas ou habilidades.)"",""type"":""Criatura — Humano Clérigo"",""flavor"":""Sua certeza é tão grande que meros fatos não podem abalá-la."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149603&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""8dbb90da-8050-4b03-a072-808e236f7ad8"",""multiverseId"":149603},""multiverseid"":149603},{""name"":""Правоверный"",""text"":""Вы имеете Пелену. (Вы не можете быть целью заклинаний или способностей.)"",""type"":""Существо — Человек Священник"",""flavor"":""Его уверенность настолько велика, что даже факты не могут заставить его усомниться."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149220&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""6c56708a-5eb4-4fce-bcc2-d55bd6c0e280"",""multiverseId"":149220},""multiverseid"":149220},{""name"":""真信者"",""text"":""你具有帷幕异能。 （你不能成为咒语或异能的目标。）"",""type"":""生物～人类／僧侣"",""flavor"":""他的信念无比坚定，单将事实摊在眼前还无法令他动摇分毫。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147987&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""4407aa95-298e-4c56-a7ae-fb45983503bf"",""multiverseId"":147987},""multiverseid"":147987}],""printings"":[""10E"",""ONS""],""originalText"":""You have shroud."",""originalType"":""Creature - Human Cleric"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""46743f03-e0a4-5976-8d42-c3b698384b4e""},{""name"":""True Believer"",""manaCost"":""{W}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Cleric""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""You have shroud. (You can't be the target of spells or abilities.)"",""flavor"":""So great is his certainty that mere facts cannot shake it."",""artist"":""Alex Horley-Orlandelli"",""number"":""53★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""46743f03-e0a4-5976-8d42-c3b698384b4e""],""printings"":[""10E"",""ONS""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""c84cfe08-d681-58e9-a003-1447f3b6d0f7""},{""name"":""Tundra Wolves"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Wolf"",""types"":[""Creature""],""subtypes"":[""Wolf""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""First strike (This creature deals combat damage before creatures without first strike.)"",""flavor"":""\""I heard their eerie howling, the wolves calling their kindred across the frozen plains.\""\n—Onean scout"",""artist"":""Richard Sardinha"",""number"":""54"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""129604"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129604&type=card"",""variations"":[""ede09d78-f4b5-59eb-8356-dd125323f74f""],""foreignNames"":[{""name"":""Tundrawölfe"",""text"":""Erstschlag (Diese Kreatur fügt Kampfschaden vor Kreaturen ohne Erstschlag zu.)"",""type"":""Kreatur — Wolf"",""flavor"":""„Ich hörte ein unheimliches Heulen, die Wölfe versammelten ihre Rudel auf den gefrorenen Ebenen.\"" —Oneanischer Späher"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148754&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""f9afd1a6-dc36-4785-a710-d71c4ba4ff86"",""multiverseId"":148754},""multiverseid"":148754},{""name"":""Lobos de la tundra"",""text"":""Daña primero. (Esta criatura hace daño de combate antes que las criaturas sin la habilidad de dañar primero.)"",""type"":""Criatura — Lobo"",""flavor"":""\""Oí sus aullidos espectrales, los lobos llamando a sus parientes a través de las praderas congeladas.\"" —Explorador oneano"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150370&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""20ef1de7-0611-4cb0-aaf0-c5d2e5814ca0"",""multiverseId"":150370},""multiverseid"":150370},{""name"":""Loups de la toundra"",""text"":""Initiative (Cette créature inflige des blessures de combat avant les créatures sans l'initiative.)"",""type"":""Créature : loup"",""flavor"":""« J'entendis alors leur hurlement lugubre ; les loups appelaient leurs frères sur la plaine glacée. » —Un éclaireur onéan"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149987&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""152b13eb-f406-4edf-bdd3-71ca4fc38cfd"",""multiverseId"":149987},""multiverseid"":149987},{""name"":""Lupi della Tundra"",""text"":""Attacco improvviso (Questa creatura infligge danno da combattimento prima delle creature senza attacco improvviso.)"",""type"":""Creatura — Lupo"",""flavor"":""\""Ho udito il loro ululato sovrannaturale, i lupi che chiamavano i propri simili attraverso le pianure gelate.\"" —Esploratore Oneano"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149137&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""17e6886f-7241-4f35-b13a-795cc900fd80"",""multiverseId"":149137},""multiverseid"":149137},{""name"":""ツンドラ狼"",""text"":""先制攻撃 （このクリーチャーは先制攻撃を持たないクリーチャーよりも先に戦闘ダメージを与える。）"",""type"":""クリーチャー — 狼"",""flavor"":""不気味な叫びが聞こえた。狼たちが凍れる野のあちこちから仲間を呼び集めているのだ。 ――オネイアンの斥候"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148371&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""b1af8fa4-46b9-4277-a6f7-9485e229ce64"",""multiverseId"":148371},""multiverseid"":148371},{""name"":""Lobos da Tundra"",""text"":""Iniciativa (Esta criatura causa dano de combate antes de criaturas sem a habilidade de iniciativa.)"",""type"":""Criatura — Lobo"",""flavor"":""\""Eu ouço seus uivos lúgubres, os lobos chamam sua família por toda a planície congelada.\"" — Batedor de Oneah"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149604&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""54ecb172-1ba2-4bb9-90dd-60060495f0e3"",""multiverseId"":149604},""multiverseid"":149604},{""name"":""Волки Тундры"",""text"":""Первый удар (Это существо наносит боевые повреждения раньше существ без Первого удара.)"",""type"":""Существо — Волк"",""flavor"":""\""Я слышал это жуткое завывание так выли волки, зовущие свою стаю на бескрайней промерзлой равнине\"". — Онеанский разведчик"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149221&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""c98f8aed-1458-40a7-9d06-b0556e97714e"",""multiverseId"":149221},""multiverseid"":149221},{""name"":""苔原狼"",""text"":""先攻（此生物会比不具先攻异能的生物提前造成战斗伤害。）"",""type"":""生物～狼"",""flavor"":""「我听到他们凄厉的吼声；狼群正呼叫冻原另一端的家族成员。」 ～欧尼亚斥候"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147988&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""644cb842-0d79-46d2-bfd8-9d3956099c78"",""multiverseId"":147988},""multiverseid"":147988}],""printings"":[""10E"",""4BB"",""4ED"",""5ED"",""6ED"",""8ED"",""LEG"",""PS11"",""REN""],""originalText"":""First strike"",""originalType"":""Creature - Wolf"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""94cb0244-f6ae-50c5-ba26-fd3f861c3bb0""},{""name"":""Tundra Wolves"",""manaCost"":""{W}"",""cmc"":1.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Wolf"",""types"":[""Creature""],""subtypes"":[""Wolf""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""First strike (This creature deals combat damage before creatures without first strike.)"",""flavor"":""\""I heard their eerie howling, the wolves calling their kindred across the frozen plains.\""\n—Onean scout"",""artist"":""Richard Sardinha"",""number"":""54★"",""power"":""1"",""toughness"":""1"",""layout"":""normal"",""variations"":[""94cb0244-f6ae-50c5-ba26-fd3f861c3bb0""],""printings"":[""10E"",""4BB"",""4ED"",""5ED"",""6ED"",""8ED"",""LEG"",""PS11"",""REN""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""ede09d78-f4b5-59eb-8356-dd125323f74f""},{""name"":""Venerable Monk"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Monk Cleric"",""types"":[""Creature""],""subtypes"":[""Human"",""Monk"",""Cleric""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""When Venerable Monk enters the battlefield, you gain 2 life."",""flavor"":""Age wears the flesh but galvanizes the soul."",""artist"":""D. Alexander Gregory"",""number"":""55"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""129786"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129786&type=card"",""foreignNames"":[{""name"":""Ehrwürdiger Mönch"",""text"":""Wenn der Ehrwürdige Mönch ins Spiel kommt, erhältst du 2 Lebenspunkte dazu."",""type"":""Kreatur — Mensch, Mönch, Kleriker"",""flavor"":""Zeit heilt Wunden, verwandelt sie aber gleichzeitig in Runzeln."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148765&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""b926e8a7-a2ad-4fde-a02f-aa800f0df06d"",""multiverseId"":148765},""multiverseid"":148765},{""name"":""Monje venerable"",""text"":""Cuando el Monje venerable entre en juego, gana 2 vidas."",""type"":""Criatura — Clérigo monje humano"",""flavor"":""La edad desgasta el cuerpo pero exalta el alma."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150371&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""8566098a-945a-4a01-b6b5-a474742b0d95"",""multiverseId"":150371},""multiverseid"":150371},{""name"":""Moine vénérable"",""text"":""Quand le Moine vénérable arrive en jeu, vous gagnez 2 points de vie."",""type"":""Créature : humain et moine et clerc"",""flavor"":""L'âge use la chair, mais renforce l'âme."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149988&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""2ca1db37-b754-4de7-ac86-9df6bfefc9a2"",""multiverseId"":149988},""multiverseid"":149988},{""name"":""Monaco Venerabile"",""text"":""Quando il Monaco Venerabile entra in gioco, guadagni 2 punti vita."",""type"":""Creatura — Chierico Monaco Umano"",""flavor"":""L'età consuma la carne ma galvanizza l'anima."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149148&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""4138b9c4-66a5-4f17-b3b1-8ec6fa63440e"",""multiverseId"":149148},""multiverseid"":149148},{""name"":""ありがたい老修道士"",""text"":""ありがたい老修道士が場に出たとき、あなたは２点のライフを得る。"",""type"":""クリーチャー — 人間・モンク・クレリック"",""flavor"":""加齢によって肉体は衰えていくが、魂は活気づいていく。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148382&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""8b6f0f92-bbc8-4798-884f-5c7115219d3c"",""multiverseId"":148382},""multiverseid"":148382},{""name"":""Monge Venerável"",""text"":""Quando Monge Venerável entra em jogo, você ganha 2 pontos de vida."",""type"":""Criatura — Humano Monge Clérigo"",""flavor"":""A idade desgasta a carne mas fortalece a alma."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149605&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""34f221fa-cd0e-4655-861b-d341f86034ee"",""multiverseId"":149605},""multiverseid"":149605},{""name"":""Почтенный Монах"",""text"":""Когда Почтенный Монах входит в игру, вы получаете 2 жизни."",""type"":""Существо — Человек Монах Священник"",""flavor"":""С годами изнашивается тело, но закаляется душа."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149222&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""316a009a-5fa2-439b-badd-07221f45dc5f"",""multiverseId"":149222},""multiverseid"":149222},{""name"":""可敬的修行僧"",""text"":""当可敬的修行僧进场时，你获得2点生命。"",""type"":""生物～人类／修行僧／僧侣"",""flavor"":""岁月磨蚀肉身，却光亮了心灵。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147999&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""4d1bf680-7b5d-4e2b-a170-249c3e52ce15"",""multiverseId"":147999},""multiverseid"":147999}],""printings"":[""10E"",""6ED"",""7ED"",""8ED"",""9ED"",""DDC"",""DVD"",""POR"",""PS11"",""S99"",""STH""],""originalText"":""When Venerable Monk comes into play, you gain 2 life."",""originalType"":""Creature - Human Monk Cleric"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""8732b0a8-0c4e-5179-9855-d139dbad85c2""},{""name"":""Voice of All"",""manaCost"":""{2}{W}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Angel"",""types"":[""Creature""],""subtypes"":[""Angel""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying\nAs Voice of All enters the battlefield, choose a color.\nVoice of All has protection from the chosen color."",""artist"":""rk post"",""number"":""56"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""136290"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=136290&type=card"",""variations"":[""3c6b79b0-1a84-5584-83c6-954430868f62""],""foreignNames"":[{""name"":""Stimme von Allem"",""text"":""Fliegend (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)\nSowie die Stimme von Allem ins Spiel kommt, bestimme eine Farbe.\nDie Stimme von Allem hat Schutz vor dieser Farbe. (Sie kann von nichts der bestimmten Farbe geblockt oder als Ziel bestimmt werden, davon Schaden zugefügt bekommen oder davon verzaubert werden.)"",""type"":""Kreatur — Engel"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148770&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""5e37e4e0-bd7b-42b8-9885-c9ce4b99147a"",""multiverseId"":148770},""multiverseid"":148770},{""name"":""Voz de todos"",""text"":""Vuela. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)\nEn cuanto la Voz de todos entre en juego, elige un color.\nLa Voz de todos tiene protección contra el color elegido. (No puede ser bloqueada, hecha objetivo, recibir daño de o estar encantada por nada del color elegido.)"",""type"":""Criatura — Ángel"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150372&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""96718ea9-3753-46bf-bd05-13c7356aa7c0"",""multiverseId"":150372},""multiverseid"":150372},{""name"":""Voix du Grand Tout"",""text"":""Vol (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée.)\nAu moment où la Voix du Grand Tout arrive en jeu, choisissez une couleur.\nLa Voix du Grand Tout a la protection contre la couleur choisie. (Elle ne peut pas être bloquée, ciblée, blessée ou enchantée par une source de la couleur choisie.)"",""type"":""Créature : ange"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149989&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""e37073de-5b9b-492f-9275-89001b3d909a"",""multiverseId"":149989},""multiverseid"":149989},{""name"":""Voce Omnia"",""text"":""Volare (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere.)\nMentre la Voce Omnia entra in gioco, scegli un colore.\nLa Voce Omnia ha protezione dal colore scelto. (Non può essere bloccata, bersagliata, non le può essere inflitto danno, né può essere incantata da nulla del colore scelto.)"",""type"":""Creatura — Angelo"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149153&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""e596f288-2378-4cd9-9dcd-8d4855a61c3a"",""multiverseId"":149153},""multiverseid"":149153},{""name"":""万物の声"",""text"":""飛行 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされない。）\n万物の声が場に出るに際し、色を１色選ぶ。\n万物の声は選ばれた色に対するプロテクションを持つ。 （それは選ばれた色に対して、ブロックされず、対象にならず、ダメージを与えられず、エンチャントされない。）"",""type"":""クリーチャー — 天使"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148387&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""fdfdf172-d55f-40ae-9390-36c04f5dccd6"",""multiverseId"":148387},""multiverseid"":148387},{""name"":""Porta-voz"",""text"":""Voar (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)\nConforme Porta-voz entra em jogo, escolha uma cor.\nPorta-voz tem proteção contra a cor escolhida. (Ela não pode ser bloqueada, ser alvo, sofrer dano nem ser encantada por algo da cor escolhida.)"",""type"":""Criatura — Anjo"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149606&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""f8e29f39-1d37-4c06-8370-edb7831390df"",""multiverseId"":149606},""multiverseid"":149606},{""name"":""Всеобщий Глас"",""text"":""Полет (Это существо может быть заблокировано только существом с Полетом или Захватом.)\nПри входе Всеобщего Гласа в игру выберите цвет.\nВсеобщий Глас имеет Защиту от выбранного цвета. (Ни один объект выбранного цвета не может блокировать его, делать его своей целью, наносить ему повреждения и зачаровывать его.)"",""type"":""Существо — Ангел"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149223&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""b93d2bcb-2fe7-4178-9be0-0c48ac8d9dd2"",""multiverseId"":149223},""multiverseid"":149223},{""name"":""万物使者"",""text"":""飞行（只有具飞行或延势异能的生物才能阻挡它。）\n于万物使者进场时，选择一种颜色。\n万物使者具有反该色的保护异能。 （它不能被该色的东西所阻挡，指定为目标，造成伤害，或是被结附。）"",""type"":""生物～天使"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148004&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""9b011356-b41c-4667-b497-4a859de7b57b"",""multiverseId"":148004},""multiverseid"":148004}],""printings"":[""10E"",""CMA"",""CMD"",""DMR"",""PLS"",""PS11""],""originalText"":""Flying\nAs Voice of All comes into play, choose a color.\nVoice of All has protection from the chosen color."",""originalType"":""Creature - Angel"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""814b54df-221c-5fdb-a516-0fbd438b57a7""},{""name"":""Voice of All"",""manaCost"":""{2}{W}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Angel"",""types"":[""Creature""],""subtypes"":[""Angel""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying\nAs Voice of All enters the battlefield, choose a color.\nVoice of All has protection from the chosen color."",""artist"":""rk post"",""number"":""56★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""814b54df-221c-5fdb-a516-0fbd438b57a7""],""printings"":[""10E"",""CMA"",""CMD"",""DMR"",""PLS"",""PS11""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""3c6b79b0-1a84-5584-83c6-954430868f62""},{""name"":""Wall of Swords"",""manaCost"":""{3}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Wall"",""types"":[""Creature""],""subtypes"":[""Wall""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Defender (This creature can't attack.)\nFlying"",""flavor"":""The air hummed with the scissoring sound of uncounted blades that hovered in front of the invaders as though wielded by a phalanx of unseen hands."",""artist"":""Zoltan Boros & Gabor Szikszai"",""number"":""57"",""power"":""3"",""toughness"":""5"",""layout"":""normal"",""multiverseid"":""132120"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132120&type=card"",""variations"":[""4ffc3780-0054-5128-b97e-b19d06b483c6""],""foreignNames"":[{""name"":""Schwertmauer"",""text"":""Verteidiger, Fliegend (Diese Kreatur kann nicht angreifen und kann fliegende Kreaturen blocken.)"",""type"":""Kreatur — Mauer"",""flavor"":""Die Eindringlinge blieben stehen, als plötzlich die Luft summte und sich unzählige Klingen vor ihnen materialisierten, als ob sie von einer Phalanx aus unsichtbaren Händen geschwungen würden."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148773&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""590123a0-c89f-458b-9f1a-668a269e44cd"",""multiverseId"":148773},""multiverseid"":148773},{""name"":""Muro de espadas"",""text"":""Defensor, vuela. (Esta criatura no puede atacar y puede bloquear criaturas que vuelan.)"",""type"":""Criatura — Muro"",""flavor"":""El aire vibró con el cortante sonido de incontables filos que flotaban frente a los invasores como si estuvieran esgrimidos por una falange de manos invisibles."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150373&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""45608064-b040-4ab5-aa62-c217bd9bcaed"",""multiverseId"":150373},""multiverseid"":150373},{""name"":""Mur d'épées"",""text"":""Défenseur, vol (Cette créature ne peut pas attaquer et elle peut bloquer les créatures avec le vol.)"",""type"":""Créature : mur"",""flavor"":""L'air résonnait du cisaillement des lames innombrables flottant devant les envahisseurs comme brandies par une armée de mains invisibles."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149990&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""717144f8-45f5-454b-bcb7-e28a71d4f2da"",""multiverseId"":149990},""multiverseid"":149990},{""name"":""Muro di Spade"",""text"":""Difensore, volare (Questa creatura non può attaccare, e può bloccare le creature con volare.)"",""type"":""Creatura — Muro"",""flavor"":""L'aria ronzava del rumore sforbiciante di innumerevoli lame, sospese di fronte agli invasori come se fossero impugnate da una falange di mani invisibili."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149156&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""c92d0b89-385a-4a7f-9e31-d21fb8ad3ff7"",""multiverseId"":149156},""multiverseid"":149156},{""name"":""剣の壁"",""text"":""防衛、飛行 （このクリーチャーは攻撃できず、飛行を持つクリーチャーをブロックできる。）"",""type"":""クリーチャー — 壁"",""flavor"":""侵略者の前に浮かんだ数え切れないほどの刃の切り刻む音が、空気に響いていた。それはまるで見えざる手の密集軍が抱える武器のようだった。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148390&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""5180e7ea-0822-4131-96ad-f7a746eaa81d"",""multiverseId"":148390},""multiverseid"":148390},{""name"":""Barreira de Espadas"",""text"":""Defensor, voar (Esta criatura não pode atacar, e pode bloquear criaturas com a habilidade de voar.)"",""type"":""Criatura — Barreira"",""flavor"":""O ar sussurrou com o som cortante de incontáveis lâminas que pairaram perante os invasores, como se empunhadas por uma falange de mãos invisíveis."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149607&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""2dae14ff-32fb-4670-a6f3-124b4d09e898"",""multiverseId"":149607},""multiverseid"":149607},{""name"":""Стена Мечей"",""text"":""Защитник, Полет (Это существо не может атаковать и может блокировать существа с Полетом.)"",""type"":""Существо — Стена"",""flavor"":""Воздух гудел от лязганья бесчисленных клинков, нависших над противником, словно ими управляла стена невидимых рук."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149224&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""4998348d-1972-4cad-a8fa-1dfb63f9c36f"",""multiverseId"":149224},""multiverseid"":149224},{""name"":""剑墙"",""text"":""守军，飞行（此生物不能攻击，且能阻挡具飞行异能的生物。）"",""type"":""生物～墙"",""flavor"":""入侵者前方的大气回荡尖啸，透出无数刀刃的破风声响，有如不见身影的方阵兵挥舞着兵器。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148007&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""55e7a929-186b-4b20-afd9-42507c786f3d"",""multiverseId"":148007},""multiverseid"":148007}],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""5ED"",""6ED"",""7ED"",""8ED"",""CED"",""CEI"",""FBB"",""LEA"",""LEB"",""M14"",""POR"",""SUM""],""originalText"":""Defender, flying"",""originalType"":""Creature - Wall"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""fefca4a6-440f-56be-ba90-b8587c55d347""},{""name"":""Wall of Swords"",""manaCost"":""{3}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Wall"",""types"":[""Creature""],""subtypes"":[""Wall""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Defender (This creature can't attack.)\nFlying"",""flavor"":""The air hummed with the scissoring sound of uncounted blades that hovered in front of the invaders as though wielded by a phalanx of unseen hands."",""artist"":""Zoltan Boros & Gabor Szikszai"",""number"":""57★"",""power"":""3"",""toughness"":""5"",""layout"":""normal"",""variations"":[""fefca4a6-440f-56be-ba90-b8587c55d347""],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""5ED"",""6ED"",""7ED"",""8ED"",""CED"",""CEI"",""FBB"",""LEA"",""LEB"",""M14"",""POR"",""SUM""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""4ffc3780-0054-5128-b97e-b19d06b483c6""},{""name"":""Warrior's Honor"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Creatures you control get +1/+1 until end of turn."",""flavor"":""\""The day will come when the righteous warrior faces a battle she cannot win. She will greet that day as she has any other.\""\n—Asmira, holy avenger"",""artist"":""D. Alexander Gregory"",""number"":""58"",""layout"":""normal"",""multiverseid"":""129797"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129797&type=card"",""rulings"":[{""date"":""2007-07-15"",""text"":""Warrior’s Honor affects creatures you control that are on the battlefield at the time it resolves. If you put a creature onto the battlefield later in the turn, that creature won’t get the bonus.""}],""foreignNames"":[{""name"":""Kriegerehre"",""text"":""Kreaturen, die du kontrollierst, erhalten +1/+1 bis zum Ende des Zuges."",""type"":""Spontanzauber"",""flavor"":""„Es wird der Tag kommen, an der die rechtschaffene Kriegerin vor einem Kampf steht, den sie nicht gewinnen kann. Und doch wird sie den Tag begrüßen wie jeden zuvor.\"" —Asmira, die heilige Rächerin"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148776&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""9d7da5e0-9b73-465a-b64f-0963e94ae362"",""multiverseId"":148776},""multiverseid"":148776},{""name"":""Honor de guerrero"",""text"":""Las criaturas que controles obtienen +1/+1 hasta el final del turno."",""type"":""Instantáneo"",""flavor"":""\""Llegará el día en que la guerrera virtuosa enfrente una batalla que no puede ganar. Ella saludará a ese día como a cualquier otro.\"" —Asmira, vengadora sagrada"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150374&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""ff79877a-e96b-47b4-813f-6714078c711e"",""multiverseId"":150374},""multiverseid"":150374},{""name"":""Honneur du guerrier"",""text"":""Les créatures que vous contrôlez gagnent +1/+1 jusqu'à la fin du tour."",""type"":""Éphémère"",""flavor"":""« Il vient un jour où la juste guerrière mène un combat qu'elle ne peut pas gagner. Elle savoure cette journée comme toutes les précédentes. » —Asmira, Sainte vengeresse"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149991&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""e35c2334-3715-4f16-afb9-214e745c8d56"",""multiverseId"":149991},""multiverseid"":149991},{""name"":""Onore del Guerriero"",""text"":""Le creature che controlli prendono +1/+1 fino alla fine del turno."",""type"":""Istantaneo"",""flavor"":""\""Verrà il giorno in cui il guerriero virtuoso affronta una battaglia che non può vincere. E saluterà quel giorno come ha sempre fatto con tutti gli altri.\"" —Asmira, vendicatrice sacra"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149159&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""94213214-5e58-4758-9d16-d18fc0be5d7d"",""multiverseId"":149159},""multiverseid"":149159},{""name"":""戦士の誉れ"",""text"":""あなたがコントロールするクリーチャーは、ターン終了時まで＋１/＋１の修整を受ける。"",""type"":""インスタント"",""flavor"":""高潔なる戦士も、勝ち目の無い戦いに面する日はやってきます。 彼女はその日を、いつもと変わらない一日として迎えるでしょう。 ――聖なる報復者アズマイラ"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148393&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""ec175c8e-59a2-4fa9-83e2-7234bf3de562"",""multiverseId"":148393},""multiverseid"":148393},{""name"":""Honra de Guerreiro"",""text"":""As criaturas que você controla recebem +1/+1 até o final do turno."",""type"":""Mágica Instantânea"",""flavor"":""\""Virá um dia em que a brava guerreira enfrentará uma batalha que não poderá vencer. Ela saudará esse dia como qualquer outro.\"" — Asmira, vingadora santa"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149608&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""e1fec512-5e80-4bfe-a47c-2c2b87d43af7"",""multiverseId"":149608},""multiverseid"":149608},{""name"":""Честь Воина"",""text"":""Существа под вашим контролем получают +1/+1 до конца хода."",""type"":""Мгновенное заклинание"",""flavor"":""\""Наступит день, когда праведная воительница должна будет принять бой, в котором не сможет одержать победу. Она встретит этот день, как любой другой\"". — Асмира, святая мстительница"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149225&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""c081fd71-c529-412b-a21f-56bd730cfe6d"",""multiverseId"":149225},""multiverseid"":149225},{""name"":""战士之光"",""text"":""由你操控的生物得+1/+1直到回合结束。"",""type"":""瞬间"",""flavor"":""「正义战士终将面对一场不可能取胜的战斗。 而她将如同往日地祝福这一天。」 ～神圣复仇者阿兹蜜拉"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148010&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""5720a1fa-9787-427a-bde6-e0e1b56cf135"",""multiverseId"":148010},""multiverseid"":148010}],""printings"":[""10E"",""6ED"",""9ED"",""ATH"",""VIS""],""originalText"":""Creatures you control get +1/+1 until end of turn."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""2bf3fff9-15c9-5cda-9a04-d0ea22180c77""},{""name"":""Wild Griffin"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Griffin"",""types"":[""Creature""],""subtypes"":[""Griffin""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying"",""flavor"":""\""I abandoned my dream of a squadron of griffin-riders when the cost proved too high. Three trainers were eaten for every griffin broken to the bridle.\""\n—King Darien of Kjeldor"",""artist"":""Matt Cavotta"",""number"":""59"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""129557"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129557&type=card"",""variations"":[""9e583bce-5201-59f8-aa5c-f85c55824f41""],""foreignNames"":[{""name"":""Wildgreif"",""text"":""Fliegend (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)"",""type"":""Kreatur — Greif"",""flavor"":""„Ich musste meinen Traum von einem Geschwader Greife aufgeben. Jeder abgerichtete Greif hat mich mindestens drei gefressene Tiertrainer gekostet.\"" —König Darien von Kjeldor"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148778&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""a4d265bf-a456-42e0-9acb-4048dfa09612"",""multiverseId"":148778},""multiverseid"":148778},{""name"":""Grifo salvaje"",""text"":""Vuela. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)"",""type"":""Criatura — Grifo"",""flavor"":""\""Abandoné mi sueño de un escuadrón de jinetes de grifos cuando fue evidente que el costo sería demasiado alto. Tres entrenadores fueron devorados por cada grifo domado.\"" —Rey Darien de Kjeldor"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150375&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""19b302d3-6232-4e21-b6c3-e20a4ee6fbaa"",""multiverseId"":150375},""multiverseid"":150375},{""name"":""Griffon sauvage"",""text"":""Vol (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée.)"",""type"":""Créature : griffon"",""flavor"":""« J'ai dû abandonner mon rêve d'escadron de chevaucheurs de griffons car le coût était trop élevé. Pour chaque griffon dressé, trois dresseurs étaient dévorés. » —Darien, roi du Kjeldor"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149992&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""b19d0ae9-c097-4b1c-8aee-6b79682c8432"",""multiverseId"":149992},""multiverseid"":149992},{""name"":""Grifone Selvaggio"",""text"":""Volare (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere.)"",""type"":""Creatura — Grifone"",""flavor"":""\""Ho abbandonato il mio sogno di uno squadrone di cavalca-grifoni quando il costo si è dimostrato troppo alto. Tre addestratori mangiati per ogni grifone domato.\"" —Re Darien di Kjeldor"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149161&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""fbfc267a-4c57-44e1-b980-eb586634e6bb"",""multiverseId"":149161},""multiverseid"":149161},{""name"":""野生のグリフィン"",""text"":""飛行 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされない。）"",""type"":""クリーチャー — グリフィン"",""flavor"":""グリフィン乗りの部隊を持つ夢は、コストがかかりすぎるために諦めた。 グリフィンが一匹を馴らすのまでに、訓練官が三人食われてしまうのだ。 ――キイェルドーの王、ダリアン"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148395&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""2b8dac21-d195-4d3b-9af9-82811af37a2d"",""multiverseId"":148395},""multiverseid"":148395},{""name"":""Grifo Selvagem"",""text"":""Voar (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)"",""type"":""Criatura — Grifo"",""flavor"":""\""Abandonei meu sonho de um esquadrão de ginetes de grifos quando o custo se mostrou muito alto. Três treinadores foram devorados para cada grifo domesticado.\"" — Darien, Rei de Kjeldor"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149609&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""ef2bdddf-5e05-42ae-8e98-ae777b1b29d1"",""multiverseId"":149609},""multiverseid"":149609},{""name"":""Дикий Грифон"",""text"":""Полет (Это существо может быть заблокировано только существом с Полетом или Захватом.)"",""type"":""Существо — Грифон"",""flavor"":""\""Я отказался от давней мечты иметь эскадрилью наездников грифонов, когда понял, какой ценой это дается. Каждый обузданный грифон обошелся мне в три сожранных наездника\"". — Дариен, Король Кьельдора"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149226&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""e1226625-1b02-431e-adeb-ac4fd8abe9d2"",""multiverseId"":149226},""multiverseid"":149226},{""name"":""野生狮鹫"",""text"":""飞行（只有具飞行或延势异能的生物才能阻挡它。）"",""type"":""生物～狮鹫"",""flavor"":""「由于代价太高，我只好放弃建立狮鹫骑兵中队的梦想。 只要一匹狮鹫挣脱辔头，就会吃掉三名训练师。」 ～奇亚多王达利安"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148012&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""92cfcc5b-f28f-43b4-a457-13c27bc48333"",""multiverseId"":148012},""multiverseid"":148012}],""printings"":[""10E"",""CN2"",""M11"",""ME4"",""P02"",""PLST"",""S00"",""S99""],""originalText"":""Flying"",""originalType"":""Creature - Griffin"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""75e792c1-d5d9-5712-9462-230fa196f861""},{""name"":""Wild Griffin"",""manaCost"":""{2}{W}"",""cmc"":3.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Griffin"",""types"":[""Creature""],""subtypes"":[""Griffin""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying"",""flavor"":""\""I abandoned my dream of a squadron of griffin-riders when the cost proved too high. Three trainers were eaten for every griffin broken to the bridle.\""\n—King Darien of Kjeldor"",""artist"":""Matt Cavotta"",""number"":""59★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""75e792c1-d5d9-5712-9462-230fa196f861""],""printings"":[""10E"",""CN2"",""M11"",""ME4"",""P02"",""PLST"",""S00"",""S99""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""9e583bce-5201-59f8-aa5c-f85c55824f41""},{""name"":""Windborn Muse"",""manaCost"":""{3}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Spirit"",""types"":[""Creature""],""subtypes"":[""Spirit""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying\nCreatures can't attack you unless their controller pays {2} for each creature they control that's attacking you."",""flavor"":""\""Her voice is justice, clear and relentless.\""\n—Akroma, angelic avenger"",""artist"":""Adam Rex"",""number"":""60"",""power"":""2"",""toughness"":""3"",""layout"":""normal"",""multiverseid"":""130549"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130549&type=card"",""variations"":[""9e49c166-9728-5259-b3e9-758b3c1357c6""],""rulings"":[{""date"":""2022-12-08"",""text"":""Creatures may still attack planeswalkers you control without their controller paying a cost.""}],""foreignNames"":[{""name"":""Muse des Windgeflüsters"",""text"":""Fliegend (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)\nKreaturen können dich nicht angreifen, falls ihr Beherrscher nicht für jede Kreatur, die er kontrolliert und die dich angreift, {2} bezahlt."",""type"":""Kreatur — Geist"",""flavor"":""„Ihre Stimme ist die der Rache, kalt und unbarmherzig.\"" —Akroma, rächender Engel"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148779&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""097db74b-e7a9-422f-a203-355812efc7ed"",""multiverseId"":148779},""multiverseid"":148779},{""name"":""Musa nacida del viento"",""text"":""Vuela. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)\nLas criaturas no pueden atacarte a menos que su controlador pague {2} por cada criatura que controla que te está atacando."",""type"":""Criatura — Espíritu"",""flavor"":""\""Su voz es justicia, clara e implacable.\"" —Akroma, vengadora angélica"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150376&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""583186d4-ac44-4bce-a7bb-b3d22d0bf3ba"",""multiverseId"":150376},""multiverseid"":150376},{""name"":""Muse née des vents"",""text"":""Vol (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée.)\nLes créatures ne peuvent pas vous attaquer à moins que leur contrôleur ne paie {2} pour chaque créature qu'il contrôle qui vous attaque."",""type"":""Créature : esprit"",""flavor"":""« Sa voix est celle de la justice, claire et implacable. » —Akroma, vengeresse angélique"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149993&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""f6598d2d-b57a-4093-87f3-da6bc207fda1"",""multiverseId"":149993},""multiverseid"":149993},{""name"":""Musa Eologena"",""text"":""Volare (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere.)\nLe creature non possono attaccarti a meno che il loro controllore non paghi {2} per ogni creatura che controlla che ti sta attaccando."",""type"":""Creatura — Spirito"",""flavor"":""\""La sua voce è la giustizia, chiara e implacabile.\"" —Akroma, vendicatrice angelica"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149162&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""b5ba52f3-b0a0-45e0-abb6-face2dfddd6a"",""multiverseId"":149162},""multiverseid"":149162},{""name"":""風生まれの詩神"",""text"":""飛行 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされない。）\nクリーチャーは、それのコントローラーが、そのプレイヤーがコントロールするあなたを攻撃している各クリーチャーにつき{2}を支払わないかぎり、あなたを攻撃できない。"",""type"":""クリーチャー — スピリット"",""flavor"":""彼女の声は、正義であり純潔であり冷酷なのだ。 ――復讐の天使アクローマ"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148396&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""6d6af7bb-b378-4c2d-91da-02d530d99ee5"",""multiverseId"":148396},""multiverseid"":148396},{""name"":""Musa Nascida do Vento"",""text"":""Voar (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)\nAs criaturas não podem atacá-lo, a menos que o controlador delas pague {2} para cada criatura controlada por ele que o estiver atacando."",""type"":""Criatura — Espírito"",""flavor"":""\""Sua voz é justiça, clara e implacável.\"" — Akroma, vingadora angelical"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149610&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""31f138fb-d0a4-4098-8a2e-ec01e4a68579"",""multiverseId"":149610},""multiverseid"":149610},{""name"":""Муза, Рожденная из Ветра"",""text"":""Полет (Это существо может быть заблокировано только существом с Полетом или Захватом.)\nСущества не могут атаковать вас, если контролирующий их игрок не заплатит {2} за каждое атакующее вас существо под его контролем."",""type"":""Существо — Дух"",""flavor"":""\""Ее голос это голос справедливости, четкий и беспощадный\"". — Акрома, ангел-мститель"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149227&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""6bd35a5a-6a3d-4250-adba-75334c62790e"",""multiverseId"":149227},""multiverseid"":149227},{""name"":""风生谬思"",""text"":""飞行（只有具飞行或延势异能的生物才能阻挡它。）\n除非攻击你的生物之操控者为每一个各支付{2}，否则生物不能攻击你。"",""type"":""生物～精怪"",""flavor"":""「她的声音蕴含正义，清晰而不息。」 ～雪仇天使爱若玛"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148013&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""a036b4f4-37d9-4828-b5d3-900b2f1d520c"",""multiverseId"":148013},""multiverseid"":148013}],""printings"":[""10E"",""C16"",""C21"",""CMD"",""DMR"",""LGN"",""MKC"",""ONC"",""PLST"",""VOC""],""originalText"":""Flying\nCreatures can't attack you unless their controller pays {2} for each creature he or she controls that's attacking you."",""originalType"":""Creature - Spirit"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""556e35e6-ad67-51c6-8580-eda71597a507""},{""name"":""Windborn Muse"",""manaCost"":""{3}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Spirit"",""types"":[""Creature""],""subtypes"":[""Spirit""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying\nCreatures can't attack you unless their controller pays {2} for each creature they control that's attacking you."",""flavor"":""\""Her voice is justice, clear and relentless.\""\n—Akroma, angelic avenger"",""artist"":""Adam Rex"",""number"":""60★"",""power"":""2"",""toughness"":""3"",""layout"":""normal"",""variations"":[""556e35e6-ad67-51c6-8580-eda71597a507""],""rulings"":[{""date"":""2022-12-08"",""text"":""Creatures may still attack planeswalkers you control without their controller paying a cost.""}],""printings"":[""10E"",""C16"",""C21"",""CMD"",""DMR"",""LGN"",""MKC"",""ONC"",""PLST"",""VOC""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""9e49c166-9728-5259-b3e9-758b3c1357c6""},{""name"":""Wrath of God"",""manaCost"":""{2}{W}{W}"",""cmc"":4.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Sorcery"",""types"":[""Sorcery""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Destroy all creatures. They can't be regenerated."",""artist"":""Kev Walker"",""number"":""61"",""layout"":""normal"",""multiverseid"":""129808"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129808&type=card"",""foreignNames"":[{""name"":""Zorn Gottes"",""text"":""Zerstöre alle Kreaturen. Sie können nicht regeneriert werden."",""type"":""Hexerei"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148780&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""cdfe5966-5407-43a4-9c15-4f85e38c8b06"",""multiverseId"":148780},""multiverseid"":148780},{""name"":""Ira de Dios"",""text"":""Destruye todas las criaturas. No pueden ser regeneradas."",""type"":""Conjuro"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150377&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""b5fb0dc6-c945-41bc-b1db-a75cc3cf0332"",""multiverseId"":150377},""multiverseid"":150377},{""name"":""Colère de Dieu"",""text"":""Détruisez toutes les créatures. Elles ne peuvent pas être régénérées."",""type"":""Rituel"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149994&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""736737f4-c448-4a62-9bc1-2b25477ba7db"",""multiverseId"":149994},""multiverseid"":149994},{""name"":""Ira di Dio"",""text"":""Distruggi tutte le creature. Non possono essere rigenerate."",""type"":""Stregoneria"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149163&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""ae74bfc5-1cd3-43c9-8143-bb33674782ce"",""multiverseId"":149163},""multiverseid"":149163},{""name"":""神の怒り"",""text"":""すべてのクリーチャーを破壊する。 それらは再生できない。"",""type"":""ソーサリー"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148397&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""579b0b98-d179-4101-9b1e-2ccbacc58ee7"",""multiverseId"":148397},""multiverseid"":148397},{""name"":""Cólera de Deus"",""text"":""Destrua todas as criaturas. Elas não podem ser regeneradas."",""type"":""Feitiço"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149611&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""e642d885-02c6-4549-b433-334d2fe497be"",""multiverseId"":149611},""multiverseid"":149611},{""name"":""Гнев Божий"",""text"":""Уничтожьте все существа. Они не могут быть регенерированы."",""type"":""Волшебство"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149228&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""f2184d66-0fde-43ae-9130-fcac34e4f1cc"",""multiverseId"":149228},""multiverseid"":149228},{""name"":""神之愤怒"",""text"":""消灭所有生物。 它们不能重生。"",""type"":""法术"",""flavor"":null,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148014&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""5d8924d0-8c5d-45ce-9ef6-64c12dcde136"",""multiverseId"":148014},""multiverseid"":148014}],""printings"":[""10E"",""2ED"",""2XM"",""30A"",""3ED"",""4BB"",""4ED"",""5ED"",""6ED"",""7ED"",""8ED"",""9ED"",""AKR"",""BRB"",""C13"",""CED"",""CEI"",""CMM"",""DMR"",""EMA"",""FBB"",""LEA"",""LEB"",""MP2"",""P07"",""PLST"",""POR"",""PRM"",""PTC"",""PZ1"",""SLD"",""SUM"",""V14"",""WC00"",""WC03"",""WC04""],""originalText"":""Destroy all creatures. They can't be regenerated."",""originalType"":""Sorcery"",""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""3c0d065a-4aa4-55f4-bc11-bd326d23e5a6""},{""name"":""Youthful Knight"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Knight"",""types"":[""Creature""],""subtypes"":[""Human"",""Knight""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""First strike"",""flavor"":""Idealism fits him better than his armor."",""artist"":""Rebecca Guay"",""number"":""62"",""power"":""2"",""toughness"":""1"",""layout"":""normal"",""multiverseid"":""129790"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129790&type=card"",""variations"":[""bfed2fdd-d029-545f-88f7-5ab45446cf27""],""foreignNames"":[{""name"":""Jugendlicher Ritter"",""text"":""Erstschlag (Diese Kreatur fügt Kampfschaden vor Kreaturen ohne Erstschlag zu.)"",""type"":""Kreatur — Mensch, Ritter"",""flavor"":""Idealismus steht ihm besser als eine Rüstung."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148784&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""e77d6430-b55b-4bcd-8c46-9bb22600c74e"",""multiverseId"":148784},""multiverseid"":148784},{""name"":""Caballero joven"",""text"":""Daña primero. (Esta criatura hace daño de combate antes que las criaturas sin la habilidad de dañar primero.)"",""type"":""Criatura — Caballero humano"",""flavor"":""El idealismo le queda mejor que su armadura."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150378&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""11dde5e5-9125-4775-b0d9-1f7dcb8e817f"",""multiverseId"":150378},""multiverseid"":150378},{""name"":""Jeune chevalier"",""text"":""Initiative (Cette créature inflige des blessures de combat avant les créatures sans l'initiative.)"",""type"":""Créature : humain et chevalier"",""flavor"":""L'idéalisme lui va mieux que son armure."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149995&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""dbe2ff6c-0b86-4b12-ae12-cfa08051635e"",""multiverseId"":149995},""multiverseid"":149995},{""name"":""Cavaliere Giovane"",""text"":""Attacco improvviso (Questa creatura infligge danno da combattimento prima delle creature senza attacco improvviso.)"",""type"":""Creatura — Cavaliere Umano"",""flavor"":""Calza meglio l'idealismo della sua armatura."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149167&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""3a148bcd-c30b-4178-a0a7-934fc131d8e0"",""multiverseId"":149167},""multiverseid"":149167},{""name"":""若年の騎士"",""text"":""先制攻撃 （このクリーチャーは先制攻撃を持たないクリーチャーよりも先に戦闘ダメージを与える。）"",""type"":""クリーチャー — 人間・騎士"",""flavor"":""理想は鎧よりも彼の身体に合う。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148401&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""e65e2002-8cbe-4d8b-b5d0-cd31b04e951c"",""multiverseId"":148401},""multiverseid"":148401},{""name"":""Jovem Cavaleiro"",""text"":""Iniciativa (Esta criatura causa dano de combate antes de criaturas sem a habilidade de iniciativa.)"",""type"":""Criatura — Humano Cavaleiro"",""flavor"":""Seu idealismo combina mais com ele que sua armadura."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149612&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""41617c05-9fb3-41f9-8351-90836096ecb6"",""multiverseId"":149612},""multiverseid"":149612},{""name"":""Юный Рыцарь"",""text"":""Первый удар (Это существо наносит боевые повреждения раньше существ без Первого удара.)"",""type"":""Существо — Человек Рыцарь"",""flavor"":""Идеализм идет ему больше, чем боевые доспехи."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149229&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""5f69205b-2553-4d6d-8fa7-66f19e6582f3"",""multiverseId"":149229},""multiverseid"":149229},{""name"":""青年骑士"",""text"":""先攻（此生物会比不具先攻异能的生物提前造成战斗伤害。）"",""type"":""生物～人类／骑士"",""flavor"":""胸中理想胜过身外铠甲。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148018&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""edbf64ed-9aa1-457a-93f0-6e875943769c"",""multiverseId"":148018},""multiverseid"":148018}],""printings"":[""10E"",""ATH"",""E01"",""ELD"",""MM3"",""PLST"",""PS11"",""STH"",""TPR""],""originalText"":""First strike"",""originalType"":""Creature - Human Knight"",""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""f4937944-9439-5887-85e0-fc3705243da8""},{""name"":""Youthful Knight"",""manaCost"":""{1}{W}"",""cmc"":2.0,""colors"":[""W""],""colorIdentity"":[""W""],""type"":""Creature — Human Knight"",""types"":[""Creature""],""subtypes"":[""Human"",""Knight""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""First strike"",""flavor"":""Idealism fits him better than his armor."",""artist"":""Rebecca Guay"",""number"":""62★"",""power"":""2"",""toughness"":""1"",""layout"":""normal"",""variations"":[""f4937944-9439-5887-85e0-fc3705243da8""],""printings"":[""10E"",""ATH"",""E01"",""ELD"",""MM3"",""PLST"",""PS11"",""STH"",""TPR""],""legalities"":[{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""bfed2fdd-d029-545f-88f7-5ab45446cf27""},{""name"":""Academy Researchers"",""manaCost"":""{1}{U}{U}"",""cmc"":3.0,""colors"":[""U""],""colorIdentity"":[""U""],""type"":""Creature — Human Wizard"",""types"":[""Creature""],""subtypes"":[""Human"",""Wizard""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""When Academy Researchers enters the battlefield, you may put an Aura card from your hand onto the battlefield attached to Academy Researchers."",""flavor"":""They brandish their latest theories as warriors would wield weapons."",""artist"":""Stephen Daniele"",""number"":""63"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""132072"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=132072&type=card"",""rulings"":[{""date"":""2007-07-15"",""text"":""You can't put an Aura card from your hand onto the battlefield this way if that Aura can't legally enchant Academy Researchers. For example, you can't put an Aura with \""enchant land\"" or \""enchant green creature\"" onto the battlefield attached to Academy Researchers (unless Academy Researchers somehow turned into a land or a green creature before the ability resolved).""}],""foreignNames"":[{""name"":""Forscher der Akademie"",""text"":""Wenn der Forscher der Akademie ins Spiel kommt, kannst du eine Aurakarte aus deiner Hand an den Forscher der Akademie angelegt ins Spiel bringen."",""type"":""Kreatur — Mensch, Zauberer"",""flavor"":""Sie werfen mit Theorien um sich wie Meuchler mit Wurfmessern."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148403&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""20e45511-877c-4ec5-8c8b-2c61e32fb8a1"",""multiverseId"":148403},""multiverseid"":148403},{""name"":""Investigadores de la Academia"",""text"":""Cuando los Investigadores de la Academia entren en juego, puedes poner en juego una carta de aura de tu mano anexada a los Investigadores de la Academia."",""type"":""Criatura — Hechicero humano"",""flavor"":""Esgrimen sus últimas teorías como los guerreros harían con sus armas."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150379&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""dcbe7e97-2f65-4711-9302-87bbb4edc048"",""multiverseId"":150379},""multiverseid"":150379},{""name"":""Chercheurs de l'académie"",""text"":""Quand les Chercheurs de l'académie arrivent en jeu, vous pouvez mettre en jeu, attachée aux Chercheurs de l'académie, une carte d'aura de votre main."",""type"":""Créature : humain et sorcier"",""flavor"":""Ils agitent leurs nouvelles théories comme des guerriers brandiraient leurs armes."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149996&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""dcd499b4-8c0a-4ac6-8ee6-33fff8a01743"",""multiverseId"":149996},""multiverseid"":149996},{""name"":""Ricercatori dell'Accademia"",""text"":""Quando i Ricercatori dell'Accademia entrano in gioco, puoi mettere in gioco una carta Aura dalla tua mano assegnata ai Ricercatori dell'Accademia."",""type"":""Creatura — Mago Umano"",""flavor"":""Brandiscono le loro più recenti teorie come i guerrieri impugnerebbero le armi."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148786&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""acdc3ee9-8063-488d-af61-d15dfaf98336"",""multiverseId"":148786},""multiverseid"":148786},{""name"":""アカデミーの研究者"",""text"":""アカデミーの研究者が場に出たとき、あなたは自分の手札にあるオーラ･カードを１枚、アカデミーの研究者につけられた状態で場に出してもよい。"",""type"":""クリーチャー — 人間・ウィザード"",""flavor"":""彼らが最新の学説を振りかざすさまは、まるで戦士の武器のようだ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148020&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""9b3fbd0b-1eca-412b-906f-f463403fa67f"",""multiverseId"":148020},""multiverseid"":148020},{""name"":""Pesquisadores da Academia"",""text"":""Quando Pesquisadores da Academia entra em jogo, você pode colocar um card de Aura de sua mão em jogo anexado a Pesquisadores da Academia."",""type"":""Criatura — Humano Mago"",""flavor"":""Eles brandiam suas últimas teorias como os guerreiros fariam com suas armas."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149613&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""f867bc43-b841-4a27-b0ea-fe804c16a011"",""multiverseId"":149613},""multiverseid"":149613},{""name"":""Исследователи из Академии"",""text"":""Когда Исследователи из Академии входят в игру, вы можете положить карту Ауры из вашей руки в игру прикрепленной к Исследователям из Академии."",""type"":""Существо — Человек Чародей"",""flavor"":""Они угрожающее размахивают своими последними теориями, словно воины, потрясающие оружием."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149230&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""14ff751b-43b8-4aaa-b60c-6b942e253a54"",""multiverseId"":149230},""multiverseid"":149230},{""name"":""学院研究员"",""text"":""当学院研究员进场时，你可以将一张灵气牌从你手上放置进场，并结附在学院研究员上。"",""type"":""生物～人类／法术师"",""flavor"":""他们炫耀最新理论的模样有如战士挥舞刀剑。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147637&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""58e46c54-8673-4ab2-b726-b026c96a3ea4"",""multiverseId"":147637},""multiverseid"":147637}],""printings"":[""10E"",""USG""],""originalText"":""When Academy Researchers comes into play, you may put an Aura card from your hand into play attached to Academy Researchers."",""originalType"":""Creature - Human Wizard"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""b8a68840-4044-52c0-a14e-0a1c630ba42c""},{""name"":""Air Elemental"",""manaCost"":""{3}{U}{U}"",""cmc"":5.0,""colors"":[""U""],""colorIdentity"":[""U""],""type"":""Creature — Elemental"",""types"":[""Creature""],""subtypes"":[""Elemental""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying"",""flavor"":""\""The East Wind, an interloper in the dominions of Westerly Weather, is an impassive-faced tyrant with a sharp poniard held behind his back for a treacherous stab.\""\n—Joseph Conrad, *The Mirror of the Sea*"",""artist"":""Kev Walker"",""number"":""64"",""power"":""4"",""toughness"":""4"",""layout"":""normal"",""multiverseid"":""129459"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129459&type=card"",""variations"":[""76d4f437-02e5-5f1c-a1d3-eddb55e8725d""],""foreignNames"":[{""name"":""Luftelementar"",""text"":""Fliegend (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)"",""type"":""Kreatur — Elementarwesen"",""flavor"":""„Der Ostwind ist ein Eindringling im Reich des Wetter des Westens; er ist ein Tyrann mit ungerührtem Blick, der hinter dem Rücken den Dolch schon bereit zum verräterischen Stoß hält.\"" —Joseph Conrad, Der Spiegel der See"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148408&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""2f3bded1-74e5-4f83-9b46-9998239c0557"",""multiverseId"":148408},""multiverseid"":148408},{""name"":""Elemental de aire"",""text"":""Vuela. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)"",""type"":""Criatura — Elemental"",""flavor"":""\""El viento del este, un entrometido en los dominios del clima del oeste, es un tirano impasible con una daga afilada guardada en su espalda lista para una estocada traicionera.\"" —Joseph Conrad, El espejo del mar"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150380&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""ab2b97f1-bb9f-464d-ba21-9b191c298792"",""multiverseId"":150380},""multiverseid"":150380},{""name"":""Élémental d'air"",""text"":""Vol (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée.)"",""type"":""Créature : élémental"",""flavor"":""« Le Vent d'Est, un intrus dans le domaine du Temps d'Ouest, est un tyran au visage impassible et qui dissimule derrière son dos une dague acérée, afin de frapper en traître. » —Joseph Conrad, Le Miroir de la mer"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149997&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""5d69d648-3d83-493a-8d0d-a023ffe042e7"",""multiverseId"":149997},""multiverseid"":149997},{""name"":""Elementale dell'Aria"",""text"":""Volare (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere.)"",""type"":""Creatura — Elementale"",""flavor"":""\""Il Vento dell'Est, un intruso nei domini del Clima di Ponente, è un tiranno dal volto impassibile, con un pugnale affilato tenuto dietro la schiena, pronto per una stilettata a tradimento.\"" —Joseph Conrad, Lo Specchio del Mare"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148791&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""1aeab2d6-b609-4f10-aabd-17e5b5f85961"",""multiverseId"":148791},""multiverseid"":148791},{""name"":""大気の精霊"",""text"":""飛行 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされない。）"",""type"":""クリーチャー — エレメンタル"",""flavor"":""ウェスタリーの気候の支配の中に潜り込む東風は無感情な暴君で、鋭い短剣を背中に隠し、不意の一撃を狙っている。 ――ジョゼフ・コンラッド、「海の鏡」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148025&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""821a24c7-934e-48e1-b2b6-2d288da9ab8f"",""multiverseId"":148025},""multiverseid"":148025},{""name"":""Elemental do Ar"",""text"":""Voar (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)"",""type"":""Criatura — Elemental"",""flavor"":""\""O vento oeste, um intruso nos domínios do tempo ocidental, é um tirano impassível segurando um afiado punhal oculto às costas para um golpe traiçoeiro.\"" — Joseph Conrad, Espelho do Mar"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149614&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""0f06bd31-fb45-4fc5-95be-79535118c252"",""multiverseId"":149614},""multiverseid"":149614},{""name"":""Элементаль Воздуха"",""text"":""Полет (Это существо может быть заблокировано только существом с Полетом или Захватом.)"",""type"":""Существо — Элементаль"",""flavor"":""\""А Восточный Ветер, делающий набеги на владения Западного, тот бесстрастный тиран и держит за спиной острый кинжал, готовясь нанести предательский удар\"". — Джозеф Конрад, Зеркало морей"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149231&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""844c2f4e-57cf-4bcd-9c9b-8ffffc2b3c6e"",""multiverseId"":149231},""multiverseid"":149231},{""name"":""大气元素"",""text"":""飞行（只有具飞行或延势异能的生物才能阻挡它。）"",""type"":""生物～元素"",""flavor"":""「东风闯入西风时令统治区，犹如背藏利刃、面容冷漠的暴君，准备要背节行刺。」 ～约瑟夫·康拉德，《如镜的大海》"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147642&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""7075179d-df9b-45a1-b19f-f48a8d3f5f00"",""multiverseId"":147642},""multiverseid"":147642}],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""5ED"",""6ED"",""7ED"",""8ED"",""9ED"",""ANB"",""BRB"",""BTD"",""CED"",""CEI"",""DD2"",""DPA"",""FBB"",""GNT"",""JVC"",""LEA"",""LEB"",""M10"",""M19"",""M20"",""ME4"",""P02"",""PLST"",""PS11"",""S99"",""SUM"",""W17"",""XLN""],""originalText"":""Flying"",""originalType"":""Creature - Elemental"",""legalities"":[{""format"":""Alchemy"",""legality"":""Legal""},{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""76ddd7f5-1a84-55d5-98f5-4883c217e0d8""},{""name"":""Air Elemental"",""manaCost"":""{3}{U}{U}"",""cmc"":5.0,""colors"":[""U""],""colorIdentity"":[""U""],""type"":""Creature — Elemental"",""types"":[""Creature""],""subtypes"":[""Elemental""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying"",""flavor"":""\""The East Wind, an interloper in the dominions of Westerly Weather, is an impassive-faced tyrant with a sharp poniard held behind his back for a treacherous stab.\""\n—Joseph Conrad, *The Mirror of the Sea*"",""artist"":""Kev Walker"",""number"":""64★"",""power"":""4"",""toughness"":""4"",""layout"":""normal"",""variations"":[""76ddd7f5-1a84-55d5-98f5-4883c217e0d8""],""printings"":[""10E"",""2ED"",""30A"",""3ED"",""4BB"",""4ED"",""5ED"",""6ED"",""7ED"",""8ED"",""9ED"",""ANB"",""BRB"",""BTD"",""CED"",""CEI"",""DD2"",""DPA"",""FBB"",""GNT"",""JVC"",""LEA"",""LEB"",""M10"",""M19"",""M20"",""ME4"",""P02"",""PLST"",""PS11"",""S99"",""SUM"",""W17"",""XLN""],""legalities"":[{""format"":""Alchemy"",""legality"":""Legal""},{""format"":""Brawl"",""legality"":""Legal""},{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Explorer"",""legality"":""Legal""},{""format"":""Gladiator"",""legality"":""Legal""},{""format"":""Historic"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Restricted""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Pioneer"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Timeless"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""76d4f437-02e5-5f1c-a1d3-eddb55e8725d""},{""name"":""Ambassador Laquatus"",""manaCost"":""{1}{U}{U}"",""cmc"":3.0,""colors"":[""U""],""colorIdentity"":[""U""],""type"":""Legendary Creature — Merfolk Wizard"",""supertypes"":[""Legendary""],""types"":[""Creature""],""subtypes"":[""Merfolk"",""Wizard""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{3}: Target player mills three cards."",""flavor"":""\""Life is a game. The only thing that matters is whether you're a pawn or a player.\"""",""artist"":""Jim Murray"",""number"":""65"",""power"":""1"",""toughness"":""3"",""layout"":""normal"",""multiverseid"":""129913"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=129913&type=card"",""foreignNames"":[{""name"":""Botschafter Laquatus"",""text"":""{3}: Ein Spieler deiner Wahl legt die obersten drei Karten seiner Bibliothek auf seinen Friedhof."",""type"":""Legendäre Kreatur — Meervolk, Zauberer"",""flavor"":""„Das ganze Leben ist ein Spiel. Nur eines interessiert: ob du eine Spielfigur oder ein Spieler bist.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148409&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""7909c404-a68d-4f6a-a1ad-1bf50aa57183"",""multiverseId"":148409},""multiverseid"":148409},{""name"":""Embajador Laquatus"",""text"":""{3}: El jugador objetivo pone las tres primeras cartas de su biblioteca en su cementerio."",""type"":""Criatura legendaria — Hechicero tritón"",""flavor"":""\""La vida es un juego. Lo único que importa es si eres un peón o un jugador.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150381&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""acb08bf3-9370-45ce-9e4e-72195e9daf7e"",""multiverseId"":150381},""multiverseid"":150381},{""name"":""Ambassadeur Laquatus"",""text"":""{3} : Le joueur ciblé met les trois cartes du dessus de sa bibliothèque dans son cimetière."",""type"":""Créature légendaire : ondin et sorcier"",""flavor"":""« La vie est un jeu. Tout ce qui importe, c'est de savoir ce que vous êtes : un pion ou un joueur. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149998&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""0cf8c772-cb32-426d-b3b8-67e8aa2aef23"",""multiverseId"":149998},""multiverseid"":149998},{""name"":""Ambasciatore Laquatus"",""text"":""{3}: Un giocatore bersaglio mette nel suo cimitero le prime tre carte del suo grimorio."",""type"":""Creatura Leggendaria — Mago Tritone"",""flavor"":""\""La vita è un gioco. L'unica cosa che conta è se sei una pedina o un giocatore.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148792&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""572648d2-0d20-41c9-99e6-68a74c56b092"",""multiverseId"":148792},""multiverseid"":148792},{""name"":""ラクァタス大使"",""text"":""{3}：プレイヤー１人を対象とする。そのプレイヤーは、自分のライブラリーの一番上のカード３枚を自分の墓地に置く。"",""type"":""伝説のクリーチャー — マーフォーク・ウィザード"",""flavor"":""人生はゲームさ。 重要なのは、君がプレイヤーなのか駒なのかってことだけだ。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148026&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""fca85117-25fc-40f5-8e83-fe692682d239"",""multiverseId"":148026},""multiverseid"":148026},{""name"":""Embaixador Laquatus"",""text"":""{3}: O jogador alvo coloca os três primeiros cards de seu próprio grimório em seu próprio cemitério."",""type"":""Criatura Lendária — Tritão Mago"",""flavor"":""\""A vida é um jogo. A única coisa que importa é se você é um peão ou um jogador.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149615&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""4bca57d2-15d3-4fbe-9300-aec19ac4b29b"",""multiverseId"":149615},""multiverseid"":149615},{""name"":""Посол Лакватус"",""text"":""{3}: Целевой игрок кладет три верхних карты своей библиотеки на свое кладбище."",""type"":""Легендарное Существо — Мерфолк Чародей"",""flavor"":""\""Жизнь игра. Главное в ней это кем вы являетесь, пешкой или ферзем\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149232&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""acbc8987-b28a-44d0-8825-6a574625741b"",""multiverseId"":149232},""multiverseid"":149232},{""name"":""大使拉夸塔"",""text"":""{3}：目标牌手将其牌库顶的三张牌置入其坟墓场。"",""type"":""传奇生物～人鱼／法术师"",""flavor"":""「生命有如一盘棋。 重点在于你是棋手或棋子。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147643&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""51272d8a-c0a1-44ce-a18c-ffe73fcccc56"",""multiverseId"":147643},""multiverseid"":147643}],""printings"":[""10E"",""TOR""],""originalText"":""{3}: Target player puts the top three cards of his or her library into his or her graveyard."",""originalType"":""Legendary Creature - Merfolk Wizard"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""29723d8d-34c9-5009-8b79-2c679e35f674""},{""name"":""Arcanis the Omnipotent"",""manaCost"":""{3}{U}{U}{U}"",""cmc"":6.0,""colors"":[""U""],""colorIdentity"":[""U""],""type"":""Legendary Creature — Wizard"",""supertypes"":[""Legendary""],""types"":[""Creature""],""subtypes"":[""Wizard""],""rarity"":""Rare"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""{T}: Draw three cards.\n{2}{U}{U}: Return Arcanis the Omnipotent to its owner's hand."",""flavor"":""\""Do not concern yourself with my origin, my race, or my ancestry. Seek my record in the pits, and then make your wager.\"""",""artist"":""Justin Sweet"",""number"":""66"",""power"":""3"",""toughness"":""4"",""layout"":""normal"",""multiverseid"":""106426"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=106426&type=card"",""rulings"":[{""date"":""2022-12-08"",""text"":""Arcanis’s last ability can be activated only while it’s on the battlefield.""}],""foreignNames"":[{""name"":""Arcanis der Allgewaltige"",""text"":""{T}: Ziehe drei Karten.\n{2}{U}{U}: Bringe Arcanis den Allgewaltigen auf die Hand seines Besitzers zurück."",""type"":""Legendäre Kreatur — Zauberer"",""flavor"":""„Meine Herkunft, meine Rasse und meine Vorfahren brauchen dich nicht zu interessieren. Schau dir die Anzahl meiner Siege in den Gruben an und mach dann deinen Einsatz.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148418&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""39206222-98e0-4d3d-9d2e-475777e9f231"",""multiverseId"":148418},""multiverseid"":148418},{""name"":""Arcanis el omnipotente"",""text"":""{T}: Roba tres cartas.\n{2}{U}{U}: Regresa a Arcanis el omnipotente a la mano de su propietario."",""type"":""Criatura legendaria — Hechicero"",""flavor"":""\""No te preocupes por mi origen, mi raza, o mis ancestros. Fíjate mi historial en los fosos y haz tu apuesta.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150382&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""d0813574-e849-4efd-8991-8b38a4f85f47"",""multiverseId"":150382},""multiverseid"":150382},{""name"":""Arcanis l'omnipotent"",""text"":""{T} : Piochez trois cartes.\n{2}{U}{U} : Renvoyez Arcanis l'omnipotent dans la main de son propriétaire."",""type"":""Créature légendaire : sorcier"",""flavor"":""« Ne vous attardez pas sur mon origine, ma race ou mes ancêtres. Comptez mes victoires dans les sangrahbas, et faites ensuite votre pari. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149999&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""cb8a6130-ba64-4c0b-bbb5-3666db8670f2"",""multiverseId"":149999},""multiverseid"":149999},{""name"":""Arcanis l'Onnipotente"",""text"":""{T}: Pesca tre carte.\n{2}{U}{U}: Fai tornare Arcanis l'Onnipotente in mano al suo proprietario."",""type"":""Creatura Leggendaria — Mago"",""flavor"":""\""Non preoccuparti della mia origine, della mia razza o del mio lignaggio. Chiedi i miei risultati nell'arena e poi fai la tua scommessa.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148801&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""6838e1d8-716e-46a4-9d7d-bada7a0d5dc9"",""multiverseId"":148801},""multiverseid"":148801},{""name"":""全能なる者アルカニス"",""text"":""{T}：カードを３枚引く。\n{2}{U}{U}：全能なる者アルカニスをオーナーの手札に戻す。"",""type"":""伝説のクリーチャー — ウィザード"",""flavor"":""私の出自とか、種族とか、祖先とかを気にするんじゃない。 ピットでの戦績を調べてから賭けるんだな。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148035&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""6eb5c2da-2bc8-4ca9-a37a-3db8e4649cc9"",""multiverseId"":148035},""multiverseid"":148035},{""name"":""Arcanis, o Onipotente"",""text"":""{T}: Compre três cards.\n{2}{U}{U}: Devolva Arcanis, o Onipotente, para a mão de seu dono."",""type"":""Criatura Lendária — Mago"",""flavor"":""\""Não se preocupe com a minha origem, minha raça ou meus antepassados. Busque minha reputação nas liças e então faça sua aposta.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149616&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""c6a929f1-639f-4685-b390-2a5332e05df6"",""multiverseId"":149616},""multiverseid"":149616},{""name"":""Арканис Всемогущий"",""text"":""{T}: Возьмите три карты.\n{2}{U}{U}: Верните Арканиса Всемогущего в руку его владельца."",""type"":""Легендарное Существо — Чародей"",""flavor"":""\""Не задавайтесь вопросами о том, кто я, откуда и какой крови. Проверьте мою репутацию бойца, и делайте ставки\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149233&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""fdf4ff90-4725-4f89-96c1-b0fa682914ba"",""multiverseId"":149233},""multiverseid"":149233},{""name"":""全能的阿卡尼思"",""text"":""{T}：抓三张牌。\n{2}{U}{U}：将全能的阿卡尼思移回其拥有者手上。"",""type"":""传奇生物～法术师"",""flavor"":""「甭费心打探我的来历宗族或八代远祖。 瞧瞧我在死斗坑的战绩，就可以下赌注了。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147652&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""e525ae35-9070-4e13-92c5-4633ad1986b3"",""multiverseId"":147652},""multiverseid"":147652}],""printings"":[""10E"",""C17"",""DDN"",""DMR"",""EMA"",""ONS"",""PRM"",""SLD"",""WC03""],""originalText"":""{T}: Draw three cards.\n{2}{U}{U}: Return Arcanis the Omnipotent to its owner's hand."",""originalType"":""Legendary Creature - Wizard"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""6f08f7f6-ecfb-584f-ae7a-c6e4d9c4da35""},{""name"":""Aura Graft"",""manaCost"":""{1}{U}"",""cmc"":2.0,""colors"":[""U""],""colorIdentity"":[""U""],""type"":""Instant"",""types"":[""Instant""],""rarity"":""Uncommon"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Gain control of target Aura that's attached to a permanent. Attach it to another permanent it can enchant."",""flavor"":""\""It's not really stealing. It's more like extended borrowing.\"""",""artist"":""Ray Lago"",""number"":""67"",""layout"":""normal"",""multiverseid"":""130976"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130976&type=card"",""rulings"":[{""date"":""2004-10-04"",""text"":""If there is no legal place to move the enchantment, then it doesn’t move but you still control it.""},{""date"":""2007-07-15"",""text"":""Aura Graft’s effect has no duration. You’ll retain control of the Aura until the game ends, the Aura leaves the battlefield, or an effect causes someone else to gain control of the Aura.""},{""date"":""2007-07-15"",""text"":""You can target an Aura you already control just to move that Aura to a new permanent.""}],""foreignNames"":[{""name"":""Aurenübertragung"",""text"":""Übernimm die Kontrolle über eine Aura deiner Wahl, die an eine bleibende Karte angelegt ist. Lege sie an eine andere bleibende Karte an, die sie verzaubern kann."",""type"":""Spontanzauber"",""flavor"":""„Das ist nicht wirklich Stehlen. Es ist eher erweitertes Ausleihen.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148421&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""31b0932a-bda7-4fc1-93d1-16a1a6acc19f"",""multiverseId"":148421},""multiverseid"":148421},{""name"":""Robo de aura"",""text"":""Gana el control del aura objetivo que está anexada a un permanente. Anéxala a otro permanente que pueda encantar."",""type"":""Instantáneo"",""flavor"":""\""No es robar realmente. Es como tomar prestado por mucho tiempo.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150383&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""e7dc5c23-7da1-41e9-b4e7-b2d3f599a5cb"",""multiverseId"":150383},""multiverseid"":150383},{""name"":""Greffe d'aura"",""text"":""Acquérez le contrôle de l'aura ciblée qui est attachée à un permanent. Attachez-la à un autre permanent qu'elle peut enchanter."",""type"":""Éphémère"",""flavor"":""« Ce n'est pas du vol, c'est juste un emprunt de très longue durée. »"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150000&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""e141ca1d-5087-4c4d-b8b3-f4bda1300b24"",""multiverseId"":150000},""multiverseid"":150000},{""name"":""Innesto dell'Aura"",""text"":""Prendi il controllo di un'Aura bersaglio che è assegnata a un permanente. Assegnala a un altro permanente che può incantare."",""type"":""Istantaneo"",""flavor"":""\""Non è proprio un furto. Diciamo che la prendo in prestito a tempo indeterminato.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148804&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""2163e1d9-ae31-4bbe-b2e2-a0e174489031"",""multiverseId"":148804},""multiverseid"":148804},{""name"":""オーラの移植"",""text"":""パーマネントにつけられているオーラ１つを対象とし、それのコントロールを得る。 それをそれがエンチャントすることのできる他のパーマネントにつける。"",""type"":""インスタント"",""flavor"":""盗むのとはちょっと違うよ。 借用の延長みたいなもんだね。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148038&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""3e974544-f660-42ae-aefd-5822578941ad"",""multiverseId"":148038},""multiverseid"":148038},{""name"":""Enxerto de Aura"",""text"":""Ganhe o controle da Aura alvo anexada a uma permanente. Anexe-a a outra permanente que ela possa encantar."",""type"":""Mágica Instantânea"",""flavor"":""\""Não é exatamente um roubo. É mais como um empréstimo a longo prazo.\"""",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149617&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""a264e216-f021-4ca0-af13-eef79500acbf"",""multiverseId"":149617},""multiverseid"":149617},{""name"":""Прививка Ауры"",""text"":""Получите контроль над целевой Аурой, прикрепленной к перманенту. Прикрепите ее к другому перманенту, который она может зачаровать."",""type"":""Мгновенное заклинание"",""flavor"":""\""Это не совсем воровство. Скорее, это можно назвать бессрочным заимствованием\""."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149234&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""31b9df53-1064-45d6-8a7e-aae86fd4b2c7"",""multiverseId"":149234},""multiverseid"":149234},{""name"":""灵气移植"",""text"":""获得目标已结附于永久物上的灵气之操控权。 将它结附于另一个它能结附的永久物上。"",""type"":""瞬间"",""flavor"":""「这不算偷，而更近于广义的借用。」"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147655&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""d941a0cc-2bd3-42f0-96e7-5249db07c6d1"",""multiverseId"":147655},""multiverseid"":147655}],""printings"":[""10E"",""ODY""],""originalText"":""Gain control of target Aura that's attached to a permanent. Attach it to another permanent it can enchant."",""originalType"":""Instant"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""aeb1b742-645c-5dab-ba9b-35d51649e074""},{""name"":""Aven Fisher"",""manaCost"":""{3}{U}"",""cmc"":4.0,""colors"":[""U""],""colorIdentity"":[""U""],""type"":""Creature — Bird Soldier"",""types"":[""Creature""],""subtypes"":[""Bird"",""Soldier""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying (This creature can't be blocked except by creatures with flying or reach.)\nWhen Aven Fisher dies, you may draw a card."",""flavor"":""The same spears that catch their food today will defend their homes tomorrow."",""artist"":""Christopher Moeller"",""number"":""68"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":""130985"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=130985&type=card"",""variations"":[""e4b4684b-8e14-5fcf-9286-497b7afd5b3a""],""foreignNames"":[{""name"":""Avior-Fischer"",""text"":""Fliegend (Diese Kreatur kann außer von fliegenden Kreaturen und Kreaturen mit Reichweite nicht geblockt werden.)\nWenn der Avior-Fischer aus dem Spiel auf einen Friedhof gelegt wird, kannst du eine Karte ziehen."",""type"":""Kreatur — Vogel, Soldat"",""flavor"":""Mit denselben Speeren, mit denen sie heute ihr Essen aufspießen, werden sie morgen ihre Häuser verteidigen."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148425&type=card"",""language"":""German"",""identifiers"":{""scryfallId"":""c9d2027f-b9e0-4273-8a41-1fc5f22d5ed1"",""multiverseId"":148425},""multiverseid"":148425},{""name"":""Pescador aven"",""text"":""Vuela. (Esta criatura no puede ser bloqueada excepto por criaturas que tengan la habilidad de volar o alcance.)\nCuando el Pescador aven vaya a un cementerio desde el juego, puedes robar una carta."",""type"":""Criatura — Soldado ave"",""flavor"":""Las mismas lanzas que usan los aven para atrapar su comida hoy, defenderán sus hogares mañana."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150384&type=card"",""language"":""Spanish"",""identifiers"":{""scryfallId"":""6563dcc8-75cf-489d-8077-eaba50e60dee"",""multiverseId"":150384},""multiverseid"":150384},{""name"":""Pêcheur avemain"",""text"":""Vol (Cette créature ne peut être bloquée que par des créatures avec le vol ou la portée.)\nQuand le Pêcheur avemain est mis dans un cimetière depuis le jeu, vous pouvez piocher une carte."",""type"":""Créature : oiseau et soldat"",""flavor"":""Ils défendront demain leur maison avec cette même lance qui leur sert aujourd'hui à attraper leur nourriture."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=150001&type=card"",""language"":""French"",""identifiers"":{""scryfallId"":""532252f9-c640-465c-ab93-8965d5721624"",""multiverseId"":150001},""multiverseid"":150001},{""name"":""Pescatore Aviano"",""text"":""Volare (Questa creatura non può essere bloccata tranne che da creature con volare o raggiungere.)\nQuando il Pescatore Aviano viene messo in un cimitero dal gioco, puoi pescare una carta."",""type"":""Creatura — Uccello Soldato"",""flavor"":""Le stesse lance che catturano oggi il loro cibo difenderanno un domani le loro case."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148808&type=card"",""language"":""Italian"",""identifiers"":{""scryfallId"":""2911001b-302f-439e-a274-5076839c6630"",""multiverseId"":148808},""multiverseid"":148808},{""name"":""エイヴンの魚捕り"",""text"":""飛行 （このクリーチャーは飛行や到達を持たないクリーチャーによってブロックされない。）\nエイヴンの魚捕りが場からいずれかの墓地に置かれたとき、あなたはカードを１枚引いてもよい。"",""type"":""クリーチャー — 鳥・兵士"",""flavor"":""エイヴンが食べ物を捕るために使う槍は、いつでも身を守る武器になるだろう。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=148042&type=card"",""language"":""Japanese"",""identifiers"":{""scryfallId"":""5d817e57-eddd-4d96-a955-a19c2cb08cca"",""multiverseId"":148042},""multiverseid"":148042},{""name"":""Pescador Aviano"",""text"":""Voar (Esta criatura só pode ser bloqueada por criaturas com a habilidade de voar ou alcance.)\nQuando Pescador Aviano é colocado num cemitério vindo de jogo, você pode comprar um card."",""type"":""Criatura — Ave Soldado"",""flavor"":""As mesmas lanças que caçam sua comida hoje irão defender suas casas amanhã."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149618&type=card"",""language"":""Portuguese (Brazil)"",""identifiers"":{""scryfallId"":""cbc1ccd4-281c-4214-824e-90566605c8b2"",""multiverseId"":149618},""multiverseid"":149618},{""name"":""Воздушный Рыболов"",""text"":""Полет (Это существо может быть заблокировано только существом с Полетом или Захватом.)\nКогда Воздушный Рыболов попадает из игры на кладбище, вы можете взять карту."",""type"":""Существо — Птица Солдат"",""flavor"":""Те же копья, что сегодня добывают им пищу, завтра будут защищать их дома."",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=149235&type=card"",""language"":""Russian"",""identifiers"":{""scryfallId"":""37720404-4331-4b36-911a-558713aba880"",""multiverseId"":149235},""multiverseid"":149235},{""name"":""艾文渔人"",""text"":""飞行（只有具飞行或延势异能的生物才能阻挡它。）\n当艾文渔人从场上置入坟墓场时，你可以抓一张牌。"",""type"":""生物～鸟／士兵"",""flavor"":""手握同支长矛，猎捕今朝食物，保卫明日家园。"",""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=147659&type=card"",""language"":""Chinese Simplified"",""identifiers"":{""scryfallId"":""a7fbbfeb-9311-4d9e-9906-d1cf9f1e54a7"",""multiverseId"":147659},""multiverseid"":147659}],""printings"":[""10E"",""8ED"",""9ED"",""DMR"",""ODY""],""originalText"":""Flying\nWhen Aven Fisher is put into a graveyard from play, you may draw a card."",""originalType"":""Creature - Bird Soldier"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""758df5d5-992f-5d30-b6bc-87cb1a0788ab""},{""name"":""Aven Fisher"",""manaCost"":""{3}{U}"",""cmc"":4.0,""colors"":[""U""],""colorIdentity"":[""U""],""type"":""Creature — Bird Soldier"",""types"":[""Creature""],""subtypes"":[""Bird"",""Soldier""],""rarity"":""Common"",""set"":""10E"",""setName"":""Tenth Edition"",""text"":""Flying (This creature can't be blocked except by creatures with flying or reach.)\nWhen Aven Fisher dies, you may draw a card."",""flavor"":""The same spears that catch their food today will defend their homes tomorrow."",""artist"":""Christopher Moeller"",""number"":""68★"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""variations"":[""758df5d5-992f-5d30-b6bc-87cb1a0788ab""],""printings"":[""10E"",""8ED"",""9ED"",""DMR"",""ODY""],""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Duel"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Oathbreaker"",""legality"":""Legal""},{""format"":""Pauper"",""legality"":""Legal""},{""format"":""Paupercommander"",""legality"":""Legal""},{""format"":""Penny"",""legality"":""Legal""},{""format"":""Predh"",""legality"":""Legal""},{""format"":""Premodern"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""e4b4684b-8e14-5fcf-9286-497b7afd5b3a""}]}";


                var result = JsonSerializer.Deserialize<MTGApiResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        }



		private string LoadTestTwoCardVerwticies()
        {
            return @"[{""Lines"":[{""Text"":""Caustic Caterpillar"",""BoundingPolygon"":[{""X"":575,""Y"":3658},{""X"":560,""Y"":3047},{""X"":631,""Y"":3045},{""X"":644,""Y"":3657}],""Words"":[{""Text"":""Caustic"",""BoundingPolygon"":[{""X"":578,""Y"":3657},{""X"":570,""Y"":3431},{""X"":639,""Y"":3430},{""X"":644,""Y"":3657}],""Confidence"":0.994},{""Text"":""Caterpillar"",""BoundingPolygon"":[{""X"":569,""Y"":3410},{""X"":560,""Y"":3051},{""X"":632,""Y"":3049},{""X"":639,""Y"":3409}],""Confidence"":0.994}]},{""Text"":""Elvish Visionary"",""BoundingPolygon"":[{""X"":500,""Y"":1692},{""X"":500,""Y"":1154},{""X"":569,""Y"":1154},{""X"":568,""Y"":1692}],""Words"":[{""Text"":""Elvish"",""BoundingPolygon"":[{""X"":500,""Y"":1691},{""X"":500,""Y"":1508},{""X"":569,""Y"":1509},{""X"":569,""Y"":1693}],""Confidence"":0.993},{""Text"":""Visionary"",""BoundingPolygon"":[{""X"":500,""Y"":1478},{""X"":502,""Y"":1168},{""X"":569,""Y"":1170},{""X"":569,""Y"":1480}],""Confidence"":0.993}]},{""Text"":""1@"",""BoundingPolygon"":[{""X"":487,""Y"":651},{""X"":484,""Y"":523},{""X"":549,""Y"":522},{""X"":550,""Y"":653}],""Words"":[{""Text"":""1@"",""BoundingPolygon"":[{""X"":486,""Y"":639},{""X"":485,""Y"":554},{""X"":549,""Y"":553},{""X"":551,""Y"":638}],""Confidence"":0.606}]},{""Text"":""Creature - Insect"",""BoundingPolygon"":[{""X"":1594,""Y"":3664},{""X"":1586,""Y"":3161},{""X"":1643,""Y"":3159},{""X"":1651,""Y"":3663}],""Words"":[{""Text"":""Creature"",""BoundingPolygon"":[{""X"":1594,""Y"":3663},{""X"":1592,""Y"":3430},{""X"":1646,""Y"":3429},{""X"":1651,""Y"":3663}],""Confidence"":0.993},{""Text"":""-"",""BoundingPolygon"":[{""X"":1592,""Y"":3406},{""X"":1591,""Y"":3379},{""X"":1645,""Y"":3378},{""X"":1646,""Y"":3405}],""Confidence"":0.992},{""Text"":""Insect"",""BoundingPolygon"":[{""X"":1590,""Y"":3337},{""X"":1586,""Y"":3165},{""X"":1643,""Y"":3163},{""X"":1645,""Y"":3336}],""Confidence"":0.989}]},{""Text"":""Creature - Elf Shaman"",""BoundingPolygon"":[{""X"":1517,""Y"":1665},{""X"":1502,""Y"":1004},{""X"":1565,""Y"":1004},{""X"":1579,""Y"":1664}],""Words"":[{""Text"":""Creature"",""BoundingPolygon"":[{""X"":1519,""Y"":1665},{""X"":1512,""Y"":1422},{""X"":1575,""Y"":1419},{""X"":1578,""Y"":1660}],""Confidence"":0.993},{""Text"":""-"",""BoundingPolygon"":[{""X"":1511,""Y"":1403},{""X"":1510,""Y"":1367},{""X"":1574,""Y"":1365},{""X"":1574,""Y"":1400}],""Confidence"":0.965},{""Text"":""Elf"",""BoundingPolygon"":[{""X"":1509,""Y"":1328},{""X"":1507,""Y"":1250},{""X"":1571,""Y"":1249},{""X"":1573,""Y"":1326}],""Confidence"":0.993},{""Text"":""Shaman"",""BoundingPolygon"":[{""X"":1507,""Y"":1236},{""X"":1502,""Y"":1013},{""X"":1565,""Y"":1015},{""X"":1571,""Y"":1235}],""Confidence"":0.996}]},{""Text"":""14, Sacrifice Caustic Caterpillar:"",""BoundingPolygon"":[{""X"":1765,""Y"":3658},{""X"":1745,""Y"":2642},{""X"":1818,""Y"":2640},{""X"":1834,""Y"":3657}],""Words"":[{""Text"":""14,"",""BoundingPolygon"":[{""X"":1768,""Y"":3653},{""X"":1768,""Y"":3524},{""X"":1832,""Y"":3519},{""X"":1834,""Y"":3648}],""Confidence"":0.602},{""Text"":""Sacrifice"",""BoundingPolygon"":[{""X"":1768,""Y"":3509},{""X"":1765,""Y"":3257},{""X"":1828,""Y"":3253},{""X"":1832,""Y"":3504}],""Confidence"":0.994},{""Text"":""Caustic"",""BoundingPolygon"":[{""X"":1764,""Y"":3237},{""X"":1759,""Y"":3007},{""X"":1824,""Y"":3004},{""X"":1828,""Y"":3233}],""Confidence"":0.994},{""Text"":""Caterpillar:"",""BoundingPolygon"":[{""X"":1758,""Y"":2988},{""X"":1745,""Y"":2643},{""X"":1818,""Y"":2641},{""X"":1824,""Y"":2984}],""Confidence"":0.993}]},{""Text"":""When Elvish Visionary enters the"",""BoundingPolygon"":[{""X"":1718,""Y"":1665},{""X"":1698,""Y"":606},{""X"":1766,""Y"":604},{""X"":1789,""Y"":1663}],""Words"":[{""Text"":""When"",""BoundingPolygon"":[{""X"":1720,""Y"":1656},{""X"":1714,""Y"":1478},{""X"":1784,""Y"":1476},{""X"":1789,""Y"":1654}],""Confidence"":0.993},{""Text"":""Elvish"",""BoundingPolygon"":[{""X"":1713,""Y"":1444},{""X"":1709,""Y"":1266},{""X"":1778,""Y"":1264},{""X"":1783,""Y"":1442}],""Confidence"":0.995},{""Text"":""Visionary"",""BoundingPolygon"":[{""X"":1708,""Y"":1242},{""X"":1702,""Y"":946},{""X"":1771,""Y"":943},{""X"":1778,""Y"":1239}],""Confidence"":0.993},{""Text"":""enters"",""BoundingPolygon"":[{""X"":1702,""Y"":922},{""X"":1699,""Y"":730},{""X"":1767,""Y"":727},{""X"":1770,""Y"":918}],""Confidence"":0.995},{""Text"":""the"",""BoundingPolygon"":[{""X"":1699,""Y"":714},{""X"":1698,""Y"":617},{""X"":1765,""Y"":614},{""X"":1767,""Y"":711}],""Confidence"":0.999}]},{""Text"":""Destroy target artifact or enchantment."",""BoundingPolygon"":[{""X"":1842,""Y"":3668},{""X"":1815,""Y"":2501},{""X"":1881,""Y"":2501},{""X"":1909,""Y"":3667}],""Words"":[{""Text"":""Destroy"",""BoundingPolygon"":[{""X"":1842,""Y"":3668},{""X"":1839,""Y"":3436},{""X"":1904,""Y"":3436},{""X"":1908,""Y"":3668}],""Confidence"":0.994},{""Text"":""target"",""BoundingPolygon"":[{""X"":1838,""Y"":3412},{""X"":1835,""Y"":3236},{""X"":1900,""Y"":3236},{""X"":1903,""Y"":3412}],""Confidence"":0.993},{""Text"":""artifact"",""BoundingPolygon"":[{""X"":1835,""Y"":3221},{""X"":1830,""Y"":3005},{""X"":1894,""Y"":3005},{""X"":1899,""Y"":3221}],""Confidence"":0.993},{""Text"":""or"",""BoundingPolygon"":[{""X"":1830,""Y"":2991},{""X"":1828,""Y"":2921},{""X"":1892,""Y"":2921},{""X"":1894,""Y"":2991}],""Confidence"":0.999},{""Text"":""enchantment."",""BoundingPolygon"":[{""X"":1827,""Y"":2906},{""X"":1815,""Y"":2503},{""X"":1879,""Y"":2503},{""X"":1892,""Y"":2906}],""Confidence"":0.993}]},{""Text"":""battlefield, draw a card."",""BoundingPolygon"":[{""X"":1792,""Y"":1667},{""X"":1779,""Y"":909},{""X"":1847,""Y"":909},{""X"":1863,""Y"":1666}],""Words"":[{""Text"":""battlefield,"",""BoundingPolygon"":[{""X"":1792,""Y"":1658},{""X"":1787,""Y"":1313},{""X"":1852,""Y"":1310},{""X"":1863,""Y"":1653}],""Confidence"":0.994},{""Text"":""draw"",""BoundingPolygon"":[{""X"":1787,""Y"":1299},{""X"":1784,""Y"":1165},{""X"":1849,""Y"":1163},{""X"":1851,""Y"":1296}],""Confidence"":0.993},{""Text"":""a"",""BoundingPolygon"":[{""X"":1783,""Y"":1119},{""X"":1783,""Y"":1086},{""X"":1847,""Y"":1085},{""X"":1848,""Y"":1117}],""Confidence"":0.998},{""Text"":""card."",""BoundingPolygon"":[{""X"":1782,""Y"":1062},{""X"":1779,""Y"":909},{""X"":1845,""Y"":909},{""X"":1847,""Y"":1061}],""Confidence"":0.996}]},{""Text"":""\u0022The rare and beautiful butterflies inspire"",""BoundingPolygon"":[{""X"":1942,""Y"":3667},{""X"":1921,""Y"":2514},{""X"":1991,""Y"":2512},{""X"":2010,""Y"":3665}],""Words"":[{""Text"":""\u0022The"",""BoundingPolygon"":[{""X"":1943,""Y"":3663},{""X"":1941,""Y"":3519},{""X"":2008,""Y"":3517},{""X"":2009,""Y"":3661}],""Confidence"":0.976},{""Text"":""rare"",""BoundingPolygon"":[{""X"":1940,""Y"":3505},{""X"":1938,""Y"":3388},{""X"":2006,""Y"":3387},{""X"":2008,""Y"":3503}],""Confidence"":0.993},{""Text"":""and"",""BoundingPolygon"":[{""X"":1938,""Y"":3374},{""X"":1936,""Y"":3268},{""X"":2005,""Y"":3266},{""X"":2006,""Y"":3372}],""Confidence"":0.998},{""Text"":""beautiful"",""BoundingPolygon"":[{""X"":1935,""Y"":3248},{""X"":1930,""Y"":3001},{""X"":2000,""Y"":2999},{""X"":2004,""Y"":3246}],""Confidence"":0.971},{""Text"":""butterflies"",""BoundingPolygon"":[{""X"":1930,""Y"":2986},{""X"":1924,""Y"":2716},{""X"":1994,""Y"":2715},{""X"":2000,""Y"":2984}],""Confidence"":0.992},{""Text"":""inspire"",""BoundingPolygon"":[{""X"":1924,""Y"":2702},{""X"":1921,""Y"":2518},{""X"":1989,""Y"":2516},{""X"":1994,""Y"":2700}],""Confidence"":0.993}]},{""Text"":""\u0022From a tiny sprout, the greatest trees"",""BoundingPolygon"":[{""X"":1903,""Y"":1643},{""X"":1884,""Y"":548},{""X"":1949,""Y"":547},{""X"":1966,""Y"":1642}],""Words"":[{""Text"":""\u0022From"",""BoundingPolygon"":[{""X"":1903,""Y"":1644},{""X"":1900,""Y"":1480},{""X"":1965,""Y"":1478},{""X"":1965,""Y"":1641}],""Confidence"":0.978},{""Text"":""a"",""BoundingPolygon"":[{""X"":1899,""Y"":1427},{""X"":1898,""Y"":1395},{""X"":1965,""Y"":1393},{""X"":1965,""Y"":1425}],""Confidence"":0.994},{""Text"":""tiny"",""BoundingPolygon"":[{""X"":1898,""Y"":1371},{""X"":1896,""Y"":1259},{""X"":1964,""Y"":1258},{""X"":1965,""Y"":1370}],""Confidence"":0.992},{""Text"":""sprout,"",""BoundingPolygon"":[{""X"":1895,""Y"":1235},{""X"":1892,""Y"":1037},{""X"":1959,""Y"":1037},{""X"":1963,""Y"":1235}],""Confidence"":0.994},{""Text"":""the"",""BoundingPolygon"":[{""X"":1891,""Y"":1023},{""X"":1890,""Y"":931},{""X"":1956,""Y"":932},{""X"":1959,""Y"":1023}],""Confidence"":0.998},{""Text"":""greatest"",""BoundingPolygon"":[{""X"":1889,""Y"":912},{""X"":1886,""Y"":684},{""X"":1947,""Y"":685},{""X"":1956,""Y"":913}],""Confidence"":0.991},{""Text"":""trees"",""BoundingPolygon"":[{""X"":1885,""Y"":670},{""X"":1884,""Y"":548},{""X"":1941,""Y"":550},{""X"":1946,""Y"":671}],""Confidence"":0.993}]},{""Text"":""the design of our thopters. The larvae,"",""BoundingPolygon"":[{""X"":2012,""Y"":3670},{""X"":2002,""Y"":2606},{""X"":2074,""Y"":2605},{""X"":2084,""Y"":3669}],""Words"":[{""Text"":""the"",""BoundingPolygon"":[{""X"":2020,""Y"":3670},{""X"":2016,""Y"":3580},{""X"":2080,""Y"":3577},{""X"":2084,""Y"":3666}],""Confidence"":0.998},{""Text"":""design"",""BoundingPolygon"":[{""X"":2016,""Y"":3565},{""X"":2011,""Y"":3400},{""X"":2074,""Y"":3398},{""X"":2080,""Y"":3562}],""Confidence"":0.987},{""Text"":""of"",""BoundingPolygon"":[{""X"":2010,""Y"":3376},{""X"":2009,""Y"":3320},{""X"":2072,""Y"":3318},{""X"":2074,""Y"":3374}],""Confidence"":0.998},{""Text"":""our"",""BoundingPolygon"":[{""X"":2008,""Y"":3306},{""X"":2006,""Y"":3210},{""X"":2070,""Y"":3209},{""X"":2072,""Y"":3304}],""Confidence"":0.996},{""Text"":""thopters."",""BoundingPolygon"":[{""X"":2006,""Y"":3196},{""X"":2002,""Y"":2960},{""X"":2069,""Y"":2960},{""X"":2070,""Y"":3195}],""Confidence"":0.988},{""Text"":""The"",""BoundingPolygon"":[{""X"":2002,""Y"":2946},{""X"":2002,""Y"":2842},{""X"":2070,""Y"":2842},{""X"":2069,""Y"":2946}],""Confidence"":0.994},{""Text"":""larvae,"",""BoundingPolygon"":[{""X"":2002,""Y"":2828},{""X"":2002,""Y"":2615},{""X"":2075,""Y"":2617},{""X"":2071,""Y"":2828}],""Confidence"":0.981}]},{""Text"":""grow and flourish. May the seeds of"",""BoundingPolygon"":[{""X"":1976,""Y"":1658},{""X"":1954,""Y"":592},{""X"":2023,""Y"":591},{""X"":2056,""Y"":1655}],""Words"":[{""Text"":""grow"",""BoundingPolygon"":[{""X"":1983,""Y"":1652},{""X"":1976,""Y"":1521},{""X"":2049,""Y"":1520},{""X"":2056,""Y"":1650}],""Confidence"":0.993},{""Text"":""and"",""BoundingPolygon"":[{""X"":1974,""Y"":1486},{""X"":1969,""Y"":1374},{""X"":2043,""Y"":1373},{""X"":2048,""Y"":1484}],""Confidence"":0.998},{""Text"":""flourish."",""BoundingPolygon"":[{""X"":1968,""Y"":1353},{""X"":1959,""Y"":1106},{""X"":2033,""Y"":1105},{""X"":2042,""Y"":1351}],""Confidence"":0.99},{""Text"":""May"",""BoundingPolygon"":[{""X"":1959,""Y"":1091},{""X"":1956,""Y"":950},{""X"":2029,""Y"":950},{""X"":2032,""Y"":1090}],""Confidence"":0.999},{""Text"":""the"",""BoundingPolygon"":[{""X"":1955,""Y"":929},{""X"":1954,""Y"":835},{""X"":2026,""Y"":834},{""X"":2028,""Y"":928}],""Confidence"":0.995},{""Text"":""seeds"",""BoundingPolygon"":[{""X"":1954,""Y"":819},{""X"":1954,""Y"":673},{""X"":2023,""Y"":673},{""X"":2026,""Y"":819}],""Confidence"":0.994},{""Text"":""of"",""BoundingPolygon"":[{""X"":1954,""Y"":657},{""X"":1954,""Y"":593},{""X"":2022,""Y"":593},{""X"":2023,""Y"":657}],""Confidence"":0.998}]},{""Text"":""however, are a different story entirely.\u0022"",""BoundingPolygon"":[{""X"":2077,""Y"":3672},{""X"":2054,""Y"":2604},{""X"":2127,""Y"":2604},{""X"":2144,""Y"":3670}],""Words"":[{""Text"":""however,"",""BoundingPolygon"":[{""X"":2085,""Y"":3672},{""X"":2082,""Y"":3429},{""X"":2141,""Y"":3424},{""X"":2144,""Y"":3667}],""Confidence"":0.98},{""Text"":""are"",""BoundingPolygon"":[{""X"":2082,""Y"":3415},{""X"":2080,""Y"":3320},{""X"":2140,""Y"":3316},{""X"":2141,""Y"":3411}],""Confidence"":0.991},{""Text"":""a"",""BoundingPolygon"":[{""X"":2080,""Y"":3305},{""X"":2079,""Y"":3274},{""X"":2140,""Y"":3270},{""X"":2140,""Y"":3302}],""Confidence"":0.994},{""Text"":""different"",""BoundingPolygon"":[{""X"":2079,""Y"":3250},{""X"":2072,""Y"":3019},{""X"":2136,""Y"":3017},{""X"":2139,""Y"":3247}],""Confidence"":0.982},{""Text"":""story"",""BoundingPolygon"":[{""X"":2071,""Y"":3006},{""X"":2066,""Y"":2873},{""X"":2133,""Y"":2871},{""X"":2136,""Y"":3004}],""Confidence"":0.993},{""Text"":""entirely.\u0022"",""BoundingPolygon"":[{""X"":2065,""Y"":2850},{""X"":2054,""Y"":2606},{""X"":2128,""Y"":2606},{""X"":2133,""Y"":2848}],""Confidence"":0.968}]},{""Text"":""your mind be equally fruitful.\u0022"",""BoundingPolygon"":[{""X"":2046,""Y"":1657},{""X"":2019,""Y"":751},{""X"":2097,""Y"":749},{""X"":2116,""Y"":1654}],""Words"":[{""Text"":""your"",""BoundingPolygon"":[{""X"":2048,""Y"":1652},{""X"":2045,""Y"":1515},{""X"":2115,""Y"":1513},{""X"":2115,""Y"":1650}],""Confidence"":0.989},{""Text"":""mind"",""BoundingPolygon"":[{""X"":2045,""Y"":1500},{""X"":2040,""Y"":1346},{""X"":2114,""Y"":1343},{""X"":2115,""Y"":1498}],""Confidence"":0.991},{""Text"":""be"",""BoundingPolygon"":[{""X"":2040,""Y"":1325},{""X"":2038,""Y"":1255},{""X"":2112,""Y"":1252},{""X"":2113,""Y"":1322}],""Confidence"":0.996},{""Text"":""equally"",""BoundingPolygon"":[{""X"":2037,""Y"":1239},{""X"":2030,""Y"":1029},{""X"":2105,""Y"":1025},{""X"":2112,""Y"":1237}],""Confidence"":0.993},{""Text"":""fruitful.\u0022"",""BoundingPolygon"":[{""X"":2029,""Y"":1012},{""X"":2019,""Y"":754},{""X"":2090,""Y"":750},{""X"":2104,""Y"":1009}],""Confidence"":0.959}]},{""Text"":""-Kiran Nalaar, Ghirapur inventor"",""BoundingPolygon"":[{""X"":2144,""Y"":3658},{""X"":2134,""Y"":2654},{""X"":2200,""Y"":2654},{""X"":2209,""Y"":3657}],""Words"":[{""Text"":""-Kiran"",""BoundingPolygon"":[{""X"":2143,""Y"":3657},{""X"":2143,""Y"":3437},{""X"":2208,""Y"":3437},{""X"":2206,""Y"":3657}],""Confidence"":0.937},{""Text"":""Nalaar,"",""BoundingPolygon"":[{""X"":2143,""Y"":3409},{""X"":2141,""Y"":3187},{""X"":2207,""Y"":3188},{""X"":2208,""Y"":3410}],""Confidence"":0.993},{""Text"":""Ghirapur"",""BoundingPolygon"":[{""X"":2141,""Y"":3174},{""X"":2138,""Y"":2907},{""X"":2202,""Y"":2908},{""X"":2207,""Y"":3175}],""Confidence"":0.994},{""Text"":""inventor"",""BoundingPolygon"":[{""X"":2137,""Y"":2892},{""X"":2134,""Y"":2659},{""X"":2194,""Y"":2660},{""X"":2201,""Y"":2893}],""Confidence"":0.993}]},{""Text"":""1/1"",""BoundingPolygon"":[{""X"":2221,""Y"":2647},{""X"":2225,""Y"":2482},{""X"":2298,""Y"":2478},{""X"":2297,""Y"":2647}],""Words"":[{""Text"":""1/1"",""BoundingPolygon"":[{""X"":2222,""Y"":2600},{""X"":2223,""Y"":2496},{""X"":2299,""Y"":2497},{""X"":2298,""Y"":2601}],""Confidence"":0.997}]},{""Text"":""1/1"",""BoundingPolygon"":[{""X"":2150,""Y"":592},{""X"":2150,""Y"":452},{""X"":2217,""Y"":446},{""X"":2215,""Y"":587}],""Words"":[{""Text"":""1/1"",""BoundingPolygon"":[{""X"":2150,""Y"":581},{""X"":2150,""Y"":480},{""X"":2217,""Y"":481},{""X"":2216,""Y"":581}],""Confidence"":0.997}]},{""Text"":""175/222 \u0026 D. ALEXANDER GREGORY"",""BoundingPolygon"":[{""X"":2234,""Y"":1688},{""X"":2249,""Y"":1075},{""X"":2315,""Y"":1076},{""X"":2308,""Y"":1689}],""Words"":[{""Text"":""175/222"",""BoundingPolygon"":[{""X"":2234,""Y"":1680},{""X"":2244,""Y"":1515},{""X"":2313,""Y"":1513},{""X"":2304,""Y"":1676}],""Confidence"":0.582},{""Text"":""\u0026"",""BoundingPolygon"":[{""X"":2245,""Y"":1498},{""X"":2247,""Y"":1468},{""X"":2314,""Y"":1465},{""X"":2313,""Y"":1495}],""Confidence"":0.118},{""Text"":""D."",""BoundingPolygon"":[{""X"":2248,""Y"":1450},{""X"":2249,""Y"":1423},{""X"":2315,""Y"":1421},{""X"":2314,""Y"":1448}],""Confidence"":0.959},{""Text"":""ALEXANDER"",""BoundingPolygon"":[{""X"":2250,""Y"":1410},{""X"":2257,""Y"":1236},{""X"":2312,""Y"":1235},{""X"":2315,""Y"":1408}],""Confidence"":0.92},{""Text"":""GREGORY"",""BoundingPolygon"":[{""X"":2258,""Y"":1223},{""X"":2262,""Y"":1078},{""X"":2304,""Y"":1077},{""X"":2312,""Y"":1222}],""Confidence"":0.985}]},{""Text"":""170/272 C"",""BoundingPolygon"":[{""X"":2314,""Y"":3703},{""X"":2324,""Y"":3445},{""X"":2372,""Y"":3447},{""X"":2358,""Y"":3704}],""Words"":[{""Text"":""170/272"",""BoundingPolygon"":[{""X"":2314,""Y"":3701},{""X"":2319,""Y"":3537},{""X"":2363,""Y"":3537},{""X"":2358,""Y"":3697}],""Confidence"":0.99},{""Text"":""C"",""BoundingPolygon"":[{""X"":2320,""Y"":3518},{""X"":2321,""Y"":3495},{""X"":2367,""Y"":3496},{""X"":2365,""Y"":3518}],""Confidence"":0.415}]},{""Text"":""IM \u0026 C 2015 Wizards of the Coast"",""BoundingPolygon"":[{""X"":2253,""Y"":913},{""X"":2253,""Y"":430},{""X"":2293,""Y"":430},{""X"":2297,""Y"":913}],""Words"":[{""Text"":""IM"",""BoundingPolygon"":[{""X"":2253,""Y"":911},{""X"":2254,""Y"":879},{""X"":2295,""Y"":879},{""X"":2294,""Y"":911}],""Confidence"":0.62},{""Text"":""\u0026"",""BoundingPolygon"":[{""X"":2254,""Y"":870},{""X"":2255,""Y"":847},{""X"":2295,""Y"":847},{""X"":2295,""Y"":870}],""Confidence"":0.841},{""Text"":""C"",""BoundingPolygon"":[{""X"":2255,""Y"":835},{""X"":2255,""Y"":817},{""X"":2296,""Y"":817},{""X"":2296,""Y"":835}],""Confidence"":0.142},{""Text"":""2015"",""BoundingPolygon"":[{""X"":2255,""Y"":800},{""X"":2256,""Y"":733},{""X"":2296,""Y"":733},{""X"":2296,""Y"":800}],""Confidence"":0.989},{""Text"":""Wizards"",""BoundingPolygon"":[{""X"":2256,""Y"":725},{""X"":2256,""Y"":610},{""X"":2294,""Y"":610},{""X"":2296,""Y"":725}],""Confidence"":0.952},{""Text"":""of"",""BoundingPolygon"":[{""X"":2256,""Y"":602},{""X"":2256,""Y"":575},{""X"":2293,""Y"":575},{""X"":2294,""Y"":602}],""Confidence"":0.988},{""Text"":""the"",""BoundingPolygon"":[{""X"":2256,""Y"":567},{""X"":2255,""Y"":520},{""X"":2291,""Y"":520},{""X"":2293,""Y"":567}],""Confidence"":0.993},{""Text"":""Coast"",""BoundingPolygon"":[{""X"":2255,""Y"":512},{""X"":2253,""Y"":432},{""X"":2285,""Y"":432},{""X"":2290,""Y"":512}],""Confidence"":0.994}]},{""Text"":""ORI . EN K JACK WANG"",""BoundingPolygon"":[{""X"":2343,""Y"":3703},{""X"":2344,""Y"":3296},{""X"":2391,""Y"":3297},{""X"":2390,""Y"":3703}],""Words"":[{""Text"":""ORI"",""BoundingPolygon"":[{""X"":2345,""Y"":3700},{""X"":2343,""Y"":3625},{""X"":2389,""Y"":3624},{""X"":2390,""Y"":3698}],""Confidence"":0.928},{""Text"":""."",""BoundingPolygon"":[{""X"":2343,""Y"":3615},{""X"":2343,""Y"":3596},{""X"":2389,""Y"":3595},{""X"":2389,""Y"":3615}],""Confidence"":0.999},{""Text"":""EN"",""BoundingPolygon"":[{""X"":2343,""Y"":3586},{""X"":2343,""Y"":3541},{""X"":2388,""Y"":3541},{""X"":2389,""Y"":3586}],""Confidence"":0.994},{""Text"":""K"",""BoundingPolygon"":[{""X"":2343,""Y"":3516},{""X"":2343,""Y"":3494},{""X"":2388,""Y"":3495},{""X"":2388,""Y"":3517}],""Confidence"":0.62},{""Text"":""JACK"",""BoundingPolygon"":[{""X"":2343,""Y"":3472},{""X"":2345,""Y"":3403},{""X"":2388,""Y"":3405},{""X"":2388,""Y"":3474}],""Confidence"":0.996},{""Text"":""WANG"",""BoundingPolygon"":[{""X"":2346,""Y"":3393},{""X"":2350,""Y"":3304},{""X"":2389,""Y"":3308},{""X"":2388,""Y"":3396}],""Confidence"":0.998}]},{""Text"":""IM \u0026 C 2015 Wizards of the Coast"",""BoundingPolygon"":[{""X"":2335,""Y"":2939},{""X"":2334,""Y"":2437},{""X"":2374,""Y"":2437},{""X"":2375,""Y"":2939}],""Words"":[{""Text"":""IM"",""BoundingPolygon"":[{""X"":2336,""Y"":2936},{""X"":2336,""Y"":2903},{""X"":2374,""Y"":2900},{""X"":2374,""Y"":2932}],""Confidence"":0.792},{""Text"":""\u0026"",""BoundingPolygon"":[{""X"":2336,""Y"":2891},{""X"":2335,""Y"":2869},{""X"":2375,""Y"":2866},{""X"":2375,""Y"":2888}],""Confidence"":0.989},{""Text"":""C"",""BoundingPolygon"":[{""X"":2335,""Y"":2857},{""X"":2335,""Y"":2838},{""X"":2375,""Y"":2835},{""X"":2375,""Y"":2854}],""Confidence"":0.606},{""Text"":""2015"",""BoundingPolygon"":[{""X"":2335,""Y"":2823},{""X"":2334,""Y"":2753},{""X"":2375,""Y"":2751},{""X"":2375,""Y"":2820}],""Confidence"":0.981},{""Text"":""Wizards"",""BoundingPolygon"":[{""X"":2334,""Y"":2745},{""X"":2334,""Y"":2630},{""X"":2375,""Y"":2629},{""X"":2375,""Y"":2742}],""Confidence"":0.993},{""Text"":""of"",""BoundingPolygon"":[{""X"":2334,""Y"":2622},{""X"":2334,""Y"":2591},{""X"":2374,""Y"":2590},{""X"":2375,""Y"":2620}],""Confidence"":0.998},{""Text"":""the"",""BoundingPolygon"":[{""X"":2334,""Y"":2582},{""X"":2334,""Y"":2536},{""X"":2374,""Y"":2535},{""X"":2374,""Y"":2581}],""Confidence"":0.993},{""Text"":""Coast"",""BoundingPolygon"":[{""X"":2334,""Y"":2527},{""X"":2335,""Y"":2439},{""X"":2372,""Y"":2439},{""X"":2374,""Y"":2527}],""Confidence"":0.994}]}]}]";

		}

        private string LoadTestResObj2()
        {
            return @"[{""Lines"":[{""Text"":""Caustic Caterpillar"",""BoundingPolygon"":[{""X"":689,""Y"":3652},{""X"":684,""Y"":3012},{""X"":756,""Y"":3012},{""X"":763,""Y"":3651}],""Words"":[{""Text"":""Caustic"",""BoundingPolygon"":[{""X"":691,""Y"":3647},{""X"":686,""Y"":3407},{""X"":760,""Y"":3407},{""X"":762,""Y"":3645}],""Confidence"":0.995},{""Text"":""Caterpillar"",""BoundingPolygon"":[{""X"":685,""Y"":3386},{""X"":684,""Y"":3015},{""X"":756,""Y"":3017},{""X"":760,""Y"":3385}],""Confidence"":0.991}]},{""Text"":""Elvish Visionary"",""BoundingPolygon"":[{""X"":700,""Y"":1887},{""X"":701,""Y"":1345},{""X"":771,""Y"":1346},{""X"":769,""Y"":1887}],""Words"":[{""Text"":""Elvish"",""BoundingPolygon"":[{""X"":700,""Y"":1887},{""X"":700,""Y"":1699},{""X"":769,""Y"":1699},{""X"":770,""Y"":1887}],""Confidence"":0.995},{""Text"":""Visionary"",""BoundingPolygon"":[{""X"":700,""Y"":1665},{""X"":701,""Y"":1354},{""X"":771,""Y"":1356},{""X"":769,""Y"":1666}],""Confidence"":0.994}]},{""Text"":""1"",""BoundingPolygon"":[{""X"":684,""Y"":828},{""X"":686,""Y"":700},{""X"":743,""Y"":702},{""X"":753,""Y"":828}],""Words"":[{""Text"":""1"",""BoundingPolygon"":[{""X"":684,""Y"":820},{""X"":684,""Y"":785},{""X"":752,""Y"":783},{""X"":753,""Y"":817}],""Confidence"":0.428}]},{""Text"":""Creature - Insect"",""BoundingPolygon"":[{""X"":1726,""Y"":3673},{""X"":1725,""Y"":3160},{""X"":1781,""Y"":3160},{""X"":1782,""Y"":3673}],""Words"":[{""Text"":""Creature"",""BoundingPolygon"":[{""X"":1725,""Y"":3673},{""X"":1725,""Y"":3437},{""X"":1781,""Y"":3436},{""X"":1782,""Y"":3673}],""Confidence"":0.994},{""Text"":""-"",""BoundingPolygon"":[{""X"":1725,""Y"":3416},{""X"":1725,""Y"":3385},{""X"":1781,""Y"":3384},{""X"":1781,""Y"":3415}],""Confidence"":0.98},{""Text"":""Insect"",""BoundingPolygon"":[{""X"":1725,""Y"":3343},{""X"":1725,""Y"":3165},{""X"":1780,""Y"":3163},{""X"":1781,""Y"":3342}],""Confidence"":0.99}]},{""Text"":""Creature - Elf Shaman"",""BoundingPolygon"":[{""X"":1736,""Y"":1857},{""X"":1720,""Y"":1180},{""X"":1787,""Y"":1179},{""X"":1797,""Y"":1856}],""Words"":[{""Text"":""Creature"",""BoundingPolygon"":[{""X"":1742,""Y"":1856},{""X"":1733,""Y"":1607},{""X"":1795,""Y"":1607},{""X"":1796,""Y"":1855}],""Confidence"":0.994},{""Text"":""-"",""BoundingPolygon"":[{""X"":1732,""Y"":1585},{""X"":1731,""Y"":1554},{""X"":1795,""Y"":1553},{""X"":1795,""Y"":1584}],""Confidence"":0.994},{""Text"":""Elf"",""BoundingPolygon"":[{""X"":1730,""Y"":1511},{""X"":1727,""Y"":1430},{""X"":1793,""Y"":1429},{""X"":1794,""Y"":1510}],""Confidence"":0.994},{""Text"":""Shaman"",""BoundingPolygon"":[{""X"":1727,""Y"":1417},{""X"":1720,""Y"":1194},{""X"":1787,""Y"":1192},{""X"":1793,""Y"":1416}],""Confidence"":0.997}]},{""Text"":""1\u058F, Sacrifice Caustic Caterpillar:"",""BoundingPolygon"":[{""X"":1906,""Y"":3684},{""X"":1905,""Y"":2637},{""X"":1973,""Y"":2637},{""X"":1975,""Y"":3684}],""Words"":[{""Text"":""1\u058F,"",""BoundingPolygon"":[{""X"":1907,""Y"":3675},{""X"":1907,""Y"":3542},{""X"":1974,""Y"":3537},{""X"":1975,""Y"":3669}],""Confidence"":0.591},{""Text"":""Sacrifice"",""BoundingPolygon"":[{""X"":1907,""Y"":3528},{""X"":1907,""Y"":3267},{""X"":1974,""Y"":3264},{""X"":1974,""Y"":3523}],""Confidence"":0.991},{""Text"":""Caustic"",""BoundingPolygon"":[{""X"":1907,""Y"":3243},{""X"":1906,""Y"":3009},{""X"":1974,""Y"":3007},{""X"":1974,""Y"":3239}],""Confidence"":0.992},{""Text"":""Caterpillar:"",""BoundingPolygon"":[{""X"":1906,""Y"":2989},{""X"":1905,""Y"":2638},{""X"":1973,""Y"":2638},{""X"":1974,""Y"":2987}],""Confidence"":0.993}]},{""Text"":""Destroy target artifact or enchantment."",""BoundingPolygon"":[{""X"":1985,""Y"":3697},{""X"":1980,""Y"":2484},{""X"":2044,""Y"":2484},{""X"":2049,""Y"":3696}],""Words"":[{""Text"":""Destroy"",""BoundingPolygon"":[{""X"":1984,""Y"":3689},{""X"":1983,""Y"":3455},{""X"":2049,""Y"":3453},{""X"":2048,""Y"":3687}],""Confidence"":0.996},{""Text"":""target"",""BoundingPolygon"":[{""X"":1983,""Y"":3432},{""X"":1983,""Y"":3249},{""X"":2048,""Y"":3247},{""X"":2049,""Y"":3430}],""Confidence"":0.993},{""Text"":""artifact"",""BoundingPolygon"":[{""X"":1983,""Y"":3234},{""X"":1982,""Y"":3009},{""X"":2047,""Y"":3008},{""X"":2048,""Y"":3233}],""Confidence"":0.993},{""Text"":""or"",""BoundingPolygon"":[{""X"":1982,""Y"":2994},{""X"":1982,""Y"":2924},{""X"":2046,""Y"":2924},{""X"":2047,""Y"":2993}],""Confidence"":0.999},{""Text"":""enchantment."",""BoundingPolygon"":[{""X"":1982,""Y"":2910},{""X"":1980,""Y"":2485},{""X"":2041,""Y"":2485},{""X"":2046,""Y"":2909}],""Confidence"":0.994}]},{""Text"":""When Elvish Visionary enters the"",""BoundingPolygon"":[{""X"":1942,""Y"":1854},{""X"":1918,""Y"":771},{""X"":1992,""Y"":770},{""X"":2013,""Y"":1852}],""Words"":[{""Text"":""When"",""BoundingPolygon"":[{""X"":1941,""Y"":1849},{""X"":1939,""Y"":1664},{""X"":2011,""Y"":1661},{""X"":2012,""Y"":1845}],""Confidence"":0.993},{""Text"":""Elvish"",""BoundingPolygon"":[{""X"":1939,""Y"":1634},{""X"":1935,""Y"":1445},{""X"":2008,""Y"":1442},{""X"":2011,""Y"":1630}],""Confidence"":0.995},{""Text"":""Visionary"",""BoundingPolygon"":[{""X"":1935,""Y"":1419},{""X"":1928,""Y"":1118},{""X"":1999,""Y"":1115},{""X"":2007,""Y"":1416}],""Confidence"":0.995},{""Text"":""enters"",""BoundingPolygon"":[{""X"":1927,""Y"":1092},{""X"":1922,""Y"":894},{""X"":1991,""Y"":892},{""X"":1999,""Y"":1090}],""Confidence"":0.995},{""Text"":""the"",""BoundingPolygon"":[{""X"":1921,""Y"":877},{""X"":1918,""Y"":777},{""X"":1986,""Y"":775},{""X"":1990,""Y"":875}],""Confidence"":0.999}]},{""Text"":""\u0022The rare and beautiful butterflies inspire"",""BoundingPolygon"":[{""X"":2085,""Y"":3688},{""X"":2084,""Y"":2503},{""X"":2154,""Y"":2503},{""X"":2156,""Y"":3687}],""Words"":[{""Text"":""\u0022The"",""BoundingPolygon"":[{""X"":2090,""Y"":3688},{""X"":2088,""Y"":3541},{""X"":2152,""Y"":3540},{""X"":2150,""Y"":3686}],""Confidence"":0.985},{""Text"":""rare"",""BoundingPolygon"":[{""X"":2088,""Y"":3526},{""X"":2087,""Y"":3407},{""X"":2154,""Y"":3406},{""X"":2152,""Y"":3525}],""Confidence"":0.989},{""Text"":""and"",""BoundingPolygon"":[{""X"":2087,""Y"":3392},{""X"":2086,""Y"":3282},{""X"":2154,""Y"":3281},{""X"":2154,""Y"":3391}],""Confidence"":0.998},{""Text"":""beautiful"",""BoundingPolygon"":[{""X"":2085,""Y"":3261},{""X"":2084,""Y"":3002},{""X"":2155,""Y"":3002},{""X"":2155,""Y"":3261}],""Confidence"":0.989},{""Text"":""butterflies"",""BoundingPolygon"":[{""X"":2084,""Y"":2988},{""X"":2084,""Y"":2710},{""X"":2154,""Y"":2711},{""X"":2155,""Y"":2988}],""Confidence"":0.989},{""Text"":""inspire"",""BoundingPolygon"":[{""X"":2084,""Y"":2696},{""X"":2084,""Y"":2504},{""X"":2153,""Y"":2506},{""X"":2154,""Y"":2697}],""Confidence"":0.994}]},{""Text"":""battlefield, draw a card."",""BoundingPolygon"":[{""X"":2020,""Y"":1850},{""X"":2006,""Y"":1083},{""X"":2076,""Y"":1081},{""X"":2090,""Y"":1849}],""Words"":[{""Text"":""battlefield,"",""BoundingPolygon"":[{""X"":2019,""Y"":1850},{""X"":2016,""Y"":1493},{""X"":2084,""Y"":1491},{""X"":2090,""Y"":1849}],""Confidence"":0.994},{""Text"":""draw"",""BoundingPolygon"":[{""X"":2015,""Y"":1479},{""X"":2013,""Y"":1341},{""X"":2081,""Y"":1337},{""X"":2084,""Y"":1476}],""Confidence"":0.993},{""Text"":""a"",""BoundingPolygon"":[{""X"":2012,""Y"":1293},{""X"":2011,""Y"":1259},{""X"":2079,""Y"":1256},{""X"":2080,""Y"":1290}],""Confidence"":0.998},{""Text"":""card."",""BoundingPolygon"":[{""X"":2010,""Y"":1239},{""X"":2006,""Y"":1086},{""X"":2075,""Y"":1082},{""X"":2079,""Y"":1235}],""Confidence"":0.958}]},{""Text"":""the design of our thopters. The larvae,"",""BoundingPolygon"":[{""X"":2159,""Y"":3699},{""X"":2159,""Y"":2612},{""X"":2225,""Y"":2612},{""X"":2225,""Y"":3699}],""Words"":[{""Text"":""the"",""BoundingPolygon"":[{""X"":2161,""Y"":3699},{""X"":2160,""Y"":3610},{""X"":2225,""Y"":3607},{""X"":2224,""Y"":3695}],""Confidence"":0.998},{""Text"":""design"",""BoundingPolygon"":[{""X"":2160,""Y"":3591},{""X"":2159,""Y"":3423},{""X"":2225,""Y"":3421},{""X"":2225,""Y"":3588}],""Confidence"":0.994},{""Text"":""of"",""BoundingPolygon"":[{""X"":2159,""Y"":3395},{""X"":2159,""Y"":3335},{""X"":2226,""Y"":3334},{""X"":2225,""Y"":3393}],""Confidence"":0.993},{""Text"":""our"",""BoundingPolygon"":[{""X"":2159,""Y"":3321},{""X"":2159,""Y"":3222},{""X"":2226,""Y"":3222},{""X"":2226,""Y"":3320}],""Confidence"":0.993},{""Text"":""thopters."",""BoundingPolygon"":[{""X"":2159,""Y"":3208},{""X"":2159,""Y"":2966},{""X"":2226,""Y"":2967},{""X"":2226,""Y"":3208}],""Confidence"":0.98},{""Text"":""The"",""BoundingPolygon"":[{""X"":2159,""Y"":2952},{""X"":2159,""Y"":2844},{""X"":2225,""Y"":2847},{""X"":2226,""Y"":2953}],""Confidence"":0.999},{""Text"":""larvae,"",""BoundingPolygon"":[{""X"":2159,""Y"":2830},{""X"":2160,""Y"":2614},{""X"":2224,""Y"":2618},{""X"":2225,""Y"":2833}],""Confidence"":0.993}]},{""Text"":""\u0022From a tiny sprout, the greatest trees"",""BoundingPolygon"":[{""X"":2131,""Y"":1832},{""X"":2114,""Y"":700},{""X"":2184,""Y"":698},{""X"":2203,""Y"":1831}],""Words"":[{""Text"":""\u0022From"",""BoundingPolygon"":[{""X"":2133,""Y"":1831},{""X"":2127,""Y"":1663},{""X"":2203,""Y"":1663},{""X"":2202,""Y"":1831}],""Confidence"":0.959},{""Text"":""a"",""BoundingPolygon"":[{""X"":2126,""Y"":1613},{""X"":2125,""Y"":1578},{""X"":2202,""Y"":1578},{""X"":2203,""Y"":1613}],""Confidence"":0.996},{""Text"":""tiny"",""BoundingPolygon"":[{""X"":2124,""Y"":1552},{""X"":2121,""Y"":1436},{""X"":2201,""Y"":1436},{""X"":2202,""Y"":1552}],""Confidence"":0.986},{""Text"":""sprout,"",""BoundingPolygon"":[{""X"":2121,""Y"":1415},{""X"":2117,""Y"":1213},{""X"":2195,""Y"":1212},{""X"":2200,""Y"":1414}],""Confidence"":0.994},{""Text"":""the"",""BoundingPolygon"":[{""X"":2117,""Y"":1198},{""X"":2116,""Y"":1101},{""X"":2191,""Y"":1100},{""X"":2195,""Y"":1197}],""Confidence"":0.998},{""Text"":""greatest"",""BoundingPolygon"":[{""X"":2115,""Y"":1085},{""X"":2114,""Y"":855},{""X"":2180,""Y"":853},{""X"":2191,""Y"":1084}],""Confidence"":0.993},{""Text"":""trees"",""BoundingPolygon"":[{""X"":2114,""Y"":840},{""X"":2114,""Y"":704},{""X"":2170,""Y"":702},{""X"":2179,""Y"":838}],""Confidence"":0.994}]},{""Text"":""however, are a different story entirely.\u0022"",""BoundingPolygon"":[{""X"":2227,""Y"":3699},{""X"":2218,""Y"":2609},{""X"":2294,""Y"":2609},{""X"":2300,""Y"":3698}],""Words"":[{""Text"":""however,"",""BoundingPolygon"":[{""X"":2231,""Y"":3698},{""X"":2235,""Y"":3448},{""X"":2298,""Y"":3448},{""X"":2292,""Y"":3698}],""Confidence"":0.986},{""Text"":""are"",""BoundingPolygon"":[{""X"":2235,""Y"":3434},{""X"":2236,""Y"":3343},{""X"":2299,""Y"":3343},{""X"":2298,""Y"":3434}],""Confidence"":0.998},{""Text"":""a"",""BoundingPolygon"":[{""X"":2236,""Y"":3324},{""X"":2236,""Y"":3292},{""X"":2299,""Y"":3292},{""X"":2299,""Y"":3324}],""Confidence"":0.994},{""Text"":""different"",""BoundingPolygon"":[{""X"":2236,""Y"":3268},{""X"":2232,""Y"":3028},{""X"":2298,""Y"":3028},{""X"":2299,""Y"":3268}],""Confidence"":0.976},{""Text"":""story"",""BoundingPolygon"":[{""X"":2232,""Y"":3013},{""X"":2228,""Y"":2879},{""X"":2296,""Y"":2879},{""X"":2298,""Y"":3013}],""Confidence"":0.996},{""Text"":""entirely.\u0022"",""BoundingPolygon"":[{""X"":2228,""Y"":2855},{""X"":2218,""Y"":2610},{""X"":2287,""Y"":2610},{""X"":2295,""Y"":2855}],""Confidence"":0.928}]},{""Text"":""grow and flourish. May the seeds of"",""BoundingPolygon"":[{""X"":2217,""Y"":1851},{""X"":2178,""Y"":753},{""X"":2255,""Y"":751},{""X"":2292,""Y"":1848}],""Words"":[{""Text"":""grow"",""BoundingPolygon"":[{""X"":2220,""Y"":1841},{""X"":2214,""Y"":1708},{""X"":2285,""Y"":1706},{""X"":2291,""Y"":1839}],""Confidence"":0.985},{""Text"":""and"",""BoundingPolygon"":[{""X"":2212,""Y"":1671},{""X"":2207,""Y"":1558},{""X"":2279,""Y"":1556},{""X"":2284,""Y"":1670}],""Confidence"":0.998},{""Text"":""flourish."",""BoundingPolygon"":[{""X"":2206,""Y"":1536},{""X"":2196,""Y"":1285},{""X"":2269,""Y"":1283},{""X"":2278,""Y"":1534}],""Confidence"":0.994},{""Text"":""May"",""BoundingPolygon"":[{""X"":2195,""Y"":1270},{""X"":2190,""Y"":1122},{""X"":2264,""Y"":1120},{""X"":2269,""Y"":1268}],""Confidence"":0.998},{""Text"":""the"",""BoundingPolygon"":[{""X"":2189,""Y"":1100},{""X"":2186,""Y"":1004},{""X"":2261,""Y"":1003},{""X"":2264,""Y"":1098}],""Confidence"":0.998},{""Text"":""seeds"",""BoundingPolygon"":[{""X"":2185,""Y"":989},{""X"":2180,""Y"":840},{""X"":2258,""Y"":838},{""X"":2261,""Y"":987}],""Confidence"":0.994},{""Text"":""of"",""BoundingPolygon"":[{""X"":2180,""Y"":824},{""X"":2178,""Y"":757},{""X"":2256,""Y"":756},{""X"":2257,""Y"":822}],""Confidence"":0.998}]},{""Text"":""-Kiran Nalaar, Ghirapur inventor"",""BoundingPolygon"":[{""X"":2295,""Y"":3685},{""X"":2300,""Y"":2660},{""X"":2367,""Y"":2660},{""X"":2363,""Y"":3685}],""Words"":[{""Text"":""-Kiran"",""BoundingPolygon"":[{""X"":2295,""Y"":3683},{""X"":2296,""Y"":3463},{""X"":2364,""Y"":3465},{""X"":2360,""Y"":3685}],""Confidence"":0.973},{""Text"":""Nalaar,"",""BoundingPolygon"":[{""X"":2296,""Y"":3439},{""X"":2298,""Y"":3205},{""X"":2366,""Y"":3206},{""X"":2364,""Y"":3441}],""Confidence"":0.992},{""Text"":""Ghirapur"",""BoundingPolygon"":[{""X"":2298,""Y"":3191},{""X"":2299,""Y"":2918},{""X"":2366,""Y"":2919},{""X"":2366,""Y"":3192}],""Confidence"":0.993},{""Text"":""inventor"",""BoundingPolygon"":[{""X"":2299,""Y"":2903},{""X"":2301,""Y"":2664},{""X"":2363,""Y"":2664},{""X"":2366,""Y"":2903}],""Confidence"":0.956}]},{""Text"":""your mind be equally fruitful.\u0022"",""BoundingPolygon"":[{""X"":2287,""Y"":1849},{""X"":2253,""Y"":916},{""X"":2334,""Y"":914},{""X"":2360,""Y"":1848}],""Words"":[{""Text"":""your"",""BoundingPolygon"":[{""X"":2291,""Y"":1844},{""X"":2285,""Y"":1699},{""X"":2358,""Y"":1698},{""X"":2359,""Y"":1844}],""Confidence"":0.988},{""Text"":""mind"",""BoundingPolygon"":[{""X"":2284,""Y"":1682},{""X"":2277,""Y"":1524},{""X"":2354,""Y"":1523},{""X"":2357,""Y"":1682}],""Confidence"":0.987},{""Text"":""be"",""BoundingPolygon"":[{""X"":2276,""Y"":1501},{""X"":2273,""Y"":1432},{""X"":2352,""Y"":1431},{""X"":2354,""Y"":1500}],""Confidence"":0.996},{""Text"":""equally"",""BoundingPolygon"":[{""X"":2272,""Y"":1415},{""X"":2263,""Y"":1201},{""X"":2344,""Y"":1200},{""X"":2352,""Y"":1414}],""Confidence"":0.994},{""Text"":""fruitful.\u0022"",""BoundingPolygon"":[{""X"":2263,""Y"":1184},{""X"":2253,""Y"":918},{""X"":2331,""Y"":916},{""X"":2344,""Y"":1182}],""Confidence"":0.949}]},{""Text"":""1/1"",""BoundingPolygon"":[{""X"":2398,""Y"":2641},{""X"":2403,""Y"":2495},{""X"":2473,""Y"":2495},{""X"":2473,""Y"":2643}],""Words"":[{""Text"":""1/1"",""BoundingPolygon"":[{""X"":2399,""Y"":2600},{""X"":2401,""Y"":2496},{""X"":2474,""Y"":2498},{""X"":2474,""Y"":2601}],""Confidence"":0.994}]},{""Text"":""1/1"",""BoundingPolygon"":[{""X"":2393,""Y"":756},{""X"":2386,""Y"":625},{""X"":2460,""Y"":617},{""X"":2465,""Y"":750}],""Words"":[{""Text"":""1/1"",""BoundingPolygon"":[{""X"":2392,""Y"":739},{""X"":2387,""Y"":634},{""X"":2461,""Y"":630},{""X"":2465,""Y"":736}],""Confidence"":0.993}]},{""Text"":""170/272 C"",""BoundingPolygon"":[{""X"":2473,""Y"":3739},{""X"":2477,""Y"":3503},{""X"":2528,""Y"":3503},{""X"":2523,""Y"":3740}],""Words"":[{""Text"":""170/272"",""BoundingPolygon"":[{""X"":2473,""Y"":3739},{""X"":2476,""Y"":3574},{""X"":2527,""Y"":3573},{""X"":2523,""Y"":3735}],""Confidence"":0.904},{""Text"":""C"",""BoundingPolygon"":[{""X"":2476,""Y"":3556},{""X"":2477,""Y"":3530},{""X"":2527,""Y"":3530},{""X"":2527,""Y"":3555}],""Confidence"":0.713}]},{""Text"":""ORI . EN JACK WANG"",""BoundingPolygon"":[{""X"":2498,""Y"":3740},{""X"":2502,""Y"":3334},{""X"":2551,""Y"":3334},{""X"":2549,""Y"":3741}],""Words"":[{""Text"":""ORI"",""BoundingPolygon"":[{""X"":2501,""Y"":3740},{""X"":2498,""Y"":3664},{""X"":2548,""Y"":3664},{""X"":2549,""Y"":3740}],""Confidence"":0.923},{""Text"":""."",""BoundingPolygon"":[{""X"":2498,""Y"":3654},{""X"":2498,""Y"":3637},{""X"":2548,""Y"":3637},{""X"":2548,""Y"":3654}],""Confidence"":0.974},{""Text"":""EN"",""BoundingPolygon"":[{""X"":2498,""Y"":3627},{""X"":2498,""Y"":3579},{""X"":2548,""Y"":3579},{""X"":2548,""Y"":3627}],""Confidence"":0.984},{""Text"":""JACK"",""BoundingPolygon"":[{""X"":2499,""Y"":3510},{""X"":2503,""Y"":3440},{""X"":2549,""Y"":3440},{""X"":2548,""Y"":3510}],""Confidence"":0.993},{""Text"":""WANG"",""BoundingPolygon"":[{""X"":2504,""Y"":3430},{""X"":2513,""Y"":3336},{""X"":2551,""Y"":3336},{""X"":2549,""Y"":3430}],""Confidence"":0.998}]},{""Text"":""IM \u0026 C 2015 Wizards of the Coast"",""BoundingPolygon"":[{""X"":2508,""Y"":2952},{""X"":2511,""Y"":2440},{""X"":2552,""Y"":2440},{""X"":2549,""Y"":2952}],""Words"":[{""Text"":""IM"",""BoundingPolygon"":[{""X"":2508,""Y"":2953},{""X"":2508,""Y"":2917},{""X"":2548,""Y"":2915},{""X"":2548,""Y"":2951}],""Confidence"":0.847},{""Text"":""\u0026"",""BoundingPolygon"":[{""X"":2508,""Y"":2907},{""X"":2509,""Y"":2882},{""X"":2549,""Y"":2881},{""X"":2548,""Y"":2906}],""Confidence"":0.918},{""Text"":""C"",""BoundingPolygon"":[{""X"":2509,""Y"":2870},{""X"":2509,""Y"":2850},{""X"":2549,""Y"":2849},{""X"":2549,""Y"":2869}],""Confidence"":0.318},{""Text"":""2015"",""BoundingPolygon"":[{""X"":2509,""Y"":2833},{""X"":2510,""Y"":2762},{""X"":2551,""Y"":2761},{""X"":2550,""Y"":2832}],""Confidence"":0.989},{""Text"":""Wizards"",""BoundingPolygon"":[{""X"":2510,""Y"":2753},{""X"":2511,""Y"":2634},{""X"":2551,""Y"":2634},{""X"":2551,""Y"":2752}],""Confidence"":0.993},{""Text"":""of"",""BoundingPolygon"":[{""X"":2511,""Y"":2626},{""X"":2511,""Y"":2595},{""X"":2551,""Y"":2594},{""X"":2551,""Y"":2625}],""Confidence"":0.981},{""Text"":""the"",""BoundingPolygon"":[{""X"":2511,""Y"":2586},{""X"":2511,""Y"":2536},{""X"":2551,""Y"":2536},{""X"":2551,""Y"":2585}],""Confidence"":0.997},{""Text"":""Coast"",""BoundingPolygon"":[{""X"":2511,""Y"":2527},{""X"":2511,""Y"":2440},{""X"":2550,""Y"":2440},{""X"":2551,""Y"":2527}],""Confidence"":0.986}]},{""Text"":""175/272 C"",""BoundingPolygon"":[{""X"":2483,""Y"":1872},{""X"":2486,""Y"":1628},{""X"":2544,""Y"":1628},{""X"":2539,""Y"":1873}],""Words"":[{""Text"":""175/272"",""BoundingPolygon"":[{""X"":2483,""Y"":1872},{""X"":2484,""Y"":1699},{""X"":2542,""Y"":1698},{""X"":2540,""Y"":1870}],""Confidence"":0.968},{""Text"":""C"",""BoundingPolygon"":[{""X"":2485,""Y"":1678},{""X"":2486,""Y"":1650},{""X"":2544,""Y"":1649},{""X"":2543,""Y"":1678}],""Confidence"":0.602}]},{""Text"":""ORI . EN \u0026 D. ALEXANDER GREGORY"",""BoundingPolygon"":[{""X"":2511,""Y"":1876},{""X"":2503,""Y"":1243},{""X"":2559,""Y"":1241},{""X"":2574,""Y"":1875}],""Words"":[{""Text"":""ORI"",""BoundingPolygon"":[{""X"":2519,""Y"":1868},{""X"":2514,""Y"":1793},{""X"":2572,""Y"":1791},{""X"":2573,""Y"":1866}],""Confidence"":0.841},{""Text"":""."",""BoundingPolygon"":[{""X"":2513,""Y"":1781},{""X"":2512,""Y"":1766},{""X"":2571,""Y"":1765},{""X"":2572,""Y"":1779}],""Confidence"":0.208},{""Text"":""EN"",""BoundingPolygon"":[{""X"":2511,""Y"":1754},{""X"":2508,""Y"":1699},{""X"":2570,""Y"":1697},{""X"":2571,""Y"":1752}],""Confidence"":0.949},{""Text"":""\u0026"",""BoundingPolygon"":[{""X"":2507,""Y"":1682},{""X"":2506,""Y"":1653},{""X"":2569,""Y"":1651},{""X"":2570,""Y"":1680}],""Confidence"":0.118},{""Text"":""D."",""BoundingPolygon"":[{""X"":2505,""Y"":1632},{""X"":2504,""Y"":1603},{""X"":2568,""Y"":1601},{""X"":2569,""Y"":1630}],""Confidence"":0.959},{""Text"":""ALEXANDER"",""BoundingPolygon"":[{""X"":2504,""Y"":1591},{""X"":2503,""Y"":1413},{""X"":2562,""Y"":1411},{""X"":2568,""Y"":1589}],""Confidence"":0.98},{""Text"":""GREGORY"",""BoundingPolygon"":[{""X"":2503,""Y"":1400},{""X"":2508,""Y"":1246},{""X"":2556,""Y"":1244},{""X"":2562,""Y"":1399}],""Confidence"":0.958}]},{""Text"":""IM \u0026 C 2015 Wizards of the Coast"",""BoundingPolygon"":[{""X"":2501,""Y"":1080},{""X"":2496,""Y"":576},{""X"":2536,""Y"":575},{""X"":2547,""Y"":1079}],""Words"":[{""Text"":""IM"",""BoundingPolygon"":[{""X"":2501,""Y"":1077},{""X"":2500,""Y"":1045},{""X"":2546,""Y"":1042},{""X"":2547,""Y"":1074}],""Confidence"":0.728},{""Text"":""\u0026"",""BoundingPolygon"":[{""X"":2500,""Y"":1037},{""X"":2499,""Y"":1011},{""X"":2545,""Y"":1007},{""X"":2546,""Y"":1033}],""Confidence"":0.918},{""Text"":""C"",""BoundingPolygon"":[{""X"":2499,""Y"":999},{""X"":2499,""Y"":978},{""X"":2544,""Y"":975},{""X"":2544,""Y"":995}],""Confidence"":0.337},{""Text"":""2015"",""BoundingPolygon"":[{""X"":2499,""Y"":963},{""X"":2498,""Y"":890},{""X"":2541,""Y"":887},{""X"":2543,""Y"":960}],""Confidence"":0.979},{""Text"":""Wizards"",""BoundingPolygon"":[{""X"":2498,""Y"":882},{""X"":2496,""Y"":765},{""X"":2537,""Y"":763},{""X"":2540,""Y"":878}],""Confidence"":0.991},{""Text"":""of"",""BoundingPolygon"":[{""X"":2496,""Y"":756},{""X"":2496,""Y"":727},{""X"":2535,""Y"":725},{""X"":2536,""Y"":754}],""Confidence"":0.998},{""Text"":""the"",""BoundingPolygon"":[{""X"":2496,""Y"":718},{""X"":2496,""Y"":670},{""X"":2534,""Y"":668},{""X"":2535,""Y"":716}],""Confidence"":0.998},{""Text"":""Coast"",""BoundingPolygon"":[{""X"":2496,""Y"":661},{""X"":2496,""Y"":577},{""X"":2530,""Y"":576},{""X"":2533,""Y"":659}],""Confidence"":0.995}]}]}]";

		}

		private string LoadTestResultObject()
		{
			return @"
            
             {
    ""lines"": [
      {
        ""text"": ""Armor of Thorns"",
        ""boundingPolygon"": [
          {
            ""x"": 1354,
            ""y"": 189
          },
          {
            ""x"": 1348,
            ""y"": 281
          },
          {
            ""x"": 1335,
            ""y"": 280
          },
          {
            ""x"": 1342,
            ""y"": 188
          }
        ],
        ""words"": [
          {
            ""text"": ""Armor"",
            ""boundingPolygon"": [
              {
                ""x"": 1353,
                ""y"": 191
              },
              {
                ""x"": 1353,
                ""y"": 222
              },
              {
                ""x"": 1342,
                ""y"": 221
              },
              {
                ""x"": 1342,
                ""y"": 190
              }
            ],
            ""confidence"": 0.94
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 1353,
                ""y"": 224
              },
              {
                ""x"": 1352,
                ""y"": 234
              },
              {
                ""x"": 1342,
                ""y"": 233
              },
              {
                ""x"": 1342,
                ""y"": 223
              }
            ],
            ""confidence"": 0.937
          },
          {
            ""text"": ""Thorns"",
            ""boundingPolygon"": [
              {
                ""x"": 1352,
                ""y"": 236
              },
              {
                ""x"": 1348,
                ""y"": 267
              },
              {
                ""x"": 1338,
                ""y"": 267
              },
              {
                ""x"": 1342,
                ""y"": 235
              }
            ],
            ""confidence"": 0.761
          }
        ]
      },
      {
        ""text"": ""Regeneration"",
        ""boundingPolygon"": [
          {
            ""x"": 1341,
            ""y"": 463
          },
          {
            ""x"": 1339,
            ""y"": 525
          },
          {
            ""x"": 1327,
            ""y"": 524
          },
          {
            ""x"": 1329,
            ""y"": 463
          }
        ],
        ""words"": [
          {
            ""text"": ""Regeneration"",
            ""boundingPolygon"": [
              {
                ""x"": 1341,
                ""y"": 464
              },
              {
                ""x"": 1339,
                ""y"": 518
              },
              {
                ""x"": 1328,
                ""y"": 519
              },
              {
                ""x"": 1329,
                ""y"": 463
              }
            ],
            ""confidence"": 0.721
          }
        ]
      },
      {
        ""text"": ""Sparring Construct"",
        ""boundingPolygon"": [
          {
            ""x"": 1367,
            ""y"": 1250
          },
          {
            ""x"": 1362,
            ""y"": 1337
          },
          {
            ""x"": 1350,
            ""y"": 1336
          },
          {
            ""x"": 1354,
            ""y"": 1250
          }
        ],
        ""words"": [
          {
            ""text"": ""Sparring"",
            ""boundingPolygon"": [
              {
                ""x"": 1368,
                ""y"": 1250
              },
              {
                ""x"": 1366,
                ""y"": 1285
              },
              {
                ""x"": 1353,
                ""y"": 1286
              },
              {
                ""x"": 1354,
                ""y"": 1250
              }
            ],
            ""confidence"": 0.975
          },
          {
            ""text"": ""Construct"",
            ""boundingPolygon"": [
              {
                ""x"": 1365,
                ""y"": 1287
              },
              {
                ""x"": 1362,
                ""y"": 1332
              },
              {
                ""x"": 1350,
                ""y"": 1334
              },
              {
                ""x"": 1353,
                ""y"": 1288
              }
            ],
            ""confidence"": 0.829
          }
        ]
      },
      {
        ""text"": ""Cartouche of Strength"",
        ""boundingPolygon"": [
          {
            ""x"": 1331,
            ""y"": 719
          },
          {
            ""x"": 1327,
            ""y"": 824
          },
          {
            ""x"": 1314,
            ""y"": 823
          },
          {
            ""x"": 1317,
            ""y"": 719
          }
        ],
        ""words"": [
          {
            ""text"": ""Cartouche"",
            ""boundingPolygon"": [
              {
                ""x"": 1331,
                ""y"": 719
              },
              {
                ""x"": 1330,
                ""y"": 762
              },
              {
                ""x"": 1316,
                ""y"": 763
              },
              {
                ""x"": 1317,
                ""y"": 721
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 1330,
                ""y"": 764
              },
              {
                ""x"": 1330,
                ""y"": 774
              },
              {
                ""x"": 1315,
                ""y"": 774
              },
              {
                ""x"": 1316,
                ""y"": 765
              }
            ],
            ""confidence"": 0.975
          },
          {
            ""text"": ""Strength"",
            ""boundingPolygon"": [
              {
                ""x"": 1329,
                ""y"": 776
              },
              {
                ""x"": 1328,
                ""y"": 816
              },
              {
                ""x"": 1314,
                ""y"": 816
              },
              {
                ""x"": 1315,
                ""y"": 777
              }
            ],
            ""confidence"": 0.957
          }
        ]
      },
      {
        ""text"": ""Drainpipe Vermin"",
        ""boundingPolygon"": [
          {
            ""x"": 1359,
            ""y"": 1464
          },
          {
            ""x"": 1362,
            ""y"": 1583
          },
          {
            ""x"": 1348,
            ""y"": 1583
          },
          {
            ""x"": 1345,
            ""y"": 1464
          }
        ],
        ""words"": [
          {
            ""text"": ""Drainpipe"",
            ""boundingPolygon"": [
              {
                ""x"": 1360,
                ""y"": 1470
              },
              {
                ""x"": 1357,
                ""y"": 1511
              },
              {
                ""x"": 1346,
                ""y"": 1510
              },
              {
                ""x"": 1346,
                ""y"": 1468
              }
            ],
            ""confidence"": 0.961
          },
          {
            ""text"": ""Vermin"",
            ""boundingPolygon"": [
              {
                ""x"": 1357,
                ""y"": 1513
              },
              {
                ""x"": 1358,
                ""y"": 1544
              },
              {
                ""x"": 1347,
                ""y"": 1543
              },
              {
                ""x"": 1346,
                ""y"": 1512
              }
            ],
            ""confidence"": 0.988
          }
        ]
      },
      {
        ""text"": ""Darksteel Axe"",
        ""boundingPolygon"": [
          {
            ""x"": 1373,
            ""y"": 1681
          },
          {
            ""x"": 1375,
            ""y"": 1749
          },
          {
            ""x"": 1362,
            ""y"": 1749
          },
          {
            ""x"": 1361,
            ""y"": 1681
          }
        ],
        ""words"": [
          {
            ""text"": ""Darksteel"",
            ""boundingPolygon"": [
              {
                ""x"": 1374,
                ""y"": 1688
              },
              {
                ""x"": 1374,
                ""y"": 1727
              },
              {
                ""x"": 1362,
                ""y"": 1728
              },
              {
                ""x"": 1362,
                ""y"": 1688
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""Axe"",
            ""boundingPolygon"": [
              {
                ""x"": 1374,
                ""y"": 1730
              },
              {
                ""x"": 1376,
                ""y"": 1747
              },
              {
                ""x"": 1364,
                ""y"": 1748
              },
              {
                ""x"": 1363,
                ""y"": 1731
              }
            ],
            ""confidence"": 0.959
          }
        ]
      },
      {
        ""text"": ""20)"",
        ""boundingPolygon"": [
          {
            ""x"": 1325,
            ""y"": 850
          },
          {
            ""x"": 1325,
            ""y"": 880
          },
          {
            ""x"": 1314,
            ""y"": 879
          },
          {
            ""x"": 1315,
            ""y"": 849
          }
        ],
        ""words"": [
          {
            ""text"": ""20)"",
            ""boundingPolygon"": [
              {
                ""x"": 1325,
                ""y"": 857
              },
              {
                ""x"": 1325,
                ""y"": 880
              },
              {
                ""x"": 1314,
                ""y"": 880
              },
              {
                ""x"": 1314,
                ""y"": 856
              }
            ],
            ""confidence"": 0.092
          }
        ]
      },
      {
        ""text"": ""Urban Burgeoning"",
        ""boundingPolygon"": [
          {
            ""x"": 1324,
            ""y"": 981
          },
          {
            ""x"": 1323,
            ""y"": 1072
          },
          {
            ""x"": 1311,
            ""y"": 1071
          },
          {
            ""x"": 1311,
            ""y"": 981
          }
        ],
        ""words"": [
          {
            ""text"": ""Urban"",
            ""boundingPolygon"": [
              {
                ""x"": 1324,
                ""y"": 983
              },
              {
                ""x"": 1324,
                ""y"": 1010
              },
              {
                ""x"": 1311,
                ""y"": 1011
              },
              {
                ""x"": 1311,
                ""y"": 984
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""Burgeoning"",
            ""boundingPolygon"": [
              {
                ""x"": 1323,
                ""y"": 1013
              },
              {
                ""x"": 1323,
                ""y"": 1064
              },
              {
                ""x"": 1311,
                ""y"": 1066
              },
              {
                ""x"": 1311,
                ""y"": 1014
              }
            ],
            ""confidence"": 0.965
          }
        ]
      },
      {
        ""text"": ""Enchant Creature"",
        ""boundingPolygon"": [
          {
            ""x"": 1220,
            ""y"": 186
          },
          {
            ""x"": 1222,
            ""y"": 268
          },
          {
            ""x"": 1212,
            ""y"": 268
          },
          {
            ""x"": 1210,
            ""y"": 186
          }
        ],
        ""words"": [
          {
            ""text"": ""Enchant"",
            ""boundingPolygon"": [
              {
                ""x"": 1221,
                ""y"": 187
              },
              {
                ""x"": 1221,
                ""y"": 219
              },
              {
                ""x"": 1212,
                ""y"": 219
              },
              {
                ""x"": 1211,
                ""y"": 186
              }
            ],
            ""confidence"": 0.965
          },
          {
            ""text"": ""Creature"",
            ""boundingPolygon"": [
              {
                ""x"": 1221,
                ""y"": 221
              },
              {
                ""x"": 1222,
                ""y"": 254
              },
              {
                ""x"": 1212,
                ""y"": 256
              },
              {
                ""x"": 1212,
                ""y"": 221
              }
            ],
            ""confidence"": 0.72
          }
        ]
      },
      {
        ""text"": ""You may choose to play"",
        ""boundingPolygon"": [
          {
            ""x"": 1201,
            ""y"": 189
          },
          {
            ""x"": 1202,
            ""y"": 296
          },
          {
            ""x"": 1189,
            ""y"": 296
          },
          {
            ""x"": 1189,
            ""y"": 189
          }
        ],
        ""words"": [
          {
            ""text"": ""You"",
            ""boundingPolygon"": [
              {
                ""x"": 1202,
                ""y"": 190
              },
              {
                ""x"": 1200,
                ""y"": 205
              },
              {
                ""x"": 1189,
                ""y"": 205
              },
              {
                ""x"": 1190,
                ""y"": 190
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""may"",
            ""boundingPolygon"": [
              {
                ""x"": 1200,
                ""y"": 209
              },
              {
                ""x"": 1199,
                ""y"": 227
              },
              {
                ""x"": 1189,
                ""y"": 227
              },
              {
                ""x"": 1189,
                ""y"": 209
              }
            ],
            ""confidence"": 0.958
          },
          {
            ""text"": ""choose"",
            ""boundingPolygon"": [
              {
                ""x"": 1199,
                ""y"": 230
              },
              {
                ""x"": 1199,
                ""y"": 259
              },
              {
                ""x"": 1190,
                ""y"": 259
              },
              {
                ""x"": 1189,
                ""y"": 230
              }
            ],
            ""confidence"": 0.988
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 1199,
                ""y"": 262
              },
              {
                ""x"": 1199,
                ""y"": 271
              },
              {
                ""x"": 1190,
                ""y"": 271
              },
              {
                ""x"": 1190,
                ""y"": 262
              }
            ],
            ""confidence"": 0.996
          },
          {
            ""text"": ""play"",
            ""boundingPolygon"": [
              {
                ""x"": 1199,
                ""y"": 273
              },
              {
                ""x"": 1201,
                ""y"": 291
              },
              {
                ""x"": 1191,
                ""y"": 291
              },
              {
                ""x"": 1190,
                ""y"": 273
              }
            ],
            ""confidence"": 0.916
          }
        ]
      },
      {
        ""text"": ""Armor of"",
        ""boundingPolygon"": [
          {
            ""x"": 1199,
            ""y"": 295
          },
          {
            ""x"": 1200,
            ""y"": 333
          },
          {
            ""x"": 1189,
            ""y"": 334
          },
          {
            ""x"": 1188,
            ""y"": 295
          }
        ],
        ""words"": [
          {
            ""text"": ""Armor"",
            ""boundingPolygon"": [
              {
                ""x"": 1199,
                ""y"": 295
              },
              {
                ""x"": 1200,
                ""y"": 321
              },
              {
                ""x"": 1189,
                ""y"": 322
              },
              {
                ""x"": 1188,
                ""y"": 295
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 1200,
                ""y"": 323
              },
              {
                ""x"": 1200,
                ""y"": 333
              },
              {
                ""x"": 1190,
                ""y"": 334
              },
              {
                ""x"": 1189,
                ""y"": 324
              }
            ],
            ""confidence"": 0.959
          }
        ]
      },
      {
        ""text"": ""Enchant Crearun"",
        ""boundingPolygon"": [
          {
            ""x"": 1214,
            ""y"": 457
          },
          {
            ""x"": 1219,
            ""y"": 545
          },
          {
            ""x"": 1205,
            ""y"": 546
          },
          {
            ""x"": 1200,
            ""y"": 458
          }
        ],
        ""words"": [
          {
            ""text"": ""Enchant"",
            ""boundingPolygon"": [
              {
                ""x"": 1213,
                ""y"": 458
              },
              {
                ""x"": 1214,
                ""y"": 486
              },
              {
                ""x"": 1202,
                ""y"": 487
              },
              {
                ""x"": 1201,
                ""y"": 459
              }
            ],
            ""confidence"": 0.893
          },
          {
            ""text"": ""Crearun"",
            ""boundingPolygon"": [
              {
                ""x"": 1215,
                ""y"": 489
              },
              {
                ""x"": 1217,
                ""y"": 521
              },
              {
                ""x"": 1203,
                ""y"": 522
              },
              {
                ""x"": 1202,
                ""y"": 490
              }
            ],
            ""confidence"": 0.28
          }
        ]
      },
      {
        ""text"": ""Artifact Creature - Construct"",
        ""boundingPolygon"": [
          {
            ""x"": 1235,
            ""y"": 1240
          },
          {
            ""x"": 1229,
            ""y"": 1363
          },
          {
            ""x"": 1215,
            ""y"": 1362
          },
          {
            ""x"": 1220,
            ""y"": 1239
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 1235,
                ""y"": 1241
              },
              {
                ""x"": 1233,
                ""y"": 1269
              },
              {
                ""x"": 1219,
                ""y"": 1268
              },
              {
                ""x"": 1220,
                ""y"": 1240
              }
            ],
            ""confidence"": 0.597
          },
          {
            ""text"": ""Creature"",
            ""boundingPolygon"": [
              {
                ""x"": 1232,
                ""y"": 1272
              },
              {
                ""x"": 1231,
                ""y"": 1302
              },
              {
                ""x"": 1218,
                ""y"": 1303
              },
              {
                ""x"": 1219,
                ""y"": 1271
              }
            ],
            ""confidence"": 0.692
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 1231,
                ""y"": 1305
              },
              {
                ""x"": 1230,
                ""y"": 1311
              },
              {
                ""x"": 1217,
                ""y"": 1312
              },
              {
                ""x"": 1218,
                ""y"": 1306
              }
            ],
            ""confidence"": 0.942
          },
          {
            ""text"": ""Construct"",
            ""boundingPolygon"": [
              {
                ""x"": 1230,
                ""y"": 1314
              },
              {
                ""x"": 1230,
                ""y"": 1354
              },
              {
                ""x"": 1215,
                ""y"": 1356
              },
              {
                ""x"": 1217,
                ""y"": 1315
              }
            ],
            ""confidence"": 0.594
          }
        ]
      },
      {
        ""text"": ""bury an instant if you do,"",
        ""boundingPolygon"": [
          {
            ""x"": 1187,
            ""y"": 187
          },
          {
            ""x"": 1190,
            ""y"": 327
          },
          {
            ""x"": 1177,
            ""y"": 328
          },
          {
            ""x"": 1174,
            ""y"": 187
          }
        ],
        ""words"": [
          {
            ""text"": ""bury"",
            ""boundingPolygon"": [
              {
                ""x"": 1186,
                ""y"": 189
              },
              {
                ""x"": 1187,
                ""y"": 210
              },
              {
                ""x"": 1176,
                ""y"": 210
              },
              {
                ""x"": 1174,
                ""y"": 188
              }
            ],
            ""confidence"": 0.628
          },
          {
            ""text"": ""an"",
            ""boundingPolygon"": [
              {
                ""x"": 1188,
                ""y"": 224
              },
              {
                ""x"": 1189,
                ""y"": 246
              },
              {
                ""x"": 1178,
                ""y"": 245
              },
              {
                ""x"": 1177,
                ""y"": 224
              }
            ],
            ""confidence"": 0.125
          },
          {
            ""text"": ""instant"",
            ""boundingPolygon"": [
              {
                ""x"": 1189,
                ""y"": 248
              },
              {
                ""x"": 1189,
                ""y"": 280
              },
              {
                ""x"": 1179,
                ""y"": 280
              },
              {
                ""x"": 1178,
                ""y"": 247
              }
            ],
            ""confidence"": 0.15
          },
          {
            ""text"": ""if"",
            ""boundingPolygon"": [
              {
                ""x"": 1189,
                ""y"": 282
              },
              {
                ""x"": 1189,
                ""y"": 289
              },
              {
                ""x"": 1179,
                ""y"": 288
              },
              {
                ""x"": 1179,
                ""y"": 282
              }
            ],
            ""confidence"": 0.711
          },
          {
            ""text"": ""you"",
            ""boundingPolygon"": [
              {
                ""x"": 1189,
                ""y"": 291
              },
              {
                ""x"": 1189,
                ""y"": 306
              },
              {
                ""x"": 1179,
                ""y"": 306
              },
              {
                ""x"": 1179,
                ""y"": 291
              }
            ],
            ""confidence"": 0.995
          },
          {
            ""text"": ""do,"",
            ""boundingPolygon"": [
              {
                ""x"": 1189,
                ""y"": 310
              },
              {
                ""x"": 1189,
                ""y"": 326
              },
              {
                ""x"": 1179,
                ""y"": 326
              },
              {
                ""x"": 1179,
                ""y"": 309
              }
            ],
            ""confidence"": 0.863
          }
        ]
      },
      {
        ""text"": ""bury it at end of turn."",
        ""boundingPolygon"": [
          {
            ""x"": 1181,
            ""y"": 187
          },
          {
            ""x"": 1181,
            ""y"": 288
          },
          {
            ""x"": 1169,
            ""y"": 288
          },
          {
            ""x"": 1169,
            ""y"": 187
          }
        ],
        ""words"": [
          {
            ""text"": ""bury"",
            ""boundingPolygon"": [
              {
                ""x"": 1181,
                ""y"": 188
              },
              {
                ""x"": 1181,
                ""y"": 209
              },
              {
                ""x"": 1170,
                ""y"": 209
              },
              {
                ""x"": 1169,
                ""y"": 188
              }
            ],
            ""confidence"": 0.956
          },
          {
            ""text"": ""it"",
            ""boundingPolygon"": [
              {
                ""x"": 1181,
                ""y"": 211
              },
              {
                ""x"": 1182,
                ""y"": 218
              },
              {
                ""x"": 1170,
                ""y"": 218
              },
              {
                ""x"": 1170,
                ""y"": 211
              }
            ],
            ""confidence"": 0.997
          },
          {
            ""text"": ""at"",
            ""boundingPolygon"": [
              {
                ""x"": 1182,
                ""y"": 220
              },
              {
                ""x"": 1182,
                ""y"": 229
              },
              {
                ""x"": 1170,
                ""y"": 228
              },
              {
                ""x"": 1170,
                ""y"": 220
              }
            ],
            ""confidence"": 0.789
          },
          {
            ""text"": ""end"",
            ""boundingPolygon"": [
              {
                ""x"": 1182,
                ""y"": 231
              },
              {
                ""x"": 1182,
                ""y"": 248
              },
              {
                ""x"": 1170,
                ""y"": 247
              },
              {
                ""x"": 1170,
                ""y"": 231
              }
            ],
            ""confidence"": 0.997
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 1182,
                ""y"": 250
              },
              {
                ""x"": 1182,
                ""y"": 259
              },
              {
                ""x"": 1170,
                ""y"": 258
              },
              {
                ""x"": 1170,
                ""y"": 250
              }
            ],
            ""confidence"": 0.991
          },
          {
            ""text"": ""turn."",
            ""boundingPolygon"": [
              {
                ""x"": 1182,
                ""y"": 261
              },
              {
                ""x"": 1181,
                ""y"": 285
              },
              {
                ""x"": 1169,
                ""y"": 284
              },
              {
                ""x"": 1170,
                ""y"": 260
              }
            ],
            ""confidence"": 0.855
          }
        ]
      },
      {
        ""text"": ""Creature - Rat"",
        ""boundingPolygon"": [
          {
            ""x"": 1228,
            ""y"": 1475
          },
          {
            ""x"": 1228,
            ""y"": 1542
          },
          {
            ""x"": 1218,
            ""y"": 1542
          },
          {
            ""x"": 1218,
            ""y"": 1475
          }
        ],
        ""words"": [
          {
            ""text"": ""Creature"",
            ""boundingPolygon"": [
              {
                ""x"": 1228,
                ""y"": 1477
              },
              {
                ""x"": 1229,
                ""y"": 1508
              },
              {
                ""x"": 1218,
                ""y"": 1509
              },
              {
                ""x"": 1218,
                ""y"": 1478
              }
            ],
            ""confidence"": 0.957
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 1229,
                ""y"": 1510
              },
              {
                ""x"": 1229,
                ""y"": 1516
              },
              {
                ""x"": 1218,
                ""y"": 1517
              },
              {
                ""x"": 1218,
                ""y"": 1511
              }
            ],
            ""confidence"": 0.875
          },
          {
            ""text"": ""Rat"",
            ""boundingPolygon"": [
              {
                ""x"": 1229,
                ""y"": 1519
              },
              {
                ""x"": 1229,
                ""y"": 1534
              },
              {
                ""x"": 1218,
                ""y"": 1535
              },
              {
                ""x"": 1218,
                ""y"": 1520
              }
            ],
            ""confidence"": 0.767
          }
        ]
      },
      {
        ""text"": ""e: Regenerate enchanted creature."",
        ""boundingPolygon"": [
          {
            ""x"": 1186,
            ""y"": 463
          },
          {
            ""x"": 1181,
            ""y"": 601
          },
          {
            ""x"": 1168,
            ""y"": 601
          },
          {
            ""x"": 1172,
            ""y"": 463
          }
        ],
        ""words"": [
          {
            ""text"": ""e:"",
            ""boundingPolygon"": [
              {
                ""x"": 1186,
                ""y"": 464
              },
              {
                ""x"": 1186,
                ""y"": 471
              },
              {
                ""x"": 1172,
                ""y"": 471
              },
              {
                ""x"": 1172,
                ""y"": 464
              }
            ],
            ""confidence"": 0.137
          },
          {
            ""text"": ""Regenerate"",
            ""boundingPolygon"": [
              {
                ""x"": 1186,
                ""y"": 473
              },
              {
                ""x"": 1185,
                ""y"": 518
              },
              {
                ""x"": 1172,
                ""y"": 518
              },
              {
                ""x"": 1172,
                ""y"": 473
              }
            ],
            ""confidence"": 0.914
          },
          {
            ""text"": ""enchanted"",
            ""boundingPolygon"": [
              {
                ""x"": 1185,
                ""y"": 521
              },
              {
                ""x"": 1182,
                ""y"": 563
              },
              {
                ""x"": 1171,
                ""y"": 563
              },
              {
                ""x"": 1172,
                ""y"": 521
              }
            ],
            ""confidence"": 0.839
          },
          {
            ""text"": ""creature."",
            ""boundingPolygon"": [
              {
                ""x"": 1182,
                ""y"": 565
              },
              {
                ""x"": 1179,
                ""y"": 601
              },
              {
                ""x"": 1169,
                ""y"": 601
              },
              {
                ""x"": 1170,
                ""y"": 565
              }
            ],
            ""confidence"": 0.673
          }
        ]
      },
      {
        ""text"": ""Enchantment - Aura Cartouche"",
        ""boundingPolygon"": [
          {
            ""x"": 1198,
            ""y"": 709
          },
          {
            ""x"": 1196,
            ""y"": 838
          },
          {
            ""x"": 1185,
            ""y"": 838
          },
          {
            ""x"": 1186,
            ""y"": 709
          }
        ],
        ""words"": [
          {
            ""text"": ""Enchantment"",
            ""boundingPolygon"": [
              {
                ""x"": 1199,
                ""y"": 712
              },
              {
                ""x"": 1198,
                ""y"": 761
              },
              {
                ""x"": 1186,
                ""y"": 762
              },
              {
                ""x"": 1186,
                ""y"": 711
              }
            ],
            ""confidence"": 0.962
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 1198,
                ""y"": 764
              },
              {
                ""x"": 1197,
                ""y"": 769
              },
              {
                ""x"": 1186,
                ""y"": 770
              },
              {
                ""x"": 1186,
                ""y"": 764
              }
            ],
            ""confidence"": 0.92
          },
          {
            ""text"": ""Aura"",
            ""boundingPolygon"": [
              {
                ""x"": 1197,
                ""y"": 772
              },
              {
                ""x"": 1197,
                ""y"": 790
              },
              {
                ""x"": 1186,
                ""y"": 791
              },
              {
                ""x"": 1186,
                ""y"": 773
              }
            ],
            ""confidence"": 0.984
          },
          {
            ""text"": ""Cartouche"",
            ""boundingPolygon"": [
              {
                ""x"": 1197,
                ""y"": 792
              },
              {
                ""x"": 1195,
                ""y"": 832
              },
              {
                ""x"": 1185,
                ""y"": 834
              },
              {
                ""x"": 1186,
                ""y"": 794
              }
            ],
            ""confidence"": 0.984
          }
        ]
      },
      {
        ""text"": ""Sparring Construct diese"",
        ""boundingPolygon"": [
          {
            ""x"": 1221,
            ""y"": 1264
          },
          {
            ""x"": 1221,
            ""y"": 1397
          },
          {
            ""x"": 1205,
            ""y"": 1397
          },
          {
            ""x"": 1205,
            ""y"": 1264
          }
        ],
        ""words"": [
          {
            ""text"": ""Sparring"",
            ""boundingPolygon"": [
              {
                ""x"": 1221,
                ""y"": 1269
              },
              {
                ""x"": 1220,
                ""y"": 1304
              },
              {
                ""x"": 1205,
                ""y"": 1303
              },
              {
                ""x"": 1206,
                ""y"": 1268
              }
            ],
            ""confidence"": 0.968
          },
          {
            ""text"": ""Construct"",
            ""boundingPolygon"": [
              {
                ""x"": 1220,
                ""y"": 1307
              },
              {
                ""x"": 1219,
                ""y"": 1347
              },
              {
                ""x"": 1205,
                ""y"": 1347
              },
              {
                ""x"": 1205,
                ""y"": 1306
              }
            ],
            ""confidence"": 0.938
          },
          {
            ""text"": ""diese"",
            ""boundingPolygon"": [
              {
                ""x"": 1219,
                ""y"": 1350
              },
              {
                ""x"": 1220,
                ""y"": 1388
              },
              {
                ""x"": 1207,
                ""y"": 1388
              },
              {
                ""x"": 1205,
                ""y"": 1350
              }
            ],
            ""confidence"": 0.155
          }
        ]
      },
      {
        ""text"": ""Artifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 1245,
            ""y"": 1698
          },
          {
            ""x"": 1246,
            ""y"": 1781
          },
          {
            ""x"": 1232,
            ""y"": 1781
          },
          {
            ""x"": 1232,
            ""y"": 1698
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 1245,
                ""y"": 1701
              },
              {
                ""x"": 1245,
                ""y"": 1728
              },
              {
                ""x"": 1233,
                ""y"": 1728
              },
              {
                ""x"": 1233,
                ""y"": 1701
              }
            ],
            ""confidence"": 0.614
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 1245,
                ""y"": 1730
              },
              {
                ""x"": 1245,
                ""y"": 1737
              },
              {
                ""x"": 1233,
                ""y"": 1737
              },
              {
                ""x"": 1233,
                ""y"": 1730
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 1245,
                ""y"": 1739
              },
              {
                ""x"": 1247,
                ""y"": 1781
              },
              {
                ""x"": 1233,
                ""y"": 1782
              },
              {
                ""x"": 1233,
                ""y"": 1740
              }
            ],
            ""confidence"": 0.61
          }
        ]
      },
      {
        ""text"": ""Plny only on a ponblack creatur"",
        ""boundingPolygon"": [
          {
            ""x"": 1175,
            ""y"": 189
          },
          {
            ""x"": 1171,
            ""y"": 325
          },
          {
            ""x"": 1156,
            ""y"": 324
          },
          {
            ""x"": 1162,
            ""y"": 188
          }
        ],
        ""words"": [
          {
            ""text"": ""Plny"",
            ""boundingPolygon"": [
              {
                ""x"": 1175,
                ""y"": 191
              },
              {
                ""x"": 1175,
                ""y"": 208
              },
              {
                ""x"": 1163,
                ""y"": 206
              },
              {
                ""x"": 1163,
                ""y"": 189
              }
            ],
            ""confidence"": 0.567
          },
          {
            ""text"": ""only"",
            ""boundingPolygon"": [
              {
                ""x"": 1175,
                ""y"": 210
              },
              {
                ""x"": 1174,
                ""y"": 229
              },
              {
                ""x"": 1163,
                ""y"": 228
              },
              {
                ""x"": 1163,
                ""y"": 208
              }
            ],
            ""confidence"": 0.736
          },
          {
            ""text"": ""on"",
            ""boundingPolygon"": [
              {
                ""x"": 1174,
                ""y"": 232
              },
              {
                ""x"": 1174,
                ""y"": 243
              },
              {
                ""x"": 1162,
                ""y"": 241
              },
              {
                ""x"": 1163,
                ""y"": 230
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 1174,
                ""y"": 245
              },
              {
                ""x"": 1174,
                ""y"": 250
              },
              {
                ""x"": 1162,
                ""y"": 249
              },
              {
                ""x"": 1162,
                ""y"": 244
              }
            ],
            ""confidence"": 0.898
          },
          {
            ""text"": ""ponblack"",
            ""boundingPolygon"": [
              {
                ""x"": 1174,
                ""y"": 253
              },
              {
                ""x"": 1172,
                ""y"": 292
              },
              {
                ""x"": 1160,
                ""y"": 291
              },
              {
                ""x"": 1162,
                ""y"": 251
              }
            ],
            ""confidence"": 0.848
          },
          {
            ""text"": ""creatur"",
            ""boundingPolygon"": [
              {
                ""x"": 1172,
                ""y"": 294
              },
              {
                ""x"": 1170,
                ""y"": 325
              },
              {
                ""x"": 1157,
                ""y"": 324
              },
              {
                ""x"": 1159,
                ""y"": 293
              }
            ],
            ""confidence"": 0.799
          }
        ]
      },
      {
        ""text"": ""\""Death is not a debt Lam yer teilling"",
        ""boundingPolygon"": [
          {
            ""x"": 1171,
            ""y"": 457
          },
          {
            ""x"": 1172,
            ""y"": 603
          },
          {
            ""x"": 1152,
            ""y"": 603
          },
          {
            ""x"": 1151,
            ""y"": 457
          }
        ],
        ""words"": [
          {
            ""text"": ""\""Death"",
            ""boundingPolygon"": [
              {
                ""x"": 1167,
                ""y"": 459
              },
              {
                ""x"": 1170,
                ""y"": 487
              },
              {
                ""x"": 1154,
                ""y"": 487
              },
              {
                ""x"": 1151,
                ""y"": 459
              }
            ],
            ""confidence"": 0.597
          },
          {
            ""text"": ""is"",
            ""boundingPolygon"": [
              {
                ""x"": 1170,
                ""y"": 490
              },
              {
                ""x"": 1171,
                ""y"": 495
              },
              {
                ""x"": 1155,
                ""y"": 496
              },
              {
                ""x"": 1155,
                ""y"": 491
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""not"",
            ""boundingPolygon"": [
              {
                ""x"": 1171,
                ""y"": 499
              },
              {
                ""x"": 1172,
                ""y"": 511
              },
              {
                ""x"": 1156,
                ""y"": 511
              },
              {
                ""x"": 1155,
                ""y"": 499
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 1172,
                ""y"": 514
              },
              {
                ""x"": 1172,
                ""y"": 518
              },
              {
                ""x"": 1156,
                ""y"": 519
              },
              {
                ""x"": 1156,
                ""y"": 514
              }
            ],
            ""confidence"": 0.975
          },
          {
            ""text"": ""debt"",
            ""boundingPolygon"": [
              {
                ""x"": 1172,
                ""y"": 521
              },
              {
                ""x"": 1172,
                ""y"": 537
              },
              {
                ""x"": 1156,
                ""y"": 537
              },
              {
                ""x"": 1156,
                ""y"": 522
              }
            ],
            ""confidence"": 0.658
          },
          {
            ""text"": ""Lam"",
            ""boundingPolygon"": [
              {
                ""x"": 1172,
                ""y"": 540
              },
              {
                ""x"": 1172,
                ""y"": 558
              },
              {
                ""x"": 1156,
                ""y"": 558
              },
              {
                ""x"": 1156,
                ""y"": 541
              }
            ],
            ""confidence"": 0.756
          },
          {
            ""text"": ""yer"",
            ""boundingPolygon"": [
              {
                ""x"": 1172,
                ""y"": 561
              },
              {
                ""x"": 1171,
                ""y"": 571
              },
              {
                ""x"": 1155,
                ""y"": 572
              },
              {
                ""x"": 1156,
                ""y"": 561
              }
            ],
            ""confidence"": 0.597
          },
          {
            ""text"": ""teilling"",
            ""boundingPolygon"": [
              {
                ""x"": 1171,
                ""y"": 574
              },
              {
                ""x"": 1168,
                ""y"": 602
              },
              {
                ""x"": 1152,
                ""y"": 603
              },
              {
                ""x"": 1155,
                ""y"": 575
              }
            ],
            ""confidence"": 0.712
          }
        ]
      },
      {
        ""text"": ""Enchant creature"",
        ""boundingPolygon"": [
          {
            ""x"": 1188,
            ""y"": 710
          },
          {
            ""x"": 1189,
            ""y"": 781
          },
          {
            ""x"": 1174,
            ""y"": 781
          },
          {
            ""x"": 1174,
            ""y"": 710
          }
        ],
        ""words"": [
          {
            ""text"": ""Enchant"",
            ""boundingPolygon"": [
              {
                ""x"": 1189,
                ""y"": 714
              },
              {
                ""x"": 1186,
                ""y"": 743
              },
              {
                ""x"": 1174,
                ""y"": 742
              },
              {
                ""x"": 1178,
                ""y"": 712
              }
            ],
            ""confidence"": 0.843
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 1186,
                ""y"": 746
              },
              {
                ""x"": 1188,
                ""y"": 777
              },
              {
                ""x"": 1177,
                ""y"": 777
              },
              {
                ""x"": 1174,
                ""y"": 745
              }
            ],
            ""confidence"": 0.552
          }
        ]
      },
      {
        ""text"": ""Enchant creature you control"",
        ""boundingPolygon"": [
          {
            ""x"": 1182,
            ""y"": 712
          },
          {
            ""x"": 1177,
            ""y"": 825
          },
          {
            ""x"": 1166,
            ""y"": 825
          },
          {
            ""x"": 1169,
            ""y"": 712
          }
        ],
        ""words"": [
          {
            ""text"": ""Enchant"",
            ""boundingPolygon"": [
              {
                ""x"": 1183,
                ""y"": 713
              },
              {
                ""x"": 1181,
                ""y"": 743
              },
              {
                ""x"": 1169,
                ""y"": 742
              },
              {
                ""x"": 1170,
                ""y"": 712
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 1181,
                ""y"": 745
              },
              {
                ""x"": 1179,
                ""y"": 774
              },
              {
                ""x"": 1168,
                ""y"": 773
              },
              {
                ""x"": 1169,
                ""y"": 744
              }
            ],
            ""confidence"": 0.911
          },
          {
            ""text"": ""you"",
            ""boundingPolygon"": [
              {
                ""x"": 1179,
                ""y"": 776
              },
              {
                ""x"": 1178,
                ""y"": 790
              },
              {
                ""x"": 1167,
                ""y"": 790
              },
              {
                ""x"": 1167,
                ""y"": 776
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""control"",
            ""boundingPolygon"": [
              {
                ""x"": 1178,
                ""y"": 792
              },
              {
                ""x"": 1177,
                ""y"": 821
              },
              {
                ""x"": 1166,
                ""y"": 821
              },
              {
                ""x"": 1167,
                ""y"": 792
              }
            ],
            ""confidence"": 0.917
          }
        ]
      },
      {
        ""text"": ""Enchantment - Aura"",
        ""boundingPolygon"": [
          {
            ""x"": 1195,
            ""y"": 984
          },
          {
            ""x"": 1195,
            ""y"": 1069
          },
          {
            ""x"": 1186,
            ""y"": 1069
          },
          {
            ""x"": 1186,
            ""y"": 984
          }
        ],
        ""words"": [
          {
            ""text"": ""Enchantment"",
            ""boundingPolygon"": [
              {
                ""x"": 1196,
                ""y"": 985
              },
              {
                ""x"": 1195,
                ""y"": 1032
              },
              {
                ""x"": 1187,
                ""y"": 1034
              },
              {
                ""x"": 1186,
                ""y"": 986
              }
            ],
            ""confidence"": 0.976
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 1195,
                ""y"": 1034
              },
              {
                ""x"": 1195,
                ""y"": 1039
              },
              {
                ""x"": 1187,
                ""y"": 1041
              },
              {
                ""x"": 1187,
                ""y"": 1035
              }
            ],
            ""confidence"": 0.729
          },
          {
            ""text"": ""Aura"",
            ""boundingPolygon"": [
              {
                ""x"": 1195,
                ""y"": 1044
              },
              {
                ""x"": 1195,
                ""y"": 1061
              },
              {
                ""x"": 1187,
                ""y"": 1063
              },
              {
                ""x"": 1187,
                ""y"": 1046
              }
            ],
            ""confidence"": 0.846
          }
        ]
      },
      {
        ""text"": ""+1 counter ononstruc die put a"",
        ""boundingPolygon"": [
          {
            ""x"": 1211,
            ""y"": 1255
          },
          {
            ""x"": 1212,
            ""y"": 1399
          },
          {
            ""x"": 1197,
            ""y"": 1399
          },
          {
            ""x"": 1197,
            ""y"": 1255
          }
        ],
        ""words"": [
          {
            ""text"": ""+1"",
            ""boundingPolygon"": [
              {
                ""x"": 1211,
                ""y"": 1256
              },
              {
                ""x"": 1211,
                ""y"": 1266
              },
              {
                ""x"": 1197,
                ""y"": 1265
              },
              {
                ""x"": 1197,
                ""y"": 1255
              }
            ],
            ""confidence"": 0.999
          },
          {
            ""text"": ""counter"",
            ""boundingPolygon"": [
              {
                ""x"": 1211,
                ""y"": 1268
              },
              {
                ""x"": 1212,
                ""y"": 1301
              },
              {
                ""x"": 1198,
                ""y"": 1300
              },
              {
                ""x"": 1197,
                ""y"": 1268
              }
            ],
            ""confidence"": 0.999
          },
          {
            ""text"": ""ononstruc"",
            ""boundingPolygon"": [
              {
                ""x"": 1212,
                ""y"": 1303
              },
              {
                ""x"": 1212,
                ""y"": 1347
              },
              {
                ""x"": 1199,
                ""y"": 1346
              },
              {
                ""x"": 1198,
                ""y"": 1302
              }
            ],
            ""confidence"": 0.132
          },
          {
            ""text"": ""die"",
            ""boundingPolygon"": [
              {
                ""x"": 1212,
                ""y"": 1350
              },
              {
                ""x"": 1212,
                ""y"": 1365
              },
              {
                ""x"": 1198,
                ""y"": 1364
              },
              {
                ""x"": 1199,
                ""y"": 1349
              }
            ],
            ""confidence"": 0.639
          },
          {
            ""text"": ""put"",
            ""boundingPolygon"": [
              {
                ""x"": 1212,
                ""y"": 1370
              },
              {
                ""x"": 1212,
                ""y"": 1385
              },
              {
                ""x"": 1198,
                ""y"": 1384
              },
              {
                ""x"": 1198,
                ""y"": 1369
              }
            ],
            ""confidence"": 0.806
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 1212,
                ""y"": 1387
              },
              {
                ""x"": 1211,
                ""y"": 1394
              },
              {
                ""x"": 1198,
                ""y"": 1393
              },
              {
                ""x"": 1198,
                ""y"": 1386
              }
            ],
            ""confidence"": 0.808
          }
        ]
      },
      {
        ""text"": ""Indestructible"",
        ""boundingPolygon"": [
          {
            ""x"": 1220,
            ""y"": 1703
          },
          {
            ""x"": 1222,
            ""y"": 1766
          },
          {
            ""x"": 1209,
            ""y"": 1766
          },
          {
            ""x"": 1208,
            ""y"": 1703
          }
        ],
        ""words"": [
          {
            ""text"": ""Indestructible"",
            ""boundingPolygon"": [
              {
                ""x"": 1221,
                ""y"": 1704
              },
              {
                ""x"": 1222,
                ""y"": 1761
              },
              {
                ""x"": 1209,
                ""y"": 1762
              },
              {
                ""x"": 1208,
                ""y"": 1704
              }
            ],
            ""confidence"": 0.937
          }
        ]
      },
      {
        ""text"": ""Enchanted creature gets +2/+2"",
        ""boundingPolygon"": [
          {
            ""x"": 1160,
            ""y"": 186
          },
          {
            ""x"": 1157,
            ""y"": 322
          },
          {
            ""x"": 1146,
            ""y"": 322
          },
          {
            ""x"": 1149,
            ""y"": 186
          }
        ],
        ""words"": [
          {
            ""text"": ""Enchanted"",
            ""boundingPolygon"": [
              {
                ""x"": 1159,
                ""y"": 188
              },
              {
                ""x"": 1158,
                ""y"": 234
              },
              {
                ""x"": 1148,
                ""y"": 234
              },
              {
                ""x"": 1150,
                ""y"": 188
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 1158,
                ""y"": 237
              },
              {
                ""x"": 1157,
                ""y"": 273
              },
              {
                ""x"": 1147,
                ""y"": 273
              },
              {
                ""x"": 1148,
                ""y"": 237
              }
            ],
            ""confidence"": 0.588
          },
          {
            ""text"": ""gets"",
            ""boundingPolygon"": [
              {
                ""x"": 1157,
                ""y"": 275
              },
              {
                ""x"": 1157,
                ""y"": 293
              },
              {
                ""x"": 1147,
                ""y"": 293
              },
              {
                ""x"": 1147,
                ""y"": 275
              }
            ],
            ""confidence"": 0.962
          },
          {
            ""text"": ""+2/+2"",
            ""boundingPolygon"": [
              {
                ""x"": 1157,
                ""y"": 295
              },
              {
                ""x"": 1157,
                ""y"": 321
              },
              {
                ""x"": 1146,
                ""y"": 321
              },
              {
                ""x"": 1147,
                ""y"": 295
              }
            ],
            ""confidence"": 0.93
          }
        ]
      },
      {
        ""text"": ""Then Cartouche of Strength enterse"",
        ""boundingPolygon"": [
          {
            ""x"": 1175,
            ""y"": 719
          },
          {
            ""x"": 1166,
            ""y"": 859
          },
          {
            ""x"": 1150,
            ""y"": 857
          },
          {
            ""x"": 1161,
            ""y"": 718
          }
        ],
        ""words"": [
          {
            ""text"": ""Then"",
            ""boundingPolygon"": [
              {
                ""x"": 1174,
                ""y"": 719
              },
              {
                ""x"": 1174,
                ""y"": 734
              },
              {
                ""x"": 1161,
                ""y"": 733
              },
              {
                ""x"": 1161,
                ""y"": 718
              }
            ],
            ""confidence"": 0.326
          },
          {
            ""text"": ""Cartouche"",
            ""boundingPolygon"": [
              {
                ""x"": 1174,
                ""y"": 736
              },
              {
                ""x"": 1173,
                ""y"": 774
              },
              {
                ""x"": 1161,
                ""y"": 773
              },
              {
                ""x"": 1161,
                ""y"": 735
              }
            ],
            ""confidence"": 0.957
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 1173,
                ""y"": 777
              },
              {
                ""x"": 1173,
                ""y"": 785
              },
              {
                ""x"": 1160,
                ""y"": 784
              },
              {
                ""x"": 1161,
                ""y"": 776
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""Strength"",
            ""boundingPolygon"": [
              {
                ""x"": 1172,
                ""y"": 787
              },
              {
                ""x"": 1169,
                ""y"": 818
              },
              {
                ""x"": 1157,
                ""y"": 817
              },
              {
                ""x"": 1160,
                ""y"": 786
              }
            ],
            ""confidence"": 0.882
          },
          {
            ""text"": ""enterse"",
            ""boundingPolygon"": [
              {
                ""x"": 1168,
                ""y"": 820
              },
              {
                ""x"": 1162,
                ""y"": 854
              },
              {
                ""x"": 1151,
                ""y"": 853
              },
              {
                ""x"": 1157,
                ""y"": 820
              }
            ],
            ""confidence"": 0.186
          }
        ]
      },
      {
        ""text"": ""The train"",
        ""boundingPolygon"": [
          {
            ""x"": 1189,
            ""y"": 1239
          },
          {
            ""x"": 1185,
            ""y"": 1273
          },
          {
            ""x"": 1172,
            ""y"": 1273
          },
          {
            ""x"": 1176,
            ""y"": 1239
          }
        ],
        ""words"": [
          {
            ""text"": ""The"",
            ""boundingPolygon"": [
              {
                ""x"": 1189,
                ""y"": 1239
              },
              {
                ""x"": 1187,
                ""y"": 1254
              },
              {
                ""x"": 1174,
                ""y"": 1252
              },
              {
                ""x"": 1176,
                ""y"": 1239
              }
            ],
            ""confidence"": 0.985
          },
          {
            ""text"": ""train"",
            ""boundingPolygon"": [
              {
                ""x"": 1187,
                ""y"": 1256
              },
              {
                ""x"": 1185,
                ""y"": 1273
              },
              {
                ""x"": 1172,
                ""y"": 1272
              },
              {
                ""x"": 1174,
                ""y"": 1255
              }
            ],
            ""confidence"": 0.599
          }
        ]
      },
      {
        ""text"": ""ainpipe Vermin dies, you"",
        ""boundingPolygon"": [
          {
            ""x"": 1207,
            ""y"": 1515
          },
          {
            ""x"": 1211,
            ""y"": 1620
          },
          {
            ""x"": 1198,
            ""y"": 1620
          },
          {
            ""x"": 1196,
            ""y"": 1515
          }
        ],
        ""words"": [
          {
            ""text"": ""ainpipe"",
            ""boundingPolygon"": [
              {
                ""x"": 1208,
                ""y"": 1516
              },
              {
                ""x"": 1208,
                ""y"": 1546
              },
              {
                ""x"": 1196,
                ""y"": 1546
              },
              {
                ""x"": 1196,
                ""y"": 1517
              }
            ],
            ""confidence"": 0.8
          },
          {
            ""text"": ""Vermin"",
            ""boundingPolygon"": [
              {
                ""x"": 1208,
                ""y"": 1548
              },
              {
                ""x"": 1209,
                ""y"": 1576
              },
              {
                ""x"": 1197,
                ""y"": 1577
              },
              {
                ""x"": 1196,
                ""y"": 1549
              }
            ],
            ""confidence"": 0.992
          },
          {
            ""text"": ""dies,"",
            ""boundingPolygon"": [
              {
                ""x"": 1209,
                ""y"": 1580
              },
              {
                ""x"": 1210,
                ""y"": 1597
              },
              {
                ""x"": 1199,
                ""y"": 1598
              },
              {
                ""x"": 1198,
                ""y"": 1581
              }
            ],
            ""confidence"": 0.804
          },
          {
            ""text"": ""you"",
            ""boundingPolygon"": [
              {
                ""x"": 1210,
                ""y"": 1600
              },
              {
                ""x"": 1211,
                ""y"": 1616
              },
              {
                ""x"": 1200,
                ""y"": 1617
              },
              {
                ""x"": 1199,
                ""y"": 1601
              }
            ],
            ""confidence"": 0.959
          }
        ]
      },
      {
        ""text"": ""discards"",
        ""boundingPolygon"": [
          {
            ""x"": 1194,
            ""y"": 1479
          },
          {
            ""x"": 1193,
            ""y"": 1512
          },
          {
            ""x"": 1179,
            ""y"": 1513
          },
          {
            ""x"": 1180,
            ""y"": 1479
          }
        ],
        ""words"": [
          {
            ""text"": ""discards"",
            ""boundingPolygon"": [
              {
                ""x"": 1194,
                ""y"": 1480
              },
              {
                ""x"": 1193,
                ""y"": 1513
              },
              {
                ""x"": 1179,
                ""y"": 1512
              },
              {
                ""x"": 1180,
                ""y"": 1479
              }
            ],
            ""confidence"": 0.782
          }
        ]
      },
      {
        ""text"": ""pay @. If you"",
        ""boundingPolygon"": [
          {
            ""x"": 1199,
            ""y"": 1497
          },
          {
            ""x"": 1201,
            ""y"": 1557
          },
          {
            ""x"": 1186,
            ""y"": 1557
          },
          {
            ""x"": 1185,
            ""y"": 1497
          }
        ],
        ""words"": [
          {
            ""text"": ""pay"",
            ""boundingPolygon"": [
              {
                ""x"": 1199,
                ""y"": 1497
              },
              {
                ""x"": 1199,
                ""y"": 1514
              },
              {
                ""x"": 1185,
                ""y"": 1515
              },
              {
                ""x"": 1185,
                ""y"": 1499
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""@."",
            ""boundingPolygon"": [
              {
                ""x"": 1199,
                ""y"": 1516
              },
              {
                ""x"": 1199,
                ""y"": 1525
              },
              {
                ""x"": 1186,
                ""y"": 1527
              },
              {
                ""x"": 1185,
                ""y"": 1518
              }
            ],
            ""confidence"": 0.155
          },
          {
            ""text"": ""If"",
            ""boundingPolygon"": [
              {
                ""x"": 1199,
                ""y"": 1528
              },
              {
                ""x"": 1200,
                ""y"": 1533
              },
              {
                ""x"": 1186,
                ""y"": 1535
              },
              {
                ""x"": 1186,
                ""y"": 1530
              }
            ],
            ""confidence"": 0.858
          },
          {
            ""text"": ""you"",
            ""boundingPolygon"": [
              {
                ""x"": 1200,
                ""y"": 1536
              },
              {
                ""x"": 1201,
                ""y"": 1553
              },
              {
                ""x"": 1187,
                ""y"": 1555
              },
              {
                ""x"": 1186,
                ""y"": 1538
              }
            ],
            ""confidence"": 0.882
          }
        ]
      },
      {
        ""text"": ""Equipped creature gets +2/+0"",
        ""boundingPolygon"": [
          {
            ""x"": 1210,
            ""y"": 1708
          },
          {
            ""x"": 1215,
            ""y"": 1826
          },
          {
            ""x"": 1202,
            ""y"": 1826
          },
          {
            ""x"": 1197,
            ""y"": 1708
          }
        ],
        ""words"": [
          {
            ""text"": ""Equipped"",
            ""boundingPolygon"": [
              {
                ""x"": 1210,
                ""y"": 1709
              },
              {
                ""x"": 1212,
                ""y"": 1744
              },
              {
                ""x"": 1200,
                ""y"": 1743
              },
              {
                ""x"": 1197,
                ""y"": 1708
              }
            ],
            ""confidence"": 0.912
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 1212,
                ""y"": 1746
              },
              {
                ""x"": 1213,
                ""y"": 1779
              },
              {
                ""x"": 1201,
                ""y"": 1779
              },
              {
                ""x"": 1200,
                ""y"": 1746
              }
            ],
            ""confidence"": 0.882
          },
          {
            ""text"": ""gets"",
            ""boundingPolygon"": [
              {
                ""x"": 1213,
                ""y"": 1781
              },
              {
                ""x"": 1214,
                ""y"": 1797
              },
              {
                ""x"": 1202,
                ""y"": 1798
              },
              {
                ""x"": 1201,
                ""y"": 1781
              }
            ],
            ""confidence"": 0.958
          },
          {
            ""text"": ""+2/+0"",
            ""boundingPolygon"": [
              {
                ""x"": 1214,
                ""y"": 1800
              },
              {
                ""x"": 1215,
                ""y"": 1825
              },
              {
                ""x"": 1203,
                ""y"": 1826
              },
              {
                ""x"": 1202,
                ""y"": 1800
              }
            ],
            ""confidence"": 0.871
          }
        ]
      },
      {
        ""text"": ""Purraj of Urbory"",
        ""boundingPolygon"": [
          {
            ""x"": 1152,
            ""y"": 531
          },
          {
            ""x"": 1150,
            ""y"": 598
          },
          {
            ""x"": 1139,
            ""y"": 598
          },
          {
            ""x"": 1141,
            ""y"": 531
          }
        ],
        ""words"": [
          {
            ""text"": ""Purraj"",
            ""boundingPolygon"": [
              {
                ""x"": 1152,
                ""y"": 538
              },
              {
                ""x"": 1152,
                ""y"": 563
              },
              {
                ""x"": 1141,
                ""y"": 562
              },
              {
                ""x"": 1141,
                ""y"": 537
              }
            ],
            ""confidence"": 0.64
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 1152,
                ""y"": 565
              },
              {
                ""x"": 1152,
                ""y"": 573
              },
              {
                ""x"": 1140,
                ""y"": 572
              },
              {
                ""x"": 1141,
                ""y"": 564
              }
            ],
            ""confidence"": 0.897
          },
          {
            ""text"": ""Urbory"",
            ""boundingPolygon"": [
              {
                ""x"": 1151,
                ""y"": 575
              },
              {
                ""x"": 1150,
                ""y"": 599
              },
              {
                ""x"": 1139,
                ""y"": 599
              },
              {
                ""x"": 1140,
                ""y"": 574
              }
            ],
            ""confidence"": 0.57
          }
        ]
      },
      {
        ""text"": ""battlefield, you may have en"",
        ""boundingPolygon"": [
          {
            ""x"": 1164,
            ""y"": 724
          },
          {
            ""x"": 1163,
            ""y"": 828
          },
          {
            ""x"": 1148,
            ""y"": 827
          },
          {
            ""x"": 1149,
            ""y"": 724
          }
        ],
        ""words"": [
          {
            ""text"": ""battlefield,"",
            ""boundingPolygon"": [
              {
                ""x"": 1163,
                ""y"": 724
              },
              {
                ""x"": 1164,
                ""y"": 762
              },
              {
                ""x"": 1150,
                ""y"": 763
              },
              {
                ""x"": 1149,
                ""y"": 725
              }
            ],
            ""confidence"": 0.578
          },
          {
            ""text"": ""you"",
            ""boundingPolygon"": [
              {
                ""x"": 1164,
                ""y"": 765
              },
              {
                ""x"": 1164,
                ""y"": 778
              },
              {
                ""x"": 1150,
                ""y"": 778
              },
              {
                ""x"": 1150,
                ""y"": 765
              }
            ],
            ""confidence"": 0.978
          },
          {
            ""text"": ""may"",
            ""boundingPolygon"": [
              {
                ""x"": 1164,
                ""y"": 781
              },
              {
                ""x"": 1163,
                ""y"": 796
              },
              {
                ""x"": 1150,
                ""y"": 796
              },
              {
                ""x"": 1150,
                ""y"": 781
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""have"",
            ""boundingPolygon"": [
              {
                ""x"": 1163,
                ""y"": 799
              },
              {
                ""x"": 1163,
                ""y"": 815
              },
              {
                ""x"": 1149,
                ""y"": 815
              },
              {
                ""x"": 1150,
                ""y"": 798
              }
            ],
            ""confidence"": 0.962
          },
          {
            ""text"": ""en"",
            ""boundingPolygon"": [
              {
                ""x"": 1162,
                ""y"": 818
              },
              {
                ""x"": 1162,
                ""y"": 828
              },
              {
                ""x"": 1148,
                ""y"": 827
              },
              {
                ""x"": 1149,
                ""y"": 817
              }
            ],
            ""confidence"": 0.757
          }
        ]
      },
      {
        ""text"": ""Enchant latid"",
        ""boundingPolygon"": [
          {
            ""x"": 1174,
            ""y"": 985
          },
          {
            ""x"": 1172,
            ""y"": 1071
          },
          {
            ""x"": 1158,
            ""y"": 1070
          },
          {
            ""x"": 1160,
            ""y"": 985
          }
        ],
        ""words"": [
          {
            ""text"": ""Enchant"",
            ""boundingPolygon"": [
              {
                ""x"": 1174,
                ""y"": 986
              },
              {
                ""x"": 1174,
                ""y"": 1017
              },
              {
                ""x"": 1161,
                ""y"": 1017
              },
              {
                ""x"": 1160,
                ""y"": 986
              }
            ],
            ""confidence"": 0.64
          },
          {
            ""text"": ""latid"",
            ""boundingPolygon"": [
              {
                ""x"": 1174,
                ""y"": 1019
              },
              {
                ""x"": 1173,
                ""y"": 1038
              },
              {
                ""x"": 1160,
                ""y"": 1038
              },
              {
                ""x"": 1161,
                ""y"": 1019
              }
            ],
            ""confidence"": 0.22
          }
        ]
      },
      {
        ""text"": ""gratitude"",
        ""boundingPolygon"": [
          {
            ""x"": 1175,
            ""y"": 1337
          },
          {
            ""x"": 1176,
            ""y"": 1380
          },
          {
            ""x"": 1165,
            ""y"": 1381
          },
          {
            ""x"": 1164,
            ""y"": 1337
          }
        ],
        ""words"": [
          {
            ""text"": ""gratitude"",
            ""boundingPolygon"": [
              {
                ""x"": 1176,
                ""y"": 1337
              },
              {
                ""x"": 1177,
                ""y"": 1375
              },
              {
                ""x"": 1165,
                ""y"": 1375
              },
              {
                ""x"": 1164,
                ""y"": 1337
              }
            ],
            ""confidence"": 0.581
          }
        ]
      },
      {
        ""text"": ""Then times are tough, the"",
        ""boundingPolygon"": [
          {
            ""x"": 1183,
            ""y"": 1481
          },
          {
            ""x"": 1180,
            ""y"": 1578
          },
          {
            ""x"": 1166,
            ""y"": 1578
          },
          {
            ""x"": 1168,
            ""y"": 1481
          }
        ],
        ""words"": [
          {
            ""text"": ""Then"",
            ""boundingPolygon"": [
              {
                ""x"": 1183,
                ""y"": 1485
              },
              {
                ""x"": 1181,
                ""y"": 1502
              },
              {
                ""x"": 1168,
                ""y"": 1503
              },
              {
                ""x"": 1169,
                ""y"": 1486
              }
            ],
            ""confidence"": 0.357
          },
          {
            ""text"": ""times"",
            ""boundingPolygon"": [
              {
                ""x"": 1181,
                ""y"": 1505
              },
              {
                ""x"": 1179,
                ""y"": 1524
              },
              {
                ""x"": 1167,
                ""y"": 1525
              },
              {
                ""x"": 1168,
                ""y"": 1506
              }
            ],
            ""confidence"": 0.492
          },
          {
            ""text"": ""are"",
            ""boundingPolygon"": [
              {
                ""x"": 1179,
                ""y"": 1527
              },
              {
                ""x"": 1179,
                ""y"": 1538
              },
              {
                ""x"": 1167,
                ""y"": 1539
              },
              {
                ""x"": 1167,
                ""y"": 1528
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""tough,"",
            ""boundingPolygon"": [
              {
                ""x"": 1179,
                ""y"": 1541
              },
              {
                ""x"": 1179,
                ""y"": 1564
              },
              {
                ""x"": 1167,
                ""y"": 1565
              },
              {
                ""x"": 1167,
                ""y"": 1542
              }
            ],
            ""confidence"": 0.883
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 1179,
                ""y"": 1566
              },
              {
                ""x"": 1180,
                ""y"": 1578
              },
              {
                ""x"": 1168,
                ""y"": 1579
              },
              {
                ""x"": 1167,
                ""y"": 1567
              }
            ],
            ""confidence"": 0.976
          }
        ]
      },
      {
        ""text"": ""Equip 2"",
        ""boundingPolygon"": [
          {
            ""x"": 1195,
            ""y"": 1710
          },
          {
            ""x"": 1200,
            ""y"": 1755
          },
          {
            ""x"": 1187,
            ""y"": 1756
          },
          {
            ""x"": 1182,
            ""y"": 1710
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 1196,
                ""y"": 1710
              },
              {
                ""x"": 1198,
                ""y"": 1732
              },
              {
                ""x"": 1185,
                ""y"": 1732
              },
              {
                ""x"": 1182,
                ""y"": 1710
              }
            ],
            ""confidence"": 0.284
          },
          {
            ""text"": ""2"",
            ""boundingPolygon"": [
              {
                ""x"": 1198,
                ""y"": 1735
              },
              {
                ""x"": 1199,
                ""y"": 1742
              },
              {
                ""x"": 1186,
                ""y"": 1742
              },
              {
                ""x"": 1185,
                ""y"": 1735
              }
            ],
            ""confidence"": 0.993
          }
        ]
      },
      {
        ""text"": ""dus. Alan Robusont"",
        ""boundingPolygon"": [
          {
            ""x"": 1137,
            ""y"": 186
          },
          {
            ""x"": 1128,
            ""y"": 298
          },
          {
            ""x"": 1119,
            ""y"": 297
          },
          {
            ""x"": 1127,
            ""y"": 185
          }
        ],
        ""words"": [
          {
            ""text"": ""dus."",
            ""boundingPolygon"": [
              {
                ""x"": 1138,
                ""y"": 187
              },
              {
                ""x"": 1136,
                ""y"": 199
              },
              {
                ""x"": 1127,
                ""y"": 199
              },
              {
                ""x"": 1129,
                ""y"": 186
              }
            ],
            ""confidence"": 0.719
          },
          {
            ""text"": ""Alan"",
            ""boundingPolygon"": [
              {
                ""x"": 1136,
                ""y"": 201
              },
              {
                ""x"": 1134,
                ""y"": 216
              },
              {
                ""x"": 1125,
                ""y"": 215
              },
              {
                ""x"": 1127,
                ""y"": 200
              }
            ],
            ""confidence"": 0.83
          },
          {
            ""text"": ""Robusont"",
            ""boundingPolygon"": [
              {
                ""x"": 1134,
                ""y"": 218
              },
              {
                ""x"": 1131,
                ""y"": 252
              },
              {
                ""x"": 1122,
                ""y"": 251
              },
              {
                ""x"": 1125,
                ""y"": 217
              }
            ],
            ""confidence"": 0.162
          }
        ]
      },
      {
        ""text"": ""Heavier than it looks, tricky to wield,"",
        ""boundingPolygon"": [
          {
            ""x"": 1188,
            ""y"": 1708
          },
          {
            ""x"": 1193,
            ""y"": 1846
          },
          {
            ""x"": 1175,
            ""y"": 1846
          },
          {
            ""x"": 1172,
            ""y"": 1708
          }
        ],
        ""words"": [
          {
            ""text"": ""Heavier"",
            ""boundingPolygon"": [
              {
                ""x"": 1188,
                ""y"": 1709
              },
              {
                ""x"": 1187,
                ""y"": 1736
              },
              {
                ""x"": 1172,
                ""y"": 1737
              },
              {
                ""x"": 1173,
                ""y"": 1709
              }
            ],
            ""confidence"": 0.871
          },
          {
            ""text"": ""than"",
            ""boundingPolygon"": [
              {
                ""x"": 1187,
                ""y"": 1739
              },
              {
                ""x"": 1187,
                ""y"": 1757
              },
              {
                ""x"": 1172,
                ""y"": 1757
              },
              {
                ""x"": 1172,
                ""y"": 1740
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""it"",
            ""boundingPolygon"": [
              {
                ""x"": 1187,
                ""y"": 1759
              },
              {
                ""x"": 1187,
                ""y"": 1764
              },
              {
                ""x"": 1172,
                ""y"": 1765
              },
              {
                ""x"": 1172,
                ""y"": 1760
              }
            ],
            ""confidence"": 0.936
          },
          {
            ""text"": ""looks,"",
            ""boundingPolygon"": [
              {
                ""x"": 1187,
                ""y"": 1767
              },
              {
                ""x"": 1188,
                ""y"": 1786
              },
              {
                ""x"": 1173,
                ""y"": 1787
              },
              {
                ""x"": 1172,
                ""y"": 1768
              }
            ],
            ""confidence"": 0.638
          },
          {
            ""text"": ""tricky"",
            ""boundingPolygon"": [
              {
                ""x"": 1188,
                ""y"": 1789
              },
              {
                ""x"": 1189,
                ""y"": 1810
              },
              {
                ""x"": 1175,
                ""y"": 1812
              },
              {
                ""x"": 1174,
                ""y"": 1790
              }
            ],
            ""confidence"": 0.986
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 1190,
                ""y"": 1813
              },
              {
                ""x"": 1190,
                ""y"": 1819
              },
              {
                ""x"": 1176,
                ""y"": 1820
              },
              {
                ""x"": 1175,
                ""y"": 1815
              }
            ],
            ""confidence"": 0.877
          },
          {
            ""text"": ""wield,"",
            ""boundingPolygon"": [
              {
                ""x"": 1190,
                ""y"": 1822
              },
              {
                ""x"": 1193,
                ""y"": 1845
              },
              {
                ""x"": 1179,
                ""y"": 1846
              },
              {
                ""x"": 1176,
                ""y"": 1823
              }
            ],
            ""confidence"": 0.611
          }
        ]
      },
      {
        ""text"": ""Kach denis damage equal to it"",
        ""boundingPolygon"": [
          {
            ""x"": 1144,
            ""y"": 726
          },
          {
            ""x"": 1145,
            ""y"": 850
          },
          {
            ""x"": 1132,
            ""y"": 850
          },
          {
            ""x"": 1130,
            ""y"": 726
          }
        ],
        ""words"": [
          {
            ""text"": ""Kach"",
            ""boundingPolygon"": [
              {
                ""x"": 1144,
                ""y"": 744
              },
              {
                ""x"": 1145,
                ""y"": 765
              },
              {
                ""x"": 1133,
                ""y"": 764
              },
              {
                ""x"": 1132,
                ""y"": 743
              }
            ],
            ""confidence"": 0.12
          },
          {
            ""text"": ""denis"",
            ""boundingPolygon"": [
              {
                ""x"": 1145,
                ""y"": 767
              },
              {
                ""x"": 1145,
                ""y"": 785
              },
              {
                ""x"": 1133,
                ""y"": 784
              },
              {
                ""x"": 1133,
                ""y"": 766
              }
            ],
            ""confidence"": 0.146
          },
          {
            ""text"": ""damage"",
            ""boundingPolygon"": [
              {
                ""x"": 1145,
                ""y"": 787
              },
              {
                ""x"": 1145,
                ""y"": 813
              },
              {
                ""x"": 1133,
                ""y"": 813
              },
              {
                ""x"": 1133,
                ""y"": 786
              }
            ],
            ""confidence"": 0.699
          },
          {
            ""text"": ""equal"",
            ""boundingPolygon"": [
              {
                ""x"": 1145,
                ""y"": 816
              },
              {
                ""x"": 1144,
                ""y"": 833
              },
              {
                ""x"": 1133,
                ""y"": 833
              },
              {
                ""x"": 1133,
                ""y"": 815
              }
            ],
            ""confidence"": 0.323
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 1144,
                ""y"": 836
              },
              {
                ""x"": 1144,
                ""y"": 842
              },
              {
                ""x"": 1133,
                ""y"": 842
              },
              {
                ""x"": 1133,
                ""y"": 835
              }
            ],
            ""confidence"": 0.895
          },
          {
            ""text"": ""it"",
            ""boundingPolygon"": [
              {
                ""x"": 1144,
                ""y"": 844
              },
              {
                ""x"": 1143,
                ""y"": 850
              },
              {
                ""x"": 1133,
                ""y"": 850
              },
              {
                ""x"": 1133,
                ""y"": 844
              }
            ],
            ""confidence"": 0.706
          }
        ]
      },
      {
        ""text"": ""ed land has \""Untap this land"",
        ""boundingPolygon"": [
          {
            ""x"": 1165,
            ""y"": 1019
          },
          {
            ""x"": 1164,
            ""y"": 1135
          },
          {
            ""x"": 1150,
            ""y"": 1135
          },
          {
            ""x"": 1151,
            ""y"": 1019
          }
        ],
        ""words"": [
          {
            ""text"": ""ed"",
            ""boundingPolygon"": [
              {
                ""x"": 1165,
                ""y"": 1019
              },
              {
                ""x"": 1165,
                ""y"": 1025
              },
              {
                ""x"": 1152,
                ""y"": 1025
              },
              {
                ""x"": 1152,
                ""y"": 1019
              }
            ],
            ""confidence"": 0.874
          },
          {
            ""text"": ""land"",
            ""boundingPolygon"": [
              {
                ""x"": 1165,
                ""y"": 1028
              },
              {
                ""x"": 1165,
                ""y"": 1046
              },
              {
                ""x"": 1151,
                ""y"": 1046
              },
              {
                ""x"": 1152,
                ""y"": 1028
              }
            ],
            ""confidence"": 0.986
          },
          {
            ""text"": ""has"",
            ""boundingPolygon"": [
              {
                ""x"": 1165,
                ""y"": 1048
              },
              {
                ""x"": 1164,
                ""y"": 1060
              },
              {
                ""x"": 1151,
                ""y"": 1060
              },
              {
                ""x"": 1151,
                ""y"": 1048
              }
            ],
            ""confidence"": 0.868
          },
          {
            ""text"": ""\""Untap"",
            ""boundingPolygon"": [
              {
                ""x"": 1164,
                ""y"": 1063
              },
              {
                ""x"": 1164,
                ""y"": 1093
              },
              {
                ""x"": 1151,
                ""y"": 1093
              },
              {
                ""x"": 1151,
                ""y"": 1063
              }
            ],
            ""confidence"": 0.866
          },
          {
            ""text"": ""this"",
            ""boundingPolygon"": [
              {
                ""x"": 1164,
                ""y"": 1095
              },
              {
                ""x"": 1164,
                ""y"": 1109
              },
              {
                ""x"": 1150,
                ""y"": 1109
              },
              {
                ""x"": 1151,
                ""y"": 1095
              }
            ],
            ""confidence"": 0.958
          },
          {
            ""text"": ""land"",
            ""boundingPolygon"": [
              {
                ""x"": 1164,
                ""y"": 1111
              },
              {
                ""x"": 1164,
                ""y"": 1131
              },
              {
                ""x"": 1150,
                ""y"": 1131
              },
              {
                ""x"": 1150,
                ""y"": 1111
              }
            ],
            ""confidence"": 0.923
          }
        ]
      },
      {
        ""text"": ""knights of New Benalia for the!"",
        ""boundingPolygon"": [
          {
            ""x"": 1170,
            ""y"": 1238
          },
          {
            ""x"": 1169,
            ""y"": 1358
          },
          {
            ""x"": 1151,
            ""y"": 1358
          },
          {
            ""x"": 1152,
            ""y"": 1238
          }
        ],
        ""words"": [
          {
            ""text"": ""knights"",
            ""boundingPolygon"": [
              {
                ""x"": 1169,
                ""y"": 1238
              },
              {
                ""x"": 1168,
                ""y"": 1264
              },
              {
                ""x"": 1152,
                ""y"": 1264
              },
              {
                ""x"": 1153,
                ""y"": 1238
              }
            ],
            ""confidence"": 0.703
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 1168,
                ""y"": 1267
              },
              {
                ""x"": 1167,
                ""y"": 1275
              },
              {
                ""x"": 1152,
                ""y"": 1274
              },
              {
                ""x"": 1152,
                ""y"": 1267
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""New"",
            ""boundingPolygon"": [
              {
                ""x"": 1167,
                ""y"": 1278
              },
              {
                ""x"": 1167,
                ""y"": 1296
              },
              {
                ""x"": 1152,
                ""y"": 1295
              },
              {
                ""x"": 1152,
                ""y"": 1278
              }
            ],
            ""confidence"": 0.902
          },
          {
            ""text"": ""Benalia"",
            ""boundingPolygon"": [
              {
                ""x"": 1167,
                ""y"": 1299
              },
              {
                ""x"": 1167,
                ""y"": 1328
              },
              {
                ""x"": 1151,
                ""y"": 1327
              },
              {
                ""x"": 1151,
                ""y"": 1298
              }
            ],
            ""confidence"": 0.852
          },
          {
            ""text"": ""for"",
            ""boundingPolygon"": [
              {
                ""x"": 1168,
                ""y"": 1331
              },
              {
                ""x"": 1168,
                ""y"": 1341
              },
              {
                ""x"": 1151,
                ""y"": 1340
              },
              {
                ""x"": 1151,
                ""y"": 1330
              }
            ],
            ""confidence"": 0.579
          },
          {
            ""text"": ""the!"",
            ""boundingPolygon"": [
              {
                ""x"": 1168,
                ""y"": 1344
              },
              {
                ""x"": 1169,
                ""y"": 1358
              },
              {
                ""x"": 1151,
                ""y"": 1357
              },
              {
                ""x"": 1151,
                ""y"": 1343
              }
            ],
            ""confidence"": 0.474
          }
        ]
      },
      {
        ""text"": ""hanted cree pers"",
        ""boundingPolygon"": [
          {
            ""x"": 1134,
            ""y"": 723
          },
          {
            ""x"": 1134,
            ""y"": 800
          },
          {
            ""x"": 1121,
            ""y"": 800
          },
          {
            ""x"": 1121,
            ""y"": 723
          }
        ],
        ""words"": [
          {
            ""text"": ""hanted"",
            ""boundingPolygon"": [
              {
                ""x"": 1134,
                ""y"": 725
              },
              {
                ""x"": 1134,
                ""y"": 749
              },
              {
                ""x"": 1122,
                ""y"": 748
              },
              {
                ""x"": 1121,
                ""y"": 724
              }
            ],
            ""confidence"": 0.849
          },
          {
            ""text"": ""cree"",
            ""boundingPolygon"": [
              {
                ""x"": 1134,
                ""y"": 751
              },
              {
                ""x"": 1134,
                ""y"": 780
              },
              {
                ""x"": 1122,
                ""y"": 780
              },
              {
                ""x"": 1122,
                ""y"": 751
              }
            ],
            ""confidence"": 0
          },
          {
            ""text"": ""pers"",
            ""boundingPolygon"": [
              {
                ""x"": 1134,
                ""y"": 782
              },
              {
                ""x"": 1134,
                ""y"": 800
              },
              {
                ""x"": 1121,
                ""y"": 800
              },
              {
                ""x"": 1122,
                ""y"": 782
              }
            ],
            ""confidence"": 0.001
          }
        ]
      },
      {
        ""text"": ""The ruins of Old Prahy became a"",
        ""boundingPolygon"": [
          {
            ""x"": 1146,
            ""y"": 990
          },
          {
            ""x"": 1141,
            ""y"": 1121
          },
          {
            ""x"": 1127,
            ""y"": 1120
          },
          {
            ""x"": 1131,
            ""y"": 990
          }
        ],
        ""words"": [
          {
            ""text"": ""The"",
            ""boundingPolygon"": [
              {
                ""x"": 1147,
                ""y"": 991
              },
              {
                ""x"": 1146,
                ""y"": 999
              },
              {
                ""x"": 1131,
                ""y"": 998
              },
              {
                ""x"": 1131,
                ""y"": 990
              }
            ],
            ""confidence"": 0.285
          },
          {
            ""text"": ""ruins"",
            ""boundingPolygon"": [
              {
                ""x"": 1146,
                ""y"": 1002
              },
              {
                ""x"": 1145,
                ""y"": 1020
              },
              {
                ""x"": 1130,
                ""y"": 1019
              },
              {
                ""x"": 1131,
                ""y"": 1001
              }
            ],
            ""confidence"": 0.534
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 1145,
                ""y"": 1022
              },
              {
                ""x"": 1144,
                ""y"": 1030
              },
              {
                ""x"": 1130,
                ""y"": 1029
              },
              {
                ""x"": 1130,
                ""y"": 1021
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""Old"",
            ""boundingPolygon"": [
              {
                ""x"": 1144,
                ""y"": 1033
              },
              {
                ""x"": 1144,
                ""y"": 1045
              },
              {
                ""x"": 1129,
                ""y"": 1044
              },
              {
                ""x"": 1130,
                ""y"": 1032
              }
            ],
            ""confidence"": 0.89
          },
          {
            ""text"": ""Prahy"",
            ""boundingPolygon"": [
              {
                ""x"": 1144,
                ""y"": 1048
              },
              {
                ""x"": 1143,
                ""y"": 1070
              },
              {
                ""x"": 1129,
                ""y"": 1069
              },
              {
                ""x"": 1129,
                ""y"": 1047
              }
            ],
            ""confidence"": 0.336
          },
          {
            ""text"": ""became"",
            ""boundingPolygon"": [
              {
                ""x"": 1142,
                ""y"": 1073
              },
              {
                ""x"": 1142,
                ""y"": 1099
              },
              {
                ""x"": 1128,
                ""y"": 1098
              },
              {
                ""x"": 1129,
                ""y"": 1072
              }
            ],
            ""confidence"": 0.628
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 1142,
                ""y"": 1102
              },
              {
                ""x"": 1141,
                ""y"": 1109
              },
              {
                ""x"": 1127,
                ""y"": 1108
              },
              {
                ""x"": 1128,
                ""y"": 1101
              }
            ],
            ""confidence"": 0.12
          }
        ]
      },
      {
        ""text"": ""for their aid"",
        ""boundingPolygon"": [
          {
            ""x"": 1162,
            ""y"": 1327
          },
          {
            ""x"": 1160,
            ""y"": 1379
          },
          {
            ""x"": 1146,
            ""y"": 1378
          },
          {
            ""x"": 1148,
            ""y"": 1327
          }
        ],
        ""words"": [
          {
            ""text"": ""for"",
            ""boundingPolygon"": [
              {
                ""x"": 1162,
                ""y"": 1330
              },
              {
                ""x"": 1161,
                ""y"": 1340
              },
              {
                ""x"": 1148,
                ""y"": 1340
              },
              {
                ""x"": 1148,
                ""y"": 1330
              }
            ],
            ""confidence"": 0.856
          },
          {
            ""text"": ""their"",
            ""boundingPolygon"": [
              {
                ""x"": 1161,
                ""y"": 1343
              },
              {
                ""x"": 1161,
                ""y"": 1361
              },
              {
                ""x"": 1147,
                ""y"": 1361
              },
              {
                ""x"": 1148,
                ""y"": 1343
              }
            ],
            ""confidence"": 0.991
          },
          {
            ""text"": ""aid"",
            ""boundingPolygon"": [
              {
                ""x"": 1161,
                ""y"": 1363
              },
              {
                ""x"": 1161,
                ""y"": 1378
              },
              {
                ""x"": 1147,
                ""y"": 1378
              },
              {
                ""x"": 1147,
                ""y"": 1363
              }
            ],
            ""confidence"": 0.882
          }
        ]
      },
      {
        ""text"": ""When timer"",
        ""boundingPolygon"": [
          {
            ""x"": 1174,
            ""y"": 1483
          },
          {
            ""x"": 1175,
            ""y"": 1525
          },
          {
            ""x"": 1160,
            ""y"": 1527
          },
          {
            ""x"": 1159,
            ""y"": 1485
          }
        ],
        ""words"": [
          {
            ""text"": ""When"",
            ""boundingPolygon"": [
              {
                ""x"": 1174,
                ""y"": 1483
              },
              {
                ""x"": 1174,
                ""y"": 1503
              },
              {
                ""x"": 1159,
                ""y"": 1503
              },
              {
                ""x"": 1159,
                ""y"": 1483
              }
            ],
            ""confidence"": 0.823
          },
          {
            ""text"": ""timer"",
            ""boundingPolygon"": [
              {
                ""x"": 1175,
                ""y"": 1506
              },
              {
                ""x"": 1175,
                ""y"": 1527
              },
              {
                ""x"": 1160,
                ""y"": 1527
              },
              {
                ""x"": 1160,
                ""y"": 1506
              }
            ],
            ""confidence"": 0.585
          }
        ]
      },
      {
        ""text"": ""Mus. Charles Gulespie ...."",
        ""boundingPolygon"": [
          {
            ""x"": 1125,
            ""y"": 458
          },
          {
            ""x"": 1120,
            ""y"": 556
          },
          {
            ""x"": 1110,
            ""y"": 555
          },
          {
            ""x"": 1114,
            ""y"": 457
          }
        ],
        ""words"": [
          {
            ""text"": ""Mus."",
            ""boundingPolygon"": [
              {
                ""x"": 1125,
                ""y"": 459
              },
              {
                ""x"": 1124,
                ""y"": 471
              },
              {
                ""x"": 1114,
                ""y"": 470
              },
              {
                ""x"": 1114,
                ""y"": 458
              }
            ],
            ""confidence"": 0.171
          },
          {
            ""text"": ""Charles"",
            ""boundingPolygon"": [
              {
                ""x"": 1124,
                ""y"": 473
              },
              {
                ""x"": 1123,
                ""y"": 497
              },
              {
                ""x"": 1113,
                ""y"": 496
              },
              {
                ""x"": 1114,
                ""y"": 472
              }
            ],
            ""confidence"": 0.694
          },
          {
            ""text"": ""Gulespie"",
            ""boundingPolygon"": [
              {
                ""x"": 1123,
                ""y"": 499
              },
              {
                ""x"": 1121,
                ""y"": 527
              },
              {
                ""x"": 1112,
                ""y"": 526
              },
              {
                ""x"": 1113,
                ""y"": 498
              }
            ],
            ""confidence"": 0.631
          },
          {
            ""text"": ""...."",
            ""boundingPolygon"": [
              {
                ""x"": 1121,
                ""y"": 529
              },
              {
                ""x"": 1120,
                ""y"": 551
              },
              {
                ""x"": 1111,
                ""y"": 550
              },
              {
                ""x"": 1112,
                ""y"": 528
              }
            ],
            ""confidence"": 0.256
          }
        ]
      },
      {
        ""text"": ""ais. When times are tougher,"",
        ""boundingPolygon"": [
          {
            ""x"": 1166,
            ""y"": 1483
          },
          {
            ""x"": 1178,
            ""y"": 1622
          },
          {
            ""x"": 1165,
            ""y"": 1624
          },
          {
            ""x"": 1153,
            ""y"": 1484
          }
        ],
        ""words"": [
          {
            ""text"": ""ais."",
            ""boundingPolygon"": [
              {
                ""x"": 1167,
                ""y"": 1483
              },
              {
                ""x"": 1167,
                ""y"": 1494
              },
              {
                ""x"": 1154,
                ""y"": 1496
              },
              {
                ""x"": 1153,
                ""y"": 1485
              }
            ],
            ""confidence"": 0.53
          },
          {
            ""text"": ""When"",
            ""boundingPolygon"": [
              {
                ""x"": 1167,
                ""y"": 1497
              },
              {
                ""x"": 1169,
                ""y"": 1519
              },
              {
                ""x"": 1156,
                ""y"": 1521
              },
              {
                ""x"": 1154,
                ""y"": 1499
              }
            ],
            ""confidence"": 0.699
          },
          {
            ""text"": ""times"",
            ""boundingPolygon"": [
              {
                ""x"": 1169,
                ""y"": 1521
              },
              {
                ""x"": 1170,
                ""y"": 1540
              },
              {
                ""x"": 1157,
                ""y"": 1542
              },
              {
                ""x"": 1156,
                ""y"": 1523
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""are"",
            ""boundingPolygon"": [
              {
                ""x"": 1170,
                ""y"": 1543
              },
              {
                ""x"": 1171,
                ""y"": 1554
              },
              {
                ""x"": 1159,
                ""y"": 1556
              },
              {
                ""x"": 1158,
                ""y"": 1545
              }
            ],
            ""confidence"": 0.888
          },
          {
            ""text"": ""tougher,"",
            ""boundingPolygon"": [
              {
                ""x"": 1172,
                ""y"": 1557
              },
              {
                ""x"": 1175,
                ""y"": 1590
              },
              {
                ""x"": 1163,
                ""y"": 1592
              },
              {
                ""x"": 1159,
                ""y"": 1559
              }
            ],
            ""confidence"": 0.123
          }
        ]
      },
      {
        ""text"": ""Enchanted"",
        ""boundingPolygon"": [
          {
            ""x"": 1129,
            ""y"": 707
          },
          {
            ""x"": 1127,
            ""y"": 752
          },
          {
            ""x"": 1113,
            ""y"": 752
          },
          {
            ""x"": 1114,
            ""y"": 707
          }
        ],
        ""words"": [
          {
            ""text"": ""Enchanted"",
            ""boundingPolygon"": [
              {
                ""x"": 1129,
                ""y"": 710
              },
              {
                ""x"": 1127,
                ""y"": 750
              },
              {
                ""x"": 1117,
                ""y"": 750
              },
              {
                ""x"": 1118,
                ""y"": 709
              }
            ],
            ""confidence"": 0.702
          }
        ]
      },
      {
        ""text"": ""ers +1/+1 a"",
        ""boundingPolygon"": [
          {
            ""x"": 1126,
            ""y"": 788
          },
          {
            ""x"": 1127,
            ""y"": 830
          },
          {
            ""x"": 1117,
            ""y"": 830
          },
          {
            ""x"": 1116,
            ""y"": 788
          }
        ],
        ""words"": [
          {
            ""text"": ""ers"",
            ""boundingPolygon"": [
              {
                ""x"": 1126,
                ""y"": 788
              },
              {
                ""x"": 1126,
                ""y"": 798
              },
              {
                ""x"": 1116,
                ""y"": 799
              },
              {
                ""x"": 1116,
                ""y"": 789
              }
            ],
            ""confidence"": 0.674
          },
          {
            ""text"": ""+1/+1"",
            ""boundingPolygon"": [
              {
                ""x"": 1126,
                ""y"": 800
              },
              {
                ""x"": 1127,
                ""y"": 822
              },
              {
                ""x"": 1117,
                ""y"": 824
              },
              {
                ""x"": 1117,
                ""y"": 801
              }
            ],
            ""confidence"": 0.679
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 1127,
                ""y"": 824
              },
              {
                ""x"": 1128,
                ""y"": 829
              },
              {
                ""x"": 1117,
                ""y"": 831
              },
              {
                ""x"": 1117,
                ""y"": 826
              }
            ],
            ""confidence"": 0.18
          }
        ]
      },
      {
        ""text"": ""cuced by lingering Atorius magic."",
        ""boundingPolygon"": [
          {
            ""x"": 1129,
            ""y"": 993
          },
          {
            ""x"": 1127,
            ""y"": 1115
          },
          {
            ""x"": 1117,
            ""y"": 1115
          },
          {
            ""x"": 1117,
            ""y"": 993
          }
        ],
        ""words"": [
          {
            ""text"": ""cuced"",
            ""boundingPolygon"": [
              {
                ""x"": 1129,
                ""y"": 994
              },
              {
                ""x"": 1128,
                ""y"": 1012
              },
              {
                ""x"": 1118,
                ""y"": 1012
              },
              {
                ""x"": 1119,
                ""y"": 994
              }
            ],
            ""confidence"": 0.413
          },
          {
            ""text"": ""by"",
            ""boundingPolygon"": [
              {
                ""x"": 1128,
                ""y"": 1014
              },
              {
                ""x"": 1128,
                ""y"": 1023
              },
              {
                ""x"": 1118,
                ""y"": 1023
              },
              {
                ""x"": 1118,
                ""y"": 1014
              }
            ],
            ""confidence"": 0.912
          },
          {
            ""text"": ""lingering"",
            ""boundingPolygon"": [
              {
                ""x"": 1128,
                ""y"": 1025
              },
              {
                ""x"": 1128,
                ""y"": 1057
              },
              {
                ""x"": 1117,
                ""y"": 1057
              },
              {
                ""x"": 1118,
                ""y"": 1025
              }
            ],
            ""confidence"": 0.692
          },
          {
            ""text"": ""Atorius"",
            ""boundingPolygon"": [
              {
                ""x"": 1128,
                ""y"": 1059
              },
              {
                ""x"": 1128,
                ""y"": 1088
              },
              {
                ""x"": 1117,
                ""y"": 1088
              },
              {
                ""x"": 1117,
                ""y"": 1059
              }
            ],
            ""confidence"": 0.271
          },
          {
            ""text"": ""magic."",
            ""boundingPolygon"": [
              {
                ""x"": 1128,
                ""y"": 1090
              },
              {
                ""x"": 1128,
                ""y"": 1115
              },
              {
                ""x"": 1118,
                ""y"": 1115
              },
              {
                ""x"": 1117,
                ""y"": 1090
              }
            ],
            ""confidence"": 0.691
          }
        ]
      },
      {
        ""text"": ""Diviner's W ..."",
        ""boundingPolygon"": [
          {
            ""x"": 1030,
            ""y"": 181
          },
          {
            ""x"": 1020,
            ""y"": 330
          },
          {
            ""x"": 1009,
            ""y"": 329
          },
          {
            ""x"": 1019,
            ""y"": 181
          }
        ],
        ""words"": [
          {
            ""text"": ""Diviner's"",
            ""boundingPolygon"": [
              {
                ""x"": 1029,
                ""y"": 188
              },
              {
                ""x"": 1027,
                ""y"": 231
              },
              {
                ""x"": 1017,
                ""y"": 232
              },
              {
                ""x"": 1019,
                ""y"": 188
              }
            ],
            ""confidence"": 0.842
          },
          {
            ""text"": ""W"",
            ""boundingPolygon"": [
              {
                ""x"": 1027,
                ""y"": 233
              },
              {
                ""x"": 1027,
                ""y"": 239
              },
              {
                ""x"": 1016,
                ""y"": 239
              },
              {
                ""x"": 1017,
                ""y"": 234
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""..."",
            ""boundingPolygon"": [
              {
                ""x"": 1026,
                ""y"": 242
              },
              {
                ""x"": 1025,
                ""y"": 259
              },
              {
                ""x"": 1015,
                ""y"": 259
              },
              {
                ""x"": 1016,
                ""y"": 242
              }
            ],
            ""confidence"": 0.14
          }
        ]
      },
      {
        ""text"": ""S Wand"",
        ""boundingPolygon"": [
          {
            ""x"": 1021,
            ""y"": 217
          },
          {
            ""x"": 1017,
            ""y"": 284
          },
          {
            ""x"": 1004,
            ""y"": 283
          },
          {
            ""x"": 1007,
            ""y"": 217
          }
        ],
        ""words"": [
          {
            ""text"": ""S"",
            ""boundingPolygon"": [
              {
                ""x"": 1019,
                ""y"": 225
              },
              {
                ""x"": 1020,
                ""y"": 230
              },
              {
                ""x"": 1009,
                ""y"": 231
              },
              {
                ""x"": 1008,
                ""y"": 226
              }
            ],
            ""confidence"": 0.144
          },
          {
            ""text"": ""Wand"",
            ""boundingPolygon"": [
              {
                ""x"": 1020,
                ""y"": 234
              },
              {
                ""x"": 1019,
                ""y"": 258
              },
              {
                ""x"": 1009,
                ""y"": 259
              },
              {
                ""x"": 1009,
                ""y"": 235
              }
            ],
            ""confidence"": 0.991
          }
        ]
      },
      {
        ""text"": ""Cobbled Wings"",
        ""boundingPolygon"": [
          {
            ""x"": 1010,
            ""y"": 430
          },
          {
            ""x"": 1008,
            ""y"": 502
          },
          {
            ""x"": 997,
            ""y"": 502
          },
          {
            ""x"": 998,
            ""y"": 430
          }
        ],
        ""words"": [
          {
            ""text"": ""Cobbled"",
            ""boundingPolygon"": [
              {
                ""x"": 1011,
                ""y"": 432
              },
              {
                ""x"": 1009,
                ""y"": 469
              },
              {
                ""x"": 997,
                ""y"": 471
              },
              {
                ""x"": 998,
                ""y"": 431
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""Wings"",
            ""boundingPolygon"": [
              {
                ""x"": 1009,
                ""y"": 471
              },
              {
                ""x"": 1008,
                ""y"": 499
              },
              {
                ""x"": 998,
                ""y"": 502
              },
              {
                ""x"": 997,
                ""y"": 473
              }
            ],
            ""confidence"": 0.917
          }
        ]
      },
      {
        ""text"": ""Healer's Headdress"",
        ""boundingPolygon"": [
          {
            ""x"": 991,
            ""y"": 697
          },
          {
            ""x"": 990,
            ""y"": 789
          },
          {
            ""x"": 983,
            ""y"": 789
          },
          {
            ""x"": 983,
            ""y"": 697
          }
        ],
        ""words"": [
          {
            ""text"": ""Healer's"",
            ""boundingPolygon"": [
              {
                ""x"": 992,
                ""y"": 700
              },
              {
                ""x"": 991,
                ""y"": 737
              },
              {
                ""x"": 984,
                ""y"": 739
              },
              {
                ""x"": 983,
                ""y"": 701
              }
            ],
            ""confidence"": 0.941
          },
          {
            ""text"": ""Headdress"",
            ""boundingPolygon"": [
              {
                ""x"": 991,
                ""y"": 741
              },
              {
                ""x"": 990,
                ""y"": 786
              },
              {
                ""x"": 984,
                ""y"": 789
              },
              {
                ""x"": 984,
                ""y"": 742
              }
            ],
            ""confidence"": 0.989
          }
        ]
      },
      {
        ""text"": ""Sparring Collar"",
        ""boundingPolygon"": [
          {
            ""x"": 982,
            ""y"": 957
          },
          {
            ""x"": 983,
            ""y"": 1026
          },
          {
            ""x"": 971,
            ""y"": 1027
          },
          {
            ""x"": 971,
            ""y"": 957
          }
        ],
        ""words"": [
          {
            ""text"": ""Sparring"",
            ""boundingPolygon"": [
              {
                ""x"": 982,
                ""y"": 958
              },
              {
                ""x"": 983,
                ""y"": 997
              },
              {
                ""x"": 971,
                ""y"": 997
              },
              {
                ""x"": 972,
                ""y"": 957
              }
            ],
            ""confidence"": 0.975
          },
          {
            ""text"": ""Collar"",
            ""boundingPolygon"": [
              {
                ""x"": 983,
                ""y"": 1000
              },
              {
                ""x"": 984,
                ""y"": 1026
              },
              {
                ""x"": 972,
                ""y"": 1027
              },
              {
                ""x"": 971,
                ""y"": 1000
              }
            ],
            ""confidence"": 0.913
          }
        ]
      },
      {
        ""text"": ""Inventor's Goggles"",
        ""boundingPolygon"": [
          {
            ""x"": 995,
            ""y"": 1192
          },
          {
            ""x"": 993,
            ""y"": 1285
          },
          {
            ""x"": 982,
            ""y"": 1285
          },
          {
            ""x"": 984,
            ""y"": 1192
          }
        ],
        ""words"": [
          {
            ""text"": ""Inventor's"",
            ""boundingPolygon"": [
              {
                ""x"": 996,
                ""y"": 1193
              },
              {
                ""x"": 994,
                ""y"": 1237
              },
              {
                ""x"": 983,
                ""y"": 1237
              },
              {
                ""x"": 985,
                ""y"": 1193
              }
            ],
            ""confidence"": 0.965
          },
          {
            ""text"": ""Goggles"",
            ""boundingPolygon"": [
              {
                ""x"": 994,
                ""y"": 1239
              },
              {
                ""x"": 994,
                ""y"": 1274
              },
              {
                ""x"": 983,
                ""y"": 1276
              },
              {
                ""x"": 983,
                ""y"": 1240
              }
            ],
            ""confidence"": 0.958
          }
        ]
      },
      {
        ""text"": ""Veteran's Sidearm"",
        ""boundingPolygon"": [
          {
            ""x"": 1002,
            ""y"": 1415
          },
          {
            ""x"": 1010,
            ""y"": 1512
          },
          {
            ""x"": 999,
            ""y"": 1513
          },
          {
            ""x"": 991,
            ""y"": 1415
          }
        ],
        ""words"": [
          {
            ""text"": ""Veteran's"",
            ""boundingPolygon"": [
              {
                ""x"": 1002,
                ""y"": 1415
              },
              {
                ""x"": 1004,
                ""y"": 1453
              },
              {
                ""x"": 994,
                ""y"": 1453
              },
              {
                ""x"": 991,
                ""y"": 1415
              }
            ],
            ""confidence"": 0.779
          },
          {
            ""text"": ""Sidearm"",
            ""boundingPolygon"": [
              {
                ""x"": 1004,
                ""y"": 1455
              },
              {
                ""x"": 1007,
                ""y"": 1488
              },
              {
                ""x"": 997,
                ""y"": 1488
              },
              {
                ""x"": 994,
                ""y"": 1455
              }
            ],
            ""confidence"": 0.993
          }
        ]
      },
      {
        ""text"": ""Short Sword"",
        ""boundingPolygon"": [
          {
            ""x"": 1011,
            ""y"": 1653
          },
          {
            ""x"": 1014,
            ""y"": 1720
          },
          {
            ""x"": 999,
            ""y"": 1720
          },
          {
            ""x"": 998,
            ""y"": 1653
          }
        ],
        ""words"": [
          {
            ""text"": ""Short"",
            ""boundingPolygon"": [
              {
                ""x"": 1012,
                ""y"": 1654
              },
              {
                ""x"": 1011,
                ""y"": 1676
              },
              {
                ""x"": 998,
                ""y"": 1677
              },
              {
                ""x"": 999,
                ""y"": 1654
              }
            ],
            ""confidence"": 0.991
          },
          {
            ""text"": ""Sword"",
            ""boundingPolygon"": [
              {
                ""x"": 1011,
                ""y"": 1679
              },
              {
                ""x"": 1013,
                ""y"": 1706
              },
              {
                ""x"": 999,
                ""y"": 1707
              },
              {
                ""x"": 998,
                ""y"": 1679
              }
            ],
            ""confidence"": 0.989
          }
        ]
      },
      {
        ""text"": ""Tribal Artifact - Wizard"",
        ""boundingPolygon"": [
          {
            ""x"": 897,
            ""y"": 162
          },
          {
            ""x"": 886,
            ""y"": 290
          },
          {
            ""x"": 877,
            ""y"": 289
          },
          {
            ""x"": 887,
            ""y"": 161
          }
        ],
        ""words"": [
          {
            ""text"": ""Tribal"",
            ""boundingPolygon"": [
              {
                ""x"": 897,
                ""y"": 168
              },
              {
                ""x"": 894,
                ""y"": 192
              },
              {
                ""x"": 885,
                ""y"": 192
              },
              {
                ""x"": 887,
                ""y"": 167
              }
            ],
            ""confidence"": 0.923
          },
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 894,
                ""y"": 194
              },
              {
                ""x"": 892,
                ""y"": 223
              },
              {
                ""x"": 882,
                ""y"": 224
              },
              {
                ""x"": 884,
                ""y"": 194
              }
            ],
            ""confidence"": 0.82
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 891,
                ""y"": 225
              },
              {
                ""x"": 891,
                ""y"": 230
              },
              {
                ""x"": 881,
                ""y"": 231
              },
              {
                ""x"": 882,
                ""y"": 226
              }
            ],
            ""confidence"": 0.173
          },
          {
            ""text"": ""Wizard"",
            ""boundingPolygon"": [
              {
                ""x"": 890,
                ""y"": 235
              },
              {
                ""x"": 888,
                ""y"": 262
              },
              {
                ""x"": 879,
                ""y"": 263
              },
              {
                ""x"": 881,
                ""y"": 236
              }
            ],
            ""confidence"": 0.124
          }
        ]
      },
      {
        ""text"": ""aw pe creature"",
        ""boundingPolygon"": [
          {
            ""x"": 875,
            ""y"": 165
          },
          {
            ""x"": 870,
            ""y"": 238
          },
          {
            ""x"": 860,
            ""y"": 237
          },
          {
            ""x"": 863,
            ""y"": 165
          }
        ],
        ""words"": [
          {
            ""text"": ""aw"",
            ""boundingPolygon"": [
              {
                ""x"": 874,
                ""y"": 171
              },
              {
                ""x"": 873,
                ""y"": 181
              },
              {
                ""x"": 863,
                ""y"": 181
              },
              {
                ""x"": 863,
                ""y"": 172
              }
            ],
            ""confidence"": 0.119
          },
          {
            ""text"": ""pe"",
            ""boundingPolygon"": [
              {
                ""x"": 873,
                ""y"": 184
              },
              {
                ""x"": 872,
                ""y"": 198
              },
              {
                ""x"": 862,
                ""y"": 199
              },
              {
                ""x"": 862,
                ""y"": 184
              }
            ],
            ""confidence"": 0.118
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 872,
                ""y"": 204
              },
              {
                ""x"": 871,
                ""y"": 236
              },
              {
                ""x"": 861,
                ""y"": 237
              },
              {
                ""x"": 861,
                ""y"": 206
              }
            ],
            ""confidence"": 0.228
          }
        ]
      },
      {
        ""text"": ""Wizard Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 886,
            ""y"": 225
          },
          {
            ""x"": 877,
            ""y"": 326
          },
          {
            ""x"": 866,
            ""y"": 325
          },
          {
            ""x"": 876,
            ""y"": 225
          }
        ],
        ""words"": [
          {
            ""text"": ""Wizard"",
            ""boundingPolygon"": [
              {
                ""x"": 885,
                ""y"": 234
              },
              {
                ""x"": 884,
                ""y"": 261
              },
              {
                ""x"": 874,
                ""y"": 263
              },
              {
                ""x"": 876,
                ""y"": 236
              }
            ],
            ""confidence"": 0.864
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 884,
                ""y"": 265
              },
              {
                ""x"": 878,
                ""y"": 308
              },
              {
                ""x"": 869,
                ""y"": 309
              },
              {
                ""x"": 874,
                ""y"": 266
              }
            ],
            ""confidence"": 0.795
          }
        ]
      },
      {
        ""text"": ""draw a card, this has \""Whenever y"",
        ""boundingPolygon"": [
          {
            ""x"": 870,
            ""y"": 166
          },
          {
            ""x"": 865,
            ""y"": 304
          },
          {
            ""x"": 852,
            ""y"": 303
          },
          {
            ""x"": 856,
            ""y"": 165
          }
        ],
        ""words"": [
          {
            ""text"": ""draw"",
            ""boundingPolygon"": [
              {
                ""x"": 869,
                ""y"": 167
              },
              {
                ""x"": 869,
                ""y"": 182
              },
              {
                ""x"": 856,
                ""y"": 181
              },
              {
                ""x"": 856,
                ""y"": 166
              }
            ],
            ""confidence"": 0.446
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 869,
                ""y"": 184
              },
              {
                ""x"": 869,
                ""y"": 190
              },
              {
                ""x"": 857,
                ""y"": 189
              },
              {
                ""x"": 857,
                ""y"": 183
              }
            ],
            ""confidence"": 1
          },
          {
            ""text"": ""card,"",
            ""boundingPolygon"": [
              {
                ""x"": 869,
                ""y"": 192
              },
              {
                ""x"": 869,
                ""y"": 211
              },
              {
                ""x"": 857,
                ""y"": 210
              },
              {
                ""x"": 857,
                ""y"": 191
              }
            ],
            ""confidence"": 0.999
          },
          {
            ""text"": ""this"",
            ""boundingPolygon"": [
              {
                ""x"": 869,
                ""y"": 213
              },
              {
                ""x"": 868,
                ""y"": 230
              },
              {
                ""x"": 856,
                ""y"": 229
              },
              {
                ""x"": 857,
                ""y"": 212
              }
            ],
            ""confidence"": 0.957
          },
          {
            ""text"": ""has"",
            ""boundingPolygon"": [
              {
                ""x"": 868,
                ""y"": 238
              },
              {
                ""x"": 867,
                ""y"": 250
              },
              {
                ""x"": 856,
                ""y"": 249
              },
              {
                ""x"": 856,
                ""y"": 237
              }
            ],
            ""confidence"": 0.978
          },
          {
            ""text"": ""\""Whenever"",
            ""boundingPolygon"": [
              {
                ""x"": 867,
                ""y"": 252
              },
              {
                ""x"": 863,
                ""y"": 295
              },
              {
                ""x"": 853,
                ""y"": 295
              },
              {
                ""x"": 856,
                ""y"": 252
              }
            ],
            ""confidence"": 0.835
          },
          {
            ""text"": ""y"",
            ""boundingPolygon"": [
              {
                ""x"": 863,
                ""y"": 298
              },
              {
                ""x"": 862,
                ""y"": 304
              },
              {
                ""x"": 852,
                ""y"": 303
              },
              {
                ""x"": 853,
                ""y"": 297
              }
            ],
            ""confidence"": 0.892
          }
        ]
      },
      {
        ""text"": ""Artifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 876,
            ""y"": 423
          },
          {
            ""x"": 872,
            ""y"": 511
          },
          {
            ""x"": 859,
            ""y"": 510
          },
          {
            ""x"": 863,
            ""y"": 423
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 875,
                ""y"": 424
              },
              {
                ""x"": 874,
                ""y"": 453
              },
              {
                ""x"": 862,
                ""y"": 454
              },
              {
                ""x"": 863,
                ""y"": 425
              }
            ],
            ""confidence"": 0.735
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 874,
                ""y"": 455
              },
              {
                ""x"": 874,
                ""y"": 462
              },
              {
                ""x"": 862,
                ""y"": 463
              },
              {
                ""x"": 862,
                ""y"": 456
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 874,
                ""y"": 466
              },
              {
                ""x"": 872,
                ""y"": 509
              },
              {
                ""x"": 860,
                ""y"": 511
              },
              {
                ""x"": 862,
                ""y"": 467
              }
            ],
            ""confidence"": 0.96
          }
        ]
      },
      {
        ""text"": ""Whenever a W."",
        ""boundingPolygon"": [
          {
            ""x"": 848,
            ""y"": 162
          },
          {
            ""x"": 844,
            ""y"": 242
          },
          {
            ""x"": 831,
            ""y"": 241
          },
          {
            ""x"": 833,
            ""y"": 161
          }
        ],
        ""words"": [
          {
            ""text"": ""Whenever"",
            ""boundingPolygon"": [
              {
                ""x"": 849,
                ""y"": 164
              },
              {
                ""x"": 844,
                ""y"": 196
              },
              {
                ""x"": 832,
                ""y"": 195
              },
              {
                ""x"": 835,
                ""y"": 162
              }
            ],
            ""confidence"": 0.832
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 844,
                ""y"": 199
              },
              {
                ""x"": 844,
                ""y"": 201
              },
              {
                ""x"": 831,
                ""y"": 201
              },
              {
                ""x"": 831,
                ""y"": 198
              }
            ],
            ""confidence"": 0.975
          },
          {
            ""text"": ""W."",
            ""boundingPolygon"": [
              {
                ""x"": 843,
                ""y"": 204
              },
              {
                ""x"": 844,
                ""y"": 235
              },
              {
                ""x"": 832,
                ""y"": 236
              },
              {
                ""x"": 831,
                ""y"": 203
              }
            ],
            ""confidence"": 0.001
          }
        ]
      },
      {
        ""text"": ""of run\"" and"",
        ""boundingPolygon"": [
          {
            ""x"": 850,
            ""y"": 264
          },
          {
            ""x"": 844,
            ""y"": 314
          },
          {
            ""x"": 832,
            ""y"": 313
          },
          {
            ""x"": 837,
            ""y"": 263
          }
        ],
        ""words"": [
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 850,
                ""y"": 264
              },
              {
                ""x"": 849,
                ""y"": 269
              },
              {
                ""x"": 837,
                ""y"": 268
              },
              {
                ""x"": 838,
                ""y"": 263
              }
            ],
            ""confidence"": 0.556
          },
          {
            ""text"": ""run\"""",
            ""boundingPolygon"": [
              {
                ""x"": 849,
                ""y"": 271
              },
              {
                ""x"": 847,
                ""y"": 292
              },
              {
                ""x"": 835,
                ""y"": 291
              },
              {
                ""x"": 837,
                ""y"": 270
              }
            ],
            ""confidence"": 0.467
          },
          {
            ""text"": ""and"",
            ""boundingPolygon"": [
              {
                ""x"": 847,
                ""y"": 295
              },
              {
                ""x"": 845,
                ""y"": 310
              },
              {
                ""x"": 833,
                ""y"": 309
              },
              {
                ""x"": 834,
                ""y"": 294
              }
            ],
            ""confidence"": 0.958
          }
        ]
      },
      {
        ""text"": ""play, you may attach Divines und"",
        ""boundingPolygon"": [
          {
            ""x"": 834,
            ""y"": 157
          },
          {
            ""x"": 824,
            ""y"": 309
          },
          {
            ""x"": 810,
            ""y"": 308
          },
          {
            ""x"": 821,
            ""y"": 156
          }
        ],
        ""words"": [
          {
            ""text"": ""play,"",
            ""boundingPolygon"": [
              {
                ""x"": 834,
                ""y"": 158
              },
              {
                ""x"": 833,
                ""y"": 174
              },
              {
                ""x"": 821,
                ""y"": 173
              },
              {
                ""x"": 821,
                ""y"": 157
              }
            ],
            ""confidence"": 0.832
          },
          {
            ""text"": ""you"",
            ""boundingPolygon"": [
              {
                ""x"": 833,
                ""y"": 177
              },
              {
                ""x"": 833,
                ""y"": 190
              },
              {
                ""x"": 820,
                ""y"": 189
              },
              {
                ""x"": 820,
                ""y"": 176
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""may"",
            ""boundingPolygon"": [
              {
                ""x"": 833,
                ""y"": 192
              },
              {
                ""x"": 832,
                ""y"": 208
              },
              {
                ""x"": 819,
                ""y"": 208
              },
              {
                ""x"": 820,
                ""y"": 192
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""attach"",
            ""boundingPolygon"": [
              {
                ""x"": 832,
                ""y"": 211
              },
              {
                ""x"": 830,
                ""y"": 234
              },
              {
                ""x"": 817,
                ""y"": 234
              },
              {
                ""x"": 819,
                ""y"": 210
              }
            ],
            ""confidence"": 0.592
          },
          {
            ""text"": ""Divines"",
            ""boundingPolygon"": [
              {
                ""x"": 830,
                ""y"": 236
              },
              {
                ""x"": 827,
                ""y"": 267
              },
              {
                ""x"": 814,
                ""y"": 267
              },
              {
                ""x"": 817,
                ""y"": 236
              }
            ],
            ""confidence"": 0.122
          },
          {
            ""text"": ""und"",
            ""boundingPolygon"": [
              {
                ""x"": 826,
                ""y"": 271
              },
              {
                ""x"": 824,
                ""y"": 294
              },
              {
                ""x"": 811,
                ""y"": 295
              },
              {
                ""x"": 814,
                ""y"": 272
              }
            ],
            ""confidence"": 0.118
          }
        ]
      },
      {
        ""text"": ""Equip 3"",
        ""boundingPolygon"": [
          {
            ""x"": 826,
            ""y"": 157
          },
          {
            ""x"": 818,
            ""y"": 204
          },
          {
            ""x"": 809,
            ""y"": 203
          },
          {
            ""x"": 816,
            ""y"": 157
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 826,
                ""y"": 158
              },
              {
                ""x"": 823,
                ""y"": 178
              },
              {
                ""x"": 813,
                ""y"": 178
              },
              {
                ""x"": 816,
                ""y"": 158
              }
            ],
            ""confidence"": 0.879
          },
          {
            ""text"": ""3"",
            ""boundingPolygon"": [
              {
                ""x"": 822,
                ""y"": 183
              },
              {
                ""x"": 821,
                ""y"": 187
              },
              {
                ""x"": 812,
                ""y"": 187
              },
              {
                ""x"": 813,
                ""y"": 183
              }
            ],
            ""confidence"": 0.588
          }
        ]
      },
      {
        ""text"": ""quipped creature"",
        ""boundingPolygon"": [
          {
            ""x"": 842,
            ""y"": 428
          },
          {
            ""x"": 842,
            ""y"": 500
          },
          {
            ""x"": 830,
            ""y"": 500
          },
          {
            ""x"": 830,
            ""y"": 428
          }
        ],
        ""words"": [
          {
            ""text"": ""quipped"",
            ""boundingPolygon"": [
              {
                ""x"": 842,
                ""y"": 430
              },
              {
                ""x"": 842,
                ""y"": 465
              },
              {
                ""x"": 831,
                ""y"": 465
              },
              {
                ""x"": 831,
                ""y"": 430
              }
            ],
            ""confidence"": 0.957
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 842,
                ""y"": 467
              },
              {
                ""x"": 842,
                ""y"": 500
              },
              {
                ""x"": 831,
                ""y"": 500
              },
              {
                ""x"": 831,
                ""y"": 468
              }
            ],
            ""confidence"": 0.797
          }
        ]
      },
      {
        ""text"": ""Artifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 860,
            ""y"": 701
          },
          {
            ""x"": 861,
            ""y"": 798
          },
          {
            ""x"": 849,
            ""y"": 799
          },
          {
            ""x"": 849,
            ""y"": 701
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 858,
                ""y"": 702
              },
              {
                ""x"": 860,
                ""y"": 729
              },
              {
                ""x"": 850,
                ""y"": 729
              },
              {
                ""x"": 849,
                ""y"": 701
              }
            ],
            ""confidence"": 0.951
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 860,
                ""y"": 731
              },
              {
                ""x"": 860,
                ""y"": 737
              },
              {
                ""x"": 850,
                ""y"": 737
              },
              {
                ""x"": 850,
                ""y"": 731
              }
            ],
            ""confidence"": 0.809
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 861,
                ""y"": 741
              },
              {
                ""x"": 861,
                ""y"": 784
              },
              {
                ""x"": 850,
                ""y"": 784
              },
              {
                ""x"": 850,
                ""y"": 740
              }
            ],
            ""confidence"": 0.924
          }
        ]
      },
      {
        ""text"": ""iquip 1 (1- Attach to target creature"",
        ""boundingPolygon"": [
          {
            ""x"": 832,
            ""y"": 427
          },
          {
            ""x"": 830,
            ""y"": 574
          },
          {
            ""x"": 815,
            ""y"": 574
          },
          {
            ""x"": 816,
            ""y"": 427
          }
        ],
        ""words"": [
          {
            ""text"": ""iquip"",
            ""boundingPolygon"": [
              {
                ""x"": 832,
                ""y"": 428
              },
              {
                ""x"": 831,
                ""y"": 450
              },
              {
                ""x"": 816,
                ""y"": 449
              },
              {
                ""x"": 816,
                ""y"": 427
              }
            ],
            ""confidence"": 0.848
          },
          {
            ""text"": ""1"",
            ""boundingPolygon"": [
              {
                ""x"": 831,
                ""y"": 453
              },
              {
                ""x"": 831,
                ""y"": 461
              },
              {
                ""x"": 816,
                ""y"": 460
              },
              {
                ""x"": 816,
                ""y"": 453
              }
            ],
            ""confidence"": 0.689
          },
          {
            ""text"": ""(1-"",
            ""boundingPolygon"": [
              {
                ""x"": 831,
                ""y"": 464
              },
              {
                ""x"": 830,
                ""y"": 477
              },
              {
                ""x"": 815,
                ""y"": 477
              },
              {
                ""x"": 815,
                ""y"": 463
              }
            ],
            ""confidence"": 0.149
          },
          {
            ""text"": ""Attach"",
            ""boundingPolygon"": [
              {
                ""x"": 830,
                ""y"": 480
              },
              {
                ""x"": 830,
                ""y"": 506
              },
              {
                ""x"": 815,
                ""y"": 506
              },
              {
                ""x"": 815,
                ""y"": 480
              }
            ],
            ""confidence"": 0.343
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 830,
                ""y"": 509
              },
              {
                ""x"": 829,
                ""y"": 516
              },
              {
                ""x"": 815,
                ""y"": 515
              },
              {
                ""x"": 815,
                ""y"": 509
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 829,
                ""y"": 519
              },
              {
                ""x"": 829,
                ""y"": 542
              },
              {
                ""x"": 815,
                ""y"": 542
              },
              {
                ""x"": 815,
                ""y"": 518
              }
            ],
            ""confidence"": 0.947
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 829,
                ""y"": 545
              },
              {
                ""x"": 828,
                ""y"": 574
              },
              {
                ""x"": 815,
                ""y"": 574
              },
              {
                ""x"": 815,
                ""y"": 544
              }
            ],
            ""confidence"": 0.619
          }
        ]
      },
      {
        ""text"": ""Prevent the next I damage that would be"",
        ""boundingPolygon"": [
          {
            ""x"": 837,
            ""y"": 703
          },
          {
            ""x"": 838,
            ""y"": 848
          },
          {
            ""x"": 823,
            ""y"": 848
          },
          {
            ""x"": 822,
            ""y"": 703
          }
        ],
        ""words"": [
          {
            ""text"": ""Prevent"",
            ""boundingPolygon"": [
              {
                ""x"": 836,
                ""y"": 703
              },
              {
                ""x"": 837,
                ""y"": 727
              },
              {
                ""x"": 823,
                ""y"": 727
              },
              {
                ""x"": 823,
                ""y"": 703
              }
            ],
            ""confidence"": 0.936
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 837,
                ""y"": 730
              },
              {
                ""x"": 837,
                ""y"": 740
              },
              {
                ""x"": 823,
                ""y"": 740
              },
              {
                ""x"": 823,
                ""y"": 730
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""next"",
            ""boundingPolygon"": [
              {
                ""x"": 837,
                ""y"": 743
              },
              {
                ""x"": 838,
                ""y"": 758
              },
              {
                ""x"": 823,
                ""y"": 758
              },
              {
                ""x"": 823,
                ""y"": 743
              }
            ],
            ""confidence"": 0.648
          },
          {
            ""text"": ""I"",
            ""boundingPolygon"": [
              {
                ""x"": 838,
                ""y"": 761
              },
              {
                ""x"": 838,
                ""y"": 765
              },
              {
                ""x"": 823,
                ""y"": 765
              },
              {
                ""x"": 823,
                ""y"": 761
              }
            ],
            ""confidence"": 0.582
          },
          {
            ""text"": ""damage"",
            ""boundingPolygon"": [
              {
                ""x"": 838,
                ""y"": 768
              },
              {
                ""x"": 838,
                ""y"": 794
              },
              {
                ""x"": 823,
                ""y"": 794
              },
              {
                ""x"": 823,
                ""y"": 768
              }
            ],
            ""confidence"": 0.877
          },
          {
            ""text"": ""that"",
            ""boundingPolygon"": [
              {
                ""x"": 838,
                ""y"": 796
              },
              {
                ""x"": 838,
                ""y"": 809
              },
              {
                ""x"": 823,
                ""y"": 809
              },
              {
                ""x"": 823,
                ""y"": 796
              }
            ],
            ""confidence"": 0.85
          },
          {
            ""text"": ""would"",
            ""boundingPolygon"": [
              {
                ""x"": 838,
                ""y"": 812
              },
              {
                ""x"": 838,
                ""y"": 833
              },
              {
                ""x"": 823,
                ""y"": 833
              },
              {
                ""x"": 823,
                ""y"": 812
              }
            ],
            ""confidence"": 0.823
          },
          {
            ""text"": ""be"",
            ""boundingPolygon"": [
              {
                ""x"": 838,
                ""y"": 835
              },
              {
                ""x"": 837,
                ""y"": 848
              },
              {
                ""x"": 824,
                ""y"": 848
              },
              {
                ""x"": 824,
                ""y"": 835
              }
            ],
            ""confidence"": 0.586
          }
        ]
      },
      {
        ""text"": ""Artifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 852,
            ""y"": 971
          },
          {
            ""x"": 855,
            ""y"": 1097
          },
          {
            ""x"": 842,
            ""y"": 1097
          },
          {
            ""x"": 839,
            ""y"": 971
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 851,
                ""y"": 972
              },
              {
                ""x"": 853,
                ""y"": 999
              },
              {
                ""x"": 841,
                ""y"": 999
              },
              {
                ""x"": 839,
                ""y"": 972
              }
            ],
            ""confidence"": 0.966
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 853,
                ""y"": 1001
              },
              {
                ""x"": 853,
                ""y"": 1009
              },
              {
                ""x"": 842,
                ""y"": 1009
              },
              {
                ""x"": 842,
                ""y"": 1001
              }
            ],
            ""confidence"": 0.747
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 853,
                ""y"": 1012
              },
              {
                ""x"": 854,
                ""y"": 1054
              },
              {
                ""x"": 843,
                ""y"": 1054
              },
              {
                ""x"": 842,
                ""y"": 1012
              }
            ],
            ""confidence"": 0.848
          }
        ]
      },
      {
        ""text"": ""Artifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 861,
            ""y"": 1206
          },
          {
            ""x"": 864,
            ""y"": 1294
          },
          {
            ""x"": 850,
            ""y"": 1295
          },
          {
            ""x"": 849,
            ""y"": 1206
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 859,
                ""y"": 1207
              },
              {
                ""x"": 862,
                ""y"": 1231
              },
              {
                ""x"": 851,
                ""y"": 1232
              },
              {
                ""x"": 849,
                ""y"": 1207
              }
            ],
            ""confidence"": 0.646
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 862,
                ""y"": 1233
              },
              {
                ""x"": 862,
                ""y"": 1239
              },
              {
                ""x"": 851,
                ""y"": 1240
              },
              {
                ""x"": 851,
                ""y"": 1234
              }
            ],
            ""confidence"": 0.82
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 862,
                ""y"": 1242
              },
              {
                ""x"": 863,
                ""y"": 1285
              },
              {
                ""x"": 851,
                ""y"": 1287
              },
              {
                ""x"": 851,
                ""y"": 1243
              }
            ],
            ""confidence"": 0.952
          }
        ]
      },
      {
        ""text"": ""tifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 870,
            ""y"": 1438
          },
          {
            ""x"": 876,
            ""y"": 1525
          },
          {
            ""x"": 866,
            ""y"": 1526
          },
          {
            ""x"": 861,
            ""y"": 1438
          }
        ],
        ""words"": [
          {
            ""text"": ""tifact"",
            ""boundingPolygon"": [
              {
                ""x"": 870,
                ""y"": 1441
              },
              {
                ""x"": 872,
                ""y"": 1460
              },
              {
                ""x"": 863,
                ""y"": 1461
              },
              {
                ""x"": 861,
                ""y"": 1441
              }
            ],
            ""confidence"": 0.362
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 872,
                ""y"": 1462
              },
              {
                ""x"": 872,
                ""y"": 1466
              },
              {
                ""x"": 864,
                ""y"": 1467
              },
              {
                ""x"": 863,
                ""y"": 1463
              }
            ],
            ""confidence"": 0.938
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 873,
                ""y"": 1472
              },
              {
                ""x"": 875,
                ""y"": 1513
              },
              {
                ""x"": 866,
                ""y"": 1515
              },
              {
                ""x"": 864,
                ""y"": 1472
              }
            ],
            ""confidence"": 0.798
          }
        ]
      },
      {
        ""text"": ""Artifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 880,
            ""y"": 1665
          },
          {
            ""x"": 879,
            ""y"": 1765
          },
          {
            ""x"": 867,
            ""y"": 1765
          },
          {
            ""x"": 867,
            ""y"": 1665
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 879,
                ""y"": 1665
              },
              {
                ""x"": 880,
                ""y"": 1690
              },
              {
                ""x"": 867,
                ""y"": 1691
              },
              {
                ""x"": 867,
                ""y"": 1666
              }
            ],
            ""confidence"": 0.747
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 880,
                ""y"": 1692
              },
              {
                ""x"": 880,
                ""y"": 1699
              },
              {
                ""x"": 867,
                ""y"": 1700
              },
              {
                ""x"": 867,
                ""y"": 1694
              }
            ],
            ""confidence"": 0.882
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 880,
                ""y"": 1701
              },
              {
                ""x"": 879,
                ""y"": 1744
              },
              {
                ""x"": 867,
                ""y"": 1746
              },
              {
                ""x"": 867,
                ""y"": 1703
              }
            ],
            ""confidence"": 0.848
          }
        ]
      },
      {
        ""text"": ""Id together by optimism."",
        ""boundingPolygon"": [
          {
            ""x"": 807,
            ""y"": 465
          },
          {
            ""x"": 806,
            ""y"": 557
          },
          {
            ""x"": 796,
            ""y"": 557
          },
          {
            ""x"": 796,
            ""y"": 465
          }
        ],
        ""words"": [
          {
            ""text"": ""Id"",
            ""boundingPolygon"": [
              {
                ""x"": 807,
                ""y"": 466
              },
              {
                ""x"": 807,
                ""y"": 473
              },
              {
                ""x"": 797,
                ""y"": 472
              },
              {
                ""x"": 797,
                ""y"": 465
              }
            ],
            ""confidence"": 0.59
          },
          {
            ""text"": ""together"",
            ""boundingPolygon"": [
              {
                ""x"": 807,
                ""y"": 475
              },
              {
                ""x"": 807,
                ""y"": 506
              },
              {
                ""x"": 797,
                ""y"": 506
              },
              {
                ""x"": 797,
                ""y"": 474
              }
            ],
            ""confidence"": 0.607
          },
          {
            ""text"": ""by"",
            ""boundingPolygon"": [
              {
                ""x"": 807,
                ""y"": 508
              },
              {
                ""x"": 807,
                ""y"": 518
              },
              {
                ""x"": 797,
                ""y"": 517
              },
              {
                ""x"": 797,
                ""y"": 508
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""optimism."",
            ""boundingPolygon"": [
              {
                ""x"": 807,
                ""y"": 520
              },
              {
                ""x"": 806,
                ""y"": 557
              },
              {
                ""x"": 796,
                ""y"": 557
              },
              {
                ""x"": 797,
                ""y"": 520
              }
            ],
            ""confidence"": 0.579
          }
        ]
      },
      {
        ""text"": ""alt to target creature of player this turn."",
        ""boundingPolygon"": [
          {
            ""x"": 822,
            ""y"": 710
          },
          {
            ""x"": 825,
            ""y"": 850
          },
          {
            ""x"": 809,
            ""y"": 850
          },
          {
            ""x"": 808,
            ""y"": 710
          }
        ],
        ""words"": [
          {
            ""text"": ""alt"",
            ""boundingPolygon"": [
              {
                ""x"": 821,
                ""y"": 711
              },
              {
                ""x"": 822,
                ""y"": 717
              },
              {
                ""x"": 809,
                ""y"": 716
              },
              {
                ""x"": 809,
                ""y"": 710
              }
            ],
            ""confidence"": 0.434
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 822,
                ""y"": 720
              },
              {
                ""x"": 822,
                ""y"": 727
              },
              {
                ""x"": 809,
                ""y"": 726
              },
              {
                ""x"": 809,
                ""y"": 719
              }
            ],
            ""confidence"": 0.921
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 822,
                ""y"": 729
              },
              {
                ""x"": 823,
                ""y"": 749
              },
              {
                ""x"": 809,
                ""y"": 748
              },
              {
                ""x"": 809,
                ""y"": 728
              }
            ],
            ""confidence"": 0.697
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 823,
                ""y"": 752
              },
              {
                ""x"": 824,
                ""y"": 780
              },
              {
                ""x"": 809,
                ""y"": 778
              },
              {
                ""x"": 809,
                ""y"": 751
              }
            ],
            ""confidence"": 0.685
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 824,
                ""y"": 783
              },
              {
                ""x"": 824,
                ""y"": 789
              },
              {
                ""x"": 809,
                ""y"": 788
              },
              {
                ""x"": 809,
                ""y"": 781
              }
            ],
            ""confidence"": 0.357
          },
          {
            ""text"": ""player"",
            ""boundingPolygon"": [
              {
                ""x"": 824,
                ""y"": 792
              },
              {
                ""x"": 824,
                ""y"": 813
              },
              {
                ""x"": 809,
                ""y"": 811
              },
              {
                ""x"": 809,
                ""y"": 791
              }
            ],
            ""confidence"": 0.624
          },
          {
            ""text"": ""this"",
            ""boundingPolygon"": [
              {
                ""x"": 824,
                ""y"": 816
              },
              {
                ""x"": 825,
                ""y"": 827
              },
              {
                ""x"": 809,
                ""y"": 826
              },
              {
                ""x"": 809,
                ""y"": 814
              }
            ],
            ""confidence"": 0.98
          },
          {
            ""text"": ""turn."",
            ""boundingPolygon"": [
              {
                ""x"": 825,
                ""y"": 830
              },
              {
                ""x"": 825,
                ""y"": 850
              },
              {
                ""x"": 810,
                ""y"": 848
              },
              {
                ""x"": 809,
                ""y"": 828
              }
            ],
            ""confidence"": 0.751
          }
        ]
      },
      {
        ""text"": ""Equipped creature gets the"",
        ""boundingPolygon"": [
          {
            ""x"": 855,
            ""y"": 1431
          },
          {
            ""x"": 862,
            ""y"": 1574
          },
          {
            ""x"": 848,
            ""y"": 1575
          },
          {
            ""x"": 840,
            ""y"": 1432
          }
        ],
        ""words"": [
          {
            ""text"": ""Equipped"",
            ""boundingPolygon"": [
              {
                ""x"": 853,
                ""y"": 1433
              },
              {
                ""x"": 858,
                ""y"": 1474
              },
              {
                ""x"": 846,
                ""y"": 1476
              },
              {
                ""x"": 841,
                ""y"": 1435
              }
            ],
            ""confidence"": 0.999
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 858,
                ""y"": 1476
              },
              {
                ""x"": 860,
                ""y"": 1509
              },
              {
                ""x"": 848,
                ""y"": 1511
              },
              {
                ""x"": 846,
                ""y"": 1478
              }
            ],
            ""confidence"": 0.999
          },
          {
            ""text"": ""gets"",
            ""boundingPolygon"": [
              {
                ""x"": 860,
                ""y"": 1512
              },
              {
                ""x"": 861,
                ""y"": 1530
              },
              {
                ""x"": 849,
                ""y"": 1532
              },
              {
                ""x"": 848,
                ""y"": 1514
              }
            ],
            ""confidence"": 0.995
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 861,
                ""y"": 1533
              },
              {
                ""x"": 862,
                ""y"": 1566
              },
              {
                ""x"": 849,
                ""y"": 1568
              },
              {
                ""x"": 849,
                ""y"": 1534
              }
            ],
            ""confidence"": 0.255
          }
        ]
      },
      {
        ""text"": ""..: Attach Healer\"""",
        ""boundingPolygon"": [
          {
            ""x"": 816,
            ""y"": 701
          },
          {
            ""x"": 814,
            ""y"": 780
          },
          {
            ""x"": 802,
            ""y"": 779
          },
          {
            ""x"": 802,
            ""y"": 701
          }
        ],
        ""words"": [
          {
            ""text"": ""..:"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 701
              },
              {
                ""x"": 816,
                ""y"": 716
              },
              {
                ""x"": 803,
                ""y"": 716
              },
              {
                ""x"": 803,
                ""y"": 701
              }
            ],
            ""confidence"": 0.197
          },
          {
            ""text"": ""Attach"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 719
              },
              {
                ""x"": 816,
                ""y"": 742
              },
              {
                ""x"": 802,
                ""y"": 742
              },
              {
                ""x"": 803,
                ""y"": 719
              }
            ],
            ""confidence"": 0.737
          },
          {
            ""text"": ""Healer\"""",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 744
              },
              {
                ""x"": 814,
                ""y"": 775
              },
              {
                ""x"": 802,
                ""y"": 776
              },
              {
                ""x"": 802,
                ""y"": 745
              }
            ],
            ""confidence"": 0.584
          }
        ]
      },
      {
        ""text"": ""ature has first strike"",
        ""boundingPolygon"": [
          {
            ""x"": 830,
            ""y"": 1029
          },
          {
            ""x"": 838,
            ""y"": 1113
          },
          {
            ""x"": 827,
            ""y"": 1114
          },
          {
            ""x"": 820,
            ""y"": 1030
          }
        ],
        ""words"": [
          {
            ""text"": ""ature"",
            ""boundingPolygon"": [
              {
                ""x"": 830,
                ""y"": 1031
              },
              {
                ""x"": 832,
                ""y"": 1052
              },
              {
                ""x"": 821,
                ""y"": 1053
              },
              {
                ""x"": 820,
                ""y"": 1032
              }
            ],
            ""confidence"": 0.882
          },
          {
            ""text"": ""has"",
            ""boundingPolygon"": [
              {
                ""x"": 832,
                ""y"": 1054
              },
              {
                ""x"": 833,
                ""y"": 1069
              },
              {
                ""x"": 823,
                ""y"": 1070
              },
              {
                ""x"": 821,
                ""y"": 1055
              }
            ],
            ""confidence"": 0.995
          },
          {
            ""text"": ""first"",
            ""boundingPolygon"": [
              {
                ""x"": 833,
                ""y"": 1071
              },
              {
                ""x"": 835,
                ""y"": 1088
              },
              {
                ""x"": 825,
                ""y"": 1089
              },
              {
                ""x"": 823,
                ""y"": 1072
              }
            ],
            ""confidence"": 0.992
          },
          {
            ""text"": ""strike"",
            ""boundingPolygon"": [
              {
                ""x"": 835,
                ""y"": 1090
              },
              {
                ""x"": 838,
                ""y"": 1113
              },
              {
                ""x"": 829,
                ""y"": 1114
              },
              {
                ""x"": 825,
                ""y"": 1091
              }
            ],
            ""confidence"": 0.936
          }
        ]
      },
      {
        ""text"": ""ets + 1/+2"",
        ""boundingPolygon"": [
          {
            ""x"": 844,
            ""y"": 1286
          },
          {
            ""x"": 851,
            ""y"": 1329
          },
          {
            ""x"": 840,
            ""y"": 1331
          },
          {
            ""x"": 833,
            ""y"": 1288
          }
        ],
        ""words"": [
          {
            ""text"": ""ets"",
            ""boundingPolygon"": [
              {
                ""x"": 845,
                ""y"": 1287
              },
              {
                ""x"": 847,
                ""y"": 1300
              },
              {
                ""x"": 835,
                ""y"": 1301
              },
              {
                ""x"": 834,
                ""y"": 1289
              }
            ],
            ""confidence"": 0.877
          },
          {
            ""text"": ""+"",
            ""boundingPolygon"": [
              {
                ""x"": 847,
                ""y"": 1302
              },
              {
                ""x"": 848,
                ""y"": 1306
              },
              {
                ""x"": 836,
                ""y"": 1308
              },
              {
                ""x"": 836,
                ""y"": 1303
              }
            ],
            ""confidence"": 0.925
          },
          {
            ""text"": ""1/+2"",
            ""boundingPolygon"": [
              {
                ""x"": 848,
                ""y"": 1308
              },
              {
                ""x"": 851,
                ""y"": 1329
              },
              {
                ""x"": 840,
                ""y"": 1330
              },
              {
                ""x"": 837,
                ""y"": 1310
              }
            ],
            ""confidence"": 0.841
          }
        ]
      },
      {
        ""text"": ""Equipped freutun her enters the"",
        ""boundingPolygon"": [
          {
            ""x"": 838,
            ""y"": 1205
          },
          {
            ""x"": 837,
            ""y"": 1346
          },
          {
            ""x"": 823,
            ""y"": 1346
          },
          {
            ""x"": 823,
            ""y"": 1205
          }
        ],
        ""words"": [
          {
            ""text"": ""Equipped"",
            ""boundingPolygon"": [
              {
                ""x"": 837,
                ""y"": 1206
              },
              {
                ""x"": 838,
                ""y"": 1245
              },
              {
                ""x"": 825,
                ""y"": 1245
              },
              {
                ""x"": 824,
                ""y"": 1206
              }
            ],
            ""confidence"": 0.762
          },
          {
            ""text"": ""freutun"",
            ""boundingPolygon"": [
              {
                ""x"": 838,
                ""y"": 1248
              },
              {
                ""x"": 838,
                ""y"": 1279
              },
              {
                ""x"": 825,
                ""y"": 1279
              },
              {
                ""x"": 825,
                ""y"": 1247
              }
            ],
            ""confidence"": 0.126
          },
          {
            ""text"": ""her"",
            ""boundingPolygon"": [
              {
                ""x"": 837,
                ""y"": 1284
              },
              {
                ""x"": 837,
                ""y"": 1297
              },
              {
                ""x"": 825,
                ""y"": 1296
              },
              {
                ""x"": 825,
                ""y"": 1284
              }
            ],
            ""confidence"": 0.12
          },
          {
            ""text"": ""enters"",
            ""boundingPolygon"": [
              {
                ""x"": 837,
                ""y"": 1299
              },
              {
                ""x"": 836,
                ""y"": 1324
              },
              {
                ""x"": 824,
                ""y"": 1324
              },
              {
                ""x"": 825,
                ""y"": 1299
              }
            ],
            ""confidence"": 0.621
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 836,
                ""y"": 1327
              },
              {
                ""x"": 835,
                ""y"": 1341
              },
              {
                ""x"": 824,
                ""y"": 1341
              },
              {
                ""x"": 824,
                ""y"": 1326
              }
            ],
            ""confidence"": 0.82
          }
        ]
      },
      {
        ""text"": ""Equip 1 (LAthem as a sorcery.)"",
        ""boundingPolygon"": [
          {
            ""x"": 844,
            ""y"": 1435
          },
          {
            ""x"": 848,
            ""y"": 1581
          },
          {
            ""x"": 834,
            ""y"": 1582
          },
          {
            ""x"": 831,
            ""y"": 1435
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 844,
                ""y"": 1436
              },
              {
                ""x"": 845,
                ""y"": 1462
              },
              {
                ""x"": 831,
                ""y"": 1462
              },
              {
                ""x"": 831,
                ""y"": 1436
              }
            ],
            ""confidence"": 0.937
          },
          {
            ""text"": ""1"",
            ""boundingPolygon"": [
              {
                ""x"": 845,
                ""y"": 1465
              },
              {
                ""x"": 845,
                ""y"": 1472
              },
              {
                ""x"": 831,
                ""y"": 1472
              },
              {
                ""x"": 831,
                ""y"": 1465
              }
            ],
            ""confidence"": 0.27
          },
          {
            ""text"": ""(LAthem"",
            ""boundingPolygon"": [
              {
                ""x"": 845,
                ""y"": 1476
              },
              {
                ""x"": 846,
                ""y"": 1520
              },
              {
                ""x"": 833,
                ""y"": 1520
              },
              {
                ""x"": 832,
                ""y"": 1476
              }
            ],
            ""confidence"": 0.119
          },
          {
            ""text"": ""as"",
            ""boundingPolygon"": [
              {
                ""x"": 847,
                ""y"": 1528
              },
              {
                ""x"": 847,
                ""y"": 1537
              },
              {
                ""x"": 833,
                ""y"": 1537
              },
              {
                ""x"": 833,
                ""y"": 1528
              }
            ],
            ""confidence"": 0.787
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 847,
                ""y"": 1540
              },
              {
                ""x"": 847,
                ""y"": 1544
              },
              {
                ""x"": 834,
                ""y"": 1544
              },
              {
                ""x"": 834,
                ""y"": 1540
              }
            ],
            ""confidence"": 0.693
          },
          {
            ""text"": ""sorcery.)"",
            ""boundingPolygon"": [
              {
                ""x"": 847,
                ""y"": 1547
              },
              {
                ""x"": 848,
                ""y"": 1582
              },
              {
                ""x"": 835,
                ""y"": 1582
              },
              {
                ""x"": 834,
                ""y"": 1547
              }
            ],
            ""confidence"": 0.69
          }
        ]
      },
      {
        ""text"": ""Equipped freue, to target creature"",
        ""boundingPolygon"": [
          {
            ""x"": 855,
            ""y"": 1667
          },
          {
            ""x"": 854,
            ""y"": 1812
          },
          {
            ""x"": 838,
            ""y"": 1812
          },
          {
            ""x"": 839,
            ""y"": 1667
          }
        ],
        ""words"": [
          {
            ""text"": ""Equipped"",
            ""boundingPolygon"": [
              {
                ""x"": 854,
                ""y"": 1668
              },
              {
                ""x"": 855,
                ""y"": 1704
              },
              {
                ""x"": 840,
                ""y"": 1706
              },
              {
                ""x"": 839,
                ""y"": 1670
              }
            ],
            ""confidence"": 0.16
          },
          {
            ""text"": ""freue,"",
            ""boundingPolygon"": [
              {
                ""x"": 855,
                ""y"": 1707
              },
              {
                ""x"": 855,
                ""y"": 1743
              },
              {
                ""x"": 840,
                ""y"": 1745
              },
              {
                ""x"": 840,
                ""y"": 1709
              }
            ],
            ""confidence"": 0.119
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 855,
                ""y"": 1746
              },
              {
                ""x"": 855,
                ""y"": 1753
              },
              {
                ""x"": 840,
                ""y"": 1755
              },
              {
                ""x"": 840,
                ""y"": 1748
              }
            ],
            ""confidence"": 0.343
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 855,
                ""y"": 1756
              },
              {
                ""x"": 855,
                ""y"": 1777
              },
              {
                ""x"": 839,
                ""y"": 1779
              },
              {
                ""x"": 840,
                ""y"": 1758
              }
            ],
            ""confidence"": 0.695
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 855,
                ""y"": 1780
              },
              {
                ""x"": 854,
                ""y"": 1811
              },
              {
                ""x"": 838,
                ""y"": 1813
              },
              {
                ""x"": 839,
                ""y"": 1782
              }
            ],
            ""confidence"": 0.584
          }
        ]
      },
      {
        ""text"": ""you control."",
        ""boundingPolygon"": [
          {
            ""x"": 807,
            ""y"": 731
          },
          {
            ""x"": 806,
            ""y"": 784
          },
          {
            ""x"": 794,
            ""y"": 783
          },
          {
            ""x"": 795,
            ""y"": 731
          }
        ],
        ""words"": [
          {
            ""text"": ""you"",
            ""boundingPolygon"": [
              {
                ""x"": 807,
                ""y"": 731
              },
              {
                ""x"": 807,
                ""y"": 744
              },
              {
                ""x"": 795,
                ""y"": 744
              },
              {
                ""x"": 795,
                ""y"": 731
              }
            ],
            ""confidence"": 0.958
          },
          {
            ""text"": ""control."",
            ""boundingPolygon"": [
              {
                ""x"": 807,
                ""y"": 746
              },
              {
                ""x"": 806,
                ""y"": 777
              },
              {
                ""x"": 794,
                ""y"": 777
              },
              {
                ""x"": 795,
                ""y"": 746
              }
            ],
            ""confidence"": 0.835
          }
        ]
      },
      {
        ""text"": ""Equip 1 (1-Attack"",
        ""boundingPolygon"": [
          {
            ""x"": 845,
            ""y"": 1669
          },
          {
            ""x"": 847,
            ""y"": 1750
          },
          {
            ""x"": 832,
            ""y"": 1750
          },
          {
            ""x"": 829,
            ""y"": 1669
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 844,
                ""y"": 1669
              },
              {
                ""x"": 846,
                ""y"": 1692
              },
              {
                ""x"": 831,
                ""y"": 1693
              },
              {
                ""x"": 830,
                ""y"": 1670
              }
            ],
            ""confidence"": 0.879
          },
          {
            ""text"": ""1"",
            ""boundingPolygon"": [
              {
                ""x"": 846,
                ""y"": 1695
              },
              {
                ""x"": 846,
                ""y"": 1701
              },
              {
                ""x"": 832,
                ""y"": 1702
              },
              {
                ""x"": 832,
                ""y"": 1696
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""(1-Attack"",
            ""boundingPolygon"": [
              {
                ""x"": 846,
                ""y"": 1704
              },
              {
                ""x"": 846,
                ""y"": 1748
              },
              {
                ""x"": 833,
                ""y"": 1748
              },
              {
                ""x"": 832,
                ""y"": 1705
              }
            ],
            ""confidence"": 0.306
          }
        ]
      },
      {
        ""text"": ""Equip 1 (De Anach to target creative you"",
        ""boundingPolygon"": [
          {
            ""x"": 799,
            ""y"": 701
          },
          {
            ""x"": 796,
            ""y"": 845
          },
          {
            ""x"": 780,
            ""y"": 845
          },
          {
            ""x"": 782,
            ""y"": 701
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 800,
                ""y"": 702
              },
              {
                ""x"": 798,
                ""y"": 724
              },
              {
                ""x"": 783,
                ""y"": 723
              },
              {
                ""x"": 785,
                ""y"": 701
              }
            ],
            ""confidence"": 0.834
          },
          {
            ""text"": ""1"",
            ""boundingPolygon"": [
              {
                ""x"": 797,
                ""y"": 726
              },
              {
                ""x"": 797,
                ""y"": 732
              },
              {
                ""x"": 782,
                ""y"": 731
              },
              {
                ""x"": 783,
                ""y"": 725
              }
            ],
            ""confidence"": 0.127
          },
          {
            ""text"": ""(De"",
            ""boundingPolygon"": [
              {
                ""x"": 797,
                ""y"": 734
              },
              {
                ""x"": 796,
                ""y"": 746
              },
              {
                ""x"": 781,
                ""y"": 745
              },
              {
                ""x"": 782,
                ""y"": 733
              }
            ],
            ""confidence"": 0.122
          },
          {
            ""text"": ""Anach"",
            ""boundingPolygon"": [
              {
                ""x"": 796,
                ""y"": 749
              },
              {
                ""x"": 795,
                ""y"": 769
              },
              {
                ""x"": 781,
                ""y"": 768
              },
              {
                ""x"": 781,
                ""y"": 748
              }
            ],
            ""confidence"": 0.6
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 794,
                ""y"": 772
              },
              {
                ""x"": 794,
                ""y"": 777
              },
              {
                ""x"": 781,
                ""y"": 776
              },
              {
                ""x"": 781,
                ""y"": 771
              }
            ],
            ""confidence"": 0.596
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 794,
                ""y"": 780
              },
              {
                ""x"": 794,
                ""y"": 799
              },
              {
                ""x"": 781,
                ""y"": 798
              },
              {
                ""x"": 781,
                ""y"": 779
              }
            ],
            ""confidence"": 0.549
          },
          {
            ""text"": ""creative"",
            ""boundingPolygon"": [
              {
                ""x"": 794,
                ""y"": 801
              },
              {
                ""x"": 794,
                ""y"": 826
              },
              {
                ""x"": 782,
                ""y"": 825
              },
              {
                ""x"": 781,
                ""y"": 800
              }
            ],
            ""confidence"": 0.306
          },
          {
            ""text"": ""you"",
            ""boundingPolygon"": [
              {
                ""x"": 794,
                ""y"": 828
              },
              {
                ""x"": 795,
                ""y"": 843
              },
              {
                ""x"": 783,
                ""y"": 842
              },
              {
                ""x"": 782,
                ""y"": 827
              }
            ],
            ""confidence"": 0.187
          }
        ]
      },
      {
        ""text"": ""a: Attach Sparring Collar to"",
        ""boundingPolygon"": [
          {
            ""x"": 813,
            ""y"": 988
          },
          {
            ""x"": 824,
            ""y"": 1103
          },
          {
            ""x"": 810,
            ""y"": 1104
          },
          {
            ""x"": 802,
            ""y"": 989
          }
        ],
        ""words"": [
          {
            ""text"": ""a:"",
            ""boundingPolygon"": [
              {
                ""x"": 814,
                ""y"": 988
              },
              {
                ""x"": 814,
                ""y"": 996
              },
              {
                ""x"": 802,
                ""y"": 998
              },
              {
                ""x"": 802,
                ""y"": 990
              }
            ],
            ""confidence"": 0.201
          },
          {
            ""text"": ""Attach"",
            ""boundingPolygon"": [
              {
                ""x"": 814,
                ""y"": 998
              },
              {
                ""x"": 815,
                ""y"": 1026
              },
              {
                ""x"": 803,
                ""y"": 1028
              },
              {
                ""x"": 802,
                ""y"": 1000
              }
            ],
            ""confidence"": 0.84
          },
          {
            ""text"": ""Sparring"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 1029
              },
              {
                ""x"": 819,
                ""y"": 1065
              },
              {
                ""x"": 807,
                ""y"": 1066
              },
              {
                ""x"": 803,
                ""y"": 1031
              }
            ],
            ""confidence"": 0.938
          },
          {
            ""text"": ""Collar"",
            ""boundingPolygon"": [
              {
                ""x"": 819,
                ""y"": 1067
              },
              {
                ""x"": 822,
                ""y"": 1093
              },
              {
                ""x"": 811,
                ""y"": 1094
              },
              {
                ""x"": 807,
                ""y"": 1068
              }
            ],
            ""confidence"": 0.905
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 822,
                ""y"": 1095
              },
              {
                ""x"": 823,
                ""y"": 1103
              },
              {
                ""x"": 813,
                ""y"": 1104
              },
              {
                ""x"": 812,
                ""y"": 1096
              }
            ],
            ""confidence"": 0.953
          }
        ]
      },
      {
        ""text"": ""henever an Artificer enters the"",
        ""boundingPolygon"": [
          {
            ""x"": 828,
            ""y"": 1215
          },
          {
            ""x"": 832,
            ""y"": 1353
          },
          {
            ""x"": 815,
            ""y"": 1354
          },
          {
            ""x"": 811,
            ""y"": 1215
          }
        ],
        ""words"": [
          {
            ""text"": ""henever"",
            ""boundingPolygon"": [
              {
                ""x"": 826,
                ""y"": 1215
              },
              {
                ""x"": 829,
                ""y"": 1247
              },
              {
                ""x"": 815,
                ""y"": 1248
              },
              {
                ""x"": 812,
                ""y"": 1215
              }
            ],
            ""confidence"": 0.388
          },
          {
            ""text"": ""an"",
            ""boundingPolygon"": [
              {
                ""x"": 829,
                ""y"": 1250
              },
              {
                ""x"": 830,
                ""y"": 1259
              },
              {
                ""x"": 816,
                ""y"": 1260
              },
              {
                ""x"": 815,
                ""y"": 1250
              }
            ],
            ""confidence"": 0.995
          },
          {
            ""text"": ""Artificer"",
            ""boundingPolygon"": [
              {
                ""x"": 830,
                ""y"": 1262
              },
              {
                ""x"": 831,
                ""y"": 1297
              },
              {
                ""x"": 817,
                ""y"": 1297
              },
              {
                ""x"": 816,
                ""y"": 1263
              }
            ],
            ""confidence"": 0.951
          },
          {
            ""text"": ""enters"",
            ""boundingPolygon"": [
              {
                ""x"": 831,
                ""y"": 1299
              },
              {
                ""x"": 831,
                ""y"": 1323
              },
              {
                ""x"": 817,
                ""y"": 1324
              },
              {
                ""x"": 817,
                ""y"": 1300
              }
            ],
            ""confidence"": 0.698
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 831,
                ""y"": 1326
              },
              {
                ""x"": 831,
                ""y"": 1344
              },
              {
                ""x"": 816,
                ""y"": 1344
              },
              {
                ""x"": 817,
                ""y"": 1327
              }
            ],
            ""confidence"": 0.121
          }
        ]
      },
      {
        ""text"": ""battlefield Twventor's Goggles to it"",
        ""boundingPolygon"": [
          {
            ""x"": 816,
            ""y"": 1207
          },
          {
            ""x"": 816,
            ""y"": 1355
          },
          {
            ""x"": 801,
            ""y"": 1355
          },
          {
            ""x"": 801,
            ""y"": 1207
          }
        ],
        ""words"": [
          {
            ""text"": ""battlefield"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 1207
              },
              {
                ""x"": 816,
                ""y"": 1249
              },
              {
                ""x"": 801,
                ""y"": 1250
              },
              {
                ""x"": 801,
                ""y"": 1207
              }
            ],
            ""confidence"": 0.203
          },
          {
            ""text"": ""Twventor's"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 1255
              },
              {
                ""x"": 816,
                ""y"": 1295
              },
              {
                ""x"": 802,
                ""y"": 1296
              },
              {
                ""x"": 801,
                ""y"": 1255
              }
            ],
            ""confidence"": 0.425
          },
          {
            ""text"": ""Goggles"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 1298
              },
              {
                ""x"": 816,
                ""y"": 1330
              },
              {
                ""x"": 802,
                ""y"": 1331
              },
              {
                ""x"": 802,
                ""y"": 1299
              }
            ],
            ""confidence"": 0.504
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 1333
              },
              {
                ""x"": 816,
                ""y"": 1341
              },
              {
                ""x"": 803,
                ""y"": 1342
              },
              {
                ""x"": 802,
                ""y"": 1334
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""it"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 1344
              },
              {
                ""x"": 815,
                ""y"": 1354
              },
              {
                ""x"": 803,
                ""y"": 1355
              },
              {
                ""x"": 803,
                ""y"": 1345
              }
            ],
            ""confidence"": 0.808
          }
        ]
      },
      {
        ""text"": ""\""I've broken three su ureids, but this"",
        ""boundingPolygon"": [
          {
            ""x"": 822,
            ""y"": 1434
          },
          {
            ""x"": 824,
            ""y"": 1581
          },
          {
            ""x"": 808,
            ""y"": 1582
          },
          {
            ""x"": 807,
            ""y"": 1435
          }
        ],
        ""words"": [
          {
            ""text"": ""\""I've"",
            ""boundingPolygon"": [
              {
                ""x"": 820,
                ""y"": 1439
              },
              {
                ""x"": 821,
                ""y"": 1459
              },
              {
                ""x"": 808,
                ""y"": 1460
              },
              {
                ""x"": 807,
                ""y"": 1441
              }
            ],
            ""confidence"": 0.277
          },
          {
            ""text"": ""broken"",
            ""boundingPolygon"": [
              {
                ""x"": 821,
                ""y"": 1461
              },
              {
                ""x"": 822,
                ""y"": 1487
              },
              {
                ""x"": 809,
                ""y"": 1488
              },
              {
                ""x"": 808,
                ""y"": 1463
              }
            ],
            ""confidence"": 0.704
          },
          {
            ""text"": ""three"",
            ""boundingPolygon"": [
              {
                ""x"": 822,
                ""y"": 1489
              },
              {
                ""x"": 823,
                ""y"": 1508
              },
              {
                ""x"": 810,
                ""y"": 1509
              },
              {
                ""x"": 809,
                ""y"": 1491
              }
            ],
            ""confidence"": 0.527
          },
          {
            ""text"": ""su"",
            ""boundingPolygon"": [
              {
                ""x"": 823,
                ""y"": 1510
              },
              {
                ""x"": 823,
                ""y"": 1517
              },
              {
                ""x"": 810,
                ""y"": 1518
              },
              {
                ""x"": 810,
                ""y"": 1512
              }
            ],
            ""confidence"": 0.153
          },
          {
            ""text"": ""ureids,"",
            ""boundingPolygon"": [
              {
                ""x"": 823,
                ""y"": 1519
              },
              {
                ""x"": 823,
                ""y"": 1546
              },
              {
                ""x"": 810,
                ""y"": 1547
              },
              {
                ""x"": 810,
                ""y"": 1521
              }
            ],
            ""confidence"": 0.172
          },
          {
            ""text"": ""but"",
            ""boundingPolygon"": [
              {
                ""x"": 823,
                ""y"": 1548
              },
              {
                ""x"": 823,
                ""y"": 1561
              },
              {
                ""x"": 809,
                ""y"": 1562
              },
              {
                ""x"": 810,
                ""y"": 1549
              }
            ],
            ""confidence"": 0.632
          },
          {
            ""text"": ""this"",
            ""boundingPolygon"": [
              {
                ""x"": 823,
                ""y"": 1563
              },
              {
                ""x"": 823,
                ""y"": 1581
              },
              {
                ""x"": 809,
                ""y"": 1582
              },
              {
                ""x"": 809,
                ""y"": 1564
              }
            ],
            ""confidence"": 0.709
          }
        ]
      },
      {
        ""text"": ""control. Equip only as a sorce"",
        ""boundingPolygon"": [
          {
            ""x"": 837,
            ""y"": 1680
          },
          {
            ""x"": 842,
            ""y"": 1793
          },
          {
            ""x"": 828,
            ""y"": 1793
          },
          {
            ""x"": 824,
            ""y"": 1680
          }
        ],
        ""words"": [
          {
            ""text"": ""control."",
            ""boundingPolygon"": [
              {
                ""x"": 838,
                ""y"": 1683
              },
              {
                ""x"": 838,
                ""y"": 1710
              },
              {
                ""x"": 825,
                ""y"": 1711
              },
              {
                ""x"": 824,
                ""y"": 1684
              }
            ],
            ""confidence"": 0.803
          },
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 838,
                ""y"": 1712
              },
              {
                ""x"": 839,
                ""y"": 1734
              },
              {
                ""x"": 826,
                ""y"": 1735
              },
              {
                ""x"": 825,
                ""y"": 1713
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""only"",
            ""boundingPolygon"": [
              {
                ""x"": 839,
                ""y"": 1737
              },
              {
                ""x"": 840,
                ""y"": 1752
              },
              {
                ""x"": 827,
                ""y"": 1754
              },
              {
                ""x"": 826,
                ""y"": 1738
              }
            ],
            ""confidence"": 0.958
          },
          {
            ""text"": ""as"",
            ""boundingPolygon"": [
              {
                ""x"": 840,
                ""y"": 1755
              },
              {
                ""x"": 840,
                ""y"": 1763
              },
              {
                ""x"": 827,
                ""y"": 1764
              },
              {
                ""x"": 827,
                ""y"": 1756
              }
            ],
            ""confidence"": 0.989
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 841,
                ""y"": 1765
              },
              {
                ""x"": 841,
                ""y"": 1770
              },
              {
                ""x"": 828,
                ""y"": 1771
              },
              {
                ""x"": 827,
                ""y"": 1767
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""sorce"",
            ""boundingPolygon"": [
              {
                ""x"": 841,
                ""y"": 1772
              },
              {
                ""x"": 843,
                ""y"": 1791
              },
              {
                ""x"": 829,
                ""y"": 1793
              },
              {
                ""x"": 828,
                ""y"": 1774
              }
            ],
            ""confidence"": 0.723
          }
        ]
      },
      {
        ""text"": ""quip - Eib only as a sorcery. )"",
        ""boundingPolygon"": [
          {
            ""x"": 793,
            ""y"": 985
          },
          {
            ""x"": 801,
            ""y"": 1125
          },
          {
            ""x"": 783,
            ""y"": 1126
          },
          {
            ""x"": 778,
            ""y"": 986
          }
        ],
        ""words"": [
          {
            ""text"": ""quip"",
            ""boundingPolygon"": [
              {
                ""x"": 794,
                ""y"": 986
              },
              {
                ""x"": 793,
                ""y"": 1006
              },
              {
                ""x"": 778,
                ""y"": 1007
              },
              {
                ""x"": 779,
                ""y"": 987
              }
            ],
            ""confidence"": 0.847
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 792,
                ""y"": 1009
              },
              {
                ""x"": 792,
                ""y"": 1015
              },
              {
                ""x"": 778,
                ""y"": 1016
              },
              {
                ""x"": 778,
                ""y"": 1010
              }
            ],
            ""confidence"": 0.121
          },
          {
            ""text"": ""Eib"",
            ""boundingPolygon"": [
              {
                ""x"": 792,
                ""y"": 1028
              },
              {
                ""x"": 793,
                ""y"": 1049
              },
              {
                ""x"": 779,
                ""y"": 1050
              },
              {
                ""x"": 778,
                ""y"": 1029
              }
            ],
            ""confidence"": 0.119
          },
          {
            ""text"": ""only"",
            ""boundingPolygon"": [
              {
                ""x"": 793,
                ""y"": 1052
              },
              {
                ""x"": 794,
                ""y"": 1068
              },
              {
                ""x"": 781,
                ""y"": 1069
              },
              {
                ""x"": 779,
                ""y"": 1053
              }
            ],
            ""confidence"": 0.118
          },
          {
            ""text"": ""as"",
            ""boundingPolygon"": [
              {
                ""x"": 794,
                ""y"": 1071
              },
              {
                ""x"": 794,
                ""y"": 1079
              },
              {
                ""x"": 782,
                ""y"": 1080
              },
              {
                ""x"": 781,
                ""y"": 1072
              }
            ],
            ""confidence"": 0.937
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 795,
                ""y"": 1082
              },
              {
                ""x"": 795,
                ""y"": 1087
              },
              {
                ""x"": 782,
                ""y"": 1088
              },
              {
                ""x"": 782,
                ""y"": 1082
              }
            ],
            ""confidence"": 0.916
          },
          {
            ""text"": ""sorcery."",
            ""boundingPolygon"": [
              {
                ""x"": 795,
                ""y"": 1089
              },
              {
                ""x"": 799,
                ""y"": 1116
              },
              {
                ""x"": 786,
                ""y"": 1117
              },
              {
                ""x"": 783,
                ""y"": 1090
              }
            ],
            ""confidence"": 0.565
          },
          {
            ""text"": "")"",
            ""boundingPolygon"": [
              {
                ""x"": 799,
                ""y"": 1118
              },
              {
                ""x"": 800,
                ""y"": 1124
              },
              {
                ""x"": 788,
                ""y"": 1125
              },
              {
                ""x"": 787,
                ""y"": 1119
              }
            ],
            ""confidence"": 0.171
          }
        ]
      },
      {
        ""text"": ""may atta"",
        ""boundingPolygon"": [
          {
            ""x"": 806,
            ""y"": 1207
          },
          {
            ""x"": 809,
            ""y"": 1244
          },
          {
            ""x"": 797,
            ""y"": 1246
          },
          {
            ""x"": 793,
            ""y"": 1208
          }
        ],
        ""words"": [
          {
            ""text"": ""may"",
            ""boundingPolygon"": [
              {
                ""x"": 806,
                ""y"": 1208
              },
              {
                ""x"": 808,
                ""y"": 1225
              },
              {
                ""x"": 795,
                ""y"": 1226
              },
              {
                ""x"": 793,
                ""y"": 1209
              }
            ],
            ""confidence"": 0.99
          },
          {
            ""text"": ""atta"",
            ""boundingPolygon"": [
              {
                ""x"": 808,
                ""y"": 1227
              },
              {
                ""x"": 809,
                ""y"": 1244
              },
              {
                ""x"": 797,
                ""y"": 1245
              },
              {
                ""x"": 795,
                ""y"": 1229
              }
            ],
            ""confidence"": 0.936
          }
        ]
      },
      {
        ""text"": ""Tances, and countless dein barile,"",
        ""boundingPolygon"": [
          {
            ""x"": 813,
            ""y"": 1442
          },
          {
            ""x"": 816,
            ""y"": 1583
          },
          {
            ""x"": 802,
            ""y"": 1583
          },
          {
            ""x"": 799,
            ""y"": 1442
          }
        ],
        ""words"": [
          {
            ""text"": ""Tances,"",
            ""boundingPolygon"": [
              {
                ""x"": 813,
                ""y"": 1442
              },
              {
                ""x"": 814,
                ""y"": 1465
              },
              {
                ""x"": 801,
                ""y"": 1466
              },
              {
                ""x"": 799,
                ""y"": 1443
              }
            ],
            ""confidence"": 0.63
          },
          {
            ""text"": ""and"",
            ""boundingPolygon"": [
              {
                ""x"": 814,
                ""y"": 1468
              },
              {
                ""x"": 815,
                ""y"": 1483
              },
              {
                ""x"": 803,
                ""y"": 1484
              },
              {
                ""x"": 801,
                ""y"": 1469
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""countless"",
            ""boundingPolygon"": [
              {
                ""x"": 815,
                ""y"": 1485
              },
              {
                ""x"": 816,
                ""y"": 1518
              },
              {
                ""x"": 804,
                ""y"": 1519
              },
              {
                ""x"": 803,
                ""y"": 1486
              }
            ],
            ""confidence"": 0.706
          },
          {
            ""text"": ""dein"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 1520
              },
              {
                ""x"": 816,
                ""y"": 1544
              },
              {
                ""x"": 805,
                ""y"": 1545
              },
              {
                ""x"": 804,
                ""y"": 1521
              }
            ],
            ""confidence"": 0.118
          },
          {
            ""text"": ""barile,"",
            ""boundingPolygon"": [
              {
                ""x"": 816,
                ""y"": 1558
              },
              {
                ""x"": 816,
                ""y"": 1582
              },
              {
                ""x"": 804,
                ""y"": 1583
              },
              {
                ""x"": 804,
                ""y"": 1559
              }
            ],
            ""confidence"": 0.406
          }
        ]
      },
      {
        ""text"": ""our control. Equip on"",
        ""boundingPolygon"": [
          {
            ""x"": 784,
            ""y"": 985
          },
          {
            ""x"": 787,
            ""y"": 1060
          },
          {
            ""x"": 776,
            ""y"": 1061
          },
          {
            ""x"": 773,
            ""y"": 986
          }
        ],
        ""words"": [
          {
            ""text"": ""our"",
            ""boundingPolygon"": [
              {
                ""x"": 785,
                ""y"": 986
              },
              {
                ""x"": 785,
                ""y"": 995
              },
              {
                ""x"": 774,
                ""y"": 996
              },
              {
                ""x"": 774,
                ""y"": 987
              }
            ],
            ""confidence"": 0.184
          },
          {
            ""text"": ""control."",
            ""boundingPolygon"": [
              {
                ""x"": 785,
                ""y"": 997
              },
              {
                ""x"": 786,
                ""y"": 1025
              },
              {
                ""x"": 775,
                ""y"": 1026
              },
              {
                ""x"": 774,
                ""y"": 998
              }
            ],
            ""confidence"": 0.615
          },
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 786,
                ""y"": 1027
              },
              {
                ""x"": 787,
                ""y"": 1051
              },
              {
                ""x"": 776,
                ""y"": 1052
              },
              {
                ""x"": 775,
                ""y"": 1028
              }
            ],
            ""confidence"": 0.94
          },
          {
            ""text"": ""on"",
            ""boundingPolygon"": [
              {
                ""x"": 787,
                ""y"": 1053
              },
              {
                ""x"": 788,
                ""y"": 1060
              },
              {
                ""x"": 777,
                ""y"": 1061
              },
              {
                ""x"": 777,
                ""y"": 1054
              }
            ],
            ""confidence"": 0.508
          }
        ]
      },
      {
        ""text"": ""Equip 2"",
        ""boundingPolygon"": [
          {
            ""x"": 797,
            ""y"": 1211
          },
          {
            ""x"": 799,
            ""y"": 1249
          },
          {
            ""x"": 786,
            ""y"": 1250
          },
          {
            ""x"": 784,
            ""y"": 1212
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 797,
                ""y"": 1211
              },
              {
                ""x"": 798,
                ""y"": 1234
              },
              {
                ""x"": 785,
                ""y"": 1235
              },
              {
                ""x"": 784,
                ""y"": 1211
              }
            ],
            ""confidence"": 0.983
          },
          {
            ""text"": ""2"",
            ""boundingPolygon"": [
              {
                ""x"": 798,
                ""y"": 1238
              },
              {
                ""x"": 799,
                ""y"": 1245
              },
              {
                ""x"": 786,
                ""y"": 1245
              },
              {
                ""x"": 785,
                ""y"": 1239
              }
            ],
            ""confidence"": 0.989
          }
        ]
      },
      {
        ""text"": ""to target creature"",
        ""boundingPolygon"": [
          {
            ""x"": 805,
            ""y"": 1289
          },
          {
            ""x"": 808,
            ""y"": 1356
          },
          {
            ""x"": 796,
            ""y"": 1356
          },
          {
            ""x"": 793,
            ""y"": 1289
          }
        ],
        ""words"": [
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 805,
                ""y"": 1291
              },
              {
                ""x"": 805,
                ""y"": 1298
              },
              {
                ""x"": 794,
                ""y"": 1298
              },
              {
                ""x"": 793,
                ""y"": 1291
              }
            ],
            ""confidence"": 0.777
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 806,
                ""y"": 1301
              },
              {
                ""x"": 807,
                ""y"": 1322
              },
              {
                ""x"": 795,
                ""y"": 1323
              },
              {
                ""x"": 794,
                ""y"": 1301
              }
            ],
            ""confidence"": 0.643
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 807,
                ""y"": 1325
              },
              {
                ""x"": 809,
                ""y"": 1355
              },
              {
                ""x"": 796,
                ""y"": 1356
              },
              {
                ""x"": 795,
                ""y"": 1325
              }
            ],
            ""confidence"": 0.987
          }
        ]
      },
      {
        ""text"": ""Sometimes the only difference between"",
        ""boundingPolygon"": [
          {
            ""x"": 821,
            ""y"": 1674
          },
          {
            ""x"": 827,
            ""y"": 1812
          },
          {
            ""x"": 811,
            ""y"": 1813
          },
          {
            ""x"": 806,
            ""y"": 1675
          }
        ],
        ""words"": [
          {
            ""text"": ""Sometimes"",
            ""boundingPolygon"": [
              {
                ""x"": 821,
                ""y"": 1675
              },
              {
                ""x"": 822,
                ""y"": 1710
              },
              {
                ""x"": 807,
                ""y"": 1711
              },
              {
                ""x"": 807,
                ""y"": 1676
              }
            ],
            ""confidence"": 0.524
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 822,
                ""y"": 1713
              },
              {
                ""x"": 822,
                ""y"": 1724
              },
              {
                ""x"": 808,
                ""y"": 1726
              },
              {
                ""x"": 807,
                ""y"": 1714
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""only"",
            ""boundingPolygon"": [
              {
                ""x"": 822,
                ""y"": 1727
              },
              {
                ""x"": 823,
                ""y"": 1743
              },
              {
                ""x"": 808,
                ""y"": 1745
              },
              {
                ""x"": 808,
                ""y"": 1728
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""difference"",
            ""boundingPolygon"": [
              {
                ""x"": 823,
                ""y"": 1746
              },
              {
                ""x"": 825,
                ""y"": 1779
              },
              {
                ""x"": 810,
                ""y"": 1781
              },
              {
                ""x"": 809,
                ""y"": 1748
              }
            ],
            ""confidence"": 0.979
          },
          {
            ""text"": ""between"",
            ""boundingPolygon"": [
              {
                ""x"": 825,
                ""y"": 1782
              },
              {
                ""x"": 827,
                ""y"": 1810
              },
              {
                ""x"": 813,
                ""y"": 1813
              },
              {
                ""x"": 811,
                ""y"": 1784
              }
            ],
            ""confidence"": 0.591
          }
        ]
      },
      {
        ""text"": ""the control. Equip only as a sorcery"",
        ""boundingPolygon"": [
          {
            ""x"": 791,
            ""y"": 1217
          },
          {
            ""x"": 795,
            ""y"": 1343
          },
          {
            ""x"": 781,
            ""y"": 1343
          },
          {
            ""x"": 778,
            ""y"": 1217
          }
        ],
        ""words"": [
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 791,
                ""y"": 1218
              },
              {
                ""x"": 791,
                ""y"": 1222
              },
              {
                ""x"": 778,
                ""y"": 1222
              },
              {
                ""x"": 778,
                ""y"": 1218
              }
            ],
            ""confidence"": 0.042
          },
          {
            ""text"": ""control."",
            ""boundingPolygon"": [
              {
                ""x"": 791,
                ""y"": 1225
              },
              {
                ""x"": 791,
                ""y"": 1254
              },
              {
                ""x"": 779,
                ""y"": 1254
              },
              {
                ""x"": 778,
                ""y"": 1225
              }
            ],
            ""confidence"": 0.84
          },
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 791,
                ""y"": 1256
              },
              {
                ""x"": 792,
                ""y"": 1278
              },
              {
                ""x"": 780,
                ""y"": 1278
              },
              {
                ""x"": 779,
                ""y"": 1256
              }
            ],
            ""confidence"": 0.924
          },
          {
            ""text"": ""only"",
            ""boundingPolygon"": [
              {
                ""x"": 792,
                ""y"": 1280
              },
              {
                ""x"": 792,
                ""y"": 1297
              },
              {
                ""x"": 781,
                ""y"": 1297
              },
              {
                ""x"": 780,
                ""y"": 1280
              }
            ],
            ""confidence"": 0.951
          },
          {
            ""text"": ""as"",
            ""boundingPolygon"": [
              {
                ""x"": 792,
                ""y"": 1299
              },
              {
                ""x"": 793,
                ""y"": 1308
              },
              {
                ""x"": 781,
                ""y"": 1308
              },
              {
                ""x"": 781,
                ""y"": 1299
              }
            ],
            ""confidence"": 0.951
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 793,
                ""y"": 1311
              },
              {
                ""x"": 793,
                ""y"": 1315
              },
              {
                ""x"": 781,
                ""y"": 1315
              },
              {
                ""x"": 781,
                ""y"": 1311
              }
            ],
            ""confidence"": 0.992
          },
          {
            ""text"": ""sorcery"",
            ""boundingPolygon"": [
              {
                ""x"": 793,
                ""y"": 1317
              },
              {
                ""x"": 794,
                ""y"": 1342
              },
              {
                ""x"": 782,
                ""y"": 1342
              },
              {
                ""x"": 781,
                ""y"": 1317
              }
            ],
            ""confidence"": 0.663
          }
        ]
      },
      {
        ""text"": ""le Blade has surtived er"",
        ""boundingPolygon"": [
          {
            ""x"": 803,
            ""y"": 1452
          },
          {
            ""x"": 811,
            ""y"": 1541
          },
          {
            ""x"": 800,
            ""y"": 1542
          },
          {
            ""x"": 791,
            ""y"": 1453
          }
        ],
        ""words"": [
          {
            ""text"": ""le"",
            ""boundingPolygon"": [
              {
                ""x"": 803,
                ""y"": 1453
              },
              {
                ""x"": 804,
                ""y"": 1458
              },
              {
                ""x"": 792,
                ""y"": 1460
              },
              {
                ""x"": 792,
                ""y"": 1454
              }
            ],
            ""confidence"": 0.579
          },
          {
            ""text"": ""Blade"",
            ""boundingPolygon"": [
              {
                ""x"": 804,
                ""y"": 1460
              },
              {
                ""x"": 805,
                ""y"": 1481
              },
              {
                ""x"": 794,
                ""y"": 1483
              },
              {
                ""x"": 792,
                ""y"": 1462
              }
            ],
            ""confidence"": 0.763
          },
          {
            ""text"": ""has"",
            ""boundingPolygon"": [
              {
                ""x"": 806,
                ""y"": 1483
              },
              {
                ""x"": 807,
                ""y"": 1497
              },
              {
                ""x"": 796,
                ""y"": 1499
              },
              {
                ""x"": 795,
                ""y"": 1485
              }
            ],
            ""confidence"": 0.969
          },
          {
            ""text"": ""surtived"",
            ""boundingPolygon"": [
              {
                ""x"": 807,
                ""y"": 1499
              },
              {
                ""x"": 810,
                ""y"": 1532
              },
              {
                ""x"": 800,
                ""y"": 1534
              },
              {
                ""x"": 796,
                ""y"": 1501
              }
            ],
            ""confidence"": 0.578
          },
          {
            ""text"": ""er"",
            ""boundingPolygon"": [
              {
                ""x"": 810,
                ""y"": 1535
              },
              {
                ""x"": 811,
                ""y"": 1541
              },
              {
                ""x"": 801,
                ""y"": 1543
              },
              {
                ""x"": 801,
                ""y"": 1537
              }
            ],
            ""confidence"": 0.569
          }
        ]
      },
      {
        ""text"": ""martyr and a hero is a sword.\"""",
        ""boundingPolygon"": [
          {
            ""x"": 814,
            ""y"": 1675
          },
          {
            ""x"": 820,
            ""y"": 1809
          },
          {
            ""x"": 804,
            ""y"": 1810
          },
          {
            ""x"": 799,
            ""y"": 1676
          }
        ],
        ""words"": [
          {
            ""text"": ""martyr"",
            ""boundingPolygon"": [
              {
                ""x"": 814,
                ""y"": 1676
              },
              {
                ""x"": 814,
                ""y"": 1701
              },
              {
                ""x"": 800,
                ""y"": 1703
              },
              {
                ""x"": 800,
                ""y"": 1677
              }
            ],
            ""confidence"": 0.873
          },
          {
            ""text"": ""and"",
            ""boundingPolygon"": [
              {
                ""x"": 814,
                ""y"": 1704
              },
              {
                ""x"": 814,
                ""y"": 1719
              },
              {
                ""x"": 800,
                ""y"": 1720
              },
              {
                ""x"": 800,
                ""y"": 1705
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 814,
                ""y"": 1721
              },
              {
                ""x"": 814,
                ""y"": 1726
              },
              {
                ""x"": 801,
                ""y"": 1727
              },
              {
                ""x"": 801,
                ""y"": 1723
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""hero"",
            ""boundingPolygon"": [
              {
                ""x"": 814,
                ""y"": 1728
              },
              {
                ""x"": 814,
                ""y"": 1745
              },
              {
                ""x"": 802,
                ""y"": 1746
              },
              {
                ""x"": 801,
                ""y"": 1730
              }
            ],
            ""confidence"": 0.38
          },
          {
            ""text"": ""is"",
            ""boundingPolygon"": [
              {
                ""x"": 815,
                ""y"": 1747
              },
              {
                ""x"": 815,
                ""y"": 1752
              },
              {
                ""x"": 802,
                ""y"": 1754
              },
              {
                ""x"": 802,
                ""y"": 1749
              }
            ],
            ""confidence"": 0.995
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 815,
                ""y"": 1755
              },
              {
                ""x"": 815,
                ""y"": 1760
              },
              {
                ""x"": 803,
                ""y"": 1762
              },
              {
                ""x"": 802,
                ""y"": 1757
              }
            ],
            ""confidence"": 0.871
          },
          {
            ""text"": ""sword.\"""",
            ""boundingPolygon"": [
              {
                ""x"": 815,
                ""y"": 1763
              },
              {
                ""x"": 818,
                ""y"": 1793
              },
              {
                ""x"": 806,
                ""y"": 1796
              },
              {
                ""x"": 803,
                ""y"": 1765
              }
            ],
            ""confidence"": 0.655
          }
        ]
      },
      {
        ""text"": ""Captain Su"",
        ""boundingPolygon"": [
          {
            ""x"": 799,
            ""y"": 1676
          },
          {
            ""x"": 802,
            ""y"": 1725
          },
          {
            ""x"": 792,
            ""y"": 1726
          },
          {
            ""x"": 790,
            ""y"": 1677
          }
        ],
        ""words"": [
          {
            ""text"": ""Captain"",
            ""boundingPolygon"": [
              {
                ""x"": 800,
                ""y"": 1679
              },
              {
                ""x"": 801,
                ""y"": 1710
              },
              {
                ""x"": 792,
                ""y"": 1711
              },
              {
                ""x"": 790,
                ""y"": 1680
              }
            ],
            ""confidence"": 0.904
          },
          {
            ""text"": ""Su"",
            ""boundingPolygon"": [
              {
                ""x"": 802,
                ""y"": 1714
              },
              {
                ""x"": 802,
                ""y"": 1723
              },
              {
                ""x"": 793,
                ""y"": 1724
              },
              {
                ""x"": 792,
                ""y"": 1715
              }
            ],
            ""confidence"": 0.804
          }
        ]
      },
      {
        ""text"": ""Honed Khopesh"",
        ""boundingPolygon"": [
          {
            ""x"": 699,
            ""y"": 137
          },
          {
            ""x"": 701,
            ""y"": 220
          },
          {
            ""x"": 689,
            ""y"": 221
          },
          {
            ""x"": 688,
            ""y"": 137
          }
        ],
        ""words"": [
          {
            ""text"": ""Honed"",
            ""boundingPolygon"": [
              {
                ""x"": 700,
                ""y"": 143
              },
              {
                ""x"": 700,
                ""y"": 172
              },
              {
                ""x"": 690,
                ""y"": 172
              },
              {
                ""x"": 689,
                ""y"": 142
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""Khopesh"",
            ""boundingPolygon"": [
              {
                ""x"": 700,
                ""y"": 176
              },
              {
                ""x"": 700,
                ""y"": 215
              },
              {
                ""x"": 691,
                ""y"": 215
              },
              {
                ""x"": 690,
                ""y"": 176
              }
            ],
            ""confidence"": 0.991
          }
        ]
      },
      {
        ""text"": ""Pirate"",
        ""boundingPolygon"": [
          {
            ""x"": 691,
            ""y"": 426
          },
          {
            ""x"": 691,
            ""y"": 456
          },
          {
            ""x"": 677,
            ""y"": 457
          },
          {
            ""x"": 678,
            ""y"": 425
          }
        ],
        ""words"": [
          {
            ""text"": ""Pirate"",
            ""boundingPolygon"": [
              {
                ""x"": 691,
                ""y"": 427
              },
              {
                ""x"": 691,
                ""y"": 456
              },
              {
                ""x"": 677,
                ""y"": 456
              },
              {
                ""x"": 677,
                ""y"": 427
              }
            ],
            ""confidence"": 0.991
          }
        ]
      },
      {
        ""text"": ""Copper Carapace"",
        ""boundingPolygon"": [
          {
            ""x"": 677,
            ""y"": 697
          },
          {
            ""x"": 682,
            ""y"": 791
          },
          {
            ""x"": 670,
            ""y"": 792
          },
          {
            ""x"": 667,
            ""y"": 697
          }
        ],
        ""words"": [
          {
            ""text"": ""Copper"",
            ""boundingPolygon"": [
              {
                ""x"": 678,
                ""y"": 698
              },
              {
                ""x"": 678,
                ""y"": 728
              },
              {
                ""x"": 667,
                ""y"": 729
              },
              {
                ""x"": 667,
                ""y"": 698
              }
            ],
            ""confidence"": 0.987
          },
          {
            ""text"": ""Carapace"",
            ""boundingPolygon"": [
              {
                ""x"": 678,
                ""y"": 731
              },
              {
                ""x"": 680,
                ""y"": 772
              },
              {
                ""x"": 670,
                ""y"": 773
              },
              {
                ""x"": 667,
                ""y"": 731
              }
            ],
            ""confidence"": 0.993
          }
        ]
      },
      {
        ""text"": ""Treva's Attendant"",
        ""boundingPolygon"": [
          {
            ""x"": 704,
            ""y"": 1599
          },
          {
            ""x"": 715,
            ""y"": 1692
          },
          {
            ""x"": 703,
            ""y"": 1693
          },
          {
            ""x"": 692,
            ""y"": 1601
          }
        ],
        ""words"": [
          {
            ""text"": ""Treva's"",
            ""boundingPolygon"": [
              {
                ""x"": 704,
                ""y"": 1602
              },
              {
                ""x"": 708,
                ""y"": 1631
              },
              {
                ""x"": 696,
                ""y"": 1634
              },
              {
                ""x"": 693,
                ""y"": 1604
              }
            ],
            ""confidence"": 0.924
          },
          {
            ""text"": ""Attendant"",
            ""boundingPolygon"": [
              {
                ""x"": 708,
                ""y"": 1633
              },
              {
                ""x"": 714,
                ""y"": 1679
              },
              {
                ""x"": 702,
                ""y"": 1684
              },
              {
                ""x"": 697,
                ""y"": 1636
              }
            ],
            ""confidence"": 0.767
          }
        ]
      },
      {
        ""text"": ""(Accorder's Shield"",
        ""boundingPolygon"": [
          {
            ""x"": 668,
            ""y"": 964
          },
          {
            ""x"": 669,
            ""y"": 1046
          },
          {
            ""x"": 654,
            ""y"": 1046
          },
          {
            ""x"": 654,
            ""y"": 964
          }
        ],
        ""words"": [
          {
            ""text"": ""(Accorder's"",
            ""boundingPolygon"": [
              {
                ""x"": 668,
                ""y"": 966
              },
              {
                ""x"": 668,
                ""y"": 1016
              },
              {
                ""x"": 654,
                ""y"": 1016
              },
              {
                ""x"": 654,
                ""y"": 964
              }
            ],
            ""confidence"": 0.842
          },
          {
            ""text"": ""Shield"",
            ""boundingPolygon"": [
              {
                ""x"": 668,
                ""y"": 1019
              },
              {
                ""x"": 668,
                ""y"": 1045
              },
              {
                ""x"": 655,
                ""y"": 1046
              },
              {
                ""x"": 654,
                ""y"": 1019
              }
            ],
            ""confidence"": 0.993
          }
        ]
      },
      {
        ""text"": ""Arcom's Sleigh"",
        ""boundingPolygon"": [
          {
            ""x"": 659,
            ""y"": 1154
          },
          {
            ""x"": 667,
            ""y"": 1262
          },
          {
            ""x"": 656,
            ""y"": 1263
          },
          {
            ""x"": 648,
            ""y"": 1154
          }
        ],
        ""words"": [
          {
            ""text"": ""Arcom's"",
            ""boundingPolygon"": [
              {
                ""x"": 660,
                ""y"": 1159
              },
              {
                ""x"": 662,
                ""y"": 1193
              },
              {
                ""x"": 652,
                ""y"": 1192
              },
              {
                ""x"": 649,
                ""y"": 1157
              }
            ],
            ""confidence"": 0.707
          },
          {
            ""text"": ""Sleigh"",
            ""boundingPolygon"": [
              {
                ""x"": 662,
                ""y"": 1195
              },
              {
                ""x"": 664,
                ""y"": 1223
              },
              {
                ""x"": 654,
                ""y"": 1222
              },
              {
                ""x"": 652,
                ""y"": 1194
              }
            ],
            ""confidence"": 0.664
          }
        ]
      },
      {
        ""text"": ""Mirro"",
        ""boundingPolygon"": [
          {
            ""x"": 659,
            ""y"": 1373
          },
          {
            ""x"": 661,
            ""y"": 1402
          },
          {
            ""x"": 645,
            ""y"": 1405
          },
          {
            ""x"": 645,
            ""y"": 1374
          }
        ],
        ""words"": [
          {
            ""text"": ""Mirro"",
            ""boundingPolygon"": [
              {
                ""x"": 660,
                ""y"": 1373
              },
              {
                ""x"": 661,
                ""y"": 1402
              },
              {
                ""x"": 645,
                ""y"": 1403
              },
              {
                ""x"": 645,
                ""y"": 1374
              }
            ],
            ""confidence"": 0.214
          }
        ]
      },
      {
        ""text"": ""For Golem"",
        ""boundingPolygon"": [
          {
            ""x"": 664,
            ""y"": 1391
          },
          {
            ""x"": 664,
            ""y"": 1437
          },
          {
            ""x"": 652,
            ""y"": 1437
          },
          {
            ""x"": 652,
            ""y"": 1391
          }
        ],
        ""words"": [
          {
            ""text"": ""For"",
            ""boundingPolygon"": [
              {
                ""x"": 664,
                ""y"": 1391
              },
              {
                ""x"": 664,
                ""y"": 1404
              },
              {
                ""x"": 653,
                ""y"": 1406
              },
              {
                ""x"": 653,
                ""y"": 1393
              }
            ],
            ""confidence"": 0.408
          },
          {
            ""text"": ""Golem"",
            ""boundingPolygon"": [
              {
                ""x"": 664,
                ""y"": 1406
              },
              {
                ""x"": 665,
                ""y"": 1434
              },
              {
                ""x"": 653,
                ""y"": 1436
              },
              {
                ""x"": 653,
                ""y"": 1408
              }
            ],
            ""confidence"": 0.992
          }
        ]
      },
      {
        ""text"": ""Artifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 559,
            ""y"": 140
          },
          {
            ""x"": 565,
            ""y"": 234
          },
          {
            ""x"": 555,
            ""y"": 235
          },
          {
            ""x"": 550,
            ""y"": 140
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 560,
                ""y"": 141
              },
              {
                ""x"": 560,
                ""y"": 173
              },
              {
                ""x"": 552,
                ""y"": 174
              },
              {
                ""x"": 550,
                ""y"": 142
              }
            ],
            ""confidence"": 0.646
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 560,
                ""y"": 175
              },
              {
                ""x"": 561,
                ""y"": 179
              },
              {
                ""x"": 552,
                ""y"": 180
              },
              {
                ""x"": 552,
                ""y"": 176
              }
            ],
            ""confidence"": 0.897
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 561,
                ""y"": 184
              },
              {
                ""x"": 565,
                ""y"": 228
              },
              {
                ""x"": 556,
                ""y"": 229
              },
              {
                ""x"": 552,
                ""y"": 185
              }
            ],
            ""confidence"": 0.877
          }
        ]
      },
      {
        ""text"": ""Artifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 553,
            ""y"": 420
          },
          {
            ""x"": 553,
            ""y"": 518
          },
          {
            ""x"": 542,
            ""y"": 518
          },
          {
            ""x"": 542,
            ""y"": 420
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 553,
                ""y"": 423
              },
              {
                ""x"": 552,
                ""y"": 453
              },
              {
                ""x"": 542,
                ""y"": 453
              },
              {
                ""x"": 542,
                ""y"": 423
              }
            ],
            ""confidence"": 0.631
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 552,
                ""y"": 455
              },
              {
                ""x"": 552,
                ""y"": 460
              },
              {
                ""x"": 542,
                ""y"": 460
              },
              {
                ""x"": 542,
                ""y"": 455
              }
            ],
            ""confidence"": 0.958
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 551,
                ""y"": 464
              },
              {
                ""x"": 552,
                ""y"": 509
              },
              {
                ""x"": 543,
                ""y"": 509
              },
              {
                ""x"": 542,
                ""y"": 464
              }
            ],
            ""confidence"": 0.959
          }
        ]
      },
      {
        ""text"": ""ped creature gets +1/+ 1."",
        ""boundingPolygon"": [
          {
            ""x"": 526,
            ""y"": 169
          },
          {
            ""x"": 532,
            ""y"": 276
          },
          {
            ""x"": 523,
            ""y"": 277
          },
          {
            ""x"": 517,
            ""y"": 169
          }
        ],
        ""words"": [
          {
            ""text"": ""ped"",
            ""boundingPolygon"": [
              {
                ""x"": 526,
                ""y"": 170
              },
              {
                ""x"": 527,
                ""y"": 184
              },
              {
                ""x"": 518,
                ""y"": 184
              },
              {
                ""x"": 517,
                ""y"": 170
              }
            ],
            ""confidence"": 0.74
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 527,
                ""y"": 188
              },
              {
                ""x"": 529,
                ""y"": 223
              },
              {
                ""x"": 520,
                ""y"": 223
              },
              {
                ""x"": 518,
                ""y"": 188
              }
            ],
            ""confidence"": 0.662
          },
          {
            ""text"": ""gets"",
            ""boundingPolygon"": [
              {
                ""x"": 529,
                ""y"": 226
              },
              {
                ""x"": 530,
                ""y"": 243
              },
              {
                ""x"": 521,
                ""y"": 244
              },
              {
                ""x"": 521,
                ""y"": 227
              }
            ],
            ""confidence"": 0.988
          },
          {
            ""text"": ""+1/+"",
            ""boundingPolygon"": [
              {
                ""x"": 530,
                ""y"": 247
              },
              {
                ""x"": 532,
                ""y"": 266
              },
              {
                ""x"": 523,
                ""y"": 267
              },
              {
                ""x"": 522,
                ""y"": 248
              }
            ],
            ""confidence"": 0.565
          },
          {
            ""text"": ""1."",
            ""boundingPolygon"": [
              {
                ""x"": 532,
                ""y"": 268
              },
              {
                ""x"": 532,
                ""y"": 276
              },
              {
                ""x"": 523,
                ""y"": 277
              },
              {
                ""x"": 523,
                ""y"": 269
              }
            ],
            ""confidence"": 0.599
          }
        ]
      },
      {
        ""text"": ""bahn Pirate's Cutlass enters the"",
        ""boundingPolygon"": [
          {
            ""x"": 527,
            ""y"": 422
          },
          {
            ""x"": 530,
            ""y"": 571
          },
          {
            ""x"": 514,
            ""y"": 572
          },
          {
            ""x"": 511,
            ""y"": 422
          }
        ],
        ""words"": [
          {
            ""text"": ""bahn"",
            ""boundingPolygon"": [
              {
                ""x"": 526,
                ""y"": 422
              },
              {
                ""x"": 528,
                ""y"": 448
              },
              {
                ""x"": 513,
                ""y"": 449
              },
              {
                ""x"": 511,
                ""y"": 423
              }
            ],
            ""confidence"": 0.119
          },
          {
            ""text"": ""Pirate's"",
            ""boundingPolygon"": [
              {
                ""x"": 528,
                ""y"": 451
              },
              {
                ""x"": 529,
                ""y"": 483
              },
              {
                ""x"": 515,
                ""y"": 483
              },
              {
                ""x"": 513,
                ""y"": 451
              }
            ],
            ""confidence"": 0.136
          },
          {
            ""text"": ""Cutlass"",
            ""boundingPolygon"": [
              {
                ""x"": 529,
                ""y"": 486
              },
              {
                ""x"": 529,
                ""y"": 517
              },
              {
                ""x"": 516,
                ""y"": 516
              },
              {
                ""x"": 515,
                ""y"": 486
              }
            ],
            ""confidence"": 0.701
          },
          {
            ""text"": ""enters"",
            ""boundingPolygon"": [
              {
                ""x"": 529,
                ""y"": 520
              },
              {
                ""x"": 529,
                ""y"": 546
              },
              {
                ""x"": 516,
                ""y"": 545
              },
              {
                ""x"": 516,
                ""y"": 519
              }
            ],
            ""confidence"": 0.949
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 529,
                ""y"": 549
              },
              {
                ""x"": 528,
                ""y"": 564
              },
              {
                ""x"": 516,
                ""y"": 563
              },
              {
                ""x"": 516,
                ""y"": 547
              }
            ],
            ""confidence"": 0.997
          }
        ]
      },
      {
        ""text"": ""Artifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 541,
            ""y"": 699
          },
          {
            ""x"": 544,
            ""y"": 795
          },
          {
            ""x"": 529,
            ""y"": 795
          },
          {
            ""x"": 529,
            ""y"": 699
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 539,
                ""y"": 700
              },
              {
                ""x"": 542,
                ""y"": 728
              },
              {
                ""x"": 530,
                ""y"": 728
              },
              {
                ""x"": 529,
                ""y"": 700
              }
            ],
            ""confidence"": 0.989
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 542,
                ""y"": 730
              },
              {
                ""x"": 542,
                ""y"": 736
              },
              {
                ""x"": 530,
                ""y"": 736
              },
              {
                ""x"": 530,
                ""y"": 730
              }
            ],
            ""confidence"": 0.777
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 543,
                ""y"": 740
              },
              {
                ""x"": 543,
                ""y"": 784
              },
              {
                ""x"": 530,
                ""y"": 785
              },
              {
                ""x"": 531,
                ""y"": 741
              }
            ],
            ""confidence"": 0.959
          }
        ]
      },
      {
        ""text"": ""ilacı Creature -- Golem"",
        ""boundingPolygon"": [
          {
            ""x"": 570,
            ""y"": 1635
          },
          {
            ""x"": 587,
            ""y"": 1734
          },
          {
            ""x"": 579,
            ""y"": 1736
          },
          {
            ""x"": 563,
            ""y"": 1636
          }
        ],
        ""words"": [
          {
            ""text"": ""ilacı"",
            ""boundingPolygon"": [
              {
                ""x"": 570,
                ""y"": 1636
              },
              {
                ""x"": 573,
                ""y"": 1652
              },
              {
                ""x"": 566,
                ""y"": 1654
              },
              {
                ""x"": 563,
                ""y"": 1637
              }
            ],
            ""confidence"": 0.4
          },
          {
            ""text"": ""Creature"",
            ""boundingPolygon"": [
              {
                ""x"": 573,
                ""y"": 1654
              },
              {
                ""x"": 579,
                ""y"": 1684
              },
              {
                ""x"": 571,
                ""y"": 1687
              },
              {
                ""x"": 566,
                ""y"": 1656
              }
            ],
            ""confidence"": 0.877
          },
          {
            ""text"": ""--"",
            ""boundingPolygon"": [
              {
                ""x"": 579,
                ""y"": 1686
              },
              {
                ""x"": 581,
                ""y"": 1695
              },
              {
                ""x"": 573,
                ""y"": 1698
              },
              {
                ""x"": 572,
                ""y"": 1688
              }
            ],
            ""confidence"": 0.567
          },
          {
            ""text"": ""Golem"",
            ""boundingPolygon"": [
              {
                ""x"": 581,
                ""y"": 1698
              },
              {
                ""x"": 585,
                ""y"": 1718
              },
              {
                ""x"": 577,
                ""y"": 1722
              },
              {
                ""x"": 574,
                ""y"": 1701
              }
            ],
            ""confidence"": 0.887
          }
        ]
      },
      {
        ""text"": ""Equip 1 (1.Anach to target creatin"",
        ""boundingPolygon"": [
          {
            ""x"": 517,
            ""y"": 141
          },
          {
            ""x"": 520,
            ""y"": 296
          },
          {
            ""x"": 504,
            ""y"": 297
          },
          {
            ""x"": 503,
            ""y"": 141
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 518,
                ""y"": 143
              },
              {
                ""x"": 517,
                ""y"": 170
              },
              {
                ""x"": 503,
                ""y"": 169
              },
              {
                ""x"": 504,
                ""y"": 142
              }
            ],
            ""confidence"": 0.957
          },
          {
            ""text"": ""1"",
            ""boundingPolygon"": [
              {
                ""x"": 517,
                ""y"": 174
              },
              {
                ""x"": 517,
                ""y"": 181
              },
              {
                ""x"": 503,
                ""y"": 180
              },
              {
                ""x"": 503,
                ""y"": 173
              }
            ],
            ""confidence"": 0.554
          },
          {
            ""text"": ""(1.Anach"",
            ""boundingPolygon"": [
              {
                ""x"": 517,
                ""y"": 184
              },
              {
                ""x"": 517,
                ""y"": 228
              },
              {
                ""x"": 504,
                ""y"": 228
              },
              {
                ""x"": 503,
                ""y"": 183
              }
            ],
            ""confidence"": 0.412
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 517,
                ""y"": 231
              },
              {
                ""x"": 517,
                ""y"": 238
              },
              {
                ""x"": 504,
                ""y"": 238
              },
              {
                ""x"": 504,
                ""y"": 231
              }
            ],
            ""confidence"": 0.989
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 518,
                ""y"": 241
              },
              {
                ""x"": 518,
                ""y"": 264
              },
              {
                ""x"": 505,
                ""y"": 264
              },
              {
                ""x"": 504,
                ""y"": 241
              }
            ],
            ""confidence"": 0.585
          },
          {
            ""text"": ""creatin"",
            ""boundingPolygon"": [
              {
                ""x"": 518,
                ""y"": 267
              },
              {
                ""x"": 520,
                ""y"": 296
              },
              {
                ""x"": 507,
                ""y"": 296
              },
              {
                ""x"": 505,
                ""y"": 267
              }
            ],
            ""confidence"": 0.414
          }
        ]
      },
      {
        ""text"": ""quefield, attach it to"",
        ""boundingPolygon"": [
          {
            ""x"": 516,
            ""y"": 430
          },
          {
            ""x"": 523,
            ""y"": 515
          },
          {
            ""x"": 509,
            ""y"": 516
          },
          {
            ""x"": 501,
            ""y"": 430
          }
        ],
        ""words"": [
          {
            ""text"": ""quefield,"",
            ""boundingPolygon"": [
              {
                ""x"": 516,
                ""y"": 430
              },
              {
                ""x"": 520,
                ""y"": 467
              },
              {
                ""x"": 506,
                ""y"": 467
              },
              {
                ""x"": 501,
                ""y"": 430
              }
            ],
            ""confidence"": 0.357
          },
          {
            ""text"": ""attach"",
            ""boundingPolygon"": [
              {
                ""x"": 520,
                ""y"": 469
              },
              {
                ""x"": 522,
                ""y"": 496
              },
              {
                ""x"": 508,
                ""y"": 496
              },
              {
                ""x"": 506,
                ""y"": 470
              }
            ],
            ""confidence"": 0.963
          },
          {
            ""text"": ""it"",
            ""boundingPolygon"": [
              {
                ""x"": 522,
                ""y"": 498
              },
              {
                ""x"": 522,
                ""y"": 505
              },
              {
                ""x"": 508,
                ""y"": 506
              },
              {
                ""x"": 508,
                ""y"": 499
              }
            ],
            ""confidence"": 0.995
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 523,
                ""y"": 508
              },
              {
                ""x"": 523,
                ""y"": 515
              },
              {
                ""x"": 509,
                ""y"": 516
              },
              {
                ""x"": 509,
                ""y"": 509
              }
            ],
            ""confidence"": 0.994
          }
        ]
      },
      {
        ""text"": ""u control."",
        ""boundingPolygon"": [
          {
            ""x"": 509,
            ""y"": 430
          },
          {
            ""x"": 513,
            ""y"": 484
          },
          {
            ""x"": 499,
            ""y"": 485
          },
          {
            ""x"": 496,
            ""y"": 430
          }
        ],
        ""words"": [
          {
            ""text"": ""u"",
            ""boundingPolygon"": [
              {
                ""x"": 509,
                ""y"": 431
              },
              {
                ""x"": 510,
                ""y"": 437
              },
              {
                ""x"": 497,
                ""y"": 437
              },
              {
                ""x"": 496,
                ""y"": 431
              }
            ],
            ""confidence"": 0.831
          },
          {
            ""text"": ""control."",
            ""boundingPolygon"": [
              {
                ""x"": 510,
                ""y"": 440
              },
              {
                ""x"": 513,
                ""y"": 477
              },
              {
                ""x"": 499,
                ""y"": 477
              },
              {
                ""x"": 497,
                ""y"": 440
              }
            ],
            ""confidence"": 0.841
          }
        ]
      },
      {
        ""text"": ""Blades and bravery go han"",
        ""boundingPolygon"": [
          {
            ""x"": 492,
            ""y"": 144
          },
          {
            ""x"": 496,
            ""y"": 251
          },
          {
            ""x"": 486,
            ""y"": 251
          },
          {
            ""x"": 484,
            ""y"": 144
          }
        ],
        ""words"": [
          {
            ""text"": ""Blades"",
            ""boundingPolygon"": [
              {
                ""x"": 493,
                ""y"": 146
              },
              {
                ""x"": 492,
                ""y"": 170
              },
              {
                ""x"": 484,
                ""y"": 170
              },
              {
                ""x"": 484,
                ""y"": 145
              }
            ],
            ""confidence"": 0.735
          },
          {
            ""text"": ""and"",
            ""boundingPolygon"": [
              {
                ""x"": 492,
                ""y"": 172
              },
              {
                ""x"": 493,
                ""y"": 187
              },
              {
                ""x"": 484,
                ""y"": 187
              },
              {
                ""x"": 484,
                ""y"": 172
              }
            ],
            ""confidence"": 0.989
          },
          {
            ""text"": ""bravery"",
            ""boundingPolygon"": [
              {
                ""x"": 493,
                ""y"": 191
              },
              {
                ""x"": 494,
                ""y"": 221
              },
              {
                ""x"": 485,
                ""y"": 221
              },
              {
                ""x"": 484,
                ""y"": 191
              }
            ],
            ""confidence"": 0.881
          },
          {
            ""text"": ""go"",
            ""boundingPolygon"": [
              {
                ""x"": 494,
                ""y"": 224
              },
              {
                ""x"": 495,
                ""y"": 233
              },
              {
                ""x"": 486,
                ""y"": 233
              },
              {
                ""x"": 486,
                ""y"": 225
              }
            ],
            ""confidence"": 0.698
          },
          {
            ""text"": ""han"",
            ""boundingPolygon"": [
              {
                ""x"": 495,
                ""y"": 236
              },
              {
                ""x"": 496,
                ""y"": 250
              },
              {
                ""x"": 488,
                ""y"": 251
              },
              {
                ""x"": 487,
                ""y"": 236
              }
            ],
            ""confidence"": 0.864
          }
        ]
      },
      {
        ""text"": ""quipped creature gets +2/+1."",
        ""boundingPolygon"": [
          {
            ""x"": 503,
            ""y"": 424
          },
          {
            ""x"": 498,
            ""y"": 560
          },
          {
            ""x"": 486,
            ""y"": 560
          },
          {
            ""x"": 489,
            ""y"": 424
          }
        ],
        ""words"": [
          {
            ""text"": ""quipped"",
            ""boundingPolygon"": [
              {
                ""x"": 503,
                ""y"": 428
              },
              {
                ""x"": 501,
                ""y"": 463
              },
              {
                ""x"": 488,
                ""y"": 464
              },
              {
                ""x"": 489,
                ""y"": 429
              }
            ],
            ""confidence"": 0.836
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 501,
                ""y"": 466
              },
              {
                ""x"": 500,
                ""y"": 501
              },
              {
                ""x"": 488,
                ""y"": 501
              },
              {
                ""x"": 488,
                ""y"": 467
              }
            ],
            ""confidence"": 0.899
          },
          {
            ""text"": ""gets"",
            ""boundingPolygon"": [
              {
                ""x"": 499,
                ""y"": 503
              },
              {
                ""x"": 499,
                ""y"": 521
              },
              {
                ""x"": 487,
                ""y"": 521
              },
              {
                ""x"": 488,
                ""y"": 504
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""+2/+1."",
            ""boundingPolygon"": [
              {
                ""x"": 499,
                ""y"": 523
              },
              {
                ""x"": 497,
                ""y"": 555
              },
              {
                ""x"": 487,
                ""y"": 555
              },
              {
                ""x"": 487,
                ""y"": 524
              }
            ],
            ""confidence"": 0.71
          }
        ]
      },
      {
        ""text"": ""quipped creature gets +2/+2 and"",
        ""boundingPolygon"": [
          {
            ""x"": 521,
            ""y"": 706
          },
          {
            ""x"": 529,
            ""y"": 838
          },
          {
            ""x"": 516,
            ""y"": 839
          },
          {
            ""x"": 510,
            ""y"": 706
          }
        ],
        ""words"": [
          {
            ""text"": ""quipped"",
            ""boundingPolygon"": [
              {
                ""x"": 521,
                ""y"": 707
              },
              {
                ""x"": 523,
                ""y"": 739
              },
              {
                ""x"": 511,
                ""y"": 739
              },
              {
                ""x"": 510,
                ""y"": 707
              }
            ],
            ""confidence"": 0.584
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 523,
                ""y"": 741
              },
              {
                ""x"": 525,
                ""y"": 773
              },
              {
                ""x"": 513,
                ""y"": 773
              },
              {
                ""x"": 511,
                ""y"": 741
              }
            ],
            ""confidence"": 0.645
          },
          {
            ""text"": ""gets"",
            ""boundingPolygon"": [
              {
                ""x"": 525,
                ""y"": 775
              },
              {
                ""x"": 526,
                ""y"": 791
              },
              {
                ""x"": 514,
                ""y"": 791
              },
              {
                ""x"": 513,
                ""y"": 775
              }
            ],
            ""confidence"": 0.945
          },
          {
            ""text"": ""+2/+2"",
            ""boundingPolygon"": [
              {
                ""x"": 526,
                ""y"": 794
              },
              {
                ""x"": 528,
                ""y"": 817
              },
              {
                ""x"": 515,
                ""y"": 817
              },
              {
                ""x"": 514,
                ""y"": 793
              }
            ],
            ""confidence"": 0.633
          },
          {
            ""text"": ""and"",
            ""boundingPolygon"": [
              {
                ""x"": 528,
                ""y"": 820
              },
              {
                ""x"": 529,
                ""y"": 835
              },
              {
                ""x"": 516,
                ""y"": 835
              },
              {
                ""x"": 515,
                ""y"": 819
              }
            ],
            ""confidence"": 0.994
          }
        ]
      },
      {
        ""text"": ""tifact - Equipment"",
        ""boundingPolygon"": [
          {
            ""x"": 529,
            ""y"": 982
          },
          {
            ""x"": 532,
            ""y"": 1063
          },
          {
            ""x"": 520,
            ""y"": 1064
          },
          {
            ""x"": 518,
            ""y"": 983
          }
        ],
        ""words"": [
          {
            ""text"": ""tifact"",
            ""boundingPolygon"": [
              {
                ""x"": 529,
                ""y"": 983
              },
              {
                ""x"": 530,
                ""y"": 1003
              },
              {
                ""x"": 519,
                ""y"": 1004
              },
              {
                ""x"": 518,
                ""y"": 984
              }
            ],
            ""confidence"": 0.972
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 530,
                ""y"": 1005
              },
              {
                ""x"": 530,
                ""y"": 1011
              },
              {
                ""x"": 519,
                ""y"": 1012
              },
              {
                ""x"": 519,
                ""y"": 1006
              }
            ],
            ""confidence"": 0.527
          },
          {
            ""text"": ""Equipment"",
            ""boundingPolygon"": [
              {
                ""x"": 530,
                ""y"": 1015
              },
              {
                ""x"": 532,
                ""y"": 1058
              },
              {
                ""x"": 520,
                ""y"": 1060
              },
              {
                ""x"": 519,
                ""y"": 1016
              }
            ],
            ""confidence"": 0.943
          }
        ]
      },
      {
        ""text"": ""a, Sacrifice Treva's Attenda"",
        ""boundingPolygon"": [
          {
            ""x"": 544,
            ""y"": 1631
          },
          {
            ""x"": 563,
            ""y"": 1740
          },
          {
            ""x"": 551,
            ""y"": 1743
          },
          {
            ""x"": 532,
            ""y"": 1633
          }
        ],
        ""words"": [
          {
            ""text"": ""a,"",
            ""boundingPolygon"": [
              {
                ""x"": 545,
                ""y"": 1632
              },
              {
                ""x"": 546,
                ""y"": 1642
              },
              {
                ""x"": 534,
                ""y"": 1645
              },
              {
                ""x"": 533,
                ""y"": 1635
              }
            ],
            ""confidence"": 0.265
          },
          {
            ""text"": ""Sacrifice"",
            ""boundingPolygon"": [
              {
                ""x"": 546,
                ""y"": 1644
              },
              {
                ""x"": 551,
                ""y"": 1678
              },
              {
                ""x"": 540,
                ""y"": 1681
              },
              {
                ""x"": 534,
                ""y"": 1647
              }
            ],
            ""confidence"": 0.964
          },
          {
            ""text"": ""Treva's"",
            ""boundingPolygon"": [
              {
                ""x"": 551,
                ""y"": 1680
              },
              {
                ""x"": 556,
                ""y"": 1706
              },
              {
                ""x"": 546,
                ""y"": 1709
              },
              {
                ""x"": 541,
                ""y"": 1683
              }
            ],
            ""confidence"": 0.708
          },
          {
            ""text"": ""Attenda"",
            ""boundingPolygon"": [
              {
                ""x"": 556,
                ""y"": 1708
              },
              {
                ""x"": 562,
                ""y"": 1740
              },
              {
                ""x"": 553,
                ""y"": 1742
              },
              {
                ""x"": 546,
                ""y"": 1711
              }
            ],
            ""confidence"": 0.794
          }
        ]
      },
      {
        ""text"": ""Equipp"",
        ""boundingPolygon"": [
          {
            ""x"": 495,
            ""y"": 422
          },
          {
            ""x"": 497,
            ""y"": 454
          },
          {
            ""x"": 483,
            ""y"": 456
          },
          {
            ""x"": 480,
            ""y"": 423
          }
        ],
        ""words"": [
          {
            ""text"": ""Equipp"",
            ""boundingPolygon"": [
              {
                ""x"": 495,
                ""y"": 422
              },
              {
                ""x"": 498,
                ""y"": 454
              },
              {
                ""x"": 483,
                ""y"": 456
              },
              {
                ""x"": 480,
                ""y"": 423
              }
            ],
            ""confidence"": 0.751
          }
        ]
      },
      {
        ""text"": ""Equip"",
        ""boundingPolygon"": [
          {
            ""x"": 486,
            ""y"": 421
          },
          {
            ""x"": 486,
            ""y"": 453
          },
          {
            ""x"": 473,
            ""y"": 454
          },
          {
            ""x"": 473,
            ""y"": 422
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 486,
                ""y"": 421
              },
              {
                ""x"": 486,
                ""y"": 449
              },
              {
                ""x"": 473,
                ""y"": 449
              },
              {
                ""x"": 473,
                ""y"": 421
              }
            ],
            ""confidence"": 0.993
          }
        ]
      },
      {
        ""text"": ""Equip 3 (3. Attach to target creature"",
        ""boundingPolygon"": [
          {
            ""x"": 505,
            ""y"": 700
          },
          {
            ""x"": 509,
            ""y"": 843
          },
          {
            ""x"": 494,
            ""y"": 843
          },
          {
            ""x"": 489,
            ""y"": 701
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 502,
                ""y"": 701
              },
              {
                ""x"": 504,
                ""y"": 725
              },
              {
                ""x"": 492,
                ""y"": 727
              },
              {
                ""x"": 490,
                ""y"": 703
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""3"",
            ""boundingPolygon"": [
              {
                ""x"": 505,
                ""y"": 727
              },
              {
                ""x"": 505,
                ""y"": 734
              },
              {
                ""x"": 493,
                ""y"": 736
              },
              {
                ""x"": 492,
                ""y"": 729
              }
            ],
            ""confidence"": 0.722
          },
          {
            ""text"": ""(3."",
            ""boundingPolygon"": [
              {
                ""x"": 505,
                ""y"": 737
              },
              {
                ""x"": 506,
                ""y"": 751
              },
              {
                ""x"": 494,
                ""y"": 752
              },
              {
                ""x"": 493,
                ""y"": 738
              }
            ],
            ""confidence"": 0.39
          },
          {
            ""text"": ""Attach"",
            ""boundingPolygon"": [
              {
                ""x"": 507,
                ""y"": 753
              },
              {
                ""x"": 508,
                ""y"": 777
              },
              {
                ""x"": 495,
                ""y"": 779
              },
              {
                ""x"": 494,
                ""y"": 755
              }
            ],
            ""confidence"": 0.551
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 508,
                ""y"": 780
              },
              {
                ""x"": 508,
                ""y"": 787
              },
              {
                ""x"": 495,
                ""y"": 788
              },
              {
                ""x"": 495,
                ""y"": 781
              }
            ],
            ""confidence"": 0.718
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 508,
                ""y"": 789
              },
              {
                ""x"": 509,
                ""y"": 810
              },
              {
                ""x"": 495,
                ""y"": 811
              },
              {
                ""x"": 495,
                ""y"": 790
              }
            ],
            ""confidence"": 0.612
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 509,
                ""y"": 812
              },
              {
                ""x"": 508,
                ""y"": 842
              },
              {
                ""x"": 494,
                ""y"": 843
              },
              {
                ""x"": 495,
                ""y"": 814
              }
            ],
            ""confidence"": 0.799
          }
        ]
      },
      {
        ""text"": ""ou control. E"",
        ""boundingPolygon"": [
          {
            ""x"": 492,
            ""y"": 708
          },
          {
            ""x"": 494,
            ""y"": 753
          },
          {
            ""x"": 483,
            ""y"": 754
          },
          {
            ""x"": 481,
            ""y"": 708
          }
        ],
        ""words"": [
          {
            ""text"": ""ou"",
            ""boundingPolygon"": [
              {
                ""x"": 493,
                ""y"": 708
              },
              {
                ""x"": 493,
                ""y"": 715
              },
              {
                ""x"": 482,
                ""y"": 716
              },
              {
                ""x"": 482,
                ""y"": 709
              }
            ],
            ""confidence"": 0.622
          },
          {
            ""text"": ""control."",
            ""boundingPolygon"": [
              {
                ""x"": 493,
                ""y"": 717
              },
              {
                ""x"": 494,
                ""y"": 744
              },
              {
                ""x"": 483,
                ""y"": 744
              },
              {
                ""x"": 482,
                ""y"": 718
              }
            ],
            ""confidence"": 0.69
          },
          {
            ""text"": ""E"",
            ""boundingPolygon"": [
              {
                ""x"": 494,
                ""y"": 746
              },
              {
                ""x"": 495,
                ""y"": 752
              },
              {
                ""x"": 483,
                ""y"": 752
              },
              {
                ""x"": 483,
                ""y"": 746
              }
            ],
            ""confidence"": 0.502
          }
        ]
      },
      {
        ""text"": ""Las vigilan"",
        ""boundingPolygon"": [
          {
            ""x"": 502,
            ""y"": 979
          },
          {
            ""x"": 504,
            ""y"": 1023
          },
          {
            ""x"": 492,
            ""y"": 1023
          },
          {
            ""x"": 490,
            ""y"": 979
          }
        ],
        ""words"": [
          {
            ""text"": ""Las"",
            ""boundingPolygon"": [
              {
                ""x"": 503,
                ""y"": 979
              },
              {
                ""x"": 503,
                ""y"": 991
              },
              {
                ""x"": 491,
                ""y"": 991
              },
              {
                ""x"": 490,
                ""y"": 979
              }
            ],
            ""confidence"": 0.433
          },
          {
            ""text"": ""vigilan"",
            ""boundingPolygon"": [
              {
                ""x"": 503,
                ""y"": 993
              },
              {
                ""x"": 504,
                ""y"": 1021
              },
              {
                ""x"": 492,
                ""y"": 1022
              },
              {
                ""x"": 491,
                ""y"": 993
              }
            ],
            ""confidence"": 0.445
          }
        ]
      },
      {
        ""text"": ""re gets +0/+3 and"",
        ""boundingPolygon"": [
          {
            ""x"": 513,
            ""y"": 1046
          },
          {
            ""x"": 515,
            ""y"": 1122
          },
          {
            ""x"": 503,
            ""y"": 1122
          },
          {
            ""x"": 501,
            ""y"": 1046
          }
        ],
        ""words"": [
          {
            ""text"": ""re"",
            ""boundingPolygon"": [
              {
                ""x"": 514,
                ""y"": 1046
              },
              {
                ""x"": 514,
                ""y"": 1054
              },
              {
                ""x"": 502,
                ""y"": 1055
              },
              {
                ""x"": 501,
                ""y"": 1046
              }
            ],
            ""confidence"": 0.963
          },
          {
            ""text"": ""gets"",
            ""boundingPolygon"": [
              {
                ""x"": 514,
                ""y"": 1057
              },
              {
                ""x"": 514,
                ""y"": 1074
              },
              {
                ""x"": 503,
                ""y"": 1074
              },
              {
                ""x"": 502,
                ""y"": 1057
              }
            ],
            ""confidence"": 0.872
          },
          {
            ""text"": ""+0/+3"",
            ""boundingPolygon"": [
              {
                ""x"": 514,
                ""y"": 1076
              },
              {
                ""x"": 515,
                ""y"": 1101
              },
              {
                ""x"": 504,
                ""y"": 1102
              },
              {
                ""x"": 503,
                ""y"": 1077
              }
            ],
            ""confidence"": 0.989
          },
          {
            ""text"": ""and"",
            ""boundingPolygon"": [
              {
                ""x"": 515,
                ""y"": 1103
              },
              {
                ""x"": 515,
                ""y"": 1120
              },
              {
                ""x"": 505,
                ""y"": 1122
              },
              {
                ""x"": 504,
                ""y"": 1105
              }
            ],
            ""confidence"": 0.995
          }
        ]
      },
      {
        ""text"": ""rt Creature - Golem"",
        ""boundingPolygon"": [
          {
            ""x"": 531,
            ""y"": 1412
          },
          {
            ""x"": 535,
            ""y"": 1498
          },
          {
            ""x"": 521,
            ""y"": 1498
          },
          {
            ""x"": 519,
            ""y"": 1413
          }
        ],
        ""words"": [
          {
            ""text"": ""rt"",
            ""boundingPolygon"": [
              {
                ""x"": 530,
                ""y"": 1415
              },
              {
                ""x"": 529,
                ""y"": 1422
              },
              {
                ""x"": 520,
                ""y"": 1424
              },
              {
                ""x"": 521,
                ""y"": 1417
              }
            ],
            ""confidence"": 0.157
          },
          {
            ""text"": ""Creature"",
            ""boundingPolygon"": [
              {
                ""x"": 529,
                ""y"": 1424
              },
              {
                ""x"": 530,
                ""y"": 1457
              },
              {
                ""x"": 520,
                ""y"": 1459
              },
              {
                ""x"": 520,
                ""y"": 1426
              }
            ],
            ""confidence"": 0.958
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 530,
                ""y"": 1459
              },
              {
                ""x"": 530,
                ""y"": 1464
              },
              {
                ""x"": 520,
                ""y"": 1467
              },
              {
                ""x"": 520,
                ""y"": 1461
              }
            ],
            ""confidence"": 0.28
          },
          {
            ""text"": ""Golem"",
            ""boundingPolygon"": [
              {
                ""x"": 531,
                ""y"": 1469
              },
              {
                ""x"": 534,
                ""y"": 1491
              },
              {
                ""x"": 524,
                ""y"": 1494
              },
              {
                ""x"": 521,
                ""y"": 1471
              }
            ],
            ""confidence"": 0.932
          }
        ]
      },
      {
        ""text"": ""\"" control. Equip only as a sorcery"",
        ""boundingPolygon"": [
          {
            ""x"": 481,
            ""y"": 432
          },
          {
            ""x"": 480,
            ""y"": 569
          },
          {
            ""x"": 466,
            ""y"": 569
          },
          {
            ""x"": 466,
            ""y"": 432
          }
        ],
        ""words"": [
          {
            ""text"": ""\"""",
            ""boundingPolygon"": [
              {
                ""x"": 481,
                ""y"": 433
              },
              {
                ""x"": 481,
                ""y"": 435
              },
              {
                ""x"": 468,
                ""y"": 435
              },
              {
                ""x"": 468,
                ""y"": 433
              }
            ],
            ""confidence"": 0.695
          },
          {
            ""text"": ""control."",
            ""boundingPolygon"": [
              {
                ""x"": 481,
                ""y"": 437
              },
              {
                ""x"": 480,
                ""y"": 467
              },
              {
                ""x"": 467,
                ""y"": 467
              },
              {
                ""x"": 468,
                ""y"": 437
              }
            ],
            ""confidence"": 0.966
          },
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 480,
                ""y"": 470
              },
              {
                ""x"": 480,
                ""y"": 493
              },
              {
                ""x"": 466,
                ""y"": 493
              },
              {
                ""x"": 467,
                ""y"": 469
              }
            ],
            ""confidence"": 0.998
          },
          {
            ""text"": ""only"",
            ""boundingPolygon"": [
              {
                ""x"": 480,
                ""y"": 496
              },
              {
                ""x"": 480,
                ""y"": 513
              },
              {
                ""x"": 466,
                ""y"": 513
              },
              {
                ""x"": 466,
                ""y"": 496
              }
            ],
            ""confidence"": 0.999
          },
          {
            ""text"": ""as"",
            ""boundingPolygon"": [
              {
                ""x"": 480,
                ""y"": 516
              },
              {
                ""x"": 480,
                ""y"": 525
              },
              {
                ""x"": 466,
                ""y"": 524
              },
              {
                ""x"": 466,
                ""y"": 516
              }
            ],
            ""confidence"": 0.997
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 480,
                ""y"": 528
              },
              {
                ""x"": 480,
                ""y"": 533
              },
              {
                ""x"": 466,
                ""y"": 532
              },
              {
                ""x"": 466,
                ""y"": 527
              }
            ],
            ""confidence"": 1
          },
          {
            ""text"": ""sorcery"",
            ""boundingPolygon"": [
              {
                ""x"": 480,
                ""y"": 535
              },
              {
                ""x"": 480,
                ""y"": 565
              },
              {
                ""x"": 467,
                ""y"": 564
              },
              {
                ""x"": 466,
                ""y"": 535
              }
            ],
            ""confidence"": 0.729
          }
        ]
      },
      {
        ""text"": ""Equipped creature gets +"",
        ""boundingPolygon"": [
          {
            ""x"": 510,
            ""y"": 975
          },
          {
            ""x"": 511,
            ""y"": 1085
          },
          {
            ""x"": 497,
            ""y"": 1085
          },
          {
            ""x"": 496,
            ""y"": 975
          }
        ],
        ""words"": [
          {
            ""text"": ""Equipped"",
            ""boundingPolygon"": [
              {
                ""x"": 510,
                ""y"": 976
              },
              {
                ""x"": 511,
                ""y"": 1017
              },
              {
                ""x"": 497,
                ""y"": 1018
              },
              {
                ""x"": 497,
                ""y"": 977
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 511,
                ""y"": 1019
              },
              {
                ""x"": 511,
                ""y"": 1053
              },
              {
                ""x"": 497,
                ""y"": 1055
              },
              {
                ""x"": 497,
                ""y"": 1021
              }
            ],
            ""confidence"": 0.829
          },
          {
            ""text"": ""gets"",
            ""boundingPolygon"": [
              {
                ""x"": 511,
                ""y"": 1056
              },
              {
                ""x"": 511,
                ""y"": 1074
              },
              {
                ""x"": 498,
                ""y"": 1075
              },
              {
                ""x"": 497,
                ""y"": 1058
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""+"",
            ""boundingPolygon"": [
              {
                ""x"": 511,
                ""y"": 1076
              },
              {
                ""x"": 511,
                ""y"": 1084
              },
              {
                ""x"": 498,
                ""y"": 1085
              },
              {
                ""x"": 498,
                ""y"": 1078
              }
            ],
            ""confidence"": 0.187
          }
        ]
      },
      {
        ""text"": ""* * to your manswan Add"",
        ""boundingPolygon"": [
          {
            ""x"": 538,
            ""y"": 1633
          },
          {
            ""x"": 566,
            ""y"": 1769
          },
          {
            ""x"": 552,
            ""y"": 1772
          },
          {
            ""x"": 525,
            ""y"": 1636
          }
        ],
        ""words"": [
          {
            ""text"": ""*"",
            ""boundingPolygon"": [
              {
                ""x"": 539,
                ""y"": 1641
              },
              {
                ""x"": 540,
                ""y"": 1647
              },
              {
                ""x"": 528,
                ""y"": 1651
              },
              {
                ""x"": 527,
                ""y"": 1645
              }
            ],
            ""confidence"": 0.982
          },
          {
            ""text"": ""*"",
            ""boundingPolygon"": [
              {
                ""x"": 540,
                ""y"": 1650
              },
              {
                ""x"": 541,
                ""y"": 1656
              },
              {
                ""x"": 529,
                ""y"": 1660
              },
              {
                ""x"": 528,
                ""y"": 1654
              }
            ],
            ""confidence"": 0.966
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 541,
                ""y"": 1658
              },
              {
                ""x"": 543,
                ""y"": 1667
              },
              {
                ""x"": 531,
                ""y"": 1671
              },
              {
                ""x"": 529,
                ""y"": 1662
              }
            ],
            ""confidence"": 1
          },
          {
            ""text"": ""your"",
            ""boundingPolygon"": [
              {
                ""x"": 543,
                ""y"": 1669
              },
              {
                ""x"": 546,
                ""y"": 1688
              },
              {
                ""x"": 535,
                ""y"": 1692
              },
              {
                ""x"": 531,
                ""y"": 1673
              }
            ],
            ""confidence"": 0.999
          },
          {
            ""text"": ""manswan"",
            ""boundingPolygon"": [
              {
                ""x"": 547,
                ""y"": 1690
              },
              {
                ""x"": 560,
                ""y"": 1746
              },
              {
                ""x"": 550,
                ""y"": 1750
              },
              {
                ""x"": 535,
                ""y"": 1694
              }
            ],
            ""confidence"": 0.052
          },
          {
            ""text"": ""Add"",
            ""boundingPolygon"": [
              {
                ""x"": 561,
                ""y"": 1752
              },
              {
                ""x"": 566,
                ""y"": 1767
              },
              {
                ""x"": 556,
                ""y"": 1772
              },
              {
                ""x"": 552,
                ""y"": 1757
              }
            ],
            ""confidence"": 0.995
          }
        ]
      },
      {
        ""text"": ""\""We quil fight as they do our flesh pro"",
        ""boundingPolygon"": [
          {
            ""x"": 485,
            ""y"": 701
          },
          {
            ""x"": 489,
            ""y"": 834
          },
          {
            ""x"": 476,
            ""y"": 834
          },
          {
            ""x"": 473,
            ""y"": 702
          }
        ],
        ""words"": [
          {
            ""text"": ""\""We"",
            ""boundingPolygon"": [
              {
                ""x"": 485,
                ""y"": 702
              },
              {
                ""x"": 486,
                ""y"": 716
              },
              {
                ""x"": 473,
                ""y"": 717
              },
              {
                ""x"": 473,
                ""y"": 704
              }
            ],
            ""confidence"": 0.714
          },
          {
            ""text"": ""quil"",
            ""boundingPolygon"": [
              {
                ""x"": 486,
                ""y"": 718
              },
              {
                ""x"": 486,
                ""y"": 731
              },
              {
                ""x"": 473,
                ""y"": 733
              },
              {
                ""x"": 473,
                ""y"": 720
              }
            ],
            ""confidence"": 0.161
          },
          {
            ""text"": ""fight"",
            ""boundingPolygon"": [
              {
                ""x"": 486,
                ""y"": 733
              },
              {
                ""x"": 486,
                ""y"": 750
              },
              {
                ""x"": 474,
                ""y"": 751
              },
              {
                ""x"": 473,
                ""y"": 735
              }
            ],
            ""confidence"": 0.786
          },
          {
            ""text"": ""as"",
            ""boundingPolygon"": [
              {
                ""x"": 486,
                ""y"": 752
              },
              {
                ""x"": 486,
                ""y"": 759
              },
              {
                ""x"": 474,
                ""y"": 761
              },
              {
                ""x"": 474,
                ""y"": 753
              }
            ],
            ""confidence"": 0.591
          },
          {
            ""text"": ""they"",
            ""boundingPolygon"": [
              {
                ""x"": 486,
                ""y"": 762
              },
              {
                ""x"": 487,
                ""y"": 776
              },
              {
                ""x"": 475,
                ""y"": 778
              },
              {
                ""x"": 474,
                ""y"": 763
              }
            ],
            ""confidence"": 0.97
          },
          {
            ""text"": ""do"",
            ""boundingPolygon"": [
              {
                ""x"": 487,
                ""y"": 779
              },
              {
                ""x"": 487,
                ""y"": 789
              },
              {
                ""x"": 475,
                ""y"": 790
              },
              {
                ""x"": 475,
                ""y"": 780
              }
            ],
            ""confidence"": 0.289
          },
          {
            ""text"": ""our"",
            ""boundingPolygon"": [
              {
                ""x"": 487,
                ""y"": 792
              },
              {
                ""x"": 488,
                ""y"": 803
              },
              {
                ""x"": 476,
                ""y"": 804
              },
              {
                ""x"": 475,
                ""y"": 793
              }
            ],
            ""confidence"": 0.963
          },
          {
            ""text"": ""flesh"",
            ""boundingPolygon"": [
              {
                ""x"": 488,
                ""y"": 805
              },
              {
                ""x"": 489,
                ""y"": 821
              },
              {
                ""x"": 477,
                ""y"": 822
              },
              {
                ""x"": 476,
                ""y"": 806
              }
            ],
            ""confidence"": 0.673
          },
          {
            ""text"": ""pro"",
            ""boundingPolygon"": [
              {
                ""x"": 489,
                ""y"": 823
              },
              {
                ""x"": 489,
                ""y"": 834
              },
              {
                ""x"": 477,
                ""y"": 835
              },
              {
                ""x"": 477,
                ""y"": 824
              }
            ],
            ""confidence"": 0.827
          }
        ]
      },
      {
        ""text"": ""2,0: Attacking Thus futh ap- You"",
        ""boundingPolygon"": [
          {
            ""x"": 510,
            ""y"": 1184
          },
          {
            ""x"": 514,
            ""y"": 1314
          },
          {
            ""x"": 498,
            ""y"": 1315
          },
          {
            ""x"": 496,
            ""y"": 1185
          }
        ],
        ""words"": [
          {
            ""text"": ""2,0:"",
            ""boundingPolygon"": [
              {
                ""x"": 507,
                ""y"": 1185
              },
              {
                ""x"": 507,
                ""y"": 1202
              },
              {
                ""x"": 497,
                ""y"": 1204
              },
              {
                ""x"": 496,
                ""y"": 1187
              }
            ],
            ""confidence"": 0.535
          },
          {
            ""text"": ""Attacking"",
            ""boundingPolygon"": [
              {
                ""x"": 508,
                ""y"": 1204
              },
              {
                ""x"": 510,
                ""y"": 1241
              },
              {
                ""x"": 498,
                ""y"": 1243
              },
              {
                ""x"": 497,
                ""y"": 1206
              }
            ],
            ""confidence"": 0.845
          },
          {
            ""text"": ""Thus"",
            ""boundingPolygon"": [
              {
                ""x"": 510,
                ""y"": 1244
              },
              {
                ""x"": 511,
                ""y"": 1258
              },
              {
                ""x"": 498,
                ""y"": 1260
              },
              {
                ""x"": 498,
                ""y"": 1246
              }
            ],
            ""confidence"": 0.192
          },
          {
            ""text"": ""futh"",
            ""boundingPolygon"": [
              {
                ""x"": 511,
                ""y"": 1261
              },
              {
                ""x"": 512,
                ""y"": 1279
              },
              {
                ""x"": 498,
                ""y"": 1281
              },
              {
                ""x"": 498,
                ""y"": 1263
              }
            ],
            ""confidence"": 0.129
          },
          {
            ""text"": ""ap-"",
            ""boundingPolygon"": [
              {
                ""x"": 512,
                ""y"": 1281
              },
              {
                ""x"": 513,
                ""y"": 1295
              },
              {
                ""x"": 498,
                ""y"": 1297
              },
              {
                ""x"": 498,
                ""y"": 1283
              }
            ],
            ""confidence"": 0.149
          },
          {
            ""text"": ""You"",
            ""boundingPolygon"": [
              {
                ""x"": 514,
                ""y"": 1298
              },
              {
                ""x"": 515,
                ""y"": 1313
              },
              {
                ""x"": 498,
                ""y"": 1315
              },
              {
                ""x"": 498,
                ""y"": 1300
              }
            ],
            ""confidence"": 0.725
          }
        ]
      },
      {
        ""text"": ""m. comes"",
        ""boundingPolygon"": [
          {
            ""x"": 521,
            ""y"": 1498
          },
          {
            ""x"": 527,
            ""y"": 1540
          },
          {
            ""x"": 515,
            ""y"": 1542
          },
          {
            ""x"": 510,
            ""y"": 1500
          }
        ],
        ""words"": [
          {
            ""text"": ""m."",
            ""boundingPolygon"": [
              {
                ""x"": 522,
                ""y"": 1500
              },
              {
                ""x"": 523,
                ""y"": 1507
              },
              {
                ""x"": 511,
                ""y"": 1509
              },
              {
                ""x"": 510,
                ""y"": 1502
              }
            ],
            ""confidence"": 0.153
          },
          {
            ""text"": ""comes"",
            ""boundingPolygon"": [
              {
                ""x"": 523,
                ""y"": 1509
              },
              {
                ""x"": 526,
                ""y"": 1533
              },
              {
                ""x"": 516,
                ""y"": 1536
              },
              {
                ""x"": 512,
                ""y"": 1512
              }
            ],
            ""confidence"": 0.725
          }
        ]
      },
      {
        ""text"": ""sh protect"",
        ""boundingPolygon"": [
          {
            ""x"": 484,
            ""y"": 813
          },
          {
            ""x"": 485,
            ""y"": 849
          },
          {
            ""x"": 473,
            ""y"": 850
          },
          {
            ""x"": 472,
            ""y"": 815
          }
        ],
        ""words"": [
          {
            ""text"": ""sh"",
            ""boundingPolygon"": [
              {
                ""x"": 484,
                ""y"": 813
              },
              {
                ""x"": 484,
                ""y"": 821
              },
              {
                ""x"": 472,
                ""y"": 821
              },
              {
                ""x"": 472,
                ""y"": 814
              }
            ],
            ""confidence"": 0.227
          },
          {
            ""text"": ""protect"",
            ""boundingPolygon"": [
              {
                ""x"": 484,
                ""y"": 823
              },
              {
                ""x"": 485,
                ""y"": 849
              },
              {
                ""x"": 473,
                ""y"": 850
              },
              {
                ""x"": 472,
                ""y"": 823
              }
            ],
            ""confidence"": 0.709
          }
        ]
      },
      {
        ""text"": ""e target creature to"",
        ""boundingPolygon"": [
          {
            ""x"": 499,
            ""y"": 1202
          },
          {
            ""x"": 506,
            ""y"": 1284
          },
          {
            ""x"": 491,
            ""y"": 1285
          },
          {
            ""x"": 487,
            ""y"": 1203
          }
        ],
        ""words"": [
          {
            ""text"": ""e"",
            ""boundingPolygon"": [
              {
                ""x"": 500,
                ""y"": 1202
              },
              {
                ""x"": 500,
                ""y"": 1206
              },
              {
                ""x"": 487,
                ""y"": 1207
              },
              {
                ""x"": 487,
                ""y"": 1204
              }
            ],
            ""confidence"": 0.673
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 500,
                ""y"": 1208
              },
              {
                ""x"": 500,
                ""y"": 1232
              },
              {
                ""x"": 487,
                ""y"": 1234
              },
              {
                ""x"": 487,
                ""y"": 1210
              }
            ],
            ""confidence"": 0.931
          },
          {
            ""text"": ""creature"",
            ""boundingPolygon"": [
              {
                ""x"": 500,
                ""y"": 1234
              },
              {
                ""x"": 503,
                ""y"": 1266
              },
              {
                ""x"": 491,
                ""y"": 1268
              },
              {
                ""x"": 488,
                ""y"": 1236
              }
            ],
            ""confidence"": 0.582
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 504,
                ""y"": 1269
              },
              {
                ""x"": 505,
                ""y"": 1278
              },
              {
                ""x"": 493,
                ""y"": 1280
              },
              {
                ""x"": 491,
                ""y"": 1270
              }
            ],
            ""confidence"": 0.537
          }
        ]
      },
      {
        ""text"": ""if defendin"",
        ""boundingPolygon"": [
          {
            ""x"": 501,
            ""y"": 1274
          },
          {
            ""x"": 504,
            ""y"": 1313
          },
          {
            ""x"": 490,
            ""y"": 1315
          },
          {
            ""x"": 486,
            ""y"": 1275
          }
        ],
        ""words"": [
          {
            ""text"": ""if"",
            ""boundingPolygon"": [
              {
                ""x"": 501,
                ""y"": 1274
              },
              {
                ""x"": 501,
                ""y"": 1277
              },
              {
                ""x"": 486,
                ""y"": 1278
              },
              {
                ""x"": 486,
                ""y"": 1275
              }
            ],
            ""confidence"": 0.276
          },
          {
            ""text"": ""defendin"",
            ""boundingPolygon"": [
              {
                ""x"": 502,
                ""y"": 1280
              },
              {
                ""x"": 505,
                ""y"": 1313
              },
              {
                ""x"": 490,
                ""y"": 1315
              },
              {
                ""x"": 487,
                ""y"": 1281
              }
            ],
            ""confidence"": 0.882
          }
        ]
      },
      {
        ""text"": ""A. Mittore target card in a"",
        ""boundingPolygon"": [
          {
            ""x"": 514,
            ""y"": 1453
          },
          {
            ""x"": 521,
            ""y"": 1551
          },
          {
            ""x"": 507,
            ""y"": 1552
          },
          {
            ""x"": 502,
            ""y"": 1454
          }
        ],
        ""words"": [
          {
            ""text"": ""A."",
            ""boundingPolygon"": [
              {
                ""x"": 515,
                ""y"": 1454
              },
              {
                ""x"": 515,
                ""y"": 1456
              },
              {
                ""x"": 502,
                ""y"": 1458
              },
              {
                ""x"": 502,
                ""y"": 1455
              }
            ],
            ""confidence"": 0.282
          },
          {
            ""text"": ""Mittore"",
            ""boundingPolygon"": [
              {
                ""x"": 515,
                ""y"": 1458
              },
              {
                ""x"": 515,
                ""y"": 1487
              },
              {
                ""x"": 502,
                ""y"": 1490
              },
              {
                ""x"": 502,
                ""y"": 1460
              }
            ],
            ""confidence"": 0.339
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 515,
                ""y"": 1490
              },
              {
                ""x"": 516,
                ""y"": 1509
              },
              {
                ""x"": 504,
                ""y"": 1512
              },
              {
                ""x"": 503,
                ""y"": 1492
              }
            ],
            ""confidence"": 0.997
          },
          {
            ""text"": ""card"",
            ""boundingPolygon"": [
              {
                ""x"": 516,
                ""y"": 1512
              },
              {
                ""x"": 518,
                ""y"": 1527
              },
              {
                ""x"": 506,
                ""y"": 1529
              },
              {
                ""x"": 504,
                ""y"": 1514
              }
            ],
            ""confidence"": 0.999
          },
          {
            ""text"": ""in"",
            ""boundingPolygon"": [
              {
                ""x"": 518,
                ""y"": 1529
              },
              {
                ""x"": 519,
                ""y"": 1536
              },
              {
                ""x"": 508,
                ""y"": 1539
              },
              {
                ""x"": 507,
                ""y"": 1532
              }
            ],
            ""confidence"": 0.992
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 519,
                ""y"": 1538
              },
              {
                ""x"": 520,
                ""y"": 1544
              },
              {
                ""x"": 509,
                ""y"": 1547
              },
              {
                ""x"": 508,
                ""y"": 1541
              }
            ],
            ""confidence"": 0.996
          }
        ]
      },
      {
        ""text"": ""\""Treva is the voice of the ur-dragon"",
        ""boundingPolygon"": [
          {
            ""x"": 522,
            ""y"": 1638
          },
          {
            ""x"": 542,
            ""y"": 1770
          },
          {
            ""x"": 524,
            ""y"": 1773
          },
          {
            ""x"": 507,
            ""y"": 1642
          }
        ],
        ""words"": [
          {
            ""text"": ""\""Treva"",
            ""boundingPolygon"": [
              {
                ""x"": 523,
                ""y"": 1639
              },
              {
                ""x"": 523,
                ""y"": 1661
              },
              {
                ""x"": 508,
                ""y"": 1664
              },
              {
                ""x"": 507,
                ""y"": 1642
              }
            ],
            ""confidence"": 0.628
          },
          {
            ""text"": ""is"",
            ""boundingPolygon"": [
              {
                ""x"": 523,
                ""y"": 1664
              },
              {
                ""x"": 523,
                ""y"": 1670
              },
              {
                ""x"": 509,
                ""y"": 1673
              },
              {
                ""x"": 509,
                ""y"": 1667
              }
            ],
            ""confidence"": 0.882
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 523,
                ""y"": 1673
              },
              {
                ""x"": 524,
                ""y"": 1684
              },
              {
                ""x"": 511,
                ""y"": 1688
              },
              {
                ""x"": 509,
                ""y"": 1676
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""voice"",
            ""boundingPolygon"": [
              {
                ""x"": 525,
                ""y"": 1687
              },
              {
                ""x"": 527,
                ""y"": 1706
              },
              {
                ""x"": 514,
                ""y"": 1709
              },
              {
                ""x"": 511,
                ""y"": 1690
              }
            ],
            ""confidence"": 0.27
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 527,
                ""y"": 1708
              },
              {
                ""x"": 528,
                ""y"": 1715
              },
              {
                ""x"": 516,
                ""y"": 1719
              },
              {
                ""x"": 515,
                ""y"": 1712
              }
            ],
            ""confidence"": 0.921
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 529,
                ""y"": 1718
              },
              {
                ""x"": 531,
                ""y"": 1728
              },
              {
                ""x"": 519,
                ""y"": 1732
              },
              {
                ""x"": 517,
                ""y"": 1721
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""ur-dragon"",
            ""boundingPolygon"": [
              {
                ""x"": 532,
                ""y"": 1731
              },
              {
                ""x"": 542,
                ""y"": 1769
              },
              {
                ""x"": 530,
                ""y"": 1773
              },
              {
                ""x"": 520,
                ""y"": 1735
              }
            ],
            ""confidence"": 0.687
          }
        ]
      },
      {
        ""text"": ""Ta Aqui, Viridian weaponsmith"",
        ""boundingPolygon"": [
          {
            ""x"": 469,
            ""y"": 703
          },
          {
            ""x"": 471,
            ""y"": 831
          },
          {
            ""x"": 458,
            ""y"": 832
          },
          {
            ""x"": 456,
            ""y"": 703
          }
        ],
        ""words"": [
          {
            ""text"": ""Ta"",
            ""boundingPolygon"": [
              {
                ""x"": 469,
                ""y"": 712
              },
              {
                ""x"": 469,
                ""y"": 722
              },
              {
                ""x"": 456,
                ""y"": 723
              },
              {
                ""x"": 456,
                ""y"": 713
              }
            ],
            ""confidence"": 0.127
          },
          {
            ""text"": ""Aqui,"",
            ""boundingPolygon"": [
              {
                ""x"": 469,
                ""y"": 727
              },
              {
                ""x"": 468,
                ""y"": 746
              },
              {
                ""x"": 456,
                ""y"": 747
              },
              {
                ""x"": 456,
                ""y"": 728
              }
            ],
            ""confidence"": 0.177
          },
          {
            ""text"": ""Viridian"",
            ""boundingPolygon"": [
              {
                ""x"": 468,
                ""y"": 748
              },
              {
                ""x"": 469,
                ""y"": 777
              },
              {
                ""x"": 457,
                ""y"": 778
              },
              {
                ""x"": 456,
                ""y"": 749
              }
            ],
            ""confidence"": 0.764
          },
          {
            ""text"": ""weaponsmith"",
            ""boundingPolygon"": [
              {
                ""x"": 469,
                ""y"": 780
              },
              {
                ""x"": 471,
                ""y"": 828
              },
              {
                ""x"": 460,
                ""y"": 829
              },
              {
                ""x"": 457,
                ""y"": 781
              }
            ],
            ""confidence"": 0.488
          }
        ]
      },
      {
        ""text"": ""Equip 3"",
        ""boundingPolygon"": [
          {
            ""x"": 489,
            ""y"": 976
          },
          {
            ""x"": 491,
            ""y"": 1019
          },
          {
            ""x"": 475,
            ""y"": 1020
          },
          {
            ""x"": 474,
            ""y"": 976
          }
        ],
        ""words"": [
          {
            ""text"": ""Equip"",
            ""boundingPolygon"": [
              {
                ""x"": 490,
                ""y"": 977
              },
              {
                ""x"": 488,
                ""y"": 1003
              },
              {
                ""x"": 475,
                ""y"": 1003
              },
              {
                ""x"": 476,
                ""y"": 977
              }
            ],
            ""confidence"": 0.991
          },
          {
            ""text"": ""3"",
            ""boundingPolygon"": [
              {
                ""x"": 489,
                ""y"": 1006
              },
              {
                ""x"": 490,
                ""y"": 1013
              },
              {
                ""x"": 477,
                ""y"": 1014
              },
              {
                ""x"": 476,
                ""y"": 1007
              }
            ],
            ""confidence"": 0.62
          }
        ]
      },
      {
        ""text"": ""ILh - In Sam SI"",
        ""boundingPolygon"": [
          {
            ""x"": 455,
            ""y"": 416
          },
          {
            ""x"": 453,
            ""y"": 477
          },
          {
            ""x"": 442,
            ""y"": 477
          },
          {
            ""x"": 444,
            ""y"": 415
          }
        ],
        ""words"": [
          {
            ""text"": ""ILh"",
            ""boundingPolygon"": [
              {
                ""x"": 456,
                ""y"": 417
              },
              {
                ""x"": 455,
                ""y"": 424
              },
              {
                ""x"": 444,
                ""y"": 423
              },
              {
                ""x"": 444,
                ""y"": 416
              }
            ],
            ""confidence"": 0.013
          },
          {
            ""text"": ""-"",
            ""boundingPolygon"": [
              {
                ""x"": 455,
                ""y"": 426
              },
              {
                ""x"": 455,
                ""y"": 427
              },
              {
                ""x"": 444,
                ""y"": 427
              },
              {
                ""x"": 444,
                ""y"": 425
              }
            ],
            ""confidence"": 0.597
          },
          {
            ""text"": ""In"",
            ""boundingPolygon"": [
              {
                ""x"": 455,
                ""y"": 429
              },
              {
                ""x"": 454,
                ""y"": 437
              },
              {
                ""x"": 443,
                ""y"": 437
              },
              {
                ""x"": 444,
                ""y"": 428
              }
            ],
            ""confidence"": 0.033
          },
          {
            ""text"": ""Sam"",
            ""boundingPolygon"": [
              {
                ""x"": 454,
                ""y"": 439
              },
              {
                ""x"": 453,
                ""y"": 454
              },
              {
                ""x"": 443,
                ""y"": 455
              },
              {
                ""x"": 443,
                ""y"": 439
              }
            ],
            ""confidence"": 0.324
          },
          {
            ""text"": ""SI"",
            ""boundingPolygon"": [
              {
                ""x"": 453,
                ""y"": 456
              },
              {
                ""x"": 453,
                ""y"": 464
              },
              {
                ""x"": 442,
                ""y"": 465
              },
              {
                ""x"": 443,
                ""y"": 457
              }
            ],
            ""confidence"": 0.003
          }
        ]
      },
      {
        ""text"": ""An Auriok shield is polished to a"",
        ""boundingPolygon"": [
          {
            ""x"": 476,
            ""y"": 980
          },
          {
            ""x"": 478,
            ""y"": 1109
          },
          {
            ""x"": 463,
            ""y"": 1109
          },
          {
            ""x"": 462,
            ""y"": 980
          }
        ],
        ""words"": [
          {
            ""text"": ""An"",
            ""boundingPolygon"": [
              {
                ""x"": 477,
                ""y"": 980
              },
              {
                ""x"": 477,
                ""y"": 990
              },
              {
                ""x"": 462,
                ""y"": 990
              },
              {
                ""x"": 462,
                ""y"": 980
              }
            ],
            ""confidence"": 0.9
          },
          {
            ""text"": ""Auriok"",
            ""boundingPolygon"": [
              {
                ""x"": 477,
                ""y"": 992
              },
              {
                ""x"": 477,
                ""y"": 1018
              },
              {
                ""x"": 463,
                ""y"": 1018
              },
              {
                ""x"": 462,
                ""y"": 993
              }
            ],
            ""confidence"": 0.88
          },
          {
            ""text"": ""shield"",
            ""boundingPolygon"": [
              {
                ""x"": 477,
                ""y"": 1020
              },
              {
                ""x"": 477,
                ""y"": 1042
              },
              {
                ""x"": 463,
                ""y"": 1043
              },
              {
                ""x"": 463,
                ""y"": 1021
              }
            ],
            ""confidence"": 0.749
          },
          {
            ""text"": ""is"",
            ""boundingPolygon"": [
              {
                ""x"": 477,
                ""y"": 1045
              },
              {
                ""x"": 477,
                ""y"": 1049
              },
              {
                ""x"": 464,
                ""y"": 1050
              },
              {
                ""x"": 463,
                ""y"": 1046
              }
            ],
            ""confidence"": 0.882
          },
          {
            ""text"": ""polished"",
            ""boundingPolygon"": [
              {
                ""x"": 478,
                ""y"": 1052
              },
              {
                ""x"": 478,
                ""y"": 1082
              },
              {
                ""x"": 464,
                ""y"": 1083
              },
              {
                ""x"": 464,
                ""y"": 1053
              }
            ],
            ""confidence"": 0.761
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 478,
                ""y"": 1085
              },
              {
                ""x"": 478,
                ""y"": 1092
              },
              {
                ""x"": 464,
                ""y"": 1094
              },
              {
                ""x"": 464,
                ""y"": 1086
              }
            ],
            ""confidence"": 0.775
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 478,
                ""y"": 1095
              },
              {
                ""x"": 478,
                ""y"": 1102
              },
              {
                ""x"": 464,
                ""y"": 1104
              },
              {
                ""x"": 464,
                ""y"": 1097
              }
            ],
            ""confidence"": 0.617
          }
        ]
      },
      {
        ""text"": ""cannot use"",
        ""boundingPolygon"": [
          {
            ""x"": 491,
            ""y"": 1184
          },
          {
            ""x"": 494,
            ""y"": 1229
          },
          {
            ""x"": 481,
            ""y"": 1230
          },
          {
            ""x"": 478,
            ""y"": 1185
          }
        ],
        ""words"": [
          {
            ""text"": ""cannot"",
            ""boundingPolygon"": [
              {
                ""x"": 491,
                ""y"": 1185
              },
              {
                ""x"": 493,
                ""y"": 1212
              },
              {
                ""x"": 480,
                ""y"": 1214
              },
              {
                ""x"": 478,
                ""y"": 1187
              }
            ],
            ""confidence"": 0.582
          },
          {
            ""text"": ""use"",
            ""boundingPolygon"": [
              {
                ""x"": 494,
                ""y"": 1214
              },
              {
                ""x"": 495,
                ""y"": 1228
              },
              {
                ""x"": 482,
                ""y"": 1230
              },
              {
                ""x"": 480,
                ""y"": 1216
              }
            ],
            ""confidence"": 0.957
          }
        ]
      },
      {
        ""text"": ""finish cuen on the inside,"",
        ""boundingPolygon"": [
          {
            ""x"": 470,
            ""y"": 975
          },
          {
            ""x"": 472,
            ""y"": 1075
          },
          {
            ""x"": 456,
            ""y"": 1075
          },
          {
            ""x"": 455,
            ""y"": 975
          }
        ],
        ""words"": [
          {
            ""text"": ""finish"",
            ""boundingPolygon"": [
              {
                ""x"": 470,
                ""y"": 978
              },
              {
                ""x"": 471,
                ""y"": 998
              },
              {
                ""x"": 456,
                ""y"": 998
              },
              {
                ""x"": 455,
                ""y"": 978
              }
            ],
            ""confidence"": 0.202
          },
          {
            ""text"": ""cuen"",
            ""boundingPolygon"": [
              {
                ""x"": 471,
                ""y"": 1001
              },
              {
                ""x"": 471,
                ""y"": 1017
              },
              {
                ""x"": 456,
                ""y"": 1018
              },
              {
                ""x"": 456,
                ""y"": 1001
              }
            ],
            ""confidence"": 0.259
          },
          {
            ""text"": ""on"",
            ""boundingPolygon"": [
              {
                ""x"": 471,
                ""y"": 1020
              },
              {
                ""x"": 471,
                ""y"": 1029
              },
              {
                ""x"": 456,
                ""y"": 1029
              },
              {
                ""x"": 456,
                ""y"": 1021
              }
            ],
            ""confidence"": 0.854
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 471,
                ""y"": 1032
              },
              {
                ""x"": 471,
                ""y"": 1042
              },
              {
                ""x"": 457,
                ""y"": 1043
              },
              {
                ""x"": 457,
                ""y"": 1032
              }
            ],
            ""confidence"": 0.691
          },
          {
            ""text"": ""inside,"",
            ""boundingPolygon"": [
              {
                ""x"": 471,
                ""y"": 1045
              },
              {
                ""x"": 472,
                ""y"": 1074
              },
              {
                ""x"": 458,
                ""y"": 1075
              },
              {
                ""x"": 457,
                ""y"": 1046
              }
            ],
            ""confidence"": 0.296
          }
        ]
      },
      {
        ""text"": ""lands."",
        ""boundingPolygon"": [
          {
            ""x"": 477,
            ""y"": 1189
          },
          {
            ""x"": 479,
            ""y"": 1226
          },
          {
            ""x"": 466,
            ""y"": 1228
          },
          {
            ""x"": 464,
            ""y"": 1190
          }
        ],
        ""words"": [
          {
            ""text"": ""lands."",
            ""boundingPolygon"": [
              {
                ""x"": 478,
                ""y"": 1189
              },
              {
                ""x"": 479,
                ""y"": 1215
              },
              {
                ""x"": 465,
                ""y"": 1216
              },
              {
                ""x"": 464,
                ""y"": 1189
              }
            ],
            ""confidence"": 0.26
          }
        ]
      },
      {
        ""text"": ""controls no snot"",
        ""boundingPolygon"": [
          {
            ""x"": 487,
            ""y"": 1209
          },
          {
            ""x"": 492,
            ""y"": 1277
          },
          {
            ""x"": 479,
            ""y"": 1278
          },
          {
            ""x"": 475,
            ""y"": 1210
          }
        ],
        ""words"": [
          {
            ""text"": ""controls"",
            ""boundingPolygon"": [
              {
                ""x"": 487,
                ""y"": 1212
              },
              {
                ""x"": 489,
                ""y"": 1245
              },
              {
                ""x"": 477,
                ""y"": 1246
              },
              {
                ""x"": 476,
                ""y"": 1214
              }
            ],
            ""confidence"": 0.948
          },
          {
            ""text"": ""no"",
            ""boundingPolygon"": [
              {
                ""x"": 489,
                ""y"": 1247
              },
              {
                ""x"": 490,
                ""y"": 1258
              },
              {
                ""x"": 479,
                ""y"": 1259
              },
              {
                ""x"": 477,
                ""y"": 1249
              }
            ],
            ""confidence"": 0.989
          },
          {
            ""text"": ""snot"",
            ""boundingPolygon"": [
              {
                ""x"": 490,
                ""y"": 1261
              },
              {
                ""x"": 492,
                ""y"": 1277
              },
              {
                ""x"": 481,
                ""y"": 1278
              },
              {
                ""x"": 479,
                ""y"": 1262
              }
            ],
            ""confidence"": 0.597
          }
        ]
      },
      {
        ""text"": ""ut and"",
        ""boundingPolygon"": [
          {
            ""x"": 474,
            ""y"": 1281
          },
          {
            ""x"": 477,
            ""y"": 1311
          },
          {
            ""x"": 466,
            ""y"": 1312
          },
          {
            ""x"": 464,
            ""y"": 1282
          }
        ],
        ""words"": [
          {
            ""text"": ""ut"",
            ""boundingPolygon"": [
              {
                ""x"": 474,
                ""y"": 1281
              },
              {
                ""x"": 475,
                ""y"": 1288
              },
              {
                ""x"": 464,
                ""y"": 1289
              },
              {
                ""x"": 464,
                ""y"": 1282
              }
            ],
            ""confidence"": 0.138
          },
          {
            ""text"": ""and"",
            ""boundingPolygon"": [
              {
                ""x"": 475,
                ""y"": 1290
              },
              {
                ""x"": 477,
                ""y"": 1305
              },
              {
                ""x"": 465,
                ""y"": 1305
              },
              {
                ""x"": 465,
                ""y"": 1291
              }
            ],
            ""confidence"": 0.959
          }
        ]
      },
      {
        ""text"": ""nection from each o"",
        ""boundingPolygon"": [
          {
            ""x"": 483,
            ""y"": 1478
          },
          {
            ""x"": 494,
            ""y"": 1546
          },
          {
            ""x"": 480,
            ""y"": 1548
          },
          {
            ""x"": 472,
            ""y"": 1479
          }
        ],
        ""words"": [
          {
            ""text"": ""nection"",
            ""boundingPolygon"": [
              {
                ""x"": 484,
                ""y"": 1479
              },
              {
                ""x"": 484,
                ""y"": 1503
              },
              {
                ""x"": 473,
                ""y"": 1504
              },
              {
                ""x"": 472,
                ""y"": 1479
              }
            ],
            ""confidence"": 0.85
          },
          {
            ""text"": ""from"",
            ""boundingPolygon"": [
              {
                ""x"": 484,
                ""y"": 1505
              },
              {
                ""x"": 487,
                ""y"": 1520
              },
              {
                ""x"": 476,
                ""y"": 1522
              },
              {
                ""x"": 473,
                ""y"": 1506
              }
            ],
            ""confidence"": 0.872
          },
          {
            ""text"": ""each"",
            ""boundingPolygon"": [
              {
                ""x"": 488,
                ""y"": 1524
              },
              {
                ""x"": 492,
                ""y"": 1539
              },
              {
                ""x"": 481,
                ""y"": 1541
              },
              {
                ""x"": 477,
                ""y"": 1526
              }
            ],
            ""confidence"": 0.912
          },
          {
            ""text"": ""o"",
            ""boundingPolygon"": [
              {
                ""x"": 492,
                ""y"": 1541
              },
              {
                ""x"": 494,
                ""y"": 1545
              },
              {
                ""x"": 483,
                ""y"": 1548
              },
              {
                ""x"": 482,
                ""y"": 1544
              }
            ],
            ""confidence"": 0.66
          }
        ]
      },
      {
        ""text"": ""Mus- Christopher Mecher"",
        ""boundingPolygon"": [
          {
            ""x"": 485,
            ""y"": 1665
          },
          {
            ""x"": 498,
            ""y"": 1763
          },
          {
            ""x"": 485,
            ""y"": 1764
          },
          {
            ""x"": 473,
            ""y"": 1667
          }
        ],
        ""words"": [
          {
            ""text"": ""Mus-"",
            ""boundingPolygon"": [
              {
                ""x"": 486,
                ""y"": 1669
              },
              {
                ""x"": 488,
                ""y"": 1685
              },
              {
                ""x"": 476,
                ""y"": 1687
              },
              {
                ""x"": 473,
                ""y"": 1672
              }
            ],
            ""confidence"": 0.195
          },
          {
            ""text"": ""Christopher"",
            ""boundingPolygon"": [
              {
                ""x"": 489,
                ""y"": 1687
              },
              {
                ""x"": 494,
                ""y"": 1725
              },
              {
                ""x"": 482,
                ""y"": 1727
              },
              {
                ""x"": 476,
                ""y"": 1690
              }
            ],
            ""confidence"": 0.68
          },
          {
            ""text"": ""Mecher"",
            ""boundingPolygon"": [
              {
                ""x"": 494,
                ""y"": 1727
              },
              {
                ""x"": 497,
                ""y"": 1756
              },
              {
                ""x"": 485,
                ""y"": 1757
              },
              {
                ""x"": 482,
                ""y"": 1729
              }
            ],
            ""confidence"": 0.334
          }
        ]
      },
      {
        ""text"": ""3/3"",
        ""boundingPolygon"": [
          {
            ""x"": 498,
            ""y"": 1771
          },
          {
            ""x"": 500,
            ""y"": 1789
          },
          {
            ""x"": 490,
            ""y"": 1792
          },
          {
            ""x"": 487,
            ""y"": 1773
          }
        ],
        ""words"": [
          {
            ""text"": ""3/3"",
            ""boundingPolygon"": [
              {
                ""x"": 499,
                ""y"": 1775
              },
              {
                ""x"": 501,
                ""y"": 1790
              },
              {
                ""x"": 490,
                ""y"": 1792
              },
              {
                ""x"": 488,
                ""y"": 1776
              }
            ],
            ""confidence"": 0.99
          }
        ]
      },
      {
        ""text"": ""elle can travel anyu"",
        ""boundingPolygon"": [
          {
            ""x"": 464,
            ""y"": 1208
          },
          {
            ""x"": 466,
            ""y"": 1294
          },
          {
            ""x"": 452,
            ""y"": 1294
          },
          {
            ""x"": 450,
            ""y"": 1208
          }
        ],
        ""words"": [
          {
            ""text"": ""elle"",
            ""boundingPolygon"": [
              {
                ""x"": 464,
                ""y"": 1220
              },
              {
                ""x"": 464,
                ""y"": 1232
              },
              {
                ""x"": 450,
                ""y"": 1233
              },
              {
                ""x"": 450,
                ""y"": 1222
              }
            ],
            ""confidence"": 0.118
          },
          {
            ""text"": ""can"",
            ""boundingPolygon"": [
              {
                ""x"": 464,
                ""y"": 1234
              },
              {
                ""x"": 464,
                ""y"": 1246
              },
              {
                ""x"": 451,
                ""y"": 1248
              },
              {
                ""x"": 450,
                ""y"": 1236
              }
            ],
            ""confidence"": 0.824
          },
          {
            ""text"": ""travel"",
            ""boundingPolygon"": [
              {
                ""x"": 464,
                ""y"": 1249
              },
              {
                ""x"": 465,
                ""y"": 1269
              },
              {
                ""x"": 451,
                ""y"": 1271
              },
              {
                ""x"": 451,
                ""y"": 1250
              }
            ],
            ""confidence"": 0.728
          },
          {
            ""text"": ""anyu"",
            ""boundingPolygon"": [
              {
                ""x"": 465,
                ""y"": 1272
              },
              {
                ""x"": 466,
                ""y"": 1292
              },
              {
                ""x"": 452,
                ""y"": 1293
              },
              {
                ""x"": 451,
                ""y"": 1273
              }
            ],
            ""confidence"": 0.134
          }
        ]
      },
      {
        ""text"": ""imprinted card's card types. (The"",
        ""boundingPolygon"": [
          {
            ""x"": 467,
            ""y"": 1418
          },
          {
            ""x"": 481,
            ""y"": 1535
          },
          {
            ""x"": 467,
            ""y"": 1537
          },
          {
            ""x"": 453,
            ""y"": 1420
          }
        ],
        ""words"": [
          {
            ""text"": ""imprinted"",
            ""boundingPolygon"": [
              {
                ""x"": 468,
                ""y"": 1418
              },
              {
                ""x"": 470,
                ""y"": 1450
              },
              {
                ""x"": 457,
                ""y"": 1453
              },
              {
                ""x"": 454,
                ""y"": 1421
              }
            ],
            ""confidence"": 0.841
          },
          {
            ""text"": ""card's"",
            ""boundingPolygon"": [
              {
                ""x"": 471,
                ""y"": 1453
              },
              {
                ""x"": 473,
                ""y"": 1473
              },
              {
                ""x"": 460,
                ""y"": 1475
              },
              {
                ""x"": 457,
                ""y"": 1455
              }
            ],
            ""confidence"": 0.825
          },
          {
            ""text"": ""card"",
            ""boundingPolygon"": [
              {
                ""x"": 473,
                ""y"": 1475
              },
              {
                ""x"": 475,
                ""y"": 1490
              },
              {
                ""x"": 462,
                ""y"": 1492
              },
              {
                ""x"": 460,
                ""y"": 1477
              }
            ],
            ""confidence"": 0.95
          },
          {
            ""text"": ""types."",
            ""boundingPolygon"": [
              {
                ""x"": 475,
                ""y"": 1493
              },
              {
                ""x"": 477,
                ""y"": 1512
              },
              {
                ""x"": 466,
                ""y"": 1514
              },
              {
                ""x"": 463,
                ""y"": 1495
              }
            ],
            ""confidence"": 0.713
          },
          {
            ""text"": ""(The"",
            ""boundingPolygon"": [
              {
                ""x"": 478,
                ""y"": 1514
              },
              {
                ""x"": 481,
                ""y"": 1534
              },
              {
                ""x"": 470,
                ""y"": 1536
              },
              {
                ""x"": 466,
                ""y"": 1516
              }
            ],
            ""confidence"": 0.882
          }
        ]
      },
      {
        ""text"": ""are armifart, creature,"",
        ""boundingPolygon"": [
          {
            ""x"": 460,
            ""y"": 1420
          },
          {
            ""x"": 466,
            ""y"": 1492
          },
          {
            ""x"": 452,
            ""y"": 1493
          },
          {
            ""x"": 446,
            ""y"": 1421
          }
        ],
        ""words"": [
          {
            ""text"": ""are"",
            ""boundingPolygon"": [
              {
                ""x"": 461,
                ""y"": 1421
              },
              {
                ""x"": 461,
                ""y"": 1430
              },
              {
                ""x"": 447,
                ""y"": 1433
              },
              {
                ""x"": 447,
                ""y"": 1424
              }
            ],
            ""confidence"": 0.649
          },
          {
            ""text"": ""armifart,"",
            ""boundingPolygon"": [
              {
                ""x"": 461,
                ""y"": 1433
              },
              {
                ""x"": 463,
                ""y"": 1458
              },
              {
                ""x"": 450,
                ""y"": 1461
              },
              {
                ""x"": 448,
                ""y"": 1436
              }
            ],
            ""confidence"": 0.59
          },
          {
            ""text"": ""creature,"",
            ""boundingPolygon"": [
              {
                ""x"": 463,
                ""y"": 1461
              },
              {
                ""x"": 466,
                ""y"": 1490
              },
              {
                ""x"": 453,
                ""y"": 1493
              },
              {
                ""x"": 450,
                ""y"": 1464
              }
            ],
            ""confidence"": 0.618
          }
        ]
      },
      {
        ""text"": ""3/4"",
        ""boundingPolygon"": [
          {
            ""x"": 450,
            ""y"": 1531
          },
          {
            ""x"": 454,
            ""y"": 1559
          },
          {
            ""x"": 443,
            ""y"": 1561
          },
          {
            ""x"": 438,
            ""y"": 1534
          }
        ],
        ""words"": [
          {
            ""text"": ""3/4"",
            ""boundingPolygon"": [
              {
                ""x"": 451,
                ""y"": 1539
              },
              {
                ""x"": 454,
                ""y"": 1556
              },
              {
                ""x"": 442,
                ""y"": 1558
              },
              {
                ""x"": 439,
                ""y"": 1541
              }
            ],
            ""confidence"": 0.915
          }
        ]
      },
      {
        ""text"": ""Traveler's Amulet"",
        ""boundingPolygon"": [
          {
            ""x"": 370,
            ""y"": 174
          },
          {
            ""x"": 375,
            ""y"": 260
          },
          {
            ""x"": 363,
            ""y"": 261
          },
          {
            ""x"": 358,
            ""y"": 174
          }
        ],
        ""words"": [
          {
            ""text"": ""Traveler's"",
            ""boundingPolygon"": [
              {
                ""x"": 370,
                ""y"": 174
              },
              {
                ""x"": 371,
                ""y"": 219
              },
              {
                ""x"": 362,
                ""y"": 220
              },
              {
                ""x"": 358,
                ""y"": 174
              }
            ],
            ""confidence"": 0.954
          },
          {
            ""text"": ""Amulet"",
            ""boundingPolygon"": [
              {
                ""x"": 371,
                ""y"": 221
              },
              {
                ""x"": 374,
                ""y"": 256
              },
              {
                ""x"": 364,
                ""y"": 258
              },
              {
                ""x"": 362,
                ""y"": 222
              }
            ],
            ""confidence"": 0.993
          }
        ]
      },
      {
        ""text"": ""Ichor Wellspring"",
        ""boundingPolygon"": [
          {
            ""x"": 380,
            ""y"": 400
          },
          {
            ""x"": 383,
            ""y"": 486
          },
          {
            ""x"": 370,
            ""y"": 486
          },
          {
            ""x"": 366,
            ""y"": 400
          }
        ],
        ""words"": [
          {
            ""text"": ""Ichor"",
            ""boundingPolygon"": [
              {
                ""x"": 380,
                ""y"": 403
              },
              {
                ""x"": 381,
                ""y"": 428
              },
              {
                ""x"": 368,
                ""y"": 428
              },
              {
                ""x"": 366,
                ""y"": 403
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""Wellspring"",
            ""boundingPolygon"": [
              {
                ""x"": 381,
                ""y"": 430
              },
              {
                ""x"": 382,
                ""y"": 482
              },
              {
                ""x"": 371,
                ""y"": 484
              },
              {
                ""x"": 368,
                ""y"": 431
              }
            ],
            ""confidence"": 0.975
          }
        ]
      },
      {
        ""text"": ""Hovermyr"",
        ""boundingPolygon"": [
          {
            ""x"": 405,
            ""y"": 1616
          },
          {
            ""x"": 403,
            ""y"": 1679
          },
          {
            ""x"": 388,
            ""y"": 1678
          },
          {
            ""x"": 390,
            ""y"": 1616
          }
        ],
        ""words"": [
          {
            ""text"": ""Hovermyr"",
            ""boundingPolygon"": [
              {
                ""x"": 401,
                ""y"": 1617
              },
              {
                ""x"": 403,
                ""y"": 1664
              },
              {
                ""x"": 391,
                ""y"": 1665
              },
              {
                ""x"": 390,
                ""y"": 1618
              }
            ],
            ""confidence"": 0.87
          }
        ]
      },
      {
        ""text"": ""Mycosynth Wellspring"",
        ""boundingPolygon"": [
          {
            ""x"": 364,
            ""y"": 644
          },
          {
            ""x"": 373,
            ""y"": 753
          },
          {
            ""x"": 361,
            ""y"": 754
          },
          {
            ""x"": 353,
            ""y"": 644
          }
        ],
        ""words"": [
          {
            ""text"": ""Mycosynth"",
            ""boundingPolygon"": [
              {
                ""x"": 365,
                ""y"": 649
              },
              {
                ""x"": 368,
                ""y"": 697
              },
              {
                ""x"": 358,
                ""y"": 698
              },
              {
                ""x"": 354,
                ""y"": 648
              }
            ],
            ""confidence"": 0.876
          },
          {
            ""text"": ""Wellspring"",
            ""boundingPolygon"": [
              {
                ""x"": 368,
                ""y"": 700
              },
              {
                ""x"": 372,
                ""y"": 750
              },
              {
                ""x"": 362,
                ""y"": 751
              },
              {
                ""x"": 358,
                ""y"": 701
              }
            ],
            ""confidence"": 0.989
          }
        ]
      },
      {
        ""text"": ""nic Cluestone"",
        ""boundingPolygon"": [
          {
            ""x"": 362,
            ""y"": 891
          },
          {
            ""x"": 366,
            ""y"": 956
          },
          {
            ""x"": 352,
            ""y"": 957
          },
          {
            ""x"": 349,
            ""y"": 892
          }
        ],
        ""words"": [
          {
            ""text"": ""nic"",
            ""boundingPolygon"": [
              {
                ""x"": 361,
                ""y"": 892
              },
              {
                ""x"": 361,
                ""y"": 903
              },
              {
                ""x"": 350,
                ""y"": 905
              },
              {
                ""x"": 349,
                ""y"": 893
              }
            ],
            ""confidence"": 0.608
          },
          {
            ""text"": ""Cluestone"",
            ""boundingPolygon"": [
              {
                ""x"": 362,
                ""y"": 906
              },
              {
                ""x"": 366,
                ""y"": 952
              },
              {
                ""x"": 353,
                ""y"": 955
              },
              {
                ""x"": 350,
                ""y"": 908
              }
            ],
            ""confidence"": 0.981
          }
        ]
      },
      {
        ""text"": ""Jack-o'-Lantern"",
        ""boundingPolygon"": [
          {
            ""x"": 363,
            ""y"": 1377
          },
          {
            ""x"": 370,
            ""y"": 1454
          },
          {
            ""x"": 356,
            ""y"": 1455
          },
          {
            ""x"": 349,
            ""y"": 1379
          }
        ],
        ""words"": [
          {
            ""text"": ""Jack-o'-Lantern"",
            ""boundingPolygon"": [
              {
                ""x"": 363,
                ""y"": 1378
              },
              {
                ""x"": 370,
                ""y"": 1449
              },
              {
                ""x"": 356,
                ""y"": 1453
              },
              {
                ""x"": 349,
                ""y"": 1381
              }
            ],
            ""confidence"": 0.726
          }
        ]
      },
      {
        ""text"": ""Baton of Courage"",
        ""boundingPolygon"": [
          {
            ""x"": 334,
            ""y"": 1130
          },
          {
            ""x"": 351,
            ""y"": 1236
          },
          {
            ""x"": 335,
            ""y"": 1239
          },
          {
            ""x"": 319,
            ""y"": 1130
          }
        ],
        ""words"": [
          {
            ""text"": ""Baton"",
            ""boundingPolygon"": [
              {
                ""x"": 335,
                ""y"": 1133
              },
              {
                ""x"": 338,
                ""y"": 1160
              },
              {
                ""x"": 323,
                ""y"": 1161
              },
              {
                ""x"": 320,
                ""y"": 1133
              }
            ],
            ""confidence"": 0.978
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 339,
                ""y"": 1163
              },
              {
                ""x"": 340,
                ""y"": 1172
              },
              {
                ""x"": 325,
                ""y"": 1173
              },
              {
                ""x"": 324,
                ""y"": 1164
              }
            ],
            ""confidence"": 0.997
          },
          {
            ""text"": ""Courage"",
            ""boundingPolygon"": [
              {
                ""x"": 341,
                ""y"": 1175
              },
              {
                ""x"": 348,
                ""y"": 1216
              },
              {
                ""x"": 332,
                ""y"": 1218
              },
              {
                ""x"": 325,
                ""y"": 1176
              }
            ],
            ""confidence"": 0.985
          }
        ]
      },
      {
        ""text"": ""Artifact"",
        ""boundingPolygon"": [
          {
            ""x"": 227,
            ""y"": 181
          },
          {
            ""x"": 229,
            ""y"": 221
          },
          {
            ""x"": 218,
            ""y"": 221
          },
          {
            ""x"": 216,
            ""y"": 181
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 226,
                ""y"": 181
              },
              {
                ""x"": 229,
                ""y"": 212
              },
              {
                ""x"": 218,
                ""y"": 215
              },
              {
                ""x"": 216,
                ""y"": 183
              }
            ],
            ""confidence"": 0.977
          }
        ]
      },
      {
        ""text"": ""tifact"",
        ""boundingPolygon"": [
          {
            ""x"": 235,
            ""y"": 420
          },
          {
            ""x"": 236,
            ""y"": 454
          },
          {
            ""x"": 225,
            ""y"": 454
          },
          {
            ""x"": 224,
            ""y"": 420
          }
        ],
        ""words"": [
          {
            ""text"": ""tifact"",
            ""boundingPolygon"": [
              {
                ""x"": 234,
                ""y"": 421
              },
              {
                ""x"": 235,
                ""y"": 443
              },
              {
                ""x"": 225,
                ""y"": 444
              },
              {
                ""x"": 225,
                ""y"": 422
              }
            ],
            ""confidence"": 0.843
          }
        ]
      },
      {
        ""text"": ""1, Sacrifice /Traveler's Amulet; Search"",
        ""boundingPolygon"": [
          {
            ""x"": 201,
            ""y"": 182
          },
          {
            ""x"": 220,
            ""y"": 334
          },
          {
            ""x"": 204,
            ""y"": 337
          },
          {
            ""x"": 187,
            ""y"": 184
          }
        ],
        ""words"": [
          {
            ""text"": ""1,"",
            ""boundingPolygon"": [
              {
                ""x"": 201,
                ""y"": 184
              },
              {
                ""x"": 202,
                ""y"": 192
              },
              {
                ""x"": 188,
                ""y"": 194
              },
              {
                ""x"": 187,
                ""y"": 186
              }
            ],
            ""confidence"": 0.648
          },
          {
            ""text"": ""Sacrifice"",
            ""boundingPolygon"": [
              {
                ""x"": 202,
                ""y"": 195
              },
              {
                ""x"": 204,
                ""y"": 229
              },
              {
                ""x"": 191,
                ""y"": 231
              },
              {
                ""x"": 188,
                ""y"": 197
              }
            ],
            ""confidence"": 0.441
          },
          {
            ""text"": ""/Traveler's"",
            ""boundingPolygon"": [
              {
                ""x"": 204,
                ""y"": 232
              },
              {
                ""x"": 209,
                ""y"": 270
              },
              {
                ""x"": 196,
                ""y"": 272
              },
              {
                ""x"": 191,
                ""y"": 234
              }
            ],
            ""confidence"": 0.493
          },
          {
            ""text"": ""Amulet;"",
            ""boundingPolygon"": [
              {
                ""x"": 209,
                ""y"": 272
              },
              {
                ""x"": 214,
                ""y"": 305
              },
              {
                ""x"": 202,
                ""y"": 308
              },
              {
                ""x"": 197,
                ""y"": 275
              }
            ],
            ""confidence"": 0.73
          },
          {
            ""text"": ""Search"",
            ""boundingPolygon"": [
              {
                ""x"": 215,
                ""y"": 308
              },
              {
                ""x"": 220,
                ""y"": 334
              },
              {
                ""x"": 208,
                ""y"": 337
              },
              {
                ""x"": 203,
                ""y"": 310
              }
            ],
            ""confidence"": 0.993
          }
        ]
      },
      {
        ""text"": ""nters the"",
        ""boundingPolygon"": [
          {
            ""x"": 218,
            ""y"": 520
          },
          {
            ""x"": 225,
            ""y"": 563
          },
          {
            ""x"": 213,
            ""y"": 565
          },
          {
            ""x"": 206,
            ""y"": 522
          }
        ],
        ""words"": [
          {
            ""text"": ""nters"",
            ""boundingPolygon"": [
              {
                ""x"": 219,
                ""y"": 520
              },
              {
                ""x"": 222,
                ""y"": 542
              },
              {
                ""x"": 210,
                ""y"": 543
              },
              {
                ""x"": 207,
                ""y"": 522
              }
            ],
            ""confidence"": 0.992
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 222,
                ""y"": 544
              },
              {
                ""x"": 225,
                ""y"": 559
              },
              {
                ""x"": 212,
                ""y"": 560
              },
              {
                ""x"": 210,
                ""y"": 546
              }
            ],
            ""confidence"": 0.998
          }
        ]
      },
      {
        ""text"": ""When Lehor webupunto a graveyard"",
        ""boundingPolygon"": [
          {
            ""x"": 209,
            ""y"": 414
          },
          {
            ""x"": 214,
            ""y"": 567
          },
          {
            ""x"": 200,
            ""y"": 567
          },
          {
            ""x"": 195,
            ""y"": 414
          }
        ],
        ""words"": [
          {
            ""text"": ""When"",
            ""boundingPolygon"": [
              {
                ""x"": 210,
                ""y"": 414
              },
              {
                ""x"": 210,
                ""y"": 438
              },
              {
                ""x"": 196,
                ""y"": 441
              },
              {
                ""x"": 195,
                ""y"": 417
              }
            ],
            ""confidence"": 0.881
          },
          {
            ""text"": ""Lehor"",
            ""boundingPolygon"": [
              {
                ""x"": 210,
                ""y"": 441
              },
              {
                ""x"": 211,
                ""y"": 464
              },
              {
                ""x"": 197,
                ""y"": 467
              },
              {
                ""x"": 196,
                ""y"": 444
              }
            ],
            ""confidence"": 0.226
          },
          {
            ""text"": ""webupunto"",
            ""boundingPolygon"": [
              {
                ""x"": 211,
                ""y"": 467
              },
              {
                ""x"": 212,
                ""y"": 515
              },
              {
                ""x"": 199,
                ""y"": 517
              },
              {
                ""x"": 197,
                ""y"": 469
              }
            ],
            ""confidence"": 0.128
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 213,
                ""y"": 517
              },
              {
                ""x"": 213,
                ""y"": 522
              },
              {
                ""x"": 199,
                ""y"": 525
              },
              {
                ""x"": 199,
                ""y"": 520
              }
            ],
            ""confidence"": 0.356
          },
          {
            ""text"": ""graveyard"",
            ""boundingPolygon"": [
              {
                ""x"": 213,
                ""y"": 525
              },
              {
                ""x"": 214,
                ""y"": 565
              },
              {
                ""x"": 201,
                ""y"": 568
              },
              {
                ""x"": 199,
                ""y"": 527
              }
            ],
            ""confidence"": 0.599
          }
        ]
      },
      {
        ""text"": ""(Artifact"",
        ""boundingPolygon"": [
          {
            ""x"": 224,
            ""y"": 656
          },
          {
            ""x"": 230,
            ""y"": 719
          },
          {
            ""x"": 219,
            ""y"": 720
          },
          {
            ""x"": 214,
            ""y"": 657
          }
        ],
        ""words"": [
          {
            ""text"": ""(Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 223,
                ""y"": 657
              },
              {
                ""x"": 227,
                ""y"": 693
              },
              {
                ""x"": 217,
                ""y"": 695
              },
              {
                ""x"": 214,
                ""y"": 658
              }
            ],
            ""confidence"": 0.568
          }
        ]
      },
      {
        ""text"": ""Artifact Creature-Myr"",
        ""boundingPolygon"": [
          {
            ""x"": 259,
            ""y"": 1637
          },
          {
            ""x"": 271,
            ""y"": 1736
          },
          {
            ""x"": 259,
            ""y"": 1737
          },
          {
            ""x"": 248,
            ""y"": 1638
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 259,
                ""y"": 1638
              },
              {
                ""x"": 263,
                ""y"": 1666
              },
              {
                ""x"": 252,
                ""y"": 1669
              },
              {
                ""x"": 248,
                ""y"": 1640
              }
            ],
            ""confidence"": 0.975
          },
          {
            ""text"": ""Creature-Myr"",
            ""boundingPolygon"": [
              {
                ""x"": 263,
                ""y"": 1668
              },
              {
                ""x"": 270,
                ""y"": 1728
              },
              {
                ""x"": 259,
                ""y"": 1732
              },
              {
                ""x"": 252,
                ""y"": 1671
              }
            ],
            ""confidence"": 0.803
          }
        ]
      },
      {
        ""text"": ""your library for a b"",
        ""boundingPolygon"": [
          {
            ""x"": 196,
            ""y"": 182
          },
          {
            ""x"": 201,
            ""y"": 259
          },
          {
            ""x"": 186,
            ""y"": 260
          },
          {
            ""x"": 181,
            ""y"": 183
          }
        ],
        ""words"": [
          {
            ""text"": ""your"",
            ""boundingPolygon"": [
              {
                ""x"": 196,
                ""y"": 182
              },
              {
                ""x"": 198,
                ""y"": 199
              },
              {
                ""x"": 182,
                ""y"": 201
              },
              {
                ""x"": 181,
                ""y"": 184
              }
            ],
            ""confidence"": 0.99
          },
          {
            ""text"": ""library"",
            ""boundingPolygon"": [
              {
                ""x"": 198,
                ""y"": 202
              },
              {
                ""x"": 200,
                ""y"": 229
              },
              {
                ""x"": 185,
                ""y"": 230
              },
              {
                ""x"": 183,
                ""y"": 204
              }
            ],
            ""confidence"": 0.977
          },
          {
            ""text"": ""for"",
            ""boundingPolygon"": [
              {
                ""x"": 200,
                ""y"": 232
              },
              {
                ""x"": 201,
                ""y"": 243
              },
              {
                ""x"": 186,
                ""y"": 244
              },
              {
                ""x"": 185,
                ""y"": 233
              }
            ],
            ""confidence"": 0.995
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 201,
                ""y"": 246
              },
              {
                ""x"": 201,
                ""y"": 250
              },
              {
                ""x"": 186,
                ""y"": 251
              },
              {
                ""x"": 186,
                ""y"": 247
              }
            ],
            ""confidence"": 0.735
          },
          {
            ""text"": ""b"",
            ""boundingPolygon"": [
              {
                ""x"": 201,
                ""y"": 253
              },
              {
                ""x"": 202,
                ""y"": 259
              },
              {
                ""x"": 187,
                ""y"": 260
              },
              {
                ""x"": 186,
                ""y"": 254
              }
            ],
            ""confidence"": 0.799
          }
        ]
      },
      {
        ""text"": ""put it into your hand,"",
        ""boundingPolygon"": [
          {
            ""x"": 192,
            ""y"": 238
          },
          {
            ""x"": 201,
            ""y"": 329
          },
          {
            ""x"": 188,
            ""y"": 330
          },
          {
            ""x"": 179,
            ""y"": 240
          }
        ],
        ""words"": [
          {
            ""text"": ""put"",
            ""boundingPolygon"": [
              {
                ""x"": 193,
                ""y"": 239
              },
              {
                ""x"": 193,
                ""y"": 250
              },
              {
                ""x"": 181,
                ""y"": 252
              },
              {
                ""x"": 180,
                ""y"": 241
              }
            ],
            ""confidence"": 0.764
          },
          {
            ""text"": ""it"",
            ""boundingPolygon"": [
              {
                ""x"": 194,
                ""y"": 253
              },
              {
                ""x"": 194,
                ""y"": 258
              },
              {
                ""x"": 182,
                ""y"": 261
              },
              {
                ""x"": 181,
                ""y"": 255
              }
            ],
            ""confidence"": 0.986
          },
          {
            ""text"": ""into"",
            ""boundingPolygon"": [
              {
                ""x"": 194,
                ""y"": 261
              },
              {
                ""x"": 196,
                ""y"": 277
              },
              {
                ""x"": 184,
                ""y"": 279
              },
              {
                ""x"": 182,
                ""y"": 263
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""your"",
            ""boundingPolygon"": [
              {
                ""x"": 196,
                ""y"": 280
              },
              {
                ""x"": 198,
                ""y"": 298
              },
              {
                ""x"": 186,
                ""y"": 300
              },
              {
                ""x"": 184,
                ""y"": 282
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""hand,"",
            ""boundingPolygon"": [
              {
                ""x"": 198,
                ""y"": 300
              },
              {
                ""x"": 201,
                ""y"": 327
              },
              {
                ""x"": 189,
                ""y"": 329
              },
              {
                ""x"": 186,
                ""y"": 302
              }
            ],
            ""confidence"": 0.673
          }
        ]
      },
      {
        ""text"": ""Then shuffle your library."",
        ""boundingPolygon"": [
          {
            ""x"": 181,
            ""y"": 184
          },
          {
            ""x"": 185,
            ""y"": 293
          },
          {
            ""x"": 171,
            ""y"": 294
          },
          {
            ""x"": 165,
            ""y"": 185
          }
        ],
        ""words"": [
          {
            ""text"": ""Then"",
            ""boundingPolygon"": [
              {
                ""x"": 181,
                ""y"": 184
              },
              {
                ""x"": 182,
                ""y"": 204
              },
              {
                ""x"": 168,
                ""y"": 205
              },
              {
                ""x"": 166,
                ""y"": 186
              }
            ],
            ""confidence"": 0.957
          },
          {
            ""text"": ""shuffle"",
            ""boundingPolygon"": [
              {
                ""x"": 182,
                ""y"": 206
              },
              {
                ""x"": 183,
                ""y"": 234
              },
              {
                ""x"": 170,
                ""y"": 235
              },
              {
                ""x"": 168,
                ""y"": 208
              }
            ],
            ""confidence"": 0.574
          },
          {
            ""text"": ""your"",
            ""boundingPolygon"": [
              {
                ""x"": 184,
                ""y"": 237
              },
              {
                ""x"": 184,
                ""y"": 255
              },
              {
                ""x"": 172,
                ""y"": 256
              },
              {
                ""x"": 170,
                ""y"": 238
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""library."",
            ""boundingPolygon"": [
              {
                ""x"": 184,
                ""y"": 258
              },
              {
                ""x"": 185,
                ""y"": 291
              },
              {
                ""x"": 173,
                ""y"": 291
              },
              {
                ""x"": 172,
                ""y"": 259
              }
            ],
            ""confidence"": 0.925
          }
        ]
      },
      {
        ""text"": ""n Mycosynth Wcaspring cht"",
        ""boundingPolygon"": [
          {
            ""x"": 204,
            ""y"": 679
          },
          {
            ""x"": 214,
            ""y"": 819
          },
          {
            ""x"": 203,
            ""y"": 820
          },
          {
            ""x"": 194,
            ""y"": 679
          }
        ],
        ""words"": [
          {
            ""text"": ""n"",
            ""boundingPolygon"": [
              {
                ""x"": 203,
                ""y"": 680
              },
              {
                ""x"": 203,
                ""y"": 685
              },
              {
                ""x"": 195,
                ""y"": 686
              },
              {
                ""x"": 195,
                ""y"": 682
              }
            ],
            ""confidence"": 0.201
          },
          {
            ""text"": ""Mycosynth"",
            ""boundingPolygon"": [
              {
                ""x"": 203,
                ""y"": 687
              },
              {
                ""x"": 205,
                ""y"": 728
              },
              {
                ""x"": 197,
                ""y"": 729
              },
              {
                ""x"": 195,
                ""y"": 689
              }
            ],
            ""confidence"": 0.855
          },
          {
            ""text"": ""Wcaspring"",
            ""boundingPolygon"": [
              {
                ""x"": 205,
                ""y"": 730
              },
              {
                ""x"": 208,
                ""y"": 767
              },
              {
                ""x"": 199,
                ""y"": 769
              },
              {
                ""x"": 197,
                ""y"": 731
              }
            ],
            ""confidence"": 0.464
          },
          {
            ""text"": ""cht"",
            ""boundingPolygon"": [
              {
                ""x"": 209,
                ""y"": 769
              },
              {
                ""x"": 210,
                ""y"": 783
              },
              {
                ""x"": 200,
                ""y"": 784
              },
              {
                ""x"": 199,
                ""y"": 771
              }
            ],
            ""confidence"": 0.118
          }
        ]
      },
      {
        ""text"": ""Then Moh put into a graveyard tror"",
        ""boundingPolygon"": [
          {
            ""x"": 200,
            ""y"": 671
          },
          {
            ""x"": 206,
            ""y"": 820
          },
          {
            ""x"": 193,
            ""y"": 821
          },
          {
            ""x"": 189,
            ""y"": 672
          }
        ],
        ""words"": [
          {
            ""text"": ""Then"",
            ""boundingPolygon"": [
              {
                ""x"": 199,
                ""y"": 672
              },
              {
                ""x"": 200,
                ""y"": 685
              },
              {
                ""x"": 190,
                ""y"": 686
              },
              {
                ""x"": 189,
                ""y"": 673
              }
            ],
            ""confidence"": 0.291
          },
          {
            ""text"": ""Moh"",
            ""boundingPolygon"": [
              {
                ""x"": 200,
                ""y"": 687
              },
              {
                ""x"": 201,
                ""y"": 718
              },
              {
                ""x"": 191,
                ""y"": 719
              },
              {
                ""x"": 190,
                ""y"": 688
              }
            ],
            ""confidence"": 0.119
          },
          {
            ""text"": ""put"",
            ""boundingPolygon"": [
              {
                ""x"": 202,
                ""y"": 721
              },
              {
                ""x"": 202,
                ""y"": 733
              },
              {
                ""x"": 191,
                ""y"": 734
              },
              {
                ""x"": 191,
                ""y"": 721
              }
            ],
            ""confidence"": 0.125
          },
          {
            ""text"": ""into"",
            ""boundingPolygon"": [
              {
                ""x"": 202,
                ""y"": 735
              },
              {
                ""x"": 203,
                ""y"": 750
              },
              {
                ""x"": 192,
                ""y"": 751
              },
              {
                ""x"": 191,
                ""y"": 736
              }
            ],
            ""confidence"": 0.669
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 203,
                ""y"": 752
              },
              {
                ""x"": 203,
                ""y"": 757
              },
              {
                ""x"": 192,
                ""y"": 757
              },
              {
                ""x"": 192,
                ""y"": 753
              }
            ],
            ""confidence"": 0.313
          },
          {
            ""text"": ""graveyard"",
            ""boundingPolygon"": [
              {
                ""x"": 203,
                ""y"": 759
              },
              {
                ""x"": 205,
                ""y"": 794
              },
              {
                ""x"": 193,
                ""y"": 794
              },
              {
                ""x"": 192,
                ""y"": 759
              }
            ],
            ""confidence"": 0.475
          },
          {
            ""text"": ""tror"",
            ""boundingPolygon"": [
              {
                ""x"": 205,
                ""y"": 796
              },
              {
                ""x"": 206,
                ""y"": 811
              },
              {
                ""x"": 193,
                ""y"": 811
              },
              {
                ""x"": 193,
                ""y"": 796
              }
            ],
            ""confidence"": 0.195
          }
        ]
      },
      {
        ""text"": ""Artifact"",
        ""boundingPolygon"": [
          {
            ""x"": 214,
            ""y"": 899
          },
          {
            ""x"": 221,
            ""y"": 941
          },
          {
            ""x"": 209,
            ""y"": 943
          },
          {
            ""x"": 202,
            ""y"": 901
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 214,
                ""y"": 902
              },
              {
                ""x"": 220,
                ""y"": 935
              },
              {
                ""x"": 209,
                ""y"": 938
              },
              {
                ""x"": 203,
                ""y"": 906
              }
            ],
            ""confidence"": 0.95
          }
        ]
      },
      {
        ""text"": ""Then shut"",
        ""boundingPolygon"": [
          {
            ""x"": 174,
            ""y"": 185
          },
          {
            ""x"": 179,
            ""y"": 225
          },
          {
            ""x"": 165,
            ""y"": 226
          },
          {
            ""x"": 159,
            ""y"": 187
          }
        ],
        ""words"": [
          {
            ""text"": ""Then"",
            ""boundingPolygon"": [
              {
                ""x"": 174,
                ""y"": 185
              },
              {
                ""x"": 177,
                ""y"": 205
              },
              {
                ""x"": 162,
                ""y"": 207
              },
              {
                ""x"": 159,
                ""y"": 187
              }
            ],
            ""confidence"": 0.977
          },
          {
            ""text"": ""shut"",
            ""boundingPolygon"": [
              {
                ""x"": 177,
                ""y"": 208
              },
              {
                ""x"": 180,
                ""y"": 224
              },
              {
                ""x"": 165,
                ""y"": 227
              },
              {
                ""x"": 162,
                ""y"": 210
              }
            ],
            ""confidence"": 0.343
          }
        ]
      },
      {
        ""text"": ""Tinto the cerie mist,"",
        ""boundingPolygon"": [
          {
            ""x"": 170,
            ""y"": 237
          },
          {
            ""x"": 173,
            ""y"": 317
          },
          {
            ""x"": 160,
            ""y"": 317
          },
          {
            ""x"": 158,
            ""y"": 238
          }
        ],
        ""words"": [
          {
            ""text"": ""Tinto"",
            ""boundingPolygon"": [
              {
                ""x"": 171,
                ""y"": 238
              },
              {
                ""x"": 171,
                ""y"": 255
              },
              {
                ""x"": 158,
                ""y"": 257
              },
              {
                ""x"": 158,
                ""y"": 240
              }
            ],
            ""confidence"": 0.281
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 171,
                ""y"": 258
              },
              {
                ""x"": 171,
                ""y"": 270
              },
              {
                ""x"": 159,
                ""y"": 272
              },
              {
                ""x"": 159,
                ""y"": 260
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""cerie"",
            ""boundingPolygon"": [
              {
                ""x"": 171,
                ""y"": 272
              },
              {
                ""x"": 172,
                ""y"": 288
              },
              {
                ""x"": 160,
                ""y"": 289
              },
              {
                ""x"": 159,
                ""y"": 274
              }
            ],
            ""confidence"": 0.605
          },
          {
            ""text"": ""mist,"",
            ""boundingPolygon"": [
              {
                ""x"": 172,
                ""y"": 290
              },
              {
                ""x"": 173,
                ""y"": 315
              },
              {
                ""x"": 161,
                ""y"": 316
              },
              {
                ""x"": 160,
                ""y"": 292
              }
            ],
            ""confidence"": 0.704
          }
        ]
      },
      {
        ""text"": ""the battlefiel"",
        ""boundingPolygon"": [
          {
            ""x"": 187,
            ""y"": 437
          },
          {
            ""x"": 193,
            ""y"": 490
          },
          {
            ""x"": 180,
            ""y"": 492
          },
          {
            ""x"": 174,
            ""y"": 438
          }
        ],
        ""words"": [
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 187,
                ""y"": 438
              },
              {
                ""x"": 189,
                ""y"": 451
              },
              {
                ""x"": 176,
                ""y"": 452
              },
              {
                ""x"": 174,
                ""y"": 439
              }
            ],
            ""confidence"": 0.992
          },
          {
            ""text"": ""battlefiel"",
            ""boundingPolygon"": [
              {
                ""x"": 189,
                ""y"": 453
              },
              {
                ""x"": 194,
                ""y"": 491
              },
              {
                ""x"": 181,
                ""y"": 492
              },
              {
                ""x"": 176,
                ""y"": 455
              }
            ],
            ""confidence"": 0.58
          }
        ]
      },
      {
        ""text"": ""atlebeid et \"" P may search your library"",
        ""boundingPolygon"": [
          {
            ""x"": 195,
            ""y"": 670
          },
          {
            ""x"": 200,
            ""y"": 820
          },
          {
            ""x"": 187,
            ""y"": 820
          },
          {
            ""x"": 181,
            ""y"": 671
          }
        ],
        ""words"": [
          {
            ""text"": ""atlebeid"",
            ""boundingPolygon"": [
              {
                ""x"": 194,
                ""y"": 671
              },
              {
                ""x"": 195,
                ""y"": 700
              },
              {
                ""x"": 183,
                ""y"": 701
              },
              {
                ""x"": 182,
                ""y"": 672
              }
            ],
            ""confidence"": 0.229
          },
          {
            ""text"": ""et"",
            ""boundingPolygon"": [
              {
                ""x"": 195,
                ""y"": 703
              },
              {
                ""x"": 195,
                ""y"": 710
              },
              {
                ""x"": 183,
                ""y"": 711
              },
              {
                ""x"": 183,
                ""y"": 703
              }
            ],
            ""confidence"": 0.131
          },
          {
            ""text"": ""\"""",
            ""boundingPolygon"": [
              {
                ""x"": 195,
                ""y"": 712
              },
              {
                ""x"": 196,
                ""y"": 718
              },
              {
                ""x"": 183,
                ""y"": 719
              },
              {
                ""x"": 183,
                ""y"": 713
              }
            ],
            ""confidence"": 0.118
          },
          {
            ""text"": ""P"",
            ""boundingPolygon"": [
              {
                ""x"": 196,
                ""y"": 721
              },
              {
                ""x"": 196,
                ""y"": 727
              },
              {
                ""x"": 184,
                ""y"": 728
              },
              {
                ""x"": 184,
                ""y"": 722
              }
            ],
            ""confidence"": 0.12
          },
          {
            ""text"": ""may"",
            ""boundingPolygon"": [
              {
                ""x"": 196,
                ""y"": 734
              },
              {
                ""x"": 197,
                ""y"": 749
              },
              {
                ""x"": 185,
                ""y"": 750
              },
              {
                ""x"": 184,
                ""y"": 735
              }
            ],
            ""confidence"": 0.123
          },
          {
            ""text"": ""search"",
            ""boundingPolygon"": [
              {
                ""x"": 197,
                ""y"": 752
              },
              {
                ""x"": 198,
                ""y"": 774
              },
              {
                ""x"": 186,
                ""y"": 775
              },
              {
                ""x"": 185,
                ""y"": 752
              }
            ],
            ""confidence"": 0.834
          },
          {
            ""text"": ""your"",
            ""boundingPolygon"": [
              {
                ""x"": 198,
                ""y"": 776
              },
              {
                ""x"": 199,
                ""y"": 792
              },
              {
                ""x"": 186,
                ""y"": 793
              },
              {
                ""x"": 186,
                ""y"": 777
              }
            ],
            ""confidence"": 0.989
          },
          {
            ""text"": ""library"",
            ""boundingPolygon"": [
              {
                ""x"": 199,
                ""y"": 795
              },
              {
                ""x"": 201,
                ""y"": 820
              },
              {
                ""x"": 187,
                ""y"": 820
              },
              {
                ""x"": 186,
                ""y"": 796
              }
            ],
            ""confidence"": 0.841
          }
        ]
      },
      {
        ""text"": ""c: Add Q or ."",
        ""boundingPolygon"": [
          {
            ""x"": 203,
            ""y"": 903
          },
          {
            ""x"": 210,
            ""y"": 969
          },
          {
            ""x"": 196,
            ""y"": 971
          },
          {
            ""x"": 189,
            ""y"": 904
          }
        ],
        ""words"": [
          {
            ""text"": ""c:"",
            ""boundingPolygon"": [
              {
                ""x"": 203,
                ""y"": 907
              },
              {
                ""x"": 204,
                ""y"": 916
              },
              {
                ""x"": 191,
                ""y"": 919
              },
              {
                ""x"": 190,
                ""y"": 910
              }
            ],
            ""confidence"": 0.129
          },
          {
            ""text"": ""Add"",
            ""boundingPolygon"": [
              {
                ""x"": 205,
                ""y"": 918
              },
              {
                ""x"": 207,
                ""y"": 936
              },
              {
                ""x"": 194,
                ""y"": 940
              },
              {
                ""x"": 192,
                ""y"": 922
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""Q"",
            ""boundingPolygon"": [
              {
                ""x"": 207,
                ""y"": 939
              },
              {
                ""x"": 208,
                ""y"": 945
              },
              {
                ""x"": 195,
                ""y"": 949
              },
              {
                ""x"": 194,
                ""y"": 943
              }
            ],
            ""confidence"": 0.125
          },
          {
            ""text"": ""or"",
            ""boundingPolygon"": [
              {
                ""x"": 208,
                ""y"": 948
              },
              {
                ""x"": 210,
                ""y"": 958
              },
              {
                ""x"": 196,
                ""y"": 962
              },
              {
                ""x"": 195,
                ""y"": 952
              }
            ],
            ""confidence"": 0.879
          },
          {
            ""text"": ""."",
            ""boundingPolygon"": [
              {
                ""x"": 210,
                ""y"": 960
              },
              {
                ""x"": 210,
                ""y"": 966
              },
              {
                ""x"": 197,
                ""y"": 971
              },
              {
                ""x"": 196,
                ""y"": 965
              }
            ],
            ""confidence"": 0.18
          }
        ]
      },
      {
        ""text"": ""Artifact"",
        ""boundingPolygon"": [
          {
            ""x"": 221,
            ""y"": 1399
          },
          {
            ""x"": 223,
            ""y"": 1444
          },
          {
            ""x"": 211,
            ""y"": 1444
          },
          {
            ""x"": 209,
            ""y"": 1399
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 221,
                ""y"": 1402
              },
              {
                ""x"": 223,
                ""y"": 1433
              },
              {
                ""x"": 211,
                ""y"": 1436
              },
              {
                ""x"": 209,
                ""y"": 1404
              }
            ],
            ""confidence"": 0.937
          }
        ]
      },
      {
        ""text"": ""Our glorious igecrian"",
        ""boundingPolygon"": [
          {
            ""x"": 175,
            ""y"": 419
          },
          {
            ""x"": 180,
            ""y"": 507
          },
          {
            ""x"": 162,
            ""y"": 508
          },
          {
            ""x"": 157,
            ""y"": 420
          }
        ],
        ""words"": [
          {
            ""text"": ""Our"",
            ""boundingPolygon"": [
              {
                ""x"": 175,
                ""y"": 419
              },
              {
                ""x"": 176,
                ""y"": 435
              },
              {
                ""x"": 158,
                ""y"": 436
              },
              {
                ""x"": 157,
                ""y"": 420
              }
            ],
            ""confidence"": 0.958
          },
          {
            ""text"": ""glorious"",
            ""boundingPolygon"": [
              {
                ""x"": 176,
                ""y"": 438
              },
              {
                ""x"": 177,
                ""y"": 468
              },
              {
                ""x"": 160,
                ""y"": 469
              },
              {
                ""x"": 158,
                ""y"": 439
              }
            ],
            ""confidence"": 0.573
          },
          {
            ""text"": ""igecrian"",
            ""boundingPolygon"": [
              {
                ""x"": 177,
                ""y"": 472
              },
              {
                ""x"": 179,
                ""y"": 507
              },
              {
                ""x"": 164,
                ""y"": 508
              },
              {
                ""x"": 161,
                ""y"": 473
              }
            ],
            ""confidence"": 0.284
          }
        ]
      },
      {
        ""text"": ""Lantern: Esi"",
        ""boundingPolygon"": [
          {
            ""x"": 219,
            ""y"": 1498
          },
          {
            ""x"": 231,
            ""y"": 1549
          },
          {
            ""x"": 218,
            ""y"": 1552
          },
          {
            ""x"": 207,
            ""y"": 1501
          }
        ],
        ""words"": [
          {
            ""text"": ""Lantern:"",
            ""boundingPolygon"": [
              {
                ""x"": 219,
                ""y"": 1499
              },
              {
                ""x"": 226,
                ""y"": 1528
              },
              {
                ""x"": 215,
                ""y"": 1534
              },
              {
                ""x"": 208,
                ""y"": 1504
              }
            ],
            ""confidence"": 0.447
          },
          {
            ""text"": ""Esi"",
            ""boundingPolygon"": [
              {
                ""x"": 227,
                ""y"": 1531
              },
              {
                ""x"": 231,
                ""y"": 1547
              },
              {
                ""x"": 220,
                ""y"": 1552
              },
              {
                ""x"": 216,
                ""y"": 1536
              }
            ],
            ""confidence"": 0.126
          }
        ]
      },
      {
        ""text"": ""lying, vigilance"",
        ""boundingPolygon"": [
          {
            ""x"": 231,
            ""y"": 1646
          },
          {
            ""x"": 232,
            ""y"": 1767
          },
          {
            ""x"": 213,
            ""y"": 1767
          },
          {
            ""x"": 213,
            ""y"": 1646
          }
        ],
        ""words"": [
          {
            ""text"": ""lying,"",
            ""boundingPolygon"": [
              {
                ""x"": 225,
                ""y"": 1647
              },
              {
                ""x"": 229,
                ""y"": 1670
              },
              {
                ""x"": 217,
                ""y"": 1673
              },
              {
                ""x"": 215,
                ""y"": 1651
              }
            ],
            ""confidence"": 0.674
          },
          {
            ""text"": ""vigilance"",
            ""boundingPolygon"": [
              {
                ""x"": 229,
                ""y"": 1673
              },
              {
                ""x"": 232,
                ""y"": 1714
              },
              {
                ""x"": 218,
                ""y"": 1716
              },
              {
                ""x"": 217,
                ""y"": 1676
              }
            ],
            ""confidence"": 0.477
          }
        ]
      },
      {
        ""text"": ""rn, Grana Cenatake o"",
        ""boundingPolygon"": [
          {
            ""x"": 167,
            ""y"": 460
          },
          {
            ""x"": 187,
            ""y"": 559
          },
          {
            ""x"": 174,
            ""y"": 561
          },
          {
            ""x"": 155,
            ""y"": 461
          }
        ],
        ""words"": [
          {
            ""text"": ""rn,"",
            ""boundingPolygon"": [
              {
                ""x"": 167,
                ""y"": 460
              },
              {
                ""x"": 170,
                ""y"": 473
              },
              {
                ""x"": 158,
                ""y"": 474
              },
              {
                ""x"": 156,
                ""y"": 461
              }
            ],
            ""confidence"": 0.98
          },
          {
            ""text"": ""Grana"",
            ""boundingPolygon"": [
              {
                ""x"": 170,
                ""y"": 475
              },
              {
                ""x"": 175,
                ""y"": 501
              },
              {
                ""x"": 162,
                ""y"": 502
              },
              {
                ""x"": 158,
                ""y"": 476
              }
            ],
            ""confidence"": 0.934
          },
          {
            ""text"": ""Cenatake"",
            ""boundingPolygon"": [
              {
                ""x"": 176,
                ""y"": 504
              },
              {
                ""x"": 184,
                ""y"": 544
              },
              {
                ""x"": 171,
                ""y"": 545
              },
              {
                ""x"": 163,
                ""y"": 505
              }
            ],
            ""confidence"": 0.235
          },
          {
            ""text"": ""o"",
            ""boundingPolygon"": [
              {
                ""x"": 186,
                ""y"": 554
              },
              {
                ""x"": 187,
                ""y"": 559
              },
              {
                ""x"": 174,
                ""y"": 560
              },
              {
                ""x"": 173,
                ""y"": 555
              }
            ],
            ""confidence"": 0.444
          }
        ]
      },
      {
        ""text"": ""a basic land card, reveal it, put it"",
        ""boundingPolygon"": [
          {
            ""x"": 182,
            ""y"": 675
          },
          {
            ""x"": 194,
            ""y"": 800
          },
          {
            ""x"": 179,
            ""y"": 801
          },
          {
            ""x"": 167,
            ""y"": 677
          }
        ],
        ""words"": [
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 183,
                ""y"": 677
              },
              {
                ""x"": 183,
                ""y"": 681
              },
              {
                ""x"": 168,
                ""y"": 682
              },
              {
                ""x"": 168,
                ""y"": 679
              }
            ],
            ""confidence"": 0.165
          },
          {
            ""text"": ""basic"",
            ""boundingPolygon"": [
              {
                ""x"": 183,
                ""y"": 683
              },
              {
                ""x"": 184,
                ""y"": 701
              },
              {
                ""x"": 171,
                ""y"": 703
              },
              {
                ""x"": 168,
                ""y"": 685
              }
            ],
            ""confidence"": 0.601
          },
          {
            ""text"": ""land"",
            ""boundingPolygon"": [
              {
                ""x"": 185,
                ""y"": 704
              },
              {
                ""x"": 186,
                ""y"": 721
              },
              {
                ""x"": 173,
                ""y"": 723
              },
              {
                ""x"": 171,
                ""y"": 706
              }
            ],
            ""confidence"": 0.953
          },
          {
            ""text"": ""card,"",
            ""boundingPolygon"": [
              {
                ""x"": 186,
                ""y"": 723
              },
              {
                ""x"": 187,
                ""y"": 741
              },
              {
                ""x"": 175,
                ""y"": 743
              },
              {
                ""x"": 173,
                ""y"": 725
              }
            ],
            ""confidence"": 0.922
          },
          {
            ""text"": ""reveal"",
            ""boundingPolygon"": [
              {
                ""x"": 188,
                ""y"": 743
              },
              {
                ""x"": 189,
                ""y"": 763
              },
              {
                ""x"": 178,
                ""y"": 765
              },
              {
                ""x"": 176,
                ""y"": 745
              }
            ],
            ""confidence"": 0.86
          },
          {
            ""text"": ""it,"",
            ""boundingPolygon"": [
              {
                ""x"": 190,
                ""y"": 766
              },
              {
                ""x"": 190,
                ""y"": 773
              },
              {
                ""x"": 179,
                ""y"": 775
              },
              {
                ""x"": 178,
                ""y"": 767
              }
            ],
            ""confidence"": 0.961
          },
          {
            ""text"": ""put"",
            ""boundingPolygon"": [
              {
                ""x"": 191,
                ""y"": 776
              },
              {
                ""x"": 192,
                ""y"": 788
              },
              {
                ""x"": 180,
                ""y"": 790
              },
              {
                ""x"": 179,
                ""y"": 777
              }
            ],
            ""confidence"": 0.944
          },
          {
            ""text"": ""it"",
            ""boundingPolygon"": [
              {
                ""x"": 192,
                ""y"": 791
              },
              {
                ""x"": 193,
                ""y"": 799
              },
              {
                ""x"": 181,
                ""y"": 801
              },
              {
                ""x"": 181,
                ""y"": 792
              }
            ],
            ""confidence"": 0.642
          }
        ]
      },
      {
        ""text"": ""1, e, Sacrifice Jack-o -Lantern; Exile"",
        ""boundingPolygon"": [
          {
            ""x"": 203,
            ""y"": 1404
          },
          {
            ""x"": 224,
            ""y"": 1563
          },
          {
            ""x"": 209,
            ""y"": 1565
          },
          {
            ""x"": 192,
            ""y"": 1406
          }
        ],
        ""words"": [
          {
            ""text"": ""1,"",
            ""boundingPolygon"": [
              {
                ""x"": 204,
                ""y"": 1407
              },
              {
                ""x"": 205,
                ""y"": 1417
              },
              {
                ""x"": 193,
                ""y"": 1419
              },
              {
                ""x"": 192,
                ""y"": 1409
              }
            ],
            ""confidence"": 0.94
          },
          {
            ""text"": ""e,"",
            ""boundingPolygon"": [
              {
                ""x"": 205,
                ""y"": 1420
              },
              {
                ""x"": 205,
                ""y"": 1428
              },
              {
                ""x"": 193,
                ""y"": 1431
              },
              {
                ""x"": 193,
                ""y"": 1422
              }
            ],
            ""confidence"": 0.788
          },
          {
            ""text"": ""Sacrifice"",
            ""boundingPolygon"": [
              {
                ""x"": 206,
                ""y"": 1431
              },
              {
                ""x"": 209,
                ""y"": 1464
              },
              {
                ""x"": 197,
                ""y"": 1467
              },
              {
                ""x"": 193,
                ""y"": 1433
              }
            ],
            ""confidence"": 0.697
          },
          {
            ""text"": ""Jack-o"",
            ""boundingPolygon"": [
              {
                ""x"": 209,
                ""y"": 1466
              },
              {
                ""x"": 212,
                ""y"": 1490
              },
              {
                ""x"": 200,
                ""y"": 1494
              },
              {
                ""x"": 197,
                ""y"": 1469
              }
            ],
            ""confidence"": 0.746
          },
          {
            ""text"": ""-Lantern;"",
            ""boundingPolygon"": [
              {
                ""x"": 213,
                ""y"": 1493
              },
              {
                ""x"": 219,
                ""y"": 1531
              },
              {
                ""x"": 207,
                ""y"": 1536
              },
              {
                ""x"": 200,
                ""y"": 1496
              }
            ],
            ""confidence"": 0.444
          },
          {
            ""text"": ""Exile"",
            ""boundingPolygon"": [
              {
                ""x"": 219,
                ""y"": 1534
              },
              {
                ""x"": 223,
                ""y"": 1556
              },
              {
                ""x"": 212,
                ""y"": 1561
              },
              {
                ""x"": 207,
                ""y"": 1538
              }
            ],
            ""confidence"": 0.69
          }
        ]
      },
      {
        ""text"": ""noaddled i"",
        ""boundingPolygon"": [
          {
            ""x"": 152,
            ""y"": 184
          },
          {
            ""x"": 155,
            ""y"": 225
          },
          {
            ""x"": 141,
            ""y"": 226
          },
          {
            ""x"": 138,
            ""y"": 185
          }
        ],
        ""words"": [
          {
            ""text"": ""noaddled"",
            ""boundingPolygon"": [
              {
                ""x"": 152,
                ""y"": 184
              },
              {
                ""x"": 154,
                ""y"": 217
              },
              {
                ""x"": 140,
                ""y"": 218
              },
              {
                ""x"": 138,
                ""y"": 185
              }
            ],
            ""confidence"": 0.606
          },
          {
            ""text"": ""i"",
            ""boundingPolygon"": [
              {
                ""x"": 155,
                ""y"": 219
              },
              {
                ""x"": 155,
                ""y"": 224
              },
              {
                ""x"": 141,
                ""y"": 225
              },
              {
                ""x"": 141,
                ""y"": 220
              }
            ],
            ""confidence"": 0.787
          }
        ]
      },
      {
        ""text"": ""4, \"", Sacrifice Simle Ciues Poo"",
        ""boundingPolygon"": [
          {
            ""x"": 186,
            ""y"": 907
          },
          {
            ""x"": 221,
            ""y"": 1047
          },
          {
            ""x"": 205,
            ""y"": 1052
          },
          {
            ""x"": 172,
            ""y"": 911
          }
        ],
        ""words"": [
          {
            ""text"": ""4,"",
            ""boundingPolygon"": [
              {
                ""x"": 188,
                ""y"": 917
              },
              {
                ""x"": 190,
                ""y"": 927
              },
              {
                ""x"": 176,
                ""y"": 931
              },
              {
                ""x"": 174,
                ""y"": 921
              }
            ],
            ""confidence"": 0.146
          },
          {
            ""text"": ""\"","",
            ""boundingPolygon"": [
              {
                ""x"": 191,
                ""y"": 930
              },
              {
                ""x"": 193,
                ""y"": 938
              },
              {
                ""x"": 178,
                ""y"": 943
              },
              {
                ""x"": 176,
                ""y"": 934
              }
            ],
            ""confidence"": 0.139
          },
          {
            ""text"": ""Sacrifice"",
            ""boundingPolygon"": [
              {
                ""x"": 194,
                ""y"": 941
              },
              {
                ""x"": 202,
                ""y"": 974
              },
              {
                ""x"": 186,
                ""y"": 979
              },
              {
                ""x"": 179,
                ""y"": 946
              }
            ],
            ""confidence"": 0.576
          },
          {
            ""text"": ""Simle"",
            ""boundingPolygon"": [
              {
                ""x"": 202,
                ""y"": 977
              },
              {
                ""x"": 208,
                ""y"": 1000
              },
              {
                ""x"": 193,
                ""y"": 1005
              },
              {
                ""x"": 187,
                ""y"": 982
              }
            ],
            ""confidence"": 0.423
          },
          {
            ""text"": ""Ciues"",
            ""boundingPolygon"": [
              {
                ""x"": 209,
                ""y"": 1003
              },
              {
                ""x"": 215,
                ""y"": 1023
              },
              {
                ""x"": 201,
                ""y"": 1029
              },
              {
                ""x"": 194,
                ""y"": 1008
              }
            ],
            ""confidence"": 0.14
          },
          {
            ""text"": ""Poo"",
            ""boundingPolygon"": [
              {
                ""x"": 216,
                ""y"": 1026
              },
              {
                ""x"": 221,
                ""y"": 1044
              },
              {
                ""x"": 208,
                ""y"": 1051
              },
              {
                ""x"": 202,
                ""y"": 1032
              }
            ],
            ""confidence"": 0.124
          }
        ]
      },
      {
        ""text"": ""Suic symbols are a ver fere vandaliz"",
        ""boundingPolygon"": [
          {
            ""x"": 172,
            ""y"": 909
          },
          {
            ""x"": 176,
            ""y"": 1070
          },
          {
            ""x"": 161,
            ""y"": 1070
          },
          {
            ""x"": 158,
            ""y"": 910
          }
        ],
        ""words"": [
          {
            ""text"": ""Suic"",
            ""boundingPolygon"": [
              {
                ""x"": 173,
                ""y"": 911
              },
              {
                ""x"": 172,
                ""y"": 932
              },
              {
                ""x"": 159,
                ""y"": 935
              },
              {
                ""x"": 159,
                ""y"": 914
              }
            ],
            ""confidence"": 0.122
          },
          {
            ""text"": ""symbols"",
            ""boundingPolygon"": [
              {
                ""x"": 172,
                ""y"": 935
              },
              {
                ""x"": 172,
                ""y"": 964
              },
              {
                ""x"": 159,
                ""y"": 967
              },
              {
                ""x"": 159,
                ""y"": 938
              }
            ],
            ""confidence"": 0.489
          },
          {
            ""text"": ""are"",
            ""boundingPolygon"": [
              {
                ""x"": 172,
                ""y"": 966
              },
              {
                ""x"": 172,
                ""y"": 979
              },
              {
                ""x"": 159,
                ""y"": 982
              },
              {
                ""x"": 159,
                ""y"": 969
              }
            ],
            ""confidence"": 0.157
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 172,
                ""y"": 981
              },
              {
                ""x"": 172,
                ""y"": 987
              },
              {
                ""x"": 160,
                ""y"": 990
              },
              {
                ""x"": 159,
                ""y"": 985
              }
            ],
            ""confidence"": 0.124
          },
          {
            ""text"": ""ver"",
            ""boundingPolygon"": [
              {
                ""x"": 172,
                ""y"": 989
              },
              {
                ""x"": 173,
                ""y"": 1000
              },
              {
                ""x"": 160,
                ""y"": 1004
              },
              {
                ""x"": 160,
                ""y"": 992
              }
            ],
            ""confidence"": 0.442
          },
          {
            ""text"": ""fere"",
            ""boundingPolygon"": [
              {
                ""x"": 173,
                ""y"": 1003
              },
              {
                ""x"": 173,
                ""y"": 1015
              },
              {
                ""x"": 161,
                ""y"": 1019
              },
              {
                ""x"": 160,
                ""y"": 1006
              }
            ],
            ""confidence"": 0.634
          },
          {
            ""text"": ""vandaliz"",
            ""boundingPolygon"": [
              {
                ""x"": 173,
                ""y"": 1018
              },
              {
                ""x"": 175,
                ""y"": 1054
              },
              {
                ""x"": 164,
                ""y"": 1057
              },
              {
                ""x"": 161,
                ""y"": 1022
              }
            ],
            ""confidence"": 0.357
          }
        ]
      },
      {
        ""text"": ""Artifact"",
        ""boundingPolygon"": [
          {
            ""x"": 189,
            ""y"": 1159
          },
          {
            ""x"": 201,
            ""y"": 1197
          },
          {
            ""x"": 190,
            ""y"": 1200
          },
          {
            ""x"": 179,
            ""y"": 1162
          }
        ],
        ""words"": [
          {
            ""text"": ""Artifact"",
            ""boundingPolygon"": [
              {
                ""x"": 190,
                ""y"": 1159
              },
              {
                ""x"": 198,
                ""y"": 1188
              },
              {
                ""x"": 188,
                ""y"": 1192
              },
              {
                ""x"": 179,
                ""y"": 1163
              }
            ],
            ""confidence"": 0.758
          }
        ]
      },
      {
        ""text"": ""olay Baton of Courage any time"",
        ""boundingPolygon"": [
          {
            ""x"": 187,
            ""y"": 1158
          },
          {
            ""x"": 205,
            ""y"": 1313
          },
          {
            ""x"": 188,
            ""y"": 1315
          },
          {
            ""x"": 175,
            ""y"": 1160
          }
        ],
        ""words"": [
          {
            ""text"": ""olay"",
            ""boundingPolygon"": [
              {
                ""x"": 187,
                ""y"": 1193
              },
              {
                ""x"": 188,
                ""y"": 1209
              },
              {
                ""x"": 176,
                ""y"": 1213
              },
              {
                ""x"": 176,
                ""y"": 1197
              }
            ],
            ""confidence"": 0.645
          },
          {
            ""text"": ""Baton"",
            ""boundingPolygon"": [
              {
                ""x"": 188,
                ""y"": 1211
              },
              {
                ""x"": 190,
                ""y"": 1232
              },
              {
                ""x"": 179,
                ""y"": 1236
              },
              {
                ""x"": 177,
                ""y"": 1215
              }
            ],
            ""confidence"": 0.857
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 190,
                ""y"": 1234
              },
              {
                ""x"": 191,
                ""y"": 1242
              },
              {
                ""x"": 180,
                ""y"": 1246
              },
              {
                ""x"": 179,
                ""y"": 1239
              }
            ],
            ""confidence"": 0.999
          },
          {
            ""text"": ""Courage"",
            ""boundingPolygon"": [
              {
                ""x"": 191,
                ""y"": 1244
              },
              {
                ""x"": 196,
                ""y"": 1273
              },
              {
                ""x"": 186,
                ""y"": 1278
              },
              {
                ""x"": 180,
                ""y"": 1249
              }
            ],
            ""confidence"": 1
          },
          {
            ""text"": ""any"",
            ""boundingPolygon"": [
              {
                ""x"": 196,
                ""y"": 1275
              },
              {
                ""x"": 199,
                ""y"": 1287
              },
              {
                ""x"": 189,
                ""y"": 1292
              },
              {
                ""x"": 186,
                ""y"": 1280
              }
            ],
            ""confidence"": 0.998
          },
          {
            ""text"": ""time"",
            ""boundingPolygon"": [
              {
                ""x"": 199,
                ""y"": 1290
              },
              {
                ""x"": 204,
                ""y"": 1307
              },
              {
                ""x"": 195,
                ""y"": 1312
              },
              {
                ""x"": 190,
                ""y"": 1294
              }
            ],
            ""confidence"": 0.733
          }
        ]
      },
      {
        ""text"": ""up to on"",
        ""boundingPolygon"": [
          {
            ""x"": 197,
            ""y"": 1405
          },
          {
            ""x"": 200,
            ""y"": 1442
          },
          {
            ""x"": 187,
            ""y"": 1444
          },
          {
            ""x"": 184,
            ""y"": 1406
          }
        ],
        ""words"": [
          {
            ""text"": ""up"",
            ""boundingPolygon"": [
              {
                ""x"": 198,
                ""y"": 1407
              },
              {
                ""x"": 199,
                ""y"": 1417
              },
              {
                ""x"": 185,
                ""y"": 1418
              },
              {
                ""x"": 184,
                ""y"": 1408
              }
            ],
            ""confidence"": 0.154
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 199,
                ""y"": 1420
              },
              {
                ""x"": 200,
                ""y"": 1428
              },
              {
                ""x"": 186,
                ""y"": 1429
              },
              {
                ""x"": 185,
                ""y"": 1421
              }
            ],
            ""confidence"": 0.828
          },
          {
            ""text"": ""on"",
            ""boundingPolygon"": [
              {
                ""x"": 200,
                ""y"": 1431
              },
              {
                ""x"": 201,
                ""y"": 1442
              },
              {
                ""x"": 187,
                ""y"": 1443
              },
              {
                ""x"": 186,
                ""y"": 1432
              }
            ],
            ""confidence"": 0.85
          }
        ]
      },
      {
        ""text"": ""inally created to ha"",
        ""boundingPolygon"": [
          {
            ""x"": 215,
            ""y"": 1664
          },
          {
            ""x"": 224,
            ""y"": 1739
          },
          {
            ""x"": 209,
            ""y"": 1740
          },
          {
            ""x"": 201,
            ""y"": 1666
          }
        ],
        ""words"": [
          {
            ""text"": ""inally"",
            ""boundingPolygon"": [
              {
                ""x"": 215,
                ""y"": 1664
              },
              {
                ""x"": 218,
                ""y"": 1684
              },
              {
                ""x"": 204,
                ""y"": 1687
              },
              {
                ""x"": 201,
                ""y"": 1667
              }
            ],
            ""confidence"": 0.66
          },
          {
            ""text"": ""created"",
            ""boundingPolygon"": [
              {
                ""x"": 218,
                ""y"": 1686
              },
              {
                ""x"": 221,
                ""y"": 1713
              },
              {
                ""x"": 208,
                ""y"": 1716
              },
              {
                ""x"": 204,
                ""y"": 1690
              }
            ],
            ""confidence"": 0.642
          },
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 222,
                ""y"": 1716
              },
              {
                ""x"": 222,
                ""y"": 1722
              },
              {
                ""x"": 209,
                ""y"": 1726
              },
              {
                ""x"": 208,
                ""y"": 1719
              }
            ],
            ""confidence"": 0.913
          },
          {
            ""text"": ""ha"",
            ""boundingPolygon"": [
              {
                ""x"": 223,
                ""y"": 1725
              },
              {
                ""x"": 224,
                ""y"": 1737
              },
              {
                ""x"": 211,
                ""y"": 1741
              },
              {
                ""x"": 209,
                ""y"": 1728
              }
            ],
            ""confidence"": 0.357
          }
        ]
      },
      {
        ""text"": ""to one target"",
        ""boundingPolygon"": [
          {
            ""x"": 192,
            ""y"": 1421
          },
          {
            ""x"": 200,
            ""y"": 1476
          },
          {
            ""x"": 188,
            ""y"": 1477
          },
          {
            ""x"": 180,
            ""y"": 1423
          }
        ],
        ""words"": [
          {
            ""text"": ""to"",
            ""boundingPolygon"": [
              {
                ""x"": 192,
                ""y"": 1421
              },
              {
                ""x"": 193,
                ""y"": 1428
              },
              {
                ""x"": 181,
                ""y"": 1431
              },
              {
                ""x"": 180,
                ""y"": 1424
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""one"",
            ""boundingPolygon"": [
              {
                ""x"": 194,
                ""y"": 1430
              },
              {
                ""x"": 196,
                ""y"": 1444
              },
              {
                ""x"": 184,
                ""y"": 1448
              },
              {
                ""x"": 181,
                ""y"": 1433
              }
            ],
            ""confidence"": 0.977
          },
          {
            ""text"": ""target"",
            ""boundingPolygon"": [
              {
                ""x"": 196,
                ""y"": 1447
              },
              {
                ""x"": 200,
                ""y"": 1473
              },
              {
                ""x"": 188,
                ""y"": 1477
              },
              {
                ""x"": 184,
                ""y"": 1450
              }
            ],
            ""confidence"": 0.933
          }
        ]
      },
      {
        ""text"": ""blinkmoths, the hovermyr are noc"",
        ""boundingPolygon"": [
          {
            ""x"": 206,
            ""y"": 1646
          },
          {
            ""x"": 222,
            ""y"": 1772
          },
          {
            ""x"": 205,
            ""y"": 1774
          },
          {
            ""x"": 189,
            ""y"": 1648
          }
        ],
        ""words"": [
          {
            ""text"": ""blinkmoths,"",
            ""boundingPolygon"": [
              {
                ""x"": 206,
                ""y"": 1647
              },
              {
                ""x"": 211,
                ""y"": 1688
              },
              {
                ""x"": 196,
                ""y"": 1690
              },
              {
                ""x"": 189,
                ""y"": 1649
              }
            ],
            ""confidence"": 0.658
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 212,
                ""y"": 1691
              },
              {
                ""x"": 213,
                ""y"": 1702
              },
              {
                ""x"": 198,
                ""y"": 1704
              },
              {
                ""x"": 196,
                ""y"": 1693
              }
            ],
            ""confidence"": 0.992
          },
          {
            ""text"": ""hovermyr"",
            ""boundingPolygon"": [
              {
                ""x"": 213,
                ""y"": 1705
              },
              {
                ""x"": 218,
                ""y"": 1741
              },
              {
                ""x"": 203,
                ""y"": 1743
              },
              {
                ""x"": 198,
                ""y"": 1707
              }
            ],
            ""confidence"": 0.716
          },
          {
            ""text"": ""are"",
            ""boundingPolygon"": [
              {
                ""x"": 218,
                ""y"": 1744
              },
              {
                ""x"": 220,
                ""y"": 1755
              },
              {
                ""x"": 204,
                ""y"": 1756
              },
              {
                ""x"": 203,
                ""y"": 1746
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""noc"",
            ""boundingPolygon"": [
              {
                ""x"": 220,
                ""y"": 1758
              },
              {
                ""x"": 222,
                ""y"": 1772
              },
              {
                ""x"": 206,
                ""y"": 1774
              },
              {
                ""x"": 205,
                ""y"": 1760
              }
            ],
            ""confidence"": 0.51
          }
        ]
      },
      {
        ""text"": ""wrycaruth. The mexcon mel"",
        ""boundingPolygon"": [
          {
            ""x"": 165,
            ""y"": 731
          },
          {
            ""x"": 174,
            ""y"": 815
          },
          {
            ""x"": 161,
            ""y"": 816
          },
          {
            ""x"": 152,
            ""y"": 732
          }
        ],
        ""words"": [
          {
            ""text"": ""wrycaruth."",
            ""boundingPolygon"": [
              {
                ""x"": 163,
                ""y"": 731
              },
              {
                ""x"": 169,
                ""y"": 765
              },
              {
                ""x"": 158,
                ""y"": 766
              },
              {
                ""x"": 153,
                ""y"": 732
              }
            ],
            ""confidence"": 0.122
          },
          {
            ""text"": ""The"",
            ""boundingPolygon"": [
              {
                ""x"": 170,
                ""y"": 767
              },
              {
                ""x"": 171,
                ""y"": 778
              },
              {
                ""x"": 159,
                ""y"": 780
              },
              {
                ""x"": 158,
                ""y"": 768
              }
            ],
            ""confidence"": 0.992
          },
          {
            ""text"": ""mexcon"",
            ""boundingPolygon"": [
              {
                ""x"": 171,
                ""y"": 780
              },
              {
                ""x"": 173,
                ""y"": 801
              },
              {
                ""x"": 161,
                ""y"": 802
              },
              {
                ""x"": 160,
                ""y"": 782
              }
            ],
            ""confidence"": 0.445
          },
          {
            ""text"": ""mel"",
            ""boundingPolygon"": [
              {
                ""x"": 173,
                ""y"": 803
              },
              {
                ""x"": 174,
                ""y"": 814
              },
              {
                ""x"": 162,
                ""y"": 816
              },
              {
                ""x"": 161,
                ""y"": 805
              }
            ],
            ""confidence"": 0.055
          }
        ]
      },
      {
        ""text"": ""Draw a ca"",
        ""boundingPolygon"": [
          {
            ""x"": 186,
            ""y"": 1408
          },
          {
            ""x"": 191,
            ""y"": 1450
          },
          {
            ""x"": 178,
            ""y"": 1451
          },
          {
            ""x"": 174,
            ""y"": 1409
          }
        ],
        ""words"": [
          {
            ""text"": ""Draw"",
            ""boundingPolygon"": [
              {
                ""x"": 186,
                ""y"": 1408
              },
              {
                ""x"": 189,
                ""y"": 1430
              },
              {
                ""x"": 177,
                ""y"": 1432
              },
              {
                ""x"": 174,
                ""y"": 1411
              }
            ],
            ""confidence"": 0.937
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 189,
                ""y"": 1433
              },
              {
                ""x"": 190,
                ""y"": 1438
              },
              {
                ""x"": 178,
                ""y"": 1440
              },
              {
                ""x"": 177,
                ""y"": 1435
              }
            ],
            ""confidence"": 0.924
          },
          {
            ""text"": ""ca"",
            ""boundingPolygon"": [
              {
                ""x"": 190,
                ""y"": 1440
              },
              {
                ""x"": 191,
                ""y"": 1450
              },
              {
                ""x"": 180,
                ""y"": 1452
              },
              {
                ""x"": 178,
                ""y"": 1442
              }
            ],
            ""confidence"": 0.988
          }
        ]
      },
      {
        ""text"": ""silent observers of a dying and the"",
        ""boundingPolygon"": [
          {
            ""x"": 198,
            ""y"": 1648
          },
          {
            ""x"": 220,
            ""y"": 1794
          },
          {
            ""x"": 203,
            ""y"": 1796
          },
          {
            ""x"": 184,
            ""y"": 1650
          }
        ],
        ""words"": [
          {
            ""text"": ""silent"",
            ""boundingPolygon"": [
              {
                ""x"": 199,
                ""y"": 1648
              },
              {
                ""x"": 199,
                ""y"": 1666
              },
              {
                ""x"": 185,
                ""y"": 1668
              },
              {
                ""x"": 184,
                ""y"": 1650
              }
            ],
            ""confidence"": 0.5
          },
          {
            ""text"": ""observers"",
            ""boundingPolygon"": [
              {
                ""x"": 200,
                ""y"": 1668
              },
              {
                ""x"": 203,
                ""y"": 1702
              },
              {
                ""x"": 189,
                ""y"": 1705
              },
              {
                ""x"": 186,
                ""y"": 1670
              }
            ],
            ""confidence"": 0.791
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 203,
                ""y"": 1705
              },
              {
                ""x"": 204,
                ""y"": 1712
              },
              {
                ""x"": 191,
                ""y"": 1715
              },
              {
                ""x"": 190,
                ""y"": 1707
              }
            ],
            ""confidence"": 0.801
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 205,
                ""y"": 1715
              },
              {
                ""x"": 205,
                ""y"": 1720
              },
              {
                ""x"": 192,
                ""y"": 1722
              },
              {
                ""x"": 191,
                ""y"": 1718
              }
            ],
            ""confidence"": 0.818
          },
          {
            ""text"": ""dying"",
            ""boundingPolygon"": [
              {
                ""x"": 206,
                ""y"": 1723
              },
              {
                ""x"": 209,
                ""y"": 1743
              },
              {
                ""x"": 196,
                ""y"": 1745
              },
              {
                ""x"": 192,
                ""y"": 1725
              }
            ],
            ""confidence"": 0.148
          },
          {
            ""text"": ""and"",
            ""boundingPolygon"": [
              {
                ""x"": 210,
                ""y"": 1745
              },
              {
                ""x"": 212,
                ""y"": 1760
              },
              {
                ""x"": 199,
                ""y"": 1762
              },
              {
                ""x"": 196,
                ""y"": 1748
              }
            ],
            ""confidence"": 0.118
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 216,
                ""y"": 1777
              },
              {
                ""x"": 220,
                ""y"": 1791
              },
              {
                ""x"": 205,
                ""y"": 1794
              },
              {
                ""x"": 202,
                ""y"": 1780
              }
            ],
            ""confidence"": 0.259
          }
        ]
      },
      {
        ""text"": ""ted Neu Phryrena,"",
        ""boundingPolygon"": [
          {
            ""x"": 151,
            ""y"": 681
          },
          {
            ""x"": 154,
            ""y"": 746
          },
          {
            ""x"": 142,
            ""y"": 746
          },
          {
            ""x"": 139,
            ""y"": 682
          }
        ],
        ""words"": [
          {
            ""text"": ""ted"",
            ""boundingPolygon"": [
              {
                ""x"": 151,
                ""y"": 682
              },
              {
                ""x"": 151,
                ""y"": 691
              },
              {
                ""x"": 140,
                ""y"": 693
              },
              {
                ""x"": 139,
                ""y"": 683
              }
            ],
            ""confidence"": 0.205
          },
          {
            ""text"": ""Neu"",
            ""boundingPolygon"": [
              {
                ""x"": 151,
                ""y"": 693
              },
              {
                ""x"": 152,
                ""y"": 709
              },
              {
                ""x"": 141,
                ""y"": 711
              },
              {
                ""x"": 140,
                ""y"": 695
              }
            ],
            ""confidence"": 0.306
          },
          {
            ""text"": ""Phryrena,"",
            ""boundingPolygon"": [
              {
                ""x"": 152,
                ""y"": 711
              },
              {
                ""x"": 155,
                ""y"": 744
              },
              {
                ""x"": 143,
                ""y"": 747
              },
              {
                ""x"": 141,
                ""y"": 713
              }
            ],
            ""confidence"": 0.553
          }
        ]
      },
      {
        ""text"": ""intricate and frag"",
        ""boundingPolygon"": [
          {
            ""x"": 157,
            ""y"": 912
          },
          {
            ""x"": 165,
            ""y"": 983
          },
          {
            ""x"": 149,
            ""y"": 984
          },
          {
            ""x"": 142,
            ""y"": 913
          }
        ],
        ""words"": [
          {
            ""text"": ""intricate"",
            ""boundingPolygon"": [
              {
                ""x"": 155,
                ""y"": 912
              },
              {
                ""x"": 159,
                ""y"": 943
              },
              {
                ""x"": 145,
                ""y"": 947
              },
              {
                ""x"": 142,
                ""y"": 916
              }
            ],
            ""confidence"": 0.687
          },
          {
            ""text"": ""and"",
            ""boundingPolygon"": [
              {
                ""x"": 159,
                ""y"": 946
              },
              {
                ""x"": 161,
                ""y"": 959
              },
              {
                ""x"": 147,
                ""y"": 964
              },
              {
                ""x"": 145,
                ""y"": 950
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""frag"",
            ""boundingPolygon"": [
              {
                ""x"": 162,
                ""y"": 962
              },
              {
                ""x"": 165,
                ""y"": 979
              },
              {
                ""x"": 150,
                ""y"": 985
              },
              {
                ""x"": 147,
                ""y"": 967
              }
            ],
            ""confidence"": 0.598
          }
        ]
      },
      {
        ""text"": ""t of the strange lifeforms that"",
        ""boundingPolygon"": [
          {
            ""x"": 157,
            ""y"": 957
          },
          {
            ""x"": 173,
            ""y"": 1068
          },
          {
            ""x"": 160,
            ""y"": 1070
          },
          {
            ""x"": 143,
            ""y"": 959
          }
        ],
        ""words"": [
          {
            ""text"": ""t"",
            ""boundingPolygon"": [
              {
                ""x"": 156,
                ""y"": 958
              },
              {
                ""x"": 157,
                ""y"": 960
              },
              {
                ""x"": 144,
                ""y"": 964
              },
              {
                ""x"": 143,
                ""y"": 961
              }
            ],
            ""confidence"": 0.582
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 157,
                ""y"": 963
              },
              {
                ""x"": 159,
                ""y"": 970
              },
              {
                ""x"": 146,
                ""y"": 974
              },
              {
                ""x"": 144,
                ""y"": 966
              }
            ],
            ""confidence"": 0.955
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 159,
                ""y"": 973
              },
              {
                ""x"": 161,
                ""y"": 983
              },
              {
                ""x"": 148,
                ""y"": 987
              },
              {
                ""x"": 146,
                ""y"": 976
              }
            ],
            ""confidence"": 0.988
          },
          {
            ""text"": ""strange"",
            ""boundingPolygon"": [
              {
                ""x"": 162,
                ""y"": 986
              },
              {
                ""x"": 166,
                ""y"": 1012
              },
              {
                ""x"": 154,
                ""y"": 1016
              },
              {
                ""x"": 149,
                ""y"": 990
              }
            ],
            ""confidence"": 0.704
          },
          {
            ""text"": ""lifeforms"",
            ""boundingPolygon"": [
              {
                ""x"": 166,
                ""y"": 1014
              },
              {
                ""x"": 171,
                ""y"": 1046
              },
              {
                ""x"": 159,
                ""y"": 1050
              },
              {
                ""x"": 154,
                ""y"": 1018
              }
            ],
            ""confidence"": 0.627
          },
          {
            ""text"": ""that"",
            ""boundingPolygon"": [
              {
                ""x"": 171,
                ""y"": 1049
              },
              {
                ""x"": 173,
                ""y"": 1067
              },
              {
                ""x"": 162,
                ""y"": 1070
              },
              {
                ""x"": 159,
                ""y"": 1053
              }
            ],
            ""confidence"": 0.975
          }
        ]
      },
      {
        ""text"": ""You may play Baton obou"",
        ""boundingPolygon"": [
          {
            ""x"": 177,
            ""y"": 1164
          },
          {
            ""x"": 190,
            ""y"": 1263
          },
          {
            ""x"": 176,
            ""y"": 1265
          },
          {
            ""x"": 163,
            ""y"": 1166
          }
        ],
        ""words"": [
          {
            ""text"": ""You"",
            ""boundingPolygon"": [
              {
                ""x"": 177,
                ""y"": 1164
              },
              {
                ""x"": 178,
                ""y"": 1176
              },
              {
                ""x"": 164,
                ""y"": 1178
              },
              {
                ""x"": 163,
                ""y"": 1167
              }
            ],
            ""confidence"": 0.701
          },
          {
            ""text"": ""may"",
            ""boundingPolygon"": [
              {
                ""x"": 178,
                ""y"": 1178
              },
              {
                ""x"": 180,
                ""y"": 1193
              },
              {
                ""x"": 167,
                ""y"": 1196
              },
              {
                ""x"": 165,
                ""y"": 1181
              }
            ],
            ""confidence"": 0.988
          },
          {
            ""text"": ""play"",
            ""boundingPolygon"": [
              {
                ""x"": 180,
                ""y"": 1196
              },
              {
                ""x"": 182,
                ""y"": 1211
              },
              {
                ""x"": 169,
                ""y"": 1213
              },
              {
                ""x"": 167,
                ""y"": 1198
              }
            ],
            ""confidence"": 0.767
          },
          {
            ""text"": ""Baton"",
            ""boundingPolygon"": [
              {
                ""x"": 182,
                ""y"": 1214
              },
              {
                ""x"": 185,
                ""y"": 1233
              },
              {
                ""x"": 172,
                ""y"": 1236
              },
              {
                ""x"": 170,
                ""y"": 1216
              }
            ],
            ""confidence"": 0.529
          },
          {
            ""text"": ""obou"",
            ""boundingPolygon"": [
              {
                ""x"": 185,
                ""y"": 1236
              },
              {
                ""x"": 190,
                ""y"": 1263
              },
              {
                ""x"": 177,
                ""y"": 1265
              },
              {
                ""x"": 173,
                ""y"": 1238
              }
            ],
            ""confidence"": 0.12
          }
        ]
      },
      {
        ""text"": ""uck-o'-Lantern from your"",
        ""boundingPolygon"": [
          {
            ""x"": 178,
            ""y"": 1450
          },
          {
            ""x"": 193,
            ""y"": 1562
          },
          {
            ""x"": 180,
            ""y"": 1564
          },
          {
            ""x"": 166,
            ""y"": 1452
          }
        ],
        ""words"": [
          {
            ""text"": ""uck-o'-Lantern"",
            ""boundingPolygon"": [
              {
                ""x"": 178,
                ""y"": 1451
              },
              {
                ""x"": 184,
                ""y"": 1509
              },
              {
                ""x"": 173,
                ""y"": 1512
              },
              {
                ""x"": 166,
                ""y"": 1454
              }
            ],
            ""confidence"": 0.602
          },
          {
            ""text"": ""from"",
            ""boundingPolygon"": [
              {
                ""x"": 185,
                ""y"": 1511
              },
              {
                ""x"": 187,
                ""y"": 1528
              },
              {
                ""x"": 176,
                ""y"": 1531
              },
              {
                ""x"": 174,
                ""y"": 1514
              }
            ],
            ""confidence"": 0.513
          },
          {
            ""text"": ""your"",
            ""boundingPolygon"": [
              {
                ""x"": 188,
                ""y"": 1532
              },
              {
                ""x"": 191,
                ""y"": 1552
              },
              {
                ""x"": 180,
                ""y"": 1555
              },
              {
                ""x"": 177,
                ""y"": 1535
              }
            ],
            ""confidence"": 0.869
          }
        ]
      },
      {
        ""text"": ""them for fear of the str"",
        ""boundingPolygon"": [
          {
            ""x"": 149,
            ""y"": 912
          },
          {
            ""x"": 159,
            ""y"": 999
          },
          {
            ""x"": 143,
            ""y"": 1001
          },
          {
            ""x"": 135,
            ""y"": 913
          }
        ],
        ""words"": [
          {
            ""text"": ""them"",
            ""boundingPolygon"": [
              {
                ""x"": 149,
                ""y"": 913
              },
              {
                ""x"": 150,
                ""y"": 931
              },
              {
                ""x"": 136,
                ""y"": 935
              },
              {
                ""x"": 135,
                ""y"": 916
              }
            ],
            ""confidence"": 0.918
          },
          {
            ""text"": ""for"",
            ""boundingPolygon"": [
              {
                ""x"": 150,
                ""y"": 934
              },
              {
                ""x"": 151,
                ""y"": 943
              },
              {
                ""x"": 138,
                ""y"": 947
              },
              {
                ""x"": 137,
                ""y"": 938
              }
            ],
            ""confidence"": 0.936
          },
          {
            ""text"": ""fear"",
            ""boundingPolygon"": [
              {
                ""x"": 151,
                ""y"": 946
              },
              {
                ""x"": 153,
                ""y"": 961
              },
              {
                ""x"": 140,
                ""y"": 965
              },
              {
                ""x"": 138,
                ""y"": 950
              }
            ],
            ""confidence"": 0.976
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 153,
                ""y"": 964
              },
              {
                ""x"": 154,
                ""y"": 970
              },
              {
                ""x"": 141,
                ""y"": 975
              },
              {
                ""x"": 140,
                ""y"": 968
              }
            ],
            ""confidence"": 0.654
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 155,
                ""y"": 973
              },
              {
                ""x"": 157,
                ""y"": 984
              },
              {
                ""x"": 144,
                ""y"": 989
              },
              {
                ""x"": 142,
                ""y"": 978
              }
            ],
            ""confidence"": 0.949
          },
          {
            ""text"": ""str"",
            ""boundingPolygon"": [
              {
                ""x"": 157,
                ""y"": 986
              },
              {
                ""x"": 159,
                ""y"": 995
              },
              {
                ""x"": 146,
                ""y"": 1001
              },
              {
                ""x"": 144,
                ""y"": 992
              }
            ],
            ""confidence"": 0.517
          }
        ]
      },
      {
        ""text"": ""you could play an inst"",
        ""boundingPolygon"": [
          {
            ""x"": 167,
            ""y"": 1165
          },
          {
            ""x"": 178,
            ""y"": 1246
          },
          {
            ""x"": 164,
            ""y"": 1248
          },
          {
            ""x"": 153,
            ""y"": 1167
          }
        ],
        ""words"": [
          {
            ""text"": ""you"",
            ""boundingPolygon"": [
              {
                ""x"": 166,
                ""y"": 1166
              },
              {
                ""x"": 168,
                ""y"": 1177
              },
              {
                ""x"": 155,
                ""y"": 1179
              },
              {
                ""x"": 153,
                ""y"": 1169
              }
            ],
            ""confidence"": 0.958
          },
          {
            ""text"": ""could"",
            ""boundingPolygon"": [
              {
                ""x"": 169,
                ""y"": 1179
              },
              {
                ""x"": 172,
                ""y"": 1199
              },
              {
                ""x"": 159,
                ""y"": 1201
              },
              {
                ""x"": 156,
                ""y"": 1182
              }
            ],
            ""confidence"": 0.529
          },
          {
            ""text"": ""play"",
            ""boundingPolygon"": [
              {
                ""x"": 173,
                ""y"": 1201
              },
              {
                ""x"": 174,
                ""y"": 1216
              },
              {
                ""x"": 162,
                ""y"": 1218
              },
              {
                ""x"": 160,
                ""y"": 1204
              }
            ],
            ""confidence"": 0.921
          },
          {
            ""text"": ""an"",
            ""boundingPolygon"": [
              {
                ""x"": 175,
                ""y"": 1218
              },
              {
                ""x"": 176,
                ""y"": 1227
              },
              {
                ""x"": 163,
                ""y"": 1229
              },
              {
                ""x"": 162,
                ""y"": 1221
              }
            ],
            ""confidence"": 0.985
          },
          {
            ""text"": ""inst"",
            ""boundingPolygon"": [
              {
                ""x"": 176,
                ""y"": 1229
              },
              {
                ""x"": 177,
                ""y"": 1246
              },
              {
                ""x"": 166,
                ""y"": 1248
              },
              {
                ""x"": 164,
                ""y"": 1231
              }
            ],
            ""confidence"": 0.455
          }
        ]
      },
      {
        ""text"": ""urst (This comes into play with a change"",
        ""boundingPolygon"": [
          {
            ""x"": 162,
            ""y"": 1185
          },
          {
            ""x"": 181,
            ""y"": 1320
          },
          {
            ""x"": 167,
            ""y"": 1322
          },
          {
            ""x"": 149,
            ""y"": 1187
          }
        ],
        ""words"": [
          {
            ""text"": ""urst"",
            ""boundingPolygon"": [
              {
                ""x"": 161,
                ""y"": 1185
              },
              {
                ""x"": 163,
                ""y"": 1196
              },
              {
                ""x"": 151,
                ""y"": 1199
              },
              {
                ""x"": 150,
                ""y"": 1188
              }
            ],
            ""confidence"": 0.918
          },
          {
            ""text"": ""(This"",
            ""boundingPolygon"": [
              {
                ""x"": 163,
                ""y"": 1199
              },
              {
                ""x"": 165,
                ""y"": 1218
              },
              {
                ""x"": 153,
                ""y"": 1221
              },
              {
                ""x"": 151,
                ""y"": 1202
              }
            ],
            ""confidence"": 0.866
          },
          {
            ""text"": ""comes"",
            ""boundingPolygon"": [
              {
                ""x"": 166,
                ""y"": 1221
              },
              {
                ""x"": 168,
                ""y"": 1238
              },
              {
                ""x"": 156,
                ""y"": 1241
              },
              {
                ""x"": 153,
                ""y"": 1224
              }
            ],
            ""confidence"": 0.868
          },
          {
            ""text"": ""into"",
            ""boundingPolygon"": [
              {
                ""x"": 168,
                ""y"": 1240
              },
              {
                ""x"": 170,
                ""y"": 1253
              },
              {
                ""x"": 158,
                ""y"": 1256
              },
              {
                ""x"": 156,
                ""y"": 1244
              }
            ],
            ""confidence"": 0.576
          },
          {
            ""text"": ""play"",
            ""boundingPolygon"": [
              {
                ""x"": 171,
                ""y"": 1255
              },
              {
                ""x"": 173,
                ""y"": 1270
              },
              {
                ""x"": 160,
                ""y"": 1273
              },
              {
                ""x"": 158,
                ""y"": 1259
              }
            ],
            ""confidence"": 0.971
          },
          {
            ""text"": ""with"",
            ""boundingPolygon"": [
              {
                ""x"": 173,
                ""y"": 1272
              },
              {
                ""x"": 175,
                ""y"": 1286
              },
              {
                ""x"": 163,
                ""y"": 1290
              },
              {
                ""x"": 161,
                ""y"": 1275
              }
            ],
            ""confidence"": 0.824
          },
          {
            ""text"": ""a"",
            ""boundingPolygon"": [
              {
                ""x"": 176,
                ""y"": 1289
              },
              {
                ""x"": 177,
                ""y"": 1293
              },
              {
                ""x"": 164,
                ""y"": 1296
              },
              {
                ""x"": 163,
                ""y"": 1292
              }
            ],
            ""confidence"": 0.983
          },
          {
            ""text"": ""change"",
            ""boundingPolygon"": [
              {
                ""x"": 177,
                ""y"": 1295
              },
              {
                ""x"": 181,
                ""y"": 1318
              },
              {
                ""x"": 169,
                ""y"": 1322
              },
              {
                ""x"": 164,
                ""y"": 1299
              }
            ],
            ""confidence"": 0.81
          }
        ]
      },
      {
        ""text"": ""1, Exile J'Add one mana of any col"",
        ""boundingPolygon"": [
          {
            ""x"": 172,
            ""y"": 1412
          },
          {
            ""x"": 185,
            ""y"": 1557
          },
          {
            ""x"": 172,
            ""y"": 1558
          },
          {
            ""x"": 159,
            ""y"": 1414
          }
        ],
        ""words"": [
          {
            ""text"": ""1,"",
            ""boundingPolygon"": [
              {
                ""x"": 172,
                ""y"": 1413
              },
              {
                ""x"": 172,
                ""y"": 1422
              },
              {
                ""x"": 160,
                ""y"": 1424
              },
              {
                ""x"": 159,
                ""y"": 1415
              }
            ],
            ""confidence"": 0.726
          },
          {
            ""text"": ""Exile"",
            ""boundingPolygon"": [
              {
                ""x"": 173,
                ""y"": 1424
              },
              {
                ""x"": 173,
                ""y"": 1444
              },
              {
                ""x"": 161,
                ""y"": 1446
              },
              {
                ""x"": 160,
                ""y"": 1426
              }
            ],
            ""confidence"": 0.547
          },
          {
            ""text"": ""J'Add"",
            ""boundingPolygon"": [
              {
                ""x"": 173,
                ""y"": 1446
              },
              {
                ""x"": 175,
                ""y"": 1471
              },
              {
                ""x"": 164,
                ""y"": 1474
              },
              {
                ""x"": 161,
                ""y"": 1448
              }
            ],
            ""confidence"": 0.12
          },
          {
            ""text"": ""one"",
            ""boundingPolygon"": [
              {
                ""x"": 175,
                ""y"": 1474
              },
              {
                ""x"": 176,
                ""y"": 1489
              },
              {
                ""x"": 166,
                ""y"": 1492
              },
              {
                ""x"": 164,
                ""y"": 1476
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""mana"",
            ""boundingPolygon"": [
              {
                ""x"": 176,
                ""y"": 1491
              },
              {
                ""x"": 179,
                ""y"": 1513
              },
              {
                ""x"": 168,
                ""y"": 1516
              },
              {
                ""x"": 166,
                ""y"": 1494
              }
            ],
            ""confidence"": 0.976
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 179,
                ""y"": 1515
              },
              {
                ""x"": 180,
                ""y"": 1524
              },
              {
                ""x"": 170,
                ""y"": 1527
              },
              {
                ""x"": 169,
                ""y"": 1518
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""any"",
            ""boundingPolygon"": [
              {
                ""x"": 180,
                ""y"": 1526
              },
              {
                ""x"": 182,
                ""y"": 1539
              },
              {
                ""x"": 172,
                ""y"": 1543
              },
              {
                ""x"": 170,
                ""y"": 1529
              }
            ],
            ""confidence"": 0.97
          },
          {
            ""text"": ""col"",
            ""boundingPolygon"": [
              {
                ""x"": 183,
                ""y"": 1542
              },
              {
                ""x"": 185,
                ""y"": 1554
              },
              {
                ""x"": 174,
                ""y"": 1558
              },
              {
                ""x"": 173,
                ""y"": 1545
              }
            ],
            ""confidence"": 0.917
          }
        ]
      },
      {
        ""text"": ""1/2"",
        ""boundingPolygon"": [
          {
            ""x"": 179,
            ""y"": 1775
          },
          {
            ""x"": 184,
            ""y"": 1804
          },
          {
            ""x"": 172,
            ""y"": 1807
          },
          {
            ""x"": 167,
            ""y"": 1777
          }
        ],
        ""words"": [
          {
            ""text"": ""1/2"",
            ""boundingPolygon"": [
              {
                ""x"": 182,
                ""y"": 1785
              },
              {
                ""x"": 184,
                ""y"": 1801
              },
              {
                ""x"": 172,
                ""y"": 1803
              },
              {
                ""x"": 169,
                ""y"": 1787
              }
            ],
            ""confidence"": 0.936
          }
        ]
      },
      {
        ""text"": ""might be released"",
        ""boundingPolygon"": [
          {
            ""x"": 140,
            ""y"": 913
          },
          {
            ""x"": 150,
            ""y"": 980
          },
          {
            ""x"": 136,
            ""y"": 982
          },
          {
            ""x"": 126,
            ""y"": 915
          }
        ],
        ""words"": [
          {
            ""text"": ""might"",
            ""boundingPolygon"": [
              {
                ""x"": 140,
                ""y"": 914
              },
              {
                ""x"": 142,
                ""y"": 936
              },
              {
                ""x"": 131,
                ""y"": 940
              },
              {
                ""x"": 127,
                ""y"": 918
              }
            ],
            ""confidence"": 0.61
          },
          {
            ""text"": ""be"",
            ""boundingPolygon"": [
              {
                ""x"": 143,
                ""y"": 938
              },
              {
                ""x"": 144,
                ""y"": 947
              },
              {
                ""x"": 133,
                ""y"": 951
              },
              {
                ""x"": 131,
                ""y"": 942
              }
            ],
            ""confidence"": 0.882
          },
          {
            ""text"": ""released"",
            ""boundingPolygon"": [
              {
                ""x"": 145,
                ""y"": 949
              },
              {
                ""x"": 150,
                ""y"": 977
              },
              {
                ""x"": 138,
                ""y"": 982
              },
              {
                ""x"": 133,
                ""y"": 953
              }
            ],
            ""confidence"": 0.886
          }
        ]
      },
      {
        ""text"": ""counter om ir for ta"",
        ""boundingPolygon"": [
          {
            ""x"": 146,
            ""y"": 1169
          },
          {
            ""x"": 157,
            ""y"": 1232
          },
          {
            ""x"": 143,
            ""y"": 1235
          },
          {
            ""x"": 133,
            ""y"": 1171
          }
        ],
        ""words"": [
          {
            ""text"": ""counter"",
            ""boundingPolygon"": [
              {
                ""x"": 147,
                ""y"": 1170
              },
              {
                ""x"": 150,
                ""y"": 1192
              },
              {
                ""x"": 136,
                ""y"": 1194
              },
              {
                ""x"": 133,
                ""y"": 1172
              }
            ],
            ""confidence"": 0.639
          },
          {
            ""text"": ""om"",
            ""boundingPolygon"": [
              {
                ""x"": 151,
                ""y"": 1194
              },
              {
                ""x"": 152,
                ""y"": 1201
              },
              {
                ""x"": 138,
                ""y"": 1204
              },
              {
                ""x"": 136,
                ""y"": 1196
              }
            ],
            ""confidence"": 0.15
          },
          {
            ""text"": ""ir"",
            ""boundingPolygon"": [
              {
                ""x"": 152,
                ""y"": 1204
              },
              {
                ""x"": 153,
                ""y"": 1208
              },
              {
                ""x"": 139,
                ""y"": 1211
              },
              {
                ""x"": 138,
                ""y"": 1206
              }
            ],
            ""confidence"": 0.467
          },
          {
            ""text"": ""for"",
            ""boundingPolygon"": [
              {
                ""x"": 153,
                ""y"": 1211
              },
              {
                ""x"": 155,
                ""y"": 1220
              },
              {
                ""x"": 142,
                ""y"": 1222
              },
              {
                ""x"": 139,
                ""y"": 1213
              }
            ],
            ""confidence"": 0.793
          },
          {
            ""text"": ""ta"",
            ""boundingPolygon"": [
              {
                ""x"": 155,
                ""y"": 1222
              },
              {
                ""x"": 157,
                ""y"": 1232
              },
              {
                ""x"": 145,
                ""y"": 1235
              },
              {
                ""x"": 143,
                ""y"": 1225
              }
            ],
            ""confidence"": 0.318
          }
        ]
      },
      {
        ""text"": ""Graveyard: A"",
        ""boundingPolygon"": [
          {
            ""x"": 163,
            ""y"": 1416
          },
          {
            ""x"": 170,
            ""y"": 1462
          },
          {
            ""x"": 159,
            ""y"": 1463
          },
          {
            ""x"": 152,
            ""y"": 1418
          }
        ],
        ""words"": [
          {
            ""text"": ""Graveyard:"",
            ""boundingPolygon"": [
              {
                ""x"": 164,
                ""y"": 1416
              },
              {
                ""x"": 169,
                ""y"": 1453
              },
              {
                ""x"": 159,
                ""y"": 1455
              },
              {
                ""x"": 153,
                ""y"": 1418
              }
            ],
            ""confidence"": 0.159
          },
          {
            ""text"": ""A"",
            ""boundingPolygon"": [
              {
                ""x"": 169,
                ""y"": 1456
              },
              {
                ""x"": 170,
                ""y"": 1461
              },
              {
                ""x"": 160,
                ""y"": 1463
              },
              {
                ""x"": 159,
                ""y"": 1457
              }
            ],
            ""confidence"": 0.994
          }
        ]
      },
      {
        ""text"": ""counter trom Baton of"",
        ""boundingPolygon"": [
          {
            ""x"": 136,
            ""y"": 1232
          },
          {
            ""x"": 152,
            ""y"": 1319
          },
          {
            ""x"": 139,
            ""y"": 1321
          },
          {
            ""x"": 124,
            ""y"": 1234
          }
        ],
        ""words"": [
          {
            ""text"": ""counter"",
            ""boundingPolygon"": [
              {
                ""x"": 136,
                ""y"": 1234
              },
              {
                ""x"": 141,
                ""y"": 1261
              },
              {
                ""x"": 129,
                ""y"": 1263
              },
              {
                ""x"": 125,
                ""y"": 1236
              }
            ],
            ""confidence"": 0.94
          },
          {
            ""text"": ""trom"",
            ""boundingPolygon"": [
              {
                ""x"": 141,
                ""y"": 1263
              },
              {
                ""x"": 144,
                ""y"": 1279
              },
              {
                ""x"": 132,
                ""y"": 1281
              },
              {
                ""x"": 129,
                ""y"": 1265
              }
            ],
            ""confidence"": 0.885
          },
          {
            ""text"": ""Baton"",
            ""boundingPolygon"": [
              {
                ""x"": 145,
                ""y"": 1283
              },
              {
                ""x"": 149,
                ""y"": 1304
              },
              {
                ""x"": 138,
                ""y"": 1307
              },
              {
                ""x"": 133,
                ""y"": 1285
              }
            ],
            ""confidence"": 0.577
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 149,
                ""y"": 1306
              },
              {
                ""x"": 152,
                ""y"": 1316
              },
              {
                ""x"": 140,
                ""y"": 1319
              },
              {
                ""x"": 138,
                ""y"": 1309
              }
            ],
            ""confidence"": 0.799
          }
        ]
      },
      {
        ""text"": ""the end of the festival, it was the only"",
        ""boundingPolygon"": [
          {
            ""x"": 149,
            ""y"": 1425
          },
          {
            ""x"": 169,
            ""y"": 1565
          },
          {
            ""x"": 156,
            ""y"": 1567
          },
          {
            ""x"": 137,
            ""y"": 1427
          }
        ],
        ""words"": [
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 150,
                ""y"": 1428
              },
              {
                ""x"": 151,
                ""y"": 1439
              },
              {
                ""x"": 139,
                ""y"": 1442
              },
              {
                ""x"": 137,
                ""y"": 1431
              }
            ],
            ""confidence"": 0.995
          },
          {
            ""text"": ""end"",
            ""boundingPolygon"": [
              {
                ""x"": 151,
                ""y"": 1441
              },
              {
                ""x"": 152,
                ""y"": 1455
              },
              {
                ""x"": 141,
                ""y"": 1458
              },
              {
                ""x"": 139,
                ""y"": 1444
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 152,
                ""y"": 1457
              },
              {
                ""x"": 153,
                ""y"": 1464
              },
              {
                ""x"": 142,
                ""y"": 1468
              },
              {
                ""x"": 141,
                ""y"": 1460
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 153,
                ""y"": 1467
              },
              {
                ""x"": 154,
                ""y"": 1478
              },
              {
                ""x"": 144,
                ""y"": 1482
              },
              {
                ""x"": 142,
                ""y"": 1470
              }
            ],
            ""confidence"": 0.994
          },
          {
            ""text"": ""festival,"",
            ""boundingPolygon"": [
              {
                ""x"": 155,
                ""y"": 1480
              },
              {
                ""x"": 158,
                ""y"": 1508
              },
              {
                ""x"": 149,
                ""y"": 1512
              },
              {
                ""x"": 144,
                ""y"": 1484
              }
            ],
            ""confidence"": 0.756
          },
          {
            ""text"": ""it"",
            ""boundingPolygon"": [
              {
                ""x"": 159,
                ""y"": 1510
              },
              {
                ""x"": 160,
                ""y"": 1516
              },
              {
                ""x"": 150,
                ""y"": 1519
              },
              {
                ""x"": 149,
                ""y"": 1514
              }
            ],
            ""confidence"": 0.917
          },
          {
            ""text"": ""was"",
            ""boundingPolygon"": [
              {
                ""x"": 160,
                ""y"": 1518
              },
              {
                ""x"": 162,
                ""y"": 1532
              },
              {
                ""x"": 153,
                ""y"": 1536
              },
              {
                ""x"": 151,
                ""y"": 1521
              }
            ],
            ""confidence"": 0.372
          },
          {
            ""text"": ""the"",
            ""boundingPolygon"": [
              {
                ""x"": 163,
                ""y"": 1535
              },
              {
                ""x"": 165,
                ""y"": 1546
              },
              {
                ""x"": 156,
                ""y"": 1549
              },
              {
                ""x"": 154,
                ""y"": 1538
              }
            ],
            ""confidence"": 0.993
          },
          {
            ""text"": ""only"",
            ""boundingPolygon"": [
              {
                ""x"": 165,
                ""y"": 1548
              },
              {
                ""x"": 168,
                ""y"": 1563
              },
              {
                ""x"": 160,
                ""y"": 1567
              },
              {
                ""x"": 156,
                ""y"": 1551
              }
            ],
            ""confidence"": 0.959
          }
        ]
      },
      {
        ""text"": ""Courage: Target cre"",
        ""boundingPolygon"": [
          {
            ""x"": 120,
            ""y"": 1176
          },
          {
            ""x"": 133,
            ""y"": 1244
          },
          {
            ""x"": 120,
            ""y"": 1246
          },
          {
            ""x"": 107,
            ""y"": 1179
          }
        ],
        ""words"": [
          {
            ""text"": ""Courage:"",
            ""boundingPolygon"": [
              {
                ""x"": 121,
                ""y"": 1177
              },
              {
                ""x"": 125,
                ""y"": 1205
              },
              {
                ""x"": 113,
                ""y"": 1208
              },
              {
                ""x"": 108,
                ""y"": 1180
              }
            ],
            ""confidence"": 0.564
          },
          {
            ""text"": ""Target"",
            ""boundingPolygon"": [
              {
                ""x"": 126,
                ""y"": 1207
              },
              {
                ""x"": 129,
                ""y"": 1228
              },
              {
                ""x"": 118,
                ""y"": 1232
              },
              {
                ""x"": 114,
                ""y"": 1211
              }
            ],
            ""confidence"": 0.762
          },
          {
            ""text"": ""cre"",
            ""boundingPolygon"": [
              {
                ""x"": 130,
                ""y"": 1231
              },
              {
                ""x"": 132,
                ""y"": 1243
              },
              {
                ""x"": 121,
                ""y"": 1246
              },
              {
                ""x"": 119,
                ""y"": 1234
              }
            ],
            ""confidence"": 0.777
          }
        ]
      },
      {
        ""text"": ""end of turn"",
        ""boundingPolygon"": [
          {
            ""x"": 111,
            ""y"": 1177
          },
          {
            ""x"": 116,
            ""y"": 1216
          },
          {
            ""x"": 104,
            ""y"": 1218
          },
          {
            ""x"": 98,
            ""y"": 1179
          }
        ],
        ""words"": [
          {
            ""text"": ""end"",
            ""boundingPolygon"": [
              {
                ""x"": 111,
                ""y"": 1177
              },
              {
                ""x"": 113,
                ""y"": 1188
              },
              {
                ""x"": 101,
                ""y"": 1190
              },
              {
                ""x"": 99,
                ""y"": 1179
              }
            ],
            ""confidence"": 0.295
          },
          {
            ""text"": ""of"",
            ""boundingPolygon"": [
              {
                ""x"": 113,
                ""y"": 1190
              },
              {
                ""x"": 114,
                ""y"": 1197
              },
              {
                ""x"": 102,
                ""y"": 1200
              },
              {
                ""x"": 101,
                ""y"": 1193
              }
            ],
            ""confidence"": 0.959
          },
          {
            ""text"": ""turn"",
            ""boundingPolygon"": [
              {
                ""x"": 114,
                ""y"": 1200
              },
              {
                ""x"": 117,
                ""y"": 1214
              },
              {
                ""x"": 105,
                ""y"": 1218
              },
              {
                ""x"": 103,
                ""y"": 1203
              }
            ],
            ""confidence"": 0.643
          }
        ]
      }
    ]
  }
            ";
		}
	}
}