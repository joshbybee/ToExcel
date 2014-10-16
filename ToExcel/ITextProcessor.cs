using System.Collections.Generic;
using ToExcel.Models;

namespace ToExcel
{
    public interface ITextProcessor
    {
        List<string> RemoveJunk(List<string> text);
        List<string> ReadFile(string fileLocation);
        List<SalesPerson> Process(List<string> text);
    }
}
