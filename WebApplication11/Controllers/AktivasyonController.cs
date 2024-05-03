using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;
using WebApplication11.Models.Dto;
 


namespace WebApplication11.Controllers
{
    public class AktivasyonController : Controller
    {
        public ActionResult KullaniciOnayla(string kullaniciid)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_KullaniciAktivasyonuYap";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", kullaniciid);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count > 0)
            {
                string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                if (!Bilgi.StartsWith("UYARI!"))
                {
                   ViewBag.Durum = "Başarılı";                
                }
                else
                {
                   ViewBag.Durum = "Hata";
                }
                
                ViewBag.Bilgi = Bilgi;
            }
            return View();
        }
    }
}