using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools
{
    namespace BasicDataStructures
    {
        /// <summary>
        /// The LinkedList class represents a doubly linked list.
        /// </summary>
        /// <typeparam name="T">The type of elements in this linked list.</typeparam>
        public class LinkedList<T> : IEnumerable<T>
        {
            /// <summary>
            /// The Node class represents a node in a LinkedList&lt;T>. This class cannot be inherited.
            /// </summary>
            private sealed class Node
            {
                /// <summary>
                /// The data stored in this node.
                /// </summary>
                public T Data { get; set; }

                /// <summary>
                /// Next node of this node.
                /// </summary>
                public Node Next { get; set; }

                /// <summary>
                /// Previous node of this node.
                /// </summary>
                public Node Prev { get; set; }

                /// <summary>
                /// Creates a node storing the given data.
                /// </summary>
                /// <param name="data">The given data.</param>
                public Node(T data)
                {
                    Data = data;
                    Next = null;
                    Prev = null;
                }
            }

            /// <summary>
            /// Head node of this linked list.
            /// </summary>
            private Node head;

            /// <summary>
            /// Last node of this linked list.
            /// </summary>
            private Node end;

            /// <summary>
            /// Gets the number of elements contained in this linked list.
            /// </summary>
            public int Size { get; private set; }

            /// <summary>
            /// True if this linked list is empty, false otherwise.
            /// </summary>
            public bool IsEmpty
            {
                get { return Size == 0; }
            }

            /// <summary>
            /// Initializes an empty linked list.
            /// </summary>
            public LinkedList()
            {
                head = null;
                end = null;
                Size = 0;
            }

            /// <summary>
            /// Adds a new node containing the specified data at the start of this linked list.
            /// </summary>
            /// <param name="data">The data to add at the start of this linked list.</param>
            public void AddFirst(T data)
            {
                Node tempFirst = new Node(data);

                // If the linked list is empty, then the head and end point to the same node.
                // Else, change the node reference.
                if (Size == 0)
                {
                    head = tempFirst;
                    end = head;
                }
                else
                {
                    tempFirst.Next = head;
                    head.Prev = tempFirst;
                    head = tempFirst;
                }

                Size++;
            }

            /// <summary>
            /// Adds a new node containing the specified data at the end of this linked list.
            /// </summary>
            /// <param name="data">The data to add at the end of this linked list.</param>
            public void AddLast(T data)
            {
                Node tempEnd = new Node(data);

                if (Size == 0)
                {
                    head = tempEnd;
                    end = head;
                }
                else
                {
                    end.Next = tempEnd;
                    tempEnd.Prev = end;
                    end = tempEnd;
                }

                Size++;
            }

            /// <summary>
            /// The read only indexer to get data from the linked list.
            /// </summary>
            /// <param name="index">The position of the node, starts from 0.</param>
            /// <returns>The data of node with specific index.</returns>
            public T this[int index]
            {
                get
                {
                    if (index < 0 || index >= Size)
                        throw new System.ArgumentOutOfRangeException(string.Format("index value: {0}", index));
                    Node targetNode = head;
                    for (int i = 0; i < index; i++)
                        targetNode = targetNode.Next;
                    return targetNode.Data;
                }
            }

            /// <summary>
            /// Removes and returns the first item from the linked list.
            /// </summary>
            /// <returns>
            /// The item stored in the first node, default valud of the type T if the linked list is empty.
            /// </returns>
            public T RemoveFirst()
            {
                if (Size == 0)
                    return default(T);

                Node tempFirst = head;
                head = head.Next;
                head.Prev = null;
                Size--;
                return tempFirst.Data;
            }

            /// <summary>
            /// Removes and returns the last item from the linked list.
            /// </summary>
            /// <returns>
            /// The item hold in the first node, default valud of the type T if the linked list is empty.
            /// </returns>
            public T RemoveLast()
            {
                if (Size == 0)
                    return default(T);

                Node tempLast = end;
                end = tempLast.Prev;
                end.Next = null;
                Size--;
                return tempLast.Data;
            }

            /// <summary>
            /// Returns an enumerator that supports a simple iteration over this linked list.
            /// </summary>
            /// <returns>An iterator that iterates through this linked list.</returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (Node current = head; current != null; current = current.Next)
                    yield return current.Data;
            }

            /// <summary>
            /// Returns an enumerator that supports a simple iteration over this linked list, by the reversed order.
            /// </summary>
            /// <returns>An iterator that iterates through this linked list, by the reversed order.</returns>
            public IEnumerable<T> Reverse()
            {
                for (Node current = end; current != null; current = current.Prev)
                    yield return current.Data;
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
    }
}
