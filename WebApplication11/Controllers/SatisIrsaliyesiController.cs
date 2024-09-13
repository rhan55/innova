using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;
using iText.Html2pdf.Resolver.Font;
using iText.Html2pdf;
using System.IO;

namespace YKPortal.Controllers
{
    public class SatisIrsaliyesiController : Controller
    {
        public ActionResult Sil(string Tip = "", string ID = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/SatisIrsaliyesi/Liste/?Tip=SI", "Sil"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            if (Tip == "")
            {
                return Redirect("~/");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_BelgeSil";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Tip", Tip);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Redirect("~/SatisIrsaliyesi/Liste/?Tip=" + Tip);
        }

        public ActionResult Liste(BelgeDto belgeDto, string Tip = "", string AranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/SatisIrsaliyesi/Liste/?Tip=SI", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }
            if (string.IsNullOrEmpty(Tip))
            {
                return Redirect("~/");
            }


            var now = DateTime.Now;

            if (belgeDto.BaslangicTarihi <= DateTime.MinValue || belgeDto.BaslangicTarihi >= DateTime.MaxValue)
            {
                belgeDto.BaslangicTarihi = now.AddMonths(-1);
            }

            if (belgeDto.BitisTarihi <= DateTime.MinValue || belgeDto.BitisTarihi >= DateTime.MaxValue)
            {
                belgeDto.BitisTarihi = now.AddDays(1);
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_BelgeListesi";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Tip", Tip);
            cmd.Parameters.AddWithValue("@AranacakKelime", AranacakKelime);
            cmd.Parameters.AddWithValue("@BelgeNo", belgeDto.BelgeNo);
            cmd.Parameters.AddWithValue("@CariAdi", belgeDto.CariAdi);
            cmd.Parameters.AddWithValue("@Durumu", belgeDto.Durumu);
            cmd.Parameters.AddWithValue("@BaslangicTarihi", belgeDto.BaslangicTarihi.ToString("yyyy-MM-dd HH:mm"));
            cmd.Parameters.AddWithValue("@BitisTarihi", belgeDto.BitisTarihi.ToString("yyyy-MM-dd HH:mm"));


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var model = new BelgeListeViewModel
            {
                Belgeler = dt,
                Sil = YetkiKontrolu("/SatisIrsaliyesi/Liste/?Tip=SI", "Sil"),
                Duzenle = YetkiKontrolu("/SatisIrsaliyesi/Liste/?Tip=SI", "Duzenle")

            };
            ViewBag.Filters = belgeDto;
            ViewBag.Durumu = belgeDto.Durumu;
            return View(model);
        }



        [HttpGet]
        public ActionResult Detay()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/SatisIrsaliyesi/Detay/?Tip=SI", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }
            ViewBag.Personeller =SatisPersoneliGetir();
            ViewBag.Depolar = DepoListesiGetir();

            return View(new BelgeDto());
        }

        [HttpPost]
        public ActionResult Detay(string Tip, string id = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/SatisIrsaliyesi/Detay/?Tip=SI", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_Belge";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", id);
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
            BelgeDto entity = new BelgeDto();
            switch (Request.QueryString["Tip"])
            {
                case "SI":
                    entity.BelgeTipi = BelgeTipi.SatisFaturasi;
                    break;
                default:
                    break;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                entity.ID = Convert.ToString(ds.Tables[0].Rows[0]["ID"]);
                entity.Tarih = Convert.ToDateTime(ds.Tables[0].Rows[0]["Tarih"]);
                entity.BelgeNo = Convert.ToString(ds.Tables[0].Rows[0]["BelgeNo"]);
                entity.CariID = Convert.ToString(ds.Tables[0].Rows[0]["CariID"]);
                entity.CariAdi = Convert.ToString(ds.Tables[0].Rows[0]["CariAdi"]);
                entity.Durumu = Convert.ToString(ds.Tables[0].Rows[0]["Durumu"]);
                entity.SatisPersonelID = Convert.ToString(ds.Tables[0].Rows[0]["SatisPersonelID"]);
                entity.Aciklama = Convert.ToString(ds.Tables[0].Rows[0]["Aciklama1"]);
                entity.DepoCikisID = Convert.ToString(ds.Tables[0].Rows[0]["DepoCikisID"]);
                entity.DepoGirisID = Convert.ToString(ds.Tables[0].Rows[0]["DepoGirisID"]);
                entity.Kalemler = new List<BelgeKalemDto>();
                foreach (DataRow satir in ds.Tables[1].Rows)
                {
                    BelgeKalemDto s = new BelgeKalemDto();
                    s.ID = Convert.ToString(satir["ID"]);
                    s.BelgeID = entity.ID;
                    s.StokID = Convert.ToString(satir["StokID"]);
                    s.StokKodu = Convert.ToString(satir["StokKodu"]);
                    s.StokAdi = Convert.ToString(satir["StokAdi"]);
                    s.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    s.Seri = Convert.ToString(satir["Seri"]);
                    s.Miktar = Convert.ToDecimal(satir["Miktar"]);
                    s.Fiyat = Convert.ToDecimal(satir["Fiyat"]);
                    s.IskontoOrani1 = Convert.ToDecimal(satir["IskontoOrani1"]);
                    s.KdvOrani = Convert.ToDecimal(satir["KdvOrani"]);
                    s.Tutar = Convert.ToDecimal(satir["Tutar"]);

                    entity.Kalemler.Add(s);
                }
            }
            else
            {
                entity.Tarih = DateTime.Today;
            }

            {
                SqlCommand cmdDepolar = new SqlCommand();
                cmdDepolar.CommandType = System.Data.CommandType.StoredProcedure;
                cmdDepolar.CommandText = "p_DepoListesi";
                cmdDepolar.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmdDepolar.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.Depolar = (DataTable)IDVeritabani.Sorgula(cmdDepolar, SorgulaTuru.Tablo);
            }

            return View(entity);
        }

        [HttpPost]
        public ActionResult Kaydet(string Tip = "", string ID = "", string BelgeNo = "", string Tarih = "", string CariID = "", string SatisPersonelID = "", string Durumu = "",
           string DepoCikisID = "", string DepoGirisID = "",
            string Aciklama = "", List<BelgeKalemDto> Kalemler = null)
        {
            JsonResult result = new JsonResult();

            #region Kayıt işlemi gerçekleştirilecek.
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_BelgeKaydet";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Tip", Tip);
            cmd.Parameters.AddWithValue("@BelgeNo", BelgeNo);
            cmd.Parameters.AddWithValue("@Tarih", Tarih);
            cmd.Parameters.AddWithValue("@CariID", CariID);
            cmd.Parameters.AddWithValue("@Durumu", Durumu);
            cmd.Parameters.AddWithValue("@SatisPersonelID", SatisPersonelID);
            cmd.Parameters.AddWithValue("@DepoCikisID", DepoCikisID);
            cmd.Parameters.AddWithValue("@DepoGirisID", DepoGirisID);
            cmd.Parameters.AddWithValue("@Aciklama1", Aciklama);
            cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
            ID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

            string silinenler = "";
            if (Kalemler != null)
            {
                foreach (BelgeKalemDto item in Kalemler)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "p_BelgeKalemKaydet";
                    cmd.Parameters.AddWithValue("@ID", item.ID);
                    cmd.Parameters.AddWithValue("@BelgeID", ID);
                    cmd.Parameters.AddWithValue("@StokID", item.StokID);
                    cmd.Parameters.AddWithValue("@Seri", item.Seri);
                    cmd.Parameters.AddWithValue("@Miktar", Convert.ToDecimal(item.Miktar));
                    cmd.Parameters.AddWithValue("@Fiyat", Convert.ToDecimal(item.Fiyat));
                    cmd.Parameters.AddWithValue("@IskontoOrani1", Convert.ToDecimal(item.IskontoOrani1));
                    cmd.Parameters.AddWithValue("@KdvOrani", Convert.ToDecimal(item.KdvOrani));
                    cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                    silinenler += Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek)) + ",";
                }
            }


            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_BelgeKalemSilinenKontrol";
            cmd.Parameters.AddWithValue("@BelgeID", ID);
            cmd.Parameters.AddWithValue("@ID", silinenler);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_BelgeTamamla";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            #endregion

            result.Data = ID;


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Duzenle(string Tip, string id = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_Belge";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", id);
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
            BelgeDto entity = new BelgeDto();
            switch (Request.QueryString["Tip"])
            {
                case "SI":
                    entity.BelgeTipi = BelgeTipi.SatisIrsaliyesi;
                    break;
                default:
                    break;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                entity.ID = Convert.ToString(ds.Tables[0].Rows[0]["ID"]);
                entity.Tarih = Convert.ToDateTime(ds.Tables[0].Rows[0]["Tarih"]);
                entity.BelgeNo = Convert.ToString(ds.Tables[0].Rows[0]["BelgeNo"]);
                entity.CariID = Convert.ToString(ds.Tables[0].Rows[0]["CariID"]);
                entity.Durumu = Convert.ToString(ds.Tables[0].Rows[0]["Durumu"]);
                entity.SatisPersonelID = Convert.ToString(ds.Tables[0].Rows[0]["SatisPersonelID"]);
                entity.CariAdi = Convert.ToString(ds.Tables[0].Rows[0]["CariAdi"]);
                entity.Aciklama = Convert.ToString(ds.Tables[0].Rows[0]["Aciklama1"]);
                entity.Kalemler = new List<BelgeKalemDto>();
                foreach (DataRow satir in ds.Tables[1].Rows)
                {
                    BelgeKalemDto s = new BelgeKalemDto();
                    s.ID = Convert.ToString(satir["ID"]);
                    s.BelgeID = entity.ID;
                    s.StokID = Convert.ToString(satir["StokID"]);
                    s.StokKodu = Convert.ToString(satir["StokKodu"]);
                    s.StokAdi = Convert.ToString(satir["StokAdi"]);
                    s.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    s.Seri = Convert.ToString(satir["Seri"]);
                    s.Miktar = Convert.ToDecimal(satir["Miktar"]);
                    s.Fiyat = Convert.ToDecimal(satir["Fiyat"]);
                    s.IskontoOrani1 = Convert.ToDecimal(satir["IskontoOrani1"]);
                    s.KdvOrani = Convert.ToDecimal(satir["KdvOrani"]);
                    s.Tutar = Convert.ToDecimal(satir["Tutar"]);

                    entity.Kalemler.Add(s);
                }
            }
            else
            {
                entity.Tarih = DateTime.Today;
            }

            {
                SqlCommand cmdDepolar = new SqlCommand();
                cmdDepolar.CommandType = System.Data.CommandType.StoredProcedure;
                cmdDepolar.CommandText = "p_DepoListesi";
                cmdDepolar.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmdDepolar.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.Depolar = (DataTable)IDVeritabani.Sorgula(cmdDepolar, SorgulaTuru.Tablo);
            }

            ViewBag.Personeller = SatisPersoneliGetir();
            ViewBag.Duzenle = YetkiKontrolu("/SatisIrsaliyesi/Liste/?Tip=SI", "Duzenle");

            return View(entity);
        }


        [HttpPost]
        public ActionResult PdfOlustur(string Tip, string id = "")
        {

            var belge = BelgeGetir(Tip, id);
            var personel = SatisPersoneliGetir().Where(m => m.ID == belge.SatisPersonelID).FirstOrDefault();

            string htmlSource = System.IO.File.ReadAllText(Server.MapPath("~/PdfKaliplari/belge-tema.html"));
            string kalemler = string.Empty;

            for (var i = 0; i < belge.Kalemler.Count(); i++)
            {
                string kalemSource = System.IO.File.ReadAllText(Server.MapPath("~/PdfKaliplari/kalemler.html"))
                    .Replace("[NO]", (i + 1).ToString())
                    .Replace("[KALEM_ADI]", belge.Kalemler[i].StokAdi)
                    .Replace("[KALEM_BIRIM]", belge.Kalemler[i].OlcuBirimi)
                    .Replace("[KALEM_MIKTAR]", String.Format("{0:N2}", belge.Kalemler[i].Miktar))
                    .Replace("[KALEM_FIYAT]", String.Format("{0:N2}", belge.Kalemler[i].Fiyat))
                    .Replace("[KALEM_TUTAR]", String.Format("{0:N2}", belge.Kalemler[i].Tutar));

                kalemler = kalemler + kalemSource;
            }


            htmlSource = htmlSource.Replace("[CARI_ADI]", belge.CariAdi)
                                .Replace("[PERSONEL_ISIM]", personel?.Isim ?? string.Empty)
                                .Replace("[TITLE]", "Satış İrsaliyesi")
                                .Replace("[BELGE_NO]", belge.BelgeNo)
                                .Replace("[TARIH]", belge.Tarih.ToString("dd/MM/yyyy HH:mm:ss"))
                                .Replace("[ACIKLAMA]", belge.Aciklama)
                                .Replace("[ARA_TOPLAM]", String.Format("{0:N2}", belge.Kalemler.Select(m => m.Fiyat * m.Miktar).Sum()))
                                .Replace("[ISKONTO_TUTAR]", String.Format("{0:N2}", belge.Kalemler.Select(m => m.Fiyat * m.Miktar * m.IskontoOrani1 / 100).Sum()))
                                .Replace("[KDV_TUTAR]", String.Format("{0:N2}", belge.Kalemler.Select(m => m.Fiyat * m.Miktar * m.KdvOrani / 100).Sum()))
                                .Replace("[TOPLAM_TUTAR]", String.Format("{0:N2}", belge.Kalemler.Select(m => m.Fiyat * m.Miktar + (m.Fiyat * m.Miktar * m.KdvOrani / 100) - (m.Fiyat * m.Miktar * m.IskontoOrani1 / 100)).Sum()))
                                .Replace("[KALEMLER]", kalemler);

            var path = Server.MapPath("~/Uploads/Dosyalar/Teklifler");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var kaydedilecekYer = Server.MapPath("~/Uploads/Dosyalar/Teklifler/Satis-Irsaliyesi.pdf");

            using (var stream = new FileStream(kaydedilecekYer, FileMode.Create))
            {
                ConverterProperties properties = new ConverterProperties();
                properties.SetFontProvider(new DefaultFontProvider(true, true, true));
                HtmlConverter.ConvertToPdf(htmlSource, stream);
            }

            return File(kaydedilecekYer, "application/pdf", "Satis-Irsaliyesi.pdf");
        }


        private BelgeDto BelgeGetir(string Tip, string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_Belge";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", id);
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
            BelgeDto entity = new BelgeDto();
            switch (Tip)
            {
                case "ST":
                    entity.BelgeTipi = BelgeTipi.SatisTalebi;
                    break;
                default:
                    break;
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                entity.ID = Convert.ToString(ds.Tables[0].Rows[0]["ID"]);
                entity.Tarih = Convert.ToDateTime(ds.Tables[0].Rows[0]["Tarih"]);
                entity.BelgeNo = Convert.ToString(ds.Tables[0].Rows[0]["BelgeNo"]);
                entity.CariID = Convert.ToString(ds.Tables[0].Rows[0]["CariID"]);
                entity.Durumu = Convert.ToString(ds.Tables[0].Rows[0]["Durumu"]);
                entity.SatisPersonelID = Convert.ToString(ds.Tables[0].Rows[0]["SatisPersonelID"]);
                entity.CariAdi = Convert.ToString(ds.Tables[0].Rows[0]["CariAdi"]);
                entity.Aciklama = Convert.ToString(ds.Tables[0].Rows[0]["Aciklama1"]);
                entity.Kalemler = new List<BelgeKalemDto>();
                foreach (DataRow satir in ds.Tables[1].Rows)
                {
                    BelgeKalemDto s = new BelgeKalemDto();
                    s.ID = Convert.ToString(satir["ID"]);
                    s.BelgeID = entity.ID;
                    s.StokID = Convert.ToString(satir["StokID"]);
                    s.StokKodu = Convert.ToString(satir["StokKodu"]);
                    s.StokAdi = Convert.ToString(satir["StokAdi"]);
                    s.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    s.Seri = Convert.ToString(satir["Seri"]);
                    s.Miktar = Convert.ToDecimal(satir["Miktar"]);
                    s.Durumu = Convert.ToBoolean(satir["Durumu"]);
                    s.Fiyat = Convert.ToDecimal(satir["Fiyat"]);
                    s.IskontoOrani1 = Convert.ToDecimal(satir["IskontoOrani1"]);
                    s.KdvOrani = Convert.ToDecimal(satir["KdvOrani"]);
                    s.Tutar = Convert.ToDecimal(satir["Tutar"]);

                    entity.Kalemler.Add(s);
                }
            }
            else
            {
                entity.Tarih = DateTime.Today;
            }

            return entity;
        }


        [HttpPost]
        public JsonResult CariAra(string aranacakKelime)
        {
            JsonResult result = new JsonResult();
            List<CariDto> entities = new List<CariDto>();
            #region İşlemler

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", aranacakKelime);
            cmd.Parameters.AddWithValue("@Isim", aranacakKelime);
            cmd.Parameters.AddWithValue("@Unvan", aranacakKelime);
            cmd.Parameters.AddWithValue("@TCKimlikNo", aranacakKelime);
            cmd.Parameters.AddWithValue("@VergiNumarasi", "");
            cmd.Parameters.AddWithValue("@CepTelefonu", "");
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            foreach (DataRow satir in dt.Rows)
            {
                CariDto entity = new CariDto();
                entity.ID = Convert.ToString(satir["ID"]);
                entity.Kod = Convert.ToString(satir["Kod"]);
                entity.Isim = Convert.ToString(satir["Isim"]);
                entities.Add(entity);
            }
            #endregion
            result.Data = entities;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult StokAra(string aranacakKelime)
        {
            JsonResult result = new JsonResult();
            List<StokDto> entities = new List<StokDto>();
            #region İşlemler

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", aranacakKelime);
            cmd.Parameters.AddWithValue("@Isim", aranacakKelime);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            foreach (DataRow satir in dt.Rows)
            {
                StokDto entity = new StokDto();
                entity.ID = Convert.ToString(satir["ID"]);
                entity.Kod = Convert.ToString(satir["Kod"]);
                entity.Isim = Convert.ToString(satir["Isim"]);
                entities.Add(entity);
            }
            #endregion
            result.Data = entities;
            return Json(result, JsonRequestBehavior.AllowGet);
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
        private List<SatisPersonelleriDto> SatisPersoneliGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_PlasiyerListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var entities = new List<SatisPersonelleriDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SatisPersonelleriDto entity = new SatisPersonelleriDto();
                entity.ID = Convert.ToString(dt.Rows[i]["ID"]);
                entity.Isim = Convert.ToString(dt.Rows[i]["Isim"]);

                entities.Add(entity);
            }
            return entities;
        }
        private DataTable DepoListesiGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DepoListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", string.Empty);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return dt;
        }
    }
}