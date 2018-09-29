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

            // R[i] = Max(R[i - 1], R[j] + p[i]).
            //double[] distances = { 0, 1, 3.14, 5.6, 9.4, 10.6, 11.3 };
            //double[] profits = { 0, 3, 1, 5, 2, 6, 3 };
            //double minDistance = 4;

            //double[] traces = new double[distances.Length];
            //traces[1] = profits[0];
            //for (int i = 2; i < traces.Length; i++)
            //{
            //    for (int j = i - 1; j >= 0; j--)
            //    {
            //        if (distances[i] - distances[j] >= minDistance)
            //        {
            //            double possibleProfit = Math.Max(traces[i - 1], traces[j] + profits[i]);
            //            traces[i] = Math.Max(traces[i], possibleProfit);
            //        }
            //        else
            //            traces[i] = Math.Max(traces[i - 1], traces[i]);
            //    }
            //}

            //Console.WriteLine(traces[distances.Length - 1]);

            UnitTest.AndOrTreeTest();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}
