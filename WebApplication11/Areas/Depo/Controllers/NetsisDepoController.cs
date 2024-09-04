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
    public class NetsisDepoController : Controller
    {


        [HttpGet]
        public ActionResult NetsisStokEkbilgiDuzenle(string Belge_Barkod = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametre";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", GetCookie("NetsisDatabase"));
            DataTable dtNetsisDatatable = (DataTable)(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo));
            string NetsisDatatable = "";
            if (dtNetsisDatatable.Rows.Count > 0)
            {
                NetsisDatatable = Convert.ToString(dtNetsisDatatable.Rows[0]["Deger"]);
            }

            if (Belge_Barkod == "")
            {

                cmd.Parameters.Clear();
                string _srg = " SELECT STOK_KODU, STOK_ADI FROM "+ NetsisDatatable + "..TBLSTSABIT WITH (NOLOCK) WHERE STOK_KODU = '"+ Belge_Barkod + "' ";
                cmd.CommandText = _srg;
                cmd.CommandType = CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    ViewBag.dtDetay = dt;
                }

            }

            return View();
        }
        [HttpPost]
        public ActionResult NetsisStokEkBilgi_Guncelle(string Stok_Kodu, string EkBilgi_1, string EkBilgi_2, string EkBilgi_3)
        {
            
            string _srg = " UPDATE TBLSTSABITEK ";
            _srg += " \r\n SET KULL1S = '"+ EkBilgi_1  + "' ";
            _srg += " \r\n ,   KULL2S = '" + EkBilgi_2 + "' ";
            _srg += " \r\n ,   KULL3S = '" + EkBilgi_3 + "' ";
            _srg += " \r\n ,   KULL3S = '" + EkBilgi_3 + "' ";
            _srg += " \r\n WHERE STOK_KODU = '" + Stok_Kodu + "' ";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = _srg;
            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.dtDetay = dt;

            return Redirect("~/Depo/NetsisDepo/NetsisStokEkbilgiDuzenle/?Belge_Lokasyon_Barkod=" + "");


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