using System;
using System.Collections.Generic;
using System.Text;

namespace UnicodeCraft
{
    public class Player
    {
        //Information used for the Grid class
        public int row;
        public int column;
        public bool firstGeneration = true;

        //inventory
        public Item[] inventory = new Item[10];
        public int inventoryPosition = 0;
        public bool inInventory = false;

        //Stats
        public int health = 100;
        public int satiation = 100;
        public bool concealed = false;

        //Crafting
        public bool inCraftingMenu = false;
        public int craftingPosition = 0;

        public Player()
        {
            ItemLibrary itemList = new ItemLibrary();
            for (int i = 0; i < inventory.Length; i++)
            {
                inventory[i] = ItemLibrary.AIR;
            }
        }

        public void SetStart()
        {
            if(!firstGeneration)
            {
                if(row == 0)
                {
                    row = Grid.GRID_HEIGHT - 2;
                }
                else if (column == 0)
                {
                    column = Grid.GRID_WIDTH - 2;
                }
                else if (row == Grid.GRID_HEIGHT - 1)
                {
                    row = 1;
                }
                else if (column == Grid.GRID_WIDTH - 1)
                {
                    column = 1;
                }
            }
            else
            {
                Random rand = new Random();
                row = rand.Next(1, Grid.GRID_HEIGHT);
                column = rand.Next(1, Grid.GRID_WIDTH);
                firstGeneration = false;
            }
        }

        public void GiveItem(Item item, int chance, int amount)
        {
            Random rand = new Random();
            int result = rand.Next(0, 100);
            if(result < chance)
            {
                bool inInventory = false;
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i].itemName == item.itemName)
                    {
                        inventory[i].itemQuantity += amount;
                        inInventory = true;
                        break;
                    }
                }
                if (!inInventory)
                {
                    for (int i = 0; i < inventory.Length; i++)
                    {
                        if (inventory[i].itemName == ItemLibrary.AIR.itemName)
                        {
                            inventory[i] = new Item();
                            inventory[i].GetCopyOf(item);
                            inventory[i].itemQuantity = amount;
                            break;
                        }
                    }
                }
            }
        }

        public void PlaceAndDestroy(ref Node gridCoordinate)
        {
            //Places item if air
            if (gridCoordinate.item.itemName == ItemLibrary.AIR.itemName)
            {
                gridCoordinate.item = new Item();
                gridCoordinate.item.GetCopyOf(inventory[inventoryPosition]);
                inventory[inventoryPosition].itemQuantity--;
                if (inventory[inventoryPosition].itemQuantity <= 0)
                {
                    inventory[inventoryPosition].GetCopyOf(ItemLibrary.AIR);
                }
            }
            //Attacks item if not air
            else
            {
                if(gridCoordinate.item.durability > 0)
                {
                    int totalDamage = inventory[inventoryPosition].damage;
                    for(int i = 0; i < gridCoordinate.item.toolsRequired.Length; i++)
                    {
                        if(inventory[inventoryPosition].itemTags.Contains(gridCoordinate.item.toolsRequired[i]))
                        {
                            totalDamage *= gridCoordinate.item.damageBoosts[i];
                            break;
                        }
                    }
                    gridCoordinate.item.durability -= totalDamage;
                }
                if (gridCoordinate.item.durability <= 0)
                {
                    for(int i = 0; i < ItemLibrary.FullList.Length; i++)
                    {
                        if(ItemLibrary.FullList[i].itemName == gridCoordinate.item.itemName)
                        {
                            GiveItem(ItemLibrary.FullList[i], 100, 1);
                            if(gridCoordinate.item.itemName == ItemLibrary.WOODEN_LOG.itemName)
                            {
                                GiveItem(ItemLibrary.APPLE, 33, 1);
                            }
                            break;
                        }
                    }
                    gridCoordinate.item = ItemLibrary.AIR; //Sets item to air
                }
            }
        }

        public void ControlPlayer(char inputKey, Grid gameGrid)
        {
            if (!inInventory && !inCraftingMenu)
            {
                //movement
                if (inputKey == 'w' && gameGrid.gridCoordinate[row - 1, column].item.walkThrough)
                {
                    row--;
                }
                else if (inputKey == 'a' && gameGrid.gridCoordinate[row, column - 1].item.walkThrough)
                {
                    column--;
                }
                else if (inputKey == 's' && gameGrid.gridCoordinate[row + 1, column].item.walkThrough)
                {
                    row++;
                }
                else if (inputKey == 'd' && gameGrid.gridCoordinate[row, column + 1].item.walkThrough)
                {
                    column++;
                }
                //block destruction/placement
                else if (inputKey == 'W')
                {
                    PlaceAndDestroy(ref gameGrid.gridCoordinate[row - 1, column]);
                }
                else if (inputKey == 'A')
                {
                    PlaceAndDestroy(ref gameGrid.gridCoordinate[row, column - 1]);
                }
                else if (inputKey == 'S')
                {
                    PlaceAndDestroy(ref gameGrid.gridCoordinate[row + 1, column]);
                }
                else if (inputKey == 'D')
                {
                    PlaceAndDestroy(ref gameGrid.gridCoordinate[row, column + 1]);
                }
                else if(inputKey == '2')
                {
                    inventory[inventoryPosition].Action(ref gameGrid, row, column);
                }
                else if (inputKey == 4)
                {

                }
                else if (inputKey == 6)
                {

                }
                else if (inputKey == 8)
                {

                }
                else if (inputKey == 'e')
                {
                    inInventory = true;
                }
                else if (inputKey == 'c')
                {
                    inCraftingMenu = true;
                }
            }
            else if(inInventory)
            {
                if (inputKey == 'a')
                {
                    if (inventoryPosition > 0)
                    {
                        inventoryPosition--;
                    }
                    else
                    {
                        inventoryPosition += 9;
                    }
                }
                else if (inputKey == 'd')
                {
                    if (inventoryPosition < 9)
                    {
                        inventoryPosition++;
                    }
                    else
                    {
                        inventoryPosition -= 9;
                    }
                }
                else if(inputKey == 'e')
                {
                    inInventory = false;
                }
            }
            else if(inCraftingMenu)
            {
                if (inputKey == 'w')
                {
                    if (craftingPosition > 0)
                    {
                        craftingPosition--;
                    }
                    else
                    {
                        craftingPosition = CraftableItemsLibrary.FullList.Length - 1;
                    }
                }
                else if (inputKey == 's')
                {
                    if (craftingPosition < CraftableItemsLibrary.FullList.Length - 1)
                    {
                        craftingPosition++;
                    }
                    else
                    {
                        craftingPosition = 0;
                    }
                }
                else if (inputKey == 13)
                {
                    List<bool> canCraft = new List<bool>();
                    for(int i = 0; i < CraftableItemsLibrary.FullList[craftingPosition].requiredMaterials.Length; i++)
                    {
                        canCraft.Add(false);
                        for(int j = 0; j < inventory.Length; j++)
                        {
                            if(inventory[j].itemName == CraftableItemsLibrary.FullList[craftingPosition].requiredMaterials[i].itemName && inventory[j].itemQuantity >= CraftableItemsLibrary.FullList[craftingPosition].requiredAmounts[i])
                            {
                                canCraft[i] = true;
                            }
                        }
                    }
                    if (!canCraft.Contains(false))
                    {
                        for (int i = 0; i < CraftableItemsLibrary.FullList[craftingPosition].requiredMaterials.Length; i++)
                        {
                            for (int j = 0; j < inventory.Length; j++)
                            {
                                if (inventory[j].itemName == CraftableItemsLibrary.FullList[craftingPosition].requiredMaterials[i].itemName)
                                {
                                    inventory[j].itemQuantity -= CraftableItemsLibrary.FullList[craftingPosition].requiredAmounts[i];
                                    if(inventory[j].itemQuantity == 0)
                                    {
                                        inventory[j] = ItemLibrary.AIR;
                                    }
                                }
                            }
                        }
                        GiveItem(CraftableItemsLibrary.FullList[craftingPosition].item, 100, CraftableItemsLibrary.FullList[craftingPosition].amount);
                    }
                }
                else if (inputKey == 'c')
                {
                    inCraftingMenu = false;
                }
            }
        }

        public void DisplayInventory()
        {
            //int inventoryIndex = 0;
            //Console.Write(CharLibrary.upperLeftCorner); //top border
            //for (int i = 0; i < 19; i++)
            //{
            //    Console.Write(CharLibrary.horizontal + "" + CharLibrary.horizontalDown);
            //}
            //Console.Write(CharLibrary.horizontal + "" + CharLibrary.upperRightCorner + "\n");
            //for (int i = 0; i < 20; i++) //row 1
            //{
            //    Console.Write(CharLibrary.vertical);
            //    if (inventoryIndex == inventoryPosition)
            //    {
            //        Console.BackgroundColor = ConsoleColor.DarkMagenta;
            //    }
            //    ItemLibrary.DisplayItem(inventory[inventoryIndex]);
            //    Console.ForegroundColor = ConsoleColor.Gray;
            //    Console.BackgroundColor = ConsoleColor.Black;
            //    inventoryIndex++;
            //}
            //Console.Write(CharLibrary.vertical + "\n" + CharLibrary.verticalRight); //middle border
            //for (int i = 0; i < 19; i++)
            //{
            //    Console.Write(CharLibrary.horizontal + "" + CharLibrary.crossroads);
            //}
            //Console.Write(CharLibrary.horizontal + "" + CharLibrary.verticalLeft + "\n");
            //for (int i = 0; i < 20; i++) //row 2
            //{
            //    Console.Write(CharLibrary.vertical);
            //    if (inventoryIndex == inventoryPosition)
            //    {
            //        Console.BackgroundColor = ConsoleColor.DarkMagenta;
            //    }
            //    ItemLibrary.DisplayItem(inventory[inventoryIndex]);
            //    Console.ForegroundColor = ConsoleColor.Gray;
            //    Console.BackgroundColor = ConsoleColor.Black;
            //    inventoryIndex++;
            //}
            //Console.Write(CharLibrary.vertical + "\n" + CharLibrary.lowerLeftCorner); //bottom border
            //for (int i = 0; i < 19; i++)
            //{
            //    Console.Write(CharLibrary.horizontal + "" + CharLibrary.horizontalUp);
            //}
            //Console.Write(CharLibrary.horizontal + "" + CharLibrary.lowerRightCorner + "\n");
            int inventoryIndex = 0;

            //Upper border
            Console.Write(CharLibrary.verticalRight); //top border
            for (int i = 0; i < 10; i++)
            {
                Console.Write(CharLibrary.horizontal + "" + CharLibrary.horizontalDown);
            }
            for (int i = 0; i < Grid.GRID_WIDTH - 20; i++)
            {
                Console.Write(CharLibrary.horizontal);
            }
            Console.Write(CharLibrary.horizontal + "\n");

            //Middle area
            Console.Write(CharLibrary.vertical);
            for (int i = 0; i < 10; i++)
            {
                inventory[i].DisplayItem();
                //ItemLibrary.DisplayItem(inventory[i]);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(CharLibrary.vertical);
            }
            Console.Write("\n");

            //Bottom border
            Console.Write(CharLibrary.lowerLeftCorner);
            for (int i = 0; i < 10; i++)
            {
                if(i < 9)
                {
                    if (i != inventoryPosition)
                    {
                        Console.Write(CharLibrary.horizontal + "" + CharLibrary.horizontalUp);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(CharLibrary.upArrow);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(CharLibrary.horizontalUp);
                    }
                }
                else
                {
                    if (i != inventoryPosition)
                    {
                        Console.Write(CharLibrary.horizontal + "" + CharLibrary.lowerRightCorner + "\n");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(CharLibrary.upArrow);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(CharLibrary.lowerRightCorner + "\n");
                    }
                }
            }
        }
    }
}
