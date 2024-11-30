using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;
using YKPortal.Areas.E.Models.Dto;

namespace YKPortal.Areas.E.Controllers
{
    public class KategoriController : Controller
    {
        // GET: E/Kategori
        [HttpGet]
        public ActionResult Kategoriler(KategorilerDto kategorilerDto)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "p_ETicaret_Kategoriler",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UyelikID", System.Configuration.ConfigurationManager.AppSettings["UyelikID"]);
            cmd.Parameters.AddWithValue("@CariID", kategorilerDto.CariID);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var kategoriler = new List<KategorilerDto>();
            foreach (DataRow row in dt.Rows)
            {
                kategoriler.Add(new KategorilerDto
                {
                    Kategori1 = row["Kategori1"].ToString(),
                    Kategori2 = row["Kategori2"].ToString(),
                    Kategori3 = row["Kategori3"].ToString(),
                    Kategori4 = row["Kategori4"].ToString(),
                    Kategori5 = row["Kategori5"].ToString(),
                    Kategori6 = row["Kategori6"].ToString(),
                });
            }

            return View(kategoriler);
        }




    }
}