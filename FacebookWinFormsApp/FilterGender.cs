using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public class FilterGender : Filterable
    {
        User.eGender m_preferanceGender;
        public FilterGender(User.eGender gender)
        {
            m_preferanceGender = gender;
        }
        public bool filter(User user)
        {
            return user.Gender == m_preferanceGender;
        }
    }
}