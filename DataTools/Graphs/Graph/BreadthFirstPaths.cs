using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.UndirectedGraph
{
    using BasicDataStructures;

    public class BreadthFirstPaths : GraphPathBase
    {
        /// <summary>
        /// Number of edges in a shortest path between the source vertex and the specific vertex.
        /// </summary>
        public int[] DistanceTo { get; private set; }

        /// <summary>
        /// Compute the shortest path between the source vertex and every other vertex in the graph.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="source"></param>
        public BreadthFirstPaths(Graph G, int source)
            : base(G, source)
        {
            DistanceTo = new int[G.V];
            Bfs(G, source);
        }

        /// <summary>
        /// Breadth first search from a single vertex.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="s"></param>
        private void Bfs(Graph G, int source)
        {
            Queue<int> queue = new Queue<int>();
            for (int v = 0; v < G.V; v++)
                DistanceTo[v] = int.MaxValue;
            DistanceTo[source] = 0;

            // Mark the source.
            Marked[source] = true;

            // Increase the connection counter.
            Count++;

            // Put the source in the queue.
            queue.Enqueue(source);

            while (!queue.IsEmpty)
            {
                // Remove next vertex from the queue.                
                int v = queue.Dequeue();

                foreach (int w in G.Adjacent(v))
                {
                    // For every un-marked adjacent vertex.
                    if (!Marked[w])
                    {
                        // Increase the connection counter.
                        Count++;

                        // Save last edge on a shortest path.
                        edgeTo[w] = v;
                        
                        // Compute the distance between source vertex to this vertex.
                        DistanceTo[w] = DistanceTo[v] + 1;

                        // Mark it because the path is known.
                        Marked[w] = true;

                        // Add it to the queue.
                        queue.Enqueue(w);
                    }
                }
            }
        }

        public override IEnumerable<int> PathTo(int v)
        {
            if (!HasPathTo(v))
                return null;

            Stack<int> path = new Stack<int>();
            int x;
            for (x = v; x != Source; x = edgeTo[x])
                path.Push(x);
            path.Push(x);
            return path;
        }
    }
}
