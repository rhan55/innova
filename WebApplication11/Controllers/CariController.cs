using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApplication11.Models;
using WebApplication11.Models.Dto;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace WebApplication11.Controllers
{
    public class CariController : Controller
    {
        [HttpGet]
        public ActionResult Ekle()
        {
            IlListesiniOlustur();
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(CariDto cariDto)
        {
            if (!ModelState.IsValid)
            {
                IlListesiniOlustur();
                return View();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Aktif", cariDto.Aktif);
            cmd.Parameters.AddWithValue("@KayitTarihi  ", DateTime.Now);
            cmd.Parameters.AddWithValue("@Kod", cariDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", cariDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", cariDto.Unvan);
            cmd.Parameters.AddWithValue("@Adres", cariDto.Adres);
            cmd.Parameters.AddWithValue("@Ilce", cariDto.Ilce);
            cmd.Parameters.AddWithValue("@Il", cariDto.Il);
            cmd.Parameters.AddWithValue("@Ulke", cariDto.Ulke);
            cmd.Parameters.AddWithValue("@Bolge ", cariDto.Bolge);
            cmd.Parameters.AddWithValue("@TCKimlikNo", cariDto.TCKimlikNo);
            cmd.Parameters.AddWithValue("@VergiDairesi", cariDto.VergiDairesi);
            cmd.Parameters.AddWithValue("@VergiNumarasi", cariDto.VergiNumarasi);
            cmd.Parameters.AddWithValue("@PostaKodu", cariDto.PostaKodu);
            cmd.Parameters.AddWithValue("@Alici", cariDto.Alici);
            cmd.Parameters.AddWithValue("@Satici", cariDto.Satici);
            cmd.Parameters.AddWithValue("@Personel", cariDto.Personel);
            cmd.Parameters.AddWithValue("@Telefon1", cariDto.Telefon1);
            cmd.Parameters.AddWithValue("@Telefon2", cariDto.Telefon2);
            cmd.Parameters.AddWithValue("@EMail", cariDto.EMail);
            cmd.Parameters.AddWithValue("@Faks", cariDto.Faks);
            cmd.Parameters.AddWithValue("@CepTelefonu", cariDto.CepTelefonu);
            cmd.Parameters.AddWithValue("@WebSite", cariDto.WebSite);
            cmd.Parameters.AddWithValue("@GrupKodu1ID", cariDto.GrupKodu1ID);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", cariDto.GrupKodu2ID);
            cmd.Parameters.AddWithValue("@GrupKodu3ID", cariDto.GrupKodu3ID);
            cmd.Parameters.AddWithValue("@GrupKodu4ID", cariDto.GrupKodu4ID);
            cmd.Parameters.AddWithValue("@GrupKodu5ID", cariDto.GrupKodu5ID);
            cmd.Parameters.AddWithValue("@GrupKodu6ID", cariDto.GrupKodu6ID);
            cmd.Parameters.AddWithValue("@MuhasebeKodu", cariDto.MuhasebeKodu);
            cmd.Parameters.AddWithValue("@Kilitli", cariDto.Kilitli);
            cmd.Parameters.AddWithValue("@KilitAciklamasi", cariDto.KilitAciklamasi);
            cmd.Parameters.AddWithValue("@DovizID", "");
            cmd.Parameters.AddWithValue("@VadeGunu", cariDto.VadeGunu);
            cmd.Parameters.AddWithValue("@Iskonto1", cariDto.Iskonto1);
            cmd.Parameters.AddWithValue("@ListeFiyat", cariDto.ListeFiyat);
            cmd.Parameters.AddWithValue("@Aciklama1", cariDto.Aciklama1 == null ? string.Empty : cariDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", cariDto.Aciklama2 == null ? string.Empty : cariDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", cariDto.Aciklama3 == null ? string.Empty : cariDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Aciklama4", cariDto.Aciklama4 == null ? string.Empty : cariDto.Aciklama4);
            cmd.Parameters.AddWithValue("@Aciklama5", cariDto.Aciklama5 == null ? string.Empty : cariDto.Aciklama5);
            cmd.Parameters.AddWithValue("@Aciklama6", cariDto.Aciklama6 == null ? string.Empty : cariDto.Aciklama6);
            cmd.Parameters.AddWithValue("@LimitAsimindaUyar", cariDto.LimitAsimindaUyar);
            cmd.Parameters.AddWithValue("@LimitAsimindaDurdur", cariDto.LimitAsimindaDurdur);
            cmd.Parameters.AddWithValue("@CekSenetRiski", cariDto.CekSenetRiski);
            cmd.Parameters.AddWithValue("@Limit", cariDto.Limit);
            cmd.Parameters.AddWithValue("@ServisPersoneli", cariDto.ServisPersoneli);
            cmd.Parameters.AddWithValue("@KullaniciAdi ", cariDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", cariDto.Parola);
            cmd.Parameters.AddWithValue("@RiskAciklama", cariDto.RiskAciklama);
            cmd.Parameters.AddWithValue("@PlasiyerID", "");
            cmd.Parameters.AddWithValue("@AnaCariID", "");
            cmd.Parameters.AddWithValue("@TeslimCariID", "");
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }
        [HttpGet]
        public ActionResult Duzenle(string id)
        {
            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Cari";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }
        [HttpPost]
        public ActionResult Duzenle(CariDto cariDto)
        {
            if (!ModelState.IsValid)
            {
                IlListesiniOlustur();
                return View();
            }

            cariDto.UyelikID = GetCookie("UyelikID");

            cariDto.ID = GetCookie("ID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Aktif ", cariDto.Aktif);
            cmd.Parameters.AddWithValue("@KayitTarihi  ", DateTime.Now);
            cmd.Parameters.AddWithValue("@Kod", cariDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", cariDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", cariDto.Unvan);
            cmd.Parameters.AddWithValue("@Adres", cariDto.Adres);
            cmd.Parameters.AddWithValue("@Ilce", cariDto.Ilce);
            cmd.Parameters.AddWithValue("@Il", cariDto.Il);
            cmd.Parameters.AddWithValue("@Ulke", cariDto.Ulke);
            cmd.Parameters.AddWithValue("@Bolge ", cariDto.Bolge);
            cmd.Parameters.AddWithValue("@TCKimlikNo", cariDto.TCKimlikNo);
            cmd.Parameters.AddWithValue("@VergiDairesi", cariDto.VergiDairesi);
            cmd.Parameters.AddWithValue("@VergiNumarasi", cariDto.VergiNumarasi);
            cmd.Parameters.AddWithValue("@PostaKodu", cariDto.PostaKodu);
            cmd.Parameters.AddWithValue("@Alici", cariDto.Alici);
            cmd.Parameters.AddWithValue("@Satici", cariDto.Satici);
            cmd.Parameters.AddWithValue("@Personel", cariDto.Personel);
            cmd.Parameters.AddWithValue("@Telefon1", cariDto.Telefon1);
            cmd.Parameters.AddWithValue("@Telefon2", cariDto.Telefon2);
            cmd.Parameters.AddWithValue("@EMail", cariDto.EMail);
            cmd.Parameters.AddWithValue("@Faks", cariDto.Faks);
            cmd.Parameters.AddWithValue("@CepTelefonu", cariDto.CepTelefonu);
            cmd.Parameters.AddWithValue("@WebSite", cariDto.WebSite);
            cmd.Parameters.AddWithValue("@GrupKodu1ID", cariDto.GrupKodu1ID);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", cariDto.GrupKodu2ID);
            cmd.Parameters.AddWithValue("@GrupKodu3ID", cariDto.GrupKodu3ID);
            cmd.Parameters.AddWithValue("@GrupKodu4ID", cariDto.GrupKodu4ID);
            cmd.Parameters.AddWithValue("@GrupKodu5ID", cariDto.GrupKodu5ID);
            cmd.Parameters.AddWithValue("@GrupKodu6ID", cariDto.GrupKodu6ID);
            cmd.Parameters.AddWithValue("@MuhasebeKodu", cariDto.MuhasebeKodu);
            cmd.Parameters.AddWithValue("@Kilitli ", cariDto.Kilitli);
            cmd.Parameters.AddWithValue("@KilitAciklamasi ", cariDto.KilitAciklamasi);
            cmd.Parameters.AddWithValue("@DovizID", "");
            cmd.Parameters.AddWithValue("@VadeGunu", cariDto.VadeGunu);
            cmd.Parameters.AddWithValue("@Iskonto1", cariDto.Iskonto1);
            cmd.Parameters.AddWithValue("@ListeFiyat", cariDto.ListeFiyat);
            cmd.Parameters.AddWithValue("@Aciklama1", cariDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", cariDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", cariDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Aciklama4", cariDto.Aciklama4);
            cmd.Parameters.AddWithValue("@Aciklama5", cariDto.Aciklama5);
            cmd.Parameters.AddWithValue("@Aciklama6", cariDto.Aciklama6);
            cmd.Parameters.AddWithValue("@LimitAsimindaUyar ", cariDto.LimitAsimindaUyar);
            cmd.Parameters.AddWithValue("@LimitAsimindaDurdur", cariDto.LimitAsimindaDurdur);
            cmd.Parameters.AddWithValue("@CekSenetRiski", cariDto.CekSenetRiski);
            cmd.Parameters.AddWithValue("@Limit", cariDto.Limit);
            cmd.Parameters.AddWithValue("@ServisPersoneli ", cariDto.ServisPersoneli);
            cmd.Parameters.AddWithValue("@KullaniciAdi ", cariDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", cariDto.Parola);
            cmd.Parameters.AddWithValue("@RiskAciklama", cariDto.RiskAciklama);
            cmd.Parameters.AddWithValue("@PlasiyerID", "");
            cmd.Parameters.AddWithValue("@AnaCariID  ", "");
            cmd.Parameters.AddWithValue("@TeslimCariID  ", "");
            cmd.Parameters.AddWithValue("@KullaniciID  ", cariDto.KullaniciID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }
        public ActionResult Liste(CariDto cariDto)
        {
           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod ", cariDto.Kod);
            cmd.Parameters.AddWithValue("@Isim  ", cariDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", cariDto.Unvan);
            cmd.Parameters.AddWithValue("@TCKimlikNo", cariDto.TCKimlikNo);
            cmd.Parameters.AddWithValue("@VergiNumarasi", cariDto.VergiNumarasi);
            cmd.Parameters.AddWithValue("@CepTelefonu", cariDto.CepTelefonu);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
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
       

        public void IlListesiniOlustur()
        {
            // Il Listesi oluşturma 
            SqlCommand ilCommand = new SqlCommand();
            ilCommand.CommandText = "p_GrupKoduListesi";
            ilCommand.CommandType = System.Data.CommandType.StoredProcedure;
            ilCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            ilCommand.Parameters.AddWithValue("@Kod", "Il");
            ilCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable ilDataTable = (DataTable)IDVeritabani.Sorgula(ilCommand, SorgulaTuru.Tablo);

            var iller = new List<string>();

            for (int i = 0; i < ilDataTable.Rows.Count; i++)
            {
                iller.Add(Convert.ToString(ilDataTable.Rows[i]["Deger"]));
            }
            ViewBag.Iller = iller;
        }

        #endregion
    }



}