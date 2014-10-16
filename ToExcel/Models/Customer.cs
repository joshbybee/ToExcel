using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ToExcel.Models
{
     public class Customer
    {
         public int CustomerId
         {
             get
             {
                 var regex = new Regex(@"^Customer  (\d+)\s+(\w+.*)");
                 var result = regex.Match(CustomerLine.Trim());
                 return Convert.ToInt32(result.Groups[1].Value);
             } 
         }

         public string CustomerDesc
         {
             get
             {
                 var regex = new Regex(@"^Customer  (\d+)\s+(\w+.*)");
                 var result = regex.Match(CustomerLine.Trim());
                 return result.Groups[2].Value;
             }
         }

         public string CustomerLine { get; set; }

         public List<Order> Orders { get; set; }
    }
}
