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
    public class GorevController : Controller
    {
        // GET: Gorev
        [HttpGet]
        public ActionResult GorevEkle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            GorevTipiListesiniOlustur();

            return View();
        }

        [HttpPost]
        public ActionResult GorevEkle(GorevDto gorevDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GorevKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@GorevTipiID", gorevDto.GorevTipiID);
            cmd.Parameters.AddWithValue("@Aciklama", gorevDto.Aciklama);
            cmd.Parameters.AddWithValue("@BaslangicTarihi", gorevDto.BaslangicTarihi);
            cmd.Parameters.AddWithValue("@Periyot", gorevDto.Periyot);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GorevListe");
        }

        public ActionResult GorevListe(GorevDto gorevDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GorevListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
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




        public void GorevTipiListesiniOlustur()
        {
            //GrupKodu1 Listesi oluşturma
            SqlCommand gorevtipiCommand = new SqlCommand();
            gorevtipiCommand.CommandText = "p_GrupKoduListesi";
            gorevtipiCommand.CommandType = System.Data.CommandType.StoredProcedure;
            gorevtipiCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

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
            ViewBag.GorevTipleri = entities;
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