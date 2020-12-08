using System;
using System.Linq;

namespace Qwirkle_WPF
{
    public enum EnumColour
    {
        Red,
        Orange,
        Blue,
        Green,
        Yellow,
        Purple,
        None
    }

    public enum EnumShape
    {
        Square,
        Circle,
        Bang,
        Star,
        Club,
        Diamond,
        None
    }

    public class MainClass
    {
        static readonly int playerCount = 3;
        static readonly int colourCount = 6;
        static readonly int shapeCount = 6;

        public readonly static int startingTileCount = 6; //the number of tiles that each player starts with
        private readonly static int duplicateTileCount = 3; //how many duplicates of each tile are in the bag;



        public static int GridRows { get; } = 10;
        public static int GridColumns { get; } = 10;

        
        static void MainConsole()
        {
            Menu.InitConsole();
            
            
            Bag.FillTheBag(
                 colourCount,
                 shapeCount,
                 duplicateTileCount);

            //Bag.ListTilesInBag();

            Console.WriteLine($"There are currently {Bag.tilesInBag.Count} tiles in the bag");

            InitializePlayers(playerCount); //creates the players, and draws their starting tiles

            foreach (Player p in Game.ListOfPlayers)
            {
                p.ListTiles();
                //p.MaxPlaceableTiles(out int highestCount);
                //Console.WriteLine($"{p.Name} can place {highestCount}");
            }

            Game.DeterminePlayerOrder();
            RunTheGame();
        }


        public static void InitializePlayers(int players)
        {
            
            if (players > 0)
            {
                for (int loop = 0; loop < players; loop++)
                {
                    Game.ListOfPlayers.Add(new Human());
                }
            }
            else
            {
                Console.WriteLine("Must have at least 1 player.");
            }
        }

        static void RunTheGame()
        {
            //Tile tempClub = new Tile((int)EnumColour.Blue, (int)EnumShape.Club);
            //Grid.PlaceTile(tempClub, 5, 5);

            bool continuePlaying = true;
            while (continuePlaying)
            {
                continuePlaying = StartTurn();
            }
        }

        static bool StartTurn()
        {
            bool endOfTurn;
            bool validityCheck;

            do
            {
                Console.WriteLine($"{Game.CurrentPlayer}'s turn");
                Menu.Turn();
                string uncheckedInput = Console.ReadLine();
                validityCheck = Int32.TryParse(uncheckedInput, out int userInput);
                if (validityCheck == false)
                {
                    Console.WriteLine("invalid input");
                    userInput = 999;
                }

                switch (userInput)
                {
                    case 1:
                        Console.WriteLine(Game.CurrentPlayer);
                        endOfTurn = Menu.MakeMove(Game.CurrentPlayer);
                        Menu.DrawGrid();
                        endOfTurn = true;
                        break;
                    case 2:
                        endOfTurn = SwapTiles(Game.CurrentPlayer);
                        break;
                    case 3:
                        Console.Clear();
                        Game.CurrentPlayer.ListTiles();
                        endOfTurn = false;
                        break;
                    case 4:
                        endOfTurn = true;
                        return false;
                    case 5:
                        Menu.DrawGrid();
                        endOfTurn = false; ;
                        break;
                    case 6:
                        endOfTurn = true;
                        break;
                    case 7:
                        Menu.PrintScores();
                        endOfTurn = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input!");
                        endOfTurn = false;
                        break;
                }
            } while (endOfTurn == false);

            Console.Clear();
            Game.EndTurn();

            return true;
        }

        public static bool SwapTiles(Player player)
        { 
                player.ListTiles();

                Console.WriteLine($"Which tile do you want to return to the bag?");
                string input = Console.ReadLine();

                try
                {
                    Tile tileToReturn = player.tilesInHand[int.Parse(input)];
                    player.returnedTiles.Add(tileToReturn);
                    player.tilesInHand.Remove(tileToReturn);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid tile index");
                }

            if (player.tilesInHand.Count() == 0)
                Console.WriteLine("That was the last tile");
            else if (player.tilesInHand.Count() > 0 && player.returnedTiles.Count() > 0)
            {
                Console.WriteLine("Do you wish to return another tile?");
                string keepGoing = Console.ReadLine();
                if (keepGoing == "y")
                    SwapTiles(player);
            }
            else if (player.returnedTiles.Count() == 0)
                return false;
            
            // start the returning

            /*
            //list the tiles to return
            foreach (Tile tile in player.returnedTiles)
            {
                Console.WriteLine($"Returning: {tile.Colour}:{tile.Shape}");
            }*/

            player.GetTilesFromBag(player.returnedTiles.Count()); //get the new tiles from the bag BEFORE returning the old ones.

            for (int tileIndex = MainClass.startingTileCount - player.returnedTiles.Count(); tileIndex < MainClass.startingTileCount; tileIndex++)
            {
                Console.WriteLine($"New Tile{tileIndex}: {player.tilesInHand[tileIndex]}");
            }

            foreach (Tile tile in player.returnedTiles.ToList())
            {
                player.ReturnTileToBag(tile);
            }

            //Console.WriteLine("finished returning");
            return true;
        }







        bool CheckEndOfGame()
        {
            bool EndOfGame = false;
            return EndOfGame;
        }




    }
}

/*
Produce an object for each tile

allow players to randomly move numbers of those tiles into their hand

track tiles in players hand

allow players to move tiles from their hand, to a valid position in the board

Interrogate tiles to the left, right, up and down of the newly placed tile.
confirm plane only contains 1 of:
1. The same colour, but different icons
2. The same icon, but different colours

calculate turn score
horizontal
vertical
check for qwirkle

announce when no tiles left in bag

end game when a player plays all their tiles

allow players to swap specific tiles in their hand, for replacements from the bag

*/
