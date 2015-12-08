using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProgramChange_ProgramChangeApplyQuery : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            HFD_Mode.Value = Util.GetQueryString("Mode");
            if (HFD_Mode.Value != "applier")
            {
                lblRequesterID.Visible = false;
                tbxRequesterID.Visible = false;
                if (HFD_Mode.Value == "processor")
                {
                    rblValidStatus.SelectedValue = "2";
                }
            }

            HFD_ApplyQuery_Key.Value = Util.GetQueryString("ApplyQuery_Key");
            string[] strArray;
            strArray = HFD_ApplyQuery_Key.Value.Split(';');
            if (strArray.Length > 1)
            {
                //ChangeDateS
                if (strArray[0] != "")
                {
                    tbxChangeDateS.Text = strArray[0];
                }
                //ChangeDateE
                if (strArray[1] != "")
                {
                    tbxChangeDateE.Text = strArray[1];
                }
                //ValidStatus
                if (strArray[2] != "")
                {
                    rblValidStatus.SelectedValue = strArray[2];
                }
                //RequesterID
                if (strArray[3] != "")
                {
                    tbxRequesterID.Text = strArray[3];
                }

                LoadFormData();
            }

        }

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }

    public void LoadFormData()
    {
        string strSql;
        DataTable dt;
        strSql = @"select Case when Status ='01' then 'approving.png' 
	                           when Status ='02' then 'approved.png' 
                               when Status ='03' then 'yes.png' 
                               when Status ='04' then 'locked.png' end as [Image], 
                          Case when Status ='01' then '待審' 
	                           when Status ='02' then '待執行' 
                               when Status ='03' then '異動完成' 
                               when Status ='04' then '退件' end as [Status], 
                ChangeID as '單號',CONVERT(VarChar,ChangeDate,111) as '異動日期' ,
                Case when Channel='01' then '一台' when Channel='02' then '二台' end as '異動頻道' ,
                Content as '異動內容' ,Requester as '申請人' ,CONVERT(VarChar,RequestDate,111) as '申請日期' ,
                Reviewer as '審核人' , Case when CONVERT(VarChar,ReviewDate,111)='9999/12/31' then '' 
                else CONVERT(VarChar,ReviewDate,111) end as '審核日期' , ActionPerformer as '片庫作業同工'
                from [dbo].[_PG01P0]   where 1=1 ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxChangeDateS.Text.Trim() != "")
        {
            strSql += " and  CONVERT(VarChar,ChangeDate,111) >= '" + tbxChangeDateS.Text.Trim() + "'";
        }
        if (tbxChangeDateE.Text.Trim() != "")
        {
            strSql += " and  CONVERT(VarChar,ChangeDate,111) <= '" + tbxChangeDateE.Text.Trim() + "'";
        }
        if (tbxRequesterID.Text.Trim() != "")
        {
            strSql += " and Requester = '" + tbxRequesterID.Text.Trim() + "'";
        }
        if (rblValidStatus.SelectedValue == "1") //未審核
        {
            strSql += " and  Status = '01' ";
        }
        else if (rblValidStatus.SelectedValue == "2") //未處理
        {
            strSql += " and  Status = '02' ";
        }
        strSql += " order by ChangeDate desc";
        //HFD_ApplyQuery_Key.Value = tbxChangeDateS.Text + ";" + tbxChangeDateE.Text + ";" + rblValidStatus.SelectedValue + ";" + tbxRequesterID.Text;
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            //lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            //lblGridList.ForeColor = System.Drawing.Color.Red;
            ClientScript.RegisterStartupScript(this.GetType(), "s", "<script>alert('查無資料');</script>");
            lblGridList.Text = "";
            tbxRequesterID.Focus();
            
            return;
        }
        else
        {
            /*
            //2015/01/21 換掉原先的 Html Table
            string strBody = "<table class='table-list' width='100%'>";
            strBody += @"<TR>
                <TH width='10px'></TH>
                <TH width='130px'><SPAN>單號</SPAN></TH>
                <TH width='100px'><SPAN>異動日期</SPAN></TH>
                <TH width='50px'><SPAN>異動頻道</SPAN></TH>
                <TH noWrap><SPAN>異動內容</SPAN></TH>
                <TH width='100px'><SPAN>申請人</SPAN></TH>
                <TH width='100px'><SPAN>申請日期</SPAN></TH>
                <TH width='100px'><SPAN>審核人</SPAN></TH>
                <TH width='100px'><SPAN>審核日期</SPAN></TH>
                <TH width='100px'><SPAN>片庫作業同工</SPAN></TH>
                        </TR>";*/
            string strBody = "";
            foreach (DataRow dr in dt.Rows)
            {

                strBody += "<TR align='left' ondblclick =\"window.event.cancelBubble=true;window.open(\'" +
                    Util.RedirectByTime("ProgramChangeApplyContent.aspx", "ChangeID=" + dr["單號"].ToString() + "&Mode=" +
                    HFD_Mode.Value + "&ApplyQuery_Key=" + (tbxChangeDateS.Text + ";" + tbxChangeDateE.Text + ";" + 
                    rblValidStatus.SelectedValue + ";" + tbxRequesterID.Text)) + "\',\'_self\',\'\')\">";
                strBody += "<TD align='center' noWrap><img alt='' src='../img/" + dr["Image"].ToString() + "' title='" + dr["Status"].ToString() + "' />";
                strBody += "<TD noWrap><SPAN>" + dr["單號"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["異動日期"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["異動頻道"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["異動內容"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["申請人"].ToString() + " " + NpoDB.GetHRUser(dr["申請人"].ToString()) + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["申請日期"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["審核人"].ToString() + " " + NpoDB.GetHRUser(dr["審核人"].ToString()) + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["審核日期"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["片庫作業同工"].ToString() + " " + NpoDB.GetHRUser(dr["片庫作業同工"].ToString()) + "</SPAN></TD>";
                strBody += "</TR>";

            }

            //strBody += "</table>";
            lblGridList.Text = strBody;

            /*//Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            //npoGridView.Keys.Add("SerNo");
            //npoGridView.DisableColumn.Add("SerNo");
            //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = false; //不換頁
            //npoGridView.EditLink = Util.RedirectByTime("SRTStatusEdit.aspx", "SerNo=");
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;*/
        }
        count.Text = String.Format("{0:N0}", dt.Rows.Count);
        Session["strSql"] = strSql;
    }

}
