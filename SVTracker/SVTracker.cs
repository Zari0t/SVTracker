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
                if (hash.data.errors.Count == 0)
                {
                    infoBox.AppendText("\r\n\r\nDeck code " + deckCodeInput.Text+" fetched successfully.");
                    listBox1.Items.Clear();

                    //We're listing cards from local database, so let's load it in here
                    database = JsonConvert.DeserializeObject<RootObject>(json);
                    cards = database.data.cards;

                    //Actually fetch the deck's contents, display info regarding it
                    deck = Methods.GetDeck(hash);
                    deckCodeLabel.Text = "Deck Code: "+deckCodeInput.Text;
                    formatLabel.Text = deck.deck_format_name;

                    //Group duplicates
                    var dup = deck.cards
                        .GroupBy(x => new { x.card_id })
                        .Select(group => new { ID = group.Key, Count = group.Count() });

                    //Show deck's contents
                    foreach (var basex in dup)
                    {
                        string cardName = cards.Find(x => x.base_card_id == basex.ID.card_id).card_name;
                        listBox1.Items.Add("[" + basex.ID.card_id + "] " + cardName + " x"+basex.Count);
                    }
                }

                //Lists error code and message if the deck code was invalid for some reason
                else
                {
                    infoBox.AppendText("\r\n\r\n"+hash.data.errors[0].type);
                    infoBox.AppendText("\r\n"+hash.data.errors[0].message);
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
