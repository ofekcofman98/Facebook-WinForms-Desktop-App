using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class Filters : IFilterBase
    {
        private readonly List<IFilterBase> r_filters = new List<IFilterBase>();

        public void AddFilter(IFilterBase filter)
        {
            r_filters.Add(filter);
        }
        
        protected override bool filter(User i_User)
        {
            bool result = false;
            foreach (IFilterable filter in r_filters)
            {
                result = filter.Filter(i_User);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }
    }
}
