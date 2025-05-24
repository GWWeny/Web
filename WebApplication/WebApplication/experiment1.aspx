<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="experiment1.aspx.cs" Inherits="WebApplication.experiment1" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>综合管理系统</title>
    <!-- 引入 Bootstrap 样式 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        /* 全局样式 */
        body {
            font-family: "Microsoft YaHei", Arial, sans-serif;
            margin: 0;
            padding: 0;
            background: #f5f5f5;
        }

        /* 布局容器 */
        .main-container {
            display: grid;
            grid-template-columns: 1fr 300px;
            gap: 30px;
            max-width: 1200px;
            margin: 0 auto;
        }

        /* 表格通用样式 */
        .data-grid {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            overflow: hidden;
        }

        /* 操作列专用样式 */
        .operation-cell {
            width: 140px;
            min-width: 140px;
            text-align: center !important;
            position: relative;
        }

        .operation-links {
            display: flex;
            justify-content: space-around;
            align-items: center;
            padding: 0 10px;
        }

        .operation-links a {
            color: #1890ff;
            text-decoration: none;
            font-size: 14px;
            padding: 2px 5px;
            transition: all 0.3s;
            white-space: nowrap;
        }

        .operation-links a:hover {
            color: #096dd9;
            background: #e6f7ff;
            border-radius: 4px;
        }

        /* 表格头部样式 */
        .data-grid th {
            background: #fafafa;
            font-weight: 600;
            padding: 16px;
            border-bottom: 2px solid #e8e8e8;
        }

        .data-grid td {
            padding: 12px 16px;
            border-bottom: 1px solid #e8e8e8;
        }

        /* 按钮样式 */
        .add-button {
            background: #1890ff;
            color: white !important;
            padding: 8px 20px;
            border-radius: 4px;
            margin-top: 15px;
            display: inline-block;
            text-decoration: none !important;
        }

        .add-button:hover {
            background: #096dd9;
        }

        /* 实验说明区域 */
        .experiment-desc {
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }

        /* 移动端适配 */
        @media (max-width: 768px) {
            .main-container {
                grid-template-columns: 1fr;
            }

            .operation-cell {
                width: 100px;
                min-width: 100px;
            }

            .operation-links {
                flex-direction: column;
                gap: 5px;
            }
        }
    </style>
</head>
<body class="bg-light">

    <!-- 导航条 -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
        <div class="container-fluid">
       
            
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav">
                    
                    <li class="nav-item">
                        <a class="nav-link" href="experiment1.aspx">实验一</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="experiment4.aspx">实验四</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="experiment5.aspx">实验五</a>
                    </li>
                    
                </ul>
            </div>
        </div>
    </nav>

    <form id="form1" runat="server">
        <div class="main-container">
            <div>
                <!-- 顾客管理 -->
                <div class="data-grid">
                    <h2 style="padding: 16px 20px; margin:0;">顾客管理</h2>
                    <asp:GridView ID="GridViewCustom" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="Id" CssClass="data-grid" HeaderStyle-CssClass="grid-header"
                        OnRowDeleting="GridViewCustom_RowDeleting" 
                        OnRowEditing="GridViewCustom_RowEditing"
                        OnRowUpdating="GridViewCustom_RowUpdating" 
                        OnRowCancelingEdit="GridViewCustom_RowCancelingEdit">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="编号" ReadOnly="True" />
                            <asp:BoundField DataField="Cname" HeaderText="姓名" />
                            <asp:BoundField DataField="DepartID" HeaderText="部门ID" />
                            <asp:BoundField DataField="Age" HeaderText="年龄" />
                            <asp:BoundField DataField="Ename" HeaderText="邮箱" />
                            <asp:BoundField DataField="Password" HeaderText="密码" />
                            <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="operation-cell">
                                <ItemTemplate>
                                    <div class="operation-links">
                                        <asp:LinkButton runat="server" CommandName="Edit" Text="编辑" />
                                        <asp:LinkButton runat="server" CommandName="Delete" Text="删除" 
                                            OnClientClick="return confirm('确定要删除该顾客吗？');" />
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="operation-links">
                                        <asp:LinkButton runat="server" CommandName="Update" Text="保存" />
                                        <asp:LinkButton runat="server" CommandName="Cancel" Text="取消" />
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="btnAddCustom" runat="server" Text="添加顾客" 
                        CssClass="add-button" OnClick="btnAddCustom_Click" />
                </div>

                <!-- 部门管理 -->
                <div class="data-grid" style="margin-top: 30px;">
                    <h2 style="padding: 16px 20px; margin:0;">部门管理</h2>
                    <asp:GridView ID="GridViewDepartment" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="Id" CssClass="data-grid"
                        OnRowDeleting="GridViewDepartment_RowDeleting" 
                        OnRowEditing="GridViewDepartment_RowEditing"
                        OnRowUpdating="GridViewDepartment_RowUpdating" 
                        OnRowCancelingEdit="GridViewDepartment_RowCancelingEdit">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" />
                            <asp:BoundField DataField="Departname" HeaderText="部门名称" />
                            <asp:BoundField DataField="Description" HeaderText="描述" />
                            <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="operation-cell">
                                <ItemTemplate>
                                    <div class="operation-links">
                                        <asp:LinkButton runat="server" CommandName="Edit" Text="编辑" />
                                        <asp:LinkButton runat="server" CommandName="Delete" Text="删除" 
                                            OnClientClick="return confirm('确定要删除该部门吗？');" />
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="operation-links">
                                        <asp:LinkButton runat="server" CommandName="Update" Text="保存" />
                                        <asp:LinkButton runat="server" CommandName="Cancel" Text="取消" />
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="btnAddDepartment" runat="server" Text="添加部门" 
                        CssClass="add-button" OnClick="btnAddDepartment_Click" />
                </div>
            </div>

            <!-- 实验说明 -->
            <div class="experiment-desc">
                <h2 style="color: #1890ff; margin-top:0;">实验一说明</h2>
                <p style="line-height: 1.6;">
                    设计一个基于 Web 的应用程序，在程序中嵌入 SQL 代码实现对 custom、department 表中的记录进行：插入、修改、删除、查询的操作 
                </p>

              <!--  <asp:Button ID="btnReturn" runat="server" Text="返回导航" CssClass="add-button" OnClick="btnReturn_Click" />  -->
            </div>
        </div>
    </form>

    <!-- 引入 Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
