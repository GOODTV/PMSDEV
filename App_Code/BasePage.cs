using System;
using System.Web.UI;
//using log4net;
using System.Reflection;
//using log4net.Appender;

/// <summary>
/// BasePage 的摘要描述
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public CSessionInfo SessionInfo;
    public NpoDB objNpoDB;
    //protected ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    //static log4net.ILog logger = log4net.LogManager.GetLogger("RollingLogFileAppender");
    public log4net.ILog logger = log4net.LogManager.GetLogger("RollingLogFileAppender");
    bool IsRedirect = false;

    //---------------------------------------------------------------------------
    protected void Page_PreInit(object sender, EventArgs e)
    {
        objNpoDB = new NpoDB();

        //取得使用者 Session 物件, 逾時則轉到 default.aspx
        SessionInfo = GetSessionInfo();
        if (SessionInfo == null)
        {
            IsRedirect = true;
            Response.Write(@"<script>window.parent.location.href='../Default.aspx';</script>");
        }
    }
    //---------------------------------------------------------------------------
    protected override void OnLoad(EventArgs e)
    {
        //當 SessionInfo == null 時,避免 Page 繼續往下執行
        if (IsRedirect == true)
        {
            return;
        }

        base.OnLoad(e);
        if (!IsPostBack)
        {
            //CaseUtil.LogData(CaseUtil.LogType.LogTime, SessionInfo.UserID);
        }
    }

    /*
    //---------------------------------------------------------------------------
    public BasePage()
    {
        this.Load += new EventHandler(BasePage_Load);
    }
    //---------------------------------------------------------------------------
    /// <summary>
    /// Handles the Load event of the BasePage control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <history>
    /// 1.Tanya Wu, 2012/8/29, Create
    /// </history>
    private void BasePage_Load(object sender, EventArgs e)
    {
        //驗證 Session
        //this.CheckSessionValidation(); 
    }
    //---------------------------------------------------------------------------

    /// <summary>
    /// 驗證 Session
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2012/8/29, Create
    /// </history>
    ///
    private void CheckSessionValidation()
    {
        try
        {
            if (Session["LoginOK"] == null || Session["LoginOK"].Equals(false))
            {
                string redirectJS = @"parent.location.replace('../Error_Page.aspx');";
                RegisterStartupJS("Redirect", redirectJS);
            }

        }
        catch (Exception ex)
        {
            string redirectJS = @"parent.location.replace('../Error_Page.aspx');";
            RegisterStartupJS("Redirect", redirectJS);

            //this.logger.Error(ex.Message, ex);
            logger.Error(ex.Message, ex);
        }
    }
    */

    //---------------------------------------------------------------------------
    /// <summary>
    /// alert訊息
    /// </summary>
    /// <param name="message">要alert的訊息</param>
    public void AlertMessage(string message)
    {
        string js = "setTimeout(function() { alert('" + EscapeStringForJS(message) + "'); alertFlag = false;},0);";
        //string js = "alert('" + EscapeStringForJS(message) + "');";
        RegisterStartupJS(message, js);
    }

    //---------------------------------------------------------------------------
    /// <summary>
    /// Replace characters for Javscript string literals
    /// 放訊息內文，不要連語法一起進去編碼，例如alert('"+ abc +"')";，應該只編碼abc
    /// </summary>
    /// <param name="text">raw string</param>
    /// <returns>escaped string</returns>
    public static string EscapeStringForJS(string s)
    {
        //REF: http://www.javascriptkit.com/jsref/escapesequence.shtml

        return s.Replace(@"\", @"\\")
                .Replace("\b", @"\b")
                .Replace("\f", @"\f")
                .Replace("\n", @"\n")
                .Replace("\0", @"\0")
                .Replace("\r", @"\r")
                .Replace("\t", @"\t")
                .Replace("\v", @"\v")
                .Replace("'", @"\'")
                .Replace(@"""", @"\""");
    }

    //---------------------------------------------------------------------------

    /// <summary>
    /// Registers the startup JS.
    /// </summary>
    /// <param name="RegisterName">Name of the register.</param>
    /// <param name="myJavascript">My javascript.</param>
    public void RegisterStartupJS(string RegisterName, string myJavascript)
    {
        string wholeJS = "<SCRIPT language=\"JavaScript\"  type=\"text/javascript\" >" + myJavascript + "</SCRIPT>";
        //wholeJS=EscapeStringForJS(wholeJS);
        if (ExistSM())
        { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), RegisterName, wholeJS, false); }
        else
        { Page.ClientScript.RegisterStartupScript(Page.GetType(), RegisterName, wholeJS); }

    }

    //---------------------------------------------------------------------------
    /// <summary>
    /// 檢查頁面上是否存在ScriptManager
    /// </summary>
    /// <returns></returns>
    private bool ExistSM()
    {
        return (ScriptManager.GetCurrent(this.Page) != null);
    }

    //---------------------------------------------------------------------------
    /// <summary>
    /// 取得今天日期 格式：YYYY/MM/DD or YYYYMMDD
    /// </summary>
    public string getToday(string Format = "YYYY/MM/DD")
    {
        string dt = string.Empty;

        if (Format == "YYYYMMDD")
            dt = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Day.ToString().PadLeft(2, '0');
        else
            dt = DateTime.Today.Year.ToString() + "/" + DateTime.Today.Month.ToString().PadLeft(2, '0') + '/' + DateTime.Today.Day.ToString().PadLeft(2, '0');

        return dt;
    }

    //---------------------------------------------------------------------------
    //Session["Msg"]有值時，會Alert，並清空
    public void ShowSysMsg()
    {
        if (Session["Msg"] != null && Session["Msg"].ToString() != "")
        {
            Alert(Session["Msg"].ToString());
            Session["Msg"] = "";
        }
    }
    //---------------------------------------------------------------------------
    //直接顯示訊息
    public void ShowSysMsg(string SysMsg)
    {
        SetSysMsg(SysMsg);
        ShowSysMsg();
    }
    //---------------------------------------------------------------------------
    //設定訊息
    public void SetSysMsg(string SysMsg)
    {
        Session["Msg"] = SysMsg;
    }
    //---------------------------------------------------------------------------
    //顯示警告
    public void Alert(string AlertMessage)
    {
        String cstext = "alert('" + AlertMessage + "');";
        
        CreateJavaScript(cstext);
    }
    //---------------------------------------------------------------------------
    //關閉視窗
    public void JS_CloseWindow()
    {
        String cstext = "window.close();";
        CreateJavaScript(cstext);
    }
    //---------------------------------------------------------------------------
    //父視窗Reload And 關閉目前視窗
    public void JS_OpenReloadAndCloseWindow()
    {
        CreateJavaScript("opener.window.location.reload();window.close();");
    }
    //---------------------------------------------------------------------------
    //嵌入JavaScript
    public void CreateJavaScript(string JavaScript)
    {
        String csname = "PageScript";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, csname, JavaScript, true);
    }
    //---------------------------------------------------------------------------
    //取得登入者資訊
    public CSessionInfo GetSessionInfo()
    {
      if (Session["SessionInfo"] != null && Session["SessionInfo"].ToString() != "")
      {
          return (CSessionInfo)Session["SessionInfo"];
      }
      return null;
    }
    //---------------------------------------------------------------------------
    public void ClearSessionInfo()
    {
        SessionInfo.Clear();
    }
    //---------------------------------------------------------------------------
    //Session判斷
    protected void SessionValid()
    {
        if (Session["SessionInfo"] != null && Session["SessionInfo"].ToString() != "")
        {
            Response.Write(@"<script>window.parent.location.href='../Default.aspx';</script>");
            Response.End();
        }
    }
    //---------------------------------------------------------------------------
    // 2014/4/3 另外新增一個顯示訊息函式 for Samson
    protected void AjaxShowMessage(string Message)
    {
        string scriptMsg = string.Empty;
        string messageMsg = string.Empty;
        messageMsg = Message.Replace("'", "\\");
        messageMsg = messageMsg.Replace("\r\n", "\\n");
        scriptMsg = string.Format("alert('{0}');", messageMsg);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", scriptMsg, true);
    }

}
