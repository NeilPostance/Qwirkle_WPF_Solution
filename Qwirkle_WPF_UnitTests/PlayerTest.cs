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
        public void GetTilesFromBag()
        {
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
        public void ReturnTileToBag()
        {
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
        public void CalculateTurnScore_horizontal()
        {
            //Grid.Empty();
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(5);

            for (int column = 0; player.tilesInHand.Count> 0; column++)
            {
                player.PlaceTile(0, 0, column);
            }

            //Menu.DrawGrid(10,10);

            Assert.AreEqual(5, Game.CalculateTurnScore2(player));
            
        }

        [TestMethod]
        public void GetPlacementDirection_H()
        {
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(5);

            for (int column = 0; player.tilesInHand.Count > 0; column++)
            {
                player.PlaceTile(0, 0, column);
            }

            Assert.AreEqual('R', player.GetPlacementDirection());

        }

        [TestMethod]
        public void GetPlacementDirection_V()
        {
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(5);

            for (int row = 0; player.tilesInHand.Count > 0; row++)
            {
                player.PlaceTile(0, row, 0);
            }

            Assert.AreEqual('C', player.GetPlacementDirection());
        }

        [TestMethod]
        public void GetPlacementDirection_Single()
        {
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(1);

            for (int row = 0; player.tilesInHand.Count > 0; row++)
            {
                player.PlaceTile(0, 0, row);
            }

            Assert.AreEqual('R', player.GetPlacementDirection());
        }

        [TestMethod]
        public void CalculateTurnScore_vertical()
        {
            Grid.Empty();
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(5);

            for (int row = 0; player.tilesInHand.Count > 0; row++)
            {
                player.PlaceTile(0, row, 0);
            }

            //Menu.DrawGrid(10,10);

            Assert.AreEqual(5, Game.CalculateTurnScore2(player));

        }
        /*
        [TestMethod]
        public void CalculateTurnScore_horizontal_Qwirkle()
        {
            Bag.FillTheBag(1, 6, 1);
            Player player = new Player(false);
            player.GetTilesFromBag(6);

            for (int column = 0; player.tilesInHand.Count > 0; column++)
            {
                player.PlaceTile(0, 0, column);
            }

            //Menu.DrawGrid(10,10);

            Assert.AreEqual(12, Game.CalculateTurnScore(player));

        }*/
        /*
        [TestMethod]
        public void CalculateTurnScore_MaximumScore()
        {
            
            for (int colour=1; colour <6; colour++)
            {
                for (int shape=0; shape <6; shape++)
                {
                    Grid.PlaceTile(new Tile(colour, shape), colour, shape);
                    //Console.WriteLine($"Placed {colour}:{shape} @ {colour},{shape}");
                }
            }
            Bag.FillTheBag(1, 6, 1);

            Player player = new Player(false);
            player.GetTilesFromBag(6);

            for (int column = 0; player.tilesInHand.Count > 0; column++)
            {
                player.PlaceTile(0, 0, column);
            }

            //Menu.DrawGrid(10,10);

            Assert.AreEqual(84, Game.CalculateTurnScore(player));
        
        }*/
    }
}
