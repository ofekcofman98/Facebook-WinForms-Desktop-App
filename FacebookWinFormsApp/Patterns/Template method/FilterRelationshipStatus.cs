using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class FilterRelationshipStatus : IFilterBase
    {
        private readonly HashSet<User.eRelationshipStatus> r_RelationshipStatusSet ;
        public FilterRelationshipStatus(HashSet<User.eRelationshipStatus> i_RelationshipStatusesSet)
        {
            r_RelationshipStatusSet = i_RelationshipStatusesSet;
        }
        internal protected override bool filter(User i_User)
        {
            if (r_RelationshipStatusSet.Count == 0)
            {
                return true;
            }
            

            return r_RelationshipStatusSet.Contains((User.eRelationshipStatus)i_User.RelationshipStatus);
        }
    }
}