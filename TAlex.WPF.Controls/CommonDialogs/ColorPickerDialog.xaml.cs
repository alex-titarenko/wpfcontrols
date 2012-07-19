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
using System.Windows.Threading;
using System.ComponentModel;

using TAlex.WPF.Media;


namespace TAlex.WPF.CommonDialogs
{
    /// <summary>
    /// Prompts the user to choose a custom color.
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        #region Fields

        private HsvColor _hsvColor;

        private bool _isHsvColorUpdateNeeded = true;

        private bool _isColorDetailsMarkerDraged = false;
        private bool _isChannelLevelValueDraged = false;
        private bool _isHexValueEdited = false;

        private string _lastHexValueInput = String.Empty;

        private ColorSelector _colorSelector = new HsbHColorSelector();

        /// <summary>
        /// Identifies the <see cref="TAlex.WPF.CommonDialogs.ColorPickerDialog.SelectedColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty;

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

        private HsvColor HsvColor
        {
            get
            {
                return _hsvColor;
            }

            set
            {
                _hsvColor = value;
                _isHsvColorUpdateNeeded = false;
                Color rgbColor = value.ToRgb();

                if (SelectedColor != rgbColor)
                    SelectedColor = value.ToRgb();
                else
                    OnSelectedColorPropertyChanged(rgbColor);

                _isHsvColorUpdateNeeded = true;
            }
        }

        #endregion

        #region Constructors

        static ColorPickerDialog()
        {
            SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPickerDialog), new PropertyMetadata(SelectedColorPropertyChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.CommonDialogs.ColorPickerDialog"/> class.
        /// </summary>
        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #region Event Handlers

        #region Window

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OnUpdate(_hsvColor);
            currentColorPreviewBorder.Background = new SolidColorBrush(SelectedColor);
        }

        #endregion

        private static void SelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPickerDialog)d).OnSelectedColorPropertyChanged((Color)e.NewValue);
        }

        private void OnSelectedColorPropertyChanged(Color newColor)
        {
            if (_isHsvColorUpdateNeeded)
            {
                _hsvColor = HsvColor.FromRgb(SelectedColor);
                _isHsvColorUpdateNeeded = false;
            }

            OnUpdate(_hsvColor);
        }

        private void OnUpdate(HsvColor newHsvColor)
        {
            Color newRgbColor = newHsvColor.ToRgb();

            // Update color details
            if (!_isColorDetailsMarkerDraged)
            {
                UpdateColorDetailsMarkerPosition(_colorSelector.GetColorDetailsMarkerCoord(newHsvColor));
                colorDetailsBorder.Background = _colorSelector.GetColorDetailsBrush(newHsvColor);
            }
            
            // Update channel level
            if (!_isChannelLevelValueDraged)
            {
                UpdateChannelLevelPosition(_colorSelector.GetChanellLevelValue(newHsvColor));
                channelLevelBg.Fill = _colorSelector.GetChanellLevelBrush(newHsvColor);
            }
            
            // Update selected color preview
            selectedColorPreviewBorder.Background = new SolidColorBrush(newRgbColor);

            // Update opacity slider
            if (opacitySlider.Value != newHsvColor.A)
            {
                opacitySlider.ValueChanged -= opacitySlider_ValueChanged;
                opacitySlider.Value = newHsvColor.A;
                opacitySlider.ValueChanged += opacitySlider_ValueChanged;
            }

            // Updating the RGB channels
            if ((byte)redChannelNumericUpDown.Value != newRgbColor.R)
            {
                redChannelNumericUpDown.ValueChanged -= redChannelNumericUpDown_ValueChanged;
                redChannelNumericUpDown.Value = newRgbColor.R;
                redChannelNumericUpDown.ValueChanged += redChannelNumericUpDown_ValueChanged;
            }

            if ((byte)greenChannelNumericUpDown.Value != newRgbColor.G)
            {
                greenChannelNumericUpDown.ValueChanged -= greenChannelNumericUpDown_ValueChanged;
                greenChannelNumericUpDown.Value = newRgbColor.G;
                greenChannelNumericUpDown.ValueChanged += greenChannelNumericUpDown_ValueChanged;
            }

            if ((byte)blueChannelNumericUpDown.Value != newRgbColor.B)
            {
                blueChannelNumericUpDown.ValueChanged -= blueChannelNumericUpDown_ValueChanged;
                blueChannelNumericUpDown.Value = newRgbColor.B;
                blueChannelNumericUpDown.ValueChanged += blueChannelNumericUpDown_ValueChanged;
            }

            // Updating the HSB channels
            if ((int)hueNumericUpDown.Value != newHsvColor.IntH)
            {
                hueNumericUpDown.ValueChanged -= hueNumericUpDown_ValueChanged;
                hueNumericUpDown.Value = newHsvColor.IntH;
                hueNumericUpDown.ValueChanged += hueNumericUpDown_ValueChanged;
            }

            if ((int)saturationNumericUpDown.Value != newHsvColor.IntS)
            {
                saturationNumericUpDown.ValueChanged -= saturationNumericUpDown_ValueChanged;
                saturationNumericUpDown.Value = newHsvColor.IntS;
                saturationNumericUpDown.ValueChanged += saturationNumericUpDown_ValueChanged;
            }

            if ((int)brightnessNumericUpDown.Value != newHsvColor.IntV)
            {
                brightnessNumericUpDown.ValueChanged -= brightnessNumericUpDown_ValueChanged;
                brightnessNumericUpDown.Value = newHsvColor.IntV;
                brightnessNumericUpDown.ValueChanged += brightnessNumericUpDown_ValueChanged;
            }

            // Updates hexadecimal color value
            if (!_isHexValueEdited)
            {
                string hexCode = ColorUtilities.GetHexCode(newRgbColor);
                if (hexValueTextBox.Text != hexCode)
                {
                    hexValueTextBox.TextChanged -= hexValueTextBox_TextChanged;
                    hexValueTextBox.Text = hexCode;
                    hexValueTextBox.TextChanged += hexValueTextBox_TextChanged;

                    _lastHexValueInput = hexCode;
                }
            }
        }


        private void OnColorSelectorUpdate()
        {
            colorDetailsBorder.Background = _colorSelector.GetColorDetailsBrush(_hsvColor);
            UpdateColorDetailsMarkerPosition(_colorSelector.GetColorDetailsMarkerCoord(_hsvColor));

            channelLevelBg.Fill = _colorSelector.GetChanellLevelBrush(_hsvColor);
            UpdateChannelLevelPosition(_colorSelector.GetChanellLevelValue(_hsvColor));
        }


        private void colorDetailsGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Dispatcher.Thread.Priority = System.Threading.ThreadPriority.Lowest;
            sampleSelector.CaptureMouse();
        }

        private void colorDetailsGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            sampleSelector.ReleaseMouseCapture();
        }

        private void colorDetailsGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition((UIElement)sender);

                pos.X = Math.Min(Math.Max(0, pos.X), colorDetailsGrid.ActualWidth);
                pos.Y = Math.Min(Math.Max(0, pos.Y), colorDetailsGrid.ActualHeight);

                Point coord = new Point(pos.X / colorDetailsGrid.ActualWidth, 1.0 - pos.Y / colorDetailsGrid.ActualHeight);
                UpdateColorDetailsMarkerPosition(coord);

                _isColorDetailsMarkerDraged = true;
                HsvColor = _colorSelector.GetColorByColorDetailsMarkerCoord(_hsvColor, coord);
                _isColorDetailsMarkerDraged = false;
            }
        }


        private void channelLevelControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            channelLevelThumb.CaptureMouse();
        }

        private void channelLevelControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            channelLevelThumb.ReleaseMouseCapture();
        }

        private void channelLevelControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition((UIElement)sender);

                pos.Y = Math.Min(Math.Max(0, pos.Y), channelLevelControl.ActualHeight);

                double value = 1.0 - pos.Y / channelLevelControl.ActualHeight;
                UpdateChannelLevelPosition(value);

                _isChannelLevelValueDraged = true;
                HsvColor = _colorSelector.GetColorByChannelLevelValue(_hsvColor, value);
                _isChannelLevelValueDraged = false;
            }
        }


        private void currentColorPreviewBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush brush = currentColorPreviewBorder.Background as SolidColorBrush;

            if (brush != null)
            {
                SelectedColor = brush.Color;
            }
        }


        private void opacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (redChannelNumericUpDown != null)
            {
                HsvColor newColor = HsvColor;
                newColor.A = e.NewValue;

                if (HsvColor != newColor)
                {
                    HsvColor = newColor;
                }
            }
        }


        private void rgbRRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _colorSelector = new RgbRColorSelector();
            OnColorSelectorUpdate();
        }

        private void rgbGRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _colorSelector = new RgbGColorSelector();
            OnColorSelectorUpdate();
        }

        private void rgbBRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _colorSelector = new RgbBColorSelector();
            OnColorSelectorUpdate();
        }


        private void hsbHRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _colorSelector = new HsbHColorSelector();
            OnColorSelectorUpdate();
        }

        private void hsbSRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _colorSelector = new HsbSColorSelector();
            OnColorSelectorUpdate();
        }

        private void hsbBRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _colorSelector = new HsbBColorSelector();
            OnColorSelectorUpdate();
        }


        private void redChannelNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            Color newColor = Color.FromArgb(SelectedColor.A, (byte)e.NewValue, SelectedColor.G, SelectedColor.B);

            if (!ColorUtilities.FuzzyColorEquals(SelectedColor, newColor))
            {
                HsvColor = HsvColor.FromRgb(newColor);
            }
        }

        private void greenChannelNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            Color newColor = Color.FromArgb(SelectedColor.A, SelectedColor.R, (byte)e.NewValue, SelectedColor.B);

            if (!ColorUtilities.FuzzyColorEquals(SelectedColor, newColor))
            {
                HsvColor = HsvColor.FromRgb(newColor);
            }
        }

        private void blueChannelNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            Color newColor = Color.FromArgb(SelectedColor.A, SelectedColor.R, SelectedColor.G, (byte)e.NewValue);

            if (!ColorUtilities.FuzzyColorEquals(SelectedColor, newColor))
            {
                HsvColor = HsvColor.FromRgb(newColor);
            }
        }


        private void hueNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            HsvColor newHsvColor = new HsvColor(_hsvColor.A, (double)e.NewValue, _hsvColor.S, _hsvColor.V);

            if (!ColorUtilities.FuzzyColorEquals(HsvColor, newHsvColor))
            {
                HsvColor = newHsvColor;
            }
        }

        private void saturationNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            HsvColor newHsvColor = new HsvColor(_hsvColor.A, _hsvColor.H, (double)(e.NewValue / 100), _hsvColor.V);

            if (!ColorUtilities.FuzzyColorEquals(HsvColor, newHsvColor))
            {
                HsvColor = newHsvColor;
            }
        }

        private void brightnessNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            HsvColor newHsvColor = new HsvColor(_hsvColor.A, _hsvColor.H, _hsvColor.S, (double)(e.NewValue / 100));

            if (!ColorUtilities.FuzzyColorEquals(HsvColor, newHsvColor))
            {
                HsvColor = newHsvColor;
            }
        }


        private void hexValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = hexValueTextBox.Text.Trim();

            if (String.IsNullOrEmpty(text) || text == "#")
            {
                return;
            }

            Color c;

            if (ColorUtilities.TryParseColor(text, out c))
            {
                _isHexValueEdited = true;
                HsvColor = HsvColor.FromRgb(c);
                _isHexValueEdited = false;

                _lastHexValueInput = text;
            }
            else
            {
                // If the operation parsing of color ended in failure, then returns previous input.
                int selectionLenght = hexValueTextBox.SelectionLength;
                int selectionStart = hexValueTextBox.SelectionStart;

                hexValueTextBox.TextChanged -= hexValueTextBox_TextChanged;
                hexValueTextBox.Text = _lastHexValueInput;
                hexValueTextBox.TextChanged += hexValueTextBox_TextChanged;

                hexValueTextBox.SelectionStart = (selectionStart == 0) ? 0 : (selectionStart - 1);
                hexValueTextBox.SelectionLength = selectionLenght;
            }
        }

        private void hexValueTextBox_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
                hexValueTextBox.TextChanged -= hexValueTextBox_TextChanged;
                hexValueTextBox.Text = _lastHexValueInput;
                hexValueTextBox.TextChanged += hexValueTextBox_TextChanged;
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

        private void UpdateColorDetailsMarkerPosition(Point coord)
        {
            Point pos = new Point(coord.X * colorDetailsGrid.ActualWidth, (1 - coord.Y) * colorDetailsGrid.ActualHeight);

            double halfHeight = sampleSelector.Height / 2;
            sampleSelector.SetValue(Canvas.LeftProperty, pos.X - halfHeight);
            sampleSelector.SetValue(Canvas.TopProperty, pos.Y - halfHeight);
        }

        private void UpdateChannelLevelPosition(double value)
        {
            channelLevelControl.RowDefinitions[0].Height = new GridLength(1 - value, GridUnitType.Star);
            channelLevelControl.RowDefinitions[2].Height = new GridLength(value, GridUnitType.Star);
        }

        #endregion

        #endregion

        #region Nested types

        abstract class ColorSelector
        {
            #region Methods

            public abstract Point GetColorDetailsMarkerCoord(HsvColor selectedColor);

            public abstract Brush GetColorDetailsBrush(HsvColor selectedColor);


            public abstract double GetChanellLevelValue(HsvColor selectedColor);

            public abstract Brush GetChanellLevelBrush(HsvColor selectedColor);


            public abstract HsvColor GetColorByColorDetailsMarkerCoord(HsvColor selectedColor, Point coord);

            public abstract HsvColor GetColorByChannelLevelValue(HsvColor selectedColor, double value);

            #endregion
        }


        abstract class RgbColorSelector : ColorSelector
        {
            #region Fields

            protected const int ByteValues = 256;

            protected int[] _pixels = new int[ByteValues * ByteValues];

            protected WriteableBitmap _wb = new WriteableBitmap(ByteValues, ByteValues, 96, 96, PixelFormats.Bgra32, null);

            protected readonly Int32Rect _sourceRect = new Int32Rect(0, 0, ByteValues, ByteValues);

            #endregion
        }

        class RgbRColorSelector : RgbColorSelector
        {
            #region Methods

            public override Point GetColorDetailsMarkerCoord(HsvColor c)
            {
                Color rbgColor = c.ToRgb();
                return new Point(ColorUtilities.ScRgbTosRgb(rbgColor.ScB), ColorUtilities.ScRgbTosRgb(rbgColor.ScG));
            }

            public override Brush GetColorDetailsBrush(HsvColor c)
            {
                int r = c.ToRgb().R;

                for (int g = 0; g < ByteValues; g++)
                {
                    for (int b = 0; b < ByteValues; b++)
                    {
                        _pixels[g * ByteValues + b] = ColorUtilities.ColorToBgra32(r, byte.MaxValue - g, b);
                    }
                }

                _wb.WritePixels(_sourceRect, _pixels, ByteValues * 4, 0);

                ImageBrush br = new ImageBrush(_wb);
                return br;
            }

            public override double GetChanellLevelValue(HsvColor c)
            {
                return ColorUtilities.ScRgbTosRgb(c.ToRgb().ScR);
            }

            public override Brush GetChanellLevelBrush(HsvColor c)
            {
                LinearGradientBrush br = new LinearGradientBrush();
                br.StartPoint = new Point(0, 1);
                br.EndPoint = new Point(0, 0);

                Color rgbColor = c.ToRgb();
                br.GradientStops.Add(new GradientStop(Color.FromScRgb(1, 0, rgbColor.ScG, rgbColor.ScB), 0));
                br.GradientStops.Add(new GradientStop(Color.FromScRgb(1, 1, rgbColor.ScG, rgbColor.ScB), 1));

                return br;
            }

            public override HsvColor GetColorByColorDetailsMarkerCoord(HsvColor c, Point coord)
            {
                Color rgbColor = c.ToRgb();
                return HsvColor.FromRgb(Color.FromScRgb(rgbColor.ScA, rgbColor.ScR, ColorUtilities.sRgbToScRgb((float)coord.Y), ColorUtilities.sRgbToScRgb((float)coord.X)));
            }

            public override HsvColor GetColorByChannelLevelValue(HsvColor c, double value)
            {
                Color rgbColor = c.ToRgb();
                return HsvColor.FromRgb(Color.FromScRgb(rgbColor.ScA, ColorUtilities.sRgbToScRgb((float)value), rgbColor.ScG, rgbColor.ScB));
            }

            #endregion
        }

        class RgbGColorSelector : RgbColorSelector
        {
            #region Methods

            public override Point GetColorDetailsMarkerCoord(HsvColor c)
            {
                Color rgbColor = c.ToRgb();
                return new Point(ColorUtilities.ScRgbTosRgb(rgbColor.ScB), ColorUtilities.ScRgbTosRgb(rgbColor.ScR));
            }

            public override Brush GetColorDetailsBrush(HsvColor c)
            {
                int g = c.ToRgb().G;

                for (int r = 0; r < ByteValues; r++)
                {
                    for (int b = 0; b < ByteValues; b++)
                    {
                        _pixels[r * ByteValues + b] = ColorUtilities.ColorToBgra32(byte.MaxValue - r, g, b);
                    }
                }

                _wb.WritePixels(_sourceRect, _pixels, ByteValues * 4, 0);

                ImageBrush br = new ImageBrush(_wb);
                return br;
            }

            public override double GetChanellLevelValue(HsvColor c)
            {
                return ColorUtilities.ScRgbTosRgb(c.ToRgb().ScG);
            }

            public override Brush GetChanellLevelBrush(HsvColor c)
            {
                LinearGradientBrush br = new LinearGradientBrush();
                br.StartPoint = new Point(0, 1);
                br.EndPoint = new Point(0, 0);

                Color rgbColor = c.ToRgb();
                br.GradientStops.Add(new GradientStop(Color.FromScRgb(1, rgbColor.ScR, 0, rgbColor.ScB), 0));
                br.GradientStops.Add(new GradientStop(Color.FromScRgb(1, rgbColor.ScR, 1, rgbColor.ScB), 1));

                return br;
            }

            public override HsvColor GetColorByColorDetailsMarkerCoord(HsvColor c, Point coord)
            {
                Color rgbColor = c.ToRgb();
                return HsvColor.FromRgb(Color.FromScRgb(rgbColor.ScA, ColorUtilities.sRgbToScRgb((float)coord.Y), rgbColor.ScG, ColorUtilities.sRgbToScRgb((float)coord.X)));
            }

            public override HsvColor GetColorByChannelLevelValue(HsvColor c, double value)
            {
                Color rgbColor = c.ToRgb();
                return HsvColor.FromRgb(Color.FromScRgb(rgbColor.ScA, rgbColor.ScR, ColorUtilities.sRgbToScRgb((float)value), rgbColor.ScB));
            }

            #endregion
        }

        class RgbBColorSelector : RgbColorSelector
        {
            #region Methods

            public override Point GetColorDetailsMarkerCoord(HsvColor c)
            {
                Color rgbColor = c.ToRgb();
                return new Point(ColorUtilities.ScRgbTosRgb(rgbColor.ScR), ColorUtilities.ScRgbTosRgb(rgbColor.ScG));
            }

            public override Brush GetColorDetailsBrush(HsvColor c)
            {
                int b = c.ToRgb().B;

                for (int r = 0; r < ByteValues; r++)
                {
                    for (int g = 0; g < ByteValues; g++)
                    {
                        _pixels[g * ByteValues + r] = ColorUtilities.ColorToBgra32(r, byte.MaxValue - g, b);
                    }
                }

                _wb.WritePixels(_sourceRect, _pixels, ByteValues * 4, 0);

                ImageBrush br = new ImageBrush(_wb);
                return br;
            }

            public override double GetChanellLevelValue(HsvColor c)
            {
                return ColorUtilities.ScRgbTosRgb(c.ToRgb().ScB);
            }

            public override Brush GetChanellLevelBrush(HsvColor c)
            {
                LinearGradientBrush br = new LinearGradientBrush();
                br.StartPoint = new Point(0, 1);
                br.EndPoint = new Point(0, 0);

                Color rgbColor = c.ToRgb();
                br.GradientStops.Add(new GradientStop(Color.FromScRgb(1, rgbColor.ScR, rgbColor.ScG, 0), 0));
                br.GradientStops.Add(new GradientStop(Color.FromScRgb(1, rgbColor.ScR, rgbColor.ScG, 1), 1));

                return br;
            }

            public override HsvColor GetColorByColorDetailsMarkerCoord(HsvColor c, Point coord)
            {
                Color rgbColor = c.ToRgb();
                return HsvColor.FromRgb(Color.FromScRgb(rgbColor.ScA, ColorUtilities.sRgbToScRgb((float)coord.X), ColorUtilities.sRgbToScRgb((float)coord.Y), rgbColor.ScB));
            }

            public override HsvColor GetColorByChannelLevelValue(HsvColor c, double value)
            {
                Color rgbColor = c.ToRgb();
                return HsvColor.FromRgb(Color.FromScRgb(rgbColor.ScA, rgbColor.ScR, rgbColor.ScG, ColorUtilities.sRgbToScRgb((float)value)));
            }

            #endregion
        }


        abstract class HsbColorSelector : ColorSelector
        {
        }

        class HsbHColorSelector : HsbColorSelector
        {
            #region Fields

            private static readonly LinearGradientBrush _chanellLevelBrush;

            #endregion

            #region Constructors

            static HsbHColorSelector()
            {
                _chanellLevelBrush = new LinearGradientBrush();
                _chanellLevelBrush.StartPoint = new Point(0, 1);
                _chanellLevelBrush.EndPoint = new Point(0, 0);

                _chanellLevelBrush.GradientStops.Add(new GradientStop(Colors.Red, 0));
                _chanellLevelBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 1.0 / 6.0));
                _chanellLevelBrush.GradientStops.Add(new GradientStop(Colors.Lime, 2.0 / 6.0));
                _chanellLevelBrush.GradientStops.Add(new GradientStop(Colors.Cyan, 0.5));
                _chanellLevelBrush.GradientStops.Add(new GradientStop(Colors.Blue, 4.0 / 6.0));
                _chanellLevelBrush.GradientStops.Add(new GradientStop(Colors.Magenta, 5.0 / 6.0));
                _chanellLevelBrush.GradientStops.Add(new GradientStop(Colors.Red, 1));

                _chanellLevelBrush.Freeze();
            }

            #endregion

            #region Methods

            public override Point GetColorDetailsMarkerCoord(HsvColor hsvColor)
            {
                return new Point(hsvColor.S, hsvColor.V);
            }

            public override Brush GetColorDetailsBrush(HsvColor hsvColor)
            {
                Color hueColor = new HsvColor(hsvColor.H, 1, 1).ToRgb();

                GeometryDrawing geomDrawing1 = new GeometryDrawing();
                geomDrawing1.Geometry = new RectangleGeometry(new Rect(0, 0, 100, 100));
                geomDrawing1.Brush = new LinearGradientBrush(Colors.Black, hueColor, new Point(0.5, 1), new Point(0.5, 0));


                GeometryDrawing geomDrawing2 = new GeometryDrawing();
                geomDrawing2.Geometry = new RectangleGeometry(new Rect(0, 0, 100, 100));
                geomDrawing2.Brush = new LinearGradientBrush(Colors.White, Colors.Black, new Point(0.5, 0), new Point(0.5, 1));

                DrawingGroup drawingGroup1 = new DrawingGroup();
                drawingGroup1.Children.Add(geomDrawing2);
                drawingGroup1.OpacityMask = new LinearGradientBrush(Color.FromScRgb(1, 0, 0, 0), Color.FromScRgb(0, 0, 0, 0), new Point(0, 0.5), new Point(1, 0.5));


                DrawingGroup rootDrawingGroup = new DrawingGroup();
                rootDrawingGroup.Children.Add(geomDrawing1);
                rootDrawingGroup.Children.Add(drawingGroup1);

                DrawingBrush br = new DrawingBrush(rootDrawingGroup);
                br.Freeze();

                return br;
            }

            public override double GetChanellLevelValue(HsvColor hsvColor)
            {
                return hsvColor.H / HsvColor.MaxHueValue;
            }

            public override Brush GetChanellLevelBrush(HsvColor hsvColor)
            {
                return _chanellLevelBrush;
            }

            public override HsvColor GetColorByColorDetailsMarkerCoord(HsvColor hsvColor, Point coord)
            {
                return new HsvColor(hsvColor.A, hsvColor.H, coord.X, coord.Y);
            }

            public override HsvColor GetColorByChannelLevelValue(HsvColor hsvColor, double value)
            {
                return new HsvColor(hsvColor.A, value * HsvColor.MaxHueValue, hsvColor.S, hsvColor.V);
            }

            #endregion
        }

        class HsbSColorSelector : HsbColorSelector
        {
            #region Methods

            public override Point GetColorDetailsMarkerCoord(HsvColor hsvColor)
            {
                return new Point(hsvColor.H / HsvColor.MaxHueValue, hsvColor.V);
            }

            public override Brush GetColorDetailsBrush(HsvColor hsvColor)
            {
                GeometryDrawing geomDrawing1 = new GeometryDrawing();
                geomDrawing1.Geometry = new RectangleGeometry(new Rect(0, 0, 100, 100));
                geomDrawing1.Brush = ColorUtilities.GetHueBrash(hsvColor.S, 1.0);
                ((LinearGradientBrush)geomDrawing1.Brush).EndPoint = new Point(1, 0);

                GeometryDrawing geomDrawing2 = new GeometryDrawing();
                geomDrawing2.Geometry = new RectangleGeometry(new Rect(0, 0, 100, 100));
                geomDrawing2.Brush = new LinearGradientBrush(Colors.Black, Color.FromScRgb(0, 0, 0, 0), new Point(0.5, 1), new Point(0.5, 0));

                DrawingGroup rootDrawingGroup = new DrawingGroup();
                rootDrawingGroup.Children.Add(geomDrawing1);
                rootDrawingGroup.Children.Add(geomDrawing2);

                DrawingBrush br = new DrawingBrush(rootDrawingGroup);
                br.Freeze();

                return br;
            }

            public override double GetChanellLevelValue(HsvColor hsvColor)
            {
                return hsvColor.S;
            }

            public override Brush GetChanellLevelBrush(HsvColor hsvColor)
            {
                LinearGradientBrush br = new LinearGradientBrush();
                br.StartPoint = new Point(0, 1);
                br.EndPoint = new Point(0, 0);

                br.GradientStops.Add(new GradientStop(new HsvColor(hsvColor.H, 0, hsvColor.V).ToRgb(), 0));
                br.GradientStops.Add(new GradientStop(new HsvColor(hsvColor.H, 1, hsvColor.V).ToRgb(), 1));

                return br;
            }

            public override HsvColor GetColorByColorDetailsMarkerCoord(HsvColor hsvColor, Point coord)
            {
                return new HsvColor(hsvColor.A, coord.X * HsvColor.MaxHueValue, hsvColor.S, coord.Y);
            }

            public override HsvColor GetColorByChannelLevelValue(HsvColor hsvColor, double value)
            {
                return new HsvColor(hsvColor.A, hsvColor.H, value, hsvColor.V);
            }

            #endregion
        }

        class HsbBColorSelector : HsbColorSelector
        {
            #region Methods

            public override Point GetColorDetailsMarkerCoord(HsvColor hsvColor)
            {
                return new Point(hsvColor.H / HsvColor.MaxHueValue, hsvColor.S);
            }

            public override Brush GetColorDetailsBrush(HsvColor hsvColor)
            {
                GeometryDrawing geomDrawing1 = new GeometryDrawing();
                geomDrawing1.Geometry = new RectangleGeometry(new Rect(0, 0, 100, 100));
                geomDrawing1.Brush = ColorUtilities.GetHueBrash(1.0, hsvColor.V);
                ((LinearGradientBrush)geomDrawing1.Brush).EndPoint = new Point(1, 0);

                GeometryDrawing geomDrawing2 = new GeometryDrawing();
                geomDrawing2.Geometry = new RectangleGeometry(new Rect(0, 0, 100, 100));
                Color zeroSat = new HsvColor(0.0, 0.0, hsvColor.V).ToRgb();
                geomDrawing2.Brush = new LinearGradientBrush(zeroSat, Color.FromScRgb(0, zeroSat.ScR, zeroSat.ScG, zeroSat.ScB), new Point(0.5, 1), new Point(0.5, 0));

                DrawingGroup rootDrawingGroup = new DrawingGroup();
                rootDrawingGroup.Children.Add(geomDrawing1);
                rootDrawingGroup.Children.Add(geomDrawing2);

                DrawingBrush br = new DrawingBrush(rootDrawingGroup);
                br.Freeze();

                return br;
            }

            public override double GetChanellLevelValue(HsvColor hsvColor)
            {
                return hsvColor.V;
            }

            public override Brush GetChanellLevelBrush(HsvColor hsvColor)
            {
                LinearGradientBrush br = new LinearGradientBrush();
                br.StartPoint = new Point(0, 1);
                br.EndPoint = new Point(0, 0);

                br.GradientStops.Add(new GradientStop(Colors.Black, 0));
                br.GradientStops.Add(new GradientStop(new HsvColor(hsvColor.H, hsvColor.S, 1).ToRgb(), 1));

                return br;
            }

            public override HsvColor GetColorByColorDetailsMarkerCoord(HsvColor hsvColor, Point coord)
            {
                return new HsvColor(hsvColor.A, coord.X * HsvColor.MaxHueValue, coord.Y, hsvColor.V);
            }

            public override HsvColor GetColorByChannelLevelValue(HsvColor hsvColor, double value)
            {
                return new HsvColor(hsvColor.A, hsvColor.H, hsvColor.S, value);
            }

            #endregion
        }

        #endregion
    }
}
