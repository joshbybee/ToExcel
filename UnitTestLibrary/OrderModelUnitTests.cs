using System.Collections.Generic;
using NUnit.Framework;
using ToExcel.Models;

namespace UnitTestLibrary
{
    [TestFixture]
    public class OrderModelUnitTests
    {
        [Test]
        public void OrderModelExists()
        {
            var model = new Order();
            Assert.NotNull(model);
        }

        [Test]
        public void EachOrderHasASortCode()
        {
            var model = new Order { SaSortCode = "1.11 MISC DOLLS" };
            Assert.NotNull(model.SaSortCode);
        }

        [Test]
        public void EachOrderHasAListOfParts()
        {
            var model = new Order { Parts = new List<Part>() };
            Assert.NotNull(model.Parts);
        }

        [Test]
        public void SaSortCodeTakesInRawDataAndGetsRidOfExcess()
        {
            var model = new Order { SaSortCode = "      SA Sort Code  10.1 BASKETBALLS" };
            Assert.AreEqual("10.1 BASKETBALLS", model.SaSortCode);
        }
    }
}
