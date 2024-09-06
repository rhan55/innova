using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.Depo.Controllers
{
    public class NetsisDepoController : Controller
    {


        [HttpGet]
        public ActionResult NetsisStokEkbilgiDuzenle(string Belge_Barkod = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametre";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", ("NetsisDatabase"));
            DataTable dtNetsisDatatable = (DataTable)(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo));
            string NetsisDatatable = "";
            if (dtNetsisDatatable.Rows.Count > 0)
            {
                NetsisDatatable = Convert.ToString(dtNetsisDatatable.Rows[0]["Deger"]);
            }

            if (Belge_Barkod != "")
            {

                cmd.Parameters.Clear();
                string _srg = " SELECT ST.STOK_KODU, STOK_ADI ";
                _srg += " , KULL1S, KULL2S, KULL3S ";
                _srg += " FROM " + NetsisDatatable + "..TBLSTSABIT ST WITH (NOLOCK) ";
                _srg += " INNER JOIN " + NetsisDatatable + "..TBLSTSABITEK EK WITH (NOLOCK) ON ST.STOK_KODU = EK.STOK_KODU ";
                _srg += " WHERE (ST.STOK_KODU = '" + Belge_Barkod + "' or ST.BARKOD1 = '" + Belge_Barkod + "' or ST.BARKOD2 = '" + Belge_Barkod + "' or ST.BARKOD3 = '" + Belge_Barkod + "' )";
                cmd.CommandText = _srg;
                cmd.CommandType = CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    ViewBag.dtDetay = dt;
                }

            }

            return View();
        }
        [HttpPost]
        public ActionResult NetsisStokEkBilgi_Guncelle(string Stok_Kodu, string Okutma_Kull1, string Okutma_Kull2, string Okutma_Kull3)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametre";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", ("NetsisDatabase"));
            DataTable dtNetsisDatatable = (DataTable)(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo));
            string NetsisDatatable = "";
            if (dtNetsisDatatable.Rows.Count > 0)
            {
                NetsisDatatable = Convert.ToString(dtNetsisDatatable.Rows[0]["Deger"]);
            }


            string _srg = " UPDATE "+ NetsisDatatable + "..TBLSTSABITEK ";
            _srg += " \r\n SET KULL1S = '"+ Okutma_Kull1 + "' ";
            _srg += " \r\n ,   KULL2S = '" + Okutma_Kull2 + "' ";
            _srg += " \r\n ,   KULL3S = '" + Okutma_Kull3 + "' ";
            _srg += " \r\n WHERE STOK_KODU = '" + Stok_Kodu + "' ";

            // SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = _srg;
            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.dtDetay = dt;

            return Redirect("~/Depo/NetsisDepo/NetsisStokEkbilgiDuzenle/?Belge_Lokasyon_Barkod=" + "");


        }

        [HttpGet]
        public ActionResult Innova_Uretim_Lokasyon(string Belge_Lokasyon_Barkod = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametre";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", ("NetsisDatabase"));
            DataTable dtNetsisDatatable = (DataTable)(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo));
            string NetsisDatatable = "";
            if (dtNetsisDatatable.Rows.Count > 0)
            {
                NetsisDatatable = Convert.ToString(dtNetsisDatatable.Rows[0]["Deger"]);
            }

            string NetsisVeritabani = NetsisDatatable;

            if (Belge_Lokasyon_Barkod == "")
            {

                cmd = new SqlCommand();
                string _srg = " SELECT TOP 10 FISNO, BARKOD, KAYIT_TARIHI FROM INNOVA..TBLOKUTMA WITH (NOLOCK) WHERE 1=1 and  DBNAME= 'Uretim_Lokasyon' ORDER BY ID DESC ";
                cmd.CommandText = _srg;
                cmd.CommandType = CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    ViewBag.dtDetay = dt;
                }

            }
            else
            {
                ViewBag.Belge_Fis_Numarasi = Belge_Lokasyon_Barkod;

                string _srg = " SELECT TOP 10 FISNO, BARKOD FROM INNOVA..TBLOKUTMA WITH (NOLOCK) WHERE 1=1 and  DBNAME= 'Uretim_Lokasyon' ORDER BY ID DESC ";
                cmd = new SqlCommand();
                cmd.CommandText = _srg;
                cmd.CommandType = CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ViewBag.dtDetay = dt;

            }

            return View();
        }
        [HttpPost]
        public ActionResult Innova_Uretim_Lokasyon_Atama(string Lokasyon_Barkod, string Lokasyon_Adresi)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametre";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", ("NetsisDatabase"));
            DataTable dtNetsisDatatable = (DataTable)(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo));
            string NetsisDatatable = "";
            if (dtNetsisDatatable.Rows.Count > 0)
            {
                NetsisDatatable = Convert.ToString(dtNetsisDatatable.Rows[0]["Deger"]);
            }

            string NetsisVeritabani = NetsisDatatable;

            string _srg = "INSERT INTO INNOVA..TBLOKUTMA ";
            _srg += " \r\n (DBNAME, TARIH, FISNO, FISNO2, BARKOD ";
            _srg += " \r\n  , MIKTAR, KAYIT_TARIHI, VERSIYON ) ";
            _srg += " \r\n SELECT 'Uretim_Lokasyon' , GETDATE(), '" + Lokasyon_Barkod + "' FISNO, '0' FISNO2, '" + Lokasyon_Adresi + "' BARKOD  ";
            _srg += " \r\n , 1 MIKTAR, GETDATE() KAYIT_TARIHI, '240703' AS VERSIYON  ";

            _srg += " \r\n UPDATE " + NetsisVeritabani + ".DBO.TBLCONFIGTRA SET LOKASYON = '" + Lokasyon_Adresi + "' WHERE INCKEYNO = '" + Lokasyon_Barkod + "' ";


            _srg += " \r\n exec " + NetsisVeritabani + "..[INN_PR_MOBILYA_01_URETIM_KAYIT] ";
            _srg += "  'TAKIM' ";
            _srg += " , '" + Lokasyon_Barkod + "' ";
            _srg += " , '1' ";
            _srg += " , 'Web' ";
            _srg += " , '500'";
            _srg += " , '' ";

            cmd = new SqlCommand();
            cmd.CommandText = _srg;
            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.dtDetay = dt;

            return Redirect("~/Depo/NetsisDepo/Innova_Uretim_Lokasyon/?Belge_Lokasyon_Barkod=" + "");


        }

        [HttpGet]
        public ActionResult Innova_Uretim_Takimlastirma(string Belge_Lokasyon_Barkod = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametre";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", ("NetsisDatabase"));
            DataTable dtNetsisDatatable = (DataTable)(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo));
            string NetsisDatatable = "";
            if (dtNetsisDatatable.Rows.Count > 0)
            {
                NetsisDatatable = Convert.ToString(dtNetsisDatatable.Rows[0]["Deger"]);
            }

            string NetsisVeritabani = NetsisDatatable;

            if (Belge_Lokasyon_Barkod == "")
            {

                cmd = new SqlCommand();
                cmd.CommandText = "SELECT TOP 1 LOKASYON from " + NetsisVeritabani + @".dbo.TBLCONFIGTRA WITH (NOLOCK) WHERE INCKEYNO = 1  ";
                cmd.CommandType = CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    ViewBag.Lokasyon = Convert.ToString(dt.Rows[0]["LOKASYON"]);
                }

            }
            else
            {
                ViewBag.Belge_Fis_Numarasi = Belge_Lokasyon_Barkod;

                string _srg = " EXEC [" + NetsisVeritabani + "].DBO.INN_PR_MOBILYA_URETIM_DETAY '" + Belge_Lokasyon_Barkod + "', 'TAKIM' ";
                cmd = new SqlCommand();
                cmd.CommandText = _srg;
                cmd.CommandType = CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ViewBag.dtDetay = dt;

            }

            return View();
        }

        [HttpPost]
        public ActionResult Innova_Uretim_Takimlastirma_Atama(string Lokasyon_Barkod, string Lokasyon_Adresi)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametre";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", ("NetsisDatabase"));
            DataTable dtNetsisDatatable = (DataTable)(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo));
            string NetsisDatatable = "";
            if (dtNetsisDatatable.Rows.Count > 0)
            {
                NetsisDatatable = Convert.ToString(dtNetsisDatatable.Rows[0]["Deger"]);
            }

            string NetsisVeritabani = NetsisDatatable;

            string _srg = "INSERT INTO INNOVA..TBLOKUTMA ";
            _srg += " (DBNAME, TARIH, FISNO, FISNO2, BARKOD ) ";
            _srg += " SELECT 'Uretim_Lokasyon' , GETDATE(), '" + Lokasyon_Barkod + "' FISNO, '0' FISNO2, '" + Lokasyon_Adresi + "' BARKOD  ";

            _srg += " \r\n exec [" + NetsisVeritabani + "].DBO.[INN_PR_MOBILYA_URETIM_KAYIT_TAKIM]   'TAKIM'  , '" + Lokasyon_Barkod + "'  , '1'  , 'Mobile'  , '500' , '" + Lokasyon_Adresi + "' , '' ";

            _srg += " \r\n UPDATE [" + NetsisVeritabani + "].DBO.TBLCONFIGTRA SET LOKASYON = '" + Lokasyon_Adresi + "' WHERE INCKEYNO = '" + Lokasyon_Barkod + "' ";

            cmd = new SqlCommand();
            cmd.CommandText = _srg;
            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.dtDetay = dt;

            return Redirect("~/Depo/NetsisDepo/Innova_Uretim_Takimlastirma/?Belge_Lokasyon_Barkod=" + Lokasyon_Barkod + "");


        }

        [HttpGet]
        public ActionResult Innova_Uretim_Okutma(string Belge_Fis_Numarasi = "")
        {
            if (Belge_Fis_Numarasi == "")
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT ISNULL((select MAX(FISNO2) +1 from INNOVA..TBLOKUTMA WITH (NOLOCK) WHERE ISNUMERIC(FISNO2) = 1 ) ,1) FISNO ";
                cmd.CommandType = CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    ViewBag.Belge_Fis_Numarasi = Convert.ToString(dt.Rows[0]["FISNO"]);
                }

            }
            else
            {
                ViewBag.Belge_Fis_Numarasi = Belge_Fis_Numarasi;

                string _srg = " SELECT FISNO2 FISNO, BARKOD, ADET, KG FROM INNOVA..TBLOKUTMA WITH (NOLOCK) WHERE FISNO2 = '" + Belge_Fis_Numarasi + "' ";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = _srg;
                cmd.CommandType = CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                ViewBag.dtDetay = dt;

            }

            return View();
        }


        [HttpPost]
        public ActionResult Innova_Uretim_Okutma_Bilgisi(string Okutma_Fisno, string Okutma_Barkod)
        {
            string _srg = "INSERT INTO INNOVA..TBLOKUTMA ";
            _srg += " ( FISNO, FISNO2, BARKOD ) ";
            _srg += " SELECT '" + Okutma_Fisno + "' FISNO, '" + Okutma_Fisno + "' FISNO2, '" + Okutma_Barkod + "' BARKOD  ";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = _srg;
            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.dtDetay = dt;

            return Redirect("~/Depo/NetsisDepo/Innova_Uretim_Okutma/?Belge_Fis_Numarasi=" + Okutma_Fisno);


        }

        [HttpPost]
        public ActionResult Innova_Uretim_Okutma_Bilgisi_Asama(string Barkod, string BelgeNo, string IslemTipi)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INN_PR_CAM_PANO_ISLEMLERI";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ISLEM_TIPI", IslemTipi);
            cmd.Parameters.AddWithValue("@SIPARIS_ID", Barkod);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return Redirect("Uretim_Okutma_Bilgisi");
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