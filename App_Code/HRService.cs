using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// HRService 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
[System.Web.Script.Services.ScriptService]
public class HRService : System.Web.Services.WebService {

    string hrcon = System.Configuration.ConfigurationManager.ConnectionStrings["HRConnection"].ConnectionString;

    public HRService()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    [WebMethod]
    public string GetEMailAddress(string EMPLOYEE_NO)
    {

        string result = "";
        string commandText = @"SELECT E.[EMPLOYEE_CNAME],E.[EMPLOYEE_EMAIL_1] FROM [HRMS_EMPLOYEE] as E,[HRMS_DEPARTMENT] as D
        WHERE E.[DEPARTMENT_ID] = D.[DEPARTMENT_ID] AND E.[EMPLOYEE_NO] = @EMPLOYEE_NO AND ISNULL(E.[EMPLOYEE_EMAIL_1],'') <> '' ";

        using (SqlConnection connection = new SqlConnection(hrcon))
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.Add("@EMPLOYEE_NO", SqlDbType.NVarChar);
            command.Parameters["@EMPLOYEE_NO"].Value = EMPLOYEE_NO;

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    result = dr["EMPLOYEE_CNAME"].ToString() + " <" + dr["EMPLOYEE_EMAIL_1"].ToString() + ">";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return result;

    }

    [WebMethod]
    public string GetEmployee(string EMPLOYEE_NO)
    {

        string query = @"
        SELECT E.[EMPLOYEE_NO],E.[EMPLOYEE_CNAME],E.[EMPLOYEE_EMAIL_1]
            ,E.[EMPLOYEE_OFFICE_TEL_1],E.[EMPLOYEE_CONTACT_TEL_1],D.[DEPARTMENT_CNAME],D.[DEPARTMENT_CODE]
        FROM [HRMS_EMPLOYEE] as E,[HRMS_DEPARTMENT] as D
        WHERE E.[DEPARTMENT_ID] = D.[DEPARTMENT_ID] AND E.[EMPLOYEE_NO] = '" + EMPLOYEE_NO + "' ";

        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(hrcon))
        {
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.Fill(ds);
        }

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        jss.MaxJsonLength = Int32.MaxValue;

        return jss.Serialize(list);

    }

}

