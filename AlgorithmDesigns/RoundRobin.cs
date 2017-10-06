using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    /// <summary>
    /// The RoundRobin class provides a static method for solving the round robin problem.
    /// </summary>
    public static class RoundRobin
    {
        /// <summary>
        /// Arrange the round robin for every player.
        /// </summary>
        /// <param name="numPlayers">The number of players.</param>
        /// <returns>An acceptable arrangement.</returns>
        public static int[][] Arrange(int numPlayers)
        {
            // Throw an exception if the number of players is not equal to 2^k, where k is a positive integer.
            if (!CanArrange(numPlayers))
                throw new ArgumentException("The number of players must be 2^k, where k is a positive integer.");

            // Generate an empty matrix.
            int[][] arrangement = GetEmptyMatrix(numPlayers, numPlayers);

            // Initialize the first column of the arrangement table.
            InitializeArrangement(arrangement);

            // Fill the blanks of the rest of the arrangement table.
            Arrange(arrangement, 0, 0, numPlayers - 1, numPlayers - 1);

            // Return the arrangement table.
            return arrangement;
        }

        /// <summary>
        /// Returns true if the number of players is equal to 2^k, false otherwise, where k is a positive integer.
        /// </summary>
        /// <param name="numPlayers">The number of players.</param>
        /// <returns>True if the number of players is equal to 2^k, false otherwise, where k is a positive integer.</returns>
        private static bool CanArrange(int numPlayers)
        {
            // Initialize i to be 2^0.
            int i = 1;

            // We set the upper limit of the number of players equal to 2^29.
            const int upperLimit = 1 << 29;

            // Loop until i is larger than 2^29.
            while (i < upperLimit)
            {
                // Returns true if there is some positive integer k, 2^k == numPlayers.
                if (i == numPlayers)
                    return true;

                // Let i equal to its twice.
                i <<= 1;
            }

            // Return false if no such k.
            return false;
        }

        /// <summary>
        /// Generate an empty matrix with specified number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns</param>
        /// <returns>An empty matrix with specified number of rows and columns.</returns>
        private static int[][] GetEmptyMatrix(int rows, int columns)
        {
            int[][] matrix = new int[columns][];
            for (int i = 0; i < columns; i++)
                matrix[i] = new int[rows];

            return matrix;
        }

        /// <summary>
        /// Let the first column to be the number of the player himself, numbers starts from 1 (instead of 0).
        /// </summary>
        /// <param name="arrangement">The arrangement table.</param>
        private static void InitializeArrangement(int[][] arrangement)
        {
            for (int i = 0; i < arrangement.Length; i++)
                arrangement[i][0] = i + 1;
        }

        /// <summary>
        /// Arrange round robins for players[startY ... endY] on day[startX ... endX].
        /// </summary>
        /// <param name="arrangement">The arrangement table.</param>
        /// <param name="startX">The lower limit of the days for round robins.</param>
        /// <param name="startY">The lower limit of the players to arrange.</param>
        /// <param name="endX">The upper limit of the days for round robins.</param>
        /// <param name="endY">The upper limit of the players to arrange.</param>
        private static void Arrange(int[][] arrangement, int startX, int startY, int endX, int endY)
        {
            // Returns if there is only 1 day to arrange.
            if (endX == startX)
                return;

            // Get the middle index of the sub-arrangement table.
            int middleX = (endX - 1 + startX) / 2;
            int middleY = (endY - 1 + startY) / 2;

            // Arrange the upper left corner of the sub-arrangement table.
            Arrange(arrangement, startX, startY, middleX, middleY);

            // Arrange the bottom left corner of the sub-arrangement table.
            Arrange(arrangement, startX, middleY + 1, middleX, endY);

            // Fill the right half the the sub-arrangement table.
            CopySubArea(arrangement, startX, startY, middleX, middleY, endX, endY);
        }

        /// <summary>
        /// Fill the right half the the sub-arrangement table.
        /// </summary>
        /// <param name="arrangement">The arrangement table.</param>
        /// <param name="startX">The lower limit of the days for round robins.</param>
        /// <param name="startY">The lower limit of the players to arrange.</param>
        /// <param name="middleX">The upper limit of the days for round robins on the left half.</param>
        /// <param name="middleY">The upper limit of the players to arrange on the left half.</param>
        /// <param name="endX">The upper limit of the days for round robins.</param>
        /// <param name="endY">The upper limit of the players to arrange.</param>
        private static void CopySubArea(int[][] arrangement, int startX, int startY, int middleX, int middleY, int endX, int endY)
        {
            // Compute the length of the copy operation for a corner.
            int copyLength = middleX - startX + 1;

            // Copy arrangement[startY ... endY][startX ... middleX] to arrangement[middleY+1 ... endY][middleX+1 ... endX].
            for (int i = 0; i < copyLength; i++)
            {
                for (int j = 0; j < copyLength; j++)
                    arrangement[middleY + 1 + i][middleX + 1 + j] = arrangement[startY + i][startX + j];
            }

            // Copy arrangement[middleY+1 ... endY][startX ... middleX] to arrangement[startY ... middleY][middleX+1 ... endX].
            for (int i = 0; i < copyLength; i++)
            {
                for (int j = 0; j < copyLength; j++)
                    arrangement[startY + i][middleX + 1 + j] = arrangement[middleY + 1 + i][startX + j];
            }
        }
    }
}