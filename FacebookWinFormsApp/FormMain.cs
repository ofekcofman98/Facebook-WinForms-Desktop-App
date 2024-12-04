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
using System.Windows.Forms.VisualStyles;
using CefSharp.DevTools.Fetch;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private readonly AppManager r_AppManager;
        private Action OnLogin;

        private Album m_currentAlbum = null;
        private int m_albumPictureCounter = 0;
        
        public FormMain()
        {
            InitializeComponent();
            r_AppManager = new AppManager();
            FacebookWrapper.FacebookService.s_CollectionLimit = 25; // ????????????????????????
            tabsController.TabPages.Remove(MyProfileTab); ////
            tabsController.TabPages.Remove(StatsTab);
            OnLogin += fetchProfileInfo;
            OnLogin += fetchLikedPages;
            OnLogin += fetchNewsFeed;
            OnLogin += fetchAlbums;
            OnLogin += fetchFriendList;
            OnLogin += fetchMyProfile;
            OnLogin += fetchStats;
            OnLogin += fetchGroups;
            OnLogin += fetchFavoriteTeams;
            OnLogin += fetchStatusPost;

        }


        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("design.patterns");

            if (r_AppManager.LoginResult == null)
            {
                performLogin();
            }

        }
        
        private void performLogin()
        {
            try
            {
                r_AppManager.Login();
                UpdateLoginButton();
                //launchFacebook();
                OnLogin?.Invoke();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateLoginButton()
        {
            buttonLogin.Text = $"Logged in as {r_AppManager.LoggedInUser.Name}";
            buttonLogin.BackColor = Color.LightGreen;
            buttonLogin.Enabled = false;
            buttonLogout.Enabled = true;
        }



        private void fetchProfileInfo()
        {
            userNameLabel.Visible = true;
            pictureBoxProfile.Visible = true;
            userNameLabel.Text = $"Hello, {r_AppManager.LoggedInUser.FirstName}!";
            pictureBoxProfile.ImageLocation = r_AppManager.LoggedInUser.PictureNormalURL;

        }

        private void fetchNewsFeed()
        {
            newsFeedListBox.Items.Clear();
            foreach (Post post in r_AppManager.LoggedInUser.Posts)
            {
                if (post.Message != null)
                {
                    newsFeedListBox.Items.Add(post);
                }
                else if (post.Caption != null)
                {
                    newsFeedListBox.Items.Add(post);
                }
                else
                {
                    newsFeedListBox.Items.Add(string.Format("[{0}]", post.Type));
                }
            }

            if (newsFeedListBox.Items.Count == 0)
            {
                MessageBox.Show("No news to retrieve :(");
            }

        }

        private void fetchFriendList()
        {
            userFriendsListBox.Visible = true;
            userFriendsListBox.Items.Clear();
            userFriendsListBox.DisplayMember = "Name";
            foreach (User User in r_AppManager.LoggedInUser.Friends)
            {
                userFriendsListBox.Items.Add(User);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (userFriendsListBox.Items.Count == 0)
            {
                MessageBox.Show("No friends to retrieve :(");
            }
            
        }
        private void fetchMyProfile()
        {
            tabsController.TabPages.Add(MyProfileTab);
            emailData.Text = r_AppManager.LoggedInUser.Email;
            birthdayData.Text = r_AppManager.LoggedInUser.Birthday;
            genderData.Text = r_AppManager.LoggedInUser.Gender.ToString();
            fullNameData.Text = r_AppManager.LoggedInUser.Name;
            ProfilePictureBox.Image = r_AppManager.LoggedInUser.ImageLarge;

        }
        private void fetchStats()
        {
            tabsController.TabPages.Add(StatsTab);
            r_AppManager.StatCenter = new StatCenter(r_AppManager.LoggedInUser.Posts.ToList());
        }

        private void fetchStatusPost()
        {
            textBoxStatusPost.Click += textBoxStatus_Click;
            textBoxStatusPost.Leave += textBoxStatus_Leave;

            if (r_AppManager.LoggedInUser != null)
            {
                textBoxStatusPost.Text = $"What's on your mind, {r_AppManager.LoggedInUser.FirstName}";
                textBoxStatusPost.ForeColor = Color.Gray;
            }
        }

        private void textBoxStatus_Click(object sender, EventArgs e)
        {
            if ( r_AppManager.LoggedInUser != null)
            {
                textBoxStatusPost.Text = ""; 
                textBoxStatusPost.ForeColor = Color.Black;
            }
        }

        private void textBoxStatus_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxStatusPost.Text))
            {
                textBoxStatusPost.Text = $"What's on your mind, {r_AppManager.LoggedInUser.Name}?";
                textBoxStatusPost.ForeColor = Color.Gray; 
            }
        }


        private void fetchAlbums()
        {
            userAlbumsListBox.Items.Clear();
            userAlbumsListBox.DisplayMember = "Name";
            foreach (Album album in r_AppManager.LoggedInUser.Albums)
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
            foreach (Page page in r_AppManager.LoggedInUser.FavofriteTeams)
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
            foreach (Group group in r_AppManager.LoggedInUser.Groups)
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

        private void fetchLikedPages()

        {
            likesListBox.Visible = true;
            likesListBox.Items.Clear(); // Assuming you have a ListBox to display likes
            likesListBox.DisplayMember = "Name";

            try
            {
                if (r_AppManager.LoggedInUser.LikedPages != null && r_AppManager.LoggedInUser.LikedPages.Count > 0)
                {
                    foreach (Page likedPage in r_AppManager.LoggedInUser.LikedPages)
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
            r_AppManager.Logout();
            performLogout();
        }

        private void performLogout()
        {
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
