using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class FilterLikedPages : Filterable
    {
        private HashSet<String> m_LikedPagesId;
        public FilterLikedPages(HashSet<String> i_LikedPagesId)
        {

            m_LikedPagesId = i_LikedPagesId;
        }

        public bool filter(User user)
        {
            bool filter = false;
            if (m_LikedPagesId.Count == 0)
            {
                filter = true;
            }
            foreach (Page page in user.LikedPages)
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