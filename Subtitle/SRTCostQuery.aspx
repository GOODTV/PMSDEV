<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SRTCostQuery.aspx.cs" Inherits="View_SRTCostQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>字幕成本維護</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "tbxCostEffectiveDate",   // id of the input field
                button: "imgCostEffectiveDate"     // 與觸發動作的物件ID相同
            });
        }
    </script>
</head>
<body style="overflow:auto;">
    <form id="Form1" runat="server">
    <div>
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1">
            <tr>
                <td>
                    成本生效日期<asp:TextBox ID="tbxCostEffectiveDate" runat="server" onchange="CheckDateFormat(this, '成本生效日期');"></asp:TextBox>
                    <img id="imgCostEffectiveDate" alt="" src="../img/date.gif" />
                    製作類別<asp:dropdownlist runat="server" ID="ddlMake_Category" CssClass="font9"></asp:dropdownlist>
                    <asp:Button ID="btnQuery" runat="server" class="ui-button ui-corner-all"  Width="15mm" Text="查詢" OnClick="btnQuery_Click"/>
                </td>
                <td align="right">
                    <asp:Button ID="btnAdd" runat="server" class="ui-button ui-corner-all" Width="15mm" Text="新增" onclick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="9">
                    <table class='table-list' width='100%'>
                        <tr><th noWrap><span>成本生效日期</span></th>
                            <th noWrap><span>製作類別</span></th>
                            <th noWrap><span>標準長度</span></th>
                            <th noWrap><span>標準金額</span></th>
                            <th noWrap><span>說明</span></th>
                            <th noWrap><span>啟用</span></th>
                        </tr>
                        <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
