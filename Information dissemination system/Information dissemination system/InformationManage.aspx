<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformationManage.aspx.cs" Inherits="Information_dissemination_system.InformationManage" MasterPageFile="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>信息管理</h2>

      <!-- 搜索栏 -->
    <div class="input-group mb-3" style="max-width: 400px;">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="请输入标题或栏目关键字" />
        <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
        <asp:Button ID="btnReset" runat="server" Text="回退" CssClass="btn btn-secondary ms-2" OnClick="btnReset_Click" />
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="mb-3"></asp:Label>

    <asp:GridView ID="gvPosts" runat="server" CssClass="table table-bordered table-hover"
        AutoGenerateColumns="false" DataKeyNames="Id"
        OnPageIndexChanging="gvPosts_PageIndexChanging"
        AllowPaging="true" PageSize="7"
        OnRowEditing="gvPosts_RowEditing"
        OnRowCancelingEdit="gvPosts_RowCancelingEdit"
        OnRowUpdating="gvPosts_RowUpdating"
        OnRowDeleting="gvPosts_RowDeleting"
        OnRowDataBound="gvPosts_RowDataBound">

        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true" />

            <asp:TemplateField HeaderText="标题">
                <ItemTemplate>
                    <%# Eval("Title") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtTitle" runat="server" Text='<%# Bind("Title") %>' CssClass="form-control" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="内容摘要">
                <ItemTemplate>
                    <%# Eval("ShortContent") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtContent" runat="server" Text='<%# Bind("Content") %>' CssClass="form-control" TextMode="MultiLine" Rows="3" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="封面图片" ItemStyle-Width="120px">
                <ItemTemplate>
                    <asp:Image ID="imgCover" runat="server" Width="100px" 
                        ImageUrl='<%# string.IsNullOrEmpty(Eval("CoverImage")?.ToString()) ? ResolveUrl("~/images/default-cover.png") : ResolveUrl("~/" + Eval("CoverImage")) %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:FileUpload ID="fuCoverImage" runat="server" CssClass="form-control-file" />
                    <asp:HiddenField ID="hfCoverImage" runat="server" Value='<%# Eval("CoverImage") %>' />
                    <br />
                    <asp:Image ID="imgCoverEdit" runat="server" Width="100px"
                        ImageUrl='<%# string.IsNullOrEmpty(Eval("CoverImage")?.ToString()) ? ResolveUrl("~/images/default-cover.png") : ResolveUrl("~/" + Eval("CoverImage")) %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="栏目">
                <ItemTemplate>
                    <%# Eval("CategoryName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="置顶" ItemStyle-Width="70px">
                <ItemTemplate>
                    <asp:CheckBox ID="chkIsTop" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsTop")) %>' Enabled="false" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox ID="chkIsTopEdit" runat="server" Checked='<%# Bind("IsTop") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="发布" ItemStyle-Width="70px">
                <ItemTemplate>
                    <asp:CheckBox ID="chkIsPublished" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsPublished")) %>' Enabled="false" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox ID="chkIsPublishedEdit" runat="server" Checked='<%# Bind("IsPublished") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="CreatedAt" HeaderText="发布时间" DataFormatString="{0:yyyy-MM-dd HH:mm}" ReadOnly="true" />

            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>

    <br />
    <a href="Home.aspx" class="btn btn-danger me-2">返回首页</a>

</asp:Content>
