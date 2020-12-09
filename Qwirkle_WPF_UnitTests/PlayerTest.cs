using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qwirkle_WPF;

namespace Qwirkle_WPF_Tests
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void CreatePlayers_Default()
        {
            Game.ResetGame();
            Player p1 = new Player();
            int playerCount = Player.GetPlayerCount();
            Assert.AreEqual($"Player{playerCount}", p1.Name);
        }

        [TestMethod]
        public void CreatePlayers_CustomName()
        {
            Game.ResetGame();
            Player p1 = new Player("Jim");
            Assert.AreEqual("Jim", p1.Name);
        }

        [TestMethod]
        public void CreatePlayers_WithdrawStartingTiles()
        {
            Game.ResetGame();
            Bag.FillTheBag(2,2,2);
            Player p1 = new Player(true);
            Assert.AreEqual(MainClass.startingTileCount, p1.tilesInHand.Count);
        }

        [TestMethod]
        public void GetTilesFromBag()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 1, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(1);

            string expectedPlayerHand = @"In hand:
# 0 : Red:Square □
---------------------------
";
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            player.ListTilesInHand();

            Assert.AreEqual(expectedPlayerHand, sw.ToString());
        }

        [TestMethod]
        public void ListTiles()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 1, 2);
            Player player = new Player("Player");
            player.GetTilesFromBag(2);
            player.PlaceTile(0, 1, 1);

            string expectedTileList = @"Player has the following tiles 
In hand:
# 0 : Red:Square □
---------------------------
Placed this turn:
# 0 : Red:Square placed (1, 1) □
---------------------------
";
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            player.ListTiles();

            Assert.AreEqual(expectedTileList, sw.ToString());

        }

        [TestMethod]
        public void ListPlacedTiles()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 1, 2);
            Player player = new Player("Player");
            player.GetTilesFromBag(2);
            player.PlaceTile(1, 1, 1);
            player.PlaceTile(0, 1, 2);

            string expectedPlacedTileList =
@"Placed this turn:
# 0 : Red:Square placed (1, 1) □
# 1 : Red:Square placed (1, 2) □
---------------------------
";
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);

            player.ListPlacedTiles();

            Assert.AreEqual(expectedPlacedTileList, sw.ToString());
        }

        [TestMethod]
        public void MaxPlaceableTiles_1()
        {
            Game.ResetGame();
            Bag.FillTheBag(6, 1, 1);
            Player player = new Player("Player");
            player.GetTilesFromBag(6);
            player.MaxPlaceableTiles(out int highestCount);
            Assert.AreEqual(6, highestCount);
        }

        [TestMethod]
        public void MaxPlaceableTiles_2()
        {
            Game.ResetGame();
            Bag.FillTheBag(3, 1, 6);
            Player player = new Player("Player");
            player.GetTilesFromBag(6);
            player.MaxPlaceableTiles(out int highestCount);
            Assert.AreEqual(3, highestCount);
        }

        [TestMethod]
        public void ReturnTileToBag()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 1, 1);
            var bagTiles = new StringWriter();
            Console.SetOut(bagTiles);
            Console.SetError(bagTiles);
            Bag.ListTilesInBag();
            Console.Out.Close();

            var junkString = new StringWriter();
            Console.SetOut(junkString);
            Console.SetError(junkString);
            Player player = new Player(false);
            player.GetTilesFromBag(1);

            var emptyBag = new StringWriter();
            Console.SetOut(emptyBag);
            Console.SetError(emptyBag);
            Bag.ListTilesInBag();

            Console.SetOut(junkString);
            Console.SetError(junkString);
            player.ReturnTileToBag(player.tilesInHand[0]);

            var refilledBag = new StringWriter();
           
            Console.SetOut(refilledBag);
            Console.SetError(refilledBag);
            Bag.ListTilesInBag();

            Assert.AreEqual(bagTiles.ToString(), refilledBag.ToString() );

        }

        [TestMethod]
        public void PlaceTile_ByRef()
        {
            Game.ResetGame();
            Bag.FillTheBag(6, 6, 3);
            Player p1 = new Player(true);
            Tile copyOfTile = p1.tilesInHand[0];
            p1.PlaceTile(p1.tilesInHand[0], 1, 1);
            
            Assert.AreEqual(copyOfTile, Grid.GetTile(1, 1));
        }

        [TestMethod]
        public void CalculateTurnScore_horizontal()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(5);

            for (int column = 0; player.tilesInHand.Count> 0; column++)
            {
                player.PlaceTile(0, 0, column);
            }

            //Menu.DrawGrid(10,10);

            Assert.AreEqual(5, Game.CalculateTurnScore(player));
            
        }

        [TestMethod]
        public void GetPlacementDirection_H()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(5);

            for (int column = 0; player.tilesInHand.Count > 0; column++)
            {
                player.PlaceTile(0, 0, column);
            }

            Assert.AreEqual(EnumAxis.Horizontal, player.GetPlacementDirection());

        }

        [TestMethod]
        public void GetPlacementDirection_V()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(5);

            for (int row = 0; player.tilesInHand.Count > 0; row++)
            {
                player.PlaceTile(0, row, 0);
            }

            Assert.AreEqual(EnumAxis.Vertical, player.GetPlacementDirection());
        }

        [TestMethod]
        public void GetPlacementDirection_Single()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(1);

            for (int row = 0; player.tilesInHand.Count > 0; row++)
            {
                player.PlaceTile(0, 0, row);
            }

            Assert.AreEqual(EnumAxis.Horizontal, player.GetPlacementDirection());
        }

        [TestMethod]
        public void CalculateTurnScore_vertical()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(5);

            for (int row = 0; player.tilesInHand.Count > 0; row++)
            {
                player.PlaceTile(0, row, 0);
            }

            Assert.AreEqual(5, Game.CalculateTurnScore(player));

        }

        [TestMethod]
        public void AddGetScore()
        {
            Game.ResetGame();
            Bag.FillTheBag(6, 1, 1); 
            Player player = new Player(true);
            player.PlaceTile(5, 0, 1);
            player.AddScore(5);
            Assert.AreEqual(5, player.GetScore());
        }

        [TestMethod]
        public void RefillHand()
        {
            Game.ResetGame();
            Bag.FillTheBag(6, 6, 1);
            Player player = new Player(true);
            player.PlaceTile(5, 0, 1);
            player.PlaceTile(4, 0, 2);
            player.PlaceTile(3, 0, 3);
            Assert.AreEqual(3, player.RefillHand());
        }
        
        [TestMethod]
        public void CalculateTurnScore_horizontal_Qwirkle()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(6);

            for (int column = 0; player.tilesInHand.Count > 0; column++)
            {
                player.PlaceTile(0, 0, column);
            }

            Assert.AreEqual(12, Game.CalculateTurnScore(player));
        }

        [TestMethod]
        public void CalculateTurnScore_vertical_Qwirkle()
        {
            Game.ResetGame();
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(6);

            for (int row = 0; player.tilesInHand.Count > 0; row++)
            {
                player.PlaceTile(0, row, 0);
            }

            Assert.AreEqual(12, Game.CalculateTurnScore(player));
        }

        [TestMethod]
        public void CalculateTurnScore_MaximumScore()
        {
            Game.ResetGame();
            for (int colour=2; colour < Enum.GetNames(typeof(EnumColour)).Length; colour++)
            {
                for (int shape=1; shape < Enum.GetNames(typeof(EnumShape)).Length; shape++)
                {
                    Grid.PlaceTile(new Tile((EnumColour)colour, (EnumShape)shape), colour, shape);
                    //Console.WriteLine($"Placed {colour}:{shape} @ {colour},{shape}");
                }
            }
            Bag.FillTheBag(1, 6, 1);

            Player player = new Player(false);
            player.GetTilesFromBag(6);
            player.OrderTilesByShape();

            for (int column = 1; player.tilesInHand.Count > 0; column++)
            {
                player.PlaceTile(0, 1, column);
            }

            Assert.AreEqual(84, Game.CalculateTurnScore(player));
        
        }
    }
}
