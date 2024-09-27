using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using YKPortal.Models;
using YKPortal.Models.Dto;
using YKPortal.Models.WebApiModels;
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
        public IDJsonResult DestekCevapla([FromBody] JObject data)
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
                if (data["GorevID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! GorevID bilgisi boş olamaz.";
                    return result;
                }
                if (data["Aciklama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Aciklama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Durumu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Durumu bilgisi boş olamaz.";
                    return result;
                }
                if (data["TamamlamaTarihi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! TamamlamaTarihi bilgisi boş olamaz.";
                    return result;
                }
                string UyelikID = Convert.ToString(data["UyelikID"]);
                string KullaniciID = Convert.ToString(data["KullaniciID"]);
                string GorevID = Convert.ToString(data["GorevID"]);
                string Aciklama = Convert.ToString(data["Aciklama"]);
                string Durumu = Convert.ToString(data["Durumu"]);
                DateTime TamamlamaTarihi = Convert.ToDateTime(data["TamamlamaTarihi"]);


                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GorevTamamla";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GorevID", GorevID);
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Durumu", Durumu);
                cmd.Parameters.AddWithValue("@TamamlamaTarihi", TamamlamaTarihi);
                string SonID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));



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


                #region Mail Gönder


                cmd.Parameters.Clear();
                cmd.CommandText = "Select * from MailKaliplari WITH(NOLOCK) Where UyelikID = @UyelikID and Kod = @Kod";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@Kod", "Destek_Gorev_Tamamlama");
                DataTable dtMail = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dtMail.Rows.Count > 0)
                {
                    string Baslik = Convert.ToString(dtMail.Rows[0]["Isim"]);
                    string Icerik = Convert.ToString(dtMail.Rows[0]["Icerik"]);
                    Icerik = Icerik.Replace("{Isim}", ""); //GetCookie("Isim")
                    Icerik = Icerik.Replace("{KayitNo}", GorevID.ToUpper());
                    Icerik = Icerik.Replace("{Durumu}", Durumu);
                    Icerik = Icerik.Replace("{Aciklama}", Aciklama);

                    cmd.Parameters.Clear();
                    cmd.CommandText = "select * from Parametreler WITH(NOLOCK) Where Modul = 'EMail' and UyelikID = @UyelikID";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    DataTable dtMailBilgileri = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                    YKUtils.MailGonder(Baslik, Icerik, "", //GetCookie("KullaniciAdi")
                            Convert.ToString(dtMailBilgileri.Select(" Isim = 'KullaniciAdi' ")[0]["Deger"]),
                            Convert.ToString(dtMailBilgileri.Select(" Isim = 'Parola' ")[0]["Deger"]),
                            Convert.ToString(dtMailBilgileri.Select(" Isim = 'Host' ")[0]["Deger"]),
                            Convert.ToInt32(dtMailBilgileri.Select(" Isim = 'Port' ")[0]["Deger"]),
                            Convert.ToString(dtMailBilgileri.Select(" Isim = 'SSL' ")[0]["Deger"]) == "0" ? false : true
                        );
                }

                #endregion

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
                    entity.Durumu = Convert.ToString(item["Durumu"]);
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
        public IDJsonResult DestekCevaplari([FromBody] JObject data)
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
                if (data["KayitID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KayitID bilgisi boş olamaz.";
                    return result;
                }
                string UyelikID = Convert.ToString(data["UyelikID"]);
                string KullaniciID = Convert.ToString(data["KullaniciID"]);
                string KayitID = Convert.ToString(data["KayitID"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Gorev";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@ID", KayitID);
                DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
                List<GorevDto> entities = new List<GorevDto>();
                foreach (DataRow item in ds.Tables[2].Rows)
                {
                    GorevDto entity = new GorevDto();
                    entity.ID = Convert.ToString(item["ID"]);
                    entity.UyelikID = Convert.ToString(item["UyelikID"]);
                    entity.Durumu = Convert.ToString(item["Durumu"]);
                    entity.Isim = Convert.ToString(item["KaydiKapatan"]);
                    entity.Tarih = Convert.ToDateTime(item["TamamlamaTarihi"]);
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

        #region Finekra Api

        [HttpPost]
        public HttpResponseMessage Cariler(string KullaniciAdi = "", string Parola = "")
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                //Log Kaydı
            }
            catch (Exception err)
            {
                ;
            }
            try
            {
                if (KullaniciAdi == ConfigurationManager.AppSettings["WebApiKullaniciAdi"] && Parola == ConfigurationManager.AppSettings["WebApiParola"])
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Select * From w_Finekra_CariListesi";
                    cmd.CommandTimeout = 0;

                    DataTable dtCariler = new DataTable();
                    dtCariler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                    List<FinekraCari> cariler = new List<FinekraCari>();
                    foreach (DataRow item in dtCariler.Rows)
                    {
                        cariler.Add(new FinekraCari()
                        {
                            ERPKodu = Convert.ToString(item["CARI_KOD"]),
                            CariKodu = Convert.ToString(item["CARI_KOD"]),
                            CariAdi = Convert.ToString(item["CARI_ISIM"]),
                            CariAdi2 = Convert.ToString(item["CARI_ISIM"]),
                            VergiDairesi = Convert.ToString(item["VERGI_DAIRESI"]),
                            VergiNumarasi = Convert.ToString(item["VERGI_NUMARASI"]),
                            TCKimlikNo = Convert.ToString(item["TCKIMLIKNO"]),
                            Telefon = Convert.ToString(item["CARI_TEL"]),
                            CepTelefonu = Convert.ToString(item["CARI_TEL"]),
                            FaxNumarasi = Convert.ToString(item["FAX"]),
                            Adres = Convert.ToString(item["CARI_ADRES"]),
                            CariTipi = Convert.ToString(item["CARI_TIP"]),
                            SaticiKodu = Convert.ToString(item["SATICI_KODU"]),
                            CariSinifi = Convert.ToString(item["CARI_SINIFI"]),
                            GrupKodu = Convert.ToString(item["GRUP_KODU"]),
                            IsletmeKodu = Convert.ToString(item["ISLETME_KODU"]),
                            SubeKodu = Convert.ToString(item["SUBE_KODU"]),
                            PlasiyerKodu = Convert.ToString(item["PLASIYER_KODU"]),
                            AnaCariKodu = Convert.ToString(item["BAGLI_CARI"]),
                            VeritabaniAdi = Convert.ToString(item["SIRKET_ADI"]),
                            Borc = Convert.ToDecimal(item["BORC"]),
                            Alacak = Convert.ToDecimal(item["ALACAK"]),
                            Bakiye = Convert.ToDecimal(item["BAKIYE"]),
                            MuhasebeKodu = Convert.ToString(item["MuhKod"]),
                            ReferansKodu = Convert.ToString(item["REFERANS_KODU"]),
                            ProjeKodu = Convert.ToString(item["PROJE_KODU"])
                        });
                    }
                    if (!object.Equals(cariler, null))
                    {
                        response = Request.CreateResponse<List<FinekraCari>>(HttpStatusCode.OK, cariler);
                    }
                }
                else
                {
                    response = Request.CreateResponse<string>(HttpStatusCode.OK, "Kullanıcı adı veya parola yanlış");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }

        // GET: CariHareket
        [System.Web.Http.HttpPost]
        public HttpResponseMessage FinekraCariHareketIsle(string KullaniciAdi = "", string Parola = "")
        {
            string islem = "";
            HttpResponseMessage response = new HttpResponseMessage();
            List<IslemBilgisi> result = new List<IslemBilgisi>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Baglanti"]);
            try
            {
                //

            }
            catch (Exception err)
            {
                ;
            }

            string resultDekontNo = "";
            string resultInckeyNo = "";

            FinekraCariHareket hareket = new FinekraCariHareket();

            try
            {
                islem = "1";
                if (KullaniciAdi == ConfigurationManager.AppSettings["WebApiKullaniciAdi"] && Parola == ConfigurationManager.AppSettings["WebApiParola"])
                {
                    islem = "2";
                    var content = System.Text.Encoding.UTF8.GetString(Request.Content.ReadAsByteArrayAsync().Result);


                    DataSet ds = (DataSet)JsonConvert.DeserializeObject("{ \"Item\": [" + content + "]}", (typeof(DataSet)));
                    DataTable dt = ds.Tables[0];
                    hareket.Banka = Convert.ToString(dt.Rows[0]["Banka"]);
                    hareket.DekontSeriNo = Convert.ToString(dt.Rows[0]["DekontSeriNo"]);
                    hareket.Tutar = Convert.ToDecimal(dt.Rows[0]["Tutar"].ToString().Replace(ConfigurationManager.AppSettings["Ondalik_Deger1"], ConfigurationManager.AppSettings["Ondalik_Deger2"]));
                    //hareket.Tutar = Convert.ToDecimal(dt.Rows[0]["Tutar"]);
                    hareket.Tarih = Convert.ToDateTime(dt.Rows[0]["Tarih"]);
                    try
                    {
                        hareket.ValorTarihi = Convert.ToDateTime(dt.Rows[0]["ValorTarihi"]);
                    }
                    catch
                    {
                        hareket.ValorTarihi = Convert.ToDateTime(dt.Rows[0]["Tarih"]);
                    }
                    hareket.CariKodu = Convert.ToString(dt.Rows[0]["CariKodu"]);
                    hareket.IslemTipi = Convert.ToString(dt.Rows[0]["IslemTipi"]);
                    hareket.sorumlulukMerkezi = Convert.ToString(dt.Rows[0]["sorumlulukMerkezi"]);
                    hareket.Aciklama = Convert.ToString(dt.Rows[0]["Aciklama"]);
                    hareket.EvrakNo = Convert.ToString(dt.Rows[0]["EvrakNo"]);
                    hareket.Durum = Convert.ToString(dt.Rows[0]["Durum"]);
                    hareket.SirketKodu = Convert.ToString(dt.Rows[0]["SirketKodu"]);
                    hareket.SubeKodu = Convert.ToString(dt.Rows[0]["SubeKodu"]);
                    hareket.ProjeKodu = Convert.ToString(dt.Rows[0]["ProjeKodu"]);
                    hareket.ReferansKodu = Convert.ToString(dt.Rows[0]["ReferansKodu"]);
                    hareket.BorcAlacak = Convert.ToString(dt.Rows[0]["BorcAlacak"]);
                    hareket.DovizTipi = Convert.ToString(dt.Rows[0]["DovizTipi"]);
                    hareket.DovizKuru = Convert.ToDecimal(dt.Rows[0]["DovizKuru"]);
                    hareket.DovizTutari = Convert.ToDecimal(dt.Rows[0]["DovizTutari"].ToString().Replace(ConfigurationManager.AppSettings["Ondalik_Deger1"], ConfigurationManager.AppSettings["Ondalik_Deger2"]));
                    //hareket.DovizTutari = Convert.ToDecimal(dt.Rows[0]["DovizTutari"]);
                    hareket.PlasiyerKodu = Convert.ToString(dt.Rows[0]["PlasiyerKodu"]);
                    if (ConfigurationManager.AppSettings["ProjeKodu"] != "")
                    {
                        hareket.ProjeKodu = ConfigurationManager.AppSettings["ProjeKodu"];
                    }
                    if (ConfigurationManager.AppSettings["ReferansKodu"] != "")
                    {
                        hareket.ReferansKodu = ConfigurationManager.AppSettings["ReferansKodu"];
                    }

                    try
                    {
                        File.WriteAllText(HttpContext.Current.Server.MapPath("~/Loglar/" + DateTime.Now.ToString("yyyyMMddHHmmss - ") + hareket.EvrakNo + ".txt"),

                                "Banka:" + hareket.Banka + System.Environment.NewLine +
                                "DekontSeriNo:" + hareket.DekontSeriNo + System.Environment.NewLine +
                                "Tutar:" + hareket.Tutar + System.Environment.NewLine +
                                "Tarih:" + hareket.Tarih + System.Environment.NewLine +
                                "ValorTarihi:" + hareket.ValorTarihi + System.Environment.NewLine +
                                "CariKodu:" + hareket.CariKodu + System.Environment.NewLine +
                                "IslemTipi:" + hareket.IslemTipi + System.Environment.NewLine +
                                "SorumlulukMerkezi:" + hareket.sorumlulukMerkezi + System.Environment.NewLine +
                                "Aciklama:" + hareket.Aciklama + System.Environment.NewLine +
                                "EvrakNo:" + hareket.EvrakNo + System.Environment.NewLine +
                                "Durum:" + hareket.Durum + System.Environment.NewLine +
                                "SirketKodu:" + hareket.SirketKodu + System.Environment.NewLine +
                                "SubeKodu:" + hareket.SubeKodu + System.Environment.NewLine +
                                "ReferansKodu:" + hareket.ReferansKodu + System.Environment.NewLine +
                                "BorcAlacak:" + hareket.BorcAlacak + System.Environment.NewLine +
                                "DovizTipi:" + hareket.DovizTipi + System.Environment.NewLine +
                                "DovizKuru:" + hareket.DovizKuru + System.Environment.NewLine +
                                "DovizTutari:" + hareket.DovizTutari + System.Environment.NewLine +
                                "ProjeKodu:" + hareket.ProjeKodu + System.Environment.NewLine +
                                "PlasiyerKodu:" + hareket.PlasiyerKodu + System.Environment.NewLine
                            );
                    }
                    catch (Exception hata1)
                    {
                        ;
                    }
                    if (ConfigurationManager.AppSettings["WebApiAyniBelgeNoIleHareketKontrolu"] == "E") //Kontrol 1
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = @" 
select 
* 
from TBLDEKOTRA WITH(NOLOCK) 
Where SERI_NO = '" + ConfigurationManager.AppSettings["DekontSeriNo"] + @"' and TARIH = @Tarih and KOD = @CariKodu 
and ACIKLAMA1 LIKE '%" + hareket.EvrakNo + "%' and TUTAR = @Tutar";
                        cmd.Parameters.AddWithValue("@Tarih", hareket.Tarih.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@CariKodu", hareket.CariKodu);
                        cmd.Parameters.AddWithValue("@Tutar", hareket.Tutar);
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dtKontrol = new DataTable();
                        adapter.Fill(dtKontrol);

                        if (dtKontrol.Rows.Count > 0)
                        {
                            result.Add(new IslemBilgisi() { SonDurumu = 7, EkBilgi1 = "Aynı belge numarası ile kayıt mevcut!" });
                            response = Request.CreateResponse(HttpStatusCode.OK, result);
                            return response;
                        }
                    }

                    if (true)
                    {
                        /*
                        islem = "3";
                        string RestApiKullanUrl = ConfigurationManager.AppSettings["RestApiKullanUrl"];

                        islem = "4 - " + RestApiKullanUrl;
                        oAuth2 _oAuth2;
                        _oAuth2 = new oAuth2(RestApiKullanUrl);
                        _oAuth2.Login(new JLogin()
                        {
                            BranchCode = Convert.ToInt32(hareket.SubeKodu),
                            NetsisUser = ConfigurationManager.AppSettings["NetOpenXKullaniciAdi"],
                            NetsisPassword = ConfigurationManager.AppSettings["NetOpenXParola"],
                            DbType = JNVTTipi.vtMSSQL,
                            DbName = ConfigurationManager.AppSettings["SirketKodu"] == "" ? hareket.SirketKodu : ConfigurationManager.AppSettings["SirketKodu"],
                            DbPassword = "",
                            DbUser = "TEMELSET"
                        });

                        islem = "5";
                        StatementsHeaderManager _manager = new StatementsHeaderManager(_oAuth2);
                        StatementsHeaderParam TmpstatementsHeader = new StatementsHeaderParam();
                        TmpstatementsHeader.Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"];

                        var tmpResult = _manager.NewNumber(TmpstatementsHeader);

                        islem = "6";
                        if (tmpResult.IsSuccessful == true)
                        {
                            resultDekontNo = tmpResult.Data;
                        }
                        else
                        {
                            result.Add(new IslemBilgisi() { Durum = 1, Aciklama = tmpResult.ErrorDesc });
                            response = Request.CreateResponse<List<IslemBilgisi>>(HttpStatusCode.OK, result);
                            return response;
                        }


                        islem = "7";
                        StatementsHeader Dekontbas = new StatementsHeader();
                        Dekontbas.Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"];
                        Dekontbas.Dekont_No = Convert.ToInt32(tmpResult.Data);
                        //kasa.KdvKalems = new List<JKasaKdv>();
                        Dekontbas.Kalemler = new List<Statements>();
                        islem = "8";


                        int dovizTipi = 0;
                        if (hareket.DovizTipi == "TL" || hareket.DovizTipi == "T")
                            dovizTipi = 0;
                        else if (hareket.DovizTipi == "USD")
                            dovizTipi = 1;
                        else if (hareket.DovizTipi == "EUR")
                            dovizTipi = 2;
                        else if (hareket.DovizTipi == "GBP")
                            dovizTipi = 3;

                        if (hareket.IslemTipi == "C")
                        {
                            #region Cari Kısmı

                            islem = "4.Yeni Dekont ";

                            Dekontbas.OtoVadeGunu = false;
                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = hareket.IslemTipi,
                                Kod = hareket.CariKodu,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak,
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });


                            islem = "4.Yeni Dekont Kaydı C  ";


                            #endregion

                            #region Banka Kısmı

                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = "B",

                                Kod = hareket.Banka,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak == "B" ? "A" : "B",
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });

                            islem = "4.Yeni Dekont Kaydı Banka ";


                            #endregion
                        }
                        else if (hareket.IslemTipi == "B")
                        {
                            #region Cari Kısmı

                            islem = "4.Yeni Dekont ";

                            Dekontbas.OtoVadeGunu = false;
                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = hareket.IslemTipi,
                                Kod = hareket.CariKodu,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak,
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });

                            islem = "4.Yeni Dekont Kaydı C  ";

                            #endregion

                            #region Banka Kısmı

                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = hareket.IslemTipi,

                                Kod = hareket.Banka,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak == "B" ? "A" : "B",
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });

                            islem = "4.Yeni Dekont Kaydı Banka ";


                            #endregion
                        }
                        else if (hareket.IslemTipi == "M")
                        {
                            #region Cari Kısmı

                            islem = "4.Yeni Dekont ";

                            Dekontbas.OtoVadeGunu = false;
                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = hareket.IslemTipi,
                                Kod = hareket.CariKodu,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak,
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });
                            islem = "4.Yeni Dekont Kaydı C  ";
                            #endregion
                            #region Banka Kısmı

                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = "B",
                                Kod = hareket.Banka,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak == "B" ? "A" : "B",
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });
                            islem = "4.Yeni Dekont Kaydı Banka ";
                            #endregion
                        }


                        var resultDekontSonucu = _manager.PostInternal(Dekontbas);
                        if (resultDekontSonucu.IsSuccessful == true)
                        {
                            result.Add(new IslemBilgisi() { Durum = 0, BelgeNo = ConfigurationManager.AppSettings["DekontSeriNo"] + "-" + resultDekontNo, Aciklama = "Hareket kaydı tamamlanmıştır." });
                        }
                        else
                        {
                            result.Add(new IslemBilgisi() { Durum = 1, Aciklama = resultDekontSonucu.ErrorDesc });
                            response = Request.CreateResponse<List<IslemBilgisi>>(HttpStatusCode.OK, result);
                            return response;
                        }

                        */

                    }


                    if (!object.Equals(hareket, null))
                    {
                        result.Add(new IslemBilgisi() { SonDurumu = 2, EkBilgi1 = islem });
                        response = Request.CreateResponse<List<IslemBilgisi>>(HttpStatusCode.OK, result);
                    }
                }
                else
                {
                    result.Add(new IslemBilgisi() { SonDurumu = 1, EkBilgi1 = "Kullanıcı adı veya parola yanlış!" });
                    response = Request.CreateResponse<List<IslemBilgisi>>(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.Message == "COM object that has been separated from its underlying RCW cannot be used.")
                    {
                        //+ "-" + resultInckeyNo
                        result.Add(new IslemBilgisi() { SonDurumu = 0, EvrakNumarasi = resultDekontNo, EkBilgi1 = "Hareket kaydı tamamlanmıştır." });
                        response = Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        result.Add(new IslemBilgisi() { SonDurumu = 2, EkBilgi1 = islem + " - " + ex.Message });
                        response = Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }
                catch (Exception err2)
                {
                    result.Add(new IslemBilgisi() { SonDurumu = 2, EkBilgi1 = islem + " - " + err2.Message });
                    response = Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }

            return response;
        }



        [HttpPost]
        public IDJsonResult PersonelCalismaKaydet([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["SicilNo"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! SicilNo bilgisi boş olamaz.";
                    return result;
                }
                string SicilNo = Convert.ToString(data["SicilNo"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"                    
declare @ID nvarchar(100) = (Select NEWID())
Insert Into PersonelCalisma(ID, SicilNo, KayitTarihi, KapanmaTarihi) values(@ID, @SicilNo, GETDATE(), null)
Select @ID as ID
                    ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@SicilNo", SicilNo);
                string ID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                result.Data = ID;
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
        public IDJsonResult PersonelCalismaTamamla([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["KayitID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KayitID bilgisi boş olamaz.";
                    return result;
                }
                string KayitID = Convert.ToString(data["KayitID"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"  Update PersonelCalisma Set KapanmaTarihi=GETDATE() Where ID = @KayitID                    ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@KayitID", KayitID);
                string ID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                result.Data = ID;
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
        public IDJsonResult PersonelSicilKontrol([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["SicilNo"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! SicilNo bilgisi boş olamaz.";
                    return result;
                }
                if (data["Parola"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Parola bilgisi boş olamaz.";
                    return result;
                }
                string SicilNo = Convert.ToString(data["SicilNo"]);
                string Parola = Convert.ToString(data["Parola"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select COUNT(*) from Kullanicilar where SicilNo = @SicilNo and PersonelParola = @Parola";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@SicilNo", SicilNo);
                cmd.Parameters.AddWithValue("@Parola", Parola);
                int kontrol = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                if (kontrol > 0)
                {
                    result.Data = "ok";
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.Data = "error";
                    result.SonucKodu = 0;
                    result.Sonuc = "HATA!";
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

        #endregion

    }
    public class IDJsonResult
    {
        public object Data { get; set; }
        public int SonucKodu { get; set; }
        public string Sonuc { get; set; }
        public string Hata { get; set; }
    }

    public class IslemBilgisi
    {
        public int SonDurumu { get; set; }
        public string EkBilgi1 { get; set; }
        public string EkBilgi2 { get; set; }
        public string EkBilgi3 { get; set; }
        public string EvrakNumarasi { get; set; }
    }

}