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
using YKEFaturaEntegrasyon.Dto;

namespace YKPortal.Areas.E.Controllers
{
    public class ETicaretKategoriController : BaseController
    {
        // GET: E/Kategori
        [HttpGet]
        public ActionResult KategoriDetay(ETicaretKategorilerDto kategorilerDto)
        {
            return View(KategorileriGetir());
        }

        [HttpGet]
        public ActionResult KategoriStoklari(ETicaretStokDto.ETicaretStokFiltreDto sorguDto)
        {
            var stokListesi = StokGetir(sorguDto);

            return View(stokListesi);  
        }

    }
}