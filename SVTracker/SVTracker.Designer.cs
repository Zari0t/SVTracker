namespace SVTracker
{
    partial class SVTracker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVTracker));
            this.getDeckButton = new System.Windows.Forms.Button();
            this.deckCodeInput = new System.Windows.Forms.TextBox();
            this.topPanel = new System.Windows.Forms.Panel();
            this.forceFetchEnJsonButton = new System.Windows.Forms.Button();
            this.forceFetchJpJsonButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.infoBox = new System.Windows.Forms.TextBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.deckCodeLabel = new System.Windows.Forms.Label();
            this.formatLabel = new System.Windows.Forms.Label();
            this.deckBannerList = new System.Windows.Forms.FlowLayoutPanel();
            this.handBannerList = new System.Windows.Forms.FlowLayoutPanel();
            this.handLabel = new System.Windows.Forms.Label();
            this.numberInDeckLabel = new System.Windows.Forms.Label();
            this.numberInHandLabel = new System.Windows.Forms.Label();
            this.resonanceLabel = new System.Windows.Forms.Label();
            this.shadowCountLabel = new System.Windows.Forms.Label();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // getDeckButton
            // 
            this.getDeckButton.Location = new System.Drawing.Point(107, 1);
            this.getDeckButton.Name = "getDeckButton";
            this.getDeckButton.Size = new System.Drawing.Size(70, 24);
            this.getDeckButton.TabIndex = 1;
            this.getDeckButton.Text = "Get Deck";
            this.getDeckButton.UseVisualStyleBackColor = true;
            this.getDeckButton.Click += new System.EventHandler(this.GetDeckButton_Click);
            // 
            // deckCodeInput
            // 
            this.deckCodeInput.AcceptsReturn = true;
            this.deckCodeInput.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.deckCodeInput.Cursor = System.Windows.Forms.Cursors.Default;
            this.deckCodeInput.Location = new System.Drawing.Point(3, 3);
            this.deckCodeInput.MaxLength = 4;
            this.deckCodeInput.Name = "deckCodeInput";
            this.deckCodeInput.Size = new System.Drawing.Size(100, 20);
            this.deckCodeInput.TabIndex = 0;
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.getDeckButton);
            this.topPanel.Controls.Add(this.deckCodeInput);
            this.topPanel.Controls.Add(this.forceFetchEnJsonButton);
            this.topPanel.Controls.Add(this.forceFetchJpJsonButton);
            this.topPanel.Controls.Add(this.resetButton);
            this.topPanel.Location = new System.Drawing.Point(12, 12);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(443, 26);
            this.topPanel.TabIndex = 2;
            // 
            // forceFetchEnJsonButton
            // 
            this.forceFetchEnJsonButton.Location = new System.Drawing.Point(188, 1);
            this.forceFetchEnJsonButton.Name = "forceFetchEnJsonButton";
            this.forceFetchEnJsonButton.Size = new System.Drawing.Size(81, 24);
            this.forceFetchEnJsonButton.TabIndex = 7;
            this.forceFetchEnJsonButton.Text = "Get Json (EN)";
            this.forceFetchEnJsonButton.UseVisualStyleBackColor = true;
            this.forceFetchEnJsonButton.Click += new System.EventHandler(this.ForceFetchEnJsonButton_Click);
            // 
            // forceFetchJpJsonButton
            // 
            this.forceFetchJpJsonButton.Location = new System.Drawing.Point(275, 1);
            this.forceFetchJpJsonButton.Name = "forceFetchJpJsonButton";
            this.forceFetchJpJsonButton.Size = new System.Drawing.Size(81, 24);
            this.forceFetchJpJsonButton.TabIndex = 2;
            this.forceFetchJpJsonButton.Text = "Get Json (JP)";
            this.forceFetchJpJsonButton.UseVisualStyleBackColor = true;
            this.forceFetchJpJsonButton.Click += new System.EventHandler(this.ForceFetchJpJsonButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(393, 1);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(48, 24);
            this.resetButton.TabIndex = 11;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // infoBox
            // 
            this.infoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.infoBox.Location = new System.Drawing.Point(12, 391);
            this.infoBox.Multiline = true;
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.Size = new System.Drawing.Size(441, 58);
            this.infoBox.TabIndex = 4;
            // 
            // exitButton
            // 
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(410, 363);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(43, 24);
            this.exitButton.TabIndex = 5;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // deckCodeLabel
            // 
            this.deckCodeLabel.AutoSize = true;
            this.deckCodeLabel.Location = new System.Drawing.Point(460, 18);
            this.deckCodeLabel.Name = "deckCodeLabel";
            this.deckCodeLabel.Size = new System.Drawing.Size(64, 13);
            this.deckCodeLabel.TabIndex = 8;
            this.deckCodeLabel.Text = "Deck Code:";
            this.deckCodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // formatLabel
            // 
            this.formatLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.formatLabel.Location = new System.Drawing.Point(672, 18);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(80, 13);
            this.formatLabel.TabIndex = 9;
            this.formatLabel.Text = "-";
            this.formatLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // deckBannerList
            // 
            this.deckBannerList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deckBannerList.AutoScroll = true;
            this.deckBannerList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.deckBannerList.Location = new System.Drawing.Point(463, 33);
            this.deckBannerList.Name = "deckBannerList";
            this.deckBannerList.Size = new System.Drawing.Size(289, 416);
            this.deckBannerList.TabIndex = 10;
            // 
            // handBannerList
            // 
            this.handBannerList.AutoScroll = true;
            this.handBannerList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.handBannerList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.handBannerList.Location = new System.Drawing.Point(12, 56);
            this.handBannerList.Name = "handBannerList";
            this.handBannerList.Size = new System.Drawing.Size(289, 232);
            this.handBannerList.TabIndex = 12;
            this.handBannerList.WrapContents = false;
            // 
            // handLabel
            // 
            this.handLabel.AutoSize = true;
            this.handLabel.Location = new System.Drawing.Point(12, 40);
            this.handLabel.Name = "handLabel";
            this.handLabel.Size = new System.Drawing.Size(70, 13);
            this.handLabel.TabIndex = 13;
            this.handLabel.Text = "Current Hand";
            // 
            // numberInDeckLabel
            // 
            this.numberInDeckLabel.AutoSize = true;
            this.numberInDeckLabel.Location = new System.Drawing.Point(310, 81);
            this.numberInDeckLabel.Name = "numberInDeckLabel";
            this.numberInDeckLabel.Size = new System.Drawing.Size(75, 13);
            this.numberInDeckLabel.TabIndex = 14;
            this.numberInDeckLabel.Text = "Cards in deck:";
            this.numberInDeckLabel.Visible = false;
            // 
            // numberInHandLabel
            // 
            this.numberInHandLabel.AutoSize = true;
            this.numberInHandLabel.Location = new System.Drawing.Point(310, 56);
            this.numberInHandLabel.Name = "numberInHandLabel";
            this.numberInHandLabel.Size = new System.Drawing.Size(75, 13);
            this.numberInHandLabel.TabIndex = 15;
            this.numberInHandLabel.Text = "Cards in hand:";
            this.numberInHandLabel.Visible = false;
            // 
            // resonanceLabel
            // 
            this.resonanceLabel.AutoSize = true;
            this.resonanceLabel.Location = new System.Drawing.Point(310, 94);
            this.resonanceLabel.Name = "resonanceLabel";
            this.resonanceLabel.Size = new System.Drawing.Size(98, 13);
            this.resonanceLabel.TabIndex = 16;
            this.resonanceLabel.Text = "Resonance Active!";
            this.resonanceLabel.Visible = false;
            // 
            // shadowCountLabel
            // 
            this.shadowCountLabel.AutoSize = true;
            this.shadowCountLabel.Location = new System.Drawing.Point(310, 117);
            this.shadowCountLabel.Name = "shadowCountLabel";
            this.shadowCountLabel.Size = new System.Drawing.Size(54, 13);
            this.shadowCountLabel.TabIndex = 17;
            this.shadowCountLabel.Text = "Shadows:";
            this.shadowCountLabel.Visible = false;
            // 
            // SVTracker
            // 
            this.AcceptButton = this.getDeckButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.exitButton;
            this.ClientSize = new System.Drawing.Size(764, 461);
            this.Controls.Add(this.shadowCountLabel);
            this.Controls.Add(this.resonanceLabel);
            this.Controls.Add(this.numberInHandLabel);
            this.Controls.Add(this.numberInDeckLabel);
            this.Controls.Add(this.handLabel);
            this.Controls.Add(this.handBannerList);
            this.Controls.Add(this.deckBannerList);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.deckCodeLabel);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.topPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SVTracker";
            this.Text = "SVTracker";
            this.Activated += new System.EventHandler(this.SVTracker_Activated);
            this.Load += new System.EventHandler(this.SVTracker_Load);
            this.Shown += new System.EventHandler(this.SVTracker_Shown);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getDeckButton;
        private System.Windows.Forms.TextBox deckCodeInput;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.TextBox infoBox;
        private System.Windows.Forms.Button forceFetchJpJsonButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button forceFetchEnJsonButton;
        private System.Windows.Forms.Label deckCodeLabel;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.FlowLayoutPanel deckBannerList;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.FlowLayoutPanel handBannerList;
        private System.Windows.Forms.Label handLabel;
        private System.Windows.Forms.Label numberInDeckLabel;
        private System.Windows.Forms.Label numberInHandLabel;
        private System.Windows.Forms.Label resonanceLabel;
        private System.Windows.Forms.Label shadowCountLabel;
    }
}

