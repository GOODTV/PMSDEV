<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadEPG.aspx.cs" Inherits="UploadEPG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Script/JScript.js"></script>
    <!--引用jQuery blockUI套件-->
    <script src="../Script/jquery.blockUI.js" type="text/javascript"></script>   
    <script type="text/javascript">
        function showBlockUI() {
            //顯示上傳中的圖片            
            $.blockUI({ message: '檔案匯入處理中,請勿關閉瀏覽器!!!<img src="../App_Themes/image/PageBlock.gif" />' });
            $("#ContentPlaceHolder1_btnImport").click(); //執行上傳            
            //$.unblockUI();/*因為會postback頁面刷新，所以有沒有unblockUI沒差*/
        }

        function validate(elem) {
            var str = elem.value;
            var extIndex = str.lastIndexOf('\\');
            if (extIndex != -1)
                var name = str.substr(extIndex + 1, str.length);
            else
                var name = str;
            if (name.substr(0, 3).toLowerCase() == "epg") {
                return true;
            } else {
                alert("所選擇的檔案不是EPG格式的檔案");
                elem.focus();
                return false;
            }

        }

    </script>
</head>
<body>
    <form id="form1" runat="server">        
    <asp:Table ID="Table1" runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell Width="8%" HorizontalAlign = "Right">
                <asp:Label ID="lblImportFile" runat="server" CssClass="tableLabel" Text="匯入檔案：" />
            </asp:TableCell>
            <asp:TableCell Width="92%" HorizontalAlign = "Left">
                <asp:FileUpload ID="fuFile" runat="server" Width="500px" onchange="validate(this)" />&nbsp;
                <input type="button" value="匯入" onclick="showBlockUI();" class="Button" />                
                <div style="display:none;"> 
                    <asp:Button ID="btnImport" runat="server" CssClass="Button" Text="匯入" OnClick="btnImport_Click" />
                </div>
                <asp:Button ID="btnQueryEPG" runat="server" Text="EPG查詢" OnClick="btnQueryEPG_Click" CssClass="Button" />
            </asp:TableCell>            
        </asp:TableRow>
</asp:Table>
    </form>
</body>
</html>
