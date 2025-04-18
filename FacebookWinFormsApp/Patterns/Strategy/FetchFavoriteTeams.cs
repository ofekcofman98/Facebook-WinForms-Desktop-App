﻿using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.Strategy
{
    public class FetchFavoriteTeams : IFetchStrategy<Page> 
    {
        public List<Page> Fetch(User i_User)
        {
            return i_User.FavofriteTeams?.ToList() ?? new List<Page>();
        }
    }
}
