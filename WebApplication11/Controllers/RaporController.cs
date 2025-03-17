using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YKPortal.Models;

namespace YKPortal.Controllers
{
    public class RaporController : Controller
    {

        public ActionResult RaporIsimleri()
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_RaporIsimleri";
            cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
            cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

        public ActionResult Getir(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Select Sorgu From Raporlar WITH(NOLOCK) Where ID = @ID";
            cmd.Parameters.AddWithValue("@ID",id);
            string Sorgu = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

            cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Select Isim From Raporlar WITH(NOLOCK) Where ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            ViewBag.Isim = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

            cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = Sorgu;
            cmd.Parameters.AddWithValue("@ID", id);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
            ViewBag.id = id;

            return View(dt);
        }
        public ActionResult GetirExcel(string id)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");

            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Select Sorgu From Raporlar WITH(NOLOCK) Where ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            string Sorgu = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

            cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Select Isim From Raporlar WITH(NOLOCK) Where ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            ViewBag.Isim = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

            cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = Sorgu;
            cmd.Parameters.AddWithValue("@ID", id);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


            // 2️⃣ XML Başlangıcı
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\"?>");
            xml.Append("<?mso-application progid=\"Excel.Sheet\"?>");
            xml.Append("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            xml.Append(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            xml.Append(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            xml.Append(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
            xml.Append("<Worksheet ss:Name=\"Siparişler\">");
            xml.Append("<Table>");

            // 3️⃣ Başlık Satırını Ekleyelim
            xml.Append("<Row>");
            foreach (DataColumn column in dt.Columns)
            {
                xml.AppendFormat("<Cell><Data ss:Type='String'>{0}</Data></Cell>", column.ColumnName);
            }
            xml.Append("</Row>");

            // 4️⃣ Satırları Ekleyelim
            foreach (DataRow row in dt.Rows)
            {
                xml.Append("<Row>");
                foreach (DataColumn column in dt.Columns)
                {
                    string value = row[column].ToString();
                    xml.AppendFormat("<Cell><Data ss:Type='String'>{0}</Data></Cell>", value);
                }
                xml.Append("</Row>");
            }

            // 5️⃣ XML Kapatma
            xml.Append("</Table>");
            xml.Append("</Worksheet>");
            xml.Append("</Workbook>");

            // 6️⃣ Dosyayı Kullanıcıya İndirtelim
            byte[] fileBytes = Encoding.UTF8.GetBytes(xml.ToString());
            return File(fileBytes, "application/vnd.ms-excel", "Rapor.xls");
        }

        public ActionResult SebatIsEmriRaporu(DateTime? Baslangic= null, DateTime? Bitis=null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            string UyelikID = GetCookie("UyelikID");
            string KullaniciID = GetCookie("KullaniciID");
            if (Baslangic == null)
            {
                Baslangic = DateTime.Today.AddDays(-30);
            }
            if (Bitis == null)
            {
                Bitis = DateTime.Today;
            }
            ViewBag.Baslangic = Baslangic;
            ViewBag.Bitis = Bitis;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "EXEC [p_UretimIsEmirleri] '66DB319A-EA5C-41AE-A9CA-387C166CD074', 3, '"+ Convert.ToDateTime(Baslangic).ToString("yyyy-MM-dd")+ "', '"+ Convert.ToDateTime(Bitis).ToString("yyyy-MM-dd") + "'";
            //cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
            //cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            return View(dt);
        }

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
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (!Bilgi.StartsWith("UYARI!"))
                    {
                        DeleteCookie("UyelikBitisTarihi");
                        CreateCookie("UyelikBitisTarihi", Convert.ToString(dt.Rows[0]["UyelikBitisTarihi"]));

                        GirisKontrol = true;
                    }
                    else
                    {
                        GirisKontrol = false;
                    }
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

        #endregion
    }
}