using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OnlineRailway.Controllers
{
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard
        public ActionResult Index()
        {

            var db = new RailwayEntities();
            ViewBag.pendingRequests = db.AccountRequests.Count();
            var transaction = db.Transactions.ToList();
            int monthlyTransaction = 0;
            int dailyTransaction = 0;
            int annualTransaction = 0;
            var date = DateTime.Now;
            List<Transaction> transactions = new List<Transaction>();
            foreach (var item in transaction)
            {
                var transactionDate = (DateTime)item.transactionDate;
                if (transactionDate.Year == date.Year)
                {
                    annualTransaction = annualTransaction + Convert.ToInt32(item.amount);
                    if (transactionDate.Month == date.Month)
                    {
                        transactions.Add(item);
                        monthlyTransaction = monthlyTransaction + Convert.ToInt32(item.amount);
                        if (transactionDate.Date == date.Date)
                        {
                            dailyTransaction = dailyTransaction + Convert.ToInt32(item.amount);
                        }
                    }
                }
            }

            ViewBag.annualEarnings = annualTransaction;
            ViewBag.monthlyEarnings = monthlyTransaction;
            ViewBag.dailyEarnings = dailyTransaction;
            ViewBag.transactions = transactions;


            if (Session["user"] != null)
            {
                User u = (User)Session["user"];
                if (u.userTypeID == 1)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("login", "Accounts");
            }
        }

        public ActionResult Report()
        {
            var db = new RailwayEntities();
            var transactions = db.Transactions;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Transaction ID,Reservation ID,Transaction Date,Transaction Type,Amount");
            foreach (var item in transactions)
            {
                sb.AppendLine(item.id + "," + item.reservationID + "," + ((DateTime)item.transactionDate).ToShortDateString() + "," + item.transactionType + "," + item.amount);
            }
            byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(sb.ToString());
            return File(data, "text/csv", "Report.csv");
        }

    }
}