using System.Collections.Generic;

namespace ToExcel.Models
{
    public class Order
    {
        private string _saSortCode;

        public string SaSortCode
        {
            get { return _saSortCode; }
            set { _saSortCode = value.Replace("      SA Sort Code  ", "").Trim(); }
        }

        public List<Part> Parts { get; set; }
    }
}
