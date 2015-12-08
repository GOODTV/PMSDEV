<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgramChangeApply.aspx.cs" Inherits="ProgramChange_ProgramChangeApply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>節目異動申請</title>
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

        //window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "tbxChangeDate",   // id of the input field
                button: "imgChangeDate"     // 與觸發動作的物件ID相同
            });
        }

        $(document).ready(function () {

            initCalendar();

            $('#cbxIsFamilyTime').change(function () {
                if ($(this).is(':checked')) {
                    document.getElementById('<%=tbxContent.ClientID%>').innerHTML = document.getElementById('<%=tbxContent.ClientID%>').innerHTML+"\r\n"+"@@ 家庭關懷專線播映時段  @@"; 
                }
            });

            $("#tbxRequester").blur(function () {

                if ($("#tbxRequester").val() == "") {
                    $("#tbxRequester").focus();
                    return false;
                }
                var ApplierName = getApplier_ajax($("#tbxRequester").val());
                if (ApplierName == "") {
                    $("#tbxRequesterData").val("");
                    alert('查無同工資料');
                    return false;
                }
                else {
                    $("#tbxRequesterData").val(ApplierName);
                    $("#tbxRequesterData").addClass("label");
                }

            });


            //排除按Enter會誤動作
            $("body").bind("keypress", function (e) {

                if (e.which == 13) {
                    if (e.target.id == "tbxContent") {
                    }
                    else if (e.target.id == "tbxRequester") {
                        $("#tbxRequester").blur();
                    }
                    else {
                        return false;
                    }
                }

            });

        });

        function CheckFieldMustFillBasic() {
            var tbxRequester = document.getElementById('tbxRequester');
            var strRet = "";
            if (tbxRequester.value == "") {
                strRet += "請輸入同工編號欄位";
                alert(strRet);
                return false;
            }
            if (confirm('確認申請？')) {
                return true;
            } else {
                return false;
            }
        }

        //取得申請人
        function getApplier_ajax(ApplierID) {

            var strData = "{'EMPLOYEE_NO':'" + ApplierID + "'}";
            var strApplierName = "";

            $.ajax({
                type: "POST",
                url: "../HRService.asmx/GetEmployee",
                data: strData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (json) {
                    var mydata = $.parseJSON(json.d);
                    if (mydata.length > 0) {
                        strApplierName = mydata[0].EMPLOYEE_CNAME + "\r\n";
                        strApplierName += mydata[0].EMPLOYEE_NO + "\r\n";
                        strApplierName += mydata[0].DEPARTMENT_CNAME + "\r\n";
                        strApplierName += mydata[0].EMPLOYEE_OFFICE_TEL_1 + "\r\n";
                        strApplierName += mydata[0].EMPLOYEE_CONTACT_TEL_1;
                    }

                },
                failure: function (ex) {
                    alert(ex.get_message);
                }
            });

            //return strApplierName strApplierNo + "\r\n" + strApplierDptName + "\r\n" + strApplierOfficeTEL + "\r\n" + strApplierMobile;
            return strApplierName;

        }

    </script>
    <style type="text/css">

        body {
            font-family:Verdana,Tahoma,Arial,Helvetica,sans-serif;
            font-size: 16px;
        }

        .label {
            font-family:Verdana,Tahoma,Arial,Helvetica,sans-serif;
            font-size:medium;
            text-align:right;
            line-height:20pt;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table-detail"> 
            <tr style="HEIGHT: 30px">
               <td style="text-align: left; background-color: #5EABDB; color: #FFFFFF; font-size: 24px;" colspan="5">節目異動申請</td>
            </tr>
            <tr>
                <th rowspan="3" style="font-size: 18px">申請內容</th>
                <td align="left">
                    異動日期
                </td>
                <td align="left" colspan="3">
                    <asp:TextBox runat="server" ID="tbxChangeDate" CssClass="font9" Width="100px"></asp:TextBox>
                    <img id="imgChangeDate" alt="" src="../img/date.gif" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    異動頻道
                </td>
                <td align="left" colspan="3">
                    <asp:Dropdownlist ID="ddlChannel" runat="server" Width="100px">
                        <asp:ListItem Value="01" Selected="True">一台</asp:ListItem>
                        <asp:ListItem Value="02">二台</asp:ListItem>
                    </asp:Dropdownlist>
                </td>
            </tr>
            <tr>
                <td align="left">
                    說明
                </td>
                <td align="left" colspan="3" height="100px">
                    <asp:TextBox runat="server" ID="tbxContent" CssClass="font9" TextMode="MultiLine" Width="400px" Height="100px"></asp:TextBox><br />
                    <asp:CheckBox runat="server" ID="cbxIsFamilyTime" Text="是否為家庭關懷專線播映時段"/>
                </td>
            </tr>
            <tr>
                <th rowspan="2" style="font-size: 18px">申請人</th>
                <td align="left">
                    同工編號
                </td>
                <td align="left" colspan="2" height="50px">
                    <asp:TextBox runat="server" ID="tbxRequester" CssClass="font9"></asp:TextBox>
                </td>
                <td rowspan="2" Width="200px"><asp:TextBox ID="tbxRequesterData" runat="server" TextMode="MultiLine" Width="200px" Height="130px" ReadOnly="True" Style="overflow: hidden;border: hidden;"></asp:TextBox></td>
            </tr>
            <tr><td colspan="3" height="100px"> </td></tr>
            <tr>
                <td align="right" colspan="5">
                    <asp:Button ID="btnAdd" runat="server" class="ui-button ui-corner-all" Text="申請" Width="70px" onclick="btnAdd_Click" OnClientClick= "return CheckFieldMustFillBasic();"/>
                    <asp:Button ID="btnExit" runat="server" class="ui-button ui-corner-all" Text="取消" Width="70px" onclick="btnExit_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
