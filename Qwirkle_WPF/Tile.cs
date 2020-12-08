using System;

namespace Qwirkle_WPF
{
    public class Tile
    {
        private static int? count;

       

        public EnumColour Colour { get; }
        public EnumShape Shape { get; }

        public bool IsPlaceholder { get; }

        public (int, int) GridXref { get; set; }

        public Tile(int colour, int shape)
        {
            if (count == null)
            {
                count = 0;
                Console.WriteLine("Creating First Tile!");
            }
            else
            {
                count++;
            }
            this.Colour = (EnumColour)colour;
            this.Shape = (EnumShape)shape;
            this.IsPlaceholder = false;

            //Id = (int)count;
            //Console.WriteLine($"Created Tile with c{this.Colour}, s{this.Shape}, id:{this.Id}");
        }

        public Tile(EnumColour colour, EnumShape shape)
        {
            if (count == null)
            {
                count = 0;
                Console.WriteLine("Creating First Tile!");
            }
            else
            {
                count++;
            }
            this.Colour = colour;
            this.Shape = shape;
            this.IsPlaceholder = false;

            //Id = (int)count;
            //Console.WriteLine($"Created Tile with c{this.Colour}, s{this.Shape}, id:{this.Id}");
        }

        public Tile(bool isPlaceholder)
        {
            this.IsPlaceholder = isPlaceholder;
            this.Colour = EnumColour.None;
            this.Shape = EnumShape.None;
        }

        public Tile()
        {
            this.IsPlaceholder = true;
            this.Colour = EnumColour.None;
            this.Shape = EnumShape.None;
        }

        

        public override string ToString()
        {
            try
            {
                return String.Concat(Colour, ":", Shape);
            }
            catch (NullReferenceException)
            {
                return "No colour/shape!";
            }
        }

        public static bool IsDuplicate(Tile t1, Tile t2, bool verbose = false)
        {

            if ((t1.Colour == t2.Colour) && (t1.Shape == t2.Shape))
            {
                if (verbose == true)
                    Console.WriteLine($"{t1} is the same as {t2}");
                return true;
            }
            else
            {
                if (verbose == true)
                    Console.WriteLine($"{t1} is different to {t2}");
                return false;
            }
        }

        public static bool ColourMatch(Tile t1, Tile t2)
        {
            if ((t1.Colour == t2.Colour) && (t1.Shape != t2.Shape))
                return true;
            else
                return false;
        }

        public static bool ShapeMatch(Tile t1, Tile t2)
        {
            if ((t1.Shape == t2.Shape) && (t1.Colour != t2.Colour))
                return true;
            else
                return false;
        }

        public static int AxisChecker(Tile tile1, Tile tile2)
        {
            var (t1Row, t1Column) = tile1.GridXref;
            var (t2Row, t2Column) = tile2.GridXref;

            if (t1Row == t2Row && t1Column != t2Column)
                return 1; //horizontal
            else if (t1Column == t2Column && t1Row != t2Row)
                return 2; //vertical
            else
                return 0; //no axis match
        }

        public static bool AxisMatch(Tile tile1, Tile tile2, int axis)
        {
            var (t1Row, t1Column) = tile1.GridXref;
            var (t2Row, t2Column) = tile2.GridXref;

            switch (axis)
            {
                case 1: //horizontal
                    if (t1Row == t2Row)
                        return true;
                    else
                        return false;
                case 2: //vertical
                    if (t1Column == t2Column)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }

        public static bool AxisMatch(int candidateRow, int candidateColumn, Tile existingTile, int axis)
        {
            var (existingRow, existingColumn) = existingTile.GridXref;

            switch (axis)
            {
                case 1: //horizontal
                    if (candidateRow == existingRow)
                        return true;
                    else
                        return false;
                case 2: //vertical
                    if (candidateColumn == existingColumn)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }

        public static bool TouchingChecker(int candidateRow, int candidateColumn, Tile existingTile)
        {
            var (existingRow, existingColumn) = existingTile.GridXref;
            if (candidateRow == existingRow & candidateColumn == existingColumn)
                return false; //this is a bad placement, the new tile location is identical to the existing tile location;
            else if (candidateRow == existingRow)
            {
                if (candidateColumn - existingColumn == 1 || existingColumn - candidateColumn == 1)
                    return true; //they're 1 grid position away from each other, so are touching;
                else
                    return false; //they're more than 1 grid position away from each other;
            }
            else if (candidateColumn == existingColumn)
            {
                if (candidateRow - existingRow == 1 || existingRow - candidateRow == 1)
                    return true; //they're 1 grid position away from each other, so are touching;
                else
                    return false; //they're more than 1 grid position away from each other;
            }
            else
                return false; //they share no grid refs in common, and so are not touching;
        }
    }
}
