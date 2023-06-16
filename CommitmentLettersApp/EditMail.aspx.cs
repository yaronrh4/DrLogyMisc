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

namespace CommitmentLettersApp
{
    public partial class EditMail : System.Web.UI.Page
    {

        private const string OPTIONS_FILENAME = "letteroptions.xml";
        private LettersPDF _lettersPDF = null;
        private LettersPDFOptions _options = null;
        private string _connection = null;
        private string _project = null;
        List<mailitem> _mails = null;

        [Serializable]
        private class mailitem
        {
            public string MailSubject { get; set; }
            public string MailBody { get; set; }
            public string MailAddress { get; set; }
            public bool Sent { get; set; }
        }

        public LettersPDF lettersPDF
        {
            get {
                return _lettersPDF;
            }
        }


        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                drpType.SelectedIndex = 0;
                LoadHtml();
            }
        }

        private void LoadHtml()
        {
            if (drpType.SelectedIndex >= 0)
            {
                string htmlName = MapPath("Templates/" + Utils.GetAppSetting($"MailMessage{drpType.SelectedValue}", ""));
                string subjectName = MapPath("Templates/" + Utils.GetAppSetting($"MailSubject{drpType.SelectedValue}", ""));

                string html = System.IO.File.ReadAllText(htmlName);
                string subject = System.IO.File.ReadAllText(subjectName);


                editorcontent.InnerText = html;
                txtSubject.Text = subject;
            }
        }

        private void SaveHTML()
        {
            if (drpType.SelectedIndex >= 0)
            {
                string htmlName = MapPath("Templates/" + Utils.GetAppSetting($"MailMessage{drpType.SelectedValue}" ,""));
                System.IO.File.WriteAllText(htmlName, editorcontent.InnerText);
                string subjectName = MapPath("Templates/" + Utils.GetAppSetting($"MailSubject{drpType.SelectedValue}", ""));
                System.IO.File.WriteAllText(subjectName, txtSubject.Text);
            }
        }

        protected void drpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHtml();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveHTML();
            successhidden.Value = "התבנית נשמרה בהצלחה";
        }
    }
}