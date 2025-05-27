using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Information_dissemination_system
{
    public partial class Register : System.Web.UI.Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "用户名和密码不能为空";
                return;
            }
            if (password != confirmPassword)
            {
                lblMessage.Text = "两次密码输入不一致";
                return;
            }

            // 读取是否为管理员复选框的值
            bool isAdmin = chkIsAdmin.Checked;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString))
                {
                    conn.Open();

                    // 判断用户名是否已存在
                    string checkUserSql = "SELECT COUNT(*) FROM Subscribers WHERE Username = @Username";
                    using (SqlCommand cmdCheck = new SqlCommand(checkUserSql, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@Username", username);
                        int exists = (int)cmdCheck.ExecuteScalar();
                        if (exists > 0)
                        {
                            lblMessage.Text = "用户名已存在，请换一个";
                            return;
                        }
                    }

                    // 插入新用户，IsAdmin 用参数传入
                    string insertSql = @"INSERT INTO Subscribers (Username, Password, Email, IsAdmin, CreatedAt) 
                                         VALUES (@Username, @Password, @Email, @IsAdmin, GETDATE())";
                    using (SqlCommand cmdInsert = new SqlCommand(insertSql, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@Username", username);
                        cmdInsert.Parameters.AddWithValue("@Password", password);  // 生产环境请用哈希
                        cmdInsert.Parameters.AddWithValue("@Email", email);
                        cmdInsert.Parameters.AddWithValue("@IsAdmin", isAdmin ? 1 : 0);

                        int rows = cmdInsert.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            lblMessage.Text = "注册成功，正在跳转到登录页面...";
                            Response.AddHeader("REFRESH", "1;URL=Login.aspx");
                        }
                        else
                        {
                            lblMessage.Text = "注册失败，请重试";
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
