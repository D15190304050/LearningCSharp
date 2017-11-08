using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.UndirectedGraph
{
    using Search;

    /// <summary>
    /// The SymbolGraph class represents an un-directed graph, where the vertex names are arbitrary strings.
    /// </summary>
    public class SymbolGraph
    {
        // String -> index.
        private ISymbolTable<string, int> st;

        // index -> string.
        private string[] keys;

        // The numeric graph of the symbol graph.
        private Graph G;

        /// <summary>
        /// Return the graph associated with the symbol graph.
        /// It's client's responsibility not to mutate the graph.
        /// </summary>
        public Graph Graph
        {
            get { return G; }
        }

        /// <summary>
        /// Initialize a graph from a file using the specified delimiter.
        /// Each in the file contains the name of a vertex, followed by a list of names of the vertices adjacent to that vertex, separated by the delimiter.
        /// </summary>
        /// <param name="fileName">The name of the file which stores the graph.</param>
        /// <param name="delimiter">The delimiter between fields.</param>
        public SymbolGraph(string fileName, string delimiter)
        {
            // Use separate chaining hash table becase it can save 0 as the value in KVP.
            st = new SeparateChainingHashTable<string, int>();

            // Read symbol graph stored in the file as lines of strings.
            string[] lines = System.IO.File.ReadAllLines(fileName);

            // Split lines into words by delimiter.
            string[][] words = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
                words[i] = System.Text.RegularExpressions.Regex.Split(lines[i], delimiter);

            // First pass build the index by reading strings to associated each distinct string with an index.
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
            G = new Graph(st.Size());
            foreach (string[] line in words)
            {
                int v = st[line[0]];
                for (int i = 1; i < line.Length; i++)
                {
                    int w = st[line[i]];
                    G.AddEdge(v, w);
                }
            }
        }

        /// <summary>
        /// Return true if the graph contains the vertex with specified name, false otherwise.
        /// </summary>
        /// <param name="name">The specified name.</param>
        /// <returns>Return true if the graph contains the vertex with specified name, false otherwise.</returns>
        public bool Contains(string name) { return st.ContainsKey(name); }

        /// <summary>
        /// Return the integer associated with the vertex with specified name.
        /// </summary>
        /// <param name="name">The specified name.</param>
        /// <returns>Returns the integer associated with the vertex with specified name.</returns>
        public int IndexOf(string name) { return st[name]; }

        /// <summary>
        /// Return the name of the vertex associated with integer v.
        /// </summary>
        /// <param name="v">The integer corresponding to a vertex (between 0 and V-1).</param>
        /// <returns>Return the name of the vertex associated with integer v.</returns>
        public string NameOf(int v) { return keys[v]; }
    }
}
