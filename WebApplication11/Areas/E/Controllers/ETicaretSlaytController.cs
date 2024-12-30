using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Areas.E.Models.Dto;
using YKPortal.Models;

namespace YKPortal.Areas.E.Controllers
{
    public class ETicaretSlaytController : BaseController
    {
        // GET: E/ETicaretSlayt
   
        public ActionResult Slaytlar(string ID)
        {
           SqlCommand cmd = new SqlCommand("SELECT * FROM ETicaret_Slaytlar WHERE SlaytID = @SlaytID AND Durum = 1");
            cmd.Parameters.AddWithValue("@SlaytID", ID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (dt.Rows.Count == 0)
            {
                return RedirectToAction("SayfaBulunamadi");
            }

            var entitiy = new ETicaretSlaytDto();

            entitiy.SlaytID = Convert.ToString(dt.Rows[0]["SlaytID"]);
            entitiy.ResimYolu = Convert.ToString(dt.Rows[0]["ResimYolu"]);
            entitiy.Link = Convert.ToString(dt.Rows[0]["Link"]);
            entitiy.Text = Convert.ToString(dt.Rows[0]["Text"]);
            entitiy.Aktif = Convert.ToBoolean(dt.Rows[0]["Aktif"]);

            return View(entitiy);
        }

        [HttpGet]
        public ActionResult SlaytEkle(string SlaytID)
        {
            var entity = new ETicaretSlaytDto();
            if (string.IsNullOrWhiteSpace(SlaytID))
            {
                return View(entity);
            }
            entity = SlaytGetir(SlaytID);
            return View(entity);
        }

        [HttpPost]
        public ActionResult SlaytEkle(ETicaretSlaytDto eTicaretSlaytDto)
        {
            SqlCommand cmd;
            cmd = new SqlCommand("UPDATE  ETicaret_Slaytlar SET ResimYolu = @ResimYolu, Link = @Link, Text = @Text, Aktif = @Aktif, OluşturulmaTarihi = @OluşturulmaTarihi WHERE SlaytID = @SlaytID");
            cmd.Parameters.AddWithValue("@ResimYolu",(eTicaretSlaytDto.ResimYolu));
            cmd.Parameters.AddWithValue("@SlaytID", eTicaretSlaytDto.SlaytID);
            cmd.Parameters.AddWithValue("@ResimYolu", eTicaretSlaytDto.ResimYolu);
            cmd.Parameters.AddWithValue("@Link", eTicaretSlaytDto.Link);
            cmd.Parameters.AddWithValue("@Text", eTicaretSlaytDto.Text);          
            cmd.Parameters.AddWithValue("@Aktif", true);
            cmd.Parameters.AddWithValue("@OluşturulmaTarihi", DateTime.Now);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View();
        }

    }
}