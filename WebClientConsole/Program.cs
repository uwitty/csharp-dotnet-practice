using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebClient;

namespace WebClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            System.Console.WriteLine("Run()");
            using (var client = new WebClient.WebClient())
            {
                System.Console.WriteLine("Get google");
                var google = client.Get("https://www.google.co.jp");

                System.Console.WriteLine("Get msdn");
                var msdn = client.Get("https://docs.microsoft.com/ja-jp/dotnet/api/system.net.http.httpclient?redirectedfrom=MSDN&view=netframework-4.7.2");


                System.Console.WriteLine("Wait google");
                var googleBytes = await google;
                System.Console.WriteLine("Got {0} bytes from google", googleBytes.Length);


                System.Console.WriteLine("Wait mdsn");
                var msdnBytes = await msdn;
                System.Console.WriteLine("Got {0} bytes from msdn", msdnBytes.Length);
            }
        }
    }
}
