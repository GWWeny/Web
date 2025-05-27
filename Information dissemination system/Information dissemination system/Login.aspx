<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Information_dissemination_system.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>用户登录</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f0f2f5;
        }
        .login-container {
            max-width: 400px;
            margin: 80px auto;
            background-color: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.1);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="login-container">
                <h3 class="text-center mb-4">登录</h3>

                <div class="mb-3">
                    <label for="txtUsername" class="form-label">用户名</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">密码</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                </div>

                <asp:Button ID="btnLogin" runat="server" Text="登录" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />

                <div class="mt-3 text-center">
                    <asp:Label ID="lblMessage" runat="server" CssClass="form-text text-danger"></asp:Label>
                </div>

                <div class="mt-3 text-center">
                    <a href="Register.aspx" class="text-decoration-none">还没有账号？立即注册</a>
                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
