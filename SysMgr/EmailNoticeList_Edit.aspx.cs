using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SysMgr_EmailNoticeList_Edit : BasePage 
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "MENU_SYSTEM_EMAIL_NOTICE";
        //權控處理
        AuthrityControl();

        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("EMailNo");
            LoadDropDownListData();
            LoadFormData();
        }
    }

    private void LoadDropDownListData()
    {
        //通知類別
        string strSql = " select distinct [EMailType] from [EMailAddressList] order by [EMailType] ";
        Util.FillDropDownList(ddlEMailType, strSql, "EMailType", "EMailType", false);
    }

    //----------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_Delete", btnDelete);
        Authrity.CheckButtonRight("_Update", btnUpdate);
    }

    //----------------------------------------------------------------------
    public void LoadFormData()
    {
        string strSql = " select * from [EMailAddressList] where [EMailNo] = @EMailNo";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("EMailNo", HFD_Uid.Value);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        //資料異常
        if (dt.Rows.Count <= 0)
        {
            Response.Redirect("Default.aspx");
        }

        DataRow dr = dt.Rows[0];

        //通知人
        txtEMailName.Text = dr["EMailName"].ToString();

        //通知人Email
        txtEMailAddress.Text = dr["EMailAddress"].ToString();

        //所屬程式名稱
        txtProgramName.Text = dr["ProgramName"].ToString();

        //同工編號
        txtEmployee_no.Text = dr["Employee_no"].ToString();

        //通知類別
        //txtEMailType.Text = dr["EMailType"].ToString();
        ddlEMailType.SelectedValue = dr["EMailType"].ToString();

        //是否使用
        if (dr["Enable"].ToString() == "True")
        {
            rdoIsUse.SelectedIndex = 0;
        }
        else
        {
            rdoIsUse.SelectedIndex = 1;
        }

    }

    //----------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string strSql = " update [EMailAddressList] set ";
        strSql += "   [EMailName] = @EMailName ";
        strSql += " , [EMailAddress] = @EMailAddress ";
        strSql += " , [ProgramName] = @ProgramName ";
        strSql += " , [Employee_no] = @Employee_no ";
        strSql += " , [EMailType] = @EMailType ";
        strSql += " , [Enable] = @IsUse ";
        strSql += "   where [EMailNo] = @EMailNo ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("EMailNo", HFD_Uid.Value);
        dict.Add("EMailName", txtEMailName.Text.Trim());
        dict.Add("EMailAddress", txtEMailAddress.Text.Trim());
        dict.Add("ProgramName", txtProgramName.Text.Trim());
        dict.Add("Employee_no", txtEmployee_no.Text.Trim());
        //dict.Add("EMailType", txtEMailType.Text.Trim());
        dict.Add("EMailType", ddlEMailType.SelectedValue);
        dict.Add("IsUse", rdoIsUse.SelectedIndex == 0 ? 1 : 0);

        if (NpoDB.ExecuteSQLS(strSql, dict) == 0)
        {
            ShowSysMsg("修改資料失敗!");
            return;
        }
        SetSysMsg("修改資料成功!");
        Response.Redirect("EmailNoticeList.aspx");
    }

    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmailNoticeList.aspx?rand=" + DateTime.Now.Ticks.ToString());
    }

    //----------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        bool flag = false;
        objNpoDB.BeginTrans();
        try
        {
            Delete();
            SetSysMsg("刪除資料成功");
            objNpoDB.CommitTrans();
            flag = true;
        }
        catch (Exception ex)
        {
            objNpoDB.RollbackTrans();
            throw ex;
        }
        if (flag == true)
        {
            Response.Redirect("EmailNoticeList.aspx");
        }
    }

    //----------------------------------------------------------------------
    private void Delete()
    {
        string strSql = "delete from  [EMailAddressList] where [EMailNo] = @EMailNo";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("EMailNo", HFD_Uid.Value);
        objNpoDB.ExecuteSQL(strSql, dict);

    }
    //----------------------------------------------------------------------

}
