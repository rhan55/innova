using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YKPortal.Areas.E.Controllers
{
    public class SepetController : BaseController
    {
        // GET: E/Sepet
        [HttpGet]
        public ActionResult SepetBilgileri()
        {
            return View();
        }
    }
}