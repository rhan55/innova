using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using System.IO;
using System.Net;
using System.Text;
using YKPortal.Models.Dto;

namespace YKPortal.Areas.E.Controllers
{
    public class KullaniciController : Controller
    {


        [HttpGet]
        public ActionResult Profil()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/E/Site/Giris");
            
            UlkeListesiniOlustur();
            IlListesiniOlustur();

            var uyelikId = GetCookie("UyelikID");
            var kullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Cari";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", kullaniciID);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult Profil(CariDto cariDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/E/Site/Giris");

            IlListesiniOlustur();
            UlkeListesiniOlustur();
          
            cariDto.UyelikID = GetCookie("UyelikID");

            cariDto.KullaniciID = GetCookie("KullaniciID");

            if (cariDto.TCKimlikNoVergiNo.Length == 10)
            {
                cariDto.VergiNumarasi = cariDto.TCKimlikNoVergiNo;
            } else if (cariDto.TCKimlikNoVergiNo.Length == 11)
            {
                cariDto.TCKimlikNo = cariDto.TCKimlikNoVergiNo;
            }

            var eskiCariDto = Getir(cariDto.KullaniciID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_CariKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UyelikID", eskiCariDto.UyelikID);
            cmd.Parameters.AddWithValue("@ID", eskiCariDto.ID);
            cmd.Parameters.AddWithValue("@KullaniciID", cariDto.KullaniciID);
            cmd.Parameters.AddWithValue("@Aktif", eskiCariDto.Aktif);
            cmd.Parameters.AddWithValue("@KayitTarihi", eskiCariDto.KayitTarihi);
            cmd.Parameters.AddWithValue("@Kod", eskiCariDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", cariDto.Isim);
            cmd.Parameters.AddWithValue("@Unvan", cariDto.Unvan);
            cmd.Parameters.AddWithValue("@Adres", cariDto.Adres);
            cmd.Parameters.AddWithValue("@Ilce", eskiCariDto.Ilce);
            cmd.Parameters.AddWithValue("@Il", cariDto.Il);
            cmd.Parameters.AddWithValue("@Ulke", cariDto.Ulke);
            cmd.Parameters.AddWithValue("@Bolge ", eskiCariDto.Bolge);
            cmd.Parameters.AddWithValue("@TCKimlikNo", cariDto.TCKimlikNo);
            cmd.Parameters.AddWithValue("@VergiNumarasi", cariDto.VergiNumarasi);
            cmd.Parameters.AddWithValue("@VergiDairesi", cariDto.VergiDairesi);
            cmd.Parameters.AddWithValue("@PostaKodu", cariDto.PostaKodu);
            cmd.Parameters.AddWithValue("@Alici", eskiCariDto.Alici);
            cmd.Parameters.AddWithValue("@Satici", eskiCariDto.Satici);
            cmd.Parameters.AddWithValue("@Personel", eskiCariDto.Personel);
            cmd.Parameters.AddWithValue("@Telefon1", eskiCariDto.Telefon1);
            cmd.Parameters.AddWithValue("@Telefon2", eskiCariDto.Telefon2);
            cmd.Parameters.AddWithValue("@EMail", eskiCariDto.EMail);
            cmd.Parameters.AddWithValue("@Faks", eskiCariDto.Faks);
            cmd.Parameters.AddWithValue("@CepTelefonu", cariDto.CepTelefonu);
            cmd.Parameters.AddWithValue("@WebSite", eskiCariDto.WebSite);
            cmd.Parameters.AddWithValue("@GrupKodu1ID", eskiCariDto.GrupKodu1ID == null ? string.Empty : eskiCariDto.GrupKodu1ID);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", eskiCariDto.GrupKodu2ID == null ? string.Empty : eskiCariDto.GrupKodu2ID);
            cmd.Parameters.AddWithValue("@GrupKodu3ID", eskiCariDto.GrupKodu3ID == null ? string.Empty : eskiCariDto.GrupKodu3ID);
            cmd.Parameters.AddWithValue("@GrupKodu4ID", eskiCariDto.GrupKodu4ID == null ? string.Empty : eskiCariDto.GrupKodu4ID);
            cmd.Parameters.AddWithValue("@GrupKodu5ID", eskiCariDto.GrupKodu5ID == null ? string.Empty : eskiCariDto.GrupKodu5ID);
            cmd.Parameters.AddWithValue("@GrupKodu6ID", eskiCariDto.GrupKodu6ID == null ? string.Empty : eskiCariDto.GrupKodu6ID);
            cmd.Parameters.AddWithValue("@MuhasebeKodu", eskiCariDto.MuhasebeKodu);
            cmd.Parameters.AddWithValue("@Kilitli", eskiCariDto.Kilitli);
            cmd.Parameters.AddWithValue("@KilitAciklamasi", eskiCariDto.KilitAciklamasi);
            cmd.Parameters.AddWithValue("@DovizID", eskiCariDto.DovizID);
            cmd.Parameters.AddWithValue("@VadeGunu", eskiCariDto.VadeGunu);
            cmd.Parameters.AddWithValue("@Iskonto1", eskiCariDto.Iskonto1);
            cmd.Parameters.AddWithValue("@ListeFiyat", eskiCariDto.ListeFiyat);
            cmd.Parameters.AddWithValue("@Aciklama1", eskiCariDto.Aciklama1 == null ? string.Empty : eskiCariDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", eskiCariDto.Aciklama2 == null ? string.Empty : eskiCariDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", eskiCariDto.Aciklama3 == null ? string.Empty : eskiCariDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Aciklama4", eskiCariDto.Aciklama4 == null ? string.Empty : eskiCariDto.Aciklama4);
            cmd.Parameters.AddWithValue("@Aciklama5", eskiCariDto.Aciklama5 == null ? string.Empty : eskiCariDto.Aciklama5);
            cmd.Parameters.AddWithValue("@Aciklama6", eskiCariDto.Aciklama6 == null ? string.Empty : eskiCariDto.Aciklama6);
            cmd.Parameters.AddWithValue("@LimitAsimindaUyar", eskiCariDto.LimitAsimindaUyar);
            cmd.Parameters.AddWithValue("@LimitAsimindaDurdur", eskiCariDto.LimitAsimindaDurdur);
            cmd.Parameters.AddWithValue("@CekSenetRiski", eskiCariDto.CekSenetRiski);
            cmd.Parameters.AddWithValue("@Limit", eskiCariDto.Limit);
            cmd.Parameters.AddWithValue("@ServisPersoneli", eskiCariDto.ServisPersoneli);
            cmd.Parameters.AddWithValue("@KullaniciAdi ", cariDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", cariDto.Parola);
            cmd.Parameters.AddWithValue("@RiskAciklama", eskiCariDto.RiskAciklama);
            cmd.Parameters.AddWithValue("@PlasiyerID", eskiCariDto.PlasiyerID);
            cmd.Parameters.AddWithValue("@AnaCariID", eskiCariDto.AnaCariID);
            cmd.Parameters.AddWithValue("@TeslimCariID", eskiCariDto.TeslimCariID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Profil");
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


        private CariDto Getir(string id)
        {
            if (id != null && id.Length > 0)

            {
                var uyelikId = GetCookie("UyelikID");
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Cari";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);



                return new CariDto
                {
                    ID = Convert.ToString(dt.Rows[0]["ID"]),
                    UyelikID = Convert.ToString(dt.Rows[0]["UyelikID"]),
                    Isim = Convert.ToString(dt.Rows[0]["Isim"]),
                    VergiNumarasi = Convert.ToString(dt.Rows[0]["VergiNumarasi"]),
                    TCKimlikNo = Convert.ToString(dt.Rows[0]["TCKimlikNo"]),
                    Kod = Convert.ToString(dt.Rows[0]["Kod"]),
                    Unvan = Convert.ToString(dt.Rows[0]["Unvan"]),
                    Adres = Convert.ToString(dt.Rows[0]["Adres"]),
                    Ilce = Convert.ToString(dt.Rows[0]["Ilce"]),
                    Il = Convert.ToString(dt.Rows[0]["Il"]),
                    Ulke = Convert.ToString(dt.Rows[0]["Ulke"]),
                    Bolge = Convert.ToString(dt.Rows[0]["Bolge"]),
                    VergiDairesi = Convert.ToString(dt.Rows[0]["VergiDairesi"]),
                    PostaKodu = Convert.ToString(dt.Rows[0]["PostaKodu"]),
                    Alici = Convert.ToBoolean(dt.Rows[0]["Alici"]),
                    Satici = Convert.ToBoolean(dt.Rows[0]["Satici"]),
                    Personel = Convert.ToBoolean(dt.Rows[0]["Personel"]),
                    Telefon1 = Convert.ToString(dt.Rows[0]["Telefon1"]),
                    Telefon2 = Convert.ToString(dt.Rows[0]["Telefon2"]),
                    EMail = Convert.ToString(dt.Rows[0]["EMail"]),
                    Faks = Convert.ToString(dt.Rows[0]["Faks"]),
                    CepTelefonu = Convert.ToString(dt.Rows[0]["CepTelefonu"]),
                    WebSite = Convert.ToString(dt.Rows[0]["WebSite"]),
                    GrupKodu1ID = Convert.ToString(dt.Rows[0]["GrupKodu1ID"]),
                    GrupKodu2ID = Convert.ToString(dt.Rows[0]["GrupKodu2ID"]),
                    GrupKodu3ID = Convert.ToString(dt.Rows[0]["GrupKodu3ID"]),
                    GrupKodu4ID = Convert.ToString(dt.Rows[0]["GrupKodu4ID"]),
                    GrupKodu5ID = Convert.ToString(dt.Rows[0]["GrupKodu5ID"]),
                    GrupKodu6ID = Convert.ToString(dt.Rows[0]["GrupKodu6ID"]),
                    MuhasebeKodu = Convert.ToString(dt.Rows[0]["MuhasebeKodu"]),
                    Kilitli = Convert.ToBoolean(dt.Rows[0]["Kilitli"]),
                    KilitAciklamasi = Convert.ToString(dt.Rows[0]["KilitAciklamasi"]),
                    DovizID = Convert.ToString(dt.Rows[0]["DovizID"]),
                    VadeGunu = Convert.ToInt32(dt.Rows[0]["VadeGunu"]),
                    Iskonto1 = Convert.ToDecimal(dt.Rows[0]["Iskonto1"]),
                    ListeFiyat = Convert.ToDecimal(dt.Rows[0]["ListeFiyat"]),
                    Aciklama1 = Convert.ToString(dt.Rows[0]["Aciklama1"]),
                    Aciklama2 = Convert.ToString(dt.Rows[0]["Aciklama2"]),
                    Aciklama3 = Convert.ToString(dt.Rows[0]["Aciklama3"]),
                    Aciklama4 = Convert.ToString(dt.Rows[0]["Aciklama4"]),
                    Aciklama5 = Convert.ToString(dt.Rows[0]["Aciklama5"]),
                    Aciklama6 = Convert.ToString(dt.Rows[0]["Aciklama6"]),
                    LimitAsimindaUyar = Convert.ToBoolean(dt.Rows[0]["LimitAsimindaUyar"]),
                    LimitAsimindaDurdur = Convert.ToBoolean(dt.Rows[0]["LimitAsimindaDurdur"]),
                    CekSenetRiski = Convert.ToBoolean(dt.Rows[0]["CekSenetRiski"]),
                    Limit = Convert.ToDecimal(dt.Rows[0]["Limit"]),
                    ServisPersoneli = Convert.ToBoolean(dt.Rows[0]["ServisPersoneli"]),
                    KullaniciAdi = Convert.ToString(dt.Rows[0]["KullaniciAdi"]),
                    Parola = Convert.ToString(dt.Rows[0]["Parola"]),
                    RiskAciklama = Convert.ToString(dt.Rows[0]["RiskAciklama"]),
                    PlasiyerID = Convert.ToString(dt.Rows[0]["PlasiyerID"]),
                    AnaCariID = Convert.ToString(dt.Rows[0]["AnaCariID"]),
                    TeslimCariID = Convert.ToString(dt.Rows[0]["TeslimCariID"]),
                    Aktif = Convert.ToBoolean(dt.Rows[0]["Aktif"])
                };

            }
            return new CariDto { };
        }


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