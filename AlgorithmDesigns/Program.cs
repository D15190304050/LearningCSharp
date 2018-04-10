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

            // O(k,j) = max(O(k,j-1), vj + O(k-wj,j-1)) if wj ≤ k.
            double[] values = { 5, 6, 3 };
            double[] weights = { 4, 5, 2 };
            int totalCapacity = 9;

            double maxValue = KnapsackProblem.SingleItem(totalCapacity, weights, values, out bool[] selectedItems);
            Console.WriteLine(maxValue);
            for (int i = 0; i < selectedItems.Length; i++)
            {
                if (selectedItems[i])
                    Console.Write(i + " ");
            }
            Console.WriteLine();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}
