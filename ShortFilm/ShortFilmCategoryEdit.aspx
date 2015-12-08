<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShortFilmCategoryEdit.aspx.cs" Inherits="ShortFilm_ShortFilmCategoryEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11"/>
    <title>短片類別編輯</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#txtSetDate").datepicker();

        });

        function CheckFieldMustFillBasic() {
            var strRet = "";
            var cnt = 0;
            var sName = "";
            var txtCategory = document.getElementById('txtCategory');
            var txtDescription = document.getElementById('txtDescription');
            var txtSetDate = document.getElementById('txtSetDate');

            if (txtCategory.value == "") {
                strRet += "短片類別不可空白";
                alert(strRet);
                return false;
            }
            if (txtDescription.value == "") {
                strRet += "短片類別說明不可空白";
                alert(strRet);
                return false;
            }
            if (txtSetDate.value == "") {
                strRet += "設定日期不可空白";
                alert(strRet);
                return false;
            }
            cnt = 0;
            sName = txtCategory.value;
            for (var i = 0; i < sName.length; i++) {
                if (escape(sName.charAt(i)).length >= 4) cnt += 2;
                else cnt++;
            }
            if (cnt > 3) {
                alert('短片類別 欄位長度超過限制！');
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Uid" />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table-detail"> 
            <tr style="HEIGHT: 25px">
               <td style="text-align: left; background-color: #5EABDB; color: #FFFFFF;" colspan="5">短片類別編輯</td>
            </tr>
            <tr>
                <th align="right"><font color="red">*</font>
                    短片類別：
                </th>
                <td align="left">
                    <asp:TextBox runat="server" ID="txtCategory" CssClass="font9" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right"><font color="red">*</font>
                    短片類別說明：
                </th>
                <td align="left">
                    <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Width="400px" Height="50px" CssClass="font9"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    申請多號：
                </th>
                <td align="left">
                    <asp:CheckBox ID="cbxRequestMultipleNumber" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="right">
                    必須有節目：
                </th>
                <td align="left">
                    <asp:CheckBox ID="cbxProgramExist" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="right">
                    必須有集數：
                </th>
                <td align="left">
                    <asp:CheckBox ID="cbxEpisodeExist" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="right"><font color="red">*</font>
                    設定日期：
                </th>
                <td align="left">
                    <asp:TextBox runat="server" ID="txtSetDate" CssClass="font9" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="btnDel" runat="server" class="ui-button ui-corner-all" Text="刪除" Width="70px" onclick="btnDel_Click" OnClientClick= "return confirm('確定要刪除？'); "/>
                    <asp:Button ID="btnEdit" runat="server" class="ui-button ui-corner-all" Text="儲存" Width="70px" onclick="btnEdit_Click" OnClientClick= "return CheckFieldMustFillBasic(); "/>
                    <asp:Button ID="btnExit" runat="server" class="ui-button ui-corner-all" Text="取消" Width="70px" onclick="btnExit_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
