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
            this.getDeckButton = new System.Windows.Forms.Button();
            this.deckCodeInput = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.infoBox = new System.Windows.Forms.TextBox();
            this.forceFetchJpJsonButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.forceFetchEnJsonButton = new System.Windows.Forms.Button();
            this.deckCodeLabel = new System.Windows.Forms.Label();
            this.formatLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
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
            this.getDeckButton.Click += new System.EventHandler(this.getDeckButton_Click);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.getDeckButton);
            this.panel1.Controls.Add(this.deckCodeInput);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(180, 26);
            this.panel1.TabIndex = 2;
            // 
            // infoBox
            // 
            this.infoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.infoBox.Location = new System.Drawing.Point(12, 330);
            this.infoBox.Multiline = true;
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.Size = new System.Drawing.Size(460, 127);
            this.infoBox.TabIndex = 4;
            // 
            // forceFetchJpJsonButton
            // 
            this.forceFetchJpJsonButton.Location = new System.Drawing.Point(285, 13);
            this.forceFetchJpJsonButton.Name = "forceFetchJpJsonButton";
            this.forceFetchJpJsonButton.Size = new System.Drawing.Size(81, 24);
            this.forceFetchJpJsonButton.TabIndex = 2;
            this.forceFetchJpJsonButton.Text = "Get Json (JP)";
            this.forceFetchJpJsonButton.UseVisualStyleBackColor = true;
            this.forceFetchJpJsonButton.Click += new System.EventHandler(this.forceFetchJpJsonButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(433, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 24);
            this.button1.TabIndex = 5;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(482, 37);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(270, 420);
            this.listBox1.TabIndex = 6;
            // 
            // forceFetchEnJsonButton
            // 
            this.forceFetchEnJsonButton.Location = new System.Drawing.Point(198, 13);
            this.forceFetchEnJsonButton.Name = "forceFetchEnJsonButton";
            this.forceFetchEnJsonButton.Size = new System.Drawing.Size(81, 24);
            this.forceFetchEnJsonButton.TabIndex = 7;
            this.forceFetchEnJsonButton.Text = "Get Json (EN)";
            this.forceFetchEnJsonButton.UseVisualStyleBackColor = true;
            this.forceFetchEnJsonButton.Click += new System.EventHandler(this.forceFetchEnJsonButton_Click);
            // 
            // deckCodeLabel
            // 
            this.deckCodeLabel.AutoSize = true;
            this.deckCodeLabel.Location = new System.Drawing.Point(479, 18);
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
            // SVTracker
            // 
            this.AcceptButton = this.getDeckButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 469);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.deckCodeLabel);
            this.Controls.Add(this.forceFetchEnJsonButton);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.forceFetchJpJsonButton);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.panel1);
            this.Name = "SVTracker";
            this.Text = "SVTracker";
            this.Activated += new System.EventHandler(this.SVTracker_Activated);
            this.Load += new System.EventHandler(this.SVTracker_Load);
            this.Shown += new System.EventHandler(this.SVTracker_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getDeckButton;
        private System.Windows.Forms.TextBox deckCodeInput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox infoBox;
        private System.Windows.Forms.Button forceFetchJpJsonButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button forceFetchEnJsonButton;
        private System.Windows.Forms.Label deckCodeLabel;
        private System.Windows.Forms.Label formatLabel;
    }
}

