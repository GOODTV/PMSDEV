<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgramChangeApplyContent.aspx.cs" Inherits="ProgramChange_ProgramChangeApplyContent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>節目異動申請內容</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {


            //排除按Enter會誤動作
            $("body").bind("keypress", function (e) {

                if (e.which == 13) {
                    if (e.target.id == "tbxReviewComment" || e.target.id == "tbxActionContent") {
                    }
                    else if (e.target.id == "tbxReviewer") {
                        if ($("#tbxReviewer").val() != "") {
                            var Reviewer = getApplier_ajax($("#tbxReviewer").val());
                            if (Reviewer != "") {
                                $("#lblReviewerName").html(Reviewer);
                                $("#lblReviewerName").css("color", "black");
                                $("#lblReviewerName").css("font-style", "normal");
                            }
                            else {
                                $("#lblReviewerName").html("查無同工資料");
                                $("#lblReviewerName").css("color", "red");
                                $("#lblReviewerName").css("font-style", "italic");
                            }

                        }
                        else {
                            $("#lblReviewerName").html("");
                            $("#lblReviewerName").css("color", "red");
                        }
                        return false;
                    }
                    else if (e.target.id == "tbxActionPerformer") {
                        if ($("#tbxActionPerformer").val() != "") {
                            var ActionPerformer = getApplier_ajax($("#tbxActionPerformer").val());
                            if (ActionPerformer != "") {
                                $("#lblActionPerformerName").html(ActionPerformer);
                                $("#lblActionPerformerName").css("color", "black");
                                $("#lblActionPerformerName").css("font-style", "normal");
                            }
                            else {
                                $("#lblActionPerformerName").html("查無同工資料");
                                $("#lblActionPerformerName").css("color", "red");
                                $("#lblActionPerformerName").css("font-style", "italic");
                            }

                        }
                        else {
                            $("#lblActionPerformerName").html("");
                            $("#lblActionPerformerName").css("color", "red");
                        }
                        return false;
                    }
                    else {
                        return false;
                    }
                }

            });

            //審核人
            $("#tbxReviewer").blur(function () {

                if ($("#tbxReviewer").val() != "") {
                    var Reviewer = getApplier_ajax($("#tbxReviewer").val());
                    if (Reviewer != "") {
                        $("#lblReviewerName").html(Reviewer);
                        $("#lblReviewerName").css("color", "black");
                        $("#lblReviewerName").css("font-style", "normal");
                    }
                    else {
                        $("#lblReviewerName").html("查無同工資料");
                        $("#lblReviewerName").css("color", "red");
                        $("#lblReviewerName").css("font-style", "italic");
                    }

                }
                else {
                    $("#lblReviewerName").html("");
                    $("#lblReviewerName").css("color", "red");
                }

            });

            //片庫作業同工
            $("#tbxActionPerformer").blur(function () {

                if ($("#tbxActionPerformer").val() != "") {
                    var ActionPerformer = getApplier_ajax($("#tbxActionPerformer").val());
                    if (ActionPerformer != "") {
                        $("#lblActionPerformerName").html(ActionPerformer);
                        $("#lblActionPerformerName").css("color", "black");
                        $("#lblActionPerformerName").css("font-style", "normal");
                    }
                    else {
                        $("#lblActionPerformerName").html("查無同工資料");
                        $("#lblActionPerformerName").css("color", "red");
                        $("#lblActionPerformerName").css("font-style", "italic");
                    }

                }
                else {
                    $("#lblActionPerformerName").html("");
                    $("#lblActionPerformerName").css("color", "red");
                }

            });

        });

        //申請作廢
        function CheckInvalid() {

            if ($("#tbxReviewer").val() == "" || $("#lblReviewerName").html() == "查無同工資料") {
                alert("請輸入審核者工號");
                return false;
            }
            if (confirm('確定要作廢？')) {
                return true;
            } else {
                return false;
            }

        }

        //核可
        function CheckApprove() {

            if ($("#tbxReviewer").val() == "" || $("#lblReviewerName").html() == "查無同工資料") {
                alert("請輸入審核者工號");
                return false;
            }
            if (confirm('確定要核可？')) {
                var exit = true;
                while (exit) {
                    var password = prompt('請輸入審核密碼', '');
                    if (password == null) {
                        exit = false;
                    }
                    else if (password != '') {
                        var ok = checkAuditPassword_ajax(password);
                        if (ok)
                            return true;
                        else {
                            alert('密碼錯誤');
                        }
                    }
                    else {
                        alert('請輸入審核密碼');
                    }
                }
                return false;
            } else {
                return false;
            }

        }

        //關閉
        function goExit() {
            $('#btnExit').css("cursor", "progress");
            return true;
        }

        //儲存
        function CheckEditSave() {

            if ($("#tbxActionPerformer").val() == "" || $("#lblActionPerformerName").html() == "查無同工資料") {
                alert('請輸入處理者工號');
                return false;
            }
    
            //return false;
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

        //檢查密碼
        function checkAuditPassword_ajax(Password) {

            var strData = "{'Password':'" + Password + "'}";
            var ok = false;

            $.ajax({
                type: "POST",
                url: "../SubtitleWebService.asmx/CheckAuditPassword",
                data: strData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (json) {
                    ok = json.d;
                },
                failure: function (ex) {
                    alert(ex.get_message);
                }
            });

            return ok;

        }

    </script>
    <style type="text/css">

        body {
            font-family: Verdana, Tahoma, Arial, Helvetica, sans-serif;
        }

        .table-detail  tr td {
            height: 30px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Uid" />
        <asp:HiddenField runat="server" ID="HFD_Mode" />
        <asp:HiddenField runat="server" ID="HFD_ApplyQuery_Key" />

        <table width="100%" cellpadding="0" cellspacing="1">
            <tr>
                <td width="20%"></td>
                <td width="60%">

        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table-detail"> 
            <tr style="height: 50px">
               <td colspan="4"><span style="font-size:26px; font-weight: bold;">節目異動申請內容</span><span style="font-size:18px;"> 
                   <asp:Label ID="lblStatus" runat="server"></asp:Label></span></td>
            </tr>
            <tr>
                <th align="left">
                    單號
                </th>
                <td align="left" colspan="3">
                    <asp:Label runat="server" ID="lblChangeID"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="left">
                    申請人
                </th>
                <td align="left" colspan="3">
                    <asp:Label runat="server" ID="lblRequester"></asp:Label>
                    <asp:Label runat="server" ID="lblRequesterName"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="left">
                    異動日期
                </th>
                <td align="left" colspan="3">
                    <asp:Label runat="server" ID="lblChangeDate"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="left">
                    異動頻道
                </th>
                <td align="left" colspan="3">
                    <asp:Label runat="server" ID="lblChannel"></asp:Label>
                </td>
            </tr>
            <tr height="100px">
                <th align="left">
                    異動內容
                </th>
                <td align="left" colspan="3" >
                    <asp:Label runat="server" ID="lblContent"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="left" width="15%">
                    審核人
                </th>
                <td align="left" Width="35%">
                    <asp:Textbox runat="server" ID="tbxReviewer" Width="160px"></asp:Textbox> 
                    <asp:Label runat="server" ID="lblReviewer"></asp:Label>
                    <asp:Label runat="server" ID="lblReviewerName"></asp:Label>
                </td>
                <th align="left" width="15%">
                    片庫作業同工
                </th>
                <td align="left" Width="35%">
                    <asp:Textbox runat="server" ID="tbxActionPerformer" Width="160px"></asp:Textbox>
                    <asp:Label runat="server" ID="lblActionPerformer"></asp:Label>
                    <asp:Label runat="server" ID="lblActionPerformerName"></asp:Label>
                </td>
            </tr>
            <tr Height="140px">
                <th align="left">
                    審核內容
                </th>
                <td Width="260px">
                    <asp:Label runat="server" ID="lblReviewComment"></asp:Label>
                    <asp:Textbox runat="server" ID="tbxReviewComment" TextMode="MultiLine" Width="260px" Height="120px"></asp:Textbox>
                </td>
                <th align="left">
                    片庫作業結果
                </th>
                <td Width="260px">
                    <asp:Label runat="server" ID="lblActionContent"></asp:Label>
                    <asp:Textbox runat="server" ID="tbxActionContent" TextMode="MultiLine" Width="260px" Height="120px"></asp:Textbox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="4">
                    <asp:Button ID="btnDel" runat="server" class="ui-button ui-corner-all" Text="取消申請" Width="80px" onclick="btnDel_Click"  OnClientClick= "return confirm('確定要取消申請？ '); "/>
                    <asp:Button ID="btnInvalid" runat="server" class="ui-button ui-corner-all" Text="申請作廢" Width="80px" onclick="btnInvalid_Click"  OnClientClick= "return CheckInvalid();"/>
                    <asp:Button ID="btnApprove" runat="server" class="ui-button ui-corner-all" Text="核可" Width="80px" OnClick="btnApprove_Click" OnClientClick="return CheckApprove();"/>
                    <asp:Button ID="btnEdit" runat="server" class="ui-button ui-corner-all" Text="儲存" Width="70px" onclick="btnEdit_Click" OnClientClick= "return CheckEditSave();"/>
                    <asp:Button ID="btnExit" runat="server" class="ui-button ui-corner-all" Text="關閉" Width="70px" OnClick="btnExit_Click" OnClientClick= "return goExit();"/>
                </td>
            </tr>
        </table>

                </td>
                <td width="20%"></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
