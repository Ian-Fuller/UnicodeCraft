using System;
using System.Collections.Generic;
using System.Text;

namespace UnicodeCraft
{
    public class GlobalTimer
    {
        //Time logic varibles
        public bool isDay = true; //Used to tell Grid whether it is night or day
        public const int DAY_LENGTH = 240; //How many actions the day-night cycle lasts //240 is current default
        public int dayNightTimer = 0; //Counts up to DAY_LENGTH. At halfway, it will switch to night

        //Sky rendering variables
        public static char sun = CharLibrary.solidCircle;
        public static char moon = 'c';
        public static char cloud = CharLibrary.wavyEquals;
        public static char star = '.';
        public string[] sky = new string[] { "", "", "" }; //Three layers of the sky
        public double skyMovementInterval; //Tells sky how frequently to move

        public GlobalTimer()
        {
            skyMovementInterval = (double)(Grid.GRID_WIDTH * 2) / (double)DAY_LENGTH; //Calculates how long each movement of the sky is based on how long the day is and how long the sky is (sky length is based on map width)
                                                                                      //small map + long day = short and infrequent movements, large map + short day = long and frequent movements
            //Generates day half of sky
            for (int row = 0; row < sky.Length; row++) //Fills night with spaces
            {
                for (int i = 0; i < Grid.GRID_WIDTH - 1; i++)
                {
                    sky[row] += " ";
                }
            }
            sky[0] += " ";
            sky[1] += sun; //Adds moon at the end of the string
            sky[2] += " ";

            //Generates night half of sky
            for (int row = 0; row < sky.Length; row++) //Fills day with spaces
            {
                for (int i = 0; i < Grid.GRID_WIDTH - 1; i++)
                {
                    sky[row] += " ";
                }
            }
            sky[0] += " ";
            sky[1] += moon; //Adds sun at the end of the string
            sky[2] += " ";

            //Generates clouds and stars
            Random rand = new Random();
            for (int row = 0; row < sky.Length; row++)
            {
                char[] tempCharArray = sky[row].ToCharArray();
                for(int i = 0; i < tempCharArray.Length; i++)
                {
                    if(tempCharArray[i] == ' ' && i > (float)tempCharArray.Length * 0.25f && i < (float)tempCharArray.Length * 0.75f && rand.Next(0, 100) < 10)
                    {
                        tempCharArray[i] = cloud;
                    }
                    else if (tempCharArray[i] == ' ' && (i < (float)tempCharArray.Length * 0.25f || i > (float)tempCharArray.Length * 0.75f) && rand.Next(0, 100) < 10)
                    {
                        tempCharArray[i] = star;
                    }
                }
                sky[row] = "";
                for(int i = 0; i < tempCharArray.Length; i++)
                {
                    sky[row] += tempCharArray[i];
                }
            }
        }

        public bool Tick()
        {
            dayNightTimer++; //Moves time forward
            if(dayNightTimer == DAY_LENGTH / 2) //At halfway through the day cycle, the time will switch to night
            {
                isDay = !isDay;
            }
            if(dayNightTimer >= DAY_LENGTH) //At the end of the cycle, the timer will be reset and a new day will start
            {
                isDay = !isDay;
                dayNightTimer = 0;
            }
            return isDay; //Returns value to DisplayGrid so it knows whether or not to light everything by default
        }

        public void DisplaySky()
        {                                        
            for (int row = 0; row < sky.Length; row++) //sky.Length refers to the size of the array (3)
            {
                UI.Vertical();
                int skyStart = (int)(dayNightTimer * skyMovementInterval); //calculates where in the string the output needs to start
                for (int i = 0; i < Grid.GRID_WIDTH; i++) //Always repeats based on the grid's width
                {
                    //Wraps back around to the beginning of the string if the end has been reached
                    if (skyStart >= sky[row].Length)
                    {
                        skyStart = 0;
                    }

                    //Coloring
                    if (skyStart > (float)sky[row].Length * 0.25f && skyStart < (float)sky[row].Length * 0.75f)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (skyStart < (float)sky[row].Length * 0.25f || skyStart > (float)sky[row].Length * 0.75f)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (sky[row][skyStart] == sun)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (sky[row][skyStart] == moon)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    //Output and incrementation
                    Console.Write(sky[row][skyStart]);
                    skyStart++;
                }
                //Resets colors and creates border
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(CharLibrary.vertical + "                    ");
                Console.Write(CharLibrary.vertical + "                    ");
                UI.Vertical();
                Console.Write("\n");
            }
        }
    }
}
