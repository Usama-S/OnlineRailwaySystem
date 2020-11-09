using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineRailway.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var db = new RailwayEntities();
            ViewBag.stations = db.Stations.ToList();
            return View();
        }
    }
}