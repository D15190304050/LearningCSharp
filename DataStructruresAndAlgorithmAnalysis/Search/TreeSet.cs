using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Search
{
    using BasicDataStructures;

    /// <summary>
    /// The Set class represents an ordered set of comparable keys. The order is tree order.
    /// </summary>
    public class TreeSet<TKey> : ISet<TKey> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// The node class represents a data type of a tree node which stores the key only.
        /// </summary>
        private class Node
        {
            /// <summary>
            /// The data stored in this node.
            /// </summary>
            public TKey Data { get; set; }

            /// <summary>
            /// The left child.
            /// </summary>
            public Node Left { get; set; }

            /// <summary>
            /// The right child.
            /// </summary>
            public Node Right { get; set; }

            /// <summary>
            /// The number of nodes rooted at this node, including this node.
            /// </summary>
            public int Size { get; set; }

            /// <summary>
            /// Create a node with specified data.
            /// </summary>
            /// <param name="data">The specified data.</param>
            public Node(TKey data)
            {
                Data = data;
                Left = null;
                Right = null;
                Size = 1;
            }
        }

        // The root of the tree set.
        private Node root;

        /// <summary>
        /// Returns the size of a specified node.
        /// </summary>
        /// <param name="n">The node.</param>
        /// <returns>The size of a specified node.</returns>
        private int Size(Node n)
        {
            if (n == null)
                return 0;
            else
                return n.Size;
        }

        /// <summary>
        /// Gets the number of elements contained in the set.
        /// </summary>
        public int Count
        {
            get { return Size(root); }
        }

        /// <summary>
        /// Gets a value indicating whether the set is read-only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Determines whether the set is empty.
        /// </summary>
        public bool isEmpty
        {
            get { return Count == 0; }
        }

        /// <summary>
        /// Create an empty set.
        /// </summary>
        /// <param name="isReadOnly">Indicating whether the set is read-only.</param>
        public TreeSet(bool isReadOnly = false)
        {
            root = null;
            IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// Create an empty set from an IEnumerable object.
        /// </summary>
        /// <param name="iterator">The IEnumerable object.</param>
        /// <param name="isReadOnly">Indicating whether the set is read-only.</param>
        public TreeSet(IEnumerable<TKey> iterator, bool isReadOnly = false)
        {
            root = null;
            foreach (TKey item in iterator)
                Add(item);
            IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// Adds an element to the current set and returns a value to indicate if the element was successfully added.
        /// </summary>
        /// <param name="item">The item to add to the current set.</param>
        /// <returns>True if the node was added successfully, false if existed.</returns>
        public bool Add(TKey item)
        {
            // Check whether the current set is read-only before modify it.
            ReadOnlyCheck();

            if (Contains(item))
                return false;

            root = Add(root, item);
            return true;
        }

        /// <summary>
        /// Modifies the current set to add a key, do nothing if it has already contains the specified key.
        /// </summary>
        /// <param name="node">The node which the target node may rooted at.</param>
        /// <param name="key">The specified key.</param>
        /// <returns></returns>
        private Node Add(Node node, TKey key)
        {
            if (node == null)
                return new Node(key);

            int cmp = key.CompareTo(node.Data);
            if (cmp < 0)
                node.Left = Add(node.Left, key);
            else if (cmp > 0)
                node.Right = Add(node.Right, key);
            
            // Otherwise, the current set has already contains the key, which means it doesn't need to add it again.

            node.Size = 1 + Size(node.Left) + Size(node.Right);
            return node;
        }

        /// <summary>
        /// Returns a node with specified key, null if no such node.
        /// </summary>
        /// <param name="node">The node where catch procedure starts.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>A node with specified key, null if no such node.</returns>
        private Node CatchNode(Node node, TKey key)
        {
            if (node == null)
                return null;

            int cmp = key.CompareTo(node.Data);
            if (cmp < 0)
                return CatchNode(node.Left, key);
            else if (cmp > 0)
                return CatchNode(node.Right, key);
            else
                return node;
        }

        /// <summary>
        /// Removes all items from the current set.
        /// </summary>
        public void Clear()
        {
            // Check whether the current set is read-only before modify it.
            ReadOnlyCheck();

            root = null;
        }

        /// <summary>
        /// Determines whether the set contains a specified value.
        /// </summary>
        /// <param name="item">The specified item.</param>
        /// <returns>True if the current set contains specified item, false othwewise.</returns>
        public bool Contains(TKey item) { return (CatchNode(root, item) != null); }

        /// <summary>
        /// Copies the elements of the set to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The start index.</param>
        public void CopyTo(TKey[] array, int arrayIndex = 0)
        {
            foreach (TKey item in this)
                array[arrayIndex++] = item;
        }

        /// <summary>
        /// Determine whether the object is equal to the current set.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>True if the object is equal to the current set, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
                return false;
            return SetEquals((TreeSet<TKey>)obj);
        }

        /// <summary>
        /// Removes all elements in the specified collection from the current set.
        /// </summary>
        /// <param name="other">The specified collection.</param>
        public void ExceptWith(IEnumerable<TKey> other)
        {
            foreach (TKey item in other)
                this.Remove(item);
        }

        /// <summary>
        /// Returns an Enumerator that iterates through the set, by the ascending order.
        /// </summary>
        /// <returns>An Enumerator that iterates through the set, by the ascending order.</returns>
        public IEnumerator<TKey> GetEnumerator()
        {
            if (isEmpty)
                return null;

            Queue<TKey> queue = new Queue<TKey>();
            Keys(root, queue);
            return queue.GetEnumerator();
        }

        /// <summary>
        /// Returns the hash value of the current set.
        /// </summary>
        /// <returns>Rhe hash value of the current set.</returns>
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (TKey item in this)
                hash = (hash + item.GetHashCode()) & 0x7FFFFFFF;
            return hash;
        }

        /// <summary>
        /// Iterate through the tree to collect the nodes in a queue.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="queue"></param>
        private void Keys(Node node, Queue<TKey> queue)
        {
            if (node == null)
                return;

            Keys(node.Left, queue);
            queue.Enqueue(node.Data);
            Keys(node.Right, queue);
        }

        /// <summary>
        /// Modifies the current set so that it contains elements that also are also in a specified collection.
        /// </summary>
        /// <param name="other">The specified collection.</param>
        public void IntersectWith(IEnumerable<TKey> other)
        {
            // Check whether the current set is read-only before modify it.
            ReadOnlyCheck();

            // Create a deep copy for the current set.
            TreeSet<TKey> set = new TreeSet<TKey>(this);

            // Clear the current set.
            Clear();

            // Iterate the collection to find elemnents in the intersection and add it to the current set.
            foreach (TKey item in other)
            {
                if (set.Contains(item))
                    this.Add(item);
            }
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) sub set of a specified collection.
        /// </summary>
        /// <param name="other">The specified collection.</param>
        /// <returns>True if the current set is a proper sub set of the specified collection, false otherwise.</returns>
        public bool IsProperSubsetOf(IEnumerable<TKey> other)
        {
            // Create a TreeSet object here because collection allows the duplicated elements.
            TreeSet<TKey> otherSet = new TreeSet<TKey>(other);
            return ((Count < otherSet.Count) && (IsSubsetOf(otherSet)));
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) super set of a specified collection.
        /// </summary>
        /// <param name="other">The specified collection.</param>
        /// <returns>Trueif the current set is a proper super of the specified collection, false otherwise.</returns>
        public bool IsProperSupersetOf(IEnumerable<TKey> other)
        {
            // Create a TreeSet object here because collection allows the duplicated elements.
            TreeSet<TKey> otherSet = new TreeSet<TKey>(other);
            return ((Count > otherSet.Count) && (IsSupersetOf(otherSet)));
        }

        /// <summary>
        /// Determines whether a set is a sub set of a specified collection.
        /// </summary>
        /// <param name="other">The specified collection.</param>
        /// <returns>True if the current set is a sub set of the specified collection, false otherwise.</returns>
        public bool IsSubsetOf(IEnumerable<TKey> other)
        {
            // The current set is a sub set of the specified collection is equal to the case that the specified collection is a super set of the current set.
            TreeSet<TKey> otherSet = new TreeSet<TKey>(other);
            return otherSet.IsSupersetOf(this);
        }

        /// <summary>
        /// Determines whether a set is a super set of a specified collection.
        /// </summary>
        /// <param name="other">The specified collection.</param>
        /// <returns>True if the set is a super set of the specified collection, false otherwise.</returns>
        public bool IsSupersetOf(IEnumerable<TKey> other)
        {
            foreach (TKey item in other)
            {
                if (!Contains(item))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The specified collection.</param>
        /// <returns>True if the current set overlaps with the specified collection, false otherwise.</returns>
        public bool Overlaps(IEnumerable<TKey> other)
        {
            foreach (TKey item in other)
            {
                if (Contains(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the current set is read-only.
        /// Throw an InvalidOperationException if try to call an operation which may change the current set when current set is read-only.
        /// </summary>
        private void ReadOnlyCheck()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("The current set is read-only.");
        }

        /// <summary>
        /// Remove a specified item from the current set, do nothing if the current doesn't contains the item.
        /// </summary>
        /// <param name="item">The specified item.</param>
        /// <returns>True if the item was successfully removed, false if the current set doesn't contains the item.</returns>
        public bool Remove(TKey item)
        {
            // Check whether the current set is read-only before modify it.
            ReadOnlyCheck();

            if (!Contains(item))
                return false;

            root = Remove(root, item);
            return true;
        }

        /// <summary>
        /// A helper method to remove the node with specified key and fix the tree linkage.
        /// </summary>
        /// <param name="node">The node where the target node may rooted at.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>A sub tree without the node which hold the speicified node, null if no such node.</returns>
        private Node Remove(Node node, TKey key)
        {
            if (node == null)
                return null;

            int cmp = key.CompareTo(node.Data);
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

                // Find the key to replace that to be removed.
                node = MinKey(node.Right);

                // Remove the replaced node in the sub-tree rooted at node.Right.
                node.Right = Remove(temp.Right, node.Data);

                // Fix the node linkage.
                node.Left = temp.Left;
            }

            node.Size = 1 + Size(node.Left) + Size(node.Right);
            return node;
        }

        /// <summary>
        /// Returns a node with the min value rooted at a specified node.
        /// </summary>
        /// <param name="node">The root node of the search.</param>
        /// <returns>A node with the min value rooted at a specified node.</returns>
        private Node MinKey(Node node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The specified collection.</param>
        /// <returns>True if the current set and the specified collection contain the same elements, false otherwise.</returns>
        public bool SetEquals(IEnumerable<TKey> other)
        {
            // Set A equals to Set B is equal to (A.Count == B.Count) && (A.IsSupersetOf(B)).
            TreeSet<TKey> otherSet = new TreeSet<TKey>(other);
            return ((Count == otherSet.Count) && IsSupersetOf(otherSet));
        }

        /// <summary>
        /// Modifies the curent set so that it contains only elements that are present in the current set or in the specified collection, but not both.
        /// </summary>
        /// <param name="other">The specified collcetion.</param>
        public void SymmetricExceptWith(IEnumerable<TKey> other)
        {
            // Check whether the current set is read-only before modify it.
            ReadOnlyCheck();

            // Find the union of the 2 sets.
            TreeSet<TKey> union = new TreeSet<TKey>(this);
            union.UnionWith(other);

            // Create a deep copy of the current set and The other collection.
            TreeSet<TKey> current = new TreeSet<TKey>(this);
            TreeSet<TKey> otherSet = new TreeSet<TKey>(other);

            // Clear the current set.
            Clear();

            // Traverse thtough the union and determine every item.
            foreach (TKey item in union)
            {
                if (!(current.Contains(item) && otherSet.Contains(item)))
                    Add(item);
            }
        }

        /// <summary>
        /// Returns the string representation of the current.
        /// </summary>
        /// <returns>The string representation of the current.</returns>
        public override string ToString()
        {
            string s = "Show the content in the current set:\n";
            foreach (TKey item in this)
                s += item.ToString() + "\n";
            return s;
        }

        /// <summary>
        /// Modifies the current set so that it contains all elements that are present in the current set, in the specified collection, or in both.
        /// </summary>
        /// <param name="other"></param>
        public void UnionWith(IEnumerable<TKey> other)
        {
            // Check whether the current set is read-only before modify it.
            ReadOnlyCheck();

            foreach (TKey item in other)
                Add(item);
        }

        /// <summary>
        /// Adds an element to the current set and returns a value to indicate if the element was successfully added.
        /// </summary>
        /// <param name="item">The item to add to the current set.</param>
        void ICollection<TKey>.Add(TKey item) { Add(item); }

        /// <summary>
        /// Returns an Enumerator that iterates through the set, by the ascending order.
        /// </summary>
        /// <returns>An Enumerator that iterates through the set, by the ascending order.</returns>
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}