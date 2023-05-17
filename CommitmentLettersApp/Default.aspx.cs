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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            get { 
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
            if (!Page.IsPostBack)
            {
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



                //options = CreateOptions();
                //bin folder
                string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
                _options = (LettersPDFOptions)Utils.DeSerializeObjectUTF(path + "\\" + OPTIONS_FILENAME, typeof(LettersPDFOptions));
                _lettersPDF = new LettersPDF(_options);

                Session["lettersPDF"] = _lettersPDF;
                Session["connection"] = _connection;
                Session["project"] = _project;
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
            SubjectData s = r.Subjects[int.Parse(subjectidx.Value)];

            r.StartDate = DateTime.ParseExact(startdate.Value.Trim(), "dd/MM/yyyy" , null);
            r.EndDate = DateTime.ParseExact(enddate.Value.Trim(), "dd/MM/yyyy" , null);

            s.Hours = decimal.Parse(hours.Value);

            _lettersPDF.RefreshStatus(int.Parse(stsubidx.Value), int.Parse(subjectidx.Value));
            RefreshData();
        }
        protected void btnAddPdf_Click(object sender, EventArgs e)
        {
            string tempDir = Utils.GetAppSetting("TempDir", "");
            tempDir = Server.MapPath(tempDir);

            foreach (var file in fuPdfs.PostedFiles)
            {
                string filename = $"{tempDir}\\{file.FileName}";
                fuPdfs.PostedFiles[0].SaveAs(filename);
                _lettersPDF.Process(filename, _connection, _project);
            }
            //RefreshData();
            RefreshData();
        }

        private void RefreshData()
        {
            rep1.DataSource = _lettersPDF.Results;
            rep1.DataBind();
        }

        protected void btnSaveStudent_Click(object sender, EventArgs e)
        {
            LetterData r = _lettersPDF.Results[int.Parse(stidx.Value)];

            r.CurrFirstName = firstname.Value;
            r.CurrLastName = lastname.Value;
            r.IdNum = idnum.Value;
            r.CurrPhone = phone.Value;
            r.CurrEmail = email.Value;
            r.CoordinatorName = coordinatorname.Value;
            r.Branch = branch.Value;
            r.SocialWorker = socialworker.Value;

            //_lettersPDF.UpdateStudent(int.Parse(stidx.Value) , _connection);

            //_lettersPDF.RefreshStatus(int.Parse(stsubidx.Value), int.Parse(subjectidx.Value));
            RefreshData();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (confirmaction.Value == "savestudents")
            {
                for (int i = 0; i < _lettersPDF.Results.Count; i++)
                    _lettersPDF.UpdateStudent(i, _connection);
                successhidden.Value = "השמירה בוצעה בהצלחה";
            }
        }

        protected void btnSendMails_Click(object sender, EventArgs e)
        {

        }
    }
}