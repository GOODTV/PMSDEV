<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgramEpisodeQuery.aspx.cs" Inherits="View_ProgramEpisodeQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>供片系統分集主檔查詢</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript">
        function CheckFieldMustFillBasic() {
            var tbxProgramCode = document.getElementById('tbxProgramCode');
            var tbxEpisodeNo_Start = document.getElementById('tbxEpisodeNo_Start');
            var tbxEpisodeNo_End = document.getElementById('tbxEpisodeNo_End');

            if (tbxProgramCode.value == "") {
                alert('節目代號必填！');
                tbxProgramCode.focus();
                return false;
            }
            if (isNaN(Number(tbxEpisodeNo_Start.value)) == true) {
                alert('集數 必須為數字！');
                tbxEpisodeNo_Start.focus();
                return false;
            }
            if (isNaN(Number(tbxEpisodeNo_End.value)) == true) {
                alert('集數 必須為數字！');
                tbxEpisodeNo_End.focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="overflow:auto;">
    <form id="Form1" runat="server">
    <div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr>
                <th align="right" colspan="1">
                節目代號：
                </th>
                <td align="left">
                    <asp:TextBox ID="tbxProgramCode" CssClass="font9" runat="server" Width="100px" />
                </td>
                <th align="right" colspan="1">
                集數：
                </th>
                <td align="left">
                    <asp:TextBox ID="tbxEpisodeNo_Start" CssClass="font9" runat="server" Width="50px" />～
                    <asp:TextBox ID="tbxEpisodeNo_End" CssClass="font9" runat="server" Width="50px" />
                </td>
                <td colspan="7" align="right">
                    <asp:Button ID="btnQuery" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="查詢" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnQuery_Click"/>
                    <asp:Button ID="btnAdd" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="新增" onclick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="9">
                    <table class='table-list' width='100%'>
                        <TR>
                            <TH noWrap><SPAN>節目代號</SPAN></TH>
                            <TH noWrap><SPAN>節目名稱</SPAN></TH>
                            <TH noWrap><SPAN>集數</SPAN></TH>
                            <TH noWrap><SPAN>系列名稱</SPAN></TH>
                            <TH noWrap><SPAN>分集名稱</SPAN></TH>
                            <TH noWrap><SPAN>主持人or講員</SPAN></TH>
                            <TH noWrap><SPAN>總長度</SPAN></TH>
                            <TH noWrap><SPAN>不播出註記</SPAN></TH>
                            <TH noWrap><SPAN>不重播註記</SPAN></TH>
                            <TH noWrap><SPAN>不供片註記</SPAN></TH>
                        </TR>
                     <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
