using System;
using System.Collections.Generic;

//Things to add:
//-Concealment from objects
//-Whether or not an object can be moved through
//-Make tree leaves render in the right place when adjacent
//-Add to items: FG ang BG color, separate grid and inventory sprites, descriptions

//-Fix Shift+S at the beginning of program bug
//-Fix coordinates in grid being displayed in 10s place when outside of 0, 0 grid

//-Add more comments
//-Precalculate more variables

namespace UnicodeCraft
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize variables
            Player player = new Player(); //Creates player object

            List<Grid> gridList = new List<Grid>(); //Will hold all grids
            int currentGrid = 0; //Current grid to be acted on
            int currentX = 0; //Used to find the proper grids
            int currentY = 0; //--

            GlobalTimer timer = new GlobalTimer(); //Keeps track of time across all grids

            //Setup
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.SetWindowSize(44 + Grid.GRID_WIDTH, 9 + Grid.GRID_HEIGHT);

            while (true)
            {
                //Display section
                Console.SetCursorPosition(0, 0); //Refreshes screen
                UI.TopBorder(); //Prints top border above the sky to make the program look better
                timer.DisplaySky(); //Displays the sky
                UI.MiddleBorder(); //Prints border between sky and grid
                for (int i = 0; i < gridList.Count + 1; i++) //Searches through the list and displays the one on the current X and Y value. If not found, creates a new grid
                {
                    currentGrid = i; //For use of i outside of loop
                    if (i == gridList.Count) //If the loop iterates through the whole list, the proper grid has not been found, so it creates a new one
                    {
                        Grid gridTemp = new Grid(player, currentX, currentY); //creates new temporary grid only used in this scope; gets deleted afer exiting of course
                        gridList.Add(gridTemp); //Adds the new grid to the list
                        gridList[currentGrid].DisplayGrid(player, timer); //Displays it
                        break; //Prevents infinite loop
                    }
                    else if (gridList[currentGrid].worldX == currentX && gridList[currentGrid].worldY == currentY) //If the proper grid has been found, displays it
                    {
                        player.SetStart(); //Since GenerateNewGrid is not being called, this needs to be called manually
                        gridList[currentGrid].DisplayGrid(player, timer); //Displays grid, passes in player so it knows what the player is
                        break; //So the loop doesn't continue iterating
                    }
                }

                char inputKey = Console.ReadKey().KeyChar; //Waits for input from the player
                player.ControlPlayer(inputKey, gridList[currentGrid]); //Passes input to the player's contorls
                if (inputKey == '~') //Used for stopping the program
                {
                    break;
                }

                if (gridList[currentGrid].BorderCheck(player) != "") //Skips if the player is not on a border. If they are, the currentX and currentY will be changed based on which border they are on
                {
                    if (gridList[currentGrid].BorderCheck(player) == "Left")
                    {
                        currentX--;
                    }
                    if (gridList[currentGrid].BorderCheck(player) == "Right")
                    {
                        currentX++;
                    }
                    if (gridList[currentGrid].BorderCheck(player) == "Up")
                    {
                        currentY++;
                    }
                    if (gridList[currentGrid].BorderCheck(player) == "Down")
                    {
                        currentY--;
                    }
                }
                timer.Tick(); //Ensures that time passes
            }
        }
    }
}
