using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using YKPortal.Models;
using YKPortal.Models.Dto;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Http.Results;
using System.Configuration;
using System.Data.OleDb;
using System.Web.Services;
using System.Collections;
using Newtonsoft.Json;

namespace YKPortal.Controllers
{
    public class StokController : Controller
    {
        // GET: Stok
        [HttpGet]
        public ActionResult Ekle()
        {
            StokGrupKod1ListesiniOlustur();
            StokGrupKod2ListesiniOlustur();
            StokGrupKod3ListesiniOlustur();
            StokGrupKod4ListesiniOlustur();
            StokGrupKod5ListesiniOlustur();
            StokGrupKod6ListesiniOlustur();
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(StokDto stokDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var cmd = new SqlCommand();
            cmd.CommandText = "p_StokKaydet";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", null);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Isim", stokDto.Isim);
            cmd.Parameters.AddWithValue("@Kod", stokDto.Kod);
            cmd.Parameters.AddWithValue("@Oiv", stokDto.Oiv);
            cmd.Parameters.AddWithValue("@Durumu", stokDto.Durumu);
            cmd.Parameters.AddWithValue("@Aciklama", stokDto.Aciklama);
            cmd.Parameters.AddWithValue("@Barkod", stokDto.Barkod);
            cmd.Parameters.AddWithValue("@OlcuBirimi", stokDto.OlcuBirimi);
            cmd.Parameters.AddWithValue("@GrupKodu1ID", string.IsNullOrEmpty(stokDto.GrupKodu1ID) ? null : stokDto.GrupKodu1ID);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", string.IsNullOrEmpty(stokDto.GrupKodu2ID) ? null : stokDto.GrupKodu2ID);
            cmd.Parameters.AddWithValue("@GrupKodu3ID", string.IsNullOrEmpty(stokDto.GrupKodu3ID) ? null : stokDto.GrupKodu3ID);
            cmd.Parameters.AddWithValue("@GrupKodu4ID", string.IsNullOrEmpty(stokDto.GrupKodu4ID) ? null : stokDto.GrupKodu4ID);
            cmd.Parameters.AddWithValue("@GrupKodu5ID", string.IsNullOrEmpty(stokDto.GrupKodu5ID) ? null : stokDto.GrupKodu5ID);
            cmd.Parameters.AddWithValue("@GrupKodu6ID", string.IsNullOrEmpty(stokDto.GrupKodu6ID) ? null : stokDto.GrupKodu6ID);
            cmd.Parameters.AddWithValue("@KdvAlis", stokDto.KdvAlis);
            cmd.Parameters.AddWithValue("@KdvSatis", stokDto.KdvSatis);
            cmd.Parameters.AddWithValue("@Otv", stokDto.Otv);
            cmd.Parameters.AddWithValue("@OtvFiyat", stokDto.OtvFiyat);
            cmd.Parameters.AddWithValue("@TevkifatPay", stokDto.TevkifatPay);
            cmd.Parameters.AddWithValue("@TevkifatPayda", stokDto.TevkifatPayda);
            cmd.Parameters.AddWithValue("@VadeGunu", stokDto.VadeGunu);
            cmd.Parameters.AddWithValue("@MinimumStok", stokDto.MinimumStok);
            cmd.Parameters.AddWithValue("@MaxsimumStok", stokDto.MaxsimumStok);
            cmd.Parameters.AddWithValue("@LimitUyarisi", stokDto.LimitUyarisi);
            cmd.Parameters.AddWithValue("@LimitDisindaIslemiDurdur", stokDto.LimitDisindaIslemiDurdur);
            cmd.Parameters.AddWithValue("@EksiBakiyeUyarisi", stokDto.EksiBakiyeUyarisi);
            cmd.Parameters.AddWithValue("@EksiBakiyedeIslemiDurdur", stokDto.EksiBakiyedeIslemiDurdur);
            cmd.Parameters.AddWithValue("@StokKilitle", stokDto.StokKilitle);
            cmd.Parameters.AddWithValue("@IskontoSatis1", stokDto.IskontoSatis1);
            cmd.Parameters.AddWithValue("@MarkaID", null);
            cmd.Parameters.AddWithValue("@ModelID", null);
            cmd.Parameters.AddWithValue("@RenkID", null);
            cmd.Parameters.AddWithValue("@BedenID", null);
            cmd.Parameters.AddWithValue("@KaliteID", null);
            cmd.Parameters.AddWithValue("@KayitYapanKullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@AnaStokID", null);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }

        public ActionResult Sil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }
        public ActionResult TopluSil(List<string> idListesi)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (idListesi == null)
            {
                return RedirectToAction("Liste");
            }

            foreach (string id in idListesi)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_StokSil";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return RedirectToAction("Liste");
        }

        public ActionResult Liste(StokDto stokDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var cmd = new SqlCommand();
            cmd.CommandText = "p_StokListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", stokDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", stokDto.Isim);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var stokListesi = new List<StokDto>();

            foreach (DataRow dr in dt.Rows)
            {
                stokListesi.Add(new StokDto
                {
                    ID = Convert.ToString(dr["ID"]),
                    Isim = Convert.ToString(dr["Isim"]),
                    Kod = Convert.ToString(dr["Kod"]),
                    Aciklama = Convert.ToString(dr["Aciklama"])
                });
            }

            ViewBag.StokListesi = stokListesi;
            ViewBag.Filters = stokDto;
            StokGrupKod1ListesiniOlustur();
            StokGrupKod2ListesiniOlustur();

            return View(dt);
        }

        [HttpGet]
        public ActionResult Duzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            StokGrupKod1ListesiniOlustur();
            StokGrupKod2ListesiniOlustur();
            StokGrupKod3ListesiniOlustur();
            StokGrupKod4ListesiniOlustur();
            StokGrupKod5ListesiniOlustur();
            StokGrupKod6ListesiniOlustur();

            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Stok";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.StokID = id;

            return View(dt);

        }
        [HttpPost]
        public ActionResult Duzenle(StokDto stokDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var cmd = new SqlCommand();
            cmd.CommandText = "p_StokKaydet";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", stokDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Isim", stokDto.Isim);
            cmd.Parameters.AddWithValue("@Kod", stokDto.Kod);
            cmd.Parameters.AddWithValue("@Oiv", stokDto.Oiv);
            cmd.Parameters.AddWithValue("@Durumu", stokDto.Durumu);
            cmd.Parameters.AddWithValue("@Aciklama", stokDto.Aciklama);
            cmd.Parameters.AddWithValue("@Barkod", stokDto.Barkod);
            cmd.Parameters.AddWithValue("@OlcuBirimi", stokDto.OlcuBirimi);
            cmd.Parameters.AddWithValue("@GrupKodu1ID", string.IsNullOrEmpty(stokDto.GrupKodu1ID) ? null : stokDto.GrupKodu1ID);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", string.IsNullOrEmpty(stokDto.GrupKodu2ID) ? null : stokDto.GrupKodu2ID);
            cmd.Parameters.AddWithValue("@GrupKodu3ID", string.IsNullOrEmpty(stokDto.GrupKodu3ID) ? null : stokDto.GrupKodu3ID);
            cmd.Parameters.AddWithValue("@GrupKodu4ID", string.IsNullOrEmpty(stokDto.GrupKodu4ID) ? null : stokDto.GrupKodu4ID);
            cmd.Parameters.AddWithValue("@GrupKodu5ID", string.IsNullOrEmpty(stokDto.GrupKodu5ID) ? null : stokDto.GrupKodu5ID);
            cmd.Parameters.AddWithValue("@GrupKodu6ID", string.IsNullOrEmpty(stokDto.GrupKodu6ID) ? null : stokDto.GrupKodu6ID);
            cmd.Parameters.AddWithValue("@KdvAlis", stokDto.KdvAlis);
            cmd.Parameters.AddWithValue("@KdvSatis", stokDto.KdvSatis);
            cmd.Parameters.AddWithValue("@Otv", stokDto.Otv);
            cmd.Parameters.AddWithValue("@OtvFiyat", stokDto.OtvFiyat);
            cmd.Parameters.AddWithValue("@TevkifatPay", stokDto.TevkifatPay);
            cmd.Parameters.AddWithValue("@TevkifatPayda", stokDto.TevkifatPayda);
            cmd.Parameters.AddWithValue("@VadeGunu", stokDto.VadeGunu);
            cmd.Parameters.AddWithValue("@MinimumStok", stokDto.MinimumStok);
            cmd.Parameters.AddWithValue("@MaxsimumStok", stokDto.MaxsimumStok);
            cmd.Parameters.AddWithValue("@LimitUyarisi", stokDto.LimitUyarisi);
            cmd.Parameters.AddWithValue("@LimitDisindaIslemiDurdur", stokDto.LimitDisindaIslemiDurdur);
            cmd.Parameters.AddWithValue("@EksiBakiyeUyarisi", stokDto.EksiBakiyeUyarisi);
            cmd.Parameters.AddWithValue("@EksiBakiyedeIslemiDurdur", stokDto.EksiBakiyedeIslemiDurdur);
            cmd.Parameters.AddWithValue("@StokKilitle", stokDto.StokKilitle);
            cmd.Parameters.AddWithValue("@IskontoSatis1", stokDto.IskontoSatis1);
            cmd.Parameters.AddWithValue("@MarkaID", null);
            cmd.Parameters.AddWithValue("@ModelID", null);
            cmd.Parameters.AddWithValue("@RenkID", null);
            cmd.Parameters.AddWithValue("@BedenID", null);
            cmd.Parameters.AddWithValue("@KaliteID", null);
            cmd.Parameters.AddWithValue("@KayitYapanKullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@AnaStokID", null);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");

        }

        [HttpGet]
        public ActionResult NotEkle(string StokID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.StokID = StokID;
            return View();
        }

        [HttpPost]
        public ActionResult NotEkle(StokNotDto stokNotDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokNotKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@StokID", stokNotDto.StokID);
            cmd.Parameters.AddWithValue("@Aciklama", stokNotDto.Aciklama);  
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("NotListe", new { StokID = stokNotDto.StokID });

        }
       
      
        [HttpGet]
        public ActionResult NotListe(StokNotDto stokNotDto)

        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokNotListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@StokID", stokNotDto.StokID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.StokID = stokNotDto.StokID;
            return View(dt);
        }
        [HttpGet]
        public ActionResult NotDuzenle(string ID, string StokID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokNot";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.ID = ID;
            ViewBag.StokID = StokID;

            return View(dt);
        }
        [HttpPost]
        public ActionResult NotDuzenle(StokNotDto stokNotDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokNotKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", stokNotDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@StokID", stokNotDto.StokID);
            cmd.Parameters.AddWithValue("@Aciklama", stokNotDto.Aciklama);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("NotListe", new { StokID = stokNotDto.StokID });
        }

        [HttpPost]
        public ActionResult NotSil(string id, string StokID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokNotSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@StokID", StokID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("NotListe", new { StokID = StokID });
        }

        [HttpGet]
        public ActionResult ExcelIceAktar()
        { 
            return View();
        }
        [HttpPost]
        public ActionResult ExcelIceAktar(string tip, HttpPostedFileBase file)
        {

            int AktarilanKayitSayisi = 0;
            int AktarilanHataliKayitSayisi = 0;
            ArrayList HataliKayitListesi = new ArrayList();
            List<StokDto> stokDtoListesi = new List<StokDto>();

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
                    var stokDto = new StokDto();
                    var sayac = 0;
                    try
                    {
                        stokDto.Durumu = Convert.ToBoolean(satir[sayac]);
                        stokDto.Kod = Convert.ToString(satir[++sayac]);
                        stokDto.Isim = Convert.ToString(satir[++sayac]);
                        stokDto.Aciklama = Convert.ToString(satir[++sayac]);
                        stokDto.Barkod = Convert.ToString(satir[++sayac]);
                        stokDto.OlcuBirimi = Convert.ToString(satir[++sayac]);
                        stokDto.KdvAlis = Convert.ToDecimal(satir[++sayac]);
                        stokDto.KdvSatis = Convert.ToDecimal(satir[++sayac]);
                        stokDto.Oiv = Convert.ToDecimal(satir[++sayac]);
                        stokDto.Otv = Convert.ToDecimal(satir[++sayac]);
                        stokDto.OtvFiyat = Convert.ToDecimal(satir[++sayac]);
                        stokDto.TevkifatPay = Convert.ToDecimal(satir[++sayac]);
                        stokDto.TevkifatPayda = Convert.ToDecimal(satir[++sayac]);
                        stokDto.VadeGunu = Convert.ToDecimal(satir[++sayac]);
                        stokDto.MinimumStok = Convert.ToDecimal(satir[++sayac]);
                        stokDto.MaxsimumStok = Convert.ToDecimal(satir[++sayac]);
                        stokDto.LimitUyarisi = Convert.ToBoolean(satir[++sayac]);
                        stokDto.LimitDisindaIslemiDurdur = Convert.ToBoolean(satir[++sayac]);
                        stokDto.EksiBakiyeUyarisi = Convert.ToBoolean(satir[++sayac]);
                        stokDto.EksiBakiyedeIslemiDurdur = Convert.ToBoolean(satir[++sayac]);
                        stokDto.StokKilitle = Convert.ToBoolean(satir[++sayac]);
                        stokDto.IskontoSatis1 = Convert.ToDecimal(satir[++sayac]);


                        // bu bilgileri okuyarak p_StokKaydet procedure'u çalıştırılacak ve hata veren satır en sonunda bilgilendirme olarak kullanıcının ekranında gösterilecek.   

                        if (tip == "kaydet")
                        {
                            var cmd = new SqlCommand();

                            cmd.CommandText = "p_StokKaydet";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", null);
                            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                            cmd.Parameters.AddWithValue("@Isim", stokDto.Isim);
                            cmd.Parameters.AddWithValue("@Kod", stokDto.Kod);
                            cmd.Parameters.AddWithValue("@Oiv", stokDto.Oiv);
                            cmd.Parameters.AddWithValue("@Durumu", stokDto.Durumu);
                            cmd.Parameters.AddWithValue("@Aciklama", stokDto.Aciklama);
                            cmd.Parameters.AddWithValue("@Barkod", stokDto.Barkod);
                            cmd.Parameters.AddWithValue("@OlcuBirimi", stokDto.OlcuBirimi);
                            cmd.Parameters.AddWithValue("@GrupKodu1ID", string.IsNullOrEmpty(stokDto.GrupKodu1ID) ? null : stokDto.GrupKodu1ID);
                            cmd.Parameters.AddWithValue("@GrupKodu2ID", string.IsNullOrEmpty(stokDto.GrupKodu2ID) ? null : stokDto.GrupKodu2ID);
                            cmd.Parameters.AddWithValue("@GrupKodu3ID", string.IsNullOrEmpty(stokDto.GrupKodu3ID) ? null : stokDto.GrupKodu3ID);
                            cmd.Parameters.AddWithValue("@GrupKodu4ID", string.IsNullOrEmpty(stokDto.GrupKodu4ID) ? null : stokDto.GrupKodu4ID);
                            cmd.Parameters.AddWithValue("@GrupKodu5ID", string.IsNullOrEmpty(stokDto.GrupKodu5ID) ? null : stokDto.GrupKodu5ID);
                            cmd.Parameters.AddWithValue("@GrupKodu6ID", string.IsNullOrEmpty(stokDto.GrupKodu6ID) ? null : stokDto.GrupKodu6ID);
                            cmd.Parameters.AddWithValue("@KdvAlis", stokDto.KdvAlis);
                            cmd.Parameters.AddWithValue("@KdvSatis", stokDto.KdvSatis);
                            cmd.Parameters.AddWithValue("@Otv", stokDto.Otv);
                            cmd.Parameters.AddWithValue("@OtvFiyat", stokDto.OtvFiyat);
                            cmd.Parameters.AddWithValue("@TevkifatPay", stokDto.TevkifatPay);
                            cmd.Parameters.AddWithValue("@TevkifatPayda", stokDto.TevkifatPayda);
                            cmd.Parameters.AddWithValue("@VadeGunu", stokDto.VadeGunu);
                            cmd.Parameters.AddWithValue("@MinimumStok", stokDto.MinimumStok);
                            cmd.Parameters.AddWithValue("@MaxsimumStok", stokDto.MaxsimumStok);
                            cmd.Parameters.AddWithValue("@LimitUyarisi", stokDto.LimitUyarisi);
                            cmd.Parameters.AddWithValue("@LimitDisindaIslemiDurdur", stokDto.LimitDisindaIslemiDurdur);
                            cmd.Parameters.AddWithValue("@EksiBakiyeUyarisi", stokDto.EksiBakiyeUyarisi);
                            cmd.Parameters.AddWithValue("@EksiBakiyedeIslemiDurdur", stokDto.EksiBakiyedeIslemiDurdur);
                            cmd.Parameters.AddWithValue("@StokKilitle", stokDto.StokKilitle);
                            cmd.Parameters.AddWithValue("@IskontoSatis1", stokDto.IskontoSatis1);
                            cmd.Parameters.AddWithValue("@MarkaID", null);
                            cmd.Parameters.AddWithValue("@ModelID", null);
                            cmd.Parameters.AddWithValue("@RenkID", null);
                            cmd.Parameters.AddWithValue("@BedenID", null);
                            cmd.Parameters.AddWithValue("@KaliteID", null);
                            cmd.Parameters.AddWithValue("@KayitYapanKullaniciID", GetCookie("KullaniciID"));
                            cmd.Parameters.AddWithValue("@AnaStokID", null);

                            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                            if (dt.Rows.Count > 0)
                                if (Convert.ToString(dt.Rows[0]["BİLGİ"]).StartsWith("UYARI!"))
                                {
                                    HataliKayitListesi.Add(stokDto.Isim + " > " + Convert.ToString(dt.Rows[0][0]).StartsWith("UYARI!"));
                                }
                        }
                        else
                        {
                            stokDtoListesi.Add(stokDto);
                        }

                        AktarilanKayitSayisi++;
                    }
                    catch (Exception err)
                    {
                        AktarilanHataliKayitSayisi++;
                        HataliKayitListesi.Add(stokDto.Isim + " > " + err.Message);
                    }
                }

                ViewBag.AktarilanKayitSayisi = AktarilanKayitSayisi;
                ViewBag.AktarilanHataliKayitSayisi = AktarilanHataliKayitSayisi;
                ViewBag.HataliKayitListesi = HataliKayitListesi;
                ViewBag.SimulasyonListesi = stokDtoListesi;
                // Burdaki bilgileri ekranda Aktarılan Kayıt : 800, Hatalı Kayıt 20 gibi gösterip
                // Hatalı kayıtların detayınıda HAtaliKayitListesi List'ini forech ile dönüş ekranda gösterebiliriz.
                return View();
            }
            return RedirectToAction("Liste");
        }
        public JsonResult SelectStokSeriListe(string StokID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokSerileri";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@StokID", StokID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<StokDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new StokDto
                {
                    SeriNo = Convert.ToString(dt.Rows[i]["SeriNo"]),
                });
            }

            return Json(liste, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SelectListe(string search)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", "");
            cmd.Parameters.AddWithValue("@Isim", search);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<StokDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new StokDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Isim = Convert.ToString(dt.Rows[i]["Isim"]),
                    Kod = Convert.ToString(dt.Rows[i]["Kod"]),
                });
            }

            return Json(liste, JsonRequestBehavior.AllowGet);

        }
        // Bir tane stok getirmek icin kullandigimiz metod, bu metod sayesinde id uzerinden bir stoğun Isim ve ID'sini getirebiliyoruz. Select2 icin kullaniyoruz.
        private StokDto Getir(string id)
        {
            if (id != null && id.Length > 0)

            {
                var uyelikId = GetCookie("UyelikID");
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Stok";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


                return new StokDto
                {
                    ID = Convert.ToString(dt.Rows[0]["ID"]),
                    Isim = Convert.ToString(dt.Rows[0]["Isim"])
                };
            }
            return new StokDto { };
        }


        public List<GrupKoduDto> StokGrupKodListesiniGetir(string kodAdi)
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod1Command = new SqlCommand();
            stokGrupKod1Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod1Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod1Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod1Command.Parameters.AddWithValue("@Kod", kodAdi);
            stokGrupKod1Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod1Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod1DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }

            return entities;
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
        public void StokGrupKod1ListesiniOlustur()
        {
            //GrupKodu1 Listesi oluşturma
            SqlCommand stokGrupKod1Command = new SqlCommand();
            stokGrupKod1Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod1Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod1Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod1Command.Parameters.AddWithValue("@Kod", "StokGrupKod1");
            stokGrupKod1Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod1Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod1DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari1 = entities;
        }
        public void StokGrupKod2ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod2Command = new SqlCommand();
            stokGrupKod2Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod2Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod2Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod2Command.Parameters.AddWithValue("@Kod", "StokGrupKod2");
            stokGrupKod2Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod2DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod2Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod2DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod2DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod2DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari2 = entities;
        }

        public void StokGrupKod3ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod3Command = new SqlCommand();
            stokGrupKod3Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod3Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod3Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod3Command.Parameters.AddWithValue("@Kod", "StokGrupKod3");
            stokGrupKod3Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod3DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod3Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod3DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod3DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod3DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari3 = entities;
        }

        public void StokGrupKod4ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod4Command = new SqlCommand();
            stokGrupKod4Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod4Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod4Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod4Command.Parameters.AddWithValue("@Kod", "StokGrupKod4");
            stokGrupKod4Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod4DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod4Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod4DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod4DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod4DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari4 = entities;
        }

        public void StokGrupKod5ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod5Command = new SqlCommand();
            stokGrupKod5Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod5Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod5Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod5Command.Parameters.AddWithValue("@Kod", "StokGrupKod5");
            stokGrupKod5Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod5DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod5Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod5DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod5DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod5DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari5 = entities;
        }

        public void StokGrupKod6ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod6Command = new SqlCommand();
            stokGrupKod6Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod6Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod6Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod6Command.Parameters.AddWithValue("@Kod", "StokGrupKod6");
            stokGrupKod6Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod6DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod6Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod6DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod6DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod6DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari6 = entities;
        }

        #endregion
    }
}
