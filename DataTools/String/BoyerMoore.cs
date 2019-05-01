using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    public class BoyerMoore : StringSearchBase
    {
        /// <summary>
        /// The bad character skip array.
        /// </summary>
        private int[] right;

        /// <summary>
        /// Process the pattern string.
        /// </summary>
        /// <param name="pattern">The pattern string.</param>
        public BoyerMoore(string pattern)
        {
            sPattern = pattern;
            cPattern = pattern.ToCharArray();

            // Position of the right-most occurance of c in the pattern.
            right = new int[R];
            for (int c = 0; c < R; c++)
                right[c] = -1;
            for (int j = 0; j < pattern.Length; j++)
                right[pattern[j]] = j;
        }

        /// <summary>
        /// Process the pattern string.
        /// </summary>
        /// <param name="pattern">The pattern string.</param>
        public BoyerMoore(char[] pattern)
        {
            int patternLength = pattern.Length;
            sPattern = new string(pattern);
            cPattern = pattern;
            for (int i = 0; i < patternLength; i++)
                cPattern[i] = pattern[i];

            // Position of the right-most occurance of c in the pattern.
            right = new int[R];
            for (int c = 0; c < R; c++)
                right[c] = -1;
            for (int i = 0; i < patternLength; i++)
                right[pattern[i]] = i;
        }

        /// <summary>
        /// Returns the index of the first occurance of the pattern string in the text string,
        /// length of the string if no such match.
        /// </summary>
        /// <param name="text">The text string.</param>
        /// <returns>
        /// The index of the first occurance of the pattern string in the text string,
        /// length of the string if no such match.
        /// </returns>
        public override int Search(string text)
        {
            int patternLength = sPattern.Length;
            int textLength = text.Length;
            int skip;

            for (int i = 0; i <= textLength - patternLength; i += skip)
            {
                skip = 0;
                for (int j = patternLength - 1; j >= 0; j--)
                {
                    if (sPattern[j] != text[i + j])
                    {
                        skip = Math.Max(1, j - right[text[i + j]]);
                        break;
                    }
                }

                // Found.
                if (skip == 0)
                    return i;
            }
            
            // Not found.
            return textLength;
        }

        /// <summary>
        /// Returns the index of the first occurance of the pattern string in the text string,
        /// length of the string if no such match.
        /// </summary>
        /// <param name="text">The text string.</param>
        /// <returns>
        /// The index of the first occurance of the pattern string in the text string,
        /// length of the string if no such match.
        /// </returns>
        public override int Search(char[] text)
        {
            int patternLength = cPattern.Length;
            int textLength = text.Length;
            int skip;

            for (int i = 0; i < textLength - patternLength; i += skip)
            {
                skip = 0;
                for (int j = patternLength - 1; j >= 0; j--)
                {
                    if (cPattern[j] != text[i + j])
                    {
                        skip = Math.Max(1, j - right[text[i + j]]);
                        break;
                    }
                }

                // Found.
                if (skip == 0)
                    return i;
            }

            // Not found.
            return textLength;
        }
    }
}