<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShortFilmCategoryQuery.aspx.cs" Inherits="ShortFilm_ShortFilmCategoryQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>短片類別維護查詢</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1">
            <tr>
                <td align="left">
                    <asp:Button ID="btnAdd" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="新增" onclick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                     <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
