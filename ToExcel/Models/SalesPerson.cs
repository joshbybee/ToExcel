using System.Collections.Generic;

namespace ToExcel.Models
{
    public class SalesPerson
    {
        private string _salesPersonName;
        public List<Customer> Customers { get; set; }

        public string SalesPersonName
        {
            get { return _salesPersonName; }
            set { _salesPersonName = value.Replace("Salesperson", "").Trim(); }
        }
    }
}
