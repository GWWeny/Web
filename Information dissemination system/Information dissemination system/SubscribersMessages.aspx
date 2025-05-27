<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscribersMessages.aspx.cs" Inherits="Information_dissemination_system.SubscribersMessages" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h3 class="mb-4">消息私信</h3>

        <!-- 我说过的留言（主留言和回复） -->
        <h4 class="mb-3">我说过的留言</h4>
        <asp:Repeater ID="rptMyComments" runat="server">
            <ItemTemplate>
                <div class="card mb-3">
                    <div class="card-header">
                        <div class="card-header bg-primary text-white">
                        <strong>帖子标题：</strong><%# Eval("PostTitle") %>
                        <span class="text-muted float-end"><%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %></span>
                    </div>
                    <div class="card-body">
                        <p><%# Eval("Content") %></p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        



        <!-- 分页 -->
        <nav aria-label="分页导航">
            <ul class="pagination justify-content-center">
                <li class="page-item">
                    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Page" CommandArgument="Prev" CssClass="page-link" OnCommand="Page_Command">上一页</asp:LinkButton>
                </li>

                <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand" OnItemDataBound="rptPager_ItemDataBound">
                    <ItemTemplate>
                        <li id="liPage" runat="server" class="page-item">
                            <asp:LinkButton ID="lnkPage" runat="server" CommandName="Page" CommandArgument='<%# Container.DataItem %>' CssClass="page-link">
                                <%# Container.DataItem %>
                            </asp:LinkButton>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>

                <li class="page-item">
                    <asp:LinkButton ID="lnkNext" runat="server" CommandName="Page" CommandArgument="Next" CssClass="page-link" OnCommand="Page_Command">下一页</asp:LinkButton>
                </li>
            </ul>
        </nav>

        <hr class="my-5" />

        <!-- 别人回复我的留言 -->
        <h4 class="mb-3">别人回复我的留言</h4>
        <asp:Repeater ID="rptRepliesToMe" runat="server" OnItemCommand="rptRepliesToMe_ItemCommand">
            <ItemTemplate>
                <div class="card mb-3 border-success shadow-sm">
                    <div class="card-header bg-success text-white">
                        <strong>帖子标题：</strong><%# Eval("PostTitle") %>
                        <span class="text-light float-end"><%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %></span>
                    </div>
                    <div class="card-body">
                        <p><strong><%# Eval("ReplierName") %> 回复:</strong> <%# Eval("Content") %></p>

                        <asp:Panel ID="pnlReplyBox2" runat="server" Visible="false" CssClass="mt-2">
                            <asp:TextBox ID="txtReplyContent2" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control mb-2" Placeholder="回复..."></asp:TextBox>
                            <asp:Button ID="btnSendReply2" runat="server" Text="发送回复" CommandName="ReplyToReply" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-success btn-sm" />
                        </asp:Panel>

                        <asp:LinkButton ID="lnkReply2" runat="server" CommandName="ShowReply2" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-outline-success btn-sm">回复</asp:LinkButton>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <hr class="my-5" />

        <!-- 我回复别人的留言 -->
        <h4 class="mb-3">我回复别人的留言</h4>
        <asp:Repeater ID="rptMyReplies" runat="server">
            <ItemTemplate>
                <div class="card mb-3 border-info shadow-sm">
                    <div class="card-header bg-info text-white">
                        <strong>帖子标题：</strong><%# Eval("PostTitle") %>
                        <span class="text-light float-end"><%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %></span>
                    </div>
                    <div class="card-body">
                        <p><strong>我回复 <span class="text-dark"><%# Eval("OriginalCommenterName") %></span>：</strong> <%# Eval("Content") %></p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
