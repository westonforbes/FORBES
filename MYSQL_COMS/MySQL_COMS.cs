using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace FORBES
{
    /// <summary>
    /// A structure to hold SQL parameters. This is done to prevent SQL injections. Use parameters for any part of a command that a user can influence.
    /// </summary>
    public struct SQL_PARAMETER
    {
        /// <summary>
        /// The sub-string to search for in a command string.
        /// </summary>
        public string ESCAPE_STRING;
        /// <summary>
        /// The string to insert at the location of the found ESCAPE_STRING.
        /// </summary>
        public string STRING_TO_INSERT;
    }
    /// <summary>
    /// This class handles communicating with a MySQL database.
    /// </summary>
    public class MYSQL_COMS
    {
        //Objects
        private readonly string HOST;
        private readonly string DATABASE;
        private readonly string USER;
        private readonly string PASSWORD;
        MySqlConnection CONNECTION = new MySqlConnection();
        readonly LOGGER EVENTS = new LOGGER("Database Connection Log");

        //Properties
        /// <summary>
        /// True when fully connected, false under all other conditions.
        /// </summary>
        public bool FULLY_CONNECTED { get; private set; }

        //Events
        /// <summary>
        /// This event is raised when the connection state is changed. The connection state is embedded in a custom EventArgs
        /// called CONNECTION_CHANGED_EVENT_ARGS.
        /// </summary>
        public event EventHandler CONNECTION_CHANGED; //Create a EventHandler Delegate that we can invoke.
        /// <summary>
        /// Custom EventArgs structure, holds the connection state in FULLY_CONNECTED.
        /// </summary>
        public class CONNECTION_CHANGED_EVENT_ARGS : EventArgs
        {
            /// <summary>
            /// Constructor for CONNECTION_CHANGED_EVENT_ARGS class, a value must be assigned on initialization.
            /// </summary>
            /// <param name="VALUE"> Current connection state.</param>
            public CONNECTION_CHANGED_EVENT_ARGS(bool VALUE)
            {
                this.FULLY_CONNECTED = VALUE;
            }
            /// <summary>
            /// Current Connection State.
            /// </summary>
            public bool FULLY_CONNECTED { get; private set; }
        } //Create a new EventArgument structure.

        //Constructor
        /// <summary>
        /// Constructor for the class, connection parameters must be passed in on initialization.
        /// </summary>
        /// <param name="HOST">The IP address of the server.</param>
        /// <param name="DATABASE">The name of the database.</param>
        /// <param name="USER">The username.</param>
        /// <param name="PASSWORD">The password.</param>
        public MYSQL_COMS(string HOST, string DATABASE, string USER, string PASSWORD)
        {
            EVENTS.LOG_MESSAGE(1, "INITIALIZE");
            this.HOST = HOST;
            this.DATABASE = DATABASE;
            this.USER = USER;
            this.PASSWORD = PASSWORD;
            CONNECTION.StateChange += new StateChangeEventHandler(CONNECTION_STATE_CHANGED); //Setup an internal handler.
        }

        //Methods
        /// <summary>
        /// Connects to the database defined in intialization.
        /// </summary>
        /// <returns>
        /// <para> 0: Connection success.</para>
        /// <para> 1: A connection-level error occurred while opening the connection.</para>
        /// <para> 2: Cannot open a connection without specifying a data source or server.</para>
        /// <para> 3: Unhandled exception type.</para>
        /// </returns>
        public int CONNECT_TO_DB()
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                string CONNECTION_STRING = "server=" + HOST + ";Database=" + DATABASE + ";User ID=" + USER + ";Password=" + PASSWORD;
                EVENTS.LOG_MESSAGE(3, string.Format("Connection string constructed: {0}", CONNECTION_STRING));
                CONNECTION.ConnectionString = CONNECTION_STRING;
                CONNECTION.Open();
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                if (EX.InnerException is MySqlException)
                {
                    EVENTS.LOG_MESSAGE(2, "A connection-level error occurred while opening the connection.");
                    EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                    return 1;
                }
                else if (EX.InnerException is InvalidOperationException)
                {
                    EVENTS.LOG_MESSAGE(2, "Cannot open a connection without specifying a data source or server.");
                    EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                    return 2;
                }
                else
                {
                    EVENTS.LOG_MESSAGE(2, "Unhandled exception type.");
                    EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                    return 3;
                }

            }
            EVENTS.LOG_MESSAGE(3, "Connection success.");
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0;
        }
        /// <summary>
        /// Disconnects from the current database. Note that this method does not indicate success or failure.
        /// </summary>
        public void DISCONNECT_FROM_DB()
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            EVENTS.LOG_MESSAGE(3, "Closing connection.");
            CONNECTION.Close(); //No exceptions are generated by these method calls.
            CONNECTION.Dispose(); //So nothing advanced needs to happen here.
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
        }
        /// <summary>
        /// This method will get a full table from the MySQL database.
        /// </summary>
        /// <param name="TABLE">The name of the table you want to get.</param>
        /// <param name="COLUMN_STRING">A string of all the columns you want to get, separated with a comma. Do not add a space after the comma.</param>
        /// <returns>
        /// <para>Success: A DataTable populated with data from the MySQL table. It will have the same column structure as COLUMN_STRING.</para>
        /// <para>Failure: An empty new DataTable.</para>
        /// </returns>
        public DataTable GET_TABLE(string TABLE, string COLUMN_STRING)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            DataTable DATA_TABLE = new DataTable(); //Create an empty DataTable to return.
            try
            {
                //Setup the query.
                MySqlDataReader READER; //Create a reader object.
                string QUERY = string.Format("SELECT * FROM {0}", TABLE); //Construct the query string.
                READER = EXECUTE_READER(QUERY); //Execute the query.

                //Setup the DataTable that'll hold results.
                string[] COLUMNS = COLUMN_STRING.Split(','); //Figure out the column listing from passed string.
                foreach (string ELEMENT in COLUMNS) //For each column in the listing... 
                    DATA_TABLE.Columns.Add(ELEMENT); //Sdd a column in the DataTable.

                //Fill the DataTable with results from the query.
                if (READER.HasRows) //If data was returned...
                {
                    while (READER.Read())//While there is data in the buffer...
                    {
                        DataRow ROW = DATA_TABLE.NewRow();
                        foreach (string ELEMENT in COLUMNS) //Read each element from the row and add it to the string.
                            ROW[ELEMENT] = READER[ELEMENT];
                        DATA_TABLE.Rows.Add(ROW);
                    }
                    READER.Close();
                }
                else //If no data was returned...
                {
                    EVENTS.LOG_MESSAGE(2, "No data returned from query.");
                    EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                    READER.Close();
                    return new DataTable();
                }

            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return new DataTable();
            }

            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return DATA_TABLE;
        }
        /// <summary>
        /// This method is used to issue database commands. If the command is a hardcoded command, it can be just sent in COMMAND_STRING. If the command
        /// string in any way depended on variables or user inputs, a array of parameters should be sent containing those values, otherwise SQL injections may be possible.
        /// </summary>
        /// <param name="COMMAND_STRING">The command you wish to execute.</param>
        /// <param name="PARAMETERS_ARRAY">The parameters you wish to send. See documentation for the SQL_PARAMETER structure for more details.</param>
        /// <returns>
        /// <para>Success: A value greater than or equal to zero. This value represents the number of rows affected.</para>
        /// <para>Failure: -1</para>
        /// </returns>
        public int EXECUTE_COMMAND(string COMMAND_STRING, SQL_PARAMETER[] PARAMETERS_ARRAY = null)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                int NUMBER_OF_ROWS_AFFECTED; //This will be returned.
                MySqlTransaction TRANSACTION = CONNECTION.BeginTransaction(); //Start connection.
                MySqlCommand COMMAND = CONNECTION.CreateCommand();
                COMMAND.CommandText = COMMAND_STRING; //Set the command literal text.
                if (PARAMETERS_ARRAY != null) //If parameters were passed...
                {
                    foreach (SQL_PARAMETER PARAMETER in PARAMETERS_ARRAY) //For each parameter passed...
                    {
                        COMMAND.Parameters.AddWithValue(PARAMETER.ESCAPE_STRING, PARAMETER.STRING_TO_INSERT); //Find the escape string in the literal string and substitute it with STRING_TO_INSERT.
                    }
                }
                NUMBER_OF_ROWS_AFFECTED = COMMAND.ExecuteNonQuery();
                TRANSACTION.Commit();
                EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
                return NUMBER_OF_ROWS_AFFECTED;
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
            }
            return -1;
        }
        private void CONNECTION_STATE_CHANGED(object SENDER, EventArgs E)
        {
            System.Data.ConnectionState STATE = CONNECTION.State; //Get the current connection status.
            EVENTS.LOG_MESSAGE(1, string.Format("Connection state change detected. Current state : {0}", STATE.ToString())); //Write to log.
                                                                                                                             //Set a simple property FULLY_CONNECTED so that other classes can easily check.
            if (STATE == System.Data.ConnectionState.Open)
                FULLY_CONNECTED = true;
            else
                FULLY_CONNECTED = false;
            var ARGS = new CONNECTION_CHANGED_EVENT_ARGS(FULLY_CONNECTED); //Insert FULLY_CONNECTED state into the arguments that the delegate will carry out.
            CONNECTION_CHANGED?.Invoke(null, ARGS); //Send out the delegate to notify other classes.
        }
        private MySqlDataReader EXECUTE_READER(string QUERY)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                MySqlDataReader READER;
                MySqlCommand COMMAND = new MySqlCommand(QUERY, CONNECTION);
                READER = COMMAND.ExecuteReader();
                EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
                return READER;
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
            }
            return null;
        }
    }
}
