<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SRTPersonnelQuery.aspx.cs" Inherits="View_SRTPersonnelQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>字幕人員維護</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
</head>
<body style="overflow:auto;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1">
            <tr>
                <td>
                    人員姓名：<asp:TextBox ID="tbxSRTName" runat="server"></asp:TextBox>
                    <asp:Button ID="btnQuery" runat="server" class="ui-button ui-corner-all" Width="15mm" Text="查詢" OnClick="btnQuery_Click"/>
                </td>
                <td align="right">
                    <asp:Button ID="btnAdd" runat="server" class="ui-button ui-corner-all" Width="15mm" Text="新增" onclick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="9">
                    <table class='table-list' width='100%'>
                        <tr><th noWrap width='80px'><span>字幕人員代號</span></th>
                            <th noWrap width='140px'><span>字幕人員姓名</span></th>
                            <th noWrap width='80px'><span>字幕人員部門</span></th>
                            <th noWrap width='140px'><span>連絡電話</span></th>
                            <th noWrap width='100px'><span>手機號碼</span></th>
                            <th noWrap width='350px'><span>戶籍地址</span></th>
                            <th noWrap width='200px'><span>電子郵件一</span></th>
                            <th noWrap width='170px'><span>電子郵件二</span></th>
                            <th noWrap width='30px'><span>評比</span></th>
                        </tr>
                        <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
