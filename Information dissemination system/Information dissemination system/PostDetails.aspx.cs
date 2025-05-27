using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace Information_dissemination_system
{
    public partial class PostDetails : System.Web.UI.Page
    {
        private class CommentDisplayItem
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Content { get; set; }
            public DateTime CreatedAt { get; set; }
            public int? ParentId { get; set; }
            public string ReplyToUsername { get; set; }
            public string ReplyToContent { get; set; }
            public int Level { get; set; }
        }

        //我想使用一个C#的外部库来做敏感词检测的，但是我的目标框架是 .NET Framework 4.7.2，
        //而我要安装的 ToolGood.Words 3.1.0.2 版本 不支持这个框架。所以只能使用轻量级自定义敏感词库
        private static readonly List<string> sensitiveWords = new List<string>
        {
            "傻逼", "妈的", "操", "你妹", "滚", "草", "垃圾", "死", "白痴", "智障",
            "fuck", "shit", "asshole", "bitch", "bastard", "nmsl", "sb", "wtf",
            "gay", "死全家", "狗东西", "王八蛋", "鸡巴", "阴道", "你丫", "贱人", "去死",
            "叼你", "日你", "干你", "老母", "废物", "脑瘫", "脑残", "低能", "变态", "吃屎"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int postId = int.Parse(Request.QueryString["id"]);
                    IncrementPostViewCount(postId);
                }

                LoadPost();
                LoadComments();
                pnlCommentForm.Visible = Session["UserId"] != null;
            }
        }

        private void LoadPost()
        {
            if (Request.QueryString["id"] == null)
            {
                lblMessage.Text = "参数错误！";
                lblMessage.Visible = true;
                return;
            }

            int postId = int.Parse(Request.QueryString["id"]);
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Title, Content, CoverImage, Views, CreatedAt FROM Posts WHERE Id = @Id AND IsPublished = 1";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", postId);

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblTitle.Text = reader["Title"].ToString();
                    lblContent.Text = reader["Content"].ToString();
                    imgCover.ImageUrl = reader["CoverImage"]?.ToString() ?? "images/default-cover.jpg";
                    lblViews.Text = reader["Views"].ToString();
                    lblCreatedAt.Text = Convert.ToDateTime(reader["CreatedAt"]).ToString("yyyy-MM-dd HH:mm");
                    pnlPost.Visible = true;
                }
                else
                {
                    lblMessage.Text = "未找到对应的文章。";
                    lblMessage.Visible = true;
                }
                conn.Close();
            }
        }

        private void IncrementPostViewCount(int postId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE Posts SET Views = Views + 1 WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", postId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void LoadComments()
        {
            if (Request.QueryString["id"] == null) return;
            int postId = int.Parse(Request.QueryString["id"]);
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

            List<CommentDisplayItem> allComments = new List<CommentDisplayItem>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT C.Id, S.Username, C.Content, C.CreatedAt, C.ParentId,
                           R.Username AS ReplyToUsername, RP.Content AS ReplyToContent
                    FROM Comments C
                    INNER JOIN Subscribers S ON C.SubscriberId = S.Id
                    LEFT JOIN Comments P ON C.ParentId = P.Id
                    LEFT JOIN Subscribers R ON P.SubscriberId = R.Id
                    LEFT JOIN Comments RP ON C.ParentId = RP.Id
                    WHERE C.PostId = @PostId AND C.IsApproved = 1 AND C.IsDeleted = 0
                    ORDER BY C.CreatedAt ASC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    allComments.Add(new CommentDisplayItem
                    {
                        Id = (int)reader["Id"],
                        Username = reader["Username"].ToString(),
                        Content = reader["Content"].ToString(),
                        CreatedAt = (DateTime)reader["CreatedAt"],
                        ParentId = reader["ParentId"] == DBNull.Value ? (int?)null : (int)reader["ParentId"],
                        ReplyToUsername = reader["ReplyToUsername"] == DBNull.Value ? null : reader["ReplyToUsername"].ToString(),
                        ReplyToContent = reader["ReplyToContent"] == DBNull.Value ? null : reader["ReplyToContent"].ToString(),
                        Level = 0
                    });
                }
                conn.Close();
            }

            var dict = new Dictionary<int, CommentDisplayItem>();
            foreach (var c in allComments)
                dict[c.Id] = c;

            foreach (var c in allComments)
                c.Level = GetCommentLevel(c, dict);

            rptComments.DataSource = allComments;
            rptComments.DataBind();
        }

        private int GetCommentLevel(CommentDisplayItem comment, Dictionary<int, CommentDisplayItem> dict)
        {
            int level = 0;
            var current = comment;
            while (current.ParentId.HasValue && dict.ContainsKey(current.ParentId.Value))
            {
                level++;
                current = dict[current.ParentId.Value];
            }
            return level;
        }

        protected int GetIndentLevel(int level)
        {
            return level * 30;
        }

        protected string GetReplyToText(object replyToUsername)
        {
            if (replyToUsername == null || replyToUsername == DBNull.Value)
                return "";
            else
                return $" 回复 @{replyToUsername.ToString()} ";
        }

        protected void btnReply_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            string parentId = btn.CommandArgument;

            hfParentId.Value = parentId;
            pnlCommentForm.Visible = true;
            btnCancelReply.Visible = true;
            lblCommentFormTitle.Text = "回复留言";
            txtComment.Focus();
        }

        protected void btnCancelReply_Click(object sender, EventArgs e)
        {
            hfParentId.Value = "";
            pnlCommentForm.Visible = true;
            btnCancelReply.Visible = false;
            lblCommentFormTitle.Text = "发表评论";
            txtComment.Text = "";
        }

        private bool IsContentSafe(string content)
        {
            foreach (var word in sensitiveWords)
            {
                if (content.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return false;
                }
            }
            return true;
        }


        protected void btnSubmitComment_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                lblMessage.Text = "请先登录后留言。";
                lblMessage.Visible = true;
                return;
            }

            string content = txtComment.Text.Trim();
            if (string.IsNullOrEmpty(content))
            {
                lblMessage.Text = "留言内容不能为空。";
                lblMessage.Visible = true;
                return;
            }

            if (Request.QueryString["id"] == null)
            {
                lblMessage.Text = "参数错误，无法提交留言。";
                lblMessage.Visible = true;
                return;
            }

            int postId = int.Parse(Request.QueryString["id"]);
            int userId = (int)Session["UserId"];
            int? parentId = null;
            if (int.TryParse(hfParentId.Value, out int pid))
                parentId = pid;

            bool isSafe = IsContentSafe(content);
            bool isApproved = isSafe;

            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
                    INSERT INTO Comments (PostId, SubscriberId, Content, ParentId, IsApproved, IsDeleted, IP, CreatedAt)
                    VALUES (@PostId, @SubscriberId, @Content, @ParentId, @IsApproved, 0, @IP, GETDATE())";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@SubscriberId", userId);
                cmd.Parameters.AddWithValue("@Content", content);
                cmd.Parameters.AddWithValue("@ParentId", (object)parentId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsApproved", isApproved);
                cmd.Parameters.AddWithValue("@IP", Request.UserHostAddress);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            lblMessage.CssClass = "alert alert-success";
            lblMessage.Text = isApproved ? "评论已自动审核通过并发布。" : "评论中含有敏感词，已提交管理员审核。";
            lblMessage.Visible = true;

            hfParentId.Value = "";
            pnlCommentForm.Visible = Session["UserId"] != null;
            btnCancelReply.Visible = false;
            lblCommentFormTitle.Text = "发表评论";
            txtComment.Text = "";

            LoadComments();
        }
    }
}
