using DrLogy.CommitmentLettersUtils;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Security.Cryptography;
using System.Web;
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
                options.DefaultCoordinatorName = Utils.GetAppSetting("DefaultCoordinator", "");

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

                List<Coordinator> coordinators = new List<Coordinator>();
                coordinators.Add(new Coordinator("מיכאל", "0545422211"));
                coordinators.Add(new Coordinator("דשה", "0546953420"));
                coordinators.Add(new Coordinator("טובי", "0542331013"));
                coordinators.Add(new Coordinator("גל", "0525498369"));
                coordinators.Add(new Coordinator("אינה", "0587024681"));
                coordinators.Add(new Coordinator("אוריין", "0502645970"));
                coordinators.Add(new Coordinator("מעיין", "0523460209"));


                options.Coordinators = coordinators;

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
                defcoordinator.Value = Utils.GetAppSetting("DefaultCoordinator", "");

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
                _project = Utils.GetAppSetting("Project", _project);

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
                _lettersPDF = new LettersPDF(_options);

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

        protected void btnSaveSubject_Click(object sender, EventArgs e)
        {
            LetterData r = _lettersPDF.Results[int.Parse(stsubidx.Value)];
            SubjectData s = null;
            if (int.Parse(subjectidx.Value) >= 0)
                s = r.Subjects[int.Parse(subjectidx.Value)];
            else
            {
                s = new SubjectData();
                s.IsNew = true;
                r.Subjects.Add(s);
                s.SubjectBTL = subjectname.Value;
                s.SubjectInDB = lettersPDF.Options.Subjects.Where(z => z.BTLName == s.SubjectBTL).Select(q => q.Name).First();

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
                    _lettersPDF.Process(filename, _connection, _project);
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

            r.CurrFirstName = firstname.Value;
            r.CurrLastName = lastname.Value;
            r.IdNum = idnum.Value;
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
        protected void btnSendMails_Click(object sender, EventArgs e)
        {

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
                SaveToExcel($"letters.xlsx");
                successhidden.Value = "השמירה בוצעה בהצלחה";
            }

        }
    }
}
