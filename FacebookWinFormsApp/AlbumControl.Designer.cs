﻿namespace BasicFacebookFeatures
{
    partial class AlbumControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonNextPhotoInAlbum = new System.Windows.Forms.Button();
            this.buttonPreviousPhotoInAlbum = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxInAlbum = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInAlbum)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonNextPhotoInAlbum);
            this.panel1.Controls.Add(this.buttonPreviousPhotoInAlbum);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 285);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 65);
            this.panel1.TabIndex = 77;
            // 
            // buttonNextPhotoInAlbum
            // 
            this.buttonNextPhotoInAlbum.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonNextPhotoInAlbum.Location = new System.Drawing.Point(238, 0);
            this.buttonNextPhotoInAlbum.Name = "buttonNextPhotoInAlbum";
            this.buttonNextPhotoInAlbum.Size = new System.Drawing.Size(52, 65);
            this.buttonNextPhotoInAlbum.TabIndex = 74;
            this.buttonNextPhotoInAlbum.Text = ">";
            this.buttonNextPhotoInAlbum.UseVisualStyleBackColor = true;
            this.buttonNextPhotoInAlbum.Click += new System.EventHandler(this.buttonNextPhotoInAlbum_Click_1);
            // 
            // buttonPreviousPhotoInAlbum
            // 
            this.buttonPreviousPhotoInAlbum.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonPreviousPhotoInAlbum.Location = new System.Drawing.Point(0, 0);
            this.buttonPreviousPhotoInAlbum.Name = "buttonPreviousPhotoInAlbum";
            this.buttonPreviousPhotoInAlbum.Size = new System.Drawing.Size(52, 65);
            this.buttonPreviousPhotoInAlbum.TabIndex = 75;
            this.buttonPreviousPhotoInAlbum.Text = "<";
            this.buttonPreviousPhotoInAlbum.UseVisualStyleBackColor = true;
            this.buttonPreviousPhotoInAlbum.Click += new System.EventHandler(this.buttonPreviousPhotoInAlbum_Click_1);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxInAlbum, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(296, 353);
            this.tableLayoutPanel1.TabIndex = 78;
            // 
            // pictureBoxInAlbum
            // 
            this.pictureBoxInAlbum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxInAlbum.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxInAlbum.Name = "pictureBoxInAlbum";
            this.pictureBoxInAlbum.Size = new System.Drawing.Size(290, 276);
            this.pictureBoxInAlbum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxInAlbum.TabIndex = 76;
            this.pictureBoxInAlbum.TabStop = false;
            // 
            // AlbumControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AlbumControl";
            this.Size = new System.Drawing.Size(296, 353);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInAlbum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonNextPhotoInAlbum;
        private System.Windows.Forms.Button buttonPreviousPhotoInAlbum;
        private System.Windows.Forms.PictureBox pictureBoxInAlbum;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
