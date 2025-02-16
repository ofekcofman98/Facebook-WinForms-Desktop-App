using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public abstract class IFilterBase
    {
        public List<User> FilterUserFriends( User i_SelectedFriendToGetFriendsFrom)
        {

            List<User> filteredUsers = new List<User>();

            foreach (User user in i_SelectedFriendToGetFriendsFrom.Friends)
            {
                if (filter(user))
                {
                    filteredUsers.Add(user);
                }
            }

            return filteredUsers;
        }

        internal protected abstract bool filter(User i_user);

    }
}