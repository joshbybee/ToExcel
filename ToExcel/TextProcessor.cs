using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ToExcel.Models;
using DataTable = System.Data.DataTable;
using Order = ToExcel.Models.Order;

namespace ToExcel
{
    public class TextProcessor : ITextProcessor
    {
        public List<string> RemoveJunk(List<string> text)
        {
            var output = new List<string>();
            var header = true;

            var endOfHeader = new Regex("^-[- ]+-$");
            var newHeader = new Regex(@"\f");
            var irrelevantDashes = new Regex("--$");
            var endOfEachReport = new Regex("^Report Totals");
            var spaceOnly = new Regex(@"\A\s*\z");
            var subTotals = new Regex("subtotals");
            var finalEndReport = new Regex("^Customer Class");

            foreach (var line in text)
            {   
                if (header)
                {
                    if (endOfHeader.IsMatch(line))
                        header = false;
                }
                else
                {
                    if (newHeader.IsMatch(line))
                    {
                        header = true;
                        continue;
                    }

                    if (irrelevantDashes.IsMatch(line) || endOfEachReport.IsMatch(line) || spaceOnly.IsMatch(line) || subTotals.IsMatch(line))
                        continue;

                    if (finalEndReport.IsMatch(line))
                        break;

                    output.Add(line);
                }
            }

            return output;
        }

        public List<string> ReadFile(string fileLocation)
        {
            //Based upon http://stackoverflow.com/a/286556 -Written by Aleksandar
            var text = new List<string>();
            using (var fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        text.Add(sr.ReadLine());
                    }
                }
            }

            return text;
        }

        public List<SalesPerson> Process(List<string> text)
        {
            var output = new List<SalesPerson>();
            var salesPerson = new Regex("  Salesperson  (.*)");
            var customer = new Regex(@"^    Customer  (\d+)\s+(\w+.*)");
            var saSortCode = new Regex(@"^\s+SA Sort Code\s+(\d+.*)");

            var salesCount = -1;
            var customerCount = -1;
            var orderCount = -1;

            foreach (var line in text)
            {
                if (salesPerson.IsMatch(line))
                {
                    output.Add(new SalesPerson {SalesPersonName = line});
                    salesCount++;
                    customerCount = -1;
                    orderCount = -1;
                    output[salesCount].Customers = new List<Customer>();
                    
                }
                else
                if (customer.IsMatch(line))
                {
                    output[salesCount].Customers.Add(new Customer {CustomerLine = line});
                    customerCount++;
                    orderCount = -1;
                    output[salesCount].Customers[customerCount].Orders = new List<Order>();
                }
                else
                if (saSortCode.IsMatch(line))
                {
                    output[salesCount].Customers[customerCount].Orders.Add(new Order{SaSortCode = line});
                    orderCount++;
                    output[salesCount].Customers[customerCount].Orders[orderCount].Parts = new List<Part>();
                }
                else
                {
                    output[salesCount].Customers[customerCount].Orders[orderCount].Parts.Add(new Part {PartLine = line});
                }
            }


            return output;
        }

        public DataTable ConvertToExcelDoc(List<SalesPerson> salesCollection)
        {
            var dataTable = new DataTable();
            
            dataTable.Columns.Add("Sales Person", typeof(string));
            dataTable.Columns.Add("Customer ID", typeof(string));
            dataTable.Columns.Add("Customer Name", typeof(string));
            dataTable.Columns.Add("SA Sort Code", typeof(string));
            dataTable.Columns.Add("Part Code", typeof(string));
            dataTable.Columns.Add("Part Description", typeof(string));
            dataTable.Columns.Add("Qty Current Period", typeof(int));
            dataTable.Columns.Add("Qty Last YR Period", typeof(int));
            dataTable.Columns.Add("Qty Pct Var 1", typeof(int));
            dataTable.Columns.Add("Qty Current YTD", typeof(int));
            dataTable.Columns.Add("Qty Last YTD", typeof(int));
            dataTable.Columns.Add("Qty Pct Var 2", typeof(int));
            dataTable.Columns.Add("Pounds Current Period", typeof(int));
            dataTable.Columns.Add("Pounds Last YR Period", typeof(int));
            dataTable.Columns.Add("Pounds Pct Var 1", typeof(int));
            dataTable.Columns.Add("Pounds Current YTD", typeof(int));
            dataTable.Columns.Add("Pounds Last YTD", typeof(int));
            dataTable.Columns.Add("Pounds Pct Var 2", typeof(int));

            foreach (var salesPerson in salesCollection)
            {
                foreach (var customer in salesPerson.Customers)
                {
                    foreach (var order in customer.Orders)
                    {
                        foreach (var part in order.Parts)
                        {
                            var row = dataTable.NewRow();

                            row["Sales Person"] = salesPerson.SalesPersonName;
                            row["Customer ID"] = customer.CustomerId;
                            row["Customer Name"] = customer.CustomerDesc;
                            row["SA Sort Code"] = order.SaSortCode;
                            row["Part Code"] = part.PartCode;
                            row["Part Description"] = part.PartDescription;
                            row["Qty Current Period"] = part.QtyCurrentPeriod;
                            row["Qty Last YR Period"] = part.QtyLastYrPeriod;
                            row["Qty Pct Var 1"] = part.QtyPctVar1;
                            row["Qty Current YTD"] = part.QtyCurrentYtd;
                            row["Qty Last YTD"] = part.QtyLastYrYtd;
                            row["Qty Pct Var 2"] = part.QtyPctVar2;
                            row["Pounds Current Period"] = part.PoundsCurrentPeriod;
                            row["Pounds Last YR Period"] = part.PoundsLastYrPeriod;
                            row["Pounds Pct Var 1"] = part.PoundsPctVar1;
                            row["Pounds Current YTD"] = part.PoundsCurrentYtd;
                            row["Pounds Last YTD"] = part.PoundsLastYrYtd;
                            row["Pounds Pct Var 2"] = part.PoundsPctVar2;

                            dataTable.Rows.Add(row);
                        }
                    }
                }
            }
            

            return dataTable;
        }  
    }
}
