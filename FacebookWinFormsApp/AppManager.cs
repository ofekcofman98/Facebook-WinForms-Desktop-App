using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    internal class AppManager
    {
        private readonly string r_AppId = "945333600988492";
        //LoginResult result = FacebookWrapper.FBService.Login("272862089537667",

        private FacebookWrapper.LoginResult m_LoginResult;
        private FacebookWrapper.ObjectModel.User m_LoggedInUser;
        public StatCenter StatCenter { get; set; }
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
                                                             "user_photos",
                                                             "user_posts",
                                                             "user_videos"
                                                         };


        public FacebookWrapper.LoginResult LoginResult
        {
            get
            {  
                return m_LoginResult;
            }
            set
            {
                m_LoginResult = value;
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
                throw new Exception("Please enter a valid App ID."); // ??????
            }

            m_LoginResult = FacebookService.Login(r_AppId, r_Permssions);

            try
            {
                if(string.IsNullOrEmpty(m_LoginResult.ErrorMessage))
                {
                    m_LoggedInUser = m_LoginResult.LoggedInUser;
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

        public string AppId
        {
            get
            {
                return r_AppId;
            }
        }


    }
}
