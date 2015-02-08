using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TAlex.WPF.Controls
{
    /// <summary>
    /// A control to provide a visual indicator when an application is busy.
    /// </summary>
    /// <remarks>
    /// Based on code from https://wpftoolkit.codeplex.com/
    /// The original license of source is Ms-PL, full text see on: https://wpftoolkit.codeplex.com/license
    /// </remarks>
    [TemplateVisualState(Name = VisualStates.StateIdle, GroupName = VisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = VisualStates.StateBusy, GroupName = VisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = VisualStates.StateVisible, GroupName = VisualStates.GroupVisibility)]
    [TemplateVisualState(Name = VisualStates.StateHidden, GroupName = VisualStates.GroupVisibility)]
    [StyleTypedProperty(Property = "OverlayStyle", StyleTargetType = typeof(Rectangle))]
    [StyleTypedProperty(Property = "ProgressBarStyle", StyleTargetType = typeof(ProgressBar))]
    public class BusyIndicator : ContentControl
    {
        #region Fields

        private DispatcherTimer _displayAfterTimer = new DispatcherTimer();

        public static readonly DependencyProperty IsBusyProperty;
        public static readonly DependencyProperty BusyContentProperty;
        public static readonly DependencyProperty BusyContentTemplateProperty;
        public static readonly DependencyProperty DisplayAfterProperty;
        public static readonly DependencyProperty OverlayStyleProperty;
        public static readonly DependencyProperty ProgressBarStyleProperty;

        #endregion

        #region Constructors

        static BusyIndicator()
        {
            IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(BusyIndicator), new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));
            BusyContentProperty = DependencyProperty.Register("BusyContent", typeof(object), typeof(BusyIndicator), new PropertyMetadata(null));
            BusyContentTemplateProperty = DependencyProperty.Register("BusyContentTemplate", typeof(DataTemplate), typeof(BusyIndicator), new PropertyMetadata(null));
            DisplayAfterProperty = DependencyProperty.Register("DisplayAfter", typeof(TimeSpan), typeof(BusyIndicator), new PropertyMetadata(TimeSpan.FromSeconds(0.1)));
            OverlayStyleProperty = DependencyProperty.Register("OverlayStyle", typeof(Style), typeof(BusyIndicator), new PropertyMetadata(null));
            ProgressBarStyleProperty = DependencyProperty.Register("ProgressBarStyle", typeof(Style), typeof(BusyIndicator), new PropertyMetadata(null));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        public BusyIndicator()
        {
            _displayAfterTimer.Tick += DisplayAfterTimerElapsed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the busy indicator should show.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return (bool)GetValue(IsBusyProperty);
            }
            set
            {
                SetValue(IsBusyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the busy content to display to the user.
        /// </summary>
        public object BusyContent
        {
            get
            {
                return (object)GetValue(BusyContentProperty);
            }
            set
            {
                SetValue(BusyContentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the template to use for displaying the busy content to the user.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(BusyContentTemplateProperty);
            }
            set
            {
                SetValue(BusyContentTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how long to delay before displaying the busy content.
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get
            {
                return (TimeSpan)GetValue(DisplayAfterProperty);
            }
            set
            {
                SetValue(DisplayAfterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the style to use for the overlay.
        /// </summary>
        public Style OverlayStyle
        {
            get
            {
                return (Style)GetValue(OverlayStyleProperty);
            }
            set
            {
                SetValue(OverlayStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the style to use for the progress bar.
        /// </summary>
        public Style ProgressBarStyle
        {
            get
            {
                return (Style)GetValue(ProgressBarStyleProperty);
            }
            set
            {
                SetValue(ProgressBarStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the BusyContent is visible.
        /// </summary>
        protected bool IsContentVisible
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="d">BusyIndicator that changed its IsBusy.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // Go visible now
                    IsContentVisible = true;
                }
                else
                {
                    // Set a timer to go visible
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // No longer visible
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }

            ChangeVisualState(true);
        }

        /// <summary>
        /// Handler for the DisplayAfterTimer.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        /// Changes the control's visual state(s).
        /// </summary>
        /// <param name="useTransitions">True if state transitions should be used.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        /// Overrides the OnApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        #endregion

        #region Nested Types

        internal static class VisualStates
        {
            /// <summary>
            /// Busyness group name.
            /// </summary>
            public const string GroupBusyStatus = "BusyStatusStates";

            /// <summary>
            /// Busy state for BusyIndicator.
            /// </summary>
            public const string StateBusy = "Busy";

            /// <summary>
            /// Idle state for BusyIndicator.
            /// </summary>
            public const string StateIdle = "Idle";

            /// <summary>
            /// BusyDisplay group.
            /// </summary>
            public const string GroupVisibility = "VisibilityStates";

            /// <summary>
            /// Visible state name for BusyIndicator.
            /// </summary>
            public const string StateVisible = "Visible";

            /// <summary>
            /// Hidden state name for BusyIndicator.
            /// </summary>
            public const string StateHidden = "Hidden";
        }

        #endregion
    }
}
