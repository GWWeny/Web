<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Information_dissemination_system.Register" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>用户注册</title>
    <!-- 引入 Bootstrap CDN -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f5f5f5;
        }
        .register-container {
            max-width: 500px;
            margin: 60px auto;
            background: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,.1);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="register-container">
                <h3 class="text-center mb-4">用户注册</h3>

                <div class="mb-3">
                    <label for="txtUsername" class="form-label">用户名</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">密码</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="txtConfirmPassword" class="form-label">确认密码</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">邮箱</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3 form-check">
                    <asp:CheckBox ID="chkIsAdmin" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label" for="chkIsAdmin">是否管理员</label>
                </div>

                <asp:Button ID="btnRegister" runat="server" Text="注册" CssClass="btn btn-primary w-100" OnClick="btnRegister_Click" />

                <div class="mt-3 text-center">
                    <asp:Label ID="lblMessage" runat="server" CssClass="form-text text-danger"></asp:Label>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS (可选，如果需要交互效果) -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
