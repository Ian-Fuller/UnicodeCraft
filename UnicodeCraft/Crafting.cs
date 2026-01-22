using System;
using System.Collections.Generic;
using System.Text;

namespace UnicodeCraft
{
    public class CraftableItem
    {
        public Item item;
        public int amount;
        public Item[] requiredMaterials;
        public int[] requiredAmounts;

        public CraftableItem()
        {

        }
        public CraftableItem(Item item, int amount, Item[] requiredMaterials, int[] requiredAmounts)
        {
            this.item = item;
            this.amount = amount;
            this.requiredMaterials = requiredMaterials;
            this.requiredAmounts = requiredAmounts;
        }
    }
    public class CraftableItemsLibrary
    {
        //Wooden Planks
        public static CraftableItem WOODEN_PLANK = new CraftableItem(
            ItemLibrary.WOODEN_PLANK, 4, 
            new Item[] { ItemLibrary.WOODEN_LOG }, new int[] { 1 }
        );
        //Stick
        public static CraftableItem STICK = new CraftableItem(
            ItemLibrary.STICK, 4,
            new Item[] { ItemLibrary.WOODEN_PLANK }, new int[] { 1 }
        );
        //Torch
        public static CraftableItem TORCH = new CraftableItem(
            ItemLibrary.TORCH, 4,
            new Item[] { ItemLibrary.STICK, ItemLibrary.COAL_ORE }, new int[] { 1, 1 }
        );
        //Flint Pickaxe
        public static CraftableItem PICKAXE = new CraftableItem(
            ItemLibrary.PICKAXE, 1,
            new Item[] { ItemLibrary.STICK, ItemLibrary.METAL_ORE }, new int[] { 2, 3 }
        );

        public static CraftableItem[] FullList = {
            WOODEN_PLANK,
            STICK,
            TORCH,
            PICKAXE
        };
    }

    public class Crafting
    {

    }
}
