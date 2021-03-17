using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableApp.Interfaces
{
    public interface ITextConverter
    {
        bool CheckTextToNumber(string text);
        int TextToNumber(string text);
        string Check(string inputString);
        string ArithmeticOperation(string inputString);
    }
}
