using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineRailway;

namespace OnlineRailway.Controllers
{
    public class AccountRequestsController : Controller
    {
        private RailwayEntities db = new RailwayEntities();

        // GET: AccountRequests
        public ActionResult Index()
        {
            return View(db.AccountRequests.ToList());
        }

        // GET: AccountRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountRequest accountRequest = db.AccountRequests.Find(id);
            if (accountRequest == null)
            {
                return HttpNotFound();
            }
            return View(accountRequest);
        }

        // GET: AccountRequests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,CNIC,email")] AccountRequest accountRequest)
        {
            if (ModelState.IsValid)
            {
                db.AccountRequests.Add(accountRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accountRequest);
        }

        // GET: AccountRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountRequest accountRequest = db.AccountRequests.Find(id);
            if (accountRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = accountRequest.id;
            return View(accountRequest);
        }

        // POST: AccountRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [Bind(Include = "id,name,CNIC,email")] AccountRequest accountRequest)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(accountRequest).State = EntityState.Modified;
                var newUser = db.AccountRequests.Find(id);
                db.Users.Add(new User()
                {
                    password = MD5Sample.Encryption.createHash("nrail123"),
                    userTypeID = 2,
                    name = newUser.name,
                    CNIC = newUser.CNIC,
                    email = newUser.email
                });
                db.AccountRequests.Remove(newUser);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accountRequest);
        }

        // GET: AccountRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountRequest accountRequest = db.AccountRequests.Find(id);
            if (accountRequest == null)
            {
                return HttpNotFound();
            }
            return View(accountRequest);
        }

        // POST: AccountRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountRequest accountRequest = db.AccountRequests.Find(id);
            db.AccountRequests.Remove(accountRequest);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
