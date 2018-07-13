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

        //Constructor
        public CardBanner(int cardId, string cardName, int cardCost, int cardRarityId, int cardCount, bool isInDeck)
        {
            //Instance variables
            this.cardId = cardId;
            this.cardName = cardName;
            this.cardCost = cardCost;
            this.cardRarityId = cardRarityId;
            string backgroundImagePath = basePath + @"\img\cardBanner\" +cardId + ".jpg";
            string costImagePath = basePath + @"\img\cost\cost_" + cardCost + ".png";
            string rarityImagePath = basePath + @"\img\rarity\rarity_" + cardRarityId + ".png";
            this.cardCount = cardCount;
            this.isInDeck = isInDeck;

            //Fetch and display card info
            if (File.Exists(backgroundImagePath))
                BackgroundImage = Image.FromFile(backgroundImagePath);
            else BackgroundImage = Image.FromFile(basePath + @"\img\cardBanner\NoImage.jpg");
            InitializeComponent(); //this has to be before the following four lines
            cardNameLabel.Text = cardName;
            rarityLabel.Image = Image.FromFile(rarityImagePath);
            costLabel.Image = new Bitmap(Image.FromFile(costImagePath), new Size(22, 22));
            countLabel.Text = "×" + cardCount;

            //Make ENTIRE banner clickable
            foreach (Control control in Controls)
            {
                control.Click += CardBanner_Click;
            }
        }

        private void CardBanner_Load(object sender, EventArgs e)
        {

        }

        //Only event I care about tbh
        private void CardBanner_Click(object sender, EventArgs e)
        {
            //
            SVTracker target = (SVTracker)Parent.Parent;
            CardBanner banner = new CardBanner(cardId, cardName, cardCost, cardRarityId, cardCount, isInDeck);

            if (isInDeck)
            {
                banner.isInDeck = false;
                banner.countLabel.ResetText();
                target.AddToHand(banner.cardId, true);
                if (cardCount > 1)
                {
                    cardCount--;
                    countLabel.Text = "×" + cardCount;
                }
                else Dispose();
            }
            else
            {
                Dispose();
                target.PlayCard(banner.cardId);
            }
            banner.Dispose();
        }
    }
}
