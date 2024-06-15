using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommitmentLettersApp
{
    public static class Pref
    {
        public static int UserId
        {
            get
            {
                return int.Parse("0" + HttpContext.Current.Session["UserId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;
            }
        }
    }
}