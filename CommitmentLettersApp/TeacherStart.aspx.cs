using DocumentFormat.OpenXml.Spreadsheet;
using DrLogy.DrLogyUtils;
using DrLogyCookies;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommitmentLettersApp
{
    public partial class TeacherStart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CookielessUtils.SetTimestamp(Utils.DateTimeNow());

            string connection = Utils.GetAzureEnvironmentVariable("Connection");
            DrLogy.DrLogyUtils.DbUtils.ConStr = connection;

            int tecid = Utils.DecodeId(Request["Token"].ToString());

            bool sec = ((int)DbUtils.ExecSP("SPAPP_CHECK_TEACHER_PERMISSION", new string[] { "teacherid", "zoneid" }, new object[] { tecid, 145 }) > 0);

            if (!sec)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            DataTable userInfo = DbUtils.GetSPData("SPMISC_GET_LOGIN_USERNAME", new string[] { "UserId" }, new object[] { tecid });

            if (userInfo.Rows.Count ==0)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            UserManager.UserId = tecid;
            UserManager.UserName = (string)userInfo.Rows[0]["username"];
            Response.Redirect("Default.aspx");

        }
    }
}