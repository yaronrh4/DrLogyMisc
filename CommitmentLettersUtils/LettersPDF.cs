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

        public void LoadStudent(string idNum, string[] subjects, DateTime startDate , DateTime endDate, string connectionString, string project , string defaultCoordinatorName)
        {
            PDFUtils utils = new PDFUtils();
            LetterData data = new LetterData();

            data.PageNumber = 1;
            data.FileName = "";
            data.Project = project;
            data.CoordinatorName = defaultCoordinatorName;
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
                data.FirstName = row["ST_FNAME"].ToString();
                data.LastName = row["ST_LNAME"].ToString();
                data.Name = data.FirstName + " " + data.LastName;
                data.SocialWorker = row["ST_PARENTNAME"].ToString();
                data.Branch = row["ST_CITY"].ToString();
                data.Phone = row["ST_PHONE1"].ToString();
                data.Email = row["ST_EMAIL"].ToString();
                data.NewKey =   row["ST_EMAIL"].ToString();
                data.ClassName = row["CLS_NAME"].ToString();
                data.Age = row["ST_AGE"].ToString();
                data.Address = row["ST_ADDRESS"].ToString();
                data.Mikud = row["ST_MIKUD"].ToString();
            }

            //Subjects
            SubjectData newSubject = null;
            data.Subjects = new List<SubjectData>();

            foreach (var selectedsubject in subjects)
            {
                var subject = this._options.Subjects.Find(z => z.NameInFile == selectedsubject);

                if (subject != null)
                {
                    newSubject = new SubjectData();
                    newSubject.SubjectFile = subject.NameInFile;
                    newSubject.SubjectInDB = subject.Name;
                    newSubject.Hours = 1; //dummy value would be updated letter
                    newSubject.IsNew = false; //default value
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

        public string ImportFromExcel(string filename, string connectionString, string project, string defaultCoordinatorName)
        {
            _results.Clear();

            var excel = new ExcelCreator();

            DataTable dt = excel.ExcelToDatatable(filename);

            var sorted = dt.AsEnumerable().OrderBy(x => x["ת.ז"]);
            DataTable badDt = dt.Clone();
            //todo validation

            int idNum = -1;
            LetterData data = null;
            foreach (var row in sorted)
            {
                int currIdNum = int.Parse (row["ת.ז"].ToString());

                if (data == null || idNum != currIdNum)
                {
                    idNum = currIdNum;
                    data = new LetterData();
                    data.IdNum = currIdNum.ToString();

                    data.FileName = filename;
                    data.Project = project;
                    data.CreateDate = row["תאריך קליטה"] is DBNull ? DateTime.Now : (DateTime)row["תאריך קליטה"]; ;

                    data.StartDate = (DateTime)row["תאריך התחלה"];
                    data.EndDate = (DateTime)row["תאריך סיום"];

                    data.Email = data.CurrEmail = row["מייל"] is DBNull ? "" : (string)row["מייל"];
                    data.CoordinatorName = data.CurrCoordinatorName = row["רכז תלמיד"] is DBNull ? "" : (string)row["רכז תלמיד"].ToString();
                    data.FirstName = data.CurrFirstName = row["שם פרטי"] is DBNull ? "" : row["שם פרטי"].ToString();
                    data.LastName = data.CurrLastName = row["שם משפחה"] is DBNull ? "" : row["שם משפחה"].ToString();
                    data.SocialWorker = data.CurrSocialWorker = row["עו\"ס"] is DBNull ? "" : row["עו\"ס"].ToString();
                    data.Branch = data.CurrBranch = row["עיר"] is DBNull ? "" : row["עיר"].ToString();
                    data.Phone = data.CurrPhone = row["טלפון"] is DBNull ? "" : row["טלפון"].ToString();

                    data.NewKey = data.CurrNewKey = row["מספר פנימי"] is DBNull ? "" : row["מספר פנימי"].ToString();
                    data.ClassName = data.CurrClassName = row["כיתה"] is DBNull ? "" : row["כיתה"].ToString();
                    data.Age = data.CurrAge = row["גיל"] is DBNull ? "" : row["גיל"].ToString();
                    data.Address = data.CurrAddress = row["כתובת"] is DBNull ? "" : row["כתובת"].ToString();
                    data.Mikud = data.CurrMikud = row["מיקוד"] is DBNull ? "" : row["מיקוד"].ToString();

                    if (!string.IsNullOrEmpty (data.SocialWorker) && data.SocialWorker.IndexOf("עוס") < 0 && data.SocialWorker.IndexOf("עו\"ס") < 0)
                        data.SocialWorker = "עו\"ס " + data.SocialWorker;
                    data.CurrSocialWorker = data.SocialWorker;

                    data.Subjects = new List<SubjectData>();
                    _results.Add(data);
                }
                SubjectData newSubject = new SubjectData();
                string subName = (string)row["הנגשה"];

                var sub = _options.Subjects.FirstOrDefault(c => c.NameInFile == subName);

                //newSubject.SubjectFile = subject.NameInFile;
                newSubject.SubjectInDB = (string)row["הנגשה"];
                newSubject.Hours = row["מכסת שעות"] is DBNull ? 0 : decimal.Parse(row["מכסת שעות"].ToString());
                newSubject.SubjectFile = subName;
                newSubject.SubjectInDB = sub == null ? subName : sub.Name;
                newSubject.IsNew = null; //ברירת מחדל
                if (row.Table.Columns.Contains ("תלמיד חדש") && row["תלמיד חדש"].ToString() == "כן")
                    newSubject.IsNew = true;
                else if (row["תלמיד חדש"].ToString() == "לא")
                    newSubject.IsNew = false;

                data.Subjects.Add(newSubject);
            }

            for (int i = 0; i < Results.Count; i++)
            {
                for (int j = 0; j < Results[i].Subjects.Count; j++)
                {
                    GetDataFromDB(i, j, connectionString);
                    RefreshStatus(i,j);
                    //שינוי סטטוס לחדש אם סטטוס הוא ברירת מחדל והתלמיד חדש
                    if (!Results[i].Subjects[j].IsNew.HasValue)
                        Results[i].Subjects[j].IsNew = (Results[i].Subjects[j].Status == StudentStatus.NoStudent);

                }

            }

            return "";
        }
        public void Process(string filename, string connectionString, string project , string defaultCoordinatorName)
        {
            RemoveOld(filename);

            PDFUtils utils = new PDFUtils();

            LetterData data = new LetterData();

            data.PageNumber = 1;
            data.FileName = filename;
            data.Project = project;
            data.CoordinatorName = defaultCoordinatorName;
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
        public string GetDbSubject(string fileSubject)
        {
            string subject = "";
            if (!string.IsNullOrEmpty(fileSubject))
            {
                var item = _options.Subjects.Where(t => t.NameInFile == fileSubject).FirstOrDefault();

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
            if (subject.SubjectFile != null && subject.SubjectFile != "" && r.StartDate.HasValue && r.EndDate.HasValue /*&& subject.Hours > 0*/)
            {
                dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut", "project", "subject", "hours", "start_date", "end_date" }, new object[] { r.IdNum, r.Project, subject.SubjectInDB, subject.Hours, r.StartDate.Value, r.EndDate.Value });

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[dt.Rows.Count - 1];
                    subject.CurrStartDate = (DateTime)row["ST_SIGNDATE"];
                    subject.CurrEndDate = (DateTime)row["ST_DATEEND"];
                    subject.CurrHours = (decimal)row["ST_MAX_HOURS"];
                    subject.CurrSubjectFile = subject.SubjectFile;
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
                        subject.CurrSubjectFile = subject.SubjectFile;

                    }
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    //בדיקה אם קיים תלמיד עם ת.ז  הנוכחית - בפרוייקט אחר / הנגשה אחרת
                    dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut" }, new object[] { r.IdNum });
                    if (subject.IsNew.HasValue)
                        subject.IsNew = true;
                }

                //עדכון כתובת ושם התלמיד מבסיס הנתונים - במידה וקיים
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[dt.Rows.Count - 1];
                    r.Id = (int)row["ST_ID"];
                    r.IsNewStudent = false;
                    r.CurrFirstName = row["ST_FNAME"].ToString();
                    r.CurrLastName = row["ST_LNAME"].ToString();
                    r.CurrBranch = row["ST_CITY"].ToString();
                    r.CurrEmail = row["ST_EMAIL"].ToString();
                    r.CurrPhone = row["ST_PHONE1"].ToString();
                    r.CurrBranch = row["ST_CITY"].ToString();
                    r.CurrSocialWorker = row["ST_PARENTNAME"].ToString();
                    r.CurrNewKey = row["ST_NEW_KEY"].ToString();
                    r.CurrClassName = row["ST_CLASSNAME"].ToString();
                    r.CurrAge = row["ST_AGE"].ToString();
                    r.CurrAddress = row["ST_ADDRESS"].ToString();
                    r.CurrMikud = row["ST_MIKUD"].ToString();

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
                        subject.SubjectFile = subject.CurrSubjectFile;
                        r.Branch = r.CurrBranch;
                        r.Email = r.CurrEmail;
                        r.Phone = r.CurrPhone;
                        r.NewKey = r.CurrNewKey;
                        r.ClassName = r.CurrClassName;
                        r.Age = r.CurrAddress;
                        r.Address = r.CurrAddress;
                        r.Mikud = r.CurrMikud;
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
                rec = utils.SearchPage(subject.NameInFile);
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
                    newSubject.SubjectFile = subject.NameInFile;
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

                if (!subject.IsNew.GetValueOrDefault())
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


                r.CurrNewKey = r.NewKey;
                r.CurrClassName = r.ClassName;
                r.CurrAge = r.Address;
                r.CurrAddress = r.Address;
                r.CurrMikud = r.Mikud;
            }

            float idnum = 0;
            if (float.TryParse(r.IdNum, out idnum) && idnum > 0)
            {
                try
                {
                    int rakazId = 0;
                    try
                    {
                        rakazId = _options.Coordinators.First(x => x.Name == r.CoordinatorName).TeacherId;
                    }
                    catch (Exception ex)
                    {
                        rc = "שגיאה באיתור שם רכז";
                        return rc;
                    }
                    DateTime dt1 = DateTime.Now;
                    var z = DbUtils.ExecSP("SPMISC_UPDATE_STUDENT", new string[] { "st_id", "zehut", "fname", "lname", "project", "phone", "city", "parentname", "email" ,"rakaz" , "newkey", "classname", "age", "address", "mikud" }, new object[] { r.Id, r.IdNum.Trim(), r.CurrFirstName.Trim(), r.CurrLastName.Trim(), r.Project.Trim(), r.CurrPhone.Trim(), r.CurrBranch.Trim(), r.CurrSocialWorker.Trim() , r.CurrEmail , rakazId  , r.CurrNewKey, r.CurrClassName, r.CurrAge , r.CurrAddress,r.CurrMikud }, true);
                    double xxx = (dt1-DateTime.Now).TotalMilliseconds;
                    r.NewKey= r.CurrNewKey;
                    r.ClassName = r.CurrClassName;
                    r.Age = r.CurrAge;
                    r.Address = r.CurrAddress;
                    r.Mikud = r.CurrMikud;
                    
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
                    int rakazId = _options.Coordinators.First(x => x.Name == r.CoordinatorName).TeacherId;

                    if (subject.Status == StudentStatus.NotUpdated)
                    {
                        DbUtils.ExecSP("SPMISC_UPDATE_SUBJECT", new string[] { "st_id", "st_zehut", "project", "subject", "rakaz", "start_date", "end_date", "hours" }, new object[] { _results[rowIndex].Id, _results[rowIndex].IdNum ,  _results[rowIndex].Project, subject.SubjectInDB, rakazId, r.StartDate, r.EndDate, subject.Hours }, true);
                        subject.Updated = true;
                    }
                    else if (subject.Status == StudentStatus.NoSubject || subject.Status == StudentStatus.NoStudent)
                    {
                        DbUtils.ExecSP("SPMISC_INSERT_SUBJECT", new string[] { "st_id", "project", "subject", "rakaz", "start_date", "end_date", "hours" }, new object[] { _results[rowIndex].Id, r.Project, subject.SubjectInDB, rakazId, r.StartDate, r.EndDate, subject.Hours } ,true);
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
                    exData.FirstName = tec.FirstName ?? tec.CurrFirstName;
                    exData.LastName = tec.LastName ?? tec.CurrLastName;
                    exData.IdNum = tec.IdNum;
                    exData.Phone = tec.Phone ?? tec.CurrPhone;
                    exData.Email = tec.Email ?? tec.CurrEmail;
                    exData.StartDate = tec.StartDate;
                    exData.EndDate = tec.EndDate;
                    exData.Hours = sub.Hours;
                    exData.SocialWorker = tec.SocialWorker ?? tec.CurrSocialWorker;
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
