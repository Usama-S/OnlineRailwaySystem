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
    public class TrainsController : Controller
    {
        private RailwayEntities db = new RailwayEntities();

        // GET: Trains
        public ActionResult Index()
        {
            var trains = db.Trains.Include(t => t.TrainName).Include(t => t.TrainRoute).Include(t => t.TrainStatu);
            return View(trains.ToList());
        }

        // GET: Trains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train train = db.Trains.Find(id);
            if (train == null)
            {
                return HttpNotFound();
            }
            return View(train);
        }

        // GET: Trains/Create
        public ActionResult Create()
        {
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1");
            ViewBag.routeID = new SelectList(db.TrainRoutes, "id", "name");
            ViewBag.trainStatusID = new SelectList(db.TrainStatus, "id", "status");
            return View();
        }

        // POST: Trains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,trainNameID,trainNo,upOrDownStatus,routeID,trainStatusID,noOfAcCabins,noOfSleeperCabins,noOfEconomyCabins,departureDate")] Train train)
        {
            if (ModelState.IsValid)
            {
                db.Trains.Add(train);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", train.trainNameID);
            ViewBag.routeID = new SelectList(db.TrainRoutes, "id", "name", train.routeID);
            ViewBag.trainStatusID = new SelectList(db.TrainStatus, "id", "status", train.trainStatusID);
            return View(train);
        }

        // GET: Trains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train train = db.Trains.Find(id);
            if (train == null)
            {
                return HttpNotFound();
            }
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", train.trainNameID);
            ViewBag.routeID = new SelectList(db.TrainRoutes, "id", "name", train.routeID);
            ViewBag.trainStatusID = new SelectList(db.TrainStatus, "id", "status", train.trainStatusID);
            return View(train);
        }

        // POST: Trains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,trainNameID,trainNo,upOrDownStatus,routeID,trainStatusID,noOfAcCabins,noOfSleeperCabins,noOfEconomyCabins,departureDate")] Train train)
        {
            if (ModelState.IsValid)
            {
                db.Entry(train).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", train.trainNameID);
            ViewBag.routeID = new SelectList(db.TrainRoutes, "id", "name", train.routeID);
            ViewBag.trainStatusID = new SelectList(db.TrainStatus, "id", "status", train.trainStatusID);
            return View(train);
        }

        // GET: Trains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train train = db.Trains.Find(id);
            if (train == null)
            {
                return HttpNotFound();
            }
            return View(train);
        }

        // POST: Trains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Train train = db.Trains.Find(id);
            db.Trains.Remove(train);
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
