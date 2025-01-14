using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public class PhotoAdapter : IActivityItem
    {
        public Photo Photo { get; }

        public PhotoAdapter(Photo i_Photo)
        {
            Photo = i_Photo;
        }

        public DateTime? CreatedTime
        {
            get { return Photo.CreatedTime; }
        }

        public string Description
        {
            get { return string.IsNullOrEmpty(Photo.Name) ? "[No title]" : Photo.Name; }
        }

        public void ProcessTimeData(Action<DateTime> i_TimeProcessor)
        {
            if(Photo.CreatedTime.HasValue)
            {
                i_TimeProcessor.Invoke(Photo.CreatedTime.Value);
            }
        }

        public bool MatchesDateFilter(int? i_Year, int? i_Month, int? i_Hour)
        {
            bool result = false;
            if(Photo.CreatedTime.HasValue)
            {
                DateTime createdTime = Photo.CreatedTime.Value;
                if((!i_Year.HasValue || createdTime.Year == i_Year)
                   && (!i_Month.HasValue || createdTime.Month == i_Month)
                   && (!i_Hour.HasValue || createdTime.Hour == i_Hour))
                {
                    result = true;
                }
            }

            return result;
        }


    }
}
