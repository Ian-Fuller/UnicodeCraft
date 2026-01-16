using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnicodeCraft
{
    public class Item
    {
        //Information
        public ItemLibrary.ItemTypes itemName;
        public ItemLibrary.ItemTags[] itemTags;
        public int itemQuantity;
        //Icons
        public char inventoryIcon;
        public char gridIcon;
        //Colors
        public ConsoleColor itemFGColor;
        public ConsoleColor itemBGColor;
        //Special properties
        public bool conceal;
        public bool walkThrough;
        public bool transparent;
        public int lightLevel;
        //Durability
        public int durability;
        public int damage;
        public ItemLibrary.ItemTags[] toolsRequired;
        public int[] damageBoosts;
        //Action
        public Action<Grid, int[], Player, Item> action;

        public Item()
        {
            
        }
        public Item(
            ItemLibrary.ItemTypes itemName, ItemLibrary.ItemTags[] itemTags, int itemQuantity,
            char inventoryIcon, char gridIcon,
            ConsoleColor itemFGColor, ConsoleColor itemBGColor,
            bool conceal, bool walkThrough, bool transparent,
            int lightLevel,
            int durability, int damage, ItemLibrary.ItemTags[] toolsRequired, int[] damageBoosts,
            Action<Grid, int[], Player, Item> action
        )
        {
            //Information
            this.itemName = itemName;
            this.itemTags = itemTags;
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
            //Action
            this.action = action;
        }

        //Makes an item a copy of an item so they can be in a different state than the original
        public virtual void GetCopyOf(Item original)
        {
            // Initialize properties (namely durability)
            original = ItemLibrary.FullList.Where(item => item.itemName == original.itemName).First();

            //Information
            itemName = original.itemName;
            itemTags = original.itemTags;
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
            //Action
            action = original.action;
        }
        //Displays item and changes colors beforehand
        public void DisplayItem()
        {
            if (transparent) //"Transparent" items
            {
                Console.ForegroundColor = itemFGColor;
                Console.Write(gridIcon);
            }
            else //Full blocks
            {
                Console.ForegroundColor = itemFGColor;
                Console.BackgroundColor = itemBGColor;
                Console.Write(gridIcon);
            }
        }
    }

    public class ItemLibrary
    {
        public static Item AIR = new Item(
            ItemTypes.AIR, new ItemTags[] { ItemTags.NONE }, 0, // Name, Tags, Default quantity
            ' ', ' ',                                           // Inventory icon, Grid icon
            ConsoleColor.Black, ConsoleColor.Black,             // FG color, BG color
            false, true, true, 0,                               // Conceal, Walk through, Transparent, Light level
            0, 1, new ItemTags[] { }, new int[] { 0 },          // Durability, Damage, Resistances, Resistance values
            null                                                // Special action
        );
        public static Item STONE = new Item(
            ItemTypes.STONE, new ItemTags[] { }, 1,
            ' ', ' ',
            ConsoleColor.Gray, ConsoleColor.Gray,
            false, false, false, 0,
            150, 15, new ItemTags[] { ItemTags.PICKAXE, ItemTags.AXE, ItemTags.SWORD }, new int[] { 10, 4, 2 },
            null
        );
        public static Item COAL_ORE = new Item(
            ItemTypes.COAL_ORE, new ItemTags[] { }, 1,
            CharLibrary.leavesThin, CharLibrary.leavesThin,
            ConsoleColor.Black, ConsoleColor.Gray,
            false, false, false, 0,
            150, 15, new ItemTags[] { ItemTags.PICKAXE, ItemTags.AXE, ItemTags.SWORD }, new int[] { 10, 4, 2 },
            null
        );
        public static Item WOODEN_LOG = new Item(
            ItemTypes.WOODEN_LOG, new ItemTags[] { }, 1,
            CharLibrary.trunkThin, CharLibrary.trunkThin,
            ConsoleColor.DarkRed, ConsoleColor.Black,
            false, false, true, 0,
            100, 10, new ItemTags[] { ItemTags.AXE, ItemTags.SWORD, ItemTags.PICKAXE }, new int[] { 5, 2, 1 },
            null
        );
        public static Item TREE_LEAVES = new Item(
            ItemTypes.TREE_LEAVES, new ItemTags[] { }, 1,
            CharLibrary.leavesThick, CharLibrary.leavesThick,
            ConsoleColor.DarkGreen, ConsoleColor.Black,
            true, true, true, 0,
            0, 1, new ItemTags[] { ItemTags.NONE }, new int[] { 0 },
            null
        );
        public static Item WOODEN_PLANK = new Item(
            ItemTypes.WOODEN_PLANK, new ItemTags[] { }, 1,
            '│', '│',
            ConsoleColor.DarkYellow, ConsoleColor.Yellow,
            false, false, false, 0,
            100, 10, new ItemTags[] { ItemTags.AXE, ItemTags.SWORD, ItemTags.PICKAXE }, new int[] { 5, 2, 1 },
            null
        );
        public static Item APPLE = new Item(
            ItemTypes.APPLE, new ItemTags[] { ItemTags.FOOD }, 1,
            CharLibrary.apple, CharLibrary.apple,
            ConsoleColor.Red, ConsoleColor.Black,
            false, true, true, 0,
            1, 1, new ItemTags[] { ItemTags.NONE }, new int[] { 1 },
            null // replenish satiation
        );
        public static Item TORCH = new Item(
            ItemTypes.TORCH, new ItemTags[] { ItemTags.LIGHT_SOURCE }, 1,
            CharLibrary.torch, CharLibrary.torch,
            ConsoleColor.DarkRed, ConsoleColor.Black,
            false, true, true, 3,
            1, 1, new ItemTags[] { ItemTags.NONE }, new int[] { 1 },
            null // possibly make this determine lighting behavior
        );
        public static Item STICK = new Item(
            ItemTypes.STICK, new ItemTags[] { }, 1,
            '/', '/',
            ConsoleColor.DarkRed, ConsoleColor.Black,
            false, true, true, 0,
            1, 1, new ItemTags[] { ItemTags.NONE }, new int[] { 1 },
            null
        );
        public static Item FLINT = new Item(
            ItemTypes.FLINT, new ItemTags[] { ItemTags.AXE }, 1,
            CharLibrary.triangle, CharLibrary.triangle,
            ConsoleColor.DarkGray, ConsoleColor.Black,
            false, true, true, 0,
            1, 5, new ItemTags[] { ItemTags.NONE }, new int[] { 1 },
            null
        );
        public static Item FLINT_PICKAXE = new Item(
            ItemTypes.FLINT_PICKAXE, new ItemTags[] { ItemTags.TOOL, ItemTags.PICKAXE }, 1,
            CharLibrary.pickaxe, CharLibrary.pickaxe,
            ConsoleColor.DarkGray, ConsoleColor.Black,
            false, true, true, 0,
            1, 5, new ItemTags[] { ItemTags.NONE }, new int[] { 1 },
            null
        );
        public static Item MAGIC_WAND = new Item(
            ItemTypes.MAGIC_WAND, new ItemTags[] { }, 1,
            '/', '/',
            ConsoleColor.Magenta, ConsoleColor.Black,
            false, true, true, 0,
            1, 1000, new ItemTags[] { ItemTags.NONE }, new int[] { 1 },
            (grid, target, player, self) =>
            {
                if (player != null)
                {
                    int[] direction = new int[] { target[0] - player.row, target[1] - player.column };
                    while (target[0] >= 0 && target[0] < Grid.GRID_HEIGHT && target[1] >= 0 && target[1] < Grid.GRID_WIDTH)
                    {
                        grid.RemoveNode(target);
                        target[0] += direction[0];
                        target[1] += direction[1];
                    }
                }
            }
        );
        public static Item BUNNY_RABBIT = new Item(
            ItemTypes.BUNNY_RABBIT, new ItemTags[] { ItemTags.NONE }, 1,
            'v', 'v',
            ConsoleColor.White, ConsoleColor.Black,
            false, false, true, 0,
            0, 1, new ItemTags[] { }, new int[] { 0 },
            (grid, target, player, self) =>
            {
                Random rand = new Random();
                int[] destination = new int[] { target[0] + rand.Next(-1, 1),  target[1] + rand.Next(-1, 1) };
                grid.RemoveNode(target);
                grid.PlaceNode(destination, self);

                Console.WriteLine("Rabbit noise");
            }
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
            FLINT_PICKAXE,
            MAGIC_WAND,
            BUNNY_RABBIT
        };

        public enum ItemTypes
        {
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
            FLINT_PICKAXE,
            MAGIC_WAND,
            BUNNY_RABBIT
        }

        public enum ItemTags
        {
            NONE,
            FOOD,
            LIGHT_SOURCE,
            TOOL,
            AXE,
            PICKAXE,
            SWORD
        }

        // Item's name is used to retrieve the proper description when needed
        public string[] ItemDescriptions =
        {
            "The absence of an item - can be moved through.", // Air
            "Durable building block and crafting material.", // Stone
            "Can be burned as a fuel source and to create light.", // Coal
            "Can be used to make crafting materials.", // Wooden Log
            "Can be walked under. Anything under the leaves will be concealed.", //Tree Leaves
            "Building and crafting material.", // Wooden Plank
            "Food item - can be used to restore satiation.", // Apple
            "Used for lighting at night and in dark areas.", // Torch
            "Crafting material for tools and other things.", // Stick
            "Tool and crafting material.", // Flint
            "Used for mining stone.", // Flint Pickaxe
            "It's a mystery." // Magic Wand
        };
    }
}