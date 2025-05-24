using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class CustomAdd1 : System.Web.UI.Page
    {
        private readonly CustomDataAccess1 customData = new CustomDataAccess1();
        private readonly DepartmentDataAccess1 deptData = new DepartmentDataAccess1();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
                ValidateDepartments();
            }
        }

        private void LoadDepartments()
        {
            try
            {
                ddlDepart.DataSource = deptData.GetAll();
                ddlDepart.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert($"部门加载失败: {ex.Message}");
            }
        }

        private void ValidateDepartments()
        {
            if (ddlDepart.Items.Count <= 1) // 1个默认项+数据项
            {
                ShowAlert("请先创建部门数据");
                Response.Redirect("DepartmentAdd1.aspx");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlDepart.SelectedValue))
            {
                ShowAlert("请选择所属部门");
                return;
            }

            try
            {
                var newUser = new Custom
                {
                    Cname = txtCname.Text.Trim(),
                    DepartID = int.Parse(ddlDepart.SelectedValue),
                    Age = int.Parse(txtAge.Text),
                    Ename = txtEname.Text.Trim(),
                    Password = txtPassword.Text
                };

                if (newUser.Password.Length < 6)
                {
                    ShowAlert("密码长度不能小于6位");
                    return;
                }

                customData.Insert(newUser);
                Response.Redirect("experiment1.aspx?success=true");
            }
            catch (System.Data.SqlClient.SqlException ex) when (ex.Number == 547)
            {
                ShowAlert("无效的部门ID，请刷新部门列表");
                LoadDepartments();
            }
            catch (FormatException)
            {
                ShowAlert("输入格式不正确");
            }
        }

        private void ShowAlert(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                $"alert('{message}');", true);
        }
    }
}