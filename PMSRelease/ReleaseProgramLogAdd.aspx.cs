using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ReleaseProgramLogAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Key.Value = Util.GetQueryString("Key");
            //Page.Response.Write("<Script language='JavaScript'>alert('" + HFD_Key.Value + "');</Script>");
            if (Session["programID"] != null)
            {
                txtProgramID.Text = Session["programID"].ToString();
            }
            if (Session["customerID"] != null)
            {
                txtCustomerID.Text = Session["customerID"].ToString();
            }
            if (Session["supplyDate"] != null)
            {
                txtSupplyDate.Text = Session["supplyDate"].ToString();
            }
            //節目名稱
            lblProgramName.Text = ProgramName();
            lblProgramName.Font.Size = 11;
            lblProgramName.ForeColor = System.Drawing.Color.Blue; ;
            lblProgramName.Font.Bold = true;
        }
    }
    private string ProgramName()
    {
        string strSql = @"SELECT ProgramName = CASE WHEN ISNULL(ProgramTitle,'') <> '' THEN ProgramTitle 
									WHEN ISNULL(programEnglishTitle,'') <> '' THEN programEnglishTitle
									WHEN ISNULL(ProgramAbbrev,'') <> '' THEN ProgramAbbrev ELSE NULL END
		                  FROM [pms].dbo._TM01P0
		                  WHERE ProgramID = '" + txtProgramID.Text + "'";
        string ProgramName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ProgramName = dr["ProgramName"].ToString();
        }
        return ProgramName;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        string strSql = "";
        //檢核是否有此客戶
        DataTable dt = null;
        Dictionary<string, object> dict = new Dictionary<string, object>();
        strSql = "select customerName from ReleaseMaster where customerID='" + txtCustomerID.Text.Trim() + "'";

        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count == 0)
        {
            this.Page.RegisterStartupScript("s", "<script>alert('查無此客戶！');</script>");
            return;
        }
        try
        {
            ReleaseProgramLog_AddNew();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (flag == true)
        {
            Response.Write("<Script language='JavaScript'>alert('節目交檔記錄新增成功！');</Script>");
            // 新增後導向頁面
            //Response.Redirect(Util.RedirectByTime("ReleaseProgramLogQuery.aspx"));
            Page.Response.Write("<Script language='JavaScript'>location.href=('ReleaseProgramLogAdd.aspx?Key=" + HFD_Key.Value + "');</Script>");
            Response.End();
        }
    }
    public void ReleaseProgramLog_AddNew()
    {
        string strSql = @"INSERT INTO dbo.ReleaseProgramLog
                        (programID,episode,customerID,supplyDate,filename,logo,CreateUser,CreateDatetime)
                        VALUES (@programID,@episode,@customerID,@supplyDate,@filename,@logo,@CreateUser,@CreateDatetime)";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("programID", txtProgramID.Text.Trim());
        dict.Add("episode", txtEpisode.Text.Trim());
        dict.Add("customerID", txtCustomerID.Text.Trim());
        dict.Add("supplyDate", txtSupplyDate.Text.Trim());
        dict.Add("filename", txtFilename.Text.Trim());
        if (cbxLogo.Checked == true)
        {
            dict.Add("logo", "1");
        }
        else
        {
            dict.Add("logo", "0");
        }
        dict.Add("CreateUser", SessionInfo.UserID);
        dict.Add("CreateDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        Session["programID"] = txtProgramID.Text.Trim();
        Session["customerID"] = txtCustomerID.Text.Trim();
        Session["supplyDate"] = txtSupplyDate.Text.Trim();
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Session["programID"] = null;
        Session["customerID"] = null;
        Session["supplyDate"] = null;
        Response.Redirect(Util.RedirectByTime("ReleaseProgramLogQuery.aspx", "Key=" + HFD_Key.Value));
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strSql = @"SELECT ProgramName = CASE WHEN ISNULL(ProgramTitle,'') <> '' THEN ProgramTitle 
									WHEN ISNULL(programEnglishTitle,'') <> '' THEN programEnglishTitle
									WHEN ISNULL(ProgramAbbrev,'') <> '' THEN ProgramAbbrev ELSE NULL END
		                  FROM [pms].dbo._TM01P0
		                  WHERE ProgramID = '" + txtProgramID.Text + "'";
        String ProgramName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ProgramName = dr["ProgramName"].ToString();
        }
        else
        {
            ProgramName = "查無節目名稱!";
        }
        lblProgramName.Text = ProgramName;
        lblProgramName.Font.Size = 11;
        lblProgramName.ForeColor = System.Drawing.Color.Blue; ;
        lblProgramName.Font.Bold = true;
    }
    protected void btnQueryCustomer_Click(object sender, EventArgs e)
    {
        string strSql = "select customerName  from [dbo].[ReleaseMaster] WHERE customerID = '" + txtCustomerID.Text + "'";
        String CustomerName = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt;
        dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            CustomerName = dr["customerName"].ToString();
        }
        else
        {
            CustomerName = "查無客戶名稱!";
        }
        lblCustomerName.Text = CustomerName;
        lblCustomerName.Font.Size = 11;
        lblCustomerName.ForeColor = System.Drawing.Color.Blue; ;
        lblCustomerName.Font.Bold = true;
    }
}