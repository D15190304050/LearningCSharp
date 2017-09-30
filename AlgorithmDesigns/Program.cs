using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            // Test your algorithm here.
            int[] data = new int[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = i;
            PersonalDataStructuresAndAlgorithm.BasicDataStructures.StdRandom.Shuffle(data);

            UnitTest.TopKUnitTest(TopKTestOption.SepcifiedArray, data, 10);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}
