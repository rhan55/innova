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
    public class ETicaretBilgilerController : BaseController
    {
        public ActionResult GenelBilgiler()
        {
            var ETicaretTelefon = GrupKodListesiniGetir("ETicaretTelefon");
            var ETicaretEmail = GrupKodListesiniGetir("ETicaretEmail");
            var ETicaretLogo = GrupKodListesiniGetir("ETicaretLogo");
            var ETicaretFirmaAdi = GrupKodListesiniGetir("ETicaretFirmaAdi");

            ViewBag.ETicaretTelefon = ETicaretTelefon;
            ViewBag.ETicaretEmail = ETicaretEmail;
            ViewBag.ETicaretLogo = ETicaretLogo;
            ViewBag.ETicaretFirmaAdi = ETicaretFirmaAdi;

            return View();
        }
        [HttpPost]
        public JsonResult Kaydet(List<GrupKoduDto> grupKodlari)
        {
            try
            {
                foreach (var grupKoduDto in grupKodlari)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "p_GrupKoduKaydet";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(grupKoduDto.ID))
                    {
                        cmd.Parameters.AddWithValue("@ID", grupKoduDto.ID); // ID dolu ise düzenleme
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ID", ""); // ID yoksa yeni kayıt
                    }
     
                    cmd.Parameters.AddWithValue("@KullaniciID", Kullanici.ID);
                    cmd.Parameters.AddWithValue("@Kod", grupKoduDto.Kod);
                    cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikIDGetir());


                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                }

                return Json(new
                {
                    success = true,
                    message = "Grup kodları başarıyla kaydedildi.",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Bir hata oluştu: {ex.Message}"
                });
            }
        }


        private List<GrupKoduDto> GrupKodListesiniGetir(string kodAdi)
        {
            // GrupKodu1 Listesi oluşturma 
            SqlCommand grupKodCommand = new SqlCommand();
            grupKodCommand.CommandText = "p_GrupKoduListesi";
            grupKodCommand.CommandType = System.Data.CommandType.StoredProcedure;

            grupKodCommand.Parameters.AddWithValue("@Kod", kodAdi);
            grupKodCommand.Parameters.AddWithValue("@UyelikID", UyelikIDGetir());

            grupKodCommand.Parameters.AddWithValue("@AranacakKelime", "");


            DataTable grupKoduTable = (DataTable)IDVeritabani.Sorgula(grupKodCommand, SorgulaTuru.Tablo);

            List<GrupKoduDto> entities = new List<GrupKoduDto>();

            for (int i = 0; i < grupKoduTable.Rows.Count; i++)
            {
                GrupKoduDto entity = new GrupKoduDto();
                entity.ID = Convert.ToString(grupKoduTable.Rows[i]["ID"]);
                entity.Kod = Convert.ToString(grupKoduTable.Rows[i]["Kod"]);
                entity.Deger = Convert.ToString(grupKoduTable.Rows[i]["Deger"]);
                entity.Aktif = Convert.ToBoolean(grupKoduTable.Rows[i]["Aktif"]);
                entities.Add(entity);
            }

            return entities;
        }
    }
}