using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TAlex.WPF.Theming;


namespace TAlex.WPF.Controls
{
    /// <summary>
    /// Represents the extended version of Image control which adds
    /// ability to make image themeble and frame based,
    /// using the following image uri pattern: {ImageName}.{ThemeName}.{FrameIndex}.{FileExtension}
    /// </summary>
    public class ImageEx : Image
    {
        #region Fields

        private static readonly Regex ThemeNameRegex = new Regex(".(?<theme>[0-9a-zA-z]{1,20})(?<ext>.[0-9a-zA-z]{1,5})$", RegexOptions.Compiled);
        private static readonly Regex FrameRegex = new Regex(".(?<frame_idx>[0-9]+)(?<ext>.[0-9a-zA-z]{1,5})$", RegexOptions.Compiled);
        private static readonly Regex ThemeNameAndFrameRegex = new Regex(".(?<theme>[0-9a-zA-z]{1,20}).(?<frame_idx>[0-9]+)(?<ext>.[0-9a-zA-z]{1,5})$", RegexOptions.Compiled);

        protected IThemeManager ThemeManager;
        protected DispatcherTimer FrameTimer = new DispatcherTimer(DispatcherPriority.Render);

        private double _framesPerSecond = 1;
        private bool _isFrameBased = false;

        #endregion

        #region Properties

        public bool IsThemed
        {
            get;
            set;
        }

        public bool IsFrameBased
        {
            get
            {
                return _isFrameBased;
            }

            set
            {
                _isFrameBased = value;
                if (IsLoaded)
                {
                    FrameTimer.IsEnabled = value;
                }
            }
        }

        public int FirstFrameIndex { get; set; }

        public int LastFrameIndex { get; set; }

        public int CurrentFrameIndex { get; private set; }

        public double FramesPerSecond
        {
            get
            {
                return _framesPerSecond;
            }

            set
            {
                _framesPerSecond = value;
                FrameTimer.Interval = TimeSpan.FromSeconds(1.0 / FramesPerSecond);
            }
        }

        #endregion

        #region Constructors

        public ImageEx()
        {
            ThemeManager = ThemeLocator.Manager;
            ThemeManager.ThemeChanged += Instance_ThemeChanged;

            this.Loaded += ImageEx_Loaded;
            this.Unloaded += ImageEx_Unloaded;
            FrameTimer.Tick += OnFrameTimerTick;

            FramesPerSecond = 1;
        }

        #endregion

        #region Methods

        private void Instance_ThemeChanged(object sender, ThemeEventArgs e)
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            ImageSource source = Source;

            if (source == null)
            {
                return;
            }

            var uri = Source.ToString();
            source = NewImageSource(uri) ?? source;

            dc.DrawImage(source, new Rect(new Point(), RenderSize));
        }

        protected virtual void OnFrameTimerTick(object sender, EventArgs e)
        {
            CurrentFrameIndex = (CurrentFrameIndex + 1) > LastFrameIndex ? FirstFrameIndex : CurrentFrameIndex + 1;
            InvalidateVisual();
        }

        private void ImageEx_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFrameBased)
            {
                StartAnimation();
            }
        }

        private void ImageEx_Unloaded(object sender, RoutedEventArgs e)
        {
            StopAnimation();
        }

        #endregion

        #region Helpers

        public void StartAnimation()
        {
            FrameTimer.Start();
        }

        public void StopAnimation()
        {
            FrameTimer.Stop();
        }


        private string NewImageUri(string uri, string newTheme)
        {
            return ThemeNameRegex.Replace(uri, String.Format(".{0}${{ext}}", ThemeManager.AvailableThemes.Single(x => x.Name == newTheme).FamilyName));
        }

        private string NewImageUri(string uri, int frameIdx)
        {
            return ThemeNameAndFrameRegex.Replace(uri, String.Format(".{0}${{ext}}", frameIdx));
        }

        private string NewImageUri(string uri, string newTheme, int frameIdx)
        {
            return ThemeNameAndFrameRegex.Replace(uri, String.Format(".{0}.{1}${{ext}}", ThemeManager.AvailableThemes.Single(x => x.Name == newTheme).FamilyName, frameIdx));
        }

        private ImageSource NewImageSource(string uri)
        {
            Uri newUri;

            string newUriString = uri;
            if (IsFrameBased && IsThemed)
                newUriString = NewImageUri(uri, ThemeManager.CurrentTheme, CurrentFrameIndex);
            else if (IsFrameBased)
                newUriString = NewImageUri(uri, CurrentFrameIndex);
            else if (IsThemed)
                newUriString = NewImageUri(uri, ThemeManager.CurrentTheme);

            if (Uri.TryCreate(newUriString, UriKind.RelativeOrAbsolute, out newUri))
            {
                try
                {
                    return new BitmapImage(newUri);
                }
                catch (IOException)
                {
                    return null;
                }
            }
            return null;
        }

        #endregion
    }
}
