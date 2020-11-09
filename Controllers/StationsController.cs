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
    public class StationsController : Controller
    {
        private RailwayEntities db = new RailwayEntities();

        // GET: Stations
        public ActionResult Index()
        {
            return View(db.Stations.ToList());
        }

        // GET: Stations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                ViewBag.nostation = "Invalid Station Name or ID";
                return View("findStation");
            }
            return View(station);
        }

        // GET: Stations/Details/5
        public ActionResult Search(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int stationCode = -1;
            Station station;
            if (int.TryParse(id,out stationCode))
                station = db.Stations.FirstOrDefault(s => s.stationCode == stationCode);
            else
                station = db.Stations.FirstOrDefault(s => s.stationName == id);

            if (station == null)
            {
                ViewBag.nostation = "Invalid Station Name or ID";
                return View("findStation");
            }
            //return View(station);
            return Redirect("/Stations/Details/" + station.stationCode);
        }

        // GET: Stations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "stationCode,stationName,railwayDivisionName")] Station station)
        {
            if (ModelState.IsValid)
            {
                db.Stations.Add(station);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(station);
        }

        // GET: Stations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "stationCode,stationName,railwayDivisionName")] Station station)
        {
            if (ModelState.IsValid)
            {
                db.Entry(station).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(station);
        }

        // GET: Stations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Station station = db.Stations.Find(id);
            db.Stations.Remove(station);
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

        public ActionResult findStation()
        {
            return View();
        }
    }
}
