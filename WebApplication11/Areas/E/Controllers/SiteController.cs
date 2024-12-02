using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YKPortal.Areas.Satinalma.Controllers;
using YKPortal.Models;
using YKPortal.Models.Dto;
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.E.Controllers
{
    public class SiteController : BaseController
    {
        [HttpGet]
        public ActionResult AnaSayfa()
        {
            return View();
        }

       
    }

}
