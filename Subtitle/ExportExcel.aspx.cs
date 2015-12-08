using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExportExcel : System.Web.UI.Page
{

    string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    string timeNow = DateTime.Now.ToString("yyyy-MM-dd");
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            string strType = Request.QueryString["type"].Trim();
            string strDeptID = Request.QueryString["DeptID"].Trim();

            string strYear;
            string strMonth;

            if (String.IsNullOrEmpty(strType)) return;

            switch (strType) {
                case "1":
                    // 查詢模式 依匯入日期
                    string strSDate = Request.QueryString["SDate"].Trim();
                    string strEDate = Request.QueryString["EDate"].Trim();
                    goType1(strSDate, strEDate, strDeptID);
                    break;
                case "2":
                    // 對帳 結帳並匯出 Excel
                    strYear = Request.QueryString["Year"].Trim();
                    strMonth = Request.QueryString["Month"].Trim();
                    string strUserID = Request.QueryString["UserID"].Trim();
                    goType2(strYear, strMonth, strDeptID, strUserID);
                    break;
                case "3":
                    // 查詢模式 依結帳年月
                    strYear = Request.QueryString["Year"].Trim();
                    strMonth = Request.QueryString["Month"].Trim();
                    goType3(strYear, strMonth, strDeptID, "query");
                    break;
                case "4":
                    // 結帳
                    strYear = Request.QueryString["Year"].Trim();
                    strMonth = Request.QueryString["Month"].Trim();
                    goType3(strYear, strMonth, strDeptID, "accounted");
                    break;

            }

        }

    }

    private void goType3(string Year, string Month, string DeptID, string Mode)
    {

        Dictionary<string, object> dict = new Dictionary<string, object>();
        string query1 = @"SELECT c.[EditorID] as [字幕人員代號],c.[EditorName] as [姓名] ,SUM([SubtitleCostPerEpisode]) as [個人小計]
                    FROM [_ST03P0] as a
                    JOIN [_ST01P0] as c ON a.[EditorID] = c.[EditorID]
                    JOIN [_ST01M1] as d ON a.[EditorID] = d.[EditorID]
                    where [BillDate] = @YearMonth  ";

        dict.Add("YearMonth", Convert.ToInt32(Year) * 12 + Convert.ToInt32(Month));
        if (DeptID != "000")
        {
            query1 += " and [EditorDept] = @EditorDept ";
            dict.Add("EditorDept", DeptID);
        }
        query1 += " group by c.[EditorID],c.[EditorName] order by c.[EditorID] desc  ";
        DataTable dt1 = NpoDB.GetDataTableS(query1, dict);

        string query2 = @"SELECT a.[EditorID] as [字幕人員代號],a.[ProgramID] as [節目代號],b.[ProgramAbbrev] as [節目名稱]
                    ,[Episode] as [集數],[SubtitleCostPerEpisode] as [字幕費用],CONVERT(char(10),[ImportDate],120) as [字幕匯入日]
                    ,case when [SubtitleCostPerEpisode] = ISNULL(c.[Amount],0) then 'V' else '' end as [標準成本]
                    FROM [_ST03P0] as a 
                    JOIN [_TM01P0] as b ON a.[ProgramID] = b.[ProgramID]
                    JOIN [_ST01M1] as d ON a.[EditorID] = d.[EditorID] 
                    LEFT JOIN [_ST02P0] as c ON  [Enable] = 1
                    and a.[Classification] = c.[Classification] and a.[ProgramLength] = c.[Length]
                    where [BillDate] = @YearMonth  ";

        if (DeptID != "000")
        {
            query2 += " and [EditorDept] = @EditorDept ";
        }
        query2 += " order by a.[EditorID] desc,a.[ProgramID],[Episode]  ";
        DataTable dt2 = NpoDB.GetDataTableS(query2, dict);

        ExcelOutput(dt1, dt2, timeNow + "_字幕費用查詢報表", Year, Month, Mode);
    }

    private void goType2(string strYear, string strMonth, string DeptID,string UserID)
    {

        Dictionary<string, object> dict = new Dictionary<string, object>();
        string query1 = @"SELECT c.[EditorID] as [字幕人員代號],c.[EditorName] as [姓名] ,SUM([SubtitleCostPerEpisode]) as [個人小計]
                    FROM [_ST03P0] as a
                    JOIN [_ST01P0] as c ON a.[EditorID] = c.[EditorID]
                    JOIN [_ST01M1] as d ON a.[EditorID] = d.[EditorID]
                    where [BillDate] = -1 and YEAR([ImportDate]) = @intYear and  MONTH([ImportDate]) = @intMonth ";

        dict.Add("intYear", Convert.ToInt32(strYear));
        dict.Add("intMonth", Convert.ToInt32(strMonth));
        if (DeptID != "000")
        {
            query1 += " and [EditorDept] = @EditorDept ";
            dict.Add("EditorDept", DeptID);
        }
        query1 += " group by c.[EditorID],c.[EditorName] order by c.[EditorID] desc  ";
        DataTable dt1 = NpoDB.GetDataTableS(query1, dict);

        string query2 = @"SELECT a.[EditorID] as [字幕人員代號],a.[ProgramID] as [節目代號],b.[ProgramAbbrev] as [節目名稱]
                    ,[Episode] as [集數],[SubtitleCostPerEpisode] as [字幕費用],CONVERT(char(10),[ImportDate],120) as [字幕匯入日]
                    ,case when [SubtitleCostPerEpisode] = ISNULL(c.[Amount],0) then 'V' else '' end as [標準成本]
                    FROM [_ST03P0] as a 
                    JOIN [_TM01P0] as b ON a.[ProgramID] = b.[ProgramID]
                    JOIN [_ST01M1] as d ON a.[EditorID] = d.[EditorID] 
                    LEFT JOIN [_ST02P0] as c ON  [Enable] = 1
                    and a.[Classification] = c.[Classification] and a.[ProgramLength] = c.[Length]
                    where [BillDate] = -1 and YEAR([ImportDate]) = @intYear and  MONTH([ImportDate]) = @intMonth ";

        if (DeptID != "000")
        {
            query2 += " and [EditorDept] = @EditorDept ";
        }
        query2 += " order by a.[EditorID] desc,a.[ProgramID],[Episode]  ";
        DataTable dt2 = NpoDB.GetDataTableS(query2, dict);

        //update
        var result = 0;

        string commandText = @"
            UPDATE [_ST03P0]
            SET [BillDate]=@BillDate,[ModifiedUser]=@ModifiedUser,[ModifiedDatetime]=GETDATE()
            WHERE [BillDate] = -1 and YEAR([ImportDate]) = @intYear and  MONTH([ImportDate]) = @intMonth
       ";

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.Add("@intYear", SqlDbType.Int).Value = Convert.ToInt32(strYear);
            command.Parameters.Add("@intMonth", SqlDbType.Int).Value = Convert.ToInt32(strMonth);
            command.Parameters.Add("@ModifiedUser", SqlDbType.NVarChar).Value = UserID;
            command.Parameters.Add("@BillDate", SqlDbType.Int).Value = Convert.ToInt32(strYear) * 12 + Convert.ToInt32(strMonth);

            try
            {
                connection.Open();
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        if (result > 0)
        {
            ExcelOutput(dt1, dt2, timeNow.Substring(0, 7) + "_字幕費用結帳報表", strYear, strMonth, "account");
        }

    }

    // 查詢模式 依匯入日期
    private void goType1(string SDate, string EDate, string DeptID)
    {

        Dictionary<string, object> dict = new Dictionary<string, object>();
        string query1 = @"SELECT c.[EditorID] as [字幕人員代號],c.[EditorName] as [姓名] ,SUM([SubtitleCostPerEpisode]) as [個人小計]
                    FROM [_ST03P0] as a
                    JOIN [_ST01P0] as c ON a.[EditorID] = c.[EditorID]
                    JOIN [_ST01M1] as d ON a.[EditorID] = d.[EditorID]
                    where cast([ImportDate] as date) between cast(@SDate as date) and cast(@EDate as date)  ";

        dict.Add("SDate", SDate);
        dict.Add("EDate", EDate);
        if (DeptID != "000")
        {
            query1 += " and [EditorDept] = @EditorDept ";
            dict.Add("EditorDept", DeptID);
        }
        query1 += " group by c.[EditorID],c.[EditorName] order by c.[EditorID] desc  ";
        DataTable dt1 = NpoDB.GetDataTableS(query1, dict);

        string query2 = @"SELECT a.[EditorID] as [字幕人員代號],a.[ProgramID] as [節目代號],b.[ProgramAbbrev] as [節目名稱]
                    ,[Episode] as [集數],[SubtitleCostPerEpisode] as [字幕費用],CONVERT(char(10),[ImportDate],120) as [字幕匯入日]
                    ,case when [SubtitleCostPerEpisode] = ISNULL(c.[Amount],0) then 'V' else '' end as [標準成本]
                    FROM [_ST03P0] as a 
                    JOIN [_TM01P0] as b ON a.[ProgramID] = b.[ProgramID]
                    JOIN [_ST01M1] as d ON a.[EditorID] = d.[EditorID] 
                    LEFT JOIN [_ST02P0] as c ON  [Enable] = 1
                    and a.[Classification] = c.[Classification] and a.[ProgramLength] = c.[Length]
                    where cast([ImportDate] as date) between cast(@SDate as date) and cast(@EDate as date)  ";

        if (DeptID != "000")
        {
            query2 += " and [EditorDept] = @EditorDept ";
        }
        query2 += " order by a.[EditorID] desc,a.[ProgramID],[Episode]  ";
        DataTable dt2 = NpoDB.GetDataTableS(query2, dict);

        ExcelOutput(dt1, dt2, timeNow + "_字幕費用查詢報表", "", "", "query");

    }

    private bool ExcelOutput(DataTable dt1, DataTable dt2, string strFileName, string intYear, string intMonth, string mode)
    {

        FileStream fs = null;
        MemoryStream ms = null;
        byte[] data = null;

        try
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("Sheet1");
            //workbook.CreateSheet("Sheet2");
            //workbook.CreateSheet("Sheet3");

            //數值儲存格
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
            style.DataFormat = format.GetFormat("0");

            //抬頭
            string title = "";
            if (mode != "query") title = intYear + " 年 " + intMonth + " 月";
            int rowIndex = 0;
            HSSFRow titleRow = (HSSFRow)sheet.CreateRow(rowIndex);
            HSSFCell hSSFCell = (HSSFCell)titleRow.CreateCell(0);
            if (mode == "query")
                hSSFCell.SetCellValue("字幕費用查詢報表");
            else
                hSSFCell.SetCellValue(title + " 字幕費用明細表");

            //列印日期
            rowIndex++;
            titleRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)titleRow.CreateCell(4);
            //hSSFCell.CellStyle = style;
            hSSFCell.SetCellValue("列印日期：");
            hSSFCell = (HSSFCell)titleRow.CreateCell(5);
            //hSSFCell.CellStyle = style;
            hSSFCell.SetCellValue(timeNow);

            foreach (DataRow dr in dt1.Rows)
            {

                rowIndex++;
                int cowIndex = 0;
                HSSFRow headerRow = (HSSFRow)sheet.CreateRow(rowIndex);
                hSSFCell = (HSSFCell)headerRow.CreateCell(cowIndex);
                hSSFCell.CellStyle = style;
                hSSFCell.SetCellValue("字幕人員代號：");

                cowIndex++;
                hSSFCell = (HSSFCell)headerRow.CreateCell(cowIndex);
                hSSFCell.CellStyle = style;
                hSSFCell.SetCellValue(dr["字幕人員代號"].ToString());

                cowIndex += 2;
                hSSFCell = (HSSFCell)headerRow.CreateCell(cowIndex);
                hSSFCell.CellStyle = style;
                hSSFCell.SetCellValue("姓名：");

                cowIndex++;
                hSSFCell = (HSSFCell)headerRow.CreateCell(cowIndex);
                hSSFCell.CellStyle = style;
                hSSFCell.SetCellValue(dr["姓名"].ToString());

                DataRow[] dr2Arrry = dt2.Select("[字幕人員代號] = '" + dr["字幕人員代號"].ToString() + "'");

                rowIndex++;
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                foreach (DataColumn column in dt2.Columns)
                {
                    if (column.Ordinal != 0)
                    {
                        hSSFCell = (HSSFCell)dataRow.CreateCell(column.Ordinal - 1);
                        hSSFCell.CellStyle = style;
                        hSSFCell.SetCellValue(column.ColumnName);
                    }
                }

                foreach (DataRow row in dr2Arrry)
                {
                    rowIndex++;
                    dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in dt2.Columns)
                    {
                        if (column.Ordinal != 0)
                        {
                            hSSFCell = (HSSFCell)dataRow.CreateCell(column.Ordinal - 1);
                            if (column.Ordinal == 3 || column.Ordinal == 4)
                            {
                                hSSFCell.SetCellValue(Convert.ToDouble(row[column]));
                                hSSFCell.CellStyle = style;
                            }
                            else
                            {
                                hSSFCell.SetCellValue(row[column].ToString());
                            }
                        }
                    }

                }

                rowIndex++;
                dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                hSSFCell = (HSSFCell)dataRow.CreateCell(1);
                if (mode == "query")
                    hSSFCell.SetCellValue("個人小計：" + dr["個人小計"].ToString());
                else
                    hSSFCell.SetCellValue(title + " 費用個人小計：" + dr["個人小計"].ToString());
                rowIndex++;
                dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
            }

            rowIndex++;
            HSSFRow totalRow = (HSSFRow)sheet.CreateRow(rowIndex);
            hSSFCell = (HSSFCell)totalRow.CreateCell(1);
            if (mode == "query")
                hSSFCell.SetCellValue("字幕費總計：" + dt1.Compute("sum([個人小計])", null).ToString());
            else
                hSSFCell.SetCellValue(title + " 字幕費總計：" + dt1.Compute("sum([個人小計])", null).ToString());

            sheet.SetColumnWidth(0, 15 * 300);
            sheet.SetColumnWidth(1, 37 * 300);
            sheet.SetColumnWidth(2, 10 * 300);
            sheet.SetColumnWidth(3, 15 * 300);
            sheet.SetColumnWidth(4, 20 * 300);
            sheet.SetColumnWidth(5, 12 * 300);

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            titleRow = null;
            workbook = null;

            string filename = strFileName + ".xls";
            string serverPathfilename = Server.MapPath("~/downloadtemp/" + filename);

            fs = new FileStream(serverPathfilename, FileMode.Create, FileAccess.Write);
            data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            dt1.Dispose();
            dt2.Dispose();

            var response = Context.Response;

            response.Clear();
            response.BufferOutput = true;
            response.ContentType = "application/ms-excel";
            response.AppendHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(filename));
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
