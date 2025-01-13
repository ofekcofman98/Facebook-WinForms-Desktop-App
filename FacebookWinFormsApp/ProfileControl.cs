using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public partial class ProfileControl : UserControl
    {
        public ProfileControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        public void LoadProfileData()
        {
            labelEmailData.Text = AppManager.Instance.LoggedInUser.Email;
            labelBirthdayData.Text = AppManager.Instance.LoggedInUser.Birthday;
            labelGenderData.Text = AppManager.Instance.LoggedInUser.Gender.ToString();
            labelFullNameData.Text = AppManager.Instance.LoggedInUser.Name;
            PictureBoxMyProfile.Image = AppManager.Instance.LoggedInUser.ImageLarge;
        }

    }
}
