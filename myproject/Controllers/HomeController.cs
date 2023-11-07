using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace myproject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Thankyou For showing interest in us";

            return View();
        }

        public ActionResult Signin()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Signup()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}

