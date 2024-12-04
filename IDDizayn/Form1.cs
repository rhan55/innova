using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDDizayn
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "p_StokFiyatGor";
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@Barkod", "12");
            DataSet ds = (DataSet)ID.Sorgula(cmd2, SorgulaTuru.DataSet);
          
                string url = IDDizayn.DizaynIslemleri.DizaynKaydet(ds, Application.StartupPath);
            MessageBox.Show(url);
            //DataSet ds = new DataSet();
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.CommandText = "p_TeklifYazir";
            //cmd.Parameters.AddWithValue("@ID", 10);
            //ds = (DataSet)ID.Sorgula(cmd, SorgulaTuru.DataSet, "");
            //IDDizayn.DizaynIslemleri.DizaynKaydet(Guid.NewGuid().ToString(), ds, "TeklifFormu", "D:\\ID\\YK\\Projects\\YKWebPortal\\YKWebPortal\\Pdf\\");
        }
    }
}
