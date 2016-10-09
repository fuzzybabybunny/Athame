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
            this.mLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.signInStatusLabel = new System.Windows.Forms.Label();
            this.signInButton = new System.Windows.Forms.Button();
            this.mLayout.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mLayout
            // 
            this.mLayout.BackColor = System.Drawing.Color.White;
            this.mLayout.ColumnCount = 1;
            this.mLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mLayout.Controls.Add(this.panel1, 0, 0);
            this.mLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mLayout.Location = new System.Drawing.Point(0, 0);
            this.mLayout.Name = "mLayout";
            this.mLayout.RowCount = 2;
            this.mLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mLayout.Size = new System.Drawing.Size(680, 362);
            this.mLayout.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.signInStatusLabel);
            this.panel1.Controls.Add(this.signInButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(674, 33);
            this.panel1.TabIndex = 0;
            // 
            // signInStatusLabel
            // 
            this.signInStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.signInStatusLabel.Location = new System.Drawing.Point(0, 9);
            this.signInStatusLabel.Name = "signInStatusLabel";
            this.signInStatusLabel.Size = new System.Drawing.Size(576, 15);
            this.signInStatusLabel.TabIndex = 1;
            this.signInStatusLabel.Text = "Signed in as {0}|Signed out";
            this.signInStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // signInButton
            // 
            this.signInButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signInButton.Location = new System.Drawing.Point(583, 3);
            this.signInButton.Name = "signInButton";
            this.signInButton.Size = new System.Drawing.Size(87, 27);
            this.signInButton.TabIndex = 0;
            this.signInButton.Text = "Sign out|Sign in";
            this.signInButton.UseVisualStyleBackColor = true;
            this.signInButton.Click += new System.EventHandler(this.signInButton_Click);
            // 
            // ServiceSettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServiceSettingsView";
            this.Size = new System.Drawing.Size(680, 362);
            this.mLayout.ResumeLayout(false);
            this.mLayout.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mLayout;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label signInStatusLabel;
        private System.Windows.Forms.Button signInButton;
    }
}
