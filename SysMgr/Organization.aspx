<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Organization.aspx.cs" Inherits="SysMgr_Organization" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>機構管理</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script src="../Scripts/pms-script.js"></script>

    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

        });	
    </script>
</head>
<body class="body">
    <form id="Form1" name="form" runat="server">
    <div id="container">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1>
            <tr>
                 <td style="float: right; width: 30px">
                     <asp:Button ID="btnAdd" class="ui-button ui-corner-all" runat="server" Text="新增" OnClick="btnAdd_Click" />
                 </td>
            </tr>
            <tr>
                <td style="width: 100%">
                    <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
