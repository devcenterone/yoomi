using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Yoomi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Comanda()
        {
            ViewData["Message"] = "Pagina de comanda.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }



        public async Task<JsonResult> SendOrder(string producIds)
        {
            await SendEmailAsync("sergiu.barbu@gmail.com");

            return Json(new { Result = "OK" });
        }


        public async Task SendEmailAsync(string email, string subject = "test", string message = "test")
        {

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Joe Bloggs", "sergiu.barbu@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("sergiu.barbu@aegon.ro", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };


            using (var client = new SmtpClient())
            {
                //client.LocalDomain = "yoomi.herokuapp.com";
                //client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls)
                //await client.AuthenticateAsync("sergiu.barbu@gmail.com", "smarandaareochiiverzi")
                //       .ConfigureAwait(false);
                //await client.SendAsync(emailMessage).ConfigureAwait(false);
                //await client.DisconnectAsync(true).ConfigureAwait(false);


                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.None).ConfigureAwait(false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync("sergiu.barbu@gmail.com", "smarandaareochiiverzi");

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        
        }


    }
}
