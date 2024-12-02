using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models.Dto;
using YKPortal.Models;
using System.IO;
using System.Net;
using System.Text;

namespace YKPortal.Areas.E.Controllers
{
    public class BilgilerController : BaseController
    {
        // GET: E/Bilgiler
        [HttpGet]
        public ActionResult GenelBilgiler()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenelBilgiler(GrupKoduDto grupKoduDto)
        {
            if (!AutoGirisKontrol())
                return Redirect("E/Yetkilendirme/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_GrupKoduKaydet";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Eğer ID varsa düzenleme, yoksa ekleme
            if (!string.IsNullOrEmpty(grupKoduDto.ID))
            {
                cmd.Parameters.AddWithValue("@ID", grupKoduDto.ID); // ID dolu ise düzenleme
            }
            else
            {
                cmd.Parameters.AddWithValue("@ID", DBNull.Value); // ID yoksa yeni kayıt
            }

            cmd.Parameters.AddWithValue("@UyelikID", GetCookie("UyelikID"));
            cmd.Parameters.AddWithValue("@KullaniciID", GetCookie("KullaniciID"));
            cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
            cmd.Parameters.AddWithValue("@Aktif", grupKoduDto.Aktif);
            cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);

            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return RedirectToAction("GrupKodu", new { grupKodu = grupKoduDto.Kod });
        }
    }
}