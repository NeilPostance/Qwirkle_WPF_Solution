using System;
using System.Threading;

namespace Qwirkle_WPF
{
    public static class Grid
    {
        public static Tile[,] tileGrid = new Tile[MainClass.GridRows, MainClass.GridColumns];

        public enum EnumDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        public static int TileCountInGrid()
        {
            int tileCount = 0;
            foreach (Tile t in tileGrid)
            {
                try
                {
                    Console.WriteLine(t.ToString());
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("No colour/shape!");
                }
                tileCount++;
            }
            return tileCount;
        }

        public static Tile GetTile(int row, int col)
        {
            if (tileGrid[row,col] == null)
            {
                return new Tile(EnumColour.None, EnumShape.None);
            }
            else
            {
                return tileGrid[row, col];
            }
            
        }

        public static string GetPositionContent(int row, int col, out EnumColour outColour, out EnumShape outShape)
        {
            if (tileGrid[row, col] is null)
            {
                outColour = EnumColour.None;
                outShape = EnumShape.None;
                return "x";
            }
            else
            {
                outColour = tileGrid[row, col].Colour;
                outShape = tileGrid[row, col].Shape;
                return tileGrid[row, col].ToString();
            }
           
        }

        public static void PlaceTile(Tile tile, int row, int column)
        {
            tileGrid[row, column] = tile;
            tile.GridXref = (row, column);
            Console.WriteLine($"{tile} has been placed at ({row},{column})");
        }

        public static bool Empty()
        {
            tileGrid = new Tile[MainClass.GridRows, MainClass.GridColumns];
            return true;
        }

        public static bool IsEmptyPosition(int row, int column)
        {
            try
            {
                Tile tile = Grid.tileGrid[row, column];
                if (tile != null)
                    return false;
                else
                    return true;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("invalid grid refs");
                return false;
            }
            
        }

        public static bool IsEmpty()
        {
            foreach (Tile t in Grid.tileGrid)
            {
                if (t != null )
                {
                    if (t.IsPlaceholder==false)
                        return false;
                    else
                        Console.WriteLine(t.ToString());
                }
                else
                    continue;
            }
            return true;
        }
    
        public static bool QueryNearbyLocation(int startingRow, int startingColumn, int direction, int distance, out Tile tile) //return true if there is a tile, false if there isn't
        {
            //if there's a tile in the location, return the tile
            int? candidateRow;
            int? candidateColumn;

            //work out the candidate coordinates
            switch (direction)
            {
                case 0: //left
                    //Console.WriteLine("Left");
                    candidateRow = startingRow - distance;
                    candidateColumn = startingColumn;
                    break;
                case 1: //right
                    //Console.WriteLine("Right");
                    candidateRow = startingRow + distance;
                    candidateColumn = startingColumn;
                    break;
                case 2: //up
                    //Console.WriteLine("Up");
                    candidateRow = startingRow;
                    candidateColumn = startingColumn - distance;
                    break;
                case 3: //down
                    //Console.WriteLine("Down");
                    candidateRow = startingRow;
                    candidateColumn = startingColumn + distance;
                    break;
                default:
                    Console.WriteLine("Invalid direction");
                    candidateRow = -1;
                    candidateColumn = -1;
                    break;
            }
            if (candidateRow < 0 || candidateColumn < 0)  //if the position doesn't exist, return null
            {
                //Console.WriteLine($"coordinate {candidateX}:{candidateY} does not exist");
                tile = null;
                return false;
            }
            else if (IsEmptyPosition((int)candidateRow, (int)candidateColumn))  // the position exists, but there's nothing in it, return null
            {
                //Console.WriteLine($"coordinate {candidateX}:{candidateY} is empty");
                tile = null;
                return false;
            }
            else
            {
                tile = tileGrid[(int)candidateRow, (int)candidateColumn];
                //Console.WriteLine($"Tile {tile.Colour}:{tile.Shape} found at coordinate {candidateX}:{candidateY} (by coordinates)");
                return true; //if there's a tile in the position, return it.
            }
        }

        public static bool QueryNearbyLocation(int startingRow, int startingColumn, int direction, int distance) //return true if there is a tile, false if there isn't
        {
            //if there's a tile in the location, return the tile
            int? candidateRow;
            int? candidateColumn;

            //work out the candidate coordinates
            switch (direction)
            {
                case 0: //left
                    //Console.WriteLine("Left");
                    candidateRow = startingRow - distance;
                    candidateColumn = startingColumn;
                    break;
                case 1: //right
                    //Console.WriteLine("Right");
                    candidateRow = startingRow + distance;
                    candidateColumn = startingColumn;
                    break;
                case 2: //up
                    //Console.WriteLine("Up");
                    candidateRow = startingRow;
                    candidateColumn = startingColumn - distance;
                    break;
                case 3: //down
                    //Console.WriteLine("Down");
                    candidateRow = startingRow;
                    candidateColumn = startingColumn + distance;
                    break;
                default:
                    Console.WriteLine("Invalid direction");
                    candidateRow = -1;
                    candidateColumn = -1;
                    break;
            }
            if (candidateRow < 0 || candidateColumn < 0)  //if the position doesn't exist, return null
            {
                //Console.WriteLine($"coordinate {candidateX}:{candidateY} does not exist");
                return false;
            }
            else if (IsEmptyPosition((int)candidateRow, (int)candidateColumn))  // the position exists, but there's nothing in it, return null
            {
                //Console.WriteLine($"coordinate {candidateX}:{candidateY} is empty");
                return false;
            }
            else
            {
                //Console.WriteLine($"Tile {tile.Colour}:{tile.Shape} found at coordinate {candidateX}:{candidateY} (by coordinates)");
                return true; //if there's a tile in the position, return true.
            }
        }

        public static bool QueryNearbyLocation(Tile startingTile, int direction, int distance, out Tile tile) //return true if there is a tile, false if there isn't
        {
            //if there's a tile in the location, return the tile
            int? candidateRow;
            int? candidateColumn;

            //retrieve the starting coordinates
            var (startingRow, startingColumn) = startingTile.GridXref;

            //work out the candidate coordinates
            switch (direction)
            {
                case 0: //left
                    //Console.WriteLine("Left");
                    candidateRow = startingRow;
                    candidateColumn = startingColumn - distance;
                    break;
                case 1: //right
                    //Console.WriteLine("Right");
                    candidateRow = startingRow;
                    candidateColumn = startingColumn + distance;
                    break;
                case 2: //up
                    //Console.WriteLine("Up");
                    candidateRow = startingRow - distance;
                    candidateColumn = startingColumn;
                    break;
                case 3: //down
                    //Console.WriteLine("Down");
                    candidateRow = startingRow + distance;
                    candidateColumn = startingColumn;
                    break;
                default:
                    Console.WriteLine("Invalid direction");
                    candidateRow = -1;
                    candidateColumn = -1;
                    break;
            }
            //Console.WriteLine($"Checking coordinates {candidateRow},{candidateColumn}");

            if (candidateRow < 0 || candidateColumn < 0)  //if the position doesn't exist, return null
            {
                //Console.WriteLine($"coordinate {candidateX}:{candidateY} does not exist");
                tile = null;
                return false;
            }
            else if (IsEmptyPosition((int)candidateRow, (int)candidateColumn))  // the position exists, but there's nothing in it, return null
            {
                //Console.WriteLine($"coordinate {candidateX}:{candidateY} is empty");
                tile = null;
                return false;
            }
            else
            {
                try {
                    tile = tileGrid[(int)candidateRow, (int)candidateColumn];
                    Console.WriteLine($"Tile {tile} found at coordinate {candidateRow}:{candidateColumn} (by tile ref)");
                    return true; //if there's a tile in the position, return it.
                }
                catch (IndexOutOfRangeException)
                {
                    tile = null;
                    Console.WriteLine("InvalidGridRefs in QueryNearbyLocation");
                    return false;
                }

               
            }
        }
    }
}
