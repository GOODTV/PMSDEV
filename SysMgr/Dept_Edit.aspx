<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dept_Edit.aspx.cs" Inherits="SysMgr_Dept_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>機構部門管理</title>
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
    <form id="Form1" name="form" method="POST" runat="server">
    <asp:HiddenField ID="HFD_Mode" runat="server" />
    <asp:HiddenField ID="HFD_DeptUID" runat="server" />
    <div id="container">
        <table width="100%" class="table-detail">
            <tr>
                <th width="10%" align="right">
                    機構名稱:
                </th>
                <td>
                    <asp:DropDownList ID="ddlOrgID" runat="server" >
<%--                    <asp:DropDownList ID="ddlOrgID" runat="server" Enabled="False">--%>
                    </asp:DropDownList>
                </td>
                <th width="10%" align="right">
                    部門代號:
                </th>
                <td width="15%">
                    <asp:TextBox runat="server" ID="txtDeptID" Width="48" MaxLength="10" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="10%" align="right">
                    部門名稱:
                </th>
                <td width="15%">
                    <asp:TextBox runat="server" ID="txtDeptName" Width="180" MaxLength="30"></asp:TextBox>
                </td>
                <th width="10%" align="right">
                    部門簡稱:
                </th>
                <td width="15%">
                    <asp:TextBox runat="server" ID="txtDeptShortName" Width="180" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="function">
            <asp:Button ID="btnAdd" class="ui-button ui-corner-all" runat="server" Text="新增"
                OnClick="btnAdd_Click" />
            <asp:Button ID="btnUpdate" class="ui-button ui-corner-all" runat="server" Text="修改"
                OnClick="btnUpdate_Click" />
            <asp:Button ID="btnDelete" class="ui-button ui-corner-all" runat="server" Text="刪除"
                OnClick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');" />
            <asp:Button ID="btnExit" class="ui-button ui-corner-all" runat="server" Text="離開"
                OnClick="btnExit_Click" />
        </div>
    </div>
    </form>
</body>
</html>
