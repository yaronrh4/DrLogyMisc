using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrLogy.CommitmentLettersUtils
{
    [Serializable]
    public class Coordinator
    {
        public Coordinator() { }

        public Coordinator(string name, string phone, string email="")
        {
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
        }


        public string Name
        {
            get; set;
        }

        public string Phone
        {
            get; set;
        }

        public string Email
        {
            get;set;
        }

    }
}
