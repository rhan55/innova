using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;
using YKPortal.Areas.E.Models.Dto;

namespace YKPortal.Areas.E.Controllers
{
    public class KategoriController : BaseController
    {
        // GET: E/Kategori
        [HttpGet]
        public ActionResult Kategoriler(KategorilerDto kategorilerDto)
        {
            return View(KategorileriGetir());
        }
    }
}