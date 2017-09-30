using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Search
{
    public static class FrequencyCounter
    {
        public static void MaxFrequency(string fileName, int minLength, ISymbolTable<string, int> st)
        {
            // Read all content from the file.
            string text = System.IO.File.ReadAllText(fileName);

            // Split the text into words.
            string[] words = System.Text.RegularExpressions.Regex.Split(text, "\\s+");

            // Build symbol table and count frequencies.
            foreach (string word in words)
            {
                // Igonre short keys.
                if (word.Length < minLength)
                    continue;

                // Add the word to symbol table if doesn't exist.
                if (!st.ContainsKey(word))
                    st.Add(word, 1);

                // Else, increase the frequency counter.
                else
                    st[word]++;
            }

            // Find a key with the highest frequency count.
            string max = "";
            st.Add(max, 0);
            foreach (string word in st.Keys())
            {
                if (st[word] > st[max])
                    max = word;
            }

            Console.WriteLine("The max word is {0}, and its frequency is {1}.", max, st[max]);
        }
    }
}