using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public interface IActivityItem
    {
        DateTime? CreatedTime { get; }
        string Description { get; }
        void ProcessTimeData(Action<DateTime> i_TimeProcessor);
        bool MatchesDateFilter(int? i_Year, int? i_Month, int? i_Hour);

    }
}
