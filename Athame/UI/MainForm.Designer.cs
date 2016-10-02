using Athame.Properties;

namespace Athame.UI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dlButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.queueTab = new System.Windows.Forms.TabPage();
            this.queueListView = new System.Windows.Forms.ListView();
            this.checkCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.trackNumberCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.titleCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.artistCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.albumCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.queueImageList = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.currTrackLabel = new System.Windows.Forms.Label();
            this.totalProgressStatus = new System.Windows.Forms.Label();
            this.totalProgressBar = new System.Windows.Forms.ProgressBar();
            this.settingsButton = new System.Windows.Forms.Button();
            this.queueMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearButton = new System.Windows.Forms.LinkLabel();
            this.pasteButton = new System.Windows.Forms.LinkLabel();
            this.urlValidStateLabel = new System.Windows.Forms.LinkLabel();
            this.startDownloadButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.queueTab.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.queueMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // idTextBox
            // 
            this.idTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.idTextBox.Location = new System.Drawing.Point(56, 59);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(642, 23);
            this.idTextBox.TabIndex = 2;
            this.idTextBox.TextChanged += new System.EventHandler(this.idTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "URL:";
            // 
            // dlButton
            // 
            this.dlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dlButton.Enabled = false;
            this.dlButton.Location = new System.Drawing.Point(708, 59);
            this.dlButton.Name = "dlButton";
            this.dlButton.Size = new System.Drawing.Size(87, 23);
            this.dlButton.TabIndex = 4;
            this.dlButton.Text = "Add";
            this.dlButton.UseVisualStyleBackColor = true;
            this.dlButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logTextBox.Location = new System.Drawing.Point(3, 3);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTextBox.Size = new System.Drawing.Size(765, 473);
            this.logTextBox.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.queueTab);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(14, 300);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(779, 507);
            this.tabControl1.TabIndex = 6;
            // 
            // queueTab
            // 
            this.queueTab.Controls.Add(this.queueListView);
            this.queueTab.Location = new System.Drawing.Point(4, 24);
            this.queueTab.Name = "queueTab";
            this.queueTab.Padding = new System.Windows.Forms.Padding(3);
            this.queueTab.Size = new System.Drawing.Size(771, 479);
            this.queueTab.TabIndex = 0;
            this.queueTab.Text = "Queue";
            this.queueTab.UseVisualStyleBackColor = true;
            // 
            // queueListView
            // 
            this.queueListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queueListView.CheckBoxes = true;
            this.queueListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.checkCol,
            this.trackNumberCol,
            this.titleCol,
            this.artistCol,
            this.albumCol});
            this.queueListView.FullRowSelect = true;
            this.queueListView.GridLines = true;
            this.queueListView.Location = new System.Drawing.Point(3, 7);
            this.queueListView.Name = "queueListView";
            this.queueListView.Size = new System.Drawing.Size(762, 464);
            this.queueListView.SmallImageList = this.queueImageList;
            this.queueListView.TabIndex = 7;
            this.queueListView.UseCompatibleStateImageBehavior = false;
            this.queueListView.View = System.Windows.Forms.View.Details;
            this.queueListView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.queueListView_ItemCheck);
            this.queueListView.SelectedIndexChanged += new System.EventHandler(this.queueListView_SelectedIndexChanged);
            this.queueListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.queueListView_MouseClick);
            // 
            // checkCol
            // 
            this.checkCol.Text = "✔";
            this.checkCol.Width = 48;
            // 
            // trackNumberCol
            // 
            this.trackNumberCol.Text = "Track/Disc";
            this.trackNumberCol.Width = 80;
            // 
            // titleCol
            // 
            this.titleCol.Text = "Title";
            this.titleCol.Width = 240;
            // 
            // artistCol
            // 
            this.artistCol.Text = "Artist";
            this.artistCol.Width = 181;
            // 
            // albumCol
            // 
            this.albumCol.Text = "Album";
            this.albumCol.Width = 152;
            // 
            // queueImageList
            // 
            this.queueImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.queueImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.queueImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.logTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(771, 479);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.currTrackLabel);
            this.groupBox2.Controls.Add(this.totalProgressStatus);
            this.groupBox2.Controls.Add(this.totalProgressBar);
            this.groupBox2.Location = new System.Drawing.Point(14, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(779, 147);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // currTrackLabel
            // 
            this.currTrackLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currTrackLabel.Location = new System.Drawing.Point(7, 27);
            this.currTrackLabel.Name = "currTrackLabel";
            this.currTrackLabel.Size = new System.Drawing.Size(764, 40);
            this.currTrackLabel.TabIndex = 2;
            this.currTrackLabel.Text = "Ready";
            this.currTrackLabel.UseMnemonic = false;
            // 
            // totalProgressStatus
            // 
            this.totalProgressStatus.AutoSize = true;
            this.totalProgressStatus.Location = new System.Drawing.Point(7, 75);
            this.totalProgressStatus.Name = "totalProgressStatus";
            this.totalProgressStatus.Size = new System.Drawing.Size(89, 15);
            this.totalProgressStatus.TabIndex = 1;
            this.totalProgressStatus.Text = "Ready to begin.";
            this.totalProgressStatus.UseMnemonic = false;
            // 
            // totalProgressBar
            // 
            this.totalProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.totalProgressBar.Location = new System.Drawing.Point(7, 113);
            this.totalProgressBar.Name = "totalProgressBar";
            this.totalProgressBar.Size = new System.Drawing.Size(763, 27);
            this.totalProgressBar.TabIndex = 0;
            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(14, 14);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(87, 23);
            this.settingsButton.TabIndex = 7;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // queueMenu
            // 
            this.queueMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeGroupToolStripMenuItem,
            this.toolStripSeparator2,
            this.showInExplorerToolStripMenuItem});
            this.queueMenu.Name = "queueMenu";
            this.queueMenu.Size = new System.Drawing.Size(171, 76);
            // 
            // removeGroupToolStripMenuItem
            // 
            this.removeGroupToolStripMenuItem.Name = "removeGroupToolStripMenuItem";
            this.removeGroupToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.removeGroupToolStripMenuItem.Text = "Remove group";
            this.removeGroupToolStripMenuItem.Click += new System.EventHandler(this.removeGroupToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // showInExplorerToolStripMenuItem
            // 
            this.showInExplorerToolStripMenuItem.Name = "showInExplorerToolStripMenuItem";
            this.showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.showInExplorerToolStripMenuItem.Text = "Show in Explorer...";
            this.showInExplorerToolStripMenuItem.Click += new System.EventHandler(this.showInExplorerToolStripMenuItem_Click);
            // 
            // clearButton
            // 
            this.clearButton.AutoSize = true;
            this.clearButton.Location = new System.Drawing.Point(660, 88);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(34, 15);
            this.clearButton.TabIndex = 9;
            this.clearButton.TabStop = true;
            this.clearButton.Text = "Clear";
            this.clearButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clearButton_LinkClicked);
            // 
            // pasteButton
            // 
            this.pasteButton.AutoSize = true;
            this.pasteButton.Location = new System.Drawing.Point(614, 88);
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Size = new System.Drawing.Size(35, 15);
            this.pasteButton.TabIndex = 10;
            this.pasteButton.TabStop = true;
            this.pasteButton.Text = "Paste";
            this.pasteButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.pasteButton_LinkClicked);
            // 
            // urlValidStateLabel
            // 
            this.urlValidStateLabel.Image = global::Athame.Properties.Resources.error;
            this.urlValidStateLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.urlValidStateLabel.Location = new System.Drawing.Point(52, 88);
            this.urlValidStateLabel.Name = "urlValidStateLabel";
            this.urlValidStateLabel.Padding = new System.Windows.Forms.Padding(23, 5, 0, 0);
            this.urlValidStateLabel.Size = new System.Drawing.Size(554, 36);
            this.urlValidStateLabel.TabIndex = 11;
            this.urlValidStateLabel.Visible = false;
            this.urlValidStateLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.urlValidStateLabel_LinkClicked);
            // 
            // startDownloadButton
            // 
            this.startDownloadButton.Location = new System.Drawing.Point(720, 295);
            this.startDownloadButton.Name = "startDownloadButton";
            this.startDownloadButton.Size = new System.Drawing.Size(75, 23);
            this.startDownloadButton.TabIndex = 12;
            this.startDownloadButton.Text = "Start";
            this.startDownloadButton.UseVisualStyleBackColor = true;
            this.startDownloadButton.Click += new System.EventHandler(this.startDownloadButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.dlButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 820);
            this.Controls.Add(this.startDownloadButton);
            this.Controls.Add(this.urlValidStateLabel);
            this.Controls.Add(this.pasteButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.dlButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idTextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::Athame.Properties.Resources.AthameIcon;
            this.Name = "MainForm";
            this.Text = "Athame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.queueTab.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.queueMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button dlButton;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage queueTab;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView queueListView;
        private System.Windows.Forms.ColumnHeader titleCol;
        private System.Windows.Forms.ColumnHeader artistCol;
        private System.Windows.Forms.ColumnHeader albumCol;
        private System.Windows.Forms.ColumnHeader trackNumberCol;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar totalProgressBar;
        private System.Windows.Forms.Label totalProgressStatus;
        private System.Windows.Forms.Label currTrackLabel;
        private System.Windows.Forms.ColumnHeader checkCol;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.ContextMenuStrip queueMenu;
        private System.Windows.Forms.ToolStripMenuItem showInExplorerToolStripMenuItem;
        private System.Windows.Forms.LinkLabel clearButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.LinkLabel pasteButton;
        private System.Windows.Forms.LinkLabel urlValidStateLabel;
        private System.Windows.Forms.Button startDownloadButton;
        private System.Windows.Forms.ImageList queueImageList;
        private System.Windows.Forms.ToolStripMenuItem removeGroupToolStripMenuItem;

    }
}

