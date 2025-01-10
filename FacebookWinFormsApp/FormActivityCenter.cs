using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public partial class FormActivityCenter : Form
    {

        private ActivityCenter m_ActivityCenter;

        public FormActivityCenter()
        {
            InitializeComponent();
        }

        public void fetchActivityCenter()
        {
            m_ActivityCenter = new ActivityCenter();

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
            List<KeyValuePair<int, int>> yearCounts = m_ActivityCenter.GetYearCounts(i_SortBy);
            foreach (KeyValuePair<int, int> year in yearCounts)
            {
                listBoxYear.Items.Add($"{year.Key}: {year.Value} posts/photos");
            }

            if (listBoxYear.Items.Count > 0)
            {
                populateSortComboBox(comboBoxYearSort, "Year");
            }
        }

        private void displayMonthCounts(int i_SelectedYear, string i_SortBy = "CountDescending")
        {
            listBoxMonth.Items.Clear();
            List<KeyValuePair<int, int>> monthCounts = m_ActivityCenter.GetMonthCounts(i_SelectedYear, i_SortBy);
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

        private void displayHoursCounts(string i_SortBy = "CountDescending")
        {
            listBoxHour.Items.Clear();
            List<KeyValuePair<int, int>> hoursCounts = m_ActivityCenter.GetHourCounts(i_SortBy);
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
            List<Post> posts = m_ActivityCenter.GetPostsByTime(i_Year, i_Month, i_Hour);
            List<Photo> photos = m_ActivityCenter.GetPhotosByTime(i_Year, i_Month, i_Hour);

            listBoxFilteredPosts.Items.Clear();
            addPostsToListbox(posts, listBoxFilteredPosts);
            addPhotosToListbox(photos, listBoxFilteredPosts);

            //albumFilteredPhotos.SetPhotos(photos);
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


    }
}
