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
        public ActionResult Ziyaretler(ZiyaretDto ziyaretDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", ziyaretDto.CariID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var ziyaretler = new List<ZiyaretDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CariDto cariDto = CariGetir(Convert.ToString(dt.Rows[i]["CariID"]));

                ziyaretler.Add(new ZiyaretDto
                {
                    CariID = Convert.ToString(dt.Rows[i]["CariID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["CariID"]),
                    Tarih = Convert.ToString(dt.Rows[i]["Tarih"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Tarih"]),
                    ZiyaretTipi = Convert.ToString(dt.Rows[i]["ZiyaretTipi"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["ZiyaretTipi"]),
                    Aciklama = Convert.ToString(dt.Rows[i]["Aciklama"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["Aciklama"]),
                    TamamlamaAciklamasi = Convert.ToString(dt.Rows[i]["TamamlamaAciklamasi"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["TamamlamaAciklamasi"]),
                    TamamlamaTarihi = Convert.ToString(dt.Rows[i]["TamamlamaTarihi"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["TamamlamaTarihi"]),
                    TamamlayanKullaniciID = Convert.ToString(dt.Rows[i]["TamamlayanKullaniciID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["TamamlayanKullaniciID"]),
                    KullaniciID = Convert.ToString(dt.Rows[i]["KullaniciID"]) == null ? string.Empty : Convert.ToString(dt.Rows[i]["KullaniciID"]),
                    CariIsim = cariDto.Isim                
                });
            }

            ViewBag.Ziyaretler = ziyaretler;
            return View(dt);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(ZiyaretDto ziyaretDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "p_ZiyaretKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", ziyaretDto.CariID);
            cmd.Parameters.AddWithValue("@Tarih", ziyaretDto.Tarih);
            cmd.Parameters.AddWithValue("@Aciklama", ziyaretDto.Aciklama);
            cmd.Parameters.AddWithValue("@TamamlamaAciklamasi", ziyaretDto.TamamlamaAciklamasi);
            cmd.Parameters.AddWithValue("@TamamlamaTarihi", ziyaretDto.TamamlamaTarihi);
            cmd.Parameters.AddWithValue("@TamamlayanKullaniciID", ziyaretDto.TamamlayanKullaniciID);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Ziyaretler");

        }

        public ActionResult Duzenle(ZiyaretDto ziyaretDto, string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_ZiyaretKaydet";
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@CariID", ziyaretDto.CariID);
            cmd.Parameters.AddWithValue("@Tarih", ziyaretDto.Tarih);
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
            return RedirectToAction("Liste", new { CariID = ziyaretDto.CariID });
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


        #endregion
    }


}
