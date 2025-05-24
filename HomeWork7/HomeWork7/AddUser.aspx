<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="HomeWork7.AddUser" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加新用户</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>添加新用户</h2>
            <label for="id">ID：</label>
            <asp:TextBox ID="id" runat="server"></asp:TextBox>
            <br />
            <label for="name">姓名：</label>
            <asp:TextBox ID="name" runat="server"></asp:TextBox>
            <br />
            <label for="email">邮箱：</label>
            <asp:TextBox ID="email" runat="server"></asp:TextBox>
            <br />
            <label for="age">年龄：</label>
            <asp:TextBox ID="age" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" />
        </div>
    </form>
</body>
</html>
