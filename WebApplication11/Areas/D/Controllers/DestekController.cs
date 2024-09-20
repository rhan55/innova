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

namespace YKPortal.Areas.D.Controllers
{
    public class DestekController : Controller
    {
        public ActionResult YeniDestek()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            GorevTipiListesiniOlustur();
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_KullaniciListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@AranacakKelime", "DestekKullanicilari");
                ViewBag.Kullanicilar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }



            return View();
        }

        public void GorevTipiListesiniOlustur()
        {
            //GrupKodu1 Listesi oluşturma
            SqlCommand gorevtipiCommand = new SqlCommand();
            gorevtipiCommand.CommandText = "p_GrupKoduListesi";
            gorevtipiCommand.CommandType = System.Data.CommandType.StoredProcedure;
            gorevtipiCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            gorevtipiCommand.Parameters.AddWithValue("@Kod", "GorevTipi");
            gorevtipiCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable gorevTipiDataTable = (DataTable)IDVeritabani.Sorgula(gorevtipiCommand, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < gorevTipiDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(gorevTipiDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(gorevTipiDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.GorevTipleri = entities;
        }


        [HttpPost]
        public ActionResult YeniDestek(GorevDto gorevDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GorevKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@GorevTipiID", gorevDto.GorevTipiID);
            cmd.Parameters.AddWithValue("@Aciklama", gorevDto.Aciklama);
            cmd.Parameters.AddWithValue("@BaslangicTarihi", gorevDto.BaslangicTarihi);
            cmd.Parameters.AddWithValue("@Periyot", gorevDto.Periyot);
            string SonID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

            cmd.Parameters.Clear();
            cmd.CommandText = "p_GorevKullanicilariniSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@GorevID", SonID); ;
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            if (gorevDto.Kullanicilar != null)
            {
                foreach (string kullanici in gorevDto.Kullanicilar)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_GorevKullaniciKaydet";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                    cmd.Parameters.AddWithValue("@GorevID", SonID);
                    cmd.Parameters.AddWithValue("@SecilenKullaniciID", kullanici);
                    cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                    #region Kullanıcıya sms ve mail gönderme
                    try
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_Kullanici";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@ID", kullanici);
                        string telefon = Convert.ToString(((DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo)).Rows[0]["Telefon"])
                            .Replace(" ", "")
                            .Replace("-", "")
                            .Replace("_", "")
                            .Replace(")", "")
                            .Replace("(", "");
                        if (telefon.Trim().Length > 0)
                        {
                            string aciklama = gorevDto.Aciklama;
                            if (aciklama.Length > 100)
                            {
                                aciklama = aciklama.Substring(0, 90) + "...";
                            }
                            try
                            {
                                //aciklama += " app.ykyazilim.com.tr";
                                string url = @"http://idyazilim.com/Site/SmsGonder/?telefon=" + telefon + "&" +
                                    "KullaniciAdi=" + YKUtils.SmsKullaniciAdi + "&" +
                                    "Parola=" + YKUtils.SmsParola + "&" +
                                    "Isim=" + YKUtils.SmsIsim + "&" +
                                    "Sirket=YK YAZILIM&" +
                                    "Program=" + "YK App" + "&" +
                                    "mesaj=" + GetCookie("Isim") + " kullanıcısı " + gorevDto.BaslangicTarihi.ToString("dd-MM-yyyy HH:mm") + " tarihli görev atadı, görev ayrıntı : " + aciklama + "";
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                var sonuc = request.GetResponse();
                            }
                            catch(Exception err)
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandText = "p_HataKaydet";
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                                cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
                                cmd.Parameters.AddWithValue("@Modul", "Gorev");
                                cmd.Parameters.AddWithValue("@Aciklama1", "~/Gorev/GorevEkle");
                                cmd.Parameters.AddWithValue("@Aciklama2", err.Message);
                                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                            }
                        }


                        #region Mail Gönder


                        cmd.Parameters.Clear();
                        cmd.CommandText = "Select * from MailKaliplari WITH(NOLOCK) Where UyelikID = @UyelikID and Kod = @Kod";
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Kod", "Destek_Gorev_Yeni");
                        DataTable dtMail = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                        if (dtMail.Rows.Count > 0)
                        {
                            string Baslik = Convert.ToString(dtMail.Rows[0]["Isim"]);
                            string Icerik = Convert.ToString(dtMail.Rows[0]["Icerik"]);
                            Icerik = Icerik.Replace("{Isim}", GetCookie("Isim"));
                            Icerik = Icerik.Replace("{Tarih}", gorevDto.BaslangicTarihi.ToString("dd-MM-yyyy HH:mm"));
                            Icerik = Icerik.Replace("{Aciklama}", gorevDto.Aciklama);
                            Icerik = Icerik.Replace("{KayitNo}", SonID);

                            cmd.Parameters.Clear();
                            cmd.CommandText = "select * from Parametreler  WITH(NOLOCK) Where Modul = 'EMail' and UyelikID = @UyelikID";
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                            DataTable dtMailBilgileri = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                            cmd.Parameters.Clear();
                            cmd.CommandText = @"Select Kullanicilar.KullaniciAdi from GorevKullanicilari WITH(NOLOCK) 
left outer join Kullanicilar WITH(NOLOCK) ON Kullanicilar.ID = GorevKullanicilari.KullaniciID
Where GorevKullanicilari.GorevID = @ID";
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.AddWithValue("@ID", SonID);
                            DataTable dtMailAdresi = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                            string mailAdresi = "";
                            if (dtMailAdresi.Rows.Count > 0)
                                mailAdresi = Convert.ToString(dtMailAdresi.Rows[0]["KullaniciAdi"]);

                            YKUtils.MailGonder(Baslik, Icerik, GetCookie("KullaniciAdi")+";"+ mailAdresi,
                                    Convert.ToString(dtMailBilgileri.Select(" Isim = 'KullaniciAdi' ")[0]["Deger"]),
                                    Convert.ToString(dtMailBilgileri.Select(" Isim = 'Parola' ")[0]["Deger"]),
                                    Convert.ToString(dtMailBilgileri.Select(" Isim = 'Host' ")[0]["Deger"]),
                                    Convert.ToInt32(dtMailBilgileri.Select(" Isim = 'Port' ")[0]["Deger"]),
                                    Convert.ToString(dtMailBilgileri.Select(" Isim = 'SSL' ")[0]["Deger"]) == "0" ? false : true
                                );
                        }

                        #endregion
                    }
                    catch (Exception err)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_HataKaydet";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
                        cmd.Parameters.AddWithValue("@Modul", "Gorev");
                        cmd.Parameters.AddWithValue("@Aciklama1", "~/Gorev/GorevEkle");
                        cmd.Parameters.AddWithValue("@Aciklama2", err.Message);
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    #endregion
                }
            }

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Keys.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + file.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Gorev");
                        cmd.Parameters.AddWithValue("@KayitID", SonID);
                        cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + file.FileName);
                        cmd.Parameters.AddWithValue("@Isim", file.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                }
            }

            return RedirectToAction("AnaSayfa");
        }

        [HttpPost]
        public ActionResult DestekTamamla(string GorevID, string Aciklama, string Durumu, DateTime TamamlamaTarihi)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GorevTamamla";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GorevID", GorevID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
            cmd.Parameters.AddWithValue("@Durumu", Durumu);
            cmd.Parameters.AddWithValue("@TamamlamaTarihi", TamamlamaTarihi);
            string SonID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Keys.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Server.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + file.FileName));
                        cmd.Parameters.Clear();
                        cmd.CommandText = "p_DosyaKaydet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", "");
                        cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                        cmd.Parameters.AddWithValue("@Modul", "Gorev");
                        cmd.Parameters.AddWithValue("@KayitID", SonID);
                        cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + file.FileName);
                        cmd.Parameters.AddWithValue("@Isim", file.FileName);
                        cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                }
            }

            #region Mail Gönder


            cmd.Parameters.Clear();
            cmd.CommandText = "Select * from MailKaliplari WITH(NOLOCK) Where UyelikID = @UyelikID and Kod = @Kod";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", "Destek_Gorev_Tamamlama");
            DataTable dtMail = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            if (dtMail.Rows.Count > 0)
            {
                string Baslik = Convert.ToString(dtMail.Rows[0]["Isim"]);
                string Icerik = Convert.ToString(dtMail.Rows[0]["Icerik"]);
                Icerik = Icerik.Replace("{Isim}", GetCookie("Isim"));
                Icerik = Icerik.Replace("{KayitNo}", GorevID);
                Icerik = Icerik.Replace("{Durumu}", Durumu);
                Icerik = Icerik.Replace("{Yanit}", Aciklama);

                cmd.Parameters.Clear();
                cmd.CommandText = "select * from Parametreler  WITH(NOLOCK) Where Modul = 'EMail' and UyelikID = @UyelikID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                DataTable dtMailBilgileri = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                cmd.Parameters.Clear();
                cmd.CommandText = @"Select Kullanicilar.KullaniciAdi from Gorevler WITH(NOLOCK)
left outer join Kullanicilar WITH(NOLOCK) ON Kullanicilar.ID = Gorevler.KayitYapanKullanici
Where Gorevler.ID = @ID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", GorevID);
                DataTable dtMailAdresi = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                string mailAdresi = "";
                if (dtMailAdresi.Rows.Count > 0)
                    mailAdresi = Convert.ToString(dtMailAdresi.Rows[0]["KullaniciAdi"]);


                YKUtils.MailGonder(Baslik, Icerik, mailAdresi,
                        Convert.ToString(dtMailBilgileri.Select(" Isim = 'KullaniciAdi' ")[0]["Deger"]),
                        Convert.ToString(dtMailBilgileri.Select(" Isim = 'Parola' ")[0]["Deger"]),
                        Convert.ToString(dtMailBilgileri.Select(" Isim = 'Host' ")[0]["Deger"]),
                        Convert.ToInt32(dtMailBilgileri.Select(" Isim = 'Port' ")[0]["Deger"]),
                        Convert.ToString(dtMailBilgileri.Select(" Isim = 'SSL' ")[0]["Deger"]) == "0" ? false : true
                    );
            }

            #endregion

            return Redirect("~/D/Destek/DestekDetayi/?ID=" + GorevID);
        }

        [HttpGet]
        public ActionResult AnaSayfa(GorevDto gorevDto, DateTime? Baslangic = null, DateTime? Bitis = null,
            string Durum = "Beklemede", string GorevTipiID = "", string KayitYapanKullanici = "", string AtananKullanici = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (Baslangic == null)
            {
                Baslangic = DateTime.Today.AddMonths(-3);
                Bitis = DateTime.Today.AddMonths(3);
            }


            GorevTipiListesiniOlustur();
            {
                SqlCommand cmdKullanici = new SqlCommand();
                cmdKullanici.CommandText = "p_KullaniciListesi";
                cmdKullanici.CommandType = System.Data.CommandType.StoredProcedure;
                cmdKullanici.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmdKullanici.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.Kullanicilar = (DataTable)IDVeritabani.Sorgula(cmdKullanici, SorgulaTuru.Tablo);
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GorevListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
            cmd.Parameters.AddWithValue("@Bitis", Bitis);
            cmd.Parameters.AddWithValue("@GorevTipiID", GorevTipiID);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", KayitYapanKullanici);
            cmd.Parameters.AddWithValue("@Durum", Durum);
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            ViewBag.Baslangic = Baslangic;
            ViewBag.Bitis = Bitis;
            ViewBag.GorevTipiID = GorevTipiID;
            ViewBag.KayitYapanKullanici = KayitYapanKullanici;
            ViewBag.AtananKullanici = AtananKullanici;
            ViewBag.Durum = Durum;

            return View(ds);
        }

        [HttpGet]
        public ActionResult DestekDetayi(string ID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            GorevTipiListesiniOlustur();
            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Gorev";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
            ViewBag.ID = ID;


            {
                SqlCommand cmdKullanici = new SqlCommand();
                cmdKullanici.CommandText = "p_KullaniciListesi";
                cmdKullanici.CommandType = System.Data.CommandType.StoredProcedure;
                cmdKullanici.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmdKullanici.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.Kullanicilar = (DataTable)IDVeritabani.Sorgula(cmdKullanici, SorgulaTuru.Tablo);
            }

            return View(ds);

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