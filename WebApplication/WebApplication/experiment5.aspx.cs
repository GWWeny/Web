using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class experiment5 : System.Web.UI.Page
    {
        [WebMethod]
        public static string GetProductInfo(string productName)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            string query = "SELECT * FROM Products WHERE Name = @Name";
            string result = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", productName);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result = reader["Price"].ToString();
                    }
                }
                else
                {
                    result = "未找到匹配的产品";
                }
                reader.Close();
            }

            return result;
        }

    }
}