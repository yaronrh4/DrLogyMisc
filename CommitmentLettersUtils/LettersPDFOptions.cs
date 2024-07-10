using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;


namespace DrLogy.CommitmentLettersUtils
{
    public class LettersPDFOptions
    {

        public LettersPDFOptions() { }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Dates { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; } 
        
        public string SocialWorker { get; set; }
        public string Branch { get; set; }

        public List <Subject> Subjects { get; set; }

        public List<Coordinator> Coordinators { get; set; }

        public void LoadSubjectsFromDb(int projectId , string connection)
        {
            DbUtils.ConStr = connection;
            DataTable dt = DbUtils.GetSPData("SPMISC_GET_PROJECT_SUBJECTS" , new string[] { "prj_id" } , new object[] { projectId });
            this.Subjects.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                this.Subjects.Add(new Subject
                {
                    SubjectId = (int)dr["SUB_ID"],
                    NameInDB = (string)dr["NAME_IN_DB"],
                    NameInFile = (string)dr["NAME_IN_FILE"],
                    Grouped = dr["GROUPED"] is DBNull ? false : (bool)dr["GROUPED"],
                    Hours = dr["HOURS"] is DBNull ? 0 : (int)dr["HOURS"]
                });
            }
        }
    }
}
