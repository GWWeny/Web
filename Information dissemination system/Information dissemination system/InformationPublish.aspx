<%@ Page Title="信息发布" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="InformationPublish.aspx.cs" Inherits="Information_dissemination_system.InformationPublish" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h2 class="mb-4 text-danger">发布新信息</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="text-success mb-3 d-block"></asp:Label>

        <div class="mb-3">
            <label class="form-label">标题</label>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
        </div>

        <div class="mb-3">
            <label class="form-label">内容</label>
            <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="8" CssClass="form-control" />
        </div>

        <div class="mb-3">
            <label for="fuCoverImage" class="form-label">封面图片：</label><br />
            <asp:FileUpload ID="fuCoverImage" runat="server" CssClass="form-control" />
            <asp:Image ID="imgPreview" runat="server" Width="200" CssClass="mt-2 rounded" Style="display:none;" />
        </div>

        <div class="mb-3">
            <label class="form-label">所属栏目</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" />
        </div>

        <div class="form-check mb-3">
            <asp:CheckBox ID="chkIsTop" runat="server" CssClass="form-check-input" />
            <label class="form-check-label">置顶</label>
        </div>

        <div class="form-check mb-4">
            <asp:CheckBox ID="chkIsPublished" runat="server" Checked="true" CssClass="form-check-input" />
            <label class="form-check-label">是否发布</label>
        </div>

        <asp:Button ID="btnPublish" runat="server" Text="发布信息" CssClass="btn btn-danger me-2" OnClick="btnPublish_Click" />
        <a href="Home.aspx" class="btn btn-danger me-2">返回首页</a>
    </div>

    <!-- 图片预览脚本 -->
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            const fileInput = document.querySelector('[id$="fuCoverImage"]');
            const previewImg = document.querySelector('[id$="imgPreview"]');

            if (!fileInput || !previewImg) return;

            fileInput.addEventListener('change', function () {
                const file = fileInput.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        previewImg.src = e.target.result;
                        previewImg.style.display = 'block';  // 显示图片
                    };
                    reader.readAsDataURL(file);
                } else {
                    previewImg.style.display = 'none';  // 无文件时隐藏图片
                    previewImg.src = '';
                }
            });
        });
    </script>
</asp:Content>
