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
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.B2BLogo.Controllers
{
    public class B2BLogoController : Controller
    {



        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            return View();
        }
        public ActionResult CariListesi(string AranacakKelime="")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_Cariler";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", AranacakKelime);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.AranacakKelime = AranacakKelime;



            return View(dtKayitlar);
        }
        public ActionResult CariSec(string CariKodu)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_Cari";
            cmd.Parameters.AddWithValue("@ID", CariKodu);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));            
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            if (dtKayitlar.Rows.Count > 0)
            {
                Session["B2BLogo_CariID"] = CariKodu;
                Session["B2BLogo_CariKodu"] = dtKayitlar.Rows[0]["Kod"];
                Session["B2BLogo_CariAdi"] = dtKayitlar.Rows[0]["Isim"];
            }
            return Redirect("~/B2BLogo/B2BLogo/AnaSayfa");
        }


        public ActionResult Siparislerim()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_Siparislerim";
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dtKayitlar);
        }
        public ActionResult SiparisDetay(string BelgeNo)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_SiparisDetay";
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@BelgeNo", BelgeNo);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dtKayitlar);
        }
        public ActionResult CariEkstre()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_CariHareketListesi";
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@BaslangicTarihi", "2000-01-01");
            cmd.Parameters.AddWithValue("@BitisTarihi", DateTime.Today.AddDays(1));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dtKayitlar);
        }

        public ActionResult YeniSiparis(string AranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_Stoklar";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@AranacakKelime", AranacakKelime);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.AranacakKelime = AranacakKelime;



            return View(dtKayitlar);
        }
        public JsonResult SepeteEkle(string StokKodu = "", decimal Miktar=0, decimal Fiyat=0)
        {
            YKJsonResult result = new YKJsonResult();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_SepetEkle";
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", "B2B");
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@StokID", StokKodu);
            cmd.Parameters.AddWithValue("@Seri", "");
            cmd.Parameters.AddWithValue("@Birim", "");
            cmd.Parameters.AddWithValue("@Miktar", Miktar);
            cmd.Parameters.AddWithValue("@Fiyat", Fiyat);
            cmd.Parameters.AddWithValue("@Tutar", Miktar * Fiyat);
            cmd.Parameters.AddWithValue("@IslemTipi", "0");
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            result.SonucKodu = "1";
            result.Aciklama = "";

            return Json(result, JsonRequestBehavior.AllowGet);
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