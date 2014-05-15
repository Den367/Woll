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

            return View(getPagedHosts(name,  count, page ));
        }

        
        public ActionResult Hosts(string name, int? count, int page = 0)
        {
            return View(getPagedHosts(name, count, page));
        }

        public ActionResult SendMagicPacket(string macAddress)
        {
            new MagicPacketSender().SendMagicPacket(macAddress);
            ViewBag.Message = string.Format("Сообщение отправлено на MAC адрес {0}",macAddress);
            return RedirectToAction("Index");
        }

        private FilteredHostListViewModel getPagedHosts(string name, int? count, int page = 0)
        {
            IWolRepository repo = new WolRepository();
            int total;
            var pageModel = new PagedList<KeyValuePair<string, List<HostResult>>>(repo.GetHostListPaged(name, page, count ?? 10, out total), page, count ?? 10, total);
            return new FilteredHostListViewModel() { HostFilter = name, Hosts = pageModel };
        }

        //public ActionResult DiscoveredHostList(int? count, int page = 1)
        //{

        //    //var hosts = GetHostList(page, count ?? 25);
        //    //var result = new PagedList<KeyValuePair<string, List<IpAddrMACPair>>>(hosts, page, count ?? 10);


        //    //return View(result);
        //}


    }
}
