﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private AppManager appManager = new AppManager();
        public FormMain()
        {
            InitializeComponent();
            FacebookWrapper.FacebookService.s_CollectionLimit = 25;
        }

        FacebookWrapper.LoginResult m_LoginResult;

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("design.patterns");

            if (m_LoginResult == null)
            {
                login();
            }


            //try
            //{
            //    FacebookService.LogoutWithUI();
            //    m_LoginResult = null; // Reset the login result
            //    buttonLogin.Text = "Login";
            //    buttonLogin.BackColor = buttonLogout.BackColor;
            //    pictureBoxProfile.Image = null; // Clear profile picture
            //    buttonLogin.Enabled = true;
            //    buttonLogout.Enabled = false;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error during logout: {ex.Message}");
            //}

        }
        
        
        private void login()
        {
            string appID = appManager.AppId;// textBoxAppID.Text.Trim();

            if(string.IsNullOrEmpty((appID)))
            {
                MessageBox.Show("Please enter a valid App ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the method if App ID is invalid.
            }

            try
            {
                m_LoginResult = FacebookService.Login(
                    appID,
                    "email",
                    "public_profile");
                

                if (string.IsNullOrEmpty(m_LoginResult.ErrorMessage))
                {
                    buttonLogin.Text = $"Logged in as {m_LoginResult.LoggedInUser.Name}";
                    buttonLogin.BackColor = Color.LightGreen;

                    buttonLogin.Enabled = false;
                    buttonLogout.Enabled = true;

                    launchFacebook();
                }
                else 
                {
                    MessageBox.Show($"Login failed: {m_LoginResult.ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void launchFacebook()
        {
            userNameLabel.Visible = true;
            pictureBoxProfile.Visible = true;
            userNameLabel.Text = $"Hello, {m_LoginResult.LoggedInUser.FirstName}!";
            pictureBoxProfile.ImageLocation = m_LoginResult.LoggedInUser.PictureNormalURL;
        }

        private void unLaunchFacebook()
        {
            userNameLabel.Visible = false;
            pictureBoxProfile.Visible = false;
        }



        private void buttonLogout_Click(object sender, EventArgs e)
        {
            FacebookService.LogoutWithUI();
            m_LoginResult = null;
            buttonLogin.Text = "Login";
            buttonLogin.BackColor = buttonLogout.BackColor;
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;

            unLaunchFacebook();

        }
    }
}
