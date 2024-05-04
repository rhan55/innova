using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;
using WebApplication11.Models.Dto;

namespace WebApplication11.Controllers
{
    public class KullaniciController : Controller
    {

        // GET: Kullanici
        public ActionResult Liste(string aranacakKelime = "")
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.AranacakKelime = aranacakKelime;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpGet]
        public ActionResult Ekle()

        {
            return View();
        }

        [HttpPost]

        public ActionResult Ekle(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {

            string imageName = "/Tema/Media/Logolar/amblem.png";
            var id = Guid.NewGuid();
            if (Resim != null && Resim.ContentLength > 0)
            {
                var extension = Resim.FileName.Split('.').Last();

                imageName = $"/Uploads/Avatarlar/{id}_{Guid.NewGuid()}.{extension}";
                Resim.SaveAs(Server.MapPath($"{imageName}"));
            }

            kullaniciEkleDto.UyelikID = GetCookie("UyelikID");

            kullaniciEkleDto.Kullanici = GetCookie("KullaniciID");

            if (kullaniciEkleDto.Aciklama1 == null)
            {
                kullaniciEkleDto.Aciklama1 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama2 == null)
            {
                kullaniciEkleDto.Aciklama2 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama3 == null)
            {
                kullaniciEkleDto.Aciklama3 = string.Empty;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Resim", $"{imageName}");
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", kullaniciEkleDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciEkleDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", kullaniciEkleDto.Parola);
            cmd.Parameters.AddWithValue("@Ad", kullaniciEkleDto.Ad);
            cmd.Parameters.AddWithValue("@Soyad", kullaniciEkleDto.Soyad);
            cmd.Parameters.AddWithValue("@Aktif", kullaniciEkleDto.Aktif);
            cmd.Parameters.AddWithValue("@Telefon", kullaniciEkleDto.Telefon);
            cmd.Parameters.AddWithValue("@Adres", kullaniciEkleDto.Adres);
            cmd.Parameters.AddWithValue("@Il", kullaniciEkleDto.Il);
            cmd.Parameters.AddWithValue("@Ilce", kullaniciEkleDto.Ilce);
            cmd.Parameters.AddWithValue("@Aciklama1", kullaniciEkleDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", kullaniciEkleDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", kullaniciEkleDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Kullanici", kullaniciEkleDto.Kullanici);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                string ID = Convert.ToString(dt.Rows[0]["ID"]);
                if (ID != "0")
                {
                    return Redirect("~/Kullanici/Liste");
                }
            }

            return View(dt);
        }

        public ActionResult Duzenle(string id)
        {
            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult Duzenle(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.ValidationError = ModelState.Values
                                                        .SelectMany(m => m.Errors)
                                                        .Select(m => m.ErrorMessage).ToList();
                return Duzenle(kullaniciEkleDto.ID);
            }


            kullaniciEkleDto.UyelikID = GetCookie("UyelikID");

            kullaniciEkleDto.Kullanici = GetCookie("KullaniciID");

            if (kullaniciEkleDto.Aciklama1 == null)
            {
                kullaniciEkleDto.Aciklama1 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama2 == null)
            {
                kullaniciEkleDto.Aciklama2 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama3 == null)
            {
                kullaniciEkleDto.Aciklama3 = string.Empty;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;



            if (Resim != null && Resim.ContentLength > 0)
            {
                var extension = Resim.FileName.Split('.').Last();

                var imageName = $"{kullaniciEkleDto.ID}_{Guid.NewGuid()}.{extension}";
                Resim.SaveAs(Server.MapPath($"/Uploads/Avatarlar/{imageName}"));
                cmd.Parameters.AddWithValue("@Resim", $"/Uploads/Avatarlar/{imageName}");
            }


            cmd.Parameters.AddWithValue("@ID", kullaniciEkleDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", kullaniciEkleDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciEkleDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", kullaniciEkleDto.Parola);
            cmd.Parameters.AddWithValue("@Ad", kullaniciEkleDto.Ad);
            cmd.Parameters.AddWithValue("@Soyad", kullaniciEkleDto.Soyad);
            cmd.Parameters.AddWithValue("@Aktif", kullaniciEkleDto.Aktif);
            cmd.Parameters.AddWithValue("@Telefon", kullaniciEkleDto.Telefon);
            cmd.Parameters.AddWithValue("@Adres", kullaniciEkleDto.Adres);
            cmd.Parameters.AddWithValue("@Il", kullaniciEkleDto.Il);
            cmd.Parameters.AddWithValue("@Ilce", kullaniciEkleDto.Ilce);
            cmd.Parameters.AddWithValue("@Aciklama1", kullaniciEkleDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", kullaniciEkleDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", kullaniciEkleDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Kullanici", kullaniciEkleDto.Kullanici);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            if (dt.Rows.Count > 0)
            {
                var columns = dt.Columns;
                if (columns.Contains("Bilgi"))
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (Bilgi == "Kullanıcı başarıyla oluşturuldu.")
                    {
                        return Redirect("~/Kullanici/Liste");
                    }
                    else
                    {
                        ViewBag.Bilgi = Bilgi;

                        var kullanici = Duzenle(Convert.ToString(dt.Rows[0]["ID"]));

                        return View(kullanici);
                    }
                }

            }

            return RedirectToAction("Liste");
        }


        public ActionResult Profil(string Mesaj = "")
        {
            var uyelikId = GetCookie("UyelikID");
            var kullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", kullaniciID);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.Mesaj = Mesaj;
            return View(dt);
        }

        [HttpPost]
        public ActionResult Profil(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.ValidationError = ModelState.Values
                                                        .SelectMany(m => m.Errors)
                                                        .Select(m => m.ErrorMessage).ToList();
                return Profil();
            }

            kullaniciEkleDto.UyelikID = GetCookie("UyelikID");

            kullaniciEkleDto.Kullanici = GetCookie("KullaniciID");

            if (kullaniciEkleDto.Aciklama1 == null)
            {
                kullaniciEkleDto.Aciklama1 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama2 == null)
            {
                kullaniciEkleDto.Aciklama2 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama3 == null)
            {
                kullaniciEkleDto.Aciklama3 = string.Empty;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (Resim != null && Resim.ContentLength > 0)
            {
                var extension = Resim.FileName.Split('.').Last();

                var imageName = $"{kullaniciEkleDto.ID}_{Guid.NewGuid()}.{extension}";
                Resim.SaveAs(Server.MapPath($"/Uploads/Avatarlar/{imageName}"));
                cmd.Parameters.AddWithValue("@Resim", $"/Uploads/Avatarlar/{imageName}");
            }

            cmd.Parameters.AddWithValue("@ID", kullaniciEkleDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", kullaniciEkleDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciEkleDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", kullaniciEkleDto.Parola);
            cmd.Parameters.AddWithValue("@Ad", kullaniciEkleDto.Ad);
            cmd.Parameters.AddWithValue("@Soyad", kullaniciEkleDto.Soyad);
            cmd.Parameters.AddWithValue("@Aktif", kullaniciEkleDto.Aktif);
            cmd.Parameters.AddWithValue("@Telefon", kullaniciEkleDto.Telefon);
            cmd.Parameters.AddWithValue("@Adres", kullaniciEkleDto.Adres);
            cmd.Parameters.AddWithValue("@Il", kullaniciEkleDto.Il);
            cmd.Parameters.AddWithValue("@Ilce", kullaniciEkleDto.Ilce);
            cmd.Parameters.AddWithValue("@Aciklama1", kullaniciEkleDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", kullaniciEkleDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", kullaniciEkleDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Kullanici", kullaniciEkleDto.Kullanici);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                var columns = dt.Columns;
                if (columns.Contains("Bilgi"))
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (!Bilgi.StartsWith("UYARI!"))
                    {
                        return RedirectToAction("Profil", new { Mesaj = "Profil başarıyla güncellendi." });
                    }
                    else
                    {
                        return RedirectToAction("Profil", new { Mesaj = Bilgi });
                    }
                }
            }

            return RedirectToAction("Profil");
        }

        private ActionResult Detay()
        {
            return View();
        }
        private ActionResult Detay(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View();
        }

        [HttpPost]
        public ActionResult Sil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }


        #region Cookie İşlemleri
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