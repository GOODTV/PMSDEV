using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_SRTCostQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownListData();
            if (Session["CostEffectiveDate"] != null)
            {
                tbxCostEffectiveDate.Text = Session["CostEffectiveDate"].ToString();
            }
            if (Session["Classification"] != null)
            {
                ddlMake_Category.SelectedValue = Session["Classification"].ToString();
            }
            if (tbxCostEffectiveDate.Text.Trim() != "" || ddlMake_Category.SelectedValue !="")
            {
                LoadFormData();
            }
        }
    }
    public void LoadDropDownListData()
    {
        Util.FillDropDownList(ddlMake_Category, Util.GetDataTable("Classification", "GroupName", "製作類別", "", ""), "ClassificationName", "ClassificationCode", false);
        ddlMake_Category.Items.Insert(0, new ListItem("", ""));
        ddlMake_Category.SelectedIndex = 0;
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    public void LoadFormData()
    {
        string strSql;
        DataTable dt;
        strSql = @"SELECT Ser_No,CONVERT(VarChar,[CostEffectiveDate],111) as '成本生效日期'
                      ,C.ClassificationName as '製作類別'
                      ,Length as '標準長度'
                      ,Amount as '標準金額'
                      ,Description as '說明'
                      ,Case when Enable = '1' then 'V' else '' end as '啟用'
                  FROM [dbo].[_ST02P0] M
                  left join Classification C on M.Classification=C.ClassificationCode
                  where 1=1 ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxCostEffectiveDate.Text.Trim() != "")
        {
            strSql += " and CostEffectiveDate ='" + tbxCostEffectiveDate.Text.Trim() + "'";
        }
        if (ddlMake_Category.SelectedIndex != 0)
        {
            strSql += " And Classification = '" + ddlMake_Category.SelectedItem.Value + "' ";
        }
        Session["CostEffectiveDate"] = tbxCostEffectiveDate.Text.Trim();
        Session["Classification"] = ddlMake_Category.SelectedItem.Value;
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
            /*strBody += @"<TR><TH noWrap><SPAN>成本生效日期</SPAN></TH>
                            <TH noWrap><SPAN>製作類別</SPAN></TH>
                            <TH noWrap><SPAN>標準長度</SPAN></TH>
                            <TH noWrap><SPAN>標準金額</SPAN></TH>
                            <TH noWrap><SPAN>說明</SPAN></TH>
                            <TH noWrap><SPAN>啟用</SPAN></TH>
                        </TR>";*/
            foreach (DataRow dr in dt.Rows)
            {

                strBody += "<TR align='left' onclick =\"window.event.cancelBubble=true;window.open(\'" + Util.RedirectByTime("SRTCostEdit.aspx", "Ser_No=" + dr["Ser_No"].ToString()) + "\',\'_self\',\'\')\"><TD noWrap><SPAN>" + dr["成本生效日期"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["製作類別"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["標準長度"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["標準金額"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap><SPAN>" + dr["說明"].ToString() + "</SPAN></TD>";
                strBody += "<TD noWrap align='center'><SPAN>" + dr["啟用"].ToString() + "</SPAN></TD></TR>";

            }

            strBody += "</table>";
            lblGridList.Text = strBody;
            //Grid initial
            /*NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("Ser_No");
            npoGridView.DisableColumn.Add("Ser_No");
            //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = false; //不換頁
            npoGridView.EditLink = Util.RedirectByTime("SRTCostEdit.aspx", "Ser_No=");
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;*/
        }

        Session["strSql"] = strSql;
    }
    public void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("SRTCostAdd.aspx"));
    }
}