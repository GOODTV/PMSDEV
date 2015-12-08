<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShortFilmApplyQueryDetail.aspx.cs" Inherits="ShortFilm_ShortFilmApplyQueryDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=11"/>
    <title>短片申請明細</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField runat="server" ID="HFD_Uid" />
        <asp:HiddenField runat="server" ID="HFD_Key" />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table-detail"> 
            <tr style="HEIGHT: 25px">
               <td style="text-align: left; background-color: #5EABDB; color: #FFFFFF;" colspan="5">短片申請明細</td>
            </tr>
            <tr>
                <th align="right" Width="400px">
                    短片編號：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxCFID" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    節目代號：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxProgramID" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    節目集數：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxEpisode" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    短片名稱：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxCFTitle" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    短片長度：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxLength" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    HDC：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxHDC" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    播出頻道：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxChannel" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    內容摘要說明：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxCFDescription" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    排播時間：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxOnAirDate" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    排播說明：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxOnAirRemark" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    短片註記(限100字)：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxAppendance" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    排播頻率：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxFrequency" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    排播時段：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxOnAirTimeSlot" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    提供官網：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxForWeb" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    申請同工：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxRequesterID" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="right">
                    片庫同工：
                </th>
                <td align="left">
                    <asp:Label runat="server" ID="tbxMediaUser" CssClass="font9"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="btnExit" runat="server" class="ui-button ui-corner-all" Text="關閉" Width="70px" onclick="btnExit_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
