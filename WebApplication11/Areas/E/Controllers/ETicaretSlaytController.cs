using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Areas.E.Models.Dto;
using YKPortal.Models;

namespace YKPortal.Areas.E.Controllers
{
    public class ETicaretSlaytController : BaseController
    {
        // GET: E/ETicaretSlayt

        [HttpGet]
        public ActionResult Slaytlar()
        {
            SlaytTipGetir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM ETicaret_Slaytlar WHERE Silindi = 0");

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var slaytTipleri = GrupKodListesiniGetir("ETicaretSlaytTip");

            if (dt.Rows.Count == 0)
            {
                return RedirectToAction("SayfaBulunamadi");
            }
            var entities = new List<ETicaretSlaytDto>();
            foreach (DataRow row in dt.Rows)
            {
                var grupKodu = slaytTipleri.Where(m => m.ID == Convert.ToString(row["Tip"])).First();
                var entity = new ETicaretSlaytDto();
                entity.SlaytID = Convert.ToString(row["SlaytID"]);
                entity.ResimYolu = Convert.ToString(row["ResimYolu"]);
                entity.Link = Convert.ToString(row["Link"]);
                entity.Text = Convert.ToString(row["Text"]);
                entity.Tip = Convert.ToString(row["Tip"]);
                entity.Aktif = Convert.ToBoolean(row["Aktif"]);
                entity.Siralama = Convert.ToInt32(row["Siralama"]);
                entity.TipDeger = grupKodu != null ? grupKodu.Deger : string.Empty;

                entities.Add(entity);
            }

            ViewBag.Tipler = entities.Select(e => e.Tip).Distinct().ToList();
            return View(entities);
        }

        [HttpGet]
        public ActionResult SlaytEkle()
        {
            SlaytTipGetir();
            var entity = new ETicaretSlaytDto();

            ViewBag.Tipler = new Dictionary<string, string>()
            {
                { "anasayfa-ust", "Anasayfa Üst" },
            };

            return View(entity);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SlaytEkle(ETicaretSlaytDto eTicaretSlaytDto, HttpPostedFileBase Dosya)
        {
            SlaytTipGetir();
            var resimYolu = "";
            if (Dosya != null && Dosya.ContentLength > 0)
            {
                var extension = Dosya.FileName.Split('.').Last();

                var fileName = Guid.NewGuid().ToString() + "." + extension;
                var filePath = Path.Combine(Server.MapPath("/Uploads/ETicaret"), fileName);
                Dosya.SaveAs(filePath);
                resimYolu = "/Uploads/ETicaret/" + fileName;
            }

            SqlCommand cmd;

            if (string.IsNullOrWhiteSpace(eTicaretSlaytDto.SlaytID))
            {
                // Yeni slayt ekleme
                cmd = new SqlCommand("INSERT INTO ETicaret_Slaytlar (ResimYolu, Link, Text, Tip,Aktif, OlusturulmaTarihi, Siralama) VALUES (@ResimYolu, @Link, @Text,@Tip, @Aktif, @OlusturulmaTarihi, @Siralama)");
            }
            else
            {
                // Mevcut slaytı güncelleme
                cmd = new SqlCommand("UPDATE ETicaret_Slaytlar SET ResimYolu = @ResimYolu, Link = @Link, Text = @Text,Tip = @Tip, Aktif = @Aktif, Siralama = @Siralama WHERE SlaytID = @SlaytID");
                cmd.Parameters.AddWithValue("@SlaytID", eTicaretSlaytDto.SlaytID);
            }
           
            cmd.Parameters.AddWithValue("@ResimYolu", resimYolu);
            cmd.Parameters.AddWithValue("@Link", eTicaretSlaytDto.Link);
            cmd.Parameters.AddWithValue("@Text", HttpUtility.HtmlEncode(eTicaretSlaytDto.Text));
            cmd.Parameters.AddWithValue("@Tip", eTicaretSlaytDto.Tip);
            cmd.Parameters.AddWithValue("@Aktif", eTicaretSlaytDto.Aktif);
            cmd.Parameters.AddWithValue("@OlusturulmaTarihi", eTicaretSlaytDto.OlusturulmaTarihi);
            cmd.Parameters.AddWithValue("@Siralama", eTicaretSlaytDto.Siralama);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Slaytlar");
        }

        [HttpGet]
        public ActionResult SlaytDuzenle(ETicaretSlaytDto eTicaretSlaytDto)
        {
            SlaytTipGetir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM ETicaret_Slaytlar WHERE Aktif = 1 AND SlaytID = @SlaytID");
            cmd.Parameters.AddWithValue("@SlaytID", eTicaretSlaytDto.SlaytID);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count == 0)
            {
                return RedirectToAction("SayfaBulunamadi");
            }

            var entity = new ETicaretSlaytDto();
            entity.SlaytID = Convert.ToString(dt.Rows[0]["SlaytID"]);
            entity.ResimYolu = Convert.ToString(dt.Rows[0]["ResimYolu"]);
            entity.Link = Convert.ToString(dt.Rows[0]["Link"]);
            entity.Text = Convert.ToString(dt.Rows[0]["Text"]);
            entity.Tip = Convert.ToString(dt.Rows[0]["Tip"]);
            entity.Aktif = Convert.ToBoolean(dt.Rows[0]["Aktif"]);
            entity.Siralama = Convert.ToInt32(dt.Rows[0]["Siralama"]);

            ViewBag.Tipler = new Dictionary<string, string>()
            {
                { "anasayfa-ust", "Anasayfa Üst" },
            };

            return View(entity);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SlaytDuzenle(ETicaretSlaytDto eTicaretSlaytDto, HttpPostedFileBase Dosya)
        {
            SlaytTipGetir();
            var resimYolu = "";
            if (Dosya != null && Dosya.ContentLength > 0)
            {
                var extension = Dosya.FileName.Split('.').Last();

                var fileName = Guid.NewGuid().ToString() + "." + extension;
                var filePath = Path.Combine(Server.MapPath("/Uploads/ETicaret"), fileName);
                Dosya.SaveAs(filePath);
                resimYolu = "/Uploads/ETicaret/" + fileName;
            }

            SqlCommand cmd;
            var resimYoluQuery = string.IsNullOrWhiteSpace(resimYolu) ? "" : "ResimYolu = @ResimYolu,"; 
            var sql = $"UPDATE ETicaret_Slaytlar SET {resimYoluQuery} Link = @Link, Text = @Text,Tip = @Tip ,Aktif = @Aktif, Siralama = @Siralama, GuncellenmeTarihi = @GuncellenmeTarihi WHERE SlaytID = @SlaytID";

            // Mevcut slaytı güncelleme
            cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@SlaytID", eTicaretSlaytDto.SlaytID);
            if (!string.IsNullOrWhiteSpace(resimYolu))
            {
                cmd.Parameters.AddWithValue("@ResimYolu", resimYolu);
            }
            cmd.Parameters.AddWithValue("@Link", eTicaretSlaytDto.Link);
            cmd.Parameters.AddWithValue("@Text", HttpUtility.HtmlEncode(eTicaretSlaytDto.Text));
            cmd.Parameters.AddWithValue("@Tip", eTicaretSlaytDto.Tip);
            cmd.Parameters.AddWithValue("@Aktif", eTicaretSlaytDto.Aktif);
            cmd.Parameters.AddWithValue("@Siralama", eTicaretSlaytDto.Siralama);
            cmd.Parameters.AddWithValue("@GuncellenmeTarihi", DateTime.Now.ToString());

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Slaytlar");
        }

     
   
        protected bool SlaytSil(string SlaytID)
        {
            try
            {
                // Silme sorgusu
                SqlCommand cmd = new SqlCommand("UPDATE ETicaret_Slaytlar SET Silindi = 1 WHERE SlaytID = @SlaytID");
                cmd.Parameters.AddWithValue("@SlaytID", SlaytID);
     
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                return true;
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }


        [HttpPost]
        public JsonResult SlaytSilJson(string SlaytID)
        {
            if (string.IsNullOrWhiteSpace(SlaytID))
            {
                return Json(new { success = false, message = "SlaytID geçersiz." });
            }

            bool silindi = SlaytSil(SlaytID);
            if (silindi)
            {
                return Json(new { success = true, message = "Slayt başarıyla silindi." });
            }
            else
            {
                return Json(new { success = false, message = "Slayt silinemedi." });
            }
        }

        



    }
}