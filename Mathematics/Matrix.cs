using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class Matrix
    {
        /// <summary>
        /// Returns the element-wise product of 2 vectors.
        /// </summary>
        /// <param name="vector1">A vector.</param>
        /// <param name="vector2">The other vector.</param>
        /// <returns>The element-wise product of 2 vectors.</returns>
        public static double[] ElementWiseProduct(double[] vector1, double[] vector2)
        {
            if ((vector1 == null) || vector2 == null)
                throw new ArgumentNullException("Input vectors must not be null.");

            // Valid the length of 2 vectors.
            // Throw an exception if they do not have the same length.
            LengthEqual(vector1, vector2);

            int length = vector1.Length;
            double[] result = new double[length];
            for (int i = 0; i < length; i++)
                result[i] = vector1[i] * vector2[i];

            return result;
        }

        /// <summary>
        /// Returns the input matrix's transpose.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>The input matrix's transpose.</returns>
        public static double[][] Transpose(double[][] matrix)
        {
            // Valid if matrix is a real MATRIX or a jagged array.
            // Throw an exception if it is not a real Matrix.
            if (!IsMatrix(matrix))
                throw new ArgumentException("The input parameter is not a matrix.");

            int m = matrix.Length;
            int n = matrix[0].Length;

            // Transpose the matrix.
            double[][] result = new double[n][];
            for (int i = 0; i < n; i++)
            {
                result[i] = new double[m];
                for (int j = 0; j < m; j++)
                    result[i][j] = matrix[j][i];
            }

            return result;
        }

        /// <summary>
        /// Returns vector[startIndex ... (endIndex - 1)] as a double[].
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <param name="startIndedx">Start index of the sub vector.</param>
        /// <param name="endIndex">End index of the sub vector.</param>
        /// <returns>vector[startIndex ... (endIndex - 1)] as a double[].</returns>
        public static double[] GetSubVector(double[] vector, int startIndedx, int endIndex)
        {
            if (vector == null)
                throw new ArgumentNullException("The input vector must not be null.");

            if ((startIndedx < 0) || (startIndedx >= endIndex) || (endIndex > vector.Length))
                throw new ArgumentOutOfRangeException("Parameters does not satisfy condition \"0 <= startIndex < endIndex < vector.Length\".");

            double[] subVector = new double[endIndex - startIndedx];
            for (int i = 0; i < subVector.Length; i++)
                subVector[i] = vector[i + startIndedx];
            return subVector;
        }

        /// <summary>
        /// Computes and returns the product of the matrix-vector multiplication.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <param name="vector">The input vector.</param>
        /// <returns>The product of the matrix-vector multiplication.</returns>
        public static double[] MatrixVectorMultiplication(double[][] matrix, double[] vector)
        {
            if ((matrix == null) || (vector == null))
                throw new ArgumentNullException("Input parameters must not be null.");

            // Valid if matrix is a real MATRIX or a jagged array.
            // Throw an exception if it is not a real Matrix.
            if (!IsMatrix(matrix))
                throw new ArgumentException("The input parameter is not a matrix.");

            int m = matrix.Length;
            int n = matrix[0].Length;

            if (n != vector.Length)
                throw new ArgumentException("The number of columns of the matrix is not equal to the length of the vector.");

            double[] result = new double[m];
            for (int i = 0; i < m; i++)
                result[i] = VectorDotMultiplication(matrix[i], vector);

            return result;
        }

        /// <summary>
        /// Returns the product of 2 vectors.
        /// </summary>
        /// <param name="vector1">A vector.</param>
        /// <param name="vector2">The other vector.</param>
        /// <returns>The product of 2 vectors.</returns>
        public static double VectorDotMultiplication(double[] vector1, double[] vector2)
        {
            if ((vector1 == null) || (vector2 == null))
                throw new ArgumentNullException("Input vectors must not be null.");

            LengthEqual(vector1, vector2);

            double product = 0;
            for (int i = 0; i < vector1.Length; i++)
                product += vector1[i] * vector2[i];

            return product;
        }

        /// <summary>
        /// Returns true if 2 input vectors are equal, false otherwise.
        /// </summary>
        /// <param name="vector1">An input vector.</param>
        /// <param name="vector2">The other input vector.</param>
        /// <returns>True if 2 input vectors are equal, false otherwis</returns>
        public static bool VectorEquals(int[] vector1, int[] vector2)
        {
            if ((vector1 == null) || (vector2 == null))
                throw new AggregateException("Input vectors must not be null.");
            LengthEqual(vector1, vector2);

            for (int i = 0; i < vector1.Length; i++)
            {
                if (vector1[i] != vector2[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a vector that has the same elements as the input matrix.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>A vector that have the same elements as the matrix.</returns>
        public static double[] Vectorize(double[][] matrix)
        {
            // Valid if matrix is a real MATRIX or a jagged array.
            // Throw an exception if it is not a real Matrix.
            if (!IsMatrix(matrix))
                throw new ArgumentException("The input parameter is not a matrix.");

            int rowCount = matrix.Length;
            int columnCount = matrix[0].Length;
            double[] vector = new double[rowCount * columnCount];

            int i = 0;
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                    vector[i++] = matrix[row][column];
            }

            return vector;
        }

        /// <summary>
        /// Returns a vector that has the same elements as the input matrix.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>A vector that have the same elements as the matrix.</returns>
        public static byte[] Vectorize(byte[][] matrix)
        {
            // Valid if matrix is a real MATRIX or a jagged array.
            // Throw an exception if it is not a real Matrix.
            if (!IsMatrix(matrix))
                throw new ArgumentException("The input parameter is not a matrix.");

            int rowCount = matrix.Length;
            int columnCount = matrix[0].Length;
            byte[] vector = new byte[rowCount * columnCount];

            int i = 0;
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                    vector[i++] = matrix[row][column];
            }

            return vector;
        }

        /// <summary>
        /// Returns a matrix that has the same elements as the input matrix with its values are double.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>A matrix that has the same elements as the input matrix with its values are double.</returns>
        public static double[][] ToDouble(byte[][] matrix)
        {
            // Valid if matrix is a real MATRIX or a jagged array.
            // Throw an exception if it is not a real Matrix.
            if (!IsMatrix(matrix))
                throw new ArgumentException("The input parameter is not a matrix.");

            int rowCount = matrix.Length;
            int columnCount = matrix[0].Length;
            double[][] doubleMatrix = new double[matrix.Length][];
            for (int i = 0; i < rowCount; i++)
            {
                doubleMatrix[i] = new double[columnCount];
                for (int j = 0; j < columnCount; j++)
                    doubleMatrix[i][j] = matrix[i][j];
            }

            return doubleMatrix;
        }

        /// <summary>
        /// Returns a vector that computed by subtracting the input vector from the input scalar.
        /// </summary>
        /// <param name="scalar">The input scalar.</param>
        /// <param name="vector">The input vector.</param>
        /// <returns>A vector that computed by subtracting the input vector from the input scalar.</returns>
        public static double[] ScalarSubtractVector(double scalar, double[] vector)
        {
            if (vector == null)
                throw new ArgumentNullException("The input vector must not be null.");

            double[] result = new double[vector.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = scalar - vector[i];

            return result;
        }

        /// <summary>
        /// Returns true if the input 2-D array has the shape of a matrix, false otherwise.
        /// </summary>
        /// <param name="array">The input 2-D array.</param>
        /// <returns>True if the input 2-D array has the shape of a matrix, false otherwise.</returns>
        public static bool IsMatrix(double[][] array)
        {
            if ((array == null) || (array[0] == null))
                return false;

            int rowCount = array.Length;
            int columnCount = array[0].Length;
            for (int i = 1; i < rowCount; i++)
            {
                if ((array[i] == null) || (array[i].Length != columnCount))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the input 2-D array has the shape of a matrix, false otherwise.
        /// </summary>
        /// <param name="array">The input 2-D array.</param>
        /// <returns>True if the input 2-D array has the shape of a matrix, false otherwise.</returns>
        public static bool IsMatrix(byte[][] array)
        {
            if ((array == null) || (array[0] == null))
                return false;

            int rowCount = array.Length;
            int columnCount = array[0].Length;
            for (int i = 1; i < rowCount; i++)
            {
                if ((array[i] == null) || (array[i].Length != columnCount))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Throw an exception if 2 input vectors don't have the same length.
        /// </summary>
        /// <param name="vector1">An input vector.</param>
        /// <param name="vector2">The other input vector.</param>
        private static void LengthEqual(double[] vector1, double[] vector2)
        {
            if (vector1.Length != vector2.Length)
                throw new AggregateException("Input vectors must have the same length.");
        }

        /// <summary>
        /// Throw an exception if 2 input vectors don't have the same length.
        /// </summary>
        /// <param name="vector1">An input vector.</param>
        /// <param name="vector2">The other input vector.</param>
        private static void LengthEqual(int[] vector1, int[] vector2)
        {
            if (vector1.Length != vector2.Length)
                throw new AggregateException("Input vectors must have the same length.");
        }
    }
}