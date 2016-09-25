namespace Athame.PlayMusicApi
{
    partial class PlayMusicSettingsControl
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
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.qHighRadioButton = new System.Windows.Forms.RadioButton();
            this.qMediumRadioButton = new System.Windows.Forms.RadioButton();
            this.qLowRadioButton = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Quality:";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel3.Controls.Add(this.qHighRadioButton);
            this.flowLayoutPanel3.Controls.Add(this.qMediumRadioButton);
            this.flowLayoutPanel3.Controls.Add(this.qLowRadioButton);
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(58, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(143, 69);
            this.flowLayoutPanel3.TabIndex = 10;
            // 
            // qHighRadioButton
            // 
            this.qHighRadioButton.AutoSize = true;
            this.qHighRadioButton.Location = new System.Drawing.Point(3, 3);
            this.qHighRadioButton.Name = "qHighRadioButton";
            this.qHighRadioButton.Size = new System.Drawing.Size(122, 17);
            this.qHighRadioButton.TabIndex = 0;
            this.qHighRadioButton.TabStop = true;
            this.qHighRadioButton.Text = "High (320kbps MP3)";
            this.qHighRadioButton.UseVisualStyleBackColor = true;
            // 
            // qMediumRadioButton
            // 
            this.qMediumRadioButton.AutoSize = true;
            this.qMediumRadioButton.Location = new System.Drawing.Point(3, 26);
            this.qMediumRadioButton.Name = "qMediumRadioButton";
            this.qMediumRadioButton.Size = new System.Drawing.Size(137, 17);
            this.qMediumRadioButton.TabIndex = 1;
            this.qMediumRadioButton.TabStop = true;
            this.qMediumRadioButton.Text = "Medium (160kbps MP3)";
            this.qMediumRadioButton.UseVisualStyleBackColor = true;
            // 
            // qLowRadioButton
            // 
            this.qLowRadioButton.AutoSize = true;
            this.qLowRadioButton.Location = new System.Drawing.Point(3, 49);
            this.qLowRadioButton.Name = "qLowRadioButton";
            this.qLowRadioButton.Size = new System.Drawing.Size(120, 17);
            this.qLowRadioButton.TabIndex = 2;
            this.qLowRadioButton.TabStop = true;
            this.qLowRadioButton.Text = "Low (128kbps MP3)";
            this.qLowRadioButton.UseVisualStyleBackColor = true;
            // 
            // PlayMusicSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Name = "PlayMusicSettingsControl";
            this.Size = new System.Drawing.Size(209, 81);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.RadioButton qHighRadioButton;
        private System.Windows.Forms.RadioButton qMediumRadioButton;
        private System.Windows.Forms.RadioButton qLowRadioButton;
    }
}
