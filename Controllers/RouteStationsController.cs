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
    public class RouteStationsController : Controller
    {
        private RailwayEntities db = new RailwayEntities();

        // GET: RouteStations
        public ActionResult Index()
        {
            var routeStations = db.RouteStations.Include(r => r.TrainRoute).Include(r => r.Station);
            return View(routeStations.ToList());
        }

        // GET: RouteStations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteStation routeStation = db.RouteStations.Find(id);
            if (routeStation == null)
            {
                return HttpNotFound();
            }
            return View(routeStation);
        }

        // GET: RouteStations/Create
        public ActionResult Create()
        {
            ViewBag.TrainRoutesID = new SelectList(db.TrainRoutes, "id", "name");
            ViewBag.StationID = new SelectList(db.Stations, "stationCode", "stationName");
            return View();
        }

        // POST: RouteStations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,TrainRoutesID,StationID,stationArrivalTime,stationDepartureTime,lastStationDistance,nextStationDistance")] RouteStation routeStation)
        {
            if (ModelState.IsValid)
            {
                db.RouteStations.Add(routeStation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TrainRoutesID = new SelectList(db.TrainRoutes, "id", "name", routeStation.TrainRoutesID);
            ViewBag.StationID = new SelectList(db.Stations, "stationCode", "stationName", routeStation.StationID);
            return View(routeStation);
        }

        // GET: RouteStations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteStation routeStation = db.RouteStations.Find(id);
            if (routeStation == null)
            {
                return HttpNotFound();
            }
            ViewBag.TrainRoutesID = new SelectList(db.TrainRoutes, "id", "name", routeStation.TrainRoutesID);
            ViewBag.StationID = new SelectList(db.Stations, "stationCode", "stationName", routeStation.StationID);
            return View(routeStation);
        }

        // POST: RouteStations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,TrainRoutesID,StationID,stationArrivalTime,stationDepartureTime,lastStationDistance,nextStationDistance")] RouteStation routeStation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routeStation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TrainRoutesID = new SelectList(db.TrainRoutes, "id", "name", routeStation.TrainRoutesID);
            ViewBag.StationID = new SelectList(db.Stations, "stationCode", "stationName", routeStation.StationID);
            return View(routeStation);
        }

        // GET: RouteStations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteStation routeStation = db.RouteStations.Find(id);
            if (routeStation == null)
            {
                return HttpNotFound();
            }
            return View(routeStation);
        }

        // POST: RouteStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RouteStation routeStation = db.RouteStations.Find(id);
            db.RouteStations.Remove(routeStation);
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
