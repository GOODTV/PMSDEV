﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReleaseProgramLogEdit.aspx.cs" Inherits="View_ReleaseProgramLogEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>節目交檔記錄修改</title>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "txtSupplyDate",   // id of the input field
                button: "imgSupplyDate"     // 與觸發動作的物件ID相同
            });
        }
        function CheckFieldMustFillBasic() {
            var strRet = "";
            var cnt = 0;
            var sName = "";
            var txtProgramID = document.getElementById('txtProgramID');
            var txtEpisode = document.getElementById('txtEpisode');
            var txtCustomerID = document.getElementById('txtCustomerID');

            if (txtProgramID.value == "") {
                strRet += "節目代號不可空白";
                alert(strRet);
                return false;
            }
            if (txtEpisode.value == "") {
                strRet += "集數不可空白";
                alert(strRet);
                return false;
            }
            if (txtCustomerID.value == "") {
                strRet += "客戶代號不可空白";
                alert(strRet);
                return false;
            }
            cnt = 0;
            sName = txtCustomerID.value;
            for (var i = 0; i < sName.length; i++) {
                if (escape(sName.charAt(i)).length >= 4) cnt += 2;
                else cnt++;
            }
            if (cnt > 3) {
                alert('客戶代號 欄位長度超過限制！');
                return false;
            }
            return true;
        }
        function FilenameKeyIn() {
            var txtProgramID = document.getElementById('txtProgramID');
            var txtEpisode = document.getElementById('txtEpisode');
            var txtFilename = document.getElementById('txtFilename');
            var ep = padLeft(txtEpisode.value, 4)
            txtFilename.value = txtProgramID.value + '_' + ep;
        }
        function padLeft(str, lenght) {
            if (str.length >= lenght)
                return str;
            else
                return padLeft("0" + str, lenght);
        }
        function WindowsOpen() {
            window.open('CustomerShow.aspx?customerID=txtCustomerID&customerName=lblCustomerName', 'NewWindows',
                        'status=no,scrollbars=yes,top=100,left=120,width=500,height=450;');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Uid" />
        <asp:HiddenField runat="server" ID="HFD_Key" />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table-detail"> 
            <tr style="HEIGHT: 25px">
               <td style="text-align: left; background-color: #5EABDB; color: #FFFFFF;" colspan="5">節目交檔記錄編輯</td>
            </tr>
            <tr>
                <asp:panel id="Panel_Query" runat="server" defaultbutton="btnQuery">
                    <th align="right" colspan="1"><font color="red">*</font>
                        節目代號：
                    </th>
                    <td align="left" colspan="2" class="style1">
                        <asp:TextBox runat="server" ID="txtProgramID" CssClass="font9" Width="100px"></asp:TextBox>
                        <asp:Button ID="btnQuery" runat="server" Width="15mm" Text="查詢" OnClick="btnQuery_Click"/> 
                        <asp:Label ID="lblProgramName" runat="server" Text=""></asp:Label>
                    </td>
                </asp:panel>
                <th align="right" colspan="1"><font color="red">*</font>
                    集數：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtEpisode" CssClass="font9" Width="70px" onkeyup="FilenameKeyIn();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1"><font color="red">*</font>
                    客戶代號：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtCustomerID" CssClass="font9" Width="50px" OnTextChanged="btnQueryCustomer_Click"></asp:TextBox>&nbsp;<!--a href onclick="WindowsOpen()" style="cursor:hand"><img border="0" src="../images/toolbar_search.gif" width="17"></!--a-->
                    <asp:Button ID="btnQueryCustomer" runat="server" Width="15mm" Text="查詢" OnClick="btnQueryCustomer_Click"/>
                    <asp:Label ID="lblCustomerName" runat="server" ReadOnly="true"></asp:Label>
                    <br /><div id="memo" style="color: red; font-size: medium;">輸入客戶代號，按「Enter」或「查詢」即可呈現客戶名稱。</div>
                </td>
                <th align="right" colspan="1"><font color="red">*</font>
                    供片時間：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtSupplyDate" CssClass="font9" Width="100px"></asp:TextBox>
                    <img id="imgSupplyDate" alt="" src="../img/date.gif" />
                </td>
            </tr>
            <tr>
                <th align="right" colspan="1">
                    檔案名稱：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtFilename" CssClass="font9" Width="200px"></asp:TextBox>
                </td>
                <th align="right" colspan="1">
                    Logo：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:CheckBox ID="cbxLogo" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="right" colspan="6">
                <asp:Button ID="btnEdit" runat="server" class="ui-button ui-corner-all" Text="修改" Width="70px" onclick="btnEdit_Click" OnClientClick= "return CheckFieldMustFillBasic(); "/>
                <asp:Button ID="btnDel" runat="server" class="ui-button ui-corner-all" Text="刪除" Width="70px" onclick="btnDel_Click" OnClientClick= "return confirm('您是否確定要刪除節目？'); "/>
                <asp:Button ID="btnExit" runat="server" class="ui-button ui-corner-all" Text="取消" Width="70px" onclientclick="history.back(); return false;" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
