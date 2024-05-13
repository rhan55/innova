using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;

namespace YKPortal.Controllers
{
    public class CariKisiController : Controller
    {
        // GET: CariKisi
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Ekle(string CariID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.CariID = CariID;

            return View();
        }

        [HttpPost]
        public ActionResult Ekle(CariKisiDto cariKisiDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKisiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@CariID", cariKisiDto.CariID);
            cmd.Parameters.AddWithValue("@Isim", cariKisiDto.Isim);
            cmd.Parameters.AddWithValue("@Email", cariKisiDto.Email);
            cmd.Parameters.AddWithValue("@Gorev", cariKisiDto.Gorev);
            cmd.Parameters.AddWithValue("@Telefon", cariKisiDto.Telefon);
            cmd.Parameters.AddWithValue("@Aktif", cariKisiDto.Aktif);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("Liste", new { CariID = cariKisiDto.CariID });
        }

        public ActionResult Liste(CariKisiDto cariKisiDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKisiListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", cariKisiDto.CariID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.CariID = cariKisiDto.CariID;

            return View(dt);
        }

        [HttpGet]
        public ActionResult Duzenle(string ID, string CariID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKisi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.ID = ID;
            ViewBag.CariID = CariID;
            return View(dt);

        }
        [HttpPost]
        public ActionResult Duzenle(CariKisiDto cariKisiDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKisiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", cariKisiDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@CariID", cariKisiDto.CariID);
            cmd.Parameters.AddWithValue("@Isim", cariKisiDto.Isim);
            cmd.Parameters.AddWithValue("@Email", cariKisiDto.Email);
            cmd.Parameters.AddWithValue("@Gorev", cariKisiDto.Gorev);
            cmd.Parameters.AddWithValue("@Telefon", cariKisiDto.Telefon);
            cmd.Parameters.AddWithValue("@Aktif", cariKisiDto.Aktif);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste", new { CariID = cariKisiDto.CariID });

        }

        [HttpPost]
        public ActionResult Sil(string id, string CariID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKisiSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@CariID", CariID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("Liste", new { CariID = CariID });
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