using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShortFilm_ShortFilmApplyQueryDetail : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("CFID");
            HFD_Key.Value = Util.GetQueryString("Key");
            //Page.Response.Write("<Script language='JavaScript'>alert('" + HFD_Key.Value + "');</Script>");
            Form_DataBind();
        }
    }

    //帶入資料
    public void Form_DataBind()
    {
        //****變數宣告****//
        string strSql, uid;
        DataTable dt;

        //****變數設定****//
        uid = HFD_Uid.Value;

        //****設定查詢****//
        strSql = @" select *, [dbo].[getProgramName](ProgramID) as ProgramName,
                        RIGHT('0' + CAST(Length / 3600 AS VARCHAR),2) + ':' +
                        RIGHT('0' + CAST((Length / 60) % 60 AS VARCHAR),2)  + ':' +
                        RIGHT('0' + CAST(Length % 60 AS VARCHAR),2)+ ':00' as Time , 
                        Case when CONVERT(VarChar,OnAirEndDate,111)='9999/12/31' then CONVERT(VarChar,OnAirBeginDate,111)+'起，無迄期' 
		                else CONVERT(VarChar,OnAirBeginDate,111)+'起，至'+CONVERT(VarChar,OnAirEndDate,111)+'止' end as OnAirDate  
                        From [dbo].[_CF02P0]  where CFID = '" + uid + "' ";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);


        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("ShortFilmApplyQuery.aspx");

        DataRow dr = dt.Rows[0];

        //短片編號
        tbxCFID.Text = dr["CFID"].ToString().Trim();
        //節目代號
        tbxProgramID.Text = dr["ProgramID"].ToString().Trim() + " " + dr["ProgramName"].ToString().Trim();
        //節目集數
        tbxEpisode.Text = dr["Episode"].ToString().Trim();
        //短片名稱
        tbxCFTitle.Text = dr["CFTitle"].ToString().Trim();
        //短片長度
        tbxLength.Text = dr["Time"].ToString().Trim();
        //HDC
        tbxHDC.Text = dr["HDC"].ToString().Trim();
        //播出頻道
        if (dr["Channel"].ToString().Trim() == "1")
        {
            tbxChannel.Text = "一台";
        }
        else if (dr["Channel"].ToString().Trim() == "2")
        {
            tbxChannel.Text = "二台";
        }
        else if (dr["Channel"].ToString().Trim() == "3")
        {
            tbxChannel.Text = "一二台";
        }
        //內容摘要說明
        tbxCFDescription.Text = dr["CFDescription"].ToString().Trim();
        //排播時間
        tbxOnAirDate.Text = dr["OnAirDate"].ToString().Trim();
        //排播說明
        tbxOnAirRemark.Text = dr["OnAirRemark"].ToString().Trim();
        //短片註記
        tbxAppendance.Text = dr["Appendance"].ToString().Trim();
        //排播頻率
        if (dr["Frequency"].ToString().Trim() == "1")
        {
            tbxFrequency.Text = "A級";
        }
        else if (dr["Frequency"].ToString().Trim() == "2")
        {
            tbxFrequency.Text = "B級";
        }
        else if (dr["Frequency"].ToString().Trim() == "3")
        {
            tbxFrequency.Text = "C級";
        }
        //tbxFrequency.Text = dr["Frequency"].ToString().Trim();
        //排播時段
        string strAirTimeSlot = dr["OnAirTimeSlot"].ToString().Trim();
        for (int i = 0 ; i < strAirTimeSlot.Length; i++)
        {
            if (strAirTimeSlot.Substring(i, 1) == "0")
            {
                tbxOnAirTimeSlot.Text += "信徒 ";
            }
            else if (strAirTimeSlot.Substring(i, 1) == "1")
            {
                tbxOnAirTimeSlot.Text += "預工 ";
            }
            else if (strAirTimeSlot.Substring(i, 1) == "2")
            {
                tbxOnAirTimeSlot.Text += "家庭 ";
            }
            else if (strAirTimeSlot.Substring(i, 1) == "3")
            {
                tbxOnAirTimeSlot.Text += "佈道 ";
            }
            else if (strAirTimeSlot.Substring(i, 1) == "4")
            {
                tbxOnAirTimeSlot.Text += "深夜 ";
            }

        }
        tbxOnAirTimeSlot.Text.Trim();
        //tbxOnAirTimeSlot.Text = dr["OnAirTimeSlot"].ToString().Trim();
        //提供官網
        tbxForWeb.Text = dr["ForWeb"].ToString().Trim();
        //申請同工
        tbxRequesterID.Text = dr["RequesterID"].ToString().Trim() + " " + GetHRUser(dr["RequesterID"].ToString().Trim());
        //片庫同工
        tbxMediaUser.Text = dr["MediaUser"].ToString().Trim() + " " + GetHRUser(dr["MediaUser"].ToString().Trim());
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ShortFilmApplyQuery.aspx", "Key=" + HFD_Key.Value));
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

}
