using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;
using System.Web.Http.Results;
using System.Net.Http;

namespace YKPortal.Controllers
{
    public class DosyaController : Controller
    {

        public ActionResult Liste(DosyaDto cariDosyaDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DosyaListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.KayitID);

            ViewBag.Modul = cariDosyaDto.Modul;
            ViewBag.KayitID = cariDosyaDto.KayitID;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }


        [HttpPost]
        public HttpResponseMessage Ekle(DosyaDto cariDosyaDto, HttpPostedFileBase Dosya)
        {
            HttpResponseMessage response;

            if (Dosya == null && Dosya.ContentLength <= 0)
            {
                response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                response.Content = new StringContent("Dosya bulunamadi.");

                return response;
            }


            if (!AutoGirisKontrol())
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

            var id = Guid.NewGuid();

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "p_DosyaKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.KayitID);

            var dosyaUzantisi = Dosya.FileName.Split('.').Last();

            var dosyaAdi = $"{id}.{dosyaUzantisi}";
            Dosya.SaveAs(Server.MapPath($"/Uploads/Dosyalar/{dosyaAdi}"));

            cmd.Parameters.AddWithValue("@Dosya", Dosya.FileName);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent("Dosya bulunamadi.");
            return response;

        }

        public ActionResult Duzenle(DosyaDto cariDosyaDto, string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Dosya";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.@KayitID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult Duzenle(DosyaDto cariDosyaDto, HttpPostedFileBase Dosya)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DosyaKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;



            if (Dosya != null && Dosya.ContentLength > 0)
            {
                var extension = Dosya.FileName.Split('.').Last();

                var imageName = $"{cariDosyaDto.ID}_{Guid.NewGuid()}.{extension}";
                Dosya.SaveAs(Server.MapPath($"/Uploads/Avatarlar/{imageName}"));
                cmd.Parameters.AddWithValue("@Dosya", $"/Uploads/Avatarlar/{imageName}");
            }


            cmd.Parameters.AddWithValue("@ID", cariDosyaDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.@KayitID);
            cmd.Parameters.AddWithValue("@Dosya", cariDosyaDto.Dosya);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }


        [HttpPost]
        public ActionResult Sil(DosyaDto cariDosyaDto, string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.@KayitID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }

        #region Cookie İşlemleri

        public bool AutoGirisKontrol()
        {
            bool GirisKontrol = false;

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string Parola = GetCookie("Parola");

            if (KullaniciAdi != null)
            {

                ViewBag.KullaniciAdi = KullaniciAdi;

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_KullaniciGirisi";
                cmd.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
                cmd.Parameters.AddWithValue("@Parola", Parola);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (!Bilgi.StartsWith("UYARI!"))
                    {

                        GirisKontrol = true;
                    }
                    else
                    {
                        GirisKontrol = false;
                    }
                }
                else
                {
                    GirisKontrol = false;
                }
            }

            return GirisKontrol;
        }



        private string GetCookie(string name)
        {
            //Böyle bir cookie mevcut mu kontrol ediyoruz
            if (Request.Cookies.AllKeys.Contains(name))
            {
                //böyle bir cookie varsa bize geri değeri döndürsün
                return Server.UrlDecode(Request.Cookies[name].Value);
            }
            return null;
        }

        private class KullaniciListesi
        {

        }


        #endregion
    }
}