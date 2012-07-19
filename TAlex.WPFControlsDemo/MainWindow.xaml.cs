using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Globalization;

using TAlex.WPF.Controls;
using TAlex.WPF.CommonDialogs;


namespace TAlex.WPFControlsDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal minimum = decimal.Parse(minimumTextBox.Text, CultureInfo.InvariantCulture);
                decimal maximum = decimal.Parse(maximumTextBox.Text, CultureInfo.InvariantCulture);
                decimal increment = decimal.Parse(incrementTextBox.Text, CultureInfo.InvariantCulture);
                int decimals = int.Parse(decimalsTextBox.Text, CultureInfo.InvariantCulture);

                numericUpDown.Minimum = minimum;
                numericUpDown.Maximum = maximum;
                numericUpDown.Increment = increment;
                numericUpDown.DecimalPlaces = decimals;
            }
            catch (FormatException)
            {
                MessageBox.Show(this, "Invalid data!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(this, "Invalid data!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void colorPickerDialogPreviewRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ColorPickerDialog cpd = new ColorPickerDialog();
            cpd.Owner = this;

            SolidColorBrush br = colorPickerDialogPreviewRectangle.Fill as SolidColorBrush;
            if (br != null)
                cpd.SelectedColor = br.Color;

            if (cpd.ShowDialog() == true)
            {
                colorPickerDialogPreviewRectangle.Fill = new SolidColorBrush(cpd.SelectedColor);
            }
        }

        private void previewText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FontChooserDialog fcd = new FontChooserDialog();
            fcd.Owner = this;
            fcd.SelectFontSettings(previewText);

            fcd.ShowColor = (bool)showColorCheckBox.IsChecked;
            fcd.ShowTextDecorations = (bool)showTextDecorationsCheckBox.IsChecked;

            if (fcd.ShowDialog() == true)
            {
                fcd.ApplyFontSettings(previewText);
            }
        }

        private void choiceFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = folderPathTextBox.Text;

            if (fbd.ShowDialog(this) == true)
            {
                folderPathTextBox.Text = fbd.SelectedPath;
            }
        }

        #endregion
    }
}