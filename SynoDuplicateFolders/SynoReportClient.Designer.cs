namespace SynoDuplicateFolders
{
    partial class SynoReportClient
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("NAS");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSharesReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportVolumeReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.KnownHosts = new System.Windows.Forms.TreeView();
            this.HostInformationTabs = new System.Windows.Forms.TabControl();
            this.tab1VolumeHistoricChart = new System.Windows.Forms.TabPage();
            this.tab2DuplicateCandidates = new System.Windows.Forms.TabPage();
            this.tab3PieCharts = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelPieCharts = new System.Windows.Forms.TableLayoutPanel();
            this.tab4FileDetails = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelFileDetails = new System.Windows.Forms.TableLayoutPanel();
            this.cmbFileDetails = new System.Windows.Forms.ComboBox();
            this.NoDataNotification = new System.Windows.Forms.Label();
            this.contextMenuAddServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuHost = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsStripMenuDividider = new System.Windows.Forms.ToolStripSeparator();
            this.removeServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuPiecharts = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showFreeVsUsedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showContentTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeHistoricChart1 = new SynoDuplicateFolders.Controls.VolumeHistoricChart();
            this.duplicateCandidatesView1 = new SynoDuplicateFolders.Controls.DuplicateCandidatesView();
            this.timestampTrackBarPieCharts = new SynoDuplicateFolders.Controls.TimeStampTrackBar();
            this.volumePies = new SynoDuplicateFolders.Controls.ChartGrid();
            this.dataGridView1 = new SynoDuplicateFolders.Controls.SynoReportDataGridView();
            this.timestampTrackBarFileDetails = new SynoDuplicateFolders.Controls.TimeStampTrackBar();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.HostInformationTabs.SuspendLayout();
            this.tab1VolumeHistoricChart.SuspendLayout();
            this.tab2DuplicateCandidates.SuspendLayout();
            this.tab3PieCharts.SuspendLayout();
            this.tableLayoutPanelPieCharts.SuspendLayout();
            this.tab4FileDetails.SuspendLayout();
            this.tableLayoutPanelFileDetails.SuspendLayout();
            this.contextMenuAddServer.SuspendLayout();
            this.contextMenuHost.SuspendLayout();
            this.contextMenuPiecharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolsStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1371, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportSharesReportToolStripMenuItem,
            this.exportVolumeReportToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem2.Text = "File";
            // 
            // exportSharesReportToolStripMenuItem
            // 
            this.exportSharesReportToolStripMenuItem.Enabled = false;
            this.exportSharesReportToolStripMenuItem.Name = "exportSharesReportToolStripMenuItem";
            this.exportSharesReportToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.exportSharesReportToolStripMenuItem.Text = "Export Shares Report ...";
            this.exportSharesReportToolStripMenuItem.Click += new System.EventHandler(this.exportSharesReportToolStripMenuItem_Click);
            // 
            // exportVolumeReportToolStripMenuItem
            // 
            this.exportVolumeReportToolStripMenuItem.Enabled = false;
            this.exportVolumeReportToolStripMenuItem.Name = "exportVolumeReportToolStripMenuItem";
            this.exportVolumeReportToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.exportVolumeReportToolStripMenuItem.Text = "Export Volume Report ...";
            this.exportVolumeReportToolStripMenuItem.Click += new System.EventHandler(this.exportVolumeReportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(198, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsStripMenuItem
            // 
            this.toolsStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem});
            this.toolsStripMenuItem.Name = "toolsStripMenuItem";
            this.toolsStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsStripMenuItem.Text = "Tools";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferences_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 706);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1371, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.MainSplitContainer);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 24);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1371, 682);
            this.MainPanel.TabIndex = 3;
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.KnownHosts);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.HostInformationTabs);
            this.MainSplitContainer.Panel2.Controls.Add(this.NoDataNotification);
            this.MainSplitContainer.Size = new System.Drawing.Size(1371, 682);
            this.MainSplitContainer.SplitterDistance = 119;
            this.MainSplitContainer.TabIndex = 1;
            // 
            // KnownHosts
            // 
            this.KnownHosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KnownHosts.Location = new System.Drawing.Point(0, 0);
            this.KnownHosts.Name = "KnownHosts";
            treeNode1.Name = "Node0";
            treeNode1.Text = "NAS";
            this.KnownHosts.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.KnownHosts.ShowNodeToolTips = true;
            this.KnownHosts.Size = new System.Drawing.Size(119, 682);
            this.KnownHosts.TabIndex = 0;
            this.KnownHosts.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.KnownHosts_NodeMouseClick);
            this.KnownHosts.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KnownHosts_KeyPress);
            // 
            // HostInformationTabs
            // 
            this.HostInformationTabs.Controls.Add(this.tab1VolumeHistoricChart);
            this.HostInformationTabs.Controls.Add(this.tab2DuplicateCandidates);
            this.HostInformationTabs.Controls.Add(this.tab3PieCharts);
            this.HostInformationTabs.Controls.Add(this.tab4FileDetails);
            this.HostInformationTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HostInformationTabs.Location = new System.Drawing.Point(0, 0);
            this.HostInformationTabs.Name = "HostInformationTabs";
            this.HostInformationTabs.SelectedIndex = 0;
            this.HostInformationTabs.Size = new System.Drawing.Size(1248, 682);
            this.HostInformationTabs.TabIndex = 1;
            this.HostInformationTabs.Visible = false;
            this.HostInformationTabs.SelectedIndexChanged += new System.EventHandler(this.HostInformationTabs_SelectedIndexChanged);
            // 
            // tab1VolumeHistoricChart
            // 
            this.tab1VolumeHistoricChart.Controls.Add(this.volumeHistoricChart1);
            this.tab1VolumeHistoricChart.Location = new System.Drawing.Point(4, 22);
            this.tab1VolumeHistoricChart.Name = "tab1VolumeHistoricChart";
            this.tab1VolumeHistoricChart.Padding = new System.Windows.Forms.Padding(3);
            this.tab1VolumeHistoricChart.Size = new System.Drawing.Size(1240, 656);
            this.tab1VolumeHistoricChart.TabIndex = 0;
            this.tab1VolumeHistoricChart.Text = "Volume Usage";
            this.tab1VolumeHistoricChart.UseVisualStyleBackColor = true;
            // 
            // tab2DuplicateCandidates
            // 
            this.tab2DuplicateCandidates.Controls.Add(this.duplicateCandidatesView1);
            this.tab2DuplicateCandidates.Location = new System.Drawing.Point(4, 22);
            this.tab2DuplicateCandidates.Name = "tab2DuplicateCandidates";
            this.tab2DuplicateCandidates.Padding = new System.Windows.Forms.Padding(3);
            this.tab2DuplicateCandidates.Size = new System.Drawing.Size(1240, 656);
            this.tab2DuplicateCandidates.TabIndex = 1;
            this.tab2DuplicateCandidates.Text = "Duplicate Candidates";
            this.tab2DuplicateCandidates.UseVisualStyleBackColor = true;
            // 
            // tab3PieCharts
            // 
            this.tab3PieCharts.Controls.Add(this.tableLayoutPanelPieCharts);
            this.tab3PieCharts.Location = new System.Drawing.Point(4, 22);
            this.tab3PieCharts.Name = "tab3PieCharts";
            this.tab3PieCharts.Padding = new System.Windows.Forms.Padding(3);
            this.tab3PieCharts.Size = new System.Drawing.Size(1240, 656);
            this.tab3PieCharts.TabIndex = 2;
            this.tab3PieCharts.Text = "PieCharts";
            this.tab3PieCharts.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelPieCharts
            // 
            this.tableLayoutPanelPieCharts.ColumnCount = 1;
            this.tableLayoutPanelPieCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPieCharts.Controls.Add(this.timestampTrackBarPieCharts, 0, 1);
            this.tableLayoutPanelPieCharts.Controls.Add(this.volumePies, 0, 0);
            this.tableLayoutPanelPieCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPieCharts.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelPieCharts.Name = "tableLayoutPanelPieCharts";
            this.tableLayoutPanelPieCharts.RowCount = 2;
            this.tableLayoutPanelPieCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPieCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanelPieCharts.Size = new System.Drawing.Size(1234, 650);
            this.tableLayoutPanelPieCharts.TabIndex = 4;
            // 
            // tab4FileDetails
            // 
            this.tab4FileDetails.Controls.Add(this.tableLayoutPanelFileDetails);
            this.tab4FileDetails.Location = new System.Drawing.Point(4, 22);
            this.tab4FileDetails.Name = "tab4FileDetails";
            this.tab4FileDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tab4FileDetails.Size = new System.Drawing.Size(1240, 656);
            this.tab4FileDetails.TabIndex = 3;
            this.tab4FileDetails.Text = "File Details";
            this.tab4FileDetails.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelFileDetails
            // 
            this.tableLayoutPanelFileDetails.ColumnCount = 1;
            this.tableLayoutPanelFileDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFileDetails.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanelFileDetails.Controls.Add(this.timestampTrackBarFileDetails, 0, 2);
            this.tableLayoutPanelFileDetails.Controls.Add(this.cmbFileDetails, 0, 0);
            this.tableLayoutPanelFileDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFileDetails.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelFileDetails.Name = "tableLayoutPanelFileDetails";
            this.tableLayoutPanelFileDetails.RowCount = 3;
            this.tableLayoutPanelFileDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanelFileDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFileDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanelFileDetails.Size = new System.Drawing.Size(1234, 650);
            this.tableLayoutPanelFileDetails.TabIndex = 0;
            // 
            // cmbFileDetails
            // 
            this.cmbFileDetails.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbFileDetails.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileDetails.FormattingEnabled = true;
            this.cmbFileDetails.Items.AddRange(new object[] {
            "Owners",
            "Most Modified",
            "Least Modified",
            "Groups"});
            this.cmbFileDetails.Location = new System.Drawing.Point(514, 25);
            this.cmbFileDetails.Name = "cmbFileDetails";
            this.cmbFileDetails.Size = new System.Drawing.Size(206, 21);
            this.cmbFileDetails.TabIndex = 1;
            this.cmbFileDetails.SelectedIndexChanged += new System.EventHandler(this.cmbFileDetails_SelectedIndexChanged);
            // 
            // NoDataNotification
            // 
            this.NoDataNotification.AutoSize = true;
            this.NoDataNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NoDataNotification.Location = new System.Drawing.Point(0, 0);
            this.NoDataNotification.Name = "NoDataNotification";
            this.NoDataNotification.Size = new System.Drawing.Size(238, 13);
            this.NoDataNotification.TabIndex = 1;
            this.NoDataNotification.Text = "Refresh the data from your NAS to fill this panel...";
            // 
            // contextMenuAddServer
            // 
            this.contextMenuAddServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addServerToolStripMenuItem});
            this.contextMenuAddServer.Name = "contextMenuAddServer";
            this.contextMenuAddServer.Size = new System.Drawing.Size(143, 26);
            // 
            // addServerToolStripMenuItem
            // 
            this.addServerToolStripMenuItem.Name = "addServerToolStripMenuItem";
            this.addServerToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.addServerToolStripMenuItem.Text = "Add server ...";
            this.addServerToolStripMenuItem.Click += new System.EventHandler(this.addServerToolStripMenuItem_Click);
            // 
            // contextMenuHost
            // 
            this.contextMenuHost.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.toolsStripMenuDividider,
            this.removeServerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.propertiesToolStripMenuItem});
            this.contextMenuHost.Name = "contextMenuHost";
            this.contextMenuHost.Size = new System.Drawing.Size(152, 82);
            this.contextMenuHost.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuHost_ItemClicked);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            // 
            // toolsStripMenuDividider
            // 
            this.toolsStripMenuDividider.Name = "toolsStripMenuDividider";
            this.toolsStripMenuDividider.Size = new System.Drawing.Size(148, 6);
            // 
            // removeServerToolStripMenuItem
            // 
            this.removeServerToolStripMenuItem.Name = "removeServerToolStripMenuItem";
            this.removeServerToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.removeServerToolStripMenuItem.Text = "Remove server";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(148, 6);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            // 
            // contextMenuPiecharts
            // 
            this.contextMenuPiecharts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showFreeVsUsedToolStripMenuItem,
            this.showContentTypesToolStripMenuItem});
            this.contextMenuPiecharts.Name = "contextMenuPiecharts";
            this.contextMenuPiecharts.Size = new System.Drawing.Size(182, 48);
            // 
            // showFreeVsUsedToolStripMenuItem
            // 
            this.showFreeVsUsedToolStripMenuItem.Name = "showFreeVsUsedToolStripMenuItem";
            this.showFreeVsUsedToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.showFreeVsUsedToolStripMenuItem.Text = "Show Free vs Used";
            this.showFreeVsUsedToolStripMenuItem.Click += new System.EventHandler(this.showFreeVsUsedToolStripMenuItem_Click);
            // 
            // showContentTypesToolStripMenuItem
            // 
            this.showContentTypesToolStripMenuItem.Name = "showContentTypesToolStripMenuItem";
            this.showContentTypesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.showContentTypesToolStripMenuItem.Text = "Show Content Types";
            this.showContentTypesToolStripMenuItem.Click += new System.EventHandler(this.showContentTypesToolStripMenuItem_Click);
            // 
            // volumeHistoricChart1
            // 
            this.volumeHistoricChart1.Configuration = null;
            this.volumeHistoricChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volumeHistoricChart1.EarliestTime = null;
            this.volumeHistoricChart1.Location = new System.Drawing.Point(3, 3);
            this.volumeHistoricChart1.Name = "volumeHistoricChart1";
            this.volumeHistoricChart1.ShowIndividualStoragePoolUsage = false;
            this.volumeHistoricChart1.Size = new System.Drawing.Size(1234, 650);
            this.volumeHistoricChart1.TabIndex = 0;
            this.volumeHistoricChart1.TabStop = false;
            this.volumeHistoricChart1.TimeRange = null;
            this.volumeHistoricChart1.View = SynoDuplicateFolders.Controls.vhcViewMode.Shares;
            // 
            // duplicateCandidatesView1
            // 
            this.duplicateCandidatesView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.duplicateCandidatesView1.ExclusionSource = null;
            this.duplicateCandidatesView1.HostName = null;
            this.duplicateCandidatesView1.Location = new System.Drawing.Point(3, 3);
            this.duplicateCandidatesView1.MaximumComparable = 3;
            this.duplicateCandidatesView1.Name = "duplicateCandidatesView1";
            this.duplicateCandidatesView1.Size = new System.Drawing.Size(1234, 650);
            this.duplicateCandidatesView1.TabIndex = 0;
            // 
            // timestampTrackBarPieCharts
            // 
            this.timestampTrackBarPieCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timestampTrackBarPieCharts.Location = new System.Drawing.Point(3, 567);
            this.timestampTrackBarPieCharts.Name = "timestampTrackBarPieCharts";
            this.timestampTrackBarPieCharts.Size = new System.Drawing.Size(1228, 80);
            this.timestampTrackBarPieCharts.TabIndex = 0;
            this.timestampTrackBarPieCharts.Value = new System.DateTime(((long)(0)));
            this.timestampTrackBarPieCharts.ValueChanged += new System.EventHandler(this.TimeStampTrackBar_ValueChanged);
            // 
            // volumePies
            // 
            this.volumePies.Configuration = null;
            this.volumePies.DataSource = null;
            this.volumePies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volumePies.Location = new System.Drawing.Point(3, 3);
            this.volumePies.Name = "volumePies";
            this.volumePies.PercentageFreeOnly = false;
            this.volumePies.Size = new System.Drawing.Size(1228, 558);
            this.volumePies.TabIndex = 0;
            this.volumePies.MouseClick += new System.Windows.Forms.MouseEventHandler(this.volumePies_MouseClick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 75);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1228, 486);
            this.dataGridView1.TabIndex = 2;
            // 
            // timestampTrackBarFileDetails
            // 
            this.timestampTrackBarFileDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timestampTrackBarFileDetails.Location = new System.Drawing.Point(3, 567);
            this.timestampTrackBarFileDetails.Name = "timestampTrackBarFileDetails";
            this.timestampTrackBarFileDetails.Size = new System.Drawing.Size(1228, 80);
            this.timestampTrackBarFileDetails.TabIndex = 0;
            this.timestampTrackBarFileDetails.Value = new System.DateTime(((long)(0)));
            this.timestampTrackBarFileDetails.ValueChanged += new System.EventHandler(this.TimeStampTrackBar_ValueChanged);
            // 
            // SynoReportClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 728);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "SynoReportClient";
            this.Text = "SynoReport Client";
            this.Load += new System.EventHandler(this.SynoReportClient_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            this.MainSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.HostInformationTabs.ResumeLayout(false);
            this.tab1VolumeHistoricChart.ResumeLayout(false);
            this.tab2DuplicateCandidates.ResumeLayout(false);
            this.tab3PieCharts.ResumeLayout(false);
            this.tableLayoutPanelPieCharts.ResumeLayout(false);
            this.tab4FileDetails.ResumeLayout(false);
            this.tableLayoutPanelFileDetails.ResumeLayout(false);
            this.contextMenuAddServer.ResumeLayout(false);
            this.contextMenuHost.ResumeLayout(false);
            this.contextMenuPiecharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ToolStripMenuItem toolsStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.TreeView KnownHosts;
        private System.Windows.Forms.TabControl HostInformationTabs;
        private System.Windows.Forms.TabPage tab1VolumeHistoricChart;
        private System.Windows.Forms.TabPage tab2DuplicateCandidates;
        private System.Windows.Forms.TabPage tab3PieCharts;

        private System.Windows.Forms.ContextMenuStrip contextMenuAddServer;
        private System.Windows.Forms.ToolStripMenuItem addServerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuHost;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;

        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripSeparator toolsStripMenuDividider;
        private System.Windows.Forms.ToolStripMenuItem removeServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPieCharts;

        private Controls.DuplicateCandidatesView duplicateCandidatesView1;
        private Controls.TimeStampTrackBar timestampTrackBarPieCharts;
        private Controls.TimeStampTrackBar timestampTrackBarFileDetails;
        private Controls.ChartGrid volumePies;
        private Controls.VolumeHistoricChart volumeHistoricChart1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exportSharesReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportVolumeReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabPage tab4FileDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFileDetails;
        private Controls.SynoReportDataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmbFileDetails;
        private System.Windows.Forms.Label NoDataNotification;
        private System.Windows.Forms.ContextMenuStrip contextMenuPiecharts;
        private System.Windows.Forms.ToolStripMenuItem showFreeVsUsedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showContentTypesToolStripMenuItem;
    }
}

