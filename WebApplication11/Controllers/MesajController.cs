using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;
using System.Web.Http.Results;
using Microsoft.AspNet.SignalR.Infrastructure;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace YKPortal.Controllers
{
    public class MesajController : Controller
    {


        [HttpGet]
        public ActionResult Chat(string kullaniciId = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            return View();
        }


        public ActionResult Chat(MesajlasmaDto mesajlasmaDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_MesajKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KarsiKullaniciID", mesajlasmaDto.KarsiKullaniciID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            if (string.IsNullOrEmpty(mesajlasmaDto.Mesaj))
            {
                cmd.Parameters.AddWithValue("@Mesaj", DBNull.Value); // NULL gönder
            }
            else
            {
                cmd.Parameters.AddWithValue("@Mesaj", mesajlasmaDto.Mesaj); // Mesajı gönder
            }

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek);

            ViewBag.KullaniciID = GetCookie("KullaniciID");
            return RedirectToAction("Chat");
        }


        [HttpPost]
        public ActionResult Sil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_MesajSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Chat");
        }

        public JsonResult Kaydet(MesajlasmaDto mesajlasma)
        {
            if (string.IsNullOrEmpty(mesajlasma.Mesaj) || string.IsNullOrEmpty(mesajlasma.KarsiKullaniciID))
            {
                return Json(new IDJsonResult { Sonuc = "Hata", SonucKodu = 422 });
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_MesajKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KarsiKullaniciID", mesajlasma.KarsiKullaniciID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Mesaj", mesajlasma.Mesaj);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek);

            return Json(new IDJsonResult { Sonuc = "Başarılı", SonucKodu = 200 });
        }


     
        public JsonResult MesajListesiGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_MesajListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var mesajListesi = new List<MesajlasmaDto>();

            for (int i = 0;i < dt.Rows.Count; i++) {
                mesajListesi.Add(new MesajlasmaDto
                {
                    ID = Guid.Parse(Convert.ToString(dt.Rows[i]["ID"])),
                    Mesaj = Convert.ToString(dt.Rows[i]["Mesaj"]),
                    KullaniciID = Convert.ToString(dt.Rows[i]["KullaniciID"]),
                    KarsiKullaniciID = Convert.ToString(dt.Rows[i]["KarsiKullaniciID"]),
                    Tarih = Convert.ToString(dt.Rows[i]["KayitTarihi"]),
                });
            }

            return Json(mesajListesi, JsonRequestBehavior.AllowGet);
        }


        public JsonResult KullaniciGetir(string aranacakKelime = "")
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var kullaniciListesi = new List<KullaniciEkleDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                kullaniciListesi.Add(new KullaniciEkleDto
                {
                    Ad = Convert.ToString(dt.Rows[i]["Ad"]),
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Soyad = Convert.ToString(dt.Rows[i]["Soyad"]),
                    KullaniciAdi = Convert.ToString(dt.Rows[i]["KullaniciAdi"]),
                });
            }

            return Json(kullaniciListesi, JsonRequestBehavior.AllowGet);
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
    }



}

         
         

           



        // GET: Mesaj


    