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

namespace YKPortal.Areas.B2BLogo.Controllers
{
    public class B2BLogoController : Controller
    {
        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!GetCookie("KullaniciAdi").Contains("@"))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_Cari";
                cmd.Parameters.AddWithValue("@ID", GetCookie("KullaniciAdi"));
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dtKayitlar.Rows.Count > 0)
                {
                    Session["B2BLogo_CariID"] = GetCookie("KullaniciID");
                    Session["B2BLogo_CariKodu"] = dtKayitlar.Rows[0]["Kod"];
                    Session["B2BLogo_CariAdi"] = dtKayitlar.Rows[0]["Isim"];
                }
            }


            return View();
        }
        public ActionResult CariListesi(string AranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_Cariler";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", AranacakKelime);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.AranacakKelime = AranacakKelime;

            return View(dtKayitlar);
        }
        public ActionResult CariSec(string CariKodu)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_Cari";
            cmd.Parameters.AddWithValue("@ID", CariKodu);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            if (dtKayitlar.Rows.Count > 0)
            {
                Session["B2BLogo_CariID"] = CariKodu;
                Session["B2BLogo_CariKodu"] = dtKayitlar.Rows[0]["Kod"];
                Session["B2BLogo_CariAdi"] = dtKayitlar.Rows[0]["Isim"];
            }
            return Redirect("~/B2BLogo/B2BLogo/AnaSayfa");
        }


        public ActionResult Siparislerim()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (Session["B2BLogo_CariID"] == null)
                return Redirect("~/B2BLogo/B2BLogo");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_Siparislerim";
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dtKayitlar);
        }
        public ActionResult SiparisDetay(string BelgeNo)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (Session["B2BLogo_CariID"] == null)
                return Redirect("~/B2BLogo/B2BLogo");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_SiparisDetay";
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@BelgeNo", BelgeNo);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dtKayitlar);
        }
        public ActionResult CariEkstre()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (Session["B2BLogo_CariID"] == null)
                return Redirect("~/B2BLogo/B2BLogo");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_CariHareketListesi";
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@BaslangicTarihi", "2000-01-01");
            cmd.Parameters.AddWithValue("@BitisTarihi", DateTime.Today.AddDays(1));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dtKayitlar);
        }

        public ActionResult YeniSiparis(string AranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (Session["B2BLogo_CariID"] == null)
                return Redirect("~/B2BLogo/B2BLogo");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_Stoklar";
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@AranacakKelime", AranacakKelime);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.AranacakKelime = AranacakKelime;

            return View(dtKayitlar);
        }
        public JsonResult SepeteEkle(string StokKodu = "", decimal Miktar = 0, decimal Gor = 0)
        {
            YKJsonResult result = new YKJsonResult();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_SepetEkle";
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", "B2B");
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@StokID", StokKodu);
            cmd.Parameters.AddWithValue("@Seri", "");
            cmd.Parameters.AddWithValue("@Birim", "");
            cmd.Parameters.AddWithValue("@Miktar", Miktar);
            //cmd.Parameters.AddWithValue("@Fiyat", Fiyat);
            //cmd.Parameters.AddWithValue("@Tutar", Miktar * Fiyat);
            cmd.Parameters.AddWithValue("@IslemTipi", "0");
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            result.SonucKodu = "1";
            result.Aciklama = "";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sepetim(string Bilgi = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (Session["B2BLogo_CariID"] == null)
                return Redirect("~/B2BLogo/B2BLogo");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_SepetListele";
            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.Bilgi = Bilgi;
            return View(dtKayitlar);
        }
        public ActionResult SepetSil(string KayitID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (Session["B2BLogo_CariID"] == null)
                return Redirect("~/B2BLogo/B2BLogo");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_B2B_SepetSil";
            cmd.Parameters.AddWithValue("@ID", KayitID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return Redirect("~/B2BLogo/B2BLogo/Sepetim");
        }

        public ActionResult SiparisOlustur()
        {
            string sonuc = "";

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_B2B_Sepettamamla";
                cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                sonuc = "Sipariş başarıyla kaydedilmiştir.";
                /*
                UnityApplication giris = new UnityApplication();
                if (giris.Connect())
                {
                    if (giris.UserLogin(ConfigurationManager.AppSettings["B2BLogoKullaniciAdi"], ConfigurationManager.AppSettings["B2BLogoParola"]))
                    {
                        if (giris.CompanyLogin(Convert.ToInt32(ConfigurationManager.AppSettings["B2BLogoSirket"]))) //Logo şirket numarası
                        {

                            UnityObjects.Data order = giris.NewDataObject(UnityObjects.DataObjectType.doSalesOrderSlip);
                            order.New();
                            order.DataFields.FieldByName("NUMBER").Value = "~";
                            order.DataFields.FieldByName("DATE").Value = DateTime.Today.ToString("dd.MM.yyyy");
                            //order.DataFields.FieldByName("TIME").Value = 171191089;
                            order.DataFields.FieldByName("ARP_CODE").Value = Session["B2BLogo_CariID"];
                            order.DataFields.FieldByName("RC_RATE").Value = 1;
                            order.DataFields.FieldByName("ORDER_STATUS").Value = 1;
                            //order.DataFields.FieldByName("SALESMAN_CODE").Value = "1";
                            order.DataFields.FieldByName("CURRSEL_TOTAL").Value = 1;
                            order.DataFields.FieldByName("DATA_SITEID").Value = 1;
                            UnityObjects.Lines transactions_lines = order.DataFields.FieldByName("TRANSACTIONS").Lines;


                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "p_B2B_SepetListele";
                            cmd.Parameters.AddWithValue("@CariID", Session["B2BLogo_CariID"]);
                            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                            foreach (DataRow satir in dtKayitlar.Rows)
                            {
                                transactions_lines.AppendLine();
                                transactions_lines[transactions_lines.Count - 1].FieldByName("TYPE").Value = 0;
                                transactions_lines[transactions_lines.Count - 1].FieldByName("MASTER_CODE").Value = satir["StokKodu"];
                                transactions_lines[transactions_lines.Count - 1].FieldByName("QUANTITY").Value = Convert.ToDecimal(satir["Miktar"]);
                                transactions_lines[transactions_lines.Count - 1].FieldByName("PRICE").Value = Convert.ToDecimal(satir["Fiyat"]);
                                transactions_lines[transactions_lines.Count - 1].FieldByName("VAT_RATE").Value = Convert.ToDecimal(satir["Kdv"]);
                                transactions_lines[transactions_lines.Count - 1].FieldByName("UNIT_CODE").Value = "ADET";
                                transactions_lines[transactions_lines.Count - 1].FieldByName("UNIT_CONV1").Value = 1;
                                transactions_lines[transactions_lines.Count - 1].FieldByName("UNIT_CONV2").Value = 1;
                                transactions_lines[transactions_lines.Count - 1].FieldByName("ORDER_RESERVE").Value = 1;
                                transactions_lines[transactions_lines.Count - 1].FieldByName("DUE_DATE").Value = DateTime.Today.ToString("dd.MM.yyyy");
                                //transactions_lines[transactions_lines.Count - 1].FieldByName("CURR_PRICE").Value = 160;
                                //transactions_lines[transactions_lines.Count - 1].FieldByName("PC_PRICE").Value = 1000;
                                transactions_lines[transactions_lines.Count( - 1].FieldByName("RC_XRATE").Value = 1;
                                transactions_lines[transactions_lines.Count - 1].FieldByName("SOURCE_WH").Value = 1;
                                transactions_lines[transactions_lines.Count - 1].FieldByName("SOURCE_COST_GRP").Value = 1;
                                transactions_lines[transactions_lines.Count - 1].FieldByName("DATA_SITEID").Value = 1;
                                transactions_lines[transactions_lines.Count - 1].FieldByName("SALESMAN_CODE").Value = 1;
                                transactions_lines[transactions_lines.Count - 1].FieldByName("AFFECT_RISK").Value = 1;
                                //transactions_lines[transactions_lines.Count - 1].FieldByName("EDT_PRICE").Value = 1000;
                                //transactions_lines[transactions_lines.Count - 1].FieldByName("EDT_CURR").Value = 160;
                                //transactions_lines[transactions_lines.Count - 1].FieldByName("ORG_DUE_DATE").Value = DateTime.Today.ToString("dd.MM.yyyy");
                                //transactions_lines[transactions_lines.Count - 1].FieldByName("ORG_QUANTITY").Value = 1;
                                //transactions_lines[transactions_lines.Count - 1].FieldByName("ORG_PRICE").Value = 1000;
                                //transactions_lines[transactions_lines.Count - 1].FieldByName("RESERVE_DATE").Value = DateTime.Today.ToString("dd.MM.yyyy");
                                //transactions_lines[transactions_lines.Count - 1].FieldByName("RESERVE_AMOUNT").Value = 1;
                            }

                            if (order.Post() == true)
                            {
                                sonuc = "Sipariş başarıyla kaydedilmiştir.";
                            }
                            else
                            {
                                if (order.ErrorCode != 0)
                                {
                                    sonuc = ("Veritabanı Hatası (" + order.ErrorCode.ToString() + ")-" + order.ErrorDesc + order.DBErrorDesc);
                                }
                                else if (order.ValidateErrors.Count > 0)
                                {
                                    sonuc = "XML Hatası:";
                                    for (int i = 0; i < order.ValidateErrors.Count; i++)
                                    {
                                        sonuc += "(" + order.ValidateErrors[i].ID.ToString() + ") - " + order.ValidateErrors[i].Error;
                                    }
                                }
                            }
                        }
                    }
                }
                */
            }
            catch (Exception err)
            {
                sonuc = err.Message;
            }

            return Redirect("/B2BLogo/B2BLogo/Sepetim/?Bilgi=" + sonuc);
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