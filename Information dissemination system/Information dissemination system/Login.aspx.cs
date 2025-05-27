using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Information_dissemination_system
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "请输入用户名和密码";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString))
                {
                    conn.Open();

                    // 第一步：先检查用户是否存在
                    string checkUserSql = "SELECT Id, Password, IsAdmin FROM Subscribers WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(checkUserSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader.GetString(1);
                                if (storedPassword == password) // 实际项目应使用加密哈希验证和加盐
                                {
                                    int userId = reader.GetInt32(0);
                                    bool isAdmin = reader.GetBoolean(2);

                                    // 保存 Session 登录信息
                                    Session["UserId"] = userId;
                                    Session["Username"] = username;
                                    Session["IsAdmin"] = isAdmin;

                                    Response.Redirect("Home.aspx");
                                }
                                else
                                {
                                    lblMessage.Text = "密码错误，请重新输入";
                                }
                            }
                            else
                            {
                                lblMessage.Text = "用户不存在，请先注册";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "发生错误: " + ex.Message;
            }
        }
    }
}