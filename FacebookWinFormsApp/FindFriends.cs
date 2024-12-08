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


        public HashSet<User> getFriendUserCommmonFriendsPages(List<Filterable> i_filters, User i_SelectedFriendToGetFriendsFrom)
        {
            HashSet<User> filterdFriends = new HashSet<User>(i_SelectedFriendToGetFriendsFrom.Friends);

            List<User> usersToRemove = new List<User>();

            foreach (User user in filterdFriends)
            {
                // Check each filter for the current user
                foreach (Filterable filterable in i_filters)
                {
                    if (!filterable.filter(user))
                    {
                        usersToRemove.Add(user); // Collect user for removal
                        break; // No need to check further filters if one fails
                    }
                }
            }

            foreach (User user in usersToRemove)
            {
                filterdFriends.Remove(user);
            }

            return filterdFriends;
        }
    }
}