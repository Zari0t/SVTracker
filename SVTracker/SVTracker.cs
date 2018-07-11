using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVTracker
{
    public partial class SVTracker : Form
    {
        string json = "";
        RootObject hash, database = new RootObject();
        Deck deck = new Deck();
        List<Card> cards = new List<Card>();

        public SVTracker()
        {
            InitializeComponent();
        }

        private void SVTracker_Load(object sender, EventArgs e)
        {
            
        }
        private void SVTracker_Shown(object sender, EventArgs e)
        {
            //idk why this works, but this forces the app to check for cards.db being there AFTER opening the main window
            Application.DoEvents();
            Methods.JsonFetch(infoBox, false, 1);
            json = File.ReadAllText(Methods.JsonPathLocal);
        }

        private void GetDeckButton_Click(object sender, EventArgs e)
        {
            //Deck codes are always 4char long, so...
            if (deckCodeInput.TextLength == 4)
            {
                hash = Methods.GetDeckHash(deckCodeInput.Text);

                //Check if the deck code is valid
                if (hash.Data.Errors.Count == 0)
                {
                    infoBox.AppendText("\r\n\r\nDeck code " + deckCodeInput.Text+" fetched successfully.");
                    deckBannerList.Controls.Clear();

                    //We're listing cards from local database, so let's load it in here
                    database = JsonConvert.DeserializeObject<RootObject>(json);
                    cards = database.Data.Cards;

                    //Actually fetch the deck's contents, display info regarding it
                    deck = Methods.GetDeck(hash);
                    deckCodeLabel.Text = "Deck Code: "+deckCodeInput.Text;
                    formatLabel.Text = deck.DeckFormatName;

                    Methods.DeckFilter(deck, deckBannerList);
                }

                //Lists error code and message if the deck code was invalid for some reason
                else
                {
                    infoBox.AppendText("\r\n\r\n"+hash.Data.Errors[0].ErrorType);
                    infoBox.AppendText("\r\n"+hash.Data.Errors[0].ErrorMessage);
                }
            }

            // :V
            else
                infoBox.AppendText("\r\n\r\nDeck code must be 4 characters long.");
        }

        private void ForceFetchEnJsonButton_Click(object sender, EventArgs e)
        {
            json = Methods.JsonFetch(infoBox, true, 1);
        }

        private void ForceFetchJpJsonButton_Click(object sender, EventArgs e)
        {
            json = Methods.JsonFetch(infoBox, true, 2);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            handBannerList.Controls.Clear();
            Methods.DeckFilter (deck, deckBannerList);
        }

        private void SVTracker_Activated(object sender, EventArgs e)
        {
            deckCodeInput.Focus();
            deckCodeInput.SelectAll();
        }

        public void AddToHand(CardBanner banner)
        {
            handBannerList.Controls.Add(banner);
        }

        public void AddToDeck(int targetId)
        {
            bool notInDeck = true;
            Card targetCard = cards.Find(x => x.CardId == targetId);
            List<CardBanner> currentDeck = new List<CardBanner>();
            foreach (CardBanner control in deckBannerList.Controls)
            {
                if (control.cardId == targetCard.CardId)
                {
                    control.cardCount++;
                    control.countLabel.Text = "×" + control.cardCount;
                    notInDeck = false;
                }
                currentDeck.Add(control);
            }
            if (notInDeck)
            {
                CardBanner banner = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, true);
                currentDeck.Add(banner);
                deckBannerList.Controls.Add(banner);
            }
        }

        public void HolyFuckItsStan()
        {
            deckBannerList.Controls.Clear();
            AddToDeck(900041010);
            AddToDeck(900041010);
            AddToDeck(900041010);
            AddToDeck(900041020);
            AddToDeck(900041020);
            AddToDeck(900041020);
            AddToDeck(900044010);
            AddToDeck(900044010);
            AddToDeck(900044010);
            AddToDeck(900044020);
        }
    }
}
