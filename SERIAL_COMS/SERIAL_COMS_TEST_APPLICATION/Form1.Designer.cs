namespace SERIAL_COMS_TEST_APPLICATION
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
            this.CMBO_BOX_BAUD_RATES = new System.Windows.Forms.ComboBox();
            this.GRP_BOX_CONNECT = new System.Windows.Forms.GroupBox();
            this.CMBO_BOX_PORTS = new System.Windows.Forms.ComboBox();
            this.LBL_BAUD = new System.Windows.Forms.Label();
            this.BTN_CONNECT = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BNT_ADVANCED = new System.Windows.Forms.Button();
            this.GRP_BOX_DATA = new System.Windows.Forms.GroupBox();
            this.GRP_BOX_CONNECT.SuspendLayout();
            this.SuspendLayout();
            // 
            // CMBO_BOX_BAUD_RATES
            // 
            this.CMBO_BOX_BAUD_RATES.FormattingEnabled = true;
            this.CMBO_BOX_BAUD_RATES.Location = new System.Drawing.Point(10, 80);
            this.CMBO_BOX_BAUD_RATES.Name = "CMBO_BOX_BAUD_RATES";
            this.CMBO_BOX_BAUD_RATES.Size = new System.Drawing.Size(75, 21);
            this.CMBO_BOX_BAUD_RATES.TabIndex = 1;
            // 
            // GRP_BOX_CONNECT
            // 
            this.GRP_BOX_CONNECT.Controls.Add(this.BNT_ADVANCED);
            this.GRP_BOX_CONNECT.Controls.Add(this.label1);
            this.GRP_BOX_CONNECT.Controls.Add(this.CMBO_BOX_PORTS);
            this.GRP_BOX_CONNECT.Controls.Add(this.LBL_BAUD);
            this.GRP_BOX_CONNECT.Controls.Add(this.BTN_CONNECT);
            this.GRP_BOX_CONNECT.Controls.Add(this.CMBO_BOX_BAUD_RATES);
            this.GRP_BOX_CONNECT.Location = new System.Drawing.Point(12, 12);
            this.GRP_BOX_CONNECT.Name = "GRP_BOX_CONNECT";
            this.GRP_BOX_CONNECT.Size = new System.Drawing.Size(95, 195);
            this.GRP_BOX_CONNECT.TabIndex = 3;
            this.GRP_BOX_CONNECT.TabStop = false;
            this.GRP_BOX_CONNECT.Text = "Connection";
            // 
            // CMBO_BOX_PORTS
            // 
            this.CMBO_BOX_PORTS.FormattingEnabled = true;
            this.CMBO_BOX_PORTS.Location = new System.Drawing.Point(10, 35);
            this.CMBO_BOX_PORTS.Name = "CMBO_BOX_PORTS";
            this.CMBO_BOX_PORTS.Size = new System.Drawing.Size(75, 21);
            this.CMBO_BOX_PORTS.TabIndex = 10;
            this.CMBO_BOX_PORTS.DropDown += new System.EventHandler(this.CMBO_ADJUST);
            this.CMBO_BOX_PORTS.DropDownClosed += new System.EventHandler(this.CMBO_MINIMAL_TEXT);
            // 
            // LBL_BAUD
            // 
            this.LBL_BAUD.Location = new System.Drawing.Point(10, 65);
            this.LBL_BAUD.Name = "LBL_BAUD";
            this.LBL_BAUD.Size = new System.Drawing.Size(75, 15);
            this.LBL_BAUD.TabIndex = 9;
            this.LBL_BAUD.Text = "Baud Rate";
            // 
            // BTN_CONNECT
            // 
            this.BTN_CONNECT.Location = new System.Drawing.Point(10, 115);
            this.BTN_CONNECT.Name = "BTN_CONNECT";
            this.BTN_CONNECT.Size = new System.Drawing.Size(75, 30);
            this.BTN_CONNECT.TabIndex = 4;
            this.BTN_CONNECT.Text = "Connect";
            this.BTN_CONNECT.UseVisualStyleBackColor = true;
            this.BTN_CONNECT.Click += new System.EventHandler(this.CONNECT);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Port";
            // 
            // BNT_ADVANCED
            // 
            this.BNT_ADVANCED.Location = new System.Drawing.Point(10, 151);
            this.BNT_ADVANCED.Name = "BNT_ADVANCED";
            this.BNT_ADVANCED.Size = new System.Drawing.Size(75, 30);
            this.BNT_ADVANCED.TabIndex = 12;
            this.BNT_ADVANCED.Text = "Advanced";
            this.BNT_ADVANCED.UseVisualStyleBackColor = true;
            // 
            // GRP_BOX_DATA
            // 
            this.GRP_BOX_DATA.Location = new System.Drawing.Point(113, 12);
            this.GRP_BOX_DATA.Name = "GRP_BOX_DATA";
            this.GRP_BOX_DATA.Size = new System.Drawing.Size(427, 195);
            this.GRP_BOX_DATA.TabIndex = 4;
            this.GRP_BOX_DATA.TabStop = false;
            this.GRP_BOX_DATA.Text = "Serial Data";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 220);
            this.Controls.Add(this.GRP_BOX_DATA);
            this.Controls.Add(this.GRP_BOX_CONNECT);
            this.Name = "Form1";
            this.Text = "Form1";
            this.GRP_BOX_CONNECT.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox CMBO_BOX_BAUD_RATES;
        private System.Windows.Forms.GroupBox GRP_BOX_CONNECT;
        private System.Windows.Forms.Button BTN_CONNECT;
        private System.Windows.Forms.Label LBL_BAUD;
        private System.Windows.Forms.ComboBox CMBO_BOX_PORTS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BNT_ADVANCED;
        private System.Windows.Forms.GroupBox GRP_BOX_DATA;
    }
}

