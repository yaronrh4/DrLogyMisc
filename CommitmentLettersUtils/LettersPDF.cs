using DrLogy.DrLogyPDFUtils;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DrLogy.CommitmentLettersUtils
{
    public class LettersPDF
    {
        private List<LetterData> _results = new List<LetterData>();
        private readonly LettersPDFOptions _options = null;

        public LettersPDF(LettersPDFOptions options)
        {
            _options = options;
        }

        public LettersPDFOptions Options
        {
            get { return _options; }
        }

        public void Process(string filename, string connectionString, string project)
        {
            RemoveOld(filename);

            PDFUtils utils = new PDFUtils();
            LetterData data = new LetterData();

            data.PageNumber = 1;
            data.FileName = filename;
            data.Project = project;
            data.CoordinatorName = _options.Coordinators[0].Name;
            utils.OpenPdf(filename);

            List<RectangleF> rec = utils.SearchPage(_options.Title);
            RectangleF newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
            string s = utils.ExtractText(newRec, 1);
            int i = s.LastIndexOf("-");
            s = s.Substring(0, i);
            i = s.LastIndexOf(".");

            data.IdNum = s.Substring(0, i - 4).Trim();
            data.Name = Utils.FixRTLString(s.Substring(i + 6).Trim());
            i = data.Name.IndexOf(" ");
            if (i > 0)
            {
                data.FirstName = data.Name.Substring(0, i);
                data.LastName = data.Name.Substring(i + 1);
            }
            else
            {
                data.FirstName = data.Name;
                data.LastName = "";
            }


            rec = utils.SearchPage(_options.Dates);
            newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
            s = utils.ExtractText(newRec, 1);
            i = s.IndexOf(":ר‎ו‎ב‎ע‎");
            data.EndDate = DateTime.ParseExact(s.Substring(i + 10, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            data.StartDate = DateTime.ParseExact(s.Substring(i + 29, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            rec = utils.SearchPage(_options.SocialWorker);
            newRec = new RectangleF(0, rec[0].Y - 14, rec[0].X + 100, rec[0].Height);
            s = utils.ExtractText(newRec, 1).Trim();
            data.SocialWorker = Utils.FixRTLString(s);

            rec = utils.SearchPage(_options.Branch);
            newRec = new RectangleF(0, rec[rec.Count() - 1].Y, rec[rec.Count() - 1].X, rec[rec.Count() - 1].Height);
            s = utils.ExtractText(newRec, 1).Trim();
            data.Branch = Utils.FixRTLString(s);

            rec = utils.SearchPage(_options.Phone);
            newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
            s = utils.ExtractText(newRec, 1).Trim();
            data.Phone = s;

            _results.Add(data);
            GetSubjectsFromPDF(_options.Subjects, utils);

            for (i = 0; i < Results.Count; i++)
                for (int j = 0; j < Results[i].Subjects.Count; j++)
                    GetDataFromDB(i, j, connectionString);

        }
        public string GetDbSubject(string btlSubject)
        {
            string subject = "";
            if (!string.IsNullOrEmpty(btlSubject))
            {
                var item = _options.Subjects.Where(t => t.BTLName == btlSubject).FirstOrDefault();

                if (item != null)
                {
                    subject = item.Name;
                }
            }

            return subject;
        }

        public void GetDataFromDB(int index, int subjectIndex, string connectionString, bool overrideDetails = false)
        {
            LetterData r = Results[index];
            SubjectData subject = r.Subjects[subjectIndex];
            //data.CurrName = null;
            //data.CurrHours = null;
            //data.CurrStartDate = null;
            //data.CurrEndDate = null;
            Results[index].Id = 0;

            DataTable dt = null;
            //sql

            DrLogy.DrLogyUtils.DbUtils.ConStr = connectionString;

            //בדיקה אם קיים התלמיד בפרוייקט ובהתנגשה הספציפיים
            if (subject.SubjectBTL != null && subject.SubjectBTL != "" && r.StartDate.HasValue && r.EndDate.HasValue && subject.Hours > 0)
            {
                dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut", "project", "subject", "hours", "start_date", "end_date" }, new object[] { r.IdNum, r.Project, subject.SubjectInDB, subject.Hours, r.StartDate.Value, r.EndDate.Value });

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[dt.Rows.Count - 1];
                    subject.CurrStartDate = (DateTime)row["ST_SIGNDATE"];
                    subject.CurrEndDate = (DateTime)row["ST_DATEEND"];
                    subject.CurrHours = (decimal)row["ST_MAX_HOURS"];
                    subject.CurrSubjectBTL = subject.SubjectBTL;
                }


                if (dt != null && dt.Rows.Count == 0 && subject.SubjectInDB != null && subject.SubjectInDB != "")
                {
                    //בדיקה אם קיים תלמיד בפרוייקט הספציפי בהנגשה הנוכחית עם נתונים אחרים
                    dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut", "project", "subject" }, new object[] { r.IdNum, r.Project, subject.SubjectInDB });

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[dt.Rows.Count - 1];

                        if (!(row["ST_SIGNDATE"] is DBNull))
                            subject.CurrStartDate = (DateTime)row["ST_SIGNDATE"];

                        if (!(row["ST_DATEEND"] is DBNull))
                            subject.CurrEndDate = (DateTime)row["ST_DATEEND"];

                        if (!(row["ST_MAX_HOURS"] is DBNull))
                            subject.CurrHours = (decimal)row["ST_MAX_HOURS"];
                        subject.CurrSubjectBTL = subject.SubjectBTL;

                    }
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    //בדיקה אם קיים תלמיד עם ת.ז  הנוכחית - בפרוייקט אחר / הנגשה אחרת
                    dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut" }, new object[] { r.IdNum });

                }

                //עדכון כתובת ושם התלמיד מבסיס הנתונים - במידה וקיים
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[dt.Rows.Count - 1];
                    r.Id = (int)row["ST_ID"];
                    r.CurrFirstName = (string)row["ST_FNAME"];
                    r.CurrLastName = (string)row["ST_LNAME"];
                    r.CurrEmail = row["ST_EMAIL"].ToString();
                    r.CurrPhone = row["ST_PHONE1"].ToString();
                    string rName = row["RAKAZ_NAME"].ToString();
                    if (rName != "")
                    {
                        if (_options.Coordinators.Where(z => z.Name == rName).FirstOrDefault() != null)
                            r.CoordinatorName = rName;
                    }

                    if (overrideDetails)
                    {
                        r.FirstName = r.CurrFirstName;
                        r.LastName = r.LastName;
                        r.Name = r.FirstName + " " + r.LastName;
                        subject.SubjectBTL = subject.CurrSubjectBTL;
                        r.Email = r.CurrEmail;
                        r.Phone = r.CurrPhone;
                    }
                }

                RefreshStatus(index, subjectIndex);
            }
        }


        private void GetSubjectsFromPDF(List<Subject> subjects, PDFUtils utils)
        {
            List<RectangleF> rec;
            RectangleF newRec;
            string s;
            int i;
            int groupedCount = 0;
            decimal hours;
            SubjectData newSubject = null;

            LetterData data = _results[_results.Count - 1];
            data.Subjects = new List<SubjectData>();

            //count grouped subjects
            foreach (var subject in subjects)
            {
                rec = utils.SearchPage(subject.BTLName);
                if (rec.Count() > 0)
                {
                    if (subject.Grouped)
                        groupedCount++;
                }
            }
            foreach (var subject in subjects)
            {
                rec = utils.SearchPage(subject.BTLName);
                if (rec.Count() > 0)
                {
                    if (subject.Hours > 0)
                        hours = subject.Hours;
                    else
                    {
                        //חיפוש השעות מתוך הטקסט

                        newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
                        s = utils.ExtractText(newRec, 1).Trim();
                        i = s.LastIndexOf("ת‎ו‎ע‎ש‎");
                        if (i == -1)
                        {
                            i = s.IndexOf(" ");
                            s = s.Substring(i + 1);

                            i = s.IndexOf(" ");
                            s = s.Substring(0, i);

                        }
                        else
                        {
                            int i1 = s.IndexOf("תועש") + 9;// "שעות"
                            int i2 = s.IndexOf(" ", i1);// "שעות"
                            s = s.Substring(i1, i2 - i1);
                        }

                        hours = decimal.Parse(s);
                    }

                    newSubject = new SubjectData();
                    newSubject.SubjectBTL = subject.BTLName;
                    newSubject.SubjectInDB = subject.Name;
                    newSubject.Hours = subject.Grouped && groupedCount > 1 ? hours / 2 : hours;

                    data.Subjects.Add(newSubject);
                }
            }
        }

        public void RemoveOld(string fileName)
        {
            for (int j = _results.Count - 1; j >= 0; j--)
            {
                if (_results[j].FileName == fileName)
                    _results.RemoveAt(j);
            }
        }

        public void RefreshStatus(int rowIndex, int subjectIndex)
        {
            LetterData r = _results[rowIndex];
            SubjectData subject = r.Subjects[subjectIndex];

            if (r.Id > 0)
            {

                if (subject != null && !string.IsNullOrEmpty(subject.SubjectBTL) /*&& subject == r.CurrSubjectBTL*/)
                {
                    if (subject.Hours == subject.CurrHours && r.StartDate == subject.CurrStartDate && r.EndDate == subject.CurrEndDate)
                        subject.Status = StudentStatus.Updated;
                    else
                        subject.Status = StudentStatus.NotUpdated;
                }
                else
                {
                    subject.Status = StudentStatus.NoSubject;
                }
            }
            else
                subject.Status = StudentStatus.NoStudent;
        }

        public int UpdateStudent(int rowIndex, string connectionString)
        {
            int rc = 0;
            DrLogy.DrLogyUtils.DbUtils.ConStr = connectionString;

            LetterData r = _results[rowIndex];
            {
                if (r.IdNum != null)
                    r.IdNum = r.IdNum.Trim();

                if (r.FirstName != null)
                    r.FirstName = r.FirstName.Trim();
                if (r.LastName != null)
                    r.LastName = r.LastName.Trim();
                if (r.Phone != null)
                    r.Phone = r.Phone.Trim();
                if (r.Branch != null)
                    r.Branch = r.Branch.Trim();
                if (r.SocialWorker != null)
                    r.SocialWorker = r.SocialWorker.Trim();
                if (r.Phone != null)
                    r.Phone = r.Phone.Trim();

                float idnum = 0;
                if (float.TryParse(r.IdNum, out idnum) && idnum > 0)
                {
                    DbUtils.ExecSP("SPMISC_UPDATE_STUDENT", new string[] { "st_id", "zehut", "fname", "lname", "project", "phone", "city", "parentname" }, new object[] { r.Id, r.IdNum, r.CurrFirstName, r.CurrLastName, r.Project, r.CurrPhone, r.Branch, r.SocialWorker });

                    //yaron to check
                    //for (int i = 0; i < _results.Count; i++)
                    //{
                    //    float oldIdnum = 0;

                    //    if (float.TryParse(_results[i].IdNum, out oldIdnum) && oldIdnum == idnum)
                    //    {
                    //        GetDataFromDB(i, connectionString);
                    //    }
                    //}
                    //rc = 1; 
                }
            }

            return rc;
        }
        public int UpdateRowToDb(int rowIndex, int subjectIndex)
        {
            LetterData r = _results[rowIndex];
            SubjectData subject = r.Subjects[subjectIndex];
            int rc = 0;

            if (!subject.Updated && subject.Status != StudentStatus.Updated)
            {
                subject.Updated = true;
                rc = 1;

                if (subject.Status == StudentStatus.NotUpdated)
                {
                    DbUtils.ExecSP("SPMISC_UPDATE_SUBJECT", new string[] { "st_id", "start_date", "end_date", "hours" }, new object[] { _results[rowIndex].Id, r.StartDate, r.EndDate, subject.Hours });
                }
                else if (subject.Status == StudentStatus.NoSubject)
                {
                    DbUtils.ExecSP("SPMISC_INSERT_SUBJECT", new string[] { "st_id", "project", "subject", "start_date", "end_date", "hours" }, new object[] { _results[rowIndex].Id, r.Project, subject.SubjectInDB, r.StartDate, r.EndDate, subject.Hours });

                }
                else
                {
                    subject.Updated = false;
                    rc = 0;
                }
            }
            return rc;
        }
        public List<LetterData> Results
        {
            get { return _results; }
            set { _results = value; }
        }
    }
}
