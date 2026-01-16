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
            inventory[0] = ItemLibrary.MAGIC_WAND;
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
                row = Grid.GRID_HEIGHT / 2;
                column = Grid.GRID_WIDTH / 2;
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

        public void Action(ref Grid gameGrid, ref Node gridCoordinate, int[] target)
        {
            //Uses the item's action ability if it has one
            if (inventory[inventoryPosition].action != null)
            {
                inventory[inventoryPosition].action(gameGrid, target, this);
            }
            //Places item if air
            else if (gridCoordinate.item.itemName == ItemLibrary.AIR.itemName)
            {
                //gameGrid.PlaceNode(target, )

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
                int totalDamage = inventory[inventoryPosition].damage;
                for (int i = 0; i < gridCoordinate.item.toolsRequired.Length; i++)
                {
                    for (int j = 0; j < inventory[inventoryPosition].itemTags.Length; j++)
                    {
                        if (gridCoordinate.item.toolsRequired[i] == inventory[inventoryPosition].itemTags[j])
                        {
                            totalDamage *= gridCoordinate.item.damageBoosts[i];
                            break;
                        }
                    }
                }
                Item drop = gameGrid.DamageNode(target, totalDamage);
                GiveItem(drop, 100, 1);
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
                else if (inputKey == 'i')
                {
                    Action(ref gameGrid, ref gameGrid.gridCoordinate[row - 1, column], new int[] { row - 1, column });
                }
                else if (inputKey == 'j')
                {
                    Action(ref gameGrid, ref gameGrid.gridCoordinate[row, column - 1], new int[] { row, column - 1 });
                }
                else if (inputKey == 'k')
                {
                    Action(ref gameGrid, ref gameGrid.gridCoordinate[row + 1, column], new int[] { row + 1, column });
                }
                else if (inputKey == 'l')
                {
                    Action(ref gameGrid, ref gameGrid.gridCoordinate[row, column + 1], new int[] { row, column + 1 });
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
