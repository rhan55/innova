using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Data.OleDb;
using System.Collections;
using System.Web.Http.Results;
using System.Text.Json;
using YKEFaturaEntegrasyon.Dto;
using YKEFaturaEntegrasyon;
using YKEFaturaEntegrasyon.LogoPostBoxService;

namespace YKPortal.Controllers
{
    public class CariController : Controller
    {
        [HttpGet]
        public ActionResult Ekle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Ekle", "Gor"))
                return Redirect("~/YK/Anasayfa");


            IlListesiniOlustur();
            UlkeListesiniOlustur();
            PlasiyerIDListesiniOlustur();
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
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Ekle", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

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
            ViewBag.PlasiyerID = cariDto;
            return RedirectToAction("Liste");
        }

        [HttpGet]
        public ActionResult ExcelIceAktar()
        {
            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            return View();
        }



        [HttpGet]
        public ActionResult Duzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            IlListesiniOlustur();
            UlkeListesiniOlustur();
            PlasiyerIDListesiniOlustur();
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
            ViewBag.CariID = id;

            var anaCari = Getir(Convert.ToString(dt.Rows[0]["AnaCariID"]));
            var teslimCari = Getir(Convert.ToString(dt.Rows[0]["TeslimCariID"]));

            ViewBag.AnaCari = anaCari;
            ViewBag.TeslimCari = teslimCari;
            ViewBag.Sil = YetkiKontrolu("/Cari/Liste", "Sil");

            return View(dt);

        }
        [HttpPost]
        public ActionResult Duzenle(CariDto cariDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

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
            ViewBag.PlasiyerID = cariDto;
            return RedirectToAction("Liste");
        }

        [HttpPost]
        public JsonResult Sil(CariDto cariDto)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", cariDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult ExcelIceAktar(string tip, HttpPostedFileBase file) // parametreyi öylesine verdik,
        //get ve post metodunun ikiside parametresi olamaz hata veri o yüzden
        {
            if (!YetkiKontrolu("/Cari/Liste", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

            int AktarilanKayitSayisi = 0;
            int AktarilanHataliKayitSayisi = 0;
            ArrayList HataliKayitListesi = new ArrayList();
            List<CariDto> cariDtoListesi = new List<CariDto>();

            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    // Burada klasör olarak bir Temp klasörü açılıp dosyaları oraya ataibliriz.
                    string fileLocation = Server.MapPath("~/Temp/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
                    {
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        using (OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString))
                        {

                            string query = string.Format("Select * from [{0}]", excelSheets[0]); //Exceldeki ilk sayfanın verilerini sorguluyoruz.
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }
                        }
                    }
                }

                foreach (DataRow satir in ds.Tables[0].Rows)
                {
                    var cariDto = new CariDto();
                    try
                    {
                        cariDto.Aktif = Convert.ToBoolean(satir[0]);
                        cariDto.Kod = Convert.ToString(satir[1]);
                        cariDto.Isim = Convert.ToString(satir[2]);
                        cariDto.Unvan = Convert.ToString(satir[3]);
                        cariDto.Alici = Convert.ToBoolean(satir[4]);
                        cariDto.Satici = Convert.ToBoolean(satir[5]);
                        cariDto.Personel = Convert.ToBoolean(satir[6]);
                        cariDto.ServisPersoneli = Convert.ToBoolean(satir[7]);
                        cariDto.Adres = Convert.ToString(satir[8]);
                        cariDto.Bolge = Convert.ToString(satir[9]);
                        cariDto.Ulke = Convert.ToString(satir[10]);
                        cariDto.Il = Convert.ToString(satir[11]);
                        cariDto.Ilce = Convert.ToString(satir[12]);
                        cariDto.PostaKodu = Convert.ToString(satir[13]);
                        cariDto.TCKimlikNo = Convert.ToString(satir[14]);
                        cariDto.VergiNumarasi = Convert.ToString(satir[15]);
                        cariDto.VergiDairesi = Convert.ToString(satir[16]);
                        cariDto.VadeGunu = Convert.ToInt32(satir[17]);
                        cariDto.Limit = Convert.ToDecimal(satir[18]);
                        cariDto.Telefon1 = Convert.ToString(satir[19]);
                        cariDto.Telefon2 = Convert.ToString(satir[20]);
                        cariDto.Faks = Convert.ToString(satir[21]);
                        cariDto.CepTelefonu = Convert.ToString(satir[22]);
                        cariDto.KullaniciAdi = Convert.ToString(satir[23]);
                        cariDto.Parola = Convert.ToString(satir[24]);
                        cariDto.EMail = Convert.ToString(satir[25]);
                        cariDto.WebSite = Convert.ToString(satir[26]);
                        cariDto.Aciklama1 = Convert.ToString(satir[27]);
                        cariDto.Aciklama2 = Convert.ToString(satir[28]);
                        cariDto.Aciklama3 = Convert.ToString(satir[29]);
                        cariDto.Aciklama4 = Convert.ToString(satir[30]);
                        cariDto.Aciklama5 = Convert.ToString(satir[31]);
                        cariDto.Aciklama6 = Convert.ToString(satir[32]);
                        // bu bilgileri okuyarak p_CariKaydet procedure'u çalıştırılacak ve hata veren satır en sonunda bilgilendirme olarak kullanıcının ekranında gösterilecek.   

                        if (tip == "kaydet")
                        {
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
                            if (dt.Rows.Count > 0)
                                if (Convert.ToString(dt.Rows[0]["BİLGİ"]).StartsWith("UYARI!"))
                                {
                                    HataliKayitListesi.Add(cariDto.Isim + " > " + Convert.ToString(dt.Rows[0][0]).StartsWith("UYARI!"));
                                }
                        }
                        else
                        {
                            cariDtoListesi.Add(cariDto);
                        }

                        AktarilanKayitSayisi++;
                    }
                    catch (Exception err)
                    {
                        AktarilanHataliKayitSayisi++;
                        HataliKayitListesi.Add(cariDto.Isim + " > " + err.Message);
                    }
                }

                ViewBag.AktarilanKayitSayisi = AktarilanKayitSayisi;
                ViewBag.AktarilanHataliKayitSayisi = AktarilanHataliKayitSayisi;
                ViewBag.HataliKayitListesi = HataliKayitListesi;
                ViewBag.SimulasyonListesi = cariDtoListesi;
                // Burdaki bilgileri ekranda Aktarılan Kayıt : 800, Hatalı Kayıt 20 gibi gösterip
                // Hatalı kayıtların detayınıda HAtaliKayitListesi List'ini forech ile dönüş ekranda gösterebiliriz.
                return View();
            }

            return RedirectToAction("Liste");
        }

        [HttpGet]
        public ActionResult Liste()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            var model = new CariListeViewModel
            {
                Sil = YetkiKontrolu("/Cari/Liste", "Sil"),
                Duzenle = YetkiKontrolu("/Cari/Liste", "Duzenle")
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Liste(CariDto cariDto)
        {
            if (!AutoGirisKontrol())
                return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);
            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
            {
                return Json(new { success = false, message = "Permission Denied" }, JsonRequestBehavior.AllowGet);
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", cariDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", cariDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", cariDto.Unvan);
            cmd.Parameters.AddWithValue("@TCKimlikNo", cariDto.TCKimlikNo);
            cmd.Parameters.AddWithValue("@VergiNumarasi", cariDto.VergiNumarasi);
            cmd.Parameters.AddWithValue("@CepTelefonu", cariDto.CepTelefonu);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var result = new CariListeJsonModel()
            {
                Sil = YetkiKontrolu("/Cari/Liste", "Sil"),
                Duzenle = YetkiKontrolu("/Cari/Liste", "Duzenle")
            };

            foreach (DataRow row in dt.Rows)
            {
                result.CariListesi.Add(new CariDto
                {
                    Kod = Convert.ToString(row["Kod"]),
                    Isim = Convert.ToString(row["Isim"]),
                    Unvan = Convert.ToString(row["Unvan"]),
                    TCKimlikNo = Convert.ToString(row["TCKimlikNo"]),
                    VergiNumarasi = Convert.ToString(row["VergiNumarasi"]),
                    CepTelefonu = Convert.ToString(row["CepTelefonu"]),
                    GrupKodu1ID = Convert.ToString(row["GrupKodu1ID"]),
                    GrupKodu2ID = Convert.ToString(row["GrupKodu2ID"]),
                    ID = Convert.ToString(row["ID"]),
                    // Yetki kontrolü ile ilgili bilgileri ekliyoruz

                });
            }

            return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ZiyaretListe(string CariID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", CariID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var ziyaretler = new List<ZiyaretDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CariDto cariDto = Getir(Convert.ToString(dt.Rows[i]["CariID"]));
                var kaydiAcan = KullaniciGetir(Convert.ToString(dt.Rows[i]["KullaniciID"]));

                var tamamlayanKullaniciId = Convert.ToString(dt.Rows[i]["TamamlayanKullaniciID"]);

                var tamamlayan = new KullaniciEkleDto();

                if (!string.IsNullOrEmpty(tamamlayanKullaniciId))
                {
                    tamamlayan = KullaniciGetir(tamamlayanKullaniciId);
                }

                ziyaretler.Add(new ZiyaretDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["ID"]),
                    CariID = Convert.ToString(dt.Rows[i]["CariID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["CariID"]),
                    Tarih = Convert.ToString(dt.Rows[i]["Tarih"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Tarih"]),
                    ZiyaretTipi = Convert.ToString(dt.Rows[i]["ZiyaretTipi"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["ZiyaretTipi"]),
                    Aciklama = Convert.ToString(dt.Rows[i]["Aciklama"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Aciklama"]),
                    TamamlamaAciklamasi = Convert.ToString(dt.Rows[i]["TamamlamaAciklamasi"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["TamamlamaAciklamasi"]),
                    TamamlamaTarihi = Convert.ToString(dt.Rows[i]["TamamlamaTarihi"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["TamamlamaTarihi"]),
                    TamamlayanKullaniciID = Convert.ToString(dt.Rows[i]["TamamlayanKullaniciID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["TamamlayanKullaniciID"]),
                    KullaniciID = Convert.ToString(dt.Rows[i]["KullaniciID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["KullaniciID"]),
                    CariIsim = cariDto.Isim,
                    KaydiAcanIsim = kaydiAcan.Ad,
                    TamamlayanIsim = tamamlayan.Ad
                });
            }

            ViewBag.Ziyaretler = ziyaretler;
            ViewBag.CariID = CariID;

            return View(dt);
        }

        [HttpGet]
        public ActionResult ZiyaretEkle(string CariID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            ViewBag.CariID = CariID;
            ZiyaretTipiListesiniOlustur();
            return View();
        }

        [HttpPost]
        public ActionResult ZiyaretEkle(ZiyaretDto ziyaretDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "p_ZiyaretKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", ziyaretDto.CariID);
            cmd.Parameters.AddWithValue("@Tarih", ziyaretDto.Tarih);
            cmd.Parameters.AddWithValue("@Aciklama", ziyaretDto.Aciklama);
            cmd.Parameters.AddWithValue("@TamamlamaAciklamasi", null);
            cmd.Parameters.AddWithValue("@TamamlamaTarihi", null);
            cmd.Parameters.AddWithValue("@TamamlayanKullaniciID", null);

            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("ZiyaretListe", new { CariID = ziyaretDto.CariID });
        }

        public ActionResult ZiyaretSil(string id, string CariID)
        {
            if (!YetkiKontrolu("/Cari/Liste", "Sil"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("ZiyaretListe", new { CariID = CariID });
        }

        [HttpGet]
        public ActionResult ZiyaretDuzenle(string ID)
        {

            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Ziyaret";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            CariDto cariDto = Getir(Convert.ToString(dt.Rows[0]["CariID"]));

            ZiyaretDto ziyaretDto = new ZiyaretDto()
            {
                ID = Convert.ToString(dt.Rows[0]["ID"]),
                CariID = Convert.ToString(dt.Rows[0]["CariID"]),
                Tarih = Convert.ToString(dt.Rows[0]["Tarih"]),
                ZiyaretTipi = Convert.ToString(dt.Rows[0]["ZiyaretTipi"]),
                TamamlamaTarihi = Convert.ToString(dt.Rows[0]["TamamlamaTarihi"]),
                Aciklama = Convert.ToString(dt.Rows[0]["Aciklama"]),
                TamamlamaAciklamasi = Convert.ToString(dt.Rows[0]["TamamlamaAciklamasi"]),
                TamamlayanKullaniciID = Convert.ToString(dt.Rows[0]["TamamlayanKullaniciID"]),
                CariIsim = cariDto.Isim
            };
            var tamamlayanKullanici = new KullaniciEkleDto();

            if (ziyaretDto.TamamlayanKullaniciID.Length > 0)
            {
                tamamlayanKullanici = KullaniciGetir(ziyaretDto.TamamlayanKullaniciID);
            }
            ziyaretDto.TamamlayanIsim = $"{tamamlayanKullanici.Ad} {tamamlayanKullanici.Soyad}";

            ViewBag.Ziyaret = ziyaretDto;
            ViewBag.CariID = ziyaretDto.CariID;

            ZiyaretTipiListesiniOlustur();

            return View();
        }

        [HttpPost]
        public ActionResult ZiyaretDuzenle(ZiyaretDto ziyaretDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Duzenle"))
                return Redirect("~/YK/Anasayfa");
            ziyaretDto.TamamlamaTarihi = ziyaretDto.TamamlamaTarihi.Replace("T", " ");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", ziyaretDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", ziyaretDto.CariID);
            cmd.Parameters.AddWithValue("@Tarih", ziyaretDto.Tarih);
            cmd.Parameters.AddWithValue("@ZiyaretTipi", ziyaretDto.ZiyaretTipi);
            cmd.Parameters.AddWithValue("@Aciklama", ziyaretDto.Aciklama);
            cmd.Parameters.AddWithValue("@TamamlamaAciklamasi", ziyaretDto.TamamlamaAciklamasi);
            cmd.Parameters.AddWithValue("@TamamlamaTarihi", ziyaretDto.TamamlamaTarihi.Replace("T", " "));
            cmd.Parameters.AddWithValue("@TamamlayanKullaniciID", GetCookie("KullaniciID"));

            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("ZiyaretListe", new { CariID = ziyaretDto.CariID });
        }

        [HttpGet]
        public ActionResult ZiyaretiKapat(string CariID, string ID)
        {

            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            ViewBag.CariID = CariID;
            ViewBag.ID = ID;

            ZiyaretTipiListesiniOlustur();

            return View();

        }
        [HttpPost]
        public ActionResult ZiyaretiKapat(ZiyaretDto ziyaretDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

            ziyaretDto.UyelikID = GetCookie("UyelikID");
            ziyaretDto.TamamlayanKullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretKapat";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", ziyaretDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", ziyaretDto.UyelikID);
            cmd.Parameters.AddWithValue("@CariID", ziyaretDto.CariID);
            cmd.Parameters.AddWithValue("@TamamlamaAciklamasi", ziyaretDto.TamamlamaAciklamasi);
            cmd.Parameters.AddWithValue("@TamamlamaTarihi", ziyaretDto.TamamlamaTarihi);
            cmd.Parameters.AddWithValue("@TamamlayanKullaniciID", ziyaretDto.TamamlayanKullaniciID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("ZiyaretListe", new { CariID = ziyaretDto.CariID });

        }


        [HttpPost]
        public JsonResult TopluSil(List<string> idListesi)
        {

            if (idListesi == null)
            {
                return Json(new { success = false, message = "ID Listesi Boş" }, JsonRequestBehavior.AllowGet);
            }

            foreach (string id in idListesi)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_CariSil";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult KisiEkle(string CariID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            ViewBag.CariID = CariID;

            return View();
        }

        [HttpPost]
        public ActionResult KisiEkle(CariKisiDto cariKisiDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

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
            return RedirectToAction("KisiListe", new { CariID = cariKisiDto.CariID });
        }

        [HttpGet]
        public ActionResult KisiListe(CariKisiDto cariKisiDto)
        {
            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKisiListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", cariKisiDto.CariID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.CariID = cariKisiDto.CariID;

            var model = new CariKisiListeViewModel
            {
                Kisiler = dt,
                Sil = YetkiKontrolu("/Cari/Liste", "Sil"),
                Duzenle = YetkiKontrolu("/Cari/Liste", "Duzenle")

            };

            return View(model);
        }

        [HttpGet]
        public ActionResult KisiDuzenle(string ID, string CariID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

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
        public ActionResult KisiDuzenle(CariKisiDto cariKisiDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

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

            return RedirectToAction("KisiListe", new { CariID = cariKisiDto.CariID });

        }

        [HttpPost]
        public ActionResult KisiSil(string id, string CariID)
        {

            if (!YetkiKontrolu("/Cari/Liste", "Sil"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKisiSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@CariID", CariID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("KisiListe", new { CariID = CariID });
        }

        [HttpGet]
        public ActionResult NotEkle(string CariID)
        {

            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            ViewBag.CariID = CariID;
            return View();
        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NotEkle(CariNotDto cariNotDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariNotKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@CariID", cariNotDto.CariID);
            cmd.Parameters.AddWithValue("@Aciklama", HttpUtility.HtmlEncode(cariNotDto.Aciklama));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("NotListe", new { CariID = cariNotDto.CariID });
        }
        [HttpGet]
        public ActionResult NotListe(CariNotDto cariNotDto)
        {
            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariNotListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", cariNotDto.CariID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.CariID = cariNotDto.CariID;

            var model = new CariNotListeViewModel
            {
                Notlar = dt,
                Sil = YetkiKontrolu("/Cari/Liste", "Sil"),
                Duzenle = YetkiKontrolu("/Cari/Liste", "Duzenle")

            };

            return View(model);
        }

        [HttpGet]
        public ActionResult NotDuzenle(string ID, string CariID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Gor"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariNot";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.ID = ID;
            ViewBag.CariID = CariID;

            return View(dt);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NotDuzenle(CariNotDto cariNotDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/Liste", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariNotKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", cariNotDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@CariID", cariNotDto.CariID);
            cmd.Parameters.AddWithValue("@Aciklama", HttpUtility.HtmlEncode(cariNotDto.Aciklama));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("NotListe", new { CariID = cariNotDto.CariID });
        }

        [HttpPost]
        public ActionResult NotSil(string id, string CariID)
        {
            if (!YetkiKontrolu("/Cari/Liste", "Sil"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariNotSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@CariID", CariID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("NotListe", new { CariID = CariID });
        }

        [HttpGet]
        public ActionResult YeniCariHareketKaydi(string CariID, string KayitID)
        {

            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/YeniCariHareketKaydi", "Gor"))
                return Redirect("~/YK/Anasayfa");

            var cari = Getir(CariID);
            ViewBag.CariID = CariID;
            ViewBag.CariIsim = cari.Isim;
            CariHareketTipiListesiniOlustur();
            DovizBirimleriListesiniOlustur();

            CariHareketDto entity = new CariHareketDto();
            if (CariID != null && KayitID != null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_CariHareketi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@CariID", CariID);
                cmd.Parameters.AddWithValue("@ID", KayitID);
                DataTable dtHareket = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dtHareket.Rows.Count > 0)
                {
                    entity.ID = Convert.ToString(dtHareket.Rows[0]["ID"]);
                    entity.CariID = Convert.ToString(dtHareket.Rows[0]["CariID"]);
                    entity.BelgeNo = Convert.ToString(dtHareket.Rows[0]["BelgeNo"]);
                    entity.Tarih = Convert.ToDateTime(dtHareket.Rows[0]["Tarih"]);
                    entity.VadeTarihi = Convert.ToDateTime(dtHareket.Rows[0]["VadeTarihi"]);
                    entity.HareketTipi = Convert.ToString(dtHareket.Rows[0]["HareketTipi"]);
                    entity.GC = Convert.ToString(dtHareket.Rows[0]["GC"]);
                    entity.Tutar = Convert.ToDecimal(dtHareket.Rows[0]["Tutar"]);
                    entity.DovizTipi = Convert.ToString(dtHareket.Rows[0]["DovizTipi"]);
                    entity.Aciklama = Convert.ToString(dtHareket.Rows[0]["Aciklama"]);
                }
            }
            else
            {
                entity.Tarih = DateTime.Today;
                entity.VadeTarihi = DateTime.Today;
                entity.Tutar = 0;
                entity.DovizTipi = "TL";
            }

            return View(entity);
        }

        [HttpPost]
        public ActionResult YeniCariHareketKaydi(CariHareketDto cariHareketDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/YeniCariHareketKaydi", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariHareketiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", cariHareketDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", cariHareketDto.CariID);
            cmd.Parameters.AddWithValue("@Tarih", cariHareketDto.Tarih);
            cmd.Parameters.AddWithValue("@VadeTarihi", cariHareketDto.VadeTarihi);
            cmd.Parameters.AddWithValue("@BelgeNo", cariHareketDto.BelgeNo);
            cmd.Parameters.AddWithValue("@Aciklama", cariHareketDto.Aciklama);
            cmd.Parameters.AddWithValue("@HareketTipi", cariHareketDto.HareketTipi);
            cmd.Parameters.AddWithValue("@GC", cariHareketDto.GC);
            cmd.Parameters.AddWithValue("@Tutar", cariHareketDto.Tutar);
            cmd.Parameters.AddWithValue("@DovizTipi", cariHareketDto.DovizTipi);
            cmd.Parameters.AddWithValue("@Kur", cariHareketDto.Kur);
            cmd.Parameters.AddWithValue("@DovizTutar", cariHareketDto.DovizTutar);
            cmd.Parameters.AddWithValue("@PlasiyerID", string.Empty);
            cmd.Parameters.AddWithValue("@BaglantiID", string.Empty);
            cmd.Parameters.AddWithValue("@Baglanti", cariHareketDto.Baglanti);
            cmd.Parameters.AddWithValue("@GrupKodu1ID", string.Empty);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", string.Empty);
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("HareketListesi", new { CariID = cariHareketDto.CariID });
        }


        [HttpGet]
        public ActionResult HareketListesi(CariHareketDto cariHareketDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/HareketListesi", "Gor"))
                return Redirect("~/YK/Anasayfa");

            DataTable dt = new DataTable();

            if (cariHareketDto.CariID != string.Empty)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_CariHareketListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

                cmd.Parameters.AddWithValue("@CariID", cariHareketDto.CariID);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", string.Empty);
                cmd.Parameters.AddWithValue("@BitisTarihi", string.Empty);

                dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            else
            {
                dt = new DataTable();
            }

            if (cariHareketDto.CariID != string.Empty)
            {
                ViewBag.Cari = Getir(cariHareketDto.CariID);
            }

            return View(dt);
        }
        [HttpPost]
        public ActionResult HareketSil(string KayitID, string CariID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Cari/HareketListesi", "Sil"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariHareketiSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", KayitID);
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@CariID", CariID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("HareketListesi", new { CariID = CariID });
        }

        public JsonResult CariEFaturaBilgiGuncelle(string CariID)
        {

            CariDto cari = Getir(CariID);

            EFaturaLogoPostBoxServiceDto eFaturaLogoAyarlari = YKEFaturaEntegrasyon.EFaturaIslemleri.EFaturaLogoPostBoxServiceAyarlariGetir();

            var logoEntegrasyon = new LogoEntegrasyon(
                    eFaturaLogoAyarlari.EFaturaLogoPostBoxServiceKullaniciAdi,
                    eFaturaLogoAyarlari.EFaturaLogoPostBoxServiceSifre
                );

            //Aşağıdaki kısımları kendine göre ayarlarsın artık.
            GibUserInfoType[] result = logoEntegrasyon.CariMukellefKontrolu(cari.VergiNumarasi + cari.TCKimlikNo);
            bool EFatura = false;
            if (result != null)
            {
                foreach (GibUserInfoType item in result)
                {
                    EFatura = true;

                    string EFaturaAdresi = item.Alias;
                    DateTime EFaturaGecisTarihi = item.AliasRegisterTime.Value;
                    string Unvan = item.Title;
                }
            }


            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "p_CariEFaturaBilgiGuncelle";
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@CariID", cariEFaturaBilgiGuncelleDto.CariID);
            //cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            //cmd.Parameters.AddWithValue("@EFaturaKayitTarihi", cariEFaturaBilgiGuncelleDto.EFaturaKayitTarihi);
            //cmd.Parameters.AddWithValue("@EFaturaPKAdresi", cariEFaturaBilgiGuncelleDto.EFaturaPKAdresi);
            //cmd.Parameters.AddWithValue("@EIrsaliyePKAdresi", cariEFaturaBilgiGuncelleDto.EIrsaliyePKAdresi);
            //cmd.Parameters.AddWithValue("@EFatura", cariEFaturaBilgiGuncelleDto.EFatura);
            //cmd.Parameters.AddWithValue("@EIrsaliye", cariEFaturaBilgiGuncelleDto.EIrsaliye);
            //cmd.Parameters.AddWithValue("@EFaturaSenaryo", cariEFaturaBilgiGuncelleDto.EFaturaSenaryo);
            //cmd.Parameters.AddWithValue("@EFaturaAktiflik", cariEFaturaBilgiGuncelleDto.EFaturaAktiflik);
            //cmd.Parameters.AddWithValue("@EIrsaliyeAktiflik", cariEFaturaBilgiGuncelleDto.EIrsaliyeAktiflik);

            //DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            //var data = new
            //{
            //    EFaturaKayitTarihi = "Tarih",
            //    EFaturaPKAdresi = "Adres",
            //    EIrsaliyePKAdresi = "Irsaliye Adresi",
            //    EFatura = "Fatura",
            //    EIrsaliye = "Irsaliye",
            //    EFaturaSenaryo = "Senaryo",
            //    EFaturaAktiflik = true,
            //    EIrsaliyeAktiflik = false
            //};


            return Json(new { success = true, Data = result, eFatura = EFatura }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SelectListe(string search)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", "");
            cmd.Parameters.AddWithValue("@Isim", search);
            cmd.Parameters.AddWithValue("@Unvan", "");
            cmd.Parameters.AddWithValue("@TCKimlikNo", "");
            cmd.Parameters.AddWithValue("@VergiNumarasi", "");
            cmd.Parameters.AddWithValue("@CepTelefonu", "");

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<CariDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new CariDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Isim = Convert.ToString(dt.Rows[i]["Isim"]),
                    Kod = Convert.ToString(dt.Rows[i]["Kod"]),
                });
            }

            return Json(liste, JsonRequestBehavior.AllowGet);

        }

        // Bir tane cari getirmek icin kullandigimiz metod, bu metod sayesinde id uzerinden bir carinin Isim ve ID'sini getirebiliyoruz. Select2 icin kullaniyoruz.
        private CariDto Getir(string id)
        {
            if (id != null && id.Length > 0)

            {
                var uyelikId = GetCookie("UyelikID");
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Cari";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


                return new CariDto
                {
                    ID = Convert.ToString(dt.Rows[0]["ID"]),
                    Isim = Convert.ToString(dt.Rows[0]["Isim"]),
                    VergiNumarasi = Convert.ToString(dt.Rows[0]["VergiNumarasi"]),
                    TCKimlikNo = Convert.ToString(dt.Rows[0]["TCKimlikNo"]),
                };
            }
            return new CariDto { };
        }
        public KullaniciEkleDto KullaniciGetir(string ID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count == 0)
            {
                return new KullaniciEkleDto();
            }

            return new KullaniciEkleDto
            {
                Ad = Convert.ToString(dt.Rows[0]["Ad"])
            };
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

        private void IlListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand ilCommand = new SqlCommand();
            ilCommand.CommandText = "p_GrupKoduListesi";
            ilCommand.CommandType = System.Data.CommandType.StoredProcedure;
            ilCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            ilCommand.Parameters.AddWithValue("@Kod", "Il");
            ilCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable ilDataTable = (DataTable)IDVeritabani.Sorgula(ilCommand, SorgulaTuru.Tablo);
            // Yeni bir Dto üretiyoruz class üzerindem 
            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < ilDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(ilDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(ilDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.Iller = entities;
        }
        private void UlkeListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand ulkeCommand = new SqlCommand();
            ulkeCommand.CommandText = "p_GrupKoduListesi";
            ulkeCommand.CommandType = System.Data.CommandType.StoredProcedure;
            ulkeCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            ulkeCommand.Parameters.AddWithValue("@Kod", "Ulke");
            ulkeCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable ulkeDataTable = (DataTable)IDVeritabani.Sorgula(ulkeCommand, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < ulkeDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(ulkeDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(ulkeDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.Ulkeler = entities;
        }
        private void PlasiyerIDListesiniOlustur()
        {
            // Il Listesi oluşturma
            SqlCommand plasiyerCommand = new SqlCommand();
            plasiyerCommand.CommandText = "p_PlasiyerListesi";
            plasiyerCommand.CommandType = System.Data.CommandType.StoredProcedure;
            plasiyerCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable PlasiyerIDDataTable = (DataTable)IDVeritabani.Sorgula(plasiyerCommand, SorgulaTuru.Tablo);

            List<SatisPersonelleriDto> entities = new List<SatisPersonelleriDto>();

            for (int i = 0; i < PlasiyerIDDataTable.Rows.Count; i++)
            {
                SatisPersonelleriDto entity = new SatisPersonelleriDto();
                entity.ID = Convert.ToString(PlasiyerIDDataTable.Rows[i]["ID"]);
                entity.Isim = Convert.ToString(PlasiyerIDDataTable.Rows[i]["Isim"]);
                entities.Add(entity);
            }
            ViewBag.PlasiyerID = entities;
        }

        private void CariGrupKod1ListesiniOlustur()
        {
            //GrupKodu1 Listesi oluşturma
            SqlCommand cariGrupKod1Command = new SqlCommand();
            cariGrupKod1Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod1Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod1Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod1Command.Parameters.AddWithValue("@Kod", "CariGrupKod1");
            cariGrupKod1Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod1Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < cariGrupKod1DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(cariGrupKod1DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(cariGrupKod1DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.CariGrupKodlari1 = entities;
        }
        private void CariGrupKod2ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod2Command = new SqlCommand();
            cariGrupKod2Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod2Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod2Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod2Command.Parameters.AddWithValue("@Kod", "CariGrupKod2");
            cariGrupKod2Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod2DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod2Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < cariGrupKod2DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(cariGrupKod2DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(cariGrupKod2DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.CariGrupKodlari2 = entities;
        }

        private void CariGrupKod3ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod3Command = new SqlCommand();
            cariGrupKod3Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod3Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod3Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod3Command.Parameters.AddWithValue("@Kod", "CariGrupKod3");
            cariGrupKod3Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod3DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod3Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < cariGrupKod3DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(cariGrupKod3DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(cariGrupKod3DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.CariGrupKodlari3 = entities;
        }

        private void CariGrupKod4ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod4Command = new SqlCommand();
            cariGrupKod4Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod4Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod4Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod4Command.Parameters.AddWithValue("@Kod", "CariGrupKod4");
            cariGrupKod4Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod4DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod4Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < cariGrupKod4DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(cariGrupKod4DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(cariGrupKod4DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.CariGrupKodlari4 = entities;
        }

        private void CariGrupKod5ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod5Command = new SqlCommand();
            cariGrupKod5Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod5Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod5Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod5Command.Parameters.AddWithValue("@Kod", "CariGrupKod5");
            cariGrupKod5Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod5DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod5Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < cariGrupKod5DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(cariGrupKod5DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(cariGrupKod5DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.CariGrupKodlari5 = entities;
        }

        private void CariGrupKod6ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod6Command = new SqlCommand();
            cariGrupKod6Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod6Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod6Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod6Command.Parameters.AddWithValue("@Kod", "CariGrupKod6");
            cariGrupKod6Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod6DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod6Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < cariGrupKod6DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(cariGrupKod6DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(cariGrupKod6DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.CariGrupKodlari6 = entities;
        }

        private void ZiyaretTipiListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand ziyaretTipiCommand = new SqlCommand();
            ziyaretTipiCommand.CommandText = "p_GrupKoduListesi";
            ziyaretTipiCommand.CommandType = System.Data.CommandType.StoredProcedure;
            ziyaretTipiCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            ziyaretTipiCommand.Parameters.AddWithValue("@Kod", "Crm_ZiyaretTipi");
            ziyaretTipiCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable ziyaretTipiDataTable = (DataTable)IDVeritabani.Sorgula(ziyaretTipiCommand, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < ziyaretTipiDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(ziyaretTipiDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(ziyaretTipiDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.ZiyaretTipleri = entities;
        }

        private void CariHareketTipiListesiniOlustur()
        {
            //GrupKodu1 Listesi oluşturma
            SqlCommand cariHareketTipiCommand = new SqlCommand();
            cariHareketTipiCommand.CommandText = "p_GrupKoduListesi";
            cariHareketTipiCommand.CommandType = System.Data.CommandType.StoredProcedure;
            cariHareketTipiCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariHareketTipiCommand.Parameters.AddWithValue("@Kod", "CariHareketTipi");
            cariHareketTipiCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariHareketTipiDataTable = (DataTable)IDVeritabani.Sorgula(cariHareketTipiCommand, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < cariHareketTipiDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(cariHareketTipiDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(cariHareketTipiDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.CariHareketTipleri = entities;
        }

        private void DovizBirimleriListesiniOlustur()
        {
            //GrupKodu1 Listesi oluşturma
            SqlCommand grupKoduCommand = new SqlCommand();
            grupKoduCommand.CommandText = "p_GrupKoduListesi";
            grupKoduCommand.CommandType = System.Data.CommandType.StoredProcedure;
            grupKoduCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            grupKoduCommand.Parameters.AddWithValue("@Kod", "DovizBirimi");
            grupKoduCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable grupKoduDataTable = (DataTable)IDVeritabani.Sorgula(grupKoduCommand, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < grupKoduDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(grupKoduDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(grupKoduDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.DovizBirimleri = entities;
        }
        private List<GrupKoduDto> CariGrupKodListesiniGetir(string kodAdi)
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod1Command = new SqlCommand();
            cariGrupKod1Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod1Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod1Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod1Command.Parameters.AddWithValue("@Kod", kodAdi);
            cariGrupKod1Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod1Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < cariGrupKod1DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(cariGrupKod1DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(cariGrupKod1DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }

            return entities;
        }

        #endregion

        private bool YetkiKontrolu(string YetkiUrl, string Tip = "Gor")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciYetkileri";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            List<YetkilerDto> yetkiler = new List<YetkilerDto>();

            foreach (DataRow row in dt.Rows)
            {
                yetkiler.Add(new YetkilerDto()
                {
                    MenuID = Convert.ToString(row["MenuID"]),
                    KullaniciID = Convert.ToString(row["KullaniciID"]),
                    UyelikID = Convert.ToString(row["UyelikID"]),
                    Menu = Convert.ToString(row["Menu"]),
                    UstID = Convert.ToString(row["UstID"]),
                    Gor = Convert.ToBoolean(row["Gor"]),
                    Duzenle = Convert.ToBoolean(row["Duzenle"]),
                    Sil = Convert.ToBoolean(row["Sil"]),
                    url = Convert.ToString(row["url"]),
                });
            }
            var yetki = yetkiler.Where(m => m.url == YetkiUrl).FirstOrDefault();
            if (yetki != null)
            {
                if (Tip == "Gor")
                {
                    return yetki.Gor;
                }
                else if (Tip == "Duzenle")
                {
                    return yetki.Duzenle;
                }
                else if (Tip == "Sil")
                {
                    return yetki.Sil;
                }
                return false;
            }
            else
            {
                return false;
            }
        }


    }
}

