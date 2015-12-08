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
        //�v���B�z
        AuthrityControl();

        txtOrgID.ReadOnly = true;

        //���o�Ҧ�, �s�W("ADD")�έק�("")
        HFD_Mode.Value = Util.GetQueryString("Mode");

        //�]�w�@�ǫ��䪺 Visible �ݩ�
        //�`�N:�n�b AuthrityControl() �Ψ��o HFD_Mode.Value �I�s
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
        //�p�G�O�s�W,�n���@�Ǫ�l�e���ʧ@
        if (HFD_Mode.Value == "ADD")
        {
            //�s�W��, �ק�ΧR����S�ʧ@
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            //btnPrint.Visible = false;
            txtOrgID.ReadOnly = false; 
        }
        else
        {
            //�ק��, �s�W��S�ʧ@
            btnAdd.Visible = false;
            txtOrgID.CssClass = "readonly";
        }
    }
    //-----------------------------------------------------------------------------
    public void LoadDropDownListData()
    {
        //�l��H�e�覡
        string strSql = "Select CaseName as Mail_SendType From CASECODE Where GroupName ='�l��H�e�覡' Order By CaseID";
        Util.FillDropDownList(ddlMail_SendType,strSql,"Mail_SendType","Mail_SendType",false);
        ddlMail_SendType.Items.Insert(0, new ListItem("", ""));
        ddlMail_SendType.SelectedIndex = 0;

        //����
        Util.FillDropDownList(ddlCity, Util.GetDataTable("CodeCity", "ParentCityID", "0", "", ""), "Name", "ZipCode", false);
        ddlCity.Items.Insert(0, new ListItem("�� ��", "�� ��"));
        ddlCity.SelectedIndex = 0;

        //�m����
        //if (ddlArea.Items.FindByText("") == null)
        ddlArea.Items.Insert(0, new ListItem("�m����", "�m����"));
        ddlArea.SelectedIndex = 0;

        //�U�ҳ\�i�帹
        strSql = "Select CaseName as Licence From CASECODE Where GroupName ='�U�ҳ\�i�帹' Order By CaseID";
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
            //��Ʋ��`
            if (dt.Rows.Count == 0)
            {
                SetSysMsg("�d�L���!");
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
        //�ϽX
        tbxZipCode.Text = (HFD_Mode.Value == "ADD") ? "" : dr["ZipCode"].ToString().Trim();
        //����
        if (HFD_Mode.Value != "ADD" && dr["City"].ToString().Trim() != "")
        {
            ddlCity.Items.FindByText("�� ��").Selected = false;
            ddlCity.Items.FindByValue(dr["City"].ToString().Trim()).Selected = true;
        }
        // �m����
        CityToArea(ddlArea, ddlCity);
        //�ϰ�
        if (HFD_Mode.Value != "ADD" && dr["Area"].ToString().Trim() != "")
        {
            ddlArea.Items.FindByText("�m����").Selected = false;
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
        Session["Msg"] = "�s�W��Ʀ��\";
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
        Session["Msg"] = "�ק��Ʀ��\";
        Response.Redirect(Util.RedirectByTime("Organization.aspx"));
    }
    //-------------------------------------------------------------------------
    private void SetColumnData(List<ColumnData> list)
    {
        if (HFD_Mode.Value == "ADD") //�s�W�ɭn�H TextBox ����Ƭ��ӷ�
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
        //�������ϥΦ����c�N�X�A�ɦ^���c�޲z����
        if (dt.Rows.Count > 0)
        {
            Session["Msg"] = "" + dt.Rows[0]["DeptDesc"].ToString() + "�ϥΦ����c�N�X�A�L�k�R��!!";
            Response.Redirect("Organization.aspx?rand=" + DateTime.Now.Ticks.ToString() + "");
            return;
        }

        strSql = " delete Organization Where OrgID=@OrgID";
        NpoDB.ExecuteSQLS(strSql, dict);

        Session["Msg"] = "�R����Ʀ��\!!";
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
        ddlTO.Items.Insert(0, new ListItem("�m����", "�m����"));
        ddlTO.SelectedIndex = 0;
    }
    //------------------------------------------------------------------------------
    //dropdownlist�￤���۰ʱa�J�a�ϩM�q�ܰϽX
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityToArea(ddlArea, ddlCity);
    }
    //------------------------------------------------------------------------------
    //dropdownlist��a�Ϧ۰ʶ�J�l���ϸ�
    protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbxZipCode.Text = Util.GetCityCode(ddlArea.SelectedItem.Text, ddlCity.SelectedValue);
    }
}
