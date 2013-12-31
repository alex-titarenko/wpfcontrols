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
        }

        #endregion

        #region Methods

        #region Dependency Property Handlers

        private static void OnEditModeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();

            //HtmlRichEditor editor = (HtmlRichEditor)sender;
            //if ((EditMode)e.NewValue == EditMode.Visual) editor.SetVisualMode();
            //else editor.SetSourceMode();
        }

        private static void OnHtmlSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();

            //HtmlRichEditor editor = (HtmlRichEditor)sender;
            //editor.myBindingContent = (string)e.NewValue;
            //editor.ContentHtml = editor.myBindingContent; 
        }

        #endregion

        #region Command Event Bindings

        private void EditHtmlExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            EditMode = (EditMode == EditMode.Source) ? EditMode.Visual : EditMode.Source;
        }

        private void EditHtmlCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void BoldExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
            //if (htmldoc != null) htmldoc.Bold();
        }

        private void EditingCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = /*htmldoc != null &&*/ EditMode == EditMode.Visual;
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

        #endregion

        #region Constructors

        static HtmlEditingCommands()
        {
            ToggleEditHtml.InputGestures.Add(new KeyGesture(Key.B, ModifierKeys.Control));
        }

        #endregion
    }
}
