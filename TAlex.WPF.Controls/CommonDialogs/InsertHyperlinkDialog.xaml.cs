using System;
using System.Collections.Generic;
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
    /// Interaction logic for InsertHyperlinkDialog.xaml
    /// </summary>
    internal partial class InsertHyperlinkDialog : Window
    {
        #region Properties

        public HyperlinkObject Model
        {
            get
            {
                return DataContext as HyperlinkObject;
            }

            set
            {
                DataContext = value;
            }
        }

        #endregion

        #region Constructors

        public InsertHyperlinkDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion
    }
}
