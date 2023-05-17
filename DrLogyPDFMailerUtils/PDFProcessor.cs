using DrLogy.DrLogyPDFUtils;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DrLogy.DrLogyPDFMailerUtils
{
    public class PDFProcessor
    {

        List<BasePDFProperties> _results;
        public delegate void LogMessageHandler(string message, int pageNumber);  // delegate
        public delegate void PagesCountChangedHandler(int pageNumber);  // delegate
        public event LogMessageHandler LogMessageSent; // event
        public event PagesCountChangedHandler PagesCountChanged; // event

        protected PDFUtils _utils;

        private bool _cancel;

        public bool Cancel
        {
            get
            {
                return _cancel;
            }
            set
            {
                _cancel = value;
            }
        }

        public List<BasePDFProperties> Results
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;
            }
        }

        private void WriteToLog(string message, int pageNumber = 0)
        {
            LogMessageSent?.Invoke(message, pageNumber);
        }

        protected virtual BasePDFProperties CreatePDFProperties()
        {
            return new BasePDFProperties();
        }

        protected virtual BasePDFProperties ProcessPDFPage(MailerOptions options,string connectionString,string parameter, int pageNumber)
        {
            BasePDFProperties prop = CreatePDFProperties();

            foreach (var option in options.EditOptions)
            {
                _utils.Replace(option, pageNumber);
                System.Windows.Forms.Application.DoEvents();
            }
            prop.PageNumber = pageNumber;

            //get id num
            string idNum = _utils.ExtractText(options.KeyRectangle, pageNumber);
            System.Windows.Forms.Application.DoEvents();
            prop.IdNum = CleanValue(idNum, true, options.KeyType == PDFKeyType.PDFNumber , true);

            prop.FileName = _utils.Filename;
            prop.IsEmpty = false;

            if (prop.IdNum != "")
            {
                DataTable dt = GetInfoFromDB(options, connectionString, prop.IdNum);
                System.Windows.Forms.Application.DoEvents();
                if (dt.Rows.Count == 0)
                {
                    prop.Found = false;
                }
                else
                {
                    prop.Found = true;
                    prop.Id = (int)dt.Rows[0][0];
                    prop.Email = dt.Rows[0][1].ToString();
                    prop.Name = dt.Rows[0][2].ToString();
                }
            }
            else
                prop.Found = false;


            //check if PDF is Empty
            if (options.EmptyRectangle != null && !options.EmptyRectangle.IsEmpty)
            {
                string tmp = CleanValue(_utils.ExtractText(options.EmptyRectangle, pageNumber), true, options.EmptyKeyType == PDFKeyType.PDFNumber);
                System.Windows.Forms.Application.DoEvents();

                if (options.EmptyKeyType == PDFKeyType.Number || options.EmptyKeyType == PDFKeyType.PDFNumber)
                {
                    if (float.TryParse(tmp, out float floatValue) && float.TryParse(options.EmptyKeyValue, out float floatKey) && floatValue == floatKey)
                    {
                        prop.IsEmpty = true;
                    }
                }
                else
                    prop.IsEmpty = (tmp == options.EmptyKeyValue);
            }
            else
                prop.IsEmpty = false;

            return prop;
        }

        private void SetSize()
        {
        }

        public void ProcessPDF(MailerOptions options, string sourceFile, string destinationFolder, string connectionString, string parameter, int fromPage, int toPage)
        {
            _results = new List<BasePDFProperties>();
            _utils = new PDFUtils();

            _utils.OpenPdf(sourceFile, destinationFolder);

            if (fromPage <= 0)
                fromPage = 1;
            if (toPage <= 0 || toPage > _utils.Pages)
                toPage = _utils.Pages;

            if (toPage < fromPage)
                toPage = fromPage;

            PagesCountChanged?.Invoke(toPage - fromPage + 1);

            for (int i = fromPage; i <= toPage && (!_cancel); i++)
            {
                WriteToLog($"מעבד עמוד {i}... " , i-fromPage +1);
                BasePDFProperties prop = ProcessPDFPage(options,connectionString, parameter,i);

                if (prop != null)
                _results.Add(prop);
                WriteToLog($"עיבוד עמוד {i} הסתיים. ", i - fromPage + 1);

                System.Windows.Forms.Application.DoEvents();
            }
            _utils.ClosePdf();
            RenameFilesToIdNum();
            if (Cancel)
            {
                WriteToLog("הפעולה בוטלה");
            }
        }

        private string CleanValue(string val, bool cleanSpaces = true, bool IsPdfNumber = false , bool removeNonNumbers = false)
        {
            val = val.Replace("\r", "");
            val = val.Replace("\n", "");
            if (cleanSpaces)
                val = val.Replace(" ", "");
            
            if (IsPdfNumber)
            {
                char[] chars = val.ToCharArray();

                for (int j = 0; j < chars.Length; j++)
                {

                    chars[j] = chars[j] != '/' ? (char)((int)chars[j] + 1) : '0';
                }
                val = new string(chars);
            }
            if (removeNonNumbers)
            {
                string val2 = val;
                val = "";
                foreach (char c in val2)
                    if (c >= '0' && c <= '9')
                        val += c;
            }
            return val;
        }
        private DataTable GetInfoFromDB(MailerOptions options, string connectionString, string keyValue)
        {
            string sql = $"SELECT {options.IDFieldName} , {options.EmailFieldName} , {options.NameFieldName} FROM " + options.TableName ;
            string where = "";
            if (options.KeyType == PDFKeyType.Number || options.KeyType == PDFKeyType.PDFNumber)
                where = $"TRY_CAST ( {options.KeyFieldName} as float) = {keyValue} ";
            else
                where = $"{options.KeyFieldName} ='{keyValue}'";

            if (!string.IsNullOrWhiteSpace(options.Filter))
            {
                where += $" AND {options.Filter}";
            }

            DrLogy.DrLogyUtils.DbUtils.ConStr = connectionString;
            sql += " WHERE " + where;
            DataTable dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData(sql, null, null, CommandType.Text);

            return dt;
        }

        private void RenameFilesToIdNum()
        {
            foreach (var p in _results)
            {
                if (p.Found)
                {
                    string newFilename = $"{System.IO.Path.GetDirectoryName(p.FileName)}\\{p.IdNum}.pdf";
                    if (File.Exists(newFilename))
                        File.Delete(newFilename);

                    File.Move(p.FileName, newFilename);
                    p.FileName = newFilename;
                }
            }

        }
        public void DeleteEmpty()
        {
            foreach (var p in _results)
            {
                if (p.IsEmpty && File.Exists(p.FileName))
                {
                    try
                    {
                        WriteToLog($"מוחק {p.FileName}...");
                        File.Delete(p.FileName);
                        WriteToLog($"המחיקה הצליחה");
                    }
                    catch
                    {
                        WriteToLog($"המחיקה נכשלה");
                    }
                }
            }
        }
    }
}
