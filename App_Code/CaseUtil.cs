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
    //    //日期不能空白
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
    //        //日期不能大於今天
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
    //        //5號之前, 可以能輸入上個月的資料
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
    //        //超過5號, 則不能輸入上個月的資料
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
    //產生上傳檔案的 JavaScript 碼
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
        Btn.Text = "捐款人資料";
        Btn.Style = "style='width:24%'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "../DonorMgr/DonorInfo_Edit.aspx?Donor_Id=" + Donor_Id;
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 2;
        Btn.Text = "捐款記錄";
        Btn.Style = "style='width:24%'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "../DonateMgr/DonateDataList.aspx?Donor_Id=" + Donor_Id;
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 3;
        Btn.Text = "轉帳授權書記錄";
        Btn.Style = "style='width:24%'";  //需要不同寬度時,可以在此設定新值
        Btn.ImgSrc = "../images/toolbar_modify.gif";
        Btn.OnClick = "../DonateMgr/PledgeDataList.aspx?Donor_Id=" + Donor_Id;
        ButtonList.Add(Btn);

        Btn = new CButton();
        Btn.TabNo = 4;
        Btn.Text = "公關贈品記錄";
        Btn.Style = "style='width:24%'";  //需要不同寬度時,可以在此設定新值
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
        Btn.Text = "讀者資料";
        Btn.Style = "style='width:100%'";  //需要不同寬度時,可以在此設定新值
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
        //帶出相關圖檔資料
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
        //如果沒有URL時(代表無上傳圖檔)，關閉圖檔刪除按鈕
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
        //以此URL顯示圖片
        string RetStr;
        if (PictURL == "")
        {
            RetStr = "";
        }
        else
        {
            //RetStr = "<img src=\".." + PictURL + "\" border=\"0\" style=\"width:180pt;height:180pt;cursor:hand \" onclick=\"var x=window.open('','','height=480, width=640, toolbar=no, menubar=no, scrollbars=1, resizable=yes, location=no, status=no');x.document.write('<img src=.." + PictURL + " border=0>');\" alt=\"點選看放大圖\">";
            RetStr = "<img src='.." + PictURL + "' border='0' style='" + Width + ";" + Height + ";cursor:hand' onclick=\"var x=window.open('','','height=480, width=640, toolbar=no, menubar=no, scrollbars=1, resizable=yes, location=no, status=no');x.document.write('<img src=.." + PictURL + " border=0>');\" alt=\"點選看放大圖\">";
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
        dict.Add("ApName", ApName); //以此名稱區分是哪一個程式上傳此檔
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count != 0)
        {
            strSql = "delete from Upload where uid=@uid";
            dict.Add("uid", dt.Rows[0]["uid"].ToString());
            NpoDB.ExecuteSQLS(strSql, dict);
            //檔案一併刪除
            string FilePath = Server.MapPath(".." + dt.Rows[0]["UploadFileURL"].ToString());
            File.Delete(FilePath);
        }
        Session["Msg"] = "圖檔刪除成功!";
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
        if ((page as BasePage).SessionInfo.GroupArea == "全區")
        {
            return true;
        }
        return false;
    }
    //----------------------------------------------------------------------
    //列印時傳回 img 碼
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
        //帶出相關圖檔資料
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
        //以此URL顯示圖片
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
    //捐款人基本資料維護
    public static DataTable DonorInfo_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("身份別");
        dtRet.Columns.Add("連絡電話");
        dtRet.Columns.Add("手機號碼");
        dtRet.Columns.Add("通訊地址");
        //20140425 修改 by Ian_Kao 調整輸出欄位
        //dtRet.Columns.Add("首捐日");
        //dtRet.Columns.Add("末捐日");
        //dtRet.Columns.Add("捐款次數");
        //dtRet.Columns.Add("累計捐款金額");
        dtRet.Columns.Add("月刊");
        dtRet.Columns.Add("DVD");
        dtRet.Columns.Add("電子<br>文宣");
        dtRet.Columns.Add("生日卡");
        dtRet.Columns.Add("最近捐款<br>日期");


        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();

            dr["編號"] = dtSrc.Rows[i][1].ToString();
            dr["捐款人"] = dtSrc.Rows[i][2].ToString();
            dr["身份別"] = dtSrc.Rows[i][4].ToString();
            dr["連絡電話"] = dtSrc.Rows[i][5].ToString();
            dr["手機號碼"] = dtSrc.Rows[i][6].ToString();
            dr["通訊地址"] = dtSrc.Rows[i][7].ToString();
            //20140425 修改 by Ian_Kao 調整輸出欄位
            //dr["首捐日"] = dtSrc.Rows[i][8].ToString();
            //dr["末捐日"] = dtSrc.Rows[i][9].ToString();
            //dr["捐款次數"] = dtSrc.Rows[i][10].ToString();
            //dr["累計捐款金額"] = dtSrc.Rows[i][11].ToString();
            dr["月刊"] = dtSrc.Rows[i][9].ToString();
            dr["DVD"] = dtSrc.Rows[i][10].ToString();
            dr["電子<br>文宣"] = dtSrc.Rows[i][11].ToString();
            dr["生日卡"] = dtSrc.Rows[i][12].ToString();
            dr["最近捐款<br>日期"] = dtSrc.Rows[i][8].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //-------------------------------------------------------------------------------------------------------------
    //捐款人基本資料_手機名單
    public static DataTable DonorInfo_Excel_Phone(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("連絡電話");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["編號"] = dtSrc.Rows[i][1].ToString();
            dr["捐款人"] = dtSrc.Rows[i][2].ToString();
            dr["連絡電話"] = dtSrc.Rows[i][3].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //-------------------------------------------------------------------------------------------------------------
    //捐款人基本資料_Email名單
    public static DataTable DonorInfo_Excel__Email(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        //20140509 修改 by Ian_Kao 格式錯誤
        //dtRet.Columns.Add("編號");
        //dtRet.Columns.Add("捐款人");
        //dtRet.Columns.Add("電子信箱");
        dtRet.Columns.Add("Email");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //20140509 修改 by Ian_Kao 格式錯誤
            //dr["編號"] = dtSrc.Rows[i][1].ToString();
            //dr["捐款人"] = dtSrc.Rows[i][2].ToString();
            //dr["電子信箱"] = dtSrc.Rows[i][3].ToString();
            dr["Email"] = dtSrc.Rows[i][0].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //捐款人名冊
    public static DataTable DonorQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        // 2014/4/8 顯示捐款人編號
        //dtRet.Columns.Add("編號");
        dtRet.Columns.Add("捐款人編號");

        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("性別");
        dtRet.Columns.Add("身份別");
        dtRet.Columns.Add("連絡電話日");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("連絡電話夜");
        dtRet.Columns.Add("電子信箱");
        dtRet.Columns.Add("通訊地址");      
        dtRet.Columns.Add("最近捐款日期");
        dtRet.Columns.Add("累積次數");
        dtRet.Columns.Add("累積金額");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            
            // 2014/4/8 顯示捐款人編號
            dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();

            dr["捐款人"] = dtSrc.Rows[i][1].ToString();
            dr["性別"] = dtSrc.Rows[i][2].ToString();
            dr["身份別"] = dtSrc.Rows[i][3].ToString();
            dr["連絡電話日"] = dtSrc.Rows[i][4].ToString();
            dr["手機"] = dtSrc.Rows[i][5].ToString();
            dr["連絡電話夜"] = dtSrc.Rows[i][6].ToString();
            dr["電子信箱"] = dtSrc.Rows[i][7].ToString();
            dr["通訊地址"] = dtSrc.Rows[i][8].ToString();
            dr["最近捐款日期"] = dtSrc.Rows[i][9].ToString();
            dr["累積次數"] = dtSrc.Rows[i][10].ToString();
            dr["累積金額"] = dtSrc.Rows[i][11].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //捐款人名冊_匯出
    public static DataTable DonorQry_Print_Excel(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        // 2014/4/8 顯示捐款人編號
        dtRet.Columns.Add("捐款人編號");

        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("性別");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身份別");
        dtRet.Columns.Add("身分證統編");
        dtRet.Columns.Add("出生日期");
        dtRet.Columns.Add("教育程度");////
        dtRet.Columns.Add("職業別");////
        dtRet.Columns.Add("婚姻狀況");////
        dtRet.Columns.Add("宗教信仰");////
        dtRet.Columns.Add("道場教會名稱");////
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("電話日");       
        dtRet.Columns.Add("電話夜");
        dtRet.Columns.Add("電子信箱");
        dtRet.Columns.Add("聯絡人");
        dtRet.Columns.Add("服務單位");
        dtRet.Columns.Add("職稱");
        dtRet.Columns.Add("通訊地址");
        dtRet.Columns.Add("海外地址");
        dtRet.Columns.Add("紙本月刊");
        dtRet.Columns.Add("DVD");
        dtRet.Columns.Add("電子文宣");
        dtRet.Columns.Add("收據開立");
        dtRet.Columns.Add("收據抬頭");
        dtRet.Columns.Add("收據身分證統編");
        dtRet.Columns.Add("首次捐款日期");
        dtRet.Columns.Add("最近捐款日期");
        dtRet.Columns.Add("累積次數");
        dtRet.Columns.Add("累積金額");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();

            // 2014/4/8 顯示捐款人編號
            dr["捐款人編號"] = dtSrc.Rows[i]["Donor_id"].ToString();

            dr["捐款人"] = dtSrc.Rows[i]["Donor_Name"].ToString();
            dr["性別"] = dtSrc.Rows[i]["Sex"].ToString();
            dr["稱謂"] = dtSrc.Rows[i]["Title"].ToString();
            dr["身份別"] = dtSrc.Rows[i]["Donor_Type"].ToString();
            dr["身分證統編"] = dtSrc.Rows[i]["IDNo"].ToString();
            dr["出生日期"] = dtSrc.Rows[i]["Birthday"].ToString() != "" ? DateTime.Parse(dtSrc.Rows[i]["Birthday"].ToString().Trim()).ToShortDateString().ToString() : "";
            dr["教育程度"] = dtSrc.Rows[i]["Education"].ToString();
            dr["職業別"] = dtSrc.Rows[i]["Occupation"].ToString();
            dr["婚姻狀況"] = dtSrc.Rows[i]["Marriage"].ToString();
            dr["宗教信仰"] = dtSrc.Rows[i]["Religion"].ToString();
            dr["道場教會名稱"] = dtSrc.Rows[i]["ReligionName"].ToString();
            dr["手機"] = dtSrc.Rows[i]["Cellular_Phone"].ToString();
            dr["電話日"] = dtSrc.Rows[i]["Tel_Office"].ToString();
            dr["電話夜"] = dtSrc.Rows[i]["Tel_Home"].ToString();
            dr["電子信箱"] = dtSrc.Rows[i]["Email"].ToString();
            dr["聯絡人"] = dtSrc.Rows[i]["Contactor"].ToString();
            dr["服務單位"] = dtSrc.Rows[i]["OrgName"].ToString();
            dr["職稱"] = dtSrc.Rows[i]["JobTitle"].ToString();
            dr["通訊地址"] = dtSrc.Rows[i]["地址"].ToString();
            dr["海外地址"] = dtSrc.Rows[i]["IsAbroad"].ToString();
            dr["紙本月刊"] = dtSrc.Rows[i]["IsSendNews"].ToString();
            dr["DVD"] = dtSrc.Rows[i]["IsDVD"].ToString();
            dr["電子文宣"] = dtSrc.Rows[i]["IsSendEpaper"].ToString();
            dr["收據開立"] = dtSrc.Rows[i]["Invoice_Type"].ToString();
            dr["收據抬頭"] = dtSrc.Rows[i]["Invoice_Title"].ToString();
            dr["收據身分證統編"] = dtSrc.Rows[i]["Invoice_IDNo"].ToString();
            if (dtSrc.Rows[i]["Begin_DonateDate"].ToString().Trim() != "")
            {
                dr["首次捐款日期"] = DateTime.Parse(dtSrc.Rows[i]["Begin_DonateDate"].ToString().Trim()).ToShortDateString().ToString();
            }
            else
            {
                dr["首次捐款日期"] = "";
            }
            if (dtSrc.Rows[i]["Last_DonateDate"].ToString().Trim() != "")
            {
                dr["最近捐款日期"] = DateTime.Parse(dtSrc.Rows[i]["Last_DonateDate"].ToString().Trim()).ToShortDateString().ToString();
            }
            else
            {
                dr["最近捐款日期"] = "";
            }
            dr["累積次數"] = dtSrc.Rows[i]["Donate_No"].ToString();
            dr["累積金額"] = Util.ToThree(dtSrc.Rows[i]["Donate_Total"].ToString(), "0");//dtSrc.Rows[i]["Donate_Total"].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //捐款人統計
    public static DataTable DonorReport_Condition(DataTable dtSrc, String Condition)
    {
        DataTable dtRet = new DataTable();
        if (Condition == "1")
        {
            dtRet.Columns.Add("類別統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");

            string[] cla = { "學生", "個人", "團體", "類別不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["類別統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "2")
        {
            dtRet.Columns.Add("性別統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "男", "女", "歿", "性別不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["性別統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "3")
        {
            dtRet.Columns.Add("年齡統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "20歲以下", "21 ～ 25歲", "26 ～ 30歲", "31 ～ 35歲", "36 ～ 40歲", "41 ～ 45歲", "46 ～ 50歲", "51 ～ 55歲", "56 ～ 60歲", "61 ～ 65歲", "66 ～ 70歲", "71歲以上", "年齡不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["年齡統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "4")
        {
            dtRet.Columns.Add("教育程度統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "不識字", "國小", "國中", "高中", "大學", "碩士", "博士", "博士後研究", "教育程度不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["教育程度統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "5")
        {
            dtRet.Columns.Add("職業別統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "公教", "軍警", "學生", "農", "公", "商", "家管", "服務", "自由", "醫護", "退休", "其他", "職業別不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["職業別統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "6")
        {
            dtRet.Columns.Add("婚姻狀況統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "未婚", "已婚", "分居", "離婚", "喪偶", "其他", "婚姻狀況不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["婚姻狀況統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "7")
        {
            dtRet.Columns.Add("宗教信仰統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "基督教", "宗教信仰不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["宗教信仰統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "8")
        {
            dtRet.Columns.Add("通訊縣市統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "基隆市", "台北市", "新北市", "桃園縣", "新竹市", "新竹縣", "苗栗縣", "台中市", "彰化縣", "南投縣", "雲林縣", "嘉義市", "嘉義縣", "台南市", "高雄市", "屏東縣", "宜蘭縣", "花蓮縣", "台東縣", "澎湖縣", "金門縣", "連江縣", "通訊縣市不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["通訊縣市統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "9")
        {
            dtRet.Columns.Add("收據縣市統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "基隆市", "台北市", "新北市", "桃園縣", "新竹市", "新竹縣", "苗栗縣", "台中市", "彰化縣", "南投縣", "雲林縣", "嘉義市", "嘉義縣", "台南市", "高雄市", "屏東縣", "宜蘭縣", "花蓮縣", "台東縣", "澎湖縣", "金門縣", "連江縣", "通訊縣市不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["收據縣市統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        return dtRet;
    }
    //-------------------------------------------------------------------------
    //取得Linked之中Gift最小的id
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
                ///新增的網頁然後重新導入
                Session["Msg"] = "無資料，請先新增公關贈品品項！";
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
    //取得Linked之中Contribute最小的id
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
                ///新增的網頁然後重新導入
                Session["Msg"] = "無資料，請先新增物品類別！";
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
    //取得Linked中Gift之最大序號
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
    //取得Linked中Contribute之最大序號
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
    //取得Linked2之最大序號
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
    //收據維護紀錄
    public static DataTable DonateInfo_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("收據抬頭");
        dtRet.Columns.Add("捐款日期");
        dtRet.Columns.Add("捐款金額");
        dtRet.Columns.Add("捐款方式");
        dtRet.Columns.Add("捐款用途");
        dtRet.Columns.Add("勸募活動");
        dtRet.Columns.Add("收據開立");
        dtRet.Columns.Add("收據編號");
        dtRet.Columns.Add("沖帳日期");
        dtRet.Columns.Add("列印");
        dtRet.Columns.Add("狀態");
        dtRet.Columns.Add("經手人");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["編號"] = dtSrc.Rows[i][1].ToString();
            dr["捐款人"] = dtSrc.Rows[i][2].ToString();
            dr["收據抬頭"] = dtSrc.Rows[i][3].ToString();
            dr["捐款日期"] = dtSrc.Rows[i][4].ToString();
            dr["捐款金額"] = dtSrc.Rows[i][5].ToString();
            dr["捐款方式"] = dtSrc.Rows[i][6].ToString();
            dr["捐款用途"] = dtSrc.Rows[i][7].ToString();
            dr["收據開立"] = dtSrc.Rows[i][8].ToString();
            dr["收據開立"] = dtSrc.Rows[i][9].ToString();
            dr["收據編號"] = dtSrc.Rows[i][10].ToString();
            dr["沖帳日期"] = dtSrc.Rows[i][11].ToString();
            if (dtSrc.Rows[i][11].ToString() != "" && DateTime.Parse(dtSrc.Rows[i][11].ToString()).ToString("yyyy/MM/dd") != "1900/01/01")
            {
                dr["沖帳日期"] = dtSrc.Rows[i][11].ToString();
            }
            else
            {
                dr["沖帳日期"] = "";
            }
            dr["列印"] = dtSrc.Rows[i][12].ToString();
            dr["狀態"] = dtSrc.Rows[i][13].ToString();
            dr["經手人"] = dtSrc.Rows[i][14].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //固定轉帳授權書
    public static DataTable PledgeQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("授權編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("授權方式");
        dtRet.Columns.Add("扣款金額");
        dtRet.Columns.Add("授權起日");
        dtRet.Columns.Add("授權迄日");
        dtRet.Columns.Add("轉帳週期");
        dtRet.Columns.Add("下次扣款日期");
        dtRet.Columns.Add("發卡銀行-卡別");
        dtRet.Columns.Add("卡號/帳號");
        dtRet.Columns.Add("信用卡有效月年");
        dtRet.Columns.Add("授權狀態");
        dtRet.Columns.Add("經手人");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["授權編號"] = dtSrc.Rows[i][0].ToString();
            dr["捐款人"] = dtSrc.Rows[i][1].ToString();
            dr["捐款人編號"] = dtSrc.Rows[i][2].ToString();
            dr["授權方式"] = dtSrc.Rows[i][3].ToString();
            dr["扣款金額"] = dtSrc.Rows[i][4].ToString();
            dr["授權起日"] = dtSrc.Rows[i][5].ToString();
            dr["授權迄日"] = dtSrc.Rows[i][6].ToString();
            dr["轉帳週期"] = dtSrc.Rows[i][7].ToString();
            dr["下次扣款日期"] = dtSrc.Rows[i][8].ToString();
            dr["發卡銀行-卡別"] = dtSrc.Rows[i][9].ToString();
            dr["卡號/帳號"] = dtSrc.Rows[i][10].ToString();
            dr["信用卡有效月年"] = dtSrc.Rows[i][11].ToString();
            dr["授權狀態"] = dtSrc.Rows[i][12].ToString();
            dr["經手人"] = dtSrc.Rows[i][13].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //固定轉帳授權書_匯出
    public static DataTable PledgeQry_Print_Excel(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("授權編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("授權方式");
        dtRet.Columns.Add("指定用途");
        dtRet.Columns.Add("扣款金額");
        dtRet.Columns.Add("授權起日");
        dtRet.Columns.Add("授權迄日");
        dtRet.Columns.Add("轉帳週期");
        dtRet.Columns.Add("下次扣款日期");
        dtRet.Columns.Add("發卡銀行-卡別");
        dtRet.Columns.Add("卡號/帳號");
        dtRet.Columns.Add("信用卡有效月年");
        dtRet.Columns.Add("授權狀態");
        dtRet.Columns.Add("經手人");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["授權編號"] = dtSrc.Rows[i][0].ToString();
            dr["捐款人"] = dtSrc.Rows[i][1].ToString();
            dr["捐款人編號"] = dtSrc.Rows[i][2].ToString();
            dr["授權方式"] = dtSrc.Rows[i][3].ToString();
            dr["指定用途"] = dtSrc.Rows[i][4].ToString();
            dr["扣款金額"] = dtSrc.Rows[i][5].ToString();
            dr["授權起日"] = dtSrc.Rows[i][6].ToString();
            dr["授權迄日"] = dtSrc.Rows[i][7].ToString();
            dr["轉帳週期"] = dtSrc.Rows[i][8].ToString();
            dr["下次扣款日期"] = dtSrc.Rows[i][9].ToString();
            dr["發卡銀行-卡別"] = dtSrc.Rows[i][10].ToString();
            dr["卡號/帳號"] = dtSrc.Rows[i][11].ToString();
            dr["信用卡有效月年"] = dtSrc.Rows[i][12].ToString();
            dr["授權狀態"] = dtSrc.Rows[i][13].ToString();
            dr["經手人"] = dtSrc.Rows[i][14].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //奉獻徵信錄-style1
    public static DataTable VerifyList_Print_style1(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();
           
        dtRet.Columns.Add("捐款日期");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("捐款金額");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["捐款日期"] = dtSrc.Rows[i][0].ToString();
            dr["捐款人"] = dtSrc.Rows[i][1].ToString();
            dr["捐款金額"] = dtSrc.Rows[i][2].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //奉獻徵信錄-style2
    public static DataTable VerifyList_Print_style2(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款金額");
        dtRet.Columns.Add("捐款人");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["捐款金額"] = dtSrc.Rows[i][0].ToString();
            dr["捐款人"] = dtSrc.Rows[i][1].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //奉獻徵信錄-style3
    public static DataTable VerifyList_Print_style3(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("序號");
        dtRet.Columns.Add("捐款用途");
        dtRet.Columns.Add("捐款日期");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("捐款金額");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["序號"] = dtSrc.Rows[i][1].ToString();
            dr["捐款用途"] = dtSrc.Rows[i][2].ToString();
            dr["捐款日期"] = dtSrc.Rows[i][3].ToString();
            dr["捐款人"] = dtSrc.Rows[i][4].ToString();
            dr["捐款金額"] = dtSrc.Rows[i][5].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //捐款名單
    public static DataTable DonateNameList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("捐款日期");
        dtRet.Columns.Add("捐款金額");
        dtRet.Columns.Add("捐款方式");
        dtRet.Columns.Add("捐款用途");
        dtRet.Columns.Add("收據開立");
        dtRet.Columns.Add("收據編號");
        dtRet.Columns.Add("沖帳日期");
        dtRet.Columns.Add("捐款類別");
        dtRet.Columns.Add("入帳銀行");
        dtRet.Columns.Add("會計科目");
        dtRet.Columns.Add("活動名稱");
        dtRet.Columns.Add("狀態");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["捐款人"] = dtSrc.Rows[i][0].ToString();
            dr["捐款日期"] = dtSrc.Rows[i][1].ToString();
            dr["捐款金額"] = dtSrc.Rows[i][2].ToString();
            dr["捐款方式"] = dtSrc.Rows[i][3].ToString();
            dr["捐款用途"] = dtSrc.Rows[i][4].ToString();
            dr["收據開立"] = dtSrc.Rows[i][5].ToString();
            dr["收據編號"] = dtSrc.Rows[i][6].ToString();
            dr["沖帳日期"] = dtSrc.Rows[i][7].ToString();
            dr["捐款類別"] = dtSrc.Rows[i][8].ToString();
            dr["入帳銀行"] = dtSrc.Rows[i][9].ToString();
            dr["會計科目"] = dtSrc.Rows[i][10].ToString();
            dr["活動名稱"] = dtSrc.Rows[i][11].ToString();
            dr["狀態"] = dtSrc.Rows[i][12].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //捐款報表
    public static DataTable DonateMonthReport_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("序號");
        dtRet.Columns.Add("收據編號");
        dtRet.Columns.Add("捐款日期");
        dtRet.Columns.Add("捐款人(編號)");
        dtRet.Columns.Add("捐款金額");
        dtRet.Columns.Add("捐款用途");
        dtRet.Columns.Add("捐款方式");


        int row = dtSrc.Rows.Count;

        for (int i = 0; i <= row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row)
            {
                dr["序號"] = "執行長：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                dr["收據編號"] = "副執行長：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                dr["捐款日期"] = "部門主管：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                dr["捐款人(編號)"] = "";
                dr["捐款金額"] = "製表：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                dr["捐款用途"] = "";
                dr["捐款方式"] = "會計：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }
            //倒數第二列
            else if (i == row - 1)
            {
                dr["序號"] = dtSrc.Rows[i][1].ToString();
                dr["收據編號"] = dtSrc.Rows[i][2].ToString();
                dr["捐款日期"] = dtSrc.Rows[i][3].ToString();
                dr["捐款人(編號)"] = "捐款金額：";
                dr["捐款金額"] = dtSrc.Rows[i][5].ToString() != "" ? dtSrc.Rows[i][5].ToString() + "" : "0";
                dr["捐款用途"] = "";
                dr["捐款方式"] = "";
            }
            else
            {
                dr["序號"] = dtSrc.Rows[i][0].ToString();
                dr["收據編號"] = dtSrc.Rows[i][8].ToString() + dtSrc.Rows[i][1].ToString();
                dr["捐款日期"] = dtSrc.Rows[i][2].ToString();
                dr["捐款人(編號)"] = dtSrc.Rows[i][3].ToString() + "(" + Convert.ToInt32(dtSrc.Rows[i][4].ToString().Substring(0)).ToString("00000") + ")";
                dr["捐款金額"] = dtSrc.Rows[i][5].ToString() != "" ? dtSrc.Rows[i][5].ToString() + "" : "0";
                dr["捐款用途"] = dtSrc.Rows[i][6].ToString();
                dr["捐款方式"] = dtSrc.Rows[i][7].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //會計科目
    public static DataTable DonateAccounReport_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("&nbsp;&nbsp;會計科目");
        dtRet.Columns.Add("捐款金額");
        dtRet.Columns.Add("手續費");
        dtRet.Columns.Add("實收金額");
        dtRet.Columns.Add("捐款筆數");

        int col = dtSrc.Columns.Count;
        for (int i = 0; i < col; i+=5)
        {
            DataRow dr = dtRet.NewRow();
            if (i + 4 == col-1)
            {
                dr["&nbsp;&nbsp;會計科目"] = "&nbsp;&nbsp;列印日期：" + dtSrc.Rows[0][i].ToString()+"      &nbsp;&nbsp;&nbsp;總計：";
                dr["捐款金額"] = dtSrc.Rows[0][i + 1].ToString() != "" ? dtSrc.Rows[0][i + 1].ToString() + "元" : "0元";
                dr["手續費"] = dtSrc.Rows[0][i + 2].ToString() != "" ? dtSrc.Rows[0][i + 2].ToString() + "元" : "0元";
                dr["實收金額"] = dtSrc.Rows[0][i + 3].ToString() != "" ? dtSrc.Rows[0][i + 3].ToString() + "元" : "0元";
                dr["捐款筆數"] = dtSrc.Rows[0][i + 4].ToString();
            }
            else
            { 
                dr["&nbsp;&nbsp;會計科目"] = "&nbsp;&nbsp;" + dtSrc.Rows[0][i].ToString();
                dr["捐款金額"] = dtSrc.Rows[0][i + 1].ToString() != "" ? dtSrc.Rows[0][i + 1].ToString() + "元" : "0元";
                dr["手續費"] = dtSrc.Rows[0][i + 2].ToString() != "" ? dtSrc.Rows[0][i + 2].ToString() + "元" : "0元";
                dr["實收金額"] = dtSrc.Rows[0][i + 3].ToString() != "" ? dtSrc.Rows[0][i + 3].ToString() + "元" : "0元";
                dr["捐款筆數"] = dtSrc.Rows[0][i + 4].ToString() + "筆";
            }

            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //實物奉獻捐贈維護
    public static DataTable ContributeList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐贈人");
        dtRet.Columns.Add("捐款日期");
        dtRet.Columns.Add("折合現金");
        dtRet.Columns.Add("捐贈內容");
        dtRet.Columns.Add("捐款方式");
        dtRet.Columns.Add("捐款用途");
        dtRet.Columns.Add("收據開立");
        dtRet.Columns.Add("收據編號");
        dtRet.Columns.Add("列印");
        dtRet.Columns.Add("狀態");
        dtRet.Columns.Add("經手人");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["捐贈人"] = dtSrc.Rows[i][2].ToString();
            dr["捐款日期"] = dtSrc.Rows[i][3].ToString();
            dr["折合現金"] = dtSrc.Rows[i][4].ToString();
            dr["捐贈內容"] = dtSrc.Rows[i][12].ToString();
            dr["捐款方式"] = dtSrc.Rows[i][5].ToString();
            dr["捐款用途"] = dtSrc.Rows[i][6].ToString();
            dr["收據開立"] = dtSrc.Rows[i][7].ToString();
            dr["收據編號"] = dtSrc.Rows[i][8].ToString();
            dr["列印"] = dtSrc.Rows[i][9].ToString();
            dr["狀態"] = dtSrc.Rows[i][10].ToString();
            dr["經手人"] = dtSrc.Rows[i][11].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //實物奉獻領用作業
    public static DataTable ContributeIssueList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("領取人");
        dtRet.Columns.Add("領取日期");
        dtRet.Columns.Add("領取用途");
        dtRet.Columns.Add("領取編號");
        dtRet.Columns.Add("領用內容");
        dtRet.Columns.Add("列印");
        dtRet.Columns.Add("狀態");
        dtRet.Columns.Add("經手人");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["領取人"] = dtSrc.Rows[i][1].ToString();
            dr["領取日期"] = dtSrc.Rows[i][2].ToString();
            dr["領取用途"] = dtSrc.Rows[i][3].ToString();
            dr["領取編號"] = dtSrc.Rows[i][4].ToString();
            dr["領用內容"] = dtSrc.Rows[i][8].ToString();
            dr["列印"] = dtSrc.Rows[i][5].ToString();
            dr["狀態"] = dtSrc.Rows[i][6].ToString();
            dr["經手人"] = dtSrc.Rows[i][7].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //物品領用列印
    public static DataTable ContributeIssue_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("物品代號");
        dtRet.Columns.Add("物品名稱");
        dtRet.Columns.Add("數量單位");
        dtRet.Columns.Add("備註");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["物品代號"] = dtSrc.Rows[i][0].ToString();
            dr["物品名稱"] = dtSrc.Rows[i][1].ToString();
            dr["數量單位"] = dtSrc.Rows[i][2].ToString();
            dr["備註"] = dtSrc.Rows[i][3].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //信用卡交易查詢
    public static DataTable EcBankCardQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("交易方式");
        dtRet.Columns.Add("交易日期");
        dtRet.Columns.Add("交易序號");
        dtRet.Columns.Add("交易金額");
        dtRet.Columns.Add("交易狀態");
        dtRet.Columns.Add("授權狀態");
        dtRet.Columns.Add("授權碼");
        dtRet.Columns.Add("是否請款");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("聯絡電話");
        dtRet.Columns.Add("電子郵件");
        dtRet.Columns.Add("捐款用途");
        dtRet.Columns.Add("收據開立");
        dtRet.Columns.Add("收據抬頭");
        dtRet.Columns.Add("收據地址");
        dtRet.Columns.Add("通訊地址");
        dtRet.Columns.Add("性別");
        dtRet.Columns.Add("身分證");
        dtRet.Columns.Add("出生日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["捐款人"] = dtSrc.Rows[i][1].ToString();
            dr["交易方式"] = dtSrc.Rows[i][2].ToString();
            dr["交易日期"] = dtSrc.Rows[i][3].ToString();
            dr["交易序號"] = dtSrc.Rows[i][4].ToString();
            dr["交易金額"] = dtSrc.Rows[i][5].ToString();
            dr["交易狀態"] = dtSrc.Rows[i][6].ToString();
            dr["授權狀態"] = dtSrc.Rows[i][7].ToString();
            dr["授權碼"] = dtSrc.Rows[i][8].ToString();
            dr["是否請款"] = dtSrc.Rows[i][9].ToString();
            dr["手機"] = dtSrc.Rows[i][10].ToString();
            dr["聯絡電話"] = dtSrc.Rows[i][11].ToString();
            dr["電子郵件"] = dtSrc.Rows[i][12].ToString();
            dr["捐款用途"] = dtSrc.Rows[i][13].ToString();
            dr["收據開立"] = dtSrc.Rows[i][14].ToString();
            dr["收據抬頭"] = dtSrc.Rows[i][15].ToString();
            dr["收據地址"] = dtSrc.Rows[i][16].ToString();
            dr["通訊地址"] = dtSrc.Rows[i][17].ToString();
            dr["性別"] = dtSrc.Rows[i][18].ToString();
            dr["身分證"] = dtSrc.Rows[i][19].ToString();
            dr["出生日期"] = dtSrc.Rows[i][20].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //非信用卡交易查詢
    public static DataTable EcBankQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("交易方式");
        dtRet.Columns.Add("交易日期");
        dtRet.Columns.Add("交易編號");
        dtRet.Columns.Add("交易金額");
        dtRet.Columns.Add("繳費截止日期");
        dtRet.Columns.Add("繳費狀態");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("聯絡電話");
        dtRet.Columns.Add("電子郵件");
        dtRet.Columns.Add("捐款用途");
        dtRet.Columns.Add("收據開立");
        dtRet.Columns.Add("收據抬頭");
        dtRet.Columns.Add("收據地址");
        dtRet.Columns.Add("通訊地址");
        dtRet.Columns.Add("性別");
        dtRet.Columns.Add("身分證");
        dtRet.Columns.Add("出生日期");
        dtRet.Columns.Add("教育程度");
        dtRet.Columns.Add("職業");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["捐款人"] = dtSrc.Rows[i][1].ToString();
            dr["交易方式"] = dtSrc.Rows[i][2].ToString();
            dr["交易日期"] = dtSrc.Rows[i][3].ToString();
            dr["交易編號"] = dtSrc.Rows[i][4].ToString();
            dr["交易金額"] = dtSrc.Rows[i][5].ToString();
            dr["繳費截止日期"] = dtSrc.Rows[i][6].ToString();
            //20140411 修改by Ian_Kao
            //-----------------------------------------------
            dr["繳費狀態"] = dtSrc.Rows[i][7].ToString();
            //-----------------------------------------------
            dr["手機"] = dtSrc.Rows[i][8].ToString();
            dr["聯絡電話"] = dtSrc.Rows[i][9].ToString();
            dr["電子郵件"] = dtSrc.Rows[i][10].ToString();
            dr["捐款用途"] = dtSrc.Rows[i][11].ToString();
            dr["收據開立"] = dtSrc.Rows[i][12].ToString();
            dr["收據抬頭"] = dtSrc.Rows[i][13].ToString();
            dr["收據地址"] = dtSrc.Rows[i][14].ToString();
            dr["通訊地址"] = dtSrc.Rows[i][15].ToString();
            dr["性別"] = dtSrc.Rows[i][16].ToString();
            dr["身分證"] = dtSrc.Rows[i][17].ToString();
            dr["出生日期"] = dtSrc.Rows[i][18].ToString();
            dr["教育程度"] = dtSrc.Rows[i][19].ToString();
            dr["職業"] = dtSrc.Rows[i][20].ToString();
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //實物奉獻主檔維護
    public static DataTable GoodsList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("物品代號");
        dtRet.Columns.Add("物品名稱");
        dtRet.Columns.Add("物品性質");
        dtRet.Columns.Add("物品類別");
        dtRet.Columns.Add("現有庫存量");
        dtRet.Columns.Add("庫存單位");
        dtRet.Columns.Add("庫存管理");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["物品代號"] = dtSrc.Rows[i][0].ToString();
            dr["物品名稱"] = dtSrc.Rows[i][1].ToString();
            dr["物品性質"] = dtSrc.Rows[i][2].ToString();
            dr["物品類別"] = dtSrc.Rows[i][3].ToString();
            dr["現有庫存量"] = dtSrc.Rows[i][4].ToString();
            dr["庫存單位"] = dtSrc.Rows[i][5].ToString();
            if (dtSrc.Rows[i][6].ToString() == "V")
            {
                dr["庫存管理"] = "是";
            }
            else if (dtSrc.Rows[i][6].ToString() == "")
            {
                dr["庫存管理"] = "否";
            }
            dtRet.Rows.Add(dr);
        }
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //讀者資料維護 
    public static DataTable MemberQry_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("編號");
        dtRet.Columns.Add("讀者姓名");
        dtRet.Columns.Add("身份別");
        dtRet.Columns.Add("連絡電話");
        dtRet.Columns.Add("手機號碼");
        dtRet.Columns.Add("通訊地址");
        dtRet.Columns.Add("電子文宣");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["編號"] = dtSrc.Rows[i][1].ToString();
            dr["讀者姓名"] = dtSrc.Rows[i][2].ToString();
            dr["身份別"] = dtSrc.Rows[i][3].ToString();
            dr["連絡電話"] = dtSrc.Rows[i][4].ToString();
            dr["手機號碼"] = dtSrc.Rows[i][5].ToString();
            dr["通訊地址"] = dtSrc.Rows[i][6].ToString();
            dr["電子文宣"] = dtSrc.Rows[i][7].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //讀者名冊
    public static DataTable MemberNameList_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("編號");
        dtRet.Columns.Add("讀者姓名");
        dtRet.Columns.Add("狀態");
        dtRet.Columns.Add("性別");
        dtRet.Columns.Add("身份別");
        dtRet.Columns.Add("聯絡電話日");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("聯絡電話夜");
        dtRet.Columns.Add("電子信箱");
        dtRet.Columns.Add("通訊地址");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["編號"] = dtSrc.Rows[i][1].ToString();
            dr["讀者姓名"] = dtSrc.Rows[i][2].ToString();
            dr["狀態"] = dtSrc.Rows[i][3].ToString();
            dr["性別"] = dtSrc.Rows[i][4].ToString();
            dr["身份別"] = dtSrc.Rows[i][5].ToString();
            dr["聯絡電話日"] = dtSrc.Rows[i][6].ToString();
            dr["手機"] = dtSrc.Rows[i][7].ToString();
            dr["聯絡電話夜"] = dtSrc.Rows[i][8].ToString();
            dr["電子信箱"] = dtSrc.Rows[i][9].ToString();
            dr["通訊地址"] = dtSrc.Rows[i][10].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //讀者名冊_匯出
    public static DataTable MemberNameList_Print_Excel(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("編號");
        dtRet.Columns.Add("讀者姓名");
        dtRet.Columns.Add("狀態");
        dtRet.Columns.Add("性別");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身份別");
        dtRet.Columns.Add("身分證統編");
        dtRet.Columns.Add("出生日期");
        dtRet.Columns.Add("教育程度");
        dtRet.Columns.Add("職業別");
        dtRet.Columns.Add("婚姻狀況");
        dtRet.Columns.Add("宗教信仰");
        dtRet.Columns.Add("所屬教會");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("電話日");
        dtRet.Columns.Add("電話夜");
        dtRet.Columns.Add("電子信箱");
        dtRet.Columns.Add("聯絡人");
        dtRet.Columns.Add("服務單位");
        dtRet.Columns.Add("職稱");
        dtRet.Columns.Add("通訊地址");
        dtRet.Columns.Add("海外地址");
        dtRet.Columns.Add("紙本月刊");
        dtRet.Columns.Add("電子報");


        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["編號"] = dtSrc.Rows[i][1].ToString();
            dr["讀者姓名"] = dtSrc.Rows[i][2].ToString();
            dr["狀態"] = dtSrc.Rows[i][3].ToString();
            dr["性別"] = dtSrc.Rows[i][4].ToString();
            dr["稱謂"] = dtSrc.Rows[i][5].ToString();
            dr["身份別"] = dtSrc.Rows[i][6].ToString();
            dr["身分證統編"] = dtSrc.Rows[i][7].ToString();
            dr["出生日期"] = dtSrc.Rows[i][8].ToString() != "" ? DateTime.Parse(dtSrc.Rows[i][8].ToString().Trim()).ToShortDateString().ToString() : "";
            dr["教育程度"] = dtSrc.Rows[i][9].ToString();
            dr["職業別"] = dtSrc.Rows[i][10].ToString();
            dr["婚姻狀況"] = dtSrc.Rows[i][11].ToString();
            dr["宗教信仰"] = dtSrc.Rows[i][12].ToString();
            dr["所屬教會"] = dtSrc.Rows[i][13].ToString();
            dr["手機"] = dtSrc.Rows[i][14].ToString();
            dr["電話日"] = dtSrc.Rows[i][15].ToString();
            dr["電話夜"] = dtSrc.Rows[i][16].ToString();
            dr["電子信箱"] = dtSrc.Rows[i][17].ToString();
            dr["聯絡人"] = dtSrc.Rows[i][18].ToString();
            dr["服務單位"] = dtSrc.Rows[i][19].ToString();
            dr["職稱"] = dtSrc.Rows[i][20].ToString();
            dr["通訊地址"] = dtSrc.Rows[i][21].ToString();
            dr["海外地址"] = dtSrc.Rows[i][22].ToString();
            dr["紙本月刊"] = dtSrc.Rows[i][23].ToString();
            dr["電子報"] = dtSrc.Rows[i][24].ToString();
            
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //讀者統計
    public static DataTable MemberReport_Condition(DataTable dtSrc, String Condition)
    {
        DataTable dtRet = new DataTable();
        if (Condition == "1")
        {
            dtRet.Columns.Add("類別統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");

            string[] cla = { "學生", "個人", "團體", "類別不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["類別統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "2")
        {
            dtRet.Columns.Add("性別統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "男", "女", "歿", "性別不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["性別統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "3")
        {
            dtRet.Columns.Add("年齡統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "20歲以下", "21 ～ 25歲", "26 ～ 30歲", "31 ～ 35歲", "36 ～ 40歲", "41 ～ 45歲", "46 ～ 50歲", "51 ～ 55歲", "56 ～ 60歲", "61 ～ 65歲", "66 ～ 70歲", "71歲以上", "年齡不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["年齡統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "4")
        {
            dtRet.Columns.Add("教育程度統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "不識字", "國小", "國中", "高中", "大學", "碩士", "博士", "博士後研究", "教育程度不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["教育程度統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "5")
        {
            dtRet.Columns.Add("職業別統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "公教", "軍警", "學生", "農", "公", "商", "家管", "服務", "自由", "醫護", "退休", "其他", "職業別不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["職業別統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "6")
        {
            dtRet.Columns.Add("婚姻狀況統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "未婚", "已婚", "分居", "離婚", "喪偶", "其他", "婚姻狀況不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["婚姻狀況統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "7")
        {
            dtRet.Columns.Add("宗教信仰統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "基督教", "宗教信仰不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["宗教信仰統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "8")
        {
            dtRet.Columns.Add("通訊縣市統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "基隆市", "台北市", "新北市", "桃園縣", "新竹市", "新竹縣", "苗栗縣", "台中市", "彰化縣", "南投縣", "雲林縣", "嘉義市", "嘉義縣", "台南市", "高雄市", "屏東縣", "宜蘭縣", "花蓮縣", "台東縣", "澎湖縣", "金門縣", "連江縣", "通訊縣市不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["通訊縣市統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        else if (Condition == "9")
        {
            dtRet.Columns.Add("狀態統計");
            dtRet.Columns.Add("人數");
            dtRet.Columns.Add("百分比");


            string[] cla = { "新入會", "服務中", "停權", "除名", "退會", "註銷", "死亡", "狀態不詳", "合計" };

            int count = 0;
            for (int i = 0; i < cla.Length; i++)
            {
                DataRow dr = dtRet.NewRow();
                dr["狀態統計"] = cla[i];
                dr["人數"] = dtSrc.Rows[0][count].ToString();
                dr["百分比"] = dtSrc.Rows[0][count + 1].ToString() + "%";
                dtRet.Rows.Add(dr);
                count += 2;
            }
        }
        return dtRet;
    }
    //20140205客製報表
    //---------------------------------------------------------------------------
    //建台/非建台奉獻級距表
    public static DataTable Donate_Week_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("序號");
        dtRet.Columns.Add("級距");
        dtRet.Columns.Add("筆數");
        dtRet.Columns.Add("小計");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["序號"] = dtSrc.Rows[i][0].ToString();
                dr["級距"] = DateTime.Now.ToString();
                dr["筆數"] = "";
                dr["小計"] = "";
            }
            else
            {
                dr["序號"] = dtSrc.Rows[i][0].ToString();
                dr["級距"] = dtSrc.Rows[i][1].ToString();
                dr["筆數"] = dtSrc.Rows[i][2].ToString();
                dr["小計"] = dtSrc.Rows[i][3].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //新捐款人明細表
    public static DataTable Donate_Week_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款人姓名");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻<br>次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("月刊寄送");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = dtSrc.Rows[i][1].ToString();
                dr["捐款人姓名"] = dtSrc.Rows[i][4].ToString() + dtSrc.Rows[i][3].ToString();
                dr["首捐日期"] = "";
                dr["累計奉獻<br>金額"] = "";
                dr["奉獻<br>次數"] = "";
                dr["郵遞<br>區號"] = dtSrc.Rows[i][5].ToString();
                dr["地址"] = dtSrc.Rows[i][6].ToString() + dtSrc.Rows[i][7].ToString();
                dr["電話"] = "";
                dr["手機"] = "";
                dr["月刊寄送"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["捐款人姓名"] = dtSrc.Rows[i][1].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][2].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][3].ToString();
                dr["奉獻<br>次數"] = dtSrc.Rows[i][4].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][5].ToString();
                dr["地址"] = dtSrc.Rows[i][6].ToString();
                dr["電話"] = dtSrc.Rows[i][7].ToString();
                dr["手機"] = dtSrc.Rows[i][8].ToString();
                dr["月刊寄送"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //查詢單筆捐款金額
    public static DataTable Donate_Week_Report3_Print(DataTable dtSrc, DataTable dtSrc2,bool XLS)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人<br>編號");
        dtRet.Columns.Add("捐款<br>日期");
        dtRet.Columns.Add("天使<br>姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("捐款<br>金額");
        dtRet.Columns.Add("舊/新<br>大額");
        dtRet.Columns.Add("首捐<br>日期");
        dtRet.Columns.Add("累計<br>金額");
        dtRet.Columns.Add("捐款<br>用途");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i <= row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row)
            {
                if (XLS == true)
                {
                    dr["捐款人<br>編號"] = "總筆數：";
                    dr["捐款<br>日期"] = dtSrc2.Rows[0][0].ToString() + "筆";
                    dr["天使<br>姓名"] = "";
                    dr["稱謂"] = "";
                    dr["捐款<br>金額"] = "";
                    dr["舊/新<br>大額"] = "";
                    dr["首捐<br>日期"] = "";
                    dr["累計<br>金額"] = "";
                    dr["捐款<br>用途"] = "";
                    dr["郵遞<br>區號"] = "總計";
                    dr["地址"] = "捐款金額：" + dtSrc2.Rows[0][1].ToString() + "元";
                    dr["電話"] = "";
                    dr["手機"] = "";
                    

                }
                else
                {
                    dr["捐款人<br>編號"] = "總筆數：";
                    dr["捐款<br>日期"] = dtSrc2.Rows[0][0].ToString() + "筆";
                    dr["天使<br>姓名"] = "";
                    dr["稱謂"] = "";
                    dr["捐款<br>金額"] = "";
                    dr["舊/新<br>大額"] = "";
                    dr["首捐<br>日期"] = "";
                    dr["累計<br>金額"] = "";
                    dr["捐款<br>用途"] = "總計";
                    dr["郵遞<br>區號"] = "捐款金額：";
                    dr["地址"] = dtSrc2.Rows[0][1].ToString() + "元";
                    dr["電話"] = "";
                    dr["手機"] = "";

                }
            }
            else
            {
                dr["捐款人<br>編號"] = dtSrc.Rows[i][0].ToString();
                dr["捐款<br>日期"] = dtSrc.Rows[i][1].ToString();
                dr["天使<br>姓名"] = dtSrc.Rows[i][2].ToString();
                dr["稱謂"] = dtSrc.Rows[i][3].ToString();
                dr["捐款<br>金額"] = dtSrc.Rows[i][4].ToString();
                dr["舊/新<br>大額"] = dtSrc.Rows[i][5].ToString();
                dr["首捐<br>日期"] = dtSrc.Rows[i][6].ToString();
                dr["累計<br>金額"] = dtSrc.Rows[i][7].ToString();
                dr["捐款<br>用途"] = dtSrc.Rows[i][8].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][9].ToString();
                dr["地址"] = dtSrc.Rows[i][10].ToString();
                dr["電話"] = dtSrc.Rows[i][11].ToString();
                dr["手機"] = dtSrc.Rows[i][12].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //查詢單筆捐款金額(新舊天使)
    public static DataTable Donate_Week_Report4_Print(DataTable dtSrc, DataTable dtSrc2, bool XLS)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款日期");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("捐款金額");
        dtRet.Columns.Add("新/舊天使");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("個人累計金額");
        dtRet.Columns.Add("捐款用途");
        dtRet.Columns.Add("郵遞區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");

        int row = dtSrc.Rows.Count;

        for (int i = 0; i <= row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row)
            {
                if (XLS == true)
                {
                    dr["捐款人編號"] = "總筆數：";
                    dr["捐款日期"] = dtSrc2.Rows[0][0].ToString() + "筆";
                    dr["天使姓名"] = "";
                    dr["稱謂"] = "";
                    dr["捐款金額"] = "";
                    dr["新/舊天使"] = "";
                    dr["首捐日期"] = "";
                    dr["個人累計金額"] = "";
                    dr["捐款用途"] = "";
                    dr["郵遞區號"] = "總計";
                    dr["地址"] = "捐款金額：" + dtSrc2.Rows[0][1].ToString() + "元";
                    dr["電話"] = "";
                    dr["手機"] = "";


                }
                else
                {
                    dr["捐款人編號"] = "總筆數：";
                    dr["捐款日期"] = dtSrc2.Rows[0][0].ToString() + "筆";
                    dr["天使姓名"] = "";
                    dr["稱謂"] = "";
                    dr["捐款金額"] = "";
                    dr["新/舊天使"] = "";
                    dr["首捐日期"] = "";
                    dr["個人累計金額"] = "";
                    dr["捐款用途"] = "總計";
                    dr["郵遞區號"] = "捐款金額：";
                    dr["地址"] = dtSrc2.Rows[0][1].ToString() + "元";
                    dr["電話"] = "";
                    dr["手機"] = "";

                }
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["捐款日期"] = dtSrc.Rows[i][1].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][2].ToString();
                dr["稱謂"] = dtSrc.Rows[i][3].ToString();
                dr["捐款金額"] = dtSrc.Rows[i][4].ToString();
                dr["新/舊天使"] = dtSrc.Rows[i][5].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][6].ToString();
                dr["個人累計金額"] = dtSrc.Rows[i][7].ToString();
                dr["捐款用途"] = dtSrc.Rows[i][8].ToString();
                dr["郵遞區號"] = dtSrc.Rows[i][9].ToString();
                dr["地址"] = dtSrc.Rows[i][10].ToString();
                dr["電話"] = dtSrc.Rows[i][11].ToString();
                dr["手機"] = dtSrc.Rows[i][12].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //大額捐款人生日查詢表
    public static DataTable Donate_Month_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻<br>次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("生日");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = dtSrc.Rows[i][1].ToString();
                dr["捐款人"] = dtSrc.Rows[i][5].ToString() + dtSrc.Rows[i][2].ToString();
                dr["稱謂"] = "";
                dr["身分別"] = "";
                dr["累計奉獻<br>金額"] = "";
                dr["奉獻<br>次數"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
                dr["電話"] = "";
                dr["手機"] = "";
                dr["生日"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["捐款人"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["身分別"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻<br>次數"] = dtSrc.Rows[i][5].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][6].ToString();
                dr["地址"] = dtSrc.Rows[i][7].ToString();
                dr["電話"] = dtSrc.Rows[i][8].ToString();
                dr["手機"] = dtSrc.Rows[i][9].ToString();
                dr["生日"] = dtSrc.Rows[i][10].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //定期奉獻授權到期明細表
    public static DataTable Donate_Month_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("新授權<br>書號");
        dtRet.Columns.Add("授權迄日");
        dtRet.Columns.Add("新授權額");
        dtRet.Columns.Add("授權方式");
        dtRet.Columns.Add("銀行");
        dtRet.Columns.Add("卡別");
        dtRet.Columns.Add("授權<br>書號");
        dtRet.Columns.Add("捐款人<br>編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["新授權<br>書號"] = dtSrc.Rows[i][0].ToString() + dtSrc.Rows[i][1].ToString() + dtSrc.Rows[i][4].ToString();
                dr["授權迄日"] = "";
                dr["新授權額"] = "";
                dr["授權方式"] = "";
                dr["銀行"] = "";
                dr["卡別"] = "";
                dr["授權<br>書號"] = "";
                dr["捐款人<br>編號"] = "";
                dr["天使姓名"] = "";
                dr["電話"] = "";
                dr["手機"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
            }
            else
            {
                dr["新授權<br>書號"] = dtSrc.Rows[i][1].ToString();
                dr["授權迄日"] = dtSrc.Rows[i][2].ToString();
                dr["新授權額"] = dtSrc.Rows[i][3].ToString();
                dr["授權方式"] = dtSrc.Rows[i][4].ToString();
                dr["銀行"] = dtSrc.Rows[i][5].ToString();
                dr["卡別"] = dtSrc.Rows[i][6].ToString();
                dr["授權<br>書號"] = dtSrc.Rows[i][7].ToString();
                dr["捐款人<br>編號"] = dtSrc.Rows[i][8].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][9].ToString();
                dr["電話"] = dtSrc.Rows[i][10].ToString();
                dr["手機"] = dtSrc.Rows[i][11].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][12].ToString();
                dr["地址"] = dtSrc.Rows[i][13].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //郵寄月刊明細表
    public static DataTable Donate_Month_Report3_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("郵遞區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("月刊份數");
        dtRet.Columns.Add("不主動聯絡");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][1].ToString() + "筆";
                dr["捐款人"] = "";
                dr["郵遞區號"] = "";
                dr["地址"] = "";
                dr["身分別"] = "";
                dr["月刊份數"] = "";
                dr["不主動聯絡"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["捐款人"] = dtSrc.Rows[i][1].ToString();
                dr["郵遞區號"] = dtSrc.Rows[i][2].ToString();
                dr["地址"] = dtSrc.Rows[i][3].ToString();
                dr["身分別"] = dtSrc.Rows[i][4].ToString();
                dr["月刊份數"] = dtSrc.Rows[i][5].ToString();
                dr["不主動聯絡"] = dtSrc.Rows[i][6].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //郵寄電子報明細表
    public static DataTable Donate_Month_Report4_Print(DataTable dtSrc,bool XLS)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人<br>編號");
        dtRet.Columns.Add("天使<br>姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("累積奉獻金額");
        dtRet.Columns.Add("奉獻<br>次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("Email");
        dtRet.Columns.Add("首捐<br>日期");
        dtRet.Columns.Add("末捐<br>日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                if (XLS == true)
                {
                    dr["捐款人<br>編號"] = dtSrc.Rows[i][1].ToString();
                    dr["天使<br>姓名"] = dtSrc.Rows[i][0].ToString() + dtSrc.Rows[i][2].ToString();
                    dr["稱謂"] = "";
                    dr["身分別"] = "";
                    dr["累積奉獻金額"] = "";
                    dr["奉獻<br>次數"] = "";
                    dr["郵遞<br>區號"] = "";
                    dr["地址"] = "";
                    dr["電話"] = dtSrc.Rows[i][6].ToString() + " " +  dtSrc.Rows[i][7].ToString();
                    dr["手機"] = dtSrc.Rows[i][8].ToString() + dtSrc.Rows[i][9].ToString();
                    dr["Email"] = "";
                    dr["首捐<br>日期"] = "";
                    dr["末捐<br>日期"] = "";
                }
                else
                {
                    dr["捐款人<br>編號"] = dtSrc.Rows[i][1].ToString();
                    dr["天使<br>姓名"] = dtSrc.Rows[i][0].ToString() + dtSrc.Rows[i][2].ToString();
                    dr["稱謂"] = "";
                    dr["身分別"] = "";
                    dr["累積奉獻金額"] = "";
                    dr["奉獻<br>次數"] = "";
                    dr["郵遞<br>區號"] = "";
                    dr["地址"] = dtSrc.Rows[i][6].ToString();
                    dr["電話"] = dtSrc.Rows[i][7].ToString();
                    dr["手機"] = dtSrc.Rows[i][8].ToString() + dtSrc.Rows[i][9].ToString();
                    dr["Email"] = "";
                    dr["首捐<br>日期"] = "";
                    dr["末捐<br>日期"] = "";
                }
            }
            else
            {
                dr["捐款人<br>編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使<br>姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["身分別"] = dtSrc.Rows[i][3].ToString();
                dr["累積奉獻金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻<br>次數"] = dtSrc.Rows[i][5].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][6].ToString();
                dr["地址"] = dtSrc.Rows[i][7].ToString();
                dr["電話"] = dtSrc.Rows[i][8].ToString();
                dr["手機"] = dtSrc.Rows[i][9].ToString();
                dr["Email"] = dtSrc.Rows[i][10].ToString();
                dr["首捐<br>日期"] = dtSrc.Rows[i][11].ToString();
                dr["末捐<br>日期"] = dtSrc.Rows[i][12].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //當月各日捐款方式統計表
    public static DataTable Donate_Month_Report5_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("日期");
        dtRet.Columns.Add("現金");
        dtRet.Columns.Add("劃撥");
        dtRet.Columns.Add("信用卡授權書<br>(一般)");
        dtRet.Columns.Add("郵局轉帳");
        dtRet.Columns.Add("匯款");
        dtRet.Columns.Add("支票");
        dtRet.Columns.Add("實物奉獻");
        dtRet.Columns.Add("ATM");
        dtRet.Columns.Add("網路信用卡");
        dtRet.Columns.Add("ACH");
        dtRet.Columns.Add("美國運通");
        dtRet.Columns.Add("小計");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();

                dr["日期"] = dtSrc.Rows[i][0].ToString();
                dr["現金"] = dtSrc.Rows[i][1].ToString();
                dr["劃撥"] = dtSrc.Rows[i][2].ToString();
                dr["信用卡授權書<br>(一般)"] = dtSrc.Rows[i][3].ToString();
                dr["郵局轉帳"] = dtSrc.Rows[i][4].ToString();
                dr["匯款"] = dtSrc.Rows[i][5].ToString();
                dr["支票"] = dtSrc.Rows[i][6].ToString();
                dr["實物奉獻"] = dtSrc.Rows[i][7].ToString();
                dr["ATM"] = dtSrc.Rows[i][8].ToString();
                dr["網路信用卡"] = dtSrc.Rows[i][9].ToString();
                dr["ACH"] = dtSrc.Rows[i][10].ToString();
                dr["美國運通"] = dtSrc.Rows[i][11].ToString();
                dr["小計"] = dtSrc.Rows[i][12].ToString();
            
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //非建台奉獻統計分析表
    public static DataTable Donate_Season_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("序號");
        dtRet.Columns.Add("人數");
        dtRet.Columns.Add("筆數");
        dtRet.Columns.Add("總金額");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["序號"] = dtSrc.Rows[i][1].ToString() + dtSrc.Rows[i][3].ToString();
                dr["人數"] = "";
                dr["筆數"] = "";
                dr["總金額"] = "";
            }
            else
            {
                dr["序號"] = dtSrc.Rows[i][0].ToString();
                dr["人數"] = dtSrc.Rows[i][1].ToString() + "人";
                dr["筆數"] = dtSrc.Rows[i][2].ToString();
                dr["總金額"] = dtSrc.Rows[i][3].ToString() + "元";
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //查詢單筆捐款金額累計
    public static DataTable Donate_Season_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("最高奉獻金額");
        dtRet.Columns.Add("累計奉獻金額");
        dtRet.Columns.Add("奉獻<br>次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("末捐日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][0].ToString() + "筆";
                dr["天使姓名"] = "";
                dr["稱謂"] = "";
                dr["最高奉獻金額"] = "總計 捐款金額：";
                dr["累計奉獻金額"] = dtSrc.Rows[i][3].ToString() + "元";
                dr["奉獻<br>次數"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
                dr["首捐日期"] = "";
                dr["末捐日期"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["最高奉獻金額"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻<br>次數"] = dtSrc.Rows[i][5].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][6].ToString();
                dr["地址"] = dtSrc.Rows[i][7].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][8].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //捐款用途各項總額明細表
    public static DataTable Donate_Season_Report3_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("奉獻用途");
        dtRet.Columns.Add("累計奉獻金額");
        dtRet.Columns.Add("奉獻<br>次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("生日");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("末捐日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["編號"] = "列印日期：" ;
                dr["天使姓名"] = DateTime.Now.ToShortDateString();
                dr["稱謂"] = DateTime.Now.ToShortTimeString();
                dr["身分別"] = "";
                dr["奉獻用途"] = "";
                dr["累計奉獻金額"] = "";
                dr["奉獻<br>次數"] = "";
                dr["郵遞<br>區號"] = "總計 ";
                dr["地址"] = "捐款金額：" + dtSrc.Rows[i][1].ToString() + "元";
                dr["電話"] = "";
                dr["手機"] = "";
                dr["生日"] = "";
                dr["首捐日期"] = "";
                dr["末捐日期"] = "";
            }
            else
            {
                dr["編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["身分別"] = dtSrc.Rows[i][3].ToString();
                dr["奉獻用途"] = dtSrc.Rows[i][4].ToString();
                dr["累計奉獻金額"] = dtSrc.Rows[i][5].ToString();
                dr["奉獻<br>次數"] = dtSrc.Rows[i][6].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][7].ToString();
                dr["地址"] = dtSrc.Rows[i][8].ToString();
                dr["電話"] = dtSrc.Rows[i][9].ToString();
                dr["手機"] = dtSrc.Rows[i][10].ToString();
                dr["生日"] = dtSrc.Rows[i][11].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][12].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][13].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //捐款總額明細表
    public static DataTable Donate_Season_Report4_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻<br>次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("末捐日期");
        dtRet.Columns.Add("刊物<br>原則");
        dtRet.Columns.Add("月刊<br>份數");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][0].ToString() + "筆";
                dr["天使姓名"] = "";
                dr["稱謂"] = "總計 ";
                dr["身分別"] = "捐款金額：";
                dr["累計奉獻<br>金額"] =  dtSrc.Rows[i][1].ToString() + "元";
                dr["奉獻<br>次數"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
                dr["電話"] = "";
                dr["手機"] = "";
                dr["首捐日期"] = "";
                dr["末捐日期"] = "";
                dr["刊物<br>原則"] = "";
                dr["月刊<br>份數"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["身分別"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻<br>次數"] = dtSrc.Rows[i][5].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][6].ToString();
                dr["地址"] = dtSrc.Rows[i][7].ToString();
                dr["電話"] = dtSrc.Rows[i][8].ToString();
                dr["手機"] = dtSrc.Rows[i][9].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][10].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][11].ToString();
                dr["刊物<br>原則"] = dtSrc.Rows[i][12].ToString();
                dr["月刊<br>份數"] = dtSrc.Rows[i][13].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //捐款總額明細表
    public static DataTable Donate_Year_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻<br>次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("末捐日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][1].ToString() + "筆";
                dr["天使姓名"] = "";
                dr["稱謂"] = "總計 捐款金額：";
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][2].ToString() + "元";
                dr["奉獻<br>次數"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
                dr["首捐日期"] = "";
                dr["末捐日期"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][3].ToString();
                dr["奉獻<br>次數"] = dtSrc.Rows[i][4].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][5].ToString();
                dr["地址"] = dtSrc.Rows[i][6].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][7].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][8].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //捐款總額與月刊索取明細表
    public static DataTable Donate_Year_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻<br>次數");
        dtRet.Columns.Add("月刊<br>份數");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("末捐日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][0].ToString() + "筆";
                dr["捐款人"] = "";
                dr["稱謂"] = "";
                dr["身分別"] = "總計 捐款金額：";
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][1].ToString() + "元";
                dr["奉獻<br>次數"] = "";
                dr["月刊<br>份數"] = "";
                dr["地址"] = "";
                dr["首捐日期"] = "";
                dr["末捐日期"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["捐款人"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["身分別"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻<br>次數"] = dtSrc.Rows[i][5].ToString();
                dr["月刊<br>份數"] = dtSrc.Rows[i][6].ToString();
                dr["地址"] = dtSrc.Rows[i][7].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][8].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //海外捐款總額明細表
    public static DataTable Donate_Year_Report3_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("最高奉獻<br>金額");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻次數");
        dtRet.Columns.Add("捐款方式");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("國別");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][1].ToString() + "筆";
                dr["天使姓名"] = "";
                dr["稱謂"] = "";
                dr["最高奉獻<br>金額"] = "總計 捐款金額：";
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][2].ToString() + "元";
                dr["奉獻次數"] = "";
                dr["捐款方式"] = "";
                dr["地址"] = "";
                dr["國別"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["最高奉獻<br>金額"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻次數"] = dtSrc.Rows[i][5].ToString();
                dr["捐款方式"] = dtSrc.Rows[i][6].ToString();
                dr["地址"] = dtSrc.Rows[i][7].ToString();
                dr["國別"] = dtSrc.Rows[i][8].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //國內地區捐款總額明細表
    public static DataTable Donate_Year_Report4_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("累計奉獻金額");
        dtRet.Columns.Add("奉獻次數");
        dtRet.Columns.Add("郵遞區號");
        dtRet.Columns.Add("地址");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][1].ToString() + "筆";
                dr["天使姓名"] = "";
                dr["稱謂"] = "總計 捐款金額：";
                dr["累計奉獻金額"] = dtSrc.Rows[i][2].ToString() + "元";
                dr["奉獻次數"] = "";
                dr["郵遞區號"] = "";
                dr["地址"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["累計奉獻金額"] = dtSrc.Rows[i][3].ToString();
                dr["奉獻次數"] = dtSrc.Rows[i][4].ToString();
                dr["郵遞區號"] = dtSrc.Rows[i][5].ToString();
                dr["地址"] = dtSrc.Rows[i][6].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //期間身份別捐款總額明細表
    public static DataTable Donate_Year_Report5_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("末捐日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][1].ToString() + "筆";
                dr["捐款人"] = "";
                dr["稱謂"] = "總計";
                dr["身分別"] = "捐款金額：";
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][2].ToString() + "元";
                dr["奉獻次數"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
                dr["首捐日期"] = "";
                dr["末捐日期"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["捐款人"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["身分別"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻次數"] = dtSrc.Rows[i][5].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][6].ToString();
                dr["地址"] = dtSrc.Rows[i][7].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][8].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //期間單筆捐款金額次數明細表
    public static DataTable Donate_Year_Report6_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("末捐日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][1].ToString() + "筆";
                dr["捐款人"] = "";
                dr["稱謂"] = "總計";
                dr["身分別"] = "捐款金額：";
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][2].ToString() + "元";
                dr["奉獻次數"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
                dr["末捐日期"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["捐款人"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["身分別"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻次數"] = dtSrc.Rows[i][5].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][6].ToString();
                dr["地址"] = dtSrc.Rows[i][7].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][8].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //新增捐款人總額明細表
    public static DataTable Donate_Year_Report7_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款人");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("最高奉獻<br>金額");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻次數");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("末捐日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][1].ToString() + "筆";
                dr["捐款人"] = "";
                dr["稱謂"] = "總計";
                dr["最高奉獻<br>金額"] = "捐款金額：";
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][2].ToString() + "元";
                dr["奉獻次數"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
                dr["首捐日期"] = "";
                dr["末捐日期"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["捐款人"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["最高奉獻<br>金額"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻次數"] = dtSrc.Rows[i][5].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][6].ToString();
                dr["地址"] = dtSrc.Rows[i][7].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][8].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][9].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //經常奉獻總額明細表
    public static DataTable Donate_Year_Report8_Print(DataTable dtSrc,bool XLS)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人<br>編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("累計奉獻<br>金額");
        dtRet.Columns.Add("奉獻<br>次數");
        dtRet.Columns.Add("奉獻用途");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("末捐日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                if (XLS == true)
                {
                    dr["捐款人<br>編號"] = "總筆數：";
                    dr["天使姓名"] = dtSrc.Rows[i][1].ToString() + "筆";
                    dr["稱謂"] = "";
                    dr["身分別"] = "";
                    dr["累計奉獻<br>金額"] = "";
                    dr["奉獻<br>次數"] = "";
                    dr["奉獻用途"] = "";
                    dr["郵遞<br>區號"] = "總計";
                    dr["地址"] = "&nbsp;&nbsp;捐款金額：" + dtSrc.Rows[i][2].ToString() + "元";
                    dr["電話"] = "";
                    dr["手機"] = "";
                    dr["首捐日期"] = "";
                    dr["末捐日期"] = "";
                }
                else
                {
                    dr["捐款人<br>編號"] = "總筆數：";
                    dr["天使姓名"] = dtSrc.Rows[i][1].ToString() + "筆";
                    dr["稱謂"] = "";
                    dr["身分別"] = "";
                    dr["累計奉獻<br>金額"] = "";
                    dr["奉獻<br>次數"] = "";
                    dr["奉獻用途"] = "";
                    dr["郵遞<br>區號"] = "總計";
                    dr["地址"] = "&nbsp;&nbsp;捐款金額：";
                    dr["電話"] = dtSrc.Rows[i][2].ToString() + "元";
                    dr["手機"] = "";
                    dr["首捐日期"] = "";
                    dr["末捐日期"] = "";
                }
            }
            else
            {
                dr["捐款人<br>編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["身分別"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻<br>金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻<br>次數"] = dtSrc.Rows[i][5].ToString();
                dr["奉獻用途"] = dtSrc.Rows[i][6].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][7].ToString();
                dr["地址"] = dtSrc.Rows[i][8].ToString();
                dr["電話"] = dtSrc.Rows[i][9].ToString();
                dr["手機"] = dtSrc.Rows[i][10].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][11].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][12].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //歷年來大陸奉獻天使累計奉獻明細表
    public static DataTable Donate_Year_Report9_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("身分別");
        dtRet.Columns.Add("累計奉獻金額");
        dtRet.Columns.Add("奉獻次數");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("首捐日期");
        dtRet.Columns.Add("末捐日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][1].ToString() + "筆";
                dr["天使姓名"] = "";
                dr["稱謂"] = "總計  捐款金額：";
                dr["身分別"] = dtSrc.Rows[i][2].ToString() + "元";
                dr["累計奉獻金額"] = "";
                dr["奉獻次數"] = "";
                dr["地址"] = "";
                dr["首捐日期"] = "";
                dr["末捐日期"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["身分別"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻金額"] = dtSrc.Rows[i][4].ToString();
                dr["奉獻次數"] = dtSrc.Rows[i][5].ToString();
                dr["地址"] = dtSrc.Rows[i][6].ToString();
                dr["首捐日期"] = dtSrc.Rows[i][7].ToString();
                dr["末捐日期"] = dtSrc.Rows[i][8].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //-------------------------------------------------------------------------------------------------------------
    //20140418 新增 by Ian_Kao & 修改 by 詩儀
    //-------------------------------------------------------------------------------------------------------------
    //人數統計查詢報表
    public static DataTable Donate_Other_Report1_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("序號");
        dtRet.Columns.Add("所有奉獻天使人數");
        dtRet.Columns.Add("累計奉獻10萬以上<br>總人數");
        dtRet.Columns.Add("累計奉獻30萬以上<br>總人數");
        dtRet.Columns.Add("累計奉獻50萬以上<br>總人數");
        dtRet.Columns.Add("累計奉獻100萬以上<br>總人數");
        dtRet.Columns.Add("友好教會及機構數量");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i <= row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row)
            {
                dr["序號"] = "列印日期：";
                dr["所有奉獻天使人數"] = DateTime.Now.ToString();
                dr["累計奉獻10萬以上<br>總人數"] = "";
                dr["累計奉獻30萬以上<br>總人數"] = "";
                dr["累計奉獻50萬以上<br>總人數"] = "";
                dr["累計奉獻100萬以上<br>總人數"] = "";
                dr["友好教會及機構數量"] = "";
            }
            /*else if (i == row - 1)
            {
                dr["序號"] = i + 1;
                dr["所有奉獻天使人數"] = "人數：" + dtSrc.Rows[i][0].ToString();
                dr["累計奉獻10萬以上<br>總人數"] = "筆數：" + dtSrc.Rows[i][1].ToString();
                dr["累計奉獻30萬以上<br>總人數"] = "總金額：" + dtSrc.Rows[i][2].ToString() + "元";
                dr["累計奉獻50萬以上<br>總人數"] = "";
                dr["累計奉獻100萬以上<br>總人數"] = "";
                dr["友好教會及機構數量"] = "";
            }*/
            else
            {
                dr["序號"] = i + 1;
                dr["所有奉獻天使人數"] = dtSrc.Rows[i][0].ToString();
                dr["累計奉獻10萬以上<br>總人數"] = dtSrc.Rows[i][1].ToString();
                dr["累計奉獻30萬以上<br>總人數"] = dtSrc.Rows[i][2].ToString();
                dr["累計奉獻50萬以上<br>總人數"] = dtSrc.Rows[i][3].ToString();
                dr["累計奉獻100萬以上<br>總人數"] = dtSrc.Rows[i][4].ToString();
                dr["友好教會及機構數量"] = dtSrc.Rows[i][5].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //DVD贈品索取人報表
    public static DataTable Donate_Other_Report2_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("稱謂");
        dtRet.Columns.Add("郵遞<br>區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("份數");
        dtRet.Columns.Add("資料輸入<br>日期");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][2].ToString() + "筆";
                dr["天使姓名"] = "";
                dr["稱謂"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
                dr["電話"] = "";
                dr["份數"] = "";
                dr["資料輸入<br>日期"] = "";
            }
            else
            {
                dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
                dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
                dr["稱謂"] = dtSrc.Rows[i][2].ToString();
                dr["郵遞<br>區號"] = dtSrc.Rows[i][3].ToString();
                dr["地址"] = dtSrc.Rows[i][4].ToString();
                dr["電話"] = dtSrc.Rows[i][5].ToString();
                dr["份數"] = dtSrc.Rows[i][6].ToString();
                dr["資料輸入<br>日期"] = dtSrc.Rows[i][7].ToString();
            }
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //雷同資料查詢
    public static DataTable Donate_Other_Report3_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("天使姓名");
        dtRet.Columns.Add("性別");
        dtRet.Columns.Add("付款方式");
        dtRet.Columns.Add("狀態");
        dtRet.Columns.Add("捐款時間");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("通訊地址");
        dtRet.Columns.Add("收據地址");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            //最後一列
            /*if (i == row - 1)
            {
                dr["捐款人編號"] = "總筆數：" + dtSrc.Rows[i][2].ToString() + "筆";
                dr["天使姓名"] = "";
                dr["稱謂"] = "";
                dr["郵遞<br>區號"] = "";
                dr["地址"] = "";
                dr["電話"] = "";
                dr["份數"] = "";
                dr["資料輸入<br>日期"] = "";
            }
            else
            {*/
            dr["捐款人編號"] = dtSrc.Rows[i][0].ToString();
            dr["天使姓名"] = dtSrc.Rows[i][1].ToString();
            dr["性別"] = dtSrc.Rows[i][2].ToString();
            dr["付款方式"] = dtSrc.Rows[i][3].ToString();
            dr["狀態"] = dtSrc.Rows[i][4].ToString();
            dr["捐款時間"] = dtSrc.Rows[i][5].ToString();
            dr["電話"] = dtSrc.Rows[i][6].ToString();
            dr["手機"] = dtSrc.Rows[i][7].ToString();
            dr["通訊地址"] = dtSrc.Rows[i][8].ToString();
            dr["收據地址"] = dtSrc.Rows[i][9].ToString();
            //}
            dtRet.Rows.Add(dr);
        };
        return dtRet;
    }
    //---------------------------------------------------------------------------
    //台銀授權失敗回覆檔查詢 20140904新增
    public static DataTable PledgeReturnError_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("序號");
        dtRet.Columns.Add("授權編號");
        dtRet.Columns.Add("授權金額");
        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款人姓名");
        dtRet.Columns.Add("發卡銀行");
        dtRet.Columns.Add("信用卡卡號");
        dtRet.Columns.Add("效期");
        dtRet.Columns.Add("末三碼CVV");
        dtRet.Columns.Add("郵遞區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        dtRet.Columns.Add("授權失敗碼");
        dtRet.Columns.Add("授權失敗原因");
        dtRet.Columns.Add("追蹤日期");
        dtRet.Columns.Add("備註");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["序號"] = dtSrc.Rows[i][0].ToString();
            dr["授權編號"] = dtSrc.Rows[i][1].ToString();
            dr["授權金額"] = dtSrc.Rows[i][2].ToString();
            dr["捐款人編號"] = dtSrc.Rows[i][3].ToString();
            dr["捐款人姓名"] = dtSrc.Rows[i][4].ToString();
            dr["發卡銀行"] = dtSrc.Rows[i][5].ToString();
            dr["信用卡卡號"] = dtSrc.Rows[i][6].ToString();
            dr["效期"] = dtSrc.Rows[i][7].ToString();
            dr["末三碼CVV"] = dtSrc.Rows[i][8].ToString();
            dr["郵遞區號"] = dtSrc.Rows[i][9].ToString();
            dr["地址"] = dtSrc.Rows[i][10].ToString();
            dr["電話"] = dtSrc.Rows[i][11].ToString();
            dr["手機"] = dtSrc.Rows[i][12].ToString();
            dr["授權失敗碼"] = dtSrc.Rows[i][13].ToString();
            dr["授權失敗原因"] = dtSrc.Rows[i][14].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //台銀授權成功回覆檔查詢 20140915新增
    public static DataTable PledgeReturnOK_Print(DataTable dtSrc)
    {
        DataTable dtRet = new DataTable();

        dtRet.Columns.Add("序號");
        dtRet.Columns.Add("授權編號");
        dtRet.Columns.Add("授權金額");
        dtRet.Columns.Add("捐款人編號");
        dtRet.Columns.Add("捐款人姓名");
        dtRet.Columns.Add("發卡銀行");
        dtRet.Columns.Add("信用卡卡號");
        dtRet.Columns.Add("效期");
        dtRet.Columns.Add("郵遞區號");
        dtRet.Columns.Add("地址");
        dtRet.Columns.Add("電話");
        dtRet.Columns.Add("手機");
        int row = dtSrc.Rows.Count;

        for (int i = 0; i < row; i++)
        {
            DataRow dr = dtRet.NewRow();
            dr["序號"] = dtSrc.Rows[i][0].ToString();
            dr["授權編號"] = dtSrc.Rows[i][1].ToString();
            dr["授權金額"] = dtSrc.Rows[i][2].ToString();
            dr["捐款人編號"] = dtSrc.Rows[i][3].ToString();
            dr["捐款人姓名"] = dtSrc.Rows[i][4].ToString();
            dr["發卡銀行"] = dtSrc.Rows[i][5].ToString();
            dr["信用卡卡號"] = dtSrc.Rows[i][6].ToString();
            dr["效期"] = dtSrc.Rows[i][7].ToString();
            dr["郵遞區號"] = dtSrc.Rows[i][8].ToString();
            dr["地址"] = dtSrc.Rows[i][9].ToString();
            dr["電話"] = dtSrc.Rows[i][10].ToString();
            dr["手機"] = dtSrc.Rows[i][11].ToString();
            dtRet.Rows.Add(dr);
        }

        return dtRet;
    }
    //---------------------------------------------------------------------------
    //20140410 新增 by Ian_Kao
    //-------------------------------------------------------------------------------------------------------------
    //紀錄Log檔
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
            if (LogoutTime != "") //如果同一天有 logout, 則另存一筆
            {
                uid = "";
            }
            //記錄登入時間
            if (uid != "") //已有UserID+LogTime 資料列
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
            //記錄動作時間
            //記錄登出時間
            if (uid == "") //Action 沒有相對應的 login 資料
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
            if (uid == "") //logout 沒有相對應的 login 資料,不處理, 隔夜會有問題
            {
                return;
            }
            //記錄登出時間
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
    //20140416 新增 by Ian_Kao
    //-------------------------------------------------------------------------------------------------------------
    //紀錄Log檔 個別紀錄內容
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
    //20140416 新增 by Ian_Kao
    //-------------------------------------------------------------------------------------------------------------
    //DateTime 時間差計算
    public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        int dateDiff = 0;
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2);
        dateDiff = (ts.Days/30); //單純回傳月數
        return dateDiff;
    }
    //-------------------------------------------------------------------------------------------------------------
    //20140521 新增 by Ian_Kao
    //抓取捐款人所在群組
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
