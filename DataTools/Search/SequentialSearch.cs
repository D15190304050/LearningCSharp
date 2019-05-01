using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Search
{
    /// <summary>
    /// An unordered linked list based symbol table.
    /// </summary>
    public class SequentialSearch<TKey, TValue> : ISymbolTable<TKey, TValue>
    {
        // Linked list node.
        private class Node
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Node Next { get; set; }

            public Node(TKey key, TValue value, Node next)
            {
                Key = key;
                Value = value;
                Next = next;
            }

            public Node(KeyValuePair<TKey, TValue> item, Node next)
            {
                Key = item.Key;
                Value = item.Value;
                Next = next;
            }
        }

        private int size;

        // First node in the linked list.
        private Node first;

        public int Size()
        {
            return size;
        }

        public bool IsEmpty
        {
            get { return size == 0; }
        }

        public TValue this[TKey key]
        {
            get
            {
                for (Node current = first; current != null; current = current.Next)
                {
                    if (current.Key.Equals(key))
                        return current.Value;
                }

                return default(TValue);
            }

            set
            {
                for (Node current = first; current != null; current = current.Next)
                {
                    if (current.Key.Equals(key))
                    {
                        current.Value = value;
                        return;
                    }
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            // Search for key. Update value if found; grow table if new.
            for (Node current = first; current != null; current = current.Next)
            {
                // Search hit: update value.
                if (current.Key.Equals(key))
                {
                    current.Value = value;
                    return;
                }
            }

            // Search miss: add new node, and update the node counter.
            first = new Node(key, value, first);
            size++;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            // Clear the node counter.
            size = 0;
            // Clear the linked list.
            first = null;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            for (Node current = first; current != null; current = current.Next)
            {
                if (current.Key.Equals(item.Key) &&
                    current.Value.Equals(item.Value))
                    return true;
            }

            return false;
        }

        public bool ContainsKey(TKey key)
        {
            for (Node current = first; current != null; current = current.Next)
            {
                if (current.Key.Equals(key))
                    return true;
            }
            return false;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs()
        {
            for (Node current = first; current != null; current = current.Next)
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
        }

        public IEnumerable<TKey> Keys()
        {
            for (Node current = first; current != null; current = current.Next)
                yield return current.Key;
        }

        public void Remove(KeyValuePair<TKey, TValue> item)
        {
            // Do nothing if the symbol table is empty.
            if (size == 0 || first == null)
                return;

            // If the first node contains the item, make first point to first.Next and decrease the size by 1.
            if (first.Key.Equals(item.Key) && first.Value.Equals(item.Value))
            {
                first = first.Next;
                size--;
                return;
            }

            // If the first node doesn't contains the item, then find the node whose next contains the item and modify the Next and Size.
            for (Node prev = first; prev.Next != null; prev = prev.Next)
            {
                if (prev.Next.Key.Equals(item.Key) && prev.Next.Value.Equals(item.Value))
                {
                    prev.Next = prev.Next.Next;
                    size--;
                }
            }

            // If the process reaches here, the symbol table doesn't contan the key-value pair.
        }

        public void Remove(TKey key)
        {
            // Do nothing if the symbol table is empty.
            if (size == 0 || first == null)
                return;

            // If the first node contains the item, make first point to first.Next and decrease the size by 1.
            if (first.Key.Equals(key))
            {
                first = first.Next;
                size--;
                return;
            }

            // If the first node doesn't contains the item, then find the node whose next contains the item and modify the Next and Size.
            for (Node prev = first; prev.Next != null; prev = prev.Next)
            {
                if (prev.Next.Key.Equals(key))
                {
                    prev.Next = prev.Next.Next;
                    size--;
                }
            }

            // If the process reaches here, the symbol table doesn't contain the key-value pair.
        }
    }
}