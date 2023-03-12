using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrLogyMergePDF
{
    public class FilenameInfo
    {
        public string BeforeText;
        public string AfterText;
        public string ClearedText;

        public static string TxtStart;
        public static string TxtEnd;

        public bool IsValid;
        public static bool IsInvoice(string fileName)
        {
            return fileName.EndsWith("1");
        }
        public FilenameInfo (string fileName )
        {
            int i1 = fileName.IndexOf(" " + TxtStart + " ");
            int i2 = fileName.IndexOf(" " + TxtEnd + " ") ;

            if (i1 > 0 && i2 > 0)
            {
                IsValid = true;
                BeforeText = fileName.Substring(0, i1);
                AfterText = fileName.Substring(i2, fileName.Length - i2 - (IsInvoice(fileName) ? 1 : 0)).Trim();
                ClearedText = BeforeText + " " + AfterText;
            }
            else
                IsValid = false;
        }
    }
}
