<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="experiment4.aspx.cs" Inherits="WebApplication.experiment4" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>客户与部门管理</title>
    <!-- 引入 Bootstrap 样式 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">

    <!-- 导航条 -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
        <div class="container-fluid">
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" href="experiment1.aspx">实验一</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="experiment4.aspx">实验四</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="experiment5.aspx">实验五</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <form id="form1" runat="server">
        <div class="container mt-5">

            <!-- 部门管理 -->
            <h2 class="mb-3 text-primary">部门管理</h2>

            <asp:GridView ID="gvDepartment" runat="server" AutoGenerateColumns="False"
                CssClass="table table-bordered table-striped"
                DataKeyNames="Id"
                OnRowEditing="gvDepartment_RowEditing"
                OnRowUpdating="gvDepartment_RowUpdating"
                OnRowCancelingEdit="gvDepartment_RowCancelingEdit"
                OnRowDeleting="gvDepartment_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="编号" ReadOnly="True" />
                    <asp:BoundField DataField="Departname" HeaderText="部门名称" />
                    <asp:BoundField DataField="Description" HeaderText="描述" />
                    <asp:CommandField ShowEditButton="True" EditText="编辑" CancelText="取消" UpdateText="保存" ShowDeleteButton="True" DeleteText="删除" />
                </Columns>
            </asp:GridView>

            <div class="row g-3 mt-3">
                <div class="col-md-4">
                    <asp:TextBox ID="txtDepartName" runat="server" CssClass="form-control" placeholder="请输入部门名称"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtDepartDesc" runat="server" CssClass="form-control" placeholder="请输入描述"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <asp:Button ID="btnAddDept" runat="server" Text="新增部门" CssClass="btn btn-success w-100" OnClick="btnAddDept_Click" />
                </div>
            </div>

            <hr class="my-5" />

            <!-- 客户管理 -->
            <h2 class="mb-3 text-primary">客户管理</h2>

            <asp:GridView ID="gvCustom" runat="server" AutoGenerateColumns="False"
                CssClass="table table-bordered table-striped"
                DataKeyNames="Id"
                OnRowEditing="gvCustom_RowEditing"
                OnRowUpdating="gvCustom_RowUpdating"
                OnRowCancelingEdit="gvCustom_RowCancelingEdit"
                OnRowDeleting="gvCustom_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="编号" ReadOnly="True" />
                    <asp:BoundField DataField="Cname" HeaderText="姓名" />
                    <asp:BoundField DataField="Age" HeaderText="年龄" />
                    <asp:BoundField DataField="Ename" HeaderText="邮箱" />
                    <asp:BoundField DataField="Password" HeaderText="密码" />
                    <asp:BoundField DataField="DepartID" HeaderText="所属部门ID" />
                    <asp:CommandField ShowEditButton="True" EditText="编辑" CancelText="取消" UpdateText="保存" ShowDeleteButton="True" DeleteText="删除" />
                </Columns>
            </asp:GridView>

            <div class="row g-3 mt-3">
                <div class="col-md-2">
                    <asp:TextBox ID="txtCname" runat="server" CssClass="form-control" placeholder="姓名"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" placeholder="年龄"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtEname" runat="server" CssClass="form-control" placeholder="邮箱"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtPwd" runat="server" CssClass="form-control" placeholder="密码"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlDepartments" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnAddCustom" runat="server" Text="新增客户" CssClass="btn btn-primary w-100" OnClick="btnAddCustom_Click" />
                </div>
            </div>

            <!-- 实验说明 -->
            <div class="bg-light p-4 mt-5 rounded shadow">
                <h3 class="text-primary">实验四说明</h3>
                <p class="lead">
                    设计一个基于Web的应用程序，采用ORM（主要就是Linq或EF的方式）对custom，department表中的记录进行：插入、修改、删除、查询的操作
                </p>
            </div>

        </div>
    </form>

    <!-- 引入 Bootstrap JS（可选） -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
