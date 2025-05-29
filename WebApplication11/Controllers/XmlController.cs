using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace YKPortal.Controllers
{
    public class XmlController : Controller
    {
        SqlConnection Baglanti = new SqlConnection(ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString);
        public JsonResult OtoSentezxml()
        {
      
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

        public JsonResult MotoGpXml()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"c:\g5qrks2g_motoucyuzaltmis1.xml");
            //   xDoc.Load("https://drdb2b.com/connectprof/g5qrks2g_motoucyuzaltmis.xml");
            if (xDoc.ChildNodes.Count > 0)
            {
                //SqlConnection Baglanti = new SqlConnection(ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString);
                if (Baglanti.State == System.Data.ConnectionState.Closed)
                    Baglanti.Open();
                foreach (XmlNode item in xDoc.ChildNodes)
                {
                    foreach (XmlNode urun in item.ChildNodes)
                    {
                        string Kategori1 = urun.SelectNodes("cat1name")[0].InnerText;
                        string Kategori2 = urun.SelectNodes("cat2name")[0].InnerText;
                        string Kategori3 = urun.SelectNodes("cat3name")[0].InnerText;
                        string StokKodu = urun.SelectNodes("code")[0].InnerText;
                        string StokAdi = urun.SelectNodes("name")[0].InnerText;
                        string stock = urun.SelectNodes("stock")[0].InnerText;
                        string reel_stock = urun.SelectNodes("reel_stock")[0].InnerText;
                        string vat = urun.SelectNodes("vat")[0].InnerText;
                        string details = urun.SelectNodes("details")[0].InnerText;

                        Console.WriteLine(Kategori1 + ">" + Kategori2 + ">" + Kategori3 + ">" + StokKodu + ">" + StokAdi);
                        foreach (XmlNode bedenler in urun.SelectNodes("subproducts")[0].ChildNodes)
                        {
                            string vws_codeat = bedenler.SelectNodes("ws_code")[0].InnerText;
                            string barcode = bedenler.SelectNodes("barcode")[0].InnerText;
                            string beden = bedenler.SelectNodes("type2")[0].InnerText;
                            string bedenstock = bedenler.SelectNodes("stock")[0].InnerText;
                            string price_list = bedenler.SelectNodes("price_list")[0].InnerText;
                            Console.WriteLine(vws_codeat + ">" + barcode + ">" + beden + ">" + price_list);
                            using (SqlCommand cmd = new SqlCommand("p_UrunAktar", Baglanti))
                            {
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@kategori1", Kategori1);
                                cmd.Parameters.AddWithValue("@kategori2", Kategori2);
                                cmd.Parameters.AddWithValue("@kategori3", Kategori3);
                                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                                cmd.Parameters.AddWithValue("@StokAdi", StokAdi);
                                cmd.Parameters.AddWithValue("@BedenKodu", vws_codeat);
                                cmd.Parameters.AddWithValue("@Barkod", barcode);
                                cmd.Parameters.AddWithValue("@Beden", beden);
                                cmd.Parameters.AddWithValue("@Resim1", "");
                                cmd.Parameters.AddWithValue("@Aciklama", details);
                                cmd.Parameters.AddWithValue("@Fiyat", Convert.ToDecimal(price_list) * (1 + (Convert.ToDecimal(vat) / 100)));
                                //  cmd.Parameters.AddWithValue("@Resim1", Path.GetFileName(urun.SelectNodes("img_item")[0].InnerText.Replace("https://www.drdb2b.com/", "")));
                                string ProductId = Convert.ToString(cmd.ExecuteScalar());
                                if (ProductId.Length > 0)
                                {
                                    try
                                    {
                                        int resimsayac = 0;
                                        foreach (XmlNode resimler in urun.SelectNodes("img_item"))
                                        {
                                            var remoteFile = resimler.InnerText;
                                            string filename = Path.GetFileName(remoteFile.Replace("https://www.drdb2b.com/", ""));
                                            using (SqlCommand cmdresim = new SqlCommand("p_UrunResimAktar", Baglanti))
                                            {
                                                cmdresim.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmdresim.Parameters.AddWithValue("@ProductId", ProductId);
                                                cmdresim.Parameters.AddWithValue("@ImageName", filename);
                                                cmdresim.Parameters.AddWithValue("@IsPrimaryImage", resimsayac == 0 ? 1 : 0);
                                                cmdresim.ExecuteNonQuery();

                                                string lastfilename = remoteFile + filename;
                                                var localFileorg = System.IO.Directory.GetCurrentDirectory() + @"\images\" + filename;
                                                WebClient webClientorg = new WebClient();
                                                webClientorg.DownloadFile(remoteFile, localFileorg);

                                                //if (!CheckIfFileExistsOnServer("ftp://moto360.com.tr/httpdocs/ProductImages/Grid/" + filename))
                                                //{
                                                WebClient client = new WebClient();
                                                client.Credentials = new NetworkCredential(username, password);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/small/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/medium/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/large/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/xlarge/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/Grid/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/Original/" + filename, localFileorg);
                                                //   }

                                            }
                                            resimsayac++;
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                if (Baglanti.State == System.Data.ConnectionState.Open)
                    Baglanti.Close();
                Console.WriteLine("Aktarım Tamamlandı..." + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                Console.ReadLine();
            }

            XmlDocument xDocd = new XmlDocument();
            xDocd.Load("https://connect.b2bstore.com/Export/xml/Export.asmx/ExportProductXmlV2?Token=de7e29db-97e0-4400-a6f7-9dba6d3c7bac=b5e61782-9aa0-4a51-b822-62c129b9be1d&type=0");
            if (xDocd.ChildNodes.Count > 0)
            {
                if (Baglanti.State == System.Data.ConnectionState.Closed)
                    Baglanti.Open();
                foreach (XmlNode item in xDocd.ChildNodes)
                {
                    foreach (XmlNode urun in item.ChildNodes)
                    {
                        foreach (XmlNode _urun in urun.ChildNodes)
                        {
                            string StokAdi = _urun.SelectNodes("ProductName")[0].InnerText;
                            string StokKodu = _urun.SelectNodes("ProductCode")[0].InnerText;
                            string ProductID = _urun.SelectNodes("ProductID")[0].InnerText;
                            string ProductDescription = _urun.SelectNodes("ProductShortDescription")[0].InnerText;
                            string imgurl = _urun.SelectNodes("ImageUrl")[0].InnerText;
                            string price = _urun.SelectNodes("Price")[0].InnerText;
                            string TaxRatio = _urun.SelectNodes("TaxRatio")[0].InnerText;

                            string category = "";
                            foreach (XmlNode kategoriler in _urun.SelectNodes("Category")[0].ChildNodes)
                            {
                                category = kategoriler.SelectNodes("CategoryName")[0].InnerText;
                            }

                            Console.WriteLine("Kategori >  " + category + " Stok Kodu  >   " + StokKodu + "  > Stok Adı  " + StokAdi);

                            using (SqlCommand cmd = new SqlCommand("p_UrunAktar2", Baglanti))
                            {
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@kategori1", category);
                                cmd.Parameters.AddWithValue("@StokKodu", ProductID);
                                cmd.Parameters.AddWithValue("@StokAdi", StokAdi);
                                cmd.Parameters.AddWithValue("@Fiyat", Convert.ToDecimal(price) * (1 + (Convert.ToDecimal(TaxRatio) / 100)));
                                cmd.Parameters.AddWithValue("@Aciklama", StokKodu + "<br/> " + ProductDescription);

                                cmd.Parameters.AddWithValue("@Barkod", "");
                                cmd.Parameters.AddWithValue("@Beden", "");
                                cmd.Parameters.AddWithValue("@kategori2", "");
                                cmd.Parameters.AddWithValue("@kategori3", "");
                                cmd.Parameters.AddWithValue("@BedenKodu", "Moto360");
                                cmd.Parameters.AddWithValue("@Resim1", "");
                                string ProductId = Convert.ToString(cmd.ExecuteScalar());
                                if (ProductId.Length > 0)
                                {
                                    if (_urun.SelectNodes("Variation")[0].ChildNodes.Count > 0)
                                    {
                                        foreach (XmlNode bedenler in _urun.SelectNodes("Variation")[0].ChildNodes)
                                        {
                                            string barcode = bedenler.SelectNodes("Barcode")[0].InnerText;
                                            string beden = bedenler.SelectNodes("VariationValue1")[0].InnerText;
                                            string StockNumber = bedenler.SelectNodes("StockNumber")[0].InnerText;


                                            using (SqlCommand cmd2 = new SqlCommand("p_UrunVariantAktar2", Baglanti))
                                            {
                                                cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmd2.Parameters.AddWithValue("@ProductId", ProductId);
                                                cmd2.Parameters.AddWithValue("@BedenKodu", "Moto360");
                                                cmd2.Parameters.AddWithValue("@Beden", beden);
                                                cmd2.Parameters.AddWithValue("@Fiyat", Convert.ToDecimal(price) * (1 + (Convert.ToDecimal(TaxRatio) / 100)));
                                                cmd2.Parameters.AddWithValue("@Barkod", barcode);
                                                cmd2.ExecuteNonQuery();
                                            }

                                            Console.WriteLine("barkod >  " + barcode + "  > beden > " + beden + "  stok number > " + StockNumber);
                                        }
                                    }

                                    if (_urun.SelectNodes("Image").Count > 0)
                                    {
                                        int resimsayac = 0;
                                        foreach (XmlElement pointCoord in _urun.SelectNodes("Image"))
                                        {
                                            XmlNodeList xnl2 = pointCoord.SelectNodes("Image");
                                            foreach (XmlNode node2 in xnl2)
                                            {
                                                string image = node2["ImageUrl"].InnerText.Replace("https//", "https://");
                                                var remoteFile = image;
                                                string filename = Path.GetFileName(remoteFile.Replace("https://www.drdb2b.com/", ""));
                                                try
                                                {
                                                    using (SqlCommand cmdresim = new SqlCommand("p_UrunResimAktar", Baglanti))
                                                    {
                                                        cmdresim.CommandType = System.Data.CommandType.StoredProcedure;
                                                        cmdresim.Parameters.AddWithValue("@ProductId", ProductId);
                                                        cmdresim.Parameters.AddWithValue("@ImageName", filename);
                                                        cmdresim.Parameters.AddWithValue("@IsPrimaryImage", resimsayac == 0 ? 1 : 0);
                                                        cmdresim.ExecuteNonQuery();

                                                        string lastfilename = remoteFile + filename;
                                                        var localFileorg = System.IO.Directory.GetCurrentDirectory() + @"\images\" + filename;
                                                        WebClient webClientorg = new WebClient();
                                                        webClientorg.DownloadFile(remoteFile, localFileorg);

                                                        WebClient client = new WebClient();
                                                        client.Credentials = new NetworkCredential(username, password);

                                                        client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/small/" + filename, localFileorg);
                                                        client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/medium/" + filename, localFileorg);
                                                        client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/large/" + filename, localFileorg);
                                                        client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/xlarge/" + filename, localFileorg);
                                                        client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/Grid/" + filename, localFileorg);
                                                        client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/Original/" + filename, localFileorg);
                                                        resimsayac++;
                                                    }
                                                }
                                                catch (Exception)
                                                {

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var remoteFile = imgurl;
                                        string filename = Path.GetFileName(remoteFile.Replace("https://www.drdb2b.com/", "").Replace("https//", "https://"));
                                        try
                                        {
                                            using (SqlCommand cmdresim = new SqlCommand("p_UrunResimAktar", Baglanti))
                                            {
                                                cmdresim.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmdresim.Parameters.AddWithValue("@ProductId", ProductId);
                                                cmdresim.Parameters.AddWithValue("@ImageName", filename);
                                                cmdresim.Parameters.AddWithValue("@IsPrimaryImage", 1);
                                                cmdresim.ExecuteNonQuery();

                                                string lastfilename = remoteFile + filename;
                                                var localFileorg = System.IO.Directory.GetCurrentDirectory() + @"\images\" + filename;
                                                WebClient webClientorg = new WebClient();
                                                webClientorg.DownloadFile(remoteFile, localFileorg);

                                                WebClient client = new WebClient();
                                                client.Credentials = new NetworkCredential(username, password);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/small/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/medium/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/large/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/xlarge/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/Grid/" + filename, localFileorg);
                                                client.UploadFile("ftp://moto360.com.tr/httpdocs/ProductImages/Original/" + filename, localFileorg);
                                            }
                                        }
                                        catch (Exception)
                                        {


                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Baglanti.State == System.Data.ConnectionState.Open)
                    Baglanti.Close();
                Console.WriteLine("Aktarım Tamamlandı..." + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                Console.ReadLine();
            }
            return Json("");
        }

    }
}