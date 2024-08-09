using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using YKPortal.Models;
using YKPortal.Models.Dto;

namespace YKPortal.Controllers
{
    public class MailKaliplariController : Controller
    {
        public ActionResult Liste(string depo, string aranacakKelime = "")
        {
 
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_MailKalibiListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);
            ViewBag.aranacakKelime = aranacakKelime;
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
        public ActionResult Ekle(MailKalibiDto emailTemplateDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_MailKalibiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", emailTemplateDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", emailTemplateDto.Isim);
            cmd.Parameters.AddWithValue("@Icerik", HttpUtility.HtmlEncode(emailTemplateDto.Icerik));


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }

        public ActionResult Sil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_MailKalibiSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }
        // GET: EmailTemplate
        public ActionResult Index(MailKalibiDto emailTemplateDto)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_MailKalibiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", emailTemplateDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", emailTemplateDto.Isim);
            cmd.Parameters.AddWithValue("@Icerik", emailTemplateDto.Icerik);
            
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var tema = System.IO.File.ReadAllText(Path.GetPathRoot("MailTemalari/KabulMaili.html"));
            tema.Replace("[Kod]", emailTemplateDto.Kod);
            tema.Replace("[Isim]", emailTemplateDto.Isim);
            tema.Replace("[Icerik]", emailTemplateDto.Icerik);

            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "mail.ykyazilim.com.tr";
            sc.EnableSsl = false;
            sc.Credentials = new NetworkCredential("ilayda@ykyazilim.com.tr", "Ilayda12#");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ilayda@ykyazilim.com.tr", ConfigurationManager.AppSettings["FirmaAdi"]);

            mail.To.Add("muharremkackin@gmail.com");

            mail.Subject = ConfigurationManager.AppSettings["FirmaAdi"] + " - Parola Sıfırlama";
            mail.IsBodyHtml = true;
            mail.Body = tema;

            return View();
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
        private bool YetkiKontrolu(string YetkiUrl, string Tip = "Gor")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciYetkileri";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            List<YetkilerDto> yetkiler = new List<YetkilerDto>();

            foreach (DataRow row in dt.Rows)
            {
                yetkiler.Add(new YetkilerDto()
                {
                    MenuID = Convert.ToString(row["MenuID"]),
                    KullaniciID = Convert.ToString(row["KullaniciID"]),
                    UyelikID = Convert.ToString(row["UyelikID"]),
                    Menu = Convert.ToString(row["Menu"]),
                    UstID = Convert.ToString(row["UstID"]),
                    Gor = Convert.ToBoolean(row["Gor"]),
                    Duzenle = Convert.ToBoolean(row["Duzenle"]),
                    Sil = Convert.ToBoolean(row["Sil"]),
                    url = Convert.ToString(row["url"]),
                });
            }
            var yetki = yetkiler.Where(m => m.url == YetkiUrl).FirstOrDefault();
            if (yetki != null)
            {
                if (Tip == "Gor")
                {
                    return yetki.Gor;
                }
                else if (Tip == "Duzenle")
                {
                    return yetki.Duzenle;
                }
                else if (Tip == "Sil")
                {
                    return yetki.Sil;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}