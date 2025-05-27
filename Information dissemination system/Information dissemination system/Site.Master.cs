using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Information_dissemination_system
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindCategories();

            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    int userId = Convert.ToInt32(Session["UserId"]);
                    string username = Session["Username"]?.ToString() ?? "";

                    // 获取头像
                    string avatarUrl = GetAvatarUrl(userId);
                    if (string.IsNullOrEmpty(avatarUrl))
                    {
                        avatarUrl = "images/default-avatar.png";
                    }

                    avatarUrl = ResolveUrl(avatarUrl);  // 转换为绝对路径，去掉 ~ 符号
                    // 更新Session头像和用户名，确保最新
                    Session["Avatar"] = avatarUrl;
                    Session["Username"] = username;

                    pnlUserInfo.Visible = true;
                    pnlLogin.Visible = false;

                    // 根据 Session 中的 IsAdmin 字段显示管理员菜单
                    bool isAdmin = Session["IsAdmin"] != null && Convert.ToBoolean(Session["IsAdmin"]);
                    pnlAdminMenu.Visible = isAdmin;

                    // 检查是否有未读新回复
                    CheckNewReplies(userId);
                }
                else
                {
                    pnlUserInfo.Visible = false;
                    pnlLogin.Visible = true;
                    pnlAdminMenu.Visible = false;
                }
            }
        }

        private string GetAvatarUrl(int userId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT Avatar FROM Subscribers WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    object result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        private void BindCategories()
        {
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Id, Name, Description FROM Categories ORDER BY Id";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptCategories.DataSource = dt;
                rptCategories.DataBind();
            }
        }

        private void CheckNewReplies(int userId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
            SELECT COUNT(*) FROM Comments c
            INNER JOIN Comments parent ON c.ParentId = parent.Id
            WHERE parent.SubscriberId = @UserId
              AND c.SubscriberId != @UserId
              AND c.IsApproved = 1
              AND c.IsDeleted = 0
              AND ISNULL(c.IsReadByParent, 0) = 0
        ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    int unreadCount = (int)cmd.ExecuteScalar();

                    // 控制小红点显示
                    msgDot.Attributes["class"] = "position-absolute top-0 start-100 translate-middle p-1 bg-danger border border-light rounded-circle" +
                                                 (unreadCount > 0 ? "" : " d-none");
                }
            }

        }
    }
}
