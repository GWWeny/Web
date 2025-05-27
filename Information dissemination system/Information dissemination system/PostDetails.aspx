<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostDetails.aspx.cs" Inherits="Information_dissemination_system.PostDetails" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <!-- 文章详情 -->
        <asp:Panel ID="pnlPost" runat="server" Visible="false">
            <div class="mb-4">
                <h2 class="fw-bold"><asp:Label ID="lblTitle" runat="server" /></h2>
                <div class="text-muted small">
                    浏览量：<asp:Label ID="lblViews" runat="server" /> |
                    发布时间：<asp:Label ID="lblCreatedAt" runat="server" />
                </div>
            </div>

            <asp:Image ID="imgCover" runat="server" CssClass="img-fluid rounded shadow-sm mb-4" />

            <div class="mb-5 fs-5" style="white-space: pre-wrap;">
                <asp:Label ID="lblContent" runat="server" />
            </div>
        </asp:Panel>

        <!-- 错误/提示消息 -->
        <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-danger fw-bold d-block" Visible="false" />

        <!-- 留言列表 -->
        <div class="mb-5">
            <h4 class="mb-3 border-bottom pb-2">留言列表</h4>
            <asp:Repeater ID="rptComments" runat="server">
                <ItemTemplate>
                    <div class="card mb-3" style="margin-left:<%# GetIndentLevel((int)Eval("Level")) %>px;">
                        <div class="card-body p-3">
                            <h6 class="card-title mb-1">
                                <strong><%# Eval("Username") %></strong>
                                <%# GetReplyToText(Eval("ReplyToUsername")) %>
                                <small class="text-muted"> 发表于 <%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %></small>
                                &nbsp;
                                <asp:LinkButton ID="btnReply" runat="server" CssClass="btn btn-link btn-sm"
                                    CommandArgument='<%# Eval("Id") %>' OnClick="btnReply_Click">回复</asp:LinkButton>
                            </h6>

                            <asp:PlaceHolder ID="phQuote" runat="server" Visible='<%# Eval("ReplyToContent") != null && Eval("ReplyToContent").ToString() != "" %>'>
                                <blockquote class="blockquote p-2 mb-2 bg-light rounded" style="border-left: 3px solid #0d6efd;">
                                    <%# Eval("ReplyToContent") %>
                                </blockquote>
                            </asp:PlaceHolder>

                            <p class="card-text mb-0"><%# Eval("Content") %></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- 隐藏字段用于存储当前回复的父评论ID -->
        <asp:HiddenField ID="hfParentId" runat="server" Value="" />

        <!-- 留言表单 -->
        <asp:Panel ID="pnlCommentForm" runat="server" Visible="false">
            <div class="card shadow-sm">
                <div class="card-header bg-primary text-white">
                    <asp:Label ID="lblCommentFormTitle" runat="server" Text="发表评论" />
                </div>
                <div class="card-body">
                    <asp:TextBox ID="txtComment" runat="server" CssClass="form-control mb-3" TextMode="MultiLine" Rows="4" placeholder="请输入您的留言..."></asp:TextBox>
                    <asp:Button ID="btnSubmitComment" runat="server" CssClass="btn btn-success" Text="提交留言" OnClick="btnSubmitComment_Click" />
                    <asp:Button ID="btnCancelReply" runat="server" CssClass="btn btn-secondary ms-2" Text="取消回复" OnClick="btnCancelReply_Click" Visible="false" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
