using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShortFilm_ShortFilmCategoryAdd : BasePage
{

    bool flag = false;
    
    protected void Page_Load(object sender, EventArgs e)
    {

        txtSetDate.Text =  DateTime.Now.ToString("yyyy/MM/dd");

    }

    //----------------------------------------------------------------------
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            ShortFilmCategory_AddNew();
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (flag == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "alert('儲存成功！');location.href=('ShortFilmCategoryQuery.aspx');", true);
            //Response.Write("<Script language='JavaScript'>alert('短片類別新增成功！');</Script>");
            // 新增後導向頁面
            //Response.Redirect("ShortFilmCategoryQuery.aspx");
            //Page.Response.Write("<Script language='JavaScript'>location.href=('ShortFilmCategoryQuery.aspx');</Script>");
            //Response.End();
        }
    }

    public void ShortFilmCategory_AddNew()
    {
        string strSql = @"INSERT INTO [dbo].[_CF01P0]
                           ([Category]
                           ,[Description]
                           ,[SetDate]
                           ,[RequestMultipleNumber]
                           ,[ProgramExist]
                           ,[EpisodeExist]
                           ,[CreatedUser]
                           ,[CreatedDatetime])
                            VALUES (@Category,@Description,@SetDate,@RequestMultipleNumber,@ProgramExist,@EpisodeExist,@CreatedUser,@CreatedDatetime)";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Category", txtCategory.Text.Trim());
        dict.Add("Description", txtDescription.Text.Trim());
        dict.Add("SetDate", txtSetDate.Text.Trim());
        if (cbxRequestMultipleNumber.Checked)
        {
            dict.Add("RequestMultipleNumber", "Y");
        }
        else
        {
            dict.Add("RequestMultipleNumber", "N");
        }
        if (cbxProgramExist.Checked)
        {
            dict.Add("ProgramExist", "Y");
        }
        else
        {
            dict.Add("ProgramExist", "N");
        }
        if (cbxEpisodeExist.Checked)
        {
            dict.Add("EpisodeExist", "Y");
        }
        else
        {
            dict.Add("EpisodeExist", "N");
        }
        dict.Add("CreatedUser", SessionInfo.UserID);
        dict.Add("CreatedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        try
        {
            NpoDB.ExecuteSQLS(strSql, dict);
            flag = true;
        }
        catch (Exception ex) {
            System.Console.Out.Write(ex.Message);
            if (ex.Message.IndexOf("重複的索引鍵值") > -1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "alert('資料已存在');", true);
                //Response.Write("<Script language='JavaScript'>alert('短片類別已存在！');</Script>");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "alert('新增失敗');", true);
                //Response.Write("<Script language='JavaScript'>alert('短片類別新增失敗！');</Script>");
            }
            //Page.Response.Write("<Script language='JavaScript'>location.href=('ShortFilmCategoryQuery.aspx');</Script>");
            //Response.End();
            return;
        }

    }

    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ShortFilmCategoryQuery.aspx"));
    }

}
