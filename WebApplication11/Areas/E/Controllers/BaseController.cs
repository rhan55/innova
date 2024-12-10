using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Areas.E.Models.Dto;
using YKPortal.Models;
using System.IO;
using System.Net;
using System.Text;
using YKPortal.Models.Dto;
using System.Security.Principal;
using System.Web.Security;
using System.Text.Json;
using YKEFaturaEntegrasyon.Dto;

namespace YKPortal.Areas.E.Controllers
{
    public abstract class BaseController : Controller
    {
        public KullaniciEkleDto Kullanici { get; set; } = new KullaniciEkleDto();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.User == null || !HttpContext.User.Identity.IsAuthenticated)
            {
                var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {
                        string[] roles = new string[] { "Profil" };
                        var identity = new FormsIdentity(authTicket);

                        var kullanici = JsonSerializer.Deserialize<KullaniciEkleDto>(authTicket.UserData);

                        Kullanici = kullanici;

                        HttpContext.User = new System.Security.Principal.GenericPrincipal(identity, roles);
                    }
                }
            }

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.KullaniciAdi = User.Identity.Name;
                ViewBag.GirisYapildi = true;
                ViewBag.SepetBilgileri = SepetListesiniGetir();
            }
            else
            {
                ViewBag.KullaniciAdi = "Ziyaretçi";
                ViewBag.GirisYapildi = false;
            }

            // Yönlendirme sadece yetki gerektiren sayfalarda yapılır
            if (IsAuthorizationRequired(filterContext))
            {
                filterContext.Result = new RedirectResult("~/E/ETicaretYetkilendirme/Giris");
                return;
            }

            // Genel kategoriler veya diğer veriler
            if (filterContext.HttpContext.Request.HttpMethod == "GET")
            {
                var kategoriler = KategorileriGetir();
                ViewBag.Kategoriler = kategoriler;
            }

            base.OnActionExecuting(filterContext);
        }

        // Hangi sayfalar için yetkilendirme gerektiğini belirle
        private bool IsAuthorizationRequired(ActionExecutingContext filterContext)
        {
            // Yetki gerektiren sayfalar listesi
            var yetkiGerekenSayfalar = new[]
            {
                "Profil",
                "GenelBilgiler"

            };

            var controller = filterContext.RouteData.Values["controller"]?.ToString();
            var action = filterContext.RouteData.Values["action"]?.ToString();

            // Yetki gerektiren bir sayfa mı?
            return yetkiGerekenSayfalar.Contains(controller + "/" + action);
        }


        protected List<ETicaretKategorilerDto> KategorileriGetir()
        {


            SqlCommand cmd = new SqlCommand
            {
                CommandText = "p_ETicaret_Kategoriler",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UyelikID", UyelikIDGetir());
            cmd.Parameters.AddWithValue("@CariId", Kullanici.ID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var kategoriler = new List<ETicaretKategorilerDto>();
            foreach (DataRow row in dt.Rows)
            {
                kategoriler.Add(new ETicaretKategorilerDto
                {
                    Kategori1 = row["Kategori1"].ToString(),
                    Kategori2 = row["Kategori2"].ToString(),
                    Kategori3 = row["Kategori3"].ToString(),
                    Kategori4 = row["Kategori4"].ToString(),
                    Kategori5 = row["Kategori5"].ToString(),
                    Kategori6 = row["Kategori6"].ToString(),
                });
            }

            return kategoriler;
        }

        protected ETicaretStokDto.ETicaretStokSonucDto StokBul(ETicaretStokDto.ETicaretStokSorguDto stokSorguDto)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "[p_ETicaret_Stok]",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UyelikID", UyelikIDGetir());
            cmd.Parameters.AddWithValue("@CariId", Kullanici.ID);
            cmd.Parameters.AddWithValue("@StokID", stokSorguDto.StokID);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                return new ETicaretStokDto.ETicaretStokSonucDto
                {
                    StokID = dt.Rows[0]["StokID"].ToString(),
                    UyelikID = dt.Rows[0]["UyelikID"].ToString(),
                    Kod = dt.Rows[0]["Kod"].ToString(),
                    Isim = dt.Rows[0]["Isim"].ToString(),
                    Aciklama = dt.Rows[0]["Aciklama"].ToString(),
                    Barkod = dt.Rows[0]["Barkod"].ToString(),
                    OlcuBirimi = dt.Rows[0]["OlcuBirimi"].ToString(),
                    KdvSatis = dt.Rows[0]["KdvSatis"].ToString(),
                    IskontoSatis1 = Convert.ToDecimal(dt.Rows[0]["IskontoSatis1"]),
                    Marka = dt.Rows[0]["Marka"].ToString(),
                    Model = dt.Rows[0]["Model"].ToString(),
                    Renk = dt.Rows[0]["Renk"].ToString(),
                    Beden = dt.Rows[0]["Beden"].ToString(),
                    Kalite = dt.Rows[0]["Kalite"].ToString(),
                    UreticiFirma = dt.Rows[0]["UreticiFirma"].ToString(),
                    Fiyat = Convert.ToDecimal(dt.Rows[0]["Fiyat"]),
                    Resim1 = dt.Rows[0]["Resim1"].ToString()
                };
            }

            return new ETicaretStokDto.ETicaretStokSonucDto();
        }

        protected List<ETicaretStokDto.ETicaretStokSonucDto> StokGetir(ETicaretStokDto.ETicaretStokSorguDto stokSorguDto)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "p_ETicaret_Stoklar",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UyelikID", UyelikIDGetir());
            cmd.Parameters.AddWithValue("@CariId", Kullanici.ID);
            cmd.Parameters.AddWithValue("@Kategori1", stokSorguDto.Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", stokSorguDto.Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", stokSorguDto.Kategori3);
            cmd.Parameters.AddWithValue("@Kategori4", stokSorguDto.Kategori4);
            cmd.Parameters.AddWithValue("@Kategori5", stokSorguDto.Kategori5);
            cmd.Parameters.AddWithValue("@Kategori6", stokSorguDto.Kategori6);
            cmd.Parameters.AddWithValue("@AranacakKelime", stokSorguDto.AranacakKelime);
            cmd.Parameters.AddWithValue("@Sayfa", stokSorguDto.Sayfa);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var stoklar = new List<ETicaretStokDto.ETicaretStokSonucDto>();
            foreach (DataRow row in dt.Rows)
            {
                stoklar.Add(new ETicaretStokDto.ETicaretStokSonucDto
                {
                    StokID = row["StokID"].ToString(),
                    UyelikID = row["UyelikID"].ToString(),
                    Kod = row["Kod"].ToString(),
                    Isim = row["Isim"].ToString(),
                    Aciklama = row["Aciklama"].ToString(),
                    Barkod = row["Barkod"].ToString(),
                    OlcuBirimi = row["OlcuBirimi"].ToString(),
                    KdvSatis = row["KdvSatis"].ToString(),
                    IskontoSatis1 = Convert.ToDecimal(row["IskontoSatis1"]),
                    Marka = row["Marka"].ToString(),
                    Model = row["Model"].ToString(),
                    Renk = row["Renk"].ToString(),
                    Beden = row["Beden"].ToString(),
                    Kalite = row["Kalite"].ToString(),
                    UreticiFirma = row["UreticiFirma"].ToString(),
                    Resim1 = row["Resim1"].ToString(),
                    Fiyat = Convert.ToDecimal(row["Fiyat"]),
                   
                });
            }

            return stoklar;
        }

        protected List<ETicaretSepetDto.ETicaretSepetListeSonucDto> SepetListesiniGetir() {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "[p_ETicaret_SepetListesi]",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UyelikID", UyelikIDGetir());
            cmd.Parameters.AddWithValue("@CariId", Kullanici.ID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            var sepetler = new List<ETicaretSepetDto.ETicaretSepetListeSonucDto>();

            if (dt.Rows.Count > 0)
            {
                sepetler.Add(new ETicaretSepetDto.ETicaretSepetListeSonucDto
                {
                    ID = dt.Rows[0]["ID"].ToString(),
                    CariID = dt.Rows[0]["CariID"].ToString(),
                    StokID = dt.Rows[0]["StokID"].ToString(),
                    UyelikID = dt.Rows[0]["UyelikID"].ToString(),
                    Kod = dt.Rows[0]["Kod"].ToString(),
                    Isim = dt.Rows[0]["Isim"].ToString(),
                    OlcuBirimi = dt.Rows[0]["OlcuBirimi"].ToString(),
                    Fiyat = Convert.ToDecimal(dt.Rows[0]["Fiyat"]),
                    Resim1 = dt.Rows[0]["Resim1"].ToString(),
                    KayitTarihi = dt.Rows[0]["KayitTarihi"].ToString(),
                    Miktar = Convert.ToDecimal(dt.Rows[0]["Miktar"]),
                    DovizBirimi = dt.Rows[0]["DovizBirimi"].ToString(),
                    Silindi = Convert.ToBoolean(dt.Rows[0]["Silindi"]),

                });
            }

            return sepetler;

        }

        protected bool SepetKaydet(ETicaretSepetDto.ETicaretSepetEkleDto eTicaretSepetEkleDto)
        {
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "[p_ETicaret_SepetEkle]",
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UyelikID", UyelikIDGetir());
                cmd.Parameters.AddWithValue("@CariId", Kullanici.ID);
                cmd.Parameters.AddWithValue("@StokID", eTicaretSepetEkleDto.StokID);
                cmd.Parameters.AddWithValue("@OlcuBirimi", eTicaretSepetEkleDto.OlcuBirimi);
                cmd.Parameters.AddWithValue("@Miktar", eTicaretSepetEkleDto.Miktar);
                cmd.Parameters.AddWithValue("@Fiyat", eTicaretSepetEkleDto.Fiyat);
                cmd.Parameters.AddWithValue("@DovizBirimi", eTicaretSepetEkleDto.DovizBirimi);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        #region Cookie İşlemleri



        protected void CreateCookie(string name, string value)
        {
            HttpCookie cookieVisitor = new HttpCookie(name, Server.UrlEncode(value));
            // cookieVisitor.Expires = DateTime.Now.AddDays(2);
            Response.Cookies.Add(cookieVisitor);
        }
        protected string GetCookie(string name)
        {
            //Böyle bir cookie mevcut mu kontrol ediyoruz
            if (Request.Cookies.AllKeys.Contains(name))
            {
                //böyle bir cookie varsa bize geri değeri döndürsün
                return Server.UrlDecode(Request.Cookies[name].Value);
            }
            return null;
        }
        protected void DeleteCookie(string name)
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

        protected void IlListesiniOlustur()
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
        protected void UlkeListesiniOlustur()
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


        protected bool AutoGirisKontrol()
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

        protected string UyelikIDGetir()
        {
            return System.Configuration.ConfigurationManager.AppSettings["UyelikID"];
        }

    }
}