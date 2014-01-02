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

        private HTMLDocument HtmlDocument
        {
            get
            {
                return VisualEditor != null ? (HTMLDocument)VisualEditor.Document : null;
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
        }

        private void EditHtml_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void Bold_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("Bold");
        }

        private void Italic_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("Italic");
        }

        private void Underline_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("Underline");
        }

        private void ClearStyle_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("RemoveFormat");
        }


        private void UnorderedList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("InsertUnorderedList");
        }

        private void OrderedList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("InsertOrderedList");
        }

        private void DecreaseIndent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("Outdent");
        }

        private void IncreaseIndent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("Indent");
        }


        private void JustifyLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("JustifyLeft");
        }

        private void JustifyCenter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("JustifyCenter");
        }

        private void JustifyRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("JustifyRight");
        }

        private void JustifyFull_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteHtmlCommand("JustifyFull");
        }


        private void EditingCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = HtmlDocument != null && EditMode == EditMode.Visual;
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

        private void ExecuteHtmlCommand(string command)
        {
            if (HtmlDocument != null) HtmlDocument.execCommand(command, false, null);
        }

        private void InitVisualEditor()
        {
            VisualEditor.NavigateToString(WrapHtmlContent(String.Empty));
            //HtmlDocument.
        }

        public static string WrapHtmlContent(string source)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(
                @"<html>
                    <head>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />

                        <style type='text/css'>
                            body { font: 14px verdana; color: #505050; background: #fcfcfc; }
                        </style>

                        <script language='JScript'>
                            function onContextMenu()
                            {
                              if (window.event.srcElement.tagName !='INPUT') 
                              {
                                window.event.returnValue = false;  
                                window.event.cancelBubble = true;
                                return false;
                              }
                            }

                            function onLoad()
                            {
                              document.oncontextmenu = onContextMenu; 
                            }
                        </script>
                    </head>
                    <body contenteditable onload='onLoad();'>"
                );

            sb.Append(source);

            sb.Append(
                    @"</body>
                </html>"
                );

            return sb.ToString();
        }

        #endregion

        #endregion
    }


    public enum EditMode
    {
        Visual,
        Source
    }


    public static class HtmlEditingCommands
    {
        #region Fields

        private static readonly RoutedUICommand _toggleEditHtml = new RoutedUICommand();//"Edit Html", "EditHtml", null);
        private static readonly RoutedUICommand _toggleBold = new RoutedUICommand();//"Bold", "ToggleBold", typeof(HtmlRichEditor));
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

        #endregion

        #region Constructors

        static HtmlEditingCommands()
        {
        }

        #endregion
    }
}
