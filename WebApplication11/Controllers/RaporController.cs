using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;

namespace YKPortal.Controllers
{
    public class RaporController : Controller
    {

        // GET: Rapor
        public ActionResult RaporIsimleri()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_RaporIsimleri";
            cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }


        public ActionResult SebatIsEmriRaporu(DateTime? Baslangic= null, DateTime? Bitis=null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");
            if (Baslangic == null)
            {
                Baslangic = DateTime.Today.AddDays(-30);
            }
            if (Bitis == null)
            {
                Bitis = DateTime.Today;
            }
            ViewBag.Baslangic = Baslangic;
            ViewBag.Bitis = Bitis;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "EXEC [p_UretimIsEmirleri] '66DB319A-EA5C-41AE-A9CA-387C166CD074', 3, '"+ Convert.ToDateTime(Baslangic).ToString("yyyy-MM-dd")+ "', '"+ Convert.ToDateTime(Bitis).ToString("yyyy-MM-dd") + "'";
            //cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
            //cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
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
                        DeleteCookie("UyelikBitisTarihi");
                        CreateCookie("UyelikBitisTarihi", Convert.ToString(dt.Rows[0]["UyelikBitisTarihi"]));

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

        #endregion
    }
}