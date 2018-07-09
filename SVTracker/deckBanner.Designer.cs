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
            this.SuspendLayout();
            // 
            // cardNameLabel
            // 
            this.cardNameLabel.AutoEllipsis = true;
            this.cardNameLabel.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cardNameLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cardNameLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cardNameLabel.Location = new System.Drawing.Point(56, 16);
            this.cardNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.cardNameLabel.Name = "cardNameLabel";
            this.cardNameLabel.Size = new System.Drawing.Size(180, 14);
            this.cardNameLabel.TabIndex = 0;
            this.cardNameLabel.Text = "Card Name";
            // 
            // DeckBanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.cardNameLabel);
            this.Name = "DeckBanner";
            this.Size = new System.Drawing.Size(270, 46);
            this.Load += new System.EventHandler(this.DeckBanner_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label cardNameLabel;
    }
}
