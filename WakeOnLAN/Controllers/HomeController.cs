using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WakeOnLAN.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Здесь будет описание";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Написать администратору";

            return View();
        }
    }
}
