namespace Athame.UI
{
    partial class ServiceSettingsView
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.signInStatusLabel = new System.Windows.Forms.Label();
            this.signInButton = new System.Windows.Forms.Button();
            this.servicePanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.signInStatusLabel);
            this.panel1.Controls.Add(this.signInButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(631, 47);
            this.panel1.TabIndex = 1;
            // 
            // signInStatusLabel
            // 
            this.signInStatusLabel.AutoSize = true;
            this.signInStatusLabel.Location = new System.Drawing.Point(3, 9);
            this.signInStatusLabel.Name = "signInStatusLabel";
            this.signInStatusLabel.Size = new System.Drawing.Size(147, 15);
            this.signInStatusLabel.TabIndex = 1;
            this.signInStatusLabel.Text = "Signed in as {0}|Signed out";
            this.signInStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // signInButton
            // 
            this.signInButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signInButton.Location = new System.Drawing.Point(540, 3);
            this.signInButton.Name = "signInButton";
            this.signInButton.Size = new System.Drawing.Size(87, 27);
            this.signInButton.TabIndex = 0;
            this.signInButton.Text = "Sign out|Sign in";
            this.signInButton.UseVisualStyleBackColor = true;
            this.signInButton.Click += new System.EventHandler(this.signInButton_Click);
            // 
            // servicePanel
            // 
            this.servicePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.servicePanel.Location = new System.Drawing.Point(0, 47);
            this.servicePanel.Name = "servicePanel";
            this.servicePanel.Size = new System.Drawing.Size(631, 315);
            this.servicePanel.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Location = new System.Drawing.Point(6, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(621, 1);
            this.panel2.TabIndex = 2;
            // 
            // ServiceSettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.servicePanel);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServiceSettingsView";
            this.Size = new System.Drawing.Size(631, 362);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label signInStatusLabel;
        private System.Windows.Forms.Button signInButton;
        private System.Windows.Forms.Panel servicePanel;
        private System.Windows.Forms.Panel panel2;
    }
}
