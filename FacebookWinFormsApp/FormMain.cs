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
using Facebook;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private Action onLogin;
        private Action onLogout;

        private readonly ActivityCenterFacade r_ActivityCenterFacade;

        private readonly List<Panel> r_HomePanels;
        private readonly List<TabPage> r_AddedTabs;



        public FormMain()
        {
            InitializeComponent();
            //InitializeTabs();

            FacebookWrapper.FacebookService.s_CollectionLimit = 25;
            r_HomePanels = new List<Panel>
                           {
                               panelAlbums,
                               panelStatusPost,
                               panelFavoriteTeams,
                               panelFriends,
                               panelLikes,
                               panelGroups
                           };
            r_AddedTabs = new List<TabPage> { tabMyProfile, tabActivityCenter, tabFindNewFriends };

            r_ActivityCenterFacade = new ActivityCenterFacade();

            updateTabs(false);
           

            onLogin += updateLoginButton;
            onLogin += updateHomePanelsVisible;
            onLogin += fetchProfileInfo;
            onLogin += fetchLikedPages;
            onLogin += fetchAlbums;
            onLogin += fetchFriendList;
            onLogin += fetchMyProfile;
            onLogin += fetchActivityCenter;
            onLogin += fetchFriendsLookupPage;
            onLogin += fetchGroups;
            onLogin += fetchFavoriteTeams;
            onLogin += fetchStatusPost;

            onLogout += updateHomePanelsVisible;
            onLogout += unLaunchFacebook;
            onLogout += updateLoginButton;
        }


        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (AppManager.Instance.LoginResult == null)
            {
                performLogin();
            }
        }
         
        private void performLogin()
        {
            try
            {
                //r_Facade.Login();
                AppManager.Instance.Login();
                updateTabs(AppManager.Instance.IsLoggedIn);
                onLogin?.Invoke();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            performLogout();
        }

        private void performLogout()
        {
            try
            {
                //r_Facade.Logout();
                AppManager.Instance.Logout();
                updateTabs(AppManager.Instance.IsLoggedIn);
                onLogout?.Invoke();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateHomePanelsVisible()
        {
            bool isVisibile;
            isVisibile = AppManager.Instance.IsLoggedIn;

            foreach(Panel panel in r_HomePanels)
            {
                panel.Visible = isVisibile;
            }
        }

        private void updateTabs(bool i_IsVisible = true)
        {
            tabsController.SelectedIndex = 0;
            if (i_IsVisible)
            {
                foreach (TabPage tab in r_AddedTabs)
                {
                    tabsController.TabPages.Add(tab);
                }
            }
            else
            {
                foreach (TabPage tab in r_AddedTabs)
                {
                    tabsController.TabPages.Remove(tab);
                }
            }
        }

        private void updateLoginButton()
        {
            if (AppManager.Instance.IsLoggedIn)
            {
                buttonLogin.Text = $"Logged in as {AppManager.Instance.LoggedInUser.Name}";
                buttonLogin.BackColor = Color.LightGreen;
                buttonLogin.Enabled = false;
                buttonLogout.Enabled = true;
            }
            else
            {
                buttonLogin.BackColor = buttonLogout.BackColor;
                buttonLogin.Enabled = true;
                buttonLogout.Enabled = false;
                buttonLogin.Text = "Login";
            }
        }

        private void fetchProfileInfo()
        {
            labelUserName.Visible = true;
            pictureBoxProfile.Visible = true;
            labelUserName.Text = $"Hello, {AppManager.Instance.LoggedInUser.FirstName}!";
            pictureBoxProfile.ImageLocation = AppManager.Instance.LoggedInUser.PictureNormalURL;
        }


        private void fetchFriendsLookupPage()
        {
            fetchFriendsComboBox();
            populateRealitionshipStatusList();
            populateLikedPagesList();
            populateGenderComboBox();

        }

        private void populateGenderComboBox()
        {
            comboBoxGender.Items.Clear();
            comboBoxGender.SelectedItem = null;
            comboBoxGender.SelectedIndex = -1;
            comboBoxGender.Text = "";
            foreach (User.eGender gender in Enum.GetValues(typeof(User.eGender)))
            {
                comboBoxGender.Items.Add(gender);
            }
            comboBoxGender.Items.Add("No Preference");
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
            foreach (Page page in AppManager.Instance.LoggedInUser.LikedPages)
            {
                checkedListBoxlikedPages.Items.Add(page);
            }

            if (checkedListBoxlikedPages.Items.Count == 0)
            {
                checkedListBoxlikedPages.Items.Add("No liked pages to retrieve");
            }
        }


        private void fetchFriendsComboBox()
        {
            comboBoxFriendList.Items.Clear();
            comboBoxFriendList.SelectedItem = null;
            comboBoxFriendList.SelectedIndex = -1;
            comboBoxFriendList.Text = "";
            comboBoxFriendList.DisplayMember = "Name";

            foreach (User user in AppManager.Instance.LoggedInUser.Friends)
            {
                comboBoxFriendList.Items.Add(user);
            }

            comboBoxFriendList.SelectedIndex = 0;
        }


        private void fetchFriendList()
        {
            panelFriends.Visible = true;
            listBoxUserFriends.Items.Clear();
            pictureBoxUserFriend.Image = null;
            listBoxUserFriends.DisplayMember = "Name";
            foreach (User user in AppManager.Instance.LoggedInUser.Friends)
            {
                listBoxUserFriends.Items.Add(user);
            }

            if (listBoxUserFriends.Items.Count == 0)
            {
                listBoxUserFriends.Items.Add("No friends to retrieve");
            }
        }

        private void fetchMyProfile()
        {

            //if (AppManager.Instance.LoggedInUser != null)
            //{
            //    m_ProfileControl.LoadProfileData();
            //}

            labelEmailData.Text = AppManager.Instance.LoggedInUser.Email;
            labelBirthdayData.Text = AppManager.Instance.LoggedInUser.Birthday;
            labelGenderData.Text = AppManager.Instance.LoggedInUser.Gender.ToString();
            labelFullNameData.Text = AppManager.Instance.LoggedInUser.Name;
            PictureBoxMyProfile.Image = AppManager.Instance.LoggedInUser.ImageLarge;
        }

        private void fetchActivityCenter()
        {
            listBoxFilteredPosts.Visible = true;
            listBoxFilteredPosts.Items.Clear();

            albumControlFilteredPhotos.Visible = true;
            albumControlFilteredPhotos.ClearPictureBoxInAlbum();

            listBoxYear.Visible = true;
            listBoxMonth.Visible = true;
            listBoxHour.Visible = true;

            displayYearCounts();
            displayHoursCounts();
        }

        private void displayYearCounts(eSortingType i_SortBy = eSortingType.CountDescending) // default?
        {
            listBoxYear.Items.Clear();
            List<KeyValuePair<int, int>> yearCounts = AppManager.Instance.ActivityCenter.GetYearCounts(i_SortBy);
            foreach (KeyValuePair<int, int> year in yearCounts)
            {
                listBoxYear.Items.Add($"{year.Key}: {year.Value} posts/photos");
            }

            if (listBoxYear.Items.Count > 0)
            {
                populateSortComboBox(comboBoxYearSort, "Year");
            }
        }

        private void displayMonthCounts(int i_SelectedYear, eSortingType i_SortBy = eSortingType.CountDescending)
        {
            listBoxMonth.Items.Clear();
            List<KeyValuePair<int, int>> monthCounts = AppManager.Instance.ActivityCenter.GetMonthCounts(i_SelectedYear, i_SortBy);
            listBoxMonth.Items.Clear();

            foreach (KeyValuePair<int, int> month in monthCounts)
            {
                listBoxMonth.Items.Add($"{UserUtils.sr_Months[month.Key - 1]}: {month.Value} posts/photos");
            }

            if (listBoxMonth.Items.Count > 0)
            {
                populateSortComboBox(comboBoxMonthSort, "Month");
            }
        }

        private void displayHoursCounts(eSortingType i_SortBy = eSortingType.CountDescending)
        {
            listBoxHour.Items.Clear();
            List<KeyValuePair<int, int>> hoursCounts = AppManager.Instance.ActivityCenter.GetHourCounts(i_SortBy);
            foreach (KeyValuePair<int, int> hour in hoursCounts)
            {
                listBoxHour.Items.Add($"{makeHourFormat(hour.Key)}: {hour.Value} posts/photos");
            }

            if (listBoxHour.Items.Count > 0)
            {
                populateSortComboBox(comboBoxHourSort, "Hour");
            }
        }

        private void populateSortComboBox(ComboBox i_ComboBox, string i_TimeType)
        {
            i_ComboBox.Items.Clear();

            foreach(KeyValuePair<eSortingType, string> sortingOption in r_ActivityCenterFacade.r_SortingDictionary)
            {
                i_ComboBox.Items.Add(sortingOption.Value);
            }


            //i_ComboBox.Items.Add($"Sort by {i_TimeType} Ascending");
            //i_ComboBox.Items.Add($"Sort by {i_TimeType} Descending");
            //i_ComboBox.Items.Add("Sort by Count Ascending");
            //i_ComboBox.Items.Add("Sort by Count Descending");
        }

        private eSortingType getSorting(int i_ComboBoxIndex)
        {
            eSortingType sorting;
            switch (i_ComboBoxIndex)
            {
                case 0:
                    sorting = eSortingType.TimeAscending;
                    break;
                case 1:
                    sorting = eSortingType.TimeDescending;
                    break;
                case 2:
                    sorting = eSortingType.CountAscending;
                    break;
                case 3:
                    sorting = eSortingType.CountDescending;
                    break;
                default:
                    sorting = eSortingType.CountDescending;
                    break;
            }

            return sorting;
        }


        private void comboBoxYearSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxMonth.Items.Clear();
            comboBoxMonthSort.Items.Clear();
            eSortingType sorting = getSorting(comboBoxYearSort.SelectedIndex);

            displayYearCounts(sorting);
        }

        private void comboBoxMonthSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedYear = int.Parse(listBoxYear.SelectedItem.ToString().Split(':')[0]);
            eSortingType sorting = getSorting(comboBoxMonthSort.SelectedIndex);
            displayMonthCounts(selectedYear, sorting);
        }

        private void comboBoxHourSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            eSortingType sorting = getSorting(comboBoxHourSort.SelectedIndex);
            displayHoursCounts(sorting);
        }


        private void listBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxYear.SelectedItem != null)
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
                int selectedMonth = Array.IndexOf(UserUtils.sr_Months, selectedMonthName) + 1; ;

                labelDateOfPosts.Text = $"Your posts from {selectedMonthName} {selectedYear}";
                filterPostsByTime(i_Year: selectedYear, i_Month: selectedMonth);
            }
        }

        private void listBoxHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxHour.SelectedItem != null)
            {
                int selectedHour = int.Parse(listBoxHour.SelectedItem.ToString().Split(':')[0]);
                labelDateOfPosts.Text = $"Your posts from {makeHourFormat(selectedHour)} in total: ";

                filterPostsByTime(i_Hour: selectedHour);
            }
        }

        private string makeHourFormat(int i_Hour)
        {
            return $"{i_Hour.ToString("D2")}:00";
        }

        private void filterPostsByTime(int? i_Year = null, int? i_Month = null, int? i_Hour = null)
        {
            List<IActivityItem> items = AppManager.Instance.ActivityCenter.GetItemsByTime(i_Year, i_Month, i_Hour);

            //List<Post> posts = AppManager.Instance.ActivityCenter.GetPostsByTime(i_Year, i_Month, i_Hour);
            //List<Photo> photos = AppManager.Instance.ActivityCenter.GetPhotosByTime(i_Year, i_Month, i_Hour);

            listBoxFilteredPosts.Items.Clear();
            addItemsToListbox(items, listBoxFilteredPosts);


            //addPostsToListbox(posts, listBoxFilteredPosts);
            //addPhotosToListbox(photos, listBoxFilteredPosts);

            List<Photo> filteredPhotos = items
                .OfType<PhotoAdapter>()
                .Select(adapter => adapter.Photo)
                .ToList();

            albumControlFilteredPhotos.SetPhotos(filteredPhotos);
        }

        private void addItemsToListbox(List<IActivityItem> i_Items, ListBox i_ListBox)
        {
            foreach(IActivityItem activityItem in i_Items)
            {
                i_ListBox.Items.Add(activityItem.Description);
            }
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
            if (AppManager.Instance.LoggedInUser != null)
            {
                textBoxStatusPost.Text = $"What's on your mind, {AppManager.Instance.LoggedInUser.FirstName}";
                textBoxStatusPost.ForeColor = Color.Gray;
            }
        }

        private void textBoxStatus_Click(object sender, EventArgs e)
        {
            if ( AppManager.Instance.LoggedInUser != null)
            {
                textBoxStatusPost.Text = ""; 
                textBoxStatusPost.ForeColor = Color.Black;
            }
        }

        private void textBoxStatus_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxStatusPost.Text))
            {
                textBoxStatusPost.Text = $"What's on your mind, {AppManager.Instance.LoggedInUser.Name}?";
                textBoxStatusPost.ForeColor = Color.Gray; 
            }
        }


        private void fetchAlbums()
        {
            panelAlbums.Visible = true;
            listBoxUserAlbums.Items.Clear();
            albumControlUserAlbum.ClearPictureBoxInAlbum();
            listBoxUserAlbums.DisplayMember = "Name";
            foreach (FacebookWrapper.ObjectModel.Album album in AppManager.Instance.LoggedInUser.Albums)
            {
                listBoxUserAlbums.Items.Add(album);
            }

            if (listBoxUserAlbums.Items.Count == 0)
            {
                listBoxUserAlbums.Items.Add("No Albums to retrieve");
            }
        }
        private void fetchFavoriteTeams()
        {
            panelFavoriteTeams.Visible = true;
            pictureBoxFavoriteTeam.Image = null;
            listBoxUserFavoriteTeams.Items.Clear();
            listBoxUserFavoriteTeams.DisplayMember = "Name";
            foreach (Page page in AppManager.Instance.LoggedInUser.FavofriteTeams)
            {
                listBoxUserFavoriteTeams.Items.Add(page);
            }

            if (listBoxUserFavoriteTeams.Items.Count == 0)
            {
                listBoxUserFavoriteTeams.Items.Add("No Albums to retrieve");
            }
        }
        private void fetchGroups()
        {
            panelGroups.Visible = true;
            listBoxUserGroups.Items.Clear();
            listBoxUserGroups.DisplayMember = "Name";
            foreach (Group group in AppManager.Instance.LoggedInUser.Groups)
            {
                listBoxUserGroups.Items.Add(group);
            }

            if (listBoxUserGroups.Items.Count == 0)
            {
                listBoxUserGroups.Items.Add("No Groups to retrieve");
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
                if (AppManager.Instance.LoggedInUser.LikedPages != null && AppManager.Instance.LoggedInUser.LikedPages.Count > 0)
                {
                    foreach (Page likedPage in AppManager.Instance.LoggedInUser.LikedPages)
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
                FacebookWrapper.ObjectModel.Album selectedAlbum = listBoxUserAlbums.SelectedItem as FacebookWrapper.ObjectModel.Album;

                if (selectedAlbum != null)
                {
                    albumControlUserAlbum.SetPhotos(selectedAlbum.Photos.ToList());
                }
            }

        }

        private void updateFilteredFriendsListBox(HashSet<User> i_FilterdFriends)
        {
            listBoxFilteredUsers.Items.Clear();
            listBoxUserFavoriteTeams.DisplayMember = "Name";
            foreach(User user in i_FilterdFriends)
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
            {
                return;
            }
            User selectedFriend = comboBoxFriendList.SelectedItem as User;
            int minAge = (int)numericUpDownMinimumAge.Value;
            int maxAge = (int)numericUpDownMaximumAge.Value;
            filterable.Add(new FilterAge(minAge, maxAge));
            if (comboBoxGender.SelectedItem != null && !comboBoxGender.SelectedItem.Equals("No Preference"))
            {

                filterable.Add(new FilterGender((User.eGender)comboBoxGender.SelectedItem));
            }
            filterable.Add(new FilterRelationshipStatus(getUserSelectedRelationshipStatuses()));
            filterable.Add(new FilterLikedPages(getUserSelectedLikedPagesId()));

            HashSet<User> filterdFriends = AppManager.Instance.FindFriends.GetFriendUserCommonFriendsPages(filterable, selectedFriend);
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

        private void buttonSetStatusPost_Click(object sender, EventArgs e)
        {
            string postData = textBoxStatusPost.Text;
            try
            {
                 AppManager.Instance.LoggedInUser.PostStatus(postData);
                 MessageBox.Show("New status was Posted!", "New Status Posted");
           
            }
            catch (FacebookOAuthException)
            {
                MessageBox.Show("cannot upload status.", "Facebook server error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
