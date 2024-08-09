using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;

namespace YKPortal.Controllers
{
    public class SatinalmaFaturaController : Controller
    {
        public ActionResult Sil(string Tip = "", string ID = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/SatinalmaFatura/Liste", "Sil"))
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
            return Redirect("~/SatinalmaFatura/Liste/?Tip=" + Tip);
        }

        public ActionResult Liste(string Tip = "", string AranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/SatinalmaFatura/Liste", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            if (Tip == "")
            {
                return Redirect("~/");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_BelgeListesi";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Tip", Tip);
            cmd.Parameters.AddWithValue("@AranacakKelime", AranacakKelime);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View(dt);
        }
        [HttpGet]
        public ActionResult Detay(string Tip, string id = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/SatinalmaFatura/Detay", "Gor"))
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
                case "AF":
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
        public ActionResult Kaydet(string Tip = "", string ID = "", string BelgeNo = "", string Tarih = "", string CariID = "",
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
            if (!YetkiKontrolu("/SatinalmaFatura/Detay", "Gor"))
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
                case "AF":  
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

            return View(entity);
        }


        [HttpGet]
        public ActionResult FaturaListesi()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (!YetkiKontrolu("/SatinalmaFatura/FaturaListesi", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            return View();
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