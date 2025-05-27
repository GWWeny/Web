<%@ Page Title="个人信息" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="Information_dissemination_system.UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .profile-form {
            max-width: 600px;
            margin: auto;
            background: #f9f9f9;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }

        .form-title {
            text-align: center;
            margin-bottom: 25px;
            font-size: 24px;
            font-weight: bold;
        }
    </style>

    <!-- 新增头像预览的JS -->
    <script type="text/javascript">
        function previewAvatar(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = document.getElementById('<%= imgCurrentAvatar.ClientID %>');
                    img.src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5" style="max-width: 600px;">
        <div class="card shadow-sm">
            <div class="card-header bg-primary text-white text-center fs-4 fw-bold">
                修改个人信息
            </div>
            <div class="card-body">

                <!-- 当前头像显示 -->
                <div class="text-center mb-4">
                    <asp:Image ID="imgCurrentAvatar" runat="server" CssClass="rounded-circle border"
                        Style="width:120px; height:120px; object-fit:cover;" />
                </div>

                <asp:Label ID="lblMessage" runat="server" CssClass="mb-3 d-block text-center" ForeColor="Red" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger mb-3" />

                <div class="mb-3">
                    <label class="form-label fw-semibold">用户名 (不可修改)</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">姓名</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">性别</label>
                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                        <asp:ListItem Text="请选择" Value="" />
                        <asp:ListItem Text="男" Value="男" />
                        <asp:ListItem Text="女" Value="女" />
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">出生日期</label>
                    <asp:TextBox ID="txtBirthDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">手机号</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">邮箱</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                </div>

                <!-- 头像上传 -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">上传头像</label>
                    <asp:FileUpload ID="fuAvatar" runat="server" CssClass="form-control" 
                        onchange="previewAvatar(this)" />
                    <small class="text-muted">支持jpg, jpeg, png, gif格式</small>
                </div>

                <hr />

                <div class="d-flex justify-content-between align-items-center">
                    <asp:Button ID="btnSave" runat="server" Text="保存修改" CssClass="btn btn-primary btn-lg flex-grow-1 me-2" OnClick="btnSave_Click" />
                    <asp:HyperLink ID="hlChangePassword" runat="server" NavigateUrl="ChangePassword.aspx" CssClass="btn btn-primary btn-lg flex-grow-1">
                        修改密码
                    </asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
