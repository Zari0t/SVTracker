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
        int cardCount;
        enum CardBannerTag
        {
            InDeck = 0,
            InHand
        }

        public CardBanner(List<Card> db, Card card, int count)
        {
            string backgroundImagePath = basePath + @"\img\cardBanner\" +card.CardId + ".jpg";
            if (File.Exists(backgroundImagePath))
                BackgroundImage = Image.FromFile(backgroundImagePath);
            else BackgroundImage = Image.FromFile(basePath + @"\img\cardBanner\NoImage.jpg");
            string costImagePath = basePath + @"\img\cost\cost_" + card.Cost + ".png";
            string rarityImagePath = basePath + @"\img\rarity\rarity_" + card.RarityId + ".png";
            InitializeComponent();
            cardNameLabel.Text = card.CardName;
            rarityLabel.Image = Image.FromFile(rarityImagePath);
            costLabel.Image = new Bitmap(Image.FromFile(costImagePath), new Size(22, 22));
            costLabel.BringToFront();
            countLabel.Text = "×" + count;
            cardCount = count;
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
            if (cardCount > 1)
            {
                cardCount--;
                countLabel.Text = "×" + cardCount;
            }
            else Dispose();
        }
    }
}
