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
    public class DepoController : Controller
    {
        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"select * from MenulerUretim WITH(NOLOCK)
INNER JOIN Yetkiler WITH(NOLOCK) ON Yetkiler.MenuID = MenulerUretim.ID
and Yetkiler.Gor = 1
and Yetkiler.KullaniciID = @KullaniciID
Where Aktif = 1 and UstID IS NULL Order by Sira";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            ViewBag.dtMenuler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }
        public ActionResult Menuler(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"select * from MenulerUretim WITH(NOLOCK)
INNER JOIN Yetkiler WITH(NOLOCK) ON Yetkiler.MenuID = MenulerUretim.ID
and Yetkiler.Gor = 1
and Yetkiler.KullaniciID = @KullaniciID 
Where Aktif = 1 and UstID = @UstID Order by Sira";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UstID", id);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            ViewBag.dtMenuler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }


        public ActionResult UretimIsEmirleri(string Operasyon, string aranacakKelime)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UretimIsEmriListesi";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Operasyon", Operasyon);
            cmd.Parameters.AddWithValue("@aranacakKelime", aranacakKelime);
            ViewBag.dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            ViewBag.Operasyon = Operasyon;
            ViewBag.aranacakKelime = aranacakKelime;

            return View();
        }

        public ActionResult UretimIsEmriDetay(string IsEmriNo)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UretimIsEmriDetay";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@IsEmriNo", IsEmriNo);
            ViewBag.dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }

        public JsonResult UretimIsEmriOkut(string IsEmriNo, string StokKodu, string SeriNo, decimal Miktar=0)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UretimIsEmriOkut";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@IsEmriNo", IsEmriNo);
            cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
            cmd.Parameters.AddWithValue("@SeriNo", SeriNo);
            cmd.Parameters.AddWithValue("@Miktar", Miktar);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public ActionResult UretimIsEmriDurumuDegistir(string IsEmriNo, string Durumu)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UretimIsEmriAkisIslem";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@IsEmriNo", IsEmriNo);
            if (Durumu == "B")
                cmd.Parameters.AddWithValue("@Islem", "BASLA");
            else if (Durumu == "T")
                cmd.Parameters.AddWithValue("@Islem", "BITIR");
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            if (Durumu == "T")
                return Redirect("~/Depo/Depo/AnaSayfa");
            else
                return Redirect("~/Depo/Depo/UretimIsEmriDetay/?IsEmriNo=" + IsEmriNo);
        }

        public ActionResult BarkodKontrol(string Barkod="")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BarkodKontrol";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Barkod", Barkod);
            ViewBag.dtDetay = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            if (((DataTable)ViewBag.dtDetay).Rows.Count == 0)
            {
                ViewBag.Uyarı = "Ürün bulunamadı!";
            }
            return View();
        }

        int SonOkutulanID = 0;
        int Kayit_Islem_Tipi_id = 0;
        int Company_id = 1;
        public ActionResult GirisCikisIslemleri(string SonOkutulan="")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if(SonOkutulan.Trim().Length > 0)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Exec dbo.IDP_SKI_StokBilgisiGetir '"+SonOkutulan+"'";
                cmd.CommandType = CommandType.Text;
                ViewBag.dtDetay = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "dbo.IDP_SKI_SonOkutulanSeriler";
                cmd.Parameters.AddWithValue("@ID", SonOkutulanID);
                cmd.CommandType = CommandType.StoredProcedure;
                ViewBag.dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return View();
        }

        public ActionResult GiriscikisIslemYap(int Islem, string barkod)
        {
            string sonuc = "";
            if (true) //(txt_Stok_Serino.Text.Contains(","))
            {
                string[] degerler = barkod.Split(',');
                foreach (string deger in degerler)
                {

                    try
                    {
                        Kayit_Islem_Tipi_id = Islem;
                    }
                    catch
                    {

                    }

                    try
                    {
                        int Kayit_Say_ = 0;

                        string Sorgu_1 = @" 
SELECT ID_SKI_StokSeri.ID,S.StokKodu, S.StokAdi as STOK_ADI  FROM dbo.ID_SKI_StokSeri (NOLOCK) 
LEFT JOIN dbo.Stoklar S WITH(NOLOCK) ON S.StokKodu = ID_SKI_StokSeri.StokKodu" +
                                         " Where  SeriNo='" + deger + "' ";
                        SqlCommand c1 = new SqlCommand();
                        c1.CommandText = Sorgu_1;

                        DataSet ds_1 = (DataSet)IDVeritabani.Sorgula(c1, SorgulaTuru.DataSet);


                        Kayit_Say_ = (ds_1.Tables[0].Rows.Count);

                        // KAYIT VAR İSE GETİRİLDİ...

                        if (Kayit_Say_ >= 1) // EĞER KAYIT VAR İSE AŞAĞIDA KONTROL İÇİN ATAMALAR YAPILIYOR..
                        {
                            string StokKodu = Convert.ToString(ds_1.Tables[0].Rows[0]["StokKodu"]);

                            int Kayit_Say = 0;

                            string Sorgu = @" 
SELECT 
*  
FROM dbo.ID_SKI_StokSeriIslemleri (NOLOCK)
LEFT JOIN dbo.ID_SKI_StokSeri Seri WITH(NOLOCK)
ON Seri_Lot_id = Seri.ID
WHERE (Islem_Tipi IN (2,3) ) and Seri.SeriNo ='" + deger + "' ";
                            SqlCommand c2 = new SqlCommand();
                            c2.CommandText = Sorgu;
                            DataSet ds = (DataSet)IDVeritabani.Sorgula(c2, SorgulaTuru.DataSet);


                            Kayit_Say = (ds.Tables[0].Rows.Count);

                            // KAYIT VAR İSE GETİRİLDİ...

                            if (Kayit_Say >= 1) // EĞER KAYIT VAR İSE AŞAĞIDA KONTROL İÇİN ATAMALAR YAPILIYOR..
                            {
                                sonuc = "Daha önce bu seri no ile kayıt yapılmıştır... KAYIT YAPILAMAZ...(" + deger + ")";
                                
                            }
                            else if (Kayit_Say == 0)
                            {

                                string Sorgu2 = "Exec dbo.IDP_SKI_StokBilgisiGetir '" + deger + "' ";
                                SqlCommand c3 = new SqlCommand();
                                c3.CommandText = Sorgu2;
                                DataSet dsDetay = (DataSet)IDVeritabani.Sorgula(c3, SorgulaTuru.DataSet);
                                string txtOtel = "";
                                int Seri_Lot_id = 0;
                                string Combo_Bolum_Adi = "";
                                if (dsDetay.Tables[0].Rows.Count > 0)
                                {
                                    Seri_Lot_id = Convert.ToInt32(dsDetay.Tables[0].Rows[0]["ID"]);
                                    //txtStokAdi.Text = Convert.ToString(dsDetay.Tables[0].Rows[0]["StokAdi"]);
                                    //txtStokKodu.Text = Convert.ToString(dsDetay.Tables[0].Rows[0]["StokKodu"]);
                                    //txtOtel.Text = Convert.ToString(dsDetay.Tables[0].Rows[0]["Otel"]);
                                    txtOtel = Convert.ToString(dsDetay.Tables[0].Rows[0]["Isletme_id"]);
                                    //Combo_Departman_Adi.Text = Convert.ToString(dsDetay.Tables[0].Rows[0]["Departman"]);
                                    //Combo_Departman_Adi.Text = Convert.ToString(dsDetay.Tables[0].Rows[0]["Departman"]);
                                    //Combo_Departman_Adi.Tag = Convert.ToString(dsDetay.Tables[0].Rows[0]["DepartmanID"]);
                                    //Combo_Bolum_Adi.Text = Convert.ToString(dsDetay.Tables[0].Rows[0]["Bolum"]);
                                    Combo_Bolum_Adi = Convert.ToString(dsDetay.Tables[0].Rows[0]["BolumID"]);

                                    sonuc = Stok_Giris_Cikis_Islemleri_Kayit(
                                        Convert.ToString(ds_1.Tables[0].Rows[0]["StokKodu"]),
                                        txtOtel,
                                        Seri_Lot_id,
                                        Combo_Bolum_Adi
                                        );
                                }
                                else
                                {
                                    sonuc = ("Seri bilgisi bulunamadı!");
                                }

                            }
                        }
                        else if (Kayit_Say_ == 0)
                        {
                            sonuc = "Bu Barkodlu Sisteme tanımlanmış Bir Seri Numarası Yok...";
                        }
                    }
                    catch
                    {
                        sonuc = "Bu Seri Nolu Kayıt Yok...!";
                    }
                }
            }
            else
            {

            }
            return Redirect("/Depo/Depo/GirisCikisIslemleri/?SonOkutulan=" + barkod + "&Bilgi="+ sonuc);
        }

        private string Stok_Giris_Cikis_Islemleri_Kayit(string stokkodu, string txtOtel, int Seri_Lot_id, string Combo_Bolum_Adi)
        {
            int Miktar = 0;
            string sonuc = "";

            try
            {
                

                if (Kayit_Islem_Tipi_id == 1)
                {
                    Miktar = 1;
                }
                if (Kayit_Islem_Tipi_id == 2 || Kayit_Islem_Tipi_id == 3)
                {
                    Miktar = -1;
                }
            }
            catch
            {
                // Hata = Hata + " -- " + "Departman Seçiminde Hata Var... ";
            }
            if (Kayit_Islem_Tipi_id <= 0)
            {
            }
            else
            {
                try
                {


                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"
Insert Into dbo.ID_SKI_StokSeriIslemleri 
(Tarih,StokKodu,Company_id,CariKart_id,Departman_id,Evrak_No,Islem_Tipi,Isletme_id,User_id,Adet,Aciklama,Seri_Lot_id,Bolum_id)
values 
(@Tarih,@StokKodu,@Company_id,@CariKart_id,@Departman_id,@Evrak_No,@Islem_Tipi,@Isletme_id,@User_id,@Adet,@Aciklama,@Seri_Lot_id,@Bolum_id)
SELECT SCOPE_IDENTITY()
";
                    cmd.Parameters.AddWithValue("Tarih", DateTime.Now);
                    cmd.Parameters.AddWithValue("StokKodu", stokkodu);
                    cmd.Parameters.AddWithValue("Company_id", Company_id);
                    cmd.Parameters.AddWithValue("CariKart_id", 103);
                    cmd.Parameters.AddWithValue("Departman_id", 2);
                    cmd.Parameters.AddWithValue("Evrak_No", "");
                    cmd.Parameters.AddWithValue("Islem_Tipi", Kayit_Islem_Tipi_id);
                    cmd.Parameters.AddWithValue("Isletme_id", txtOtel);
                    cmd.Parameters.AddWithValue("User_id", 0);
                    cmd.Parameters.AddWithValue("Adet", 1);
                    cmd.Parameters.AddWithValue("Aciklama", "");
                    cmd.Parameters.AddWithValue("Seri_Lot_id", Seri_Lot_id);
                    cmd.Parameters.AddWithValue("Bolum_id", Combo_Bolum_Adi);
                    int SonID = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                    


                    sonuc = "KAYIT BAŞARILI...";


                }
                catch (Exception err)
                {
                    sonuc = "KAYIT HATASI...!" + err.Message;

                    // this.ActiveControl = txt_Stok_Barkodu;
                }
            }
            return sonuc;
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