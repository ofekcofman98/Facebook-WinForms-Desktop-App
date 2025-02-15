using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Patterns.Observer
{
    public interface IUserDataObserver
    {
        void OnUserDataUpdated(User i_User);
    }
}
