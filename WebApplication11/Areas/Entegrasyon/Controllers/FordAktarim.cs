using NetOpenX.Rest.Client;
using NetOpenX.Rest.Client.BLL;
using NetOpenX.Rest.Client.Model;
using NetOpenX.Rest.Client.Model.Custom;
using NetOpenX.Rest.Client.Model.Enums;
using NetOpenX.Rest.Client.Model.NetOpenX;
using Newtonsoft.Json;
using RestSharp;
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
using System.Xml;
using YKPortal.Models;
using YKPortal.Models.YKClasses;

namespace YKPortal.Areas.Entegrasyon.Controllers
{
    public class FordAktarimController : Controller
    {

        public JsonResult CariAc()
        {
            oAuth2 _oAuth2;
            _oAuth2 = new oAuth2("http://178.233.83.125:7070");
            var l = new JLogin()
            {
                BranchCode = 0,
                NetsisUser = "netsis",
                NetsisPassword = "net1",
                DbType = JNVTTipi.vtMSSQL,
                DbName = "ENTERPRISE9",
                DbPassword = "",
                DbUser = "TEMELSET"
            };
            _oAuth2.Login(l);

            var _ARPsManager = new ARPsManager(_oAuth2);
            var resultPostDataCM = _ARPsManager.PostInternal(new ARPs()
            {
                CariTemelBilgi = new ARPsPrimInfo()
                {
                    CARI_KOD = "J0001",
                    CARI_ISIM = "Rest Cari 1",
                    CARI_TIP = "S",
                    Sube_Kodu = 0,
                    ACIK1 = "acik1",
                    ACIK2 = "acik2",
                    ACIK3 = "acik3",
                    CARI_ADRES = "izmir",
                    CARI_IL = "izmir",
                    CARI_TEL = "2322225566",
                    CARI_ILCE = "ksk",
                    EMAIL = "oner.kaya@logo.com.tr",
                    WEB = "www.logo.com.tr",
                    CM_RAP_TARIH = DateTime.Now.AddMonths(1)
                },
                CariEkBilgi = new ARPsSuppInfo()
                {
                    CARI_KOD = "J0001",
                    TcKimlikNo = "12345678911",
                    Kull1N = 1,
                    Kull1S = "1",
                    S_Yedek1 = "syedek1"
                }
            });

            if (resultPostDataCM.IsSuccessful)
            {
                var carikod = resultPostDataCM.Data.CariTemelBilgi.CARI_KOD;
            }
            else
            {
                var hata = (resultPostDataCM.ErrorDesc);

            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AnaSayfa(DateTime? Tarih = null)
        {
            if (!AutoGirisKontrol())
                return Redirect("~/YK/Giris");
            if (Tarih == null)
            {
                Tarih = DateTime.Today;
            }
            ViewBag.Tarih = Tarih;
            if (Tarih != null)
            {
                var access_Token = "";
                {
                    var options = new RestClientOptions("https://login.ford.com.tr")
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("/Oauth2/Oauth/Token", Method.Post);
                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    request.AddHeader("Cookie", "FOSecurity=U=bhEblFsU%2fDEpxvM7ECAE0K9MDAC7zeNlrglzdB%2fkaiXD57rwBT9cKfn8T6AYXPOb&TSLE=202409141303; FOSecurityKS=U=omdl2HHZD7pBV9C93XUNivQoCDlUltRHr6WPg%2bQyipp3qJFamox1VWWWnBZ6rMQP&TSLE=202409141303; TS016c9d30=01871ecc045329913d9cdfdfaf53d22383575c0df2d3e3920ffad1766ca4a0446869fbac07bc6ae712c3e2a1459cc246b8ebfef8a9; TS0186149b=01871ecc04f1d198761cf4ce87d54fd908796cf6e3b2e2d96cb8ea2aa72c0170245263ac81f0c6f735746ed14c6dd99817988ef05c; ASP.NET_SessionId=40yvgpy1xb5dpb2sh4vn2o2r; fcookie=1426205962.47873.0000");
                    request.AddParameter("grant_type", "refresh_token");
                    request.AddParameter("refresh_token", "zy_0JDeuRIHABIEuhXRY7rLAS0_OYD0_MHa0ngPRvhtfPIXPafrMkgH0tu_gh3Y4cIQEqpcXJ-XMDTqn2eGr3OsATUg8bhGR-chtEpDjJQp0TU6H92emgk8VqnZcq4pI0d0yp9hDemmyTzis1kUFwuY1dgT1Kp2z-6dPWSz4_M9BfBepBfR53itBN2V-2uBMXAP6OVAhcesImTvfk9zbteM8nDJGFFMUl5UgpSVoPJ4rdec21WcWROgT_SSs5RHAp_m3prq4DEAY9hty7PgigyU5VLFkAE7UJc1ZNEcPO0N_K35aP5YrqjeixVKPv8OK8QFG5dnbriYqa1o6a60W6IoUNb-YbzZ4TqtEwMP74iJxofSbJtUYoSsWom6oxFo1PwLYnqe10Ir1UZlg626q3yZgOP_axM0GKBBxRZuDsgdKGyM1FIrWT9D_ETfh_jXMTmK-qQPRrz48canXsGQNxlyf_loeNs_OqsaTgR37NX3E4fHzH_BLF2Qz_iQwMTH6BzoQKBUk9aABpHZhqvFz8WyrVV2WIe4m9DuAYie1M5pef_5h9xZ2gkv9r2oE5tHQBhxANypgSECHop18VzwLrit9CLb3z70ZSiPZ4Av_warbknHaI4bQDQbWP2N95BGW");
                    request.AddParameter("client_id", "134021b5daf34bada5b71b7a623b4053.login.ford.com.tr");
                    request.AddParameter("client_secret", "00ba4fc3-a897-4787-9159-dc6820c30e46");
                    RestResponse response = client.Execute(request);
                    string jsonOturum = (response.Content);
                    //dynamic result = JsonConvert.DeserializeObject<T>(jsonOturum);
                    dynamic result = JsonConvert.DeserializeObject(jsonOturum);

                    access_Token = result.access_token.Value;
                }
                string dosya = Server.MapPath("~/Uploads/Dosyalar/Entegrasyon_" + Convert.ToDateTime(Tarih).ToString("yyyy-MM-dd") + "_" + Guid.NewGuid().ToString() + ".txt");
                {
                    var options = new RestClientOptions("https://api.ford.com.tr")
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("/ws/rest/Accounting/SalesInvoice", Method.Post);
                    request.AddHeader("Authorization", "Basic YWNjb3VudGluZ0Bmb3JkLUI0WlNZUzphNmIyN2Y5My1kMjEwLTQ2NDAtOGExOC0wMGQ2YzM1ZGQ3OGQ=");
                    request.AddHeader("foauth", access_Token);
                    request.AddHeader("Content-Type", "application/json");
                    //request.AddHeader("Cookie", access_Token);
                    var body = @"{""InvoiceDate"": """ + Convert.ToDateTime(Tarih).ToString("dd.MM.yyyy") + @"""}";
                    request.AddStringBody(body, DataFormat.Json);
                    RestResponse response = client.Execute(request);
                    string jsonKayitlar = (response.Content);
                    System.IO.File.WriteAllText(dosya, jsonKayitlar);
                }


                XmlDocument IDOCXML = new XmlDocument();
                XmlNode IDOCchildNode;
                XmlNode IDOCchildNode2;
                XmlNode IDOCchildNode3;
                XmlNode IDOCchildNode_Musteri;
                XmlNode IDOCchildNode_Node_4;
                DataTable dtBelgeler = new DataTable();
                string basliklar = "|BSec|SAkt|SModul|STipi|SDöviz|SSat.Sayisi|SFaturaNo|SNetsis FaturaNo|SIşEmriNo|STarih|STutar|SCari Kodu|SNetsis Cari Kodu|SÜnvan|SŞehir|SPlaka-KM|SDosya No|SSiparis No|SAktarım Durumu|SVDaire|SVkn|SEmail|STelefon|SAdres|SUlke|SMüşteri No";
                foreach (string item in basliklar.Split('|'))
                {
                    if (item.Trim().Length > 0)
                    {
                        dtBelgeler.Columns.Add(item);
                    }
                }
                string text2 = "";

                try
                {
                    if (dosya != "")
                    {
                        StreamReader streamReader = new StreamReader(dosya, Encoding.GetEncoding("utf-8"));
                        string xml = streamReader.ReadToEnd();
                        streamReader.Close();
                        IDOCXML.LoadXml(xml);
                        if (IDOCXML.ChildNodes[1].Name == "_-FO_-INVOICE_CONTAINER")
                        {
                            XmlNodeList xmlNodeList = IDOCXML.SelectNodes("_-FO_-INVOICE_CONTAINER");
                            XmlNode xmlNode = xmlNodeList.Item(0);
                            int num = 0;
                            for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
                            {
                                IDOCchildNode = xmlNode.ChildNodes.Item(i);
                                if (IDOCchildNode.Name == "_-FO_-EF_UBL21TR12")
                                {
                                    string text3 = "";
                                    string value = "";
                                    string text4 = "";
                                    string value2 = "";
                                    string value3 = "";
                                    string text5 = "";

                                    string _Xml_Cari_Ismi = "";
                                    string _Xml_Cari_Adres = "";
                                    string _Xml_Cari_Email = "";
                                    string _Xml_Cari_Vd = "";
                                    string _Xml_Cari_Vkn = "";
                                    string _Xml_Cari_MusteriNo = "";
                                    string _Xml_Cari_Telefon = "";
                                    string _Xml_Ulke = "";
                                    string text7 = "0";
                                    string _Xml_Cari_Il = "";
                                    string text8 = "";
                                    string value6 = "";
                                    string value7 = "";
                                    int num2 = 0;
                                    for (int j = 0; j < IDOCchildNode.ChildNodes.Count; j++)
                                    {
                                        IDOCchildNode2 = IDOCchildNode.ChildNodes.Item(j);
                                        text2 = "_-ISISTR_";
                                        if (IDOCchildNode2.Name.StartsWith("_-FO_"))
                                        {
                                            text2 = "_-FO_";
                                        }
                                        if (IDOCchildNode2.Name == text2 + "-EF_REFERENCE")
                                        {
                                            value = IDOCchildNode2.SelectSingleNode("DOCUMENT_MODUL_CODE").InnerText;
                                            if (text2 == "_-ISISTR_")
                                            {
                                                text3 = IDOCchildNode2.SelectSingleNode("DOCUMENT_UNIQUE_KEY").InnerText;
                                            }
                                        }
                                        if (IDOCchildNode2.Name == text2 + "-EF_HEADER")
                                        {
                                            text4 = IDOCchildNode2.SelectSingleNode("INVOICEID").InnerText;
                                            value2 = IDOCchildNode2.SelectSingleNode("INVOICETYPECODE").InnerText;
                                            value3 = IDOCchildNode2.SelectSingleNode("DOCUMENTCURRENCYCODE").InnerText;
                                            text5 = IDOCchildNode2.SelectSingleNode("ISSUEDATE").InnerText;
                                            num2 = Convert.ToInt32(IDOCchildNode2.SelectSingleNode("LINECOUNTNUMERIC").InnerText.Trim());
                                        }
                                        if (IDOCchildNode2.Name == text2 + "-EF_HEADERNOTE")
                                        {
                                            if (text2 == "_-ISISTR_")
                                            {
                                                if (IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Contains("[Fatura Adresi]"))
                                                {
                                                    _Xml_Cari_Adres = IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Replace("[Fatura Adresi]", "");
                                                }

                                                if (IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Contains("[Plaka]"))
                                                {
                                                    text8 = IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Replace("[Plaka] ", "");
                                                }
                                                if (IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Contains("[Km]"))
                                                {
                                                    text8 = text8 + "-" + IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Replace("[Km] ", "");
                                                }
                                                if (IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Contains("[Fatura Notu] DOSYA NO:"))
                                                {
                                                    value6 = IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Replace("[Fatura Notu] DOSYA NO:", "");
                                                }
                                                if (IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Contains("[Sipariş No]"))
                                                {
                                                    value7 = IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Replace("[Sipariş No] ", "");
                                                }
                                            }
                                            if (text2 == "_-FO_")
                                            {
                                                if (IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Contains("#[Araç Vin No]"))
                                                {
                                                    text8 = IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Replace("#[Araç Vin No] ", "");
                                                }
                                                if (IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Contains("#[Motor No]"))
                                                {
                                                    text8 = text8 + "-" + IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Replace("#[Motor No] ", "");
                                                }
                                                if (IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Contains("#[Satın Alma Sipariş Fat.No]"))
                                                {
                                                    value6 = IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Replace("#[Satın Alma Sipariş Fat.No]", "");
                                                }
                                                if (IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Contains("#[Satın Alma Sip. No]"))
                                                {
                                                    value7 = IDOCchildNode2.SelectSingleNode("VALUE").InnerText.Replace("#[Satın Alma Sip. No] ", "");
                                                }
                                            }
                                        }
                                        if (IDOCchildNode2.Name == text2 + "-EF_PARTY")
                                        {
                                            IDOCchildNode3 = IDOCchildNode2.ChildNodes.Item(0);
                                            if (IDOCchildNode3.Name == "PARTYTYPE")
                                            {
                                                IDOCchildNode_Musteri = IDOCchildNode3.ChildNodes.Item(0);
                                                if (IDOCchildNode_Musteri.InnerText == "CUSTOMER")
                                                {
                                                    for (int k = 0; k < IDOCchildNode2.ChildNodes.Count; k++)
                                                    {
                                                        string _Baslik_Bilgisi = Convert.ToString(IDOCchildNode_Musteri.Name);

                                                        IDOCchildNode_Musteri = IDOCchildNode2.ChildNodes.Item(k);
                                                        if (IDOCchildNode_Musteri.Name == "PARTYNAME")
                                                        {
                                                            _Xml_Cari_Ismi = IDOCchildNode2.SelectSingleNode("PARTYNAME").InnerText;
                                                        }

                                                        if (IDOCchildNode_Musteri.Name == "COUNTRYNAME")
                                                        {
                                                            _Xml_Ulke = IDOCchildNode2.SelectSingleNode("COUNTRYNAME").InnerText;
                                                        }
                                                        if (IDOCchildNode_Musteri.Name == "PARTYTAXSCHEME")
                                                        {
                                                            _Xml_Cari_Vd = IDOCchildNode2.SelectSingleNode("PARTYTAXSCHEME").InnerText;
                                                        }


                                                        if (IDOCchildNode_Musteri.Name == "CITYNAME")
                                                        {
                                                            _Xml_Cari_Il = IDOCchildNode2.SelectSingleNode("CITYNAME").InnerText;
                                                        }
                                                        if (IDOCchildNode_Musteri.Name == text2 + "-EF_PARTYID")
                                                        {
                                                            string _Secilen_Satir = IDOCchildNode_Musteri.SelectSingleNode("PARTYIDENTIFICATIONID").InnerText;
                                                            if (_Secilen_Satir == "TCKN")
                                                            {
                                                                if (IDOCchildNode_Musteri.HasChildNodes)
                                                                {
                                                                    string _Tc = Convert.ToString(IDOCchildNode_Musteri.SelectSingleNode("VALUE").InnerText);
                                                                    {
                                                                        _Xml_Cari_Vkn = _Tc;
                                                                    }
                                                                }
                                                            }
                                                            if (_Secilen_Satir == "VKN")
                                                            {
                                                                if (IDOCchildNode_Musteri.HasChildNodes)
                                                                {
                                                                    string _Vkn = Convert.ToString(IDOCchildNode_Musteri.SelectSingleNode("VALUE").InnerText);
                                                                    {
                                                                        _Xml_Cari_Vkn = _Vkn;
                                                                    }
                                                                }
                                                            }
                                                            if (_Secilen_Satir == "MUSTERINO")
                                                            {
                                                                if (IDOCchildNode_Musteri.HasChildNodes)
                                                                {
                                                                    string text9 = IDOCchildNode_Musteri.SelectSingleNode("VALUE").InnerText.Trim();
                                                                    if (text9 != "")
                                                                    {
                                                                        _Xml_Cari_MusteriNo = text9;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (IDOCchildNode_Musteri.Name == text2 + "-EF_CONTACT")
                                                        {
                                                            if (IDOCchildNode_Musteri.ChildNodes.Count >= 1)
                                                            {
                                                                for (int iletisim = 0; iletisim < IDOCchildNode_Musteri.ChildNodes.Count; iletisim++)
                                                                {
                                                                    IDOCchildNode_Node_4 = IDOCchildNode_Musteri.ChildNodes.Item(iletisim);

                                                                    if (IDOCchildNode_Node_4.Name == "TELEPHONE")
                                                                    {
                                                                        _Xml_Cari_Telefon = IDOCchildNode_Musteri.SelectSingleNode("TELEPHONE").InnerText;
                                                                    }
                                                                    if (IDOCchildNode_Node_4.Name == "ELECTRONICMAIL")
                                                                    {
                                                                        _Xml_Cari_Email = IDOCchildNode_Musteri.SelectSingleNode("ELECTRONICMAIL").InnerText;
                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (IDOCchildNode2.Name == text2 + "-EF_LEGALMONETARYTOT")
                                        {
                                            for (int l = 0; l < IDOCchildNode2.ChildNodes.Count; l++)
                                            {
                                                IDOCchildNode_Musteri = IDOCchildNode2.ChildNodes.Item(l);
                                                if (IDOCchildNode_Musteri.Name == "TAXINCLUSIVEAMOUNT")
                                                {
                                                    text7 = IDOCchildNode2.SelectSingleNode("TAXINCLUSIVEAMOUNT").InnerText;
                                                }
                                            }
                                        }
                                    }
                                    if (!(text7 == "0"))
                                    {
                                        DataRow dataRow = dtBelgeler.NewRow();
                                        dataRow[0] = 0;
                                        dataRow[1] = "";
                                        dataRow[2] = value;
                                        dataRow[3] = value2;
                                        dataRow[4] = value3;
                                        dataRow[5] = num2.ToString();
                                        dataRow[6] = text3;
                                        dataRow[7] = "";
                                        dataRow[8] = text4;
                                        if (text5.IndexOf('-') <= 0 && text5.Length == 8)
                                        {
                                            text5 = text5.Substring(0, 4) + "-" + text5.Substring(4, 2) + "-" + text5.Substring(6, 2);
                                        }
                                        dataRow[9] = text5;
                                        dataRow[10] = text7;
                                        dataRow[11] = _Xml_Cari_Vkn;
                                        if (CariKontrol(_Xml_Cari_Vkn))
                                        {
                                            dataRow[12] = _Xml_Cari_Vkn;
                                        }
                                        else
                                        {
                                            dataRow[12] = "";
                                        }
                                        dataRow[13] = _Xml_Cari_Ismi;
                                        dataRow[14] = _Xml_Cari_Il;
                                        dataRow[15] = text8;
                                        dataRow[16] = value6;
                                        dataRow[17] = value7;
                                        dataRow[18] = "";
                                        dataRow[19] = _Xml_Cari_Vd;
                                        dataRow[20] = _Xml_Cari_Vkn;
                                        dataRow[21] = _Xml_Cari_Email;
                                        dataRow[22] = _Xml_Cari_Telefon;
                                        dataRow[23] = _Xml_Cari_Adres;
                                        dataRow[24] = _Xml_Ulke;
                                        dataRow[25] = _Xml_Cari_MusteriNo;
                                        dtBelgeler.Rows.Add(dataRow);
                                        string text10 = BelgeKontrol(text4, _Xml_Cari_Vkn, text3);
                                        if (text10 != "")
                                        {
                                            dataRow[1] = "M";
                                            dataRow[7] = text10;
                                        }
                                        num++;
                                    }
                                }
                            }
                            //Not_Bilgisi_Ekle("Toplam: " + num.ToString() + " kayıt okundu.");
                        }
                    }

                    ViewBag.dtBelgeler = dtBelgeler;
                }
                catch (Exception ex)
                {
                    
                }

            }


            return View();
        }

        private string BelgeKontrol(string IsEmriNo, string CariKodu, string FatNo)
        {
            string NetsisDatabase = YKUtils.ParametreGetir(GetCookie("UyelikID"),"NetsisDatabase");
            string text = "";
            string result = "";
            text = "SELECT FATIRSNO FROM " + NetsisDatabase + "..TBLFATUEK WITH (NOLOCK) WHERE  ACIK14='" + FatNo + "' AND isnull(ACIK15,'')='" + IsEmriNo + "'";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = text;
            cmd.CommandType = CommandType.Text;
            result = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
            
            return result;
        }

        private bool CariKontrol(string CariKodu)
        {
            string NetsisDatabase = YKUtils.ParametreGetir(GetCookie("UyelikID"), "NetsisDatabase");
            string strQuery = "SELECT COUNT(*) FROM " + NetsisDatabase + "..TBLCASABIT WHERE CARI_KOD='" + CariKodu.Trim() + "' OR VERGI_NUMARASI IN(SELECT TOP 1 CARI_KOD FROM " + NetsisDatabase + "..TBLCASABIT WHERE CARI_KOD LIKE '2%')";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strQuery;
            cmd.CommandType = CommandType.Text;
            DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            foreach (DataRow row in dt.Rows)
            {
                if (!Convert.IsDBNull(row[0]) && Convert.ToInt32(row[0].ToString()) > 0)
                {
                    return true;
                }
            }
            return false;
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

}