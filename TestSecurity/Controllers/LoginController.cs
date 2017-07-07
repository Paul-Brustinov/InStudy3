using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestSecurity.Models;

namespace TestSecurity.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(HUser user, string ReturnUrl)
        {
            if (isValid(user))
            {
                FormsAuthentication.SetAuthCookie(user.Name, false);
                return Redirect(ReturnUrl);
            }
            else
            {
                return View(user);
            }
            
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Home/Index");
        }

        private bool isValid(HUser user)
        {
            // Do DB work here
            return (user.Name == "test" && user.Password == "test");
        }
    }
}