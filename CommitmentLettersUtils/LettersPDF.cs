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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;

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

        public void LoadStudent(string idNum, string[] subjects, DateTime startDate , DateTime endDate, string connectionString, string project)
        {
            PDFUtils utils = new PDFUtils();
            LetterData data = new LetterData();

            data.PageNumber = 1;
            data.FileName = "";
            data.Project = project;
            data.CoordinatorName = _options.DefaultCoordinatorName;
            data.CreateDate = DateTime.Now.Date;
            if (data.CoordinatorName == "")
                data.CoordinatorName = _options.Coordinators[0].Name;

            data.IdNum = idNum;
            data.EndDate = endDate;
            data.StartDate = startDate;

            DrLogy.DrLogyUtils.DbUtils.ConStr = connectionString;

            //בדיקה אם קיים תלמיד בפרוייקט הספציפי בהנגשה הנוכחית עם נתונים אחרים
            DataTable dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut", "project"}, new object[] { idNum, project });
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[dt.Rows.Count - 1];
                data.Id = (int)row["ST_ID"];
                data.FirstName = (string)row["ST_FNAME"];
                data.LastName = (string)row["ST_LNAME"];
                data.Name = data.FirstName + " " + data.LastName;
                data.SocialWorker = (string)row["ST_PARENTNAME"];
                data.Branch = (string)row["ST_CITY"];
                data.Phone = (string)row["ST_PHONE1"];
                data.Email = (string)row["ST_EMAIL"];
            }

            //Subjects
            SubjectData newSubject = null;
            data.Subjects = new List<SubjectData>();

            foreach (var selectedsubject in subjects)
            {
                var subject = this._options.Subjects.Find(z => z.Name == selectedsubject);

                if (subject != null)
                {
                    newSubject = new SubjectData();
                    newSubject.SubjectBTL = subject.BTLName;
                    newSubject.SubjectInDB = subject.Name;
                    newSubject.Hours = 1; //dummy value would be updated letter
                    data.Subjects.Add(newSubject);
                }
            }

            _results.Add(data);
            
            for (int i = 0; i < Results.Count; i++)
                for (int j = 0; j < Results[i].Subjects.Count; j++)
                    GetDataFromDB(i, j, connectionString);

            foreach (var subject in data.Subjects)
            {
                subject.Hours = subject.CurrHours.GetValueOrDefault();
            }

            for (int i=0; i < Results[Results.Count-1].Subjects.Count; i++)
            {
                RefreshStatus(Results.Count - 1, i);
            }
        }

        public void Process(string filename, string connectionString, string project)
        {
            RemoveOld(filename);

            PDFUtils utils = new PDFUtils();
            LetterData data = new LetterData();

            data.PageNumber = 1;
            data.FileName = filename;
            data.Project = project;
            data.CoordinatorName = _options.DefaultCoordinatorName;
            data.CreateDate = DateTime.Now.Date;
            if (data.CoordinatorName == "")
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

            if (data.SocialWorker.IndexOf("עוס") < 0 && data.SocialWorker.IndexOf("עו\"ס") < 0)
                data.SocialWorker = "עו\"ס " + data.SocialWorker;

            var allRec = utils.SearchAllPages(_options.Branch);
            rec = allRec[allRec.Count - 1].res;
            newRec = new RectangleF(0, rec[rec.Count() - 1].Y, rec[rec.Count() - 1].X, rec[rec.Count() - 1].Height);
            s = utils.ExtractText(newRec, allRec[allRec.Count - 1].pageNumber).Trim();
            data.Branch = Utils.FixRTLString(s);

            rec = utils.SearchPage(_options.Phone);
            newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
            s = utils.ExtractText(newRec, 1).Trim();
            data.Phone = s;
            rec = utils.SearchPage(_options.Email);
            newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
            s = utils.ExtractText(newRec, 1).Trim();
            data.Email = s;

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
                    subject.IsNew = true;
                }

                //עדכון כתובת ושם התלמיד מבסיס הנתונים - במידה וקיים
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[dt.Rows.Count - 1];
                    r.Id = (int)row["ST_ID"];
                    r.IsNewStudent = false;
                    r.CurrFirstName = (string)row["ST_FNAME"];
                    r.CurrLastName = (string)row["ST_LNAME"];
                    r.CurrEmail = row["ST_EMAIL"].ToString();
                    r.CurrPhone = row["ST_PHONE1"].ToString();
                    r.CurrBranch = row["ST_CITY"].ToString();
                    r.CurrSocialWorker = row["ST_PARENTNAME"].ToString();
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
            List<SubjectData> groupedSubjects = new List<SubjectData>();
            decimal groupedHours = 0;

            decimal hours;
            SubjectData newSubject = null;

            LetterData data = _results[_results.Count - 1];
            data.Subjects = new List<SubjectData>();

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
                    newSubject.Hours = hours;

                    if (subject.Grouped)
                    {
                        groupedSubjects.Add(newSubject);
                        groupedHours += hours;
                    }

                    data.Subjects.Add(newSubject);
                }
            }

            if (groupedSubjects.Count > 1)
            {
                //set the average of the hours for the grouped subject
                foreach (var sub in groupedSubjects)
                    sub.Hours = groupedHours / Convert.ToDecimal(groupedSubjects.Count) / Convert.ToDecimal(groupedSubjects.Count);
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

                if (!subject.IsNew)
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

        public string UpdateStudent(int rowIndex, string connectionString)
        {
            string rc = "";
            DrLogy.DrLogyUtils.DbUtils.ConStr = connectionString;

            LetterData r = _results[rowIndex];
            if (r.IdNum != null)
                r.IdNum = r.IdNum.Trim();

            if (r.CurrFirstName == null)
            {
                r.CurrFirstName = r.FirstName;
                r.CurrLastName = r.LastName;
                r.CurrPhone = r.Phone;
                r.CurrEmail = r.Email;
                r.CurrBranch = r.Branch;
                r.CurrSocialWorker = r.SocialWorker;
            }

            float idnum = 0;
            if (float.TryParse(r.IdNum, out idnum) && idnum > 0)
            {
                try
                {
                    var z = DbUtils.ExecSP("SPMISC_UPDATE_STUDENT", new string[] { "st_id", "zehut", "fname", "lname", "project", "phone", "city", "parentname", "email" }, new object[] { r.Id, r.IdNum.Trim(), r.CurrFirstName.Trim(), r.CurrLastName.Trim(), r.Project.Trim(), r.CurrPhone.Trim(), r.CurrBranch.Trim(), r.CurrSocialWorker.Trim() , r.CurrEmail }, true);
                    r.Id = (int)z;
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
                catch (Exception ex)
                {
                    rc = "שגיאה בשמירה בבסיס הנתונים, הנתונים לא נשמרו";
                }
            }
            else
                rc = "תעודת הזהות אינה תקינה, הנתונים לא נשמרו";

            return rc;
        }
        public string UpdateSubject(int rowIndex, int subjectIndex)
        {
            LetterData r = _results[rowIndex];
            SubjectData subject = r.Subjects[subjectIndex];
            string rc = "";

            if (/*!subject.Updated && */subject.Status != StudentStatus.Updated)
            {
                subject.Updated = false;
                try
                {
                    if (subject.Status == StudentStatus.NotUpdated)
                    {
                        DbUtils.ExecSP("SPMISC_UPDATE_SUBJECT", new string[] { "st_id", "subject", "curr_start_date", "curr_end_date", "start_date", "end_date", "hours" }, new object[] { _results[rowIndex].Id, subject.SubjectInDB, subject.CurrStartDate, subject.CurrEndDate, r.StartDate, r.EndDate, subject.Hours });
                        subject.Updated = true;
                    }
                    else if (subject.Status == StudentStatus.NoSubject || subject.Status == StudentStatus.NoStudent)
                    {
                        DbUtils.ExecSP("SPMISC_INSERT_SUBJECT", new string[] { "st_id", "project", "subject", "start_date", "end_date", "hours" }, new object[] { _results[rowIndex].Id, r.Project, subject.SubjectInDB, r.StartDate, r.EndDate, subject.Hours });
                        subject.Updated = true;
                    }
                    else
                    {
                        rc = "סטטוס לא תקין, השורה לא עודכנה";
                    }
                }
                catch (Exception ex)
                {
                    rc = "שגיאה בבסיס הנתונים, הנתונים לא נשמרו";
                }

                if (subject.Updated)
                {
                    subject.CurrHours = subject.Hours;
                    subject.CurrStartDate = r.StartDate;
                    subject.CurrEndDate = r.EndDate;
                    subject.IsNew = false;
                    RefreshStatus(rowIndex, subjectIndex);
                }
            }
            return rc;
        }
        public List<LetterData> Results
        {
            get { return _results; }
            set { _results = value; }
        }

        private class ExcelData
        {
            public DateTime CreateDate { get; set; }
            public string CoordinatorName { get; set; }
            public string Subject { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string IdNum { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public Decimal Hours { get; set; }
            public string SocialWorker { get; set; }
        }   
        public void ExportToExcel(string filename)
        {
            List<ExcelData> lst = new List<ExcelData>();
            foreach (var tec in Results)
            {
                foreach (SubjectData sub in tec.Subjects)
                {
                    ExcelData exData = new ExcelData();
                    exData.CreateDate = tec.CreateDate;
                    exData.CoordinatorName = tec.CoordinatorName;
                    exData.Subject = sub.SubjectInDB;
                    exData.FirstName = tec.FirstName;
                    exData.LastName = tec.LastName;
                    exData.IdNum = tec.IdNum;
                    exData.Phone = tec.Phone;
                    exData.Email = tec.Email;
                    exData.StartDate = tec.StartDate;
                    exData.EndDate = tec.EndDate;
                    exData.Hours = sub.Hours;
                    exData.SocialWorker = tec.SocialWorker;
                    lst.Add(exData);
                }
            }
            DataTable dt = lst.ToDataTable();
            var captions = new Dictionary<string, string>();

            ////foreach (DataGridViewColumn col in dataGridView1.Columns)
            ////captions.Add(col.Name, col.HeaderText);

            ////var names =  ddataGridView1.Columns.Cast<DataGridViewColumn>().Select(x => x.Name).ToArray();
            //..var captions = dataGridView1.Columns.Cast<DataGridViewColumn>().Select(x => x.HeaderText).ToArray();

            //string[] names = new string[] {
            //        "Phone",
            //        "SocialWorker" ,
            //        "Project"  ,
            //        "Branch" ,
            //        "FirstName",
            //        "LastName" ,
            //        "CurrPhone" ,
            //        "CurrEmail" ,
            //        "CurrFirstName" ,
            //        "CurrLastName" ,
            //        "CurrBranch" ,
            //        "CurrSocialWorker" ,
            //        "CoordinatorName" ,
            //        "Comments" ,
            //        "IsSelected" ,
            //        "StartDate" ,
            //        "EndDate" ,
            //    "Subjects"};

            captions["CreateDate"] = "תאריך קליטה";
            captions["CoordinatorName"] = "רכז";
            captions["Subject"] = "הנגשה";
            captions["FirstName"] = "שם";
            captions["LastName"] = "שם משפחה";
            captions["IdNum"] = "ת.ז";
            captions["Phone"] = "מס פלאפון";
            captions["Email"] = "מייל";
            captions["StartDate"] = "ת.זכאות";
            captions["EndDate"] = "ס.זכאות";
            captions["Hours"] = "מכסת שעות";
            captions["SocialWorker"] = "עו\"ס";

            ExcelCreator ex = new DrLogy.DrLogyUtils.ExcelCreator();
            ex.DataTableToExcel(dt , 0, filename, "", captions);
            //WriteToLog("הטבלה נשמרה בהצלחה");

        }
    }
}
