using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class LanMultiThread
    {
        private class Signal
        {
            public bool IsConverged { get; set; }
            public Signal()
            {
                IsConverged = false;
            }
        }

        private class BridgeUpdater
        {
            public Bridge Bridge { get; set; }

            private Signal signal;

            private ManualResetEvent done;

            public BridgeUpdater(Bridge bridge, ManualResetEvent done, ref Signal signal)
            {
                this.Bridge = bridge;
                this.signal = signal;
                this.done = done;
            }

            public void Update()
            {
                while (!signal.IsConverged)
                {
                    Bridge.Update();
                    Thread.Sleep(5);
                }

                done.Set();
            }
        }

        private class LanWorker
        {
            public Lan Lan { get; set; }

            private Signal signal;
            private ManualResetEvent done;

            public LanWorker(Lan lan, ManualResetEvent done, ref Signal signal)
            {
                this.Lan = lan;
                this.done = done;
                this.signal = signal;
            }

            public void Transmit()
            {
                while (!signal.IsConverged)
                {
                    Lan.Transmit();
                    Thread.Sleep(5);
                }

                done.Set();
            }
        }

        private Signal signal;

        public int BridgeCount { get; private set; }
        public int LanCount { get; private set; }

        private Bridge[] bridges;

        private Lan[] lans;

        private int[] rootId;

        public LanMultiThread(int bridgeCount, int lanCount)
        {
            Initialize(bridgeCount, lanCount);
        }

        public LanMultiThread(string filePath)
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
            signal = new Signal();
            signal.IsConverged = false;

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

            ManualResetEvent[] waitSignals = new ManualResetEvent[this.BridgeCount + this.LanCount];
            for (int i = 0; i < waitSignals.Length; i++)
                waitSignals[i] = new ManualResetEvent(false);

            for (int i = 0; i < bridges.Length; i++)
            {
                BridgeUpdater updater = new BridgeUpdater(bridges[i], waitSignals[i], ref signal);
                Thread t = new Thread(updater.Update);
                t.Start();
            }

            for (int i = 0; i < lans.Length; i++)
            {
                LanWorker worker = new LanWorker(lans[i], waitSignals[i + this.BridgeCount], ref signal);
                Thread t = new Thread(worker.Transmit);
                t.Start();
            }

            Thread monitor = new Thread(MonitorConvergence);
            monitor.Start();

            WaitHandle.WaitAll(waitSignals);
        }

        public void MonitorConvergence()
        {
            while (!IsConverged())
                Thread.Sleep(5);
            signal.IsConverged = true;
        }

        private void ClearST()
        {
            signal.IsConverged = false;

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