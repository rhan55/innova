using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;
using System.Net;

namespace YKPortal.Controllers
{
    public class TanimlamalarController : Controller
    {


        public JsonResult SelectGrupKodu(string grupKodu, string aranacakKelime = "")
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cmd.Parameters.AddWithValue("@Kod", grupKodu);
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<StokDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new StokDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Isim = Convert.ToString(dt.Rows[i]["Isim"]),
                    Aciklama = Convert.ToString(dt.Rows[i]["Aciklama"]),
                    Kod = Convert.ToString(dt.Rows[i]["Kod"]),
                });
            }

            return Json(liste, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult GrupKodu(string grupKodu, string aranacakKelime="")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu($"/Tanimlamalar/GrupKodu?grupKodu={grupKodu}", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            cmd.Parameters.AddWithValue("@Kod", grupKodu);
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.GrupKodu = grupKodu;
            ViewBag.aranacakKelime = aranacakKelime;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var model = new GrupkoduListeViewModel
            {
                GrupKodlari = dt,
                Sil = YetkiKontrolu($"/Tanimlamalar/GrupKodu?grupKodu={grupKodu}", "Sil"),
                Duzenle = YetkiKontrolu($"/Tanimlamalar/GrupKodu?grupKodu={grupKodu}", "Duzenle")

            };

            return View(model);

        }
        [HttpGet]
        public ActionResult Ekle(string grupKodu = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (string.IsNullOrWhiteSpace(grupKodu))
            {
                if (!YetkiKontrolu("/Tanimlamalar/Ekle", "Gor"))
                {
                    return Redirect("~/YK/Anasayfa");
                }
            } else
            {
                if (!YetkiKontrolu("/Tanimlamalar/GrupKodu?grupKodu=" + grupKodu, "Duzenle"))
                {
                    return Redirect("~/YK/Anasayfa");
                }
            }

            

            ViewBag.GrupKodu = grupKodu;

            return View();
        }

        [HttpPost]
        public ActionResult Ekle(GrupKoduDto grupKoduDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (!YetkiKontrolu("/Tanimlamalar/Ekle", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu", new { grupKodu = grupKoduDto.Kod });
        }
        [HttpGet]
        public ActionResult Duzenle(string grupKodu, string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
         

            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKodu";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.GrupKodu = grupKodu;
            return View(dt);
        }

        [HttpPost]
        public ActionResult Duzenle(GrupKoduDto grupKoduDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

           

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", grupKoduDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", grupKoduDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return RedirectToAction("GrupKodu", new { grupKodu = grupKoduDto.Kod });
        }

        [HttpPost]
        public ActionResult Sil(string id, string grupKodu)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (!YetkiKontrolu($"/Tanimlamalar/GrupKodu?grupKodu={grupKodu}", "Sil"))
            {
                return Redirect("~/YK/Anasayfa");
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
           
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu", new { grupKodu = grupKodu });
        }

        [HttpGet]
        public ActionResult DovizBirimiEkle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

     
            return View();
        }

        [HttpPost]
        public ActionResult DovizBirimiEkle(DovizBirimiDto dovizBirimiDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DovizKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", dovizBirimiDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", dovizBirimiDto.Isim);
           

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("DovizBirimiListe");
        }
        [HttpGet]
        public ActionResult DovizBirimiListe(string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DovizListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.AranacakKelime = aranacakKelime;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpGet]
        public ActionResult DovizBirimiDuzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

       
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Doviz";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }
        [HttpPost]
        public ActionResult DovizBirimiDuzenle(DovizBirimiDto dovizBirimiDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

         

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DovizKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@ID", dovizBirimiDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", dovizBirimiDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", dovizBirimiDto.Isim);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("DovizBirimiListe");
        }
        [HttpPost]
        public ActionResult DovizBirimiSil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DovizSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
           

            return RedirectToAction("DovizBirimiListe");
        }

        [HttpGet]
        public ActionResult KasaEkle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.Personeller = PersonelGetir();
            ViewBag.Dovizler = DovizGetir();

            return View();
        }

        [HttpPost]
        public ActionResult KasaEkle(KasaTanimlamaDto kasaTanimlamaDto)
         {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KasaKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", kasaTanimlamaDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", kasaTanimlamaDto.Isim);
            cmd.Parameters.AddWithValue("@PersonelID", kasaTanimlamaDto.PersonelID);
            cmd.Parameters.AddWithValue("@DovizID", kasaTanimlamaDto.DovizID);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("KasaListe");
        
        }
        [HttpGet]
        public ActionResult KasaListe(string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KasaListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.AranacakKelime = aranacakKelime;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            List<KasaTanimlamaDto> entities = new List<KasaTanimlamaDto>();

            var personeller = PersonelGetir();
            var dovizler = DovizGetir();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                KasaTanimlamaDto entity = new KasaTanimlamaDto();
                entity.ID = Convert.ToString(dt.Rows[i]["ID"]);
                entity.Isim = Convert.ToString(dt.Rows[i]["Isim"]);
                entity.Kod = Convert.ToString(dt.Rows[i]["Kod"]);
                entity.PersonelID = Convert.ToString(dt.Rows[i]["PersonelID"]);
                entity.DovizID = Convert.ToString(dt.Rows[i]["DovizID"]);
                entity.Personel = personeller.FirstOrDefault(m =>  m.ID == entity.PersonelID)?.Isim ?? "Personel Bulunamadı";
                entity.Doviz = dovizler.FirstOrDefault(m => m.ID == entity.DovizID)?.Isim ?? "Döviz Bulunamadı";
                entities.Add(entity);
            }

            var model = new BankaTanimlamalariDtoListeViewModel
            {
                Kasalar = entities,
                Duzenle = true,
                Sil = true
            };

         
            return View(model);
        }

        [HttpGet]
        public ActionResult KasaDuzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.Duzenle = true;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kasa";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }
        [HttpPost]
        public ActionResult KasaDuzenle(KasaTanimlamaDto kasaTanimlamaDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KasaKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", kasaTanimlamaDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", kasaTanimlamaDto.Isim);
            cmd.Parameters.AddWithValue("@PersonelID", kasaTanimlamaDto.PersonelID);
            cmd.Parameters.AddWithValue("@DovizID", kasaTanimlamaDto.DovizID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            ViewBag.Duzenle = YetkiKontrolu("/Tanimlamalar/KasaListe", "Duzenle");
            return RedirectToAction("KasaListe");
        }
        [HttpPost]
        public ActionResult KasaSil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KasaSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            return RedirectToAction("KasaListe");
        }

        [HttpGet]
        public ActionResult BankaHesabiEkle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.Bankalar = BankaGetir();

            return View();
        }

        [HttpPost]
        public ActionResult BankaHesabiEkle(BankaTanimlamalariDto bankaTanimlamalariDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Tanimlamalar/BankaHesabiEkle", "Duzenle"))
                return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BankaHesaplariKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@BankaID", bankaTanimlamalariDto.BankaID);
            cmd.Parameters.AddWithValue("@Kod", bankaTanimlamalariDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", bankaTanimlamalariDto.Isim);
            cmd.Parameters.AddWithValue("@HesapNo", bankaTanimlamalariDto.HesapNo);
            cmd.Parameters.AddWithValue("@Iban", bankaTanimlamalariDto.Iban);
  
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return RedirectToAction("BankaHesabiListe");

        }
        [HttpGet]
        public ActionResult BankaHesabiListe(string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            //if (!YetkiKontrolu("/Tanimlamalar/BankaHesabiListe", "Gor"))
            //    return Redirect("~/YK/Anasayfa");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BankaHesaplariListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.AranacakKelime = aranacakKelime;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

           
           
            var model = new BankaTanimlamalariListeViewModel
            {
                Bankalar = dt,
                Duzenle = true,
                Sil = true
            };
            return View(model);
            
        }

        [HttpGet]
        public ActionResult BankaHesabiDuzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            //if (!YetkiKontrolu("/Tanimlamalar/Liste", "Duzenle"))
            //    return Redirect("~/YK/Anasayfa");

            ViewBag.Duzenle = true;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BankaHesaplari";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }
        [HttpPost]
        public ActionResult BankaHesabiDuzenle(BankaTanimlamalariDto bankaTanimlamalariDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");



            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BankaHesaplariKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@BankaID", bankaTanimlamalariDto.BankaID);
            cmd.Parameters.AddWithValue("@Kod", bankaTanimlamalariDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", bankaTanimlamalariDto.Isim);
            cmd.Parameters.AddWithValue("@HesapNo", bankaTanimlamalariDto.HesapNo);
            cmd.Parameters.AddWithValue("@Iban", bankaTanimlamalariDto.Iban);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            ViewBag.Duzenle = YetkiKontrolu("/Tanimlamalar/KasaListe", "Duzenle");
            return RedirectToAction("KasaListe");
        }
        [HttpPost]
        public ActionResult BankaHesabiSil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BankaHesaplariSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            return RedirectToAction("KasaListe");
        }


        public List<PersonelDto> PersonelGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_PersonellerListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime ", string.Empty);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var entities = new List<PersonelDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PersonelDto entity = new PersonelDto();
                entity.ID = Convert.ToString(dt.Rows[i]["ID"]);
                entity.Isim = Convert.ToString(dt.Rows[i]["Isim"]);
                entity.Email = Convert.ToString(dt.Rows[i]["Email"]);
                entity.Telefon = Convert.ToString(dt.Rows[i]["Telefon"]);

                entities.Add(entity);
            }
            return entities;
        }
        public List<DovizBirimiDto> DovizGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DovizListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime ", string.Empty);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var entities = new List<DovizBirimiDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DovizBirimiDto entity = new DovizBirimiDto();
                entity.ID = Convert.ToString(dt.Rows[i]["ID"]);
                entity.Isim = Convert.ToString(dt.Rows[i]["Isim"]);

                entities.Add(entity);
            }
            return entities;
        }

        public List<DovizBirimiDto> BankaGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BankaListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime ", string.Empty);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var entities = new List<DovizBirimiDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DovizBirimiDto entity = new DovizBirimiDto();
                entity.ID = Convert.ToString(dt.Rows[i]["ID"]);
                entity.Isim = Convert.ToString(dt.Rows[i]["Isim"]);

                entities.Add(entity);
            }
            return entities;
        }

        #region Cookie İşlemleri
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