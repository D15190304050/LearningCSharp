using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mathematics
{
    internal class UnitTest
    {
        public static void UnitTestForMatrix()
        {
            Console.WriteLine("Test for HadamardProduct(double[], double[])");
            double[] vector1 = { 2, 3, 5 };
            double[] vector2 = { 4, 5, 9 };
            foreach (double d in Matrix.HadamardProduct(vector1, vector2))
                Console.Write(d + " ");
            Console.WriteLine("\n");

            Console.WriteLine("Test for Transpose(double[][])");
            double[][] matrix = new double[2][];
            matrix[0] = vector1;
            matrix[1] = vector2;
            double[][] transposedMatrix = Matrix.Transpose(matrix);
            foreach (double[] row in transposedMatrix)
            {
                foreach (double d in row)
                    Console.Write(d + " ");
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Test for GetSubVector(double[], int, int)");
            double[] vector3 = { 1, 2, 3, 4, 5, 6, 7 };
            foreach (double d in Matrix.GetSubVector(vector3, 2, 7))
                Console.Write(d + " ");
            Console.WriteLine("\n");

            Console.WriteLine("Test for VectorDotMultiplication(double[], double[])");
            Console.WriteLine(Matrix.VectorDotMultiplication(vector1, vector2) + "\n");

            Console.WriteLine("Test for MatrixVectorMultiplication(double[][], double[])");
            double[] vector4 = Matrix.MatrixVectorMultiplication(matrix, vector1);
            foreach (double d in vector4)
                Console.Write(d + " ");
            Console.WriteLine("\n");

            Console.WriteLine("Test for Vectorize(double[][])");
            foreach (double d in Matrix.Vectorize(matrix))
                Console.Write(d + " ");
            Console.WriteLine("\n");

            Console.WriteLine("Test for ScalarSubtractVector(double, double[])");
            foreach (double d in Matrix.ScalarSubtractVector(5, vector3))
                Console.Write(d + " ");
            Console.WriteLine("\n");
        }
        
    }
}