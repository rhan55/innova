using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YKPortal.Areas.E.Controllers
{
    public class YasalBilgilerController : BaseController
    {
        // GET: E/YasalBilgiler

        [HttpGet]
        public ActionResult MesafeliSatisSozlesmesi()
        {
            return View();
        }
        [HttpGet]
        public ActionResult KisiselVerilerinKorunmasi()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Gizlilik()
        {
            return View();
        }
    }
}