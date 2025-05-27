<%@ Page Title="修改密码" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Information_dissemination_system.ChangePassword" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5" style="max-width: 500px;">
        <div class="card shadow-sm">
            <div class="card-header bg-warning text-dark text-center fs-4 fw-bold">
                修改密码
            </div>
            <div class="card-body">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger text-center d-block mb-3" />
                
                <div class="mb-3">
                    <label for="txtOldPassword" class="form-label">旧密码</label>
                    <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password" />
                </div>

                <div class="mb-3">
                    <label for="txtNewPassword" class="form-label">新密码</label>
                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" />
                </div>

                <div class="mb-3">
                    <label for="txtConfirmPassword" class="form-label">确认新密码</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                </div>

                <div class="d-grid">
                    <asp:Button ID="btnChangePassword" runat="server" Text="确认修改" CssClass="btn btn-warning" OnClick="btnChangePassword_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
