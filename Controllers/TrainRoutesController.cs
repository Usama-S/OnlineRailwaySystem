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
    public class TrainRoutesController : Controller
    {
        private RailwayEntities db = new RailwayEntities();

        // GET: TrainRoutes
        public ActionResult Index()
        {
            var trainRoutes = db.TrainRoutes.Include(t => t.TrainName);
            return View(trainRoutes.ToList());
        }

        // GET: TrainRoutes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainRoute trainRoute = db.TrainRoutes.Find(id);
            if (trainRoute == null)
            {
                return HttpNotFound();
            }
            ViewBag.routeStations = trainRoute.RouteStations.ToList();
            return View(trainRoute);
        }

        // GET: TrainRoutes/Create
        public ActionResult Create()
        {
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1");
            return View();
        }

        // POST: TrainRoutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,trainNameID,upOrDownStatus")] TrainRoute trainRoute)
        {
            if (ModelState.IsValid)
            {
                db.TrainRoutes.Add(trainRoute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", trainRoute.trainNameID);
            return View(trainRoute);
        }

        // GET: TrainRoutes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainRoute trainRoute = db.TrainRoutes.Find(id);
            if (trainRoute == null)
            {
                return HttpNotFound();
            }
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", trainRoute.trainNameID);
            return View(trainRoute);
        }

        // POST: TrainRoutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,trainNameID,upOrDownStatus")] TrainRoute trainRoute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainRoute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.trainNameID = new SelectList(db.TrainNames, "id", "trainName1", trainRoute.trainNameID);
            return View(trainRoute);
        }

        // GET: TrainRoutes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainRoute trainRoute = db.TrainRoutes.Find(id);
            if (trainRoute == null)
            {
                return HttpNotFound();
            }
            return View(trainRoute);
        }

        // POST: TrainRoutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainRoute trainRoute = db.TrainRoutes.Find(id);
            db.TrainRoutes.Remove(trainRoute);
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
