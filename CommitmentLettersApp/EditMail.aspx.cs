using DrLogy.CommitmentLettersUtils;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;

namespace CommitmentLettersApp
{
    public partial class EditMail : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (UserManager.UserId == 0)
                    Response.Redirect("Login.aspx");


                DataTable dtType = DbUtils.GetSPData("SPMISC_GET_TEMPLATE_TYPES");
                    
                    //DbUtils.GetSPData("SPMISC_GET_TEMPLATE_TYPES");
                drpType.DataSource = dtType;
                drpType.DataValueField = "ACTT_ID";
                drpType.DataTextField  = "ACTT_NAME";
                drpType.DataBind();
                drpType.SelectedIndex = 0;
                LoadHtml();
            }
        }

        private void LoadHtml()
        {
            if (drpType.SelectedIndex >= 0)
            {
                DataTable dt = DbUtils.GetSPData("SPMISC_GET_USER_TEMPLATE", new string[] { "TEACHER_ID", "TEMPLATE_TYPE" }, new object[] { -1, int.Parse(drpType.SelectedValue) });

                if (dt.Rows.Count > 0)
                {
                    string html = (string)dt.Rows[0]["ACT_HTML"];
                    string subject = (string)dt.Rows[0]["ACT_SUBJECT"];


                    editorcontent.InnerText = html;
                    txtSubject.Text = subject;
                }
                else 
                    throw new Exception("שגיאה בטעינת תבנית");
            }
            else
                throw new Exception("לא נבחר סוג תבנית לעריכה");

        }

        private void SaveHTML()
        {
            if (drpType.SelectedIndex >= 0)
            {
                DbUtils.ExecSP("SPMISC_SET_USER_TEMPLATE", new string[] { "TEACHER_ID", "TEMPLATE_TYPE", "TEMPLATE_SUBJECT", "TEMPLATE_HTML" ,"CDATE"}, new object[] { -1, int.Parse(drpType.SelectedValue), txtSubject.Text, editorcontent.InnerText , Utils.DateTimeNow() }, true);
            }
        }

        protected void drpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHtml();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (UserManager.UserId == 0)
                Response.Redirect("Login.aspx");

            SaveHTML();
            successhidden.Value = "התבנית נשמרה בהצלחה";
        }
    }
}