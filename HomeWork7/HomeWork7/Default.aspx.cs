using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace HomeWork7
{
    public partial class Default : Page
    {
        private SqlDataAccess _dataAccess = new SqlDataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        private void BindGridView()
        {
            string query = "SELECT * FROM Users";
            DataTable dataTable = _dataAccess.GetDataTable(query);
            GridView1.DataSource = dataTable;
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddUser.aspx");
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            string name = ((TextBox)GridView1.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string email = ((TextBox)GridView1.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            int age = Convert.ToInt32(((TextBox)GridView1.Rows[e.RowIndex].Cells[3].Controls[0]).Text);

            string query = "UPDATE Users SET Name=@Name, Email=@Email, Age=@Age WHERE Id=@Id";
            _dataAccess.ExecuteNonQuery(query, new SqlParameter("@Name", name), new SqlParameter("@Email", email), new SqlParameter("@Age", age), new SqlParameter("@Id", id));

            GridView1.EditIndex = -1;
            BindGridView();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            string query = "DELETE FROM Users WHERE Id = @Id";
            _dataAccess.ExecuteNonQuery(query, new SqlParameter("@Id", id));

            BindGridView();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridView();
        }
    }
}