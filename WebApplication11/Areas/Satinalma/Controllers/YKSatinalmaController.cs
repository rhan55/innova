using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.Satinalma.Controllers
{
    public class YKSatinalmaController : Controller
    {


        public ActionResult PdfGoster(string BelgeNo)
        {
            string Dosya = "";
            if (BelgeNo.Trim().Length <= 15)
            {
                foreach (string item in Directory.GetFiles(Server.MapPath("~/Pdf")))
                {

                    if (item.Contains(BelgeNo))
                    {
                        Dosya = item;
                        break;
                    }
                }
            }
            else
            {
                Dosya = BelgeNo;
            }

            return Redirect("~/Pdf/" + Dosya.Replace("C:\\inetpub\\wwwroot\\Satinalma\\Pdf\\", ""));
        }


        public void SiparisKontroluYap()
        {
            string KullaniciAdi = GetCookie("KullaniciAdi");
            SiparisKontroluYap(KullaniciAdi);
        }



        public JsonResult TeklifDosyasiSil(int KayitID)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                int SonID = KayitID;
                {

                    SqlCommand cmdDosya = new SqlCommand();
                    cmdDosya.CommandText = "Delete From Dosyalar Where ID = @KayitID ";
                    cmdDosya.Parameters.AddWithValue("@KayitID", KayitID);
                    IDVeritabani.Sorgula(cmdDosya, SorgulaTuru.Bos);
                }
                result.SonucKodu = "1";
                //result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TeklifDosyasiKaydet(int KayitID, HttpPostedFileBase Dosya = null)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                int SonID = KayitID;
                if (Dosya != null)
                {
                    string dosyaYolu = Path.GetFileName(Dosya.FileName);
                    string yeniIsmi = Guid.NewGuid().ToString();
                    string uzanti = Path.GetExtension(Dosya.FileName);
                    var yuklemeYeri = Path.Combine(Server.MapPath("~/Uploads/"), yeniIsmi + uzanti);
                    Dosya.SaveAs(yuklemeYeri);

                    SqlCommand cmdDosya = new SqlCommand();
                    cmdDosya.CommandText = "Insert Into Dosyalar (Modul,Dosya,Isim,DosyaUzantisi,KayitTarihi,KayitYapanKullanici,KayitID) values (@Modul,@Dosya,@Isim,@DosyaUzantisi,@KayitTarihi,@KayitYapanKullanici,@KayitID) ";
                    cmdDosya.Parameters.AddWithValue("@Modul", "Talep");
                    cmdDosya.Parameters.AddWithValue("@Dosya", yeniIsmi + uzanti);
                    cmdDosya.Parameters.AddWithValue("@Isim", Dosya.FileName);
                    cmdDosya.Parameters.AddWithValue("@DosyaUzantisi", uzanti);
                    cmdDosya.Parameters.AddWithValue("@KayitTarihi", DateTime.Now);
                    cmdDosya.Parameters.AddWithValue("@KayitYapanKullanici", KullaniciAdi);
                    cmdDosya.Parameters.AddWithValue("@KayitID", SonID);
                    IDVeritabani.Sorgula(cmdDosya, SorgulaTuru.Bos);
                }
                result.SonucKodu = "1";
                //result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Raporlar











        public void TalepRaporuExcel(string Sube, DateTime? Tarih1, DateTime? Tarih2, string Kategori1, string TamamlananlariGoster)
        {

            if (Tarih1 == null)
            {
                Tarih1 = DateTime.Today.AddDays(-30);
            }
            if (Tarih2 == null)
            {
                Tarih2 = DateTime.Today.AddDays(1);
            }
            ViewBag.Sube = Sube;
            ViewBag.Tarih1 = Tarih1;
            ViewBag.Tarih2 = Tarih2;
            ViewBag.Kategori1 = Kategori1;
            ViewBag.TamamlananlariGoster = TamamlananlariGoster;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_TalepRaporuExcel";
            cmd.Parameters.AddWithValue("@KullaniciAdi", GetCookie("KullaniciAdi"));
            cmd.Parameters.AddWithValue("@Sube", Sube);
            cmd.Parameters.AddWithValue("@Tarih1", Tarih1);
            cmd.Parameters.AddWithValue("@Tarih2", Tarih2);
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@TamamlananlariGoster", TamamlananlariGoster);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            ExporttoExcel(dtKayitlar);
        }

        public ActionResult TalepRaporu(string Sube, DateTime? Tarih1, DateTime? Tarih2, string Kategori1, string TamamlananlariGoster)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");


            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_KullaniciSubeYetkileri";
            cmd.Parameters.AddWithValue("@ID", KullaniciID);
            ViewBag.dtSubeler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = @"select Kategori1 from w_Stoklar 
--Where KategoriKodu1 collate SQL_Latin1_General_CP1_CI_AS IN (
--	select MenuID from YetkilerKategori where Kullanici = @KullaniciAdi and Yetki = 1
--) 
Group by KategoriKodu1,Kategori1 order by KategoriKodu1";
            cmd.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
            ViewBag.dtKategori1 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            if (Tarih1 == null)
            {
                Tarih1 = DateTime.Today.AddDays(-30);
            }
            if (Tarih2 == null)
            {
                Tarih2 = DateTime.Today.AddDays(1);
            }
            ViewBag.Tarih1 = Tarih1;
            ViewBag.Tarih2 = Tarih2;
            ViewBag.Sube = Sube;
            ViewBag.Kategori1 = Kategori1;
            ViewBag.TamamlananlariGoster = TamamlananlariGoster;

            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_TalepRaporu";
            cmd.Parameters.AddWithValue("@KullaniciAdi", GetCookie("KullaniciAdi"));
            cmd.Parameters.AddWithValue("@Sube", Sube);
            cmd.Parameters.AddWithValue("@Tarih1", Tarih1);
            cmd.Parameters.AddWithValue("@Tarih2", Tarih2);
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@TamamlananlariGoster", TamamlananlariGoster);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);




            return View(dtKayitlar);
        }


        public bool Gonder(string konu, string icerik, string yol, string KullaniciAdi)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress(ConfigurationManager.ConnectionStrings["MailKullaniciAdi"].ConnectionString, "Satınalma - Yeni Sipariş Formu");

            ePosta.To.Add("purchasemng@sherwoodhotels.com.tr");
            ePosta.To.Add("purchase1@sherwoodhotels.com.tr");
            ePosta.To.Add("purchase2.head @ceylanholding.net");
            ePosta.To.Add("purchase3.head @ceylanholding.net");
            ePosta.To.Add("purchase4@sherwoodhotels.com.tr");
            ePosta.To.Add("purchase5.head@sherwoodhotels.com.tr");
            ePosta.Bcc.Add("mail.yunuskose@gmail.com");
            ePosta.Bcc.Add("budget1.head@ceylanholding.net");
            ePosta.Bcc.Add("erayerdinc@sherwoodhotels.com.tr");

            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = @"SELECT Mail FROM ID_Satinalma_Kullanicilar(NOLOCK) WHERE(KullaniciAdi = '" + KullaniciAdi + "')";
            //string mailAdresi = Convert.ToString(ID.Sorgula(cmd, SorgulaTuru.Tek));
            //if (mailAdresi.Trim().Length > 0)
            //{
            //    if (mailAdresi != "budget1.head@ceylanholding.net")
            //        ePosta.To.Add(mailAdresi);
            //}
            try
            {
                ePosta.Attachments.Add(new Attachment(yol));
            }
            catch
            {
                ;
            }
            //
            ePosta.Subject = konu;
            //
            ePosta.Body = icerik;
            //

            SmtpClient smtp = new SmtpClient();
            //
            smtp.Credentials = new System.Net.NetworkCredential(
                ConfigurationManager.ConnectionStrings["MailKullaniciAdi"].ConnectionString,
                ConfigurationManager.ConnectionStrings["MailParola"].ConnectionString);

            smtp.Port = Convert.ToInt32(ConfigurationManager.ConnectionStrings["MailPort"].ConnectionString);
            smtp.Host = ConfigurationManager.ConnectionStrings["MailServer"].ConnectionString;
            smtp.EnableSsl = true;
            object userState = ePosta;
            bool kontrol = true;
            try
            {
                smtp.Send(ePosta);//, (object)ePosta);
            }
            catch (SmtpException ex)
            {
                kontrol = false;
            }
            return kontrol;
        }

        void SiparisKontroluYap(string KullaniciAdi)
        {
            {
                TekrarDene:
                SqlCommand cmdSip = new SqlCommand();
                cmdSip.CommandType = System.Data.CommandType.StoredProcedure;
                cmdSip.CommandText = "p_SiparisKaydetERP";
                //cmdSip.Parameters.AddWithValue("@ID", 4318); //Silinecek
                DataTable dtSiparis = (DataTable)IDVeritabani.Sorgula(cmdSip, SorgulaTuru.Tablo);
                while (dtSiparis.Rows.Count > 0)
                {
                    try
                    {
                        string DosyaAdi = dtSiparis.Rows[0]["SiparisNo"] + "-" + Guid.NewGuid() + ".pdf";
                        //string yol = IDDizayn.DizaynIslemleri.DizaynKaydetSiparis("",
                        //    dtSiparis,
                        //    "", ConfigurationManager.AppSettings["KaydedilecekKlasor"] + DosyaAdi);
                        string yol = ""; //Yukarda olduğu gibi dizayn yapılacak devexpress dll eklenecek.
                        string yol2 = Server.MapPath("~/Pdf/" + DosyaAdi);

                        Gonder(
                            dtSiparis.Rows[0]["SubeAdi"] + " | Firma:" + dtSiparis.Rows[0]["CariAdi"] + " | Sipariş Formu : " + dtSiparis.Rows[0]["SiparisNo"],
                            dtSiparis.Rows[0]["SiparisNo"] + " nolu sipariş için detay bilgisi ektedir.\n\nCari İsmi : " + dtSiparis.Rows[0]["CariAdi"] + "\n\n\nTarih:" + DateTime.Now,
                            yol2,
                            KullaniciAdi
                            );

                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Update SatinalmaTalepleri set PdfDosyaAdi = @DosyaAdi Where ISNULL(AktarimNo,'') = @SiparisNo ";
                        cmd.Parameters.AddWithValue("@DosyaAdi", DosyaAdi);
                        cmd.Parameters.AddWithValue("@SiparisNo", dtSiparis.Rows[0]["SiparisNo"]);
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                    catch (Exception err)
                    {
                    }
                    goto TekrarDene;
                }
            }
        }
        #endregion

        #region Sözleşmeler

        public ActionResult CariStokSozlesmesiSil(int id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");
            SqlCommand cmd = new SqlCommand();

            {
                //Delete
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Delete from SatinalmaCariStokSozlesmeleri  Where ID = @ID";
                cmd.Parameters.AddWithValue("@ID", id);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }


            return Redirect("~/Satinalma/YKSatinalma/CariSozlesmeleri");
        }

        public ActionResult CariStokSozlesmesiKaydet(int id, DateTime Baslangic, DateTime Bitis,
            string CariID, string StokID, decimal Miktar, string Aciklama)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");
            SqlCommand cmd = new SqlCommand();
            if (id == 0)
            {
                //İnsert
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Insert Into SatinalmaCariStokSozlesmeleri (CariID,Baslangic,Bitis,StokID,Miktar,Aciklama,CDate,Kullanici) values (@CariID,@Baslangic,@Bitis,@StokID,@Miktar,@Aciklama,GETDATE(),@Kullanici)";
                cmd.Parameters.AddWithValue("@CariID", CariID);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@StokID", StokID);
                cmd.Parameters.AddWithValue("@Miktar", Miktar);
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
            }
            else
            {
                //Update
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Update SatinalmaCariStokSozlesmeleri Set CariID=@CariID,Baslangic=@Baslangic,Bitis=@Bitis,StokID=@StokID,Miktar=@Miktar,Aciklama=@Aciklama,Kullanici=@Kullanici  Where ID = @ID";
                cmd.Parameters.AddWithValue("@CariID", CariID);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@StokID", StokID);
                cmd.Parameters.AddWithValue("@Miktar", Miktar);
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                cmd.Parameters.AddWithValue("@ID", id);
            }

            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/Satinalma/YKSatinalma/CariSozlesmeleri");
        }

        public ActionResult YeniCariSozlesme(int KayitID = 0)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "p_CariStokSozlesmesi";
            cmd.Parameters.AddWithValue("@KayitID", KayitID);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);



            return View(dtKayitlar);
        }

        public ActionResult CariSozlesmeleri()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "p_CariStokSozlesmeleri";
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);



            return View(dtKayitlar);
        }

        public ActionResult CariStokSozlesmesiSilTutar(int id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");
            SqlCommand cmd = new SqlCommand();

            {
                //Delete
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Delete from SatinalmaCariTutarSozlesmeleri  Where ID = @ID";
                cmd.Parameters.AddWithValue("@ID", id);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }


            return Redirect("~/Satinalma/YKSatinalma/CariSozlesmeleriTutar");
        }

        public ActionResult CariSozlesmesiKaydetTutar(int id, DateTime Baslangic, DateTime Bitis,
            string CariID, decimal Tutar, string Aciklama)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");
            SqlCommand cmd = new SqlCommand();
            if (id == 0)
            {
                //İnsert
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Insert Into SatinalmaCariTutarSozlesmeleri (CariID,Baslangic,Bitis,Tutar,Aciklama,CDate,Kullanici) values (@CariID,@Baslangic,@Bitis,@Tutar,@Aciklama,GETDATE(),@Kullanici)";
                cmd.Parameters.AddWithValue("@CariID", CariID);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@Tutar", Tutar);
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
            }
            else
            {
                //Update
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Update SatinalmaCariTutarSozlesmeleri Set CariID=@CariID,Baslangic=@Baslangic,Bitis=@Bitis,Tutar=@Tutar,Aciklama=@Aciklama,Kullanici=@Kullanici  Where ID = @ID";
                cmd.Parameters.AddWithValue("@CariID", CariID);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@Tutar", Tutar);
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                cmd.Parameters.AddWithValue("@ID", id);
            }

            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/Satinalma/YKSatinalma/CariSozlesmeleriTutar");
        }

        public ActionResult YeniCariSozlesmeTutar(int KayitID = 0)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "p_CariStokSozlesmesiTutar";
            cmd.Parameters.AddWithValue("@KayitID", KayitID);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);



            return View(dtKayitlar);
        }

        public ActionResult CariSozlesmeleriTutar()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "p_CariStokSozlesmeleriTutar";
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);



            return View(dtKayitlar);
        }

        #endregion
        #region Onay 5

        [HttpPost]
        public JsonResult TalepOnayla5(int Grupla, string stokkodu)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalepOnayla5";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                //SiparisKontroluYap(KullaniciAdi);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderOnay5(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderOnay5";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Talep'e Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderSatinalmaya5(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderSatinalmaya5";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Satınalma'ya Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SatinalmaTalepDedayiOnay5(string StokKodu = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTalepDetayiOnay5";
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.ID = Convert.ToString(satir["ID"]);
                    entity.SubeKodu = Convert.ToString(satir["SubeKodu"]);
                    entity.Kod = Convert.ToString(satir["StokKodu"]);
                    entity.Isim = Convert.ToString(satir["StokAdi"]);
                    entity.Miktar = Convert.ToDecimal(satir["TalepMiktari"]);
                    entity.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    entity.Aciklama1 = Convert.ToString(satir["Aciklama1"]);
                    entity.Aciklama2 = Convert.ToString(satir["Aciklama2"]);
                    entity.Aciklama3 = Convert.ToString(satir["Aciklama3"]);
                    entity.Tarih = Convert.ToString(satir["KayitTarihi"]);
                    entity.Kullanici = Convert.ToString(satir["KayitYapanKullanici"]);

                    entity.SonAlisCarisi = Convert.ToString(satir["SonAlisCarisi"]);
                    if (satir["SonAlisTarihi"] != DBNull.Value)
                        entity.SonAlisTarihi = Convert.ToDateTime(satir["SonAlisTarihi"]).ToString("dd-MM-yyyy");

                    entity.SonAlisSubesi = Convert.ToString(satir["SonAlisSubesi"]);
                    if (satir["SonAlisFiyati"] != DBNull.Value)
                        entity.SonAlisFiyati = String.Format("Fiyat : {0:N2}", Convert.ToDecimal(satir["SonAlisFiyati"]));
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Onay5(string Kategori1 = "", string Kategori2 = "", string Kategori3 = "", string Grupla = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (Grupla == null)
                Grupla = "0";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_SatinalmaKayitListesiOnay5";
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", Kategori3);
            cmd.Parameters.AddWithValue("@Grupla", Grupla);
            cmd.Parameters.AddWithValue("@Kullanici", "");
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            ViewBag.dtKategori1 = ds.Tables[3];
            ViewBag.Kategori1 = Kategori1;
            ViewBag.Kategori2 = Kategori2;
            ViewBag.Kategori3 = Kategori3;
            ViewBag.Grupla = Grupla;

            return View(ds);
        }

        #endregion




        #region Onay 4

        [HttpPost]
        public JsonResult TalepOnayla4(int Grupla, string stokkodu)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalepOnayla4";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                //SiparisKontroluYap(KullaniciAdi);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderOnay4(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderOnay4";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Talep'e Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderSatinalmaya4(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderSatinalmaya4";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Satınalma'ya Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SatinalmaTalepDedayiOnay4(string StokKodu = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTalepDetayiOnay4";
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.ID = Convert.ToString(satir["ID"]);
                    entity.SubeKodu = Convert.ToString(satir["SubeKodu"]);
                    entity.Kod = Convert.ToString(satir["StokKodu"]);
                    entity.Isim = Convert.ToString(satir["StokAdi"]);
                    entity.Miktar = Convert.ToDecimal(satir["TalepMiktari"]);
                    entity.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    entity.Aciklama1 = Convert.ToString(satir["Aciklama1"]);
                    entity.Aciklama2 = Convert.ToString(satir["Aciklama2"]);
                    entity.Aciklama3 = Convert.ToString(satir["Aciklama3"]);
                    entity.Tarih = Convert.ToString(satir["KayitTarihi"]);
                    entity.Kullanici = Convert.ToString(satir["KayitYapanKullanici"]);

                    entity.SonAlisCarisi = Convert.ToString(satir["SonAlisCarisi"]);
                    if (satir["SonAlisTarihi"] != DBNull.Value)
                        entity.SonAlisTarihi = Convert.ToDateTime(satir["SonAlisTarihi"]).ToString("dd-MM-yyyy");

                    entity.SonAlisSubesi = Convert.ToString(satir["SonAlisSubesi"]);
                    if (satir["SonAlisFiyati"] != DBNull.Value)
                        entity.SonAlisFiyati = String.Format("Fiyat : {0:N2}", Convert.ToDecimal(satir["SonAlisFiyati"]));
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Onay4(string Kategori1 = "", string Kategori2 = "", string Kategori3 = "", string Grupla = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (Grupla == null)
                Grupla = "0";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_SatinalmaKayitListesiOnay4";
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", Kategori3);
            cmd.Parameters.AddWithValue("@Grupla", Grupla);
            cmd.Parameters.AddWithValue("@Kullanici", "");
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            ViewBag.dtKategori1 = ds.Tables[3];
            ViewBag.Kategori1 = Kategori1;
            ViewBag.Kategori2 = Kategori2;
            ViewBag.Kategori3 = Kategori3;
            ViewBag.Grupla = Grupla;

            return View(ds);
        }

        #endregion

        #region Onay 3

        [HttpPost]
        public JsonResult TalepOnayla3(int Grupla, string stokkodu)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalepOnayla3";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                //SiparisKontroluYap(KullaniciAdi);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderOnay3(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderOnay3";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Talep'e Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderSatinalmaya3(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderSatinalmaya3";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Satınalma'ya Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SatinalmaTalepDedayiOnay3(string StokKodu = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTalepDetayiOnay3";
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.ID = Convert.ToString(satir["ID"]);
                    entity.SubeKodu = Convert.ToString(satir["SubeKodu"]);
                    entity.Kod = Convert.ToString(satir["StokKodu"]);
                    entity.Isim = Convert.ToString(satir["StokAdi"]);
                    entity.Miktar = Convert.ToDecimal(satir["TalepMiktari"]);
                    entity.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    entity.Aciklama1 = Convert.ToString(satir["Aciklama1"]);
                    entity.Aciklama2 = Convert.ToString(satir["Aciklama2"]);
                    entity.Aciklama3 = Convert.ToString(satir["Aciklama3"]);
                    entity.Tarih = Convert.ToString(satir["KayitTarihi"]);
                    entity.Kullanici = Convert.ToString(satir["KayitYapanKullanici"]);

                    entity.SonAlisCarisi = Convert.ToString(satir["SonAlisCarisi"]);
                    if (satir["SonAlisTarihi"] != DBNull.Value)
                        entity.SonAlisTarihi = Convert.ToDateTime(satir["SonAlisTarihi"]).ToString("dd-MM-yyyy");

                    entity.SonAlisSubesi = Convert.ToString(satir["SonAlisSubesi"]);
                    if (satir["SonAlisFiyati"] != DBNull.Value)
                        entity.SonAlisFiyati = String.Format("Fiyat : {0:N2}", Convert.ToDecimal(satir["SonAlisFiyati"]));
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Onay3(string Kategori1 = "", string Kategori2 = "", string Kategori3 = "", string Grupla = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (Grupla == null)
                Grupla = "0";

            string KullaniciAdi = GetCookie("KullaniciAdi");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_SatinalmaKayitListesiOnay3";
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", Kategori3);
            cmd.Parameters.AddWithValue("@Grupla", Grupla);
            cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            ViewBag.dtKategori1 = ds.Tables[3];
            ViewBag.Kategori1 = Kategori1;
            ViewBag.Kategori2 = Kategori2;
            ViewBag.Kategori3 = Kategori3;
            ViewBag.Grupla = Grupla;

            return View(ds);
        }

        #endregion

        #region Onay 2


        public ActionResult SatinalmaStokKoduGuncelle(int id, string StokKodu)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.CommandText = "Select * from w_Stoklar Where StokKodu = @StokKodu";
                cmd2.Parameters.AddWithValue("@StokKodu", StokKodu);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd2, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {

                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "Update SatinalmaTalepleri set StokID = @StokID,StokKodu=@StokKodu,StokAdi=@StokAdi Where ID = @ID";
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@StokID", dt.Rows[0]["ID"]);
                        cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                        cmd.Parameters.AddWithValue("@StokAdi", dt.Rows[0]["StokAdi"]);
                        IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                    }
                }
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SatinalmaMiktarGuncelle(int id, decimal miktar)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "Update SatinalmaTalepleri set TalepMiktari = @Miktar Where ID = @ID";
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Miktar", miktar);
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalepOnayla2(int Grupla, string stokkodu)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalepOnayla2";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                //SiparisKontroluYap(KullaniciAdi);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderOnay2(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderOnay2";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Talep'e Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderSatinalmaya2(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderSatinalmaya2";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Satınalma'ya Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SatinalmaTalepDedayiOnay2(string StokKodu = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTalepDetayiOnay2";
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.ID = Convert.ToString(satir["ID"]);
                    entity.SubeKodu = Convert.ToString(satir["SubeKodu"]);
                    entity.Kod = Convert.ToString(satir["StokKodu"]);
                    entity.Isim = Convert.ToString(satir["StokAdi"]);
                    entity.Miktar = Convert.ToDecimal(satir["TalepMiktari"]);
                    entity.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    entity.Aciklama1 = Convert.ToString(satir["Aciklama1"]);
                    entity.Aciklama2 = Convert.ToString(satir["Aciklama2"]);
                    entity.Aciklama3 = Convert.ToString(satir["Aciklama3"]);
                    entity.Tarih = Convert.ToString(satir["KayitTarihi"]);
                    entity.Kullanici = Convert.ToString(satir["KayitYapanKullanici"]);

                    entity.SonAlisCarisi = Convert.ToString(satir["SonAlisCarisi"]);
                    if (satir["SonAlisTarihi"] != DBNull.Value)
                        entity.SonAlisTarihi = Convert.ToDateTime(satir["SonAlisTarihi"]).ToString("dd-MM-yyyy");

                    entity.SonAlisSubesi = Convert.ToString(satir["SonAlisSubesi"]);
                    if (satir["SonAlisFiyati"] != DBNull.Value)
                        entity.SonAlisFiyati = String.Format("Fiyat : {0:N2}", Convert.ToDecimal(satir["SonAlisFiyati"]));

                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Onay2(string Kategori1 = "", string Kategori2 = "", string Kategori3 = "", string Grupla = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (Grupla == null)
                Grupla = "0";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_SatinalmaKayitListesiOnay2";
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", Kategori3);
            cmd.Parameters.AddWithValue("@Grupla", Grupla);
            cmd.Parameters.AddWithValue("@Kullanici", "");
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            ViewBag.dtKategori1 = ds.Tables[3];
            ViewBag.Kategori1 = Kategori1;
            ViewBag.Kategori2 = Kategori2;
            ViewBag.Kategori3 = Kategori3;
            ViewBag.Grupla = Grupla;

            return View(ds);
        }

        #endregion

        #region Onay 1

        [HttpPost]
        public JsonResult SatinalmaFiyatKaldirSatinalma(int Grupla, int KayitID, string StokKodu, string SecilenCariID, decimal SecilenFiyat, string SecilenDovizBirimi)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTeklifKaldirSatinalma";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@KayitID", KayitID);
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                cmd.Parameters.AddWithValue("@SecilenCariID", SecilenCariID);
                cmd.Parameters.AddWithValue("@SecilenFiyat", SecilenFiyat);
                cmd.Parameters.AddWithValue("@SecilenDovizBirimi", SecilenDovizBirimi);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SatinalmaFiyatSecSatinalma(int Grupla, int KayitID, string StokKodu, string SecilenCariID, decimal SecilenFiyat, string SecilenDovizBirimi)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTeklifSecSatinalma";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@KayitID", KayitID);
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                cmd.Parameters.AddWithValue("@SecilenCariID", SecilenCariID);
                cmd.Parameters.AddWithValue("@SecilenFiyat", SecilenFiyat);
                cmd.Parameters.AddWithValue("@SecilenDovizBirimi", SecilenDovizBirimi);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult TalepOnayla1(int Grupla, string stokkodu, string BitisNoktasi, string OnaySeviyesi)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalepOnayla1";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                cmd.Parameters.AddWithValue("@BitisNoktasi", BitisNoktasi);
                cmd.Parameters.AddWithValue("@OnaySeviyesi", OnaySeviyesi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                //SiparisKontroluYap(KullaniciAdi);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderOnay1(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderOnay1";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Talep'e Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderSatinalmaya(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderSatinalmaya";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Satınalma'ya Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SatinalmaTalepDedayiOnay1(string StokKodu = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTalepDetayiOnay1";
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.ID = Convert.ToString(satir["ID"]);
                    entity.SubeKodu = Convert.ToString(satir["SubeKodu"]);
                    entity.Kod = Convert.ToString(satir["StokKodu"]);
                    entity.Isim = Convert.ToString(satir["StokAdi"]);
                    entity.Miktar = Convert.ToDecimal(satir["TalepMiktari"]);
                    entity.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    entity.Aciklama1 = Convert.ToString(satir["Aciklama1"]);
                    entity.Aciklama2 = Convert.ToString(satir["Aciklama2"]);
                    entity.Aciklama3 = Convert.ToString(satir["Aciklama3"]);
                    entity.Tarih = Convert.ToString(satir["KayitTarihi"]);
                    entity.Kullanici = Convert.ToString(satir["KayitYapanKullanici"]);

                    entity.SonAlisCarisi = Convert.ToString(satir["SonAlisCarisi"]);
                    if (satir["SonAlisTarihi"] != DBNull.Value)
                        entity.SonAlisTarihi = Convert.ToDateTime(satir["SonAlisTarihi"]).ToString("dd-MM-yyyy");

                    entity.SonAlisSubesi = Convert.ToString(satir["SonAlisSubesi"]);
                    if (satir["SonAlisFiyati"] != DBNull.Value)
                        entity.SonAlisFiyati = String.Format("Fiyat : {0:N2}", Convert.ToDecimal(satir["SonAlisFiyati"]));
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Onay1(string Kategori1 = "", string Kategori2 = "", string Kategori3 = "", string Grupla = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (Grupla == null)
                Grupla = "0";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_SatinalmaKayitListesiOnay1";
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", Kategori3);
            cmd.Parameters.AddWithValue("@Grupla", Grupla);
            cmd.Parameters.AddWithValue("@Kullanici", "");
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            ViewBag.dtKategori1 = ds.Tables[3];
            ViewBag.Kategori1 = Kategori1;
            ViewBag.Kategori2 = Kategori2;
            ViewBag.Kategori3 = Kategori3;
            ViewBag.Grupla = Grupla;

            return View(ds);
        }

        #endregion

        #region Satınalma İşlemleri


        public ActionResult SatinalmaAciklamaGuncelle(int id, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "Update SatinalmaTalepleri set Aciklama3 = @Aciklama3 Where ID = @ID";
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Aciklama3", aciklama);
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TeklifFiyatiGuncelle(string stok, string cari, decimal fiyat)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Update SatinalmaTeklifleri set Fiyat = @Fiyat Where StokID = @StokID and CariID = @CariID and CAST(GETDATE() as DATE) between Baslangic and Bitis";
            cmd.Parameters.AddWithValue("@StokID", stok);
            cmd.Parameters.AddWithValue("@CariID", cari);
            cmd.Parameters.AddWithValue("@Fiyat", fiyat);
            IDVeritabani.Sorgula(cmd, Models.SorgulaTuru.Bos);

            return Redirect("~/Satinalma/YKSatinalma/Teklifler");
        }

        public ActionResult TeklifSil(int id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Update SatinalmaTeklifleri set Silindi=1,SilinenTarih=GETDATE(),SilenKullanici=@KullaniciAdi Where ID = @ID";

            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
            IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

            return Redirect("~/Satinalma/YKSatinalma/Teklifler");
        }

        [HttpPost]
        public JsonResult TalebiGeriyeGonderID(string kayitid, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonderID";
                cmd.Parameters.AddWithValue("@KayitID", kayitid);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Talep'e Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TalebiGeriyeGonder(int Grupla, string stokkodu, string aciklama)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalebiGeriyeGonder";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama + " - " + KullaniciAdi + " - " + DateTime.Now + " - Talep'e Gönderildi");
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TalepOnayaGonder(int Grupla, string stokkodu)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalepOnayaGonder";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@StokKodu", stokkodu);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SatinalmaTalepDedayi(string StokKodu = "", string Grupla = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTalepDetayi";
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.ID = Convert.ToString(satir["ID"]);
                    entity.SubeKodu = Convert.ToString(satir["SubeKodu"]);
                    entity.Kod = Convert.ToString(satir["StokKodu"]);
                    entity.Isim = Convert.ToString(satir["StokAdi"]);
                    entity.Miktar = Convert.ToDecimal(satir["TalepMiktari"]);
                    entity.OlcuBirimi = Convert.ToString(satir["OlcuBirimi"]);
                    entity.Aciklama1 = Convert.ToString(satir["Aciklama1"]);
                    entity.Aciklama2 = Convert.ToString(satir["Aciklama2"]);
                    entity.Aciklama3 = Convert.ToString(satir["Aciklama3"]);
                    entity.DosyaAdi = Convert.ToString(satir["DosyaAdi"]);
                    entity.DosyaLinki = Convert.ToString(satir["DosyaLinki"]);
                    entity.DosyaID = Convert.ToString(satir["DosyaID"]);
                    entity.Tarih = Convert.ToString(satir["KayitTarihi"]);
                    entity.Kullanici = Convert.ToString(satir["KayitYapanKullanici"]);
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SatinalmaFiyatKaldir(int Grupla, int KayitID, string StokKodu, string SecilenCariID, decimal SecilenFiyat, string SecilenDovizBirimi)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTeklifKaldir";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@KayitID", KayitID);
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                cmd.Parameters.AddWithValue("@SecilenCariID", SecilenCariID);
                cmd.Parameters.AddWithValue("@SecilenFiyat", SecilenFiyat);
                cmd.Parameters.AddWithValue("@SecilenDovizBirimi", SecilenDovizBirimi);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SatinalmaFiyatSec(int Grupla, int KayitID, string StokKodu, string SecilenCariID, decimal SecilenFiyat, string SecilenDovizBirimi)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_SatinalmaTeklifSec";
                cmd.Parameters.AddWithValue("@Grupla", Grupla);
                cmd.Parameters.AddWithValue("@KayitID", KayitID);
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                cmd.Parameters.AddWithValue("@SecilenCariID", SecilenCariID);
                cmd.Parameters.AddWithValue("@SecilenFiyat", SecilenFiyat);
                cmd.Parameters.AddWithValue("@SecilenDovizBirimi", SecilenDovizBirimi);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void SatinalmaExcel(string Kategori1 = "", string Kategori2 = "", string Kategori3 = "", string Grupla = null)
        {
            if (Grupla == null)
                Grupla = "0";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_SatinalmaKayitListesiExcel";
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", Kategori3);
            cmd.Parameters.AddWithValue("@Grupla", Grupla);
            cmd.Parameters.AddWithValue("@Kullanici", "");
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            ExporttoExcel(ds.Tables[1]);
        }

        public ActionResult Satinalma(string Kategori1 = "", string Kategori2 = "", string Kategori3 = "", string Grupla = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (Grupla == null)
                Grupla = "0";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_SatinalmaKayitListesi";
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", Kategori3);
            cmd.Parameters.AddWithValue("@Grupla", Grupla);
            cmd.Parameters.AddWithValue("@Kullanici", "");
            DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);

            ViewBag.dtKategori1 = ds.Tables[3];
            ViewBag.Kategori1 = Kategori1;
            ViewBag.Kategori2 = Kategori2;
            ViewBag.Kategori3 = Kategori3;
            ViewBag.Grupla = Grupla;

            return View(ds);
        }


        public ActionResult Teklifler(string Cari = "", string Stok = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            ViewBag.Cari = Cari;
            ViewBag.Stok = Stok;

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_SatinalmaTeklifler";
            cmd.Parameters.AddWithValue("@Cari", Cari);
            cmd.Parameters.AddWithValue("@Stok", Stok);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View(dt);
        }

        public ActionResult Teklif(int id = 0)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = @"Select 
ST.*,
S.StokKodu,
S.StokAdi,
C.CariKodu,
C.CariAdi
from SatinalmaTeklifleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_Stoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
Where ST.Silindi = 0 and ST.ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            return View(dt);
        }

        [HttpPost]
        public ActionResult TeklifKaydet(int id, string CariID, string Sozlesmeli, string StokID, DateTime Baslangic, DateTime Bitis,
            decimal Miktar, decimal Fiyat, string DovizBirimi, string Aciklama1, string Aciklama2, string Aciklama3,
            string Yonlendir = "", string Yonlendir1 = "", string Yonlendir2 = "", string Yonlendir3 = "", string Grupla = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            if (id > 0)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"Update SatinalmaTeklifleri Set 
CariID=@CariID,Sozlesmeli=@Sozlesmeli,StokID=@StokID,Baslangic=@Baslangic,Bitis=@Bitis,Miktar=@Miktar,Fiyat=@Fiyat,DovizBirimi=@DovizBirimi,Aciklama1=@Aciklama1,Aciklama2=@Aciklama2,Aciklama3=@Aciklama3
Where ID = @id
";
                cmd.Parameters.AddWithValue("@CariID", CariID);
                cmd.Parameters.AddWithValue("@Sozlesmeli", Sozlesmeli);
                cmd.Parameters.AddWithValue("@StokID", StokID);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@Miktar", Miktar);
                cmd.Parameters.AddWithValue("@Fiyat", Fiyat);
                cmd.Parameters.AddWithValue("@DovizBirimi", DovizBirimi);
                cmd.Parameters.AddWithValue("@Aciklama1", Aciklama1);
                cmd.Parameters.AddWithValue("@Aciklama2", Aciklama2);
                cmd.Parameters.AddWithValue("@Aciklama3", Aciklama3);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                cmd.Parameters.AddWithValue("@id", id);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "Insert Into SatinalmaTeklifleri (CariID,Sozlesmeli,StokID,Baslangic,Bitis,Miktar,Fiyat,DovizBirimi,Aciklama1,Aciklama2,Aciklama3,KayitTarihi,KayitYapanKullanici,Silindi) values (@CariID,@Sozlesmeli,@StokID,@Baslangic,@Bitis,@Miktar,@Fiyat,@DovizBirimi,@Aciklama1,@Aciklama2,@Aciklama3,GETDATE(),@Kullanici,0)";
                cmd.Parameters.AddWithValue("@CariID", CariID);
                cmd.Parameters.AddWithValue("@Sozlesmeli", Sozlesmeli);
                cmd.Parameters.AddWithValue("@StokID", StokID);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@Miktar", Miktar);
                cmd.Parameters.AddWithValue("@Fiyat", Fiyat);
                cmd.Parameters.AddWithValue("@DovizBirimi", DovizBirimi);
                cmd.Parameters.AddWithValue("@Aciklama1", Aciklama1);
                cmd.Parameters.AddWithValue("@Aciklama2", Aciklama2);
                cmd.Parameters.AddWithValue("@Aciklama3", Aciklama3);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }
            if (Yonlendir == "Satınalma")
            {
                return Redirect("~/Satinalma/YKSatinalma/Satinalma/?Kategori1=" + Yonlendir1 + "&Kategori2=" + Yonlendir2 + "&Kategori3=" + Yonlendir3 + "&Grupla=" + Grupla);
            }
            return Redirect("~/Satinalma/YKSatinalma/Teklifler");
        }


        #endregion

        #region Talep İşlemleri

        public ActionResult YeniTalep2(string Kategori1 = "", string Kategori2 = "", string Kategori3 = "", string AranacakKelime = "", string Sube = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_KullaniciSubeYetkileri";
            cmd.Parameters.AddWithValue("@ID", KullaniciID);
            ViewBag.dtSubeler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            if (Sube == "")
            {
                Sube = Convert.ToString(((DataTable)ViewBag.dtSubeler).Rows[0]["Kod"]);
            }

            string kisit = "";
            if (Kategori1 + Kategori2 + Kategori3 + AranacakKelime == "")
            {
                kisit = " 1=2 and ";
            }
            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = @"
select  top(2000)
w_Stoklar.ID,
Kategori1,
Kategori2,
Kategori3,
w_Stoklar.StokKodu,
w_Stoklar.StokAdi,
w_Stoklar.BirimID,
Kdv,
OlcuBirimi,
TalepMiktari,
ISNULL([w_StokBakiyeleri].Bakiye,0) as Bakiye,
Aciklama1,
Aciklama2
from w_Stoklar LEFT OUTER JOIN [w_StokBakiyeleri] ON [w_StokBakiyeleri].ID = w_Stoklar.ID and [w_StokBakiyeleri].SubeKodu = @SubeKodu
Where " + kisit + @"(w_Stoklar.Kategori1 = @Kategori1 or @Kategori1 = '') 
and (w_Stoklar.Kategori2 = @Kategori2 or @Kategori2 = '') 
and (w_Stoklar.Kategori3 = @Kategori3 or @Kategori3 = '') 
and (w_Stoklar.StokAdi like '%" + AranacakKelime + "%') ";
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", Kategori3);
            cmd.Parameters.AddWithValue("@SubeKodu", Sube);
            cmd.Parameters.AddWithValue("@AranacakKelime", AranacakKelime);
            DataTable dtStoklar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select Kategori1 from w_Stoklar Group by KategoriKodu1,Kategori1 order by KategoriKodu1";
            ViewBag.dtKategori1 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.Kategori1 = Kategori1;
            ViewBag.Kategori2 = Kategori2;
            ViewBag.Kategori3 = Kategori3;
            ViewBag.Sube = Sube;
            return View(dtStoklar);
        }
        public ActionResult YeniTalep(string Kategori1 = "", string Kategori2 = "", string Kategori3 = "", string AranacakKelime = "", string Sube = "")
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_KullaniciSubeYetkileri";
            cmd.Parameters.AddWithValue("@ID", KullaniciID);
            ViewBag.dtSubeler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            if (Sube == "")
            {
                Sube = Convert.ToString(((DataTable)ViewBag.dtSubeler).Rows[0]["Kod"]);
            }

            string kisit = "";
            if (Kategori1 + Kategori2 + Kategori3 + AranacakKelime == "")
            {
                kisit = " 1=2 and ";
            }
            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = @"
select  top(2000)
w_SatinalmaStoklar.ID,
w_SatinalmaStoklar.IDTresiz,
Kategori1,
Kategori2,
Kategori3,
w_SatinalmaStoklar.StokKodu,
w_SatinalmaStoklar.StokAdi,
w_SatinalmaStoklar.BirimID,
Kdv,
OlcuBirimi,
TalepMiktari,
ISNULL([w_StokBakiyeleri].Bakiye,0) as Bakiye,
Aciklama1,
Aciklama2
from w_SatinalmaStoklar LEFT OUTER JOIN [w_StokBakiyeleri] ON [w_StokBakiyeleri].ID = w_SatinalmaStoklar.ID and [w_StokBakiyeleri].SubeKodu = @SubeKodu
Where " + kisit + @"(w_SatinalmaStoklar.Kategori1 = @Kategori1 or @Kategori1 = '') 
and (w_SatinalmaStoklar.Kategori2 = @Kategori2 or @Kategori2 = '') 
and (w_SatinalmaStoklar.Kategori3 = @Kategori3 or @Kategori3 = '') 
and (w_SatinalmaStoklar.StokAdi like '%" + AranacakKelime + "%') ";
            cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
            cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
            cmd.Parameters.AddWithValue("@Kategori3", Kategori3);
            cmd.Parameters.AddWithValue("@SubeKodu", Sube);
            cmd.Parameters.AddWithValue("@AranacakKelime", AranacakKelime);
            DataTable dtStoklar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
           

            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select Kategori1 from w_Stoklar Group by KategoriKodu1,Kategori1 order by KategoriKodu1";
            ViewBag.dtKategori1 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.Kategori1 = Kategori1;
            ViewBag.Kategori2 = Kategori2;
            ViewBag.Kategori3 = Kategori3;
            ViewBag.Sube = Sube;
            return View(dtStoklar);
        }

        public JsonResult SifreKontrol(string Sifre = "")
        {
            YKJsonResult result = new YKJsonResult();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Select * from Sifreler Where Sifre = @Sifre and DATEADD(MINUTE,+120,KayitTarihi) > GETDATE()";
            cmd.Parameters.AddWithValue("@Sifre", Sifre);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            if (dt.Rows.Count > 0)
            {
                result.SonucKodu = "1";
            }
            else
            {
                result.SonucKodu = "0";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TalepKaydet2(int id, string SubeKodu = "", decimal miktar = 0, string aciklama = "", HttpPostedFileBase Dosya = null)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalepKaydet2";
                cmd.Parameters.AddWithValue("@StokID", id);
                cmd.Parameters.AddWithValue("@SubeKodu", SubeKodu);
                cmd.Parameters.AddWithValue("@Miktar", miktar);
                cmd.Parameters.AddWithValue("@Aciklama1", aciklama);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (Bilgi.StartsWith("UYARI!"))
                    {
                        result.Aciklama += Bilgi + "<br>";
                    }
                    else
                    {
                        result.Aciklama += Bilgi + "<br>";
                        int SonID = Convert.ToInt32(dt.Rows[0]["Data"]);
                        if (Dosya != null)
                        {
                            string dosyaYolu = Path.GetFileName(Dosya.FileName);
                            string yeniIsmi = Guid.NewGuid().ToString();
                            string uzanti = Path.GetExtension(Dosya.FileName);
                            var yuklemeYeri = Path.Combine(Server.MapPath("~/Uploads/"), yeniIsmi + uzanti);
                            Dosya.SaveAs(yuklemeYeri);

                            SqlCommand cmdDosya = new SqlCommand();
                            cmdDosya.CommandText = "Insert Into Dosyalar (Modul,Dosya,Isim,DosyaUzantisi,KayitTarihi,KayitYapanKullanici,KayitID) values (@Modul,@Dosya,@Isim,@DosyaUzantisi,@KayitTarihi,@KayitYapanKullanici,@KayitID) ";
                            cmdDosya.Parameters.AddWithValue("@Modul", "Talep");
                            cmdDosya.Parameters.AddWithValue("@Dosya", yeniIsmi + uzanti);
                            cmdDosya.Parameters.AddWithValue("@Isim", Dosya.FileName);
                            cmdDosya.Parameters.AddWithValue("@DosyaUzantisi", uzanti);
                            cmdDosya.Parameters.AddWithValue("@KayitTarihi", DateTime.Now);
                            cmdDosya.Parameters.AddWithValue("@KayitYapanKullanici", KullaniciAdi);
                            cmdDosya.Parameters.AddWithValue("@KayitID", SonID);
                            IDVeritabani.Sorgula(cmdDosya, SorgulaTuru.Bos);
                        }
                    }
                }
                result.SonucKodu = "1";
                //result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalepKaydet(string id="", string SubeKodu = "", decimal miktar = 0, string aciklama = "", HttpPostedFileBase Dosya = null)
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalepKaydet";
                cmd.Parameters.AddWithValue("@StokID", id);
                cmd.Parameters.AddWithValue("@SubeKodu", SubeKodu);
                cmd.Parameters.AddWithValue("@Miktar", miktar);
                cmd.Parameters.AddWithValue("@Aciklama1", aciklama);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (Bilgi.StartsWith("UYARI!"))
                    {
                        result.Aciklama += Bilgi + "<br>";
                    }
                    else
                    {
                        result.Aciklama += Bilgi + "<br>";
                        int SonID = Convert.ToInt32(dt.Rows[0]["Data"]);
                        if (Dosya != null)
                        {
                            string dosyaYolu = Path.GetFileName(Dosya.FileName);
                            string yeniIsmi = Guid.NewGuid().ToString();
                            string uzanti = Path.GetExtension(Dosya.FileName);
                            var yuklemeYeri = Path.Combine(Server.MapPath("~/Uploads/"), yeniIsmi + uzanti);
                            Dosya.SaveAs(yuklemeYeri);

                            SqlCommand cmdDosya = new SqlCommand();
                            cmdDosya.CommandText = "Insert Into Dosyalar (Modul,Dosya,Isim,DosyaUzantisi,KayitTarihi,KayitYapanKullanici,KayitID) values (@Modul,@Dosya,@Isim,@DosyaUzantisi,@KayitTarihi,@KayitYapanKullanici,@KayitID) ";
                            cmdDosya.Parameters.AddWithValue("@Modul", "Talep");
                            cmdDosya.Parameters.AddWithValue("@Dosya", yeniIsmi + uzanti);
                            cmdDosya.Parameters.AddWithValue("@Isim", Dosya.FileName);
                            cmdDosya.Parameters.AddWithValue("@DosyaUzantisi", uzanti);
                            cmdDosya.Parameters.AddWithValue("@KayitTarihi", DateTime.Now);
                            cmdDosya.Parameters.AddWithValue("@KayitYapanKullanici", KullaniciAdi);
                            cmdDosya.Parameters.AddWithValue("@KayitID", SonID);
                            IDVeritabani.Sorgula(cmdDosya, SorgulaTuru.Bos);
                        }
                    }
                }
                result.SonucKodu = "1";
                if(result.Aciklama == "")
                    result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void TaleplerimExcel(DateTime? Tarih1, DateTime? Tarih2)
        {
            if (Tarih1 == null)
            {
                Tarih1 = DateTime.Today.AddDays(-30);
            }
            if (Tarih2 == null)
            {
                Tarih2 = DateTime.Today.AddDays(1);
            }
            ViewBag.Tarih1 = Tarih1;
            ViewBag.Tarih2 = Tarih2;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_Taleplerim";
            cmd.Parameters.AddWithValue("@KullaniciAdi", GetCookie("KullaniciAdi"));
            cmd.Parameters.AddWithValue("@Tarih1", Tarih1);
            cmd.Parameters.AddWithValue("@Tarih2", Tarih2);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            foreach (DataRow dr in dtKayitlar.Rows)
            {
                switch (Convert.ToString(dr["Durumu"]))
                {
                    case "Onay4": dr["Durumu"] = "Tamamlandı"; break;
                    case "Onay3": dr["Durumu"] = "Yönetim Onayda"; break;
                    case "Onay2": dr["Durumu"] = "Genel Müdür Onayda"; break;
                    case "Onay1": dr["Durumu"] = "Cost Onayda"; break;
                    case "Onayda": dr["Durumu"] = "Satınalma Onayda"; break;
                    case "Talep Onaylandı": dr["Durumu"] = "Fiyat Bekliyor"; break;
                    case "Genel Müdür Red": dr["Durumu"] = "Genel Müdür Red"; break;
                    case "Yönetim Red": dr["Durumu"] = "Yönetim Red"; break;
                    case "Cost Red": dr["Durumu"] = "Cost Red"; break;
                    case "Satınalma Red": dr["Durumu"] = "Satınalma Red"; break;
                    default: dr["Durumu"] = "Fiyat Bekliyor"; break;
                }
            }
            ExporttoExcel(dtKayitlar);
        }

        private void ExporttoExcel(DataTable table)
        {



            if (true)
            {
                HttpContext.Response.Clear();
                HttpContext.Response.ClearContent();
                HttpContext.Response.ClearHeaders();
                HttpContext.Response.Buffer = true;
                HttpContext.Response.ContentType = "application/vnd.ms-excel";
                //HttpContext.Current.Response.ContentType = "application/ms-word";
                //HttpContext.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");
                // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.doc");
                HttpContext.Response.Charset = "utf-8";
                //HttpContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

                HttpContext.Response.ContentEncoding = Encoding.UTF8;
                HttpContext.Response.BinaryWrite(Encoding.UTF8.GetPreamble()); // Turkce karakter sorunu icin mutlaka olmasi lazim...
                HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);

                HttpContext.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                HttpContext.Response.Write("<BR><BR><BR>");
                HttpContext.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                int columnscount = table.Columns.Count;

                for (int j = 0; j < columnscount; j++)
                {
                    HttpContext.Response.Write("<Td>");
                    HttpContext.Response.Write("<B>");
                    HttpContext.Response.Write(table.Columns[j].Caption.ToString());
                    HttpContext.Response.Write("</B>");
                    HttpContext.Response.Write("</Td>");
                }
                HttpContext.Response.Write("</TR>");
                foreach (DataRow row in table.Rows)
                {
                    HttpContext.Response.Write("<TR>");
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        HttpContext.Response.Write("<Td>");
                        HttpContext.Response.Write(row[i].ToString());
                        HttpContext.Response.Write("</Td>");
                    }

                    HttpContext.Response.Write("</TR>");
                }
                HttpContext.Response.Write("</Table>");
                HttpContext.Response.Write("</font>");
                HttpContext.Response.Flush();
                HttpContext.Response.End();
            }
        }

        public ActionResult Taleplerim(DateTime? Tarih1, DateTime? Tarih2)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (Tarih1 == null)
            {
                Tarih1 = DateTime.Today.AddDays(-30);
            }
            if (Tarih2 == null)
            {
                Tarih2 = DateTime.Today.AddDays(1);
            }
            ViewBag.Tarih1 = Tarih1;
            ViewBag.Tarih2 = Tarih2;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_Taleplerim";
            cmd.Parameters.AddWithValue("@KullaniciAdi", GetCookie("KullaniciAdi"));
            cmd.Parameters.AddWithValue("@Tarih1", Tarih1);
            cmd.Parameters.AddWithValue("@Tarih2", Tarih2);
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dtKayitlar);
        }

        public ActionResult TalepOnayi()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_TalepOnayListesi";
            cmd.Parameters.AddWithValue("@KullaniciAdi", GetCookie("KullaniciAdi"));
            DataTable dtKayitlar = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dtKayitlar);
        }
        [HttpPost]

        public JsonResult TalepOnayla(int KayitID, decimal Miktar = 0, string aciklama = "")
        {
            YKJsonResult result = new YKJsonResult();

            string KullaniciAdi = GetCookie("KullaniciAdi");
            string KullaniciID = GetCookie("KullaniciID");

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_TalepOnayla";
                cmd.Parameters.AddWithValue("@KayitID", KayitID);
                cmd.Parameters.AddWithValue("@Miktar", Miktar);
                cmd.Parameters.AddWithValue("@Aciklama1", aciklama);
                cmd.Parameters.AddWithValue("@Kullanici", KullaniciAdi);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Genel Json İşlemleri

        public JsonResult JsonGetirStoklar(string Stok = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select top(25) * From w_Stoklar Where StokAdi like '%'+@Cari+'%'";
                cmd.Parameters.AddWithValue("@Cari", Stok);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.ID = Convert.ToString(satir["ID"]);
                    entity.Kod = Convert.ToString(satir["StokKodu"]);
                    entity.Isim = Convert.ToString(satir["StokAdi"]);
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JsonGetirCariler(string Cari = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select top(25) * From w_Cariler Where CariAdi like '%'+@Cari+'%'";
                cmd.Parameters.AddWithValue("@Cari", Cari);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.ID = Convert.ToString(satir["ID"]);
                    entity.Kod = Convert.ToString(satir["CariKodu"]);
                    entity.Isim = Convert.ToString(satir["CariAdi"]);
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonGetirKategori2(string Kategori1 = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select Kategori2 from w_Stoklar Where Kategori1 = @Kategori1 Group by KategoriKodu2,Kategori2 order by KategoriKodu2";
                cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.Isim = Convert.ToString(satir["Kategori2"]);
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JsonGetirKategori3(string Kategori1 = "", string Kategori2 = "")
        {
            YKJsonResult result = new YKJsonResult();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select Kategori3 from w_Stoklar Where Kategori1 = @Kategori1 and Kategori2 = @Kategori2 Group by KategoriKodu3,Kategori3 order by KategoriKodu3";
                cmd.Parameters.AddWithValue("@Kategori1", Kategori1);
                cmd.Parameters.AddWithValue("@Kategori2", Kategori2);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                List<YKModelSatinalma> entities = new List<YKModelSatinalma>();

                foreach (DataRow satir in dt.Rows)
                {
                    YKModelSatinalma entity = new YKModelSatinalma();
                    entity.Isim = Convert.ToString(satir["Kategori3"]);
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = "1";
                result.Aciklama = "Başarılı.";
            }
            catch (Exception err)
            {
                result.SonucKodu = "0";
                result.Aciklama = "HATA! " + err.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cookie İşlemleri


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
                    GirisKontrol = true;
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

        #endregion
    }

    public class YKJsonResult
    {
        public object Data { get; set; }
        public string SonucKodu { get; set; }
        public string Aciklama { get; set; }
    }
}