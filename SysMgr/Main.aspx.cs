using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SysMgr_Main : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            lblRemoteAddr.Text = Request.ServerVariables["REMOTE_ADDR"];
            if (lblRemoteAddr.Text == "::1")
            {
                lblRemoteAddr.Text = "127.0.0.1";
            }

            DataTable dt = GetTopMenu(); //先選擇第一層功能表
            string menuStr = CreateMenuList(dt); //建立 1,2 層的 menu
            lblMenuContainer.Text = menuStr;
        }

    }

    //---------------------------------------------------------------------------
    //取得選單群組資料
    public DataTable GetTopMenu()
    {
        //string DeptID = SessionInfo.DeptID;
        //string UserID = SessionInfo.UserID;

        string strSql = "select m.MenuID, m.ParentID, m.MenuName \n";
        strSql += "FROM AdminMenu m \n";
        strSql += "left join AdminRight r on m.MenuID=r.MenuID \n";
        strSql += "where m.IsUse=1 and m.IsMenu=1 and m.ParentID=0 \n";
        strSql += "and r._focus=1 and r.GroupID=@GroupID \n";
        strSql += "order by m.sort \n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("DeptID", SessionInfo.DeptID);
        dict.Add("GroupID", SessionInfo.GroupID);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //---------------------------------------------------------------------------
    //製作選單
    public string CreateMenuList(DataTable dt)
    {
        int count = dt.Rows.Count;
        StringBuilder MenuListSb = new StringBuilder();
        DataRow dr;

        //MenuListSb.AppendLine("<div id='menu'>");
        MenuListSb.AppendLine("<ul id='menu' class='menu'>");
        MenuListSb.AppendLine("<li><a id='home' href='#' onclick=\"window.open('/SysMgr/MainDefault.aspx','content'); \">首頁</a></li>");
        for (int i = 1; i <= count; i++)
        {
            dr = dt.Rows[i - 1];
            MenuListSb.AppendLine(CreateMenu(dr["MenuID"].ToString(), dr["ParentID"].ToString(), dr["MenuName"].ToString(), i));
        }
        MenuListSb.AppendLine("<li><a href=\"JavaScript:if(confirm('是否確定要登出 ?')){window.location.href='/Default.aspx?logout=true&UserID=" + SessionInfo.UserID + "';} \">登出</a></li>");

        MenuListSb.AppendLine("</ul>");
        //MenuListSb.AppendLine("</div>");
        return MenuListSb.ToString();
    }
    //---------------------------------------------------------------------------
    //製作子選單
    public string CreateMenu(string MenuID, string ParentID, string MenuName, int i)
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder htmlSb = new StringBuilder();
        //string DeptID = SessionInfo.DeptID;
        //string UserID = SessionInfo.UserID;

        string strSql = "select m.* FROM AdminMenu m\n";
        strSql += "left join AdminRight r on m.MenuID=r.MenuID\n";
        strSql += "where m.IsUse=1 and m.IsMenu=1 and m.ParentID=@MenuID\n";
        strSql += "and r._focus=1 and r.GroupID=@GroupID\n";
        strSql += "order by m.sort\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("UserID", SessionInfo.UserID);
        dict.Add("DeptID", SessionInfo.DeptID);
        dict.Add("MenuID", MenuID);
        dict.Add("GroupID", SessionInfo.GroupID);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        //int count = 0;
        //count = dt.Rows.Count;
        int j = 0;
        DataRow dr;
        int dataCount = dt.Rows.Count;

        //第一層選單名稱
        if (dataCount > 0)
        {
            htmlSb.AppendLine("<li class='menu-item'><a href='#'>" + MenuName + "<div class=\"arrow-bottom\"></div></a>");
            htmlSb.AppendLine("<ul class='sub-menu'>");
        }
        else
        {
            htmlSb.AppendLine("<li class='menu-item'><a href='#'>" + MenuName + "</a>");
        }

        //第二層選單名稱
        for (j = 0; j < dataCount; j++)
        {
            dr = dt.Rows[j];
            //string Extension = System.IO.Path.GetExtension(dr["ProgramURL"].ToString()).ToUpper();
            htmlSb.AppendLine("<li style='z-index: 1;'>");
            //if (Extension == ".ASPX")
            //{
                if (dr["ProgramURL"].ToString().IndexOf("?") == -1)
                {
                    htmlSb.AppendLine("<a href=\"#\" onclick=\"window.open('/" + dr["ProgramURL"].ToString() + "?ProgID=" + dr["ProgramID"].ToString() + "','content'); \">");
                    //htmlSb.AppendLine("<a href=\"/" + dr["ProgramURL"].ToString() + "?ProgID=" + dr["ProgramID"].ToString() + "\" target='content'>");
                    //htmlSb.AppendLine("<a href=\"#\" onclick=\"javascript:location.replace='/" + dr["ProgramURL"].ToString() + "'; \">");
                }
                else
                {
                    htmlSb.AppendLine("<a href=\"#\" onclick=\"window.open('/" + dr["ProgramURL"].ToString() + "&ProgID=" + dr["ProgramID"].ToString() + "','content'); \">");
                }
            //}

            htmlSb.AppendLine(dr["MenuName"].ToString());

            //if (Extension == ".ASPX")
            //{
                //htmlSb.AppendLine("</a><li>");
                htmlSb.AppendLine("</a>");
            //}
            htmlSb.AppendLine("</li>");
        }
        if (dataCount > 0)
        {
            htmlSb.AppendLine("</ul>");
        }
        htmlSb.AppendLine("</li>");

        return htmlSb.ToString();
    }
    //---------------------------------------------------------------------------

}
