using System.Data.SqlClient;
using System.Data;
using System;
using System.Web.Mvc;
using YKPortal.Areas.E.Models.Dto;
using YKPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YKEFaturaEntegrasyon.EFaturaEDM;
using YKPortal.Controllers;


namespace YKPortal.Areas.E.Controllers
{
    public class ETicaretSiteController : BaseController
    {
        [HttpGet]
        public ActionResult AnaSayfa()
        {
            // Stokları al ve sadece ilk 25 ürünü seç
            var stoklar = StokGetir(new ETicaretStokDto.ETicaretStokFiltreDto { });
           

            // İlk 25 kaydı al ve ViewBag'e ata
            ViewBag.Stoklar = stoklar.Take(25).ToList();
            ViewBag.Slaytlar = SlaytListesi("bba8dbbd-b83a-43cd-9699-a407653a1cdb");
            return View();
        }
        [HttpPost]
        public JsonResult SepeteEkle(ETicaretSepetDto.ETicaretSepetEkleDto sepetEkleDto)
        {

            var sonuc = SepetKaydet(sepetEkleDto);

            return Json(new YKJsonResult { SonucKodu = sonuc ? "Başarılı" : "Hata!" });
        }



    }

}
