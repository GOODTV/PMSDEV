using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShortFilm_ShortFilmApplyQuery : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            HFD_Key.Value = Util.GetQueryString("Key");
            //Page.Response.Write("<Script language='JavaScript'>alert('" + HFD_Key.Value + "');</Script>");
            string[] strArray;
            strArray = HFD_Key.Value.Split(';');
            if (strArray.Length > 1)
            {
                //CFID
                if (strArray[0] != "")
                {
                    tbxCFID.Text = strArray[0];
                }
                //ProgramID
                if (strArray[1] != "")
                {
                    tbxProgramID.Text = strArray[1];
                }
                //RequesterID
                if (strArray[2] != "")
                {
                    tbxRequesterID.Text = strArray[2];
                }
                //ValidStatus
                if (strArray[3] != "")
                {
                    rblValidStatus.SelectedValue = strArray[3];
                }
            }
            if (tbxCFID.Text.Trim() != "" || tbxProgramID.Text.Trim() != "" || tbxRequesterID.Text.Trim() != "")
            {
                LoadFormData();
            }

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
        strSql = @"select CFID as '短片代號',CFTitle as '短片名稱'
                ,CASE WHEN ISNULL(M.ProgramID,'') = '' THEN '' ELSE M.ProgramID + ' - ' + ISNULL(T.ProgramAbbrev,'') END as '節目代號 - 名稱'
                ,Episode as '集數' ,
                Case when CONVERT(VarChar,RequestDate,111)='9999/12/31' then '' else CONVERT(VarChar,RequestDate,111) end as '申請日期',
                RequesterID as '申請人',
                Case when CONVERT(VarChar,OnAirEndDate,111)='9999/12/31' then CONVERT(VarChar,OnAirBeginDate,111)+'起，無迄期' 
                else CONVERT(VarChar,OnAirBeginDate,111)+'起，至'+CONVERT(VarChar,OnAirEndDate,111)+'止' end as '播映期間' ,
                Case when ISNULL(MediaUser,'')='' then 'N' else 'Y' end as '片庫確認' , Frequency as '頻率',OnAirTimeSlot as '時段'
                ,MediaUser as '片庫同工' ,ISNULL(ForWeb,'') as '提供官網',Appendance as '註記欄'
                from [dbo].[_CF02P0] M
                left join [_TM01P0] T on M.ProgramID = T.ProgramID
                where 1=1 ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxCFID.Text.Trim() != "")
        {
            strSql += " and CFID like N'%" + tbxCFID.Text.Trim() + "%'";
        }
        if (tbxProgramID.Text.Trim() != "")
        {
            strSql += " and M.ProgramID like N'%" + tbxProgramID.Text.Trim() + "%'";
        }
        if (tbxRequesterID.Text.Trim() != "")
        {
            strSql += " and RequesterID = N'" + tbxRequesterID.Text.Trim() + "'";
        }
        if (rblValidStatus.SelectedValue == "1")
        {
            strSql += " and  CONVERT(VarChar,OnAirEndDate,111) >= CONVERT(VarChar,getdate() ,111) ";
        }
        else if (rblValidStatus.SelectedValue == "2")
        {
            strSql += " and  CONVERT(VarChar,OnAirEndDate,111) < CONVERT(VarChar,getdate() ,111) ";
        }
        strSql += " order by CFID,CONVERT(VarChar,RequestDate,111) ";
        HFD_Key.Value = tbxCFID.Text + ";" + tbxProgramID.Text + ";" + tbxRequesterID.Text + ";" + rblValidStatus.SelectedValue;
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "alert('查無資料！');", true);
            //lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            //lblGridList.ForeColor = System.Drawing.Color.Red;
            lblGridList.Text = "";
        }
        else
        {
            //2015/01/21 換掉原先的 Html Table
            string strBody = "";// "<table class='table-list' width='100%'>";
            /*strBody += @"<TR><TH width='80px'><SPAN>短片代號</SPAN></TH>
                            <TH width='200px'><SPAN>短片名稱</SPAN></TH>
                            <TH width='200px'><SPAN>節目代號 - 名稱</SPAN></TH>
                            <TH width='40px'><SPAN>集數</SPAN></TH>
                            <TH width='80px'><SPAN>申請日期</SPAN></TH>
                            <TH width='100px'><SPAN>申請人</SPAN></TH>
                            <TH width='200px'><SPAN>播映期間</SPAN></TH>
                            <TH noWrap><SPAN>片庫確認</SPAN></TH>
                            <TH noWrap><SPAN>頻率</SPAN></TH>
                            <TH noWrap><SPAN>時段</SPAN></TH>
                            <TH width='100px'><SPAN>片庫同工</SPAN></TH>
                            <TH noWrap><SPAN>提供官網</SPAN></TH>
                            <TH noWrap><SPAN>註記欄</SPAN></TH>
                        </TR>";*/
            foreach (DataRow dr in dt.Rows)
            {

                strBody += "<TR align='left' ondblclick =\"window.event.cancelBubble=true;window.open(\'" + Util.RedirectByTime("ShortFilmApplyQueryDetail.aspx", "CFID=" + dr["短片代號"].ToString() + "&Key=" + HFD_Key.Value) + "\',\'_self\',\'\')\"><TD noWrap><SPAN>" + dr["短片代號"].ToString() + "</SPAN>";
                strBody += "<TD><SPAN>" + dr["短片名稱"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["節目代號 - 名稱"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["集數"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["申請日期"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["申請人"].ToString() + " " + GetHRUser(dr["申請人"].ToString()) + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["播映期間"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["片庫確認"].ToString() + "</SPAN></TD>";
                string strFrequency = dr["頻率"].ToString();
                string Frequency = "";
                if (strFrequency == "1")
                {
                    Frequency = "A級";
                }
                else if (strFrequency == "2")
                {
                    Frequency = "B級";
                }
                else if (strFrequency == "3")
                {
                    Frequency = "C級";
                }
                strBody += "<TD><SPAN>" + Frequency + "</SPAN></TD>";
                string strAirTimeSlot = dr["時段"].ToString();
                string AirTimeSlot = "";
                for (int i = 0; i < strAirTimeSlot.Length; i++)
                {
                    if (strAirTimeSlot.Substring(i, 1) == "0")
                    {
                        AirTimeSlot += "信徒 ";
                    }
                    else if (strAirTimeSlot.Substring(i, 1) == "1")
                    {
                        AirTimeSlot += "預工 ";
                    }
                    else if (strAirTimeSlot.Substring(i, 1) == "2")
                    {
                        AirTimeSlot += "家庭 ";
                    }
                    else if (strAirTimeSlot.Substring(i, 1) == "3")
                    {
                        AirTimeSlot += "佈道 ";
                    }
                    else if (strAirTimeSlot.Substring(i, 1) == "4")
                    {
                        AirTimeSlot += "深夜 ";
                    }

                }
                strBody += "<TD><SPAN>" + AirTimeSlot.Trim() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["片庫同工"].ToString() + " " + GetHRUser(dr["片庫同工"].ToString()) + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["提供官網"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN><font color=blue>" + dr["註記欄"].ToString() + "</font></SPAN></TD></TR>";

            }

            //strBody += "</table>";
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
        count.Text = String.Format("{0:N0}", dt.Rows.Count);
        Session["strSql"] = strSql;
    }

    public string GetHRUser(String Employee_no)
    {
        if (String.IsNullOrEmpty(Employee_no)) return "";
        string strEmployee_cname = "";
        string sql = "SELECT [EMPLOYEE_CNAME] FROM [HRMS_EMPLOYEE] where [EMPLOYEE_NO] = @EMPLOYEE_NO ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("EMPLOYEE_NO", Employee_no);
        strEmployee_cname = NpoDB.GetScalarS(sql, dict, "HRConnection");
        return strEmployee_cname;
    }

    /*private string Count()
    {
        string strSql = @"select count(*) as count
                            from [dbo].[_CF02P0] M
                            left join [_TM01P0] T on M.ProgramID=T.ProgramID
                            where 1=1 ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        if (tbxCFID.Text.Trim() != "")
        {
            strSql += " and CFID like N'%" + tbxCFID.Text.Trim() + "%'";
        }
        if (tbxProgramID.Text.Trim() != "")
        {
            strSql += " and M.ProgramID like N'%" + tbxProgramID.Text.Trim() + "%'";
        }
        if (tbxRequesterID.Text.Trim() != "")
        {
            strSql += " and RequesterID like N'" + tbxRequesterID.Text.Trim() + "%'";
        }
        if (rblValidStatus.SelectedValue == "1")
        {
            strSql += " and  CONVERT(VarChar,OnAirEndDate,111) >= CONVERT(VarChar,getdate() ,111) ";
        }
        else if (rblValidStatus.SelectedValue == "2")
        {
            strSql += " and  CONVERT(VarChar,OnAirEndDate,111) < CONVERT(VarChar,getdate() ,111) ";
        }
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        DataRow dr = dt.Rows[0];
        string count = dr["count"].ToString();
        return count;
    }*/

}
