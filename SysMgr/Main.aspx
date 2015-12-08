<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="Main.aspx.cs" Inherits="SysMgr_Main" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>節目管理系統</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/mainmenu.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/mainmenu.js"></script>
</head>
<body>
    <table id="nav-table" class="nav-table">
        <tr>
            <td>
                <span id="nav-title" class="nav-title">[首頁]</span>
            </td>
            <td>
                <asp:Literal ID="lblMenuContainer" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>

    <asp:Label ID="lblRemoteAddr" runat="server" Visible="False"></asp:Label>
    <iframe id="content" name="content" src="/SysMgr/MainDefault.aspx" style="width: 98%; border: 0px; overflow: hidden;"></iframe>

</body>
</html>
