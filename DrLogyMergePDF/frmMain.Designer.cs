
namespace DrLogyMergePDF
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnMerge = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtDest = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.btnBrowseDest = new System.Windows.Forms.Button();
            this.chkClearDest = new System.Windows.Forms.CheckBox();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAddToFile = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnMerge
            // 
            this.btnMerge.AutoSize = true;
            this.btnMerge.Location = new System.Drawing.Point(471, 295);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(64, 23);
            this.btnMerge.TabIndex = 0;
            this.btnMerge.Text = "מזג";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "ספריית מקור";
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(195, 55);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(263, 20);
            this.txtSource.TabIndex = 3;
            // 
            // txtDest
            // 
            this.txtDest.Location = new System.Drawing.Point(195, 90);
            this.txtDest.Name = "txtDest";
            this.txtDest.Size = new System.Drawing.Size(263, 20);
            this.txtDest.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "ספריית יעד";
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(480, 55);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnBrowseSource.Size = new System.Drawing.Size(64, 20);
            this.btnBrowseSource.TabIndex = 6;
            this.btnBrowseSource.Text = "Browse";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // btnBrowseDest
            // 
            this.btnBrowseDest.Location = new System.Drawing.Point(480, 90);
            this.btnBrowseDest.Name = "btnBrowseDest";
            this.btnBrowseDest.Size = new System.Drawing.Size(64, 20);
            this.btnBrowseDest.TabIndex = 7;
            this.btnBrowseDest.Text = "Browse";
            this.btnBrowseDest.UseVisualStyleBackColor = true;
            this.btnBrowseDest.Click += new System.EventHandler(this.btnBrowseDest_Click);
            // 
            // chkClearDest
            // 
            this.chkClearDest.AutoSize = true;
            this.chkClearDest.Checked = true;
            this.chkClearDest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClearDest.Location = new System.Drawing.Point(347, 267);
            this.chkClearDest.Name = "chkClearDest";
            this.chkClearDest.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkClearDest.Size = new System.Drawing.Size(104, 17);
            this.chkClearDest.TabIndex = 10;
            this.chkClearDest.Text = "מחק ספרית יעד";
            this.chkClearDest.UseVisualStyleBackColor = true;
            // 
            // txtStart
            // 
            this.txtStart.Enabled = false;
            this.txtStart.Location = new System.Drawing.Point(195, 156);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(263, 20);
            this.txtStart.TabIndex = 11;
            // 
            // txtEnd
            // 
            this.txtEnd.Enabled = false;
            this.txtEnd.Location = new System.Drawing.Point(195, 182);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(263, 20);
            this.txtEnd.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "תוספת לשם קובץ ממוזג";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "טקסט התחלתי לחיפוש";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "טקסט התחלתי לחיפוש";
            // 
            // txtAddToFile
            // 
            this.txtAddToFile.Location = new System.Drawing.Point(195, 208);
            this.txtAddToFile.Name = "txtAddToFile";
            this.txtAddToFile.Size = new System.Drawing.Size(263, 20);
            this.txtAddToFile.TabIndex = 16;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 324);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(662, 194);
            this.richTextBox1.TabIndex = 17;
            this.richTextBox1.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 530);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.txtAddToFile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtEnd);
            this.Controls.Add(this.txtStart);
            this.Controls.Add(this.chkClearDest);
            this.Controls.Add(this.btnBrowseDest);
            this.Controls.Add(this.btnBrowseSource);
            this.Controls.Add(this.txtDest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnMerge);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "דר לוגי מיזוג קבצים";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtDest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.Button btnBrowseDest;
        private System.Windows.Forms.CheckBox chkClearDest;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAddToFile;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

