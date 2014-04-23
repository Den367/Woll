using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Computer.Enumerate.DL;

namespace WakeOnLAN.Repository.Wol
{
   public  interface IWolRepository
    {
        List<KeyValuePair<string, List<IpAddrMACPair>>> GetAllHostList();
       List<KeyValuePair<string, List<IpAddrMACPair>>> GetHostListPaged(string hostName, int pageNo, int pagesize, out int total);
    }
}
