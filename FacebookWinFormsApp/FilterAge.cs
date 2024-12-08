using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class FilterAge : Filterable
    {
        private int m_MinAge;
        private int m_MaxAge;
        public FilterAge(int i_MinAge, int i_MaxAge)
        {
            m_MinAge = i_MinAge;
            m_MaxAge = i_MaxAge;
        }

        public bool filter(User user)
        {
            int age = UserUtils.getUserAge(user);
            return m_MinAge <= age && age <= m_MaxAge;
        }

    }
}
