using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{

    internal class ActivityStats
    {
        public class likerStat
        {
            public string Period { get; set; }
            public int PostsCount { get; set; }
            public int PhotosCount { get; set; }
            public int TotalCount { get; set; }
        }

        private readonly List<Post> r_UserPosts;
        private readonly List<Photo> r_UserPhotos;

        public ActivityStats(List<Post> i_UserPosts, List<Photo> r_userPhotos)
        {
            if (i_UserPosts != null)
            {
                this.r_UserPosts = i_UserPosts;
            }
            if (r_userPhotos != null)
            {
                this.r_UserPhotos = r_userPhotos;
            }
        }

        public void CheckTime()
        {
            foreach(Post post in r_UserPosts)
            {
                if(post.CreatedTime.HasValue)
                {
                    DateTime creationTime = post.CreatedTime.Value;
                    Console.WriteLine($"Post created on: {creationTime}");
                }

                else
                {
                    Console.WriteLine("Post does not have a creation time.");
                }
            }
        }

        /*public List<likerStat> CalculateLikes()
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
                            likersDict[liker.Name] = new likerStat
                                                         {
                                                             Name = liker.Name,
                                                             PostsLikedNumber = 0,
                                                             PhotosLikedNumber = 0, 
                                                             TotalLikes = 0
                                                         };
                        }

                        likersDict[liker.Name].PostsLikedNumber++;
                        likersDict[liker.Name].TotalLikes++;
                    }
                }
            }

            foreach (Photo photo in r_UserPhotos)
            {
                if (photo != null)
                {
                    foreach (User liker in photo.LikedBy)
                    {
                        if (!likersDict.ContainsKey(liker.Name))
                        {
                            likersDict[liker.Name] = new likerStat
                                                         {
                                                             Name = liker.Name,
                                                             PostsLikedNumber = 0,
                                                             PhotosLikedNumber = 0,
                                                             TotalLikes = 0
                                                         };
                        }

                        likersDict[liker.Name].PhotosLikedNumber++;
                        likersDict[liker.Name].TotalLikes++;
                    }
                }
            }


            return likersDict.Values.ToList();
        }*/

        /*public List<likerStat> SortLikers(List<likerStat> i_Likers, string i_SortBy, bool isDescending = true)
        {
            switch (i_SortBy.ToLower())
            {
                case "postslikednumber":
                    if (isDescending)
                    {
                        i_Likers.Sort((liker1, liker2) => liker2.PostsLikedNumber.CompareTo(liker1.PostsLikedNumber));
                    }
                    else
                    {
                        i_Likers.Sort((liker1, liker2) => liker1.PostsLikedNumber.CompareTo(liker2.PostsLikedNumber));
                    }
                    break;

                case "photoslikednumber":
                    if (isDescending)
                    {
                        i_Likers.Sort((liker1, liker2) => liker2.PhotosLikedNumber.CompareTo(liker1.PhotosLikedNumber));
                    }
                    else
                    {
                        i_Likers.Sort((liker1, liker2) => liker1.PhotosLikedNumber.CompareTo(liker2.PhotosLikedNumber));
                    }
                    break;

                case "totallikes":
                    if (isDescending)
                    {
                        i_Likers.Sort((liker1, liker2) => liker2.TotalLikes.CompareTo(liker1.TotalLikes));
                    }
                    else
                    {
                        i_Likers.Sort((liker1, liker2) => liker1.TotalLikes.CompareTo(liker2.TotalLikes));
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid sort option. Use 'postslikednumber', 'photoslikednumber', or 'totallikes'.");
            }

            return i_Likers;
        }*/
    }
}
