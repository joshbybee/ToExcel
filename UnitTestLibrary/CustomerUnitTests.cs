using System.Collections.Generic;
using NUnit.Framework;
using ToExcel.Models;

namespace UnitTestLibrary
{
    [TestFixture]
    public class CustomerUnitTests
    {
        [Test]
        public void CustomerModelExists()
        {
            var model = new Customer();
            Assert.NotNull(model);
        }
        [Test]
        public void CustomerModelCanContainCustomerDesc()
        {
            var model = new Customer { CustomerLine = "    Customer  1099 COMPANY 1" };
            Assert.AreEqual("COMPANY 1", model.CustomerDesc);
        }
        [Test]
        public void CustomerModelCanContainCustomerId()
        {
            var model = new Customer { CustomerLine = "    Customer  1099 COMPANY 1" };
            Assert.AreEqual(1099, model.CustomerId);
        }

        [Test]
        public void CustomerModelCanContainListOfSales()
        {
            var model = new Customer {Orders = new List<Order>()};
            Assert.NotNull(model.Orders);
        }

        [Test]
        public void CustomerModelConvertsLineIntoDataVals()
        {
            var model = new Customer{CustomerLine = "    Customer  1099 COMPANY 3474"};

            Assert.AreEqual(1099, model.CustomerId);
            Assert.AreEqual("COMPANY 3474", model.CustomerDesc);
        }
    }
}
