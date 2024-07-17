using DrLogy.DrLogyPDFMailerUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
namespace DrLogy.CommitmentLettersUtils
{
    public enum StudentStatus
    {
        [Description("הנגשה מעודכנת")] 
        Updated =0,
        [Description("הנגשה לא קיימת")]
        NoSubject =1,
        [Description("הנגשה לא מעודכנת")]
        NotUpdated = 2,
        [Description("תלמיד לא זוהה")]
        NoStudent = 3,
    }
    [Serializable]
    public class SubjectData     {
        public int SubjectId { get; set; }
        public bool Exists { get; set; } = true;
        public bool Updated { get; set; }

        public int ProjectId { get; set; }

        public string Branch { get; set; }

        public string SubjectInFile { get; set; }

        public string SubjectInDB { get; set; }

        public decimal? Hours { get; set; }

        public StudentStatus Status { get; set; }

        public DateTime? CurrStartDate { get; set; }
        public DateTime? CurrEndDate { get; set; }
        public decimal? CurrHours { get; set; }
        public bool IsSelected { get; set; }

    }
}
