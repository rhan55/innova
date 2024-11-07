using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;
using System.Configuration;
using YKEFaturaEntegrasyon.Dto;

namespace YKPortal.Controllers
{
    public class AyarlarController : Controller
    {

        [HttpGet]
        public ActionResult ConfigAyarlari()
        {
            var kullanici = KullaniciGetir(GetCookie("KullaniciID"));

            if (kullanici == null || !kullanici.KullaniciAdi.Contains("@ykyazilim.com.tr"))
            {
                return Redirect("~/YK/AnaSayfa");
            }

            var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

            // B2B Ayarlari
            var b2bAyarlariDto = new B2BAyarlariDto();

            b2bAyarlariDto.B2BLogoKullaniciAdi = config.AppSettings.Settings["B2BLogoKullaniciAdi"].Value;
            b2bAyarlariDto.B2BLogoSirket = config.AppSettings.Settings["B2BLogoSirket"].Value;
            b2bAyarlariDto.B2BLogoParola = config.AppSettings.Settings["B2BLogoParola"].Value;
            // Seo Ayarlari
            var seoAyarlariDto = new SeoAyarlariDto();

            seoAyarlariDto.SeoDescription = config.AppSettings.Settings["SeoDescription"].Value;
            seoAyarlariDto.SeoKeywords = config.AppSettings.Settings["SeoKeywords"].Value;
            seoAyarlariDto.FirmaAdi = config.AppSettings.Settings["FirmaAdi"].Value;

            seoAyarlariDto.SSLYonlendir = BoolKontrolu(config.AppSettings.Settings["SSLYonlendir"].Value);
            seoAyarlariDto.AnaSayfadaAcilisSayfasiKontrolu = BoolKontrolu(config.AppSettings.Settings["AnaSayfadaAcilisSayfasiKontrolu"].Value);
            seoAyarlariDto.IlkUyelikdeKullaniciyiOnayliYap = BoolKontrolu(config.AppSettings.Settings["IlkUyelikdeKullaniciyiOnayliYap"].Value);
            seoAyarlariDto.YeniUyelikKaydi = BoolKontrolu(config.AppSettings.Settings["YeniUyelikKaydi"].Value);
            seoAyarlariDto.SifremiUnuttum = BoolKontrolu(config.AppSettings.Settings["SifremiUnuttum"].Value);
            // Sms Ayarlari

            var smsAyarlariDto = new SmsAyarlariDto();

            smsAyarlariDto.SmsKullaniciAdi = config.AppSettings.Settings["SmsKullaniciAdi"].Value;
            smsAyarlariDto.SmsParola = config.AppSettings.Settings["SmsParola"].Value;
            smsAyarlariDto.SmsIsim = config.AppSettings.Settings["SmsIsim"].Value;


            ViewBag.B2B = b2bAyarlariDto;
            ViewBag.SEO = seoAyarlariDto;
            ViewBag.SMS = smsAyarlariDto;

            return View();
        }

      
        [HttpPost]
        public JsonResult B2bAyarlari(B2BAyarlariDto b2bAyarlariDto)
        {
            try
            {
                var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

                config.AppSettings.Settings["B2BLogoKullaniciAdi"].Value = b2bAyarlariDto.B2BLogoKullaniciAdi;
                config.AppSettings.Settings["B2BLogoSirket"].Value = b2bAyarlariDto.B2BLogoSirket;
                config.AppSettings.Settings["B2BLogoParola"].Value = b2bAyarlariDto.B2BLogoParola;

                config.Save();

                return Json(new { success = true, message = "B2B ayarları başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SeoAyarlari(SeoAyarlariDto seoAyarlariDto)
        {
            try
            {
                var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");


                config.AppSettings.Settings["SeoDescription"].Value = seoAyarlariDto.SeoDescription;
                config.AppSettings.Settings["SeoKeywords"].Value = seoAyarlariDto.SeoKeywords;
                config.AppSettings.Settings["FirmaAdi"].Value = seoAyarlariDto.FirmaAdi;

                // Boolean conversion
                config.AppSettings.Settings["SSLYonlendir"].Value = seoAyarlariDto.SSLYonlendir.ToString();
                config.AppSettings.Settings["AnaSayfadaAcilisSayfasiKontrolu"].Value = seoAyarlariDto.AnaSayfadaAcilisSayfasiKontrolu.ToString();
                config.AppSettings.Settings["IlkUyelikdeKullaniciyiOnayliYap"].Value = seoAyarlariDto.IlkUyelikdeKullaniciyiOnayliYap.ToString();
                config.AppSettings.Settings["YeniUyelikKaydi"].Value = seoAyarlariDto.YeniUyelikKaydi.ToString();
                config.AppSettings.Settings["SifremiUnuttum"].Value = seoAyarlariDto.SifremiUnuttum.ToString();


                config.Save();

                return Json(new { success = true, message = "SEO ayarları başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SmsAyarlari(SmsAyarlariDto smsAyarlariDto)
        {
            try
            {
                var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");


                config.AppSettings.Settings["SmsKullaniciAdi"].Value = smsAyarlariDto.SmsKullaniciAdi;
                config.AppSettings.Settings["SmsParola"].Value = smsAyarlariDto.SmsParola;
                config.AppSettings.Settings["SmsIsim"].Value = smsAyarlariDto.SmsIsim;

                config.Save();


                return Json(new { success = true, message = "SMS ayarları başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }


        [HttpGet]
        public ActionResult Entegrasyonlar()
        {
            Entegratorler();
            var kullanici = KullaniciGetir(GetCookie("KullaniciID"));

            if (kullanici == null || !kullanici.KullaniciAdi.Contains("@ykyazilim.com.tr"))
            {
                return Redirect("~/YK/AnaSayfa");
            }

            var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

            // Logo Ayarlari
            var eFaturaLogoPostBoxServiceDto = new EFaturaAyarlariDto();

            eFaturaLogoPostBoxServiceDto.ServiceUrl = config.AppSettings.Settings["EFaturaLogoPostBoxServiceUrl"].Value;
            eFaturaLogoPostBoxServiceDto.KullaniciAdi = config.AppSettings.Settings["EFaturaLogoPostBoxServiceKullaniciAdi"].Value;
            eFaturaLogoPostBoxServiceDto.Sifre = config.AppSettings.Settings["EFaturaLogoPostBoxServiceSifre"].Value;
            eFaturaLogoPostBoxServiceDto.GrupKodu = "Logo";

            var eFaturaEdmDto = new EFaturaAyarlariDto();

            //eFaturaEdmDto.ServiceUrl = config.AppSettings.Settings["EFaturaEdmServiceUrl"].Value;
            //eFaturaEdmDto.KullaniciAdi = config.AppSettings.Settings["EFaturaEdmKullaniciAdi"].Value;
            //eFaturaEdmDto.Sifre = config.AppSettings.Settings["EFaturaEdmSifre"].Value;
            eFaturaEdmDto.GrupKodu = "EDM";

            var eFaturaVbtDto = new EFaturaAyarlariDto();

            //eFaturaVbtDto.ServiceUrl = config.AppSettings.Settings["EFaturaVbtServiceUrl"].Value;
            //eFaturaVbtDto.KullaniciAdi = config.AppSettings.Settings["EFaturaVbtKullaniciAdi"].Value;
            //eFaturaVbtDto.Sifre = config.AppSettings.Settings["EFaturaVbtSifre"].Value;
            eFaturaVbtDto.GrupKodu = "VBT";

            ViewBag.EFaturaEntegratorler = new List<EFaturaAyarlariDto> {
                eFaturaLogoPostBoxServiceDto,
                eFaturaEdmDto,
                eFaturaVbtDto
            };
            

            return View();
        }

        [HttpPost]
        public JsonResult EFaturaAyarlari(EFaturaAyarlariDto eFaturaAyarlariDto)
        {
            if (string.IsNullOrWhiteSpace(eFaturaAyarlariDto.GrupKodu))
            {
                return Json(new { success = false, message = "Grup Kodu Gönderilmeli"});

            }

            var kullanici = KullaniciGetir(GetCookie("KullaniciID"));

            if (kullanici == null || !kullanici.KullaniciAdi.Contains("@ykyazilim.com.tr"))
            {
                return Json(new { success = true, redirectUrl = Url.Action("AnaSayfa", "YK") });

            }

            var entegratorler = Entegratorler();

            if (!entegratorler.Any(m => m.Deger == eFaturaAyarlariDto.GrupKodu))
            {
                return Json(new { success = false, message = "Grup Kodu Hatalı" });
            }

            try
            {
                
                var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

                switch(eFaturaAyarlariDto.GrupKodu)
                {
                    case "EDM":
                    
                        break;
                    case "Logo":
                        config.AppSettings.Settings["EFaturaLogoPostBoxServiceUrl"].Value = eFaturaAyarlariDto.ServiceUrl;
                        config.AppSettings.Settings["EFaturaLogoPostBoxServiceKullaniciAdi"].Value = eFaturaAyarlariDto.KullaniciAdi;
                        config.AppSettings.Settings["EFaturaLogoPostBoxServiceSifre"].Value = eFaturaAyarlariDto.Sifre;
                        break;

                    case "VBT":

                        break;
                }


                config.Save();


                return Json(new { success = true, message = "EFatura ayarları başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Seriler()
        {
            return View();
        }
        private bool BoolKontrolu(string key)
        {
            return key == "True";
        }



        private  List<GrupKoduDto> Entegratorler()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand entegratorCommand = new SqlCommand();
            entegratorCommand.CommandText = "p_GrupKoduListesi";
            entegratorCommand.CommandType = System.Data.CommandType.StoredProcedure;
            entegratorCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            entegratorCommand.Parameters.AddWithValue("@Kod", "EFaturaEntegrator");
            entegratorCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable entegratorDataTable = (DataTable)IDVeritabani.Sorgula(entegratorCommand, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < entegratorDataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(entegratorDataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(entegratorDataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.Entegratorler = entities;

            return entities;
        }
        private KullaniciEkleDto KullaniciGetir(string ID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count == 0)
            {
                return new KullaniciEkleDto();
            }

            return new KullaniciEkleDto
            {
                Ad = Convert.ToString(dt.Rows[0]["Ad"]),
                KullaniciAdi = Convert.ToString(dt.Rows[0]["KullaniciAdi"])
            };
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

        private class KullaniciListesi
        {

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
        #endregion

    }


}
