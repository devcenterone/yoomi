﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Yoomi.Entity;

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



        public async Task<JsonResult> SendOrder(OrderForm form, FormCollection col)
        {
            //If model not valid return to form page for validation
            if (!ModelState.IsValid)
            {

                var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage);
                string errors = String.Join(string.Empty, allErrors);

                return Json(new { Result = "ERROR", Message = errors });
            }


            StringBuilder mailBody = new StringBuilder();

            mailBody.AppendFormat("Nume client: {0}", form.Name);
            mailBody.Append("\r\n ");
            mailBody.AppendFormat("Email client: {0}", form.Email);
            mailBody.Append("\r\n ");
            mailBody.AppendFormat("Telefon client: {0}", form.Phone);
            mailBody.Append("\r\n ");
            mailBody.Append("\r\n ");
            mailBody.Append("Produsele:");
            mailBody.Append("\r\n ");
            var index = 1;
            foreach (var prod in form.Products)
            {
                mailBody.AppendFormat(" {0}. {1}",index++,prod);
                mailBody.Append("\r\n ");
            }




            await SendEmailAsync("sergiu.barbu@gmail.com", mailBody.ToString());

            return Json(new { Result = "OK" });
        }


        public async Task SendEmailAsyncOld(string email, string subject = "Comanda Yoomi.shop.ro", string message = "")
        {

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Yoomi.shop.ro", "yoomi.shop.ro@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("c.pop.vaida@gmail.com", email));
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


                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls).ConfigureAwait(false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync("yoomi.shop.ro@gmail.com", "cristipopvaida");                
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        
        }

        public async Task SendEmailAsync(string email, string subject = "Comanda Yoomi.shop.ro", string message = "")
        {           

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Yoomi.shop.ro", "yoomi.shop.ro@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("c.pop.vaida@gmail.com", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };



            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential
                {
                    UserName = "yoomi.shop.ro@gmail.com", // replace with valid value
                    Password = "cristipopvaida" // replace with valid value
                };

                client.LocalDomain = "yoomi.herokuapp.com";
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto).ConfigureAwait(false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(credentials);

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
                //You need to add return here
                //return RedirectToAction("Thanks");
            }

        }



    }
}
