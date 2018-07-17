namespace SVTracker
{
    partial class DeckWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.deckBannerList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // deckBannerList
            // 
            this.deckBannerList.AutoSize = true;
            this.deckBannerList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.deckBannerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deckBannerList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.deckBannerList.Location = new System.Drawing.Point(0, 0);
            this.deckBannerList.Margin = new System.Windows.Forms.Padding(0);
            this.deckBannerList.Name = "deckBannerList";
            this.deckBannerList.Size = new System.Drawing.Size(184, 61);
            this.deckBannerList.TabIndex = 0;
            // 
            // DeckWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(184, 61);
            this.ControlBox = false;
            this.Controls.Add(this.deckBannerList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeckWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "deckCode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel deckBannerList;
    }
}