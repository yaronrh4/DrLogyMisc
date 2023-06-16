using DrLogy.DrLogyPDFMailerUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DrLogy.CommitmentLettersUtils
{
    [Serializable]
    public class LetterData : BasePDFProperties    {
        public LetterData() { }

        public string Phone { get; set; }
        public string SocialWorker { get; set; }

        public string Project { get; set; }

        public string Branch { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CurrPhone { get; set; }
        public string CurrEmail { get; set; }
        public string CurrFirstName { get; set; }
        public string CurrLastName { get; set; }

        public string CurrBranch { get; set; }
        public string CurrSocialWorker { get; set; }


        public string CoordinatorName { get; set; }

        public string Comments { get; set; }

        public bool IsSelected { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List <SubjectData> Subjects { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
