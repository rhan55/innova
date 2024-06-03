using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;

namespace YKPortal.Controllers
{
    public class KullaniciController: Controller
    {

        // GET: Kullanici
        public ActionResult Liste(string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.AranacakKelime = aranacakKelime;
            IlListesiniOlustur();

            var iller = (List<GrupKoduDto>)ViewBag.Iller;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            for(var i = 0; i < dt.Rows.Count; i++)
            {
                string id = Convert.ToString(dt.Rows[i]["Il"]);
                var il = iller.Where(m => m.ID == id).ToList();
                if (il != null && il.Count > 0)
                {
                    dt.Rows[i]["Il"] = il[0].Deger;
                }
            }

            return View(dt);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            IlListesiniOlustur();

            return View();
        }

        [HttpPost]

        public ActionResult Ekle(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string imageName = "/Tema/Media/Logolar/amblem.png";
            var id = Guid.NewGuid();
            if (Resim != null && Resim.ContentLength > 0)
            {
                var extension = Resim.FileName.Split('.').Last();

                imageName = $"/Uploads/Avatarlar/{id}_{Guid.NewGuid()}.{extension}";
                Resim.SaveAs(Server.MapPath($"{imageName}"));
            }

            kullaniciEkleDto.UyelikID = GetCookie("UyelikID");

            kullaniciEkleDto.Kullanici = GetCookie("KullaniciID");

            if (kullaniciEkleDto.Aciklama1 == null)
            {
                kullaniciEkleDto.Aciklama1 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama2 == null)
            {
                kullaniciEkleDto.Aciklama2 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama3 == null)
            {
                kullaniciEkleDto.Aciklama3 = string.Empty;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Resim", $"{imageName}");
            cmd.Parameters.AddWithValue("@ID", "");
            cmd.Parameters.AddWithValue("@UyelikID", kullaniciEkleDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciEkleDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", kullaniciEkleDto.Parola);
            cmd.Parameters.AddWithValue("@Ad", kullaniciEkleDto.Ad);
            cmd.Parameters.AddWithValue("@Soyad", kullaniciEkleDto.Soyad);
            cmd.Parameters.AddWithValue("@Aktif", kullaniciEkleDto.Aktif);
            cmd.Parameters.AddWithValue("@Telefon", kullaniciEkleDto.Telefon);
            cmd.Parameters.AddWithValue("@Adres", kullaniciEkleDto.Adres);
            cmd.Parameters.AddWithValue("@Il", kullaniciEkleDto.Il);
            cmd.Parameters.AddWithValue("@Ilce", kullaniciEkleDto.Ilce);
            cmd.Parameters.AddWithValue("@Aciklama1", kullaniciEkleDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", kullaniciEkleDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", kullaniciEkleDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Kullanici", kullaniciEkleDto.Kullanici);


            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                string ID = Convert.ToString(dt.Rows[0]["ID"]);
                if (ID != "0")
                {
                    return Redirect("~/Kullanici/Liste");
                }
            }

            ViewBag.Form = kullaniciEkleDto;

            return View(dt);
        }

        public ActionResult Duzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            IlListesiniOlustur();
            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        [HttpPost]
        public ActionResult Duzenle(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!ModelState.IsValid)
            {
                ViewBag.ValidationError = ModelState.Values
                                                        .SelectMany(m => m.Errors)
                                                        .Select(m => m.ErrorMessage).ToList();
                return Duzenle(kullaniciEkleDto.ID);
            }


            kullaniciEkleDto.UyelikID = GetCookie("UyelikID");

            kullaniciEkleDto.Kullanici = GetCookie("KullaniciID");

            if (kullaniciEkleDto.Aciklama1 == null)
            {
                kullaniciEkleDto.Aciklama1 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama2 == null)
            {
                kullaniciEkleDto.Aciklama2 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama3 == null)
            {
                kullaniciEkleDto.Aciklama3 = string.Empty;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;



            if (Resim != null && Resim.ContentLength > 0)
            {
                var extension = Resim.FileName.Split('.').Last();

                var imageName = $"{kullaniciEkleDto.ID}_{Guid.NewGuid()}.{extension}";
                Resim.SaveAs(Server.MapPath($"/Uploads/Avatarlar/{imageName}"));
                cmd.Parameters.AddWithValue("@Resim", $"/Uploads/Avatarlar/{imageName}");
            }


            cmd.Parameters.AddWithValue("@ID", kullaniciEkleDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", kullaniciEkleDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciEkleDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", kullaniciEkleDto.Parola);
            cmd.Parameters.AddWithValue("@Ad", kullaniciEkleDto.Ad);
            cmd.Parameters.AddWithValue("@Soyad", kullaniciEkleDto.Soyad);
            cmd.Parameters.AddWithValue("@Aktif", kullaniciEkleDto.Aktif);
            cmd.Parameters.AddWithValue("@Telefon", kullaniciEkleDto.Telefon);
            cmd.Parameters.AddWithValue("@Adres", kullaniciEkleDto.Adres);
            cmd.Parameters.AddWithValue("@Il", kullaniciEkleDto.Il);
            cmd.Parameters.AddWithValue("@Ilce", kullaniciEkleDto.Ilce);
            cmd.Parameters.AddWithValue("@Aciklama1", kullaniciEkleDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", kullaniciEkleDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", kullaniciEkleDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Kullanici", kullaniciEkleDto.Kullanici);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            if (dt.Rows.Count > 0)
            {
                var columns = dt.Columns;
                if (columns.Contains("Bilgi"))
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (Bilgi == "Kullanıcı başarıyla oluşturuldu.")
                    {
                        return Redirect("~/Kullanici/Liste");
                    }
                    else
                    {
                        ViewBag.Bilgi = Bilgi;

                        var kullanici = Duzenle(Convert.ToString(dt.Rows[0]["ID"]));

                        return View(kullanici);
                    }
                }

            }
            IlListesiniOlustur();
            return RedirectToAction("Liste");
        }


        public ActionResult Profil(string Mesaj = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            IlListesiniOlustur();
            var uyelikId = GetCookie("UyelikID");
            var kullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", kullaniciID);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.Mesaj = Mesaj;
            return View(dt);
        }

        [HttpPost]
        public ActionResult Profil(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            IlListesiniOlustur();
            if (!ModelState.IsValid)
            {
                ViewBag.ValidationError = ModelState.Values
                                                        .SelectMany(m => m.Errors)
                                                        .Select(m => m.ErrorMessage).ToList();
                return Profil();
            }

            kullaniciEkleDto.UyelikID = GetCookie("UyelikID");

            kullaniciEkleDto.Kullanici = GetCookie("KullaniciID");

            if (kullaniciEkleDto.Aciklama1 == null)
            {
                kullaniciEkleDto.Aciklama1 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama2 == null)
            {
                kullaniciEkleDto.Aciklama2 = string.Empty;
            }

            if (kullaniciEkleDto.Aciklama3 == null)
            {
                kullaniciEkleDto.Aciklama3 = string.Empty;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (Resim != null && Resim.ContentLength > 0)
            {
                var extension = Resim.FileName.Split('.').Last();

                var imageName = $"{kullaniciEkleDto.ID}_{Guid.NewGuid()}.{extension}";
                Resim.SaveAs(Server.MapPath($"/Uploads/Avatarlar/{imageName}"));
                cmd.Parameters.AddWithValue("@Resim", $"/Uploads/Avatarlar/{imageName}");
            }

            cmd.Parameters.AddWithValue("@ID", kullaniciEkleDto.ID);
            cmd.Parameters.AddWithValue("@UyelikID", kullaniciEkleDto.UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciEkleDto.KullaniciAdi);
            cmd.Parameters.AddWithValue("@Parola", kullaniciEkleDto.Parola);
            cmd.Parameters.AddWithValue("@Ad", kullaniciEkleDto.Ad);
            cmd.Parameters.AddWithValue("@Soyad", kullaniciEkleDto.Soyad);
            cmd.Parameters.AddWithValue("@Aktif", kullaniciEkleDto.Aktif);
            cmd.Parameters.AddWithValue("@Telefon", kullaniciEkleDto.Telefon);
            cmd.Parameters.AddWithValue("@Adres", kullaniciEkleDto.Adres);
            cmd.Parameters.AddWithValue("@Il", kullaniciEkleDto.Il);
            cmd.Parameters.AddWithValue("@Ilce", kullaniciEkleDto.Ilce);
            cmd.Parameters.AddWithValue("@Aciklama1", kullaniciEkleDto.Aciklama1);
            cmd.Parameters.AddWithValue("@Aciklama2", kullaniciEkleDto.Aciklama2);
            cmd.Parameters.AddWithValue("@Aciklama3", kullaniciEkleDto.Aciklama3);
            cmd.Parameters.AddWithValue("@Kullanici", kullaniciEkleDto.Kullanici);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                var columns = dt.Columns;
                if (columns.Contains("Bilgi"))
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (!Bilgi.StartsWith("UYARI!"))
                    {
                        return RedirectToAction("Profil", new { Mesaj = "Profil başarıyla güncellendi." });
                    }
                    else
                    {
                        return RedirectToAction("Profil", new { Mesaj = Bilgi });
                    }
                }
            }

            return RedirectToAction("Profil");
        }

        private ActionResult Detay()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            return View();
        }
        private ActionResult Detay(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View();
        }

        [HttpPost]
        public ActionResult Sil(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
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

        public void IlListesiniOlustur()
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand ilCommand = new SqlCommand();
            ilCommand.CommandText = "p_GrupKoduListesi";
            ilCommand.CommandType = System.Data.CommandType.StoredProcedure;
            ilCommand.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            ilCommand.Parameters.AddWithValue("@Kod", "Il");
            ilCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable ilDataTable = (DataTable)IDVeritabani.Sorgula(ilCommand, SorgulaTuru.Tablo);

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
    }
}