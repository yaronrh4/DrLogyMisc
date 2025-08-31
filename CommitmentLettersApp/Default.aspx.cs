using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;
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
using System.Threading;
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
        protected int ProjectId
        {
            set
            {
                ViewState["ProjectId"] = value;
            }
            get
            {
                return ViewState["ProjectId"] != null ? (int)ViewState["ProjectId"] : 0;
            }
        }

        private string Connection
        {
            set
            {
                ViewState["Connection"] = value;
            }
            get
            {
                return ViewState["Connection"] != null ? (string)ViewState["Connection"] : null;
            }
        }
        private string DefaultCoordinatorName
        {
            set
            {
                ViewState["DefaultCoordinatorName"] = value;
            }
            get
            {
                return ViewState["DefaultCoordinatorName"] != null ? (string)ViewState["DefaultCoordinatorName"] : null;
            }
        }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            datachanged.Value = "";
            allowchangeproject.Value = "1";

            if (!Page.IsPostBack)
            {
                if (UserManager.UserId == 0)
                    Response.Redirect("Login.aspx");
                else
                {
                    //set login log if we want
                }

                    //SettingsProperty prop = null;
                    //int i = 1;
                    //while ((prop = Settings.Default.Properties[$"Connection{i}Name"]) != null)
                    //{
                    //    cmbConnection.Items.Add(prop.DefaultValue);
                    //    i++;
                    //}
                    //cmbConnection.SelectedIndex = cmbConnection.FindString(Utils.Utils.GetAzureEnvironmentVariable("Connection"));
                    //if (cmbConnection.SelectedIndex == -1)
                    //    cmbConnection.SelectedIndex = 0;
                    this.Connection = Utils.GetAzureEnvironmentVariable("Connection");

                DrLogy.DrLogyUtils.DbUtils.ConStr = Connection;

                DataTable dtProject = DbUtils.GetSPData("SPMISC_GET_USER_TEMPLATE", new string[] { "TEACHER_ID", "TEMPLATE_TYPE" }, new object[] { UserManager.UserId, 3 /*Project*/ });
                if (dtProject.Rows.Count == 0)
                    throw new Exception("שגיאה בטעינת פרוייקט ברירת מחדל");

                DataTable dtDefaultCoordinator = DbUtils.GetSPData("SPMISC_GET_USER_TEMPLATE", new string[] { "TEACHER_ID", "TEMPLATE_TYPE" }, new object[] { UserManager.UserId, 4 /*DefaultCoordinator*/ });
                if (dtDefaultCoordinator.Rows.Count == 0)
                    throw new Exception("שגיאה בטעינת רכז ברירת מחדל לתלמיד");
                string projectId = (string)dtProject.Rows[0]["ACT_VALUE"] ;
                if (projectId == "")
                    projectId = "304";// שיקום ביטוח לאומי
                this.ProjectId = int.Parse(projectId); 
                DataTable dtPrj = DbUtils.GetSPData("SPAPP_GET_PROJECTS", new string[] { "IN_COMMITMENT" }, new object[] { 1 });
                drpProjects.DataSource = dtPrj;
                drpProjects.DataTextField = "PRJ_NAME";
                drpProjects.DataValueField = "PRJ_ID";
                drpProjects.DataBind();
                drpProjects.SelectedValue = projectId;
                this.DefaultCoordinatorName = (string)dtDefaultCoordinator.Rows[0]["ACT_VALUE"];

                defcoordinator.Value = this.DefaultCoordinatorName;


                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                var descriptionAttribute = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
                string description = descriptionAttribute?.Description ?? "No description available";

                this.Title += $" {version} {description}" ;

                //txtProject.Text = Utils.Utils.GetAzureEnvironmentVariable("Project", txtProject.Text);
                //txtBCC.Text = Utils.Utils.GetAzureEnvironmentVariable("BCC", txtBCC.Text);
                //txtTestEmail.Text = Utils.Utils.GetAzureEnvironmentVariable("TestEmail", txtTestEmail.Text);



                //Uncomment to update xml file
                //_options = CreateOptions();

                //bin folder
                string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
                _options = (LettersPDFOptions)Utils.DeSerializeObjectUTF(path + "\\" + OPTIONS_FILENAME, typeof(LettersPDFOptions));
                _options.LoadSubjectsFromDb(this.ProjectId, this.Connection);
                _options.Coordinators = LoadCoordinators();
                _options.ProjectId = int.Parse(projectId);

                _lettersPDF = new LettersPDF(_options);

                foreach (var z in _lettersPDF.Options.Subjects)
                {
                    chklstSubjects.Items.Add(z.NameInFile);
                }

                Session["lettersPDF"] = _lettersPDF;

                coordinatorname.DataSource = _options.Coordinators;
                coordinatorname.DataValueField = "Name";
                coordinatorname.DataTextField = "Name";
                coordinatorname.DataBind();

                DataTable dtFiles = DbUtils.GetSPData("SPMISC_GET_TEMPLATE_FILES"); 
                drpFiles.DataSource = dtFiles;
                drpFiles.DataTextField = "ACTF_NAME";
                drpFiles.DataValueField = "ACTF_FILENAME";
                drpFiles.DataBind();
                drpFiles.SelectedIndex = 0;


            }
            else
            {
                _lettersPDF = Session["lettersPDF"] as LettersPDF;
            }
        }

        private List<Coordinator> LoadCoordinators()
        {
            DrLogy.DrLogyUtils.DbUtils.ConStr = Connection;

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
            if (UserManager.UserId == 0)
                Response.Redirect("Login.aspx");
            try
            {
                LetterData r = _lettersPDF.Results[int.Parse(stsubidx.Value)];
                SubjectData s = null;
                if (int.Parse(subjectidx.Value) >= 0)
                    s = r.Subjects[int.Parse(subjectidx.Value)];
                else
                {
                    s = new SubjectData();
                    r.Subjects.Add(s);
                    s.SubjectInFile = subjectname.Value;
                    s.SubjectInDB = lettersPDF.Options.Subjects.Where(z => z.NameInFile == s.SubjectInFile).Select(q => q.NameInDB).First();
                    s.SubjectId = lettersPDF.Options.Subjects.Where(z => z.NameInFile == s.SubjectInFile).Select(q => q.SubjectId).First();
                    DataTable dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData("SPMISC_GET_SUBJECT", new string[] { "zehut", "prj_id", "sub_id" }, new object[] { r.IdNum, r.ProjectId, s.SubjectId });

                    if (dt.Rows.Count > 0)
                    {
                        var row = dt.Rows[0];
                        s.CurrHours = (decimal?)row["ST_MAX_HOURS"];
                        s.CurrStartDate = (DateTime?)row["ST_SIGNDATE"];
                        s.CurrEndDate = (DateTime?)row["ST_DATEEND"];
                    }
                }

                r.StartDate = DateTime.ParseExact(startdate.Value.Trim(), "dd/MM/yyyy", null);
                r.EndDate = DateTime.ParseExact(enddate.Value.Trim(), "dd/MM/yyyy", null);

                if (!string.IsNullOrEmpty(hours.Value))
                    s.Hours = decimal.Parse(hours.Value);
                else
                    s.Hours = null;

                if (int.Parse(subjectidx.Value) >= 0)
                    _lettersPDF.RefreshStatus(int.Parse(stsubidx.Value), int.Parse(subjectidx.Value));
                else
                    _lettersPDF.RefreshStatus(int.Parse(stsubidx.Value), r.Subjects.Count - 1);
                RefreshData();
            }
            catch (Exception ex)
            {
                errorhidden.Value = $"שגיאה בשמירה {ex.Message}";
            }
        }
        protected void btnAddPdf_Click(object sender, EventArgs e)
        {
            string rc = "";
            if (UserManager.UserId == 0)
                Response.Redirect("Login.aspx");
            try
            {
                string tempDir = Utils.GetAzureEnvironmentVariable("TempDir");
                tempDir = Server.MapPath(tempDir);

                foreach (var file in fuPdfs.PostedFiles)
                {
                    string filename = $"{tempDir}\\{file.FileName}";
                    file.SaveAs(filename);

                    _lettersPDF.Results.Clear();
                    rc = _lettersPDF.Process(filename, Connection, DefaultCoordinatorName);

                    if (!string.IsNullOrEmpty (rc))
                        warninghidden.Value = $"אזהרה: {rc}";
                }


                RefreshData();
                allowchangeproject.Value = "0";
            }
            catch (Exception ex)
            {
                errorhidden.Value = $"שגיאה בטעינת קובץ {ex.Message}";
            }

        }

        private void RefreshData()
        {
            rep1.DataSource = _lettersPDF.Results;
            rep1.DataBind();
            datachanged.Value = "";
            for (int i = 0; i < _lettersPDF.Results.Count && datachanged.Value == ""; i++)
            {
                if (_lettersPDF.Results[i].Edited)
                {
                    datachanged.Value = "1";
                    allowchangeproject.Value = "0";
                }
                else
                {
                    foreach (var sub in _lettersPDF.Results[i].Subjects)
                        if (sub.Status == StudentStatus.NoStudent || sub.Status == StudentStatus.NoSubject || sub.Status == StudentStatus.NotUpdated)
                        {
                            datachanged.Value = "1";
                            allowchangeproject.Value = "0";
                        }
                }
            }
        }

        protected void btnSaveStudent_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserManager.UserId == 0)
                    Response.Redirect("Login.aspx");

                LetterData r = null;
                int stIdx = int.Parse(stidx.Value);
                if (stIdx >= 0)
                {
                    r = _lettersPDF.Results[int.Parse(stidx.Value)];
                }
                else
                {
                    _lettersPDF.Results.Clear();
                    r = new LetterData();
                    r.ProjectId = this.ProjectId;
                    r.Subjects = new List<SubjectData>();
                    lettersPDF.Results.Add(r);
                }

                r.Id = (int)DrLogy.DrLogyUtils.DbUtils.ExecSP("SPMISC_GET_STID_BY_ZEHUT", new string[] { "zehut" }, new object[] { idnum.Value.Trim() }, true);
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

                //force opening the edit popup with the current values
                r.Edited = true;

                if (!_lettersPDF.CheckId ( r.IdNum))
                {
                    warninghidden.Value = $"אזהרה: תעודת הזהות {r.IdNum} אינה חוקית";
                }
                RefreshData();
            }
            catch (Exception ex)
            {
                errorhidden.Value = $"שגיאה בשמירת נתונים סטודנט {ex.Message}";

            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (UserManager.UserId == 0)
                Response.Redirect("Login.aspx");

            string rc = "";
            if (confirmaction.Value == "save")
            {
                try
                {
                    string userName = Utils.GetAzureEnvironmentVariable("AuditUserName") + " " + UserManager.UserName;
                    string addHours = Utils.GetAzureEnvironmentVariable("AuditAddHours");
                    if (string.IsNullOrEmpty(addHours))
                        addHours = "0";
                    DbUtils.ExecSP("SP_AUDIT", new string[] { "UserName", "AddHours" }, new object[] { userName, int.Parse(addHours) });
                    for (int i = 0; i < _lettersPDF.Results.Count && rc == ""; i++)
                    {
                        rc = _lettersPDF.UpdateStudent(i, Connection);
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
                        datachanged.Value = "";
                        var arc = new FileArchive();
                        string prevFileName = "";
                        foreach (var res in _lettersPDF.Results)
                        {
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(res.FileName) && prevFileName != res.FileName)
                                {
                                    prevFileName = res.FileName;
                                    if (res.FileName.ToLower().EndsWith(".xlsx") || res.FileName.ToLower().EndsWith(".xls"))
                                    {
                                        arc.UploadArchive(1, res.FileName, 0, new string[] { "PRJ" }, new string[] { res.ProjectId.ToString() });
                                    }
                                    else //PDF
                                    {
                                        arc.UploadArchive(1, res.FileName, 0, new string[] { "ZEHUT", "PRJ", "START_DATE", "END_DATE" }, new string[] { res.IdNum, res.ProjectId.ToString(), Utils.DateToString(res.StartDate), Utils.DateToString(res.EndDate) });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                errorhidden.Value = $"שגיאה בהעלאת קובץ ההתחייבות לארכיון: {ex.Message}";
                            }
                        }
                    }
                    else
                    {
                        datachanged.Value = "1";
                    }

                    RefreshData();
                }
                catch (Exception ex)
                {
                    errorhidden.Value = $"שגיאה בשמירה {ex.Message}";
                }
            }
        }

        public object Eval2(string r , string format = null , object defaultValue =null)
        {
            if (defaultValue == null)
                defaultValue = "";
            var obj = format == null ? Eval(r) : Eval(r, format);
            if (obj == null)
                obj = defaultValue;
            return obj;
        }

        public object AttrEval(string r)
        {
            object rc = Eval(r);

            if (rc==null)
            {
                rc = "";
            }
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
            if (UserManager.UserId == 0)
                Response.Redirect("Login.aspx");

            _lettersPDF.Results.Clear();
            allowchangeproject.Value = "1";
            RefreshData();
        }


        private void SaveToExcel(string fileName)
        {
            try
            {
                _lettersPDF.ExportToExcel(fileName);
            }
            catch (ThreadAbortException ex)
            {
                //WriteToLog("שגיאה בשמירת לוג");
                //throw;
            }
            catch (Exception ex2)
            {
                throw;
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (UserManager.UserId == 0)
                Response.Redirect("Login.aspx");

            try
            {
                if (_lettersPDF.Results.Count > 0)
                {
                    SaveToExcel($"letters");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                errorhidden.Value = $"שגיאה ביצוא לאקסל {ex.Message}";
            }

        }

        protected void btnLoadStudent_Click(object sender, EventArgs e)
        {
            if (UserManager.UserId == 0)
                Response.Redirect("Login.aspx");
            try
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
                _lettersPDF.Results.Clear();
                
                if (!_lettersPDF.LoadStudent(Loadidnum.Value, subjects.ToArray(), startDate, endDate, Connection, DefaultCoordinatorName))
                { 
                    if (!_lettersPDF.CheckId (Loadidnum.Value))
                        errorhidden.Value = $"אזהרה: תעודת הזהות {Loadidnum.Value} אינה חוקית <br>";

                    errorhidden.Value += "לא נמצאו תלמידים עם הת.ז המבוקשת. לצורך הוספת תלמיד חדש יש ללחוץ על הוספת תלמיד ";
                    return;
                }
                Loadidnum.Value = "";
                Loadstartdate.Value = "";
                Loadenddate.Value = "";
                chklstSubjects.ClearSelection();

                if (_lettersPDF.Results.Count > 0 && !_lettersPDF.CheckId (_lettersPDF.Results[0].IdNum))
                    warninghidden.Value = $"אזהרה: תעודת הזהות {_lettersPDF.Results[0].IdNum} אינה חוקית";

                RefreshData();
                allowchangeproject.Value = "0";
            }
            catch (Exception ex)
            {
                errorhidden.Value = ex.Message;
            }

        }

        protected void btnImportFromExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string tempDir = Utils.GetAzureEnvironmentVariable("TempDir");
                tempDir = Server.MapPath(tempDir);

                string filename = $"{tempDir}\\{fuExcel.PostedFile.FileName}";
                fuExcel.PostedFile.SaveAs(filename);
                string rc = lettersPDF.ImportFromExcel(filename, Connection, DefaultCoordinatorName);

                if (string.IsNullOrEmpty(rc))
                {
                    datachanged.Value = "1";
                    allowchangeproject.Value = "0";
                    RefreshData();
                }
                else
                {
                    errorhidden.Value = rc;

                }

            }
            catch (Exception ex)
            {
                errorhidden.Value = $"שגיאה ביבוא קובץ אקסל {ex.Message}";
            }
        }

        DateTime dtStart;

        protected override void OnPreInit(EventArgs e)
        {
            dtStart = DateTime.Now;
            errorhidden.Value = "";
            warninghidden.Value = "";

            base.OnPreInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                base.Render(writer);
                Response.Write("<!--" + (DateTime.Now - dtStart).TotalMilliseconds + "-->");
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSetProject_Click(object sender, EventArgs e)
        {
            try
            {
                DbUtils.ExecSP("SPMISC_SET_USER_TEMPLATE", new string[] { "TEACHER_ID", "TEMPLATE_TYPE", "TEMPLATE_VALUE", "CDATE" }, new object[] { UserManager.UserId, 3, int.Parse(drpProjects.SelectedValue), Utils.DateTimeNow() }, true);
            }
            catch (Exception ex)
            {
                errorhidden.Value = $"שגיאה בהחלפת פרוייקט {ex.Message}";
            }
        }

        protected void drpProjects_SelectedIndexChanged(object sender, EventArgs e)
        {

            _lettersPDF.Options.ProjectId = this.ProjectId = int.Parse(drpProjects.SelectedValue);
            _lettersPDF.Options.LoadSubjectsFromDb(_lettersPDF.Options.ProjectId, this.Connection);
            chklstSubjects.Items.Clear();
            foreach (var z in _lettersPDF.Options.Subjects)
            {
                chklstSubjects.Items.Add(z.NameInFile);
            }      
        }
        protected string AllowLoadPDF()
        {
            string allow = " disabled='disabled'";
            if (drpProjects.SelectedValue == "304")
                allow = "";

            return allow;
        }
    }
}