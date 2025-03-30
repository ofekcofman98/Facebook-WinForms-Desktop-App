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
        private List<IActivityItem> m_ActivityItems;

        private readonly Dictionary<int, int> r_YearCounts = new Dictionary<int, int>();
        private readonly Dictionary<int, int> r_MonthCounts = new Dictionary<int, int>();
        private readonly Dictionary<int, int> r_HourCounts = new Dictionary<int, int>();

        private const int k_StartYear = 2010;
        private const int k_StartMonth = 1;
        private const int k_EndMonth = 12;
        private const int k_StartHour = 0;
        private const int k_EndHour = 23;

        public List<IActivityItem> FilteredActivityItems { get; private set; }

        public readonly Dictionary<eSortingType, string> r_SortingDictionary = new Dictionary<eSortingType, string>()
                                                                                   {
                                                                                       { eSortingType.TimeAscending, "Sort by Time Ascending" },
                                                                                       { eSortingType.TimeDescending, "Sort by Time Descending" },
                                                                                       { eSortingType.CountAscending, "Sort by Count Ascending" },
                                                                                       { eSortingType.CountDescending, "Sort by Count Descending" }
                                                                                   };

        public ActivityCenter()
        {
            initializeCounts(r_YearCounts, k_StartYear, DateTime.Now.Year);
            initializeCounts(r_MonthCounts, k_StartMonth, k_EndMonth);
            initializeCounts(r_HourCounts, k_StartHour, k_EndHour);

            FilteredActivityItems = new List<IActivityItem>();
        }

        public void InitializeUserData(List<IActivityItem> i_ActivityItems)
        {
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

        public void ResetCounts()
        {
            resetCountDictionary(r_YearCounts);
            resetCountDictionary(r_HourCounts);
            resetCountDictionary(r_MonthCounts);
        }

        private void resetCountDictionary(Dictionary<int, int> i_CountDict)
        {
            List<int> keys = i_CountDict.Keys.ToList();
            foreach (int key in keys)
            {
                i_CountDict[key] = 0;
            }
        }

    }

}
