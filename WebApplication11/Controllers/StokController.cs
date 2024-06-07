using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;

namespace YKPortal.Controllers
{
    public class StokController : Controller
    {
        // GET: Stok
        [HttpGet]
        public ActionResult Ekle()
        {
            StokGrupKod1ListesiniOlustur();
            StokGrupKod2ListesiniOlustur();
            StokGrupKod3ListesiniOlustur();
            StokGrupKod4ListesiniOlustur();
            StokGrupKod5ListesiniOlustur();
            StokGrupKod6ListesiniOlustur();
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(StokDto stokDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var cmd = new SqlCommand();
            cmd.CommandText = "p_StokKaydet";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", null);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Isim", stokDto.Isim);
            cmd.Parameters.AddWithValue("@Kod", stokDto.Kod);
            cmd.Parameters.AddWithValue("@Oiv", stokDto.Oiv);
            cmd.Parameters.AddWithValue("@Durumu", stokDto.Durumu);
            cmd.Parameters.AddWithValue("@Aciklama", stokDto.Aciklama);
            cmd.Parameters.AddWithValue("@Barkod", stokDto.Barkod);
            cmd.Parameters.AddWithValue("@OlcuBirimi", stokDto.OlcuBirimi);
            cmd.Parameters.AddWithValue("@GrupKodu1ID", string.IsNullOrEmpty(stokDto.GrupKodu1ID) ? null : stokDto.GrupKodu1ID);
            cmd.Parameters.AddWithValue("@GrupKodu2ID", string.IsNullOrEmpty(stokDto.GrupKodu2ID) ? null : stokDto.GrupKodu2ID);
            cmd.Parameters.AddWithValue("@GrupKodu3ID", string.IsNullOrEmpty(stokDto.GrupKodu3ID) ? null : stokDto.GrupKodu3ID);
            cmd.Parameters.AddWithValue("@GrupKodu4ID", string.IsNullOrEmpty(stokDto.GrupKodu4ID) ? null : stokDto.GrupKodu4ID);
            cmd.Parameters.AddWithValue("@GrupKodu5ID", string.IsNullOrEmpty(stokDto.GrupKodu5ID) ? null : stokDto.GrupKodu5ID);
            cmd.Parameters.AddWithValue("@GrupKodu6ID", string.IsNullOrEmpty(stokDto.GrupKodu6ID) ? null : stokDto.GrupKodu6ID);
            cmd.Parameters.AddWithValue("@KdvAlis", stokDto.KdvAlis);
            cmd.Parameters.AddWithValue("@KdvSatis", stokDto.KdvSatis);
            cmd.Parameters.AddWithValue("@Otv", stokDto.Otv);
            cmd.Parameters.AddWithValue("@OtvFiyat", stokDto.OtvFiyat);
            cmd.Parameters.AddWithValue("@TevkifatPay", stokDto.TevkifatPay);
            cmd.Parameters.AddWithValue("@TevkifatPayda", stokDto.TevkifatPayda);
            cmd.Parameters.AddWithValue("@VadeGunu", stokDto.VadeGunu);
            cmd.Parameters.AddWithValue("@MinimumStok", stokDto.MinimumStok);
            cmd.Parameters.AddWithValue("@MaxsimumStok", stokDto.MaxsimumStok);
            cmd.Parameters.AddWithValue("@LimitUyarisi", stokDto.LimitUyarisi);
            cmd.Parameters.AddWithValue("@LimitDisindaIslemiDurdur", stokDto.LimitDisindaIslemiDurdur);
            cmd.Parameters.AddWithValue("@EksiBakiyeUyarisi", stokDto.EksiBakiyeUyarisi);
            cmd.Parameters.AddWithValue("@EksiBakiyedeIslemiDurdur", stokDto.EksiBakiyedeIslemiDurdur);
            cmd.Parameters.AddWithValue("@StokKilitle", stokDto.StokKilitle);
            cmd.Parameters.AddWithValue("@IskontoSatis1", stokDto.IskontoSatis1);
            cmd.Parameters.AddWithValue("@MarkaID", null);
            cmd.Parameters.AddWithValue("@ModelID", null);
            cmd.Parameters.AddWithValue("@RenkID", null);
            cmd.Parameters.AddWithValue("@BedenID", null);
            cmd.Parameters.AddWithValue("@KaliteID", null);
            cmd.Parameters.AddWithValue("@KayitYapanKullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@AnaStokID", null);
            
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }

        [HttpGet]
        public ActionResult Liste(StokDto stokDto)
        {

            var cmd = new SqlCommand();
            cmd.CommandText = "p_StokListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", stokDto.Kod);
            cmd.Parameters.AddWithValue("@Isim", stokDto.Isim);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var stokListesi = new List<StokDto>();

            foreach(DataRow dr in dt.Rows)
            {
                stokListesi.Add(new StokDto { 
                    ID = Convert.ToString(dr["ID"]),
                    Isim = Convert.ToString(dr["Isim"]),
                    Kod = Convert.ToString(dr["Kod"]),
                    Aciklama = Convert.ToString(dr["Aciklama"])
                });
            }

            ViewBag.StokListesi = stokListesi;
            StokGrupKod1ListesiniOlustur();
            StokGrupKod2ListesiniOlustur();

            return View(dt);
        }
        public JsonResult SelectListe(string search)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_StokListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kod", "");
            cmd.Parameters.AddWithValue("@Isim", search);
            cmd.Parameters.AddWithValue("@Unvan", "");
            cmd.Parameters.AddWithValue("@TCKimlikNo", "");
            cmd.Parameters.AddWithValue("@VergiNumarasi", "");
            cmd.Parameters.AddWithValue("@CepTelefonu", "");

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<StokDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new StokDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Isim = Convert.ToString(dt.Rows[i]["Isim"]),
                    Kod = Convert.ToString(dt.Rows[i]["Kod"]),
                });
            }

            return Json(liste, JsonRequestBehavior.AllowGet);

        }
        // Bir tane cari getirmek icin kullandigimiz metod, bu metod sayesinde id uzerinden bir carinin Isim ve ID'sini getirebiliyoruz. Select2 icin kullaniyoruz.
        private StokDto Getir(string id)
        {
            if (id != null && id.Length > 0)

            {
                var uyelikId = GetCookie("UyelikID");
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Stok";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


                return new StokDto
                {
                    ID = Convert.ToString(dt.Rows[0]["ID"]),
                    Isim = Convert.ToString(dt.Rows[0]["Isim"])
                };
            }
            return new StokDto { };
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
    
        public void StokGrupKod1ListesiniOlustur()
        {
            //GrupKodu1 Listesi oluşturma
            SqlCommand stokGrupKod1Command = new SqlCommand();
            stokGrupKod1Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod1Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod1Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod1Command.Parameters.AddWithValue("@Kod", "StokGrupKod1");
            stokGrupKod1Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod1Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod1DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari1 = entities;
        }
        public void StokGrupKod2ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod2Command = new SqlCommand();
            stokGrupKod2Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod2Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod2Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod2Command.Parameters.AddWithValue("@Kod", "StokGrupKod2");
            stokGrupKod2Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod2DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod2Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod2DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod2DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod2DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari2 = entities;
        }

        public void StokGrupKod3ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod3Command = new SqlCommand();
            stokGrupKod3Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod3Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod3Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod3Command.Parameters.AddWithValue("@Kod", "StokGrupKod3");
            stokGrupKod3Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod3DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod3Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod3DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod3DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod3DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari3 = entities;
        }

        public void StokGrupKod4ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod4Command = new SqlCommand();
            stokGrupKod4Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod4Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod4Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod4Command.Parameters.AddWithValue("@Kod", "StokGrupKod4");
            stokGrupKod4Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod4DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod4Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod4DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod4DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod4DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari4 = entities;
        }

        public void StokGrupKod5ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod5Command = new SqlCommand();
            stokGrupKod5Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod5Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod5Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod5Command.Parameters.AddWithValue("@Kod", "StokGrupKod5");
            stokGrupKod5Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod5DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod5Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod5DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod5DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod5DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari5 = entities;
        }

        public void StokGrupKod6ListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod6Command = new SqlCommand();
            stokGrupKod6Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod6Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod6Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod6Command.Parameters.AddWithValue("@Kod", "StokGrupKod6");
            stokGrupKod6Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod6DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod6Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod6DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod6DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod6DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }
            ViewBag.StokGrupKodlari6 = entities;
        }

   
        public List<GrupKoduDto> StokGrupKodListesiniGetir(string kodAdi)
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand stokGrupKod1Command = new SqlCommand();
            stokGrupKod1Command.CommandText = "p_GrupKoduListesi";
            stokGrupKod1Command.CommandType = System.Data.CommandType.StoredProcedure;
            stokGrupKod1Command.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            stokGrupKod1Command.Parameters.AddWithValue("@Kod", kodAdi);
            stokGrupKod1Command.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(stokGrupKod1Command, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(stokGrupKod1DataTable.Rows[i]["Deger"]);
                entities.Add(entity);
            }

            return entities;
        }

        #endregion
    }
}