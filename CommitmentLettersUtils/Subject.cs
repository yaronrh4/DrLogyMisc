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

        public Subject(string nameInFile, string name, int hours = 0, bool grouped = false) 
        { 
            this.NameInFile = nameInFile; 
            this.Name= name; 
            this.Grouped = grouped;
            this.Hours = hours;
        }

        public string NameInFile
        {
            get;set;
        }

        public string Name
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
