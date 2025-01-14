using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public class PostAdapter : IActivityItem
    {
        public Post Post { get; }

        public PostAdapter(Post i_Post)
        {
            Post = i_Post;
        }

        public DateTime? CreatedTime
        {
            get { return Post.CreatedTime; }
        }

        public string Description
        {
            get { return string.IsNullOrEmpty(Post.Description) ? "[No message]" : Post.Description; }
        }

        public void ProcessTimeData(Action<DateTime> i_TimeProcessor)
        {
            if (Post.CreatedTime.HasValue)
            {
                i_TimeProcessor.Invoke(Post.CreatedTime.Value);
            }
        }

        public bool MatchesDateFilter(int? i_Year, int? i_Month, int? i_Hour)
        {
            bool result = false;
            if (Post.CreatedTime.HasValue)
            {
                DateTime createdTime = Post.CreatedTime.Value;
                if ((!i_Year.HasValue || createdTime.Year == i_Year)
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
