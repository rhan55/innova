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
        public ActionResult Sil(string Tip = "", string ID = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


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
            return Redirect("~/Belge/Liste/?Tip="+Tip);
        }

        public ActionResult Liste(BelgeDto belgeDto, string Tip = "", string AranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");       

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

            ViewBag.Filters = belgeDto;
            ViewBag.Durumu = belgeDto.Durumu;
            return View(dt);
        }
 

        [HttpGet]
        public ActionResult Yazdir(string Tip, string id = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_BelgeYazdir";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/Belge/Liste/?Tip=" + Tip);
        }
        [HttpGet]
        public ActionResult Detay(string Tip , string id = "")
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
                case "AI":
                    entity.BelgeTipi = BelgeTipi.AlisIrsaliyesi;
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
                    s.Iskonto = Convert.ToDecimal(satir["IskontoOrani1"]);
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


            {
                SqlCommand cmdKayit = new SqlCommand();
                cmdKayit.CommandType = System.Data.CommandType.Text;
                cmdKayit.CommandText = "select ISNULL(Deger,0) as Deger from Parametreler WITH(NOLOCK) Where UyelikID = @UyelikID and Modul = 'DepoDepoTransferiAciklamaGizle'";
                cmdKayit.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                DataTable dtKayit = (DataTable)IDVeritabani.Sorgula(cmdKayit, SorgulaTuru.Tablo);
                if (dtKayit.Rows.Count > 0)
                    ViewBag.DepoDepoTransferiAciklamaGizle = Convert.ToString(dtKayit.Rows[0]["Deger"]) == "1" ? true : false;
                else
                    ViewBag.DepoDepoTransferiAciklamaGizle = false;
            }
            {
                SqlCommand cmdKayit = new SqlCommand();
                cmdKayit.CommandType = System.Data.CommandType.Text;
                cmdKayit.CommandText = "select ISNULL(Deger,0) as Deger from Parametreler WITH(NOLOCK) Where UyelikID = @UyelikID and Modul = 'DepoDepoTransferiFiyatGizle'";
                cmdKayit.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                DataTable dtKayit = (DataTable)IDVeritabani.Sorgula(cmdKayit, SorgulaTuru.Tablo);
                if (dtKayit.Rows.Count > 0)
                    ViewBag.DepoDepoTransferiFiyatGizle = Convert.ToString(dtKayit.Rows[0]["Deger"]) == "1" ? true : false;
                else
                    ViewBag.DepoDepoTransferiFiyatGizle = false;
            }

            return View(entity);
        }

        [HttpPost]
        public ActionResult Kaydet(string Tip = "",string ID="", string BelgeNo="", string Tarih = "", string CariID = "", 
            string DepoCikisID="", string DepoGirisID="",
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
                    if (item.StokID != "")
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
                        cmd.Parameters.AddWithValue("@IskontoOrani1", 0);
                        cmd.Parameters.AddWithValue("@KdvOrani", 0);
                        cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                        silinenler += Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek)) + ",";
                    }
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
            //return Redirect(Request.Url.Authority+"/Belge/Detay/"+ID+"&Tip="+Tip);
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


        public JsonResult SelectStokSeriliGiris(string StokID)
        {
            JsonResult result = new JsonResult();
            List<StokDto> entities = new List<StokDto>();
            #region İşlemler

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select SeriliGiris,SeriliCikis from w_Stoklar where ID = @StokID";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@StokID", StokID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            #endregion
            result.Data = Convert.ToString(dt.Rows[0]["SeriliGiris"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SelectStokSeriliCikis(string StokID)
        {
            JsonResult result = new JsonResult();
            List<StokDto> entities = new List<StokDto>();
            #region İşlemler

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select SeriliGiris,SeriliCikis from w_Stoklar where ID = @StokID";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@StokID", StokID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            #endregion
            result.Data = Convert.ToString(dt.Rows[0]["SeriliCikis"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SelectStokSeriListe(string StokID, string DepoID)
        {
            JsonResult result = new JsonResult();
            List<StokDto> entities = new List<StokDto>();
            #region İşlemler

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokSerileri";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@StokID", StokID);
            cmd.Parameters.AddWithValue("@DepoID", DepoID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            foreach (DataRow satir in dt.Rows)
            {
                if (Convert.ToDecimal(satir["Bakiye"]) > 0)
                {
                    StokDto entity = new StokDto();
                    entity.ID = Convert.ToString(satir["ID"]);
                    entity.SeriNo = Convert.ToString(satir["SeriNo"]);
                    entity.Bakiye = Convert.ToDecimal(satir["Bakiye"]);
                    entities.Add(entity);
                }
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


    }
}