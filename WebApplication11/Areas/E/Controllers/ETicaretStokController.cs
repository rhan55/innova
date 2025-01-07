using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace YKPortal.Areas.E.Controllers
{
    [RouteArea("E")]
    public class ETicaretStokController : BaseController
    {

        [HttpGet]
        public ActionResult Stoklar(Models.Dto.ETicaretStokDto.ETicaretStokFiltreDto stokSorguDto = null)
        {
          
            ViewBag.Stoklar = StokGetir(stokSorguDto);
            ViewBag.Slaytlar = SlaytListesi("251cb42f-2aaa-46f2-9379-d0d5babed510");

            return View("~/Areas/E/Views/ETicaretStok/Stoklar.cshtml");
        }

        [HttpGet]
        public ActionResult StokDetay(YKPortal.Areas.E.Models.Dto.ETicaretStokDto.ETicaretStokSorguDto stokSorguDto)
        {
            var stok = StokBul(stokSorguDto);
            
            ViewBag.Stok = stok;
            return View();
        }
    }
}

 