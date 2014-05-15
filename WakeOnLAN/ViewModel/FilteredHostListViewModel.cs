using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Paging;

namespace WakeOnLAN.ViewModel
{
    public class FilteredHostListViewModel
    {
        public PagedList<KeyValuePair<string, List<Network.Computer.Enumerate.DL.HostResult>>> Hosts { get; set; }
        public string HostFilter { get; set; }


    }
}