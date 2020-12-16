using System;
using System.Drawing;

namespace FORBES.ASCII_RENDER_NAMESPACE
{
    /// <summary>
    /// This class can convert a image to ASCII art.
    /// </summary>
    public static class ASCII_RENDER
    {
        /// <summary>
        /// This function will convert a image to a luminescence byte array.
        /// </summary>
        /// <param name="IMAGE">The image to convert.</param>
        /// <param name="X_SIZE">If you want to scale the image, send the output width. Note: Both X and Y values must be sent for scaling to process.</param>
        /// <param name="Y_SIZE">If you want to scale the image, send the output height. Note: Both X and Y values must be sent for scaling to process.</param>
        /// <returns>A byte array of luminescence values.</returns>
        public static byte [,] CONVERT_IMAGE(Image IMAGE, int X_SIZE = 0, int Y_SIZE = 0)
        {
            Bitmap INPUT_IMAGE = new Bitmap(IMAGE); //Convert the image to a bitmap.
            if(X_SIZE != 0 && Y_SIZE != 0) //If a new X and Y size is defined...
                INPUT_IMAGE = new Bitmap(INPUT_IMAGE, new Size(X_SIZE, Y_SIZE)); //Scale the bitmap.
            int WIDTH = INPUT_IMAGE.Width; //Define the width.
            int HEIGHT = INPUT_IMAGE.Height; //Define the height.
            byte[,] PIXEL_ARRAY = new byte[WIDTH, HEIGHT]; //Create the array that will be populated.

            //Process the array, go through each row, and process each value (across first, then down).
            for (int Y = 0; Y < HEIGHT; Y++) //Loop through each row.
            {
                for (int X = 0; X < WIDTH; X++) //Loop through each column in a row.
                {
                    Color PIXEL_COLOR = INPUT_IMAGE.GetPixel(X, Y); //Get the color of each pixel.
                    byte PIXEL_GRAYSCALE = (byte)((PIXEL_COLOR.R * .3) + 
                                                  (PIXEL_COLOR.G *.59) + 
                                                  (PIXEL_COLOR.B * .11)); //Convert to grayscale, I found these values do a pretty good job.
                    PIXEL_ARRAY[X, Y] = PIXEL_GRAYSCALE; //Save the value to the array.
                }
            }
            return PIXEL_ARRAY; //Return the array.
        }

        /// <summary>
        /// This function will convert a byte array of luminescence values to ASCII representations of those values. The function has 12 shades.
        /// </summary>
        /// <param name="BYTE_ARRAY">The byte array to convert.</param>
        /// <returns>A character array of an image ready to print.</returns>
        public static char[,] CONVERT_TO_ASCII(byte[,] BYTE_ARRAY)
        {
            int WIDTH = BYTE_ARRAY.GetLength(0); //Get the width of the image.
            int HEIGHT = BYTE_ARRAY.GetLength(1); //Get the height of the image.
            char[,] CHAR_ARRAY = new char[WIDTH, HEIGHT];
            //Process the array, go through each row, and process each value (across first, then down).
            for (int Y = 0; Y < HEIGHT; Y++) //Loop through each row.
            {
                for (int X = 0; X < WIDTH; X++) //Loop through each column in a row.
                {
                    int REDUCED_BYTE_VALUE = (int)Math.Round(BYTE_ARRAY[X, Y] / 23.18); //Scale the value from 0 to 255 to 0 to 11.
                    switch(REDUCED_BYTE_VALUE)
                    {
                        case (0):
                            CHAR_ARRAY[X, Y] = '.'; //Lightest grayscale value.
                            break;
                        case (1):
                            CHAR_ARRAY[X, Y] = ',';
                            break;
                        case (2):
                            CHAR_ARRAY[X, Y] = '-';
                            break;
                        case (3):
                            CHAR_ARRAY[X, Y] = '~';
                            break;
                        case (4):
                            CHAR_ARRAY[X, Y] = ':';
                            break;
                        case (5):
                            CHAR_ARRAY[X, Y] = ';';
                            break;
                        case (6):
                            CHAR_ARRAY[X, Y] = '=';
                            break;
                        case (7):
                            CHAR_ARRAY[X, Y] = '!';
                            break;
                        case (8):
                            CHAR_ARRAY[X, Y] = '*';
                            break;
                        case (9):
                            CHAR_ARRAY[X, Y] = '#';
                            break;
                        case (10):
                            CHAR_ARRAY[X, Y] = '$';
                            break;
                        case (11):
                            CHAR_ARRAY[X, Y] = '@'; //Darkest grayscale value.
                            break;
                        default:
                            CHAR_ARRAY[X, Y] = 'X'; //If the array has invalid values.
                            break;
                    }
                }
            }
            return CHAR_ARRAY;
        }
    }
}

