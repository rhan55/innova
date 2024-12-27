using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using YKPortal.Areas.E.Models.Dto;
using YKPortal.Models.Dto;
using YKPortal.Models;
using System.Collections.Generic;
using System.Web;

namespace YKPortal.Areas.E.Controllers
{
    public class ETicaretSabitSayfalarController : BaseController
    {
        public ActionResult Sayfa(string id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM ETicaret_SabitSayfalar WHERE UrlAd = @UrlAd AND Durum = 1");
            cmd.Parameters.AddWithValue("@UrlAd", id);
           
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            if (dt.Rows.Count == 0)
            {
                return RedirectToAction("SayfaBulunamadi");
            }

            var entity = new ETicaretSabitSayfalarDto();

            entity.SayfaID = Convert.ToString(dt.Rows[0]["SayfaID"]);
            entity.Ad = Convert.ToString(dt.Rows[0]["Ad"]);
            entity.UrlAd = Convert.ToString(dt.Rows[0]["UrlAd"]);
            entity.Icerik = Convert.ToString(dt.Rows[0]["Icerik"]);

            return View(entity);
        }

        [HttpGet]
        public ActionResult SayfaBulunamadi()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SayfaGuncelle(string SayfaID)
        {
            var entity = new ETicaretSabitSayfalarDto();
            if (string.IsNullOrWhiteSpace(SayfaID))
            {
                return View(entity);
            } 

            entity = SabitSayfaGetir(SayfaID);
            return View(entity);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SayfaGuncelle(ETicaretSabitSayfalarDto eTicaretSabitSayfalarDto)
        {

            SqlCommand cmd;

            if (string.IsNullOrWhiteSpace(eTicaretSabitSayfalarDto.SayfaID))
            {
                cmd = new SqlCommand("INSERT INTO ETicaret_SabitSayfalar (Ad,UrlAd, Icerik, Durum, OlusturulmaTarihi, GuncellemeTarihi) VALUES (@Ad, @UrlAd, @Icerik, @Durum, @OlusturulmaTarihi, @GuncellenmeTarihi)");
                cmd.Parameters.AddWithValue("@OlusturulmaTarihi", DateTime.Now);  // Tarih
            }
            else
            {
                cmd = new SqlCommand("UPDATE  ETicaret_SabitSayfalar SET Ad = @Ad, UrlAd = @UrlAd, Icerik = @Icerik, Durum = @Durum, GuncellenmeTarihi = @GuncellenmeTarihi WHERE SayfaID = @SayfaID");
            }

            cmd.Parameters.AddWithValue("@Icerik", HttpUtility.HtmlEncode(eTicaretSabitSayfalarDto.Icerik));
            cmd.Parameters.AddWithValue("@SayfaID", eTicaretSabitSayfalarDto.SayfaID);
            cmd.Parameters.AddWithValue("@UrlAd", eTicaretSabitSayfalarDto.UrlAd);
            cmd.Parameters.AddWithValue("@Ad", eTicaretSabitSayfalarDto.Ad);           
            cmd.Parameters.AddWithValue("@Durum", true);  
            cmd.Parameters.AddWithValue("@GuncellenmeTarihi", DateTime.Now);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
   
            return RedirectToAction($"Sayfa/{eTicaretSabitSayfalarDto.UrlAd}");
        }

    }
}