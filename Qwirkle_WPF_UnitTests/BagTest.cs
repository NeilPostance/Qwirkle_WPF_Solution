using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qwirkle_WPF;


namespace Qwirkle_WPF_Tests
{
    [TestClass]
    public class BagTests
    {
        [TestMethod]
        public void FillTheBag()
        {
            Bag.EmptyTheBag();
            
            Bag.FillTheBag(6, 6, 3);
            
            Assert.AreEqual(6 * 6 * 3, Bag.tilesInBag.Count);
        }

        [TestMethod]
        public void Count()
        {
            Bag.FillTheBag(6, 6, 5);

            Assert.AreEqual(Bag.tilesInBag.Count, Bag.Count());
        }


        [TestMethod]
        public void ListTilesInBag()
        {
            Bag.FillTheBag(2, 2, 2);

            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);
            string bagContents = @"The Bag Contains:
Red:Square
Red:Circle
Orange:Square
Orange:Circle
Red:Square
Red:Circle
Orange:Square
Orange:Circle
There are 8 tiles in the bag.
";
            Bag.ListTilesInBag();
            Assert.AreEqual(bagContents, sw.ToString());
        }
    }
}
