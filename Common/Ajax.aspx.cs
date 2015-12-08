using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Threading;

public partial class Ajax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Type = Request.Form["Type"];
        string result = "";
        switch (Type)
        {
            case "1":  //檢核客戶代號是否有交檔記錄
                result = CustomerIDIsUsed();
                break;
            case "2":  //依節目代號及集數檢核分集資料庫中不播出、不重播及不供片標記
                result = ProgramEpisodeIsNoted();
                break;
            case "3":  //檢核同一客戶是否己供過同一節目同一集數
                result = SameCustomerProgramEpisodeIsSuppplyed();
                break;
        }
        Response.Write(result);
    }
    
    //-------------------------------------------------------------------------
    private string CustomerIDIsUsed()
    {
        //Thread.Sleep(20000);
        string strRet = "N";
        string CustomerID = Request.Form["CustomerID"];

        string strSql1 = "select * from ReleaseProgramLog where customerID='" + CustomerID + "' ";
        string strSql2 = "select * from ReleaseShortFilmLog where customerID='" + CustomerID + "' ";

        Dictionary<string, object> dict1 = new Dictionary<string, object>();
        DataTable dt1 = NpoDB.GetDataTableS(strSql1, dict1);
        Dictionary<string, object> dict2 = new Dictionary<string, object>();
        DataTable dt2 = NpoDB.GetDataTableS(strSql2, dict2);

        if (dt1.Rows.Count > 0 || dt2.Rows.Count > 0)
        {
            strRet = "Y";
        }
        return strRet;
    }
    private string ProgramEpisodeIsNoted()
    {
        //Thread.Sleep(20000);
        string strRet = "N";
        string ProgramID = Request.Form["ProgramID"];
        string Episode = Request.Form["Episode"];

        string strSql = "select ISNULL(NoBroadcastNote,'') as NoBroadcastNote,ISNULL(NoReplayNote,'') as NoReplayNote,ISNULL(NoProvidevideoNote,'') as NoProvidevideoNote from [dbo].[ProgramEpisode] where ProgramCode='" + ProgramID + "' and Episode='" + Episode + "'";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count > 0 )
        {
            DataRow dr = dt.Rows[0];
            if (dr["NoBroadcastNote"].ToString().Trim() == "1" || dr["NoReplayNote"].ToString().Trim() == "1" || dr["NoProvidevideoNote"].ToString().Trim() == "1")
            {
                strRet = "Y";
            }
        }
        return strRet;
    }
    private string SameCustomerProgramEpisodeIsSuppplyed()
    {
        //Thread.Sleep(20000);
        string strRet = "N";
        string CustomerID = Request.Form["CustomerID"];
        string ProgramID = Request.Form["ProgramID"];
        string Episode = Request.Form["Episode"];

        string strSql = "select CONVERT(VARCHAR, supplyDate, 111) AS supplyDate from dbo.ReleaseProgramLog where customerID='" + CustomerID + "' and programID='" + ProgramID + "' and episode='" + Episode + "' ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            strRet = "Y" + dr["supplyDate"].ToString();
        }
        return strRet;
    }
}   