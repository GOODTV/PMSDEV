using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShortFilm_ShortFilmCategoryQuery : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFormData();
        }
    }
    public void LoadFormData()
    {
        string strSql;
        DataTable dt;
        strSql = @"select Category ,Category as 短片類別, Description as 短片類別說明 ,CONVERT(VarChar,[SetDate],111) as 設定日期 ,
                    Case when RequestMultipleNumber ='Y' then '多號' else '單號' end as 可申請多號 ,
                    Case when ProgramExist = 'Y' then '必填' else '非必填' end as 必須有節目 ,Case when EpisodeExist = 'Y' then '必填' else '非必填' end as 必須有集數 
                    from [dbo].[_CF01P0] ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        strSql += " order by Category ";
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            lblGridList.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            //Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("Category");
            npoGridView.DisableColumn.Add("Category");
            //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = false; //不換頁
            npoGridView.EditLink = Util.RedirectByTime("ShortFilmCategoryEdit.aspx", "Category=");
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;
        }

        Session["strSql"] = strSql;
    }
    //新增
    public void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ShortFilmCategoryAdd.aspx"));
    }
}