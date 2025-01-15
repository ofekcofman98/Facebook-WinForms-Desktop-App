using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public enum eSortingType
    {
        TimeAscending,
        TimeDescending,
        CountAscending,
        CountDescending, // also default
    }

    public class ActivityCenter
    {

        //private List<Post> m_UserPosts;
        //private List<Photo> m_UserPhotos;

        private List<IActivityItem> m_ActivityItems;

        private readonly Dictionary<int, int> r_YearCounts = new Dictionary<int, int>();
        private readonly Dictionary<int, int> r_MonthCounts = new Dictionary<int, int>();
        private readonly Dictionary<int, int> r_HourCounts = new Dictionary<int, int>();

        //public List<Post> FilteredPosts { get; private set; }
        //public List<Photo> FilteredPhotos { get; private set; }

        public List<IActivityItem> FilteredActivityItems { get; private set; }

        public ActivityCenter()
        {
            initializeCounts(r_YearCounts, 2010, DateTime.Now.Year);
            initializeCounts(r_MonthCounts, 1, 12);
            initializeCounts(r_HourCounts, 0, 23);

            //FilteredPosts = new List<Post>();
            //FilteredPhotos = new List<Photo>();

            FilteredActivityItems = new List<IActivityItem>();
        }

        public void InitializeUserData(/*List<Post> i_UserPosts, List<Photo> i_UserPhotos*/ List<IActivityItem> i_ActivityItems)
        {
            //m_UserPosts = i_UserPosts;
            //m_UserPhotos = i_UserPhotos;

            //m_ActivityItems = new List<IActivityItem>();
            //foreach(Photo photo in i_UserPhotos)
            //{
            //    m_ActivityItems.Add(new PhotoAdapter(photo));
            //}

            //foreach(Post post in i_UserPosts)
            //{
            //    m_ActivityItems.Add(new PostAdapter(post));    
            //}

            //processPosts();
            //processPhotos();

            m_ActivityItems = i_ActivityItems;
            processActivityItems();
        }

        private void processActivityItems()
        {
            foreach(IActivityItem item in m_ActivityItems)
            {
                item.ProcessTimeData(processTimeData);
            }
        }

        private void processTimeData(DateTime i_CreatedTime)
        {
            if(r_YearCounts.ContainsKey(i_CreatedTime.Year))
            {
                r_YearCounts[i_CreatedTime.Year]++;
            }

            if (r_MonthCounts.ContainsKey(i_CreatedTime.Month))
            {
                r_MonthCounts[i_CreatedTime.Month]++;
            }
            
            if (r_HourCounts.ContainsKey(i_CreatedTime.Hour))
            {
                r_HourCounts[i_CreatedTime.Hour]++;
            }
        }

        private void initializeCounts(Dictionary<int, int> i_CountDict, int i_From, int i_To)
        {
            for(int i = i_From; i <= i_To; i++)
            {
                i_CountDict[i] = 0;
            }
        }

        //private void processPosts()
        //{
        //    foreach(Post post in m_UserPosts)
        //    {
        //        if(post.CreatedTime.HasValue)
        //        {
        //            processTimeData(post.CreatedTime.Value);
        //        }
        //    }
        //}

        //private void processPhotos()
        //{
        //    foreach(Photo photo in m_UserPhotos)
        //    {
        //        if(photo.CreatedTime.HasValue)
        //        {
        //            processTimeData(photo.CreatedTime.Value);
        //        }
        //    }
        //}

        //private void processTimeData(DateTime? i_CreatedTime)
        //{
        //    if(i_CreatedTime.HasValue)
        //    {
        //        DateTime createdTime = i_CreatedTime.Value;

        //        if(r_YearCounts.ContainsKey(createdTime.Year))
        //        {
        //            r_YearCounts[createdTime.Year]++;
        //        }

        //        if(r_MonthCounts.ContainsKey(createdTime.Month))
        //        {
        //            r_MonthCounts[createdTime.Month]++;
        //        }

        //        if(r_HourCounts.ContainsKey(createdTime.Hour))
        //        {
        //            r_HourCounts[createdTime.Hour]++;
        //        }
        //    }
        //}
        public List<KeyValuePair<int, int>> GetYearCounts(eSortingType i_SortBy)
        {
            return sortCountsList(r_YearCounts, i_SortBy);
        }

        public List<KeyValuePair<int, int>> GetMonthCounts(int i_Year, eSortingType i_SortBy)
        {
            Dictionary<int, int> monthCounts = new Dictionary<int, int>();

            foreach(IActivityItem item in m_ActivityItems)
            {
                if(item.CreatedTime.HasValue && item.CreatedTime.Value.Year == i_Year)
                {
                    int month = item.CreatedTime.Value.Month;

                    if(!monthCounts.ContainsKey(month))
                    {
                        monthCounts[month] = 0;
                    }

                    monthCounts[month]++;
                }
            }

            //foreach(Post post in m_UserPosts)
            //{
            //    if(post.CreatedTime.HasValue && post.CreatedTime.Value.Year == i_Year)
            //    {
            //        int month = post.CreatedTime.Value.Month;

            //        if(!monthCounts.ContainsKey(month))
            //        {
            //            monthCounts[month] = 0;
            //        }

            //        monthCounts[month]++;
            //    }
            //}

            //foreach(Photo photo in m_UserPhotos)
            //{
            //    if(photo.CreatedTime.HasValue && photo.CreatedTime.Value.Year == i_Year)
            //    {
            //        int month = photo.CreatedTime.Value.Month;

            //        if(!monthCounts.ContainsKey(month))
            //        {
            //            monthCounts[month] = 0;
            //        }

            //        monthCounts[month]++;
            //    }
            //}

            return sortCountsList(monthCounts, i_SortBy);
        }


        public List<KeyValuePair<int, int>> GetHourCounts(eSortingType i_SortBy)
        {
            Dictionary<int, int> hourCounts = new Dictionary<int, int>();

            foreach (IActivityItem item in m_ActivityItems)
            {
                if (item.CreatedTime.HasValue)
                {
                    int hour = item.CreatedTime.Value.Hour;
                    if (!hourCounts.ContainsKey(hour))
                    {
                        hourCounts[hour] = 0;
                    }

                    hourCounts[hour]++;
                }
            }

            //foreach (Post post in m_UserPosts)
            //{
            //    if(post.CreatedTime.HasValue)
            //    {
            //        int hour = post.CreatedTime.Value.Hour;
            //        if(!hourCounts.ContainsKey(hour))
            //        {
            //            hourCounts[hour] = 0;
            //        }

            //        hourCounts[hour]++;
            //    }
            //}

            //foreach (Photo photo in m_UserPhotos)
            //{
            //    if (photo.CreatedTime.HasValue)
            //    {
            //        int hour = photo.CreatedTime.Value.Hour;
            //        if (!hourCounts.ContainsKey(hour))
            //        {
            //            hourCounts[hour] = 0;
            //        }

            //        hourCounts[hour]++;
            //    }
            //}

            return sortCountsList(hourCounts, i_SortBy);
        }

        private List<KeyValuePair<int, int>> sortCountsList(Dictionary<int, int> i_CountsList, eSortingType i_SortBy)
        {
            List<KeyValuePair<int, int>> sortedCounts = i_CountsList.ToList();

            switch (i_SortBy)
            {
                case eSortingType.CountAscending:
                    sortedCounts = sortedCounts.OrderBy(pair => pair.Value).ToList();
                    break;
                case eSortingType.CountDescending:
                    sortedCounts = sortedCounts.OrderByDescending(pair => pair.Value).ToList();
                    break;
                case eSortingType.TimeAscending:
                    sortedCounts = sortedCounts.OrderBy(pair => pair.Key).ToList();
                    break;
                case eSortingType.TimeDescending:
                    sortedCounts = sortedCounts.OrderByDescending(pair => pair.Key).ToList();
                    break;
            }

            return sortedCounts;
        }

        public List<IActivityItem> GetItemsByTime(int? i_Year = null, int? i_Month = null, int? i_Hour = null)
        {
            List<IActivityItem> filteredItems = new List<IActivityItem>();

            foreach(IActivityItem item in m_ActivityItems)
            {
                if(item.MatchesDateFilter(i_Year, i_Month, i_Hour))
                {
                    filteredItems.Add(item);
                }
            }

            return filteredItems;
        }

        //public List<Post> GetPostsByTime(int? i_Year = null, int? i_Month = null, int? i_Hour = null)
        //{
        //    List<Post> filteredPosts = new List<Post>();

        //    foreach(Post post in m_UserPosts)
        //    {
        //        if(post.CreatedTime.HasValue)
        //        {
        //            DateTime createdTime = post.CreatedTime.Value;

        //            if((!i_Year.HasValue || createdTime.Year == i_Year)
        //               && (!i_Month.HasValue || createdTime.Month == i_Month)
        //               && (!i_Hour.HasValue || createdTime.Hour == i_Hour))
        //            {
        //                filteredPosts.Add(post);
        //            }

        //        }
        //    }

        //    return filteredPosts;
        //}

        //public List<Photo> GetPhotosByTime(int? i_Year = null, int? i_Month = null, int? i_Hour = null)
        //{
        //    List<Photo> filteredPhotos = new List<Photo>();

        //    foreach(Photo photo in m_UserPhotos)
        //    {
        //        if(photo.CreatedTime.HasValue)
        //        {
        //            DateTime createdTime = photo.CreatedTime.Value;

        //            if((!i_Year.HasValue || createdTime.Year == i_Year)
        //               && (!i_Month.HasValue || createdTime.Month == i_Month)
        //               && (!i_Hour.HasValue || createdTime.Hour == i_Hour))
        //            {
        //                filteredPhotos.Add(photo);
        //            }

        //        }
        //    }

        //    return filteredPhotos;
        //}


    }

}
