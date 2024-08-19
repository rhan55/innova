using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

            SmtpClient sc = new SmtpClient();
            sc.Port = GonderenMailPort; // 587;
            sc.Host = GonderenMailServer; // "mail.ykyazilim.com.tr";
            sc.EnableSsl = GonderenMailSSL; // false;
            sc.Credentials = new NetworkCredential(GonderenMailAdresi, GonderenMailSifresi);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(GonderenMailAdresi, ConfigurationManager.AppSettings["FirmaAdi"]);

            foreach (string mailAdresi in GonderilecekMailAdresleri.Split(';'))
            {
                if(mailAdresi.Trim().Length > 0)
                    mail.To.Add(mailAdresi);
            }
            

            mail.Subject = Baslik;
            mail.IsBodyHtml = true;
            mail.Body =Icerik;
            try
            {
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
    }
}