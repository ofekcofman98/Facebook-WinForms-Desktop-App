using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class FetchContext<T>
    {
        private IFetchStrategy<T> m_FetchStrategy;

        public FetchContext(IFetchStrategy<T> i_FetchStrategy)
        {
            m_FetchStrategy = i_FetchStrategy;
        }

        public List<T> ExecuteFetch(User i_LoggedInUser)
        {
            return m_FetchStrategy?.Fetch(i_LoggedInUser) ?? new List<T>();
        }
    }
}
