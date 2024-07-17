using DrLogy.DrLogyPDFUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrLogy.DrLogyPDFMailerUtils
{

    public enum PDFKeyType
    {
        Number = 1,
        String = 2,
        PDFNumber = 3
    }
    public class MailerOptions
    {
        public MailerOptions() { }

        public List<PDFReplaceOption> EditOptions = new List<PDFReplaceOption>();

        public string IDFieldName;

        public string EmptyKeyValue;
        public RectangleF EmptyRectangle;
        public PDFKeyType EmptyKeyType = PDFKeyType.Number;

        public RectangleF KeyRectangle;
        public RectangleF[] ArchiveKeyRectangles;
        public string[] ArchiveKeyNames;
        public string KeyFieldName;
        public PDFKeyType KeyType = PDFKeyType.Number;

        public string TableName;
        public string EmailFieldName;
        public string NameFieldName;
        public string Filter;

    }
}