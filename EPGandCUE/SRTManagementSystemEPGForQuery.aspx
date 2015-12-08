<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SRTManagementSystemEPGForQuery.aspx.cs" Inherits="SRTManagementSystemEPG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../Script/JScript.js"></script>
    <!--引用jQuery blockUI套件-->
    <script src="../Script/jquery.blockUI.js" type="text/javascript"></script>   
     <script type="text/javascript">
         window.onload = initCalendar;
         function initCalendar() {
             Calendar.setup({
                 inputField: "StartDate_box",   // id of the input field
                 button: "imgStartDate"     // 與觸發動作的物件ID相同
             });
             Calendar.setup({
                 inputField: "EndDate_box",   // id of the input field
                 button: "imgEndDate"     // 與觸發動作的物件ID相同
             });
         }
    </script>

</head>
<body>
    <form id="form1" runat="server">        
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True" />

    <asp:Label ID="StartDate_txt" runat="server" Text="開始日期" CssClass="gridTitle"></asp:Label>
    <asp:TextBox ID="StartDate_box" runat="server" onchange="CheckDateFormat(this, '開始日期');"></asp:TextBox>
    <img id="imgStartDate" alt="" src="../img/date.gif" />

    <asp:Label ID="EndDate_txt" runat="server" Text="結束日期" CssClass="gridTitle"></asp:Label>
    <asp:TextBox ID="EndDate_box" runat="server" onchange="CheckDateFormat(this, '結束日期');"></asp:TextBox>
    <img id="imgEndDate" alt="" src="../img/date.gif" />

    <asp:Label ID="Channel_txt" runat="server" Text="頻道別" CssClass="gridTitle"></asp:Label>
    <asp:DropDownList ID="Channel_box" runat="server">
        <asp:ListItem Value="">請選擇</asp:ListItem>
        <asp:ListItem Value="1">GOODTV1</asp:ListItem>
        <asp:ListItem Value="2">GOODTV2</asp:ListItem>
    </asp:DropDownList>

    <asp:Label ID="PlanningTitle_txt" runat="server" Text="節目代號" CssClass="gridTitle"></asp:Label>
    <asp:TextBox ID="PlanningTitle_box" runat="server" Width="70px" />
    
    <asp:Label ID="EpisodeNumber_txt" runat="server" Text="集數" CssClass="gridTitle"></asp:Label>
    <asp:TextBox ID="EpisodeNumber_box" runat="server" Width="40px" />

    <asp:Label ID="HouseNo_txt" runat="server" Text="HouseNumber" CssClass="gridTitle"></asp:Label>
    <asp:TextBox ID="HouseNo_box" runat="server" Width="70px" />

    <asp:Label ID="Premier_txt" runat="server" Text="首/重播" CssClass="gridTitle"></asp:Label>
     <asp:DropDownList ID="Premier_box" runat="server">
        <asp:ListItem Value="">請選擇</asp:ListItem>
        <asp:ListItem Value="1">首播</asp:ListItem>
        <asp:ListItem Value="0">重播</asp:ListItem>
    </asp:DropDownList>

    <p/>

    <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" CssClass="Button" />

    <p/>

    <asp:Panel ID="InfoPanel" runat="server" Visible="false">
        <asp:Label ID="lblGrid" runat="server" Text="查詢結果" CssClass="gridTitle"></asp:Label>
        <asp:Label ID="lblGridCount" runat="server" ForeColor="Red" Font-Size="Small" />
        &nbsp<font color="#00CED1" size="5px">■</font><font size="2px">每日第1筆(早上5點)</font>
        &nbsp<font color="#FFFF00" size="5px">■</font><font size="2px">匯入資料格式錯誤</font>
    </asp:Panel>

    <asp:Panel ID="GridPanel" runat="server">

        <asp:GridView ID="gridEPGList" runat="server" Width="100%" 
            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
            CellPadding="3" GridLines="Vertical" Font-Size="13px"            
            OnRowDataBound="gridEPGList_RowDataBound"
            OnPageIndexChanging="gridEPGList_PageIndexChanging" 
            OnPageIndexChanged="gridEPGList_PageIndexChanged"
            AllowPaging="True" AutoGenerateColumns="false" PageSize="6000">
            
            <AlternatingRowStyle BackColor="#DCDCDC" />

            <Columns>
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="頻道別"        DataField="Channel" />                
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="播映日期"      DataField="CalendarDate" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="播出時間"      DataField="CalendarTime" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%"  HeaderText="HouseNumber"   DataField="HouseNo" />
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
                <table style="width:100%">
                    <tr >
                        <td width="7%"  class="gridHead">頻道別</td>
                        <td width="7%"  class="gridHead">播映日期</td>
                        <td width="7%"  class="gridHead">播出時間</td>
                        <td width="8%"  class="gridHead">HouseNumber</td>
                        <td width="5%"  class="gridHead">節目代號</td>
                        <td width="10%" class="gridHead">節目名稱</td>
                        <td width="4%"  class="gridHead">集數</td>
                        <td width="1%"  class="gridHead">首/重/Live</td>
                        <td width="5%"  class="gridHead">播映長度(分)</td>                                              
                        <td width="11%" class="gridHead">系列名稱</td>  
                        <td width="12%" class="gridHead">分集名稱</td>
                        <td width="6%"  class="gridHead">講員姓名</td>                     
                        <td width="6%"  class="gridHead">字幕已疊映</td>         
                        <td width="8%"  class="gridHead">字幕上傳記錄</td>    
                    </tr>
                    <tr>
                        <td colspan="14" class="gridText">無資料</td>
                    </tr> 
                </table>
            </EmptyDataTemplate>       
            
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
            <PagerSettings FirstPageImageUrl="~/App_Themes/image/arrow_top.gif" 
                LastPageImageUrl="~/App_Themes/image/arrow_down.gif" 
                Mode="NextPreviousFirstLast" 
                NextPageImageUrl="~/App_Themes/image/arrow_next.gif" 
                PreviousPageImageUrl="~/App_Themes/image/arrow_back.gif" />
            <PagerStyle HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        
        </asp:GridView>

        <asp:GridView ID="gridEPGList2" runat="server" Width="100%" 
            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
            CellPadding="3" GridLines="Vertical" Font-Size="13px"            
            OnRowDataBound="gridEPGList_RowDataBound"
            OnPageIndexChanging="gridEPGList_PageIndexChanging" 
            OnPageIndexChanged="gridEPGList_PageIndexChanged"
            AllowPaging="false" AutoGenerateColumns="false">
            
            <AlternatingRowStyle BackColor="#DCDCDC" />

            <Columns>
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%"  HeaderText="節目代號"      DataField="PlanningTitleID" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%" HeaderText="節目名稱"      DataField="PlanningTitleName" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="4%"  HeaderText="集數"          DataField="EpisodeNumber" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%"  HeaderText="HouseNumber"   DataField="HouseNo" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="頻道別"        DataField="Channel" />                
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="播映日期"      DataField="CalendarDate" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  HeaderText="播出時間"      DataField="CalendarTime" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="1%"  HeaderText="首/重/Live"    DataField="PremierRepeat" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%"  HeaderText="播映長度(分)"  DataField="Duration" />            
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="11%" HeaderText="系列名稱"      DataField="EpisodeTitle1" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="14%" HeaderText="分集名稱"      DataField="EpisodeTitle2" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="6%"  HeaderText="講員姓名"      DataField="EpisodeTitle3" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="6%"  HeaderText="字幕已疊映"    DataField="TypeCommnet" />
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%"  HeaderText="字幕上傳記錄"  DataField="UploadRecord" />
            </Columns>

            <EmptyDataTemplate>
                <table style="width:100%">
                    <tr >
                        <td width="5%" class="gridHead">節目代號</td>
                        <td width="10%" class="gridHead">節目名稱</td>
                        <td width="4%"  class="gridHead">集數</td>
                        <td width="8%"  class="gridHead">HouseNumber</td>
                        <td width="7%"  class="gridHead">頻道別</td>
                        <td width="7%"  class="gridHead">播映日期</td>
                        <td width="7%"  class="gridHead">播出時間</td>
                        <td width="1%"  class="gridHead">首/重/Live</td>
                        <td width="5%"  class="gridHead">播映長度(分)</td>
                        <td width="11%" class="gridHead">系列名稱</td>                                              
                        <td width="14%" class="gridHead">分集名稱</td>
                        <td width="6%"  class="gridHead">講員姓名</td>   
                        <td width="6%"  class="gridHead">字幕已疊映</td>
                        <td width="8%"  class="gridHead">字幕上傳記錄</td>                      
                    </tr>
                    <tr>
                        <td colspan="14" class="gridText">無資料</td>
                    </tr> 
                </table>
            </EmptyDataTemplate>

            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
            <PagerSettings FirstPageImageUrl="~/App_Themes/image/arrow_top.gif" 
                LastPageImageUrl="~/App_Themes/image/arrow_down.gif" 
                Mode="NextPreviousFirstLast" 
                NextPageImageUrl="~/App_Themes/image/arrow_next.gif" 
                PreviousPageImageUrl="~/App_Themes/image/arrow_back.gif" />
            <PagerStyle HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        
        </asp:GridView>

    </asp:Panel>
    </form>
</body>
</html>


