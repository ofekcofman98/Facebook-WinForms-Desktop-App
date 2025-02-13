using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Patterns.Strategy
{
    internal class FetchAlbums : IFetchStrategy<Album>
    {
        public List<Album> Fetch(User i_LoggedInUser)
        {
            return i_LoggedInUser?.Albums?.ToList() ?? new List<Album>();
        }
    }
}
