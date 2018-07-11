using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace SVTracker
{
    public partial class CardBanner : UserControl
    {
        static string basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public int cardCount, cardId, cardCost, cardRarityId;
        public bool isInDeck;
        public string cardName;

        public CardBanner(int cardId, string cardName, int cardCost, int cardRarityId, int count, bool inDeck)
        {
            this.cardId = cardId;
            this.cardName = cardName;
            this.cardCost = cardCost;
            this.cardRarityId = cardRarityId;
            string backgroundImagePath = basePath + @"\img\cardBanner\" +cardId + ".jpg";
            if (File.Exists(backgroundImagePath))
                BackgroundImage = Image.FromFile(backgroundImagePath);
            else BackgroundImage = Image.FromFile(basePath + @"\img\cardBanner\NoImage.jpg");
            string costImagePath = basePath + @"\img\cost\cost_" + cardCost + ".png";
            string rarityImagePath = basePath + @"\img\rarity\rarity_" + cardRarityId + ".png";
            InitializeComponent();
            cardNameLabel.Text = cardName;
            rarityLabel.Image = Image.FromFile(rarityImagePath);
            costLabel.Image = new Bitmap(Image.FromFile(costImagePath), new Size(22, 22));
            costLabel.BringToFront();
            countLabel.Text = "×" + count;
            cardCount = count;
            isInDeck = inDeck;
            foreach (Control control in Controls)
            {
                control.Click += CardBanner_Click;
            }
        }

        private void CardBanner_Load(object sender, EventArgs e)
        {

        }

        private void CardBanner_Click(object sender, EventArgs e)
        {
            SVTracker target = (SVTracker)Parent.Parent;
            CardBanner banner = new CardBanner(cardId, cardName, cardCost, cardRarityId, cardCount, isInDeck);
            if (isInDeck)
            {
                banner.isInDeck = false;
                banner.countLabel.ResetText();
                target.AddToHand(banner);
                if (cardCount > 1)
                {
                    cardCount--;
                    countLabel.Text = "×" + cardCount;
                }
                else Dispose();
            }
            else
            {
                switch (cardId)
                {
                    case 107834020: //Biofabrication
                        //target.AddToDeck();
                        break;
                    case 106024010: //Mystic Ring
                        //
                        break;
                    case 107824020: //Metaproduction
                        target.AddToDeck(900811030); 
                        break;
                    case 107823010: //Ancient Amplifier
                        //two at random 900811010 900811020 900811030 900811040
                        break;
                    case 109813010: //Ancient Apparatus
                        //two at random 900811020 900811040 900811060
                        break;
                    case 100811070: //Magisteel Lion
                        target.AddToDeck(900811030);
                        target.AddToDeck(900811030);
                        break;
                    case 107811100: //Mech Wing Swordsman
                        //two at random 900811010 900811020 900811030 900811040
                        break;
                    case 107821030: //Icarus
                        target.AddToDeck(900811010);
                        target.AddToDeck(900811010);
                        break;
                    case 108811010: //Knower of History
                        target.AddToDeck(900811060);
                        break;
                    case 900821010: //L'Ange Miriam
                        target.AddToDeck(900811040);
                        target.AddToDeck(900811060);
                        break;
                    case 107811050: //Cat Cannoneer
                        target.AddToDeck(900811010);
                        target.AddToDeck(900811010);
                        break;
                    case 107811070: //Iron Staff Mechanic
                        target.AddToDeck(900811020);
                        target.AddToDeck(900811020);
                        break;
                    case 100811040: //Ironforged Fighter
                        target.AddToDeck(900811040);
                        target.AddToDeck(900811040);
                        break;
                    case 107811030: //Gravikinetic Warrior
                        //two at random 900811010 900811020 900811030 900811040
                        break;
                    case 108841030: //Electromagical Rhino
                        //special case, on swing:
                        target.AddToDeck(900841080);
                        target.AddToDeck(900841080);
                        target.AddToDeck(900841080);
                        break;
                    case 900841080: //Artifact Rhino
                        //special case, on swing:
                        target.AddToDeck(900841080);
                        target.AddToDeck(900841080);
                        target.AddToDeck(900841080);
                        break;
                    case 101041030: //Prince of Darkness
                        target.HolyFuckItsStan();
                        break;
                }
                Dispose();
            }
        }
    }
}
