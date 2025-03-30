using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Strategy
{
    public interface IFetchStrategy<T>
    {
        List<T> Fetch(User i_User);
    }
}
