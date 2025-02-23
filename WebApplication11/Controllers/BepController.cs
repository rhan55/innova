using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKEFaturaEntegrasyon.EFaturaEDM;
using YKPortal.Models.Dto;
using System.Drawing;

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
                entity.KayitYapanKullanici = Convert.ToString(stokGrupKod1DataTable.Rows[i]["KayitYapanKullanici"]);
                entities.Add(entity);
            }
            ViewBag.iller = entities;
            return View();
        }
        public JsonResult ilcegetir(string ilid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Bepilce WITH(NOLOCK) WHERE ILID=@ILID";
            cmd.Parameters.AddWithValue("@ILID", ilid);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<ILCEDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new ILCEDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    ILCE = Convert.ToString(dt.Rows[i]["ILCE"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ilcekaydet(string ilid, string ilceid, string ilce)
        {
            SqlCommand cmd = new SqlCommand("p_BepilceKaydet");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ILID", ilid);
            cmd.Parameters.AddWithValue("@ILCEID", ilceid);
            cmd.Parameters.AddWithValue("@ILCE", ilce);
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
            cmd.CommandText = "SELECT * FROM BepOkul WITH(NOLOCK) WHERE ILCEID=@ILCEID AND ILID=@ILID";
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
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }
        public JsonResult okulkaydet(string id, string ilid, string ilceid, string okul)
        {
            SqlCommand cmd = new SqlCommand("p_BepOkulKaydet");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@ILID", ilid);
            cmd.Parameters.AddWithValue("@ILCEID", ilceid);
            cmd.Parameters.AddWithValue("@OKUL", okul);
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
        public ActionResult EgitimDuzeyiKaydet(string EgitimDuzeyi, string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "pBepEgitimDuzeyiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@EgitimDuzeyi", EgitimDuzeyi);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            ViewBag.iller = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return Redirect("~/bep/egitimduzeyi");
        }
        public JsonResult egitimduzeylerigetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepEgitimDuzeyi WITH(NOLOCK)";
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepEgitimDuzeyi>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepEgitimDuzeyi
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    EgitimDuzeyi = Convert.ToString(dt.Rows[i]["EgitimDuzeyi"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
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
        public JsonResult siniflarigetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepSinif WITH(NOLOCK)";
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
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        public JsonResult siniflarigetir(string egitimduzeyid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepSinif WITH(NOLOCK) WHERE EgitimDuzeyId=@EgitimDuzeyId";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", egitimduzeyid);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepSinifDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepSinifDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Sinif = Convert.ToString(dt.Rows[i]["Sinif"]),
                    EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SinifKaydet(string sinif, string id, string egitimduzeyid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepSinifKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", egitimduzeyid);
            cmd.Parameters.AddWithValue("@Sinif", sinif);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Redirect("~/bep/egitimduzeyi");
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
            cmd.CommandText = "SELECT * FROM BepSinifDuzeyi WITH(NOLOCK)";
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepSinifDuzeyiDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepSinifDuzeyiDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    SinifDuzeyi = Convert.ToString(dt.Rows[i]["SinifDuzeyi"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SinifDuzeyiKaydet(string sinifduzeyi, string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepSinifDuzeyiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@SinifDuzeyi", sinifduzeyi);
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
            //if (!AutoGirisKontrol())
            //    return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");
            return View();
        }
        public JsonResult DersGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepDers WITH(NOLOCK)";
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepDers>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepDers
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    Ders = Convert.ToString(dt.Rows[i]["Ders"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DersKaydet(string ders, string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepDersKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Ders", ders);
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
            cmd.CommandText = "SELECT * FROM BepSoru WITH(NOLOCK) where DersId=@DersId";
            cmd.Parameters.AddWithValue("@DersId", dersid);
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepSoru>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepSoru
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    DERSID = Convert.ToString(dt.Rows[i]["DERSID"]),
                    Soru = Convert.ToString(dt.Rows[i]["Soru"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SoruKaydet(string soru, string id,string dersid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepSoruKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Soru", soru);
            cmd.Parameters.AddWithValue("@DersId", dersid);
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
            cmd.CommandText = "SELECT * FROM BepAracGerec WITH(NOLOCK) where DersId=@DersId";
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
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AracGerecKaydet(string id, string dersid, string aracgerec)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepAracGerecKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@DersId", dersid);
            cmd.Parameters.AddWithValue("@AracGerec", aracgerec);
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
                    YontemTeknik = Convert.ToString(dt.Rows[i]["YontemTeknik"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult YontemTeknikKaydet(string id, string dersid, string yontemteknik)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepYontemTeknikKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@DersId", dersid);
            cmd.Parameters.AddWithValue("@YontemTeknik", yontemteknik);
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