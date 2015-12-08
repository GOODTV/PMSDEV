using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_SRTCostAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownListData();
        }
    }
    public void LoadDropDownListData()
    {
        Util.FillDropDownList(ddlMake_Category, Util.GetDataTable("Classification", "GroupName", "製作類別", "", ""), "ClassificationName", "ClassificationCode", false);
        ddlMake_Category.SelectedIndex = 0;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        try
        {
            SRTCost_AddNew();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (flag == true)
        {
            //Page.Controls.Add(new LiteralControl("<script>alert(\"字幕成本新增成功！\");</script>"));
            //this.Page.RegisterStartupScript("s", "<script>alert('字幕成本新增成功！');</script>");
            AjaxShowMessage("字幕成本新增成功！");
            // 新增後導向頁面
            Response.Redirect(Util.RedirectByTime("SRTCostAdd.aspx"));

        }
    }
    public void SRTCost_AddNew()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string strSql = @"INSERT INTO [dbo].[_ST02P0]
                           ([CostEffectiveDate]
                           ,[Classification]
                           ,[Length]
                           ,[Amount]
                           ,[Description]
                           ,[CreatedUser]
                           ,[CreatedDatetime]
                           ,[Enable])
                            VALUES (@CostEffectiveDate,@Classification,@Length,@Amount,@Description
                            ,@CreatedUser,@CreatedDatetime,@Enable)";
        dict.Add("CostEffectiveDate", tbxCostEffectiveDate.Text);
        dict.Add("Classification", ddlMake_Category.SelectedValue);
        dict.Add("Length", tbxLength.Text.Trim());
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
        dict.Add("CreatedUser", SessionInfo.UserID);
        dict.Add("CreatedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("SRTCostQuery.aspx"));
    }
}