using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Search
{
    using BasicDataStructures;

    public class BinarySearchTree<TKey, TValue> : BinarySearchTreeBase<TKey, TValue> where TKey : IComparable<TKey>
    {
        protected class Node
        {
            // The key.
            public TKey Key { get; set; }
            // The associted value.
            public TValue Value { get; set; }
            // Links to sub-trees.
            public Node Left { get; set; }
            public Node Right { get; set; }
            // Number of nodes in this sub-tree.
            public int Size { get; set; }

            public Node(TKey key, TValue value, int size)
            {
                Key = key;
                Value = value;
                Size = size;
                Left = null;
                Right = null;
            }
        }

        // Root of the binary search tree.
        private Node root;

        private int Size(Node node)
        {
            if (node == null)
                return 0;
            else
                return node.Size;
        }

        public BinarySearchTree()
        {
            root = null;
        }

        public override TValue this[TKey key]
        {
            // The indexer throws an exception if the request key is not in the binary search tree.
            get
            {
                Node target = CatchNode(root, key);
                if (target != null)
                    return target.Value;

                throw new KeyNotFoundException(string.Format("Key \"{0}\" is not found", key));
            }

            // If a key doesn't exist, setting the indexer for that key adds a new key-value pair.
            set{ Add(key, value); }
        }

        private Node CatchNode(Node node, TKey key)
        {
            if (node == null)
                return null;

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
                return CatchNode(node.Left, key);
            else if (cmp > 0)
                return CatchNode(node.Right, key);
            else
                return node;
        }

        public override int Size(TKey low, TKey high)
        {
            int size = 0;
            Size(root, ref size, low, high);
            return size;
        }

        private void Size(Node node, ref int size, TKey low, TKey high)
        {
            if (node == null)
                return;

            int cmpLow = low.CompareTo(node.Key);
            int cmpHigh = high.CompareTo(node.Key);
            if (cmpLow < 0)
                Size(node.Left, ref size, low, high);
            if (cmpLow <= 0 && cmpHigh >= 0)
                size++;
            if (cmpHigh > 0)
                Size(node.Right, ref size, low, high);
        }

        public override void Add(TKey key, TValue value)
        {
            root = Add(root, key, value);
        }

        // Change key's value if key in the sub-tree rooted at node.
        // Otherwise, add new node to sub-tree associated key with value.
        private Node Add(Node node, TKey key, TValue value)
        {
            if (node == null)
                return new Node(key, value, 1);
            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
                node.Left = Add(node.Left, key, value);
            else if (cmp > 0)
                node.Right = Add(node.Right, key, value);
            else
                node.Value = value;
            node.Size = Size(node.Left) + Size(node.Right) + 1;
            return node;
        }

        public override TKey CeilingKey(TKey key)
        {
            Node target = CeilingNode(root, key);
            if (target != null)
                return target.Key;
            else
                return default(TKey);
        }
        
        private Node CeilingNode(Node node, TKey key)
        {
            if (node == null)
                return null;
            int cmp = key.CompareTo(node.Key);
            if (cmp == 0)
                return node;
            if (cmp > 0)
                return CeilingNode(node.Right, key);
            Node temp = CeilingNode(node.Left, key);
            if (temp != null)
                return temp;
            else
                return node;
        }

        public override void Clear()
        {
            root = null;
        }

        public override bool Contains(KeyValuePair<TKey, TValue> item)
        {
            Node target = CatchNode(root, item.Key);
            if ((target != null) || (target.Value.Equals(item.Value)))
                return true;
            return false;
        }

        public override bool ContainsKey(TKey key)
        {
            return ContainsKey(root, key);
        }

        private bool ContainsKey(Node node, TKey key)
        {
            if (node == null)
                return false;

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
                return ContainsKey(node.Left, key);
            else if (cmp > 0)
                return ContainsKey(node.Right, key);

            return true;
        }

        public override TKey FloorKey(TKey key)
        {
            Node floorNode = FloorNode(root, key);
            if (floorNode == null)
                return default(TKey);
            return floorNode.Key;
        }

        private Node FloorNode(Node node, TKey key)
        {
            if (node == null)
                return null;
            int cmp = key.CompareTo(node.Key);
            if (cmp == 0)
                return node;
            else if (cmp < 0)
                return FloorNode(node.Left, key);
            Node temp = FloorNode(node.Right, key);
            if (temp != null)
                return temp;
            else
                return node;
        }

        public override IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs()
        {
            foreach (Node node in Nodes(MinKey(), MaxKey()))
                yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
        }

        public override IEnumerable<TKey> Keys(TKey low, TKey high)
        {
            foreach (Node node in Nodes(low, high))
                yield return node.Key;
        }

        public override IEnumerable<TValue> Values()
        {
            foreach (Node node in Nodes(MinKey(), MaxKey()))
                yield return node.Value;
        }

        /// <summary>
        /// Returns all nodes in the binary tree.
        /// This method is used to implement the set indexer and the other IEnumerable method.
        /// </summary>
        private IEnumerable<Node> Nodes(TKey low, TKey high)
        {
            Queue<Node> queue = new Queue<Node>();
            Nodes(root, queue, low, high);
            return queue;
        }

        private void Nodes(Node node, Queue<Node> queue, TKey low, TKey high)
        {
            if (node == null)
                return;

            int cmpLow = low.CompareTo(node.Key);
            int cmpHigh = high.CompareTo(node.Key);
            if (cmpLow < 0)
                Nodes(node.Left, queue, low, high);
            if (cmpLow <= 0 && cmpHigh >= 0)
                queue.Enqueue(node);
            if (cmpHigh > 0)
                Nodes(node.Right, queue, low, high);
        }

        public override TKey MaxKey()
        {
            Node max = root;
            while (max.Right != null)
                max = max.Right;
            return max.Key;
        }

        public override TKey MinKey()
        {
            Node min = root;
            while (min.Left != null)
                min = min.Left;
            return min.Key;
        }

        private Node MinKey(Node node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        public override int Rank(TKey key)
        {
            return Rank(root, key);
        }

        /// <summary>
        /// Return number of keys less than key in the sub-tree rooted at x.
        /// </summary>
        private int Rank(Node node, TKey key)
        {
            if (node == null)
                return 0;
            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
                return Rank(node.Left, key);
            else if (cmp > 0)
                return Rank(node.Right, key) + Size(node.Left) + 1;
            else
                return Size(node.Left);
        }

        public override void RemoveMax()
        {
            root = RemoveMax(root);
        }

        private Node RemoveMax(Node node)
        {
            // Delete the node with max key.
            if (node.Right == null)
                return node.Left;

            // Find the node with max key.
            node.Right = RemoveMax(node.Right);

            // Fix the size.
            node.Size = Size(node.Left) + Size(node.Right) + 1;
            return node;
        }

        public override void RemoveMin()
        {
            RemoveMin(root);
        }

        private Node RemoveMin(Node node)
        {
            // Delete the node with min key.
            if (node.Left == null)
                return node.Right;

            // Find the node with min key.
            node.Left = RemoveMin(node.Left);

            // Fix the size.
            node.Size = Size(node.Left) + Size(node.Right) + 1;
            return node;
        }

        public override void Remove(TKey key)
        {
            root = Remove(root, key);
        }

        private Node Remove(Node node, TKey key)
        {
            if (node == null)
                return null;

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
                node.Left = Remove(node.Left, key);
            else if (cmp > 0)
                node.Right = Remove(node.Right, key);
            else
            {
                if (node.Right == null)
                    return node.Left;
                if (node.Left == null)
                    return node.Right;

                // Save the node to be removed.
                Node temp = node;
                // Find the node to replace that to be removed.
                node = MinKey(node.Right);
                // Remove the repeated node in the sub-tree rooted at node.Right.
                node.Right = RemoveMin(temp.Right);
                // Fix the node linkage.
                node.Left = temp.Left;
            }

            node.Size = Size(node.Left) + Size(node.Right) + 1;
            return node;
        }

        public override void Remove(KeyValuePair<TKey, TValue> item)
        {
            root = Remove(root, item);
        }

        private Node Remove(Node node, KeyValuePair<TKey, TValue> item)
        {
            if (node == null)
                return null;

            int cmp = item.Key.CompareTo(node.Key);
            if (cmp < 0)
                node.Left = Remove(node.Left, item);
            else if (cmp > 0)
                node.Right = Remove(node.Right, item);
            else if (item.Value.Equals(node.Value))
            {
                if (node.Right == null)
                    return node.Left;
                if (node.Left == null)
                    return node.Right;

                Node temp = node;
                node = MinKey(node.Right);
                node.Right = RemoveMin(node.Right);
                node.Left = temp.Left;
            }

            node.Size = Size(node.Left) + Size(node.Right) + 1;
            return node;
        }

        public override TKey Select(int rank)
        {
            if (rank < 0 || rank >= Size())
                throw new ArgumentOutOfRangeException(string.Format("rank: {0}", rank));
            Node target = Select(root, rank);
            if (target == null)
                return default(TKey);
            else
                return target.Key;
        }

        /// <summary>
        /// Return Node containing key of the specific rank.
        /// </summary>
        private Node Select(Node node, int rank)
        {
            if (node == null)
                return null;

            int leftSize = Size(node.Left);
            if (leftSize > rank)
                return Select(node.Left, rank);
            else if (leftSize < rank)
                return Select(node.Right, rank - leftSize - 1);
            else
                return node;
        }

        public override int Size()
        {
            return Size(root);
        }
    }
}