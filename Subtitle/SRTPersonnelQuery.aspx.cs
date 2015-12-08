using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_SRTPersonnelQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EditorName"] != null)
            {
                tbxSRTName.Text = Session["EditorName"].ToString();
            }
            if (tbxSRTName.Text.Trim() != "")
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
        strSql = @"select M.EditorID , M.EditorID as 字幕人員代號, EditorName as 字幕人員姓名
                    , Case when D.EditorDept = '300,' then '節目部' when D.EditorDept ='400,' then '國外部' when D.EditorDept ='300,400,' then'節目部,國外部' else '' end as 字幕人員部門 
                    , LandPhone as 連絡電話 , MobilePhone as 手機號碼 
                    , Address as 戶籍地址 , Email_1 as 電子郵件一, Email_2 as 電子郵件二, Comparison as 評比
                    from [dbo].[_ST01P0] M
                    left join (select EditorID,(select EditorDept+',' from [dbo].[_ST01M1] A where A.EditorID=B.EditorID For XML PATH('')) as EditorDept from [_ST01M1] B group by B.EditorID) D 
                    on M.EditorID=D.EditorID
                    where 1=1 ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxSRTName.Text.Trim() != "")
        {
            strSql += " and EditorName like N'%" + tbxSRTName.Text.Trim() + "%'";
        }
        Session["EditorName"] = tbxSRTName.Text.Trim();
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            lblGridList.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            //2015/01/21 換掉原先的 Html Table
            string strBody = "";// "<table class='table-list' width='100%'>";
            /*strBody += @"<TR><TH noWrap width='80px'><SPAN>字幕人員代號</SPAN></TH>
                            <TH noWrap width='140px'><SPAN>字幕人員姓名</SPAN></TH>
                            <TH noWrap width='80px'><SPAN>字幕人員部門</SPAN></TH>
                            <TH noWrap width='140px'><SPAN>連絡電話</SPAN></TH>
                            <TH noWrap width='100px'><SPAN>手機號碼</SPAN></TH>
                            <TH noWrap width='350px'><SPAN>戶籍地址</SPAN></TH>
                            <TH noWrap width='200px'><SPAN>電子郵件一</SPAN></TH>
                            <TH noWrap width='170px'><SPAN>電子郵件二</SPAN></TH>
                            <TH noWrap width='30px'><SPAN>評比</SPAN></TH>
                        </TR>";*/
            foreach (DataRow dr in dt.Rows)
            {

                strBody += "<TR align='left' onclick =\"window.event.cancelBubble=true;window.open(\'" + Util.RedirectByTime("SRTPersonnelEdit.aspx", "EditorID=" + dr["EditorID"].ToString()) + "\',\'_self\',\'\')\">";
                strBody += "<TD noWrap><SPAN>" + dr["字幕人員代號"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["字幕人員姓名"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["字幕人員部門"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["連絡電話"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["手機號碼"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["戶籍地址"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["電子郵件一"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["電子郵件二"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap align='center'><SPAN>" + dr["評比"].ToString() + "</SPAN></TD></TR>";

            }

            strBody += "</table>";
            lblGridList.Text = strBody;
            /*//Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("EditorID");
            npoGridView.DisableColumn.Add("EditorID");
            //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = false; //不換頁
            npoGridView.EditLink = Util.RedirectByTime("SRTPersonnelEdit.aspx", "EditorID=");
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;*/
        }

        Session["strSql"] = strSql;
    }
    //新增
    public void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("SRTPersonnelAdd.aspx"));
    }
}