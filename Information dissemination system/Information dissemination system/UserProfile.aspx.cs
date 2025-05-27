using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace Information_dissemination_system
{
    public partial class UserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserInfo();
            }
        }

        private void LoadUserInfo()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT Username, FullName, Gender, BirthDate, Email, Phone, Avatar FROM Subscribers WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtUsername.Text = reader["Username"].ToString();
                            txtFullName.Text = reader["FullName"].ToString();
                            ddlGender.SelectedValue = reader["Gender"].ToString();
                            txtBirthDate.Text = reader["BirthDate"] != DBNull.Value ? Convert.ToDateTime(reader["BirthDate"]).ToString("yyyy-MM-dd") : "";
                            txtEmail.Text = reader["Email"].ToString();
                            txtPhone.Text = reader["Phone"].ToString();

                            string avatarPath = reader["Avatar"].ToString();
                            if (!string.IsNullOrEmpty(avatarPath))
                            {
                                imgCurrentAvatar.ImageUrl = avatarPath;
                            }
                            else
                            {
                                imgCurrentAvatar.ImageUrl = "~/Images/default-avatar.png"; // 默认头像
                            }
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

            string newAvatarPath = null;

            // 头像上传处理
            if (fuAvatar.HasFile)
            {
                string extension = Path.GetExtension(fuAvatar.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                if (Array.IndexOf(allowedExtensions, extension) < 0)
                {
                    lblMessage.Text = "仅支持jpg, jpeg, png, gif格式的图片";
                    return;
                }

                string fileName = Guid.NewGuid().ToString() + extension;
                string savePath = Server.MapPath("~/Uploads/Avatars/");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                string fullPath = Path.Combine(savePath, fileName);
                try
                {
                    fuAvatar.SaveAs(fullPath);
                    newAvatarPath = "~/Uploads/Avatars/" + fileName;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "上传头像失败：" + ex.Message;
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"UPDATE Subscribers SET 
                                FullName = @FullName,
                                Gender = @Gender,
                                BirthDate = @BirthDate,
                                Email = @Email,
                                Phone = @Phone";

                if (newAvatarPath != null)
                {
                    sql += ", Avatar = @Avatar";
                }

                sql += " WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@BirthDate", string.IsNullOrEmpty(txtBirthDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtBirthDate.Text));
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    if (newAvatarPath != null)
                    {
                        cmd.Parameters.AddWithValue("@Avatar", newAvatarPath);
                    }
                    cmd.Parameters.AddWithValue("@Id", userId);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            lblMessage.Text = "修改成功，正在刷新页面...";
                            Response.AddHeader("REFRESH", "1;URL=UserProfile.aspx");
                        }
                        else
                        {
                            lblMessage.Text = "修改失败，请稍后重试";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "发生错误：" + ex.Message;
                    }
                }
            }
        }
    }
}
