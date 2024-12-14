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
using YKPortal.Extensions;

namespace YKPortal.Areas.E.Controllers
{
    public class ETicaretBilgilerController : BaseController
    {
        public ActionResult GenelBilgiler()
        {
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
                    if (grupKoduDto.Kod == "ETicaretLogo" && grupKoduDto.Deger.Length > 0)
                    {

                        var dosyaUzantisi = grupKoduDto.Deger.Base64DosyaUzantisiniGetir();
                        if (dosyaUzantisi == string.Empty)
                        {
                            continue;
                        }

                        var dosyaAdi = Guid.NewGuid().ToString();

                        var dosyaBase64Data = grupKoduDto.Deger.Contains(",") ? grupKoduDto.Deger.Split(',')[1] : grupKoduDto.Deger;

                        var dosyaByteDegeri = Convert.FromBase64String(dosyaBase64Data);

                        if (!Directory.Exists(Server.MapPath("/Uploads/ETicaret")))
                        {
                            Directory.CreateDirectory(Server.MapPath("/Uploads/ETicaret"));
                        }

                        System.IO.File.WriteAllBytes(Server.MapPath("/Uploads/ETicaret/" + dosyaAdi + dosyaUzantisi), dosyaByteDegeri);
                        cmd.Parameters.AddWithValue("@Deger", dosyaAdi + dosyaUzantisi);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Deger", grupKoduDto.Deger);
                    }
                    
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


        
    }
}