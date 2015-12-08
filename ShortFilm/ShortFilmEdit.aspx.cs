using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShortFilm_ShortFilmEdit : BasePage
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
        strSql = @" select *,b.[ProgramAbbrev] as ProgramName, 
                        CAST((Length / 60) % 60 AS VARCHAR) as Min ,
                        CAST(Length % 60 AS VARCHAR) as Sec , 
                        Case when CONVERT(VarChar,OnAirEndDate,111)='9999/12/31' then CONVERT(VarChar,OnAirBeginDate,111)+'起，無迄期' 
		                else CONVERT(VarChar,OnAirBeginDate,111)+'起，至'+CONVERT(VarChar,OnAirEndDate,111)+'止' end as OnAirDate  
                        From [dbo].[_CF02P0] as a left join [dbo].[_TM01P0] as b on a.ProgramID = b.ProgramID  where CFID='" + uid + "'";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);
        Session["Uid"] = HFD_Uid.Value;

        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("ShortFilmQuery.aspx");

        DataRow dr = dt.Rows[0];

        //短片編號
        tbxCFID.Text = dr["CFID"].ToString().Trim();
        //節目代號
        tbxProgramID.Text = dr["ProgramID"].ToString().Trim();
        tbxProgramName.Text = dr["ProgramName"].ToString().Trim();
        //節目集數
        tbxEpisode.Text = dr["Episode"].ToString().Trim();
        //短片名稱
        tbxCFTitle.Text = dr["CFTitle"].ToString().Trim();
        //短片長度
        tbxLengthMin.Text = dr["Min"].ToString().Trim();
        tbxLengthSec.Text = dr["Sec"].ToString().Trim();
        //HDC
        ddlHDC.SelectedValue = dr["HDC"].ToString();
        //播出頻道
        ddlChannel.SelectedValue = dr["Channel"].ToString();
        //內容摘要說明
        tbxCFDescription.Text = dr["CFDescription"].ToString().Trim();
        //排播時間
        tbxOnAirBeginDate.Text = DateTime.Parse(dr["OnAirBeginDate"].ToString()).ToString("yyyy/MM/dd");
        tbxOnAirEndDate.Text = DateTime.Parse(dr["OnAirEndDate"].ToString()).ToString("yyyy/MM/dd");
        //排播說明
        tbxOnAirRemark.Text = dr["OnAirRemark"].ToString().Trim();
        //短片註記
        tbxAppendance.Text = dr["Appendance"].ToString().Trim();
        //排播頻率
        rblFrequency.SelectedValue = dr["Frequency"].ToString();
        //tbxFrequency.Text = dr["Frequency"].ToString().Trim();
        //排播時段
        //lblOnAirTimeSlot.Text = dr["OnAirTimeSlot"].ToString().Trim();
        lblOnAirTimeSlot.Text = LoadCheckBoxListData(dr["OnAirTimeSlot"].ToString());
        //提供官網
        if (dr["ForWeb"].ToString().Trim() == "Y")
        {
            cbxForWeb.Checked = true;
        }
        else
        {
            cbxForWeb.Checked = false;
        }

        //申請同工
        string RequesterID = dr["RequesterID"].ToString().Trim();

        if (RequesterID != "")
        {
            tbxRequesterID.Text = RequesterID;
            tbxRequesterName.Text = Util.GetHRUser(RequesterID);
        }
        Session["RequesterID"] = tbxRequesterID.Text;

        //片庫同工
        string MediaUser = dr["MediaUser"].ToString().Trim();
        if (MediaUser != "")
        {
            tbxMediaUser.Text = MediaUser;
            tbxMediaUserName.Text = Util.GetHRUser(MediaUser);
        }

    }

    public string LoadCheckBoxListData(string OnAirTimeSlotValues)
    {
        List<ControlData> list = new List<ControlData>();
        list.Clear();
        string OnAirTimeSlot = "信徒,預工,家庭,佈道,深夜";
        string[] strArray = OnAirTimeSlot.Split(',');
        string OnAirTimeSlotValuesComm = String.IsNullOrEmpty(OnAirTimeSlotValues) ? "" : OnAirTimeSlotValues.Substring(0, 1);

        /*
        foreach (string i in strArray)
        {
            string TimeSlot = i.ToString();
            bool ShowBR = false;
            list.Add(new ControlData("Checkbox", "GN_OnAirTimeSlot", "CB_OnAirTimeSlot", i, TimeSlot, ShowBR, OnAirTimeSlotValues));
        }
        */

        for (int i = 1; i < OnAirTimeSlotValues.Length; i++)
        {
            OnAirTimeSlotValuesComm += "," + OnAirTimeSlotValues.Substring(i, 1);
        }

        for (int i = 0; i < strArray.Length; i++)
        {
            list.Add(new ControlData("Checkbox", "GN_OnAirTimeSlot", "CB_OnAirTimeSlot_" + i.ToString(), i.ToString(), strArray[i], false, OnAirTimeSlotValuesComm));
        }

        return HtmlUtil.RenderControl(list);
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        bool flag = false;
        try
        {
            ShortFilm_Edit();
            flag = true;
        }
        catch (Exception ex)
        {
            //throw ex;
            System.Console.Out.Write(ex.Message);
            Response.Redirect("ShortFilmQuery.aspx");
        }
        if (flag == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", 
                "if (confirm('儲存成功，請問是否要下載托播單?\\n關閉視窗後請重新搜尋')) location.href=('ShortFilmQuery.aspx?Mode=print&Key=" + 
                HFD_Key.Value + "'); else location.href=('ShortFilmQuery.aspx?Key=" + HFD_Key.Value + "');", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", 
            //      "if (confirm('儲存成功，請問是否要下載托播單?\\n關閉視窗後請重新搜尋')) location.href=('ShortFilm_Print_Excel.aspx?Key=" + 
            //        HFD_Key.Value + "'); else location.href=('ShortFilmQuery.aspx?Key=" + HFD_Key.Value + "');", true);
            //Response.Write("<Script language='JavaScript'>alert('短片申請修改成功！');</Script>");
            //Response.Write("<Script language='JavaScript'>if (confirm('儲存成功，請問是否要下載托播單？')){location.href=('ShortFilm_Print_Excel.aspx?Key=" + HFD_Key.Value + "');}</Script>");
            //Response.Redirect(Util.RedirectByTime("ShortFilmQuery.aspx"));
            //Page.Response.Write("<Script language='JavaScript'>location.href=('ShortFilmQuery.aspx?Key=" + HFD_Key.Value + "');</Script>");
            //Response.End();
        }
    }

    public void ShortFilm_Edit()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string strSql = " update [dbo].[_CF02P0] set ";

        strSql += "  CFTitle = @CFTitle";
        strSql += ", ProgramID = @ProgramID";
        strSql += ", Episode = @Episode";
        strSql += ", Length = @Length";
        strSql += ", Channel = @Channel";
        strSql += ", CFDescription = @CFDescription";
        strSql += ", OnAirBeginDate = @OnAirBeginDate";
        strSql += ", OnAirEndDate = @OnAirEndDate";
        strSql += ", OnAirRemark = @OnAirRemark";

        strSql += ", HDC = @HDC";
        strSql += ", Appendance = @Appendance";
        strSql += ", Frequency = @Frequency";
        strSql += ", RequesterID = @RequesterID";
        strSql += ", MediaUser = @MediaUser";
        strSql += ", OnAirTimeSlot = @OnAirTimeSlot";
        strSql += ", ForWeb = @ForWeb";

        strSql += ", ModifiedUser= @ModifiedUser";
        strSql += ", ModifiedDatetime = @ModifiedDatetime";
        strSql += " where CFID = @CFID";

        dict.Add("CFTitle", tbxCFTitle.Text.Trim());
        dict.Add("ProgramID", tbxProgramID.Text.Trim());
        dict.Add("Episode", tbxEpisode.Text.Trim());
        dict.Add("Length", Int16.Parse(tbxLengthMin.Text) * 60 + Int16.Parse(tbxLengthSec.Text));
        dict.Add("Channel", ddlChannel.SelectedValue);
        dict.Add("CFDescription", tbxCFDescription.Text.Trim());
        dict.Add("OnAirBeginDate", tbxOnAirBeginDate.Text.Trim());
        dict.Add("OnAirEndDate", tbxOnAirEndDate.Text.Trim());

        dict.Add("OnAirRemark", tbxOnAirRemark.Text.Trim());

        dict.Add("HDC", ddlHDC.SelectedValue);
        dict.Add("Appendance", tbxAppendance.Text.Trim());
        dict.Add("Frequency", rblFrequency.SelectedValue);
        dict.Add("RequesterID", tbxRequesterID.Text.Trim());
        dict.Add("MediaUser", tbxMediaUser.Text.Trim());
        dict.Add("OnAirTimeSlot", Util.GetControlValue("GN_OnAirTimeSlot").Replace(",",""));
        if (cbxForWeb.Checked == true)
        {
            dict.Add("ForWeb", "Y");
        }
        else
        {
            dict.Add("ForWeb", "N");
        }

        dict.Add("ModifiedUser", SessionInfo.UserID);
        dict.Add("ModifiedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("CFID", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        string strSql = "delete from [dbo].[_CF02P0] where CFID=@CFID";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CFID", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "alert('刪除成功');location.href=('ShortFilmQuery.aspx?Key=" + HFD_Key.Value + "');", true);
        //Response.Write("<Script language='JavaScript'>alert('短片申請刪除成功！');</Script>");
        //Response.Redirect(Util.RedirectByTime("ShortFilmQuery.aspx"));
        //Page.Response.Write("<Script language='JavaScript'>location.href=('ShortFilmQuery.aspx?Key=" + HFD_Key.Value + "');</Script>");
        //Response.End();
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ShortFilmQuery.aspx", "Key=" + HFD_Key.Value));
    }

}
