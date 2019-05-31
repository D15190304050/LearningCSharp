using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Search
{
    using Collections;

    class RedBlackTree<TKey, TValue> : BinarySearchTreeBase<TKey, TValue> where TKey : IComparable<TKey>
    {
        private const bool Red = true;
        private const bool Black = false;

        // Node for red black tree, with color bit.
        private class Node
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

            // The color link from parent to this node. Black for any null node.
            public bool Color { get; set; }

            public Node(TKey key, TValue value, int size, bool color)
            {
                Key = key;
                Value = value;
                Size = size;
                Left = null;
                Right = null;
                Color = color;
            }
        }

        private Node root;

        /// <summary>
        /// Return the color of the specific node to its parent. Return Black if the node is null.
        /// </summary>
        private bool IsRed(Node node)
        {
            if (node == null)
                return false;
            else
                return node.Color == Red;
        }

        /// <summary>
        /// Return size of the specific node. Return 0 if the node is null.
        /// </summary>
        private int Size(Node node)
        {
            if (node == null)
                return 0;
            else
                return node.Size;
        }

        private Node RotateLeft(Node currentSubRoot)
        {
            Node nextSubRoot = currentSubRoot.Right;
            currentSubRoot.Right = nextSubRoot.Left;
            nextSubRoot.Left = currentSubRoot;
            nextSubRoot.Color = currentSubRoot.Color;
            currentSubRoot.Color = Red;
            nextSubRoot.Size = currentSubRoot.Size;
            currentSubRoot.Size = 1 + Size(currentSubRoot.Left) + Size(currentSubRoot.Right);
            return nextSubRoot;
        }

        private Node RotateRight(Node currentSubRoot)
        {
            Node nextSubRoot = currentSubRoot.Left;
            currentSubRoot.Left = nextSubRoot.Right;
            nextSubRoot.Right = currentSubRoot;
            nextSubRoot.Color = currentSubRoot.Color;
            currentSubRoot.Color = Red;
            nextSubRoot.Size = currentSubRoot.Size;
            currentSubRoot.Size = 1 + Size(currentSubRoot.Left) + Size(currentSubRoot.Right);
            return nextSubRoot;
        }

        private void FlipColors(Node currentSubRoot)
        {
            currentSubRoot.Color = Red;
            currentSubRoot.Left.Color = Black;
            currentSubRoot.Right.Color = Black;
        }

        public RedBlackTree()
        {
            root = null;
        }

        public override TValue this[TKey key]
        {
            // The indexer throws an exception if the request key is not in the red black tree.
            get
            {
                Node target = CatchNode(root, key);
                if (target != null)
                    return target.Value;

                return default(TValue);
            }

            // If a key doesn't exist, setting the indexer for that key adds a new key-value pair.
            set { Add(key, value); }
        }

        private Node CatchNode(Node node, TKey key)
        {
            if (node == null)
                return null;

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
                return CatchNode(node.Left, key);
            if (cmp > 0)
                return CatchNode(node.Right, key);
            else
                return node;
        }

        public override void Add(TKey key, TValue value)
        {
            root = Add(root, key, value);
            root.Color = Black;
        }

        private Node Add(Node currentSubRoot, TKey key, TValue value)
        {
            // Do standard insert, with red link to parent.
            if (currentSubRoot == null)
                return new Node(key, value, 1, Red);

            int cmp = key.CompareTo(currentSubRoot.Key);
            if (cmp < 0)
                currentSubRoot.Left = Add(currentSubRoot.Left, key, value);
            else if (cmp > 0)
                currentSubRoot.Right = Add(currentSubRoot.Right, key, value);
            else
                currentSubRoot.Value = value;

            if (IsRed(currentSubRoot.Right) && (!IsRed(currentSubRoot.Left)))
                currentSubRoot = RotateLeft(currentSubRoot);
            if (IsRed(currentSubRoot.Left) && IsRed(currentSubRoot.Left.Left))
                currentSubRoot = RotateRight(currentSubRoot);
            if (IsRed(currentSubRoot.Left) && IsRed(currentSubRoot.Right))
                FlipColors(currentSubRoot);

            currentSubRoot.Size = 1 + Size(currentSubRoot.Left) + Size(currentSubRoot.Right);
            return currentSubRoot;
        }

        public override TKey CeilingKey(TKey key)
        {
            if (key == null)
                throw new NullReferenceException("Argument to CeilingKey() is null");
            if (IsEmpty)
                throw new KeyNotFoundException("Called CeilingKey() with empty red black tree.");

            Node target = CeilingNode(root, key);
            if (target == null)
                return default(TKey);
            else
                return target.Key;
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
            if (target != null && target.Value.Equals(item.Value))
                return true;
            return false;
        }

        public override bool ContainsKey(TKey key)
        {
            return ((this[key] != null) && (!this[key].Equals(default(TValue))));
        }

        public override TKey FloorKey(TKey key)
        {
            if (key == null)
                throw new NullReferenceException("Argument to FloorKey() is null.");
            if (IsEmpty)
                throw new KeyNotFoundException("Called FloorKey() with empty red black tree.");

            Node target = FloorNode(root, key);
            if (target == null)
                return default(TKey);
            else
                return target.Key;
        }

        private Node FloorNode(Node node, TKey key)
        {
            if (node == null)
                return null;
            int cmp = key.CompareTo(node.Key);
            if (cmp == 0)
                return node;
            if (cmp < 0)
                return FloorNode(node.Left, key);
            Node temp = FloorNode(node.Right, key);
            if (temp != null)
                return temp;
            else
                return node;
        }

        public override IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs()
        {
            foreach (Node n in Nodes(MinKey(), MaxKey()))
                yield return new KeyValuePair<TKey, TValue>(n.Key, n.Value);
        }

        public override IEnumerable<TKey> Keys(TKey low, TKey high)
        {
            try  {  RangeCheck(low, high); }
            catch (NullReferenceException e)  { throw e; }
            catch (ArgumentException e) { throw e; }

            foreach (Node n in Nodes(low, high))
                yield return n.Key;
        }

        private void RangeCheck(TKey low, TKey high)
        {
            if (low == null)
                throw new NullReferenceException("First argumrnt to Keys() is null.");
            if (high == null)
                throw new NullReferenceException("Second argument to Keys() is null.");
            if (high.CompareTo(low) < 0)
                throw new ArgumentException("low.CompareTo(high) must be less than 0.");
        }

        private IEnumerable<Node> Nodes(TKey low, TKey high)
        {
            Queue<Node> queue = new Queue<Node>();
            Keys(queue, root, low, high);
            return queue;
        }

        /// <summary>
        /// Add the keys between low and high in the sub tree rooted at node to the queue.
        /// </summary>
        private void Keys(Queue<Node> queue, Node node, TKey low, TKey high)
        {
            if (node == null)
                return;

            int cmpLow = low.CompareTo(node.Key);
            int cmpHigh = high.CompareTo(node.Key);
            if (cmpLow < 0)
                Keys(queue, node.Left, low, high);
            if (cmpLow <= 0 && cmpHigh >= 0)
                queue.Enqueue(node);
            if (cmpHigh > 0)
                Keys(queue, node.Right, low, high);
        }

        public override TKey MaxKey()
        {
            Node current = root;
            while (current.Right != null)
                current = current.Right;
            return current.Key;
        }

        public override TKey MinKey()
        {
            Node current = root;
            while (current.Left != null)
                current = current.Left;
            return current.Key;
        }

        public override int Rank(TKey key)
        {
            if (key == null)
                throw new NullReferenceException("Argument to Rank() is null.");

            return Rank(root, key);
        }

        private int Rank(Node node, TKey key)
        {
            if (node == null)
                return 0;

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
                return Rank(node.Left, key);
            else if (cmp > 0)
                return 1 + Size(node.Left) + Rank(node.Right, key);
            return Size(node.Left);
        }

        public override void Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public override void Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public override TKey Select(int rank)
        {
            if (rank < 0 || rank >= Size())
                throw new ArgumentOutOfRangeException(string.Format("rank: {0}", rank));
            if (root == null)
                throw new KeyNotFoundException("Called Select() with empty red black tree.");

            Node target = Select(root, rank);
            return target.Key;
        }

        /// <summary>
        /// The key of the specific rank in the sub-tree rooted at node.
        /// </summary>
        private Node Select(Node node, int rank)
        {
            int leftSize = Size(node.Left);
            if (leftSize > rank)
                return Select(node.Left, rank);
            else if (leftSize < rank)
                return Select(node.Right, rank - leftSize - 1);
            return node;
        }

        public override int Size()
        {
            return Size(root);
        }

        public override int Size(TKey low, TKey high)
        {
            try { RangeCheck(low, high); }
            catch (NullReferenceException e) { throw e; }
            catch (ArgumentException e) { throw e; }

            if (ContainsKey(high))
                return Rank(high) - Rank(low) + 1;
            else
                return Rank(high) - Rank(low);
        }

        public override IEnumerable<TValue> Values()
        {
            foreach (Node n in Nodes(MinKey(), MaxKey()))
                yield return n.Value;
        }
    }
}