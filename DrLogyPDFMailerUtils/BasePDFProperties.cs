using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrLogy.DrLogyPDFMailerUtils
{
    [Serializable]
    public class BasePDFProperties
    {

        public int PageNumber { get; set; }
        public string FileName { get; set; }
        public bool Found { get; set; }
        public bool IsEmpty { get; set; }
        public string Name { get; set; }
        public string IdNum { get; set; }
        public string Email { get; set; }
        public DateTime? SentDate { get; set; }
        public bool MailError { get; set; }
        public int Id { get; set; }

        public string[] ArchiveKeyNames { get; set; }
        public string[] ArchiveKeyValues { get; set; }

    }
}
