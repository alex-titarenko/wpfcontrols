using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Globalization;


namespace TAlex.WPF.CommonDialogs
{
    /// <summary>
    /// Prompts the user to choose a font from among those installed on the local computer.
    /// </summary>
    public partial class FontChooserDialog : Window
    {
        #region Fields

        private const int ThresholdLightTone = 700;

        private static readonly XmlLanguage EnglishXmlLanguage = XmlLanguage.GetLanguage("en-us");

        private static readonly double[] CommonlyUsedFontSizes = new double[]
        {
            8.0,   9.0,   10.0,  11.0,  12.0,  14.0,  16.0,  18.0,
            20.0,  22.0,  24.0,  26.0,  28.0,  36.0,  48.0,  72.0
        };

        /// <summary>
        /// Represents the default text preview. This field is constant.
        /// </summary>
        public const string DefaultTextPreview = "The quick brown fox jumps over the lazy dog";


        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.SelectedFontFamily"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedFontFamilyProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.SelectedFontWeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedFontWeightProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.SelectedFontStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedFontStyleProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.SelectedFontStretch"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedFontStretchProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.SelectedFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedFontSizeProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.SelectedTextDecorations"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedTextDecorationsProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.SelectedFontColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedFontColorProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.TextPreview"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextPreviewProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.ShowTextDecorations"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowTextDecorationsProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog.ShowColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected font family.
        /// This is a dependency property.
        /// </summary>
        public FontFamily SelectedFontFamily
        {
            get
            {
                return (FontFamily)GetValue(SelectedFontFamilyProperty);
            }

            set
            {
                SetValue(SelectedFontFamilyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected font weight.
        /// This is a dependency property.
        /// </summary>
        public FontWeight SelectedFontWeight
        {
            get
            {
                return (FontWeight)GetValue(SelectedFontWeightProperty);
            }

            set
            {
                SetValue(SelectedFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected font style (normal, italic, or oblique).
        /// This is a dependency property.
        /// </summary>
        public FontStyle SelectedFontStyle
        {
            get
            {
                return (FontStyle)GetValue(SelectedFontStyleProperty);
            }

            set
            {
                SetValue(SelectedFontStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected font stretch.
        /// This is a dependency property.
        /// </summary>
        public FontStretch SelectedFontStretch
        {
            get
            {
                return (FontStretch)GetValue(SelectedFontStretchProperty);
            }

            set
            {
                SetValue(SelectedFontStretchProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected font size.
        /// This is a dependency property.
        /// </summary>
        public double SelectedFontSize
        {
            get
            {
                return (double)GetValue(SelectedFontSizeProperty);
            }

            set
            {
                SetValue(SelectedFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected text decorations, such as underline, strikethrough, and etc.
        /// This is a dependency property.
        /// </summary>
        public TextDecorationCollection SelectedTextDecorations
        {
            get
            {
                return (TextDecorationCollection)GetValue(SelectedTextDecorationsProperty);
            }

            set
            {
                SetValue(SelectedTextDecorationsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected font color.
        /// This is a dependency property.
        /// </summary>
        public Color SelectedFontColor
        {
            get
            {
                return (Color)GetValue(SelectedFontColorProperty);
            }

            set
            {
                SetValue(SelectedFontColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text to preview the selected font options.
        /// This is a dependency property.
        /// </summary>
        public string TextPreview
        {
            get
            {
                return (string)GetValue(TextPreviewProperty);
            }

            set
            {
                SetValue(TextPreviewProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box contains controls
        /// that allow the user to specify strikethrough, underline, and etc.
        /// This is a dependency property.
        /// </summary>
        public bool ShowTextDecorations
        {
            get
            {
                return (bool)GetValue(ShowTextDecorationsProperty);
            }

            set
            {
                SetValue(ShowTextDecorationsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box displays the color choice.
        /// This is a dependency property.
        /// </summary>
        public bool ShowColor
        {
            get
            {
                return (bool)GetValue(ShowColorProperty);
            }

            set
            {
                SetValue(ShowColorProperty, value);
            }
        }

        #endregion

        #region Constructors

        static FontChooserDialog()
        {
            SelectedFontFamilyProperty = DependencyProperty.Register("SelectedFontFamily", typeof(FontFamily), typeof(FontChooserDialog), new PropertyMetadata(SelectedFontFamilyPropertyChanged));
            SelectedFontWeightProperty = DependencyProperty.Register("SelectedFontWeight", typeof(FontWeight), typeof(FontChooserDialog), new PropertyMetadata(SelectedTypefaceChanged));
            SelectedFontStyleProperty = DependencyProperty.Register("SelectedFontStyle", typeof(FontStyle), typeof(FontChooserDialog), new PropertyMetadata(SelectedTypefaceChanged));
            SelectedFontStretchProperty = DependencyProperty.Register("SelectedFontStretch", typeof(FontStretch), typeof(FontChooserDialog), new PropertyMetadata(SelectedTypefaceChanged));
            SelectedFontSizeProperty = DependencyProperty.Register("SelectedFontSize", typeof(double), typeof(FontChooserDialog), new PropertyMetadata(SelectedFontSizePropertyChanged));
            SelectedTextDecorationsProperty = DependencyProperty.Register("SelectedTextDecorations", typeof(TextDecorationCollection), typeof(FontChooserDialog), new PropertyMetadata(SelectedTextDecorationsPropertyChanged));
            SelectedFontColorProperty = DependencyProperty.Register("SelectedFontColor", typeof(Color), typeof(FontChooserDialog), new PropertyMetadata(SelectedFontColorPropertyChanged));
            TextPreviewProperty = DependencyProperty.Register("TextPreview", typeof(string), typeof(FontChooserDialog), new PropertyMetadata(DefaultTextPreview));

            ShowTextDecorationsProperty = DependencyProperty.Register("ShowTextDecorations", typeof(bool), typeof(FontChooserDialog), new PropertyMetadata(true, ShowTextDecorationsPropertyChanged));
            ShowColorProperty = DependencyProperty.Register("ShowColor", typeof(bool), typeof(FontChooserDialog), new PropertyMetadata(true, ShowColorPropertyChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.CommonDialogs.FontChooserDialog"/> class.
        /// </summary>
        public FontChooserDialog()
        {
            InitializeComponent();

            InitializeFontFamilyList();
            InitializeFontSize();
        }

        #endregion

        #region Methods

        #region Event Handlers

        /// <summary>
        /// Called when IsInitialized is set to true.
        /// </summary>
        /// <param name="e">Provides data for the Initialized event.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            previewTextBox.SetBinding(TextBox.TextProperty, new Binding("TextPreview") { Source = this });
        }


        private static void SelectedFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooserDialog)d).OnSelectedFontFamilyChanged((FontFamily)e.NewValue);
        }

        private static void SelectedTypefaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooserDialog)d).InvalidateTypefaceListSelection();
        }

        private static void SelectedFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooserDialog)d).OnSelectedFontSizeChanged((double)e.NewValue);
        }

        private static void SelectedTextDecorationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooserDialog)d).OnSelectedTextDecorationsChanged();
        }

        private static void SelectedFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooserDialog)d).OnSelectedFontColorChanged((Color)e.NewValue);
        }

        private static void ShowTextDecorationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooserDialog)d).OnShowTextDecorationsChanged((bool)e.NewValue);
        }

        private static void ShowColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooserDialog)d).OnShowColorChanged((bool)e.NewValue);
        }


        private void OnSelectedFontFamilyChanged(FontFamily fontFamily)
        {
            if (fontFamilyListBox.Items.Contains(fontFamily))
            {
                fontFamilyListBox.SelectedItem = fontFamily;
                fontFamilyListBox.ScrollIntoView(fontFamily);

                fontFamilyTextBox.Text = fontFamily.ToString();
            }

            previewTextBox.FontFamily = fontFamily;

            InvalidateTypefaceList(fontFamily);
            InvalidateTypefaceListSelection();
        }

        private void OnSelectedFontSizeChanged(double fontSize)
        {
            if (sizeListBox.Items.Contains(fontSize))
            {
                sizeListBox.SelectedItem = fontSize;
                sizeListBox.ScrollIntoView(fontSize);
            }
            else
            {
                sizeListBox.SelectedIndex = -1;
            }

            double textBoxValue;
            if (!double.TryParse(sizeTextBox.Text, out textBoxValue) || (textBoxValue != fontSize))
            {
                sizeTextBox.Text = fontSize.ToString();
            }

            previewTextBox.FontSize = fontSize;
        }

        private void InvalidateTypefaceListSelection()
        {
            Typeface typeface = new Typeface(SelectedFontFamily, SelectedFontStyle, SelectedFontWeight, SelectedFontStretch);
            string typefaceName = GetTypefaceName(typeface);

            foreach (ListBoxItem item in typefaceListBox.Items)
            {
                Typeface itemTypeface = (Typeface)item.Tag;

                if (Typeface.Equals(typeface, itemTypeface))
                {
                    typefaceListBox.SelectedItem = item;
                    typefaceListBox.ScrollIntoView(item);

                    typefaceTextBox.Text = typefaceName;
                    break;
                }
            }

            previewTextBox.FontWeight = SelectedFontWeight;
            previewTextBox.FontStyle = SelectedFontStyle;
            previewTextBox.FontStretch = SelectedFontStretch;
        }

        private void OnSelectedTextDecorationsChanged()
        {
            bool underline = false;
            bool baseline = false;
            bool strikethrough = false;
            bool overline = false;

            TextDecorationCollection textDecorations = SelectedTextDecorations;

            previewTextBox.TextDecorations.Clear();

            if (textDecorations != null)
            {
                foreach (TextDecoration td in textDecorations)
                {
                    switch (td.Location)
                    {
                        case TextDecorationLocation.Underline:
                            underline = true;
                            break;
                        case TextDecorationLocation.Baseline:
                            baseline = true;
                            break;
                        case TextDecorationLocation.Strikethrough:
                            strikethrough = true;
                            break;
                        case TextDecorationLocation.OverLine:
                            overline = true;
                            break;
                    }

                    previewTextBox.TextDecorations.Add(td);
                }
            }

            underlineCheckBox.IsChecked = underline;
            baselineCheckBox.IsChecked = baseline;
            strikethroughCheckBox.IsChecked = strikethrough;
            overlineCheckBox.IsChecked = overline;
        }

        private void OnSelectedFontColorChanged(Color fontColor)
        {
            fontColorComboBox.SelectedColor = fontColor;
            previewTextBox.Foreground = new SolidColorBrush(fontColor);
            previewTextBox.Background = (fontColor.R + fontColor.G + fontColor.B) > ThresholdLightTone ? Brushes.Black : Brushes.White;
        }

        private void OnShowTextDecorationsChanged(bool showTextDecorations)
        {
            if (showTextDecorations)
                decorationsGroupBox.Visibility = Visibility.Visible;
            else
                decorationsGroupBox.Visibility = Visibility.Collapsed;
        }

        private void OnShowColorChanged(bool showColor)
        {
            if (showColor)
                colorGroupBox.Visibility = Visibility.Visible;
            else
                colorGroupBox.Visibility = Visibility.Collapsed;
        }


        private void fontFamilyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (FontFamily item in fontFamilyListBox.Items)
            {
                if (item.Source.StartsWith(fontFamilyTextBox.Text))
                {
                    fontFamilyListBox.ScrollIntoView(item);
                    break;
                }
            }
        }

        private void fontFamilyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedFontFamily = fontFamilyListBox.SelectedItem as FontFamily;
        }


        private void typefaceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (ListBoxItem item in typefaceListBox.Items)
            {
                string text = item.Content as string;

                if (text.StartsWith(typefaceTextBox.Text))
                {
                    typefaceListBox.ScrollIntoView(item);
                    break;
                }
            }
        }

        private void typefaceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typefaceListBox.SelectedItem != null)
            {
                Typeface typeface = (typefaceListBox.SelectedItem as ListBoxItem).Tag as Typeface;

                if (typeface != null)
                {
                    SelectedFontWeight = typeface.Weight;
                    SelectedFontStyle = typeface.Style;
                    SelectedFontStretch = typeface.Stretch;
                }
            }
        }


        private void sizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double fontSize;

            if (double.TryParse(sizeTextBox.Text, out fontSize))
            {
                if (fontSize > 0 && SelectedFontSize != fontSize)
                {
                    SelectedFontSize = fontSize;
                }
            }
        }

        private void sizeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sizeListBox.SelectedIndex != -1)
            {
                SelectedFontSize = (double)sizeListBox.SelectedItem;
            }
        }


        private void textDecorationCheckStateChanged(object sender, RoutedEventArgs e)
        {
            TextDecorationCollection textDecorations = new TextDecorationCollection();

            if (underlineCheckBox.IsChecked == true)
            {
                textDecorations.Add(TextDecorations.Underline[0]);
            }
            if (baselineCheckBox.IsChecked == true)
            {
                textDecorations.Add(TextDecorations.Baseline[0]);
            }
            if (strikethroughCheckBox.IsChecked == true)
            {
                textDecorations.Add(TextDecorations.Strikethrough[0]);
            }
            if (overlineCheckBox.IsChecked == true)
            {
                textDecorations.Add(TextDecorations.OverLine[0]);
            }

            textDecorations.Freeze();
            SelectedTextDecorations = textDecorations;
        }


        private void fontColorComboBox_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (e.NewValue != null)
            {
                SelectedFontColor = e.NewValue;
            }
        }


        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Applies all the font settings that were selected by the user to the dependent element.
        /// </summary>
        /// <param name="element">Dependent element which applies the font settings.</param>
        public void ApplyFontSettings(DependencyObject element)
        {
            TextElement.SetFontFamily(element, SelectedFontFamily);
            TextElement.SetFontSize(element, SelectedFontSize);
            TextElement.SetFontStretch(element, SelectedFontStretch);
            TextElement.SetFontStyle(element, SelectedFontStyle);
            TextElement.SetFontWeight(element, SelectedFontWeight);
            TextElement.SetForeground(element, new SolidColorBrush(SelectedFontColor));

            element.SetValue(TextBlock.TextDecorationsProperty, SelectedTextDecorations);
        }

        /// <summary>
        /// Selects all the font settings from the dependent element.
        /// </summary>
        /// <param name="element">Dependent element from the which is selected by the font settings.</param>
        public void SelectFontSettings(DependencyObject element)
        {
            SelectedFontFamily = TextElement.GetFontFamily(element);
            SelectedFontSize = TextElement.GetFontSize(element);
            SelectedFontStretch = TextElement.GetFontStretch(element);
            SelectedFontStyle = TextElement.GetFontStyle(element);
            SelectedFontWeight = TextElement.GetFontWeight(element);

            SolidColorBrush foreBrush = TextElement.GetForeground(element) as SolidColorBrush;
            if (foreBrush != null)
                SelectedFontColor = foreBrush.Color;

            SelectedTextDecorations = element.GetValue(TextBlock.TextDecorationsProperty) as TextDecorationCollection;
        }

        private void InitializeFontFamilyList()
        {
            fontFamilyListBox.Items.Clear();

            foreach (FontFamily item in Fonts.SystemFontFamilies)
            {
                fontFamilyListBox.Items.Add(item);
            }
        }

        private void InitializeFontSize()
        {
            sizeListBox.Items.Clear();

            foreach (double size in CommonlyUsedFontSizes)
            {
                sizeListBox.Items.Add(size);
            }
        }

        private string GetTypefaceName(Typeface typeface)
        {
            XmlLanguage userLanguage = XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag);

            string name;
            if (typeface.FaceNames.TryGetValue(userLanguage, out name))
            {
                return name;
            }

            return typeface.FaceNames[EnglishXmlLanguage];
        }

        private void InvalidateTypefaceList(FontFamily fontFamily)
        {
            typefaceListBox.Items.Clear();

            if (fontFamily != null)
            {
                foreach (Typeface typeface in fontFamily.GetTypefaces())
                {
                    string typefaceName = GetTypefaceName(typeface);
                    typefaceListBox.Items.Add(new ListBoxItem() { Content = typefaceName, Tag = typeface });
                }
            }
        }

        #endregion

        #endregion
    }
}
