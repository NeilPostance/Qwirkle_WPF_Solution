using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qwirkle_WPF;
using System;
using System.Threading;

namespace Qwirkle_WPF_Tests
{
    [TestClass]
    public class GridTests
    {
        [TestMethod]
        public void PlaceTile()
        {
            Tile tile = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);

            Grid.PlaceTile(tile, 0, 0);

            Tile returnedTile = Grid.tileGrid[0, 0];
            Assert.AreEqual(tile, returnedTile);
        }

        [TestMethod]
        public void IsEmpty()
        {
            Grid.Empty();
            
            Console.WriteLine($"There are {Grid.TileCountInGrid()} in the grid");
            Assert.IsTrue(Grid.IsEmpty());
        }

        [TestMethod]
        public void QueryNearbyLocation_LeftExists()
        {
            Tile tile = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);

            Grid.PlaceTile(tile, 4, 4);
            bool tileExists = Grid.QueryNearbyLocation(5, 4, (int)Grid.EnumDirection.Left, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsTrue(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_RightExists()
        {
            Tile tile = new Tile((int)EnumColour.Red, (int)EnumShape.Square);

            Grid.PlaceTile(tile, 4, 4);
            bool tileExists = Grid.QueryNearbyLocation(3, 4, (int)Grid.EnumDirection.Right, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsTrue(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_UpExists()
        {
            Tile tile = new Tile((int)EnumColour.Green, (int)EnumShape.Diamond);

            Grid.PlaceTile(tile, 4, 4);
            bool tileExists = Grid.QueryNearbyLocation(4, 5, (int)Grid.EnumDirection.Up, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsTrue(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_DownExists()
        {
            Tile tile = new Tile((int)EnumColour.Orange, (int)EnumShape.Circle);

            Grid.PlaceTile(tile, 4, 4);
            bool tileExists = Grid.QueryNearbyLocation(4, 3, (int)Grid.EnumDirection.Down, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsTrue(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_EmptyLocation()
        {
            Tile tile = new Tile((int)EnumColour.Orange, (int)EnumShape.Circle);

            Grid.PlaceTile(tile, 0, 0);
            bool tileExists = Grid.QueryNearbyLocation(9, 9, (int)Grid.EnumDirection.Up, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsFalse(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_InvalidLocation()
        {
            Tile tile = new Tile((int)EnumColour.Orange, (int)EnumShape.Circle);

            Grid.PlaceTile(tile, 0, 0);
            bool tileExists = Grid.QueryNearbyLocation(0, 0, (int)Grid.EnumDirection.Up, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsFalse(tileExists);
        }
    }
}
