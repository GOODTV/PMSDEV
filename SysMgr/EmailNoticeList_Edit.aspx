<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailNoticeList_Edit.aspx.cs" Inherits="SysMgr_EmailNoticeList_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>修改通知名單設定</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
        });
    </script>
</head>
<body class="body">
    <form id="Form1" runat="server">
    <asp:HiddenField runat="server" ID="HFD_Uid" />
    <table class="table-detail" width="100%">
        <tr>
            <th width="15%" align="right">
                通知人：
            </th>
            <td width="20%">
                <asp:TextBox runat="server" ID="txtEMailName" CssClass="font9"></asp:TextBox>
            </td>
            <th width="15%" align="right">
                通知人Email：
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtEMailAddress" Width="80mm"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%" align="right">
                所屬程式名稱：
            </th>
            <td width="20%">
                <asp:TextBox runat="server" ID="txtProgramName" CssClass="font9"></asp:TextBox>
            </td>
            <th width="15%" align="right">
                同工編號：
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtEmployee_no" Width="80mm"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="10%" align="right">
                通知類別：
            </th>
            <td width="5%">
                <asp:DropDownList ID="ddlEMailType" DataTextField="EMailType" DataValueField="EMailType" runat="server">
                </asp:DropDownList>
                <!--<asp:TextBox runat="server" ID="txtEMailType" CssClass="font9"></asp:TextBox>-->
            </td>
            <th width="10%" align="right">
                是否使用：
            </th>
            <td align="left">
                <asp:RadioButtonList ID="rdoIsUse" runat="server" RepeatDirection="Horizontal" 
                    CssClass="table_noborder">
                    <asp:ListItem Selected="True">使用</asp:ListItem>
                    <asp:ListItem>禁用</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <div class="function">
        <asp:Button ID="btnUpdate" class="ui-button ui-corner-all" runat="server" 
            Text="修改" onclick="btnUpdate_Click"/>
        <asp:Button ID="btnDelete" class="ui-button ui-corner-all" runat="server" 
            Text="刪除" onclick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');"/>
        <asp:Button ID="btnExit" class="ui-button ui-corner-all" runat="server" 
            Text="離開" onclick="btnExit_Click"/>
    </div>
    </form>
</body>
</html>
