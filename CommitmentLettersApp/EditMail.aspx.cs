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

        private List<mailitem> ProcessMails(string testEmail)
        {
            string name = "";
            List<string> subjects = null;
            List<decimal> subjectHours = null;

            string comments = "";
            string rakazName = "";
            string rakazPhone = "";
            string email = "";
            DateTime? startDate = null;
            DateTime? endDate = null;
            int mailType = 0;
            List<int> studentRows = null;

            List<mailitem> mailitems = new List<mailitem>();


            for (int i = 0; i < lettersPDF.Results.Count; i++)
            {
                var r = lettersPDF.Results[i];
                if (true/*r.IsSelected*/)
                {
                    //r.IsSelected = false;
                    studentRows = new List<int>();

                    name = r.Name;
                    subjects = new List<string>();
                    subjectHours = new List<decimal>();
                    startDate = r.StartDate;
                    endDate = r.EndDate;
                    comments = "";
                    mailType = 1;//(r.Status == StudentStatus.NoStudent) || (r.Status == StudentStatus.NoStudent) ? 1 : 2;
                    rakazName = r.CoordinatorName;
                    rakazPhone = lettersPDF.Options.Coordinators.First(x => x.Name == rakazName).Phone;

                    //studentRows.Add(i);
                    for (int j = 0; j < r.Subjects.Count; j++)
                    {
                        SubjectData s = r.Subjects[j];
                        email = string.IsNullOrEmpty(testEmail) ? r.CurrEmail : testEmail;

                        subjects.Add(_lettersPDF.Options.Subjects.First(x => x.BTLName == s.SubjectBTL).Name);
                        subjectHours.Add(s.Hours);

                        if (!string.IsNullOrWhiteSpace(r.Comments))
                        {
                            if (comments != "")
                                comments += "<br/>";
                            comments += r.Comments;
                        }

                    }
                }

                if (subjects != null && subjects.Count > 0)
                {
                    //Create email
                    string html = Utils.GetAppSetting($"MailMessage{mailType}", "");
                    string subject = Utils.GetAppSetting($"MailSubject{mailType}", "");

                    if (comments != "")
                        comments = "הערות: " + comments;

                    html = html.Replace("|שם|", name);
                    html = html.Replace("|התחלה|", startDate.Value.ToString("dd/MM/yyyy"));
                    html = html.Replace("|סוף|", endDate.Value.ToString("dd/MM/yyyy"));
                    html = html.Replace("|הערות|", comments);
                    html = html.Replace("|רכז|", rakazName);
                    html = html.Replace("|טלפון רכז|", rakazPhone);

                    string subjectsText = "";
                    for (int j = 0; j < subjects.Count; j++)
                    {
                        if (j > 0)
                            subjectsText += "<br/>";
                        string hourType = subjects[j].IndexOf("אסטרטגיות") < 0 ? "חודשיות" : "שנתיות";
                        subjectsText += $"מכסת השעות עבור {subjects[j]} היא {Convert.ToInt32(subjectHours[j])} שעות {hourType}.";
                    }

                    //if (email != "")
                    //{
                        html = html.Replace("|הנגשות|", subjectsText);
                        html = "<div dir='rtl'>" + html + "</div>";

                        mailitem m = new mailitem()
                        {
                            MailAddress = email,
                            MailBody = html,
                            MailSubject = subject
                        };

                        mailitems.Add(m);
                    //}

                }
            }

            return mailitems;
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
                string html = Utils.GetAppSetting($"MailMessage{drpType.SelectedValue}", "");
                editorcontent.InnerText = html;
                txtSubject.Text = Utils.GetAppSetting($"MailSubject{drpType.SelectedValue}", "");
            }
        }

        private void SaveHTML()
        {
            if (drpType.SelectedIndex >= 0)
            {
                Utils.SetAppSetting($"MailMessage{drpType.SelectedValue}", editorcontent.InnerText);
                Utils.SetAppSetting($"MailSubject{drpType.SelectedValue}", txtSubject.Text);
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