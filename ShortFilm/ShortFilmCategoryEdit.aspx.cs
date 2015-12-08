using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShortFilm_ShortFilmCategoryEdit : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HFD_Uid.Value = Util.GetQueryString("Category");
            Form_DataBind();
        }
    }

    //帶入資料
    public void Form_DataBind()
    {
        //****變數宣告****//
        string strSql, uid;
        DataTable dt;

        //****變數設定****//
        uid = HFD_Uid.Value;

        //****設定查詢****//
        strSql = @" select * From [dbo].[_CF01P0]
                    where Category='" + uid + "'";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);


        //資料異常
        if (dt.Rows.Count <= 0)
            //todo : add Default.aspx page
            Response.Redirect("ShortFilmCategoryQuery.aspx");

        DataRow dr = dt.Rows[0];

        //短片類別
        txtCategory.Text = dr["Category"].ToString().Trim();
        //短片類別說明
        txtDescription.Text = dr["Description"].ToString().Trim();
        //設定日期
        txtSetDate.Text = DateTime.Parse(dr["SetDate"].ToString()).ToString("yyyy/MM/dd");
        //可申請多號
        if (dr["RequestMultipleNumber"].ToString().Trim() == "Y")
        {
            cbxRequestMultipleNumber.Checked = true;
        }
        else
        {
            cbxRequestMultipleNumber.Checked = false;
        }
        //必須有節目
        if (dr["ProgramExist"].ToString().Trim() == "Y")
        {
            cbxProgramExist.Checked = true;
        }
        else
        {
            cbxProgramExist.Checked = false;
        }
        //必須有集數
        if (dr["EpisodeExist"].ToString().Trim() == "Y")
        {
            cbxEpisodeExist.Checked = true;
        }
        else
        {
            cbxEpisodeExist.Checked = false;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        bool flag = false;
        try
        {
            ShortFilmCategory_Edit();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        if (flag == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "alert('儲存成功！');location.href=('ShortFilmCategoryQuery.aspx');", true);
            //Response.Write("<Script language='JavaScript'>alert('儲存成功');</Script>");
            //Response.Redirect(Util.RedirectByTime("ShortFilmCategoryQuery.aspx"));
            //Page.Response.Write("<Script language='JavaScript'>location.href=('ShortFilmCategoryQuery.aspx');</Script>");
            //Response.End();
        }
    }

    public void ShortFilmCategory_Edit()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string strSql = " update [dbo].[_CF01P0] set ";

        //strSql += "  Category = @Category";
        strSql += "  Description = @Description";
        strSql += ", SetDate = @SetDate";
        strSql += ", RequestMultipleNumber = @RequestMultipleNumber";
        strSql += ", ProgramExist = @ProgramExist";
        strSql += ", EpisodeExist = @EpisodeExist";

        strSql += ", ModifiedUser= @ModifiedUser";
        strSql += ", ModifiedDatetime = @ModifiedDatetime";
        strSql += " where Category = @Category";

        //dict.Add("Category", txtCategory.Text.Trim());
        dict.Add("Description", txtDescription.Text.Trim());
        dict.Add("SetDate", txtSetDate.Text.Trim());
        if (cbxRequestMultipleNumber.Checked == true)
        {
            dict.Add("RequestMultipleNumber", "Y");
        }
        else
        {
            dict.Add("RequestMultipleNumber", "N");
        }
        if (cbxProgramExist.Checked == true)
        {
            dict.Add("ProgramExist", "Y");
        }
        else
        {
            dict.Add("ProgramExist", "N");
        }
        if (cbxEpisodeExist.Checked == true)
        {
            dict.Add("EpisodeExist", "Y");
        }
        else
        {
            dict.Add("EpisodeExist", "N");
        }

        dict.Add("ModifiedUser", SessionInfo.UserID);
        dict.Add("ModifiedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("Category", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        string strSql = "delete from [dbo].[_CF01P0] where Category=@Category";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Category", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PopupScript", "alert('刪除成功！');location.href=('ShortFilmCategoryQuery.aspx');", true);
        //Response.Write("<Script language='JavaScript'>alert('刪除成功');location.href=('ShortFilmCategoryQuery.aspx');</Script>");
        //Response.Redirect(Util.RedirectByTime("ShortFilmCategoryQuery.aspx"));
        //Page.Response.Write("<Script language='JavaScript'>location.href=('ShortFilmCategoryQuery.aspx');</Script>");
        //Response.End();
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ShortFilmCategoryQuery.aspx"));
    }

}
