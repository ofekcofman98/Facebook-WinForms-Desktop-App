using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class FindFriends
    {

        private User m_userLoggedIn;
        private User m_FriendUser;


       public List<User> getFriendUserCommmonFriendsPages(List<String> filters)
        {
        List<User> likedPages = new List<User>();
            foreach (User user in m_FriendUser.Friends)
            {

            }
            return null;
        }
    }
}
