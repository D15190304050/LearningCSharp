using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    using Collections;

    /// <summary>
    /// The TrieSymbolTable class represents a symbol table of key-value pairs, with string keys and generic values.
    /// This is better for class or Nullable type cause there are default value in codes.
    /// </summary>
    /// <typeparam name="TValue">The value type of the key-value pairs.</typeparam>
    public class TrieSymbolTable<TValue>
    {
        // Extended ASCII.
        private const int R = 256;

        /// <summary>
        /// R-way trie node.
        /// </summary>
        private class Node
        {
            /// <summary>
            /// The value of this node.
            /// </summary>
            public TValue Value { get; set; }

            /// <summary>
            /// The child nodes rooted at this node.
            /// </summary>
            public Node[] Next { get; set; }

            /// <summary>
            /// Create a new node.
            /// </summary>
            public Node()
            {
                Value = default(TValue);
                Next = new Node[R];
            }
        }

        /// <summary>
        /// Root of trie.
        /// </summary>
        private Node root;

        /// <summary>
        /// Number of keys in trie.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// True if this symbol table is empty, false otherwise.
        /// </summary>
        public bool IsEmtpy
        {
            get { return Size == 0; }
        }

        /// <summary>
        /// Returns the value associated with the specified key, default(TValue) if no such key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the specified key, default(TValue) if no such key.</returns>
        public TValue this[string key]
        {
            get
            {
                Node node = CatchNode(root, key, 0);
                if (node == null)
                    return default(TValue);
                return node.Value;
            }
        }

        /// <summary>
        /// Initializes an emtpy string symbol table.
        /// </summary>
        public TrieSymbolTable()
        {
            root = null;
            Size = 0;
        }

        /// <summary>
        /// Returns the node which hold the specified key, default(TValue) if no such node.
        /// </summary>
        /// <param name="node">The next node to visit.</param>
        /// <param name="key">The key to search.</param>
        /// <param name="index">The index of string to compare.</param>
        /// <returns>The node which hold the specified key, default(TValue) if no such node.</returns>
        private Node CatchNode(Node node, string key, int index)
        {
            if (node == null)
                return null;

            if (index == key.Length)
                return node;

            char c = key[index];
            return CatchNode(node.Next[c], key, index + 1);
        }

        /// <summary>
        /// Returns true if this symbol table contains the given key, false otherwise.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if this symbol table contains the given key, false otherwise.</returns>
        public bool Contains(string key)
        {
            return (!this[key].Equals(default(TValue)));
        }

        /// <summary>
        /// Adds the given key into this trie, and returns the node which is being visited.
        /// </summary>
        /// <param name="node">The next node to visit.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index of string to compare.</param>
        /// <returns>he node which is being visited now.</returns>
        private Node Add(Node node, string key, TValue value, int index)
        {
            if (node == null)
                node = new Node();

            if (index == key.Length)
            {
                if (node.Value.Equals(default(TValue)))
                    Size++;
                node.Value = value;
                return node;
            }

            char c = key[index];
            node.Next[c] = Add(node.Next[c], key, value, index + 1);

            return node;
        }

        /// <summary>
        /// Adds the key-value pair into the symbol table, overwriting the old value with the new value if the key is already in the symbol table.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value</param>
        public void Add(string key, TValue value)
        {
            root = Add(root, key, value, 0);
        }

        /// <summary>
        /// Collect the strings with specified prefix into a queue.
        /// </summary>
        /// <param name="node">The next node to visit.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="results">The queue to store the strings with specified prefix.</param>
        private void Collect(Node node, StringBuilder prefix, Queue<string> results)
        {
            if (node == null)
                return;

            if (!node.Value.Equals(default(TValue)))
                results.Enqueue(prefix.ToString());

            for (char c = (char)0; c < R; c++)
            {
                prefix.Append(c);
                Collect(node.Next[c], prefix, results);
                prefix.Remove(prefix.Length - 1, 1);
            }
        }

        /// <summary>
        /// Returns all of the keys in the trie that start with the specified prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>All of the keys in the trie that start with the specified prefix.</returns>
        public IEnumerable<string> KeysWithPrefix(string prefix)
        {
            Queue<string> results = new Queue<string>();
            Node start = CatchNode(root, prefix, 0);
            Collect(start, new StringBuilder(prefix), results);
            return results;
        }

        /// <summary>
        /// Returns all keys in the symbol table as an enumerator.
        /// </summary>
        /// <returns>All keys in the symbol table as an enumerator.</returns>
        public IEnumerable<string> Keys()
        {
            return KeysWithPrefix("");
        }

        /// <summary>
        /// Collect all keys in the symbol table that match pattern.
        /// </summary>
        /// <param name="node">The next node to visit.</param>
        /// <param name="prefix">The prefix of the string.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="results">The queue to stroe the keys.</param>
        private void Collect(Node node, StringBuilder prefix, string pattern, Queue<string> results)
        {
            if (node == null)
                return;

            int length = prefix.Length;

            if ((length == pattern.Length) && (!node.Value.Equals(default(TValue))))
                results.Enqueue(prefix.ToString());

            if (length == pattern.Length)
                return;

            char c = pattern[length];
            if (c == '.')
            {
                for (char ch = (char)0; ch < R; ch++)
                {
                    prefix.Append(ch);
                    Collect(node.Next[ch], prefix, pattern, results);
                    prefix.Remove(prefix.Length - 1, 1);
                }
            }
            else
            {
                prefix.Append(c);
                Collect(node.Next[c], prefix, pattern, results);
                prefix.Remove(prefix.Length - 1, 1);
            }
        }

        /// <summary>
        /// Returns all of the keys in the symbol table that match pattern as an enumerator.
        /// </summary>
        /// <param name="pattern">The pattern</param>
        /// <returns>All of the keys in the symbol table that match pattern as an enumerator.</returns>
        public IEnumerable<string> KeysThatMatch(string pattern)
        {
            Queue<string> results = new Queue<string>();
            Collect(root, new StringBuilder(), pattern, results);
            return results;
        }

        /// <summary>
        /// Returns the length of the longest string key in the sub-trie rooted at node that is a prefix of the query string.
        /// Assuming the first index character match and we have already found a prefix match of given length (-1 if no such match).
        /// </summary>
        /// <param name="node">The next node to visit.</param>
        /// <param name="query">The query string.</param>
        /// <param name="index">The index of string to compare.</param>
        /// <param name="length">The length of posibile prefix.</param>
        /// <returns>The length of the longest string key in the sub-trie rooted at node that is a prefix of the query string.</returns>
        private int LongestPrefixOf(Node node, string query, int index, int length)
        {
            if (node == null)
                return length;

            if (!node.Value.Equals(default(TValue)))
                length = index;

            if (index == query.Length)
                return length;

            char c = query[index];
            return LongestPrefixOf(node.Next[c], query, index + 1, length);
        }

        /// <summary>
        /// Returns the string in the symbol table that is the longest prefix of query, default(TValue) if no such string.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <returns>The string in the symbol table that is the longest prefix of query, default(TValue) if no such string.</returns>
        public string LongestPrefixOf(string query)
        {
            int length = LongestPrefixOf(root, query, 0, -1);
            if (length == -1)
                return null;
            else
                return query.Substring(0, length);
        }

        /// <summary>
        /// Remove the key from the trie if the key is present.
        /// </summary>
        /// <param name="node">The node which posibily hold the key.</param>
        /// <param name="key">The key.</param>
        /// <param name="index">The index of the string to compare.</param>
        /// <returns>Node current.</returns>
        private Node Remove(Node node, string key, int index)
        {
            if (node == null)
                return null;

            if (index == key.Length)
            {
                if (!node.Value.Equals(default(TValue)))
                    Size--;
                node.Value = default(TValue);
            }
            else
            {
                char c = key[index];
                node.Next[c] = Remove(node.Next[c], key, index + 1);
            }

            // Remove sub-trie rooted at node if it is completely empty.
            for (int c = 0; c < R; c++)
            {
                if (!node.Next[c].Equals(default(TValue)))
                    return node;
            }

            return null;
        }

        /// <summary>
        /// Removes the key from the set if the key is present.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            root = Remove(root, key, 0);
        }
    }
}