using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YKPortal.Areas.Satinalma.Controllers;
using YKPortal.Models;
using YKPortal.Models.Dto;
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.Crm2.Controllers
{
    public class Crm2Controller : Controller
    {
        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            return View();
        }

        public ActionResult AnaSayfa2(string Menu="")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.Menu = Menu;

            return View();
        }
        public ActionResult YeniCari()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            return View();
        }
        public ActionResult Cariler()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            return View();
        }
        public ActionResult Tanimlamalar()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            return View();
        }



        [HttpGet]
        public ActionResult GrupKodu(string grupKodu, string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cmd.Parameters.AddWithValue("@Kod", grupKodu);
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.GrupKodu = grupKodu;
            ViewBag.aranacakKelime = aranacakKelime;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var model = new GrupkoduListeViewModel
            {
                GrupKodlari = dt,
                Sil = true,
                Duzenle = true

            };

            return View(model);

        }
        [HttpGet]
        public ActionResult GrupKoduEkle(string grupKodu)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            ViewBag.GrupKodu = grupKodu;

            return View();
        }

        [HttpPost]
        public ActionResult GrupKoduEkle(GrupKoduDto grupKoduDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu", new { Area = "Crm2", grupKodu = grupKoduDto.Kod });
        }
        [HttpGet]
        public ActionResult GrupKoduDuzenle(string grupKodu, string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKodu";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.GrupKodu = grupKodu;
            return View(dt);
        }

        [HttpPost]
        public ActionResult GrupKoduDuzenle(GrupKoduDto grupKoduDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", grupKoduDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", grupKoduDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return RedirectToAction("GrupKodu", new { Area = "Crm2", grupKodu = grupKoduDto.Kod });
        }

        [HttpPost]
        public ActionResult GrupKoduSil(string id, string grupKodu)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu", new { Area = "Crm2", grupKodu = grupKodu });
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
                    GirisKontrol = true;
                }
                else
                {
                    GirisKontrol = false;
                }
            }

            return GirisKontrol;
        }

        private void CreateCookie(string name, string value)
        {
            HttpCookie cookieVisitor = new HttpCookie(name, Server.UrlEncode(value));
            // cookieVisitor.Expires = DateTime.Now.AddDays(2);
            Response.Cookies.Add(cookieVisitor);
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
        private void DeleteCookie(string name)
        {
            //Böyle bir cookie var mı kontrol ediyoruz
            if (GetCookie(name) != null)
            {
                //Varsa cookiemizi temizliyoruz
                Response.Cookies.Remove(name);
                //ya da 
                Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);
            }
        }

        #endregion
    }

}