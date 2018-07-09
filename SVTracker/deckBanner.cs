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
    public partial class DeckBanner : UserControl
    {
        static string basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string backgroundImagePath = basePath + @"\img\cardBanner\";
        string costImagePath = basePath + @"\img\cost\";

        public DeckBanner(Card card)
        {
            this.backgroundImagePath += card.CardId + ".jpg";
            if (File.Exists(backgroundImagePath))
                this.BackgroundImage = Image.FromFile(backgroundImagePath);
            else this.BackgroundImage = Image.FromFile(basePath + @"\img\cardBanner\NoImage.jpg");
            this.costImagePath += @"cost_" + card.Cost + ".png";
            this.cardNameLabel.Text = card.CardName;
            InitializeComponent();
        }

        private void DeckBanner_Load(object sender, EventArgs e)
        {

        }
    }
}
