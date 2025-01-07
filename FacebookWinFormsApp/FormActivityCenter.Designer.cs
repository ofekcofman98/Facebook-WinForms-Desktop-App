namespace BasicFacebookFeatures
{
    partial class FormActivityCenter
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
            this.listBoxFilteredPosts = new System.Windows.Forms.ListBox();
            this.labelDateOfPosts = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.listBoxHour = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxHourSort = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.listBoxMonth = new System.Windows.Forms.ListBox();
            this.listBoxYear = new System.Windows.Forms.ListBox();
            this.comboBoxYearSort = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxMonthSort = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.labelActivityCenterDiscription = new System.Windows.Forms.Label();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxFilteredPosts
            // 
            this.listBoxFilteredPosts.FormattingEnabled = true;
            this.listBoxFilteredPosts.ItemHeight = 16;
            this.listBoxFilteredPosts.Location = new System.Drawing.Point(30, 127);
            this.listBoxFilteredPosts.Name = "listBoxFilteredPosts";
            this.listBoxFilteredPosts.Size = new System.Drawing.Size(227, 196);
            this.listBoxFilteredPosts.TabIndex = 4;
            // 
            // labelDateOfPosts
            // 
            this.labelDateOfPosts.AutoSize = true;
            this.labelDateOfPosts.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDateOfPosts.Location = new System.Drawing.Point(25, 78);
            this.labelDateOfPosts.Name = "labelDateOfPosts";
            this.labelDateOfPosts.Size = new System.Drawing.Size(66, 29);
            this.labelDateOfPosts.TabIndex = 76;
            this.labelDateOfPosts.Text = "Date";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel4.Controls.Add(this.labelDateOfPosts);
            this.panel4.Controls.Add(this.listBoxFilteredPosts);
            this.panel4.Location = new System.Drawing.Point(689, 170);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(444, 425);
            this.panel4.TabIndex = 87;
            // 
            // listBoxHour
            // 
            this.listBoxHour.FormattingEnabled = true;
            this.listBoxHour.ItemHeight = 16;
            this.listBoxHour.Location = new System.Drawing.Point(55, 78);
            this.listBoxHour.Name = "listBoxHour";
            this.listBoxHour.Size = new System.Drawing.Size(224, 260);
            this.listBoxHour.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(37, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 16);
            this.label10.TabIndex = 77;
            this.label10.Text = "Hours in day";
            // 
            // comboBoxHourSort
            // 
            this.comboBoxHourSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHourSort.FormattingEnabled = true;
            this.comboBoxHourSort.Location = new System.Drawing.Point(159, 33);
            this.comboBoxHourSort.Name = "comboBoxHourSort";
            this.comboBoxHourSort.Size = new System.Drawing.Size(172, 24);
            this.comboBoxHourSort.TabIndex = 80;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel3.Controls.Add(this.comboBoxHourSort);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.listBoxHour);
            this.panel3.Location = new System.Drawing.Point(321, 170);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(344, 425);
            this.panel3.TabIndex = 86;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(33, 239);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 16);
            this.label13.TabIndex = 75;
            this.label13.Text = "Months";
            // 
            // listBoxMonth
            // 
            this.listBoxMonth.FormattingEnabled = true;
            this.listBoxMonth.ItemHeight = 16;
            this.listBoxMonth.Location = new System.Drawing.Point(37, 284);
            this.listBoxMonth.Name = "listBoxMonth";
            this.listBoxMonth.Size = new System.Drawing.Size(237, 68);
            this.listBoxMonth.TabIndex = 2;
            // 
            // listBoxYear
            // 
            this.listBoxYear.FormattingEnabled = true;
            this.listBoxYear.ItemHeight = 16;
            this.listBoxYear.Location = new System.Drawing.Point(38, 78);
            this.listBoxYear.Name = "listBoxYear";
            this.listBoxYear.Size = new System.Drawing.Size(236, 68);
            this.listBoxYear.TabIndex = 1;
            // 
            // comboBoxYearSort
            // 
            this.comboBoxYearSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxYearSort.FormattingEnabled = true;
            this.comboBoxYearSort.Location = new System.Drawing.Point(103, 33);
            this.comboBoxYearSort.Name = "comboBoxYearSort";
            this.comboBoxYearSort.Size = new System.Drawing.Size(171, 24);
            this.comboBoxYearSort.TabIndex = 78;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(34, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 16);
            this.label11.TabIndex = 5;
            this.label11.Text = "Years";
            // 
            // comboBoxMonthSort
            // 
            this.comboBoxMonthSort.AllowDrop = true;
            this.comboBoxMonthSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonthSort.FormattingEnabled = true;
            this.comboBoxMonthSort.Location = new System.Drawing.Point(111, 239);
            this.comboBoxMonthSort.Name = "comboBoxMonthSort";
            this.comboBoxMonthSort.Size = new System.Drawing.Size(163, 24);
            this.comboBoxMonthSort.TabIndex = 79;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Controls.Add(this.comboBoxMonthSort);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.comboBoxYearSort);
            this.panel2.Controls.Add(this.listBoxYear);
            this.panel2.Controls.Add(this.listBoxMonth);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Location = new System.Drawing.Point(5, 170);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(289, 425);
            this.panel2.TabIndex = 85;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label12.Location = new System.Drawing.Point(442, 84);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(291, 48);
            this.label12.TabIndex = 84;
            this.label12.Text = "Activity Center";
            // 
            // labelActivityCenterDiscription
            // 
            this.labelActivityCenterDiscription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelActivityCenterDiscription.AutoSize = true;
            this.labelActivityCenterDiscription.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelActivityCenterDiscription.Location = new System.Drawing.Point(330, 132);
            this.labelActivityCenterDiscription.Name = "labelActivityCenterDiscription";
            this.labelActivityCenterDiscription.Size = new System.Drawing.Size(514, 26);
            this.labelActivityCenterDiscription.TabIndex = 88;
            this.labelActivityCenterDiscription.Text = "discover your most active times for revisit memories";
            // 
            // FormActivityCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 691);
            this.Controls.Add(this.labelActivityCenterDiscription);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Name = "FormActivityCenter";
            this.Text = "FormActivityCenter";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxFilteredPosts;
        private System.Windows.Forms.Label labelDateOfPosts;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListBox listBoxHour;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxHourSort;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListBox listBoxMonth;
        private System.Windows.Forms.ListBox listBoxYear;
        private System.Windows.Forms.ComboBox comboBoxYearSort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxMonthSort;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelActivityCenterDiscription;
    }
}