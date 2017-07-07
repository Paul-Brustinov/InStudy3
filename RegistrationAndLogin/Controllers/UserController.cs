using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.Pkcs;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RegistrationAndLogin.Models;

namespace RegistrationAndLogin.Controllers
{
    public class UserController : Controller
    {

        //Registration Action

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVeryfied,ActivationCode")] User user)
        {
            bool Status = false;
            string Message = "";

            //Model validation
            if (!ModelState.IsValid)
            {
                ViewBag.Message = Message;
                ViewBag.Status = Status;
                return View(user);
            }

            #region Email is already exists

                var isExists = IsEmailExists(user.EmailID);
                if (IsEmailExists(user.EmailID))
                {
                    ModelState.AddModelError("EmailExists", "Email already exists!");
                    return View(user);
                }
            #endregion


            #region Save data to database   
                user.ActivationCode = Guid.NewGuid(); //Generate activation code

                user.Password = Crypto.Hash(user.Password); //Password hashing
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);

                user.IsEmailVerified = false;


                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();
                }

                //Send Email to User
                SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                Message = "Registration successfully done. Account activation link has been send to you email id :" + user.EmailID;
                Status = true;
            #endregion

            ViewBag.Message = Message;
            ViewBag.Status = Status;
  
            return View(user);
        }

        //Vetify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool status = false;

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; //to avoid password does not match issue on save changes

                var user = dc.Users.FirstOrDefault(u => u.ActivationCode == new Guid(id));
                if (user != null)
                {
                    user.IsEmailVerified = true;
                    dc.SaveChanges();
                    status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }

            ViewBag.Status = status;

            return View();
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string returnUrl="")
        {
            string message = "";
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var user = dc.Users.FirstOrDefault(u => u.EmailID == login.EmailID);
                if (user != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), user.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 == year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encryped = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryped);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                };
            }
            ViewBag.Message = message;

            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User"); 
        }



        [NonAction]
        public bool IsEmailExists(String EmailID)
        {
            using (var dc = new MyDatabaseEntities())
            {
               return dc.Users.FirstOrDefault(u => u.EmailID == EmailID) != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("ab.lern4@gmail.com", "Paul");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "G@n1u$2017";
            string subject = "Your account is successfully created!";

            string body = "<br/> <br/> we are excited to tell you that your account is succsesfully created. Please click on the link to verify your account " +
                "<a href = "+ link + ">" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            }) smtp.Send(message);

        }

    }

}