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
using static System.Windows.Forms.AxHost;
using static BasicFacebookFeatures.ActivityCenter;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private readonly AppManager r_AppManager;
        private Action OnLogin;
        private Action OnLogout;

        private FacebookWrapper.ObjectModel.Album m_currentAlbum = null;
        private int m_albumPictureCounter = 0;
        private FindFriends m_FindFriends = new FindFriends();
        private int m_FilteredPhotoIndex = -1;

        private readonly string[] r_Months =
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };

        private readonly List<Panel> m_HomePanels;
        private readonly List<TabPage> m_AddedTabs;

        public FormMain()
        {
            InitializeComponent();
            r_AppManager = new AppManager();
            //FacebookWrapper.FacebookService.s_CollectionLimit = 25;
            m_HomePanels = new List<Panel>
                           {
                               panelAlbums,
                               panelStatusPost,
                               panelFavoriteTeams,
                               panelFriends,
                               panelLikes,
                               panelGroups
                           };
            m_AddedTabs = new List<TabPage> { tabMyProfile, tabActivityCenter, tabPage1 };
            updateTabs();
            OnLogin += fetchProfileInfo;
            OnLogin += fetchLikedPages;
            OnLogin += fetchAlbums;
            OnLogin += fetchFriendList;
            OnLogin += fetchMyProfile;
            OnLogin += fetchActivityCenter;
            OnLogin += fetchFriendsLookupPage;
            OnLogin += fetchGroups;
            OnLogin += fetchFavoriteTeams;
            OnLogin += fetchStatusPost;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText("design.patterns");

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
                updateLoginButton();
                updateHomePanelsVisible();
                updateTabs();
                OnLogin?.Invoke();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateHomePanelsVisible()
        {
            bool i_IsVisibile;
            if(r_AppManager.IsLoggedIn)
            {
                i_IsVisibile = true;
            }
            else
            {
                i_IsVisibile = false;
            }

            foreach(Panel panel in m_HomePanels)
            {
                panel.Visible = i_IsVisibile;
            }
        }
        private void updateTabs()
        {
            tabsController.SelectedIndex = 0;
            if (r_AppManager.IsLoggedIn)
            {
                foreach (TabPage tab in m_AddedTabs)
                {
                    tabsController.TabPages.Add(tab);
                }
            }
            else
            {
                foreach (TabPage tab in m_AddedTabs)
                {
                    tabsController.TabPages.Remove(tab);
                }
            }
        }

        private void updateLoginButton()
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



        private void fetchFriendsLookupPage()
        {
            fetchFriendsComboBox();
            populateRealitionshipStatusList();
            populateLikedPagesList();
            PopulateGenderComboBox();

        }

        private void PopulateGenderComboBox()
        {
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

        private void populateRealitionshipStatusList()
        {
            checkedListBoxRealitionshipStatus.Visible = true;
            checkedListBoxRealitionshipStatus.Items.Clear();

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


        private void fetchFriendList()
        {
            panelFriends.Visible = true;
            listBoxUserFriends.Items.Clear();
            listBoxUserFriends.DisplayMember = "Name";
            foreach (User User in r_AppManager.LoggedInUser.Friends)
            {
                listBoxUserFriends.Items.Add(User);
            }

            if (listBoxUserFriends.Items.Count == 0)
            {
                MessageBox.Show("No friends to retrieve :(");
            }
            
        }
        private void fetchMyProfile()
        {
            labelEmailData.Text = r_AppManager.LoggedInUser.Email;
            labelBirthdayData.Text = r_AppManager.LoggedInUser.Birthday;
            labelGenderData.Text = r_AppManager.LoggedInUser.Gender.ToString();
            labelFullNameData.Text = r_AppManager.LoggedInUser.Name;
            PictureBoxMyProfile.Image = r_AppManager.LoggedInUser.ImageLarge;

        }
        private void fetchActivityCenter()
        {
            listBoxFilteredPosts.Visible = true;
            listBoxFilteredPosts.Items.Clear();

            listBoxYear.Visible = true;
            listBoxMonth.Visible = true;
            listBoxHour.Visible = true;

            displayYearCounts();
            displayHoursCounts();
        }

        private void displayYearCounts(string i_SortBy = "CountDescending")
        {
            listBoxYear.Items.Clear();
            List<KeyValuePair<int, int>> yearCounts = r_AppManager.ActivityCenter.GetYearCounts(i_SortBy);
            foreach (KeyValuePair<int, int> year in yearCounts)
            {
                listBoxYear.Items.Add($"{year.Key}: {year.Value} posts/photos");
            }

            if(listBoxYear.Items.Count > 0)
            {
                populateSortComboBox(comboBoxYearSort, "Year");
            }
        }

        private void displayMonthCounts(int i_SelectedYear, string i_SortBy = "CountDescending")
        {
            listBoxMonth.Items.Clear();
            List<KeyValuePair<int, int>> monthCounts = r_AppManager.ActivityCenter.GetMonthCounts(i_SelectedYear, i_SortBy);
            listBoxMonth.Items.Clear();

            foreach (KeyValuePair<int, int> month in monthCounts)
            {
                listBoxMonth.Items.Add($"{r_Months[month.Key - 1]}: {month.Value} posts/photos");
            }

            if(listBoxMonth.Items.Count > 0)
            {
                populateSortComboBox(comboBoxMonthSort, "Month");
            }
        }

        private void displayHoursCounts(string i_SortBy = "CountDescending")
        {
            listBoxHour.Items.Clear();
            List<KeyValuePair<int, int>> hoursCounts = r_AppManager.ActivityCenter.GetHourCounts(i_SortBy);
            foreach (KeyValuePair<int, int> hour in hoursCounts)
            {
                listBoxHour.Items.Add($"{makeHourFormat(hour.Key)}: {hour.Value} posts/photos");
            }

            if(listBoxHour.Items.Count > 0)
            {
                populateSortComboBox(comboBoxHourSort, "Hour");
            }
        }

        private void populateSortComboBox(ComboBox i_ComboBox, string i_TimeType)
        {
            i_ComboBox.Items.Clear();
            i_ComboBox.Items.Add($"Sort by {i_TimeType} Ascending");
            i_ComboBox.Items.Add($"Sort by {i_TimeType} Descending");
            i_ComboBox.Items.Add("Sort by Count Ascending");
            i_ComboBox.Items.Add("Sort by Count Descending");
        }

        private string getSorting(int i_ComboBoxIndex)
        {
            string sorting;
            switch (i_ComboBoxIndex)
            {
                case 0:
                    sorting = "TimeAscending";
                    break;
                case 1:
                    sorting = "TimeDescending";
                    break;
                case 2:
                    sorting = "CountAscending";
                    break;
                case 3:
                    sorting = "CountDescending";
                    break;
                default:
                    sorting = "CountDescending";
                    break;
            }

            return sorting;
        }


        private void comboBoxYearSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxMonth.Items.Clear();
            comboBoxMonthSort.Items.Clear();
            string sorting = getSorting(comboBoxYearSort.SelectedIndex);
            displayYearCounts(sorting);
        }

        private void comboBoxMonthSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedYear = int.Parse(listBoxYear.SelectedItem.ToString().Split(':')[0]);
            string sorting = getSorting(comboBoxMonthSort.SelectedIndex);
            displayMonthCounts(selectedYear, sorting);
        }

        private void comboBoxHourSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sorting = getSorting(comboBoxHourSort.SelectedIndex);
            displayHoursCounts(sorting);
        }


        private void listBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
           if(listBoxYear.SelectedItem != null)
           {
               int selectedYear = int.Parse(listBoxYear.SelectedItem.ToString().Split(':')[0]);
               labelDateOfPosts.Text = $"Your posts from {selectedYear}";
               filterPostsByTime(i_Year: selectedYear);

               displayMonthCounts(selectedYear);
           }
        }


        private void listBoxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxYear.SelectedItem != null && listBoxMonth.SelectedItem != null)
            {
                int selectedYear = int.Parse(listBoxYear.SelectedItem.ToString().Split(':')[0]);
                string selectedMonthName = listBoxMonth.SelectedItem.ToString().Split(':')[0];
                int selectedMonth = Array.IndexOf(r_Months, selectedMonthName) + 1; ;

                labelDateOfPosts.Text = $"Your posts from {selectedMonthName} {selectedYear}";
                filterPostsByTime(i_Year: selectedYear, i_Month: selectedMonth);
            }
        }

        private void listBoxHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBoxHour.SelectedItem != null)
            {
                int selectedHour = int.Parse(listBoxHour.SelectedItem.ToString().Split(':')[0]);
                labelDateOfPosts.Text = $"Your posts from {makeHourFormat(selectedHour)}: ";

                filterPostsByTime(i_Hour: selectedHour);
            }
        }

        private string makeHourFormat(int i_Hour)
        {
            return $"{i_Hour.ToString("D2")}:00";
        }

        private void filterPostsByTime(int? i_Year = null, int? i_Month = null, int? i_Hour = null)
        {
            List<Post> posts = r_AppManager.ActivityCenter.GetPostsByTime(i_Year, i_Month, i_Hour);
            List<Photo> photos = r_AppManager.ActivityCenter.GetPhotosByTime(i_Year, i_Month, i_Hour);

            listBoxFilteredPosts.Items.Clear();
            addPostsToListbox(posts, listBoxFilteredPosts);
            addPhotosToListbox(photos, listBoxFilteredPosts);

            albumFilteredPhotos.SetPhotos(photos);
        }


        private void addPostsToListbox(List<Post> i_Posts, ListBox i_ListBox)
        {
            foreach (Post post in i_Posts)
            {
                if (post.Message == null)
                {
                    i_ListBox.Items.Add("[No message]");
                }
                else
                {
                    i_ListBox.Items.Add(post.Message);
                }
            }
        }

        private void addPhotosToListbox(List<Photo> i_Photos, ListBox i_ListBox)
        {
            foreach (Photo photo in i_Photos)
            {
                if (photo.Name == null)
                {
                    i_ListBox.Items.Add("[No title]");
                }
                else
                {
                    i_ListBox.Items.Add(photo.Message);
                }
            }
        }

        private void fetchStatusPost()
        {
            panelStatusPost.Visible = true;
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
            panelAlbums.Visible = true;
            listBoxUserAlbums.Items.Clear();
            listBoxUserAlbums.DisplayMember = "Name";
            foreach (FacebookWrapper.ObjectModel.Album album in r_AppManager.LoggedInUser.Albums)
            {
                listBoxUserAlbums.Items.Add(album);
            }

            if (listBoxUserAlbums.Items.Count == 0)
            {
                MessageBox.Show("No Albums to retrieve :(");
            }
        }
        private void fetchFavoriteTeams()
        {
            panelFavoriteTeams.Visible = true;
            listBoxUserFavoriteTeams.Items.Clear();
            listBoxUserFavoriteTeams.DisplayMember = "Name";
            foreach (Page page in r_AppManager.LoggedInUser.FavofriteTeams)
            {
                listBoxUserFavoriteTeams.Items.Add(page);
            }

            if (listBoxUserFavoriteTeams.Items.Count == 0)
            {
                MessageBox.Show("No Albums to retrieve :(");
            }
        }
        private void fetchGroups()
        {
            panelGroups.Visible = true;
            listBoxUserGroups.Items.Clear();
            listBoxUserGroups.DisplayMember = "Name";
            foreach (Group group in r_AppManager.LoggedInUser.Groups)
            {
                listBoxUserGroups.Items.Add(group);
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
            panelLikes.Visible = true;
            listBoxLikes.Items.Clear();
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
            updateHomePanelsVisible();
            updateTabs();
            unLaunchFacebook();
            buttonLogin.BackColor = buttonLogout.BackColor;
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;
            buttonLogin.Text = "Login";
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

        private void userAlbumsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxUserAlbums.SelectedItems.Count == 1)
            {
                m_albumPictureCounter = 0;
                m_currentAlbum = listBoxUserAlbums.SelectedItem as FacebookWrapper.ObjectModel.Album;
                albumUserAlbums.SetPhotos(m_currentAlbum.Photos.ToList());
            }

        }

        private void updateFilteredFriendsListBox(HashSet<User> i_filterdFriends)
        {
            listBoxFilteredUsers.Items.Clear();
            listBoxUserFavoriteTeams.DisplayMember = "Name";
            foreach(User user in i_filterdFriends)
            {
                listBoxFilteredUsers.Items.Add(user);
            }

            if(listBoxFilteredUsers.Items.Count == 0)
            {
                listBoxFilteredUsers.Items.Add("No Users to retrieve");
            }
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
            List<IFilterable> filterable = new List<IFilterable>();
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
            }
            else
            {
                filterable.Add(new FilterAge(minAge, maxAge));
            }
            if (comboBoxGender.SelectedItem != null && !comboBoxGender.SelectedItem.Equals("No Preference"))
            {

                filterable.Add(new FilterGender((User.eGender)comboBoxGender.SelectedItem));
            }
            filterable.Add(new FilterRelationshipStatus(getUserSelectedRelationshipStatuses()));
            filterable.Add(new FilterLikedPages(getUserSelectedLikedPagesId()));

            HashSet<User> filterdFriends = m_FindFriends.getFriendUserCommmonFriendsPages(filterable, selectedFriend);
            updateFilteredFriendsListBox(filterdFriends);
        }

        private void numericUpDownMaximumAge_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownMinimumAge.Value < 18)
            {
                numericUpDownMinimumAge.Value = 18;
            }

            if (numericUpDownMaximumAge.Value < numericUpDownMinimumAge.Value)
            {
                numericUpDownMaximumAge.Value = numericUpDownMinimumAge.Value;
            }

        }

        private void numericUpDownMinimumAge_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownMaximumAge.Value < 18)
            {
                numericUpDownMaximumAge.Value = 18;
            }

            if (numericUpDownMaximumAge.Value < numericUpDownMinimumAge.Value)
            {
                numericUpDownMaximumAge.Value = numericUpDownMinimumAge.Value;
            }

        }

        private void listBoxFilteredUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBoxFilteredUsers.Image = (listBoxFilteredUsers.SelectedItem as User).ImageNormal;
        }

    }
}
