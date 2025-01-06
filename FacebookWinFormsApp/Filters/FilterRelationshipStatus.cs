using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class FilterRelationshipStatus : IFilterable
    {
        private HashSet<User.eRelationshipStatus> m_RelationshipStatusesSet = new HashSet<User.eRelationshipStatus>();
        public FilterRelationshipStatus(HashSet<User.eRelationshipStatus> i_RelationshipStatusesSet)
        {
            m_RelationshipStatusesSet = i_RelationshipStatusesSet;
        }
        public bool Filter(User i_User)
        {
            if (m_RelationshipStatusesSet.Count == 0)
            {
                return true;
            }

            return m_RelationshipStatusesSet.Contains((User.eRelationshipStatus)i_User.RelationshipStatus);
        }
    }
}