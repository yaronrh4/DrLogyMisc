using DocumentFormat.OpenXml.Wordprocessing;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string connection = Utils.GetAppSetting("Connection", "");
            loginerror.Value = "";
            DrLogy.DrLogyUtils.DbUtils.ConStr = connection;

            int rc = (int)DbUtils.ExecSP("SPMISC_LOGIN", new string[] { "UserNameOrZehut", "Pass" }, new object[] { inputUsername.Value, inputPassword.Value });
            if (rc == 0)
            {
                loginerror.Value = "שגיאה בהתחברות";
            }
            else
            {
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
                if (chkRememberMe.Checked)
                {
                    UserManager.SetUserCookie();
                }

                Response.Redirect("Default.aspx");
            }
        }
    }
}