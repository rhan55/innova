using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace YKPortal.Models.YKClasses
{
    public class YKUtils
    {

        public static string GetIPAdress()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return new System.Net.WebClient().DownloadString("https://api.ipify.org");
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

        public static void LogKaydet_KullaniciGirisi(string ProgramAdi,
            string Sirket,
            string KullaniciAdi,
            string Parola)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString;
                DateTime Tarih = DateTime.Now;
                string IP = GetIPAdress();
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
                cmd.Parameters.AddWithValue("@IP", IP);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
            }
            catch (Exception err)
            {
                ;
            }


        }
    }
}