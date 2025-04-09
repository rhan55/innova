using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDDizayn
{
    public class DizaynIslemleri
    {
        
        public static string DizaynKaydet(DataSet ds, string KaydedilecekKlasor)
        {

            XtraReport rapor = new XtraReport();

            {
                rapor.LoadLayout(KaydedilecekKlasor + "\\Barkod.repx");
                rapor.DataSource = ds;
                
                string dosyaadi = DateTime.Now.ToString("yyyy-MM-dd ssmm") + " - " + Guid.NewGuid().ToString() + ".png";
                string dosya = KaydedilecekKlasor + dosyaadi;
                if (false)
                {
                    rapor.ExportToImage(dosya);
                    return dosyaadi;
                }
                else
                {
                    rapor.ShowDesigner();
                    return "";
                }
            }
        }
    }
}
