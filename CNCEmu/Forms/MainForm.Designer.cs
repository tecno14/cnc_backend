namespace CNCEmu.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.FileMenubarMS = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PlayerProfileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditConfigMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GenerateProviderIDMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OnlyHighMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HighAndMediumMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabsTC = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.LogRTB = new System.Windows.Forms.RichTextBox();
            this.SearchbarTS = new System.Windows.Forms.ToolStrip();
            this.SearchTB = new System.Windows.Forms.ToolStripTextBox();
            this.SearchBtn = new System.Windows.Forms.ToolStripButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tv1 = new System.Windows.Forms.TreeView();
            this.rtb2 = new System.Windows.Forms.RichTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.RefreshSB = new System.Windows.Forms.ToolStripButton();
            this.CacheSB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.LoadFolderSB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.LabelByteSB = new System.Windows.Forms.ToolStripButton();
            this.ByteLabelSB = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox3 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox4 = new System.Windows.Forms.ToolStripTextBox();
            this.CompressIntSB = new System.Windows.Forms.ToolStripButton();
            this.DecompressIntSB = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox5 = new System.Windows.Forms.ToolStripTextBox();
            this.StartBlazeServerBtn = new System.Windows.Forms.Button();
            this.StartRedirectorServerBtn = new System.Windows.Forms.Button();
            this.FileMenubarTLP = new System.Windows.Forms.TableLayoutPanel();
            this.StartWebServerBtn = new System.Windows.Forms.Button();
            this.VersionLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusbarSS = new System.Windows.Forms.StatusStrip();
            this.FileMenubarMS.SuspendLayout();
            this.TabsTC.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SearchbarTS.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).BeginInit();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.FileMenubarTLP.SuspendLayout();
            this.StatusbarSS.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileMenubarMS
            // 
            this.FileMenubarMS.Dock = System.Windows.Forms.DockStyle.None;
            this.FileMenubarMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.SettingsMenuItem,
            this.HelpMenuItem});
            this.FileMenubarMS.Location = new System.Drawing.Point(0, 0);
            this.FileMenubarMS.Name = "FileMenubarMS";
            this.FileMenubarMS.Size = new System.Drawing.Size(286, 24);
            this.FileMenubarMS.TabIndex = 0;
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutMenuItem,
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileMenuItem.Text = "File";
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Image = global::CNCEmu.Properties.Resources.ExternalLink;
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(107, 22);
            this.AboutMenuItem.Text = "About";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(107, 22);
            this.ExitMenuItem.Text = "Exit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // SettingsMenuItem
            // 
            this.SettingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PlayerProfileMenuItem,
            this.EditConfigMenuItem,
            this.ToolsMenuItem,
            this.LogLevelMenuItem});
            this.SettingsMenuItem.Name = "SettingsMenuItem";
            this.SettingsMenuItem.Size = new System.Drawing.Size(61, 20);
            this.SettingsMenuItem.Text = "Settings";
            // 
            // PlayerProfileMenuItem
            // 
            this.PlayerProfileMenuItem.Name = "PlayerProfileMenuItem";
            this.PlayerProfileMenuItem.Size = new System.Drawing.Size(143, 22);
            this.PlayerProfileMenuItem.Text = "Player profile";
            this.PlayerProfileMenuItem.Click += new System.EventHandler(this.PlayerProfileMenuItem_Click);
            // 
            // EditConfigMenuItem
            // 
            this.EditConfigMenuItem.Name = "EditConfigMenuItem";
            this.EditConfigMenuItem.Size = new System.Drawing.Size(143, 22);
            this.EditConfigMenuItem.Text = "Edit config";
            this.EditConfigMenuItem.Click += new System.EventHandler(this.EditConfigMenuItem_Click);
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GenerateProviderIDMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            this.ToolsMenuItem.Size = new System.Drawing.Size(143, 22);
            this.ToolsMenuItem.Text = "Tools";
            // 
            // GenerateProviderIDMenuItem
            // 
            this.GenerateProviderIDMenuItem.Name = "GenerateProviderIDMenuItem";
            this.GenerateProviderIDMenuItem.Size = new System.Drawing.Size(211, 22);
            this.GenerateProviderIDMenuItem.Text = "Generate ProviderID.dat ...";
            this.GenerateProviderIDMenuItem.Click += new System.EventHandler(this.GenerateProviderIDMenuItem_Click);
            // 
            // LogLevelMenuItem
            // 
            this.LogLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OnlyHighMenuItem,
            this.HighAndMediumMenuItem,
            this.AllMenuItem});
            this.LogLevelMenuItem.Name = "LogLevelMenuItem";
            this.LogLevelMenuItem.Size = new System.Drawing.Size(143, 22);
            this.LogLevelMenuItem.Text = "Log Level";
            // 
            // OnlyHighMenuItem
            // 
            this.OnlyHighMenuItem.Name = "OnlyHighMenuItem";
            this.OnlyHighMenuItem.Size = new System.Drawing.Size(171, 22);
            this.OnlyHighMenuItem.Text = "Only High";
            this.OnlyHighMenuItem.Click += new System.EventHandler(this.OnlyHighMenuItem_Click);
            // 
            // HighAndMediumMenuItem
            // 
            this.HighAndMediumMenuItem.Name = "HighAndMediumMenuItem";
            this.HighAndMediumMenuItem.Size = new System.Drawing.Size(171, 22);
            this.HighAndMediumMenuItem.Text = "High and Medium";
            this.HighAndMediumMenuItem.Click += new System.EventHandler(this.HighAndMediumMenuItem_Click);
            // 
            // AllMenuItem
            // 
            this.AllMenuItem.Name = "AllMenuItem";
            this.AllMenuItem.Size = new System.Drawing.Size(171, 22);
            this.AllMenuItem.Text = "All";
            this.AllMenuItem.Click += new System.EventHandler(this.AllMenuItem_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.Image = global::CNCEmu.Properties.Resources.ExternalLink;
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(60, 20);
            this.HelpMenuItem.Text = "Help";
            this.HelpMenuItem.Click += new System.EventHandler(this.HelpMenuItem_Click);
            // 
            // TabsTC
            // 
            this.TabsTC.Controls.Add(this.tabPage1);
            this.TabsTC.Controls.Add(this.tabPage2);
            this.TabsTC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabsTC.Location = new System.Drawing.Point(0, 28);
            this.TabsTC.Name = "TabsTC";
            this.TabsTC.SelectedIndex = 0;
            this.TabsTC.Size = new System.Drawing.Size(1020, 470);
            this.TabsTC.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TabsTC.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.LogRTB);
            this.tabPage1.Controls.Add(this.SearchbarTS);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1012, 444);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Log";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // LogRTB
            // 
            this.LogRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogRTB.Location = new System.Drawing.Point(3, 3);
            this.LogRTB.Name = "LogRTB";
            this.LogRTB.Size = new System.Drawing.Size(1006, 413);
            this.LogRTB.TabIndex = 1;
            this.LogRTB.Text = "";
            // 
            // SearchbarTS
            // 
            this.SearchbarTS.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SearchbarTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SearchTB,
            this.SearchBtn});
            this.SearchbarTS.Location = new System.Drawing.Point(3, 416);
            this.SearchbarTS.Name = "SearchbarTS";
            this.SearchbarTS.Size = new System.Drawing.Size(1006, 25);
            this.SearchbarTS.TabIndex = 0;
            this.SearchbarTS.Text = "toolStrip1";
            // 
            // SearchTB
            // 
            this.SearchTB.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SearchTB.Name = "SearchTB";
            this.SearchTB.Size = new System.Drawing.Size(100, 25);
            this.SearchTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchTB_KeyUp);
            // 
            // SearchBtn
            // 
            this.SearchBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SearchBtn.Image = ((System.Drawing.Image)(resources.GetObject("SearchBtn.Image")));
            this.SearchBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(46, 22);
            this.SearchBtn.Text = "Search";
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.SplitContainer1);
            this.tabPage2.Controls.Add(this.toolStrip2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1012, 444);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Packets";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.Location = new System.Drawing.Point(3, 28);
            this.SplitContainer1.Name = "SplitContainer1";
            this.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.SplitContainer2);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.rtb2);
            this.SplitContainer1.Size = new System.Drawing.Size(1006, 413);
            this.SplitContainer1.SplitterDistance = 308;
            this.SplitContainer1.TabIndex = 1;
            // 
            // SplitContainer2
            // 
            this.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer2.Name = "SplitContainer2";
            this.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.Controls.Add(this.listBox1);
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.Controls.Add(this.tv1);
            this.SplitContainer2.Size = new System.Drawing.Size(1006, 308);
            this.SplitContainer2.SplitterDistance = 161;
            this.SplitContainer2.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1006, 161);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // tv1
            // 
            this.tv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv1.Location = new System.Drawing.Point(0, 0);
            this.tv1.Name = "tv1";
            this.tv1.Size = new System.Drawing.Size(1006, 143);
            this.tv1.TabIndex = 0;
            this.tv1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv1_AfterSelect);
            // 
            // rtb2
            // 
            this.rtb2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb2.Location = new System.Drawing.Point(0, 0);
            this.rtb2.Name = "rtb2";
            this.rtb2.Size = new System.Drawing.Size(1006, 101);
            this.rtb2.TabIndex = 0;
            this.rtb2.Text = "";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshSB,
            this.CacheSB,
            this.toolStripSeparator1,
            this.LoadFolderSB,
            this.toolStripSeparator2,
            this.toolStripTextBox2,
            this.LabelByteSB,
            this.ByteLabelSB,
            this.toolStripTextBox3,
            this.toolStripSeparator3,
            this.toolStripTextBox4,
            this.CompressIntSB,
            this.DecompressIntSB,
            this.toolStripTextBox5});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(1006, 25);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // RefreshSB
            // 
            this.RefreshSB.Image = ((System.Drawing.Image)(resources.GetObject("RefreshSB.Image")));
            this.RefreshSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshSB.Name = "RefreshSB";
            this.RefreshSB.Size = new System.Drawing.Size(66, 22);
            this.RefreshSB.Text = "Refresh";
            this.RefreshSB.Click += new System.EventHandler(this.RefreshSB_Click);
            // 
            // CacheSB
            // 
            this.CacheSB.Image = ((System.Drawing.Image)(resources.GetObject("CacheSB.Image")));
            this.CacheSB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CacheSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CacheSB.Name = "CacheSB";
            this.CacheSB.Size = new System.Drawing.Size(60, 22);
            this.CacheSB.Text = "Cache";
            this.CacheSB.ToolTipText = "Delete your cached packet data.";
            this.CacheSB.Click += new System.EventHandler(this.CacheSB_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // LoadFolderSB
            // 
            this.LoadFolderSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.LoadFolderSB.Image = ((System.Drawing.Image)(resources.GetObject("LoadFolderSB.Image")));
            this.LoadFolderSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LoadFolderSB.Name = "LoadFolderSB";
            this.LoadFolderSB.Size = new System.Drawing.Size(82, 22);
            this.LoadFolderSB.Text = "Load Folder...";
            this.LoadFolderSB.Click += new System.EventHandler(this.LoadFolderSB_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(100, 25);
            // 
            // LabelByteSB
            // 
            this.LabelByteSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.LabelByteSB.Image = ((System.Drawing.Image)(resources.GetObject("LabelByteSB.Image")));
            this.LabelByteSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LabelByteSB.Name = "LabelByteSB";
            this.LabelByteSB.Size = new System.Drawing.Size(86, 22);
            this.LabelByteSB.Text = "Label -> Bytes";
            this.LabelByteSB.Click += new System.EventHandler(this.LabelByteSB_Click);
            // 
            // ByteLabelSB
            // 
            this.ByteLabelSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ByteLabelSB.Image = ((System.Drawing.Image)(resources.GetObject("ByteLabelSB.Image")));
            this.ByteLabelSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ByteLabelSB.Name = "ByteLabelSB";
            this.ByteLabelSB.Size = new System.Drawing.Size(86, 22);
            this.ByteLabelSB.Text = "Label <- Bytes";
            this.ByteLabelSB.Click += new System.EventHandler(this.ByteLabelSB_Click);
            // 
            // toolStripTextBox3
            // 
            this.toolStripTextBox3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox3.Name = "toolStripTextBox3";
            this.toolStripTextBox3.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox4
            // 
            this.toolStripTextBox4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox4.Name = "toolStripTextBox4";
            this.toolStripTextBox4.Size = new System.Drawing.Size(100, 25);
            // 
            // CompressIntSB
            // 
            this.CompressIntSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CompressIntSB.Image = ((System.Drawing.Image)(resources.GetObject("CompressIntSB.Image")));
            this.CompressIntSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CompressIntSB.Name = "CompressIntSB";
            this.CompressIntSB.Size = new System.Drawing.Size(97, 22);
            this.CompressIntSB.Text = "Compress Int ->";
            this.CompressIntSB.Click += new System.EventHandler(this.CompressIntSB_Click);
            // 
            // DecompressIntSB
            // 
            this.DecompressIntSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DecompressIntSB.Image = ((System.Drawing.Image)(resources.GetObject("DecompressIntSB.Image")));
            this.DecompressIntSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DecompressIntSB.Name = "DecompressIntSB";
            this.DecompressIntSB.Size = new System.Drawing.Size(109, 22);
            this.DecompressIntSB.Text = "<- Decompress Int";
            this.DecompressIntSB.Click += new System.EventHandler(this.DecompressIntSB_Click);
            // 
            // toolStripTextBox5
            // 
            this.toolStripTextBox5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox5.Name = "toolStripTextBox5";
            this.toolStripTextBox5.Size = new System.Drawing.Size(100, 23);
            // 
            // StartBlazeServerBtn
            // 
            this.StartBlazeServerBtn.AutoSize = true;
            this.StartBlazeServerBtn.Location = new System.Drawing.Point(780, 3);
            this.StartBlazeServerBtn.Name = "StartBlazeServerBtn";
            this.StartBlazeServerBtn.Size = new System.Drawing.Size(75, 22);
            this.StartBlazeServerBtn.TabIndex = 1;
            this.StartBlazeServerBtn.Text = "Blaze";
            this.StartBlazeServerBtn.UseVisualStyleBackColor = true;
            this.StartBlazeServerBtn.Click += new System.EventHandler(this.StartBlazeServerBtn_Click);
            // 
            // StartRedirectorServerBtn
            // 
            this.StartRedirectorServerBtn.AutoSize = true;
            this.StartRedirectorServerBtn.Location = new System.Drawing.Point(861, 3);
            this.StartRedirectorServerBtn.Name = "StartRedirectorServerBtn";
            this.StartRedirectorServerBtn.Size = new System.Drawing.Size(75, 22);
            this.StartRedirectorServerBtn.TabIndex = 2;
            this.StartRedirectorServerBtn.Text = "Redirector";
            this.StartRedirectorServerBtn.UseVisualStyleBackColor = true;
            this.StartRedirectorServerBtn.Click += new System.EventHandler(this.StartRedirectorServerBtn_Click);
            // 
            // FileMenubarTLP
            // 
            this.FileMenubarTLP.ColumnCount = 4;
            this.FileMenubarTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FileMenubarTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FileMenubarTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FileMenubarTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FileMenubarTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.FileMenubarTLP.Controls.Add(this.FileMenubarMS, 0, 0);
            this.FileMenubarTLP.Controls.Add(this.StartWebServerBtn, 3, 0);
            this.FileMenubarTLP.Controls.Add(this.StartRedirectorServerBtn, 2, 0);
            this.FileMenubarTLP.Controls.Add(this.StartBlazeServerBtn, 1, 0);
            this.FileMenubarTLP.Dock = System.Windows.Forms.DockStyle.Top;
            this.FileMenubarTLP.Location = new System.Drawing.Point(0, 0);
            this.FileMenubarTLP.Name = "FileMenubarTLP";
            this.FileMenubarTLP.RowCount = 1;
            this.FileMenubarTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FileMenubarTLP.Size = new System.Drawing.Size(1020, 28);
            this.FileMenubarTLP.TabIndex = 4;
            // 
            // StartWebServerBtn
            // 
            this.StartWebServerBtn.AutoSize = true;
            this.StartWebServerBtn.Enabled = false;
            this.StartWebServerBtn.Location = new System.Drawing.Point(942, 3);
            this.StartWebServerBtn.Name = "StartWebServerBtn";
            this.StartWebServerBtn.Size = new System.Drawing.Size(75, 22);
            this.StartWebServerBtn.TabIndex = 5;
            this.StartWebServerBtn.Text = "Frontend";
            this.StartWebServerBtn.UseVisualStyleBackColor = true;
            // 
            // VersionLbl
            // 
            this.VersionLbl.ForeColor = System.Drawing.Color.DimGray;
            this.VersionLbl.Name = "VersionLbl";
            this.VersionLbl.Size = new System.Drawing.Size(1005, 17);
            this.VersionLbl.Spring = true;
            this.VersionLbl.Text = "version number here";
            this.VersionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StatusbarSS
            // 
            this.StatusbarSS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VersionLbl});
            this.StatusbarSS.Location = new System.Drawing.Point(0, 498);
            this.StatusbarSS.Name = "StatusbarSS";
            this.StatusbarSS.Size = new System.Drawing.Size(1020, 22);
            this.StatusbarSS.TabIndex = 1;
            this.StatusbarSS.Text = "statusStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 520);
            this.Controls.Add(this.TabsTC);
            this.Controls.Add(this.StatusbarSS);
            this.Controls.Add(this.FileMenubarTLP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.FileMenubarMS;
            this.Name = "MainForm";
            this.Text = "Blaze Backend for Command & Conquer™ by Xevrac";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FileMenubarMS.ResumeLayout(false);
            this.FileMenubarMS.PerformLayout();
            this.TabsTC.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.SearchbarTS.ResumeLayout(false);
            this.SearchbarTS.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).EndInit();
            this.SplitContainer2.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.FileMenubarTLP.ResumeLayout(false);
            this.FileMenubarTLP.PerformLayout();
            this.StatusbarSS.ResumeLayout(false);
            this.StatusbarSS.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip FileMenubarMS;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LogLevelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OnlyHighMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HighAndMediumMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AllMenuItem;
        private System.Windows.Forms.TabControl TabsTC;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStrip SearchbarTS;
        private System.Windows.Forms.ToolStripTextBox SearchTB;
        private System.Windows.Forms.ToolStripButton SearchBtn;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.RichTextBox LogRTB;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.SplitContainer SplitContainer2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TreeView tv1;
        private System.Windows.Forms.RichTextBox rtb2;
        private System.Windows.Forms.ToolStripButton RefreshSB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton LoadFolderSB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripButton LabelByteSB;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ByteLabelSB;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox4;
        private System.Windows.Forms.ToolStripButton CompressIntSB;
        private System.Windows.Forms.ToolStripButton DecompressIntSB;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox5;
        private System.Windows.Forms.ToolStripMenuItem EditConfigMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PlayerProfileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GenerateProviderIDMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.Button StartBlazeServerBtn;
        private System.Windows.Forms.Button StartRedirectorServerBtn;
        private System.Windows.Forms.TableLayoutPanel FileMenubarTLP;
        private System.Windows.Forms.Button StartWebServerBtn;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripButton CacheSB;
        private System.Windows.Forms.ToolStripStatusLabel VersionLbl;
        private System.Windows.Forms.StatusStrip StatusbarSS;
    }
}

