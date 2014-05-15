using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Computer.Enumerate.DL
{
    public class HostResult
    {
        public string IpAddress { get; set; }
        public string MACAddress { get; set; }
        public bool Online { get; set; }
        public long ReplyRoundtripTime { get; set; }

        public override string ToString()
        {
            return string.Format("{0}  {1}",IpAddress,MACAddress);
        }
    }
}
