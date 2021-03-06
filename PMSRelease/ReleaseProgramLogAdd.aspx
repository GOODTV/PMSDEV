﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReleaseProgramLogAdd.aspx.cs" Inherits="View_ReleaseProgramLogAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>節目交檔記錄新增</title>
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
            var txtSupplyDate = document.getElementById('txtSupplyDate');

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
            if (txtSupplyDate.value == "") {
                strRet += "供片時間不可空白";
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
            var NotedisOK = CheckProgramEpisodeIsNoted();
            if (!NotedisOK) {
                return false;
            }
            var SuppplyedisOK = CheckSameCustomerProgramEpisodeIsSuppplyed();
            if (!SuppplyedisOK) {
                return false;
            }
            return true;
        }
        //依節目代號及集數檢核分集資料庫中不播出、不重播及不供片標記 2015/1/30增加
        function CheckProgramEpisodeIsNoted() {
            ProgramEpisodeIsNoted($.trim($('#txtProgramID').val()), $.trim($('#txtEpisode').val()));
            //alert($('#HFD_ProgramEpisodeIsNoted').val());
            if ($('#HFD_ProgramEpisodeIsNoted').val() == 'Y') {
                if (confirm('此集 有不播出、不重播或不供片標記。\n您確定要新增交檔供片記錄？')) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }
        function ProgramEpisodeIsNoted(ProgramID, Episode) {
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=2' + '&ProgramID=' + ProgramID + '&Episode=' + Episode,
                async: false, //同步
                success: function (result) {
                    $('#HFD_ProgramEpisodeIsNoted').val(result);
                },
                error: function () { alert('ajax failed'); }
            })
        }
        //檢核同一客戶是否己供過同一節目同一集數 2015/1/30增加
        function CheckSameCustomerProgramEpisodeIsSuppplyed() {
            SameCustomerProgramEpisodeIsSuppplyed($.trim($('#txtCustomerID').val()), $.trim($('#txtProgramID').val()), $.trim($('#txtEpisode').val()));
            //alert($('#HFD_SameCustomerProgramEpisodeIsSuppplyed').val().substr(0,1));
            if ($('#HFD_SameCustomerProgramEpisodeIsSuppplyed').val().substr(0, 1) == 'Y') {
                var str = $('#HFD_SameCustomerProgramEpisodeIsSuppplyed').val();
                var supplydate = str.substr(1, str.length - 1);
                //alert(supplydate);
                if (confirm('該節目集數已於"' + supplydate + '"供片過！\n您確定要新增交檔供片記錄？')) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }
        function SameCustomerProgramEpisodeIsSuppplyed(CustomerID, ProgramID, Episode) {
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=3' + '&CustomerID=' + CustomerID + '&ProgramID=' + ProgramID + '&Episode=' + Episode,
                async: false, //同步
                success: function (result) {
                    $('#HFD_SameCustomerProgramEpisodeIsSuppplyed').val(result);
                },
                error: function () { alert('ajax failed'); }
            })
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
            window.open('CustomerShow.aspx?customerID=txtCustomerID&customerName=tbxCustomerName', 'NewWindows',
                        'status=no,scrollbars=yes,top=100,left=120,width=500,height=450;');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="HFD_ProgramEpisodeIsNoted" runat="server" />
        <asp:HiddenField ID="HFD_SameCustomerProgramEpisodeIsSuppplyed" runat="server" />
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
                    <asp:TextBox runat="server" ID="txtCustomerID" CssClass="font9" Width="50px" OnTextChanged="btnQueryCustomer_Click"></asp:TextBox>&nbsp;<!--a href onclick="WindowsOpen()" style="cursor:hand"><img border="0" src="../images/toolbar_search.gif" width="17"></a-->
                    <%-- <asp:Button ID="btnQueryCustomer" runat="server" Width="15mm" Text="查詢" OnClick="btnQueryCustomer_Click"/>--%>
                    <asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label>
                    <br /><div id="memo" style="color: red; font-size: medium;">輸入客戶代號，按「Enter」即可呈現客戶名稱。</div>
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
                    <asp:Button ID="btnAdd" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="儲存" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnAdd_Click"/>
                    <asp:Button ID="btnExit" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="離開" Onclick="btnExit_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
