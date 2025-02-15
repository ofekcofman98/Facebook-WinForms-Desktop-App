using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Patterns.Observer
{
    public interface IUserDataNotifier
    {
        void AttachObserver(IUserDataObserver i_Observer);
        void DetachObserver(IUserDataObserver i_Observer);
        void NotifyObservers();
    }
}
