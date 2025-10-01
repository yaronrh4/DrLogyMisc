using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DrLogy.DrLogyUtils;

namespace CommitmentLettersApp
{
    public partial class Debug : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (DbUtils.GetDbParamValueInt(1000)==1)
            {
                pnl1.Visible= true;
            }
            if (!Page.IsPostBack)
            {
                txtDebugTime.Text = Utils.DateTimeNow().ToString();
            }
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            lblDate.Text = Utils.DateTimeNow().ToString();
            lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        protected void btnSetTime_Click(object sender, EventArgs e)
        {
            DateTime newDate = DateTime.Parse(txtDebugTime.Text);
            Utils.DebugTime = newDate;
            RefreshDisplay();

        }

        protected void btnClearTime_Click(object sender, EventArgs e)
        {
            Utils.DebugTime = null;
            RefreshDisplay();
        }
   }
}