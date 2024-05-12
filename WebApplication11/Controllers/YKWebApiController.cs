using Newtonsoft.Json.Linq;
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
        public IDJsonResult LogKaydet_KullaniciGirisi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                string ProgramAdi = Convert.ToString(data["ProgramAdi"]);
                string Sirket = Convert.ToString(data["Sirket"]);
                string KullaniciAdi = Convert.ToString(data["KullaniciAdi"]);
                string Parola = Convert.ToString(data["Parola"]);
                string IP = Convert.ToString(data["IP"]);

                YKUtils.LogKaydet_KullaniciGirisi(ProgramAdi,
                        Sirket,
                        KullaniciAdi,
                        Parola,
                        IP
                        );

                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public IDJsonResult KullaniciGirisi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["KullaniciAdi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Parola"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                string KullaniciAdi = Convert.ToString(data["KullaniciAdi"]);
                string Parola = Convert.ToString(data["Parola"]);

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

                        result.Data = entity;
                        result.SonucKodu = 1;
                        result.Sonuc = "Başarılı";
                        return result;
                    }
                    else
                    {
                        result.SonucKodu = 0;
                        result.Hata = Bilgi;
                        return result;
                    }
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanıcı bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult KullaniciListesi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {

                if (data["KullaniciAdi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Parola"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Parola bilgisi boş olamaz.";
                    return result;
                }
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                string KullaniciAdi = Convert.ToString(data["KullaniciAdi"]);
                string Parola = Convert.ToString(data["Parola"]);
                string UyelikID = Convert.ToString(data["UyelikID"]);

                if (KullaniciAdi == "info@ykyazilim.com.tr" && Parola == "4jtj2jsmv")
                {

                    List<dynamic> entities = new List<dynamic>();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "p_KullaniciListesi";
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@AranacakKelime", "");
                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                    if (dt.Rows.Count > 0)
                    {
                        #region Cookie İşlemleri
                        foreach (DataRow satir in dt.Rows)
                        {
                            dynamic entity = new System.Dynamic.ExpandoObject();
                            entity.ID = Convert.ToString(satir["ID"]);
                            entity.UyelikID = Convert.ToString(satir["UyelikID"]);
                            entity.KullaniciAdi = Convert.ToString(satir["KullaniciAdi"]);
                            entity.Parola = Convert.ToString(satir["Parola"]);
                            entity.Ad = Convert.ToString(satir["Ad"]);
                            entity.Soyad = Convert.ToString(satir["Soyad"]);
                            entity.Telefon = Convert.ToString(satir["Telefon"]);
                            entity.Adres = Convert.ToString(satir["Adres"]);
                            entity.Il = Convert.ToString(satir["Il"]);
                            entity.Ilce = Convert.ToString(satir["Ilce"]);
                            entity.Aktif = Convert.ToString(satir["Aktif"]);
                            entity.Aciklama1 = Convert.ToString(satir["Aciklama1"]);
                            entity.Aciklama2 = Convert.ToString(satir["Aciklama2"]);
                            entity.Aciklama3 = Convert.ToString(satir["Aciklama3"]);
                            entity.Tarih = Convert.ToString(satir["Tarih"]);
                            entity.Resim = Convert.ToString(satir["Resim"]);
                            entity.KayitTarihi = Convert.ToString(satir["KayitTarihi"]);
                            entity.KayitYapanKullanici = Convert.ToString(satir["KayitYapanKullanici"]);
                            entity.DuzenlemeTarihi = Convert.ToString(satir["DuzenlemeTarihi"]);
                            entity.DuzenlemeYapanKullanici = Convert.ToString(satir["DuzenlemeYapanKullanici"]);
                            entity.Silindi = Convert.ToString(satir["Silindi"]);
                            entity.SilinenTarih = Convert.ToString(satir["SilinenTarih"]);
                            entity.SilenKullanici = Convert.ToString(satir["SilenKullanici"]);
                            entity.Onay = Convert.ToString(satir["Onay"]);
                            entities.Add(entity);
                        }
                        #endregion
                        result.Data = entities;
                        result.SonucKodu = 1;
                        result.Sonuc = "Başarılı";
                        return result;
                    }
                    else
                    {
                        result.SonucKodu = 0;
                        result.Hata = "UYARI! Kayıt bulunamadı!";
                        return result;
                    }
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanıcı adı veya parola yanlış!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult UyelikListesi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {

                if (data["KullaniciAdi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Parola"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                string KullaniciAdi = Convert.ToString(data["KullaniciAdi"]);
                string Parola = Convert.ToString(data["Parola"]);

                if (KullaniciAdi == "info@ykyazilim.com.tr" && Parola == "4jtj2jsmv")
                {

                    List<dynamic> entities = new List<dynamic>();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "p_UyelikListesi";
                    cmd.Parameters.AddWithValue("@AranacakKelime", "");
                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                    if (dt.Rows.Count > 0)
                    {
                        #region Cookie İşlemleri
                        foreach (DataRow satir in dt.Rows)
                        {
                            dynamic entity = new System.Dynamic.ExpandoObject();
                            entity.ID = Convert.ToString(satir["ID"]);
                            entity.Isim = Convert.ToString(satir["Isim"]);
                            entity.Unvan = Convert.ToString(satir["Unvan"]);
                            entity.VergiNumarasi = Convert.ToString(satir["VergiNumarasi"]);
                            entity.VergiDairesi = Convert.ToString(satir["VergiDairesi"]);
                            entity.Adres = Convert.ToString(satir["Adres"]);
                            entity.Iletisim = Convert.ToString(satir["Iletisim"]);
                            entity.Email = Convert.ToString(satir["Email"]);
                            entity.UyelikBaslangicTarihi = Convert.ToString(satir["UyelikBaslangicTarihi"]);
                            entity.UyelikBitisTarihi = Convert.ToString(satir["UyelikBitisTarihi"]);
                            entity.KayitTarihi = Convert.ToString(satir["KayitTarihi"]);
                            entity.KayitYapanKullanici = Convert.ToString(satir["KayitYapanKullanici"]);
                            entity.DuzenlemeTarihi = Convert.ToString(satir["DuzenlemeTarihi"]);
                            entity.DuzenlemeYapanKullanici = Convert.ToString(satir["DuzenlemeYapanKullanici"]);
                            entity.Silindi = Convert.ToString(satir["Silindi"]);
                            entity.SilinenTarih = Convert.ToString(satir["SilinenTarih"]);
                            entity.SilenKullanici = Convert.ToString(satir["SilenKullanici"]);
                            entity.ApiUrl = Convert.ToString(satir["ApiUrl"]);
                            entities.Add(entity);
                        }
                        #endregion
                        result.Data = entities;
                        result.SonucKodu = 1;
                        result.Sonuc = "Başarılı";
                        return result;
                    }
                    else
                    {
                        result.SonucKodu = 0;
                        result.Hata = "UYARI! Kayıt bulunamadı!";
                        return result;
                    }
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanıcı adı veya parola yanlış!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
    }
    public class IDJsonResult
    {
        public object Data { get; set; }
        public int SonucKodu { get; set; }
        public string Sonuc { get; set; }
        public string Hata { get; set; }
    }

}