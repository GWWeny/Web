using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace Information_dissemination_system
{
    public partial class AdminComments : System.Web.UI.Page
    {
        private int PendingPageSize = 5;   // 未审核分页大小
        private int ApprovedPageSize = 5;  // 已审核分页大小

        private int PendingCurrentPage
        {
            get => ViewState["PendingCurrentPage"] == null ? 1 : (int)ViewState["PendingCurrentPage"];
            set => ViewState["PendingCurrentPage"] = value;
        }
        private int ApprovedCurrentPage
        {
            get => ViewState["ApprovedCurrentPage"] == null ? 1 : (int)ViewState["ApprovedCurrentPage"];
            set => ViewState["ApprovedCurrentPage"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPendingComments();
                BindApprovedComments();
            }
        }

        private string ConnStr => ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

        #region 绑定数据

        private void BindPendingComments()
        {
            string search = txtSearch.Text.Trim();
            int totalCount = 0;

            DataTable dt = GetComments(false, PendingCurrentPage, PendingPageSize, search, out totalCount);
            rptPendingComments.DataSource = dt;
            rptPendingComments.DataBind();

            BindPager(totalCount, PendingPageSize, PendingCurrentPage, rptPendingPager);

            lnkPendingPrev.Enabled = PendingCurrentPage > 1;
            lnkPendingNext.Enabled = PendingCurrentPage * PendingPageSize < totalCount;
        }

        private void BindApprovedComments()
        {
            string search = txtSearch.Text.Trim();
            int totalCount = 0;

            DataTable dt = GetComments(true, ApprovedCurrentPage, ApprovedPageSize, search, out totalCount);
            rptApprovedComments.DataSource = dt;
            rptApprovedComments.DataBind();

            BindPager(totalCount, ApprovedPageSize, ApprovedCurrentPage, rptApprovedPager);

            lnkApprovedPrev.Enabled = ApprovedCurrentPage > 1;
            lnkApprovedNext.Enabled = ApprovedCurrentPage * ApprovedPageSize < totalCount;
        }

        /// <summary>
        /// 获取评论数据（带联表查询用户名）
        /// </summary>
        /// <param name="isApproved"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private DataTable GetComments(bool isApproved, int page, int pageSize, string search, out int totalCount)
        {
            DataTable dt = new DataTable();
            totalCount = 0;

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                // 先统计总数，需联表
                string countSql = @"
                    SELECT COUNT(*)
                    FROM Comments c
                    INNER JOIN Subscribers s ON c.SubscriberId = s.Id
                    WHERE c.IsApproved = @IsApproved AND c.IsDeleted = 0
                ";
                if (!string.IsNullOrEmpty(search))
                {
                    countSql += @" AND (s.Username LIKE @search OR c.Content LIKE @search OR CONVERT(varchar(10), c.CreatedAt, 120) = @searchExact)";
                }

                using (SqlCommand cmdCount = new SqlCommand(countSql, conn))
                {
                    cmdCount.Parameters.AddWithValue("@IsApproved", isApproved);
                    if (!string.IsNullOrEmpty(search))
                    {
                        cmdCount.Parameters.AddWithValue("@search", "%" + search + "%");
                        cmdCount.Parameters.AddWithValue("@searchExact", search);
                    }
                    totalCount = (int)cmdCount.ExecuteScalar();
                }

                if (totalCount == 0)
                    return dt;

                // 分页查询，联表取用户名
                string querySql = @"
                    SELECT c.Id, s.Username, c.Content, c.CreatedAt
                    FROM Comments c
                    INNER JOIN Subscribers s ON c.SubscriberId = s.Id
                    WHERE c.IsApproved = @IsApproved AND c.IsDeleted = 0
                ";
                if (!string.IsNullOrEmpty(search))
                {
                    querySql += @" AND (s.Username LIKE @search OR c.Content LIKE @search OR CONVERT(varchar(10), c.CreatedAt, 120) = @searchExact)";
                }
                querySql += " ORDER BY c.CreatedAt DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(querySql, conn))
                {
                    cmd.Parameters.AddWithValue("@IsApproved", isApproved);
                    if (!string.IsNullOrEmpty(search))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                        cmd.Parameters.AddWithValue("@searchExact", search);
                    }
                    cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        private void BindPager(int totalCount, int pageSize, int currentPage, Repeater rptPager)
        {
            int pageCount = (totalCount + pageSize - 1) / pageSize;
            int[] pages = new int[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pages[i] = i + 1;
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        #endregion

        #region 事件处理

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PendingCurrentPage = 1;
            ApprovedCurrentPage = 1;
            BindPendingComments();
            BindApprovedComments();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            PendingCurrentPage = 1;
            ApprovedCurrentPage = 1;
            BindPendingComments();
            BindApprovedComments();
        }

        protected void rptPendingComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Approve")
            {
                ApproveComment(id);
            }
            else if (e.CommandName == "Delete")
            {
                DeleteComment(id);
            }
            BindPendingComments();
            BindApprovedComments();
        }

        protected void rptApprovedComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Delete")
            {
                DeleteComment(id);
            }
            BindPendingComments();
            BindApprovedComments();
        }

        protected void rptPendingPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                PendingCurrentPage = Convert.ToInt32(e.CommandArgument);
                BindPendingComments();
            }
        }

        protected void rptApprovedPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                ApprovedCurrentPage = Convert.ToInt32(e.CommandArgument);
                BindApprovedComments();
            }
        }

        protected void lnkPendingPrev_Click(object sender, EventArgs e)
        {
            if (PendingCurrentPage > 1)
            {
                PendingCurrentPage--;
                BindPendingComments();
            }
        }

        protected void lnkPendingNext_Click(object sender, EventArgs e)
        {
            PendingCurrentPage++;
            BindPendingComments();
        }

        protected void lnkApprovedPrev_Click(object sender, EventArgs e)
        {
            if (ApprovedCurrentPage > 1)
            {
                ApprovedCurrentPage--;
                BindApprovedComments();
            }
        }

        protected void lnkApprovedNext_Click(object sender, EventArgs e)
        {
            ApprovedCurrentPage++;
            BindApprovedComments();
        }

        #endregion

        #region 数据操作

        private void ApproveComment(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                string sql = "UPDATE Comments SET IsApproved = 1 WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteComment(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                string sql = "DELETE FROM Comments WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
