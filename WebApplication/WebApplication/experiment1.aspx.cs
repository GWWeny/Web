using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication;

namespace WebApplication
{
    public partial class experiment1 : System.Web.UI.Page
    {
        private CustomDataAccess1 customDataAccess = new CustomDataAccess1();
        private DepartmentDataAccess1 departmentDataAccess = new DepartmentDataAccess1();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomGrid();
                BindDepartmentGrid();
            }
        }

        private void BindCustomGrid()
        {
            List<Custom> customs = customDataAccess.GetAll();
            GridViewCustom.DataSource = customs;
            GridViewCustom.DataBind();
        }

        private void BindDepartmentGrid()
        {
            List<Department> departments = departmentDataAccess.GetAll();
            GridViewDepartment.DataSource = departments;
            GridViewDepartment.DataBind();
        }

        protected void GridViewCustom_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridViewCustom.DataKeys[e.RowIndex].Value);
            customDataAccess.Delete(id);
            BindCustomGrid();
        }

        protected void GridViewCustom_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCustom.EditIndex = e.NewEditIndex;
            BindCustomGrid();
        }

        protected void GridViewCustom_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Custom custom = new Custom();
            custom.Id = Convert.ToInt32(GridViewCustom.DataKeys[e.RowIndex].Value);
            custom.Cname = ((TextBox)GridViewCustom.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            custom.DepartID = Convert.ToInt32(((TextBox)GridViewCustom.Rows[e.RowIndex].Cells[2].Controls[0]).Text);
            custom.Age = Convert.ToInt32(((TextBox)GridViewCustom.Rows[e.RowIndex].Cells[3].Controls[0]).Text);
            custom.Ename = ((TextBox)GridViewCustom.Rows[e.RowIndex].Cells[4].Controls[0]).Text;
            custom.Password = ((TextBox)GridViewCustom.Rows[e.RowIndex].Cells[5].Controls[0]).Text;

            customDataAccess.Update(custom);
            GridViewCustom.EditIndex = -1;
            BindCustomGrid();
        }

        protected void GridViewCustom_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCustom.EditIndex = -1;
            BindCustomGrid();
        }

        protected void btnAddCustom_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomAdd1.aspx");
        }

        protected void GridViewDepartment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridViewDepartment.DataKeys[e.RowIndex].Value);
            departmentDataAccess.Delete(id);
            BindDepartmentGrid();
        }

        protected void GridViewDepartment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewDepartment.EditIndex = e.NewEditIndex;
            BindDepartmentGrid();
        }

        protected void GridViewDepartment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Department department = new Department();
            department.Id = Convert.ToInt32(GridViewDepartment.DataKeys[e.RowIndex].Value);
            department.Departname = ((TextBox)GridViewDepartment.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            department.Description = ((TextBox)GridViewDepartment.Rows[e.RowIndex].Cells[2].Controls[0]).Text;

            departmentDataAccess.Update(department);
            GridViewDepartment.EditIndex = -1;
            BindDepartmentGrid();
        }

        protected void GridViewDepartment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewDepartment.EditIndex = -1;
            BindDepartmentGrid();
        }

        protected void btnAddDepartment_Click(object sender, EventArgs e)
        {
            Response.Redirect("DepartmentAdd1.aspx");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Navigation.aspx");
        }
    }
}