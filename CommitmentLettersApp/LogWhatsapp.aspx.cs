using DocumentFormat.OpenXml.Spreadsheet;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommitmentLettersApp
{
    public partial class LogWhatsapp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["stid"] != null)
            {
                int userId = UserManager.UserId;

                int studentId = Convert.ToInt32(Request["stid"]);
                DbUtils.LogActivity(26 /*whatsapp*/, 102 /*Commitments*/, userId , 0, studentId);
                //DbUtils.LogActivity(Constants.ActTypePrint, Constants.ActObjNewApp, teacherId, 0, studentId, 0, Request["page"]);
            }
        }
    }
}