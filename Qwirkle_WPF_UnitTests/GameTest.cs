using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qwirkle_WPF;



namespace Qwirkle_WPF_Tests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void CountTilesInLine_rows6()
        {
            Game.ResetGame();
            for (int i = 0; i<6; i++)
            {
                Grid.PlaceTile(new Tile((EnumColour)0, (EnumShape)i), 0, i);
            }

            Grid.QueryNearbyLocation(0, 0, 0, 0, out Tile startingTile);

            Assert.AreEqual(6, Game.CountScoringTilesInLine(startingTile, EnumAxis.Horizontal));
        }

        [TestMethod]
        public void CountTilesInLine_rows1()
        {
            Game.ResetGame();
            for (int i = 0; i < 6; i++)
            {
                Grid.PlaceTile(new Tile((EnumColour)0, (EnumShape)i), 0, i);
            }

            Grid.QueryNearbyLocation(0, 0, 0, 0, out Tile startingTile);

            Assert.AreEqual(1, Game.CountScoringTilesInLine(startingTile, EnumAxis.Vertical));
        }

        [TestMethod]
        public void CountTilesInLine_columns5()
        {
            Game.ResetGame();
            for (int i = 0; i < 5; i++)
            {
                Grid.PlaceTile(new Tile((EnumColour)0, (EnumShape)i), i, 0);
            }

            Grid.QueryNearbyLocation(0, 0, 0, 0, out Tile startingTile);

            Assert.AreEqual(5, Game.CountScoringTilesInLine(startingTile, EnumAxis.Vertical));
        }

        [TestMethod]
        public void CountTilesInLine_columns1()
        {
            Game.ResetGame();
            for (int i = 0; i < 5; i++)
            {
                Grid.PlaceTile(new Tile((EnumColour)0, (EnumShape)i), i, 0);
            }

            Grid.QueryNearbyLocation(0, 0, 0, 0, out Tile startingTile);

            Assert.AreEqual(1, Game.CountScoringTilesInLine(startingTile, EnumAxis.Horizontal));
        }

        [TestMethod]
        public void CalculateTurnScore2_qwirkle()
        {
            Game.ResetGame();
            Bag.FillTheBag(6, 6, 1);
            
            Player player = new Player();
            player.GetTilesFromBag(6);
            for (int i=0; i<6; i++)
            {
                player.PlaceTile(i, 0, i);
            }
            Assert.AreEqual(12, Game.CalculateTurnScore(player));

        }

        [TestMethod]
        public void CalculateTurnScore2()
        {
            Game.ResetGame();
            Bag.FillTheBag(6, 6, 1);

            Player player = new Player();
            player.GetTilesFromBag(5);
            for (int i = 0; i < 5; i++)
            {
                player.PlaceTile(i, 0, i);
            }
            Assert.AreEqual(5, Game.CalculateTurnScore(player));

        }

        [TestMethod]
        public void DeterminePlayerOrder()
        {
            Game.ResetGame();
            Bag.FillTheBag(3, 1, 1);
            Player p1 = new Player(false);
            p1.GetTilesFromBag(3); //they'll be able to place 3 tiles

            Bag.FillTheBag(1, 5, 1);
            Player p2 = new Player(false);
            p2.GetTilesFromBag(5);//they'll be able to place 5 tiles

            Bag.FillTheBag(1, 1, 6);
            Player p3 = new Player(false);
            p3.GetTilesFromBag(6);//they'll be able to place 1 tile

            List<Player> expectedPlayerList = new List<Player>();
            expectedPlayerList.Add(p2); //p2 has the most placeable tiles so they go first
            expectedPlayerList.Add(p3); //the rest follow in sequence of creation, looping.
            expectedPlayerList.Add(p1);

            Game.DeterminePlayerOrder();

            bool isSame = true;
            for (int playerIndex = 0; playerIndex < Game.ListOfOrderedPlayers.Count; playerIndex++)
            {
                isSame = expectedPlayerList[playerIndex] == Game.ListOfOrderedPlayers[playerIndex] ? true : false;
                if (isSame == false)
                    break;
            }

            Assert.IsTrue(isSame);
        }

        [TestMethod]
        public void NextPlayer()
        {
            Game.ResetGame();
            Bag.FillTheBag(3, 1, 1);
            Player p1 = new Player(false);
            p1.GetTilesFromBag(3); //they'll be able to place 3 tiles

            Bag.FillTheBag(1, 5, 1);
            Player p2 = new Player(false);
            p2.GetTilesFromBag(5);//they'll be able to place 5 tiles

            Game.DeterminePlayerOrder(); // p2 will play first
            Game.NextPlayer(); //p1 plays next
            Assert.IsTrue(Game.CurrentPlayer == p1);
            
            
        }
    }
}
