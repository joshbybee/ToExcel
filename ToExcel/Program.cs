using System;
using System.Diagnostics;
using System.IO;
using ClosedXML.Excel;

namespace ToExcel
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                const string fileName = "period2-2002.txt";
                var fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report", fileName);

                var processor = new TextProcessor();

                var fullText = processor.ReadFile(fileLocation);

                var reducedText = processor.RemoveJunk(fullText);

                var salesReport = processor.Process(reducedText);

                var excelWorksheet = processor.ConvertToExcelDoc(salesReport);

                var excelWorkbook = new XLWorkbook();
                excelWorkbook.AddWorksheet(excelWorksheet, "Period");
                excelWorkbook.SaveAs(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report", "Period.xlsx"));
                Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report", "Period.xlsx"));

                Environment.Exit(0);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}
