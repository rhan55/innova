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
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.Ziyaret.Controllers
{
    public class ZiyaretController : Controller
    {
        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            string plasiyerkisiti = " and w_Ziyaretler.kayityapankullanici = '" + KullaniciID + "' ";


            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) from w_Cariler where UyelikID = @UyelikID and silindi = 0 ";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.MusteriSayisi = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) from w_Ziyaretler where UyelikID = @UyelikID and tamamlandi = 1 and silindi = 0 " + plasiyerkisiti;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.ZiyaretSayisi = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) from w_Ziyaretler where UyelikID = @UyelikID and tamamlandi = 1 and teklifverildi = 1 and silindi = 0  " + plasiyerkisiti;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.TeklifVerildi = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) from w_Ziyaretler where silindi = 0 and UyelikID = @UyelikID " + plasiyerkisiti;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.ToplamIslem = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                cmd = new SqlCommand();
                cmd.CommandText = "select count(*) from w_Ziyaretler where silindi = 0 and UyelikID = @UyelikID and tamamlandi = 1 " + plasiyerkisiti;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.ToplamIslem += Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            DataTable dtZiyaretler = new DataTable();
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select top(150) w_Ziyaretler.*,w_Cariler.CariAdi from w_Ziyaretler inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID where w_Ziyaretler.silindi = 0 and w_Cariler.silindi = 0 and  w_Ziyaretler.UyelikID = @UyelikID " + plasiyerkisiti + "  order by Tamamlandi,w_Ziyaretler.tarih desc ";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                dtZiyaretler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            DataTable dtGrup1 = new DataTable();
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select top(30) tarih, count(*) adet
 from w_Ziyaretler
inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID
 Where w_Cariler.silindi = 0 and w_Ziyaretler.UyelikID = @UyelikID " + plasiyerkisiti + @" and w_Ziyaretler.tamamlandi = 0 and w_Ziyaretler.silindi = 0
 group by w_Ziyaretler.tarih
order by w_Ziyaretler.tarih desc
";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                dtGrup1 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ViewBag.dtGrup1 = dtGrup1;
            }

            DataTable dtGrup3 = new DataTable();
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select top(30) tarih, count(*) adet
 from w_Ziyaretler
inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID 
 Where w_Cariler.silindi = 0 and w_Ziyaretler.UyelikID = @UyelikID and w_Ziyaretler.teklifverildi = 1  " + plasiyerkisiti + @" and w_Ziyaretler.silindi = 0
 group by w_Ziyaretler.tarih
order by w_Ziyaretler.tarih desc
";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                dtGrup3 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ViewBag.dtGrup3 = dtGrup3;
            }

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) 
from w_Ziyaretler 
inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID 
where w_Cariler.silindi = 0 and w_Ziyaretler.UyelikID = @UyelikID and w_Ziyaretler.tamamlandi = 0 and w_Ziyaretler.silindi = 0 " + plasiyerkisiti;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.ToplamBekleyen = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }

            DataTable dtGrupGecikenler = new DataTable();
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select top(30) w_Ziyaretler.tarih, count(*) adet
 from w_Ziyaretler
inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID 
 Where w_Cariler.silindi = 0 and w_Ziyaretler.UyelikID = @UyelikID and w_Ziyaretler.silindi = 0 and w_Ziyaretler.tamamlandi = 0 and w_Ziyaretler.tarih < '" + DateTime.Today.ToString("yyyy-MM-dd") + @"' " + plasiyerkisiti + @"
 group by w_Ziyaretler.tarih
order by w_Ziyaretler.tarih desc
";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                dtGrupGecikenler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ViewBag.dtGrupGecikenler = dtGrupGecikenler;
            }

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) 
from w_Ziyaretler
inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID 
where w_Cariler.silindi = 0 and w_Ziyaretler.UyelikID = @UyelikID and w_Ziyaretler.tamamlandi = 0 and w_Ziyaretler.silindi = 0 " + plasiyerkisiti;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.ToplamBekleyen = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }

            DataTable dtGrup4 = new DataTable();

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select ZiyaretTipi,count(*) adet 
from w_Ziyaretler 
inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID
Where w_Cariler.silindi = 0 and w_Ziyaretler.UyelikID = @UyelikID and w_Ziyaretler.tamamlandi = 0  and w_Ziyaretler.silindi = 0
group by w_Ziyaretler.ZiyaretTipi
order by count(*) desc
";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                dtGrup4 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ViewBag.dtGrup4 = dtGrup4;
            }

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) adet 
from w_Ziyaretler 
inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID
Where w_Cariler.silindi = 0 and w_Ziyaretler.UyelikID = @UyelikID and w_Ziyaretler.tamamlandi = 0 " + plasiyerkisiti + @"  and w_Ziyaretler.tarih < '" + DateTime.Today.ToString("yyyy-MM-dd") + @"' and w_Ziyaretler.silindi = 0
order by count(*) desc
";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.ToplamGecikmis = IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek);

            }

            return View();
        }



        #region Müşteriler
        public ActionResult Musteriler(string id = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");

            SqlCommand cmd = new SqlCommand();
            if (id.Length == 1)
                cmd.CommandText = "select * from w_Cariler where UyelikID = @UyelikID and silindi = 0 and (CariAdi like '" + id.Replace(";", "").Replace("'", "") + "%' ) Order by CariAdi ";
            else
                cmd.CommandText = "select * from w_Cariler where UyelikID = @UyelikID and silindi = 0 and (CariAdi like '%" + id.Replace(";", "").Replace("'", "") + "%'  ) Order by CariAdi ";

            cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.Arama = id;
            return View(dt);
        }

        public void MusterilerExcel(string id = "")
        {
            string UyelikID = GetCookie("UyelikID");

            SqlCommand cmd = new SqlCommand();
            if (id.Length == 1)
                cmd.CommandText = "select * from w_Cariler where UyelikID = @UyelikID and silindi = 0 and (Isim like '" + id.Replace(";", "").Replace("'", "") + "%' ) Order by Isim ";
            else
                cmd.CommandText = "select * from w_Cariler where UyelikID = @UyelikID and silindi = 0 and (Isim like '" + id.Replace(";", "").Replace("'", "") + "%' or Il = '" + id.Replace(";", "").Replace("'", "") + "' or Ilce = '" + id.Replace(";", "").Replace("'", "") + "'  or Isim1 like '%" + id.Replace(";", "").Replace("'", "") + "%' ) Order by Isim ";

            cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.Arama = id;

            ExporttoExcel(dt);
        }

        #endregion

        #region Ziyaret

        [HttpGet]
        public ActionResult Ziyaret(string id = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");

            DataSet ds = new DataSet();
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from w_Cariler where UyelikID = @UyelikID and ID = @ID and silindi = 0";
                if (id == null)
                    cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ds.Tables.Add(dt);
            }
            string idler = "null,";
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from w_Ziyaretler where CariID = @ID and silindi = 0 order by Tamamlandi, tarih desc,kayittarihi desc";
                if (id == null)
                    cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ID", id);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ds.Tables.Add(dt);
                foreach (DataRow item in dt.Rows)
                {
                    idler += item["id"] + ",";
                }
                idler = idler.Substring(0, idler.Length - 1);
            }

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from dosyalar Where KayitID IN (select A1.ID from w_Ziyaretler as A1 Where A1.ID in (" + idler + ")) order by ID desc";
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ds.Tables.Add(dt);
            }

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select 'GENEL' Deger ";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.Urunler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select Deger from GrupKodlari WITH(NOLOCK) Where Kod = 'Crm_ZiyaretTipi' Order by Deger";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.ZiyaretTipi = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return View(ds);
        }


        [HttpPost]
        public ActionResult ZiyaretKaydet(int id = 0, int CariID = 0, string Tarih = null, string Baslik = null, string Aciklama = null, string TeklifVerildi = null, string Urun = null, IEnumerable<HttpPostedFileBase> Dosya = null, string YeniTarih = null, string YeniBaslik = null, string YeniAciklama = null, string Tip = "", string Baslangic = null, string Bitis = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            if (id == 0)
            {
                cmd.CommandText = @"
Insert Into w_Ziyaretler 
(UyelikID,CariID,tarih,aciklama,teklifverildi,urun,kayittarihi,kayityapankullanici,tamamlandi,baslik,silindi) 
values 
('" + UyelikID + "','" + CariID + "',@tarih,@aciklama,@teklifverildi,@urun,@kayittarihi,@kayityapankullanici,0,@baslik,0); select LAST_INSERT_ID() ";
                //cmd.Parameters.AddWithValue("@CariID", CariID);
                cmd.Parameters.AddWithValue("@tarih", Convert.ToDateTime(Tarih));
                cmd.Parameters.AddWithValue("@baslik", Baslik);
                cmd.Parameters.AddWithValue("@aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@teklifverildi", TeklifVerildi == null ? 0 : 1);
                cmd.Parameters.AddWithValue("@urun", Urun);
                cmd.Parameters.AddWithValue("@kayittarihi", DateTime.Now);
                cmd.Parameters.AddWithValue("@kayityapankullanici", KullaniciID);
                id = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            else
            {
                cmd.CommandText = @"
Update w_Ziyaretler set
kapanistarih=@tarih,aciklama2=@aciklama,teklifverildi=@teklifverildi,urun=@urun,duzenlemetarihi=@kayittarihi,duzenlemeyapankullanici=@kayityapankullanici,tamamlandi=1
Where id = @id
";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@tarih", DateTime.Today); // Convert.ToDateTime(Tarih));
                cmd.Parameters.AddWithValue("@aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@teklifverildi", TeklifVerildi == null ? 0 : 1);
                cmd.Parameters.AddWithValue("@urun", Urun);
                cmd.Parameters.AddWithValue("@kayittarihi", DateTime.Now);
                cmd.Parameters.AddWithValue("@kayityapankullanici", KullaniciID);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                if (YeniTarih != null && YeniTarih != "")
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"
Insert Into w_Ziyaretler 
(UyelikID,CariID,tarih,baslik,aciklama,teklifverildi,urun,kayittarihi,kayityapankullanici,tamamlandi) 
values 
('" + UyelikID + "','" + CariID + "',@tarih,@baslik,@aciklama,@teklifverildi,@urun,@kayittarihi,@kayityapankullanici,0); select LAST_INSERT_ID() ";
                    //cmd.Parameters.AddWithValue("@CariID", CariID);
                    cmd.Parameters.AddWithValue("@tarih", Convert.ToDateTime(YeniTarih));
                    cmd.Parameters.AddWithValue("@baslik", YeniBaslik);
                    cmd.Parameters.AddWithValue("@aciklama", YeniAciklama);
                    cmd.Parameters.AddWithValue("@teklifverildi", 0);
                    cmd.Parameters.AddWithValue("@urun", "");
                    cmd.Parameters.AddWithValue("@kayittarihi", DateTime.Now);
                    cmd.Parameters.AddWithValue("@kayityapankullanici", KullaniciID);
                    id = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                }
            }


            if (Dosya != null)
            {
                foreach (var dosya in Dosya)
                {
                    if (dosya != null)
                    {
                        string dosyaYolu = Path.GetFileName(dosya.FileName);
                        string yeniIsmi = "Dosya_" + Guid.NewGuid().ToString();
                        string uzanti = Path.GetExtension(dosya.FileName);
                        var yuklemeYeri = Path.Combine(Server.MapPath("~/Content/"), yeniIsmi + uzanti);
                        dosya.SaveAs(yuklemeYeri);

                        SqlCommand cmdDosya = new SqlCommand();
                        cmdDosya.CommandText = "Insert Into Dosyalar (Modul,Dosya,Isim,DosyaUzantisi,KayitTarihi,KayitYapanKullanici,KayitID) values (@Modul,@Dosya,@Isim,@DosyaUzantisi,@KayitTarihi,@KayitYapanKullanici,@KayitID) ";
                        cmdDosya.Parameters.AddWithValue("@Modul", "Ziyaret");
                        cmdDosya.Parameters.AddWithValue("@Dosya", yeniIsmi + uzanti);
                        cmdDosya.Parameters.AddWithValue("@Isim", dosya.FileName);
                        cmdDosya.Parameters.AddWithValue("@DosyaUzantisi", uzanti);
                        cmdDosya.Parameters.AddWithValue("@KayitTarihi", DateTime.Now);
                        cmdDosya.Parameters.AddWithValue("@KayitYapanKullanici", KullaniciID);
                        cmdDosya.Parameters.AddWithValue("@KayitID", id);
                        IDVeritabani.Sorgula(cmdDosya, SorgulaTuru.Bos);
                    }
                }
            }

            if (Tip == "BekleyenZiyaretler")
            {
                return Redirect("~/Home/BekleyenZiyaretler/?Baslangic=" + Baslangic + "&Bitis=" + Bitis);
            }
            if (Tip == "GunlukIslemler")
            {
                return Redirect("~/Home/GunlukIslemler/?Baslangic=" + Baslangic + "&Bitis=" + Bitis);
            }
            return Redirect("~/Home/Ziyaret/" + CariID);
        }

        #endregion

        #region Rapor

        public ActionResult BekleyenZiyaretler(string Baslangic = null, string Bitis = null, string ZiyaretTipi = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            if (Baslangic == null)
            {
                Baslangic = DateTime.Today.AddYears(-5).ToString("yyyy-MM-dd");
            }
            if (Bitis == null)
            {
                Bitis = DateTime.Today.AddYears(5).ToString("yyyy-MM-dd");
            }

            ViewBag.Baslagic = Baslangic;
            ViewBag.Bitis = Bitis;
            ViewBag.ZiyaretTipiFiltre = ZiyaretTipi;



            string plasiyerkisiti = " and w_Ziyaretler.kayityapankullanici = '" + KullaniciID + "' ";

            string baslikkisiti = "";
            if (ZiyaretTipi == "Diğer")
            {
                baslikkisiti = " and (w_Ziyaretler.Baslik IS NULL or w_Ziyaretler.Baslik = '') ";
            }
            else if (ZiyaretTipi != "")
            {
                baslikkisiti = " and w_Ziyaretler.Baslik = '" + ZiyaretTipi + "' ";
            }

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select  w_Ziyaretler.*,w_Cariler.Isim,w_Cariler.seviye from w_Ziyaretler inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID where w_Cariler.silindi = 0 and w_Ziyaretler.UyelikID = '" + UyelikID + "' and w_Ziyaretler.silindi = 0 and tamamlandi = 0 " + plasiyerkisiti + "  " + baslikkisiti + "  and tarih between '" + Baslangic + "' and '" + Bitis + "' order by IFNULL(w_Cariler.seviye,6),w_Ziyaretler.tarih ";
            DataTable dtBekleyenZiyaretler = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);


            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select  Isim from urunler where UyelikID = @UyelikID order by Isim";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.Urunler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select  Isim from ziyarettipi where UyelikID = @UyelikID order by Isim";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.ZiyaretTipi = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return View(dtBekleyenZiyaretler);

        }

        public void BekleyenZiyaretlerExcel(string Baslangic = null, string Bitis = null, string ZiyaretTipi = "")
        {
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            if (Baslangic == null)
            {
                Baslangic = DateTime.Today.AddYears(-5).ToString("yyyy-MM-dd");
            }
            if (Bitis == null)
            {
                Bitis = DateTime.Today.AddYears(5).ToString("yyyy-MM-dd");
            }

            ViewBag.Baslagic = Baslangic;
            ViewBag.Bitis = Bitis;
            ViewBag.ZiyaretTipi = ZiyaretTipi;



            string plasiyerkisiti = " and w_Ziyaretler.kayityapankullanici = '" + KullaniciID + "' ";

            string baslikkisiti = "";
            if (ZiyaretTipi != "")
            {
                baslikkisiti = " and w_Ziyaretler.Baslik = '" + ZiyaretTipi + "' ";
            }

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = @"select  
w_Ziyaretler.id as ZiyaretID,
w_Cariler.Isim as Musteri,
w_Ziyaretler.tarih,
w_Ziyaretler.kapanistarih,
w_Ziyaretler.urun,
w_Ziyaretler.baslik,
w_Ziyaretler.aciklama as Planlama,
w_Ziyaretler.aciklama2 as PlanlamaSonucu,
w_Ziyaretler.tamamlandi,
w_Ziyaretler.teklifverildi,
w_Cariler.seviye as MusteriSeviye,
w_Ziyaretler.kayittarihi,
w_Cariler.Adres,
w_Cariler.Telefon,
w_Cariler.mail,
w_Cariler.website,
w_Cariler.Unvan1 as Kisi1Unvan,
w_Cariler.Isim1 as Kisi1Isim,
w_Cariler.Telefon1 as Kisi1Telefon
from w_Ziyaretler inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID where w_Cariler.silindi = 0 and w_Ziyaretler.UyelikID = '" + UyelikID + "' and w_Ziyaretler.silindi = 0 and tamamlandi = 0 " + plasiyerkisiti + "  " + baslikkisiti + "  and tarih between '" + Baslangic + "' and '" + Bitis + "' order by IFNULL(w_Cariler.seviye,6),w_Ziyaretler.tarih ";
            DataTable dtBekleyenZiyaretler = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);


            ExporttoExcel(dtBekleyenZiyaretler);

        }

        public ActionResult TeklifVerilenler(string Baslangic = null, string Bitis = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            if (Baslangic == null)
            {
                Baslangic = DateTime.Today.AddYears(-5).ToString("yyyy-MM-dd");
            }
            if (Bitis == null)
            {
                Bitis = DateTime.Today.AddYears(5).ToString("yyyy-MM-dd");
            }

            ViewBag.Baslagic = Baslangic;
            ViewBag.Bitis = Bitis;

            string plasiyerkisiti = " and w_Ziyaretler.kayityapankullanici = '" + KullaniciID + "' ";


            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select  w_Ziyaretler.*,w_Cariler.Isim,w_Cariler.seviye from w_Ziyaretler inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID where w_Ziyaretler.UyelikID = '" + UyelikID + "'  and w_Ziyaretler.silindi = 0 and  teklifverildi = 1 " + plasiyerkisiti + "  and tarih between '" + Baslangic + "' and '" + Bitis + "' order by IFNULL(w_Cariler.seviye,6),w_Ziyaretler.tarih ";
            DataTable dtBekleyenZiyaretler = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select  Isim from urunler where UyelikID = @UyelikID order by Isim";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.Urunler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return View(dtBekleyenZiyaretler);

        }

        public ActionResult TamamlananZiyaretler(string Baslangic = null, string Bitis = null, string Teklif = "0")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");


            if (Baslangic == null)
            {
                Baslangic = DateTime.Today.AddMonths(-2).ToString("yyyy-MM-dd");
            }
            if (Bitis == null)
            {
                Bitis = DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd");
            }

            ViewBag.Baslagic = Baslangic;
            ViewBag.Bitis = Bitis;
            ViewBag.Teklif = Teklif;

            string plasiyerkisiti = " and w_Ziyaretler.kayityapankullanici = '" + KullaniciID + "' ";

            string teklifverildi = "";
            if (Teklif != "0")
            {
                teklifverildi = " and w_Ziyaretler.teklifverildi = '1' ";
            }

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select  w_Ziyaretler.*,w_Cariler.Isim,w_Cariler.seviye from w_Ziyaretler inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID where w_Ziyaretler.UyelikID = '" + UyelikID + "' and w_Ziyaretler.silindi = 0 and tamamlandi = 1 " + plasiyerkisiti + " " + teklifverildi + "  and CAST(w_Ziyaretler.DuzenlemeTarihi as DATE) between '" + Baslangic + "' and '" + Bitis + "' order by IFNULL(w_Cariler.seviye,6),w_Ziyaretler.tarih ";
            DataTable dtBekleyenZiyaretler = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);

            return View(dtBekleyenZiyaretler);

        }

        public ActionResult GunlukIslemler(string Baslangic = null, string Bitis = null, string Personel = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");


            if (Baslangic == null)
            {
                Baslangic = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
            }
            if (Bitis == null)
            {
                Bitis = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
            }


            ViewBag.Baslagic = Baslangic;
            ViewBag.Bitis = Bitis;
            ViewBag.Personel = Personel;

            string plasiyerkisiti = "";
            if (Personel != "")
            {
                plasiyerkisiti = " and w_Ziyaretler.kayityapankullanici = '" + Personel + "' ";
            }

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select  w_Ziyaretler.*,w_Cariler.Isim,w_Cariler.seviye from w_Ziyaretler inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID where w_Ziyaretler.UyelikID = '" + UyelikID + "' " + plasiyerkisiti + "   and w_Ziyaretler.silindi = 0 and  tarih between '" + Baslangic + "' and '" + Bitis + "' order by IFNULL(w_Cariler.seviye,6),w_Ziyaretler.tarih ";
            DataTable dtBekleyenZiyaretler = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);

            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select  Isim from urunler where UyelikID = @UyelikID order by Isim";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.Urunler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select  Isim from ziyarettipi where UyelikID = @UyelikID order by Isim";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.ZiyaretTipi = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            {

                string plasiyerkisiti2 = " and KullaniciAdi = '" + KullaniciID + "' ";


                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select KullaniciAdi,Isim from Kullanicilar Where UyelikID = @UyelikID " + plasiyerkisiti2 + " Order by KullaniciAdi";
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                ViewBag.Kullanicilar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return View(dtBekleyenZiyaretler);

        }

        public void GunlukIslemlerExcel(string Baslangic = null, string Bitis = null, string Personel = "")
        {
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            if (Baslangic == null)
            {
                Baslangic = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
            }
            if (Bitis == null)
            {
                Bitis = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
            }

            ViewBag.Baslagic = Baslangic;
            ViewBag.Bitis = Bitis;
            ViewBag.Personel = Personel;

            string plasiyerkisiti = "";
            if (Personel != "")
            {
                plasiyerkisiti = " and w_Ziyaretler.kayityapankullanici = '" + Personel + "' ";
            }

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = @"select  
w_Ziyaretler.id as ZiyaretID,
w_Cariler.Isim as Musteri,
w_Ziyaretler.tarih,
w_Ziyaretler.kapanistarih,
w_Ziyaretler.urun,
w_Ziyaretler.baslik,
w_Ziyaretler.aciklama as Planlama,
w_Ziyaretler.aciklama2 as PlanlamaSonucu,
w_Ziyaretler.tamamlandi,
w_Ziyaretler.teklifverildi,
w_Cariler.seviye as MusteriSeviye,
w_Ziyaretler.kayittarihi,
w_Cariler.Adres,
w_Cariler.Telefon,
w_Cariler.mail,
w_Cariler.website,
w_Cariler.Unvan1 as Kisi1Unvan,
w_Cariler.Isim1 as Kisi1Isim,
w_Cariler.Telefon1 as Kisi1Telefon
from w_Ziyaretler inner join w_Cariler on w_Ziyaretler.CariID = w_Cariler.ID where w_Ziyaretler.UyelikID = '" + UyelikID + "' " + plasiyerkisiti + "   and w_Ziyaretler.silindi = 0 and  tarih between '" + Baslangic + "' and '" + Bitis + "' order by IFNULL(w_Cariler.seviye,6),w_Ziyaretler.tarih ";
            DataTable dtBekleyenZiyaretler = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);


            ExporttoExcel(dtBekleyenZiyaretler);
        }

        private void ExporttoExcel(DataTable table)
        {
            if (true)
            {
                HttpContext.Response.Clear();
                HttpContext.Response.ClearContent();
                HttpContext.Response.ClearHeaders();
                HttpContext.Response.Buffer = true;
                HttpContext.Response.ContentType = "application/vnd.ms-excel";
                //HttpContext.Current.Response.ContentType = "application/ms-word";
                HttpContext.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");
                // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.doc");
                HttpContext.Response.Charset = "utf-8";
                HttpContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                HttpContext.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                HttpContext.Response.Write("<BR><BR><BR>");
                HttpContext.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                int columnscount = table.Columns.Count;

                for (int j = 0; j < columnscount; j++)
                {
                    HttpContext.Response.Write("<Td>");
                    HttpContext.Response.Write("<B>");
                    HttpContext.Response.Write(table.Columns[j].Caption.ToString());
                    HttpContext.Response.Write("</B>");
                    HttpContext.Response.Write("</Td>");
                }
                HttpContext.Response.Write("</TR>");
                foreach (DataRow row in table.Rows)
                {
                    HttpContext.Response.Write("<TR>");
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        HttpContext.Response.Write("<Td>");
                        HttpContext.Response.Write(row[i].ToString());
                        HttpContext.Response.Write("</Td>");
                    }

                    HttpContext.Response.Write("</TR>");
                }
                HttpContext.Response.Write("</Table>");
                HttpContext.Response.Write("</font>");
                HttpContext.Response.Flush();
                HttpContext.Response.End();
            }
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

}