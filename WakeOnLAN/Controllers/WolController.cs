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

        public ActionResult Index(string name,int? count, int page = 0   )
        {
            IWolRepository repo = new WolRepository();
            int total ;
            var pageModel = new PagedList<KeyValuePair<string, List<IpAddrMACPair>>>(repo.GetHostListPaged(name, page, count ?? 25, out total), page , count ?? 25, total);
            var result = new FilteredHostListViewModel() {HostFilter = name, Hosts = pageModel};

            return View(result);
        }

        public ActionResult Hosts(string name, int? count, int page = 0)
        {
            IWolRepository repo = new WolRepository();
            int total;
            var result = new PagedList<KeyValuePair<string, List<IpAddrMACPair>>>(repo.GetHostListPaged(name, page, count ?? 25, out total), page , count ?? 25, total);


            return View(result);
        }

        public ActionResult SendMagicPacket(string macAddress)
        {
            new MagicPacketSender().SendMagicPacket(macAddress);
            ViewBag.Message = string.Format("Сообщение отправлено на MAC адрес {0}",macAddress);
            return View("Index");
        }

        //public ActionResult DiscoveredHostList(int? count, int page = 1)
        //{

        //    //var hosts = GetHostList(page, count ?? 25);
        //    //var result = new PagedList<KeyValuePair<string, List<IpAddrMACPair>>>(hosts, page, count ?? 10);


        //    //return View(result);
        //}


    }
}
