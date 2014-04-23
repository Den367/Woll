using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Network.Computer.Enumerate;
using WakeOnLAN.Repository;
using WakeOnLAN.Repository.Save;


namespace WakeOnLAN.Controllers
{
    public class SaveController : Controller
    {
        //
        // GET: /Save/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Actualize()
        {
            var repo = new SaveRepository();

            var model = repo.ActualizeHostList();

            return View("Index", model);
        }
    }
}
