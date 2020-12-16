using System;
using FORBES.ASCII_RENDER_NAMESPACE;
using FORBES.PROCESS_TIME_NAMESPACE;
using System.IO;
using System.Drawing;
namespace ASCII_RENDER_TEST_APPLICATION
{
    class Program
    {
        static void Main(string[] args)
        {
            PROCESS_TIME TOTAL_RUN_TIME = new PROCESS_TIME(true);
            Console.WriteLine("ASCII Render Test Application."); //Welcome Message.

            int VALIDITY_RESULT = CHECK_FILE_VALIDITY(ref args); //Check to make sure there is a valid filepath...
            if (VALIDITY_RESULT != 0) //Zero represents a valid file, if the result is not zero...
                EXIT(VALIDITY_RESULT); //Exit with error code from the function.
            string FILEPATH = args[0]; //At this point we know there is a valid path at this location, so its safe to assign.

            Image IMAGE = Image.FromFile(FILEPATH); //Get the image.
            Image[] FRAMES = EXTRACT_FRAMES(IMAGE); //Extract frames from image.
            Console.Clear(); //Clear the console before going into the loop.
            for (int i = 0; i < 30; i++) //Loop through the gif this many times.
            {
                foreach (Image FRAME in FRAMES) //For each frame in the gif.
                {
                    PRINT_FRAME(FRAME); //Print the frame.
                    GC.Collect();
                    System.Threading.Thread.Sleep(70); //Add a bit of a delay.
                    Console.Clear();
                }
            }
            TOTAL_RUN_TIME.STOP();
            Console.WriteLine("Process Time: {0} ms.", TOTAL_RUN_TIME.TIME_ELAPSED.TotalMilliseconds); //Print the total running time.
            EXIT(); //Go to shutdown routine.
        }
        private static Image[] EXTRACT_FRAMES(Image IMAGE)
        {
            int COUNT = IMAGE.GetFrameCount(System.Drawing.Imaging.FrameDimension.Time);
            Image[] FRAMES = new Image[COUNT];
            for (int i = 0; i < COUNT; i++)
            {
                IMAGE.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Time, i);
                FRAMES[i] = ((Image)IMAGE.Clone());
            }
            return FRAMES;
        }

        private static void PRINT_FRAME(Image FRAME)
        {
            byte[,] IMAGE_ILLUM_ARRAY = new byte[0, 0]; //Create a array that will hold all the illumination values for each pixel.
            IMAGE_ILLUM_ARRAY = ASCII_RENDER.CONVERT_IMAGE(FRAME, 30, 30); //Convert the image into a byte array of illumination values.

            char[,] CHAR_ARRAY = new char[0, 0]; //Define the array which will hold the terminal characters.
            CHAR_ARRAY = ASCII_RENDER.CONVERT_TO_ASCII(IMAGE_ILLUM_ARRAY); //Convert the byte array into a array of terminal characters.
            int WIDTH = CHAR_ARRAY.GetLength(0); //Get the width of the array.
            int HEIGHT = CHAR_ARRAY.GetLength(1); //Get the height of the array.
            //Scan through the array left to right, then downward.
            for (int Y = 0; Y < HEIGHT; Y++) //Loop through each row.
            {
                string ROW_TEXT = ""; //At the beginning of each row, reinitialize the string.
                for (int X = 0; X < WIDTH; X++) //Loop through each column in a row.
                {
                    ROW_TEXT += CHAR_ARRAY[X, Y]; //Add the character to the string.
                }
                Console.WriteLine(ROW_TEXT); //Print the row string.
            }
        }

        /// <summary>
        /// This method exits the application.
        /// </summary>
        /// <param name="EXIT_CODE"></param>
        private static void EXIT(int EXIT_CODE = 0)
        {
            Console.WriteLine("Press <ENTER> to exit...");
            Console.ReadLine();
            Environment.Exit(EXIT_CODE);
        }

        /// <summary>
        /// This fuction checks if the passed file is valid.
        /// </summary>
        /// <param name="args">startup parameters.</param>
        /// <returns>
        /// <para>0 = Valid.</para>
        /// <para>1 = No parameter passed to application.</para>
        /// <para>2 = File does not exist.</para></returns>
        private static int CHECK_FILE_VALIDITY(ref string[] args)
        {
            if (args.Length == 0) //If there was no argument passed to the application...
            {
                Console.WriteLine("No file passed to application.");
                return 1;
            }
            string FILEPATH = ""; //Create a string that will hold the filepath.
            FILEPATH = args[0]; //If the thread gets to here, there was a argument passed. Assign it to filepath.
            Console.WriteLine("Filepath: {0}", FILEPATH); //Echo the filepath.
            bool FILE_EXISTS = File.Exists(FILEPATH); //Check if the file exists.
            if (!FILE_EXISTS) //If the file does not exist.
            {
                Console.WriteLine("File does not exist.");
                return 2;
            }
            return 0; //File is valid.
        }
    }
}
