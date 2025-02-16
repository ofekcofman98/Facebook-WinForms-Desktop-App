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
using BasicFacebookFeatures;
using BasicFacebookFeatures.Patterns.Observer;
using CefSharp;
using Action = System.Action;
using BasicFacebookFeatures.Strategy;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form, IUserDataObserver
    {
        private Action m_OnLogin;
        private Action m_OnLogout;

        private readonly List<Panel> r_HomePanels;
        private readonly List<TabPage> r_AddedTabs;

        private User m_LoggedInUser;

        public FormMain()
        {
            InitializeComponent();
            FacebookWrapper.FacebookService.s_CollectionLimit = 25;
            r_HomePanels = new List<Panel> { panelAlbums, panelStatusPost, panelFavoriteTeams, panelFriends, panelLikes, panelGroups };
            r_AddedTabs = new List<TabPage> { tabMyProfile, tabActivityCenter, tabFindNewFriends };
            updateTabs(i_IsVisible: false);
            this.Load += FormMain_Load;
            AddLoginMethods();
            AddLogoutMethods();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            AppManager.Instance.AttachObserver(this);
        }

        void AddToOnLoginWithThread(Action action)
        {
            m_OnLogin += () => new Thread(() => action()).Start();
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
                    updateTabs(i_IsVisible: true);
                    AppManager.Instance.GetUserData();
                    
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddLogoutMethods()
        {
            m_OnLogout += updateHomePanelsVisible;
            m_OnLogout += unLaunchFacebook;
            m_OnLogout += updateLoginButton;
            m_OnLogout += clearUI;
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            performLogout();
        }

        private void performLogout()
        {
            try
            {
                AppManager.Instance.Logout();
                updateTabs(i_IsVisible: false);
                m_OnLogout?.Invoke();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppManager.Instance.DetachObserver(this);
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

        public void OnUserDataUpdated(User i_User)
        {

            m_LoggedInUser = i_User;

            if (m_LoggedInUser != null)
            {
                updateUI();
            }
            else
            {
                clearUI();
            }
        }
        public void AddLoginMethods()
        {

            AddToOnLoginWithThread(() => updateLoginButton());
            AddToOnLoginWithThread(() => updateHomePanelsVisible());
            AddToOnLoginWithThread(() => fetchProfileInfo());
            AddToOnLoginWithThread(() => fetchMyProfile());
            AddToOnLoginWithThread(() => fetchActivityCenter());
            AddToOnLoginWithThread(() => fetchFriendsLookupPage());
            AddToOnLoginWithThread(() => fetchUserDataIntoListBox());

            m_OnLogin += fetchStatusPost; //no need for server request
        }

        private void updateUI()
        {
            updateLoginButton();
            updateHomePanelsVisible();
            fetchProfileInfo();
            fetchMyProfile();
            fetchActivityCenter();
            fetchFriendsLookupPage();
            fetchUserDataIntoListBox();
            fetchStatusPost();
        }

        private void clearUI()
        {
            labelUserName.Visible = false;
            pictureBoxProfile.Visible = false;
            listBoxUserFriends.DataSource = null;
            listBoxUserAlbums.DataSource = null;
            listBoxUserGroups.DataSource = null;
            listBoxLikes.DataSource = null;
            listBoxUserFavoriteTeams.DataSource = null;
            buttonLogin.Enabled = true;
            buttonLogin.Text = "Login";
            buttonLogin.BackColor = buttonLogout.BackColor;
            buttonLogout.Enabled = false;
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

        private void fetchUserDataIntoListBox()
        {
            if (AppManager.Instance.IsLoggedIn)
            {
                fetchData(new FetchFriends(), listBoxUserFriends, friendListBindingSource);
                fetchData(new FetchAlbums(), listBoxUserAlbums, albumBindingSource);
                fetchData(new FetchGroups(), listBoxUserGroups, groupBindingSource);
                fetchData(new FetchLikedPages(), listBoxLikes, pageBindingSource);
                fetchData(new FetchFavoriteTeams(), listBoxUserFavoriteTeams, pageBindingSource);
            }
        }

        private void fetchData<T>(IFetchStrategy<T> i_FetchStrategy, ListBox i_ListBox, BindingSource i_BindingSource)
        {
            Fetcher<T> fetcher = new Fetcher<T>(i_FetchStrategy);
            List<T> dataList = fetcher.FetchData(m_LoggedInUser);

            i_ListBox.DataSource = dataList;
            i_ListBox.DisplayMember = "Name";

            if (!handleEmptyDataSourceForListbox(dataList, i_ListBox, typeof(T).Name))
            {
                bindDataSourceToListbox(dataList, i_BindingSource, i_ListBox);
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
            populateRelationshipStatusList();
            populateLikedPagesList();
            populateGenderComboBox();

        }
        private void clearChoiceItemFromCombobox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.SelectedItem = null;
            comboBox.SelectedIndex = -1;
            comboBox.Text = "";
        }

        private void populateGenderComboBox()
        {
            comboBoxGender.Invoke(new Action(()=>clearChoiceItemFromCombobox(comboBoxGender)));
            foreach (User.eGender gender in Enum.GetValues(typeof(User.eGender)))
            {
                comboBoxGender.Invoke(new Action(() => comboBoxGender.Items.Add(gender)));
            }
            comboBoxGender.Invoke(new Action(() => comboBoxGender.Items.Add("No Preference")));
            comboBoxGender.Invoke(new Action(()=> comboBoxGender.SelectedIndex = 0));
        }

        private void populateRelationshipStatusList()
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
        }


        private void fetchFriendsComboBox()
        {
            comboBoxFriendList.Invoke(new Action(() => clearChoiceItemFromCombobox(comboBoxFriendList)));
     
            foreach (User user in m_LoggedInUser.Friends)
            {
                comboBoxFriendList.Invoke(new Action(() => comboBoxFriendList.Items.Add(user)));
            }
            comboBoxFriendList.Invoke(new Action(() => comboBoxFriendList.SelectedIndex = 0));

        }

        private bool handleEmptyDataSourceForListbox<T>(IEnumerable<T> i_Items, ListBox i_ListBox, string i_ItemString)
        {
            bool isEmpty = false;

            if(i_Items == null || !i_Items.Any())
            {
                i_ListBox.Invoke(new Action(() => {
                    i_ListBox.DataSource = null;
                    i_ListBox.Items.Clear();
                    i_ListBox.Items.Add($"No {i_ItemString} to retrieve.");

                }));

                isEmpty = true;
            }

            return isEmpty;
        }

        private void bindDataSourceToListbox<T>(IEnumerable<T> i_Items, BindingSource i_BindingSource, ListBox i_ListBox)
        {
            if(!i_ListBox.InvokeRequired)
            {
                i_BindingSource.DataSource = i_Items;
            }
            else
            {
                i_ListBox.Invoke(new Action(() => i_BindingSource.DataSource = i_Items));
            }
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
        }


        private void fetchActivityCenter()
        {
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
                comboBoxYearSort.Invoke(new Action(() => populateSortComboBox(comboBoxYearSort)));
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
                comboBoxMonthSort.Invoke(new Action(() => populateSortComboBox(comboBoxMonthSort)));
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
                comboBoxHourSort.Invoke(new Action(() => populateSortComboBox(comboBoxHourSort)));
            }
        }

        private void populateSortComboBox(ComboBox i_ComboBox)
        {
            i_ComboBox.Items.Clear();
            
            var sortingOptionDictionary = AppManager.Instance.ActivityCenter.r_SortingDictionary;
            
            foreach (KeyValuePair<eSortingType, string> sortingOption in sortingOptionDictionary)
            {
                i_ComboBox.Items.Add(sortingOption.Value);
            }
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
                int selectedMonth = Array.IndexOf(UserUtils.sr_Months, selectedMonthName) + 1;

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

        private void unLaunchFacebook()
        {
            labelUserName.Visible = false;
            pictureBoxProfile.Visible = false;
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

        private void updateFilteredFriendsListBox(List<User> i_FilteredFriends)
        {
            listBoxFilteredUsers.Items.Clear();
            listBoxFilteredUsers.DisplayMember = "Name";
            foreach(User user in i_FilteredFriends)
            {
                listBoxFilteredUsers.Items.Add(user);
            }

            if(listBoxFilteredUsers.Items.Count == 0)
            {
                listBoxFilteredUsers.Items.Add("No Users to retrieve");
            }
            listBoxFilteredUsers.SelectedIndex = 0;
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
            Filters filters = new Filters();
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

            List<User> filteredFriends = filters.FilterUserFriends(selectedFriend);
            updateFilteredFriendsListBox(filteredFriends);
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
