<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dept.aspx.cs" Inherits="SysMgr_Dept" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>部門管理</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
        });
    </script>
</head>
<body class="body">
    <form id="Form1" name="form" runat="server">
    <table width="100%">
        <tr>
            <th align="right" width="15%">部門名稱：</th>
            <td width="20%" align="left">
                <asp:TextBox runat="server" ID="txtDeptName" Width="60mm" CssClass="font9"></asp:TextBox>
             </td>
             <th width="15%" align="right">部門描述：</th>
             <td width="20%" align="left">
                 <asp:TextBox runat="server" ID="txtDeptDesc" Width="60mm" CssClass="font9"></asp:TextBox>
             </td>
             <td width="30%" align="right">
                 <asp:Button ID="btnQuery" class="ui-button ui-corner-all" runat="server" Text="查詢" OnClick="btnQuery_Click"/>
                 <asp:Button ID="btnAdd" class="ui-button ui-corner-all" runat="server" Text="新增" OnClick="btnAdd_Click"/>
             </td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
