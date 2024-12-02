using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YKPortal.Areas.E.Controllers
{
    public class HakkimizdaController : BaseController
    {
        // GET: E/Hakkimizda
        [HttpGet]
        public ActionResult Hakkimizda()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Misyonumuz()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Vizyonumuz()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Iletisim()
        {
            return View();
        }
    }
}