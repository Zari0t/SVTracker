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
        RootObject hash = new RootObject();
        Deck deck = new Deck();

        public SVTracker()
        {
            InitializeComponent();
        }

        private void SVTracker_Load(object sender, EventArgs e)
        {
            
        }
        private void SVTracker_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            Methods.JsonFetch(infoBox, false, 1);
        }
        
        private void getDeckButton_Click(object sender, EventArgs e)
        {
            if (deckCodeInput.TextLength == 4)
            {
                hash = Methods.GetDeckHash(deckCodeInput.Text);
                if (hash.data.errors.Count == 0)
                {
                    infoBox.AppendText("\r\n\r\nDeck code " + deckCodeInput.Text+" fetched successfully.");
                    listBox1.Items.Clear();
                    deck = Methods.GetDeck(hash);
                    foreach (Card card in deck.cards)
                    {
                        listBox1.Items.Add("[" + card.card_id + "] " + card.card_name);
                    }
                }
                else
                {
                    infoBox.AppendText("\r\n\r\n"+hash.data.errors[0].type);
                    infoBox.AppendText("\r\n"+hash.data.errors[0].message);
                }
            }
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
