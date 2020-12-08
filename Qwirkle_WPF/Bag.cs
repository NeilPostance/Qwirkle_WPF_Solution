using System;
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
                    for (int colourLoop = 0; colourLoop < colourCount; colourLoop++) //loop through the colours
                    {
                        for (int shapeLoop = 0; shapeLoop < shapeCount; shapeLoop++) // loop through the symbols
                        {
                            tilesInBag.Add(new Tile(colourLoop, shapeLoop));

                            //Console.WriteLine($"There are {tilesInBag.Count} tiles in the bag");

                            //Console.WriteLine($"Added {tilesInBag[tilesInBag.Count - 1]} count {tilesInBag.Count}");
                            //System.Threading.Thread.Sleep(1000);
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
