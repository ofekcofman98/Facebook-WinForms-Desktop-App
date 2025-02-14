using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Strategy
{
    public class FetchFriends : IFetchStrategy<User>
    {
        public List<User> Fetch(User i_User)
        {
            return i_User.Friends?.ToList() ?? new List<User>();
        }
    }

}
