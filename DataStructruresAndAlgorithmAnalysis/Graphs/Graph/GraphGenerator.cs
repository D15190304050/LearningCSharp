using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs.UndirectedGraph
{
    using BasicDataStructures;
    using Sort;
    using Search;

    /// <summary>
    /// The GraphGenerator class provides static methods for creating various graphs.
    /// Untested.
    /// </summary>
    public static class GraphGenerator
    {
        private sealed class Edge : IComparable<Edge>
        {
            private int v;
            private int w;

            public Edge(int v, int w)
            {
                if (v < w)
                {
                    this.v = v;
                    this.w = w;
                }
                else
                {
                    this.w = v;
                    this.v = w;
                }
            }

            public int CompareTo(Edge e)
            {
                if (v < e.v)
                    return -1;
                if (v > e.v)
                    return 1;
                if (w < e.w)
                    return -1;
                if (w > e.w)
                    return 1;
                return 0;
            }
        }

        /// <summary>
        /// Check the probabiliy p given by the argument passed by the client, throw an ArgumentException if p is out of the range [0,1].
        /// </summary>
        /// <param name="p">The probabiliy given by the argument passed by the client</param>
        private static void ProbabilityRangeCheck(double p)
        {
            if (p < 0 || p > 1)
                throw new ArgumentException("Probability must between [0,1].");
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
        /// Generate a simple graph with containing V vertices and E edges.
        /// </summary>
        /// <param name="V"></param>
        /// <param name="E"></param>
        /// <returns></returns>
        public static Graph Simple(int V, int E)
        {
            if (E > (long)(V * (V - 1) / 2))
                throw new ArgumentException("Too many edges.");
            if (E < 0)
                throw new ArgumentException("Too few edges.");

            Graph G = new Graph(V);
            Random random = new Random();
            TreeSet<Edge> set = new TreeSet<Edge>();

            while (G.E < E)
            {
                int v = random.Next(V);
                int w = random.Next(V);
                if (v == w)
                    continue;

                Edge e = new Edge(v, w);
                if (!set.Contains(e))
                {
                    set.Add(e);
                    G.AddEdge(v, w);
                }
            }

            return G;
        }

        /// <summary>
        /// Generates a simple graph on V vertices, with an edge between any 2 vertices with probability p.
        /// This is sometimes referred to as Erdos-Renyi random graph model.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <param name="p">The probability of choosing an edge.</param>
        /// <returns>A simple graph on V vertices, with an edge between any 2 vertices with probability p.</returns>
        public static Graph Simple(int V, double p)
        {
            ProbabilityRangeCheck(p);

            Random random = new Random();
            Graph G = new Graph(V);

            for (int v = 0; v < V; v++)
            {
                for (int w = v + 1; w < V; w++)
                {
                    double pr = random.NextDouble();
                    if (pr < p)
                        G.AddEdge(v, w);
                }
            }

            return G;
        }

        /// <summary>
        /// Generates the complete graph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices.</param>
        /// <returns>The complete graph on V vertices.</returns>
        public static Graph Complete(int V) { return Simple(V, 1.0); }

        /// <summary>
        /// Generates a random simple bipartite graph on (V1+V2) vertices with E edges.
        /// </summary>
        /// <param name="V1">The number of vertices on one partition.</param>
        /// <param name="V2">The number of vertices on the other partition.</param>
        /// <param name="E">The number of edges.</param>
        /// <returns>A random simple bipartite graph on V1+V2 vertices with E edges.</returns>
        public static Graph Bipartite(int V1, int V2, int E)
        {
            if (E > (long)(V1 * V2))
                throw new ArgumentException("Too many edges.");
            if (E < 0)
                throw new ArgumentException("The number of edges must be non-negative.");

            int V = V1 + V2;
            Graph G = new Graph(V);
            int[] vertices = RandomVertices(V);

            TreeSet<Edge> set = new TreeSet<Edge>();
            while (G.E < E)
            {
                int i = StdRandom.Uniform(V1);
                int j = StdRandom.Uniform(V2) + V1;
                Edge e = new Edge(vertices[i], vertices[j]);
                if (!set.Contains(e))
                {
                    set.Add(e);
                    G.AddEdge(vertices[i], vertices[j]);
                }
            }

            return G;
        }

        /// <summary>
        /// Generate a random simple bipartite graph on (V1+V2) edges, containing each posibile edge with probablity p.
        /// </summary>
        /// <param name="V1">The number of vertices in one part.</param>
        /// <param name="V2">The number of vertices in the other part.</param>
        /// <param name="p">The probablity that the graph contains an edge with one end-point in either side.</param>
        /// <returns>A random simple bipartite graph on (V1+V2) edges, containing each posibile edge with probablity p.</returns>
        public static Graph Bipartite(int V1, int V2, double p)
        {
            ProbabilityRangeCheck(p);

            int V = V1 + V2;
            int[] vertices = RandomVertices(V);
            Graph G = new Graph(V);

            for (int i = 0; i < V1; i++)
            {
                for (int j = 0; j < V2; j++)
                {
                    if (StdRandom.Bernouli(p))
                        G.AddEdge(vertices[i], vertices[V1 + j]);
                }
            }

            return G;
        }

        /// <summary>
        /// Generates a complete bipartite graph on V1 and V2 vertices.
        /// </summary>
        /// <param name="V1">The number of vertices in one partition.</param>
        /// <param name="V2">The number of vertices in the other partition.</param>
        /// <returns>A complete bipartite graph on V1 and V2 vertices.</returns>
        public static Graph CompleteBipartite(int V1, int V2) { return Bipartite(V1, V2, V1 * V2); }

        /// <summary>
        /// Generates a path graph on V verticexs.
        /// </summary>
        /// <param name="V">The number of vertices on the path.</param>
        /// <returns>A path graph on V verticexs.</returns>
        public static Graph Path(int V)
        {
            Graph G = new Graph(V);
            int[] vertices = RandomVertices(V);
            for (int i = 0; i < V - 1; i++)
                G.AddEdge(vertices[i], vertices[i + 1]);
            return G;
        }

        /// <summary>
        /// Generates a complete binary tree graph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices in the binary tree.</param>
        /// <returns>A complete binary tree graph on V vertices.</returns>
        public static Graph BinaryTree(int V)
        {
            Graph G = new Graph(V);
            int[] vertices = RandomVertices(V);
            for (int i = 0; i < V; i++)
                G.AddEdge(vertices[i], vertices[(i - 1) / 2]);
            return G;
        }

        /// <summary>
        /// Returns a cycle graph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices in the cycle.</param>
        /// <returns>A cycle graph on V vertices.</returns>
        public static Graph Cycle(int V)
        {
            Graph G = new Graph(V);
            int[] vertices = RandomVertices(V);
            for (int i = 0; i < V - 1; i++)
                G.AddEdge(vertices[i], vertices[i + 1]);
            G.AddEdge(vertices[0], vertices[V - 1]);
            return G;
        }

        /// <summary>
        /// Generates an Eulerian cycle graph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices in the cycle.</param>
        /// <param name="E">The number of edges in the cycle.</param>
        /// <returns>An Eulerian cycle graph on V vertices.</returns>
        public static Graph EulerianCycle(int V, int E)
        {
            if (V <= 0)
                throw new ArgumentException("An Eulerian cycle must have at least one vertex.");
            if (E <= 0)
                throw new ArgumentException("An Eulerian cycle must have at least one edge.");
            Graph G = new Graph(V);
            int[] vertices = new int[E];
            for (int i = 0; i < E; i++)
                vertices[i] = StdRandom.Uniform(V);
            for (int i = 0; i < E - 1; i++)
                G.AddEdge(vertices[i], vertices[i + 1]);
            G.AddEdge(vertices[0], vertices[E - 1]);
            return G;
        }

        /// <summary>
        /// Generates an Eulerian path graph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices in the path.</param>
        /// <param name="E">The number of edges in the path.</param>
        /// <returns>An Eulerian path graph on V vertices.</returns>
        public static Graph EulerianPath(int V, int E)
        {
            if (E < 0)
                throw new ArgumentException("Negative number of edges.");
            if (V <= 0)
                throw new ArgumentException("An Eulerian path must have at least one vertex.");
            Graph G = new Graph(V);
            int[] vertices = new int[E + 1];
            for (int i = 0; i < E + 1; i++)
                vertices[i] = StdRandom.Uniform(V);
            for (int i = 0; i < E; i++)
                G.AddEdge(vertices[i], vertices[i + 1]);
            return G;
        }

        /// <summary>
        /// Generates a wheel graph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices in the wheel.</param>
        /// <returns>A wheel graph on V vertices: a single vertex connected to every vertex in a cycle on (V-1) vertices.</returns>
        public static Graph Wheel(int V)
        {
            if (V <= 1)
                throw new ArgumentException("Number of vertices must be at least 2.");
            Graph G = new Graph(V);
            int[] vertices = RandomVertices(V);

            // Simple cycle on (V-1) vertices.
            for (int i = 1; i < V - 1; i++)
                G.AddEdge(vertices[i], vertices[i + 1]);
            G.AddEdge(vertices[1], vertices[V - 1]);

            // Connect vertices[0] to everty vertex on cycle.
            for (int i = 1; i < V; i++)
                G.AddEdge(vertices[0], vertices[i]);

            return G;
        }

        /// <summary>
        /// Generates a star graph on V vertices.
        /// </summary>
        /// <param name="V">The number of vertices in the star.</param>
        /// <returns>A star graph on V vertices: a single vertex connected to every other vertex.</returns>
        public static Graph Star(int V)
        {
            if (V <= 0)
                throw new ArgumentException("Number of vertices must be positive.");
            Graph G = new Graph(V);
            int[] vertices = RandomVertices(V);

            // Connect vertices[0] to every other vertex.
            for (int i = 1; i < V; i++)
                G.AddEdge(vertices[0], vertices[i]);

            return G;
        }

        /// <summary>
        /// Generates a uniformly random k-regular graph on V vertices (not necessarily simple).
        /// The graph is simple with probability only about e^(-k^2/4), which is tiny when k = 14.
        /// </summary>
        /// <param name="V">The number of vertices in the graph.</param>
        /// <param name="k">The number of edges connected to every vertex.</param>
        /// <returns>A uniformly random k-regular graph on V vertices (not necessarily simple).</returns>
        public static Graph Regular(int V, int k)
        {
            if (V * k % 2 != 0)
                throw new ArgumentException("Number of vertices * k must be even.");

            Graph G = new Graph(V);
            int[] vertices = new int[V * k];

            // Create k copies of each vertex.
            for (int v = 0; v < V; v++)
            {
                for (int i = 0; i < k; i++)
                    vertices[v + V * i] = v;
            }

            // Pick a random perfect matching.
            StdRandom.Shuffle(vertices);
            for (int i = 0; i < V * k / 2; i++)
                G.AddEdge(vertices[i * 2], vertices[i * 2 + 1]);

            return G;
        }

        /// <summary>
        /// Generates a uniformly random tree on V vertices.
        /// This algorithm uses a Prufer sequence and takes time proportional to VlogV.
        /// </summary>
        /// <param name="V">The number of vertices in the tree.</param>
        /// <returns>A uniformly random tree on V vertices.</returns>
        public static Graph Tree(int V)
        {
            Graph G = new Graph(V);

            // Special case.
            if (V == 1)
                return G;

            // Cayley's theorem: there are V^(V-2) labeled trees on V vertices.
            // Prufer sequence: sequence of (V-2) values between 0 and (V-1).
            // Prufer's proof of theorem: Prufer sequence are in 1-1 with labeled trees on V vertices.
            int[] prufer = new int[V - 2];
            for (int i = 0; i < V - 2; i++)
                prufer[i] = StdRandom.Uniform(V);

            // Degree of vertex v = 1 + number of times it appears in Prufer sequence.
            int[] degree = new int[V];
            for (int v = 0; v < V; v++)
                degree[v] = 1;
            for (int i = 0; i < V - 2; i++)
                degree[prufer[i]]++;

            // pq contains all vertices of degree 1.
            MinPriorityQueue<int> pq = new MinPriorityQueue<int>();
            for (int v = 0; v < V; v++)
            {
                if (degree[v] == 1)
                    pq.Add(v);
            }

            // Repeatedly DeleteMin() degree 1 vertex that has the min index.
            for (int i = 0; i < V - 2; i++)
            {
                int v = pq.DeleteMin();
                G.AddEdge(v, prufer[i]);
                degree[v]--;
                degree[prufer[i]]--;
                if (degree[prufer[i]] == 1)
                    pq.Add(prufer[i]);
            }
            G.AddEdge(pq.DeleteMin(), pq.DeleteMin());

            return G;
        }
    }
}