using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FORBES.TABLE_PROCESSOR_NAMESPACE;
using FORBES.TRIANGULATOR_NAMESPACE;


namespace TRIANGULATOR_TEST_APPLICATION
{
    public partial class FORM_MAIN : Form
    {
        TRIANGULATOR TRIANGULATOR_INST = new TRIANGULATOR();
        TABLE_PROCESSOR TABLE_PROCESSOR_INST = new TABLE_PROCESSOR();
        DataTable DATUMS = new DataTable();
        DataTable POINTS = new DataTable();
        public bool INITIALIZED = false; //Prevents the DGV validation from happening on load.
        public FORM_MAIN()
        {
            InitializeComponent();
        }
        private void FORM_LOAD(object sender, EventArgs e)
        {
            SETUP_DGV_DATUMS();
            SETUP_DGV_POINTS();
            SETUP_ALL_LOG_FUNCTIONS();
            PLOT.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
            PLOT.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;
            PLOT.Legends[0].Enabled = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            DGV_DATUMS.Focus();
            INITIALIZED = true;
        }
        private void SETUP_DGV_POINTS()
        {
            while(POINTS.Columns.Count < 11) //Add required columns if they do not already exist.
                POINTS.Columns.Add();

            //Bind to the DaataTable.
            DGV_POINTS.DataSource = POINTS;

            //Set all the column names.
            POINTS.Columns[0].ColumnName = "d1";
            POINTS.Columns[1].ColumnName = "d2";
            POINTS.Columns[2].ColumnName = "d3";
            POINTS.Columns[3].ColumnName = "x̅";
            POINTS.Columns[4].ColumnName = "y̅";
            POINTS.Columns[5].ColumnName = "x(1,2)";
            POINTS.Columns[6].ColumnName = "y(1,2)";
            POINTS.Columns[7].ColumnName = "x(1,3)";
            POINTS.Columns[8].ColumnName = "y(1,3)";
            POINTS.Columns[9].ColumnName = "x(2,3)";
            POINTS.Columns[10].ColumnName = "y(2,3)";

            //Format each column.
            foreach (DataGridViewColumn COLUMN in DGV_POINTS.Columns)
            {
                COLUMN.Width = 60;
                COLUMN.SortMode = DataGridViewColumnSortMode.NotSortable;
                COLUMN.ReadOnly = false;
                COLUMN.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                if (COLUMN.Index > 2)
                {
                    COLUMN.ReadOnly = true;
                    COLUMN.DefaultCellStyle.BackColor = Color.LightGray;
                }
            }


            //Set column special cases.
            DGV_POINTS.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
            //Format settings that apply to all columns.
            DGV_POINTS.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGV_POINTS.AllowUserToAddRows = true;
            DGV_POINTS.AllowUserToDeleteRows = true;
            DGV_POINTS.AllowUserToOrderColumns = false;
            DGV_POINTS.AllowUserToResizeColumns = false;
            DGV_POINTS.AllowUserToResizeRows = false;
            DGV_POINTS.RowHeadersVisible = false;
        }
        private void SETUP_DGV_DATUMS()
        {
            while (DATUMS.Columns.Count < 3) //Add required columns if they do not already exist.
                DATUMS.Columns.Add();

            //Bind to the DaataTable.
            DGV_DATUMS.DataSource = DATUMS;

            //Set all the column names.
            DATUMS.Columns[0].ColumnName = "Datum 1";
            DATUMS.Columns[1].ColumnName = "Datum 2";
            DATUMS.Columns[2].ColumnName = "Datum 3";

            while (DATUMS.Rows.Count < 2)
                DATUMS.Rows.Add();
            DGV_DATUMS.Rows[0].HeaderCell.Value = "X";
            DGV_DATUMS.Rows[1].HeaderCell.Value = "Y";
            DGV_DATUMS.RowHeadersWidth = 50;

            DGV_DATUMS.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DGV_DATUMS.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DGV_DATUMS.ColumnHeadersHeight = 24;
            DGV_DATUMS.AllowUserToAddRows = false;
            DGV_DATUMS.AllowUserToDeleteRows = false;
            DGV_DATUMS.AllowUserToOrderColumns = false;
            DGV_DATUMS.AllowUserToResizeColumns = false;
            DGV_DATUMS.AllowUserToResizeRows = false;
            DGV_DATUMS.Columns[0].Width = 210;
            DGV_DATUMS.Columns[1].Width = 210;
            DGV_DATUMS.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DGV_DATUMS.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV_DATUMS.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV_DATUMS.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void PROCESS_DATA(object sender, EventArgs e)
        {
            PLOT.Series["Datums"].Points.Clear();
            PLOT.Series["Points"].Points.Clear();
            DATA_STRUCT DATA = new DATA_STRUCT();
            POINT_STRUCT OUTPUT_POINT = new POINT_STRUCT();
            INTERSECTIONS_OUT_STRUCT MINOR_POINTS = new INTERSECTIONS_OUT_STRUCT();
            DATA.DATUM_1.X = double.Parse(DATUMS.Rows[0][0].ToString());
            DATA.DATUM_1.Y = double.Parse(DATUMS.Rows[1][0].ToString());
            PLOT.Series["Datums"].Points.AddXY(DATA.DATUM_1.X, DATA.DATUM_1.Y);
            DATA.DATUM_2.X = double.Parse(DATUMS.Rows[0][1].ToString());
            DATA.DATUM_2.Y = double.Parse(DATUMS.Rows[1][1].ToString());
            PLOT.Series["Datums"].Points.AddXY(DATA.DATUM_2.X, DATA.DATUM_2.Y);
            DATA.DATUM_3.X = double.Parse(DATUMS.Rows[0][2].ToString());
            DATA.DATUM_3.Y = double.Parse(DATUMS.Rows[1][2].ToString());
            PLOT.Series["Datums"].Points.AddXY(DATA.DATUM_3.X, DATA.DATUM_3.Y);

            foreach (DataRow ROW in POINTS.Rows)
            {
                if(ROW.ItemArray[0].ToString() != "" && ROW.ItemArray[1].ToString() != "" && ROW.ItemArray[2].ToString() != "")
                {
                    DATA.DISTANCE_TO_DATUM_1 = double.Parse(ROW.ItemArray[0].ToString());
                    DATA.DISTANCE_TO_DATUM_2 = double.Parse(ROW.ItemArray[1].ToString());
                    DATA.DISTANCE_TO_DATUM_3 = double.Parse(ROW.ItemArray[2].ToString());
                    TRIANGULATOR_INST.TRIANGULATE(DATA, out OUTPUT_POINT, out MINOR_POINTS);
                    ROW.SetField(3, Math.Round(OUTPUT_POINT.X,4));
                    ROW.SetField(4, Math.Round(OUTPUT_POINT.Y,4));
                    ROW.SetField(5, Math.Round(MINOR_POINTS.RESULT_GENERATED_BY_1_2.X, 4));
                    ROW.SetField(6, Math.Round(MINOR_POINTS.RESULT_GENERATED_BY_1_2.Y, 4));
                    ROW.SetField(7, Math.Round(MINOR_POINTS.RESULT_GENERATED_BY_1_3.X, 4));
                    ROW.SetField(8, Math.Round(MINOR_POINTS.RESULT_GENERATED_BY_1_3.Y, 4));
                    ROW.SetField(9, Math.Round(MINOR_POINTS.RESULT_GENERATED_BY_2_3.X, 4));
                    ROW.SetField(10, Math.Round(MINOR_POINTS.RESULT_GENERATED_BY_2_3.Y, 4));
                    PLOT.Series["Points"].Points.AddXY(OUTPUT_POINT.X, OUTPUT_POINT.Y);
                    PLOT.Series["1_2"].Points.AddXY(MINOR_POINTS.RESULT_GENERATED_BY_1_2.X, MINOR_POINTS.RESULT_GENERATED_BY_1_2.Y);
                    PLOT.Series["1_3"].Points.AddXY(MINOR_POINTS.RESULT_GENERATED_BY_1_3.X, MINOR_POINTS.RESULT_GENERATED_BY_1_3.Y);
                    PLOT.Series["2_3"].Points.AddXY(MINOR_POINTS.RESULT_GENERATED_BY_2_3.X, MINOR_POINTS.RESULT_GENERATED_BY_2_3.Y);
                    DGV_POINTS.Update();
                }

            }

            
        }
        private void DGV_CHECK_NUMERIC(object SENDER, DataGridViewCellValidatingEventArgs ARG)
        {
            if (!INITIALIZED)
                return;
            double RESULT; //Holds result of the TryParse.
            if (!double.TryParse(Convert.ToString(ARG.FormattedValue),out RESULT) && ARG.FormattedValue.ToString() != "")
            {
                ARG.Cancel = true;
                MessageBox.Show("The data you entered was not numeric, please enter numeric data only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LOAD_DISTANCE_LIST(object sender, EventArgs e)
        {
            OpenFileDialog OPEN_DIAG = new OpenFileDialog();
            OPEN_DIAG.FileName = "";
            OPEN_DIAG.Filter = "Distance List (*.dist)|*.dist|All files (*.*)|*.*";
            OPEN_DIAG.Title = "Open Distance List";
            OPEN_DIAG.FilterIndex = 0;
            OPEN_DIAG.RestoreDirectory = true;
            if (OPEN_DIAG.InitialDirectory == null)
                OPEN_DIAG.InitialDirectory = Application.StartupPath;
            if (OPEN_DIAG.ShowDialog() == DialogResult.OK)
            {
                DataTable INSTRUCTIONS = new DataTable();
                INSTRUCTIONS.Columns.Add();
                INSTRUCTIONS.Rows.Add("CSV");
                TABLE_PROCESSOR_INST.PROCESS_INSTRUCTIONS(ref POINTS, ref INSTRUCTIONS, OPEN_DIAG.FileName,',',null,',');
                SETUP_DGV_POINTS();
            }
        }
        private void BTN_LOAD_DATUMS(object sender, EventArgs e)
        {
            OpenFileDialog OPEN_DIAG = new OpenFileDialog();
            OPEN_DIAG.FileName = "";
            OPEN_DIAG.Filter = "Datum List (*.datum)|*.datum|All files (*.*)|*.*";
            OPEN_DIAG.Title = "Open Datum List";
            OPEN_DIAG.FilterIndex = 0;
            OPEN_DIAG.RestoreDirectory = true;
            if (OPEN_DIAG.InitialDirectory == null)
                OPEN_DIAG.InitialDirectory = Application.StartupPath;
            if (OPEN_DIAG.ShowDialog() == DialogResult.OK)
            {
                DataTable INSTRUCTIONS = new DataTable();
                INSTRUCTIONS.Columns.Add();
                INSTRUCTIONS.Rows.Add("CSV");
                TABLE_PROCESSOR_INST.PROCESS_INSTRUCTIONS(ref DATUMS, ref INSTRUCTIONS, OPEN_DIAG.FileName, ',', null, ',');
                SETUP_DGV_DATUMS();
            }
        }
        private void EXPORT(object sender, EventArgs e)
        {
            SaveFileDialog SAVE_DIAG = new SaveFileDialog();
            SAVE_DIAG.FileName = "";
            SAVE_DIAG.Filter = "Points List (*.points)|*.points|All files (*.*)|*.*";
            SAVE_DIAG.Title = "Save Points List";
            SAVE_DIAG.FilterIndex = 0;
            SAVE_DIAG.RestoreDirectory = true;
            if (SAVE_DIAG.InitialDirectory == null)
                SAVE_DIAG.InitialDirectory = Application.StartupPath;
            if (SAVE_DIAG.ShowDialog() == DialogResult.OK)
            {
                
                DataTable INSTRUCTIONS = new DataTable(); //Create an instruction table to send to the TABLE_PROCESSOR.
                INSTRUCTIONS.Columns.Add(); //Add a column for the instructions.
                INSTRUCTIONS.Rows.Add("WRT"); //All we want to do is write.
                DataRow HEADER = POINTS.NewRow();
                foreach(DataColumn COLUMN in POINTS.Columns)
                {
                    string NAME = COLUMN.ColumnName.Replace(',', '_');
                    HEADER[COLUMN.Ordinal] = NAME;
                }
                POINTS.Rows.InsertAt(HEADER, 0);
                TABLE_PROCESSOR_INST.PROCESS_INSTRUCTIONS(ref POINTS, ref INSTRUCTIONS, null, ',', SAVE_DIAG.FileName, ',');
                POINTS.Rows.RemoveAt(0);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------
        //LOG DISPLAY FUNCTIONS - COMPATIBLE WITH LOGGER CLASS 2020-10-19.

        private Timer LOG_TIMER = new Timer();
        private void SETUP_ALL_LOG_FUNCTIONS()
        {
            SETUP_DGV_LOG();
            LOG_TIMER.Interval = 100; //Set the debounce window for the data added event.
            this.LOG_TIMER.Tick += new System.EventHandler(this.LOG_FORMAT_DATA); //Add a handler for the tick event.
            this.TRCK_VERBOSE.Scroll += new System.EventHandler(this.TRCK_VERBOSE_SCROLL); //Add a handler for verbose level change.
            DGV_LOG.RowsAdded += new DataGridViewRowsAddedEventHandler(LOG_NEW_ENTRY); //Setup the event handler for new data.
            this.BTN_CLEAR_LOG.Click += new System.EventHandler(this.BTN_CLEAR_LOG_CLICK);
        }        
        private void SETUP_DGV_LOG()
        {
            DGV_LOG.DataSource = TRIANGULATOR_INST.EVENTS.LOG;
            DGV_LOG.Columns[0].Width = 60;
            DGV_LOG.Columns[1].Width = 120;
            DGV_LOG.Columns[2].Width = 140;
            DGV_LOG.Columns[3].Width = 60;
            DGV_LOG.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DGV_LOG.Columns[5].Visible = false;
            DGV_LOG.Columns[0].Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGV_LOG.Columns[1].Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGV_LOG.Columns[2].Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGV_LOG.Columns[3].Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGV_LOG.Columns[4].Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGV_LOG.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV_LOG.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV_LOG.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV_LOG.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            LOG_FORMAT_DATA(new object(), new EventArgs()); //Format the new data.
        }       
        private void LOG_NEW_ENTRY(object sender, EventArgs e)
        {
            //This function is called when new rows are added. It resets a timer every time so that
            //updates to the table can happen at the end of an instruction set.
            //The timer is a debouncer, essentially.
            LOG_TIMER.Stop();
            LOG_TIMER.Start();
        }
        private void LOG_FORMAT_DATA(object sender, EventArgs e)
        {
            GRP_BOX_LOG.Enabled = true;
            DGV_LOG.Visible = true;
            LOG_TIMER.Stop();
            //--------------------------------------------------------------------------------------------------
            //Determine the caller of this function and scroll if its the timer.
            if (sender.ToString() != "TRACKBAR" && sender.ToString() != "CHECKBOX")
            {
                for (int i = DGV_LOG.Rows.Count - 1; i >= 0; i--)
                {
                    if (DGV_LOG.Rows[i].Visible == true)
                    {
                        DGV_LOG.FirstDisplayedScrollingRowIndex = i;
                        break;
                    }

                }
            }
            foreach (DataGridViewRow ROW in DGV_LOG.Rows)
            {
                //--------------------------------------------------------------------------------------------------
                //Determine if a row should be visible based on the verbose slider.
                ROW.Visible = true; //First turn on the visibility.
                try
                {
                    if (TRCK_VERBOSE.Value < int.Parse(ROW.Cells[3].Value.ToString())) //If the slider value is less than the row value, its not high enough to be displayed.
                        ROW.Visible = false; //Turn off the visiblity.
                }
                catch (Exception) { }
                //--------------------------------------------------------------------------------------------------
                //Determine the color of the row based off of its verbose level.
                switch (ROW.Cells[3].Value.ToString())
                {
                    case ("1"):
                        ROW.DefaultCellStyle.BackColor = Color.Beige;
                        break;
                    case ("2"):
                        ROW.DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                        break;
                    case ("3"):
                        ROW.DefaultCellStyle.BackColor = Color.BurlyWood;
                        break;
                }
                //--------------------------------------------------------------------------------------------------
                //Recolor special case rows.
                switch (ROW.Cells[5].Value.ToString())
                {
                    case ("1"): //Enter method.
                        ROW.DefaultCellStyle.BackColor = Color.Green;
                        break;
                    case ("2"): //Exception.
                        ROW.DefaultCellStyle.BackColor = Color.Orange;
                        break;
                    case ("3"): //Exit method fail.
                        ROW.DefaultCellStyle.BackColor = Color.Crimson;
                        break;
                    case ("4"): //Exit method success.
                        ROW.DefaultCellStyle.BackColor = Color.LightGreen;
                        break;
                    case ("5"): //Class Initialized.
                        ROW.DefaultCellStyle.BackColor = Color.YellowGreen;
                        break;
                }

            }
        }
        private void TRCK_VERBOSE_SCROLL(object sender, EventArgs e)
        {
            object S = new object();
            S = "TRACKBAR";
            LOG_FORMAT_DATA(S, new EventArgs());
            LBL_VERBOSE.Text = "Verbose Detail Level: " + TRCK_VERBOSE.Value.ToString();
        }
        private void BTN_CLEAR_LOG_CLICK(object sender, EventArgs e)
        {
            TRIANGULATOR_INST.EVENTS.RESET_LOG();
        }
    }
}
