using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Create_Greenlight_835
{
    public static class DataAccess
    {
        public static List<string> GetFileNames(string cn)
        {
            List<string> result = new List<string>();
            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlDataAdapter da = new SqlDataAdapter("select distinct FileName from GreenlightHeader", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        result.Add(dt.Rows[i][0].ToString());
                    }
                }

            }

            return result;
        }
        public static List<string> GetDcns(string cn)
        {
            List<String> result = new List<string>();
            using (SqlConnection conn = new SqlConnection(cn))
            {

            }
            return result;
        }
        public static void GetGreenlight835Date(string cn, string DCN, ref Header835 header835, ref List<Line835> line835s)
        {

        }
    }
}
