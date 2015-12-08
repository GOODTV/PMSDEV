using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class View_ShortFilmShow : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    //---------------------------------------------------------------------------
    public void LoadFormData()
    {
        string strSql;
        DataTable dt;
        strSql = "select [CFID] as '短片代號' ,[CFTitle] as '短片名稱' from [dbo].[_CF02P0] where 1=1 ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        if (txtFilmID.Text.Trim() != "")
        {
            strSql += " and [CFID] like N'%" + txtFilmID.Text.Trim() + "%'";
        }
        if (txtFilmName.Text.Trim() != "")
        {
            strSql += " and [CFTitle] like N'%" + txtFilmName.Text.Trim() + "%'";
        }
        strSql += " Order By [CFID] ";

        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
        }
        else
        {
            //Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.ShowPage = false;
            npoGridView.Keys.Add("短片代號");
            npoGridView.Keys.Add("短片名稱");

            //在前端設置JS語法並傳入KeyValue並執行
            npoGridView.EditClick = "ReturnOpener";
            lblGridList.Text = npoGridView.Render();
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
}