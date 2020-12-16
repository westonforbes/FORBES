using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FORBES.SERIAL_COMS_NAMESPACE;


namespace SERIAL_COMS_TEST_APPLICATION
{
    public partial class Form1 : Form
    {
        SERIAL_COM CONNECTION = new SERIAL_COM();
        public Form1()
        {
            InitializeComponent();
            CONNECTION_GROUP_BOX_INITIALIZE();
        }
        //-----------------------------------------------------------------------

        DataTable PORTS = new DataTable();

        //Initializers (psuedo-constructors)
        private void CONNECTION_GROUP_BOX_INITIALIZE()
        {
            CONNECTION.DISCONNECTED += new EventHandler(DISCONNECTED_EVENT);
            CONNECTION.PORTS_LIST_CHANGED += new EventHandler(PORTS_LIST_CHANGED_EVENT);
            CONNECTION.MESSAGE_RECIEVED += new EventHandler(MESSAGE_RECIEVED);
            GET_BAUD_RATES();
            GET_COM_PORTS();
            CONNECTION_CONTROLS_SWITCH(false);
            CMBO_BOX_PORTS.DropDownStyle = ComboBoxStyle.DropDownList;
        } //Call this method on form load to setup everything in the connection groupbox.

        //EventHandler Methods (accessed by worker threads, not the main UI thread)
        private void PORTS_LIST_CHANGED_EVENT(object SENDER, EventArgs E)
        {
            GET_COM_PORTS();
        } //When any of the available ports change, a event is raised which is handled by this method.
        private void DISCONNECTED_EVENT(object SENDER, EventArgs E)
        {
            CONNECTION_CONTROLS_SWITCH(false);
        } //When the program is connected to a port and it disconnects (whether thru proper means or unexpected), this method is called.

        private void MESSAGE_RECIEVED(object SENDER, EventArgs E)
        {
            if (!InvokeRequired)
            {
                while (CONNECTION.BUFFER.Count > 0)
                {
                    listBox1.Items.Add(CONNECTION.BUFFER.Dequeue());
                }
            }
            else
            {
                Invoke(new Action<object, EventArgs>(MESSAGE_RECIEVED), SENDER, E); //Invoke this same function using the UI thread rather than the POLL_TIMER thread.
            }
        }

        //Multithread access methods.
        private void GET_COM_PORTS(object SENDER = null, EventArgs E = null)
        {
            if (!InvokeRequired)
            {

                bool PASS = CONNECTION.GET_COM_PORTS(ref PORTS);
                if (PASS)
                {
                    CMBO_BOX_PORTS.Items.Clear();
                    foreach (DataRow ROW in PORTS.Rows)
                    {
                        CMBO_BOX_PORTS.Items.Add(ROW["Caption"].ToString());
                    }
                }
            }
            else
            {
                Invoke(new Action<object, EventArgs>(GET_COM_PORTS), SENDER, E); //Invoke this same function using the UI thread rather than the POLL_TIMER thread.
            }


        } //Populates the DataTable PORTS with port info.
        private void CONNECTION_CONTROLS_SWITCH(bool CONNECTED)
        {
            if (!InvokeRequired) //Check if a cross-thread invoke is required, it will be if the POLL_TIMER worker thread is whats accessing this method.
            {
                CMBO_BOX_BAUD_RATES.Enabled = !CONNECTED;
                CMBO_BOX_PORTS.Enabled = !CONNECTED;
                if (CONNECTED)
                {
                    BTN_CONNECT.Text = "Disconnect";

                }
                else
                {
                    BTN_CONNECT.Text = "Connect";
                }
            }
            else
            {
                Invoke(new Action<bool>(CONNECTION_CONTROLS_SWITCH), false); //Invoke this same function using the UI thread rather than the POLL_TIMER thread.
            }

        } //Changes the state of controls.

        //Methods
        private void GET_BAUD_RATES(object SENDER = null, EventArgs E = null)
        {
            List<string> BAUD_RATES = new List<string> { };
            CONNECTION.GET_BAUD_RATES(out BAUD_RATES);
            CMBO_BOX_BAUD_RATES.DataSource = BAUD_RATES;
            CMBO_BOX_BAUD_RATES.SelectedItem = "9600";
        } //Gets a list of common baud rates.
        private void CONNECT(object SENDER = null, EventArgs E = null)
        {
            if (!CONNECTION.CONNECTED)
            {

                int BAUD_RATE; //Create a baud rate variable, as the SerialPort parameter BaudRate cannot be used as a out param of TryParse.
                bool SUCCESS = int.TryParse(CMBO_BOX_BAUD_RATES.SelectedItem.ToString(), out BAUD_RATE); //Try to convert the baud rate string.
                PORT_SETTINGS SETTINGS = new PORT_SETTINGS();
                SETTINGS.PORT_NAME = CMBO_BOX_PORTS.Text.ToString(); //Set the port name to the value selected.
                SETTINGS.BAUD_RATE = BAUD_RATE; //Write it to the SerialPort parameter.
                SETTINGS.PARITY = System.IO.Ports.Parity.None;
                SETTINGS.STOP_BITS = System.IO.Ports.StopBits.One;
                SETTINGS.DATA_BITS = 8;
                SETTINGS.RTS_ENABLE = false;
                SETTINGS.READ_TIMEOUT = 1000;
                SETTINGS.WRITE_TIMEOUT = 1000;


                int CONNECTION_RESULT = -1;
                if (SUCCESS) //If the parsing was successful...
                    CONNECTION_RESULT = CONNECTION.OPEN_SERIAL_PORT(SETTINGS); //Open the serial port.
            }
            else
            {
                CONNECTION.CLOSE_SERIAL_PORT();
            }
            CONNECTION_CONTROLS_SWITCH(CONNECTION.CONNECTED);
        } //Connects to a serial port.
        private void CMBO_ADJUST(object SENDER, EventArgs e)
        {
            GET_COM_PORTS(); //Get the list of available COM ports.
            var SENDER_CMBO_BOX = (ComboBox)SENDER;
            int WIDTH = SENDER_CMBO_BOX.DropDownWidth;
            Graphics GRAPHICS = SENDER_CMBO_BOX.CreateGraphics();
            Font FONT = SENDER_CMBO_BOX.Font;

            int VERT_SCROLLBAR_WIDTH = (SENDER_CMBO_BOX.Items.Count > SENDER_CMBO_BOX.MaxDropDownItems)
                    ? SystemInformation.VerticalScrollBarWidth : 0;

            var itemsList = SENDER_CMBO_BOX.Items.Cast<object>().Select(item => item.ToString());

            foreach (string S in itemsList)
            {
                int NEW_WIDTH = (int)GRAPHICS.MeasureString(S, FONT).Width + VERT_SCROLLBAR_WIDTH;

                if (WIDTH < NEW_WIDTH)
                {
                    WIDTH = NEW_WIDTH;
                }
            }

            SENDER_CMBO_BOX.DropDownWidth = WIDTH;
        } //Expands the ComboBox drop-down to fit the longest string in the list.
        private void CMBO_MINIMAL_TEXT(object SENDER, EventArgs E)
        {
            try
            {
                string TXT = "";
                if (CMBO_BOX_PORTS.SelectedItem != null)
                    TXT = CMBO_BOX_PORTS.SelectedItem.ToString();
                string WRITE_TEXT = null;
                foreach (DataRow ROW in PORTS.Rows)
                {
                    string ROW_CAPTION = ROW["Caption"].ToString();
                    if (TXT == ROW_CAPTION)
                    {
                        WRITE_TEXT = ROW["Name"].ToString();
                        break;
                    }
                }
                int INDEX = CMBO_BOX_PORTS.SelectedIndex;
                CMBO_BOX_PORTS.Items.RemoveAt(INDEX);
                CMBO_BOX_PORTS.Items.Insert(INDEX, WRITE_TEXT);
                CMBO_BOX_PORTS.SelectedIndex = INDEX;
                //this.BeginInvoke((MethodInvoker)delegate { this.CMBO_BOX_PORTS.Text = WRITE_TEXT; });
            }
            catch (Exception) { }

        } //Changes the text in the combobox to a shorter COM name.
    }
}
