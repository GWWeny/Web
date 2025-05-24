<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HomeWork7.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户信息管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>用户信息管理</h2>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting" OnRowCancelingEdit="GridView1_RowCancelingEdit">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" SortExpression="Id" />
                    <asp:BoundField DataField="Name" HeaderText="姓名" SortExpression="Name" />
                    <asp:BoundField DataField="Email" HeaderText="邮箱" SortExpression="Email" />
                    <asp:BoundField DataField="Age" HeaderText="年龄" SortExpression="Age" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
            <asp:Button ID="btnAdd" runat="server" Text="添加新用户" OnClick="btnAdd_Click" />
        </div>
    </form>
</body>
</html>
