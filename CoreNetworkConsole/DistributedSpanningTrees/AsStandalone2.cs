using System;
using System.Collections.Generic;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class AsStandalone2
    {
        private Lan[] lans;

        private Bridge2[] bridges;

        private LinkedList<LanBridgeConnection> connections;

        public AsStandalone2(int numLans, int numBridges)
        {
            lans = new Lan[numLans];
            bridges = new Bridge2[numBridges];

            for (int i = 0; i < numLans; i++)
                lans[i] = new Lan(i + 1);

            for (int i = 0; i < numBridges; i++)
                bridges[i] = new Bridge2(i + 1);

            connections = new LinkedList<LanBridgeConnection>();
        }


    }
}
