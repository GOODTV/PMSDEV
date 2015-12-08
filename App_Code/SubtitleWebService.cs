using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Collections;
using System.Web.Script.Services;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net.Mail;


/// <summary>
/// SubtitleWebService 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
[System.Web.Script.Services.ScriptService]
public class SubtitleWebService : System.Web.Services.WebService {

    string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    static log4net.ILog logger = log4net.LogManager.GetLogger("RollingLogFileAppender");

    public SubtitleWebService () {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    [WebMethod]
    public int InsertSubtitle(string tables)
    {

        int result = 0;
        string stremailSubtitle = "";
        SubtitleTables SubtitleList = (SubtitleTables)GetObjectFromJSON(tables, typeof(SubtitleTables));
        string commandText = @"INSERT INTO [dbo].[_ST03P0]
        ([ProgramID] ,[Episode] ,[ProgramLength] ,[EditorID] ,[Classification]
        ,[SubtitleCostPerEpisode] ,[SubtitleComparePerEpisode] ,[RequestDate]
        ,[ImportDate] ,[SubtitleFilename] ,[Status] ,[LockedDate]
        ,[BillDate],[Description] ,[CreatedUser] ,[CreatedDatetime],[ModifiedUser],[ModifiedDatetime],[Requester])
        VALUES (@ProgramID ,@Episode ,@ProgramLength ,@EditorID ,@Classification
        ,@SubtitleCostPerEpisode ,@SubtitleComparePerEpisode ,getdate() ,@ImportDate
        ,@SubtitleFilename ,@Status ,@LockedDate ,@BillDate,@Description ,@CreatedUser
        ,getdate(),@ModifiedUser,getdate(),@Requester)";

        string ProgramID = SubtitleList.ProgramID;
        string ProgramAbbrev = SubtitleList.ProgramAbbrev;
        int ProgramLength = Convert.ToInt32(SubtitleList.ProgramLength);
        string Description = "申請人: " + SubtitleList.Requester.Split(' ')[1] + "\n分　機: " + SubtitleList.CallNo +
        "\n手　機: " + String.Format("0{0:###-###-###}", Convert.ToInt32(SubtitleList.Mobile)) + "\n說　明: " + SubtitleList.Description;

        string UserID = SubtitleList.UserID;
        string Requester = SubtitleList.Requester;
        foreach (SubtitleTable Subtitle in SubtitleList.table)
        {
            int Episode = Convert.ToInt32(Subtitle.Episode);
            //int ProgramLength = Convert.ToInt32(Subtitle.ProgramLength);
            string ClassificationCode = Subtitle.ClassificationCode;

            // 申請節目代號 - 申請節目名稱 - 集數(內容) for Send Email
            stremailSubtitle += "<TR><TD>" + ProgramID + "</TD><TD>" + ProgramAbbrev + "</TD><TD>" + Episode + "</TD></TR>";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.Add("@ProgramID", SqlDbType.NVarChar).Value = ProgramID;
                command.Parameters.Add("@Episode", SqlDbType.Int).Value = Episode;
                command.Parameters.Add("@ProgramLength", SqlDbType.Int).Value = ProgramLength;
                command.Parameters.Add("@EditorID", SqlDbType.NVarChar).Value = "";
                command.Parameters.Add("@Classification", SqlDbType.NVarChar).Value = ClassificationCode;
                command.Parameters.Add("@SubtitleCostPerEpisode", SqlDbType.Int).Value = 0;
                command.Parameters.Add("@SubtitleComparePerEpisode", SqlDbType.Int).Value = 0;
                command.Parameters.Add("@ImportDate", SqlDbType.DateTime).Value = Convert.ToDateTime("9999-12-31 23:59:59.000");
                command.Parameters.Add("@SubtitleFilename", SqlDbType.NVarChar).Value = "";
                command.Parameters.Add("@Status", SqlDbType.NChar).Value = "N";
                command.Parameters.Add("@LockedDate", SqlDbType.DateTime).Value = Convert.ToDateTime("9999-12-31 23:59:59.000");
                command.Parameters.Add("@BillDate", SqlDbType.Int).Value = -1;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
                command.Parameters.Add("@CreatedUser", SqlDbType.NVarChar).Value = UserID;
                command.Parameters.Add("@ModifiedUser", SqlDbType.NVarChar).Value = UserID;
                command.Parameters.Add("@Requester", SqlDbType.NVarChar).Value = Requester;
                /*
                command.Parameters["@ProgramID"].Value = ProgramID;
                command.Parameters["@Episode"].Value = Episode;
                command.Parameters["@ProgramLength"].Value = ProgramLength;
                command.Parameters["@EditorID"].Value = "";
                command.Parameters["@Classification"].Value = ClassificationCode;
                command.Parameters["@SubtitleCostPerEpisode"].Value = 0;
                command.Parameters["@SubtitleComparePerEpisode"].Value = 0;
                command.Parameters["@ImportDate"].Value = Convert.ToDateTime("9999-12-31 23:59:59.000");
                command.Parameters["@SubtitleFilename"].Value = "";
                command.Parameters["@Status"].Value = "N";
                command.Parameters["@LockedDate"].Value = Convert.ToDateTime("9999-12-31 23:59:59.000");
                command.Parameters["@BillDate"].Value = -1;
                command.Parameters["@Description"].Value = Description;
                command.Parameters["@CreatedUser"].Value = UserID;
                command.Parameters["@ModifiedUser"].Value = UserID;
                command.Parameters["@Requester"].Value = Requester;
                */

                try
                {
                    connection.Open();
                    result += command.ExecuteNonQuery();
                    //測試用
                    //result = 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        if (result > 0)
        {

            // 申請同工
            string stremailBody = "申請同工 ： " + Requester + "<BR>";
            stremailBody += "分機 : " + SubtitleList.CallNo + "<BR>";
            stremailBody += "手機 : " + String.Format("0{0:###-###-###}", Convert.ToInt32(SubtitleList.Mobile)) + "<BR>";
            stremailBody += "申請日期 : " + String.Format("{0:yyyy/MM/dd}", DateTime.Today) + "<BR><BR>";
            // 申請節目代號 - 申請節目名稱 - 集數(標題)
            stremailBody += "<TABLE><TR><TD>申請節目代號：</TD><TD>申請節目名稱：</TD><TD>集數：</TD></TR>";
            // 申請節目代號 - 申請節目名稱 - 集數(內容)
            stremailBody += stremailSubtitle + "</TABLE>";
            stremailBody += "<BR>此為系統發出之信件，請勿回覆此信件！<BR>";

            string strEmailFrom = "pms@goodtv.tv";
            string strEmailFromName = "字幕申請系統";
            string Subject = "字幕申請通知單";
            string[] arryToEmail = GetEMailAddressList("字幕申請通知單");
            SendEmail(arryToEmail, strEmailFrom, strEmailFromName, Subject, stremailBody);
        }
        else
        {
            // 發送錯誤的email
            string stremailBody = "字幕申請儲存錯誤通知：<BR>";
            stremailBody += "錯誤點：Web Service InsertSubtitle insert [_ST03P0] Error<BR><BR>";
            stremailBody += "<TABLE><TR><TD>申請節目代號：</TD><TD>申請節目名稱：</TD><TD>集數：</TD></TR>";
            // 申請節目代號 - 申請節目名稱 - 集數(內容)
            stremailBody += stremailSubtitle + "</TABLE>";
            stremailBody += "<BR>新片庫系統內部通知<BR>";
            string strEmailFrom = "pms@goodtv.tv";
            string strEmailFromName = "字幕申請系統";
            string[] arryToEmail = GetEMailAddressList("錯誤內部通知");
            string Subject = "字幕申請儲存錯誤通知";
            SendEmail(arryToEmail, strEmailFrom, strEmailFromName, Subject, stremailBody);

        }

        return result;

    }

    [WebMethod]
    public string GetSubtitle(string ProgramID, int Episode1, int Episode2, int GetDataDay)
    {

        var result = "";
        string query = @"
	SELECT a.[ProgramID],a.[Episode],a.[ProgramLength],a.[EditorID] ,d.[ClassificationName] as [Classification],a.[SubtitleCostPerEpisode]
    ,a.[SubtitleComparePerEpisode],CONVERT(VARCHAR(10),a.[RequestDate],111) as [RequestDate]
	,case when cast(a.[ImportDate] as date)=cast('9999-12-31' as date) then null else CONVERT(VARCHAR(10),a.[ImportDate],111) end as [ImportDate]
	,a.[SubtitleFilename],a.[Status],d.[ClassificationCode],b.[ProgramLength] as [ProgramLength_Source]
	,case when cast(a.[LockedDate] as date)=cast('9999-12-31' as date) then null else CONVERT(VARCHAR(10),a.[LockedDate],111) end as [LockedDate]
	,a.[BillDate],a.[Description],b.[ProgramAbbrev],ISNULL(c.[EditorName],'') as [EditorName] ,a.[Requester]
	,case when cast(a.[LockedDate] as date) < cast('9999-12-31 23:59:59' as date) then 'locked.png' 
	 when cast(a.[ImportDate] as date) < cast('9999-12-31 23:59:59' as date) then 'approved.png' else 'approving.png' end as [Image]
	,case when cast(a.[LockedDate] as date) < cast('9999-12-31 23:59:59' as date) then '已鎖定' 
	 when cast(a.[ImportDate] as date) < cast('9999-12-31 23:59:59' as date) then '已完成' else '申請中' end as [Tooltiptext]
	,(case when ISNULL(e.[Subtitle],'') <> '' then '1' else '0' end) as TXT
	,(case when ISNULL(e.[SubtitleSRT],'') <> '' then '1' else '0' end) as SRT
    FROM [_ST03P0] as a 
    INNER JOIN [_TM01P0] as b ON a.[ProgramID] = b.[ProgramID] 
    LEFT JOIN [_ST01P0] as c ON a.[EditorID] = c.[EditorID] 
	LEFT JOIN [Classification] as d ON a.[Classification] = d.[ClassificationCode] 
	and rtrim(d.[GroupName]) = '製作類別'
	LEFT JOIN [_ST03P1] as e ON a.[ProgramID] = e.[ProgramID] and a.[Episode] = e.[Episode]
        WHERE 1=1 ";
        if (!String.IsNullOrEmpty(ProgramID.Trim()))
            query += " and a.[ProgramID] like '" + ProgramID.Trim() + "%'";
        if (Episode1 > 0)
            query += " AND a.[Episode] >= " + Episode1;
        if (Episode2 > 0)
            query += " AND a.[Episode] <= " + Episode2;
        if (GetDataDay > 0)
            query += " AND cast(a.[RequestDate] as date) >= cast(getdate()-" + GetDataDay + " as date) ";

        DataSet ds = queryDataTable(query);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        jss.MaxJsonLength = Int32.MaxValue;
        result = jss.Serialize(list);

        return result;

    }

    [WebMethod]
    public string GetSubtitle2(string ProgramID, int Episode)
    {

        string result = "";
        string query = @"SELECT TOP 1 [Subtitle] FROM [_ST03P1] WHERE [ProgramID] = '" + ProgramID + "' AND [Episode] = " + Episode;

        DataSet ds = queryDataTable(query);

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                result = row[col].ToString();
            }
        }
        
        return result;

    }

    [WebMethod]
    public string CheckExistSubtitle(string ProgramID, int Episode)
    {

        string result = "";
        string query = @"SELECT TOP 1 [ProgramID] FROM [_ST03P0] WHERE [ProgramID] = '" + ProgramID + "' AND [Episode] = " + Episode;

        DataSet ds = queryDataTable(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            result = ds.Tables[0].Rows[0]["ProgramID"].ToString();
        }
        else
        {
            result = "";
        }
        return result;

    }

    [WebMethod]
    public string[] GetProgramID()
    {

        string query = @"SELECT distinct [ProgramID] FROM [_ST03P0] order by [ProgramID] ";
        ArrayList array = new ArrayList();

        DataSet ds = queryDataTable(query);
        DataTable dt = ds.Tables[0];

        foreach (DataRow dr in dt.Rows)
        {
            array.Add(dr["ProgramID"].ToString());
        }

        return (string[])array.ToArray(typeof(string));     

    }

    [WebMethod]
    public string GetProgramName(string ProgramID)
    {

        string query = @"SELECT distinct [ProgramID],[ProgramAbbrev],[ProgramLength] FROM [_TM01P0] WHERE [ProgramID] = '" + ProgramID + "' ";

        DataSet ds = queryDataTable(query);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        jss.MaxJsonLength = Int32.MaxValue;

        return jss.Serialize(list);

    }

    [WebMethod]
    public string GetProgramData(string ProgramID, string ProgramAbbrev)
    {

        string query = @"SELECT [ProgramID],[ProgramTitle],[ProgramAbbrev],[programEnglishTitle],[ProgramLength] FROM [_TM01P0] WHERE 1=1 ";
        if (!String.IsNullOrEmpty(ProgramID))
        {
            query += " AND [ProgramID] like '" + ProgramID + "%' ";
        }
        if (!String.IsNullOrEmpty(ProgramAbbrev))
        {
            query += " AND ProgramAbbrev like '%" + ProgramAbbrev + "%' ";
        }

        DataSet ds = queryDataTable(query);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        jss.MaxJsonLength = Int32.MaxValue;

        return jss.Serialize(list);

    }

    [WebMethod]
    public string GetClassification()
    {

        //2014/12/16 依照啟用欄位顯示，啟用欄位由使用者自行維護
        string query = @"SELECT [Classification],[Length],[Amount],[Description] ,b.[ClassificationName]
            FROM [_ST02P0] as a
            JOIN [Classification] AS b ON a.[Classification] = b.[ClassificationCode]
            AND RTRIM([GroupName]) = '製作類別'  WHERE [Enable] = 1 ";
        
        DataSet ds = queryDataTable(query);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        jss.MaxJsonLength = Int32.MaxValue;

        return jss.Serialize(list);

    }


    [WebMethod]
    public string CheckClassification(string Classification)
    {

        if (String.IsNullOrEmpty(Classification)) return "";

        string query = @"SELECT [Classification] FROM [_ST02P0] WHERE [Enable] = 1 ";
        query += " AND [Classification] = '" + Classification + "' ";

        DataSet ds = queryDataTable(query);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        jss.MaxJsonLength = Int32.MaxValue;

        return jss.Serialize(list);

    }

    [WebMethod]
    public int UpdateSubtitle(string ProgramID, int Episode, string EditorID, string Classification,bool checkSRT,
        int SubtitleCostPerEpisode, int SubtitleComparePerEpisode, string Description, string ImportDate, string UserID)
    {

        string strHouseNo = checkSRT ? getHouseNoForSRTExport(ProgramID, Episode) : "";
        int result = 0;
        DateTime dtImportDate = String.IsNullOrEmpty(ImportDate) ? Convert.ToDateTime("9999-12-31 23:59:59") : 
            Convert.ToDateTime(ImportDate);
        string commandText = @"UPDATE [_ST03P0]
            SET EditorID=@EditorID,Classification=@Classification,[Description]=@Description,ImportDate=@ImportDate
	        ,SubtitleCostPerEpisode=@SubtitleCostPerEpisode,SubtitleComparePerEpisode=@SubtitleComparePerEpisode
            ,ModifiedUser=@ModifiedUser,ModifiedDatetime=GETDATE() ";
        if (!String.IsNullOrEmpty(strHouseNo))
        {
            commandText += ",SubtitleFilename=@SubtitleFilename ";
        }
        commandText += " WHERE ProgramID=@ProgramID AND Episode=@Episode";

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            //條件欄位
            command.Parameters.Add("@ProgramID", SqlDbType.NVarChar).Value = ProgramID;
            command.Parameters.Add("@Episode", SqlDbType.Int).Value = Episode;
            //command.Parameters["@ProgramID"].Value = ProgramID;
            //command.Parameters["@Episode"].Value = Episode;
            //更新欄位
            command.Parameters.Add("@EditorID", SqlDbType.NVarChar).Value = EditorID;
            command.Parameters.Add("@Classification", SqlDbType.NVarChar).Value = Classification;
            command.Parameters.Add("@SubtitleCostPerEpisode", SqlDbType.Int).Value = SubtitleCostPerEpisode;
            command.Parameters.Add("@SubtitleComparePerEpisode", SqlDbType.Int).Value = SubtitleComparePerEpisode;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
            command.Parameters.Add("@ImportDate", SqlDbType.DateTime).Value = dtImportDate;
            command.Parameters.Add("@ModifiedUser", SqlDbType.NVarChar).Value = UserID;
            /*
            command.Parameters["@EditorID"].Value = EditorID;
            command.Parameters["@Classification"].Value = Classification;
            command.Parameters["@SubtitleCostPerEpisode"].Value = SubtitleCostPerEpisode;
            command.Parameters["@SubtitleComparePerEpisode"].Value = SubtitleComparePerEpisode;
            command.Parameters["@Description"].Value = Description;
            command.Parameters["@ImportDate"].Value = ImportDate;
            command.Parameters["@ModifiedUser"].Value = UserID;
            */
            if (!String.IsNullOrEmpty(strHouseNo))
            {
                command.Parameters.Add("@SubtitleFilename", SqlDbType.NVarChar).Value = strHouseNo + ".srt";
                //command.Parameters["@SubtitleFilename"].Value = strHouseNo + ".srt";
            }

            try
            {
                connection.Open();
                result = command.ExecuteNonQuery();
                //測試用
                //result = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return result;

    }

    [WebMethod]
    public string GetEditor(string queryInput)
    {

        string query = @"
            SELECT a.[EditorID],[EditorName],[LandPhone],[MobilePhone],[Comparison]
	              ,REPLACE(REPLACE(left(b.[EditorDept],len(b.[EditorDept])-1),'300','節目部'),'400','國外部') as [EditorDept]
            FROM [_ST01P0] as a
            JOIN (SELECT [EditorID],(SELECT [EditorDept]+',' FROM [_ST01M1]
            WHERE [EditorID] = m.EditorID FOR XML PATH('')) as [EditorDept]
            FROM [_ST01M1] as m GROUP BY [EditorID]) as b
            ON a.[EditorID] = b.[EditorID]
        ";
        if (!String.IsNullOrEmpty(queryInput.Trim()))
            query += " WHERE a.[EditorName] like '" + queryInput.Trim() + "%'";

        DataSet ds = queryDataTable(query);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        jss.MaxJsonLength = Int32.MaxValue;

        return jss.Serialize(list);

    }

    [WebMethod]
    public string GetEditorName(string EditorID)
    {

        string query = "SELECT [EditorName] FROM [_ST01P0] where [EditorID] = '" + EditorID.Trim() + "' ";

        DataSet ds = queryDataTable(query);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        jss.MaxJsonLength = Int32.MaxValue;

        return jss.Serialize(list);

    }

    [WebMethod]
    public string UploadFile(string fileName, string fileContent)
    {

        String UploadPath = Server.MapPath("~//uploads");

        if (!Directory.Exists(UploadPath))
        {
            Directory.CreateDirectory(UploadPath);
        }
        string savePathFilename = UploadPath + "//" + fileName.Split('.')[0] + "_" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".txt";

        //檢查
        try
        {
            StreamWriter sw = new StreamWriter(savePathFilename, false, Encoding.Unicode);
            if (null != sw)
            {
                sw.WriteLine((fileContent.IndexOf("\r\n") == -1) ? fileContent.Replace("\n", "\r\n") : fileContent);
                sw.Close();
            } 

            return "ok";
        }
        catch (Exception ex)
        {
            //return ex.Message;
            Console.WriteLine(ex.Message);
        }
        return "";
    }

    [WebMethod]
    public int UpdateLocked(string ProgramID, int Episode, string UserID)
    {

        var result = 0;

        string commandText = @"
            UPDATE [_ST03P0]
            SET [Status]='Y',[LockedDate]=GETDATE(),[ModifiedUser]=@ModifiedUser,[ModifiedDatetime]=GETDATE()
            WHERE ISNULL([Status],'') <> 'Y' AND ProgramID=@ProgramID AND Episode=@Episode
       ";

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.Add("@ProgramID", SqlDbType.NVarChar).Value = ProgramID;
            command.Parameters.Add("@Episode", SqlDbType.Int).Value = Episode;
            command.Parameters.Add("@ModifiedUser", SqlDbType.NVarChar).Value = UserID;

            //command.Parameters["@ProgramID"].Value = ProgramID;
            //command.Parameters["@Episode"].Value = Episode;
            //command.Parameters["@ModifiedUser"].Value = UserID;

            try
            {
                connection.Open();
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return result;

    }

    [WebMethod]
    public int UpdateUnLock(string ProgramID, int Episode, string UserID)
    {

        var result = 0;

        string commandText = @"
            UPDATE [_ST03P0]
            SET [Status]='N',[LockedDate]=cast('9999-12-31 23:59:59' as datetime),[ModifiedUser]=@ModifiedUser,[ModifiedDatetime]=GETDATE()
            WHERE ProgramID=@ProgramID AND Episode=@Episode
       ";
        //ISNULL([Status],'') = 'Y' AND 
        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.Add("@ProgramID", SqlDbType.NVarChar).Value = ProgramID;
            command.Parameters.Add("@Episode", SqlDbType.Int).Value = Episode;
            command.Parameters.Add("@ModifiedUser", SqlDbType.NVarChar).Value = UserID;

            //command.Parameters["@ProgramID"].Value = ProgramID;
            //command.Parameters["@Episode"].Value = Episode;
            //command.Parameters["@ModifiedUser"].Value = UserID;

            try
            {
                connection.Open();
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return result;

    }

    [WebMethod]
    public int SubtitleDelete(string ProgramID, int Episode)
    {

        var result = 0;

        string commandText = @"DELETE [_ST03P0] WHERE ProgramID=@ProgramID AND Episode=@Episode";

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.Add("@ProgramID", SqlDbType.NVarChar).Value = ProgramID;
            command.Parameters.Add("@Episode", SqlDbType.Int).Value = Episode;

            //command.Parameters["@ProgramID"].Value = ProgramID;
            //command.Parameters["@Episode"].Value = Episode;

            try
            {
                connection.Open();
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return result;

    }

    [WebMethod]
    public string CheckSubtitle(string textContent)
    {
        //過濾掉不正常的檔頭
        string textContent1 = textContent.Substring(0, 2);
        string textContent2 = textContent.Substring(2);
        string newContent1 = "";
        string ok = "ok";
        for (int i = 0; i < textContent1.Length; i++)
        {
            char ii = textContent1[i];
            if (ii != 65279)
            {
                newContent1 += textContent1[i];
            }
            else
            {
                //有過濾掉不正常的檔頭，紀錄回傳更新的字幕內容
                ok = "update";
            }
        }
        textContent = newContent1 + textContent2;
        string strSRT = "";
        if (textContent.Substring(0, 11).Split(':').Length == 4)
        {
            strSRT = setTxtToSrt(textContent, "1234567", 0, "CheckSubtitle");
        }
        if (String.IsNullOrEmpty(strSRT))
        {
            //不是TC格式
            return "ok";
        }
        else if (strSRT.Substring(0, 5) == "ERROR")
        {
            //TC時間或間距不對
            return strSRT;
        }
        else if (ok == "update")
        {
            return ok + textContent;
        }
        else
        {
            return ok;
        }

    }

    [WebMethod]
    public string UploadSubtitleContent(string ProgramID, int Episode, string textContent, string ProgramName
        , string Requester, bool sendSrtUpdateEmail, string UserID)
    {

        string result = "";
        //bool sendSrtUpdateEmail = strSRTUpdateEmail == "true" ? true : false;
        //SRT字幕轉換
        string strSRT = "";
        if (textContent.Substring(0, 11).Split(':').Length == 4)
        {
            strSRT = setTxtToSrt(textContent, ProgramID, Episode, ProgramName);
        }

        bool intUpdateSRT = false;
        if (String.IsNullOrEmpty(strSRT))
        {
            //不是TC格式
            //return "ok";
        }
        else if (strSRT.Substring(0, 5) == "ERROR")
        {
            //TC時間或間距不對
            return strSRT.Substring(5);
        }
        else
        {
            intUpdateSRT = true;
        }

        //純字幕轉換
        string strSubtitleWeb = setTxtToSubtitleWeb(textContent);

        //儲存資料庫
        int intSaveDB = 0;
        /*
        string commandText = @"UPDATE [_ST03P1] SET [Subtitle]=@Subtitle,[SubtitleWeb]=@SubtitleWeb,[SubtitleSRT]=@SubtitleSRT
                            ,[ModifiedUser]=@ModifiedUser,[ModifiedDatetime]=GETDATE()
                            WHERE ProgramID=@ProgramID AND Episode=@Episode ";
        */
        string commandText = @" declare @cut int = 0
             SELECT @cut = count([ProgramID])  FROM [_ST03P1] where [ProgramID] = @ProgramID and [Episode] = @Episode
             if @cut > 0
             begin
               UPDATE [_ST03P1] SET [Subtitle]=@Subtitle,[SubtitleWeb]=@SubtitleWeb,[SubtitleSRT]=@SubtitleSRT
               ,[ModifiedUser]=@ModifiedUser,[ModifiedDatetime]=GETDATE()  WHERE ProgramID=@ProgramID AND Episode=@Episode
             end
             else
             begin
               INSERT INTO [_ST03P1] ([ProgramID] ,[Episode] ,[Subtitle] ,[CreatedUser] 
              ,[CreatedDatetime] ,[ModifiedUser] ,[ModifiedDatetime] ,[SubtitleSRT] ,[SubtitleWeb])
              VALUES (@ProgramID ,@Episode ,@Subtitle ,@ModifiedUser ,GETDATE()  ,@ModifiedUser ,GETDATE() ,@SubtitleSRT ,@SubtitleWeb) 
             end
        ";

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.Add("@ProgramID", SqlDbType.NVarChar).Value = ProgramID;
            command.Parameters.Add("@Episode", SqlDbType.Int).Value = Episode;
            command.Parameters.Add("@SubtitleWeb", SqlDbType.NVarChar).Value = strSubtitleWeb;
            command.Parameters.Add("@Subtitle", SqlDbType.NVarChar).Value = textContent;
            command.Parameters.Add("@ModifiedUser", SqlDbType.NVarChar).Value = UserID;
            command.Parameters.Add("@SubtitleSRT", SqlDbType.NVarChar).Value = strSRT;
            /*
            command.Parameters["@ProgramID"].Value = ProgramID;
            command.Parameters["@Episode"].Value = Episode;
            command.Parameters["@SubtitleWeb"].Value = strSubtitleWeb;
            command.Parameters["@Subtitle"].Value = textContent;
            command.Parameters["@ModifiedUser"].Value = UserID;
            command.Parameters["@SubtitleSRT"].Value = strSRT;
            */

            try
            {
                connection.Open();
                intSaveDB = command.ExecuteNonQuery();
                //intSaveDB = 1; //測試用
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        // 字幕處理作業完發送email
        string strEmailFrom = "pms@goodtv.tv";
        //string strRequester = $("#Requester").val();

        if (intSaveDB > 0)
        {

            // 申請同工
            string stremailBody = "申請同工 ： " + Requester + "<BR><BR>";
            // 申請節目代號 - 申請節目名稱 - 集數(標題)
            stremailBody += "<TABLE><TR><TD>申請節目代號：</TD><TD>申請節目名稱：</TD><TD>集數：</TD></TR>";
            // 申請節目代號 - 申請節目名稱 - 集數(內容)
            stremailBody += "<TR><TD>" + ProgramID + "</TD><TD>" + ProgramName + "</TD><TD>" + Episode + "</TD></TR></TABLE>";
            stremailBody += "<BR>說明：本集字幕己完成，並己匯入字幕系統<BR>此為系統發出之信件，請勿回覆此信件！<BR>";
            //stremailBody += "新片庫系統通知<BR>";

            //string strEmailFrom = System.Configuration.ConfigurationManager.AppSettings["MailFrom"];
            //string strEmailFromName = System.Configuration.ConfigurationManager.AppSettings["EmailFromName"];
            string strEmailFromName = "字幕匯入確認系統";
            string Subject = "字幕上傳確認通知單";
            string[] arryToEmail = GetEMailAddressList("字幕處理通知");
            SendEmail(arryToEmail, strEmailFrom, strEmailFromName, Subject, stremailBody);

            //取得申請同工Email
            /* 測試中取消*/
            HRService sr = new HRService();
            string[] RequesterEmail = { sr.GetEMailAddress(Requester.Split(' ')[0]) };
            SendEmail(RequesterEmail, strEmailFrom, strEmailFromName, Subject, stremailBody);

            if (intUpdateSRT)
            {

                //bool sendSrtUpdateEmail = true; //測試用
                string strHouseNo = getHouseNoForSRTExport(ProgramID,Episode);
                if (!String.IsNullOrEmpty(strHouseNo))
                {

                    // 如果_ST037()不為空值，表示不是第一次上傳字幕檔(不是第一次上傳字幕檔，又轉拋字幕 => 發送Email 通知主控人員有轉拋字幕)
                    //if (hasSRT)
                    //{
                    //    sendSrtUpdateEmail = true;
                    //}
                    //HouseNo檔案儲存到SRT目錄
                    string strExportDir = System.Configuration.ConfigurationManager.AppSettings["srtPath"];
                    using (StreamWriter sw = new StreamWriter(strExportDir + "\\" + strHouseNo + ".srt", false, Encoding.UTF8))
                    {
                        sw.Write(strSRT.Replace("\r\n","\n"));
                    }

                    // 不是第一次上傳字幕，且有HouseNo的話發送Email通知
                    if (sendSrtUpdateEmail)
                    {

                        // 發送 SRT Update email
                        // 信件內容：
                        // 異動日期
                        stremailBody = "異動日期：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + "<BR>";
                        // 異動字幕之所屬節目名稱
                        stremailBody += "異動字幕之所屬節目名稱：" + ProgramName + "  節目代碼: " + ProgramID + "  節目集數:" + Episode + "<BR>";
                        // 異動字幕檔名
                        stremailBody += "異動字幕檔名：" + strHouseNo + ".srt" + "<BR><BR>";
                        stremailBody += "請主控同工立即在K2 Edge同步更新該字幕檔<BR>";

                        strEmailFromName = "PMS";
                        arryToEmail = GetEMailAddressList("字幕處理通知");

                        Subject = "字幕異動通知, 請同步更新K2 Edge";
                        SendEmail(GetEMailAddressList("SRT字幕更新通知"), strEmailFrom, strEmailFromName, Subject, stremailBody);

                    }
                }
            }

            result = "ok";
        }
        else
        {
            /*
            // 發送錯誤的email
            string stremailBody = "字幕轉換錯誤：<BR>";
            stremailBody += "錯誤點：Web Service UploadSubtitleContent update [_ST03P1] Error<BR><BR>";
            stremailBody += "節目代號：" + ProgramID + "<BR>";
            stremailBody += "節目集數；" + Episode + "<BR>";
            stremailBody += "節目名稱；" + ProgramName + "<BR><BR>";
            stremailBody += "新片庫系統內部通知<BR>";
            //string strEmailFrom = System.Configuration.ConfigurationManager.AppSettings["MailFrom"];
            //string strEmailFromName = System.Configuration.ConfigurationManager.AppSettings["EmailFromName"];
            string strEmailFromName = "字幕匯入系統";
            string[] arryToEmail = GetEMailAddressList("字幕轉換錯誤通知");
            string Subject = "字幕轉換錯誤通知";
            SendEmail(arryToEmail, strEmailFrom, strEmailFromName, Subject, stremailBody);
            result = "error儲存字幕錯誤！";
            */
            result = "資料庫儲存失敗！";
        }
        return result;

    }

    [WebMethod]
    public string[] GetEMailAddressList(string MailType)
    {
        string query = @"SELECT distinct [EMailName],[EMailAddress] FROM [EMailAddressList] " +
                        " WHERE [Enable] = 1 AND [EMailType] = '" + MailType + "' ";
        ArrayList array = new ArrayList();

        DataSet ds = queryDataTable(query);
        DataTable dt = ds.Tables[0];

        foreach (DataRow dr in dt.Rows)
        {
            if (String.IsNullOrEmpty(dr["EMailName"].ToString()))
            {
                array.Add(dr["EMailAddress"].ToString());
            }
            else
            {
                array.Add(dr["EMailName"].ToString() + " <" + dr["EMailAddress"].ToString() + ">");
            }
        }

        return (string[])array.ToArray(typeof(string));

    }

    [WebMethod]
    public string getHouseNoForSRTExport(string ProgramID, int Episode)
    {

        string result = "";

        string commandText = @"	SELECT HouseNo FROM (
	            SELECT top 1 PresentationTitle,EpisodeNumber,HouseNo,[Date],
		            Case When Len(IsNull([Comment],''))> 0 Then 
			            (Case When Substring([Type],1,1)='Y' And Substring([Comment],1,1)='Y' Then 'Y' 
					            When Substring([Type],1,1)='Y' And Substring([Comment],1,1)='N' Then 'N' 
					            When Substring([Type],1,1)='N' And Substring([Comment],1,1)='Y' Then 'Y' 
					            When Substring([Type],1,1)='N' And Substring([Comment],1,1)='N' Then 'N' 
				            Else '' End)
		            Else (Case When Substring([Type],1,1)='Y' Then 'Y' 
					            When Substring([Type],1,1)='N' Then 'N' 
				            Else '' End) End [TypeCommnet]
		            FROM [dbo].[srtmcs_cue]
		            WHERE SUBSTRING(LTRIM(PresentationTitle),1,LEN(@ProgramID)) like @ProgramID AND EpisodeNumber  = @Episode
		            ORDER BY [Date] DESC
	            ) CUE WHERE [TypeCommnet] = 'N'";

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.Add("@ProgramID", SqlDbType.NVarChar).Value = ProgramID;
            command.Parameters.Add("@Episode", SqlDbType.Int).Value = Episode;

            //command.Parameters["@ProgramID"].Value = ProgramID;
            //command.Parameters["@Episode"].Value = Episode;

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    //result += dr["HouseNo"].ToString();
                    result = dr["HouseNo"].ToString();
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return result;

    }

    [WebMethod]
    public string GetStatistics(string SDate, string EDate, int Year = 0, int Month = 0, string DeptID = "", string Mode = "")
    {

        string result = "";
        if ((String.IsNullOrEmpty(SDate) || String.IsNullOrEmpty(EDate)) && (Year <= 0 || Month <= 0))
        {
            return result;
        }

        string query1 = @"SELECT [EditorID],[EditorName],[DeptName],COUNT(*) as EditorCount,SUM([SubtitleCostPerEpisode]) AS CostSum
            ,AVG([SubtitleComparePerEpisode]) AS Compare FROM (
            SELECT  a.[EditorID],c.[EditorName],e.[DeptName],e.[DeptID]
            ,[SubtitleCostPerEpisode],[SubtitleComparePerEpisode],[ImportDate]
            ,case when [BillDate] = -1 then '' when [BillDate]%12=0 then rtrim(cast([BillDate]/12-1 as char))+'-12'
             else rtrim(cast([BillDate]/12 as char))+'-'+rtrim(cast([BillDate]%12 as char)) end as [Bill_Date]
            FROM [_ST03P0] as a
            LEFT JOIN [_ST01P0] as c ON a.[EditorID] = c.[EditorID]
            JOIN [_ST01M1] as d ON a.[EditorID] = d.[EditorID]
            LEFT JOIN [Dept] as e ON d.[EditorDept] = e.[DeptID] ) as main  where 1=1 ";
        if (Mode == "account")
        {
            if (Year > 0 && Month > 0)
                query1 += " and [Bill_Date] = '' and YEAR([ImportDate]) = " + Convert.ToString(Year) + " and MONTH([ImportDate]) = " + 
                    Convert.ToString(Month);
        }
        else
        {
            if (Year > 0 && Month > 0)
                query1 += " and [Bill_Date] = '" + Convert.ToString(Year) + "-" + Convert.ToString(Month) + "' ";
            if (!String.IsNullOrEmpty(SDate) && !String.IsNullOrEmpty(EDate))
                query1 += " and cast([ImportDate] as date) between cast('" + SDate + "' as date) and cast('" + EDate + "' as date) ";
        }
        if (!String.IsNullOrEmpty(DeptID) && DeptID != "000")
            query1 += " and [DeptID] = '" + DeptID + "' ";
        query1 += " group by [EditorID],[EditorName],[DeptName] order by [EditorID] desc ";

        string query2 = @"SELECT * FROM (SELECT  a.[EditorID],a.[ProgramID],b.[ProgramAbbrev],[Episode],[SubtitleCostPerEpisode]
            ,cast([RequestDate] as date) as [RequestDate],cast([ImportDate] as date) as [ImportDate]
            ,case when [BillDate] = -1 then '' when [BillDate]%12=0 then rtrim(cast([BillDate]/12-1 as char))+'-12'
             else rtrim(cast([BillDate]/12 as char))+'-'+rtrim(cast([BillDate]%12 as char)) end as [Bill_Date]
            FROM [_ST03P0] as a 
            JOIN [_TM01P0] as b ON a.[ProgramID] = b.[ProgramID]
            JOIN [_ST01M1] as d ON a.[EditorID] = d.[EditorID] ) as main  where 1=1 ";
        if (Mode == "account")
        {
            if (Year > 0 && Month > 0)
                query2 += " and [Bill_Date] = '' and YEAR([ImportDate]) = " + Convert.ToString(Year) + " and MONTH([ImportDate]) = " + 
                    Convert.ToString(Month);
        }
        else
        {
            if (Year > 0 && Month > 0)
                query2 += " and [Bill_Date] = '" + Convert.ToString(Year) + "-" + Convert.ToString(Month) + "' ";
            if (!String.IsNullOrEmpty(SDate) && !String.IsNullOrEmpty(EDate))
                query2 += " and cast([ImportDate] as date) between cast('" + SDate + "' as date) and cast('" + EDate + "' as date) ";
        }
        if (!String.IsNullOrEmpty(DeptID) && DeptID != "000")
            query2 += " and [DeptID] = '" + DeptID + "' ";
        query2 += " order by [EditorID] desc,[ProgramID],[Episode] ";

        DataSet ds = queryDataTwoTable(query1, "tables", query2, "table");
        List<StatisticsTables> Statisticslists = new List<StatisticsTables>();

        foreach (DataRow rowForMain in ds.Tables["tables"].Rows)
        {

            List<StatisticsTable> Statisticslist = new List<StatisticsTable>();
            StatisticsTables Tables = new StatisticsTables();
            Tables.EditorID = (string)rowForMain["EditorID"];
            Tables.EditorName = (string)rowForMain["EditorName"];
            Tables.DeptName = (string)rowForMain["DeptName"];
            Tables.SubtitleCostCount = (int)rowForMain["EditorCount"];
            Tables.SubtitleCostSum = (int)rowForMain["CostSum"];
            Tables.SubtitleCompare = (int)rowForMain["Compare"];

            foreach (DataRow row in ds.Tables["table"].Rows)
            {
                if (Tables.EditorID == (string)row["EditorID"])
                {
                    StatisticsTable Table = new StatisticsTable();
                    Table.ProgramID = (string)row["ProgramID"];
                    Table.ProgramAbbrev = (string)row["ProgramAbbrev"];
                    Table.Episode = (int)row["Episode"];
                    Table.SubtitleCostPerEpisode = (int)row["SubtitleCostPerEpisode"];
                    Table.RequestDate = String.Format("{0:yyyy/MM/dd}",row["RequestDate"]);
                    Table.ImportDate = String.Format("{0:yyyy/MM/dd}",row["ImportDate"]);
                    Table.Bill_Date = (string)row["Bill_Date"];
                    Statisticslist.Add(Table);
                }
            }
            Tables.table = Statisticslist;
            Statisticslists.Add(Tables);

        }

        result = GetJSONFromObject(Statisticslists);

        return result;

    }

    /** 
     * 發送內部錯誤Email
     * @param EmailFromName 寄件人名稱
     * @param Subject       主旨
     * @param Body          Email內容
     * @return
     */
    [WebMethod]
    public string SendErrorEmail(string EmailFromName, string Subject, string Body)
    {
        string[] MailTo = GetEMailAddressList("錯誤內部通知");
        string MailFrom = "pms@goodtv.tv";
        SendEmail(MailTo, MailFrom, EmailFromName, Subject, Body);
        return "";
    }

    // ==========================================================================
    // 共用函式
    // ==========================================================================

    /// <summary>
    /// 依據SQL語句，回傳DataTable物件
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    private DataSet queryDataTable(string sql)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(constr))
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(ds);
        }

        //return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        return ds;

    }

    /// <summary>
    /// 依據SQL語句，回傳DataTable物件
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    private DataSet queryDataTwoTable(string sql1, string tablename1, string sql2, string tablename2)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(constr))
        {
            SqlDataAdapter da1 = new SqlDataAdapter(sql1, conn);
            da1.Fill(ds, tablename1);
            SqlDataAdapter da2 = new SqlDataAdapter(sql2, conn);
            da2.Fill(ds, tablename2);
        }

        //return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        return ds;

    }

    /// <summary>
    /// 判斷是否為時間
    /// </summary>
    /// <param name="pTime">傳入字串。</param>
    public static bool IsTime(string pTime)
    {
        return Regex.IsMatch(pTime, @"^((20|21|22|23|[0-1]\d):[0-5]\d:[0-5]\d:[0-5]\d)$");
    }

    // 從Java轉移
    /**
     * 將txt檔案格式轉成srt格式
     * @param txt
     * @return
     */
    public static string setTxtToSrt(string txtContent, string ProgramID, int Episode, string ProgramName)
    {

        if (String.IsNullOrEmpty(txtContent) || txtContent.Length < 11 || txtContent.Substring(0, 12).Split(':').Length != 4)
        {
            // 如果沒有時間格式的話，就不做轉檔動作
            return "";
        }

        //字幕內容陣列
        string[] txtList = txtContent.IndexOf("\r\n") == -1 ? txtContent.Split('\n') : txtContent.Replace("\r\n", "\n").Split('\n');
        //字幕內容字串
        StringBuilder sb = new StringBuilder();
        //字幕內容錯誤字串
        StringBuilder sbError = new StringBuilder();
        // 字幕內容序號
        int seqno = 1;

        // 2013/12/19 仲平哥說主播系統有問題 SRT檔要增加第一筆資料
        /*
        1
        00:00:00,000 --> 00:00:00,000
        節目代碼 節目名稱 集數 yyyy/MM/dd hh:mm:ss
        備註不對應該是如下
        節目代碼 集數 節目名稱 yyyy/MM/dd hh:mm:ss
        */
        DateTime dateTime = DateTime.Now;
        string createTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss");

        // 字幕內容序號
        sb.AppendLine(seqno.ToString());
        // 時間
        sb.AppendLine("00:00:00,000 --> 00:00:00,000");
        // 文字內容
        sb.AppendLine(ProgramID + " " + Episode + " " + ProgramName + " " + createTime);
        // 空白行
        sb.AppendLine();

        for (int i = 0; i < txtList.Length - 1; i++)
        {
            // 目前當筆字幕內容
            String startStr = txtList[i];
            // 下一筆字幕內容
            String nextStr = txtList[i + 1];

            String[] startData = startStr.Split(' ');
            String[] nextData = nextStr.Split(' ');

            String[] startTime = startData[0].Split(':');
            String[] endTime = nextData[0].Split(':');

            /*
            if (startData.Length < 2)
            {
                continue;
            }
            */
            //過濾掉下一筆沒有時間
            if (endTime.Length < 4)
            {
                continue;
            }

            // 毫秒轉換
            String startMS = padLeft(ConvertMS(startTime[3]).ToString(), 3);
            String endMS = "";
            String time = "";

            // 時間組合字串
            if (ConvertMS(endTime[3]) >= 33)
            {
                endMS = padLeft((ConvertMS(endTime[3]) - 33).ToString(), 3);

                String sTime = startTime[0] + ":" + startTime[1] + ":" + startTime[2] + "," + startMS;
                String eTime = endTime[0] + ":" + endTime[1] + ":" + endTime[2] + "," + endMS;

                time = timeCheckForSRT(sTime, eTime);
                //time = sTime + " --> " + eTime;   //測試用
            }
            else
            {
                String endTime1 = "";
                endMS = padLeft((ConvertMS(endTime[3]) + 967).ToString(), 3);

                try
                {

                    DateTime dt = new DateTime(2000, 1, 1, Convert.ToInt32(endTime[0])
                        , Convert.ToInt32(endTime[1]), Convert.ToInt32(endTime[2]));
                    endTime1 = dt.AddSeconds(-1).ToString("HH:mm:ss");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                String sTime = startTime[0] + ":" + startTime[1] + ":" + startTime[2] + "," + startMS;
                String eTime = endTime1 + "," + endMS;

                time = timeCheckForSRT(sTime, eTime);
                //time = sTime + " --> " + eTime;   //測試用

            }

            if (String.IsNullOrEmpty(time))
            {
                sbError.AppendLine(startData[0]);
            }
            else if (startData.Length > 1)
            {

                //字幕內容序號+1
                seqno++;
                sb.AppendLine(seqno.ToString());
                sb.AppendLine(time);

                //字幕內容的「//」自訂的換行符號
                string context = startStr.Substring(startData[0].Length + 1).Trim();
                if (context.IndexOf("//") > -1)
                {
                    context = context.Replace("//", "/");
                    //原程式沒有只對「/」符號做不換行的防呆，而一起做了換行
                    string[] arrryContext = context.Split('/');
                    for (int k = 0; k < arrryContext.Length; k++)
                    {
                        if (!String.IsNullOrEmpty(arrryContext[k]))
                        {
                            sb.AppendLine(arrryContext[k]);
                        }
                    }

                }
                else
                {
                    sb.AppendLine(context);
                }

                sb.AppendLine();

            }

        }

        // 2014/4/18 檢查錯誤
        if (!String.IsNullOrEmpty(sbError.ToString()))
        {
            return "ERROR" + sbError.ToString();
        }

        return sb.ToString();

    }

    // 從Java轉移
    /** 
     * 為了官網使用的字幕，拿掉時間的邏輯處理
     * @return
     */
    private string setTxtToSubtitleWeb(string textContent)
    {

        if (String.IsNullOrEmpty(textContent))
        {
            // 如果沒有內容，就不做轉檔動作
            return "";
        }

        //字幕內容陣列
        string[] txtList = textContent.Split('\n');
        //字幕內容字串
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < txtList.Length; i++)
        {
            String startStr = txtList[i];

            if (String.IsNullOrEmpty(startStr))
            {
                sb.AppendLine();
            }
            else if (startStr.Split(' ')[0].Split(':').Length == 4)
            {
                if (startStr.Length == 11 || startStr.Length == 12)
                {
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine(startStr.Substring(startStr.Split(' ')[0].Length).Trim());
                }
            }
            else
            {
                sb.AppendLine(startStr);
            }

        }

        return sb.ToString();

    }

    /**
     * 轉換影格時間
     * @return
     */
    public static int ConvertMS(String time)
    {

        double doubleTime = double.Parse(time);
        double ms = (doubleTime / 29.97) * 1000;

        return (int)Math.Round(ms);
    }

   /* 左邊補0 */
    public static string padLeft(string str, int len) {
        str = "" + str;
        if (str.Length >= len) {
            return str;
        } else {
            return padLeft("0" + str, len);
        }
    }

    /* 檢查時間間距 */
    /** 
     * 為了SRT字幕結束時間會小於或等於開始時間所做的邏輯處理
     * @param startTime  hh:mm:ss,SSS
     * @param endTime    hh:mm:ss,SSS
     * @return
     */
    public static string timeCheckForSRT(string startTime, string endTime)
    {

        /*檢查正確的數字字串
        char = 數值
        0~9 = 48~57, : = 58 , = 44
        */
        string newStartTime = "";
        for (int i = 0; i < startTime.Length; i++)
        {
            char ii = startTime[i];
            if (ii >= 48 && ii <= 57 || ii == 44 || ii == 58)
                newStartTime += startTime[i];

        }
        string newEndTime = "";
        for (int i = 0; i < endTime.Length; i++)
        {
            char ii = endTime[i];
            if (ii >= 48 && ii <= 57 || ii == 44 || ii == 58)
                newEndTime += endTime[i];

        }

        startTime = newStartTime;
        endTime = newEndTime;
        //檢查正確的數字字串 結束

        if (startTime.Split(new char[] { ':', ',' }).Length < 4 || endTime.Split(new char[] { ':', ',' }).Length < 4)
        {
            return "";
        }

        /*
        string[] arryDate1 = startTime.Replace(",", ":").Split(':');
        string[] arryDate2 = endTime.Replace(",", ":").Split(':');

        string strHour = arryDate1[0].Trim();
        string strMinute = arryDate1[1].Trim();
        string strSecond = arryDate1[2].Trim();
        string strMilliSecond = arryDate1[3].Trim();
        */

        string strYearMonthMonth = DateTime.Now.ToString("yyyy/MM/dd");
        //string strStartTime = startTime.Substring(0, 8);
        string strStartDateTime = strYearMonthMonth + " " + startTime;
        //string strEndTime = endTime.Substring(0, 8);
        string strEndDateTime = strYearMonthMonth + " " + endTime;

        DateTime dt1;
        bool b1 = DateTime.TryParseExact(strStartDateTime, "yyyy/MM/dd HH:mm:ss,fff", null,
            DateTimeStyles.None, out dt1);
        //DateTime dt1 = parsedDate1.AddMilliseconds(Convert.ToInt32(startTime.Split(',')[1]) + 60);

        DateTime dt2;
        bool b2 = DateTime.TryParseExact(strEndDateTime, "yyyy/MM/dd HH:mm:ss,fff", null,
            DateTimeStyles.None, out dt2);
        //DateTime dt2 = parsedDate1.AddMilliseconds(Convert.ToInt32(endTime.Split(',')[1]));

        if (!(b1 && b2)) return "";

        string timeCode = "";

        // 2014/4/18 增加播放機器容許60ms間距
        //DateTime dt3 = dt1.AddMilliseconds(60);
        // 0=相等 ， -1= eTime > sTime， 1= sTime > eTime
        //int checkTime = dt3.CompareTo(dt2);
        int checkTime = dt1.AddMilliseconds(60).CompareTo(dt2);

        if (checkTime >= 0)
        {
            // 20131223 不做防呆，改成錯誤提醒
            //timeCode = "ERROR" + "=====> startTime : " + startTime + "    endTime : " + endTime + " <=====";
        }
        else
        {
            timeCode = startTime + " --> " + endTime;
        }

        return timeCode;
    }

    /** 
     * 發送Email
     * @param MailTo        收件人Email清單
     * @param MailFrom      寄件人Email
     * @param EmailFromName 寄件人名稱
     * @param Subject       主旨
     * @param Body          Email內容
     * @return
     */
    public bool SendEmail(string[] MailTo, string MailFrom, string EmailFromName, string Subject, string Body)
    {
        bool result = false;
        string SmtpServer = System.Configuration.ConfigurationManager.AppSettings["MailServer"];
        string SmtpAccount = System.Configuration.ConfigurationManager.AppSettings["MailUserID"];
        string SmtpPassword = System.Configuration.ConfigurationManager.AppSettings["MailUserPW"];

        MailAddress Mailfrom = new MailAddress(MailFrom, EmailFromName);
        MailMessage Message = new MailMessage();
        Message.From = Mailfrom;
        Message.Subject = Subject;
        Message.Body = Body;

        //string[] arryEmailTo = MailTo.Split(';');
        foreach (string EmailTo in MailTo)
        {
            Message.To.Add(EmailTo);
        }

        Message.IsBodyHtml = true;

        SmtpClient client = new SmtpClient(SmtpServer, 25);
        try
        {
            client.Credentials = new System.Net.NetworkCredential(SmtpAccount, SmtpPassword);
            client.Send(Message);
            result = true;
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine(ex.Message);
            logger.Error(ex.Message);
        }
        finally
        {
            Message.Dispose();
        }

        return result;
    }

    /// <summary>
    /// 建立指定資料夾與檔案
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2012/9/20, Create
    /// 2.Samson Hsu, 2014/12/25 複製過來使用
    /// </history>
    private bool CreateFile(string ExportDir, string FileName)
    {

        StreamWriter sw;

        try
        {
            //檢查匯出檔是否存在
            string ExportFile = ExportDir + "\\" + FileName + ".srt";
            if (!File.Exists(ExportFile))
            {
                //建立匯出檔
                sw = File.CreateText(ExportFile);
            }
            else
            {
                //先刪除再重新建立匯出檔
                File.Delete(ExportFile);
                sw = File.CreateText(ExportFile);
            }

            //關閉StreamWriter
            sw.Close();
            sw.Dispose();

            return true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            return false;
        }
    }

    //接收JSON的類別
    public class SubtitleTable
    {
        public string Episode { get; set; }
        //public string ProgramLength { get; set; }
        public string ClassificationCode { get; set; }
    }

    public class SubtitleTables
    {
        public string ProgramID { get; set; }
        public string ProgramAbbrev { get; set; }
        public string ProgramLength { get; set; }
        public string CallNo { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
        public string Requester { get; set; }
        public List<SubtitleTable> table { get; set; }
    }

    //反序列轉換
    public static object GetObjectFromJSON(string JSON, Type ObjectType)    //將JSON轉為物件
    {
        JavaScriptSerializer JSONSerializer = new JavaScriptSerializer();
        return JSONSerializer.Deserialize(JSON, ObjectType);
    }

    //產生JSON的類別
    public class StatisticsTable
    {
        public string ProgramID { get; set; }
        public string ProgramAbbrev { get; set; }
        public int Episode { get; set; }
        public int SubtitleCostPerEpisode { get; set; }
        public string RequestDate { get; set; }
        public string ImportDate { get; set; }
        public string Bill_Date { get; set; }
    }

    public class StatisticsTables
    {
        public string EditorID { get; set; }
        public string EditorName { get; set; }
        public string DeptName { get; set; }
        public int SubtitleCostCount { get; set; }
        public int SubtitleCostSum { get; set; }
        public int SubtitleCompare { get; set; }
        public List<StatisticsTable> table { get; set; }
    }

    public static string GetJSONFromObject(object Source)              //將物件轉為JSON
    {
        JavaScriptSerializer JSONSerializer = new JavaScriptSerializer();
        JSONSerializer.MaxJsonLength = Int32.MaxValue;
        return JSONSerializer.Serialize(Source);
    }

    //檢查審核密碼
    [WebMethod]
    public bool CheckAuditPassword(string Password)
    {

        bool result = false;
        string strPassword = System.Configuration.ConfigurationManager.AppSettings["ProgramChangeApplyContent_Auditpassword"];

        if (strPassword == Password)
        {
            result = true;
        }
        return result;

    }


}

