using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKEFaturaEntegrasyon.EFaturaEDM;
using YKPortal.Areas.E.Models.Dto;
using YKPortal.Controllers;

namespace YKPortal.Areas.E.Controllers
{
    public class ETicaretSepetController : BaseController
    {
        // GET: E/Sepet
        [HttpGet]
        public ActionResult SepetBilgileri()
        {
            UlkeListesiniOlustur();
            IlListesiniOlustur();
            return View();
        }

        [HttpPost]
        public JsonResult SepeteEkle(ETicaretSepetDto.ETicaretSepetEkleDto sepetEkleDto)
        {

            var sonuc = SepetKaydet(sepetEkleDto);

            return Json(new YKJsonResult { SonucKodu = sonuc ? "Başarılı" : "Hata!" } );
        }
    
    }
}