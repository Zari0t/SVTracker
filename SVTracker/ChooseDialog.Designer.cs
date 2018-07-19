namespace SVTracker
{
    partial class ChooseDialog
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
            this.choiceBannerList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // choiceBannerList
            // 
            this.choiceBannerList.AutoSize = true;
            this.choiceBannerList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.choiceBannerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.choiceBannerList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.choiceBannerList.Location = new System.Drawing.Point(0, 0);
            this.choiceBannerList.Margin = new System.Windows.Forms.Padding(0);
            this.choiceBannerList.Name = "choiceBannerList";
            this.choiceBannerList.Size = new System.Drawing.Size(184, 61);
            this.choiceBannerList.TabIndex = 0;
            // 
            // ChooseDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(184, 61);
            this.ControlBox = false;
            this.Controls.Add(this.choiceBannerList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Choose";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel choiceBannerList;
    }
}