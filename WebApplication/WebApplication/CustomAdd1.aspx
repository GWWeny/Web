<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomAdd1.aspx.cs" Inherits="WebApplication.CustomAdd1" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>添加用户</title>
    <style>
        body {
            font-family: 'Microsoft YaHei', sans-serif;
            background: #f0f2f5;
            margin: 0;
            padding: 20px;
        }

        .form-container {
            max-width: 600px;
            margin: 40px auto;
            background: white;
            padding: 24px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }

        h2 {
            color: #2c3e50;
            margin-bottom: 24px;
            font-weight: 500;
            border-bottom: 1px solid #e8e8e8;
            padding-bottom: 12px;
        }

        .form-row {
            display: flex;
            align-items: center;
            margin-bottom: 16px;
        }

        label {
            width: 100px;
            color: #5a5a5a;
            font-size: 14px;
            margin-right: 16px;
            text-align: right;
        }

        .text-box {
            flex: 1;
            padding: 8px 12px;
            border: 1px solid #d9d9d9;
            border-radius: 4px;
            font-size: 14px;
            transition: border-color 0.3s;
        }

        .text-box:focus {
            border-color: #1890ff;
            outline: none;
            box-shadow: 0 0 0 2px rgba(24,144,255,0.2);
        }

        .submit-btn {
            background: #1890ff;
            color: white;
            padding: 8px 24px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            transition: background 0.3s;
            margin-left: 116px;
        }

        .submit-btn:hover {
            background: #096dd9;
        }
        /* 新增下拉列表样式 */
        .dropdown-list {
            width: 100%;
            padding: 8px 12px;
            border: 1px solid #d9d9d9;
            border-radius: 4px;
            background: white url('data:image/svg+xml;utf8,<svg viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg"><path d="M512 714.016L128 330.016 202.016 256 512 565.984 821.984 256 896 330.016z"/></svg>') no-repeat right 8px center;
            background-size: 12px;
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>添加用户</h2>
            

            <div class="form-row">
                <label for="txtCname">姓名:</label>
                <asp:TextBox ID="txtCname" runat="server" CssClass="text-box" />
            </div>

            <!-- 修改部门选择为下拉列表 -->
            <div class="form-row">
                <label for="txtDepartID">部门ID:</label>
                <asp:DropDownList ID="ddlDepart" runat="server" 
                    CssClass="dropdown-list" DataTextField="Id" 
                    DataValueField="Id" AppendDataBoundItems="true">
                    <asp:ListItem Value="" Selected="True">-- 请选择部门ID --</asp:ListItem>
                </asp:DropDownList>
            </div>

            

            <div class="form-row">
                <label for="txtAge">年龄:</label>
                <asp:TextBox ID="txtAge" runat="server" CssClass="text-box" TextMode="Number" />
            </div>

            <div class="form-row">
                <label for="txtEname">邮箱:</label>
                <asp:TextBox ID="txtEname" runat="server" CssClass="text-box" TextMode="Email" />
            </div>

            <div class="form-row">
                <label for="txtPassword">密码:</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="text-box" TextMode="Password" />
            </div>

            <asp:Button ID="btnAdd" runat="server" Text="添加" CssClass="submit-btn" OnClick="btnAdd_Click" />
        </div>
    </form>
</body>
</html>