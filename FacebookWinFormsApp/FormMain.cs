using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
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
using BasicFacebookFeatures.Filters;
using CefSharp;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private Action onLogin;
        private Action onLogout;
        
        private readonly ActivityCenterFacade r_ActivityCenterFacade;

        private readonly List<Panel> r_HomePanels;
        private readonly List<TabPage> r_AddedTabs;

        private User m_LoggedInUser;

        public FormMain()
        {
            InitializeComponent();
            
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

            addLoginMethods();
            addLogoutMethods();
        }

        void AddToOnLoginWithThread(Action action)
        {
            onLogin += () => new Thread(() => action()).Start();
        }

        public void addLogoutMethods()
        {
            onLogout += updateHomePanelsVisible;
            onLogout += unLaunchFacebook;
            onLogout += updateLoginButton;
        }

        public void addLoginMethods()
        {
            AddToOnLoginWithThread(() => updateLoginButton());
            AddToOnLoginWithThread(() => updateHomePanelsVisible());
            AddToOnLoginWithThread(() => fetchProfileInfo());
            AddToOnLoginWithThread(() => fetchLikedPages());
            AddToOnLoginWithThread(() => fetchAlbums());
            AddToOnLoginWithThread(() => fetchFriendList());
            AddToOnLoginWithThread(() => fetchMyProfile());
            AddToOnLoginWithThread(() => fetchActivityCenter());
            AddToOnLoginWithThread(() => fetchFriendsLookupPage());
            AddToOnLoginWithThread(() => fetchGroups());
            AddToOnLoginWithThread(() => fetchFavoriteTeams());
            onLogin += fetchStatusPost;//no need for server request
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
                AppManager.Instance.Login();

                if(AppManager.Instance.IsLoggedIn)
                {
                    m_LoggedInUser = AppManager.Instance.LoggedInUser;
                    updateTabs(AppManager.Instance.IsLoggedIn);
                    onLogin?.Invoke();
                }
            }
            catch(Exception e)
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
            bool isVisible = AppManager.Instance.IsLoggedIn;

            foreach (Panel panel in r_HomePanels)
            {
                panel.Invoke(new Action(() => panel.Visible = isVisible));
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
                string loggedInText = $"Logged in as {m_LoggedInUser.Name}";
                buttonLogin.Invoke(new Action(() =>
                {
                    buttonLogin.Text = loggedInText;
                    buttonLogin.BackColor = Color.LightGreen;
                    buttonLogin.Enabled = false;
                    buttonLogout.Enabled = true;
                }));
            }
            else
            {
                buttonLogin.Invoke(new Action(() =>
                {
                    buttonLogin.BackColor = buttonLogout.BackColor;
                    buttonLogin.Enabled = true;
                    buttonLogin.Text = "Login";
                    buttonLogout.Enabled = false;
                }));
            }
        }


        private void fetchProfileInfo()
        {
            string userName = m_LoggedInUser.FirstName;
            string profilePictureUrl = m_LoggedInUser.PictureNormalURL;

            labelUserName.Invoke(new Action(() =>
            {
                labelUserName.Visible = true;
                labelUserName.Text = $"Hello, {userName}!";
            }));

            pictureBoxProfile.Invoke(new Action(() =>
            {
                pictureBoxProfile.Visible = true;
                pictureBoxProfile.ImageLocation = m_LoggedInUser.PictureNormalURL;
            }));
        }


        private void fetchFriendsLookupPage()
        {
            fetchFriendsComboBox();
            populateRealitionshipStatusList();
            populateLikedPagesList();
            populateGenderComboBox();

        }
        private void clearChoiceItemFromComobox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.SelectedItem = null;
            comboBox.SelectedIndex = -1;
            comboBox.Text = "";
        }

        private void populateGenderComboBox()
        {
            comboBoxGender.Invoke(new Action(()=>clearChoiceItemFromComobox(comboBoxGender)));
            foreach (User.eGender gender in Enum.GetValues(typeof(User.eGender)))
            {
                comboBoxGender.Invoke(new Action(() => comboBoxGender.Items.Add(gender)));
            }
            comboBoxGender.Invoke(new Action(() => comboBoxGender.Items.Add("No Preference")));
            comboBoxGender.Invoke(new Action(()=> comboBoxGender.SelectedIndex = 0));
        }

        private void populateRealitionshipStatusList()
        {
            checkedListBoxRealitionshipStatus.Invoke(new Action(() =>
            {
                checkedListBoxRealitionshipStatus.Visible = true;
                checkedListBoxRealitionshipStatus.Items.Clear();
            }));
            foreach (User.eRelationshipStatus relationshipStatus in Enum.GetValues(typeof(User.eRelationshipStatus)))
            {
                checkedListBoxRealitionshipStatus.Invoke(new Action(() =>
                checkedListBoxRealitionshipStatus.Items.Add(relationshipStatus)));

            }

        }

        private void populateLikedPagesList()
        {
            checkedListBoxlikedPages.Invoke(new Action(() => resetListBox(checkedListBoxlikedPages)));
    
            foreach (Page page in m_LoggedInUser.LikedPages)
            {
                checkedListBoxlikedPages.Invoke(new Action(()=> checkedListBoxlikedPages.Items.Add(page)));
            }

            //if (checkedListBoxlikedPages.Items.Count == 0)    
            //{
            //    checkedListBoxlikedPages.Items.Add("No liked pages to retrieve");
            //}
        }


        private void fetchFriendsComboBox()
        {
            comboBoxFriendList.Invoke(new Action(() => clearChoiceItemFromComobox(comboBoxFriendList)));
     
            foreach (User user in m_LoggedInUser.Friends)
            {
                comboBoxFriendList.Invoke(new Action(() => comboBoxFriendList.Items.Add(user)));
            }
            comboBoxFriendList.Invoke(new Action(() => comboBoxFriendList.SelectedIndex = 0));

        }


        private void fetchFriendList()
        {
            var allFriends = m_LoggedInUser.Friends;

            //if(allFriends.Count == 0)
            //{
            //    List<string> emptyList = new List<string> { "No friends to display" };

            //    if(!listBoxUserFriends.InvokeRequired)
            //    {
            //        userBindingSource.DataSource = emptyList;
            //        listBoxUserFriends.DisplayMember = null;
            //    }
            //    else
            //    {
            //        listBoxUserFriends.Invoke(new Action(() =>
            //            {
            //                userBindingSource.DataSource = new List<string> { "No friends to display" };
            //                listBoxUserFriends.DisplayMember = null;
            //            }));

            //    }
            //}

            if (!listBoxUserFriends.InvokeRequired)
            {
                friendListBindingSource.DataSource = allFriends;
            }
            else
            {
                listBoxUserFriends.Invoke(new Action(() => friendListBindingSource.DataSource = allFriends));
            }
            //if (allFriends.Count == 0)
            //{
            //    userBindingSource.DataSource = new List<string> { "No friends to display" };
            //}

        }

        private void fetchMyProfile()
        {
            if(m_LoggedInUser != null)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => userBindingSource.DataSource = m_LoggedInUser));
                    labelGenderData.Invoke(new Action(() =>
                        labelGenderData.Text = m_LoggedInUser.Gender.ToString()));

                }
                else
                {
                    userBindingSource.DataSource = m_LoggedInUser;
                }
            }
            //    labelEmailData.Invoke(new Action(() =>
            //        labelEmailData.Text = m_LoggedInUser.Email));

            //    labelBirthdayData.Invoke(new Action(() =>
            //        labelBirthdayData.Text = m_LoggedInUser.Birthday));

            //    labelGenderData.Invoke(new Action(() =>
            //        labelGenderData.Text = m_LoggedInUser.Gender.ToString()));

            //    labelFullNameData.Invoke(new Action(() =>
            //        labelFullNameData.Text = m_LoggedInUser.Name));

            //    PictureBoxMyProfile.Invoke(new Action(() =>
            //        PictureBoxMyProfile.Image = m_LoggedInUser.ImageLarge));
            //}
        }


        private void fetchActivityCenter()
        {
            //AppManager.Instance.getUserData();
            listBoxFilteredItemsDescriptions.Invoke(new Action(() => {
                listBoxFilteredItemsDescriptions.Visible = true;
                listBoxFilteredItemsDescriptions.Items.Clear();
            }));

            albumControlFilteredPhotos.Invoke(new Action(() =>
            {
                albumControlFilteredPhotos.Visible = true;
                albumControlFilteredPhotos.ClearPictureBoxInAlbum();
            }));

            listBoxYear.Invoke(new Action(() => listBoxYear.Visible = true));
            listBoxMonth.Invoke(new Action(() => listBoxMonth.Visible = true));
            listBoxHour.Invoke(new Action(() => listBoxHour.Visible = true));
   

            displayYearCounts();
            displayHoursCounts();
        }

        private void displayYearCounts(eSortingType i_SortBy = eSortingType.CountDescending) // default?
        {
            listBoxYear.Invoke(new Action(()=> listBoxYear.Items.Clear()));
            List<KeyValuePair<int, int>> yearCounts = AppManager.Instance.ActivityCenter.GetYearCounts(i_SortBy);
            foreach (KeyValuePair<int, int> year in yearCounts)
            {
                listBoxYear.Invoke(new Action(() => listBoxYear.Items.Add($"{year.Key}: {year.Value} posts/photos")));
            }

            if (listBoxYear.Items.Count > 0)
            {
                comboBoxYearSort.Invoke(new Action(() => populateSortComboBox(comboBoxYearSort, "Year")));
            }
        }

        private void displayMonthCounts(int i_SelectedYear, eSortingType i_SortBy = eSortingType.CountDescending)
        {
            listBoxMonth.Invoke(new Action(() => listBoxMonth.Items.Clear()));
            List<KeyValuePair<int, int>> monthCounts = AppManager.Instance.ActivityCenter.GetMonthCounts(i_SelectedYear, i_SortBy);

            foreach (KeyValuePair<int, int> month in monthCounts)
            {
                listBoxMonth.Invoke(new Action(() => listBoxMonth.Items.Add($"{UserUtils.sr_Months[month.Key - 1]}: {month.Value} posts/photos")));
            }

            if (listBoxMonth.Items.Count > 0)
            {
                comboBoxMonthSort.Invoke(new Action(() => populateSortComboBox(comboBoxMonthSort, "Month")));
            }
        }

        private void displayHoursCounts(eSortingType i_SortBy = eSortingType.CountDescending)
        {
           listBoxHour.Invoke(new Action(()=> listBoxHour.Items.Clear()));
            List<KeyValuePair<int, int>> hoursCounts = AppManager.Instance.ActivityCenter.GetHourCounts(i_SortBy);
            foreach (KeyValuePair<int, int> hour in hoursCounts)
            {
                listBoxHour.Invoke(new Action(() => listBoxHour.Items.Add($"{makeHourFormat(hour.Key)}: {hour.Value} posts/photos")));
            }

            if (listBoxHour.Items.Count > 0)
            {
                comboBoxHourSort.Invoke(new Action(() => populateSortComboBox(comboBoxHourSort, "Hour")));
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

            listBoxFilteredItemsDescriptions.Items.Clear();
            addItemsToListbox(items, listBoxFilteredItemsDescriptions);

            List<Photo> filteredPhotos = new List<Photo>();

            foreach(IActivityItem item in items)
            {
                if(item is PhotoAdapter adapter)
                {
                    filteredPhotos.Add(adapter.Photo);
                }
            }

            albumControlFilteredPhotos.SetPhotos(filteredPhotos);
        }

        private void addItemsToListbox(List<IActivityItem> i_Items, ListBox i_ListBox)
        {
            foreach(IActivityItem activityItem in i_Items)
            {
                i_ListBox.Items.Add(activityItem.Description);
            }
        }


        //private void addPostsToListbox(List<Post> i_Posts, ListBox i_ListBox)
        //{
        //    foreach (Post post in i_Posts)
        //    {
        //        if (post.Message == null)
        //        {
        //            i_ListBox.Items.Add("[No message]");
        //        }
        //        else
        //        {
        //            i_ListBox.Items.Add(post.Message);
        //        }
        //    }
        //}

        //private void addPhotosToListbox(List<Photo> i_Photos, ListBox i_ListBox)
        //{
        //    foreach (Photo photo in i_Photos)
        //    {
        //        if (photo.Name == null)
        //        {
        //            i_ListBox.Items.Add("[No title]");
        //        }
        //        else
        //        {
        //            i_ListBox.Items.Add(photo.Message);
        //        }
        //    }
        //}

        private void fetchStatusPost()
        {
            panelStatusPost.Visible = true;
            textBoxStatusPost.Click += textBoxStatus_Click;
            textBoxStatusPost.Leave += textBoxStatus_Leave;
            if (m_LoggedInUser != null)
            {
                textBoxStatusPost.Text = $"What's on your mind, {m_LoggedInUser.FirstName}";
                textBoxStatusPost.ForeColor = Color.Gray;
            }
        }

        private void textBoxStatus_Click(object sender, EventArgs e)
        {
            if ( m_LoggedInUser != null)
            {
                textBoxStatusPost.Text = ""; 
                textBoxStatusPost.ForeColor = Color.Black;
            }
        }

        private void textBoxStatus_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxStatusPost.Text))
            {
                textBoxStatusPost.Text = $"What's on your mind, {m_LoggedInUser.Name}?";
                textBoxStatusPost.ForeColor = Color.Gray; 
            }
        }


        private void fetchAlbums()
        {
            panelAlbums.Invoke(new Action(() => panelAlbums.Visible = true));
            listBoxUserAlbums.Invoke(new Action(() => resetListBox(listBoxUserAlbums)));
            albumControlUserAlbum.Invoke(new Action(() => albumControlUserAlbum.ClearPictureBoxInAlbum()));
            foreach (Album album in m_LoggedInUser.Albums)
            {
                listBoxUserAlbums.Invoke(new Action(() => listBoxUserAlbums.Items.Add(album)));
            }

            //if (listBoxUserAlbums.Items.Count == 0)
            //{
            //    listBoxUserAlbums.Items.Add("No Albums to retrieve");
            //}
        }
        private void fetchFavoriteTeams()
        {
            var allFavoriteTeams = m_LoggedInUser.FavofriteTeams;

            if(!listBoxUserFavoriteTeams.InvokeRequired)
            {
                pageBindingSource.DataSource = allFavoriteTeams;
            }
            else
            {
                listBoxUserFavoriteTeams.Invoke(new Action(() => pageBindingSource.DataSource = allFavoriteTeams));
            }
            pictureBoxFavoriteTeam.Image = null;
        }

        private void resetListBox(ListBox listBox)
        {
            pictureBoxFavoriteTeam.Image = null;
            if (listBox.DataSource != null)
            {
                listBox.DataSource = null;
            }
            else
            {
                listBox.Items.Clear(); 
            }

            listBox.DisplayMember = "Name"; 
        }

        private void fetchGroups()
        {
            panelGroups.Invoke(new Action(() => panelGroups.Visible = true));

            //var allGroups = m_LoggedInUser.Groups;

            //if (allGroups == null || allGroups.Count == 0)
            //{
            //    MessageBox.Show("No groups found or failed to load groups.");
            //    return;
            //}

            //if (!listBoxUserGroups.InvokeRequired)
            //{
            //    groupBindingSource.DataSource = allGroups;
            //}
            //else
            //{
            //    listBoxUserGroups.Invoke(new Action(() => groupBindingSource.DataSource = allGroups));
            //}

            listBoxUserGroups.Invoke(new Action(() => resetListBox(listBoxUserGroups)));
            foreach (Group group in m_LoggedInUser.Groups)
            {

                listBoxUserGroups.Invoke(new Action(() => listBoxUserGroups.Items.Add(group)));
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
            panelLikes.Invoke(new Action(() => panelLikes.Visible = true));

            listBoxLikes.Invoke(new Action(() => resetListBox(listBoxLikes)));

            try
            {
                if (m_LoggedInUser.LikedPages != null && m_LoggedInUser.LikedPages.Count > 0)
                {
                    foreach (Page likedPage in m_LoggedInUser.LikedPages)
                    {
                        listBoxLikes.Invoke(new Action(()=> listBoxLikes.Items.Add(likedPage)));
                    }
                }
                else
                {
                    listBoxLikes.Invoke(new Action(() => listBoxLikes.Items.Add("No liked pages to display.")));
                }
            }
            catch (Exception ex)
            {
                listBoxLikes.Invoke(new Action(() => listBoxLikes.Items.Add("Couldn't fetch liked pages.")));
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void userFavoriteTeamsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxUserFavoriteTeams.SelectedItem is Page selectedTeam)
            {
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
            listBoxUserFavoriteTeams.SelectedIndex = 0;
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
            CompositeFilter filters = new CompositeFilter();
            if (comboBoxFriendList.SelectedItem == null)
            {
                return;
            }
            User selectedFriend = comboBoxFriendList.SelectedItem as User;
            int minAge = (int)numericUpDownMinimumAge.Value;
            int maxAge = (int)numericUpDownMaximumAge.Value;
            filters.AddFilter(new FilterAge(minAge, maxAge));
            if (comboBoxGender.SelectedItem != null && !comboBoxGender.SelectedItem.Equals("No Preference"))
            {

                filters.AddFilter(new FilterGender((User.eGender)comboBoxGender.SelectedItem));
            }
            filters.AddFilter(new FilterRelationshipStatus(getUserSelectedRelationshipStatuses()));
            filters.AddFilter(new FilterLikedPages(getUserSelectedLikedPagesId()));

            HashSet<User> filterdFriends = AppManager.Instance.FindFriends.GetFriendUserCommonFriendsPages(filters, selectedFriend);
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
            pictureBoxFilteredUsers.Image = (listBoxFilteredUsers.SelectedItem as User)?.ImageNormal;
        }

        private void buttonSetStatusPost_Click(object sender, EventArgs e)
        {
            string postData = textBoxStatusPost.Text;
            try
            {
                 m_LoggedInUser.PostStatus(postData);
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
