using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Subtitle_SubtitleStatistics : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //權控處理
            Session["ProgID"] = Util.GetQueryString("ProgID");
            AuthrityControl();

            UserID.Value = SessionInfo.UserID;
        }
    }

    //---------------------------------------------------------------------------
    public void AuthrityControl()
    {
        if (Authrity.PageRight("_Focus") == false)
        {
            return;
        }
        /* 備用
        Authrity.CheckButtonRight("_Update", btnEdit);
        Authrity.CheckButtonRight("_Delete", btnDel);
        Authrity.CheckButtonRight("_Update", btnGroupEdit);
        //MemberMenu.Visible = Authrity.RightCheck("_Update");
        Authrity.CheckButtonRight("_AddNew", btnAddDonateData);
         * */
    }

}
