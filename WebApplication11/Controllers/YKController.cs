         using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;
using YKPortal.Models.YKClasses;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace YKPortal.Controllers
{
    public class YKController : Controller
    {
        public ActionResult AnaSayfa()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
            if (!Request.IsLocal && !Request.IsSecureConnection && Convert.ToBoolean(ConfigurationManager.AppSettings["SSLYonlendir"]))
            {
                Response.Redirect(redirectUrl, false);
                HttpContext.ApplicationInstance.CompleteRequest();
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AnaSayfadaAcilisSayfasiKontrolu"]))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_KullaniciGirisi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@KullaniciAdi", GetCookie("KullaniciAdi"));
                cmd.Parameters.AddWithValue("@Parola", GetCookie("Parola"));
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);

                    if (!Bilgi.StartsWith("UYARI!"))
                    {
                        if (Convert.ToString(dt.Rows[0]["AcilisSayfasi"]).Trim().Length > 0)
                        {
                            return Redirect(Convert.ToString(dt.Rows[0]["AcilisSayfasi"]).Trim());
                        }
                    }
                }
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Parametre";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "AnaSayfaAktifCari");
                DataTable dtParametre = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dtParametre.Rows.Count > 0)
                    ViewBag.AnaSayfaAktifCari = Convert.ToBoolean(Convert.ToString(dtParametre.Rows[0]["Deger"]) == "1" ? true : false);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Parametre";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "AnaSayfaAktifStok");
                DataTable dtParametre = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dtParametre.Rows.Count > 0)
                    ViewBag.AnaSayfaAktifStok = Convert.ToBoolean(Convert.ToString(dtParametre.Rows[0]["Deger"]) == "1" ? true : false);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Parametre";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "AnaSayfaAktifSiparis");
                DataTable dtParametre = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dtParametre.Rows.Count > 0)
                    ViewBag.AnaSayfaAktifSiparis = Convert.ToBoolean(Convert.ToString(dtParametre.Rows[0]["Deger"]) == "1" ? true : false);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Parametre";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "AnaSayfaBekleyenGorevSayisi");
                DataTable dtParametre = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dtParametre.Rows.Count > 0)
                    ViewBag.AnaSayfaBekleyenGorevSayisi = Convert.ToBoolean(Convert.ToString(dtParametre.Rows[0]["Deger"]) == "1" ? true : false);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Parametre";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "AnaSayfaTakvim");
                DataTable dtParametre = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dtParametre.Rows.Count > 0)
                    ViewBag.AnaSayfaTakvim = Convert.ToBoolean(Convert.ToString(dtParametre.Rows[0]["Deger"]) == "1" ? true : false);
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Parametre";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@Kod", "AnaSayfaSonAktiviteler");
                DataTable dtParametre = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dtParametre.Rows.Count > 0)
                    ViewBag.AnaSayfaSonAktiviteler = Convert.ToBoolean(Convert.ToString(dtParametre.Rows[0]["Deger"]) == "1" ? true : false);
            }

            SonAktiviteler();
            AnaSayfaBilgileri();
            AnaSayfaTakvimDurumlariListesiGetir();
            return View();
        }



        [HttpPost]
        public ActionResult TakvimKaydet(AnasayfaTakvimKaydetDto anasayfaTakvimKaydetDto)
        {
            if (anasayfaTakvimKaydetDto == null)
            {
                return Json(new { Success = false, Message = "Invalid input data." }, JsonRequestBehavior.AllowGet);
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_AnaSayfaTakvimKaydet";

            // Parametreleri ekle
            cmd.Parameters.AddWithValue("@ID", anasayfaTakvimKaydetDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Tarih", anasayfaTakvimKaydetDto.Tarih);
            cmd.Parameters.AddWithValue("@Durumu", anasayfaTakvimKaydetDto.Durumu);
            cmd.Parameters.AddWithValue("@Baslik", anasayfaTakvimKaydetDto.Baslik);
            cmd.Parameters.AddWithValue("@Aciklama", anasayfaTakvimKaydetDto.Aciklama);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            try
            {
                string ID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                return Json(new { Success = true, Message = "Record saved successfully.", ID = ID }, JsonRequestBehavior.AllowGet);
                // Başarılı kayıttan sonra Durum seçeneğini ViewBag'e ekle
                ViewBag.SeciliDurum = anasayfaTakvimKaydetDto.Durumu;
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpGet]
        public JsonResult Takvim(DateTime start, DateTime end)
        {
            JsonResult result = new JsonResult();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_AnaSayfaTakvimListesi";

            // Parametreleri ekle
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@BaslangicTarihi", start);
            cmd.Parameters.AddWithValue("@BitisTarihi", end);

            try
            {
                DataTable takvimListesi = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<FullcalendarDto> entities = new List<FullcalendarDto>();

                for (int i = 0; i < takvimListesi.Rows.Count; i++)
                {
                    var entity = new FullcalendarDto();
                    entity.id = Convert.ToString(takvimListesi.Rows[i]["ID"]);
                    entity.title = Convert.ToString(takvimListesi.Rows[i]["Baslik"]);
                    entity.start = new DateTimeOffset(Convert.ToDateTime(takvimListesi.Rows[i]["Tarih"]).ToUniversalTime()).ToUnixTimeMilliseconds();
                    entity.end = new DateTimeOffset(Convert.ToDateTime(takvimListesi.Rows[i]["Tarih"]).ToUniversalTime()).ToUnixTimeMilliseconds();
                    entity.description = Convert.ToString(takvimListesi.Rows[i]["Aciklama"]) ?? string.Empty;
                    entity.customStatus = Convert.ToString(takvimListesi.Rows[i]["Durumu"]) ?? string.Empty;
                    entities.Add(entity);
                }
                result.Data = new { Success = true, Message = "Başarılı", Data = entities };
                return Json(entities, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                result.Data = new { Success = false, Message = exception.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]

        public JsonResult TakvimEtkinlikSil(FullcalendarDto fullcalendarDto)
        {

            if (fullcalendarDto.id == null)
            {
                return Json(new IDJsonResult { Sonuc = "Hata", SonucKodu = 422 });
            }

            string kullaniciID = GetCookie("KullaniciID");
            string UyelikID = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_AnasayfaTakvimSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@ID", fullcalendarDto.id);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek);

            return Json(new IDJsonResult { Sonuc = "Başarılı", SonucKodu = 200 });
        }

        [HttpPost]
        public JsonResult AnaSayfaBilgileriGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_AnaSayfa";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var anasayfaBilgileri = new AnaSayfaDto
            {
                Cari = Convert.ToInt32(dt.Rows[0]["Cari"]),
                Stok = Convert.ToInt32(dt.Rows[0]["Stok"]),
                Belge = Convert.ToInt32(dt.Rows[0]["Belge"]),
                Gorev = Convert.ToInt32(dt.Rows[0]["Gorev"]),
                YeniOkunmamisMesaj = Convert.ToInt32(dt.Rows[0]["YeniOkunmamisMesaj"]),
            };

            JsonResult result = new JsonResult()
            {
                Data = new IDJsonResult
                {
                    Data = anasayfaBilgileri,
                    Sonuc = "0"
                },
            };

            return result;
        }

        private void AnaSayfaBilgileri()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_AnaSayfa";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var ansayfaBilgileri = new AnaSayfaDto
            {
                Cari = Convert.ToInt32(dt.Rows[0]["Cari"]),
                Stok = Convert.ToInt32(dt.Rows[0]["Stok"]),
                Belge = Convert.ToInt32(dt.Rows[0]["Belge"]),
                Gorev = Convert.ToInt32(dt.Rows[0]["Gorev"]),
                YeniOkunmamisMesaj = Convert.ToInt32(dt.Rows[0]["YeniOkunmamisMesaj"]),
            };

            ViewBag.Bilgiler = ansayfaBilgileri;
        }
        private void SonAktiviteler()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_SonHareketler";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var sonAktiviteler = new List<SonAktivitelerDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sonAktiviteler.Add(new SonAktivitelerDto
                {
                    Tarih = Convert.ToString(dt.Rows[i]["Tarih"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Tarih"]),
                    Modul = Convert.ToString(dt.Rows[i]["Modul"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Modul"]),
                    Aciklama1 = Convert.ToString(dt.Rows[i]["Aciklama1"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Aciklama1"]),
                    Aciklama2 = Convert.ToString(dt.Rows[i]["Aciklama2"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Aciklama2"]),
                    Kullanici = Convert.ToString(dt.Rows[i]["Kullanici"]) == null ? "Kullanıcı Bulunamadı" : Convert.ToString(dt.Rows[i]["Kullanici"]),
                });
            }

            ViewBag.SonAktiviteler = sonAktiviteler;
        }

        #region Giriş İşlemleri 
        [HttpGet]
        public ActionResult Giris()
        {

            string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
            if (!Request.IsLocal && !Request.IsSecureConnection && ConfigurationManager.AppSettings["SSLYonlendir"] == "1")
            {
                Response.Redirect(redirectUrl, false);
                HttpContext.ApplicationInstance.CompleteRequest();
            }
            if (ConfigurationManager.AppSettings["IlkAcilisSayfasi"] != "")
            {
                return Redirect(ConfigurationManager.AppSettings["IlkAcilisSayfasi"]);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Giris(string txtKullaniciAdi, string txtParola)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciGirisi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KullaniciAdi", txtKullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", txtParola);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            if (dt.Rows.Count > 0)
            {
                string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);

                if (!Bilgi.StartsWith("UYARI!"))
                {
                    #region Cookie İşlemleri
                    CreateCookie("Isim", Convert.ToString(dt.Rows[0]["Ad"]) + " " + Convert.ToString(dt.Rows[0]["Soyad"]));
                    CreateCookie("KullaniciID", Convert.ToString(dt.Rows[0]["ID"]));
                    CreateCookie("UyelikIsim", Convert.ToString(dt.Rows[0]["UyelikIsim"]));
                    CreateCookie("UyelikID", Convert.ToString(dt.Rows[0]["UyelikID"]));
                    CreateCookie("KullaniciAdi", Convert.ToString(dt.Rows[0]["KullaniciAdi"]));
                    CreateCookie("Parola", Convert.ToString(dt.Rows[0]["Parola"]));
                    CreateCookie("Resim", Convert.ToString(dt.Rows[0]["Resim"]));
                    CreateCookie("Logo", Convert.ToString(dt.Rows[0]["Logo"]));
                    CreateCookie("UyelikBitisTarihi", Convert.ToString(dt.Rows[0]["UyelikBitisTarihi"]));
                    CreateCookie("AcilisSayfasi", Convert.ToString(dt.Rows[0]["AcilisSayfasi"]));

                    #endregion

                    if (Convert.ToString(dt.Rows[0]["AcilisSayfasi"]).Trim().Length > 0)
                    {

                        return Redirect(Convert.ToString(dt.Rows[0]["AcilisSayfasi"]).Trim());
                    }

                    return Redirect("~/YK/AnaSayfa");
                }
            }


            return View(dt);
        }


        [HttpGet]
        public ActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SifremiUnuttum(string email)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciAdiBul";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@KullaniciAdi", email);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count <= 0 || !dt.Columns.Contains("KullaniciAdi"))
            {
                ViewBag.Bilgi = "Hata! Gönderdiğiniz bilgilere göre bir kullanıcı bulunamadı.";

                return View();
            }
            if (ConfigurationManager.AppSettings["SifremiUnuttum"] == "1")
            {
                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "mail.ykyazilim.com.tr";
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential("ilayda@ykyazilim.com.tr", "Ilayda12#");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ilayda@ykyazilim.com.tr", ConfigurationManager.AppSettings["FirmaAdi"]);

                mail.To.Add(email);

                mail.Subject = ConfigurationManager.AppSettings["FirmaAdi"] + " - Parola Sıfırlama";
                mail.IsBodyHtml = true;
                mail.Body =
                    $@"
                     <table style=""width: 100%;text-align: center;font-family: sans-serif;"">
                        <tr>
                            <td>
                                <table border=""0"" style=""padding: 16px 24px;width: 100%;text-align: center;max-width: 600px;margin-left: auto;margin-right: auto;"" >
                                    <tbody>
                                        <tr >
                                            <td><img width=""80px"" style=""margin-left: auto;margin-right: auto;padding-bottom: 16px;"" src=""https://app.ykyazilim.com.tr/Tema/media/Logolar/orijinal.png""/></td>
                                        </tr>
                                        <tr>
                                            <td style=""font-size: 24px; padding-top: 24px; padding-bottom: 24px;"">Merhaba Sayın, {dt.Rows[0]["Ad"]} {dt.Rows[0]["Soyad"]}</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <p style=""font-size: 20px;"">Şifreniz aşağıda belirtilmiştir. Lütfen başka bir şahısla paylaşmayınız.</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""text-align: center;""><p  style=""margin-left: auto;margin-right: auto; font-family: sans-serif;padding-top: 12px; padding-bottom: 12px; font-weight: 700; font-size: 36px; width: 200px;"">{dt.Rows[0]["Parola"]}</p> </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                ";
                try
                {
                    sc.Send(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }

            ViewBag.Bilgi = "Şifre bilgileriniz mail adresinize gönderildi.";

            return View();
        }


        [HttpPost]
        public ActionResult KullaniciOnayla(string kullaniciid)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciAktivasyonuYap";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", kullaniciid);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }

        public ActionResult Cikis()
        {
            DeleteCookie("Isim");
            DeleteCookie("KullaniciID");
            DeleteCookie("UyelikIsim");
            DeleteCookie("UyelikID");
            DeleteCookie("KullaniciAdi");
            DeleteCookie("Parola");
            DeleteCookie("Resim");
            DeleteCookie("Logo");
            DeleteCookie("AcilisSayfasi");

            return Redirect("~/YK/Giris");

        }

        #endregion

        #region Üyelik İşlemleri

        public ActionResult UyelikListesi(string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.AranacakKelime = aranacakKelime;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpGet]
        public ActionResult UyeOl()
        {
            IlListesiniOlustur();

            return View();

        }



        [HttpPost]
        public ActionResult UyeOl(UyelikDto uyelikDto)
        {


            IlListesiniOlustur();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@Isim", uyelikDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", uyelikDto.Unvan);
            cmd.Parameters.AddWithValue("@VergiNumarasi", uyelikDto.VergiNumarasi);
            cmd.Parameters.AddWithValue("@VergiDairesi", uyelikDto.VergiDairesi);
            cmd.Parameters.AddWithValue("@Adres", uyelikDto.Adres);
            cmd.Parameters.AddWithValue("@EMail", uyelikDto.EMail);
            cmd.Parameters.AddWithValue("@Iletisim", uyelikDto.Iletisim);
            cmd.Parameters.AddWithValue("@Kullanici", "");
            cmd.Parameters.AddWithValue("@UyelikBaslangicTarihi", DateTime.Now);
            cmd.Parameters.AddWithValue("@UyelikBitisTarihi", DateTime.Now.AddMonths(1));
            cmd.Parameters.AddWithValue("@ApiUrl", "http://api.ykyazilim.com.tr/api/");
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["ID"]).Length > 1)
            {
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_KullaniciKaydet";
                cmd.Parameters.AddWithValue("@ID", "");
                cmd.Parameters.AddWithValue("@UyelikID", Convert.ToString(dt.Rows[0]["ID"]));
                cmd.Parameters.AddWithValue("@KullaniciAdi", uyelikDto.EMail);
                cmd.Parameters.AddWithValue("@Parola", uyelikDto.Parola);
                cmd.Parameters.AddWithValue("@Ad", uyelikDto.Ad);
                cmd.Parameters.AddWithValue("@Soyad", uyelikDto.Soyad);
                cmd.Parameters.AddWithValue("@Aktif", true);
                cmd.Parameters.AddWithValue("@Telefon", uyelikDto.Iletisim);
                cmd.Parameters.AddWithValue("@Adres", uyelikDto.Adres);
                cmd.Parameters.AddWithValue("@Il", uyelikDto.Il);
                cmd.Parameters.AddWithValue("@Ilce", uyelikDto.Ilce);
                cmd.Parameters.AddWithValue("@Aciklama1", "");
                cmd.Parameters.AddWithValue("@Aciklama2", "");
                cmd.Parameters.AddWithValue("@Aciklama3", "");
                cmd.Parameters.AddWithValue("@Onay", Convert.ToBoolean(ConfigurationManager.AppSettings["IlkUyelikdeKullaniciyiOnayliYap"]));
                cmd.Parameters.AddWithValue("@Kullanici", "");
                cmd.Parameters.AddWithValue("@Resim", "");
                cmd.Parameters.AddWithValue("@Ilk", "1");
                dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["ID"]).Length > 0)
                {
                    ViewBag.Mesaj = "Üyeliğiniz başarıyla oluşturuldu, lütfen e-mail adresinizi kontrol ederek kullanıcınızı onaylayınız.";
                }
            }

            ViewBag.Form = uyelikDto;

            return View(dt);
        }

        public ActionResult UyelikDuzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Uyelik";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult UyelikDuzenle(UyelikDto uyelikDto, HttpPostedFileBase Resim)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikKaydet";

            if (Resim != null && Resim.ContentLength > 0)
            {
                var extension = Resim.FileName.Split('.').Last();

                var imageName = $"{uyelikDto.ID}_{Guid.NewGuid()}.{extension}";
                Resim.SaveAs(Server.MapPath($"/Uploads/Avatarlar/{imageName}"));
                cmd.Parameters.AddWithValue("@Resim", $"/Uploads/Avatarlar/{imageName}");
            }

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", uyelikDto.ID);
            cmd.Parameters.AddWithValue("@Isim", uyelikDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", uyelikDto.Unvan);
            cmd.Parameters.AddWithValue("@VergiDairesi", uyelikDto.VergiDairesi);
            cmd.Parameters.AddWithValue("@VergiNumarasi", uyelikDto.VergiNumarasi);
            cmd.Parameters.AddWithValue("@Adres", uyelikDto.Adres);
            cmd.Parameters.AddWithValue("@EMail", uyelikDto.EMail);
            cmd.Parameters.AddWithValue("@Iletisim", uyelikDto.Iletisim);
            cmd.Parameters.AddWithValue("@ApiUrl", uyelikDto.ApiUrl);
            cmd.Parameters.AddWithValue("@AcilisSayfasi", uyelikDto.AcilisSayfasi);
            cmd.Parameters.AddWithValue("@UyelikBaslangicTarihi", uyelikDto.UyelikBaslangicTarihi);
            cmd.Parameters.AddWithValue("@UyelikBitisTarihi", uyelikDto.UyelikBitisTarihi);
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                var columns = dt.Columns;
                if (columns.Contains("Bilgi"))
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    ViewBag.Bilgi = Bilgi;
                }
            }


            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "p_Uyelik";
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@ID", uyelikDto.ID);
            DataTable dt2 = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);

            return View(dt2);

        }
        public ActionResult UyelikSil(UyelikDto uyelikDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", uyelikDto.ID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("UyelikListesi");
        }
        #endregion

        public ActionResult Liste(string UyelikID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "p_SonHareketler";
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dt2 = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
            return View();
        }

        [HttpGet]
        public ActionResult TabBaslangic()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            return View();
        }

        [HttpGet]
        public ActionResult FiyatGor()
        {
            return View();
        }

        public JsonResult FiyatGorGetirHTML(string Barkod)
        {
            StokDto entity = new StokDto();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "p_StokFiyatGor";
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@Barkod", Barkod);
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd2, SorgulaTuru.DataSet);
            if (ds.Tables[0].Rows.Count > 0)
            {
                entity.HTMLPrint = Encoding.Default.GetBytes(Convert.ToString(ds.Tables[0].Rows[0]["Aciklama"]));
                entity.Aciklama = Convert.ToString(ds.Tables[0].Rows[0]["Aciklama"]);
                string dosya = "";// IDDizayn.DizaynIslemleri.DizaynKaydet(ds, ConfigurationManager.AppSettings["Klasor"]);
                entity.Aciklama2 = ConfigurationManager.AppSettings["WebSiteUrl"]+"/Temp/"+dosya;
            }
            else
            {
                entity.Kod = "ÜRÜN BULUNAMADI!";
            }
            return Json(entity, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FiyatGorGetir(string Barkod)
        {
            StokDto entity = new StokDto();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "p_StokFiyatGor";
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@Barkod", Barkod);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
            if (dt.Rows.Count > 0)
            {
                entity.Kod = Convert.ToString(dt.Rows[0]["Isim"]);
                entity.Isim = String.Format("{0:N2} TL", Convert.ToDecimal(dt.Rows[0]["Fiyat"]));
                entity.Fiyat = String.Format("{0:N2} TL", Convert.ToDecimal(dt.Rows[0]["Fiyat"]));

                entity.Barkod = Convert.ToString(dt.Rows[0]["Barkod"]);
                entity.OlcuBirimi = Convert.ToString(dt.Rows[0]["OlcuBirimi"]);
                entity.Tarih = Convert.ToDateTime(dt.Rows[0]["KayitTarihi"]).ToString("dd-MM-yyyy");
                entity.BirimFiyat = Convert.ToString(dt.Rows[0]["BirimFiyat"]);
                entity.Mensei = Convert.ToString(dt.Rows[0]["Mensei"]);
            }
            else
            {
                entity.Kod = "ÜRÜN BULUNAMADI!";
            }

            return Json(entity, JsonRequestBehavior.AllowGet);
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
                        #region Log Kaydı
                        try
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            string postData = "{\r\n    \"ProgramAdi\":\"Portal\",\r\n    \"Sirket\":\"" + Convert.ToString(dt.Rows[0]["UyelikIsim"]) + " - " + Convert.ToString(dt.Rows[0]["Ad"]) + " " + Convert.ToString(dt.Rows[0]["Soyad"]) + "\",\r\n    \"KullaniciAdi\":\"" + KullaniciAdi + "\",\r\n    \"Parola\":\"" + Parola + "\", \"IP\":\"" + Request.UserHostAddress + "\"   \r\n}";
                            var url = "https://app.ykyazilim.com.tr/api/YKWebApi/LogKaydet_KullaniciGirisi";
                            byte[] data = Encoding.UTF8.GetBytes(postData.ToString());
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                            request.KeepAlive = false;
                            request.ProtocolVersion = HttpVersion.Version10;
                            request.Method = "POST";
                            byte[] postBytes = Encoding.UTF8.GetBytes(postData.ToString());
                            request.ContentType = "application/json; charset=UTF-8";
                            request.Accept = "application/json";
                            request.ContentLength = postBytes.Length;
                            Stream requestStream = request.GetRequestStream();
                            requestStream.Write(postBytes, 0, postBytes.Length);
                            requestStream.Close();
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            string result;
                            using (StreamReader rdr = new StreamReader(response.GetResponseStream()))
                            {
                                result = rdr.ReadToEnd();
                            }
                        }
                        catch
                        {
                            ;
                        }
                        #endregion

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

        public void IlListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand ilCommand = new SqlCommand();
            ilCommand.CommandText = "p_GrupKoduListesi";
            ilCommand.CommandType = System.Data.CommandType.StoredProcedure;
            ilCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            ilCommand.Parameters.AddWithValue("@Kod", "Il");
            ilCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable ilDataTable = (DataTable)IDVeritabani.Sorgula(ilCommand, SorgulaTuru.Tablo);
            // Yeni bir Dto üretiyoruz class üzerindem 
            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < ilDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(ilDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(ilDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.Iller = entities;
        }

        private void AnaSayfaTakvimDurumlariListesiGetir()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand cariGrupKod1Command = new SqlCommand();
            cariGrupKod1Command.CommandText = "p_GrupKoduListesi";
            cariGrupKod1Command.CommandType = System.Data.CommandType.StoredProcedure;
            cariGrupKod1Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cariGrupKod1Command.Parameters.AddWithValue("@Kod", "AnaSayfaTakvimDurumlari");
            cariGrupKod1Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable cariGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cariGrupKod1Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < cariGrupKod1DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(cariGrupKod1DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(cariGrupKod1DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }

            ViewBag.TakvimDurumlari = entities;
        }

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

        public JsonResult LisansKontrol(
                    string _bulunduguDizin,
                    string _modul,
                    string _dosyaOlusturmaTarihi, 
                    string _bilgisayarAdi,
                    string _icIp,
                    string _disIp,
                    string _BaglantiCumlesi,
                    string _sirketKodu,
                    string _sirketIsmi,
                    string _lisansKodu)
        {


            string result = "";
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "IDP_LisansKontrol";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LisansKodu", _lisansKodu);
                cmd.Parameters.AddWithValue("@SabitIP", _disIp);
                result = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            }
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
Insert Into IDLog 
(Dizin,[Version],ExeTarihi,BilgisayarAdi,LocalIP,DisIP,BaglantiCumlesi,SirketKodu,SirketIsmi,Tarih,LisansKodu) 
values 
(@Dizin,@Version,@ExeTarihi,@BilgisayarAdi,@LocalIP,@DisIP,@BaglantiCumlesi,@SirketKodu,@SirketIsmi,GETDATE(),@LisansKodu)
";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Dizin", _bulunduguDizin);
                cmd.Parameters.AddWithValue("@Version", _modul);
                cmd.Parameters.AddWithValue("@ExeTarihi", _dosyaOlusturmaTarihi);
                cmd.Parameters.AddWithValue("@BilgisayarAdi", _bilgisayarAdi);
                cmd.Parameters.AddWithValue("@LocalIP", _icIp);
                cmd.Parameters.AddWithValue("@DisIP", _disIp);
                cmd.Parameters.AddWithValue("@BaglantiCumlesi", _BaglantiCumlesi);
                cmd.Parameters.AddWithValue("@SirketKodu", _sirketKodu);
                cmd.Parameters.AddWithValue("@SirketIsmi", _sirketIsmi);
                cmd.Parameters.AddWithValue("@LisansKodu", _lisansKodu);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}