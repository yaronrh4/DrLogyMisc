using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommitmentLetters
{
    public partial class frmMain : Form
    {
        private const string OPTIONS_FILENAME  = "letteroptions.xml";
        private BindingSource SBind = new BindingSource();
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text += " " + Application.ProductVersion;
            txtConnection.Text = Utils.GetAppSetting("Connection", txtConnection.Text);
            txtSource.Text = Utils.GetAppSetting("Source", "");

        }
        private LettersPDFOptions CreateOptions()
        {
            LettersPDFOptions options = new LettersPDFOptions();
            options.Title = "הנדון:";
            options.Dates = "אנו מתחייבים לממן";
            options.Phone = "מס' טלפון";
            options.Email = "כתובת מייל";
            options.SocialWorker = "פקיד/ת שיקום";
            options.Branch = "סניף:";

            List<Subject> subjects= new List<Subject>();
            subjects.Add(new Subject("תמלול", "תמלול",0,true));
            subjects.Add(new Subject("הקניית אסטרטגיות למידה", "הקניית אסטרטגיות למידה"));
            subjects.Add(new Subject("שעורי עזר", "שעורי עזר"));
            subjects.Add(new Subject("הקניית אסטרטגיות למידה", "הקניית אסטרטגיות למידה"));
            subjects.Add(new Subject("ליווי בהכשרה מקצועית", "ליווי בהכשרה מקצועית" ,10));
            subjects.Add(new Subject("תרגום הרצאה לשפת סימנים", "תרגום הרצאה לשפת סימנים",0,true));
            subjects.Add(new Subject("חונכות", "חונכות"));

            options.Subjects = subjects;
            Utils.SerializeObjectUTF(OPTIONS_FILENAME, options);

            return options;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            LettersPDFOptions options;
            //options = CreateOptions();
            options = (LettersPDFOptions)Utils.DeSerializeObjectUTF(OPTIONS_FILENAME, typeof (LettersPDFOptions));
            var p = new LettersPDF();
            p.Process (txtSource.Text, txtConnection.Text, options);

            ShowData(p.Results);
            //var letterProcessor = new 
        }

        private void ShowData(List<LetterData>list)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns.Clear();
            DataGridViewColumn c;


            c = new DataGridViewCheckBoxColumn();
            c.Name = "IsSelected";
            c.DataPropertyName = "";
            c.HeaderText = "";
            c.ReadOnly = false;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.DataPropertyName = "PageNumber";
            c.HeaderText = "עמוד";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "FileName";
            c.DataPropertyName = "FileName";
            c.HeaderText = "שם קובץ";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "Id";
            c.DataPropertyName = "Id";
            c.HeaderText = "ת.ז";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "Name";
            c.DataPropertyName = "Name";
            c.HeaderText = "שם התלמיד";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "Phone";
            c.DataPropertyName = "Phone";
            c.HeaderText = "טלפון";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "NameInPDF";
            c.DataPropertyName = "NameInPDF";
            c.HeaderText = "שם בקובץ";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);


            c = new DataGridViewTextBoxColumn();
            c.Name = "SubjectHours";
            c.DataPropertyName = "SubjectName";
            c.HeaderText = "הנגשה";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "SubjectHours";
            c.DataPropertyName = "SubjectHours";
            c.HeaderText = "שעות";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);


            c = new DataGridViewTextBoxColumn();
            c.Name = "StartDate";
            c.DataPropertyName = "StartDate";
            c.HeaderText = "התחלה";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "EndDate";
            c.DataPropertyName = "EndDate";
            c.HeaderText = "סיום";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "Branch";
            c.DataPropertyName = "Branch";
            c.HeaderText = "סניף";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "SocialWorker";
            c.DataPropertyName = "SocialWorker";
            c.HeaderText = "עו\"ס";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);


        c = new DataGridViewTextBoxColumn();
            c.Name = "Email";
            c.DataPropertyName = "Email";
            c.HeaderText = "דוא\"ל";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            c = new DataGridViewTextBoxColumn();
            c.Name = "SentDate";
            c.DataPropertyName = "SentDate";
            c.HeaderText = "תאריך משלוח";
            c.ReadOnly = true;
            dataGridView1.Columns.Add(c);

            SBind.DataSource = list;
            dataGridView1.DataSource = SBind;

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Title = "בחרו קובץ";
            fileDialog.Filter = "PDF|*.PDF";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = fileDialog.FileName;
            }
        }
    }
}
