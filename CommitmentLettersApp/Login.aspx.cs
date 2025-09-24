using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommitmentLettersApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (UserManager.UserId > 0)
                    Response.Redirect("default.aspx");

                chkRememberMe.Checked = true; //default value

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                this.Title += " " + version;

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string connection = Utils.GetAzureEnvironmentVariable("Connection");
            loginerror.Value = "";
            DrLogy.DrLogyUtils.DbUtils.ConStr = connection;
            UserManager.Logout();
            int actType = 40; // login
            int actUserId = 0;
            string actComments = "";

            DataTable dt =  DbUtils.GetSPData ("SPAPP_LOGIN_TEACHER", new string[] { "USERNAME", "ZEHUT" ,"PASSWORD" , "CDATE" }, new object[] { inputUsername.Value, inputUsername.Value, inputPassword.Value , Utils.DateTimeNow() });

            int rc = 0;

            if (dt.Rows.Count > 0)
            {
                actUserId =  rc = (int)dt.Rows[0]["tec_id"];
                
            }
            else
            {
                loginerror.Value = "שגיאה בהתחברות";
                actType = 42; //invalid user name or pass
                actComments = inputUsername.Value;
            }

            string username = "";
            if (rc != 0)
            { 
                username = (string)dt.Rows[0]["tec_username"];
                bool sec = ((int)DbUtils.ExecSP("SPAPP_CHECK_TEACHER_PERMISSION", new string[] { "teacherid", "zoneid" }, new object[] { rc, 145 }) > 0);

                if (!sec)
                {
                    loginerror.Value = "למשתמש אין הרשאות גישה לאפליקציה";
                    rc = 0;
                    actType = 44;// no permissons
                }
            }

            if (rc > 0 && (int)dt.Rows[0]["tec_status"] != 1)
            {
                loginerror.Value = "המשתמש חסום, נא לפנות לרכז";
                actType = 43;
                rc = 0;
            }


            if (rc != 0)
            {
                UserManager.UserId = rc;
                UserManager.UserName = username;
                if (chkRememberMe.Checked)
                {
                    UserManager.SetUserCookie();

                }

                Response.Redirect("Default.aspx");
            }
            else
                //log error
                DbUtils.LogActivity(actType, 102 /*Commitments*/, rc, actUserId , 0, 0, actComments);

        }
    }
}