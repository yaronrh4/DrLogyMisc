﻿using DrLogy.DrLogyPDFMailerUtils;
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
    public class LetterData : BasePDFProperties {
        public LetterData() { }

        public string Phone { get; set; }
        public string SocialWorker { get; set; }

        public int ProjectId { get; set; }

        public string Branch { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CurrPhone { get; set; }
        public string CurrEmail { get; set; }
        public string CurrFirstName { get; set; }
        public string CurrLastName { get; set; }

        public string CurrBranch { get; set; }
        public string CurrSocialWorker { get; set; }
        public string CurrCoordinatorName { get; set; }

        public string CoordinatorName { get; set; }

        public string Comments { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<SubjectData> Subjects { get; set; }

        public DateTime CreateDate { get; set; }

        public bool? IsNewStudent { get; set; } = null;

        public bool Edited { get; set; } = false;

        //New fields for ExcelImport

        public string NewKey { get; set; }

        public string ClassName { get; set; }

        public string Age { get; set; }


        public string Address { get; set; }


        public string Mikud { get; set; }


        public string CurrNewKey { get; set; }

        public string CurrClassName { get; set; }

        public string CurrAge { get; set; }

        public string CurrAddress { get; set; }


        public string CurrMikud { get; set; }


    }
}
