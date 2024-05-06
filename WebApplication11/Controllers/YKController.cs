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
    public class YKController : Controller
    {
        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
            if (!Request.IsLocal && !Request.IsSecureConnection)
            {
                Response.Redirect(redirectUrl, false);
                HttpContext.ApplicationInstance.CompleteRequest();
            }



            return View();
        }

        #region Giriş İşlemleri 
        [HttpGet]
        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(string txtKullaniciAdi, string txtParola)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciGirisi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KullaniciAdi", txtKullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", txtParola);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                if (!Bilgi.StartsWith("UYARI!"))
                {
                    #region Cookie İşlemleri

                    CreateCookie("Isim", Convert.ToString(dt.Rows[0]["Ad"])+" "+ Convert.ToString(dt.Rows[0]["Soyad"]));
                    CreateCookie("KullaniciID", Convert.ToString(dt.Rows[0]["ID"]));
                    CreateCookie("UyelikIsim", Convert.ToString(dt.Rows[0]["UyelikIsim"]));
                    CreateCookie("UyelikID", Convert.ToString(dt.Rows[0]["UyelikID"]));
                    CreateCookie("KullaniciAdi", Convert.ToString(dt.Rows[0]["KullaniciAdi"]));
                    CreateCookie("Parola", Convert.ToString(dt.Rows[0]["Parola"]));
                    CreateCookie("Resim", Convert.ToString(dt.Rows[0]["Resim"]));

                    #endregion

                    return Redirect("~/YK/AnaSayfa");
                }
            }


            return View(dt);
        }
        [HttpGet]
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KullaniciOnayla(string kullaniciid)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciAktivasyonuYap";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", kullaniciid);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }

        public ActionResult Cikis()
        {
            DeleteCookie("Isim");
            DeleteCookie("KullaniciID");
            DeleteCookie("UyelikIsim");
            DeleteCookie("UyelikID");
            DeleteCookie("KullaniciAdi");
            DeleteCookie("Parola");
            DeleteCookie("Resim");

            return Redirect("~/YK/Giris");

        }

        #endregion

        #region Üyelik İşlemleri

        public ActionResult UyelikListesi(string aranacakKelime = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.AranacakKelime = aranacakKelime;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }
        [HttpGet]
        public ActionResult UyeOl()
        {
            return View();
          
        }
       


        [HttpPost]
        public ActionResult UyeOl(UyelikDto uyelikDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@Isim", uyelikDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", uyelikDto.Unvan);
            cmd.Parameters.AddWithValue("@VergiNumarasi", uyelikDto.VergiNumarasi);
            cmd.Parameters.AddWithValue("@VergiDairesi", uyelikDto.VergiDairesi);
            cmd.Parameters.AddWithValue("@Adres", uyelikDto.Adres);
            cmd.Parameters.AddWithValue("@EMail", uyelikDto.EMail);
            cmd.Parameters.AddWithValue("@Iletisim", uyelikDto.Iletisim);
            cmd.Parameters.AddWithValue("@Kullanici", "");
            cmd.Parameters.AddWithValue("@UyelikBaslangicTarihi", uyelikDto.UyelikBaslangicTarihi);
            cmd.Parameters.AddWithValue("@UyelikBitisTarihi", uyelikDto.UyelikBitisTarihi);
            cmd.Parameters.AddWithValue("@ApiUrl", uyelikDto.ApiUrl);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View(dt);
        }

        public ActionResult UyelikDuzenle(string id)
        {
            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Uyelik";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult UyelikDuzenle(            
            string id,
            string Isim,
            string Unvan,
            string VergiNumarasi,
            string VergiDairesi,
            string Adres,
            string EMail,
            string Iletisim,
            DateTime UyelikBaslangicTarihi,
            DateTime UyelikBitisTarihi,
            string ApiUrl)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Isim", Isim);
            cmd.Parameters.AddWithValue("@Unvan", Unvan);
            cmd.Parameters.AddWithValue("@VergiDairesi", VergiDairesi);
            cmd.Parameters.AddWithValue("@VergiNumarasi", VergiNumarasi);
            cmd.Parameters.AddWithValue("@Adres", Adres);
            cmd.Parameters.AddWithValue("@EMail", EMail);
            cmd.Parameters.AddWithValue("@Iletisim", Iletisim);
            cmd.Parameters.AddWithValue("@ApiUrl", ApiUrl);
            cmd.Parameters.AddWithValue("@UyelikBaslangicTarihi", UyelikBaslangicTarihi);
            cmd.Parameters.AddWithValue("@UyelikBitisTarihi", UyelikBitisTarihi);
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                var columns = dt.Columns;
                if (columns.Contains("Bilgi"))
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    ViewBag.Bilgi = Bilgi;
                }
            }


            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "p_Uyelik";
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@ID", id);
            DataTable dt2 = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);

            return View(dt2);

        }
        #endregion

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