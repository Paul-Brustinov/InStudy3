using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestSecurity.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            ViewData["uname"] = User.Identity.Name;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}