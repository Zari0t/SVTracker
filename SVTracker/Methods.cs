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

            //Add clan_name
            switch (deck.CraftId)
            {
                case 1:
                    deck.CraftName = "Forestcraft";
                    break;
                case 2:
                    deck.CraftName = "Swordcraft";
                    break;
                case 3:
                    deck.CraftName = "Runecraft";
                    break;
                case 4:
                    deck.CraftName = "Dragoncraft";
                    break;
                case 5:
                    deck.CraftName = "Shadowcraft";
                    break;
                case 6:
                    deck.CraftName = "Bloodcraft";
                    break;
                case 7:
                    deck.CraftName = "Havencraft";
                    break;
                case 8:
                    deck.CraftName = "Portalcraft";
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
            int count = 0;
            DialogResult dr;

            Card sourceCard = cards.Find(x => x.CardId == sourceId);
            if (sourceCard != null)
                target.infoBox.AppendText("\r\nPlayed " + sourceCard.CardName + ". ");

            ChooseDialog choose = new ChooseDialog();
            CardBanner choice = new CardBanner();
            Card targetCard = new Card();

            //oh boy here we go
            switch (sourceCard.CardId)
            {
                //Neutral
                case 106024010:                         //Mystic Ring
                    foreach (CardBanner possibleTarget in target.handBannerList.Controls)
                    {
                        targetCard = cards.Find(x => x.CardId == possibleTarget.cardId);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                    }
                    if (choose.choiceBannerList.Controls.Count > 0)
                    {
                        choose.ShowDialog();
                        target.AddToDeck(choose.choice);
                        target.infoBox.AppendText("Added " + cards.Find(x => x.CardId == choose.choice).CardName + " to the deck. ");
                    }
                    else
                    {
                        target.AddToHand(107834020, false);
                        target.infoBox.AppendText("No valid targets. Put Mystic Ring back in hand.");
                    }
                    break;
                case 107024010:                         //Treasure Map
                    targetCard = cards.Find(x => x.CardId == 101113010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 101232010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 104313010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 101432020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 104522010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 101623010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 103723010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Added a " + cards.Find(x => x.CardId == choose.choice).CardName + " to hand.");
                    break;
                case 103021040:                         //Gourmet Emperor, Khaiza
                    target.AddToHand(900021010, false);
                    target.infoBox.AppendText("Added an Ultimate Carrot to hand.");
                    break;
                case 107031020:                         //Goblin Emperor
                    targetCard = cards.Find(x => x.CardId == 100011010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 105011030);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 104021020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Added a " + cards.Find(x => x.CardId == choose.choice).CardName + " to hand.");
                    break;
                case 107041020:                         //Badb Catha
                    dr = MessageBox.Show("Was Badb Catha played for 9pp","Enhance Effect", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            target.AddToHand(900044030, false);
                            target.infoBox.AppendText("Added a Morrigna's Gospel to hand.");
                            break;
                        case DialogResult.No:
                            break;
                    }
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
                case 105041020:                         //Queen of the Dread Sea
                    foreach (CardBanner possibleTarget in target.handBannerList.Controls)
                    {
                        targetCard = cards.Find(x => x.CardId == possibleTarget.cardId && x.CraftId == 0);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                    }
                    choose.ShowDialog();
                    Card targetOne = cards.Find(x => x.CardId == choose.choice);
                    foreach (CardBanner possibleTarget in target.handBannerList.Controls)
                    {
                        targetCard = cards.Find(x => x.CardId == possibleTarget.cardId && x.CraftId != 0);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                    }
                    choose.ShowDialog();
                    target.handBannerList.Controls.Clear();
                    target.AddToHand(targetOne.CardId, false);
                    target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Discarded all cards in hand except one Neutral and one non-Neutral.");
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
                    if (sourceCard.CardId == 105111010) //Flower Princess specific
                    {
                        if (target.NeuralCheck() > 2)
                        {
                            target.AddToHand(900144010, false);
                            target.infoBox.AppendText("Added a Thorn Burst to hand.");
                        }
                    }
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
                case 109113010:                         //Fairy Refuge
                    targetCard = cards.Find(x => x.CardId == 109113010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900113010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900113020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    if (choose.choice != 109113010)
                        target.PlayCard(choose.choice);
                    break;
                case 108131030:                         //Storied Falconer
                case 103134010:                         //Selwyn's Command
                    target.AddToHand(900131010, false);
                    target.infoBox.AppendText("Added a Skystride Raptor to hand.");
                    if (sourceCard.CardId == 103134010)
                    {
                        dr = MessageBox.Show("Was Selwyn's Command played for 8pp", "Enhance Effect", MessageBoxButtons.YesNo);
                        switch (dr)
                        {
                            case DialogResult.Yes:
                                target.AddToHand(102131020, false);
                                target.infoBox.AppendText("Added a Grand Archer Selwyn to hand.");
                                break;
                            case DialogResult.No:
                                break;
                        }
                    }
                    break;
                case 108131010:                         //Paula, Icy Warmth
                    targetCard = cards.Find(x => x.CardId == 108131010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900131020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900131030);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    if (choose.choice != 108131010)
                        target.PlayCard(choose.choice);
                    break;
                case 109131010:                         //Shamu and Shama, Noblekin
                    targetCard = cards.Find(x => x.CardId == 900134010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900134020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Added a " + cards.Find(x => x.CardId == choose.choice).CardName + " to hand.");
                    break;
                case 106121020:                         //Ariana, Natural Tutor
                    target.AddToHand(104114010, false);
                    target.infoBox.AppendText("Added an Ivy Spellbomb to hand.");
                    break;
                case 107141010:                         //Aria, Guiding Fairy
                    target.AddToHand(900111020, false);
                    target.infoBox.AppendText("Added a Fairy Wisp to hand.");
                    dr = MessageBox.Show("Was Aria played for 9pp", "Enhance Effect", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            target.AddFairy(9 - target.cardsInHand);
                            break;
                        case DialogResult.No:
                            break;
                    }
                    break;
                case 108121010:                         //Fashionista Nelcha
                    targetCard = cards.Find(x => x.CardId == 900124010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900124020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Added a(n) " + cards.Find(x => x.CardId == choose.choice).CardName + " to hand.");
                    break;
                case 106131010:                         //Venus
                    target.AddToHand(101122010, false);
                    target.infoBox.AppendText("Added a Harvest Festival to hand.");
                    break;
                case 109141010:                         //Korwa, Ravishing Designer
                    dr = MessageBox.Show("Was Korwa played for 8pp", "Enhance Effect", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            for (int i = 0; i < 3; i++)
                            {
                                target.AddToHand(900144040, false);
                            }
                            target.infoBox.AppendText("Added three Fils to hand.");
                            break;
                        case DialogResult.No:
                            break;
                    }
                    break;
                case 101141010:                         //Fairy Princess
                    target.AddFairy(9 - target.cardsInHand);
                    break;
                case 104141010:                         //Elf Queen
                    target.infoBox.AppendText("Restored " + target.shadowCount + " defense to your leader. Set shadows to 0.");
                    target.shadowCount = 0;
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
                case 108234010:                         //Chromatic Duel
                    dr = MessageBox.Show("Was Chromatic Duel played for 8pp", "Enhance Effect", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            target.AddToHand(900231010, false);
                            target.AddToHand(900231020, false);
                            target.infoBox.AppendText("Added a Queen Hemera the White and a Queen Magnus the Black to hand.");
                            break;
                        case DialogResult.No:
                            targetCard = cards.Find(x => x.CardId == 900231010);
                            choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                            choose.choiceBannerList.Controls.Add(choice);
                            targetCard = cards.Find(x => x.CardId == 900231020);
                            choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                            choose.choiceBannerList.Controls.Add(choice);
                            choose.ShowDialog();
                            target.AddToHand(choose.choice, false);
                            target.infoBox.AppendText("Added a " + cards.Find(x => x.CardId == choose.choice).CardName + " to hand.");
                            break;
                    }
                    break;
                case 109214010:                         //Grand Auction
                    target.AddToHand(109223010, false);
                    target.infoBox.AppendText("Added an Ageworn Weaponry to hand.");
                    break;
                case 108231010:                         //Innocent Princess Prim
                    dr = MessageBox.Show("Was Prim played for 8pp", "Enhance Effect", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            target.AddToHand(108221010, false);
                            target.infoBox.AppendText("Added a Nonja, Silent Maid to hand.");
                            break;
                        case DialogResult.No:
                            break;
                    }
                    break;
                case 108241010:                         //Sky Commander Celia
                    targetCard = cards.Find(x => x.CardId == 108241010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900241020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900241030);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    if (choose.choice != 108241010)
                        target.PlayCard(choose.choice);
                    break;
                case 109223010:                         //Ageworn Weaponry
                    targetCard = cards.Find(x => x.CardId == 109223010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900223010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900223020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    if (choose.choice != 108241010)
                        target.PlayCard(choose.choice);
                    break;
                case 108221030:                         //Valse, Magical Marksman
                    targetCard = cards.Find(x => x.CardId == 900224020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900224010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Added a " + cards.Find(x => x.CardId == choose.choice).CardName + " to hand.");
                    break;
                case 103231020:                         //Amelia, Silver Paladin
                    target.AddToHand(103221030, false);
                    target.infoBox.AppendText("Added a Gelt, Vice Captain to hand.");
                    break;
                case 109231010:                         //Dei, Secret Agent
                    targetCard = cards.Find(x => x.CardId == 900231030);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900234010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Added an " + cards.Find(x => x.CardId == choose.choice).CardName + " to hand.");
                    break;
                case 109234010:                         //Dragon Knights
                    count = 1;
                    dr = MessageBox.Show("Was Dragon Knights played for 8pp", "Enhance Effect", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            count++;
                            break;
                        case DialogResult.No:
                            break;
                    }
                    for (int i=0; i<count; i++)
                    {
                        targetCard = cards.Find(x => x.CardId == 900231040);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900231050);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900231060);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900231070);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        choose.ShowDialog();
                        target.PlayCard(choose.choice);
                    }
                    break;

                //Rune
                case 107324010:                         //Mysterian Knowledge
                    targetCard = cards.Find(x => x.CardId == 900314040);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900314050);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Added a " + cards.Find(x => x.CardId == choose.choice).CardName + " to hand.");
                    break;
                case 109332010:                         //Mystic Rune
                    targetCard = cards.Find(x => x.CardId == 109332010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900332010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900332020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    if (choose.choice != 109332010)
                        target.PlayCard(choose.choice);
                    break;
                case 101311060:                         //Apprentice Alchemist
                case 101311070:                         //Master Alchemist
                    dr = MessageBox.Show("Did Earth Rite activate?", "Earth Rite", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            target.AddToHand(900314010, false);
                            break;
                        case DialogResult.No:
                            break;
                    }
                    break;
                case 103321030:                         //Dwarf Alchemist
                    target.AddToHand(900312010, false);
                    target.infoBox.AppendText("Added an Earth Essence to hand.");
                    break;
                case 108321020:                         //Mysterian Wyrmist
                    targetCard = cards.Find(x => x.CardId == 108321020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900321010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900321020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    if (choose.choice != 108321020)
                        target.PlayCard(choose.choice);
                    break;
                case 105334010:                         //Golem Assault
                    count = 1;
                    if (sourceId == 105334010)
                    {
                        dr = MessageBox.Show("Was Golem Assault played for 6pp", "Enhance Effect", MessageBoxButtons.YesNo);
                        switch (dr)
                        {
                            case DialogResult.Yes:
                                count = 3;
                                break;
                            case DialogResult.No:
                                break;
                        }
                    }
                    for (int i=0; i<count; i++)
                        target.AddToHand(900314010, false);
                    target.infoBox.AppendText("Added " + count + " Conjure Guardian to hand.");
                    break;
                case 101334030:                         //First Curse
                    target.AddToHand(900334010, false);
                    target.infoBox.AppendText("Added a Second Curse to hand.");
                    break;
                case 103334010:                         //Secrets of Erasmus
                    DialogResult dr8 = MessageBox.Show("Was Secrets of Erasmus played for 9pp", "Enhance Effect", MessageBoxButtons.YesNo);
                    switch (dr8)
                    {
                        case DialogResult.Yes:
                            target.PlayCard(101341030);
                            break;
                        case DialogResult.No:
                            break;
                    }
                    break;
                case 108341010:                         //Runie, Destiny's Bard
                    targetCard = cards.Find(x => x.CardId == 900344010);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    targetCard = cards.Find(x => x.CardId == 900344020);
                    choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                    choose.choiceBannerList.Controls.Add(choice);
                    choose.ShowDialog();
                    target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Added a " + cards.Find(x => x.CardId == choose.choice).CardName + " to hand.");
                    break;
                case 101331010:                         //Ancient Alchemist
                    dr = MessageBox.Show("Did Earth Rite activate?", "Earth Rite", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            for (int i=0;i<3;i++)
                                target.AddToHand(900314010, false);
                            break;
                        case DialogResult.No:
                            break;
                    }
                    target.infoBox.AppendText("Added 3 Conjure Guardian to hand.");
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
                case 104341020:                         //Hulking Giant
                    foreach (CardBanner card in target.handBannerList.Controls)
                    {
                        targetCard = cards.Find(x => x.CardId == card.cardId);
                        if (targetCard.Trait == "Earth Sigil")
                        {
                            card.Dispose();
                            target.shadowCount++;
                        }
                    }
                    target.infoBox.AppendText("Discarded all Earth Sigils.");
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
                case 109321010:                         //Abyss Summoner
                    if (target.cardsInDeck <= 20)
                    {
                        targetCard = cards.Find(x => x.CardId == 900321030);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900321040);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        choose.ShowDialog();
                        target.PlayCard(choose.choice);
                    }
                    break;
                case 103341010:                         //Daria, Dimensional Witch
                    target.handBannerList.Controls.Clear();
                    target.infoBox.AppendText("Banished all cards in hand.");
                    break;
                case 108341020:                         //Unbodied Witch
                    foreach (CardBanner card in target.handBannerList.Controls)
                    {
                        targetCard = cards.Find(x => x.CardId == card.cardId);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                    }
                    choose.ShowDialog();
                    while (target.cardsInHand < 9)
                        target.AddToHand(choose.choice, false);
                    target.infoBox.AppendText("Filled hand with copies of " + cards.Find(x => x.CardId == choose.choice).CardName + ".");
                    break;

                //Dragon
                case 107414010:                         //Stormborne Wings
                    foreach (CardBanner card in target.handBannerList.Controls)
                    {
                        targetCard = cards.Find(x => x.CardId == card.cardId);
                        if (targetCard.CraftId == 4)
                        {
                            choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                            choose.choiceBannerList.Controls.Add(choice);
                        }
                    }
                    choose.ShowDialog();
                    foreach (CardBanner card in target.handBannerList.Controls)
                    {
                        if (card.cardId == choose.choice)
                        {
                            card.Dispose();
                            target.AddToHand(900411020, false);
                            break;
                        }
                    }
                    target.infoBox.AppendText("Transformed " + cards.Find(x => x.CardId == choose.choice).CardName + " in hand into a Windblast Dragon.");
                    break;
                case 107424010:                         //Dragon Horde
                    count = 1;
                    dr = MessageBox.Show("Was Dragon Horde played for 3pp", "Enhance Effect", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            count++;
                            break;
                        case DialogResult.No:
                            break;
                    }
                    for (int i=0; i<count; i++)
                        target.AddToHand(101411020, false);
                    target.infoBox.AppendText("Added " + count + " Windblast Dragon to hand.");
                    break;
                case 106411010:                         //Dragonreader Matilda
                    target.AddToHand(101411110, false);
                    target.infoBox.AppendText("Added a Fire Lizard to hand.");
                    break;
                case 107421010:                         //Marion, Fierce Dragonewt
                    foreach (CardBanner card in target.handBannerList.Controls)
                    {
                        targetCard = cards.Find(x => x.CardId == card.cardId);
                        if (targetCard.CraftId == 4)
                        {
                            choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                            choose.choiceBannerList.Controls.Add(choice);
                        }
                    }
                    choose.ShowDialog();
                    foreach (CardBanner card in target.handBannerList.Controls)
                    {
                        if (card.cardId == choose.choice)
                        {
                            card.Dispose();
                            target.AddToHand(100414020, false);
                            break;
                        }
                    }
                    target.infoBox.AppendText("Transformed " + cards.Find(x => x.CardId == choose.choice).CardName + " in hand into a Blazing Breath.");
                    break;
                case 108421010:                         //Heroic Dragonslayer
                    foreach (CardBanner card in target.handBannerList.Controls)
                    {
                        targetCard = cards.Find(x => x.CardId == card.cardId);
                        if (targetCard.CraftId == 4)
                        {
                            choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                            choose.choiceBannerList.Controls.Add(choice);
                        }
                    }
                    choose.ShowDialog();
                    foreach (CardBanner card in target.handBannerList.Controls)
                    {
                        if (card.cardId == choose.choice)
                        {
                            card.Dispose();
                            target.AddToHand(900414010, false);
                            break;
                        }
                    }
                    target.infoBox.AppendText("Transformed " + cards.Find(x => x.CardId == choose.choice).CardName + " in hand into a Dragon's Handspur.");
                    break;
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
                case 107834020:                         //Biofabrication
                    foreach (CardBanner possibleTarget in target.handBannerList.Controls)
                    {
                        targetCard = cards.Find(x => x.CardId == possibleTarget.cardId);
                        if (targetCard.Trait == "Artifact")
                        {
                            choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                            choose.choiceBannerList.Controls.Add(choice);
                        }
                    }
                    if (choose.choiceBannerList.Controls.Count > 0)
                    {
                        choose.ShowDialog();
                        for (int i = 0; i < 3; i++)
                            target.AddToDeck(choose.choice);
                        target.infoBox.AppendText("Added three " + cards.Find(x => x.CardId == choose.choice).CardName + "s to the deck. ");
                    }
                    else
                    {
                        target.AddToHand(107834020, false);
                        target.infoBox.AppendText("No valid targets. Put Biofabrication back in hand.");
                    }
                    break;
                case 107824020:                         //Metaproduction
                    target.AddToDeck(900811030);
                    target.infoBox.AppendText("Added an Analyzing Artifact to the deck.");
                    break;
                case 107823010:                         //Ancient Amplifier
                case 107811100:                         //Mech Wing Swordsman
                case 107811030:                         //Gravikinetic Warrior
                    for (int i=0; i<2; i++)
                    {
                        targetCard = cards.Find(x => x.CardId == 900811010);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900811020);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900811030);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900811040);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        choose.ShowDialog();
                        target.AddToDeck(choose.choice);
                        target.infoBox.AppendText("Added a "+cards.Find(x => x.CardId == choose.choice).CardName+" to the deck. ");
                    }
                    break;
                case 109813010:                         //Ancient Apparatus
                    for (int i = 0; i < 2; i++)
                    {
                        targetCard = cards.Find(x => x.CardId == 900811020);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900811040);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900811060);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        choose.ShowDialog();
                        target.AddToDeck(choose.choice);
                        target.infoBox.AppendText("Added a " + cards.Find(x => x.CardId == choose.choice).CardName + " to the deck. ");
                    }
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
                case 108821010:                         //Miriam, Synthetic Being
                    if (target.ResonanceCheck())
                    {
                        targetCard = cards.Find(x => x.CardId == 900821010);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        targetCard = cards.Find(x => x.CardId == 900821020);
                        choice = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);
                        choose.choiceBannerList.Controls.Add(choice);
                        choose.ShowDialog();
                        target.PlayCard(choose.choice);
                    }
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

            choose.Dispose();
        }



        //------------------------------------------------



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
