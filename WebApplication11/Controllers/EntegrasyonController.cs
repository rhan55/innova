using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;

namespace YKPortal.Controllers
{
    public class EntegrasyonController : Controller
    {
        // GET: Entegrasyon
        public ActionResult AntOto1()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            return View();
        }

        // POST: Entegrasyon/AntOto1AktarimYap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AntOto1AktarimYap(int yil)
        {
            try
            {
                // Read records from DB for the selected year
                var cmd = new SqlCommand();
                cmd.CommandText = @"SELECT SLIPNR, TARIH, CODE, DESCRIPTION, DEBIT, CREDIT, LINENR, LINEEXP, MASRAF_MERKEZI, PERSONEL_KODU, PERSONEL_ACIKLAMASI, AUXCODE, AY, YIL 
                                    FROM IYB_JV_MUHASEBE_FISLERI_TABLO 
                                    WHERE YIL = @YIL";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@YIL", yil);

                DataTable dt = (DataTable)Models.IDVeritabani.Sorgula(cmd, Models.SorgulaTuru.Tablo);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return Json(new { success = true, message = "Seçilen yıl için aktarılacak kayıt bulunamadı." });
                }

                // Build payload list
                var payloadList = new List<object>();
                foreach (DataRow row in dt.Rows)
                {
                    string sirketkodu = "1000"; // if company code exists elsewhere, replace accordingly
                    string fisno = row["SLIPNR"]?.ToString();
                    string tarih = "";
                    if (row["TARIH"] != DBNull.Value)
                    {
                        DateTime dtTarih;
                        if (DateTime.TryParse(row["TARIH"].ToString(), out dtTarih))
                            tarih = dtTarih.ToString("yyyy-MM-dd");
                    }
                    string anahesap = row["CODE"]?.ToString();
                    if (string.IsNullOrEmpty(anahesap))
                        anahesap = row["AUXCODE"]?.ToString();
                    string aciklama = row["DESCRIPTION"]?.ToString();
                    if (string.IsNullOrEmpty(aciklama))
                        aciklama = row["LINEEXP"]?.ToString();
                    string borc = row["DEBIT"] != DBNull.Value ? Convert.ToString(row["DEBIT"]) : "0";
                    string alacak = row["CREDIT"] != DBNull.Value ? Convert.ToString(row["CREDIT"]) : "0";
                    string masrafyeri = row["MASRAF_MERKEZI"]?.ToString();
                    string personelkodu = row["PERSONEL_KODU"]?.ToString();

                    payloadList.Add(new {
                        sirketkodu = sirketkodu,
                        fisno = fisno,
                        tarih = tarih,
                        anahesap = anahesap,
                        aciklama = aciklama,
                        borc = borc,
                        alacak = alacak,
                        masrafyeri = masrafyeri,
                        personelkodu = personelkodu
                    });
                }

                var payload = JsonConvert.SerializeObject(payloadList);

                // Endpoint and credentials from Postman example
                var url = "https://integration-suite-dev-test-69ldgkyu.it-cpi024-rt.cfapps.eu10-002.hana.ondemand.com/http/Logo_Bordro_Inbound";
                var username = "sb-d71d2cd5-4862-41b7-9a76-e48995a2956c!b552840|it-rt-integration-suite-dev-test-69ldgkyu!b182722";
                var password = "290f827e-51a8-4c59-8046-708416a28a40$IgI8poO4RIilZ1-WnTMZuHeD09cQmev845SFVzHBr8A=";

                using (var http = new HttpClient())
                {
                    var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
                    http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(payload, Encoding.UTF8, "application/json");
                    var response = await http.PostAsync(url, content).ConfigureAwait(false);
                    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = responseText });
                    }
                    else
                    {
                        return Json(new { success = false, message = $"HTTP {(int)response.StatusCode} - {response.ReasonPhrase}: {responseText}" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
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
    }
}