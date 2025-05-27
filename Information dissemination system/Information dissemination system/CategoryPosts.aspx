<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CategoryPosts.aspx.cs" Inherits="Information_dissemination_system.CategoryPosts" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
       

        <!-- 文章列表 -->
        <asp:Repeater ID="rptPosts" runat="server">
            <ItemTemplate>
                <div class="card mb-4 shadow-sm">
                    <div class="row g-0">
                        <div class="col-md-4">
                             <a href='PostDetails.aspx?id=<%# Eval("Id") %>'>
                                <img src='<%# Eval("CoverImage") ?? "images/default-cover.jpg" %>' class="img-fluid h-100 rounded-start object-fit-cover" alt="封面图" />
                             </a>
                        </div>
                        <div class="col-md-8">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <a href='PostDetails.aspx?id=<%# Eval("Id") %>' class="text-decoration-none text-dark">
                                        <%# Eval("Title") %>
                                    </a>
                                </h5>
                                <p class="card-text text-truncate"><%# Eval("Content") %></p>
                                <p class="card-text">
                                    <small class="text-muted">浏览量：<%# Eval("Views") %> | 发布时间：<%# Eval("CreatedAt", "{0:yyyy-MM-dd}") %></small>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <!-- 无数据提示 -->
        <asp:Label ID="lblEmpty" runat="server" CssClass="text-muted" Visible="false" />

        <!-- 自定义分页 -->
        <div class="text-center mt-4">
            <asp:Button ID="btnPrev" runat="server" Text="上一页" CssClass="btn btn-outline-primary me-2" OnClick="btnPrev_Click" />
            <asp:Label ID="lblPageInfo" runat="server" CssClass="me-2 fw-bold" />
            <asp:Button ID="btnNext" runat="server" Text="下一页" CssClass="btn btn-outline-primary" OnClick="btnNext_Click" />
        </div>
    </div>
</asp:Content>
