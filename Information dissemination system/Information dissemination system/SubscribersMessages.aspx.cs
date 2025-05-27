using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Information_dissemination_system
{
    public partial class SubscribersMessages : System.Web.UI.Page
    {
        private int pageSize = 5; // 每页显示5条
        private int totalPages = 1;

        private int CurrentUserId
        {
            get
            {
                // 从 Session 获取当前用户ID
                return Convert.ToInt32(Session["UserId"]);
            }
        }

        // 使用 ViewState 保存当前页码，避免回发时重置
        private int CurrentPage
        {
            get
            {
                object obj = ViewState["CurrentPage"];
                if (obj == null)
                    return 1;
                else
                    return (int)obj;
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx"); // 重定向到登录页
                return;
            }

            if (!IsPostBack)
            {
                CurrentPage = 1; // 初始化页码
                int userId = CurrentUserId;
                MarkRepliesAsRead(userId);

                BindMyComments(CurrentPage);
                BindRepliesToMe();
                BindMyReplies();
                BindPager();
            }
        }

        #region 数据库连接

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
        }

        #endregion

        #region 绑定 我说过的留言（分页）

        private void BindMyComments(int page)
        {
            CurrentPage = page;

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string countSql = @"
                    SELECT COUNT(*) FROM Comments c
                    WHERE c.SubscriberId = @UserId AND c.IsDeleted = 0 AND c.IsApproved = 1
                ";

                SqlCommand countCmd = new SqlCommand(countSql, conn);
                countCmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                int totalCount = (int)countCmd.ExecuteScalar();

                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                string sql = @"
                    SELECT c.Id, c.Content, c.CreatedAt, p.Title AS PostTitle
                    FROM Comments c
                    INNER JOIN Posts p ON c.PostId = p.Id
                    WHERE c.SubscriberId = @UserId AND c.IsDeleted = 0 AND c.IsApproved = 1
                    ORDER BY c.CreatedAt DESC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
                ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);
                cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptMyComments.DataSource = dt;
                rptMyComments.DataBind();
            }
        }

        #endregion

        #region 绑定 别人回复我的留言（不分页，显示最新5条）

        private void BindRepliesToMe()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string sql = @"
                    SELECT TOP 5 c.Id, c.Content, c.CreatedAt, p.Title AS PostTitle, s.Username AS ReplierName
                    FROM Comments c
                    INNER JOIN Comments parent ON c.ParentId = parent.Id
                    INNER JOIN Posts p ON c.PostId = p.Id
                    INNER JOIN Subscribers s ON c.SubscriberId = s.Id
                    WHERE parent.SubscriberId = @UserId AND c.IsDeleted = 0 AND c.IsApproved = 1
                    ORDER BY c.CreatedAt DESC
                ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptRepliesToMe.DataSource = dt;
                rptRepliesToMe.DataBind();
            }
        }

        #endregion

        #region 分页相关

        private void BindPager()
        {
            List<int> pages = new List<int>();
            for (int i = 1; i <= totalPages; i++)
            {
                pages.Add(i);
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();

            lnkPrev.Enabled = CurrentPage > 1;
            lnkNext.Enabled = CurrentPage < totalPages;
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int pageIndex = Convert.ToInt32(e.CommandArgument);
            if (pageIndex != CurrentPage)
            {
                CurrentPage = pageIndex;
                BindMyComments(CurrentPage);
                BindPager();
            }
        }

        protected void rptPager_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int page = (int)e.Item.DataItem;
                HtmlGenericControl li = (HtmlGenericControl)e.Item.FindControl("liPage");
                if (page == CurrentPage)
                    li.Attributes["class"] = "page-item active";
                else
                    li.Attributes["class"] = "page-item";
            }
        }

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument.ToString() == "Prev" && CurrentPage > 1)
                CurrentPage--;
            else if (e.CommandArgument.ToString() == "Next" && CurrentPage < totalPages)
                CurrentPage++;

            BindMyComments(CurrentPage);
            BindPager();
        }

        #endregion

        #region 回复功能

        protected void rptRepliesToMe_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ShowReply2")
            {
                foreach (RepeaterItem item in rptRepliesToMe.Items)
                {
                    var pnl = (Panel)item.FindControl("pnlReplyBox2");
                    pnl.Visible = false;
                }

                var pnlReply = (Panel)e.Item.FindControl("pnlReplyBox2");
                pnlReply.Visible = true;
            }
            else if (e.CommandName == "ReplyToReply")
            {
                var txtReply = (TextBox)e.Item.FindControl("txtReplyContent2");
                string replyContent = txtReply.Text.Trim();
                if (string.IsNullOrEmpty(replyContent))
                {
                    // 提示不能为空
                    return;
                }
                int parentId = int.Parse(e.CommandArgument.ToString());
                SaveReply(parentId, replyContent);
                BindMyComments(CurrentPage);
                BindRepliesToMe();
                BindPager();
            }
        }

        private void SaveReply(int parentCommentId, string content)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string getPostIdSql = "SELECT PostId FROM Comments WHERE Id = @ParentId";
                SqlCommand cmdGetPost = new SqlCommand(getPostIdSql, conn);
                cmdGetPost.Parameters.AddWithValue("@ParentId", parentCommentId);
                int postId = (int)cmdGetPost.ExecuteScalar();

                string insertSql = @"
                    INSERT INTO Comments (PostId, SubscriberId, Content, ParentId, IsApproved, IsDeleted, CreatedAt, IP)
                    VALUES (@PostId, @SubscriberId, @Content, @ParentId, @IsApproved, 0, GETDATE(), @IP)
                ";

                SqlCommand cmd = new SqlCommand(insertSql, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@SubscriberId", CurrentUserId);
                cmd.Parameters.AddWithValue("@Content", content);
                cmd.Parameters.AddWithValue("@ParentId", parentCommentId);
                cmd.Parameters.AddWithValue("@IP", Request.UserHostAddress);
                cmd.Parameters.AddWithValue("@IsApproved", 0);

                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        private void MarkRepliesAsRead(int userId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
            UPDATE c
            SET c.IsReadByParent = 1
            FROM Comments c
            INNER JOIN Comments parent ON c.ParentId = parent.Id
            WHERE parent.SubscriberId = @UserId
              AND c.SubscriberId != @UserId
              AND c.IsApproved = 1
              AND c.IsDeleted = 0
              AND ISNULL(c.IsReadByParent, 0) = 0";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void BindMyReplies()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string sql = @"
            SELECT c.Id, c.Content, c.CreatedAt, p.Title AS PostTitle, s.Username AS OriginalCommenterName
            FROM Comments c
            INNER JOIN Comments parent ON c.ParentId = parent.Id
            INNER JOIN Subscribers s ON parent.SubscriberId = s.Id
            INNER JOIN Posts p ON c.PostId = p.Id
            WHERE c.SubscriberId = @UserId AND c.IsDeleted = 0 AND c.IsApproved = 1
            ORDER BY c.CreatedAt DESC
            OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", CurrentUserId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptMyReplies.DataSource = dt;
                rptMyReplies.DataBind();
            }
        }
    }
}
