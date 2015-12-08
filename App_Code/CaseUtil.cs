using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web;
using System.Web.SessionState;


/// <summary>
/// Summary description for CaseUtil
/// </summary>

public class CaseUtil
{
    public static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    public static HttpRequest Request { get { return HttpContext.Current.Request; } }
    public static HttpResponse Response { get { return HttpContext.Current.Response; } }
    public static HttpServerUtility Server { get { return HttpContext.Current.Server; } }
    public static Page page { get { return HttpContext.Current.CurrentHandler as Page; } }

    static List<CButton> ButtonList1 = new List<CButton>();
    static List<CButton> ButtonList2 = new List<CButton>();
    //-------------------------------------------------------------------------------------------------------------
    //public static bool CheckDate(string Date, int LimitDay)
    //{
    //    //�������ť�
    //    if (Date == "")
    //    {
    //        return false;
    //    }
    //    DateTime LimitStartDate = Util.GetStartDateOfMonth(DateTime.Now.AddMonths(-1));
    //    DateTime LimitEndDate = Util.GetEndDateOfMonth(DateTime.Now.AddMonths(-1));
    //    DateTime ServiceDate;
    //    bool flag = false;
    //    try
    //    {
    //        ServiceDate = Convert.ToDateTime(Date);
    //        //�������j�󤵤�
    //        if (ServiceDate > DateTime.Now)
    //        {
    //            return false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return false;
    //    }

    //    if (DateTime.Now.Day <= LimitDay)
    //    {
    //        //5�����e, �i�H���J�W�Ӥ몺���
    //        if (ServiceDate >= LimitStartDate)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    else
    //    {
    //        //�W�L5��, �h�����J�W�Ӥ몺���
    //        if (ServiceDate < LimitEndDate)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //}
    //-------------------------------------------------------------------------------------------------------------
    //���ͤW���ɮת� JavaScript �X
    public static string GetUploadJavaScriptCode()
    {
        string Script = "";
        Script += "function openUploadWindow(Ap_Name) {\n";
        Script += "var PersonProfile_Uid = document.getElementById('HFD_Uid').value;\n";
        Script += "window.open('../Common/UpLoad.aspx?PersonProfile_Uid=' + PersonProfile_Uid + '&Ap_Name=' + Ap_Name + '&AllowType=img', 'upload', 'scrollbars=no,status=no,toolbar=no,top=100,left=120,width=560,height=120');\n";
        Script += "}\n";
        return Script;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static List<CButton> GetButtonList(int MenuNo)
    {
        if (MenuNo == 1)
        {
            return ButtonList1;
        }
        else if (MenuNo == 2)
        {
            return ButtonList2;
        }
        return null;
    }
    //-------------------------------------------------------------------------
    public static string MakeMenu(string Donor_Id, int SelectedTab)
    {
        List<CButton> ButtonList = new List<CButton>();
        CButton Btn = new CButton();
        Btn.TabNo = 1;
        Btn.Text = "���ڤH���";
        Btn.Style = "style='width:24%'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "../DonorMgr/DonorInfo_Edit.aspx?Donor_Id=" + Donor_Id;
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "���ڰO��";
        Btn.Style = "style='width:24%'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "../DonateMgr/DonateDataList.aspx?Donor_Id=" + Donor_Id;
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 3;
        Btn.Text = "��b���v�ѰO��";
        Btn.Style = "style='width:24%'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "../DonateMgr/PledgeDataList.aspx?Donor_Id=" + Donor_Id;
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 4;
        Btn.Text = "�����ث~�O��";
        Btn.Style = "style='width:24%'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "../ContributeMgr/ContributeDataList.aspx?Donor_Id=" + Donor_Id;
        ButtonList.Add(Btn);

        return MenuList(ButtonList, SelectedTab);
    }
    //-------------------------------------------------------------------------
    public static string MakeMemberMenu(string Donor_Id, int SelectedTab)
    {
        List<CButton> ButtonList = new List<CButton>();
        CButton Btn = new CButton();
        Btn.TabNo = 1;
        Btn.Text = "Ū�̸��";
        Btn.Style = "style='width:100%'";  //�ݭn���P�e�׮�,�i�H�b���]�w�s��
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "Member_Edit.aspx?Donor_Id=" + Donor_Id;
        ButtonList.Add(Btn);

        return MenuList(ButtonList, SelectedTab);
    }
    //-------------------------------------------------------------------------
    public static string MenuList(List<CButton> ButtonList, int SelectedTab)
    {
        string s = "";
        foreach (CButton btn in ButtonList)
        {
            if (btn.TabNo == SelectedTab)
            {
                btn.CssClass = "tabSelected";
            }
            else
            {
                btn.CssClass = "tabNormal";
            }

            string disabled = "";
            if (btn.Disabled == true)
            {
                disabled = "disabled='disabled'";
            }
            //s += "<button type='button' " + btn.Style + " class='" + btn.CssClass + "' onclick=\"javascript:location.href='" + btn.OnClick + "' \"><img src='" + btn.ImgSrc + "' width='" + btn.Width + "' height='" + btn.Height + "' align='" + btn.Align + "' />" + btn.Text + "</button>\n";
            s += "<button type='button' title='" + btn.Title + "' " + disabled + " class='" + btn.CssClass + "' " + btn.Style + " onclick=\"javascript:location.href='" + btn.OnClick + "' \"><img src='" + btn.ImgSrc + "' width='" + btn.Width + "' height='" + btn.Height + "' align='" + btn.Align + "' />" + btn.Text + "</button>\n";
            if (btn.ShowBR == true)
            {
                s += "<br/>";
            }
        }
        return s;
    }
    //-------------------------------------------------------------------------------------------------------------} //end of class CaseUtil
    public static string GetUploadPic(string ObjectID, string ApName, Button UploadButton, Button DelButton, string Width, string Height)
    {
        if (Width == "" && Height == "")
        {
            Width = "width:180px";
            Height = "height:180px";
        }
        else if (Width == "0" && Height == "0")
        {
            Width = "";
            Height = "";
        }
        else
        {
            Width = "width:" + Width;
            Height = "height:" + Height;
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        //�a�X�������ɸ��
        string strSql = " select ApName, UploadFileURL from Upload\n";
        strSql += " where ObjectID=@ObjectID and ApName=@ApName\n";
        dict.Add("ObjectID", ObjectID);
        dict.Add("ApName", ApName);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        string PictURL = "";
        if (dt.Rows.Count != 0)
        {
            PictURL = dt.Rows[0]["UploadFileURL"].ToString(); ;
        }
        //�p�G�S��URL��(�N��L�W�ǹ���)�A�������ɧR�����s
        if (PictURL == "")
        {
            UploadButton.Visible = true;
            DelButton.Visible = false;
        }
        else
        {
            UploadButton.Visible = false;
            DelButton.Visible = true;
        }
        //�H��URL��ܹϤ�
        string RetStr;
        if (PictURL == "")
        {
            RetStr = "";
        }
        else
        {
            //RetStr = "<img src=\".." + PictURL + "\" border=\"0\" style=\"width:180pt;height:180pt;cursor:hand \" onclick=\"var x=window.open('','','height=480, width=640, toolbar=no, menubar=no, scrollbars=1, resizable=yes, location=no, status=no');x.document.write('<img src=.." + PictURL + " border=0>');\" alt=\"�I��ݩ�j��\">";
            RetStr = "<img src='.." + PictURL + "' border='0' style='" + Width + ";" + Height + ";cursor:hand' onclick=\"var x=window.open('','','height=480, width=640, toolbar=no, menubar=no, scrollbars=1, resizable=yes, location=no, status=no');x.document.write('<img src=.." + PictURL + " border=0>');\" alt=\"�I��ݩ�j��\">";
        }
        return RetStr;
    }
    //----------------------------------------------------------------------
    public static void DeletePic(string ObjectID, string ApName, string URL)
    {
        string strSql = @"
                  select uid, UploadFileURL from Upload
                  where ObjectID=@ObjectID and ApName=@ApName
              ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ObjectID", ObjectID);
        dict.Add("ApName", ApName); //�H���W�ٰϤ��O���@�ӵ{���W�Ǧ���
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count != 0)
        {
            strSql = "delete from Upload where uid=@uid";
            dict.Add("uid", dt.Rows[0]["uid"].ToString());
            NpoDB.ExecuteSQLS(strSql, dict);
            //�ɮפ@�֧R��
            string FilePath = Server.MapPath(".." + dt.Rows[0]["UploadFileURL"].ToString());
            File.Delete(FilePath);
        }
        Session["Msg"] = "���ɧR�����\!";
        Response.Redirect(URL);
    }
    //----------------------------------------------------------------------
    public static void FillDept(DropDownList ddl)
    {
        string strSql = @"
                         select DeptID, DeptName
                         from Dept where DeptType='1'
                         order by DeptID
                       ";
        Util.FillDropDownList(ddl, strSql, "DeptName", "DeptID", true);
    }
    //----------------------------------------------------------------------
    public static bool IsAllDept()
    {
        if ((page as BasePage).SessionInfo.GroupArea == "����")
        {
            return true;
        }
        return false;
    }
    //----------------------------------------------------------------------
    //�C�L�ɶǦ^ img �X
    public static string GetUploadPic(string ObjectID, string ApName, string Width, string Height)
    {
        if (Width == "" && Height == "")
        {
            Width = "width:180px";
            Height = "height:180px";
        }
        else if (Width == "0" && Height == "0")
        {
            Width = "";
            Height = "";
        }
        else
        {
            Width = "width:" + Width;
            Height = "height:" + Height;
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        //�a�X�������ɸ��
        string strSql = " select ApName, UploadFileURL from Upload\n";
        strSql += " where ObjectID=@ObjectID and ApName=@ApName\n";
        dict.Add("ObjectID", ObjectID);
        dict.Add("ApName", ApName);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        string PictURL = "";
        if (dt.Rows.Count != 0)
        {
            PictURL = dt.Rows[0]["UploadFileURL"].ToString(); ;
        }
        //�H��URL��ܹϤ�
        string RetStr;
        if (PictURL == "")
        {
            RetStr = "";
        }
        else
        {
            RetStr = "<img src='.." + PictURL + "' border='0' style='" + Width + ";" + Height + "'";
        }
        return RetStr;
    }
    //----------------------------------------------------------------------
    public static string GetDeptNameByDeptID(string DeptID)
    {
        string DeptName = "";
        string strSql = @"
                          select DeptName
                          from Dept
                          where DeptID=@DeptID
                         ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("DeptID", DeptID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count != 0)
        {
            DeptName = dt.Rows[0]["DeptName"].ToString();
        }
        return DeptName;
    }
    //---------------------------------------------------------------------------
    //���ڤH�򥻸�ƺ��@
    public static DataTable DonorInfo_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�s���q��");
        dtRet.Columns.Add("������X");
        dtRet.Columns.Add("�q�T�a�}");
        //20140425 �ק� by Ian_Kao �վ��X���
        //dtRet.Columns.Add("������");
        //dtRet.Columns.Add("������");
        //dtRet.Columns.Add("���ڦ���");
        //dtRet.Columns.Add("�֭p���ڪ��B");
        dtRet.Columns.Add("��Z");
        dtRet.Columns.Add("DVD");
        dtRet.Columns.Add("�q�l<br>���");
        dtRet.Columns.Add("�ͤ�d");
        dtRet.Columns.Add("�̪񮽴�<br>���");


        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();

            dr["�s��"] = dtSrc.Rows[i][1].ToString();
            dr["���ڤH"] = dtSrc.Rows[i][2].ToString();
            dr["�����O"] = dtSrc.Rows[i][4].ToString();
            dr["�s���q��"] = dtSrc.Rows[i][5].ToString();
            dr["������X"] = dtSrc.Rows[i][6].ToString();
            dr["�q�T�a�}"] = dtSrc.Rows[i][7].ToString();
            //20140425 �ק� by Ian_Kao �վ��X���
            //dr["������"] = dtSrc.Rows[i][8].ToString();
            //dr["������"] = dtSrc.Rows[i][9].ToString();
            //dr["���ڦ���"] = dtSrc.Rows[i][10].ToString();
            //dr["�֭p���ڪ��B"] = dtSrc.Rows[i][11].ToString();
            dr["��Z"] = dtSrc.Rows[i][9].ToString();
            dr["DVD"] = dtSrc.Rows[i][10].ToString();
            dr["�q�l<br>���"] = dtSrc.Rows[i][11].ToString();
            dr["�ͤ�d"] = dtSrc.Rows[i][12].ToString();
            dr["�̪񮽴�<br>���"] = dtSrc.Rows[i][8].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //-------------------------------------------------------------------------------------------------------------
    //���ڤH�򥻸��_����W��
    public static DataTable DonorInfo_Excel_Phone(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�s���q��");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["�s��"] = dtSrc.Rows[i][1].ToString();
            dr["���ڤH"] = dtSrc.Rows[i][2].ToString();
            dr["�s���q��"] = dtSrc.Rows[i][3].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //-------------------------------------------------------------------------------------------------------------
    //���ڤH�򥻸��_Email�W��
    public static DataTable DonorInfo_Excel__Email(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        //20140509 �ק� by Ian_Kao �榡���~
        //dtRet.Columns.Add("�s��");
        //dtRet.Columns.Add("���ڤH");
        //dtRet.Columns.Add("�q�l�H�c");
        dtRet.Columns.Add("Email");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //20140509 �ק� by Ian_Kao �榡���~
            //dr["�s��"] = dtSrc.Rows[i][1].ToString();
            //dr["���ڤH"] = dtSrc.Rows[i][2].ToString();
            //dr["�q�l�H�c"] = dtSrc.Rows[i][3].ToString();
            dr["Email"] = dtSrc.Rows[i][0].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���ڤH�W�U
    public static DataTable DonorQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        // 2014/4/8 ��ܮ��ڤH�s��
        //dtRet.Columns.Add("�s��");
        dtRet.Columns.Add("���ڤH�s��");

        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�ʧO");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�s���q�ܤ�");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�s���q�ܩ]");
        dtRet.Columns.Add("�q�l�H�c");
        dtRet.Columns.Add("�q�T�a�}");      
        dtRet.Columns.Add("�̪񮽴ڤ��");
        dtRet.Columns.Add("�ֿn����");
        dtRet.Columns.Add("�ֿn���B");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            
            // 2014/4/8 ��ܮ��ڤH�s��
            dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();

            dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
            dr["�ʧO"] = dtSrc.Rows[i][2].ToString();
            dr["�����O"] = dtSrc.Rows[i][3].ToString();
            dr["�s���q�ܤ�"] = dtSrc.Rows[i][4].ToString();
            dr["���"] = dtSrc.Rows[i][5].ToString();
            dr["�s���q�ܩ]"] = dtSrc.Rows[i][6].ToString();
            dr["�q�l�H�c"] = dtSrc.Rows[i][7].ToString();
            dr["�q�T�a�}"] = dtSrc.Rows[i][8].ToString();
            dr["�̪񮽴ڤ��"] = dtSrc.Rows[i][9].ToString();
            dr["�ֿn����"] = dtSrc.Rows[i][10].ToString();
            dr["�ֿn���B"] = dtSrc.Rows[i][11].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���ڤH�W�U_�ץX
    public static DataTable DonorQry_Print_Excel(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        // 2014/4/8 ��ܮ��ڤH�s��
        dtRet.Columns.Add("���ڤH�s��");

        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�ʧO");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�����Ҳνs");
        dtRet.Columns.Add("�X�ͤ��");
        dtRet.Columns.Add("�Ш|�{��");////
        dtRet.Columns.Add("¾�~�O");////
        dtRet.Columns.Add("�B�ê��p");////
        dtRet.Columns.Add("�v�ЫH��");////
        dtRet.Columns.Add("�D���з|�W��");////
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�q�ܤ�");       
        dtRet.Columns.Add("�q�ܩ]");
        dtRet.Columns.Add("�q�l�H�c");
        dtRet.Columns.Add("�p���H");
        dtRet.Columns.Add("�A�ȳ��");
        dtRet.Columns.Add("¾��");
        dtRet.Columns.Add("�q�T�a�}");
        dtRet.Columns.Add("���~�a�}");
        dtRet.Columns.Add("�ȥ���Z");
        dtRet.Columns.Add("DVD");
        dtRet.Columns.Add("�q�l���");
        dtRet.Columns.Add("���ڶ}��");
        dtRet.Columns.Add("���ک��Y");
        dtRet.Columns.Add("���ڨ����Ҳνs");
        dtRet.Columns.Add("�������ڤ��");
        dtRet.Columns.Add("�̪񮽴ڤ��");
        dtRet.Columns.Add("�ֿn����");
        dtRet.Columns.Add("�ֿn���B");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();

            // 2014/4/8 ��ܮ��ڤH�s��
            dr["���ڤH�s��"] = dtSrc.Rows[i]["Donor_id"].ToString();

            dr["���ڤH"] = dtSrc.Rows[i]["Donor_Name"].ToString();
            dr["�ʧO"] = dtSrc.Rows[i]["Sex"].ToString();
            dr["�ٿ�"] = dtSrc.Rows[i]["Title"].ToString();
            dr["�����O"] = dtSrc.Rows[i]["Donor_Type"].ToString();
            dr["�����Ҳνs"] = dtSrc.Rows[i]["IDNo"].ToString();
            dr["�X�ͤ��"] = dtSrc.Rows[i]["Birthday"].ToString() != "" ? DateTime.Parse(dtSrc.Rows[i]["Birthday"].ToString().Trim()).ToShortDateString().ToString() : "";
            dr["�Ш|�{��"] = dtSrc.Rows[i]["Education"].ToString();
            dr["¾�~�O"] = dtSrc.Rows[i]["Occupation"].ToString();
            dr["�B�ê��p"] = dtSrc.Rows[i]["Marriage"].ToString();
            dr["�v�ЫH��"] = dtSrc.Rows[i]["Religion"].ToString();
            dr["�D���з|�W��"] = dtSrc.Rows[i]["ReligionName"].ToString();
            dr["���"] = dtSrc.Rows[i]["Cellular_Phone"].ToString();
            dr["�q�ܤ�"] = dtSrc.Rows[i]["Tel_Office"].ToString();
            dr["�q�ܩ]"] = dtSrc.Rows[i]["Tel_Home"].ToString();
            dr["�q�l�H�c"] = dtSrc.Rows[i]["Email"].ToString();
            dr["�p���H"] = dtSrc.Rows[i]["Contactor"].ToString();
            dr["�A�ȳ��"] = dtSrc.Rows[i]["OrgName"].ToString();
            dr["¾��"] = dtSrc.Rows[i]["JobTitle"].ToString();
            dr["�q�T�a�}"] = dtSrc.Rows[i]["�a�}"].ToString();
            dr["���~�a�}"] = dtSrc.Rows[i]["IsAbroad"].ToString();
            dr["�ȥ���Z"] = dtSrc.Rows[i]["IsSendNews"].ToString();
            dr["DVD"] = dtSrc.Rows[i]["IsDVD"].ToString();
            dr["�q�l���"] = dtSrc.Rows[i]["IsSendEpaper"].ToString();
            dr["���ڶ}��"] = dtSrc.Rows[i]["Invoice_Type"].ToString();
            dr["���ک��Y"] = dtSrc.Rows[i]["Invoice_Title"].ToString();
            dr["���ڨ����Ҳνs"] = dtSrc.Rows[i]["Invoice_IDNo"].ToString();
            if (dtSrc.Rows[i]["Begin_DonateDate"].ToString().Trim() != "")
            {
                dr["�������ڤ��"] = DateTime.Parse(dtSrc.Rows[i]["Begin_DonateDate"].ToString().Trim()).ToShortDateString().ToString();
            }
            else
            {
                dr["�������ڤ��"] = "";
            }
            if (dtSrc.Rows[i]["Last_DonateDate"].ToString().Trim() != "")
            {
                dr["�̪񮽴ڤ��"] = DateTime.Parse(dtSrc.Rows[i]["Last_DonateDate"].ToString().Trim()).ToShortDateString().ToString();
            }
            else
            {
                dr["�̪񮽴ڤ��"] = "";
            }
            dr["�ֿn����"] = dtSrc.Rows[i]["Donate_No"].ToString();
            dr["�ֿn���B"] = Util.ToThree(dtSrc.Rows[i]["Donate_Total"].ToString(), "0");//dtSrc.Rows[i]["Donate_Total"].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���ڤH�έp
    public static DataTable DonorReport_Condition(DataTable dtSrc, String Condition)
    {
        DataTable dtRet = new DataTable();
        if (Condition == "1")
        {
            dtRet.Columns.Add("���O�έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");

            string[] cla = { "�ǥ�", "�ӤH", "����", "���O����", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["���O�έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "2")
        {
            dtRet.Columns.Add("�ʧO�έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "�k", "�k", "�\", "�ʧO����", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�ʧO�έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "3")
        {
            dtRet.Columns.Add("�~�ֲέp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "20���H�U", "21 �� 25��", "26 �� 30��", "31 �� 35��", "36 �� 40��", "41 �� 45��", "46 �� 50��", "51 �� 55��", "56 �� 60��", "61 �� 65��", "66 �� 70��", "71���H�W", "�~�֤���", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�~�ֲέp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "4")
        {
            dtRet.Columns.Add("�Ш|�{�ײέp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "���Ѧr", "��p", "�ꤤ", "����", "�j��", "�Ӥh", "�դh", "�դh���s", "�Ш|�{�פ���", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�Ш|�{�ײέp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "5")
        {
            dtRet.Columns.Add("¾�~�O�έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "����", "�xĵ", "�ǥ�", "�A", "��", "��", "�a��", "�A��", "�ۥ�", "���@", "�h��", "��L", "¾�~�O����", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["¾�~�O�έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "6")
        {
            dtRet.Columns.Add("�B�ê��p�έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "���B", "�w�B", "���~", "���B", "�స", "��L", "�B�ê��p����", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�B�ê��p�έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "7")
        {
            dtRet.Columns.Add("�v�ЫH���έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "�����", "�v�ЫH������", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�v�ЫH���έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "8")
        {
            dtRet.Columns.Add("�q�T�����έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "�򶩥�", "�x�_��", "�s�_��", "��鿤", "�s�˥�", "�s�˿�", "�]�߿�", "�x����", "���ƿ�", "�n�뿤", "���L��", "�Ÿq��", "�Ÿq��", "�x�n��", "������", "�̪F��", "�y����", "�Ὤ��", "�x�F��", "���", "������", "�s����", "�q�T��������", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�q�T�����έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "9")
        {
            dtRet.Columns.Add("���ڿ����έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "�򶩥�", "�x�_��", "�s�_��", "��鿤", "�s�˥�", "�s�˿�", "�]�߿�", "�x����", "���ƿ�", "�n�뿤", "���L��", "�Ÿq��", "�Ÿq��", "�x�n��", "������", "�̪F��", "�y����", "�Ὤ��", "�x�F��", "���", "������", "�s����", "�q�T��������", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["���ڿ����έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        return dtRet;
    }
    //-------------------------------------------------------------------------
    //���oLinked����Gift�̤p��id
    public static string GetOldMin_Gift_Linked_Id()
    {
        string strRet = "";

        string strSql = @"select isnull(Min(Linked_Id),'') as Linked_Id from Linked ";
        strSql += " where Linked_Type='gift'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            string Linked_Id = dt.Rows[0]["Linked_Id"].ToString();
            if (Linked_Id == "0")
            {
                ///�s�W�������M�᭫�s�ɤJ
                Session["Msg"] = "�L��ơA�Х��s�W�����ث~�~���I";
                Session["Linked_Id"] = "0";
                Response.Redirect(Util.RedirectByTime("LinkedList_Add.aspx"));
            }
            else
            {
                strRet = Convert.ToInt32(Linked_Id).ToString();
            }
        }

        return strRet;
    }
    //-------------------------------------------------------------------------
    //���oLinked����Contribute�̤p��id
    public static string GetOldMin_Good_Linked_Id()
    {
        string strRet = "";

        string strSql = @"select isnull(Min(Linked_Id),'') as Linked_Id from Linked ";
        strSql += " where Linked_Type='contribute'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            string Linked_Id = dt.Rows[0]["Linked_Id"].ToString();
            if (Linked_Id == "0")
            {
                ///�s�W�������M�᭫�s�ɤJ
                Session["Msg"] = "�L��ơA�Х��s�W���~���O�I";
                Session["Linked_Id"] = "0";
                Response.Redirect(Util.RedirectByTime("GoodList_Add.aspx"));
            }
            else
            {
                strRet = Convert.ToInt32(Linked_Id).ToString();
            }
        }

        return strRet;
    }
    //-------------------------------------------------------------------------
    //���oLinked��Gift���̤j�Ǹ�
    public static string GetNewMaxLinked_Seq()
    {
        string strRet = "";

        string strSql = @"select isnull(MAX(Linked_Seq),'') as Linked_Seq from Linked  where Linked_Type='gift'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            string Linked_Seq = dt.Rows[0]["Linked_Seq"].ToString();
            if (Linked_Seq == "")
            {
                strRet = "1";
            }
            else
            {
                strRet = (Convert.ToInt32(Linked_Seq) + 1).ToString("0");
            }
        }

        return strRet;
    }
    //-------------------------------------------------------------------------
    //���oLinked��Contribute���̤j�Ǹ�
    public static string GetNewMaxLinked2_Seq()
    {
        string strRet = "";

        string strSql = @"select isnull(MAX(Linked_Seq),'') as Linked_Seq from Linked  where Linked_Type='Contribute'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            string Linked_Seq = dt.Rows[0]["Linked_Seq"].ToString();
            if (Linked_Seq == "")
            {
                strRet = "1";
            }
            else
            {
                strRet = (Convert.ToInt32(Linked_Seq) + 1).ToString("0");
            }
        }

        return strRet;
    }
    //-------------------------------------------------------------------------
    //���oLinked2���̤j�Ǹ�
    public static string GetNewMaxLinked2_Seq(String Linked_Id)
    {
        string strRet = "";

        string strSql = @"select isnull(MAX(Linked2_Seq),'') as Linked2_Seq from Linked2 ";
        strSql += " where Linked_Id='" + Linked_Id + "'";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);
        if (dt.Rows.Count > 0)
        {
            string Linked_Seq2 = dt.Rows[0]["Linked2_Seq"].ToString();
            if (Linked_Seq2 == "")
            {
                strRet = "1";
            }
            else
            {
                strRet = (Convert.ToInt32(Linked_Seq2) + 1).ToString("0");
            }
        }

        return strRet;
    }
    //---------------------------------------------------------------------------
    //���ں��@����
    public static DataTable DonateInfo_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("���ک��Y");
        dtRet.Columns.Add("���ڤ��");
        dtRet.Columns.Add("���ڪ��B");
        dtRet.Columns.Add("���ڤ覡");
        dtRet.Columns.Add("���ڥγ~");
        dtRet.Columns.Add("�U�Ҭ���");
        dtRet.Columns.Add("���ڶ}��");
        dtRet.Columns.Add("���ڽs��");
        dtRet.Columns.Add("�R�b���");
        dtRet.Columns.Add("�C�L");
        dtRet.Columns.Add("���A");
        dtRet.Columns.Add("�g��H");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["�s��"] = dtSrc.Rows[i][1].ToString();
            dr["���ڤH"] = dtSrc.Rows[i][2].ToString();
            dr["���ک��Y"] = dtSrc.Rows[i][3].ToString();
            dr["���ڤ��"] = dtSrc.Rows[i][4].ToString();
            dr["���ڪ��B"] = dtSrc.Rows[i][5].ToString();
            dr["���ڤ覡"] = dtSrc.Rows[i][6].ToString();
            dr["���ڥγ~"] = dtSrc.Rows[i][7].ToString();
            dr["���ڶ}��"] = dtSrc.Rows[i][8].ToString();
            dr["���ڶ}��"] = dtSrc.Rows[i][9].ToString();
            dr["���ڽs��"] = dtSrc.Rows[i][10].ToString();
            dr["�R�b���"] = dtSrc.Rows[i][11].ToString();
            if (dtSrc.Rows[i][11].ToString() != "" && DateTime.Parse(dtSrc.Rows[i][11].ToString()).ToString("yyyy/MM/dd") != "1900/01/01")
            {
                dr["�R�b���"] = dtSrc.Rows[i][11].ToString();
            }
            else
            {
                dr["�R�b���"] = "";
            }
            dr["�C�L"] = dtSrc.Rows[i][12].ToString();
            dr["���A"] = dtSrc.Rows[i][13].ToString();
            dr["�g��H"] = dtSrc.Rows[i][14].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�T�w��b���v��
    public static DataTable PledgeQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���v�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���v�覡");
        dtRet.Columns.Add("���ڪ��B");
        dtRet.Columns.Add("���v�_��");
        dtRet.Columns.Add("���v����");
        dtRet.Columns.Add("��b�g��");
        dtRet.Columns.Add("�U�����ڤ��");
        dtRet.Columns.Add("�o�d�Ȧ�-�d�O");
        dtRet.Columns.Add("�d��/�b��");
        dtRet.Columns.Add("�H�Υd���Ĥ�~");
        dtRet.Columns.Add("���v���A");
        dtRet.Columns.Add("�g��H");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���v�s��"] = dtSrc.Rows[i][0].ToString();
            dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
            dr["���ڤH�s��"] = dtSrc.Rows[i][2].ToString();
            dr["���v�覡"] = dtSrc.Rows[i][3].ToString();
            dr["���ڪ��B"] = dtSrc.Rows[i][4].ToString();
            dr["���v�_��"] = dtSrc.Rows[i][5].ToString();
            dr["���v����"] = dtSrc.Rows[i][6].ToString();
            dr["��b�g��"] = dtSrc.Rows[i][7].ToString();
            dr["�U�����ڤ��"] = dtSrc.Rows[i][8].ToString();
            dr["�o�d�Ȧ�-�d�O"] = dtSrc.Rows[i][9].ToString();
            dr["�d��/�b��"] = dtSrc.Rows[i][10].ToString();
            dr["�H�Υd���Ĥ�~"] = dtSrc.Rows[i][11].ToString();
            dr["���v���A"] = dtSrc.Rows[i][12].ToString();
            dr["�g��H"] = dtSrc.Rows[i][13].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�T�w��b���v��_�ץX
    public static DataTable PledgeQry_Print_Excel(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���v�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���v�覡");
        dtRet.Columns.Add("���w�γ~");
        dtRet.Columns.Add("���ڪ��B");
        dtRet.Columns.Add("���v�_��");
        dtRet.Columns.Add("���v����");
        dtRet.Columns.Add("��b�g��");
        dtRet.Columns.Add("�U�����ڤ��");
        dtRet.Columns.Add("�o�d�Ȧ�-�d�O");
        dtRet.Columns.Add("�d��/�b��");
        dtRet.Columns.Add("�H�Υd���Ĥ�~");
        dtRet.Columns.Add("���v���A");
        dtRet.Columns.Add("�g��H");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���v�s��"] = dtSrc.Rows[i][0].ToString();
            dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
            dr["���ڤH�s��"] = dtSrc.Rows[i][2].ToString();
            dr["���v�覡"] = dtSrc.Rows[i][3].ToString();
            dr["���w�γ~"] = dtSrc.Rows[i][4].ToString();
            dr["���ڪ��B"] = dtSrc.Rows[i][5].ToString();
            dr["���v�_��"] = dtSrc.Rows[i][6].ToString();
            dr["���v����"] = dtSrc.Rows[i][7].ToString();
            dr["��b�g��"] = dtSrc.Rows[i][8].ToString();
            dr["�U�����ڤ��"] = dtSrc.Rows[i][9].ToString();
            dr["�o�d�Ȧ�-�d�O"] = dtSrc.Rows[i][10].ToString();
            dr["�d��/�b��"] = dtSrc.Rows[i][11].ToString();
            dr["�H�Υd���Ĥ�~"] = dtSrc.Rows[i][12].ToString();
            dr["���v���A"] = dtSrc.Rows[i][13].ToString();
            dr["�g��H"] = dtSrc.Rows[i][14].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�^�m�x�H��-style1
    public static DataTable VerifyList_Print_style1(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();
           
        dtRet.Columns.Add("���ڤ��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("���ڪ��B");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���ڤ��"] = dtSrc.Rows[i][0].ToString();
            dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
            dr["���ڪ��B"] = dtSrc.Rows[i][2].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�^�m�x�H��-style2
    public static DataTable VerifyList_Print_style2(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڪ��B");
        dtRet.Columns.Add("���ڤH");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���ڪ��B"] = dtSrc.Rows[i][0].ToString();
            dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�^�m�x�H��-style3
    public static DataTable VerifyList_Print_style3(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�Ǹ�");
        dtRet.Columns.Add("���ڥγ~");
        dtRet.Columns.Add("���ڤ��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("���ڪ��B");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["�Ǹ�"] = dtSrc.Rows[i][1].ToString();
            dr["���ڥγ~"] = dtSrc.Rows[i][2].ToString();
            dr["���ڤ��"] = dtSrc.Rows[i][3].ToString();
            dr["���ڤH"] = dtSrc.Rows[i][4].ToString();
            dr["���ڪ��B"] = dtSrc.Rows[i][5].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���ڦW��
    public static DataTable DonateNameList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("���ڤ��");
        dtRet.Columns.Add("���ڪ��B");
        dtRet.Columns.Add("���ڤ覡");
        dtRet.Columns.Add("���ڥγ~");
        dtRet.Columns.Add("���ڶ}��");
        dtRet.Columns.Add("���ڽs��");
        dtRet.Columns.Add("�R�b���");
        dtRet.Columns.Add("�������O");
        dtRet.Columns.Add("�J�b�Ȧ�");
        dtRet.Columns.Add("�|�p���");
        dtRet.Columns.Add("���ʦW��");
        dtRet.Columns.Add("���A");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���ڤH"] = dtSrc.Rows[i][0].ToString();
            dr["���ڤ��"] = dtSrc.Rows[i][1].ToString();
            dr["���ڪ��B"] = dtSrc.Rows[i][2].ToString();
            dr["���ڤ覡"] = dtSrc.Rows[i][3].ToString();
            dr["���ڥγ~"] = dtSrc.Rows[i][4].ToString();
            dr["���ڶ}��"] = dtSrc.Rows[i][5].ToString();
            dr["���ڽs��"] = dtSrc.Rows[i][6].ToString();
            dr["�R�b���"] = dtSrc.Rows[i][7].ToString();
            dr["�������O"] = dtSrc.Rows[i][8].ToString();
            dr["�J�b�Ȧ�"] = dtSrc.Rows[i][9].ToString();
            dr["�|�p���"] = dtSrc.Rows[i][10].ToString();
            dr["���ʦW��"] = dtSrc.Rows[i][11].ToString();
            dr["���A"] = dtSrc.Rows[i][12].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���ڳ���
    public static DataTable DonateMonthReport_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�Ǹ�");
        dtRet.Columns.Add("���ڽs��");
        dtRet.Columns.Add("���ڤ��");
        dtRet.Columns.Add("���ڤH(�s��)");
        dtRet.Columns.Add("���ڪ��B");
        dtRet.Columns.Add("���ڥγ~");
        dtRet.Columns.Add("���ڤ覡");


        int row = dtSrc.Rows.Count;

        for (int i = 0; i <= row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row)
            {
                dr["�Ǹ�"] = "������G&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                dr["���ڽs��"] = "�ư�����G&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                dr["���ڤ��"] = "�����D�ޡG&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                dr["���ڤH(�s��)"] = "";
                dr["���ڪ��B"] = "�s��G&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                dr["���ڥγ~"] = "";
                dr["���ڤ覡"] = "�|�p�G&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }
            //�˼ƲĤG�C
            else if (i == row - 1)
            {
                dr["�Ǹ�"] = dtSrc.Rows[i][1].ToString();
                dr["���ڽs��"] = dtSrc.Rows[i][2].ToString();
                dr["���ڤ��"] = dtSrc.Rows[i][3].ToString();
                dr["���ڤH(�s��)"] = "���ڪ��B�G";
                dr["���ڪ��B"] = dtSrc.Rows[i][5].ToString() != "" ? dtSrc.Rows[i][5].ToString() + "" : "0";
                dr["���ڥγ~"] = "";
                dr["���ڤ覡"] = "";
            }
            else
            {
                dr["�Ǹ�"] = dtSrc.Rows[i][0].ToString();
                dr["���ڽs��"] = dtSrc.Rows[i][8].ToString() + dtSrc.Rows[i][1].ToString();
                dr["���ڤ��"] = dtSrc.Rows[i][2].ToString();
                dr["���ڤH(�s��)"] = dtSrc.Rows[i][3].ToString() + "(" + Convert.ToInt32(dtSrc.Rows[i][4].ToString().Substring(0)).ToString("00000") + ")";
                dr["���ڪ��B"] = dtSrc.Rows[i][5].ToString() != "" ? dtSrc.Rows[i][5].ToString() + "" : "0";
                dr["���ڥγ~"] = dtSrc.Rows[i][6].ToString();
                dr["���ڤ覡"] = dtSrc.Rows[i][7].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�|�p���
    public static DataTable DonateAccounReport_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("&nbsp;&nbsp;�|�p���");
        dtRet.Columns.Add("���ڪ��B");
        dtRet.Columns.Add("����O");
        dtRet.Columns.Add("�ꦬ���B");
        dtRet.Columns.Add("���ڵ���");

        int col = dtSrc.Columns.Count;
        for (int i = 0; i < col; i+=5)
        {
            DataRow dr = dtRet.NewRow();
            if (i + 4 == col-1)
            {
                dr["&nbsp;&nbsp;�|�p���"] = "&nbsp;&nbsp;�C�L����G" + dtSrc.Rows[0][i].ToString()+"      &nbsp;&nbsp;&nbsp;�`�p�G";
                dr["���ڪ��B"] = dtSrc.Rows[0][i + 1].ToString() != "" ? dtSrc.Rows[0][i + 1].ToString() + "��" : "0��";
                dr["����O"] = dtSrc.Rows[0][i + 2].ToString() != "" ? dtSrc.Rows[0][i + 2].ToString() + "��" : "0��";
                dr["�ꦬ���B"] = dtSrc.Rows[0][i + 3].ToString() != "" ? dtSrc.Rows[0][i + 3].ToString() + "��" : "0��";
                dr["���ڵ���"] = dtSrc.Rows[0][i + 4].ToString();
            }
            else
            { 
                dr["&nbsp;&nbsp;�|�p���"] = "&nbsp;&nbsp;" + dtSrc.Rows[0][i].ToString();
                dr["���ڪ��B"] = dtSrc.Rows[0][i + 1].ToString() != "" ? dtSrc.Rows[0][i + 1].ToString() + "��" : "0��";
                dr["����O"] = dtSrc.Rows[0][i + 2].ToString() != "" ? dtSrc.Rows[0][i + 2].ToString() + "��" : "0��";
                dr["�ꦬ���B"] = dtSrc.Rows[0][i + 3].ToString() != "" ? dtSrc.Rows[0][i + 3].ToString() + "��" : "0��";
                dr["���ڵ���"] = dtSrc.Rows[0][i + 4].ToString() + "��";
            }

            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�ꪫ�^�m���غ��@
    public static DataTable ContributeList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ؤH");
        dtRet.Columns.Add("���ڤ��");
        dtRet.Columns.Add("��X�{��");
        dtRet.Columns.Add("���ؤ��e");
        dtRet.Columns.Add("���ڤ覡");
        dtRet.Columns.Add("���ڥγ~");
        dtRet.Columns.Add("���ڶ}��");
        dtRet.Columns.Add("���ڽs��");
        dtRet.Columns.Add("�C�L");
        dtRet.Columns.Add("���A");
        dtRet.Columns.Add("�g��H");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���ؤH"] = dtSrc.Rows[i][2].ToString();
            dr["���ڤ��"] = dtSrc.Rows[i][3].ToString();
            dr["��X�{��"] = dtSrc.Rows[i][4].ToString();
            dr["���ؤ��e"] = dtSrc.Rows[i][12].ToString();
            dr["���ڤ覡"] = dtSrc.Rows[i][5].ToString();
            dr["���ڥγ~"] = dtSrc.Rows[i][6].ToString();
            dr["���ڶ}��"] = dtSrc.Rows[i][7].ToString();
            dr["���ڽs��"] = dtSrc.Rows[i][8].ToString();
            dr["�C�L"] = dtSrc.Rows[i][9].ToString();
            dr["���A"] = dtSrc.Rows[i][10].ToString();
            dr["�g��H"] = dtSrc.Rows[i][11].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�ꪫ�^�m��Χ@�~
    public static DataTable ContributeIssueList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("����H");
        dtRet.Columns.Add("������");
        dtRet.Columns.Add("����γ~");
        dtRet.Columns.Add("����s��");
        dtRet.Columns.Add("��Τ��e");
        dtRet.Columns.Add("�C�L");
        dtRet.Columns.Add("���A");
        dtRet.Columns.Add("�g��H");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["����H"] = dtSrc.Rows[i][1].ToString();
            dr["������"] = dtSrc.Rows[i][2].ToString();
            dr["����γ~"] = dtSrc.Rows[i][3].ToString();
            dr["����s��"] = dtSrc.Rows[i][4].ToString();
            dr["��Τ��e"] = dtSrc.Rows[i][8].ToString();
            dr["�C�L"] = dtSrc.Rows[i][5].ToString();
            dr["���A"] = dtSrc.Rows[i][6].ToString();
            dr["�g��H"] = dtSrc.Rows[i][7].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���~��ΦC�L
    public static DataTable ContributeIssue_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���~�N��");
        dtRet.Columns.Add("���~�W��");
        dtRet.Columns.Add("�ƶq���");
        dtRet.Columns.Add("�Ƶ�");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���~�N��"] = dtSrc.Rows[i][0].ToString();
            dr["���~�W��"] = dtSrc.Rows[i][1].ToString();
            dr["�ƶq���"] = dtSrc.Rows[i][2].ToString();
            dr["�Ƶ�"] = dtSrc.Rows[i][3].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�H�Υd����d��
    public static DataTable EcBankCardQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("����覡");
        dtRet.Columns.Add("������");
        dtRet.Columns.Add("����Ǹ�");
        dtRet.Columns.Add("������B");
        dtRet.Columns.Add("������A");
        dtRet.Columns.Add("���v���A");
        dtRet.Columns.Add("���v�X");
        dtRet.Columns.Add("�O�_�д�");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�p���q��");
        dtRet.Columns.Add("�q�l�l��");
        dtRet.Columns.Add("���ڥγ~");
        dtRet.Columns.Add("���ڶ}��");
        dtRet.Columns.Add("���ک��Y");
        dtRet.Columns.Add("���ڦa�}");
        dtRet.Columns.Add("�q�T�a�}");
        dtRet.Columns.Add("�ʧO");
        dtRet.Columns.Add("������");
        dtRet.Columns.Add("�X�ͤ��");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
            dr["����覡"] = dtSrc.Rows[i][2].ToString();
            dr["������"] = dtSrc.Rows[i][3].ToString();
            dr["����Ǹ�"] = dtSrc.Rows[i][4].ToString();
            dr["������B"] = dtSrc.Rows[i][5].ToString();
            dr["������A"] = dtSrc.Rows[i][6].ToString();
            dr["���v���A"] = dtSrc.Rows[i][7].ToString();
            dr["���v�X"] = dtSrc.Rows[i][8].ToString();
            dr["�O�_�д�"] = dtSrc.Rows[i][9].ToString();
            dr["���"] = dtSrc.Rows[i][10].ToString();
            dr["�p���q��"] = dtSrc.Rows[i][11].ToString();
            dr["�q�l�l��"] = dtSrc.Rows[i][12].ToString();
            dr["���ڥγ~"] = dtSrc.Rows[i][13].ToString();
            dr["���ڶ}��"] = dtSrc.Rows[i][14].ToString();
            dr["���ک��Y"] = dtSrc.Rows[i][15].ToString();
            dr["���ڦa�}"] = dtSrc.Rows[i][16].ToString();
            dr["�q�T�a�}"] = dtSrc.Rows[i][17].ToString();
            dr["�ʧO"] = dtSrc.Rows[i][18].ToString();
            dr["������"] = dtSrc.Rows[i][19].ToString();
            dr["�X�ͤ��"] = dtSrc.Rows[i][20].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�D�H�Υd����d��
    public static DataTable EcBankQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("����覡");
        dtRet.Columns.Add("������");
        dtRet.Columns.Add("����s��");
        dtRet.Columns.Add("������B");
        dtRet.Columns.Add("ú�O�I����");
        dtRet.Columns.Add("ú�O���A");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�p���q��");
        dtRet.Columns.Add("�q�l�l��");
        dtRet.Columns.Add("���ڥγ~");
        dtRet.Columns.Add("���ڶ}��");
        dtRet.Columns.Add("���ک��Y");
        dtRet.Columns.Add("���ڦa�}");
        dtRet.Columns.Add("�q�T�a�}");
        dtRet.Columns.Add("�ʧO");
        dtRet.Columns.Add("������");
        dtRet.Columns.Add("�X�ͤ��");
        dtRet.Columns.Add("�Ш|�{��");
        dtRet.Columns.Add("¾�~");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
            dr["����覡"] = dtSrc.Rows[i][2].ToString();
            dr["������"] = dtSrc.Rows[i][3].ToString();
            dr["����s��"] = dtSrc.Rows[i][4].ToString();
            dr["������B"] = dtSrc.Rows[i][5].ToString();
            dr["ú�O�I����"] = dtSrc.Rows[i][6].ToString();
            //20140411 �ק�by Ian_Kao
            //-----------------------------------------------
            dr["ú�O���A"] = dtSrc.Rows[i][7].ToString();
            //-----------------------------------------------
            dr["���"] = dtSrc.Rows[i][8].ToString();
            dr["�p���q��"] = dtSrc.Rows[i][9].ToString();
            dr["�q�l�l��"] = dtSrc.Rows[i][10].ToString();
            dr["���ڥγ~"] = dtSrc.Rows[i][11].ToString();
            dr["���ڶ}��"] = dtSrc.Rows[i][12].ToString();
            dr["���ک��Y"] = dtSrc.Rows[i][13].ToString();
            dr["���ڦa�}"] = dtSrc.Rows[i][14].ToString();
            dr["�q�T�a�}"] = dtSrc.Rows[i][15].ToString();
            dr["�ʧO"] = dtSrc.Rows[i][16].ToString();
            dr["������"] = dtSrc.Rows[i][17].ToString();
            dr["�X�ͤ��"] = dtSrc.Rows[i][18].ToString();
            dr["�Ш|�{��"] = dtSrc.Rows[i][19].ToString();
            dr["¾�~"] = dtSrc.Rows[i][20].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�ꪫ�^�m�D�ɺ��@
    public static DataTable GoodsList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���~�N��");
        dtRet.Columns.Add("���~�W��");
        dtRet.Columns.Add("���~�ʽ�");
        dtRet.Columns.Add("���~���O");
        dtRet.Columns.Add("�{���w�s�q");
        dtRet.Columns.Add("�w�s���");
        dtRet.Columns.Add("�w�s�޲z");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["���~�N��"] = dtSrc.Rows[i][0].ToString();
            dr["���~�W��"] = dtSrc.Rows[i][1].ToString();
            dr["���~�ʽ�"] = dtSrc.Rows[i][2].ToString();
            dr["���~���O"] = dtSrc.Rows[i][3].ToString();
            dr["�{���w�s�q"] = dtSrc.Rows[i][4].ToString();
            dr["�w�s���"] = dtSrc.Rows[i][5].ToString();
            if (dtSrc.Rows[i][6].ToString() == "V")
            {
                dr["�w�s�޲z"] = "�O";
            }
            else if (dtSrc.Rows[i][6].ToString() == "")
            {
                dr["�w�s�޲z"] = "�_";
            }
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //Ū�̸�ƺ��@ 
    public static DataTable MemberQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�s��");
        dtRet.Columns.Add("Ū�̩m�W");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�s���q��");
        dtRet.Columns.Add("������X");
        dtRet.Columns.Add("�q�T�a�}");
        dtRet.Columns.Add("�q�l���");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["�s��"] = dtSrc.Rows[i][1].ToString();
            dr["Ū�̩m�W"] = dtSrc.Rows[i][2].ToString();
            dr["�����O"] = dtSrc.Rows[i][3].ToString();
            dr["�s���q��"] = dtSrc.Rows[i][4].ToString();
            dr["������X"] = dtSrc.Rows[i][5].ToString();
            dr["�q�T�a�}"] = dtSrc.Rows[i][6].ToString();
            dr["�q�l���"] = dtSrc.Rows[i][7].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //Ū�̦W�U
    public static DataTable MemberNameList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�s��");
        dtRet.Columns.Add("Ū�̩m�W");
        dtRet.Columns.Add("���A");
        dtRet.Columns.Add("�ʧO");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�p���q�ܤ�");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�p���q�ܩ]");
        dtRet.Columns.Add("�q�l�H�c");
        dtRet.Columns.Add("�q�T�a�}");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["�s��"] = dtSrc.Rows[i][1].ToString();
            dr["Ū�̩m�W"] = dtSrc.Rows[i][2].ToString();
            dr["���A"] = dtSrc.Rows[i][3].ToString();
            dr["�ʧO"] = dtSrc.Rows[i][4].ToString();
            dr["�����O"] = dtSrc.Rows[i][5].ToString();
            dr["�p���q�ܤ�"] = dtSrc.Rows[i][6].ToString();
            dr["���"] = dtSrc.Rows[i][7].ToString();
            dr["�p���q�ܩ]"] = dtSrc.Rows[i][8].ToString();
            dr["�q�l�H�c"] = dtSrc.Rows[i][9].ToString();
            dr["�q�T�a�}"] = dtSrc.Rows[i][10].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //Ū�̦W�U_�ץX
    public static DataTable MemberNameList_Print_Excel(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�s��");
        dtRet.Columns.Add("Ū�̩m�W");
        dtRet.Columns.Add("���A");
        dtRet.Columns.Add("�ʧO");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�����Ҳνs");
        dtRet.Columns.Add("�X�ͤ��");
        dtRet.Columns.Add("�Ш|�{��");
        dtRet.Columns.Add("¾�~�O");
        dtRet.Columns.Add("�B�ê��p");
        dtRet.Columns.Add("�v�ЫH��");
        dtRet.Columns.Add("���ݱз|");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�q�ܤ�");
        dtRet.Columns.Add("�q�ܩ]");
        dtRet.Columns.Add("�q�l�H�c");
        dtRet.Columns.Add("�p���H");
        dtRet.Columns.Add("�A�ȳ��");
        dtRet.Columns.Add("¾��");
        dtRet.Columns.Add("�q�T�a�}");
        dtRet.Columns.Add("���~�a�}");
        dtRet.Columns.Add("�ȥ���Z");
        dtRet.Columns.Add("�q�l��");


        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["�s��"] = dtSrc.Rows[i][1].ToString();
            dr["Ū�̩m�W"] = dtSrc.Rows[i][2].ToString();
            dr["���A"] = dtSrc.Rows[i][3].ToString();
            dr["�ʧO"] = dtSrc.Rows[i][4].ToString();
            dr["�ٿ�"] = dtSrc.Rows[i][5].ToString();
            dr["�����O"] = dtSrc.Rows[i][6].ToString();
            dr["�����Ҳνs"] = dtSrc.Rows[i][7].ToString();
            dr["�X�ͤ��"] = dtSrc.Rows[i][8].ToString() != "" ? DateTime.Parse(dtSrc.Rows[i][8].ToString().Trim()).ToShortDateString().ToString() : "";
            dr["�Ш|�{��"] = dtSrc.Rows[i][9].ToString();
            dr["¾�~�O"] = dtSrc.Rows[i][10].ToString();
            dr["�B�ê��p"] = dtSrc.Rows[i][11].ToString();
            dr["�v�ЫH��"] = dtSrc.Rows[i][12].ToString();
            dr["���ݱз|"] = dtSrc.Rows[i][13].ToString();
            dr["���"] = dtSrc.Rows[i][14].ToString();
            dr["�q�ܤ�"] = dtSrc.Rows[i][15].ToString();
            dr["�q�ܩ]"] = dtSrc.Rows[i][16].ToString();
            dr["�q�l�H�c"] = dtSrc.Rows[i][17].ToString();
            dr["�p���H"] = dtSrc.Rows[i][18].ToString();
            dr["�A�ȳ��"] = dtSrc.Rows[i][19].ToString();
            dr["¾��"] = dtSrc.Rows[i][20].ToString();
            dr["�q�T�a�}"] = dtSrc.Rows[i][21].ToString();
            dr["���~�a�}"] = dtSrc.Rows[i][22].ToString();
            dr["�ȥ���Z"] = dtSrc.Rows[i][23].ToString();
            dr["�q�l��"] = dtSrc.Rows[i][24].ToString();
            
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //Ū�̲έp
    public static DataTable MemberReport_Condition(DataTable dtSrc, String Condition)
    {
        DataTable dtRet = new DataTable();
        if (Condition == "1")
        {
            dtRet.Columns.Add("���O�έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");

            string[] cla = { "�ǥ�", "�ӤH", "����", "���O����", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["���O�έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "2")
        {
            dtRet.Columns.Add("�ʧO�έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "�k", "�k", "�\", "�ʧO����", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�ʧO�έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "3")
        {
            dtRet.Columns.Add("�~�ֲέp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "20���H�U", "21 �� 25��", "26 �� 30��", "31 �� 35��", "36 �� 40��", "41 �� 45��", "46 �� 50��", "51 �� 55��", "56 �� 60��", "61 �� 65��", "66 �� 70��", "71���H�W", "�~�֤���", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�~�ֲέp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "4")
        {
            dtRet.Columns.Add("�Ш|�{�ײέp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "���Ѧr", "��p", "�ꤤ", "����", "�j��", "�Ӥh", "�դh", "�դh���s", "�Ш|�{�פ���", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�Ш|�{�ײέp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "5")
        {
            dtRet.Columns.Add("¾�~�O�έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "����", "�xĵ", "�ǥ�", "�A", "��", "��", "�a��", "�A��", "�ۥ�", "���@", "�h��", "��L", "¾�~�O����", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["¾�~�O�έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "6")
        {
            dtRet.Columns.Add("�B�ê��p�έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "���B", "�w�B", "���~", "���B", "�స", "��L", "�B�ê��p����", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�B�ê��p�έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "7")
        {
            dtRet.Columns.Add("�v�ЫH���έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "�����", "�v�ЫH������", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�v�ЫH���έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "8")
        {
            dtRet.Columns.Add("�q�T�����έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "�򶩥�", "�x�_��", "�s�_��", "��鿤", "�s�˥�", "�s�˿�", "�]�߿�", "�x����", "���ƿ�", "�n�뿤", "���L��", "�Ÿq��", "�Ÿq��", "�x�n��", "������", "�̪F��", "�y����", "�Ὤ��", "�x�F��", "���", "������", "�s����", "�q�T��������", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["�q�T�����έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "9")
        {
            dtRet.Columns.Add("���A�έp");
            dtRet.Columns.Add("�H��");
            dtRet.Columns.Add("�ʤ���");


            string[] cla = { "�s�J�|", "�A�Ȥ�", "���v", "���W", "�h�|", "���P", "���`", "���A����", "�X�p" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["���A�έp"] = cla[i];
                dr["�H��"] = dtSrc.Rows[0][count].ToString();
                dr["�ʤ���"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        return dtRet;
    }
    //20140205�Ȼs����
    //---------------------------------------------------------------------------
    //�إx/�D�إx�^�m�ŶZ��
    public static DataTable Donate_Week_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�Ǹ�");
        dtRet.Columns.Add("�ŶZ");
        dtRet.Columns.Add("����");
        dtRet.Columns.Add("�p�p");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["�Ǹ�"] = dtSrc.Rows[i][0].ToString();
                dr["�ŶZ"] = DateTime.Now.ToString();
                dr["����"] = "";
                dr["�p�p"] = "";
            }
            else
            {
                dr["�Ǹ�"] = dtSrc.Rows[i][0].ToString();
                dr["�ŶZ"] = dtSrc.Rows[i][1].ToString();
                dr["����"] = dtSrc.Rows[i][2].ToString();
                dr["�p�p"] = dtSrc.Rows[i][3].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�s���ڤH���Ӫ�
    public static DataTable Donate_Week_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤH�m�W");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m<br>����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("��Z�H�e");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][1].ToString();
                dr["���ڤH�m�W"] = dtSrc.Rows[i][4].ToString() + dtSrc.Rows[i][3].ToString();
                dr["�������"] = "";
                dr["�֭p�^�m<br>���B"] = "";
                dr["�^�m<br>����"] = "";
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][5].ToString();
                dr["�a�}"] = dtSrc.Rows[i][6].ToString() + dtSrc.Rows[i][7].ToString();
                dr["�q��"] = "";
                dr["���"] = "";
                dr["��Z�H�e"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["���ڤH�m�W"] = dtSrc.Rows[i][1].ToString();
                dr["�������"] = dtSrc.Rows[i][2].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][3].ToString();
                dr["�^�m<br>����"] = dtSrc.Rows[i][4].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][5].ToString();
                dr["�a�}"] = dtSrc.Rows[i][6].ToString();
                dr["�q��"] = dtSrc.Rows[i][7].ToString();
                dr["���"] = dtSrc.Rows[i][8].ToString();
                dr["��Z�H�e"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�d�߳浧���ڪ��B
    public static DataTable Donate_Week_Report3_Print(DataTable dtSrc, DataTable dtSrc2,bool XLS)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH<br>�s��");
        dtRet.Columns.Add("����<br>���");
        dtRet.Columns.Add("�Ѩ�<br>�m�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("����<br>���B");
        dtRet.Columns.Add("��/�s<br>�j�B");
        dtRet.Columns.Add("����<br>���");
        dtRet.Columns.Add("�֭p<br>���B");
        dtRet.Columns.Add("����<br>�γ~");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i <= row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row)
            {
                if (XLS == true)
                {
                    dr["���ڤH<br>�s��"] = "�`���ơG";
                    dr["����<br>���"] = dtSrc2.Rows[0][0].ToString() + "��";
                    dr["�Ѩ�<br>�m�W"] = "";
                    dr["�ٿ�"] = "";
                    dr["����<br>���B"] = "";
                    dr["��/�s<br>�j�B"] = "";
                    dr["����<br>���"] = "";
                    dr["�֭p<br>���B"] = "";
                    dr["����<br>�γ~"] = "";
                    dr["�l��<br>�ϸ�"] = "�`�p";
                    dr["�a�}"] = "���ڪ��B�G" + dtSrc2.Rows[0][1].ToString() + "��";
                    dr["�q��"] = "";
                    dr["���"] = "";
                    

                }
                else
                {
                    dr["���ڤH<br>�s��"] = "�`���ơG";
                    dr["����<br>���"] = dtSrc2.Rows[0][0].ToString() + "��";
                    dr["�Ѩ�<br>�m�W"] = "";
                    dr["�ٿ�"] = "";
                    dr["����<br>���B"] = "";
                    dr["��/�s<br>�j�B"] = "";
                    dr["����<br>���"] = "";
                    dr["�֭p<br>���B"] = "";
                    dr["����<br>�γ~"] = "�`�p";
                    dr["�l��<br>�ϸ�"] = "���ڪ��B�G";
                    dr["�a�}"] = dtSrc2.Rows[0][1].ToString() + "��";
                    dr["�q��"] = "";
                    dr["���"] = "";

                }
            }
            else
            {
                dr["���ڤH<br>�s��"] = dtSrc.Rows[i][0].ToString();
                dr["����<br>���"] = dtSrc.Rows[i][1].ToString();
                dr["�Ѩ�<br>�m�W"] = dtSrc.Rows[i][2].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][3].ToString();
                dr["����<br>���B"] = dtSrc.Rows[i][4].ToString();
                dr["��/�s<br>�j�B"] = dtSrc.Rows[i][5].ToString();
                dr["����<br>���"] = dtSrc.Rows[i][6].ToString();
                dr["�֭p<br>���B"] = dtSrc.Rows[i][7].ToString();
                dr["����<br>�γ~"] = dtSrc.Rows[i][8].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][9].ToString();
                dr["�a�}"] = dtSrc.Rows[i][10].ToString();
                dr["�q��"] = dtSrc.Rows[i][11].ToString();
                dr["���"] = dtSrc.Rows[i][12].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�d�߳浧���ڪ��B(�s�¤Ѩ�)
    public static DataTable Donate_Week_Report4_Print(DataTable dtSrc, DataTable dtSrc2, bool XLS)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤ��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("���ڪ��B");
        dtRet.Columns.Add("�s/�¤Ѩ�");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�ӤH�֭p���B");
        dtRet.Columns.Add("���ڥγ~");
        dtRet.Columns.Add("�l���ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i <= row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row)
            {
                if (XLS == true)
                {
                    dr["���ڤH�s��"] = "�`���ơG";
                    dr["���ڤ��"] = dtSrc2.Rows[0][0].ToString() + "��";
                    dr["�Ѩϩm�W"] = "";
                    dr["�ٿ�"] = "";
                    dr["���ڪ��B"] = "";
                    dr["�s/�¤Ѩ�"] = "";
                    dr["�������"] = "";
                    dr["�ӤH�֭p���B"] = "";
                    dr["���ڥγ~"] = "";
                    dr["�l���ϸ�"] = "�`�p";
                    dr["�a�}"] = "���ڪ��B�G" + dtSrc2.Rows[0][1].ToString() + "��";
                    dr["�q��"] = "";
                    dr["���"] = "";


                }
                else
                {
                    dr["���ڤH�s��"] = "�`���ơG";
                    dr["���ڤ��"] = dtSrc2.Rows[0][0].ToString() + "��";
                    dr["�Ѩϩm�W"] = "";
                    dr["�ٿ�"] = "";
                    dr["���ڪ��B"] = "";
                    dr["�s/�¤Ѩ�"] = "";
                    dr["�������"] = "";
                    dr["�ӤH�֭p���B"] = "";
                    dr["���ڥγ~"] = "�`�p";
                    dr["�l���ϸ�"] = "���ڪ��B�G";
                    dr["�a�}"] = dtSrc2.Rows[0][1].ToString() + "��";
                    dr["�q��"] = "";
                    dr["���"] = "";

                }
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["���ڤ��"] = dtSrc.Rows[i][1].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][2].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][3].ToString();
                dr["���ڪ��B"] = dtSrc.Rows[i][4].ToString();
                dr["�s/�¤Ѩ�"] = dtSrc.Rows[i][5].ToString();
                dr["�������"] = dtSrc.Rows[i][6].ToString();
                dr["�ӤH�֭p���B"] = dtSrc.Rows[i][7].ToString();
                dr["���ڥγ~"] = dtSrc.Rows[i][8].ToString();
                dr["�l���ϸ�"] = dtSrc.Rows[i][9].ToString();
                dr["�a�}"] = dtSrc.Rows[i][10].ToString();
                dr["�q��"] = dtSrc.Rows[i][11].ToString();
                dr["���"] = dtSrc.Rows[i][12].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�j�B���ڤH�ͤ�d�ߪ�
    public static DataTable Donate_Month_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m<br>����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�ͤ�");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][1].ToString();
                dr["���ڤH"] = dtSrc.Rows[i][5].ToString() + dtSrc.Rows[i][2].ToString();
                dr["�ٿ�"] = "";
                dr["�����O"] = "";
                dr["�֭p�^�m<br>���B"] = "";
                dr["�^�m<br>����"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�q��"] = "";
                dr["���"] = "";
                dr["�ͤ�"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�����O"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m<br>����"] = dtSrc.Rows[i][5].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][6].ToString();
                dr["�a�}"] = dtSrc.Rows[i][7].ToString();
                dr["�q��"] = dtSrc.Rows[i][8].ToString();
                dr["���"] = dtSrc.Rows[i][9].ToString();
                dr["�ͤ�"] = dtSrc.Rows[i][10].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�w���^�m���v������Ӫ�
    public static DataTable Donate_Month_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�s���v<br>�Ѹ�");
        dtRet.Columns.Add("���v����");
        dtRet.Columns.Add("�s���v�B");
        dtRet.Columns.Add("���v�覡");
        dtRet.Columns.Add("�Ȧ�");
        dtRet.Columns.Add("�d�O");
        dtRet.Columns.Add("���v<br>�Ѹ�");
        dtRet.Columns.Add("���ڤH<br>�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["�s���v<br>�Ѹ�"] = dtSrc.Rows[i][0].ToString() + dtSrc.Rows[i][1].ToString() + dtSrc.Rows[i][4].ToString();
                dr["���v����"] = "";
                dr["�s���v�B"] = "";
                dr["���v�覡"] = "";
                dr["�Ȧ�"] = "";
                dr["�d�O"] = "";
                dr["���v<br>�Ѹ�"] = "";
                dr["���ڤH<br>�s��"] = "";
                dr["�Ѩϩm�W"] = "";
                dr["�q��"] = "";
                dr["���"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
            }
            else
            {
                dr["�s���v<br>�Ѹ�"] = dtSrc.Rows[i][1].ToString();
                dr["���v����"] = dtSrc.Rows[i][2].ToString();
                dr["�s���v�B"] = dtSrc.Rows[i][3].ToString();
                dr["���v�覡"] = dtSrc.Rows[i][4].ToString();
                dr["�Ȧ�"] = dtSrc.Rows[i][5].ToString();
                dr["�d�O"] = dtSrc.Rows[i][6].ToString();
                dr["���v<br>�Ѹ�"] = dtSrc.Rows[i][7].ToString();
                dr["���ڤH<br>�s��"] = dtSrc.Rows[i][8].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][9].ToString();
                dr["�q��"] = dtSrc.Rows[i][10].ToString();
                dr["���"] = dtSrc.Rows[i][11].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][12].ToString();
                dr["�a�}"] = dtSrc.Rows[i][13].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�l�H��Z���Ӫ�
    public static DataTable Donate_Month_Report3_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�l���ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("��Z����");
        dtRet.Columns.Add("���D���p��");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][1].ToString() + "��";
                dr["���ڤH"] = "";
                dr["�l���ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�����O"] = "";
                dr["��Z����"] = "";
                dr["���D���p��"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
                dr["�l���ϸ�"] = dtSrc.Rows[i][2].ToString();
                dr["�a�}"] = dtSrc.Rows[i][3].ToString();
                dr["�����O"] = dtSrc.Rows[i][4].ToString();
                dr["��Z����"] = dtSrc.Rows[i][5].ToString();
                dr["���D���p��"] = dtSrc.Rows[i][6].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�l�H�q�l�����Ӫ�
    public static DataTable Donate_Month_Report4_Print(DataTable dtSrc,bool XLS)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH<br>�s��");
        dtRet.Columns.Add("�Ѩ�<br>�m�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�ֿn�^�m���B");
        dtRet.Columns.Add("�^�m<br>����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("Email");
        dtRet.Columns.Add("����<br>���");
        dtRet.Columns.Add("����<br>���");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                if (XLS == true)
                {
                    dr["���ڤH<br>�s��"] = dtSrc.Rows[i][1].ToString();
                    dr["�Ѩ�<br>�m�W"] = dtSrc.Rows[i][0].ToString() + dtSrc.Rows[i][2].ToString();
                    dr["�ٿ�"] = "";
                    dr["�����O"] = "";
                    dr["�ֿn�^�m���B"] = "";
                    dr["�^�m<br>����"] = "";
                    dr["�l��<br>�ϸ�"] = "";
                    dr["�a�}"] = "";
                    dr["�q��"] = dtSrc.Rows[i][6].ToString() + " " +  dtSrc.Rows[i][7].ToString();
                    dr["���"] = dtSrc.Rows[i][8].ToString() + dtSrc.Rows[i][9].ToString();
                    dr["Email"] = "";
                    dr["����<br>���"] = "";
                    dr["����<br>���"] = "";
                }
                else
                {
                    dr["���ڤH<br>�s��"] = dtSrc.Rows[i][1].ToString();
                    dr["�Ѩ�<br>�m�W"] = dtSrc.Rows[i][0].ToString() + dtSrc.Rows[i][2].ToString();
                    dr["�ٿ�"] = "";
                    dr["�����O"] = "";
                    dr["�ֿn�^�m���B"] = "";
                    dr["�^�m<br>����"] = "";
                    dr["�l��<br>�ϸ�"] = "";
                    dr["�a�}"] = dtSrc.Rows[i][6].ToString();
                    dr["�q��"] = dtSrc.Rows[i][7].ToString();
                    dr["���"] = dtSrc.Rows[i][8].ToString() + dtSrc.Rows[i][9].ToString();
                    dr["Email"] = "";
                    dr["����<br>���"] = "";
                    dr["����<br>���"] = "";
                }
            }
            else
            {
                dr["���ڤH<br>�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩ�<br>�m�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�����O"] = dtSrc.Rows[i][3].ToString();
                dr["�ֿn�^�m���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m<br>����"] = dtSrc.Rows[i][5].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][6].ToString();
                dr["�a�}"] = dtSrc.Rows[i][7].ToString();
                dr["�q��"] = dtSrc.Rows[i][8].ToString();
                dr["���"] = dtSrc.Rows[i][9].ToString();
                dr["Email"] = dtSrc.Rows[i][10].ToString();
                dr["����<br>���"] = dtSrc.Rows[i][11].ToString();
                dr["����<br>���"] = dtSrc.Rows[i][12].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���U�鮽�ڤ覡�έp��
    public static DataTable Donate_Month_Report5_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�{��");
        dtRet.Columns.Add("����");
        dtRet.Columns.Add("�H�Υd���v��<br>(�@��)");
        dtRet.Columns.Add("�l����b");
        dtRet.Columns.Add("�״�");
        dtRet.Columns.Add("�䲼");
        dtRet.Columns.Add("�ꪫ�^�m");
        dtRet.Columns.Add("ATM");
        dtRet.Columns.Add("�����H�Υd");
        dtRet.Columns.Add("ACH");
        dtRet.Columns.Add("����B�q");
        dtRet.Columns.Add("�p�p");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();

                dr["���"] = dtSrc.Rows[i][0].ToString();
                dr["�{��"] = dtSrc.Rows[i][1].ToString();
                dr["����"] = dtSrc.Rows[i][2].ToString();
                dr["�H�Υd���v��<br>(�@��)"] = dtSrc.Rows[i][3].ToString();
                dr["�l����b"] = dtSrc.Rows[i][4].ToString();
                dr["�״�"] = dtSrc.Rows[i][5].ToString();
                dr["�䲼"] = dtSrc.Rows[i][6].ToString();
                dr["�ꪫ�^�m"] = dtSrc.Rows[i][7].ToString();
                dr["ATM"] = dtSrc.Rows[i][8].ToString();
                dr["�����H�Υd"] = dtSrc.Rows[i][9].ToString();
                dr["ACH"] = dtSrc.Rows[i][10].ToString();
                dr["����B�q"] = dtSrc.Rows[i][11].ToString();
                dr["�p�p"] = dtSrc.Rows[i][12].ToString();
            
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�D�إx�^�m�έp���R��
    public static DataTable Donate_Season_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�Ǹ�");
        dtRet.Columns.Add("�H��");
        dtRet.Columns.Add("����");
        dtRet.Columns.Add("�`���B");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["�Ǹ�"] = dtSrc.Rows[i][1].ToString() + dtSrc.Rows[i][3].ToString();
                dr["�H��"] = "";
                dr["����"] = "";
                dr["�`���B"] = "";
            }
            else
            {
                dr["�Ǹ�"] = dtSrc.Rows[i][0].ToString();
                dr["�H��"] = dtSrc.Rows[i][1].ToString() + "�H";
                dr["����"] = dtSrc.Rows[i][2].ToString();
                dr["�`���B"] = dtSrc.Rows[i][3].ToString() + "��";
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�d�߳浧���ڪ��B�֭p
    public static DataTable Donate_Season_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�̰��^�m���B");
        dtRet.Columns.Add("�֭p�^�m���B");
        dtRet.Columns.Add("�^�m<br>����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�������");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][0].ToString() + "��";
                dr["�Ѩϩm�W"] = "";
                dr["�ٿ�"] = "";
                dr["�̰��^�m���B"] = "�`�p ���ڪ��B�G";
                dr["�֭p�^�m���B"] = dtSrc.Rows[i][3].ToString() + "��";
                dr["�^�m<br>����"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�������"] = "";
                dr["�������"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�̰��^�m���B"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m<br>����"] = dtSrc.Rows[i][5].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][6].ToString();
                dr["�a�}"] = dtSrc.Rows[i][7].ToString();
                dr["�������"] = dtSrc.Rows[i][8].ToString();
                dr["�������"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���ڥγ~�U���`�B���Ӫ�
    public static DataTable Donate_Season_Report3_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�^�m�γ~");
        dtRet.Columns.Add("�֭p�^�m���B");
        dtRet.Columns.Add("�^�m<br>����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�ͤ�");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�������");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["�s��"] = "�C�L����G" ;
                dr["�Ѩϩm�W"] = DateTime.Now.ToShortDateString();
                dr["�ٿ�"] = DateTime.Now.ToShortTimeString();
                dr["�����O"] = "";
                dr["�^�m�γ~"] = "";
                dr["�֭p�^�m���B"] = "";
                dr["�^�m<br>����"] = "";
                dr["�l��<br>�ϸ�"] = "�`�p ";
                dr["�a�}"] = "���ڪ��B�G" + dtSrc.Rows[i][1].ToString() + "��";
                dr["�q��"] = "";
                dr["���"] = "";
                dr["�ͤ�"] = "";
                dr["�������"] = "";
                dr["�������"] = "";
            }
            else
            {
                dr["�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�����O"] = dtSrc.Rows[i][3].ToString();
                dr["�^�m�γ~"] = dtSrc.Rows[i][4].ToString();
                dr["�֭p�^�m���B"] = dtSrc.Rows[i][5].ToString();
                dr["�^�m<br>����"] = dtSrc.Rows[i][6].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][7].ToString();
                dr["�a�}"] = dtSrc.Rows[i][8].ToString();
                dr["�q��"] = dtSrc.Rows[i][9].ToString();
                dr["���"] = dtSrc.Rows[i][10].ToString();
                dr["�ͤ�"] = dtSrc.Rows[i][11].ToString();
                dr["�������"] = dtSrc.Rows[i][12].ToString();
                dr["�������"] = dtSrc.Rows[i][13].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�����`�B���Ӫ�
    public static DataTable Donate_Season_Report4_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m<br>����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�Z��<br>��h");
        dtRet.Columns.Add("��Z<br>����");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][0].ToString() + "��";
                dr["�Ѩϩm�W"] = "";
                dr["�ٿ�"] = "�`�p ";
                dr["�����O"] = "���ڪ��B�G";
                dr["�֭p�^�m<br>���B"] =  dtSrc.Rows[i][1].ToString() + "��";
                dr["�^�m<br>����"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�q��"] = "";
                dr["���"] = "";
                dr["�������"] = "";
                dr["�������"] = "";
                dr["�Z��<br>��h"] = "";
                dr["��Z<br>����"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�����O"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m<br>����"] = dtSrc.Rows[i][5].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][6].ToString();
                dr["�a�}"] = dtSrc.Rows[i][7].ToString();
                dr["�q��"] = dtSrc.Rows[i][8].ToString();
                dr["���"] = dtSrc.Rows[i][9].ToString();
                dr["�������"] = dtSrc.Rows[i][10].ToString();
                dr["�������"] = dtSrc.Rows[i][11].ToString();
                dr["�Z��<br>��h"] = dtSrc.Rows[i][12].ToString();
                dr["��Z<br>����"] = dtSrc.Rows[i][13].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�����`�B���Ӫ�
    public static DataTable Donate_Year_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m<br>����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�������");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][1].ToString() + "��";
                dr["�Ѩϩm�W"] = "";
                dr["�ٿ�"] = "�`�p ���ڪ��B�G";
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][2].ToString() + "��";
                dr["�^�m<br>����"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�������"] = "";
                dr["�������"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][3].ToString();
                dr["�^�m<br>����"] = dtSrc.Rows[i][4].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][5].ToString();
                dr["�a�}"] = dtSrc.Rows[i][6].ToString();
                dr["�������"] = dtSrc.Rows[i][7].ToString();
                dr["�������"] = dtSrc.Rows[i][8].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�����`�B�P��Z�������Ӫ�
    public static DataTable Donate_Year_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m<br>����");
        dtRet.Columns.Add("��Z<br>����");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�������");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][0].ToString() + "��";
                dr["���ڤH"] = "";
                dr["�ٿ�"] = "";
                dr["�����O"] = "�`�p ���ڪ��B�G";
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][1].ToString() + "��";
                dr["�^�m<br>����"] = "";
                dr["��Z<br>����"] = "";
                dr["�a�}"] = "";
                dr["�������"] = "";
                dr["�������"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�����O"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m<br>����"] = dtSrc.Rows[i][5].ToString();
                dr["��Z<br>����"] = dtSrc.Rows[i][6].ToString();
                dr["�a�}"] = dtSrc.Rows[i][7].ToString();
                dr["�������"] = dtSrc.Rows[i][8].ToString();
                dr["�������"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���~�����`�B���Ӫ�
    public static DataTable Donate_Year_Report3_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�̰��^�m<br>���B");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m����");
        dtRet.Columns.Add("���ڤ覡");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("��O");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][1].ToString() + "��";
                dr["�Ѩϩm�W"] = "";
                dr["�ٿ�"] = "";
                dr["�̰��^�m<br>���B"] = "�`�p ���ڪ��B�G";
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][2].ToString() + "��";
                dr["�^�m����"] = "";
                dr["���ڤ覡"] = "";
                dr["�a�}"] = "";
                dr["��O"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�̰��^�m<br>���B"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m����"] = dtSrc.Rows[i][5].ToString();
                dr["���ڤ覡"] = dtSrc.Rows[i][6].ToString();
                dr["�a�}"] = dtSrc.Rows[i][7].ToString();
                dr["��O"] = dtSrc.Rows[i][8].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�ꤺ�a�Ϯ����`�B���Ӫ�
    public static DataTable Donate_Year_Report4_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�֭p�^�m���B");
        dtRet.Columns.Add("�^�m����");
        dtRet.Columns.Add("�l���ϸ�");
        dtRet.Columns.Add("�a�}");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][1].ToString() + "��";
                dr["�Ѩϩm�W"] = "";
                dr["�ٿ�"] = "�`�p ���ڪ��B�G";
                dr["�֭p�^�m���B"] = dtSrc.Rows[i][2].ToString() + "��";
                dr["�^�m����"] = "";
                dr["�l���ϸ�"] = "";
                dr["�a�}"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�֭p�^�m���B"] = dtSrc.Rows[i][3].ToString();
                dr["�^�m����"] = dtSrc.Rows[i][4].ToString();
                dr["�l���ϸ�"] = dtSrc.Rows[i][5].ToString();
                dr["�a�}"] = dtSrc.Rows[i][6].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���������O�����`�B���Ӫ�
    public static DataTable Donate_Year_Report5_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�������");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][1].ToString() + "��";
                dr["���ڤH"] = "";
                dr["�ٿ�"] = "�`�p";
                dr["�����O"] = "���ڪ��B�G";
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][2].ToString() + "��";
                dr["�^�m����"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�������"] = "";
                dr["�������"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�����O"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m����"] = dtSrc.Rows[i][5].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][6].ToString();
                dr["�a�}"] = dtSrc.Rows[i][7].ToString();
                dr["�������"] = dtSrc.Rows[i][8].ToString();
                dr["�������"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�����浧���ڪ��B���Ʃ��Ӫ�
    public static DataTable Donate_Year_Report6_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�������");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][1].ToString() + "��";
                dr["���ڤH"] = "";
                dr["�ٿ�"] = "�`�p";
                dr["�����O"] = "���ڪ��B�G";
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][2].ToString() + "��";
                dr["�^�m����"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�������"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�����O"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m����"] = dtSrc.Rows[i][5].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][6].ToString();
                dr["�a�}"] = dtSrc.Rows[i][7].ToString();
                dr["�������"] = dtSrc.Rows[i][8].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�s�W���ڤH�`�B���Ӫ�
    public static DataTable Donate_Year_Report7_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤH");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�̰��^�m<br>���B");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m����");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�������");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][1].ToString() + "��";
                dr["���ڤH"] = "";
                dr["�ٿ�"] = "�`�p";
                dr["�̰��^�m<br>���B"] = "���ڪ��B�G";
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][2].ToString() + "��";
                dr["�^�m����"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�������"] = "";
                dr["�������"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["���ڤH"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�̰��^�m<br>���B"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m����"] = dtSrc.Rows[i][5].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][6].ToString();
                dr["�a�}"] = dtSrc.Rows[i][7].ToString();
                dr["�������"] = dtSrc.Rows[i][8].ToString();
                dr["�������"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�g�`�^�m�`�B���Ӫ�
    public static DataTable Donate_Year_Report8_Print(DataTable dtSrc,bool XLS)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH<br>�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�֭p�^�m<br>���B");
        dtRet.Columns.Add("�^�m<br>����");
        dtRet.Columns.Add("�^�m�γ~");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�������");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                if (XLS == true)
                {
                    dr["���ڤH<br>�s��"] = "�`���ơG";
                    dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString() + "��";
                    dr["�ٿ�"] = "";
                    dr["�����O"] = "";
                    dr["�֭p�^�m<br>���B"] = "";
                    dr["�^�m<br>����"] = "";
                    dr["�^�m�γ~"] = "";
                    dr["�l��<br>�ϸ�"] = "�`�p";
                    dr["�a�}"] = "&nbsp;&nbsp;���ڪ��B�G" + dtSrc.Rows[i][2].ToString() + "��";
                    dr["�q��"] = "";
                    dr["���"] = "";
                    dr["�������"] = "";
                    dr["�������"] = "";
                }
                else
                {
                    dr["���ڤH<br>�s��"] = "�`���ơG";
                    dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString() + "��";
                    dr["�ٿ�"] = "";
                    dr["�����O"] = "";
                    dr["�֭p�^�m<br>���B"] = "";
                    dr["�^�m<br>����"] = "";
                    dr["�^�m�γ~"] = "";
                    dr["�l��<br>�ϸ�"] = "�`�p";
                    dr["�a�}"] = "&nbsp;&nbsp;���ڪ��B�G";
                    dr["�q��"] = dtSrc.Rows[i][2].ToString() + "��";
                    dr["���"] = "";
                    dr["�������"] = "";
                    dr["�������"] = "";
                }
            }
            else
            {
                dr["���ڤH<br>�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�����O"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m<br>���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m<br>����"] = dtSrc.Rows[i][5].ToString();
                dr["�^�m�γ~"] = dtSrc.Rows[i][6].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][7].ToString();
                dr["�a�}"] = dtSrc.Rows[i][8].ToString();
                dr["�q��"] = dtSrc.Rows[i][9].ToString();
                dr["���"] = dtSrc.Rows[i][10].ToString();
                dr["�������"] = dtSrc.Rows[i][11].ToString();
                dr["�������"] = dtSrc.Rows[i][12].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //���~�Ӥj���^�m�Ѩϲ֭p�^�m���Ӫ�
    public static DataTable Donate_Year_Report9_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�����O");
        dtRet.Columns.Add("�֭p�^�m���B");
        dtRet.Columns.Add("�^�m����");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�������");
        dtRet.Columns.Add("�������");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][1].ToString() + "��";
                dr["�Ѩϩm�W"] = "";
                dr["�ٿ�"] = "�`�p  ���ڪ��B�G";
                dr["�����O"] = dtSrc.Rows[i][2].ToString() + "��";
                dr["�֭p�^�m���B"] = "";
                dr["�^�m����"] = "";
                dr["�a�}"] = "";
                dr["�������"] = "";
                dr["�������"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�����O"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m���B"] = dtSrc.Rows[i][4].ToString();
                dr["�^�m����"] = dtSrc.Rows[i][5].ToString();
                dr["�a�}"] = dtSrc.Rows[i][6].ToString();
                dr["�������"] = dtSrc.Rows[i][7].ToString();
                dr["�������"] = dtSrc.Rows[i][8].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //-------------------------------------------------------------------------------------------------------------
    //20140418 �s�W by Ian_Kao & �ק� by �ֻ�
    //-------------------------------------------------------------------------------------------------------------
    //�H�Ʋέp�d�߳���
    public static DataTable Donate_Other_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�Ǹ�");
        dtRet.Columns.Add("�Ҧ��^�m�ѨϤH��");
        dtRet.Columns.Add("�֭p�^�m10�U�H�W<br>�`�H��");
        dtRet.Columns.Add("�֭p�^�m30�U�H�W<br>�`�H��");
        dtRet.Columns.Add("�֭p�^�m50�U�H�W<br>�`�H��");
        dtRet.Columns.Add("�֭p�^�m100�U�H�W<br>�`�H��");
        dtRet.Columns.Add("�ͦn�з|�ξ��c�ƶq");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i <= row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row)
            {
                dr["�Ǹ�"] = "�C�L����G";
                dr["�Ҧ��^�m�ѨϤH��"] = DateTime.Now.ToString();
                dr["�֭p�^�m10�U�H�W<br>�`�H��"] = "";
                dr["�֭p�^�m30�U�H�W<br>�`�H��"] = "";
                dr["�֭p�^�m50�U�H�W<br>�`�H��"] = "";
                dr["�֭p�^�m100�U�H�W<br>�`�H��"] = "";
                dr["�ͦn�з|�ξ��c�ƶq"] = "";
            }
            /*else if (i == row - 1)
            {
                dr["�Ǹ�"] = i + 1;
                dr["�Ҧ��^�m�ѨϤH��"] = "�H�ơG" + dtSrc.Rows[i][0].ToString();
                dr["�֭p�^�m10�U�H�W<br>�`�H��"] = "���ơG" + dtSrc.Rows[i][1].ToString();
                dr["�֭p�^�m30�U�H�W<br>�`�H��"] = "�`���B�G" + dtSrc.Rows[i][2].ToString() + "��";
                dr["�֭p�^�m50�U�H�W<br>�`�H��"] = "";
                dr["�֭p�^�m100�U�H�W<br>�`�H��"] = "";
                dr["�ͦn�з|�ξ��c�ƶq"] = "";
            }*/
            else
            {
                dr["�Ǹ�"] = i + 1;
                dr["�Ҧ��^�m�ѨϤH��"] = dtSrc.Rows[i][0].ToString();
                dr["�֭p�^�m10�U�H�W<br>�`�H��"] = dtSrc.Rows[i][1].ToString();
                dr["�֭p�^�m30�U�H�W<br>�`�H��"] = dtSrc.Rows[i][2].ToString();
                dr["�֭p�^�m50�U�H�W<br>�`�H��"] = dtSrc.Rows[i][3].ToString();
                dr["�֭p�^�m100�U�H�W<br>�`�H��"] = dtSrc.Rows[i][4].ToString();
                dr["�ͦn�з|�ξ��c�ƶq"] = dtSrc.Rows[i][5].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //DVD�ث~�����H����
    public static DataTable Donate_Other_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ٿ�");
        dtRet.Columns.Add("�l��<br>�ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("����");
        dtRet.Columns.Add("��ƿ�J<br>���");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][2].ToString() + "��";
                dr["�Ѩϩm�W"] = "";
                dr["�ٿ�"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�q��"] = "";
                dr["����"] = "";
                dr["��ƿ�J<br>���"] = "";
            }
            else
            {
                dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
                dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
                dr["�ٿ�"] = dtSrc.Rows[i][2].ToString();
                dr["�l��<br>�ϸ�"] = dtSrc.Rows[i][3].ToString();
                dr["�a�}"] = dtSrc.Rows[i][4].ToString();
                dr["�q��"] = dtSrc.Rows[i][5].ToString();
                dr["����"] = dtSrc.Rows[i][6].ToString();
                dr["��ƿ�J<br>���"] = dtSrc.Rows[i][7].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�p�P��Ƭd��
    public static DataTable Donate_Other_Report3_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("�Ѩϩm�W");
        dtRet.Columns.Add("�ʧO");
        dtRet.Columns.Add("�I�ڤ覡");
        dtRet.Columns.Add("���A");
        dtRet.Columns.Add("���ڮɶ�");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("�q�T�a�}");
        dtRet.Columns.Add("���ڦa�}");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //�̫�@�C
            /*if (i == row - 1)
            {
                dr["���ڤH�s��"] = "�`���ơG" + dtSrc.Rows[i][2].ToString() + "��";
                dr["�Ѩϩm�W"] = "";
                dr["�ٿ�"] = "";
                dr["�l��<br>�ϸ�"] = "";
                dr["�a�}"] = "";
                dr["�q��"] = "";
                dr["����"] = "";
                dr["��ƿ�J<br>���"] = "";
            }
            else
            {*/
            dr["���ڤH�s��"] = dtSrc.Rows[i][0].ToString();
            dr["�Ѩϩm�W"] = dtSrc.Rows[i][1].ToString();
            dr["�ʧO"] = dtSrc.Rows[i][2].ToString();
            dr["�I�ڤ覡"] = dtSrc.Rows[i][3].ToString();
            dr["���A"] = dtSrc.Rows[i][4].ToString();
            dr["���ڮɶ�"] = dtSrc.Rows[i][5].ToString();
            dr["�q��"] = dtSrc.Rows[i][6].ToString();
            dr["���"] = dtSrc.Rows[i][7].ToString();
            dr["�q�T�a�}"] = dtSrc.Rows[i][8].ToString();
            dr["���ڦa�}"] = dtSrc.Rows[i][9].ToString();
            //}
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�x�ȱ��v���Ѧ^���ɬd�� 20140904�s�W
    public static DataTable PledgeReturnError_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�Ǹ�");
        dtRet.Columns.Add("���v�s��");
        dtRet.Columns.Add("���v���B");
        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤH�m�W");
        dtRet.Columns.Add("�o�d�Ȧ�");
        dtRet.Columns.Add("�H�Υd�d��");
        dtRet.Columns.Add("�Ĵ�");
        dtRet.Columns.Add("���T�XCVV");
        dtRet.Columns.Add("�l���ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        dtRet.Columns.Add("���v���ѽX");
        dtRet.Columns.Add("���v���ѭ�]");
        dtRet.Columns.Add("�l�ܤ��");
        dtRet.Columns.Add("�Ƶ�");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["�Ǹ�"] = dtSrc.Rows[i][0].ToString();
            dr["���v�s��"] = dtSrc.Rows[i][1].ToString();
            dr["���v���B"] = dtSrc.Rows[i][2].ToString();
            dr["���ڤH�s��"] = dtSrc.Rows[i][3].ToString();
            dr["���ڤH�m�W"] = dtSrc.Rows[i][4].ToString();
            dr["�o�d�Ȧ�"] = dtSrc.Rows[i][5].ToString();
            dr["�H�Υd�d��"] = dtSrc.Rows[i][6].ToString();
            dr["�Ĵ�"] = dtSrc.Rows[i][7].ToString();
            dr["���T�XCVV"] = dtSrc.Rows[i][8].ToString();
            dr["�l���ϸ�"] = dtSrc.Rows[i][9].ToString();
            dr["�a�}"] = dtSrc.Rows[i][10].ToString();
            dr["�q��"] = dtSrc.Rows[i][11].ToString();
            dr["���"] = dtSrc.Rows[i][12].ToString();
            dr["���v���ѽX"] = dtSrc.Rows[i][13].ToString();
            dr["���v���ѭ�]"] = dtSrc.Rows[i][14].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //�x�ȱ��v���\�^���ɬd�� 20140915�s�W
    public static DataTable PledgeReturnOK_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("�Ǹ�");
        dtRet.Columns.Add("���v�s��");
        dtRet.Columns.Add("���v���B");
        dtRet.Columns.Add("���ڤH�s��");
        dtRet.Columns.Add("���ڤH�m�W");
        dtRet.Columns.Add("�o�d�Ȧ�");
        dtRet.Columns.Add("�H�Υd�d��");
        dtRet.Columns.Add("�Ĵ�");
        dtRet.Columns.Add("�l���ϸ�");
        dtRet.Columns.Add("�a�}");
        dtRet.Columns.Add("�q��");
        dtRet.Columns.Add("���");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["�Ǹ�"] = dtSrc.Rows[i][0].ToString();
            dr["���v�s��"] = dtSrc.Rows[i][1].ToString();
            dr["���v���B"] = dtSrc.Rows[i][2].ToString();
            dr["���ڤH�s��"] = dtSrc.Rows[i][3].ToString();
            dr["���ڤH�m�W"] = dtSrc.Rows[i][4].ToString();
            dr["�o�d�Ȧ�"] = dtSrc.Rows[i][5].ToString();
            dr["�H�Υd�d��"] = dtSrc.Rows[i][6].ToString();
            dr["�Ĵ�"] = dtSrc.Rows[i][7].ToString();
            dr["�l���ϸ�"] = dtSrc.Rows[i][8].ToString();
            dr["�a�}"] = dtSrc.Rows[i][9].ToString();
            dr["�q��"] = dtSrc.Rows[i][10].ToString();
            dr["���"] = dtSrc.Rows[i][11].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //20140410 �s�W by Ian_Kao
    //-------------------------------------------------------------------------------------------------------------
    //����Log��
    public enum LogType
    {
        LogTime,
        ActionTime,
        LogoutTime
    }
    public static void LogData(LogType logType, string UserID)
    {
        string Today = Util.GetToday(DateType.yyyyMMdd);
        DataTable dt = Util.GetDataTable("Log", "UserID", UserID, "CONVERT(char(10), LogTime, 111)", Today, "LogTime", "desc");

        string uid = "";
        string LogoutTime = "";
        if (dt.Rows.Count != 0)
        {
            uid = dt.Rows[0]["uid"].ToString();
            LogoutTime = dt.Rows[0]["LogoutTime"].ToString();
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        List<ColumnData> list = new List<ColumnData>();
        string strSql = "";
        if (logType == LogType.LogTime)
        {
            if (LogoutTime != "") //�p�G�P�@�Ѧ� logout, �h�t�s�@��
            {
                uid = "";
            }
            //�O���n�J�ɶ�
            if (uid != "") //�w��UserID+LogTime ��ƦC
            {
                SetColumnData4Log(list, LogType.ActionTime, UserID, uid);
                strSql = Util.CreateUpdateCommand("Log", list, dict);
            }
            else
            {
                SetColumnData4Log(list, LogType.LogTime, UserID, uid);
                strSql = Util.CreateInsertCommand("Log", list, dict);
            }
        }
        else if (logType == LogType.ActionTime)
        {
            //�O���ʧ@�ɶ�
            //�O���n�X�ɶ�
            if (uid == "") //Action �S���۹����� login ���
            {
                SetColumnData4Log(list, LogType.LogTime, UserID, uid);
                strSql = Util.CreateInsertCommand("Log", list, dict);
            }
            else
            {
                SetColumnData4Log(list, LogType.ActionTime, UserID, uid);
                strSql = Util.CreateUpdateCommand("Log", list, dict);
            }
        }
        else if (logType == LogType.LogoutTime)
        {
            if (uid == "") //logout �S���۹����� login ���,���B�z, �j�]�|�����D
            {
                return;
            }
            //�O���n�X�ɶ�
            SetColumnData4Log(list, LogType.LogoutTime, UserID, uid);
            strSql = Util.CreateUpdateCommand("Log", list, dict);
        }
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    private static void SetColumnData4Log(List<ColumnData> list, LogType logType, string UserID, string uid)
    {
        list.Add(new ColumnData("uid", uid, false, false, true));
        if (logType == LogType.LogTime)
        {
            list.Add(new ColumnData("UserID", UserID, true, true, false));
            list.Add(new ColumnData("LogTime", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
            list.Add(new ColumnData("ActionTime", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
            list.Add(new ColumnData("LogIP", Request.ServerVariables["REMOTE_ADDR"].ToString(), true, true, false));
        }
        else if (logType == LogType.ActionTime)
        {
            list.Add(new ColumnData("ActionTime", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        }
        else if (logType == LogType.LogoutTime)
        {
            list.Add(new ColumnData("LogoutTime", Util.GetToday(DateType.yyyyMMddHHmmss), true, true, false));
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //20140416 �s�W by Ian_Kao
    //-------------------------------------------------------------------------------------------------------------
    //����Log�� �ӧO�������e
    public static void InsertLogData(string UserID,string Log_Type,string Log_Desc)
    {
        string strSql = "insert into  Log\n";
        strSql += "( UserID, LogTime, LogIP, Log_Type, Log_Desc) values\n";
        strSql += "(@UserID,GetDate(),@LogIP,@Log_Type,@Log_Desc);";
        strSql += "\n";
        strSql += "select @@IDENTITY";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("UserID", UserID);
        dict.Add("LogIP", Request.ServerVariables["REMOTE_ADDR"].ToString());
        dict.Add("Log_Type", Log_Type);
        dict.Add("Log_Desc", Log_Desc);
        NpoDB.ExecuteSQLS(strSql, dict);
    }
    //-------------------------------------------------------------------------------------------------------------
    //20140416 �s�W by Ian_Kao
    //-------------------------------------------------------------------------------------------------------------
    //DateTime �ɶ��t�p��
    public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        int dateDiff = 0;
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2);
        dateDiff = (ts.Days/30); //��¦^�Ǥ��
        return dateDiff;
    }
    //-------------------------------------------------------------------------------------------------------------
    //20140521 �s�W by Ian_Kao
    //������ڤH�Ҧb�s��
    public static string GetDonorGroup(string strUid)
    {
        string strSql = @"
                            select I.GroupItemName+',' 
                            from Donor D
                            left join GroupMapping M
                            on D.Donor_Id=M.DonorID
                            and M.DeleteDate is null
                            left join GroupItem I
                            on M.GroupItemUid=I.Uid
                            and I.DeleteDate is null
                            where D.Donor_Id=@DonorID
                            group by I.GroupItemName
                            for xml path('')
                        ";

        string strRet = "";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("DonorID", strUid);

        DataTable dt = NpoDB.GetDataTableS(strSql, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0][0].ToString().TrimEnd(',');
        }

        return strRet;
    }
} 
//end of CaseUtil

public class CButton
{
    private int _TabNo;
    private string _CssClass;
    private string _OnClick;
    private string _ImgSrc;
    private string _Width = "20";
    private string _Height = "20";
    private string _Align = "absbottom";
    private string _Title = "";
    private string _Text;
    private string _Style;
    private bool _ShowBR = false;
    private bool _Disabled = false;

    public int TabNo
    {
        get { return _TabNo; }
        set { _TabNo = value; }
    }
    public string CssClass
    {
        get { return _CssClass; }
        set { _CssClass = value; }
    }
    public string OnClick
    {
        get { return _OnClick; }
        set { _OnClick = value; }
    }
    public string ImgSrc
    {
        get { return _ImgSrc; }
        set { _ImgSrc = value; }
    }
    public string Width
    {
        get { return _Width; }
        set { _Width = value; }
    }
    public string Height
    {
        get { return _Height; }
        set { _Height = value; }
    }
    public string Align
    {
        get { return _Align; }
        set { _Align = value; }
    }
    public string Title
    {
        get { return _Title; }
        set { _Title = value; }
    }
    public string Text
    {
        get { return _Text; }
        set { _Text = value; }
    }
    public string Style
    {
        get { return _Style; }
        set { _Style = value; }
    }
    public bool ShowBR
    {
        get { return _ShowBR; }
        set { _ShowBR = value; }
    }
    public bool Disabled
    {
        get { return _Disabled; }
        set { _Disabled = value; }
    }
}
