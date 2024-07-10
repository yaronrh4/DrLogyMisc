using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrLogy.CommitmentLettersUtils
{
    [Serializable]
    public class Subject
    {
        public Subject() { }

        public Subject(int subjectId , string nameInFile, string nameInDB, int hours = 0, bool grouped = false) 
        { 
            this.SubjectId = subjectId; 
            this.NameInFile = nameInFile; 
            this.NameInDB = nameInDB; 
            this.Grouped = grouped;
            this.Hours = hours;
        }
        public int SubjectId { get; set; }
        public string NameInFile
        {
            get;set;
        }

        public string NameInDB
        {
            get; set;
        }
        public decimal Hours
        {
            get; set;
        }

        public bool Grouped
        { 
            get; set; 
        }
    }
}
