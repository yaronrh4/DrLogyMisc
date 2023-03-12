using DrLogy.DrLogyPDFMailerUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitmentLetters
{
    public class LetterData : BasePDFProperties    {
        public LetterData() { }
        public string Phone { get; set; }
        public string SocialWorker { get; set; }

        public string Branch { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string NameInPDF { get; set; }

        public string SubjectName { get; set; }
        public decimal SubjectHours { get; set; }
    }
}
