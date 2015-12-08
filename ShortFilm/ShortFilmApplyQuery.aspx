<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShortFilmApplyQuery.aspx.cs" Inherits="ShortFilm_ShortFilmApplyQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=11"/>
    <title>短片申請查詢</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript">

        function CheckFieldMustFillBasic() {
            var strRet = "";
            var tbxCFID = document.getElementById('tbxCFID');
            var tbxProgramID = document.getElementById('tbxProgramID');
            var tbxRequesterID = document.getElementById('tbxRequesterID');
            if (tbxCFID.value == "" && tbxProgramID.value == "" && tbxRequesterID.value == "") {
                strRet += "請輸入查詢參數";
                alert(strRet);
                return false;
            }
            return true;
        }

    </script>
</head>
<body style="overflow:auto;">
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Key" />
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1">
            <tr>
                <td>
                短片代號
                <asp:TextBox ID="tbxCFID" runat="server" Width="100px" />
                節目代號
                    <asp:TextBox ID="tbxProgramID" runat="server" Width="100px" />
                同工編號
                    <asp:TextBox ID="tbxRequesterID" runat="server" Width="100px" />
                </td>
                <td>
                    <asp:RadioButtonList ID="rblValidStatus" runat="server" RepeatDirection="Horizontal" >
                        <asp:ListItem Value="1">有效短片</asp:ListItem>
                        <asp:ListItem Value="2">無效短片</asp:ListItem>
                        <asp:ListItem Value="3" Selected="True">全部短片</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Button ID="btnQuery" class="ui-button ui-corner-all" runat="server" Width="15mm" Text="查詢" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnQuery_Click"/>
                    資料筆數：<asp:Label ID="count" runat="server"></asp:Label>
                </td>
                <td><div style="width:300px;"></div>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="4">
                    <table class='table-list' width='100%'>
                        <tr><th width='80px'><span>短片代號</span></th>
                            <th width='200px'><span>短片名稱</span></th>
                            <th width='200px'><span>節目代號 - 名稱</span></th>
                            <th width='40px'><span>集數</span></th>
                            <th width='80px'><span>申請日期</span></th>
                            <th width='100px'><span>申請人</span></th>
                            <th width='200px'><span>播映期間</span></th>
                            <th noWrap><span>片庫確認</span></th>
                            <th noWrap><span>頻率</span></th>
                            <th noWrap><span>時段</span></th>
                            <th width='100px'><span>片庫同工</span></th>
                            <th noWrap><span>提供官網</span></th>
                            <th noWrap><span>註記欄</span></th>
                        </tr>
                        <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
