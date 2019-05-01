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
        /// The Stack class represents a last-in, first out collection of generic objects.
        /// </summary>
        /// <typeparam name="T">The type of items in this stack.</typeparam>
        public class Stack<T> : IEnumerable<T>
        {
            /// <summary>
            /// The Node class represents a node in a Stack<T>. This class cannot be inherited.
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
            /// Head node of this stack.
            /// </summary>
            private Node head;

            /// <summary>
            /// True if this stack is empty, false otherwise.
            /// </summary>
            public bool IsEmpty
            {
                get { return Size == 0; }
            }

            /// <summary>
            /// Gets the number of items contained in this stack.
            /// </summary>
            public int Size { get; private set; }

            /// <summary>
            /// Initializes a new Stack that is empty.
            /// </summary>
            public Stack()
            {
                head = null;
                Size = 0;
            }

            /// <summary>
            /// Returns the item at the top of this stack without removing it.
            /// </summary>
            /// <returns>The item at the top of this stack.</returns>
            public T Peek() { return head.Data; }

            /// <summary>
            /// Inserts an item at the top of this stack.
            /// </summary>
            /// <param name="data">The data to push onto this stack.</param>
            public void Push(T data)
            {
                Node tempHead = new Node(data);
                tempHead.Next = head;
                head = tempHead;
                Size++;
            }

            /// <summary>
            /// Removes and returns the item at the top of this stack.
            /// </summary>
            /// <returns>The item at the top of this stack.</returns>
            public T Pop()
            {
                if (Size == 0)
                    return default(T);

                Node tempHead = head;
                head = head.Next;
                Size--;
                return tempHead.Data;
            }

            /// <summary>
            /// Returns an enumerator that iterates through this stack.
            /// </summary>
            /// <returns>An enumerator that iterates through this stack.</returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (Node current = head; current != null; current = current.Next)
                    yield return current.Data;
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
    }
}
