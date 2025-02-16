using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BasicFacebookFeatures.Patterns.Observer;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public sealed class AppManager : IUserDataNotifier
    {
        private readonly string r_AppId = "945333600988492";
        private LoginResult m_LoginResult;
        private User m_LoggedInUser;
        private readonly List<IUserDataObserver> r_Observers = new List<IUserDataObserver>();

        public ActivityCenter ActivityCenter { get; private set; }


        public bool IsLoggedIn { get; private set; } = false;
        private readonly string[] r_Permissions = new string[]
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
                                                             "user_photos",
                                                             "user_posts",
                                                             "user_videos"
                                                         };

        public List<Post> UserPosts { get; set; }
        public List<Photo> UserPhotos { get; set; }
        public List<Album> UserAlbums { get; private set; }
        

        private AppManager()
        {
            ActivityCenter = new ActivityCenter();
        }

        public static AppManager Instance
        {
            get
            {
                return Singleton<AppManager>.Instance;
            }
        }


        public LoginResult LoginResult
        {
            get
            {  
                return m_LoginResult;
            }
        }

        public User LoggedInUser
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

            m_LoginResult = FacebookService.Login(r_AppId, r_Permissions);
            try
            {
                if(string.IsNullOrEmpty(m_LoginResult.ErrorMessage))
                {
                    IsLoggedIn = true;
                    if (m_LoggedInUser == null)
                    {
                        m_LoggedInUser = m_LoginResult.LoggedInUser;
                    }

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

        public void GetUserData()
        {
            UserPosts = m_LoggedInUser.Posts.ToList();
            UserPhotos = GetPhotos();

            List<IActivityItem> activityItems = new List<IActivityItem>();

            foreach (Photo photo in UserPhotos)
            {
                activityItems.Add(new PhotoAdapter(photo));
            }

            foreach(Post post in UserPosts)
            {
                activityItems.Add(new PostAdapter(post));
            }

            ActivityCenter.InitializeUserData(activityItems);
            NotifyObservers();

        }

        public List<Photo> GetPhotos()
        {
            List<Photo> photos = new List<Photo>();
            if(m_LoggedInUser.Albums != null)
            {
                UserAlbums = m_LoggedInUser.Albums?.ToList() ?? new List<Album>();

                foreach (Album album in m_LoggedInUser.Albums)
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
            ActivityCenter.ResetCounts();
            IsLoggedIn = false; 
        }

        public void AttachObserver(IUserDataObserver i_Observer)
        {
            if (!r_Observers.Contains(i_Observer))
            {
                r_Observers.Add(i_Observer);
            }
        }

        public void DetachObserver(IUserDataObserver i_Observer)
        {
            r_Observers.Remove(i_Observer);
        }

        public void NotifyObservers()
        {
            foreach (IUserDataObserver observer in r_Observers)
            {
                observer.OnUserDataUpdated(m_LoggedInUser);
            }
        }
    }
}
