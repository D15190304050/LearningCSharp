using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.UndirectedGraph
{
    using BasicDataStructures;

    public class DepthFirstPaths : GraphPathBase
    {
        public DepthFirstPaths(Graph G, int source)
            : base(G, source)
        { Dfs(G, source); }

        /// <summary>
        /// Depth search from v.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="v"></param>
        private void Dfs(Graph G, int v)
        {
            Count++;
            Marked[v] = true;
            foreach (int w in G.Adjacent(v))
            {
                if (!Marked[w])
                {
                    edgeTo[w] = v;
                    Dfs(G, w);
                }
            }
        }

        public override IEnumerable<int> PathTo(int v)
        {
            // Validate vertex.
            if (v < 0 || v >= edgeTo.Length)
                throw new ArgumentOutOfRangeException("Vertex " + v + " is not between 0 and " + (edgeTo.Length - 1));

            if (!HasPathTo(v))
                return null;

            Stack<int> path = new Stack<int>();
            for (int x = v; x != Source; x = edgeTo[x])
                path.Push(x);
            path.Push(Source);
            return path;
        }
    }
}
