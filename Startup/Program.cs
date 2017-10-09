using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Startup
{
    class Program
    {
        static int Main(string[] args)
        {
            string s1 = "123";
            string s2 = Console.ReadLine();
            Console.WriteLine(s1.Equals(s2));

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}