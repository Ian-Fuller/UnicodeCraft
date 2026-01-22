using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace UnicodeCraft
{
    public struct Node
    {
        public Item item; //Stores the item in the node

        //borders
        public bool isBorder;
        public bool isLeftBorder;
        public bool isRightBorder;
        public bool isUpBorder;
        public bool isDownBorder;

        //Lighting
        public bool isLit;
    }

    public class Grid
    {
        //Variables used in all grids. Some will be altered in the constructor
        public const int GRID_HEIGHT = 23; //Grid size contant //Default 23
        public const int GRID_WIDTH = 43; //--                 //Default 43
        public Node[,] topLayer = new Node[GRID_HEIGHT, GRID_WIDTH]; //Used for additional visual and mechanical effects
        public Node[,] gridCoordinate = new Node[GRID_HEIGHT, GRID_WIDTH]; //The grid itself

        public List<Node> actionNodes = new List<Node>();

        public int[,] adjacencyOffsets = new int[4, 2]
        {
            { 0, -1 }, //up
            { 1, 0 }, //right
            { 0, 1 }, //down
            { -1, 0 } //left
        };


        public int worldX; //Stores grid's position so it can be called upon in the main program
        public int worldY; //--

        public Grid(Player player, int currentX, int currentY)
        {
            worldX = currentX;
            worldY = currentY;

            Random rand = new Random(); //RNG

            player.SetStart(); //Calls function to set the player's starting position

            //creates border
            for (int row = 0; row < GRID_HEIGHT; row++)
            {
                for (int column = 0; column < GRID_WIDTH; column++)
                {
                    if (column == 0)
                    {
                        gridCoordinate[row, column].isLeftBorder = true;
                        gridCoordinate[row, column].isBorder = true;
                    }
                    else if (column == GRID_WIDTH - 1)
                    {
                        gridCoordinate[row, column].isRightBorder = true;
                        gridCoordinate[row, column].isBorder = true;
                    }
                    else if (row == 0)
                    {
                        gridCoordinate[row, column].isUpBorder = true;
                        gridCoordinate[row, column].isBorder = true;
                    }
                    else if (row == GRID_HEIGHT - 1)
                    {
                        gridCoordinate[row, column].isDownBorder = true;
                        gridCoordinate[row, column].isBorder = true;
                    }
                }
            }

            //generates features & terrain
            for (int row = 0; row < GRID_HEIGHT; row++)
            {
                for (int column = 0; column < GRID_WIDTH; column++)
                {
                    int r = rand.Next(0, 100); //100 possibilities; 0-99
                    gridCoordinate[row, column].item = new Item();
                    if ((r >= 0 && r < 25) && !gridCoordinate[row, column].isBorder && (row != player.row || column != player.column)) //places stone item as long as space is not border or player
                    {
                        gridCoordinate[row, column].item.GetCopyOf(ItemLibrary.STONE);
                        if (rand.Next(0, 100) < 10)
                        {
                            gridCoordinate[row, column].item.GetCopyOf(ItemLibrary.COAL_ORE);
                        }
                        if (rand.Next(0, 100) < 5)
                        {
                            gridCoordinate[row, column].item.GetCopyOf(ItemLibrary.METAL_ORE);
                        }
                    }
                    else if ((r >= 50 && r < 55) && !gridCoordinate[row, column].isBorder && (row != player.row || column != player.column)) //tree
                    {
                        gridCoordinate[row, column].item.GetCopyOf(ItemLibrary.WOODEN_LOG);
                    }
                    else if ((r >= 98 && r < 99) && !gridCoordinate[row, column].isBorder && (row != player.row || column != player.column))
                    {
                        gridCoordinate[row, column].item.GetCopyOf(ItemLibrary.FLINT);
                    }
                    else if ((r >= 99 && r < 100) && !gridCoordinate[row, column].isBorder && (row != player.row || column != player.column))
                    {
                        gridCoordinate[row, column].item.GetCopyOf(ItemLibrary.BUNNY_RABBIT);
                    }
                    else //places air
                    {
                        gridCoordinate[row, column].item.GetCopyOf(ItemLibrary.AIR);
                    }
                }
            }
        }

        public Item ItemAt(int[] coordinates)
        {
            try
            {
                return gridCoordinate[coordinates[0], coordinates[1]].item;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        //Checks is player is on the border so the game knows when to put the player in a different/new map
        public string BorderCheck(Player player)
        {
            if (!gridCoordinate[player.row, player.column].isBorder) //If the player isn't on any border, the if statements inside will be skipped to make the program faster
            {
                return "";
            }
            else
            {
                if (gridCoordinate[player.row, player.column].isLeftBorder)
                {
                    return "Left";
                }
                if (gridCoordinate[player.row, player.column].isRightBorder)
                {
                    return "Right";
                }
                if (gridCoordinate[player.row, player.column].isUpBorder)
                {
                    return "Up";
                }
                if (gridCoordinate[player.row, player.column].isDownBorder)
                {
                    return "Down";
                }
                else
                {
                    return "";
                }
            }
        }

        public void DisplayGrid(Player player, GlobalTimer timer)
        {
            //Changes lighting based on time of day and nearby light sources
            if(!timer.isDay)
            {
                //Makes everything dark before determining what to light up
                for (int row = 1; row < GRID_HEIGHT - 1; row++) //for every row
                {
                    for (int column = 1; column < GRID_WIDTH - 1; column++) //for every column
                    {
                        gridCoordinate[row, column].isLit = false; //unlight the coordinate
                    }
                }

                //Lights up the area around light sources (Currently the player and torches are the only two, but this will be expanded to whether an item gives off light or not)
                for (int row = 1; row < GRID_HEIGHT - 1; row++) //for every row
                {
                    for(int column = 1; column < GRID_WIDTH - 1; column++) //for every column
                    {
                        //Player lighting
                        if (player.row == row && player.column == column || player.row == row + 1 && player.column == column || player.row == row - 1 && player.column == column || player.row == row && player.column == column + 1 || player.row == row && player.column == column - 1)
                        {
                            gridCoordinate[row, column].isLit = true;
                        }
                        
                        if(gridCoordinate[row, column].item.lightLevel > 0)
                        {
                            List<int[]> toSearch = new List<int[]>();
                            toSearch.Add(new int[] { row, column });
                            List<int[]> toLight = new List<int[]>();
                            toLight.Add(new int[] { row, column });

                            for (int g = 0; g < gridCoordinate[row, column].item.lightLevel; g++) //g represents the distance the light will travel out from the source
                            {
                                int h2 = toSearch.Count; //gets the count of toLight variables to start off so the loop doesn't endlessley repeat
                                for (int h = 0; h < h2; h++) //h represents the current toLight that neads to be searched
                                {
                                    for (int i = 0; i < 4; i++) //i represents the current index of the light offset
                                    {
                                        if (!gridCoordinate[toSearch[h][0] + adjacencyOffsets[i, 0], toSearch[h][1] + adjacencyOffsets[i, 1]].isBorder)
                                        {
                                            if (gridCoordinate[toSearch[h][0] + adjacencyOffsets[i, 0], toSearch[h][1] + adjacencyOffsets[i, 1]].item.transparent)
                                            {
                                                toSearch.Add(new int[] { toSearch[h][0] + adjacencyOffsets[i, 0], toSearch[h][1] + adjacencyOffsets[i, 1] });
                                                toLight.Add(new int[] { toSearch[h][0] + adjacencyOffsets[i, 0], toSearch[h][1] + adjacencyOffsets[i, 1] });
                                            }
                                            else
                                            {
                                                toLight.Add(new int[] { toSearch[h][0] + adjacencyOffsets[i, 0], toSearch[h][1] + adjacencyOffsets[i, 1] });
                                            }
                                            
                                        }
                                    }
                                }
                            }

                            for (int i = 0; i < toLight.Count; i++)
                            {
                                gridCoordinate[toLight[i][0], toLight[i][1]].isLit = true;
                            }
                        }
                    }
                }
            }
            else if(timer.isDay) //if it is day, light up everything
            {
                for (int row = 1; row < GRID_HEIGHT - 1; row++) //for every row
                {
                    for (int column = 1; column < GRID_WIDTH - 1; column++) //for every column
                    {
                        gridCoordinate[row, column].isLit = true; //light up coordinate 
                    }
                }
            }

            //Generates topLayer
            for (int row = 1; row < GRID_HEIGHT - 1; row++)
            {
                for (int column = 1; column < GRID_WIDTH - 1; column++)
                {
                    if (gridCoordinate[row + 1, column].item.itemName == ItemLibrary.WOODEN_LOG.itemName)
                    {
                        topLayer[row, column - 1].item = new Item();
                        topLayer[row, column - 1].item.GetCopyOf(ItemLibrary.TREE_LEAVES);
                        topLayer[row, column].item = new Item();
                        topLayer[row, column].item.GetCopyOf(ItemLibrary.TREE_LEAVES);
                        topLayer[row, column + 1].item = new Item();
                        topLayer[row, column + 1].item.GetCopyOf(ItemLibrary.TREE_LEAVES);
                        column++;
                    }
                    else
                    {
                        topLayer[row, column].item = ItemLibrary.AIR;
                    }
                }
            }

            //Main display function
            for (int row = 0; row < GRID_HEIGHT; row++)
            {
                UI.Vertical();
                for (int column = 0; column < GRID_WIDTH; column++)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    //border
                    if (gridCoordinate[row, column].isBorder)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        if (gridCoordinate[row, column].isLeftBorder)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write(CharLibrary.leftArrow);
                            Console.BackgroundColor = ConsoleColor.Green;
                        }
                        else if (gridCoordinate[row, column].isRightBorder)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write(CharLibrary.rightArrow);
                        }
                        else if (gridCoordinate[row, column].isUpBorder)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write(CharLibrary.upArrow);
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write(CharLibrary.downArrow);
                        }
                    }
                    //Lighting
                    else if(!gridCoordinate[row, column].isLit)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    //Covers node with topLayer block if one exists
                    else if(topLayer[row, column].item != ItemLibrary.AIR)
                    {
                        topLayer[row, column].item.DisplayItem();
                    }
                    //Player
                    else if (player.row == row && player.column == column)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("i");
                    }
                    //Item
                    else
                    {
                        gridCoordinate[row, column].item.DisplayItem();
                    }
                }

                //Sets information for information tab
                string[] information = {
                    $"World: X: {worldX}, Y: {worldY}",
                    $"On Map: X: {player.column}, Y: {player.row}",
                    $"Health: {player.health}%",
                    $"Satiation: {player.satiation}%",
                    $"",
                    $"Current Item:",
                    $"{player.inventory[player.inventoryPosition].FormattedName()} x{player.inventory[player.inventoryPosition].itemQuantity}"
                };

                Console.BackgroundColor = ConsoleColor.Black;
                UI.Vertical();

                if (row < information.Length)
                {
                    int extraSpaces = 20 - information[row].Length;
                    for (int i = 0; i < extraSpaces; i++)
                    {
                        information[row] += " ";
                    }
                    Console.Write(information[row]);
                }
                else
                {
                    Console.Write("                    ");
                }
                Console.Write(CharLibrary.vertical);

                if(row < CraftableItemsLibrary.FullList.Length)
                {
                    string outputString = $"{CraftableItemsLibrary.FullList[row].item.FormattedName()} x{CraftableItemsLibrary.FullList[row].amount}";
                    int extraSpaces = 20 - outputString.Length;
                    for(int i = 0; i < extraSpaces; i++)
                    {
                        outputString += " ";
                    }
                    Console.Write(outputString);
                }
                else
                {
                    Console.Write("                    ");
                }
                if (row == player.craftingPosition)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{CharLibrary.leftArrow}\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.Write($"{CharLibrary.vertical}\n");
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;

            player.DisplayInventory();
        }

        public void PlaceNode(int[] coordinates, Item nodeType)
        {
            try
            {
                gridCoordinate[coordinates[0], coordinates[1]].item = nodeType;
            }
            catch (IndexOutOfRangeException) { }
        }

        public void RemoveNode(int[] coordinates)
        {
            try
            {
                gridCoordinate[coordinates[0], coordinates[1]].item = ItemLibrary.AIR;
            }
            catch (IndexOutOfRangeException) { }
        }

        public Item DamageNode(int[] coordinates, int damage)
        {
            gridCoordinate[coordinates[0], coordinates[1]].item.durability -= damage;
            if (gridCoordinate[coordinates[0], coordinates[1]].item.durability <= 0)
            {
                Item drop = new Item();
                drop.GetCopyOf(gridCoordinate[coordinates[0], coordinates[1]].item);
                gridCoordinate[coordinates[0], coordinates[1]].item = ItemLibrary.AIR;
                return drop;
            }
            return ItemLibrary.AIR;
        }

        public void Clear()
        {
            for (int row = 0; row < GRID_HEIGHT; row++)
            {
                for (int column = 0; column < GRID_WIDTH; column++)
                {
                    gridCoordinate[row, column].item.GetCopyOf(ItemLibrary.AIR);
                }
            }
        }

        public void Tick()
        {
            for (int row = 0; row < GRID_HEIGHT; row++)
            {
                for (int column = 0; column < GRID_WIDTH; column++)
                {
                    int[] coordinate = new int[] { row, column };
                    Item currentItem = ItemAt(coordinate);
                    if (currentItem.action != null)
                    {
                        currentItem.action(this, coordinate, null, currentItem);
                    }
                }
            }
        }
    }
}