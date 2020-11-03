namespace LOGGER_TEST_APPLICATION
{
    partial class Form1
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
            this.GRP_BOX_LOG = new System.Windows.Forms.GroupBox();
            this.BTN_CLEAR_LOG = new System.Windows.Forms.Button();
            this.LBL_VERBOSE = new System.Windows.Forms.Label();
            this.DGV_LOG = new System.Windows.Forms.DataGridView();
            this.TRCK_VERBOSE = new System.Windows.Forms.TrackBar();
            this.BTN_TEST = new System.Windows.Forms.Button();
            this.BTN_EXCEPTION = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.GRP_BOX_LOG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_LOG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TRCK_VERBOSE)).BeginInit();
            this.SuspendLayout();
            // 
            // GRP_BOX_LOG
            // 
            this.GRP_BOX_LOG.Controls.Add(this.BTN_CLEAR_LOG);
            this.GRP_BOX_LOG.Controls.Add(this.LBL_VERBOSE);
            this.GRP_BOX_LOG.Controls.Add(this.DGV_LOG);
            this.GRP_BOX_LOG.Controls.Add(this.TRCK_VERBOSE);
            this.GRP_BOX_LOG.Location = new System.Drawing.Point(20, 20);
            this.GRP_BOX_LOG.Name = "GRP_BOX_LOG";
            this.GRP_BOX_LOG.Size = new System.Drawing.Size(850, 330);
            this.GRP_BOX_LOG.TabIndex = 113;
            this.GRP_BOX_LOG.TabStop = false;
            this.GRP_BOX_LOG.Text = "Log";
            // 
            // BTN_CLEAR_LOG
            // 
            this.BTN_CLEAR_LOG.Location = new System.Drawing.Point(665, 18);
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
            this.DGV_LOG.Size = new System.Drawing.Size(830, 265);
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
            // BTN_TEST
            // 
            this.BTN_TEST.Location = new System.Drawing.Point(20, 356);
            this.BTN_TEST.Name = "BTN_TEST";
            this.BTN_TEST.Size = new System.Drawing.Size(175, 25);
            this.BTN_TEST.TabIndex = 111;
            this.BTN_TEST.TabStop = false;
            this.BTN_TEST.Text = "Test Main Log";
            this.BTN_TEST.UseVisualStyleBackColor = true;
            this.BTN_TEST.Click += new System.EventHandler(this.BTN_TEST_CLICK);
            // 
            // BTN_EXCEPTION
            // 
            this.BTN_EXCEPTION.Location = new System.Drawing.Point(201, 356);
            this.BTN_EXCEPTION.Name = "BTN_EXCEPTION";
            this.BTN_EXCEPTION.Size = new System.Drawing.Size(175, 25);
            this.BTN_EXCEPTION.TabIndex = 114;
            this.BTN_EXCEPTION.TabStop = false;
            this.BTN_EXCEPTION.Text = "Generate Exception";
            this.BTN_EXCEPTION.UseVisualStyleBackColor = true;
            this.BTN_EXCEPTION.Click += new System.EventHandler(this.BTN_EXCEPTION_CLICK);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(382, 356);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 25);
            this.button1.TabIndex = 115;
            this.button1.TabStop = false;
            this.button1.Text = "Test Alternate Log";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BTN_TEST_ALT_CLICK);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 411);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BTN_EXCEPTION);
            this.Controls.Add(this.BTN_TEST);
            this.Controls.Add(this.GRP_BOX_LOG);
            this.Name = "Form1";
            this.Text = "Logger Library Test Application";
            this.GRP_BOX_LOG.ResumeLayout(false);
            this.GRP_BOX_LOG.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_LOG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TRCK_VERBOSE)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GRP_BOX_LOG;
        private System.Windows.Forms.Button BTN_CLEAR_LOG;
        private System.Windows.Forms.Label LBL_VERBOSE;
        private System.Windows.Forms.DataGridView DGV_LOG;
        private System.Windows.Forms.TrackBar TRCK_VERBOSE;
        private System.Windows.Forms.Button BTN_TEST;
        private System.Windows.Forms.Button BTN_EXCEPTION;
        private System.Windows.Forms.Button button1;
    }
}

