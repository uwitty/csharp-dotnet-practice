using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOp
{
    class Program
    {
        static void Main(string[] args)
        {
#if true
            using (var excel = new OfficeOp.ExcelOp())
            {
                System.Console.WriteLine("hello OfficeOp");
                excel["A1"] = "A1";
                excel["A2"] = "A2";
                excel["B1"] = "B1";
            }
#else
            using (var excel = new OfficeOp.ExcelOp("c:\\Users\\u\\Documents\\Book1.xlsx", "Sheet2"))
            {
                System.Console.WriteLine("hello OfficeOp");
                excel["C1"] = "hahaha";
                //excel["A1"] = "A1";
                //excel["A2"] = "A2";
                //excel["B1"] = "B1";
            }
#endif
        }
    }
}
