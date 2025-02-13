using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Patterns.Strategy
{
    internal class FetchFriends : IFetchStrategy<User>
    {
        public List<User> Fetch(User i_LoggedInUser)
        {
            return i_LoggedInUser?.Friends?.ToList() ?? new List<User>();
        }
    }
}
