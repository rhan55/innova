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
    public class KullaniciController : Controller
    {

        // GET: Kullanici

        [HttpGet]

        public ActionResult Liste(string aranacakKelime = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Kullanici/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciListesi";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@AranacakKelime", aranacakKelime);

            ViewBag.AranacakKelime = aranacakKelime;
            IlListesiniOlustur();

            var iller = (List<GrupKoduDto>)ViewBag.Iller;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                string id = Convert.ToString(dt.Rows[i]["Il"]);
                var il = iller.Where(m => m.ID == id).ToList();
                if (il != null && il.Count > 0)
                {
                    dt.Rows[i]["Il"] = il[0].Deger;
                }
            }
            var model = new KullaniciListeViewModel
            {
                Kullanicilar = dt,
                Sil = YetkiKontrolu("/Kullanici/Liste", "Sil"),
                Duzenle = YetkiKontrolu("/Kullanici/Liste", "Duzenle")

            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Kullanici/Ekle", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            IlListesiniOlustur();

            return View();
        }

        [HttpPost]

        public ActionResult Ekle(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {
            IlListesiniOlustur();
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Kullanici/Ekle", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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
            cmd.Parameters.AddWithValue("@Onay", kullaniciEkleDto.Onay);
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

        [HttpGet]
        public ActionResult Duzenle(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Kullanici/Liste", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            IlListesiniOlustur();
            var uyelikId = GetCookie("UyelikID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Kullanici";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", uyelikId);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            IlListesiniOlustur();

            return View(dt);
        }

        [HttpPost]
        public ActionResult Duzenle(KullaniciEkleDto kullaniciEkleDto, HttpPostedFileBase Resim)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Kullanici/Liste", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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
            cmd.Parameters.AddWithValue("@Onay", kullaniciEkleDto.Onay);

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

                        return Redirect("~/Kullanici/Liste");
                        //return View(kullanici);
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

            if (!YetkiKontrolu("/Kullanici/Profil", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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

            if (!YetkiKontrolu("/Kullanici/Profil", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

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
        [HttpGet]
        private ActionResult Detay()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Kullanici/Detay", "Gor"))
            {
                return Redirect("~/YK/Anasayfa");
            }
            return View();
        }

        [HttpPost]
        private ActionResult Detay(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            if (!YetkiKontrolu("/Kullanici/Detay", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }
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


            if (!YetkiKontrolu("/Kullanici/Liste", "Sil"))
            {
                return Redirect("~/YK/Anasayfa");
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciSil";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@Kullanici", GetCookie("KullaniciID"));
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("Liste");
        }

        [HttpGet]
        public ActionResult Yetkiler(YetkilerDto yetkilerDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciYetkileri";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", yetkilerDto.KullaniciID);
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
                    Sil = Convert.ToBoolean(row["Sil"])
                });
            }


            YetkiYapisiniOlustur(yetkiler);

            return View();
        }

        [HttpPost]
        public ActionResult YetkiKaydet(List<YetkilerDto> yetkiler)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            if (!YetkiKontrolu("/Kullanici/Yetkiler", "Duzenle"))
            {
                return Redirect("~/YK/Anasayfa");
            }

            if (yetkiler.Count == 0)
            {
                RedirectToAction("Liste");
            }

            foreach (var yetki in yetkiler)
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "p_KullaniciYetkiKaydet";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
                cmd.Parameters.AddWithValue("@KullaniciID", yetki.KullaniciID);
                cmd.Parameters.AddWithValue("@MenuID", yetki.MenuID);
                cmd.Parameters.AddWithValue("@Gor", yetki.Gor);
                cmd.Parameters.AddWithValue("@Duzenle", yetki.Duzenle);
                cmd.Parameters.AddWithValue("@Sil", yetki.Sil);

                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            }

            return RedirectToAction("Yetkiler", new { KullaniciID = yetkiler[0].KullaniciID });
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

        public void YetkiYapisiniOlustur(List<YetkilerDto> yetkiler)
        {
            var enUstMenuler = yetkiler.Where(m => m.UstID == string.Empty).ToList();

            enUstMenuler.ForEach(m =>
            {

                m.AltListe = yetkiler.Where(x => x.UstID == m.MenuID).ToList();

                m.AltListe.ForEach(t =>
                {
                    t.AltListe = yetkiler.Where(x => x.UstID == t.MenuID).ToList();
                });
            });
            ViewBag.Yetkiler = enUstMenuler;
        }
    }
}