using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Computer.Enumerate.DL
{
    public class IpAddrMACPair
    {
        public string IpAddress { get; set; }
        public string MACAddress { get; set; }
        public override string ToString()
        {
            return string.Format("{0}  {1}",IpAddress,MACAddress);
        }
    }
}
