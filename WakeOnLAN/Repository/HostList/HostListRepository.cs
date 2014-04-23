using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Network.Computer.Enumerate;
using Network.Computer.Enumerate.DL;
using WakeOnLAN.Repository.HostList;

namespace WakeOnLAN.Repository
{
    public class HostListRepository: IHostListRepository
    {

        public List<KeyValuePair<string, List<IpAddrMACPair>>> GetHostList()
        {
            NetworkBrowser browser = new NetworkBrowser();
            return browser.GetAvailableHostList();

        }

        public List<KeyValuePair<string, List<IpAddrMACPair>>> GetHostListPaged(int pageNo, int pagesize, ref int total)
        {
            NetworkBrowser browser = new NetworkBrowser();
            var hosts = browser.GetAvailableHostList();
            total = hosts.Count;
            if (pageNo <= 0) pageNo = 1;
            int start = (pageNo - 1) * pagesize;
            int end = start + pagesize;
            List<KeyValuePair<string, List<IpAddrMACPair>>> result = new List<KeyValuePair<string, List<IpAddrMACPair>>>();

            for (int i = start; i <= end; i++) if (i < total) result.Add(hosts[i]);
            
            return result;

        }

    }
}