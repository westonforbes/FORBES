using System;
using System.Data;
using System.Collections.Generic;
namespace FORBES
{
    /// <summary> This class manages a key logger. It uses the library created by Fabriciorissetto on GitHub.
    ///It works well and is much easier than engineering from the ground up.
    ///https://github.com/fabriciorissetto/KeystrokeAPI pulled 2020-10-28.
    ///I modified the disposal function of the library.
    /// </summary>
    public static class KEY_LOGGER
    {
        //Properties (Initial values set in constructor).

        /// <summary>
        /// Value is true when the key logger is active.
        /// </summary>
        public static bool KEY_LOGGER_ACTIVE { get; private set; }

        //Events
        /// <summary>
        /// This event is raised on every key press.
        /// </summary>
        public static event EventHandler KEY_PRESSED;

        //Objects
        
        /// <summary>
        /// Holds all recorded keys in a List of string format.
        /// </summary>
        public static readonly List<string> KEYS_LIST = new List<string> { };
        /// <summary>
        /// Holds all recorded keys in a DataTable format. DataTable has one column named "Keys".
        /// </summary>
        public static readonly DataTable KEYS_TABLE = new DataTable();
        
        private static Keystroke.API.KeystrokeAPI API_OBJ = new Keystroke.API.KeystrokeAPI(); //Create object.
        private static int ENTRY_COUNT = 0;
        private const int OVERFLOW_LIMIT = 10000;

        //Constructor
        static KEY_LOGGER()
        {
            KEY_LOGGER_ACTIVE = false; //Set the property to off.
            KEYS_TABLE.Columns.Add("Keys"); //Setup the DataTable.
            API_OBJ.CreateKeyboardHook((KEY) => { LOG_KEYS(KEY.ToString()); }); //Install hooks.
        }

        //Public Methods
        /// <summary>
        /// Starts the key logger.
        /// </summary>
        public static void START_KEY_LOGGER()
        {
            CLEAR_KEY_LOGGER(); //Clear the logs.
            KEY_LOGGER_ACTIVE = true; //Set the property to on. This property is used as a blocker to logging keys.
        }
        /// <summary>
        /// Stops the key logger.
        /// </summary>
        public static void PAUSE_KEY_LOGGER()
        {
            KEY_LOGGER_ACTIVE = false; //Set the property to off. This property is used as a blocker to logging keys.
        }
        /// <summary>
        /// Erases all recorded keys.
        /// </summary>
        public static void CLEAR_KEY_LOGGER()
        {
            KEYS_LIST.Clear(); //Erase the list of strings.
            KEYS_TABLE.Clear(); //Erase the DataTable.
        }

        //Private Methods
        private static void LOG_KEYS(string KEY_STRING)
        {
            if (!KEY_LOGGER_ACTIVE) //If the keylogger is not active...
                return; //Return out and don't log any keys.

            KEY_PRESSED?.Invoke(KEY_STRING, null); //Raise an event for the calling class to consume.
            if (ENTRY_COUNT < OVERFLOW_LIMIT) //If the overflow limit has not been reached...
            {
                KEYS_LIST.Add(KEY_STRING); //Add the key to the list of strings.
                KEYS_TABLE.Rows.Add(KEY_STRING); //Add the key to the DataTable.
                ENTRY_COUNT++; //Index up the entry count.
            }
            else //If the overflow limit has been reached...
            {
                CLEAR_KEY_LOGGER(); //Clear the log.
                ENTRY_COUNT = 0; //Reset the count.
            }
        }
    }
}
