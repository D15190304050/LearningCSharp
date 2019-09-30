using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    public class LongestIncreasingSequence
    {
        // TODO: Find and program an efficient algorithm to get LIS.

        public static int LengthOfLis(double[] values)
        {
            int count = values.Count();
            int[] lisLength = new int[count];
            for (int i = 0; i < lisLength.Length; i++)
                lisLength[i] = 1;

            for (int i = 0; i < lisLength.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if ((values[i] > values[j]) && (lisLength[i] < lisLength[j] + 1))
                        lisLength[i] = lisLength[j] + 1;
                }
            }

            int max = 0;
            foreach (int i in lisLength)
            {
                if (max < i)
                    max = i;
            }

            return max;
        }
    }
}
