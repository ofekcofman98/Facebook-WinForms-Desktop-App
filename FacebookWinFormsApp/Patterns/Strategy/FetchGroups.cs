using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Patterns.Strategy
{
    internal class FetchGroups : IFetchStrategy<Group>
    {
        public List<Group> Fetch(User i_LoggedInUser)
        {
            return i_LoggedInUser?.Groups?.ToList() ?? new List<Group>();
        }

    }
}
