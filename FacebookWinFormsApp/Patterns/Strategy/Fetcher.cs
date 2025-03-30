using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Strategy
{
    public class Fetcher<T>
    {
        private IFetchStrategy<T> m_Strategy;

        public Fetcher(IFetchStrategy<T> i_Strategy)
        {
            m_Strategy = i_Strategy;
        }

        public List<T> FetchData(User i_User)
        {
            return m_Strategy.Fetch(i_User);
        }

        public void SetStrategy(IFetchStrategy<T> i_Strategy)
        {
            m_Strategy = i_Strategy;
        }
    }
}
