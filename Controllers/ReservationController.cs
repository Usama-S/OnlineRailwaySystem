using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineRailway.Controllers
{
    public class ReservationController : Controller
    {
        Object prevRef;
        // GET: Reservation
        public ActionResult Index()
        {
            if (Session["user"] != null)
                return View();
            else
                return RedirectToAction("Login", "Accounts");
        }

        public ActionResult Search(int pnr)
        {
            var db = new RailwayEntities();
            var ticket = db.Reservations.Find(pnr);
            ViewBag.ticket = ticket;
            if (ticket != null)
                return View();
            else
            {
                ViewBag.inValidPnr = "Invalid PNR No.";
                return View("Index");
            }
        }

        public ActionResult Reservation()
        {
            if (Session["user"] != null)
            {
                var db = new RailwayEntities();
                ViewBag.stations = db.Stations.ToList();
                ViewBag.trains = db.TrainNames.ToList();
                return View();
            }
            else
                return RedirectToAction("Login", "Accounts");
        }

        [HttpGet]
        public ActionResult Proceed(int from, int to, int trainID, string trainClass, string seats,
            string name, string email, string cnic, string phoneNumber, int fare)
        {
            var db = new RailwayEntities();
            prevRef = Request.UrlReferrer;
            if (from > 0 && to > 0 && trainID > 0 && trainClass != null && seats != null &&
                name != null && email != null && cnic != null && phoneNumber != null)
            {
                var seatsAvailable = db.Trains.Find(trainID).seatsAvailables.First();

                if ((trainClass == "economy" && seatsAvailable.economySeatsAvailable >= Convert.ToInt32(seats)) ||
                    (trainClass == "ac" && seatsAvailable.acSeatsAvailable >= Convert.ToInt32(seats)) ||
                    (trainClass == "sleeper" && seatsAvailable.sleeperSeatsAvailable >= Convert.ToInt32(seats)))
                {
                    db.Reservations.Add(new Reservation()
                    {
                        fromStation = from,
                        toStation = to,
                        trainID = trainID,
                        reservationDate = DateTime.Now,
                        userID = ((User)Session["user"]).userID,
                        noOfSeats = Convert.ToInt32(seats),
                        fare = fare,
                        reservationStatus = 1,
                        trainClass = trainClass,
                        passengerName = name,
                        passengerEmail = email,
                        passengerPh = phoneNumber,
                        passengerCNIC = cnic,
                    });
                    var thisTrain = db.seatsAvailables.Find(trainID);
                    switch (trainClass)
                    {
                        case "economy":
                            thisTrain.economySeatsAvailable = thisTrain.economySeatsAvailable - Convert.ToInt32(seats);
                            break;
                        case "ac":
                            thisTrain.acSeatsAvailable = thisTrain.acSeatsAvailable - Convert.ToInt32(seats);
                            break;
                        case "sleeper":
                            thisTrain.sleeperSeatsAvailable = thisTrain.sleeperSeatsAvailable - Convert.ToInt32(seats);
                            break;
                        default:
                            thisTrain.economySeatsAvailable = thisTrain.economySeatsAvailable - Convert.ToInt32(seats);
                            break;
                    }
                    db.SaveChanges();
                    var currentData = db.Reservations.Where(s => s.passengerEmail == email).ToList();
                    db.Transactions.Add(new Transaction()
                    {
                        transactionDate = currentData.Last().reservationDate,
                        transactionType = "reservation",
                        reservationID = currentData.Last().PNRno,
                        amount = currentData.Last().fare
                    });
                    db.SaveChanges();
                    var seatsfound = db.seats.Where(s => s.trainId == trainID && s.trainClass == trainClass).ToList();
                    for (int i = 0; i< Convert.ToInt32(seats); i++)
                    {
                        seatsfound[i].reservationId = currentData.Last().PNRno;
                        seatsfound[i].statusId = 2;
                    }
                    db.SaveChanges();
                    Mailings.Program.mail(email, currentData.Last().PNRno, "CONFIRMED");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(prevRef.ToString());
                }
            }
            else
            {
                return Redirect(prevRef.ToString());
            }
        }

        [Route("Reservation/Cancel/{id}")]
        public ActionResult Cancel(int id)
        {
            var db = new RailwayEntities();
            var booking = db.Reservations.Find(id);
            DateTime currentDate = DateTime.Now;
            DateTime departure = (DateTime)booking.Train.departureDate;
            TimeSpan duration = departure.Subtract(currentDate);
            var refund = 0;

            if (booking.reservationStatus != 2)
            {
                if (duration.TotalDays > 0 || (duration.TotalDays == 0 && duration.TotalHours > 6))
                {
                    booking.reservationStatus = 2;

                    switch (booking.trainClass)
                    {
                        case "economy":
                            db.Trains.Find(booking.trainID).seatsAvailables.First().economySeatsAvailable += booking.noOfSeats;
                            break;
                        case "ac":
                            db.Trains.Find(booking.trainID).seatsAvailables.First().acSeatsAvailable += booking.noOfSeats;
                            break;
                        case "sleeper":
                            db.Trains.Find(booking.trainID).seatsAvailables.First().sleeperSeatsAvailable += booking.noOfSeats;
                            break;
                        default:
                            break;
                    }

                    if (duration.TotalDays == 2)
                        refund = -1 * (Convert.ToInt32(booking.fare * 0.75));
                    else if (duration.TotalDays == 1 || (duration.TotalDays == 0 && duration.TotalHours >= 6))
                        refund = -1 * (Convert.ToInt32(booking.fare * 0.5));
                    else if (duration.TotalDays == 0 && duration.TotalHours < 6)
                        refund = -1 * (Convert.ToInt32(booking.fare * 0.25));
                    else
                        refund = -1 * (Convert.ToInt32(booking.fare));

                    db.Transactions.Add(new Transaction()
                    {
                        reservationID = booking.PNRno,
                        transactionDate = DateTime.Now,
                        transactionType = "Cancellation",
                        amount = refund
                    });
                    db.SaveChanges();
                    var seatCancel = db.seats.Where(s => s.reservationId == id).ToList();
                    foreach (var item in seatCancel)
                    {
                        item.reservationId  = null;
                        item.statusId = 1;
                    }
                    db.SaveChanges();
                    ViewBag.cancellationSuccess = "Your Reservation has Been Cancelled";
                    Mailings.Program.mail(booking.passengerEmail, booking.PNRno, db.Reservations.Find(booking.PNRno).ReservationStatu.status);
                    return View("Index");
                }
                else
                {
                    ViewBag.cancellationSuccess = "Your Reservation can NOT be Cancelled";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.cancellationSuccess = "Your Reservation is already Cancelled";
                return View("Index");
            }
        }
    }
}