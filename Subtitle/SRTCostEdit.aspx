<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SRTCostEdit.aspx.cs" Inherits="View_SRTCostEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>字幕成本修改</title>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript">
        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "tbxCostEffectiveDate",   // id of the input field
                button: "imgCostEffectiveDate"     // 與觸發動作的物件ID相同
            });
        }
        function CheckFieldMustFillBasic() {
            var strRet = "";
            var tbxCostEffectiveDate = document.getElementById('tbxCostEffectiveDate');
            var ddlMake_Category = document.getElementById('ddlMake_Category');
            var tbxLength = document.getElementById('tbxLength');
            var tbxAmount = document.getElementById('tbxAmount');

            if (tbxCostEffectiveDate.value == "") {
                strRet += "成本生效日期不可空白";
                alert(strRet);
                return false;
            }
            /*if (tbxLength.value == "") {
                strRet += "標準長度不可空白";
                alert(strRet);
                return false;
            }*/
            if (isNaN(Number(tbxLength.value)) == true) {
                alert('標準長度  欄位必須為數字！');
                tbxLength.focus();
                return false;
            }
            /*if (tbxAmount.value == "") {
                strRet += "標準金額不可空白";
                alert(strRet);
                return false;
            }*/
            if (isNaN(Number(tbxAmount.value)) == true) {
                alert('標準長度  欄位必須為數字！');
                tbxAmount.focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Uid" />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table-detail"> 
            <tr style="HEIGHT: 25px">
               <td style="text-align: left; background-color: #5EABDB; color: #FFFFFF;" colspan="2">字幕成本編輯</td>
            </tr>
            <tr>
                <td align="right"><font color="red">成本生效日期</font>
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox ID="tbxCostEffectiveDate" runat="server" onchange="CheckDateFormat(this, '成本生效日期');" Width="100px" Enabled="false"></asp:TextBox>
                    <img id="imgCostEffectiveDate" alt="" src="../img/date.gif" />
                </td>
            </tr>
            <tr>
                <td align="right"><font color="red">製作類別</font>
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:dropdownlist runat="server" ID="ddlMake_Category" Enabled="false"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td align="right"><font color="red">標準長度</font>
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxLength" Width="100px" Enabled="false"></asp:TextBox>分鐘
                </td>
            </tr>
            <tr>
                <td align="right"><font color="red">標準金額</font>
                </td>
                <td align="left" colspan="5" class="style1">
                    <asp:TextBox runat="server" ID="tbxAmount" Width="100px"></asp:TextBox>元
                </td>
            </tr>
            <tr>
                <td align="right">說明
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxDescription" TextMode="MultiLine" Width="550px" Height="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right"><font color="red">啟用</font>
                </td>
                <td align="left" colspan="5" class="style1">
                    <asp:CheckBox ID="cbxEnable" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="right" colspan="6">
                    <asp:Button ID="btnEdit" class="ui-button ui-corner-all" runat="server" Text="修改" Width="70px" onclick="btnEdit_Click" OnClientClick= "return CheckFieldMustFillBasic(); "/>
                    <%-- <asp:Button ID="btnDel" class="ui-button ui-corner-all" runat="server" Text="刪除" Width="70px" onclick="btnDel_Click" OnClientClick= "return confirm('您是否確定要刪除？'); "/>--%>
                    <asp:Button ID="btnExit" class="ui-button ui-corner-all" runat="server" Width="20mm" Text="離開" onclick="btnExit_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
