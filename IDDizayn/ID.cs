using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDDizayn
{
    public class ID
    {


        public static object Sorgula(SqlCommand cmd, SorgulaTuru tur, string BaglantiCumlesi = "")
        {
            if (BaglantiCumlesi == "")
            {
                BaglantiCumlesi = ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString;
            }
            using (SqlConnection BaglantiIDYAZILIM = new SqlConnection(BaglantiCumlesi))
            {

                try
                {

                    if (BaglantiIDYAZILIM.State == System.Data.ConnectionState.Closed)
                        BaglantiIDYAZILIM.Open();
                    cmd.Connection = BaglantiIDYAZILIM;
                    cmd.CommandTimeout = 360;
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
                    if (BaglantiIDYAZILIM.State == System.Data.ConnectionState.Open)
                        BaglantiIDYAZILIM.Close();
                }

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
