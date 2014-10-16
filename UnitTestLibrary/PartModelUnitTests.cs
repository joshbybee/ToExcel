using NUnit.Framework;
using ToExcel.Models;

namespace UnitTestLibrary
{
    [TestFixture]
    public class PartModelUnitTests
    {
        [Test]
        public void PartModelExists()
        {
            var model = new Part();
            Assert.NotNull(model);
        }

        [Test]
        public void PartModelContainsPartDescription()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            30       0    0      30       0    0       300         0    0       300         0    0" };
            Assert.AreEqual("LARGE 19 X 18 X 14", model.PartDescription);
        }

        [Test]
        public void PartModelContainsPartCode()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            30       0    0      30       0    0       300         0    0       300         0    0" };
            Assert.AreEqual("921137-73", model.PartCode);
        }

        [Test]
        public void PartModelContainsQtyCurrentPeriod()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            30       0    0      0       0    0       0         0    0       0         0    0" };
            Assert.AreEqual(30, model.QtyCurrentPeriod);
        }

        [Test]
        public void PartModelContainsQtyLastYrPeriod()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       1    0      0       0    0       0         0    0       0         0    0" };
            Assert.AreEqual(1, model.QtyLastYrPeriod);
        }

        [Test]
        public void PartModelContainsQtyPctVar1()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    1      0       0    0       0         0    0       0         0    0" };
            Assert.AreEqual(1, model.QtyPctVar1);
        }
        [Test]
        public void PartModelContainsQtyCurrentYtd()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    0      30       0    0       0         0    0       0         0    0" };
            Assert.AreEqual(30, model.QtyCurrentYtd);
        }
        [Test]
        public void PartModelContainsQtyLastYrYtd()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    0      0       1    0       0         0    0       0         0    0" };
            Assert.AreEqual(1, model.QtyLastYrYtd);
        }
        [Test]
        public void PartModelContainsQtyPctVar2()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    0      0       0    1       0         0    0       0         0    0" };
            Assert.AreEqual(1, model.QtyPctVar2);
        }

        [Test]
        public void PartModelContainsPoundsCurrentPeriod()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    0      0       0    0       1         0    0       0         0    0" };
            Assert.AreEqual(1, model.PoundsCurrentPeriod);
        }

        [Test]
        public void PartModelContainsPoundsLastYrPeriod()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    0      0       0    0       0         1    0       0         0    0" };
            Assert.AreEqual(1, model.PoundsLastYrPeriod);
        }

        [Test]
        public void PartModelContainsPoundsPctVar1()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    0      0       0    0       0         0    1       0         0    0" };
            Assert.AreEqual(1, model.PoundsPctVar1);
        }
        [Test]
        public void PartModelContainsPoundsCurrentYtd()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    0      0       0    0       0         0    0       1         0    0" };
            Assert.AreEqual(1, model.PoundsCurrentYtd);
        }
        [Test]
        public void PartModelContainsPoundsLastYrYtd()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    0      0       0    0       0         0    0       0         1    0" };
            Assert.AreEqual(1, model.PoundsLastYrYtd);
        }
        [Test]
        public void PartModelContainsPoundsPctVar2()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            0       0    0      0       0    0       0         0    0       0         0    1" };
            Assert.AreEqual(1, model.PoundsPctVar2);
        }

        [Test]
        public void PartModelContainsPartLine()
        {
            var model = new Part { PartLine = "TEST" };
            Assert.NotNull(model.PartLine);
        }

        [Test]
        public void PartModelPartLineRemovesCommas()
        {
            var model = new Part { PartLine = "921137-73       LARGE 19 X 18 X 14            1,000       0    0      0       0    0       0         0    0       0         0    1" };
            Assert.AreEqual(1000, model.QtyCurrentPeriod);
        }

        [Test]
        public void PartModelCodeCanBeTheSameAsOneOftheDataValues()
        {

            var model = new Part { PartLine = "70              .29 SIZE PLASTIC, NO BATT     40     135  -70      40     198  -80       392     1,322  -70       392     1,938  -80" };
            Assert.AreEqual(-70, model.QtyPctVar1);
        }

        [Test]
        public void PartModelCanHandlePercentSignsInIntegerValues()
        {
            var model = new Part { PartLine = "7743-337        Model 53359-DBL               35       0    0     160      -1 %%%%       420         0    0     1,920       -12 %%%%" };
            Assert.AreEqual(0, model.QtyPctVar2);
            Assert.AreEqual(0, model.PoundsPctVar2);
            Assert.AreEqual(0, model.QtyPctVar1);
            Assert.AreEqual(0, model.PoundsPctVar1);
        }
    }
}
