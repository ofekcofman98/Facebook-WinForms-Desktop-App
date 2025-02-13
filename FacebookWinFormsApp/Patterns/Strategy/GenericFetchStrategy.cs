using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class GenericFetchStrategy<T> : IFetchStrategy<T>
    {
        private readonly Func<User, List<T>> m_FetchFunction;

        public GenericFetchStrategy(Func<User, List<T>> i_FetchFunction)
        {
            m_FetchFunction = i_FetchFunction;
        }

        public List<T> Fetch(User i_LoggedInUser)
        {
            return m_FetchFunction?.Invoke(i_LoggedInUser) ?? new List<T>();
        }
    }
    
}
