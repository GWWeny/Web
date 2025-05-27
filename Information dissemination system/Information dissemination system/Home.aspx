<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Information_dissemination_system.Home" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="UTF-8" />
    <title>信息发布系统</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            font-family: 'Microsoft Yahei', sans-serif;
        }
        .header-banner {
            background: url('banner.jpg') center center / cover no-repeat;
            height: 400px;
            color: white;
            position: relative;
            text-align: center;
        }
        .header-banner h1 {
            font-size: 48px;
            font-weight: bold;
            position: absolute;
            top: 30%;
            left: 50%;
            transform: translate(-50%, -50%);
        }
        .search-box {
            position: absolute;
            bottom: 60px;
            left: 50%;
            transform: translateX(-50%);
            width: 60%;
        }
        .news-card img {
            object-fit: cover;
            height: 180px;
        }
        .navbar-brand {
            font-weight: bold;
            color: #f40 !important;
            font-size: 22px;
        }
        .nav-link.active {
            border-bottom: 2px solid red;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <!-- 顶部导航栏 -->
    <nav class="navbar navbar-expand-lg navbar-light bg-white shadow-sm sticky-top">
        <div class="container">
            <a class="navbar-brand" href="#">信息发布系统</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarContent">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarContent">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0" id="navCategories" runat="server" ClientIDMode="Static">
                    <asp:Repeater ID="rptCategories" runat="server">
                        <ItemTemplate>
                            <li class="nav-item">
                                <a class="nav-link" href='CategoryPosts.aspx?categoryId=<%# Eval("Id") %>' title='<%# Eval("Description") %>'>
                                    <%# Eval("Name") %>
                                </a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>

                    <!-- 更多下拉菜单 -->
                    <li class="nav-item dropdown" id="moreMenu" runat="server" ClientIDMode="Static" style="display:none;">
                        <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">更多</a>
                        <ul class="dropdown-menu" id="moreDropdown" runat="server" ClientIDMode="Static"></ul>
                    </li>
                </ul>

                <form class="d-flex me-2" method="get" action="SearchResults.aspx">
                    <input class="form-control" type="search" name="q" placeholder="搜索" />
                    <button class="btn btn-outline-danger ms-2" type="submit">
                        <i class="bi bi-search"></i>
                    </button>
                </form>

                <!-- 登录按钮 (未登录时显示) -->
                <asp:Panel ID="pnlLogin" runat="server">
                    <a href="Login.aspx" class="btn btn-danger">登录</a>
                </asp:Panel>

                <!-- 用户信息 (已登录时显示) -->
<asp:Panel ID="pnlUserInfo" runat="server" Visible="false">
    <div class="dropdown">
        <a class="btn dropdown-toggle d-flex align-items-center" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
            <asp:Image ID="imgAvatar" runat="server" CssClass="rounded-circle me-2" Width="32" Height="32" AlternateText="头像" />
            <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>
        </a>
        <ul class="dropdown-menu dropdown-menu-end">
            <li><a class="dropdown-item" href="UserProfile.aspx">个人信息</a></li>

            <li>
                <a class="dropdown-item position-relative" href="SubscribersMessages.aspx">
                    消息私信
                    <span id="msgDot" runat="server" class="position-absolute top-0 start-100 translate-middle p-1 bg-danger border border-light rounded-circle d-none">
                        <span class="visually-hidden">新消息</span>
                    </span>
                </a>
            </li>
            
            <asp:Panel ID="pnlAdminMenu" runat="server" Visible="false">
                <li><a class="dropdown-item" href="ColumnManagement.aspx">栏目管理</a></li>
                <li><a class="dropdown-item" href="InformationPublish.aspx">信息发布</a></li>
                <li><a class="dropdown-item" href="InformationManage.aspx">信息管理</a></li>
                <li><a class="dropdown-item" href="AdminComments.aspx">留言管理</a></li>
            </asp:Panel>
            
            <li><a class="dropdown-item" href="Logout.aspx">退出登录</a></li>
        </ul>
    </div>
</asp:Panel>
            </div>
        </div>
    </nav>

    <!-- 顶部横幅 -->
    <div class="header-banner position-relative text-white d-flex flex-column justify-content-center align-items-center" style="height: 400px; overflow: hidden;">
        <!-- 背景图 + 模糊效果 -->
        <img src="images/111.png" alt="横幅图" style="position: absolute; top: 0; left: 0; width: 100%; height: 100%; object-fit: cover; filter: blur(2px) brightness(0.85);" />

        <!-- 内容层，置于图片上方 -->
        <div class="position-relative text-center" style="z-index: 2; max-width: 800px; width: 100%; padding: 0 20px;">
            <form id="searchForm" runat="server" onsubmit="return onSearchSubmit();">
                <div class="input-group input-group-lg">
                    <input id="txtSearch" name="q" type="text" class="form-control" placeholder="搜索你感兴趣的新闻" />
                    <button type="submit" class="btn btn-danger">
                        <i class="bi bi-search"></i>
                    </button>
                </div>
            </form>
        </div>
    </div>

    <script>
        function onSearchSubmit() {
            var input = document.getElementById('txtSearch');
            var query = input.value.trim();
            if (!query) {
                alert('请输入搜索关键词');
                input.focus();
                return false;
            }
            window.location.href = 'SearchResults.aspx?q=' + encodeURIComponent(query);
            return false;
        }
    </script>

    <!-- 内容区域 -->
    <div class="row">
        <asp:Repeater ID="rptHomePosts" runat="server">
            <ItemTemplate>
                <div class="col-md-4 mb-4">
                    <div class="card news-card h-100">
                        <a href='PostDetails.aspx?id=<%# Eval("Id") %>'>
                            <img src='<%# Eval("CoverImage") ?? "images/default-cover.jpg" %>' class="card-img-top" alt="封面图" />
                        </a>
                        <div class="card-body">
                            <h5 class="card-title">
                                <a href='PostDetails.aspx?id=<%# Eval("Id") %>'><%# Eval("Title") %></a>
                            </h5>
                            <p class="card-text text-truncate"><%# Eval("Content") %></p>
                            <a href='PostDetails.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-outline-primary">查看详情</a>
                        </div>
                        <div class="card-footer text-muted small">
                            栏目：<%# Eval("CategoryName") %> &nbsp;&nbsp; 浏览量：<%# Eval("Views") %>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <!-- 可选图标库 -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script>
        window.onload = function () {
            var nav = document.getElementById('navCategories');
            var moreMenu = document.getElementById('moreMenu');
            var moreDropdown = document.getElementById('moreDropdown');

            if (!nav || !moreMenu || !moreDropdown) return;

            var items = nav.querySelectorAll('li.nav-item:not(#moreMenu)');

            // 从第8个开始（索引7），移动到更多菜单中
            for (var i = 7; i < items.length; i++) {
                moreDropdown.appendChild(items[i]);
            }

            if (items.length > 7) {
                moreMenu.style.display = 'block';
            } else {
                moreMenu.style.display = 'none';
            }
        };
    </script>

</body>
</html>
