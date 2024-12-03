using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;
using System.IO;
using System.Net;
using System.Text;

namespace YKPortal.Areas.E.Controllers
{
    public class BilgilerController : BaseController
    {
        public ActionResult GenelBilgiler()
        {
            var cmd = new SqlCommand("p_GrupKoduListesi");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", System.Configuration.ConfigurationManager.AppSettings["UyelikID"]);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            // Veriyi modele dönüştür:
            var bilgiler = dt.AsEnumerable().Select(row => new GrupKoduDto
            {
                ID = Convert.ToString(row["ID"]),
                UyelikID = System.Configuration.ConfigurationManager.AppSettings["UyelikID"],
                Kod = row["Kod"].ToString(),
                Deger = row["Deger"].ToString(),
                Aktif = Convert.ToBoolean(row["Aktif"])
            }).ToList();

            return View(bilgiler);
        }

        [HttpPost]
        public ActionResult Kaydet(FormCollection form)
        {
            int uyelikID = int.Parse(GetCookie("UyelikID"));

            var bilgiler = new Dictionary<string, string>
            {
                { "ETicaretTelefon", form["ETicaretTelefon"] },
                { "ETicaretEmail", form["ETicaretEmail"] },
                { "ETicaretLogo", form["ETicaretLogo"] },
                { "ETicaretFirmaAdi", form["ETicaretFirmaAdi"] }
            };

            foreach (var bilgi in bilgiler)
            {
                var cmd = new SqlCommand("p_GrupKoduEkleVeyaGuncelle");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", uyelikID);
                cmd.Parameters.AddWithValue("@GrupKodu", bilgi.Key);
                cmd.Parameters.AddWithValue("@Deger", bilgi.Value);

                IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return RedirectToAction("GenelBilgiler");
        }


        [HttpPost]
        public ActionResult GenelBilgiler(GrupKoduDto grupKoduDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("E/Yetkilendirme/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Eğer ID varsa düzenleme, yoksa ekleme
            if (!string.IsNullOrEmpty(grupKoduDto.ID))
            {
                cmd.Parameters.AddWithValue("@ID", grupKoduDto.ID); // ID dolu ise düzenleme
            }
            else
            {
                cmd.Parameters.AddWithValue("@ID", DBNull.Value); // ID yoksa yeni kayıt
            }

            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu", new { grupKodu = grupKoduDto.Kod });
        }
    }
}