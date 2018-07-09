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
            List<Card> cards = database.data.cards; //we only want a list of Card objects

            //Add missing ease-of-use values
            foreach (Card card in cards)
            {
                //Add clan_name
                switch (card.clan)
                {
                    case 0:
                        card.clan_name = "Neutral";
                        break;
                    case 1:
                        card.clan_name = "Forestcraft";
                        break;
                    case 2:
                        card.clan_name = "Swordcraft";
                        break;
                    case 3:
                        card.clan_name = "Runecraft";
                        break;
                    case 4:
                        card.clan_name = "Dragoncraft";
                        break;
                    case 5:
                        card.clan_name = "Shadowcraft";
                        break;
                    case 6:
                        card.clan_name = "Bloodcraft";
                        break;
                    case 7:
                        card.clan_name = "Havencraft";
                        break;
                    case 8:
                        card.clan_name = "Portalcraft";
                        break;
                }
                //Add char_type_name
                switch (card.char_type)
                {
                    case 1:
                        card.char_type_name = "Follower";
                        break;
                    case 2:
                    case 3:
                        card.char_type_name = "Amulet";
                        break;
                    case 4:
                        card.char_type_name = "Spell";
                        break;
                }
                //Add rarity_name
                switch (card.rarity)
                {
                    case 1:
                        card.rarity_name = "Bronze";
                        break;
                    case 2:
                        card.rarity_name = "Silver";
                        break;
                    case 3:
                        card.rarity_name = "Gold";
                        break;
                    case 4:
                        card.rarity_name = "Legendary";
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
            string JDeckUrl = "https://shadowverse-portal.com/api/v1/deck?format=json&lang=en&hash=" + deckHash.data.hash;
            string JDeck = client.DownloadString(JDeckUrl);
            RootObject JDeckObject = JsonConvert.DeserializeObject<RootObject>(JDeck);
            Deck deck = JDeckObject.data.deck;

            //Check deck format
            deck.deck_format_name = "Rotation";
            if (deck.deck_format == 2)
                deck.deck_format_name = "Take Two";
            else foreach (Card card in deck.cards)
                    if (card.format_type == false)
                    {
                        deck.deck_format_name = "Unlimited";
                        break;
                    }

            return deck;
        }
    }
}
