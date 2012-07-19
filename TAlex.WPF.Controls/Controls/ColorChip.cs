using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

using TAlex.WPF.Media;
using TAlex.WPF.CommonDialogs;


namespace TAlex.WPF.Controls
{
    /// <summary>
    /// Represents one element of the color palette with the ability
    /// to assign custom color or choose one of the predefined colors.
    /// </summary>
    public class ColorChip : ContentControl
    {
        #region Fields

        private Border _selectedColorBorder;

        private Rectangle _selectedColorPreviewRectangle;

        private Popup _colorPalettesPopup;

        private ListBox _knownColorsListBox;

        private ListBox _standardColorsListBox;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorChip.SelectedColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorChip.PreviewAreaWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PreviewAreaWidthProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorChip.PreviewAreaHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PreviewAreaHeightProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorChip.ShowStandardToolTip"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowStandardToolTipProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorChip.ShowColorPalettesPopup"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowColorPalettesPopupProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorChip.ShowKnownColors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowKnownColorsProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorChip.ShowStandardColors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowStandardColorsProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorChip.SelectedColorChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent SelectedColorChangedEvent;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color selected by the user.
        /// This is a dependency property.
        /// </summary>
        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue(SelectedColorProperty);
            }

            set
            {
                SetValue(SelectedColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the preview area.
        /// This is a dependency property.
        /// </summary>
        public double PreviewAreaWidth
        {
            get
            {
                return (double)GetValue(PreviewAreaWidthProperty);
            }

            set
            {
                SetValue(PreviewAreaWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height of the preview area.
        /// This is a dependency property.
        /// </summary>
        public double PreviewAreaHeight
        {
            get
            {
                return (double)GetValue(PreviewAreaHeightProperty);
            }

            set
            {
                SetValue(PreviewAreaHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the standard tooltip.
        /// This is a dependency property.
        /// </summary>
        public bool ShowStandardToolTip
        {
            get
            {
                return (bool)GetValue(ShowStandardToolTipProperty);
            }

            set
            {
                SetValue(ShowStandardToolTipProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the color palettes popup.
        /// This is a dependency property.
        /// </summary>
        public bool ShowColorPalettesPopup
        {
            get
            {
                return (bool)GetValue(ShowColorPalettesPopupProperty);
            }

            set
            {
                SetValue(ShowColorPalettesPopupProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the known colors.
        /// This is a dependency property.
        /// </summary>
        public bool ShowKnownColors
        {
            get
            {
                return (bool)GetValue(ShowKnownColorsProperty);
            }

            set
            {
                SetValue(ShowKnownColorsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the standard colors.
        /// This is a dependency property.
        /// </summary>
        public bool ShowStandardColors
        {
            get
            {
                return (bool)GetValue(ShowStandardColorsProperty);
            }

            set
            {
                SetValue(ShowStandardColorsProperty, value);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the selected color has been changed.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
        {
            add { AddHandler(SelectedColorChangedEvent, value); }
            remove { RemoveHandler(SelectedColorChangedEvent, value); }
        }

        #endregion

        #region Constructors

        static ColorChip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorChip), new FrameworkPropertyMetadata(typeof(ColorChip)));

            SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorChip), new PropertyMetadata(OnSelectedColorPropertyChanged));
            PreviewAreaWidthProperty = DependencyProperty.Register("PreviewAreaWidth", typeof(double), typeof(ColorChip), new PropertyMetadata(18.0));
            PreviewAreaHeightProperty = DependencyProperty.Register("PreviewAreaHeight", typeof(double), typeof(ColorChip), new PropertyMetadata(18.0));
            ShowStandardToolTipProperty = DependencyProperty.Register("ShowStandardToolTip", typeof(bool), typeof(ColorChip), new PropertyMetadata(true, OnShowStandardToolTipPropertyChanged));
            ShowColorPalettesPopupProperty = DependencyProperty.Register("ShowColorPalettesPopup", typeof(bool), typeof(ColorChip), new PropertyMetadata(true));
            ShowKnownColorsProperty = DependencyProperty.Register("ShowKnownColors", typeof(bool), typeof(ColorChip), new PropertyMetadata(true));
            ShowStandardColorsProperty = DependencyProperty.Register("ShowStandardColors", typeof(bool), typeof(ColorChip), new PropertyMetadata(true));

            SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorChip));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Controls.ColorChip"/> class.
        /// </summary>
        public ColorChip()
        {
        }

        #endregion

        #region Methods

        #region Event Handlers

        /// <summary>
        /// Called when the template's tree is generated.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _selectedColorBorder = (Border)base.GetTemplateChild("selectedColorBorder");
            _selectedColorPreviewRectangle = (Rectangle)base.GetTemplateChild("selectedColorPreviewRectangle");
            _colorPalettesPopup = (Popup)base.GetTemplateChild("colorsListPopup");
            _knownColorsListBox = (ListBox)base.GetTemplateChild("knownColorsListBox");
            _standardColorsListBox = (ListBox)base.GetTemplateChild("standardColorsListBox");

            if (_selectedColorBorder != null)
            {
                UpdateStandardToolTip();
            }

            if (_selectedColorPreviewRectangle != null)
            {
                _selectedColorPreviewRectangle.Fill = new SolidColorBrush(SelectedColor);
            }

            if (_colorPalettesPopup != null)
            {
                _colorPalettesPopup.Opened += new EventHandler(colorsListPopup_Opened);
                _colorPalettesPopup.MouseLeftButtonUp += new MouseButtonEventHandler(colorPalettesPopup_MouseLeftButtonUp);
            }

            if (_knownColorsListBox != null)
            {
                InitializeKnownColorsWithoutTransparentList();
                _knownColorsListBox.MouseLeftButtonUp += new MouseButtonEventHandler(colorsListPopup_MouseLeftButtonUp);
            }

            if (_standardColorsListBox != null)
            {
                InitializeStandardColorsList();
                _standardColorsListBox.MouseLeftButtonUp += new MouseButtonEventHandler(colorsListPopup_MouseLeftButtonUp);
            }
        }


        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ColorChip cp = d as ColorChip;

            Color newColor = (Color)args.NewValue;
            Color oldColor = (Color)args.OldValue;

            if (newColor == oldColor)
                return;

            if (cp._selectedColorPreviewRectangle != null)
                cp._selectedColorPreviewRectangle.Fill = new SolidColorBrush(newColor);

            cp.UpdateStandardToolTip();

            cp.OnSelectedColorChanged(oldColor, newColor);
        }

        private static void OnShowStandardToolTipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorChip)d).UpdateStandardToolTip();
        }

        /// <summary>
        /// Raises the <see cref="TAlex.WPF.Controls.ColorChip.SelectedColorChanged"/> event.
        /// </summary>
        /// <param name="oldValue">Old value of the color.</param>
        /// <param name="newValue">New value of the color.</param>
        protected virtual void OnSelectedColorChanged(Color oldValue, Color newValue)
        {
            RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldValue, newValue);
            args.RoutedEvent = ColorComboBox.SelectedColorChangedEvent;
            RaiseEvent(args);
        }


        /// <summary>
        /// Responds to the <see cref="UIElement.MouseLeftButtonUp"/> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            ShowColorPickerDialog();
        }

        /// <summary>
        /// Responds to the <see cref="UIElement.OnMouseRightButtonUp"/> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);
            ShowPopup();
        }


        private void colorsListPopup_Opened(object sender, EventArgs e)
        {
            if (_knownColorsListBox != null)
                _knownColorsListBox.SelectedIndex = -1;               

            if (_standardColorsListBox != null && ShowStandardColors)
            {
                _standardColorsListBox.SelectedIndex = -1;

                foreach (ColorItem item in _standardColorsListBox.Items)
                {
                    if (Color.AreClose(item.Color, SelectedColor))
                    {
                        _standardColorsListBox.SelectedItem = item;
                        return;
                    }
                }
            }

            if (_knownColorsListBox != null)
            {
                foreach (ColorItem item in _knownColorsListBox.Items)
                {
                    if (Color.AreClose(item.Color, SelectedColor))
                    {
                        _knownColorsListBox.SelectedItem = item;
                        return;
                    }
                }
            }
        }

        private void colorsListPopup_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            
            if (listBox != null)
            {
                var listItem = Helpers.WPFVisualHelper.FindAncestor<ListBoxItem>(e.OriginalSource as DependencyObject);

                if (listItem != null)
                {
                    ColorItem colorItem = listItem.DataContext as ColorItem;

                    if (colorItem != null)
                    {
                        SelectedColor = colorItem.Color;
                        listBox.SelectedIndex = -1;
                        ClosePopup();
                    }
                }
            }
        }

        private void colorPalettesPopup_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region Helpers

        private static string GetToolTip(Color newColor)
        {
            StringBuilder sb = new StringBuilder();

            string name;

            if (ColorUtilities.IsKnownColor(newColor, out name))
                sb.AppendLine(name);
            else
                sb.AppendLine(ColorUtilities.GetHexCode(newColor));

            sb.AppendFormat("Argb: {0}, {1}, {2}, {3}", newColor.A, newColor.R, newColor.G, newColor.B);

            return sb.ToString();
        }

        private void UpdateStandardToolTip()
        {
            if (_selectedColorBorder != null)
            {
                if (ShowStandardToolTip)
                {
                    _selectedColorBorder.ToolTip = GetToolTip(SelectedColor);
                }
                else
                {
                    _selectedColorBorder.ToolTip = null;
                }
            }
        }

        private void ShowColorPickerDialog()
        {
            ColorPickerDialog cpd = new ColorPickerDialog();
            cpd.SelectedColor = SelectedColor;
            cpd.Owner = Window.GetWindow(this);

            if (cpd.ShowDialog() == true)
            {
                SelectedColor = cpd.SelectedColor;
            }
        }

        private void ShowPopup()
        {
            if (ShowColorPalettesPopup)
            {
                if (ShowKnownColors || ShowStandardColors)
                {
                    _colorPalettesPopup.IsOpen = true;
                }
            }
        }

        private void ClosePopup()
        {
            _colorPalettesPopup.IsOpen = false;
        }

        private void InitializeKnownColorsWithoutTransparentList()
        {
            _knownColorsListBox.ItemsSource = ColorUtilities.GetKnownColorsWithoutTransparent();
        }

        private void InitializeStandardColorsList()
        {
            _standardColorsListBox.Items.Clear();

            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Black, "Black"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Gray, "Gray"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.DarkRed, "DarkRed"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Red, "Red"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.OrangeRed, "OrangeRed"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Yellow, "Yellow"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Green, "Green"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.DodgerBlue, "DodgerBlue"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Blue, "Blue"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Purple, "Purple"));

            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.White, "White"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Silver, "Silver"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Peru, "Peru"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Pink, "Pink"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Gold, "Gold"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Wheat, "Wheat"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.Lime, "Lime"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.PowderBlue, "PowderBlue"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.CornflowerBlue, "CornflowerBlue"));
            _standardColorsListBox.Items.Add(ColorUtilities.GetColorItem(Colors.MediumPurple, "MediumPurple"));
        }

        #endregion

        #endregion
    }
}
