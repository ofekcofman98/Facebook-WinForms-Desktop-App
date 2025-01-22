using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Filters
{
    public class CompositeFilter : IFilterable
    {
        private readonly List<IFilterable> r_filters = new List<IFilterable>();

        public void AddFilter(IFilterable filter)
        {
            r_filters.Add(filter);
        }

        public bool Filter(User i_User)
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
