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
            UlkeListesiniOlustur();
            CariGrupKod1ListesiniOlustur();
            CariGrupKod2ListesiniOlustur();
            CariGrupKod3ListesiniOlustur();
            CariGrupKod4ListesiniOlustur();
            CariGrupKod5ListesiniOlustur();
            CariGrupKod6ListesiniOlustur();
                
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(CariDto cariDto)
        {
            if (!ModelState.IsValid)
            {
                IlListesiniOlustur();
                UlkeListesiniOlustur();
                CariGrupKod1ListesiniOlustur();
                CariGrupKod2ListesiniOlustur();
                CariGrupKod3ListesiniOlustur();
                CariGrupKod4ListesiniOlustur();
                CariGrupKod5ListesiniOlustur();
                CariGrupKod6ListesiniOlustur();
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
            cmd.Parameters.AddWithValue("@GrupKodu1ID", cariDto.GrupKodu1ID == null ? string.Empty : cariDto.GrupKodu1ID);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", cariDto.GrupKodu2ID == null ? string.Empty : cariDto.GrupKodu2ID);
            cmd.Parameters.AddWithValue("@GrupKodu3ID", cariDto.GrupKodu3ID == null ? string.Empty : cariDto.GrupKodu3ID);
            cmd.Parameters.AddWithValue("@GrupKodu4ID", cariDto.GrupKodu4ID == null ? string.Empty : cariDto.GrupKodu4ID);
            cmd.Parameters.AddWithValue("@GrupKodu5ID", cariDto.GrupKodu5ID == null ? string.Empty : cariDto.GrupKodu5ID);
            cmd.Parameters.AddWithValue("@GrupKodu6ID", cariDto.GrupKodu6ID == null ? string.Empty : cariDto.GrupKodu6ID);
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
        [HttpGet]
        public ActionResult Duzenle(string id)
        {
            IlListesiniOlustur();
            UlkeListesiniOlustur();
            CariGrupKod1ListesiniOlustur();
            CariGrupKod2ListesiniOlustur();
            CariGrupKod3ListesiniOlustur();
            CariGrupKod4ListesiniOlustur();
            CariGrupKod5ListesiniOlustur();
            CariGrupKod6ListesiniOlustur();

            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Cari";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
           
        }
        [HttpPost]
        public ActionResult Duzenle(CariDto cariDto)
        {
            if (!ModelState.IsValid)
            {
                IlListesiniOlustur();
                UlkeListesiniOlustur();
                CariGrupKod1ListesiniOlustur();
                CariGrupKod2ListesiniOlustur();
                CariGrupKod3ListesiniOlustur();
                CariGrupKod4ListesiniOlustur();
                CariGrupKod5ListesiniOlustur();
                CariGrupKod6ListesiniOlustur();
                return View();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", cariDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Aktif", cariDto.Aktif);
            cmd.Parameters.AddWithValue("@KayitTarihi", DateTime.Now);
            cmd.Parameters.AddWithValue("@Kod", cariDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", cariDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", cariDto.Unvan);
            cmd.Parameters.AddWithValue("@Adres", cariDto.Adres);
            cmd.Parameters.AddWithValue("@Ilce", cariDto.Ilce == null ? string.Empty : cariDto.Ilce);
            cmd.Parameters.AddWithValue("@Il", cariDto.Il == null ? string.Empty : cariDto.Il);
            cmd.Parameters.AddWithValue("@Ulke", cariDto.Ulke == null ? string.Empty : cariDto.Ulke);
            cmd.Parameters.AddWithValue("@Bolge", cariDto.Bolge == null ? string.Empty : cariDto.Bolge);
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
            cmd.Parameters.AddWithValue("@GrupKodu1ID", cariDto.GrupKodu1ID == null ? string.Empty : cariDto.GrupKodu1ID);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", cariDto.GrupKodu2ID == null ? string.Empty : cariDto.GrupKodu2ID);
            cmd.Parameters.AddWithValue("@GrupKodu3ID", cariDto.GrupKodu3ID == null ? string.Empty : cariDto.GrupKodu3ID);
            cmd.Parameters.AddWithValue("@GrupKodu4ID", cariDto.GrupKodu4ID == null ? string.Empty : cariDto.GrupKodu4ID);
            cmd.Parameters.AddWithValue("@GrupKodu5ID", cariDto.GrupKodu5ID == null ? string.Empty : cariDto.GrupKodu5ID);
            cmd.Parameters.AddWithValue("@GrupKodu6ID", cariDto.GrupKodu6ID == null ? string.Empty : cariDto.GrupKodu6ID);
            cmd.Parameters.AddWithValue("@MuhasebeKodu", cariDto.MuhasebeKodu);
            cmd.Parameters.AddWithValue("@Kilitli", cariDto.Kilitli);
            cmd.Parameters.AddWithValue("@KilitAciklamasi ", cariDto.KilitAciklamasi);
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
            cmd.Parameters.AddWithValue("@LimitAsimindaUyar ", cariDto.LimitAsimindaUyar);
            cmd.Parameters.AddWithValue("@LimitAsimindaDurdur", cariDto.LimitAsimindaDurdur);
            cmd.Parameters.AddWithValue("@CekSenetRiski", cariDto.CekSenetRiski);
            cmd.Parameters.AddWithValue("@Limit", cariDto.Limit);
            cmd.Parameters.AddWithValue("@ServisPersoneli", cariDto.ServisPersoneli);
            cmd.Parameters.AddWithValue("@KullaniciAdi ", cariDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", cariDto.Parola);
            cmd.Parameters.AddWithValue("@RiskAciklama", cariDto.RiskAciklama);
            cmd.Parameters.AddWithValue("@PlasiyerID", cariDto.PlasiyerID == null ? string.Empty : cariDto.PlasiyerID);
            cmd.Parameters.AddWithValue("@AnaCariID", cariDto.AnaCariID == null ? string.Empty : cariDto.AnaCariID);
            cmd.Parameters.AddWithValue("@TeslimCariID", cariDto.TeslimCariID == null ? string.Empty : cariDto.TeslimCariID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }
        public ActionResult Sil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
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

        public void UlkeListesiniOlustur()
        {
            // Ulke Listesi oluşturma 
            SqlCommand ulkeCommand = new SqlCommand();
            ulkeCommand.CommandText = "p_GrupKoduListesi";
            ulkeCommand.CommandType = System.Data.CommandType.StoredProcedure;
            ulkeCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            ulkeCommand.Parameters.AddWithValue("@Kod", "Ulke");
            ulkeCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable ulkeDataTable = (DataTable)IDVeritabani.Sorgula(ulkeCommand, SorgulaTuru.Tablo);

            var ulkeler = new List<string>();

            for (int i = 0; i < ulkeDataTable.Rows.Count; i++)
            {
                ulkeler.Add(Convert.ToString(ulkeDataTable.Rows[i]["Deger"]));
            }
            ViewBag.Ulkeler = ulkeler;
        }
        public void CariGrupKod1ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod1Command = new SqlCommand();
            cariGrupKod1Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod1Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod1Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod1Command.Parameters.AddWithValue("@Kod", "CariGrupKod1");
            cariGrupKod1Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod1Command, SorgulaTuru.Tablo);

            var cariGrupKodlari1 = new Dictionary<string, string>();

            for (int i = 0; i < cariGrupKod1DataTable.Rows.Count; i++)
            {
                cariGrupKodlari1.Add(Convert.ToString(cariGrupKod1DataTable.Rows[i]["ID"]), Convert.ToString(cariGrupKod1DataTable.Rows[i]["Deger"]));
            }
            ViewBag.CariGrupKodlari1 = cariGrupKodlari1;
        }

        public void CariGrupKod2ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod2Command = new SqlCommand();
            cariGrupKod2Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod2Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod2Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod2Command.Parameters.AddWithValue("@Kod", "CariGrupKod2");
            cariGrupKod2Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod2DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod2Command, SorgulaTuru.Tablo);

            var cariGrupKodlari2 = new Dictionary<string, string>();

            for (int i = 0; i < cariGrupKod2DataTable.Rows.Count; i++)
            {
                cariGrupKodlari2.Add(Convert.ToString(cariGrupKod2DataTable.Rows[i]["ID"]), Convert.ToString(cariGrupKod2DataTable.Rows[i]["Deger"]));
            }
            ViewBag.CariGrupKodlari2 = cariGrupKodlari2;
        }
        public void CariGrupKod3ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod3Command = new SqlCommand();
            cariGrupKod3Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod3Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod3Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod3Command.Parameters.AddWithValue("@Kod", "CariGrupKod3");
            cariGrupKod3Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod3DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod3Command, SorgulaTuru.Tablo);

            var cariGrupKodlari3 = new Dictionary<string, string>();

            for (int i = 0; i < cariGrupKod3DataTable.Rows.Count; i++)
            {
                cariGrupKodlari3.Add(Convert.ToString(cariGrupKod3DataTable.Rows[i]["ID"]), Convert.ToString(cariGrupKod3DataTable.Rows[i]["Deger"]));
            }
            ViewBag.CariGrupKodlari3 = cariGrupKodlari3;
        }
        public void CariGrupKod4ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod4Command = new SqlCommand();
            cariGrupKod4Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod4Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod4Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod4Command.Parameters.AddWithValue("@Kod", "CariGrupKod4");
            cariGrupKod4Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod4DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod4Command, SorgulaTuru.Tablo);

            var cariGrupKodlari4 = new Dictionary<string, string>();

            for (int i = 0; i < cariGrupKod4DataTable.Rows.Count; i++)
            {
                cariGrupKodlari4.Add(Convert.ToString(cariGrupKod4DataTable.Rows[i]["ID"]), Convert.ToString(cariGrupKod4DataTable.Rows[i]["Deger"]));
            }
            ViewBag.CariGrupKodlari4 = cariGrupKodlari4;
        }
        public void CariGrupKod5ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod5Command = new SqlCommand();
            cariGrupKod5Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod5Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod5Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod5Command.Parameters.AddWithValue("@Kod", "CariGrupKod5");
            cariGrupKod5Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod5DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod5Command, SorgulaTuru.Tablo);

            var cariGrupKodlari5 = new Dictionary<string, string>();

            for (int i = 0; i < cariGrupKod5DataTable.Rows.Count; i++)
            {
                cariGrupKodlari5.Add(Convert.ToString(cariGrupKod5DataTable.Rows[i]["ID"]), Convert.ToString(cariGrupKod5DataTable.Rows[i]["Deger"]));
            }
            ViewBag.CariGrupKodlari5 = cariGrupKodlari5;
        }
        public void CariGrupKod6ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod6Command = new SqlCommand();
            cariGrupKod6Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod6Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod6Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod6Command.Parameters.AddWithValue("@Kod", "CariGrupKod6");
            cariGrupKod6Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod6DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod6Command, SorgulaTuru.Tablo);

            var cariGrupKodlari6 = new Dictionary<string, string>();

            for (int i = 0; i < cariGrupKod6DataTable.Rows.Count; i++)
            {
                cariGrupKodlari6.Add(Convert.ToString(cariGrupKod6DataTable.Rows[i]["ID"]), Convert.ToString(cariGrupKod6DataTable.Rows[i]["Deger"]));
            }
            ViewBag.CariGrupKodlari6 = cariGrupKodlari6;
        }

        #endregion
    }



}