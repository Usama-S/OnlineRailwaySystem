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
    public class TrainDaysController : Controller
    {
        private RailwayEntities db = new RailwayEntities();

        // GET: TrainDays
        public ActionResult Index()
        {
            var trainDays = db.TrainDays.Include(t => t.TrainName);
            return View(trainDays.ToList());
        }

        // GET: TrainDays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainDay trainDay = db.TrainDays.Find(id);
            if (trainDay == null)
            {
                return HttpNotFound();
            }
            return View(trainDay);
        }

        // GET: TrainDays/Create
        public ActionResult Create()
        {
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1");
            return View();
        }

        // POST: TrainDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,trainNameID,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] TrainDay trainDay)
        {
            if (ModelState.IsValid)
            {
                db.TrainDays.Add(trainDay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", trainDay.trainNameID);
            return View(trainDay);
        }

        // GET: TrainDays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainDay trainDay = db.TrainDays.Find(id);
            if (trainDay == null)
            {
                return HttpNotFound();
            }
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", trainDay.trainNameID);
            return View(trainDay);
        }

        // POST: TrainDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,trainNameID,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] TrainDay trainDay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainDay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", trainDay.trainNameID);
            return View(trainDay);
        }

        // GET: TrainDays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainDay trainDay = db.TrainDays.Find(id);
            if (trainDay == null)
            {
                return HttpNotFound();
            }
            return View(trainDay);
        }

        // POST: TrainDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainDay trainDay = db.TrainDays.Find(id);
            db.TrainDays.Remove(trainDay);
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
