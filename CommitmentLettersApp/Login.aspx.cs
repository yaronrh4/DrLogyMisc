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
            string connection = Utils.GetAppSetting("Connection", "");
            loginerror.Value = "";
            DrLogy.DrLogyUtils.DbUtils.ConStr = connection;
            UserManager.Logout();

            DataTable dt =  DbUtils.GetSPData ("SPMISC_LOGIN", new string[] { "UserNameOrZehut", "Pass" }, new object[] { inputUsername.Value, inputPassword.Value });
            int rc = (int)dt.Rows[0]["user_id"];
            string username = "";
            if (rc == 0)
            {
                loginerror.Value = "שגיאה בהתחברות";
            }
            else
            {
                username = (string)dt.Rows[0]["user_name"];
                bool sec = ((int)DbUtils.ExecSP("SPAPP_CHECK_TEACHER_PERMISSION", new string[] { "teacherid", "zoneid" }, new object[] { rc, 145 }) > 0);

                if (!sec)
                {
                    loginerror.Value = "למשתמש אין הרשאות גישה לאפליקציה";
                    rc = 0;
                }
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
        }
    }
}