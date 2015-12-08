<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SRTPersonnelAdd.aspx.cs" Inherits="View_SRTPersonnelAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>字幕人員新增</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript">
        function CheckFieldMustFillBasic() {
            var strRet = "";
            var tbxEditorID = document.getElementById('tbxEditorID');
            var tbxEditorName = document.getElementById('tbxEditorName');
            var tbxID = document.getElementById('tbxID');
            var tbxLandPhone = document.getElementById('tbxLandPhone');
            var tbxMobilePhone = document.getElementById('tbxMobilePhone');
            var tbxAddress = document.getElementById('tbxAddress');

            if (tbxEditorID.value == "") {
                strRet += "字幕人員代號不可空白";
                alert(strRet);
                return false;
            }
            if (tbxEditorName.value == "") {
                strRet += "字幕人員姓名不可空白";
                alert(strRet);
                return false;
            }
            if (tbxID.value == "") {
                strRet += "身份證/統一編號不可空白";
                alert(strRet);
                return false;
            }
            if (tbxLandPhone.value == "" && tbxMobilePhone.value == "") {
                strRet = "連絡電話、手機號碼必須擇一填入 ";
                alert(strRet);
                return false;
            }
            if (tbxAddress.value == "") {
                strRet += "戶籍地址不可空白";
                alert(strRet);
                return false;
            }

            return true;
        }
    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table-detail"> 
            <tr style="HEIGHT: 25px">
               <td style="text-align: left; background-color: #5EABDB; color: #FFFFFF;" colspan="2">字幕人員編輯</td>
            </tr>
            <tr>
                <td align="right"><font color="red">字幕人員代號</font>
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxEditorID" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right"><font color="red">字幕人員姓名</font>
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxEditorName" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td align="left" colspan="2">
                    <asp:CheckBoxList ID="cbldepart" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td align="right"><font color="red">身份證/統一編號</font>
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxID" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right"><font color="red">連絡電話<br /></font>
                    格式: (02)2631-0101#1234
                </td>
                <td align="left" colspan="5" class="style1">
                    <asp:TextBox runat="server" ID="tbxLandPhone" CssClass="font9" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right"><font color="red">手機號碼</font><br />
                    格式: 0935-123-456
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxMobilePhone" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right"><font color="red">戶籍地址</font>
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxAddress" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    電子郵件1
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxEmail_1" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    電子郵件2
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxEmail_2" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    評比
                </td>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="tbxComparison" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="6">
                    <asp:Button ID="btnAdd" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="儲存" OnClientClick= "return CheckFieldMustFillBasic();"  OnClick="btnAdd_Click"/>
                    <asp:Button ID="btnExit" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="離開" onclick="btnExit_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
