<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShortFilmEdit.aspx.cs" Inherits="ShortFilm_ShortFilmEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=11"/>
    <title>短片申請修改</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $("#tbxOnAirBeginDate").datepicker();
            $("#tbxOnAirEndDate").datepicker();

            /*
            var ApplierName = getApplier_ajax($("#tbxMediaUser").val());
            if (ApplierName == "") {
                $("#tbxMediaUserName").val("");
                $("#tbxMediaUser").addClass("z-textbox-text-invalid");
                alert('排程同工代號錯誤！');
                return false;
            }
            else {
                $("#tbxMediaUserName").val(ApplierName);
                $("#tbxMediaUser").removeClass("z-textbox-text-invalid");
            }

            var ApplierName = getApplier_ajax($("#tbxRequesterID").val());
            if (ApplierName == "") {
                $("#tbxRequesterName").val("");
                $("#tbxRequesterID").addClass("z-textbox-text-invalid");
                alert('申請同工代號錯誤！');
                return false;
            }
            else {
                $("#tbxRequesterName").val(ApplierName);
                $("#tbxRequesterID").removeClass("z-textbox-text-invalid");
            }
            */

            $("#tbxRequesterID").blur(function () {

                var ApplierName = getApplier_ajax($("#tbxRequesterID").val());
                if (ApplierName == "") {
                    $("#tbxRequesterName").val("");
                    alert('查無同工資料');
                    return false;
                }
                else {
                    $("#tbxRequesterName").val(ApplierName);
                }

            });

            $("#tbxMediaUser").blur(function () {

                var ApplierName = getApplier_ajax($("#tbxMediaUser").val());
                if (ApplierName == "") {
                    $("#tbxMediaUserName").val("");
                    alert('查無同工資料！');
                    return false;
                }
                else {
                    $("#tbxMediaUserName").val(ApplierName);
                }

            });

            $("#tbxProgramID").blur(function () {

                if ($("#tbxProgramID").val() == "") {
                    $("#tbxProgramName").val("");
                    $("#tbxEpisode").focus();
                    return;
                }
                var getProgramName_ajax_ok = getProgramName_ajax($("#tbxProgramID").val());
                if (getProgramName_ajax_ok == "no") {
                    $("#tbxProgramName").val("");
                    alert('找不到節目代碼');
                    $("#tbxProgramID").focus();
                    return false;
                }
                else if (getProgramName_ajax_ok == "err") {
                    $("#tbxProgramName").val("");
                    alert('無法連到資料庫');
                    $("#tbxCFTitle").focus();
                    return false;
                }

            });

            /*
            $("#tbxMediaUser").bind("keypress", function (e) {

                if (e.which == 13) {
                    $("#tbxMediaUser").blur();
                }

            });
            */

            //排除按Enter會誤動作
            $("body").bind("keypress", function (e) {

                if (e.which == 13) {
                    return false;
                }

            });


        });

        function CheckField() {

            var strRet = "";

            if ($("#tbxMediaUser").val() == "") {
                $("#tbxMediaUser").addClass("z-textbox-text-invalid");
                alert('排程同工不可空白！');
                return false;
            }

            if ($("#tbxRequesterID").val() == "") {
                $("#tbxRequesterID").addClass("z-textbox-text-invalid");
                alert('申請同工不可空白！');
                return false;
            }

            if ($("#tbxLengthSec").val() >= 60) {
                alert('短片長度的秒數有誤！');
                return false;
            }

            if ($("#tbxOnAirBeginDate").val() == "") {
                alert('請檢查排播時間');
                return false;
            }

            if ($("#tbxOnAirEndDate").val() == "") {
                alert('請檢查排播時間！');
                return false;
            }

            if (!dateValidationCheck($('#tbxOnAirBeginDate').val())) {
                alert('請檢查排播時間！');
                return false;
            }

            if (!dateValidationCheck($('#tbxOnAirEndDate').val())) {
                alert('請檢查排播時間！');
                return false;
            }

            if (!checkDateStartEnd($('#tbxOnAirBeginDate').val(),$('#tbxOnAirEndDate').val())) {
                alert('請檢查排播時間！');
                return false;
            }

            if ($('input:radio[name="rblFrequency"]:checked').length == 0) {
                alert('請檢查排播頻率！');
                return false;
            }

            if ($('input:checkbox[name="GN_OnAirTimeSlot"]:checked').length == 0) {
                alert('請檢查排播時段！');
                return false;
            }


            return true;
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
                        strApplierName = mydata[0].EMPLOYEE_CNAME;
                    }
                },
                failure: function (ex) {
                    alert(ex.get_message);
                }
            });

            return strApplierName;

        }

        //取得節目名稱
        function getProgramName_ajax(ProgramID) {

            var strData = "{'ProgramID':'" + ProgramID + "'}";
            var ok = "err";

            $.ajax({
                type: "POST",
                url: "../SubtitleWebService.asmx/GetProgramName",
                data: strData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (json) {
                    var mydata = $.parseJSON(json.d);
                    if (mydata.length > 0) {
                        $("#tbxProgramID").val(mydata[0].ProgramID);
                        $("#tbxProgramName").val(mydata[0].ProgramAbbrev);
                        ok = "yes";
                    }
                    else {
                        ok = "no";
                    }
                },
                failure: function (ex) {
                    alert(ex.get_message);
                }
            });
            return ok;
        }

    </script>
    <style type="text/css">

        .z-textbox-text-invalid {
            background: #FFF repeat-x 0 0;
            background-image: url(/img/misc/text-bg-invalid.gif);
            border-color: #DD7870;
        }

        #rblFrequency td {
            border-right: 0px solid #CCC;
            border-bottom: 0px solid #DDD;
        }

        .table-detail th {
            height: 30px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Uid" />
        <asp:HiddenField runat="server" ID="HFD_Key" />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table-detail"> 
            <tr style="HEIGHT: 25px">
               <td style="text-align: left; background-color: #5EABDB; color: #FFFFFF;" colspan="5">短片申請修改</td>
            </tr>
            <tr>
                <th align="right" Width="400px">
                    排程同工：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxMediaUser" CssClass="font9" Width="100px"></asp:Textbox>
                    <asp:Textbox runat="server" ID="tbxMediaUserName" Width="100px" CssClass="font9" ReadOnly="True" style="background: #ECEAE4; color: black;"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    短片編號：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxCFID" CssClass="font9" Width="100px"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    節目代號：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxProgramID" Width="100px" CssClass="font9"></asp:Textbox>
                    <asp:Textbox runat="server" ID="tbxProgramName" Width="200px" CssClass="font9" ReadOnly="true" style="background: #ECEAE4; color: black;"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    節目集數：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxEpisode" Width="50px" CssClass="font9"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    短片名稱：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxCFTitle" Width="700px" CssClass="font9"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    短片長度：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxLengthMin" Width="50px" CssClass="font9"></asp:Textbox>分
                    <asp:Textbox runat="server" ID="tbxLengthSec" Width="50px" CssClass="font9"></asp:Textbox>秒
                </td>
            </tr>
            <tr>
                <th align="right">
                    HDC：
                </th>
                <td align="left">
                    <asp:DropDownList runat="server" ID="ddlHDC" Width="100px">
                        <asp:ListItem Value="Y">Y</asp:ListItem>
                        <asp:ListItem Value="N">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th align="right">
                    播出頻道：
                </th>
                <td align="left">
                    <asp:Dropdownlist ID="ddlChannel" runat="server" Width="100px">
                        <asp:ListItem Value="1">一台</asp:ListItem>
                        <asp:ListItem Value="2">二台</asp:ListItem>
                        <asp:ListItem Value="3">一二台</asp:ListItem>
                    </asp:Dropdownlist>
                </td>
            </tr>
            <tr>
                <th align="right">
                    內容摘要說明：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxCFDescription" CssClass="font9" Width="700px"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    排播時間：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxOnAirBeginDate" CssClass="font9" Width="80px"></asp:Textbox>
                    至 <asp:Textbox runat="server" ID="tbxOnAirEndDate" CssClass="font9" Width="80px"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    排播說明：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxOnAirRemark" Width="700px" CssClass="font9"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    短片註記(限100字)：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxAppendance" CssClass="font9" Width="700px"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    排播頻率：
                </th>
                <td align="left">
                    <asp:RadioButtonList ID="rblFrequency" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="A級" Value="1"></asp:ListItem>
                        <asp:ListItem Text="B級" Value="2"></asp:ListItem>
                        <asp:ListItem Text="C級" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th align="right">
                    排播時段：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="lblOnAirTimeSlot" CssClass="font9" Width="300px"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    提供官網：
                </th>
                <td align="left">
                    <asp:Checkbox runat="server" ID="cbxForWeb" CssClass="font9" Width="100px"></asp:Checkbox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    申請同工：
                </th>
                <td align="left">
                    <asp:Textbox runat="server" ID="tbxRequesterID" CssClass="font9" Width="100px"></asp:Textbox>
                    <asp:Textbox runat="server" ID="tbxRequesterName" Width="100px" CssClass="font9" ReadOnly="true" style="background: #ECEAE4; color: black;"></asp:Textbox>
                </td>
            </tr>
            
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="btnDel" runat="server" class="ui-button ui-corner-all" Text="刪除" Width="70px" onclick="btnDel_Click" OnClientClick="return confirm('確定刪除？');" />
                    <asp:Button ID="btnEdit" runat="server" class="ui-button ui-corner-all" Text="確認" Width="70px" onclick="btnEdit_Click" OnClientClick="return CheckField();" />
                    <asp:Button ID="btnExit" runat="server" class="ui-button ui-corner-all" Text="關閉" Width="70px" onclick="btnExit_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
