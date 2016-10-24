namespace Athame.UI
{
    partial class SettingsForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.generalTab = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.saveLocBrowseButton = new System.Windows.Forms.Button();
            this.pathFormatTextBox = new System.Windows.Forms.TextBox();
            this.saveLocLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.servicesListBox = new System.Windows.Forms.ListBox();
            this.serviceUiPanel = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.generalTab);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(14, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(716, 375);
            this.tabControl1.TabIndex = 0;
            // 
            // generalTab
            // 
            this.generalTab.Controls.Add(this.label2);
            this.generalTab.Controls.Add(this.label1);
            this.generalTab.Controls.Add(this.label5);
            this.generalTab.Controls.Add(this.saveLocBrowseButton);
            this.generalTab.Controls.Add(this.pathFormatTextBox);
            this.generalTab.Controls.Add(this.saveLocLabel);
            this.generalTab.Controls.Add(this.label3);
            this.generalTab.Location = new System.Drawing.Point(4, 24);
            this.generalTab.Name = "generalTab";
            this.generalTab.Padding = new System.Windows.Forms.Padding(3);
            this.generalTab.Size = new System.Drawing.Size(708, 347);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            this.generalTab.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(447, 75);
            this.label2.TabIndex = 11;
            this.label2.Text = "Track artist\r\nTrack title\r\nTrack number relative to disc\r\nWill use the track\'s ar" +
    "tist if the album artist isn\'t available, otherwise the album artist\r\nAlbum titl" +
    "e";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(88, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 75);
            this.label1.TabIndex = 2;
            this.label1.Text = "{Artist}\r\n{Title}\r\n{TrackNumber}\r\n{AlbumArtistOrArtist}\r\n{Album.Title}";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Path format:";
            // 
            // saveLocBrowseButton
            // 
            this.saveLocBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveLocBrowseButton.Location = new System.Drawing.Point(627, 20);
            this.saveLocBrowseButton.Name = "saveLocBrowseButton";
            this.saveLocBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.saveLocBrowseButton.TabIndex = 8;
            this.saveLocBrowseButton.Text = "Browse...";
            this.saveLocBrowseButton.UseVisualStyleBackColor = true;
            this.saveLocBrowseButton.Click += new System.EventHandler(this.saveLocBrowseButton_Click);
            // 
            // pathFormatTextBox
            // 
            this.pathFormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathFormatTextBox.Location = new System.Drawing.Point(91, 59);
            this.pathFormatTextBox.Name = "pathFormatTextBox";
            this.pathFormatTextBox.Size = new System.Drawing.Size(611, 23);
            this.pathFormatTextBox.TabIndex = 10;
            this.pathFormatTextBox.TextChanged += new System.EventHandler(this.pathFormatTextBox_TextChanged);
            // 
            // saveLocLabel
            // 
            this.saveLocLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveLocLabel.Location = new System.Drawing.Point(91, 24);
            this.saveLocLabel.Name = "saveLocLabel";
            this.saveLocLabel.Size = new System.Drawing.Size(514, 15);
            this.saveLocLabel.TabIndex = 7;
            this.saveLocLabel.Text = "~";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Save to:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.saveButton);
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(541, 396);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(186, 33);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(96, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(87, 27);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(3, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 27);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.serviceUiPanel);
            this.tabPage1.Controls.Add(this.servicesListBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(708, 347);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Services";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // servicesListBox
            // 
            this.servicesListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.servicesListBox.FormattingEnabled = true;
            this.servicesListBox.ItemHeight = 15;
            this.servicesListBox.Location = new System.Drawing.Point(6, 6);
            this.servicesListBox.Name = "servicesListBox";
            this.servicesListBox.Size = new System.Drawing.Size(173, 332);
            this.servicesListBox.TabIndex = 0;
            this.servicesListBox.SelectedIndexChanged += new System.EventHandler(this.servicesListBox_SelectedIndexChanged);
            // 
            // serviceUiPanel
            // 
            this.serviceUiPanel.Location = new System.Drawing.Point(185, 6);
            this.serviceUiPanel.Name = "serviceUiPanel";
            this.serviceUiPanel.Size = new System.Drawing.Size(517, 332);
            this.serviceUiPanel.TabIndex = 1;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 441);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.tabControl1.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.generalTab.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage generalTab;
        private System.Windows.Forms.TextBox pathFormatTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button saveLocBrowseButton;
        private System.Windows.Forms.Label saveLocLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.FolderBrowserDialog mFolderBrowserDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox servicesListBox;
        private System.Windows.Forms.Panel serviceUiPanel;
    }
}