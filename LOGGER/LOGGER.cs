using System;
using System.Data;
using System.Collections.Generic;

namespace FORBES
{
    public class LOGGER
    {
        //Objects
        public int CONSOLE_VERBOSE = 9999;
        public readonly DataTable LOG = new DataTable(); //The public event log for this class instance.
        public static DataTable UNIFIED_LOG = new DataTable(); //The public event log for all instances in the current program.
        public string ENTER_MESSAGE = ">>> Thread entering function.";
        public string EXCEPTION_MESSAGE = "XXX Exception: {0}";
        public string EXIT_FAIL_MESSAGE = "<<< Thread exiting function (fail).";
        public string EXIT_SUCCESS_MESSAGE = "<<< Thread exiting function (success).";
        public string INITIALIZE_MESSAGE = "^^^ Class initialized.";
        private int ENTRY_ID = 0; //Holds how many entries are in the log. Used for entry ID purposes and overflow protection.
        static int UNIFIED_ENTRY_ID = 0;

        //Events
        public event EventHandler LOG_RESET;
        static event EventHandler ALL_LOGS_RESET;

        //Constructor
        public LOGGER(string LOG_NAME)
        {
            ADD_LOG_COLUMNS(); //Add all the columns to the DataTable. 
            LOG.TableName = LOG_NAME; //Set the name of this instances log.
            ALL_LOGS_RESET += new EventHandler(ALL_LOGS_RESET_EVENT);
        }

        //Methods

        private void ALL_LOGS_RESET_EVENT(object sender, EventArgs E)
        {
            RESET_LOG();
        } //When a static event to reset all logs triggers, each instance will clear their logs thru this delegate.
        public void LOG_MESSAGE(int GROUP_ID, string MESSAGE, string EX_MESSAGE = null, [System.Runtime.CompilerServices.CallerMemberName] string FUNCTION = "")
        {
            OVERFLOW_PROTECT(); //Check to make sure the log isn't going to overflow.
            int RES_MSG_FLAG = CHECK_IF_RESERVED_MESSAGE(ref MESSAGE, EX_MESSAGE); //Check if the message is one of the reserved messages.
            DateTime TIME_STAMP = DateTime.Now; //Get the time.
            string CONSOLE_MESSAGE = null; //Construct the console message.
            CONSOLE_MESSAGE = LOG.TableName + ": "; //Construct the console message.
            CONSOLE_MESSAGE += ENTRY_ID.ToString("00000000") + ": "; //Construct the console message.
            CONSOLE_MESSAGE += TIME_STAMP.ToString("s") + ": "; //Construct the console message.
            CONSOLE_MESSAGE += FUNCTION + ": " + MESSAGE; //Construct the console message.
            if (MESSAGE == "") //Special case to output a blank console row, for readability reasons.
                Console.WriteLine(""); //Write a blank line to the console.
            else //If its not a special case...
            {
                UNIFIED_LOG.Rows.Add(LOG.TableName, UNIFIED_ENTRY_ID, ENTRY_ID, TIME_STAMP.ToString("s"), FUNCTION, GROUP_ID, MESSAGE, RES_MSG_FLAG); //Add the message to the log.
                LOG.Rows.Add(LOG.TableName, UNIFIED_ENTRY_ID, ENTRY_ID, TIME_STAMP.ToString("s"), FUNCTION, GROUP_ID, MESSAGE, RES_MSG_FLAG); //Add the message to the log.
                ENTRY_ID++; //Index up the ENTRY_ID. Don't omit/remove this as its used for overflow protection.
                UNIFIED_ENTRY_ID++;
                if (GROUP_ID <= CONSOLE_VERBOSE) Console.WriteLine(CONSOLE_MESSAGE); //Write the console message.
            }
        } //Main logging function.
        public void RESET_LOG()
        {
            LOG.Clear();
            ENTRY_ID = 0; //Reset the entry ID.
            LOG_RESET?.Invoke(null, null); //Raise a event so that other classes can be notified that a reset happened.
        } //Reset the log to its original state.
        public static void RESET_ALL_LOGS()
        {
            UNIFIED_LOG.Clear();
            UNIFIED_ENTRY_ID = 0;
            ALL_LOGS_RESET?.Invoke(null, null); //Raise a event so that other classes can be notified that a reset happened.
        } //Reset the log to its original state.
        private int CHECK_IF_RESERVED_MESSAGE(ref string MESSAGE, string EX_MESSAGE = null)
        {

            switch (MESSAGE)
            {
                case ("ENTER"):
                    MESSAGE = ENTER_MESSAGE;
                    return 1;
                case ("EXCEPTION"):
                    MESSAGE = String.Format(EXCEPTION_MESSAGE, EX_MESSAGE);
                    return 2;
                case ("EXIT_FAIL"):
                    MESSAGE = EXIT_FAIL_MESSAGE;
                    return 3;
                case ("EXIT_SUCCESS"):
                    MESSAGE = EXIT_SUCCESS_MESSAGE;
                    return 4;
                case ("INITIALIZE"):
                    MESSAGE = INITIALIZE_MESSAGE;
                    return 5;
                default:
                    return 0;
            }
        } //Check if the message is reserved and needs to be substituted.
        private void ADD_LOG_COLUMNS() //Adds the appropriate columns to the log.
        {
            List<string> COLUMN_NAMES = new List<string> 
            {"Log Name", "Shared Entry ID", "Entry ID", "ISO 8601 TimeStamp", "Function", "Verbose Level", "Message", "Reserved Message Flag"};
            DataColumnCollection COLLECTION = LOG.Columns;
            DataColumnCollection UNIFIED_COLLECTION = UNIFIED_LOG.Columns;
            foreach(string NAME in COLUMN_NAMES)
            {
                if (!COLLECTION.Contains(NAME))
                    LOG.Columns.Add(NAME);
                if (!UNIFIED_COLLECTION.Contains(NAME))
                    UNIFIED_LOG.Columns.Add(NAME);
            }

        }
        private void OVERFLOW_PROTECT() //Protects the log from overflowing.
        {
            //This function relies on ENTRY_ID being updated properly as doing a DataTable row count repeatedly would be a significant performance hit.
            if (ENTRY_ID > 16000000) //16,777,216 is the maximum row count of a DataTable.
            {
                RESET_LOG();
            }
            if (UNIFIED_ENTRY_ID > 16000000) //16,777,216 is the maximum row count of a DataTable.
            {
                RESET_ALL_LOGS();
            }
        }
    }
}

