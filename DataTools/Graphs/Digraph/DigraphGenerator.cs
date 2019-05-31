using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphs.DirectedGraph
{
    using Collections;
    using Search;

    /// <summary>
    /// The DigraphGenerator class provides static methods for creating various digraphs.
    /// </summary>
    public static class DigraphGenerator
    {
        /// <summary>
        /// The edge class represents a data type of directed edge.
        /// </summary>
        private sealed class Edge : IComparable<Edge>
        {
            // The source vertex of this edge.
            private int v;

            // The target vertex of this edge.
            private int w;

            /// <summary>
            /// Generates the directed edge v->w.
            /// </summary>
            /// <param name="v">The source vertex.</param>
            /// <param name="w">The target vertex.</param>
            public Edge(int v, int w)
            {
                this.v = v;
                this.w = w;
            }

            /// <summary>
            /// Compare 2 edges.
            /// </summary>
            /// <param name="that">The other edge.</param>
            /// <returns>1 if this > that, -1 if this &lt; that, 0 if this == that.</returns>
            public int CompareTo(Edge that)
            {
                if (v < that.v)
                    return -1;
                if (v > that.v)
                    return 1;
                if (w < that.w)
                    return -1;
                if (w > that.w)
                    return 1;
                return 0;
            }
        }

        /// <summary>
        /// Check the probabiliy p given by the argument passed by the client, throw an ArgumentException if p is out of the range [0,1].
        /// </summary>
        /// <param name="p">The probabiliy given by the argument passed by the client</param>
        private static void ProbablityRangeCheck(double p)
        {
            if (p < 0 || p > 1)
                throw new ArgumentException("Probability must between [0,1].");
        }

        /// <summary>
        /// Check the range of V and E given by the argument passed by the client, throw an ArgumentException if E is out of range.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        private static void VECheck(int V, int E)
        {
            if (E > (long)V * (V - 1))
                throw new ArgumentException("Too many edges.");
            if (E < 0)
                throw new ArgumentException("Too few edges.");
        }

        /// <summary>
        /// A hepler method for Bipartite.
        /// Generate an int array with values from 0 to V-1 uniform distributed.
        /// </summary>
        /// <param name="V"></param>
        /// <returns></returns>
        public static int[] RandomVertices(int V)
        {
            int[] vertices = new int[V];
            for (int i = 0; i < V; i++)
                vertices[i] = i;
            StdRandom.Shuffle(vertices);
            return vertices;
        }

        /// <summary>
        /// Generates a random simple digraph containing V vertices and E edges.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        /// <returns>A random simple digraph containing V vertices and E edges.</returns>
        public static Digraph Simple(int V, int E)
        {
            VECheck(V, E);

            Digraph G = new Digraph(V);
            TreeSet<Edge> set = new TreeSet<Edge>();
            while (G.E < E)
            {
                int v = StdRandom.Uniform(V);
                int w = StdRandom.Uniform(V);
                Edge e = new Edge(v, w);
                if ((v != w) && (!set.Contains(e)))
                {
                    set.Add(e);
                    G.AddEdge(v, w);
                }
            }

            return G;
        }

        /// <summary>
        /// Generates a random simple digraph on V vertices, with an edge between any 2 vertices with probablity p.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="p">The probablity of choosing an edge.</param>
        /// <returns>A random simple digraph on V vertices, with an edge between any 2 vertices with probablity p.</returns>
        public static Digraph Simple(int V, double p)
        {
            ProbablityRangeCheck(p);

            Digraph G = new Digraph(V);
            for (int v = 0; v < V; v++)
            {
                for (int w = 0; w < V; w++)
                {
                    if (v != w)
                    {
                        if (StdRandom.Bernouli(p))
                            G.AddEdge(v, w);
                    }
                }
            }

            return G;
        }

        /// <summary>
        /// Generates the complete digraph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <returns>The complete digraph on V vertices.</returns>
        public static Digraph Complete(int V) { return Simple(V, V * (V - 1)); }

        /// <summary>
        /// Generates a random simple directed acyclic graph containing V vertices and E edges.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        /// <returns>A random simple directed acyclic graph containing V vertices and E edges.</returns>
        public static Digraph DirectedAcyclicGraph(int V, int E)
        {
            VECheck(V, E);

            Digraph G = new Digraph(V);
            TreeSet<Edge> set = new TreeSet<Edge>();
            int[] vertices = RandomVertices(V);

            while (G.E < E)
            {
                int v = StdRandom.Uniform(V);
                int w = StdRandom.Uniform(V);
                Edge e = new Edge(v, w);
                if ((v < w) && (!set.Contains(e)))
                {
                    set.Add(e);
                    G.AddEdge(v, w);
                }
            }

            return G;
        }

        /// <summary>
        /// Generates a random tournament digraph on V vertices.
        /// A tournament is a DAG in which for every 2 vertices, there is one directed edge.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <returns>A random tournament digraph on V vertices.</returns>
        public static Digraph Tournament(int V)
        {
            Digraph G = new Digraph(V);
            for (int v = 0; v < V; v++)
            {
                for (int w = v + 1; w < V; w++)
                {
                    if (StdRandom.Bernouli())
                        G.AddEdge(v, w);
                    else
                        G.AddEdge(w, v);
                }
            }
            return G;
        }

        /// <summary>
        /// Generates a random rooted-in DAG on V vertices and E edges.
        /// A rooted-in tree is a DAG which there is a single vertex reachable from every other vertex.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        /// <returns>A random rooted-in DAG on V vertices and E edges.</returns>
        public static Digraph RootedInDAG(int V, int E)
        {
            VECheck(V, E);

            Digraph G = new Digraph(V);
            TreeSet<Edge> set = new TreeSet<Edge>();

            // Fix a topological roder.
            int[] vertices = RandomVertices(V);

            // One edge pointing from each vertex, other than the root = vertices[V - 1].
            for (int v = 0; v < V - 1; v++)
            {
                int w = StdRandom.Uniform(v + 1, V);
                Edge e = new Edge(v, w);
                set.Add(e);
                G.AddEdge(vertices[v], vertices[w]);
            }

            while (G.E < E)
            {
                int v = StdRandom.Uniform(V);
                int w = StdRandom.Uniform(V);
                Edge e = new Edge(v, w);
                if ((v < w) && (!set.Contains(e)))
                {
                    set.Add(e);
                    G.AddEdge(vertices[v], vertices[w]);
                }
            }

            return G;
        }

        /// <summary>
        /// Generates a random rooted-out DAG on V vertices and E edges.
        /// A rooted-out tree is a DAG which in which every vertex is reachable from a single vertex.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        /// <returns>A random rooted-out DAG on V vertices and E edges.</returns>
        public static Digraph RootedOutDAG(int V, int E)
        {
            VECheck(V, E);

            Digraph G = new Digraph(V);
            TreeSet<Edge> set = new TreeSet<Edge>();

            // Fix a topological order.
            int[] vertices = RandomVertices(V);

            // One edge pointing from each vertex, other than the root = vertices[V - 1].
            for (int v = 0; v < V - 1; v++)
            {
                int w = StdRandom.Uniform(v + 1, V);
                Edge e = new Edge(w, v);
                set.Add(e);
                G.AddEdge(vertices[w], vertices[v]);
            }

            while (G.E < E)
            {
                int v = StdRandom.Uniform(V);
                int w = StdRandom.Uniform(V);
                Edge e = new Edge(w, v);
                if ((v < w) && (!set.Contains(e)))
                {
                    set.Add(e);
                    G.AddEdge(vertices[w], vertices[v]);
                }
            }

            return G;
        }

        /// <summary>
        /// Generates a random rooted-in tree on V vertices.
        /// A rooted-in tree is an oriented tree in which there is a single vertex reachable from every other vertex.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <returns>A random rooted-in tree on V vertices.</returns>
        public static Digraph RootedInTree(int V) { return RootedInDAG(V, V - 1); }

        /// <summary>
        /// Generates a random rooted-out tree on V vertices.
        /// A rooted-out tree is an oriented tree in which each vertex is reachable from a single vertex.
        /// </summary>
        /// <param name="V"></param>
        /// <returns></returns>
        public static Digraph RootedOutTree(int V) { return RootedOutDAG(V, V - 1); }

        /// <summary>
        /// Generates a path digraph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices in this path.</param>
        /// <returns>A path digraph on V vertices.</returns>
        public static Digraph Path(int V)
        {
            Digraph G = new Digraph(V);
            int[] vertices = RandomVertices(V);
            for (int v = 0; v < V - 1; v++)
                G.AddEdge(vertices[v], vertices[v + 1]);
            return G;
        }

        /// <summary>
        /// Returns a complete binary tree digraph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <returns>A complete binary tree digraph on V vertices.</returns>
        public static Digraph BinaryTree(int V)
        {
            Digraph G = new Digraph(V);
            int[] vertices = RandomVertices(V);
            for (int i = 0; i < V; i++)
                G.AddEdge(vertices[i], vertices[(i - 1) / 2]);
            return G;
        }

        /// <summary>
        /// Generates a cycle digraph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices in the cycle.</param>
        /// <returns>A cycle digraph on V vertices.</returns>
        public static Digraph Cycle(int V)
        {
            Digraph G = new Digraph(V);
            int[] vertices = RandomVertices(V);
            for (int v = 0; v < V - 1; v++)
                G.AddEdge(vertices[v], vertices[v + 1]);
            G.AddEdge(vertices[V - 1], vertices[0]);
            return G;
        }

        /// <summary>
        /// Generates an Eulerian cycle digraph on V vertices and E edges..
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        /// <returns>An Eulerian cycle digraph on V vertices.</returns>
        public static Digraph EulerianCycle(int V, int E)
        {
            if (E <= 0)
                throw new ArgumentException("An Eulerian cycle must have at least one edge.");
            if (V <= 0)
                throw new ArgumentException("An Euleriaan cycle must have at least one vertex.");

            Digraph G = new Digraph(V);
            int[] vertices = new int[E];
            for (int i = 0; i < E; i++)
                vertices[i] = StdRandom.Uniform(V);
            for (int i = 0; i < E - 1; i++)
                G.AddEdge(vertices[i], vertices[i + 1]);
            G.AddEdge(vertices[E - 1], vertices[0]);

            return G;
        }

        /// <summary>
        /// Generates an Eulerian path digraph on V vertices and E edges.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        /// <returns>An Eulerian path digraph on V vertices and E edges.</returns>
        public static Digraph EulerianPath(int V, int E)
        {
            if (E < 0)
                throw new ArgumentException("Negative number of edges.");
            if (V <= 0)
                throw new ArgumentException("An Eulerian path must have at least one vertex.");

            Digraph G = new Digraph(V);
            int[] vertices = new int[E + 1];
            for (int i = 0; i < E + 1; i++)
                vertices[i] = StdRandom.Uniform(V);
            for (int i = 0; i < E; i++)
                G.AddEdge(vertices[i], vertices[i + 1]);

            return G;
        }

        /// <summary>
        /// Generates a random simple digraph on V vertices and E edges and (at most) c strong components.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="E">The number of edges.</param>
        /// <param name="c">The (max) number of strong components.</param>
        /// <returns>A random simple digraph on V vertices and E edges and (at most) c strong components.</returns>
        public static Digraph Strong(int V, int E, int c)
        {
            if (c > V || c <= 0)
                throw new ArgumentException("Number of components must be between [1,V].");
            if (E < 2 * (V - c))
                throw new ArgumentException("Number of edges must be at least 2*(V-c).");
            if (E > (long)V * (V - 1) / 2)
                throw new ArgumentException("Too many edges");

            Digraph G = new Digraph(V);
            TreeSet<Edge> set = new TreeSet<Edge>();

            int[] label = new int[V];
            for (int v = 0; v < V; v++)
                label[v] = StdRandom.Uniform(c);

            // Mark all vertices with label c a strong component by combining rooted-in tree and rooted-out tree.
            for (int i = 0; i < c; i++)
            {
                // How many vertices in component c.
                int count = 0;
                for (int v = 0; v < V; v++)
                {
                    if (label[v] == i)
                        count++;
                }

                //if (count == 0)
                //    Console.Error.WriteLine("Less than desired number of strong components.");

                // Copy vertices with label c into vertices[].
                int[] vertices = new int[count];
                int j = 0;
                for (int v = 0; v < V; v++)
                {
                    if (label[v] == i)
                        vertices[j++] = v;
                }
                StdRandom.Shuffle(vertices);

                // Rooted-in tree with root = vertices[count - 1].
                for (int v = 0; v < count - 1; v++)
                {
                    int w = StdRandom.Uniform(v + 1, V);
                    Edge e = new Edge(w, v);
                    set.Add(e);
                    G.AddEdge(vertices[w], vertices[v]);
                }

                // Rooted-out tree with root = vertices[count - 1].
                for (int v = 0; v < count - 1; v++)
                {
                    int w = StdRandom.Uniform(v + 1, V);
                    Edge e = new Edge(v, w);
                    set.Add(e);
                    G.AddEdge(vertices[v], vertices[w]);
                }
            }

            while (G.E < E)
            {
                int v = StdRandom.Uniform(V);
                int w = StdRandom.Uniform(V);
                Edge e = new Edge(v, w);
                if ((!set.Contains(e)) && (v != w) && (label[v] < label[w]))
                {
                    set.Add(e);
                    G.AddEdge(v, w);
                }
            }

            return G;
        }
    }
}