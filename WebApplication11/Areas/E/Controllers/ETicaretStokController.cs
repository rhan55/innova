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
        public ActionResult Stoklar(YKPortal.Areas.E.Models.Dto.ETicaretStokDto.ETicaretStokSorguDto stokSorguDto = null)
        {
            if (stokSorguDto == null)
            {
                stokSorguDto = new YKPortal.Areas.E.Models.Dto.ETicaretStokDto.ETicaretStokSorguDto();
            }
            ViewBag.Stoklar = StokGetir(stokSorguDto);
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

 