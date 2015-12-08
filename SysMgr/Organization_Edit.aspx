<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Organization_Edit.aspx.cs" Inherits="SysMgr_Organization_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>機構部門管理</title>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>

    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
        });
    </script>
    <script type="text/javascript">
        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "txtCreateDate",   // id of the input field
                button: "imgCreateDate"     // 與觸發動作的物件ID相同
            });
        }
    </script>
</head>
<body class="body">
    <form id="Form1" name="form" method="POST" runat="server">
    <asp:HiddenField ID="HFD_Mode" runat="server" />
    <asp:HiddenField ID="HFD_OrgUID" runat="server" />
    <div id="container">
        <table width="100%" class="table-detail">
            <tr>
                <th width="20%" align="right">
                    機構代碼:&nbsp;
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOrgID" Width="80" ReadOnly="true" MaxLength="10"></asp:TextBox>
                </td>
                <th>&nbsp;</th><td>&nbsp;</td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    機構名稱:&nbsp;
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOrgName" Width="200" MaxLength="60"></asp:TextBox>
                </td>
                <th width="20%" align="right">
                    機構Slogan:&nbsp;
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOrg_Slogan" Width="200" MaxLength="60"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    機構簡稱:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOrgShortName" Width="200" MaxLength="20"></asp:TextBox>
                </td>
                <th>&nbsp;</th><td>&nbsp;</td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    網站位址:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtIP" Width="200" MaxLength="20"></asp:TextBox>
                </td>
                <th width="20%" align="right">
                    網站名稱:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtSys_Name" Width="200" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    寄送E-Mail:&nbsp;
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtMail_Url" Width="200" MaxLength="40"></asp:TextBox>
                </td>
                <th width="20%" align="right">
                    郵件寄送方式:&nbsp;
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlMail_SendType" ></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    連絡人:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtContactor" Width="80" MaxLength="10"></asp:TextBox>
                </td>
                <th width="20%" align="right">
                    電話:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtTel" Width="140" MaxLength="20"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <th width="20%" align="right">
                    傳真:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtFax" Width="140" MaxLength="20"></asp:TextBox>
                </td>
                <th width="20%" align="right">
                    E-Mail:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtEmail" Width="200" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    住址:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="tbxZipCode" Width="60px"></asp:TextBox>
                    <asp:dropdownlist runat="server" ID="ddlCity"
                         AutoPostBack="True" onselectedindexchanged="ddlCity_SelectedIndexChanged"></asp:dropdownlist>
                    <asp:dropdownlist runat="server" ID="ddlArea"
                         AutoPostBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged"></asp:dropdownlist>
                    <asp:TextBox runat="server" ID="txtAddress" Width="200" MaxLength="60"></asp:TextBox>
                </td>
                <th>&nbsp;</th><td>&nbsp;</td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    統一編號:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtUniform_No" Width="80" MaxLength="8"></asp:TextBox>
                </td>
                <th width="20%" align="right">
                    劃撥帳號:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtAccount" Width="80" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    成立日期:
                </th>
                <td>
                    <asp:TextBox ID="txtCreateDate" onchange="CheckDateFormat(this, '成立日期');" runat="server"></asp:TextBox>
                <img id="imgCreateDate" alt="" src="../img/date.gif" />
                </td>
                <th width="20%" align="right">
                    立案字號:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtLicence" Width="200" MaxLength="80"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%" align="right">
                    密碼使用天數:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPasswordDay" Width="12" MaxLength="3">0</asp:TextBox>
                    (若設為 0 則密碼可無限期使用)
                </td>
                <th width="20%" align="right">
                    勸募許可文號:
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlRept_Licence" ></asp:DropDownList>
                </td>
            </tr>
        </table>
        <div class="function">
            <asp:Button ID="btnAdd" class="ui-button ui-corner-all" runat="server" 
                Text="新增" onclick="btnAdd_Click"/>
            <asp:Button ID="btnUpdate" class="ui-button ui-corner-all" runat="server" 
                Text="修改" onclick="btnUpdate_Click"/>
            <asp:Button ID="btnDelete" class="ui-button ui-corner-all" runat="server" 
                Text="刪除" onclick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');"/>
            <asp:Button ID="btnExit" class="ui-button ui-corner-all" runat="server" 
                Text="離開" onclick="btnExit_Click"/>
        </div>
    </div>
    </form>
</body>
</html>
