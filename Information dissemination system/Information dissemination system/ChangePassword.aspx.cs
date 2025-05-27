using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Information_dissemination_system
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (newPassword != confirmPassword)
            {
                lblMessage.Text = "新密码与确认密码不一致。";
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 1. 验证旧密码是否正确（明文比较）
                string sql = "SELECT Password FROM Subscribers WHERE Id = @Id";
                string storedPassword = null;
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            storedPassword = reader["Password"] as string;
                        }
                        else
                        {
                            lblMessage.Text = "用户不存在。";
                            return;
                        }
                    }
                }

                if (storedPassword == null || storedPassword != oldPassword)
                {
                    lblMessage.Text = "旧密码错误。";
                    return;
                }

                // 2. 更新密码（明文存储）
                string updateSql = "UPDATE Subscribers SET Password = @NewPassword WHERE Id = @Id";
                using (SqlCommand updateCmd = new SqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@Id", userId);

                    int rows = updateCmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "密码修改成功！正在跳转...";
                        Response.AddHeader("REFRESH", "2;URL=UserProfile.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "密码更新失败。";
                    }
                }
            }
        }
    }
}
