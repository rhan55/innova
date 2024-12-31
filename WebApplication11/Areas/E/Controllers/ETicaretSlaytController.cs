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
           SqlCommand cmd = new SqlCommand("SELECT * FROM ETicaret_Slaytlar WHERE Aktif = 1");

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count == 0)
            {
                return RedirectToAction("SayfaBulunamadi");
            }
            var entities = new List<ETicaretSlaytDto>();
            foreach (DataRow row in dt.Rows)
            {
                var entity = new ETicaretSlaytDto();
                entity.SlaytID = Convert.ToString(row["SlaytID"]);
                entity.ResimYolu = Convert.ToString(row["ResimYolu"]);
                entity.Link = Convert.ToString(row["Link"]);
                entity.Text = Convert.ToString(row["Text"]);
                entity.Aktif = Convert.ToBoolean(row["Aktif"]);
                entity.Siralama = Convert.ToInt32(row["Aktif"]);

                entities.Add(entity);
            }
        

            return View(entities);
        }

        [HttpGet]
        public ActionResult SlaytEkle(string SlaytID)
        {
            var entity = new ETicaretSlaytDto();
            if (string.IsNullOrWhiteSpace(SlaytID))
            {
                return View(entity);
            }

            entity = string.IsNullOrWhiteSpace(SlaytID) ? new ETicaretSlaytDto() : SlaytGetir(SlaytID);
            return View(entity);
        }

        [HttpPost]
        public ActionResult SlaytEkle(ETicaretSlaytDto eTicaretSlaytDto, HttpPostedFileBase Dosya)
        {
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
                cmd = new SqlCommand("INSERT INTO ETicaret_Slaytlar (ResimYolu, Link, Text, Aktif, OlusturulmaTarihi, Siralama) VALUES (@ResimYolu, @Link, @Text, @Aktif, @OlusturulmaTarihi, @Siralama)");
            }
            else
            {
                // Mevcut slaytı güncelleme
                cmd = new SqlCommand("UPDATE ETicaret_Slaytlar SET ResimYolu = @ResimYolu, Link = @Link, Text = @Text, Aktif = @Aktif, Siralama = @Siralama WHERE SlaytID = @SlaytID");
                cmd.Parameters.AddWithValue("@SlaytID", eTicaretSlaytDto.SlaytID);
            }

            cmd.Parameters.AddWithValue("@ResimYolu", resimYolu);
            cmd.Parameters.AddWithValue("@Link", eTicaretSlaytDto.Link);
            cmd.Parameters.AddWithValue("@Text", eTicaretSlaytDto.Text);
            cmd.Parameters.AddWithValue("@Aktif", eTicaretSlaytDto.Aktif);
            cmd.Parameters.AddWithValue("@OlusturulmaTarihi", eTicaretSlaytDto.OlusturulmaTarihi);
            cmd.Parameters.AddWithValue("@Siralama", string.IsNullOrWhiteSpace(eTicaretSlaytDto.SlaytID) ? 0 : int.Parse(eTicaretSlaytDto.SlaytID));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Slaytlar");
        }



    }
}