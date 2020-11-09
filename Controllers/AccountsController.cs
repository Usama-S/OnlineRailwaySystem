using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineRailway.Controllers
{
    public class AccountsController : Controller
    {
        string prevRef;
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult login()
        {
            //prevRef = Request.UrlReferrer.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult login(int userID, string password, string controllername)
        {
            string encryptedPassword = MD5Sample.Encryption.createHash(password);
            var db = new RailwayEntities();
            var user = db.Users.FirstOrDefault(u => u.userID == userID && u.password == encryptedPassword);
            if (user != null)
            {
                Session["user"] = user;
                if (user.userTypeID == 1)
                {
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else if (user.userTypeID == 2)
                {
                    if (prevRef != null)
                    {
                        return Redirect(prevRef.ToString());
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                ViewBag.loginError = "Invalid User ID or Password...!";
            }
            return View();
        }

        [HttpPost]
        public ActionResult requestAccount(string name, string cnic, string email)
        {
            var db = new RailwayEntities();
            var request = db.AccountRequests.FirstOrDefault(r => r.CNIC == cnic);
            if (request != null)
            {
                ViewBag.signUpError = "A request with this CNIC already exits";
            }
            else
            {
                var user = db.Users.FirstOrDefault(u => u.CNIC == cnic);
                if (user != null)
                {
                    ViewBag.signUpError = "A user with this CNIC already exists";
                }
                else
                {
                    db.AccountRequests.Add(new AccountRequest() { name = name, email = email, CNIC = cnic });
                    db.SaveChanges();
                    ViewBag.requestSuccess = "Your request has been submitted successfully...!";
                }
            }

            return View("Index");
        }

        public ActionResult changePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult changePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            var db = new RailwayEntities();
            User u = (User)Session["user"];
            var toChangePassword = db.Users.Find(u.userID);
            if (toChangePassword.password == MD5Sample.Encryption.createHash(oldPassword))
            {
                if (newPassword == confirmNewPassword)
                {
                    toChangePassword.password = MD5Sample.Encryption.createHash(newPassword);
                    db.SaveChanges();
                    ViewBag.passwordChangeSuccess = "Password changed Successfully...!";
                }
                else
                {
                    ViewBag.passwordChange = "The new passwords don't match...!";
                }
            }
            else
            {
                ViewBag.passwordChange = "Sorry, Incorrect Old Password...!";
            }
            return View();
        }

        public ActionResult logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}