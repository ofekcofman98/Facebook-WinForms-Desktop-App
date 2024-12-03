namespace BasicFacebookFeatures
{
    partial class FormMain
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
            this.profileInfo = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.emailData = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.birthdayData = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.genderData = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.fullNameData = new System.Windows.Forms.Label();
            this.homePageTab = new System.Windows.Forms.TabPage();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonLogout = new System.Windows.Forms.Button();
            this.pictureBoxProfile = new System.Windows.Forms.PictureBox();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.likesListBox = new System.Windows.Forms.ListBox();
            this.userAlbumsListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.userFriendsListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabsController = new System.Windows.Forms.TabControl();
            this.profileInfo.SuspendLayout();
            this.homePageTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).BeginInit();
            this.tabsController.SuspendLayout();
            this.SuspendLayout();
            // 
            // profileInfo
            // 
            this.profileInfo.Controls.Add(this.fullNameData);
            this.profileInfo.Controls.Add(this.label5);
            this.profileInfo.Controls.Add(this.genderData);
            this.profileInfo.Controls.Add(this.label8);
            this.profileInfo.Controls.Add(this.birthdayData);
            this.profileInfo.Controls.Add(this.label6);
            this.profileInfo.Controls.Add(this.emailData);
            this.profileInfo.Controls.Add(this.label4);
            this.profileInfo.Location = new System.Drawing.Point(4, 35);
            this.profileInfo.Name = "profileInfo";
            this.profileInfo.Padding = new System.Windows.Forms.Padding(3);
            this.profileInfo.Size = new System.Drawing.Size(1235, 658);
            this.profileInfo.TabIndex = 1;
            this.profileInfo.Text = "Profile Info";
            this.profileInfo.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 26);
            this.label4.TabIndex = 0;
            this.label4.Text = "Email:";
            // 
            // emailData
            // 
            this.emailData.AutoSize = true;
            this.emailData.Location = new System.Drawing.Point(422, 136);
            this.emailData.Name = "emailData";
            this.emailData.Size = new System.Drawing.Size(70, 26);
            this.emailData.TabIndex = 1;
            this.emailData.Text = "label5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(276, 202);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 26);
            this.label6.TabIndex = 2;
            this.label6.Text = "Birthday:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // birthdayData
            // 
            this.birthdayData.AutoSize = true;
            this.birthdayData.Location = new System.Drawing.Point(422, 202);
            this.birthdayData.Name = "birthdayData";
            this.birthdayData.Size = new System.Drawing.Size(70, 26);
            this.birthdayData.TabIndex = 3;
            this.birthdayData.Text = "label7";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(281, 267);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 26);
            this.label8.TabIndex = 4;
            this.label8.Text = "Gender:";
            // 
            // genderData
            // 
            this.genderData.AutoSize = true;
            this.genderData.Location = new System.Drawing.Point(422, 267);
            this.genderData.Name = "genderData";
            this.genderData.Size = new System.Drawing.Size(0, 26);
            this.genderData.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(271, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 26);
            this.label5.TabIndex = 6;
            this.label5.Text = "Full Name:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // fullNameData
            // 
            this.fullNameData.AutoSize = true;
            this.fullNameData.Location = new System.Drawing.Point(427, 76);
            this.fullNameData.Name = "fullNameData";
            this.fullNameData.Size = new System.Drawing.Size(82, 26);
            this.fullNameData.TabIndex = 7;
            this.fullNameData.Text = "label10";
            this.fullNameData.Click += new System.EventHandler(this.label10_Click);
            // 
            // homePageTab
            // 
            this.homePageTab.Controls.Add(this.label3);
            this.homePageTab.Controls.Add(this.userFriendsListBox);
            this.homePageTab.Controls.Add(this.label2);
            this.homePageTab.Controls.Add(this.label1);
            this.homePageTab.Controls.Add(this.userAlbumsListBox);
            this.homePageTab.Controls.Add(this.likesListBox);
            this.homePageTab.Controls.Add(this.userNameLabel);
            this.homePageTab.Controls.Add(this.pictureBoxProfile);
            this.homePageTab.Controls.Add(this.buttonLogout);
            this.homePageTab.Controls.Add(this.buttonLogin);
            this.homePageTab.Location = new System.Drawing.Point(4, 35);
            this.homePageTab.Name = "homePageTab";
            this.homePageTab.Padding = new System.Windows.Forms.Padding(3);
            this.homePageTab.Size = new System.Drawing.Size(1235, 658);
            this.homePageTab.TabIndex = 0;
            this.homePageTab.Text = "Home Page";
            this.homePageTab.UseVisualStyleBackColor = true;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(18, 17);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(268, 32);
            this.buttonLogin.TabIndex = 36;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // buttonLogout
            // 
            this.buttonLogout.Enabled = false;
            this.buttonLogout.Location = new System.Drawing.Point(18, 57);
            this.buttonLogout.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(268, 32);
            this.buttonLogout.TabIndex = 52;
            this.buttonLogout.Text = "Logout";
            this.buttonLogout.UseVisualStyleBackColor = true;
            this.buttonLogout.Click += new System.EventHandler(this.buttonLogout_Click);
            // 
            // pictureBoxProfile
            // 
            this.pictureBoxProfile.Location = new System.Drawing.Point(18, 96);
            this.pictureBoxProfile.Name = "pictureBoxProfile";
            this.pictureBoxProfile.Size = new System.Drawing.Size(79, 78);
            this.pictureBoxProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProfile.TabIndex = 55;
            this.pictureBoxProfile.TabStop = false;
            // 
            // userNameLabel
            // 
            this.userNameLabel.Location = new System.Drawing.Point(14, 223);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(272, 33);
            this.userNameLabel.TabIndex = 56;
            this.userNameLabel.Text = "Hello, ";
            this.userNameLabel.Visible = false;
            // 
            // likesListBox
            // 
            this.likesListBox.FormattingEnabled = true;
            this.likesListBox.ItemHeight = 26;
            this.likesListBox.Location = new System.Drawing.Point(589, 133);
            this.likesListBox.Name = "likesListBox";
            this.likesListBox.Size = new System.Drawing.Size(340, 160);
            this.likesListBox.TabIndex = 57;
            this.likesListBox.Visible = false;
            // 
            // userAlbumsListBox
            // 
            this.userAlbumsListBox.FormattingEnabled = true;
            this.userAlbumsListBox.ItemHeight = 26;
            this.userAlbumsListBox.Location = new System.Drawing.Point(313, 133);
            this.userAlbumsListBox.Name = "userAlbumsListBox";
            this.userAlbumsListBox.Size = new System.Drawing.Size(206, 160);
            this.userAlbumsListBox.TabIndex = 58;
            this.userAlbumsListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(351, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 26);
            this.label1.TabIndex = 59;
            this.label1.Text = "user albums";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(632, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 26);
            this.label2.TabIndex = 60;
            this.label2.Text = "user liked posts";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // userFriendsListBox
            // 
            this.userFriendsListBox.FormattingEnabled = true;
            this.userFriendsListBox.ItemHeight = 26;
            this.userFriendsListBox.Location = new System.Drawing.Point(326, 410);
            this.userFriendsListBox.Name = "userFriendsListBox";
            this.userFriendsListBox.Size = new System.Drawing.Size(156, 134);
            this.userFriendsListBox.TabIndex = 61;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(380, 364);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 26);
            this.label3.TabIndex = 62;
            this.label3.Text = "friends";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // tabsController
            // 
            this.tabsController.Controls.Add(this.homePageTab);
            this.tabsController.Controls.Add(this.profileInfo);
            this.tabsController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsController.Location = new System.Drawing.Point(0, 0);
            this.tabsController.Name = "tabsController";
            this.tabsController.SelectedIndex = 0;
            this.tabsController.Size = new System.Drawing.Size(1243, 697);
            this.tabsController.TabIndex = 54;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1243, 697);
            this.Controls.Add(this.tabsController);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facebook App";
            this.profileInfo.ResumeLayout(false);
            this.profileInfo.PerformLayout();
            this.homePageTab.ResumeLayout(false);
            this.homePageTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).EndInit();
            this.tabsController.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage profileInfo;
        private System.Windows.Forms.Label fullNameData;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label genderData;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label birthdayData;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label emailData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage homePageTab;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox userFriendsListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox userAlbumsListBox;
        private System.Windows.Forms.ListBox likesListBox;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.PictureBox pictureBoxProfile;
        private System.Windows.Forms.Button buttonLogout;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.TabControl tabsController;
    }
}

