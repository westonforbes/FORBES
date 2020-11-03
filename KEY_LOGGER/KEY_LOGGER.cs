using System;
using System.Data;
using System.Collections.Generic;
namespace FORBES
{
    public static class KEY_LOGGER
    {
        //These methods manage the keylogger. They use the library created by Fabriciorissetto on GitHub.
        //It works well and is much easier than engineering from the ground up.
        //https://github.com/fabriciorissetto/KeystrokeAPI pulled 2020-10-28.
        //I modified the disposal function of the library.

        //Properties (Initial values set in constructor).
        public static bool KEY_LOGGER_ACTIVE { get; private set; }

        //Events
        public static event EventHandler KEY_PRESSED;

        //Objects
        private static Keystroke.API.KeystrokeAPI API_OBJ = new Keystroke.API.KeystrokeAPI(); //Create object.
        public static readonly List<string> KEYS_LIST = new List<string> { };
        public static readonly DataTable KEYS_TABLE = new DataTable();
        private static int ENTRY_COUNT = 0;
        private const int OVERFLOW_LIMIT = 10000;

        //Constructor
        static KEY_LOGGER()
        {
            KEY_LOGGER_ACTIVE = false; //Set the property to off.
            KEYS_TABLE.Columns.Add("Keys"); //Setup the DataTable.
            API_OBJ.CreateKeyboardHook((KEY) => { LOG_KEYS(KEY.ToString()); }); //Install hooks.
        }

        //Methods
        public static void START_KEY_LOGGER()
        {
            CLEAR_KEY_LOGGER(); //Clear the logs.
            KEY_LOGGER_ACTIVE = true; //Set the property to on. This property is used as a blocker to logging keys.
        }
        public static void PAUSE_KEY_LOGGER()
        {
            KEY_LOGGER_ACTIVE = false; //Set the property to off. This property is used as a blocker to logging keys.
        }
        public static void CLEAR_KEY_LOGGER()
        {
            KEYS_LIST.Clear(); //Erase the list of strings.
            KEYS_TABLE.Clear(); //Erase the DataTable.
        }
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
