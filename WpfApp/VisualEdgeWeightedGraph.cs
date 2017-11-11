using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class VisualEdgeWeightedGraph
    {
        private List<SortedSet<VisualEdge>> adjacent;

        public int V { get; private set; }
        public int E { get; private set; }

        public VisualEdgeWeightedGraph()
        {
            this.V = 0;
            this.E = 0;
            adjacent = new List<SortedSet<VisualEdge>>();
        }

        private void ValidateVertex(int v)
        {
            if ((v < 0) || (v >= V))
                throw new ArgumentException(string.Format("Vertex {0} is not between [0,{1}].", v, (V - 1).ToString()));
        }

        public bool AddEdge(VisualEdge e)
        {
            int v = e.Either();
            int w = e.Other(v);
            ValidateVertex(v);
            ValidateVertex(w);

            if ((adjacent[v] == null) || (adjacent[w] == null))
                return false;

            adjacent[v].Add(e);
            adjacent[w].Add(e);
            E++;
            return true;
        }

        public void RemoveEdge(VisualEdge e)
        {
            int v = e.Either();
            int w = e.Other(v);
            if ((adjacent[v] != null) && (adjacent[w] != null))
            {
                adjacent[v].Remove(e);
                adjacent[w].Remove(e);
                this.E--;
            }
        }

        public void RemoveVertex(int v)
        {
            IEnumerable<VisualEdge> adjacents = adjacent[v].ToArray();
            foreach (VisualEdge e in adjacents)
                RemoveEdge(e);
            adjacent[v] = null;
        }

        public void AddVertex()
        {
            this.V++;
            adjacent.Add(new SortedSet<VisualEdge>());
        }

        public bool ContainsVertex(int v)
        {
            if ((v < 0) || (v >= V))
                return false;
            else if (adjacent[v] == null)
                return false;
            return true;
        }

        public IEnumerable<VisualEdge> Adjacent(int v)
        {
            ValidateVertex(v);
            return adjacent[v];
        }

        public IEnumerable<VisualEdge> Edges()
        {
            LinkedList<VisualEdge> edges = new LinkedList<VisualEdge>();
            for (int v = 0; v < V; v++)
            {
                if (adjacent[v] == null)
                    continue;

                int selfLoops = 0;
                foreach (VisualEdge e in adjacent[v])
                {
                    if (e.Other(v) > v)
                        edges.AddFirst(e);

                    // Only add one copy of self loop (self loop will be constructive).
                    else if (e.Other(v) == v)
                    {
                        if (selfLoops % 2 == 0)
                            edges.AddFirst(e);
                        selfLoops++;
                    }
                }
            }

            return edges;
        }

        public override string ToString()
        {
            int vertexCount = 0;
            StringBuilder s = new StringBuilder();
            for (int v = 0; v < V; v++)
            {
                if (adjacent[v] != null)
                {
                    vertexCount++;
                    s.Append(v + ": ");
                    foreach (VisualEdge e in adjacent[v])
                        s.Append(e + "; ");
                    s.Append(Environment.NewLine);
                }
            }

            return vertexCount + " vertices  " + E + " edges\n" + s.ToString();
        }

        public void Clear()
        {
            this.V = 0;
            this.E = 0;
            adjacent.Clear();
        }
    }
}
