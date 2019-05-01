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
        /// The Queue class represents a first-in, first-out collection of generic objects.
        /// </summary>
        /// <typeparam name="T">The type of items in this queue.</typeparam>
        public class Queue<T> : IEnumerable<T>
        {
            /// <summary>
            /// The Node class represents a node in a Queue<T>. This class cannot be inherited.
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
                /// Creates a node storing the given data.
                /// </summary>
                /// <param name="data">The given data.</param>
                public Node(T data)
                {
                    Data = data;
                    Next = null;
                }
            }

            /// <summary>
            /// Head node of this queue.
            /// </summary>
            private Node head;

            /// <summary>
            /// Last node of this queue.
            /// </summary>
            private Node end;

            /// <summary>
            /// Gets the number of elements contained in this queue.
            /// </summary>
            public int Size { get; private set; }

            /// <summary>
            /// True if this queue is empty, false otherwise.
            /// </summary>
            public bool IsEmpty
            {
                get { return Size == 0; }
            }

            /// <summary>
            /// Initializes a new queue that is empty.
            /// </summary>
            public Queue()
            {
                head = null;
                end = null;
                Size = 0;
            }

            /// <summary>
            /// Adds an item to the end of the queue.
            /// </summary>
            /// <param name="data">The item to add to this queue. The value can be null for reference types.</param>
            public void Enqueue(T data)
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
                    end = end.Next;
                }

                Size++;
            }

            /// <summary>
            /// Removes and returns the object at the beginning of this queue.
            /// </summary>
            /// <returns>The item that is removed from the beginning of this queue.</returns>
            public T Dequeue()
            {
                if (Size == 0)
                    return default(T);

                Node tempHead = head;
                head = head.Next;
                Size--;
                return tempHead.Data;
            }

            /// <summary>
            /// Returns an enumerator that iterates through this queue.
            /// </summary>
            /// <returns>An enumerator that iterates through this queue.</returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (Node current = head; current != null; current = current.Next)
                    yield return current.Data;
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
    }
}
