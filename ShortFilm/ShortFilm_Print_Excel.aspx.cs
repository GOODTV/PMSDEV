using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using NPOI.HSSF.UserModel;


public partial class ShortFilm_ShortFilm_Print_Excel : BasePage
{

    string hrcon = System.Configuration.ConfigurationManager.ConnectionStrings["HRConnection"].ConnectionString;
    List<ControlData> list = new List<ControlData>();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["Uid"] == null || Session["Uid"].ToString() == "") return;

            string strSql = @"select CONVERT(VarChar,RequestDate,111) as RequestDate,ProgramID +' '+ [dbo].[getProgramName](ProgramID) as ProgramName, 
	                            Episode ,
                                RIGHT('0' + CAST(Length / 3600 AS VARCHAR),2) + ':' +RIGHT('0' + CAST((Length / 60) % 60 AS VARCHAR),2)  + ':' +RIGHT('0' + CAST(Length % 60 AS VARCHAR),2)+ ':00' as TimePeriod ,
                                Case when Channel='1' then '一台' when Channel='2' then '二台' when Channel='3' then '一二台' end as Channel,ISNULL(HDC,'') as HDC,
	                            CONVERT(VarChar,OnAirBeginDate,111)+' - '+CONVERT(VarChar,OnAirEndDate,111) as OnAirDate , OnAirRemark ,
	                            CFDescription ,Frequency,
	                            OnAirTimeSlot ,Case when ISNULL(ForWeb,'')='Y' then 'Y' else 'N' end as ForWeb,
	                            CFID ,CFTitle ,b.[Description]
	                            From [dbo].[_CF02P0] as a left join [_CF01P0] as b on SUBSTRING(a.CFID,1,3) = b.Category
                            where CFID='" + Session["Uid"].ToString() + "'";
            Print_Excel(strSql);
        }

    }

    //---------------------------------------------------------------------------
    private void Print_Excel(string strSql)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count != 1)
        {
            return;
        }

        GetTable(dt, Session["Uid"] + "_重_短片托播單");
    }

    //---------------------------------------------------------------------------
    private bool GetTable(DataTable dt, string fileName)
    {

        int count = dt.Rows.Count;
        DataRow dr = dt.Rows[0];

        //人事資料
        string query = @"
                            SELECT E.[EMPLOYEE_NO],E.[EMPLOYEE_CNAME],E.[EMPLOYEE_EMAIL_1]
                                ,E.[EMPLOYEE_OFFICE_TEL_1],E.[EMPLOYEE_CONTACT_TEL_1],D.[DEPARTMENT_CNAME],D.[DEPARTMENT_CODE]
                            FROM [HRMS_EMPLOYEE] as E,[HRMS_DEPARTMENT] as D
                            WHERE E.[DEPARTMENT_ID] = D.[DEPARTMENT_ID] AND E.[EMPLOYEE_NO] = '" + Session["RequesterID"] + "' ";
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(hrcon))
        {
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.Fill(ds, "HR");
        }
        DataRow HRrow = ds.Tables["HR"].Rows[0];

        FileStream fs = null;
        MemoryStream ms = null;
        byte[] data = null;

        try
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("Sheet0");

            HSSFCellStyle cs = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.FontName = "新細明體";
            font.FontHeightInPoints = 14;
            cs.SetFont(font);

            //標頭
            int rowIndex = 0;
            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            HSSFCell hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("節目預告 / 短片申請單(重)");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("填單日期：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["RequestDate"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("部門名稱：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(HRrow["DEPARTMENT_CNAME"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("申請同工：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(HRrow["EMPLOYEE_NO"].ToString().Trim() + " " + HRrow["EMPLOYEE_CNAME"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("申請同工分機-手機：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(HRrow["EMPLOYEE_OFFICE_TEL_1"].ToString().Substring(14) + " - " +
                HRrow["EMPLOYEE_CONTACT_TEL_1"].ToString().Substring(0, 4) + HRrow["EMPLOYEE_CONTACT_TEL_1"].ToString().Substring(5, 3) +
                HRrow["EMPLOYEE_CONTACT_TEL_1"].ToString().Substring(9, 3));
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("申請節目代號名稱：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["ProgramName"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("集數：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["Episode"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("分集名稱及最近播映日期：");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("短片類別：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["Description"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("申請短片片數：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue("1");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("短片長度：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["TimePeriod"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("播出頻道：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["Channel"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("HDC：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["HDC"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("排播起迄日：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["OnAirDate"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("排播說明：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["OnAirRemark"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("短片內容摘要說明：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["CFDescription"].ToString().Trim());
            hSSFCell.CellStyle = cs;


            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("排播頻率(每日排播次數)：");
            hSSFCell.CellStyle = cs;

            string Frequency = dr["Frequency"].ToString().Trim();

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue((Frequency == "1" ? "v" : "x") + "A級 " +
                (Frequency == "2" ? "v" : "x") + "B級 " + (Frequency == "3" ? "v" : "x") + "C級");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("排播時段：");
            hSSFCell.CellStyle = cs;

            string OnAirTimeSlot = dr["OnAirTimeSlot"].ToString().Trim();

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue((OnAirTimeSlot.IndexOf("0") > -1 ? "v" : "x") + "信徒 " +
                (OnAirTimeSlot.IndexOf("1") > -1 ? "v" : "x") + "預工 " + (OnAirTimeSlot.IndexOf("2") > -1 ? "v" : "x") + "家庭 " +
                (OnAirTimeSlot.IndexOf("3") > -1 ? "v" : "x") + "佈道 " + (OnAirTimeSlot.IndexOf("4") > -1 ? "v" : "x") + "深夜");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("短片須提供官網：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["ForWeb"].ToString().Trim() + " (如申請多支，請於後填註須提供的短片編號)");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("短片編號：");
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue("短片名稱：");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue(dr["CFID"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            hSSFCell = (HSSFCell)dataRow.CreateCell(1);
            hSSFCell.SetCellValue(dr["CFTitle"].ToString().Trim());
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("託播人簽核：");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("製作人簽核：");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("編審簽核：");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("經理簽核：");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("影音資料組：一台                         二台                         組長：");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("說明：");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("1、排播頻率說明:A集表示每日播出8~10次、B級表示6~8次、C級表示2~4次。");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("2、排播時段說明:深夜時段表示 24:00~09:00。");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("3、各欄位務請詳實填列，如需修改，請直接於本單上改訂，並請簽字確認修改人員。");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("4、以鉛筆修訂或無人於修改處簽名者，一律依電腦列印的資訊排播。");
            hSSFCell.CellStyle = cs;

            rowIndex++;
            dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)dataRow.CreateCell(0);
            hSSFCell.SetCellValue("5、本單各簽核人員均需簽註。");
            hSSFCell.CellStyle = cs;

            sheet.SetColumnWidth(0, 30 * 270);
            sheet.SetColumnWidth(1, 30 * 270);

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            workbook = null;

            string serverPathfilename = Server.MapPath("~/downloadtemp/" + fileName + ".xls");

            fs = new FileStream(serverPathfilename, FileMode.Create, FileAccess.Write);
            data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            dt.Dispose();

            var response = Context.Response;

            response.Clear();
            response.BufferOutput = true;
            response.ContentType = "application/ms-excel";
            response.AppendHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName + ".xls"));
            response.WriteFile(serverPathfilename);
            response.Flush();
            response.End();

        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Excel Output Error:" + ex.Message);

            return false;
        }
        finally
        {
            fs = null;
            data = null;
            ms = null;
        }

        return true;
    }

}
