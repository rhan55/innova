using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.Depo.Controllers
{
    public class DepoController : Controller
    {
        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"select * from MenulerUretim WITH(NOLOCK)
INNER JOIN Yetkiler WITH(NOLOCK) ON Yetkiler.MenuID = MenulerUretim.ID
and Yetkiler.Gor = 1
and Yetkiler.KullaniciID = @KullaniciID
Where Aktif = 1 and UstID IS NULL Order by Sira";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            ViewBag.dtMenuler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }
        public ActionResult Menuler(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"select * from MenulerUretim WITH(NOLOCK)
INNER JOIN Yetkiler WITH(NOLOCK) ON Yetkiler.MenuID = MenulerUretim.ID
and Yetkiler.Gor = 1
and Yetkiler.KullaniciID = @KullaniciID 
Where Aktif = 1 and UstID = @UstID Order by Sira";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UstID", id);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            ViewBag.dtMenuler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }


        public ActionResult UretimIsEmirleri(string Operasyon, string aranacakKelime)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UretimIsEmriListesi";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Operasyon", Operasyon);
            cmd.Parameters.AddWithValue("@aranacakKelime", aranacakKelime);
            ViewBag.dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            ViewBag.Operasyon = Operasyon;
            ViewBag.aranacakKelime = aranacakKelime;

            return View();
        }

        public ActionResult UretimIsEmriDetay(string IsEmriNo)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UretimIsEmriDetay";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@IsEmriNo", IsEmriNo);
            ViewBag.dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }

        public ActionResult UretimIsEmriDurumuDegistir(string IsEmriNo, string Durumu)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UretimIsEmriAkisIslem";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@IsEmriNo", IsEmriNo);
            if (Durumu == "B")
                cmd.Parameters.AddWithValue("@Islem", "BASLA");
            else if (Durumu == "T")
                cmd.Parameters.AddWithValue("@Islem", "BITIR");
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            if (Durumu == "T")
                return Redirect("~/Depo/Depo/AnaSayfa");
            else
                return Redirect("~/Depo/Depo/UretimIsEmriDetay/?IsEmriNo=" + IsEmriNo);
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