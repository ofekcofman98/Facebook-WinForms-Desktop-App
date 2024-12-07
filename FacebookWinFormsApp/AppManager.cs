using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    internal class AppManager
    {
        private readonly string r_AppId = "945333600988492";
        private FacebookWrapper.LoginResult m_LoginResult;
        private FacebookWrapper.ObjectModel.User m_LoggedInUser;
        public ActivityCenter ActivityCenter { get; set; }
        private readonly string[] r_Permssions = new string[]
                                                         {
                                                             "email",
                                                             "public_profile",
                                                             "user_age_range",
                                                             "user_birthday",
                                                             "user_events",
                                                             "user_friends",
                                                             "user_gender",
                                                             "user_hometown",
                                                             "user_likes",
                                                             "user_link",
                                                             "user_location",
                                                             //"user_reactions",//
                                                             "user_photos",
                                                             "user_posts",
                                                             "user_videos"
                                                         };

        public List<Post> UserPosts { get; set; }
        public List<Photo> UserPhotos { get; set; }


        public string AppId
        {
            get
            {
                return r_AppId;
            }
        }

        public FacebookWrapper.LoginResult LoginResult
        {
            get
            {  
                return m_LoginResult;
            }
        }
        public FacebookWrapper.ObjectModel.User LoggedInUser
        {
            get
            {
                return m_LoggedInUser;
            }
        }


        public void Login()
        {
            if (string.IsNullOrEmpty((r_AppId)))
            {
                throw new Exception("Please enter a valid App ID."); 
            }

            m_LoginResult = FacebookService.Login(r_AppId, r_Permssions);

            try
            {
                if(string.IsNullOrEmpty(m_LoginResult.ErrorMessage))
                {
                    m_LoggedInUser = m_LoginResult.LoggedInUser;
                    getUserData();
                    ActivityCenter = new ActivityCenter(this);
                }
                else
                {
                    throw new Exception($"Login failed: {m_LoginResult.ErrorMessage}");
                }

            }
            catch(Exception exception)
            {
                throw new Exception($"An error occurred during login: {exception.Message}");
            }
        }

        private void getUserData()
        {
            UserPosts = m_LoggedInUser.Posts.ToList();
            UserPhotos = GetPhotos();
        }

        public List<Photo> GetPhotos()
        {
            List<Photo> photos = new List<Photo>();
            if(m_LoggedInUser.Albums != null)
            {
                foreach(FacebookWrapper.ObjectModel.Album album in m_LoggedInUser.Albums)
                {
                    if(album.Photos != null)
                    {
                        foreach(Photo photo in album.Photos)
                        {
                            photos.Add(photo);
                        }
                    }
                }
            }

            return photos;
        }

        public void Logout()
        {
            
            FacebookService.LogoutWithUI();
            m_LoginResult = null;
            m_LoggedInUser = null;
            UserPhotos = null;
            UserPosts = null;
        }



    }
}
