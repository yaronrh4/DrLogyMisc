using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitmentLetters
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

    }
}
