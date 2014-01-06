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
using System.Windows.Navigation;
using System.Windows.Shapes;
using mshtml;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using TAlex.WPF.Media;
using TAlex.WPF.CommonDialogs;


namespace TAlex.WPF.Controls
{
    /// <summary>
    /// Interaction logic for HtmlRichEditor.xaml
    /// </summary>
    public partial class HtmlRichEditor : UserControl
    {
        #region Fields

        public static readonly DependencyProperty HtmlSourceProperty;
        public static readonly DependencyProperty EditModeProperty;

        private DispatcherTimer _styleTimer;

        #endregion

        #region Properties

        public string HtmlSource
        {
            get
            {
                return (string)GetValue(HtmlSourceProperty);
            }

            set
            {
                SetValue(HtmlSourceProperty, value);
            }
        }

        public EditMode EditMode
        {
            get
            {
                return (EditMode)GetValue(EditModeProperty);
            }

            set
            {
                SetValue(EditModeProperty, value);
            }
        }

        private IHTMLDocument2 HtmlDocument
        {
            get
            {
                return VisualEditor != null ? (IHTMLDocument2)VisualEditor.Document : null;
            }
        }

        private bool DocumentIsReady
        {
            get
            {
                return HtmlDocument != null && HtmlDocument.readyState == "complete";
            }
        }

        #endregion

        #region Constructors
        
        static HtmlRichEditor()
        {
            EditModeProperty = DependencyProperty.Register("EditMode", typeof(EditMode), typeof(HtmlRichEditor), new FrameworkPropertyMetadata(EditMode.Visual, new PropertyChangedCallback(OnEditModeChanged)));
            HtmlSourceProperty = DependencyProperty.Register("HtmlSource", typeof(string), typeof(HtmlRichEditor), new FrameworkPropertyMetadata(String.Empty, new PropertyChangedCallback(OnHtmlSourceChanged)));
        }

        public HtmlRichEditor()
        {
            InitializeComponent();
            InitVisualEditor();

            _styleTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            _styleTimer.Tick += StyleTimer_Tick;
            _styleTimer.Start();
        }

        #endregion

        #region Methods

        #region Dependency Property Handlers

        private static void OnEditModeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HtmlRichEditor editor = (HtmlRichEditor)sender;

            if ((EditMode)e.NewValue == EditMode.Visual)
                editor.SetVisualMode();
            else
                editor.SetSourceMode();
        }

        private static void OnHtmlSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HtmlRichEditor editor = (HtmlRichEditor)sender;
            editor.UpdateHtmlSource((string)e.NewValue);
        }

        #endregion

        #region Command Event Bindings

        private void EditHtml_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditMode = (EditMode == EditMode.Source) ? EditMode.Visual : EditMode.Source;
            bool canEdit = EditMode == Controls.EditMode.Visual;
            FontColorChip.IsEnabled = canEdit;
            BackColorChip.IsEnabled = canEdit;
            FontFamilyList.IsEnabled = canEdit;
            FontSizeList.IsEnabled = canEdit;
        }

        private void EditHtml_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void Bold_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.Bold);
        }

        private void Italic_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.Italic);
        }

        private void Underline_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.Underline);
        }

        private void ClearStyle_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.RemoveFormat);
        }


        private void UnorderedList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.InsertUnorderedList);
        }

        private void OrderedList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.InsertOrderedList);
        }

        private void DecreaseIndent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.Outdent);
        }

        private void IncreaseIndent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.Indent);
        }


        private void JustifyLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.JustifyLeft);
        }

        private void JustifyCenter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.JustifyCenter);
        }

        private void JustifyRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.JustifyRight);
        }

        private void JustifyFull_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand(HtmlCommands.JustifyFull);
        }


        private void InsertHyperlink_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (DocumentIsReady)
            {
                InsertHyperlinkDialog dialog = new InsertHyperlinkDialog
                {
                    Owner = Window.GetWindow(this),
                    Model = new Models.HyperlinkObject { Text = HtmlDocument.selection.createRange().text }
                };
                if (dialog.ShowDialog() == true)
                {
                    InsertHtml(dialog.Model.ToHtml());
                }
            }
        }

        private void InsertImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void InsertTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }


        private void EditingCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DocumentIsReady && EditMode == EditMode.Visual;
        }

        #endregion

        #region Event Handlers

        private void VisualEditor_Navigated(object sender, NavigationEventArgs e)
        {
            ((HTMLTextContainerEvents2_Event)HtmlDocument.body).onchange += new HTMLTextContainerEvents2_onchangeEventHandler(b);
        }

        private void b(IHTMLEventObj obj)
        {
            int d = 5;
            var state = HtmlDocument.readyState;
        }

        private void StyleTimer_Tick(object sender, EventArgs e)
        {
            string fontName = GetHtmlCommandValue(HtmlCommands.FontName);
            FontFamilyList.SelectionChanged -= FontFamilyList_SelectionChanged;
            FontFamilyList.SelectedItem = FontFamilyList.Items.Cast<string>().FirstOrDefault(x => x == fontName);
            FontFamilyList.SelectionChanged += FontFamilyList_SelectionChanged;

            string fontSize = GetHtmlCommandValue(HtmlCommands.FontSize);
            FontSizeList.SelectionChanged -= FontSizeList_SelectionChanged;
            FontSizeList.SelectedItem = FontSizeList.Items.Cast<Controls.FontSize>().FirstOrDefault(x => x.Key == fontSize);
            FontSizeList.SelectionChanged += FontSizeList_SelectionChanged;

            ToggleBold.IsChecked = GetHtmlCommandState(HtmlCommands.Bold);
            ToggleItalic.IsChecked = GetHtmlCommandState(HtmlCommands.Italic);
            ToggleUnderline.IsChecked = GetHtmlCommandState(HtmlCommands.Underline);
            JustifyLeft.IsChecked = GetHtmlCommandState(HtmlCommands.JustifyLeft);
            JustifyCenter.IsChecked = GetHtmlCommandState(HtmlCommands.JustifyCenter);
            JustifyRight.IsChecked = GetHtmlCommandState(HtmlCommands.JustifyRight);
            JustifyFull.IsChecked = GetHtmlCommandState(HtmlCommands.JustifyFull);
            ToggleUnorderedList.IsChecked = GetHtmlCommandState(HtmlCommands.InsertUnorderedList);
            ToggleOrderedList.IsChecked = GetHtmlCommandState(HtmlCommands.InsertOrderedList);

            FontColorChip.SelectedColorChanged -= FontColorChip_SelectedColorChanged;
            FontColorChip.SelectedColor = GetHtmlCommandValueAsColor(HtmlCommands.ForeColor, Colors.Black);
            FontColorChip.SelectedColorChanged += FontColorChip_SelectedColorChanged;

            BackColorChip.SelectedColorChanged -= BackColorChip_SelectedColorChanged;
            BackColorChip.SelectedColor = GetHtmlCommandValueAsColor(HtmlCommands.BackColor, Colors.Transparent);
            BackColorChip.SelectedColorChanged += BackColorChip_SelectedColorChanged;
        }

        private void FontSizeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFontSize = FontSizeList.SelectedItem as Controls.FontSize;
            if (selectedFontSize != null)
            {
                ExecuteHtmlCommand(HtmlCommands.FontSize, selectedFontSize.Key);
            }
        }

        private void FontFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFontName = FontFamilyList.SelectedItem as string;
            if (selectedFontName != null)
            {
                ExecuteHtmlCommand(HtmlCommands.FontName, selectedFontName);
            }
        }

        private void FontColorChip_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (e.OldValue != e.NewValue)
            {
                ExecuteHtmlCommand(HtmlCommands.ForeColor, ColorUtilities.ColorToHexString(e.NewValue));
            }
        }

        private void BackColorChip_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (e.OldValue != e.NewValue)
            {
                ExecuteHtmlCommand(HtmlCommands.BackColor, ColorUtilities.ColorToHexString(e.NewValue));
            }
        }

        #endregion

        #region Helpers

        private void SetVisualMode()
        {
            if (EditMode == Controls.EditMode.Visual)
            {
                CodeEditor.Visibility = System.Windows.Visibility.Hidden;
                VisualEditor.Visibility = System.Windows.Visibility.Visible;

                UpdateHtmlSource(CodeEditor.Text);
            }
        }

        private void SetSourceMode()
        {
            if (EditMode == Controls.EditMode.Source)
            {
                VisualEditor.Visibility = System.Windows.Visibility.Hidden;
                CodeEditor.Visibility = System.Windows.Visibility.Visible;

                UpdateHtmlSource(HtmlDocument.body.innerHTML);
            }
        }

        private void UpdateHtmlSource(string source)
        {
            if (EditMode == Controls.EditMode.Visual)
                HtmlDocument.body.innerHTML = source;
            else
                CodeEditor.Text = source;
        }

        private void ExecuteHtmlCommand(string command, object value = null)
        {
            if (DocumentIsReady) HtmlDocument.execCommand(command, false, value);
        }

        private bool GetHtmlCommandState(string command)
        {
            return DocumentIsReady && HtmlDocument.queryCommandState(command);
        }

        private string GetHtmlCommandValue(string command)
        {
            return DocumentIsReady ? (HtmlDocument.queryCommandValue(command) + String.Empty) : null;
        }

        private Color GetHtmlCommandValueAsColor(string command, Color defaultColor)
        {
            var value = GetHtmlCommandValue(command);
            if (!String.IsNullOrEmpty(value))
            {
                if (value.StartsWith("#")) return (Color)ColorConverter.ConvertFromString(value);
                return (Color)ColorConverter.ConvertFromString(String.Format("#FF{0:X6}", ColorUtilities.BgrToRgb(int.Parse(value))));
            }
            return defaultColor;
        }

        private void InsertHtml(string source)
        {
            var range = HtmlDocument.selection.createRange() as IHTMLTxtRange;
            range.pasteHTML(source);
        }


        private ReadOnlyCollection<string> GetDefaultFontFamilies()
        {
            List<string> ls = new List<string>
            {
                "Arial",
                "Calibri",
                "Cambria",
                "Candara",
                "Century",
                "Consolas",
                "Constantia",
                "Corbel",
                "Courier New",
                "Segoe UI",
                "Tahoma",
                "Times New Roman",
                "Verdana",
                "Vrinda"
            };
            return new ReadOnlyCollection<string>(ls);
        }

        private ReadOnlyCollection<FontSize> GetDefaultFontSizes()
        {
            List<FontSize> ls = new List<FontSize>
            {
                Controls.FontSize.XXSmall,
                Controls.FontSize.XSmall,
                Controls.FontSize.Small,
                Controls.FontSize.Middle,
                Controls.FontSize.Large,
                Controls.FontSize.XLarge,
                Controls.FontSize.XXLarge
            };
            return new ReadOnlyCollection<FontSize>(ls);
        } 

        private void InitVisualEditor()
        {
            VisualEditor.Navigated += VisualEditor_Navigated;
            VisualEditor.NavigateToString(WrapHtmlContent(String.Empty));

            FontSizeList.ItemsSource = GetDefaultFontSizes();
            FontSizeList.SelectionChanged += FontSizeList_SelectionChanged;

            FontFamilyList.ItemsSource = GetDefaultFontFamilies();
            FontFamilyList.SelectionChanged += FontFamilyList_SelectionChanged;
        }

        private static string WrapHtmlContent(string source, string styles = null)
        {
            return String.Format(
                @"<html>
                    <head>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />

                        <style type='text/css'>
                            body {{ font: 14px verdana; color: #505050; background: #fcfcfc; }}
                            {1}
                        </style>
                    </head>
                    <body contenteditable>{0}</body>
                </html>",
                source, styles);
        }

        #endregion

        #endregion
    }


    public enum EditMode
    {
        Visual,
        Source
    }

    public class FontSize
    {
        public string Key { get; set; }
        public double Size { get; set; }
        public string Text { get; set; }

        public static readonly FontSize NO = new FontSize { Size = 0 };
        public static readonly FontSize XXSmall = new FontSize { Key = "1", Size = 8.5, Text = "8pt" };
        public static readonly FontSize XSmall = new FontSize { Key = "2", Size = 10.5, Text = "10pt" };
        public static readonly FontSize Small = new FontSize { Key = "3", Size = 12, Text = "12pt" };
        public static readonly FontSize Middle = new FontSize { Key = "4", Size = 14, Text = "14pt" };
        public static readonly FontSize Large = new FontSize { Key = "5", Size = 18, Text = "18pt" };
        public static readonly FontSize XLarge = new FontSize { Key = "6", Size = 24, Text = "24pt" };
        public static readonly FontSize XXLarge = new FontSize { Key = "7", Size = 36, Text = "36pt" };
    }

    public static class HtmlEditingCommands
    {
        #region Fields

        private static readonly RoutedUICommand _toggleEditHtml = new RoutedUICommand();
        private static readonly RoutedUICommand _toggleBold = new RoutedUICommand();
        private static readonly RoutedUICommand _toggleItalic = new RoutedUICommand();
        private static readonly RoutedUICommand _toggleUnderline = new RoutedUICommand();
        private static readonly RoutedUICommand _clearStyle = new RoutedUICommand();

        private static readonly RoutedUICommand _toggleUnorderedList = new RoutedUICommand();
        private static readonly RoutedUICommand _toggleOrderedList = new RoutedUICommand();
        private static readonly RoutedUICommand _decreaseIndent = new RoutedUICommand();
        private static readonly RoutedUICommand _increaseIndent = new RoutedUICommand();

        private static readonly RoutedUICommand _justifyLeft = new RoutedUICommand();
        private static readonly RoutedUICommand _justifyCenter = new RoutedUICommand();
        private static readonly RoutedUICommand _justifyRight = new RoutedUICommand();
        private static readonly RoutedUICommand _justifyFull = new RoutedUICommand();

        private static readonly RoutedUICommand _insertHyperlink = new RoutedUICommand();
        private static readonly RoutedUICommand _insertImage = new RoutedUICommand();
        private static readonly RoutedUICommand _insertTable = new RoutedUICommand();

        #endregion

        #region Properties

        public static RoutedUICommand ToggleEditHtml
        {
            get { return _toggleEditHtml; }
        }


        public static RoutedUICommand ToggleBold
        {
            get { return _toggleBold; }
        }

        public static RoutedUICommand ToggleItalic
        {
            get { return _toggleItalic; }
        }

        public static RoutedUICommand ToggleUnderline
        {
            get { return _toggleUnderline; }
        }

        public static RoutedUICommand ClearStyle
        {
            get { return _clearStyle; }
        }


        public static RoutedUICommand ToggleUnorderedList
        {
            get { return _toggleUnorderedList; }
        }

        public static RoutedUICommand ToggleOrderedList
        {
            get { return _toggleOrderedList; }
        }

        public static RoutedUICommand DecreaseIndent
        {
            get { return _decreaseIndent; }
        }

        public static RoutedUICommand IncreaseIndent
        {
            get { return _increaseIndent; }
        }


        public static RoutedUICommand JustifyLeft
        {
            get { return _justifyLeft; }
        }

        public static RoutedUICommand JustifyCenter
        {
            get { return _justifyCenter; }
        }

        public static RoutedUICommand JustifyRight
        {
            get { return _justifyRight; }
        }

        public static RoutedUICommand JustifyFull
        {
            get { return _justifyFull; }
        }


        public static RoutedUICommand InsertHyperlink
        {
            get { return _insertHyperlink; }
        }

        public static RoutedUICommand InsertImage
        {
            get { return _insertImage; }
        }

        public static RoutedUICommand InsertTable
        {
            get { return _insertTable; }
        }

        #endregion
    }

    public static class HtmlCommands
    {
        public static readonly string FontName = "FontName";
        public static readonly string FontSize = "FontSize";

        public static readonly string Bold = "Bold";
        public static readonly string Italic = "Italic";
        public static readonly string Underline = "Underline";
        public static readonly string ForeColor = "ForeColor";
        public static readonly string BackColor = "BackColor";
        public static readonly string RemoveFormat = "RemoveFormat";

        public static readonly string InsertUnorderedList = "InsertUnorderedList";
        public static readonly string InsertOrderedList = "InsertOrderedList";
        public static readonly string Outdent = "Outdent";
        public static readonly string Indent = "Indent";

        public static readonly string JustifyLeft = "JustifyLeft";
        public static readonly string JustifyCenter = "JustifyCenter";
        public static readonly string JustifyRight = "JustifyRight";
        public static readonly string JustifyFull = "JustifyFull";
    }
}
