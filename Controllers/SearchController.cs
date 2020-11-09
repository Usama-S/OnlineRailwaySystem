using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineRailway.Controllers
{
    public class SearchController : Controller
    {
        Object prevRef;
        // GET: Search
        public ActionResult Index()
        {
            prevRef = Request.UrlReferrer;
            return View();
        }

        [HttpGet]
        public ActionResult searchTrip(int departureCity, int arrivalCity, DateTime departureDate)
        {
            ViewBag.departure = departureCity;
            ViewBag.arrival = arrivalCity;
            List<Train_custom> trainSearchResutls = new List<Train_custom>();
            var db = new RailwayEntities();
            var trains = db.Trains.ToList();
            Train train = null;
            int fare = 0;
            int distance = 0;
            foreach (var item in trains)
            {
                train = null;
                fare = 0;
                distance = 0;
                if (departureDate == (DateTime)item.departureDate)
                {
                    var stationName = departureCity;
                    var checkFirstStation = 0;
                    foreach (var items in item.TrainRoute.RouteStations)
                    {
                        if (items.Station.stationCode == stationName)
                        {
                            if (checkFirstStation == 1)
                            {
                                checkFirstStation = 0;
                                train = item;
                            }
                            else
                            {
                                stationName = arrivalCity;
                                checkFirstStation = 1;
                            }
                        }
                        if (checkFirstStation == 1)
                        {
                            //var f = (FareRule)item.TrainName.FareRules;
                            //fare = fare + Convert.ToDouble(f.economyFare);
                            distance = Convert.ToInt32(items.nextStationDistance);
                        }

                    }
                }
                else
                {
                    ViewBag.trainSearchError = "Sorry, No trains found for this date.";
                }
                if (train != null)
                {
                    var far = (FareRule)train.TrainName.FareRules.FirstOrDefault(k => k.trainNameID == train.trainNameID);
                    //var f = (FareRule)item.TrainName.FareRules;
                    fare = Convert.ToInt32(Convert.ToDouble(far.economyFare) * distance);
                    trainSearchResutls.Add(new Train_custom() { train = train, fare = fare });
                }
            }
            if (!trainSearchResutls.Any())
            {
                ViewBag.trainSearchError2 = "Sorry, No trains found for this route.";
            }
            else
            {
                ViewBag.trainSearch = trainSearchResutls;
                //ViewBag.fares = fares;
                return View("Index");
            }
            return Redirect(Convert.ToString(prevRef));
        }
    }

    public class Train_custom
    {
        public Train train;
        public int fare;
    }
}