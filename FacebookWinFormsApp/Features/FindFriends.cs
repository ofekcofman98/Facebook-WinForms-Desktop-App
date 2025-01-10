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
        public HashSet<User> GetFriendUserCommonFriendsPages(List<IFilterable> i_Filters, User i_SelectedFriendToGetFriendsFrom)
        {
            HashSet<User> filterdFriends = new HashSet<User>(i_SelectedFriendToGetFriendsFrom.Friends);

            List<User> usersToRemove = new List<User>();

            foreach (User user in filterdFriends)
            {
                foreach (IFilterable filterable in i_Filters)
                {
                    if (!filterable.Filter(user))
                    {
                        usersToRemove.Add(user); 
                        break; 
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