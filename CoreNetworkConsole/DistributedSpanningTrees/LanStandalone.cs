using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class LanStandalone
    {
        public int BridgeCount { get; private set; }
        public int LanCount { get; private set; }

        private Bridge[] bridges;

        private Lan[] lans;

        private int[] rootId;

        public LanStandalone(int bridgeCount, int lanCount)
        {
            Initialize(bridgeCount, lanCount);
        }

        public LanStandalone(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("No such file.");
            string[] lines = File.ReadAllLines(filePath);

            int bridgeCount = int.Parse(lines[0]);
            int lanCount = int.Parse(lines[1]);

            Initialize(bridgeCount, lanCount);

            for (int nextLineIndex = 2; nextLineIndex < lines.Length; nextLineIndex++)
            {
                string[] line = lines[nextLineIndex].Split(' ');
                int bridgeId = int.Parse(line[0]);
                int lanId = int.Parse(line[1]);
                AddConnection(bridgeId, lanId);
            }
        }

        private void Initialize(int bridgeCount, int lanCount)
        {
            this.BridgeCount = bridgeCount;
            this.LanCount = lanCount;

            bridges = new Bridge[bridgeCount];
            for (int i = 0; i < bridgeCount; i++)
                bridges[i] = new Bridge(i);

            lans = new Lan[lanCount];
            for (int i = 0; i < lanCount; i++)
                lans[i] = new Lan(i);

            rootId = new int[bridgeCount];
            for (int i = 0; i < bridgeCount; i++)
                rootId[i] = i;
        }

        private void AddConnection(int bridgeId, int lanId)
        {
            // Make some validation here.

            bridges[bridgeId].AddConnection(lans[lanId]);
            lans[lanId].AddConnection(bridges[bridgeId]);
        }

        public void RunDST()
        {
            ClearST();

            for (; ; )
            {
                for (int i = 0; i < bridges.Length; i++)
                    bridges[i].Update();
                for (int i = 0; i < lans.Length; i++)
                    lans[i].Transmit();
                if (IsConverged())
                    break;
            }
        }

        private void ClearST()
        {
            foreach (Bridge bridge in bridges)
                bridge.Clear();
            for (int i = 0; i < bridges.Length; i++)
                rootId[i] = i;
        }

        public string GetST()
        {
            StringBuilder result = new StringBuilder();

            // Get the information of designate bridge for each LAN.
            foreach (Lan lan in lans)
                result.Append(lan.ToString() + Environment.NewLine);

            // Get the
            foreach (Bridge bridge in bridges)
                result.Append(bridge.ToString() + Environment.NewLine);

            return result.ToString();
        }

        public override string ToString()
        {
            StringBuilder connectionInfo = new StringBuilder();
            foreach (Bridge bridge in bridges)
                connectionInfo.Append(bridge.GetConnectionInfo());
            return connectionInfo.ToString();
        }

        private void UpdateRootIdInfo()
        {
            for (int i = 0; i < this.BridgeCount; i++)
                rootId[i] = bridges[i].RootId;
        }

        private bool IsConverged()
        {
            UpdateRootIdInfo();

            int potentialRootId = rootId[0];
            for (int i = 1; i < this.BridgeCount; i++)
            {
                if (rootId[i] != potentialRootId)
                    return false;
            }
            return true;
        }
    }
}
