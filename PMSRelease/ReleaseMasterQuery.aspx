<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReleaseMasterQuery.aspx.cs" Inherits="View_ReleaseMasterQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>交檔客戶管理</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
</head>
<body style="overflow:auto;">
    <form id="form1" runat="server" defaultbutton="btnQuery">
    <div>
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1">
            <tr>
                <td align="left">
                    查詢：<asp:TextBox ID="tbxCustomerName" CssClass="font9" runat="server" Width="250px" />　資料筆數：<asp:Label ID="count" runat="server"></asp:Label>
                </td>
                <td align="right">
                    <asp:Button ID="btnQuery" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="查詢" OnClick="btnQuery_Click"/>
                    <asp:Button ID="btnAdd" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="新增" onclick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="2">
                    <table class='table-list' width='100%'>
                        <TR>
                            <TH noWrap><SPAN>客戶代號</SPAN></TH>
                            <TH noWrap><SPAN>客戶名稱</SPAN></TH>
                            <TH noWrap><SPAN>聯絡人姓名1</SPAN></TH>
                            <TH noWrap><SPAN>聯絡人電話1</SPAN></TH>
                            <TH noWrap><SPAN>聯絡人手機1</SPAN></TH>
                            <TH noWrap><SPAN>聯絡人電郵1</SPAN></TH>
                            <TH noWrap><SPAN>聯絡人姓名2</SPAN></TH>
                            <TH noWrap><SPAN>聯絡人電話2</SPAN></TH>
                            <TH noWrap><SPAN>聯絡人手機2</SPAN></TH>
                            <TH noWrap><SPAN>聯絡人電郵2</SPAN></TH>
                            <TH noWrap><SPAN>供片間隔</SPAN></TH>
                        </TR>
                     <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
