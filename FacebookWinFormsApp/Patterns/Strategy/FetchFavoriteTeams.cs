using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.Patterns.Strategy
{
    internal class FetchFavoriteTeams : IFetchStrategy<Page>
    {
        public List<Page> Fetch(User i_LoggedInUser)
        {
            return i_LoggedInUser?.FavofriteTeams?.ToList() ?? new List<Page>();
        }
    }
}
