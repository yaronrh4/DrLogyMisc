using DrLogy.DrLogyUtils;
using DrLogyCookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace CommitmentLettersApp
{
    public class DrLogyPage : Page
    {

        protected static string _version;

        internal static string Version
        {
            get
            {
                if (_version == null)
                {
                    var asm = Assembly.GetExecutingAssembly();

                    var descriptionAttribute = asm.GetCustomAttribute<AssemblyDescriptionAttribute>();
                    string description = descriptionAttribute?.Description ?? "No description available";

                    _version = $"{asm.GetName().Version.ToString()} {description}";
                }

                return _version;
            }
        }

        DateTime dtStart;

        protected override void OnPreInit(EventArgs e)
        {
            dtStart = DateTime.Now;

            base.OnPreInit(e);
        }



        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LogPrintScript", Utils.GetPrintLogJs());
        }

        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                Response.Write($"<!-- \r\n{Math.Round((DateTime.Now - dtStart).TotalMilliseconds)}ms\r\n V{Version}\r\n-->");

                base.Render(writer);
            }
            catch (Exception ex)
            {
            }
        }


        protected override void OnInit(EventArgs e)
        {
            //Ensure user disconnected 
            if (UserManager.UserId != 0 && Utils.CheckExpired(532))
            {
                UserManager.Logout();
                DrLogyCookies.CookielessUtils.Redirect("Login.aspx~Message=התקשורת עם השרת התנתקה, יש להתחבר שוב למערכת.");
                Response.End();
            }

            CookielessUtils.SetTimestamp(Utils.DateTimeNow());
            base.OnInit(e);
        }
    }
}