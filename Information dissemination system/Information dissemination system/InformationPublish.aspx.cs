using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Information_dissemination_system
{
    public partial class InformationPublish : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT Id, Name FROM Categories ORDER BY CreatedAt DESC";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlCategory.DataSource = reader;
                    ddlCategory.DataTextField = "Name";
                    ddlCategory.DataValueField = "Id";
                    ddlCategory.DataBind();
                }
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string content = txtContent.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                lblMessage.CssClass = "text-danger mb-3 d-block";
                lblMessage.Text = "标题不能为空。";
                return;
            }

            if (string.IsNullOrEmpty(content))
            {
                lblMessage.CssClass = "text-danger mb-3 d-block";
                lblMessage.Text = "内容不能为空。";
                return;
            }

            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            bool isTop = chkIsTop.Checked;
            bool isPublished = chkIsPublished.Checked;

            // 取当前登录用户ID，确保 Session["UserId"] 不为空
            if (Session["UserId"] == null)
            {
                lblMessage.CssClass = "text-danger mb-3 d-block";
                lblMessage.Text = "请先登录。";
                return;
            }
            int authorId = Convert.ToInt32(Session["UserId"]);

            string coverImagePath = "";

            if (fuCoverImage.HasFile)
            {
                string extension = Path.GetExtension(fuCoverImage.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(extension))
                {
                    lblMessage.CssClass = "text-danger mb-3 d-block";
                    lblMessage.Text = "只允许上传 jpg、jpeg、png、gif 格式的图片。";
                    return;
                }

                try
                {
                    string fileName = Guid.NewGuid().ToString() + extension;
                    string savePath = Server.MapPath("~/uploads/" + fileName);

                    // 确保目录存在
                    string uploadDir = Server.MapPath("~/uploads/");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    fuCoverImage.SaveAs(savePath);
                    coverImagePath = "uploads/" + fileName;
                }
                catch (Exception ex)
                {
                    lblMessage.CssClass = "text-danger mb-3 d-block";
                    lblMessage.Text = "图片上传失败：" + ex.Message;
                    return;
                }
            }

            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"INSERT INTO Posts (Title, Content, CoverImage, CategoryId, AuthorId, IsTop, IsPublished)
                               VALUES (@Title, @Content, @CoverImage, @CategoryId, @AuthorId, @IsTop, @IsPublished)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Content", content);
                    cmd.Parameters.AddWithValue("@CoverImage", string.IsNullOrEmpty(coverImagePath) ? (object)DBNull.Value : coverImagePath);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@AuthorId", authorId);
                    cmd.Parameters.AddWithValue("@IsTop", isTop);
                    cmd.Parameters.AddWithValue("@IsPublished", isPublished);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        lblMessage.CssClass = "text-success mb-3 d-block";
                        lblMessage.Text = "发布成功！";
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.CssClass = "text-danger mb-3 d-block";
                        lblMessage.Text = "发布失败：" + ex.Message;
                    }
                }
            }
        }

        private void ClearForm()
        {
            txtTitle.Text = "";
            txtContent.Text = "";
            ddlCategory.SelectedIndex = 0;
            chkIsTop.Checked = false;
            chkIsPublished.Checked = true;

            imgPreview.ImageUrl = "";
            imgPreview.Style["display"] = "none"; // 隐藏预览图
        }
    }
}
