using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class View_SRTStatusQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Session["ProgramCode"] != null)
            //{
            //    tbxProgramCode.Text = Session["ProgramCode"].ToString();
            //}
            //if (Session["EpisodeNo_Start"] != null)
            //{
            //    tbxEpNo_Start.Text = Session["EpisodeNo_Start"].ToString();
            //}
            //if (Session["EpisodeNo_End"] != null)
            //{
            //    tbxEpNo_End.Text = Session["EpisodeNo_End"].ToString();
            //}
            //if (tbxProgramCode.Text != "" || tbxEpNo_Start.Text != "" || tbxEpNo_End.Text != "")
            //{
            //    LoadFormData();
            //}
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    public void LoadFormData()
    {
        string strSql;
        DataTable dt;
        strSql = @"select case when cast([LockedDate] as date) < cast('9999-12-31 23:59:59' as date) then 'locked.png' 
	                           when cast([ImportDate] as date) < cast('9999-12-31 23:59:59' as date) then 'approved.png' else 'approving.png' end as [Image]
                         ,case when cast([LockedDate] as date) < cast('9999-12-31 23:59:59' as date) then '已鎖定' 
	                           when cast([ImportDate] as date) < cast('9999-12-31 23:59:59' as date) then '已完成' else '申請中' end as [Tooltiptext]
                     ,M.ProgramID as 節目代號, T.ProgramAbbrev as 節目簡稱 ,
                     Episode as 集數 ,M.ProgramLength as 節目長度 ,M.EditorID as 字幕人員 ,
                     EditorName as 字幕人員姓名 , C.ClassificationName as 製作類別 , 
                     SubtitleCostPerEpisode as 單集成本 ,SubtitleComparePerEpisode as 單集評比 ,
                     Case when CONVERT(VarChar,RequestDate,111)='9999/12/31' then '' else CONVERT(VarChar,RequestDate,111) end as 申請日期,
                     Case when CONVERT(VarChar,ImportDate,111)='9999/12/31' then '' else CONVERT(VarChar,ImportDate,111) end as 匯入日期 ,
                     Case when CONVERT(VarChar,LockedDate,111)='9999/12/31' then '' else CONVERT(VarChar,LockedDate,111) end as 鎖定日期,
                     CAST((Case when BillDate%12=0 then BillDate/12-1 else BillDate/12 end) as varchar(10))+'-'+
                     CAST((Case when BillDate%12=0 then '12' else BillDate%12 end) as varchar(10)) as 結帳年月
                    from [dbo].[_ST03P0] M
                    left join [_ST01P0] D on M.EditorID=D.EditorID
                    left join Classification C on M.Classification=C.ClassificationCode
                    left join [_TM01P0] T on M.ProgramID=T.ProgramID
                    where 1=1 ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxProgramCode.Text.Trim() != "")
        {
            strSql += " and M.ProgramID like N'%" + tbxProgramCode.Text.Trim() + "%'";
        }
        if (tbxEpNo_Start.Text.Trim() != "")
        {
            strSql += " and M.Episode >= '" + tbxEpNo_Start.Text.Trim() + "' ";
        }
        if (tbxEpNo_End.Text.Trim() != "")
        {
            strSql += " and M.Episode <= '" + tbxEpNo_End.Text.Trim() + "' ";
        }
        if (rblStatus.SelectedValue=="1")
        {
            strSql += " and CONVERT(VarChar,ImportDate,111) ='9999/12/31' ";
        }
        else if (rblStatus.SelectedValue=="2")
        {
            strSql += " and CONVERT(VarChar,ImportDate,111) !='9999/12/31' ";
        }
        strSql += " order by M.ProgramID,Episode ";
        //Session["ProgramCode"] = tbxProgramCode.Text.Trim();
        //Session["EpisodeNo_Start"] = tbxEpNo_Start.Text.Trim();
        //Session["EpisodeNo_End"] = tbxEpNo_End.Text.Trim();
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            lblGridList.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            //2015/01/21 換掉原先的 Html Table
            string strBody = "";// "<table class='table-list' width='100%'>";
            /*strBody += @"<TR><TH width='10px'></TH>
                            <TH noWrap><SPAN>節目代號</SPAN></TH>
                            <TH noWrap><SPAN>節目簡稱</SPAN></TH>
                            <TH noWrap><SPAN>集數</SPAN></TH>
                            <TH noWrap><SPAN>節目長度</SPAN></TH>
                            <TH noWrap><SPAN>字幕人員</SPAN></TH>
                            <TH noWrap><SPAN>字幕人員姓名</SPAN></TH>
                            <TH noWrap><SPAN>製作類別</SPAN></TH>
                            <TH noWrap><SPAN>單集成本</SPAN></TH>
                            <TH noWrap><SPAN>單集評比</SPAN></TH>
                            <TH noWrap><SPAN>申請日期</SPAN></TH>
                            <TH noWrap><SPAN>匯入日期</SPAN></TH>
                            <TH noWrap><SPAN>鎖定日期</SPAN></TH>
                            <TH noWrap><SPAN>結帳年月</SPAN></TH>
                        </TR>";*/
            foreach (DataRow dr in dt.Rows)
            {

                strBody += "<TR align='left'><TD><img alt='' src='../img/" + dr["Image"].ToString() + "' title='" + dr["Tooltiptext"].ToString() + "' /></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["節目代號"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["節目簡稱"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["集數"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["節目長度"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["字幕人員"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["字幕人員姓名"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["製作類別"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["單集成本"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["單集評比"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["申請日期"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["匯入日期"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["鎖定日期"].ToString() + "&nbsp;</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["結帳年月"].ToString() + "&nbsp;</SPAN></TD></TR>";
            }

            strBody += "</table>";
            lblGridList.Text = strBody;
            /*//Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            //npoGridView.Keys.Add("SerNo");
            //npoGridView.DisableColumn.Add("SerNo");
            //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = false; //不換頁
            //npoGridView.EditLink = Util.RedirectByTime("SRTStatusEdit.aspx", "SerNo=");
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;*/
        }

        Session["strSql"] = strSql;
    }
}