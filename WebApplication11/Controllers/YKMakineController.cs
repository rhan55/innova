using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.YKClasses;

namespace YKPortal.Controllers
{
    public class YKMakineController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Bilgilendirme(string mesaj)
        {
            ViewBag.Mesaj = mesaj;
            return View();
        }

        #region Sayım İşlemleri

        public ActionResult Sayim(string Baslangic = "", string Bitis = "", string Teknisyen = "", string Cari = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/Kullanici/Giris");

            if (Baslangic == "")
            {
                Baslangic = DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd");
            }
            if (Bitis == "")
            {
                Bitis = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            }

            ViewBag.Baslangic = Baslangic;
            ViewBag.Bitis = Bitis;
            ViewBag.Teknisyen = Teknisyen;
            ViewBag.Cari = Cari;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_SayimListesi";
            cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
            cmd.Parameters.AddWithValue("@Bitis", Bitis);
            cmd.Parameters.AddWithValue("@Teknisyen", Teknisyen);
            cmd.Parameters.AddWithValue("@Cari", Cari);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select DISTINCT DepoAdi from [w_StokSeriBakiye] order by DepoAdi";
            cmd2.CommandType = System.Data.CommandType.Text;
            ViewBag.dtCariler = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);

            cmd.Parameters.Clear();
            cmd.CommandText = "select ID,Ad+' '+Soyad as Isim from Kullanicilar WITH(NOLOCK) Where Aktif = 1 and UyelikID = @UyelikID";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            ViewBag.Teknisyenler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(ds);

        }

        public JsonResult SayimCihazBilgisi(string SeriNo)
        {
            JsonResult result = new JsonResult();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = @"p_Sayim_CihazDetay";
            cmd.Parameters.AddWithValue("@SeriNo", SeriNo);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            IDModel entity = new IDModel();
            if (dt.Rows.Count > 0)
            {
                entity.CariAdi = Convert.ToString(dt.Rows[0]["DepoAdi"]);
                entity.StokKod = Convert.ToString(dt.Rows[0]["STOK_KODU"]);
                entity.StokIsim = Convert.ToString(dt.Rows[0]["StokAdi"]);
            }
            result.Data = entity;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SayimKaydet(string SeriNo, string CariKodu, string Bolum)
        {
            JsonResult result = new JsonResult();

            string KullaniciID = GetCookie("KullaniciID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = @"p_Sayim_Kaydet";
            cmd.Parameters.AddWithValue("@SeriNo", SeriNo);
            cmd.Parameters.AddWithValue("@CariKodu", CariKodu.Replace("Cari : ", ""));
            cmd.Parameters.AddWithValue("@Bolum", Bolum);
            cmd.Parameters.AddWithValue("@Kullanici", KullaniciID);

            result.Data = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Arıza Kayıtları


        public JsonResult CariCihazGetir(string CariKodu)
        {
            JsonResult result = new JsonResult();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = @"p_Servis_CariCihaz";
            cmd.Parameters.AddWithValue("@CariKodu", CariKodu);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ArrayList list = new ArrayList();
            foreach (DataRow dr in dt.Rows)
            {
                SelectListItem item = new SelectListItem();
                item.Text = Convert.ToString(dr["Isim"]);
                item.Value = Convert.ToString(dr["Kod"]);
                list.Add(item);
            }
            result.Data = list;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Arizalar(string Baslangic = "", string Bitis = "", string Durum = "", string Teknisyen = "", string Cari = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/Kullanici/Giris");



            if (Baslangic == "")
            {
                Baslangic = DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd");
            }
            if (Bitis == "")
            {
                Bitis = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            }

            ViewBag.Baslangic = Baslangic;
            ViewBag.Bitis = Bitis;
            ViewBag.Durum = Durum;
            ViewBag.Teknisyen = Teknisyen;
            ViewBag.Cari = Cari;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ArizaListesi";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
            cmd.Parameters.AddWithValue("@Bitis", Bitis);
            cmd.Parameters.AddWithValue("@Durum", Durum);
            cmd.Parameters.AddWithValue("@Teknisyen", Teknisyen);
            cmd.Parameters.AddWithValue("@Cari", Cari);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select * from w_CariStokBakiyeleri ";
            cmd2.CommandType = System.Data.CommandType.Text;
            ViewBag.dtCariler = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);

            cmd.Parameters.Clear();
            cmd.CommandText = "select ID,Ad+' '+Soyad as Isim from Kullanicilar WITH(NOLOCK) Where Aktif = 1 and UyelikID = @UyelikID ";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            ViewBag.Teknisyenler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);



            return View(ds);
        }

        public ActionResult ArizaKaydet(string CariKodu, string SeriNo, string Sikayet)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ArizaKaydet";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Tarih", DateTime.Now);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@KullaniciAdi", GetCookie("KullaniciAdi"));
            cmd.Parameters.AddWithValue("@PlasiyerKodu", "");
            cmd.Parameters.AddWithValue("@CariKodu", CariKodu);
            cmd.Parameters.AddWithValue("@SeriNo", Convert.ToString(SeriNo));
            cmd.Parameters.AddWithValue("@Aciklama", Sikayet);
            string SonID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

            return Redirect("~/YKMakine/ArizaDetay/" + SonID);
        }

        public ActionResult ArizaGuncelle(int id, string CariKodu, string Teknisyen, string Durum, string Aciklama,
            string DegisenParcalar,
            string SeriNo,
            string ArizayiBildiren, string ArizayiBildirenTelefon, string EMail, string BulunduguYer,
            string Imza)
        {


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ArizaGuncelle";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@Teknisyen", Teknisyen);
            cmd.Parameters.AddWithValue("@Durum", Durum);
            cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
            cmd.Parameters.AddWithValue("@SeriNo", SeriNo);
            cmd.Parameters.AddWithValue("@ArizayiBildiren", ArizayiBildiren);
            cmd.Parameters.AddWithValue("@ArizayiBildirenTelefon", ArizayiBildirenTelefon);
            cmd.Parameters.AddWithValue("@EMail", EMail);
            cmd.Parameters.AddWithValue("@BulunduguYer", BulunduguYer);
            cmd.Parameters.AddWithValue("@Imza", Imza);
            cmd.Parameters.AddWithValue("@KullaniciAdi", GetCookie("KullaniciAdi"));


            bool ResimYuklendi = false;
            bool ResimYuklendi2 = false;
            bool ResimYuklendi3 = false;
            bool ResimYuklendi4 = false;
            bool ResimYuklendi5 = false;
            if (Request.Files.Count > 0)
            {
                foreach (string resimAdi in Request.Files)
                {
                    if (resimAdi == ("ArizaResmi"))
                    {
                        var file = Request.Files[resimAdi];
                        if (file != null && file.ContentLength > 0)
                        {
                            string YeniResimAdi = Guid.NewGuid().ToString();
                            file.SaveAs(Server.MapPath("~/Resimler/" + YeniResimAdi + file.FileName));
                            cmd.Parameters.AddWithValue("@Dosya", YeniResimAdi + file.FileName);
                            ResimYuklendi = true;
                        }
                    }
                    if (resimAdi == ("ArizaResmi2"))
                    {
                        var file = Request.Files[resimAdi];
                        if (file != null && file.ContentLength > 0)
                        {
                            string YeniResimAdi = Guid.NewGuid().ToString();
                            file.SaveAs(Server.MapPath("~/Resimler/" + YeniResimAdi + file.FileName));
                            cmd.Parameters.AddWithValue("@Dosya2", YeniResimAdi + file.FileName);
                            ResimYuklendi2 = true;
                        }
                    }
                    if (resimAdi == ("ArizaResmi3"))
                    {
                        var file = Request.Files[resimAdi];
                        if (file != null && file.ContentLength > 0)
                        {
                            string YeniResimAdi = Guid.NewGuid().ToString();
                            file.SaveAs(Server.MapPath("~/Resimler/" + YeniResimAdi + file.FileName));
                            cmd.Parameters.AddWithValue("@Dosya3", YeniResimAdi + file.FileName);
                            ResimYuklendi3 = true;
                        }
                    }
                    if (resimAdi == ("ArizaResmi4"))
                    {
                        var file = Request.Files[resimAdi];
                        if (file != null && file.ContentLength > 0)
                        {
                            string YeniResimAdi = Guid.NewGuid().ToString();
                            file.SaveAs(Server.MapPath("~/Resimler/" + YeniResimAdi + file.FileName));
                            cmd.Parameters.AddWithValue("@Dosya4", YeniResimAdi + file.FileName);
                            ResimYuklendi4 = true;
                        }
                    }
                    if (resimAdi == ("ArizaResmi5"))
                    {
                        var file = Request.Files[resimAdi];
                        if (file != null && file.ContentLength > 0)
                        {
                            string YeniResimAdi = Guid.NewGuid().ToString();
                            file.SaveAs(Server.MapPath("~/Resimler/" + YeniResimAdi + file.FileName));
                            cmd.Parameters.AddWithValue("@Dosya5", YeniResimAdi + file.FileName);
                            ResimYuklendi5 = true;
                        }
                    }
                }
            }
            if (!ResimYuklendi)
                cmd.Parameters.AddWithValue("@Dosya", "");
            if (!ResimYuklendi2)
                cmd.Parameters.AddWithValue("@Dosya2", "");
            if (!ResimYuklendi3)
                cmd.Parameters.AddWithValue("@Dosya3", "");
            if (!ResimYuklendi4)
                cmd.Parameters.AddWithValue("@Dosya4", "");
            if (!ResimYuklendi5)
                cmd.Parameters.AddWithValue("@Dosya5", "");


            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            cmd.Parameters.Clear();
            cmd.CommandText = "Delete From ServisDegisenParcalar Where ServisID = @ServisID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ServisID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            foreach (string stokkodu in DegisenParcalar.Trim().Split(','))
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "Insert Into ServisDegisenParcalar (ServisID,StokKodu) values (@ServisID,@StokKodu)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ServisID", id);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu.Trim());
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }


            try
            {
                string telefon = "";
                cmd.Parameters.Clear();
                cmd.CommandText = "Select Telefon From Kullanicilar WITH(NOLOCK) Where ID = @ID";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", Teknisyen);
                telefon = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                if (telefon.Trim().Length > 0 && Durum == "Beklemede")
                {
                    string teknisyenIsim = "";
                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select Isim From Kullanicilar WITH(NOLOCK) Where ID = @ID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ID", Teknisyen);
                    teknisyenIsim = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

                    string url = @"http://idyazilim.com/Site/SmsGonder/?telefon=" + telefon + "&" +
                        "KullaniciAdi=" + ConfigurationManager.AppSettings["SmsKullaniciAdi"] + "&" +
                        "Parola=" + ConfigurationManager.AppSettings["SmsParola"] + "&" +
                        "Isim=" + ConfigurationManager.AppSettings["SmsIsim"] + "&" +
                        "Sirket=Schiller&" +
                        "Program=" + "IDERP-App" + "&" +
                        "mesaj=" + "Yenni Arıza Kaydı! Cari: " + CariKodu + ", Seri: " + SeriNo + ", Yer: " + BulunduguYer + ", Teknisyen: " + teknisyenIsim + ", Bildiren: " + ArizayiBildiren + ", Telefon: " + ArizayiBildirenTelefon;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    var sonuc = request.GetResponse();
                }
            }
            catch (Exception err)
            {
                ;
            }

            if (Durum.EndsWith("Yapıldı"))
            {
                #region Mail Gönderimi
                string mail = EMail + ";cigdem@schillerturkiye.com;info@ykyazilim.com.tr";

                string Icerik = @"
Değerli iş ortağımız,<br>
Aşağıda yer alan bilgiler doğrultusunda teknik ekibimiz tarafından yapılan işlemleri bilgilerinize sunarız.<br><br>
Durum : " + Durum + @" <br>
İsim : " + CariKodu + @" <br>
SeriNo : " + SeriNo + @" <br>
BulunduguYer : " + BulunduguYer + @" <br>
Teknisyen : " + Teknisyen + @" <br>
Aciklama : " + Aciklama + @" <br>
ArizayiBildiren : " + ArizayiBildiren + @" <br>
ArizayiBildirenTelefon : " + ArizayiBildirenTelefon + @" <br>
İyi günler, iyi çalışmalar diliyoruz. Sevgi ve saygılarımla.<br>
";
                YKUtils.MailGonder(CariKodu + " YAPILAN MÜDAHALELER HAKKINDA", Icerik, mail, "", null, "Teknik");

                #endregion
            }
            else if (Durum.EndsWith("İade Alındı"))
            {
                #region Mail Gönderimi
                string mail = EMail + ";cigdem@schillerturkiye.com;info@ykyazilim.com.tr";

                string Icerik = @"
Değerli iş ortağımız,<br>
Aşağıda yer alan bilgiler doğrultusunda teknik ekibimiz tarafından yapılan işlemleri bilgilerinize sunarım.<br>
Durum : " + Durum + @" <br>
İsim : " + CariKodu + @" <br>
SeriNo : " + SeriNo + @" <br>
BulunduguYer : " + BulunduguYer + @" <br>
Teknisyen : " + Teknisyen + @" <br>
Aciklama : " + Aciklama + @" <br>
ArizayiBildiren : " + ArizayiBildiren + @" <br>
ArizayiBildirenTelefon : " + ArizayiBildirenTelefon + @" <br>
İyi günler, iyi çalışmalar diliyorum. Sevgi ve saygılarımla.<br>
";
                YKUtils.MailGonder(CariKodu + " YAPILAN MÜDAHALELER HAKKINDA", Icerik, mail, "", null, "Teknik");

                #endregion
            }

            return Redirect("~/YKMakine/Arizalar");
        }
        public ActionResult ArizaDetay(int id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/Kullanici/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ArizaDetay";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID",GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", id);
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            cmd.Parameters.Clear();
            cmd.CommandText = "select ID,Ad+' '+Soyad as Isim from Kullanicilar WITH(NOLOCK) Where Aktif = 1 and UyelikID = @UyelikID ";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            ViewBag.Teknisyenler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            cmd.Parameters.Clear();
            cmd.CommandText = "select * from w_Stoklar order by StokKodu";
            cmd.CommandType = System.Data.CommandType.Text;
            ViewBag.YedekParcalar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            return View(ds);
        }

        #endregion

        #region Bakım İşlemleri

        public ActionResult BakimPlaniOlustur()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BakimKayitlariniOlustur";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Yil", DateTime.Today.Year);
            cmd.Parameters.AddWithValue("@Ay", DateTime.Today.Month);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return Redirect("~/YKMakine/Bilgilendirme/?mesaj=Bu ayki bakım planı oluşturuldu.");
        }

        public ActionResult BakimDetay(int id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/Kullanici/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BakimDetay";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult BakimYapSoru(int id, string Tip, string KategoriAdi)
        {
            Tip = "Bakım";
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "p_SorulariGetir";
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@Tip", Tip);
            cmd2.Parameters.AddWithValue("@ServisID", id);
            DataTable dtSorular = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);

            foreach (DataRow soru in dtSorular.Rows)
            {
                if (Convert.ToString(soru["KategoriAdi"]) == KategoriAdi)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Insert Into Cevaplar  (ServisID,SoruID,Dosya,TestTipi1,TestTipi2,TestTipi3,TestTipi4,TestTipi5,TestSonucu1,TestSonucu2,TestSonucu3,TestSonucu4,TestSonucu5,KayitYapanKullanici) values (@ServisID,@SoruID,@Dosya,@TestTipi1,@TestTipi2,@TestTipi3,@TestTipi4,@TestTipi5,@TestSonucu1,@TestSonucu2,@TestSonucu3,@TestSonucu4,@TestSonucu5,@KayitYapanKullanici) ";
                    cmd.Parameters.AddWithValue("@ServisID", id);
                    cmd.Parameters.AddWithValue("@SoruID", soru["ID"]);

                    var deger1 = Request["Cevap1_" + soru["ID"]];
                    var deger2 = Request["Cevap2_" + soru["ID"]];
                    var deger3 = Request["Cevap3_" + soru["ID"]];
                    var deger4 = Request["Cevap4_" + soru["ID"]];
                    var deger5 = Request["Cevap5_" + soru["ID"]];

                    cmd.Parameters.AddWithValue("@TestTipi1", deger1 == null ? false : true);
                    cmd.Parameters.AddWithValue("@TestTipi2", deger2 == null ? false : true);
                    cmd.Parameters.AddWithValue("@TestTipi3", deger3 == null ? false : true);
                    cmd.Parameters.AddWithValue("@TestTipi4", deger4 == null ? false : true);
                    cmd.Parameters.AddWithValue("@TestTipi5", deger5 == null ? false : true);

                    cmd.Parameters.AddWithValue("@TestSonucu1", false);
                    cmd.Parameters.AddWithValue("@TestSonucu2", false);
                    cmd.Parameters.AddWithValue("@TestSonucu3", false);
                    cmd.Parameters.AddWithValue("@TestSonucu4", false);
                    cmd.Parameters.AddWithValue("@TestSonucu5", false);
                    cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("KullaniciAdi"));

                    bool ResimYuklendi = false;
                    if (Request.Files.Count > 0)
                    {
                        foreach (string resimAdi in Request.Files)
                        {
                            if (resimAdi.Contains("Dosya_[" + soru["ID"] + "]"))
                            {
                                var file = Request.Files[resimAdi];
                                if (file != null)
                                {
                                    string YeniResimAdi = Guid.NewGuid().ToString();
                                    file.SaveAs(Server.MapPath("~/Resimler/" + YeniResimAdi + file.FileName));
                                    cmd.Parameters.AddWithValue("@Dosya", YeniResimAdi + file.FileName);
                                    ResimYuklendi = true;
                                }
                            }
                        }
                    }
                    if (!ResimYuklendi)
                    {
                        cmd.Parameters.AddWithValue("@Dosya", "");
                    }

                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }
            }

            return RedirectToAction("BakimYap", "YKMakine", new { id = id });
        }

        [HttpPost]
        public ActionResult BakimSikayetKaydet(int id, string Sikayet = "")
        {
            if (Sikayet.Trim().Length > 0)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_BakimKapat";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Durum", "");
                cmd.Parameters.AddWithValue("@SeriNo", "");
                cmd.Parameters.AddWithValue("@Aciklama", "");
                cmd.Parameters.AddWithValue("@Sikayet", Sikayet);
                cmd.Parameters.AddWithValue("@YetkiliIsim", "");
                cmd.Parameters.AddWithValue("@Imza", "");

                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }
            return RedirectToAction("BakimYap", "YKMakine", new { id = id });
        }

        [HttpPost]
        public ActionResult BakimKapat(int id, string Durum,
            string SeriNo = "", string Aciklama = "", string YetkiliIsim = "", string Imza = "")
        {

            #region Resim İşlemleri
            /*
            if (Cevap1 != null)
            {
                if (Cevap1.ContentLength > 0)
                {
                    //string guid = Cevap1.FileName.Replace(Path.GetExtension(Cevap1.FileName), "") + "_" + Kod;// 
                    string guid = id + "_" + Guid.NewGuid().ToString();
                    string[] fileName = Path.GetFileName(Cevap1.FileName).Split('.');
                    string _yol = guid + "." + fileName[fileName.Length - 1];
                    _Cevap1 = _yol;
                    var path = Path.Combine(Server.MapPath(Url.Content("~/Resimler")), _yol);

                    Cevap1.SaveAs(path);
                }
            }
            if (Cevap2 != null)
            {
                if (Cevap2.ContentLength > 0)
                {
                    //string guid = Cevap1.FileName.Replace(Path.GetExtension(Cevap1.FileName), "") + "_" + Kod;// 
                    string guid = id + "_" + Guid.NewGuid().ToString();
                    string[] fileName = Path.GetFileName(Cevap2.FileName).Split('.');
                    string _yol = guid + "." + fileName[fileName.Length - 1];
                    _Cevap2 = _yol;
                    var path = Path.Combine(Server.MapPath(Url.Content("~/Resimler")), _yol);

                    Cevap2.SaveAs(path);
                }
            }
            if (Cevap3 != null)
            {
                if (Cevap3.ContentLength > 0)
                {
                    //string guid = Cevap1.FileName.Replace(Path.GetExtension(Cevap1.FileName), "") + "_" + Kod;// 
                    string guid = id + "_" + Guid.NewGuid().ToString();
                    string[] fileName = Path.GetFileName(Cevap3.FileName).Split('.');
                    string _yol = guid + "." + fileName[fileName.Length - 1];
                    _Cevap3 = _yol;
                    var path = Path.Combine(Server.MapPath(Url.Content("~/Resimler")), _yol);

                    Cevap3.SaveAs(path);
                }
            }
            if (Cevap4 != null)
            {
                if (Cevap4.ContentLength > 0)
                {
                    //string guid = Cevap1.FileName.Replace(Path.GetExtension(Cevap1.FileName), "") + "_" + Kod;// 
                    string guid = id + "_" + Guid.NewGuid().ToString();
                    string[] fileName = Path.GetFileName(Cevap4.FileName).Split('.');
                    string _yol = guid + "." + fileName[fileName.Length - 1];
                    _Cevap4 = _yol;
                    var path = Path.Combine(Server.MapPath(Url.Content("~/Resimler")), _yol);

                    Cevap4.SaveAs(path);
                }
            }
            if (Cevap5 != null)
            {
                if (Cevap5.ContentLength > 0)
                {
                    //string guid = Cevap1.FileName.Replace(Path.GetExtension(Cevap1.FileName), "") + "_" + Kod;// 
                    string guid = id + "_" + Guid.NewGuid().ToString();
                    string[] fileName = Path.GetFileName(Cevap5.FileName).Split('.');
                    string _yol = guid + "." + fileName[fileName.Length - 1];
                    _Cevap5 = _yol;
                    var path = Path.Combine(Server.MapPath(Url.Content("~/Resimler")), _yol);

                    Cevap5.SaveAs(path);
                }
            }
            if (Cevap6 != null)
            {
                if (Cevap6.ContentLength > 0)
                {
                    //string guid = Cevap1.FileName.Replace(Path.GetExtension(Cevap1.FileName), "") + "_" + Kod;// 
                    string guid = id + "_" + Guid.NewGuid().ToString();
                    string[] fileName = Path.GetFileName(Cevap6.FileName).Split('.');
                    string _yol = guid + "." + fileName[fileName.Length - 1];
                    _Cevap6 = _yol;
                    var path = Path.Combine(Server.MapPath(Url.Content("~/Resimler")), _yol);

                    Cevap6.SaveAs(path);
                }
            }
            if (Cevap7 != null)
            {
                if (Cevap7.ContentLength > 0)
                {
                    //string guid = Cevap1.FileName.Replace(Path.GetExtension(Cevap1.FileName), "") + "_" + Kod;// 
                    string guid = id + "_" + Guid.NewGuid().ToString();
                    string[] fileName = Path.GetFileName(Cevap7.FileName).Split('.');
                    string _yol = guid + "." + fileName[fileName.Length - 1];
                    _Cevap7 = _yol;
                    var path = Path.Combine(Server.MapPath(Url.Content("~/Resimler")), _yol);

                    Cevap7.SaveAs(path);
                }
            }
            if (Cevap8 != null)
            {
                if (Cevap8.ContentLength > 0)
                {
                    //string guid = Cevap1.FileName.Replace(Path.GetExtension(Cevap1.FileName), "") + "_" + Kod;// 
                    string guid = id + "_" + Guid.NewGuid().ToString();
                    string[] fileName = Path.GetFileName(Cevap8.FileName).Split('.');
                    string _yol = guid + "." + fileName[fileName.Length - 1];
                    _Cevap8 = _yol;
                    var path = Path.Combine(Server.MapPath(Url.Content("~/Resimler")), _yol);

                    Cevap8.SaveAs(path);
                }
            }
            */
            #endregion
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BakimKapat";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Durum", Durum);
            cmd.Parameters.AddWithValue("@SeriNo", SeriNo);
            cmd.Parameters.AddWithValue("@Sikayet", "");
            cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
            cmd.Parameters.AddWithValue("@YetkiliIsim", YetkiliIsim);
            cmd.Parameters.AddWithValue("@Imza", Imza);

            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return RedirectToAction("BakimYap", "YKMakine", new { id = id });
        }

        [HttpGet]
        public ActionResult BakimYap(int id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/Kullanici/Giris");

            string Tip = "Bakım";
            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = @"SELECT ID, Kod, Deger, Tip FROM GrupKodlari WITH(NOLOCK) WHERE(Tip = 'ServisIslemTipleri')";
                cmd2.CommandType = System.Data.CommandType.Text;

                DataTable dtIslemTipleri = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
                ViewBag.IslemTipleri = dtIslemTipleri;
            }
            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "p_SorulariGetir";
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@Tip", Tip);
                cmd2.Parameters.AddWithValue("@ServisID", id);
                DataTable dtSorular = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
                ViewBag.Sorular = dtSorular;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BakimListemTek";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View(dt);
        }

        [HttpGet]
        public ActionResult BakimListem(DateTime? Tarih = null, string Barkod = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/Kullanici/Giris");

            if (Tarih == null)
                Tarih = DateTime.Today;
            ViewBag.Tarih = Convert.ToDateTime(Tarih).ToString("yyyy-MM-dd");
            ViewBag.Barkod = Barkod;

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = @"SELECT ID, Kod, Deger, Tip
FROM GrupKodlari
WHERE(Tip = 'ServisIslemTipleri')";
            cmd2.CommandType = System.Data.CommandType.Text;
            DataTable dtIslemTipleri = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
            ViewBag.IslemTipleri = dtIslemTipleri;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BakimListem";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Tarih", Convert.ToDateTime(Tarih).ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@Barkod", Barkod);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View(dt);
        }
        #endregion

        #region Cari Cihazları
        [HttpGet]
        public ActionResult CariCihazlari(string Isim = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_CariCihazListesi";
            cmd.Parameters.AddWithValue("@Isim", Isim);
            DataTable dt = (DataTable)Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }
        [HttpGet]
        public ActionResult CariCihazlariDetay(int id = 0, string Isim = "", string Stok = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_CariCihazListesiDetay";
            cmd.Parameters.AddWithValue("@Isim", Isim);
            cmd.Parameters.AddWithValue("@Stok", Stok);
            DataTable dt = (DataTable)Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }
        [HttpPost]
        public ActionResult CariCihazEkle(int id, int CariID, string Sube, string Bolum,
            string Marka, string Model, string Isim, string Seri)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            if (id == 0)
            {
                cmd.CommandText = "Insert Into Cihazlar (CariID,Sube,Bolum,Marka,Model,Isim,Seri) values (@CariID,@Sube,@Bolum,@Marka,@Model,@Isim,@Seri)";
                cmd.Parameters.AddWithValue("@CariID", CariID);
                cmd.Parameters.AddWithValue("@Sube", Sube);
                cmd.Parameters.AddWithValue("@Bolum", Bolum);
                cmd.Parameters.AddWithValue("@Marka", Marka);
                cmd.Parameters.AddWithValue("@Model", Model);
                cmd.Parameters.AddWithValue("@Isim", Isim);
                cmd.Parameters.AddWithValue("@Seri", Seri);
            }
            else
            {
                cmd.CommandText = "Update Cihazlar set Sube=@Sube,Bolum=@Bolum,Marka=@Marka,Model=@Model,Isim=@Isim,Seri=@Seri Where ID = @ID";
                cmd.Parameters.AddWithValue("@Sube", Sube);
                cmd.Parameters.AddWithValue("@Bolum", Bolum);
                cmd.Parameters.AddWithValue("@Marka", Marka);
                cmd.Parameters.AddWithValue("@Model", Model);
                cmd.Parameters.AddWithValue("@Isim", Isim);
                cmd.Parameters.AddWithValue("@Seri", Seri);
                cmd.Parameters.AddWithValue("@ID", id);
            }
            Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/YKMakine/CariCihazlariDetay/?id=0&CariId=" + CariID);
        }

        #endregion

        #region Soru İşlemleri

        [HttpGet]
        public ActionResult SoruListesi(string Isim = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~" + ConfigurationManager.AppSettings["ISSIcIsim"] + "/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = @"select
K.Deger as Kategori,
S.* 
from Sorular as S WITH(NOLOCK) 
left outer join Kategoriler as K WITH(NOLOCK) on K.ID = S.KategoriID 
Where S.Soru like '%'+@Soru+'%'";
            cmd.Parameters.AddWithValue("@Soru", Isim);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            return View(dt);
        }

        [HttpGet]
        public ActionResult Soru(int id = 0)
        {
            if (!AutoGirisKontrol())
                return Redirect("~" + ConfigurationManager.AppSettings["ISSIcIsim"] + "/YK/Giris");

            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Select * from Sorular WITH(NOLOCK) Where ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandType = System.Data.CommandType.Text;
            cmd2.CommandText = "select * from Kategoriler WITH(NOLOCK) Order By Deger";
            ViewBag.dtKategoriler = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);

            ViewBag.id = id;

            return View(dt);
        }
        [HttpPost]
        public ActionResult Soru(int id,
            string Modul, string SoruSirasi, string KategoriID, string Soru, string Dosya,
            string TestTipi1, string TestTipi2, string TestTipi3, string TestTipi4, string TestTipi5,
            string TestSonucu1, string TestSonucu2, string TestSonucu3, string TestSonucu4, string TestSonucu5
            )
        {
            if (!AutoGirisKontrol())
                return Redirect("~" + ConfigurationManager.AppSettings["ISSIcIsim"] + "/YK/Giris");

            string _KullaniciAdi = GetCookie("KullaniciAdi");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            if (id == 0)
            {
                cmd.CommandText = "Insert Into Sorular (Modul,SoruSirasi,KategoriID,Soru,Dosya,TestTipi1,TestTipi2,TestTipi3,TestTipi4,TestTipi5,TestSonucu1,TestSonucu2,TestSonucu3,TestSonucu4,TestSonucu5,KayitTarihi,KayitYapanKullanici) values (@Modul,@SoruSirasi,@KategoriID,@Soru,@Dosya,@TestTipi1,@TestTipi2,@TestTipi3,@TestTipi4,@TestTipi5,@TestSonucu1,@TestSonucu2,@TestSonucu3,@TestSonucu4,@TestSonucu5,GETDATE(),@Kullanici) select SCOPE_IDENTITY()";
                cmd.Parameters.AddWithValue("@Modul", Modul);
                cmd.Parameters.AddWithValue("@SoruSirasi", SoruSirasi);
                cmd.Parameters.AddWithValue("@KategoriID", KategoriID);
                cmd.Parameters.AddWithValue("@Soru", Soru);
                cmd.Parameters.AddWithValue("@Dosya", Dosya == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi1", TestTipi1 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi2", TestTipi2 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi3", TestTipi3 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi4", TestTipi4 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi5", TestTipi5 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu1", TestSonucu1 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu2", TestSonucu2 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu3", TestSonucu3 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu4", TestSonucu4 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu5", TestSonucu5 == null ? false : true);
                cmd.Parameters.AddWithValue("@Kullanici", _KullaniciAdi);
                id = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            else
            {
                cmd.CommandText = "Update Sorular set Modul=@Modul,SoruSirasi=@SoruSirasi,KategoriID=@KategoriID,Soru=@Soru,Dosya=@Dosya,TestTipi1=@TestTipi1,TestTipi2=@TestTipi2,TestTipi3=@TestTipi3,TestTipi4=@TestTipi4,TestTipi5=@TestTipi5,TestSonucu1=@TestSonucu1,TestSonucu2=@TestSonucu2,TestSonucu3=@TestSonucu3,TestSonucu4=@TestSonucu4,TestSonucu5=@TestSonucu5 Where ID = @ID";
                cmd.Parameters.AddWithValue("@Modul", Modul);
                cmd.Parameters.AddWithValue("@SoruSirasi", SoruSirasi);
                cmd.Parameters.AddWithValue("@KategoriID", KategoriID);
                cmd.Parameters.AddWithValue("@Soru", Soru);
                cmd.Parameters.AddWithValue("@Dosya", Dosya == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi1", TestTipi1 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi2", TestTipi2 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi3", TestTipi3 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi4", TestTipi4 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestTipi5", TestTipi5 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu1", TestSonucu1 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu2", TestSonucu2 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu3", TestSonucu3 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu4", TestSonucu4 == null ? false : true);
                cmd.Parameters.AddWithValue("@TestSonucu5", TestSonucu5 == null ? false : true);
                cmd.Parameters.AddWithValue("@ID", id);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }
            return Redirect("~/YKMakine/SoruListesi/" + id);
        }
        [HttpGet]
        public ActionResult SoruSil(int id = 0)
        {
            if (!AutoGirisKontrol())
                return Redirect("~" + ConfigurationManager.AppSettings["ISSIcIsim"] + "/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Delete From Sorular Where ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Redirect("~/YKMakine/SoruListesi");
        }
        #endregion

        #region Kategoriler
        [HttpGet]
        public ActionResult Kategoriler(int id = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from Kategoriler WITH(NOLOCK) Order by Sira,Deger";
            DataTable dt = (DataTable)Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandType = System.Data.CommandType.Text;
            cmd2.CommandText = "select * from Kategoriler WITH(NOLOCK) Where ID = @ID";
            cmd2.Parameters.AddWithValue("@ID", id);
            ViewBag.dtKayit = (DataTable)Models.IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
            ViewBag.id = id;
            return View(dt);
        }

        [HttpPost]
        public ActionResult KategoriEkle(int id, string Sira, string Deger)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            if (id == 0)
            {
                cmd.CommandText = "Insert Into Kategoriler (Modul,Sira,Deger) values (@Modul,@Sira,@Deger)";
                cmd.Parameters.AddWithValue("@Modul", "Bakım");
                cmd.Parameters.AddWithValue("@Sira", Sira);
                cmd.Parameters.AddWithValue("@Deger", Deger);
            }
            else
            {
                cmd.CommandText = "Update Kategoriler set Sira=@Sira,Deger=@deger Where ID = @ID";
                cmd.Parameters.AddWithValue("@Sira", Sira);
                cmd.Parameters.AddWithValue("@Deger", Deger);
                cmd.Parameters.AddWithValue("@ID", id);
            }
            Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/YKMakine/Kategoriler");
        }

        [HttpGet]
        public ActionResult KategoriSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Delete From Kategoriler Where ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/YKMakine/Kategoriler");
        }
        #endregion

        #region İzin Günleri
        [HttpGet]
        public ActionResult IzinGunleri(int id = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from IzinGunleri WITH(NOLOCK) Order by Baslangic, Bitis";
            DataTable dt = (DataTable)Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandType = System.Data.CommandType.Text;
            cmd2.CommandText = "select * from IzinGunleri WITH(NOLOCK) Where ID = @ID";
            cmd2.Parameters.AddWithValue("@ID", id);
            ViewBag.dtKayit = (DataTable)Models.IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
            ViewBag.id = id;
            return View(dt);
        }

        [HttpPost]
        public ActionResult IzinGunuEkle(int id, string Aciklama, DateTime Baslangic, DateTime Bitis)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            if (id == 0)
            {
                cmd.CommandText = "Insert Into IzinGunleri (Aciklama,Baslangic,Bitis) values (@Aciklama,@Baslangic,@Bitis)";
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
            }
            else
            {
                cmd.CommandText = "Update IzinGunleri set Aciklama=@Aciklama,Baslangic=@Baslangic,Bitis=@Bitis Where ID = @ID";
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@ID", id);
            }
            Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/YKMakine/IzinGunleri");
        }

        [HttpGet]
        public ActionResult IzinGunuSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Delete From IzinGunleri Where ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/YKMakine/IzinGunleri");
        }
        #endregion

        #region Tatil Günleri
        [HttpGet]
        public ActionResult TatilGunleri(int id = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from TatilGunleri WITH(NOLOCK) Order by Baslangic, Bitis";
            DataTable dt = (DataTable)Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandType = System.Data.CommandType.Text;
            cmd2.CommandText = "select * from TatilGunleri WITH(NOLOCK) Where ID = @ID";
            cmd2.Parameters.AddWithValue("@ID", id);
            ViewBag.dtKayit = (DataTable)Models.IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
            ViewBag.id = id;
            return View(dt);
        }

        [HttpPost]
        public ActionResult TatilGunuEkle(int id, string Aciklama, DateTime Baslangic, DateTime Bitis)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            if (id == 0)
            {
                cmd.CommandText = "Insert Into TatilGunleri (Aciklama,Baslangic,Bitis) values (@Aciklama,@Baslangic,@Bitis)";
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
            }
            else
            {
                cmd.CommandText = "Update TatilGunleri set Aciklama=@Aciklama,Baslangic=@Baslangic,Bitis=@Bitis Where ID = @ID";
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@ID", id);
            }
            Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/YKMakine/TatilGunleri");
        }

        [HttpGet]
        public ActionResult TatilGunuSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Delete From TatilGunleri Where ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/YKMakine/TatilGunleri");
        }
        #endregion

        #region Cari Sıralaması
        [HttpGet]
        public ActionResult CariSiralama()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from IDW_Servis_CariListesi Order by Sira,Il,Ilce,Adres";
            DataTable dt = (DataTable)Models.IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult CariSiralamaKaydet(string CariKodu, int Sira)
        {
            YKJsonResult result = new YKJsonResult();
            try
            {
                string _KullaniciAdi = GetCookie("KullaniciAdi");

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"
IF NOT EXISTS(Select * from CariSiralamalari WITH(NOLOCK) Where CariKodu = @CariKodu)
BEGIN
Insert Into CariSiralamalari 
(CariKodu,Sira,KayitTarihi,KayitYapanKullanici) 
values 
(@CariKodu,@Sira,GETDATE(),@Kullanici)
END
ELSE
BEGIN
Update CariSiralamalari set Sira = @Sira Where CariKodu = @CariKodu
END
";
                cmd.Parameters.AddWithValue("@CariKodu", CariKodu);
                cmd.Parameters.AddWithValue("@Sira", Sira);
                cmd.Parameters.AddWithValue("@Kullanici", _KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                result.SonucKodu = "1";
                result.Aciklama = "";
            }
            catch (Exception err)
            {
                result.SonucKodu = "-1";
                result.Aciklama = err.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Giriş İşlemleri
        [HttpGet]
        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(string KullaniciAdi, string Parola, string returnUrl)
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
                CreateCookie("Isim", Convert.ToString(dt.Rows[0]["Isim"]));
                CreateCookie("KullaniciID", Convert.ToString(dt.Rows[0]["ID"]));
                CreateCookie("KullaniciAdi", Convert.ToString(dt.Rows[0]["KullaniciAdi"]));
                CreateCookie("Parola", Convert.ToString(dt.Rows[0]["Parola"]));

                cmd.Parameters.Clear();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_KullaniciYetkileri";
                cmd.Parameters.AddWithValue("@ID", Convert.ToString(dt.Rows[0]["ID"]));
                DataTable dtYetkiler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                string sYetkiler = "";

                foreach (DataRow item in dtYetkiler.Rows)
                {
                    sYetkiler += item["ID"] + "=" + item["Isim"] + "=" + (Convert.ToBoolean(item["Yetki"]) ? "1" : "0") + "|";
                }

                CreateCookie("KullaniciYetkileri", sYetkiler);

                return Redirect("~" + ConfigurationManager.AppSettings["ISSIcIsim"] + "/YK/AnaSayfa");
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı adı veya parola yanlış.";
                return View();
            }

        }

        public ActionResult Cikis()
        {
            DeleteCookie("Isim");
            DeleteCookie("KullaniciID");
            DeleteCookie("KullaniciAdi");
            DeleteCookie("Parola");
            DeleteCookie("Yetkiler");

            return Redirect("~" + ConfigurationManager.AppSettings["ISSIcIsim"] + "/YK/Giris");

        }
        #endregion

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

    public class YKJsonResult
    {
        public object Data { get; set; }
        public string SonucKodu { get; set; }
        public string Aciklama { get; set; }
    }

    public class IDModel
    {
        public string ID { get; set; }
        public string Tarih { get; set; }
        public string CariKodu { get; set; }
        public string CariAdi { get; set; }
        public string Barkod { get; set; }
        public string Barkod2 { get; set; }
        public string StokKod { get; set; }
        public string StokIsim { get; set; }
        public string Fason { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public decimal Miktar { get; set; }
        public decimal ToplamMiktar { get; set; }
        public decimal Fiyat { get; set; }
        public decimal Kdv { get; set; }
        public decimal Iskonto { get; set; }
        public decimal Tutar { get; set; }
        public string Kullanici { get; set; }
        public string KullaniciKodu { get; set; }
        public string KullaniciAdi { get; set; }
    }

}