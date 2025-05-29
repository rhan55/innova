using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace YKPortal.Controllers
{
    public class XmlController : Controller
    {
        // GET: Xml
        public JsonResult OtoSentezxml()
        {
            SqlConnection Baglanti = new SqlConnection(ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString);
            try
            {
                XmlDocument xDocdd = new XmlDocument();
                xDocdd.Load("https://customer.meydangroup.com.tr/xml/mxml.aspx?cari_kod=07_07_173");
                if (xDocdd.ChildNodes.Count > 0)
                {
                    if (Baglanti.State == System.Data.ConnectionState.Closed)
                        Baglanti.Open();

                    Console.WriteLine("dosya okunuyor...");
                    foreach (XmlNode item in xDocdd.SelectNodes("Products"))
                    {
                        foreach (XmlNode urun in item.SelectNodes("Product"))
                        {
                            string stok_kodu = urun.SelectNodes("KOD")[0].InnerText;
                            string stok_adi = urun.SelectNodes("stok_adi")[0].InnerText;
                            decimal fiyat = Convert.ToDecimal(urun.SelectNodes("fiyat")[0].InnerText);
                            string fiyatk = urun.SelectNodes("fiyatk")[0].InnerText;
                            string dovtip = urun.SelectNodes("dovtip")[0].InnerText;
                            decimal bakiye = Convert.ToDecimal(urun.SelectNodes("BAKIYE")[0].InnerText);
                            string marka = urun.SelectNodes("MARKA")[0].InnerText;
                            string model = urun.SelectNodes("MODEL")[0].InnerText;
                            string ana_grup = urun.SelectNodes("ANA_GRUP")[0].InnerText;
                            string alt_grup = urun.SelectNodes("ALT_GRUP")[0].InnerText;
                            string kampanya = urun.SelectNodes("KAMPANYA")[0].InnerText;
                            string oem_kod = urun.SelectNodes("OEM_KOD")[0].InnerText;
                            string katsayi = urun.SelectNodes("KATSAYI")[0].InnerText;
                            string kasa = urun.SelectNodes("KASA")[0].InnerText;

                            Console.WriteLine("Stok Kodu >  " + stok_kodu + " Stok Adi  >   " + stok_adi + "  > Fiyat " + fiyat);

                            using (SqlCommand cmd = new SqlCommand("pXmlStokAktar", Baglanti))
                            {
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@stok_kodu", stok_kodu);
                                cmd.Parameters.AddWithValue("@stok_adi", stok_adi);
                                cmd.Parameters.AddWithValue("@fiyat", fiyat);
                                cmd.Parameters.AddWithValue("@fiyatk", fiyatk);
                                cmd.Parameters.AddWithValue("@dovtip", dovtip);
                                cmd.Parameters.AddWithValue("@bakiye", bakiye);
                                cmd.Parameters.AddWithValue("@marka", marka);
                                cmd.Parameters.AddWithValue("@Beden", model);
                                cmd.Parameters.AddWithValue("@ana_grup", ana_grup);
                                cmd.Parameters.AddWithValue("@alt_grup", alt_grup);
                                cmd.Parameters.AddWithValue("@kampanya", kampanya);
                                cmd.Parameters.AddWithValue("@oem_kod", oem_kod);
                                cmd.Parameters.AddWithValue("@katsayi", katsayi);
                                cmd.Parameters.AddWithValue("@kasa", kasa);

                                string ProductId = Convert.ToString(cmd.ExecuteScalar());
                                if (ProductId.Length > 0)
                                {
                                    int resimsayac = 0;
                                    foreach (XmlElement pointCoord in urun.SelectNodes("Images"))
                                    {
                                        XmlNodeList xnl2 = pointCoord.SelectNodes("Image");
                                        foreach (XmlNode node2 in xnl2)
                                        {
                                            var remoteFile = node2.InnerText;
                                            string filename = Path.GetFileName(remoteFile);
                                            using (SqlCommand cmdresim = new SqlCommand("pXmlStokResimAktar", Baglanti))
                                            {
                                                cmdresim.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmdresim.Parameters.AddWithValue("@StokId", ProductId);
                                                cmdresim.Parameters.AddWithValue("@StokKodu", stok_kodu);
                                                cmdresim.Parameters.AddWithValue("@ResimYolu", filename);
                                                cmdresim.ExecuteNonQuery();
                                            }
                                        }
                                        resimsayac++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return Json("");
        }
    }
}