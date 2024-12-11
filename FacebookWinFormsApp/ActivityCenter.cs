using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{

    internal class ActivityCenter
    {

        private readonly List<Post> r_UserPosts;
        private readonly List<Photo> r_UserPhotos;

        private readonly Dictionary<int, int> r_YearCounts = new Dictionary<int, int>();
        private readonly Dictionary<int, int> r_MonthCounts = new Dictionary<int, int>();
        private readonly Dictionary<int, int> r_HourCounts = new Dictionary<int, int>();

        public List<Post> FilteredPosts { get; private set; }
        public List<Photo> FilteredPhotos { get; private set; }


        public ActivityCenter(AppManager i_AppManager)
        {
            r_UserPhotos = i_AppManager.UserPhotos;
            r_UserPosts = i_AppManager.UserPosts;

            initializeCounts(r_YearCounts, 2010, DateTime.Now.Year);
            initializeCounts(r_MonthCounts, 1, 12);
            initializeCounts(r_HourCounts, 0, 23);

            FilteredPosts = new List<Post>();
            FilteredPhotos = new List<Photo>();

            processPosts();
            processPhotos();
        }

        private void initializeCounts(Dictionary<int, int> i_CountDict, int i_From, int i_To)
        {
            for(int i = i_From; i <= i_To; i++)
            {
                i_CountDict[i] = 0;
            }
        }

        private void processPosts()
        {
            foreach(Post post in r_UserPosts)
            {
                if(post.CreatedTime.HasValue)
                {
                    processTimeData(post.CreatedTime.Value);
                }
            }
        }

        private void processPhotos()
        {
            foreach(Photo photo in r_UserPhotos)
            {
                if(photo.CreatedTime.HasValue)
                {
                    processTimeData(photo.CreatedTime.Value);
                }
            }
        }

        private void processTimeData(DateTime? i_CreatedTime)
        {
            if(i_CreatedTime.HasValue)
            {
                DateTime createdTime = i_CreatedTime.Value;

                if(r_YearCounts.ContainsKey(createdTime.Year))
                {
                    r_YearCounts[createdTime.Year]++;
                }

                if(r_MonthCounts.ContainsKey(createdTime.Month))
                {
                    r_MonthCounts[createdTime.Month]++;
                }

                if(r_HourCounts.ContainsKey(createdTime.Hour))
                {
                    r_HourCounts[createdTime.Hour]++;
                }
            }
        }
        public List<KeyValuePair<int, int>> GetYearCounts(string i_SortBy = "CountDescending")
        {
            return sortCountsList(r_YearCounts, i_SortBy);
        }

        public List<KeyValuePair<int, int>> GetMonthCounts(int i_Year, string i_SortBy = "CountDescending")
        {
            Dictionary<int, int> monthCounts = new Dictionary<int, int>();
            foreach(Post post in r_UserPosts)
            {
                if(post.CreatedTime.HasValue && post.CreatedTime.Value.Year == i_Year)
                {
                    int month = post.CreatedTime.Value.Month;

                    if(!monthCounts.ContainsKey(month))
                    {
                        monthCounts[month] = 0;
                    }

                    monthCounts[month]++;
                }
            }

            foreach(Photo photo in r_UserPhotos)
            {
                if(photo.CreatedTime.HasValue && photo.CreatedTime.Value.Year == i_Year)
                {
                    int month = photo.CreatedTime.Value.Month;

                    if(!monthCounts.ContainsKey(month))
                    {
                        monthCounts[month] = 0;
                    }

                    monthCounts[month]++;
                }
            }

            return sortCountsList(monthCounts, i_SortBy);
        }


        public List<KeyValuePair<int, int>> GetHourCounts(string i_SortBy = "CountDescending")
        {
            Dictionary<int, int> hourCounts = new Dictionary<int, int>();

            foreach(Post post in r_UserPosts)
            {
                if(post.CreatedTime.HasValue)
                {
                    int hour = post.CreatedTime.Value.Hour;
                    if(!hourCounts.ContainsKey(hour))
                    {
                        hourCounts[hour] = 0;
                    }

                    hourCounts[hour]++;
                }
            }

            foreach (Photo photo in r_UserPhotos)
            {
                if (photo.CreatedTime.HasValue)
                {
                    int hour = photo.CreatedTime.Value.Hour;
                    if (!hourCounts.ContainsKey(hour))
                    {
                        hourCounts[hour] = 0;
                    }

                    hourCounts[hour]++;
                }
            }

            return sortCountsList(hourCounts, i_SortBy);
        }

        private List<KeyValuePair<int, int>> sortCountsList(Dictionary<int, int> i_CountsList, string i_SortBy)
        {
            List<KeyValuePair<int, int>> sortedCounts = i_CountsList.ToList();

            switch (i_SortBy)
            {
                case "CountAscending":
                    sortedCounts = sortedCounts.OrderBy(pair => pair.Value).ToList();
                    break;
                case "CountDescending":
                    sortedCounts = sortedCounts.OrderByDescending(pair => pair.Value).ToList();
                    break;
                case "TimeAscending":
                    sortedCounts = sortedCounts.OrderBy(pair => pair.Key).ToList();
                    break;
                case "TimeDescending":
                    sortedCounts = sortedCounts.OrderByDescending(pair => pair.Key).ToList();
                    break;
            }

            return sortedCounts;
        }


        public List<Post> GetPostsByTime(int? i_Year = null, int? i_Month = null, int? i_Hour = null)
        {
            List<Post> filteredPosts = new List<Post>();

            foreach(Post post in r_UserPosts)
            {
                if(post.CreatedTime.HasValue)
                {
                    DateTime createdTime = post.CreatedTime.Value;

                    if((!i_Year.HasValue || createdTime.Year == i_Year)
                       && (!i_Month.HasValue || createdTime.Month == i_Month)
                       && (!i_Hour.HasValue || createdTime.Hour == i_Hour))
                    {
                        filteredPosts.Add(post);
                    }

                }
            }

            return filteredPosts;
        }

        public List<Photo> GetPhotosByTime(int? i_Year = null, int? i_Month = null, int? i_Hour = null)
        {
            List<Photo> filteredPhotos = new List<Photo>();

            foreach(Photo photo in r_UserPhotos)
            {
                if(photo.CreatedTime.HasValue)
                {
                    DateTime createdTime = photo.CreatedTime.Value;

                    if((!i_Year.HasValue || createdTime.Year == i_Year)
                       && (!i_Month.HasValue || createdTime.Month == i_Month)
                       && (!i_Hour.HasValue || createdTime.Hour == i_Hour))
                    {
                        filteredPhotos.Add(photo);
                    }

                }
            }

            return filteredPhotos;
        }


    }

}
