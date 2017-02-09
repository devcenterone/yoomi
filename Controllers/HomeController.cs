using System;

using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;
using MimeKit;
using MimeKit.Text;
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

        public IActionResult ModDeUtilizare()
        {
            ViewData["Message"] = "Mod de utilizare.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "pagina de contact.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }



        public async Task<JsonResult> SendOrder(OrderForm form, FormCollection col)
        {

            if (form.Products == null || !form.Products.Where(x => x > 0).Any())
                return Json(new { Result = "ERROR", Message = "Alegeti cel putin un produs pt comanda." });

            //If model not valid return to form page for validation
            if (!ModelState.IsValid || ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).Any())
            {

                var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage);
                string errors = String.Join("<br />", allErrors);

                return Json(new { Result = "ERROR", Message = errors });
            }


            StringBuilder mailBody = new StringBuilder();

            mailBody.AppendFormat("Nume: {0}", form.Name).Append("<---->"); 
            
            mailBody.AppendFormat("Adresa: {0}", form.Address).Append("<---->");
            
            mailBody.AppendFormat("Email: {0}", form.Email).Append("<---->");
            
            mailBody.AppendFormat("Telefon: {0}", form.Phone).Append("<---->");
            
            
            mailBody.Append("Produsele:").Append("<---->");
            
    
            for (var i = 0; i < form.Products.Where(x => x > 0).Count(); i++)
            {
                mailBody.AppendFormat(" {0}. {1} [{2} buc]",i+1, form.ProductsNm[i], form.Products[i]).Append("<---->");
                
            }


            await SendEmailAsync("c.pop.vaida@gmail.com", mailBody.ToString());
            //await SendEmailAsync("sergiu.barbu@gmail.com", mailBody.ToString());

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
            //emailMessage.To.Add(new MailboxAddress("c.pop.vaida@gmail.com", email));
            emailMessage.To.Add(new MailboxAddress("cristi pop", email));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart( TextFormat.Plain) { Text = message, };



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
