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
using YKPortal.Areas.Satinalma.Controllers;
using YKPortal.Models;
using YKPortal.Models.Dto;
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.E.Controllers
{
    public class SiteController : Controller
    {
        [HttpGet]
        public ActionResult AnaSayfa()
        {

            if (!AutoGirisKontrol())
            {
                return RedirectToAction("Giris");
            }

            return View();
        }

        [HttpGet]
        public ActionResult UyeOl()
        {
            if (!AutoGirisKontrol())
            {
                return RedirectToAction("Giris");
            }
            IlListesiniOlustur();
            UlkeListesiniOlustur();


            return View();
        }
      
        [HttpPost]  
        public ActionResult UyeOl(CariDto cariDto)
        {
            IlListesiniOlustur();
            UlkeListesiniOlustur();
            // 1. TCKimlikNoVergiNo Validasyonu
            if (string.IsNullOrEmpty(cariDto.TCKimlikNoVergiNo))
            {
                ModelState.AddModelError("TCKimlikNoVergiNo", "TCKN veya Vergi Numarası boş olamaz.");
            }
            else if (cariDto.TCKimlikNoVergiNo.Length != 10 && cariDto.TCKimlikNoVergiNo.Length != 11)
            {
                ModelState.AddModelError("TCKimlikNoVergiNo", "TCKN 11 haneli veya Vergi Numarası 10 haneli olmalıdır.");
            }
            else if (!long.TryParse(cariDto.TCKimlikNoVergiNo, out _))
            {
                ModelState.AddModelError("TCKimlikNoVergiNo", "TCKN veya Vergi Numarası yalnızca rakamlardan oluşmalıdır.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Form = cariDto;
                return View();
            }

            // 2. SQL Komut Hazırlığı
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "p_CariKaydet",
                CommandType = CommandType.StoredProcedure
            };            
            cmd.Parameters.AddWithValue("@UyelikID", System.Configuration.ConfigurationManager.AppSettings["UyelikID"]);
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@KullaniciID", "");
            cmd.Parameters.AddWithValue("@Aktif", cariDto.Aktif);
            cmd.Parameters.AddWithValue("@KayitTarihi  ", DateTime.Now);
            cmd.Parameters.AddWithValue("@Kod", cariDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", cariDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", cariDto.Unvan);
            cmd.Parameters.AddWithValue("@Adres", cariDto.Adres);
            cmd.Parameters.AddWithValue("@Ilce", cariDto.Ilce);
            cmd.Parameters.AddWithValue("@Il", cariDto.Il);
            cmd.Parameters.AddWithValue("@Ulke", cariDto.Ulke);
            cmd.Parameters.AddWithValue("@Bolge ", cariDto.Bolge);
            cmd.Parameters.AddWithValue("@VergiDairesi", cariDto.VergiDairesi);          
            cmd.Parameters.AddWithValue("@PostaKodu", cariDto.PostaKodu);
            cmd.Parameters.AddWithValue("@Alici", cariDto.Alici);
            cmd.Parameters.AddWithValue("@Satici", cariDto.Satici);
            cmd.Parameters.AddWithValue("@Personel", cariDto.Personel);
            cmd.Parameters.AddWithValue("@Telefon1", cariDto.Telefon1);
            cmd.Parameters.AddWithValue("@Telefon2", cariDto.Telefon2);
            cmd.Parameters.AddWithValue("@EMail", cariDto.EMail);
            cmd.Parameters.AddWithValue("@Faks", cariDto.Faks);
            cmd.Parameters.AddWithValue("@CepTelefonu", cariDto.CepTelefonu);
            cmd.Parameters.AddWithValue("@WebSite", cariDto.WebSite);
            cmd.Parameters.AddWithValue("@GrupKodu1ID", cariDto.GrupKodu1ID == null ? string.Empty : cariDto.GrupKodu1ID);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", cariDto.GrupKodu2ID == null ? string.Empty : cariDto.GrupKodu2ID);
            cmd.Parameters.AddWithValue("@GrupKodu3ID", cariDto.GrupKodu3ID == null ? string.Empty : cariDto.GrupKodu3ID);
            cmd.Parameters.AddWithValue("@GrupKodu4ID", cariDto.GrupKodu4ID == null ? string.Empty : cariDto.GrupKodu4ID);
            cmd.Parameters.AddWithValue("@GrupKodu5ID", cariDto.GrupKodu5ID == null ? string.Empty : cariDto.GrupKodu5ID);
            cmd.Parameters.AddWithValue("@GrupKodu6ID", cariDto.GrupKodu6ID == null ? string.Empty : cariDto.GrupKodu6ID);
            cmd.Parameters.AddWithValue("@MuhasebeKodu", cariDto.MuhasebeKodu);
            cmd.Parameters.AddWithValue("@Kilitli", cariDto.Kilitli);
            cmd.Parameters.AddWithValue("@KilitAciklamasi", cariDto.KilitAciklamasi);
            cmd.Parameters.AddWithValue("@DovizID", "");
            cmd.Parameters.AddWithValue("@VadeGunu", cariDto.VadeGunu);
            cmd.Parameters.AddWithValue("@Iskonto1", cariDto.Iskonto1);
            cmd.Parameters.AddWithValue("@ListeFiyat", cariDto.ListeFiyat);
            cmd.Parameters.AddWithValue("@Aciklama1", cariDto.Aciklama1 == null ? string.Empty : cariDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", cariDto.Aciklama2 == null ? string.Empty : cariDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", cariDto.Aciklama3 == null ? string.Empty : cariDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Aciklama4", cariDto.Aciklama4 == null ? string.Empty : cariDto.Aciklama4);
            cmd.Parameters.AddWithValue("@Aciklama5", cariDto.Aciklama5 == null ? string.Empty : cariDto.Aciklama5);
            cmd.Parameters.AddWithValue("@Aciklama6", cariDto.Aciklama6 == null ? string.Empty : cariDto.Aciklama6);
            cmd.Parameters.AddWithValue("@LimitAsimindaUyar", cariDto.LimitAsimindaUyar);
            cmd.Parameters.AddWithValue("@LimitAsimindaDurdur", cariDto.LimitAsimindaDurdur);
            cmd.Parameters.AddWithValue("@CekSenetRiski", cariDto.CekSenetRiski);
            cmd.Parameters.AddWithValue("@Limit", cariDto.Limit);
            cmd.Parameters.AddWithValue("@ServisPersoneli", cariDto.ServisPersoneli);
            cmd.Parameters.AddWithValue("@KullaniciAdi ", cariDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", cariDto.Parola);
            cmd.Parameters.AddWithValue("@RiskAciklama", cariDto.RiskAciklama);
            cmd.Parameters.AddWithValue("@PlasiyerID", "");
            cmd.Parameters.AddWithValue("@AnaCariID", "");
            cmd.Parameters.AddWithValue("@TeslimCariID", "");

            // 4. TCKimlikNo veya VergiNumarasi Eklenmesi
            if (cariDto.TCKimlikNoVergiNo.Length == 11)
            {
                cmd.Parameters.AddWithValue("@TCKimlikNo", cariDto.TCKimlikNoVergiNo);
                cmd.Parameters.AddWithValue("@VergiNumarasi", string.Empty);
            }
            else if (cariDto.TCKimlikNoVergiNo.Length == 10) 
            {
                cmd.Parameters.AddWithValue("@TCKimlikNo", string.Empty);
                cmd.Parameters.AddWithValue("@VergiNumarasi", cariDto.TCKimlikNoVergiNo);
            }

            // 5. Veritabanı İşlemi
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            // 6. İşlem Başarısı Mesajı
            if (dt.Rows.Count == 0)
            {
                return RedirectToAction("Giris", new { returnCode = 2000 });
            }

            // 7. Formu Yeniden Gönder
            ViewBag.Form = cariDto;
            return View();
        }

        [HttpGet]
        public ActionResult Giris()
        {
            string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
            if (!Request.IsLocal && !Request.IsSecureConnection && ConfigurationManager.AppSettings["SSLYonlendir"] == "1")
            {
                Response.Redirect(redirectUrl, false);
                HttpContext.ApplicationInstance.CompleteRequest();
            }
      
            return View();
        }
      
        [HttpPost]
        public ActionResult Giris(string txtKullaniciAdi, string txtParola)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ETicaret_KullaniciGirisi";
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
                    CreateCookie("UyelikID", Convert.ToString(dt.Rows[0]["UyelikID"]));                   
                    CreateCookie("KullaniciID", Convert.ToString(dt.Rows[0]["ID"]));
                    CreateCookie("KullaniciAdi", Convert.ToString(dt.Rows[0]["KullaniciAdi"]));
                    CreateCookie("Parola", Convert.ToString(dt.Rows[0]["Parola"]));
                    CreateCookie("Isim", Convert.ToString(dt.Rows[0]["Isim"]));
                    CreateCookie("Unvan", Convert.ToString(dt.Rows[0]["Unvan"]));             
                    #endregion

                    return RedirectToAction("AnaSayfa");
                }
            }

            return View(dt);
        }


        private void IlListesiniOlustur()
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
        private void UlkeListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand ulkeCommand = new SqlCommand();
            ulkeCommand.CommandText = "p_GrupKoduListesi";
            ulkeCommand.CommandType = System.Data.CommandType.StoredProcedure;
            ulkeCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            ulkeCommand.Parameters.AddWithValue("@Kod", "Ulke");
            ulkeCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable ulkeDataTable = (DataTable)IDVeritabani.Sorgula(ulkeCommand, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < ulkeDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(ulkeDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(ulkeDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.Ulkeler = entities;
        }

        #region Cookie İşlemleri



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
                cmd.CommandText = "p_ETicaret_KullaniciGirisi";
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
    }

}
