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

public partial class Subtitle_Download : System.Web.UI.Page
{

    string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            string ProgramID = Request.QueryString["ProgramID"].Trim();
            string Episode = Request.QueryString["Episode"].Trim();
            string SubtitleType = Request.QueryString["SubtitleType"];
            string EpisodeN4F = String.Format("{0:0000}", Convert.ToInt32(Episode));

            string query = "SELECT " + (SubtitleType.ToUpper() == "SRT" ? "[SubtitleSRT] as [Subtitle] " : "[Subtitle] ");
            query += " ,b.Classification ,(SELECT TOP 1 [HouseNo] FROM [srtmcs_cue] ";
            query += " WHERE [PresentationTitle] like '" + ProgramID + "%' and EpisodeNumber = " + Episode + ") as [HouseNo] ";
            query += " FROM [_ST03P1] as a LEFT JOIN [_ST03P0] as b ";
            query += " on a.[ProgramID] = b.[ProgramID] and a.[Episode] = b.[Episode] ";
            query += " WHERE a.[ProgramID] = '" + ProgramID + "' AND a.[Episode] = " + Episode + " ;";
            DataSet ds = queryDataTable(query);

            //List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            string result = "";
            string Classification = "";
            string HouseNo = "";

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                //Dictionary<string, object> dict = new Dictionary<string, object>();

                //foreach (DataColumn col in ds.Tables[0].Columns)
                //{
                    //result = System.Text.Encoding.Unicode.GetBytes(row[col].ToString());
                    result = row["Subtitle"].ToString();
                    Classification = row["Classification"].ToString();
                    HouseNo = row["HouseNo"].ToString();
                //}
            }

            string filename = "";
            Encoding encoding = Encoding.Unicode;

            if (SubtitleType.ToUpper() == "SRT")
            {
                encoding = Encoding.UTF8;
                if (HouseNo != "")
                    filename = HouseNo + ".srt";
                else
                    filename = ProgramID + EpisodeN4F + (Classification == "01" ? "_TC" : "") + ".srt";
            }
            else if (SubtitleType.ToUpper() == "TC")
            {
                filename = ProgramID + "_" + EpisodeN4F + "HDN.TC";
            }
            else
            {
                filename = ProgramID + EpisodeN4F + (Classification == "01" ? "_TC" : "") + ".txt";
            }
            string serverPathfilename = Server.MapPath("~/downloadtemp/" + filename);

            StreamWriter sw = new StreamWriter(serverPathfilename, false, encoding);
            if (null != sw)
            {
                sw.WriteLine((SubtitleType.ToUpper() == "SRT" ? ((result.IndexOf("\r\n") == -1) ? result : result.Replace("\r\n", "\n")) : ((result.IndexOf("\r\n") == -1) ? result.Replace("\n", "\r\n") : result)));
                sw.Close();
            }

            var response = Context.Response;

            response.Clear();
            response.BufferOutput = true;
            response.ContentType = "text/plain;charset=utf-8";
            response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            response.WriteFile(serverPathfilename);
            response.Flush();
            response.End();
            
        }

    }


    /// <summary>
    /// 依據SQL語句，回傳DataTable物件
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    private DataSet queryDataTable(string sql)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(constr))
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(ds);
        }

        //return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        return ds;

    }

}
