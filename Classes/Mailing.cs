using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace Mailings
{
    class Program
    {
        public static void mail(string emailAdd, int pnr, string status)
        {
            var toaddress = emailAdd;
            var fromaddress = "awami.usama@gmail.com";
            var subject = "Reservation";
            var body = "Dear Passenger, Your Reservation against PNR no. " + pnr + " has been " + status.ToUpper();

            var host = "smtp.gmail.com";                    //should be kept in a .config file
            var port = 587;                                 //should be kept in a .config file
            var username = "youremail@gmail.com";      //should be kept in a .config file
            var password = "your-email-password";                //should be kept in a .config file

            var client = new System.Net.Mail.SmtpClient(host, port);
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;

            MailMessage msg = new MailMessage(fromaddress, toaddress);
            msg.Subject = subject;
            msg.Body = body;
            client.Send(msg);
        }
    }
}
