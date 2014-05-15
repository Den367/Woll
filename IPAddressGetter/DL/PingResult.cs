using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Network.Computer.Enumerate.DL
{
    public  class PingResult
    {
        public IPStatus Status { get; set; }
        public long ReplyRoundtripTime { get; set; }

    }
}
