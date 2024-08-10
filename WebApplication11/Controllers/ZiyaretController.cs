using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;

namespace YKPortal.Controllers
{
    public class ZiyaretController : Controller
    {

        [HttpGet]
        public ActionResult Ziyaretler()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Ziyaret/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", "");
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var ziyaretler = new List<ZiyaretDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CariDto cariDto = CariGetir(Convert.ToString(dt.Rows[i]["CariID"]));
                if (cariDto.ID.Length == 0)
                {
                    continue;
                }
                var kaydiAcan = KullaniciGetir(Convert.ToString(dt.Rows[i]["KullaniciID"]));
                var tamamlayan = new KullaniciEkleDto();
                if (Convert.ToString(dt.Rows[i]["TamamlayanKullaniciID"]).Length != 0)
                {
                    tamamlayan = KullaniciGetir(Convert.ToString(dt.Rows[i]["TamamlayanKullaniciID"]));
                }

                ziyaretler.Add(new ZiyaretDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["ID"]),
                    CariID = Convert.ToString(dt.Rows[i]["CariID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["CariID"]),
                    Tarih = Convert.ToString(dt.Rows[i]["Tarih"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Tarih"]),
                    ZiyaretTipi = Convert.ToString(dt.Rows[i]["ZiyaretTipi"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["ZiyaretTipi"]),
                    Aciklama = Convert.ToString(dt.Rows[i]["Aciklama"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Aciklama"]),
                    TamamlamaAciklamasi = Convert.ToString(dt.Rows[i]["TamamlamaAciklamasi"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["TamamlamaAciklamasi"]),
                    TamamlamaTarihi = Convert.ToString(dt.Rows[i]["TamamlamaTarihi"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["TamamlamaTarihi"]),
                    TamamlayanKullaniciID = Convert.ToString(dt.Rows[i]["TamamlayanKullaniciID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["TamamlayanKullaniciID"]),
                    KullaniciID = Convert.ToString(dt.Rows[i]["KullaniciID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["KullaniciID"]),
                    CariIsim = cariDto.Isim,
                    KaydiAcanIsim = kaydiAcan.Ad,
                    TamamlayanIsim = tamamlayan.Ad
                });
            }

            ViewBag.Ziyaretler = ziyaretler;
      

            return View();
        }
      

        [HttpGet]
        public ActionResult Ekle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Ziyaret/Ekle", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Ekle(ZiyaretDto ziyaretDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Ziyaret/Ekle", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "p_ZiyaretKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", ziyaretDto.CariID);
            cmd.Parameters.AddWithValue("@Tarih", ziyaretDto.Tarih.Length > 0 ? Convert.ToDateTime(ziyaretDto.Tarih).ToString() : null);
            cmd.Parameters.AddWithValue("@Aciklama", ziyaretDto.Aciklama);
            cmd.Parameters.AddWithValue("@TamamlamaAciklamasi", ziyaretDto.TamamlamaAciklamasi);
            cmd.Parameters.AddWithValue("@TamamlamaTarihi", ziyaretDto.TamamlamaTarihi.Length > 0 ? Convert.ToDateTime(ziyaretDto.TamamlamaTarihi).ToString() : null);
            cmd.Parameters.AddWithValue("@TamamlayanKullaniciID", ziyaretDto.TamamlayanKullaniciID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Ziyaretler");

        }

        [HttpGet]
        public ActionResult Duzenle(ZiyaretDto ziyaretDto, string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Ziyaret/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }


            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretKaydet";
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", ziyaretDto.CariID);
            cmd.Parameters.AddWithValue("@Tarih", ziyaretDto.Tarih.ToString());
            cmd.Parameters.AddWithValue("@Aciklama", ziyaretDto.Aciklama);
            cmd.Parameters.AddWithValue("@TamamlamaAciklamasi", ziyaretDto.TamamlamaAciklamasi);
            cmd.Parameters.AddWithValue("@TamamlamaTarihi", ziyaretDto.TamamlamaTarihi);
            cmd.Parameters.AddWithValue("@TamamlayanKullaniciID", ziyaretDto.TamamlayanKullaniciID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult Duzenle(ZiyaretDto ziyaretDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Ziyaret/Liste", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }
            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretKaydet";
            cmd.Parameters.AddWithValue("@ID", ziyaretDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", ziyaretDto.CariID);
            cmd.Parameters.AddWithValue("@Tarih", ziyaretDto.Tarih);
            cmd.Parameters.AddWithValue("@Aciklama", ziyaretDto.Aciklama);
            cmd.Parameters.AddWithValue("@TamamlamaAciklamasi", ziyaretDto.TamamlamaAciklamasi);
            cmd.Parameters.AddWithValue("@TamamlamaTarihi", ziyaretDto.TamamlamaTarihi);
            cmd.Parameters.AddWithValue("@TamamlayanKullaniciID", ziyaretDto.TamamlayanKullaniciID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }


        [HttpPost]
        public ActionResult Sil(ZiyaretDto ziyaretDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Ziyaret/Liste", "Sil"))
            {
                return Redirect("~/YK/Anasayfa");
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretSil";

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", ziyaretDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            //if(System.IO.File.Exists("excel-no-2.xlsx"))
            //{
            //    System.IO.File.Delete("excel-no-2.xlsx");
            //} 
            return RedirectToAction("Ziyaretler", new { CariID = ziyaretDto.CariID });
        }

        public KullaniciEkleDto KullaniciGetir(string ID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count == 0)
            {
                return new KullaniciEkleDto();
            }

            return new KullaniciEkleDto
            {
                Ad = Convert.ToString(dt.Rows[0]["Ad"])
            };
        }

        private CariDto CariGetir(string id)
        {

            if (id != null && id.Length > 0)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Cari";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count == 0) { 
                    return new CariDto();
                }
                return new CariDto
                {
                    ID = Convert.ToString(dt.Rows[0]["ID"]),
                    Isim = Convert.ToString(dt.Rows[0]["Isim"])
                };
            }
            return new CariDto { };
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
        #endregion

    }


}
