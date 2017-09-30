using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.Graphs
{
    using BasicDataStructures;
    using UndirectedGraph;
    using DirectedGraph;
    using EdgeWeightedUndirectedGraph;
    using EdgeWeightedDirectedGraph;

    /// <summary>
    /// A static class used to make unit test for graph data structures and algorithm. All files will be packaged somewhere.
    /// </summary>
    internal static class UnitTest
    {
        // Relative path of text which stores the graphs and digraphs.
        private const string tinyG = @"Test Data\tinyG.txt";
        private const string tinyCG = @"Test Data\tinyCG.txt";
        private const string tinyDG = @"Test data\tinyDG.txt";
        private const string tinyDAG = @"Test data\tinyDAG.txt";
        private const string routes = @"Test data\routes.txt";
        private const string tinyEWG = @"Test data\tinyEWG.txt";
        private const string tinyEWD = @"Test data\tinyEWD.txt";
        private const string tinyEWDAG = @"Test data\tinyEWDAG.txt";
        private const string jobsPC = @"Test data\jobsPC.txt";
        private const string tinyEWDn = @"Test data\tinyEWDn.txt";
        private const string rates = @"Test data\rates.txt";

        /// <summary>
        /// Unit test method for Graph.
        /// </summary>
        public static void GraphUnitTest()
        {
            // Load graph from file.
            Graph G = new Graph(tinyG);

            // Print the graph.
            Console.WriteLine("Print the graph by the format of adjacency list:");
            Console.WriteLine(G);

            // Test the deep-copy constructor.
            Graph G2 = new Graph(G);
            Console.WriteLine("Print the graph construct by the first graph:");
            Console.WriteLine(G2);
        }
        /* Output:
            Print the graph by the format of adjacency list:
            13 vertices 13 edges
            0: 6 2 1 5
            1: 0
            2: 0
            3: 5 4
            4: 5 6 3
            5: 3 4 0
            6: 0 4
            7: 8
            8: 7
            9: 11 10 12
            10: 9
            11: 9 12
            12: 11 9

            Print the graph construct by the first graph:
            13 vertices 13 edges
            0: 6 2 1 5
            1: 0
            2: 0
            3: 5 4
            4: 5 6 3
            5: 3 4 0
            6: 0 4
            7: 8
            8: 7
            9: 11 10 12
            10: 9
            11: 9 12
            12: 11 9

            */

        /// <summary>
        /// Unit test method for DepthFirstSearch.
        /// </summary>
        public static void DFSUnitTest()
        {
            Graph G = new Graph(tinyG);

            // Run depth first search by vertex 0.
            int source1 = 0;
            DepthFirstSearch Dfs = new DepthFirstSearch(G, source1);
            GraphSearchBaseUnitTest(Dfs, G.V);
            Console.WriteLine();

            // Run depth first search by vertex 9.
            int source2 = 9;
            Dfs = new DepthFirstSearch(G, source2);
            GraphSearchBaseUnitTest(Dfs, G.V);
        }
        /* Output:
            0 1 2 3 4 5 6
            Not Connected.

            9 10 11 12
            Not Connected.
            */

        /// <summary>
        /// A helper method to test graph search algorithm.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="search"></param>
        private static void GraphSearchBaseUnitTest(GraphSearchBase search, int vertexCount)
        {
            for (int v = 0; v < vertexCount; v++)
            {
                if (search.HasPathTo(v))
                    Console.Write(v + " ");
            }
            Console.WriteLine();

            if (search.Count != vertexCount)
                Console.Write("Not ");
            Console.WriteLine("Connected.");

        }

        /// <summary>
        /// Unit test method for DepthFirstPaths.
        /// </summary>
        public static void DepthFirstPathsUnitTest()
        {
            Graph G = new Graph(tinyCG);
            int source = 0;
            GraphPathBase search = new DepthFirstPaths(G, source);

            GraphPathBaseUnitTest(search, G.V);
            Console.WriteLine("There are {0} vertices conntected to the source vertex.", search.Count);
        }
        /* Output:
            0 to 0: 0
            0 to 1: 0-2-1
            0 to 2: 0-2
            0 to 3: 0-2-3
            0 to 4: 0-2-3-4
            0 to 5: 0-2-3-5
            */

        /// <summary>
        /// Unit test method for BreadthFirstPaths.
        /// </summary>
        public static void BreadthFirstPathsUnitTest()
        {
            Graph G = new Graph(tinyCG);
            int source = 0;
            GraphPathBase search = new BreadthFirstPaths(G, source);

            GraphPathBaseUnitTest(search, G.V);
            Console.WriteLine("There are {0} vertices conntected to the source vertex.", search.Count);
        }
        /* Output:
            0 to 0: 0
            0 to 1: 0-1
            0 to 2: 0-2
            0 to 3: 0-2-3
            0 to 4: 0-2-4
            0 to 5: 0-5
            */

        private static void GraphPathBaseUnitTest(GraphPathBase search, int vertexCount)
        {
            for (int v = 0; v < vertexCount; v++)
            {
                Console.Write(search.Source + " to " + v + ": ");
                if (search.HasPathTo(v))
                {
                    foreach (int x in search.PathTo(v))
                    {
                        if (x == search.Source)
                            Console.Write(x);
                        else
                            Console.Write("-" + x);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Unit test method for ConnectedComponents.
        /// </summary>
        public static void ConnectedComponentsUnitTest()
        {
            // Compute the connected components from a graph.
            Graph G = new Graph(tinyG);
            ConnectedComponents cc = new ConnectedComponents(G);

            // Create linked list to store the vertices in same connected components.
            int componentsCount = cc.Count;
            LinkedList<int>[] components = new LinkedList<int>[componentsCount];
            for (int i = 0; i < componentsCount; i++)
                components[i] = new LinkedList<int>();

            // Store the vertices.
            for (int v = 0; v < G.V; v++)
                components[cc.Id[v]].AddFirst(v);

            // Print the connected components.
            Console.WriteLine(componentsCount + " components.");
            for (int i = 0; i < componentsCount; i++)
            {
                foreach (int v in components[i])
                    Console.Write(v + " ");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Unit test for Cycle.
        /// </summary>
        public static void CycleUnitTest()
        {
            // Test the graph tinyG.
            Graph G = new Graph(tinyG);
            CycleUnitTestHelper(G);

            // Test the graph tinyCG.
            G = new Graph(tinyCG);
            CycleUnitTestHelper(G);
        }
        /* Output:
            3 4 5 3
            1 0 2 1
            */

        /// <summary>
        /// A hepler method for CycleUnitTest.
        /// </summary>
        /// <param name="c"></param>
        private static void CycleUnitTestHelper(Graph G)
        {
            Cycle c = new Cycle(G);

            if (c.HasCycle)
            {
                foreach (int v in c.GetCycle())
                    Console.Write(v + " ");
                Console.WriteLine();
            }
            else
                Console.WriteLine("Graph is acyclic");
        }

        /// <summary>
        /// Unit test method for SymbolGraph.
        /// </summary>
        public static void SymbolGraphUnitTest()
        {
            // Prepare the file name and delimiter to build the symbol graph.
            string delimiter = " ";

            // Build the symbol graph and get the graph object.
            SymbolGraph sg = new SymbolGraph(routes, delimiter);
            Graph G = sg.Graph;

            // Declare 2 strings for test.
            string[] testVertex = { "JFK", "LAX" };

            for (int v = 0; v < G.V; v++)
            {
                Console.WriteLine(sg.NameOf(v) + ":");
                foreach (int w in G.Adjacent(v))
                    Console.WriteLine("\t" + sg.NameOf(w));
            }

            // Test the Contains() method.
            Console.WriteLine("Contains key {0}, {1}.", "JFK", sg.Contains("JFK"));
            Console.WriteLine("Contains key {0}, {1}.", "JPG", sg.Contains("JPG"));
        }
        /* Output:
            JFK:
                    ORD
                    ATL
                    MCO
            MCO:
                    HOU
                    ATL
                    JFK
            ORD:
                    ATL
                    JFK
                    PHX
                    DFW
                    HOU
                    DEN
            DEN:
                    LAS
                    PHX
                    ORD
            HOU:
                    MCO
                    DFW
                    ATL
                    ORD
            DFW:
                    HOU
                    ORD
                    PHX
            PHX:
                    LAS
                    LAX
                    DEN
                    ORD
                    DFW
            ATL:
                    MCO
                    ORD
                    HOU
                    JFK
            LAX:
                    LAS
                    PHX
            LAS:
                    PHX
                    LAX
                    DEN
            Contains key JFK, True.
            Contains key JPG, False.
            */

        /// <summary>
        /// Unit test method for GraphGenerator.
        /// </summary>
        public static void GraphGeneratorUnitTest()
        {
            int V = 5;
            int E = 6;
            int V1 = V / 2;
            int V2 = V - V1;

            Console.WriteLine("Complete graph:");
            Console.WriteLine(GraphGenerator.Complete(V));

            Console.WriteLine("Simple:");
            Console.WriteLine(GraphGenerator.Simple(V, E));

            Console.WriteLine("Erdos-Renyi:");
            double p = E / (V * (V - 1) / 2.0);
            Console.WriteLine(GraphGenerator.Simple(V, p));

            Console.WriteLine("Complete bipartite:");
            Console.WriteLine(GraphGenerator.CompleteBipartite(V1, V2));

            Console.WriteLine("Bipartite:");
            Console.WriteLine(GraphGenerator.Bipartite(V1, V2, E));

            Console.WriteLine("Erdos Renyi bipartite:");
            p = (double)E / (V1 * V2);
            Console.WriteLine(GraphGenerator.Bipartite(V1, V2, p));

            Console.WriteLine("Path:");
            Console.WriteLine(GraphGenerator.Path(V));

            Console.WriteLine("Cycle:");
            Console.WriteLine(GraphGenerator.Cycle(V));

            Console.WriteLine("Binary tree:");
            Console.WriteLine(GraphGenerator.BinaryTree(V));

            Console.WriteLine("Tree:");
            Console.WriteLine(GraphGenerator.Tree(V));

            Console.WriteLine("2-Regular:");
            Console.WriteLine(GraphGenerator.Regular(V, 2));

            Console.WriteLine("Star:");
            Console.WriteLine(GraphGenerator.Star(V));

            Console.WriteLine("Wheel:");
            Console.WriteLine(GraphGenerator.Wheel(V));
        }
        /* Sample Output (output dependent on the random distribution):
            Complete graph:
            5 vertices 10 edges
            0: 4 3 2 1
            1: 4 3 2 0
            2: 4 3 1 0
            3: 4 2 1 0
            4: 3 2 1 0

            Simple:
            5 vertices 6 edges
            0: 4
            1: 4 2 3
            2: 4 1 3
            3: 1 2
            4: 2 1 0

            Erdos-Renyi:
            5 vertices 5 edges
            0: 4 2
            1: 4 3
            2: 4 0
            3: 1
            4: 2 1 0

            Complete bipartite:
            5 vertices 6 edges
            0: 2 4 1
            1: 0 3
            2: 0 3
            3: 4 2 1
            4: 0 3

            Bipartite:
            5 vertices 6 edges
            0: 2 1 3
            1: 4 0
            2: 0 4
            3: 4 0
            4: 1 2 3

            Erdos Renyi bipartite:
            5 vertices 6 edges
            0: 1 4
            1: 3 0 2
            2: 1 4
            3: 1 4
            4: 3 0 2

            Path:
            5 vertices 4 edges
            0: 4
            1: 4 2
            2: 1 3
            3: 2
            4: 0 1

            Cycle:
            5 vertices 5 edges
            0: 3 4
            1: 3 2
            2: 4 1
            3: 1 0
            4: 0 2

            Binary tree:
            5 vertices 5 edges
            0: 2
            1: 3
            2: 0 3 2 2
            3: 1 4 2
            4: 3

            Tree:
            5 vertices 4 edges
            0: 4
            1: 2
            2: 4 1
            3: 4
            4: 3 2 0

            2-Regular:
            5 vertices 5 edges
            0: 0 0
            1: 3 4
            2: 4 3
            3: 2 1
            4: 2 1

            Star:
            5 vertices 4 edges
            0: 3
            1: 3
            2: 3
            3: 1 0 4 2
            4: 3

            Wheel:
            5 vertices 8 edges
            0: 1 3 4
            1: 3 0 4 2
            2: 1 3 4
            3: 1 2 0
            4: 1 0 2
            */

        /// <summary>
        /// Unit test method for Digraph.
        /// </summary>
        public static void DigraphUnitTest()
        {
            // Load graph from file.
            Digraph G = new Digraph(tinyDG);

            // Print the graph.
            Console.WriteLine("Print the digraph by the format of adjacency lists.");
            Console.WriteLine(G);

            // Test the deep-copy constructor.
            Digraph G2 = new Digraph(G);
            Console.WriteLine("Print the graph construct by the first digraph.");
            Console.WriteLine(G2);
        }
        /* Output:
            Print the digraph by the format of adjacency lists.
            13 vertices and 22 edges
            0: 5 1
            1:
            2: 0 3
            3: 5 2
            4: 3 2
            5: 4
            6: 9 4 0
            7: 6 8
            8: 7 9
            9: 11 10
            10: 12
            11: 4 12
            12: 9

            Print the graph construct by the first digraph.
            13 vertices and 22 edges
            0: 5 1
            1:
            2: 0 3
            3: 5 2
            4: 3 2
            5: 4
            6: 9 4 0
            7: 6 8
            8: 7 9
            9: 11 10
            10: 12
            11: 4 12
            12: 9
            */

        /// <summary>
        /// Unit test method for DigraphDFS.
        /// </summary>
        public static void DigraphDFSUnitTest()
        {
            // Read in digraph from file.
            Digraph G = new Digraph(tinyDG);

            // Set sources.
            int source1 = 1;
            int source2 = 2;

            // Run Digraph DFS for source1.
            DirectedDFS search = new DirectedDFS(G, source1);
            DigraphDFSHepler(search, G.V);
            Console.WriteLine();

            // Run Digraph DFS for source2.
            search = new DirectedDFS(G, source2);
            DigraphDFSHepler(search, G.V);
        }
        /* Output:
            1
            0 1 2 3 4 5
            */

        /// <summary>
        /// A hepler method for DigraphDFSUnitTest().
        /// </summary>
        /// <param name="search">The DigraphDFS object.</param>
        /// <param name="V">The number of G's vertices.</param>
        private static void DigraphDFSHepler(DirectedDFS search, int V)
        {
            for (int v = 0; v < V; v++)
            {
                if (search.HasPathTo(v))
                    Console.Write(v + " ");
            }
        }

        /// <summary>
        /// Unit test method for DepthFirstDirectedPaths.
        /// </summary>
        public static void DepthFirstDirectedPathsUnitTest()
        {
            // Specify the digraph and the source vertex.
            Digraph G = new Digraph(tinyDG);
            int source = 3;

            DepthFirstDirectedPaths dfs = new DepthFirstDirectedPaths(G, source);

            for (int v = 0; v < G.V; v++)
            {
                if (dfs.HasPathTo(v))
                {
                    Console.Write("{0} to {1}: ", source, v);
                    foreach (int w in dfs.PathTo(v))
                    {
                        if (w == source)
                            Console.Write(w);
                        else
                            Console.Write("-" + w);
                    }
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("{0} to {1}: not connected.", source, v);
            }
        }
        /* Output:
            3 to 0: 3-5-4-2-0
            3 to 1: 3-5-4-2-0-1
            3 to 2: 3-5-4-2
            3 to 3: 3
            3 to 4: 3-5-4
            3 to 5: 3-5
            3 to 6: not connected.
            3 to 7: not connected.
            3 to 8: not connected.
            3 to 9: not connected.
            3 to 10: not connected.
            3 to 11: not connected.
            3 to 12: not connected.
            */

        public static void BreadthFirstDirectedPathsUnitTest()
        {
            // Specify the digraph and the source vertex.
            Digraph G = new Digraph(tinyDG);
            int source = 3;

            BreadthFirstDirectedPaths search = new BreadthFirstDirectedPaths(G, source);

            for (int v = 0; v < G.V; v++)
            {
                Console.Write("{0} to {1} ", source, v);
                if (search.HasPathTo(v))
                {
                    Console.Write("({0}): ", search.DistanceTo(v));
                    foreach (int w in search.PathTo(v))
                    {
                        if (w == source)
                            Console.Write(w);
                        else
                            Console.Write("->" + w);
                    }
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("(-): not connected.");
            }
        }
        /* Output:
            3 to 0 (2): 3->2->0
            3 to 1 (3): 3->2->0->1
            3 to 2 (1): 3->2
            3 to 3 (0): 3
            3 to 4 (2): 3->5->4
            3 to 5 (1): 3->5
            3 to 6 (-): not connected.
            3 to 7 (-): not connected.
            3 to 8 (-): not connected.
            3 to 9 (-): not connected.
            3 to 10 (-): not connected.
            3 to 11 (-): not connected.
            3 to 12 (-): not connected.
            */

        /// <summary>
        /// Unit test method for DirectedCycle.
        /// </summary>
        public static void DirectedCycleUnitTest()
        {
            DirectedCycleHepler(tinyDG);
            DirectedCycleHepler(tinyDAG);
        }
        /* Output:
            Directed cycle: 3 5 4 3
            No directed cycle.
            */

        /// <summary>
        /// A helper method for DirectedCycleUnitTest().
        /// </summary>
        /// <param name="digraph"></param>
        private static void DirectedCycleHepler(string digraph)
        {
            Digraph G = new Digraph(digraph);
            DirectedCycle cycleFinder = new DirectedCycle(G);

            if (cycleFinder.HasCycle)
            {
                Console.Write("Directed cycle: ");
                foreach (int w in cycleFinder.GetCycle())
                    Console.Write(w + " ");
                Console.WriteLine();
            }
            else
                Console.WriteLine("No directed cycle.");
        }

        /// <summary>
        /// Unit test method for DigraphGenerator.
        /// </summary>
        public static void DigraphGeneratorUnitTest()
        {
            int V = 5;
            int E = 6;

            Console.WriteLine("Complete digraph:");
            Console.WriteLine(DigraphGenerator.Complete(V));

            Console.WriteLine("Simple digraph:");
            Console.WriteLine(DigraphGenerator.Simple(V, E));

            Console.WriteLine("Path:");
            Console.WriteLine(DigraphGenerator.Path(V));

            Console.WriteLine("Cycle:");
            Console.WriteLine(DigraphGenerator.Cycle(V));

            Console.WriteLine("Eulerian path:");
            Console.WriteLine(DigraphGenerator.EulerianPath(V, E));

            Console.WriteLine("Eulerian cycle:");
            Console.WriteLine(DigraphGenerator.EulerianCycle(V, E));

            Console.WriteLine("Binary tree:");
            Console.WriteLine(DigraphGenerator.BinaryTree(V));

            Console.WriteLine("Tournament:");
            Console.WriteLine(DigraphGenerator.Tournament(V));

            Console.WriteLine("Directed Acyclic Graph:");
            Console.WriteLine(DigraphGenerator.DirectedAcyclicGraph(V, E));

            Console.WriteLine("Rooted-in DAG:");
            Console.WriteLine(DigraphGenerator.RootedInDAG(V, E));

            Console.WriteLine("Rooted-out DAG:");
            Console.WriteLine(DigraphGenerator.RootedOutDAG(V, E));

            Console.WriteLine("Rooted-in tree:");
            Console.WriteLine(DigraphGenerator.RootedInTree(V));

            Console.WriteLine("Rooted-out tree:");
            Console.WriteLine(DigraphGenerator.RootedOutTree(V));
        }
        /* Sample output:
            Complete digraph:
            5 vertices and 20 edges
            0: 2 1 3 4
            1: 4 2 0 3
            2: 0 3 1 4
            3: 2 1 0 4
            4: 1 0 3 2

            Simple digraph:
            5 vertices and 6 edges
            0:
            1: 3 2 4
            2: 3 1
            3:
            4: 3

            Path:
            5 vertices and 4 edges
            0: 1
            1:
            2: 4
            3: 2
            4: 0

            Cycle:
            5 vertices and 5 edges
            0: 4
            1: 0
            2: 1
            3: 2
            4: 3

            Eulerian path:
            5 vertices and 6 edges
            0:
            1:
            2: 4 2 3
            3: 2 2
            4: 2

            Eulerian cycle:
            5 vertices and 6 edges
            0:
            1: 1 2 1
            2: 4 2
            3:
            4: 1

            Binary tree:
            5 vertices and 5 edges
            0: 2
            1: 2
            2: 3
            3: 3
            4: 3

            Tournament:
            5 vertices and 10 edges
            0: 4 3
            1: 4 0
            2: 4 3 1 0
            3: 1
            4: 3

            Directed Acyclic Graph:
            5 vertices and 6 edges
            0: 3 1
            1: 3 2
            2: 4
            3: 4
            4:

            Rooted-in DAG:
            5 vertices and 6 edges
            0:
            1: 0
            2: 1
            3: 1 0 4
            4: 1

            Rooted-out DAG:
            5 vertices and 6 edges
            0:
            1: 0
            2: 1
            3: 0 2
            4: 3 0

            Rooted-in tree:
            5 vertices and 4 edges
            0: 4
            1: 2
            2: 4
            3: 0
            4:

            Rooted-out tree:
            5 vertices and 4 edges
            0: 3
            1:
            2:
            3: 2 4 1
            4:
            */

        public static void DirectedEulerianPathUnitTest()
        {
            int V = 5;
            int E = 6;
            Type t = typeof(DirectedEulerianPath);

            // Eulerian cycle.
            Digraph G = DigraphGenerator.EulerianCycle(V, E);
            DirectedEulerianPathHelper(G, "Eulerian cycle");

            // Eulerian path.
            G = DigraphGenerator.EulerianPath(V, E);
            DirectedEulerianPathHelper(G, "Eulerian path");

            // Add one random edge.
            G.AddEdge(StdRandom.Uniform(G.V), StdRandom.Uniform(G.V));
            DirectedEulerianPathHelper(G, "One random edge add to Eulerian path");

            // Self loop.
            G = new Digraph(V);
            int v = StdRandom.Uniform(V);
            G.AddEdge(v, v);
            DirectedEulerianPathHelper(G, "Single self loop");

            // Single edge.
            G = new Digraph(V);
            G.AddEdge(StdRandom.Uniform(V), StdRandom.Uniform(V));
            DirectedEulerianPathHelper(G, "Single edge");

            // Empty digraph.
            G = new Digraph(V);
            DirectedEulerianPathHelper(G, "Empty digraph.");
        }
        /* Sample output:
            Eulerian cycle
            ----------------------------
            5 vertices and 6 edges
            0: 2 2
            1:
            2: 3 0 2
            3: 0
            4:

            Eulerian path:
            0 2 3 0 2 2 0

            Eulerian path
            ----------------------------
            5 vertices and 6 edges
            0: 3 2
            1: 0
            2: 1
            3: 0 3
            4:

            Eulerian path:
            0 3 3 0 2 1 0

            One random edge add to Eulerian path
            ----------------------------
            5 vertices and 7 edges
            0: 3 2
            1: 0
            2: 0 1
            3: 0 3
            4:

            Eulerian path:
            2 0 3 3 0 2 1 0

            Single self loop
            ----------------------------
            5 vertices and 1 edges
            0:
            1:
            2:
            3:
            4: 4

            Eulerian path:
            4 4

            Single edge
            ----------------------------
            5 vertices and 1 edges
            0:
            1: 2
            2:
            3:
            4:

            Eulerian path:
            1 2

            Empty digraph.
            ----------------------------
            5 vertices and 0 edges
            0:
            1:
            2:
            3:
            4:

            Eulerian path:
            0
            */

        /// <summary>
        /// A helper method for DirectedEulerianPathUnitTest()
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <param name="description">The descripition of the digraph.</param>
        private static void DirectedEulerianPathHelper(Digraph G, string description)
        {
            PrintDescrpition(G, description);

            DirectedEulerianPath euler = new DirectedEulerianPath(G);
            
            Console.WriteLine("Eulerian path: ");
            if (euler.HasEulerianPath())
            {
                foreach (int v in euler.Path())
                    Console.Write(v + " ");
                Console.WriteLine();
            }
            else
                Console.WriteLine("none");

            Console.WriteLine();
        }

        public static void DirectedEulerianCycleUnitTest()
        {
            int V = 5;
            int E = 6;

            // Eulerian cycle.
            Digraph G = DigraphGenerator.EulerianCycle(V, E);
            DirectedEulerianCycleHelper(G, "Eulerian cycle");

            // Eulerian path.
            G = DigraphGenerator.EulerianPath(V, E);
            DirectedEulerianCycleHelper(G, "Eulerian path");

            // Empty digraph.
            G = new Digraph(V);
            DirectedEulerianCycleHelper(G, "Empty digraph");

            // Self loop;
            int v = StdRandom.Uniform(V);
            G.AddEdge(v, v);
            DirectedEulerianCycleHelper(G, "Self loop");

            // Union of 2 disjoint Eulerian cycles.
            Digraph H1 = DigraphGenerator.EulerianCycle(V / 2, E / 2);
            Digraph H2 = DigraphGenerator.EulerianCycle(V - V / 2, E - E / 2);
            G = new Digraph(V);
            int[] perm = new int[V];
            for (int i = 0; i < V; i++)
                perm[i] = i;
            StdRandom.Shuffle(perm);
            // Multiplex the variable declared before.
            for (v = 0; v < H1.V; v++)
            {
                foreach (int w in H1.Adjacent(v))
                    G.AddEdge(perm[v], perm[w]);
            }
            for (v = 0; v < H2.V; v++)
            {
                foreach (int w in H2.Adjacent(v))
                    G.AddEdge(perm[V / 2 + v], perm[V / 2 + w]);
            }
            DirectedEulerianCycleHelper(G, "Union of 2 disjoint Eulerian cycles");

            // Random digraph.
            G = DigraphGenerator.Simple(V, E);
            DirectedEulerianCycleHelper(G, "Simple digraph");
        }
        /* Sample output:
            Eulerian cycle
            ----------------------------
            5 vertices and 6 edges
            0: 4
            1: 4
            2: 4
            3:
            4: 1 0 2

            Eulerian cycle: 0 4 1 4 2 4 0

            Eulerian path
            ----------------------------
            5 vertices and 6 edges
            0: 3 1
            1: 0 4
            2:
            3: 1
            4: 0

            Eulerian cycle: 0 3 1 0 1 4 0

            Empty digraph
            ----------------------------
            5 vertices and 0 edges
            0:
            1:
            2:
            3:
            4:

            Eulerian cycle: None

            Self loop
            ----------------------------
            5 vertices and 1 edges
            0: 0
            1:
            2:
            3:
            4:

            Eulerian cycle: 0 0

            Union of 2 disjoint Eulerian cycles
            ----------------------------
            5 vertices and 6 edges
            0: 4
            1:
            2: 2 3
            3: 2
            4: 0 4

            Eulerian cycle: None

            Simple digraph
            ----------------------------
            5 vertices and 6 edges
            0: 4 2
            1: 4 0
            2:
            3: 4 0
            4:

            Eulerian cycle: None
            */

        /// <summary>
        /// A hepler method for DirectedEulerianCycleUnitTest().
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <param name="description">The description of the digraph.</param>
        private static void DirectedEulerianCycleHelper(Digraph G, string description)
        {
            PrintDescrpition(G, description);

            DirectedEulerianCycle euler = new DirectedEulerianCycle(G);

            Console.Write("Eulerian cycle: ");
            if (euler.HasEulerianCycle())
            {
                foreach (int w in euler.Cycle())
                    Console.Write(w + " ");
                Console.WriteLine();
            }
            else
                Console.WriteLine("None");

            Console.WriteLine();
        }

        /// <summary>
        /// A hepler method to print the digraph and its description.
        /// </summary>
        /// <param name="G">The digraph.</param>
        /// <param name="description">The description of the digraph.</param>
        private static void PrintDescrpition(Digraph G, string description)
        {
            Console.WriteLine(description);
            Console.WriteLine("----------------------------");
            Console.WriteLine(G);
        }

        /// <summary>
        /// Unit test method for DepthFirstOrder.
        /// </summary>
        public static void DepthFirstOrderUnitTest()
        {
            Digraph G = new Digraph(tinyDAG);
            DepthFirstOrder dfs = new DepthFirstOrder(G);

            // Print the pre/post number of v.
            Console.WriteLine("    v  pre  post");
            Console.WriteLine("-----------------");
            for (int v = 0; v < G.V; v++)
                Console.WriteLine("{0,4} {1,4} {2,4}", v, dfs.PreOrderNumberOf(v), dfs.PostOrderNumberOf(v));

            Console.WriteLine("Pre-order:");
            foreach (int v in dfs.PreOrder)
                Console.Write(v + " ");
            Console.WriteLine();

            Console.WriteLine("Post-order:");
            foreach (int v in dfs.PostOrder)
                Console.Write(v + " ");
            Console.WriteLine();

            Console.WriteLine("Reverse post-order:");
            foreach (int v in dfs.ReversePostOrder)
                Console.Write(v + " ");
            Console.WriteLine();
        }
        /* Output:
                v  pre  post
            -----------------
               0    0    8
               1    3    2
               2    9   10
               3   10    9
               4    2    0
               5    1    1
               6    4    7
               7   11   11
               8   12   12
               9    5    6
              10    8    5
              11    6    4
              12    7    3
            Pre-order:
            0 5 4 1 6 9 11 12 10 2 3 7 8
            Post-order:
            4 5 1 12 11 10 9 6 0 3 2 7 8
            Reverse post-order:
            8 7 2 3 0 6 9 10 11 12 1 5 4
            */

        /// <summary>
        /// Unit test method for SymbolDigraph.
        /// </summary>
        public static void SymbolDigraphUnitTest()
        {
            // Build the SymbolDigraph object by the specified file and delimiter.
            string delimiter = " ";
            SymbolDigraph sg = new SymbolDigraph(routes, delimiter);

            string[] testVertex = { "JFK", "ATL", "LAX" };
            foreach (string name in testVertex)
            {
                Console.WriteLine(name + ": ");
                int v = sg.IndexOf(name);
                foreach (int w in sg.Digraph.Adjacent(v))
                    Console.Write(" " + sg.NameOf(w));
                Console.WriteLine();
            }

            // Test the Contains() method.
            Console.WriteLine("Contains key {0}, {1}.", "JFK", sg.Contains("JFK"));
            Console.WriteLine("Contains key {0}, {1}.", "JPG", sg.Contains("JPG"));
        }
        /* Output:
            JFK:
             ORD ATL MCO
            ATL:
             MCO HOU
            LAX:

            Contains key JFK, True.
            Contains key JPG, False.
            */

        /// <summary>
        /// Unit test method for Topological.
        /// </summary>
        public static void TopologicalUnitTest()
        {
            string fullFileName = @"Test data\jobs.txt";
            string delimiter = @"/";
            SymbolDigraph sg = new SymbolDigraph(fullFileName, delimiter);
            Digraph G = sg.Digraph;
            Topological topological = new Topological(G);
            foreach (int v in topological.Order)
                Console.WriteLine(sg.NameOf(v));
        }
        /* Output:
            Calculus
            Linear Algebra
            Introduction to CS
            Advanced Programming
            Algorithms
            Theoretical CS
            Artificial Intelligence
            Robotics
            Machine Learning
            Neural Networks
            Databases
            Scientific Computing
            Computational Biology
            */

        /// <summary>
        /// Unit test method for TransitiveClosure.
        /// </summary>
        public static void TransitiveClosureUnitTest()
        {
            Digraph G = new Digraph(tinyDG);
            TransitiveClosure tc = new TransitiveClosure(G);

            // Print header.
            Console.Write("    ");
            for (int v = 0; v < G.V; v++)
                Console.Write("{0,3}", v);
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------");

            // Print transitive closure.
            for (int v = 0; v < G.V; v++)
            {
                Console.Write("{0,3}:", v);
                for (int w = 0; w < G.V; w++)
                {
                    if (tc.Reachable(v, w))
                        Console.Write("  T");
                    else
                        Console.Write("   ");
                }
                Console.WriteLine();
            }
        }
        /* Output:
                  0  1  2  3  4  5  6  7  8  9 10 11 12
            -------------------------------------------
              0:  T  T  T  T  T  T
              1:     T
              2:  T  T  T  T  T  T
              3:  T  T  T  T  T  T
              4:  T  T  T  T  T  T
              5:  T  T  T  T  T  T
              6:  T  T  T  T  T  T  T        T  T  T  T
              7:  T  T  T  T  T  T  T  T  T  T  T  T  T
              8:  T  T  T  T  T  T  T  T  T  T  T  T  T
              9:  T  T  T  T  T  T           T  T  T  T
             10:  T  T  T  T  T  T           T  T  T  T
             11:  T  T  T  T  T  T           T  T  T  T
             12:  T  T  T  T  T  T           T  T  T  T
            */

        /// <summary>
        /// Unit test method for TarjanSCC.
        /// </summary>
        public static void TarjanSCCUnitTest()
        {
            Digraph G = new Digraph(tinyDG);
            TarjanSCC scc = new TarjanSCC(G);
            SCCUnitTest(scc, G.V);
        }

        /// <summary>
        /// Unit test method for GabowSCC.
        /// </summary>
        public static void GabowSCCUnitTest()
        {
            Digraph G = new Digraph(tinyDG);
            GabowSCC scc = new GabowSCC(G);
            SCCUnitTest(scc, G.V);
        }

        /// <summary>
        /// Unit test method for SCC.
        /// </summary>
        public static void SCCUnitTest(StronglyConnectedComponentsBase scc, int V)
        {
            // Number of SCCs.
            int sccCount = scc.Count;
            Console.WriteLine(sccCount + " components.");

            // Compute list of vertices in each SCC.
            Queue<int>[] components = new Queue<int>[sccCount];
            for (int i = 0; i < sccCount; i++)
                components[i] = new Queue<int>();

            for (int v = 0; v < V; v++)
                components[scc.ID(v)].Enqueue(v);

            // Print results.
            for (int i = 0; i < sccCount; i++)
            {
                foreach (int v in components[i])
                    Console.Write(v + " ");
                Console.WriteLine();
            }
        }
        /* Output:
            5 components.
            1
            0 2 3 4 5
            9 10 11 12
            6
            7 8
            */

        /// <summary>
        /// Unit test method for EdgeWeightedGraph.
        /// </summary>
        public static void EdgeWeightedGraphUnitTest()
        {
            EdgeWeightedGraph G = new EdgeWeightedGraph(tinyEWG);
            Console.WriteLine(G);

            Console.WriteLine("Make a deep copy of original edge-weighted graph:");
            EdgeWeightedGraph G2 = new EdgeWeightedGraph(G);
            Console.WriteLine(G2);
        }
        /* Output:
            8 vertices  16 edges
            8: 6-0 0.58000 0-2 0.26000 0-4 0.38000 0-7 0.16000
            8: 1-3 0.29000 1-2 0.36000 1-7 0.19000 1-5 0.32000
            8: 6-2 0.40000 2-7 0.34000 1-2 0.36000 0-2 0.26000 2-3 0.17000
            8: 3-6 0.52000 1-3 0.29000 2-3 0.17000
            8: 6-4 0.93000 0-4 0.38000 4-7 0.37000 4-5 0.35000
            8: 1-5 0.32000 5-7 0.28000 4-5 0.35000
            8: 6-4 0.93000 6-0 0.58000 3-6 0.52000 6-2 0.40000
            8: 2-7 0.34000 1-7 0.19000 0-7 0.16000 5-7 0.28000 4-7 0.37000

            Make a deep copy of original edge-weighted graph:
            8 vertices  16 edges
            8: 6-0 0.58000 0-2 0.26000 0-4 0.38000 0-7 0.16000
            8: 1-3 0.29000 1-2 0.36000 1-7 0.19000 1-5 0.32000
            8: 6-2 0.40000 2-7 0.34000 1-2 0.36000 0-2 0.26000 2-3 0.17000
            8: 3-6 0.52000 1-3 0.29000 2-3 0.17000
            8: 6-4 0.93000 0-4 0.38000 4-7 0.37000 4-5 0.35000
            8: 1-5 0.32000 5-7 0.28000 4-5 0.35000
            8: 6-4 0.93000 6-0 0.58000 3-6 0.52000 6-2 0.40000
            8: 2-7 0.34000 1-7 0.19000 0-7 0.16000 5-7 0.28000 4-7 0.37000

            Press any key to continue...
            */

        /// <summary>
        /// Unit test methof for LazyMST.
        /// </summary>
        public static void LazyPrimMSTUnitTest()
        {
            EdgeWeightedGraph G = new EdgeWeightedGraph(tinyEWG);
            LazyPrimMST mst = new LazyPrimMST(G);

            MSTUnitTest(mst);
        }
        
        /// <summary>
        /// Unit test method for PrimMST.
        /// </summary>
        public static void PrimMSTUniteTest()
        {
            EdgeWeightedGraph G = new EdgeWeightedGraph(tinyEWG);
            PrimMST mst = new PrimMST(G);

            MSTUnitTest(mst);
        }

        /// <summary>
        /// Unit test methof for KruskalMST.
        /// </summary>
        public static void KruskalMSTUnitTest()
        {
            EdgeWeightedGraph G = new EdgeWeightedGraph(tinyEWG);
            KruskalMST mst = new KruskalMST(G);

            MSTUnitTest(mst);
        }

        public static void BorukaMSTUnitTest()
        {
            EdgeWeightedGraph G = new EdgeWeightedGraph(tinyEWG);
            BoruvkaMST mst = new BoruvkaMST(G);

            MSTUnitTest(mst);
        }

        /// <summary>
        /// Unit test method for MinSpanningTreeBase.
        /// </summary>
        /// <param name="mst"></param>
        private static void MSTUnitTest(MinSpanningTreeBase mst)
        {
            foreach (Edge e in mst.Edges())
                Console.WriteLine(e);
            Console.WriteLine("{0:F5}", mst.Weight);
        }
        /* Output:
            0-7 0.16000
            1-7 0.19000
            0-2 0.26000
            2-3 0.17000
            5-7 0.28000
            4-5 0.35000
            6-2 0.40000
            1.81000
            */

        /// <summary>
        /// Unit test method for EdgeWeightedDigraph.
        /// </summary>
        public static void EdgeWeightedDigraphUnitTest()
        {
            EdgeWeightedDigraph G = new EdgeWeightedDigraph(tinyEWD);
            Console.WriteLine(G);

            Console.WriteLine("Make a deep copy of original edge-weighted digraph:");
            EdgeWeightedDigraph G2 = new EdgeWeightedDigraph(G);
            Console.WriteLine(G2);
        }
        /* Output:
            8 vertices and 15 edges
            0: 0->2  0.26 0->4  0.38
            1: 1->3  0.29
            2: 2->7  0.34
            3: 3->6  0.52
            4: 4->7  0.37 4->5  0.35
            5: 5->1  0.32 5->7  0.28 5->4  0.35
            6: 6->4  0.93 6->0  0.58 6->2  0.40
            7: 7->3  0.39 7->5  0.28

            Make a deep copy of original edge-weighted digraph:
            8 vertices and 30 edges
            0: 0->2  0.26 0->4  0.38
            1: 1->3  0.29
            2: 2->7  0.34
            3: 3->6  0.52
            4: 4->7  0.37 4->5  0.35
            5: 5->1  0.32 5->7  0.28 5->4  0.35
            6: 6->4  0.93 6->0  0.58 6->2  0.40
            7: 7->3  0.39 7->5  0.28
            */

        /// <summary>
        /// Unit test method for DijkstraUndirectedShortestPaths.
        /// </summary>
        public static void DijkstraUndirectedShortestPathsUnitTest()
        {
            EdgeWeightedGraph G = new EdgeWeightedGraph(tinyEWG);
            int source = 6;

            // Compute shortest paths.
            DijkstraUndirectedShortestPaths sp = new DijkstraUndirectedShortestPaths(G, source);

            // Print shortest paths.
            for (int t = 0; t < G.V; t++)
            {
                if (sp.HasPathTo(t))
                {
                    Console.Write("{0} to {1} ({2:F2}) ", source, t, sp.DistanceTo(t));
                    foreach (Edge e in sp.PathTo(t))
                        Console.Write(e + "   ");
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("{0} to {1}         no path", source, t);
            }
        }
        /* Output:
            6 to 0 (0.58) 6-0 0.58000
            6 to 1 (0.76) 6-2 0.40000   1-2 0.36000
            6 to 2 (0.40) 6-2 0.40000
            6 to 3 (0.52) 3-6 0.52000
            6 to 4 (0.93) 6-4 0.93000
            6 to 5 (1.02) 6-2 0.40000   2-7 0.34000   5-7 0.28000
            6 to 6 (0.00)
            6 to 7 (0.74) 6-2 0.40000   2-7 0.34000
            */

        /// <summary>
        /// Unit test method for DijkstraShortestPaths.
        /// </summary>
        public static void DijkstraShortestPathsUnitTest()
        {
            EdgeWeightedDigraph G = new EdgeWeightedDigraph(tinyEWD);
            int source = 0;

            // Compute shortest paths.
            DijkstraShortestPaths sp = new DijkstraShortestPaths(G, source);

            // Print shortest paths.
            ShortestPathBaseUnitTest(sp, source, G.V);
        }
        /* Output:
            0 to 0 (0.00)
            0 to 1 (1.05)0->4  0.38   4->5  0.35   5->1  0.32
            0 to 2 (0.26)0->2  0.26
            0 to 3 (0.99)0->2  0.26   2->7  0.34   7->3  0.39
            0 to 4 (0.38)0->4  0.38
            0 to 5 (0.73)0->4  0.38   4->5  0.35
            0 to 6 (1.51)0->2  0.26   2->7  0.34   7->3  0.39   3->6  0.52
            0 to 7 (0.60)0->2  0.26   2->7  0.34
            */

        /// <summary>
        /// Unit test method for ShortestPathsBase, which print the shortest paths.
        /// </summary>
        /// <param name="sp">The instance of ShortestPathBase.</param>
        /// <param name="V">The number of vertices in the edge-weighted digraph.</param>
        private static void ShortestPathBaseUnitTest(ShortestPathBase sp,int source, int V)
        {
            for (int t = 0; t < V; t++)
            {
                if (sp.HasPathTo(t))
                {
                    Console.Write("{0} to {1} ({2:F2})", source, t, sp.DistanceTo(t));
                    foreach (DirectedEdge e in sp.PathTo(t))
                        Console.Write(e + "   ");
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("{0} to {1}         no path", source, t);
            }
        }

        /// <summary>
        /// Unit test method for DijkstraAllPairsSP.
        /// </summary>
        public static void DijkstraAllPairsSPUnitTest()
        {
            EdgeWeightedDigraph G = new EdgeWeightedDigraph(tinyEWD);
            DijkstraAllPairsSP sp = new DijkstraAllPairsSP(G);

            for (int v = 0; v < G.V; v++)
            {
                for (int t = 0; t < G.V; t++)
                {
                    if (sp.HasPath(v, t))
                    {
                        Console.Write("{0} to {1} ({2:F2})", v, t, sp.Distance(v, t));
                        foreach (DirectedEdge e in sp.Path(v, t))
                            Console.Write(e + "   ");
                        Console.WriteLine();
                    }
                    else
                        Console.WriteLine("{0} to {1}         no path", v, t);
                }
                Console.WriteLine();
            }
        }
        /* Output:
            0 to 0 (0.00)
            0 to 1 (1.05)0->4  0.38   4->5  0.35   5->1  0.32
            0 to 2 (0.26)0->2  0.26
            0 to 3 (0.99)0->2  0.26   2->7  0.34   7->3  0.39
            0 to 4 (0.38)0->4  0.38
            0 to 5 (0.73)0->4  0.38   4->5  0.35
            0 to 6 (1.51)0->2  0.26   2->7  0.34   7->3  0.39   3->6  0.52
            0 to 7 (0.60)0->2  0.26   2->7  0.34

            1 to 0 (1.39)1->3  0.29   3->6  0.52   6->0  0.58
            1 to 1 (0.00)
            1 to 2 (1.21)1->3  0.29   3->6  0.52   6->2  0.40
            1 to 3 (0.29)1->3  0.29
            1 to 4 (1.74)1->3  0.29   3->6  0.52   6->4  0.93
            1 to 5 (1.83)1->3  0.29   3->6  0.52   6->2  0.40   2->7  0.34   7->5  0.28
            1 to 6 (0.81)1->3  0.29   3->6  0.52
            1 to 7 (1.55)1->3  0.29   3->6  0.52   6->2  0.40   2->7  0.34

            2 to 0 (1.83)2->7  0.34   7->3  0.39   3->6  0.52   6->0  0.58
            2 to 1 (0.94)2->7  0.34   7->5  0.28   5->1  0.32
            2 to 2 (0.00)
            2 to 3 (0.73)2->7  0.34   7->3  0.39
            2 to 4 (0.97)2->7  0.34   7->5  0.28   5->4  0.35
            2 to 5 (0.62)2->7  0.34   7->5  0.28
            2 to 6 (1.25)2->7  0.34   7->3  0.39   3->6  0.52
            2 to 7 (0.34)2->7  0.34

            3 to 0 (1.10)3->6  0.52   6->0  0.58
            3 to 1 (1.86)3->6  0.52   6->2  0.40   2->7  0.34   7->5  0.28   5->1  0.32
            3 to 2 (0.92)3->6  0.52   6->2  0.40
            3 to 3 (0.00)
            3 to 4 (1.45)3->6  0.52   6->4  0.93
            3 to 5 (1.54)3->6  0.52   6->2  0.40   2->7  0.34   7->5  0.28
            3 to 6 (0.52)3->6  0.52
            3 to 7 (1.26)3->6  0.52   6->2  0.40   2->7  0.34

            4 to 0 (1.86)4->7  0.37   7->3  0.39   3->6  0.52   6->0  0.58
            4 to 1 (0.67)4->5  0.35   5->1  0.32
            4 to 2 (1.68)4->7  0.37   7->3  0.39   3->6  0.52   6->2  0.40
            4 to 3 (0.76)4->7  0.37   7->3  0.39
            4 to 4 (0.00)
            4 to 5 (0.35)4->5  0.35
            4 to 6 (1.28)4->7  0.37   7->3  0.39   3->6  0.52
            4 to 7 (0.37)4->7  0.37

            5 to 0 (1.71)5->1  0.32   1->3  0.29   3->6  0.52   6->0  0.58
            5 to 1 (0.32)5->1  0.32
            5 to 2 (1.53)5->1  0.32   1->3  0.29   3->6  0.52   6->2  0.40
            5 to 3 (0.61)5->1  0.32   1->3  0.29
            5 to 4 (0.35)5->4  0.35
            5 to 5 (0.00)
            5 to 6 (1.13)5->1  0.32   1->3  0.29   3->6  0.52
            5 to 7 (0.28)5->7  0.28

            6 to 0 (0.58)6->0  0.58
            6 to 1 (1.34)6->2  0.40   2->7  0.34   7->5  0.28   5->1  0.32
            6 to 2 (0.40)6->2  0.40
            6 to 3 (1.13)6->2  0.40   2->7  0.34   7->3  0.39
            6 to 4 (0.93)6->4  0.93
            6 to 5 (1.02)6->2  0.40   2->7  0.34   7->5  0.28
            6 to 6 (0.00)
            6 to 7 (0.74)6->2  0.40   2->7  0.34

            7 to 0 (1.49)7->3  0.39   3->6  0.52   6->0  0.58
            7 to 1 (0.60)7->5  0.28   5->1  0.32
            7 to 2 (1.31)7->3  0.39   3->6  0.52   6->2  0.40
            7 to 3 (0.39)7->3  0.39
            7 to 4 (0.63)7->5  0.28   5->4  0.35
            7 to 5 (0.28)7->5  0.28
            7 to 6 (0.91)7->3  0.39   3->6  0.52
            7 to 7 (0.00)
            */

        /// <summary>
        /// Unit test method for EdgeWeightedDirectedCycle.
        /// </summary>
        public static void EdgeWeightedDirectedCycleUnitTest()
        {
            // Test for edge-weighted DAG.
            EdgeWeightedDigraph G = new EdgeWeightedDigraph(tinyEWDAG);
            EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(G);
            EdgeWeightedDirectedCycleHepler(finder);

            // Test for tinyEWD which has a directed cycle.
            G = new EdgeWeightedDigraph(tinyEWD);
            finder = new EdgeWeightedDirectedCycle(G);
            EdgeWeightedDirectedCycleHepler(finder);
        }
        /* Output:
            No directed cycle.
            Cycle: 7->3  0.39 3->6  0.52 6->4  0.93 4->7  0.37
            */

        /// <summary>
        /// A helper method for EdgeWeightedDirectedCycleUnitTest().
        /// </summary>
        /// <param name="finder">An instance of EdgeWeightedDirectedCycle.</param>
        private static void EdgeWeightedDirectedCycleHepler(EdgeWeightedDirectedCycle finder)
        {
            // Find a directed cycle.
            if (finder.HasCycle)
            {
                Console.Write("Cycle: ");
                foreach (DirectedEdge e in finder.GetCycle())
                    Console.Write(e + " ");
                Console.WriteLine();
            }

            // Or give topological order.
            else
                Console.WriteLine("No directed cycle.");
        }

        /// <summary>
        /// Unit test method for AcyclicShortestPaths.
        /// </summary>
        public static void AcyclicShortestPathsUnitTest()
        {
            EdgeWeightedDigraph G = new EdgeWeightedDigraph(tinyEWDAG);
            int source = 5;

            AcyclicShortestPaths sp = new AcyclicShortestPaths(G, source);
            ShortestPathBaseUnitTest(sp, source, G.V);
        }
        /* Output:
            5 to 0 (0.73)5->4  0.35   4->0  0.38
            5 to 1 (0.32)5->1  0.32
            5 to 2 (0.62)5->7  0.28   7->2  0.34
            5 to 3 (0.61)5->1  0.32   1->3  0.29
            5 to 4 (0.35)5->4  0.35
            5 to 5 (0.00)
            5 to 6 (1.13)5->1  0.32   1->3  0.29   3->6  0.52
            5 to 7 (0.28)5->7  0.28
            */

        /// <summary>
        /// Unit test method for AcyclicLongestPaths.
        /// </summary>
        public static void AcyclicLongestPathsUnitTest()
        {
            EdgeWeightedDigraph G = new EdgeWeightedDigraph(tinyEWDAG);
            int source = 5;

            AcyclicLongestPaths lp = new AcyclicLongestPaths(G, source);

            for (int t = 0; t < G.V; t++)
            {
                if (lp.HasPathTo(t))
                {
                    Console.Write("{0} to {1} ({2:F2}) ", source, t, lp.DistanceTo(t));
                    foreach (DirectedEdge e in lp.PathTo(t))
                        Console.Write(e + "   ");
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("{0} to {1}         no path", source, t);
            }
        }
        /* Output:
            5 to 0 (2.44) 5->1  0.32   1->3  0.29   3->6  0.52   6->4  0.93   4->0  0.38
            5 to 1 (0.32) 5->1  0.32
            5 to 2 (2.77) 5->1  0.32   1->3  0.29   3->6  0.52   6->4  0.93   4->7  0.37   7->2  0.34
            5 to 3 (0.61) 5->1  0.32   1->3  0.29
            5 to 4 (2.06) 5->1  0.32   1->3  0.29   3->6  0.52   6->4  0.93
            5 to 5 (0.00)
            5 to 6 (1.13) 5->1  0.32   1->3  0.29   3->6  0.52
            5 to 7 (2.43) 5->1  0.32   1->3  0.29   3->6  0.52   6->4  0.93   4->7  0.37
            */

        /// <summary>
        /// Unit test method for CiriticalPathMethod.
        /// </summary>
        public static void CriticalPathMethodUnitTest()
        {
            CriticalPathMethod.ComputeCPM(jobsPC);
        }
        /* Output:
              job  start  finish
            ---------------------
               0     0.0    41.0
               1    41.0    92.0
               2   123.0   173.0
               3    91.0   127.0
               4    70.0   108.0
               5     0.0    45.0
               6    70.0    91.0
               7    41.0    73.0
               8    91.0   123.0
               9    41.0    70.0
            Finish time:   173.0
            */

        /// <summary>
        /// Unit test method for BellmanFordShortestPaths.
        /// </summary>
        public static void BellmanFordShortestPathsUntiTest()
        {
            EdgeWeightedDigraph G = new EdgeWeightedDigraph(tinyEWDn);
            int source = 0;
            BellmanFordShortestPaths sp = new BellmanFordShortestPaths(G, source);

            // Print negative cycle.
            if (sp.HasNegativeCycle)
            {
                foreach (DirectedEdge e in sp.GetNegativeCycle())
                    Console.Write(e + " ");
            }

            // Print shortest paths.
            else
            {
                for (int v = 0; v < G.V; v++)
                {
                    if (sp.HasPathTo(v))
                    {
                        Console.Write("{0} to {1} ({2,5:F2}) ", source, v, sp.DistanceTo(v));
                        foreach (DirectedEdge e in sp.PathTo(v))
                            Console.Write(e + " ");
                        Console.WriteLine();
                    }
                    else
                        Console.WriteLine("{0} to {1}           no path", source, v);
                }
            }
        }
        /* Output:
            0 to 0 ( 0.00)
            0 to 1 ( 0.93) 0->2  0.26 2->7  0.34 7->3  0.39 3->6  0.52 6->4 -1.25 4->5  0.35 5->1  0.32
            0 to 2 ( 0.26) 0->2  0.26
            0 to 3 ( 0.99) 0->2  0.26 2->7  0.34 7->3  0.39
            0 to 4 ( 0.26) 0->2  0.26 2->7  0.34 7->3  0.39 3->6  0.52 6->4 -1.25
            0 to 5 ( 0.61) 0->2  0.26 2->7  0.34 7->3  0.39 3->6  0.52 6->4 -1.25 4->5  0.35
            0 to 6 ( 1.51) 0->2  0.26 2->7  0.34 7->3  0.39 3->6  0.52
            0 to 7 ( 0.60) 0->2  0.26 2->7  0.34
            */

        /// <summary>
        /// Unit test method for Arbitrage.
        /// </summary>
        public static void ArbitrageUnitTest()
        {
            Arbitrage.ComputeArbitrage(rates);
        }
        /* Output:
            1000.00000 USD =  741.00000 EUR
             741.00000 EUR = 1012.20600 CAD
            1012.20600 CAD = 1007.14497 USD
            */
    }
}