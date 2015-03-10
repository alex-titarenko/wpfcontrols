# WPF Controls
[![Build status](https://ci.appveyor.com/api/projects/status/lfao253dbnmpv3pw?svg=true)](https://ci.appveyor.com/project/T-Alex/wpfcontrols)
[![NuGet Version](http://img.shields.io/nuget/v/TAlex.WPF.Controls.svg?style=flat)](https://www.nuget.org/packages/TAlex.WPF.Controls/) [![NuGet Downloads](http://img.shields.io/nuget/dt/TAlex.WPF.Controls.svg?style=flat)](https://www.nuget.org/packages/TAlex.WPF.Controls/)

Set of Controls for WPF.

## Features

#### Controls
* **ColorChip** - color picker control with advance functionality like in Photoshop.
* **ColorComboBox** - combo box with predefined list of standard html colors with ability to choose custom color.
* **HtmlRichEditor** - rich html visual editor which allow to format your text in WYSIWYG mode. Supporting inserting images, tables, font decorations and etc.
* **ImageEx** - extends standard image control to be able play animation from sets of usual images like png.
* **NumericUpDown** - control provides a TextBox with button spinners that allow incrementing and decrementing numeric values by using the spinner buttons, keyboard up/down arrows, or mouse wheel.
* **BusyIndicator** - helps to notify user when your application is busy with appropriate indicator.

#### Converters
* **BooleanToVisibilityConverter** - converts boolean value to Visibility and vice versa.
* **CamelTextToRegularTextConverter** - converts camel case text like *"HelloWorld"* to regual text like *"Hello World"*
* **ColorToBrushConverter** - converts color to brush.
* **DoubleToStringConverter** - converts double value to string and vice versa.
* **EnumToBooleanConverter** - converts any Enum value to boolean and vice verse.
* **Int32ToDecimalConverter** - converts Int32 numeric value to it's equivalent decimal value.
* **Int32ToDoubleConverter** - converts Int32 numeric value to it's equivalent double value.
* **IsNotNullToBooleanConverter** - converts any not nullable object to true and vice versa.
* **NotEmptyStringToBooleanConverter** - converts any not empty string to true and vice versa.


## Get it on NuGet!

    Install-Package TAlex.WPF.Controls

## License
WPFControls is under the [MIT license](LICENSE.md).
