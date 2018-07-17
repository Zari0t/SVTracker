using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SVTracker
{
    public class Methods
    {
        public static string JsonPathLocal = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\cards.db";
        static WebClient client = new WebClient();

        //Check if cards.json database is present in local files, fetch it from SVAPI if not
        public static string JsonFetch(TextBox infoBox, bool forceDelete, int lang)
        {
            string JCardUrl = "https://shadowverse-portal.com/api/v1/cards?format=json&lang=";
            switch (lang)
            {
                case 1:
                    JCardUrl += "en";
                    break;
                case 2:
                    JCardUrl += "ja";
                    break;
            }
            if (forceDelete)
            {
                File.Delete(JsonPathLocal);
                infoBox.AppendText("\r\n\r\ncards.db deleted.");
            }
            if (!File.Exists(JsonPathLocal))
            {
                if (!forceDelete)
                    infoBox.Text = "cards.db not found.";
                infoBox.AppendText("\r\nFetching cards.db...");
                client.DownloadFile(JCardUrl, JsonPathLocal);
                infoBox.AppendText ("\r\ncards.db obtained.");
            }
            else infoBox.Text = "cards.db found.";

            return File.ReadAllText(JsonPathLocal);
        }

        //Create a list with all cards
        public static List<Card> GetCards()
        {
            //Convert cards.json into object
            string json = File.ReadAllText(JsonPathLocal);
            RootObject database = JsonConvert.DeserializeObject<RootObject>(json);
            List<Card> cards = database.Data.Cards; //we only want a list of Card objects

            //Add missing ease-of-use values
            foreach (Card card in cards)
            {
                //Add clan_name
                switch (card.CraftId)
                {
                    case 0:
                        card.CraftName = "Neutral";
                        break;
                    case 1:
                        card.CraftName = "Forestcraft";
                        break;
                    case 2:
                        card.CraftName = "Swordcraft";
                        break;
                    case 3:
                        card.CraftName = "Runecraft";
                        break;
                    case 4:
                        card.CraftName = "Dragoncraft";
                        break;
                    case 5:
                        card.CraftName = "Shadowcraft";
                        break;
                    case 6:
                        card.CraftName = "Bloodcraft";
                        break;
                    case 7:
                        card.CraftName = "Havencraft";
                        break;
                    case 8:
                        card.CraftName = "Portalcraft";
                        break;
                }
                //Add char_type_name
                switch (card.TypeId)
                {
                    case 1:
                        card.TypeName = "Follower";
                        break;
                    case 2:
                    case 3:
                        card.TypeName = "Amulet";
                        break;
                    case 4:
                        card.TypeName = "Spell";
                        break;
                }
                //Add rarity_name
                switch (card.RarityId)
                {
                    case 1:
                        card.RarityName = "Bronze";
                        break;
                    case 2:
                        card.RarityName = "Silver";
                        break;
                    case 3:
                        card.RarityName = "Gold";
                        break;
                    case 4:
                        card.RarityName = "Legendary";
                        break;
                }
            }

            return cards;
        }

        public static RootObject GetDeckHash(string deckCode)
        {
            //Fetch a deck's hash via 4-character deck code
            string JDeckCodeUrl = "https://shadowverse-portal.com/api/v1/deck/import?format=json&lang=en&deck_code=" +deckCode;
            string JDeckCode = client.DownloadString(JDeckCodeUrl);
            RootObject deckHash = JsonConvert.DeserializeObject<RootObject>(JDeckCode);
            
            return deckHash;
        }

        public static Deck GetDeck(RootObject deckHash)
        {
            //Use the previously acquired hash to fetch the deck itself
            string JDeckUrl = "https://shadowverse-portal.com/api/v1/deck?format=json&lang=en&hash=" + deckHash.Data.DeckHash;
            string JDeck = client.DownloadString(JDeckUrl);
            RootObject JDeckObject = JsonConvert.DeserializeObject<RootObject>(JDeck);
            Deck deck = JDeckObject.Data.Deck;

            //Check deck format
            deck.DeckFormatName = "Rotation";
            if (deck.DeckFormat == 2)
                deck.DeckFormatName = "Take Two";
            else foreach (Card card in deck.Cards)
                    if (card.FormatType == false)
                    {
                        deck.DeckFormatName = "Unlimited";
                        break;
                    }

            return deck;
        }

        public static void DeckFilter (Deck deck, FlowLayoutPanel target)
        {
            target.Controls.Clear();

            string json = File.ReadAllText(JsonPathLocal);
            RootObject database = JsonConvert.DeserializeObject<RootObject>(json);
            List<Card> cards = database.Data.Cards; //we only want a list of Card objects

            //Group duplicates
            var dup = deck.Cards
                .GroupBy(x => new { x.CardId })
                .Select(group => new { ID = group.Key, Count = group.Count() });

            //Show deck's contents
            target.Hide();
            foreach (var basex in dup)
            {
                Card targetCard = cards.Find(x => x.CardId == basex.ID.CardId);
                CardBanner banner = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, basex.Count, true);
                target.Controls.Add(banner);
            }
            target.Show();
        }

        public static void PlayCard (int sourceId, SVTrackerSplit target)
        {
            string json = File.ReadAllText(JsonPathLocal);
            RootObject database = JsonConvert.DeserializeObject<RootObject>(json);
            List<Card> cards = database.Data.Cards; //we only want a list of Card objects

            Card sourceCard = cards.Find(x => x.CardId == sourceId);
            if (sourceCard != null)
                target.infoBox.AppendText("\r\nPlayed " + sourceCard.CardName + ". ");

            //oh boy here we go
            switch (sourceCard.CardId)
            {
                //Neutral
                case 106024010:                         //Mystic Ring
                                                        //targetInHand.AddToDeck();
                    break;
                case 103021040:                         //Gourmet Emperor, Khaiza
                    target.AddToHand(900021010, false);
                    target.infoBox.AppendText("Added an Ultimate Carrot to hand.");
                    break;
                case 109011020:                         //Lowain of the Brofamily
                    target.AddToHand(900011050, false);
                    target.infoBox.AppendText("Added an Elsam of the Brofamily to hand.");
                    break;
                case 900011050:                         //Elsam of the Brofamily
                    target.AddToHand(900011060, false);
                    target.infoBox.AppendText("Added a Tomoi of the Brofamily to hand.");
                    break;
                case 900011060:                         //Tomoi of the Brofamily
                    target.AddToHand(900011070, false);
                    target.infoBox.AppendText("Added a Human! Pyramid! Attack! to hand.");
                    break;
                case 101031030:                         //Angel Crusher
                case 104434010:                         //Dragonslayer's Price (not Neutral!)
                case 103441010:                         //Imperial Dragoon (not Neutral!)
                    target.shadowCount += target.handBannerList.Controls.Count;
                    target.handBannerList.Controls.Clear();
                    target.infoBox.AppendText("Discarded all cards in hand.");
                    break;
                case 101041030:                         //Prince of Darkness
                   target.HolyFuckItsStan();
                    break;

                //Forest
                case 101114050:                         //Fairy Circle
                case 100111020:                         //Fairy Whisperer
                case 106124010:                         //Elf Song
                case 105123010:                         //Wood of Brambles
                case 105111010:                         //Flower Princess
                case 107111020:                         //Weald Philosopher
                case 102121040:                         //Baalt, King of the Elves
                    target.AddFairy(2);
                    break;
                case 100114010:                         //Sylvan Justice
                case 101121010:                         //Waltzing Fairy
                case 103111030:                         //Fairy Knight
                case 108111010:                         //Elf General
                    target.AddFairy(1);
                    break;
                case 101111080:                         //Fairy Caster
                    target.AddFairy(3);
                    break;
                case 108131030:                         //Storied Falconer
                case 103134010:                         //Selwyn's Command
                    target.AddToHand(900131010, false);
                    target.infoBox.AppendText("Added a Skystride Raptor to hand.");
                    break;
                case 106121020:                         //Ariana, Natural Tutor
                    target.AddToHand(104114010, false);
                    target.infoBox.AppendText("Added an Ivy Spellbomb to hand.");
                    break;
                case 107141010:                         //Aria, Guiding Fairy
                    target.AddToHand(900111020, false);
                    target.infoBox.AppendText("Added a Fairy Wisp to hand.");
                    break;
                case 106131010:                         //Venus
                    target.AddToHand(101122010, false);
                    target.infoBox.AppendText("Added a Harvest Festival to hand.");
                    break;
                case 101141010:                         //Fairy Princess
                    target.AddFairy(9 - target.cardsInHand);
                    break;
                case 101141030:                         //Rose Queen
                    foreach (CardBanner cardBanner in target.handBannerList.Controls)
                    {
                        if (cardBanner.cardId == 900111010)
                        {
                            cardBanner.Dispose();
                            target.AddToHand(900144010, false);
                        }
                    }
                    target.infoBox.AppendText("Transformed all Fairies in hand into Thorn Bursts.");
                    break;

                //Sword
                case 109214010:                         //Grand Auction
                    target.AddToHand(109223010, false);
                    target.infoBox.AppendText("Added an Ageworn Weaponry to hand.");
                    break;
                case 103231020:                         //Amelia, Silver Paladin
                    target.AddToHand(103221030, false);
                    target.infoBox.AppendText("Added a Gelt, Vice Captain to hand.");
                    break;

                //Rune
                case 103321030:                         //Dwarf Alchemist
                    target.AddToHand(900312010, false);
                    target.infoBox.AppendText("Added an Earth Essence to hand.");
                    break;
                case 105334010:                         //Golem Assault
                    target.AddToHand(900314010, false);
                    target.infoBox.AppendText("Added a Conjure Guardian to hand.");
                    break;
                case 101334030:                         //First Curse
                    target.AddToHand(900334010, false);
                    target.infoBox.AppendText("Added a Second Curse to hand.");
                    break;
                case 107314020:                         //Chain Lightning
                    if (target.cardsInHand <= 4)
                    {
                        target.AddToHand(107314020, false);
                        target.infoBox.AppendText("Added a Chain Lightning to hand.");
                    }
                    break;
                case 103321020:                         //Disaster Witch
                    target.AddToHand(900314020, false);
                    target.infoBox.AppendText("Added a Crimson Sorcery to hand.");
                    break;
                case 900321010:                         //Mysterian Whitewyrm
                    target.AddToHand(900314050, false);
                    target.infoBox.AppendText("Added a Mysterian Circle to hand.");
                    break;
                case 900321020:                         //Mysterian Blackwyrm
                    target.AddToHand(900314040, false);
                    target.infoBox.AppendText("Added a Mysterian Missile to hand.");
                    break;
                case 900334010:                         //Second Curse
                    target.AddToHand(900334020, false);
                    target.infoBox.AppendText("Added a Final Curse to hand.");
                    break;
                case 103341010:                         //Daria, Dimensional Witch
                    target.handBannerList.Controls.Clear();
                    target.infoBox.AppendText("Banished all cards in hand.");
                    break;

                //Dragon
                case 108441030:                         //Whitefrost Dragon, Filene
                    target.AddToHand(900444010, false);
                    target.infoBox.AppendText("Added a Whitefrost Whisper to hand.");
                    break;
                case 109431020:                         //Pyrewyrm Commander
                    target.AddToHand(100414020, false);
                    target.infoBox.AppendText("Added a Blazing Breath to hand.");
                    break;
                case 106441020:                         //Python
                    foreach (CardBanner cardBanner in target.deckWindow.deckBannerList.Controls)
                    {
                        if (cardBanner.cardCost <= 7)
                        {
                            cardBanner.Dispose();
                        }
                    }
                    target.infoBox.AppendText("Banished all 7pp or less cost cards from deck.");
                    break;
                case 109441010:                         //Zooey, Arbiter of the Skies
                    target.AddToDeck(900441030);
                    target.infoBox.AppendText("Added a 10pp 6/5 Zooey, Arbiter of the Skies to the deck.");
                    break;

                //Shadow
                case 101521010:                         //Skeleton Fighter
                    target.Necromancy(1);
                    break;
                case 100514010:                         //Undying Resentment
                case 103511060:                         //Zombie Buccaneer
                    target.Necromancy(2);
                    break;
                case 108511030:                         //Mischievous Spirit
                    target.Necromancy(3);
                    break;
                case 101511010:                         //Skeleton Viper
                case 101511110:                         //Lesser Mummy
                case 101524020:                         //Ethereal Form
                case 100511030:                         //Apprentice Necromancer
                case 101531010:                         //Wight King
                case 101521080:                         //Soulhungry Wraith
                case 102511040:                         //Commander of Destruction
                case 100514020:                         //Call of the Void
                case 101511070:                         //Cursed Soldier
                case 101511100:                         //Orcus
                case 102521020:                         //Andras
                    target.Necromancy(4);
                    break;
                case 900521030:                         //Fran's Attendant
                case 101521020:                         //Deadly Widow
                case 105521010:                         //Usher of Styx
                case 101534020:                         //Foul Tempest
                case 101534030:                         //Death's Breath
                case 109541020:                         //Mordecai, Eternal Duelist
                    target.Necromancy(6);
                    break;
                case 107541010:                         //Underworld Ruler Aisha
                    target.Necromancy(10);
                    break;
                case 101531040:                         //Deathly Tyrant
                    target.Necromancy(20);
                    break;
                case 100511010:                         //Spartoi Sergeant
                case 101514030:                         //Soul Hunt
                    target.shadowCount++;
                    target.infoBox.AppendText("Gained 1 shadow.");
                    break;
                case 100511040:                         //Elder Spartoi Soldier
                    target.shadowCount++;
                    target.shadowCount++;
                    target.infoBox.AppendText("Gained 2 shadows.");
                    break;
                case 101541010:                         //Cerberus
                    target.AddToHand(900544010, false);
                    target.AddToHand(900544020, false);
                    break;
                case 108541020:                         //Hinterland Ghoul
                    target.PtpWithLegs();
                    break;

                //Blood
                case 108641030:                         //Waltz, King of Wolves
                    target.AddToHand(104633010, false);
                    target.infoBox.AppendText("Added a Blood Moon to hand.");
                    break;
                case 107641020:                         //Diabolus Psema
                    target.AddToHand(101014030, false);
                    target.AddToHand(100624010, false);
                    target.infoBox.AppendText("Added a Demonic Strike and a Demonic Storm to hand.");
                    break;

                //Haven
                case 108722010:                         //Temple of the Holy Lion
                case 108721010:                         //Peaceweaver
                case 109724010:                         //Prism Swing
                    target.AddToHand(108722010, false);
                    target.infoBox.AppendText("Added a Holy Lion Crystal to hand.");
                    break;
                case 105711020:                         //Monk of Purification
                    target.AddToHand(102714040, false);
                    target.AddToHand(102714040, false);
                    target.infoBox.AppendText("Added two Monastic Holy Waters to hand.");
                    break;

                //Portal
                case 107824020:                         //Metaproduction
                    target.AddToDeck(900811030);
                    target.infoBox.AppendText("Added an Analyzing Artifact to the deck.");
                    break;
                case 107823010:                         //Ancient Amplifier
                case 107811100:                         //Mech Wing Swordsman
                case 107811030:                         //Gravikinetic Warrior
                                                        //two at random 900811010 900811020 900811030 900811040
                    break;
                case 109813010:                         //Ancient Apparatus
                                                        //two at random 900811020 900811040 900811060
                    break;
                case 100811070:                         //Magisteel Lion
                    target.AddToDeck(900811030);
                    target.AddToDeck(900811030);
                    target.infoBox.AppendText("Added two Analyzing Artifacts to the deck.");
                    break;
                case 107821030:                         //Icarus
                case 107811050:                         //Cat Cannoneer
                    target.AddToDeck(900811010);
                    target.AddToDeck(900811010);
                    target.infoBox.AppendText("Added two Ancient Artifacts to the deck.");
                    break;
                case 108811010:                         //Knower of History
                    target.AddToDeck(900811060);
                    target.infoBox.AppendText("Added a Prime Artifact to the deck.");
                    break;
                case 900821010:                         //L'Ange Miriam
                    target.AddToDeck(900811040);
                    target.AddToDeck(900811060);
                    target.infoBox.AppendText("Added a Radiant Artifact and a Prime Artifact to the deck.");
                    break;
                case 100811010:                         //Toy Soldier
                case 108811030:                         //Junk
                case 107824010:                         //Substitution
                case 107813020:                         //Puppet Room
                case 107811120:                         //Masked Puppet
                case 107821100:                         //Automaton Soldier
                case 108834010:                         //Heartless Battle
                case 107821060:                         //Vengefull Puppeteer Noah
                    target.AddPuppet(1);
                    break;
                case 100824010:                         //Puppeteer's Strings
                    target.AddPuppet(2);
                    break;
                case 108841010:                         //Orchis, Puppet Girl
                    target.AddPuppet(3);
                    break;
                case 107811070:                         //Iron Staff Mechanic
                    target.AddToDeck(900811020);
                    target.AddToDeck(900811020);
                    target.infoBox.AppendText("Added two Mystic Artifacts to the deck.");
                    break;
                case 100811040:                         //Ironforged Fighter
                    target.AddToDeck(900811040);
                    target.AddToDeck(900811040);
                    target.infoBox.AppendText("Added two Radiant Artifacts to the deck.");
                    break;
                case 109811030:                         //Enkidu
                    if (target.cardsInDeck % 2 == 0)
                    {
                        target.AddToHand(101021030, false);
                        target.infoBox.AppendText("\r\nResonance effect activated. Added a Gilgamesh to hand.");
                    }
                    break;

                //Literally everything else - I THINK I'm not missing anything(?)
                //Manually maintaining this is going to be a pain in the ass and I regret it already
                default:
                    break;
            }
        }

        public static void PlayCard(int sourceId, SVTracker target)
        {
            string json = File.ReadAllText(JsonPathLocal);
            RootObject database = JsonConvert.DeserializeObject<RootObject>(json);
            List<Card> cards = database.Data.Cards; //we only want a list of Card objects

            Card sourceCard = cards.Find(x => x.CardId == sourceId);
            if (sourceCard != null)
                target.infoBox.AppendText("\r\nPlayed " + sourceCard.CardName + ". ");

            //oh boy here we go
            switch (sourceCard.CardId)
            {
                //Neutral
                case 106024010:                         //Mystic Ring
                                                        //targetInHand.AddToDeck();
                    break;
                case 103021040:                         //Gourmet Emperor, Khaiza
                    target.AddToHand(900021010, false);
                    target.infoBox.AppendText("Added an Ultimate Carrot to hand.");
                    break;
                case 109011020:                         //Lowain of the Brofamily
                    target.AddToHand(900011050, false);
                    target.infoBox.AppendText("Added an Elsam of the Brofamily to hand.");
                    break;
                case 900011050:                         //Elsam of the Brofamily
                    target.AddToHand(900011060, false);
                    target.infoBox.AppendText("Added a Tomoi of the Brofamily to hand.");
                    break;
                case 900011060:                         //Tomoi of the Brofamily
                    target.AddToHand(900011070, false);
                    target.infoBox.AppendText("Added a Human! Pyramid! Attack! to hand.");
                    break;
                case 101031030:                         //Angel Crusher
                case 104434010:                         //Dragonslayer's Price (not Neutral!)
                case 103441010:                         //Imperial Dragoon (not Neutral!)
                    target.shadowCount += target.handBannerList.Controls.Count;
                    target.handBannerList.Controls.Clear();
                    target.infoBox.AppendText("Discarded all cards in hand.");
                    break;
                case 101041030:                         //Prince of Darkness
                    target.HolyFuckItsStan();
                    break;

                //Forest
                case 101114050:                         //Fairy Circle
                case 100111020:                         //Fairy Whisperer
                case 106124010:                         //Elf Song
                case 105123010:                         //Wood of Brambles
                case 105111010:                         //Flower Princess
                case 107111020:                         //Weald Philosopher
                case 102121040:                         //Baalt, King of the Elves
                    target.AddFairy(2);
                    break;
                case 100114010:                         //Sylvan Justice
                case 101121010:                         //Waltzing Fairy
                case 103111030:                         //Fairy Knight
                case 108111010:                         //Elf General
                    target.AddFairy(1);
                    break;
                case 101111080:                         //Fairy Caster
                    target.AddFairy(3);
                    break;
                case 108131030:                         //Storied Falconer
                case 103134010:                         //Selwyn's Command
                    target.AddToHand(900131010, false);
                    target.infoBox.AppendText("Added a Skystride Raptor to hand.");
                    break;
                case 106121020:                         //Ariana, Natural Tutor
                    target.AddToHand(104114010, false);
                    target.infoBox.AppendText("Added an Ivy Spellbomb to hand.");
                    break;
                case 107141010:                         //Aria, Guiding Fairy
                    target.AddToHand(900111020, false);
                    target.infoBox.AppendText("Added a Fairy Wisp to hand.");
                    break;
                case 106131010:                         //Venus
                    target.AddToHand(101122010, false);
                    target.infoBox.AppendText("Added a Harvest Festival to hand.");
                    break;
                case 101141010:                         //Fairy Princess
                    target.AddFairy(9 - target.cardsInHand);
                    break;
                case 101141030:                         //Rose Queen
                    foreach (CardBanner cardBanner in target.handBannerList.Controls)
                    {
                        if (cardBanner.cardId == 900111010)
                        {
                            cardBanner.Dispose();
                            target.AddToHand(900144010, false);
                        }
                    }
                    target.infoBox.AppendText("Transformed all Fairies in hand into Thorn Bursts.");
                    break;

                //Sword
                case 109214010:                         //Grand Auction
                    target.AddToHand(109223010, false);
                    target.infoBox.AppendText("Added an Ageworn Weaponry to hand.");
                    break;
                case 103231020:                         //Amelia, Silver Paladin
                    target.AddToHand(103221030, false);
                    target.infoBox.AppendText("Added a Gelt, Vice Captain to hand.");
                    break;

                //Rune
                case 103321030:                         //Dwarf Alchemist
                    target.AddToHand(900312010, false);
                    target.infoBox.AppendText("Added an Earth Essence to hand.");
                    break;
                case 105334010:                         //Golem Assault
                    target.AddToHand(900314010, false);
                    target.infoBox.AppendText("Added a Conjure Guardian to hand.");
                    break;
                case 101334030:                         //First Curse
                    target.AddToHand(900334010, false);
                    target.infoBox.AppendText("Added a Second Curse to hand.");
                    break;
                case 107314020:                         //Chain Lightning
                    if (target.cardsInHand <= 4)
                    {
                        target.AddToHand(107314020, false);
                        target.infoBox.AppendText("Added a Chain Lightning to hand.");
                    }
                    break;
                case 103321020:                         //Disaster Witch
                    target.AddToHand(900314020, false);
                    target.infoBox.AppendText("Added a Crimson Sorcery to hand.");
                    break;
                case 900321010:                         //Mysterian Whitewyrm
                    target.AddToHand(900314050, false);
                    target.infoBox.AppendText("Added a Mysterian Circle to hand.");
                    break;
                case 900321020:                         //Mysterian Blackwyrm
                    target.AddToHand(900314040, false);
                    target.infoBox.AppendText("Added a Mysterian Missile to hand.");
                    break;
                case 900334010:                         //Second Curse
                    target.AddToHand(900334020, false);
                    target.infoBox.AppendText("Added a Final Curse to hand.");
                    break;
                case 103341010:                         //Daria, Dimensional Witch
                    target.handBannerList.Controls.Clear();
                    target.infoBox.AppendText("Banished all cards in hand.");
                    break;

                //Dragon
                case 108441030:                         //Whitefrost Dragon, Filene
                    target.AddToHand(900444010, false);
                    target.infoBox.AppendText("Added a Whitefrost Whisper to hand.");
                    break;
                case 109431020:                         //Pyrewyrm Commander
                    target.AddToHand(100414020, false);
                    target.infoBox.AppendText("Added a Blazing Breath to hand.");
                    break;
                case 106441020:                         //Python
                    foreach (CardBanner cardBanner in target.deckBannerList.Controls)
                    {
                        if (cardBanner.cardCost <= 7)
                        {
                            cardBanner.Dispose();
                        }
                    }
                    target.infoBox.AppendText("Banished all 7pp or less cost cards from deck.");
                    break;
                case 109441010:                         //Zooey, Arbiter of the Skies
                    target.AddToDeck(900441030);
                    target.infoBox.AppendText("Added a 10pp 6/5 Zooey, Arbiter of the Skies to the deck.");
                    break;

                //Shadow
                case 101521010:                         //Skeleton Fighter
                    target.Necromancy(1);
                    break;
                case 100514010:                         //Undying Resentment
                case 103511060:                         //Zombie Buccaneer
                    target.Necromancy(2);
                    break;
                case 108511030:                         //Mischievous Spirit
                    target.Necromancy(3);
                    break;
                case 101511010:                         //Skeleton Viper
                case 101511110:                         //Lesser Mummy
                case 101524020:                         //Ethereal Form
                case 100511030:                         //Apprentice Necromancer
                case 101531010:                         //Wight King
                case 101521080:                         //Soulhungry Wraith
                case 102511040:                         //Commander of Destruction
                case 100514020:                         //Call of the Void
                case 101511070:                         //Cursed Soldier
                case 101511100:                         //Orcus
                case 102521020:                         //Andras
                    target.Necromancy(4);
                    break;
                case 900521030:                         //Fran's Attendant
                case 101521020:                         //Deadly Widow
                case 105521010:                         //Usher of Styx
                case 101534020:                         //Foul Tempest
                case 101534030:                         //Death's Breath
                case 109541020:                         //Mordecai, Eternal Duelist
                    target.Necromancy(6);
                    break;
                case 107541010:                         //Underworld Ruler Aisha
                    target.Necromancy(10);
                    break;
                case 101531040:                         //Deathly Tyrant
                    target.Necromancy(20);
                    break;
                case 100511010:                         //Spartoi Sergeant
                case 101514030:                         //Soul Hunt
                    target.shadowCount++;
                    target.infoBox.AppendText("Gained 1 shadow.");
                    break;
                case 100511040:                         //Elder Spartoi Soldier
                    target.shadowCount++;
                    target.shadowCount++;
                    target.infoBox.AppendText("Gained 2 shadows.");
                    break;
                case 101541010:                         //Cerberus
                    target.AddToHand(900544010, false);
                    target.AddToHand(900544020, false);
                    break;
                case 108541020:                         //Hinterland Ghoul
                    target.PtpWithLegs();
                    break;

                //Blood
                case 108641030:                         //Waltz, King of Wolves
                    target.AddToHand(104633010, false);
                    target.infoBox.AppendText("Added a Blood Moon to hand.");
                    break;
                case 107641020:                         //Diabolus Psema
                    target.AddToHand(101014030, false);
                    target.AddToHand(100624010, false);
                    target.infoBox.AppendText("Added a Demonic Strike and a Demonic Storm to hand.");
                    break;

                //Haven
                case 108722010:                         //Temple of the Holy Lion
                case 108721010:                         //Peaceweaver
                case 109724010:                         //Prism Swing
                    target.AddToHand(108722010, false);
                    target.infoBox.AppendText("Added a Holy Lion Crystal to hand.");
                    break;
                case 105711020:                         //Monk of Purification
                    target.AddToHand(102714040, false);
                    target.AddToHand(102714040, false);
                    target.infoBox.AppendText("Added two Monastic Holy Waters to hand.");
                    break;

                //Portal
                case 107824020:                         //Metaproduction
                    target.AddToDeck(900811030);
                    target.infoBox.AppendText("Added an Analyzing Artifact to the deck.");
                    break;
                case 107823010:                         //Ancient Amplifier
                case 107811100:                         //Mech Wing Swordsman
                case 107811030:                         //Gravikinetic Warrior
                                                        //two at random 900811010 900811020 900811030 900811040
                    break;
                case 109813010:                         //Ancient Apparatus
                                                        //two at random 900811020 900811040 900811060
                    break;
                case 100811070:                         //Magisteel Lion
                    target.AddToDeck(900811030);
                    target.AddToDeck(900811030);
                    target.infoBox.AppendText("Added two Analyzing Artifacts to the deck.");
                    break;
                case 107821030:                         //Icarus
                case 107811050:                         //Cat Cannoneer
                    target.AddToDeck(900811010);
                    target.AddToDeck(900811010);
                    target.infoBox.AppendText("Added two Ancient Artifacts to the deck.");
                    break;
                case 108811010:                         //Knower of History
                    target.AddToDeck(900811060);
                    target.infoBox.AppendText("Added a Prime Artifact to the deck.");
                    break;
                case 900821010:                         //L'Ange Miriam
                    target.AddToDeck(900811040);
                    target.AddToDeck(900811060);
                    target.infoBox.AppendText("Added a Radiant Artifact and a Prime Artifact to the deck.");
                    break;
                case 100811010:                         //Toy Soldier
                case 108811030:                         //Junk
                case 107824010:                         //Substitution
                case 107813020:                         //Puppet Room
                case 107811120:                         //Masked Puppet
                case 107821100:                         //Automaton Soldier
                case 108834010:                         //Heartless Battle
                case 107821060:                         //Vengefull Puppeteer Noah
                    target.AddPuppet(1);
                    break;
                case 100824010:                         //Puppeteer's Strings
                    target.AddPuppet(2);
                    break;
                case 108841010:                         //Orchis, Puppet Girl
                    target.AddPuppet(3);
                    break;
                case 107811070:                         //Iron Staff Mechanic
                    target.AddToDeck(900811020);
                    target.AddToDeck(900811020);
                    target.infoBox.AppendText("Added two Mystic Artifacts to the deck.");
                    break;
                case 100811040:                         //Ironforged Fighter
                    target.AddToDeck(900811040);
                    target.AddToDeck(900811040);
                    target.infoBox.AppendText("Added two Radiant Artifacts to the deck.");
                    break;
                case 109811030:                         //Enkidu
                    if (target.cardsInDeck % 2 == 0)
                    {
                        target.AddToHand(101021030, false);
                        target.infoBox.AppendText("\r\nResonance effect activated. Added a Gilgamesh to hand.");
                    }
                    break;

                //Literally everything else - I THINK I'm not missing anything(?)
                //Manually maintaining this is going to be a pain in the ass and I regret it already
                default:
                    break;
            }
        }
    }
}
