﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_ReleaseMasterQuery : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["customerName"] != null)
            {
                tbxCustomerName.Text = Session["customerName"].ToString();
            }
            if (tbxCustomerName.Text != "")
            {
                LoadFormData();
            }
        }
    }
    public void LoadFormData()
    {
        string strSql;
        DataTable dt;
        strSql = @"select customerID as '客戶代號', customerName as '客戶名稱', name_1 as '聯絡人姓名1' ,phone_1 as '聯絡人電話1' ,mobile_1 as '聯絡人手機1' ,email_1 as '聯絡人電郵1' ,
                    name_2 as '聯絡人姓名2' ,phone_2 as '聯絡人電話2' ,mobile_2 as '聯絡人手機2' ,email_2 as '聯絡人電郵2' ,interval as '供片間隔'
                    from dbo.ReleaseMaster where DeleteDatetime is Null ";

        Dictionary<string, object> dict = new Dictionary<string, object>();

        if (tbxCustomerName.Text.Trim() != "")
        {
            strSql += " and (customerID like N'%" + tbxCustomerName.Text.Trim() + "%' or customerName like N'%" + tbxCustomerName.Text.Trim() + "%' or name_1 like N'%" + tbxCustomerName.Text.Trim() + "%' or phone_1 like N'%" + tbxCustomerName.Text.Trim() + "%' or mobile_1 like N'%" + tbxCustomerName.Text.Trim() + "%' or email_1 like N'%" + tbxCustomerName.Text.Trim() + "%' ";
            strSql += " or name_2 like N'%" + tbxCustomerName.Text.Trim() + "%' or phone_2 like N'%" + tbxCustomerName.Text.Trim() + "%' or mobile_2 like N'%" + tbxCustomerName.Text.Trim() + "%' or email_2 like N'%" + tbxCustomerName.Text.Trim() + "%' or interval like N'%" + tbxCustomerName.Text.Trim() + "%')";
        }
        strSql += " order by customerID,customerName ";
        Session["customerName"] = tbxCustomerName.Text.Trim();
        dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count == 0)
        {
            lblGridList.Text = "** 沒有符合條件的資料 **";
            // 2014/4/9 有顏色區別
            lblGridList.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            //2015/04/21 換掉原先的 Html Table
            string strBody = "";
            foreach (DataRow dr in dt.Rows)
            {

                strBody += "<TR onclick =\"window.event.cancelBubble=true;window.open(\'" + Util.RedirectByTime("ReleaseMasterEdit.aspx", "customerID=" + dr["客戶代號"].ToString()) + "\',\'_self\',\'\')\">";
                strBody += "<TD noWrap><SPAN>" + dr["客戶代號"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["客戶名稱"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["聯絡人姓名1"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["聯絡人電話1"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["聯絡人手機1"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["聯絡人電郵1"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["聯絡人姓名2"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["聯絡人電話2"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["聯絡人手機2"].ToString() + "</SPAN></TD>";
                strBody += "<TD><SPAN>" + dr["聯絡人電郵2"].ToString() + "</SPAN></TD>";
                strBody += "<TD align='center'><SPAN>" + dr["供片間隔"].ToString() + "</SPAN></TD></TR>";
            }

            strBody += "</table>";
            lblGridList.Text = strBody;
            /*//Grid initial
            NPOGridView npoGridView = new NPOGridView();
            npoGridView.Source = NPOGridViewDataSource.fromDataTable;
            npoGridView.dataTable = dt;
            npoGridView.Keys.Add("customerID");
            npoGridView.DisableColumn.Add("customerID");
            //npoGridView.CurrentPage = Util.String2Number(HFD_CurrentPage.Value);
            npoGridView.ShowPage = false; //不換頁
            npoGridView.EditLink = Util.RedirectByTime("ReleaseMasterEdit.aspx", "customerID=");
            lblGridList.Text = npoGridView.Render();
            lblGridList.ForeColor = System.Drawing.Color.Black;*/
        }
        count.Text = String.Format("{0:N0}", dt.Rows.Count);
        Session["strSql"] = strSql;
    }
    //---------------------------------------------------------------------------
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LoadFormData();
    }
    //新增
    public void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Util.RedirectByTime("ReleaseMasterAdd.aspx"));
    }
}