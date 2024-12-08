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
        private FindFriends m_FindFriends=new FindFriends();

        private Album m_currentAlbum = null;
        private int m_albumPictureCounter = 0;
        
        public FormMain()
        {
            InitializeComponent();
            r_AppManager = new AppManager();
            FacebookWrapper.FacebookService.s_CollectionLimit = 25; // ????????????????????????
            tabsController.TabPages.Remove(tabMyProfile); ////
            tabsController.TabPages.Remove(tabStats);
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
            OnLogin += fetchFriendsLookupPage;

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
            labelUserName.Visible = true;
            pictureBoxProfile.Visible = true;
            labelUserName.Text = $"Hello, {r_AppManager.LoggedInUser.FirstName}!";
            pictureBoxProfile.ImageLocation = r_AppManager.LoggedInUser.PictureNormalURL;

        }

        private void fetchNewsFeed()
        {
            listBoxNewsFeed.Items.Clear();
            foreach (Post post in r_AppManager.LoggedInUser.Posts)
            {
                if (post.Message != null)
                {
                    listBoxNewsFeed.Items.Add(post);
                }
                else if (post.Caption != null)
                {
                    listBoxNewsFeed.Items.Add(post);
                }
                else
                {
                    listBoxNewsFeed.Items.Add(string.Format("[{0}]", post.Type));
                }
            }

            if (listBoxNewsFeed.Items.Count == 0)
            {
                MessageBox.Show("No news to retrieve :(");
            }

        }

        private void fetchFriendList()
        {
            listBoxUserFriends.Visible = true;
            listBoxUserFriends.Items.Clear();
            listBoxUserFriends.DisplayMember = "Name";
            foreach (User User in r_AppManager.LoggedInUser.Friends)
            {
                listBoxUserFriends.Items.Add(User);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (listBoxUserFriends.Items.Count == 0)
            {
                MessageBox.Show("No friends to retrieve :(");
            }
            
        }
        private void fetchMyProfile()
        {
            tabsController.TabPages.Add(tabMyProfile);
            labelEmailData.Text = r_AppManager.LoggedInUser.Email;
            labelBirthdayData.Text = r_AppManager.LoggedInUser.Birthday;
            labelGenderData.Text = r_AppManager.LoggedInUser.Gender.ToString();
            labelFullNameData.Text = r_AppManager.LoggedInUser.Name;
            MyProfilePictureBox.Image = r_AppManager.LoggedInUser.ImageLarge;

        }
        private void fetchStats()
        {
            tabsController.TabPages.Add(tabStats);
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
            listBoxUserAlbums.Items.Clear();
            listBoxUserAlbums.DisplayMember = "Name";
            foreach (Album album in r_AppManager.LoggedInUser.Albums)
            {
                listBoxUserAlbums.Items.Add(album);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (listBoxUserAlbums.Items.Count == 0)
            {
                MessageBox.Show("No Albums to retrieve :(");
            }
        }
        private void fetchFavoriteTeams()
        {
            listBoxUserFavoriteTeams.Items.Clear();
            listBoxUserFavoriteTeams.DisplayMember = "Name";
            foreach (Page page in r_AppManager.LoggedInUser.FavofriteTeams)
            {
                listBoxUserFavoriteTeams.Items.Add(page);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (listBoxUserFavoriteTeams.Items.Count == 0)
            {
                MessageBox.Show("No Albums to retrieve :(");
            }
        }
        private void fetchGroups()
        {
            listBoxUserGroups.Items.Clear();
            listBoxUserGroups.DisplayMember = "Name";
            foreach (Group group in r_AppManager.LoggedInUser.Groups)
            {
                listBoxUserGroups.Items.Add(group);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (listBoxUserGroups.Items.Count == 0)
            {
                MessageBox.Show("No Groups to retrieve :(");
            }
        }

        private void unLaunchFacebook()
        {
            labelUserName.Visible = false;
            pictureBoxProfile.Visible = false;
        }

        private void fetchLikedPages()

        {
            listBoxLikes.Visible = true;
            listBoxLikes.Items.Clear(); // Assuming you have a ListBox to display likes
            listBoxLikes.DisplayMember = "Name";

            try
            {
                if (r_AppManager.LoggedInUser.LikedPages != null && r_AppManager.LoggedInUser.LikedPages.Count > 0)
                {
                    foreach (Page likedPage in r_AppManager.LoggedInUser.LikedPages)
                    {
                        listBoxLikes.Items.Add(likedPage);
                    }
                }
                else
                {
                    listBoxLikes.Items.Add("No liked pages to display.");
                }
            }
            catch (Exception ex)
            {
                listBoxLikes.Items.Add("Couldn't fetch liked pages.");
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
            if (listBoxUserFavoriteTeams.SelectedItems.Count == 1)
            {
                Page selectedTeam = listBoxUserFavoriteTeams.SelectedItem as Page;
                pictureBoxFavoriteTeam.LoadAsync(selectedTeam.PictureNormalURL);
            }
        }

        private void userFriendsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxUserFriends.SelectedItems.Count == 1)
            {
                User selectedUser = listBoxUserFriends.SelectedItem as User;
                pictureBoxUserFriend.LoadAsync(selectedUser.PictureNormalURL);
            }

        }

        private void nextAlbumPictureButton_Click(object sender, EventArgs e)
        {
            if (m_albumPictureCounter +1  <= m_currentAlbum.Count)
            {
                m_albumPictureCounter++;
                pictureBoxalbumPicture.LoadAsync(getCurrentAlbumPictureUrl());
            }

        }

        private void prevAlbumPictureButton_Click(object sender, EventArgs e)
        {
            if (m_albumPictureCounter - 1 >= 0)
            {
                m_albumPictureCounter--;
                pictureBoxalbumPicture.LoadAsync(getCurrentAlbumPictureUrl());
            }

        }

        private void userAlbumsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxUserAlbums.SelectedItems.Count == 1)
            {
                m_albumPictureCounter = 0;
                m_currentAlbum = listBoxUserAlbums.SelectedItem as Album;
                pictureBoxalbumPicture.LoadAsync(getCurrentAlbumPictureUrl());
            }

        }
        private String getCurrentAlbumPictureUrl()
        {
            return m_currentAlbum.Photos[m_albumPictureCounter].PictureNormalURL;
        }

        private void populateRealitionshipStatusList()
        {
            checkedListBoxRealitionshipStatus.Visible = true;
            checkedListBoxRealitionshipStatus.Items.Clear();

            // Loop through each value in the enum and add it to the CheckedListBox
            foreach (User.eRelationshipStatus relationshipStatus in Enum.GetValues(typeof(User.eRelationshipStatus)))
            {
                checkedListBoxRealitionshipStatus.Items.Add(relationshipStatus);
            }

        }
        private void populateLikedPagesList()
        {
            checkedListBoxlikedPages.Items.Clear();
            checkedListBoxlikedPages.DisplayMember = "Name";
            foreach (Page page in r_AppManager.LoggedInUser.LikedPages)
            {
                checkedListBoxlikedPages.Items.Add(page);
                //album.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            if (checkedListBoxlikedPages.Items.Count == 0)
            {
                MessageBox.Show("No liked pages to retrieve :(");
            }

        }
        private void fetchFriendsLookupPage()
        {
            fetchFriendsComboBox();
            populateRealitionshipStatusList();
            populateLikedPagesList();
            PopulateGenderComboBox();

        }
        private void PopulateGenderComboBox()
        {
            // Clear the ComboBox before adding items
            comboBoxGender.Items.Clear();
            comboBoxGender.SelectedItem = null;
            comboBoxGender.SelectedIndex = -1;
            comboBoxGender.Text = "";
            // Add "No Preference" as the first option


            // Add all enum values to the ComboBox
            foreach (User.eGender gender in Enum.GetValues(typeof(User.eGender)))
            {
                comboBoxGender.Items.Add(gender);
            }
            comboBoxGender.Items.Add("No Preference");

            // Set the default selected item to "No Preference"
            comboBoxGender.SelectedIndex = 0;
        }
        private void fetchFriendsComboBox()
        {
            comboBoxFriendList.Items.Clear();
            comboBoxFriendList.SelectedItem = null;
            comboBoxFriendList.SelectedIndex = -1;
            comboBoxFriendList.Text = "";
            comboBoxFriendList.DisplayMember = "Name";


            foreach (User user in r_AppManager.LoggedInUser.Friends)
            {
                comboBoxFriendList.Items.Add(user);
            }

            comboBoxFriendList.SelectedIndex = 0;
        }
        private HashSet<User.eRelationshipStatus> getUserSelectedRelationshipStatuses()
        {
            HashSet<User.eRelationshipStatus> selectedStatuses = new HashSet<User.eRelationshipStatus>();

            foreach (User.eRelationshipStatus item in checkedListBoxRealitionshipStatus.CheckedItems)
            {
                if (item is User.eRelationshipStatus status)
                {
                    selectedStatuses.Add(status);
                }
            }

            return selectedStatuses;
        }
        private HashSet<String> getUserSelectedLikedPagesId()
        {
            HashSet<String> selectedLikedPages = new HashSet<String>();
            
            foreach (Page item in checkedListBoxlikedPages.CheckedItems)
            {
                if (item is Page page)
                {
                    selectedLikedPages.Add(page.Id);
                }
            }

            return selectedLikedPages;
        }

        private void buttonApplySearch_Click(object sender, EventArgs e)
        {
            List<Filterable> filterable = new List<Filterable>();
            if (comboBoxFriendList.SelectedItem == null)
            {//show alart 
                return;
            }
            User selectedFriend = comboBoxFriendList.SelectedItem as User;
            int minAge = (int)numericUpDownMinimumAge.Value;
            int maxAge = (int)numericUpDownMaximumAge.Value;
            if (minAge > maxAge)
            {
                //show alart...
            }else
             {
                filterable.Add(new FilterAge(minAge, maxAge));
             }
            if (comboBoxGender.SelectedItem != null && !comboBoxGender.SelectedItem.Equals("No Preference")) {

                filterable.Add(new FilterGender((User.eGender)comboBoxGender.SelectedItem));
            }
            filterable.Add(new FilterRelationshipStatus(getUserSelectedRelationshipStatuses()));
            filterable.Add(new FilterLikedPages(getUserSelectedLikedPagesId()));
            
            HashSet<User> filterdFriends = m_FindFriends.getFriendUserCommmonFriendsPages(filterable,selectedFriend);
            updateFilterdFriendsComboBox(filterdFriends);
        }
        private void updateFilterdFriendsComboBox(HashSet<User> i_filterdFriends)
        {
            comboBoxFilterdUsers.Items.Clear();
            comboBoxFilterdUsers.SelectedItem = null;
            comboBoxFilterdUsers.SelectedIndex = -1;
            comboBoxFilterdUsers.Text = "";
            if (i_filterdFriends.Count > 0)
            {
                comboBoxFilterdUsers.DisplayMember = "Name";


                foreach (User user in i_filterdFriends)
                {
                    comboBoxFilterdUsers.Items.Add(user);
                }

                comboBoxFilterdUsers.SelectedIndex = 0;

            }

        }

        private void numericUpDownMinimumAge_ValueChanged(object sender, EventArgs e)
        {
            // Ensure minimum age is at least 18
            if (numericUpDownMinimumAge.Value < 18)
            {
                numericUpDownMinimumAge.Value = 18;
            }

            // Ensure the maximum age is greater than or equal to minimum age
            if (numericUpDownMaximumAge.Value < numericUpDownMinimumAge.Value)
            {
                numericUpDownMaximumAge.Value = numericUpDownMinimumAge.Value;
            }
        }

        private void numericUpDownMaximumAge_ValueChanged(object sender, EventArgs e)
        {
            // Ensure maximum age is at least 18
            if (numericUpDownMaximumAge.Value < 18)
            {
                numericUpDownMaximumAge.Value = 18;
            }

            // Ensure the maximum age is greater than or equal to minimum age
            if (numericUpDownMaximumAge.Value < numericUpDownMinimumAge.Value)
            {
                numericUpDownMaximumAge.Value = numericUpDownMinimumAge.Value;
            }
        }

        private void comboBoxFriendList_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBoxSelectedFriendToFilter.Image = (comboBoxFriendList.SelectedItem as User).ImageNormal;

        }

        private void buttonSendFriendRequest_Click(object sender, EventArgs e)
        {

        }

        private void buttonSetStatusPost_Click(object sender, EventArgs e)
        {
        }
    }
}
