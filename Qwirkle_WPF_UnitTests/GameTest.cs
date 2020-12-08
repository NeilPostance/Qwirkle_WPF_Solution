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
            Grid.Empty();
            for (int i = 0; i<6; i++)
            {
                Grid.PlaceTile(new Tile(0, i), 0, i);
            }

            Grid.QueryNearbyLocation(0, 0, 0, 0, out Tile startingTile);

            Assert.AreEqual(6, Game.CountScoringTilesInLine(startingTile, 'R'));
        }

        [TestMethod]
        public void CountTilesInLine_rows1()
        {
            Grid.Empty();
            for (int i = 0; i < 6; i++)
            {
                Grid.PlaceTile(new Tile(0, i), 0, i);
            }

            Grid.QueryNearbyLocation(0, 0, 0, 0, out Tile startingTile);

            Assert.AreEqual(1, Game.CountScoringTilesInLine(startingTile, 'C'));
        }

        [TestMethod]
        public void CountTilesInLine_columns5()
        {
            Grid.Empty();
            for (int i = 0; i < 5; i++)
            {
                Grid.PlaceTile(new Tile(0, i), i, 0);
            }

            Grid.QueryNearbyLocation(0, 0, 0, 0, out Tile startingTile);

            Assert.AreEqual(5, Game.CountScoringTilesInLine(startingTile, 'C'));
        }

        [TestMethod]
        public void CountTilesInLine_columns1()
        {
            Grid.Empty();
            for (int i = 0; i < 5; i++)
            {
                Grid.PlaceTile(new Tile(0, i), i, 0);
            }

            Grid.QueryNearbyLocation(0, 0, 0, 0, out Tile startingTile);

            Assert.AreEqual(1, Game.CountScoringTilesInLine(startingTile, 'R'));
        }

        [TestMethod]
        public void CalculateTurnScore2_qwirkle()
        {
            Grid.Empty();
            Bag.FillTheBag(6, 6, 1);
            
            Player player = new Player();
            player.GetTilesFromBag(6);
            for (int i=0; i<6; i++)
            {
                player.PlaceTile(i, 0, i);
            }
            Assert.AreEqual(12, Game.CalculateTurnScore2(player));

        }

        [TestMethod]
        public void CalculateTurnScore2()
        {
            Grid.Empty();
            Bag.FillTheBag(6, 6, 1);

            Player player = new Player();
            player.GetTilesFromBag(5);
            for (int i = 0; i < 5; i++)
            {
                player.PlaceTile(i, 0, i);
            }
            Assert.AreEqual(5, Game.CalculateTurnScore2(player));

        }
    }
}
