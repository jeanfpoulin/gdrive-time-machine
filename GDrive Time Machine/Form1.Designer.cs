namespace GDrive_Time_Machine
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFindRevisions = new System.Windows.Forms.Button();
            this.lblModifiedSince = new System.Windows.Forms.Label();
            this.dateModifiedSince = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.timeModifiedSince = new System.Windows.Forms.DateTimePicker();
            this.btnRevertSelected = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.timeModifiedSince);
            this.groupBox1.Controls.Add(this.btnFindRevisions);
            this.groupBox1.Controls.Add(this.lblModifiedSince);
            this.groupBox1.Controls.Add(this.dateModifiedSince);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(646, 68);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Step 1";
            // 
            // btnFindRevisions
            // 
            this.btnFindRevisions.Location = new System.Drawing.Point(489, 26);
            this.btnFindRevisions.Name = "btnFindRevisions";
            this.btnFindRevisions.Size = new System.Drawing.Size(138, 22);
            this.btnFindRevisions.TabIndex = 4;
            this.btnFindRevisions.Text = "Find Revisions";
            this.btnFindRevisions.UseVisualStyleBackColor = true;
            this.btnFindRevisions.Click += new System.EventHandler(this.btnFindRevisions_Click);
            // 
            // lblModifiedSince
            // 
            this.lblModifiedSince.AutoSize = true;
            this.lblModifiedSince.Location = new System.Drawing.Point(15, 32);
            this.lblModifiedSince.Name = "lblModifiedSince";
            this.lblModifiedSince.Size = new System.Drawing.Size(104, 13);
            this.lblModifiedSince.TabIndex = 3;
            this.lblModifiedSince.Text = "Files Modified Since:";
            // 
            // dateModifiedSince
            // 
            this.dateModifiedSince.Location = new System.Drawing.Point(125, 29);
            this.dateModifiedSince.Name = "dateModifiedSince";
            this.dateModifiedSince.Size = new System.Drawing.Size(230, 20);
            this.dateModifiedSince.TabIndex = 2;
            this.dateModifiedSince.ValueChanged += new System.EventHandler(this.dateModifiedSince_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstResults);
            this.groupBox2.Location = new System.Drawing.Point(12, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(646, 381);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Step 2";
            // 
            // lstResults
            // 
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(18, 30);
            this.lstResults.Name = "lstResults";
            this.lstResults.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstResults.Size = new System.Drawing.Size(609, 342);
            this.lstResults.TabIndex = 0;
            // 
            // timeModifiedSince
            // 
            this.timeModifiedSince.CustomFormat = "";
            this.timeModifiedSince.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timeModifiedSince.Location = new System.Drawing.Point(361, 28);
            this.timeModifiedSince.Name = "timeModifiedSince";
            this.timeModifiedSince.ShowUpDown = true;
            this.timeModifiedSince.Size = new System.Drawing.Size(122, 20);
            this.timeModifiedSince.TabIndex = 5;
            this.timeModifiedSince.ValueChanged += new System.EventHandler(this.timeModifiedSince_ValueChanged);
            // 
            // btnRevertSelected
            // 
            this.btnRevertSelected.Location = new System.Drawing.Point(673, 104);
            this.btnRevertSelected.Name = "btnRevertSelected";
            this.btnRevertSelected.Size = new System.Drawing.Size(194, 66);
            this.btnRevertSelected.TabIndex = 4;
            this.btnRevertSelected.Text = "Revert Selected";
            this.btnRevertSelected.UseVisualStyleBackColor = true;
            this.btnRevertSelected.Click += new System.EventHandler(this.btnRevertSelected_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 489);
            this.Controls.Add(this.btnRevertSelected);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblModifiedSince;
        private System.Windows.Forms.DateTimePicker dateModifiedSince;
        private System.Windows.Forms.Button btnFindRevisions;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.DateTimePicker timeModifiedSince;
        private System.Windows.Forms.Button btnRevertSelected;
    }
}

