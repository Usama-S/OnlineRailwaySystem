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
    public class TrainNamesController : Controller
    {
        private RailwayEntities db = new RailwayEntities();

        // GET: TrainNames
        public ActionResult Index()
        {
            return View(db.TrainNames.ToList());
        }

        // GET: TrainNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainName trainName = db.TrainNames.Find(id);
            if (trainName == null)
            {
                return HttpNotFound();
            }
            return View(trainName);
        }

        // GET: TrainNames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,trainName1")] TrainName trainName)
        {
            if (ModelState.IsValid)
            {
                db.TrainNames.Add(trainName);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trainName);
        }

        // GET: TrainNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainName trainName = db.TrainNames.Find(id);
            if (trainName == null)
            {
                return HttpNotFound();
            }
            return View(trainName);
        }

        // POST: TrainNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,trainName1")] TrainName trainName)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainName).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainName);
        }

        // GET: TrainNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainName trainName = db.TrainNames.Find(id);
            if (trainName == null)
            {
                return HttpNotFound();
            }
            return View(trainName);
        }

        // POST: TrainNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainName trainName = db.TrainNames.Find(id);
            db.TrainNames.Remove(trainName);
            db.SaveChanges();
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
