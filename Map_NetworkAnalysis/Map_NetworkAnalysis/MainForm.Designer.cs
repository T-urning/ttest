namespace Map_NetworkAnalysis
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
            //Ensures that any ESRI libraries that have been used are unloaded in the correct order. 
            //Failure to do this may result in random crashes on exit due to the operating system unloading 
            //the libraries in the incorrect order. 
            ESRI.ArcGIS.ADF.COMSupport.AOUninitialize.Shutdown();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.最短路径分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addStopsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addBarriesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.routeSolverMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearResultMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryBySpatialRelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryAttributeMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.queyBySpatialRelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSelectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.后悔药ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unDoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reDoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusBarXY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showFTLayerAttributeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToLayerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLayerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.最短路径分析ToolStripMenuItem,
            this.queryBySpatialRelMenuItem,
            this.后悔药ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(859, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewDoc,
            this.menuOpenDoc,
            this.menuSaveDoc,
            this.menuSaveAs,
            this.menuSeparator,
            this.menuExitApp});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(39, 21);
            this.menuFile.Text = "File";
            // 
            // menuNewDoc
            // 
            this.menuNewDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuNewDoc.Image")));
            this.menuNewDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuNewDoc.Name = "menuNewDoc";
            this.menuNewDoc.Size = new System.Drawing.Size(180, 22);
            this.menuNewDoc.Text = "New Document";
            this.menuNewDoc.Click += new System.EventHandler(this.menuNewDoc_Click);
            // 
            // menuOpenDoc
            // 
            this.menuOpenDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuOpenDoc.Image")));
            this.menuOpenDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuOpenDoc.Name = "menuOpenDoc";
            this.menuOpenDoc.Size = new System.Drawing.Size(180, 22);
            this.menuOpenDoc.Text = "Open Document...";
            this.menuOpenDoc.Click += new System.EventHandler(this.menuOpenDoc_Click);
            // 
            // menuSaveDoc
            // 
            this.menuSaveDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuSaveDoc.Image")));
            this.menuSaveDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuSaveDoc.Name = "menuSaveDoc";
            this.menuSaveDoc.Size = new System.Drawing.Size(180, 22);
            this.menuSaveDoc.Text = "SaveDocument";
            this.menuSaveDoc.Click += new System.EventHandler(this.menuSaveDoc_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(180, 22);
            this.menuSaveAs.Text = "Save As...";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // menuSeparator
            // 
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // menuExitApp
            // 
            this.menuExitApp.Name = "menuExitApp";
            this.menuExitApp.Size = new System.Drawing.Size(180, 22);
            this.menuExitApp.Text = "Exit";
            this.menuExitApp.Click += new System.EventHandler(this.menuExitApp_Click);
            // 
            // 最短路径分析ToolStripMenuItem
            // 
            this.最短路径分析ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addStopsMenuItem,
            this.addBarriesMenuItem,
            this.routeSolverMenuItem,
            this.clearResultMenuItem});
            this.最短路径分析ToolStripMenuItem.Name = "最短路径分析ToolStripMenuItem";
            this.最短路径分析ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.最短路径分析ToolStripMenuItem.Text = "最短路径分析";
            // 
            // addStopsMenuItem
            // 
            this.addStopsMenuItem.Name = "addStopsMenuItem";
            this.addStopsMenuItem.Size = new System.Drawing.Size(148, 22);
            this.addStopsMenuItem.Text = "添加站点";
            this.addStopsMenuItem.Click += new System.EventHandler(this.addStopsMenuItem_Click);
            // 
            // addBarriesMenuItem
            // 
            this.addBarriesMenuItem.Name = "addBarriesMenuItem";
            this.addBarriesMenuItem.Size = new System.Drawing.Size(148, 22);
            this.addBarriesMenuItem.Text = "添加障碍点";
            this.addBarriesMenuItem.Click += new System.EventHandler(this.addBarriesMenuItem_Click);
            // 
            // routeSolverMenuItem
            // 
            this.routeSolverMenuItem.Name = "routeSolverMenuItem";
            this.routeSolverMenuItem.Size = new System.Drawing.Size(148, 22);
            this.routeSolverMenuItem.Text = "执行分析";
            this.routeSolverMenuItem.Click += new System.EventHandler(this.routeSolverMenuItem_Click);
            // 
            // clearResultMenuItem
            // 
            this.clearResultMenuItem.Name = "clearResultMenuItem";
            this.clearResultMenuItem.Size = new System.Drawing.Size(148, 22);
            this.clearResultMenuItem.Text = "清除分析结果";
            this.clearResultMenuItem.Click += new System.EventHandler(this.clearResultMenuItem_Click);
            // 
            // queryBySpatialRelMenuItem
            // 
            this.queryBySpatialRelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queryAttributeMenuItem1,
            this.queyBySpatialRelMenuItem,
            this.clearSelectionMenuItem});
            this.queryBySpatialRelMenuItem.Name = "queryBySpatialRelMenuItem";
            this.queryBySpatialRelMenuItem.Size = new System.Drawing.Size(44, 21);
            this.queryBySpatialRelMenuItem.Text = "查询";
            // 
            // queryAttributeMenuItem1
            // 
            this.queryAttributeMenuItem1.Name = "queryAttributeMenuItem1";
            this.queryAttributeMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.queryAttributeMenuItem1.Text = "按属性查询";
            this.queryAttributeMenuItem1.Click += new System.EventHandler(this.queryAttributeMenuItem1_Click);
            // 
            // queyBySpatialRelMenuItem
            // 
            this.queyBySpatialRelMenuItem.Name = "queyBySpatialRelMenuItem";
            this.queyBySpatialRelMenuItem.Size = new System.Drawing.Size(160, 22);
            this.queyBySpatialRelMenuItem.Text = "按空间关系查询";
            this.queyBySpatialRelMenuItem.Click += new System.EventHandler(this.queyBySpatialRelMenuItem_Click);
            // 
            // clearSelectionMenuItem
            // 
            this.clearSelectionMenuItem.Name = "clearSelectionMenuItem";
            this.clearSelectionMenuItem.Size = new System.Drawing.Size(160, 22);
            this.clearSelectionMenuItem.Text = "清除选择集";
            this.clearSelectionMenuItem.Click += new System.EventHandler(this.clearSelectionMenuItem_Click);
            // 
            // 后悔药ToolStripMenuItem
            // 
            this.后悔药ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unDoMenuItem,
            this.reDoMenuItem});
            this.后悔药ToolStripMenuItem.Name = "后悔药ToolStripMenuItem";
            this.后悔药ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.后悔药ToolStripMenuItem.Text = "前后视图";
            // 
            // unDoMenuItem
            // 
            this.unDoMenuItem.Name = "unDoMenuItem";
            this.unDoMenuItem.Size = new System.Drawing.Size(124, 22);
            this.unDoMenuItem.Text = "上一视图";
            this.unDoMenuItem.Click += new System.EventHandler(this.unDoMenuItem_Click);
            // 
            // reDoMenuItem
            // 
            this.reDoMenuItem.Name = "reDoMenuItem";
            this.reDoMenuItem.Size = new System.Drawing.Size(124, 22);
            this.reDoMenuItem.Text = "后一视图";
            this.reDoMenuItem.Click += new System.EventHandler(this.reDoMenuItem_Click);
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(191, 53);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(668, 466);
            this.axMapControl1.TabIndex = 2;
          
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            this.axMapControl1.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.axMapControl1_OnMapReplaced);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 25);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(859, 28);
            this.axToolbarControl1.TabIndex = 3;
            
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axTOCControl1.Location = new System.Drawing.Point(3, 53);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(188, 466);
            this.axTOCControl1.TabIndex = 4;
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(466, 278);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 5;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 53);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 488);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarXY,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(3, 519);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(856, 22);
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusBar1";
            // 
            // statusBarXY
            // 
            this.statusBarXY.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.statusBarXY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusBarXY.Name = "statusBarXY";
            this.statusBarXY.Size = new System.Drawing.Size(57, 17);
            this.statusBarXY.Text = "Test 123";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showFTLayerAttributeMenuItem,
            this.zoomToLayerMenuItem,
            this.deleteLayerMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(137, 70);
            // 
            // showFTLayerAttributeMenuItem
            // 
            this.showFTLayerAttributeMenuItem.Name = "showFTLayerAttributeMenuItem";
            this.showFTLayerAttributeMenuItem.Size = new System.Drawing.Size(136, 22);
            this.showFTLayerAttributeMenuItem.Text = "属性表";
            this.showFTLayerAttributeMenuItem.Click += new System.EventHandler(this.showFTLayerAttributeMenuItem_Click);
            // 
            // zoomToLayerMenuItem
            // 
            this.zoomToLayerMenuItem.Name = "zoomToLayerMenuItem";
            this.zoomToLayerMenuItem.Size = new System.Drawing.Size(136, 22);
            this.zoomToLayerMenuItem.Text = "缩放至图层";
            this.zoomToLayerMenuItem.Click += new System.EventHandler(this.zoomToLayerMenuItem_Click);
            // 
            // deleteLayerMenuItem
            // 
            this.deleteLayerMenuItem.Name = "deleteLayerMenuItem";
            this.deleteLayerMenuItem.Size = new System.Drawing.Size(136, 22);
            this.deleteLayerMenuItem.Text = "移除图层";
            this.deleteLayerMenuItem.Click += new System.EventHandler(this.deleteLayerMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 541);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "ArcEngine Controls Application";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuNewDoc;
        private System.Windows.Forms.ToolStripMenuItem menuOpenDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuExitApp;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusBarXY;
        private System.Windows.Forms.ToolStripMenuItem 最短路径分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addStopsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addBarriesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem routeSolverMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearResultMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem queryBySpatialRelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryAttributeMenuItem1;

        private System.Windows.Forms.ToolStripMenuItem queyBySpatialRelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSelectionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 后悔药ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unDoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reDoMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem showFTLayerAttributeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToLayerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteLayerMenuItem;
    }
}

