using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ToExcel;
using ToExcel.Models;

namespace UnitTestLibrary
{
    [TestFixture]
    public class TextProcessorUnitTests
    {
        private const string FileName = "period2-2002.txt";
        private readonly string _fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report", FileName);

        [Test]
        public void TextProcessorDoesntPrintHeaders()
        {

            var sampleText = new List<string>
            {
                "\f",
                "TEE_X_101 26-Mar-2002 15:26 1: GE",
                "---------------Current LastYrPart Code Description Period Period",
                "--------------- ------------------------ ------- -------",
                "Good data"
            };
            var processor = new TextProcessor();

            var result = processor.RemoveJunk(sampleText);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Good data", result.First());
        }

        [Test]
        public void TextProcessorDoesntPrintEndOfReport()
        {

            var sampleText = new List<string>
            {
                "--------------- ------------------------ ------- -------",
                "Customer Class                     FS                                ",
                "Salesperson                     X                                    ",
                "Broker Code                                                          ",
                "Customer Number                 X                                    ",
                "Redistributor Name                                                   ",
                "Nutrition Status                                                     ",
                "Sales Analysis Sort Code        X                                    ",
                "Part Code                       X                                    ",
                "Salesperson as Customer         N                                    ",
                "Historical Salesperson          N                                    ",
                "Display Weight or Value         W                                    ",
                "Year                            2002                                 ",
                "Period                          2                                    ",
                "Start Week                                                           ",
                "End Week                                                             "
            };
            var processor = new TextProcessor();

            var result = processor.RemoveJunk(sampleText);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void TextProcessorDoesntPrintSubtotals()
        {

            var sampleText = new List<string>
            {
                "--------------- ------------------------ ------- -------",
                "      SA Sort Code subtotals                  30       0    0      70       0    0       300         0    0       700         0    0",
                "    Customer subtotals                        40       0    0      80       0    0       400         0    0       800         0    0",
                "Good data"
            };
            var processor = new TextProcessor();

            var result = processor.RemoveJunk(sampleText);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Good data", result.First());
        }

        [Test]
        public void TextProcessorDoesntKeepSpaceLines()
        {

            var sampleText = new List<string>
            {
                "--------------- ------------------------ ------- -------",
                "                                                        ",
                "Good data"
            };
            var processor = new TextProcessor();

            var result = processor.RemoveJunk(sampleText);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Good data", result.First());
        }

        [Test]
        public void TextProcessorDoesntPrintHeadersTwoIterations()
        {

            var sampleText = new List<string>
            {
                "\f",
                "TEE_X_101 26-Mar-2002 15:26 1: GE",
                "---------------Current LastYrPart Code Description Period Period",
                "--------------- ------------------------ ------- -------",
                "Good data",
                "\f",
                "TEE_X_101 26-Mar-2002 15:26 1: GE",
                "---------------Current LastYrPart Code Description Period Period",
                "--------------- ------------------------ ------- -------",
                "More data"
            };
            var processor = new TextProcessor();

            var result = processor.RemoveJunk(sampleText);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Good data", result.First());
            Assert.AreEqual("More data", result[1]);
        }

        [Test]
        public void TextProcessorRemoveDashes()
        {

            var sampleText = new List<string>
            {
                "--------------- ------------------------ ------- -------",
                "--",
                "-----",
                "Keep me",
                "--------------- ------------------------ ------- -------"
            };
            var processor = new TextProcessor();

            var result = processor.RemoveJunk(sampleText);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Keep me", result.First());
        }

        [Test]
        public void TextProcessorRemovesEndReportText()
        {

            var sampleText = new List<string>
            {
                "--------------- ------------------------ ------- -------",
                "Report Totals                            924,553 704,028   31  1,612K  1,226K   31 9,446,795 7,553,917   25   16,837K   13,176K   28",
                "Good Data"
            };
            var processor = new TextProcessor();

            var result = processor.RemoveJunk(sampleText);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Good Data", result.First());
        }

        [Test]
        public void ReadFileOutputsListFromFile()
        {
            var processor = new TextProcessor();

            var result = processor.ReadFile(_fileLocation);
            Assert.Greater(result.Count, 0, "Result was empty");
        }

        [Test]
        public void ProcessReturnsListOfSalesPersonsAndSales()
        {
            var sampleText = new List<string>
            {
                "  Salesperson  00 NOBODY                                                                                                            ",
                "    Customer  1036 COMPANY 501                                                                                                      ",
                "      SA Sort Code  1.43 WATER DOLLS                                                                                                ",
                "78-143FS        17/8# SS MODEL                10       1    2      10       3    4       100         4    5       100         6    7"
            };

            var processor = new TextProcessor();

            var result = processor.Process(sampleText);

            Assert.AreEqual("00 NOBODY", result.First().SalesPersonName);
            Assert.AreEqual(1036, result.First().Customers.First().CustomerId);
            Assert.AreEqual("COMPANY 501", result.First().Customers.First().CustomerDesc);
            Assert.AreEqual("1.43 WATER DOLLS", result.First().Customers.First().Orders.First().SaSortCode);
            Assert.AreEqual("78-143FS", result.First().Customers.First().Orders.First().Parts.First().PartCode);
            Assert.AreEqual("17/8# SS MODEL", result.First().Customers.First().Orders.First().Parts.First().PartDescription);
            Assert.AreEqual(10, result.First().Customers.First().Orders.First().Parts.First().QtyCurrentPeriod);
            Assert.AreEqual(1, result.First().Customers.First().Orders.First().Parts.First().QtyLastYrPeriod);
            Assert.AreEqual(2, result.First().Customers.First().Orders.First().Parts.First().QtyPctVar1);
            Assert.AreEqual(10, result.First().Customers.First().Orders.First().Parts.First().QtyCurrentYtd);
            Assert.AreEqual(3, result.First().Customers.First().Orders.First().Parts.First().QtyLastYrYtd);
            Assert.AreEqual(4, result.First().Customers.First().Orders.First().Parts.First().QtyPctVar2);
            Assert.AreEqual(100, result.First().Customers.First().Orders.First().Parts.First().PoundsCurrentPeriod);
            Assert.AreEqual(4, result.First().Customers.First().Orders.First().Parts.First().PoundsLastYrPeriod);
            Assert.AreEqual(5, result.First().Customers.First().Orders.First().Parts.First().PoundsPctVar1);
            Assert.AreEqual(100, result.First().Customers.First().Orders.First().Parts.First().PoundsCurrentYtd);
            Assert.AreEqual(6, result.First().Customers.First().Orders.First().Parts.First().PoundsLastYrYtd);
            Assert.AreEqual(7, result.First().Customers.First().Orders.First().Parts.First().PoundsPctVar2);
        }

        [Test]
        public void ConvertToExcelDocOutputsCorrectColumnsAndRows()
        {
            var processor = new TextProcessor();
            var salesCollection = new List<SalesPerson>
            {
                new SalesPerson
                {
                    SalesPersonName = "John Doe",
                    Customers = new List<Customer>
                    {
                        new Customer
                        {
                            CustomerLine = "    Customer  1036 COMPANY 501",
                            Orders = new List<Order>
                            {
                                new Order
                                {
                                    SaSortCode = "      SA Sort Code  1.43 WATER DOLLS",
                                    Parts = new List<Part>
                                    {
                                        new Part
                                        {
                                            PartLine =
                                                "8341            .21 SIZE PLASTIC, NO BATT      0      10 -100      60      10  500         0       120 -100       720       120  500"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var result = processor.ConvertToExcelDoc(salesCollection);

            Assert.AreEqual("John Doe", result.Rows[0]["Sales Person"]);
            Assert.AreEqual("1036", result.Rows[0]["Customer ID"]);
            Assert.AreEqual("COMPANY 501", result.Rows[0]["Customer Name"]);
            Assert.AreEqual("1.43 WATER DOLLS", result.Rows[0]["SA Sort Code"]);
            Assert.AreEqual("8341", result.Rows[0]["Part Code"]);
            Assert.AreEqual(".21 SIZE PLASTIC, NO BATT", result.Rows[0]["Part Description"]);
            Assert.AreEqual(0, result.Rows[0]["Qty Current Period"]);
            Assert.AreEqual(10, result.Rows[0]["Qty Last YR Period"]);
            Assert.AreEqual(-100, result.Rows[0]["Qty Pct Var 1"]);
            Assert.AreEqual(60, result.Rows[0]["Qty Current YTD"]);
            Assert.AreEqual(10, result.Rows[0]["Qty Last YTD"]);
            Assert.AreEqual(500, result.Rows[0]["Qty Pct Var 2"]);
            Assert.AreEqual(0, result.Rows[0]["Pounds Current Period"]);
            Assert.AreEqual(120, result.Rows[0]["Pounds Last YR Period"]);
            Assert.AreEqual(-100, result.Rows[0]["Pounds Pct Var 1"]);
            Assert.AreEqual(720, result.Rows[0]["Pounds Current YTD"]);
            Assert.AreEqual(120, result.Rows[0]["Pounds Last YTD"]);
            Assert.AreEqual(500, result.Rows[0]["Pounds Pct Var 2"]);
        }

    }
}
