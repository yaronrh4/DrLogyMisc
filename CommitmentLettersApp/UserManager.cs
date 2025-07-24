using DocumentFormat.OpenXml.Wordprocessing;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace CommitmentLettersApp
{
    public static class UserManager
    {
        public static int UserId
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return (int)HttpContext.Current.Session["UserId"];
                }
                else
                {
                    string user = DrLogyCookies.CookielessUtils.GetCookie("UserId");
                    if (!string.IsNullOrEmpty(user))
                    {
                        UserId  = int.Parse(user);
                        return UserId;
                    }
                }
                return 0;
            }
            set
            {

                HttpContext.Current.Session["UserId"] = value;
                if (UserId> 0 )
                {
                    DbUtils.LogActivity(40 /*Login*/, 102 /*Commitments*/, UserId,UserId, 0);

                }
            }
        }

        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["UserName"] != null)
                {
                    return (string)HttpContext.Current.Session["UserName"];
                }
                else
                {
                    string userName = DrLogyCookies.CookielessUtils.GetCookie("UserName");
                    return userName;
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }
        public static void SetUserCookie()
        {
            DrLogyCookies.CookielessUtils.SetCookie("UserId", UserId.ToString());
            DrLogyCookies.CookielessUtils.SetCookie("UserName", UserName);
        }

        public static void Logout ()
        {
            int lastUserId = UserId;
            if (lastUserId > 0)
            {
                DbUtils.LogActivity( 41 /*Logout*/, 102 /*Commitments*/, lastUserId, lastUserId, 0);
            }
            HttpContext.Current.Session["UserId"] = null;
            HttpContext.Current.Session["UserName"] = null;
            DrLogyCookies.CookielessUtils.DeleteCookie("UserId");
            DrLogyCookies.CookielessUtils.DeleteCookie("UserName");
        }
    }
}