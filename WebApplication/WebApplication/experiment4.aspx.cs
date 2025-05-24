using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication.Models;

namespace WebApplication
{
    public partial class experiment4 : System.Web.UI.Page
    {
        AppDbContext db = new AppDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
                LoadCustoms();
                LoadDepartmentDropdown();
            }
        }

        private void LoadDepartments()
        {
            gvDepartment.DataSource = db.Departments.ToList();
            gvDepartment.DataBind();
        }

        private void LoadCustoms()
        {
            gvCustom.DataSource = db.Customs.ToList();
            gvCustom.DataBind();
        }

        private void LoadDepartmentDropdown()
        {
            ddlDepartments.DataSource = db.Departments.ToList();
            ddlDepartments.DataTextField = "Departname";
            ddlDepartments.DataValueField = "Id";
            ddlDepartments.DataBind();
        }

        // -------- Department CRUD --------

        protected void btnAddDept_Click(object sender, EventArgs e)
        {
            var dept = new WebApplication.Models.Department
            {
                Departname = txtDepartName.Text,
                Description = txtDepartDesc.Text
            };
            db.Departments.Add(dept);
            db.SaveChanges();
            LoadDepartments();
            LoadDepartmentDropdown();
        }

        protected void gvDepartment_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvDepartment.EditIndex = e.NewEditIndex;
            LoadDepartments();
        }

        protected void gvDepartment_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvDepartment.DataKeys[e.RowIndex].Value);
            var dept = db.Departments.Find(id);
            dept.Departname = ((System.Web.UI.WebControls.TextBox)gvDepartment.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            dept.Description = ((System.Web.UI.WebControls.TextBox)gvDepartment.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            db.SaveChanges();
            gvDepartment.EditIndex = -1;
            LoadDepartments();
        }

        protected void gvDepartment_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvDepartment.EditIndex = -1;
            LoadDepartments();
        }

        protected void gvDepartment_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvDepartment.DataKeys[e.RowIndex].Value);
            var dept = db.Departments.Find(id);
            db.Departments.Remove(dept);
            db.SaveChanges();
            LoadDepartments();
            LoadDepartmentDropdown();
        }

        // -------- Custom CRUD --------

        protected void btnAddCustom_Click(object sender, EventArgs e)
        {
            var custom = new Models.Custom
            {
                Cname = txtCname.Text,
                Age = int.Parse(txtAge.Text),
                Ename = txtEname.Text,
                Password = txtPwd.Text,
                DepartID = int.Parse(ddlDepartments.SelectedValue)
            };
            db.Customs.Add(custom);
            db.SaveChanges();
            LoadCustoms();
        }

        protected void gvCustom_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvCustom.EditIndex = e.NewEditIndex;
            LoadCustoms();
        }

        protected void gvCustom_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvCustom.DataKeys[e.RowIndex].Value);
            var custom = db.Customs.Find(id);
            custom.Cname = ((System.Web.UI.WebControls.TextBox)gvCustom.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            custom.Age = int.Parse(((System.Web.UI.WebControls.TextBox)gvCustom.Rows[e.RowIndex].Cells[2].Controls[0]).Text);
            custom.Ename = ((System.Web.UI.WebControls.TextBox)gvCustom.Rows[e.RowIndex].Cells[3].Controls[0]).Text;
            custom.Password = ((System.Web.UI.WebControls.TextBox)gvCustom.Rows[e.RowIndex].Cells[4].Controls[0]).Text;
            custom.DepartID = int.Parse(((System.Web.UI.WebControls.TextBox)gvCustom.Rows[e.RowIndex].Cells[5].Controls[0]).Text);
            db.SaveChanges();
            gvCustom.EditIndex = -1;
            LoadCustoms();
        }

        protected void gvCustom_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvCustom.EditIndex = -1;
            LoadCustoms();
        }

        protected void gvCustom_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvCustom.DataKeys[e.RowIndex].Value);
            var custom = db.Customs.Find(id);
            db.Customs.Remove(custom);
            db.SaveChanges();
            LoadCustoms();
        }
    }
}