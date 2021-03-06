﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReleaseMasterAdd.aspx.cs" Inherits="View_ReleaseMasterAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>交檔客戶新增</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        document.onkeydown = function () {
            if (window.event)
                if (event.keyCode == 13 && event.srcElement.nodeName != "TEXTAREA" && event.srcElement.type != "submit")
                    event.keyCode = 9;
        }
        function CheckFieldMustFillBasic() {
            var strRet = "";
            var cnt = 0;
            var sName = "";
            var txtCustomerID = document.getElementById('txtCustomerID');
            var txtCustomerName = document.getElementById('txtCustomerName');
            var txtPhone_1 = document.getElementById('txtPhone_1');
            var txtMobile_1 = document.getElementById('txtMobile_1');
            var txtPhone_2 = document.getElementById('txtPhone_2');
            var txtMobile_2 = document.getElementById('txtMobile_2');

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
            if (cnt != 3) {
                alert('客戶代號 長度為3碼！');
                return false;
            }
            if (txtCustomerName.value == "") {
                strRet += "客戶名稱不可空白";
                alert(strRet);
                return false;
            }
            /*if (isNaN(Number(txtPhone_1.value)) == true) {
                alert('聯絡人電話1 必須為數字！');
                txtPhone_1.focus();
                return false;
            }
            if (isNaN(Number(txtMobile_1.value)) == true) {
                alert('聯絡人手機1 必須為數字！');
                txtMobile_1.focus();
                return false;
            }
            if (isNaN(Number(txtPhone_2.value)) == true) {
                alert('聯絡人電話2 必須為數字！');
                txtPhone_2.focus();
                return false;
            }
            if (isNaN(Number(txtMobile_2.value)) == true) {
                alert('聯絡人手機2 必須為數字！');
                txtMobile_2.focus();
                return false;
            }*/
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table-detail"> 
            <tr style="HEIGHT: 25px">
               <td style="text-align: left; background-color: #5EABDB; color: #FFFFFF;" colspan="6">交檔客戶編輯</td>
            </tr>
            <tr>
                <th align="right" colspan="1"><font color="red">*</font>
                    客戶代號：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtCustomerID" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
                <th align="right"><font color="red">*</font>
                    客戶名稱：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtCustomerName" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    聯絡人姓名1：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtName_1" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
                <th align="right">
                    聯絡人電話1：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtPhone_1" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    聯絡人手機1：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtMobile_1" CssClass="font9" Width="150px"></asp:TextBox>手機格式：09XX-XXX-XXX
                </td>
                <th align="right">
                    聯絡人電郵1：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtEmail_1" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    聯絡人姓名2：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtName_2" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
                <th align="right">
                    聯絡人電話2：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtPhone_2" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    聯絡人手機2：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtMobile_2" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
                <th align="right">
                    聯絡人電郵2：
                </th>
                <td align="left" colspan="2" class="style1">
                    <asp:TextBox runat="server" ID="txtEmail_2" CssClass="font9" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    供片間隔：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:RadioButtonList ID="rblInterval" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="月" Value="1"></asp:ListItem>
                        <asp:ListItem Text="季" Value="2"></asp:ListItem>
                        <asp:ListItem Text="年" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th align="right">
                    備註：
                </th>
                <td align="left" colspan="5" class="style1">
                    <asp:TextBox runat="server" ID="txtRemark" CssClass="font9" Width="400px" TextMode="MultiLine" Height="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="6">
                    <asp:Button ID="btnAdd" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="儲存" OnClientClick= "return CheckFieldMustFillBasic();" OnClick="btnAdd_Click"/>
                    <asp:Button ID="btnExit" runat="server" class="ui-button ui-corner-all" Width="20mm" Text="離開" onclick="btnExit_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
