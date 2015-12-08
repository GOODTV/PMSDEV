using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_SRTPersonnelAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tbxEditorName.Text = Util.GetQueryString("EditorName");
            //部門
            cbldepart.Items.Insert(0, new ListItem("節目部", "節目部"));
            cbldepart.Items.Insert(1, new ListItem("國外部", "國外部"));
        }
    }
    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        try
        {
            SRTPersonnel_AddNew();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (flag == true)
        {
            //Page.Controls.Add(new LiteralControl("<script>alert(\"分集資料新增成功！\");</script>"));
            //this.Page.RegisterStartupScript("s", "<script>alert('分集資料新增成功！');</script>");
            AjaxShowMessage("字幕人員新增成功！");
            // 新增後導向頁面
            Response.Redirect(Util.RedirectByTime("SRTPersonnelAdd.aspx"));

        }
    }
    public void SRTPersonnel_AddNew()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string Sql2 = "";
        if (cbldepart.Items[0].Selected)
        {
            Sql2 = "INSERT INTO [dbo].[_ST01M1] ([EditorID],[EditorDept]) VALUES ('" + tbxEditorID.Text.Trim() + "','300');";
        }
        if (cbldepart.Items[1].Selected)
        {
            Sql2 += "INSERT INTO [dbo].[_ST01M1] ([EditorID],[EditorDept]) VALUES ('" + tbxEditorID.Text.Trim() + "','400');";
        }
        if (Sql2 != "")
        {
            NpoDB.ExecuteSQLS(Sql2, dict);
        }

        string strSql = @"INSERT INTO [dbo].[_ST01P0]
                           ([EditorID]
                           ,[EditorName]
                           ,[ID]
                           ,[LandPhone]
                           ,[MobilePhone]
                           ,[Address]
                           ,[Email_1]
                           ,[Email_2]
                           ,[Comparison]
                           ,[CreatedUser]
                           ,[CreatedDatetime])
                            VALUES (@EditorID,@EditorName,@ID,@LandPhone,@MobilePhone,@Address,@Email_1,@Email_2,@Comparison
                            ,@CreatedUser,@CreatedDatetime)";
        dict.Add("EditorID", tbxEditorID.Text.Trim());
        dict.Add("EditorName", tbxEditorName.Text.Trim());
        dict.Add("ID", tbxID.Text.Trim());
        dict.Add("LandPhone", tbxLandPhone.Text.Trim());
        dict.Add("MobilePhone", tbxMobilePhone.Text.Trim());
        dict.Add("Address", tbxAddress.Text.Trim());
        dict.Add("Email_1", tbxEmail_1.Text.Trim());
        dict.Add("Email_2", tbxEmail_2.Text.Trim());
        dict.Add("Comparison", tbxComparison.Text.Trim());
        dict.Add("CreatedUser", SessionInfo.UserID);
        dict.Add("CreatedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        NpoDB.ExecuteSQLS(strSql, dict);

        
    }
    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("SRTPersonnelQuery.aspx"));
    }
}