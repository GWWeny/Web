using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Information_dissemination_system
{
    public partial class CategoryPosts : System.Web.UI.Page
    {
        private int pageSize = 7; // 每页显示数量
        private int categoryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["categoryId"], out categoryId))
            {
                lblEmpty.Text = "栏目 ID 参数无效。";
                lblEmpty.Visible = true;
                rptPosts.Visible = false;
                return;
            }

            if (!IsPostBack)
            {
                ViewState["PageIndex"] = 0;
                LoadPosts();
            }
        }

        private void LoadPosts()
        {
            int pageIndex = (int)ViewState["PageIndex"];
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

            string sql = @"
                SELECT COUNT(*) FROM Posts WHERE CategoryId = @CategoryId AND IsPublished = 1;
                SELECT Id, Title, Content, CoverImage, Views, CreatedAt
                FROM (
                    SELECT ROW_NUMBER() OVER (ORDER BY IsTop DESC, CreatedAt DESC) AS RowNum,
                        Id, Title, Content, CoverImage, Views, CreatedAt
                    FROM Posts
                    WHERE CategoryId = @CategoryId AND IsPublished = 1
                ) AS Paged
                WHERE RowNum > @Offset AND RowNum <= @Offset + @PageSize
                ORDER BY RowNum;";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                int offset = pageIndex * pageSize;
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Offset", offset);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // 总记录数
                    reader.Read();
                    int totalRecords = reader.GetInt32(0);
                    int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                    // 跳到下一结果集
                    reader.NextResult();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    if (dt.Rows.Count > 0)
                    {
                        rptPosts.DataSource = dt;
                        rptPosts.DataBind();
                        rptPosts.Visible = true;
                        lblEmpty.Visible = false;

                        // 分页控件状态
                        lblPageInfo.Text = $"第 {pageIndex + 1} 页，共 {totalPages} 页";
                        btnPrev.Enabled = pageIndex > 0;
                        btnNext.Enabled = pageIndex < totalPages - 1;
                    }
                    else
                    {
                        rptPosts.Visible = false;
                        lblEmpty.Text = "该栏目暂无发布的文章。";
                        lblEmpty.Visible = true;
                        lblPageInfo.Text = "";
                        btnPrev.Enabled = false;
                        btnNext.Enabled = false;
                    }
                }
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            int pageIndex = (int)ViewState["PageIndex"];
            if (pageIndex > 0)
            {
                ViewState["PageIndex"] = pageIndex - 1;
                LoadPosts();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ViewState["PageIndex"] = (int)ViewState["PageIndex"] + 1;
            LoadPosts();
        }
    }
}
