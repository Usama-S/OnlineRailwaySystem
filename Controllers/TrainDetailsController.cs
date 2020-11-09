using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineRailway.Controllers
{
    public class TrainDetailsController : ApiController
    {
        [HttpGet]
        [Route("api/searchTrain/{from}/{to}")]
        public List<Train> searchTrain(int from, int to)
        {
            var db = new RailwayEntities();
            List<Train> trainsFound = new List<Train>();
            if (from != to && from > 0 && to > 0)
            {
                var trains = db.Trains;
                Train train = null;
                foreach (var item in trains)
                {
                    train = null;
                    var stationName = from;
                    var checkFirstStation = 0;

                    if (item.TrainRoute != null)
                        foreach (var items in item.TrainRoute.RouteStations)
                        {
                            if (items.Station != null && items.Station.stationCode == stationName)
                            {
                                if (checkFirstStation == 1)
                                {
                                    checkFirstStation = 0;
                                    train = item;
                                }
                                else
                                {
                                    stationName = to;
                                    checkFirstStation = 1;
                                }
                            }

                        }
                    if (train != null)
                    {
                        trainsFound.Add(new Train()
                        {
                            id = train.id,
                            TrainName = new TrainName() { trainName1 = train.TrainName.trainName1, id = train.TrainName.id }
                        });
                    }
                }
            }
            return trainsFound;
        }

        [HttpGet]
        [Route("api/searchTrainWithDate/{from}/{to}/{year}/{month}/{date}")]
        public List<Train> searchTrainWithDate(int from, int to, int year, int month, int date)
        {
            List<Train> trainsFound = new List<Train>();
            var db = new RailwayEntities();
            var trains = db.Trains.ToList();
            var departureDate = new DateTime(year, month, date);
            Train train = null;
            foreach (var item in trains)
            {
                train = null;
                if (departureDate == (DateTime)item.departureDate)
                {
                    var stationName = from;
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
                                stationName = to;
                                checkFirstStation = 1;
                            }
                        }
                    }
                }
                if (train != null &&
                    (train.seatsAvailables.First().economySeatsAvailable > 0 ||
                    train.seatsAvailables.First().acSeatsAvailable > 0 ||
                    train.seatsAvailables.First().sleeperSeatsAvailable > 0))
                {
                    trainsFound.Add(new Train()
                    {
                        id = train.id,
                        TrainName = new TrainName() { trainName1 = train.TrainName.trainName1, id = train.TrainName.id }
                    });
                }

            }
            return trainsFound;
        }

        [HttpGet]
        [Route("api/getTime/{from}/{to}/{trainID}")]
        public List<string> getTime(int from, int to, int trainID)
        {
            List<string> times = new List<string>();
            var db = new RailwayEntities();
            var train = db.Trains.Find(trainID);
            foreach (var item in train.TrainRoute.RouteStations)
            {
                if (item.StationID == from)
                {
                    times.Add(item.stationDepartureTime.ToString());
                }
                if (item.StationID == to)
                {
                    times.Add(item.stationArrivalTime.ToString());
                }
            }
            return times;
        }

        [HttpGet]
        [Route("api/getFare/{from}/{to}/{trainID}/{trainClass}/{seats}")]
        public int getFare(int from, int to, int trainID, string trainClass, string seats)
        {
            int fare = 0;
            int distance = 0;
            var db = new RailwayEntities();
            var train = db.Trains.Find(trainID);
            var stationName = from;
            var checkFirstStation = 0;
            foreach (var item in train.TrainRoute.RouteStations)
            {
                if (item.StationID == from)
                {
                    checkFirstStation = 1;
                }
                else if (item.StationID == to)
                {
                    checkFirstStation = 0;
                }
                if (checkFirstStation == 1)
                {
                    distance += Convert.ToInt32(item.nextStationDistance);
                }
            }

            var fareRule = train.TrainName.FareRules.First();

            switch (trainClass)
            {
                case "economy":
                    if (train.seatsAvailables.First().economySeatsAvailable > 0)
                        fare = Convert.ToInt32(fareRule.economyFare * distance) * Convert.ToInt32(seats);
                    else
                        fare = -1;
                    break;
                case "ac":
                    if (train.seatsAvailables.First().acSeatsAvailable > 0)
                        fare = Convert.ToInt32(fareRule.acFare * distance) * Convert.ToInt32(seats);
                    else
                        fare = -1;
                    break;
                case "sleeper":
                    if (train.seatsAvailables.First().sleeperSeatsAvailable > 0)
                        fare = Convert.ToInt32(fareRule.sleeperFare * distance) * Convert.ToInt32(seats);
                    else
                        fare = -1;
                    break;
                default:
                    if (train.seatsAvailables.First().economySeatsAvailable > 0)
                        fare = Convert.ToInt32(fareRule.economyFare * distance) * Convert.ToInt32(seats);
                    else
                        fare = -1;
                    break;
            }
            return fare;
        }

    }
}