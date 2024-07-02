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
    public class ParametreController : Controller
    {
        // GET: Parametreler
        public ActionResult MailAyarlari()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var parametreler = ParametreleriGetir();

            var parametreListesi = new List<ParametreDto>();

            foreach (var item in parametreler)
            {
                if (item.Modul == "EMail")
                {
                    parametreListesi.Add(item);
                }
            }

            ViewBag.Parametreler = parametreListesi;

            return View();
        }
        [HttpPost]
        public ActionResult MailAyarlari(MailAyarlariDto mailAyarlari)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ParametreKaydet(new ParametreDto { Modul = "EMail", Deger = mailAyarlari.Host, Isim = "Host" });
            ParametreKaydet(new ParametreDto { Modul = "EMail", Deger = mailAyarlari.Isim, Isim = "Isim" });
            ParametreKaydet(new ParametreDto { Modul = "EMail", Deger = mailAyarlari.Parola, Isim = "Parola" });
            ParametreKaydet(new ParametreDto { Modul = "EMail", Deger = mailAyarlari.KullaniciAdi, Isim = "KullaniciAdi" });
            ParametreKaydet(new ParametreDto { Modul = "EMail", Deger = mailAyarlari.Port, Isim = "Port" });
            ParametreKaydet(new ParametreDto { Modul = "EMail", Deger = mailAyarlari.SSL ? "1" : "0", Isim = "SSL" });

            var parametreler = ParametreleriGetir();

            var parametreListesi = new List<ParametreDto>();

            foreach (var item in parametreler)
            {
                if (item.Modul == "EMail")
                {
                    parametreListesi.Add(item);
                }
            }

            ViewBag.Parametreler = parametreListesi;

            return View(); 
        }


        private List<ParametreDto> ParametreleriGetir()
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametreler";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var entities = new List<ParametreDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ParametreDto entity = new ParametreDto();
                entity.ID = Convert.ToString(dt.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(dt.Rows[i]["Deger"]);
                entity.Isim = Convert.ToString(dt.Rows[i]["Isim"]);
                entity.Modul = Convert.ToString(dt.Rows[i]["Modul"]);
                entity.Tip = Convert.ToString(dt.Rows[i]["Tip"]);
                entity.Kategori = Convert.ToString(dt.Rows[i]["Kategori"]);
                entities.Add(entity);
            }

            return entities;
        }

        [HttpGet]
        public ActionResult ParametreListesi()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametreler";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var entities = new List<ParametreDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ParametreDto entity = new ParametreDto();
                entity.ID = Convert.ToString(dt.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(dt.Rows[i]["Deger"]);
                entity.Isim = Convert.ToString(dt.Rows[i]["Isim"]);
                entity.Modul = Convert.ToString(dt.Rows[i]["Modul"]);
                entity.Tip = Convert.ToString(dt.Rows[i]["Tip"]);
                entity.StandartID = Convert.ToString(dt.Rows[i]["StandartID"]);
                entity.Kategori = Convert.ToString(dt.Rows[i]["Kategori"]);
                entities.Add(entity);
            }

            return View(entities);
        }

        [HttpPost]
        public JsonResult ParametreyiKaydet(string Modul, string Isim, string Deger)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ParametreKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", Modul);
            cmd.Parameters.AddWithValue("@Deger", Deger);
            cmd.Parameters.AddWithValue("@Isim", Isim);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        private ActionResult ParametreKaydet(ParametreDto parametre)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ParametreKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", parametre.Modul);
            cmd.Parameters.AddWithValue("@Deger", parametre.Deger);
            cmd.Parameters.AddWithValue("@Isim", parametre.Isim);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Json("OK", JsonRequestBehavior.AllowGet);
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
    }
}