<%@ Page Title="搜索结果" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchResults.aspx.cs" Inherits="Information_dissemination_system.SearchResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h3>搜索结果：<small class="text-muted"><%# Server.HtmlEncode(Request.QueryString["q"] ?? "") %></small></h3>

        <div class="row g-3">
            <asp:Repeater ID="rptSearchResults" runat="server">
                <ItemTemplate>
                    <div class="col-md-4">
                        <div class="card h-100 shadow-sm">
                            <a href='PostDetails.aspx?id=<%# Eval("Id") %>'>
                            <img src='<%# Eval("CoverImage") ?? "images/default-cover.jpg" %>' class="card-img-top" style="height:180px; object-fit:cover;" />
                                </a>
                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title">
                                    <a href='PostDetails.aspx?id=<%# Eval("Id") %>'><%# Eval("Title") %></a>
                                </h5>
                                <p class="card-text text-truncate flex-grow-1"><%# Eval("Content") %></p>
                                <a href='PostDetails.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-outline-primary mt-2 align-self-start">查看详情</a>
                            </div>
                            <div class="card-footer text-muted small">
                                栏目：<%# Eval("CategoryName") %> &nbsp;&nbsp; 浏览量：<%# Eval("Views") %>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- 分页导航 -->
        <nav aria-label="分页导航" class="mt-4">
            <ul class="pagination justify-content-center">
                <li class="page-item <%# CurrentPage <= 1 ? "disabled" : "" %>">
                    <a class="page-link" href="?q=<%= Server.UrlEncode(SearchQuery) %>&page=<%= CurrentPage - 1 %>">上一页</a>
                </li>
                <% for (int i = 1; i <= TotalPages; i++) { %>
                    <li class="page-item <%= (i == CurrentPage) ? "active" : "" %>">
                        <a class="page-link" href="?q=<%= Server.UrlEncode(SearchQuery) %>&page=<%= i %>"><%= i %></a>
                    </li>
                <% } %>
                <li class="page-item <%# CurrentPage >= TotalPages ? "disabled" : "" %>">
                    <a class="page-link" href="?q=<%= Server.UrlEncode(SearchQuery) %>&page=<%= CurrentPage + 1 %>">下一页</a>
                </li>
            </ul>
        </nav>

    </div>
</asp:Content>
