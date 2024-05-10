using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;

namespace YKPortal.Controllers
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
            SonAktiviteler();
            return View();
        }
        private void SonAktiviteler()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_SonHareketler";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var sonAktiviteler = new List<SonAktivitelerDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sonAktiviteler.Add(new SonAktivitelerDto
                {
                    Tarih = Convert.ToString(dt.Rows[i]["Tarih"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Tarih"]),
                    Modul = Convert.ToString(dt.Rows[i]["Modul"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Modul"]),
                    Aciklama1 = Convert.ToString(dt.Rows[i]["Aciklama1"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Aciklama1"]),
                    Aciklama2 = Convert.ToString(dt.Rows[i]["Aciklama2"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Aciklama2"]),
                    Kullanici = Convert.ToString(dt.Rows[i]["Kullanici"]) == null ? "Kullanıcı Bulunamadı" : Convert.ToString(dt.Rows[i]["Kullanici"]),
                });
            }

            ViewBag.SonAktiviteler = sonAktiviteler;
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

                    CreateCookie("Isim", Convert.ToString(dt.Rows[0]["Ad"]) + " " + Convert.ToString(dt.Rows[0]["Soyad"]));
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


        //[HttpGet]
        // public ActionResult MailGonder()
        // {
        // SqlCommand cmd = new SqlCommand();
        //  cmd.CommandText = "p_KullaniciAdiBul";
        //  cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //  cmd.Parameters.AddWithValue("@KullaniciAdi", "");
        //  DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
        //       return View();
        // }

        // [HttpPost]
        // public ActionResult MailGonder(string kullaniciid)
        // {
        // SqlCommand cmd = new SqlCommand();
        // cmd.CommandText = "p_KullaniciAdiBul";
        // cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciid);
        //DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
        // SmtpClient sc = new SmtpClient();
        // sc.Port = 587;
        // sc.Host = "mail.ykyazilim.com.tr";
        //  sc.EnableSsl = false;
        //  sc.Credentials = new NetworkCredential("yunus@ykyazilim.com.tr", "parola");
        //  MailMessage mail = new MailMessage();
        //  mail.From = new MailAddress("yunus@ykyazilmi.com.tr", "YK YAZILIM");

        //mail.To.Add("gonderilecekmailadresi");

        //mail.Subject = "YK YAZILIM - Parola Sıfırlama";
        //mail.IsBodyHtml = true;
        //mail.Body = "içerik burada html kodu yazabiliriz ve paroal bilgisinide ekliycez";

        //mail.Attachments.Add(new Attachment(@"C:\Sonuc.pptx"));

        //sc.Send(mail);
        //    return View();
        // }
        //[HttpPost]
        //public ActionResult SifremiUnuttum(string kullaniciId)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandText = "p_KullaniciAdiBul";
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciId);

        //    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
        //    if (dt.Rows.Count == 0)
        //    {
        //        ViewBag.Bilgi = "Kullanıcı Bulunamadı";
        //        return View();

        //    }

        //    ViewBag.Bilgi = "Şifre bilgileriniz mail adresinize gönderildi.";
        //    return View();
        //}
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
            IlListesiniOlustur();

            return View();

        }



        [HttpPost]
        public ActionResult UyeOl(UyelikDto uyelikDto)
        {

            IlListesiniOlustur();

            if (!ModelState.IsValid)
            {
                ViewBag.Form = uyelikDto;
                return View();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@Isim", uyelikDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", uyelikDto.Unvan);
            cmd.Parameters.AddWithValue("@VergiNumarasi", uyelikDto.VergiNumarasi);
            cmd.Parameters.AddWithValue("@VergiDairesi", uyelikDto.VergiDairesi);
            cmd.Parameters.AddWithValue("@Adres", uyelikDto.Adres);
            cmd.Parameters.AddWithValue("@Il", uyelikDto.Il);
            cmd.Parameters.AddWithValue("@Ilce", uyelikDto.Ilce);
            cmd.Parameters.AddWithValue("@EMail", uyelikDto.EMail);
            cmd.Parameters.AddWithValue("@Ad", uyelikDto.Ad);
            cmd.Parameters.AddWithValue("@Soyad", uyelikDto.Soyad);
            cmd.Parameters.AddWithValue("@Parola", uyelikDto.Parola);
            cmd.Parameters.AddWithValue("@Iletisim", uyelikDto.Iletisim);
            cmd.Parameters.AddWithValue("@Kullanici", "");
            cmd.Parameters.AddWithValue("@UyelikBaslangicTarihi", DateTime.Now);
            cmd.Parameters.AddWithValue("@UyelikBitisTarihi", DateTime.Now.AddMonths(1));
            cmd.Parameters.AddWithValue("@ApiUrl", "http://api.ykyazilim.com.tr/api/");

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

        public ActionResult Liste(string UyelikID)
        {
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "p_SonHareketler";
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dt2 = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
            return View();
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

        public void IlListesiniOlustur()
        {
            // Il Listesi oluşturma
            SqlCommand ilCommand = new SqlCommand();
            ilCommand.CommandText = "p_GrupKoduListesi";
            ilCommand.CommandType = System.Data.CommandType.StoredProcedure;
            ilCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            ilCommand.Parameters.AddWithValue("@Kod", "Il");
            ilCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable ilDataTable = (DataTable)IDVeritabani.Sorgula(ilCommand, SorgulaTuru.Tablo);

            var iller = new List<string>();

            for (int i = 0; i < ilDataTable.Rows.Count; i++)
            {
                iller.Add(Convert.ToString(ilDataTable.Rows[i]["Deger"]));
            }
            ViewBag.Iller = iller;
        }
    }
}