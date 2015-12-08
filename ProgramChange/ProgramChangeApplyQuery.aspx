<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgramChangeApplyQuery.aspx.cs" Inherits="ProgramChange_ProgramChangeApplyQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>節目異動查詢</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <!--<script type="text/javascript" src="../include/common.js"></script>-->
    <script type="text/javascript">

        $(document).ready(function () {

            //排除按Enter會誤動作
            $("body").bind("keypress", function (e) {
                
                if (e.which == 13) {
                    if (e.target.id == "tbxRequesterID") {
                    }
                    else {
                        return false;
                    }
                }

            });

            $('#tbxChangeDateS').datepicker();
            $('#tbxChangeDateE').datepicker();


        });

        function CheckFieldMustFillBasic() {

            if ($('#HFD_Mode').val() == "applier") {
                if ($('#tbxRequesterID').val() == "") {
                    alert('請輸入申請人員工編號');
                    return false;
                }
            }

            var ChangeDateS = $('#tbxChangeDateS').val();
            var ChangeDateE = $('#tbxChangeDateE').val();

            if (ChangeDateS != "") {
                if (!dateValidationCheck(ChangeDateS)) {
                    alert('異動日期起日錯誤');
                    return false;
                }
            }
            if (ChangeDateE != "") {
                if (!dateValidationCheck(ChangeDateE)) {
                    alert('異動日期迄日錯誤');
                    return false;
                }
            }
            if (ChangeDateS != "" && ChangeDateE != "") {
                if (!checkDateStartEnd($('#tbxChangeDateS').val(), $('#tbxChangeDateE').val())) {
                    alert('異動日期範圍錯誤');
                    return false;
                }
            }
            
            $('#btnQuery').css("cursor", "progress");
        }

        </script>
</head>
<body style="overflow:auto;">
    <form id="form1" runat="server">
    <div>
    <asp:HiddenField runat="server" ID="HFD_Mode" />
    <asp:HiddenField runat="server" ID="HFD_ApplyQuery_Key" />
        <table width="100%" border="0" cellpadding="0" cellspacing="1">
            <tr>
                <td nowrap>
                    <asp:Label ID="lblRequesterID" runat="server" Text="申請人"></asp:Label>
                    <asp:TextBox ID="tbxRequesterID" runat="server" Width="100px" />
                    異動日期
                    <asp:TextBox runat="server" ID="tbxChangeDateS" Width="100px"></asp:TextBox>
                    至
                    <asp:TextBox runat="server" ID="tbxChangeDateE" Width="100px"></asp:TextBox>
                </td>
                <td nowrap>
                    <asp:RadioButtonList ID="rblValidStatus" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Selected="True">未審核</asp:ListItem>
                        <asp:ListItem Value="2">未處理</asp:ListItem>
                        <asp:ListItem Value="3">全部</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td nowrap>
                    <asp:Button ID="btnQuery" class="ui-button ui-corner-all" runat="server" Width="15mm" Text="查詢" OnClientClick="return CheckFieldMustFillBasic();" OnClick="btnQuery_Click" />
                    資料筆數：<asp:Label ID="count" runat="server"></asp:Label>
                </td>
                <td width="80%">&nbsp;</td>
            </tr>
            <tr>
                <td align="center" width="100%" colspan="4">
                    <table class='table-list' width='100%'>
                        <tr>
                            <th width='10px'></th>
                            <th width='130px'><span>單號</span></th>
                            <th width='100px'><span>異動日期</span></th>
                            <th width='50px'><span>異動頻道</span></th>
                            <th nowrap><span>異動內容</span></th>
                            <th width='100px'><span>申請人</span></th>
                            <th width='100px'><span>申請日期</span></th>
                            <th width='100px'><span>審核人</span></th>
                            <th width='100px'><span>審核日期</span></th>
                            <th width='100px'><span>片庫作業同工</span></th>
                        </tr>
                        <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
