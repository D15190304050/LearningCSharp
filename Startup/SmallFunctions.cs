using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathematics;

namespace Startup
{
    public static class SmallFunctions
    {
        public static double[][] RotateMartixClockwise(double[][] matrix)
        {
            if (!Matrix.IsMatrix(matrix))
                throw new ArgumentException("Input argument is not a matrix!");

            int m = matrix.Length;
            int n = matrix[0].Length;
            double[][] result = new double[n][];
            for (int i = 0; i < n; i++)
                result[i] = new double[m];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    result[j][i] = matrix[m - 1 - i][j];
            }

            return result;
        }
    }
}
