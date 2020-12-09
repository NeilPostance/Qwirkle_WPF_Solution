using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qwirkle_WPF;

namespace Qwirkle_WPF_Tests
{
    [TestClass]
    public class TileTests
    {
        [TestMethod]
        public void IsDuplicate()
        {
            Assert.IsTrue(Tile.IsDuplicate(new Tile(EnumColour.Blue, EnumShape.Bang), new Tile(EnumColour.Blue, EnumShape.Bang)));
        }

        [TestMethod]
        public void IsNotDuplicate_ColourDiff()
        {
            Assert.IsFalse(Tile.IsDuplicate(new Tile(EnumColour.Blue, EnumShape.Bang), new Tile(EnumColour.Green, EnumShape.Bang)));
        }

        [TestMethod]
        public void IsNotDuplicate_ShapeDiff()
        {
            Assert.IsFalse(Tile.IsDuplicate(new Tile(EnumColour.Blue, EnumShape.Bang), new Tile(EnumColour.Blue, EnumShape.Circle)));
        }

        [TestMethod]
        public void IsNotDuplicate_BothDiff()
        {
            Assert.IsFalse(Tile.IsDuplicate(new Tile(EnumColour.Blue, EnumShape.Bang), new Tile(EnumColour.Green, EnumShape.Circle)));
        }

        [TestMethod]
        public void IsColourMatch()
        {
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Blue, EnumShape.Circle);

            Assert.IsTrue(Tile.ColourMatch(t1, t2));
        }

        [TestMethod]
        public void IsNotColourMatch()
        {
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Bang);

            Assert.IsFalse(Tile.ColourMatch(t1, t2));
        }

        [TestMethod]
        public void IsShapeMatch()
        {
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Bang);

            Assert.IsTrue(Tile.ShapeMatch(t1, t2));
        }

        [TestMethod]
        public void IsNotShapeMatch()
        {
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Assert.IsFalse(Tile.ShapeMatch(t1, t2));
        }

        [TestMethod]
        public void ToStringTest()
        {
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);

            Assert.AreEqual("Blue:Bang", t1.ToString());
        }

        [TestMethod]
        public void AxisChecker_none()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 2, 2);

            Assert.AreEqual(EnumAxis.None, Tile.AxisChecker(t1, t2));
        }

        [TestMethod]
        public void AxisChecker_same()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 1, 1);

            Assert.AreEqual(EnumAxis.SameLocation, Tile.AxisChecker(t1, t2));
        }

        [TestMethod]
        public void AxisChecker_Horizontal()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 1, 2);

            Assert.AreEqual(EnumAxis.Horizontal, Tile.AxisChecker(t1, t2));
        }

        [TestMethod]
        public void AxisChecker_Vertical()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 2, 1);

            Assert.AreEqual(EnumAxis.Vertical, Tile.AxisChecker(t1, t2));
        }

        [TestMethod]
        public void AxisMatch_a_none()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 2, 2);

            Assert.IsFalse(Tile.AxisMatch(t1, t2, 0));
        }

        public void AxisMatch_a_horizontal()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 2, 1);

            Assert.IsTrue(Tile.AxisMatch(t1, t2, EnumAxis.Horizontal));
        }

        public void AxisMatch_a_vertical()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);
            Tile t2 = new Tile(EnumColour.Green, EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 1, 2);

            Assert.IsTrue(Tile.AxisMatch(t1, t2, EnumAxis.Vertical));
        }

        [TestMethod]
        public void AxisMatch_b_none()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);

            Grid.PlaceTile(t1, 1, 1);

            Assert.IsFalse(Tile.AxisMatch(2, 2, t1, EnumAxis.Horizontal));
        }

        [TestMethod]
        public void AxisMatch_b_horizontal()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);


            Grid.PlaceTile(t1, 1, 1);


            Assert.IsTrue(Tile.AxisMatch(1, 2, t1, EnumAxis.Horizontal));
        }

        [TestMethod]
        public void AxisMatch_b_vertical()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);


            Grid.PlaceTile(t1, 1, 1);


            Assert.IsTrue(Tile.AxisMatch(2, 1, t1, EnumAxis.Vertical));
        }

        [TestMethod]
        public void TouchingChecker_true1()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);

            Grid.PlaceTile(t1, 1, 1);

            Assert.IsTrue(Tile.TouchingChecker(1, 2, t1));
        }

        [TestMethod]
        public void TouchingChecker_true2()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);

            Grid.PlaceTile(t1, 1, 1);

            Assert.IsTrue(Tile.TouchingChecker(2, 1, t1));
        }

        [TestMethod]
        public void TouchingChecker_false()
        {
            Game.ResetGame();
            Tile t1 = new Tile(EnumColour.Blue, EnumShape.Bang);

            Grid.PlaceTile(t1, 1, 1);

            Assert.IsFalse(Tile.TouchingChecker(2, 2, t1));
        }

    }
}
