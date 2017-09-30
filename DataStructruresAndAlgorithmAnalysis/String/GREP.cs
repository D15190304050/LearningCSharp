using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.String
{
    /// <summary>
    /// The GREP (short for Globally search a Regular Expression and Print) provides a client for reading a sequence
    /// of lines from file and printing to standard output those lines that contain a sub-string matching a specified
    /// regular expression.
    /// </summary>
    public static class GREP
    {
        /// <summary>
        /// The file contains a regular expression, and following with text to search from.
        /// This regular expression supports closure, binary or, parentheses, and wildchar.
        /// Reads in lines from file, writes to standard output those lines that contains a sub-string matching the
        /// the regular expression.
        /// </summary>
        public static void Grep(string fullFilePath)
        {
            // Read all lines from file.
            string[] lines = System.IO.File.ReadAllLines(fullFilePath);

            // Index of line which is been accessed, increased by 1 every time read one.
            int currentIndex = 0;

            // Create the regular expression pattern.
            string regex = "(.*" + lines[currentIndex++] + ".*)";

            NFA nfa = new NFA(regex);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (nfa.Recognizes(line))
                    Console.WriteLine(line);
            }
        }
    }
}