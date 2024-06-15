using DrLogy.CommitmentLettersUtils;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Security.Cryptography;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace CommitmentLettersApp
{
    public partial class Default : System.Web.UI.Page
    {

        private const string OPTIONS_FILENAME = "letteroptions.xml";
        private LettersPDF _lettersPDF = null;
        private LettersPDFOptions _options = null;
        private string _connection = null;
        private string _project = null;
        private string _defaultCoordinatorName = null;

        public LettersPDF lettersPDF
        {
            get
            {
                return _lettersPDF;
            }
        }


        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        private LettersPDFOptions CreateOptions()
        {
            try
            {
                LettersPDFOptions options = new LettersPDFOptions();
                options.Title = "הנדון:";
                options.Dates = "אנו מתחייבים לממן";
                options.Phone = "מס' טלפון:";
                options.Email = "כתובת מייל:";
                options.SocialWorker = "פקיד/ת שיקום";
                options.Branch = "סניף:";

                List<Subject> subjects = new List<Subject>();
                subjects.Add(new Subject("תמלול", "תמלול", 0, true));
                subjects.Add(new Subject("הקניית אסטרטגיות למידה", "אסטרטגיות למידה"));
                subjects.Add(new Subject("שעורי עזר", "שיעורי עזר"));
                subjects.Add(new Subject("ליווי בהכשרה מקצועית", "ליווי בהכשרה מקצועית", 10));
                subjects.Add(new Subject("תרגום הרצאה לשפת סימנים", "תרגום לשפת הסימנים", 0, true));
                subjects.Add(new Subject("שקלוט הרצאה", "שקלוט", 0, true));
                subjects.Add(new Subject("חונכות", "חונכות"));
                subjects.Add(new Subject("הקראה למקבל השירות", "הקראות"));

                options.Subjects = subjects;

                Utils.SerializeObjectUTF($"c:\\temp\\{OPTIONS_FILENAME}", options);

                return options;
            }
            catch (Exception ex)
            {
                //LogError("CreateOptions", ex);
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            datachanged.Value = "";
            if (!Page.IsPostBack)
            {
                Pref.UserId = 4921; //for now until login - default orian

                //SettingsProperty prop = null;
                //int i = 1;
                //while ((prop = Settings.Default.Properties[$"Connection{i}Name"]) != null)
                //{
                //    cmbConnection.Items.Add(prop.DefaultValue);
                //    i++;
                //}
                //cmbConnection.SelectedIndex = cmbConnection.FindString(Utils.GetAppSetting("Connection", "0"));
                //if (cmbConnection.SelectedIndex == -1)
                //    cmbConnection.SelectedIndex = 0;
                _connection = Utils.GetAppSetting("Connection", "");

                DrLogy.DrLogyUtils.DbUtils.ConStr = _connection;

                DataTable dtProject = DbUtils.GetSPData("SPMISC_GET_USER_TEMPLATE", new string[] { "TEACHER_ID", "TEMPLATE_TYPE" }, new object[] { Pref.UserId, 3 /*Project*/ });
                if (dtProject.Rows.Count == 0)
                    throw new Exception("שגיאה בטעינת פרוייקט ברירת מחדל");

                DataTable dtDefaultCoordinator = DbUtils.GetSPData("SPMISC_GET_USER_TEMPLATE", new string[] { "TEACHER_ID", "TEMPLATE_TYPE" }, new object[] { Pref.UserId, 4 /*DefaultCoordinator*/ });
                if (dtDefaultCoordinator.Rows.Count == 0)
                    throw new Exception("שגיאה בטעינת רכז ברירת מחדל לתלמיד");

                _project = (string)dtProject.Rows[0]["ACT_VALUE"];
                _defaultCoordinatorName = (string)dtDefaultCoordinator.Rows[0]["ACT_VALUE"];

                defcoordinator.Value = _defaultCoordinatorName;


                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                this.Title += " " + version;

                //txtProject.Text = Utils.GetAppSetting("Project", txtProject.Text);
                //txtBCC.Text = Utils.GetAppSetting("BCC", txtBCC.Text);
                //txtTestEmail.Text = Utils.GetAppSetting("TestEmail", txtTestEmail.Text);



                //Uncomment to update xml file
                //_options = CreateOptions();

                //bin folder
                string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
                _options = (LettersPDFOptions)Utils.DeSerializeObjectUTF(path + "\\" + OPTIONS_FILENAME, typeof(LettersPDFOptions));
                _options.Coordinators = LoadCoordinators();
                _lettersPDF = new LettersPDF(_options);

                foreach (var z in _lettersPDF.Options.Subjects)
                {
                    chklstSubjects.Items.Add(z.BTLName);
                }

                Session["lettersPDF"] = _lettersPDF;
                Session["connection"] = _connection;
                Session["project"] = _project;

                coordinatorname.DataSource = _options.Coordinators;
                coordinatorname.DataValueField = "Name";
                coordinatorname.DataTextField = "Name";
                coordinatorname.DataBind();
            }
            else
            {
                _lettersPDF = Session["lettersPDF"] as LettersPDF;
                _connection = Session["connection"] as string;
                _project = Session["project"] as string;
            }
        }

        private List<Coordinator> LoadCoordinators()
        {
            DrLogy.DrLogyUtils.DbUtils.ConStr = _connection;

            DataTable dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_COORDINATORS" );

            List<Coordinator> lst = new List<Coordinator>();

            foreach (DataRow r in dt.Rows)
            {
                lst.Add(new Coordinator()
                {
                    Name = r["ACR_NAME"].ToString(),
                    Email = r["ACR_EMAIL"].ToString(),
                    Phone = r["ACR_PHONE"].ToString(),
                    TeacherId = (int)r["ACR_TEACHER_ID"]
                });
                ;
            }
            return lst;
        }

        protected void btnSaveSubject_Click(object sender, EventArgs e)
        {
            LetterData r = _lettersPDF.Results[int.Parse(stsubidx.Value)];
            SubjectData s = null;
            if (int.Parse(subjectidx.Value) >= 0)
                s = r.Subjects[int.Parse(subjectidx.Value)];
            else
            {
                s = new SubjectData();
                r.Subjects.Add(s);
                s.SubjectBTL = subjectname.Value;
                s.SubjectInDB = lettersPDF.Options.Subjects.Where(z => z.BTLName == s.SubjectBTL).Select(q => q.Name).First();

                DataTable dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_SUBJECT", new string[] { "zehut", "project", "subject" }, new object[] { r.IdNum, r.Project, s.SubjectInDB });

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    s.CurrHours = (decimal?)row["ST_MAX_HOURS"];
                    s.CurrStartDate = (DateTime?)row["ST_SIGNDATE"];
                    s.CurrEndDate = (DateTime?)row["ST_DATEEND"];
                    s.IsNew = false;
                }
                else
                    s.IsNew = true;

            }

            r.StartDate = DateTime.ParseExact(startdate.Value.Trim(), "dd/MM/yyyy", null);
            r.EndDate = DateTime.ParseExact(enddate.Value.Trim(), "dd/MM/yyyy", null);

            s.Hours = decimal.Parse(hours.Value);

            if (int.Parse(subjectidx.Value) >= 0)
                _lettersPDF.RefreshStatus(int.Parse(stsubidx.Value), int.Parse(subjectidx.Value));
            else
                _lettersPDF.RefreshStatus(int.Parse(stsubidx.Value), r.Subjects.Count - 1);
            RefreshData();
            datachanged.Value = "1";
        }
        protected void btnAddPdf_Click(object sender, EventArgs e)
        {
            try
            {
                string tempDir = Utils.GetAppSetting("TempDir", "");
                tempDir = Server.MapPath(tempDir);

                foreach (var file in fuPdfs.PostedFiles)
                {
                    string filename = $"{tempDir}\\{file.FileName}";
                    file.SaveAs(filename);
                    _lettersPDF.Process(filename, _connection, _project , _defaultCoordinatorName);
                }

                for (int i = 0; i < _lettersPDF.Results.Count && datachanged.Value == ""; i++)
                    foreach (var sub in _lettersPDF.Results[i].Subjects)
                        if (sub.Status == StudentStatus.NoStudent || sub.Status == StudentStatus.NoSubject || sub.Status == StudentStatus.NotUpdated)
                            datachanged.Value = "1";

                RefreshData();
            }
            catch (Exception ex)
            {
                errorhidden.Value = ex.Message;
            }
        }

        private void RefreshData()
        {
            rep1.DataSource = _lettersPDF.Results;
            rep1.DataBind();
        }

        protected void btnSaveStudent_Click(object sender, EventArgs e)
        {
            LetterData r = null;
            int stIdx = int.Parse(stidx.Value);
            if (stIdx >= 0)
            {
                r = _lettersPDF.Results[int.Parse(stidx.Value)];
            }
            else
            {
                r = new LetterData();
                r.Project = _project;
                r.Subjects = new List<SubjectData>();
                lettersPDF.Results.Add(r);
            }

            r.Id = (int)DrLogy.DrLogyUtils.DbUtils.ExecSP("SPMISC_GET_STID_BY_ZEHUT", new string[] { "zehut" }, new object[] { idnum.Value.Trim() },true);
            r.IdNum = idnum.Value.Trim();
            r.CurrFirstName = firstname.Value;
            r.CurrLastName = lastname.Value;
            r.CurrPhone = phone.Value;
            r.CurrEmail = email.Value;
            r.CoordinatorName = coordinatorname.SelectedValue;
            r.CurrBranch = branch.Value;
            r.CurrSocialWorker = socialworker.Value;
            r.CreateDate = DateTime.ParseExact(createdate.Value.Trim(), "dd/MM/yyyy", null);
            r.IsNewStudent = isnewstudent.Checked;
            r.Comments = comments.Value;

            //_lettersPDF.UpdateStudent(int.Parse(stidx.Value) , _connection);

            //_lettersPDF.RefreshStatus(int.Parse(stsubidx.Value), int.Parse(subjectidx.Value));
            datachanged.Value = "1";

            //force opening the edit popup with the current values
            r.Edited = true;
            RefreshData();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string rc = "";
            if (confirmaction.Value == "save")
            {
                string userName = System.Configuration.ConfigurationManager.AppSettings["AuditUserName"].ToString();
                string addHours = System.Configuration.ConfigurationManager.AppSettings["AuditAddHours"].ToString();
                if (string.IsNullOrEmpty(addHours))
                    addHours = "0"; 
                DbUtils.ExecSP ("SP_AUDIT" , new string[] { "UserName" , "AddHours"} , new object[] {userName, int.Parse(addHours)} );
                for (int i = 0; i < _lettersPDF.Results.Count && rc == ""; i++)
                {
                    rc = _lettersPDF.UpdateStudent(i, _connection);
                    if (rc != "")
                        errorhidden.Value = rc;
                }

                for (int i = 0; i < _lettersPDF.Results.Count && rc == ""; i++)
                    for (int j = 0; j < _lettersPDF.Results[i].Subjects.Count && rc == ""; j++)
                    {
                        rc = _lettersPDF.UpdateSubject(i, j);
                        if (rc != "")
                            errorhidden.Value = rc;
                    }

                if (rc == "")
                {
                    successhidden.Value = "השמירה בוצעה בהצלחה";
                }
                else
                    datachanged.Value = "1";

                RefreshData();
            }
        }

        public object AttrEval(string r)
        {
            object rc = Eval(r);
            if (rc is string)
                rc = (rc as string).Replace("\"", "&quot;");
            return rc;
        }

        public object StudentAttrEval(string r)
        {
            object rc = Eval(r);
            if (rc is null)
            {
                rc = AttrEval("Curr" + r);
            }
            else
                rc = AttrEval(r);

            return rc;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            _lettersPDF.Results.Clear();
            RefreshData();
        }


        private void SaveToExcel(string fileName)
        {
            try
            {
                _lettersPDF.ExportToExcel(fileName);
            }
            catch (Exception ex)
            {
                //WriteToLog("שגיאה בשמירת לוג");
                throw;
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (_lettersPDF.Results.Count > 0)
            {
                SaveToExcel($"letters");
                successhidden.Value = "השמירה בוצעה בהצלחה";
            }

        }

        protected void btnLoadStudent_Click(object sender, EventArgs e)
        {
            List<string> subjects = new List<string>();

            for (int i = 0; i < chklstSubjects.Items.Count; i++)
            {
                if (chklstSubjects.Items[i].Selected)
                {
                    subjects.Add(chklstSubjects.Items[i].Value);
                }
            }

            DateTime startDate = DateTime.ParseExact(Loadstartdate.Value.Trim(), "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(Loadenddate.Value.Trim(), "dd/MM/yyyy", null);

            _lettersPDF.LoadStudent(Loadidnum.Value, subjects.ToArray(), startDate, endDate, _connection, _project , _defaultCoordinatorName );

            for (int i = 0; i < _lettersPDF.Results.Count && datachanged.Value == ""; i++)
                foreach (var sub in _lettersPDF.Results[i].Subjects)
                    if (sub.Status == StudentStatus.NoStudent || sub.Status == StudentStatus.NoSubject || sub.Status == StudentStatus.NotUpdated)
                        datachanged.Value = "1";
            Loadidnum.Value = "";
            Loadstartdate.Value = "";
            Loadenddate.Value = "";
            chklstSubjects.ClearSelection();

            RefreshData();

        }
    }
}