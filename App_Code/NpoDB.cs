using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Collections;

/// <summary>
/// CmsADODB 的摘要描述
/// </summary>
public class NpoDB : IDisposable
{

    public static log4net.ILog logger = log4net.LogManager.GetLogger("RollingLogFileAppender");

    public string ConnectionString = "";
    public SqlConnection Conn;
    public SqlCommand Cmd;
    public SqlTransaction Tran;
    bool disposed = false;
    //---------------------------------------------------------------------------
    public NpoDB()
    {
        ConnectionString = GetConnectionString();
        Conn = new SqlConnection(ConnectionString);
        Cmd = new SqlCommand();
        Cmd.Connection = Conn;
    }
    //---------------------------------------------------------------------------
    //開啟交易
    public void BeginTrans()
    {
        Conn.Open();
        Tran = Conn.BeginTransaction();
    }
    //---------------------------------------------------------------------------
    //結束交易
    public void CommitTrans()
    {
        Tran.Commit();
    }
    //---------------------------------------------------------------------------
    //還原交易
    public void RollbackTrans()
    {
        Tran.Rollback();
    }
    //---------------------------------------------------------------------------
    ///<summary >
    ///取得查詢資料(回傳DataTable)
    ///</summary>
    public DataTable GetDataTable(string strSql)
    {
        Cmd.CommandText = strSql;
        Cmd.Transaction = Tran;

        SqlDataAdapter adp = new SqlDataAdapter(Cmd);
        DataTable dt = new DataTable();
        adp.Fill(dt);
        return dt;
    }
    //---------------------------------------------------------------------------
    ///<summary >
    ///取得查詢資料(回傳DataTable)
    ///</summary>
    public DataTable GetDataTable(string strSql, Dictionary<string, object> ParametersDict)
    {
        Cmd.CommandText = strSql;
        Cmd.Transaction = Tran;
        Cmd.Parameters.Clear();

        foreach (KeyValuePair<string, object> pair in ParametersDict)
        {
            if (pair.Value == null)
            {
                Cmd.Parameters.AddWithValue(pair.Key, DBNull.Value);
            }
            else
            {
                Cmd.Parameters.AddWithValue(pair.Key, pair.Value);
            }
        }

        SqlDataAdapter adp = new SqlDataAdapter(Cmd);
        DataTable dt = new DataTable();
        adp.Fill(dt);
        return dt;
    }
    //---------------------------------------------------------------------------
    ///<summary >
    ///取得查詢資料(回傳單一字串)
    ///</summary>
    public string QueryGetScalar(string strSql)
    {
        DataTable dt = GetDataTable(strSql);
        if (dt.Rows.Count > 0)
            return dt.Rows[0][0].ToString();
        else
            return "";
    }
    //---------------------------------------------------------------------------
    ///<summary >
    ///取得查詢資料(回傳單一字串)
    ///</summary>
    public string QueryGetScalar(string strSql, Dictionary<string, object> ParametersDict)
    {
        DataTable dt = GetDataTable(strSql, ParametersDict);
        if (dt.Rows.Count > 0)
            return dt.Rows[0][0].ToString();
        else
            return "";
    }
    //---------------------------------------------------------------------------
    ///<summary >
    ///取得查詢資料(回傳DataTable)
    ///</summary>
    public static DataTable QueryGetTable(string strSql, string ConnectionStringName = "")
    {
        using (SqlConnection conn = GetSqlConnection(ConnectionStringName))
        {
            SqlDataAdapter adp = new SqlDataAdapter(strSql, conn);
            adp.SelectCommand.CommandTimeout = 999;
            DataTable dt = new DataTable();
            adp.Fill(dt);

            return dt;
        }
    }
    //---------------------------------------------------------------------------
    ///<summary >
    ///執行 SQL, 回傳 effect 數
    ///</summary>
    public int ExecuteSQL(string strSql, Dictionary<string, object> ParametersDict)
    {
        Cmd.CommandText = strSql;
        Cmd.Transaction = Tran;
        Cmd.Parameters.Clear();

        int effect = 0;

        foreach (KeyValuePair<string, object> pair in ParametersDict)
        {
            if (pair.Value == null)
            {
                Cmd.Parameters.AddWithValue(pair.Key, DBNull.Value);
            }
            else
            {
                Cmd.Parameters.AddWithValue(pair.Key, pair.Value);
            }
        }
        try
        {
            effect = Cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            throw ex;
        }
        return effect;
    }
    //---------------------------------------------------------------------------
    /// <summary>
    /// 取得完整的SQL語法，將參數Replace為值
    /// </summary>    
    /// <param name="strSql">SQL 語法未帶值</param>
    /// <param name="ParametersDict">Key Value 集合</param>
    /// <returns>SQL語法帶值</returns>
    public string GetRealSql(string strSql, Dictionary<string, object> ParametersDict)
    {
        string returnSQL = strSql;
        foreach (KeyValuePair<string, object> pair in ParametersDict)
        {
            returnSQL = returnSQL.Replace("@" + pair.Key, "\'" + pair.Value.ToString().Replace("\'", "\'\'") + "\'");
        }
        return returnSQL;
    }
    //---------------------------------------------------------------------------
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (Conn != null)
                Conn.Close();
            if (Cmd != null)
                Cmd.Dispose();
            if (Tran != null)
                Tran.Dispose();
            if (Conn != null)
                Conn.Dispose();
            disposed = true;
        }
    }
    //---------------------------------------------------------------------------
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    //---------------------------------------------------------------------------
    public static Page page { get { return HttpContext.Current.CurrentHandler as Page; } }
    ///<summary >
    ///取得查詢資料(static 版)
    ///</summary>
    public static DataTable GetDataTableS(string strSql, Dictionary<string, object> ParametersDict, string ConnectionStringName = "")
    {
        using (SqlConnection conn = GetSqlConnection(ConnectionStringName))
        {
            SqlCommand cmd = new SqlCommand(strSql, conn);
            cmd.CommandTimeout = 999;
            if (ParametersDict != null)
            {
                foreach (KeyValuePair<string, object> pair in ParametersDict)
                {
                    if (pair.Value == null)
                    {
                        cmd.Parameters.AddWithValue(pair.Key, DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                    }
                }
            }
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            return dt;
        }
    }
    //---------------------------------------------------------------------------
    ///<summary >
    ///執行SQL
    ///</summary>
    public static int ExecuteSQLS(string strSql, Dictionary<string, object> ParametersDict)
    {
        using (SqlConnection conn = GetSqlConnection())
        {
            SqlCommand cmd = new SqlCommand(strSql, conn);
            if (ParametersDict != null)
            {
                foreach (KeyValuePair<string, object> pair in ParametersDict)
                {
                    if (pair.Value == DBNull.Value || pair.Value == null || pair.Value.ToString().ToUpper() == "NULL")
                    {
                        cmd.Parameters.AddWithValue(pair.Key, DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                }
            }

            int effect = 0;
            try
            {
                conn.Open();
                effect = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                logger.Error(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return effect;
        }
    }
    //---------------------------------------------------------------------------
    public static string GetConnectionString(string ConnectionStringName = "")
    {
        if (String.IsNullOrEmpty(ConnectionStringName))
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
        else
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
        }
    }
    //---------------------------------------------------------------------------
    ///<summary >
    ///取得Connection物件
    ///</summary>
    public static SqlConnection GetSqlConnection(string ConnectionStringName = "")
    {
        string ConnectionString = GetConnectionString(ConnectionStringName);
        SqlConnection conn = new SqlConnection(ConnectionString);
        return conn;
    }
    //---------------------------------------------------------------------------

    ///<summary >
    ///執行 SQL 指令, 回傳單一字串(可能傳回最新的 RowID)
    ///</summary>
    public static string GetScalarS(string strSql, Dictionary<string, object> ParametersDict, string ConnectionStringName = "")
    {
        string returnStr = "";
        try
        {
            DataTable dt = GetDataTableS(strSql, ParametersDict, ConnectionStringName);
            returnStr = dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
        }
        catch (Exception ex)
        {
            returnStr = "";
            logger.Error(ex.Message);
            throw ex;
        }

        return returnStr;
    }
    //---------------------------------------------------------------------------

    public static string[] GetEMailAddressList(string MailType)
    {
        string query = @"SELECT distinct [EMailName],[EMailAddress] FROM [EMailAddressList] " +
                        " WHERE [Enable] = 1 AND [EMailType] = '" + MailType + "' ";
        ArrayList array = new ArrayList();

        DataTable dt = NpoDB.QueryGetTable(query);

        foreach (DataRow dr in dt.Rows)
        {
            //if (String.IsNullOrEmpty(dr["EMailName"].ToString()))
            //{
                array.Add(dr["EMailAddress"].ToString());
            //}
            //else
            //{
            //    array.Add(dr["EMailName"].ToString() + " <" + dr["EMailAddress"].ToString() + ">");
            //}
        }

        return (string[])array.ToArray(typeof(string));

    }

    ///<summary >
    ///執行 SQL 指令, 回傳同工姓名
    ///</summary>
    public static string GetHRUser(string Employee_no)
    {
        if (String.IsNullOrEmpty(Employee_no)) return "";
        string strEmployee_cname = "";
        string sql = "SELECT [EMPLOYEE_CNAME] FROM [HRMS_EMPLOYEE] where [EMPLOYEE_NO] = @EMPLOYEE_NO ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("EMPLOYEE_NO", Employee_no);
        strEmployee_cname = NpoDB.GetScalarS(sql, dict, "HRConnection");
        return strEmployee_cname;
    }

}

