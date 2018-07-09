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

        private void getDeckButton_Click(object sender, EventArgs e)
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

                    //Group duplicates
                    var dup = deck.Cards
                        .GroupBy(x => new { x.CardId })
                        .Select(group => new { ID = group.Key, Count = group.Count() });

                    //Show deck's contents
                    deckBannerList.Hide();
                    foreach (var basex in dup)
                    {
                        Card targetCard = cards.Find(x => x.BaseCardId == basex.ID.CardId);
                        DeckBanner banner = new DeckBanner(targetCard, basex.Count);
                        deckBannerList.Controls.Add(banner);
                    }
                    deckBannerList.Show();
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

        private void forceFetchEnJsonButton_Click(object sender, EventArgs e)
        {
            Methods.JsonFetch(infoBox, true, 1);
        }

        private void forceFetchJpJsonButton_Click(object sender, EventArgs e)
        {
            Methods.JsonFetch(infoBox, true, 2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SVTracker_Activated(object sender, EventArgs e)
        {
            deckCodeInput.Focus();
        }
    }
}
