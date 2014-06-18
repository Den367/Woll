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
        List<KeyValuePair<string, List<HostResult>>> GetAllHostList();
       List<KeyValuePair<string, List<HostResult>>> GetHostListPaged(string hostName, int pageNo, int pagesize, out int total);
       /// <summary>
       /// Retrieve MAC  address from DB
       /// </summary>
       /// <param name="hostName"></param>
       /// <returns></returns>
       string GetMacAddrByHostName(string hostName);
    }
}
