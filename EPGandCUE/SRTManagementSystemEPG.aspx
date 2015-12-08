<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SRTManagementSystemEPG.aspx.cs" Inherits="SRTManagementSystemEPG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <!--<script type="text/javascript" src="../include/common.js"></script>-->

    <script type="text/javascript">

        function validate() {

            if ($("#StartDate_box").val() == "" || $("#EndDate_box").val() == "") {
                alert("未輸入開始日期或結束日期！");
                return false;
            }
            var dt1 = new Date($("#StartDate_box").val());
            var dt2 = new Date($("#EndDate_box").val());

            if ((dt2 - dt1) / (1000 * 60 * 60 * 24) >= 62) {
                alert("開始日期至結束日期的範圍太大！請選擇二個月內範圍。");
                return false;
            }
            else
                return true;
            return false;

        }

        $().ready(function () {
            $("#StartDate_box").datepicker();
            $("#EndDate_box").datepicker();

            // 取得資料表格高度
            $("#GridPanel").css('height', $(window).height() - (Math.round(($("#GridPanel").position().top) + 20)));

            $(window).resize(function () {
                $("#GridPanel").css('height', $(window).height() - (Math.round(($("#GridPanel").position().top) + 20)));
            });

        });

    </script>
    <style type="text/css">

        .table-list tr th {
            white-space: pre-wrap;
            min-width: 30px;
            word-break: break-all;
        }

        .table-list tr td {
            min-width: 30px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">        
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True" />

    <asp:Label ID="StartDate_txt" runat="server" Text="開始日期"></asp:Label>
    <asp:TextBox ID="StartDate_box" runat="server" ></asp:TextBox>

    <asp:Label ID="EndDate_txt" runat="server" Text="結束日期"></asp:Label>
    <asp:TextBox ID="EndDate_box" runat="server" ></asp:TextBox>

    <asp:Label ID="Channel_txt" runat="server" Text="頻道別"></asp:Label>
    <asp:DropDownList ID="Channel_box" runat="server" Width="100px">
        <asp:ListItem Value="">請選擇</asp:ListItem>
        <asp:ListItem Value="1">GOODTV1</asp:ListItem>
        <asp:ListItem Value="2">GOODTV2</asp:ListItem>
    </asp:DropDownList>

    <asp:Label ID="PlanningTitle_txt" runat="server" Text="節目代號"></asp:Label>
    <asp:TextBox ID="PlanningTitle_box" runat="server" Width="80px" />
    
    <asp:Label ID="EpisodeNumber_txt" runat="server" Text="集數"></asp:Label>
    <asp:TextBox ID="EpisodeNumber_box" runat="server" Width="40px" />

    <asp:Label ID="HouseNo_txt" runat="server" Text="HouseNumber"></asp:Label>
    <asp:TextBox ID="HouseNo_box" runat="server" Width="110px" />

    <asp:Label ID="Premier_txt" runat="server" Text="首/重播"></asp:Label>
     <asp:DropDownList ID="Premier_box" runat="server">
        <asp:ListItem Value="">請選擇</asp:ListItem>
        <asp:ListItem Value="1">首播</asp:ListItem>
        <asp:ListItem Value="0">重播</asp:ListItem>
    </asp:DropDownList>

    <p/>

    <asp:Button ID="btnQuery" class="ui-button ui-corner-all" runat="server" Text="查詢" OnClick="btnQuery_Click" OnClientClick="return validate();" />
    <asp:Button ID="impEPG" class="ui-button ui-corner-all" runat="server" Text="匯入EPG" OnClick="btnEPG_Click" />

    <p/>

    <asp:Panel ID="InfoPanel" runat="server" Visible="false">
        <asp:Label ID="lblGrid" runat="server" Text="查詢結果"></asp:Label>
        <asp:Label ID="lblGridCount" runat="server" ForeColor="Red" Font-Size="Small" />
        &nbsp<font color="#00CED1" size="5px">■</font><font size="2px">每日第1筆(早上5點)</font>
        &nbsp<font color="#FFFF00" size="5px">■</font><font size="2px">匯入資料格式錯誤</font>
    </asp:Panel>

    <asp:Panel ID="GridPanel" runat="server" style="height: 600px; overflow: auto;">

        <asp:GridView ID="gridEPGList" runat="server" Width="100%" 
            GridLines="Vertical" AllowPaging="False" AutoGenerateColumns="False"
            EmptyDataRowStyle-VerticalAlign="Top" CssClass="table-list">
            
            <Columns>
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="頻道別"        DataField="Channel" HeaderStyle-Width="65px" />                
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="播映日期"      DataField="CalendarDate" HeaderStyle-Width="75px" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="播出時間"      DataField="CalendarTime" HeaderStyle-Width="90px" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%"  HeaderText="HouseNumber"   DataField="HouseNo" HeaderStyle-Width="100px" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%"  HeaderText="節目代號"      DataField="PlanningTitleID" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%" HeaderText="節目名稱"      DataField="PlanningTitleName" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="4%"  HeaderText="集數"          DataField="EpisodeNumber" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="1%"  HeaderText="首/重/Live"    DataField="PremierRepeat" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%"  HeaderText="播映長度(分)"  DataField="Duration" />            
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="11%" HeaderText="系列名稱"      DataField="EpisodeTitle1" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="12%" HeaderText="分集名稱"      DataField="EpisodeTitle2" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="6%"  HeaderText="講員姓名"      DataField="EpisodeTitle3" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="6%"  HeaderText="字幕已疊映"    DataField="TypeCommnet" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%"  HeaderText="字幕上傳記錄"  DataField="UploadRecord" />
            </Columns>

            <EmptyDataTemplate>
                <table class="table-list">
                    <tr>
                        <th width="7%">頻道別</th>
                        <th width="7%">播映日期</th>
                        <th width="7%">播出時間</th>
                        <th width="8%">HouseNumber</th>
                        <th width="5%">節目代號</th>
                        <th width="10%">節目名稱</th>
                        <th width="4%">集數</th>
                        <th width="1%">首/重/Live</th>
                        <th width="5%">播映長度(分)</th>                                              
                        <th width="11%">系列名稱</th>  
                        <th width="12%">分集名稱</th>
                        <th width="6%">講員姓名</th>                     
                        <th width="6%">字幕已疊映</th>         
                        <th width="8%">字幕上傳記錄</th>    
                    </tr>
                    <tr>
                        <td colspan="14">無資料</td>
                    </tr> 
                </table>
            </EmptyDataTemplate>       
            
            <HeaderStyle HorizontalAlign="Left" />
            <PagerSettings FirstPageImageUrl="~/App_Themes/image/arrow_top.gif" 
                LastPageImageUrl="~/App_Themes/image/arrow_down.gif" 
                Mode="NextPreviousFirstLast" 
                NextPageImageUrl="~/App_Themes/image/arrow_next.gif" 
                PreviousPageImageUrl="~/App_Themes/image/arrow_back.gif" />
            <PagerStyle HorizontalAlign="Center" />
                        
        </asp:GridView>

        <asp:GridView ID="gridEPGList2" runat="server" Width="100%" 
            GridLines="Vertical" AllowPaging="False" AutoGenerateColumns="False"
            EmptyDataRowStyle-VerticalAlign="Top" CssClass="table-list">
            
            <Columns>
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%"  HeaderText="節目代號"      DataField="PlanningTitleID" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%" HeaderText="節目名稱"      DataField="PlanningTitleName" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="4%"  HeaderText="集數"          DataField="EpisodeNumber" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%"  HeaderText="HouseNumber"   DataField="HouseNo" HeaderStyle-Width="100px" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="頻道別"        DataField="Channel" HeaderStyle-Width="65px" />                
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="播映日期"      DataField="CalendarDate" HeaderStyle-Width="75px" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="播出時間"      DataField="CalendarTime" HeaderStyle-Width="90px" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="1%"  HeaderText="首/重/Live"    DataField="PremierRepeat" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%"  HeaderText="播映長度(分)"  DataField="Duration" />            
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="11%" HeaderText="系列名稱"      DataField="EpisodeTitle1" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="14%" HeaderText="分集名稱"      DataField="EpisodeTitle2" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="6%"  HeaderText="講員姓名"      DataField="EpisodeTitle3" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="6%"  HeaderText="字幕已疊映"    DataField="TypeCommnet" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%"  HeaderText="字幕上傳記錄"  DataField="UploadRecord" />
            </Columns>

            <EmptyDataTemplate>
                <table class="table-list">
                    <tr>
                        <th width="5%">節目代號</th>
                        <th width="10%">節目名稱</th>
                        <th width="4%">集數</th>
                        <th width="8%">HouseNumber</th>
                        <th width="7%">頻道別</th>
                        <th width="7%">播映日期</th>
                        <th width="7%">播出時間</th>
                        <th width="1%">首/重/Live</th>
                        <th width="5%">播映長度(分)</th>
                        <th width="11%">系列名稱</th>                                              
                        <th width="14%">分集名稱</th>
                        <th width="6%">講員姓名</th>   
                        <th width="6%">字幕已疊映</th>
                        <th width="8%">字幕上傳記錄</th>                      
                    </tr>
                    <tr>
                        <td colspan="14">無資料</td>
                    </tr> 
                </table>
            </EmptyDataTemplate>

            <HeaderStyle HorizontalAlign="Left" />
            <PagerSettings FirstPageImageUrl="~/App_Themes/image/arrow_top.gif" 
                LastPageImageUrl="~/App_Themes/image/arrow_down.gif" 
                Mode="NextPreviousFirstLast" 
                NextPageImageUrl="~/App_Themes/image/arrow_next.gif" 
                PreviousPageImageUrl="~/App_Themes/image/arrow_back.gif" />
            <PagerStyle HorizontalAlign="Center" />
                        
        </asp:GridView>

    </asp:Panel>
    </form>
</body>
</html>

