using DrLogy.DrLogyPDFMailerUtils;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CommitmentLetters
{
    public class LettersPDF
    {
        private List<LetterData> _results;
        public void Process(string filename, string connectionString, LettersPDFOptions options)
        {
            if (_results == null)
                _results = new List<LetterData>();

            PDFUtils utils = new PDFUtils();
            LetterData data = new LetterData();
            utils.OpenPdf(filename);

            _results.Add(data);

            List<RectangleF> rec = utils.SearchPage(options.Title);
            RectangleF newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
            string s = utils.ExtractText(newRec, 1);
            int i = s.LastIndexOf("-");
            s = s.Substring(0, i);
            i = s.LastIndexOf(".");

            data.IdNum = s.Substring(0, i - 4).Trim();
            data.NameInPDF = Utils.RotateString(s.Substring(i + 6).Trim());

            rec = utils.SearchPage(options.Dates);
            newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
            s = utils.ExtractText(newRec, 1);
            i = s.IndexOf(":ר‎ו‎ב‎ע‎");
            data.EndDate = DateTime.ParseExact(s.Substring(i + 10, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            data.StartDate = DateTime.ParseExact(s.Substring(i + 29, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            rec = utils.SearchPage(options.SocialWorker);
            newRec = new RectangleF(0, rec[0].Y - 14, rec[0].X + 100, rec[0].Height);
            s = utils.ExtractText(newRec, 1).Trim();
            data.SocialWorker = Utils.RotateString(s);

            rec = utils.SearchPage(options.Branch);
            newRec = new RectangleF(0, rec[rec.Count() - 1].Y, rec[rec.Count() - 1].X, rec[rec.Count() - 1].Height);
            s = utils.ExtractText(newRec, 1).Trim();
            data.Branch = Utils.RotateString (s);

            rec = utils.SearchPage(options.Phone);
            newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
            s = utils.ExtractText(newRec, 1).Trim();
            data.Phone = s;
            GetSubjectsFromPDF(options.Subjects, utils);

            GetDataFromDB(connectionString, data);

        }
        private void GetDataFromDB(string connectionString, LetterData data)
        {
            //sql
            string sql = $"SELECT ST_EMAIL, ST_FNAME + ' ' + ST_LNAME ST_NAME FROM TBL_STUDENTS WHERE TRY_CAST( ST_ZEHUT as float) = {data.IdNum}";

            DrLogy.DrLogyUtils.DbUtils.ConStr = connectionString;
            DataTable dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData(sql, null, null, CommandType.Text);

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[dt.Rows.Count - 1];
                data.Name = (string)r["ST_NAME"];
                data.Email = (string)r["ST_EMAIL"];
            }

        }

        private void GetSubjectsFromPDF( List<Subject> subjects, PDFUtils utils)
        {
            List<RectangleF> rec;
            RectangleF newRec;
            string s;
            int i;
            int groupedCount = 0;
            decimal hours;
            bool first = true;
            LetterData data = _results[_results.Count - 1];
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
                        newRec = new RectangleF(0, rec[0].Y, rec[0].X, rec[0].Height);
                        s = utils.ExtractText(newRec, 1).Trim();
                        i = s.LastIndexOf("ת‎ו‎ע‎ש‎");
                        if (i == -1)
                        {
                            i = s.IndexOf(" ");
                            s = s.Substring(i + 1);
                        }
                        else
                        {
                            s = s.Substring(i + 9);
                        }

                        i = s.IndexOf(" ");
                        s = s.Substring(0, i);

                        hours = decimal.Parse(s);
                    }
                    if (!first)
                    {
                        data = DrLogy.DrLogyUtils.Utils.Clone (data);
                        _results.Add(data);
                    }
                    data.SubjectName = subject.Name;
                    data.SubjectHours = subject.Grouped && groupedCount > 1 ? hours / 2 : hours;
                }
            }
        }

        public List<LetterData> Results
        {
            get { return _results; }
        }
    }
}
