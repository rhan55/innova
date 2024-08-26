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
using YKPortal.Models.Dto;
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.Crm2.Controllers
{
    public class Crm2Controller : Controller
    {
        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            return View();
        }

        public ActionResult AnaSayfa2(string Menu = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.Menu = Menu;

            return View();
        }

        [HttpGet]
        public ActionResult YeniCari()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            GenelBilgilerGetir();

            return View();
        }

        [HttpPost]
        public JsonResult YeniCari(string Bayi, DateTime Tarih, string ProjeTipi, string BlokSayisi,
            string Miktar, string Unvan, string Ad, string Soyad, string Telefon1, string Telefon2,
            string Gorev, string UlasimSekli, string Projeadi, string Il, string Ilce, string Mahalle,
            string Ada, string Parsel, string PortalNumarasi, string Resim, DateTime? SonucTarihi)
        {
            YKJsonResult result = new YKJsonResult();
            try
            {
                string SonID = Guid.NewGuid().ToString();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
                        Insert Into Crm2Kayitlar
                        (ID,UyelikID,AlanKullanici,VerenKullanici,KabulTarihi,Bayi,Tarih,ProjeTipi,BlokSayisi,Miktar,Unvan,Ad,Soyad,Telefon1,Telefon2,Gorev,UlasimSekli,Projeadi,Il,Ilce,Mahalle,Ada,Parsel,PortalNumarasi,Resim,SonucTarihi,KayitTarihi,KayitYapankullanici,Silindi)
                        values
                        (@ID,@UyelikID,@AlanKullanici,@VerenKullanici,@KabulTarihi,@Bayi,@Tarih,@ProjeTipi,@BlokSayisi,@Miktar,@Unvan,@Ad,@Soyad,@Telefon1,@Telefon2,@Gorev,@UlasimSekli,@Projeadi,@Il,@Ilce,@Mahalle,@Ada,@Parsel,@PortalNumarasi,@Resim,@SonucTarihi,GETDATE(),@KayitYapankullanici,0)
                        ";
                cmd.Parameters.AddWithValue("@ID", SonID);
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

                cmd.Parameters.AddWithValue("@AlanKullanici", GetCookie("KullaniciID"));
                cmd.Parameters.AddWithValue("@VerenKullanici", DBNull.Value);
                cmd.Parameters.AddWithValue("@KabulTarihi", DateTime.Today);

                cmd.Parameters.AddWithValue("@Bayi", Bayi);
                cmd.Parameters.AddWithValue("@Tarih", Tarih);
                cmd.Parameters.AddWithValue("@ProjeTipi", ProjeTipi);
                cmd.Parameters.AddWithValue("@BlokSayisi", BlokSayisi);
                cmd.Parameters.AddWithValue("@Miktar", Miktar);
                cmd.Parameters.AddWithValue("@Unvan", Unvan);
                cmd.Parameters.AddWithValue("@Ad", Ad);
                cmd.Parameters.AddWithValue("@Soyad", Soyad);
                cmd.Parameters.AddWithValue("@Telefon1", Telefon1);
                cmd.Parameters.AddWithValue("@Telefon2", Telefon2);
                cmd.Parameters.AddWithValue("@Gorev", Gorev);
                cmd.Parameters.AddWithValue("@UlasimSekli", UlasimSekli);
                cmd.Parameters.AddWithValue("@Projeadi", Projeadi);
                cmd.Parameters.AddWithValue("@Il", Il);
                cmd.Parameters.AddWithValue("@Ilce", Ilce);
                cmd.Parameters.AddWithValue("@Mahalle", Mahalle);
                cmd.Parameters.AddWithValue("@Ada", Ada);
                cmd.Parameters.AddWithValue("@Parsel", Parsel);
                cmd.Parameters.AddWithValue("@PortalNumarasi", PortalNumarasi);
                cmd.Parameters.AddWithValue("@Resim", Resim);
                if (SonucTarihi == null)
                    cmd.Parameters.AddWithValue("@SonucTarihi", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@SonucTarihi", SonucTarihi);
                cmd.Parameters.AddWithValue("@KayitYapankullanici", GetCookie("KullaniciID"));

                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);





                result.SonucKodu = "1";
                result.Aciklama = "Kayıt başarıyla kaydedildi.<br>Yönlendiriliyorsunuz...";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void GenelBilgilerGetir()
        {
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GrupKoduListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "Crm2_ProjeTipi");
                cmd.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.dtProjeTipi = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GrupKoduListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "Crm2_Gorevler");
                cmd.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.dtGorevler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GrupKoduListesi";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "Crm2_UlasmaSekli");
                cmd.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.dtUlasimSekli = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GrupKoduListesi";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "Il");
                cmd.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.dtIl = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
        }

        [HttpGet]
        public ActionResult Cariler(string Tur = "",string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Crm2_Cariler";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Tur", Tur);
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);
            ViewBag.aranacakKelime = aranacakKelime;
            ViewBag.Tur = Tur;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View(dt);

        }
        public ActionResult Tanimlamalar()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            return View();
        }



        [HttpGet]
        public ActionResult GrupKodu(string grupKodu, string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cmd.Parameters.AddWithValue("@Kod", grupKodu);
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.GrupKodu = grupKodu;
            ViewBag.aranacakKelime = aranacakKelime;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var model = new GrupkoduListeViewModel
            {
                GrupKodlari = dt,
                Sil = true,
                Duzenle = true

            };

            return View(model);

        }
        [HttpGet]
        public ActionResult GrupKoduEkle(string grupKodu)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            ViewBag.GrupKodu = grupKodu;

            return View();
        }

        [HttpPost]
        public ActionResult GrupKoduEkle(GrupKoduDto grupKoduDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu", new { Area = "Crm2", grupKodu = grupKoduDto.Kod });
        }
        [HttpGet]
        public ActionResult GrupKoduDuzenle(string grupKodu, string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKodu";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.GrupKodu = grupKodu;
            return View(dt);
        }

        [HttpPost]
        public ActionResult GrupKoduDuzenle(GrupKoduDto grupKoduDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", grupKoduDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", grupKoduDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return RedirectToAction("GrupKodu", new { Area = "Crm2", grupKodu = grupKoduDto.Kod });
        }

        [HttpPost]
        public ActionResult GrupKoduSil(string id, string grupKodu)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu", new { Area = "Crm2", grupKodu = grupKodu });
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