using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YKPortal.Models;
using YKPortal.Models.Dto;
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

        [HttpPost]
        public IDJsonResult DestekTipleri([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                string UyelikID = Convert.ToString(data["UyelikID"]);

                //GrupKodu1 Listesi oluşturma
                SqlCommand gorevtipiCommand = new SqlCommand();
                gorevtipiCommand.CommandText = "p_GrupKoduListesi";
                gorevtipiCommand.CommandType = System.Data.CommandType.StoredProcedure;
                gorevtipiCommand.Parameters.AddWithValue("@UyelikID", UyelikID);
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
                result.Data = entities;
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
        public IDJsonResult YeniDestek([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                if (data["KullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciID bilgisi boş olamaz.";
                    return result;
                }
                if (data["SecilenKullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! SecilenKullaniciID bilgisi boş olamaz.";
                    return result;
                }
                if (data["GorevTipiID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! GorevTipiID bilgisi boş olamaz.";
                    return result;
                }
                if (data["Aciklama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Aciklama bilgisi boş olamaz.";
                    return result;
                }

                string UyelikID = Convert.ToString(data["UyelikID"]);
                string KullaniciID = Convert.ToString(data["KullaniciID"]);
                string SecilenKullaniciID = Convert.ToString(data["SecilenKullaniciID"]);
                string GorevTipiID = Convert.ToString(data["GorevTipiID"]);
                string Aciklama = Convert.ToString(data["Aciklama"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GorevKaydet";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", "");
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@GorevTipiID", GorevTipiID);
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", DateTime.Now);
                cmd.Parameters.AddWithValue("@Periyot", "T");
                cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                string SonID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));


                cmd.Parameters.Clear();
                cmd.CommandText = "p_GorevKullanicilariniSil";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@GorevID", SonID); ;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);



                cmd.Parameters.Clear();
                cmd.CommandText = "p_GorevKullaniciKaydet";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@GorevID", SonID);
                cmd.Parameters.AddWithValue("@SecilenKullaniciID", SecilenKullaniciID);
                cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                #region Kullanıcıya sms gönderme
                try
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_Kullanici";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@ID", SecilenKullaniciID);
                    string telefon = Convert.ToString(((DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo)).Rows[0]["Telefon"])
                        .Replace(" ", "")
                        .Replace("-", "")
                        .Replace("_", "")
                        .Replace(")", "")
                        .Replace("(", "");

                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_Kullanici";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@ID", KullaniciID);
                    DataTable dtKullanici = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                    string Isim = Convert.ToString(dtKullanici.Rows[0]["Ad"]) + " " + Convert.ToString(dtKullanici.Rows[0]["Soyad"]);
                    if (telefon.Trim().Length > 0)
                    {
                        string aciklama = Aciklama;
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
                            "mesaj=" + Isim + " kullanıcısı " + DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " tarihli görev atadı, görev ayrıntı : " + aciklama + "";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        var sonuc = request.GetResponse();
                    }
                }
                catch (Exception err)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_HataKaydet";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@Kullanici", KullaniciID);
                    cmd.Parameters.AddWithValue("@Modul", "Gorev");
                    cmd.Parameters.AddWithValue("@Aciklama1", "~/Gorev/GorevEkle");
                    cmd.Parameters.AddWithValue("@Aciklama2", err.Message);
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }
                #endregion


                List<DosyaDto> UrunResimleri = data["Resimler"].ToObject<List<DosyaDto>>();

                foreach (DosyaDto resim in UrunResimleri)
                {
                    File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + resim.Dosya), resim.ImageByte);
                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_DosyaKaydet";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", "");
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@Modul", "Gorev");
                    cmd.Parameters.AddWithValue("@KayitID", SonID);
                    cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + resim.Dosya);
                    cmd.Parameters.AddWithValue("@Isim", resim.Dosya);
                    cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }


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
        public IDJsonResult DestekKayitlari([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                if (data["KullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciID bilgisi boş olamaz.";
                    return result;
                }
                if (data["Baslangic"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Baslangic bilgisi boş olamaz.";
                    return result;
                }
                if (data["Bitis"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Bitis bilgisi boş olamaz.";
                    return result;
                }
                if (data["Durum"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Durum bilgisi boş olamaz.";
                    return result;
                }
                if (data["GorevTipiID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! GorevTipiID bilgisi boş olamaz.";
                    return result;
                }
                string UyelikID = Convert.ToString(data["UyelikID"]);
                string KullaniciID = Convert.ToString(data["KullaniciID"]);
                DateTime? Baslangic = Convert.ToDateTime(data["Baslangic"]);
                DateTime? Bitis = Convert.ToDateTime(data["Bitis"]);
                string Durum = Convert.ToString(data["Durum"]);
                string GorevTipiID = Convert.ToString(data["GorevTipiID"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GorevListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@GorevTipiID", GorevTipiID);
                cmd.Parameters.AddWithValue("@KayitYapanKullanici", KullaniciID);
                cmd.Parameters.AddWithValue("@Durum", Durum);
                DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
                List<GorevDto> entities = new List<GorevDto>();
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    GorevDto entity = new GorevDto();
                    entity.ID = Convert.ToString(item["ID"]);
                    entity.GorevTipiID = Convert.ToString(item["GorevTipiID"]);
                    entity.KullaniciID = Convert.ToString(item["KayitYapanKullanici"]);
                    entity.BaslangicTarihi = Convert.ToDateTime(item["BaslangicTarihi"]);
                    entity.Aciklama = Convert.ToString(item["Aciklama"]);
                    entities.Add(entity);
                }

                result.Data = entities;
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



    }
    public class IDJsonResult
    {
        public object Data { get; set; }
        public int SonucKodu { get; set; }
        public string Sonuc { get; set; }
        public string Hata { get; set; }
    }

}