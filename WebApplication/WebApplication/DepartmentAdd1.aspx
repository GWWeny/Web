<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DepartmentAdd1.aspx.cs" Inherits="WebApplication.DepartmentAdd1" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>添加部门</title>
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

        /* 针对多行文本框的特殊样式 */
        textarea.text-box {
            min-height: 80px;
            resize: vertical;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>添加部门</h2>
            
            <div class="form-row">
                <label for="txtDepartname">部门名称:</label>
                <asp:TextBox ID="txtDepartname" runat="server" CssClass="text-box" />
            </div>

            <div class="form-row">
                <label for="txtDescription">描述:</label>
                <asp:TextBox ID="txtDescription" runat="server" 
                    CssClass="text-box" TextMode="MultiLine" Rows="3" />
            </div>

            <asp:Button ID="btnAdd" runat="server" Text="添加" CssClass="submit-btn" OnClick="btnAdd_Click" />
        </div>
    </form>
</body>
</html>