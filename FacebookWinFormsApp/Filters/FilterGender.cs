using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public class FilterGender : IFilterable
    {
        private User.eGender m_PreferanceGender;
        public FilterGender(User.eGender i_Gender)
        {
            m_PreferanceGender = i_Gender;
        }
        public bool Filter(User i_User)
        {
            return i_User.Gender == m_PreferanceGender;
        }
    }
}