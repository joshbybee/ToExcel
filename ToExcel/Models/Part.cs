using System;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Bibliography;

namespace ToExcel.Models
{
    public class Part
    {
        public string PartLine { get; set; }

        public string PartCode
        {
            get
            {
                var partCode = new Regex(@"([^\s]+)");
                var result = partCode.Match(PartLine.Trim());
                return result.Groups[1].Value;
            }
        }

        public string PartDescription
        {
            get
            {
                var result = PartLine.Replace(PartCode, string.Empty);
                return result.Trim().Substring(0, 25).Trim();
            }
        }

        public int QtyCurrentPeriod
        {
            get { return Convert.ToInt32(PartsData[0].Groups[1].Value); }
        }

        public int QtyLastYrPeriod
        {
            get { return Convert.ToInt32(PartsData[1].Groups[1].Value); }
        }

        public int QtyPctVar1
        {
            get  { return PartsData[2].Groups[1].Value == "%%%%" ? 0 : Convert.ToInt32(PartsData[2].Groups[1].Value); }
        }

        public int QtyCurrentYtd
        {
            get { return Convert.ToInt32(PartsData[3].Groups[1].Value); }
        }

        public int QtyLastYrYtd
        {
            get { return Convert.ToInt32(PartsData[4].Groups[1].Value); }
        }

        public int QtyPctVar2
        {
            get { return PartsData[5].Groups[1].Value == "%%%%" ? 0 : Convert.ToInt32(PartsData[5].Groups[1].Value); }
        }

        public int PoundsCurrentPeriod
        {
            get { return Convert.ToInt32(PartsData[6].Groups[1].Value); }
        }

        public int PoundsLastYrPeriod
        {
            get { return Convert.ToInt32(PartsData[7].Groups[1].Value); }
        }

        public int PoundsPctVar1
        {
            get { return PartsData[8].Groups[1].Value == "%%%%" ? 0 : Convert.ToInt32(PartsData[8].Groups[1].Value); }
        }

        public int PoundsCurrentYtd
        {
            get { return Convert.ToInt32(PartsData[9].Groups[1].Value); }
        }

        public int PoundsLastYrYtd
        {
            get { return Convert.ToInt32(PartsData[10].Groups[1].Value); }
        }

        public int PoundsPctVar2
        {
            get { return PartsData[11].Groups[1].Value == "%%%%" ? 0 : Convert.ToInt32(PartsData[11].Groups[1].Value); }
        }

        private MatchCollection PartsData
        {
            get
            {
                var partsData = PartLine.Substring(41);
                var regex = new Regex(@"([^\s]+)");
                var result = regex.Matches(partsData.Trim().Replace(",",""));
                return result;
            }
        }
    }
}
