using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{

    internal class StatCenter
    {
        public class likerStat
        {
            public string Name { get; set; }
            public int PostsLikedNumber { get; set; }
            public int TotalLikes { get; set; }
        }

        private readonly List<Post> r_UserPosts;

        public StatCenter(List<Post> i_UserPosts)
        {
            if(i_UserPosts != null)
            {
                this.r_UserPosts = i_UserPosts;
            }
        }

        public List<likerStat> CalculateLikes()
        {
            Dictionary<string, likerStat> likersDict = new Dictionary<string, likerStat>();

            foreach(Post post in r_UserPosts)
            {
                if(post.LikedBy != null)
                {
                    foreach(User liker in post.LikedBy)
                    {
                        if(!likersDict.ContainsKey(liker.Name))
                        {
                            likersDict[liker.Name] = new likerStat { Name = liker.Name, PostsLikedNumber = 0, TotalLikes = 0 };
                        }

                        likersDict[liker.Name].PostsLikedNumber++;
                        likersDict[liker.Name].TotalLikes++;
                    }
                }
            }

            return likersDict.Values.ToList();
        }
    }
}
