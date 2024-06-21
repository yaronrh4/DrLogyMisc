using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
                    string user = Utils.GetCookie("UserId");
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
            }
        }

        public static void SetUserCookie()
        {
            Utils.SetCookie("UserId", UserId.ToString());
        }

        public static void Logout ()
        {
            UserId = 0;
            SetUserCookie();
        }
    }
}