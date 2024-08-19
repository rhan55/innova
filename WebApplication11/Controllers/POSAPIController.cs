using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web.Http.Results;

namespace YKPortal.Controllers
{

    public class ParamPosService
    {
        private readonly HttpClient _httpClient;

        public ParamPosService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> SendPaymentRequest(UyelikOdemesiDto request)
        {
            var values = new Dictionary<string, string>
        {
            { "KrediKartIsim", request.KrediKartIsim },
            { "KrediKartNo", request.KrediKartNo },
            { "KrediKartSonKullanim", request.KrediKartSonKullanim },
            { "KrediKartCVV", request.KrediKartCVV },
            { "OrderID", request.OrderID },
            { "Tutar", request.Tutar.ToString() }
        };

            var content = new FormUrlEncodedContent(values);
            var response = await _httpClient.PostAsync("https://api.parampos.com.tr/odeme", content);

            return await response.Content.ReadAsStringAsync();
        }
    }


    public class POSAPIController : Controller
    {
     

        // GET: POSAPI
        [HttpGet]
        public ActionResult UyelikPaketleri()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikPaketleri";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            UyelikPaketleriViewModel model = new UyelikPaketleriViewModel();
            model.UyelikPaketleri = dt;

            return View(model);
        }

        [HttpGet]
        public ActionResult UyelikOdemesi(string paketID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.PaketID = paketID;

            return View();
        }

        [HttpPost]
        public ActionResult UyelikOdemesi(UyelikOdemesiDto uyelikOdemesi)
        {
            var uyelikPaketi = UyelikPaketiGetir(uyelikOdemesi.UyelikPaketID);
            if (uyelikPaketi == null)
            {
                ViewBag.Hata = "İstenilen paket değerleri bulunamadı";
                return View();
            }
            var orderId = Guid.NewGuid().ToString();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Uygulama", "PARAMPOS");
            cmd.Parameters.AddWithValue("@Tutar", uyelikPaketi.Tutar); // Paket tutarını al
            cmd.Parameters.AddWithValue("@UzatilacakAy", uyelikPaketi.Ay); // Paket süresini al
            cmd.Parameters.AddWithValue("@OrderID", orderId);
            cmd.Parameters.AddWithValue("@KrediKartIsim", uyelikOdemesi.KrediKartIsim);
            cmd.Parameters.AddWithValue("@KrediKartNo", uyelikOdemesi.KrediKartNo);
            cmd.Parameters.AddWithValue("@KrediKartSonKullanim", uyelikOdemesi.KrediKartSonKullanim);
            cmd.Parameters.AddWithValue("@KrediKartCVV", uyelikOdemesi.KrediKartCVV);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            return View();

        }

        private UyelikPaketDto UyelikPaketiGetir(string uyelikPaketID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_UyelikPaketleri";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var entities = new List<UyelikPaketDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UyelikPaketDto entity = new UyelikPaketDto();
                entity.ID = Convert.ToString(dt.Rows[i]["ID"]);
                entity.Isim = Convert.ToString(dt.Rows[i]["Isim"]);
                entity.Ay = Convert.ToString(dt.Rows[i]["Ay"]);
                entity.Tutar = Convert.ToString(dt.Rows[i]["Tutar"]);
                entity.ResimUrl = Convert.ToString(dt.Rows[i]["ResimUrl"]);
                entity.Aciklama = Convert.ToString(dt.Rows[i]["Aciklama"]);
                entities.Add(entity);
            }

            try
            {
               var entity = entities.FirstOrDefault(m => m.ID == uyelikPaketID);
                return entity;
            } catch (ArgumentNullException exception)
            {
                return null;
            } 
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
    }



}
    

