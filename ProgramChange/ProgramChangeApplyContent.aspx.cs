using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProgramChange_ProgramChangeApplyContent : BasePage //System.Web.UI.Page
{

    string hrcon = System.Configuration.ConfigurationManager.ConnectionStrings["HRConnection"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //接收參數
            HFD_Mode.Value = Util.GetQueryString("Mode");
            HFD_Uid.Value = Util.GetQueryString("ChangeID");
            HFD_ApplyQuery_Key.Value = Util.GetQueryString("ApplyQuery_Key");
            //Page.Response.Write("<Script language='JavaScript'>alert('" + HFD_Key.Value + "');</Script>");

            //輸入欄位
            tbxReviewer.Visible = false;
            tbxActionPerformer.Visible = false;
            tbxReviewComment.Visible = false;
            tbxActionContent.Visible = false;

            //按鈕
            btnDel.Visible = false;
            btnInvalid.Visible = false;
            btnApprove.Visible = false;
            btnEdit.Visible = false;

            //顯示
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
        strSql = @" select ChangeID, Requester, CONVERT(VarChar,ChangeDate,111) as 'ChangeDate', Channel, Content,
                           Reviewer, ReviewComment, ActionPerformer, ActionContent, Status
                           from [dbo].[_PG01P0]
                    where ChangeID = '" + uid + "'";
        //****執行語法****//
        dt = NpoDB.QueryGetTable(strSql);

        //資料異常
        if (dt.Rows.Count <= 0)
        {
            Response.Redirect("ProgramChangeApplyQuery.aspx?Mode=" + HFD_Mode.Value);
        }
        DataRow dr = dt.Rows[0];

        //狀態
        string strStatus = dr["Status"].ToString();
        switch (strStatus)
        {
            case "01":
                lblStatus.Text = "待審";
                if (HFD_Mode.Value == "applier")
                {
                    btnDel.Visible = true;
                }
                else if (HFD_Mode.Value == "reviewer")
                {
                    btnInvalid.Visible = true;
                    btnApprove.Visible = true;
                    tbxReviewer.Visible = true;
                    tbxReviewComment.Visible = true;
                }
                break;
            case "02":
                lblStatus.Text = "待執行";
                if (HFD_Mode.Value == "processor")
                {
                    btnEdit.Visible = true;
                    tbxActionPerformer.Visible = true;
                    tbxActionContent.Visible = true;
                }
                break;
            case "03":
                lblStatus.Text = "異動完成";
                break;
            case "04":
                lblStatus.Text = "退件";
                break;
        }

        //單號
        lblChangeID.Text = dr["ChangeID"].ToString();
        //申請人
        lblRequester.Text = dr["Requester"].ToString();
        lblRequesterName.Text = NpoDB.GetHRUser(dr["Requester"].ToString());
        //異動日期
        lblChangeDate.Text = dr["ChangeDate"].ToString();
        //異動頻道
        if (dr["Channel"].ToString() == "01")
        {
            lblChannel.Text = "一台";
        }
        else if (dr["Channel"].ToString() == "02")
        {
            lblChannel.Text = "二台";
        }
        //異動內容
        lblContent.Text = dr["Content"].ToString().Replace("\r\n","<br/>");
        //審核人
        lblReviewer.Text = dr["Reviewer"].ToString();
        lblReviewerName.Text = dr["Reviewer"].ToString() != "" ? NpoDB.GetHRUser(dr["Reviewer"].ToString()) : "";
        //審核內容
        lblReviewComment.Text = dr["ReviewComment"].ToString().Replace("\r\n", "<br/>");
        //片庫作業同工
        lblActionPerformer.Text = dr["ActionPerformer"].ToString();
        lblActionPerformerName.Text = dr["ActionPerformer"].ToString() != "" ? NpoDB.GetHRUser(dr["ActionPerformer"].ToString()) : "";
        //片庫作業結果
        lblActionContent.Text = dr["ActionContent"].ToString().Replace("\r\n", "<br/>");

    }

    //離開
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProgramChangeApplyQuery.aspx?Mode=" + HFD_Mode.Value + "&ApplyQuery_Key=" + HFD_ApplyQuery_Key.Value);
    }

    //取消申請
    protected void btnDel_Click(object sender, EventArgs e)
    {
        try
        {
            string strSql = "delete from [dbo].[_PG01P0] where ChangeID = @ChangeID";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ChangeID", HFD_Uid.Value);
            NpoDB.ExecuteSQLS(strSql, dict);

            ClientScript.RegisterStartupScript(this.GetType(), "s", "<script language='JavaScript'>alert('取消完成');location.href=('ProgramChangeApplyQuery.aspx?Mode=" + HFD_Mode.Value + "&ApplyQuery_Key=" + HFD_ApplyQuery_Key.Value + "');</script>");
            //Response.Write("<Script language='JavaScript'>alert('取消完成');location.href=('ProgramChangeApplyQuery.aspx?ApplyQuery_Key=" + HFD_ApplyQuery_Key.Value + "');</Script>");
            //Response.Redirect(Util.RedirectByTime("ProgramChangeApplyQuery.aspx"));
            //Page.Response.Write("<Script language='JavaScript'>location.href=('ProgramChangeApplyQuery.aspx?ApplyQuery_Key=" + HFD_ApplyQuery_Key.Value + "');</Script>");
            //Response.End();
        }
        catch (Exception ex)
        {
            System.Console.Out.Write(ex.Message);
        }
    }

    //申請作廢
    protected void btnInvalid_Click(object sender, EventArgs e)
    {

        bool flag = false;
        try
        {
            ProgramChange_ApplyInvalid();
            flag = true;
        }
        catch (Exception ex)
        {
            System.Console.Out.Write(ex.Message);
            //throw ex;
        }
        if (flag == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('作廢完成');location.href=('ProgramChangeApplyQuery.aspx?Mode=" + HFD_Mode.Value + "&ApplyQuery_Key=" + HFD_ApplyQuery_Key.Value + "');</script>");
            //Response.Write("<Script language='JavaScript'>alert('節目異動申請內容動作完成！');</Script>");
            //Response.Redirect(Util.RedirectByTime("ProgramChangeVerify.aspx"));
            //Page.Response.Write("<Script language='JavaScript'>location.href=('ProgramChangeVerify.aspx?Key=" + HFD_Key.Value + "');</Script>");
            //Response.End();
        }

    }

    //申請作廢 update data
    public void ProgramChange_ApplyInvalid()
    {
        //****變數宣告****
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****
        string strSql = " update [dbo].[_PG01P0] set ";

        strSql += "  Reviewer = @Reviewer";
        strSql += ", ReviewDate = @ReviewDate";
        strSql += ", ReviewComment = @ReviewComment";
        strSql += ", Status = @Status";

        strSql += ", ModifiedUser= @ModifiedUser";
        strSql += ", ModifiedDatetime = @ModifiedDatetime";
        strSql += "  where ChangeID = @ChangeID";

        dict.Add("Reviewer", tbxReviewer.Text);
        dict.Add("ReviewDate", DateTime.Now.ToString("yyyy-MM-dd"));
        dict.Add("ReviewComment", tbxReviewComment.Text);
        dict.Add("Status", "04");

        dict.Add("ModifiedUser", SessionInfo.UserID);
        dict.Add("ModifiedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("ChangeID", lblChangeID.Text);
        NpoDB.ExecuteSQLS(strSql, dict);

    }

    //核可
    protected void btnApprove_Click(object sender, EventArgs e)
    {

        bool flag = false;
        try
        {
            ProgramChangeApply_Approve();
            flag = true;
        }
        catch (Exception ex)
        {
            System.Console.Out.Write(ex.Message);
            //throw ex;
        }
        if (flag == true)
        {
            //部分信件內容
            string query = @"
                            SELECT E.[EMPLOYEE_NO],E.[EMPLOYEE_CNAME],E.[EMPLOYEE_EMAIL_1]
                                ,E.[EMPLOYEE_OFFICE_TEL_1],E.[EMPLOYEE_CONTACT_TEL_1],D.[DEPARTMENT_CNAME],D.[DEPARTMENT_CODE]
                            FROM [HRMS_EMPLOYEE] as E,[HRMS_DEPARTMENT] as D
                            WHERE E.[DEPARTMENT_ID] = D.[DEPARTMENT_ID] AND E.[EMPLOYEE_NO] = '" + lblRequester.Text + "' ";
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(hrcon))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(ds, "HR");
            }
            DataRow HRrow = ds.Tables["HR"].Rows[0];

            SendEMailObject MailObject = new SendEMailObject();
            //MailObject.SmtpServer = SmtpServer;
            string MailSubject = "節目異動--審核通知";
            //string MailTo = System.Configuration.ConfigurationManager.AppSettings["EmailToPMS"];
            //申請人
            string MailToRequester = HRrow["EMPLOYEE_EMAIL_1"].ToString().Trim();
            //處理人
            string MailTo = String.Join(";", NpoDB.GetEMailAddressList("節目異動審核通知"));

            //審核通知信
            string MailBody = "姓名：" + HRrow["EMPLOYEE_CNAME"].ToString().Trim() + "<BR>";
            MailBody += "工號：" + HRrow["EMPLOYEE_NO"].ToString().Trim() + "<BR>";
            MailBody += "部門：" + HRrow["DEPARTMENT_CNAME"].ToString().Trim() + "<BR>";
            MailBody += "分機：" + HRrow["EMPLOYEE_OFFICE_TEL_1"].ToString().Substring(14) + "<BR>";
            MailBody += "手機：" + HRrow["EMPLOYEE_CONTACT_TEL_1"].ToString().Trim() + "<BR>";
            MailBody += "異動編號：" + lblChangeID.Text + "<BR>";
            MailBody += "異動台別：" + lblChannel.Text + "<BR>";
            MailBody += "異動日期：" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(lblChangeDate.Text)) + "<BR>";
            MailBody += "異動內容：<BR>" + lblContent.Text + "<BR><BR>";
            MailBody += "審核者：" + NpoDB.GetHRUser(tbxReviewer.Text) + "<BR>";
            MailBody += "審核內容：" + tbxReviewComment.Text.Trim().Replace("\r\n", "<BR>") + "<BR><BR>";
            MailBody += "此為系統發出之信件，請勿回覆此信件！";

            string MailFrom = System.Configuration.ConfigurationManager.AppSettings["MailFrom"];
            string result = MailObject.SendEmail(MailTo, MailFrom, MailSubject, MailBody);
            if (MailObject.ErrorCode != 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result + "');</script>");
            }
            string result2 = MailObject.SendEmail(MailToRequester, MailFrom, MailSubject, MailBody);
            if (MailObject.ErrorCode != 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result2 + "');</script>");
            }
            //------------------------------------------------------------------------------------------
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('審核通過');location.href=('ProgramChangeApplyQuery.aspx?Mode=" + HFD_Mode.Value + "&ApplyQuery_Key=" + HFD_ApplyQuery_Key.Value + "');</script>");
            //Response.Write("<Script language='JavaScript'>alert('節目異動申請內容審核完成');</Script>");
            //Response.Redirect(Util.RedirectByTime("ProgramChangeVerify.aspx"));
            //Page.Response.Write("<Script language='JavaScript'>location.href=('ProgramChangeVerify.aspx?Key=" + HFD_Key.Value + "');</Script>");
            //Response.End();
        }

    }

    //核可 update data
    public void ProgramChangeApply_Approve()
    {
        //****變數宣告****
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****
        string strSql = " update [dbo].[_PG01P0] set ";

        strSql += "  Reviewer = @Reviewer";
        strSql += ", ReviewDate = @ReviewDate";
        strSql += ", ReviewComment = @ReviewComment";
        strSql += ", Status = @Status";

        strSql += ", ModifiedUser= @ModifiedUser";
        strSql += ", ModifiedDatetime = @ModifiedDatetime";
        strSql += "  where ChangeID = @ChangeID";

        dict.Add("Reviewer", tbxReviewer.Text);
        dict.Add("ReviewDate", DateTime.Now.ToString("yyyy-MM-dd"));
        dict.Add("ReviewComment", tbxReviewComment.Text.Trim());
        dict.Add("Status", "02");

        dict.Add("ModifiedUser", SessionInfo.UserID);
        dict.Add("ModifiedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("ChangeID", lblChangeID.Text);
        NpoDB.ExecuteSQLS(strSql, dict);

    }

    //儲存
    protected void btnEdit_Click(object sender, EventArgs e)
    {

        bool flag = false;
        try
        {
            ProgramChangeApply_Handle();
            flag = true;
        }
        catch (Exception ex)
        {
            System.Console.Out.Write(ex.Message);
            //throw ex;
        }
        if (flag == true)
        {
            //部分信件內容
            string query = @"
                            SELECT E.[EMPLOYEE_NO],E.[EMPLOYEE_CNAME],E.[EMPLOYEE_EMAIL_1]
                                ,E.[EMPLOYEE_OFFICE_TEL_1],E.[EMPLOYEE_CONTACT_TEL_1],D.[DEPARTMENT_CNAME],D.[DEPARTMENT_CODE]
                            FROM [HRMS_EMPLOYEE] as E,[HRMS_DEPARTMENT] as D
                            WHERE E.[DEPARTMENT_ID] = D.[DEPARTMENT_ID] AND E.[EMPLOYEE_NO] = '" + lblRequester.Text + "' ";
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(hrcon))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(ds, "HR");
            }
            DataRow HRrow = ds.Tables["HR"].Rows[0];

            SendEMailObject MailObject = new SendEMailObject();
            //MailObject.SmtpServer = SmtpServer;
            string MailSubject = "節目異動--確認通知";
            //MailTo = System.Configuration.ConfigurationManager.AppSettings["EmailToPMS"];
            //申請人
            string MailToRequester = HRrow["EMPLOYEE_EMAIL_1"].ToString().Trim();
            //確認人
            string MailTo = String.Join(";", NpoDB.GetEMailAddressList("節目異動確認通知"));

            //確認通知信
            string MailBody = "姓名：" + HRrow["EMPLOYEE_CNAME"].ToString().Trim() + "<BR>";
            MailBody += "工號：" + HRrow["EMPLOYEE_NO"].ToString().Trim() + "<BR>";
            MailBody += "部門：" + HRrow["DEPARTMENT_CNAME"].ToString().Trim() + "<BR>";
            MailBody += "分機：" + HRrow["EMPLOYEE_OFFICE_TEL_1"].ToString().Substring(14) + "<BR>";
            MailBody += "手機：" + HRrow["EMPLOYEE_CONTACT_TEL_1"].ToString().Trim() + "<BR>";
            MailBody += "異動編號：" + lblChangeID.Text + "<BR>";
            MailBody += "異動台別：" + lblChannel.Text + "<BR>";
            MailBody += "異動日期：" + String.Format("{0:yyyy-MM-dd}",Convert.ToDateTime(lblChangeDate.Text)) + "<BR>";
            MailBody += "異動內容：<BR>" + lblContent.Text + "<BR><BR>";
            MailBody += "審核者：" + lblReviewerName.Text + "<BR>";
            MailBody += "審核內容：" + lblReviewComment.Text + "<BR><BR>";
            MailBody += "片庫執行同工：" + NpoDB.GetHRUser(tbxActionPerformer.Text) + "<BR>";
            MailBody += "片庫執行結果：" + tbxActionContent.Text.Trim().Replace("\r\n", "<BR>") + "<BR><BR>";
            MailBody += "此為系統發出之信件，請勿回覆此信件！";

            string MailFrom = System.Configuration.ConfigurationManager.AppSettings["MailFrom"];
            string result = MailObject.SendEmail(MailTo, MailFrom, MailSubject, MailBody);
            if (MailObject.ErrorCode != 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result + "');</script>");
            }
            string result2 = MailObject.SendEmail(MailToRequester, MailFrom, MailSubject, MailBody);
            if (MailObject.ErrorCode != 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result2 + "');</script>");
            }
            //--------------------------------------------------------------------------------------------------
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('處理資料儲存完成');location.href=('ProgramChangeApplyQuery.aspx?Mode=" + HFD_Mode.Value + "&ApplyQuery_Key=" + HFD_ApplyQuery_Key.Value + "');</script>");
            //Response.Write("<Script language='JavaScript'>alert('處理資料儲存完成');</Script>");
            //Response.Redirect(Util.RedirectByTime("ProgramChangeHandle.aspx"));
            //Page.Response.Write("<Script language='JavaScript'>location.href=('ProgramChangeHandle.aspx?Key=" + HFD_Key.Value + "');</Script>");
            //Response.End();
        }

    }

    //儲存 update data
    public void ProgramChangeApply_Handle()
    {

        //****變數宣告****
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****
        string strSql = " update [dbo].[_PG01P0] set ";

        strSql += "  ActionPerformer = @ActionPerformer";
        strSql += ", ActionDate = @ActionDate";
        strSql += ", ActionContent = @ActionContent";
        strSql += ", Status = @Status";

        strSql += ", ModifiedUser= @ModifiedUser";
        strSql += ", ModifiedDatetime = @ModifiedDatetime";
        strSql += " where ChangeID = @ChangeID";

        dict.Add("ActionPerformer", tbxActionPerformer.Text);
        dict.Add("ActionDate", DateTime.Now.ToString("yyyy-MM-dd"));
        dict.Add("ActionContent", tbxActionContent.Text.Trim());
        dict.Add("Status", "03");

        dict.Add("ModifiedUser", SessionInfo.UserID);
        dict.Add("ModifiedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

        dict.Add("ChangeID", HFD_Uid.Value);
        NpoDB.ExecuteSQLS(strSql, dict);

    }

}
