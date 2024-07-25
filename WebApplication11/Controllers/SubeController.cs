using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;

namespace YKPortal.Controllers
{
    public class SubeController : Controller
    {
        // GET: Sube
        public ActionResult Liste(string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_SubeListesi";
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
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            return View();
        }

        [HttpPost]
        public ActionResult Ekle(SubeDto subeDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_SubeKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
           
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", subeDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", subeDto.Isim);
            cmd.Parameters.AddWithValue("@Adres", subeDto.Adres);
            cmd.Parameters.AddWithValue("@Telefon", subeDto.Telefon);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("Liste");
        }

        [HttpGet]
        public ActionResult Duzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
       
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Sube";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }
        [HttpPost]
        public ActionResult Duzenle(SubeDto subeDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
          
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_SubeKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID",subeDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", subeDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", subeDto.Isim);
            cmd.Parameters.AddWithValue("@Adres", subeDto.Adres);
            cmd.Parameters.AddWithValue("@Telefon", subeDto.Telefon);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
    
            return RedirectToAction("Liste");
        }
        public ActionResult Sil(string id, SubeDto subeDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_SubeSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure; 
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
     
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.ID = subeDto.ID;

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

