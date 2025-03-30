using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp.DevTools.WebAudio;
using FacebookWrapper.ObjectModel;
namespace BasicFacebookFeatures
{
    public static class UserUtils
    {
        private static readonly String sr_FacebookDateFormat = "MM/dd/yyyy";
        public static readonly string[] sr_Months =
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };

        public static int GetUserAge(User i_User)
        {
            String birthDateString = i_User.Birthday;

            if (DateTime.TryParseExact(birthDateString, sr_FacebookDateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
            {
                DateTime today = DateTime.Today;
                int age = today.Year - birthDate.Year;
                if (today < birthDate.AddYears(age))
                {
                    age--;
                }

                return age;
            }
            else
            {
                throw new ArgumentException("Invalid date format. Please provide a date in MM/dd/yyyy format.");
            }
        }
    }
}