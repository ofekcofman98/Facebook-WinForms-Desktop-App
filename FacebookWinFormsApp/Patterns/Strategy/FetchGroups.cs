using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Strategy
{
    public class FetchGroups : IFetchStrategy<Group>
    {
        public List<Group> Fetch(User i_User)
        {
            return i_User.Groups?.ToList() ?? new List<Group>();
        }
    }
}
