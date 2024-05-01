using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models.Dto;

namespace WebApplication11.Controllers
{
    public class TanimlamalarController : Controller
    {
        // GET: Tanimlamalar

        public ActionResult Kaydet()
        {
            return View();
        }
        public ActionResult Kaydet(GrupKoduDto grupKoduDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", grupKoduDto.ID);
            cmd.Parameters.AddWithValue("UyelikID", grupKoduDto.UyelikID);
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);
            return View();
        }
    }
}