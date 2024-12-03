using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    internal class AppManager
    {
        private readonly string r_AppId = "945333600988492"; 
        //LoginResult result = FacebookWrapper.FBService.Login("272862089537667",
        public StatCenter StatCenter { get; set; }

        public string AppId
        {
            get
            {
                return r_AppId;
            }
        }


    }
}
