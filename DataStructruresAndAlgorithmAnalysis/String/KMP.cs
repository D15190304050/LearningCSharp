using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.String
{
    /// <summary>
    /// The KMP (Knuth-Morris-Pratt) class finds the first occurance of a pattern string in a text string.
    /// </summary>
    public class KMP : StringSearchBase
    {

        /// <summary>
        /// The KMP automatic machine.
        /// </summary>
        private int[][] dfa;

        /// <summary>
        /// Process the pattern string.
        /// </summary>
        /// <param name="pattern">The pattern string.</param>
        public KMP(string pattern)
        {
            sPattern = pattern;
            cPattern = pattern.ToCharArray();

            // Build DFA from pattern.
            int patternLength = pattern.Length;
            dfa = new int[R][];
            for (int i = 0; i < R; i++)
                dfa[i] = new int[patternLength];
            dfa[pattern[0]][0] = 1;
            for (int x = 0, j = 1; j < patternLength; j++)
            {
                // Copy mis-match cases.
                for (int c = 0; c < R; c++)
                    dfa[c][j] = dfa[c][x];

                // Set match cases.
                dfa[pattern[j]][j] = j + 1;

                // Update restart state.
                x = dfa[pattern[j]][x];
            }

        }

        /// <summary>
        /// Process the pattern string.
        /// </summary>
        /// <param name="pattern">The pattern string.</param>
        public KMP(char[] pattern)
        {
            int patternLength = pattern.Length;
            sPattern = new string(pattern);

            cPattern = new char[patternLength];
            for (int i = 0; i < patternLength; i++)
                cPattern[i] = pattern[i];

            // Build DFA from pattern.
            dfa = new int[R][];
            for (int i = 0; i < R; i++)
                dfa[i] = new int[patternLength];
            dfa[pattern[0]][0] = 1;
            for (int x = 0, j = 1; j < patternLength; j++)
            {
                // Copy mis-match cases.
                for (int c = 0; c < R; c++)
                    dfa[c][j] = dfa[c][x];

                // Set match cases.
                dfa[pattern[j]][j] = j + 1;

                // Update restart state.
                x = dfa[pattern[j]][x];
            }
        }

        /// <summary>
        /// Returns the index of the first occurance of the pattern string in the text.
        /// </summary>
        /// <param name="text">The text string.</param>
        /// <returns>The index of the first occurance of the pattern string in the text.</returns>
        public override int Search(string text)
        {
            // Simulate operation of DFA on text.
            int patternLength = sPattern.Length;
            int textLength = text.Length;
            int i, j;

            for (i = 0, j = 0; (i < textLength) && (j < patternLength); i++)
                j = dfa[text[i]][j];

            // Found.
            if (j == patternLength)
                return i - patternLength;

            // Not found.
            return textLength;
        }

        /// <summary>
        /// Returns the index of the first occurace of the pattern string in the text.
        /// </summary>
        /// <param name="text">The text string.</param>
        /// <returns>The index of the first occurace of the pattern string in the text.</returns>
        public override int Search(char[] text)
        {
            // Simulate operation of DFA on text.
            int patternLength = cPattern.Length;
            int textLength = text.Length;
            int i, j;

            for (i = 0, j = 0; (i < textLength) && (j < patternLength); i++)
                j = dfa[text[i]][j];

            // Found.
            if (j == patternLength)
                return i - patternLength;

            // Not found.
            return textLength;
        }
    }
}