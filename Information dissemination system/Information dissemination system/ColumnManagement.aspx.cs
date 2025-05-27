using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Information_dissemination_system
{
    public partial class ColumnManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 仅管理员可访问（需Session["IsAdmin"]提前设置为true或false）
                if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                {
                    Response.Redirect("Home.aspx");
                }
                BindGrid();
            }
        }

        private void BindGrid()
        {
            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Id, Name, Description FROM Categories ORDER BY Id";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvCategories.DataSource = dt;
                gvCategories.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                lblMessage.Text = "栏目名称不能为空";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "INSERT INTO Categories (Name, Description) VALUES (@Name, @Description)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // 清空输入框（刷新后其实会重新渲染页面，下面也可不写）
            txtName.Text = "";
            txtDescription.Text = "";

            // 先给提示，再刷新页面（刷新后提示会消失）
            lblMessage.Text = "添加成功";

            // 页面刷新
            Response.Redirect(Request.RawUrl);
        }


        protected void gvCategories_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvCategories.EditIndex = e.NewEditIndex;
            BindGrid();

            
        }

        protected void gvCategories_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvCategories.EditIndex = -1;
            BindGrid();

            
        }

        protected void gvCategories_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
            string name = ((System.Web.UI.WebControls.TextBox)gvCategories.Rows[e.RowIndex].Cells[1].Controls[0]).Text.Trim();
            string description = ((System.Web.UI.WebControls.TextBox)gvCategories.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                lblMessage.Text = "栏目名称不能为空";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE Categories SET Name=@Name, Description=@Description WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            gvCategories.EditIndex = -1;
            lblMessage.Text = "更新成功";
            BindGrid();

            // 页面刷新
            Response.Redirect(Request.RawUrl);
        }

        protected void gvCategories_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);

            string connStr = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "DELETE FROM Categories WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            lblMessage.Text = "删除成功";
            BindGrid();

            // 页面刷新
            Response.Redirect(Request.RawUrl);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}