<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="experiment5.aspx.cs" Inherits="WebApplication.experiment5" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实验五 - 产品价格查询</title>

    <!-- Bootstrap 样式 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <style>
        body {
            padding-top: 70px;
            background-color: #f8f9fa;
        }

        .form-box {
            max-width: 600px;
            margin: 30px auto;
            background: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        }

        .result-label {
            font-size: 1.2rem;
            font-weight: bold;
            color: #0d6efd;
        }

        .description-box {
            max-width: 900px;
            margin: 0 auto 30px auto;
            background-color: #fff;
            padding: 20px 30px;
            border-left: 6px solid #0d6efd;
            border-radius: 6px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnQuery").click(function () {
                var productName = $("#<%= txtProductName.ClientID %>").val();
                $.ajax({
                    type: "POST",
                    url: "experiment5.aspx/GetProductInfo",
                    data: JSON.stringify({ productName: productName }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $("#<%= result.ClientID %>").html("￥" + response.d);
                    },
                    error: function () {
                        $("#<%= result.ClientID %>").text("查询失败");
                    }
                });
            });
        });
    </script>
</head>
<body>
    <!-- 导航栏 -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary fixed-top">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">综合管理系统</a>
            <div class="collapse navbar-collapse">
                <ul class="navbar-nav">
                    <li class="nav-item"><a class="nav-link" href="experiment1.aspx">实验一</a></li>
                    <li class="nav-item"><a class="nav-link" href="experiment4.aspx">实验四</a></li>
                    <li class="nav-item"><a class="nav-link active" href="experiment5.aspx">实验五</a></li>
                </ul>
            </div>
        </div>
    </nav>

    <form id="form1" runat="server">

        <!-- 实验说明 -->
        <div class="description-box">
            <h5 class="text-primary fw-bold">实验说明</h5>
            <p>
                编写一个页面程序，应用 <strong>Ajax 技术</strong> 实现如下图所示的产品价格查询功能。
                用户输入产品名称后，点击查询按钮，页面无需刷新即可返回该产品的价格。
            </p>
        </div>

        <!-- 查询区域 -->
        <div class="form-box">
            <h4 class="mb-4 text-center text-primary">产品价格查询</h4>

            <div class="mb-3">
                <label for="txtProductName" class="form-label">产品名称</label>
                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="请输入产品名称"></asp:TextBox>
            </div>

            <div class="mb-3">
                <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn btn-primary w-100" OnClientClick="return false;" />
            </div>

            <div class="mb-3">
                <label class="form-label">价格</label><br />
                <asp:Label ID="result" runat="server" CssClass="result-label"></asp:Label>
            </div>
        </div>
    </form>

    <!-- Bootstrap 脚本 -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
