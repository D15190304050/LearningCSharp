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
            //MySqlConnectionTest.ConnectionTest();

            string[] txt = { "I", "am", "fine", "thank", "you" };
            LinkedList<string> list = new LinkedList<string>();
            SortedSet<string> set = new SortedSet<string>(txt);
            set.Remove("I");
            foreach (string s in set)
                Console.Write(s + " ");
            Console.WriteLine();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}