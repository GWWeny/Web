<%@ Page Title="栏目管理" Language="C#" AutoEventWireup="true" CodeBehind="ColumnManagement.aspx.cs" Inherits="Information_dissemination_system.ColumnManagement" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            max-width: 900px;
            background: white;
            padding: 30px;
            margin-top: 40px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgb(0 0 0 / 0.1);
        }
        h2 {
            margin-bottom: 25px;
            font-weight: 700;
            color: #dc3545;
            text-align: center;
        }
        .form-control:focus {
            box-shadow: 0 0 8px #dc3545;
            border-color: #dc3545;
        }
        .btn-primary {
            background-color: #dc3545;
            border-color: #dc3545;
        }
        .btn-primary:hover {
            background-color: #b52b35;
            border-color: #b52b35;
        }
        .message {
            font-weight: 600;
        }
        .gridview-header {
            background-color: #dc3545;
            color: white;
            font-weight: 600;
        }
        .gridview-row:hover {
            background-color: #fbeaea;
        }
    </style>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>栏目管理</h2>

        <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
            OnRowEditing="gvCategories_RowEditing" OnRowCancelingEdit="gvCategories_RowCancelingEdit" OnRowUpdating="gvCategories_RowUpdating"
            OnRowDeleting="gvCategories_RowDeleting" DataKeyNames="Id" 
            HeaderStyle-CssClass="gridview-header" RowStyle-CssClass="gridview-row">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true" ItemStyle-Width="60px" />
                <asp:BoundField DataField="Name" HeaderText="栏目名称" ItemStyle-Width="200px" />
                <asp:BoundField DataField="Description" HeaderText="栏目描述" />
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="140px" />
            </Columns>
        </asp:GridView>

        <h4 class="mt-4">添加新栏目</h4>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control mb-2" Placeholder="栏目名称"></asp:TextBox>
        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control mb-2" Placeholder="栏目描述（可选）"></asp:TextBox>
        <asp:Button ID="btnAdd" runat="server" Text="添加栏目" CssClass="btn btn-primary mt-2" OnClick="btnAdd_Click" />
        <asp:Button ID="btnBack" runat="server" Text="返回首页" CssClass="btn btn-primary mt-2" OnClick="btnBack_Click" />

        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-3 message"></asp:Label>

        
    </div>
</asp:Content>
