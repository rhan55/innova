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
using YKPortal.YKPARAMPOS;

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
            var paramposOdeme = new ST_TP_Islem_Odeme();

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

        public ActionResult BasariliOdeme(string TURKPOS_RETVAL_Sonuc = "", string TURKPOS_RETVAL_Sonuc_Str = "",
            string TURKPOS_RETVAL_Dekont_ID = "", string TURKPOS_RETVAL_Tahsilat_Tutari = "", string TURKPOS_RETVAL_Odeme_Tutari = "",
            string TURKPOS_RETVAL_Siparis_ID = "")
        {

            string sonuc = "";
            if (Request.Form.AllKeys.Length > 0)
            {
                string[] _allkeys = Request.Form.AllKeys;
                string _sonuc = "Sonuc:";
                foreach (var item in _allkeys)
                {
                    if (Request.Form.GetValues(item)[0] != null)
                    {
                        _sonuc += "|" + item + ":" + Request.Form.GetValues(item)[0];
                    }
                }
                if (Convert.ToInt32(TURKPOS_RETVAL_Sonuc) > 0)
                {
                    sonuc = "Ödeme başarılı bir şekilde alınmıştır. <br />Onay Kodunuz : " + TURKPOS_RETVAL_Dekont_ID + " ";

                    string UyelikID = GetCookie("UyelikID");
                    string KullaniciID = GetCookie("KullaniciID");
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "p_UyelikOdemesiTamamla";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                    cmd.Parameters.AddWithValue("@Uygulama", "PARAMPOS");
                    cmd.Parameters.AddWithValue("@OrderID ", TURKPOS_RETVAL_Siparis_ID);
                    cmd.Parameters.AddWithValue("@Durumu ", "Başarılı");
                    cmd.Parameters.AddWithValue("@SonucKodu ", sonuc);
                    cmd.Parameters.AddWithValue("@SonucAciklama ", _sonuc);
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }
            }
            return Redirect("~/POSAPI/Bilgilendirme/?Mesaj="+sonuc);
        }

        public ActionResult BasarisizOdeme(string TURKPOS_RETVAL_Sonuc = "", string TURKPOS_RETVAL_Sonuc_Str = "",
            string TURKPOS_RETVAL_Dekont_ID = "", string TURKPOS_RETVAL_Tahsilat_Tutari = "", string TURKPOS_RETVAL_Odeme_Tutari = "",
            string TURKPOS_RETVAL_Siparis_ID = "")
        {
            string sonuc = "";
            if (Request.Form.AllKeys.Length > 0)
            {
                string[] _allkeys = Request.Form.AllKeys;
                string _sonuc = "Sonuc:";
                foreach (var item in _allkeys)
                {
                    if (Request.Form.GetValues(item)[0] != null)
                    {
                        _sonuc += "|" + item + ":" + Request.Form.GetValues(item)[0];
                    }
                }
                if (Convert.ToInt32(TURKPOS_RETVAL_Sonuc) <= 0)
                {
                    sonuc = "Ödeme başarısız. <br />İşlem Açıklaması : " + TURKPOS_RETVAL_Sonuc_Str + " ";

                    string UyelikID = GetCookie("UyelikID");
                    string KullaniciID = GetCookie("KullaniciID");
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "p_UyelikOdemesiTamamla";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                    cmd.Parameters.AddWithValue("@Uygulama", "PARAMPOS");
                    cmd.Parameters.AddWithValue("@OrderID ", TURKPOS_RETVAL_Siparis_ID);
                    cmd.Parameters.AddWithValue("@Durumu ", "Başarısız");
                    cmd.Parameters.AddWithValue("@SonucKodu ", sonuc);
                    cmd.Parameters.AddWithValue("@SonucAciklama ", _sonuc);
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }
            }
            return Redirect("~/POSAPI/Bilgilendirme/?Mesaj=" + sonuc);
        }

        [ValidateInput(false)]
        public ActionResult Bilgilendirme(string Mesaj)
        {
            ViewBag.Mesaj = Mesaj;
            return View();
        }

        [HttpGet]
        public ActionResult UyelikOdemesi(string paketID)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.Paket = UyelikPaketiGetir(paketID);

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
            cmd.CommandText = "p_UyelikOdemesiOlustur";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Uygulama", "PARAMPOS");
            cmd.Parameters.AddWithValue("@Tutar", Convert.ToDecimal(uyelikPaketi.Tutar)); // Paket tutarını al
            cmd.Parameters.AddWithValue("@UzatilacakAy", uyelikPaketi.Ay); // Paket süresini al
            cmd.Parameters.AddWithValue("@OrderID", orderId);
            cmd.Parameters.AddWithValue("@Durum", string.Empty);
            cmd.Parameters.AddWithValue("@KrediKartIsim", uyelikOdemesi.KrediKartIsim);
            cmd.Parameters.AddWithValue("@KrediKartNo", uyelikOdemesi.KrediKartNo);
            cmd.Parameters.AddWithValue("@KrediKartSonKullanim", $"{uyelikOdemesi.KrediKartiSonKullanimAy}/{uyelikOdemesi.KrediKartiSonKullanimYil}");
            cmd.Parameters.AddWithValue("@KrediKartCVV", uyelikOdemesi.KrediKartCVV);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            // parampos odeme gerceklesicek
            var paramposPosOdeme = new TurkPosWSTEST();

            var CLIENT_CODE = "88503";
            var CLIENT_USERNAME = "TP10113004";
            var CLIENT_PASSWORD = "B539B78228C91FD9";
            var GUID = "8C885165-F05D-47C1-BF40-A116EF6A7FA2";
            var hataUrl = Request.Url.Scheme+"://"+ Request.Url.Authority + "/POSAPI/BasarisizOdeme";
            var basariliUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/POSAPI/BasariliOdeme";
            var refUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/POSAPI/UyelikOdemesi";
            var guvenlikNesnesi = new ST_WS_Guvenlik
            {
                CLIENT_CODE = CLIENT_CODE,
                CLIENT_USERNAME = CLIENT_USERNAME,
                CLIENT_PASSWORD = CLIENT_PASSWORD
            };
            var kullaniciIpAdresi = GetIPAddress();


            uyelikOdemesi.Tutar = uyelikPaketi.Tutar;
            uyelikOdemesi.OrderID = orderId;
            //string sifrele = $"{CLIENT_CODE}{GUID}{1}{uyelikOdemesi.Tutar}{uyelikOdemesi.Tutar}{uyelikOdemesi.OrderID}{hataUrl}{basariliUrl}";

            var spid = 0;
            var dtBinsonuc = paramposPosOdeme.BIN_SanalPos(guvenlikNesnesi, uyelikOdemesi.KrediKartNo.Substring(0, 8));
            DataTable dtPosID = dtBinsonuc.DT_Bilgi;
            if (dtPosID.Rows.Count > 0)
                spid = Convert.ToInt32(dtPosID.Rows[0]["SanalPOS_ID"]);

            var odemeUrl = String.Format("{0}://{1}/", Request.Url.Scheme, Request.Url.Authority, "turkpos.api/ETSPreIslemOdeme.aspx");

            string sifrele = CLIENT_CODE + GUID + Convert.ToInt32(spid) + "1" + uyelikOdemesi.Tutar + uyelikOdemesi.Tutar + uyelikOdemesi.OrderID + hataUrl + basariliUrl;




            var islemGuvenlikHash = paramposPosOdeme.SHA2B64(sifrele);
            //var sonuc = paramposPosOdeme.Pos_Odeme(G: guvenlikNesnesi, GUID: GUID, KK_Sahibi: uyelikOdemesi.KrediKartIsim, KK_No: uyelikOdemesi.KrediKartNo, uyelikOdemesi.KrediKartiSonKullanimAy, uyelikOdemesi.KrediKartiSonKullanimYil, uyelikOdemesi.KrediKartCVV, "telefon", hataUrl, basariliUrl, uyelikOdemesi.OrderID, uyelikOdemesi.Uygulama, 1, uyelikOdemesi.Tutar, uyelikOdemesi.Tutar, islemGuvenlikHash, "3D", "", kullaniciIpAdresi, refUrl, "", "", "", "", "", "", "", "", "", "");
            var sonuc = paramposPosOdeme.TP_Islem_Odeme(guvenlikNesnesi, Convert.ToInt32(spid), GUID,
                uyelikOdemesi.KrediKartIsim, uyelikOdemesi.KrediKartNo, uyelikOdemesi.KrediKartiSonKullanimAy, uyelikOdemesi.KrediKartiSonKullanimYil,
                uyelikOdemesi.KrediKartCVV, "5355089134", hataUrl, basariliUrl, uyelikOdemesi.OrderID, "", Convert.ToInt32("1"),
                uyelikOdemesi.Tutar.ToString().Replace(".", ","), uyelikOdemesi.Tutar.ToString().Replace(".", ","), islemGuvenlikHash, "123", "127.0.0.1", odemeUrl, "", "", "", "", "");

            object result = null;
            if (Convert.ToInt32(sonuc.Sonuc) >= 0)
            {
                var msg = String.Format(@"PostParam: {0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n",
                            "TURKPOS_RETVAL_Sonuc", "TURKPOS_RETVAL_Sonuc_Str", "TURKPOS_RETVAL_Dekont_ID",
                            "TURKPOS_RETVAL_Tahsilat_Tutari", "TURKPOS_RETVAL_Odeme_Tutari", "TURKPOS_RETVAL_Siparis_ID", "ekbilgi");
                ViewBag.Mesaj1 = msg;
                ViewBag.Mesaj2 = sonuc.UCD_URL;
                Response.Redirect(sonuc.UCD_URL);

            }
            return View("~/Views/POSAPI/OdemeEkrani/?Bilgi=" + sonuc.Sonuc_Str, result);

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
            }
            catch (ArgumentNullException exception)
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

        private string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }



}


