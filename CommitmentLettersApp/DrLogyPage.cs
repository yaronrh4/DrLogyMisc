using DrLogy.DrLogyUtils;
using DrLogyCookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CommitmentLettersApp
{
    public class DrLogyPage : Page
    {
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
                base.Render(writer);
                Response.Write("<!--" + (DateTime.Now - dtStart).TotalMilliseconds + "-->");
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