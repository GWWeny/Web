<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminComments.aspx.cs" Inherits="Information_dissemination_system.AdminComments" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">

        <h3 class="mb-4">留言管理</h3>

        <!-- 搜索栏 -->
        <div class="row mb-4">
            <div class="col-md-4">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="搜索用户名 / 内容 / 时间（yyyy-MM-dd）" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                <asp:Button ID="Button1" runat="server" Text="重置" CssClass="btn btn-primary" OnClick="btnReset_Click" />
            </div>
        </div>

        <!-- 未审核留言 -->
        <h4 class="mb-3">未审核留言</h4>
        <asp:Repeater ID="rptPendingComments" runat="server" OnItemCommand="rptPendingComments_ItemCommand">
            <HeaderTemplate>
                <table class="table table-bordered table-hover">
                    <thead class="table-light">
                        <tr>
                            <th style="width: 5%;">ID</th>
                            <th style="width: 15%;">用户名</th>
                            <th>留言内容</th>
                            <th style="width: 15%;">留言时间</th>
                            <th style="width: 15%;">操作</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Id") %></td>
                    <td><%# Eval("Username") %></td>
                    <td><%# Eval("Content") %></td>
                    <td><%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %></td>
                    <td>
                        <asp:LinkButton ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-success btn-sm me-2">通过</asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-danger btn-sm">删除</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <!-- 未审核留言分页 -->
        <div class="d-flex justify-content-center align-items-center gap-2 mt-2">
            <asp:LinkButton ID="lnkPendingPrev" runat="server" OnClick="lnkPendingPrev_Click" CssClass="btn btn-outline-primary btn-sm">上一页</asp:LinkButton>
        
            <div class="d-flex flex-wrap gap-1">
                <asp:Repeater ID="rptPendingPager" runat="server" OnItemCommand="rptPendingPager_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPage" runat="server" CommandName="Page" CommandArgument='<%# Container.DataItem %>' CssClass="btn btn-outline-primary btn-sm"><%# Container.DataItem %></asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        
            <asp:LinkButton ID="lnkPendingNext" runat="server" OnClick="lnkPendingNext_Click" CssClass="btn btn-outline-primary btn-sm">下一页</asp:LinkButton>
        </div>

        <!-- 已审核留言 -->
        <h4 class="mt-5 mb-3">已审核留言</h4>
        <asp:Repeater ID="rptApprovedComments" runat="server" OnItemCommand="rptApprovedComments_ItemCommand">
            <HeaderTemplate>
                <table class="table table-bordered table-hover">
                    <thead class="table-light">
                        <tr>
                            <th style="width: 5%;">ID</th>
                            <th style="width: 15%;">用户名</th>
                            <th>留言内容</th>
                            <th style="width: 15%;">留言时间</th>
                            <th style="width: 10%;">操作</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Id") %></td>
                    <td><%# Eval("Username") %></td>
                    <td><%# Eval("Content") %></td>
                    <td><%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %></td>
                    <td>
                        <asp:LinkButton ID="btnDeleteApproved" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-danger btn-sm">删除</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <!-- 已审核留言分页 -->
        <div class="d-flex justify-content-center align-items-center gap-2 mt-2">
            <asp:LinkButton ID="lnkApprovedPrev" runat="server" OnClick="lnkApprovedPrev_Click" CssClass="btn btn-outline-primary btn-sm">上一页</asp:LinkButton>
        
            <div class="d-flex flex-wrap gap-1">
                <asp:Repeater ID="rptApprovedPager" runat="server" OnItemCommand="rptApprovedPager_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPage" runat="server" CommandName="Page" CommandArgument='<%# Container.DataItem %>' CssClass="btn btn-outline-primary btn-sm"><%# Container.DataItem %></asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        
            <asp:LinkButton ID="lnkApprovedNext" runat="server" OnClick="lnkApprovedNext_Click" CssClass="btn btn-outline-primary btn-sm">下一页</asp:LinkButton>
        </div>

    </div>
</asp:Content>
