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
using YKPortal.Models.YKClasses;

namespace YKPortal.Controllers
{
    public class StokController : Controller
    {
        #region Sayım İşlemleri

        public ActionResult HizliStokTanimlama()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            {
                var cmd = new SqlCommand();
                cmd.CommandText = "p_GrupKoduListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@UstID", DBNull.Value);
                cmd.Parameters.AddWithValue("@Kod", "CariGrupKod1");
                cmd.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.dtGrupKodu1 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "p_GrupKoduListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@UstID", DBNull.Value);
                cmd.Parameters.AddWithValue("@Kod", "CariGrupKod2");
                cmd.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.dtGrupKodu2 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return View();
        }

        [HttpPost]
        public JsonResult HizliStokKaydet(
                string Kod1, string Kod2,
                string StokKodu, string StokAdi, string OlcuBirimi,
                string Barkod1, string Barkod2, string Barkod3, HttpPostedFileBase Resim
            )
        {

            var cmd = new SqlCommand();

            cmd.Parameters.Clear();
            cmd.CommandText = "p_HizliStokKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod1", Kod1);
            cmd.Parameters.AddWithValue("@Kod2", Kod2);
            cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
            cmd.Parameters.AddWithValue("@StokAdi", StokAdi);
            cmd.Parameters.AddWithValue("@OlcuBirimi", OlcuBirimi);
            cmd.Parameters.AddWithValue("@Barkod1", Barkod1);
            cmd.Parameters.AddWithValue("@Barkod2", Barkod2);
            cmd.Parameters.AddWithValue("@Barkod3", Barkod3);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (Resim != null && Resim.ContentLength > 0)
            {
                try
                {
                    //Resim.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + StokKodu + "_" + Resim.FileName));
                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_N_StokResimKaydet";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                    cmd.Parameters.AddWithValue("@KayitID", StokKodu);
                    cmd.Parameters.AddWithValue("@Dosya", StokKodu + "_" + Resim.FileName);
                    byte[] file = new byte[Resim.ContentLength];
                    Resim.InputStream.Read(file, 0, file.Length);
                    cmd.Parameters.AddWithValue("@Resim", file);
                    cmd.Parameters.AddWithValue("@ResimBoyut", file.Length);
                    cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }
                catch(Exception err)
                {

                }
            }

            return Json("Stok kaydedildi.", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sayim()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var cmd = new SqlCommand();
            cmd.CommandText = "p_FirmaListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", "");
            ViewBag.dtFirmalar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }

        public JsonResult DepolariGetir(string Sube)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = "p_DepoListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", Sube);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            List<DepoDto> entities = new List<DepoDto>();
            foreach (DataRow dr in dt.Rows)
            {
                entities.Add(new DepoDto
                {
                    ID = Convert.ToString(dr["ID"]),
                    UyelikID = Convert.ToString(dr["UyelikID"]),
                    Kod = Convert.ToString(dr["Kod"]),
                    Isim = Convert.ToString(dr["Isim"])
                });
            }

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SubeleriGetir(string Firma)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = "p_SubeListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", Firma);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            List<SubeDto> entities = new List<SubeDto>();
            foreach (DataRow dr in dt.Rows)
            {
                entities.Add(new SubeDto
                {
                    ID = Convert.ToString(dr["ID"]),
                    UyelikID = Convert.ToString(dr["UyelikID"]),
                    Kod = Convert.ToString(dr["Kod"]),
                    Isim = Convert.ToString(dr["Isim"])
                });
            }

            return Json(entities, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SayimSil(string id)
        {

            var cmd = new SqlCommand();
            cmd.CommandText = "p_StokSayimSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            string bilgi = "ok";
            if (dtKayitlar.Rows.Count > 0)
            {
                if (Convert.ToString(dtKayitlar.Rows[0]["Bilgi"]).StartsWith("UYARI!"))
                {
                    bilgi = Convert.ToString(dtKayitlar.Rows[0]["Bilgi"]);
                }
            }
            return Json(bilgi, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SayimKaydet(DateTime Tarih, string Firma, string Sube, string Depo, string Barkod, string Stok, int Miktar)
        {
            if (Barkod.Contains("*"))
            {
                Miktar = Convert.ToInt32(Barkod.Split('*')[0]);
                Barkod = Convert.ToString(Barkod.Split('*')[1]);
            }

            var cmd = new SqlCommand();
            cmd.CommandText = "p_StokSayimKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Tarih", Tarih);
            cmd.Parameters.AddWithValue("@Firma", Firma);
            cmd.Parameters.AddWithValue("@Sube", Sube);
            cmd.Parameters.AddWithValue("@Depo", Depo);
            cmd.Parameters.AddWithValue("@Barkod", Barkod);
            cmd.Parameters.AddWithValue("@Stok", Stok);
            cmd.Parameters.AddWithValue("@Miktar", Miktar);
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            string bilgi = "ok";
            if (dtKayitlar.Rows.Count > 0)
            {
                if (Convert.ToString(dtKayitlar.Rows[0]["Bilgi"]).StartsWith("UYARI!"))
                {
                    bilgi = Convert.ToString(dtKayitlar.Rows[0]["Bilgi"]);
                }
            }
            return Json(bilgi, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SayimListesi(string Tarih, string Firma, string Sube, string Depo)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = "p_StokSayimListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Tarih", Convert.ToDateTime(Tarih));
            cmd.Parameters.AddWithValue("@Firma", Firma);
            cmd.Parameters.AddWithValue("@Sube", Sube);
            cmd.Parameters.AddWithValue("@Depo", Depo);
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            List<StokSayim> entities = new List<StokSayim>();
            foreach (DataRow dr in dt.Rows)
            {
                entities.Add(new StokSayim
                {
                    ID = Convert.ToString(dr["ID"]),
                    UyelikID = Convert.ToString(dr["UyelikID"]),
                    Tarih = Convert.ToDateTime(dr["Tarih"]),
                    Firma = Convert.ToString(dr["Firma"]),
                    Sube = Convert.ToString(dr["Sube"]),
                    Depo = Convert.ToString(dr["Depo"]),
                    Barkod = Convert.ToString(dr["Barkod"]),
                    StokID = Convert.ToString(dr["StokID"]),
                    StokKodu = Convert.ToString(dr["StokKodu"]),
                    StokAdi = Convert.ToString(dr["StokAdi"]).Replace("\n\r", ""),
                    Miktar = Convert.ToDecimal(dr["Miktar"])
                });
            }

            return Json(entities, JsonRequestBehavior.AllowGet);
        }
        #endregion

        // GET: Stok
        [HttpGet]
        public ActionResult Ekle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/Ekle", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }
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

            if (!YetkiKontrolu("/Stok/Ekle", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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

        public JsonResult Sil(StokDto stokDto)
        {
           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", stokDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TopluSil(List<string> idListesi)
        {
       
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

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Liste()
        {
          
            var model = new StokListeViewModel
            {
                Sil = YetkiKontrolu("/Stok/Liste", "Sil"),
                Duzenle = YetkiKontrolu("/Stok/Liste", "Duzenle")
            };
            StokGrupKod1ListesiniOlustur();
            StokGrupKod2ListesiniOlustur();

            return View(model);
        }


        [HttpPost]
        public JsonResult Liste(StokDto stokDto)
        { 
            var cmd = new SqlCommand();
            cmd.CommandText = "p_StokListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", stokDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", stokDto.Isim);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var result = new StokListeJsonModel()
            {
                Sil = YetkiKontrolu("/Stok/Liste", "Sil"),
                Duzenle = YetkiKontrolu("/Stok/Liste", "Duzenle")
            };

            foreach (DataRow dr in dt.Rows)
            {
                result.StokListesi.Add(new StokDto
                {
                    ID = Convert.ToString(dr["ID"]),
                    Isim = Convert.ToString(dr["Isim"]),
                    Kod = Convert.ToString(dr["Kod"]),
                    Aciklama = Convert.ToString(dr["Aciklama"])

                });
            }
          
            StokGrupKod1ListesiniOlustur();
            StokGrupKod2ListesiniOlustur();

            return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Duzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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

            if (!YetkiKontrolu("/Stok/Liste", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            if (!YetkiKontrolu("/Stok/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            ViewBag.StokID = StokID;
            return View();
        }

        [HttpPost]
        public ActionResult NotEkle(StokNotDto stokNotDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            if (!YetkiKontrolu("/Stok/Liste", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }
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
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            if (!YetkiKontrolu("/Stok/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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


            if (!YetkiKontrolu("/Stok/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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


            if (!YetkiKontrolu("/Stok/Liste", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }
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
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/Liste", "Sil"))
            {
                return Redirect("~/YK/Anasayfa");
            }
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

            if (!YetkiKontrolu("/Stok/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            return View();

        }
        [HttpPost]
        public ActionResult ExcelIceAktar(string tip, HttpPostedFileBase file)
        {
            if (!YetkiKontrolu("/Stok/Liste", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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

        [HttpGet]
        public ActionResult YeniStokHareketKaydi(string StokID, string KayitID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/YeniStokHareketKaydi", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            ViewBag.StokID = StokID;
            StokHareketTipiListesiniOlustur();
            DovizBirimleriListesiniOlustur();

            StokHareketDto entity = new StokHareketDto();
            if (StokID != null && KayitID != null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_StokHareketi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@StokID", StokID);
                cmd.Parameters.AddWithValue("@ID", KayitID);
                DataTable dtHareket = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dtHareket.Rows.Count > 0)
                {
                    entity.ID = Convert.ToString(dtHareket.Rows[0]["ID"]);
                    entity.StokID = Convert.ToString(dtHareket.Rows[0]["StokID"]);
                    entity.BelgeNo = Convert.ToString(dtHareket.Rows[0]["BelgeNo"]);
                    entity.Tarih = Convert.ToDateTime(dtHareket.Rows[0]["Tarih"]);
                    entity.VadeTarihi = Convert.ToDateTime(dtHareket.Rows[0]["VadeTarihi"]);
                    entity.HareketTipi = Convert.ToString(dtHareket.Rows[0]["HareketTipi"]);
                    entity.GC = Convert.ToString(dtHareket.Rows[0]["GC"]);
                    entity.Miktar = Convert.ToDecimal(dtHareket.Rows[0]["Miktar"]);
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
                entity.Miktar = 0;
                entity.DovizTipi = "TL";
            }

            return View(entity);
        }

        [HttpPost]
        public ActionResult YeniStokHareketKaydi(StokHareketDto stokHareketDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/YeniStokHareketKaydi", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokHareketiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", stokHareketDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@StokID", stokHareketDto.StokID);
            cmd.Parameters.AddWithValue("@Tarih", stokHareketDto.Tarih);
            cmd.Parameters.AddWithValue("@VadeTarihi", stokHareketDto.VadeTarihi);
            cmd.Parameters.AddWithValue("@BelgeNo", stokHareketDto.BelgeNo);
            cmd.Parameters.AddWithValue("@Aciklama", stokHareketDto.Aciklama);
            cmd.Parameters.AddWithValue("@HareketTipi", stokHareketDto.HareketTipi);
            cmd.Parameters.AddWithValue("@GC", stokHareketDto.GC);
            cmd.Parameters.AddWithValue("@Tutar", stokHareketDto.Tutar);
            cmd.Parameters.AddWithValue("@DovizTipi", stokHareketDto.DovizTipi);
            cmd.Parameters.AddWithValue("@Miktar", stokHareketDto.Miktar);
            cmd.Parameters.AddWithValue("@Kur", stokHareketDto.Kur);
            cmd.Parameters.AddWithValue("@DovizTutar", stokHareketDto.DovizTutar);
            cmd.Parameters.AddWithValue("@PlasiyerID", string.Empty);
            cmd.Parameters.AddWithValue("@BaglantiID", string.Empty);
            cmd.Parameters.AddWithValue("@Baglanti", stokHareketDto.Baglanti);
            cmd.Parameters.AddWithValue("@GrupKodu1ID", string.Empty);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", string.Empty);
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("HareketListesi", new { StokID = stokHareketDto.StokID });
        }

        [HttpGet]
        public ActionResult HareketListesi(StokHareketDto stokHareketDto)
        {

            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/HareketListesi", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            DataTable dt = new DataTable();

            if (stokHareketDto.StokID != string.Empty)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_StokHareketListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

                cmd.Parameters.AddWithValue("@StokID", stokHareketDto.StokID);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", string.Empty);
                cmd.Parameters.AddWithValue("@BitisTarihi", string.Empty);

                dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            else
            {
                dt = new DataTable();
            }

            if (stokHareketDto.StokID != string.Empty)
            {
                ViewBag.Stok = Getir(stokHareketDto.StokID);
            }

            return View(dt);
        }

        public ActionResult HareketSil(string KayitID, string StokID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/HareketListesi", "Sil"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokHareketiSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", KayitID);
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@StokID", StokID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("HareketListesi", new { StokID = StokID });
        }


        [HttpGet]
        public ActionResult FiyatEkle(string StokID, string ID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            ViewBag.StokID = StokID;
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Stok";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@ID", StokID);
                ViewBag.Isim = Convert.ToString(((DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo)).Rows[0]["Isim"]);
            }

            StokFiyatDto entity = new StokFiyatDto();
            if (ID != null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_StokFiyat";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@ID", ID);
                DataTable dtStokFiyat = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dtStokFiyat.Rows.Count > 0)
                {

                    entity.ID = Convert.ToString(dtStokFiyat.Rows[0]["ID"]);
                    entity.StokID = Convert.ToString(dtStokFiyat.Rows[0]["StokID"]);
                    entity.CariID = Convert.ToString(dtStokFiyat.Rows[0]["CariID"]);
                    entity.FiyatGrubu = Convert.ToString(dtStokFiyat.Rows[0]["FiyatGrubu"]);
                    entity.Tip = Convert.ToString(dtStokFiyat.Rows[0]["Tip"]);
                    entity.Fiyat = Convert.ToDecimal(dtStokFiyat.Rows[0]["Fiyat"]);
                    entity.BaslangicTarihi = Convert.ToDateTime(dtStokFiyat.Rows[0]["BaslangicTarihi"]);
                    entity.BitisTarihi = Convert.ToDateTime(dtStokFiyat.Rows[0]["BitisTarihi"]);
                }
            }
            else
            {
                entity.BaslangicTarihi = DateTime.Today;
                entity.BitisTarihi = new DateTime(DateTime.Now.Year, 12, 31);
                entity.Fiyat = 0;

            }

            return View(entity);
        }


        [HttpPost]
        public ActionResult FiyatEkle(StokFiyatDto stokFiyatDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/Liste", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokFiyatKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", stokFiyatDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@StokID", stokFiyatDto.StokID);
            cmd.Parameters.AddWithValue("@CariID", null);
            cmd.Parameters.AddWithValue("@FiyatGrubu", stokFiyatDto.FiyatGrubu);
            cmd.Parameters.AddWithValue("@Tip", stokFiyatDto.Tip);
            cmd.Parameters.AddWithValue("@Fiyat", stokFiyatDto.Fiyat);
            cmd.Parameters.AddWithValue("@BaslangicTarihi", stokFiyatDto.BaslangicTarihi);
            cmd.Parameters.AddWithValue("@BitisTarihi ", stokFiyatDto.BitisTarihi);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));



            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("FiyatListesi", new { StokID = stokFiyatDto.StokID });
        }

        [HttpGet]
        public ActionResult FiyatListesi(string StokID)
        {

            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            DataTable dt = new DataTable();
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Stok";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@ID", StokID);
                ViewBag.Isim = Convert.ToString(((DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo)).Rows[0]["Isim"]);

            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_StokFiyatListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@StokID", StokID);
                dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            ViewBag.StokID = StokID;
            return View(dt);
        }



        public ActionResult FiyatSil(string id, string StokID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Stok/FiyatListesi", "Sil"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokFiyatSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@StokID", StokID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Redirect("~/Stok/FiyatListesi/?StokID=" + StokID);
        }
        bool Calis_SelectStokBarkodSeriListe = false;
        public JsonResult SelectStokBarkodSeriListe(string Barkod)
        {
            var liste = new List<StokDto>();
            try
            {
                if (Calis_SelectStokBarkodSeriListe == false)
                {
                    Calis_SelectStokBarkodSeriListe = true;

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "p_StokBarkodSerileri";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                    cmd.Parameters.AddWithValue("@Barkod", Barkod);

                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        liste.Add(new StokDto
                        {
                            SeriNo = Convert.ToString(dt.Rows[i]["SeriNo"]),
                        });
                    }

                }
            }
            finally
            {
                Calis_SelectStokBarkodSeriListe = false;
            }

            return Json(liste, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SelectStokSeriListe(string StokID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokBarkodSerileri";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Barkod", StokID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<StokDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToDecimal(dt.Rows[i]["Bakiye"]) > 0)
                {
                    liste.Add(new StokDto
                    {
                        SeriNo = Convert.ToString(dt.Rows[i]["SeriNo"]),
                        Bakiye = Convert.ToDecimal(dt.Rows[i]["Miktar"]),
                    });
                }
            }

            return Json(liste, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SelectStok(string search)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokBul";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Barkod", search);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<StokDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new StokDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Isim = Convert.ToString(dt.Rows[i]["Isim"]),
                    Aciklama = Convert.ToString(dt.Rows[i]["Isim2"]),
                    GrupKodu1ID = Convert.ToString(dt.Rows[i]["KategoriKodu1"]),
                    GrupKodu2ID = Convert.ToString(dt.Rows[i]["KategoriKodu2"]),
                    OlcuBirimi = Convert.ToString(dt.Rows[i]["OlcuBirimi"]),
                    Barkod = Convert.ToString(dt.Rows[i]["BARKOD"]),
                    Barkod2 = Convert.ToString(dt.Rows[i]["BARKOD2"]),
                    Barkod3 = Convert.ToString(dt.Rows[i]["BARKOD3"]),
                    Kod = Convert.ToString(dt.Rows[i]["Kod"]),
                    Dosya = dt.Rows[i]["Dosya"] == DBNull.Value ? "" : Convert.ToBase64String((byte[])dt.Rows[i]["Dosya"]),
                });
                break;
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
                    Aciklama = Convert.ToString(dt.Rows[i]["Aciklama"]),
                    Kod = Convert.ToString(dt.Rows[i]["Kod"]),
                });
            }

            return Json(liste, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CariSelectListe(string search)
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
        public void StokHareketTipiListesiniOlustur()
        {
            //GrupKodu1 Listesi oluşturma
            SqlCommand stokHareketTipiCommand = new SqlCommand();
            stokHareketTipiCommand.CommandText = "p_GrupKoduListesi";
            stokHareketTipiCommand.CommandType = System.Data.CommandType.StoredProcedure;
            stokHareketTipiCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokHareketTipiCommand.Parameters.AddWithValue("@Kod", "StokHareketTipi");
            stokHareketTipiCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokHareketTipiDataTable = (DataTable)IDVeritabani.Sorgula(stokHareketTipiCommand, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokHareketTipiDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokHareketTipiDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokHareketTipiDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokHareketTipleri = entities;
        }

        public void DovizBirimleriListesiniOlustur()
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
