using iText.Commons.Bouncycastle.Asn1.X509;
using iText.IO.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using YKPortal.Models;
using YKPortal.Models.Dto;
using YKPortal.Models.WebApiModels;
using YKPortal.Models.YKClasses;

namespace YKPortal.Controllers
{
    public class YKWebApiController : ApiController
    {

        #region Mobile App Sipariş Metodları



        #endregion

        #region Mobile App Metodları

        public IDJsonResult Subeler([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["LisansNumarasi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! LisansNumarasi bilgisi boş olamaz.";
                    return result;
                }
                string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "OYP_Subeler";
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.Kod = Convert.ToString(satir["Kod"]);
                        entity.Isim = Convert.ToString(satir["Isim"]);
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #region Netsis_StokEkBilgi_Kaydet
        public IDJsonResult Netsis_StokEkBilgi_Kaydet([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }

                if (data["Kullanici_Adi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici_Adi bilgisi boş olamaz.";
                    return result;
                }
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string _Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string _Kull1s = Convert.ToString(data["Kull1S"]);
                string _Kull2s = Convert.ToString(data["Kull2S"]);
                string _Kull3s = Convert.ToString(data["Kull3S"]);
                string _Barkod1 = Convert.ToString(data["BARKOD1"]);
                string _Barkod2 = Convert.ToString(data["BARKOD2"]);
                string _Barkod3 = Convert.ToString(data["BARKOD3"]);
                string _ONCEKI_KOD = Convert.ToString(data["ONCEKI_KOD"]);
                string Kullanici_Adi = Convert.ToString(data["Kullanici_Adi"]);


                string _srg = "UPDATE " + Uygulama_Db + ".[dbo].[TBLSTSABITEK] ";
                _srg += " \r\n SET ";
                _srg += " \r\n KULL1S = left('" + _Kull1s + "',50) ";
                if (_Kull2s != "")
                {
                    _srg += " , \r\n KULL2S = left('" + _Kull2s + "',50) ";
                }
                if (_Kull3s != "")
                {
                    _srg += " , \r\n KULL3S = left('" + _Kull3s + "',50) ";
                }
                _srg += " \r\n , DUZELTMEYAPANKUL = left('" + Kullanici_Adi + "',8), DUZELTMETARIHI = getdate()  ";
                _srg += " \r\n WHERE STOK_KODU = '" + _Stok_Kodu + "' ";

                _srg += " \r\n UPDATE " + Uygulama_Db + ".[dbo].[TBLSTSABIT] ";
                _srg += " \r\n SET ";
                _srg += " \r\n BARKOD1 = left('" + _Barkod1 + "',50) ";
                if (_Barkod2 != "")
                {
                    _srg += " , \r\n BARKOD2 = left('" + _Barkod2 + "',50) ";
                }
                if (_Barkod3 != "")
                {
                    _srg += " , \r\n BARKOD3 = left('" + _Barkod3 + "',50) ";
                }
                if (_ONCEKI_KOD != "")
                {
                    _srg += " , \r\n ONCEKI_KOD = left('" + _ONCEKI_KOD + "',50) ";
                }
                _srg += " \r\n WHERE STOK_KODU = '" + _Stok_Kodu + "' ";

                _srg += " \r\n INSERT INTO INNOVA..TBLLOGUSER ";
                _srg += " ( FORM, TARIH, KAYITID ";
                _srg += " , KULLANICI ";
                _srg += " , BILGI, ISLEM, KAYNAK) ";
                _srg += " \r\n SELECT 'Web Servis', getdate(), '" + _Stok_Kodu + "' ";
                _srg += " \r\n , '" + Kullanici_Adi + "' AS KULLANICI ";
                _srg += " \r\n , '" + _Kull1s + ':' + _Kull2s + ':' + _Kull3s + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_StokEkBilgi_Kaydet' AS KAYNAK ";
                _srg += " \r\n UNION ALL ";
                _srg += " \r\n SELECT 'Web Servis', getdate(), '" + _Stok_Kodu + "' ";
                _srg += " \r\n , '" + Kullanici_Adi + "' AS KULLANICI ";
                _srg += " \r\n , '" + _Barkod1 + ':' + _Barkod2 + ':' + _Barkod3 + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_StokEkBilgi_Kaydet' AS KAYNAK ";
                _srg += " \r\n UNION ALL ";
                _srg += " \r\n SELECT 'Web Servis', getdate(), '" + _Stok_Kodu + "' ";
                _srg += " \r\n , '" + Kullanici_Adi + "' AS KULLANICI ";
                _srg += " \r\n , '" + _ONCEKI_KOD + "' AS BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_StokEkBilgi_Kaydet' AS KAYNAK ";
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #region Netsis_Plastik_Okutma_YeniFisno
        public IDJsonResult Netsis_Plastik_Okutma_YeniFisno([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250808";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {
                    _srg = " ";
                    _srg += " \r\n SELECT ISNULL((select MAX(FISNO2) +1 from [INNOVA].[dbo].[TBLOKUTMA] WITH (NOLOCK) WHERE ISNUMERIC(FISNO2) = 1 ) ,1) FISNO  ";
                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.FISNO = Convert.ToString(satir["FISNO"]);
                        entity.Servis_Versiyon = 250808;
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    result.Sonuc_Versiyon = 250806;
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion Netsis_Plastik_Okutma_YeniFisno

        #region Netsis_Plastik_Okutma_Listele
        public IDJsonResult Netsis_Plastik_Okutma_Listele([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250808";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Okutma_No"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Okutma_No bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);

                string _Okutma_No = Convert.ToString(data["Okutma_No"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {
                    _srg = " ";
                    _srg += " \r\n SELECT FISNO2 FISNO, BARKOD, ADET, KG ";
                    _srg += " \r\n FROM [INNOVA].[dbo].[TBLOKUTMA] WITH (NOLOCK) ";
                    _srg += " \r\n WHERE FISNO2 = '" + _Okutma_No + "' ";
                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.BARKOD = Convert.ToString(satir["BARKOD"]);
                        entity.ADET = Convert.ToString(satir["ADET"]);
                        entity.KG = Convert.ToString(satir["KG"]);
                        entity.Servis_Versiyon = 250808;
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    result.Sonuc_Versiyon = 250806;
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Plastik_Okutma_Listele
        #region Netsis_Plastik_Okutma_Kaydet
        public IDJsonResult Netsis_Plastik_Okutma_Kaydet([FromBody] JObject data)
        {
            string _GuidKey = Guid.NewGuid().ToString();
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Okutma_No"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Okutma_No bilgisi boş olamaz.";
                    return result;
                }
                if (data["Barkod"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Barkod bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string _Okutma_No = Convert.ToString(data["Okutma_No"]);
                string _Barkod = Convert.ToString(data["Barkod"]);
                string _Kullanici = Convert.ToString(data["Kullanici"]);


                string _srg = " \r\n INSERT INTO INNOVA..TBLOKUTMA ";
                _srg += " \r\n ( FISNO, FISNO2, BARKOD ) ";
                _srg += " \r\n SELECT '" + _Okutma_No + "' FISNO, '" + _Okutma_No + "' FISNO2, '" + _Barkod + "' BARKOD  ";

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = 250806;
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Sonuc_Versiyon = -250806;
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion Netsis_Plastik_Okutma_Kaydet


        #region Netsis_Wms_Basit_Uretim_Kaydet
        public IDJsonResult Netsis_Wms_Basit_Uretim_Kaydet([FromBody] JObject data)
        {
            string _GuidKey = Guid.NewGuid().ToString();
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Sube_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Sube Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Miktar"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Miktar bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string _Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string _Miktar = Convert.ToString(data["Miktar"]);
                string Kullanici_Adi = Convert.ToString(data["Kullanici_Adi"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                if (Sube_Kodu == "")
                {
                    Sube_Kodu = "0";
                }
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                if (Depo_Kodu == "")
                {
                    Depo_Kodu = "0";
                }

                string _srg = " INSERT INTO [INNOVA].[dbo].[TBLBELGE_KAYIT] ";
                _srg += " ( ";
                _srg += " \r\n [GUIDID], SUBE_KODU, BELGE_NO, FTIRSIP, CARI_KODU, [STOK_KODU] ";
                _srg += " \r\n , MIKTAR, MIKTAR2, MIKTAR3, ADET ";
                _srg += " \r\n , FIYAT ";
                _srg += " \r\n , SERI_NO ";
                _srg += " \r\n , [GIT_DEPO_KODU], [DEPO_KODU] ";
                _srg += " \r\n , [PLASIYER_KODU], TARIH, ACIKLAMA ";
                _srg += " \r\n , KALEM_ACIKLAMA1, KALEM_ACIKLAMA2 ";
                _srg += " \r\n , KAYIT_KULLANICI, KAYIT_TARIHI ";
                _srg += " \r\n , TIP, TIP_ACIKLAMA ";
                _srg += " \r\n , GCKOD  ";
                _srg += " \r\n ) ";
                _srg += " \r\n SELECT '" + _GuidKey + "' [GUIDID], '" + Sube_Kodu + "' SUBE_KODU, LEFT('" + _GuidKey + "',15) BELGE_NO, 'U' FTIRSIP, '' CARI_KODU, '" + _Stok_Kodu + "' AS STOK_KODU ";
                _srg += " \r\n , replace(replace('" + _Miktar + "','.',''),',','.') MIKTAR, replace(replace('" + _Miktar + "','.',''),',','.') MIKTAR2, 0 MIKTAR3 , 1 ADET ";
                _srg += " \r\n , 0 AS FIYAT ";
                _srg += " \r\n , '' as SERI_NO ";
                _srg += " \r\n , '" + Depo_Kodu + "' GIT_DEPO_KODU, '" + Depo_Kodu + "' DEPO_KODU ";
                _srg += " \r\n , '' PLASIYER_KODU, '" + DateTime.Now.ToString("yyyy/MM/dd") + "', '' ";
                _srg += " \r\n , '' KALEM_ACIKLAMA1, '' KALEM_ACIKLAMA2 ";
                _srg += " \r\n , '" + Kullanici_Adi + "' KAYIT_KULLANICI, getdate() KAYIT_TARIHI ";
                _srg += " \r\n , 100 TIP, 'Üretim Kaydi' TIP_ACIKLAMA ";
                _srg += " \r\n , 'G' AS GCKOD ";
                _srg += " \r\n ";

                _srg += " \r\n EXEC [" + Uygulama_Db + "].[dbo].[INN_PR_URETIM_KAYDET_REC] '" + _GuidKey + "', '" + _Stok_Kodu + "' ";


                _srg += " \r\n INSERT INTO [INNOVA].[dbo].[TBLLOGUSER] ";
                _srg += " \r\n ( FORM, TARIH, KAYITID ";
                _srg += " \r\n , KULLANICI ";
                _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                _srg += " \r\n SELECT 'Üretim Kaydı', getdate(), '" + _Stok_Kodu + "' ";
                _srg += " \r\n , '" + Kullanici_Adi + "' AS KULLANICI ";
                _srg += " \r\n , '" + _Stok_Kodu + "' BILGI, 'Üretim Kaydı' as ISLEM, 'Netsis_Wms_Basit_Uretim_Kaydet' AS KAYNAK ";



                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = 250806;
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Sonuc_Versiyon = -250806;
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion Netsis_Wms_Basit_Uretim_Kaydet
        #region Netsis_Wms_Basit_Uretim_Listele
        public IDJsonResult Netsis_Wms_Basit_Uretim_Listele([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250806";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Sube_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Sube Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                if (Depo_Kodu == "")
                {
                    Depo_Kodu = "0";
                }
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {
                    _srg = " ";
                    _srg += " \r\n  -- Netsis_Wms_Qr_Listele ";
                    _srg += " \r\n SELECT TOP 10 UR.URETSON_MAMUL as STOK_KODU, DBO.TRK1( ST.STOK_ADI) STOK_ADI, URETSON_MIKTAR, URETSON_FISNO ";
                    _srg += " \r\n FROM [" + Uygulama_Db + "].[dbo].[TBLSTOKURS] UR WITH (NOLOCK) ";
                    _srg += " \r\n INNER JOIN [" + Uygulama_Db + "].[dbo].[TBLSTSABIT] ST WITH (NOLOCK) ON ST.STOK_KODU = UR.URETSON_MAMUL ";
                    _srg += " \r\n WHERE 1=1 ";
                    if (Sube_Kodu != "")
                    {
                        _srg += " \r\n AND UR.SUBE_KODU = '" + Sube_Kodu + "' ";
                    }
                    _srg += " \r\n ORDER BY UR.URETSON_TARIH DESC, URETSON_FISNO DESC ";

                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Stok_Kodu = Convert.ToString(satir["STOK_KODU"]);
                        entity.Stok_Adi = Convert.ToString(satir["STOK_ADI"]);
                        entity.Miktar = Convert.ToString(satir["URETSON_MIKTAR"]);
                        entity.Fisno = Convert.ToString(satir["URETSON_FISNO"]);
                        entity.Servis_Versiyon = 250624;
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    result.Sonuc_Versiyon = 250806;
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Basit_Uretim_Listele


        #region Netsis_Wms_Stok_Ara
        public IDJsonResult Netsis_Wms_00_Stok_Ara([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250806";
            string _srg = "";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Barkod = Convert.ToString(data["Barkod"]);
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                if (Depo_Kodu == "")
                {
                    Depo_Kodu = "0";
                }

                string Kullanici = Convert.ToString(data["Kullanici"]);
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                {
                    if (Uygulama == "NETSIS")
                    {
                        _srg = " EXEC [" + Uygulama_Db + "].[dbo].[INN_PR_STOK_BILGI_GETIR] '" + Barkod + "', '', '" + Depo_Kodu + "' ";
                    }

                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Stok_Kodu = Convert.ToString(satir["STOK_KODU"]);
                        entity.Stok_Adi = Convert.ToString(satir["STOK_ADI"]);
                        entity.Birim = Convert.ToString(satir["BIRIM"]);
                        entity.KDV_ORANI = Convert.ToString(satir["KDV_ORANI"]);
                        entity.Miktar = Convert.ToString(satir["MIKTAR"]);
                        entity.TARTI = Convert.ToString(satir["TARTI"]);
                        entity.Barkod = Convert.ToString(Barkod);

                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    result.Sonuc_Versiyon = 280608;
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    result.Sonuc_Versiyon = -280608;
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion Netsis_Wms_Stok_Ara


        public IDJsonResult Netsis_StokEkBilgi_Listele([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                //if (data["LisansNumarasi"] == null)
                //{
                //    result.SonucKodu = 0;
                //    result.Hata = "UYARI! LisansNumarasi bilgisi boş olamaz.";
                //    return result;
                //}
                //string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Kisit = Convert.ToString(data["Kisit"]);
                string _Belge_Barkod = Convert.ToString(data["Barkod"]);

                string Kullanici_Guid = Convert.ToString(data["Kullanici_Guid"]);
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                {
                    if (Uygulama == "NETSIS")
                    {
                        string _srg = " SELECT ST.STOK_KODU, STOK_ADI +'('+ ISNULL(EK.KULL1S,'') +' '+ ISNULL(EK.KULL2S,'') +' '+ ISNULL(EK.KULL3S,'')+')' as STOK_ADI ";
                        _srg += " , KULL1S, KULL2S, KULL3S,ST.BARKOD1,ST.BARKOD2, ST.BARKOD3, ST.ONCEKI_KOD ";
                        _srg += " FROM " + Uygulama_Db + "..TBLSTSABIT ST WITH (NOLOCK) ";
                        _srg += " INNER JOIN " + Uygulama_Db + "..TBLSTSABITEK EK WITH (NOLOCK) ON ST.STOK_KODU = EK.STOK_KODU ";
                        _srg += " WHERE 'Netsis_StokEk_250526' = 'Netsis_StokEk_250526'";
                        _srg += " AND (ST.STOK_KODU = '" + _Belge_Barkod + "' or ST.BARKOD1 = '" + _Belge_Barkod + "' or ST.BARKOD2 = '" + _Belge_Barkod + "' or ST.BARKOD3 = '" + _Belge_Barkod + "' OR URETICI_KODU = '" + _Belge_Barkod + "' )";


                        cmd.CommandText = _srg;
                        if (Kisit != "")
                        {
                            cmd.CommandText += Kisit;
                        }
                    }

                }

                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Stok_Kodu = Convert.ToString(satir["STOK_KODU"]);
                        entity.Stok_Adi = Convert.ToString(satir["STOK_ADI"]);
                        entity.Kull1S = Convert.ToString(satir["KULL1S"]);
                        entity.Kull2S = Convert.ToString(satir["KULL2S"]);
                        entity.Kull3S = Convert.ToString(satir["KULL3S"]);
                        entity.Barkod1 = Convert.ToString(satir["BARKOD1"]);
                        entity.Barkod2 = Convert.ToString(satir["BARKOD2"]);
                        entity.Barkod3 = Convert.ToString(satir["BARKOD3"]);
                        entity.ONCEKI_KOD = Convert.ToString(satir["ONCEKI_KOD"]);


                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion
        public IDJsonResult Sabit_Listeler([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                //if (data["LisansNumarasi"] == null)
                //{
                //    result.SonucKodu = 0;
                //    result.Hata = "UYARI! LisansNumarasi bilgisi boş olamaz.";
                //    return result;
                //}
                //string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Donem_Kodu = Convert.ToString(data["Donem_Kodu"]);
                string Islem_Tipi = Convert.ToString(data["Islem_Tipi"]);
                string Kisit = Convert.ToString(data["Kisit"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {
                    if (Islem_Tipi == "Cariler")
                    {
                        cmd.CommandText = "SELECT TOP 50 CARI_KOD AS ID, CARI_KOD AS Kod, CARI_ISIM AS Isim ";
                        cmd.CommandText += " FROM [" + Uygulama_Db + "].[dbo].OYG_NV_CARI_KART ";
                        cmd.CommandText += " WHERE 'Cari' = 'Cari'";
                        if (Kisit != "")
                        {
                            cmd.CommandText += Kisit;
                        }
                    }
                    if (Islem_Tipi == "Stoklar")
                    {
                        cmd.CommandText = "SELECT TOP 50 STOK_KODU AS ID, STOK_KODU AS Kod, STOK_ADI AS Isim ";
                        cmd.CommandText += " FROM [" + Uygulama_Db + "].[dbo].OYG_NV_STOK_KART ";
                        cmd.CommandText += " WHERE 'Stok' = 'Stok'";
                        if (Kisit != "")
                        {
                            cmd.CommandText += Kisit;
                        }
                    }
                    if (Islem_Tipi == "Subeler")
                    {
                        cmd.CommandText = "SELECT TOP 50 SUBE_KODU AS ID, SUBE_KODU AS Kod, UNVAN AS Isim ";
                        cmd.CommandText += " FROM [" + Uygulama_Db + "].[dbo].INN_VW_SUBELER ";
                        cmd.CommandText += " WHERE 'Sube' = 'Sube'";
                        if (Kisit != "")
                        {
                            cmd.CommandText += Kisit;
                        }
                    }
                    if (Islem_Tipi == "Depolar")
                    {
                        cmd.CommandText = "SELECT TOP 50 DEPO_KODU AS ID, DEPO_KODU AS Kod, DEPO_ISMI AS Isim ";
                        cmd.CommandText += " FROM [" + Uygulama_Db + "].[dbo].OYG_NV_DEPOLAR ";
                        cmd.CommandText += " WHERE 'Depo' = 'Depo'";
                        if (Kisit != "")
                        {
                            cmd.CommandText += Kisit;
                        }
                    }
                    if (Islem_Tipi == "Hucreler")
                    {
                        cmd.CommandText = "SELECT TOP 50 HUCREKODU AS ID, HUCREKODU AS Kod, Ad as Isim ";
                        cmd.CommandText += " FROM [" + Uygulama_Db + "].[dbo].OYG_NV_HUCRELER ";
                        cmd.CommandText += " WHERE 'Hucre' = 'Hucre' ";
                        if (Kisit != "")
                        {
                            cmd.CommandText += Kisit;
                        }
                    }
                    if (Islem_Tipi == "ZimDemirbas")
                    {
                        cmd.CommandText = "SELECT TOP 50 DEMIRBAS_KODU AS ID, DEMIRBAS_KODU AS Kod, DEMIRBAS_ADI as Isim ";
                        cmd.CommandText += " FROM [" + Uygulama_Db + "].[dbo].OYG_NV_ZIMDEMIRBASLAR ";
                        cmd.CommandText += " WHERE 'Hucre' = 'Hucre' ";
                        if (Kisit != "")
                        {
                            cmd.CommandText += Kisit;
                        }
                    }
                    if (Islem_Tipi == "ZimPersonel")
                    {
                        cmd.CommandText = "SELECT TOP 50 PERSONEL_KODU AS ID, PERSONEL_KODU AS Kod, PERSONEL_ADI as Isim ";
                        cmd.CommandText += " FROM [" + Uygulama_Db + "].[dbo].OYG_NV_ZIMPERSONELLER ";
                        cmd.CommandText += " WHERE 'Hucre' = 'Hucre' ";
                        if (Kisit != "")
                        {
                            cmd.CommandText += Kisit;
                        }
                    }
                    if (Islem_Tipi == "StokHucreBakiye")
                    {
                        cmd.CommandText = "SELECT TOP 50 HUCREKODU AS ID, STOK_KODU AS Kod, STOK_ADI as Isim ";
                        cmd.CommandText += " , ISNULL(NETBAKIYE,0) AS BAKIYE ";
                        cmd.CommandText += " , ISNULL(HUCREKODU,'') AS HUCREKODU ";
                        cmd.CommandText += " , ISNULL(OLCU_BR1,'') AS OLCU_BR1 ";
                        cmd.CommandText += " FROM [" + Uygulama_Db + "].[dbo].OYG_NV_HUCRESERI_BAKIYE ";
                        cmd.CommandText += " WHERE 'Hucre' = 'Hucre' ";
                        if (Kisit != "")
                        {
                            cmd.CommandText += Kisit;
                        }
                    }
                }
                if (Uygulama == "LOGO")
                {
                    if (Islem_Tipi == "Depolar")
                    {
                        cmd.CommandText = "SELECT DEPO_KODU AS ID, DEPO_KODU AS Kod, DEPO_ISMI AS Isim FROM OYG_NV_DEPOLAR WHERE 'Depo' = 'Depo'";
                    }
                    if (Islem_Tipi == "Hucreler")
                    {
                        cmd.CommandText = "SELECT * FROM OYG_NV_HUCRELER WHERE 'Hucre' = 'Hucre' ";
                    }
                }
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.Kod = Convert.ToString(satir["Kod"]);
                        entity.Isim = Convert.ToString(satir["Isim"]);

                        if (Islem_Tipi == "StokHucreBakiye")
                        {
                            entity.OlcuBr = Convert.ToString(satir["OLCU_BR1"]);
                            entity.Bakiye = Convert.ToString(satir["BAKIYE"]);
                            entity.HucreKodu = Convert.ToString(satir["HUCREKODU"]);
                        }

                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #region Netsis_Wms_Qr_Olustur
        public IDJsonResult Netsis_Wms_01_Qr_Olustur([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250725";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Sube_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Sube Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                if (Sube_Kodu == "")
                {
                    Sube_Kodu = "0";
                }
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                if (Depo_Kodu == "")
                {
                    Depo_Kodu = "0";
                }
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Seri_Lot = Convert.ToString(data["Seri_No"]);
                string Seri_TicariAdi = Convert.ToString(data["Seri_Ticari_Adi"]);
                string Seri_Tedarikci = Convert.ToString(data["Tedarikci_Kodu"]);
                string Seri_Skt = Convert.ToString(data["Seri_Skt"]);
                string Seri_Ambalaj = Convert.ToString(data["Seri_Ambalaj"]);
                string Seri_RafNo = Convert.ToString(data["Seri_RafNo"]);
                string Seri_RafSire = Convert.ToString(data["Seri_RafSira"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                if (Stok_Kodu == "")
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok Kodu boş gönderilemez";
                    return result;
                }
                if (Seri_Lot == "")
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Seri_Lot boş gönderilemez";
                    return result;
                }

                if (Uygulama == "NETSIS")
                {
                    _srg = "";
                    _srg += " \r\n  -- Netsis_Wms_Qr_Olustur ";
                    _srg += " \r\n  -- Kontrol ";
                    _srg += " \r\n SELECT 'Netsis_Wms_Qr_Stok_Bak_Kontrol' as Servis_Adi, '" + _Procedure_Versiyon + "' as Servis_Versiyonu ";
                    _srg += " \r\n , '" + Stok_Kodu + "' as STOK_KODU, '" + Seri_Lot + "' AS SERI_NO, '" + Seri_Skt + "' TARIH, '1' AS MIKTAR  ";
                    _srg += " \r\n , '" + Uygulama + "' as Uygulama, '" + Uygulama_Db + "' as Uygulama_Db, '" + Sube_Kodu + "' as Sube_Kodu, '" + Depo_Kodu + "' as Depo_Kodu  ";

                    _srg += " \r\n  -- Kontrol ";
                    _srg += " \r\n INSERT INTO [" + Uygulama_Db + "].[dbo].[TBLSERITRA] ";
                    _srg += " \r\n ( KAYIT_TIPI, SUBE_KODU, SERI_NO, STOK_KODU ";
                    _srg += " \r\n , HARACIK, TARIH ";
                    _srg += " \r\n , ACIK1, ACIK2, ACIK3, ACIKLAMA_4, ACIKLAMA_5 ";
                    _srg += " \r\n , MIKTAR, SON_KULLANMA_TARIHI, BARKOD ";
                    _srg += " \r\n , GCKOD, DEPOKOD, BELGENO, BELGETIP ";
                    _srg += " \r\n ) ";
                    _srg += " \r\n SELECT 'D' AS KAYIT_TIPI, '" + Sube_Kodu + "' SUBE_KODU, '" + Seri_Lot + "' AS SERI_NO, '" + Stok_Kodu + "' AS STOK_KODU  ";
                    _srg += " \r\n , '" + Seri_Tedarikci + "' AS HARACIK, CONVERT(nvarchar, GETDATE(),102) AS TARIH ";
                    _srg += " \r\n , LEFT('" + Seri_TicariAdi + "',50) as ACIK1, LEFT('" + Seri_Ambalaj + "',50) as ACIK2, LEFT('" + Seri_RafNo + "',50) as ACIK3, '" + Seri_RafSire + "' as ACIKLAMA_4, 'Wms_Seri' as ACIKLAMA_5 ";
                    _srg += " \r\n , 0 MIKTAR, '" + Seri_Skt + "' SON_KULLANMA_TARIHI, '" + Seri_Lot + "' AS BARKOD  ";
                    _srg += " \r\n , 'G' AS GCKOD, '" + Depo_Kodu + "' AS DEPOKOD, left('" + Kullanici + "',15) AS BELGENO, NULL AS BELGETIP ";

                    _srg += " \r\n  -- Logla ";
                    _srg += " \r\n INSERT INTO INNOVA.[dbo].TBLLOGUSER ";
                    _srg += " \r\n ( FORM, TARIH, KAYITID, BELGE_NO ";
                    _srg += " \r\n , KULLANICI, CARI_KODU ";
                    _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                    _srg += " \r\n SELECT 'Web Servis Wms 01 Qr tanımlama' AS FORM, getdate(), '" + Stok_Kodu + "' AS KAYITID, '" + Seri_Lot + "' AS BELGE_NO ";
                    _srg += " \r\n , '" + Kullanici + "' AS KULLANICI, '" + Seri_Tedarikci + "' AS CARI_KODU ";
                    _srg += " \r\n , '" + Seri_Lot + ':' + Seri_Tedarikci + ':' + Seri_Ambalaj + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_Wms_Qr_Olustur' AS KAYNAK ";
                }
                if (Uygulama == "LOGO")
                {

                }
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = 250702;
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Qr_Olustur

        #region Netsis_Wms_Qr_Stok_Bak_Kontrol
        public IDJsonResult Netsis_Wms_06_Qr_Stok_Bak_Kontrol([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250725";
            IDJsonResult result = new IDJsonResult();
            try
            {
                #region Kontroller
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Sube_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Sube_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Depo_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Depo_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Seri_No"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Seri_No bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Seri_Sayim"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Sayım bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                #endregion Kontroller
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                if (Sube_Kodu == "")
                {
                    Sube_Kodu = "0";
                }
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                if (Depo_Kodu == "")
                {
                    Depo_Kodu = "0";
                }
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Seri_No = Convert.ToString(data["Seri_No"]);
                string Belge_Tarihi = Convert.ToString(data["Belge_Tarihi"]);
                string Seri_Bakiye = Convert.ToString(data["Seri_Bakiye"]);
                string Seri_Sayim = Convert.ToString(data["Seri_Sayim"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                string _SayimFisno = DateTime.Now.ToString("yyMMddHHmmssfff");

                if (Stok_Kodu == "")
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok Kodu boş gönderilemez";
                    return result;
                }
                if (Seri_No == "")
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Seri_No boş gönderilemez";
                    return result;
                }

                if (Uygulama == "NETSIS")
                {
                    _srg = "";
                    _srg += " \r\n  -- Netsis_Wms_Qr_Stok_Bak_Kontrol ";


                    _srg += " \r\n  -- Sayim Kaydi ";
                    _srg += " \r\n INSERT INTO [" + Uygulama_Db + "].[dbo].[TBLSAYIM] ";
                    _srg += " \r\n ( SUBE_KODU, STOK_KODU, DEPO_KODU, MIKTAR, SAYIM_FIAT, TARIH, SAYIM_FISNO ";
                    _srg += " \r\n , KAYITYAPANKUL, KAYITTARIHI, SAYIM_KODU ";
                    _srg += " \r\n , SAYIM_ACIKLAMA ";
                    _srg += " \r\n ) ";
                    _srg += " \r\n SELECT '" + Sube_Kodu + "' AS SUBE_KODU, '" + Stok_Kodu + "' as STOK_KODU, '" + Depo_Kodu + "' AS DEPO_KODU ";
                    _srg += " \r\n , '" + Seri_Sayim.Replace(",", ".") + "' MIKTAR ";
                    _srg += " \r\n , '" + Seri_Bakiye.Replace(",", ".") + "' SAYIM_FIAT ";
                    _srg += " \r\n , '" + Belge_Tarihi + "' TARIH, '" + _SayimFisno + "' as SAYIM_FISNO ";
                    _srg += " \r\n , left('" + Kullanici + "',8) as  KAYITYAPANKUL, getdate() KAYITTARIHI, '" + _Procedure_Versiyon + "' AS SAYIM_FISNO ";
                    _srg += " \r\n , left('Wms Stok Kontrol',35) as SAYIM_ACIKLAMA ";
                    _srg += " \r\n ";

                    _srg += " \r\n  -- Sayim Seri Kaydi ";
                    _srg += " \r\n INSERT INTO " + Uygulama_Db + ".[dbo].[TBLSAYIMSERI] ";
                    _srg += " \r\n ( SUBE_KODU, STOK_KODU, DEPOKOD ";
                    _srg += " \r\n , SERI_NO ";
                    _srg += " \r\n , MIKTAR, GCKOD, TARIH, BELGENO ";
                    _srg += " \r\n , STRA_INC, KAYIT_TIPI ";
                    _srg += " \r\n ) ";
                    _srg += " \r\n SELECT '" + Sube_Kodu + "' AS SUBE_KODU, '" + Stok_Kodu + "' as STOK_KODU, '" + Depo_Kodu + "' AS DEPOKOD ";
                    _srg += " \r\n , '" + Seri_No + "' AS SERI_NO ";
                    _srg += " \r\n , '" + Seri_Sayim.Replace(",", ".") + "' AS MIKTAR, 'G' AS GCKOD, '" + Belge_Tarihi + "' TARIH,  '" + _SayimFisno + "' as SAYIM_FISNO ";
                    _srg += " \r\n , ISNULL((SELECT TOP 1 IC.INCKEYNO FROM [" + Uygulama_Db + "].[dbo].[TBLSAYIM] IC WITH (NOLOCK) WHERE IC.SAYIM_FISNO = '" + _SayimFisno + "' AND IC.STOK_KODU = '" + Stok_Kodu + "' ORDER BY IC.INCKEYNO DESC),0) AS STRA_INC ";
                    _srg += " \r\n , 'A' AS KAYIT_TIPI ";
                    _srg += " \r\n ";

                    _srg += " \r\n  -- Log Kaydi ";
                    _srg += " \r\n INSERT INTO INNOVA..TBLLOGUSER ";
                    _srg += " \r\n ( FORM, TARIH, KAYITID, BELGE_NO ";
                    _srg += " \r\n , KULLANICI, CARI_KODU ";
                    _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                    _srg += " \r\n SELECT 'Web Servis Wms 05 Qr Seri Kontrol', getdate(), '" + Stok_Kodu + "' AS KAYITID, '" + Seri_No + "' AS BELGE_NO ";
                    _srg += " \r\n , '" + Kullanici + "' AS KULLANICI, '" + Depo_Kodu + "' AS CARI_KODU ";
                    _srg += " \r\n , '" + Seri_No + ':' + Depo_Kodu + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_Wms_Qr_Stok_Bak_Kontrol' AS KAYNAK ";

                    _srg += " \r\n ";
                    _srg += " \r\n  -- Seri Harekete Kayit ";
                    _srg += " \r\n INSERT INTO [" + Uygulama_Db + "].[dbo].TBLSERITRA  ";
                    _srg += " \r\n (KAYIT_TIPI, SERI_NO, STOK_KODU, SUBE_KODU, DEPOKOD, TARIH ";
                    _srg += " \r\n 	, MIKTAR, MIKTAR2, GCKOD ";
                    _srg += " \r\n 	, BELGETIP, BELGENO, HARACIK) ";
                    _srg += " \r\n SELECT 'A' KAYIT_TIPI, SERI_NO, STOK_KODU, SUBE_KODU, DEPOKOD, TARIH ";
                    _srg += " \r\n 	, ABS( SAYIM - BAKIYE) AS MIKTAR, MIKTAR2,  CASE WHEN SAYIM - BAKIYE < 0 THEN 'C' ELSE 'G' END AS GCKOD  ";
                    _srg += " \r\n 	, 'A' BELGETIP, BELGENO BELGENO, 'SAYIM' AS HARACIK ";
                    _srg += " \r\n FROM ( ";
                    _srg += " \r\n 	SELECT SERI_NO, STOK_KODU, SUBE_KODU, SY.DEPOKOD, SY.TARIH, MIKTAR AS SAYIM ";
                    _srg += " \r\n 	, (SELECT SUM(CASE WHEN SR.GCKOD = 'G' THEN MIKTAR ELSE MIKTAR * -1 END ) FROM [" + Uygulama_Db + "].[dbo].TBLSERITRA SR WITH (NOLOCK) WHERE SR.STOK_KODU = SY.STOK_KODU AND SR.SERI_NO = SY.SERI_NO ) BAKIYE ";
                    _srg += " \r\n 	, BELGENO, MIKTAR2 ";
                    _srg += " \r\n     FROM [" + Uygulama_Db + "].[dbo].TBLSAYIMSERI SY WITH (NOLOCK) WHERE BELGENO = '" + _SayimFisno + "' ";
                    _srg += " \r\n ) SAYIM_FARKI ";
                    _srg += " \r\n WHERE ABS( SAYIM - BAKIYE) <> 0 ";
                    _srg += " \r\n ";

                    _srg += " \r\n  -- Stok Harekete Kayit ";
                    _srg += " \r\n INSERT INTO [" + Uygulama_Db + "].[dbo].TBLSTHAR ";
                    _srg += " \r\n (STOK_KODU, FISNO, SUBE_KODU, DEPO_KODU, STHAR_HTUR, STHAR_TARIH ";
                    _srg += " \r\n     , STHAR_GCKOD, STHAR_GCMIK, STHAR_GCMIK2 ";
                    _srg += " \r\n     , EKALAN, STHAR_ACIKLAMA ";
                    _srg += " \r\n     , STHAR_ODEGUN) ";
                    _srg += " \r\n SELECT STOK_KODU, BELGENO, SUBE_KODU, DEPOKOD, 'A' AS STHAR_HTUR, TARIH ";
                    _srg += " \r\n 	, GCKOD, MIKTAR , MIKTAR2 ";
                    _srg += " \r\n 	, SERI_NO, 'SAYIM' AS STHAR_ACIKLAMA ";
                    _srg += " \r\n     , 0 STHAR_ODEGUN ";
                    _srg += " \r\n FROM [" + Uygulama_Db + "].[dbo].TBLSERITRA SY WITH (NOLOCK) WHERE BELGENO = '" + _SayimFisno + "' ";
                    _srg += " \r\n ";

                    _srg += " \r\n  -- Kontrol ";
                    _srg += " \r\n SELECT 'Netsis_Wms_Qr_Stok_Bak_Kontrol' as Servis_Adi, '" + _Procedure_Versiyon + "' as Servis_Versiyonu ";
                    _srg += " \r\n , '" + Stok_Kodu + "' as STOK_KODU, '" + Seri_No + "' AS Seri_No, '" + Belge_Tarihi + "' Belge_Tarihi, '" + Seri_Sayim.Replace(",", ".") + "' AS MIKTAR  ";
                    _srg += " \r\n , '" + Uygulama + "' as Uygulama, '" + Uygulama_Db + "' as Uygulama_Db, '" + Sube_Kodu + "' as Sube_Kodu, '" + Depo_Kodu + "' as Depo_Kodu  ";
                }
                if (Uygulama == "LOGO")
                {

                }
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = Convert.ToInt32(_Procedure_Versiyon);
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Qr_StokKontrol

        #region Netsis_Wms_Qr_Stok_Red
        public IDJsonResult Netsis_Wms_07_Qr_Stok_Red([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250818";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Sube_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Sube_Kodu bilgisi boş olamaz.";
                    return result;
                }

                if (data["Depo_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Depo_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Seri_No"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Seri_No bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Seri_Red_Aciklama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Aciklama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                if (Sube_Kodu == "")
                {
                    Sube_Kodu = "0";
                }
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                if (Depo_Kodu == "")
                {
                    Depo_Kodu = "0";
                }
                string Belge_No = Convert.ToString(data["Belge_No"]);
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string _Seri_No = Convert.ToString(data["Seri_No"]);
                string Seri_Sira_No = Convert.ToString(data["SERI_SIRA_NO"]);
                string Belge_Tarihi = Convert.ToString(data["Belge_Tarihi"]);
                string Seri_Bakiye = Convert.ToString(data["Seri_Bakiye"]);
                string Seri_Sayim = "0"; // Convert.ToString(data["Seri_Sayim"]);
                string Seri_Ambalaj = Convert.ToString(data["Seri_Ambalaj"]);
                string Seri_Red_Aciklama = Convert.ToString(data["Seri_Red_Aciklama"]);
                string Seri_RafNo = Convert.ToString(data["Seri_RafNo"]);
                string Seri_RafSire = Convert.ToString(data["Seri_RafSira"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                string _SayimFisno = Convert.ToString("Red" + DateTime.Now.ToString("MddHHmmssfff")).Substring(0, 15);

                //if (Stok_Kodu == "")
                //{
                //    result.SonucKodu = 0;
                //    result.Hata = "UYARI! Stok Kodu boş gönderilemez";
                //    return result;
                //}
                //if (Seri_Lot == "")
                //{
                //    result.SonucKodu = 0;
                //    result.Hata = "UYARI! Seri_Lot boş gönderilemez";
                //    return result;
                //}

                if (Uygulama == "NETSIS")
                {
                    _srg = "";
                    _srg += " \r\n  -- Netsis_Wms_Qr_Stok_Red ";


                    _srg += " \r\n  -- Sayim Kaydi ";
                    _srg += " \r\n INSERT INTO [" + Uygulama_Db + "].[dbo].[TBLSAYIM] ";
                    _srg += " \r\n ( SUBE_KODU, STOK_KODU, DEPO_KODU, MIKTAR, SAYIM_FIAT, TARIH, SAYIM_FISNO ";
                    _srg += " \r\n , KAYITYAPANKUL, KAYITTARIHI ";
                    _srg += " \r\n , SAYIM_ACIKLAMA, SAYIM_KODU ";
                    _srg += " \r\n ) ";
                    _srg += " \r\n SELECT '" + Sube_Kodu + "' AS SUBE_KODU, '" + Stok_Kodu + "' as STOK_KODU, '" + Depo_Kodu + "' AS DEPO_KODU ";
                    _srg += " \r\n , '" + Seri_Sayim.Replace(",", ".") + "' MIKTAR ";
                    _srg += " \r\n , '" + Seri_Bakiye.Replace(",", ".") + "' SAYIM_FIAT ";
                    _srg += " \r\n , '" + Belge_Tarihi + "' TARIH, right( '" + _SayimFisno + "',15) as SAYIM_FISNO ";
                    _srg += " \r\n , left('" + Kullanici + "',8) as  KAYITYAPANKUL, getdate() KAYITTARIHI ";
                    _srg += " \r\n , left('" + Seri_Red_Aciklama + "',35) as SAYIM_ACIKLAMA, '" + _Procedure_Versiyon + "' AS SAYIM_KODU  ";
                    _srg += " \r\n ";

                    _srg += " \r\n  -- Sayim Seri Kaydi ";
                    _srg += " \r\n INSERT INTO " + Uygulama_Db + ".[dbo].[TBLSAYIMSERI] ";
                    _srg += " \r\n ( SUBE_KODU, STOK_KODU, DEPOKOD ";
                    _srg += " \r\n , SERI_NO ";
                    _srg += " \r\n , MIKTAR, GCKOD, TARIH, BELGENO ";
                    _srg += " \r\n , STRA_INC, KAYIT_TIPI ";
                    _srg += " \r\n ) ";
                    _srg += " \r\n SELECT '" + Sube_Kodu + "' AS SUBE_KODU, '" + Stok_Kodu + "' as STOK_KODU, '" + Depo_Kodu + "' AS DEPOKOD ";
                    _srg += " \r\n , '" + _Seri_No + "' AS SERI_NO ";
                    _srg += " \r\n , '" + Seri_Sayim.Replace(",", ".") + "' AS MIKTAR, 'G' AS GCKOD, '" + Belge_Tarihi + "' TARIH,  right( '" + _SayimFisno + "',15)  as SAYIM_FISNO ";
                    _srg += " \r\n , ISNULL((SELECT TOP 1 IC.INCKEYNO FROM [" + Uygulama_Db + "].[dbo].[TBLSAYIM] IC WITH (NOLOCK) WHERE IC.SAYIM_FISNO = '" + _SayimFisno + "' AND IC.STOK_KODU = '" + Stok_Kodu + "' ORDER BY IC.INCKEYNO DESC),0) AS STRA_INC ";
                    _srg += " \r\n , 'A' AS KAYIT_TIPI ";
                    _srg += " \r\n ";

                    _srg += " \r\n  -- Log Kaydi ";
                    _srg += " \r\n ";
                    _srg += " \r\n INSERT INTO INNOVA..TBLLOGUSER ";
                    _srg += " \r\n ( FORM, TARIH, KAYITID, BELGE_NO ";
                    _srg += " \r\n , KULLANICI, CARI_KODU ";
                    _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                    _srg += " \r\n SELECT 'Web Servis Wms 05 Qr Seri Kontrol', getdate(), '" + Stok_Kodu + "' AS KAYITID, '" + _Seri_No + "' AS BELGE_NO ";
                    _srg += " \r\n , '" + Kullanici + "' AS KULLANICI, '" + Depo_Kodu + "' AS CARI_KODU ";
                    _srg += " \r\n , '" + _Seri_No + ':' + Depo_Kodu + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_Wms_Qr_Stok_Red' AS KAYNAK ";

                    _srg += " \r\n  -- Seri Hareket Kaydi ";
                    _srg += " \r\n ";
                    _srg += " \r\n INSERT INTO [" + Uygulama_Db + "].[dbo].TBLSERITRA  ";
                    _srg += " \r\n (KAYIT_TIPI, SERI_NO, STOK_KODU, SUBE_KODU, DEPOKOD, TARIH ";
                    _srg += " \r\n 	, MIKTAR, MIKTAR2, GCKOD ";
                    _srg += " \r\n 	, BELGETIP, BELGENO, HARACIK) ";
                    _srg += " \r\n SELECT 'A' KAYIT_TIPI, SERI_NO, STOK_KODU, SUBE_KODU, DEPOKOD, TARIH ";
                    _srg += " \r\n 	, ABS( SAYIM - BAKIYE) AS MIKTAR, MIKTAR2,  CASE WHEN SAYIM - BAKIYE < 0 THEN 'C' ELSE 'G' END AS GCKOD  ";
                    _srg += " \r\n 	, 'A' BELGETIP, BELGENO BELGENO, 'SAYIM' AS HARACIK ";
                    _srg += " \r\n FROM ( ";
                    _srg += " \r\n  	SELECT SERI_NO, STOK_KODU, SUBE_KODU, SY.DEPOKOD, SY.TARIH, MIKTAR AS SAYIM ";
                    _srg += " \r\n  	, (SELECT SUM(CASE WHEN SR.GCKOD = 'G' THEN MIKTAR ELSE MIKTAR * -1 END ) FROM [" + Uygulama_Db + "].[dbo].TBLSERITRA SR WITH (NOLOCK) WHERE SR.STOK_KODU = SY.STOK_KODU AND SR.SERI_NO = SY.SERI_NO ) BAKIYE ";
                    _srg += " \r\n  	, BELGENO, MIKTAR2 ";
                    _srg += " \r\n      FROM [" + Uygulama_Db + "].[dbo].TBLSAYIMSERI SY WITH (NOLOCK) WHERE BELGENO = '" + _SayimFisno + "' ";
                    _srg += " \r\n ) SAYIM_FARKI ";
                    _srg += " \r\n WHERE ABS( SAYIM - BAKIYE) <> 0 ";
                    _srg += " \r\n ";

                    _srg += " \r\n  -- Stok Hareket Kaydi ";
                    _srg += " \r\n INSERT INTO [" + Uygulama_Db + "].[dbo].TBLSTHAR ";
                    _srg += " \r\n (STOK_KODU, FISNO, SUBE_KODU, DEPO_KODU, STHAR_HTUR, STHAR_TARIH ";
                    _srg += " \r\n     , STHAR_GCKOD, STHAR_GCMIK, STHAR_GCMIK2 ";
                    _srg += " \r\n     , EKALAN, STHAR_ACIKLAMA ";
                    _srg += " \r\n     , STHAR_ODEGUN) ";
                    _srg += " \r\n SELECT STOK_KODU, BELGENO, SUBE_KODU, DEPOKOD, 'A' AS STHAR_HTUR, TARIH ";
                    _srg += " \r\n 	, GCKOD, MIKTAR , MIKTAR2 ";
                    _srg += " \r\n 	, SERI_NO, 'RED' AS STHAR_ACIKLAMA ";
                    _srg += " \r\n     , 0 STHAR_ODEGUN ";
                    _srg += " \r\n FROM [" + Uygulama_Db + "].[dbo].TBLSERITRA SY WITH (NOLOCK) WHERE BELGENO = '" + _SayimFisno + "' ";
                    _srg += " \r\n ";
                    if (Seri_Sira_No != "")
                    {
                        _srg += " \r\n AND SR.SIRA_NO = '" + Seri_Sira_No + "' ";
                    }

                    _srg += " \r\n  -- Kontrol ";
                    _srg += " \r\n SELECT 'Netsis_Wms_Qr_Stok_Red' as Servis_Adi, '" + _Procedure_Versiyon + "' as Servis_Versiyonu ";
                    _srg += " \r\n , '" + Stok_Kodu + "' as STOK_KODU, '" + _Seri_No + "' AS Seri_No, '" + Belge_Tarihi + "' Belge_Tarihi, '" + Seri_Sayim.Replace(",", ".") + "' AS MIKTAR  ";
                    _srg += " \r\n , '" + Uygulama + "' as Uygulama, '" + Uygulama_Db + "' as Uygulama_Db, '" + Sube_Kodu + "' as Sube_Kodu, '" + Depo_Kodu + "' as Depo_Kodu  ";

                    _srg += " \r\n , ";
                    _srg += " \r\n UPDATE [" + Uygulama_Db + "].[dbo].TBLSAYIMSERI ";
                    _srg += " \r\n SET KAYIT_TIPI = 'R' ";
                    _srg += " \r\n WHERE SERI_NO = '" + _Seri_No + "' ";
                    if (Seri_Sira_No != "")
                    {
                        _srg += " \r\n AND SIRA_NO = '" + Seri_Sira_No + "' ";
                    }
                    
                }
                if (Uygulama == "LOGO")
                {

                }
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = 250707;
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Qr_Stok_Red

        #region Sabit_Listeler_Stok_Kart_Bilgileri
        public IDJsonResult Sabit_Listeler_Stok_Bilgileri([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }

                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);

                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                string Islem_Tipi = Convert.ToString(data["Islem_Tipi"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {
                    if (Islem_Tipi == "Stok_Birimleri")
                    {
                        _srg = " SELECT TOP 1 ST.STOK_KODU, ST.STOK_ADI ";
                        _srg += " , ISNULL(OLCU_BR1,'AD') AS BIRIM1, OLCU_BR2 BIRIM2, PAY_1 BIRIM2_PAY, PAYDA_1 BIRIM2_PAYDA, OLCU_BR3 BIRIM3, PAY2 BIRIM3_PAY, PAYDA2 BIRIM3_PAYDA";
                        _srg += " FROM " + Uygulama_Db + ".[dbo].[TBLSTSABIT] ST WITH (NOLOCK) ";
                        _srg += " WHERE 1=1 ";
                        _srg += " AND ST.STOK_KODU = '" + Stok_Kodu + "' ";

                    }
                    if (Islem_Tipi == "Stok_Bakiyesi")
                    {
                        _srg = " SELECT TOP 1 ST.STOK_KODU, ST.STOK_ADI ";
                        _srg += " \r\n , (SELECT SUM(CASE WHEN SH.STHAR_GCKOD = 'G' THEN STHAR_GCMIK ELSE STHAR_GCMIK * -1 END) AS BAKIYE FROM [" + Uygulama_Db + "].[dbo].TBLSTHAR SH WITH (NOLOCK) WHERE SH.STOK_KODU = ST.STOK_KODU ";
                        if (Depo_Kodu != "" || Depo_Kodu == "-1")
                        {
                            _srg += " AND SH.DEPO_KODU = '" + Depo_Kodu + "' ";
                        }
                        _srg += "   ) AS BAKIYE";
                        _srg += " FROM [" + Uygulama_Db + "].[dbo].[TBLSTSABIT] ST WITH (NOLOCK) ";
                        _srg += " WHERE 1=1 AND ST.STOK_KODU = '" + Stok_Kodu + "' ";

                    }
                    if (Islem_Tipi == "Depo_Bakiyeleri")
                    {
                        _srg = " SELECT TOP 1 BAK.STOK_KODU, BAK.STOK_ADI ";
                        _srg += " \r\n , BAK.SUBE_KODU, BAK.DEPO_KODU, BAK.BAKIYE ";
                        _srg += " FROM [" + Uygulama_Db + "].[dbo].[INN_VW_STOK_BAKIYE] BAK WITH (NOLOCK) ";
                        _srg += " WHERE 1=1 ";
                        _srg += " AND BAK.STOK_KODU = '" + Stok_Kodu + "' ";

                    }
                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri

                    if (Islem_Tipi == "Stok_Birimleri")
                    {
                        foreach (DataRow satir in dt.Rows)
                        {
                            dynamic entity = new System.Dynamic.ExpandoObject();
                            entity.Stok_Kodu = Convert.ToString(satir["STOK_KODU"]);
                            entity.Stok_Adi = Convert.ToString(satir["STOK_ADI"]);
                            entity.Birim1 = Convert.ToString(satir["BIRIM1"]);
                            entity.Birim2 = Convert.ToString(satir["BIRIM2"]);
                            entity.Birim2_Pay = Convert.ToString(satir["BIRIM2_PAY"]);
                            entity.Birim2_Payda = Convert.ToString(satir["BIRIM2_PAYDA"]);
                            entity.Birim3 = Convert.ToString(satir["BIRIM3"]);
                            entity.Birim3_Pay = Convert.ToString(satir["BIRIM3_PAY"]);
                            entity.Birim3_Payda = Convert.ToString(satir["BIRIM3_PAYDA"]);
                            entities.Add(entity);
                        }
                    }
                    if (Islem_Tipi == "Stok_Bakiyesi")
                    {
                        foreach (DataRow satir in dt.Rows)
                        {
                            dynamic entity = new System.Dynamic.ExpandoObject();
                            entity.Stok_Kodu = Convert.ToString(satir["STOK_KODU"]);
                            entity.Stok_Adi = Convert.ToString(satir["STOK_ADI"]);
                            entity.Bakiye = Convert.ToString(satir["BAKIYE"]);
                            entities.Add(entity);
                        }
                    }
                    if (Islem_Tipi == "Depo_Bakiyeleri")
                    {
                        foreach (DataRow satir in dt.Rows)
                        {
                            dynamic entity = new System.Dynamic.ExpandoObject();
                            entity.Stok_Kodu = Convert.ToString(satir["STOK_KODU"]);
                            entity.Stok_Adi = Convert.ToString(satir["STOK_ADI"]);
                            entity.Sube_Kodu = Convert.ToString(satir["SUBE_KODU"]);
                            entity.Depo_Kodu = Convert.ToString(satir["DEPO_KODU"]);
                            entity.Bakiye = Convert.ToString(satir["BAKIYE"]);

                            entities.Add(entity);
                        }
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Sabit_Listeler_Stok_Kart_Bilgileri

        #region Netsis_Wms_Qr_KayitlariListele
        public IDJsonResult Netsis_Wms_03_Qr_KayitlariListele([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250808";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Seri_No"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Seri_No bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);

                string Seri_No = Convert.ToString(data["Seri_No"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {
                    _srg += " \r\n SELECT IC.SIRA_NO SERI_SIRA_NO, SERI_NO,TARIH, MIKTAR, SON_KULLANMA_TARIHI  ";
                    _srg += " \r\n , IC.SUBE_KODU AS SUBE_KODU ";
                    _srg += " \r\n , [" + Uygulama_Db + "].DBO.TRK1(HARACIK) as TEDARIKCI_KODU, [" + Uygulama_Db + "].DBO.TRK1(CS.CARI_ISIM) AS TEDARIKCI_ADI ";
                    _srg += " \r\n FROM [" + Uygulama_Db + "].[dbo].[TBLSERITRA] IC WITH (NOLOCK) ";
                    _srg += " \r\n LEFT OUTER JOIN " + Uygulama_Db + ".[dbo].[TBLCASABIT] CS WITH (NOLOCK) ON CS.CARI_KOD = IC.HARACIK ";
                    _srg += " \r\n WHERE IC.SERI_NO = '" + Seri_No + "' AND IC.KAYIT_TIPI= 'D' ";

                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Seri_Sira_No = Convert.ToString(satir["SERI_SIRA_NO"]);
                        entity.Seri_No = Convert.ToString(satir["SERI_NO"]);
                        entity.Tarih = Convert.ToString(satir["TARIH"]);
                        entity.Miktar = Convert.ToString(satir["MIKTAR"]);
                        entity.Son_Kul_Tarihi = Convert.ToString(satir["SON_KULLANMA_TARIHI"]);
                        entity.Seri_Tedarikci = Convert.ToString(satir["TEDARIKCI_KODU"]);
                        entity.Seri_Tedarikci_Adi = Convert.ToString(satir["TEDARIKCI_ADI"]);
                        entity.Servis_Versiyon = 250808;
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    result.Sonuc_Versiyon = 250806;
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Qr_KayitlariListele

        #region Netsis_Wms_Qr_Listele
        public IDJsonResult Netsis_Wms_02_Qr_Listele([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250808";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Seri_No"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Seri_No bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                if (Depo_Kodu == "")
                {
                    Depo_Kodu = "0";
                }
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Seri_Sira_No = Convert.ToString(data["SERI_SIRA_NO"]);
                string Seri_No = Convert.ToString(data["Seri_No"]);
                string Seri_TicAdi = Convert.ToString(data["Ticari_Adi"]);
                string Seri_Tedarikci = Convert.ToString(data["Tedarikci_Kodu"]);
                string Seri_Skt = Convert.ToString(data["Seri_Skt"]);
                string Seri_Ambalaj = Convert.ToString(data["Seri_Ambalaj"]);
                string Seri_RafNo = Convert.ToString(data["Seri_RafNo"]);
                string Seri_RafSire = Convert.ToString(data["Seri_RafSira"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {
                    _srg = " ";
                    _srg += " \r\n  -- Netsis_Wms_Qr_Listele ";


                    _srg += " \r\n  -- Log Kaydi ";
                    _srg += " \r\n INSERT INTO INNOVA.[dbo].TBLLOGUSER ";
                    _srg += " \r\n ( FORM, TARIH, KAYITID, BELGE_NO ";
                    _srg += " \r\n , KULLANICI, CARI_KODU ";
                    _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                    _srg += " \r\n SELECT 'Web Servis Wms 03 Qr Listele' AS FORM, getdate(), '" + Stok_Kodu + "' AS KAYITID, '" + Seri_No + "' AS BELGE_NO ";
                    _srg += " \r\n , '" + Kullanici + "' AS KULLANICI, '" + Seri_Tedarikci + "' AS CARI_KODU ";
                    _srg += " \r\n , '" + Seri_No + ':' + Seri_Tedarikci + ':' + Seri_Ambalaj + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_Wms_Qr_Listele' AS KAYNAK ";
                    _srg += " \r\n ";

                    _srg += " \r\n  -- Donecek Veri ";
                    _srg += " \r\n SELECT TOP 1 [" + Uygulama_Db + "].DBO.TRK1(SR.STOK_KODU) as STOK_KODU, [" + Uygulama_Db + "].DBO.TRK1(STOK_ADI) AS STOK_ADI ";
                    _srg += " \r\n , [" + Uygulama_Db + "].DBO.TRK1(SR.SERI_NO) AS SERI_NO ";
                    _srg += " \r\n , SR.SUBE_KODU AS SUBE_KODU ";
                    _srg += " \r\n , [" + Uygulama_Db + "].DBO.TRK1(HARACIK) as TEDARIKCI_KODU, DBO.TRK1(CS.CARI_ISIM) AS TEDARIKCI_ADI ";
                    _srg += " \r\n , [" + Uygulama_Db + "].DBO.TRK1(SR.ACIK1) as STOK_TICARI_ADI ";
                    _srg += " \r\n , [" + Uygulama_Db + "].DBO.TRK1(SR.ACIK2) as AMBALAJ ";
                    _srg += " \r\n , [" + Uygulama_Db + "].DBO.TRK1(SR.ACIK3) as RAFNO ";
                    _srg += " \r\n , [" + Uygulama_Db + "].DBO.TRK1(SR.ACIKLAMA_4) as RAFSIRA ";
                    _srg += " \r\n , CONVERT(nvarchar, SON_KULLANMA_TARIHI,102) AS SON_KULLANMA_TARIHI, SON_KULLANMA_TARIHI as SON_KULLANMA_TARIHI_ORJ ";
                    _srg += " \r\n , ISNULL( ";
                    _srg += " \r\n           ROUND((SELECT SUM(CASE WHEN ICSR.GCKOD = 'G' THEN ICSR.MIKTAR ELSE ICSR.MIKTAR * -1 END) AS BAK FROM [" + Uygulama_Db + "].[dbo].[TBLSERITRA] ICSR WITH (NOLOCK) ";
                    _srg += " \r\n            WHERE ICSR.SERI_NO = SR.SERI_NO AND ICSR.SUBE_KODU = '" + Sube_Kodu + "' ";
                    if (Depo_Kodu != "")
                    {
                        _srg += " \r\n        AND ICSR.DEPOKOD = '" + Depo_Kodu + "' ";
                    }
                    _srg += " \r\n           ),2) ";
                    _srg += " \r\n   , 0 ) as BAKIYE ";
                    _srg += " \r\n , SR.SIRA_NO ";
                    _srg += " \r\n , (SELECT COUNT(IC.SIRA_NO) AS SAY FROM [" + Uygulama_Db + "].[dbo].[TBLSERITRA] IC WITH (NOLOCK) WHERE IC.SERI_NO = SR.SERI_NO AND IC.KAYIT_TIPI= 'D'  ) AS KAYIT_SAYISI  ";
                    _srg += " \r\n FROM [" + Uygulama_Db + "].[dbo].[TBLSERITRA] SR WITH (NOLOCK) ";
                    _srg += " \r\n INNER JOIN " + Uygulama_Db + ".[dbo].[TBLSTSABIT] ST WITH (NOLOCK) ON SR.STOK_KODU = ST.STOK_KODU ";
                    _srg += " \r\n LEFT OUTER JOIN " + Uygulama_Db + ".[dbo].[TBLCASABIT] CS WITH (NOLOCK) ON CS.CARI_KOD = SR.HARACIK ";
                    _srg += " \r\n WHERE 1=1 and SR.KAYIT_TIPI= 'D' ";
                    _srg += " \r\n AND SR.SERI_NO = '" + Seri_No + "' ";
                    if (Seri_Sira_No != "")
                    {
                        _srg += " \r\n AND SR.SIRA_NO = '" + Seri_Sira_No + "' ";
                    }
               
                    // stok gelmiyor, seri tek olmalı
                    _srg += " \r\n ORDER BY SR.SIRA_NO DESC  ";

                    _srg += " \r\n  -- Kontrol Atta Olmalı, Ustteki Sorgudan ilk degerler dönmeli";
                    _srg += " \r\n SELECT 'Netsis_Wms_Qr_Listele' as Servis_Adi, '" + _Procedure_Versiyon + "' as Servis_Versiyonu ";
                    _srg += " \r\n , '" + Stok_Kodu + "' as STOK_KODU, '" + Seri_No + "' AS SERI_NO, '" + Seri_Skt + "' TARIH, '1' AS MIKTAR  ";
                    _srg += " \r\n , '" + Uygulama + "' as Uygulama, '" + Uygulama_Db + "' as Uygulama_Db, '" + Sube_Kodu + "' as Sube_Kodu, '" + Depo_Kodu + "' as Depo_Kodu  ";
                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Seri_Sira_No = Convert.ToString(satir["SIRA_NO"]);
                        entity.Stok_Kodu = Convert.ToString(satir["STOK_KODU"]);
                        entity.Stok_Adi = Convert.ToString(satir["STOK_ADI"]);
                        entity.Seri_No = Convert.ToString(satir["SERI_NO"]);
                        entity.Seri_Ticari_Adi = Convert.ToString(satir["STOK_TICARI_ADI"]);
                        entity.Seri_Tedarikci = Convert.ToString(satir["TEDARIKCI_KODU"]);
                        entity.Seri_Tedarikci_Adi = Convert.ToString(satir["TEDARIKCI_ADI"]);
                        entity.Seri_Ambalaj = Convert.ToString(satir["AMBALAJ"]);
                        entity.Seri_RafNo = Convert.ToString(satir["RAFNO"]);
                        entity.Seri_RafSira = Convert.ToString(satir["RAFSIRA"]);
                        entity.Seri_Skt = Convert.ToString(satir["SON_KULLANMA_TARIHI"]);
                        entity.Seri_Bakiye = Convert.ToString(satir["BAKIYE"]);
                        entity.Seri_KayitSayisi = Convert.ToString(satir["KAYIT_SAYISI"]);
                        entity.Bilgi = "Olcu Birimi ayriyeten çekiliyor";
                        entity.Servis_Versiyon = 250808;
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    result.Sonuc_Versiyon = 250624;
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Qr_Listele

        #region Belge_Listele
        public IDJsonResult Belge_Listele([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Belge_Türü"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Seri_No bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Belge_Türü = Convert.ToString(data["Belge_Türü"]);
                string Tarih_Bas = Convert.ToString(data["Tarih_Bas"]);
                string Tarih_Bit = Convert.ToString(data["Tarih_Bit"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {

                    _srg = " SELECT FT.FATIRS_NO BELGE_NO, FT.CARI_KODU ";
                    _srg += " , DBO.TRK1(CS.CARI_ISIM) AS CARI_ADI ";
                    _srg += " , FT.TARIH AS TARIH ";
                    _srg += " , FT.SUBE_KODU AS SUBE_KODU ";
                    _srg += " FROM " + Uygulama_Db + ".[dbo].[TBLFATUIRS] FT WITH (NOLOCK) ";
                    _srg += " LEFT OUTER JOIN " + Uygulama_Db + ".[dbo].[TBLCASABIT] CS WITH (NOLOCK) ON CS.CARI_KOD = FT.CARI_KODU ";
                    _srg += " WHERE 250624=250624 ";
                    if (Belge_Türü != "")
                    {
                        _srg += " AND FTIRSIP = '" + Belge_Türü + "' ";
                    }
                    _srg += " ORDER BY FT.TARIH DESC, FT.FATIRS_NO  ";

                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Belge_No = Convert.ToString(satir["BELGE_NO"]);
                        entity.Cari_Kodu = Convert.ToString(satir["CARI_KODU"]);
                        entity.Cari_Adi = Convert.ToString(satir["CARI_ADI"]);
                        entity.Tarih = Convert.ToString(satir["TARIH"]);
                        entity.Sube_Kodu = Convert.ToString(satir["SUBE_KODU"]);
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Belge_Listele

        #region Belge_Detaylari
        public IDJsonResult Belge_Detaylari([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Belge_No"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Belge Numarası bilgisi boş olamaz.";
                    return result;
                }
                if (data["Belge_Türü"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Belge Türü bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Belge_Türü = Convert.ToString(data["Belge_Türü"]);
                string Belge_No = Convert.ToString(data["Belge_No"]);
                string Tarih_Bas = Convert.ToString(data["Tarih_Bas"]);
                string Tarih_Bit = Convert.ToString(data["Tarih_Bit"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {

                    _srg = " SELECT SH.STOK_KODU, DBO.TRK1(STOK_ADI) AS STOK_ADI ";
                    _srg += " \r\n , SH.STHAR_GCMIK AS MIKTAR ";
                    _srg += " \r\n , SH.STHAR_BF AS BRUT_FIYAT ";
                    _srg += " \r\n , SH.STHAR_NF AS NET_FIYAT ";

                    _srg += " \r\n , SH.STHAR_KDV AS KDV_ORANI ";
                    _srg += " \r\n , SH.SUBE_KODU AS SUBE_KODU ";
                    _srg += " \r\n , SH.FISNO AS BELGE_NO, SH.STHAR_TARIH AS TARIH ";
                    _srg += " \r\n , SH.STHAR_ACIKLAMA AS CARI_KODU, DBO.TRK1(CS.CARI_ISIM) AS CARI_ADI ";
                    _srg += " FROM " + Uygulama_Db + ".[dbo].[TBLSTHAR] SH WITH (NOLOCK) ";
                    _srg += " LEFT OUTER JOIN " + Uygulama_Db + ".[dbo].[TBLSTSABIT] ST WITH (NOLOCK) ON ST.STOK_KODU = SH.STOK_KODU ";
                    _srg += " LEFT OUTER JOIN " + Uygulama_Db + ".[dbo].[TBLCASABIT] CS WITH (NOLOCK) ON CS.CARI_KOD = SH.STHAR_ACIKLAMA ";
                    _srg += " WHERE 250624=250624 ";
                    if (Belge_Türü != "")
                    {
                        _srg += " AND STHAR_FTIRSIP = '" + Belge_Türü + "' ";
                    }
                    if (Belge_No != "")
                    {
                        _srg += " AND SH.FISNO = '" + Belge_No + "' ";
                    }
                    _srg += " ORDER BY SH.INCKEYNO ";

                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Stok_Kodu = Convert.ToString(satir["STOK_KODU"]);
                        entity.Stok_Adi = Convert.ToString(satir["STOK_ADI"]);
                        entity.Miktar = Convert.ToString(satir["MIKTAR"]);
                        entity.Kdv_Orani = Convert.ToString(satir["KDV_ORANI"]);
                        entity.Brut_Fiyat = Convert.ToString(satir["BRUT_FIYAT"]);

                        entity.Net_Fiyat = Convert.ToString(satir["NET_FIYAT"]);
                        entity.Belge_No = Convert.ToString(satir["BELGE_NO"]);
                        entity.Cari_Kodu = Convert.ToString(satir["CARI_KODU"]);
                        entity.Cari_Adi = Convert.ToString(satir["CARI_ADI"]);
                        entity.Tarih = Convert.ToString(satir["TARIH"]);
                        entity.Sube_Kodu = Convert.ToString(satir["SUBE_KODU"]);
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Belge_Detaylari

        #region Netsis_Wms_Qr_TumListe
        public IDJsonResult Netsis_Wms_04_Qr_TumListe([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250728";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);


                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {
                    _srg = " ";
                    _srg += " \r\n  -- Netsis_Wms_Qr_TumListe ";


                    _srg += " \r\n  -- Listele ";
                    _srg += " \r\n INSERT INTO INNOVA.[dbo].TBLLOGUSER ";
                    _srg += " \r\n ( FORM, TARIH, KAYITID, BELGE_NO ";
                    _srg += " \r\n , KULLANICI, CARI_KODU ";
                    _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                    _srg += " \r\n SELECT 'Web Servis Wms 04 Qr Tüm Listele' AS FORM, getdate(), '" + Stok_Kodu + "' AS KAYITID, '' AS BELGE_NO ";
                    _srg += " \r\n , '" + Kullanici + "' AS KULLANICI, '" + Kullanici + "' AS CARI_KODU ";
                    _srg += " \r\n , '" + Stok_Kodu + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_Wms_Qr_TumListe' AS KAYNAK ";
                    _srg += " \r\n ";

                    _srg += " \r\n SELECT top 50 SR.STOK_KODU, [" + Uygulama_Db + "].[dbo].TRK1(STOK_ADI) AS STOK_ADI ";
                    _srg += " \r\n , [" + Uygulama_Db + "].[dbo].TRK1(SR.SERI_NO) AS SERI_NO ";
                    _srg += " \r\n , SR.SUBE_KODU AS SUBE_KODU ";
                    _srg += " \r\n , HARACIK as TEDARIKCI_KODU, [" + Uygulama_Db + "].[dbo].TRK1(CS.CARI_ISIM) AS TEDARIKCI_ADI ";
                    _srg += " \r\n , [" + Uygulama_Db + "].[dbo].TRK1(SR.ACIK1) as STOK_TICARI_ADI ";
                    _srg += " \r\n , [" + Uygulama_Db + "].[dbo].TRK1(SR.ACIK2) as AMBALAJ ";
                    _srg += " \r\n , [" + Uygulama_Db + "].[dbo].TRK1(SR.ACIK3) as RAFNO ";
                    _srg += " \r\n , [" + Uygulama_Db + "].[dbo].TRK1(SR.ACIKLAMA_4) as RAFSIRA ";
                    _srg += " \r\n , CONVERT(nvarchar, SON_KULLANMA_TARIHI,102) AS SON_KULLANMA_TARIHI, SON_KULLANMA_TARIHI as SON_KULLANMA_TARIHI_ORJ ";
                    _srg += " \r\n , 0 as BAKIYE ";
                    _srg += " \r\n FROM [" + Uygulama_Db + "].[dbo].[TBLSERITRA] SR WITH (NOLOCK) ";
                    _srg += " \r\n INNER JOIN " + Uygulama_Db + ".[dbo].[TBLSTSABIT] ST WITH (NOLOCK) ON SR.STOK_KODU = ST.STOK_KODU ";
                    _srg += " \r\n LEFT OUTER JOIN " + Uygulama_Db + ".[dbo].[TBLCASABIT] CS WITH (NOLOCK) ON CS.CARI_KOD = SR.HARACIK ";
                    _srg += " \r\n WHERE 1=1 and SR.KAYIT_TIPI= 'D' ";
                    if (Stok_Kodu != "")
                    {
                        _srg += " AND SR.STOK_KODU = '" + Stok_Kodu + "' ";
                    }
                    _srg += " ORDER BY SR.SIRA_NO DESC  ";

                    _srg += " \r\n  -- Kontrol Atta Olmalı, Ustteki Sorgudan ilk degerler dönmeli";
                    _srg += " \r\n SELECT 'Netsis_Wms_Qr_TumListe' as Servis_Adi, '" + _Procedure_Versiyon + "' as Servis_Versiyonu ";
                    _srg += " \r\n , '" + Stok_Kodu + "' as STOK_KODU, '' AS Seri_No, getdate() Belge_Tarihi, '' AS MIKTAR  ";
                    _srg += " \r\n , '" + Uygulama + "' as Uygulama, '" + Uygulama_Db + "' as Uygulama_Db, '" + Sube_Kodu + "' as Sube_Kodu, '0' as Depo_Kodu  ";
                }
                cmd.CommandText = _srg;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Stok_Kodu = Convert.ToString(satir["STOK_KODU"]);
                        entity.Stok_Adi = Convert.ToString(satir["STOK_ADI"]);
                        entity.Seri_No = Convert.ToString(satir["SERI_NO"]);
                        entity.Seri_Ticari_Adi = Convert.ToString(satir["STOK_TICARI_ADI"]);
                        entity.Seri_Tedarikci = Convert.ToString(satir["TEDARIKCI_KODU"]);
                        entity.Seri_Tedarikci_Adi = Convert.ToString(satir["TEDARIKCI_ADI"]);
                        entity.Seri_Ambalaj = Convert.ToString(satir["AMBALAJ"]);
                        entity.Seri_RafNo = Convert.ToString(satir["RAFNO"]);
                        entity.Seri_RafSira = Convert.ToString(satir["RAFSIRA"]);
                        entity.Seri_Skt = Convert.ToString(satir["SON_KULLANMA_TARIHI"]);
                        entity.Seri_Bakiye = Convert.ToString(satir["BAKIYE"]);
                        entity.Servis_Versiyon = 250624;
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    result.Sonuc_Versiyon = 250702;
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Qr_TumListe

        #region Netsis_Wms_Qr_Yazdir
        public IDJsonResult Netsis_Wms_90_Qr_Yazdir([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250725";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Seri_No"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Seri_No bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Seri_Lot = Convert.ToString(data["Seri_No"]);

                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                if (Uygulama == "NETSIS")
                {
                    _srg = " ";
                    _srg += " \r\n SELECT SIRA_NO ";
                    _srg += " \r\n FROM " + Uygulama_Db + ".[dbo].[TBLSERITRA] SR WITH (NOLOCK) ";
                    _srg += " \r\n WHERE SERI_NO = '" + Seri_Lot + "' ";

                    SqlCommand cmdKont = new SqlCommand();
                    cmdKont.CommandType = System.Data.CommandType.Text;
                    cmdKont.CommandText = _srg;
                    DataTable dtKont = (DataTable)IDVeritabani.Sorgula(cmdKont, SorgulaTuru.Tablo);
                    if (dtKont.Rows.Count > 0)
                    {
                        _srg = " ";
                        _srg += " \r\n  -- Netsis_Wms_Qr_Yazdir ";
                        _srg += " \r\n  -- Yazdir ";
                        _srg += " \r\n INSERT INTO INNOVA.[dbo].[TBLBELGE_YAZDIR] ";
                        _srg += " \r\n ( DBNAME,SUBE_KODU, FTIRSIP, BELGE_NO, CARI_KODU ";
                        _srg += " \r\n , DIZAYN_ADI ";
                        _srg += " \r\n , KAYIT_KULLANICI, KAYIT_TARIHI ";
                        _srg += " \r\n ) ";
                        _srg += " \r\n SELECT top 1 '" + Uygulama_Db + "' AS DBNAME, '" + Sube_Kodu + "' AS SUBE_KODU, 'Wms_Seri' AS FTIRSIP, '" + Stok_Kodu + "' BELGE_NO, '" + Seri_Lot + "' AS CARI_KODU ";
                        _srg += " \r\n , 'WMS_SERI_BASIMI' AS DIZAYN_ADI ";
                        _srg += " \r\n , '" + Kullanici + "' AS KAYIT_KULLANICI, GETDATE() AS KAYIT_TARIHI ";
                        _srg += " \r\n FROM " + Uygulama_Db + ".[dbo].[TBLSERITRA] SR WITH (NOLOCK) ";
                        _srg += " \r\n WHERE SERI_NO = '" + Seri_Lot + "' ";
                        _srg += " \r\n ORDER BY SIRA_NO ";

                        _srg += " \r\n -- Logla ";
                        _srg += " \r\n INSERT INTO INNOVA..TBLLOGUSER ";
                        _srg += " \r\n ( FORM, TARIH, KAYITID, BELGE_NO ";
                        _srg += " \r\n , KULLANICI, CARI_KODU ";
                        _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                        _srg += " \r\n SELECT 'Web Servis Wms 02 Qr Yazdır' AS FORM, getdate(), '" + Stok_Kodu + "' AS KAYITID, '" + Seri_Lot + "' AS BELGE_NO ";
                        _srg += " \r\n , '" + Kullanici + "' AS KULLANICI, '" + Seri_Lot + "' AS CARI_KODU ";
                        _srg += " \r\n , '" + Seri_Lot + ':' + Seri_Lot + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_Wms_Qr_Yazdir' AS KAYNAK ";

                        _srg += " \r\n -- Seriyi Güncelle Yazdırma";
                        _srg += " \r\n UPDATE [" + Uygulama_Db + "].[dbo].[TBLSERITRA] SET YEDEK4 = ISNULL(YEDEK4,0) + 1 ";
                        _srg += " \r\n WHERE STOK_KODU = '" + Stok_Kodu + "' AND SERI_NO = '" + Seri_Lot + "' ";
                        _srg += " \r\n ";

                        _srg += " \r\n  -- Kontrol ";
                        _srg += " \r\n SELECT 'Netsis_Wms_Qr_TumListe' as Servis_Adi, '" + _Procedure_Versiyon + "' as Servis_Versiyonu ";
                        _srg += " \r\n , '" + Stok_Kodu + "' as STOK_KODU, '" + Seri_Lot + "' AS Seri_No, getdate() Belge_Tarihi, '' AS MIKTAR  ";
                        _srg += " \r\n , '" + Uygulama + "' as Uygulama, '" + Uygulama_Db + "' as Uygulama_Db, '" + Sube_Kodu + "' as Sube_Kodu, '0' as Depo_Kodu  ";
                    }
                    else
                    {
                        result.SonucKodu = 0;
                        result.Hata = "UYARI! Seri Kodu Bulunamadı.";
                        return result;
                    }
                }
                if (Uygulama == "LOGO")
                {

                }
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = 2507072;
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Qr_Yazdir

        #region Netsis_Wms_Stok_Girisi_Kalem
        public IDJsonResult Netsis_Wms_10_Stok_Girisi_Kalem([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250725";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Sube_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Sube Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Miktar"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Miktar bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                if (Sube_Kodu == "")
                {
                    Sube_Kodu = "0";
                }
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                if (Depo_Kodu == "")
                {
                    Depo_Kodu = "0";
                }
                string Belge_No = Convert.ToString(data["Belge_No"]);
                string Belge_Tarihi = Convert.ToString(data["Belge_Tarihi"]);
                string Belge_Tipi = Convert.ToString(data["Belge_Tipi"]);
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Seri_Lot = Convert.ToString(data["Seri_No"]);
                string Cari_Kodu = Convert.ToString(data["Cari_Kodu"]);
                string Seri_Miktar = Convert.ToString(data["Miktar"]);
                string Seri_Adet = Convert.ToString(data["Adet"]);
                if (Seri_Adet == "")
                {
                    Seri_Adet = "0";
                }
                string GcKodu = Convert.ToString(data["GcKodu"]);
                string Seri_Skt = Convert.ToString(data["Seri_Skt"]);
                string Seri_Ambalaj = Convert.ToString(data["Seri_Ambalaj"]);
                string Seri_RafNo = Convert.ToString(data["Seri_RafNo"]);
                string Seri_RafSire = Convert.ToString(data["Seri_RafSira"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                if (Uygulama == "NETSIS")
                {
                    _srg = " ";
                    _srg += " \r\n  -- Netsis_Wms_Stok_Girisi_Kalem ";
                    _srg += " \r\n  -- Kontrol ";
                    _srg += " \r\n SELECT 'Netsis_Wms_Stok_Girisi_Kalem' as Servis_Adi, '" + _Procedure_Versiyon + "' as Servis_Versiyonu ";
                    _srg += " \r\n , '" + Stok_Kodu + "' as STOK_KODU, '" + Seri_Lot + "' AS Seri_No, '" + Belge_Tarihi + "' as Belge_Tarihi, '' AS MIKTAR  ";
                    _srg += " \r\n , '" + Uygulama + "' as Uygulama, '" + Uygulama_Db + "' as Uygulama_Db, '" + Sube_Kodu + "' as Sube_Kodu, '" + Depo_Kodu + "' as Depo_Kodu  ";

                    _srg += " \r\n  -- Belge Kayıt Tablosuna Ekle ";
                    _srg += " \r\n INSERT INTO [INNOVA].[dbo].[TBLBELGE_KAYIT] ";
                    _srg += " \r\n ( FTIRSIP, SUBE_KODU, BELGE_NO ";
                    _srg += " \r\n , CARI_KODU, TARIH ";
                    _srg += " \r\n , STOK_KODU, STOK_ADI ";
                    _srg += " \r\n , MIKTAR, ADET ";
                    _srg += " \r\n , SERI_NO ";
                    _srg += " \r\n , GCKOD, DEPO_KODU ";
                    _srg += " \r\n , KAYIT_KULLANICI, KAYIT_TARIHI ";
                    _srg += " \r\n ) ";
                    _srg += " \r\n SELECT '" + Belge_Tipi + "' AS FTIRSIP, '" + Sube_Kodu + "' SUBE_KODU, right('" + Belge_No + "',15) AS BELGE_NO ";
                    _srg += " \r\n , '" + Cari_Kodu + "' AS CARI_KODU, '" + Belge_Tarihi + "' AS TARIH ";
                    _srg += " \r\n , '" + Stok_Kodu + "' AS STOK_KODU, '' as STOK_ADI  ";
                    _srg += " \r\n , '" + Seri_Miktar + "' MIKTAR ";
                    _srg += " \r\n , '" + Seri_Adet + "' AS ADET  ";
                    _srg += " \r\n , '" + Seri_Lot + "' AS SERI_NO ";
                    _srg += " \r\n , '" + GcKodu + "' AS GCKOD, '" + Depo_Kodu + "' AS DEPO_KODU ";
                    _srg += " \r\n , '" + Kullanici + "' AS KAYIT_KULLANICI, GETDATE() KAYIT_TARIHI ";

                    _srg += " \r\n  -- Logla ";
                    _srg += " \r\n INSERT INTO INNOVA..TBLLOGUSER ";
                    _srg += " \r\n ( FORM, TARIH, KAYITID, BELGE_NO, CARI_KODU ";
                    _srg += " \r\n , KULLANICI ";
                    _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                    _srg += " \r\n SELECT 'Web Servis Wms 11 Qr Okutma', getdate(), '" + Stok_Kodu + "' as KAYITID, '" + Seri_Lot + "' AS BELGE_NO, '" + Cari_Kodu + "' AS Cari_Kodu ";
                    _srg += " \r\n , '" + Kullanici + "' AS KULLANICI ";
                    _srg += " \r\n , '" + Seri_Lot + ':' + Cari_Kodu + ':' + Seri_Ambalaj + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_Wms_Stok_Girisi_Kalem' AS KAYNAK ";
                }
                if (Uygulama == "LOGO")
                {

                }
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = 250702;
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Stok_Girisi_Kalem

        #region Netsis_Wms_Stok_Girisi_Tamamla
        public IDJsonResult Netsis_Wms_11_Stok_Girisi_Tamamla([FromBody] JObject data)
        {
            string _Procedure_Versiyon = "250725";
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Sube_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Sube Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Belge_No"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Belge_No bilgisi boş olamaz.";
                    return result;
                }
                if (data["Cari_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Cari_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Belge_No = Convert.ToString(data["Belge_No"]);
                string Belge_Tarihi = Convert.ToString(data["Belge_Tarihi"]);
                string Belge_Tipi = Convert.ToString(data["Belge_Tipi"]);
                string Cari_Kodu = Convert.ToString(data["Cari_Kodu"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                if (Uygulama == "NETSIS")
                {
                    _srg = " ";
                    _srg += " \r\n  -- Netsis_Wms_Stok_Girisi_Tamamla ";
                    _srg += " \r\n  -- Kontrol ";
                    _srg += " \r\n SELECT 'Netsis_Wms_Stok_Girisi_Kalem' as Servis_Adi, '" + _Procedure_Versiyon + "' as Servis_Versiyonu ";
                    _srg += " \r\n , '" + Cari_Kodu + "' as Cari_Kodu, '" + Belge_No + "' AS Belge_No, '" + Belge_Tarihi + "' as Belge_Tarihi, '' AS MIKTAR  ";
                    _srg += " \r\n , '" + Uygulama + "' as Uygulama, '" + Uygulama_Db + "' as Uygulama_Db, '" + Sube_Kodu + "' as Sube_Kodu, '0' as Depo_Kodu  ";

                    _srg += " \r\n  -- Islem ";
                    _srg += " \r\nEXEC " + Uygulama_Db + ".[dbo].INN_PR_BELGE_KAYIT '" + Belge_No + "', '" + Cari_Kodu + "', '" + Belge_Tipi + "', 'H'  ";

                    _srg += " \r\n  -- Logla ";
                    _srg += " \r\n INSERT INTO INNOVA..TBLLOGUSER ";
                    _srg += " \r\n ( FORM, TARIH, KAYITID, CARI_KODU ";
                    _srg += " \r\n , KULLANICI ";
                    _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                    _srg += " \r\n SELECT 'Web Servis Wms 12 Belge Tamamla', getdate(), right('" + Belge_No + "',15), '" + Cari_Kodu + "' AS CARI_KODU ";
                    _srg += " \r\n , '" + Kullanici + "' AS KULLANICI ";
                    _srg += " \r\n , '" + Belge_No + ':' + Cari_Kodu + ':' + Belge_Tipi + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_Wms_Stok_Girisi_Tamamla' AS KAYNAK ";
                }
                if (Uygulama == "LOGO")
                {

                }
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = 250702;
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Wms_Stok_Girisi_Tamamla

        #region Netsis_Uretim_Lokasyon_Liste
        public IDJsonResult Netsis_Uretim_Lokasyon_Liste([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Islem_Tipi = Convert.ToString(data["Islem_Tipi"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (Uygulama == "NETSIS")
                {
                    if (Islem_Tipi == "Lokasyon")
                    {
                        cmd.CommandText = " SELECT TOP 10 LEFT(FISNO,10) ID, BARKOD Kod, CONVERT(nvarchar, KAYIT_TARIHI,108) as Isim FROM INNOVA..TBLOKUTMA WITH (NOLOCK) WHERE 1=1 and  DBNAME= 'Uretim_Lokasyon' ORDER BY ID DESC  ";

                    }

                }
                if (Uygulama == "LOGO")
                {
                    if (Islem_Tipi == "Lokasyon")
                    {
                        cmd.CommandText = " SELECT TOP 10 LEFT(FISNO,10) ID, BARKOD Kod, CONVERT(nvarchar, KAYIT_TARIHI,108) as Isim FROM INNOVA..TBLOKUTMA WITH (NOLOCK) WHERE 1=1 and  DBNAME= 'Uretim_Lokasyon' ORDER BY ID DESC  ";
                    }
                }
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.Kod = Convert.ToString(satir["Kod"]);
                        entity.Isim = Convert.ToString(satir["Isim"]);

                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    result.Sonuc_Versiyon = 250702;
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Netsis_Uretim_Lokasyon_Liste

        #region Netsis_Uretim_Lokasyon_Kaydet
        public IDJsonResult Netsis_Uretim_Lokasyon_Atama([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Lokasyon_Barkod"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Lokasyon_Barkod bilgisi boş olamaz.";
                    return result;
                }
                if (data["Lokasyon_Adresi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Lokasyon_Adresi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string _Lokasyon_Barkod = Convert.ToString(data["Lokasyon_Barkod"]);
                string _Lokasyon_Adresi = Convert.ToString(data["Lokasyon_Adresi"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);


                string _srg = "INSERT INTO INNOVA..TBLOKUTMA ";
                _srg += " \r\n (DBNAME, TARIH, FISNO, FISNO2, BARKOD ";
                _srg += " \r\n  , MIKTAR, KAYIT_YAPAN, KAYIT_TARIHI, VERSIYON ) ";
                _srg += " \r\n SELECT 'Uretim_Lokasyon' , GETDATE(), '" + _Lokasyon_Barkod + "' FISNO, '0' FISNO2, '" + _Lokasyon_Adresi + "' BARKOD  ";
                _srg += " \r\n , 1 MIKTAR, '" + Kullanici + "' AS KAYIT_YAPAN, GETDATE() KAYIT_TARIHI, '240703' AS VERSIYON  ";

                _srg += " \r\n UPDATE " + Uygulama_Db + ".DBO.TBLCONFIGTRA SET LOKASYON = '" + _Lokasyon_Adresi + "' WHERE CAST(INCKEYNO AS NVARCHAR(15)) = '" + _Lokasyon_Barkod + "' ";


                _srg += " \r\n exec " + Uygulama_Db + "..[INN_PR_MOBILYA_01_URETIM_KAYIT] ";
                _srg += "  'TAKIM' ";
                _srg += " , '" + _Lokasyon_Barkod + "' ";
                _srg += " , '1' ";
                _srg += " , 'Web' ";
                _srg += " , '500'";
                _srg += " , '' ";


                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion

        #region Netsis_Uretim_Lokasyon_Kaydet
        public IDJsonResult Netsis_Hucre_Tra_Kaydet([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["LisansNumarasi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! LisansNumarasi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Tarih"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Tarih bilgisi boş olamaz.";
                    return result;
                }
                if (data["Cikan_Depo_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Çıkan Depo_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Giren_Depo_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Giren Depo_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Cikan_Hucre_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Çıkan Hücre bilgisi boş olamaz.";
                    return result;
                }
                if (data["Giren_Hucre_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Giren Hücre bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Miktar"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Miktar bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Donem_Kodu = Convert.ToString(data["Donem_Kodu"]);
                string Islem_Tipi = Convert.ToString(data["Islem_Tipi"]);


                string Belge_No = Convert.ToString(data["Belge_No"]);
                DateTime Tarih = Convert.ToDateTime(data["Tarih"]);
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Cikan_Depo_Kodu = Convert.ToString(data["Cikan_Depo_Kodu"]);
                string Cikan_Hucre_Kodu = Convert.ToString(data["Cikan_Hucre_Kodu"]);
                string Giren_Depo_Kodu = Convert.ToString(data["Giren_Depo_Kodu"]);
                string Giren_Hucre_Kodu = Convert.ToString(data["Giren_Hucre_Kodu"]);
                decimal Miktar = Convert.ToDecimal(data["Miktar"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                string Tip = "HT";
                if (Cikan_Depo_Kodu != Giren_Depo_Kodu)
                {
                    Tip = "HTDAT";
                }

                string _sorgu = "";
                _sorgu += " INSERT INTO INNOVA.DBO.[TBLBELGE_KAYIT]";
                _sorgu += " ( ";
                _sorgu += " BELGE_NO, FTIRSIP, CARI_KODU ";
                _sorgu += " , STOK_REF, [STOK_KODU], MIKTAR, SIRA, ADET ";
                _sorgu += " , HUCRE_KODU, SERI_NO, GCKOD ";
                _sorgu += " , TARIH, FIYAT ";
                _sorgu += " , GIT_SUBE_KODU, GIT_DEPO_KODU ";
                _sorgu += " , SUBE_KODU, DEPO_KODU, PROJE_KODU ";
                _sorgu += " , UST_ACIKLAMA1, UST_ACIKLAMA2 ";
                _sorgu += " , KAYIT_KULLANICI, KAYIT_TARIHI ";
                _sorgu += " ) ";
                _sorgu += " SELECT '" + Belge_No + "' BELGE_NO, '" + Tip + "' FTIRSIP, 'HT' CARI_KODU ";
                _sorgu += " , '0' STOK_REF, '" + Stok_Kodu + "' STOK_KODU, REPLACE('" + Miktar + "',',','.') MIKTAR, 1 SIRA, 1 ADET ";
                _sorgu += " , '" + Cikan_Hucre_Kodu + "' HUCRE_KODU, '' SERI_NO, 'C' GCKOD ";
                _sorgu += " , '" + Tarih.ToString("yyyy.MM.dd") + "' TARIH, 0 FIYAT ";
                _sorgu += " , '0' GIT_SUBE_KODU, '0' GIT_DEPO_KODU ";
                _sorgu += " , '0' SUBE_KODU, '" + Cikan_Depo_Kodu + "' DEPO_KODU, '' PROJE_KODU";
                _sorgu += " , '' UST_ACIKLAMA1, '' UST_ACIKLAMA2 ";
                _sorgu += " , '" + Kullanici + "' KAYIT_KULLANICI, GETDATE() KAYIT_TARIHI ";
                _sorgu += " UNION ALL ";
                _sorgu += " SELECT '" + Belge_No + "' BELGE_NO, '" + Tip + "' FTIRSIP, 'HT' CARI_KODU ";
                _sorgu += " , '0' STOK_REF, '" + Stok_Kodu + "' STOK_KODU, REPLACE('" + Miktar + "',',','.') MIKTAR, 1 SIRA, 1 ADET ";
                _sorgu += " , '" + Giren_Hucre_Kodu + "' HUCRE_KODU, '' SERI_NO, 'G' GCKOD ";
                _sorgu += " , '" + Tarih.ToString("yyyy.MM.dd") + "' TARIH, 0 FIYAT ";
                _sorgu += " , '0' GIT_SUBE_KODU, '0' GIT_DEPO_KODU ";
                _sorgu += " , '0' SUBE_KODU, '" + Giren_Depo_Kodu + "' DEPO_KODU, '' PROJE_KODU";
                _sorgu += " , '' UST_ACIKLAMA1, '' UST_ACIKLAMA2 ";
                _sorgu += " , '" + Kullanici + "' KAYIT_KULLANICI, GETDATE() KAYIT_TARIHI ";

                _sorgu += " EXEC [" + Uygulama_Db + "].[dbo].INN_PR_BELGE_KAYIT '" + Belge_No + "', 'HT' , '" + Tip + "', 'H' ";

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _sorgu;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion

        public IDJsonResult Depolar([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["LisansNumarasi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! LisansNumarasi bilgisi boş olamaz.";
                    return result;
                }
                string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "OYP_Depolar";
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.Kod = Convert.ToString(satir["Kod"]);
                        entity.Isim = Convert.ToString(satir["Isim"]);
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        public IDJsonResult StokAra([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["LisansNumarasi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! LisansNumarasi bilgisi boş olamaz.";
                    return result;
                }
                if (data["AranacakKelime"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! AranacakKelime bilgisi boş olamaz.";
                    return result;
                }
                if (data["SubeKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! SubeKodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["DepoKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! DepoKodu bilgisi boş olamaz.";
                    return result;
                }
                string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);
                string SubeKodu = Convert.ToString(data["SubeKodu"]);
                string DepoKodu = Convert.ToString(data["DepoKodu"]);
                string AranacakKelime = Convert.ToString(data["AranacakKelime"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "OYP_Stoklar";
                cmd.Parameters.AddWithValue("@SubeKodu", SubeKodu);
                cmd.Parameters.AddWithValue("@DepoKodu", DepoKodu);
                cmd.Parameters.AddWithValue("@AranacakKelime", AranacakKelime);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.Kod = Convert.ToString(satir["Kod"]);
                        entity.Isim = Convert.ToString(satir["Isim"]);
                        entity.Isim = Convert.ToString(satir["OlcuBirimi"]);
                        entity.Bakiye = Convert.ToString(satir["Bakiye"]);
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        public IDJsonResult SayimListesi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["LisansNumarasi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! LisansNumarasi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Tarih"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Tarih bilgisi boş olamaz.";
                    return result;
                }
                if (data["SubeKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! SubeKodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["DepoKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! DepoKodu bilgisi boş olamaz.";
                    return result;
                }
                string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);
                string Tarih = Convert.ToString(data["Tarih"]);
                string SubeKodu = Convert.ToString(data["SubeKodu"]);
                string DepoKodu = Convert.ToString(data["DepoKodu"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "OYP_SayimListesi";
                cmd.Parameters.AddWithValue("@Tarih", Tarih);
                cmd.Parameters.AddWithValue("@SubeKodu", SubeKodu);
                cmd.Parameters.AddWithValue("@DepoKodu", DepoKodu);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.Kod = Convert.ToString(satir["Kod"]);
                        entity.Isim = Convert.ToString(satir["Isim"]);
                        entity.Isim = Convert.ToString(satir["OlcuBirimi"]);
                        entity.Miktar = Convert.ToString(satir["Miktar"]);
                        entity.Bakiye = Convert.ToString(satir["Bakiye"]);
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        public IDJsonResult ZimmetKaydet([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["LisansNumarasi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! LisansNumarasi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Tarih"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Tarih bilgisi boş olamaz.";
                    return result;
                }
                if (data["CikisPersonelKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Çıkış Personel Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["GirisPersonelKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Giriş Personel Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["StokKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Miktar"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Miktar bilgisi boş olamaz.";
                    return result;
                }
                if (data["GcKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Gc Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["KayitYapanKullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);
                DateTime _Tarih = Convert.ToDateTime(data["Tarih"]);
                string _Belge_No = Convert.ToString(data["Belge_No"]);
                string _Islem_Tipi = Convert.ToString(data["Islem_Tipi"]);
                string _CikisPersonelKodu = Convert.ToString(data["CikisPersonelKodu"]);
                string _GirisPersonelKodu = Convert.ToString(data["GirisPersonelKodu"]);
                string _StokKodu = Convert.ToString(data["StokKodu"]);
                decimal _Miktar = Convert.ToDecimal(data["Miktar"]);
                string _GcKodu = Convert.ToString(data["GcKodu"]);
                string _KayitYapanKullanici = Convert.ToString(data["KayitYapanKullanici"]);

                string _sorgu = "";
                _sorgu += " INSERT INTO OYGROUP.DBO.[ZimmetHareketleri] ";
                _sorgu += " ( ";
                _sorgu += "   [UyelikID] ";
                _sorgu += " , [BelgeNo], [Tarih], [HareketTipi] ";
                _sorgu += " , [CariKodu], [StokKodu] ";
                _sorgu += " , [GcKodu], [Miktar] ";
                _sorgu += " , KayitYapanKullanici ";
                _sorgu += " ) ";
                _sorgu += " SELECT ";
                _sorgu += "   '" + LisansNumarasi + "' [UyelikID] ";
                _sorgu += " , '" + _Belge_No + "' BELGE_NO, '" + _Tarih.ToString("yyyy.MM.dd") + "' TARIH, '" + _Islem_Tipi + "' AS HareketTipi ";
                _sorgu += " , '" + _CikisPersonelKodu + "' AS [CariKodu], '" + _StokKodu + "' AS [StokKodu] ";
                _sorgu += " , 'C' AS [GcKodu], '" + _Miktar + "' AS [Miktar] ";
                _sorgu += " , '" + _KayitYapanKullanici + "' AS KayitYapanKullanici ";
                _sorgu += " WHERE '" + _CikisPersonelKodu + "' != '' ";
                _sorgu += " UNION ALL ";
                _sorgu += " SELECT ";
                _sorgu += "   '" + LisansNumarasi + "' [UyelikID] ";
                _sorgu += " , '" + _Belge_No + "' BELGE_NO, '" + _Tarih.ToString("yyyy.MM.dd") + "' TARIH, '" + _Islem_Tipi + "' AS HareketTipi ";
                _sorgu += " , '" + _GirisPersonelKodu + "' AS [CariKodu], '" + _StokKodu + "' AS [StokKodu] ";
                _sorgu += " , 'G' AS [GcKodu], '" + _Miktar + "' AS [Miktar] ";
                _sorgu += " , '" + _KayitYapanKullanici + "' AS KayitYapanKullanici ";
                _sorgu += " WHERE '" + _CikisPersonelKodu + "' != '' ";

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _sorgu;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;




            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        public IDJsonResult Kullanici_Yetkileri([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                string _sorgu = "";
                _sorgu += @"select  distinct s1.MenuID,s2.Menu,s1.Gor,s1.Duzenle,s1.Sil,s1.KullaniciID,s2.UstID 
                          from   Yetkiler as s1 with(nolock) 
                          inner join Menuler as s2 
                          on s1.MenuId=s2.ID
                          where s1.Gor=1 and s1.KullaniciID='" + Convert.ToString(data["KullaniciId"]) + "'  order by MenuID  ";

                List<dynamic> entities = new List<dynamic>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = _sorgu;
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.MenuID = Convert.ToString(satir["MenuID"]);
                        entity.Menu = Convert.ToString(satir["Menu"]);
                        entity.UstID = Convert.ToString(satir["UstID"]);
                        entities.Add(entity);
                    }
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        public IDJsonResult Kullanici_Kisayollari([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string kullaniciIdStr = Convert.ToString(data["kullaniciId"]);

                if (!Guid.TryParse(kullaniciIdStr, out Guid kullaniciId))
                {
                    result.SonucKodu = -1;
                    result.Sonuc = "HATA!";
                    result.Hata = "Geçersiz Kullanıcı ID (GUID değil)";
                    return result;
                }

                string _sorgu = "";
                _sorgu += @"select * from KullaniciKisayollari with(nolock) where KullaniciId='" + kullaniciIdStr.ToString() + "'";
                List<dynamic> entities = new List<dynamic>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _sorgu;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Id = Convert.ToString(satir["Id"]);
                        entity.KullaniciId = Convert.ToString(satir["KullaniciId"]);
                        entity.Baslik = Convert.ToString(satir["Baslik"]);
                        entity.Ikon = Convert.ToString(satir["Ikon"]);
                        entities.Add(entity);
                    }
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        public IDJsonResult Kisayol_Kaydet([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);

                string kullaniciIdStr = Convert.ToString(data["kullaniciId"]);

                if (!Guid.TryParse(kullaniciIdStr, out Guid kullaniciId))
                {
                    result.SonucKodu = -1;
                    result.Sonuc = "HATA!";
                    result.Hata = "Geçersiz Kullanıcı ID (GUID değil)";
                    return result;
                }
                string _sorgu = "";
                //    _sorgu += @"delete from [" + Uygulama_Db + "].[dbo].KullaniciKisayollari where KullaniciId='" + Convert.ToString(data["KullaniciId"]) + "'";
                _sorgu += @"delete from KullaniciKisayollari where KullaniciId='" + kullaniciId.ToString() + "'";
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = _sorgu;
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }
                _sorgu = "";
                List<KisayolModel> kisayollar = data["kisayollar"].ToObject<List<KisayolModel>>();
                foreach (var kisayol in kisayollar)
                {
                    //  _sorgu += " EXEC [" + Uygulama_Db + "].[dbo].[pKisayolKaydet]";
                    _sorgu += " exec pKisayolKaydet";
                    _sorgu += "  '" + kullaniciId.ToString() + "'";
                    _sorgu += " , '" + kisayol.Baslik + "'";
                    _sorgu += " , '" + kisayol.Ikon + "'";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = _sorgu;
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }

                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Hata = "Kısayol Kaydedildi.";
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            return result;
        }

        [HttpPost]
        public dynamic MobilAlisIrsaliyeKaydet([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                IrsaliyeUst UstBilgi = data["UstBilgi"].ToObject<IrsaliyeUst>();
                List<IrsaliyeKalemler> Kalemler = data["Kalemler"].ToObject<List<IrsaliyeKalemler>>();
                foreach (var kalem in Kalemler)
                {
                    //item.ConfirmedQuantity = item.RequestedQuantity;
                    //item.ConfirmedDeliveryDatetime = item.RequestedDeliveryDatetime;
                }
                result.SonucKodu = 1;
                result.Hata = "Irsaliye Kaydedildi.";
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult Ariza_Listesi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            List<dynamic> entities = new List<dynamic>();
            try
            {
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string kullaniciIdStr = Convert.ToString(data["kullaniciId"]);
                string Baslangic = Convert.ToString(data["Baslangic"]);
                string Bitis = Convert.ToString(data["Bitis"]);
                string Durum = Convert.ToString(data["Durum"]);
                string Teknisyen = Convert.ToString(data["Teknisyen"]);
                string Cari = Convert.ToString(data["Cari"]);
                string SeriNo = Convert.ToString(data["SeriNo"]);
                string EvrakNo = Convert.ToString(data["EvrakNo"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "[BravoKahve].[dbo].[p_ArizaListesi]";
                cmd.Parameters.AddWithValue("@UyelikID", kullaniciIdStr);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@Durum", Durum);
                cmd.Parameters.AddWithValue("@Teknisyen", Teknisyen);
                cmd.Parameters.AddWithValue("@Cari", Cari);
                cmd.Parameters.AddWithValue("@SeriNo", SeriNo);
                cmd.Parameters.AddWithValue("@EvrakNo", EvrakNo);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        var dict = (IDictionary<string, object>)entity;
                        foreach (DataColumn col in dt.Columns)
                        {
                            dict[col.ColumnName] = Convert.ToString(satir[col]);
                        }
                        entities.Add(entity);
                    }
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult Ariza_Teknisyenler([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                List<dynamic> entities = new List<dynamic>();
                string kullaniciId = Convert.ToString(data["kullaniciId"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select ID,Ad+' '+Soyad as Isim from Kullanicilar WITH(NOLOCK)";
                //cmd.CommandText = "select ID,Ad+' '+Soyad as Isim from Kullanicilar WITH(NOLOCK) Where Aktif = 1 and UyelikID = @UyelikID";
                //cmd.Parameters.AddWithValue("@UyelikID", kullaniciId);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.Isim = Convert.ToString(satir["Isim"]);
                        entities.Add(entity);
                    }
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;

        }

        [HttpPost]
        public IDJsonResult Ariza_Kategorileri([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                List<dynamic> entities = new List<dynamic>();
                string kullaniciId = Convert.ToString(data["kullaniciId"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT ID, UyelikID, Kod, Deger as Kategori, UstID, Aktif FROM  GrupKodlari";
                //cmd.CommandText = "SELECT ID, UyelikID, Kod, Deger as Kategori, UstID, Aktif FROM  GrupKodlari WHERE (Kod = 'ArizaKaynagi')";
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.Kategori = Convert.ToString(satir["Kategori"]);
                        entities.Add(entity);
                    }
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;

        }

        [HttpPost]
        public IDJsonResult Ariza_Kaynaklari([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                List<dynamic> entities = new List<dynamic>();
                string kullaniciId = Convert.ToString(data["kullaniciId"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT ID, UyelikID, Kod, Deger as Kategori, UstID, Aktif FROM  GrupKodlari";
                //  cmd.CommandText = "SELECT ID, UyelikID, Kod, Deger as Kategori, UstID, Aktif FROM  GrupKodlari WHERE (Kod = 'ArizaKaynagi')";
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.Kaynak = Convert.ToString(satir["Kategori"]);
                        entities.Add(entity);
                    }
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;

        }

        [HttpPost]
        public IDJsonResult Ariza_ParcaListesi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                List<dynamic> entities = new List<dynamic>();
                string kullaniciId = Convert.ToString(data["kullaniciId"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT ID, UyelikID, Kod, Deger as Kategori, UstID, Aktif FROM  GrupKodlari";
                //  cmd.CommandText = "SELECT ID, UyelikID, Kod, Deger as Kategori, UstID, Aktif FROM  GrupKodlari WHERE (Kod = 'ArizaKaynagi')";
                cmd.CommandType = System.Data.CommandType.Text;
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.ID = Convert.ToString(satir["ID"]);
                        entity.ParcaAdi = Convert.ToString(satir["Kategori"]);
                        entities.Add(entity);
                    }
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;

        }

        #endregion

        #region SayimKaydet
        public IDJsonResult Stok_Sayim_Kayit([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                #region Kontroller
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }
                if (data["Sube_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Sube Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Depo_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! DepoKodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Stok_Kodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Stok_Kodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Tarih"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Tarih bilgisi boş olamaz.";
                    return result;
                }

                if (data["Miktar"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Miktar bilgisi boş olamaz.";
                    return result;
                }
                if (data["KullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici Idsi bilgisi boş olamaz.";
                    return result;
                }
                #endregion Kontroller

                string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);
                DateTime Tarih = Convert.ToDateTime(data["Tarih"]);
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Belge_No = Convert.ToString(data["Belge_No"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Seri_No = Convert.ToString(data["Seri_No"]);
                decimal Miktar = Convert.ToDecimal(data["Miktar"]);
                string Aciklama = Convert.ToString(data["Aciklama"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);
                string KullaniciId = Convert.ToString(data["KullaniciId"]);
                List<dynamic> entities = new List<dynamic>();

                if (Uygulama == "IYB")
                {
                    string _sorgu = "";

                    _sorgu += " EXEC [" + Uygulama_Db + "].[dbo].[IYB_PR_SAYIM_KAYIT] ";
                    _sorgu += "  '" + Sube_Kodu + "' ";
                    _sorgu += " , '" + Depo_Kodu + "' ";
                    _sorgu += " , '" + Convert.ToDateTime(Tarih).ToString("yyyy.MM.dd") + "' ";
                    _sorgu += " , '" + Stok_Kodu + "' ";
                    _sorgu += " , '" + Seri_No + "' ";
                    _sorgu += " , '" + (Miktar).ToString().Replace(",", ".") + "' ";
                    _sorgu += " , '" + Aciklama + "' ";
                    _sorgu += " , '" + Belge_No + "' ";
                    _sorgu += " , '" + Kullanici + "' ";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = _sorgu;
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                }
                if (Uygulama == "NETSIS")
                {
                    string _sorgu = "";

                    _sorgu += " EXEC [" + Uygulama_Db + "].[dbo].[IYB_NP_SAYIM_KAYIT] ";
                    _sorgu += "  '" + Sube_Kodu + "' ";
                    _sorgu += " , '" + Depo_Kodu + "' ";
                    _sorgu += " , '" + Convert.ToDateTime(Tarih).ToString("yyyy.MM.dd") + "' ";
                    _sorgu += " , '" + Stok_Kodu + "' ";
                    _sorgu += " , '" + Seri_No + "' ";
                    _sorgu += " , '" + (Miktar).ToString().Replace(",", ".") + "' ";
                    _sorgu += " , '" + Aciklama + "' ";
                    _sorgu += " , '" + Belge_No + "' ";
                    _sorgu += " , '" + Kullanici + "' ";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = _sorgu;
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                }
                if (Uygulama == "LOGO")
                {

                }
                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = 250702;
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion

        #region SayimKaydet
        public IDJsonResult SayimKaydet([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["LisansNumarasi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! LisansNumarasi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Tarih"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Tarih bilgisi boş olamaz.";
                    return result;
                }
                if (data["SubeKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! SubeKodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["DepoKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! DepoKodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["StokKodu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! StokKodu bilgisi boş olamaz.";
                    return result;
                }
                if (data["Miktar"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Miktar bilgisi boş olamaz.";
                    return result;
                }
                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string LisansNumarasi = Convert.ToString(data["LisansNumarasi"]);
                DateTime Tarih = Convert.ToDateTime(data["Tarih"]);
                string SubeKodu = Convert.ToString(data["SubeKodu"]);
                string DepoKodu = Convert.ToString(data["DepoKodu"]);
                string StokKodu = Convert.ToString(data["StokKodu"]);
                decimal Miktar = Convert.ToDecimal(data["Miktar"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "OYP_SayimKaydet";
                cmd.Parameters.AddWithValue("@Tarih", Tarih);
                cmd.Parameters.AddWithValue("@SubeKodu", SubeKodu);
                cmd.Parameters.AddWithValue("@DepoKodu", DepoKodu);
                cmd.Parameters.AddWithValue("@StokKodu", StokKodu);
                cmd.Parameters.AddWithValue("@Miktar", Miktar);
                cmd.Parameters.AddWithValue("@Kullanici", Kullanici);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion

        #region Subabase.com İşlemleri

        private static readonly string supabaseUrl = "https://fvkgptxqequznptzszvz.supabase.co";
        private static readonly string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZ2a2dwdHhxZXF1em5wdHpzenZ6Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDAyMjEyMTYsImV4cCI6MjA1NTc5NzIxNn0.eINWQ43tKukWFwy3Y4bawKF4smyN--OPhi8dguxhTjA";

        [HttpPost]
        public async Task<IDJsonResult> InsertLogIslem([FromBody] JObject data, [FromUri] string LisansKodu = "")
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                string clientIp = HttpContext.Current?.Request.UserHostAddress ?? "IP Bulunamadı";

                var newRecord = new
                {
                    created_at = DateTime.UtcNow,
                    Sirket = Convert.ToString(data["Sirket"]),
                    Program = Convert.ToString(data["Program"]),
                    Modul = Convert.ToString(data["Modul"]),
                    Islem = Convert.ToString(data["Islem"]),
                    Baslik = Convert.ToString(data["Baslik"]),
                    Deger = Convert.ToString(data["Deger"]),
                    Kullanici = Convert.ToString(data["Kullanici"]),
                    IP = clientIp
                };
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", apiKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                    string json = JsonConvert.SerializeObject(newRecord);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync($"{supabaseUrl}/rest/v1/LOG_IT_Islemler", content);

                    if (response.IsSuccessStatusCode)
                    {
                        //Console.WriteLine("✅ Kayıt başarıyla eklendi.");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        //Console.WriteLine($"❌ Hata: {error}");
                    }
                }
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #region Lisans İşlemleri 
        [HttpPost]
        public async Task<IDJsonResult> Subabase_Lisans_LisansKaydet([FromBody] JArray data, [FromUri] string LisansKodu = "")
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Add("apikey", apiKey);

                string tableName = "Lisanslar";
                string insertUrl = $"{supabaseUrl}/rest/v1/{tableName}";

                List<dynamic> newRecords = new List<dynamic>();
                List<dynamic> updateRecords = new List<dynamic>();

                foreach (var item in data)
                {
                    // ID'yi kontrol et (null ise 0 yapma, kontrolsüz bırak!)
                    var idToken = item["id"];
                    if (idToken == null || idToken.Type == JTokenType.Null)
                    {
                        newRecords.Add(item); // Yeni kayıt ekle
                    }
                    else
                    {
                        int id = idToken.Value<int>();
                        updateRecords.Add(item); // Güncelleme listesine ekle
                    }
                }

                // 🟢 1. INSERT İşlemi (Yeni Kayıt)
                if (newRecords.Count > 0)
                {
                    string insertBody = JsonConvert.SerializeObject(newRecords);
                    HttpContent insertContent = new StringContent(insertBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage insertResponse = await client.PostAsync(insertUrl, insertContent);
                    string insertResultJson = await insertResponse.Content.ReadAsStringAsync();

                    if (!insertResponse.IsSuccessStatusCode)
                    {
                        result.SonucKodu = -1;
                        result.Sonuc = "INSERT Hatası!";
                        result.Hata = insertResultJson;
                        return result;
                    }
                }

                // 🟢 2. UPDATE İşlemi (Güncelleme)
                foreach (var updateItem in updateRecords)
                {
                    int id = updateItem["id"] != null ? Convert.ToInt32(updateItem["id"]) : 0;

                    if (id > 0)
                    {
                        string updateBody = JsonConvert.SerializeObject(updateItem);
                        HttpContent updateContent = new StringContent(updateBody, Encoding.UTF8, "application/json");

                        HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{supabaseUrl}/rest/v1/{tableName}?id=eq.{id}");
                        request.Content = updateContent;

                        HttpResponseMessage updateResponse = await client.SendAsync(request);
                        string updateResultJson = await updateResponse.Content.ReadAsStringAsync();

                        if (!updateResponse.IsSuccessStatusCode)
                        {
                            result.SonucKodu = -1;
                            result.Sonuc = "UPDATE Hatası!";
                            result.Hata = updateResultJson;
                            return result;
                        }
                    }
                }

                result.Data = "";
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
                return result;
            }
        }

        [HttpPost]
        public async Task<IDJsonResult> Subabase_Lisans_Lisanslar([FromBody] JObject data, [FromUri] string LisansKodu = "", string id = "")
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Add("apikey", apiKey);

                string tableName = "Lisanslar";
                string filtreID = "";
                if (id != "")
                {
                    filtreID = "&id=eq." + id;
                }

                int limit = 100; // Her seferinde çekilecek kayıt sayısı
                int offset = 0;  // Sayfalama için offset
                bool hasMoreData = true; // Sayfaların olup olmadığını kontrol etmek için bir bayrak

                List<dynamic> allLisanslar = new List<dynamic>(); // Tüm kayıtları tutacak liste

                while (hasMoreData)
                {
                    // İlgili sayfa verisini çekme
                    string requestUrl = $"{supabaseUrl}/rest/v1/{tableName}?{filtreID}&select=*&order=Baslangic.desc&limit={limit}&offset={offset}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    string resultJson = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var lisanslar = JsonConvert.DeserializeObject<List<dynamic>>(resultJson);
                        if (lisanslar.Count > 0)
                        {
                            // Veriyi listeye ekleyin
                            allLisanslar.AddRange(lisanslar);

                            // Eğer çekilen kayıt sayısı `limit` kadar ise, daha fazla veri olduğunu varsayabiliriz
                            if (lisanslar.Count == limit)
                            {
                                offset += limit; // Bir sonraki sayfa için offset'i arttırıyoruz
                            }
                            else
                            {
                                hasMoreData = false; // Eğer çekilen kayıt sayısı limitten az ise, daha fazla veri yok
                            }
                        }
                        else
                        {
                            hasMoreData = false; // Eğer veri yoksa, döngüyü bitir
                        }
                    }
                    else
                    {
                        // API isteği başarısız olursa hata mesajını döndür
                        result.SonucKodu = -1;
                        result.Sonuc = "HATA!";
                        result.Hata = resultJson;
                        return result;
                    }
                }

                // Tüm lisansları sonuç olarak döndürüyoruz
                result.Data = JsonConvert.SerializeObject(allLisanslar);
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            return result;
        }

        public async Task<IDJsonResult> Subabase_Lisans_LisansSil([FromBody] JObject data, [FromUri] string LisansKodu = "")
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Add("apikey", apiKey);

                string tableName = "Lisanslar";
                int id = data["id"] != null ? Convert.ToInt32(data["id"]) : 0;

                if (id == 0)
                {
                    result.SonucKodu = -1;
                    result.Sonuc = "HATA! Geçersiz id";
                    return result;
                }

                string deleteUrl = $"{supabaseUrl}/rest/v1/{tableName}?id=eq.{id}";

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, deleteUrl);
                HttpResponseMessage response = await client.SendAsync(request);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    result.SonucKodu = -1;
                    result.Sonuc = "DELETE Hatası!";
                    result.Hata = responseJson;
                    return result;
                }

                result.SonucKodu = 1;
                result.Sonuc = "Başarıyla Silindi";
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
                return result;
            }
        }

        #endregion

        #region Kullanıcı İşlemleri

        [HttpPost]
        public async Task<IDJsonResult> Subabase_KullaniciKontrol([FromBody] JObject data, [FromUri] string LisansKodu = "")
        {
            IDJsonResult result = new IDJsonResult();
            try
            {

                if (data["KullaniciAdi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Parola"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Parola bilgisi boş olamaz.";
                    return result;
                }
                if (data["Program"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Program bilgisi boş olamaz.";
                    return result;
                }

                string KullaniciAdi = Convert.ToString(data["KullaniciAdi"]);
                string Parola = Convert.ToString(data["Parola"]);
                string Program = Convert.ToString(data["Program"]);


                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Add("apikey", apiKey);

                string tableName = "Kullanicilar";
                // Burada kullanıcı adı ve parolayı filtreliyoruz
                string requestUrl = $"{supabaseUrl}/rest/v1/{tableName}?KullaniciAdi=eq.{KullaniciAdi}&Parola=eq.{Parola}&Program=eq.{Program}";

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                string resultJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(resultJson) && resultJson != "[]")
                {
                    // Kullanıcı bulundu
                    result.SonucKodu = 1;
                    result.Sonuc = "Kullanıcı doğrulandı";
                    result.Data = resultJson;
                }
                else
                {
                    // Kullanıcı yok
                    result.SonucKodu = -1;
                    result.Sonuc = "Kullanıcı bulunamadı";
                    result.Data = resultJson;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }

            return result;
        }

        #endregion

        #endregion

        #region Destek Metodları
        [HttpPost]
        public IDJsonResult DestekTipleri([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                string UyelikID = Convert.ToString(data["UyelikID"]);

                //GrupKodu1 Listesi oluşturma
                SqlCommand gorevtipiCommand = new SqlCommand();
                gorevtipiCommand.CommandText = "p_GrupKoduListesi";
                gorevtipiCommand.CommandType = System.Data.CommandType.StoredProcedure;
                gorevtipiCommand.Parameters.AddWithValue("@UyelikID", UyelikID);
                gorevtipiCommand.Parameters.AddWithValue("@Kod", "GorevTipi");
                gorevtipiCommand.Parameters.AddWithValue("@AranacakKelime", "");


                DataTable gorevTipiDataTable = (DataTable)IDVeritabani.Sorgula(gorevtipiCommand, SorgulaTuru.Tablo);

                List<GrupKoduDto> entities = new List<GrupKoduDto>();

                for (int i = 0; i < gorevTipiDataTable.Rows.Count; i++)
                {
                    GrupKoduDto entity = new GrupKoduDto();
                    entity.ID = Convert.ToString(gorevTipiDataTable.Rows[i]["ID"]);
                    entity.Deger = Convert.ToString(gorevTipiDataTable.Rows[i]["Deger"]);
                    entities.Add(entity);
                }
                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult YeniDestek([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                if (data["KullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciID bilgisi boş olamaz.";
                    return result;
                }
                if (data["SecilenKullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! SecilenKullaniciID bilgisi boş olamaz.";
                    return result;
                }
                if (data["GorevTipiID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! GorevTipiID bilgisi boş olamaz.";
                    return result;
                }
                if (data["Aciklama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Aciklama bilgisi boş olamaz.";
                    return result;
                }

                string UyelikID = Convert.ToString(data["UyelikID"]);
                string KullaniciID = Convert.ToString(data["KullaniciID"]);
                string SecilenKullaniciID = Convert.ToString(data["SecilenKullaniciID"]);
                string GorevTipiID = Convert.ToString(data["GorevTipiID"]);
                string Aciklama = Convert.ToString(data["Aciklama"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GorevKaydet";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", "");
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@GorevTipiID", GorevTipiID);
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", DateTime.Now);
                cmd.Parameters.AddWithValue("@Periyot", "T");
                cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                string SonID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));


                cmd.Parameters.Clear();
                cmd.CommandText = "p_GorevKullanicilariniSil";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@GorevID", SonID); ;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);



                cmd.Parameters.Clear();
                cmd.CommandText = "p_GorevKullaniciKaydet";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@GorevID", SonID);
                cmd.Parameters.AddWithValue("@SecilenKullaniciID", SecilenKullaniciID);
                cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                #region Kullanıcıya sms gönderme
                try
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_Kullanici";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@ID", SecilenKullaniciID);
                    string telefon = Convert.ToString(((DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo)).Rows[0]["Telefon"])
                        .Replace(" ", "")
                        .Replace("-", "")
                        .Replace("_", "")
                        .Replace(")", "")
                        .Replace("(", "");

                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_Kullanici";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@ID", KullaniciID);
                    DataTable dtKullanici = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                    string Isim = Convert.ToString(dtKullanici.Rows[0]["Ad"]) + " " + Convert.ToString(dtKullanici.Rows[0]["Soyad"]);
                    if (telefon.Trim().Length > 0)
                    {
                        string aciklama = Aciklama;
                        if (aciklama.Length > 100)
                        {
                            aciklama = aciklama.Substring(0, 90) + "...";
                        }
                        aciklama += " app.ykyazilim.com.tr";
                        string url = @"http://idyazilim.com/Site/SmsGonder/?telefon=" + telefon + "&" +
                            "KullaniciAdi=" + YKUtils.SmsKullaniciAdi + "&" +
                            "Parola=" + YKUtils.SmsParola + "&" +
                            "Isim=" + YKUtils.SmsIsim + "&" +
                            "Sirket=YK YAZILIM&" +
                            "Program=" + "YK App" + "&" +
                            "mesaj=" + Isim + " kullanıcısı " + DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " tarihli görev atadı, görev ayrıntı : " + aciklama + "";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        var sonuc = request.GetResponse();
                    }
                }
                catch (Exception err)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_HataKaydet";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@Kullanici", KullaniciID);
                    cmd.Parameters.AddWithValue("@Modul", "Gorev");
                    cmd.Parameters.AddWithValue("@Aciklama1", "~/Gorev/GorevEkle");
                    cmd.Parameters.AddWithValue("@Aciklama2", err.Message);
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }
                #endregion


                List<DosyaDto> UrunResimleri = data["Resimler"].ToObject<List<DosyaDto>>();

                foreach (DosyaDto resim in UrunResimleri)
                {
                    File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + resim.Dosya), resim.ImageByte);
                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_DosyaKaydet";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", "");
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@Modul", "Gorev");
                    cmd.Parameters.AddWithValue("@KayitID", SonID);
                    cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + resim.Dosya);
                    cmd.Parameters.AddWithValue("@Isim", resim.Dosya);
                    cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }


                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public IDJsonResult DestekCevapla([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                if (data["KullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciID bilgisi boş olamaz.";
                    return result;
                }
                if (data["GorevID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! GorevID bilgisi boş olamaz.";
                    return result;
                }
                if (data["Aciklama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Aciklama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Durumu"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Durumu bilgisi boş olamaz.";
                    return result;
                }
                if (data["TamamlamaTarihi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! TamamlamaTarihi bilgisi boş olamaz.";
                    return result;
                }
                string UyelikID = Convert.ToString(data["UyelikID"]);
                string KullaniciID = Convert.ToString(data["KullaniciID"]);
                string GorevID = Convert.ToString(data["GorevID"]);
                string Aciklama = Convert.ToString(data["Aciklama"]);
                string Durumu = Convert.ToString(data["Durumu"]);
                DateTime TamamlamaTarihi = Convert.ToDateTime(data["TamamlamaTarihi"]);


                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GorevTamamla";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GorevID", GorevID);
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                cmd.Parameters.AddWithValue("@Aciklama", Aciklama);
                cmd.Parameters.AddWithValue("@Durumu", Durumu);
                cmd.Parameters.AddWithValue("@TamamlamaTarihi", TamamlamaTarihi);
                string SonID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));



                List<DosyaDto> UrunResimleri = data["Resimler"].ToObject<List<DosyaDto>>();

                foreach (DosyaDto resim in UrunResimleri)
                {
                    File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Dosyalar/" + SonID + "_" + resim.Dosya), resim.ImageByte);
                    cmd.Parameters.Clear();
                    cmd.CommandText = "p_DosyaKaydet";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", "");
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@Modul", "Gorev");
                    cmd.Parameters.AddWithValue("@KayitID", SonID);
                    cmd.Parameters.AddWithValue("@Dosya", SonID + "_" + resim.Dosya);
                    cmd.Parameters.AddWithValue("@Isim", resim.Dosya);
                    cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                    IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                }


                #region Mail Gönder


                cmd.Parameters.Clear();
                cmd.CommandText = "Select * from MailKaliplari WITH(NOLOCK) Where UyelikID = @UyelikID and Kod = @Kod";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@Kod", "Destek_Gorev_Tamamlama");
                DataTable dtMail = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dtMail.Rows.Count > 0)
                {
                    string Baslik = Convert.ToString(dtMail.Rows[0]["Isim"]);
                    string Icerik = Convert.ToString(dtMail.Rows[0]["Icerik"]);
                    Icerik = Icerik.Replace("{Isim}", ""); //GetCookie("Isim")
                    Icerik = Icerik.Replace("{KayitNo}", GorevID.ToUpper());
                    Icerik = Icerik.Replace("{Durumu}", Durumu);
                    Icerik = Icerik.Replace("{Aciklama}", Aciklama);

                    cmd.Parameters.Clear();
                    cmd.CommandText = "select * from Parametreler WITH(NOLOCK) Where Modul = 'EMail' and UyelikID = @UyelikID";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    DataTable dtMailBilgileri = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                    YKUtils.MailGonder(Baslik, Icerik, "", //GetCookie("KullaniciAdi")
                            Convert.ToString(dtMailBilgileri.Select(" Isim = 'KullaniciAdi' ")[0]["Deger"]),
                            Convert.ToString(dtMailBilgileri.Select(" Isim = 'Parola' ")[0]["Deger"]),
                            Convert.ToString(dtMailBilgileri.Select(" Isim = 'Host' ")[0]["Deger"]),
                            Convert.ToInt32(dtMailBilgileri.Select(" Isim = 'Port' ")[0]["Deger"]),
                            Convert.ToString(dtMailBilgileri.Select(" Isim = 'SSL' ")[0]["Deger"]) == "0" ? false : true
                        );
                }

                #endregion

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public IDJsonResult DestekKayitlari([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                if (data["KullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciID bilgisi boş olamaz.";
                    return result;
                }
                if (data["Baslangic"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Baslangic bilgisi boş olamaz.";
                    return result;
                }
                if (data["Bitis"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Bitis bilgisi boş olamaz.";
                    return result;
                }
                if (data["Durum"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Durum bilgisi boş olamaz.";
                    return result;
                }
                if (data["GorevTipiID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! GorevTipiID bilgisi boş olamaz.";
                    return result;
                }
                string UyelikID = Convert.ToString(data["UyelikID"]);
                string KullaniciID = Convert.ToString(data["KullaniciID"]);
                DateTime? Baslangic = Convert.ToDateTime(data["Baslangic"]);
                DateTime? Bitis = Convert.ToDateTime(data["Bitis"]);
                string Durum = Convert.ToString(data["Durum"]);
                string GorevTipiID = Convert.ToString(data["GorevTipiID"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_GorevListesi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@KullaniciID", KullaniciID);
                cmd.Parameters.AddWithValue("@Baslangic", Baslangic);
                cmd.Parameters.AddWithValue("@Bitis", Bitis);
                cmd.Parameters.AddWithValue("@GorevTipiID", GorevTipiID);
                cmd.Parameters.AddWithValue("@KayitYapanKullanici", KullaniciID);
                cmd.Parameters.AddWithValue("@Durum", Durum);
                DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
                List<GorevDto> entities = new List<GorevDto>();
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    GorevDto entity = new GorevDto();
                    entity.ID = Convert.ToString(item["ID"]);
                    entity.GorevTipiID = Convert.ToString(item["GorevTipiID"]);
                    entity.KullaniciID = Convert.ToString(item["KayitYapanKullanici"]);
                    entity.BaslangicTarihi = Convert.ToDateTime(item["BaslangicTarihi"]);
                    entity.Aciklama = Convert.ToString(item["Aciklama"]);
                    entity.Durumu = Convert.ToString(item["Durumu"]);
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult DestekCevaplari([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                if (data["KullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciID bilgisi boş olamaz.";
                    return result;
                }
                if (data["KayitID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KayitID bilgisi boş olamaz.";
                    return result;
                }
                string UyelikID = Convert.ToString(data["UyelikID"]);
                string KullaniciID = Convert.ToString(data["KullaniciID"]);
                string KayitID = Convert.ToString(data["KayitID"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_Gorev";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                cmd.Parameters.AddWithValue("@ID", KayitID);
                DataSet ds = (DataSet)IDVeritabani.Sorgula(cmd, SorgulaTuru.DataSet);
                List<GorevDto> entities = new List<GorevDto>();
                foreach (DataRow item in ds.Tables[2].Rows)
                {
                    GorevDto entity = new GorevDto();
                    entity.ID = Convert.ToString(item["ID"]);
                    entity.UyelikID = Convert.ToString(item["UyelikID"]);
                    entity.Durumu = Convert.ToString(item["Durumu"]);
                    entity.Isim = Convert.ToString(item["KaydiKapatan"]);
                    entity.Tarih = Convert.ToDateTime(item["TamamlamaTarihi"]);
                    entity.Aciklama = Convert.ToString(item["Aciklama"]);
                    entities.Add(entity);
                }

                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion

        #region Finekra Api

        [HttpPost]
        public HttpResponseMessage Cariler(string KullaniciAdi = "", string Parola = "")
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                //Log Kaydı
            }
            catch (Exception err)
            {
                ;
            }
            try
            {
                if (KullaniciAdi == ConfigurationManager.AppSettings["WebApiKullaniciAdi"] && Parola == ConfigurationManager.AppSettings["WebApiParola"])
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Select * From w_Finekra_CariListesi";
                    cmd.CommandTimeout = 0;

                    DataTable dtCariler = new DataTable();
                    dtCariler = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                    List<FinekraCari> cariler = new List<FinekraCari>();
                    foreach (DataRow item in dtCariler.Rows)
                    {
                        cariler.Add(new FinekraCari()
                        {
                            ERPKodu = Convert.ToString(item["CARI_KOD"]),
                            CariKodu = Convert.ToString(item["CARI_KOD"]),
                            CariAdi = Convert.ToString(item["CARI_ISIM"]),
                            CariAdi2 = Convert.ToString(item["CARI_ISIM"]),
                            VergiDairesi = Convert.ToString(item["VERGI_DAIRESI"]),
                            VergiNumarasi = Convert.ToString(item["VERGI_NUMARASI"]),
                            TCKimlikNo = Convert.ToString(item["TCKIMLIKNO"]),
                            Telefon = Convert.ToString(item["CARI_TEL"]),
                            CepTelefonu = Convert.ToString(item["CARI_TEL"]),
                            FaxNumarasi = Convert.ToString(item["FAX"]),
                            Adres = Convert.ToString(item["CARI_ADRES"]),
                            CariTipi = Convert.ToString(item["CARI_TIP"]),
                            SaticiKodu = Convert.ToString(item["SATICI_KODU"]),
                            CariSinifi = Convert.ToString(item["CARI_SINIFI"]),
                            GrupKodu = Convert.ToString(item["GRUP_KODU"]),
                            IsletmeKodu = Convert.ToString(item["ISLETME_KODU"]),
                            SubeKodu = Convert.ToString(item["SUBE_KODU"]),
                            PlasiyerKodu = Convert.ToString(item["PLASIYER_KODU"]),
                            AnaCariKodu = Convert.ToString(item["BAGLI_CARI"]),
                            VeritabaniAdi = Convert.ToString(item["SIRKET_ADI"]),
                            Borc = Convert.ToDecimal(item["BORC"]),
                            Alacak = Convert.ToDecimal(item["ALACAK"]),
                            Bakiye = Convert.ToDecimal(item["BAKIYE"]),
                            MuhasebeKodu = Convert.ToString(item["MuhKod"]),
                            ReferansKodu = Convert.ToString(item["REFERANS_KODU"]),
                            ProjeKodu = Convert.ToString(item["PROJE_KODU"])
                        });
                    }
                    if (!object.Equals(cariler, null))
                    {
                        response = Request.CreateResponse<List<FinekraCari>>(HttpStatusCode.OK, cariler);
                    }
                }
                else
                {
                    response = Request.CreateResponse<string>(HttpStatusCode.OK, "Kullanıcı adı veya parola yanlış");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }

        // GET: CariHareket
        [System.Web.Http.HttpPost]
        public HttpResponseMessage FinekraCariHareketIsle(string KullaniciAdi = "", string Parola = "")
        {
            string islem = "";
            HttpResponseMessage response = new HttpResponseMessage();
            List<IslemBilgisi> result = new List<IslemBilgisi>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Baglanti"]);
            try
            {
                //

            }
            catch (Exception err)
            {
                ;
            }

            string resultDekontNo = "";
            string resultInckeyNo = "";

            FinekraCariHareket hareket = new FinekraCariHareket();

            try
            {
                islem = "1";
                if (KullaniciAdi == ConfigurationManager.AppSettings["WebApiKullaniciAdi"] && Parola == ConfigurationManager.AppSettings["WebApiParola"])
                {
                    islem = "2";
                    var content = System.Text.Encoding.UTF8.GetString(Request.Content.ReadAsByteArrayAsync().Result);


                    DataSet ds = (DataSet)JsonConvert.DeserializeObject("{ \"Item\": [" + content + "]}", (typeof(DataSet)));
                    DataTable dt = ds.Tables[0];
                    hareket.Banka = Convert.ToString(dt.Rows[0]["Banka"]);
                    hareket.DekontSeriNo = Convert.ToString(dt.Rows[0]["DekontSeriNo"]);
                    hareket.Tutar = Convert.ToDecimal(dt.Rows[0]["Tutar"].ToString().Replace(ConfigurationManager.AppSettings["Ondalik_Deger1"], ConfigurationManager.AppSettings["Ondalik_Deger2"]));
                    //hareket.Tutar = Convert.ToDecimal(dt.Rows[0]["Tutar"]);
                    hareket.Tarih = Convert.ToDateTime(dt.Rows[0]["Tarih"]);
                    try
                    {
                        hareket.ValorTarihi = Convert.ToDateTime(dt.Rows[0]["ValorTarihi"]);
                    }
                    catch
                    {
                        hareket.ValorTarihi = Convert.ToDateTime(dt.Rows[0]["Tarih"]);
                    }
                    hareket.CariKodu = Convert.ToString(dt.Rows[0]["CariKodu"]);
                    hareket.IslemTipi = Convert.ToString(dt.Rows[0]["IslemTipi"]);
                    hareket.sorumlulukMerkezi = Convert.ToString(dt.Rows[0]["sorumlulukMerkezi"]);
                    hareket.Aciklama = Convert.ToString(dt.Rows[0]["Aciklama"]);
                    hareket.EvrakNo = Convert.ToString(dt.Rows[0]["EvrakNo"]);
                    hareket.Durum = Convert.ToString(dt.Rows[0]["Durum"]);
                    hareket.SirketKodu = Convert.ToString(dt.Rows[0]["SirketKodu"]);
                    hareket.SubeKodu = Convert.ToString(dt.Rows[0]["SubeKodu"]);
                    hareket.ProjeKodu = Convert.ToString(dt.Rows[0]["ProjeKodu"]);
                    hareket.ReferansKodu = Convert.ToString(dt.Rows[0]["ReferansKodu"]);
                    hareket.BorcAlacak = Convert.ToString(dt.Rows[0]["BorcAlacak"]);
                    hareket.DovizTipi = Convert.ToString(dt.Rows[0]["DovizTipi"]);
                    hareket.DovizKuru = Convert.ToDecimal(dt.Rows[0]["DovizKuru"]);
                    hareket.DovizTutari = Convert.ToDecimal(dt.Rows[0]["DovizTutari"].ToString().Replace(ConfigurationManager.AppSettings["Ondalik_Deger1"], ConfigurationManager.AppSettings["Ondalik_Deger2"]));
                    //hareket.DovizTutari = Convert.ToDecimal(dt.Rows[0]["DovizTutari"]);
                    hareket.PlasiyerKodu = Convert.ToString(dt.Rows[0]["PlasiyerKodu"]);
                    if (ConfigurationManager.AppSettings["ProjeKodu"] != "")
                    {
                        hareket.ProjeKodu = ConfigurationManager.AppSettings["ProjeKodu"];
                    }
                    if (ConfigurationManager.AppSettings["ReferansKodu"] != "")
                    {
                        hareket.ReferansKodu = ConfigurationManager.AppSettings["ReferansKodu"];
                    }

                    try
                    {
                        File.WriteAllText(HttpContext.Current.Server.MapPath("~/Loglar/" + DateTime.Now.ToString("yyyyMMddHHmmss - ") + hareket.EvrakNo + ".txt"),

                                "Banka:" + hareket.Banka + System.Environment.NewLine +
                                "DekontSeriNo:" + hareket.DekontSeriNo + System.Environment.NewLine +
                                "Tutar:" + hareket.Tutar + System.Environment.NewLine +
                                "Tarih:" + hareket.Tarih + System.Environment.NewLine +
                                "ValorTarihi:" + hareket.ValorTarihi + System.Environment.NewLine +
                                "CariKodu:" + hareket.CariKodu + System.Environment.NewLine +
                                "IslemTipi:" + hareket.IslemTipi + System.Environment.NewLine +
                                "SorumlulukMerkezi:" + hareket.sorumlulukMerkezi + System.Environment.NewLine +
                                "Aciklama:" + hareket.Aciklama + System.Environment.NewLine +
                                "EvrakNo:" + hareket.EvrakNo + System.Environment.NewLine +
                                "Durum:" + hareket.Durum + System.Environment.NewLine +
                                "SirketKodu:" + hareket.SirketKodu + System.Environment.NewLine +
                                "SubeKodu:" + hareket.SubeKodu + System.Environment.NewLine +
                                "ReferansKodu:" + hareket.ReferansKodu + System.Environment.NewLine +
                                "BorcAlacak:" + hareket.BorcAlacak + System.Environment.NewLine +
                                "DovizTipi:" + hareket.DovizTipi + System.Environment.NewLine +
                                "DovizKuru:" + hareket.DovizKuru + System.Environment.NewLine +
                                "DovizTutari:" + hareket.DovizTutari + System.Environment.NewLine +
                                "ProjeKodu:" + hareket.ProjeKodu + System.Environment.NewLine +
                                "PlasiyerKodu:" + hareket.PlasiyerKodu + System.Environment.NewLine
                            );
                    }
                    catch (Exception hata1)
                    {
                        ;
                    }
                    if (ConfigurationManager.AppSettings["WebApiAyniBelgeNoIleHareketKontrolu"] == "E") //Kontrol 1
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = @" 
select 
* 
from TBLDEKOTRA WITH(NOLOCK) 
Where SERI_NO = '" + ConfigurationManager.AppSettings["DekontSeriNo"] + @"' and TARIH = @Tarih and KOD = @CariKodu 
and ACIKLAMA1 LIKE '%" + hareket.EvrakNo + "%' and TUTAR = @Tutar";
                        cmd.Parameters.AddWithValue("@Tarih", hareket.Tarih.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@CariKodu", hareket.CariKodu);
                        cmd.Parameters.AddWithValue("@Tutar", hareket.Tutar);
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dtKontrol = new DataTable();
                        adapter.Fill(dtKontrol);

                        if (dtKontrol.Rows.Count > 0)
                        {
                            result.Add(new IslemBilgisi() { SonDurumu = 7, EkBilgi1 = "Aynı belge numarası ile kayıt mevcut!" });
                            response = Request.CreateResponse(HttpStatusCode.OK, result);
                            return response;
                        }
                    }

                    if (true)
                    {
                        /*
                        islem = "3";
                        string RestApiKullanUrl = ConfigurationManager.AppSettings["RestApiKullanUrl"];

                        islem = "4 - " + RestApiKullanUrl;
                        oAuth2 _oAuth2;
                        _oAuth2 = new oAuth2(RestApiKullanUrl);
                        _oAuth2.Login(new JLogin()
                        {
                            BranchCode = Convert.ToInt32(hareket.SubeKodu),
                            NetsisUser = ConfigurationManager.AppSettings["NetOpenXKullaniciAdi"],
                            NetsisPassword = ConfigurationManager.AppSettings["NetOpenXParola"],
                            DbType = JNVTTipi.vtMSSQL,
                            DbName = ConfigurationManager.AppSettings["SirketKodu"] == "" ? hareket.SirketKodu : ConfigurationManager.AppSettings["SirketKodu"],
                            DbPassword = "",
                            DbUser = "TEMELSET"
                        });

                        islem = "5";
                        StatementsHeaderManager _manager = new StatementsHeaderManager(_oAuth2);
                        StatementsHeaderParam TmpstatementsHeader = new StatementsHeaderParam();
                        TmpstatementsHeader.Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"];

                        var tmpResult = _manager.NewNumber(TmpstatementsHeader);

                        islem = "6";
                        if (tmpResult.IsSuccessful == true)
                        {
                            resultDekontNo = tmpResult.Data;
                        }
                        else
                        {
                            result.Add(new IslemBilgisi() { Durum = 1, Aciklama = tmpResult.ErrorDesc });
                            response = Request.CreateResponse<List<IslemBilgisi>>(HttpStatusCode.OK, result);
                            return response;
                        }


                        islem = "7";
                        StatementsHeader Dekontbas = new StatementsHeader();
                        Dekontbas.Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"];
                        Dekontbas.Dekont_No = Convert.ToInt32(tmpResult.Data);
                        //kasa.KdvKalems = new List<JKasaKdv>();
                        Dekontbas.Kalemler = new List<Statements>();
                        islem = "8";


                        int dovizTipi = 0;
                        if (hareket.DovizTipi == "TL" || hareket.DovizTipi == "T")
                            dovizTipi = 0;
                        else if (hareket.DovizTipi == "USD")
                            dovizTipi = 1;
                        else if (hareket.DovizTipi == "EUR")
                            dovizTipi = 2;
                        else if (hareket.DovizTipi == "GBP")
                            dovizTipi = 3;

                        if (hareket.IslemTipi == "C")
                        {
                            #region Cari Kısmı

                            islem = "4.Yeni Dekont ";

                            Dekontbas.OtoVadeGunu = false;
                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = hareket.IslemTipi,
                                Kod = hareket.CariKodu,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak,
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });


                            islem = "4.Yeni Dekont Kaydı C  ";


                            #endregion

                            #region Banka Kısmı

                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = "B",

                                Kod = hareket.Banka,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak == "B" ? "A" : "B",
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });

                            islem = "4.Yeni Dekont Kaydı Banka ";


                            #endregion
                        }
                        else if (hareket.IslemTipi == "B")
                        {
                            #region Cari Kısmı

                            islem = "4.Yeni Dekont ";

                            Dekontbas.OtoVadeGunu = false;
                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = hareket.IslemTipi,
                                Kod = hareket.CariKodu,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak,
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });

                            islem = "4.Yeni Dekont Kaydı C  ";

                            #endregion

                            #region Banka Kısmı

                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = hareket.IslemTipi,

                                Kod = hareket.Banka,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak == "B" ? "A" : "B",
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });

                            islem = "4.Yeni Dekont Kaydı Banka ";


                            #endregion
                        }
                        else if (hareket.IslemTipi == "M")
                        {
                            #region Cari Kısmı

                            islem = "4.Yeni Dekont ";

                            Dekontbas.OtoVadeGunu = false;
                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = hareket.IslemTipi,
                                Kod = hareket.CariKodu,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak,
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });
                            islem = "4.Yeni Dekont Kaydı C  ";
                            #endregion
                            #region Banka Kısmı

                            Dekontbas.Kalemler.Add(new Statements
                            {
                                OtoVadeGunuGetir = false,
                                Seri_No = ConfigurationManager.AppSettings["DekontSeriNo"],
                                Dekont_No = Convert.ToInt32(resultDekontNo),
                                C_M = "B",
                                Kod = hareket.Banka,
                                Aciklama1 = hareket.Aciklama,
                                Fisno = ConfigurationManager.AppSettings["FisNoGonder"] == "E" ? hareket.EvrakNo : null,
                                Belge_Tipi = ConfigurationManager.AppSettings["BelgeTipiGonder"] == "E" ? ConfigurationManager.AppSettings["BelgeTipi"] : null,
                                Plasiyer = hareket.PlasiyerKodu != "" ? hareket.PlasiyerKodu : null,
                                B_A = hareket.BorcAlacak == "B" ? "A" : "B",
                                ValorGun = 0,
                                ValorTrh = hareket.ValorTarihi,
                                Referans = hareket.ReferansKodu.Trim().Length > 0 ? hareket.ReferansKodu : null,
                                DOVTIP = dovizTipi,
                                DOVTUT = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? 0 : Convert.ToDouble(hareket.DovizTutari),
                                ACIKLAMA3 = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "" : hareket.DovizKuru.ToString(),
                                Aciklama4 = ConfigurationManager.AppSettings["Aciklama4EvrakNoAta"] == "1" ? hareket.EvrakNo : "",
                                DovTL = hareket.DovizTipi == "TL" || hareket.DovizTipi == "T" ? "T" : "D",
                                Tarih = hareket.Tarih,
                                Tutar = Convert.ToDouble(hareket.Tutar),
                                Proje_Kodu = hareket.ProjeKodu == "" ? null : hareket.ProjeKodu,
                                Odeme_Turu = ConfigurationManager.AppSettings["OdemeTuru"] != "" ? ConfigurationManager.AppSettings["OdemeTuru"] : null,
                                DekontTip = JTDekontTip.dekCari
                            });
                            islem = "4.Yeni Dekont Kaydı Banka ";
                            #endregion
                        }


                        var resultDekontSonucu = _manager.PostInternal(Dekontbas);
                        if (resultDekontSonucu.IsSuccessful == true)
                        {
                            result.Add(new IslemBilgisi() { Durum = 0, BelgeNo = ConfigurationManager.AppSettings["DekontSeriNo"] + "-" + resultDekontNo, Aciklama = "Hareket kaydı tamamlanmıştır." });
                        }
                        else
                        {
                            result.Add(new IslemBilgisi() { Durum = 1, Aciklama = resultDekontSonucu.ErrorDesc });
                            response = Request.CreateResponse<List<IslemBilgisi>>(HttpStatusCode.OK, result);
                            return response;
                        }

                        */

                    }


                    if (!object.Equals(hareket, null))
                    {
                        result.Add(new IslemBilgisi() { SonDurumu = 2, EkBilgi1 = islem });
                        response = Request.CreateResponse<List<IslemBilgisi>>(HttpStatusCode.OK, result);
                    }
                }
                else
                {
                    result.Add(new IslemBilgisi() { SonDurumu = 1, EkBilgi1 = "Kullanıcı adı veya parola yanlış!" });
                    response = Request.CreateResponse<List<IslemBilgisi>>(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.Message == "COM object that has been separated from its underlying RCW cannot be used.")
                    {
                        //+ "-" + resultInckeyNo
                        result.Add(new IslemBilgisi() { SonDurumu = 0, EvrakNumarasi = resultDekontNo, EkBilgi1 = "Hareket kaydı tamamlanmıştır." });
                        response = Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        result.Add(new IslemBilgisi() { SonDurumu = 2, EkBilgi1 = islem + " - " + ex.Message });
                        response = Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }
                catch (Exception err2)
                {
                    result.Add(new IslemBilgisi() { SonDurumu = 2, EkBilgi1 = islem + " - " + err2.Message });
                    response = Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }

            return response;
        }



        [HttpPost]
        public IDJsonResult PersonelCalismaKaydet([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["SicilNo"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! SicilNo bilgisi boş olamaz.";
                    return result;
                }
                string SicilNo = Convert.ToString(data["SicilNo"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"                    
declare @ID nvarchar(100) = (Select NEWID())
Insert Into PersonelCalisma(ID, SicilNo, KayitTarihi, KapanmaTarihi) values(@ID, @SicilNo, GETDATE(), null)
Select @ID as ID
                    ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@SicilNo", SicilNo);
                string ID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                result.Data = ID;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        [HttpPost]
        public IDJsonResult PersonelCalismaTamamla([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["KayitID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KayitID bilgisi boş olamaz.";
                    return result;
                }
                string KayitID = Convert.ToString(data["KayitID"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"  Update PersonelCalisma Set KapanmaTarihi=GETDATE() Where ID = @KayitID                    ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@KayitID", KayitID);
                string ID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                result.Data = ID;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult PersonelSicilKontrol([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["SicilNo"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! SicilNo bilgisi boş olamaz.";
                    return result;
                }
                if (data["Parola"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Parola bilgisi boş olamaz.";
                    return result;
                }
                string SicilNo = Convert.ToString(data["SicilNo"]);
                string Parola = Convert.ToString(data["Parola"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select COUNT(*) from Kullanicilar where SicilNo = @SicilNo and PersonelParola = @Parola";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@SicilNo", SicilNo);
                cmd.Parameters.AddWithValue("@Parola", Parola);
                int kontrol = Convert.ToInt32(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                if (kontrol > 0)
                {
                    result.Data = "ok";
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.Data = "error";
                    result.SonucKodu = 0;
                    result.Sonuc = "HATA!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion

        #region Pirelli Web Api

        [HttpPost]
        public List<PirelliResponseDto> STOCKCHECK([FromBody] JObject data)
        {
            List<PirelliResponseDto> result1 = new List<PirelliResponseDto>();
            string City = "";
            try
            {
                if (data["Product"] == null)
                {
                    //result.SonucKodu = 0;
                    //result.Hata = "UYARI! Product bilgisi boş olamaz.";
                    //return result;
                }
                if (data["City"] == null)
                {
                    //result.SonucKodu = 0;
                    //result.Hata = "UYARI! City bilgisi boş olamaz.";
                    //return result;
                }

                City = Convert.ToString(data["City"]);
                List<PirelliDto> products = data["Product"].ToObject<List<PirelliDto>>();

                int sira = 0;
                foreach (PirelliDto entity in products)
                {
                    sira++;

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "p_PirelliStockCheck";
                    cmd.Parameters.AddWithValue("@StokKodu", entity.ProductCode);
                    cmd.Parameters.AddWithValue("@City", City);
                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                    foreach (DataRow item in dt.Rows)
                    {
                        PirelliResponseDto et = new PirelliResponseDto();
                        et.DeliveryDatetime = Convert.ToDateTime(item["TeslimTarihi"]).ToString("yyyy-MM-ddTHH:mm:sszzz").ToString();
                        et.Manufacturer = Convert.ToString(item["Uretici"]);
                        et.ProductCode = Convert.ToString(item["StokKodu"]);
                        et.ProductDescription = Convert.ToString(item["StokAdi"]);
                        et.ProductionDate = Convert.ToString(item["UretimYili"]);
                        et.Quantity = Convert.ToInt32(item["Miktar"]);
                        result1.Add(et);
                    }
                }

                return result1;
            }
            catch (Exception err)
            {

                PirelliResponseDto et = new PirelliResponseDto();
                et.ProductCode = City;
                et.ProductDescription = err.Message;
                result1.Add(et);
            }
            finally
            {

            }
            return result1;
        }


        [HttpPost]
        public dynamic CREATEORDER([FromBody] JObject data)
        {
            string _sira = "";
            PirelliSiparisResponseDto result1 = new PirelliSiparisResponseDto();
            try
            {
                _sira = "0";
                PirelliHeader Header = data["Header"].ToObject<PirelliHeader>();
                List<PirelliItems> Items = data["Items"].ToObject<List<PirelliItems>>();
                int sira = 1;
                foreach (var item in Items)
                {
                    item.ConfirmedQuantity = item.RequestedQuantity;
                    item.ConfirmedDeliveryDatetime = item.RequestedDeliveryDatetime;
                }
                _sira = "1";
                List<PirelliNotes> Notes = new List<PirelliNotes>();
                try
                {
                    Notes = data["Notes"].ToObject<List<PirelliNotes>>();
                }
                catch (Exception err)
                {
                    ;
                }
                string aciklama1 = "";
                if (Notes.Count >= 1)
                {
                    aciklama1 = Notes[0].Text;
                }
                _sira = "2";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_PirelliOrderSave";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrackingId", Header.TrackingId);
                cmd.Parameters.AddWithValue("@BuyerCode", Header.BuyerCode);
                cmd.Parameters.AddWithValue("@CariKodu", Header.Customer.Code);
                cmd.Parameters.AddWithValue("@CariAdi", Header.Customer.Name);
                cmd.Parameters.AddWithValue("@Adres", Header.Customer.Address.Street[0]);
                cmd.Parameters.AddWithValue("@Ilce", Header.Customer.Address.District);
                cmd.Parameters.AddWithValue("@Il", Header.Customer.Address.City);
                cmd.Parameters.AddWithValue("@SiparisNumarasi", Header.PurchaseOrderNumber);
                cmd.Parameters.AddWithValue("@Tarih", DateTimeOffset.Parse(Header.RequestedDatetime));
                cmd.Parameters.AddWithValue("@Aciklama1", aciklama1);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                _sira = "3";
                foreach (var item in Items)
                {
                    if (Convert.ToDecimal(item.Price) <= 0)
                    {
                        return new PirelliNotes()
                        {
                            Code = null,
                            Type = null,
                            Text = "Product price cannot be zero. Product Code : " + item.ProductCode
                        };
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "p_PirelliOrderLineSave";
                    cmd.Parameters.AddWithValue("@LineId", item.LineId);
                    cmd.Parameters.AddWithValue("@TrackingId", Header.TrackingId);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@RequestedDeliveryDatetime", DateTimeOffset.Parse(item.RequestedDeliveryDatetime));
                    cmd.Parameters.AddWithValue("@RequestedQuantity", item.RequestedQuantity);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(item.Price));
                    cmd.Parameters.AddWithValue("@ConfirmedDeliveryDatetime", null);
                    cmd.Parameters.AddWithValue("@ConfirmedQuantity", 0);
                    cmd.Parameters.AddWithValue("@SiparisNumarasi", dt.Rows[0]["SIPARIS_NO"]);
                    DataTable dt2 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                    item.RequestedDeliveryDatetime = DateTimeOffset.Parse(item.RequestedDeliveryDatetime).ToString("yyyy-MM-ddTHH:mm:sszzz").ToString();

                    item.ConfirmedDeliveryDatetime = DateTimeOffset.Parse(item.RequestedDeliveryDatetime).ToString("yyyy-MM-ddTHH:mm:sszzz").ToString();

                    item.ConfirmedQuantity = item.RequestedQuantity;
                    SqlCommand cmd11 = new SqlCommand();
                    cmd11.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd11.CommandText = "p_PirelliStockCheck";
                    cmd11.Parameters.AddWithValue("@StokKodu", item.ProductCode);
                    cmd11.Parameters.AddWithValue("@City", Header.Customer.Address.City);
                    DataTable dt11 = (DataTable)IDVeritabani.Sorgula(cmd11, SorgulaTuru.Tablo);
                    if (dt11.Rows.Count > 0)
                    {
                        item.ConfirmedDeliveryDatetime = DateTimeOffset.Parse(dt11.Rows[0]["TeslimTarihi"].ToString()).ToString("yyyy-MM-ddTHH:mm:sszzz").ToString();
                        if (Convert.ToInt32(dt11.Rows[0]["Miktar"]) <= 0)
                        {
                            //HAta
                            Header.RequestedDatetime = DateTimeOffset.Parse(Header.RequestedDatetime).ToString("yyyy-MM-ddTHH:mm:sszzz").ToString();

                            result1.Header = null;
                            result1.Items = null;
                            result1.Notes = new List<PirelliNotes>();
                            //result1.Notes.Add();
                            return new PirelliNotes()
                            {
                                Code = null,
                                Type = null,
                                Text = "The requested product " + item.ProductCode + " cannot be delivered on the requested delivery date. The requested delivery date should be after the " + Header.RequestedDatetime
                            };
                        }

                        if (item.RequestedQuantity <= 0)
                        {
                            Header.RequestedDatetime = DateTimeOffset.Parse(Header.RequestedDatetime).ToString("yyyy-MM-ddTHH:mm:sszzz").ToString();

                            result1.Header = null;
                            result1.Items = null;
                            result1.Notes = new List<PirelliNotes>();
                            //result1.Notes.Add();
                            return new PirelliNotes()
                            {
                                Code = null,
                                Type = null,
                                Text = "Stock quantity cannot be 0!! Product : " + item.ProductCode + ""
                            };

                        }
                        if (item.RequestedQuantity > 10000)
                        {
                            Header.RequestedDatetime = DateTimeOffset.Parse(Header.RequestedDatetime).ToString("yyyy-MM-ddTHH:mm:sszzz").ToString();

                            result1.Header = null;
                            result1.Items = null;
                            result1.Notes = new List<PirelliNotes>();
                            //result1.Notes.Add();
                            return new PirelliNotes()
                            {
                                Code = null,
                                Type = null,
                                Text = "The stock quantity cannot exceed 10,000! Product : " + item.ProductCode + ""
                            };

                        }
                        if (item.RequestedQuantity > Convert.ToInt32(dt11.Rows[0]["Miktar"]))
                        {
                            Header.RequestedDatetime = DateTimeOffset.Parse(Header.RequestedDatetime).ToString("yyyy-MM-ddTHH:mm:sszzz").ToString();

                            result1.Header = null;
                            result1.Items = null;
                            result1.Notes = new List<PirelliNotes>();
                            //result1.Notes.Add();
                            return new PirelliNotes()
                            {
                                Code = null,
                                Type = null,
                                Text = "Insufficient stock! Product : " + item.ProductCode + ""
                            };
                        }
                    }
                    else
                    {
                        result1.Header = null;
                        result1.Items = null;
                        result1.Notes = new List<PirelliNotes>();
                        //result1.Notes.Add();
                        return new PirelliNotes()
                        {
                            Code = null,
                            Type = null,
                            Text = "Stock not found! Product : " + item.ProductCode + ""
                        };
                    }
                }

                _sira = "4";
                cmd.Parameters.Clear();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "p_PirelliOrderComplate";
                cmd.Parameters.AddWithValue("@CariKodu", Header.Customer.Code);
                cmd.Parameters.AddWithValue("@TrackingId", Header.TrackingId);
                cmd.Parameters.AddWithValue("@SiparisNumarasi", dt.Rows[0]["SIPARIS_NO"]);
                DataTable dt3 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                _sira = "5";
                //Header.BuyerCode = "2400001349";
                Header.SalesOrderNumber = Convert.ToString(dt3.Rows[0]["SIPARIS_NO"]);
                Header.RequestedDatetime = DateTimeOffset.Parse(Header.RequestedDatetime).ToString("yyyy-MM-ddTHH:mm:sszzz").ToString();

                result1.Header = Header;
                result1.Items = Items;
                //result1.Notes = Notes;

                _sira = "6";

                #region Xml
                if (false)
                {
                    string dosyaadi = "DESADV" + Header.PurchaseOrderNumber + ".xml";
                    FileInfo info = new FileInfo(ConfigurationManager.AppSettings["Klasor"] + dosyaadi);
                    _sira = "7";
                    if (!info.Exists)
                    {
                        _sira = "7.1";
                        using (StreamWriter writer = info.CreateText())
                        {
                            writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                            writer.WriteLine("<ew:desadv_list xmlns:ew=\"http://www.reifen.net\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                            writer.WriteLine("  <DocumentID>B2</DocumentID>");
                            writer.WriteLine("  <Variant>2</Variant>");
                            writer.WriteLine("  <ErrorHead>");
                            writer.WriteLine("    <ErrorCode>0</ErrorCode>");
                            writer.WriteLine("  </ErrorHead>");
                            writer.WriteLine("  <NumberOfMessages>1</NumberOfMessages>");
                            writer.WriteLine("  <desadv>");
                            _sira = "7.4";
                            writer.WriteLine("    <IssueDate>" + DateTimeOffset.Parse(dt3.Rows[0]["TARIH"].ToString()).ToString("yyyy-MM-dd") + "</IssueDate>");
                            _sira = "7.4.1";
                            writer.WriteLine("    <DocumentNumber>" + dt.Rows[0]["SIPARIS_NO"] + "</DocumentNumber>");
                            writer.WriteLine("    <DespatchDate>" + DateTimeOffset.Parse(dt3.Rows[0]["TARIH"].ToString()).ToString("yyyy-MM-dd") + "</DespatchDate>");
                            writer.WriteLine("    <ArrivalDate>" + DateTimeOffset.Parse(dt3.Rows[0]["TARIH"].ToString()).ToString("yyyy-MM-dd") + "</ArrivalDate>");
                            writer.WriteLine("    <BuyerParty>");
                            writer.WriteLine("      <PartyID>" + Convert.ToString(dt3.Rows[0]["PartyId"]) + "</PartyID>");
                            writer.WriteLine("      <AgencyCode>92</AgencyCode>");
                            writer.WriteLine("    </BuyerParty>");
                            writer.WriteLine("    <Consignee>");
                            writer.WriteLine("      <PartyID>" + Header.Customer.Code + "</PartyID>");
                            writer.WriteLine("      <AgencyCode>92</AgencyCode>");
                            writer.WriteLine("    </Consignee>");
                            _sira = "7.5";

                            foreach (DataRow item in dt3.Rows)
                            {
                                writer.WriteLine("    <LineLevel>");
                                writer.WriteLine("      <LineID>" + item["LineId"] + "</LineID>");
                                writer.WriteLine("      <References>");
                                writer.WriteLine("        <SuppliersOrderReference>");
                                writer.WriteLine("          <DocumentID>" + Convert.ToString(dt3.Rows[0]["SIPARIS_NO"]) + "</DocumentID>");
                                writer.WriteLine("          <LineID>" + item["LineId"] + "</LineID>");
                                writer.WriteLine("        </SuppliersOrderReference>");
                                writer.WriteLine("        <BuyerOrderReference>");
                                writer.WriteLine("          <DocumentID>" + Convert.ToString(dt3.Rows[0]["SIPARIS_NO"]) + "</DocumentID>");
                                writer.WriteLine("          <LineID>" + item["LineId"] + "</LineID>");
                                writer.WriteLine("        </BuyerOrderReference>");
                                writer.WriteLine("      </References>");
                                writer.WriteLine("      <Article>");
                                writer.WriteLine("        <ArticleIdentification>");
                                writer.WriteLine("          <BuyersArticleID>" + Convert.ToString(item["URETICI_KODU"]) + "</BuyersArticleID>");
                                writer.WriteLine("        </ArticleIdentification>");
                                writer.WriteLine("        <ArticleDescription>");
                                writer.WriteLine("          <ArticleDescriptionText>" + Convert.ToString(item["STOK_ADI"]) + "</ArticleDescriptionText>");
                                writer.WriteLine("        </ArticleDescription>");
                                writer.WriteLine("        <DespatchedQuantity>");
                                _sira = "7.4";
                                writer.WriteLine("          <QuantityValue>" + String.Format("{0:N0}", Convert.ToDecimal(item["MIKTAR"])) + "</QuantityValue>");
                                _sira = "7.5";
                                writer.WriteLine("          <MeasureUnitCode>" + Convert.ToString(item["OLCU_BR"]) + "</MeasureUnitCode>");
                                writer.WriteLine("        </DespatchedQuantity>");
                                writer.WriteLine("      </Article>");
                                writer.WriteLine("    </LineLevel>");
                            }
                            _sira = "7.6";
                            writer.WriteLine("  </desadv>");
                            writer.WriteLine("</ew:desadv_list>");
                            _sira = "7.7";

                        }
                    }
                    _sira = "8";
                    if (true) //sftp    2025-04-24 tarihinde ayrı metod haline getirildi - Yunus KÖSE
                    {
                        _sira = "9";
                        var client = new SftpClient("mft.pirelli.com", 22, "Sertglobal_prod", "wFdEPr38UDEH"); // Canlı
                        //var client = new SftpClient("mfttest.pirelli.com", 22, "Sertglobal_test", "mA8CD5eZ5mth"); // Test
                        _sira = "10";
                        var fileStream = new FileStream(ConfigurationManager.AppSettings["Klasor"] + dosyaadi, FileMode.Open);
                        _sira = "10.1";
                        client.Connect();
                        _sira = "10.2";
                        client.UploadFile(fileStream, "/LD/TR/SERTGLOBAL/from/DeliveryStatus/" + dosyaadi);
                        _sira = "11";
                        //client.DownloadFile("/documents/document2.docx", stream);
                        _sira = "12";
                        //client.Delete("/documents/document1.docx");
                        _sira = "13";
                        client.Disconnect();
                        _sira = "14";
                        client.Dispose();
                        _sira = "15";
                    }

                }
                #endregion

                return result1;
            }
            catch (Exception err)
            {
                result1.Header = null;
                result1.Items = null;
                result1.Notes = new List<PirelliNotes>();
                //result1.Notes.Add();
                return new PirelliNotes()
                {
                    Code = null,
                    Type = null,
                    Text = _sira + " - " + err.Message
                };
            }
            finally
            {

            }
            return result1;
        }


        #region Crm Cagri Kayitlari
        public IDJsonResult Crm_CagriKaydi_Olustur([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Uygulama"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama bilgisi boş olamaz.";
                    return result;
                }
                if (data["Uygulama_Db"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Uygulama_Db bilgisi boş olamaz.";
                    return result;
                }

                if (data["Kullanici"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string _srg = "";
                string Uygulama = Convert.ToString(data["Uygulama"]);
                string Uygulama_Db = Convert.ToString(data["Uygulama_Db"]);
                string Sube_Kodu = Convert.ToString(data["Sube_Kodu"]);
                if (Sube_Kodu == "")
                {
                    Sube_Kodu = "0";
                }
                string Depo_Kodu = Convert.ToString(data["Depo_Kodu"]);
                if (Depo_Kodu == "")
                {
                    Depo_Kodu = "0";
                }
                string Stok_Kodu = Convert.ToString(data["Stok_Kodu"]);
                string Seri_Lot = Convert.ToString(data["Seri_No"]);
                string Seri_TicariAdi = Convert.ToString(data["Seri_Ticari_Adi"]);
                string Seri_Tedarikci = Convert.ToString(data["Tedarikci_Kodu"]);
                string Seri_Skt = Convert.ToString(data["Seri_Skt"]);
                string Seri_Ambalaj = Convert.ToString(data["Seri_Ambalaj"]);
                string Seri_RafNo = Convert.ToString(data["Seri_RafNo"]);
                string Seri_RafSire = Convert.ToString(data["Seri_RafSira"]);
                string Kullanici = Convert.ToString(data["Kullanici"]);

                if (Uygulama == "NETSIS")
                {
                    _srg = "INSERT INTO [" + Uygulama_Db + "].[dbo].[TBLSERITRA] ";
                    _srg += " \r\n ( KAYIT_TIPI, SUBE_KODU, SERI_NO, STOK_KODU ";
                    _srg += " \r\n , HARACIK, TARIH ";
                    _srg += " \r\n , ACIK1, ACIK2, ACIK3, ACIKLAMA_4, ACIKLAMA_5 ";
                    _srg += " \r\n , MIKTAR, SON_KULLANMA_TARIHI, BARKOD ";
                    _srg += " \r\n , GCKOD, DEPOKOD, BELGENO, BELGETIP ";
                    _srg += " \r\n ) ";
                    _srg += " \r\n SELECT 'D' AS KAYIT_TIPI, '" + Sube_Kodu + "' SUBE_KODU, '" + Seri_Lot + "' AS SERI_NO, '" + Stok_Kodu + "' AS STOK_KODU  ";
                    _srg += " \r\n , '" + Seri_Tedarikci + "' AS HARACIK, CONVERT(nvarchar, GETDATE(),102) AS TARIH ";
                    _srg += " \r\n , LEFT('" + Seri_TicariAdi + "',50) as ACIK1, LEFT('" + Seri_Ambalaj + "',50) as ACIK2, LEFT('" + Seri_RafNo + "',50) as ACIK3, '" + Seri_RafSire + "' as ACIKLAMA_4, 'Wms_Seri' as ACIKLAMA_5 ";
                    _srg += " \r\n , 0 MIKTAR, '" + Seri_Skt + "' SON_KULLANMA_TARIHI, '" + Seri_Lot + "' AS BARKOD  ";
                    _srg += " \r\n , 'G' AS GCKOD, '" + Depo_Kodu + "' AS DEPOKOD, left('" + Kullanici + "',15) AS BELGENO, NULL AS BELGETIP ";

                    _srg += " \r\n INSERT INTO INNOVA.[dbo].TBLLOGUSER ";
                    _srg += " \r\n ( FORM, TARIH, KAYITID, BELGE_NO ";
                    _srg += " \r\n , KULLANICI, CARI_KODU ";
                    _srg += " \r\n , BILGI, ISLEM, KAYNAK) ";
                    _srg += " \r\n SELECT 'Web Servis Wms 01 Qr tanımlama' AS FORM, getdate(), '" + Stok_Kodu + "' AS KAYITID, '" + Seri_Lot + "' AS BELGE_NO ";
                    _srg += " \r\n , '" + Kullanici + "' AS KULLANICI, '" + Seri_Tedarikci + "' AS CARI_KODU ";
                    _srg += " \r\n , '" + Seri_Lot + ':' + Seri_Tedarikci + ':' + Seri_Ambalaj + "' BILGI, 'Kullanici Güncellemesi' as ISLEM, 'Netsis_Wms_Qr_Olustur' AS KAYNAK ";
                }
                if (Uygulama == "LOGO")
                {

                }
                List<dynamic> entities = new List<dynamic>();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = _srg;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);


                result.Data = entities;
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                result.Sonuc_Versiyon = 250702;
                return result;


            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        #endregion Crm Cagri Kayitlari

        /// <summary>
        /// DOMAINNNN.com/api/YKWebApi/ComplateOrder/?CariKodu=XXXXXXXXX&SiparisNo=YYYYYYYY&TrackingId=ZZZZZZ
        /// </summary>
        /// <param name="CariKodu"></param>
        /// <param name="SiparisNo"></param>
        /// <param name="TrackingId"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic ComplateOrder(string CariKodu, string SiparisNo)
        {
            string _sira = "";
            _sira = "4";
            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.Clear();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "p_PirelliOrderDelivery";
            cmd.Parameters.AddWithValue("@CariKodu", CariKodu);
            cmd.Parameters.AddWithValue("@TrackingId", "");
            cmd.Parameters.AddWithValue("@SiparisNumarasi", SiparisNo);
            DataTable dt3 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

            _sira = "5";

            if (true)
            {
                string dosyaadi = "DESADV" + SiparisNo + ".xml";
                FileInfo info = new FileInfo(ConfigurationManager.AppSettings["Klasor"] + dosyaadi);
                _sira = "7";
                if (!info.Exists)
                {
                    _sira = "7.1";
                    using (StreamWriter writer = info.CreateText())
                    {
                        writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        writer.WriteLine("<ew:desadv_list xmlns:ew=\"http://www.reifen.net\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                        writer.WriteLine("  <DocumentID>B2</DocumentID>");
                        writer.WriteLine("  <Variant>2</Variant>");
                        writer.WriteLine("  <ErrorHead>");
                        writer.WriteLine("    <ErrorCode>0</ErrorCode>");
                        writer.WriteLine("  </ErrorHead>");
                        writer.WriteLine("  <NumberOfMessages>1</NumberOfMessages>");
                        writer.WriteLine("  <desadv>");
                        _sira = "7.4";
                        writer.WriteLine("    <IssueDate>" + DateTimeOffset.Parse(dt3.Rows[0]["TARIH"].ToString()).ToString("yyyy-MM-dd") + "</IssueDate>");
                        _sira = "7.4.1";
                        writer.WriteLine("    <DocumentNumber>" + Convert.ToString(dt3.Rows[0]["IRSALIYE_NO"]) + "</DocumentNumber>");
                        writer.WriteLine("    <DespatchDate>" + DateTimeOffset.Parse(dt3.Rows[0]["TARIH"].ToString()).ToString("yyyy-MM-dd") + "</DespatchDate>");
                        writer.WriteLine("    <ArrivalDate>" + DateTimeOffset.Parse(dt3.Rows[0]["TARIH"].ToString()).ToString("yyyy-MM-dd") + "</ArrivalDate>");
                        _sira = "7.4.2";
                        writer.WriteLine("    <BuyerParty>");
                        writer.WriteLine("      <PartyID>" + Convert.ToString(dt3.Rows[0]["PartyId"]) + "</PartyID>");
                        writer.WriteLine("      <AgencyCode>92</AgencyCode>");
                        writer.WriteLine("    </BuyerParty>");
                        _sira = "7.4.3";
                        writer.WriteLine("    <Consignee>");
                        writer.WriteLine("      <PartyID>" + Convert.ToString(dt3.Rows[0]["TESLIM_CARI_KODU"]) + "</PartyID>");
                        writer.WriteLine("      <AgencyCode>92</AgencyCode>");
                        writer.WriteLine("    </Consignee>");
                        _sira = "7.5";
                        foreach (DataRow item in dt3.Rows)
                        {
                            writer.WriteLine("    <LineLevel>");
                            writer.WriteLine("      <LineID>" + Convert.ToString(item["LineID"]) + "</LineID>");
                            writer.WriteLine("      <References>");
                            writer.WriteLine("        <SuppliersOrderReference>");
                            writer.WriteLine("          <DocumentID>" + Convert.ToString(item["SIPARIS_NO"]) + "</DocumentID>");
                            writer.WriteLine("          <LineID>" + Convert.ToString(item["LineID"]) + "</LineID>");
                            writer.WriteLine("        </SuppliersOrderReference>");
                            writer.WriteLine("        <BuyerOrderReference>");
                            writer.WriteLine("          <DocumentID>" + Convert.ToString(item["SIPARIS_NO"]) + "</DocumentID>");
                            writer.WriteLine("          <LineID>" + Convert.ToString(item["LineID"]) + "</LineID>");
                            writer.WriteLine("        </BuyerOrderReference>");
                            writer.WriteLine("      </References>");
                            writer.WriteLine("      <Article>");
                            writer.WriteLine("        <ArticleIdentification>");
                            writer.WriteLine("          <BuyersArticleID>" + Convert.ToString(item["URETICI_KODU"]) + "</BuyersArticleID>");
                            writer.WriteLine("        </ArticleIdentification>");
                            writer.WriteLine("        <ArticleDescription>");
                            writer.WriteLine("          <ArticleDescriptionText>" + Convert.ToString(item["STOK_ADI"]) + "</ArticleDescriptionText>");
                            writer.WriteLine("        </ArticleDescription>");
                            writer.WriteLine("        <DespatchedQuantity>");
                            _sira = "7.4";
                            writer.WriteLine("          <QuantityValue>" + String.Format("{0:N0}", Convert.ToDecimal(item["MIKTAR"])) + "</QuantityValue>");
                            _sira = "7.5";
                            writer.WriteLine("          <MeasureUnitCode>" + Convert.ToString(item["OLCU_BR"]) + "</MeasureUnitCode>");
                            writer.WriteLine("        </DespatchedQuantity>");
                            writer.WriteLine("      </Article>");
                            writer.WriteLine("    </LineLevel>");
                        }
                        _sira = "7.6";
                        writer.WriteLine("  </desadv>");
                        writer.WriteLine("</ew:desadv_list>");
                        _sira = "7.7";

                    }
                }
                _sira = "8";
                if (true) //sftp
                {
                    _sira = "9";
                    var client = new SftpClient("mft.pirelli.com", 22, "Sertglobal_prod", "wFdEPr38UDEH"); // Canlı
                    _sira = "10";
                    var fileStream = new FileStream(ConfigurationManager.AppSettings["Klasor"] + dosyaadi, FileMode.Open);
                    _sira = "10.1";
                    client.Connect();
                    _sira = "10.2";
                    client.UploadFile(fileStream, "/LD/TR/SERTGLOBAL/from/DeliveryStatus/" + dosyaadi);
                    _sira = "11";
                    //client.DownloadFile("/documents/document2.docx", stream);
                    _sira = "12";
                    //client.Delete("/documents/document1.docx");
                    _sira = "13";
                    client.Disconnect();
                    _sira = "14";
                    client.Dispose();
                    _sira = "15";
                }

            }

            return new PirelliNotes()
            {
                Code = null,
                Type = null,
                Text = "Complated."
            };

        }
        [HttpPost]
        public PirelliSiparisResponseDto CREATEORDERR(SupplierOrders data)
        {
            string _sira = "";
            PirelliSiparisResponseDto result1 = new PirelliSiparisResponseDto();
            try
            {
                _sira = "0";
                //SupplierOrders Header = data["Header"].ToObject<SupplierOrders>();
                //List<SupplierOrderProductsList> Items = data["Items"].ToObject<List<SupplierOrderProductsList>>();
                int sira = 1;
                //foreach (var item in Items)
                //{
                //    item.ConfirmedQuantity = item.RequestedQuantity;
                //    item.ConfirmedDeliveryDatetime = item.RequestedDeliveryDatetime;
                //}
                _sira = "1";
                //List<PirelliNotes> Notes = new List<PirelliNotes>();
                //try
                //{
                //    Notes = data["Notes"].ToObject<List<PirelliNotes>>();
                //}
                //catch (Exception err)
                //{
                //    ;
                //}
                //string aciklama1 = "";
                //if (Notes.Count >= 1)
                //{
                //    aciklama1 = Notes[0].Text;
                //}
                _sira = "2";
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = "p_PirelliOrderSave";
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@TrackingId", Header.TrackingId);
                //cmd.Parameters.AddWithValue("@BuyerCode", Header.BuyerCode);
                //cmd.Parameters.AddWithValue("@CariKodu", Header.Customer.Code);
                //cmd.Parameters.AddWithValue("@CariAdi", Header.Customer.Name);
                //cmd.Parameters.AddWithValue("@Adres", Header.Customer.Address.Street[0]);
                //cmd.Parameters.AddWithValue("@Ilce", Header.Customer.Address.District);
                //cmd.Parameters.AddWithValue("@Il", Header.Customer.Address.City);
                //cmd.Parameters.AddWithValue("@SiparisNumarasi", Header.PurchaseOrderNumber);
                //cmd.Parameters.AddWithValue("@Tarih", Convert.ToDateTime(Header.RequestedDatetime));
                //cmd.Parameters.AddWithValue("@Aciklama1", aciklama1);
                //DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                _sira = "3";
                //foreach (var item in Items)
                //{
                //    //cmd.Parameters.Clear();
                //    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //    //cmd.CommandText = "p_PirelliOrderLineSave";
                //    //cmd.Parameters.AddWithValue("@LineId", item.LineId);
                //    //cmd.Parameters.AddWithValue("@TrackingId", Header.TrackingId);
                //    //cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                //    //cmd.Parameters.AddWithValue("@RequestedDeliveryDatetime", Convert.ToDateTime(item.RequestedDeliveryDatetime));
                //    //cmd.Parameters.AddWithValue("@RequestedQuantity", item.RequestedQuantity);
                //    //cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(item.Price));
                //    //cmd.Parameters.AddWithValue("@ConfirmedDeliveryDatetime", Convert.ToDateTime(item.ConfirmedDeliveryDatetime));
                //    //cmd.Parameters.AddWithValue("@ConfirmedQuantity", item.ConfirmedQuantity);
                //    //cmd.Parameters.AddWithValue("@SiparisNumarasi", dt.Rows[0]["SIPARIS_NO"]);
                //    //DataTable dt2 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                //    //item.ConfirmedDeliveryDatetime = DateTime.Now.ToString("yyyy-MM-dd") + "T03:00:00+03:00";
                //    //item.RequestedDeliveryDatetime = Convert.ToDateTime(item.RequestedDeliveryDatetime).ToString("yyyy-MM-dd") + "T03:00:00+03:00";
                //}

                _sira = "4";
                //cmd.Parameters.Clear();
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.CommandText = "p_PirelliOrderComplate";
                ////cmd.Parameters.AddWithValue("@CariKodu", Header.Customer.Code);
                ////cmd.Parameters.AddWithValue("@TrackingId", Header.TrackingId);
                //cmd.Parameters.AddWithValue("@SiparisNumarasi", dt.Rows[0]["SIPARIS_NO"]);
                //DataTable dt3 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                _sira = "5";
                //Header.BuyerCode = "2400001349";
                //Header.SalesOrderNumber = Convert.ToString(dt3.Rows[0]["SIPARIS_NO"]);
                //Header.RequestedDatetime = Convert.ToDateTime(dt3.Rows[0]["TARIH"]).ToString("yyyy-MM-dd") + "T03:00:00+03:00";
                //result1.Header = Header;
                //result1.Items = Items;
                ////result1.Notes = Notes;
                //_sira = "6";
                return result1;
            }
            catch (Exception err)
            {
                result1.Notes = new List<PirelliNotes>();
                result1.Notes.Add(new PirelliNotes() { Text = _sira + " - " + err.Message });
            }
            finally
            {

            }
            return result1;
        }

        #endregion

        #region İmece Web Api

        [HttpPost]
        public List<SupplierOrder> ImaceSiparisKaydi([FromBody] JObject data)
        {
            string _sira = "";
            List<SupplierOrder> result1 = new List<SupplierOrder>();
            List<SupplierOrder> Headers = data["SupplierOrders"].ToObject<List<SupplierOrder>>();
            try
            {
                _sira = "0";

                int sira = 1;
                foreach (SupplierOrder Header in Headers)
                {
                    _sira = "1";
                    _sira = "2";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "p_ImeceOrderSave";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderNumber", Header.OrderNumber);
                    cmd.Parameters.AddWithValue("@SupplierOrderNumber", Header.SupplierOrderNumber);
                    cmd.Parameters.AddWithValue("@OrderStatus", Header.OrderStatus);
                    cmd.Parameters.AddWithValue("@StatusDescription", Header.StatusDescription);
                    cmd.Parameters.AddWithValue("@TotalAmount", Header.TotalAmount);
                    cmd.Parameters.AddWithValue("@CompanyName", Header.CompanyName);
                    cmd.Parameters.AddWithValue("@TaxNumber", Header.TaxNumber);
                    cmd.Parameters.AddWithValue("@OrderDate", Header.OrderDate);
                    cmd.Parameters.AddWithValue("@DeliveryDate", Header.DeliveryDate);
                    cmd.Parameters.AddWithValue("@Description", Header.Description);

                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                    _sira = "3";
                    foreach (SupplierOrderProducts satir in Header.SupplierOrderProductsList)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "p_ImeceOrderLineSave";
                        cmd.Parameters.AddWithValue("@ProductName", satir.ProductName);
                        cmd.Parameters.AddWithValue("@ProductCode", satir.ProductCode);
                        cmd.Parameters.AddWithValue("@ProductBarcode", satir.ProductBarcode);
                        cmd.Parameters.AddWithValue("@Quantity", satir.Quantity);
                        cmd.Parameters.AddWithValue("@Price", satir.Price);
                        cmd.Parameters.AddWithValue("@TotalAmount", satir.TotalAmount);
                        cmd.Parameters.AddWithValue("@SiparisNumarasi", dt.Rows[0]["SIPARIS_NO"]);
                        DataTable dt2 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                        //item.ConfirmedDeliveryDatetime = DateTime.Now.ToString("yyyy-MM-dd") + "T03:00:00+03:00";
                    }

                    _sira = "4";
                    cmd.Parameters.Clear();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "p_ImeceOrderComplate";
                    cmd.Parameters.AddWithValue("@SiparisNumarasi", dt.Rows[0]["SIPARIS_NO"]);
                    DataTable dt3 = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);


                }
                _sira = "5";
                result1 = Headers;
                //result1.Notes = Notes;

                _sira = "6";

                return result1;
            }
            catch (Exception err)
            {
                result1 = Headers;
                foreach (var item in result1)
                {
                    item.Bilgi = err.Message;
                }
            }
            finally
            {

            }
            return result1;
        }
        #endregion

        #region Whatsapp Api

        public IDJsonResult WPMesajBilgisiOlustur([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["Telefon"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Telefon bilgisi boş olamaz.";
                    return result;
                }
                if (data["Mesaj"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Mesaj bilgisi boş olamaz.";
                    return result;
                }
                if (data["DosyaUrl"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! DosyaUrl bilgisi boş olamaz.";
                    return result;
                }
                string Telefon = Convert.ToString(data["Telefon"]);
                string Mesaj = Convert.ToString(data["Mesaj"]);
                string DosyaUrl = Convert.ToString(data["DosyaUrl"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Insert Into WhatsAppMesajlari (Telefon,Mesaj,DosyaUrl) values (@Telefon,@Mesaj,@DosyaUrl)";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Telefon", Telefon);
                cmd.Parameters.AddWithValue("@Mesaj", Mesaj);
                cmd.Parameters.AddWithValue("@DosyaUrl", DosyaUrl);
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                result.Data = "";
                result.SonucKodu = 1;
                result.Sonuc = "Mesaj kaydı oluşturuldu.";

                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        public IDJsonResult WPMesajGonderildi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["ID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! ID bilgisi boş olamaz.";
                    return result;
                }
                string ID = Convert.ToString(data["ID"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
Update WhatsAppMesajlari Set Gonderildi = 1 Where ID = @ID

";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", ID);

                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                {
                    result.Data = "";
                    result.SonucKodu = 1;
                    result.Sonuc = "Mesaj gönderimi sağlandı.";
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        public IDJsonResult WPMesajBilgisi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
declare @ID nvarchar(100) = (select top(1) ID from WhatsAppMesajlari WITH(NOLOCK) Where Iletildi = 0)
Update WhatsAppMesajlari Set Iletildi = 1 Where ID = @ID
select * from WhatsAppMesajlari WITH(NOLOCK) Where ID = @ID

";
                cmd.CommandType = System.Data.CommandType.Text;

                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    result.Data = new MesajlasmaDto()
                    {
                        ID = (Guid)(dt.Rows[0]["ID"]),
                        Telefon = Convert.ToString(dt.Rows[0]["Telefon"]),
                        Mesaj = Convert.ToString(dt.Rows[0]["Mesaj"]),
                        Dosya = Convert.ToString(dt.Rows[0]["DosyaUrl"])
                    };
                    result.SonucKodu = 1;
                    result.Sonuc = "Mesaj detayı iletildi.";
                }
                else
                {
                    result.Data = "";
                    result.SonucKodu = 0;
                    result.Sonuc = "Gönderilecek mesaj bulunamadı.";
                }
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        public IDJsonResult WPOturumBilgisiOlustur([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"

if NOT EXISTS( select * from WhatsAppOturumlari WITH(NOLOCK) Where OturumIstegi = 0 and OturumKapatilsin = 0 )
BEGIN
    Insert Into WhatsAppOturumlari (KayitTarihi,OturumIstegi,OturumAcildi,OturumKapatilsin) values (GETDATE(),0,0,0)
END

";
                cmd.CommandType = System.Data.CommandType.Text;
                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);
                result.Data = "";
                result.SonucKodu = 1;
                result.Sonuc = "Oturum kaydı oluşturuldu.";

                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        public IDJsonResult WPOturumBilgisi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"

if EXISTS( select * from WhatsAppOturumlari WITH(NOLOCK) Where OturumIstegi = 0 and OturumKapatilsin = 0 )
BEGIN
    declare @ID nvarchar(100)=(select top(1) ID from WhatsAppOturumlari WITH(NOLOCK) Where OturumIstegi = 0 and OturumKapatilsin = 0)
    Update WhatsAppOturumlari Set
        OturumIstegi=1
    Where ID = @ID
    select @ID
END
eLSE
BEGIN
	Select top(1) ID From  WhatsAppOturumlari WITH(NOLOCK) Where OturumIstegi = 0 and OturumKapatilsin = 0
END

";
                cmd.CommandType = System.Data.CommandType.Text;

                string SonID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));
                if (SonID.Trim() != "")
                {
                    result.Data = SonID;
                    result.SonucKodu = 1;
                    result.Sonuc = "Oturum kaydı açıldı. ID:" + SonID;
                }
                else
                {
                    result.Data = "";
                    result.SonucKodu = 0;
                    result.Sonuc = "Sistemde bekleyen oturum mevcut.";
                }
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        public IDJsonResult WPOturumBarkoduGonder([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["ID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! ID bilgisi boş olamaz.";
                    return result;
                }
                if (data["Barkod"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Barkod bilgisi boş olamaz.";
                    return result;
                }
                string ID = Convert.ToString(data["ID"]);
                string Barkod = Convert.ToString(data["Barkod"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update WhatsAppOturumlari set OturumIstegi=@OturumIstegi,OturumBarkodu=@OturumBarkodu Where ID = @ID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@OturumIstegi", true);
                cmd.Parameters.AddWithValue("@OturumBarkodu", Barkod);

                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                {
                    result.Data = "";
                    result.SonucKodu = 1;
                    result.Sonuc = "Barkod bilgisi sisteme işlendi.";
                }
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        public IDJsonResult WPOturumKapatilacakmi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Select ID From WhatsAppOturumlari WITH(NOLOCK) Where OturumKapatilsin = 1";
                cmd.CommandType = System.Data.CommandType.Text;

                string ID = Convert.ToString(IDVeritabani.Sorgula(cmd, SorgulaTuru.Tek));

                if (ID.Trim() != "")
                {
                    result.Data = ID;
                    result.SonucKodu = 1;
                    result.Sonuc = "Oturum kapatılacak.";
                }
                else
                {
                    result.Data = "";
                    result.SonucKodu = 0;
                    result.Sonuc = "Oturum devam edecek.";
                }
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        public IDJsonResult WPOturumKapat([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["ID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! ID bilgisi boş olamaz.";
                    return result;
                }
                string ID = Convert.ToString(data["ID"]);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Delete From  WhatsAppOturumlari Where ID = @ID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", ID);

                IDVeritabani.Sorgula(cmd, SorgulaTuru.Bos);

                {
                    result.Data = "";
                    result.SonucKodu = 1;
                    result.Sonuc = "Oturum başarıyla kapatıldı.";
                }
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }
        #endregion

        [HttpPost]
        public IDJsonResult LogKaydet_KullaniciGirisi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                string ProgramAdi = Convert.ToString(data["ProgramAdi"]);
                string Sirket = Convert.ToString(data["Sirket"]);
                string KullaniciAdi = Convert.ToString(data["KullaniciAdi"]);
                string Parola = Convert.ToString(data["Parola"]);
                string IP = Convert.ToString(data["IP"]);
                YKUtils.LogKaydet_KullaniciGirisi(ProgramAdi,
                        Sirket,
                        KullaniciAdi,
                        Parola,
                        IP
                        );
                result.SonucKodu = 1;
                result.Sonuc = "Başarılı";
                return result;
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult KullaniciGirisi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["KullaniciAdi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Parola"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                string KullaniciAdi = Convert.ToString(data["KullaniciAdi"]);
                string Parola = Convert.ToString(data["Parola"]);

                YKModelKullanici entity = new YKModelKullanici();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "p_KullaniciGirisi";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
                cmd.Parameters.AddWithValue("@Parola", Parola);
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    string Bilgi = Convert.ToString(dt.Rows[0]["Bilgi"]);
                    if (!Bilgi.StartsWith("UYARI!"))
                    {
                        #region Cookie İşlemleri
                        entity.Isim = Convert.ToString(dt.Rows[0]["Ad"]) + " " + Convert.ToString(dt.Rows[0]["Soyad"]);
                        entity.KullaniciID = Convert.ToString(dt.Rows[0]["ID"]);
                        entity.UyelikIsim = Convert.ToString(dt.Rows[0]["UyelikIsim"]);
                        entity.UyelikID = Convert.ToString(dt.Rows[0]["UyelikID"]);
                        entity.KullaniciAdi = Convert.ToString(dt.Rows[0]["KullaniciAdi"]);
                        entity.Parola = Convert.ToString(dt.Rows[0]["Parola"]);
                        entity.Resim = Convert.ToString(dt.Rows[0]["Resim"]);
                        entity.Uygulama = Convert.ToString(dt.Rows[0]["Uygulama"]);
                        entity.Uygulama_Db = Convert.ToString(dt.Rows[0]["Uygulama_Db"]);
                        entity.Uygulama_Sube_Kodu = Convert.ToString(dt.Rows[0]["Uygulama_Sube_Kodu"]);
                        entity.Uygulama_Depo_Kodu = Convert.ToString(dt.Rows[0]["Uygulama_Depo_Kodu"]);
                        #endregion

                        result.Data = entity;
                        result.SonucKodu = 1;
                        result.Sonuc = "Başarılı";
                        return result;
                    }
                    else
                    {
                        result.SonucKodu = 0;
                        result.Hata = Bilgi;
                        return result;
                    }
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanıcı bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult Baglanti_Kontrol([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                List<dynamic> entities = new List<dynamic>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select GETDATE() AS TARIH ";
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);
                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.Tarih = Convert.ToString(satir["TARIH"]);
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult KullaniciKisitlari([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["KullaniciID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanici bilgisi boş olamaz.";
                    return result;
                }
                string KullaniciId = Convert.ToString(data["KullaniciID"]);
                List<dynamic> entities = new List<dynamic>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select * from [dbo].[KullaniciKisitlari] with (nolock) where KullaniciId =  '" + KullaniciId + "' ";
                DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                if (dt.Rows.Count > 0)
                {
                    #region Cookie İşlemleri
                    foreach (DataRow satir in dt.Rows)
                    {
                        dynamic entity = new System.Dynamic.ExpandoObject();
                        entity.KisitId = Convert.ToString(satir["KisitId"]);
                        entity.KisitTuru = Convert.ToString(satir["KisitTuru"]);
                        entity.Kisit = Convert.ToString(satir["Kisit"]);
                        entities.Add(entity);
                    }
                    #endregion
                    result.Data = entities;
                    result.SonucKodu = 1;
                    result.Sonuc = "Başarılı";
                    return result;
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kayıt bulunamadı!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult KullaniciListesi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {
                if (data["KullaniciAdi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Parola"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Parola bilgisi boş olamaz.";
                    return result;
                }
                if (data["UyelikID"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! UyelikID bilgisi boş olamaz.";
                    return result;
                }
                string KullaniciAdi = Convert.ToString(data["KullaniciAdi"]);
                string Parola = Convert.ToString(data["Parola"]);
                string UyelikID = Convert.ToString(data["UyelikID"]);

                if (KullaniciAdi == "info@ykyazilim.com.tr" && Parola == "4jtj2jsmv")
                {

                    List<dynamic> entities = new List<dynamic>();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "p_KullaniciListesi";
                    cmd.Parameters.AddWithValue("@UyelikID", UyelikID);
                    cmd.Parameters.AddWithValue("@AranacakKelime", "");
                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                    if (dt.Rows.Count > 0)
                    {
                        #region Cookie İşlemleri
                        foreach (DataRow satir in dt.Rows)
                        {
                            dynamic entity = new System.Dynamic.ExpandoObject();
                            entity.ID = Convert.ToString(satir["ID"]);
                            entity.UyelikID = Convert.ToString(satir["UyelikID"]);
                            entity.KullaniciAdi = Convert.ToString(satir["KullaniciAdi"]);
                            entity.Parola = Convert.ToString(satir["Parola"]);
                            entity.Ad = Convert.ToString(satir["Ad"]);
                            entity.Soyad = Convert.ToString(satir["Soyad"]);
                            entity.Telefon = Convert.ToString(satir["Telefon"]);
                            entity.Adres = Convert.ToString(satir["Adres"]);
                            entity.Il = Convert.ToString(satir["Il"]);
                            entity.Ilce = Convert.ToString(satir["Ilce"]);
                            entity.Aktif = Convert.ToString(satir["Aktif"]);
                            entity.Aciklama1 = Convert.ToString(satir["Aciklama1"]);
                            entity.Aciklama2 = Convert.ToString(satir["Aciklama2"]);
                            entity.Aciklama3 = Convert.ToString(satir["Aciklama3"]);
                            entity.Tarih = Convert.ToString(satir["Tarih"]);
                            entity.Resim = Convert.ToString(satir["Resim"]);
                            entity.KayitTarihi = Convert.ToString(satir["KayitTarihi"]);
                            entity.KayitYapanKullanici = Convert.ToString(satir["KayitYapanKullanici"]);
                            entity.DuzenlemeTarihi = Convert.ToString(satir["DuzenlemeTarihi"]);
                            entity.DuzenlemeYapanKullanici = Convert.ToString(satir["DuzenlemeYapanKullanici"]);
                            entity.Silindi = Convert.ToString(satir["Silindi"]);
                            entity.SilinenTarih = Convert.ToString(satir["SilinenTarih"]);
                            entity.SilenKullanici = Convert.ToString(satir["SilenKullanici"]);
                            entity.Onay = Convert.ToString(satir["Onay"]);
                            entities.Add(entity);
                        }
                        #endregion
                        result.Data = entities;
                        result.SonucKodu = 1;
                        result.Sonuc = "Başarılı";
                        return result;
                    }
                    else
                    {
                        result.SonucKodu = 0;
                        result.Hata = "UYARI! Kayıt bulunamadı!";
                        return result;
                    }
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanıcı adı veya parola yanlış!";
                    return result;
                }
            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public IDJsonResult UyelikListesi([FromBody] JObject data)
        {
            IDJsonResult result = new IDJsonResult();
            try
            {

                if (data["KullaniciAdi"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                if (data["Parola"] == null)
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! KullaniciAdi bilgisi boş olamaz.";
                    return result;
                }
                string KullaniciAdi = Convert.ToString(data["KullaniciAdi"]);
                string Parola = Convert.ToString(data["Parola"]);

                if (KullaniciAdi == "info@ykyazilim.com.tr" && Parola == "4jtj2jsmv")
                {

                    List<dynamic> entities = new List<dynamic>();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "p_UyelikListesi";
                    cmd.Parameters.AddWithValue("@AranacakKelime", "");
                    DataTable dt = (DataTable)IDVeritabani.Sorgula(cmd, SorgulaTuru.Tablo);

                    if (dt.Rows.Count > 0)
                    {
                        #region Cookie İşlemleri
                        foreach (DataRow satir in dt.Rows)
                        {
                            dynamic entity = new System.Dynamic.ExpandoObject();
                            entity.ID = Convert.ToString(satir["ID"]);
                            entity.Isim = Convert.ToString(satir["Isim"]);
                            entity.Unvan = Convert.ToString(satir["Unvan"]);
                            entity.VergiNumarasi = Convert.ToString(satir["VergiNumarasi"]);
                            entity.VergiDairesi = Convert.ToString(satir["VergiDairesi"]);
                            entity.Adres = Convert.ToString(satir["Adres"]);
                            entity.Iletisim = Convert.ToString(satir["Iletisim"]);
                            entity.Email = Convert.ToString(satir["Email"]);
                            entity.UyelikBaslangicTarihi = Convert.ToString(satir["UyelikBaslangicTarihi"]);
                            entity.UyelikBitisTarihi = Convert.ToString(satir["UyelikBitisTarihi"]);
                            entity.KayitTarihi = Convert.ToString(satir["KayitTarihi"]);
                            entity.KayitYapanKullanici = Convert.ToString(satir["KayitYapanKullanici"]);
                            entity.DuzenlemeTarihi = Convert.ToString(satir["DuzenlemeTarihi"]);
                            entity.DuzenlemeYapanKullanici = Convert.ToString(satir["DuzenlemeYapanKullanici"]);
                            entity.Silindi = Convert.ToString(satir["Silindi"]);
                            entity.SilinenTarih = Convert.ToString(satir["SilinenTarih"]);
                            entity.SilenKullanici = Convert.ToString(satir["SilenKullanici"]);
                            entity.ApiUrl = Convert.ToString(satir["ApiUrl"]);
                            entities.Add(entity);
                        }
                        #endregion
                        result.Data = entities;
                        result.SonucKodu = 1;
                        result.Sonuc = "Başarılı";
                        return result;
                    }
                    else
                    {
                        result.SonucKodu = 0;
                        result.Hata = "UYARI! Kayıt bulunamadı!";
                        return result;
                    }
                }
                else
                {
                    result.SonucKodu = 0;
                    result.Hata = "UYARI! Kullanıcı adı veya parola yanlış!";
                    return result;
                }

            }
            catch (Exception err)
            {
                result.SonucKodu = -1;
                result.Sonuc = "HATA!";
                result.Hata = err.Message;
            }
            finally
            {

            }
            return result;
        }

    }
    public class SupplierOrders
    {
        public string OrderNumber { get; set; }
        public string SupplierOrderNumber { get; set; }
        public string OrderStatus { get; set; }
        public string StatusDescription { get; set; }
        public string TotalAmount { get; set; }
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string Description { get; set; }
        public int TotalCount { get; set; }
        public List<SupplierOrder> SupplierOrder { get; set; } //İmece için eklendi
        public List<SupplierOrderProductsList> SupplierOrderProductsList { get; set; }

    }

    public class SupplierOrderProductsList
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductBarcode { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Unit { get; set; }
        public string TotalAmount { get; set; }
        public string Description { get; set; }
    }

    #region Pirelli Class
    public class PirelliHeader
    {
        public string TrackingId { get; set; }
        public string BuyerCode { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string RequestedDatetime { get; set; }
        public PirelliHeaderCustomer Customer { get; set; }
        public string SalesOrderNumber { get; set; }

    }
    public class PirelliHeaderCustomer
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public PirelliHeaderCustomerAdres Address { get; set; }
    }
    public class PirelliHeaderCustomerAdres
    {
        public List<string> Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }

    public class PirelliNotes
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
    public class PirelliItems
    {
        public string LineId { get; set; }
        public string ProductCode { get; set; }
        public string RequestedDeliveryDatetime { get; set; }
        public int RequestedQuantity { get; set; }
        public string Price { get; set; }
        public string ConfirmedDeliveryDatetime { get; set; }
        public int ConfirmedQuantity { get; set; }
    }
    public class PirelliSiparisDto
    {
        public PirelliHeader Header { get; set; }
        public List<PirelliItems> Items { get; set; }
        public List<PirelliNotes> Notes { get; set; }
    }

    public class PirelliSiparisResponseDto
    {
        public PirelliHeader Header { get; set; }
        public List<PirelliItems> Items { get; set; }
        public List<PirelliNotes> Notes { get; set; }
    }

    public class PirelliDto
    {
        public string ProductCode { get; set; }
    }
    public class PirelliResponseDto
    {
        public string Manufacturer { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int Quantity { get; set; }
        public string DeliveryDatetime { get; set; }
        public string ProductionDate { get; set; }
    }

    #endregion

    #region İmace Plastik Class


    public class SupplierOrder
    {
        public string OrderNumber { get; set; }
        public string SupplierOrderNumber { get; set; }
        public int OrderStatus { get; set; }
        public string StatusDescription { get; set; }
        public float TotalAmount { get; set; }
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string Description { get; set; }
        public string Bilgi { get; set; }

        public List<SupplierOrderProducts> SupplierOrderProductsList { get; set; }

    }

    public class SupplierOrderProducts
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductBarcode { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public float TotalAmount { get; set; }
        public string[] Description { get; set; }

    }

    #endregion 
    public class KisayolModel
    {
        public string Ikon { get; set; }
        public string KullaniciId { get; set; }
        public string Baslik { get; set; } = string.Empty;
    }
    public class KisayolRequest
    {
        public string KullaniciId { get; set; } = string.Empty;
        public List<KisayolModel> Kisayollar { get; set; } = new List<KisayolModel>();
    }
    public class IDJsonResult
    {
        public object Data { get; set; }
        public int SonucKodu { get; set; }
        public string Sonuc { get; set; }
        public string Hata { get; set; }
        public int Sonuc_Versiyon { get; set; }
    }
    public class IslemBilgisi
    {
        public int SonDurumu { get; set; }
        public string EkBilgi1 { get; set; }
        public string EkBilgi2 { get; set; }
        public string EkBilgi3 { get; set; }
        public string EvrakNumarasi { get; set; }
    }
    public class IrsaliyeUst
    {
        public string BelgeNo { get; set; }
        public string Tarih { get; set; }
        public string CariKodu { get; set; }
    }
    public class IrsaliyeKalemler
    {
        public string StokKodu { get; set; }
        public string Fiyat { get; set; }
        public string Miktar { get; set; }
        public string Tutar { get; set; }
    }
}