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

            Assert.IsTrue(Tile.IsDuplicate(new Tile((int)EnumColour.Blue, (int)EnumShape.Bang), new Tile((int)EnumColour.Blue, (int)EnumShape.Bang)));

        }

        [TestMethod]
        public void IsNotDuplicate_ColourDiff()
        {

            Assert.IsFalse(Tile.IsDuplicate(new Tile((int)EnumColour.Blue, (int)EnumShape.Bang), new Tile((int)EnumColour.Green, (int)EnumShape.Bang)));

        }

        [TestMethod]
        public void IsNotDuplicate_ShapeDiff()
        {

            Assert.IsFalse(Tile.IsDuplicate(new Tile((int)EnumColour.Blue, (int)EnumShape.Bang), new Tile((int)EnumColour.Blue, (int)EnumShape.Circle)));

        }

        [TestMethod]
        public void IsNotDuplicate_BothDiff()
        {

            Assert.IsFalse(Tile.IsDuplicate(new Tile((int)EnumColour.Blue, (int)EnumShape.Bang), new Tile((int)EnumColour.Green, (int)EnumShape.Circle)));

        }

        [TestMethod]
        public void IsColourMatch()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Blue, (int)EnumShape.Circle);

            Assert.IsTrue(Tile.ColourMatch(t1, t2));

        }

        [TestMethod]
        public void IsNotColourMatch()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Bang);

            Assert.IsFalse(Tile.ColourMatch(t1, t2));

        }

        [TestMethod]
        public void IsShapeMatch()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Bang);

            Assert.IsTrue(Tile.ShapeMatch(t1, t2));

        }

        [TestMethod]
        public void IsNotShapeMatch()
        {
            
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Circle);

            Assert.IsFalse(Tile.ShapeMatch(t1, t2));

        }

        [TestMethod]
        public void ToStringTest()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);

            Assert.AreEqual("Blue:Bang", t1.ToString());
        }

        [TestMethod]
        public void AxisChecker_none()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 2, 2);

            Assert.AreEqual(0, Tile.AxisChecker(t1, t2));
        }

        [TestMethod]
        public void AxisChecker_same()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 1, 1);

            Assert.AreEqual(0, Tile.AxisChecker(t1, t2));
        }

        [TestMethod]
        public void AxisChecker_Horizontal()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 1, 2);

            Assert.AreEqual(1, Tile.AxisChecker(t1, t2));
        }

        [TestMethod]
        public void AxisChecker_Vertical()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 2, 1);

            Assert.AreEqual(2, Tile.AxisChecker(t1, t2));
        }

        [TestMethod]
        public void AxisMatch_a_none()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 2, 2);

            Assert.IsFalse(Tile.AxisMatch(t1, t2, 0));
        }

        public void AxisMatch_a_horizontal()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 2, 1);

            Assert.IsTrue(Tile.AxisMatch(t1, t2, 1));
        }

        public void AxisMatch_a_vertical()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);
            Tile t2 = new Tile((int)EnumColour.Green, (int)EnumShape.Circle);

            Grid.PlaceTile(t1, 1, 1);
            Grid.PlaceTile(t2, 1, 2);

            Assert.IsTrue(Tile.AxisMatch(t1, t2, 2));
        }

        [TestMethod]
        public void AxisMatch_b_none()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);

            Grid.PlaceTile(t1, 1, 1);

            Assert.IsFalse(Tile.AxisMatch(2, 2, t1, 0));
        }

        [TestMethod]
        public void AxisMatch_b_horizontal()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);


            Grid.PlaceTile(t1, 1, 1);


            Assert.IsTrue(Tile.AxisMatch(1, 2, t1, 1));
        }

        [TestMethod]
        public void AxisMatch_b_vertical()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);


            Grid.PlaceTile(t1, 1, 1);


            Assert.IsTrue(Tile.AxisMatch(2, 1, t1, 2));
        }

        [TestMethod]
        public void TouchingChecker_true1()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);

            Grid.PlaceTile(t1, 1, 1);

            Assert.IsTrue(Tile.TouchingChecker(1, 2, t1));
        }

        [TestMethod]
        public void TouchingChecker_true2()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);

            Grid.PlaceTile(t1, 1, 1);

            Assert.IsTrue(Tile.TouchingChecker(2, 1, t1));
        }

        [TestMethod]
        public void TouchingChecker_false()
        {
            Tile t1 = new Tile((int)EnumColour.Blue, (int)EnumShape.Bang);

            Grid.PlaceTile(t1, 1, 1);

            Assert.IsFalse(Tile.TouchingChecker(2, 2, t1));
        }

    }
}
