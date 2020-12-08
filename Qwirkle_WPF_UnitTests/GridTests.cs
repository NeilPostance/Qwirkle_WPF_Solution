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
       
            Assert.IsTrue(Grid.IsEmpty());
        }

        [TestMethod]
        public void IsNotEmpty()
        {
            Grid.Empty();
            Grid.PlaceTile(new Tile(EnumColour.Green, EnumShape.Star), 4, 4);
            Assert.IsFalse(Grid.IsEmpty());
        }

        [TestMethod]
        public void IsEmptyPosition()
        {
            Grid.Empty();
            Grid.PlaceTile(new Tile(EnumColour.Blue, EnumShape.Bang), 4, 4);
            Assert.IsTrue(Grid.IsEmptyPosition(3, 3));
        }

        [TestMethod]
        public void IsNotEmptyPosition()
        {
            Grid.Empty();
            Grid.PlaceTile(new Tile(EnumColour.Blue, EnumShape.Bang), 4, 4);
            Assert.IsFalse(Grid.IsEmptyPosition(4, 4));
        }

        [TestMethod]
        public void QueryNearbyLocation_LeftExists()
        {
            Tile tile = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);

            Grid.PlaceTile(tile, 4, 4);
            bool tileExists = Grid.QueryNearbyLocation(5, 4, EnumDirection.Left, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsTrue(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_RightExists()
        {
            Tile tile = new Tile((int)EnumColour.Red, (int)EnumShape.Square);

            Grid.PlaceTile(tile, 4, 4);
            bool tileExists = Grid.QueryNearbyLocation(3, 4, EnumDirection.Right, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsTrue(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_UpExists()
        {
            Tile tile = new Tile((int)EnumColour.Green, (int)EnumShape.Diamond);

            Grid.PlaceTile(tile, 4, 4);
            bool tileExists = Grid.QueryNearbyLocation(4, 5, EnumDirection.Up, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsTrue(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_DownExists()
        {
            Tile tile = new Tile((int)EnumColour.Orange, (int)EnumShape.Circle);

            Grid.PlaceTile(tile, 4, 4);
            bool tileExists = Grid.QueryNearbyLocation(4, 3, EnumDirection.Down, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsTrue(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_EmptyLocation()
        {
            Tile tile = new Tile((int)EnumColour.Orange, (int)EnumShape.Circle);

            Grid.PlaceTile(tile, 0, 0);
            bool tileExists = Grid.QueryNearbyLocation(9, 9, EnumDirection.Up, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsFalse(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_InvalidLocation()
        {
            Tile tile = new Tile((int)EnumColour.Orange, (int)EnumShape.Circle);

            Grid.PlaceTile(tile, 0, 0);
            bool tileExists = Grid.QueryNearbyLocation(0, 0, EnumDirection.Up, 1, out Tile outTile);
            if (tileExists)
                Console.WriteLine($"Found {outTile}");

            Assert.IsFalse(tileExists);
        }

        [TestMethod]
        public void QueryNearbyLocation_TileBased_Left()
        {
            Grid.Empty();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 2, 2);
            Grid.PlaceTile(t2, 2, 1);

            Grid.QueryNearbyLocation(t1, EnumDirection.Left, 1, out Tile outTile);

            Assert.ReferenceEquals(t2, outTile);
        }

        [TestMethod]
        public void QueryNearbyLocation_TileBased_Right()
        {
            Grid.Empty();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 2, 2);
            Grid.PlaceTile(t2, 2, 1);

            Grid.QueryNearbyLocation(t2, EnumDirection.Right, 1, out Tile outTile);

            Assert.ReferenceEquals(t1, outTile);
        }

        [TestMethod]
        public void QueryNearbyLocation_TileBased_Up()
        {
            Grid.Empty();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 2);
            Grid.PlaceTile(t2, 2, 2);

            Grid.QueryNearbyLocation(t2, EnumDirection.Up, 1, out Tile outTile);

            Assert.ReferenceEquals(t1, outTile);
        }

        [TestMethod]
        public void QueryNearbyLocation_TileBased_Down()
        {
            Grid.Empty();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 2);
            Grid.PlaceTile(t2, 2, 2);

            Grid.QueryNearbyLocation(t1, EnumDirection.Down, 1, out Tile outTile);

            Assert.ReferenceEquals(t2, outTile);
        }

        [TestMethod]
        public void QueryNearbyLocation_TileBased_Down2()
        {
            Grid.Empty();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);
            Tile t3 = new Tile(EnumColour.Red, EnumShape.Club);

            Grid.PlaceTile(t1, 1, 2);
            Grid.PlaceTile(t2, 2, 2);
            Grid.PlaceTile(t3, 3, 2);

            Grid.QueryNearbyLocation(t1, EnumDirection.Down, 2, out Tile outTile);

            Assert.ReferenceEquals(t3, outTile);
        }

    }
}
