using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;

namespace WebApplication11.Controllers
{
    public class YKController : Controller
    {

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

       


        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");




            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public ActionResult Cikis()
        {
            DeleteCookie("Isim");
            DeleteCookie("KullaniciID");
            DeleteCookie("UyelikID");
            DeleteCookie("KullaniciAdi");
            DeleteCookie("Parola");

            return Redirect("~/YK/Giris");

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
    }
}