using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgParse
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(args);
            SimpleExample();
        }

        static void SimpleExample()
        {
            var parser = new ArgParser(desc: "simple example");
            try
            {
                parser.AddArgument<string>("--host", "localhost");
                parser.AddArgument<int>("--port", 80);

                Dictionary<string, object> args = parser.Parse("--host 192.168.0.1 --port 8080".Split(' '));

                System.Console.WriteLine((string)args["host"]);
                System.Console.WriteLine((int)args["port"]);
            }
            catch (ArgumentException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
