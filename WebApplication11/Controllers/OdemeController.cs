// Controllers/HomeController.cs
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.Dto;

public class OdemeController : Controller
{
    private string connectionString = "YourConnectionStringHere";

    // Üyelik Paketlerini Getirme
    public ActionResult UyelikPaketleri()
    {
        var paketler = new List<POSAPIDto>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT * FROM UyelikPaketleri", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        paketler.Add(new POSAPIDto
                        {
                            UyelikID = reader.GetInt32(0),
                            PaketAdi = reader.GetString(1),
                            Tutar = reader.GetDecimal(2),
                            UzatilacakAy = reader.GetInt32(3)
                        });
                    }
                }
            }
        }

        return View(paketler);
    }

    // Ödeme Sayfası
    [HttpGet]
    public ActionResult Odeme(int id)
    {
        var paket = new UyelikPaketleriViewModel();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT * FROM UyelikPaketleri WHERE UyelikID = @UyelikID", connection))
            {
                command.Parameters.AddWithValue("@UyelikID", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        paket = new POSAPIDto
                        {
                            UyelikID = reader.GetInt32(0),
                            PaketAdi = reader.GetString(1),
                            Tutar = reader.GetDecimal(2),
                            UzatilacakAy = reader.GetInt32(3)
                        };
                    }
                }
            }
        }

        return View(paket);
    }

    // Ödeme İşlemi
    [HttpPost]
    public ActionResult Odeme(POSAPIDto model)
    {
        // Benzersiz bir OrderID oluştur
        string orderID = Guid.NewGuid().ToString();

        // Ödeme oluşturma prosedürünü çağır
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("exec p_UyelikOdemesiOlustur @UyelikID, @KullaniciID, @Uygulama, @Tutar, @UzatilacakAy, @OrderID, @KrediKartIsim, @KrediKartNo, @KrediKartSonKullanim, @KrediKartCVV", connection))
            {
                command.Parameters.AddWithValue("@UyelikID", model.UyelikID);
                command.Parameters.AddWithValue("@KullaniciID", model.KullaniciID);
                command.Parameters.AddWithValue("@Uygulama", "PARAMPOS");
                command.Parameters.AddWithValue("@Tutar", GetPaketTutar(model.UyelikID)); // Paket tutarını al
                command.Parameters.AddWithValue("@UzatilacakAy", GetPaketAy(model.UyelikID)); // Paket süresini al
                command.Parameters.AddWithValue("@OrderID", orderID);
                command.Parameters.AddWithValue("@KrediKartIsim", model.KrediKartIsim);
                command.Parameters.AddWithValue("@KrediKartNo", model.KrediKartNo);
                command.Parameters.AddWithValue("@KrediKartSonKullanim", model.KrediKartSonKullanim);
                command.Parameters.AddWithValue("@KrediKartCVV", model.KrediKartCVV);

                command.ExecuteNonQuery();
            }
        }

        // PARAMPOS API çağrısını yapın (HttpClient ile)
        // Örnek bir çağrı yapılabilir, gerçek API entegrasyonunu PARAMPOS dokümantasyonuna göre yapın

        // Ödeme tamamlanma prosedürünü çağır
        // Bu örnekte ödeme başarılı olarak kabul edilmektedir
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("exec p_UyelikOdemesiTamamla @UyelikID, @KullaniciID, @Uygulama, @OrderID, @Durumu, @SonucKodu, @SonucAciklama", connection))
            {
                command.Parameters.AddWithValue("@UyelikID", model.UyelikID);
                command.Parameters.AddWithValue("@KullaniciID", model.KullaniciID);
                command.Parameters.AddWithValue("@Uygulama", "PARAMPOS");
                command.Parameters.AddWithValue("@OrderID", orderID);
                command.Parameters.AddWithValue("@Durumu", "Başarılı"); // Bu kısım PARAMPOS'tan gelen yanıta göre değiştirilmeli
                command.Parameters.AddWithValue("@SonucKodu", "00"); // Bu kısım PARAMPOS'tan gelen yanıta göre değiştirilmeli
                command.Parameters.AddWithValue("@SonucAciklama", "Ödeme başarılı");

                command.ExecuteNonQuery();
            }
        }

        return RedirectToAction("OdemeSonucu");
    }

    // Ödeme Sonucu Sayfası
    public ActionResult OdemeSonucu()
    {
        return View();
    }

    private decimal GetPaketTutar(int uyelikID)
    {
        // Üyelik paketinin tutarını almak için veritabanı sorgusu
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT Tutar FROM UyelikPaketleri WHERE UyelikID = @UyelikID", connection))
            {
                command.Parameters.AddWithValue("@UyelikID", uyelikID);
                return (decimal)command.ExecuteScalar();
            }
        }
    }

    private int GetPaketAy(int uyelikID)
    {
        // Üyelik paketinin süresini almak için veritabanı sorgusu
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT UzatilacakAy FROM UyelikPaketleri WHERE UyelikID = @UyelikID", connection))
            {
                command.Parameters.AddWithValue("@UyelikID", uyelikID);
                return (int)command.ExecuteScalar();
            }
        }
    }
}
