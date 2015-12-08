using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class ProgramChange_ProgramChangeApply : BasePage
{
    string ChangeId = "";
    string hrcon = System.Configuration.ConfigurationManager.ConnectionStrings["HRConnection"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tbxChangeDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ChangeId = DateTime.Now.ToString("yyyyMMddhhmmss");
        //寄送Email用
        //string MailHead = "";
        //string MailFrom = "";
        //string MailSubject = "";
        //string MailBody = "";
        //string MailBody2 = "";
        //string MailTo = "";
        //string MailToRequester = "";
        bool flag = false;
        try
        {
            ProgramChangeApply_AddNew();
            flag = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (flag == true)
        {
            string query = @"
                            SELECT E.[EMPLOYEE_NO],E.[EMPLOYEE_CNAME],E.[EMPLOYEE_EMAIL_1]
                                ,E.[EMPLOYEE_OFFICE_TEL_1],E.[EMPLOYEE_CONTACT_TEL_1],D.[DEPARTMENT_CNAME],D.[DEPARTMENT_CODE]
                            FROM [HRMS_EMPLOYEE] as E,[HRMS_DEPARTMENT] as D
                            WHERE E.[DEPARTMENT_ID] = D.[DEPARTMENT_ID] AND E.[EMPLOYEE_NO] = '" + tbxRequester.Text.Trim() + "' ";
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(hrcon))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(ds, "HR");
            }
            DataRow HRrow = ds.Tables["HR"].Rows[0];
            //申請通知信
            string strBody = "";
            string strBody2 = "";
            string MailEnd = "";
            string web = System.Configuration.ConfigurationManager.AppSettings["web"];
            //string url = web + "ProgramChange/ProgramChangeEmailCheckApprove.aspx?ChangeId=";
            string url = web + "ProgramChange/ProgramChangeEmailCheckApprove.aspx";
            string MailHead = "有一張異動申請正等待您的核可:<BR><BR>";
            string MailHead2 = "有一張異動申請知會:<BR><BR>";
            strBody = "姓名：" + HRrow["EMPLOYEE_CNAME"].ToString().Trim() + "<BR>";
            strBody += "工號：" + HRrow["EMPLOYEE_NO"].ToString().Trim() + "<BR>";
            strBody += "部門：" + HRrow["DEPARTMENT_CNAME"].ToString().Trim() + "<BR>";
            strBody += "分機：" + HRrow["EMPLOYEE_OFFICE_TEL_1"].ToString().Substring(14) + "<BR>";
            strBody += "手機：" + HRrow["EMPLOYEE_CONTACT_TEL_1"].ToString().Trim() + "<BR>";
            strBody += "異動編號：" + ChangeId + "<BR>";
            strBody += "異動台別：" + ddlChannel.SelectedValue + "<BR>";
            strBody += "異動日期：" + String.Format("{0:yyyy-MM-dd}",Convert.ToDateTime(tbxChangeDate.Text)) + "<BR>";
            strBody += "異動內容：<BR>" + tbxContent.Text.Replace("\r\n", "<BR>") + "<BR><BR>";
            strBody2 = "<BR><form action='" + url + "' method='POST'>";
            strBody2 += "<input name='ChangeId' type='hidden' value='" + ChangeId + "'>";
            strBody2 += "<input name='Email' type='hidden' value='{0}'>";
            strBody2 += "<select name='check'><option value='Y'>核可</option><option value='N'>作廢</option></select><BR>";
            strBody2 += "<BR>審核內容：<BR><textarea name='reviewcomment' rows='6' cols='20'></textarea><BR>";
            strBody2 += "<BR><input type='submit' value='確認送出'></form><BR><BR>";
            MailEnd = "此為系統發出之信件，請勿回覆此信件！";

            SendEMailObject MailObject = new SendEMailObject();
            //MailObject.SmtpServer = SmtpServer;
            string MailFrom = System.Configuration.ConfigurationManager.AppSettings["MailFrom"];
            string MailSubject = "節目異動--申請通知";
            string MailBody = MailHead + strBody + strBody2 + MailEnd;
            //MailTo = System.Configuration.ConfigurationManager.AppSettings["EmailToPMS"];

            //審核人通知信
            string[] MailToArray = NpoDB.GetEMailAddressList("節目異動申請通知審核");
            foreach (string MailTo in MailToArray)
            {
                string result = MailObject.SendEmail(MailTo, MailFrom, MailSubject, String.Format(MailBody,MailTo));
                if (MailObject.ErrorCode != 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result + "');</script>");
                    return;
                }
            }
            //知會通知信
            string MailBody3 = MailHead2 + strBody + MailEnd;
            string MailToNotify = String.Join(";", NpoDB.GetEMailAddressList("節目異動申請通知知會"));
            string result2 = MailObject.SendEmail(MailToNotify, MailFrom, MailSubject, MailBody3);
            if (MailObject.ErrorCode != 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result2 + "');</script>");
                return;
            }
            //申請人通知信
            string MailBody2 = strBody + MailEnd;
            string MailToRequester = HRrow["EMPLOYEE_EMAIL_1"].ToString().Trim();
            string result3 = MailObject.SendEmail(MailToRequester, MailFrom, MailSubject, MailBody2);
            if (MailObject.ErrorCode != 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('" + MailObject.ErrorMessage + MailObject.ErrorCode + "=>" + result3 + "');</script>");
                return;
            }
            //Response.Write("<Script language='JavaScript'>alert('節目異動申請新增成功！');</Script>");
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<Script language='JavaScript'>alert('申請單號：" + ChangeId + "');location.href=('ProgramChangeApply.aspx');</Script>");
            //Response.Write("<Script language='JavaScript'>alert('申請單號：" + ChangeId + "');location.href=('ProgramChangeApply.aspx');</Script>");
            // 新增後導向頁面
            //Page.Response.Write("<Script language='JavaScript'>location.href=('ProgramChangeApply.aspx');</Script>");
            //Response.End();
        }
    }

    public void ProgramChangeApply_AddNew()
    {
        string strSql = @"INSERT INTO [dbo].[_PG01P0]
                        (ChangeID,ChangeDate,Channel,Content,Requester,RequestDate,Status,CreatedUser,CreatedDatetime)
                        VALUES (@ChangeID,@ChangeDate,@Channel,@Content,@Requester,@RequestDate,@Status,@CreatedUser,@CreatedDatetime)";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ChangeID", ChangeId);
        dict.Add("ChangeDate", tbxChangeDate.Text.Trim());
        dict.Add("Channel", ddlChannel.SelectedValue);
        dict.Add("Content", tbxContent.Text.Trim());
        dict.Add("Requester", tbxRequester.Text.Trim());
        dict.Add("RequestDate", DateTime.Now.ToString("yyyy-MM-dd"));
        dict.Add("Status", "01");

        dict.Add("CreatedUser", SessionInfo.UserID);
        //dict.Add("CreatedUser", "admin");
        dict.Add("CreatedDatetime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        NpoDB.ExecuteSQLS(strSql, dict);
    }

    //----------------------------------------------------------------------
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ProgramChangeApply.aspx"));
    }

}
