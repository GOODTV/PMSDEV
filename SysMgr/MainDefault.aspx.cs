using System;
using System.Web.UI;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class SysMgr_MainDefault : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        string JavascriptCode = "$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();";
        JavascriptCode += "$('#btnPreviousPage2').hide();$('#btnNextPage2').hide();$('#btnGoPage2').hide();";
        JavascriptCode += "$('#btnPreviousPage3').hide();$('#btnNextPage3').hide();$('#btnGoPage3').hide();";
        JavascriptCode += "$('#btnPreviousPage4').hide();$('#btnNextPage4').hide();$('#btnGoPage4').hide();";
        JavascriptCode += "$('#btnPreviousPage5').hide();$('#btnNextPage5').hide();$('#btnGoPage5').hide();";
        Util.ShowSysMsgWithScript(JavascriptCode);

        if (!IsPostBack)
        {
            //公告訊息
            lblNews.Text = GetNewData();
        }
        */
    }
    //------------------------------------------------------------------------------
    public string GetNewData()
    {
        string strSql = "";
        DataTable dt;
        string SysDate =  Util.DateTime2String(DateTime.Now, DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        strSql = "select News.* from news\n";
        strSql += "where @SysDate between NewsBeginDate and NewsEndDate\n";
        strSql += "and DeptName like @DeptName and IsNull(IsDelete,'') <> '1'\n";
        strSql += "order by NewsRegDate Desc";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("SysDate", SysDate);
        dict.Add("DeptName", "%" + SessionInfo.DeptName + "%");
        dt = NpoDB.GetDataTableS(strSql, dict);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<table width='98%' id='AutoNumber1' class='table_v' >");
        sb.AppendLine("<tr>");
        sb.AppendLine("<td width='100%' colspan='2'> </td>");
        sb.AppendLine("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine(" <td width='4%' align='right'><img border='0' src='../images/DIR_tri.gif' /></td>");
            sb.AppendLine(@" <td width='96%' style='color:#660000' align='left'>");
            //sb.AppendLine("<a href='../filemgr/News_Show.aspx?NewsUID=" + dr["uid"].ToString() + "' class='news'>" + dr["NewsSubject"].ToString() + "</a> ");
            sb.AppendLine("<a href='../filemgr/News_Show.aspx?NewsUID=" + dr["uid"].ToString() + "' class='news'>" +"【"+ dr["NewsType"].ToString() +"】"+ dr["NewsSubject"].ToString() + "</a> ");
            //sb.AppendLine("(" + dr["NewsRegDate"] + ")");
            sb.AppendLine("(" + Convert.ToDateTime(dr["NewsBeginDate"].ToString()).ToString("yyyy/MM/dd") + "～" + Convert.ToDateTime(dr["NewsEndDate"].ToString()).ToString("yyyy/MM/dd") + ")");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td width='4%' height='5'></td>");
            sb.AppendLine("<td width='96%' style='color:#4B4B4B' height='5'  align='left'> ");
            sb.AppendLine(dr["NewsBrief"].ToString());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");
        return sb.ToString();
    }
}
