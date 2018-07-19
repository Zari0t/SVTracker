using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SVTracker
{
    public partial class SVTrackerSplit : Form
    {
        string json = "";
        RootObject hash, database = new RootObject();
        Deck deck = new Deck();
        public DeckWindow deckWindow = new DeckWindow();
        List<Card> cards = new List<Card>();
        public int cardsInHand = 0, cardsInDeck = 0, shadowCount = 0;

        public SVTrackerSplit()
        {
            InitializeComponent();
        }

        private void SVTrackerSplit_Load(object sender, EventArgs e)
        {
            
        }
        private void SVTrackerSplit_Shown(object sender, EventArgs e)
        {
            //idk why this works, but this forces the app to check for cards.db being there AFTER opening the main window
            Application.DoEvents();
            Methods.JsonFetch(infoBox, false, 1);
            json = File.ReadAllText(Methods.JsonPathLocal);
            deckWindow = new DeckWindow(this);
            deckWindow.Hide();
        }

        //Kind of what the app is supposed to do
        private void GetDeckButton_Click(object sender, EventArgs e)
        {
            //Deck codes are always 4char long, so...
            if (deckCodeInput.TextLength == 4)
            {
                hash = Methods.GetDeckHash(deckCodeInput.Text);

                //Check if the deck code is valid
                if (hash.Data.Errors.Count == 0)
                {
                    deckWindow.Hide();
                    infoBox.AppendText("\r\n\r\nDeck code " + deckCodeInput.Text+" fetched successfully.\r\n");
                    deckWindow.deckBannerList.Controls.Clear();
                    handBannerList.Controls.Clear();

                    //We're listing cards from local database, so let's load it in here
                    database = JsonConvert.DeserializeObject<RootObject>(json);
                    cards = database.Data.Cards;

                    //Actually fetch the deck's contents, display info regarding it
                    deck = Methods.GetDeck(hash);
                    deckWindow.Text = deck.CraftName + " - " + deckCodeInput.Text;

                    Methods.DeckFilter(deck, deckWindow.deckBannerList);

                    cardsInHand = 0;
                    shadowCount = 0;
                    cardsInDeck = deck.Cards.Count;
                    numberInHandLabel.Text = "Cards in hand: " + cardsInHand;
                    numberInDeckLabel.Text = "Cards in deck: " + cardsInDeck;
                    shadowCountLabel.Text = "Shadows: " + shadowCount;
                    numberInHandLabel.Show();
                    numberInDeckLabel.Show();
                    shadowCountLabel.Show();
                    ResonanceCheck();
                    deckWindow.SetDesktopLocation(Location.X + Width - 15, Location.Y);
                    deckWindow.Show();
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
            deckCodeInput.SelectAll();
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

        //Resets everything to "game start"
        private void ResetButton_Click(object sender, EventArgs e)
        {
            cardsInHand = 0;
            shadowCount = 0;
            cardsInDeck = deck.Cards.Count;
            numberInHandLabel.Text = "Cards in hand: " + cardsInHand;
            numberInDeckLabel.Text = "Cards in deck: " + cardsInDeck;
            shadowCountLabel.Text = "Shadows: " + shadowCount;
            handBannerList.Controls.Clear();
            ResonanceCheck();
            Methods.DeckFilter (deck, deckWindow.deckBannerList);
        }

        private void SVTrackerSplit_Activated(object sender, EventArgs e)
        {
            deckCodeInput.Focus();
            deckCodeInput.SelectAll();
        }

        //Checks if Resonance is active or not
        public bool ResonanceCheck()
        {
            if (deck.CraftId == 8)
            {
                if (cardsInDeck % 2 == 0)
                {
                    resonanceLabel.Show();
                    return true;
                }
                else
                {
                    resonanceLabel.Hide();
                    return false;
                }
            }
            else return false;
        }

        public int NeuralCheck()
        {
            int neutralCount = 0;
            foreach (CardBanner card in handBannerList.Controls)
            {
                if (cards.Find(x => x.CardId == card.cardId).CraftId == 0)
                    neutralCount++;
            }
            return neutralCount;
        }

        //Adds a card to the player's hand
        public void AddToHand(int targetId, bool isDraw)
        {
            //Create instance of card to add
            Card targetCard = cards.Find(x => x.CardId == targetId);

            //Check hand size
            if (cardsInHand < 9)
            {
                CardBanner banner = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);

                //Each card is a separate banner, even if a player draws into multiples
                banner.countLabel.ResetText();

                //Put newly created CardBanner in hand
                handBannerList.Controls.Add(banner);
                if (isDraw)
                    infoBox.AppendText("\r\nDrew " + targetCard.CardName + ".");

                //Update numbers I guess
                cardsInHand++;
                numberInHandLabel.Text = "Cards in hand: " + cardsInHand;
            }
            //If hand was too full...
            else
            {
                shadowCount++;
                infoBox.AppendText("\r\nHand is too full! Overdrew " + targetCard.CardName + ".");
                shadowCountLabel.Text = "Shadows: " + shadowCount;
            }

            if (isDraw)
            {
                cardsInDeck--;
                numberInDeckLabel.Text = "Cards in deck: " + cardsInDeck;
                ResonanceCheck();
            }
        }

        //Adds a card to the player's deck
        public void AddToDeck(int targetId)
        {
            //control variable, it makes sense dw
            bool notInDeck = true;

            //Create instance of the card we're adding
            Card targetCard = cards.Find(x => x.CardId == targetId);

            //Check to see if the card is already in the deck
            //If it is, simply increase its count by 1
            foreach (CardBanner control in deckWindow.deckBannerList.Controls)
            {
                if (control.cardId == targetCard.CardId)
                {
                    control.cardCount++;
                    control.countLabel.Text = "×" + control.cardCount;
                    notInDeck = false;
                }
            }
            //If it isn't, add a new banner for it
            if (notInDeck)
            {
                CardBanner banner = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, true);
                deckWindow.deckBannerList.Controls.Add(banner);
            }

            //Update numbers I guess
            cardsInDeck++;
            numberInDeckLabel.Text = "Cards in deck: " + cardsInDeck;
            ResonanceCheck();
        }

        //-------------------------------------------------------------------

        //This is a horrible idea.

        //Check which card was played and...do things accordingly
        public void PlayCard(int sourceId)
        {
            //Create instance of the card being played
            if (cardsInHand > 0)
                cardsInHand--;
            shadowCount++;
            Methods.PlayCard(sourceId, this);

            //Update labels
            numberInHandLabel.Text = "Cards in hand: " + cardsInHand;
            shadowCountLabel.Text = "Shadows: " + shadowCount;
            ResonanceCheck();
        }

        //PoD aka Stan aka Satan
        public void HolyFuckItsStan()
        {
            //Delete deck contents
            deckWindow.deckBannerList.Controls.Clear();
            AddToDeck(900041010);                       //3x Servant of Darkness
            AddToDeck(900041010);
            AddToDeck(900041010);
            AddToDeck(900041020);                       //3x Silent Rider
            AddToDeck(900041020);
            AddToDeck(900041020);
            AddToDeck(900044010);                       //3x Dis's Damnation
            AddToDeck(900044010);
            AddToDeck(900044010);
            AddToDeck(900044020);                       //1x Astaroth's Reckoning
            infoBox.AppendText("Replaced deck with Apocalypse deck.");
            cardsInDeck = 10;
            numberInDeckLabel.Text = "Cards in deck: 10";
            //Here's to hoping this is the only special case I ever have to code in
        }

        //it wasn't
        public void PtpWithLegs()
        {
            for (int i = 0; i < 10; i++)
                    AddToHand(900511010, false);
            infoBox.AppendText("Added 10 Skeletons to hand.");
        }

        //Auxiliary stuff (because fuck writing the same thing repeatedly)
        public void AddFairy(int count)
        {
            for (int i=0; i<count; i++)
                AddToHand(900111010, false);
            if (count == 1)
                infoBox.AppendText("Added a Fairy to hand.");
            else infoBox.AppendText("Added "+ count + " Fairies to hand.");
        }

        public void AddPuppet(int count)
        {
            for (int i = 0; i < count; i++)
                AddToHand(900811050, false);
            if (count == 1)
                infoBox.AppendText("Added a Puppet to hand.");
            else infoBox.AppendText("Added " + count + " Puppets to hand.");
        }

        public void Necromancy(int cost)
        {
            if (shadowCount >= cost)
            {
                shadowCount -= cost;
                infoBox.AppendText("Necromancy (" + cost + ") activated.");
            }
        }
    }
}