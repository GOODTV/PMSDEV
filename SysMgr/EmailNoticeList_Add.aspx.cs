using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class SysMgr_EmailNoticeList_Add : BasePage 
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            LoadDropDownListData();
        }

    }

    private void LoadDropDownListData()
    {
        //通知類別
        string strSql = " select distinct [EMailType] from [EMailAddressList] order by [EMailType] ";
        Util.FillDropDownList(ddlEMailType, strSql, "EMailType", "EMailType", true);
    }

    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string strSql = " insert into  [EMailAddressList] "
             + " ( [ProgramName], [EMailType], [Employee_no], [EMailName], [EMailAddress], [Enable]) "
             + " values (@ProgramName, @EMailType, @Employee_no, @EMailName, @EMailAddress, @IsUse) ; "
             + " select @@IDENTITY ; ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("EMailName", txtEMailName.Text.Trim());
        dict.Add("EMailAddress", txtEMailAddress.Text.Trim());
        dict.Add("ProgramName", txtProgramName.Text.Trim());
        dict.Add("Employee_no", txtEmployee_no.Text.Trim());
        //dict.Add("EMailType", txtEMailType.Text.Trim());
        dict.Add("EMailType", ddlEMailType.SelectedValue);
        dict.Add("IsUse", rdoIsUse.SelectedIndex == 0 ? 1 : 0);

        HFD_Uid.Value = NpoDB.GetScalarS(strSql, dict);
        ShowSysMsg("新增資料成功!");
        Response.Redirect(Util.RedirectByTime("EmailNoticeList.aspx"));

    }

    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmailNoticeList.aspx?rand=" + DateTime.Now.Ticks.ToString());
    }
    //----------------------------------------------------------------------

}
