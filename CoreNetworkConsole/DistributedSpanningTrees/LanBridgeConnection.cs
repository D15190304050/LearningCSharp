using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class LanBridgeConnection
    {
        public Lan Lan { get; set; }
        public Bridge2 Bridge { get; set; }

        public ConcurrentQueue<ConfigurationMessage> MessageQueueToLan { get; set; }

        public ConcurrentQueue<ConfigurationMessage> MessageQueueToBridge { get; set; }

        public LanBridgeConnection(Lan lan, Bridge2 bridge)
        {
            this.Lan = lan;
            this.Bridge = bridge;

            this.MessageQueueToBridge = new ConcurrentQueue<ConfigurationMessage>();
            this.MessageQueueToLan = new ConcurrentQueue<ConfigurationMessage>();
        }
    }
}
