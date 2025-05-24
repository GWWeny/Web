using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeWork7
{
    public partial class AddUser : System.Web.UI.Page
    {
        private SqlDataAccess _dataAccess = new SqlDataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // 使用FindControl方法获取控件实例
            TextBox idTextBox = (TextBox)FindControl("ID");
            TextBox nameTextBox = (TextBox)FindControl("name");
            TextBox emailTextBox = (TextBox)FindControl("email");
            TextBox ageTextBox = (TextBox)FindControl("age");

            int id = int.Parse(idTextBox.Text);
            string name = nameTextBox.Text;
            string email = emailTextBox.Text;
            int age = int.Parse(ageTextBox.Text);

            string query = "INSERT INTO Users (ID, Name, Email, Age) VALUES (@ID, @Name, @Email, @Age)";
            _dataAccess.ExecuteNonQuery(query,new SqlParameter("@ID",id),  new SqlParameter("@Name", name), new SqlParameter("@Email", email), new SqlParameter("@Age", age));

            Response.Redirect("Default.aspx");
        }
    }
}