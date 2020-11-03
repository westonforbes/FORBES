using System;
using System.Drawing;
using System.Windows.Forms;
using FORBES;

namespace LOGGER_TEST_APPLICATION
{
    public partial class Form1 : Form
    {
        LOGGER EVENTS = new LOGGER("Events Log");
        LOGGER EVENTS_ALT = new LOGGER("Alternate Events Log");
        public Form1()
        {
            InitializeComponent();
            SETUP_ALL_LOG_FUNCTIONS();
            EVENTS.LOG_MESSAGE(1, "INITIALIZE");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
        }
        //Log view controls-------------------------------------------------
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
            DGV_LOG.DataSource = LOGGER.UNIFIED_LOG;
            for(int i = 0; i < 7; i++)
            {
                DGV_LOG.Columns[i].Resizable = DataGridViewTriState.False;
                DGV_LOG.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            DGV_LOG.Columns[0].Width = 120;
            DGV_LOG.Columns[1].Width = 60;
            DGV_LOG.Columns[2].Width = 60;
            DGV_LOG.Columns[3].Width = 120;
            DGV_LOG.Columns[4].Width = 140;
            DGV_LOG.Columns[5].Width = 60;
            DGV_LOG.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DGV_LOG.Columns[7].Visible = false;
            
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
                    if (TRCK_VERBOSE.Value < int.Parse(ROW.Cells[5].Value.ToString())) //If the slider value is less than the row value, its not high enough to be displayed.
                        ROW.Visible = false; //Turn off the visiblity.
                }
                catch (Exception) { }
                //--------------------------------------------------------------------------------------------------
                //Determine the color of the row based off of its verbose level.
                switch (ROW.Cells[5].Value.ToString())
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
                switch (ROW.Cells[7].Value.ToString())
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
            LOGGER.RESET_ALL_LOGS();
        }
        //------------------------------------------------------------------
        private void BTN_TEST_CLICK(object sender, EventArgs e)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            TEST_METHOD();
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
        }
        private void TEST_METHOD()
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            EVENTS.CONSOLE_VERBOSE = 3;
            EVENTS.LOG_MESSAGE(1, "Heres a message in verbose level 1.");
            EVENTS.LOG_MESSAGE(2, "Heres a message in verbose level 2.");
            EVENTS.LOG_MESSAGE(3, "Heres a message in verbose level 3.");
            EVENTS.CONSOLE_VERBOSE = 1;
            EVENTS.LOG_MESSAGE(3, "This message wont be in the console.");
            EVENTS.LOG_MESSAGE(3, "CONSOLE_VERBOSE is set too low for the console to print this.");
            EVENTS.CONSOLE_VERBOSE = 9999;
            EVENTS.LOG_MESSAGE(2, "CONSOLE_VERBOSE set back to normal.");
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
        }
        private void BTN_EXCEPTION_CLICK(object sender, EventArgs e)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            GENERATE_EXCEPTION();
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
        }
        private void GENERATE_EXCEPTION()
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            int ZERO = 0;
            try
            {
                double RESULT = 5 / ZERO;
            }
            catch(Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return;
            }
        }
        private void BTN_TEST_ALT_CLICK(object sender, EventArgs e)
        {
            EVENTS_ALT.LOG_MESSAGE(1, "ENTER");
            TEST_METHOD_ALT();
            EVENTS_ALT.LOG_MESSAGE(1, "EXIT_SUCCESS");
        }
        private void TEST_METHOD_ALT()
        {
            EVENTS_ALT.LOG_MESSAGE(1, "ENTER");
            EVENTS_ALT.CONSOLE_VERBOSE = 3;
            EVENTS_ALT.LOG_MESSAGE(1, "Heres a message in verbose level 1.");
            EVENTS_ALT.LOG_MESSAGE(2, "Heres a message in verbose level 2.");
            EVENTS_ALT.LOG_MESSAGE(3, "Heres a message in verbose level 3.");
            EVENTS_ALT.CONSOLE_VERBOSE = 1;
            EVENTS_ALT.LOG_MESSAGE(3, "This message wont be in the console.");
            EVENTS_ALT.LOG_MESSAGE(3, "CONSOLE_VERBOSE is set too low for the console to print this.");
            EVENTS_ALT.CONSOLE_VERBOSE = 9999;
            EVENTS_ALT.LOG_MESSAGE(2, "CONSOLE_VERBOSE set back to normal.");
            EVENTS_ALT.LOG_MESSAGE(1, "EXIT_SUCCESS");
        }
    }
}
