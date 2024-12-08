using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YKPortal.Areas.E.Controllers
{
    public class StokController : BaseController
    {
        // GET: E/Stok
        public ActionResult Stoklar()
        {
            ViewBag.Stoklar = StokGetir(new YKEFaturaEntegrasyon.Dto.ETicaretStokDto.ETicaretStokSorguDto());
            return View();
        }
    }
}

 