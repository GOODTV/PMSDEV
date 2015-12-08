<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShortFilmQuery.aspx.cs" Inherits="ShortFilm_ShortFilmQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=11"/>
    <title>短片維護</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            if ($('#HFD_Mode').val() == 'print') {
                location.href = 'ShortFilm_Print_Excel.aspx';
            }

        });

        function CheckFieldMustFillBasic() {

            if ($("#tbxEpisode").val() != "") {
                //檢查數字欄位值樣本
                var checkNumber = /^\d+$/;
                if (!checkNumber.test($("#tbxEpisode").val())) {
                    alert('節目集數必需為數字！');
                    return false;
                }
            }

            if ($('#tbxCFID').val() == "" && $('#tbxCFTitle').val() == "" && $('#tbxProgramID').val() == "") {
                alert("請輸入查詢條件，至少輸入短片代號、短片名稱、節目代號其中之一，以預防資料量過大。");
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
    <asp:HiddenField runat="server" ID="HFD_Mode" />
       <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1">
            <tr>
                <td align="left">
                短片代號
                <asp:TextBox ID="tbxCFID" runat="server" Width="100px" />
                短片名稱
                <asp:TextBox ID="tbxCFTitle" runat="server" Width="150px" />
                節目代號
                    <asp:TextBox ID="tbxProgramID" runat="server" Width="100px" />
                集數
                    <asp:TextBox ID="tbxEpisode" runat="server" Width="100px" />
                台別
                    <asp:Dropdownlist ID="ddlChannel" runat="server" Width="100px">
                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1">一台</asp:ListItem>
                        <asp:ListItem Value="2">二台</asp:ListItem>
                        <asp:ListItem Value="3">一二台</asp:ListItem>
                    </asp:Dropdownlist>
                    <asp:Button ID="btnQuery" class="ui-button ui-corner-all" runat="server" Width="15mm" Text="搜尋" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnQuery_Click"/>
                    資料筆數：<asp:Label ID="count" runat="server"></asp:Label>
                    
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="2">
                    <table class='table-list' width='100%'>
                        <tr>
                            <th width='30px'><span>台別</span></th>
                            <th width='80px'><span>短片代號</span></th>
                            <th width='200px'><span>節目代號 - 名稱</span></th>
                            <th width='40px'><span>集數</span></th>
                            <th width='200px'><span>短片名稱</span></th>
                            <th width='90px'><span>長度(秒)</span></th>
                            <th width='200px'><span>播映期間</span></th>
                            <th width='40px'><span>頻率</span></th>
                            <th><span>時段</span></th>
                            <th width='60px'><span>提供官網</span></th>
                            <th width='100px'><span>排程同工</span></th>
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
