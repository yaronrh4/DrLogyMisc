using System;
using System.Collections.Generic;


namespace DrLogy.CommitmentLettersUtils
{
    public class LettersPDFOptions
    {
        public LettersPDFOptions() { }
        public string Title { get; set; }
        public string Dates { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; } 
        
        public string SocialWorker { get; set; }
        public string Branch { get; set; }

        public List <Subject> Subjects { get; set; }

        public List<Coordinator> Coordinators { get; set; }


    }
}
