using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SVTracker
{
    public class Methods
    {
        public static string JsonPathLocal = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\cards.db";
        static WebClient client = new WebClient();

        //Check if cards.json database is present in local files, fetch it from SVAPI if not
        public static void JsonFetch(TextBox infoBox, bool forceDelete, int lang)
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
    }
}
