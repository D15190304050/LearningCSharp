using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.IO;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class AsStandalone
    {
        public int V { get; private set; }
        public int E { get; private set; }

        private Bridge[] bridges;

        private int[] rootId;

        private ConcurrentQueue<BridgeInfo>[] messageQueues;

        public AsStandalone(int V)
        {
            Initialize(V);
        }

        public AsStandalone(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
                throw new ArgumentException("No such file.");

            string[] lines = File.ReadAllLines(filePath);
            this.V = int.Parse(lines[0]);
            int E = int.Parse(lines[1]);

            Initialize(this.V);

            for (int i = 0; i < E; i++)
            {
                string[] line = lines[i + 2].Split(' ');
                int v = int.Parse(line[0]);
                int w = int.Parse(line[1]);

                AddEdge(v, w);
            }
        }

        private void Initialize(int V)
        {
            this.V = V;
            this.E = 0;

            bridges = new Bridge[V];
            for (int v = 0; v < V; v++)
                bridges[v] = new Bridge(v);

            rootId = new int[V];
            for (int v = 0; v < V; v++)
                rootId[v] = v;

            messageQueues = new ConcurrentQueue<BridgeInfo>[V];
            for (int v = 0; v < V; v++)
                messageQueues[v] = new ConcurrentQueue<BridgeInfo>();
        }

        public void AddEdge(int v, int w)
        {
            ValidateVertex(v);
            ValidateVertex(w);

            bridges[v].AddAdjacent(w);
            bridges[w].AddAdjacent(v);
            this.E++;
        }

        public IEnumerable<int> Adjacent(int v)
        {
            ValidateVertex(v);
            foreach (BridgeSlim bridge in bridges[v].Adjacent)
                yield return bridge.Id;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder($"{V} bridges and {E} edges." + Environment.NewLine);

            for (int v = 0; v < this.V; v++)
            {
                s.Append(v + $" with root id = {rootId[v]} : ");
                foreach (int w in Adjacent(v))
                    s.Append(w + " ");
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public void ComputeSpanningTree()
        {
            while (!IsConverged())
            {
                foreach (Bridge bridge in bridges)
                    bridge.Update(messageQueues);

                for (int v = 0; v < V; v++)
                    rootId[v] = bridges[v].SelfInfo.RootId;
            }
        }

        private bool IsConverged()
        {
            int potentialRoot = rootId[0];
            for (int v = 1; v < V; v++)
            {
                if (potentialRoot != rootId[v])
                    return false;
            }
            return true;
        }

        public string GetSpanningTree()
        {
            StringBuilder s = new StringBuilder($"{V} bridges and {E} edges." + Environment.NewLine);

            for (int v = 0; v < this.V; v++)
            {
                s.Append(v + $" with root id = {rootId[v]} and designate bridge = {bridges[v].DesignateBridge} : ");
                foreach (int w in Adjacent(v))
                    s.Append(w + " ");
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        private void ValidateVertex(int v)
        {
            if ((v < 0) || (v >= V))
                throw new ArgumentException($"The name of the vertex must between 0 and {V - 1}.");
        }
    }
}
