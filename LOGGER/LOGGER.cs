using System;
using System.Data;
using System.Collections.Generic;

namespace FORBES.LOGGER_NAMESPACE
{
    /// <summary>
    /// This class creates and manages a event log. It stores the log in a public readonly DataTable and it outputs to the console.
    /// </summary>
    public class LOGGER
    {
        //Objects
        /// <summary>
        /// This is the threshold value to output to the console. Any messages with an assigned value higher than this value will not print to the console.
        /// Higher value messages will continue to appear in the log.
        /// </summary>
        public int CONSOLE_VERBOSE = 9999;
        /// <summary>
        /// Holds all the messages of the current class instance.
        /// </summary>
        /// <remarks>
        /// <para> Column 0: Log Name: Name of the log as set by the constructor argument.</para>
        /// <para> Column 1: Shared Entry ID: Entry ID of the message, this value is statically controlled (its shared by all instances of this class).Value will roll over at 16,000,000 and log will reset.</para>
        /// <para> Column 2: Entry ID: The Entry ID of the message. Value will roll over at 16,000,000 and log will reset.</para>
        /// <para> Column 3: IS8601 TimeStamp: DateTime.Now stored in "s" sortable ISO8601 format.</para>
        /// <para> Column 4: Function: The name of the function that called the LOG_MESSAGE method.</para>
        /// <para> Column 5: Verbose Level: The level of the message. This is picked by the programmer, its intended to allow for filtering.</para>
        /// <para> Column 6: Message: The message you wish to write. Note that using string.Format can allow for sending compound messages.</para>
        /// <para> Column 7: Reserved Message Flag: Indicates if the message was one of the class defined reserved messages.</para>
        /// </remarks>
        public readonly DataTable LOG = new DataTable();
        /// <summary>
        /// Holds all the messages for all instances of this class.
        /// </summary>
        /// <remarks>
        /// <para> Column 0: Log Name: Name of the log as set by the constructor argument.</para>
        /// <para> Column 1: Shared Entry ID: Entry ID of the message, this value is statically controlled (its shared by all instances of this class).Value will roll over at 16,000,000 and log will reset.</para>
        /// <para> Column 2: Entry ID: The Entry ID of the message. Value will roll over at 16,000,000 and log will reset.</para>
        /// <para> Column 3: IS8601 TimeStamp: DateTime.Now stored in "s" sortable ISO8601 format.</para>
        /// <para> Column 4: Function: The name of the function that called the LOG_MESSAGE method.</para>
        /// <para> Column 5: Verbose Level: The level of the message. This is picked by the programmer, its intended to allow for filtering.</para>
        /// <para> Column 6: Message: The message you wish to write. Note that using string.Format can allow for sending compound messages.</para>
        /// <para> Column 7: Reserved Message Flag: Indicates if the message was one of the class defined reserved messages.</para>
        /// </remarks>
        public static DataTable UNIFIED_LOG = new DataTable(); //The public event log for all instances in the current program.
        /// <summary>
        /// Reserved message that is activated when "ENTER" is sent. Default message is "Thread entering function.".
        /// </summary>
        public string ENTER_MESSAGE = ">>> Thread entering function.";
        /// <summary>
        /// Reserved message that is activated when "EXCEPTION" is sent. Default message is "Exception: {0}". 
        /// Note that this reserved message expects the LOG_MESSAGE optional argument EX_MESSAGE to also be passed.
        /// </summary>
        public string EXCEPTION_MESSAGE = "XXX Exception: {0}";
        /// <summary>
        /// Reserved message that is activated when "EXIT_FAIL" is sent. Default message is "Thread exiting function (fail).".
        /// </summary>
        public string EXIT_FAIL_MESSAGE = "<<< Thread exiting function (fail).";
        /// <summary>
        /// Reserved message that is activated when "EXIT_SUCCESS" is sent. Default message is "Thread exiting function (success).".
        /// </summary>
        public string EXIT_SUCCESS_MESSAGE = "<<< Thread exiting function (success).";
        /// <summary>
        /// Reserved message that is activated when "INITIALIZE" is sent. Default message is "Class initialized.".
        /// </summary>
        public string INITIALIZE_MESSAGE = "^^^ Class initialized.";
        private int ENTRY_ID = 0; //Holds how many entries are in the log. Used for entry ID purposes and overflow protection.
        static int UNIFIED_ENTRY_ID = 0;

        //Events
        /// <summary>
        /// This event is raised when the current class instance has its logs reset. 
        /// This may be triggered by the user or happen automatically as a overflow protection.
        /// </summary>
        public event EventHandler LOG_RESET;
        /// <summary>
        /// This event is raised when the log for all instances is reset.
        /// This may be triggered by the user or happen automatically as a overflow protection.
        /// </summary>
        public static event EventHandler ALL_LOGS_RESET;

        //Constructor
        /// <summary>
        /// Class constructor, requires a name for the log.
        /// </summary>
        /// <param name="LOG_NAME"></param>
        public LOGGER(string LOG_NAME)
        {
            ADD_LOG_COLUMNS(); //Add all the columns to the DataTable. 
            LOG.TableName = LOG_NAME; //Set the name of this instances log.
            ALL_LOGS_RESET += new EventHandler(ALL_LOGS_RESET_EVENT);
        }

        //Methods
        /// <summary>
        /// This method is the main point of interaction with this class. When called, it will log the attached message to the logs and display it in the console.
        /// </summary>
        /// <param name="GROUP_ID"> Value picked by the programmer to help filter/catagorize messages. Developing a schema for this is up to the programmer.</param>
        /// <param name="MESSAGE"> The message to be logged.</param>
        /// <param name="EX_MESSAGE"> Used to easily pass the Exception text when using the reserved message "EXCEPTION"</param>
        /// <param name="FUNCTION">Do not use this, this is used to get the calling method name. Manually assigning this would set the FUNCTION entry in the log.</param>
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
        }
        /// <summary>
        /// This method removes all entries from the current class instance log.
        /// </summary>
        public void RESET_LOG()
        {
            LOG.Clear();
            ENTRY_ID = 0; //Reset the entry ID.
            LOG_RESET?.Invoke(null, null); //Raise a event so that other classes can be notified that a reset happened.
        }
        /// <summary>
        /// This method performs a resets the log that is shared by all instances. Note that it will not reset each instances private log, just the shared one.
        /// </summary>
        public static void RESET_ALL_LOGS()
        {
            UNIFIED_LOG.Clear();
            UNIFIED_ENTRY_ID = 0;
            ALL_LOGS_RESET?.Invoke(null, null); //Raise a event so that other classes can be notified that a reset happened.
        }
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
        private void ALL_LOGS_RESET_EVENT(object sender, EventArgs E)
        {
            RESET_LOG();
        } //When a static event to reset all logs triggers, each instance will clear their logs thru this delegate.
    }
}

