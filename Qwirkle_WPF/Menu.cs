using System;
using System.Linq;
using System.Text;

namespace Qwirkle_WPF
{
    public static class Menu
    {
        public static void InitConsole()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public static void DrawGrid()
        {
            Console.Clear();
            for (int row = 0; row < Grid.tileGrid.GetLength(0); row += 1) 
            {
                for (int column = 0; column < Grid.tileGrid.GetLength(1); column += 1) 
                {
                    if (Grid.tileGrid[row, column] == null)
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        DrawTile(Grid.tileGrid[row, column]);

                    }

                    if (column >= Grid.tileGrid.GetLength(1) - 1)
                        Console.Write("\n");
                }
            }
        }

        public static void DrawGrid(int rows, int columns)
        {
            Console.Clear();
            for (int row = 0; row < rows; row += 1)
            {
                for (int column = 0; column < columns; column += 1)
                {
                    if (Grid.tileGrid[row, column] == null)
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        DrawTile(Grid.tileGrid[row, column]);

                    }

                    if (column >= Grid.tileGrid.GetLength(1) - 1)
                        Console.Write("\n");
                }
            }
        }



        public static void DrawTile(Tile tile)
        {
            string colour = tile.Colour.ToString();
            string shape = tile.Shape.ToString();
            
            char icon;
            switch (shape)
            {
                case "Square":
                    //icon = '\u25a1';
                    icon = '\u25a1';
                    break;
                case "Circle":
                    icon = 'O';
                    break;
                case "Bang":
                    icon = '+';
                    break;
                case "Star":
                    icon = '*';
                    break;
                case "Diamond":
                    icon = '<';
                    break;
                case "Club":
                    icon = '&';
                    break;
                default:
                    icon = '*';
                    break;
            }

            //ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            // Save the current background and foreground colors.
            ConsoleColor currentForeground = Console.ForegroundColor;

            Console.ForegroundColor = ToConsoleColor(colour);
            Console.Write(icon);
            Console.ForegroundColor = currentForeground;
        }

        static ConsoleColor ToConsoleColor(string colour)
        {
            switch (colour)
            {
                case "Red":
                    return ConsoleColor.Red;
                case "Orange":
                    return ConsoleColor.DarkYellow;
                case "Blue":
                    return ConsoleColor.Blue;
                case "Green":
                    return ConsoleColor.Green;
                case "Yellow":
                    return ConsoleColor.Yellow;
                case "Purple":
                    return ConsoleColor.Magenta;
                default:
                    return ConsoleColor.White;
            }
        }
        public static void Turn()
        {
            Console.WriteLine($"1 = Make a move;");
            Console.WriteLine($"2 = Swap Tiles");
            Console.WriteLine($"3 = Display Hand");
            Console.WriteLine($"4 = Exit");
            Console.WriteLine($"5 = Print Grid");
            Console.WriteLine($"6 = End Turn");
            Console.WriteLine($"7 = Show Scores");
        }

        public static void PrintScores()
        {
            Console.Clear();
            Console.WriteLine("The Current Scores are:");
            foreach (Player player in Game.ListOfOrderedPlayers)
            {
                Console.WriteLine($"{player} has {player.GetScore()}");
            }
            Console.WriteLine("-----------------------");
        }

        public static bool MakeMove(Player currentPlayer)
        {
            //loop while endOfMove isn't true
            // return true if a move has been made, false if it hasn't.

            bool endOfMove;
            Console.Clear();
            if (currentPlayer.tilesInHand.Count() == 0)
            {
                Console.WriteLine("No tiles remaining in hand! \n Press any key to continue.");
                Console.Read();
                return true;
            }

            DrawGrid();
            currentPlayer.ListTiles();

            bool gotAcceptableInput = false;
            int tileHandIndex = 999;
            int xRef = 999;
            int yRef = 999;

            while (gotAcceptableInput == false)
            {
                Console.WriteLine($"Which tile do you wish to place?");
                bool validityCheck = Int32.TryParse(Console.ReadLine(), out tileHandIndex);
                if (validityCheck == false)
                    gotAcceptableInput = false;
                else if (tileHandIndex < currentPlayer.tilesInHand.Count())
                    gotAcceptableInput = true;
            }

            bool positionIsEmpty;
            do
            {
                gotAcceptableInput = false;
                while (gotAcceptableInput == false)
                {
                    Console.WriteLine($"Where do you wish to place it: X");
                    bool validityCheck = Int32.TryParse(Console.ReadLine(), out xRef);
                    if (validityCheck == false)
                        gotAcceptableInput = false;
                    else if (xRef < MainClass.GridRows)
                        gotAcceptableInput = true;
                }

                gotAcceptableInput = false;
                while (gotAcceptableInput == false)
                {
                    Console.WriteLine($"Where do you wish to place it: Y");
                    bool validityCheck = Int32.TryParse(Console.ReadLine(), out yRef);
                    if (validityCheck == false)
                        gotAcceptableInput = false;
                    else if (yRef < MainClass.GridColumns)
                        gotAcceptableInput = true;
                }

                

                positionIsEmpty = Grid.IsEmptyPosition(xRef, yRef);
                if (!positionIsEmpty)
                    Console.WriteLine("That position has already been taken, please specify another location");

            } while (positionIsEmpty == false);

            //validate the position - is it empty?
            //validate the position - is it valid re: neighboring tiles?
            //Console.WriteLine($"Placing {currentPlayer.tilesInHand[n]}");
            if (Game.IsValidMove(currentPlayer.tilesInHand[tileHandIndex], xRef, yRef, currentPlayer))
            {
                currentPlayer.PlaceTile(tileHandIndex, xRef, yRef);
            }
            else
            {
                Console.WriteLine("That move is invalid, try another.");
            }
           

            gotAcceptableInput = false;
            bool keepGoing = false;
            while (gotAcceptableInput == false)
            {
                Console.WriteLine($"Do you wish to place another tile?");
                string inputString = Console.ReadLine();
                switch (inputString.ToUpper())
                {
                    case "YES":
                        gotAcceptableInput = true;
                        keepGoing = true;
                        break;
                    case "Y":
                        gotAcceptableInput = true;
                        keepGoing = true;
                        break;
                    case "TRUE":
                        gotAcceptableInput = true;
                        keepGoing = true;
                        break;
                    case "NO":
                        gotAcceptableInput = true;
                        keepGoing = false;
                        break;
                    case "N":
                        gotAcceptableInput = true;
                        keepGoing = false;
                        break;
                    case "FALSE":
                        gotAcceptableInput = true;
                        keepGoing = false;
                        break;
                    default:
                        gotAcceptableInput = false;
                        Console.WriteLine("Invalid input");
                        break;
                }
            }

            if (keepGoing == true)
                endOfMove = false;
            else
                endOfMove = true;
                


            if (endOfMove == false)
                MakeMove(currentPlayer); //the player wishes to continue to move, and there are tiles remaining.
            

            if (endOfMove == true && (currentPlayer.tilesInHand.Count() < MainClass.startingTileCount))
                return true; //Some tiles have been placed, and the move is over.
            else 
                return false; //no tiles have been placed, but the player no longer wishes to move.
           
        }
    }
}
