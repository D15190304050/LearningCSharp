using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class VisualEdgeWeightedGraph
    {
        private List<LinkedList<VisualEdge>> adjacent;

        public int V { get; private set; }
        public int E { get; private set; }

        public VisualEdgeWeightedGraph()
        {
            this.V = 0;
            this.E = 0;
            adjacent = new List<LinkedList<VisualEdge>>();
        }

        private void ValidateVertex(int v)
        {
            if ((v < 0) || (v >= V))
                throw new ArgumentException(string.Format("Vertex {0} is not between [0,{1}].", v, (V - 1).ToString()));
        }

        public void AddEdge(VisualEdge e)
        {
            int v = e.Either();
            int w = e.Other(v);
            ValidateVertex(v);
            ValidateVertex(w);
            adjacent[v].AddFirst(e);
            adjacent[w].AddFirst(e);
            E++;
        }

        public void AddVertex()
        {
            this.V++;
            adjacent.Add(new LinkedList<VisualEdge>());
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
            StringBuilder s = new StringBuilder(V + " vertices  " + E + " edges\n");
            for (int v = 0; v < V; v++)
            {
                s.Append(v + ": ");
                foreach (VisualEdge e in adjacent[v])
                    s.Append(e + "; ");
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public void Clear()
        {
            this.V = 0;
            this.E = 0;
            adjacent.Clear();
        }
    }
}
