using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    using BasicDataStructures;

    /// <summary>
    /// The TernarySearchTrie class represents a symbol table of key-value pairs, with string keys and generic keys.
    /// </summary>
    /// <typeparam name="TValue">The value type of the key-value pairs.</typeparam>
    public class TernarySearchTrie<TValue>
    {
        private class Node
        {
            /// <summary>
            /// Character.
            /// </summary>
            public char C { get; set; }

            /// <summary>
            /// Left sub-trie.
            /// </summary>
            public Node Left { get; set; }

            /// <summary>
            /// Middle sub-trie.
            /// </summary>
            public Node Middle { get; set; }

            /// <summary>
            /// Right sub-trie.
            /// </summary>
            public Node Right { get; set; }

            /// <summary>
            /// Value associated with string.
            /// </summary>
            public TValue Value { get; set; }

            /// <summary>
            /// Create a node of character c.
            /// </summary>
            /// <param name="c">The character.</param>
            public Node(char c)
            {
                C = c;
                Left = Middle = Right = null;
            }
        }

        /// <summary>
        /// The number of key-value pairs in this symbol table.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Get or set the value associated with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        public TValue this[string key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("Key mustn't be null.");
                if (key.Length == 0)
                    throw new ArgumentException("Key must have length >= 1.");

                Node target = CatchNode(root, key, 0);
                if (target == null)
                    return default(TValue);
                else
                    return target.Value;
            }

            set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Root of symbol ternary search trie.
        /// </summary>
        private Node root;

        /// <summary>
        /// Initializes an emtpy string symbol table of ternary search trie.
        /// </summary>
        public TernarySearchTrie()
        {
            Size = 0;
            root = null;
        }

        /// <summary>
        /// Returns the sub-trie corresponding to the given key.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="key">The key.</param>
        /// <param name="index">The index of string to compare.</param>
        /// <returns>The sub-trie corresponding to the given key.</returns>
        private Node CatchNode(Node current, string key, int index)
        {
            if (current == null)
                return null;

            char c = key[index];
            if (c < current.C)
                return CatchNode(current.Left, key, index);
            else if (c > current.C)
                return CatchNode(current.Right, key, index);
            else if (index < key.Length - 1)
                return CatchNode(current.Middle, key, index + 1);
            return current;
        }

        /// <summary>
        /// Returns true if this symbol table contains the key, false otherwise.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if this symbol table contains the key, false otherwise.</returns>
        public bool Contains(string key)
        {
            return CatchNode(root, key, 0) != null;
        }

        /// <summary>
        /// Inserts the key-value pair into the symbol table,
        /// over-writing the old value with the new value if the key is already in the symbol table.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index of string to compare.</param>
        /// <returns>Node current.</returns>
        private Node Add(Node current, string key, TValue value, int index)
        {
            char c = key[index];
            if (current == null)
                current = new Node(c);

            if (c < current.C)
                current.Left = Add(current.Left, key, value, index);
            else if (c > current.C)
                current.Right = Add(current.Right, key, value, index);
            else if (index < key.Length - 1)
                current.Middle = Add(current.Middle, key, value, index + 1);
            else
                current.Value = value;

            return current;
        }

        /// <summary>
        /// Inserts the key-value pair into this symbol table,
        /// over-writing the old value with the new value if the key is already in the symbol table.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, TValue value)
        {
            // Null check is not needed here because it will be excuted in Contains().
            if (!Contains(key))
                Size++;
            root = Add(root, key, value, 0);
        }

        /// <summary>
        /// Returns the string in the symbol table that is longest prefix of query, null if no such string.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <returns>The string in the symbol table that is longest prefix of query, null if no such string.</returns>
        public string LongestPrefixOf(string query)
        {
            if ((query == null) || (query.Length == 0))
                return null;

            int length = 0;
            Node current = root;
            int index = 0;

            while ((current != null) && (index < query.Length))
            {
                char c = query[index];
                if (c < current.C)
                    current = current.Left;
                else if (c > current.C)
                    current = current.Right;
                else
                {
                    index++;
                    if (!current.Value.Equals(default(TValue)))
                        length = index;
                    current = current.Middle;
                }
            }

            return query.Substring(0, length);
        }

        /// <summary>
        /// Collect all keys in the symbol table that matches the prefix.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="results">The queue to store the keys.</param>
        private void Collect(Node current, StringBuilder prefix, Queue<string> results)
        {
            if (current == null)
                return;

            Collect(current.Left, prefix, results);
            if (!current.Value.Equals(default(TValue)))
                results.Enqueue(prefix.ToString() + current.C);
            Collect(current.Middle, prefix.Append(current.C), results);
            prefix.Remove(prefix.Length - 1, 1);
            Collect(current.Right, prefix, results);
        }

        /// <summary>
        /// Returns all of the keys in the symbol table that start with prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>All of the keys in the symbol table that start with prefix.</returns>
        public IEnumerable<string> KeysWithPrefix(string prefix)
        {
            Queue<string> results = new Queue<string>();
            Node start = CatchNode(root, prefix, 0);
            if (start == null)
                return results;

            if (!start.Value.Equals(default(TValue)))
                results.Enqueue(prefix);
            Collect(start.Middle, new StringBuilder(prefix), results);
            return results;
        }

        /// <summary>
        /// Returns all keys in the symbol table as an enumerator.
        /// </summary>
        /// <returns>All keys in the symbol table as an enumerator.</returns>
        public IEnumerable<string> Keys()
        {
            Queue<string> results = new Queue<string>();
            Collect(root, new StringBuilder(), results);
            return results;
        }

        /// <summary>
        /// Collect all of the keys in the symbol table that match the pattern.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="index">The index of string to compare.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="results">All of the keys in the symbol table that matches the pattern.</param>
        private void Collect(Node current, StringBuilder prefix, int index, string pattern, Queue<string> results)
        {
            if (current == null)
                return;

            char c = pattern[index];
            if ((c == '.') || (c < current.C))
                Collect(current.Left, prefix, index, pattern, results);
            if ((c == '.') || (c == current.C))
            {
                if ((index == pattern.Length - 1) && (!current.Value.Equals(default(TValue))))
                    results.Enqueue(prefix.ToString() + current.C);
                if (index < pattern.Length - 1)
                {
                    Collect(current.Middle, prefix.Append(current.C), index + 1, pattern, results);
                    prefix.Remove(prefix.Length - 1, 1);
                }
            }
            if ((c == '.') || (c > current.C))
                Collect(current.Right, prefix, index, pattern, results);
        }

        /// <summary>
        /// Returns all of the keys in the symbol table that match the pattern, where '.' is treated as a wildcard character.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>All of the keys in the symbol table that match the pattern, where '.' is treated as a wildcard character.</returns>
        public IEnumerable<string> KeysThatMatch(string pattern)
        {
            Queue<string> result = new Queue<string>();
            Collect(root, new StringBuilder(), 0, pattern, result);
            return result;
        }
    }
}