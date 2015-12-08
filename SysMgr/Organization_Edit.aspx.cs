using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class SysMgr_Organization_Edit :BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "MENU_SYSTEM_ORGANIZATION";
        //權控處理
        AuthrityControl();

        txtOrgID.ReadOnly = true;

        //取得模式, 新增("ADD")或修改("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //設定一些按鍵的 Visible 屬性
        //注意:要在 AuthrityControl() 及取得 HFD_Mode.Value 呼叫
        SetButton();

        HFD_OrgUID.Value = Util.GetQueryString("OrgUID");
        
        ShowSysMsg();
        if (!IsPostBack)
        {
            txtOrgID.Text = Util.GetQueryString("OrgID");
            LoadDropDownListData();
            LoadFormData();
        }
    }
    //------------------------------------------------------------------------------
    private void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        Authrity.CheckButtonRight("_Update", btnUpdate);
        Authrity.CheckButtonRight("_Delete", btnDelete);
        //Authrity.CheckButtonRight("_Print", btnExit);
    }
    //------------------------------------------------------------------------------
    private void SetButton()
    {
        //如果是新增,要做一些初始畫的動作
        if (HFD_Mode.Value == "ADD")
        {
            //新增時, 修改及刪除鍵沒動作
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            //btnPrint.Visible = false;
            txtOrgID.ReadOnly = false; 
        }
        else
        {
            //修改時, 新增鍵沒動作
            btnAdd.Visible = false;
            txtOrgID.CssClass = "readonly";
        }
    }
    //-----------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //郵件寄送方式
        string strSql = "Select CaseName as Mail_SendType From CASECODE Where GroupName ='郵件寄送方式' Order By CaseID";
        Util.FillDropDownList(ddlMail_SendType,strSql,"Mail_SendType","Mail_SendType",false);
        ddlMail_SendType.Items.Insert(0, new ListItem("", ""));
        ddlMail_SendType.SelectedIndex = 0;

        //縣市
        Util.FillDropDownList(ddlCity, Util.GetDataTable("CodeCity", "ParentCityID", "0", "", ""), "Name", "ZipCode", false);
        ddlCity.Items.Insert(0, new ListItem("縣 市", "縣 市"));
        ddlCity.SelectedIndex = 0;

        //鄉鎮市區
        //if (ddlArea.Items.FindByText("") == null)
        ddlArea.Items.Insert(0, new ListItem("鄉鎮市區", "鄉鎮市區"));
        ddlArea.SelectedIndex = 0;

        //勸募許可文號
        strSql = "Select CaseName as Licence From CASECODE Where GroupName ='勸募許可文號' Order By CaseID";
        Util.FillDropDownList(ddlRept_Licence, strSql, "Licence", "Licence", false);
        ddlRept_Licence.Items.Insert(0, new ListItem("", ""));
        ddlRept_Licence.SelectedIndex = 0;
    }
    //-------------------------------------------------------------------------
    public void LoadFormData()
    {
        string strSql = "";
        DataTable dt = null;
        DataRow dr = null;

        if (HFD_Mode.Value != "ADD")
        {
            strSql = "select * from Organization where OrgID=@OrgID";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("OrgID", HFD_OrgUID.Value);
            dt = NpoDB.GetDataTableS(strSql, dict);
            //資料異常
            if (dt.Rows.Count == 0)
            {
                SetSysMsg("查無資料!");
                Response.Redirect(Util.RedirectByTime("Organization.aspx"));
            }
            dr = dt.Rows[0];
        }
        txtOrgID.Text = (HFD_Mode.Value == "ADD") ? "" : dr["OrgID"].ToString();
        txtOrgName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["OrgName"].ToString();
        txtOrg_Slogan.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Org_Slogan"].ToString();
        txtOrgShortName.Text = (HFD_Mode.Value == "ADD") ? "" : dr["OrgShortName"].ToString();
        txtIP.Text = (HFD_Mode.Value == "ADD") ? "" : dr["IP"].ToString();
        txtSys_Name.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Sys_Name"].ToString();
        txtMail_Url.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Mail_Url"].ToString();
        ddlMail_SendType.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Mail_SendType"].ToString();
        txtContactor.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Contactor"].ToString();
        txtTel.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Tel"].ToString();
        txtFax.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Fax"].ToString();
        txtEmail.Text = (HFD_Mode.Value == "ADD") ? "" : dr["email"].ToString();
        //區碼
        tbxZipCode.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ZipCode"].ToString().Trim();
        //縣市
        if (HFD_Mode.Value != "ADD" && dr["City"].ToString().Trim() != "")
        {
            ddlCity.Items.FindByText("縣 市").Selected = false;
            ddlCity.Items.FindByValue(dr["City"].ToString().Trim()).Selected = true;
        }
        // 鄉鎮市區
        CityToArea(ddlArea, ddlCity);
        //區域
        if (HFD_Mode.Value != "ADD" && dr["Area"].ToString().Trim() != "")
        {
            ddlArea.Items.FindByText("鄉鎮市區").Selected = false;
            ddlArea.Items.FindByValue(dr["Area"].ToString().Trim()).Selected = true;
        }
        txtAddress.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Address"].ToString();
        txtUniform_No.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Uniform_No"].ToString();
        txtAccount.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Account"].ToString();
        txtCreateDate.Text = (HFD_Mode.Value == "ADD") ? "" : Util.DateTime2String(dr["CreateDate"], DateType.yyyyMMdd, EmptyType.ReturnEmpty);
        txtLicence.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Licence"].ToString();
        txtPasswordDay.Text = (HFD_Mode.Value == "ADD") ? "" : dr["PasswordDay"].ToString();
        ddlRept_Licence.Text = (HFD_Mode.Value == "ADD") ? "" : dr["Rept_Licence"].ToString();
    }
    //-------------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateInsertCommand("Organization", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "新增資料成功";
        Response.Redirect(Util.RedirectByTime("Organization.aspx"));
    }
    //------------------------------------------------------------------------------
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        SetColumnData(list);
        string strSql = Util.CreateUpdateCommand("Organization", list, dict);
        NpoDB.ExecuteSQLS(strSql, dict);
        Session["Msg"] = "修改資料成功";
        Response.Redirect(Util.RedirectByTime("Organization.aspx"));
    }
    //-------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        if (HFD_Mode.Value == "ADD") //新增時要以 TextBox 的資料為來源
        {
            list.Add(new ColumnData("OrgID", txtOrgID.Text, true, false, true));
        }
        else
        {
            list.Add(new ColumnData("OrgID", HFD_OrgUID.Value, true, false, true));
        }
        list.Add(new ColumnData("OrgName", txtOrgName.Text, true, true, false));
        list.Add(new ColumnData("Org_Slogan", txtOrg_Slogan.Text, true, true, false));
        list.Add(new ColumnData("OrgShortName", txtOrgShortName.Text, true, true, false));
        list.Add(new ColumnData("IP", txtIP.Text, true, true, false));
        list.Add(new ColumnData("Sys_Name", txtSys_Name.Text, true, true, false));
        list.Add(new ColumnData("Mail_Url", txtMail_Url.Text, true, true, false));
        list.Add(new ColumnData("Mail_SendType", ddlMail_SendType.SelectedItem.Text, true, true, false));
        list.Add(new ColumnData("Contactor", txtContactor.Text, true, true, false));
        list.Add(new ColumnData("Tel", txtTel.Text, true, true, false));
        list.Add(new ColumnData("Fax", txtFax.Text, true, true, false));
        list.Add(new ColumnData("Email", txtEmail.Text, true, true, false));
        list.Add(new ColumnData("ZipCode", tbxZipCode.Text, true, true, false));
        list.Add(new ColumnData("City", ddlCity.SelectedValue, true, true, false));
        list.Add(new ColumnData("Area", ddlArea.SelectedValue, true, true, false));
        list.Add(new ColumnData("Address", txtAddress.Text, true, true, false));
        list.Add(new ColumnData("Uniform_No", txtUniform_No.Text, true, true, false));
        list.Add(new ColumnData("Account", txtAccount.Text, true, true, false));
        list.Add(new ColumnData("CreateDate", Util.DateTime2String(txtCreateDate.Text, DateType.yyyyMMdd, EmptyType.ReturnEmpty), true, true, false));
        list.Add(new ColumnData("Licence", txtLicence.Text, true, true, false));
        list.Add(new ColumnData("PasswordDay", txtPasswordDay.Text, true, true, false));
        list.Add(new ColumnData("Rept_Licence", ddlRept_Licence.SelectedItem.Text, true, true, false));
    }
    //-------------------------------------------------------------------------
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string strSql;

        strSql = " select DeptDesc from Dept where OrgID=@OrgID";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("OrgID", txtOrgID.Text);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        //有部門使用此機構代碼，導回機構管理頁面
        if (dt.Rows.Count > 0)
        {
            Session["Msg"] = "" + dt.Rows[0]["DeptDesc"].ToString() + "使用此機構代碼，無法刪除!!";
            Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
            return;
        }

        strSql = " delete Organization Where OrgID=@OrgID";
        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "刪除資料成功!!";
        Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
    }
    //-------------------------------------------------------------------------
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
    }
    //-------------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
    }
    public void CityToArea(DropDownList ddlTO, DropDownList ddlFrom)
    {
        Util.FillDropDownList(ddlTO, Util.GetDataTable("CodeCity", "ParentCityID", ddlFrom.SelectedValue, "", ""), "Name", "ZipCode", false);
        ddlTO.Items.Insert(0, new ListItem("鄉鎮市區", "鄉鎮市區"));
        ddlTO.SelectedIndex = 0;
    }
    //------------------------------------------------------------------------------
    //dropdownlist選縣市自動帶入地區和電話區碼
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityToArea(ddlArea, ddlCity);
    }
    //------------------------------------------------------------------------------
    //dropdownlist選地區自動填入郵遞區號
    protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbxZipCode.Text = Util.GetCityCode(ddlArea.SelectedItem.Text, ddlCity.SelectedValue);
    }
}
