using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YKPortal.Models;
using YKPortal.Models.YKClasses;

namespace YKPortal.Controllers
{

    public class YKWebApiController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage KullaniciGirisi(string KullaniciAdi, string Parola)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                YKModelKullanici entity = new YKModelKullanici();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_KullaniciGirisi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
                cmd.Parameters.AddWithValue("@Parola", Parola);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (!Bilgi.StartsWith("UYARI!"))
                    {
                        #region Cookie İşlemleri
                        entity.Isim = Convert.ToString(dt.Rows[0]["Ad"]) + " " + Convert.ToString(dt.Rows[0]["Soyad"]);
                        entity.KullaniciID = Convert.ToString(dt.Rows[0]["ID"]);
                        entity.UyelikIsim = Convert.ToString(dt.Rows[0]["UyelikIsim"]);
                        entity.UyelikID = Convert.ToString(dt.Rows[0]["UyelikID"]);
                        entity.KullaniciAdi = Convert.ToString(dt.Rows[0]["KullaniciAdi"]);
                        entity.Parola = Convert.ToString(dt.Rows[0]["Parola"]);
                        entity.Resim = Convert.ToString(dt.Rows[0]["Resim"]);

                        #endregion
                        response = Request.CreateResponse<YKModelKullanici>(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        entity.KullaniciID = "";
                        response = Request.CreateResponse<string>(HttpStatusCode.OK, Bilgi);
                    }
                }
                else
                {
                    entity.KullaniciID = "";
                    response = Request.CreateResponse<string>(HttpStatusCode.OK, "UYARI! Kullanıcı bulunamadı!");
                }



            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                
            }
            return response;
        }
    }
}