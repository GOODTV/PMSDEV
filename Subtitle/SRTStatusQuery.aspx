<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SRTStatusQuery.aspx.cs" Inherits="View_SRTStatusQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>字幕狀態查詢</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript">
        function CheckFieldMustFillBasic() {
            var strRet = "";
            var tbxProgramCode = document.getElementById('tbxProgramCode');
            var tbxEpNo_Start = document.getElementById('tbxEpNo_Start');
            var tbxEpNo_End = document.getElementById('tbxEpNo_End');
            if (tbxProgramCode.value == "" && tbxEpNo_Start.value == "" && tbxEpNo_End.value == "") {
                strRet += "請輸入查詢參數";
                alert(strRet);
                return false;
            }
            if (isNaN(Number(tbxEpNo_Start.value)) == true ) {
                alert('集數只能輸入數字');
                tbxEpNo_Start.focus();
                return false;
            }
            if (isNaN(Number(tbxEpNo_End.value)) == true) {
                alert('集數只能輸入數字');
                tbxEpNo_End.focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="overflow:auto;">
    <form id="Form1" runat="server">
    <div>
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1">
            <tr>
                <td align="left">
                節目代號
                    <asp:TextBox ID="tbxProgramCode" runat="server" Width="100px" />
                集數
                    <asp:TextBox ID="tbxEpNo_Start" runat="server" Width="40px" />～
                    <asp:TextBox ID="tbxEpNo_End" runat="server" Width="40px" />
                    <asp:Button ID="btnQuery" runat="server" class="ui-button ui-corner-all" Width="15mm" Text="查詢" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnQuery_Click"/>
                    <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">未匯入</asp:ListItem>
                        <asp:ListItem Value="2">已匯入</asp:ListItem>
                        <asp:ListItem Value="3">全部</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="2">
                    <table class='table-list' width='100%'>
                        <TR><TH width='10px'></TH>
                            <TH noWrap><SPAN>節目代號</SPAN></TH>
                            <TH noWrap><SPAN>節目簡稱</SPAN></TH>
                            <TH noWrap><SPAN>集數</SPAN></TH>
                            <TH noWrap><SPAN>節目長度</SPAN></TH>
                            <TH noWrap><SPAN>字幕人員</SPAN></TH>
                            <TH noWrap><SPAN>字幕人員姓名</SPAN></TH>
                            <TH noWrap><SPAN>製作類別</SPAN></TH>
                            <TH noWrap><SPAN>單集成本</SPAN></TH>
                            <TH noWrap><SPAN>單集評比</SPAN></TH>
                            <TH noWrap><SPAN>申請日期</SPAN></TH>
                            <TH noWrap><SPAN>匯入日期</SPAN></TH>
                            <TH noWrap><SPAN>鎖定日期</SPAN></TH>
                            <TH noWrap><SPAN>結帳年月</SPAN></TH>
                        </TR>
                        <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
