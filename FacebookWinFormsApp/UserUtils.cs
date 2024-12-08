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
        private static readonly String facebookDateFormat = "MM/dd/yyyy";
        public static int getUserAge(User user)
        {
            String birthDateString = user.Birthday;

            if (DateTime.TryParseExact(birthDateString, facebookDateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
            {
                DateTime today = DateTime.Today;

                // Calculate age
                int age = today.Year - birthDate.Year;

                // Adjust for cases where the birthday hasn't occurred yet this year
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