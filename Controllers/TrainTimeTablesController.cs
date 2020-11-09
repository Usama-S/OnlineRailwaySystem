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
    public class TrainTimeTablesController : Controller
    {
        private RailwayEntities db = new RailwayEntities();

        // GET: TrainTimeTables
        public ActionResult Index()
        {
            var trainTimeTables = db.TrainTimeTables.Include(t => t.TrainDay).Include(t => t.TrainName);
            return View(trainTimeTables.ToList());
        }

        // GET: TrainTimeTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainTimeTable trainTimeTable = db.TrainTimeTables.Find(id);
            if (trainTimeTable == null)
            {
                return HttpNotFound();
            }
            return View(trainTimeTable);
        }

        // GET: TrainTimeTables/Create
        public ActionResult Create()
        {
            ViewBag.trainDaysID = new SelectList(db.TrainDays, "id", "id");
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1");
            return View();
        }

        // POST: TrainTimeTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,trainNameID,trainDaysID,upDepartureTime,upArrivalTime,downDepartureTime,downArrivalTime")] TrainTimeTable trainTimeTable)
        {
            if (ModelState.IsValid)
            {
                db.TrainTimeTables.Add(trainTimeTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.trainDaysID = new SelectList(db.TrainDays, "id", "id", trainTimeTable.trainDaysID);
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", trainTimeTable.trainNameID);
            return View(trainTimeTable);
        }

        // GET: TrainTimeTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainTimeTable trainTimeTable = db.TrainTimeTables.Find(id);
            if (trainTimeTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.trainDaysID = new SelectList(db.TrainDays, "id", "id", trainTimeTable.trainDaysID);
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", trainTimeTable.trainNameID);
            return View(trainTimeTable);
        }

        // POST: TrainTimeTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,trainNameID,trainDaysID,upDepartureTime,upArrivalTime,downDepartureTime,downArrivalTime")] TrainTimeTable trainTimeTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainTimeTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.trainDaysID = new SelectList(db.TrainDays, "id", "id", trainTimeTable.trainDaysID);
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", trainTimeTable.trainNameID);
            return View(trainTimeTable);
        }

        // GET: TrainTimeTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainTimeTable trainTimeTable = db.TrainTimeTables.Find(id);
            if (trainTimeTable == null)
            {
                return HttpNotFound();
            }
            return View(trainTimeTable);
        }

        // POST: TrainTimeTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainTimeTable trainTimeTable = db.TrainTimeTables.Find(id);
            db.TrainTimeTables.Remove(trainTimeTable);
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
