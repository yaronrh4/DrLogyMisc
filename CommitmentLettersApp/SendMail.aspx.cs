using DocumentFormat.OpenXml.Drawing;
using DrLogy.CommitmentLettersUtils;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Security.Authentication.ExtendedProtection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace CommitmentLettersApp
{
    public partial class SendMail : System.Web.UI.Page
    {

        private LettersPDF _lettersPDF = null;
        List<mailitem> _mails = null;
        string _bcc = "";

        [Serializable]
        private class mailitem
        {
            public int StId { get; set; }
            public string MailSubject { get; set; }
            public string MailBody { get; set; }
            public string MailAddress { get; set; }
            public string WhatsappPhone { get; set; }
            public string RakazEmail { get; set; }
            public bool Sent { get; set; }
        }

        public LettersPDF lettersPDF
        {
            get
            {
                return _lettersPDF;
            }
        }


        private List<mailitem> ProcessMails(string testEmail)
        {
            string name = "";
            List<string> subjects = null;
            List<decimal?> subjectHours = null;

            string comments = "";
            string rakazName = "";
            string rakazPhone = "";
            string email = "";
            string phone = "";
            string rakazEmail = "";
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

                    name = r.FirstName + ' ' + r.LastName;
                    if (string.IsNullOrWhiteSpace (name))
                        name = r.CurrFirstName + ' ' + r.CurrLastName;

                    subjects = new List<string>();

                    subjectHours = new List<decimal?>();
                    startDate = r.StartDate;
                    endDate = r.EndDate;
                    comments = "";
                    mailType = r.IsNewStudent.GetValueOrDefault() ? 1 : 2;
                    rakazName = r.CoordinatorName;
                    rakazPhone = lettersPDF.Options.Coordinators.First(x => x.Name == rakazName).Phone;
                    rakazEmail = lettersPDF.Options.Coordinators.First(x => x.Name == rakazName).Email;

                    //studentRows.Add(i);
                    for (int j = 0; j < r.Subjects.Count; j++)
                    {
                        SubjectData s = r.Subjects[j];
                        email = string.IsNullOrEmpty(testEmail) ? r.CurrEmail : testEmail;
                        if (string.IsNullOrEmpty(email))
                            email = r.Email;

                        phone = r.CurrPhone;
                        subjects.Add(_lettersPDF.Options.Subjects.First(x => x.NameInFile == s.SubjectInFile).NameInDB);
                        subjectHours.Add(s.Hours);
                    }

                    if (!string.IsNullOrWhiteSpace(r.Comments))
                    {
                        //if (comments != "")
                        //    comments += "<br/>";
                        comments += r.Comments;
                    }
                }

                if (subjects != null && subjects.Count > 0)
                {
                    //Create email
                    DataTable dt = DbUtils.GetSPData("SPMISC_GET_USER_TEMPLATE", new string[] { "TEACHER_ID", "TEMPLATE_TYPE" }, new object[] { -1, mailType });

                    if (dt.Rows.Count == 0)
                        throw new Exception("שגיאה בטעינת תבנית מייל");

                    string html = (string)dt.Rows[0]["ACT_HTML"];
                    string subject = (string)dt.Rows[0]["ACT_SUBJECT"];

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
                        StId = r.Id,
                        MailAddress = email,
                        WhatsappPhone = Utils.CleanPhoneNumber(phone, "+972"),
                        MailBody = html,
                        MailSubject = subject,
                        RakazEmail = rakazEmail
                    };

                    mailitems.Add(m);
                    //}

                }
            }

            return mailitems;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            _lettersPDF = Session["lettersPDF"] as LettersPDF;
            _bcc = Utils.GetAzureEnvironmentVariable("MailBcc");

            if (!Page.IsPostBack)
            {
                if (UserManager.UserId == 0)
                    Response.Redirect("Login.aspx");

                var m = ProcessMails(txtTestEmail.Text);

                rep1.DataSource = m;

                ViewState["src"] = rep1.DataSource;
                rep1.DataBind();
            }
            else
            {
                _mails = ViewState["src"] as List<mailitem>;
            }
        }

        protected void btnSendAllMails_Click(object sender, EventArgs e)
        {
            if (UserManager.UserId == 0)
                Response.Redirect("Login.aspx");

            EmailSender eml = new EmailSender();
            int cnt = 0;
            foreach (var m in _mails)
            {
                cnt++;
                string mailAddress = txtTestEmail.Text != "" ? txtTestEmail.Text : m.MailAddress;
                eml.SendEmail(m.StId,mailAddress, "", m.MailBody, m.MailSubject, null, false, _bcc , m.RakazEmail);
            }
            successhidden.Value = $"{cnt} מיילים נשלחו בהצלחה";

        }

        protected void btnSendSelectedMails_Click(object sender, EventArgs e)
        {
            if (UserManager.UserId == 0)
                Response.Redirect("Login.aspx");

            EmailSender eml = new EmailSender();
            int cnt = 0;
            for (int i = 0; i < _mails.Count; i++)
            {
                if (((CheckBox)rep1.Items[i].FindControl("chk1")).Checked)
                {
                    ((CheckBox)rep1.Items[i].FindControl("chk1")).Checked = false;
                    mailitem m = _mails[i];
                    cnt++;
                    string mailAddress = txtTestEmail.Text != "" ? txtTestEmail.Text : m.MailAddress;
                    eml.SendEmail(m.StId,mailAddress, "", m.MailBody, m.MailSubject, null, false, _bcc , m.RakazEmail);
                }
            }
            successhidden.Value = $"{cnt} מיילים נשלחו בהצלחה";

        }

        public static string EscapeHtml(string html)
        {
            string escaped = System.Web.HttpUtility.HtmlEncode(html);


            return escaped;
        }

    }
}