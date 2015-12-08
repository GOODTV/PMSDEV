﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ProgramEpisodeAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtProgramCode.Text = Util.GetQueryString("ProgramCode");
        }
    }
    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        //20141016 新增 若已有重複的節目帶碼和集數即無法新增
        string strSql = "";
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        strSql = "select * from dbo.ProgramEpisode where ProgramCode='" + txtProgramCode.Text.Trim() + "' and Episode='" + txtEpisode.Text.Trim() + "'";

        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count != 0)
        {
            this.Page.RegisterStartupScript("s", "<script>alert('此集數已存在，請填寫其他集數！！');</script>");
            return;
        }
        try
        {
            ProgramEpisode_AddNew();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (flag == true)
        {
            Response.Write("<Script language='JavaScript'>alert('分集資料新增成功！');</Script>");
            // 新增後導向頁面
            //Response.Redirect(Util.RedirectByTime("ProgramEpisodeQuery.aspx"));
            Page.Response.Write("<Script language='JavaScript'>location.href=('ProgramEpisodeQuery.aspx');</Script>");
            Response.End();
        }
    }
    public void ProgramEpisode_AddNew()
    {
        string strSql = @"INSERT INTO dbo.ProgramEpisode
                           (ProgramCode,Episode,Original_ProgramCode,Original_Episode,MainSeries,SubSeries,EpisodeName,EpisodeSynopsisOriginal,EpisodeSynopsis500,EpisodeSynopsis300,EpisodeSynopsis120
                            ,EpisodeSynopsisWEB,PresenterSpeaker,GuestName,TotalLength,PartLength,PremiereDate,CopyrightNote,NoBroadcastNote,NoReplayNote,NoProvidevideoNote,Lock,ReminderNotes,PackagerReplaySuggest,EditorReplaySuggest
                            ,CreateUser,CreateDatetime)
                            VALUES (@ProgramCode,@Episode,@Original_ProgramCode,@Original_Episode,@MainSeries,@SubSeries,@EpisodeName,@EpisodeSynopsisOriginal,@EpisodeSynopsis500,@EpisodeSynopsis300,@EpisodeSynopsis120
                            ,@EpisodeSynopsisWEB,@PresenterSpeaker,@GuestName,@TotalLength,@PartLength,@PremiereDate,@CopyrightNote,@NoBroadcastNote,@NoReplayNote,@NoProvidevideoNote,@Lock,@ReminderNotes,@PackagerReplaySuggest,@EditorReplaySuggest
                            ,@CreateUser,@CreateDatetime)";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ProgramCode", txtProgramCode.Text.Trim());
        dict.Add("Episode", txtEpisode.Text.Trim());
        dict.Add("Original_ProgramCode", txtOriginal_ProgramCode.Text.Trim());
        dict.Add("Original_Episode", txtOriginal_Episode.Text.Trim());
        dict.Add("MainSeries", txtMainSeries.Text.Trim());
        dict.Add("SubSeries", txtSubSeries.Text.Trim());
        dict.Add("EpisodeName", txtEpisodeName.Text.Trim());
        dict.Add("EpisodeSynopsisOriginal", txtEpisodeSynopsisOriginal.Text.Trim());
        dict.Add("EpisodeSynopsis500", txtEpisodeSynopsis500.Text.Trim());
        dict.Add("EpisodeSynopsis300", txtEpisodeSynopsis300.Text.Trim());
        dict.Add("EpisodeSynopsis120", txtEpisodeSynopsis120.Text.Trim());
        dict.Add("EpisodeSynopsisWEB", txtEpisodeSynopsisWEB.Text.Trim());
        dict.Add("PresenterSpeaker", txtPresenterSpeaker.Text.Trim());
        dict.Add("GuestName", txtGuestName.Text.Trim());
        dict.Add("TotalLength", txtTotalLength.Text.Trim());
        dict.Add("PartLength", txtPartLength.Text.Trim());
        dict.Add("PremiereDate", txtPremiereDate.Text.Trim());
        if (cbxCopyrightNote.Checked)
        {
            dict.Add("CopyrightNote", "1");
        }
        else
        {
            dict.Add("CopyrightNote", "0");
        }
        if (cbxNoBroadcastNote.Checked)
        {
            dict.Add("NoBroadcastNote", "1");
        }
        else
        {
            dict.Add("NoBroadcastNote", "0");
        }
        if (cbxNoReplayNote.Checked)
        {
            dict.Add("NoReplayNote", "1");
        }
        else
        {
            dict.Add("NoReplayNote", "0");
        }
        if (cbxNoProvidevideoNote.Checked)
        {
            dict.Add("NoProvidevideoNote", "1");
        }
        else
        {
            dict.Add("NoProvidevideoNote", "0");
        }
        if (cbxIsLock.Checked)
        {
            dict.Add("Lock", "Y");
        }
        else
        {
            dict.Add("Lock", "");
        }
        dict.Add("ReminderNotes", txtReminderNotes.Text.Trim());
        dict.Add("PackagerReplaySuggest", txtPackagerReplaySuggest.Text.Trim());
        dict.Add("EditorReplaySuggest", txtEditorReplaySuggest.Text.Trim());
        dict.Add("CreateUser", SessionInfo.UserID);
        dict.Add("CreateDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ProgramEpisodeQuery.aspx"));
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strSql = @"SELECT ProgramName = CASE WHEN ISNULL(ProgramTitle,'') <> '' THEN ProgramTitle 
									WHEN ISNULL(programEnglishTitle,'') <> '' THEN programEnglishTitle
									WHEN ISNULL(ProgramAbbrev,'') <> '' THEN ProgramAbbrev ELSE NULL END
		                  FROM [pms].dbo._TM01P0
		                  WHERE ProgramID = '" + txtProgramCode.Text + "'";
        String ProgramName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ProgramName = dr["ProgramName"].ToString();
        }
        else
        {
            ProgramName = "查無節目名稱!";
        }
        lblProgramName.Text = ProgramName;
        lblProgramName.Font.Size = 11;
        lblProgramName.ForeColor = System.Drawing.Color.Blue; ;
        lblProgramName.Font.Bold = true;
    }
}