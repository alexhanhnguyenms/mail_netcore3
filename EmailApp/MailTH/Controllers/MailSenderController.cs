using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentEmail.Core;
using System.Net.Mail;


namespace MailTH.Controllers
{
    public class MailSenderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public string SendEmail(string Name, string Email, string Message)
        {

            try
            {
                // Credentials
                var credentials = new NetworkCredential("your-email@gmail.com", "your-password");
                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("noreply@codinginfinite.com"),
                    Subject = "Email Sender App",
                    Body = Message
                };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(Email));
                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };
                client.Send(mail);
                return "Email Sent Successfully!";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

        }
    }
}