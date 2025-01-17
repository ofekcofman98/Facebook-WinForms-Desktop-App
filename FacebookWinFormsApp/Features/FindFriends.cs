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
            HashSet<User> filterdFriends = new HashSet<User>(i_SelectedFriendToGetFriendsFrom.Friends);

            List<User> filteredUsers = new List<User>();

            foreach (User user in filterdFriends)
            {
                if (i_filter.Filter(user))
                {
                    filterdFriends.Add(user);
                }
            }

            return filterdFriends;
        }
    }
}