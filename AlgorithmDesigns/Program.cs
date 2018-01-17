using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    public class Program
    {
        private static double CostFunc(double followingCost, double defectiveProbability, int lotSize)
        {
            double setupCost = lotSize == 0 ? 0 : 3;

            return (setupCost + lotSize + followingCost * Math.Pow(defectiveProbability, lotSize));
        }

        public static int Main(string[] args)
        {
            // Test your algorithm here.
            //UnitTest.TopKUnitTest();
            //UnitTest.NumberOfInversionsUnitTest();
            //UnitTest.RoundRobinFunctionalityTest(16);

            RejectAllowances ra = new RejectAllowances(3, 16, CostFunc, 0.5, 3);
            ra.Execute();
            Console.WriteLine($"Expected total cost = {ra.ExpectedTotalCost * 100}");
            foreach (int i in ra.Policy)
                Console.Write(i + " ");
            Console.WriteLine();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}
