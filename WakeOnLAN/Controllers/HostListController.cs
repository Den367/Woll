using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Network.Computer.Enumerate;
using Network.Computer.Enumerate.DL;
using Paging;
using WakeOnLAN.Repository;
using WakeOnLAN.Repository.HostList;

namespace WakeOnLAN.Controllers
{
    public class HostListController : Controller
    {
        //
        // GET: /HostList/

        public ActionResult Index(int? count, int page = 0)
        {
            IHostListRepository repo = new HostListRepository();
            int total = 0;
            var result = new PagedList<KeyValuePair<string, List<HostResult>>>(repo.GetHostListPaged(page, count ?? 25, ref total ), page, count ?? 25,total);
           
            ViewBag.Title = "Доступные хосты";

            return View(result);
        }


      

       

    }
}
