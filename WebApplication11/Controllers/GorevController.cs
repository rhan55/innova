using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;
using YKPortal.Models.YKClasses;
using System.Net;

namespace YKPortal.Controllers
{
    public class GorevController : Controller
    {


        public ActionResult DosyaSil(string GorevID, string KayitID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DosyaSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", KayitID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Modul", "Gorev");
            cmd.Parameters.AddWithValue("@KayitID", GorevID);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Redirect("~/Gorev/GorevDuzenle/" + GorevID);
        }
        [HttpPost]
        public ActionResult GorevTamamla(string GorevID, string Aciklama, DateTime TamamlamaTarihi)
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
            cmd.Parameters.AddWithValue("@TamamlamaTarihi", TamamlamaTarihi);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


            return Redirect("~/Gorev/GorevDuzenle/?ID=" + GorevID);
        }

        // GET: Gorev
        [HttpGet]
        public ActionResult GorevEkle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            GorevTipiListesiniOlustur();
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_KullaniciListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@AranacakKelime", "");
                ViewBag.Kullanicilar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }
            return View();
        }

        [HttpPost]
        public ActionResult GorevEkle(GorevDto gorevDto)
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

                    #region Kullanıcıya sms gönderme
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
                            aciklama += " app.ykyazilim.com.tr";
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

            return RedirectToAction("GorevListe");
        }

        [HttpGet]
        public ActionResult GorevDuzenle(string ID)
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

        [HttpPost]
        public ActionResult GorevDuzenle(GorevDto gorevDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GorevKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", gorevDto.ID);
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

            return RedirectToAction("GorevListe");
        }
        [HttpGet]
        public ActionResult GorevListe(GorevDto gorevDto, DateTime? Baslangic = null, DateTime? Bitis = null,
            string GorevTipiID = "", string KayitYapanKullanici = "", string AtananKullanici = "")
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
            cmd.Parameters.AddWithValue("@AtananKullanici", AtananKullanici);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            ViewBag.Baslangic = Baslangic;
            ViewBag.Bitis = Bitis;
            ViewBag.GorevTipiID = GorevTipiID;
            ViewBag.KayitYapanKullanici = KayitYapanKullanici;
            ViewBag.AtananKullanici = AtananKullanici;

            return View(dt);
        }
        [HttpPost]
        public ActionResult GorevSil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GorevSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GorevListe");
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

        private class KullaniciListesi
        {

        }


        #endregion
    }
}