using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public static class FormFactory
    {
        public enum eForm
        {
            FormMain,
            MyProfile,
            ActivityCenter,
            FindFriends
        }

        //public static Form Create(eForm i_Form)
        //{
        //    Form newForm;

        //    switch(i_Form)
        //    {
        //        case eForm.FormMain:
        //            newForm = new FormMain();
        //            break;
        //        case eForm.MyProfile:
        //            newForm = new Form()
        //    }
        //}
    }
}
