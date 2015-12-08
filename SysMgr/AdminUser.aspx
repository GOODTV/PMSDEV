<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminUser.aspx.cs" Inherits="SysMgr_AdminUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>使用者設定</title>
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
    <form id="Form1" runat="server">
    <table width="100%" >
        <tr>
            <th width="10%" align="right">
                帳號：
            </th>
            <td width="10%" align="left">
                <asp:TextBox runat="server" ID="txtUserID" Width="20mm"></asp:TextBox>
            </td>
            <th width="14%" align="right">
                使用者名稱：
            </th>
            <td width="10%" align="left">
                <asp:TextBox runat="server" ID="txtUserName" Width="20mm"></asp:TextBox>
            </td>
            <th width="12%" align="right">
                部門名稱：
            </th>
            <td width="10%" align="left">
                <asp:DropDownList ID="ddlDept" runat="server">
                </asp:DropDownList>
            </td>
            <th width="14%" align="right">
                使用者群組：
            </th>
            <td width="10%" align="left">
                <asp:DropDownList ID="ddlGroup" DataTextField="GroupName" DataValueField="uid" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="9" align="right">
                 <asp:Button ID="btnQuery" class="ui-button ui-corner-all" runat="server" Text="查詢" OnClick="btnQuery_Click"/>
                 <asp:Button ID="btnAdd" class="ui-button ui-corner-all" runat="server" Text="新增" OnClick="btnAdd_Click"/>
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="9">
                <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
