/*!
 *\file User Conroller file
 *
 *  Control User processing
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.Pkcs;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InStudy.Models;

namespace InStudy.Controllers
{

    /// <summary>
    ///     User Controller for support users operations
    /// </summary>
    public class UserController : Controller
    {

        /*!
         * Registration GET action
         */
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        /*!
         * Registration POST action
         */
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

        /*!
         * Vetify Account 
         */
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

        /*!
         * Process sended login data
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string returnUrl="")
        {
            using (var dc = new MyDatabaseEntities())
            {
                var user = dc.Users.FirstOrDefault(u => u.EmailID == login.EmailID);
                if (user != null)
                {
                    if (string.CompareOrdinal(Crypto.Hash(login.Password), user.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 == year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encryped = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryped)
                        {
                            Expires = DateTime.Now.AddMinutes(timeout),
                            HttpOnly = true
                        };
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                        return RedirectToAction("Index", "Home");
                     }
                }

            }
            ViewBag.Message = "Invalid credential provided";
            return View();
        }
        
        /*!
         *  Processing Logout
         * \return redurect to Action Login
         */
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User"); 
        }


        /*!
            *Method check if email exists in database
            * \params[in] EmailID - email
            * \returns true if email exists or false otherwise
        */

        [NonAction]
        public bool IsEmailExists(String EmailID)
        {
            using (var dc = new MyDatabaseEntities())
            {
               return dc.Users.FirstOrDefault(u => u.EmailID == EmailID) != null;
            }
        }


        //TODO: Move SendVerificationLinkEmail to Model
        //TODO: Move email account data to a separate settings file
        /*!
         * Method send email with vetification code to activate User account
         * \param[in] emailID - users email
         * \param[in] activationCode - verification code
         * \return void
         */
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