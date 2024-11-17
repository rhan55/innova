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
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select Aciklama2 from Kullanicilar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and ID = @KullaniciID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                ViewBag.KullaniciAciklama2 = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) as Sayi1 from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and AlanKullanici = @KullaniciID and Sozlesme = 0";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                ViewBag.Havuzum = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }

            return View();
        }

        public ActionResult AnaSayfa2(string Menu = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select Aciklama2 from Kullanicilar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and ID = @KullaniciID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                ViewBag.KullaniciAciklama2 = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) as Sayi1 from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and AlanKullanici = @KullaniciID and Sozlesme = 0";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                ViewBag.Havuzum = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) as Sayi1 from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and AlanKullanici IS NULL and Sozlesme = 0";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                ViewBag.BosHavuz = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) as Sayi1 from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID  and Sozlesme = 1 --and AlanKullanici = @KullaniciID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                ViewBag.Sozlesme = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) as Sayi1 from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                ViewBag.Tumu = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) as Sayi1 from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and DATEDIFF(DAY,KabulTarihi,GETDATE()) > 90";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                ViewBag.Gun90 = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }

            ViewBag.Menu = Menu;
            return View();
        }

        public ActionResult Bilgilendirme(string Mesaj)
        {
            ViewBag.Mesaj = Mesaj;
            return View();
        }


        [HttpPost]
        public JsonResult IlceleriGetir(string UStID)
        {
            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");
            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_GrupKoduListesi";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@UstID", UStID);
            cmd.Parameters.AddWithValue("@Kod", "Ilce");
            cmd.Parameters.AddWithValue("@AranacakKelime", "");
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            foreach (DataRow item in dt.Rows)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(item["ID"]);
                entity.Deger = Convert.ToString(item["Deger"]);
                entities.Add(entity);
            }

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult MahalleleriGetir(string UStID)
        {
            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");
            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_GrupKoduListesi";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@UstID", UStID);
            cmd.Parameters.AddWithValue("@Kod", "Mahalle");
            cmd.Parameters.AddWithValue("@AranacakKelime", "");
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            foreach (DataRow item in dt.Rows)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(item["ID"]);
                entity.Deger = Convert.ToString(item["Deger"]);
                entities.Add(entity);
            }

            return Json(entities, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult CariSil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Update Crm2Kayitlar Set Silindi = 1, SilinenTarih=GETDATE(), SilenKullanici = @KullaniciID  Where ID = @ID";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Cariler", new { Area = "Crm2"});
        }

        [HttpGet]
        public ActionResult KabulEt(string KayitID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) as Sayi1 from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and AlanKullanici = @KullaniciID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                int kontrol = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                if (kontrol >= 100)
                {
                    string mesaj = "Havuzunuz 100 adet, daha fazla kayıt kabul edemezsiniz.";
                    return Redirect("~/Crm2/Crm2/Bilgilendirme/?Mesaj=" + mesaj);
                }
            }

            {
                GenelBilgilerGetir();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Update Crm2Kayitlar Set AlanKullanici = @KullaniciID,KabulTarihi=GETDATE() Where Silindi = 0 and UyelikID = @UyelikID and ID = @ID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                cmd.Parameters.AddWithValue("@ID", KayitID);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            return Redirect("~/Crm2/Crm2/CariDetay/?id=" + KayitID);
        }
        [HttpGet]
        public ActionResult Reddet(string KayitID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            GenelBilgilerGetir();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Update Crm2Kayitlar Set VerenKullanici=AlanKullanici,AlanKullanici = NULL,KabulTarihi=GETDATE() Where Silindi = 0 and UyelikID = @UyelikID and ID = @ID";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", KayitID);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Redirect("~/Crm2/Crm2/CariDetay/?id=" + KayitID);
        }
        [HttpGet]
        public ActionResult Sozlesme(string KayitID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            GenelBilgilerGetir();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Update Crm2Kayitlar Set Sozlesme=1 Where Silindi = 0 and UyelikID = @UyelikID and ID = @ID";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", KayitID);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Redirect("~/Crm2/Crm2/CariDetay/?id=" + KayitID);
        }

        [HttpPost]
        public ActionResult NotEkle(string KayitID, string Aciklama)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            GenelBilgilerGetir();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"Insert Into Crm2Notlar (UyelikID,KullaniciID,KayitID,Aciklama,KayitTarihi) 
values 
(@UyelikID,@KullaniciID,@KayitID,@Aciklama,GETDATE())";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@KayitID", KayitID);
            cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Redirect("~/Crm2/Crm2/CariDetay/?id=" + KayitID);
        }


        [HttpGet]
        public ActionResult CariDetay(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            GenelBilgilerGetir();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and ID = @ID";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", id);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "p_DosyaListesi";
                cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd2.Parameters.AddWithValue("@KayitID", id);
                cmd2.Parameters.AddWithValue("@Modul", "Crm2");
                DataTable dtDosyalar = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
                ViewBag.dtDosyalar = dtDosyalar;
            }
            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "p_DosyaListesi";
                cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd2.Parameters.AddWithValue("@KayitID", id);
                cmd2.Parameters.AddWithValue("@Modul", "Crm2Sozlesme");
                DataTable dtDosyalar = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
                ViewBag.dtDosyalarSozlesme = dtDosyalar;
            }
            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = @"Select 
Crm2Notlar.*,
Kullanicilar.Ad+' '+Kullanicilar.Soyad as Kullanici
from Crm2Notlar WITH(NOLOCK) 
LEFT OUTER JOIN Kullanicilar WITH(NOLOCK) ON Kullanicilar.ID = Crm2Notlar.KullaniciID
Where Crm2Notlar.UyelikID = @UyelikID and Crm2Notlar.KayitID = @KayitID Order by Crm2Notlar.KayitTarihi desc";
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd2.Parameters.AddWithValue("@KayitID", id);
                DataTable dtNotlar = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
                ViewBag.dtNotlar = dtNotlar;
            }
            return View(dt);
        }
        [HttpGet]
        public ActionResult CariDuzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            GenelBilgilerGetir();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and ID = @ID";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", id);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public JsonResult CariDuzenle(string id, string Bayi, DateTime Tarih, string ProjeTipi, string BlokSayisi,
            string Miktar, string Unvan, string Ad, string Soyad, string Telefon1, string Telefon2,
            string Gorev, string UlasimSekli, string Projeadi, string Il, string Ilce, string Mahalle,
            string Ada, string Parsel, string PortalNumarasi, string Resim, DateTime? SonucTarihi)
        {
            YKJsonResult result = new YKJsonResult();
            try
            {
                string SonID = id;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
                        Update Crm2Kayitlar Set
                            Bayi=@Bayi,Tarih=@Tarih,ProjeTipi=@ProjeTipi,BlokSayisi=@BlokSayisi,Miktar=@Miktar,
                            Unvan=@Unvan,Ad=@Ad,Soyad=@Soyad,Telefon1=@Telefon1,Telefon2=@Telefon2,Gorev=@Gorev,
                            UlasimSekli=@UlasimSekli,ProjeAdi=@Projeadi,Il=@Il,Ilce=@Ilce,Mahalle=@Mahalle,
                            Ada=@Ada,Parsel=@Parsel,PortalNumarasi=@PortalNumarasi,SonucTarihi=@SonucTarihi,
                            DuzenlemeTarihi=GETDATE(),DuzenlemeYapanKullanici=@KayitYapankullanici
                        Where ID = @ID
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
                if (SonucTarihi == null)
                    cmd.Parameters.AddWithValue("@SonucTarihi", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@SonucTarihi", SonucTarihi);
                cmd.Parameters.AddWithValue("@KayitYapankullanici", GetCookie("KullaniciID"));

                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.SonucKodu = "1";
                result.Aciklama = "Kayıt başarıyla güncellenmiştir.<br>Yönlendiriliyorsunuz...";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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
            string Ada, string Parsel, string PortalNumarasi,
            HttpPostedFileBase Resim, HttpPostedFileBase Resim2, HttpPostedFileBase Resim3, HttpPostedFileBase Resim4, HttpPostedFileBase Resim5, DateTime? SonucTarihi)
        {
            YKJsonResult result = new YKJsonResult();
            try
            {
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "select count(*) as Sayi1 from Crm2Kayitlar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and AlanKullanici = @KullaniciID";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                    cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                    int kontrol = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                    if (kontrol >= 100)
                    {
                        result.SonucKodu = "0";
                        result.Aciklama = "Havuzunuz 100 adet, daha fazla kayıt yapamazsınız.";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
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
                    cmd.Parameters.AddWithValue("@Resim", "");
                    if (SonucTarihi == null)
                        cmd.Parameters.AddWithValue("@SonucTarihi", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@SonucTarihi", SonucTarihi);
                    cmd.Parameters.AddWithValue("@KayitYapankullanici", GetCookie("KullaniciID"));
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                    if (Resim != null && Resim.ContentLength > 0)
                    {
                        Resim.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + Resim.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2");
                        cmd.Parameters.AddWithValue("@KayitID", SonID);
                        cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + Resim.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    if (Resim2 != null && Resim2.ContentLength > 0)
                    {
                        Resim2.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + Resim2.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2");
                        cmd.Parameters.AddWithValue("@KayitID", SonID);
                        cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + Resim2.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim2.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    if (Resim3 != null && Resim3.ContentLength > 0)
                    {
                        Resim3.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + Resim3.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2");
                        cmd.Parameters.AddWithValue("@KayitID", SonID);
                        cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + Resim3.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim3.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    if (Resim4 != null && Resim4.ContentLength > 0)
                    {
                        Resim4.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + Resim4.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2");
                        cmd.Parameters.AddWithValue("@KayitID", SonID);
                        cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + Resim4.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim4.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    if (Resim5 != null && Resim5.ContentLength > 0)
                    {
                        Resim5.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + Resim5.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2");
                        cmd.Parameters.AddWithValue("@KayitID", SonID);
                        cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + Resim5.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim5.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }







                    result.SonucKodu = "1";
                    result.Aciklama = "Kayıt başarıyla kaydedildi.<br>Yönlendiriliyorsunuz...";
                }



            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SozlesmeYukle(string ID,
          HttpPostedFileBase Resim, HttpPostedFileBase Resim2, HttpPostedFileBase Resim3, HttpPostedFileBase Resim4, HttpPostedFileBase Resim5)
        {
            YKJsonResult result = new YKJsonResult();
            try
            {
                SqlCommand cmd = new SqlCommand();
                {
                    if (Resim != null && Resim.ContentLength > 0)
                    {
                        Resim.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + ID + "_" + Resim.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2Sozlesme");
                        cmd.Parameters.AddWithValue("@KayitID", ID);
                        cmd.Parameters.AddWithValue("@Dosya", ID + "_" + Resim.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    if (Resim2 != null && Resim2.ContentLength > 0)
                    {
                        Resim2.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + ID + "_" + Resim2.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2Sozlesme");
                        cmd.Parameters.AddWithValue("@KayitID", ID);
                        cmd.Parameters.AddWithValue("@Dosya", ID + "_" + Resim2.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim2.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    if (Resim3 != null && Resim3.ContentLength > 0)
                    {
                        Resim3.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + ID + "_" + Resim3.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2Sozlesme");
                        cmd.Parameters.AddWithValue("@KayitID", ID);
                        cmd.Parameters.AddWithValue("@Dosya", ID + "_" + Resim3.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim3.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    if (Resim4 != null && Resim4.ContentLength > 0)
                    {
                        Resim4.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + ID + "_" + Resim4.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2Sozlesme");
                        cmd.Parameters.AddWithValue("@KayitID", ID);
                        cmd.Parameters.AddWithValue("@Dosya", ID + "_" + Resim4.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim4.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    if (Resim5 != null && Resim5.ContentLength > 0)
                    {
                        Resim5.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + ID + "_" + Resim5.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Crm2Sozlesme");
                        cmd.Parameters.AddWithValue("@KayitID", ID);
                        cmd.Parameters.AddWithValue("@Dosya", ID + "_" + Resim5.FileName);
                        cmd.Parameters.AddWithValue("@Isim", Resim5.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }


                    result.SonucKodu = "1";
                    result.Aciklama = "Sözleşme belgeleri başarıyla kaydedildi.<br>Yönlendiriliyorsunuz...";
                }

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
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Kullanici";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@ID", GetCookie("KullaniciID"));
                ViewBag.dtKullanici = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
        }

        [HttpGet]
        public ActionResult Cariler(string Tur = "", string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Crm2_Cariler";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Tur", Tur);
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);
            ViewBag.aranacakKelime = aranacakKelime;
            ViewBag.Tur = Tur;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "p_Kullanici";
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd2.Parameters.AddWithValue("@ID", GetCookie("KullaniciID"));
                ViewBag.dtKullanici = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
            }

            return View(dt);
        }

        [HttpGet]
        public ActionResult Cariler2(string Tur = "", string KullaniciID="", string aranacakKelime = "", bool Kullanici=false)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Crm2_Cariler";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
            cmd.Parameters.AddWithValue("@Tur", Tur);
            cmd.Parameters.AddWithValue("@AranacakKelime", Kullanici == true ? "XXXXXXXXXXYKXXXXXXXXXX" : aranacakKelime);
            ViewBag.aranacakKelime = aranacakKelime;
            ViewBag.Tur = Tur;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "p_Kullanici";
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd2.Parameters.AddWithValue("@ID", GetCookie("KullaniciID"));
                ViewBag.dtKullanici = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
            }

            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = @"
select 
* 
from (
	select 
	Kullanicilar.ID,
	Kullanicilar.Ad+' '+Kullanicilar.Soyad as Isim,
    Kullanicilar.Resim,
	(select COUNT(*) from Crm2Kayitlar WITH(NOLOCK) Where Crm2Kayitlar.Silindi = 0 and Crm2Kayitlar.Sozlesme = 0 and Crm2Kayitlar.AlanKullanici = Kullanicilar.ID) as Miktar from kullanicilar
) YK1 


";
                cmd2.CommandType = System.Data.CommandType.Text;
                DataTable dtKullanicilar = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
                ViewBag.dtKullanicilar = dtKullanicilar;
            }

            return View(dt);
        }
        public ActionResult Tanimlamalar()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            return View();
        }

        #region Grupkodları



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
        #endregion


        #region Grupkodları 2

        [HttpGet]
        public ActionResult GrupKodu2(string grupKodu, string UstID = "", string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@UstID", UstID);
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
        public ActionResult GrupKoduEkle2(string grupKodu, string UstID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            ViewBag.GrupKodu = grupKodu;

            return View();
        }

        [HttpPost]
        public ActionResult GrupKoduEkle2(GrupKoduDto grupKoduDto, string UstID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@UstID", UstID);
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu2", new { Area = "Crm2", grupKodu = grupKoduDto.Kod, UstID = UstID });
        }
        [HttpGet]
        public ActionResult GrupKoduDuzenle2(string grupKodu, string id, string UstID)
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
            ViewBag.UstID = UstID;
            return View(dt);
        }

        [HttpPost]
        public ActionResult GrupKoduDuzenle2(GrupKoduDto grupKoduDto, string UstID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", grupKoduDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", grupKoduDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@UstID", UstID);
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return RedirectToAction("GrupKodu2", new { Area = "Crm2", grupKodu = grupKoduDto.Kod, UstID = UstID });
        }

        [HttpPost]
        public ActionResult GrupKoduSil2(string id, string grupKodu, string UstID)
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

            return RedirectToAction("GrupKodu2", new { Area = "Crm2", grupKodu = grupKodu, UstID = UstID });
        }
        #endregion


        #region Grupkodları 3

        [HttpGet]
        public ActionResult GrupKodu3(string grupKodu, string UstID = "", string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@UstID", UstID);
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
        public ActionResult GrupKoduEkle3(string grupKodu, string UstID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            ViewBag.GrupKodu = grupKodu;

            return View();
        }

        [HttpPost]
        public ActionResult GrupKoduEkle3(GrupKoduDto grupKoduDto, string UstID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@UstID", UstID);
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu3", new { Area = "Crm2", grupKodu = grupKoduDto.Kod, UstID = UstID });
        }
        [HttpGet]
        public ActionResult GrupKoduDuzenle3(string grupKodu, string id, string UstID)
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
            ViewBag.UstID = UstID;
            return View(dt);
        }

        [HttpPost]
        public ActionResult GrupKoduDuzenle3(GrupKoduDto grupKoduDto, string UstID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", grupKoduDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", grupKoduDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@UstID", UstID);
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return RedirectToAction("GrupKodu3", new { Area = "Crm2", grupKodu = grupKoduDto.Kod, UstID = UstID });
        }

        [HttpPost]
        public ActionResult GrupKoduSil3(string id, string grupKodu, string UstID)
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

            return RedirectToAction("GrupKodu3", new { Area = "Crm2", grupKodu = grupKodu, UstID = UstID });
        }
        #endregion 


        [HttpGet]

        public ActionResult Kullanicilar(string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.AranacakKelime = aranacakKelime;
            IlListesiniOlustur();

            var iller = (List<GrupKoduDto>)ViewBag.Iller;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                string id = Convert.ToString(dt.Rows[i]["Il"]);
                var il = iller.Where(m => m.ID == id).ToList();
                if (il != null && il.Count > 0)
                {
                    dt.Rows[i]["Il"] = il[0].Deger;
                }
            }
            var model = new KullaniciListeViewModel
            {
                Kullanicilar = dt

            };

            return View(model);
        }

        [HttpGet]
        public ActionResult YeniKullanici()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            IlListesiniOlustur();

            return View();
        }

        [HttpPost]

        public ActionResult YeniKullanici(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {
            IlListesiniOlustur();
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            string imageName = "/Tema/Media/Logolar/amblem.png";
            var id = Guid.NewGuid();
            if (Resim != null && Resim.ContentLength > 0)
            {
                var extension = Resim.FileName.Split('.').Last();

                imageName = $"/Uploads/Avatarlar/{id}_{Guid.NewGuid()}.{extension}";
                Resim.SaveAs(Server.MapPath($"{imageName}"));
            }

            kullaniciEkleDto.UyelikID = GetCookie("UyelikID");

            kullaniciEkleDto.Kullanici = GetCookie("KullaniciID");

            if (kullaniciEkleDto.Aciklama1 == null)
            {
                kullaniciEkleDto.Aciklama1 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama2 == null)
            {
                kullaniciEkleDto.Aciklama2 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama3 == null)
            {
                kullaniciEkleDto.Aciklama3 = string.Empty;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Resim", $"{imageName}");
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", kullaniciEkleDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciEkleDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", kullaniciEkleDto.Parola);
            cmd.Parameters.AddWithValue("@Ad", kullaniciEkleDto.Ad);
            cmd.Parameters.AddWithValue("@Soyad", kullaniciEkleDto.Soyad);
            cmd.Parameters.AddWithValue("@Aktif", kullaniciEkleDto.Aktif);
            cmd.Parameters.AddWithValue("@Onay", kullaniciEkleDto.Onay);
            cmd.Parameters.AddWithValue("@Telefon", kullaniciEkleDto.Telefon);
            cmd.Parameters.AddWithValue("@Adres", kullaniciEkleDto.Adres);
            cmd.Parameters.AddWithValue("@Il", kullaniciEkleDto.Il);
            cmd.Parameters.AddWithValue("@Ilce", kullaniciEkleDto.Ilce);
            cmd.Parameters.AddWithValue("@Aciklama1", kullaniciEkleDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", kullaniciEkleDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", kullaniciEkleDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Kullanici", kullaniciEkleDto.Kullanici);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                string ID = Convert.ToString(dt.Rows[0]["ID"]);
                if (ID != "0")
                {
                    return Redirect("~/Crm2/Crm2/Kullanicilar");
                }
            }

            ViewBag.Form = kullaniciEkleDto;

            return View(dt);
        }

        [HttpGet]
        public ActionResult KullaniciDuzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            IlListesiniOlustur();
            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult KullaniciDuzenle(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            if (!ModelState.IsValid)
            {
                ViewBag.ValidationError = ModelState.Values
                                                        .SelectMany(m => m.Errors)
                                                        .Select(m => m.ErrorMessage).ToList();
                return KullaniciDuzenle(kullaniciEkleDto.ID);
            }


            kullaniciEkleDto.UyelikID = GetCookie("UyelikID");

            kullaniciEkleDto.Kullanici = GetCookie("KullaniciID");

            if (kullaniciEkleDto.Aciklama1 == null)
            {
                kullaniciEkleDto.Aciklama1 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama2 == null)
            {
                kullaniciEkleDto.Aciklama2 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama3 == null)
            {
                kullaniciEkleDto.Aciklama3 = string.Empty;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;



            if (Resim != null && Resim.ContentLength > 0)
            {
                var extension = Resim.FileName.Split('.').Last();

                var imageName = $"{kullaniciEkleDto.ID}_{Guid.NewGuid()}.{extension}";
                Resim.SaveAs(Server.MapPath($"/Uploads/Avatarlar/{imageName}"));
                cmd.Parameters.AddWithValue("@Resim", $"/Uploads/Avatarlar/{imageName}");
            }


            cmd.Parameters.AddWithValue("@ID", kullaniciEkleDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", kullaniciEkleDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciEkleDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", kullaniciEkleDto.Parola);
            cmd.Parameters.AddWithValue("@Ad", kullaniciEkleDto.Ad);
            cmd.Parameters.AddWithValue("@Soyad", kullaniciEkleDto.Soyad);
            cmd.Parameters.AddWithValue("@Aktif", kullaniciEkleDto.Aktif);
            cmd.Parameters.AddWithValue("@Telefon", kullaniciEkleDto.Telefon);
            cmd.Parameters.AddWithValue("@Adres", kullaniciEkleDto.Adres);
            cmd.Parameters.AddWithValue("@Il", kullaniciEkleDto.Il);
            cmd.Parameters.AddWithValue("@Ilce", kullaniciEkleDto.Ilce);
            cmd.Parameters.AddWithValue("@Aciklama1", kullaniciEkleDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", kullaniciEkleDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", kullaniciEkleDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Kullanici", kullaniciEkleDto.Kullanici);
            cmd.Parameters.AddWithValue("@Onay", kullaniciEkleDto.Onay);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            if (dt.Rows.Count > 0)
            {
                var columns = dt.Columns;
                if (columns.Contains("Bilgi"))
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (Bilgi == "Kullanıcı başarıyla oluşturuldu.")
                    {
                        return Redirect("~/Crm2/Crm2/Kullanicilar");
                    }
                    else
                    {
                        ViewBag.Bilgi = Bilgi;

                        var kullanici = KullaniciDuzenle(Convert.ToString(dt.Rows[0]["ID"]));

                        return Redirect("~/Crm2/Crm2/Kullanicilar");
                        //return View(kullanici);
                    }
                }

            }
            IlListesiniOlustur();
            return Redirect("~/Crm2/Crm2/Kullanicilar");
        }

        public void IlListesiniOlustur()
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

        [HttpPost]
        public ActionResult KullaniciSil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Redirect("~/Crm2/Crm2/Kullanicilar");
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