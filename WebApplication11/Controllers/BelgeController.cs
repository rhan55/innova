using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;

namespace YKPortal.Controllers
{
    public class BelgeController : Controller
    {
        public ActionResult Liste(string Tip="", string AranacakKelime="")
        {
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
        public ActionResult Detay(string Tip , int id = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_Belge";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", id);
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
            BelgeDto entity = new BelgeDto();
            if (ds.Tables[0].Rows.Count > 0)
            {
                entity.ID = Convert.ToString(ds.Tables[0].Rows[0]["ID"]);
                entity.Tarih = Convert.ToDateTime(ds.Tables[0].Rows[0]["Tarih"]);
                entity.BelgeNo = Convert.ToString(ds.Tables[0].Rows[0]["BelgeNo"]);
                entity.CariID = Convert.ToString(ds.Tables[0].Rows[0]["CariID"]);
                entity.CariAdi = Convert.ToString(ds.Tables[0].Rows[0]["CariAdi"]);
                entity.Aciklama = Convert.ToString(ds.Tables[0].Rows[0]["Aciklama"]);
                entity.Kalemler = new List<BelgeKalemDto>();
                foreach (DataRow satir in ds.Tables[1].Rows)
                {
                    BelgeKalemDto s = new BelgeKalemDto();
                    s.BelgeID = entity.ID;
                    s.StokID = Convert.ToString(satir["StokID"]);
                    s.StokKodu = Convert.ToString(satir["StokKodu"]);
                    s.StokAdi = Convert.ToString(satir["StokAdi"]);
                    s.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    s.Miktar = Convert.ToDecimal(satir["Miktar"]);
                    s.Fiyat = Convert.ToDecimal(satir["Fiyat"]);
                    s.Iskonto = Convert.ToDecimal(satir["IskontoOrani1"]);
                    s.Tutar = Convert.ToDecimal(satir["Tutar"]);
                    
                    entity.Kalemler.Add(s);
                }
            }
            return View(entity);
        }

        [HttpPost]
        public JsonResult KaydetUst(BelgeDto belge,List<BelgeKalemDto> kalemler)
        {
            JsonResult result = new JsonResult();

            #region Kayıt işlemi gerçekleştirilecek.

            #endregion

            result.Data = belge.ID;
            return Json(result, JsonRequestBehavior.AllowGet);
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
    }
}