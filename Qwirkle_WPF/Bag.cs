﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Qwirkle_WPF
{

    public static class Bag
    {

        public static List<Tile> tilesInBag = new List<Tile>(); // a collection of Tiles which are in the bag

        public static void FillTheBag(int colourCount, int shapeCount, int duplicateCount)
        {
            
                Bag.tilesInBag.Clear();
                for (int duplicateLoop = 0; duplicateLoop < duplicateCount; duplicateLoop++) //create the appropriate number of duplicates
                {
                    for (int colourLoop = 1; colourLoop < (colourCount+1); colourLoop++) //loop through the colours
                    {
                        for (int shapeLoop = 1; shapeLoop < (shapeCount+1); shapeLoop++) // loop through the symbols
                        {
                            tilesInBag.Add(new Tile((EnumColour)colourLoop, (EnumShape)shapeLoop));
                        }
                    }
                }
        }

        public static void ListTilesInBag()
        {
            Console.WriteLine($"The Bag Contains:");
            tilesInBag.ForEach(delegate (Tile tile)
            {
                Console.WriteLine($"{tile}");
            });
            Console.WriteLine($"There are {Bag.tilesInBag.Count} tiles in the bag.");
        }

        public static void EmptyTheBag()
        {
            tilesInBag.Clear();
        }

        public static int Count()
        {
            return tilesInBag.Count;
        }
    }
}
