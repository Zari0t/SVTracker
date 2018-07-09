namespace SVTracker
{
    partial class DeckBanner
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cardNameLabel = new System.Windows.Forms.Label();
            this.rarityLabel = new System.Windows.Forms.Label();
            this.costLabel = new System.Windows.Forms.Label();
            this.countLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cardNameLabel
            // 
            this.cardNameLabel.AutoEllipsis = true;
            this.cardNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.cardNameLabel.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cardNameLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cardNameLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cardNameLabel.Location = new System.Drawing.Point(60, 16);
            this.cardNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.cardNameLabel.Name = "cardNameLabel";
            this.cardNameLabel.Size = new System.Drawing.Size(176, 14);
            this.cardNameLabel.TabIndex = 0;
            this.cardNameLabel.Text = "Card Name";
            // 
            // rarityLabel
            // 
            this.rarityLabel.BackColor = System.Drawing.Color.Transparent;
            this.rarityLabel.ForeColor = System.Drawing.Color.Transparent;
            this.rarityLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rarityLabel.Location = new System.Drawing.Point(0, 28);
            this.rarityLabel.Margin = new System.Windows.Forms.Padding(0);
            this.rarityLabel.Name = "rarityLabel";
            this.rarityLabel.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.rarityLabel.Size = new System.Drawing.Size(56, 20);
            this.rarityLabel.TabIndex = 1;
            this.rarityLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // costLabel
            // 
            this.costLabel.BackColor = System.Drawing.Color.Transparent;
            this.costLabel.ForeColor = System.Drawing.Color.Transparent;
            this.costLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.costLabel.Location = new System.Drawing.Point(0, 0);
            this.costLabel.Margin = new System.Windows.Forms.Padding(0);
            this.costLabel.Name = "costLabel";
            this.costLabel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.costLabel.Size = new System.Drawing.Size(56, 27);
            this.costLabel.TabIndex = 2;
            this.costLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // countLabel
            // 
            this.countLabel.AutoEllipsis = true;
            this.countLabel.BackColor = System.Drawing.Color.Transparent;
            this.countLabel.Font = new System.Drawing.Font("Verdana", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.countLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.countLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.countLabel.Location = new System.Drawing.Point(240, 16);
            this.countLabel.Margin = new System.Windows.Forms.Padding(0);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(30, 14);
            this.countLabel.TabIndex = 3;
            this.countLabel.Text = "×";
            // 
            // DeckBanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.rarityLabel);
            this.Controls.Add(this.costLabel);
            this.Controls.Add(this.cardNameLabel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DeckBanner";
            this.Size = new System.Drawing.Size(270, 46);
            this.Load += new System.EventHandler(this.DeckBanner_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label cardNameLabel;
        private System.Windows.Forms.Label costLabel;
        private System.Windows.Forms.Label rarityLabel;
        private System.Windows.Forms.Label countLabel;
    }
}
