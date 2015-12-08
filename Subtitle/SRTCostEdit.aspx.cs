using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_SRTCostEdit : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("Ser_No");
            LoadDropDownListData();
            Form_DataBind();
        }
    }
    public void LoadDropDownListData()
    {
        Util.FillDropDownList(ddlMake_Category, Util.GetDataTable("Classification", "GroupName", "製作類別", "", ""), "ClassificationName", "ClassificationCode", false);
        ddlMake_Category.SelectedIndex = 0;
    }
    //帶入資料
    public void Form_DataBind()
    {
        //****變數宣告****//
        string strSql, uid;
        DataTable dt, dt1;

        //****變數設定****//
        uid = HFD_Uid.Value;

        //****設定查詢****//
        strSql = @" select * From [dbo].[_ST02P0]
                    where Ser_No='" + uid + "'";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);

        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("SRTCostQuery.aspx");

        DataRow dr = dt.Rows[0];

        //字幕生效日期
        tbxCostEffectiveDate.Text = DateTime.Parse(dr["CostEffectiveDate"].ToString().Trim()).ToShortDateString().ToString();
        //製作類別
        ddlMake_Category.SelectedValue = dr["Classification"].ToString().Trim();
        //標準長度
        tbxLength.Text = dr["Length"].ToString().Trim();
        //標準金額
        tbxAmount.Text = dr["Amount"].ToString().Trim();
        //說明
        tbxDescription.Text = dr["Description"].ToString().Trim();
        //啟用
        if (dr["Enable"].ToString() == "1")
        {
            cbxEnable.Checked = true;
        }

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        bool flag = false;
        try
        {
            SRTCost_Edit();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        if (flag == true)
        {
            AjaxShowMessage("字幕成本修改成功！");
            Response.Redirect(Util.RedirectByTime("SRTCostEdit.aspx","Ser_No=" + HFD_Uid.Value));
        }
    }
    public void SRTCost_Edit()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string strSql = " update [dbo].[_ST02P0] set ";

        /*strSql += "  CostEffectiveDate = @CostEffectiveDate";
        strSql += ", Classification = @Classification";
        strSql += ", Length = @Length";*/
        strSql += " Amount = @Amount";
        strSql += ", Description = @Description";
        strSql += ", Enable = @Enable";

        strSql += ", ModifiedUser= @ModifiedUser";
        strSql += ", ModifiedDatetime = @ModifiedDatetime";
        strSql += " where Ser_No = @Ser_No;";

        /*dict.Add("CostEffectiveDate", tbxCostEffectiveDate.Text);
        dict.Add("Classification", ddlMake_Category.SelectedValue);
        dict.Add("Length", tbxLength.Text.Trim());*/
        dict.Add("Amount", tbxAmount.Text.Trim());
        dict.Add("Description", tbxDescription.Text.Trim());
        if (cbxEnable.Checked)
        {
            dict.Add("Enable", "1");
        }
        else
        {
            dict.Add("Enable", "0");
        }

        dict.Add("ModifiedUser", SessionInfo.UserID);
        dict.Add("ModifiedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("Ser_No", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

    }
    //----------------------------------------------------------------------
    protected void btnDel_Click(object sender, EventArgs e)
    {
        string strSql = "delete from [dbo].[_ST02P0] where Ser_No='" + HFD_Uid.Value + "';";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        NpoDB.ExecuteSQLS(strSql, dict);

        AjaxShowMessage("字幕成本刪除成功！");
        Response.Redirect(Util.RedirectByTime("SRTCostQuery.aspx"));
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("SRTCostQuery.aspx"));
    }
}