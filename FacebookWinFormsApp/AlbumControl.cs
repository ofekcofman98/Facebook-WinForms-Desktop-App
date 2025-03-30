using FacebookWrapper.ObjectModel;
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
    public partial class AlbumControl : UserControl
    {
        private List<Photo> m_Photos = new List<Photo>();
        private int m_CurrentIndex = 0;


        public AlbumControl()
        {
            InitializeComponent();
            updateButtons();

        }

        public void SetPhotos(List<Photo> i_Photos)
        {
            m_Photos = i_Photos;
            m_CurrentIndex = 0;

            if (m_Photos.Count > 0)
            {
                displayPhoto(m_Photos[m_CurrentIndex]);
            }
            else
            {
                pictureBoxInAlbum.Image = null;
            }

            updateButtons();
        }
        public void ClearPictureBoxInAlbum()
        {
            pictureBoxInAlbum.Image = null;
        }

        private void updateButtons()
        {
            buttonPreviousPhotoInAlbum.Enabled = m_CurrentIndex > 0;
            buttonNextPhotoInAlbum.Enabled = m_CurrentIndex < m_Photos.Count - 1;
        }

        private void displayPhoto(Photo i_Photo)
        {
            try
            {
                if (!string.IsNullOrEmpty(i_Photo.PictureNormalURL))
                {
                    pictureBoxInAlbum.Load(i_Photo.PictureNormalURL);
                }
                else
                {
                    pictureBoxInAlbum.Image = null;
                }
            }
            catch
            {
                pictureBoxInAlbum.Image = null;
            }
        }

        private void buttonNextPhotoInAlbum_Click_1(object sender, EventArgs e)
        {
            if (m_CurrentIndex < m_Photos.Count - 1)
            {
                m_CurrentIndex++;
                displayPhoto(m_Photos[m_CurrentIndex]);
                updateButtons();
            }
        }

        private void buttonPreviousPhotoInAlbum_Click_1(object sender, EventArgs e)
        {
            if (m_CurrentIndex > 0)
            {
                m_CurrentIndex--;
                displayPhoto(m_Photos[m_CurrentIndex]);
                updateButtons();
            }

        }
    }
}
