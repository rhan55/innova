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
            cmd.CommandText = "SELECT * FROM BepSinif WITH(NOLOCK) WHERE EgitimDuzeyId=@egitimduzeyid order by Sira ASC";
            cmd.Parameters.AddWithValue("@egitimduzeyid", egitimduzeyid);
            cmd.CommandType = CommandType.Text;
            var list = new List<BepSinifDto>();
            if (egitimduzeyid.Length > 0)
            {
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

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
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult sinifduzeyigetir(string sinifId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT s1.Id,s1.Sira,s1.SinifDuzeyi, s2.EgitimDuzeyi,s2.ID as EgitimDuzeyId,s3.ID as SinifId, 
                                s3.Sinif
                                FROM BepSinifDuzeyi as s1 WITH(NOLOCK)
								LEFT JOIN 
                                BepEgitimDuzeyi as s2 WITH(NOLOCK)
								ON s1.EgitimDuzeyId=s2.ID
								LEFT JOIN BepSinif s3 
								ON s3.Id=s1.SinifId
                                where s1.EgitimDuzeyId=s2.ID and s3.Id=s1.SinifId and s1.SinifId=@SinifId
                                ORDER BY s1.Sira ASC";
            cmd.Parameters.AddWithValue("@SinifId", sinifId);
            cmd.CommandType = CommandType.Text;
            var list = new List<BepSinifDuzeyiDto>();
            if (sinifId != null)
            {
                if (sinifId.Length > 0)
                {
                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new BepSinifDuzeyiDto
                        {
                            ID = Convert.ToString(dt.Rows[i]["ID"]),
                            SinifDuzeyi = Convert.ToString(dt.Rows[i]["SinifDuzeyi"]),
                            EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                            Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                        });
                    }
                }
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
        public JsonResult SinifDuzeyiListele()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT s1.Id,s1.Sira,s1.SinifDuzeyi, s2.EgitimDuzeyi,s2.ID as EgitimDuzeyId,s3.ID as SinifId, 
                                s3.Sinif
                                FROM BepSinifDuzeyi as s1 WITH(NOLOCK), 
                                BepEgitimDuzeyi as s2 WITH(NOLOCK),BepSinif s3 
                                where s1.EgitimDuzeyId=s2.ID and s3.Id=s1.SinifId 
                                ORDER BY s1.Sira ASC";
            cmd.CommandType = CommandType.Text;

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepSinifDuzeyiDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepSinifDuzeyiDto
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                    EgitimDuzeyi = Convert.ToString(dt.Rows[i]["EgitimDuzeyi"]),
                    SinifId = Convert.ToString(dt.Rows[i]["SinifId"]),
                    Sinif = Convert.ToString(dt.Rows[i]["Sinif"]),
                    SinifDuzeyi = Convert.ToString(dt.Rows[i]["SinifDuzeyi"]),

                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SinifDuzeyiKaydet(string EgitimDuzeyId, string SinifId, string sinifduzeyi, string id, string sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepSinifDuzeyiKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", EgitimDuzeyId);
            cmd.Parameters.AddWithValue("@SinifId", SinifId);
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

        public JsonResult SinifDuzeyiDesleriniListele(string sinifduzeyId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"select * from BepDers with(nolock) where SinifDuzeyId=@SinifDuzeyId ORDER BY Sira ASC";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@SinifDuzeyId", sinifduzeyId);
            var ilcelistesi = new List<BepDers>();
            if (sinifduzeyId != null)
            {


                if (sinifduzeyId.Length > 0)
                {
                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ilcelistesi.Add(new BepDers
                        {
                            ID = Convert.ToString(dt.Rows[i]["ID"]),
                            EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                            SinifDuzeyId = Convert.ToString(dt.Rows[i]["SinifDuzeyId"]),
                            Ders = Convert.ToString(dt.Rows[i]["Ders"]),
                            Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                        });
                    }
                }
            }

            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
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
            cmd.CommandText = @"SELECT b.Id,b.Sira,b.Ders,s.Sinif,e.EgitimDuzeyi,b.EgitimDuzeyId,b.SinifId,s1.SinifDuzeyi,s1.Id as SinifDuzeyId FROM BepDers as b WITH(NOLOCK) 
                                LEFT JOIN BepSinif as s WITH(NOLOCK)
                                ON s.Id=b.SinifId
                                LEFT JOIN BepEgitimDuzeyi as E WITH(NOLOCK)
                                ON s.EgitimDuzeyId=e.ID
								LEFT JOIN BepSinifDuzeyi as s1 WITH(NOLOCK)
								on s1.Id=b.SinifDuzeyId
								ORDER BY B.Sira ASC";
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
                    Sinif = Convert.ToString(dt.Rows[i]["Sinif"]),
                    EgitimDuzeyi = Convert.ToString(dt.Rows[i]["EgitimDuzeyi"]),
                    SinifDuzeyi = Convert.ToString(dt.Rows[i]["SinifDuzeyi"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(liste, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DersKaydet(string ders, string id, string egitimduzeyid, string sinifduzeyId, string sinifId, string Sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepDersKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Ders", ders);
            cmd.Parameters.AddWithValue("@Sira", Sira);
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", egitimduzeyid);
            cmd.Parameters.AddWithValue("@SinifDuzeyId", sinifduzeyId);
            cmd.Parameters.AddWithValue("@SinifId", sinifId);
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

        public ActionResult Hedefler()
        {
            //if (!AutoGirisKontrol())
            //    return Redirect("~/YK/Giris");
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
        public JsonResult HedefleriGetir(string dersid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT s1.Id,s1.Hedef,s1.DersId,s1.Sira,s1.EgitimDuzeyId,s2.EgitimDuzeyi, s1.SinifId,s1.SinifDuzeyId ,s4.SinifDuzeyi,s3.Sinif,s5.Ders
													FROM BepHedef as s1 WITH(NOLOCK) 
													LEFT JOIN BepEgitimDuzeyi s2 WITH(NOLOCK)
													on s1.EgitimDuzeyId=s2.ID 
												    LEFT JOIN BepSinif s3 WITH(NOLOCK)
													on s1.SinifId=s3.Id
											        LEFT JOIN BepSinifDuzeyi s4 WITH(NOLOCK)
													on s1.SinifDuzeyId=s4.Id
													LEFT JOIN BepDers s5 WITH(NOLOCK)
													on s1.DersId=s5.Id
													Order by S1.Sira ASC"; // where DersId = @DersId //cmd.Parameters.AddWithValue("@DersId", dersid);

            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<BepHedef>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new BepHedef
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                    EgitimDuzeyi = Convert.ToString(dt.Rows[i]["EgitimDuzeyi"]),
                    Sinif = Convert.ToString(dt.Rows[i]["Sinif"]),
                    SinifId = Convert.ToString(dt.Rows[i]["SinifId"]),
                    SinifDuzeyId = Convert.ToString(dt.Rows[i]["SinifDuzeyId"]),
                    SinifDuzeyi = Convert.ToString(dt.Rows[i]["SinifDuzeyi"]),
                    DersId = Convert.ToString(dt.Rows[i]["DersId"]),
                    Ders = Convert.ToString(dt.Rows[i]["Ders"]),
                    Hedef = Convert.ToString(dt.Rows[i]["Hedef"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                });
            }
            return Json(liste, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HedefKaydet(string id,  string EgitimDuzeyId,string SinifId,string SinifDuzeyId, string DersId, string hedef, string sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepHedefKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@DersId", DersId);
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", EgitimDuzeyId);
            cmd.Parameters.AddWithValue("@SinifId", SinifId);
            cmd.Parameters.AddWithValue("@SinifDuzeyId", SinifDuzeyId);
            cmd.Parameters.AddWithValue("@Hedef", hedef);
            cmd.Parameters.AddWithValue("@Sira", sira);
            cmd.Parameters.AddWithValue("@KayitYapanKullanici", GetCookie("UyelikID"));
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult HedefSil(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "delete from BepHedef WHERE ID=@ID";
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
        public JsonResult AracGerecleriGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT s1.Id,s1.AracGerec,s1.DersId,s1.Sira,s1.EgitimDuzeyId,s2.EgitimDuzeyi, s1.SinifId,s1.SinifDuzeyId ,s4.SinifDuzeyi,s3.Sinif,s5.Ders
													FROM BepAracGerec as s1 WITH(NOLOCK) 
													LEFT JOIN BepEgitimDuzeyi s2 WITH(NOLOCK)
													on s1.EgitimDuzeyId=s2.ID 
												    LEFT JOIN BepSinif s3 WITH(NOLOCK)
													on s1.SinifId=s3.Id
											        LEFT JOIN BepSinifDuzeyi s4 WITH(NOLOCK)
													on s1.SinifDuzeyId=s4.Id
													LEFT JOIN BepDers s5 WITH(NOLOCK)
													on s1.DersId=s5.Id
													Order by S1.Sira ASC";
            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var liste = new List<BepAracGerec>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(new BepAracGerec
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    DersId = Convert.ToString(dt.Rows[i]["DersId"]),
                    Ders = Convert.ToString(dt.Rows[i]["Ders"]),
                    EgitimDuzeyi = Convert.ToString(dt.Rows[i]["EgitimDuzeyi"]),
                    EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                    SinifId = Convert.ToString(dt.Rows[i]["SinifId"]),
                    Sinif = Convert.ToString(dt.Rows[i]["Sinif"]),
                    SinifDuzeyId = Convert.ToString(dt.Rows[i]["SinifDuzeyId"]),
                    SinifDuzeyi = Convert.ToString(dt.Rows[i]["SinifDuzeyi"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                    AracGerec = Convert.ToString(dt.Rows[i]["AracGerec"]),
                });
            }
            return Json(liste, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AracGerecGetir(string dersid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM BepAracGerec WITH(NOLOCK)";
            //cmd.Parameters.AddWithValue("@DersId", dersid);
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
        public JsonResult AracGerecKaydet(string id, string DersId, string EgitimDuzeyId,string SinifId,string SinifDuzeyId, string aracgerec, string Sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepAracGerecKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", EgitimDuzeyId);
            cmd.Parameters.AddWithValue("@SinifId", SinifId);
            cmd.Parameters.AddWithValue("@SinifDuzeyId", SinifDuzeyId);
            cmd.Parameters.AddWithValue("@DersId", DersId);
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
        public JsonResult YontemTeknikGetir()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT s1.Id,s1.YontemTeknik,s1.DersId,s1.Sira,s1.EgitimDuzeyId,s2.EgitimDuzeyi, s1.SinifId,s1.SinifDuzeyId ,s4.SinifDuzeyi,s3.Sinif,s5.Ders
													FROM BepYontemTeknik as s1 WITH(NOLOCK) 
													LEFT JOIN BepEgitimDuzeyi s2 WITH(NOLOCK)
													on s1.EgitimDuzeyId=s2.ID 
												    LEFT JOIN BepSinif s3 WITH(NOLOCK)
													on s1.SinifId=s3.Id
											        LEFT JOIN BepSinifDuzeyi s4 WITH(NOLOCK)
													on s1.SinifDuzeyId=s4.Id
													LEFT JOIN BepDers s5 WITH(NOLOCK)
													on s1.DersId=s5.Id
													Order by S1.Sira ASC";
            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            var ilcelistesi = new List<BepYontemTeknik>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ilcelistesi.Add(new BepYontemTeknik
                {
                    ID = Convert.ToString(dt.Rows[i]["ID"]),
                    DersId = Convert.ToString(dt.Rows[i]["DersId"]),
                    Ders = Convert.ToString(dt.Rows[i]["Ders"]),
                    EgitimDuzeyi = Convert.ToString(dt.Rows[i]["EgitimDuzeyi"]),
                    EgitimDuzeyId = Convert.ToString(dt.Rows[i]["EgitimDuzeyId"]),
                    SinifId = Convert.ToString(dt.Rows[i]["SinifId"]),
                    Sinif = Convert.ToString(dt.Rows[i]["Sinif"]),
                    SinifDuzeyId = Convert.ToString(dt.Rows[i]["SinifDuzeyId"]),
                    SinifDuzeyi = Convert.ToString(dt.Rows[i]["SinifDuzeyi"]),
                    Sira = Convert.ToString(dt.Rows[i]["Sira"]),
                    YontemTeknik = Convert.ToString(dt.Rows[i]["YontemTeknik"]),
                });
            }
            return Json(ilcelistesi, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult YontemTeknikKaydet(string id, string SinifId, string SinifDuzeyId, string EgitimDuzeyId, string DersId, string yontemteknik, string sira)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepYontemTeknikKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@DersId", DersId);
            cmd.Parameters.AddWithValue("@SinifId", SinifId);
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", EgitimDuzeyId);
            cmd.Parameters.AddWithValue("@SinifDuzeyId", SinifDuzeyId);
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
            string dersler,
            string hedefler,
            string yontemler,
            string aracgerec
            )
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_BepKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IlId", ilid);
            cmd.Parameters.AddWithValue("@IlceId", ilceid);
            cmd.Parameters.AddWithValue("@OkulId", okulid);
            cmd.Parameters.AddWithValue("@Donem", donem);
            cmd.Parameters.AddWithValue("@Adi", adi);
            cmd.Parameters.AddWithValue("@Soyadi", soyadi);
            cmd.Parameters.AddWithValue("@EgitimDuzeyId", egitimduzeyi);
            cmd.Parameters.AddWithValue("@SinifId", sinif);
            cmd.Parameters.AddWithValue("@OgrenciVelisi", ogrencivelisi);
            cmd.Parameters.AddWithValue("@SinifDuzeyId", sinifduzeyi);
            cmd.Parameters.AddWithValue("@SinifRehberOgretmeni", sinifrehberogretmeni);
            cmd.Parameters.AddWithValue("@BransOgretmeni", bransogretmeni);
            cmd.Parameters.AddWithValue("@RehberOgretmeni", rehberogretmeni);
            cmd.Parameters.AddWithValue("@Uygulayici", uygulayici);
            cmd.Parameters.AddWithValue("@Mudur", mudur);
            cmd.Parameters.AddWithValue("@Dersler", dersler);
            String Id = IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek).ToString();
            List<String> _hedefler = hedefler.Split('&').ToList();
            List<String> _yontemler = yontemler.Split('&').ToList();
            List<String> _aracgerec = aracgerec.Split('&').ToList();
            for (int i = 0; i < _hedefler.Count; i++)
            {
                if (_hedefler[i] != "")
                {
                    string hedef = _hedefler[i].ToString().Split('|')[0];
                    string durum = _hedefler[i].ToString().Split('|')[1];

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandText = "INSERT INTO BepHedefler (BepId,HedefId,Durum)values(@BepId,@HedefId,@Durum)";
                    cmd1.CommandType = System.Data.CommandType.Text;
                    cmd1.Parameters.AddWithValue("@BepId", Id);
                    cmd1.Parameters.AddWithValue("@HedefId", hedef);
                    cmd1.Parameters.AddWithValue("@Durum", durum);
                    IDVeritabani.Sorgula(cmd1, SorgulaTuru.Bos);
                }
            }
            for (int i = 0; i < _yontemler.Count; i++)
            {
                if (_yontemler[i] != "")
                {
                    string yontem = _yontemler[i].ToString().Split('|')[0];
                    string durum = _yontemler[i].ToString().Split('|')[1];

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandText = "INSERT INTO BepYontemler (BepId,YontemId,Durum)values(@BepId,@YontemId,@Durum)";
                    cmd1.CommandType = System.Data.CommandType.Text;
                    cmd1.Parameters.AddWithValue("@BepId", Id);
                    cmd1.Parameters.AddWithValue("@YontemId", yontem);
                    cmd1.Parameters.AddWithValue("@Durum", durum);
                    IDVeritabani.Sorgula(cmd1, SorgulaTuru.Bos);
                }
            }
            for (int i = 0; i < _aracgerec.Count; i++)
            {
                if (_aracgerec[i] != "")
                {
                    string yontem = _aracgerec[i].ToString().Split('|')[0];
                    string durum = _aracgerec[i].ToString().Split('|')[1];

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandText = "INSERT INTO BepAracGerecler (BepId,AracGerecId,Durum)values(@BepId,@AracGerecId,@Durum)";
                    cmd1.CommandType = System.Data.CommandType.Text;
                    cmd1.Parameters.AddWithValue("@BepId", Id);
                    cmd1.Parameters.AddWithValue("@AracGerecId", yontem);
                    cmd1.Parameters.AddWithValue("@Durum", durum);
                    IDVeritabani.Sorgula(cmd1, SorgulaTuru.Bos);
                }
            }
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