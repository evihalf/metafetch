namespace metafetch
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.grpMetadataWithout = new System.Windows.Forms.GroupBox();
            this.chkAutoFetch = new System.Windows.Forms.CheckBox();
            this.lvMetadataWithout = new System.Windows.Forms.ListView();
            this.chWithoutCheck = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWithoutName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWithoutPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsWithout = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.withoutManualFetchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilListIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnFetchSelected = new System.Windows.Forms.Button();
            this.grpMetadataWith = new System.Windows.Forms.GroupBox();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.lvMetadataWith = new System.Windows.Forms.ListView();
            this.chWithCheck = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWithTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWithYear = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsWith = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.withManualFetchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnFlushSelected = new System.Windows.Forms.Button();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.btnCancel = new System.Windows.Forms.ToolStripDropDownButton();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.librariesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFileSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ttMain = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.grpMetadataWithout.SuspendLayout();
            this.cmsWithout.SuspendLayout();
            this.grpMetadataWith.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.cmsWith.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.msMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // scMain
            // 
            this.scMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scMain.Location = new System.Drawing.Point(0, 27);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.grpMetadataWithout);
            this.scMain.Panel1MinSize = 100;
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.grpMetadataWith);
            this.scMain.Panel2MinSize = 100;
            this.scMain.Size = new System.Drawing.Size(624, 390);
            this.scMain.SplitterDistance = 284;
            this.scMain.TabIndex = 0;
            // 
            // grpMetadataWithout
            // 
            this.grpMetadataWithout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMetadataWithout.Controls.Add(this.chkAutoFetch);
            this.grpMetadataWithout.Controls.Add(this.lvMetadataWithout);
            this.grpMetadataWithout.Controls.Add(this.btnFetchSelected);
            this.grpMetadataWithout.Location = new System.Drawing.Point(12, 9);
            this.grpMetadataWithout.Name = "grpMetadataWithout";
            this.grpMetadataWithout.Size = new System.Drawing.Size(269, 369);
            this.grpMetadataWithout.TabIndex = 3;
            this.grpMetadataWithout.TabStop = false;
            this.grpMetadataWithout.Text = "Without Metadata";
            // 
            // chkAutoFetch
            // 
            this.chkAutoFetch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoFetch.AutoSize = true;
            this.chkAutoFetch.Location = new System.Drawing.Point(185, 336);
            this.chkAutoFetch.Name = "chkAutoFetch";
            this.chkAutoFetch.Size = new System.Drawing.Size(78, 17);
            this.chkAutoFetch.TabIndex = 3;
            this.chkAutoFetch.Text = "Auto Fetch";
            this.chkAutoFetch.UseVisualStyleBackColor = true;
            this.chkAutoFetch.Visible = false;
            this.chkAutoFetch.CheckedChanged += new System.EventHandler(this.chkAutoFetch_CheckedChanged);
            // 
            // lvMetadataWithout
            // 
            this.lvMetadataWithout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMetadataWithout.CheckBoxes = true;
            this.lvMetadataWithout.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chWithoutCheck,
            this.chWithoutName,
            this.chWithoutPath});
            this.lvMetadataWithout.ContextMenuStrip = this.cmsWithout;
            this.lvMetadataWithout.FullRowSelect = true;
            this.lvMetadataWithout.HideSelection = false;
            this.lvMetadataWithout.Location = new System.Drawing.Point(6, 28);
            this.lvMetadataWithout.MultiSelect = false;
            this.lvMetadataWithout.Name = "lvMetadataWithout";
            this.lvMetadataWithout.ShowItemToolTips = true;
            this.lvMetadataWithout.Size = new System.Drawing.Size(257, 289);
            this.lvMetadataWithout.SmallImageList = this.ilListIcons;
            this.lvMetadataWithout.TabIndex = 1;
            this.lvMetadataWithout.UseCompatibleStateImageBehavior = false;
            this.lvMetadataWithout.View = System.Windows.Forms.View.Details;
            this.lvMetadataWithout.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMetadataWithout_ColumnClick);
            this.lvMetadataWithout.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvMetadataWithout_ItemChecked);
            // 
            // chWithoutCheck
            // 
            this.chWithoutCheck.Text = "";
            this.chWithoutCheck.Width = 40;
            // 
            // chWithoutName
            // 
            this.chWithoutName.Text = "Title";
            this.chWithoutName.Width = 114;
            // 
            // chWithoutPath
            // 
            this.chWithoutPath.Text = "Path";
            this.chWithoutPath.Width = 350;
            // 
            // cmsWithout
            // 
            this.cmsWithout.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.withoutManualFetchMenuItem});
            this.cmsWithout.Name = "cmsWithout";
            this.cmsWithout.Size = new System.Drawing.Size(156, 26);
            this.cmsWithout.Opening += new System.ComponentModel.CancelEventHandler(this.cmsWithout_Opening);
            // 
            // withoutManualFetchMenuItem
            // 
            this.withoutManualFetchMenuItem.Name = "withoutManualFetchMenuItem";
            this.withoutManualFetchMenuItem.Size = new System.Drawing.Size(155, 22);
            this.withoutManualFetchMenuItem.Text = "&Manual Fetch...";
            this.withoutManualFetchMenuItem.Click += new System.EventHandler(this.withoutManualFetchMenuItem_Click);
            // 
            // ilListIcons
            // 
            this.ilListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilListIcons.ImageStream")));
            this.ilListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilListIcons.Images.SetKeyName(0, "error.png");
            // 
            // btnFetchSelected
            // 
            this.btnFetchSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFetchSelected.Enabled = false;
            this.btnFetchSelected.Location = new System.Drawing.Point(6, 323);
            this.btnFetchSelected.Name = "btnFetchSelected";
            this.btnFetchSelected.Size = new System.Drawing.Size(117, 40);
            this.btnFetchSelected.TabIndex = 2;
            this.btnFetchSelected.Text = "Fetch Selected";
            this.btnFetchSelected.UseVisualStyleBackColor = true;
            this.btnFetchSelected.Click += new System.EventHandler(this.btnFetchSelected_Click);
            // 
            // grpMetadataWith
            // 
            this.grpMetadataWith.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMetadataWith.Controls.Add(this.pbImage);
            this.grpMetadataWith.Controls.Add(this.lvMetadataWith);
            this.grpMetadataWith.Controls.Add(this.btnFlushSelected);
            this.grpMetadataWith.Location = new System.Drawing.Point(3, 9);
            this.grpMetadataWith.Name = "grpMetadataWith";
            this.grpMetadataWith.Size = new System.Drawing.Size(321, 369);
            this.grpMetadataWith.TabIndex = 5;
            this.grpMetadataWith.TabStop = false;
            this.grpMetadataWith.Text = "With Metadata";
            // 
            // pbImage
            // 
            this.pbImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pbImage.ErrorImage")));
            this.pbImage.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbImage.InitialImage")));
            this.pbImage.Location = new System.Drawing.Point(225, 253);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(90, 110);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 5;
            this.pbImage.TabStop = false;
            // 
            // lvMetadataWith
            // 
            this.lvMetadataWith.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMetadataWith.CheckBoxes = true;
            this.lvMetadataWith.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chWithCheck,
            this.chWithTitle,
            this.chWithYear});
            this.lvMetadataWith.ContextMenuStrip = this.cmsWith;
            this.lvMetadataWith.FullRowSelect = true;
            this.lvMetadataWith.HideSelection = false;
            this.lvMetadataWith.Location = new System.Drawing.Point(6, 28);
            this.lvMetadataWith.MultiSelect = false;
            this.lvMetadataWith.Name = "lvMetadataWith";
            this.lvMetadataWith.ShowItemToolTips = true;
            this.lvMetadataWith.Size = new System.Drawing.Size(309, 217);
            this.lvMetadataWith.SmallImageList = this.ilListIcons;
            this.lvMetadataWith.TabIndex = 2;
            this.lvMetadataWith.UseCompatibleStateImageBehavior = false;
            this.lvMetadataWith.View = System.Windows.Forms.View.Details;
            this.lvMetadataWith.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMetadataWith_ColumnClick);
            this.lvMetadataWith.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvMetadataWith_ItemChecked);
            this.lvMetadataWith.SelectedIndexChanged += new System.EventHandler(this.lvMetadataWith_SelectedIndexChanged);
            // 
            // chWithCheck
            // 
            this.chWithCheck.Text = "";
            this.chWithCheck.Width = 40;
            // 
            // chWithTitle
            // 
            this.chWithTitle.Text = "Title";
            this.chWithTitle.Width = 189;
            // 
            // chWithYear
            // 
            this.chWithYear.Text = "Year";
            this.chWithYear.Width = 50;
            // 
            // cmsWith
            // 
            this.cmsWith.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.withManualFetchMenuItem});
            this.cmsWith.Name = "cmsWithout";
            this.cmsWith.Size = new System.Drawing.Size(156, 26);
            this.cmsWith.Opening += new System.ComponentModel.CancelEventHandler(this.cmsWith_Opening);
            // 
            // withManualFetchMenuItem
            // 
            this.withManualFetchMenuItem.Name = "withManualFetchMenuItem";
            this.withManualFetchMenuItem.Size = new System.Drawing.Size(155, 22);
            this.withManualFetchMenuItem.Text = "&Manual Fetch...";
            this.withManualFetchMenuItem.Click += new System.EventHandler(this.withManualFetchMenuItem_Click);
            // 
            // btnFlushSelected
            // 
            this.btnFlushSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlushSelected.Enabled = false;
            this.btnFlushSelected.Location = new System.Drawing.Point(6, 251);
            this.btnFlushSelected.Name = "btnFlushSelected";
            this.btnFlushSelected.Size = new System.Drawing.Size(96, 27);
            this.btnFlushSelected.TabIndex = 4;
            this.btnFlushSelected.Text = "Flush Selected";
            this.btnFlushSelected.UseVisualStyleBackColor = true;
            this.btnFlushSelected.Click += new System.EventHandler(this.btnFlushSelected_Click);
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.pbProgress,
            this.btnCancel});
            this.ssStatus.Location = new System.Drawing.Point(0, 420);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(624, 22);
            this.ssStatus.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // pbProgress
            // 
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(100, 16);
            this.pbProgress.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCancel.Image = global::metafetch.Properties.Resources.close;
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ShowDropDownArrow = false;
            this.btnCancel.Size = new System.Drawing.Size(20, 20);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(624, 24);
            this.msMain.TabIndex = 2;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.librariesToolStripMenuItem,
            this.toolStripFileSeparator,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // librariesToolStripMenuItem
            // 
            this.librariesToolStripMenuItem.Name = "librariesToolStripMenuItem";
            this.librariesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.librariesToolStripMenuItem.Text = "&Libraries...";
            this.librariesToolStripMenuItem.Click += new System.EventHandler(this.librariesToolStripMenuItem_Click);
            // 
            // toolStripFileSeparator
            // 
            this.toolStripFileSeparator.Name = "toolStripFileSeparator";
            this.toolStripFileSeparator.Size = new System.Drawing.Size(124, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.msMain);
            this.Controls.Add(this.scMain);
            this.MainMenuStrip = this.msMain;
            this.MinimumSize = new System.Drawing.Size(480, 300);
            this.Name = "frmMain";
            this.Text = "metafetch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.grpMetadataWithout.ResumeLayout(false);
            this.grpMetadataWithout.PerformLayout();
            this.cmsWithout.ResumeLayout(false);
            this.grpMetadataWith.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.cmsWith.ResumeLayout(false);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.ListView lvMetadataWithout;
        private System.Windows.Forms.ListView lvMetadataWith;
        private System.Windows.Forms.Button btnFetchSelected;
        private System.Windows.Forms.Button btnFlushSelected;
        private System.Windows.Forms.GroupBox grpMetadataWithout;
        private System.Windows.Forms.GroupBox grpMetadataWith;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripFileSeparator;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem librariesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkAutoFetch;
        private System.Windows.Forms.ColumnHeader chWithoutName;
        private System.Windows.Forms.ColumnHeader chWithTitle;
        private System.Windows.Forms.ToolStripProgressBar pbProgress;
        private System.Windows.Forms.ToolStripDropDownButton btnCancel;
        private System.Windows.Forms.ColumnHeader chWithoutCheck;
        private System.Windows.Forms.ColumnHeader chWithCheck;
        private System.Windows.Forms.ToolTip ttMain;
        private System.Windows.Forms.ImageList ilListIcons;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.ColumnHeader chWithYear;
        private System.Windows.Forms.ContextMenuStrip cmsWith;
        private System.Windows.Forms.ToolStripMenuItem withManualFetchMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsWithout;
        private System.Windows.Forms.ToolStripMenuItem withoutManualFetchMenuItem;
        private System.Windows.Forms.ColumnHeader chWithoutPath;
    }
}

