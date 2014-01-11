using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TAlex.WPF.Models;


namespace TAlex.WPF.CommonDialogs
{
    /// <summary>
    /// Interaction logic for InsertImageDialog.xaml
    /// </summary>
    internal partial class InsertImageDialog : Window
    {
        #region Properties

        public ImageObject Model
        {
            get
            {
                return DataContext as ImageObject;
            }

            set
            {
                DataContext = value;
            }
        }

        #endregion

        #region Constructors

        public InsertImageDialog()
        {
            InitializeComponent();
            Model = new ImageObject();
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void SourceTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                LoadImage();
            }
        }

        private void BrowseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images|*.jpg;*.jpeg;*.png;*gif|JPEG|*.jpg;*.jpeg|PNG|*.png|GIF|*.gif";
            dialog.FilterIndex = 0;
            if (dialog.ShowDialog() == true)
            {
                Model.Source = dialog.FileName;
                LoadImage();
            }
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Image_DownloadCompleted(object sender, EventArgs e)
        {
            BitmapImage image = (BitmapImage)sender;

            Model.OriginalWidth = (int)image.PixelWidth;
            Model.OriginalHeight = (int)image.PixelHeight;
        }

        private void Image_DownloadFailed(object sender, ExceptionEventArgs e)
        {
            SetInvalidState();
        }

        #endregion

        #region Helpers

        private void LoadImage()
        {
            try
            {
                BitmapImage image = new BitmapImage(new Uri(Model.Source, UriKind.RelativeOrAbsolute));
                image.DownloadCompleted += Image_DownloadCompleted;
                image.DownloadFailed += Image_DownloadFailed;

                PreviewImage.Source = image;
                
                Model.OriginalWidth = (int)image.PixelWidth;
                Model.OriginalHeight = (int)image.PixelHeight;
                Model.ResizeRate = 100;
                Model.SetValidState();
            }
            catch (IOException)
            {
                SetInvalidState();
            }
            catch (ArgumentNullException)
            {
                SetInvalidState();
            }
        }

        private void SetInvalidState()
        {
            PreviewImage.Width = 0;
            PreviewImage.Height = 0;
            PreviewImage.Source = null;

            Model.SetInvalidState();
        }

        #endregion

        #endregion
    }
}
