using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShortFilm_ShortFilmQuery : System.Web.UI.Page
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
                //CFTitle
                if (strArray[1] != "")
                {
                    tbxCFTitle.Text = strArray[1];
                }
                //ProgramID
                if (strArray[2] != "")
                {
                    tbxProgramID.Text = strArray[2];
                }
                //Episode
                if (strArray[3] != "")
                {
                    tbxEpisode.Text = strArray[3];
                }
                //ValidStatus
                if (strArray[4] != "")
                {
                    ddlChannel.SelectedValue = strArray[4];
                }
            }
            if (tbxCFID.Text.Trim() != "" || tbxProgramID.Text.Trim() != "" || tbxCFTitle.Text.Trim() != "")
            {
                LoadFormData();
            }
            HFD_Mode.Value = Util.GetQueryString("Mode");

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
        //只顯示查詢已經處理過得資料
        strSql = @"select CFID ,M.Channel as '台別',CASE WHEN ISNULL(M.ProgramID,'') = '' THEN '' ELSE 
                        ISNULL(M.ProgramID,'')+' - '+ISNULL(T.ProgramAbbrev,'') END as '節目代號 - 名稱',
                        Episode as '集數' ,CFTitle as '短片名稱',
	                    RIGHT('0' + CAST(Length / 3600 AS VARCHAR),2) + ':' +RIGHT('0' + CAST((Length / 60) % 60 AS VARCHAR),2)  + ':' +
                        RIGHT('0' + CAST(Length % 60 AS VARCHAR),2)+ ':00' as '長度(秒)' , 
                        Case when CONVERT(VarChar,OnAirEndDate,111)='9999/12/31' then CONVERT(VarChar,OnAirBeginDate,111)+'起，無迄期' 
	                    else CONVERT(VarChar,OnAirBeginDate,111)+'起，至'+CONVERT(VarChar,OnAirEndDate,111)+'止' end as '播映期間' ,
                        Case when Frequency='1' then 'A級' when Frequency='2' then 'B級' when Frequency='3' then 'C級' end as '頻率',
                        OnAirTimeSlot as '時段',ISNULL(ForWeb,'') as '提供官網',MediaUser as '排程同工' ,Appendance as '註記欄'
                    from [dbo].[_CF02P0] M
                    left join [_TM01P0] T on M.ProgramID = T.ProgramID
                    where ISNULL(M.[MediaUser],'') != '' ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxCFID.Text.Trim() != "")
        {
            strSql += " and M.CFID like N'" + tbxCFID.Text.Trim() + "%'";
        }
        if (tbxCFTitle.Text.Trim() != "")
        {
            strSql += " and M.CFTitle like N'%" + tbxCFTitle.Text.Trim() + "%'";
        }
        if (tbxProgramID.Text.Trim() != "")
        {
            strSql += " and M.ProgramID like N'" + tbxProgramID.Text.Trim() + "%'";
        }
        if (tbxEpisode.Text.Trim() != "")
        {
            strSql += " and M.Episode = " + tbxEpisode.Text.Trim() + " ";
        }
        if (ddlChannel.SelectedValue == "1")
        {
            strSql += " and M.Channel = '1' ";
        }
        else if (ddlChannel.SelectedValue == "2")
        {
            strSql += " and  M.Channel = '2' ";
        }
        else if (ddlChannel.SelectedValue == "3")
        {
            strSql += " and  M.Channel = '3' ";
        }
        if (tbxProgramID.Text.Trim() != "")
        {
            strSql += " order by M.[ProgramID],M.[Episode],[CFID] ";
        }
        else
        {
            strSql += " order by CFID ";    //,CONVERT(VarChar,RequestDate,111)
        }
        HFD_Key.Value = tbxCFID.Text + ";" + tbxCFTitle.Text + ";" + tbxProgramID.Text + ";" + tbxEpisode.Text + ";" + 
            ddlChannel.SelectedValue;
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "alert('查無資料！');", true);
            lblGridList.Text = "";
            //lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            //lblGridList.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            //2015/01/21 換掉原先的 Html Table
            string strBody = "";// "<table class='table-list' width='100%'>";
            /*strBody += @"<TR>
                            <TH width='30px'><SPAN>台別</SPAN></TH>
                            <TH width='200px'><SPAN>節目代號 - 名稱</SPAN></TH>
                            <TH width='40px'><SPAN>集數</SPAN></TH>
                            <TH width='200px'><SPAN>短片名稱</SPAN></TH>
                            <TH width='90px'><SPAN>長度(秒)</SPAN></TH>
                            <TH width='200px'><SPAN>播映期間</SPAN></TH>
                            <TH noWrap><SPAN>頻率</SPAN></TH>
                            <TH noWrap><SPAN>時段</SPAN></TH>
                            <TH noWrap><SPAN>提供官網</SPAN></TH>
                            <TH width='100px'><SPAN>排程同工</SPAN></TH>
                            <TH noWrap><SPAN>註記欄</SPAN></TH>
                        </TR>";*/
            foreach (DataRow dr in dt.Rows)
            {

                strBody += "<TR align='left' ondblclick =\"window.event.cancelBubble=true;window.open(\'" + Util.RedirectByTime("ShortFilmEdit.aspx", "CFID=" + dr["CFID"].ToString() + "&Key=" + HFD_Key.Value) + "\',\'_self\',\'\')\">";
                strBody += "<TD><SPAN>" + dr["台別"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["CFID"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["節目代號 - 名稱"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["集數"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["短片名稱"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["長度(秒)"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["播映期間"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["頻率"].ToString() + "</SPAN></TD>";
                //strBody += "<TD><SPAN>" + dr["時段"].ToString() + "</SPAN></TD>";
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
                strBody += "<TD><SPAN>" + dr["提供官網"].ToString() + "</SPAN></TD>";
                //strBody += "<TD><SPAN>" + dr["排程同工"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["排程同工"].ToString() + " " + Util.GetHRUser(dr["排程同工"].ToString()) + "</SPAN></TD>";
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

}
