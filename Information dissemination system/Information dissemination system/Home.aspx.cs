using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Information_dissemination_system
{
    public partial class Home : System.Web.UI.Page
    {
        // 新增头像和用户名字段，用于绑定控件
        protected string AvatarUrl = "images/default-avatar.png"; // 默认头像
        protected string UsernameDisplay = "";
        private int userId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserInfoAndPermissions();
                BindUserInfoControls(); // 新增，给头像和用户名控件赋值
                CheckNewReplies(userId);
                LoadHomePosts();
                BindCategories();
                CheckLoginState();
            }
        }

        private void LoadUserInfoAndPermissions()
        {
            if (Session["UserId"] != null)
            {
                // ✅ 赋值给类字段
                userId = Convert.ToInt32(Session["UserId"]);
                string username = Session["Username"]?.ToString() ?? "";

                string avatarUrl = null;
                bool isAdmin = false;

                string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT Avatar, IsAdmin FROM Subscribers WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                avatarUrl = reader["Avatar"] == DBNull.Value ? null : reader["Avatar"].ToString();
                                isAdmin = reader["IsAdmin"] != DBNull.Value && (bool)reader["IsAdmin"];
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(avatarUrl))
                {
                    avatarUrl = "images/default-avatar.png";
                }
                else if (avatarUrl.StartsWith("~"))
                {
                    avatarUrl = ResolveUrl(avatarUrl);
                }

                AvatarUrl = avatarUrl;
                UsernameDisplay = username;

                Session["Avatar"] = avatarUrl;
                Session["Username"] = username;
                Session["IsAdmin"] = isAdmin;

                pnlUserInfo.Visible = true;
                pnlLogin.Visible = false;
                pnlAdminMenu.Visible = isAdmin;
            }
            else
            {
                pnlUserInfo.Visible = false;
                pnlLogin.Visible = true;
                pnlAdminMenu.Visible = false;
            }
        }

        // 新增方法，绑定头像和用户名到前端控件
        private void BindUserInfoControls()
        {
            imgAvatar.ImageUrl = AvatarUrl;  // imgAvatar 是前端 <asp:Image> 控件 ID
            lblUsername.Text = UsernameDisplay; // lblUsername 是显示用户名的控件 ID
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

        private void LoadHomePosts()
        {
            string keyword = Request.QueryString["q"];
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
            SELECT TOP 100 
                P.Id, P.Title, P.Content, P.CoverImage, P.Views, P.CreatedAt,
                C.Name AS CategoryName
            FROM Posts P
            INNER JOIN Categories C ON P.CategoryId = C.Id
            WHERE P.IsPublished = 1";

                if (!string.IsNullOrEmpty(keyword))
                {
                    sql += " AND (P.Title LIKE @kw OR P.Content LIKE @kw)";
                }

                sql += " ORDER BY P.IsTop DESC, P.CreatedAt DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (!string.IsNullOrEmpty(keyword))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptHomePosts.DataSource = dt;
                rptHomePosts.DataBind();
            }
        }

        private void CheckLoginState()
        {
            bool isLoggedIn = Session["UserId"] != null;
            pnlLogin.Visible = !isLoggedIn;
            pnlUserInfo.Visible = isLoggedIn;

            if (isLoggedIn && Convert.ToBoolean(Session["IsAdmin"] ?? false))
            {
                pnlAdminMenu.Visible = true;
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
