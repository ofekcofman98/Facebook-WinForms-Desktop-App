using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Strategy
{
    public class FetchAlbums : IFetchStrategy<Album>
    {
        public List<Album> Fetch(User i_User)
        {
            return AppManager.Instance.UserAlbums ?? new List<Album>();
        }
    }
}
