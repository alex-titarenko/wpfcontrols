using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Globalization;
using System.ComponentModel;

namespace TAlex.WPF.Controls
{
    /// <summary>
    /// Represents an up-down control that displays numeric values.
    /// </summary>
    [DefaultProperty("Value"), DefaultEvent("ValueChanged")]
    public class NumericUpDown : Control
    {
        #region Fields

        private const decimal DefaultMinimum = 0M;

        private const decimal DefaultMaximum = 100M;

        private const decimal DefaultIncrement = 1M;

        private const int DefaultDecimalPlaces = 0;

        private const NumberStyles NumberStyle = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.NumericUpDown.Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.NumericUpDown.Minimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.NumericUpDown.Maximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.NumericUpDown.Increment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncrementProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.NumericUpDown.DecimalPlaces"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DecimalPlacesProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.NumericUpDown.InterceptArrowKeys"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InterceptArrowKeysProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.NumericUpDown.IsReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.NumericUpDown.NumberFormatInfo"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NumberFormatInfoProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.NumericUpDown.ValueChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent;

        /// <summary>
        /// The command that increases the value assigned to the numeric up-down control.
        /// </summary>
        public static RoutedCommand IncreaseCommand;

        /// <summary>
        /// The command that decreases the value assigned to the numeric up-down control.
        /// </summary>
        public static RoutedCommand DecreaseCommand;

        private decimal _inputValue;

        private string _lastInput;

        private TextBox _textBox;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value assigned to the numeric up-down control.
        /// This is a dependency property.
        /// </summary>
        public decimal Value
        {
            get
            {
                return (decimal)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value for the numeric up-down control.
        /// This is a dependency property.
        /// </summary>
        public decimal Minimum
        {
            get
            {
                return (decimal)GetValue(MinimumProperty);
            }

            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed value for the numeric up-down control.
        /// This is a dependency property.
        /// </summary>
        public decimal Maximum
        {
            get
            {
                return (decimal)GetValue(MaximumProperty);
            }

            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value to increment or decrement for the numeric up-down control.
        /// This is a dependency property.
        /// </summary>
        public decimal Increment
        {
            get
            {
                return (decimal)GetValue(IncrementProperty);
            }

            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of decimal places to display in the numeric up-down control.
        /// This is a dependency property.
        /// </summary>
        public int DecimalPlaces
        {
            get
            {
                return (int)GetValue(DecimalPlacesProperty);
            }

            set
            {
                SetValue(DecimalPlacesProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicating whether the user can use the UP ARROW and DOWN ARROW keys to select values.
        /// This is a dependency property.
        /// </summary>
        public bool InterceptArrowKeys
        {
            get
            {
                return (bool)GetValue(InterceptArrowKeysProperty);
            }

            set
            {
                SetValue(InterceptArrowKeysProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicating whether the text can be changed by the use of the up or down buttons only.
        /// This is a dependency property.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }

            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the NumberFormatInfo value.
        /// This is a dependency property.
        /// </summary>
        public NumberFormatInfo NumberFormatInfo
        {
            get
            {
                return (NumberFormatInfo)GetValue(NumberFormatInfoProperty);
            }

            set
            {
                SetValue(NumberFormatInfoProperty, value);
            }
        }

        /// <summary>
        /// Gets the current text content held by the text box.
        /// </summary>
        public string ContentText
        {
            get
            {
                if (_textBox != null)
                {
                    return _textBox.Text;
                }

                return null;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }

            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        #endregion

        #region Constructors

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));

            InitializeCommands();

            ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultMinimum, OnValueChanged, CoerceValue));

            MinimumProperty = DependencyProperty.Register("Minimum", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultMinimum, OnMinimumChanged, CoerceMinimum));

            MaximumProperty = DependencyProperty.Register("Maximum", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultMaximum, OnMaximumChanged, CoerceMaximum));

            IncrementProperty = DependencyProperty.Register("Increment", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultIncrement, OnIncrementChanged, CoerceIncrement), ValidateIncrement);

            DecimalPlacesProperty = DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultDecimalPlaces, OnDecimalPlacesChanged), ValidateDecimalPlaces);

            InterceptArrowKeysProperty = DependencyProperty.Register("InterceptArrowKeys", typeof(bool), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(true));

            IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(false, OnIsReadOnlyChanged));

            NumberFormatInfoProperty = DependencyProperty.Register("NumberFormatInfo", typeof(NumberFormatInfo), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(NumberFormatInfo.CurrentInfo.Clone(), OnNumberFormatInfoChanged));


            ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(NumericUpDown));

            // Listen to MouseLeftButtonDown event to determine if NumericUpDown should move focus to itself
            EventManager.RegisterClassHandler(typeof(NumericUpDown),
                Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown), true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Controls.NumericUpDown"/> class.
        /// </summary>
        public NumericUpDown()
            : base()
        {
            _lastInput = String.Empty;
        }

        #endregion

        #region Methods

        #region Statics

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)obj;

            decimal oldValue = (decimal)args.OldValue;
            decimal newValue = (decimal)args.NewValue;

            // Fire Automation events
            NumericUpDownAutomationPeer peer = UIElementAutomationPeer.FromElement(control) as NumericUpDownAutomationPeer;
            if (peer != null)
            {
                peer.RaiseValueChangedEvent(oldValue, newValue);
            }

            RoutedPropertyChangedEventArgs<decimal> e = new RoutedPropertyChangedEventArgs<decimal>(
                oldValue, newValue, ValueChangedEvent);

            control.OnValueChanged(e);
            control.UpdateText();
        }

        private static void OnMinimumChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            element.CoerceValue(MaximumProperty);
            element.CoerceValue(ValueProperty);
        }

        private static void OnMaximumChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            element.CoerceValue(ValueProperty);
        }

        private static void OnIncrementChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
        }

        private static void OnDecimalPlacesChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)element;

            control.CoerceValue(IncrementProperty);
            control.CoerceValue(MinimumProperty);
            control.CoerceValue(MaximumProperty);
            control.CoerceValue(ValueProperty);

            control.UpdateText();
        }

        private static void OnIsReadOnlyChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = element as NumericUpDown;
            bool readOnly = (bool)args.NewValue;

            if (readOnly != control._textBox.IsReadOnly)
            {
                control._textBox.IsReadOnly = readOnly;
            }
        }

        private static void OnNumberFormatInfoChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = element as NumericUpDown;
            control.UpdateText();
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            decimal newValue = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;

            newValue = Math.Max(control.Minimum, Math.Min(control.Maximum, newValue));
            newValue = Decimal.Round(newValue, control.DecimalPlaces);
            
            return newValue;
        }

        private static object CoerceMinimum(DependencyObject element, object value)
        {
            decimal newMinimum = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;
            return Decimal.Round(newMinimum, control.DecimalPlaces);
        }

        private static object CoerceMaximum(DependencyObject element, object value)
        {
            NumericUpDown control = (NumericUpDown)element;
            decimal newMaximum = (decimal)value;
            return Decimal.Round(Math.Max(newMaximum, control.Minimum), control.DecimalPlaces);
        }

        private static object CoerceIncrement(DependencyObject element, object value)
        {
            decimal newIncrement = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;

            decimal coercedNewIncrement = Decimal.Round(newIncrement, control.DecimalPlaces);

            if (coercedNewIncrement == Decimal.Zero)
            {
                coercedNewIncrement = SmallestForDecimalPlaces(control.DecimalPlaces);
            }

            return coercedNewIncrement;
        }

        private static bool ValidateIncrement(object value)
        {
            decimal change = (decimal)value;
            return change > 0;
        }

        private static bool ValidateDecimalPlaces(object value)
        {
            int decimalPlaces = (int)value;
            return decimalPlaces >= 0;
        }

        private static void InitializeCommands()
        {
            IncreaseCommand = new RoutedCommand("IncreaseCommand", typeof(NumericUpDown));
            CommandManager.RegisterClassCommandBinding(typeof(NumericUpDown), new CommandBinding(IncreaseCommand, OnIncreaseCommand));
            CommandManager.RegisterClassInputBinding(typeof(NumericUpDown), new InputBinding(IncreaseCommand, new KeyGesture(Key.Up)));

            DecreaseCommand = new RoutedCommand("DecreaseCommand", typeof(NumericUpDown));
            CommandManager.RegisterClassCommandBinding(typeof(NumericUpDown), new CommandBinding(DecreaseCommand, OnDecreaseCommand));
            CommandManager.RegisterClassInputBinding(typeof(NumericUpDown), new InputBinding(DecreaseCommand, new KeyGesture(Key.Down)));
        }

        private static void OnIncreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;

            if (control != null)
            {
                control.OnIncrease();
            }
        }

        private static void OnDecreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;

            if (control != null)
            {
                control.OnDecrease();
            }
        }

        /// <summary>
        /// This is a class handler for MouseLeftButtonDown event.
        /// The purpose of this handle is to move input focus to NumericUpDown when user pressed
        /// mouse left button on any part of slider that is not focusable.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NumericUpDown control = (NumericUpDown)sender;

            // When someone click on a part in the NumericUpDown and it's not focusable
            // NumericUpDown needs to take the focus in order to process keyboard correctly
            if (!control.IsKeyboardFocusWithin)
            {
                e.Handled = control.Focus() || e.Handled;
            }
        }

        private static decimal SmallestForDecimalPlaces(int decimalPlaces)
        {
            if (decimalPlaces < 0)
                throw new ArgumentOutOfRangeException("decimalPlaces");

            decimal d = 1;

            for (int i = 0; i < decimalPlaces; i++)
            {
                d /= 10;
            }

            return d;
        }

        #endregion

        #region Dynamics

        /// <summary>
        /// Called when the template's tree is generated.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_textBox != null)
            {
                _textBox.TextChanged -= new TextChangedEventHandler(OnTextBoxTextChanged);
                _textBox.PreviewKeyDown -= new KeyEventHandler(OnTextBoxPreviewKeyDown);
            }

            _textBox = (TextBox)base.GetTemplateChild("textbox");

            if (_textBox != null)
            {
                _textBox.TextChanged += new TextChangedEventHandler(OnTextBoxTextChanged);
                _textBox.PreviewKeyDown += new KeyEventHandler(OnTextBoxPreviewKeyDown);
                _textBox.IsReadOnly = false;
            }

            UpdateText();
        }

        /// <summary>
        /// Creates an appropriate NumericUpDownAutomationPeer for this control as part of the WPF infrastructure.
        /// </summary>
        /// <returns></returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new NumericUpDownAutomationPeer(this);
        }

        /// <summary>
        /// Reports that the IsKeyboardFocusWithin property changed.
        /// </summary>
        /// <param name="e">The event data for the IsKeyboardFocusWithinChanged event.</param>
        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                OnGotFocus();
            }
            else
            {
                OnLostFocus();
            }
        }

        /// <summary>
        /// Handles the System.Windows.Input.Mouse.MouseWheel routed event.
        /// </summary>
        /// <param name="e">The System.Windows.Input.MouseWheelEventArgs that contains the event data.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (IsKeyboardFocusWithin)
            {
                if (e.Delta > 0)
                {
                    OnIncrease();
                }
                else
                {
                    OnDecrease();
                }
            }
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
        {
            RaiseEvent(args);
        }

        /// <summary>
        /// IncreaseCommand event handler.
        /// </summary>
        protected virtual void OnIncrease()
        {
            UpdateValue();

            if (Value + Increment <= Maximum)
            {
                Value += Increment;
            }
        }

        /// <summary>
        /// DecreaseCommand event handler.
        /// </summary>
        protected virtual void OnDecrease()
        {
            UpdateValue();

            if (Value - Increment >= Minimum)
            {
                Value -= Increment;
            }
        }

        private void OnGotFocus()
        {
            if (_textBox != null)
            {
                _textBox.Focus();
            }

            UpdateText();
        }

        private void OnLostFocus()
        {
            UpdateValue();
            UpdateText();
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsReadOnly)
            {
                string text = _textBox.Text;

                if (String.IsNullOrEmpty(text) || text == NumberFormatInfo.NegativeSign)
                {
                    return;
                }

                decimal parsedValue = 0M;
                if (decimal.TryParse(text, NumberStyle, NumberFormatInfo, out parsedValue))
                {
                    if ((DecimalPlaces == 0) && (text.Contains(NumberFormatInfo.NumberDecimalSeparator)))
                    {
                        ReturnPreviousInput();
                        return;
                    }

                    _lastInput = text;
                    _inputValue = parsedValue;
                    return;
                }

                ReturnPreviousInput();
            }
            else
            {
                _lastInput = _textBox.Text;
                _inputValue = Value;
            }
        }

        private void OnTextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (InterceptArrowKeys)
                        OnIncrease();
                    break;

                case Key.Down:
                    if (InterceptArrowKeys)
                        OnDecrease();
                    break;

                case Key.Return:
                    UpdateValue();
                    UpdateText();
                    break;

                default:
                    return;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Displays the current value of the numeric up-down control in the appropriate format.
        /// </summary>
        internal void UpdateText()
        {
            NumberFormatInfo formatInfo = (NumberFormatInfo)NumberFormatInfo.Clone();
            formatInfo.NumberGroupSeparator = String.Empty;

            string formattedValue = Value.ToString("F" + DecimalPlaces, formatInfo);

            if (_textBox != null)
            {
                _lastInput = formattedValue;
                _textBox.Text = formattedValue;
            }
        }

        /// <summary>
        /// Update of the Value property.
        /// </summary>
        internal void UpdateValue()
        {
            if (_inputValue != Value)
            {
                Value = (decimal)CoerceValue(this, _inputValue);
            }
        }

        private void ReturnPreviousInput()
        {
            int selectionLenght = _textBox.SelectionLength;
            int selectionStart = _textBox.SelectionStart;

            _textBox.Text = _lastInput;
            _textBox.SelectionStart = (selectionStart == 0) ? 0 : (selectionStart - 1);
            _textBox.SelectionLength = selectionLenght;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Exposes <see cref="TAlex.WPF.Controls.NumericUpDown"/> types to UI Automation.
    /// </summary>
    public class NumericUpDownAutomationPeer : FrameworkElementAutomationPeer, IRangeValueProvider
    {
        #region Properties

        /// <summary>
        /// Gets the value of the control.
        /// </summary>
        double IRangeValueProvider.Value
        {
            get
            {
                return (double)GetOwner().Value;
            }
        }

        /// <summary>
        /// Gets the minimum range value supported by the control.
        /// </summary>
        double IRangeValueProvider.Minimum
        {
            get
            {
                return (double)GetOwner().Minimum;
            }
        }

        /// <summary>
        /// Gets the maximum range value supported by the control.
        /// </summary>
        double IRangeValueProvider.Maximum
        {
            get
            {
                return (double)GetOwner().Maximum;
            }
        }

        /// <summary>
        /// Gets the value that is added to or subtracted from the <see cref="P:System.Windows.Automation.Provider.IRangeValueProvider.Value" />
        /// property when a small change is made, such as with an arrow key.
        /// </summary>
        double IRangeValueProvider.SmallChange
        {
            get
            {
                return (double)GetOwner().Increment;
            }
        }

        /// <summary>
        /// Gets the value that is added to or subtracted from the <see cref="P:System.Windows.Automation.Provider.IRangeValueProvider.Value" />
        /// property when a large change is made, such as with the PAGE DOWN key.
        /// </summary>
        double IRangeValueProvider.LargeChange
        {
            get
            {
                return (double)GetOwner().Increment;
            }
        }

        /// <summary>
        /// Gets a value that specifies whether the value of a control is read-only.
        /// </summary>
        bool IRangeValueProvider.IsReadOnly
        {
            get
            {
                return GetOwner().IsReadOnly;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Controls.NumericUpDownAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="TAlex.WPF.Controls.NumericUpDown"/> that is associated with this <see cref="TAlex.WPF.Controls.NumericUpDownAutomationPeer"/>.</param>
        public NumericUpDownAutomationPeer(NumericUpDown owner)
            : base(owner)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the control pattern for the <see cref="TAlex.WPF.Controls.NumericUpDown"/> that is associated with this <see cref="TAlex.WPF.Controls.NumericUpDownAutomationPeer"/>.
        /// </summary>
        /// <param name="patternInterface">A value in the enumeration.</param>
        /// <returns></returns>
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.RangeValue)
            {
                return this;
            }
            return base.GetPattern(patternInterface);
        }

        /// <summary>
        /// Returns the name of the <see cref="TAlex.WPF.Controls.NumericUpDown"/> that is associated with this <see cref="TAlex.WPF.Controls.NumericUpDownAutomationPeer"/>.
        /// </summary>
        /// <returns>A string that contains "NumericUpDown".</returns>
        protected override string GetClassNameCore()
        {
            return "NumericUpDown";
        }

        /// <summary>
        /// Returns the control type for the <see cref="TAlex.WPF.Controls.NumericUpDown"/> that is associated with this <see cref="TAlex.WPF.Controls.NumericUpDownAutomationPeer"/>.
        /// </summary>
        /// <returns>Spinner.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Spinner;
        }

        internal void RaiseValueChangedEvent(decimal oldValue, decimal newValue)
        {
            base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.ValueProperty,
                (double)oldValue, (double)newValue);
        }

        /// <summary>
        /// Returns the <see cref="TAlex.WPF.Controls.NumericUpDown"/> that is associated with this <see cref="TAlex.WPF.Controls.NumericUpDownAutomationPeer"/>.
        /// </summary>
        /// <returns>The <see cref="TAlex.WPF.Controls.NumericUpDown"/> that is associated with this <see cref="TAlex.WPF.Controls.NumericUpDownAutomationPeer"/>.</returns>
        private NumericUpDown GetOwner()
        {
            return (NumericUpDown)base.Owner;
        }

        /// <summary>
        /// Sets the value of the control.
        /// </summary>
        /// <param name="value">The value to set.</param>
        void IRangeValueProvider.SetValue(double value)
        {
            if (!IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            decimal val = (decimal)value;
            NumericUpDown control = GetOwner();

            if (val < control.Minimum || val > control.Maximum)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            control.Value = val;
        }

        #endregion
    }
}