using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using App2.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace App2.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model) {

            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }
            TempData["success"] = true;

            MailMessage message = new MailMessage();
            message.To.Add("username@eaaa.dk");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            IContent msg = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "message");
            msg.SetValue("messageName", model.Name);
            msg.SetValue("email", model.Email);
            msg.SetValue("subject", model.Subject);
            msg.SetValue("messageContent", model.Message);

            Services.ContentService.Save(msg);

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("rinadoho@gmail.com", "algr tmtc siju zlgb");
                // send mail
                smtp.Send(message);
            }

                return RedirectToCurrentUmbracoPage();
        }
        //            // Read data from model and send mail
        
    }
}