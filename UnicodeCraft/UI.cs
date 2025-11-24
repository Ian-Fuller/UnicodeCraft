using System;
using System.Collections.Generic;
using System.Text;

namespace UnicodeCraft
{
    class UI
    {
        public static void Vertical()
        {
            Console.Write(CharLibrary.vertical);
        }
        public static void Horizontal()
        {
            Console.Write(CharLibrary.horizontal);
        }
        public static void Corner1()
        {
            Console.Write(CharLibrary.upperLeftCorner);
        }
        public static void Corner2()
        {
            Console.Write(CharLibrary.upperRightCorner);
        }
        public static void Corner3()
        {
            Console.Write(CharLibrary.lowerLeftCorner);
        }
        public static void Corner4()
        {
            Console.Write(CharLibrary.lowerRightCorner);
        }

        public static void TopBorder()
        {
            Console.Write(CharLibrary.upperLeftCorner);
            for (int i = 0; i < Grid.GRID_WIDTH; i++)
            {
                Console.Write(CharLibrary.horizontal);
            }
            Console.Write(CharLibrary.horizontalDown);
            for (int i = 0; i < 20; i++)
            {
                Console.Write(CharLibrary.horizontal);
            }
            Console.Write(CharLibrary.horizontalDown);
            for (int i = 0; i < 20; i++)
            {
                Console.Write(CharLibrary.horizontal);
            }
            Console.Write(CharLibrary.upperRightCorner + "\n");
        }
        public static void MiddleBorder()
        {
            Console.Write(CharLibrary.verticalRight);
            for (int i = 0; i < Grid.GRID_WIDTH; i++)
            {
                Console.Write(CharLibrary.horizontal);
            }
            Console.Write(CharLibrary.verticalLeft);
            //             Information:
            Console.Write("Information:        " + CharLibrary.vertical);
            Console.Write("Crafting:           " + CharLibrary.vertical + "\n");
        }
    }
}
