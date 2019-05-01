using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    using System.Collections;
    using BasicDataStructures;

    /// <summary>
    /// The TrieSet class represents an order set of strings over the extended ASCII alphabel.
    /// It's an ordered trie symbol table rather than a set, which means this class doesn't implemet the ISet<> interface.
    /// </summary>
    public class TrieSet : IEnumerable<string>
    {
        // Extended ASCII.
        private const int R = 256;

        /// <summary>
        /// R-way trie node.
        /// </summary>
        private class Node
        {
            /// <summary>
            /// True if this node is a string, false otherwise.
            /// </summary>
            public bool IsString { get; set; }

            /// <summary>
            /// Sub-tries.
            /// </summary>
            public Node[] Next { get; set; }

            /// <summary>
            /// Create a new node of the trie.
            /// </summary>
            public Node()
            {
                IsString = false;
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
        /// True if this trie is empty, false otherwise.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Size == 0;
            }
        }

        /// <summary>
        /// Initializes an empty set of strings.
        /// </summary>
        public TrieSet()
        {
            root = null;
            Size = 0;
        }

        /// <summary>
        /// Returns the node which hold the specified key, default(TValue) if no such node.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="key">The key to search.</param>
        /// <param name="index">The index of string to compare.</param>
        /// <returns>The node which hold the specified key, default(TValue) if no such node.</returns>
        private Node CatchNode(Node current, string key, int index)
        {
            if (current == null)
                return null;

            if (index == key.Length)
                return current;

            char c = key[index];
            return CatchNode(current.Next[c], key, index + 1);
        }

        /// <summary>
        /// Returns true if this set contains the given key, false otherwise.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if this set contains the given key, false otherwise.</returns>
        public bool Contains(string key)
        {
            Node result = CatchNode(root, key, 0);
            if (result == null)
                return false;
            return result.IsString;
        }

        /// <summary>
        /// Adds the key to this set if it is not already present.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="index">The index of string to compare.</param>
        /// <returns>current.</returns>
        private Node Add(Node current, string key, int index)
        {
            if (current == null)
                current = new Node();

            if (index == key.Length)
            {
                if (!current.IsString)
                    Size++;
                current.IsString = true;
            }
            else
            {
                char c = key[index];
                current.Next[c] = Add(current.Next[c], key, index + 1);
            }

            return current;
        }

        /// <summary>
        /// Adds the key to this set if it is not already present.
        /// </summary>
        /// <param name="key">The key to add.</param>
        public void Add(string key)
        {
            root = Add(root, key, 0);
        }

        /// <summary>
        /// Collect all keys in the set that match pattern.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="prefix">The prefix of the string.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="results">The queue to stroe the keys.</param>
        private void Collect(Node current, StringBuilder prefix, Queue<string> results)
        {
            if (current == null)
                return;

            if (current.IsString)
                results.Enqueue(prefix.ToString());

            for (char c = (char)0; c < R; c++)
            {
                prefix.Append(c);
                Collect(current.Next[c], prefix, results);
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
        /// Returns all of the keys in the set, as an enumerator.
        /// </summary>
        /// <returns>All of the keys in the set, as an enumerator.</returns>
        public IEnumerator<string> GetEnumerator()
        {
            return KeysWithPrefix("").GetEnumerator();
        }

        /// <summary>
        /// Collect all keys in the set that match pattern.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="prefix">The prefix of the string.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="results">The queue to stroe the keys.</param>
        private void Collect(Node current, StringBuilder prefix, string pattern, Queue<string> results)
        {
            if (current == null)
                return;

            int index = prefix.Length;
            if ((index == pattern.Length) && current.IsString)
                results.Enqueue(prefix.ToString());
            if (index == pattern.Length)
                return;

            char c = pattern[index];
            if (c == '.')
            {
                for (char ch = (char)0; ch < R; ch++)
                {
                    prefix.Append(ch);
                    Collect(current.Next[ch], prefix, pattern, results);
                    prefix.Remove(prefix.Length - 1, 1);
                }
            }
            else
            {
                prefix.Append(c);
                Collect(current.Next[c], prefix, pattern, results);
                prefix.Remove(prefix.Length - 1, 1);
            }
        }

        /// <summary>
        /// Returns all of the keys in the set that match pattern, where '.' is treated as wildcard character.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>all of the keys in the set that match pattern, where '.' is treated as wildcard character.</returns>
        public IEnumerable<string> KeysThatMatch(string pattern)
        {
            Queue<string> results = new Queue<string>();
            StringBuilder prefix = new StringBuilder();
            Collect(root, prefix, pattern, results);
            return results;
        }

        /// <summary>
        /// Returns the length of the longest string key in the sub-trie rooted at node that is a prefix of the query string.
        /// Assuming the first index character match and we have already found a prefix match of given length (-1 if no such match).
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="query">The query string.</param>
        /// <param name="index">The index of string to compare.</param>
        /// <param name="length">The length of posibile prefix.</param>
        /// <returns>The length of the longest string key in the sub-trie rooted at node that is a prefix of the query string.</returns>
        private int LongestPrefixOf(Node current, string query, int index, int length)
        {
            if (current == null)
                return length;

            if (current.IsString)
                length = index;

            if (index == query.Length)
                return length;

            char c = query[index];
            return LongestPrefixOf(current.Next[c], query, index + 1, length);
        }

        /// <summary>
        /// Returns the string in the set that is the longest prefix of query, null if no such string.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <returns>The string in the set that is the longest prefix of query, null if no such string.</returns>
        public string LongestPrefixOf(string query)
        {
            int length = LongestPrefixOf(root, query, 0, -1);
            if (length == -1)
                return null;
            return query.Substring(0, length);
        }

        /// <summary>
        /// Removes the key form the set if the key is present.
        /// </summary>
        /// <param name="current">The next node to visit.</param>
        /// <param name="key">The key do delete.</param>
        /// <param name="index">The index of the string to compare.</param>
        /// <returns>Node current.</returns>
        private Node Remove(Node current, string key, int index)
        {
            if (current == null)
                return null;

            if (index == key.Length)
            {
                if (current.IsString)
                    Size--;
                current.IsString = false;
            }
            else
            {
                char c = key[index];
                current.Next[c] = Remove(current.Next[c], key, index + 1);
            }

            // Remove sub-trie root at current if it is complete emtpy.
            if (current.IsString)
                return current;
            for (int c = 0; c < R; c++)
            {
                if (current.Next[c] != null)
                    return current;
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}