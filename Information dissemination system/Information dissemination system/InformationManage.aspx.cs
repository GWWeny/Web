using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace Information_dissemination_system
{
    public partial class InformationManage : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
        private string SearchKeyword
        {
            get { return ViewState["SearchKeyword"] as string ?? ""; }
            set { ViewState["SearchKeyword"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPosts();
            }
        }

        private void BindPosts()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
            SELECT p.Id, p.Title, p.Content, p.CoverImage, p.IsTop, p.IsPublished, p.CreatedAt, c.Name AS CategoryName,
                CASE WHEN LEN(p.Content) > 50 THEN LEFT(p.Content, 50) + '...' ELSE p.Content END AS ShortContent
            FROM Posts p
            INNER JOIN Categories c ON p.CategoryId = c.Id
            WHERE (@keyword = '' OR p.Title LIKE '%' + @keyword + '%' OR c.Name LIKE '%' + @keyword + '%')
            ORDER BY p.CreatedAt DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@keyword", SearchKeyword);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPosts.DataSource = dt;
                gvPosts.DataBind();
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchKeyword = txtSearch.Text.Trim();
            gvPosts.PageIndex = 0; // 搜索后回到首页
            BindPosts();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            SearchKeyword = "";
            txtSearch.Text = "";
            gvPosts.PageIndex = 0;
            BindPosts();
        }

        protected void gvPosts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPosts.PageIndex = e.NewPageIndex;
            BindPosts();
        }

        protected void gvPosts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPosts.EditIndex = e.NewEditIndex;
            BindPosts();
        }

        protected void gvPosts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPosts.EditIndex = -1;
            lblMessage.Text = "";
            BindPosts();
        }

        protected void gvPosts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvPosts.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlCategory = (DropDownList)e.Row.FindControl("ddlCategory");
                if (ddlCategory != null)
                {
                    BindCategories(ddlCategory);

                    DataRowView drv = e.Row.DataItem as DataRowView;
                    if (drv != null)
                    {
                        string categoryName = drv["CategoryName"].ToString();
                        ListItem selectedItem = ddlCategory.Items.FindByText(categoryName);
                        if (selectedItem != null)
                        {
                            ddlCategory.ClearSelection();
                            selectedItem.Selected = true;
                        }
                    }
                }
            }
        }

        private void BindCategories(DropDownList ddl)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT Id, Name FROM Categories ORDER BY Name";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddl.Items.Clear();
                while (reader.Read())
                {
                    ddl.Items.Add(new ListItem(reader["Name"].ToString(), reader["Id"].ToString()));
                }
            }
        }

        protected void gvPosts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvPosts.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvPosts.Rows[e.RowIndex];

            string newTitle = ((TextBox)row.FindControl("txtTitle")).Text.Trim();
            string newContent = ((TextBox)row.FindControl("txtContent")).Text.Trim();
            CheckBox chkIsTop = (CheckBox)row.FindControl("chkIsTopEdit");
            CheckBox chkIsPublished = (CheckBox)row.FindControl("chkIsPublishedEdit");
            DropDownList ddlCategory = (DropDownList)row.FindControl("ddlCategory");
            FileUpload fuCover = (FileUpload)row.FindControl("fuCoverImage");
            HiddenField hfCoverImage = (HiddenField)row.FindControl("hfCoverImage");

            string coverImageFileName = hfCoverImage.Value; // 默认使用旧图片名

            if (fuCover.HasFile)
            {
                string ext = Path.GetExtension(fuCover.FileName).ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif")
                {
                    lblMessage.CssClass = "text-danger";
                    lblMessage.Text = "封面图片格式仅支持 jpg, jpeg, png, gif！";
                    return;
                }

                string uploadsFolder = Server.MapPath("~/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // 上传成功后保存的文件名
                coverImageFileName = "uploads/" + Guid.NewGuid().ToString() + ext;

                // 保存文件路径
                string savePath = Server.MapPath("~/" + coverImageFileName);

                try
                {
                    fuCover.SaveAs(savePath);
                }
                catch (Exception ex)
                {
                    lblMessage.CssClass = "text-danger";
                    lblMessage.Text = "上传封面图片失败：" + ex.Message;
                    return;
                }
            }

            int categoryId = int.Parse(ddlCategory.SelectedValue);
            bool isTop = chkIsTop.Checked;
            bool isPublished = chkIsPublished.Checked;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"UPDATE Posts 
                               SET Title=@Title, Content=@Content, CoverImage=@CoverImage, CategoryId=@CategoryId, 
                                   IsTop=@IsTop, IsPublished=@IsPublished 
                               WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Title", newTitle);
                cmd.Parameters.AddWithValue("@Content", newContent);
                cmd.Parameters.AddWithValue("@CoverImage", string.IsNullOrEmpty(coverImageFileName) ? (object)DBNull.Value : coverImageFileName);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@IsTop", isTop);
                cmd.Parameters.AddWithValue("@IsPublished", isPublished);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    cmd.ExecuteNonQuery();
                    lblMessage.CssClass = "text-success";
                    lblMessage.Text = "更新成功！";
                    gvPosts.EditIndex = -1;
                    BindPosts();
                }
                catch (Exception ex)
                {
                    lblMessage.CssClass = "text-danger";
                    lblMessage.Text = "更新失败：" + ex.Message;
                }
            }
        }

        protected void gvPosts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvPosts.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 先获取封面图片名称，用于删除文件
                string sqlGetImage = "SELECT CoverImage FROM Posts WHERE Id=@Id";
                SqlCommand cmdGet = new SqlCommand(sqlGetImage, conn);
                cmdGet.Parameters.AddWithValue("@Id", id);
                string coverImage = Convert.ToString(cmdGet.ExecuteScalar());

                string sqlDelete = "DELETE FROM Posts WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sqlDelete, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    cmd.ExecuteNonQuery();

                    // 删除图片文件（如果存在且不是空）
                    if (!string.IsNullOrEmpty(coverImage))
                    {
                        string filePath = Server.MapPath("~/uploads/" + coverImage);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }

                    lblMessage.CssClass = "text-success";
                    lblMessage.Text = "删除成功！";
                    BindPosts();
                }
                catch (Exception ex)
                {
                    lblMessage.CssClass = "text-danger";
                    lblMessage.Text = "删除失败：" + ex.Message;
                }
            }
        }
    }
}
