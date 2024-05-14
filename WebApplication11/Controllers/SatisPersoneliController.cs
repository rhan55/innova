using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;
using System.Net;

namespace YKPortal.Controllers
{
    public class SatisPersoneliController : Controller
    {
        [HttpGet]
        public ActionResult Ekle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            return View();
        }

        [HttpPost]
        public ActionResult Ekle(SatisPersoneliDto satisPersoneliDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_PlasiyerKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Isim", satisPersoneliDto.Isim);
            cmd.Parameters.AddWithValue("@Aciklama1", satisPersoneliDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", satisPersoneliDto.Aciklama2);
            

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View(dt);
        }

        [HttpGet]
        public ActionResult Liste(SatisPersoneliDto satisPersoneliDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_PlasiyerListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View(dt);
        }

        [HttpGet]
        public ActionResult Duzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Plasiyer";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult Duzenle(SatisPersoneliDto satisPersoneliDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_PlasiyerKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            var uyelikId = GetCookie("UyelikID");
            cmd.Parameters.AddWithValue("@ID" ,satisPersoneliDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Isim", satisPersoneliDto.Isim);
            cmd.Parameters.AddWithValue("@Aciklama1", satisPersoneliDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", satisPersoneliDto.Aciklama2);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);      
            return RedirectToAction("Liste");

        }

        public ActionResult Sil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_PlasiyerSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
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

        #endregion

    }
}