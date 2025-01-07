using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    internal class AppFacade
    {
        public void Login()
        {
            try
            {
                AppManager.Instance.Login();

                if (!AppManager.Instance.IsLoggedIn)
                {
                    throw new Exception("Login failed. Please try again.");
                }

            }

            catch(Exception ex) 
            {
                throw new Exception($"An error occurred during login: {ex.Message}");

            }
        }

        public void Logout()
        {
            try
            {
                AppManager.Instance.Logout();

                if (!AppManager.Instance.IsLoggedIn)
                {
                    throw new Exception("Logout failed. Please try again.");
                }

            }

            catch (Exception ex)
            {
                throw new Exception($"An error occurred during logout: {ex.Message}");

            }

        }
    }
}
