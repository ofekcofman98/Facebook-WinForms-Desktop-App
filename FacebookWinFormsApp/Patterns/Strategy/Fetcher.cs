using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Patterns.Strategy
{
    internal class Fetcher<T>
    {
        public IFetchStrategy<T> FetchStrategy { get; set; }
        public Fetcher(IFetchStrategy<T> i_FetchStrategy)
        {
            FetchStrategy = i_FetchStrategy;
        }

        public List<T> ExecuteFetch(User i_LoggedInUser)
        {
            return FetchStrategy.Fetch(i_LoggedInUser);
        }

    }
}
