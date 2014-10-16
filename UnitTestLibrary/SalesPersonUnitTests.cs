using System.Collections.Generic;
using NUnit.Framework;
using ToExcel.Models;
namespace UnitTestLibrary
{
    [TestFixture]
    public class SalesPersonUnitTests
    {
        [Test]
        public void SalesPersonModelExists()
        {
            var model = new SalesPerson();

            Assert.NotNull(model);
        }

        [Test]
        public void SalesPersonModelCanContainsCustomers()
        {
            var model = new SalesPerson {Customers = new List<Customer>()};
            Assert.NotNull(model.Customers);
        }

        [Test]
        public void SalesPersonModelContainsSalesPersonName()
        {
            var model = new SalesPerson { SalesPersonName = "00 JOE" };
            Assert.AreEqual("00 JOE", model.SalesPersonName);
        }

        [Test]
        public void SalesPersonNameWillScrubOffUnimportantStuff()
        {
            var model = new SalesPerson { SalesPersonName = "  Salesperson  00 NOBODY   " };
            Assert.AreEqual("00 NOBODY", model.SalesPersonName);
        }
    }
}
