﻿using DrLogy.CommitmentLettersUtils;
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
    public partial class SendMail : System.Web.UI.Page
    {

        private const string OPTIONS_FILENAME = "letteroptions.xml";
        private LettersPDF _lettersPDF = null;
        private LettersPDFOptions _options = null;
        private string _connection = null;
        private string _project = null;
        List<mailitem> _mails = null;
        string _bcc = "";

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
            get
            {
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
            _lettersPDF = Session["lettersPDF"] as LettersPDF;

            if (!Page.IsPostBack)
            {
                _bcc = Utils.GetAppSetting("MailBcc", "");
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

        protected void btnSaveSubject_Click(object sender, EventArgs e)
        {
            LetterData r = _lettersPDF.Results[int.Parse(stsubidx.Value)];
            SubjectData s = r.Subjects[int.Parse(subjectidx.Value)];

            r.StartDate = DateTime.ParseExact(startdate.Value.Trim(), "dd/MM/yyyy", null);
            r.EndDate = DateTime.ParseExact(enddate.Value.Trim(), "dd/MM/yyyy", null);

            s.Hours = int.Parse(hours.Value);

            _lettersPDF.RefreshStatus(int.Parse(stsubidx.Value), int.Parse(subjectidx.Value));
            RefreshData();
        }
        protected void btnAddPdf_Click(object sender, EventArgs e)
        {
            string filename = $"c:\\temp\\{fuPdfs.PostedFiles[0].FileName}";
            fuPdfs.PostedFiles[0].SaveAs(filename);
            _lettersPDF.Process(filename, _connection, _project);
            //RefreshData();
            RefreshData();
        }

        private void RefreshData()
        {
            //rep1.DataSource = _lettersPDF.Results;
            //rep1.DataBind();
        }

        protected void btnSaveStudent_Click(object sender, EventArgs e)
        {
            LetterData r = _lettersPDF.Results[int.Parse(stidx.Value)];

            r.CurrFirstName = firstname.Value;
            r.CurrLastName = lastname.Value;
            r.IdNum = idnum.Value;
            r.CurrPhone = phone.Value;
            r.CurrEmail = email.Value;
            r.CoordinatorName = coordinatorname.Value;
            r.Branch = branch.Value;
            r.SocialWorker = socialworker.Value;

            //_lettersPDF.UpdateStudent(int.Parse(stidx.Value) , _connection);

            //_lettersPDF.RefreshStatus(int.Parse(stsubidx.Value), int.Parse(subjectidx.Value));
            RefreshData();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
        }

        protected void btnSendAllMails_Click(object sender, EventArgs e)
        {
            EmailSender eml = new EmailSender();
            int cnt = 0;
            foreach (var m in _mails)
            {
                cnt++;
                string mailAddress = txtTestEmail.Text != "" ? txtTestEmail.Text : m.MailAddress;
                eml.SendEmail(mailAddress, "", m.MailBody, m.MailSubject, null, false, _bcc);
            }
            successhidden.Value = $"{cnt} מיילים נשלחו בהצלחה";

        }

        protected void btnSendSelectedMails_Click(object sender, EventArgs e)
        {
            EmailSender eml = new EmailSender();
            int cnt = 0;
            for (int i = 0; i < _mails.Count; i++)
            {
                if (((CheckBox)rep1.Items[i].FindControl("chk1")).Checked)
                {
                    mailitem m = _mails[i];
                    cnt++;
                    eml.SendEmail(m.MailAddress, "", m.MailBody, m.MailSubject, null, false, _bcc);
                }
            }
            successhidden.Value = $"{cnt} מיילים נשלחו בהצלחה";

        }
    }
}