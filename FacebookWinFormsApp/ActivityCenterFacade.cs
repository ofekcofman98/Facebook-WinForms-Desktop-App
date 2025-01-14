using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{

    internal class ActivityCenterFacade
    {
        public readonly Dictionary<eSortingType, string> r_SortingDictionary = new Dictionary<eSortingType, string>()
                                                                                    {
                                                                                        { eSortingType.TimeAscending, "Sort by Time Ascending" },
                                                                                        { eSortingType.TimeDescending, "Sort by Time Descending" },
                                                                                        { eSortingType.CountAscending, "Sort by Count Ascending" },
                                                                                        { eSortingType.CountDescending, "Sort by Count Descending" }
                                                                                    };


        //public List<string> GetYearCounts(eSortingType i_SortingType )
        //{
        //    List<KeyValuePair<int, int>> yearCounts = AppManager.Instance.ActivityCenter.GetYearCounts();
        //    List<string> result = new List<string>();

        //    foreach (KeyValuePair<int, int> year in yearCounts)
        //    {
        //        result.Add(year.Key + ": " + year.Value + " posts/photos");
        //    }

        //    return result;
        //}


    }
}
