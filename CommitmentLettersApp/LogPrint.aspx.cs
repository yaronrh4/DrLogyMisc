
    using DrLogy.DrLogyUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    namespace CommitmentLettersApp
    {
        public partial class LogPrint : System.Web.UI.Page
        {
            protected void Page_Load(object sender, EventArgs e)
            {
                if (Request["Page"] != null)
                {
                    int teacherId = UserManager.UserId;
                    DbUtils.LogActivity(25 /*Print*/, 102 /*Commitments*/, teacherId, 0, 0, 0, Request["page"]);
                }

            }
        }
    }