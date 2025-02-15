using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class FilterLikedPages : IFilterBase
    {
        private HashSet<String> m_LikedPagesId;
        public FilterLikedPages(HashSet<String> i_LikedPagesId)
        {

            m_LikedPagesId = i_LikedPagesId;
        }

        protected override bool filter(User i_User)
        {
            bool filter = false;
            if (m_LikedPagesId.Count == 0)
            {
                filter = true;
            }
            foreach (Page page in i_User.LikedPages)
            {
                if (m_LikedPagesId.Contains(page.Id))
                {
                    filter = true;
                    continue;
                }

            }

            return filter;
        }
    }
}