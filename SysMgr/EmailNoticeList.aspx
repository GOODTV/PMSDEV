<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailNoticeList.aspx.cs" Inherits="SysMgr_EmailNoticeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>通知名單設定</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
</head>
<body class="body">
    <form id="Form1" name="form" runat="server">
    <div id="container">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr>
                <th align="right" width="15%">通知人：</th>
                <td width="20%" align="left">
                    <asp:TextBox runat="server" ID="txtEMailName" Width="60mm" CssClass="font9"></asp:TextBox>
                 </td>
                 <th width="15%" align="right">通知人Email：</th>
                 <td width="20%" align="left">
                     <asp:TextBox runat="server" ID="txtEMailAddress" Width="60mm" CssClass="font9"></asp:TextBox>
                 </td>
                 <td width="30%" align="right">
                     <asp:Button ID="btnQuery" class="ui-button ui-corner-all" runat="server" Text="查詢" OnClick="btnQuery_Click"/>
                     <asp:Button ID="btnAdd" class="ui-button ui-corner-all" runat="server" Text="新增" OnClick="btnAdd_Click"/>
                 </td>
            </tr>
            <tr>
                <td width="100%" colspan="5">
                    <br/>
                    <asp:Label ID="Title" runat="server" Text="「通知類別」欄位為「節目異動申請通知審核」，必須輸入「同仁編號」。" ForeColor="Red" Font-Size="Large"></asp:Label>
                    <br/>
                    <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
