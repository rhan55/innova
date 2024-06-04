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
    public class ParametreController : Controller
    {
        // GET: Parametreler
        public ActionResult MailAyarları()
        {
            var parametreler = Parametreler();

            var parametreListesi = new List<ParametreDto>();

            foreach (var item in parametreler)
            {
                if (item.Modul == "EMail")
                {
                    parametreListesi.Add(item);
                }
            }

            ViewBag.Parametreler = parametreListesi;

            return View();
        }

        private List<ParametreDto> Parametreler()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametreler";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            var entities = new List<ParametreDto>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ParametreDto entity = new ParametreDto();
                entity.ID = Convert.ToString(dt.Rows[i]["ID"]);
                entity.Deger = Convert.ToString(dt.Rows[i]["Deger"]);
                entity.Isim = Convert.ToString(dt.Rows[i]["Isim"]);
                entity.Modul = Convert.ToString(dt.Rows[i]["Modul"]);
                entities.Add(entity);
            }

            return entities;
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
    }
}