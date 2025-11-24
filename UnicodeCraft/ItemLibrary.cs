using System;
using System.Collections.Generic;
using System.Text;

namespace UnicodeCraft
{
    public class Item
    {
        //Information
        public string itemName;
        public List<string> itemTags;
        public string itemDescription;
        public int itemQuantity;
        //Icons
        public char inventoryIcon;
        public char gridIcon;
        //Colors
        public string itemFGColor;
        public string itemBGColor;
        //Special properties
        public bool conceal;
        public bool walkThrough;
        public bool transparent;
        public int lightLevel;
        //Durability
        public int durability;
        public int damage;
        public string[] toolsRequired;
        public int[] damageBoosts;
        //Action properties
        public char[,] itemSprites;
        public int[,,] spriteOffsets;
        public string[,] itemFGColors;
        public string[,] itemBGColors;

        public Item()
        {

        }
        public Item(
            string itemName, List<string> itemTags, string itemDescription, int itemQuantity,
            char inventoryIcon, char gridIcon,
            string itemFGColor, string itemBGColor,
            bool conceal, bool walkThrough, bool transparent,
            int lightLevel,
            int durability, int damage, string[] toolsRequired, int[] damageBoosts,
            char[,] itemSprites, int[,,] spriteOffsets, string[,] itemFGColors, string[,] itemBGColors
        )
        {
            //Information
            this.itemName = itemName;
            this.itemTags = itemTags;
            this.itemDescription = itemDescription;
            this.itemQuantity = itemQuantity;
            //Icons
            this.inventoryIcon = inventoryIcon;
            this.gridIcon = gridIcon;
            //Colors
            this.itemFGColor = itemFGColor;
            this.itemBGColor = itemBGColor;
            //Special properties
            this.conceal = conceal;
            this.walkThrough = walkThrough;
            this.transparent = transparent;
            this.lightLevel = lightLevel;
            //Durability
            this.durability = durability;
            this.damage = damage;
            this.toolsRequired = toolsRequired;
            this.damageBoosts = damageBoosts;
            //Action properties
            this.itemSprites = itemSprites;
            this.spriteOffsets = spriteOffsets;
            this.itemFGColors = itemFGColors;
            this.itemBGColors = itemBGColors;
    }

        //Makes an item a copy of an item so they can be in a different state than the original
        public virtual void GetCopyOf(Item original)
        {
            //Information
            itemName = original.itemName;
            itemTags = original.itemTags;
            itemDescription = original.itemDescription;
            itemQuantity = original.itemQuantity;
            //Icons
            inventoryIcon = original.inventoryIcon;
            gridIcon = original.gridIcon;
            //Colors
            itemFGColor = original.itemFGColor;
            itemBGColor = original.itemBGColor;
            //Special properties
            conceal = original.conceal;
            walkThrough = original.walkThrough;
            transparent = original.transparent;
            lightLevel = original.lightLevel;
            //Durability
            durability = original.durability;
            damage = original.damage;
            toolsRequired = original.toolsRequired;
            damageBoosts = original.damageBoosts;
            //Action properties
            itemSprites = original.itemSprites;
            spriteOffsets = original.spriteOffsets;
            itemFGColors = original.itemFGColors;
            itemBGColors = original.itemBGColors;
        }
        //For useable and animated items
        public void Action(ref Grid gameGrid, int playerRow, int playerColumn)
        {
            if(itemTags.Contains("Tool"))
            {
                Actions.UseTool(ref gameGrid, this, playerRow, playerColumn);
            }
        }
        //Displays item and changes colors beforehand
        public void DisplayItem()
        {
            if (itemBGColor == "") //"Transparent" items
            {
                QuickColor.changeFG(itemFGColor);
                Console.Write(gridIcon);
            }
            else //Full blocks
            {
                QuickColor.changeFG(itemFGColor);
                QuickColor.changeBG(itemBGColor);
                Console.Write(gridIcon);
            }
        }
    }

    /*
    public class Tool : Item
    {
        public char[,] itemSprites;
        public int[,,] spriteOffsets;
        public string[,] itemFGColors;
        public string[,] itemBGColors;

        public Tool() : base()
        {

        }
        public Tool(
            string itemName, string itemTag, string itemDescription, int itemQuantity,
            char inventoryIcon, char gridIcon,
            string itemFGColor, string itemBGColor,
            bool conceal, bool walkThrough, bool transparent,
            int lightLevel,
            int durability, int damage, string[] toolsRequired, int[] damageBoosts,
            char[,] itemSprites, int[,,] spriteOffsets, string[,] itemFGColors, string[,] itemBGColors
        ) : base(
            itemName, itemTag, itemDescription, itemQuantity,
            inventoryIcon, gridIcon,
            itemFGColor, itemBGColor,
            conceal, walkThrough, transparent,
            lightLevel,
            durability, damage, toolsRequired, damageBoosts
        )
        {
            this.itemSprites = itemSprites;
            this.spriteOffsets = spriteOffsets;
            this.itemFGColors = itemFGColors;
            this.itemBGColors = itemBGColors;
        }

        public override void GetCopyOf(Item original)
        {
            base.GetCopyOf(original);
            Tool tempTool = (Tool)original;

            itemSprites = tempTool.itemSprites;
            spriteOffsets = tempTool.spriteOffsets;
            itemFGColors = tempTool.itemFGColors;
            itemBGColors = tempTool.itemBGColors;
        }
        public override void Action(ref Grid gameGrid, int playerRow, int playerColumn)
        {
            for(int i = 0; i < itemSprites.Length; i++)
            {
                for(int j = 0; j < j + 1; j++)
                {
                    try
                    {
                        if (gameGrid.topLayer[playerRow + spriteOffsets[i, j, 0], playerColumn + spriteOffsets[i, j, 1]].item.itemName == ItemLibrary.AIR.itemName)
                        {
                            gameGrid.topLayer[playerRow + spriteOffsets[i, j, 0], playerColumn + spriteOffsets[i, j, 1]].item.gridIcon = itemSprites[i, j];
                        }
                    }
                    catch(Exception ex)
                    {
                        //Ignore IOOB error
                    }
                }
            }
        }
    }
    */

    public class ItemLibrary
    {
        public static Item AIR = new Item(
            "Air", new List<string> { "" } , "The absence of an item - can be moved through.", 0, //Name, Desc., Default quantity
            ' ', ' ', //Inventory icon, Grid icon
            "Gray", "", //FG color, BG color
            false, true, true, 0, //Conceal, Walk through, Transparent, Light level
            0, 1, new string[] { "None" }, new int[] { 0 }, //Durability, Damage, Resistances, Resistance values
            null, null, null, null
        );
        public static Item STONE = new Item(
            "Stone", new List<string> { "" }, "Durable building block and crafting material.", 1,
            ' ', ' ',
            "Gray", "Gray",
            false, false, false, 0,
            150, 15, new string[] { "Pickaxe", "Axe", "Sword" }, new int[] { 10, 4, 2 },
            null, null, null, null
        );
        public static Item COAL_ORE = new Item(
            "Coal Ore", new List<string> { "" }, "Fuel source", 1,
            CharLibrary.leavesThin, CharLibrary.leavesThin,
            "Black", "Gray",
            false, false, false, 0,
            150, 15, new string[] { "Pickaxe", "Axe", "Sword" }, new int[] { 10, 4, 2 },
            null, null, null, null
        );
        public static Item WOODEN_LOG = new Item(
            "Wooden Log", new List<string> { "" }, "Can be used to make crafting materials.", 1,
            CharLibrary.trunkThin, CharLibrary.trunkThin,
            "DarkRed", "",
            false, false, false, 0,
            100, 10, new string[] { "Axe", "Sword", "Pickaxe" }, new int[] { 5, 2, 1 },
            null, null, null, null
        );
        public static Item TREE_LEAVES = new Item(
            "Tree Leaves", new List<string> { "" }, "Can be walked under. Anything under the leaves will be concealed.", 1,
            CharLibrary.leavesThick, CharLibrary.leavesThick,
            "DarkGreen", "",
            true, true, true, 0,
            0, 1, new string[] { "None" }, new int[] { 0 },
            null, null, null, null
        );
        public static Item WOODEN_PLANK = new Item(
            "Wooden Plank", new List<string> { "" }, "Building and crafting material.", 1,
            '│', '│',
            "DarkYellow", "Yellow",
            false, false, false, 0,
            100, 10, new string[] { "Axe", "Sword", "Pickaxe" }, new int[] { 5, 2, 1 },
            null, null, null, null
        );
        public static Item APPLE = new Item(
            "Apple", new List<string> { "Food" }, "Food item - can be used to restore satiation.", 1,
            CharLibrary.apple, CharLibrary.apple,
            "Red", "",
            false, true, true, 0,
            1, 1, new string[] { "All" }, new int[] { 1 },
            null, null, null, null
        );
        public static Item TORCH = new Item(
            "Torch", new List<string> { "Light Source" }, "Used for lighting at night and in dark areas.", 1,
            CharLibrary.torch, CharLibrary.torch,
            "DarkRed", "",
            false, true, true, 3,
            1, 1, new string[] { "All" }, new int[] { 1 },
            null, null, null, null
        );
        public static Item STICK = new Item(
            "Stick", new List<string> { "" }, "Crafting material for tools and other things.", 1,
            '/', '/',
            "DarkRed", "",
            false, true, true, 0,
            1, 1, new string[] { "All" }, new int[] { 1 },
            null, null, null, null
        );
        public static Item FLINT = new Item(
            "Flint", new List<string> { "Axe" }, "Tool and crafting material.", 1,
            CharLibrary.triangle, CharLibrary.triangle,
            "DarkGray", "",
            false, true, true, 0,
            1, 5, new string[] { "All" }, new int[] { 1 },
            null, null, null, null
        );
        public static Item FLINT_PICKAXE = new Item(
            "Flint Pickaxe", new List<string> { "Tool", "Pickaxe" }, "Used for mining stone.", 1,
            CharLibrary.pickaxe, CharLibrary.pickaxe,
            "DarkGray", "",
            false, true, true, 0,
            1, 5, new string[] { "All " }, new int[] { 1 },
            new char[,] { { CharLibrary.angleLL, '/' }, { CharLibrary.lineH, '|' }, { CharLibrary.angleLR, '\\' } },
            new int[,,] { { { -2, -1 }, { -1, 0 } }, { { -2, 0 }, { -1, 0 } }, { { -2, 1 }, { -1, 0 } } },
            new string[,] { { "Dark Gray", "DarkRed" }, { "Dark Gray", "DarkRed" }, { "Dark Gray", "DarkRed" } },
            new string[,] { { "", "" }, { "", "" }, { "", "" } }
        );

        public static Item[] FullList = {
            AIR,
            STONE,
            COAL_ORE,
            WOODEN_LOG,
            TREE_LEAVES,
            WOODEN_PLANK,
            APPLE,
            TORCH,
            STICK,
            FLINT,
            FLINT_PICKAXE
        };
    }

    public class Actions
    {
        public static void UseTool(ref Grid gameGrid, Item tool, int playerRow, int playerColumn)
        {
            //gameGrid.topLayer[playerRow + tool.spriteOffsets[0, 0, 0], playerColumn + tool.spriteOffsets[0, 0, 1]].item = new Item();
            //gameGrid.topLayer[playerRow + tool.spriteOffsets[0, 0, 0], playerColumn + tool.spriteOffsets[0, 0, 1]].item.GetCopyOf(ItemLibrary.AIR);
            //gameGrid.topLayer[playerRow + tool.spriteOffsets[0, 0, 0], playerColumn + tool.spriteOffsets[0, 0, 1]].item.gridIcon = '/';

            for (int i = 0; i < tool.itemSprites.Length; i++)
            {
                for (int j = 0; j < tool.spriteOffsets.Length; j++)
                {
                    try
                    {
                        if (gameGrid.gridCoordinate[playerRow + tool.spriteOffsets[i, j, 0], playerColumn + tool.spriteOffsets[i, j, 1]].item.itemName == ItemLibrary.AIR.itemName)
                        {
                            gameGrid.gridCoordinate[playerRow + tool.spriteOffsets[i, j, 0], playerColumn + tool.spriteOffsets[i, j, 1]].item.gridIcon = tool.itemSprites[i, j];
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
                //for(int j = 0; j < 1000000; j++)
                //{

                //}
            }
        }
    }
}
