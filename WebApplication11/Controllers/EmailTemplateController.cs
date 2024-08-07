using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace YKPortal.Controllers
{
    public class EmailTemplateController : Controller
    {
        // GET: EmailTemplate
        public ActionResult Index()
        {

            var tema = System.IO.File.ReadAllText(Path.GetPathRoot("MailTemalari/KabulMaili.html"));
            tema.Replace("[SiparisNumarasi]", "123459595");
            tema.Replace("[CariIsmi]", "YK YAzilim");

            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "mail.ykyazilim.com.tr";
            sc.EnableSsl = false;
            sc.Credentials = new NetworkCredential("ilayda@ykyazilim.com.tr", "Ilayda12#");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ilayda@ykyazilim.com.tr", ConfigurationManager.AppSettings["FirmaAdi"]);

            mail.To.Add("muharremkackin@gmail.com");

            mail.Subject = ConfigurationManager.AppSettings["FirmaAdi"] + " - Parola Sıfırlama";
            mail.IsBodyHtml = true;
            mail.Body = tema;

            return View();
        }
    }
}