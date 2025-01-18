using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class FindFriends
    {
        public HashSet<User> GetFriendUserCommonFriendsPages(IFilterable i_filter, User i_SelectedFriendToGetFriendsFrom)
        {

            HashSet<User> filteredUsers = new HashSet<User>();

            foreach (User user in i_SelectedFriendToGetFriendsFrom.Friends)
            {
                if (i_filter.Filter(user))
                {
                    filteredUsers.Add(user);
                }
            }

            return filteredUsers;
        }
    }
}