using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace YKPortal.Models
{
    public class IDVeritabani
    {
        public static object Sorgula(SqlCommand cmd, SorgulaTuru tur)
        {
            SqlConnection Baglanti = new SqlConnection(ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString);
            try
            {
                if (Baglanti.State == System.Data.ConnectionState.Closed)
                    Baglanti.Open();
                cmd.Connection = Baglanti;
                cmd.CommandTimeout = 100000;
                switch (tur)
                {
                    case SorgulaTuru.Bos:
                        return cmd.ExecuteNonQuery();
                        break;
                    case SorgulaTuru.Tek:
                        return cmd.ExecuteScalar();
                        break;
                    case SorgulaTuru.Tablo:
                        DataTable dt = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter();  
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dt);
                        return dt;
                        break;
                    case SorgulaTuru.DataSet:
                        DataSet ds = new DataSet();
                        SqlDataAdapter adapter2 = new SqlDataAdapter();
                        adapter2.SelectCommand = cmd;
                        adapter2.Fill(ds);
                        return ds;
                        break;
                }
            }
            finally
            {
                if (Baglanti.State == System.Data.ConnectionState.Open)
                    Baglanti.Close();
            }
            return "";
        }



    }

    public enum SorgulaTuru
    {
        Bos,
        Tek,
        Tablo,
        DataSet
    }

}