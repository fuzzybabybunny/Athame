namespace Athame.TidalApi
{
    partial class TidalSettingsControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.qLosslessRadioButton = new System.Windows.Forms.RadioButton();
            this.qHighRadioButton = new System.Windows.Forms.RadioButton();
            this.qLowRadioButton = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Quality:";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.qLosslessRadioButton);
            this.flowLayoutPanel2.Controls.Add(this.qHighRadioButton);
            this.flowLayoutPanel2.Controls.Add(this.qLowRadioButton);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(56, 7);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(281, 69);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // qLosslessRadioButton
            // 
            this.qLosslessRadioButton.AutoSize = true;
            this.qLosslessRadioButton.Location = new System.Drawing.Point(3, 3);
            this.qLosslessRadioButton.Name = "qLosslessRadioButton";
            this.qLosslessRadioButton.Size = new System.Drawing.Size(275, 17);
            this.qLosslessRadioButton.TabIndex = 0;
            this.qLosslessRadioButton.TabStop = true;
            this.qLosslessRadioButton.Text = "Lossless (FLAC, only available with Hi-Fi subscription)";
            this.qLosslessRadioButton.UseVisualStyleBackColor = true;
            // 
            // qHighRadioButton
            // 
            this.qHighRadioButton.AutoSize = true;
            this.qHighRadioButton.Location = new System.Drawing.Point(3, 26);
            this.qHighRadioButton.Name = "qHighRadioButton";
            this.qHighRadioButton.Size = new System.Drawing.Size(121, 17);
            this.qHighRadioButton.TabIndex = 1;
            this.qHighRadioButton.TabStop = true;
            this.qHighRadioButton.Text = "High (320kbps AAC)";
            this.qHighRadioButton.UseVisualStyleBackColor = true;
            // 
            // qLowRadioButton
            // 
            this.qLowRadioButton.AutoSize = true;
            this.qLowRadioButton.Location = new System.Drawing.Point(3, 49);
            this.qLowRadioButton.Name = "qLowRadioButton";
            this.qLowRadioButton.Size = new System.Drawing.Size(113, 17);
            this.qLowRadioButton.TabIndex = 2;
            this.qLowRadioButton.TabStop = true;
            this.qLowRadioButton.Text = "Low (96kbps AAC)";
            this.qLowRadioButton.UseVisualStyleBackColor = true;
            // 
            // TidalSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Name = "TidalSettingsControl";
            this.Size = new System.Drawing.Size(356, 205);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RadioButton qLosslessRadioButton;
        private System.Windows.Forms.RadioButton qHighRadioButton;
        private System.Windows.Forms.RadioButton qLowRadioButton;

    }
}
