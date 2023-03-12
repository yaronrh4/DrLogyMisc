namespace PDFMailer
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtSource = new System.Windows.Forms.TextBox();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.txtDestFolder = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConnection = new System.Windows.Forms.TextBox();
            this.btnSaveLog = new System.Windows.Forms.Button();
            this.btnDeleteEmpty = new System.Windows.Forms.Button();
            this.txtMailMessage = new System.Windows.Forms.TextBox();
            this.txtSampleEmail = new System.Windows.Forms.TextBox();
            this.btnSendOneMail = new System.Windows.Forms.Button();
            this.btnSendMail = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtToPage = new System.Windows.Forms.TextBox();
            this.txtFromPage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMailSubject = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbPDFType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.btnLoadLog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(497, 31);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(188, 20);
            this.txtSource.TabIndex = 0;
            this.txtSource.Text = "c:\\_a\\p_1-5.pdf";
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Location = new System.Drawing.Point(416, 31);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(75, 23);
            this.btnChooseFile.TabIndex = 2;
            this.btnChooseFile.Text = "בחר קובץ";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(708, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "שם קובץ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(696, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "תיקיית יעד";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(416, 60);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFolder.TabIndex = 3;
            this.btnSelectFolder.Text = "בחר תיקיה";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // txtDestFolder
            // 
            this.txtDestFolder.Location = new System.Drawing.Point(497, 60);
            this.txtDestFolder.Name = "txtDestFolder";
            this.txtDestFolder.ReadOnly = true;
            this.txtDestFolder.Size = new System.Drawing.Size(188, 20);
            this.txtDestFolder.TabIndex = 3;
            this.txtDestFolder.Text = "c:\\_work";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(408, 146);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "יציאה";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(648, 146);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 6;
            this.btnProcess.Text = "בצע";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(-15, 175);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(788, 215);
            this.dataGridView1.TabIndex = 11;
            this.dataGridView1.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseDoubleClick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(1, 128);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RightToLeftLayout = true;
            this.progressBar1.Size = new System.Drawing.Size(392, 23);
            this.progressBar1.TabIndex = 10;
            // 
            // txtResults
            // 
            this.txtResults.Location = new System.Drawing.Point(12, 13);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ReadOnly = true;
            this.txtResults.Size = new System.Drawing.Size(380, 109);
            this.txtResults.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(691, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "בסיס נתונים";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(497, 1);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.Size = new System.Drawing.Size(188, 20);
            this.txtConnection.TabIndex = 1;
            // 
            // btnSaveLog
            // 
            this.btnSaveLog.Enabled = false;
            this.btnSaveLog.Location = new System.Drawing.Point(683, 397);
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(75, 23);
            this.btnSaveLog.TabIndex = 12;
            this.btnSaveLog.Text = "שמור לוג";
            this.btnSaveLog.UseVisualStyleBackColor = true;
            this.btnSaveLog.Click += new System.EventHandler(this.btnSaveLog_Click);
            // 
            // btnDeleteEmpty
            // 
            this.btnDeleteEmpty.Enabled = false;
            this.btnDeleteEmpty.Location = new System.Drawing.Point(570, 397);
            this.btnDeleteEmpty.Name = "btnDeleteEmpty";
            this.btnDeleteEmpty.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteEmpty.TabIndex = 13;
            this.btnDeleteEmpty.Text = "מחק ריקים";
            this.btnDeleteEmpty.UseVisualStyleBackColor = true;
            this.btnDeleteEmpty.Click += new System.EventHandler(this.btnDeleteEmpty_Click);
            // 
            // txtMailMessage
            // 
            this.txtMailMessage.Enabled = false;
            this.txtMailMessage.Location = new System.Drawing.Point(231, 425);
            this.txtMailMessage.Multiline = true;
            this.txtMailMessage.Name = "txtMailMessage";
            this.txtMailMessage.Size = new System.Drawing.Size(282, 82);
            this.txtMailMessage.TabIndex = 16;
            // 
            // txtSampleEmail
            // 
            this.txtSampleEmail.Enabled = false;
            this.txtSampleEmail.Location = new System.Drawing.Point(43, 423);
            this.txtSampleEmail.Name = "txtSampleEmail";
            this.txtSampleEmail.Size = new System.Drawing.Size(182, 20);
            this.txtSampleEmail.TabIndex = 18;
            // 
            // btnSendOneMail
            // 
            this.btnSendOneMail.Enabled = false;
            this.btnSendOneMail.Location = new System.Drawing.Point(64, 394);
            this.btnSendOneMail.Name = "btnSendOneMail";
            this.btnSendOneMail.Size = new System.Drawing.Size(161, 23);
            this.btnSendOneMail.TabIndex = 17;
            this.btnSendOneMail.Text = "שנה את כל המיילים ל:";
            this.btnSendOneMail.UseVisualStyleBackColor = true;
            this.btnSendOneMail.Click += new System.EventHandler(this.btnSendOneMail_Click);
            // 
            // btnSendMail
            // 
            this.btnSendMail.Enabled = false;
            this.btnSendMail.Location = new System.Drawing.Point(565, 453);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(146, 23);
            this.btnSendMail.TabIndex = 14;
            this.btnSendMail.Text = "שלח מייל למורים";
            this.btnSendMail.UseVisualStyleBackColor = true;
            this.btnSendMail.Click += new System.EventHandler(this.btnSendMail_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(554, 146);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "עצור";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtToPage
            // 
            this.txtToPage.Location = new System.Drawing.Point(497, 86);
            this.txtToPage.Name = "txtToPage";
            this.txtToPage.Size = new System.Drawing.Size(46, 20);
            this.txtToPage.TabIndex = 5;
            // 
            // txtFromPage
            // 
            this.txtFromPage.Location = new System.Drawing.Point(639, 86);
            this.txtFromPage.Name = "txtFromPage";
            this.txtFromPage.Size = new System.Drawing.Size(46, 20);
            this.txtFromPage.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(549, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "עד עמוד";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(721, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "מעמוד";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtMailSubject
            // 
            this.txtMailSubject.Enabled = false;
            this.txtMailSubject.Location = new System.Drawing.Point(231, 396);
            this.txtMailSubject.Name = "txtMailSubject";
            this.txtMailSubject.Size = new System.Drawing.Size(282, 20);
            this.txtMailSubject.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(519, 458);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "הודעה";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(519, 396);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "נושא";
            // 
            // cmbPDFType
            // 
            this.cmbPDFType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPDFType.FormattingEnabled = true;
            this.cmbPDFType.Items.AddRange(new object[] {
            "פירוט",
            "טופס 806"});
            this.cmbPDFType.Location = new System.Drawing.Point(549, 112);
            this.cmbPDFType.Name = "cmbPDFType";
            this.cmbPDFType.Size = new System.Drawing.Size(136, 21);
            this.cmbPDFType.TabIndex = 30;
            this.cmbPDFType.SelectedIndexChanged += new System.EventHandler(this.cmbPDFType_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(706, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "סוג קובץ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(497, 115);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "שנה";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtYear
            // 
            this.txtYear.Enabled = false;
            this.txtYear.Location = new System.Drawing.Point(445, 112);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(46, 20);
            this.txtYear.TabIndex = 33;
            // 
            // btnLoadLog
            // 
            this.btnLoadLog.Enabled = false;
            this.btnLoadLog.Location = new System.Drawing.Point(683, 426);
            this.btnLoadLog.Name = "btnLoadLog";
            this.btnLoadLog.Size = new System.Drawing.Size(75, 23);
            this.btnLoadLog.TabIndex = 34;
            this.btnLoadLog.Text = "טען לוג";
            this.btnLoadLog.UseVisualStyleBackColor = true;
            this.btnLoadLog.Click += new System.EventHandler(this.btnLoadLog_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 540);
            this.Controls.Add(this.btnLoadLog);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbPDFType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtMailSubject);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFromPage);
            this.Controls.Add(this.txtToPage);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnSendMail);
            this.Controls.Add(this.btnSendOneMail);
            this.Controls.Add(this.txtSampleEmail);
            this.Controls.Add(this.txtMailMessage);
            this.Controls.Add(this.btnDeleteEmpty);
            this.Controls.Add(this.btnSaveLog);
            this.Controls.Add(this.txtConnection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.txtDestFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.txtSource);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "PDFMailer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TextBox txtDestFolder;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConnection;
        private System.Windows.Forms.Button btnSaveLog;
        private System.Windows.Forms.Button btnDeleteEmpty;
        private System.Windows.Forms.TextBox txtMailMessage;
        private System.Windows.Forms.TextBox txtSampleEmail;
        private System.Windows.Forms.Button btnSendOneMail;
        private System.Windows.Forms.Button btnSendMail;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtToPage;
        private System.Windows.Forms.TextBox txtFromPage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMailSubject;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbPDFType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.Button btnLoadLog;
    }
}

