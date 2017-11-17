using System;
using System.Collections.Generic;
using System.Text;

namespace CoreNetworkConsole.DistributedSpanningTrees
{
    public class BridgeSlim
    {
        public int Id { get; private set; }

        public bool CanSend { get; set; }

        public BridgeSlim(int id)
        {
            Id = id;
            CanSend = true;
        }
    }
}
