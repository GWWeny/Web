using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class DepartmentAdd1 : System.Web.UI.Page
    {
        private DepartmentDataAccess1 departmentDataAccess = new DepartmentDataAccess1();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Department newDepartment = new Department
            {
                Departname = txtDepartname.Text,
                Description = txtDescription.Text
            };

            departmentDataAccess.Insert(newDepartment);
            Response.Redirect("experiment1.aspx"); 
        }
    }
}