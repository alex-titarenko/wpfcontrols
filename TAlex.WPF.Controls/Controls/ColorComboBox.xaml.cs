using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection;

using TAlex.WPF.Media;
using TAlex.WPF.CommonDialogs;


namespace TAlex.WPF.Controls
{
    /// <summary>
    /// Represents a color selection control with a drop-down list of predefined colors
    /// that can be shown or hidden by clicking the arrow on the control.
    /// </summary>
    public partial class ColorComboBox : UserControl
    {
        #region Fields

        private const string MoreColorsItemText = "More colors...";

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorComboBox.SelectedColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorComboBox.SelectedBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedBrushProperty;

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.Controls.ColorComboBox.SelectedColorChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent SelectedColorChangedEvent;

        private bool _containsCustomColors = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color selected by the user.
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
        /// Gets or sets the solid brush selected by the user.
        /// </summary>
        public Brush SelectedBrush
        {
            get
            {
                return (Brush)GetValue(SelectedBrushProperty);
            }

            set
            {
                SetValue(SelectedBrushProperty, value);
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

        static ColorComboBox()
        {
            SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorComboBox), new PropertyMetadata(OnSelectedColorPropertyChanged));
            SelectedBrushProperty = DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(ColorComboBox), new PropertyMetadata(OnSelectedBrushPropertyChanged));

            SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorComboBox));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Controls.ColorComboBox"/> class.
        /// </summary>
        public ColorComboBox()
        {
            InitializeComponent();
            InitializeColorList();
        }

        #endregion

        #region Methods

        #region Event Handlers

        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ColorComboBox ccb = d as ColorComboBox;

            Color newColor = (Color)args.NewValue;
            Color oldColor = (Color)args.OldValue;

            if (newColor == oldColor)
                return;

            // When the SelectedColor changes, set the selected value of the combo box
            ColorItem selectedColorItem = ccb.colorList.SelectedValue as ColorItem;
            if (selectedColorItem == null || selectedColorItem.Color != newColor)
            {
                ColorItem colorItem = ccb.ContainsColor(newColor);

                // Add the color if not found
                if (colorItem == null)
                {
                    if (!ccb._containsCustomColors)
                    {
                        ccb._containsCustomColors = true;
                        ccb.colorList.Items.Add(new Separator());
                    }

                    colorItem = ColorUtilities.GetColorItem(newColor, ColorUtilities.GetHexCode(newColor));
                    ccb.colorList.Items.Add(colorItem);
                }

                ccb.colorList.SelectedValue = colorItem;
            }

            // Also update the brush
            ccb.SelectedBrush = new SolidColorBrush(newColor);
            ccb.OnSelectedColorChanged(oldColor, newColor);
        }

        private static void OnSelectedBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ColorComboBox ccb = (ColorComboBox)d;
            SolidColorBrush newBrush = (SolidColorBrush)args.NewValue;

            if (ccb.SelectedColor != newBrush.Color)
                ccb.SelectedColor = newBrush.Color;
        }

        /// <summary>
        /// Raises the <see cref="TAlex.WPF.Controls.ColorComboBox.SelectedColorChanged"/> event.
        /// </summary>
        /// <param name="oldValue">Old value of the color.</param>
        /// <param name="newValue">New value of the color.</param>
        protected virtual void OnSelectedColorChanged(Color oldValue, Color newValue)
        {
            RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldValue, newValue);
            args.RoutedEvent = ColorComboBox.SelectedColorChangedEvent;
            RaiseEvent(args);
        }

        private void colorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            ColorItem item = e.AddedItems[0] as ColorItem;

            if (item != null)
            {
                SelectedColor = item.Color;
            }
            else
            {
                if (e.RemovedItems.Count != 0)
                {
                    colorList.SelectedValue = e.RemovedItems[0];
                }
                else
                {
                    int nextItemIndex = -1;
                    for (int i = 0; i < colorList.Items.Count; i++)
                    {
                        if (colorList.Items[i] is ColorItem)
                        {
                            nextItemIndex = i;
                            break;
                        }
                    }

                    colorList.SelectedIndex = nextItemIndex;
                }
            }
        }

        private void moreColorsItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowColorPickerDialog();
        }

        private void moreColorsItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowColorPickerDialog();
            }
        }

        #endregion

        #region Helpers

        private void ShowColorPickerDialog()
        {
            ColorPickerDialog cpd = new ColorPickerDialog();
            cpd.Owner = Window.GetWindow(this);
            cpd.SelectedColor = SelectedColor;

            if (cpd.ShowDialog() == true)
            {
                SelectedColor = cpd.SelectedColor;
            }
        }

        private ColorItem ContainsColor(Color newColor)
        {
            foreach (Object o in colorList.Items)
            {
                ColorItem item = o as ColorItem;

                if (item == null) continue;
                if (Color.AreClose(item.Color, newColor)) return item;
            }

            return null;
        }

        private void InitializeColorList()
        {
            colorList.Items.Clear();

            // Add an item to access the dialog box color selection
            ComboBoxItem moreColorsItem = new ComboBoxItem();
            moreColorsItem.HorizontalContentAlignment = HorizontalAlignment.Center;
            moreColorsItem.Content = new TextBlock() { Text = MoreColorsItemText };
            moreColorsItem.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(moreColorsItem_PreviewMouseLeftButtonUp);
            moreColorsItem.PreviewKeyDown += new KeyEventHandler(moreColorsItem_PreviewKeyDown);
            colorList.Items.Add(moreColorsItem);

            colorList.Items.Add(new Separator());

            // Add standard colors
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.DarkRed, "DarkRed"));
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.Red, "Red"));
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.Orange, "Orange"));
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.Yellow, "Yellow"));
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.LightGreen, "LightGreen"));
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.Green, "Green"));
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.LightBlue, "LightBlue"));
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.Blue, "Blue"));
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.DarkBlue, "DarkBlue"));
            colorList.Items.Add(ColorUtilities.GetColorItem(Colors.Purple, "Purple"));

            colorList.Items.Add(new Separator());

            // Add known colors
            ICollection<ColorItem> knownColors = ColorUtilities.GetKnownColorItems();
            foreach (ColorItem knownColor in knownColors)
            {
                colorList.Items.Add(knownColor);
            }
        }

        #endregion

        #endregion
    }
}
