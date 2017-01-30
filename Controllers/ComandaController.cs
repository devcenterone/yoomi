using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;


namespace Yoomi.Controllers
{
    public class ComandaController : Controller
    {
        private const string fromPassword = "fromPassword";
        private const string subject = "Subject";
        private const string body = "Body";

        public IActionResult Index()
        {            
            return View();
        }        
        public IActionResult Error()
        {
            return View();
        }


        public async Task<JsonResult> SendOrder(string producIds)
        {
            await SendEmailAsync("sergiu.barbu@gmail.com");

            return Json(new { Result = "OK"});
        }


        public async Task SendEmailAsync(string email, string subject="test", string message="test")
        {
            
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Joe Bloggs", "sergiu.barbu@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("sergiu.barbu@gmail.com", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };
            

            using (var client = new SmtpClient())
            {
                client.LocalDomain = "yoomi.herokuapp.com";                
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.None).ConfigureAwait(false);
                await client.AuthenticateAsync("sergiu.barbu@gmail.com", "smarandaareochiiverzi")
                       .ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }



    }
}
