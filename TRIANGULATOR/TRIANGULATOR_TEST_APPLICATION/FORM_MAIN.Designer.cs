namespace TRIANGULATOR_TEST_APPLICATION
{
    partial class FORM_MAIN
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FORM_MAIN));
            this.BTN_PROCESS = new System.Windows.Forms.Button();
            this.DGV_POINTS = new System.Windows.Forms.DataGridView();
            this.DGV_DATUMS = new System.Windows.Forms.DataGridView();
            this.GRP_BOX_LOG = new System.Windows.Forms.GroupBox();
            this.BTN_CLEAR_LOG = new System.Windows.Forms.Button();
            this.LBL_VERBOSE = new System.Windows.Forms.Label();
            this.DGV_LOG = new System.Windows.Forms.DataGridView();
            this.TRCK_VERBOSE = new System.Windows.Forms.TrackBar();
            this.PLOT = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.BTN_LOAD_DISTANCE = new System.Windows.Forms.Button();
            this.GRP_BOX_DATA = new System.Windows.Forms.GroupBox();
            this.GRP_BOX_GRAPH = new System.Windows.Forms.GroupBox();
            this.GRP_BOX_CONTROLS = new System.Windows.Forms.GroupBox();
            this.BTN_EXPORT = new System.Windows.Forms.Button();
            this.BTN_LOAD_DATUM = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_POINTS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_DATUMS)).BeginInit();
            this.GRP_BOX_LOG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_LOG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TRCK_VERBOSE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PLOT)).BeginInit();
            this.GRP_BOX_DATA.SuspendLayout();
            this.GRP_BOX_GRAPH.SuspendLayout();
            this.GRP_BOX_CONTROLS.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTN_PROCESS
            // 
            this.BTN_PROCESS.Location = new System.Drawing.Point(10, 90);
            this.BTN_PROCESS.Name = "BTN_PROCESS";
            this.BTN_PROCESS.Size = new System.Drawing.Size(350, 25);
            this.BTN_PROCESS.TabIndex = 4;
            this.BTN_PROCESS.Text = "Process Data";
            this.BTN_PROCESS.UseVisualStyleBackColor = true;
            this.BTN_PROCESS.Click += new System.EventHandler(this.PROCESS_DATA);
            // 
            // DGV_POINTS
            // 
            this.DGV_POINTS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_POINTS.Location = new System.Drawing.Point(10, 95);
            this.DGV_POINTS.Name = "DGV_POINTS";
            this.DGV_POINTS.Size = new System.Drawing.Size(680, 160);
            this.DGV_POINTS.TabIndex = 1;
            this.DGV_POINTS.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGV_CHECK_NUMERIC);
            // 
            // DGV_DATUMS
            // 
            this.DGV_DATUMS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_DATUMS.Location = new System.Drawing.Point(10, 20);
            this.DGV_DATUMS.Name = "DGV_DATUMS";
            this.DGV_DATUMS.Size = new System.Drawing.Size(680, 70);
            this.DGV_DATUMS.TabIndex = 0;
            this.DGV_DATUMS.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGV_CHECK_NUMERIC);
            // 
            // GRP_BOX_LOG
            // 
            this.GRP_BOX_LOG.Controls.Add(this.BTN_CLEAR_LOG);
            this.GRP_BOX_LOG.Controls.Add(this.LBL_VERBOSE);
            this.GRP_BOX_LOG.Controls.Add(this.DGV_LOG);
            this.GRP_BOX_LOG.Controls.Add(this.TRCK_VERBOSE);
            this.GRP_BOX_LOG.Location = new System.Drawing.Point(15, 295);
            this.GRP_BOX_LOG.Name = "GRP_BOX_LOG";
            this.GRP_BOX_LOG.Size = new System.Drawing.Size(700, 330);
            this.GRP_BOX_LOG.TabIndex = 112;
            this.GRP_BOX_LOG.TabStop = false;
            this.GRP_BOX_LOG.Text = "TRIANGULATOR Class Log";
            // 
            // BTN_CLEAR_LOG
            // 
            this.BTN_CLEAR_LOG.Location = new System.Drawing.Point(515, 20);
            this.BTN_CLEAR_LOG.Name = "BTN_CLEAR_LOG";
            this.BTN_CLEAR_LOG.Size = new System.Drawing.Size(175, 25);
            this.BTN_CLEAR_LOG.TabIndex = 7;
            this.BTN_CLEAR_LOG.TabStop = false;
            this.BTN_CLEAR_LOG.Text = "Clear Data Log";
            this.BTN_CLEAR_LOG.UseVisualStyleBackColor = true;
            // 
            // LBL_VERBOSE
            // 
            this.LBL_VERBOSE.AutoSize = true;
            this.LBL_VERBOSE.Location = new System.Drawing.Point(76, 24);
            this.LBL_VERBOSE.Name = "LBL_VERBOSE";
            this.LBL_VERBOSE.Size = new System.Drawing.Size(117, 13);
            this.LBL_VERBOSE.TabIndex = 110;
            this.LBL_VERBOSE.Text = "Verbose Detail Level: 3";
            // 
            // DGV_LOG
            // 
            this.DGV_LOG.AllowUserToAddRows = false;
            this.DGV_LOG.AllowUserToDeleteRows = false;
            this.DGV_LOG.AllowUserToResizeRows = false;
            this.DGV_LOG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_LOG.Location = new System.Drawing.Point(10, 50);
            this.DGV_LOG.Name = "DGV_LOG";
            this.DGV_LOG.ReadOnly = true;
            this.DGV_LOG.RowHeadersVisible = false;
            this.DGV_LOG.Size = new System.Drawing.Size(680, 265);
            this.DGV_LOG.TabIndex = 99;
            // 
            // TRCK_VERBOSE
            // 
            this.TRCK_VERBOSE.LargeChange = 1;
            this.TRCK_VERBOSE.Location = new System.Drawing.Point(10, 20);
            this.TRCK_VERBOSE.Maximum = 3;
            this.TRCK_VERBOSE.Minimum = 1;
            this.TRCK_VERBOSE.Name = "TRCK_VERBOSE";
            this.TRCK_VERBOSE.Size = new System.Drawing.Size(60, 45);
            this.TRCK_VERBOSE.TabIndex = 109;
            this.TRCK_VERBOSE.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TRCK_VERBOSE.Value = 3;
            // 
            // PLOT
            // 
            this.PLOT.BackColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX2.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackSecondaryColor = System.Drawing.Color.Transparent;
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.PLOT.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.PLOT.Legends.Add(legend1);
            this.PLOT.Location = new System.Drawing.Point(10, 20);
            this.PLOT.Name = "PLOT";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Points";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Legend = "Legend1";
            series2.Name = "Datums";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.Legend = "Legend1";
            series3.Name = "1_2";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series4.Legend = "Legend1";
            series4.Name = "1_3";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series5.Legend = "Legend1";
            series5.Name = "2_3";
            this.PLOT.Series.Add(series1);
            this.PLOT.Series.Add(series2);
            this.PLOT.Series.Add(series3);
            this.PLOT.Series.Add(series4);
            this.PLOT.Series.Add(series5);
            this.PLOT.Size = new System.Drawing.Size(350, 240);
            this.PLOT.TabIndex = 113;
            this.PLOT.Text = "chart1";
            // 
            // BTN_LOAD_DISTANCE
            // 
            this.BTN_LOAD_DISTANCE.Location = new System.Drawing.Point(10, 55);
            this.BTN_LOAD_DISTANCE.Name = "BTN_LOAD_DISTANCE";
            this.BTN_LOAD_DISTANCE.Size = new System.Drawing.Size(350, 25);
            this.BTN_LOAD_DISTANCE.TabIndex = 3;
            this.BTN_LOAD_DISTANCE.Text = "Load Distance List";
            this.BTN_LOAD_DISTANCE.UseVisualStyleBackColor = true;
            this.BTN_LOAD_DISTANCE.Click += new System.EventHandler(this.LOAD_DISTANCE_LIST);
            // 
            // GRP_BOX_DATA
            // 
            this.GRP_BOX_DATA.Controls.Add(this.DGV_DATUMS);
            this.GRP_BOX_DATA.Controls.Add(this.DGV_POINTS);
            this.GRP_BOX_DATA.Location = new System.Drawing.Point(15, 15);
            this.GRP_BOX_DATA.Name = "GRP_BOX_DATA";
            this.GRP_BOX_DATA.Size = new System.Drawing.Size(700, 270);
            this.GRP_BOX_DATA.TabIndex = 115;
            this.GRP_BOX_DATA.TabStop = false;
            this.GRP_BOX_DATA.Text = "Data";
            // 
            // GRP_BOX_GRAPH
            // 
            this.GRP_BOX_GRAPH.Controls.Add(this.PLOT);
            this.GRP_BOX_GRAPH.Location = new System.Drawing.Point(735, 15);
            this.GRP_BOX_GRAPH.Name = "GRP_BOX_GRAPH";
            this.GRP_BOX_GRAPH.Size = new System.Drawing.Size(370, 270);
            this.GRP_BOX_GRAPH.TabIndex = 116;
            this.GRP_BOX_GRAPH.TabStop = false;
            this.GRP_BOX_GRAPH.Text = "Plot";
            // 
            // GRP_BOX_CONTROLS
            // 
            this.GRP_BOX_CONTROLS.Controls.Add(this.BTN_EXPORT);
            this.GRP_BOX_CONTROLS.Controls.Add(this.BTN_LOAD_DATUM);
            this.GRP_BOX_CONTROLS.Controls.Add(this.BTN_PROCESS);
            this.GRP_BOX_CONTROLS.Controls.Add(this.BTN_LOAD_DISTANCE);
            this.GRP_BOX_CONTROLS.Location = new System.Drawing.Point(735, 295);
            this.GRP_BOX_CONTROLS.Name = "GRP_BOX_CONTROLS";
            this.GRP_BOX_CONTROLS.Size = new System.Drawing.Size(370, 330);
            this.GRP_BOX_CONTROLS.TabIndex = 117;
            this.GRP_BOX_CONTROLS.TabStop = false;
            this.GRP_BOX_CONTROLS.Text = "Controls";
            // 
            // BTN_EXPORT
            // 
            this.BTN_EXPORT.Location = new System.Drawing.Point(10, 125);
            this.BTN_EXPORT.Name = "BTN_EXPORT";
            this.BTN_EXPORT.Size = new System.Drawing.Size(350, 25);
            this.BTN_EXPORT.TabIndex = 5;
            this.BTN_EXPORT.Text = "Export Data";
            this.BTN_EXPORT.UseVisualStyleBackColor = true;
            this.BTN_EXPORT.Click += new System.EventHandler(this.EXPORT);
            // 
            // BTN_LOAD_DATUM
            // 
            this.BTN_LOAD_DATUM.Location = new System.Drawing.Point(10, 20);
            this.BTN_LOAD_DATUM.Name = "BTN_LOAD_DATUM";
            this.BTN_LOAD_DATUM.Size = new System.Drawing.Size(350, 25);
            this.BTN_LOAD_DATUM.TabIndex = 2;
            this.BTN_LOAD_DATUM.Text = "Load Datum List";
            this.BTN_LOAD_DATUM.UseVisualStyleBackColor = true;
            this.BTN_LOAD_DATUM.Click += new System.EventHandler(this.BTN_LOAD_DATUMS);
            // 
            // FORM_MAIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 641);
            this.Controls.Add(this.GRP_BOX_CONTROLS);
            this.Controls.Add(this.GRP_BOX_GRAPH);
            this.Controls.Add(this.GRP_BOX_DATA);
            this.Controls.Add(this.GRP_BOX_LOG);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FORM_MAIN";
            this.Text = "Point Processor";
            this.Load += new System.EventHandler(this.FORM_LOAD);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_POINTS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_DATUMS)).EndInit();
            this.GRP_BOX_LOG.ResumeLayout(false);
            this.GRP_BOX_LOG.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_LOG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TRCK_VERBOSE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PLOT)).EndInit();
            this.GRP_BOX_DATA.ResumeLayout(false);
            this.GRP_BOX_GRAPH.ResumeLayout(false);
            this.GRP_BOX_CONTROLS.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BTN_PROCESS;
        private System.Windows.Forms.DataGridView DGV_POINTS;
        private System.Windows.Forms.DataGridView DGV_DATUMS;
        private System.Windows.Forms.GroupBox GRP_BOX_LOG;
        private System.Windows.Forms.Label LBL_VERBOSE;
        private System.Windows.Forms.DataGridView DGV_LOG;
        private System.Windows.Forms.TrackBar TRCK_VERBOSE;
        private System.Windows.Forms.Button BTN_CLEAR_LOG;
        private System.Windows.Forms.DataVisualization.Charting.Chart PLOT;
        private System.Windows.Forms.Button BTN_LOAD_DISTANCE;
        private System.Windows.Forms.GroupBox GRP_BOX_DATA;
        private System.Windows.Forms.GroupBox GRP_BOX_GRAPH;
        private System.Windows.Forms.GroupBox GRP_BOX_CONTROLS;
        private System.Windows.Forms.Button BTN_EXPORT;
        private System.Windows.Forms.Button BTN_LOAD_DATUM;
    }
}

