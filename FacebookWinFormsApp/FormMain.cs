using System;
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
        private readonly AppManager r_AppManager;
        private Album m_currentAlbum = null;
        private int m_albumPictureCounter = 0;

        FacebookWrapper.LoginResult m_LoginResult;
        FacebookWrapper.ObjectModel.User m_LoggedInUser;
        
        public FormMain()
        {
            InitializeComponent();
            r_AppManager = new AppManager();
            FacebookWrapper.FacebookService.s_CollectionLimit = 25;
            tabsController.TabPages.Remove(profileInfo);
        }


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
            string appID = r_AppManager.AppId;// textBoxAppID.Text.Trim();

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
                    "public_profile",
                    "user_age_range",
                    "user_birthday",
                    "user_events",
                    "user_friends",
                    "user_gender",
                    "user_hometown",
                    "user_likes",
                    "user_link",
                    "user_location",
                    "user_photos",
                    "user_posts",
                    "user_videos");


                if (string.IsNullOrEmpty(m_LoginResult.ErrorMessage))
                {
                    m_LoggedInUser = m_LoginResult.LoggedInUser;
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
            likesListBox.Visible = true;
            friendsList();
            fetchAlbums();
            fetchFriendList();
            fetchProfileInfo();
            fetchGroups();
            fetchFavoriteTeams();

        }
       private void fetchFriendList()
        {
            userFriendsListBox.Visible = true;
            userFriendsListBox.Items.Clear();
            userFriendsListBox.DisplayMember = "Name";
            foreach (User User in m_LoggedInUser.Friends)
            {
                userFriendsListBox.Items.Add(User);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (userFriendsListBox.Items.Count == 0)
            {
                MessageBox.Show("No friends to retrieve :(");
            }
            
        }
        private void fetchProfileInfo()
        {
            tabsController.TabPages.Add(profileInfo);
            emailData.Text = m_LoggedInUser.Email;
            birthdayData.Text = m_LoggedInUser.Birthday;
            genderData.Text = m_LoggedInUser.Gender.ToString();
            fullNameData.Text = m_LoggedInUser.Name;

        }
        private void fetchAlbums()
        {
            userAlbumsListBox.Items.Clear();
            userAlbumsListBox.DisplayMember = "Name";
            foreach (Album album in m_LoggedInUser.Albums)
            {
                userAlbumsListBox.Items.Add(album);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (userAlbumsListBox.Items.Count == 0)
            {
                MessageBox.Show("No Albums to retrieve :(");
            }
        }
        private void fetchFavoriteTeams()
        {
            userFavoriteTeamsListBox.Items.Clear();
            userFavoriteTeamsListBox.DisplayMember = "Name";
            foreach (Page page in m_LoggedInUser.FavofriteTeams)
            {
                userFavoriteTeamsListBox.Items.Add(page);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (userFavoriteTeamsListBox.Items.Count == 0)
            {
                MessageBox.Show("No Albums to retrieve :(");
            }
        }
        private void fetchGroups()
        {
            userGroupsListBox.Items.Clear();
            userGroupsListBox.DisplayMember = "Name";
            foreach (Group group in m_LoggedInUser.Groups)
            {
                userGroupsListBox.Items.Add(group);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (userGroupsListBox.Items.Count == 0)
            {
                MessageBox.Show("No Groups to retrieve :(");
            }
        }

        private void unLaunchFacebook()
        {
            userNameLabel.Visible = false;
            pictureBoxProfile.Visible = false;
        }

        private void friendsList()
        {
            likesListBox.Items.Clear(); // Assuming you have a ListBox to display likes
            likesListBox.DisplayMember = "Name";

            try
            {
                if (m_LoginResult.LoggedInUser.LikedPages != null && m_LoginResult.LoggedInUser.LikedPages.Count > 0)
                {
                    foreach (Page likedPage in m_LoginResult.LoggedInUser.LikedPages)
                    {
                        likesListBox.Items.Add(likedPage);
                    }
                }
                else
                {
                    likesListBox.Items.Add("No liked pages to display.");
                }
            }
            catch (Exception ex)
            {
                likesListBox.Items.Add("Couldn't fetch liked pages.");
                MessageBox.Show($"Error: {ex.Message}");
            }


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

        private void userFavoriteTeamsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userFavoriteTeamsListBox.SelectedItems.Count == 1)
            {
                Page selectedTeam = userFavoriteTeamsListBox.SelectedItem as Page;
                favoriteTeamPictureBox.LoadAsync(selectedTeam.PictureNormalURL);
            }
        }

        private void userFriendsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userFriendsListBox.SelectedItems.Count == 1)
            {
                User selectedUser = userFriendsListBox.SelectedItem as User;
                userFriendPictureBox.LoadAsync(selectedUser.PictureNormalURL);
            }

        }

        private void nextAlbumPictureButton_Click(object sender, EventArgs e)
        {
            if (m_albumPictureCounter +1  <= m_currentAlbum.Count)
            {
                m_albumPictureCounter++;
                albumPicture.LoadAsync(getCurrentAlbumPictureUrl());
            }

        }

        private void prevAlbumPictureButton_Click(object sender, EventArgs e)
        {
            if (m_albumPictureCounter - 1 >= 0)
            {
                m_albumPictureCounter--;
                albumPicture.LoadAsync(getCurrentAlbumPictureUrl());
            }

        }

        private void userAlbumsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userAlbumsListBox.SelectedItems.Count == 1)
            {
                m_albumPictureCounter = 0;
                m_currentAlbum = userAlbumsListBox.SelectedItem as Album;
                albumPicture.LoadAsync(getCurrentAlbumPictureUrl());
            }

        }
        private String getCurrentAlbumPictureUrl()
        {
            return m_currentAlbum.Photos[m_albumPictureCounter].PictureNormalURL;
        }
    }
}
