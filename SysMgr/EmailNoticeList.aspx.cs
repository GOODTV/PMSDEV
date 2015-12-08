using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;

public partial class SysMgr_EmailNoticeList : BasePage
{
    //NpoGridView 處理換頁相關程式碼
    Button btnNextPage, btnPreviousPage, btnGoPage;
    HiddenField HFD_CurrentPage;

    override protected void OnInit(EventArgs e)
    {
        CreatePageControl();
        base.OnInit(e);
    }

    private void CreatePageControl()
    {
        // Create dynamic controls here.
        btnNextPage = new Button();
        btnNextPage.ID = "btnNextPage";
        Form1.Controls.Add(btnNextPage);
        btnNextPage.Click += new System.EventHandler(btnNextPage_Click);

        btnPreviousPage = new Button();
        btnPreviousPage.ID = "btnPreviousPage";
        Form1.Controls.Add(btnPreviousPage);
        btnPreviousPage.Click += new System.EventHandler(btnPreviousPage_Click);

        btnGoPage = new Button();
        btnGoPage.ID = "btnGoPage";
        Form1.Controls.Add(btnGoPage);
        btnGoPage.Click += new System.EventHandler(btnGoPage_Click);

        HFD_CurrentPage = new HiddenField();
        HFD_CurrentPage.Value = "1";
        HFD_CurrentPage.ID = "HFD_CurrentPage";
        Form1.Controls.Add(HFD_CurrentPage);
    }

    protected void btnPreviousPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.MinusStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }

    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        HFD_CurrentPage.Value = Util.AddStringNumber(HFD_CurrentPage.Value);
        LoadFormData();
    }

    protected void btnGoPage_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }

    //---------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProgID"] = "MENU_SYSTEM_EMAIL_NOTICE";
        //權控處理
        AuthrityControl();

        Util.ShowSysMsgWithScript("$('#btnPreviousPage').hide();$('#btnNextPage').hide();$('#btnGoPage').hide();");

        if (!IsPostBack)
        {
            LoadFormData();
        }
    }

    //----------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }

        Authrity.CheckButtonRight("_AddNew", btnAdd);
        Authrity.CheckButtonRight("_Query", btnQuery);
    }

    //----------------------------------------------------------------------
    public void LoadFormData()
    {
        string strSql = "select [EMailNo],[ProgramName] as [所屬程式名稱], [EMailType] as [通知類別], [EMailName] as [通知人] "
             + " , [Employee_no] as [同工編號], [EMailAddress] as [通知人Email] "
             + " , case when [Enable]=1 then '使用' else '禁用' end as [是否使用] "
             + "   from [EMailAddressList] where 1=1 ";
        if (txtEMailName.Text != "")
        {
            strSql += "and [EMailName] like @EMailName ";
        }
        if (txtEMailAddress.Text != "")
        {
            strSql += "and [EMailAddress] like @EMailAddress ";
        }
        strSql += "order by [ProgramName],[EMailNo] ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("EMailName", "%" + txtEMailName.Text.Trim() + "%");
        dict.Add("EMailAddress", "%" + txtEMailAddress.Text.Trim() + "%");
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = dt;
        npoGridView.Keys.Add("EMailNo");
        npoGridView.DisableColumn.Add("EMailNo");
        npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
        npoGridView.EditLink = Util.RedirectByTime("EmailNoticeList_Edit.aspx", "EMailNo=");
        lblGridList.Text = npoGridView.Render();
    }

    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmailNoticeList_Add.aspx");
    }

    //----------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //----------------------------------------------------------------------
}
