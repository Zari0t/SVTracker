using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVTracker
{
    public partial class DeckWindow : Form
    {
        public SVTrackerSplit mainWindow;

        public DeckWindow() { }

        public DeckWindow(SVTrackerSplit mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
        }
    }
}
