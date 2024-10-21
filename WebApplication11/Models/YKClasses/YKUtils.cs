using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace YKPortal.Models.YKClasses
{
    public class YKUtils
    {
        public static string SmsKullaniciAdi = ConfigurationManager.AppSettings["SmsKullaniciAdi"];
        public static string SmsParola = ConfigurationManager.AppSettings["SmsParola"];
        public static string SmsIsim = ConfigurationManager.AppSettings["SmsIsim"];
        public static string GetIPAdress()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return new System.Net.WebClient().DownloadString("https://api.ipify.org");
        }

        public static string GetClientIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public static string GetVersion()
        {
            try
            {
                return System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static void MailGonder(string Baslik, string Icerik, string GonderilecekMailAdresleri,
            string GonderenMailAdresi, string GonderenMailSifresi, string GonderenMailServer, int GonderenMailPort, 
            bool GonderenMailSSL)
        {

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(GonderenMailAdresi, ConfigurationManager.AppSettings["FirmaAdi"]);

            foreach (string mailAdresi in GonderilecekMailAdresleri.Split(';'))
            {
                if(mailAdresi.Trim().Length > 0)
                    mail.To.Add(mailAdresi);
            }


            mail.IsBodyHtml = true;
            mail.Subject = Baslik;
            mail.Body =Icerik;
            mail.IsBodyHtml = true;
            try
            {
                SmtpClient sc = new SmtpClient();
                sc.Port = GonderenMailPort; // 587;
                sc.Host = GonderenMailServer; // "mail.ykyazilim.com.tr";
                sc.EnableSsl = GonderenMailSSL; // false;
                sc.Credentials = new NetworkCredential(GonderenMailAdresi, GonderenMailSifresi);
                sc.Send(mail);
            }
            catch (Exception err)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Clear();
                cmd.CommandText = "p_HataKaydet";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", "");
                cmd.Parameters.AddWithValue("@Kullanici", "Ortak Metod");
                cmd.Parameters.AddWithValue("@Modul", "YKUtils");
                cmd.Parameters.AddWithValue("@Aciklama1", "~/YKUtils/Mailgonder");
                cmd.Parameters.AddWithValue("@Aciklama2", err.Message);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }
        }

        public static string ParametreGetir(string UyelikID, string ParametreKodu)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "p_Parametre";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
            cmd.Parameters.AddWithValue("@Kod", ParametreKodu);
            DataTable dtNetsisDatatable = (DataTable)(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo));
            string Deger = "";
            if (dtNetsisDatatable.Rows.Count > 0)
            {
                Deger = Convert.ToString(dtNetsisDatatable.Rows[0]["Deger"]);
            }
            return Deger;
        }

        public static void LogKaydet_KullaniciGirisi(string ProgramAdi,
            string Sirket,
            string KullaniciAdi,
            string Parola,
            string IP)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString;
                DateTime Tarih = DateTime.Now;
                string IP_ = GetIPAdress()+" - "+ IP;
                string Surum = GetVersion();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
Insert Into LoglarGiris 
(ProgramAdi,Surum,Tarih,Sirket,ConnectionString,KullaniciAdi,Parola,IP)
values
(@ProgramAdi,@Surum,@Tarih,@Sirket,@ConnectionString,@KullaniciAdi,@Parola,@IP)
";
                cmd.Parameters.AddWithValue("@ProgramAdi", ProgramAdi);
                cmd.Parameters.AddWithValue("@Surum", Surum);
                cmd.Parameters.AddWithValue("@Tarih", Tarih);
                cmd.Parameters.AddWithValue("@Sirket", Sirket);
                cmd.Parameters.AddWithValue("@ConnectionString", ConnectionString);
                cmd.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
                cmd.Parameters.AddWithValue("@Parola", Parola);
                cmd.Parameters.AddWithValue("@IP", IP_);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }
            catch (Exception err)
            {
                ;
            }


        }

        private static readonly object balanceLock = new object();
        public static void MailGonder(string Baslik, string Icerik, string GonderilecekMailAdresleri, string dosya, ArrayList eklenecekDosyalar, string tip = "Teklif")
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            lock (balanceLock)
            {
                try
                {
                    // Hata Maili Gönderimi
                    #region 
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT ID, Server, MailAdresi, MailAdresi2, Parola, Port, Tip, Aktif, IsletmeID, OrjinalMailAdresi FROM ID_MailTanimlamalari WITH(NOLOCK) Where Aktif = 1 and Tip = '" + tip + "'";
                    DataTable dtMailBilgileri = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                    MailMessage ePosta = new MailMessage();
                    ePosta.From = new MailAddress(
                        Convert.ToString(dtMailBilgileri.Rows[0]["OrjinalMailAdresi"]),
                        Convert.ToString(dtMailBilgileri.Rows[0]["OrjinalMailAdresi"]));

                    if (dosya.Trim().Length > 0)
                    {
                        ePosta.Attachments.Add(new Attachment(dosya));
                    }
                    if (eklenecekDosyalar != null)
                    {
                        foreach (string item in eklenecekDosyalar)
                        {
                            ePosta.Attachments.Add(new Attachment(item));
                        }
                    }

                    if (ConfigurationManager.AppSettings["TestMail"] != "1")
                    {
                        foreach (var item in GonderilecekMailAdresleri.Split(';'))
                        {
                            if (item.Trim().Length > 0)
                            {
                                ePosta.To.Add(new MailAddress(item.Trim(), item.Trim()));
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in ConfigurationManager.AppSettings["TestMailAdresi"].Split(';'))
                        {
                            if (item.Trim().Length > 0)
                            {
                                ePosta.To.Add(new MailAddress(item.Trim(), item.Trim()));
                            }
                        }
                    }

                    foreach (var item in ConfigurationManager.AppSettings["MailCC"].Split(';'))
                    {
                        if (item.Trim().Length > 0)
                        {
                            ePosta.To.Add(new MailAddress(item.Trim(), item.Trim()));
                        }
                    }
                    ePosta.Subject = Baslik;
                    ePosta.Body = Icerik;
                    ePosta.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Credentials = new System.Net.NetworkCredential(
                        Convert.ToString(dtMailBilgileri.Rows[0]["MailAdresi"]),
                        Convert.ToString(dtMailBilgileri.Rows[0]["Parola"]));
                    smtp.Port = Convert.ToInt32(dtMailBilgileri.Rows[0]["Port"]);
                    smtp.Host = Convert.ToString(dtMailBilgileri.Rows[0]["Server"]);
                    smtp.EnableSsl = Convert.ToString(dtMailBilgileri.Rows[0]["MailAdresi2"]) == "0" ? false : true;
                    smtp.Send(ePosta);

                    #endregion

                }
                catch (Exception err)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "Insert Into Loglar (Sayfa,Parametre1,Parametre2,Parametre3,Parametre4,CDate) values (@Sayfa,@Parametre1,@Parametre2,@Parametre3,@Parametre4,GETDATE())";
                    cmd.Parameters.AddWithValue("@Sayfa", "Teklif Mail Gönderim");
                    cmd.Parameters.AddWithValue("@Parametre1", err.Message);
                    cmd.Parameters.AddWithValue("@Parametre2", "");
                    cmd.Parameters.AddWithValue("@Parametre3", "");
                    cmd.Parameters.AddWithValue("@Parametre4", "");
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }
            }
        }


    }
}