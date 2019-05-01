using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    /// <summary>
    /// The StringSearchBase class finds the first occurace of a pattern string in a text string.
    /// </summary>
    public abstract class StringSearchBase
    {
        /// <summary>
        /// The Radix.
        /// </summary>
        protected const int R = 256;

        /// <summary>
        /// Store the pattern as a string.
        /// </summary>
        protected string sPattern;

        /// <summary>
        /// Store the pattern as a character array.
        /// </summary>
        protected char[] cPattern;

        /// <summary>
        /// Returnns the index of the first occurance of the pattern string,
        /// length of the pattern string if no such match.
        /// </summary>
        /// <param name="text">The text string.</param>
        /// <returns>
        /// The index of the first occurance of the pattern string,
        /// length of the pattern string if no such match.
        /// </returns>
        public abstract int Search(string text);

        /// <summary>
        /// Returnns the index of the first occurance of the pattern string,
        /// length of the pattern string if no such match.
        /// </summary>
        /// <param name="text">The text string.</param>
        /// <returns>
        /// The index of the first occurance of the pattern string,
        /// length of the pattern string if no such match.
        /// </returns>
        public abstract int Search(char[] text);
    }
}