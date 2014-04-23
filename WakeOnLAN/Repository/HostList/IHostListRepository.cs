using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Computer.Enumerate.DL;

namespace WakeOnLAN.Repository.HostList
{
    public interface IHostListRepository
    {
        List<KeyValuePair<string, List<IpAddrMACPair>>> GetHostList();
        List<KeyValuePair<string, List<IpAddrMACPair>>> GetHostListPaged(int pageNo, int pagesize, ref int total);
    }
}
