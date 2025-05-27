using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Information_dissemination_system
{
    public partial class SearchResults : System.Web.UI.Page
    {
        public string SearchQuery { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 9; // 每页9篇文章
        public int TotalPages { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SearchQuery = Request.QueryString["q"] ?? "";
                if (string.IsNullOrWhiteSpace(SearchQuery))
                {
                    // 无搜索词时可以跳转首页或显示空内容
                    rptSearchResults.DataSource = null;
                    rptSearchResults.DataBind();
                    return;
                }

                // 读取当前页码，默认第一页
                if (!int.TryParse(Request.QueryString["page"], out int page))
                    page = 1;
                CurrentPage = page < 1 ? 1 : page;

                BindSearchResults();
            }
        }

        private void BindSearchResults()
        {
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 先获取总记录数
                string countSql = @"
                    SELECT COUNT(*)
                    FROM Posts p
                    INNER JOIN Categories c ON p.CategoryId = c.Id
                    WHERE p.IsPublished = 1 AND (p.Title LIKE @q OR p.Content LIKE @q)";

                using (SqlCommand countCmd = new SqlCommand(countSql, conn))
                {
                    countCmd.Parameters.AddWithValue("@q", "%" + SearchQuery + "%");
                    int totalRecords = (int)countCmd.ExecuteScalar();
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

                    // 防止当前页码超过总页数
                    if (CurrentPage > TotalPages) CurrentPage = TotalPages == 0 ? 1 : TotalPages;
                }

                // 取分页数据
                string sql = @"
                    SELECT p.Id, p.Title, p.Content, p.CoverImage, p.Views, c.Name AS CategoryName
                    FROM Posts p
                    INNER JOIN Categories c ON p.CategoryId = c.Id
                    WHERE p.IsPublished = 1 AND (p.Title LIKE @q OR p.Content LIKE @q)
                    ORDER BY p.CreatedAt DESC
                    OFFSET @offset ROWS FETCH NEXT @pagesize ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@q", "%" + SearchQuery + "%");
                    cmd.Parameters.AddWithValue("@offset", (CurrentPage - 1) * PageSize);
                    cmd.Parameters.AddWithValue("@pagesize", PageSize);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptSearchResults.DataSource = dt;
                    rptSearchResults.DataBind();
                }
            }
        }
    }
}
