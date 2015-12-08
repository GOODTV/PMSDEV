using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class ProgramChange_ProgramChangeEmailCheckApprove : System.Web.UI.Page
{
    string ChangeId;
    string check = "";
    string ReviewComment = "";
    string hrcon = System.Configuration.ConfigurationManager.ConnectionStrings["HRConnection"].ConnectionString;
    //寄送Email用
    string MailFrom = "";
    string MailSubject = "";
    string MailBody = "";
    string MailTo = "";
    string MailToRequester = "";
    string strEmployeeno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //信件接收參數
            strEmployeeno = GetEmployeenoFromEMail(Request.Params["Email"].ToString());
            if (String.IsNullOrEmpty(strEmployeeno))
            {
                Response.Write("<Script language='JavaScript'>alert('權限不足，無法審核！');window.close();</Script>");
                Response.End();
                return;
            }
            ChangeId = Request.Params["ChangeId"].ToString();
            check = Request.Params["check"].ToString();
            ReviewComment = Request.Params["reviewcomment"].ToString();

            DataTable dt;
            string strSql = @" select ChangeID, Requester, [ChangeDate], Content, Reviewer, ReviewComment, Channel, [Status]
                            from [dbo].[_PG01P0] where ChangeID = '" + ChangeId + "'";
            //****執行語法****//
            dt = NpoDB.QueryGetTable(strSql);

            if (dt.Rows.Count == 0)
            {
                Response.Write("<Script language='JavaScript'>alert('此節目異動已取消！');window.close();</Script>");
                Response.End();
                return;
            }

            DataRow dr = dt.Rows[0];
            string Status = dr["Status"].ToString();

            switch (Status)
            {
                case "02":
                    Response.Write("<Script language='JavaScript'>alert('此節目異動已審核！');window.close();</Script>");
                    Response.End();
                    return;
                case "03":
                    Response.Write("<Script language='JavaScript'>alert('此節目異動已處理！');window.close();</Script>");
                    Response.End();
                    return;
                case "04":
                    Response.Write("<Script language='JavaScript'>alert('此節目異動已退件！');window.close();</Script>");
                    Response.End();
                    return;
            }

            //信件部分內容
            
            //申請人
            string query = @"
                            SELECT E.[EMPLOYEE_NO],E.[EMPLOYEE_CNAME],E.[EMPLOYEE_EMAIL_1]
                                ,E.[EMPLOYEE_OFFICE_TEL_1],E.[EMPLOYEE_CONTACT_TEL_1],D.[DEPARTMENT_CNAME],D.[DEPARTMENT_CODE]
                            FROM [HRMS_EMPLOYEE] as E,[HRMS_DEPARTMENT] as D
                            WHERE E.[DEPARTMENT_ID] = D.[DEPARTMENT_ID] AND E.[EMPLOYEE_NO] = '" + dr["Requester"].ToString().Trim() + "' ";
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(hrcon))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(ds, "HR");
            }
            DataRow HRrow = ds.Tables["HR"].Rows[0];

            //審核者
            string query2 = @"
                            SELECT E.[EMPLOYEE_NO],E.[EMPLOYEE_CNAME],E.[EMPLOYEE_EMAIL_1]
                                ,E.[EMPLOYEE_OFFICE_TEL_1],E.[EMPLOYEE_CONTACT_TEL_1],D.[DEPARTMENT_CNAME],D.[DEPARTMENT_CODE]
                            FROM [HRMS_EMPLOYEE] as E,[HRMS_DEPARTMENT] as D
                            WHERE E.[DEPARTMENT_ID] = D.[DEPARTMENT_ID] AND E.[EMPLOYEE_NO] = '" + strEmployeeno + "' ";
            DataSet ds2 = new DataSet();
            using (SqlConnection conn = new SqlConnection(hrcon))
            {
                SqlDataAdapter da = new SqlDataAdapter(query2, conn);
                da.Fill(ds2, "HR");
            }
            DataRow HR_row = ds2.Tables["HR"].Rows[0];

            //申請通知信
            string strBody = "";
            strBody = "姓名：" + HRrow["EMPLOYEE_CNAME"].ToString().Trim() + "<BR>";
            strBody += "工號：" + HRrow["EMPLOYEE_NO"].ToString().Trim() + "<BR>";
            strBody += "部門：" + HRrow["DEPARTMENT_CNAME"].ToString().Trim() + "<BR>";
            strBody += "分機：" + HRrow["EMPLOYEE_OFFICE_TEL_1"].ToString().Substring(14) + "<BR>";
            strBody += "手機：" + HRrow["EMPLOYEE_CONTACT_TEL_1"].ToString().Trim() + "<BR>";
            strBody += "異動編號：" + ChangeId + "<BR>";
            strBody += "異動台別：" + dr["Channel"].ToString().Trim() + "<BR>";
            strBody += "異動日期：" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dr["ChangeDate"])) + "<BR>";
            strBody += "異動內容：<BR>" + dr["Content"].ToString().Trim() + "<BR><BR>";
            strBody += "審核者：" + HR_row["EMPLOYEE_CNAME"].ToString().Trim() + "<BR>";
            strBody += "審核內容：" + ReviewComment.Replace("\r\n", "<BR>") + "<BR><BR>";
            strBody += "此為系統發出之信件，請勿回覆此信件！";

            MailToRequester = HRrow["EMPLOYEE_EMAIL_1"].ToString().Trim();
            MailFrom = ConfigurationManager.AppSettings["MailFrom"];

            if (check == "Y") //核可
            {
                //送信

                ProgramChangeApply_Edit();

                SendEMailObject MailObject = new SendEMailObject();
                //MailObject.SmtpServer = SmtpServer;
                MailBody = strBody;
                MailSubject = " 節目異動--審核通知";
                //MailTo = System.Configuration.ConfigurationManager.AppSettings["EmailToPMS"];
                MailTo = String.Join(";", NpoDB.GetEMailAddressList("節目異動審核通知"));
                string result = MailObject.SendEmail(MailTo, MailFrom, MailSubject, MailBody);
                if (MailObject.ErrorCode != 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result + "');</script>");
                    return;
                }
                string result2 = MailObject.SendEmail(MailToRequester, MailFrom, MailSubject, MailBody);
                if (MailObject.ErrorCode != 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result2 + "');</script>");
                    return;
                }
                //------------------------------------------------------------------------------------------
            }
            else //作廢
            {
                ProgramChangeApply_Void();

                SendEMailObject MailObject = new SendEMailObject();
                string MailHead = "有一張異動申請已作廢:<BR><BR>";
                MailBody = MailHead + strBody;
                MailSubject = " 節目異動--作廢通知";
                string result2 = MailObject.SendEmail(MailToRequester, MailFrom, MailSubject, MailBody);
                if (MailObject.ErrorCode != 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result2 + "');</script>");
                    return;
                }
            }
            
            Response.Write("<Script language='JavaScript'>alert('審核完成！');window.close();</Script>");
            Response.End();
        }
    }
    public void ProgramChangeApply_Void()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string strSql = " update [dbo].[_PG01P0] set ";
        strSql += "  Reviewer = @Reviewer";
        strSql += ", ReviewDate = @ReviewDate";
        strSql += ", ReviewComment = @ReviewComment";
        strSql += ", Status = @Status";
        strSql += ", ModifiedUser= @ModifiedUser";
        strSql += ", ModifiedDatetime = @ModifiedDatetime";
        strSql += " where ChangeID = @ChangeID";

        dict.Add("Reviewer", strEmployeeno);
        dict.Add("ReviewDate", DateTime.Now.ToString("yyyy-MM-dd"));
        dict.Add("ReviewComment", ReviewComment);
        dict.Add("Status", "04");
        dict.Add("ModifiedUser", strEmployeeno);
        dict.Add("ModifiedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        dict.Add("ChangeID", ChangeId);
        NpoDB.ExecuteSQLS(strSql, dict);

    }
    public void ProgramChangeApply_Edit()
    {
        //****變數宣告****//
        Dictionary<string, object> dict = new Dictionary<string, object>();

        //****設定SQL指令****//
        string strSql = " update [dbo].[_PG01P0] set ";
        strSql += "  Reviewer = @Reviewer";
        strSql += ", ReviewDate = @ReviewDate";
        strSql += ", ReviewComment = @ReviewComment";
        strSql += ", Status = @Status";
        strSql += ", ModifiedUser= @ModifiedUser";
        strSql += ", ModifiedDatetime = @ModifiedDatetime";
        strSql += " where ChangeID = @ChangeID";

        dict.Add("Reviewer", strEmployeeno);
        dict.Add("ReviewDate", DateTime.Now.ToString("yyyy-MM-dd"));
        dict.Add("ReviewComment", ReviewComment);
        dict.Add("Status", "02");
        dict.Add("ModifiedUser", strEmployeeno);
        dict.Add("ModifiedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        dict.Add("ChangeID", ChangeId);
        NpoDB.ExecuteSQLS(strSql, dict);

    }

    public string GetEmployeenoFromEMail(string EMail)
    {
        string query = @"SELECT distinct [Employee_no] FROM [EMailAddressList] " +
                        " WHERE [Enable] = 1 AND [EMailType] = '節目異動申請通知審核' AND [EMailAddress] = '" + EMail + "' ";
        DataTable dt = NpoDB.QueryGetTable(query);

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();

        }
        else
        {
            return "";
        }

    }

}
