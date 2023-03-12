using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitmentLetters
{
    public class Subject
    {
        public Subject() { }

        public Subject(string btlName, string name, int hours = 0, bool grouped = false) 
        { 
            this.BTLName = btlName; 
            this.Name= name; 
            this.Grouped = grouped;
            this.Hours = hours;
        }

        public string BTLName
        {
            get;set;
        }

        public string Name
        {
            get; set;
        }

        public int Hours
        {
            get; set;
        }

        public bool Grouped
        { 
            get; set; 
        }
    }
}
