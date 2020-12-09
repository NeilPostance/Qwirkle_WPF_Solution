using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Qwirkle_WPF
{
    public static class Game
    {
        public static List<Player> ListOfPlayers { get; set; } = new List<Player>(); // a list of created players
        public static List<Player> ListOfOrderedPlayers { get; set; } = new List<Player>(); // a list of created players in order of play

        public static Player CurrentPlayer { get; set; }

        private static bool LastRound { get; set; } = false;
        
        
        public static void EndTurn()
        {
            
            int turnScore = CalculateTurnScore(CurrentPlayer);
            Console.WriteLine($"{CurrentPlayer} has scored {turnScore} this turn.");
            CurrentPlayer.AddScore(turnScore);
            CurrentPlayer.RefillHand();
            NextPlayer();
        }

        public static void ResetGame()
        {
            ListOfPlayers.Clear();
            ListOfOrderedPlayers.Clear();
            Grid.Empty();
            Bag.EmptyTheBag();
        }


        public static void DeterminePlayerOrder()
        {
            // determine the player order, and store in playerOrder[]
            ListOfOrderedPlayers.Clear(); //empty out the ordered player list so it doesn't just get added to.
            Player firstPlayer = Game.ListOfPlayers[0];
            int mostMoves = 0;

            foreach (Player p in Game.ListOfPlayers)
            {
                p.MaxPlaceableTiles(out int highestCount);
                Console.WriteLine($"{p} can place {highestCount} tiles");

                if (highestCount > mostMoves)
                {
                    firstPlayer = p;
                    mostMoves = highestCount;
                }
            }

            Console.WriteLine($"{firstPlayer} goes first with {mostMoves} tiles");

            int startingPlayerIndex = Game.ListOfPlayers.IndexOf(firstPlayer);
            Game.ListOfOrderedPlayers.Add(firstPlayer);
            int currentPlayerIndex = startingPlayerIndex + 1;
            while (Game.ListOfOrderedPlayers.Count() < Game.ListOfPlayers.Count())
            {

                if (currentPlayerIndex >= Game.ListOfPlayers.Count())
                {
                    currentPlayerIndex = 0;
                    Game.ListOfOrderedPlayers.Add(Game.ListOfPlayers[currentPlayerIndex]);
                }
                else
                {
                    Game.ListOfOrderedPlayers.Add(Game.ListOfPlayers[currentPlayerIndex]);
                }
                currentPlayerIndex++;

            }

            Console.WriteLine("This playing order is as follows:");
            foreach (Player p in Game.ListOfOrderedPlayers)
            {
                Console.WriteLine(p);
            }

            Game.CurrentPlayer = firstPlayer;

        }

        private static int CurrentPlayerIndex()
        {
            //Console.WriteLine($"The current player index is {ListOfOrderedPlayers.IndexOf(CurrentPlayer)}");
            return ListOfOrderedPlayers.IndexOf(CurrentPlayer);
        }

        public static void NextPlayer()
        {
            //Console.WriteLine($"The current player index is {ListOfOrderedPlayers.IndexOf(CurrentPlayer)} out of {ListOfOrderedPlayers.Count} and it's about to be incremented");
            if (CurrentPlayerIndex() < ListOfOrderedPlayers.Count - 1)
            {
                CurrentPlayer = ListOfOrderedPlayers[CurrentPlayerIndex() + 1];//if there's another player in the list after them, set them as the current player

            }
            else
                CurrentPlayer = ListOfOrderedPlayers[0];
            Console.WriteLine($"The current player index is now {CurrentPlayer}");
        }

        public static bool IsValidMove(Tile newTile, int startingRow, int startingColumn, Player currentPlayer)
        {


            bool hasNearbyTiles = false;
            int distance = 1;
            EnumDirection direction = EnumDirection.Left;
            int matchType = 0; //0: no matchtype, 1:Colour, 2:Shape
            bool invalidMove = false;
            if (!Grid.IsEmptyPosition(startingRow, startingColumn))
            {
                Console.WriteLine($"A Tile already exists at {startingRow}:{startingColumn}");
                return false;
            }

            if (Grid.IsEmpty())
                return true; //if there are no existing tiles in the grid, you can put any tile down.

            do
            {
                //Is there a nearbyTile?
                bool nearbyTileExists = Grid.QueryNearbyLocation(startingRow, startingColumn, direction, distance, out Tile nearbyTile);
                if (nearbyTileExists)
                {
                    hasNearbyTiles = true;
                    // Is the nearbyTile identical to the newTile?
                    if (Tile.IsDuplicate(nearbyTile, newTile))
                        return false; //if a nearby tile is identical , the move is invalid.

                    // If the player has already placed tiles this turn, there are some things we need to check;
                    if (currentPlayer.placedTiles.Count() > 0)
                    {
                        int i = 0;
                        // is the new tile touching a tile previously placed this turn?
                        foreach (Tile existingTile in currentPlayer.placedTiles)
                        {
                            if (Tile.TouchingChecker(startingRow, startingColumn, existingTile) == false && i >= currentPlayer.placedTiles.Count() - 1)
                            {
                                Console.WriteLine("All tiles placed in a turn must touch each other.");
                                return false; // all tiles placed in a turn must touch each other;
                            }
                            else if (Tile.TouchingChecker(startingRow, startingColumn, existingTile) == false && i < currentPlayer.placedTiles.Count() - 1)
                            {
                                Console.WriteLine($"{newTile}@{startingRow},{startingColumn} does not touch {existingTile.GridXref}, but there are more placed tiles to check against.");

                            }
                            i++;

                        }
                        Console.WriteLine("The new tile DOES touch a previously placed tile");

                        // in addition to touching each other, they must all be placed along the same axis;
                        if (currentPlayer.placedTiles.Count() > 1)
                        {
                            EnumAxis existingAxis = Tile.AxisChecker(currentPlayer.placedTiles[0], currentPlayer.placedTiles[1]);
                            /*
                                0 = no axis match (shouldn't be possible)
                                1 = horizontal axis match
                                2 = vertical axis match
                            */
                            if (existingAxis == EnumAxis.None)
                            {
                                Console.WriteLine("Somehow there are placed tiles which do not share an axis");
                                return false;
                            }

                            foreach (Tile existingTile in currentPlayer.placedTiles)
                            {
                                if (Tile.AxisMatch(startingRow, startingColumn, existingTile, existingAxis) == false)
                                    return false;
                            }
                            Console.WriteLine("The new tile DOES share an axis with a previously placed tile");

                        }
                    }
                    else
                    {
                        Console.WriteLine("Skipping nearby tile checks, as this is the first tile placed this turn");
                    }



                    // Do I have a current matchType?
                    switch (matchType)
                    {
                        case 0: //no current matchtype. can match either colour OR shape
                            if (Tile.ColourMatch(nearbyTile, newTile))
                            {
                                Console.WriteLine($"New Tile ({newTile}) Matches existing tile ({nearbyTile} on Colour.");
                                matchType = 1;
                                distance++;
                                continue;
                            }
                            else if (Tile.ShapeMatch(nearbyTile, newTile))
                            {
                                Console.WriteLine($"New Tile ({newTile}) Matches existing tile ({nearbyTile} on Shape.");
                                matchType = 2;
                                distance++;
                                continue;
                            }
                            else
                            {
                                Console.WriteLine($"New Tile ({newTile}) Matches existing tile ({nearbyTile} on Neither!.");
                                return false;
                            }
                        case 1: //should only match colour
                            if (Tile.ColourMatch(nearbyTile, newTile) == true && Tile.ShapeMatch(nearbyTile, newTile) == false) //only matches colour, doesn't match shape
                            {
                                Console.WriteLine($"New Tile ({newTile}) Matches existing tile ({nearbyTile} on Colour. (case2)");
                                distance++; //proceeed to checking the next tile;
                                continue;
                            }
                            else //if the above check failed, then the new tile is of an invalid type, so reject the move.
                            {
                                Console.WriteLine($"New Tile ({newTile}) Does not match existing tile ({nearbyTile} on Colour. (case2)");
                                invalidMove = true;
                                continue;
                            }
                        case 2: //should only match shape
                            if (Tile.ColourMatch(nearbyTile, newTile) == false && Tile.ShapeMatch(nearbyTile, newTile) == true) //only matches shape, doesn't match colour
                            {
                                Console.WriteLine($"New Tile ({newTile}) Matches existing tile ({nearbyTile} on Shape. (case3)");
                                distance++; //proceeed to checking the next tile;
                                continue;
                            }
                            else //if the above check failed, then the new tile is of an invalid type, so reject the move.
                            {
                                Console.WriteLine($"New Tile ({newTile}) does not match existing tile ({nearbyTile} on Shape. (case3)");
                                invalidMove = true;
                                continue;
                            }
                    }
                    // Is the nearbyTile a Colour Match for newTile?
                    // Is the nearbyTile a Shape Match for newTile?
                }
                else
                {
                    Console.WriteLine($"No more tiles found {direction}");
                    switch (direction)
                    {
                        case EnumDirection.Right:
                            direction = EnumDirection.Up;
                            distance = 1; //reset the distance, because we're now starting with a new direction
                            matchType = 0; //the next direction is up, we're therefore changing plane and so we need to reset the match type.
                            continue;
                        case EnumDirection.Down: // If we've got here, we must've checked all directions without being told it's invalid.
                            if (hasNearbyTiles)
                            {
                                Console.WriteLine("That move is valid");
                                return true; //the new tile must be touching existing tiles, and not declared invalid, so it must be valid.
                            }
                            else
                                return false; //the new tile doesn't touch any existing tiles, but existing tiles exist on the board. new tiles MUST touch existing tiles.
                        default:
                            direction++; //go to the next direction
                            distance = 1; //reset the distance
                            continue; //start the loop again.
                    }
                }

            } while (invalidMove == false);

            return false;


        }


        public static int CalculateTurnScore(Player player)
        {
            int tempScore;
            if (player.placedTiles.Count > 0)
            {

                //calculate the placed tile direction first
                switch (player.GetPlacementDirection())
                {
                    case 'R':
                        Console.WriteLine($"The tiles has been placed along a row. Calculating that direction first.");
                        int caseRRowScore = CountScoringTilesInLine(player.placedTiles[0], EnumAxis.Horizontal);
                        if (caseRRowScore == 6)
                            caseRRowScore += 6;
                        int caseRColumnScore = CountScoringTilesPerpendicular(player, EnumAxis.Vertical);
                        tempScore = caseRRowScore + caseRColumnScore;
                        break;
                    case 'C':
                        Console.WriteLine($"The tiles has been placed along a column. Calculating that direction first.");
                        int caseCColumnScore = CountScoringTilesInLine(player.placedTiles[0], EnumAxis.Vertical);
                        if (caseCColumnScore == 6)
                            caseCColumnScore += 6;
                        int caseCRowScore = CountScoringTilesPerpendicular(player, EnumAxis.Horizontal);
                        tempScore = caseCRowScore + caseCColumnScore;
                        break;
                    default:
                        return 0;
                }
            }
            else
            {
                tempScore = 0;
            }
            

            Console.WriteLine($"Score = {tempScore}");
            return tempScore;
        }

        public static int CountScoringTilesInLine(Tile tile, EnumAxis direction)
        {
            int tileCount = 0;

            var (startingR, startingC) = tile.GridXref;

            switch (direction)
            {
                case EnumAxis.Horizontal:
                    for (int currentC = startingC - 1; currentC >= 0; currentC--) //count left
                    {
                        if (Grid.IsEmptyPosition(startingR, currentC))
                            break;
                        else
                            tileCount++;
                    }
                    for (int currentC = startingC + 1; currentC <= MainClass.GridColumns; currentC++) //count right
                    {
                        if (Grid.IsEmptyPosition(startingR, currentC))
                            break;
                        else
                            tileCount++;
                    }
                    tileCount++; //count self
                    return tileCount;
                case EnumAxis.Vertical: //column-wise
                    for (int currentR = startingR - 1; currentR >= 0; currentR--) // count up
                    {
                        if (Grid.IsEmptyPosition(currentR, startingC))
                            break;
                        else
                            tileCount++;
                    }
                    for (int currentR = startingR + 1; currentR <= MainClass.GridRows; currentR++) //count down
                    {
                        if (Grid.IsEmptyPosition(currentR, startingC))
                            break;
                        else
                            tileCount++;
                    }
                    tileCount++; //count self
                    return tileCount;
                default:
                    return 0;
            }
           
        }

        public static int CountScoringTilesPerpendicular(Player player, EnumAxis direction)
        {
            int perpendicularScore = 0;
            //for this one, we don't include the actual tile itself in the score, unless the perpendicular score is greater than 1

            foreach (Tile tile in player.placedTiles)
            {

                //get the position of the tile, and look in the specified direction to see how many tiles it touches. If it doesn't touch any, the perpendicular score for that tile is 0;
                int tilePerpendicularScore = CountScoringTilesInLine(tile, direction);
                if (tilePerpendicularScore > 1 && tilePerpendicularScore < 6)
                    perpendicularScore += tilePerpendicularScore;
                else if (tilePerpendicularScore == 6)
                    perpendicularScore += 12;
                else
                    continue;
            }

            return perpendicularScore;
        }

        public static void SetLastRound()
        {
            LastRound = true;
            Console.WriteLine("It's the last round!");
        }
    }
}
