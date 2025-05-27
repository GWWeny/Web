using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Information_dissemination_system
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear(); // 清除所有Session
            Session.Abandon();
            Response.Redirect("Home.aspx"); // 返回主页
        }
    }
}