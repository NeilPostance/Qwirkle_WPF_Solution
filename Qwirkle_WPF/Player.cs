using System;
using System.Collections.Generic;
using System.Linq;

namespace Qwirkle_WPF
{
    public class Player
    {
        private static int? count; // incrementing value for playerId

        public int Id { get; } // instance value for playerId
        public string Name { get; }

        private int score = 0;

        public int Score 
        { get
            {
                return score;
            }
            set
            {
                if (value >= score)
                    score = value;
                else
                    return;
            }
        }

        public List<Tile> tilesInHand = new List<Tile>(); // Tiles in the players hand

        public List<Tile> placedTiles = new List<Tile>(); // Tiles the player has placed on the current turn so that we can calculate the score later

        public List<Tile> returnedTiles = new List<Tile>(); // Tiles the player has returned this turn, so that they don't draw the same times this go.



        // Constructor
        public Player(string playerName)
        {
            if (count == null)
            {
                count = 1;
                //Console.WriteLine("Count is NULL");
            }
            else
            {
                count++;
            }
            Id = (int)count;
            this.Name = playerName;
            Console.WriteLine($"Player {this.Name} with ID: {this.Id} created");
            this.GetTilesFromBag(MainClass.startingTileCount);
        }

        public Player()
        {
            if (count == null)
            {
                count = 1;
                //Console.WriteLine("Count is NULL");
            }
            else
            {
                count++;
            }
            Id = (int)count;
            this.Name = $"Player{Id}";

            Console.WriteLine($"Player {this.Name} with ID: {this.Id} created");
            this.GetTilesFromBag(MainClass.startingTileCount);
        }

        public Player(bool getStartingTiles)
        {
            if (count == null)
            {
                count = 1;
                //Console.WriteLine("Count is NULL");
            }
            else
            {
                count++;
            }
            Id = (int)count;
            this.Name = $"Player{Id}";

            Console.WriteLine($"Player {this.Name} with ID: {this.Id} created");
            if (getStartingTiles)
                this.GetTilesFromBag(MainClass.startingTileCount);
        }

        public override string ToString()
        {
            return this.Name.ToString();
        }

        public static int GetPlayerCount()
        {
            return (int)count;
        }

        public bool GetTilesFromBag(int tileCount)
        {
            if (tileCount < 1)
            {
                return false;
            }
            else
            {
                while (tileCount > 0)
                {
                    //pick a random tile
                    int indexOfTileToMove = RandomGenerator.RandomNumber(0, Bag.tilesInBag.Count());
                    //Console.WriteLine($"index to move {indexOfTileToMove}");
                    try
                    {
                        Tile tileToMove = Bag.tilesInBag[indexOfTileToMove];
                        tilesInHand.Add(tileToMove);

                        //remove the tile from the bag
                        Bag.tilesInBag.Remove(tileToMove);
                        //remove a random tile from the bag
                        //add the tile to the players hand

                        tileCount--;
                    }
                    catch (ArgumentOutOfRangeException)
                    { 
                        Console.WriteLine("Not enough tiles in the bag.");
                        Game.SetLastRound();
                        return false;
                    }
                    
                    //add to hand
                    //Console.WriteLine($"Adding tile with {tileToMove.Colour} {tileToMove.Shape} to hand.");

                    
                }
                Console.WriteLine($"There are currently {Bag.tilesInBag.Count} tiles in the bag");
                return true;
            }
        }

        public void ReturnTileToBag(Tile tile)
        {
            try
            {
                this.returnedTiles.Remove(tile); //remove the specified tile from the players hand
                Console.WriteLine($"{tile} has been removed from {Name}'s hand");

                Bag.tilesInBag.Add(tile); //put the tile into the bag
                Console.WriteLine($"{tile} has been returned to the bag");

            }
            catch (Exception)
            {
                tilesInHand.Add(tile);
            }
        }

        public void ListTiles()
        {
            Console.WriteLine($"{this.Name} has the following tiles ");
            ListTilesInHand();
            ListPlacedTiles();
        }

        public void ListTilesInHand()
        {
            Console.WriteLine($"In hand:");
            foreach (Tile t in tilesInHand)
            {
                Console.Write($"# {tilesInHand.IndexOf(t)} : {t} ");
                Menu.DrawTile(t);
                Console.WriteLine();
            }
            Console.WriteLine("---------------------------");
        }

        public void ListPlacedTiles()
        {
            Console.WriteLine($"Placed this turn:");
            placedTiles.ForEach(delegate (Tile tile)
            {
                Console.Write($"# {placedTiles.IndexOf(tile)} : {tile} placed {tile.GridXref} ");
                Menu.DrawTile(tile);
                Console.WriteLine();
            });
            Console.WriteLine("---------------------------");

        }

        public void MaxPlaceableTiles(out int highestCount, bool verbose = false)
        {
            highestCount = 0;
            int maxFoundColourCount = 0;
            int maxFoundShapeCount = 0;
            EnumColour maxFoundColour = EnumColour.Blue;
            EnumShape maxFoundShape = EnumShape.Square;

            //dedup the hand first
            List<Tile> dedupHand = new List<Tile>();
            foreach (Tile candidateTile in tilesInHand)
            {
                if (dedupHand.Count == 0)
                {
                    dedupHand.Add(candidateTile);
                    if (verbose)
                        Console.WriteLine($"---------First Tile---------\n Adding {candidateTile} as it's unique.");
                }
                else
                {
                    bool duplicate = false;
                    foreach (Tile dedupTile in dedupHand)
                    {
                        if (Tile.IsDuplicate(candidateTile, dedupTile))
                            duplicate = true;
                    }
                    if (duplicate)
                        Console.WriteLine($"{this.Name}: Not considering {candidateTile} as it's a duplicate.");
                    else
                    {
                        dedupHand.Add(candidateTile); //if it's not a duplicate, add it to the deduplicated list
                        if (verbose)
                            Console.WriteLine($"Adding {candidateTile} as it's unique.");
                    }
                }
                if (verbose)
                    Console.WriteLine("--");
            }
            Console.WriteLine($"The dedup list contains {dedupHand.Count} tiles for {this.Name}");

            //now that the hand is deduplicated, let's check for colour matches
            foreach (EnumColour tempColour in Enum.GetValues(typeof(EnumColour))) //check for colour matches first
            {
                List<Tile> foundTiles = dedupHand.FindAll(i => i.Colour == tempColour); //find all the tiles in the hand which are of the given colour
                if (foundTiles.Count() > maxFoundColourCount)
                {
                    maxFoundColourCount = foundTiles.Count();
                    maxFoundColour = tempColour;
                }
            }

            //now shape matches
            foreach (EnumShape tempShape in Enum.GetValues(typeof(EnumShape))) //check for Shape matches
            {
                List<Tile> foundTiles = dedupHand.FindAll(i => i.Shape == tempShape); //find all the tiles in the hand which are of the given Shape
                if (foundTiles.Count() > maxFoundShapeCount)
                {
                    maxFoundShapeCount = foundTiles.Count();
                    maxFoundShape = tempShape;
                }
            }

            highestCount = Math.Max(maxFoundColourCount, maxFoundShapeCount);
            dedupHand.Clear();

        }

        public bool PlaceTile(int tileIndex, int row, int column) //Take a tile from a players hand, and place it in a position on the board.
        {
            if (tileIndex > tilesInHand.Count)
                return false;
            Tile tile = this.tilesInHand[tileIndex];
            Grid.PlaceTile(tile, row, column); //place the tile in the grid
            this.placedTiles.Add(tile); //add it to the list of tiles placed this turn
            this.tilesInHand.Remove(tile); //remove the tile from the current hand

            return true;
        }

        public bool PlaceTile(Tile tileToPlace, int row, int column) //Take a tile from a players hand, and place it in a position on the board.
        {
            if (tilesInHand.Contains(tileToPlace) && Game.IsValidMove(tileToPlace,row,column,Game.CurrentPlayer) == true)
            {
                Grid.PlaceTile(tileToPlace, row, column); //place the tile in the grid
                this.placedTiles.Add(tileToPlace); //add it to the list of tiles placed this turn
                this.tilesInHand.Remove(tileToPlace); //remove the tile from the current hand
                return true;
            }
            else // A player mustn't be able to place a tile which isn't in their hand, or will be placed in an invalid position.
            {
                return false;
            }
        }

        public void AddScore(int verticalScore, int horizontalScore)
        {
            if (this.placedTiles.Count() == 0)
                return;
            else
            {
                this.score += (verticalScore + horizontalScore);
                this.placedTiles.Clear(); //clear the placed tiles here to reduce the chance of adding the same score more than once
            }
            
        }

        public void AddScore(int turnScore)
        {
            if (this.placedTiles.Count() == 0)
                return;
            else
            {
                this.score += turnScore;
                this.placedTiles.Clear(); //clear the placed tiles to reduce the chance of adding the same score more than once
            }

        }

        public int GetScore()
        {
            return this.score;
        }

        public char GetPlacementDirection()
        {
            if (placedTiles.Count > 1)
            {
                var (r1, c1) = placedTiles[0].GridXref;
                var (r2, c2) = placedTiles[1].GridXref;
                if (r1 == r2)
                    return 'R';
                else if (c1 == c2)
                    return 'C';
                else
                    return 'X';
            }
            else
            {
                return 'R';
            }
        }

        public void RefillHand()
        {
            if (this.tilesInHand.Count < MainClass.startingTileCount)
            {
                int tilesToGet = MainClass.startingTileCount - this.tilesInHand.Count;
                GetTilesFromBag(tilesToGet);
                Console.WriteLine($"Refilled bag with {tilesToGet} tiles.");
            }
            else
                Console.WriteLine($"The hand is full, no need to get any additional tiles.");
        }
    }
}
