using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.String
{
    using Sort;

    /// <summary>
    /// The Huffman class provides static methods for compressing and expanding a bianry input using Huffman codes over the 8-bit extended ASCII alphabet.
    /// </summary>
    public static class Huffman
    {
        /// <summary>
        /// Alphabet size of extended ASCII.
        /// </summary>
        private const int R = 256;

        private class Node : IComparable<Node>
        {
            /// <summary>
            /// The character stored in this node.
            /// </summary>
            public char C { get; set; }

            /// <summary>
            /// The frequency of this word.
            /// </summary>
            public int Frequency { get; set; }

            /// <summary>
            /// Left branch of this node.
            /// </summary>
            public Node Left { get; set; }

            /// <summary>
            /// Right branch of this node.
            /// </summary>
            public Node Right { get; set; }

            /// <summary>
            /// Returns true if this node is a leaf node, false otherwise.
            /// </summary>
            public bool IsLeaf
            {
                get { return ((Left == null) && (Right == null)); }
            }

            /// <summary>
            /// Construct a node of this Huffman trie.
            /// </summary>
            /// <param name="c">The character to store in this node.</param>
            /// <param name="frequency">The frequency of this word.</param>
            /// <param name="left">Left branch of this node.</param>
            /// <param name="right">Right branch of this node.</param>
            public Node(char c, int frequency, Node left, Node right)
            {
                C = c;
                Frequency = frequency;
                Left = left;
                Right = right;
            }

            /// <summary>
            /// Compare, based on frequency.
            /// </summary>
            /// <param name="that">The node to compare to this node.</param>
            /// <returns>The value of this.Frequency - that.Frequency.</returns>
            public int CompareTo(Node that)
            {
                return Frequency - that.Frequency;
            }
        }

        /// <summary>
        /// Reads a sequence of 8-bit bytes from standard input; compress them using Huffman codes with an 8-bit
        /// alphabet; and writes the results to standard output.
        /// </summary>
        public static void Compress()
        {
            // Read the input.
            string s = BinaryStandardInput.ReadString();
            char[] input = s.ToCharArray();

            // Tabulate frequency counts.
            int[] frequencies = new int[R];
            for (int i = 0; i < input.Length; i++)
                frequencies[input[i]]++;

            // Build Huffman trie.
            Node root = BuildTrie(frequencies);

            // Build code table.
            string[] st = new string[R];
            BuildCode(st, root, "");

            // Print the trie for decoder.
            WriteTrie(root);

            // Print number of bytes in original un-compressed message.
            BinaryStandardOutput.Write(input.Length);

            // Use Huffman code to encode input.
            for (int i = 0; i < input.Length; i++)
            {
                string code = st[input[i]];
                for (int j = 0; j < code.Length; j++)
                {
                    char c = code[j];
                    if (c == '1')
                        BinaryStandardOutput.Write(true);
                    else if (c == '0')
                        BinaryStandardOutput.Write(false);
                    else
                        throw new InvalidOperationException("Illegal state.");
                }
            }
        }

        /// <summary>
        /// Build the Huffman trie by given frequencies.
        /// </summary>
        /// <param name="frequencies">The frequencies of all characters.</param>
        /// <returns>The root of the trie.</returns>
        private static Node BuildTrie(int[] frequencies)
        {
            // Initialize priority queue with singleton trees.
            MinPriorityQueue<Node> pq = new MinPriorityQueue<Node>();
            for (int i = 0; i < R; i++)
            {
                if (frequencies[i] > 0)
                    pq.Add(new Node((char)i, frequencies[i], null, null));
            }

            // Special case in case there is only one character with a non-zero frequency.
            if (pq.Size == 1)
            {
                if (frequencies[(int)'\0'] == 0)
                    pq.Add(new Node('\0', 0, null, null));
                else
                    pq.Add(new Node('1', 0, null, null));
            }

            // Merge 2 smallest trees.
            while (pq.Size > 1)
            {
                Node left = pq.DeleteMin();
                Node right = pq.DeleteMin();
                Node parent = new Node('\0', left.Frequency + right.Frequency, left, right);
                pq.Add(parent);
            }

            return pq.DeleteMin();
        }

        /// <summary>
        /// write bit-string-encoded trie to standard output.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        private static void WriteTrie(Node current)
        {
            if (current.IsLeaf)
            {
                BinaryStandardOutput.Write(true);
                BinaryStandardOutput.Write(current.C, 8);
                return;
            }

            BinaryStandardOutput.Write(false);
            WriteTrie(current.Left);
            WriteTrie(current.Right);
        }

        /// <summary>
        /// Make a lookup table from symbols and their encodings.
        /// </summary>
        /// <param name="st">The symbol table.</param>
        /// <param name="current">The next node to visit.</param>
        /// <param name="s">The code of current node.</param>
        private static void BuildCode(string[] st, Node current, string s)
        {
            if (!current.IsLeaf)
            {
                BuildCode(st, current.Left, s + "0");
                BuildCode(st, current.Right, "1");
            }
            else
                st[current.C] = s;
        }

        /// <summary>
        /// Reads a sequence of bits that represents a Huffman-compressed message from standard input;
        /// expands them; and writes the results to standard output.
        /// </summary>
        public static void Expand()
        {
            // Read in Huffman trie from input stream.
            Node root = ReadTrie();

            // Number of bytes to write.
            int length = BinaryStandardInput.ReadInt();

            // Decoding using the Huffman trie.
            for (int i = 0; i < length; i++)
            {
                Node current = root;
                while (!current.IsLeaf)
                {
                    bool bit = BinaryStandardInput.ReadBoolean();
                    if (bit)
                        current = current.Right;
                    else
                        current = current.Left;
                }
                BinaryStandardOutput.Write(current.C, 8);
            }
        }

        /// <summary>
        /// Read trie from standard input and returns its root.
        /// </summary>
        /// <returns>The root of trie read from standard input.</returns>
        private static Node ReadTrie()
        {
            bool isLeaf = BinaryStandardInput.ReadBoolean();
            if (isLeaf)
                return new Node(BinaryStandardInput.ReadChar(), -1, null, null);
            else
                return new Node('\0', -1, ReadTrie(), ReadTrie());
        }
    }
}