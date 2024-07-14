using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;
using System.Net.Http;

namespace YKPortal.Controllers
{
    public class DosyaController : Controller
    {

        public ActionResult Liste(DosyaDto cariDosyaDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DosyaListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.KayitID);

            ViewBag.Modul = cariDosyaDto.Modul;
            ViewBag.KayitID = cariDosyaDto.KayitID;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }


        [HttpPost]
        public HttpResponseMessage Ekle(DosyaDto cariDosyaDto, HttpPostedFileBase Dosya)
        {
            HttpResponseMessage response;

            if (Dosya == null && Dosya.ContentLength <= 0)
            {
                response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                response.Content = new StringContent("Dosya bulunamadi.");

                return response;
            }

            if (!AutoGirisKontrol())
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

  

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "p_DosyaKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.KayitID);
            Dosya.SaveAs(Server.MapPath($"/Uploads/Dosyalar/{Dosya.FileName}"));
            cmd.Parameters.AddWithValue("@Dosya", Dosya.FileName);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent("Dosya yuklendi.");
            return response;

        }

        public ActionResult Duzenle(DosyaDto cariDosyaDto, string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Dosya";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.@KayitID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult Duzenle(DosyaDto cariDosyaDto, HttpPostedFileBase Dosya)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DosyaKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            //if (Dosya != null && Dosya.ContentLength > 0)
            //{   
            //    //dosyayı kaydetme işlemi ve 
            //    Dosya.SaveAs(Server.MapPath($"/Uploads/Dosyalar/{Dosya.FileName}"));
            //    cmd.Parameters.AddWithValue("@Dosya", $"/Uploads/Dosyalar/{Dosya.FileName}");
            //}


            cmd.Parameters.AddWithValue("@ID", cariDosyaDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.@KayitID);
            cmd.Parameters.AddWithValue("@Dosya", cariDosyaDto.Dosya);
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }


        [HttpPost]
        public ActionResult Sil(DosyaDto cariDosyaDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_DosyaSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", cariDosyaDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Modul", cariDosyaDto.Modul);
            cmd.Parameters.AddWithValue("@KayitID", cariDosyaDto.KayitID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            //if(System.IO.File.Exists("excel-no-2.xlsx"))
            //{
            //    System.IO.File.Delete("excel-no-2.xlsx");
            //} 
            return RedirectToAction("Liste", new {KayitID = cariDosyaDto.KayitID, Modul = cariDosyaDto.Modul});
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