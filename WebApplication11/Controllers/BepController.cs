using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;

namespace YKPortal.Controllers
{
    public class BepController : Controller
    {
        public ActionResult Index()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");
            return View();
        }
        public ActionResult AnaSayfa()
        {

            SqlCommand cmdil = new SqlCommand();
            cmdil.CommandText = "select * from BepIL With(nolock)  ORDER BY Sira ASC";
            cmdil.CommandType = System.Data.CommandType.Text;
            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cmdil, SorgulaTuru.Tablo);
            List<ILDto> entities = new List<ILDto>();
            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                ILDto entity = new ILDto();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.IL = Convert.ToString(stokGrupKod1DataTable.Rows[i]["IL"]);
                entities.Add(entity);
            }
            ViewBag.iller = entities;
            return View();
        }
        public ActionResult il()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmdil = new SqlCommand();
            cmdil.CommandText = "select * from BepIL With(nolock)";
            cmdil.CommandType = System.Data.CommandType.Text;
            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cmdil, SorgulaTuru.Tablo);
            List<ILDto> entities = new List<ILDto>();
            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                ILDto entity = new ILDto();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.IL = Convert.ToString(stokGrupKod1DataTable.Rows[i]["IL"]);
                entity.KayitYapanKullanici = Convert.ToString(stokGrupKod1DataTable.Rows[i]["KayitYapanKullanici"]);
                entities.Add(entity);
            }
            ViewBag.iller = entities;
            return View();
        }
        public JsonResult ilgetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepIL WITH(NOLOCK)";

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<ILDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new ILDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    IL = Convert.ToString(dt.Rows[i]["IL"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ilekle(string il, string id)
        {
            SqlCommand cmd = new SqlCommand("p_BepilKaydet");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@il", il);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult İlSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepIL WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult il_getir(string aranacakkelime)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepIL WITH(NOLOCK) WHERE IL Like @Ara";
            cmd.Parameters.AddWithValue("@Ara", "%" + aranacakkelime + "%");
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<ILDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new ILDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    IL = Convert.ToString(dt.Rows[i]["IL"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ilce()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmdil = new SqlCommand();
            cmdil.CommandText = "select * from BepIL With(nolock)";
            cmdil.CommandType = System.Data.CommandType.Text;
            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cmdil, SorgulaTuru.Tablo);
            List<ILDto> entities = new List<ILDto>();
            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                ILDto entity = new ILDto();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.IL = Convert.ToString(stokGrupKod1DataTable.Rows[i]["IL"]);
                entities.Add(entity);
            }
            ViewBag.iller = entities;
            return View();
        }

        [HttpPost]
        public JsonResult ilcegetir(string ilid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Bepilce WITH(NOLOCK) WHERE ILID=@ILID ORDER BY Sira ASC";
            cmd.Parameters.AddWithValue("@ILID", ilid);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<ILCEDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new ILCEDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    ilce = Convert.ToString(dt.Rows[i]["ilce"]),
                    ILID = Convert.ToString(dt.Rows[i]["ILID"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ilce_getir(string aranacakkelime, string ilid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Bepilce WITH(NOLOCK) WHERE ILID=@ILID AND ilce like @Ara ORDER BY Sira ASC";
            cmd.Parameters.AddWithValue("@Ara", "%" + aranacakkelime + "%");
            cmd.Parameters.AddWithValue("@ILID", ilid);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<ILCEDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new ILCEDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    ilce = Convert.ToString(dt.Rows[i]["ilce"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ilcekaydet(string ilid, string ilceid, string ilce, string Sira)
        {
            SqlCommand cmd = new SqlCommand("p_BepilceKaydet");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ILID", ilid);
            cmd.Parameters.AddWithValue("@ILCEID", ilceid);
            cmd.Parameters.AddWithValue("@ILCE", ilce);
            cmd.Parameters.AddWithValue("@Sira", Sira);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult İlceSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from Bepilce WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Okul()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmdil = new SqlCommand();
            cmdil.CommandText = "select * from BepIL With(nolock)";
            cmdil.CommandType = System.Data.CommandType.Text;
            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cmdil, SorgulaTuru.Tablo);
            List<ILDto> entities = new List<ILDto>();
            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                ILDto entity = new ILDto();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.IL = Convert.ToString(stokGrupKod1DataTable.Rows[i]["IL"]);
                entity.KayitYapanKullanici = Convert.ToString(stokGrupKod1DataTable.Rows[i]["KayitYapanKullanici"]);
                entities.Add(entity);
            }
            ViewBag.iller = entities;
            return View();
        }
        public JsonResult okulgetir(string ilid, string ilceid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepOkul WITH(NOLOCK) WHERE ILCEID=@ILCEID AND ILID=@ILID ORDER BY Sira ASC";
            cmd.Parameters.AddWithValue("@ILID", ilid);
            cmd.Parameters.AddWithValue("@ILCEID", ilceid);
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<OkulDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new OkulDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    ILID = Convert.ToString(dt.Rows[i]["ILID"]),
                    ILCEID = Convert.ToString(dt.Rows[i]["ILCEID"]),
                    OKUL = Convert.ToString(dt.Rows[i]["OKUL"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }
        public JsonResult okulkaydet(string id, string ilid, string ilceid, string okul, string Sira)
        {
            SqlCommand cmd = new SqlCommand("p_BepOkulKaydet");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@ILID", ilid);
            cmd.Parameters.AddWithValue("@ILCEID", ilceid);
            cmd.Parameters.AddWithValue("@OKUL", okul);
            cmd.Parameters.AddWithValue("@Sira", Sira);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult OkulSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepOkul WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult EgitimDuzeyi()
        {
            //if (!AutoGirisKontrol())
            //    return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");


            SqlCommand cmdil = new SqlCommand();
            cmdil.CommandText = "select * from BepEgitimDuzeyi With(nolock)";
            cmdil.CommandType = System.Data.CommandType.Text;
            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cmdil, SorgulaTuru.Tablo);
            List<BepEgitimDuzeyi> entities = new List<BepEgitimDuzeyi>();
            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                BepEgitimDuzeyi entity = new BepEgitimDuzeyi();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.EgitimDuzeyi = Convert.ToString(stokGrupKod1DataTable.Rows[i]["EgitimDuzeyi"]);
                entity.KayitYapanKullanici = Convert.ToString(stokGrupKod1DataTable.Rows[i]["KayitYapanKullanici"]);
                entities.Add(entity);
            }
            ViewBag.iller = entities;
            return View();
        }

        [HttpPost]
        public ActionResult EgitimDuzeyiKaydet(string EgitimDuzeyi, string id, string sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepEgitimDuzeyiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Sira", sira);
            cmd.Parameters.AddWithValue("@EgitimDuzeyi", EgitimDuzeyi);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            ViewBag.iller = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return Redirect("~/bep/egitimduzeyi");
        }
        public JsonResult egitimduzeylerigetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepEgitimDuzeyi WITH(NOLOCK) ORDER BY Sira ASC";
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var list = new List<BepEgitimDuzeyi>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(new BepEgitimDuzeyi
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    EgitimDuzeyi = Convert.ToString(dt.Rows[i]["EgitimDuzeyi"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult egitimduzeylerisinifgetir(string egitimduzeyid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepSinif WITH(NOLOCK) WHERE EgitimDuzeyId=@egitimduzeyid";
            cmd.Parameters.AddWithValue("@egitimduzeyid", egitimduzeyid);
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var list = new List<BepSinifDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(new BepSinifDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Sinif = Convert.ToString(dt.Rows[i]["Sinif"]),
                    EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EgitimDuzeyiSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepEgitimDuzeyi WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sinif()
        {
            //if (!AutoGirisKontrol())
            //    return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");


            SqlCommand cmdil = new SqlCommand();
            cmdil.CommandText = "SELECT * FROM BepEgitimDuzeyi WITH(NOLOCK) ORDER BY Sira ASC";
            cmdil.CommandType = System.Data.CommandType.Text;
            DataTable stokGrupKod1DataTable = (DataTable)IDVeritabani.Sorgula(cmdil, SorgulaTuru.Tablo);
            List<BepEgitimDuzeyi> entities = new List<BepEgitimDuzeyi>();
            for (int i = 0; i < stokGrupKod1DataTable.Rows.Count; i++)
            {
                BepEgitimDuzeyi entity = new BepEgitimDuzeyi();
                entity.ID = Convert.ToString(stokGrupKod1DataTable.Rows[i]["ID"]);
                entity.EgitimDuzeyi = Convert.ToString(stokGrupKod1DataTable.Rows[i]["EgitimDuzeyi"]);
                entity.KayitYapanKullanici = Convert.ToString(stokGrupKod1DataTable.Rows[i]["KayitYapanKullanici"]);
                entities.Add(entity);
            }
            ViewBag.iller = entities;
            return View();
        }
        public JsonResult siniflarigetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepSinif WITH(NOLOCK) ORDER BY Sira ASC";
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepSinifDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepSinifDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Sinif = Convert.ToString(dt.Rows[i]["Sinif"]),
                    EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult siniflarigetir(string egitimduzeyid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepSinif WITH(NOLOCK) WHERE EgitimDuzeyId=@EgitimDuzeyId ORDER BY Sira ASC";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", egitimduzeyid);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var list = new List<BepSinifDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(new BepSinifDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Sinif = Convert.ToString(dt.Rows[i]["Sinif"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                    EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SinifKaydet(string sinif, string id, string egitimduzeyid, string sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepSinifKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", egitimduzeyid);
            cmd.Parameters.AddWithValue("@Sinif", sinif);
            cmd.Parameters.AddWithValue("@Sira", sira);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SinifSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepSinif WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SinifDuzeyi()
        {
            //if (!AutoGirisKontrol())
            //    return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");
            return View();
        }
        public JsonResult SinifDuzeyiGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepSinifDuzeyi WITH(NOLOCK) ORDER BY Sira ASC";
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepSinifDuzeyiDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepSinifDuzeyiDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    SinifDuzeyi = Convert.ToString(dt.Rows[i]["SinifDuzeyi"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SinifDuzeyiKaydet(string sinifduzeyi, string id, string sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepSinifDuzeyiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@SinifDuzeyi", sinifduzeyi);
            cmd.Parameters.AddWithValue("@Sira", sira);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SinifDuzeyiSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepSinifDuzeyi WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Dersler()
        {
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepEgitimDuzeyi WITH(NOLOCK) ORDER BY Sira ASC";
            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<BepEgitimDuzeyi>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new BepEgitimDuzeyi
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    EgitimDuzeyi = Convert.ToString(dt.Rows[i]["EgitimDuzeyi"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            ViewBag.EgitimDuzeyi = liste;
            return View();
        }
        public JsonResult DersGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepDers WITH(NOLOCK) ORDER BY Sira ASC";
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<BepDers>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new BepDers
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Ders = Convert.ToString(dt.Rows[i]["Ders"]),
                    EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                    SinifId = Convert.ToString(dt.Rows[i]["SinifId"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(liste, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DersKaydet(string ders, string id, string Sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepDersKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Ders", ders);
            cmd.Parameters.AddWithValue("@Sira", Sira);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult DersSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepDers WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sorular()
        {
            //if (!AutoGirisKontrol())
            //    return Redirect("~/YK/Giris");
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmdil = new SqlCommand();
            cmdil.CommandText = "select * from BepDers With(nolock)";
            cmdil.CommandType = System.Data.CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmdil, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepDers>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepDers
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Ders = Convert.ToString(dt.Rows[i]["Ders"]),
                });
            }
            ViewBag.iller = ilcelistesi;
            return View();
        }
        public JsonResult SorulariGetir(string dersid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepSoru WITH(NOLOCK) where DersId=@DersId ORDER BY Sira ASC";
            cmd.Parameters.AddWithValue("@DersId", dersid);
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<BepSoru>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new BepSoru
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    DERSID = Convert.ToString(dt.Rows[i]["DERSID"]),
                    Soru = Convert.ToString(dt.Rows[i]["Soru"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(liste, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SoruKaydet(string soru, string id, string dersid, string sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepSoruKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Soru", soru);
            cmd.Parameters.AddWithValue("@DersId", dersid);
            cmd.Parameters.AddWithValue("@Sira", sira);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SoruSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepSoru WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AracGerec()
        {
            //if (!AutoGirisKontrol())
            //    return Redirect("~/YK/Giris");
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmdil = new SqlCommand();
            cmdil.CommandText = "select * from BepDers With(nolock)";
            cmdil.CommandType = System.Data.CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmdil, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepDers>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepDers
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Ders = Convert.ToString(dt.Rows[i]["Ders"]),
                });
            }
            ViewBag.iller = ilcelistesi;
            return View();
        }
        public JsonResult AracGerecGetir(string dersid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepAracGerec WITH(NOLOCK) where DersId=@DersId ORDER BY Sira ASC";
            cmd.Parameters.AddWithValue("@DersId", dersid);
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepAracGerec>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepAracGerec
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    DersId = Convert.ToString(dt.Rows[i]["DersId"]),
                    AracGerec = Convert.ToString(dt.Rows[i]["AracGerec"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AracGerecKaydet(string id, string dersid, string aracgerec, string Sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepAracGerecKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@DersId", dersid);
            cmd.Parameters.AddWithValue("@AracGerec", aracgerec);
            cmd.Parameters.AddWithValue("@Sira", Sira);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult AracGerecSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepAracGerec WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult YontemTeknik()
        {
            //if (!AutoGirisKontrol())
            //    return Redirect("~/YK/Giris");
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmdil = new SqlCommand();
            cmdil.CommandText = "select * from BepDers With(nolock)";
            cmdil.CommandType = System.Data.CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmdil, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepDers>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepDers
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Ders = Convert.ToString(dt.Rows[i]["Ders"]),
                });
            }
            ViewBag.iller = ilcelistesi;
            return View();
        }
        public JsonResult YontemTeknikGetir(string dersid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepYontemTeknik WITH(NOLOCK) where DersId=@DersId";
            cmd.Parameters.AddWithValue("@DersId", dersid);
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepYontemTeknik>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepYontemTeknik
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    DersId = Convert.ToString(dt.Rows[i]["DersId"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                    YontemTeknik = Convert.ToString(dt.Rows[i]["YontemTeknik"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult YontemTeknikKaydet(string id, string dersid, string yontemteknik, string sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepYontemTeknikKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@DersId", dersid);
            cmd.Parameters.AddWithValue("@YontemTeknik", yontemteknik);
            cmd.Parameters.AddWithValue("@Sira", sira);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult YontemTeknikSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepYontemTeknik WHERE ID=@ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BepKaydet(
            string ilid,
            string ilceid,
            string okulid,
            string donem,
            string adi,
            string soyadi,
            string egitimduzeyi,
            string sinif,
            string ogrencivelisi,
            string sinifduzeyi,
            string sinifrehberogretmeni,
            string bransogretmeni,
            string rehberogretmeni,
            string uygulayici,
            string mudur,
            string dersler
            )
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ilid", ilid);
            cmd.Parameters.AddWithValue("@ilceid", ilceid);
            cmd.Parameters.AddWithValue("@okulid", okulid);
            cmd.Parameters.AddWithValue("@donem", donem);
            cmd.Parameters.AddWithValue("@adi", adi);
            cmd.Parameters.AddWithValue("@soyadi", soyadi);
            cmd.Parameters.AddWithValue("@egitimduzeyi", egitimduzeyi);
            cmd.Parameters.AddWithValue("@sinif", sinif);
            cmd.Parameters.AddWithValue("@ogrencivelisi", ogrencivelisi);
            cmd.Parameters.AddWithValue("@sinifduzeyi", sinifduzeyi);
            cmd.Parameters.AddWithValue("@sinifrehberogretmeni", sinifrehberogretmeni);
            cmd.Parameters.AddWithValue("@bransogretmeni", bransogretmeni);
            cmd.Parameters.AddWithValue("@rehberogretmeni", rehberogretmeni);
            cmd.Parameters.AddWithValue("@uygulayici", uygulayici);
            cmd.Parameters.AddWithValue("@mudur", mudur);
            cmd.Parameters.AddWithValue("@dersler", dersler);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
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
    }
}