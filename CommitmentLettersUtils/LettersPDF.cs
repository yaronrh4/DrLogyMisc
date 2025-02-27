using DrLogy.DrLogyPDFUtils;
using DrLogy.DrLogyUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public bool LoadStudent(string idNum, string[] subjects, DateTime startDate, DateTime endDate, string connectionString, string defaultCoordinatorName)
        {
            PDFUtils utils = new PDFUtils();
            LetterData data = new LetterData();

            data.PageNumber = 1;
            data.FileName = "";
            data.ProjectId = Options.ProjectId;
            data.CoordinatorName = defaultCoordinatorName;
            data.CreateDate = DateTime.Now.Date;
            if (data.CoordinatorName == "")
                data.CoordinatorName = _options.Coordinators[0].Name;

            data.IdNum = idNum;
            data.EndDate = endDate;
            data.StartDate = startDate;

            DrLogy.DrLogyUtils.DbUtils.ConStr = connectionString;

            //בדיקה אם קיים תלמיד בפרוייקט הספציפי בהנגשה הנוכחית עם נתונים אחרים
            DataTable dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut", "prj_id" }, new object[] { idNum, Options.ProjectId });
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
                data.NewKey = row["ST_EMAIL"].ToString();
                data.ClassName = row["ST_CLASSNAME"].ToString();
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
                    newSubject.SubjectId = subject.SubjectId;
                    newSubject.SubjectInFile = subject.NameInFile;
                    //newSubject.SubjectInDB = subject.Name;
                    newSubject.Hours = 1; //dummy value would be updated letter
                    data.Subjects.Add(newSubject);
                }
            }

            _results.Add(data);
            int i = Results.Count() - 1;
            for (int j = 0; j < Results[i].Subjects.Count; j++)
                if (!GetDataFromDB(i, j, connectionString))
                {
                    Results.RemoveAt(i);
                    return false;
                }

            foreach (var subject in data.Subjects)
            {
                subject.Hours = subject.CurrHours;
            }

            for (i = 0; i < Results[Results.Count - 1].Subjects.Count; i++)
            {
                RefreshStatus(Results.Count - 1, i);
            }

            return true;
        }
        private string ValidateRequiredFieldsOnInsert(DataTable dt, string keyField, string[] fields)
        {
            string fldsErr = "";
            string prevId = "";
            for (int i=0; i <dt.Rows.Count; i++ )
            {
                DataRow r = dt.Rows[i];
                string zehut = r[keyField].ToString();
                int exists = 0;
                if (prevId == zehut)
                    exists = 1;
                else
                    exists = (int)DbUtils.ExecSP("SPMISC_ZEHUT_EXISTS", new string[] { "zehut" }, new object[] { zehut });
                prevId = zehut; 
                //new student
                if (exists==0)
                {
                    string s = ValidateRequiredFields(dt, fields, i);
                    if (s != "")
                    {
                        if (fldsErr != "")
                            fldsErr += " , ";
                        fldsErr +=$" שורה {i+1} : {s}";
                    }
                }
            }

            return fldsErr; 
        }

        private string ValidateMinValue(DataTable dt, string field, int minValue , bool includeValue)
        {
            string fldsErr = "";
            List<int> err = new List<int>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                if (dt.Columns.Contains(field) && !(row[field] is DBNull) &&  ((decimal.Parse(row[field].ToString()) < minValue) || (includeValue && decimal.Parse(row[field].ToString()) <= minValue)))
                    err.Add(i+1);
            }

            if (err.Count > 0) {
                fldsErr = field; 
                fldsErr += (err.Count == 1 ? " שורה " : " שורות ") + string.Join(",", err.ToArray());
            }

            return fldsErr;
        }


        private string ValidateRequiredFields(DataTable dt, string[] fields , int rowIndex=-1)
        {
            string fldsErr = "";

            foreach (string fld in fields)
            {
                List<int> emptyLst = new List<int>();
                bool empty = false;

                if (!dt.Columns.Contains(fld))
                    empty = true;
                else
                {
                    if (rowIndex == -1)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][fld] is DBNull)
                                emptyLst.Add(i + 1);
                        }
                    }
                    else if (dt.Rows[rowIndex][fld] is DBNull)
                        empty = true;
                }

                if (empty || emptyLst.Count > 0)
                {
                    if (fldsErr != "")
                        fldsErr += ",";
                    fldsErr += fld;
                    if (emptyLst.Count > 0)
                        fldsErr += (emptyLst.Count == 1 ? " שורה " : " שורות ") + string.Join(",", emptyLst.ToArray());
                }
            }
            return fldsErr;
        }

        private string ValidateExcel(DataTable dt)
        {
            //check required fields
            string[] reqFields = { "ת.ז" , "תאריך התחלה", "תאריך סיום", "הנגשה" };
            string fldsErr = "";

            fldsErr = ValidateRequiredFields(dt, reqFields);

            if (fldsErr != "")
                return $"שדות חובה לא קיימים בקובץ האקסל: {fldsErr}";

            //בדיקת שדות חובה בהוספה
            string[] reqFieldsOnInsert = { "שם פרטי" , "שם משפחה" , "טלפון" , "מייל" };
            fldsErr =  ValidateRequiredFieldsOnInsert (dt, "ת.ז", reqFieldsOnInsert) ;
            if (fldsErr != "")
                return $"שדות חובה בהוספת תלמיד לא קיימים בקובץ האקסל: {fldsErr}";

            if (fldsErr != "")
                return $"שדות חובה לא קיימים בקובץ האקסל: {fldsErr}";

            //field types
            string[] intFields = { "ת.ז" , "מספר פנימי" };
            string[] dateFields = { "תאריך התחלה" , "תאריך סיום" , "תאריך קליטה" };

            fldsErr = ValidateColumnType(dt, intFields, typeof (int) );
            if (fldsErr != "")
            {
                return $"הערכים בשדות הבאים אינם מספרים שלמים  : {fldsErr}";
            }

            fldsErr = ValidateColumnType(dt, dateFields, typeof(DateTime));

            if (fldsErr != "")
            {
                return $"הערכים בשדות הבאים אינם תאריכים  : {fldsErr}";
            }

            string[] decimalFields = { "מכסת שעות" };

            fldsErr = ValidateColumnType(dt, decimalFields, typeof(Decimal));

            if (fldsErr != "")
            {
                return $"הערכים בשדות הבאים אינם מספרים  : {fldsErr}";
            }

            fldsErr = ValidateMinValue(dt, "מכסת שעות", 0 , true);

            if (fldsErr != "")
            {
                return $"הערכים לא תקינים בשדה : {fldsErr}";
            }

            fldsErr = ValidateProject(dt);
            if (fldsErr != "")
            {
                return fldsErr;
            }


            fldsErr = ValidateSubjects(dt);
            if (fldsErr != "")
            {
                return fldsErr;
            }

            return "";
        }

        private string ValidateSubjects(DataTable dt)
        {
            //בדיקת הנגשות
            string prj = "";
            if (dt.Columns.Contains("הנגשה"))
            {
                foreach (DataRow r in dt.Rows)
                {
                    if (!(r["הנגשה"] is DBNull))
                    {
                        prj = r["הנגשה"].ToString();

                        if (_options.Subjects.FirstOrDefault(p => p.NameInFile == prj) == null)
                        {
                            return $"ההנגשה {prj} לא נתמכת בפרוייקט";
                        }
                    }

                }
            }
            
            return "";
        }


        private string ValidateProject (DataTable dt)
        {
            //בדיקת פרוייקט
            string prj = "";
            if (dt.Columns.Contains("פרוייקט"))
            {
                string lastPrj = "";
                foreach (DataRow r in dt.Rows)
                {
                    if (!(r["פרוייקט"] is DBNull))
                    {
                        prj = r["פרוייקט"].ToString();
                        if (prj != lastPrj && lastPrj != "")
                        {
                            return "ניתן לייבא תלמידים רק מפרוייקט אחד בקובץ";
                        }
                        lastPrj = prj;
                    }
                }
            }
            if (prj != "")
            {
                int prjId = (int)DbUtils.ExecSP("SPMISC_GET_PROJECT_ID", new string[] { "project" }, new object[] { prj });
                if (prjId == 0)
                    return $"הפרוייקט {prj} לא נתמך באפליקציה";
                else
                {
                    if (prjId != this.Options.ProjectId)
                    {
                        return $"חובה לבחור את הפרוייקט  {prj} באפליקציה לפני ביצוע יבוא";
                    }
                }
            }

            return "";
        }
        private string ValidateColumnType(DataTable dt , string[] fields , Type dataType)
        {
            string fldsErr = "";
            foreach (string fld in fields)
            {
                if (dt.Columns.Contains(fld))
                {
                    int errors = 0;
                    string rowsError = "";
                    Type typ = dt.Columns[fld].DataType;
                    if (typ != dataType)
                    {
                        int tmpInt;
                        decimal tmpDecimal; 
                        DateTime tmpDateTime;
                        for (int i = 0; i < dt.Rows.Count && errors < 10; i++)
                        {
                            DataRow row = dt.Rows[i];
                            if (!(row[fld] is DBNull) )
                            {
                                bool ok = true;

                                if (dataType == typeof(int))
                                    ok = int.TryParse(row[fld].ToString(), out tmpInt);
                                if (dataType == typeof(decimal))
                                    ok = decimal.TryParse(row[fld].ToString(), out tmpDecimal);
                                if (dataType == typeof(DateTime))
                                    ok = DateTime.TryParse(row[fld].ToString(), out tmpDateTime);

                                if (!ok)
                                {

                                    errors++;
                                    rowsError += (errors > 1) ? "," : "";

                                    if (errors >= 10)
                                        rowsError += "...";
                                    else
                                        rowsError += $"{i + 1}";
                                }
                            }
                        }
                    }
                    if (errors > 0)
                    {
                        if (fldsErr != "")
                            fldsErr += ",";
                        fldsErr += fld;

                        fldsErr += ((errors == 1) ? " שורה " : " שורות ") + rowsError;
                    }
                }
            }

            return fldsErr; 
        }
        public string ImportFromExcel(string filename, string connectionString, string defaultCoordinatorName)
        {
            _results.Clear();

            var excel = new ExcelCreator();

            DataTable dt = excel.ExcelToDatatable(filename , "" , true);

            string err = ValidateExcel(dt);
            if (err != "")
                return err;

            var sorted = dt.AsEnumerable().OrderBy(x => x["ת.ז"]);
            DataTable badDt = dt.Clone();

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
                    if (dt.Columns.Contains ("פרוייקט") &&  !(row["פרוייקט"] is DBNull))
                        data.ProjectId = (int)DbUtils.ExecSP("SPMISC_GET_PROJECT_ID", new string[] { "project" }, new object[] { row["פרוייקט"] });
                    else
                        data.ProjectId = _options.ProjectId;

                    data.CreateDate = dt.Columns.Contains("תאריך קליטה") && row["תאריך קליטה"] is DBNull ? DateTime.Now.Date : Convert.ToDateTime( row["תאריך קליטה"]).Date; ;

                    data.StartDate = Convert.ToDateTime(row["תאריך התחלה"]);
                    data.EndDate = Convert.ToDateTime(row["תאריך סיום"]);

                    data.Email = data.CurrEmail = dt.Columns.Contains("מייל") && row["מייל"] is DBNull ? "" : (string)row["מייל"];
                    data.CoordinatorName = data.CurrCoordinatorName = dt.Columns.Contains("תלמיד") && row["רכז תלמיד"] is DBNull ? "" : (string)row["רכז תלמיד"].ToString();
                    data.FirstName = data.CurrFirstName = dt.Columns.Contains("שם פרטי") && row["שם פרטי"] is DBNull ? "" : row["שם פרטי"].ToString();
                    data.LastName = data.CurrLastName = dt.Columns.Contains("שם משפחה") && row["שם משפחה"] is DBNull ? "" : row["שם משפחה"].ToString();
                    data.SocialWorker = data.CurrSocialWorker = dt.Columns.Contains("עו\"ס") && row["עו\"ס"] is DBNull ? "" : row["עו\"ס"].ToString();
                    data.Branch = data.CurrBranch = dt.Columns.Contains("עיר") && row["עיר"] is DBNull ? "" : row["עיר"].ToString();
                    data.Phone = data.CurrPhone = dt.Columns.Contains("טלפון") && row["טלפון"]  is DBNull ? "" : row["טלפון"].ToString();

                    data.NewKey = data.CurrNewKey = dt.Columns.Contains("מספר פנימי") && row["מספר פנימי"] is DBNull ? "" : row["מספר פנימי"].ToString();
                    data.ClassName = data.CurrClassName = dt.Columns.Contains("כיתה") && row["כיתה"] is DBNull ? "" : row["כיתה"].ToString();
                    data.Age = data.CurrAge = dt.Columns.Contains("גיל") && row["גיל"] is DBNull ? "" : row["גיל"].ToString();
                    data.Address = data.CurrAddress = dt.Columns.Contains("כתובת") && row["כתובת"] is DBNull ? "" : row["כתובת"].ToString();
                    data.Mikud = data.CurrMikud = dt.Columns.Contains("מיקוד") && row["מיקוד"] is DBNull ? "" : row["מיקוד"].ToString();
                    data.Comments = dt.Columns.Contains("הערות") && row["הערות"] is DBNull ? "" : row["הערות"].ToString();
                    if (!string.IsNullOrEmpty (data.SocialWorker) && data.SocialWorker.IndexOf("עוס") < 0 && data.SocialWorker.IndexOf("עו\"ס") < 0)
                        data.SocialWorker = "עו\"ס " + data.SocialWorker;
                    data.CurrSocialWorker = data.SocialWorker;

                    data.Subjects = new List<SubjectData>();
                    _results.Add(data);
                }
                SubjectData newSubject = new SubjectData();
                string subName = (string)row["הנגשה"];

                var sub = _options.Subjects.FirstOrDefault(c => c.NameInFile == subName);
                if (sub != null)
                {
                    newSubject.SubjectId = sub.SubjectId;
                    newSubject.SubjectInDB = sub.NameInDB;
                    newSubject.SubjectInFile = sub.NameInFile;
                }
                else
                {
                    newSubject.SubjectId = (int)DbUtils.ExecSP("SPMISC_GET_SUBJECT_ID", new string[] { "subject" }, new object[] { subName });
                    newSubject.SubjectInDB = newSubject.SubjectInFile = subName;
                }
                if (row.Table.Columns.Contains("מכסת שעות") && !string.IsNullOrEmpty(row["מכסת שעות"].ToString()))
                    newSubject.Hours = decimal.Parse(row["מכסת שעות"].ToString());
                else
                    newSubject.Hours = null;
                if (row.Table.Columns.Contains ("תלמיד חדש למייל") && row["תלמיד חדש למייל"].ToString() == "כן")
                    data.IsNewStudent = true;
                else if (row["תלמיד חדש למייל"].ToString() == "לא")
                    data.IsNewStudent = false;

                data.Subjects.Add(newSubject);
            }

            for (int i = 0; i < Results.Count; i++)
            {
                for (int j = 0; j < Results[i].Subjects.Count; j++)
                {
                    GetDataFromDB(i, j, connectionString);

                }
                //שינוי סטטוס לחדש אם סטטוס הוא ברירת מחדל והתלמיד חדש
                if  (Results[i].IsNewStudent == null  && Results[i].Subjects[0].Status == StudentStatus.NoStudent)
                    Results[i].IsNewStudent = true;
                else
                    if (Results[i].IsNewStudent ==null)
                        Results[i].IsNewStudent = false;
            }

            return "";
        }
        public void Process(string filename, string connectionString, string defaultCoordinatorName)
        {
            RemoveOld(filename);

            PDFUtils utils = new PDFUtils();

            LetterData data = new LetterData();

            data.PageNumber = 1;
            data.FileName = filename;
            data.ProjectId = Options.ProjectId;
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
                {
                    GetDataFromDB(i, j, connectionString);

                    //עדכון עו"ס וסניף מתוך הקובץ
                    if (!string.IsNullOrEmpty(Results[i].SocialWorker) && Results[i].CurrSocialWorker != Results[i].SocialWorker)
                    {
                        Results[i].CurrSocialWorker = Results[i].SocialWorker;
                        Results[i].Edited = true;
                    }
                    if (!string.IsNullOrEmpty(Results[i].Branch) && Results[i].CurrBranch != Results[i].Branch)
                    {
                        Results[i].CurrBranch = Results[i].Branch;
                        Results[i].Edited = true;

                    }
                }
        }
        public string GetDbSubject(string fileSubject)
        {
            string subject = "";
            if (!string.IsNullOrEmpty(fileSubject))
            {
                var item = _options.Subjects.Where(t => t.NameInFile == fileSubject).FirstOrDefault();

                if (item != null)
                {
                    subject = item.NameInDB;
                }
            }

            return subject;
        }

        public bool GetDataFromDB(int index, int subjectIndex, string connectionString, bool overrideDetails = false)
        {
            bool rc = false;
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
            if (subject.SubjectId > 0 && r.StartDate.HasValue && r.EndDate.HasValue /*&& subject.Hours > 0*/)
            {
                rc = true;
                dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut", "prj_id", "sub_id", "hours", "start_date", "end_date" }, new object[] { r.IdNum, r.ProjectId, subject.SubjectId, subject.Hours, r.StartDate.Value, r.EndDate.Value });

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[dt.Rows.Count - 1];

                    subject.CurrStartDate = (DateTime)row["ST_SIGNDATE"];
                    subject.CurrEndDate = (DateTime)row["ST_DATEEND"];
                    if (!(row["ST_MAX_HOURS"] is DBNull))
                        subject.CurrHours = decimal.Parse(row["ST_MAX_HOURS"].ToString());
                    else
                        subject.CurrHours = null;
                }
            
                
                if (dt != null && dt.Rows.Count == 0 && subject.SubjectId > 0 )
                {
                    //בדיקה אם קיים תלמיד בפרוייקט הספציפי בהנגשה הנוכחית עם נתונים אחרים
                    dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut", "prj_id", "sub_id" }, new object[] { r.IdNum, r.ProjectId, subject.SubjectId });

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[dt.Rows.Count - 1];

                        if (!(row["ST_SIGNDATE"] is DBNull))
                            subject.CurrStartDate = (DateTime)row["ST_SIGNDATE"];

                        if (!(row["ST_DATEEND"] is DBNull))
                            subject.CurrEndDate = (DateTime)row["ST_DATEEND"];

                        if (!(row["ST_MAX_HOURS"] is DBNull))
                            subject.CurrHours = (decimal)row["ST_MAX_HOURS"];

                    }
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    //בדיקה אם קיים תלמיד עם ת.ז  הנוכחית - בפרוייקט אחר / הנגשה אחרת
                    dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_USER", new string[] { "zehut" }, new object[] { r.IdNum });
                    if (dt.Rows.Count > 0 )
                        subject.Exists = false;

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
                else
                {
                    rc = false;
                    r.CurrFirstName = r.FirstName;
                    r.CurrLastName = r.LastName;
                    r.CurrBranch = r.Branch;
                    r.CurrEmail = r.Email;
                    r.CurrPhone = r.Phone; 
                    r.CurrBranch = r.Branch;
                    r.CurrSocialWorker = r.SocialWorker;
                    r.CurrNewKey = r.NewKey;
                    r.CurrClassName = r.ClassName;
                    r.CurrAge = r.Age;
                    r.CurrAddress = r.Address;
                    r.CurrMikud = r.Mikud;

                    r.IsNewStudent = true;
                }

                RefreshStatus(index, subjectIndex);
            }
            return rc;
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
                    newSubject.SubjectInFile = subject.NameInFile;
                    newSubject.SubjectInDB = subject.NameInDB;
                    newSubject.Hours = hours;
                    newSubject.SubjectId = (int)DbUtils.ExecSP("SPMISC_GET_SUBJECT_ID", new string[] { "subject" }, new object[] { newSubject.SubjectInDB });

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

                if (subject.Exists)
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
                    var z = DbUtils.ExecSP("SPMISC_UPDATE_STUDENT", new string[] { "st_id", "zehut", "fname", "lname", "prj_id", "phone", "city", "parentname", "email" ,"rakaz" , "newkey", "classname", "age", "address", "mikud" }, new object[] { r.Id, r.IdNum.Trim(), r.CurrFirstName.Trim(), r.CurrLastName.Trim(), r.ProjectId, r.CurrPhone.Trim(), r.CurrBranch.Trim(), r.CurrSocialWorker.Trim() , r.CurrEmail , rakazId  , r.CurrNewKey, r.CurrClassName, r.CurrAge , r.CurrAddress,r.CurrMikud }, true);
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
                    r.Edited = false;
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
                        DbUtils.ExecSP("SPMISC_UPDATE_SUBJECT", new string[] { "st_id", "st_zehut", "prj_id", "sub_id", "rakaz", "start_date", "end_date", "hours" , "parentname" , "city"}, new object[] { _results[rowIndex].Id, _results[rowIndex].IdNum ,  _results[rowIndex].ProjectId, subject.SubjectId, rakazId, r.StartDate, r.EndDate, subject.Hours , r.CurrSocialWorker  , r.CurrBranch}, true);
                        subject.Updated = true;
                    }
                    else if (subject.Status == StudentStatus.NoSubject || subject.Status == StudentStatus.NoStudent)
                    {
                        DbUtils.ExecSP("SPMISC_INSERT_SUBJECT", new string[] { "st_id", "prj_id", "sub_id", "rakaz", "start_date", "end_date", "hours" , "parentname" }, new object[] { _results[rowIndex].Id, r.ProjectId, subject.SubjectId, rakazId, r.StartDate, r.EndDate, subject.Hours ,r.CurrSocialWorker} ,true);
                        subject.Updated = true;
                        subject.Exists = true;
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
            public Decimal? Hours { get; set; }
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
                    //exData.Subject = sub.SubjectInDB;
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
