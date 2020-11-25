using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using FORBES.LOGGER_NAMESPACE;

namespace FORBES.TABLE_PROCESSOR_NAMESPACE
{
    /// <summary>
    /// This static class handles some simple operations related to dealing with the INSTRUCTION_TABLE.
    /// </summary>
    public static class INSTRUCTION_SET
    {
        /// <summary>
        /// The event log for the class.
        /// </summary>
        public static LOGGER EVENTS = new LOGGER("Instruction Log");

        /// <summary>
        /// This function will load in a INSTRUCTION_TABLE from a file.
        /// </summary>
        /// <param name="PATH">The filepath of the file to read in.</param>
        /// <returns>A DataTable formatted in the INSTRUCTION_TABLE format.</returns>
        public static DataTable LOAD_INSTRUCTION_FILE(string PATH)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            DataTable INSTRUCTION_TABLE = new DataTable();
            try { INSTRUCTION_TABLE.Columns.Add("Instructions"); } //Create a column to put the data in. If this isn't the first time, an exception will be thrown.
            catch (Exception) { } //Catch the exception if the INSTRUCTIONS column already exists.
            try //Do the following in a "try" as exceptions can be thrown by the StreamReader.
            {
                using (var READER = new StreamReader(PATH)) //Create a StreamReader for the file.
                {
                    EVENTS.LOG_MESSAGE(3, string.Format("Reading file: {0}", PATH));
                    while (!READER.EndOfStream) //While there is data to read...
                    {
                        string DATA_STRING = READER.ReadLine();
                        INSTRUCTION_TABLE.Rows.Add(DATA_STRING); //Add each line to the DataTable.
                        EVENTS.LOG_MESSAGE(3, string.Format("Reading line: {0}", DATA_STRING));
                    }
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return new DataTable();
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return INSTRUCTION_TABLE; //Return the table up to the calling function.
        }

        /// <summary>
        /// This function will write a copy of the instruction set to a file.
        /// </summary>
        /// <param name="INSTRUCTION_TABLE">A instruction DataTable following the format created by CREATE_INSTRUCTION_TABLE.</param>
        /// <param name="PATH">The full filepath of the new file.</param>
        /// <returns>True on success, false on error.</returns>
        public static bool WRITE_INSTRUCTION_FILE(DataTable INSTRUCTION_TABLE, string PATH)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                using (StreamWriter WRITER = File.CreateText(PATH))
                {
                    foreach (DataRow ROW in INSTRUCTION_TABLE.Rows) //Go through each row in the table.
                    {
                        WRITER.WriteLine(ROW.ItemArray[0].ToString()); //Write the line to the output file.
                        EVENTS.LOG_MESSAGE(3, "WRITE_INSTRUCTION_FILE", "Writing instruction...");
                    }
                    WRITER.Close(); //Close up shop.
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return false;
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return true; //Return the table up to the calling function.
        }

        /// <summary>
        /// This function creates a properly formatted instruction table.
        /// </summary>
        /// <returns>A single column DataTable with the first row containing entry "CSV".</returns>
        public static DataTable CREATE_INSTRUCTION_TABLE()
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            DataTable INSTRUCTION_TABLE = new DataTable();
            INSTRUCTION_TABLE.Columns.Add("Instructions");
            INSTRUCTION_TABLE.Rows.Add("CSV");
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return INSTRUCTION_TABLE;
        }
    }

    /// <summary>
    /// This class processes CSV files and can perform a whole host of transformations on them. The main access point is PROCESS_INSTRUCTIONS.
    /// See documentation for a list of available commands.
    /// </summary>
    public class TABLE_PROCESSOR
    {
        //Objects
        /// <summary>
        /// The event log for the class.
        /// </summary>
        public LOGGER EVENTS = new LOGGER("Table Processor Log");
        private bool TRAILING_DELIM_FLAG = false;
        //Below declarations were originally locally held but I broke off INSTRUCTION_ROUTER into its own function. Its too much of
        //a pain to try to keep it local.
        private bool INV_FLAG = false; //Retained across calls of INSTRUCTION_ROUTER.
        private int DMC_NUMBER = 0; //Retained across calls of INSTRUCTION_ROUTER.
        private int DMR_NUMBER = 0; //Retained across calls of INSTRUCTION_ROUTER.

        //Constructor
        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public TABLE_PROCESSOR()
        {
            EVENTS.LOG_MESSAGE(1, "INITIALIZE");
        }

        //Methods
        /// <summary>
        /// This method transforms CSV file data by performing operations on it as defined by INSTRUCTION_TABLE.
        /// It outputs these transformations to DATA_TABLE and can optionally write to new files.
        /// See documentation for a list of valid instructions.
        /// </summary>
        /// <param name="DATA_TABLE">The table to transform.</param>
        /// <param name="INSTRUCTION_TABLE">A list of instructions. The table must be formatted properly. Use the INSTRUCTION_SET class to do this.</param>
        /// <param name="INPUT_PATH">The input path of the CSV file.</param>
        /// <param name="INPUT_DELIMITER">The delimeter used in the CSV file.</param>
        /// <param name="OUTPUT_PATH">The optional output file path. Be sure to include if your instructions contain a WRT command.</param>
        /// <param name="OUTPUT_DELIMITER">The delimeter used in the output file. Defaults to a comma.</param>
        /// <returns>0 on successful execution, 1 on failure.</returns>
        public int PROCESS_INSTRUCTIONS(ref DataTable DATA_TABLE, ref DataTable INSTRUCTION_TABLE, string INPUT_PATH, char INPUT_DELIMITER, string OUTPUT_PATH = null, char OUTPUT_DELIMITER = ',')
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            int RESULT = 0; //Stores the result of the instruction router.
            foreach (DataRow ROW in INSTRUCTION_TABLE.Rows) //For each row in the instruction table.
            {
                if (RESULT != 0) { break; } //Break out of the foreach if a previous instruction failed.
                string INSTRUCTION = ROW[0].ToString(); //Load the instruction in from the cell.

                //------------------------------------------------------------------------------------------------------------------
                //Extract the instruction code and any data on the tail end of the instruction. Capitalize the instruction code as
                //well because the INSTRUCTION_ROUTER processes only uppercase instructions.

                string TAIL = null; //holds any data on the tail end of the instruction code.
                int EXTRACT_SUCCESS = EXTRACT_INSTRUCTION_CODE(ref INSTRUCTION, ref TAIL); //Extract the code and break it into two.
                if (EXTRACT_SUCCESS != 0) //If the extraction failed...
                    break;
                else
                    ROW[0] = INSTRUCTION + TAIL; //Insert the uppercase version of the instruction back in the table.


                //------------------------------------------------------------------------------------------------------------------
                //Extract the number from the end of the instruction. 
                int NUMBER = EXTRACT_INSTRUCTION_NUM_ARG(TAIL);
                if (NUMBER == -2) //-2 is error code output from function.
                    break;

                EVENTS.LOG_MESSAGE(1, "");
                EVENTS.LOG_MESSAGE(1, string.Format("[{0}] -Instruction started.", INSTRUCTION));
                RESULT = INSTRUCTION_ROUTER(INSTRUCTION, ref DATA_TABLE,
                    INPUT_PATH, INPUT_DELIMITER,
                    OUTPUT_PATH, OUTPUT_DELIMITER,
                    ref TAIL, NUMBER); //Route the instruction.
                if (RESULT == 0)
                    EVENTS.LOG_MESSAGE(1, string.Format("[{0}] -Instruction finished.", INSTRUCTION));
                else
                {
                    EVENTS.LOG_MESSAGE(1, string.Format("[{0}] -Instruction failed execution. No additional instructions will be performed.", INSTRUCTION));
                    EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                    return 1; //Return failed execution status to the calling function.
                }
                EVENTS.LOG_MESSAGE(1, "");
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return successful execution status to the calling function.
        }
        private int INSTRUCTION_ROUTER(string INSTRUCTION, ref DataTable DATA_TABLE, string INPUT_PATH, char INPUT_DELIMITER, string OUTPUT_PATH, char OUTPUT_DELIMITER, ref string TAIL, int NUMBER = -1)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            int FUNCTION_RESULT = 0;
            if (INV_FLAG) //Process if the instruction arguments should be inverted or not due to a previous [INV] call.
            {
                if (INSTRUCTION == "RCO" || INSTRUCTION == "DMC" || INSTRUCTION == "EMC") //If the current instruction is column related...
                    NUMBER = DATA_TABLE.Columns.Count - NUMBER - 1;
                else //If its not column related, treat it as if its column related.
                    NUMBER = DATA_TABLE.Rows.Count - NUMBER - 1;
                INV_FLAG = false; //Turn off the invert flag so the next instruction is processed normally.
            }
            switch (INSTRUCTION) //Process what the instruction means.
            {
                case ("SCD"):
                    FUNCTION_RESULT = SUBSTITUTE_CELL_DATA(ref DATA_TABLE, TAIL);
                    break;
                case ("INV"):
                    INV_FLAG = true;
                    break;
                case ("TDE"):
                    TRAILING_DELIM_FLAG = true;
                    break;
                case ("TDD"):
                    TRAILING_DELIM_FLAG = false;
                    break;
                case ("CSV"): //Process CSV file into DataTable.
                    FUNCTION_RESULT = PROCESS_CSV_FILE(ref DATA_TABLE, INPUT_PATH, INPUT_DELIMITER);
                    break;
                case ("WRT"): //Write the DataTable to output csv file.
                    FUNCTION_RESULT = WRITE_CSV_FILE(ref DATA_TABLE, OUTPUT_PATH, OUTPUT_DELIMITER);
                    break;
                case ("TRP"): //Transpose the DataTable.
                    FUNCTION_RESULT = TRANSPOSE(ref DATA_TABLE);
                    break;
                case ("RVC"): //Reverse the column order.
                    FUNCTION_RESULT = REVERSE_COLUMNS(ref DATA_TABLE);
                    break;
                case ("RVR"): //Reverse the row order.
                    FUNCTION_RESULT = REVERSE_ROWS(ref DATA_TABLE);
                    break;
                case ("REC"): //Remove all empty columns from DataTable.
                    FUNCTION_RESULT = REMOVE_EMPTY_COLUMNS(ref DATA_TABLE);
                    break;
                case ("RER"): //Remove all empty rows from DataTable.
                    FUNCTION_RESULT = REMOVE_EMPTY_ROWS(ref DATA_TABLE);
                    break;
                case ("RCO"): //Remove column at a specified position.
                    if (NUMBER != -1) //If the passed number is anything but the default -1 value...
                        FUNCTION_RESULT = REMOVE_COLUMN_AT_ORDINAL(ref DATA_TABLE, NUMBER);
                    else
                    {
                        EVENTS.LOG_MESSAGE(3, "RCO command requires number, which was undetected.");
                        FUNCTION_RESULT = 2;
                    }
                    break;
                case ("RRI"): //Remove row at specified position.
                    if (NUMBER != -1) //If the passed number is anything but the default -1 value...
                        FUNCTION_RESULT = REMOVE_ROW_AT_INDEX(ref DATA_TABLE, NUMBER);
                    else
                    {
                        EVENTS.LOG_MESSAGE(3, "RRI command requires number, which was undetected.");
                        FUNCTION_RESULT = 2;
                    }
                    break;
                case ("EMR"): //Execute move row.
                    if (NUMBER != -1) //If the passed number is anything but the default -1 value...
                        FUNCTION_RESULT = MOVE_ROW(ref DATA_TABLE, DMR_NUMBER, NUMBER);
                    else
                    {
                        EVENTS.LOG_MESSAGE(3, "EMR command requires number, which was undetected.");
                        FUNCTION_RESULT = 2;
                    }
                    break;
                case ("DMC"): //Define which column will be moved.
                    if (NUMBER != -1) //If the passed number is anything but the default -1 value...
                        DMC_NUMBER = NUMBER; //Set the number.
                    else
                    {
                        EVENTS.LOG_MESSAGE(3, "DMC command requires number, which was undetected.");
                        FUNCTION_RESULT = 2;
                    }
                    break;
                case ("DMR"): //Define which row will be moved.
                    if (NUMBER != -1) //If the passed number is anything but the default -1 value...
                        DMR_NUMBER = NUMBER; //Set the number.
                    else
                    {
                        EVENTS.LOG_MESSAGE(3, "DMR command requires number, which was undetected.");
                        FUNCTION_RESULT = 2;
                    }
                    break;
                case ("EMC"):
                    if (NUMBER != -1) //If the passed number is anything but the default -1 value...
                        FUNCTION_RESULT = MOVE_COLUMN(ref DATA_TABLE, DMC_NUMBER, NUMBER);
                    else
                    {
                        EVENTS.LOG_MESSAGE(3, "EMC command requires number, which was undetected.");
                        FUNCTION_RESULT = 2;
                    }
                    break;
            }
            EVENTS.LOG_MESSAGE(3, string.Format("Exit code: {0}", FUNCTION_RESULT.ToString()));
            if (FUNCTION_RESULT == 0)
                EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            else
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
            return FUNCTION_RESULT;
        }
        private int EXTRACT_INSTRUCTION_CODE(ref string INSTRUCTION, ref string TAIL_DATA)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            string INSTRUCTION_LEFT = null;  //Stores the left 3 characters of each instruction.
            string INSTRUCTION_RIGHT = null; //Stores the characters not allocated to INSTRUCTION_LEFT.
            try
            {
                INSTRUCTION_LEFT = INSTRUCTION.Substring(0, 3).ToUpper(); //Get the left three chars and capitalize them.
                if (INSTRUCTION.Length > 3) //If there are more than three characters...
                    INSTRUCTION_RIGHT = INSTRUCTION.Substring(3, INSTRUCTION.Length - 3); //Put them to in the right side.
                INSTRUCTION = INSTRUCTION_LEFT;
                TAIL_DATA = INSTRUCTION_RIGHT;
                EVENTS.LOG_MESSAGE(3, string.Format("Extracted instruction [{0}].", INSTRUCTION));
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int EXTRACT_INSTRUCTION_NUM_ARG(string INSTRUCTION)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            int NUMBER; //Will hold the number extracted from the end of the instruction.
            bool NUM_PRESENT; //True if there is a number in the instruction.
            if (INSTRUCTION == "" || INSTRUCTION == null)
            {
                EVENTS.LOG_MESSAGE(3, "No number detected.");
                EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
                return -1;
            }
            try
            {
                EVENTS.LOG_MESSAGE(3, "Checking instruction for number...");
                NUM_PRESENT = int.TryParse(INSTRUCTION.Substring(0, INSTRUCTION.Length), out NUMBER); //Extract the number from the end of the instruction (if present).
                if (NUM_PRESENT)
                    EVENTS.LOG_MESSAGE(3, "Number extracted.");
                else
                {
                    NUMBER = -1;
                    EVENTS.LOG_MESSAGE(3, "No number detected [alt thread path].");
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return -2;
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return NUMBER;
        }
        private int SUBSTITUTE_CELL_DATA(ref DataTable SOURCE_TABLE, string DATA)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");

            string[] SPLIT_DATA_FIXED = new string[5]; //This will hold the coordinates and new data.
                                                       //------------------------------------------------------------------------------------------------------------------
                                                       //Try to split the message up by commas and then re-add in the commas. If the cell data has commas in it, it could
                                                       //split into multiple fields so we have to start with a unbounded array and then re-cram all the data back into
                                                       //index 4 with commas added back in.
            try
            {
                string[] SPLIT_DATA_VAR = DATA.Split(','); //Split up the message by comma.
                if (SPLIT_DATA_VAR.Length > 5)
                {
                    for (int i = 4; i < SPLIT_DATA_VAR.Length; i++)
                    {
                        if (i == 4) //Dont include the comma on the first iteration.
                            SPLIT_DATA_FIXED[4] += SPLIT_DATA_VAR[i];
                        else //Reinsert commas.
                            SPLIT_DATA_FIXED[4] += "," + SPLIT_DATA_VAR[i];
                    }
                }
                else
                    SPLIT_DATA_FIXED = SPLIT_DATA_VAR;
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "Failed to split data.");
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }

            //------------------------------------------------------------------------------------------------------------------
            //Try to process coordinates and invert flags. First attempt to convert coordinates to int. If it failed throw
            //exceptions out. If that goes well then try to process flags. If that fails throw exeptions out. If that goes
            //well then write the data to the field.
            try
            {
                bool GOOD_ROW, GOOD_COLUMN;
                int ROW, COLUMN;
                //Try to get int coordinates.
                GOOD_ROW = int.TryParse(SPLIT_DATA_FIXED[1], out ROW);
                GOOD_COLUMN = int.TryParse(SPLIT_DATA_FIXED[0], out COLUMN);
                //If the conversions failed, exception out.
                if (!GOOD_ROW)
                    throw new Exception("Could not parse row data.");
                if (!GOOD_COLUMN)
                    throw new Exception("Could not parse column data.");
                //Try to interpret the inversion flags.
                switch (SPLIT_DATA_FIXED[3]) //Row flag
                {
                    case ("1"):
                        ROW = SOURCE_TABLE.Rows.Count - ROW - 1;
                        break;
                    case ("0"):
                        //ROW remains untouched.
                        break;
                    default:
                        throw new Exception("Failed to interpret row flag.");
                }
                switch (SPLIT_DATA_FIXED[2]) //Column flag
                {
                    case ("1"):
                        COLUMN = SOURCE_TABLE.Columns.Count - COLUMN - 1;
                        break;
                    case ("0"):
                        //COLUMN remains untouched.
                        break;
                    default:
                        throw new Exception("Failed to interpret column flag.");
                }
                //Write data to field.
                SOURCE_TABLE.Rows[ROW][COLUMN] = SPLIT_DATA_FIXED[4];
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 2; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int WRITE_CSV_FILE(ref DataTable DATA, string PATH, char DELIMITER)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                using (StreamWriter WRITER = File.CreateText(PATH))
                {
                    foreach (DataRow ROW in DATA.Rows) //Go through each row in the table.
                    {
                        string OUTPUT_ROW = null;
                        foreach (var ELEMENT in ROW.ItemArray) //Go through each element in the row.
                        {
                            OUTPUT_ROW += ELEMENT.ToString() + DELIMITER;
                        }
                        if (!TRAILING_DELIM_FLAG)
                        {
                            OUTPUT_ROW = OUTPUT_ROW.Substring(0, OUTPUT_ROW.Length - 1);//Remove the trailing delimeter character.
                        }
                        WRITER.WriteLine(OUTPUT_ROW); //Write the line to the output file.
                        EVENTS.LOG_MESSAGE(3, "Writing row...");
                    }
                    WRITER.Close(); //Close up shop.
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int MOVE_COLUMN(ref DataTable SOURCE_TABLE, int DMC_NUM, int EMC_NUM)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                SOURCE_TABLE.Columns[DMC_NUM].SetOrdinal(EMC_NUM);
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int MOVE_ROW(ref DataTable SOURCE_TABLE, int DMR_NUM, int EMR_NUM)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                DataRow MOVE_ROW = SOURCE_TABLE.Rows[DMR_NUM]; //Define the row to be moved.
                EVENTS.LOG_MESSAGE(3, string.Format("Row moved from: {0}", DMR_NUM.ToString()));
                DataRow NEW_ROW = SOURCE_TABLE.NewRow(); //Create an empty new row.
                NEW_ROW.ItemArray = MOVE_ROW.ItemArray; //Move all the data from the current row into the empty one.
                SOURCE_TABLE.Rows.Remove(MOVE_ROW); //Remove the current row (the previously empty row is holding all the data).
                SOURCE_TABLE.Rows.InsertAt(NEW_ROW, EMR_NUM); //Insert the new row.
                EVENTS.LOG_MESSAGE(3, string.Format("Row inserted at: {0}", EMR_NUM.ToString()));
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int REVERSE_ROWS(ref DataTable SOURCE_TABLE)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            DataTable OUTPUT_TABLE = SOURCE_TABLE.Clone(); //Clone the schema from source table. This does not copy data.
            try
            {
                for (int i = SOURCE_TABLE.Rows.Count - 1; i >= 0; i--) //Go through each row in the table in reverse order...
                {
                    OUTPUT_TABLE.ImportRow(SOURCE_TABLE.Rows[i]); //Copy the data from the source table into the output table.
                    EVENTS.LOG_MESSAGE(3, string.Format("Row shifted at index: {0}", i.ToString()));
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            SOURCE_TABLE = OUTPUT_TABLE;
            return 0; //Return indicating successful execution.
        }
        private int REVERSE_COLUMNS(ref DataTable SOURCE_TABLE)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                if (SOURCE_TABLE.Rows.Count > 0)
                {
                    DataTable REVERSED_TABLE = new DataTable();
                    REVERSED_TABLE.Columns.AddRange(SOURCE_TABLE.Columns.Cast<DataColumn>().Reverse().Select(x => new DataColumn(x.ColumnName)).ToArray());
                    for (int j = 0; j < SOURCE_TABLE.Rows.Cast<DataRow>().Select(x => x.ItemArray.Reverse()).Count(); j++)
                    {
                        REVERSED_TABLE.Rows.Add();
                        var ROW = SOURCE_TABLE.Rows.Cast<DataRow>().Select(x => x.ItemArray.Reverse()).ElementAt(j);
                        for (int k = 0; k < ROW.Count(); k++)
                        {
                            REVERSED_TABLE.Rows[j][k] = ROW.ElementAt(k);
                        }
                    }
                    SOURCE_TABLE = REVERSED_TABLE;
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int TRANSPOSE(ref DataTable SOURCE_TABLE)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                DataTable NEW_TABLE = new DataTable(); //Create a empty DataTable. Populate the NEW_TABLE with enough columns.
                EVENTS.LOG_MESSAGE(3, "Adding columns...");
                for (int i = 1; i <= SOURCE_TABLE.Rows.Count; i++) //For each row in the SOURCE_TABLE...
                {
                    NEW_TABLE.Columns.Add(i.ToString()); //Add a one column for each row in the SOURCE_TABLE. 
                }
                //Populate the NEW_TABLE with data from the SOURCE_TABLE.
                EVENTS.LOG_MESSAGE(3, "Adding and populating rows...");
                for (int k = 0; k <= SOURCE_TABLE.Columns.Count - 1; k++) //For each column in the SOURCE_TABLE...
                {
                    DataRow NEW_ROW = NEW_TABLE.NewRow(); //Create a row in the NEW_TABLE for each column in the SOURCE_TABLE.
                    for (int j = 0; j < SOURCE_TABLE.Rows.Count; j++)
                        NEW_ROW[j] = SOURCE_TABLE.Rows[j][k]; //Add the data from the source column to the new row.
                    NEW_TABLE.Rows.Add(NEW_ROW);
                }
                SOURCE_TABLE = NEW_TABLE;
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int REMOVE_COLUMN_AT_ORDINAL(ref DataTable SOURCE_TABLE, int COLUMN)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                SOURCE_TABLE.Columns.RemoveAt(COLUMN); //Remove the specified column.
                EVENTS.LOG_MESSAGE(3, string.Format("Removing column at ordinal: {0}", COLUMN.ToString()));
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int REMOVE_ROW_AT_INDEX(ref DataTable SOURCE_TABLE, int ROW)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            try
            {
                SOURCE_TABLE.Rows.RemoveAt(ROW); //Remove the specified row.
                EVENTS.LOG_MESSAGE(3, string.Format("Removing column at ordinal: {0}", ROW.ToString()));
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int REMOVE_EMPTY_ROWS(ref DataTable SOURCE_TABLE)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            List<int> ROW_ID = new List<int>(); //List of rows that are empty.
            try
            {
                foreach (DataRow ROW in SOURCE_TABLE.Rows) //For each row in the DataTable...
                {
                    bool NOT_EMPTY = false; //If NOT_EMPTY goes high, it means the row has data in it.
                    EVENTS.LOG_MESSAGE(3, string.Format("Evaluating row index: {0}", SOURCE_TABLE.Rows.IndexOf(ROW).ToString()));
                    foreach (var VALUE in ROW.ItemArray) //For each value in the row...
                    {
                        if (VALUE.ToString() != "") //If the value is not empty...
                        {
                            NOT_EMPTY = true; //Flag\ the row as not empty.
                            break; //Break out of the loop for the current row.
                        }
                    }
                    if (!NOT_EMPTY) //If the row IS empty...
                    {
                        ROW_ID.Add(SOURCE_TABLE.Rows.IndexOf(ROW)); //Add the row ID to the list of empty rows.
                        EVENTS.LOG_MESSAGE(3, string.Format("Empty row detected at index: {0}", SOURCE_TABLE.Rows.IndexOf(ROW).ToString()));
                    }
                }
                //Once each row has been checked.
                for (int i = ROW_ID.Count - 1; i >= 0; i--) //Recurse thru the row list in reverse order...
                {
                    SOURCE_TABLE.Rows.RemoveAt(ROW_ID[i]); //Remove the column.
                    EVENTS.LOG_MESSAGE(3, string.Format("Removing row index: {0}", ROW_ID[i].ToString()));
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int REMOVE_EMPTY_COLUMNS(ref DataTable SOURCE_TABLE)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            List<int> COLUMN_ID = new List<int>(); //List of columns that are empty.
            try
            {
                foreach (DataColumn COLUMN in SOURCE_TABLE.Columns) //For each column in the table...
                {
                    bool NOT_EMPTY = false; //If NOT_EMPTY goes high, it means the column has data in it.
                    EVENTS.LOG_MESSAGE(3, string.Format("Evaluating column ordinal: {0}", COLUMN.Ordinal.ToString()));
                    foreach (DataRow ROW in SOURCE_TABLE.Rows) //For each row in the column...
                    {
                        if (ROW[COLUMN].ToString() != "") //If the cell has data in it...
                        {
                            NOT_EMPTY = true; //Flag the column as not empty.
                            break; //Break out of the loop for the current column.
                        }
                    }
                    if (!NOT_EMPTY) //If the column IS empty...
                    {
                        COLUMN_ID.Add(COLUMN.Ordinal); //Add the column ID to the list of empty columns.
                        EVENTS.LOG_MESSAGE(3, string.Format("Empty column detected at ordinal: {0}", COLUMN.Ordinal.ToString()));
                    }
                }
                //Once each column has been checked.
                for (int i = COLUMN_ID.Count - 1; i >= 0; i--) //Recurse thru the column list in reverse order...
                {
                    SOURCE_TABLE.Columns.RemoveAt(COLUMN_ID[i]); //Remove the column.
                    EVENTS.LOG_MESSAGE(3, string.Format("Removing column ordinal: {0}", COLUMN_ID[i].ToString()));
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0; //Return indicating successful execution.
        }
        private int PROCESS_CSV_FILE(ref DataTable SOURCE_TABLE, string PATH, char DELIMITER)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            DataTable LINE_DATA = new DataTable(); //Create a temporary table for raw line data.
            DataTable FULL_DATA = new DataTable(); //Table that will become the final table.
            bool ABORT = false; //If this bit goes high, something went wrong.
            try
            {
                LINE_DATA = READ_CSV_LINES(PATH); //Attempt to read in line data. This function creates rows but does not do the columns.
            }
            catch (Exception EX)
            {
                ABORT = true;
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
            }
            try
            {
                if (!ABORT) //If the previous section worked alright...
                    FULL_DATA = SPLIT_LINES(LINE_DATA, ','); //split the data into columns. Now there should be a fully populated data table.
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //Return indicating error.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            SOURCE_TABLE = FULL_DATA;
            return 0; //Return indicating successful execution.
        }
        private DataTable READ_CSV_LINES(string PATH)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            DataTable DATA = new DataTable(); //Create DataTable.
            DATA.Columns.Add("RAW_LINES"); //Create a column to put the data in.
            try //Do the following in a "try" as exceptions can be thrown by the StreamReader.
            {
                using (var READER = new StreamReader(PATH)) //Create a StreamReader for the file.
                {
                    EVENTS.LOG_MESSAGE(3, string.Format("Reading file: {0}", PATH));
                    while (!READER.EndOfStream) //While there is data to read...
                    {
                        string DATA_STRING = READER.ReadLine();
                        DATA.Rows.Add(DATA_STRING); //Add each line to the DataTable.
                        EVENTS.LOG_MESSAGE(3, string.Format("Reading line: {0}", DATA_STRING));
                    }
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(2, "Throwing exception up to calling function.");
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                throw EX; //If there was an exception throw it up to the calling function.
            }
            if (DATA.Rows.Count == 0) { throw new Exception("No data was read in."); }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return DATA; //Return the table up to the calling function.
        }
        private DataTable SPLIT_LINES(DataTable SOURCE_TABLE, Char DELIMITER)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            DataTable NEW_DATA = new DataTable(); //Create DataTable.
            try
            {
                foreach (DataRow ROW in SOURCE_TABLE.Rows) //For each element in the source table...
                {
                    var VALUES = ROW[0].ToString().Split(DELIMITER); //Split the data based on the delimiter.
                    int ARRAY_SIZE = VALUES.Length; //Determine how many elements the row split into.
                    while (NEW_DATA.Columns.Count < ARRAY_SIZE) //If there are more elements than there are column...
                    {
                        EVENTS.LOG_MESSAGE(3, string.Format("New column added at position: {0}", NEW_DATA.Columns.Count.ToString()));
                        NEW_DATA.Columns.Add(); //Add columns.
                    }
                    NEW_DATA.Rows.Add(VALUES); //Once there are enough columns, add the data.
                }
            }
            catch (Exception EX)
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", EX.Message);
                EVENTS.LOG_MESSAGE(2, "Throwing exception up to calling function.");
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                throw EX; //If there was an exception throw it up to the calling function.
            }
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return NEW_DATA; //Return the data table.
        }
    }
}
