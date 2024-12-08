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

        private int m_FilteredPhotoIndex = -1;

        private readonly string[] r_Months =
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };

        private readonly List<Panel> m_Panels;

        public FormMain()
        {
            InitializeComponent();
            r_AppManager = new AppManager();
            FacebookWrapper.FacebookService.s_CollectionLimit = 25;
            tabsController.TabPages.Remove(tabMyProfile);
            tabsController.TabPages.Remove(tabActivityCenter);
            m_Panels = new List<Panel>
                           {
                               panelAlbums,
                               panelStatusPost,
                               panelFavoriteTeams,
                               panelFriends,
                               panelLikes,
                               panelGroups
                           };
            OnLogin += fetchProfileInfo;
            OnLogin += fetchLikedPages;
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
                SetHomePanelsVisible();
                OnLogin?.Invoke();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetHomePanelsVisible(bool i_Visibile = true)
        {
            foreach(Panel panel in m_Panels)
            {
                panel.Visible = i_Visibile;
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
            tabsController.TabPages.Add(tabMyProfile);
            labelEmailData.Text = r_AppManager.LoggedInUser.Email;
            labelBirthdayData.Text = r_AppManager.LoggedInUser.Birthday;
            labelGenderData.Text = r_AppManager.LoggedInUser.Gender.ToString();
            labelFullNameData.Text = r_AppManager.LoggedInUser.Name;
            PictureBoxMyProfile.Image = r_AppManager.LoggedInUser.ImageLarge;

        }
        private void fetchStats()
        {
            tabsController.TabPages.Add(tabActivityCenter);

            listBoxFilteredPosts.Visible = true;
            listBoxFilteredPosts.Items.Clear();

            listBoxYear.Visible = true;
            listBoxYear.Items.Clear();

            listBoxMonth.Visible = true;
            listBoxMonth.Items.Clear();

            listBoxHour.Visible = true;
            listBoxHour.Items.Clear();

            displayYearCounts();
            displayHoursCounts();
        }

        private void displayYearCounts()
        {
            List<KeyValuePair<int, int>> yearCounts = r_AppManager.ActivityCenter.GetYearCounts();
            foreach (KeyValuePair<int, int> year in yearCounts)
            {
                listBoxYear.Items.Add($"{year.Key}: {year.Value} posts/photos");
            }
        }

        private void displayHoursCounts()
        {
            List<KeyValuePair<int, int>> hoursCounts = r_AppManager.ActivityCenter.GetHourCounts();
            foreach (KeyValuePair<int, int> hour in hoursCounts)
            {
                listBoxHour.Items.Add($"{hour.Key}: {hour.Value} posts/photos");
            }
        }


        private void listBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
           if(listBoxYear.SelectedItem != null)
           {
               int selectedYear = int.Parse(listBoxYear.SelectedItem.ToString().Split(':')[0]);
               labelDateOfPosts.Text = $"Your posts from {selectedYear}";
               filterPostsByTime(i_Year: selectedYear);

               List<KeyValuePair<int, int>> monthCounts = r_AppManager.ActivityCenter.GetMonthCounts(selectedYear);
               listBoxMonth.Items.Clear();

               foreach (KeyValuePair<int, int> month in monthCounts)
               {
                   listBoxMonth.Items.Add($"{r_Months[month.Key-1]}: {month.Value} posts/photos");
               }
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
                filterPostsByTime(i_Hour: selectedHour);
            }
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
            buttonLogin.Text = "Login";
            buttonLogin.BackColor = buttonLogout.BackColor;
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;
            SetHomePanelsVisible(false);
            tabsController.TabPages.Remove(tabMyProfile);
            tabsController.TabPages.Remove(tabActivityCenter);
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

        private void userAlbumsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxUserAlbums.SelectedItems.Count == 1)
            {
                m_albumPictureCounter = 0;
                m_currentAlbum = listBoxUserAlbums.SelectedItem as FacebookWrapper.ObjectModel.Album;
                albumUserAlbums.SetPhotos(m_currentAlbum.Photos.ToList());
            }

        }
        private void MyProfileTab_Click(object sender, EventArgs e)
        {

        }

        private void listBoxFilteredPosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //pictureBoxFilteredPosts.Image = 
        }

    }
}
