using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.DirectedGraph
{
    using Search;

    /// <summary>
    /// The SymbolDigraph class represents a digrpah, where the vertex names are artribray strings.
    /// </summary>
    public class SymbolDigraph
    {
        // String -> index.
        private ISymbolTable<string, int> st;

        // Index -> string.
        private string[] keys;

        private Digraph G;

        /// <summary>
        /// Returns the digraph associated with the symbol digraph.
        /// It is the client's resposibility not to mutate the digraph.
        /// </summary>
        public Digraph Digraph
        {
            get { return G; }
        }

        /// <summary>
        /// Initializes a digraph from a file using the specified delimiter.
        /// Each line in the file contains the name of a vertex, followed by a list of names of the vertices adjacent to that vertex, separated by the delimiter.
        /// </summary>
        /// <param name="fullFileName">The full file name of the file which stores the symbol digrpah.</param>
        /// <param name="delimiter">The delimiter between fields.</param>
        public SymbolDigraph(string fullFileName, string delimiter)
        {
            // Initialize the symbol table to store the <string, int> KVP.
            st = new SeparateChainingHashTable<string, int>();

            // Read symbol digraph stored in the file as lines of strings.
            string[] lines = System.IO.File.ReadAllLines(fullFileName);

            // Split lines into words by delimiter.
            string[][] words = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
                words[i] = System.Text.RegularExpressions.Regex.Split(lines[i], delimiter);

            // First pass, build the index by reading string to associated each distinct string with an index.
            foreach (string[] line in words)
            {
                foreach (string word in line)
                {
                    if (!st.ContainsKey(word))
                        st.Add(word, st.Size());
                }
            }

            // Inverted index to get string keys in an array.
            keys = new string[st.Size()];
            foreach (var kvp in st.GetKeyValuePairs())
                keys[kvp.Value] = kvp.Key;

            // Second pass build the graph by connecting first vertex on each line to all others.
            G = new Digraph(st.Size());
            foreach (string[] line in words)
            {
                int v = st[line[0]];
                for (int i = 1; i < line.Length; i++)
                    G.AddEdge(v, st[line[i]]);
            }
        }

        /// <summary>
        /// Returns the integer associated with the vertex with specified name.
        /// </summary>
        /// <param name="name">The name of the vertex.</param>
        /// <returns>The integer associated with the vertex with specified name.</returns>
        public int IndexOf(string name) { return st[name]; }

        /// <summary>
        /// Returns the name of the vertex associated with the integer v.
        /// </summary>
        /// <param name="v">The integer corresponding to a vertex between 0 and V-1.</param>
        /// <returns>The name of the vertex associated with the integer v.</returns>
        public string NameOf(int v) { return keys[v]; }

        /// <summary>
        /// Return true if the digraph contains the vertex with specified name, false otherwise.
        /// </summary>
        /// <param name="name">The specified name.</param>
        /// <returns>Return true if the digraph contains the vertex with specified name, false otherwise.</returns>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name) { return st.ContainsKey(name); }
    }
}