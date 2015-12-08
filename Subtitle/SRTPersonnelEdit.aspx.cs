using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class View_SRTPersonnelEdit : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("EditorID");
            //部門
            cbldepart.Items.Insert(0, new ListItem("節目部", "節目部"));
            cbldepart.Items.Insert(1, new ListItem("國外部", "國外部"));
            Form_DataBind();
        }
    }
    //帶入資料
    public void Form_DataBind()
    {
        //****變數宣告****//
        string strSql, Sql, uid;
        DataTable dt,dt1;

        //****變數設定****//
        uid = HFD_Uid.Value;

        //****設定查詢****//
        strSql = @" select * From [dbo].[_ST01P0]
                    where EditorID='" + uid + "'";
        Sql = "select EditorID,(select EditorDept+',' from [dbo].[_ST01M1] A where A.EditorID=B.EditorID For XML PATH('')) as EditorDept from [_ST01M1] B  where EditorID='" + uid + "' group by B.EditorID ";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);
        dt1 = NpoDB.QueryGetTable(Sql);

        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("SRTPersonnelQuery.aspx");

        DataRow dr = dt.Rows[0];

        //字幕人員代號
        tbxEditorID.Text = dr["EditorID"].ToString().Trim();
        //字幕人員姓名
        tbxEditorName.Text = dr["EditorName"].ToString().Trim();
        //身份證/統一編號
        tbxID.Text = dr["ID"].ToString().Trim();
        //連絡電話
        tbxLandPhone.Text = dr["LandPhone"].ToString().Trim();
        //手機號碼
        tbxMobilePhone.Text = dr["MobilePhone"].ToString().Trim();
        //戶籍地址
        tbxAddress.Text = dr["Address"].ToString();
        //電子郵件 1
        tbxEmail_1.Text = dr["Email_1"].ToString();
        //電子郵件 2
        tbxEmail_2.Text = dr["Email_2"].ToString().Trim();
        //評比
        tbxComparison.Text = dr["Comparison"].ToString().Trim();
        
        if (dt1.Rows.Count > 0)
        {
            DataRow dr1 = dt1.Rows[0];
            if (dr1["EditorDept"].ToString().Contains("300"))
            {
                cbldepart.Items[0].Selected = true;
            }
            if (dr1["EditorDept"].ToString().Contains("400"))
            {
                cbldepart.Items[1].Selected = true;
            }
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        bool flag = false;
        try
        {
            SRTPersonnel_Edit();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        if (flag == true)
        {
            AjaxShowMessage("字幕人員修改成功！");
            Response.Redirect(Util.RedirectByTime("SRTPersonnelEdit.aspx", "EditorID=" + HFD_Uid.Value));
        }
    }
    public void SRTPersonnel_Edit()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string Sql1 = "", Sql2 = "";
        Sql1 = "delete from [dbo].[_ST01M1] where EditorID='" + HFD_Uid.Value + "';";
        NpoDB.ExecuteSQLS(Sql1, dict);
        if (cbldepart.Items[0].Selected)
        {
            Sql2 = "INSERT INTO [dbo].[_ST01M1] ([EditorID],[EditorDept]) VALUES ('" + HFD_Uid.Value + "','300');";
        }
        if (cbldepart.Items[1].Selected)
        {
            Sql2 += "INSERT INTO [dbo].[_ST01M1] ([EditorID],[EditorDept]) VALUES ('" + HFD_Uid.Value + "','400');";
        }
        if (Sql2 != "")
        {
            NpoDB.ExecuteSQLS(Sql2, dict);
        }

        string strSql = " update [dbo].[_ST01P0] set ";

        strSql += "  EditorName = @EditorName";
        strSql += ", ID = @ID";
        strSql += ", LandPhone = @LandPhone";
        strSql += ", MobilePhone = @MobilePhone";
        strSql += ", Address = @Address";
        strSql += ", Email_1 = @Email_1";
        strSql += ", Email_2 = @Email_2";
        strSql += ", Comparison = @Comparison";

        strSql += ", ModifiedUser= @ModifiedUser";
        strSql += ", ModifiedDatetime = @ModifiedDatetime";
        strSql += " where EditorID = @EditorID;";

        dict.Add("EditorName", tbxEditorName.Text.Trim());
        dict.Add("ID", tbxID.Text.Trim());
        dict.Add("LandPhone", tbxLandPhone.Text.Trim());
        dict.Add("MobilePhone", tbxMobilePhone.Text.Trim());
        dict.Add("Address", tbxAddress.Text.Trim());
        dict.Add("Email_1", tbxEmail_1.Text.Trim());
        dict.Add("Email_2", tbxEmail_2.Text.Trim());
        dict.Add("Comparison", tbxComparison.Text.Trim());

        dict.Add("ModifiedUser", SessionInfo.UserID);
        dict.Add("ModifiedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("EditorID", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

    }
    //----------------------------------------------------------------------
    protected void btnDel_Click(object sender, EventArgs e)
    {
        string strSql = "delete from [dbo].[_ST01P0] where EditorID='" + HFD_Uid.Value + "';";
        strSql += "delete from [dbo].[_ST01M1] where EditorID='" + HFD_Uid.Value + "';";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        NpoDB.ExecuteSQLS(strSql, dict);

        AjaxShowMessage("字幕人員刪除成功！");
        Response.Redirect(Util.RedirectByTime("SRTPersonnelQuery.aspx"));
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("SRTPersonnelQuery.aspx"));
    }
}