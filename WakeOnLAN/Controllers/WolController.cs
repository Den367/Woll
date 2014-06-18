using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Network.Computer.Enumerate.DL;
using Paging;
using WakeOnLAN.NetworkStuff;
using WakeOnLAN.Repository.Wol;
using WakeOnLAN.ViewModel;

namespace WakeOnLAN.Controllers
{
    public class WolController : Controller
    {
        //
        // GET: /Wol/

        public ActionResult Index(string name,int? count, int page = 0  )
        {
            ViewBag.Title = "Включение по сети";
            return View(getPagedHosts(name,  count, page ));
        }

        
        public ActionResult Hosts(string name, int? count, int page = 0)
        {
            return View(getPagedHosts(name, count, page));
        }

        public ActionResult SendMagicPacket(string macAddress)
        {
            new MagicPacketSender().SendMagicPacket(macAddress);

            return Sent(string.Format("Сообщение отправлено на MAC адрес {0}", macAddress));
        }

        public ActionResult SendMagicPacketByHostName(string hostName)
        {
            var macAddress = getMacAddress(hostName);
            new MagicPacketSender().SendMagicPacket(macAddress);

            return Sent(string.Format("Сообщение отправлено на MAC адрес {0}", macAddress));
        }

        public ActionResult Sent(string message)
        {
            ViewBag.Title = "Включение по сети";
            ViewBag.Message = message;
            return View("Index", getPagedHosts(string.Empty, null, 0));
        }

        private FilteredHostListViewModel getPagedHosts(string name, int? count, int page = 0)
        {
            IWolRepository repo = new WolRepository();
            int total;
            var pageModel = new PagedList<KeyValuePair<string, List<HostResult>>>(repo.GetHostListPaged(name, page, count ?? 10, out total), page, count ?? 10, total);
            return new FilteredHostListViewModel() { HostFilter = name, Hosts = pageModel };
        }

        private string getMacAddress(string name)
        {
            IWolRepository repo = new WolRepository();

            return repo.GetMacAddrByHostName(name);
        }

        //public ActionResult DiscoveredHostList(int? count, int page = 1)
        //{

        //    //var hosts = GetHostList(page, count ?? 25);
        //    //var result = new PagedList<KeyValuePair<string, List<IpAddrMACPair>>>(hosts, page, count ?? 10);


        //    //return View(result);
        //}


    }
}
